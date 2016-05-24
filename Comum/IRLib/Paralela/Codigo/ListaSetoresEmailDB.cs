/******************************************************
* Arquivo ListaSetoresEmailDB.cs
* Gerado em: 07/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "ListaSetoresEmail_B"

    public abstract class ListaSetoresEmail_B : BaseBD
    {

        public setor Setor = new setor();
        public email Email = new email();
        public responsavel Responsavel = new responsavel();

        public ListaSetoresEmail_B() { }

        /// <summary>
        /// Preenche todos os atributos de ListaSetoresEmail
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tListaSetoresEmail WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Setor.ValorBD = bd.LerString("Setor");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Responsavel.ValorBD = bd.LerString("Responsavel");
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
        /// Inserir novo(a) ListaSetoresEmail
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tListaSetoresEmail");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tListaSetoresEmail(IDSetor, Email, Responsavel) ");
                sql.Append("VALUES (@ID,'@001',,'@002',,'@003')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Setor.ValorBD);
                sql.Replace("@002", this.Email.ValorBD);
                sql.Replace("@003", this.Responsavel.ValorBD);

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
        /// Inserir novo(a) ListaSetoresEmail
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MAX(ID) AS ID FROM tListaSetoresEmail");
            object obj = bd.ConsultaValor(sql);
            int id = (obj != null) ? Convert.ToInt32(obj) : 0;

            this.Control.ID = ++id;

            sql = new StringBuilder();
            sql.Append("INSERT INTO tListaSetoresEmail(IDSetor, Email, Responsavel) ");
            sql.Append("VALUES (@ID,'@001',,'@002',,'@003')");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.Setor.ValorBD);
            sql.Replace("@002", this.Email.ValorBD);
            sql.Replace("@003", this.Responsavel.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;

        }


        /// <summary>
        /// Atualiza ListaSetoresEmail
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tListaSetoresEmail SET Setor = '@001', Email = '@002', Responsavel = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Setor.ValorBD);
                sql.Replace("@002", this.Email.ValorBD);
                sql.Replace("@003", this.Responsavel.ValorBD);

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
        /// Atualiza ListaSetoresEmail
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tListaSetoresEmail SET Setor = '@001', Email = '@002', Responsavel = '@003' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.Setor.ValorBD);
            sql.Replace("@002", this.Email.ValorBD);
            sql.Replace("@003", this.Responsavel.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui ListaSetoresEmail com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tListaSetoresEmail WHERE ID=" + id;

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
        /// Exclui ListaSetoresEmail com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tListaSetoresEmail WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui ListaSetoresEmail
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

            this.Setor.Limpar();
            this.Email.Limpar();
            this.Responsavel.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Setor.Desfazer();
            this.Email.Desfazer();
            this.Responsavel.Desfazer();
        }

        public class setor : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Setor";
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

        public class email : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Email";
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

        public class responsavel : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Responsavel";
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

                DataTable tabela = new DataTable("ListaSetoresEmail");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Setor", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Responsavel", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ListaSetoresEmailLista_B"

    public abstract class ListaSetoresEmailLista_B : BaseLista
    {

        protected ListaSetoresEmail listaSetoresEmail;

        // passar o Usuario logado no sistema
        public ListaSetoresEmailLista_B()
        {
            listaSetoresEmail = new ListaSetoresEmail();
        }

        public ListaSetoresEmail ListaSetoresEmail
        {
            get { return listaSetoresEmail; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ListaSetoresEmail especifico
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
                    listaSetoresEmail.Ler(id);
                    return listaSetoresEmail;
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
                    sql = "SELECT ID FROM tListaSetoresEmail";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tListaSetoresEmail";

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
                    sql = "SELECT ID FROM tListaSetoresEmail";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tListaSetoresEmail";

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
        /// Preenche ListaSetoresEmail corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                listaSetoresEmail.Ler(id);

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

                bool ok = listaSetoresEmail.Excluir();
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

                        string sqlDelete = "DELETE FROM tListaSetoresEmail WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) ListaSetoresEmail na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = listaSetoresEmail.Inserir();
                if (ok)
                {
                    lista.Add(listaSetoresEmail.Control.ID);
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
        /// Obtem uma tabela de todos os campos de ListaSetoresEmail carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ListaSetoresEmail");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Setor", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Responsavel", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = listaSetoresEmail.Control.ID;
                        linha["Setor"] = listaSetoresEmail.Setor.Valor;
                        linha["Email"] = listaSetoresEmail.Email.Valor;
                        linha["Responsavel"] = listaSetoresEmail.Responsavel.Valor;
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

                DataTable tabela = new DataTable("RelatorioListaSetoresEmail");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Setor", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("Responsavel", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Setor"] = listaSetoresEmail.Setor.Valor;
                        linha["Email"] = listaSetoresEmail.Email.Valor;
                        linha["Responsavel"] = listaSetoresEmail.Responsavel.Valor;
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
                    case "Setor":
                        sql = "SELECT ID, Setor FROM tListaSetoresEmail WHERE " + FiltroSQL + " ORDER BY Setor";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tListaSetoresEmail WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "Responsavel":
                        sql = "SELECT ID, Responsavel FROM tListaSetoresEmail WHERE " + FiltroSQL + " ORDER BY Responsavel";
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

    #region "ListaSetoresEmailException"

    [Serializable]
    public class ListaSetoresEmailException : Exception
    {

        public ListaSetoresEmailException() : base() { }

        public ListaSetoresEmailException(string msg) : base(msg) { }

        public ListaSetoresEmailException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}