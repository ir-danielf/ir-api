/******************************************************
* Arquivo VendaBilheteriaFormaPagamentoTEFDB.cs
* Gerado em: 18/02/2010
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "VendaBilheteriaFormaPagamentoTEF_B"

    public abstract class VendaBilheteriaFormaPagamentoTEF_B : BaseBD
    {

        public codigorespostavenda CodigoRespostaVenda = new codigorespostavenda();
        public mensagemretorno MensagemRetorno = new mensagemretorno();
        public horatransacao HoraTransacao = new horatransacao();
        public datatransacao DataTransacao = new datatransacao();
        public codigoir CodigoIR = new codigoir();
        public numeroautorizacao NumeroAutorizacao = new numeroautorizacao();
        public nsuhost NSUHost = new nsuhost();
        public nsusitef NSUSitef = new nsusitef();
        public cupom Cupom = new cupom();
        public dadosconfirmacaovenda DadosConfirmacaoVenda = new dadosconfirmacaovenda();
        public rede Rede = new rede();
        public codigorespostatransacao CodigoRespostaTransacao = new codigorespostatransacao();
        public cartaoid CartaoID = new cartaoid();

        public VendaBilheteriaFormaPagamentoTEF_B() { }

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoTEF_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamentoTEF
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteriaFormaPagamentoTEF WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CodigoRespostaVenda.ValorBD = bd.LerInt("CodigoRespostaVenda").ToString();
                    this.MensagemRetorno.ValorBD = bd.LerString("MensagemRetorno");
                    this.HoraTransacao.ValorBD = bd.LerString("HoraTransacao");
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.CodigoIR.ValorBD = bd.LerString("CodigoIR");
                    this.NumeroAutorizacao.ValorBD = bd.LerString("NumeroAutorizacao");
                    this.NSUHost.ValorBD = bd.LerInt("NSUHost").ToString();
                    this.NSUSitef.ValorBD = bd.LerInt("NSUSitef").ToString();
                    this.Cupom.ValorBD = bd.LerString("Cupom");
                    this.DadosConfirmacaoVenda.ValorBD = bd.LerString("DadosConfirmacaoVenda");
                    this.Rede.ValorBD = bd.LerInt("Rede").ToString();
                    this.CodigoRespostaTransacao.ValorBD = bd.LerInt("CodigoRespostaTransacao").ToString();
                    this.CartaoID.ValorBD = bd.LerInt("CartaoID").ToString();
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
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamentoTEF do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xVendaBilheteriaFormaPagamentoTEF WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CodigoRespostaVenda.ValorBD = bd.LerInt("CodigoRespostaVenda").ToString();
                    this.MensagemRetorno.ValorBD = bd.LerString("MensagemRetorno");
                    this.HoraTransacao.ValorBD = bd.LerString("HoraTransacao");
                    this.DataTransacao.ValorBD = bd.LerString("DataTransacao");
                    this.CodigoIR.ValorBD = bd.LerString("CodigoIR");
                    this.NumeroAutorizacao.ValorBD = bd.LerString("NumeroAutorizacao");
                    this.NSUHost.ValorBD = bd.LerInt("NSUHost").ToString();
                    this.NSUSitef.ValorBD = bd.LerInt("NSUSitef").ToString();
                    this.Cupom.ValorBD = bd.LerString("Cupom");
                    this.DadosConfirmacaoVenda.ValorBD = bd.LerString("DadosConfirmacaoVenda");
                    this.Rede.ValorBD = bd.LerInt("Rede").ToString();
                    this.CodigoRespostaTransacao.ValorBD = bd.LerInt("CodigoRespostaTransacao").ToString();
                    this.CartaoID.ValorBD = bd.LerInt("CartaoID").ToString();
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
                sql.Append("INSERT INTO cVendaBilheteriaFormaPagamentoTEF (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xVendaBilheteriaFormaPagamentoTEF (ID, Versao, CodigoRespostaVenda, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CartaoID) ");
                sql.Append("SELECT ID, @V, CodigoRespostaVenda, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CartaoID FROM tVendaBilheteriaFormaPagamentoTEF WHERE ID = @I");
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
        /// Inserir novo(a) VendaBilheteriaFormaPagamentoTEF
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteriaFormaPagamentoTEF");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamentoTEF(ID, CodigoRespostaVenda, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, CartaoID) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006',@007,@008,'@009','@010',@011,@012,@013)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CodigoRespostaVenda.ValorBD);
                sql.Replace("@002", this.MensagemRetorno.ValorBD);
                sql.Replace("@003", this.HoraTransacao.ValorBD);
                sql.Replace("@004", this.DataTransacao.ValorBD);
                sql.Replace("@005", this.CodigoIR.ValorBD);
                sql.Replace("@006", this.NumeroAutorizacao.ValorBD);
                sql.Replace("@007", this.NSUHost.ValorBD);
                sql.Replace("@008", this.NSUSitef.ValorBD);
                sql.Replace("@009", this.Cupom.ValorBD);
                sql.Replace("@010", this.DadosConfirmacaoVenda.ValorBD);
                sql.Replace("@011", this.Rede.ValorBD);
                sql.Replace("@012", this.CodigoRespostaTransacao.ValorBD);
                sql.Replace("@013", this.CartaoID.ValorBD);

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
        /// Atualiza VendaBilheteriaFormaPagamentoTEF
        /// </summary>
        /// <returns></returns>	
        [Obsolete("Do not use! ", true)]
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamentoTEF WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamentoTEF SET CodigoRespostaVenda = @001, MensagemRetorno = '@002', HoraTransacao = '@003', DataTransacao = '@004', CodigoIR = '@005', NumeroAutorizacao = '@006', NSUHost = @007, NSUSitef = @008, Cupom = '@009', DadosConfirmacaoVenda = '@010', Rede = @011, CodigoRespostaTransacao = @012, CartaoID = @013 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CodigoRespostaVenda.ValorBD);
                sql.Replace("@002", this.MensagemRetorno.ValorBD);
                sql.Replace("@003", this.HoraTransacao.ValorBD);
                sql.Replace("@004", this.DataTransacao.ValorBD);
                sql.Replace("@005", this.CodigoIR.ValorBD);
                sql.Replace("@006", this.NumeroAutorizacao.ValorBD);
                sql.Replace("@007", this.NSUHost.ValorBD);
                sql.Replace("@008", this.NSUSitef.ValorBD);
                sql.Replace("@009", this.Cupom.ValorBD);
                sql.Replace("@010", this.DadosConfirmacaoVenda.ValorBD);
                sql.Replace("@011", this.Rede.ValorBD);
                sql.Replace("@012", this.CodigoRespostaTransacao.ValorBD);
                sql.Replace("@013", this.CartaoID.ValorBD);

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
        /// Exclui VendaBilheteriaFormaPagamentoTEF com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteriaFormaPagamentoTEF WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamentoTEF WHERE ID=" + id;

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
        /// Exclui VendaBilheteriaFormaPagamentoTEF
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

            this.CodigoRespostaVenda.Limpar();
            this.MensagemRetorno.Limpar();
            this.HoraTransacao.Limpar();
            this.DataTransacao.Limpar();
            this.CodigoIR.Limpar();
            this.NumeroAutorizacao.Limpar();
            this.NSUHost.Limpar();
            this.NSUSitef.Limpar();
            this.Cupom.Limpar();
            this.DadosConfirmacaoVenda.Limpar();
            this.Rede.Limpar();
            this.CodigoRespostaTransacao.Limpar();
            this.CartaoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.CodigoRespostaVenda.Desfazer();
            this.MensagemRetorno.Desfazer();
            this.HoraTransacao.Desfazer();
            this.DataTransacao.Desfazer();
            this.CodigoIR.Desfazer();
            this.NumeroAutorizacao.Desfazer();
            this.NSUHost.Desfazer();
            this.NSUSitef.Desfazer();
            this.Cupom.Desfazer();
            this.DadosConfirmacaoVenda.Desfazer();
            this.Rede.Desfazer();
            this.CodigoRespostaTransacao.Desfazer();
            this.CartaoID.Desfazer();
        }

        public class codigorespostavenda : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRespostaVenda";
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

        public class mensagemretorno : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MensagemRetorno";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
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

        public class horatransacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "HoraTransacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 6;
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

        public class datatransacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataTransacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 6;
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

        public class codigoir : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoIR";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 25;
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

        public class numeroautorizacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroAutorizacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 25;
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

        public class nsuhost : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NSUHost";
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

        public class nsusitef : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NSUSitef";
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

        public class cupom : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cupom";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 300;
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

        public class dadosconfirmacaovenda : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DadosConfirmacaoVenda";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 40;
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

        public class rede : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Rede";
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

        public class codigorespostatransacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRespostaTransacao";
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

        public class cartaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CartaoID";
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

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoTEF");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CodigoRespostaVenda", typeof(int));
                tabela.Columns.Add("MensagemRetorno", typeof(string));
                tabela.Columns.Add("HoraTransacao", typeof(string));
                tabela.Columns.Add("DataTransacao", typeof(string));
                tabela.Columns.Add("CodigoIR", typeof(string));
                tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                tabela.Columns.Add("NSUHost", typeof(int));
                tabela.Columns.Add("NSUSitef", typeof(int));
                tabela.Columns.Add("Cupom", typeof(string));
                tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                tabela.Columns.Add("Rede", typeof(int));
                tabela.Columns.Add("CodigoRespostaTransacao", typeof(int));
                tabela.Columns.Add("CartaoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "VendaBilheteriaFormaPagamentoTEFLista_B"

    public abstract class VendaBilheteriaFormaPagamentoTEFLista_B : BaseLista
    {

        private bool backup = false;
        protected VendaBilheteriaFormaPagamentoTEF vendaBilheteriaFormaPagamentoTEF;

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoTEFLista_B()
        {
            vendaBilheteriaFormaPagamentoTEF = new VendaBilheteriaFormaPagamentoTEF();
        }

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoTEFLista_B(int usuarioIDLogado)
        {
            vendaBilheteriaFormaPagamentoTEF = new VendaBilheteriaFormaPagamentoTEF();
        }

        public VendaBilheteriaFormaPagamentoTEF VendaBilheteriaFormaPagamentoTEF
        {
            get { return vendaBilheteriaFormaPagamentoTEF; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteriaFormaPagamentoTEF especifico
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
                    vendaBilheteriaFormaPagamentoTEF.Ler(id);
                    return vendaBilheteriaFormaPagamentoTEF;
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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamentoTEF";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamentoTEF";

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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamentoTEF";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamentoTEF";

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
                    sql = "SELECT ID FROM xVendaBilheteriaFormaPagamentoTEF";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xVendaBilheteriaFormaPagamentoTEF";

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
        /// Preenche VendaBilheteriaFormaPagamentoTEF corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    vendaBilheteriaFormaPagamentoTEF.Ler(id);
                else
                    vendaBilheteriaFormaPagamentoTEF.LerBackup(id);

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

                bool ok = vendaBilheteriaFormaPagamentoTEF.Excluir();
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
        /// Inseri novo(a) VendaBilheteriaFormaPagamentoTEF na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteriaFormaPagamentoTEF.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteriaFormaPagamentoTEF.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de VendaBilheteriaFormaPagamentoTEF carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoTEF");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CodigoRespostaVenda", typeof(int));
                tabela.Columns.Add("MensagemRetorno", typeof(string));
                tabela.Columns.Add("HoraTransacao", typeof(string));
                tabela.Columns.Add("DataTransacao", typeof(string));
                tabela.Columns.Add("CodigoIR", typeof(string));
                tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                tabela.Columns.Add("NSUHost", typeof(int));
                tabela.Columns.Add("NSUSitef", typeof(int));
                tabela.Columns.Add("Cupom", typeof(string));
                tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                tabela.Columns.Add("Rede", typeof(int));
                tabela.Columns.Add("CodigoRespostaTransacao", typeof(int));
                tabela.Columns.Add("CartaoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteriaFormaPagamentoTEF.Control.ID;
                        linha["CodigoRespostaVenda"] = vendaBilheteriaFormaPagamentoTEF.CodigoRespostaVenda.Valor;
                        linha["MensagemRetorno"] = vendaBilheteriaFormaPagamentoTEF.MensagemRetorno.Valor;
                        linha["HoraTransacao"] = vendaBilheteriaFormaPagamentoTEF.HoraTransacao.Valor;
                        linha["DataTransacao"] = vendaBilheteriaFormaPagamentoTEF.DataTransacao.Valor;
                        linha["CodigoIR"] = vendaBilheteriaFormaPagamentoTEF.CodigoIR.Valor;
                        linha["NumeroAutorizacao"] = vendaBilheteriaFormaPagamentoTEF.NumeroAutorizacao.Valor;
                        linha["NSUHost"] = vendaBilheteriaFormaPagamentoTEF.NSUHost.Valor;
                        linha["NSUSitef"] = vendaBilheteriaFormaPagamentoTEF.NSUSitef.Valor;
                        linha["Cupom"] = vendaBilheteriaFormaPagamentoTEF.Cupom.Valor;
                        linha["DadosConfirmacaoVenda"] = vendaBilheteriaFormaPagamentoTEF.DadosConfirmacaoVenda.Valor;
                        linha["Rede"] = vendaBilheteriaFormaPagamentoTEF.Rede.Valor;
                        linha["CodigoRespostaTransacao"] = vendaBilheteriaFormaPagamentoTEF.CodigoRespostaTransacao.Valor;
                        linha["CartaoID"] = vendaBilheteriaFormaPagamentoTEF.CartaoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioVendaBilheteriaFormaPagamentoTEF");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("CodigoRespostaVenda", typeof(int));
                    tabela.Columns.Add("MensagemRetorno", typeof(string));
                    tabela.Columns.Add("HoraTransacao", typeof(string));
                    tabela.Columns.Add("DataTransacao", typeof(string));
                    tabela.Columns.Add("CodigoIR", typeof(string));
                    tabela.Columns.Add("NumeroAutorizacao", typeof(string));
                    tabela.Columns.Add("NSUHost", typeof(int));
                    tabela.Columns.Add("NSUSitef", typeof(int));
                    tabela.Columns.Add("Cupom", typeof(string));
                    tabela.Columns.Add("DadosConfirmacaoVenda", typeof(string));
                    tabela.Columns.Add("Rede", typeof(int));
                    tabela.Columns.Add("CodigoRespostaTransacao", typeof(int));
                    tabela.Columns.Add("CartaoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CodigoRespostaVenda"] = vendaBilheteriaFormaPagamentoTEF.CodigoRespostaVenda.Valor;
                        linha["MensagemRetorno"] = vendaBilheteriaFormaPagamentoTEF.MensagemRetorno.Valor;
                        linha["HoraTransacao"] = vendaBilheteriaFormaPagamentoTEF.HoraTransacao.Valor;
                        linha["DataTransacao"] = vendaBilheteriaFormaPagamentoTEF.DataTransacao.Valor;
                        linha["CodigoIR"] = vendaBilheteriaFormaPagamentoTEF.CodigoIR.Valor;
                        linha["NumeroAutorizacao"] = vendaBilheteriaFormaPagamentoTEF.NumeroAutorizacao.Valor;
                        linha["NSUHost"] = vendaBilheteriaFormaPagamentoTEF.NSUHost.Valor;
                        linha["NSUSitef"] = vendaBilheteriaFormaPagamentoTEF.NSUSitef.Valor;
                        linha["Cupom"] = vendaBilheteriaFormaPagamentoTEF.Cupom.Valor;
                        linha["DadosConfirmacaoVenda"] = vendaBilheteriaFormaPagamentoTEF.DadosConfirmacaoVenda.Valor;
                        linha["Rede"] = vendaBilheteriaFormaPagamentoTEF.Rede.Valor;
                        linha["CodigoRespostaTransacao"] = vendaBilheteriaFormaPagamentoTEF.CodigoRespostaTransacao.Valor;
                        linha["CartaoID"] = vendaBilheteriaFormaPagamentoTEF.CartaoID.Valor;
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
                    case "CodigoRespostaVenda":
                        sql = "SELECT ID, CodigoRespostaVenda FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY CodigoRespostaVenda";
                        break;
                    case "MensagemRetorno":
                        sql = "SELECT ID, MensagemRetorno FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY MensagemRetorno";
                        break;
                    case "HoraTransacao":
                        sql = "SELECT ID, HoraTransacao FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY HoraTransacao";
                        break;
                    case "DataTransacao":
                        sql = "SELECT ID, DataTransacao FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY DataTransacao";
                        break;
                    case "CodigoIR":
                        sql = "SELECT ID, CodigoIR FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY CodigoIR";
                        break;
                    case "NumeroAutorizacao":
                        sql = "SELECT ID, NumeroAutorizacao FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY NumeroAutorizacao";
                        break;
                    case "NSUHost":
                        sql = "SELECT ID, NSUHost FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY NSUHost";
                        break;
                    case "NSUSitef":
                        sql = "SELECT ID, NSUSitef FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY NSUSitef";
                        break;
                    case "Cupom":
                        sql = "SELECT ID, Cupom FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY Cupom";
                        break;
                    case "DadosConfirmacaoVenda":
                        sql = "SELECT ID, DadosConfirmacaoVenda FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY DadosConfirmacaoVenda";
                        break;
                    case "Rede":
                        sql = "SELECT ID, Rede FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY Rede";
                        break;
                    case "CodigoRespostaTransacao":
                        sql = "SELECT ID, CodigoRespostaTransacao FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY CodigoRespostaTransacao";
                        break;
                    case "CartaoID":
                        sql = "SELECT ID, CartaoID FROM tVendaBilheteriaFormaPagamentoTEF WHERE " + FiltroSQL + " ORDER BY CartaoID";
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

    #region "VendaBilheteriaFormaPagamentoTEFException"

    [Serializable]
    public class VendaBilheteriaFormaPagamentoTEFException : Exception
    {

        public VendaBilheteriaFormaPagamentoTEFException() : base() { }

        public VendaBilheteriaFormaPagamentoTEFException(string msg) : base(msg) { }

        public VendaBilheteriaFormaPagamentoTEFException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}