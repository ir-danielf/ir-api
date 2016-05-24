

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "EventoTipoPagamento_B"

    public abstract class EventoTipoPagamento_B : BaseBD
    {


        public eventoid EventoID = new eventoid();

        public formapagamentoid FormaPagamentoID = new formapagamentoid();

        public dias Dias = new dias();


        public EventoTipoPagamento_B() { }

        // passar o Usuario logado no sistema
        public EventoTipoPagamento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EventoTipoPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEventoTipoPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();

                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();

                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();

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
        /// Preenche todos os atributos de EventoTipoPagamento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEventoTipoPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();

                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();

                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();

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
                sql.Append("INSERT INTO cEventoTipoPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


                sql.Append("INSERT INTO xEventoTipoPagamento (ID, Versao, EventoID, FormaPagamentoID, Dias) ");
                sql.Append("SELECT ID, @V, EventoID, FormaPagamentoID, Dias FROM tEventoTipoPagamento WHERE ID = @I");
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
        /// Inserir novo(a) EventoTipoPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEventoTipoPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoTipoPagamento(ID, EventoID, FormaPagamentoID, Dias) ");
                sql.Append("VALUES (@ID,@001,@002,@003)");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.EventoID.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.Dias.ValorBD);


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
        /// Inserir novo(a) EventoTipoPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEventoTipoPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoTipoPagamento(ID, EventoID, FormaPagamentoID, Dias) ");
                sql.Append("VALUES (@ID,@001,@002,@003)");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.EventoID.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.Dias.ValorBD);


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
        /// Atualiza EventoTipoPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEventoTipoPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tEventoTipoPagamento SET EventoID = @001, FormaPagamentoID = @002, Dias = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.EventoID.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.Dias.ValorBD);


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
        /// Atualiza EventoTipoPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEventoTipoPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tEventoTipoPagamento SET EventoID = @001, FormaPagamentoID = @002, Dias = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.EventoID.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.Dias.ValorBD);


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
        /// Exclui EventoTipoPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEventoTipoPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEventoTipoPagamento WHERE ID=" + id;

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
        /// Exclui EventoTipoPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEventoTipoPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEventoTipoPagamento WHERE ID=" + id;

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
        /// Exclui EventoTipoPagamento
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


            this.EventoID.Limpar();

            this.FormaPagamentoID.Limpar();

            this.Dias.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.EventoID.Desfazer();

            this.FormaPagamentoID.Desfazer();

            this.Dias.Desfazer();

        }


        public class eventoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoID";
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


        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
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


        public class dias : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Dias";
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

                DataTable tabela = new DataTable("EventoTipoPagamento");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("EventoID", typeof(int));

                tabela.Columns.Add("FormaPagamentoID", typeof(int));

                tabela.Columns.Add("Dias", typeof(int));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "EventoTipoPagamentoLista_B"


    public abstract class EventoTipoPagamentoLista_B : BaseLista
    {

        private bool backup = false;
        protected EventoTipoPagamento eventoTipoPagamento;

        // passar o Usuario logado no sistema
        public EventoTipoPagamentoLista_B()
        {
            eventoTipoPagamento = new EventoTipoPagamento();
        }

        // passar o Usuario logado no sistema
        public EventoTipoPagamentoLista_B(int usuarioIDLogado)
        {
            eventoTipoPagamento = new EventoTipoPagamento(usuarioIDLogado);
        }

        public EventoTipoPagamento EventoTipoPagamento
        {
            get { return eventoTipoPagamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EventoTipoPagamento especifico
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
                    eventoTipoPagamento.Ler(id);
                    return eventoTipoPagamento;
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
                    sql = "SELECT ID FROM tEventoTipoPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEventoTipoPagamento";

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
                    sql = "SELECT ID FROM tEventoTipoPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEventoTipoPagamento";

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
                    sql = "SELECT ID FROM xEventoTipoPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEventoTipoPagamento";

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
        /// Preenche EventoTipoPagamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    eventoTipoPagamento.Ler(id);
                else
                    eventoTipoPagamento.LerBackup(id);

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

                bool ok = eventoTipoPagamento.Excluir();
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
        /// Inseri novo(a) EventoTipoPagamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = eventoTipoPagamento.Inserir();
                if (ok)
                {
                    lista.Add(eventoTipoPagamento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EventoTipoPagamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EventoTipoPagamento");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("EventoID", typeof(int));

                tabela.Columns.Add("FormaPagamentoID", typeof(int));

                tabela.Columns.Add("Dias", typeof(int));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = eventoTipoPagamento.Control.ID;

                        linha["EventoID"] = eventoTipoPagamento.EventoID.Valor;

                        linha["FormaPagamentoID"] = eventoTipoPagamento.FormaPagamentoID.Valor;

                        linha["Dias"] = eventoTipoPagamento.Dias.Valor;

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

                DataTable tabela = new DataTable("RelatorioEventoTipoPagamento");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("EventoID", typeof(int));

                    tabela.Columns.Add("FormaPagamentoID", typeof(int));

                    tabela.Columns.Add("Dias", typeof(int));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["EventoID"] = eventoTipoPagamento.EventoID.Valor;

                        linha["FormaPagamentoID"] = eventoTipoPagamento.FormaPagamentoID.Valor;

                        linha["Dias"] = eventoTipoPagamento.Dias.Valor;

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

                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tEventoTipoPagamento WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;

                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tEventoTipoPagamento WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;

                    case "Dias":
                        sql = "SELECT ID, Dias FROM tEventoTipoPagamento WHERE " + FiltroSQL + " ORDER BY Dias";
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

    #region "EventoTipoPagamentoException"

    [Serializable]
    public class EventoTipoPagamentoException : Exception
    {

        public EventoTipoPagamentoException() : base() { }

        public EventoTipoPagamentoException(string msg) : base(msg) { }

        public EventoTipoPagamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}