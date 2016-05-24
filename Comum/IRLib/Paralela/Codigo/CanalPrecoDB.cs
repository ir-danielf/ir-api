/******************************************************
* Arquivo CanalPrecoDB.cs
* Gerado em: 15/09/2006
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "CanalPreco_B"

    public abstract class CanalPreco_B : BaseBD
    {

        public canalid CanalID = new canalid();
        public precoid PrecoID = new precoid();
        public datainicio DataInicio = new datainicio();
        public datafim DataFim = new datafim();
        public quantidade Quantidade = new quantidade();

        public CanalPreco_B() { }

        // passar o Usuario logado no sistema
        public CanalPreco_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CanalPreco
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCanalPreco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.DataInicio.ValorBD = bd.LerString("DataInicio");
                    this.DataFim.ValorBD = bd.LerString("DataFim");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
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
        /// Preenche todos os atributos de CanalPreco do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCanalPreco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.DataInicio.ValorBD = bd.LerString("DataInicio");
                    this.DataFim.ValorBD = bd.LerString("DataFim");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
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
                sql.Append("INSERT INTO cCanalPreco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao = this.Control.Versao + 1;

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

                sql.Append("INSERT INTO xCanalPreco (ID, Versao, CanalID, PrecoID, DataInicio, DataFim, Quantidade) ");
                sql.Append("SELECT ID, @V, CanalID, PrecoID, DataInicio, DataFim, Quantidade FROM tCanalPreco WHERE ID = @I");
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
        /// Inserir novo(a) CanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCanalPreco(CanalID, PrecoID, DataInicio, DataFim, Quantidade) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005); SELECT SCOPE_IDENTITY()");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PrecoID.ValorBD);
                sql.Replace("@003", this.DataInicio.ValorBD);
                sql.Replace("@004", this.DataFim.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);

                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = (this.Control.ID > 0);

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
        /// Atualiza CanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCanalPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanalPreco SET CanalID = @001, PrecoID = @002, DataInicio = '@003', DataFim = '@004', Quantidade = @005 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PrecoID.ValorBD);
                sql.Replace("@003", this.DataInicio.ValorBD);
                sql.Replace("@004", this.DataFim.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);

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
        /// Exclui CanalPreco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {
            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalPreco WHERE ID=" + id;

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
        /// Exclui CanalPreco
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

            this.CanalID.Limpar();
            this.PrecoID.Limpar();
            this.DataInicio.Limpar();
            this.DataFim.Limpar();
            this.Quantidade.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.CanalID.Desfazer();
            this.PrecoID.Desfazer();
            this.DataInicio.Desfazer();
            this.DataFim.Desfazer();
            this.Quantidade.Desfazer();
        }

        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
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

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class datainicio : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataInicio";
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
                return base.Valor.ToString("dd/MM/yyyy");
            }

        }

        public class datafim : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataFim";
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
                return base.Valor.ToString("dd/MM/yyyy");
            }

        }

        public class quantidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Quantidade";
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

                DataTable tabela = new DataTable("CanalPreco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("DataInicio", typeof(DateTime));
                tabela.Columns.Add("DataFim", typeof(DateTime));
                tabela.Columns.Add("Quantidade", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Precos(int canalid);

        public abstract int Validade();

        public abstract int Validade(int canalid, int precoid);

        public abstract DateTime DataValidade();

        public abstract DateTime DataValidade(int canalid, int precoid);

        public abstract int QuantidadeDisponivel();

        public abstract int QuantidadeDisponivel(int canalid, int precoid);

        public abstract DataTable Canais();

        public abstract DataTable Canais(int precoid);

    }
    #endregion

    #region "CanalPrecoLista_B"

    public abstract class CanalPrecoLista_B : BaseLista
    {

        private bool backup = false;
        protected CanalPreco canalPreco;

        // passar o Usuario logado no sistema
        public CanalPrecoLista_B()
        {
            canalPreco = new CanalPreco();
        }

        // passar o Usuario logado no sistema
        public CanalPrecoLista_B(int usuarioIDLogado)
        {
            canalPreco = new CanalPreco(usuarioIDLogado);
        }

        public CanalPreco CanalPreco
        {
            get { return canalPreco; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CanalPreco especifico
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
                    canalPreco.Ler(id);
                    return canalPreco;
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
                    sql = "SELECT ID FROM tCanalPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanalPreco";

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
                    sql = "SELECT ID FROM tCanalPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanalPreco";

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
                    sql = "SELECT ID FROM xCanalPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCanalPreco";

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
        /// Preenche CanalPreco corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    canalPreco.Ler(id);
                else
                    canalPreco.LerBackup(id);

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

                bool ok = canalPreco.Excluir();
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
        /// Inseri novo(a) CanalPreco na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = canalPreco.Inserir();
                if (ok)
                {
                    lista.Add(canalPreco.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CanalPreco carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CanalPreco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("DataInicio", typeof(DateTime));
                tabela.Columns.Add("DataFim", typeof(DateTime));
                tabela.Columns.Add("Quantidade", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = canalPreco.Control.ID;
                        linha["CanalID"] = canalPreco.CanalID.Valor;
                        linha["PrecoID"] = canalPreco.PrecoID.Valor;
                        linha["DataInicio"] = canalPreco.DataInicio.Valor;
                        linha["DataFim"] = canalPreco.DataFim.Valor;
                        linha["Quantidade"] = canalPreco.Quantidade.Valor;
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

                DataTable tabela = new DataTable("RelatorioCanalPreco");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("DataInicio", typeof(DateTime));
                    tabela.Columns.Add("DataFim", typeof(DateTime));
                    tabela.Columns.Add("Quantidade", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CanalID"] = canalPreco.CanalID.Valor;
                        linha["PrecoID"] = canalPreco.PrecoID.Valor;
                        linha["DataInicio"] = canalPreco.DataInicio.Valor;
                        linha["DataFim"] = canalPreco.DataFim.Valor;
                        linha["Quantidade"] = canalPreco.Quantidade.Valor;
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
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tCanalPreco WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tCanalPreco WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "DataInicio":
                        sql = "SELECT ID, DataInicio FROM tCanalPreco WHERE " + FiltroSQL + " ORDER BY DataInicio";
                        break;
                    case "DataFim":
                        sql = "SELECT ID, DataFim FROM tCanalPreco WHERE " + FiltroSQL + " ORDER BY DataFim";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tCanalPreco WHERE " + FiltroSQL + " ORDER BY Quantidade";
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

    #region "CanalPrecoException"

    [Serializable]
    public class CanalPrecoException : Exception
    {

        public CanalPrecoException() : base() { }

        public CanalPrecoException(string msg) : base(msg) { }

        public CanalPrecoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}