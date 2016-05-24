/**************************************************
* Arquivo: CanalEvento.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using IRCore.Util;
using IRLib.ClientObjects;

namespace IRLib
{

    public class CanalEvento : CanalEvento_B
    {
        public CanalEvento() { }

        public CanalEvento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        private RoboCanalEvento oRoboCanalEvento = new RoboCanalEvento();

        public bool Filme { get; set; }

        public void Ler(int canalID, int eventoID)
        {
            try
            {
                string sql = "SELECT * FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalID + " AND EventoID=" + eventoID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaMinima.ValorBD = bd.LerDecimal("TaxaMinima").ToString();
                    this.TaxaMaxima.ValorBD = bd.LerDecimal("TaxaMaxima").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> LerQtde(int empresaID, int eventoID)
        {
            try
            {
                Dictionary<string, string> retorno = new Dictionary<string, string>();
                StringBuilder sql = new StringBuilder();


                sql.Append("ListaQuantidadesCanaisIR @EventoID = " + eventoID);
                if (empresaID > 0)
                {
                    sql.Append(", @EmpresaID = " + empresaID);
                }

                if (bd.Consulta(sql.ToString()).Read())
                {
                    retorno.Add("QuantidadeCallCenter", bd.LerInt("QuantidadeCallCenter").ToString());
                    retorno.Add("QuantidadeCallCenterTotal", bd.LerInt("QuantidadeCallCenterTotal").ToString());
                    retorno.Add("QuantidadePDV", bd.LerInt("QuantidadePDV").ToString());
                    retorno.Add("QuantidadePDVTotal", bd.LerInt("QuantidadePDVTotal").ToString());
                    retorno.Add("QuantidadeInternet", bd.LerInt("QuantidadeInternet").ToString());
                    retorno.Add("QuantidadeInternetTotal", bd.LerInt("QuantidadeInternetTotal").ToString());
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataTable Eventos(int canalid)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaMinima", typeof(decimal));
                tabela.Columns.Add("TaxaMaxima", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoMinima", typeof(decimal));
                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                string sql = "SELECT tEvento.ID,tEvento.Nome,tCanalEvento.TaxaConveniencia, tCanalEvento.TaxaMinima,tCanalEvento.TaxaMaxima,tCanalEvento.TaxaComissa,tCanalEvento.ComissaoMinima,tCanalEvento.ComissaoMaxima " +
                    "FROM tEvento (NOLOCK), tCanalEvento (NOLOCK) " +
                    "WHERE tEvento.ID=tCanalEvento.EventoID AND " +
                    "tCanalEvento.CanalID=" + canalid + " " +
                    "ORDER BY tEvento.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataTable Canais(int eventoid)
        {
            try
            {
                DataTable tabela = new DataTable("CanalEvento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("CanalEventoID", typeof(int));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaMinima", typeof(decimal));
                tabela.Columns.Add("TaxaMaxima", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoMinima", typeof(decimal));
                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                string sql = "SELECT tCanal.ID,tCanal.Nome,tCanalEvento.ID AS CanalEventoID,tCanalEvento.TaxaConveniencia,tCanalEvento.TaxaMinima, tCanalEvento.TaxaMaxima, tEmpresa.Nome AS Empresa, " +
                    "tCanalEvento.TaxaComissao,tCanalEvento.ComissaoMinima,tCanalEvento.ComissaoMaxima " +
                    "FROM tCanal (NOLOCK), tCanalEvento (NOLOCK), tEmpresa (NOLOCK) " +
                    "WHERE tEmpresa.ID=tCanal.EmpresaID AND tCanal.ID=tCanalEvento.CanalID AND " +
                    "tCanalEvento.EventoID=" + eventoid + " ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Empresa"] = bd.LerString("Empresa");
                    //Campo CanalEventoID irrelevante para a tela de distribuição de Eventos nos canais
                    linha["CanalEventoID"] = 0;
                    linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> FiltroPorTipoCanal(List<int> precosIDs, EstruturaCanaisEvento.EnumTipoFiltragemCanaisEvento tipoFiltragem)
        {

            //try
            //{
            //    string precos = String.Join(",", precosIDs);

            //    string versao = ConfigurationManager.AppSettings["ConfigVersion"];
            //    string canalCallCenter = Configuracao.Get("CanaisDistribuicaoCC", versao);
            //    string canalSite = Configuracao.Get("CanaisDistribuicaoWeb", versao);

            List<int> result = new List<int>();

            //    string sql = String.Format("SELECT DISTINCT tPreco.ID " +
            //                               "FROM tPreco " +
            //                               "INNER JOIN tCanalPreco ON tPreco.ID=tCanalPreco.PrecoID " +
            //                               "INNER JOIN tCanal ON tCanal.ID=tCanalPreco.CanalID " +
            //                               "WHERE tPreco.ID IN ({0}) ", precos);

            //    switch (tipoFiltragem)
            //    {
            //        case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.PontoVenda:
            //            sql = sql + String.Format(" AND tCanal.ID NOT IN ({0}, {1})", canalCallCenter, canalSite);
            //            break;

            //        case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.CallCenter:
            //            sql = sql + String.Format(" AND tCanal.ID IN ({0})", canalCallCenter);
            //            break;

            //        case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.Internet:
            //            sql = sql + String.Format(" AND tCanal.ID IN ({0})", canalSite);
            //            break;

            //    }

            //    bd.Consulta(sql);

            //    while (bd.Consulta().Read())
            //    {
            //        result.Add(bd.LerInt("ID"));
            //    }

            return result;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    bd.Fechar();
            //}
        }

        public override int BuscaTaxaConveniencia(int canalid, int eventoid)
        {
            try
            {
                int taxaConveniencia;

                string sql = "SELECT TaxaConveniencia " +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND EventoID=" + eventoid;

                object ret = bd.ConsultaValor(sql);

                bd.Fechar();

                taxaConveniencia = (ret != null) ? (int)ret : 0;

                return taxaConveniencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BuscaTaxasConvenienciaComissao(int canalid, int eventoid)
        {
            try
            {
                DataTable taxas = new DataTable();
                taxas.Columns.Add("TaxaConveniencia", typeof(int));
                taxas.Columns.Add("TaxaMinima", typeof(decimal));
                taxas.Columns.Add("TaxaMaxima", typeof(decimal));
                taxas.Columns.Add("TaxaComissao", typeof(int));
                taxas.Columns.Add("ComissaoMinima", typeof(decimal));
                taxas.Columns.Add("ComissaoMaxima", typeof(decimal));


                string sql = "SELECT TaxaConveniencia,TaxaMinima,TaxaMaxima,TaxaComissao,ComissaoMinima,ComissaoMaxima " +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND EventoID=" + eventoid;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = taxas.NewRow();
                    linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                    taxas.Rows.Add(linha);
                }
                return taxas;
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

        public int BuscaTaxaConveniencia(int canalid, int eventoid, BD database)
        {
            try
            {
                int taxaConveniencia;

                string sql = "SELECT TaxaConveniencia " +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND EventoID=" + eventoid;

                object ret = database.ConsultaValor(sql);


                if (ret == null)
                    throw new Exception("Não foi possível reservar. O evento em questão não está distribuído para o canal que está utilizando.");
                else
                    taxaConveniencia = (int)ret;

                return taxaConveniencia;

            }
            catch (Exception ex)
            {
                database.FecharConsulta();

                throw ex;
            }

        }

        public decimal[] BuscaTaxasMinMax(int canalid, int eventoid)
        {
            try
            {
                decimal[] taxas = new decimal[2];

                string sql = "SELECT TaxaMinima,TaxaMaxima " +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND EventoID=" + eventoid;

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    taxas[0] = bd.LerDecimal("TaxaMinima");
                    taxas[1] = bd.LerDecimal("TaxaMaxima");
                }
                else
                    throw new Exception("Não foi possível reservar. O evento em questão não está distribuído para o canal que está utilizando.");


                return taxas;

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

        public decimal[] BuscaTaxasMinMaxEConveniencia(int canalid, int eventoid)
        {
            try
            {
                var taxas = new decimal[3];

                var sql = string.Format(@"SELECT
                                          TaxaMinima,
                                          TaxaMaxima,
                                          TaxaConveniencia
                                        FROM tCanalEvento ( NOLOCK )
                                        WHERE CanalID = {0} AND EventoID = {1}", canalid, eventoid);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    taxas[0] = bd.LerDecimal("TaxaMinima");
                    taxas[1] = bd.LerDecimal("TaxaMaxima");
                    taxas[2] = bd.LerDecimal("TaxaConveniencia");
                }
                else
                {
                    LogUtil.Info(string.Format("##Get.BuscaTaxasMinMaxEConveniencia.ERROR## CANAL ID: {0}, EVENTO ID: {1}, MSG: Não foram encontradas Taxa Mínima, Taxa Máxima e/ou Taxa de Conveniência nesse canal para o evento selecionado.", canalid, eventoid));
                    throw new Exception("Não foi possível calcular os valores das taxas deste evento. Por favor, tente novamente.");   
                }                    

                return taxas;
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

        public decimal[] BuscaTaxasComissaoMinMax(int canalid, int eventoid)
        {
            try
            {
                decimal[] taxas = new decimal[2];

                string sql = "SELECT ComissaoMinima,ComissaoMaxima" +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID=" + canalid + " AND EventoID=" + eventoid;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    taxas[0] = bd.LerDecimal("TaxaMinima");
                    taxas[1] = bd.LerDecimal("TaxaMaxima");
                }
                return taxas;

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

        public List<int> BuscaCanaisEventoSelecionados(DataTable canais, int eventoid)
        {
            try
            {
                List<int> canaisSelecionados = new List<int>();
                string canaisFormatados = "";

                foreach (DataRow item in canais.Rows)
                {
                    canaisFormatados += item["ID"].ToString() + ", ";
                }

                string sql = "SELECT CanalID " +
                    "FROM tCanalEvento (NOLOCK) " +
                    "WHERE CanalID IN (" + canaisFormatados.Substring(0, canaisFormatados.Length - 2) + ") AND EventoID=" + eventoid;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    canaisSelecionados.Add(bd.LerInt("CanalID"));
                }
                return canaisSelecionados;
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

        public override bool Atualizar(BD database)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCanalEvento WHERE ID=" + this.Control.ID;
                object obj = database.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", database);
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanalEvento SET CanalID = @001, EventoID = @002, TaxaConveniencia = @003, TaxaMinima = @004, TaxaMaxima = @005, TaxaComissao = @006, ComissaoMinima = @007, ComissaoMaxima = @008 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.TaxaConveniencia.ValorBD);
                sql.Replace("@004", this.TaxaMinima.ValorBD);
                sql.Replace("@005", this.TaxaMaxima.ValorBD);
                sql.Replace("@006", this.TaxaComissao.ValorBD);
                sql.Replace("@007", this.ComissaoMinima.ValorBD);
                sql.Replace("@008", this.ComissaoMaxima.ValorBD);


                int x = database.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao, BD dataBase)
        {
            try
            {
                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCanalEvento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                dataBase.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int QuantidadeCanaisIREvento(int eventoID)
        {
            try
            {
                string sql = string.Format(@"SELECT 
                        COUNT(tCanalEvento.ID)
                        FROM tCanalEvento (NOLOCK)
                        INNER JOIN tCanal (NOLOCK) ON tCanal.ID = CanalID 
                        INNER JOIN tEmpresa (NOLOCK) ON tEmpresa.ID = EmpresaID
                        WHERE EventoID = {0}
                        AND EmpresaVende = 'T' 
                        AND EmpresaPromove = 'F'", eventoID);

                object retorno = bd.ConsultaValor(sql);

                if (retorno != null)
                    return Convert.ToInt32(retorno);
                else
                    throw new Exception("Problemas ao buscar quantidade de canais IR");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MarcaComoIRVende(int EventoID)
        {
            try
            {
                BD bd = new BD();

                bd.Consulta(string.Format(@"SELECT DISTINCT tCanal.ID
                                        FROM tEmpresa (NOLOCK), tCanal (NOLOCK)
                                        INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.CanalID=tCanal.ID
                                        WHERE EmpresaVende = 'T'
                                        AND EmpresaPromove = 'F'
                                        AND tCanal.EmpresaID = tEmpresa.ID
                                        AND tCanal.ID NOT IN(SELECT CanalID FROM tCanalEvento WHERE EventoID={0})", EventoID));

                List<int> CanaisID = new List<int>();

                while (bd.Consulta().Read())
                    CanaisID.Add(bd.LerInt("ID"));

                Canal canal = new Canal(this.Control.UsuarioID);

                foreach (int CanalID in CanaisID)
                {
                    canal.Ler(CanalID);
                    this.Limpar();
                    this.CanalID.Valor = CanalID;
                    this.EventoID.Valor = EventoID;
                    this.TaxaConveniencia.Valor = canal.TaxaConveniencia.Valor;
                    this.TaxaMinima.Valor = canal.TaxaMinima.Valor;
                    this.TaxaMaxima.Valor = canal.TaxaMaxima.Valor;
                    this.TaxaComissao.Valor = canal.TaxaComissao.Valor;
                    this.ComissaoMinima.Valor = canal.ComissaoMinima.Valor;
                    this.ComissaoMaxima.Valor = canal.ComissaoMaxima.Valor;
                    this.Inserir(false);
                }
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

        public void DesmarcaComoIRVende(int EventoID)
        {
            try
            {
                BD bd = new BD();

                List<int> CanaisID = new List<int>();
                List<int> CanaisIDDeletar = new List<int>();

                bd.Consulta(@"SELECT CanalID as ID FROM tCanalEvento (NOLOCK) where EventoID = " + EventoID + " ORDER BY ID");

                while (bd.Consulta().Read())
                    CanaisID.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                bd.Consulta(@"SELECT DISTINCT tCanal.ID FROM tEmpresa (NOLOCK) INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID WHERE EmpresaVende = 'T' AND EmpresaPromove = 'F' AND tCanal.EmpresaID = tEmpresa.ID");

                while (bd.Consulta().Read())
                    CanaisIDDeletar.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                CanalEvento canalEvento = new CanalEvento(this.Control.UsuarioID);

                bd.IniciarTransacao();

                foreach (int CanalID in CanaisID)
                {
                    canalEvento.Ler(CanalID, EventoID);
                    canalEvento.Excluir(bd, canalEvento.Control.ID);
                }

                foreach (int CanalID in CanaisIDDeletar)
                {
                    oRoboCanalEvento.UsuarioID.Valor = this.Control.UsuarioID;
                    oRoboCanalEvento.CanalID.Valor = CanalID;
                    oRoboCanalEvento.EventoID.Valor = EventoID;
                    oRoboCanalEvento.IsFilme.Valor = false;
                    oRoboCanalEvento.Operacao.Valor = Convert.ToChar(RoboCanalEvento.operacaobanco.Deleletar).ToString();
                    oRoboCanalEvento.Inserir(bd);
                }

                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Excluir(int id)
        {
            try
            {
                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalEvento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalEvento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Excluir(BD bd, int id)
        {
            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalEvento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalEvento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExcluirPorEvento(BD bd, int eventoID)
        {
            try
            {
                string sqlDelete = "DELETE FROM tCanalEvento WHERE EventoID=" + eventoID;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InserirBulk(DataTable dados, int eventoID) 
        {
            BD database = new BD();
            try
            {
                bool result = false;
                
                ExcluirPorEvento(database, eventoID);
                foreach (DataRow item in dados.Rows)
                {
                    int controle = 0;
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO tCanalEvento(CanalID, EventoID, TaxaConveniencia,TaxaMinima,TaxaMaxima,TaxaComissao,ComissaoMinima,ComissaoMaxima) ");
                    sql.Append("VALUES (@001,@002,@003,@004,@005, @006, @007, @008); SELECT SCOPE_IDENTITY();");

                    sql.Replace("@001", item["CanalID"].ToString());
                    sql.Replace("@002", item["EventoID"].ToString());
                    sql.Replace("@003", item["TaxaConveniencia"].ToString());
                    sql.Replace("@004", item["TaxaMinima"].ToString().Replace(",","."));
                    sql.Replace("@005", item["TaxaMaxima"].ToString().Replace(",", "."));
                    sql.Replace("@006", item["TaxaComissao"].ToString());
                    sql.Replace("@007", item["ComissaoMinima"].ToString().Replace(",", "."));
                    sql.Replace("@008", item["ComissaoMaxima"].ToString().Replace(",", "."));


                    object x = database.ConsultaValor(sql.ToString());
                    controle = (x != null) ? Convert.ToInt32(x) : 0;

                    result = controle > 0;

                    if (result)
                        InserirControle("I", database);

                }


                //Comentado temporariamente - Não descomentar
                //database.BulkInsert(dados, "tCanalEvento", false, false);
                
                return true;
            }
            catch (Exception ex)
            {
                database.DesfazerTransacao();
                throw ex;
            }
        }

        public bool Inserir(bool inserir)
        {
            BD database = new BD();

            try
            {
                return this.Inserir(database, inserir);
            }
            catch (Exception ex)
            {
                database.DesfazerTransacao();
                throw ex;
            }
        }

        public bool Inserir(BD database, bool inserir)
        {
            try
            {
                bool result = false;

                oRoboCanalEvento.UsuarioID.Valor = this.Control.UsuarioID;
                oRoboCanalEvento.CanalID.Valor = this.CanalID.Valor;
                oRoboCanalEvento.EventoID.Valor = this.EventoID.Valor;
                oRoboCanalEvento.IsFilme.Valor = false;


                if (oRoboCanalEvento.VerificarEventoGeradoDepois(this.CanalID.Valor) && !inserir)
                {
                    oRoboCanalEvento.Operacao.Valor = Convert.ToChar(RoboCanalEvento.operacaobanco.Inserir).ToString();
                    result = oRoboCanalEvento.Inserir(bd);
                }
                else
                {
                    oRoboCanalEvento.Operacao.Valor = Convert.ToChar(RoboCanalEvento.operacaobanco.Deleletar).ToString();
                    oRoboCanalEvento.Inserir(bd);

                    this.Control.Versao = 0;

                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO tCanalEvento(CanalID, EventoID, TaxaConveniencia,TaxaMinima,TaxaMaxima,TaxaComissao,ComissaoMinima,ComissaoMaxima) ");
                    sql.Append("VALUES (@001,@002,@003,@004,@005, @006, @007, @008); SELECT SCOPE_IDENTITY();");

                    sql.Replace("@001", this.CanalID.ValorBD);
                    sql.Replace("@002", this.EventoID.ValorBD);
                    sql.Replace("@003", this.TaxaConveniencia.ValorBD);
                    sql.Replace("@004", this.TaxaMinima.ValorBD);
                    sql.Replace("@005", this.TaxaMaxima.ValorBD);
                    sql.Replace("@006", this.TaxaComissao.ValorBD);
                    sql.Replace("@007", this.ComissaoMinima.ValorBD);
                    sql.Replace("@008", this.ComissaoMaxima.ValorBD);


                    object x = database.ConsultaValor(sql.ToString());
                    this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                    result = this.Control.ID > 0;

                    if (result)
                        InserirControle("I", database);
                    try
                    {
                        //se for canal internet envia e-mail
                        if (this.CanalID.ValorBD == Canal.CANAL_INTERNET.ToString())
                        {
                            Evento evento = new Evento();

                            string[] eventoLocalNome = evento.EventoLocalNome(this.EventoID.Valor);

                            string PARA = System.Configuration.ConfigurationManager.AppSettings["EmailDestinoGeral"];
                            string PARA_ABRIL = System.Configuration.ConfigurationManager.AppSettings["EmailDestinoAbril"];

                            foreach (string destino in PARA.Split(','))
                                ServicoEmail.EnviarInseridoCanalEventoLogistica(destino, eventoLocalNome[1], eventoLocalNome[0]);

                            foreach (string destino in PARA_ABRIL.Split(','))
                                ServicoEmail.EnviarInseridoCanalEventoLogistica(destino, eventoLocalNome[1], eventoLocalNome[0]);
                        }
                    }
                    catch
                    { }
                }
                return result;

            }
            catch (Exception ex)
            {
                database.DesfazerTransacao();
                throw ex;
            }
        }
    }

    public class CanalEventoLista : CanalEventoLista_B
    {
        public CanalEventoLista() { }

        public CanalEventoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public bool AlterarTaxas(int[] canalEventosIDs, int[] canaisIDs, int eventoID, int taxa, decimal taxaMin, decimal taxaMax, int comissao, decimal comissaoMin, decimal comissaoMax)
        {

            try
            {

                bool ok = true;

                bd.IniciarTransacao();

                for (int i = 0; i < canalEventosIDs.Length; i++)
                {
                    int canalEventoID = canalEventosIDs[i];
                    int canalID = canaisIDs[i];

                    CanalEvento.Limpar();
                    CanalEvento.Control.ID = canalEventoID;
                    CanalEvento.CanalID.Valor = canalID;
                    CanalEvento.EventoID.Valor = eventoID;
                    CanalEvento.TaxaConveniencia.Valor = taxa;
                    CanalEvento.TaxaMinima.Valor = taxaMin;
                    CanalEvento.TaxaMaxima.Valor = taxaMax;
                    CanalEvento.TaxaComissao.Valor = comissao;
                    CanalEvento.ComissaoMinima.Valor = comissaoMin;
                    CanalEvento.ComissaoMaxima.Valor = comissaoMax;

                    ok &= CanalEvento.Atualizar(bd);

                    if (!ok)
                        throw new CanalPacoteException("Não conseguiu atualizar canal " + canalID + " e evento " + eventoID);

                }

                bd.FinalizarTransacao();

                return ok;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public bool AlterarTaxas(int[] canalEventosIDs, int[] canaisIDs, int eventoID, int taxa)
        {

            try
            {

                bool ok = true;

                bd.IniciarTransacao();

                for (int i = 0; i < canalEventosIDs.Length; i++)
                {
                    int canalEventoID = canalEventosIDs[i];
                    int canalID = canaisIDs[i];

                    CanalEvento.Limpar();
                    CanalEvento.Control.ID = canalEventoID;
                    CanalEvento.CanalID.Valor = canalID;
                    CanalEvento.EventoID.Valor = eventoID;
                    CanalEvento.TaxaConveniencia.Valor = taxa;

                    ok &= CanalEvento.Atualizar(bd);

                    if (!ok)
                        throw new CanalPacoteException("Não conseguiu atualizar canal " + canalID + " e evento " + eventoID);
                }

                bd.FinalizarTransacao();

                return ok;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioCanalEvento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Evento", typeof(string));
                    tabela.Columns.Add("TaxaConveniencia", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        Evento evento = new Evento();
                        evento.Ler(canalEvento.EventoID.Valor);
                        linha["Evento"] = evento.Nome.Valor;
                        linha["TaxaConveniencia"] = canalEvento.TaxaConveniencia.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
