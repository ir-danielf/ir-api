/* ---------------------------------------------------------------
-- Arquivo Canal.cs
-- Gerado em: segunda-feira, 28 de fevereiro de 2005
-- Autor: Celeritas Ltda
---------------------------------------------------------------- */

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;

namespace IRLib
{

    public class Canal : Canal_B
    {

        public Obrigatoriedade Obrigatoriedade { get; set; }
        public enum ResponsavelValores
        {
            [System.ComponentModel.Description("Cliente")]
            Cliente = 'C',
            [System.ComponentModel.Description("Ingresso Rápido")]
            IngressoRapido = 'I',
            [System.ComponentModel.Description("Regional responsável pelo canal")]
            Regional = 'R'
        }

        public enum TipoCadastroObrigatorio
        {
            [System.ComponentModel.Description("Cadastro não obrigatório")]
            CadastroNaoObrigatorio = 'F',
            [System.ComponentModel.Description("Cadastro para entrega")]
            CadastroEntrega = 'E',
            [System.ComponentModel.Description("Cadastro básico")]
            CadastroBasico = 'B',
            [System.ComponentModel.Description("Cadastro único")]
            CadastroUnico = 'U',
            [System.ComponentModel.Description("Cadastro Fora Padrão")]
            CadastroForaPadrao = 'P'
        }

        public enum TipoDeVenda
        {
            [System.ComponentModel.Description("Venda remota")]
            VendaRemota = 'F',
            [System.ComponentModel.Description("Impressão de ingresso")]
            ImpressaoIngresso = 'T',
            [System.ComponentModel.Description("Impressão de voucher")]
            ImpressaoVoucher = 'V'
        }

        public enum TipodeImpressaoTermica
        {
            [System.ComponentModel.Description("Ingresso")]
            Ingresso = 'I',
            [System.ComponentModel.Description("Comprovante")]
            Comprovante = 'C',
        }

        public enum TEFTipos
        {
            [System.ComponentModel.Description("TEF Digitado")]
            Digitado = 'D',
            [System.ComponentModel.Description("TEF Com Cartão")]
            CartaoPresente = 'C',
            [System.ComponentModel.Description("Sem TEF")]
            SemTef = 'S'
        }

        public EstruturaObrigatoriedade EstruturaObrigatoriedade { get; set; }


        //public static int CANAL_INTERNET = new ConfigGerenciador().getCanalInternetID();
        //public static int CANAL_MOBILE = new ConfigGerenciador().getCanalMobileID();
        public static int CANAL_CALL_CENTER = new ConfigGerenciador().getCanalCallCenterID();

        public static int CANAL_INTERNET
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CanalInternet"]))
                    return 2;
                else
                    return Convert.ToInt32(ConfigurationManager.AppSettings["CanalInternet"]);

            }
        }

        public Canal() { }

        public Canal(int usuarioIDLogado) : base(usuarioIDLogado) { }

        protected void InserirControle(BD bd, string acao)
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

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCanal (ID, Versao, EmpresaID, CanalTipoID, Nome, TaxaConveniencia, TaxaComissao, TaxaMinima, TaxaMaxima, ComissaoMinima, ComissaoMaxima, ComprovanteQuantidade, TipoVenda, OpcaoImprimirSemPreco, Cartao, NaoCartao, ObrigaCadastroCliente, Obs, Comissao, PoliticaTroca, ConfirmacaoPorEmail, ObrigatoriedadeID, EnviaSms , TEFF, NroEstabelecimento, NroEstabelecimentoAmex, Ativo) ");
                sql.Append("SELECT ID, @V, EmpresaID, CanalTipoID, Nome, TaxaConveniencia, TaxaComissao, TaxaMinima, TaxaMaxima, ComissaoMinima, ComissaoMaxima, ComprovanteQuantidade, TipoVenda, OpcaoImprimirSemPreco, Cartao, NaoCartao, ObrigaCadastroCliente, Obs, Comissao, PoliticaTroca, ConfirmacaoPorEmail, ObrigatoriedadeID, EnviaSms, TEFF, NroEstabelecimento, NroEstabelecimentoAmex, Ativo FROM tCanal WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        ///// <summary>
        ///// Inserir novo(a) Canal -- ANTES DE Incluir - Passar a estrutura Propriedade
        ///// </summary>
        ///// <returns></returns>	
        public bool Inserir(BD bd)
        {
            try
            {
                if (this.Obrigatoriedade != null)
                {
                    this.Obrigatoriedade.Inserir(bd);
                    this.ObrigatoriedadeID.Valor = this.Obrigatoriedade.Control.ID;
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCanal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCanal(ID, EmpresaID, CanalTipoID, Nome, TaxaConveniencia, TaxaComissao, TaxaMinima, TaxaMaxima, ComissaoMinima, ComissaoMaxima, ComprovanteQuantidade, TipoVenda, OpcaoImprimirSemPreco, Cartao, NaoCartao, ObrigaCadastroCliente, Obs, Comissao, PoliticaTroca, ConfirmacaoPorEmail, ObrigatoriedadeID, EnviaSms , TEFF, NroEstabelecimento, NroEstabelecimentoAmex, Ativo, ImprimeTermica, ImprimeArgox ) ");
                sql.Append("VALUES (@ID,@001,@002,'@003',@004,@005,'@006','@007','@008','@009',@010,'@011','@012','@013','@014','@015','@016',@017,'@018','@019',@020,'@021', '@022', '@023', '@025', '@024', '@026', '@027')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.CanalTipoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaComissao.ValorBD);
                sql.Replace("@006", this.TaxaMinima.ValorBD);
                sql.Replace("@007", this.TaxaMaxima.ValorBD);
                sql.Replace("@008", this.ComissaoMinima.ValorBD);
                sql.Replace("@009", this.ComissaoMaxima.ValorBD);
                sql.Replace("@010", this.ComprovanteQuantidade.ValorBD);
                sql.Replace("@011", this.TipoVenda.ValorBD);
                sql.Replace("@012", this.OpcaoImprimirSemPreco.ValorBD);
                sql.Replace("@013", this.Cartao.ValorBD);
                sql.Replace("@014", this.NaoCartao.ValorBD);
                sql.Replace("@015", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@016", this.Obs.ValorBD);
                sql.Replace("@017", this.Comissao.ValorBD);
                sql.Replace("@018", this.PoliticaTroca.ValorBD);
                sql.Replace("@019", this.ConfirmacaoPorEmail.ValorBD);
                sql.Replace("@020", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@021", this.EnviaSms.ValorBD);
                sql.Replace("@022", this.TEFF.ValorBD);
                sql.Replace("@023", this.NroEstabelecimento.ValorBD);
                sql.Replace("@025", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@024", this.Ativo.ValorBD);
                sql.Replace("@026", this.ImprimeTermica.ValorBD);
                sql.Replace("@027", this.ImprimeArgox.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle(bd, "I");

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

                int obrigatoriedadeID = Convert.ToInt32(this.ObrigatoriedadeID.ValorBD);
                if (obrigatoriedadeID != 0 && this.Obrigatoriedade != null)
                {
                    this.Obrigatoriedade.Control.ID = Convert.ToInt32(this.ObrigatoriedadeID.ValorBD);
                    this.Obrigatoriedade.Atualizar(bd);
                }
                else if (this.Obrigatoriedade != null)
                {
                    this.Obrigatoriedade.Inserir();
                    this.ObrigatoriedadeID.Valor = this.Obrigatoriedade.Control.ID;
                }

                string sqlVersion = "SELECT MAX(Versao) FROM cCanal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanal SET EmpresaID = @EmpresaID, CanalTipoID = @CanalTipoID, Nome = '@Nome', TaxaConveniencia = @TaxaConveniencia, TaxaComissao = @TaxaComissao, TaxaMinima = '@TaxaMinima', TaxaMaxima = '@TaxaMaxima', ComissaoMinima = '@ComissaoMinima', ComissaoMaxima = '@ComissaoMaxima', ComprovanteQuantidade = @ComprovanteQuantidade, TipoVenda = '@TipoVenda', OpcaoImprimirSemPreco = '@OpcaoImprimirSemPreco', Cartao = '@Cartao', NaoCartao = '@NaoCartao', ObrigaCadastroCliente = '@ObrigaCadastroCliente', Obs = '@Obs', Comissao = @Comissao, PoliticaTroca = '@PoliticaTroca', ConfirmacaoPorEmail = '@ConfirmacaoPorEmail', ObrigatoriedadeID = @ObrigatoriedadeID , EnviaSms = '@EnviaSms' , TEFF = '@TEFF', NroEstabelecimento = '@NroEstabelecimento', NroEstabelecimentoAmex = '@NroEstabelecimentoAmex', Ativo = '@Ativo', ImprimeTermica = '@ImprimeTermica', TipoImpressaoTermica = '@TipoImpressaoTermica' ");
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
                sql.Replace("@NroEstabelecimentoAmex", this.NroEstabelecimentoAmex.ValorBD);
                sql.Replace("@NroEstabelecimento", this.NroEstabelecimento.ValorBD);
                sql.Replace("@Ativo", this.Ativo.ValorBD);
                sql.Replace("@ImprimeTermica", this.ImprimeTermica.ValorBD);
                sql.Replace("@TipoImpressaoTermica", this.TipoImpressaoTermica.ValorBD);

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

        public List<EstruturaIDNome> CarregarCanaisVendaRemota(int EmpresaID, string CanaisID)
        {
            try
            {
                List<EstruturaIDNome> lstIDNome = new List<EstruturaIDNome>();
                StringBuilder stbSQL = new StringBuilder();
                EstruturaIDNome item;
                IRLib.Canal canal = new Canal();
                item = new EstruturaIDNome();
                item.ID = 0;
                item.Nome = "Selecione...";
                lstIDNome.Add(item);

                stbSQL.Append("    SELECT tCanal.ID, tCanal.Nome FROM tEmpresa (NOLOCK) ");
                stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID ");
                stbSQL.Append("     WHERE tCanal.TipoVenda = '" + (char)TipoDeVenda.VendaRemota + "'");
                stbSQL.Append("       AND tCanal.EmpresaID = " + EmpresaID);
                if (CanaisID.Length > 0)
                    stbSQL.Append("        AND tCanal.ID IN (" + CanaisID + ")");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    item = new EstruturaIDNome();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    lstIDNome.Add(item);
                }
                return lstIDNome;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


        /// <summary>
        /// Captura os prefixos do Tipo Canal - LP 20090313 - 229
        /// </summary>
        /// <param name="prefixo">Nome do Prefixo</param>
        /// <param name="registroZero">Valor Padrão</param>
        /// <returns></returns>
        public DataTable CarregaPrefixos(string prefixo, string registroZero)
        {

            DataTable tabela = new DataTable("Prefixo");
            tabela.Columns.Add("Prefixo", typeof(string));
            tabela.Columns.Add("Tipo", typeof(string));

            try
            {
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { "", registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tPrefixo.Prefixo " +
                    "FROM " +
                    "   tPrefixo " +
                    "WHERE " +
                    "   (tPrefixo.Tipo = 'C') " +
                    ((prefixo != "") ? "" +
                    "AND " +
                    "   (tPrefixo.Prefixo = '" + prefixo + "') " : "") +
                    "ORDER BY " +
                    "   tPrefixo.Prefixo"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Prefixo"] = bd.LerString("Prefixo");
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public int QuantidadeCanaisIR()
        {
            string sql = @"SELECT COUNT(tCanal.ID)
                        FROM tEmpresa, tCanal 
                        WHERE EmpresaVende = 'T' 
                        AND EmpresaPromove = 'F'
                        AND tCanal.EmpresaID = tEmpresa.ID";
            CTLib.BD bd = new CTLib.BD();
            object retorno = bd.ConsultaValor(sql);
            if (retorno is int)
                return (int)retorno;
            throw new Exception("Problemas ao buscar quantidade de canais.");
        }

        /// <summary>
        /// Retorna o ID e Nome dos Canais IR.
        /// </summary>
        /// <returns></returns>
        public List<EstruturaIDNome> CanaisIR()
        {
            try
            {
                string sql = @"SELECT tCanal.ID,tCanal.Nome 
                        FROM tEmpresa, tCanal 
                        WHERE EmpresaVende = 'T' 
                        AND EmpresaPromove = 'F'
                        AND tCanal.EmpresaID = tEmpresa.ID";
                EstruturaIDNome aux;
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    aux = new EstruturaIDNome();
                    aux.ID = bd.LerInt("ID");
                    aux.Nome = bd.LerString("Nome");
                    retorno.Add(aux);
                }
                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //		public override bool Excluir(int id){
        //
        //			base.Excluir();
        //			bool excluiuPrecosCanais	= false,
        //				 excluiuPerfisCanais	= false,
        //				 excluiuEventosCanais	= false,
        //				 excluiuProdutosCanais	= false,
        //				 excluiuLojas			= false,
        //				 excluiuPagamentosCanais= false;
        //
        //			excluiuPrecosCanais		= DeletaPrecosCanais(id);
        //			excluiuPerfisCanais		= DeletaPerfisCanais(id);
        //			excluiuEventosCanais	= DeletaEventosCanais(id);
        //			excluiuProdutosCanais	= DeletaProdutosCanais(id);
        //			excluiuLojas			= DeletaLojas(id);
        //			excluiuPagamentosCanais = DeletaPagamentosCanais(id);
        //
        //			if (!excluiuPrecosCanais  && !excluiuPerfisCanais && !excluiuEventosCanais && !excluiuProdutosCanais && !excluiuLojas && !excluiuPagamentosCanais)
        //				return false;
        //			else return true;

        //		}

        //		private bool DeletaPrecosCanais(int id) {return false;}
        //
        //		private bool DeletaPerfisCanais(int id) {return false;}
        //
        //		private bool DeletaEventosCanais(int id) {return false;}
        //
        //		private bool DeletaProdutosCanais(int id) {return false;}
        //
        //		private bool DeletaLojas(int id){
        //
        //			LojaLista lojas = new LojaLista();
        //			lojas.FiltroSQL = "CanalID = " + id;
        //			lojas.Carregar();
        //			lojas.Primeiro();
        //
        //			while (lojas.Loja.Excluir(lojas.Loja.Control.ID)){
        //				lojas.Proximo();
        //			}
        //			base.Excluir();
        //
        //			if (lojas.Loja.Control.ID > 0)
        //				return true;
        //			else return false;
        //		}

        private bool DeletaPagamentosCanais(int id) { return false; }

        /// <summary>		
        /// Obter pagamentos de um canal
        /// </summary>
        /// <returns></returns>
        public string FormaPagamentos()
        {
            try
            {
                string pagamentos = "";
                string sql =
                    "SELECT        FormaPagamentoID, CanalID " +
                    "FROM            tCanalFormaPagamento " +
                    "WHERE        (CanalID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                bool primeiraVez = true;
                while (bd.Consulta().Read())
                {
                    if (primeiraVez)
                    {
                        pagamentos = bd.LerInt("FormaPagamentoID").ToString();
                        primeiraVez = false;
                    }
                    else
                    {
                        pagamentos = pagamentos + "," + bd.LerInt("FormaPagamentoID").ToString();
                    }
                }
                bd.Fechar();
                return pagamentos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Traz o campo ObrigaCadastroCliente do canal através da loja
        /// </summary>
        /// <param name="LojaID"></param>
        /// <returns></returns>
        public string ObrigaCadastro(int lojaID)
        {
            try
            {
                string sql = "SELECT ObrigaCadastroCliente FROM tCanal(NOLOCK) " +
                             "INNER JOIN tLoja (NOLOCK) ON tLoja.CanalID = tCanal.ID " +
                             "WHERE tLoja.ID = " + lojaID;
                object retorno = bd.ConsultaValor(sql);
                if (retorno is string)
                    return (string)retorno;
                else
                {
                    retorno = "F";
                    return (string)retorno;
                }
            }
            finally
            {
                bd.Fechar();
            }


        }

        /// <summary>		
        /// Obter categorias de um canal especifico
        /// </summary>
        /// <returns></returns>
        public override DataTable Categorias()
        {

            try
            {

                DataTable tabela = new DataTable("Categoria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tProdutoCategoria.ID,tProdutoCategoria.Nome " +
                    "FROM tProdutoCategoria,tProduto,tCanalProduto " +
                    "WHERE tcanalproduto.CanalID=" + this.Control.ID + " AND tProdutoCategoria.id=tProduto.ProdutoCategoriaID AND tProduto.ID=tCanalProduto.ProdutoID " +
                    "GROUP BY tProdutoCategoria.ID,tProdutoCategoria.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        /// <summary>		
        /// Lojas dos canais informados
        /// </summary>
        /// <returns></returns>
        public DataTable LojasPorCanais(string canaisInformados)
        {
            try
            {
                DataTable tabela = new DataTable("Loja");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha = tabela.NewRow();
                linha["ID"] = -1;
                linha["Nome"] = "Lojas discriminadas";
                tabela.Rows.Add(linha);
                linha = tabela.NewRow();
                linha["ID"] = 0;
                linha["Nome"] = "Lojas agrupadas";
                tabela.Rows.Add(linha);
                if (canaisInformados != "")
                {
                    string sql =
                        "SELECT     DISTINCT tLoja.ID, tCanal.ID, tLoja.Nome + ', ' + tCanal.Nome + ', ' + tEmpresa.Nome AS Nome, tEmpresa.Nome AS Empresa, tCanal.Nome AS Canal, tLoja.Nome AS Loja " +
                        "FROM       tCanal INNER JOIN " +
                        "tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
                        "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                        "WHERE      (tLoja.CanalID IN (" + canaisInformados + ")) " +
                        "ORDER BY tLoja.Nome ";
                    bd.Consulta(sql);
                    while (bd.Consulta().Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                    bd.Fechar();
                }
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>		
        /// Lojas deste canal
        /// </summary>
        /// <returns></returns>
        public DataTable Lojas(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tLoja WHERE CanalID=" + this.Control.ID + " ORDER BY Nome";
                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

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
        ///Lojas deste canal
        /// </summary>
        /// <returns></returns>
        public override DataTable Lojas()
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CanalID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID, Nome FROM tLoja " +
                    "WHERE CanalID=" + this.Control.ID + " " +
                    "ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

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

        public DataTable EventosDisponivelRelatorio(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql =
                    "SELECT DISTINCT tEvento.Nome, tEvento.ID " +
                    "FROM            tEvento INNER JOIN " +
                    "tCanalEvento ON tEvento.ID = tCanalEvento.EventoID INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID " +
                    "WHERE        (tApresentacao.DisponivelRelatorio = 'T') AND (tCanalEvento.CanalID = " + this.Control.ID + ") " +
                    "ORDER BY tEvento.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        /// <summary>
        /// Obter eventos desse canal
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable Eventos(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql = "SELECT e.ID,e.Nome FROM tEvento as e, tCanalEvento as ce WHERE e.ID=ce.EventoID AND " +
                    "ce.CanalID=" + this.Control.ID + " ORDER BY e.Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        //		/// <summary>		
        //		///Eventos a venda por este canal
        //		/// </summary>
        //		/// <returns></returns>
        //		public DataTable EventosComImagem(){
        //
        //			try{
        //
        //				DataTable tabela = new DataTable("Evento");
        //
        //				tabela.Columns.Add("ID", typeof(int));
        //				tabela.Columns.Add("Nome", typeof(string));
        //				tabela.Columns.Add("LocalID", typeof(int)); 
        //				tabela.Columns.Add("VersaoImagemIngresso", typeof(int)); //usado na ImagemAtualizar
        //				tabela.Columns.Add("VersaoImagemVale", typeof(int)); //usado na ImagemAtualizar
        //				tabela.Columns.Add("VersaoImagemVale2", typeof(int)); //usado na ImagemAtualizar
        //				tabela.Columns.Add("VersaoImagemVale3", typeof(int)); //usado na ImagemAtualizar
        //				
        //				string sql = "SELECT e.ID, e.Nome, e.LocalID, e.VersaoImagemIngresso, e.VersaoImagemVale, e.VersaoImagemVale2, e.VersaoImagemVale3 "+
        //					"FROM tEvento as e, tCanalEvento as ce WHERE e.ID=ce.EventoID AND "+
        //					"ce.CanalID="+this.Control.ID+" ORDER BY e.Nome";
        //
        //				bd.Consulta(sql);
        //
        //				while(bd.Consulta().Read()){
        //					DataRow linha = tabela.NewRow();
        //					linha["ID"]= bd.LerInt("ID");
        //					linha["Nome"]= bd.LerString("Nome");
        //					linha["LocalID"]= bd.LerInt("LocalID");
        //					linha["VersaoImagemIngresso"]= bd.LerInt("VersaoImagemIngresso");
        //					linha["VersaoImagemVale"]= bd.LerInt("VersaoImagemVale");
        //					linha["VersaoImagemVale2"]= bd.LerInt("VersaoImagemVale2");
        //					linha["VersaoImagemVale3"]= bd.LerInt("VersaoImagemVale3");
        //					tabela.Rows.Add(linha);
        //				}
        //				bd.Fechar();
        //
        //				return tabela;
        //
        //			}catch(Exception ex){
        //				throw ex;
        //			}
        //
        //		}

        /// <summary>		
        ///Pacotes a venda por este canal
        /// </summary>
        /// <returns></returns>
        public DataTable Pacotes(Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Pacote");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("LocalID", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT pa.ID, pa.Nome, pa.LocalID, SUM(pe.Valor * pi.Quantidade) AS Valor " +
                    "FROM tPacote AS pa, tCanalPacote AS cp, tPacoteItem AS pi, tPreco AS pe, tApresentacao AS a, tApresentacaoSetor AS tas " +
                    "WHERE tas.ApresentacaoID=a.ID AND tas.ID=pe.ApresentacaoSetorID AND pa.ID=cp.PacoteID AND pe.ID=pi.PrecoID AND pi.PacoteID=pa.ID AND cp.CanalID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " GROUP BY pa.ID, pa.Nome, pa.LocalID " +
                    "ORDER BY pa.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["Valor"] = bd.LerDecimal("Valor");
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

        /// <summary>		
        ///Eventos a venda por este canal
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT e.ID, e.Nome, e.LocalID " +
                    "FROM tEvento AS e, tCanalEvento as ce, tApresentacao AS a " +
                    "WHERE e.ID=ce.EventoID AND e.ID=a.EventoID AND ce.CanalID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LocalID"] = bd.LerInt("LocalID");
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

        /// <summary>		
        ///Pacotes a venda por este canal
        /// </summary>
        /// <returns></returns>
        public DataTable PacotesParaVenda()
        {

            try
            {

                DataTable tabela = new DataTable("Pacote");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT pa.ID, pa.Nome, pa.LocalID, SUM(pe.Valor * pi.Quantidade) AS Valor " +
                    "FROM tPacote AS pa, tCanalPacote AS cp, tPacoteItem AS pi, tPreco AS pe, tApresentacao AS a, tApresentacaoSetor AS tas " +
                    "WHERE tas.ApresentacaoID=a.ID AND tas.ID=pe.ApresentacaoSetorID AND pa.ID=cp.PacoteID AND pe.ID=pi.PrecoID AND pi.PacoteID=pa.ID AND cp.CanalID=" + this.Control.ID +
                    " AND a.DisponivelVenda='T' " +
                    "GROUP BY pa.ID, pa.Nome, pa.LocalID " +
                    "ORDER BY pa.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["Valor"] = bd.LerDecimal("Valor");
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

        /// <summary>		
        ///Eventos a venda por este canal
        /// </summary>
        /// <returns></returns>
        public DataTable EventosParaVenda()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                //string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                string sql = "SELECT DISTINCT e.ID, e.Nome, e.LocalID " +
                    "FROM tEvento AS e, tCanalEvento AS ce, tApresentacao AS a " +
                    "WHERE e.ID=ce.EventoID AND e.ID=a.EventoID AND ce.CanalID=" + this.Control.ID + " AND a.DisponivelVenda='T' " +
                    "ORDER BY e.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LocalID"] = bd.LerInt("LocalID");
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

        /// <summary>		
        ///Eventos a venda por este canal
        /// </summary>
        /// <returns></returns>
        public override DataTable Eventos()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));

                string sql = "SELECT e.ID, e.Nome, e.LocalID " +
                    "FROM tEvento as e, tCanalEvento as ce WHERE e.ID=ce.EventoID AND " +
                    "ce.CanalID=" + this.Control.ID + " ORDER BY e.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LocalID"] = bd.LerInt("LocalID");
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

        /// <summary>		
        /// Obter todos os canais
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                string sql =
                    "SELECT   tCanal.Nome + ', ' + tEmpresa.Nome AS Nome, tCanal.ID, tCanal.EmpresaID " +
                    "FROM     tCanal INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    "ORDER BY tEmpresa.Nome ";
                bd.Consulta(sql);
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        /// <summary>		
        /// Obter todos os canais
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tCanal.ID,tCanal.Nome,tEmpresa.Nome AS Empresa " +
                    "FROM tCanal,tEmpresa " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + "/" + bd.LerString("Empresa");
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


        private decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        private string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }



        /// <summary>
        /// Vendas Gerenciais por canal com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total
        /// Com porcentagem
        /// </summary>
        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                { // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os dados na condição especificada
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // canal zero pega todos
                int totaisVendidos = QuantidadeIngressosPorCanal(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorCanal(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorCanal(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorCanal(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada canal na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorCanal(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorCanal(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorCanal(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorCanal(ingressoLogIDsCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "Canal";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Canais por definido pelos IngressoLogIDs
        /// </summary>
        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tCanal.ID AS CanalID, tEmpresa.Nome + '-' + tCanal.Nome AS Canal " +
                        "FROM tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tCanal ON tLoja.CanalID = tCanal.ID INNER JOIN " +
                        "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("CanalID");
                        linha["VariacaoLinha"] = obterDados.LerString("Canal");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorCanal(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoCanal = "";
                    if (this.Control.ID > 0)
                        condicaoCanal = "AND (tCanal.ID = " + this.Control.ID + ") ";
                    else
                        condicaoCanal = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT  COUNT(tCanal.ID) AS QuantidadeIngressos " +
                        "FROM    tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tCanal ON tLoja.CanalID = tCanal.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoCanal;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de canal
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos canais
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorCanal
        /// <summary>
        /// Obter valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorCanal(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoCanal = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoCanal = "AND (tCanal.ID = " + this.Control.ID + ") ";
                    else
                        condicaoCanal = " "; // todos se for = zero
                    string sql;
                    sql =
                        "SELECT  SUM(tPreco.Valor) AS Valor " +
                        "FROM    tVendaBilheteria INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tCanal ON tLoja.CanalID = tCanal.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoCanal;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do canal
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos canais
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorCanal

        /// <summary>
        /// Os caixas deste canal
        /// </summary>
        /// <returns></returns>
        public override DataTable Caixas(string registroZero)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("Caixas");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID " +
                    "FROM tCanal INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
                    "tCaixa ON tLoja.ID = tCaixa.LojaID INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE    (tCanal.ID = " + this.Control.ID + ") " +
                    "ORDER BY tCaixa.DataAbertura DESC ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DataAbertura"] = bd.LerString("Nome") + " - " + bd.LerStringFormatoDataHora("DataAbertura");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        }
        /// <summary>
        /// Os caixas deste canal em função do usuário
        /// </summary>
        /// <returns></returns>
        public override DataTable Caixas(string registroZero, int usuarioID)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("Caixas");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID " +
                    "FROM tCanal INNER JOIN " +
                    "tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
                    "tCaixa ON tLoja.ID = tCaixa.LojaID INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE    (tCanal.ID = " + this.Control.ID + ") AND (tCaixa.UsuarioID = " + usuarioID + ") " +
                    "ORDER BY tCaixa.DataAbertura DESC ";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DataAbertura"] = bd.LerString("Nome") + " - " + bd.LerStringFormatoDataHora("DataAbertura");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        }
        public int QuantidadeCortesiasPorCaixa(string dataInicial, string dataFinal, int eventoID)
        {
            try
            {
                int quantidade = 0;
                int quantidadeVendidos = 0;
                int quantidadeCancelados = 0;
                if (dataInicial == "" || dataFinal == "")
                    return 0;
                string condicaoEvento = "";
                #region Obtendo quantidade de cortesias vendidos
                BD bdVendidos = new BD();
                if (eventoID > 0)
                    condicaoEvento = "GROUP BY tIngressoLog.Acao, tCanal.ID, tApresentacao.EventoID " +
                        "HAVING (tIngressoLog.Acao = N'" + IngressoLog.VENDER + "') AND (tCanal.ID = " + this.Control.ID + ") AND (tApresentacao.EventoID = " + eventoID + ") ";
                else
                    condicaoEvento = "GROUP BY tIngressoLog.Acao, tCanal.ID " +
                        "HAVING (tIngressoLog.Acao = N'" + IngressoLog.VENDER + "') AND (tCanal.ID = " + this.Control.ID + ") ";
                string sqlVendidos =
                    "SELECT        COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.Acao, tCanal.ID AS CanalID " +
                    "FROM            tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                    "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                    "tCanal ON tLoja.CanalID = tCanal.ID " +
                    "WHERE        (tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tIngressoLog.CortesiaID > 0) " + condicaoEvento;
                bdVendidos.Consulta(sqlVendidos);
                if (bdVendidos.Consulta().Read())
                {
                    quantidadeVendidos = bdVendidos.LerInt("Quantidade");
                }
                bdVendidos.Fechar();
                #endregion
                #region Obtendo quantidade de cortesias cancelados
                BD bdCancelados = new BD();
                if (eventoID > 0)
                    condicaoEvento = "GROUP BY tIngressoLog.Acao, tCanal.ID, tApresentacao.EventoID " +
                        "HAVING (tIngressoLog.Acao = N'" + IngressoLog.CANCELAR + "') AND (tCanal.ID = " + this.Control.ID + ") AND (tApresentacao.EventoID = " + eventoID + ") ";
                else
                    condicaoEvento = "GROUP BY tIngressoLog.Acao, tCanal.ID " +
                        "HAVING (tIngressoLog.Acao = N'" + IngressoLog.CANCELAR + "') AND (tCanal.ID = " + this.Control.ID + ") ";
                string sqlCancelados =
                    "SELECT        COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.Acao, tCanal.ID AS CanalID " +
                    "FROM            tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                    "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                    "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                    "tCanal ON tLoja.CanalID = tCanal.ID " +
                    "WHERE        (tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') AND (tIngressoLog.CortesiaID > 0) " + condicaoEvento;
                bdCancelados.Consulta(sqlCancelados);
                if (bdCancelados.Consulta().Read())
                {
                    quantidadeCancelados = bdCancelados.LerInt("Quantidade");
                }
                bdCancelados.Fechar();
                #endregion
                quantidade = quantidadeVendidos - quantidadeCancelados;
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeCortesiasPorCaixa

        public DataTable Listagem(int empresaID)
        {
            try
            {
                DataTable tabela = new DataTable("CanalListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Taxa", typeof(int));
                tabela.Columns.Add("ComprovanteQuantidade", typeof(int));
                tabela.Columns.Add("Tipo Venda", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0)
                {
                    condicao = "WHERE      (tEmpresa.ID = " + empresaID + ") ";
                }
                else
                {
                    condicao = "";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.ID, tCanal.Nome AS Canal, tCanalTipo.Nome AS CanalTipo, tCanal.TaxaConveniencia, tCanal.ComprovanteQuantidade, " +
                    "CASE " +
                    "  WHEN tCanal.TipoVenda = 'T' THEN 'Impressão de Ingresso' " +
                    "  WHEN tCanal.TipoVenda = 'F' THEN 'Venda remota' " +
                    "  ELSE 'Impressão de voucher' " +
                    "END AS TipoVenda, " +
                    "tCanal.Obs " +
                    "FROM     tCanalTipo INNER JOIN " +
                    "tCanal ON tCanalTipo.ID = tCanal.CanalTipoID INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Tipo"] = bd.LerString("CanalTipo");
                    linha["Taxa"] = bd.LerInt("TaxaConveniencia");
                    linha["ComprovanteQuantidade"] = bd.LerBoolean("ComprovanteQuantidade");
                    linha["Tipo Venda"] = bd.LerString("TipoVenda");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable ListagemPorRegionalID(int empresaID, int regionalID)
        {
            try
            {
                DataTable tabela = new DataTable("CanalListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Canal", typeof(string));
                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Taxa", typeof(int));
                tabela.Columns.Add("ComprovanteQuantidade", typeof(int));
                tabela.Columns.Add("Tipo de Venda", typeof(bool));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0)
                {
                    condicao = "WHERE (tEmpresa.ID = " + empresaID + " AND tEmpresa.RegionalID = " + regionalID + ") ";
                }
                else
                {
                    condicao = "WHERE (tEmpresa.RegionalID = " + regionalID + ") ";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tCanal.ID, tCanal.Nome AS Canal, tCanalTipo.Nome AS CanalTipo, tCanal.TaxaConveniencia, tCanal.ComprovanteQuantidade, " +
                    "CASE " +
                    "  WHEN tCanal.TipoVenda = 'T' THEN 'Impressão de Ingresso' " +
                    "  WHEN tCanal.TipoVenda = 'F' THEN 'Venda remota' " +
                    "  ELSE 'Impressão de voucher' " +
                    "END AS TipoVenda, " +
                    "tCanal.Obs " +
                    "FROM     tCanalTipo INNER JOIN " +
                    "tCanal ON tCanalTipo.ID = tCanal.CanalTipoID INNER JOIN " +
                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tCanal.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Tipo"] = bd.LerString("CanalTipo");
                    linha["Taxa"] = bd.LerInt("TaxaConveniencia");
                    linha["ComprovanteQuantidade"] = bd.LerBoolean("ComprovanteQuantidade");
                    linha["Tipo de Venda"] = bd.LerBoolean("TipoVenda");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem


        /// <summary>
        /// Busca o nome do canal, empresa e empresaID.
        /// </summary>
        /// <param name="canalID"></param>
        /// <returns></returns>
        public string InfoParaVenda(int canalID)
        {
            try
            {
                string sql = "SELECT tCanal.Nome AS CanalNome, tEmpresa.Nome AS EmpresaNome, tEmpresa.ID AS EmpresaID, tCanal.ComprovanteQuantidade, tCanal.TipoVenda, OpcaoImprimirSemPreco FROM tCanal, tEmpresa WHERE tCanal.EmpresaID = tEmpresa.ID AND tCanal.ID = " + canalID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Nome.Valor = bd.LerString("CanalNome");
                    this.EmpresaID.Valor = bd.LerInt("EmpresaID");
                    this.ComprovanteQuantidade.Valor = bd.LerInt("ComprovanteQuantidade");
                    this.TipoVenda.Valor = bd.LerString("TipoVenda");
                    this.OpcaoImprimirSemPreco.Valor = bd.LerBoolean("OpcaoImprimirSemPreco");
                    return bd.LerString("EmpresaNome");
                }
                else
                    throw new CanalException("Informações sobre o Canal ou Empresa não encontrada.");
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }
        public DataTable ListarTodosFinanceiro()
        {
            DataTable dt = new DataTable();
            dt.Load(base.bd.Consulta("SELECT tCanal.ID CanalID, tEmpresa.Nome Empresa,tCanal.Nome CanalNome, tCanalTipo.Nome CanalTipoNome, tCanalTipo.ID CanalTipoID,\r\n                        CASE \r\n                            WHEN Cartao = 'C' THEN 'Cliente'\r\n                            WHEN Cartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SCartao, \r\n                        CASE\r\n                            WHEN NaoCartao = 'C' THEN 'Cliente'\r\n                            WHEN NaoCartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SNaoCartao,\r\n\t\t\t\t\t\ttCanal.Cartao Cartao,\r\n\t\t\t\t\t\ttCanal.NaoCartao NaoCartao\r\n                        FROM tCanal\r\n                        INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID\r\n                        INNER JOIN tCanalTipo ON tCanal.CanalTipoID = tCanalTipo.ID"));
            return dt;
        }

        public DataTable ListarCartaoVazioFinanceiro()
        {
            DataTable dt = new DataTable();
            dt.Load(base.bd.Consulta("SELECT tCanal.ID CanalID, tEmpresa.Nome Empresa,tCanal.Nome CanalNome, tCanalTipo.Nome CanalTipoNome, tCanalTipo.ID CanalTipoID,\r\n                        CASE \r\n                            WHEN Cartao = 'C' THEN 'Cliente'\r\n                            WHEN Cartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SCartao, \r\n                        CASE\r\n                            WHEN NaoCartao = 'C' THEN 'Cliente'\r\n                            WHEN NaoCartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SNaoCartao,\r\n\t\t\t\t\t\ttCanal.Cartao Cartao,\r\n\t\t\t\t\t\ttCanal.NaoCartao NaoCartao\r\n                        FROM tCanal\r\n                        INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID\r\n                        INNER JOIN tCanalTipo ON tCanal.CanalTipoID = tCanalTipo.ID\r\n                        WHERE Cartao IS NULL OR Cartao = '' OR NaoCartao IS NULL OR NaoCartao = ''"));
            return dt;
        }


        public DataTable ListarCartaoPreenchidoFinanceiro()
        {
            DataTable dt = new DataTable();
            dt.Load(base.bd.Consulta("SELECT tCanal.ID CanalID, tEmpresa.Nome Empresa,tCanal.Nome CanalNome, tCanalTipo.Nome CanalTipoNome, tCanalTipo.ID CanalTipoID, \r\n                        CASE \r\n                            WHEN Cartao = 'C' THEN 'Cliente'\r\n                            WHEN Cartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SCartao, \r\n                        CASE\r\n                            WHEN NaoCartao = 'C' THEN 'Cliente'\r\n                            WHEN NaoCartao = 'I' THEN 'IR'\r\n                            ELSE ''\r\n                        END AS SNaoCartao,\r\n\t\t\t\t\t\ttCanal.Cartao Cartao,\r\n\t\t\t\t\t\ttCanal.NaoCartao NaoCartao\r\n                        FROM tCanal\r\n                        INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID\r\n                        INNER JOIN tCanalTipo ON tCanal.CanalTipoID = tCanalTipo.ID\r\n                        WHERE Cartao IS NOT NULL AND Cartao <> '' AND NaoCartao IS NOT NULL AND NaoCartao <> ''"));
            return dt;
        }


        public bool AtualizarFinanceiro(int CanalID, int CanalTipoID, string Cartao, string NaoCartao)
        {
            if ((Cartao == "") || (Cartao == null))
            {
                Cartao = "NULL";
            }
            else
            {
                Cartao = "'" + Cartao + "'";
            }
            if ((NaoCartao == "") || (NaoCartao == null))
            {
                NaoCartao = "NULL";
            }
            else
            {
                NaoCartao = "'" + NaoCartao + "'";
            }
            return (base.bd.Executar(string.Concat(new object[] { "UPDATE tCanal SET CanalTipoID = ", CanalTipoID, " , Cartao = ", Cartao, ", NaoCartao = ", NaoCartao, " WHERE ID = ", CanalID })) != 0);
        }

        /// <summary>
        /// Preenche atributos basicos do canal, mais usados em relatorio
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerEmpresaTipoNomeTaxa(int id)
        {

            try
            {

                string sql = "SELECT ID,EmpresaID,CanalTipoID,Nome,TaxaConveniencia FROM tCanal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CanalTipoID.ValorBD = bd.LerInt("CanalTipoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
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

        public int BuscaEmpresaIDporLojaID(int lojaID)
        {
            try
            {
                string sql =
                    @"SELECT c.EmpresaID FROM 
                    tLoja l (NOLOCK)
                    INNER JOIN tCanal c (NOLOCK) ON c.ID = l.CanalID
                    WHERE l.ID = " + lojaID;

                int empresaid = Convert.ToInt32(bd.ConsultaValor(sql));
                return empresaid;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int BuscaCanalIDporLojaID(int lojaID)
        {
            try
            {
                object retorno = bd.ConsultaValor("SELECT tCanal.ID FROM tLoja, tCanal WHERE tLoja.CanalID = tCanal.ID AND tLoja.ID =" + lojaID);
                if (retorno is int)
                    return (int)retorno;
                throw new Exception("Canal não encontrado!");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string BuscaCanalNomeporLojaID(int lojaID)
        {
            object retorno = bd.ConsultaValor("SELECT tCanal.Nome FROM tLoja, tCanal WHERE tLoja.CanalID = tCanal.ID AND tLoja.ID =" + lojaID);
            return retorno != null ? retorno.ToString() : string.Empty;
        }

        public string BuscaCanalIDporNomeLoja(string NomeLoja)
        {
            try
            {

                object retorno = bd.ConsultaValor("SELECT tCanal.Nome FROM tLoja, tCanal WHERE tLoja.CanalID = tCanal.ID AND tLoja.Nome = '" + NomeLoja + "'");
                if (retorno is string)
                    return (string)retorno;
                throw new Exception("Canal não encontrado!");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscaCanalIDporNomeCanal(string NomeCanal)
        {
            try
            {

                object retorno = bd.ConsultaValor("SELECT tCanal.ID FROM tCanal WHERE tCanal.Nome = '" + NomeCanal + "'");
                if (retorno is int)
                    return (int)retorno;
                throw new Exception("Canal não encontrado!");
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Busca todos os canais de uma empresa trazendo junto o nome do tipo de canal
        /// </summary>
        /// <param name="empresaID">ID da Empresa</param>
        /// <param name="eventoID">ID do Evento</param>
        /// <returns>Lista de canais no formado da CanalEvento adicionando o nome da tCanalTipo</returns>
        public List<EstruturaCanaisEvento> BuscaCanaisPorEmpresa(int empresaID, int eventoID)
        {
            List<EstruturaCanaisEvento> retorno = new List<EstruturaCanaisEvento>();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT tc.ID AS CanalID, tc.TaxaConveniencia, tc.TaxaMinima, tc.TaxaMaxima, tc.TaxaComissao, tc.ComissaoMinima, tc.ComissaoMaxima, tct.Nome AS CanalTipo ");
            sql.Append("FROM tCanal tc INNER JOIN tCanaltipo tct ON tc.CanalTipoID = tct.ID ");
            sql.Append("WHERE tc.EmpresaID = " + empresaID);

            using (BD bd = new BD())
            {
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    EstruturaCanaisEvento item = new EstruturaCanaisEvento();
                    item.CanalID = bd.LerInt("CanalID");
                    item.EventoID = eventoID;
                    item.TaxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                    item.TaxaMinima = bd.LerDecimal("TaxaMinima");
                    item.TaxaMaxima = bd.LerDecimal("TaxaMaxima");
                    item.TaxaComissao = bd.LerDecimal("TaxaComissao");
                    item.ComissaoMinima = bd.LerDecimal("ComissaoMinima");
                    item.ComissaoMaxima = bd.LerDecimal("ComissaoMaxima");
                    item.CanalTipo = bd.LerString("CanalTipo");

                    retorno.Add(item);
                }
            }

            return retorno;
        }

        public List<EstruturaCanaisEvento> BuscaCanaisDistribuidosPorEvento(int eventoID)
        {
            List<EstruturaCanaisEvento> retorno = new List<EstruturaCanaisEvento>();

            StringBuilder sql = new StringBuilder();

            //sql.Append("SELECT tc.ID AS CanalID, tc.TaxaConveniencia, tc.TaxaMinima, tc.TaxaMaxima, tc.TaxaComissao, tc.ComissaoMinima, tc.ComissaoMaxima, tct.Nome AS CanalTipo ");
            //sql.Append("FROM tCanal tc INNER JOIN tCanaltipo tct ON tc.CanalTipoID = tct.ID ");
            //sql.Append("WHERE tc.EmpresaID = " + empresaID);

            sql.Append("SELECT tc.ID AS CanalID, tc.TaxaConveniencia, tc.TaxaMinima, tc.TaxaMaxima, tc.TaxaComissao, tc.ComissaoMinima, tc.ComissaoMaxima, tct.Nome AS CanalTipo ");
            sql.Append("FROM dbo.tCanal tc INNER JOIN dbo.tEmpresa te ON te.ID = tc.EmpresaID LEFT JOIN dbo.tCanalTipo tct ON tct.ID = tc.CanalTipoID ");
            sql.Append("WHERE te.EmpresaVende = 'T' AND te.EmpresaPromove = 'F'");

            //sql.Append("SELECT tc.ID AS CanalID, tc.TaxaConveniencia, tc.TaxaMinima, tc.TaxaMaxima, tc.TaxaComissao, tc.ComissaoMinima, tc.ComissaoMaxima, tct.Nome AS CanalTipo ");
            //sql.Append("FROM tCanal tc (NOLOCK) INNER JOIN tCanaltipo tct (NOLOCK) ON tc.CanalTipoID = tct.ID ");
            //sql.Append("INNER JOIN dbo.tEmpresa te (NOLOCK) ON te.ID = tc.EmpresaID INNER JOIN dbo.tCanalEvento tce (NOLOCK) ON tce.CanalID = tc.ID ");
            //sql.Append("WHERE EmpresaVende = 'T' AND EmpresaPromove = 'F' AND tce.EventoID = " + eventoID);

            using (BD bd = new BD())
            {
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    EstruturaCanaisEvento item = new EstruturaCanaisEvento();
                    item.CanalID = bd.LerInt("CanalID");
                    item.EventoID = eventoID;
                    item.TaxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                    item.TaxaMinima = bd.LerDecimal("TaxaMinima");
                    item.TaxaMaxima = bd.LerDecimal("TaxaMaxima");
                    item.TaxaComissao = bd.LerDecimal("TaxaComissao");
                    item.ComissaoMinima = bd.LerDecimal("ComissaoMinima");
                    item.ComissaoMaxima = bd.LerDecimal("ComissaoMaxima");
                    item.CanalTipo = bd.LerString("CanalTipo");

                    retorno.Add(item);
                }
            }

            return retorno;
        }

        public void AtribuirEventoPreco(int CanalID)
        {
            string sql = "EXEC AtribuirEventoEPreco " + CanalID + ", " + DateTime.Now.ToString("yyyyMMddHHmmss");

            bd.Executar(sql);

        }

        /// <summary>
        /// Verifica se o canal é Cliente Presente
        /// </summary>
        /// <param name="CanalID">ID do Canal</param>
        /// <returns>True se for um canal com Cliente </returns>
        public bool EhClientePresente(int CanalID)
        {
            bool retorno = false;
            BD bd = new BD();

            try
            {
                bd.Consulta(@"SELECT  ID, TipoVenda
                               FROM    tCanal
                              WHERE TipoVenda = 'T' and ID = " + CanalID);

                if (!bd.Consulta().Read())
                    retorno = false;
                else
                    retorno = bd.LerBoolean("TipoVenda");

                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se o canal é um Canal IR
        /// </summary>
        /// <param name="CanalID">ID do Canal</param>
        /// <returns>True se for um Canal IR </returns>
        public bool EhCanalIR(int CanalID)
        {
            bool retorno = false;
            BD bd = new BD();

            try
            {
                bd.Consulta(@"SELECT c.ID
                                FROM tCanal AS c
                          INNER JOIN tEmpresa AS e ON e.ID = c.EmpresaID
                               WHERE e.EmpresaPromove = 'F' 
                                 AND e.EmpresaVende = 'T' 
                                 AND c.id = " + CanalID);

                if (!bd.Consulta().Read())
                    retorno = false;
                else
                    retorno = bd.LerInt("ID") > 0;

                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public List<int> CanaisPV(bool ativos = false)
        {
            List<int> retorno = new List<int>();

            string versao = ConfigurationManager.AppSettings["ConfigVersion"];
            string canalCallCenter = Configuracao.Get("CanaisDistribuicaoCC", versao);
            string canalSite = Configuracao.Get("CanaisDistribuicaoWeb", versao);

            string sql = String.Format("SELECT tCanal.ID " +
                                        "FROM tCanal " +
                                        "INNER JOIN tEmpresa ON tEmpresa.ID = tCanal.EmpresaID " +
                                        "WHERE tEmpresa.EmpresaVende = 'T' AND tEmpresa.EmpresaPromove = 'F' " +
                                        "AND tCanal.ID NOT IN ({0},{1})", canalCallCenter, canalSite);

            if (ativos)
                sql += String.Format(" AND tCanal.Ativo = 'T'");

            using (BD bd = new BD())
            {
                bd.Consulta(sql);

                while(bd.Consulta().Read())
                {
                    retorno.Add(bd.LerInt("ID"));
                }
            }

            return retorno;
        }

        public List<int> Existentes(List<int> canais, bool ativos = false)
        {
            List<int> retorno = new List<int>();
            string ids = String.Join(",", canais);

            string sql = String.Format("SELECT ID FROM tCanal (NOLOCK) WHERE ID IN ({0})", ids);

            if (ativos)
                sql += String.Format(" AND Ativo = 'T'");

            using (BD bd = new BD())
            {
                DataTable Canais = bd.QueryToTable(sql);

                retorno = (from row in Canais.AsEnumerable()
                           select row.Field<int>("ID")).ToList<int>();
            }

            return retorno;
        }

        public DataTable BuscaNomeCanalPorIngressoID(int IngressoID)
        {
            try
            {
                DataTable retorno = new DataTable();
                retorno.Columns.Add("Canal", typeof(string));
                retorno.Columns.Add("FormaPagamento", typeof(string));
                DataRow linha = retorno.NewRow();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT tc.Nome AS Canal, tfp.Nome AS FormaPagamento ");
                sql.Append("FROM tIngresso ti (NOLOCK) ");
                sql.Append("INNER JOIN tLoja tl (NOLOCK) ON ti.LojaID = tl.ID ");
                sql.Append("INNER JOIN tCanal tc (NOLOCK) ON tc.ID = tl.CanalID ");
                sql.Append("INNER JOIN tVendaBilheteriaFormaPagamento tvbfp (NOLOCK) ON ti.VendaBilheteriaID = tvbfp.VendaBilheteriaID ");
                sql.Append("INNER JOIN tFormaPagamento tfp (NOLOCK) ON tvbfp.FormaPagamentoID = tfp.ID ");
                sql.Append("WHERE ti.ID = " + IngressoID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    linha = retorno.NewRow();
                    linha["Canal"] = bd.LerString("Canal");
                    linha["FormaPagamento"] = bd.LerString("FormaPagamento");
                    retorno.Rows.Add(linha);
                }
                return retorno;

                throw new Exception("Canal não encontrado!");
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class CanalLista : CanalLista_B
    {

        public CanalLista() { }

        public CanalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela de todos os campos de canal carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Empresa", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Tipo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        Empresa e = new Empresa();
                        e.Ler(canal.EmpresaID.Valor);
                        linha["Empresa"] = e.Nome.Valor;
                        linha["Nome"] = canal.Nome.Valor;
                        CanalTipo canalTipo = new CanalTipo();
                        canalTipo.Ler(canal.CanalTipoID.Valor);
                        linha["Tipo"] = canalTipo.Nome.Valor;
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

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tCanal WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY EmpresaID, Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Inserir()
        {
            try
            {
                bd.IniciarTransacao();

                bool ok = canal.Inserir(bd);
                if (ok)
                {
                    lista.Add(canal.Control.ID);
                    bd.FinalizarTransacao();
                    Indice = lista.Count - 1;
                }
                else
                    bd.DesfazerTransacao();

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


    }

}
