/******************************************************
* Arquivo MusicaCabecaAgregadoDB.cs
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

    #region "MusicaCabecaAgregado_B"

    public abstract class MusicaCabecaAgregado_B : BaseBD
    {

        public musicacabecainscritoid MusicaCabecaInscritoID = new musicacabecainscritoid();
        public agregadoid AgregadoID = new agregadoid();
        public presente Presente = new presente();
        public datainscricao DataInscricao = new datainscricao();

        public MusicaCabecaAgregado_B() { }

        /// <summary>
        /// Preenche todos os atributos de MusicaCabecaAgregado
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tMusicaCabecaAgregado WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.MusicaCabecaInscritoID.ValorBD = bd.LerInt("MusicaCabecaInscritoID").ToString();
                    this.AgregadoID.ValorBD = bd.LerInt("AgregadoID").ToString();
                    this.Presente.ValorBD = bd.LerString("Presente");
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
        /// Inserir novo(a) MusicaCabecaAgregado
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tMusicaCabecaAgregado(MusicaCabecaInscritoID, AgregadoID, Presente, DataInscricao) ");
                sql.Append("VALUES (@001,@002,'@003','@004'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.MusicaCabecaInscritoID.ValorBD);
                sql.Replace("@002", this.AgregadoID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.DataInscricao.ValorBD);

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
        /// Inserir novo(a) MusicaCabecaAgregado
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tMusicaCabecaAgregado(MusicaCabecaInscritoID, AgregadoID, Presente, DataInscricao) ");
            sql.Append("VALUES (@001,@002,'@003','@004'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.MusicaCabecaInscritoID.ValorBD);
            sql.Replace("@002", this.AgregadoID.ValorBD);
            sql.Replace("@003", this.Presente.ValorBD);
            sql.Replace("@004", this.DataInscricao.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza MusicaCabecaAgregado
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMusicaCabecaAgregado SET MusicaCabecaInscritoID = @001, AgregadoID = @002, Presente = '@003', DataInscricao = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.MusicaCabecaInscritoID.ValorBD);
                sql.Replace("@002", this.AgregadoID.ValorBD);
                sql.Replace("@003", this.Presente.ValorBD);
                sql.Replace("@004", this.DataInscricao.ValorBD);

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
        /// Atualiza MusicaCabecaAgregado
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tMusicaCabecaAgregado SET MusicaCabecaInscritoID = @001, AgregadoID = @002, Presente = '@003', DataInscricao = '@004' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.MusicaCabecaInscritoID.ValorBD);
            sql.Replace("@002", this.AgregadoID.ValorBD);
            sql.Replace("@003", this.Presente.ValorBD);
            sql.Replace("@004", this.DataInscricao.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui MusicaCabecaAgregado com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tMusicaCabecaAgregado WHERE ID=" + id;

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
        /// Exclui MusicaCabecaAgregado com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tMusicaCabecaAgregado WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui MusicaCabecaAgregado
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

            this.MusicaCabecaInscritoID.Limpar();
            this.AgregadoID.Limpar();
            this.Presente.Limpar();
            this.DataInscricao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.MusicaCabecaInscritoID.Desfazer();
            this.AgregadoID.Desfazer();
            this.Presente.Desfazer();
            this.DataInscricao.Desfazer();
        }

        public class musicacabecainscritoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MusicaCabecaInscritoID";
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

        public class agregadoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AgregadoID";
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

                DataTable tabela = new DataTable("MusicaCabecaAgregado");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("MusicaCabecaInscritoID", typeof(int));
                tabela.Columns.Add("AgregadoID", typeof(int));
                tabela.Columns.Add("Presente", typeof(bool));
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

    #region "MusicaCabecaAgregadoLista_B"

    public abstract class MusicaCabecaAgregadoLista_B : BaseLista
    {

        protected MusicaCabecaAgregado musicaCabecaAgregado;

        // passar o Usuario logado no sistema
        public MusicaCabecaAgregadoLista_B()
        {
            musicaCabecaAgregado = new MusicaCabecaAgregado();
        }

        public MusicaCabecaAgregado MusicaCabecaAgregado
        {
            get { return musicaCabecaAgregado; }
        }

        /// <summary>
        /// Retorna um IBaseBD de MusicaCabecaAgregado especifico
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
                    musicaCabecaAgregado.Ler(id);
                    return musicaCabecaAgregado;
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
                    sql = "SELECT ID FROM tMusicaCabecaAgregado";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabecaAgregado";

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
                    sql = "SELECT ID FROM tMusicaCabecaAgregado";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabecaAgregado";

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
        /// Preenche MusicaCabecaAgregado corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                musicaCabecaAgregado.Ler(id);

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

                bool ok = musicaCabecaAgregado.Excluir();
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

                        string sqlDelete = "DELETE FROM tMusicaCabecaAgregado WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) MusicaCabecaAgregado na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = musicaCabecaAgregado.Inserir();
                if (ok)
                {
                    lista.Add(musicaCabecaAgregado.Control.ID);
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
        /// Obtem uma tabela de todos os campos de MusicaCabecaAgregado carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("MusicaCabecaAgregado");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("MusicaCabecaInscritoID", typeof(int));
                tabela.Columns.Add("AgregadoID", typeof(int));
                tabela.Columns.Add("Presente", typeof(bool));
                tabela.Columns.Add("DataInscricao", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = musicaCabecaAgregado.Control.ID;
                        linha["MusicaCabecaInscritoID"] = musicaCabecaAgregado.MusicaCabecaInscritoID.Valor;
                        linha["AgregadoID"] = musicaCabecaAgregado.AgregadoID.Valor;
                        linha["Presente"] = musicaCabecaAgregado.Presente.Valor;
                        linha["DataInscricao"] = musicaCabecaAgregado.DataInscricao.Valor;
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

                DataTable tabela = new DataTable("RelatorioMusicaCabecaAgregado");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("MusicaCabecaInscritoID", typeof(int));
                    tabela.Columns.Add("AgregadoID", typeof(int));
                    tabela.Columns.Add("Presente", typeof(bool));
                    tabela.Columns.Add("DataInscricao", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["MusicaCabecaInscritoID"] = musicaCabecaAgregado.MusicaCabecaInscritoID.Valor;
                        linha["AgregadoID"] = musicaCabecaAgregado.AgregadoID.Valor;
                        linha["Presente"] = musicaCabecaAgregado.Presente.Valor;
                        linha["DataInscricao"] = musicaCabecaAgregado.DataInscricao.Valor;
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
                    case "MusicaCabecaInscritoID":
                        sql = "SELECT ID, MusicaCabecaInscritoID FROM tMusicaCabecaAgregado WHERE " + FiltroSQL + " ORDER BY MusicaCabecaInscritoID";
                        break;
                    case "AgregadoID":
                        sql = "SELECT ID, AgregadoID FROM tMusicaCabecaAgregado WHERE " + FiltroSQL + " ORDER BY AgregadoID";
                        break;
                    case "Presente":
                        sql = "SELECT ID, Presente FROM tMusicaCabecaAgregado WHERE " + FiltroSQL + " ORDER BY Presente";
                        break;
                    case "DataInscricao":
                        sql = "SELECT ID, DataInscricao FROM tMusicaCabecaAgregado WHERE " + FiltroSQL + " ORDER BY DataInscricao";
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

    #region "MusicaCabecaAgregadoException"

    [Serializable]
    public class MusicaCabecaAgregadoException : Exception
    {

        public MusicaCabecaAgregadoException() : base() { }

        public MusicaCabecaAgregadoException(string msg) : base(msg) { }

        public MusicaCabecaAgregadoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}