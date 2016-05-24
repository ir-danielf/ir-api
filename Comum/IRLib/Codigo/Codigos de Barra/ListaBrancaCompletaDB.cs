/******************************************************
* Arquivo ListaBrancaCompletaDB.cs
* Gerado em: 03/08/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "ListaBrancaCompleta_B"

    public abstract class ListaBrancaCompleta_B : BaseBD
    {

        public codigobarra CodigoBarra = new codigobarra();
        public utilizado Utilizado = new utilizado();
        public datautilizado DataUtilizado = new datautilizado();

        public ListaBrancaCompleta_B() { }

        /// <summary>
        /// Preenche todos os atributos de ListaBrancaCompleta
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tListaBrancaCompleta WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.Utilizado.ValorBD = bd.LerString("Utilizado");
                    this.DataUtilizado.ValorBD = bd.LerString("DataUtilizado");
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
        /// Inserir novo(a) ListaBrancaCompleta
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tListaBrancaCompleta");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tListaBrancaCompleta(ID, CodigoBarra, Utilizado, DataUtilizado) ");
                sql.Append("VALUES (@ID,'@001','@002','@003')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CodigoBarra.ValorBD);
                sql.Replace("@002", this.Utilizado.ValorBD);
                sql.Replace("@003", this.DataUtilizado.ValorBD);

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
        /// Atualiza ListaBrancaCompleta
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tListaBrancaCompleta SET CodigoBarra = '@001', Utilizado = '@002', DataUtilizado = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CodigoBarra.ValorBD);
                sql.Replace("@002", this.Utilizado.ValorBD);
                sql.Replace("@003", this.DataUtilizado.ValorBD);

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
        /// Exclui ListaBrancaCompleta com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tListaBrancaCompleta WHERE ID=" + id;

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
        /// Exclui ListaBrancaCompleta
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

            this.CodigoBarra.Limpar();
            this.Utilizado.Limpar();
            this.DataUtilizado.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.CodigoBarra.Desfazer();
            this.Utilizado.Desfazer();
            this.DataUtilizado.Desfazer();
        }

        public class codigobarra : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoBarra";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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

        public class utilizado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Utilizado";
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

        public class datautilizado : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataUtilizado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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

                DataTable tabela = new DataTable("ListaBrancaCompleta");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("Utilizado", typeof(bool));
                tabela.Columns.Add("DataUtilizado", typeof(DateTime));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ListaBrancaCompletaLista_B"

    public abstract class ListaBrancaCompletaLista_B : BaseLista
    {

        protected ListaBrancaCompleta listaBrancaCompleta;

        // passar o Usuario logado no sistema
        public ListaBrancaCompletaLista_B()
        {
            listaBrancaCompleta = new ListaBrancaCompleta();
        }

        public ListaBrancaCompleta ListaBrancaCompleta
        {
            get { return listaBrancaCompleta; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ListaBrancaCompleta especifico
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
                    listaBrancaCompleta.Ler(id);
                    return listaBrancaCompleta;
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
                    sql = "SELECT ID FROM tListaBrancaCompleta";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tListaBrancaCompleta";

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
                    sql = "SELECT ID FROM tListaBrancaCompleta";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tListaBrancaCompleta";

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
        /// Preenche ListaBrancaCompleta corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                listaBrancaCompleta.Ler(id);

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

                bool ok = listaBrancaCompleta.Excluir();
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

                        string sqlDelete = "DELETE FROM tListaBrancaCompleta WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) ListaBrancaCompleta na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = listaBrancaCompleta.Inserir();
                if (ok)
                {
                    lista.Add(listaBrancaCompleta.Control.ID);
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
        /// Obtem uma tabela de todos os campos de ListaBrancaCompleta carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ListaBrancaCompleta");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("Utilizado", typeof(bool));
                tabela.Columns.Add("DataUtilizado", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = listaBrancaCompleta.Control.ID;
                        linha["CodigoBarra"] = listaBrancaCompleta.CodigoBarra.Valor;
                        linha["Utilizado"] = listaBrancaCompleta.Utilizado.Valor;
                        linha["DataUtilizado"] = listaBrancaCompleta.DataUtilizado.Valor;
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

                DataTable tabela = new DataTable("RelatorioListaBrancaCompleta");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("Utilizado", typeof(bool));
                    tabela.Columns.Add("DataUtilizado", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CodigoBarra"] = listaBrancaCompleta.CodigoBarra.Valor;
                        linha["Utilizado"] = listaBrancaCompleta.Utilizado.Valor;
                        linha["DataUtilizado"] = listaBrancaCompleta.DataUtilizado.Valor;
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
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tListaBrancaCompleta WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "Utilizado":
                        sql = "SELECT ID, Utilizado FROM tListaBrancaCompleta WHERE " + FiltroSQL + " ORDER BY Utilizado";
                        break;
                    case "DataUtilizado":
                        sql = "SELECT ID, DataUtilizado FROM tListaBrancaCompleta WHERE " + FiltroSQL + " ORDER BY DataUtilizado";
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

    #region "ListaBrancaCompletaException"

    [Serializable]
    public class ListaBrancaCompletaException : Exception
    {

        public ListaBrancaCompletaException() : base() { }

        public ListaBrancaCompletaException(string msg) : base(msg) { }

        public ListaBrancaCompletaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}