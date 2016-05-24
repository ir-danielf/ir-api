/**************************************************
* Arquivo: Cor.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.Criptografia;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace IRLib.Paralela
{

    public class Cartao : Cartao_B
    {
        public Cartao() { }

        public Cartao(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public enum enumBandeira
        {
            Todos = 0,
            Visa = 2,
            Mastercard = 3,
            Diners = 4,
            Amex = 5,
            Redeshop = 9,
            Hipercard = 12,
            Aura = 13
        }

        public enum enumBandeiraMondial
        {
            Visa = 1,
            Mastercard = 2,
            Diners = 4,
            Amex = 3,
            Hipercard = 5,
        }

        public enum enumStatusCartao
        {
            Bloqueado = 'B',
            Liberado = 'L'
        }

        public int Registros { get; set; }
        public int BuscaTipo { get; set; }
        public string BuscaTexto { get; set; }
        public bool Reiniciar { get; set; }
        public int IndexAtual { get; set; }
        public int Bandeira { get; set; }
        public int QuantidadePorPagina { get; set; }
        public bool SemPaginacao { get; set; }
        public string Ordem { get; set; }


        public int InserirCartao(string numeroCartao, int formaPagamentoID, int clienteID, string NomeCartao)
        {
            return this.InserirCartao(numeroCartao, string.Empty, string.Empty, formaPagamentoID, clienteID, NomeCartao);
        }

        public int InserirCartao(string numeroCartao, string cvv2, string dataValidade, int formaPagamentoID, int clienteID, string NomeCartao)
        {
            string numeroCartaoC = string.Empty;
            int bandeiraID = 0;
            int newCartaoID = 0;

            try
            {
                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(numeroCartao);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    numeroCartaoC = Convert.ToBase64String(hash);
                }

                string numeroCartaoMasc = string.Empty;

                //Máscara NumCartao;
                numeroCartaoMasc = numeroCartao.Substring(0, 6) + "XXXXXX" + numeroCartao.Substring(numeroCartao.Length - 4, 4);

                bandeiraID = this.BuscaBandeira(formaPagamentoID);

                if (bandeiraID <= 0)
                    throw new Exception("Falha ao consultar a Bandeira do cartão informado.");

                this.ClienteID.Valor = clienteID;
                this.BandeiraID.Valor = bandeiraID;
                this.CheckSumCartao.Valor = numeroCartaoC;
                this.NroCartao.Valor = numeroCartaoMasc;
                this.Status.Valor = ((char)enumStatusCartao.Liberado).ToString();
                this.Ativo.Valor = true;
                this.CartaoCr.Valor = string.IsNullOrEmpty(numeroCartao) ? string.Empty : Crypto.Criptografar(numeroCartao, ConfiguracaoCriptografia.Instancia.Chaves.Cartao.Valor);
                this.CVVCr.Valor = string.IsNullOrEmpty(cvv2) ? string.Empty : Crypto.Criptografar(cvv2, ConfiguracaoCriptografia.Instancia.Chaves.CVV.Valor);
                this.DataCr.Valor = string.IsNullOrEmpty(dataValidade) ? string.Empty : Crypto.Criptografar(dataValidade, ConfiguracaoCriptografia.Instancia.Chaves.Data.Valor);
                this.NomeCartao.Valor = NomeCartao;

                this.Inserir(bd);

                return newCartaoID = this.Control.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCartao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCartao (ID, Versao , ClienteID, NroCartao, CheckSumCartao, BandeiraID, Status) ");
                sql.Append("SELECT ID, @V, ClienteID, NroCartao, CheckSumCartao, BandeiraID, Status FROM tCartao WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Desbloquear(int id)
        {
            int retorno = 0;
            try
            {
                this.Control.ID = id;

                string sqlVersion = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                string sql = "Update tCartao SET Status = '" + ((char)enumStatusCartao.Liberado).ToString() + "' WHERE ID = " + id;
                retorno = bd.Executar(sql);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao atualizar o Cartão");
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public void Desvincular(int id)
        {
            try
            {
                this.Ler(id);
                this.Ativo.Valor = false;
                this.Atualizar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GetCartaoID(int VendaBilheteriaID)
        {
            int retorno = 0;

            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT vbfp.CartaoID ");
            sql.Append("   FROM tVendaBilheteria (NOLOCK) ");
            sql.Append("  INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID  ");
            sql.Append("  WHERE tVendaBilheteria.ID =" + VendaBilheteriaID);

            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                retorno = bd.LerInt("CartaoID");
            }
            return retorno;
        }

        public bool AtualizarStatus(int cartaoID, enumStatusCartao status)
        {
            try
            {
                bool retorno = false;
                this.Ler(cartaoID);
                this.Status.Valor = ((char)status).ToString();
                retorno = this.Atualizar();
                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar o Cartão, Erro:" + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizarPorVendabilheteriaID(int VendaBilheteriaID, enumStatusCartao status)
        {
            int cartaoID = 0;
            try
            {
                cartaoID = GetCartaoID(VendaBilheteriaID);

                this.Ler(cartaoID);
                this.Status.Valor = ((char)status).ToString();
                this.Atualizar();

            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar o Cartão. Erro: " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }
        public void AtualizarPorVendabilheteriaID(int VendaBilheteriaID, enumStatusCartao status, BD bd)
        {
            int cartaoID = 0;
            try
            {
                cartaoID = GetCartaoID(VendaBilheteriaID);

                this.Ler(cartaoID);
                this.Status.Valor = ((char)status).ToString();
                this.Atualizar(bd);

            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar o Cartão. Erro: " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;


                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCartao SET ClienteID = @001, NroCartao = '@002', CheckSumCartao = '@003', BandeiraID = @004, Status = '@005', Ativo = '@006', CartaoCr = '@007', CVVCr = '@008', DataCr = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.NroCartao.ValorBD);
                sql.Replace("@003", this.CheckSumCartao.ValorBD);
                sql.Replace("@004", this.BandeiraID.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.Ativo.ValorBD);
                sql.Replace("@007", this.CartaoCr.ValorBD);
                sql.Replace("@008", this.CVVCr.ValorBD);
                sql.Replace("@009", this.DataCr.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public int Bloquear(int id)
        {
            int retorno = 0;
            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cCartao (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                string sql = "Update tCartao SET Status = '" + ((char)enumStatusCartao.Bloqueado).ToString() + "' WHERE ID = " + id;
                retorno = bd.Executar(sql);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao atualizar o Cartão");
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        /// <summary>
        /// Método para o SISTEMA IR
        /// </summary>
        /// <param name="numeroCartao"></param>
        /// <param name="clienteID"></param>
        /// <returns></returns>
        public string[] ValidaCartao(string numeroCartao, int clienteID)
        {
            try
            {
                this.Limpar();
                string[] retorno = new string[2];
                string numeroCartaoC = "";

                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(numeroCartao);
                    byte[] hash = hashAlg.ComputeHash(pwordData);

                    numeroCartaoC = Convert.ToBase64String(hash);
                }

                string sql = @"SELECT ID, ClienteID, NroCartao, CheckSumCartao, BandeiraID, Status, IsNull(Ativo, 'T') AS Ativo,
                            IsNull(CartaoCr, '') AS CartaoCr, IsNull(CVVCr, '') AS CVVCr, IsNull(DataCr, '') AS DataCr
                            FROM tCartao (NOLOCK) WHERE CheckSumCartao = '" + numeroCartaoC + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.ClienteID.Valor = bd.LerInt("ClienteID");
                    this.NroCartao.Valor = bd.LerString("NroCartao");
                    this.CheckSumCartao.Valor = bd.LerString("CheckSumCartao");
                    this.BandeiraID.Valor = bd.LerInt("BandeiraID");
                    this.Status.Valor = bd.LerString("Status");
                    this.Ativo.Valor = bd.LerBoolean("Ativo");
                    this.CartaoCr.Valor = bd.LerString("CartaoCr");
                    this.CVVCr.Valor = bd.LerString("CVVCr");
                    this.DataCr.Valor = bd.LerString("DataCr");
                }

                bd.FecharConsulta();

                if (!string.IsNullOrEmpty(this.CheckSumCartao.Valor))
                {
                    if (this.Ativo.Valor && this.ClienteID.Valor == clienteID && this.Status.Valor == ((char)enumStatusCartao.Liberado).ToString())
                    {
                        retorno[0] = this.Control.ID.ToString();
                        retorno[1] = ((char)enumStatusCartao.Liberado).ToString();
                    }
                    else if (this.Ativo.Valor && this.ClienteID.Valor != clienteID && this.Status.Valor == ((char)enumStatusCartao.Liberado).ToString())
                    {
                        retorno[0] = this.Control.ID.ToString();
                        retorno[1] = ((char)enumStatusCartao.Bloqueado).ToString();
                    }
                    else if (!this.Ativo.Valor && this.Status.Valor == ((char)enumStatusCartao.Liberado).ToString())
                    {
                        this.ClienteID.Valor = clienteID;
                        this.Ativo.Valor = true;
                        this.Atualizar();
                        retorno[0] = this.Control.ID.ToString();
                        retorno[1] = this.Status.Valor;
                    }
                    else if (this.Status.Valor != ((char)enumStatusCartao.Liberado).ToString())
                        throw new Exception("Problemas com o cartão, entre em contato com a Ingresso Rápido.");
                }
                else
                {
                    retorno[0] = "0";
                    retorno[1] = ((char)enumStatusCartao.Liberado).ToString();
                }

                bd.FecharConsulta();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscaBandeira(int formaPagamentoID)
        {
            int retorno = 0;
            StringBuilder sql = new StringBuilder();

            sql.Append("Select BandeiraID from tFormaPagamento (NOLOCK) WHERE ID = " + formaPagamentoID + "");

            retorno = Convert.ToInt32(bd.ConsultaValor(sql));

            return retorno;
        }

        public List<EstruturaBlackListCartao> ListaCartoes()
        {
            try
            {
                List<EstruturaBlackListCartao> lista = new List<EstruturaBlackListCartao>();
                EstruturaBlackListCartao item;

                StringBuilder sql = new StringBuilder();
                string campo = string.Empty;
                string buscaTexto = this.BuscaTexto;

                if (this.Ordem == "C")
                    this.Ordem = "ASC";
                else
                    this.Ordem = "DESC";

                sql.Append(" WITH tbGeral AS( 	 ");
                sql.Append(" SELECT tBandeira.Nome Bandeira ,tCartao.ID, CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = ''  ");
                sql.Append(" THEN  tCliente.Nome ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS ClienteNome, ");
                sql.Append(" NroCartao, CheckSumCartao, BandeiraID, Status ");
                sql.Append(" FROM tCartao (NOLOCK) ");
                sql.Append(" LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tCartao.ClienteID ");
                sql.Append(" LEFT JOIN tBandeira (NOLOCK) ON tCartao.BandeiraID = tBandeira.ID ");
                sql.Append(" WHERE Status = '" + ((char)enumStatusCartao.Bloqueado).ToString() + "' ");

                if (this.BuscaTexto != null)
                {
                    string cartaoCripto = string.Empty;
                    if (this.BuscaTexto.Length > 0 && this.BuscaTexto.Length >= 16)
                    {
                        using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                        {
                            byte[] pwordData = Encoding.Default.GetBytes(this.BuscaTexto.Replace("'", ""));
                            byte[] hash = hashAlg.ComputeHash(pwordData);
                            cartaoCripto = Convert.ToBase64String(hash);
                        }
                        sql.Append("AND tCartao.CheckSumCartao = '" + cartaoCripto + "' ");
                    }
                    else if (this.BuscaTexto.Length > 6 && this.BuscaTexto.Length < 16)
                    {
                        sql.Append("AND tCartao.NroCartao LIKE '" + this.BuscaTexto.Replace("'", "").Substring(0, 6) + "%'");
                        sql.Append("AND tCartao.NroCartao LIKE '%" + this.BuscaTexto.Replace("'", "").Substring(6, this.BuscaTexto.Length - 6) + "'");
                    }
                    else if (this.BuscaTexto.Length == 6)
                        sql.Append("AND tCartao.NroCartao LIKE '" + this.BuscaTexto.Replace("'", "").Substring(0, 6) + "%'");

                    //if (!string.IsNullOrEmpty(buscaTexto))
                    //    sql.Append(" AND NroCartao LIKE '%" + buscaTexto + "%'");
                }
                if (this.Bandeira > 0)
                    sql.Append(" AND BandeiraID = " + this.Bandeira + "");

                sql.Append(" ) ");
                sql.Append(" 	,tbCount AS( ");
                sql.Append(" 				 	SELECT Count(ID) as Registros FROM tbGeral ");
                sql.Append(" 							) ");
                sql.Append(" 	, tbOrdenada AS ");
                sql.Append(" 				( 	 ");
                sql.Append(" 					SELECT Bandeira, ID,	ClienteNome, NroCartao, CheckSumCartao, BandeiraID, Status  ");
                sql.Append(" 					 ,ROW_NUMBER() OVER (ORDER BY ClienteNome) AS 'RowNumber' FROM tbGeral ");
                sql.Append(" 				)  ");
                sql.Append("  ");
                sql.Append(" SELECT Bandeira,ID, ClienteNome, NroCartao, CheckSumCartao, BandeiraID, Status , ");
                sql.Append(" 		RowNumber,  ");
                sql.Append(" 		Registros  ");
                sql.Append(" FROM tbOrdenada, tbCount  ");

                if (!SemPaginacao)
                    sql.Append(" WHERE RowNumber between " + this.IndexAtual + " and " + (this.IndexAtual + this.QuantidadePorPagina - 1) + "  ");

                sql.Append(" ORDER BY NroCartao  " + this.Ordem + "");

                bd.Consulta(sql.ToString());
                while (bd.Consulta().Read())
                {
                    string imgSrc = string.Empty;
                    item = new EstruturaBlackListCartao();
                    EstruturaTransacoesResultadoLista oEstrutura = new EstruturaTransacoesResultadoLista();
                    string[] bandeiraC = oEstrutura.Bandeiras(bd.LerInt("BandeiraID"));
                    item.imgSrc = bandeiraC[0];
                    item.ID = bd.LerInt("ID");
                    item.ClienteNome = bd.LerString("ClienteNome");
                    item.NroCartao = bd.LerString("NroCartao");
                    item.CheckSumCartao = bd.LerString("CheckSumCartao");
                    item.BandeiraID = bd.LerInt("BandeiraID");
                    item.Bandeira = bd.LerString("Bandeira");
                    item.Status = bd.LerString("Status");

                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");

                    lista.Add(item);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public bool BuscaCartao(string checkSumCartao)
        {
            bool retorno = false;

            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT ID FROM tCartao (NOLOCK) WHERE CheckSumCartao ='" + checkSumCartao + "' ");

            retorno = Convert.ToBoolean(bd.ConsultaValor(sql.ToString()));

            return retorno;
        }

        public string getAntiFraudeMsgCartaoUtilizado()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AntiFraudeMsgCartaoUtilizadoOutroCliente"];
        }

        public string getAntiFraudeMsgCartaoBloqueado()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AntiFraudeMsgCartaoBloqueado"];
        }

        public string getNumeroMascaradoBySenha(string senha)
        {
            try
            {
                string retorno = string.Empty;

                StringBuilder sql = new StringBuilder();

                sql.Append("     SELECT NroCartao ");
                sql.Append("       FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK)");
                sql.Append(" INNER JOIN tVendaBilheteria vb (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID ");
                sql.Append(" INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                sql.Append("      WHERE vb.Senha = '" + senha + "'");

                object retorno1 = bd.ConsultaValor(sql);

                if (retorno1 != null)
                    retorno = retorno1.ToString();

                return retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string getNumeroMascaradoVBID(int vendaBilheteriaID)
        {
            try
            {
                string retorno = string.Empty;

                StringBuilder sql = new StringBuilder();

                sql.Append("     SELECT NroCartao ");
                sql.Append("       FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK)");
                sql.Append(" INNER JOIN tVendaBilheteria vb (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID ");
                sql.Append(" INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                sql.Append("      WHERE vb.ID = " + vendaBilheteriaID);

                object retorno1 = bd.ConsultaValor(sql);

                if (retorno1 != null)
                    retorno = retorno1.ToString();

                return retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizarCriptografiaCartao(int cartaoID, string nroCartao, string dataVencimento, string cvv2, string NomeCartao)
        {
            bd.Executar(
                string.Format(@"UPDATE tCartao SET CartaoCr = '{0}', DataCr = '{1}', CVVCr = '{2}' , NomeCartao = '{3}'
                        WHERE ID = {4}",
                        string.IsNullOrEmpty(nroCartao) ? string.Empty : Criptografia.Crypto.Criptografar(nroCartao, ConfiguracaoCriptografia.Instancia.Chaves.Cartao.Valor),
                        string.IsNullOrEmpty(dataVencimento) ? string.Empty : Criptografia.Crypto.Criptografar(dataVencimento, ConfiguracaoCriptografia.Instancia.Chaves.Data.Valor),
                        string.IsNullOrEmpty(cvv2) ? string.Empty : Criptografia.Crypto.Criptografar(cvv2, ConfiguracaoCriptografia.Instancia.Chaves.CVV.Valor),
                        NomeCartao,
                        cartaoID));
        }

        /// <summary>
        /// Método que retorna o TefID a partir do bin do cartão
        /// </summary>
        /// <param name="bin">6 primeiros digitos do cartão</param>
        /// <param name="Credito">Indica se o cartão é crédito ou débito. True = Crédito / False = Débito</param>
        /// <returns></returns>
        public string ValidarBandeira(string bin, bool Credito)
        {
            string consulta = @"select * from tbandeirabin tbb inner join tbandeira tb
                            on tbb.bandeiraid = tb.id
                            where tbb.tipocartao = '" + Credito + "'";

            //Dictionary que contem a lista de bins do tipo do cartão (credito/debito)
            //e a bandeira com a qual cada bin se relaciona.
            Dictionary<string, string> Bandeiras = new Dictionary<string, string>();
            using (var reader = bd.Consulta(consulta))
            {
                while (reader.Read())
                {
                    Bandeiras.Add((string)reader["Bin"], reader["TefID"].ToString());
                }
            }

            string retorno = "-";
            string binProv = "";
            string ultimoValido = "";

            for (int i = 0; i < bin.Length; i++)
            {
                binProv += bin[i];
                if (Bandeiras.Count(x => x.Key.StartsWith(binProv)) == 1)
                {
                    retorno = Bandeiras.First(x => x.Key.StartsWith(binProv)).Value;
                    return retorno;
                }
                else
                {
                    if (Bandeiras.Count(x => x.Key.StartsWith(binProv)) == 0)
                    {
                        if (Bandeiras.ContainsKey(ultimoValido))
                        {
                            retorno = Bandeiras[ultimoValido];
                        }
                        return retorno;
                    }
                    else
                    {
                        ultimoValido = binProv;
                    }
                }
            }

            if (Bandeiras.ContainsKey(ultimoValido))
            {
                retorno = Bandeiras[ultimoValido];
            }

            return retorno;
        }
    }
}
