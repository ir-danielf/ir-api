/******************************************************
* Arquivo PrecoExclusivoDB.cs
* Gerado em: 13/11/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "PrecoExclusivo_B"

    public abstract class PrecoExclusivo_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public nome Nome = new nome();
        public precoiniciacom PrecoIniciaCom = new precoiniciacom();
        public quantidademaxima QuantidadeMaxima = new quantidademaxima();
        public ativo Ativo = new ativo();

        public PrecoExclusivo_B() { }

        // passar o Usuario logado no sistema
        public PrecoExclusivo_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de PrecoExclusivo
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tPrecoExclusivo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.PrecoIniciaCom.ValorBD = bd.LerString("PrecoIniciaCom");
                    this.QuantidadeMaxima.ValorBD = bd.LerInt("QuantidadeMaxima").ToString();
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
        /// Preenche todos os atributos de PrecoExclusivo do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xPrecoExclusivo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.PrecoIniciaCom.ValorBD = bd.LerString("PrecoIniciaCom");
                    this.QuantidadeMaxima.ValorBD = bd.LerInt("QuantidadeMaxima").ToString();
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
                sql.Append("INSERT INTO cPrecoExclusivo (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xPrecoExclusivo (ID, Versao, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo) ");
                sql.Append("SELECT ID, @V, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo FROM tPrecoExclusivo WHERE ID = @I");
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
        /// Inserir novo(a) PrecoExclusivo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPrecoExclusivo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPrecoExclusivo(ID, EventoID, Nome, PrecoIniciaCom, QuantidadeMaxima, Ativo) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@004", this.QuantidadeMaxima.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);

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
        /// Atualiza PrecoExclusivo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cPrecoExclusivo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPrecoExclusivo SET EventoID = @001, Nome = '@002', PrecoIniciaCom = '@003', QuantidadeMaxima = @004, Ativo = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@004", this.QuantidadeMaxima.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);

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
        /// Exclui PrecoExclusivo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPrecoExclusivo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPrecoExclusivo WHERE ID=" + id;

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
        /// Exclui PrecoExclusivo
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
            this.Nome.Limpar();
            this.PrecoIniciaCom.Limpar();
            this.QuantidadeMaxima.Limpar();
            this.Ativo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EventoID.Desfazer();
            this.Nome.Desfazer();
            this.PrecoIniciaCom.Desfazer();
            this.QuantidadeMaxima.Desfazer();
            this.Ativo.Desfazer();
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

        public class precoiniciacom : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoIniciaCom";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
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

        public class quantidademaxima : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMaxima";
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

        public class ativo : BooleanProperty
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

                DataTable tabela = new DataTable("PrecoExclusivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Todos();

        public abstract DataTable ListagemPorPrecoExclusivo(int precoexclusivoid);

        public abstract DataTable ListagemPorEventoID(int eventoid);

    }
    #endregion

    #region "PrecoExclusivoLista_B"

    public abstract class PrecoExclusivoLista_B : BaseLista
    {

        private bool backup = false;
        protected PrecoExclusivo precoExclusivo;

        // passar o Usuario logado no sistema
        public PrecoExclusivoLista_B()
        {
            precoExclusivo = new PrecoExclusivo();
        }

        // passar o Usuario logado no sistema
        public PrecoExclusivoLista_B(int usuarioIDLogado)
        {
            precoExclusivo = new PrecoExclusivo(usuarioIDLogado);
        }

        public PrecoExclusivo PrecoExclusivo
        {
            get { return precoExclusivo; }
        }

        /// <summary>
        /// Retorna um IBaseBD de PrecoExclusivo especifico
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
                    precoExclusivo.Ler(id);
                    return precoExclusivo;
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
                    sql = "SELECT ID FROM tPrecoExclusivo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPrecoExclusivo";

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
                    sql = "SELECT ID FROM tPrecoExclusivo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPrecoExclusivo";

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
                    sql = "SELECT ID FROM xPrecoExclusivo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xPrecoExclusivo";

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
        /// Preenche PrecoExclusivo corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    precoExclusivo.Ler(id);
                else
                    precoExclusivo.LerBackup(id);

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

                bool ok = precoExclusivo.Excluir();
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
        /// Inseri novo(a) PrecoExclusivo na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = precoExclusivo.Inserir();
                if (ok)
                {
                    lista.Add(precoExclusivo.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de PrecoExclusivo carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("PrecoExclusivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = precoExclusivo.Control.ID;
                        linha["EventoID"] = precoExclusivo.EventoID.Valor;
                        linha["Nome"] = precoExclusivo.Nome.Valor;
                        linha["PrecoIniciaCom"] = precoExclusivo.PrecoIniciaCom.Valor;
                        linha["QuantidadeMaxima"] = precoExclusivo.QuantidadeMaxima.Valor;
                        linha["Ativo"] = precoExclusivo.Ativo.Valor;
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

                DataTable tabela = new DataTable("RelatorioPrecoExclusivo");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                    tabela.Columns.Add("QuantidadeMaxima", typeof(int));
                    tabela.Columns.Add("Ativo", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = precoExclusivo.EventoID.Valor;
                        linha["Nome"] = precoExclusivo.Nome.Valor;
                        linha["PrecoIniciaCom"] = precoExclusivo.PrecoIniciaCom.Valor;
                        linha["QuantidadeMaxima"] = precoExclusivo.QuantidadeMaxima.Valor;
                        linha["Ativo"] = precoExclusivo.Ativo.Valor;
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
                        sql = "SELECT ID, EventoID FROM tPrecoExclusivo WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tPrecoExclusivo WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "PrecoIniciaCom":
                        sql = "SELECT ID, PrecoIniciaCom FROM tPrecoExclusivo WHERE " + FiltroSQL + " ORDER BY PrecoIniciaCom";
                        break;
                    case "QuantidadeMaxima":
                        sql = "SELECT ID, QuantidadeMaxima FROM tPrecoExclusivo WHERE " + FiltroSQL + " ORDER BY QuantidadeMaxima";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tPrecoExclusivo WHERE " + FiltroSQL + " ORDER BY Ativo";
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

    #region "PrecoExclusivoException"

    [Serializable]
    public class PrecoExclusivoException : Exception
    {

        public PrecoExclusivoException() : base() { }

        public PrecoExclusivoException(string msg) : base(msg) { }

        public PrecoExclusivoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}