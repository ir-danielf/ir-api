/******************************************************
* Arquivo ValeIngressoTipoDB.cs
* Gerado em: 15/08/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "ValeIngressoTipo_B"

    public abstract class ValeIngressoTipo_B : BaseBD
    {

        public nome Nome = new nome();
        public valor Valor = new valor();
        public validadediasimpressao ValidadeDiasImpressao = new validadediasimpressao();
        public validadedata ValidadeData = new validadedata();
        public clientetipo ClienteTipo = new clientetipo();
        public procedimentotroca ProcedimentoTroca = new procedimentotroca();
        public saudacaopadrao SaudacaoPadrao = new saudacaopadrao();
        public saudacaonominal SaudacaoNominal = new saudacaonominal();
        public quantidadelimitada QuantidadeLimitada = new quantidadelimitada();
        public empresaid EmpresaID = new empresaid();
        public codigotrocafixo CodigoTrocaFixo = new codigotrocafixo();
        public acumulativo Acumulativo = new acumulativo();
        public versaoimagem VersaoImagem = new versaoimagem();
        public versaoimageminternet VersaoImagemInternet = new versaoimageminternet();
        public releaseinternet ReleaseInternet = new releaseinternet();
        public publicarinternet PublicarInternet = new publicarinternet();
        public ultimocodigoimpresso UltimoCodigoImpresso = new ultimocodigoimpresso();
        public trocaentrega TrocaEntrega = new trocaentrega();
        public trocaingresso TrocaIngresso = new trocaingresso();
        public trocaconveniencia TrocaConveniencia = new trocaconveniencia();
        public valortipo ValorTipo = new valortipo();
        public valorpagamento ValorPagamento = new valorpagamento();

        public ValeIngressoTipo_B() { }

        // passar o Usuario logado no sistema
        public ValeIngressoTipo_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ValeIngressoTipo
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tValeIngressoTipo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ValidadeDiasImpressao.ValorBD = bd.LerInt("ValidadeDiasImpressao").ToString();
                    this.ValidadeData.ValorBD = bd.LerString("ValidadeData");
                    this.ClienteTipo.ValorBD = bd.LerString("ClienteTipo");
                    this.ProcedimentoTroca.ValorBD = bd.LerString("ProcedimentoTroca");
                    this.SaudacaoPadrao.ValorBD = bd.LerString("SaudacaoPadrao");
                    this.SaudacaoNominal.ValorBD = bd.LerString("SaudacaoNominal");
                    this.QuantidadeLimitada.ValorBD = bd.LerString("QuantidadeLimitada");
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CodigoTrocaFixo.ValorBD = bd.LerString("CodigoTrocaFixo");
                    this.Acumulativo.ValorBD = bd.LerString("Acumulativo");
                    this.VersaoImagem.ValorBD = bd.LerInt("VersaoImagem").ToString();
                    this.VersaoImagemInternet.ValorBD = bd.LerInt("VersaoImagemInternet").ToString();
                    this.ReleaseInternet.ValorBD = bd.LerString("ReleaseInternet");
                    this.PublicarInternet.ValorBD = bd.LerString("PublicarInternet");
                    this.UltimoCodigoImpresso.ValorBD = bd.LerInt("UltimoCodigoImpresso").ToString();
                    this.TrocaEntrega.ValorBD = bd.LerString("TrocaEntrega");
                    this.TrocaIngresso.ValorBD = bd.LerString("TrocaIngresso");
                    this.TrocaConveniencia.ValorBD = bd.LerString("TrocaConveniencia");
                    this.ValorTipo.ValorBD = bd.LerString("ValorTipo");
                    this.ValorPagamento.ValorBD = bd.LerDecimal("ValorPagamento").ToString();
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

        /// <summary>
        /// Preenche todos os atributos de ValeIngressoTipo do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xValeIngressoTipo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ValidadeDiasImpressao.ValorBD = bd.LerInt("ValidadeDiasImpressao").ToString();
                    this.ValidadeData.ValorBD = bd.LerString("ValidadeData");
                    this.ClienteTipo.ValorBD = bd.LerString("ClienteTipo");
                    this.ProcedimentoTroca.ValorBD = bd.LerString("ProcedimentoTroca");
                    this.SaudacaoPadrao.ValorBD = bd.LerString("SaudacaoPadrao");
                    this.SaudacaoNominal.ValorBD = bd.LerString("SaudacaoNominal");
                    this.QuantidadeLimitada.ValorBD = bd.LerString("QuantidadeLimitada");
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CodigoTrocaFixo.ValorBD = bd.LerString("CodigoTrocaFixo");
                    this.Acumulativo.ValorBD = bd.LerString("Acumulativo");
                    this.VersaoImagem.ValorBD = bd.LerInt("VersaoImagem").ToString();
                    this.VersaoImagemInternet.ValorBD = bd.LerInt("VersaoImagemInternet").ToString();
                    this.ReleaseInternet.ValorBD = bd.LerString("ReleaseInternet");
                    this.PublicarInternet.ValorBD = bd.LerString("PublicarInternet");
                    this.UltimoCodigoImpresso.ValorBD = bd.LerInt("UltimoCodigoImpresso").ToString();
                    this.TrocaEntrega.ValorBD = bd.LerString("TrocaEntrega");
                    this.TrocaIngresso.ValorBD = bd.LerString("TrocaIngresso");
                    this.TrocaConveniencia.ValorBD = bd.LerString("TrocaConveniencia");
                    this.ValorTipo.ValorBD = bd.LerString("ValorTipo");
                    this.ValorPagamento.ValorBD = bd.LerDecimal("ValorPagamento").ToString();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cValeIngressoTipo (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xValeIngressoTipo (ID, Versao, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, VersaoImagem, VersaoImagemInternet, ReleaseInternet, PublicarInternet, UltimoCodigoImpresso, TrocaEntrega, TrocaIngresso, TrocaConveniencia, ValorTipo, ValorPagamento) ");
                sql.Append("SELECT ID, @V, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, VersaoImagem, VersaoImagemInternet, ReleaseInternet, PublicarInternet, UltimoCodigoImpresso, TrocaEntrega, TrocaIngresso, TrocaConveniencia, ValorTipo, ValorPagamento FROM tValeIngressoTipo WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cValeIngressoTipo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tValeIngressoTipo(ID, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, VersaoImagem, VersaoImagemInternet, ReleaseInternet, PublicarInternet, UltimoCodigoImpresso, TrocaEntrega, TrocaIngresso, TrocaConveniencia, ValorTipo, ValorPagamento) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,'@004','@005','@006','@007','@008','@009',@010,'@011','@012',@013,@014,'@015','@016',@017,'@018','@019','@020','@021','@022')");

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
                sql.Replace("@013", this.VersaoImagem.ValorBD);
                sql.Replace("@014", this.VersaoImagemInternet.ValorBD);
                sql.Replace("@015", this.ReleaseInternet.ValorBD);
                sql.Replace("@016", this.PublicarInternet.ValorBD);
                sql.Replace("@017", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@018", this.TrocaEntrega.ValorBD);
                sql.Replace("@019", this.TrocaIngresso.ValorBD);
                sql.Replace("@020", this.TrocaConveniencia.ValorBD);
                sql.Replace("@021", this.ValorTipo.ValorBD);
                sql.Replace("@022", this.ValorPagamento.ValorBD);

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
        /// Inserir novo(a) ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
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
                sql.Append("INSERT INTO tValeIngressoTipo(ID, Nome, Valor, ValidadeDiasImpressao, ValidadeData, ClienteTipo, ProcedimentoTroca, SaudacaoPadrao, SaudacaoNominal, QuantidadeLimitada, EmpresaID, CodigoTrocaFixo, Acumulativo, VersaoImagem, VersaoImagemInternet, ReleaseInternet, PublicarInternet, UltimoCodigoImpresso, TrocaEntrega, TrocaIngresso, TrocaConveniencia, ValorTipo, ValorPagamento) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,'@004','@005','@006','@007','@008','@009',@010,'@011','@012',@013,@014,'@015','@016',@017,'@018','@019','@020','@021','@022')");

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
                sql.Replace("@013", this.VersaoImagem.ValorBD);
                sql.Replace("@014", this.VersaoImagemInternet.ValorBD);
                sql.Replace("@015", this.ReleaseInternet.ValorBD);
                sql.Replace("@016", this.PublicarInternet.ValorBD);
                sql.Replace("@017", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@018", this.TrocaEntrega.ValorBD);
                sql.Replace("@019", this.TrocaIngresso.ValorBD);
                sql.Replace("@020", this.TrocaConveniencia.ValorBD);
                sql.Replace("@021", this.ValorTipo.ValorBD);
                sql.Replace("@022", this.ValorPagamento.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cValeIngressoTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tValeIngressoTipo SET Nome = '@001', Valor = '@002', ValidadeDiasImpressao = @003, ValidadeData = '@004', ClienteTipo = '@005', ProcedimentoTroca = '@006', SaudacaoPadrao = '@007', SaudacaoNominal = '@008', QuantidadeLimitada = '@009', EmpresaID = @010, CodigoTrocaFixo = '@011', Acumulativo = '@012', VersaoImagem = @013, VersaoImagemInternet = @014, ReleaseInternet = '@015', PublicarInternet = '@016', UltimoCodigoImpresso = @017, TrocaEntrega = '@018', TrocaIngresso = '@019', TrocaConveniencia = '@020', ValorTipo = '@021', ValorPagamento = '@022' ");
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
                sql.Replace("@013", this.VersaoImagem.ValorBD);
                sql.Replace("@014", this.VersaoImagemInternet.ValorBD);
                sql.Replace("@015", this.ReleaseInternet.ValorBD);
                sql.Replace("@016", this.PublicarInternet.ValorBD);
                sql.Replace("@017", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@018", this.TrocaEntrega.ValorBD);
                sql.Replace("@019", this.TrocaIngresso.ValorBD);
                sql.Replace("@020", this.TrocaConveniencia.ValorBD);
                sql.Replace("@021", this.ValorTipo.ValorBD);
                sql.Replace("@022", this.ValorPagamento.ValorBD);

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
        /// Atualiza ValeIngressoTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
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
                sql.Append("UPDATE tValeIngressoTipo SET Nome = '@001', Valor = '@002', ValidadeDiasImpressao = @003, ValidadeData = '@004', ClienteTipo = '@005', ProcedimentoTroca = '@006', SaudacaoPadrao = '@007', SaudacaoNominal = '@008', QuantidadeLimitada = '@009', EmpresaID = @010, CodigoTrocaFixo = '@011', Acumulativo = '@012', VersaoImagem = @013, VersaoImagemInternet = @014, ReleaseInternet = '@015', PublicarInternet = '@016', UltimoCodigoImpresso = @017, TrocaEntrega = '@018', TrocaIngresso = '@019', TrocaConveniencia = '@020', ValorTipo = '@021', ValorPagamento = '@022' ");
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
                sql.Replace("@013", this.VersaoImagem.ValorBD);
                sql.Replace("@014", this.VersaoImagemInternet.ValorBD);
                sql.Replace("@015", this.ReleaseInternet.ValorBD);
                sql.Replace("@016", this.PublicarInternet.ValorBD);
                sql.Replace("@017", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@018", this.TrocaEntrega.ValorBD);
                sql.Replace("@019", this.TrocaIngresso.ValorBD);
                sql.Replace("@020", this.TrocaConveniencia.ValorBD);
                sql.Replace("@021", this.ValorTipo.ValorBD);
                sql.Replace("@022", this.ValorPagamento.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui ValeIngressoTipo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cValeIngressoTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tValeIngressoTipo WHERE ID=" + id;

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

        /// <summary>
        /// Exclui ValeIngressoTipo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cValeIngressoTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tValeIngressoTipo WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui ValeIngressoTipo
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void Limpar()
        {

            this.Nome.Limpar();
            this.Valor.Limpar();
            this.ValidadeDiasImpressao.Limpar();
            this.ValidadeData.Limpar();
            this.ClienteTipo.Limpar();
            this.ProcedimentoTroca.Limpar();
            this.SaudacaoPadrao.Limpar();
            this.SaudacaoNominal.Limpar();
            this.QuantidadeLimitada.Limpar();
            this.EmpresaID.Limpar();
            this.CodigoTrocaFixo.Limpar();
            this.Acumulativo.Limpar();
            this.VersaoImagem.Limpar();
            this.VersaoImagemInternet.Limpar();
            this.ReleaseInternet.Limpar();
            this.PublicarInternet.Limpar();
            this.UltimoCodigoImpresso.Limpar();
            this.TrocaEntrega.Limpar();
            this.TrocaIngresso.Limpar();
            this.TrocaConveniencia.Limpar();
            this.ValorTipo.Limpar();
            this.ValorPagamento.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.Valor.Desfazer();
            this.ValidadeDiasImpressao.Desfazer();
            this.ValidadeData.Desfazer();
            this.ClienteTipo.Desfazer();
            this.ProcedimentoTroca.Desfazer();
            this.SaudacaoPadrao.Desfazer();
            this.SaudacaoNominal.Desfazer();
            this.QuantidadeLimitada.Desfazer();
            this.EmpresaID.Desfazer();
            this.CodigoTrocaFixo.Desfazer();
            this.Acumulativo.Desfazer();
            this.VersaoImagem.Desfazer();
            this.VersaoImagemInternet.Desfazer();
            this.ReleaseInternet.Desfazer();
            this.PublicarInternet.Desfazer();
            this.UltimoCodigoImpresso.Desfazer();
            this.TrocaEntrega.Desfazer();
            this.TrocaIngresso.Desfazer();
            this.TrocaConveniencia.Desfazer();
            this.ValorTipo.Desfazer();
            this.ValorPagamento.Desfazer();
        }

        public class nome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class valor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class validadediasimpressao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValidadeDiasImpressao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class validadedata : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValidadeData";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class clientetipo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteTipo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class procedimentotroca : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProcedimentoTroca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 500;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class saudacaopadrao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SaudacaoPadrao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class saudacaonominal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SaudacaoNominal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class quantidadelimitada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeLimitada";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class codigotrocafixo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoTrocaFixo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class acumulativo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acumulativo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class versaoimagem : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagem";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class versaoimageminternet : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemInternet";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class releaseinternet : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ReleaseInternet";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1000;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class publicarinternet : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PublicarInternet";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class ultimocodigoimpresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UltimoCodigoImpresso";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class trocaentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaEntrega";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class trocaingresso : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaIngresso";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class trocaconveniencia : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaConveniencia";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class valortipo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorTipo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class valorpagamento : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorPagamento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("ValeIngressoTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ValidadeDiasImpressao", typeof(int));
                tabela.Columns.Add("ValidadeData", typeof(DateTime));
                tabela.Columns.Add("ClienteTipo", typeof(string));
                tabela.Columns.Add("ProcedimentoTroca", typeof(string));
                tabela.Columns.Add("SaudacaoPadrao", typeof(string));
                tabela.Columns.Add("SaudacaoNominal", typeof(string));
                tabela.Columns.Add("QuantidadeLimitada", typeof(bool));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                tabela.Columns.Add("Acumulativo", typeof(bool));
                tabela.Columns.Add("VersaoImagem", typeof(int));
                tabela.Columns.Add("VersaoImagemInternet", typeof(int));
                tabela.Columns.Add("ReleaseInternet", typeof(string));
                tabela.Columns.Add("PublicarInternet", typeof(bool));
                tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                tabela.Columns.Add("TrocaEntrega", typeof(bool));
                tabela.Columns.Add("TrocaIngresso", typeof(bool));
                tabela.Columns.Add("TrocaConveniencia", typeof(bool));
                tabela.Columns.Add("ValorTipo", typeof(string));
                tabela.Columns.Add("ValorPagamento", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ValeIngressoTipoLista_B"

    public abstract class ValeIngressoTipoLista_B : BaseLista
    {

        private bool backup = false;
        protected ValeIngressoTipo valeIngressoTipo;

        // passar o Usuario logado no sistema
        public ValeIngressoTipoLista_B()
        {
            valeIngressoTipo = new ValeIngressoTipo();
        }

        // passar o Usuario logado no sistema
        public ValeIngressoTipoLista_B(int usuarioIDLogado)
        {
            valeIngressoTipo = new ValeIngressoTipo(usuarioIDLogado);
        }

        public ValeIngressoTipo ValeIngressoTipo
        {
            get { return valeIngressoTipo; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ValeIngressoTipo especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    valeIngressoTipo.Ler(id);
                    return valeIngressoTipo;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tValeIngressoTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngressoTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tValeIngressoTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngressoTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xValeIngressoTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xValeIngressoTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche ValeIngressoTipo corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    valeIngressoTipo.Ler(id);
                else
                    valeIngressoTipo.LerBackup(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = valeIngressoTipo.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) ValeIngressoTipo na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = valeIngressoTipo.Inserir();
                if (ok)
                {
                    lista.Add(valeIngressoTipo.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Obtem uma tabela de todos os campos de ValeIngressoTipo carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ValeIngressoTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ValidadeDiasImpressao", typeof(int));
                tabela.Columns.Add("ValidadeData", typeof(DateTime));
                tabela.Columns.Add("ClienteTipo", typeof(string));
                tabela.Columns.Add("ProcedimentoTroca", typeof(string));
                tabela.Columns.Add("SaudacaoPadrao", typeof(string));
                tabela.Columns.Add("SaudacaoNominal", typeof(string));
                tabela.Columns.Add("QuantidadeLimitada", typeof(bool));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                tabela.Columns.Add("Acumulativo", typeof(bool));
                tabela.Columns.Add("VersaoImagem", typeof(int));
                tabela.Columns.Add("VersaoImagemInternet", typeof(int));
                tabela.Columns.Add("ReleaseInternet", typeof(string));
                tabela.Columns.Add("PublicarInternet", typeof(bool));
                tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                tabela.Columns.Add("TrocaEntrega", typeof(bool));
                tabela.Columns.Add("TrocaIngresso", typeof(bool));
                tabela.Columns.Add("TrocaConveniencia", typeof(bool));
                tabela.Columns.Add("ValorTipo", typeof(string));
                tabela.Columns.Add("ValorPagamento", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = valeIngressoTipo.Control.ID;
                        linha["Nome"] = valeIngressoTipo.Nome.Valor;
                        linha["Valor"] = valeIngressoTipo.Valor.Valor;
                        linha["ValidadeDiasImpressao"] = valeIngressoTipo.ValidadeDiasImpressao.Valor;
                        linha["ValidadeData"] = valeIngressoTipo.ValidadeData.Valor;
                        linha["ClienteTipo"] = valeIngressoTipo.ClienteTipo.Valor;
                        linha["ProcedimentoTroca"] = valeIngressoTipo.ProcedimentoTroca.Valor;
                        linha["SaudacaoPadrao"] = valeIngressoTipo.SaudacaoPadrao.Valor;
                        linha["SaudacaoNominal"] = valeIngressoTipo.SaudacaoNominal.Valor;
                        linha["QuantidadeLimitada"] = valeIngressoTipo.QuantidadeLimitada.Valor;
                        linha["EmpresaID"] = valeIngressoTipo.EmpresaID.Valor;
                        linha["CodigoTrocaFixo"] = valeIngressoTipo.CodigoTrocaFixo.Valor;
                        linha["Acumulativo"] = valeIngressoTipo.Acumulativo.Valor;
                        linha["VersaoImagem"] = valeIngressoTipo.VersaoImagem.Valor;
                        linha["VersaoImagemInternet"] = valeIngressoTipo.VersaoImagemInternet.Valor;
                        linha["ReleaseInternet"] = valeIngressoTipo.ReleaseInternet.Valor;
                        linha["PublicarInternet"] = valeIngressoTipo.PublicarInternet.Valor;
                        linha["UltimoCodigoImpresso"] = valeIngressoTipo.UltimoCodigoImpresso.Valor;
                        linha["TrocaEntrega"] = valeIngressoTipo.TrocaEntrega.Valor;
                        linha["TrocaIngresso"] = valeIngressoTipo.TrocaIngresso.Valor;
                        linha["TrocaConveniencia"] = valeIngressoTipo.TrocaConveniencia.Valor;
                        linha["ValorTipo"] = valeIngressoTipo.ValorTipo.Valor;
                        linha["ValorPagamento"] = valeIngressoTipo.ValorPagamento.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
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

                DataTable tabela = new DataTable("RelatorioValeIngressoTipo");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("ValidadeDiasImpressao", typeof(int));
                    tabela.Columns.Add("ValidadeData", typeof(DateTime));
                    tabela.Columns.Add("ClienteTipo", typeof(string));
                    tabela.Columns.Add("ProcedimentoTroca", typeof(string));
                    tabela.Columns.Add("SaudacaoPadrao", typeof(string));
                    tabela.Columns.Add("SaudacaoNominal", typeof(string));
                    tabela.Columns.Add("QuantidadeLimitada", typeof(bool));
                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                    tabela.Columns.Add("Acumulativo", typeof(bool));
                    tabela.Columns.Add("VersaoImagem", typeof(int));
                    tabela.Columns.Add("VersaoImagemInternet", typeof(int));
                    tabela.Columns.Add("ReleaseInternet", typeof(string));
                    tabela.Columns.Add("PublicarInternet", typeof(bool));
                    tabela.Columns.Add("UltimoCodigoImpresso", typeof(int));
                    tabela.Columns.Add("TrocaEntrega", typeof(bool));
                    tabela.Columns.Add("TrocaIngresso", typeof(bool));
                    tabela.Columns.Add("TrocaConveniencia", typeof(bool));
                    tabela.Columns.Add("ValorTipo", typeof(string));
                    tabela.Columns.Add("ValorPagamento", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = valeIngressoTipo.Nome.Valor;
                        linha["Valor"] = valeIngressoTipo.Valor.Valor;
                        linha["ValidadeDiasImpressao"] = valeIngressoTipo.ValidadeDiasImpressao.Valor;
                        linha["ValidadeData"] = valeIngressoTipo.ValidadeData.Valor;
                        linha["ClienteTipo"] = valeIngressoTipo.ClienteTipo.Valor;
                        linha["ProcedimentoTroca"] = valeIngressoTipo.ProcedimentoTroca.Valor;
                        linha["SaudacaoPadrao"] = valeIngressoTipo.SaudacaoPadrao.Valor;
                        linha["SaudacaoNominal"] = valeIngressoTipo.SaudacaoNominal.Valor;
                        linha["QuantidadeLimitada"] = valeIngressoTipo.QuantidadeLimitada.Valor;
                        linha["EmpresaID"] = valeIngressoTipo.EmpresaID.Valor;
                        linha["CodigoTrocaFixo"] = valeIngressoTipo.CodigoTrocaFixo.Valor;
                        linha["Acumulativo"] = valeIngressoTipo.Acumulativo.Valor;
                        linha["VersaoImagem"] = valeIngressoTipo.VersaoImagem.Valor;
                        linha["VersaoImagemInternet"] = valeIngressoTipo.VersaoImagemInternet.Valor;
                        linha["ReleaseInternet"] = valeIngressoTipo.ReleaseInternet.Valor;
                        linha["PublicarInternet"] = valeIngressoTipo.PublicarInternet.Valor;
                        linha["UltimoCodigoImpresso"] = valeIngressoTipo.UltimoCodigoImpresso.Valor;
                        linha["TrocaEntrega"] = valeIngressoTipo.TrocaEntrega.Valor;
                        linha["TrocaIngresso"] = valeIngressoTipo.TrocaIngresso.Valor;
                        linha["TrocaConveniencia"] = valeIngressoTipo.TrocaConveniencia.Valor;
                        linha["ValorTipo"] = valeIngressoTipo.ValorTipo.Valor;
                        linha["ValorPagamento"] = valeIngressoTipo.ValorPagamento.Valor;
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

        /// <summary>
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "ValidadeDiasImpressao":
                        sql = "SELECT ID, ValidadeDiasImpressao FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ValidadeDiasImpressao";
                        break;
                    case "ValidadeData":
                        sql = "SELECT ID, ValidadeData FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ValidadeData";
                        break;
                    case "ClienteTipo":
                        sql = "SELECT ID, ClienteTipo FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ClienteTipo";
                        break;
                    case "ProcedimentoTroca":
                        sql = "SELECT ID, ProcedimentoTroca FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ProcedimentoTroca";
                        break;
                    case "SaudacaoPadrao":
                        sql = "SELECT ID, SaudacaoPadrao FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY SaudacaoPadrao";
                        break;
                    case "SaudacaoNominal":
                        sql = "SELECT ID, SaudacaoNominal FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY SaudacaoNominal";
                        break;
                    case "QuantidadeLimitada":
                        sql = "SELECT ID, QuantidadeLimitada FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY QuantidadeLimitada";
                        break;
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "CodigoTrocaFixo":
                        sql = "SELECT ID, CodigoTrocaFixo FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY CodigoTrocaFixo";
                        break;
                    case "Acumulativo":
                        sql = "SELECT ID, Acumulativo FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY Acumulativo";
                        break;
                    case "VersaoImagem":
                        sql = "SELECT ID, VersaoImagem FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY VersaoImagem";
                        break;
                    case "VersaoImagemInternet":
                        sql = "SELECT ID, VersaoImagemInternet FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY VersaoImagemInternet";
                        break;
                    case "ReleaseInternet":
                        sql = "SELECT ID, ReleaseInternet FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ReleaseInternet";
                        break;
                    case "PublicarInternet":
                        sql = "SELECT ID, PublicarInternet FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY PublicarInternet";
                        break;
                    case "UltimoCodigoImpresso":
                        sql = "SELECT ID, UltimoCodigoImpresso FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY UltimoCodigoImpresso";
                        break;
                    case "TrocaEntrega":
                        sql = "SELECT ID, TrocaEntrega FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY TrocaEntrega";
                        break;
                    case "TrocaIngresso":
                        sql = "SELECT ID, TrocaIngresso FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY TrocaIngresso";
                        break;
                    case "TrocaConveniencia":
                        sql = "SELECT ID, TrocaConveniencia FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY TrocaConveniencia";
                        break;
                    case "ValorTipo":
                        sql = "SELECT ID, ValorTipo FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ValorTipo";
                        break;
                    case "ValorPagamento":
                        sql = "SELECT ID, ValorPagamento FROM tValeIngressoTipo WHERE " + FiltroSQL + " ORDER BY ValorPagamento";
                        break;
                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            try
            {

                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ValeIngressoTipoException"

    [Serializable]
    public class ValeIngressoTipoException : Exception
    {

        public ValeIngressoTipoException() : base() { }

        public ValeIngressoTipoException(string msg) : base(msg) { }

        public ValeIngressoTipoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}