/******************************************************
* Arquivo AssinaturaBancoIngressoDB.cs
* Gerado em: 02/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaBancoIngresso_B"

    public abstract class AssinaturaBancoIngresso_B : BaseBD
    {

        public ingressoid IngressoID = new ingressoid();
        public clienteid ClienteID = new clienteid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public ano Ano = new ano();
        public assinaturaid AssinaturaID = new assinaturaid();

        public AssinaturaBancoIngresso_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaBancoIngresso
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaBancoIngresso WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.Ano.ValorBD = bd.LerString("Ano");
                    this.AssinaturaID.ValorBD = bd.LerInt("AssinaturaID").ToString();
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
        /// Inserir novo(a) AssinaturaBancoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaBancoIngresso(IngressoID, ClienteID, ApresentacaoID, Ano, AssinaturaID) ");
                sql.Append("VALUES (@001,@002,@003,'@004',@005); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.Ano.ValorBD);
                sql.Replace("@005", this.AssinaturaID.ValorBD);

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
        /// Inserir novo(a) AssinaturaBancoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaBancoIngresso(IngressoID, ClienteID, ApresentacaoID, Ano, AssinaturaID) ");
            sql.Append("VALUES (@001,@002,@003,'@004',@005); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.IngressoID.ValorBD);
            sql.Replace("@002", this.ClienteID.ValorBD);
            sql.Replace("@003", this.ApresentacaoID.ValorBD);
            sql.Replace("@004", this.Ano.ValorBD);
            sql.Replace("@005", this.AssinaturaID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaBancoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaBancoIngresso SET IngressoID = @001, ClienteID = @002, ApresentacaoID = @003, Ano = '@004', AssinaturaID = @005 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.Ano.ValorBD);
                sql.Replace("@005", this.AssinaturaID.ValorBD);

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
        /// Atualiza AssinaturaBancoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaBancoIngresso SET IngressoID = @001, ClienteID = @002, ApresentacaoID = @003, Ano = '@004', AssinaturaID = @005 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.IngressoID.ValorBD);
            sql.Replace("@002", this.ClienteID.ValorBD);
            sql.Replace("@003", this.ApresentacaoID.ValorBD);
            sql.Replace("@004", this.Ano.ValorBD);
            sql.Replace("@005", this.AssinaturaID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaBancoIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaBancoIngresso WHERE ID=" + id;

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
        /// Exclui AssinaturaBancoIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaBancoIngresso WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaBancoIngresso
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

            this.IngressoID.Limpar();
            this.ClienteID.Limpar();
            this.ApresentacaoID.Limpar();
            this.Ano.Limpar();
            this.AssinaturaID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.IngressoID.Desfazer();
            this.ClienteID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.Ano.Desfazer();
            this.AssinaturaID.Desfazer();
        }

        public class ingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressoID";
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

        public class ano : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ano";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4;
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

        public class assinaturaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaID";
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

                DataTable tabela = new DataTable("AssinaturaBancoIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("Ano", typeof(string));
                tabela.Columns.Add("AssinaturaID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaBancoIngressoLista_B"

    public abstract class AssinaturaBancoIngressoLista_B : BaseLista
    {

        protected AssinaturaBancoIngresso assinaturaBancoIngresso;

        // passar o Usuario logado no sistema
        public AssinaturaBancoIngressoLista_B()
        {
            assinaturaBancoIngresso = new AssinaturaBancoIngresso();
        }

        public AssinaturaBancoIngresso AssinaturaBancoIngresso
        {
            get { return assinaturaBancoIngresso; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaBancoIngresso especifico
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
                    assinaturaBancoIngresso.Ler(id);
                    return assinaturaBancoIngresso;
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
                    sql = "SELECT ID FROM tAssinaturaBancoIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngresso";

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
                    sql = "SELECT ID FROM tAssinaturaBancoIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngresso";

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
        /// Preenche AssinaturaBancoIngresso corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaBancoIngresso.Ler(id);

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

                bool ok = assinaturaBancoIngresso.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaBancoIngresso WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaBancoIngresso na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaBancoIngresso.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaBancoIngresso.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaBancoIngresso carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaBancoIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("Ano", typeof(string));
                tabela.Columns.Add("AssinaturaID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaBancoIngresso.Control.ID;
                        linha["IngressoID"] = assinaturaBancoIngresso.IngressoID.Valor;
                        linha["ClienteID"] = assinaturaBancoIngresso.ClienteID.Valor;
                        linha["ApresentacaoID"] = assinaturaBancoIngresso.ApresentacaoID.Valor;
                        linha["Ano"] = assinaturaBancoIngresso.Ano.Valor;
                        linha["AssinaturaID"] = assinaturaBancoIngresso.AssinaturaID.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaBancoIngresso");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("IngressoID", typeof(int));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("Ano", typeof(string));
                    tabela.Columns.Add("AssinaturaID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["IngressoID"] = assinaturaBancoIngresso.IngressoID.Valor;
                        linha["ClienteID"] = assinaturaBancoIngresso.ClienteID.Valor;
                        linha["ApresentacaoID"] = assinaturaBancoIngresso.ApresentacaoID.Valor;
                        linha["Ano"] = assinaturaBancoIngresso.Ano.Valor;
                        linha["AssinaturaID"] = assinaturaBancoIngresso.AssinaturaID.Valor;
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
                    case "IngressoID":
                        sql = "SELECT ID, IngressoID FROM tAssinaturaBancoIngresso WHERE " + FiltroSQL + " ORDER BY IngressoID";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tAssinaturaBancoIngresso WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tAssinaturaBancoIngresso WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "Ano":
                        sql = "SELECT ID, Ano FROM tAssinaturaBancoIngresso WHERE " + FiltroSQL + " ORDER BY Ano";
                        break;
                    case "AssinaturaID":
                        sql = "SELECT ID, AssinaturaID FROM tAssinaturaBancoIngresso WHERE " + FiltroSQL + " ORDER BY AssinaturaID";
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

    #region "AssinaturaBancoIngressoException"

    [Serializable]
    public class AssinaturaBancoIngressoException : Exception
    {

        public AssinaturaBancoIngressoException() : base() { }

        public AssinaturaBancoIngressoException(string msg) : base(msg) { }

        public AssinaturaBancoIngressoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}