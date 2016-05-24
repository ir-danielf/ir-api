/**************************************************
* Arquivo: ValeIngressoTipo.cs
* Gerado: 09/11/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using IRLib.ClientObjects.Arvore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace IRLib
{

    public class ValeIngressoTipo : ValeIngressoTipo_B
    {
        public enum EnumClienteTipo
        {
            Pessoa_Juridica = 'J',
            Pessoa_Fisica = 'F'
        }
        public enum EnumAcaoCanais
        {
            Manter = 'M',
            Distribuir = 'D',
            Remover = 'R'
        }


        public enum EnumValorTipo
        {
            Valor = 'V',
            Porcentagem = 'P'
        }

        int usuarioIDLogado;
        public ValeIngressoTipo() { }
        public ValeIngressoTipo(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.usuarioIDLogado = usuarioIDLogado;
        }
        public int QuantidadeDisponivel(int valeIngressoTipoID)
        {
            try
            {
                string sql = "SELECT COUNT(ID) FROM tValeIngresso (NOLOCK) WHERE ValeIngressoTipoID = " + valeIngressoTipoID + " AND Status = '" + (char)ValeIngresso.enumStatus.Disponivel + "'";

                return Convert.ToInt32(bd.ConsultaValor(sql));
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Inserir novo(a) ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {


                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cValeIngressoTipo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tValeIngressoTipo(ID, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo,ReleaseInternet,PublicarInternet,TrocaEntrega, TrocaIngresso , TrocaConveniencia,ValorTipo,ValorPagamento) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,'@004','@005','@006','@007','@008','@009',@010,'@011','@012','@013','@014','@015','@016','@017','@018', '@019')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.ValidadeDiasImpressao.ValorBD);
                sql.Replace("@004", this.ValidadeData.ValorBD);
                sql.Replace("@005", this.ClienteTipo.ValorBD);
                sql.Replace("@006", this.ProcedimentoTroca.ValorBD);
                sql.Replace("@007", this.SaudacaoPadrao.ValorBD);
                sql.Replace("@008", this.SaudacaoNominal.ValorBD);
                sql.Replace("@009", this.QuantidadeLimitada.ValorBD);
                sql.Replace("@010", this.EmpresaID.ValorBD);
                sql.Replace("@011", this.CodigoTrocaFixo.ValorBD);
                sql.Replace("@012", this.Acumulativo.ValorBD);
                sql.Replace("@013", this.ReleaseInternet.ValorBD);
                sql.Replace("@014", this.PublicarInternet.ValorBD);
                sql.Replace("@015", this.TrocaEntrega.ValorBD);
                sql.Replace("@016", this.TrocaIngresso.ValorBD);
                sql.Replace("@017", this.TrocaConveniencia.ValorBD);
                sql.Replace("@018", this.ValorTipo.ValorBD);
                sql.Replace("@019", this.ValorPagamento.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

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


        /// <summary>
        /// Atualiza ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cValeIngressoTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tValeIngressoTipo SET Nome = '@001', Valor = '@002', ValidadeDiasImpressao = @003, ValidadeData = '@004', ClienteTipo = '@005', ProcedimentoTroca = '@006', SaudacaoPadrao = '@007', SaudacaoNominal = '@008', QuantidadeLimitada = '@009', EmpresaID = @010, CodigoTrocaFixo = '@011', Acumulativo = '@012', ReleaseInternet = '@013', PublicarInternet='@014' , TrocaEntrega='@015', TrocaIngresso='@016' , TrocaConveniencia='@017', ValorTipo='@018' , ValorPagamento='@019'");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.ValidadeDiasImpressao.ValorBD);
                sql.Replace("@004", this.ValidadeData.ValorBD);
                sql.Replace("@005", this.ClienteTipo.ValorBD);
                sql.Replace("@006", this.ProcedimentoTroca.ValorBD);
                sql.Replace("@007", this.SaudacaoPadrao.ValorBD);
                sql.Replace("@008", this.SaudacaoNominal.ValorBD);
                sql.Replace("@009", this.QuantidadeLimitada.ValorBD);
                sql.Replace("@010", this.EmpresaID.ValorBD);
                sql.Replace("@011", this.CodigoTrocaFixo.ValorBD);
                sql.Replace("@012", this.Acumulativo.ValorBD);
                sql.Replace("@013", this.ReleaseInternet.ValorBD);
                sql.Replace("@014", this.PublicarInternet.ValorBD);
                sql.Replace("@015", this.TrocaEntrega.ValorBD);
                sql.Replace("@016", this.TrocaIngresso.ValorBD);
                sql.Replace("@017", this.TrocaConveniencia.ValorBD);
                sql.Replace("@018", this.ValorTipo.ValorBD);
                sql.Replace("@019", this.ValorPagamento.ValorBD);

                int x = bd.Executar(sql.ToString());

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
        /// <summary>
        /// Atualiza ValeIngressoTipo para vender na internet
        /// </summary>
        /// <returns></returns>	
        public bool SalvarParaInternet(EstruturaValeIngressoTipo salvar)
        {

            try
            {
                bd.IniciarTransacao();
                this.Control.ID = salvar.ID;
                string sqlVersion = "SELECT MAX(Versao) FROM cValeIngressoTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tValeIngressoTipo SET ReleaseInternet = '@001', PublicarInternet='@002' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", salvar.ID.ToString());
                sql.Replace("@001", salvar.ReleaseInternet.Replace("'", "''"));
                sql.Replace("@002", salvar.PublicarInternet ? "T" : "F");


                int x = bd.Executar(sql.ToString());

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

        /// <summary>		
        /// Obter canais distribuidos da empresa e ValeIngressoTipo
        /// retorno um DataTable para ser utilizado no UltraGrid da ValeIngressoWizard
        /// kim
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisDistribuidos(int empresaID)
        {

            try
            {

                DataTable canaisDistribuidos = new DataTable();
                canaisDistribuidos.Columns.Add("CanalValeIngressoID", typeof(int));
                canaisDistribuidos.Columns.Add("CanalID", typeof(int));
                canaisDistribuidos.Columns.Add("CanalNome", typeof(string));
                canaisDistribuidos.Columns.Add("DistribuidoAtual", typeof(bool));
                canaisDistribuidos.Columns.Add("DistribuidoAtualizar", typeof(bool));
                DataRow canal;

                string sql = "SELECT ce.ID AS CanalValeIngressoID, c.ID AS CanalID, c.Nome AS CanalNome, CASE WHEN ce.ID > 0 THEN 'T' ELSE 'F' END AS Distribuido " +
                             "FROM tCanal c (NOLOCK) " +
                             "LEFT JOIN tCanalValeIngresso ce (NOLOCK) ON  ValeIngressoTipoID = " + this.Control.ID + " AND CanalID = c.ID " +
                             "WHERE c.EmpresaID = " + empresaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    canal = canaisDistribuidos.NewRow();
                    canal["CanalValeIngressoID"] = bd.LerInt("CanalValeIngressoID");
                    canal["CanalID"] = bd.LerInt("CanalID");
                    canal["CanalNome"] = bd.LerString("CanalNome");
                    canal["DistribuidoAtual"] = bd.LerBoolean("Distribuido");
                    canal["DistribuidoAtualizar"] = bd.LerBoolean("Distribuido");
                    canaisDistribuidos.Rows.Add(canal);
                }




                return canaisDistribuidos;

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
        /// <summary>
        /// Obter canais distribuidos da empresa e ValeIngressoTipo
        /// Se o o this.Control.ID for 0 traz todos os canais da empresa como se não tivessem vale ingresso nem canais distribuidos.
        /// O Control.ID = 0 é utilizado para carregar o ValeIngressoWizard com um novo Vale Ingresso.
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        public List<EstruturaCanalValeIngresso> CanaisDistribuidosLista(int empresaID)
        {

            try
            {
                List<EstruturaCanalValeIngresso> retorno = new List<EstruturaCanalValeIngresso>();
                EstruturaCanalValeIngresso canal;
                string sql;

                if (this.Control.ID > 0)
                    sql = "SELECT ce.ID AS CanalValeIngressoID, c.ID AS CanalID, c.Nome AS CanalNome, CASE WHEN ce.ID > 0 THEN 'T' ELSE 'F' END AS Distribuido " +
                                 "FROM tCanal c (NOLOCK) " +
                                 "LEFT JOIN tCanalValeIngresso ce (NOLOCK) ON  ValeIngressoTipoID = " + this.Control.ID + " AND CanalID = c.ID " +
                                 "WHERE c.EmpresaID = " + empresaID;
                else
                    sql = "SELECT c.ID, c.Nome FROM tCanal c (NOLOCK) WHERE EmpresaID = " + empresaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    canal = new EstruturaCanalValeIngresso();
                    if (this.Control.ID > 0)
                    {
                        canal.CanalValeIngressoID = bd.LerInt("CanalValeIngressoID");
                        canal.CanalID = bd.LerInt("CanalID");
                        canal.CanalNome = bd.LerString("CanalNome");
                        canal.Distribuido = bd.LerBoolean("Distribuido");

                    }
                    else
                    {
                        canal.CanalValeIngressoID = 0;
                        canal.CanalID = bd.LerInt("ID");
                        canal.CanalNome = bd.LerString("Nome");
                        canal.Distribuido = false;
                    }

                    retorno.Add(canal);
                }

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

        /// <summary>		
        /// Obter todos os VIRs de acordo com o empresa
        /// kim
        /// </summary>
        /// <returns></returns>
        public List<EstruturaIDNome> Todos(int empresaID, string primeiroRegistro)
        {

            try
            {
                EstruturaIDNome valeIngressoTipo;
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();
                //,Valor,ValidadeData,ClienteTipo,ProcedimentoTroca,SaudacaoPadrao,SaudacaoNominal,QuantidadeLimitada,CodigoTrocaFixo,Acumulativo
                string sql = "SELECT ID,Nome " +
                    "FROM tValeIngressoTipo (NOLOCK) WHERE EmpresaID = " + empresaID + " ORDER BY Nome";

                bd.Consulta(sql);
                if (primeiroRegistro != null && primeiroRegistro != String.Empty)
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = -1;
                    valeIngressoTipo.Nome = primeiroRegistro;
                    retorno.Add(valeIngressoTipo);
                }

                while (bd.Consulta().Read())
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");
                    retorno.Add(valeIngressoTipo);
                }

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

        public BindingList<EstruturaIDNome> TodosBinding(int empresaID, string primeiroRegistro)
        {

            try
            {
                EstruturaIDNome valeIngressoTipo;
                BindingList<EstruturaIDNome> retorno = new BindingList<EstruturaIDNome>();
                string sql = "SELECT ID,Nome " +
                    "FROM tValeIngressoTipo (NOLOCK) WHERE EmpresaID = " + empresaID + " ORDER BY Nome";

                bd.Consulta(sql);
                if (primeiroRegistro != null && primeiroRegistro != String.Empty)
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = -1;
                    valeIngressoTipo.Nome = primeiroRegistro;
                    retorno.Add(valeIngressoTipo);
                }

                while (bd.Consulta().Read())
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");

                    retorno.Add(valeIngressoTipo);
                }

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


        public List<EstruturaIDNome> Todos(string primeiroRegistro)
        {
            try
            {
                EstruturaIDNome valeIngressoTipo;
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();
                string sql = "SELECT ID,Nome " +
                    "FROM tValeIngressoTipo (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);
                if (primeiroRegistro != null && primeiroRegistro != String.Empty)
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = -1;
                    valeIngressoTipo.Nome = primeiroRegistro;
                    retorno.Add(valeIngressoTipo);
                }

                while (bd.Consulta().Read())
                {
                    valeIngressoTipo = new EstruturaIDNome();
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");

                    retorno.Add(valeIngressoTipo);
                }

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

        /// <summary>		
        /// Obter todos os VIRs para a internet
        /// kim
        /// </summary>
        /// <returns></returns>
        public List<EstruturaValeIngressoTipo> LerEstruturaValeIngressoTipo(string primeiroRegistro)
        {

            try
            {
                DataTable tabela = new DataTable("Evento");

                EstruturaValeIngressoTipo valeIngressoTipo;
                List<EstruturaValeIngressoTipo> retorno = new List<EstruturaValeIngressoTipo>();
                //,Valor,ValidadeData,ClienteTipo,ProcedimentoTroca,SaudacaoPadrao,SaudacaoNominal,QuantidadeLimitada,CodigoTrocaFixo,Acumulativo
                string sql = "SELECT ID, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, VersaoImagem,ReleaseInternet,PublicarInternet " +
                    "FROM tValeIngressoTipo (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);
                if (primeiroRegistro != null && primeiroRegistro != String.Empty)
                {
                    valeIngressoTipo = new EstruturaValeIngressoTipo();
                    valeIngressoTipo.ID = -1;
                    valeIngressoTipo.Nome = primeiroRegistro;
                    retorno.Add(valeIngressoTipo);
                }

                while (bd.Consulta().Read())
                {
                    valeIngressoTipo = new EstruturaValeIngressoTipo();
                    DataRow linha = tabela.NewRow();
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");
                    valeIngressoTipo.Valor = bd.LerDecimal("Valor");
                    valeIngressoTipo.ValidadeDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                    valeIngressoTipo.ValidadeData = bd.LerDateTime("ValidadeData");
                    valeIngressoTipo.ClienteTipo = (ValeIngressoTipo.EnumClienteTipo)(Convert.ToChar(bd.LerString("ClienteTipo")));
                    valeIngressoTipo.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                    valeIngressoTipo.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                    valeIngressoTipo.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                    valeIngressoTipo.QuantidadeLimitada = bd.LerBoolean("QuantidadeLimitada");
                    valeIngressoTipo.EmpresaID = bd.LerInt("EmpresaID");
                    valeIngressoTipo.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    valeIngressoTipo.Acumulativo = bd.LerBoolean("Acumulativo");
                    valeIngressoTipo.ReleaseInternet = bd.LerString("ReleaseInternet");
                    valeIngressoTipo.PublicarInternet = bd.LerBoolean("PublicarInternet");

                    retorno.Add(valeIngressoTipo);
                }

                bd.Fechar();

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>		
        /// Obter todos os VIRs para a publicação e edição para a internet
        /// Somente as seguintes colunas serão carregadas: ID, Nome, Valor, ImagemInternet,ReleaseInternet,PublicarInternet
        /// kim
        /// </summary>
        /// <returns></returns>
        public List<EstruturaValeIngressoTipo> LerEstruturaValeIngressoTipoInternet(string primeiroRegistro)
        {

            try
            {
                DataTable tabela = new DataTable("Evento");

                EstruturaValeIngressoTipo valeIngressoTipo;
                List<EstruturaValeIngressoTipo> retorno = new List<EstruturaValeIngressoTipo>();
                string sql = "SELECT ID, Nome, Valor, ImagemInternet,ReleaseInternet,PublicarInternet" +
                    "FROM tValeIngressoTipo (NOLOCK) WHERE ORDER BY Nome";

                bd.Consulta(sql);
                if (primeiroRegistro != null && primeiroRegistro != String.Empty)
                {
                    valeIngressoTipo = new EstruturaValeIngressoTipo();
                    valeIngressoTipo.ID = -1;
                    valeIngressoTipo.Nome = primeiroRegistro;
                    retorno.Add(valeIngressoTipo);
                }

                while (bd.Consulta().Read())
                {
                    valeIngressoTipo = new EstruturaValeIngressoTipo();
                    DataRow linha = tabela.NewRow();
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");
                    valeIngressoTipo.Valor = bd.LerDecimal("Valor");
                    valeIngressoTipo.ReleaseInternet = bd.LerString("ReleaseInternet");
                    valeIngressoTipo.PublicarInternet = bd.LerBoolean("PublicarInternet");
                    retorno.Add(valeIngressoTipo);
                }

                bd.Fechar();

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Popula o objeto utilizado na ValeIngressoWizard.
        /// Esse objeto possui todas as informações necessárias para editar nessa tela, incluindo distribuição de canais.
        /// </summary>
        /// <param name="valeIngressoTipoID"></param>
        /// <returns></returns>
        public EstruturaValeIngressoWizard Carregar(int valeIngressoTipoID)
        {
            try
            {
                EstruturaValeIngressoWizard retorno = new EstruturaValeIngressoWizard();
                EstruturaValeIngressoTipo valeIngressoTipo = new EstruturaValeIngressoTipo();

                //Carrega as Informações do ValeIngressoTipo. Já com a quantidade de virs disponiveis desse tipo
                string sql = @"SELECT virTipo.ID, virTipo.Nome, virTipo.Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, 
	                           SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, ISNULL(COUNT(vir.ID),0) AS QuantidadeDisponivel, PublicarInternet,ReleaseInternet,
                               virTipo.TrocaEntrega,virTipo.TrocaIngresso,virTipo.TrocaConveniencia, virTipo.ValorTipo, virTipo.ValorPagamento
                               FROM tValeIngressoTipo virTipo (NOLOCK)
                               LEFT JOIN tValeIngresso vir (NOLOCK) ON virTipo.ID = vir.ValeIngressoTipoID AND Status = '" + (char)ValeIngresso.enumStatus.Disponivel + @"'
                               WHERE virTipo.ID = " + valeIngressoTipoID + @" 
                               GROUP BY virTipo.ID, virTipo.Nome, virTipo.Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, 
	                           SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo,PublicarInternet,ReleaseInternet,virTipo.TrocaEntrega,virTipo.TrocaIngresso,virTipo.TrocaConveniencia, virTipo.ValorTipo, virTipo.ValorPagamento";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    valeIngressoTipo.ID = bd.LerInt("ID");
                    valeIngressoTipo.Nome = bd.LerString("Nome");
                    valeIngressoTipo.Valor = bd.LerDecimal("Valor");
                    valeIngressoTipo.ValidadeDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                    valeIngressoTipo.ValidadeData = bd.LerDateTime("ValidadeData");
                    valeIngressoTipo.ClienteTipo = (ValeIngressoTipo.EnumClienteTipo)(Convert.ToChar(bd.LerString("ClienteTipo")));
                    valeIngressoTipo.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                    valeIngressoTipo.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                    valeIngressoTipo.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                    valeIngressoTipo.QuantidadeLimitada = bd.LerBoolean("QuantidadeLimitada");
                    valeIngressoTipo.EmpresaID = bd.LerInt("EmpresaID");
                    valeIngressoTipo.CodigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                    valeIngressoTipo.Acumulativo = bd.LerBoolean("Acumulativo");
                    valeIngressoTipo.QuantidadeDisponivelTipo = bd.LerInt("QuantidadeDisponivel");
                    valeIngressoTipo.PublicarInternet = bd.LerBoolean("PublicarInternet");
                    valeIngressoTipo.ReleaseInternet = bd.LerString("ReleaseInternet");
                    valeIngressoTipo.TrocaEntrega = bd.LerBoolean("TrocaEntrega");
                    valeIngressoTipo.TrocaIngresso = bd.LerBoolean("TrocaIngresso");
                    valeIngressoTipo.TrocaConveniencia = bd.LerBoolean("TrocaConveniencia");
                    valeIngressoTipo.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));
                    valeIngressoTipo.ValorPagamento = bd.LerDecimal("ValorPagamento");
                }
                //preenche o objeto de retorno
                retorno.ValeIngressoTipo = valeIngressoTipo;
                //Preenche esse objeto
                this.Control.ID = valeIngressoTipo.ID;

                //preenche os canais IR
                sql = @"SELECT CASE WHEN vi.ValeIngressoTipoID IS NOT NULL THEN 'T' ELSE 'F' END AS Distribuido,c.ID AS CanalID
                        FROM tCanal c (NOLOCK)
                        LEFT JOIN tCanalValeIngresso vi (NOLOCK) ON vi.CanalID = c.ID AND vi.ValeIngressoTipoID = " + valeIngressoTipoID + @"
                        INNER JOIN tEmpresa e (NOLOCK) ON e.ID = c.EmpresaID
                        WHERE (e.EmpresaVende = 'T' AND e.EmpresaPromove = 'F')";

                bd.Consulta(sql);
                int canalID = 0;
                bool distribuido;
                int canalInternet = Canal.CANAL_INTERNET;
                int canalCC = Canal.CANAL_CALL_CENTER;
                while (bd.Consulta().Read())
                {
                    //verifica se o call center e internet estão distribuidos
                    canalID = bd.LerInt("CanalID");
                    distribuido = bd.LerBoolean("Distribuido");
                    //call center
                    if (canalID == canalInternet)
                        retorno.DistribuidoCanalInternet = distribuido;
                    //internet
                    else if (canalID == canalCC)
                        retorno.DistribuidoCanalCallCenter = distribuido;
                    //faz a contagem dos canais próprios
                    else
                    {
                        if (distribuido)
                            retorno.QtdeCanaisIRDistribuidos++;
                        else
                            retorno.QtdeCanaisIRNaoDistribuidos++;
                    }
                }
                //preencher os canais próprios
                retorno.Canais = this.CanaisDistribuidosLista(valeIngressoTipo.EmpresaID);

                //Busca a quantidade disponivel para o vale ingresso
                sql = "SELECT COUNT(ID) AS Quantidade FROM tValeIngresso (NOLOCK) WHERE Status = '" + (char)ValeIngresso.enumStatus.Disponivel + "' AND ValeIngressoTipoID = " + valeIngressoTipoID;
                retorno.QtdeValeIngressoDisponivel = Convert.ToInt32(bd.ConsultaValor(sql));

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { bd.Fechar(); }
        }
        public bool SalvarValeIngresso(EstruturaValeIngressoWizard dadosSalvar)
        {
            ValeIngresso valeIngresso;
            CanalValeIngresso canalValeIngresso;
            bool novoVirTipo = false;
            BD bdDelete = new BD();
            try
            {

                //td deve estar dentro de transaction
                bd.IniciarTransacao();
                bdDelete.IniciarTransacao();
                novoVirTipo = dadosSalvar.ValeIngressoTipo.ID == 0;

                //TODO: Fazer a validação dos dados. Inclusive usuarioID


                //Popula o objeto para salvar no banco
                this.Nome.Valor = dadosSalvar.ValeIngressoTipo.Nome;
                this.Valor.Valor = dadosSalvar.ValeIngressoTipo.Valor;
                this.ValidadeDiasImpressao.Valor = dadosSalvar.ValeIngressoTipo.ValidadeDiasImpressao;
                if (this.ValidadeDiasImpressao.Valor == 0)
                    this.ValidadeData.Valor = dadosSalvar.ValeIngressoTipo.ValidadeData;
                this.CodigoTrocaFixo.Valor = dadosSalvar.ValeIngressoTipo.CodigoTrocaFixo;
                this.ClienteTipo.Valor = ((char)dadosSalvar.ValeIngressoTipo.ClienteTipo).ToString();
                this.ProcedimentoTroca.Valor = dadosSalvar.ValeIngressoTipo.ProcedimentoTroca;
                this.SaudacaoPadrao.Valor = dadosSalvar.ValeIngressoTipo.SaudacaoPadrao;
                this.SaudacaoNominal.Valor = dadosSalvar.ValeIngressoTipo.SaudacaoNominal;
                this.QuantidadeLimitada.Valor = dadosSalvar.ValeIngressoTipo.QuantidadeLimitada;
                this.EmpresaID.Valor = dadosSalvar.ValeIngressoTipo.EmpresaID;
                this.Acumulativo.Valor = dadosSalvar.ValeIngressoTipo.Acumulativo;
                this.PublicarInternet.Valor = dadosSalvar.ValeIngressoTipo.PublicarInternet;
                this.ReleaseInternet.Valor = dadosSalvar.ValeIngressoTipo.ReleaseInternet;
                this.TrocaConveniencia.Valor = dadosSalvar.ValeIngressoTipo.TrocaConveniencia;
                this.TrocaIngresso.Valor = dadosSalvar.ValeIngressoTipo.TrocaIngresso;
                this.TrocaEntrega.Valor = dadosSalvar.ValeIngressoTipo.TrocaEntrega;
                this.ValorTipo.Valor = ((char)dadosSalvar.ValeIngressoTipo.ValorTipo).ToString();
                this.ValorPagamento.Valor = dadosSalvar.ValeIngressoTipo.ValorPagamento;

                if (novoVirTipo)
                {//Inserir

                    this.Inserir(bd);
                    //Insere na tValeIngressoTipo
                }
                else
                {//Atualizar
                    this.Control.ID = dadosSalvar.ValeIngressoTipo.ID;
                    this.Atualizar(bd);
                }

                //Verifica a necessidade de inserir registros na tValeIngresso
                if (dadosSalvar.ValeIngressoTipo.QuantidadeLimitada)
                { //Inserir na tValeIngresso
                    for (int i = 0; i < dadosSalvar.AdicionarQuantidade; i++)
                    {
                        try
                        {
                            valeIngresso = new ValeIngresso();
                            valeIngresso.ValeIngressoTipoID.Valor = this.Control.ID;
                            valeIngresso.DataCriacao.Valor = DateTime.Now;
                            valeIngresso.Status.Valor = ((char)ValeIngresso.enumStatus.Disponivel).ToString();

                            if (dadosSalvar.ValeIngressoTipo.CodigoTrocaFixo != String.Empty)
                                //Codigo fixo. Todos devem ter todos os códigos 
                                valeIngresso.CodigoTroca.Valor = dadosSalvar.ValeIngressoTipo.CodigoTrocaFixo;
                            else
                                valeIngresso.CodigoTroca.Valor = string.Empty;

                            valeIngresso.Inserir(bd);
                        }
                        catch
                        {

                            throw;
                        }
                    }
                }

                int canalInternet = Canal.CANAL_INTERNET;
                int canalCC = Canal.CANAL_CALL_CENTER;
                //Atualiza os canais
                string sql = string.Empty;
                if (!novoVirTipo)
                {
                    //Verifica se deve remover ou inserir os canais ir (tirando call center e internet)
                    if (dadosSalvar.acaoCanaisIR == EnumAcaoCanais.Distribuir)
                    {
                        //Inserre todos os canais IR. Tirando Call Center e Internet
                        sql = "INSERT INTO tCanalValeIngresso (CanalID,ValeIngressoTipoID) " +
                            " SELECT c.ID, " + this.Control.ID + " FROM tCanal c (NOLOCK) " +
                            " LEFT JOIN tCanalValeIngresso vi (NOLOCK) ON vi.CanalID = c.ID AND vi.ValeIngressoTipoID =" + this.Control.ID +
                            " INNER JOIN tEmpresa e (NOLOCK) ON e.ID = c.EmpresaID " +
                            " WHERE (e.EmpresaVende = 'T' AND e.EmpresaPromove = 'F') AND vi.ID IS NULL AND c.ID NOT IN (" + canalInternet + "," + canalCC + ")";
                    }
                    else if (dadosSalvar.acaoCanaisIR == EnumAcaoCanais.Remover) // Remove somente os canais que não forem IR
                    {
                        sql = "DELETE cvir FROM tCanalValeIngresso AS cvir " +
                            "LEFT JOIN tCanal ON tCanal.ID = cvir.CanalID " +
                            "INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID AND (tEmpresa.EmpresaVende = 'T' AND tEmpresa.EmpresaPromove = 'F') " +
                            "WHERE ValeIngressoTipoID = " + dadosSalvar.ValeIngressoTipo.ID + " AND tCanal.ID NOT IN (" + canalInternet + "," + canalCC + ")";
                    }
                    if (dadosSalvar.acaoCanaisIR != EnumAcaoCanais.Manter)
                        bd.Executar(sql);
                }
                //Distribui os canais próprios + Call Center e Internet
                foreach (EstruturaCanalValeIngresso canal in dadosSalvar.Canais)
                {
                    if (canal.acao == CanalValeIngresso.EnumAcaoCanal.Inserir)
                    {
                        canalValeIngresso = new CanalValeIngresso(usuarioIDLogado);
                        canalValeIngresso.CanalID.Valor = canal.CanalID;
                        canalValeIngresso.ValeIngressoTipoID.Valor = this.Control.ID;
                        canalValeIngresso.Inserir(bd);
                    }
                    else if (canal.acao == CanalValeIngresso.EnumAcaoCanal.Remover)
                    {
                        canalValeIngresso = new CanalValeIngresso(usuarioIDLogado);
                        canalValeIngresso.Control.ID = canal.CanalValeIngressoID;
                        canalValeIngresso.CanalID.Valor = canal.CanalID;
                        canalValeIngresso.ValeIngressoTipoID.Valor = canal.ValeIngressoTipoID;
                        //Call Center e Internet
                        if (canal.CanalValeIngressoID == 0 && (canal.CanalID == canalInternet || canal.CanalID == canalCC))
                            canalValeIngresso.Excluir(canal.CanalID, canal.ValeIngressoTipoID, bd);
                        else
                            canalValeIngresso.Excluir(canalValeIngresso.Control.ID, bd);
                    }
                }

                bd.FinalizarTransacao();
                bdDelete.FinalizarTransacao();
                return true;
            }
            catch (Exception)
            {
                bdDelete.DesfazerTransacao();
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<EstruturaSelecaoArvoreValeIngresso> BuscarPorEmpresa(int empresaID)
        {
            try
            {
                List<EstruturaSelecaoArvoreValeIngresso> lista = new List<EstruturaSelecaoArvoreValeIngresso>();
                string filtro = empresaID > 0 ? "WHERE vit.EmpresaID = " + empresaID : string.Empty;
                string consulta =
                    string.Format(
                    @"
                        SELECT e.Nome AS Empresa, vit.ID AS ValeIngressoTipoID, vit.Nome AS ValeIngressoTipo
                        FROM tEmpresa e (NOLOCK)
                        INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.EmpresaID = e.ID
                        {0}
                    ", filtro);

                bd.Consulta(consulta);
                while (bd.Consulta().Read())
                    lista.Add(new EstruturaSelecaoArvoreValeIngresso()
                    {
                        Empresa = bd.LerString("Empresa"),
                        ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID"),
                        ValeIngressoTipo = bd.LerString("ValeIngressoTipo"),
                    });
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidaCodigoFixo(string CodigoTroca)
        {
            try
            {
                string SQL;
                bool retorno = false;

                SQL = @"SELECT ID FROM tValeIngressoTipo (NOLOCK) WHERE CodigoTrocaFixo = '" + CodigoTroca + "'";

                bd.Consulta(SQL);

                if (bd.Consulta().Read())
                {
                    retorno = true;
                }

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

        public EstruturaRetornoReservaValeIngresso CarregarEstrutura(int ValeIngressoID)
        {
            try
            {
                EstruturaRetornoReservaValeIngresso estrutura = new EstruturaRetornoReservaValeIngresso();

                string SQL = @"SELECT vit.ValidadeDiasImpressao,vit.ValidadeData 
                                FROM tValeIngresso vi (nolock)
                                INNER JOIN tValeIngressoTipo vit (nolock) on vi.ValeIngressoTipoID = vit.ID
                                WHERE vi.ID =  " + ValeIngressoID;

                bd.Consulta(SQL);

                if (bd.Consulta().Read())
                {
                    estrutura.ValidadeDias = bd.LerInt("ValidadeDiasImpressao");
                    estrutura.Validade = bd.LerDateTime("ValidadeData");
                }

                return estrutura;
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
    }

    public class ValeIngressoTipoLista : ValeIngressoTipoLista_B
    {

        public ValeIngressoTipoLista() { }

        public ValeIngressoTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
