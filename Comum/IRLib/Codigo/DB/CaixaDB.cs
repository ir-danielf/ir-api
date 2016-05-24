/******************************************************
* Arquivo CaixaDB.cs
* Gerado em: 20/12/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Caixa_B"

    public abstract class Caixa_B : BaseBD
    {

        public usuarioid UsuarioID = new usuarioid();
        public lojaid LojaID = new lojaid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public saldoinicial SaldoInicial = new saldoinicial();
        public dataabertura DataAbertura = new dataabertura();
        public datafechamento DataFechamento = new datafechamento();
        public comissao Comissao = new comissao();
        public conciliacaoid ConciliacaoID = new conciliacaoid();

        public Caixa_B() { }

        // passar o Usuario logado no sistema
        public Caixa_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Caixa
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCaixa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SaldoInicial.ValorBD = bd.LerDecimal("SaldoInicial").ToString();
                    this.DataAbertura.ValorBD = bd.LerString("DataAbertura");
                    this.DataFechamento.ValorBD = bd.LerString("DataFechamento");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.ConciliacaoID.ValorBD = bd.LerInt("ConciliacaoID").ToString();
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

        public void LerCaixaDetalhe(string data)
        {

            try
            {
                    this.DataAbertura.ValorBD = data;
                    this.DataFechamento.ValorBD = data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// Preenche todos os atributos de Caixa do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCaixa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SaldoInicial.ValorBD = bd.LerDecimal("SaldoInicial").ToString();
                    this.DataAbertura.ValorBD = bd.LerString("DataAbertura");
                    this.DataFechamento.ValorBD = bd.LerString("DataFechamento");
                    this.Comissao.ValorBD = bd.LerInt("Comissao").ToString();
                    this.ConciliacaoID.ValorBD = bd.LerInt("ConciliacaoID").ToString();
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
                sql.Append("INSERT INTO cCaixa (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCaixa (ID, Versao, UsuarioID, LojaID, ApresentacaoID, SaldoInicial, DataAbertura, DataFechamento, Comissao, ConciliacaoID) ");
                sql.Append("SELECT ID, @V, UsuarioID, LojaID, ApresentacaoID, SaldoInicial, DataAbertura, DataFechamento, Comissao, ConciliacaoID FROM tCaixa WHERE ID = @I");
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
        /// Inserir novo(a) Caixa
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TCAIXA");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCaixa(ID, UsuarioID, LojaID, ApresentacaoID, SaldoInicial, DataAbertura, DataFechamento, Comissao, ConciliacaoID) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005','@006',@007,@008)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.LojaID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.SaldoInicial.ValorBD);
                sql.Replace("@005", this.DataAbertura.ValorBD);
                sql.Replace("@006", this.DataFechamento.ValorBD);
                sql.Replace("@007", this.Comissao.ValorBD);
                sql.Replace("@008", this.ConciliacaoID.ValorBD);

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
        /// Inserir novo(a) Caixa
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TCAIXA");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCaixa(ID, UsuarioID, LojaID, ApresentacaoID, SaldoInicial, DataAbertura, DataFechamento, Comissao, ConciliacaoID) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005','@006',@007,@008)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.LojaID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.SaldoInicial.ValorBD);
                sql.Replace("@005", this.DataAbertura.ValorBD);
                sql.Replace("@006", this.DataFechamento.ValorBD);
                sql.Replace("@007", this.Comissao.ValorBD);
                sql.Replace("@008", this.ConciliacaoID.ValorBD);

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
        /// Atualiza Caixa
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCaixa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCaixa SET UsuarioID = @001, LojaID = @002, ApresentacaoID = @003, SaldoInicial = '@004', DataAbertura = '@005', DataFechamento = '@006', Comissao = @007, ConciliacaoID = @008 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.LojaID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.SaldoInicial.ValorBD);
                sql.Replace("@005", this.DataAbertura.ValorBD);
                sql.Replace("@006", this.DataFechamento.ValorBD);
                sql.Replace("@007", this.Comissao.ValorBD);
                sql.Replace("@008", this.ConciliacaoID.ValorBD);

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
        /// Atualiza Caixa
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCaixa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCaixa SET UsuarioID = @001, LojaID = @002, ApresentacaoID = @003, SaldoInicial = '@004', DataAbertura = '@005', DataFechamento = '@006', Comissao = @007, ConciliacaoID = @008 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.LojaID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.SaldoInicial.ValorBD);
                sql.Replace("@005", this.DataAbertura.ValorBD);
                sql.Replace("@006", this.DataFechamento.ValorBD);
                sql.Replace("@007", this.Comissao.ValorBD);
                sql.Replace("@008", this.ConciliacaoID.ValorBD);

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
        /// Exclui Caixa com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCaixa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCaixa WHERE ID=" + id;

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
        /// Exclui Caixa com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCaixa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCaixa WHERE ID=" + id;

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
        /// Exclui Caixa
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

            this.UsuarioID.Limpar();
            this.LojaID.Limpar();
            this.ApresentacaoID.Limpar();
            this.SaldoInicial.Limpar();
            this.DataAbertura.Limpar();
            this.DataFechamento.Limpar();
            this.Comissao.Limpar();
            this.ConciliacaoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.UsuarioID.Desfazer();
            this.LojaID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.SaldoInicial.Desfazer();
            this.DataAbertura.Desfazer();
            this.DataFechamento.Desfazer();
            this.Comissao.Desfazer();
            this.ConciliacaoID.Desfazer();
        }

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
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

        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class saldoinicial : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "SaldoInicial";
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

        public class dataabertura : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataAbertura";
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

        public class datafechamento : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataFechamento";
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

        public class conciliacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ConciliacaoID";
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

                DataTable tabela = new DataTable("Caixa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SaldoInicial", typeof(decimal));
                tabela.Columns.Add("DataAbertura", typeof(DateTime));
                tabela.Columns.Add("DataFechamento", typeof(DateTime));
                tabela.Columns.Add("Comissao", typeof(int));
                tabela.Columns.Add("ConciliacaoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract bool Aberto(int usuarioid);

        public abstract bool Aberto(int usuarioid, int lojaid);

        public abstract bool Abrir(int usuarioid, int lojaid, int apresentacaoid, decimal saldoinicial);

        public abstract int QuantidadeIngressos(string status);

        public abstract string IngressoLogIDsPorPeriodoCaixaSQL(string datainicial, string datafinal, string status, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, int usuarioid, int lojaid, int canalid, string tipolinha, bool disponivel, bool vendascanal, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract string VendaSenhaPorPeriodoCaixaSQL(string datainicial, string datafinal, string status, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, int usuarioid, int lojaid, int canalid);

        public abstract DataTable LinhasVendasGerenciaisDias(string ingressologids);

        public abstract DataTable VendasGerenciaisDias(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract int QuantidadeIngressosPorDia(string ingressologids, string dia);

        public abstract decimal ValorIngressosPorDia(string ingressologids, string dia);

        public abstract DataTable VendasDetalhe();

        public abstract DataTable Reimpressos();

    }
    #endregion

    #region "CaixaLista_B"

    public abstract class CaixaLista_B : BaseLista
    {

        private bool backup = false;
        protected Caixa caixa;

        // passar o Usuario logado no sistema
        public CaixaLista_B()
        {
            caixa = new Caixa();
        }

        // passar o Usuario logado no sistema
        public CaixaLista_B(int usuarioIDLogado)
        {
            caixa = new Caixa(usuarioIDLogado);
        }

        public Caixa Caixa
        {
            get { return caixa; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Caixa especifico
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
                    caixa.Ler(id);
                    return caixa;
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
                    sql = "SELECT ID FROM tCaixa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCaixa";

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
                    sql = "SELECT ID FROM tCaixa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCaixa";

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
                    sql = "SELECT ID FROM xCaixa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCaixa";

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
        /// Preenche Caixa corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    caixa.Ler(id);
                else
                    caixa.LerBackup(id);

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

                bool ok = caixa.Excluir();
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
        /// Inseri novo(a) Caixa na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = caixa.Inserir();
                if (ok)
                {
                    lista.Add(caixa.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Caixa carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Caixa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SaldoInicial", typeof(decimal));
                tabela.Columns.Add("DataAbertura", typeof(DateTime));
                tabela.Columns.Add("DataFechamento", typeof(DateTime));
                tabela.Columns.Add("Comissao", typeof(int));
                tabela.Columns.Add("ConciliacaoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = caixa.Control.ID;
                        linha["UsuarioID"] = caixa.UsuarioID.Valor;
                        linha["LojaID"] = caixa.LojaID.Valor;
                        linha["ApresentacaoID"] = caixa.ApresentacaoID.Valor;
                        linha["SaldoInicial"] = caixa.SaldoInicial.Valor;
                        linha["DataAbertura"] = caixa.DataAbertura.Valor;
                        linha["DataFechamento"] = caixa.DataFechamento.Valor;
                        linha["Comissao"] = caixa.Comissao.Valor;
                        linha["ConciliacaoID"] = caixa.ConciliacaoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioCaixa");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("SaldoInicial", typeof(decimal));
                    tabela.Columns.Add("DataAbertura", typeof(DateTime));
                    tabela.Columns.Add("DataFechamento", typeof(DateTime));
                    tabela.Columns.Add("Comissao", typeof(int));
                    tabela.Columns.Add("ConciliacaoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["UsuarioID"] = caixa.UsuarioID.Valor;
                        linha["LojaID"] = caixa.LojaID.Valor;
                        linha["ApresentacaoID"] = caixa.ApresentacaoID.Valor;
                        linha["SaldoInicial"] = caixa.SaldoInicial.Valor;
                        linha["DataAbertura"] = caixa.DataAbertura.Valor;
                        linha["DataFechamento"] = caixa.DataFechamento.Valor;
                        linha["Comissao"] = caixa.Comissao.Valor;
                        linha["ConciliacaoID"] = caixa.ConciliacaoID.Valor;
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
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tCaixa WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tCaixa WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tCaixa WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "SaldoInicial":
                        sql = "SELECT ID, SaldoInicial FROM tCaixa WHERE " + FiltroSQL + " ORDER BY SaldoInicial";
                        break;
                    case "DataAbertura":
                        sql = "SELECT ID, DataAbertura FROM tCaixa WHERE " + FiltroSQL + " ORDER BY DataAbertura";
                        break;
                    case "DataFechamento":
                        sql = "SELECT ID, DataFechamento FROM tCaixa WHERE " + FiltroSQL + " ORDER BY DataFechamento";
                        break;
                    case "Comissao":
                        sql = "SELECT ID, Comissao FROM tCaixa WHERE " + FiltroSQL + " ORDER BY Comissao";
                        break;
                    case "ConciliacaoID":
                        sql = "SELECT ID, ConciliacaoID FROM tCaixa WHERE " + FiltroSQL + " ORDER BY ConciliacaoID";
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

    #region "CaixaException"

    [Serializable]
    public class CaixaException : Exception
    {

        public CaixaException() : base() { }

        public CaixaException(string msg) : base(msg) { }

        public CaixaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}