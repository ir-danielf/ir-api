/**************************************************
* Arquivo: ChargeBack.cs
* Gerado: 04/07/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IRLib
{

    public class ChargeBack : ChargeBack_B
    {
        /*
         * STATUS
         *  I - carta importada (ainda não processado)
         *  A - CB associado
         *  C - CB associado a partir da lista de pendências
         *  N - CB não encontrado
         *  X - CB não encontrado a partir da lista de pendências    
         *  P - CB Análise Pendente
         */

        public ChargeBack() { }

        public ChargeBack(int usuarioIDLogado) : base(usuarioIDLogado) { }

        #region "Importar CSV, Validar, Gravar no bd"

        public void LerCSV(string CaminhoArquivo)
        {
            var content = File.ReadAllLines(CaminhoArquivo);
            var registro = new List<EstruturaCarta>();

            for (int i = 1; i < content.Length; i++)
            {
                registro.Add(new EstruturaCarta(content[i]));
            }

            try
            {
                foreach (EstruturaCarta item in registro)
                {
                    if (!string.IsNullOrEmpty(item.Cartao.Trim()) || !string.IsNullOrEmpty(item.Cv.Trim()))
                    {
                        if (ValidarRegistro(item.Cv, item.DataCompra, item.ValorTransacao, item.NroCartao))
                        {
                            if (item.Cartao.Length >= 14) { item.Cartao = HashCartao(item.Cartao); }
                            this.Inserir(item);
                            this.StatusRecemImportado();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void Inserir(EstruturaCarta carta)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tChargeBack (NumeroProcesso, NroCartao, Cartao, Cv, DataCompra, DataCarta, ValorChargeBack, ValorTransacao, AcaoTomadaAF, Origem) VALUES(@001, @010, @002, @003, @004, @005, @006, @007, @008, @009)");
                sql.Replace("@001", "'" + carta.NumeroProcesso + "'");
                sql.Replace("@002", "'" + carta.Cartao + "'");
                sql.Replace("@003", "'" + carta.Cv + "'");
                sql.Replace("@004", "'" + carta.DataCompra + "'");
                sql.Replace("@005", "'" + carta.DataCarta + "'");
                sql.Replace("@006", carta.ValorChargeBack.Replace(',', '.'));
                sql.Replace("@007", carta.ValorTransacao.Replace(',', '.'));
                sql.Replace("@008", "'" + carta.AcaoTomadaAF + "'");
                sql.Replace("@009", "'" + carta.Origem + "'");
                sql.Replace("@010", "'" + carta.NroCartao + "'");

                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void StatusRecemImportado()
        {
            try
            {
                string sqlStatus = @"UPDATE tChargeBack SET Status = 'I' WHERE Status IS NULL";
                bd.Executar(sqlStatus);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidarRegistro(string cv, string dataCompra, string ValorTransacao, string NroCartao)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                int r = 0;
                sql.Append("SELECT COUNT(ID) FROM tChargeBack WHERE CV = @Cv AND CV <> '' AND DataCompra = @DataCompra AND ValorTransacao = @ValorTransacao AND NroCartao = @NroCartao");
                sql.Replace("@Cv", "'" + cv + "'");
                sql.Replace("@DataCompra", "'" + dataCompra + "'");
                sql.Replace("@ValorTransacao", ValorTransacao.Replace(',', '.'));
                sql.Replace("@NroCartao", "'" + NroCartao + "'");
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    r = bd.LerInt(0);
                }

                if (r > 0) { return false; }
                else return true;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string HashCartao(string numCartao)
        {
            try
            {
                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(numCartao);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    numCartao = Convert.ToBase64String(hash);
                }
                return numCartao;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ResultadoImportacao()
        {
            try
            {
                int r = 0;
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT COUNT(ID) FROM tChargeBack (NOLOCK) WHERE Status = 'I'");
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    r = bd.LerInt(0);
                }
                return r;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region "Cruzar informações dos registros importados"

        public void AtualizarRelatorio()
        {
            try
            {
                DeterminarPendentes();
                CompletarID();
                QtdeIngressos();
                DataApresentacao();
                DataCancelamento();
                DataImpressao();
                MotivoCancelamento();
                StatusChargeBack();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void DeterminarPendentes()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT
	                            tChargeBack.ID, tVendaBilheteriaFormaPagamento.NSUHost, COUNT(tVendaBilheteriaFormaPagamento.NSUHost) AS Possibilidades
                            INTO #maybe
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.NSUHost = tChargeBack.CV AND CV <> ''
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            LEFT JOIN tCartao (NOLOCK) ON tCartao.NroCartao = tChargeBack.NroCartao
                            WHERE tChargeBack.Status NOT IN ('A','X','P') AND tVendaBilheteria.DataVenda LIKE '%' + tChargeBack.DataCompra + '%'
                            GROUP BY tChargeBack.ID, tVendaBilheteriaFormaPagamento.NSUHost
                            HAVING COUNT(tVendaBilheteriaFormaPagamento.NSUHost) > 1

                            UPDATE tChargeBack SET Status = 'P' FROM #maybe WHERE #maybe.ID = tChargeBack.ID

                            DROP TABLE #maybe");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CompletarID()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT
	                            tVendaBilheteriaFormaPagamento.NSUHost, tBandeira.Nome AS Bandeira ,tVendaBilheteria.ID AS VendaBilheteriaID,
	                            tCaixa.UsuarioID, tCaixa.LojaID, CaixaID, tLoja.CanalID, tVendaBilheteria.ClienteID ,Senha, ValorTotal,
	                            tVendaBilheteria.Fraude, tVendaBilheteria.AprovacaoAutomatica, VendaCancelada, tEntregaControle.EntregaID, tEntregaControle.EntregaAreaID AS AreaID
                            INTO #cb
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.NSUHost = tChargeBack.CV AND tChargeBack.CV <> ''
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                            INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
                            INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tVendaBilheteriaFormaPagamento.FormaPagamentoID
                            INNER JOIN tBandeira (NOLOCK) ON tBandeira.ID = tFormaPagamento.BandeiraID
                            LEFT JOIN tCartao (NOLOCK) ON tCartao.ClienteID = tVendaBilheteria.ClienteID
                            LEFT JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tVendaBilheteria.EntregaControleID
                            LEFT JOIN tEntrega (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
                            WHERE tChargeBack.Status IN ('I','C','N') AND tVendaBilheteria.DataVenda LIKE tChargeBack.DataCompra + '%' AND tVendaBilheteria.ValorTotal = tChargeBack.ValorTransacao

                            UPDATE tChargeBack SET SenhaCompra = Senha,
	                            ValorCompra = ValorTotal, 
	                            tChargeBack.Fraude = #cb.Fraude, 
	                            Cancelado = VendaCancelada,
	                            tChargeBack.AprovacaoAutomatica = #cb.AprovacaoAutomatica,
	                            tChargeBack.UsuarioID = #cb.UsuarioID,
	                            tChargeBack.LojaID = #cb.LojaID,
	                            tChargeBack.CanalID = #cb.CanalID,
	                            tChargeBack.ClienteID = #cb.ClienteID,
	                            tChargeBack.EntregaID = #cb.EntregaID,
	                            tChargeBack.VendaBilheteriaID = #cb.VendaBilheteriaID,
	                            tChargeBack.AreaID = #cb.AreaID,
	                            tChargeBack.Bandeira = #cb.Bandeira,
	                            tChargeBack.Status = 'A'
                            FROM #cb
                            WHERE tChargeBack.Cv = #cb.NSUHost AND tChargeBack.ValorTransacao = #cb.ValorTotal
                            DROP TABLE #cb");
                bd.Executar(sql);

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DataApresentacao()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT 
									tChargeBack.VendaBilheteriaID, MAX(tApresentacao.Horario) AS Horario 
								INTO #hora 
								FROM tIngressoLog (NOLOCK)
								INNER JOIN tChargeBack (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tChargeBack.VendaBilheteriaID
								INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID
								INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
								GROUP BY tChargeBack.VendaBilheteriaID


								UPDATE tChargeBack SET MaiorDataApresentacao = #hora.Horario FROM #hora
								WHERE tChargeBack.VendaBilheteriaID = #hora.VendaBilheteriaID

								DROP TABLE #hora");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DataCancelamento()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT DISTINCT
									SenhaCompra, tChargeBack.VendaBilheteriaID AS VendaBilheteriaID, IngressoID 
								INTO  #vbID 
								FROM tChargeBack (NOLOCK)
								INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tChargeBack.VendaBilheteriaID

								SELECT 
								#vbID.VendaBilheteriaID, MAX(TimeStamp) AS MaiorData 
								INTO #Mdata 
								FROM tIngressoLog (NOLOCK)
								INNER JOIN #vbID ON #vbID.IngressoID = tIngressoLog.IngressoID AND Acao = 'C' AND tIngressoLog.VendaBilheteriaID > #vbID.VendaBilheteriaID
								GROUP BY #vbID.VendaBilheteriaID

								UPDATE tChargeBack SET DataCancelamento = #Mdata.MaiorData FROM #Mdata
								WHERE tChargeBack.VendaBilheteriaID = #Mdata.VendaBilheteriaID

								DROP TABLE #vbID
								DROP TABLE #Mdata");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DataImpressao()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT DISTINCT
									tChargeBack.VendaBilheteriaID, MAX(tIngressoLog.TimeStamp) AS Impresso
								INTO #print
								FROM tIngressoLog
								INNER JOIN tChargeBack (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tChargeBack.VendaBilheteriaID
								WHERE Acao = 'I'
								GROUP BY tChargeBack.VendaBilheteriaID


								UPDATE tChargeBack SET MaiorDataImpressao = #print.Impresso FROM #print
								WHERE tChargeBack.VendaBilheteriaID = #print.VendaBilheteriaID

								DROP TABLE #print");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void QtdeIngressos()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT 
									SenhaCompra, COUNT(tIngressoLog.ID) AS Qtde 
								INTO #passwd 
								FROM tChargeBack 
								INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tChargeBack.VendaBilheteriaID
								WHERE Acao = 'V'
								GROUP BY SenhaCompra

								UPDATE 	tChargeBack SET QuantidadeIngressos = Qtde FROM #passwd
								WHERE tChargeBack.SenhaCompra = #passwd.SenhaCompra

								DROP TABLE #passwd");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void MotivoCancelamento()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT
	                            tChargeBack.SenhaCompra ,Motivo
                            INTO #motivo
                            FROM tVendaBilheteria (NOLOCK)
                            INNER JOIN tChargeBack (NOLOCK) ON tChargeBack.VendaBilheteriaID = tVendaBilheteria.ID
                            INNER JOIN tMotivo (NOLOCK) ON tMotivo.ID = tVendaBilheteria.MotivoID
                            WHERE Cancelado = 'T' AND tMotivo.Tipo IN ('C','F','N')

                            UPDATE tChargeBack SET MotivoCancelamento = #motivo.Motivo
                            FROM #motivo
                            WHERE tChargeBack.SenhaCompra = #motivo.SenhaCompra

                            DROP TABLE #motivo");
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void StatusChargeBack()
        {

            // FIX-ME
            try
            {
                //string associado = @"UPDATE tChargeBack SET Status = 'A' WHERE VendaBilheteriaID IS NOT NULL AND SenhaCompra IS NOT NULL AND Status <> 'P'";
                string naoAssociado = @"UPDATE tChargeBack SET Status = 'N' WHERE VendaBilheteriaID IS NULL AND Status <> 'P' AND Status <> 'X'";

                //bd.Executar(associado);
                bd.Executar(naoAssociado);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        private string FiltrarOrigem(string origem)
        {
            switch (origem)
            {
                case null:
                    return null;
                case "Todos":
                    return null;
                default:
                    return " AND Origem LIKE '" + origem + "'";
            }
        }

        public int ContarAssociadas(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT COUNT(ID) FROM tChargeBack (NOLOCK) WHERE Status IN ('A','C')");
                sql.Append(this.FiltrarOrigem(origem));

                int r = 0;

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    r = bd.LerInt(0);
                }

                return r;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ContarNaoAssociadas(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                int r = 0;
                sql.Append(@"SELECT COUNT(ID) FROM tChargeBack (NOLOCK) WHERE Status IN ('N','X')");
                sql.Append(this.FiltrarOrigem(origem));

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    r = bd.LerInt(0);
                }

                return r;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ContarPendentes(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                int r = 0;
                sql.Append(@"SELECT COUNT(ID) FROM tChargeBack (NOLOCK) WHERE Status = 'P'");
                sql.Append(this.FiltrarOrigem(origem));

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    r = bd.LerInt(0);
                }

                return r;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void EditarCarta(int id, EstruturaCarta carta)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"UPDATE tChargeBack SET
									NumeroProcesso = @001,
									Cartao = @002,
									Cv = @003,
									DataCompra = @004,
									DataCarta = @005,
									ValorChargeBack = @006,
									ValorTransacao = @007,
									AcaoTomadaAF = @008
									WHERE ID = @000");

                sql.Replace("@000", "'" + id + "'");
                sql.Replace("@001", "'" + carta.NumeroProcesso + "'");
                sql.Replace("@002", "'" + carta.Cartao + "'");
                sql.Replace("@003", "'" + carta.Cv + "'");
                sql.Replace("@004", "'" + carta.DataCompra + "'");
                sql.Replace("@005", "'" + carta.DataCarta + "'");
                sql.Replace("@006", carta.ValorChargeBack.Replace(',', '.'));
                sql.Replace("@007", carta.ValorTransacao.Replace(',', '.'));
                sql.Replace("@008", "'" + carta.AcaoTomadaAF + "'");
                bd.Executar(sql);

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AssociarChargeBack(string passwd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT
	                            tVendaBilheteriaFormaPagamento.NSUHost, tBandeira.Nome AS Bandeira, tVendaBilheteria.ID AS VendaBilheteriaID,
	                            tCaixa.UsuarioID, tCaixa.LojaID, CaixaID, tLoja.CanalID, tVendaBilheteria.ClienteID ,Senha, ValorTotal,
	                            tVendaBilheteria.Fraude, tVendaBilheteria.AprovacaoAutomatica, VendaCancelada, tEntregaControle.EntregaID, tEntregaControle.EntregaAreaID AS AreaID
                            INTO #cb
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.NSUHost = tChargeBack.CV AND tChargeBack.CV <> ''
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                            INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
                            INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tVendaBilheteriaFormaPagamento.FormaPagamentoID
                            INNER JOIN tBandeira (NOLOCK) ON tBandeira.ID = tFormaPagamento.BandeiraID
                            LEFT JOIN tCartao (NOLOCK) ON tCartao.ClienteID = tVendaBilheteria.ClienteID
                            LEFT JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tVendaBilheteria.EntregaControleID
                            LEFT JOIN tEntrega (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
                            WHERE tVendaBilheteria.Senha = '@001'


                            UPDATE tChargeBack SET SenhaCompra = Senha,
	                            ValorCompra = ValorTotal, 
	                            tChargeBack.Fraude = #cb.Fraude, 
	                            Cancelado = VendaCancelada,
	                            tChargeBack.AprovacaoAutomatica = #cb.AprovacaoAutomatica,
	                            tChargeBack.UsuarioID = #cb.UsuarioID,
	                            tChargeBack.LojaID = #cb.LojaID,
	                            tChargeBack.CanalID = #cb.CanalID,
	                            tChargeBack.ClienteID = #cb.ClienteID,
	                            tChargeBack.EntregaID = #cb.EntregaID,
	                            tChargeBack.VendaBilheteriaID = #cb.VendaBilheteriaID,
	                            tChargeBack.AreaID = #cb.AreaID,
	                            tChargeBack.Bandeira = #cb.Bandeira,
                                tChargeBack.Status = 'A'
                            FROM #cb
                            WHERE tChargeBack.Cv = #cb.NSUHost

                            DROP TABLE #cb");
                sql.Replace("@001", passwd);
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void MarcarNaoEncontrado(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"UPDATE tChargeBack SET Status = 'X' WHERE ID = @001");
                sql.Replace("@001", id.ToString());
                bd.Executar(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaCarta DetalheCarta(int id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                var carta = new EstruturaCarta();
                sql.Append(@"SELECT ID, NumeroProcesso, Cartao, Cv, DataCompra, DataCarta, ValorChargeback, ValorTransacao, AcaoTomadaAF FROM tChargeBack WHERE ID = @ID");
                sql.Replace("@ID", id.ToString());
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    carta.CartaID = bd.LerInt("ID");
                    carta.NumeroProcesso = bd.LerString("NumeroProcesso");
                    carta.Cartao = bd.LerString("Cartao");
                    carta.Cv = bd.LerString("Cv");
                    carta.DataCompra = bd.LerString("DataCompra");
                    carta.DataCarta = bd.LerString("DataCarta");
                    carta.ValorChargeBack = bd.LerString("ValorChargeBack");
                    carta.ValorTransacao = bd.LerString("ValorTransacao");
                    carta.AcaoTomadaAF = bd.LerString("AcaoTomadaAF");
                }

                return carta;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<string> ListaOrigem()
        {
            try
            {
                List<string> ListaRetorno = new List<string>();
                StringBuilder sql = new StringBuilder(@"SELECT DISTINCT Origem FROM tChargeBack");
                bd.Consulta(sql);
                ListaRetorno.Add("Todos");
                while (bd.Consulta().Read())
                {
                    ListaRetorno.Add(bd.LerString("Origem"));
                }

                return ListaRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaDetalheChargeBack> ListaAssociadas(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                var ListaRetorno = new List<EstruturaDetalheChargeBack>();
                sql.Append(@"SELECT	
	                            tCliente.Nome,
	                            tCliente.StatusAtual AS StatusCliente,
	                            tChargeBack.CV,
	                            tChargeBack.NroCartao,	
	                            SUBSTRING(DataVenda,7,2) + '/' + SUBSTRING(DataVenda,5,2) + '/' + SUBSTRING(DataVenda,0,5) AS DataVenda,
	                            DataCarta,
	                            SenhaCompra,
	                            ValorCompra,
	                            tCanal.Nome AS Canal
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tChargeBack.VendaBilheteriaID
                            INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tChargeBack.CanalID
                            LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID
                            WHERE tChargeBack.Status IN ('A','C') @001");
                sql.Replace("@001", this.FiltrarOrigem(origem));
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    ListaRetorno.Add(new EstruturaDetalheChargeBack
                    {
                        Cliente = bd.LerString("Nome"),
                        StatusCliente = bd.LerString("StatusCliente"),
                        Nsuhost = bd.LerString("CV"),
                        NroCartao = bd.LerString("NroCartao"),
                        DataVenda = bd.LerString("DataVenda"),
                        DataCarta = bd.LerString("DataCarta"),
                        Senha = bd.LerString("SenhaCompra"),
                        ValorTotal = bd.LerString("ValorCompra"),
                        Canal = bd.LerString("Canal")
                    });
                }

                return ListaRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaCarta> ListaNaoAssociadas(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                var ListaRetorno = new List<EstruturaCarta>();
                sql.Append(@"SELECT ID, NumeroProcesso, Cartao, Cv, DataCompra, DataCarta, ValorChargeback, ValorTransacao,AcaoTomadaAF FROM tChargeBack WHERE Status IN ('N','X') @001");
                sql.Replace("@001", this.FiltrarOrigem(origem));
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    ListaRetorno.Add(new EstruturaCarta
                    {
                        CartaID = bd.LerInt("ID"),
                        NumeroProcesso = bd.LerString("NumeroProcesso"),
                        Cartao = bd.LerString("Cartao"),
                        Cv = bd.LerString("Cv"),
                        DataCompra = bd.LerString("DataCompra"),
                        DataCarta = bd.LerString("DataCarta"),
                        ValorChargeBack = bd.LerString("ValorChargeBack"),
                        ValorTransacao = bd.LerString("ValorTransacao"),
                        AcaoTomadaAF = bd.LerString("AcaoTomadaAF")
                    });
                }

                return ListaRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaCarta> ListaPendencias(string origem)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                var ListaRetorno = new List<EstruturaCarta>();
                sql.Append(@"SELECT
	                            tChargeBack.ID, NumeroProcesso, NroCartao, Cv,
	                            DataCompra, ValorTransacao,  COUNT(tChargeBack.ID) AS Possibilidades
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.NSUHost = tChargeBack.CV AND CV <> ''
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            WHERE tChargeBack.Status <> 'A' AND tChargeBack.Status <> 'X' AND tVendaBilheteria.DataVenda LIKE tChargeBack.DataCompra + '%' @001
                            GROUP BY tChargeBack.ID, NumeroProcesso, NroCartao, Cv, DataCompra, ValorTransacao");
                sql.Replace("@001", this.FiltrarOrigem(origem));
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    ListaRetorno.Add(new EstruturaCarta
                    {
                        CartaID = bd.LerInt("ID"),
                        NumeroProcesso = bd.LerString("NumeroProcesso"),
                        Cartao = bd.LerString("NroCartao"),
                        Cv = bd.LerString("Cv"),
                        DataCompra = bd.LerString("DataCompra"),
                        ValorTransacao = bd.LerString("ValorTransacao"),
                        Possibilidades = bd.LerString("Possibilidades")
                    });
                }
                return ListaRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaDetalheChargeBack> ListaPossibilidades(string cv)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                var ListaRetorno = new List<EstruturaDetalheChargeBack>();

                sql.Append(@"SELECT
                                tChargeBack.ID,
	                            tCliente.Nome,
	                            tCartao.NroCartao,
	                            SUBSTRING(DataVenda,7,2) + '/' + SUBSTRING(DataVenda,5,2) + '/' + SUBSTRING(DataVenda,0,5)+ ' '+ SUBSTRING(DataVenda, 9, 2) +':'+ SUBSTRING(DataVenda,11,2) AS DataVenda,
	                            tVendaBilheteria.Senha,
	                            ValorTotal,
                                tCanal.Nome AS Canal
                            FROM tChargeBack (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.NSUHost = tChargeBack.CV AND tChargeBack.CV <> ''
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                            INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
                            INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
                            LEFT JOIN tCartao (NOLOCK) ON tCartao.ID = tVendaBilheteriaFormaPagamento.CartaoID
                            LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID
                            WHERE tVendaBilheteriaFormaPagamento.NSUHost = @001 AND tVendaBilheteria.DataVenda LIKE tChargeBack.DataCompra + '%'");
                sql.Replace("@001", cv);
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    ListaRetorno.Add(new EstruturaDetalheChargeBack
                        {
                            id = bd.LerInt("ID"),
                            Cliente = bd.LerString("Nome"),
                            Cartao = bd.LerString("NroCartao"),
                            DataVenda = bd.LerString("DataVenda"),
                            Senha = bd.LerString("Senha"),
                            ValorTotal = bd.LerString("ValorTotal"),
                            Canal = bd.LerString("Canal")
                        });
                }

                return ListaRetorno;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class ChargeBackLista : ChargeBackLista_B
    {
        public ChargeBackLista() { }

        public ChargeBackLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}