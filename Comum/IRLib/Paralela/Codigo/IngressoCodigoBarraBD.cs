/******************************************************
* Arquivo IngressoCodigoBarraDB.cs
* Gerado em: 07/12/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "IngressoCodigoBarra_B"

    public abstract class IngressoCodigoBarra_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public codigobarra CodigoBarra = new codigobarra();
        public blacklist BlackList = new blacklist();
        public timestamp TimeStamp = new timestamp();

        public IngressoCodigoBarra_B() { }

        /// <summary>
        /// Preenche todos os atributos de IngressoCodigoBarra
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tIngressoCodigoBarra WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.BlackList.ValorBD = bd.LerString("BlackList");
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
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
        /// Inserir novo(a) IngressoCodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tIngressoCodigoBarra(EventoID, CodigoBarra, BlackList, TimeStamp) ");
                sql.Append("VALUES (@001,'@002','@003','@004'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.CodigoBarra.ValorBD);
                sql.Replace("@003", this.BlackList.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);

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
        /// Inserir novo(a) IngressoCodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tIngressoCodigoBarra(EventoID, CodigoBarra, BlackList, TimeStamp) ");
            sql.Append("VALUES (@001,'@002','@003','@004'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.EventoID.ValorBD);
            sql.Replace("@002", this.CodigoBarra.ValorBD);
            sql.Replace("@003", this.BlackList.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza IngressoCodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tIngressoCodigoBarra SET EventoID = @001, CodigoBarra = '@002', BlackList = '@003', TimeStamp = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.CodigoBarra.ValorBD);
                sql.Replace("@003", this.BlackList.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);

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
        /// Atualiza IngressoCodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tIngressoCodigoBarra SET EventoID = @001, CodigoBarra = '@002', BlackList = '@003', TimeStamp = '@004' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.EventoID.ValorBD);
            sql.Replace("@002", this.CodigoBarra.ValorBD);
            sql.Replace("@003", this.BlackList.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui IngressoCodigoBarra com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tIngressoCodigoBarra WHERE ID=" + id;

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
        /// Exclui IngressoCodigoBarra com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tIngressoCodigoBarra WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui IngressoCodigoBarra
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
            this.CodigoBarra.Limpar();
            this.BlackList.Limpar();
            this.TimeStamp.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.EventoID.Desfazer();
            this.CodigoBarra.Desfazer();
            this.BlackList.Desfazer();
            this.TimeStamp.Desfazer();
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
                    return 18;
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

        public class blacklist : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "BlackList";
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

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStamp";
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

                DataTable tabela = new DataTable("IngressoCodigoBarra");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("BlackList", typeof(bool));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "IngressoCodigoBarraLista_B"

    public abstract class IngressoCodigoBarraLista_B : BaseLista
    {

        protected IngressoCodigoBarra ingressoCodigoBarra;

        // passar o Usuario logado no sistema
        public IngressoCodigoBarraLista_B()
        {
            ingressoCodigoBarra = new IngressoCodigoBarra();
        }

        public IngressoCodigoBarra IngressoCodigoBarra
        {
            get { return ingressoCodigoBarra; }
        }

        /// <summary>
        /// Retorna um IBaseBD de IngressoCodigoBarra especifico
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
                    ingressoCodigoBarra.Ler(id);
                    return ingressoCodigoBarra;
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
                    sql = "SELECT ID FROM tIngressoCodigoBarra";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoCodigoBarra";

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
                    sql = "SELECT ID FROM tIngressoCodigoBarra";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoCodigoBarra";

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
        /// Preenche IngressoCodigoBarra corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                ingressoCodigoBarra.Ler(id);

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

                bool ok = ingressoCodigoBarra.Excluir();
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

                        string sqlDelete = "DELETE FROM tIngressoCodigoBarra WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) IngressoCodigoBarra na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = ingressoCodigoBarra.Inserir();
                if (ok)
                {
                    lista.Add(ingressoCodigoBarra.Control.ID);
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
        /// Obtem uma tabela de todos os campos de IngressoCodigoBarra carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("IngressoCodigoBarra");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("BlackList", typeof(bool));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = ingressoCodigoBarra.Control.ID;
                        linha["EventoID"] = ingressoCodigoBarra.EventoID.Valor;
                        linha["CodigoBarra"] = ingressoCodigoBarra.CodigoBarra.Valor;
                        linha["BlackList"] = ingressoCodigoBarra.BlackList.Valor;
                        linha["TimeStamp"] = ingressoCodigoBarra.TimeStamp.Valor;
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

                DataTable tabela = new DataTable("RelatorioIngressoCodigoBarra");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("BlackList", typeof(bool));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = ingressoCodigoBarra.EventoID.Valor;
                        linha["CodigoBarra"] = ingressoCodigoBarra.CodigoBarra.Valor;
                        linha["BlackList"] = ingressoCodigoBarra.BlackList.Valor;
                        linha["TimeStamp"] = ingressoCodigoBarra.TimeStamp.Valor;
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
                        sql = "SELECT ID, EventoID FROM tIngressoCodigoBarra WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tIngressoCodigoBarra WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "BlackList":
                        sql = "SELECT ID, BlackList FROM tIngressoCodigoBarra WHERE " + FiltroSQL + " ORDER BY BlackList";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tIngressoCodigoBarra WHERE " + FiltroSQL + " ORDER BY TimeStamp";
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

    #region "IngressoCodigoBarraException"

    [Serializable]
    public class IngressoCodigoBarraException : Exception
    {

        public IngressoCodigoBarraException() : base() { }

        public IngressoCodigoBarraException(string msg) : base(msg) { }

        public IngressoCodigoBarraException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}