

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "Canal_B"

    public abstract class Canal_B : BaseBD
    {


        public empresaid EmpresaID = new empresaid();
        public canaltipoid CanalTipoID = new canaltipoid();
        public nome Nome = new nome();
        public taxaconveniencia TaxaConveniencia = new taxaconveniencia();
        public taxacomissao TaxaComissao = new taxacomissao();
        public taxaminima TaxaMinima = new taxaminima();
        public taxamaxima TaxaMaxima = new taxamaxima();
        public comissaominima ComissaoMinima = new comissaominima();
        public comissaomaxima ComissaoMaxima = new comissaomaxima();
        public comprovantequantidade ComprovanteQuantidade = new comprovantequantidade();
        public tipovenda TipoVenda = new tipovenda();
        public opcaoimprimirsempreco OpcaoImprimirSemPreco = new opcaoimprimirsempreco();
        public cartao Cartao = new cartao();
        public naocartao NaoCartao = new naocartao();
        public obrigacadastrocliente ObrigaCadastroCliente = new obrigacadastrocliente();
        public obs Obs = new obs();
        public comissao Comissao = new comissao();
        public politicatroca PoliticaTroca = new politicatroca();
        public confirmacaoporemail ConfirmacaoPorEmail = new confirmacaoporemail();
        public obrigatoriedadeid ObrigatoriedadeID = new obrigatoriedadeid();
        public enviasms EnviaSms = new enviasms();
        public teff TEFF = new teff();
        public nroestabelecimento NroEstabelecimento = new nroestabelecimento();
        public nroestabelecimentoamex NroEstabelecimentoAmex = new nroestabelecimentoamex();
        public imprimetermica ImprimeTermica = new imprimetermica();
        public imprimeargox ImprimeArgox = new imprimeargox();
        public auttar Auttar = new auttar();
        public tipoimpressaotermica TipoImpressaoTermica = new tipoimpressaotermica();
        public ativo Ativo = new ativo();

        public Canal_B() { }

        // passar o Usuario logado no sistema
        public Canal_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Canal
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tCanal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CanalTipoID.ValorBD = bd.LerInt("CanalTipoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.TaxaMinima.ValorBD = bd.LerDecimal("TaxaMinima").ToString();
                    this.TaxaMaxima.ValorBD = bd.LerDecimal("TaxaMaxima").ToString();
                    this.ComissaoMinima.ValorBD = bd.LerDecimal("ComissaoMinima").ToString();
                    this.ComissaoMaxima.ValorBD = bd.LerDecimal("ComissaoMaxima").ToString();
                    this.ComprovanteQuantidade.ValorBD = bd.LerInt("ComprovanteQuantidade").ToString();
                    this.TipoVenda.ValorBD = bd.LerString("TipoVenda");
                    this.OpcaoImprimirSemPreco.ValorBD = bd.LerString("OpcaoImprimirSemPreco");
                    this.Cartao.ValorBD = bd.LerString("Cartao");
                    this.NaoCartao.ValorBD = bd.LerString("NaoCartao");
                    this.ObrigaCadastroCliente.ValorBD = bd.LerString("ObrigaCadastroCliente");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.PoliticaTroca.ValorBD = bd.LerString("PoliticaTroca");
                    this.ConfirmacaoPorEmail.ValorBD = bd.LerString("ConfirmacaoPorEmail");
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.EnviaSms.ValorBD = bd.LerString("EnviaSms");
                    this.ImprimeTermica.ValorBD = bd.LerString("ImprimeTermica");
                    this.ImprimeArgox.ValorBD = bd.LerString("ImprimeArgox");
                    this.TipoImpressaoTermica.ValorBD = bd.LerString("TipoImpressaoTermica");
                    this.TEFF.ValorBD = bd.LerString("TEFF");
                    this.NroEstabelecimento.ValorBD = bd.LerString("NroEstabelecimento");
                    this.NroEstabelecimentoAmex.ValorBD = bd.LerString("NroEstabelecimentoAmex");
                    this.Auttar.ValorBD = bd.LerString("Auttar");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
        /// Preenche todos os atributos de Canal do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCanal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CanalTipoID.ValorBD = bd.LerInt("CanalTipoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.TaxaMinima.ValorBD = bd.LerDecimal("TaxaMinima").ToString();
                    this.TaxaMaxima.ValorBD = bd.LerDecimal("TaxaMaxima").ToString();
                    this.ComissaoMinima.ValorBD = bd.LerDecimal("ComissaoMinima").ToString();
                    this.ComissaoMaxima.ValorBD = bd.LerDecimal("ComissaoMaxima").ToString();
                    this.ComprovanteQuantidade.ValorBD = bd.LerInt("ComprovanteQuantidade").ToString();
                    this.TipoVenda.ValorBD = bd.LerString("TipoVenda");
                    this.OpcaoImprimirSemPreco.ValorBD = bd.LerString("OpcaoImprimirSemPreco");
                    this.ImprimeTermica.ValorBD = bd.LerString("ImprimeTermica");
                    this.Cartao.ValorBD = bd.LerString("Cartao");
                    this.NaoCartao.ValorBD = bd.LerString("NaoCartao");
                    this.ObrigaCadastroCliente.ValorBD = bd.LerString("ObrigaCadastroCliente");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.PoliticaTroca.ValorBD = bd.LerString("PoliticaTroca");
                    this.ConfirmacaoPorEmail.ValorBD = bd.LerString("ConfirmacaoPorEmail");
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.EnviaSms.ValorBD = bd.LerString("EnviaSms");
                    this.TEFF.ValorBD = bd.LerString("TEFF");
                    this.Auttar.ValorBD = bd.LerString("Auttar");
                    this.ImprimeArgox.ValorBD = bd.LerString("ImprimeArgox");
                    this.TipoImpressaoTermica.ValorBD = bd.LerString("TipoImpressaoTermica");
                    this.NroEstabelecimento.ValorBD = bd.LerString("NroEstabelecimento");
                    this.NroEstabelecimentoAmex.ValorBD = bd.LerString("NroEstabelecimentoAmex");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
                sql.Append("INSERT INTO cCanal (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCanal (ID, Versao, EmpresaID, CanalTipoID, Nome, TaxaConveniencia, TaxaComissao, TaxaMinima, TaxaMaxima, ComissaoMinima, ComissaoMaxima, ComprovanteQuantidade, TipoVenda, OpcaoImprimirSemPreco, Cartao, NaoCartao, ObrigaCadastroCliente, Obs, Comissao, PoliticaTroca, ConfirmacaoPorEmail, ObrigatoriedadeID, EnviaSms, ImprimeTermica, TEFF, NroEstabelecimento, NroEstabelecimentoAmex, Ativo, ImprimeTermica, Auttar, ImprimeArgox, TipoImpressaoTermica) ");
                sql.Append("SELECT ID, @V, EmpresaID, CanalTipoID, Nome, TaxaConveniencia, TaxaComissao, TaxaMinima, TaxaMaxima, ComissaoMinima, ComissaoMaxima, ComprovanteQuantidade, TipoVenda, OpcaoImprimirSemPreco, Cartao, NaoCartao, ObrigaCadastroCliente, Obs, Comissao, PoliticaTroca, ConfirmacaoPorEmail, ObrigatoriedadeID, EnviaSms, ImprimeTermica, TEFF, NroEstabelecimento, NroEstabelecimentoAmex, Ativo, ImprimeTermica, Auttar, ImprimeArgox, TipoImpressaoTermica FROM tCanal WHERE ID = @I");
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
        /// Inserir novo(a) Canal
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCanal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCanal( ID,     EmpresaID,  CanalTipoID,    Nome,    TaxaConveniencia,   TaxaComissao,   TaxaMinima,    TaxaMaxima,     ComissaoMinima,     ComissaoMaxima,     ComprovanteQuantidade,  TipoVenda,      OpcaoImprimirSemPreco,      Cartao,     NaoCartao,      ObrigaCadastroCliente,      Obs,    Comissao,   PoliticaTroca,      ConfirmacaoPorEmail,    ObrigatoriedadeID,  EnviaSms,   ImprimeTermica,     TEFF,       NroEstabelecimento,     NroEstabelecimentoAmex,     Ativo,      TipoImpressaoTermica, Auttar, ImprimeArgox) ");
                sql.Append("VALUES (            @ID,    @EmpresaID, @CanalTipoID,   '@Nome', @TaxaConveniencia,  @TaxaComissao,  '@TaxaMinima', '@TaxaMaxima',  '@ComissaoMinima',  '@ComissaoMaxima',  @ComprovanteQuantidade, '@TipoVenda',   '@OpcaoImprimirSemPreco',   '@Cartao',  '@NaoCartao',   '@ObrigaCadastroCliente',   '@Obs', @Comissao,  '@PoliticaTroca',   '@ConfirmacaoPorEmail', @ObrigatoriedadeID, '@EnviaSms','@ImprimeTermica',  '@TEFF',    '@NroEstabelecimento',  '@NroEstabelecimentoAmex',  '@Ativo',   '@TipoImpressaoTermica', '@Auttar', '@ImprimeArgox')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@EmpresaID", this.EmpresaID.ValorBD);
                sql.Replace("@CanalTipoID", this.CanalTipoID.ValorBD);
                sql.Replace("@Nome", this.Nome.ValorBD);
                sql.Replace("@TaxaConveniencia", this.TaxaConveniencia.ValorBD);
                sql.Replace("@TaxaComissao", this.TaxaComissao.ValorBD);
                sql.Replace("@TaxaMinima", this.TaxaMinima.ValorBD);
                sql.Replace("@TaxaMaxima", this.TaxaMaxima.ValorBD);
                sql.Replace("@ComissaoMinima", this.ComissaoMinima.ValorBD);
                sql.Replace("@ComissaoMaxima", this.ComissaoMaxima.ValorBD);
                sql.Replace("@ComprovanteQuantidade", this.ComprovanteQuantidade.ValorBD);
                sql.Replace("@TipoVenda", this.TipoVenda.ValorBD);
                sql.Replace("@OpcaoImprimirSemPreco", this.OpcaoImprimirSemPreco.ValorBD);
                sql.Replace("@Cartao", this.Cartao.ValorBD);
                sql.Replace("@NaoCartao", this.NaoCartao.ValorBD);
                sql.Replace("@ObrigaCadastroCliente", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@Obs", this.Obs.ValorBD);
                sql.Replace("@Comissao", this.Comissao.ValorBD);
                sql.Replace("@PoliticaTroca", this.PoliticaTroca.ValorBD);
                sql.Replace("@ConfirmacaoPorEmail", this.ConfirmacaoPorEmail.ValorBD);
                sql.Replace("@ObrigatoriedadeID", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@EnviaSms", this.EnviaSms.ValorBD);
                sql.Replace("@ImprimeTermica", this.ImprimeTermica.ValorBD);
                sql.Replace("@TipoImpressaoTermica", this.TipoImpressaoTermica.ValorBD);
                sql.Replace("@TEFF", this.TEFF.ValorBD);
                sql.Replace("@NroEstabelecimento", this.NroEstabelecimento.ValorBD);
                sql.Replace("@NroEstabelecimentoAmex", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@Ativo", this.Ativo.ValorBD);
                sql.Replace("@Auttar", this.Auttar.ValorBD);
                sql.Replace("@ImprimeArgox", this.ImprimeArgox.ValorBD);

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
        /// Inserir novo(a) Canal
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCanal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCanal( ID,  EmpresaID,  CanalTipoID,    Nome,       TaxaConveniencia,   TaxaComissao,   TaxaMinima,     TaxaMaxima,     ComissaoMinima,     ComissaoMaxima,     ComprovanteQuantidade, TipoVenda,   OpcaoImprimirSemPreco,      Cartao,     NaoCartao,      ObrigaCadastroCliente,      Obs,    Comissao, PoliticaTroca,    ConfirmacaoPorEmail,    ObrigatoriedadeID, EnviaSms,    TEFF,   NroEstabelecimento,     NroEstabelecimentoAmex,     Ativo,      ImprimeTermica,     TipoImpressaoTermica,      Auttar,     ImprimeArgox) ");
                sql.Append("VALUES (            @ID, @EmpresaID, @CanalTipoID,   '@Nome',    @TaxaConveniencia,  @TaxaComissao,  '@TaxaMinima',  '@TaxaMaxima',  '@ComissaoMinima',  '@ComissaoMaxima',  @ComprovanteQuantidade,'@TipoVenda','@OpcaoImprimirSemPreco',   '@Cartao',  '@NaoCartao',   '@ObrigaCadastroCliente',   '@Obs', @Comissao,'@PoliticaTroca','@ConfirmacaoPorEmail',  @ObrigatoriedadeID,'@EnviaSms', '@TEFF','@NroEstabelecimento',  '@NroEstabelecimentoAmex',  '@Ativo',   @ImprimeTermica,    '@TipoImpressaoTermica',   '@Auttar',  '@ImprimeArgox')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@EmpresaID", this.EmpresaID.ValorBD);
                sql.Replace("@CanalTipoID", this.CanalTipoID.ValorBD);
                sql.Replace("@Nome", this.Nome.ValorBD);
                sql.Replace("@TaxaConveniencia", this.TaxaConveniencia.ValorBD);
                sql.Replace("@TaxaComissao", this.TaxaComissao.ValorBD);
                sql.Replace("@TaxaMinima", this.TaxaMinima.ValorBD);
                sql.Replace("@TaxaMaxima", this.TaxaMaxima.ValorBD);
                sql.Replace("@ComissaoMinima", this.ComissaoMinima.ValorBD);
                sql.Replace("@ComissaoMaxima", this.ComissaoMaxima.ValorBD);
                sql.Replace("@ComprovanteQuantidade", this.ComprovanteQuantidade.ValorBD);
                sql.Replace("@TipoVenda", this.TipoVenda.ValorBD);
                sql.Replace("@OpcaoImprimirSemPreco", this.OpcaoImprimirSemPreco.ValorBD);
                sql.Replace("@Cartao", this.Cartao.ValorBD);
                sql.Replace("@NaoCartao", this.NaoCartao.ValorBD);
                sql.Replace("@ObrigaCadastroCliente", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@Obs", this.Obs.ValorBD);
                sql.Replace("@Comissao", this.Comissao.ValorBD);
                sql.Replace("@PoliticaTroca", this.PoliticaTroca.ValorBD);
                sql.Replace("@ConfirmacaoPorEmail", this.ConfirmacaoPorEmail.ValorBD);
                sql.Replace("@ObrigatoriedadeID", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@EnviaSms", this.EnviaSms.ValorBD);
                sql.Replace("@TEFF", this.TEFF.ValorBD);
                sql.Replace("@NroEstabelecimento", this.NroEstabelecimento.ValorBD);
                sql.Replace("@NroEstabelecimentoAmex", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@Ativo", this.Ativo.ValorBD);
                sql.Replace("@ImprimeTermica", this.ImprimeTermica.ValorBD);
                sql.Replace("@TipoImpressaoTermica", this.TipoImpressaoTermica.ValorBD);
                sql.Replace("@Auttar", this.Auttar.ValorBD);
                sql.Replace("@ImprimeArgox", this.ImprimeArgox.ValorBD);
                

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
        /// Atualiza Canal
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCanal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tCanal SET EmpresaID = @EmpresaID, CanalTipoID = @CanalTipoID, Nome = '@Nome', TaxaConveniencia = @TaxaConveniencia, TaxaComissao = @TaxaComissao, TaxaMinima = '@TaxaMinima', TaxaMaxima = '@TaxaMaxima', ComissaoMinima = '@ComissaoMinima', ComissaoMaxima = '@ComissaoMaxima', ComprovanteQuantidade = @ComprovanteQuantidade, TipoVenda = '@TipoVenda', OpcaoImprimirSemPreco = '@OpcaoImprimirSemPreco', Cartao = '@Cartao', NaoCartao = '@NaoCartao', ObrigaCadastroCliente = '@ObrigaCadastroCliente', Obs = '@Obs', Comissao = @Comissao, PoliticaTroca = '@PoliticaTroca', ConfirmacaoPorEmail = '@ConfirmacaoPorEmail', ObrigatoriedadeID = @ObrigatoriedadeID, EnviaSms = '@EnviaSms', TEFF = '@TEFF', NroEstabelecimento = '@NroEstabelecimento', NroEstabelecimentoAmex = '@NroEstabelecimentoAmex', Ativo = '@Ativo', ImprimeTermica = '@ImprimeTermica', TipoImpressaoTermica = '@TipoImpressaoTermica', Auttar = '@Auttar', ImprimeArgox = '@ImprimeArgox' ");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@EmpresaID", this.EmpresaID.ValorBD);
                sql.Replace("@CanalTipoID", this.CanalTipoID.ValorBD);
                sql.Replace("@Nome", this.Nome.ValorBD);
                sql.Replace("@TaxaConveniencia", this.TaxaConveniencia.ValorBD);
                sql.Replace("@TaxaComissao", this.TaxaComissao.ValorBD);
                sql.Replace("@TaxaMinima", this.TaxaMinima.ValorBD);
                sql.Replace("@TaxaMaxima", this.TaxaMaxima.ValorBD);
                sql.Replace("@ComissaoMinima", this.ComissaoMinima.ValorBD);
                sql.Replace("@ComissaoMaxima", this.ComissaoMaxima.ValorBD);
                sql.Replace("@ComprovanteQuantidade", this.ComprovanteQuantidade.ValorBD);
                sql.Replace("@TipoVenda", this.TipoVenda.ValorBD);
                sql.Replace("@OpcaoImprimirSemPreco", this.OpcaoImprimirSemPreco.ValorBD);
                sql.Replace("@Cartao", this.Cartao.ValorBD);
                sql.Replace("@NaoCartao", this.NaoCartao.ValorBD);
                sql.Replace("@ObrigaCadastroCliente", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@Obs", this.Obs.ValorBD);
                sql.Replace("@Comissao", this.Comissao.ValorBD);
                sql.Replace("@PoliticaTroca", this.PoliticaTroca.ValorBD);
                sql.Replace("@ConfirmacaoPorEmail", this.ConfirmacaoPorEmail.ValorBD);
                sql.Replace("@ObrigatoriedadeID", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@EnviaSms", this.EnviaSms.ValorBD);
                sql.Replace("@TEFF", this.TEFF.ValorBD);
                sql.Replace("@NroEstabelecimento", this.NroEstabelecimento.ValorBD);
                sql.Replace("@NroEstabelecimentoAmex", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@Ativo", this.Ativo.ValorBD);
                sql.Replace("@ImprimeTermica", this.ImprimeTermica.ValorBD);
                sql.Replace("@TipoImpressaoTermica", this.TipoImpressaoTermica.ValorBD);
                sql.Replace("@Auttar", this.Auttar.ValorBD);
                sql.Replace("@ImprimeArgox", this.ImprimeArgox.ValorBD);

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
        /// Atualiza Canal
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCanal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tCanal SET EmpresaID = @EmpresaID, CanalTipoID = @CanalTipoID, Nome = '@Nome', TaxaConveniencia = @TaxaConveniencia, TaxaComissao = @TaxaComissao, TaxaMinima = '@TaxaMinima', TaxaMaxima = '@TaxaMaxima', ComissaoMinima = '@ComissaoMinima', ComissaoMaxima = '@ComissaoMaxima', ComprovanteQuantidade = @ComprovanteQuantidade, TipoVenda = '@TipoVenda', OpcaoImprimirSemPreco = '@OpcaoImprimirSemPreco', Cartao = '@Cartao', NaoCartao = '@NaoCartao', ObrigaCadastroCliente = '@ObrigaCadastroCliente', Obs = '@Obs', Comissao = @Comissao, PoliticaTroca = '@PoliticaTroca', ConfirmacaoPorEmail = '@ConfirmacaoPorEmail', ObrigatoriedadeID = @ObrigatoriedadeID, EnviaSms = '@EnviaSms', TEFF = '@TEFF', NroEstabelecimento = '@NroEstabelecimento', NroEstabelecimentoAmex = '@NroEstabelecimentoAmex', Ativo = '@Ativo', ImprimeTermica = '@ImprimeTermica', TipoImpressaoTermica = '@TipoImpressaoTermica', Auttar = '@Auttar', ImprimeArgox = '@ImprimeArgox' ");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@EmpresaID", this.EmpresaID.ValorBD);
                sql.Replace("@CanalTipoID", this.CanalTipoID.ValorBD);
                sql.Replace("@Nome", this.Nome.ValorBD);
                sql.Replace("@TaxaConveniencia", this.TaxaConveniencia.ValorBD);
                sql.Replace("@TaxaComissao", this.TaxaComissao.ValorBD);
                sql.Replace("@TaxaMinima", this.TaxaMinima.ValorBD);
                sql.Replace("@TaxaMaxima", this.TaxaMaxima.ValorBD);
                sql.Replace("@ComissaoMinima", this.ComissaoMinima.ValorBD);
                sql.Replace("@ComissaoMaxima", this.ComissaoMaxima.ValorBD);
                sql.Replace("@ComprovanteQuantidade", this.ComprovanteQuantidade.ValorBD);
                sql.Replace("@TipoVenda", this.TipoVenda.ValorBD);
                sql.Replace("@OpcaoImprimirSemPreco", this.OpcaoImprimirSemPreco.ValorBD);
                sql.Replace("@Cartao", this.Cartao.ValorBD);
                sql.Replace("@NaoCartao", this.NaoCartao.ValorBD);
                sql.Replace("@ObrigaCadastroCliente", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@Obs", this.Obs.ValorBD);
                sql.Replace("@Comissao", this.Comissao.ValorBD);
                sql.Replace("@PoliticaTroca", this.PoliticaTroca.ValorBD);
                sql.Replace("@ConfirmacaoPorEmail", this.ConfirmacaoPorEmail.ValorBD);
                sql.Replace("@ObrigatoriedadeID", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@EnviaSms", this.EnviaSms.ValorBD);
                sql.Replace("@TEFF", this.TEFF.ValorBD);
                sql.Replace("@NroEstabelecimento", this.NroEstabelecimento.ValorBD);
                sql.Replace("@NroEstabelecimentoAmex", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@Ativo", this.Ativo.ValorBD);
                sql.Replace("@ImprimeTermica", this.ImprimeTermica.ValorBD);
                sql.Replace("@TipoImpressaoTermica", this.TipoImpressaoTermica.ValorBD);
                sql.Replace("@Auttar", this.Auttar.ValorBD);
                sql.Replace("@ImprimeArgox", this.ImprimeArgox.ValorBD);

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
        /// Exclui Canal com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanal WHERE ID=" + id;

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
        /// Exclui Canal com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {
            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanal WHERE ID=" + id;

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
        /// Exclui Canal
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


            this.EmpresaID.Limpar();

            this.CanalTipoID.Limpar();

            this.Nome.Limpar();

            this.TaxaConveniencia.Limpar();

            this.TaxaComissao.Limpar();

            this.TaxaMinima.Limpar();

            this.TaxaMaxima.Limpar();

            this.ComissaoMinima.Limpar();

            this.ComissaoMaxima.Limpar();

            this.ComprovanteQuantidade.Limpar();

            this.TipoVenda.Limpar();

            this.OpcaoImprimirSemPreco.Limpar();

            this.Cartao.Limpar();

            this.NaoCartao.Limpar();

            this.ObrigaCadastroCliente.Limpar();

            this.Obs.Limpar();

            this.Comissao.Limpar();

            this.PoliticaTroca.Limpar();

            this.ConfirmacaoPorEmail.Limpar();

            this.ObrigatoriedadeID.Limpar();

            this.EnviaSms.Limpar();

            this.TEFF.Limpar();

            this.NroEstabelecimento.Limpar();

            this.NroEstabelecimentoAmex.Limpar();

            this.ImprimeTermica.Limpar();
            this.ImprimeArgox.Limpar();

            this.Auttar.Limpar();

            this.TipoImpressaoTermica.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.EmpresaID.Desfazer();

            this.CanalTipoID.Desfazer();

            this.Nome.Desfazer();

            this.TaxaConveniencia.Desfazer();

            this.TaxaComissao.Desfazer();

            this.TaxaMinima.Desfazer();

            this.TaxaMaxima.Desfazer();

            this.ComissaoMinima.Desfazer();

            this.ComissaoMaxima.Desfazer();

            this.ComprovanteQuantidade.Desfazer();

            this.TipoVenda.Desfazer();

            this.OpcaoImprimirSemPreco.Desfazer();

            this.Cartao.Desfazer();

            this.NaoCartao.Desfazer();

            this.ObrigaCadastroCliente.Desfazer();

            this.Obs.Desfazer();

            this.Comissao.Desfazer();

            this.PoliticaTroca.Desfazer();

            this.ConfirmacaoPorEmail.Desfazer();

            this.ObrigatoriedadeID.Desfazer();

            this.EnviaSms.Desfazer();

            this.ImprimeTermica.Desfazer();
            this.TipoImpressaoTermica.Desfazer();

            this.ImprimeArgox.Desfazer();

            this.Auttar.Desfazer();

            this.TEFF.Desfazer();

            this.NroEstabelecimento.Desfazer();

            this.NroEstabelecimentoAmex.Desfazer();

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


        public class canaltipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalTipoID";
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
                    return 46;
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


        public class taxaconveniencia : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConveniencia";
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


        public class taxacomissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaComissao";
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


        public class taxaminima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaMinima";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class taxamaxima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaMaxima";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class comissaominima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoMinima";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class comissaomaxima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoMaxima";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class comprovantequantidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComprovanteQuantidade";
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


        public class tipovenda : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoVenda";
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


        public class opcaoimprimirsempreco : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "OpcaoImprimirSemPreco";
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


        public class cartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cartao";
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


        public class naocartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NaoCartao";
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


        public class obrigacadastrocliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObrigaCadastroCliente";
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


        public class obs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Obs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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


        public class comissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Comissao";
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


        public class politicatroca : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PoliticaTroca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2000;
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


        public class confirmacaoporemail : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ConfirmacaoPorEmail";
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


        public class obrigatoriedadeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObrigatoriedadeID";
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


        public class enviasms : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnviaSms";
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

        public class imprimetermica : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImprimeTermica";
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

        public class imprimeargox : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImprimeArgox";
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

        public class auttar : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImprimeTermica";
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

        public class tipoimpressaotermica : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoImpressaoTermica";
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



        public class teff : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TEFF";
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


        public class nroestabelecimento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroEstabelecimento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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

        public class nroestabelecimentoamex : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroEstabelecimentoAmex";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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

        public class ativo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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
                return base.Valor.ToString();
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

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("EmpresaID", typeof(int));

                tabela.Columns.Add("CanalTipoID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("TaxaConveniencia", typeof(int));

                tabela.Columns.Add("TaxaComissao", typeof(int));

                tabela.Columns.Add("TaxaMinima", typeof(decimal));

                tabela.Columns.Add("TaxaMaxima", typeof(decimal));

                tabela.Columns.Add("ComissaoMinima", typeof(decimal));

                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                tabela.Columns.Add("ComprovanteQuantidade", typeof(int));

                tabela.Columns.Add("TipoVenda", typeof(string));

                tabela.Columns.Add("OpcaoImprimirSemPreco", typeof(bool));

                tabela.Columns.Add("Cartao", typeof(string));

                tabela.Columns.Add("NaoCartao", typeof(string));

                tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));

                tabela.Columns.Add("Obs", typeof(string));

                tabela.Columns.Add("Comissao", typeof(int));

                tabela.Columns.Add("PoliticaTroca", typeof(string));

                tabela.Columns.Add("ConfirmacaoPorEmail", typeof(bool));

                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));

                tabela.Columns.Add("EnviaSms", typeof(bool));

                tabela.Columns.Add("TEFF", typeof(string));

                tabela.Columns.Add("NroEstabelecimento", typeof(string));

                tabela.Columns.Add("ImprimeTermica", typeof(bool));

                tabela.Columns.Add("ImprimeArgox", typeof(bool));

                tabela.Columns.Add("TipoImpressaoTermica", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Todos();
        public abstract DataTable Categorias();
        public abstract DataTable Lojas();
        public abstract DataTable Eventos();
        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);
        public abstract DataTable LinhasVendasGerenciais(string ingressologids);
        public abstract int QuantidadeIngressosPorCanal(string ingressologids);
        public abstract decimal ValorIngressosPorCanal(string ingressologids);
        public abstract DataTable Caixas(string registrozero);
        public abstract DataTable Caixas(string registrozero, int usuarioid);

    }
    #endregion

    #region "CanalLista_B"


    public abstract class CanalLista_B : BaseLista
    {

        private bool backup = false;
        protected Canal canal;

        // passar o Usuario logado no sistema
        public CanalLista_B()
        {
            canal = new Canal();
        }

        // passar o Usuario logado no sistema
        public CanalLista_B(int usuarioIDLogado)
        {
            canal = new Canal(usuarioIDLogado);
        }

        public Canal Canal
        {
            get { return canal; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Canal especifico
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
                    canal.Ler(id);
                    return canal;
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
                    sql = "SELECT ID FROM tCanal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanal";

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
                    sql = "SELECT ID FROM tCanal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanal";

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
                    sql = "SELECT ID FROM xCanal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCanal";

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
        /// Preenche Canal corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    canal.Ler(id);
                else
                    canal.LerBackup(id);

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

                bool ok = canal.Excluir();
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
        /// Inseri novo(a) Canal na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = canal.Inserir();
                if (ok)
                {
                    lista.Add(canal.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Canal carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("EmpresaID", typeof(int));

                tabela.Columns.Add("CanalTipoID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("TaxaConveniencia", typeof(int));

                tabela.Columns.Add("TaxaComissao", typeof(int));

                tabela.Columns.Add("TaxaMinima", typeof(decimal));

                tabela.Columns.Add("TaxaMaxima", typeof(decimal));

                tabela.Columns.Add("ComissaoMinima", typeof(decimal));

                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                tabela.Columns.Add("ComprovanteQuantidade", typeof(int));

                tabela.Columns.Add("TipoVenda", typeof(string));

                tabela.Columns.Add("OpcaoImprimirSemPreco", typeof(bool));

                tabela.Columns.Add("Cartao", typeof(string));

                tabela.Columns.Add("NaoCartao", typeof(string));

                tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));

                tabela.Columns.Add("Obs", typeof(string));

                tabela.Columns.Add("Comissao", typeof(int));

                tabela.Columns.Add("PoliticaTroca", typeof(string));

                tabela.Columns.Add("ConfirmacaoPorEmail", typeof(bool));

                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));

                tabela.Columns.Add("EnviaSms", typeof(bool));

                tabela.Columns.Add("TEFF", typeof(string));

                tabela.Columns.Add("NroEstabelecimento", typeof(string));

                tabela.Columns.Add("ImprimeTermica", typeof(bool));

                tabela.Columns.Add("TipoImpressaoTermica", typeof(bool));

                tabela.Columns.Add("ImprimeArgox", typeof(bool));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = canal.Control.ID;

                        linha["EmpresaID"] = canal.EmpresaID.Valor;

                        linha["CanalTipoID"] = canal.CanalTipoID.Valor;

                        linha["Nome"] = canal.Nome.Valor;

                        linha["TaxaConveniencia"] = canal.TaxaConveniencia.Valor;

                        linha["TaxaComissao"] = canal.TaxaComissao.Valor;

                        linha["TaxaMinima"] = canal.TaxaMinima.Valor;

                        linha["TaxaMaxima"] = canal.TaxaMaxima.Valor;

                        linha["ComissaoMinima"] = canal.ComissaoMinima.Valor;

                        linha["ComissaoMaxima"] = canal.ComissaoMaxima.Valor;

                        linha["ComprovanteQuantidade"] = canal.ComprovanteQuantidade.Valor;

                        linha["TipoVenda"] = canal.TipoVenda.Valor;

                        linha["OpcaoImprimirSemPreco"] = canal.OpcaoImprimirSemPreco.Valor;

                        linha["Cartao"] = canal.Cartao.Valor;

                        linha["NaoCartao"] = canal.NaoCartao.Valor;

                        linha["ObrigaCadastroCliente"] = canal.ObrigaCadastroCliente.Valor;

                        linha["Obs"] = canal.Obs.Valor;

                        linha["Comissao"] = canal.Comissao.Valor;

                        linha["PoliticaTroca"] = canal.PoliticaTroca.Valor;

                        linha["ConfirmacaoPorEmail"] = canal.ConfirmacaoPorEmail.Valor;

                        linha["ObrigatoriedadeID"] = canal.ObrigatoriedadeID.Valor;

                        linha["EnviaSms"] = canal.EnviaSms.Valor;

                        linha["TEFF"] = canal.TEFF.Valor;

                        linha["NroEstabelecimento"] = canal.NroEstabelecimento.Valor;

                        linha["ImprimeTermica"] = canal.ImprimeTermica.Valor;

                        linha["TipoImpressaoTermica"] = canal.TipoImpressaoTermica.Valor;

                        linha["ImprimeArgox"] = canal.ImprimeArgox.Valor;

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

                DataTable tabela = new DataTable("RelatorioCanal");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("EmpresaID", typeof(int));

                    tabela.Columns.Add("CanalTipoID", typeof(int));

                    tabela.Columns.Add("Nome", typeof(string));

                    tabela.Columns.Add("TaxaConveniencia", typeof(int));

                    tabela.Columns.Add("TaxaComissao", typeof(int));

                    tabela.Columns.Add("TaxaMinima", typeof(decimal));

                    tabela.Columns.Add("TaxaMaxima", typeof(decimal));

                    tabela.Columns.Add("ComissaoMinima", typeof(decimal));

                    tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                    tabela.Columns.Add("ComprovanteQuantidade", typeof(int));

                    tabela.Columns.Add("TipoVenda", typeof(string));

                    tabela.Columns.Add("OpcaoImprimirSemPreco", typeof(bool));

                    tabela.Columns.Add("Cartao", typeof(string));

                    tabela.Columns.Add("NaoCartao", typeof(string));

                    tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));

                    tabela.Columns.Add("Obs", typeof(string));

                    tabela.Columns.Add("Comissao", typeof(int));

                    tabela.Columns.Add("PoliticaTroca", typeof(string));

                    tabela.Columns.Add("ConfirmacaoPorEmail", typeof(bool));

                    tabela.Columns.Add("ObrigatoriedadeID", typeof(int));

                    tabela.Columns.Add("EnviaSms", typeof(bool));

                    tabela.Columns.Add("TEFF", typeof(string));

                    tabela.Columns.Add("NroEstabelecimento", typeof(string));

                    tabela.Columns.Add("ImprimeTermica", typeof(bool));

                    tabela.Columns.Add("TipoImpressaoTermica", typeof(bool));

                    tabela.Columns.Add("ImprimeArgox", typeof(bool));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["EmpresaID"] = canal.EmpresaID.Valor;

                        linha["CanalTipoID"] = canal.CanalTipoID.Valor;

                        linha["Nome"] = canal.Nome.Valor;

                        linha["TaxaConveniencia"] = canal.TaxaConveniencia.Valor;

                        linha["TaxaComissao"] = canal.TaxaComissao.Valor;

                        linha["TaxaMinima"] = canal.TaxaMinima.Valor;

                        linha["TaxaMaxima"] = canal.TaxaMaxima.Valor;

                        linha["ComissaoMinima"] = canal.ComissaoMinima.Valor;

                        linha["ComissaoMaxima"] = canal.ComissaoMaxima.Valor;

                        linha["ComprovanteQuantidade"] = canal.ComprovanteQuantidade.Valor;

                        linha["TipoVenda"] = canal.TipoVenda.Valor;

                        linha["OpcaoImprimirSemPreco"] = canal.OpcaoImprimirSemPreco.Valor;

                        linha["Cartao"] = canal.Cartao.Valor;

                        linha["NaoCartao"] = canal.NaoCartao.Valor;

                        linha["ObrigaCadastroCliente"] = canal.ObrigaCadastroCliente.Valor;

                        linha["Obs"] = canal.Obs.Valor;

                        linha["Comissao"] = canal.Comissao.Valor;

                        linha["PoliticaTroca"] = canal.PoliticaTroca.Valor;

                        linha["ConfirmacaoPorEmail"] = canal.ConfirmacaoPorEmail.Valor;

                        linha["ObrigatoriedadeID"] = canal.ObrigatoriedadeID.Valor;

                        linha["EnviaSms"] = canal.EnviaSms.Valor;

                        linha["TEFF"] = canal.TEFF.Valor;

                        linha["NroEstabelecimento"] = canal.NroEstabelecimento.Valor;

                        linha["ImprimeTermica"] = canal.ImprimeTermica.Valor;

                        linha["TipoImpressaoTermica"] = canal.TipoImpressaoTermica.Valor;

                        linha["ImprimeArgox"] = canal.ImprimeArgox.Valor;

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

                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tCanal WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;

                    case "CanalTipoID":
                        sql = "SELECT ID, CanalTipoID FROM tCanal WHERE " + FiltroSQL + " ORDER BY CanalTipoID";
                        break;

                    case "Nome":
                        sql = "SELECT ID, Nome FROM tCanal WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;

                    case "TaxaConveniencia":
                        sql = "SELECT ID, TaxaConveniencia FROM tCanal WHERE " + FiltroSQL + " ORDER BY TaxaConveniencia";
                        break;

                    case "TaxaComissao":
                        sql = "SELECT ID, TaxaComissao FROM tCanal WHERE " + FiltroSQL + " ORDER BY TaxaComissao";
                        break;

                    case "TaxaMinima":
                        sql = "SELECT ID, TaxaMinima FROM tCanal WHERE " + FiltroSQL + " ORDER BY TaxaMinima";
                        break;

                    case "TaxaMaxima":
                        sql = "SELECT ID, TaxaMaxima FROM tCanal WHERE " + FiltroSQL + " ORDER BY TaxaMaxima";
                        break;

                    case "ComissaoMinima":
                        sql = "SELECT ID, ComissaoMinima FROM tCanal WHERE " + FiltroSQL + " ORDER BY ComissaoMinima";
                        break;

                    case "ComissaoMaxima":
                        sql = "SELECT ID, ComissaoMaxima FROM tCanal WHERE " + FiltroSQL + " ORDER BY ComissaoMaxima";
                        break;

                    case "ComprovanteQuantidade":
                        sql = "SELECT ID, ComprovanteQuantidade FROM tCanal WHERE " + FiltroSQL + " ORDER BY ComprovanteQuantidade";
                        break;

                    case "TipoVenda":
                        sql = "SELECT ID, TipoVenda FROM tCanal WHERE " + FiltroSQL + " ORDER BY TipoVenda";
                        break;

                    case "OpcaoImprimirSemPreco":
                        sql = "SELECT ID, OpcaoImprimirSemPreco FROM tCanal WHERE " + FiltroSQL + " ORDER BY OpcaoImprimirSemPreco";
                        break;

                    case "Cartao":
                        sql = "SELECT ID, Cartao FROM tCanal WHERE " + FiltroSQL + " ORDER BY Cartao";
                        break;

                    case "NaoCartao":
                        sql = "SELECT ID, NaoCartao FROM tCanal WHERE " + FiltroSQL + " ORDER BY NaoCartao";
                        break;

                    case "ObrigaCadastroCliente":
                        sql = "SELECT ID, ObrigaCadastroCliente FROM tCanal WHERE " + FiltroSQL + " ORDER BY ObrigaCadastroCliente";
                        break;

                    case "Obs":
                        sql = "SELECT ID, Obs FROM tCanal WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;

                    case "Comissao":
                        sql = "SELECT ID, Comissao FROM tCanal WHERE " + FiltroSQL + " ORDER BY Comissao";
                        break;

                    case "PoliticaTroca":
                        sql = "SELECT ID, PoliticaTroca FROM tCanal WHERE " + FiltroSQL + " ORDER BY PoliticaTroca";
                        break;

                    case "ConfirmacaoPorEmail":
                        sql = "SELECT ID, ConfirmacaoPorEmail FROM tCanal WHERE " + FiltroSQL + " ORDER BY ConfirmacaoPorEmail";
                        break;

                    case "ObrigatoriedadeID":
                        sql = "SELECT ID, ObrigatoriedadeID FROM tCanal WHERE " + FiltroSQL + " ORDER BY ObrigatoriedadeID";
                        break;

                    case "EnviaSms":
                        sql = "SELECT ID, EnviaSms FROM tCanal WHERE " + FiltroSQL + " ORDER BY EnviaSms";
                        break;

                    case "TEFF":
                        sql = "SELECT ID, TEFF FROM tCanal WHERE " + FiltroSQL + " ORDER BY TEFF";
                        break;

                    case "NroEstabelecimento":
                        sql = "SELECT ID, NroEstabelecimento FROM tCanal WHERE " + FiltroSQL + " ORDER BY NroEstabelecimento";
                        break;

                    case "ImprimeTermica":
                        sql = "SELECT ID, ImprimeTermica FROM tCanal WHERE " + FiltroSQL + " ORDER BY ImprimeTermica";
                        break;

                    case "TipoImpressaoTermica":
                        sql = "SELECT ID, TipoImpressaoTermica FROM tCanal WHERE " + FiltroSQL + " ORDER BY TipoImpressaoTermica";
                        break;

                    case "ImprimeArgox":
                        sql = "SELECT ID, ImprimeArgox FROM tCanal WHERE " + FiltroSQL + " ORDER BY ImprimeArgox";
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

    #region "CanalException"

    [Serializable]
    public class CanalException : Exception
    {

        public CanalException() : base() { }

        public CanalException(string msg) : base(msg) { }

        public CanalException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}