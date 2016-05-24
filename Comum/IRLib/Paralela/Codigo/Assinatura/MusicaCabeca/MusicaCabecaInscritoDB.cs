/******************************************************
* Arquivo MusicaCabecaInscritoDB.cs
* Gerado em: 13/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "MusicaCabecaInscrito_B"

    public abstract class MusicaCabecaInscrito_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public musicacabecaid MusicaCabecaID = new musicacabecaid();
        public presente Presente = new presente();
        public assinante Assinante = new assinante();
        public datainscricao DataInscricao = new datainscricao();

        public MusicaCabecaInscrito_B() { }

        /// <summary>
        /// Preenche todos os atributos de MusicaCabecaInscrito
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tMusicaCabecaInscrito WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.MusicaCabecaID.ValorBD = bd.LerInt("MusicaCabecaID").ToString();
                    this.Presente.ValorBD = bd.LerString("Presente");
                    this.Assinante.ValorBD = bd.LerString("Assinante");
                    this.DataInscricao.ValorBD = bd.LerString("DataInscricao");
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
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Inserir novo(a) MusicaCabecaInscrito
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tMusicaCabecaInscrito(ClienteID, MusicaCabecaID, Presente, Assinante, DataInscricao) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.MusicaCabecaID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.Assinante.ValorBD);
                sql.Replace("@005", this.DataInscricao.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

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
        /// Inserir novo(a) MusicaCabecaInscrito
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tMusicaCabecaInscrito(ClienteID, MusicaCabecaID, Presente, Assinante, DataInscricao) ");
            sql.Append("VALUES (@001,@002,'@003','@004','@005'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.MusicaCabecaID.ValorBD);
            sql.Replace("@003", this.Presente.ValorBD);
            sql.Replace("@004", this.Assinante.ValorBD);
            sql.Replace("@005", this.DataInscricao.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza MusicaCabecaInscrito
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMusicaCabecaInscrito SET ClienteID = @001, MusicaCabecaID = @002, Presente = '@003', Assinante = '@004', DataInscricao = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.MusicaCabecaID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.Assinante.ValorBD);
                sql.Replace("@005", this.DataInscricao.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

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
        /// Atualiza MusicaCabecaInscrito
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tMusicaCabecaInscrito SET ClienteID = @001, MusicaCabecaID = @002, Presente = '@003', Assinante = '@004', DataInscricao = '@005' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.MusicaCabecaID.ValorBD);
            sql.Replace("@003", this.Presente.ValorBD);
            sql.Replace("@004", this.Assinante.ValorBD);
            sql.Replace("@005", this.DataInscricao.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui MusicaCabecaInscrito com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tMusicaCabecaInscrito WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

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
        /// Exclui MusicaCabecaInscrito com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tMusicaCabecaInscrito WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui MusicaCabecaInscrito
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

            this.ClienteID.Limpar();
            this.MusicaCabecaID.Limpar();
            this.Presente.Limpar();
            this.Assinante.Limpar();
            this.DataInscricao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.MusicaCabecaID.Desfazer();
            this.Presente.Desfazer();
            this.Assinante.Desfazer();
            this.DataInscricao.Desfazer();
        }

        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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

        public class musicacabecaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MusicaCabecaID";
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

        public class presente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Presente";
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

        public class assinante : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Assinante";
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

        public class datainscricao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataInscricao";
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

                DataTable tabela = new DataTable("MusicaCabecaInscrito");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("MusicaCabecaID", typeof(int));
                tabela.Columns.Add("Presente", typeof(bool));
                tabela.Columns.Add("Assinante", typeof(bool));
                tabela.Columns.Add("DataInscricao", typeof(DateTime));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "MusicaCabecaInscritoLista_B"

    public abstract class MusicaCabecaInscritoLista_B : BaseLista
    {

        protected MusicaCabecaInscrito musicaCabecaInscrito;

        // passar o Usuario logado no sistema
        public MusicaCabecaInscritoLista_B()
        {
            musicaCabecaInscrito = new MusicaCabecaInscrito();
        }

        public MusicaCabecaInscrito MusicaCabecaInscrito
        {
            get { return musicaCabecaInscrito; }
        }

        /// <summary>
        /// Retorna um IBaseBD de MusicaCabecaInscrito especifico
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
                    musicaCabecaInscrito.Ler(id);
                    return musicaCabecaInscrito;
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
                    sql = "SELECT ID FROM tMusicaCabecaInscrito";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabecaInscrito";

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
                    sql = "SELECT ID FROM tMusicaCabecaInscrito";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabecaInscrito";

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
        /// Preenche MusicaCabecaInscrito corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                musicaCabecaInscrito.Ler(id);

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

                bool ok = musicaCabecaInscrito.Excluir();
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

                    try
                    {
                        string ids = ToString();

                        string sqlDelete = "DELETE FROM tMusicaCabecaInscrito WHERE ID in (" + ids + ")";

                        int x = bd.Executar(sqlDelete);
                        bd.Fechar();

                        ok = Convert.ToBoolean(x);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

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
        /// Inseri novo(a) MusicaCabecaInscrito na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = musicaCabecaInscrito.Inserir();
                if (ok)
                {
                    lista.Add(musicaCabecaInscrito.Control.ID);
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
        /// Obtem uma tabela de todos os campos de MusicaCabecaInscrito carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("MusicaCabecaInscrito");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("MusicaCabecaID", typeof(int));
                tabela.Columns.Add("Presente", typeof(bool));
                tabela.Columns.Add("Assinante", typeof(bool));
                tabela.Columns.Add("DataInscricao", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = musicaCabecaInscrito.Control.ID;
                        linha["ClienteID"] = musicaCabecaInscrito.ClienteID.Valor;
                        linha["MusicaCabecaID"] = musicaCabecaInscrito.MusicaCabecaID.Valor;
                        linha["Presente"] = musicaCabecaInscrito.Presente.Valor;
                        linha["Assinante"] = musicaCabecaInscrito.Assinante.Valor;
                        linha["DataInscricao"] = musicaCabecaInscrito.DataInscricao.Valor;
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

                DataTable tabela = new DataTable("RelatorioMusicaCabecaInscrito");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("MusicaCabecaID", typeof(int));
                    tabela.Columns.Add("Presente", typeof(bool));
                    tabela.Columns.Add("Assinante", typeof(bool));
                    tabela.Columns.Add("DataInscricao", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = musicaCabecaInscrito.ClienteID.Valor;
                        linha["MusicaCabecaID"] = musicaCabecaInscrito.MusicaCabecaID.Valor;
                        linha["Presente"] = musicaCabecaInscrito.Presente.Valor;
                        linha["Assinante"] = musicaCabecaInscrito.Assinante.Valor;
                        linha["DataInscricao"] = musicaCabecaInscrito.DataInscricao.Valor;
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
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tMusicaCabecaInscrito WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "MusicaCabecaID":
                        sql = "SELECT ID, MusicaCabecaID FROM tMusicaCabecaInscrito WHERE " + FiltroSQL + " ORDER BY MusicaCabecaID";
                        break;
                    case "Presente":
                        sql = "SELECT ID, Presente FROM tMusicaCabecaInscrito WHERE " + FiltroSQL + " ORDER BY Presente";
                        break;
                    case "Assinante":
                        sql = "SELECT ID, Assinante FROM tMusicaCabecaInscrito WHERE " + FiltroSQL + " ORDER BY Assinante";
                        break;
                    case "DataInscricao":
                        sql = "SELECT ID, DataInscricao FROM tMusicaCabecaInscrito WHERE " + FiltroSQL + " ORDER BY DataInscricao";
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

    #region "MusicaCabecaInscritoException"

    [Serializable]
    public class MusicaCabecaInscritoException : Exception
    {

        public MusicaCabecaInscritoException() : base() { }

        public MusicaCabecaInscritoException(string msg) : base(msg) { }

        public MusicaCabecaInscritoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}