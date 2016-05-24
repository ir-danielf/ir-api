/******************************************************
* Arquivo ContatoSiteDB.cs
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

    #region "ContatoSite_B"

    public abstract class ContatoSite_B : BaseBD
    {

        public nome Nome = new nome();
        public email Email = new email();
        public ddd DDD = new ddd();
        public telefone Telefone = new telefone();
        public mensagem Mensagem = new mensagem();

        public ContatoSite_B() { }

        /// <summary>
        /// Preenche todos os atributos de ContatoSite
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tContatoSite WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.DDD.ValorBD = bd.LerString("DDD");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Mensagem.ValorBD = bd.LerString("Mensagem");
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
        /// Inserir novo(a) ContatoSite
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tContatoSite");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContatoSite(ID, Nome, Email, DDD, Telefone, Mensagem) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Email.ValorBD);
                sql.Replace("@003", this.DDD.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.Mensagem.ValorBD);

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
        /// Inserir novo(a) ContatoSite
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MAX(ID) AS ID FROM tContatoSite");
            object obj = bd.ConsultaValor(sql);
            int id = (obj != null) ? Convert.ToInt32(obj) : 0;

            this.Control.ID = ++id;

            sql = new StringBuilder();
            sql.Append("INSERT INTO tContatoSite(ID, Nome, Email, DDD, Telefone, Mensagem) ");
            sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005')");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.Email.ValorBD);
            sql.Replace("@003", this.DDD.ValorBD);
            sql.Replace("@004", this.Telefone.ValorBD);
            sql.Replace("@005", this.Mensagem.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;

        }


        /// <summary>
        /// Atualiza ContatoSite
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContatoSite SET Nome = '@001', Email = '@002', DDD = '@003', Telefone = '@004', Mensagem = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Email.ValorBD);
                sql.Replace("@003", this.DDD.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.Mensagem.ValorBD);

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
        /// Atualiza ContatoSite
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tContatoSite SET Nome = '@001', Email = '@002', DDD = '@003', Telefone = '@004', Mensagem = '@005' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.Email.ValorBD);
            sql.Replace("@003", this.DDD.ValorBD);
            sql.Replace("@004", this.Telefone.ValorBD);
            sql.Replace("@005", this.Mensagem.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui ContatoSite com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tContatoSite WHERE ID=" + id;

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
        /// Exclui ContatoSite com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tContatoSite WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui ContatoSite
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

            this.Nome.Limpar();
            this.Email.Limpar();
            this.DDD.Limpar();
            this.Telefone.Limpar();
            this.Mensagem.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Nome.Desfazer();
            this.Email.Desfazer();
            this.DDD.Desfazer();
            this.Telefone.Desfazer();
            this.Mensagem.Desfazer();
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

        public class ddd : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDD";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
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

        public class telefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Telefone";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
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

        public class mensagem : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Mensagem";
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

                DataTable tabela = new DataTable("ContatoSite");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Mensagem", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ContatoSiteLista_B"

    public abstract class ContatoSiteLista_B : BaseLista
    {

        protected ContatoSite contatoSite;

        // passar o Usuario logado no sistema
        public ContatoSiteLista_B()
        {
            contatoSite = new ContatoSite();
        }

        public ContatoSite ContatoSite
        {
            get { return contatoSite; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ContatoSite especifico
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
                    contatoSite.Ler(id);
                    return contatoSite;
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
                    sql = "SELECT ID FROM tContatoSite";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContatoSite";

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
                    sql = "SELECT ID FROM tContatoSite";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContatoSite";

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
        /// Preenche ContatoSite corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                contatoSite.Ler(id);

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

                bool ok = contatoSite.Excluir();
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

                        string sqlDelete = "DELETE FROM tContatoSite WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) ContatoSite na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = contatoSite.Inserir();
                if (ok)
                {
                    lista.Add(contatoSite.Control.ID);
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
        /// Obtem uma tabela de todos os campos de ContatoSite carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ContatoSite");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Mensagem", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = contatoSite.Control.ID;
                        linha["Nome"] = contatoSite.Nome.Valor;
                        linha["Email"] = contatoSite.Email.Valor;
                        linha["DDD"] = contatoSite.DDD.Valor;
                        linha["Telefone"] = contatoSite.Telefone.Valor;
                        linha["Mensagem"] = contatoSite.Mensagem.Valor;
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

                DataTable tabela = new DataTable("RelatorioContatoSite");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("DDD", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("Mensagem", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = contatoSite.Nome.Valor;
                        linha["Email"] = contatoSite.Email.Valor;
                        linha["DDD"] = contatoSite.DDD.Valor;
                        linha["Telefone"] = contatoSite.Telefone.Valor;
                        linha["Mensagem"] = contatoSite.Mensagem.Valor;
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
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tContatoSite WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tContatoSite WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "DDD":
                        sql = "SELECT ID, DDD FROM tContatoSite WHERE " + FiltroSQL + " ORDER BY DDD";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tContatoSite WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "Mensagem":
                        sql = "SELECT ID, Mensagem FROM tContatoSite WHERE " + FiltroSQL + " ORDER BY Mensagem";
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

    #region "ContatoSiteException"

    [Serializable]
    public class ContatoSiteException : Exception
    {

        public ContatoSiteException() : base() { }

        public ContatoSiteException(string msg) : base(msg) { }

        public ContatoSiteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}