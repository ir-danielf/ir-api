/******************************************************
* Arquivo AssinaturaBancoIngressoComprovanteDB.cs
* Gerado em: 06/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaBancoIngressoComprovante_B"

    public abstract class AssinaturaBancoIngressoComprovante_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public timestamp Timestamp = new timestamp();
        public acao Acao = new acao();
        public usuarioid UsuarioID = new usuarioid();

        public AssinaturaBancoIngressoComprovante_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaBancoIngressoComprovante
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaBancoIngressoComprovante WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.Timestamp.ValorBD = bd.LerString("Timestamp");
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
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
        /// Inserir novo(a) AssinaturaBancoIngressoComprovante
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaBancoIngressoComprovante(ClienteID, Timestamp, Acao, UsuarioID) ");
                sql.Append("VALUES (@001,'@002','@003',@004); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.Timestamp.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);

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
        /// Inserir novo(a) AssinaturaBancoIngressoComprovante
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaBancoIngressoComprovante(ClienteID, Timestamp, Acao, UsuarioID) ");
            sql.Append("VALUES (@001,'@002','@003',@004); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.Timestamp.ValorBD);
            sql.Replace("@003", this.Acao.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaBancoIngressoComprovante
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaBancoIngressoComprovante SET ClienteID = @001, Timestamp = '@002', Acao = '@003', UsuarioID = @004 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.Timestamp.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);

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
        /// Atualiza AssinaturaBancoIngressoComprovante
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaBancoIngressoComprovante SET ClienteID = @001, Timestamp = '@002', Acao = '@003', UsuarioID = @004 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.Timestamp.ValorBD);
            sql.Replace("@003", this.Acao.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaBancoIngressoComprovante com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoComprovante WHERE ID=" + id;

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
        /// Exclui AssinaturaBancoIngressoComprovante com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoComprovante WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaBancoIngressoComprovante
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
            this.Timestamp.Limpar();
            this.Acao.Limpar();
            this.UsuarioID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.Timestamp.Desfazer();
            this.Acao.Desfazer();
            this.UsuarioID.Desfazer();
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

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "Timestamp";
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

        public class acao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
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

                DataTable tabela = new DataTable("AssinaturaBancoIngressoComprovante");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("Timestamp", typeof(DateTime));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("UsuarioID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaBancoIngressoComprovanteLista_B"

    public abstract class AssinaturaBancoIngressoComprovanteLista_B : BaseLista
    {

        protected AssinaturaBancoIngressoComprovante assinaturaBancoIngressoComprovante;

        // passar o Usuario logado no sistema
        public AssinaturaBancoIngressoComprovanteLista_B()
        {
            assinaturaBancoIngressoComprovante = new AssinaturaBancoIngressoComprovante();
        }

        public AssinaturaBancoIngressoComprovante AssinaturaBancoIngressoComprovante
        {
            get { return assinaturaBancoIngressoComprovante; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaBancoIngressoComprovante especifico
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
                    assinaturaBancoIngressoComprovante.Ler(id);
                    return assinaturaBancoIngressoComprovante;
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
                    sql = "SELECT ID FROM tAssinaturaBancoIngressoComprovante";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngressoComprovante";

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
                    sql = "SELECT ID FROM tAssinaturaBancoIngressoComprovante";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngressoComprovante";

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
        /// Preenche AssinaturaBancoIngressoComprovante corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaBancoIngressoComprovante.Ler(id);

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

                bool ok = assinaturaBancoIngressoComprovante.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoComprovante WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaBancoIngressoComprovante na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaBancoIngressoComprovante.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaBancoIngressoComprovante.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaBancoIngressoComprovante carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaBancoIngressoComprovante");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("Timestamp", typeof(DateTime));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("UsuarioID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaBancoIngressoComprovante.Control.ID;
                        linha["ClienteID"] = assinaturaBancoIngressoComprovante.ClienteID.Valor;
                        linha["Timestamp"] = assinaturaBancoIngressoComprovante.Timestamp.Valor;
                        linha["Acao"] = assinaturaBancoIngressoComprovante.Acao.Valor;
                        linha["UsuarioID"] = assinaturaBancoIngressoComprovante.UsuarioID.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaBancoIngressoComprovante");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("Timestamp", typeof(DateTime));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("UsuarioID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = assinaturaBancoIngressoComprovante.ClienteID.Valor;
                        linha["Timestamp"] = assinaturaBancoIngressoComprovante.Timestamp.Valor;
                        linha["Acao"] = assinaturaBancoIngressoComprovante.Acao.Valor;
                        linha["UsuarioID"] = assinaturaBancoIngressoComprovante.UsuarioID.Valor;
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
                        sql = "SELECT ID, ClienteID FROM tAssinaturaBancoIngressoComprovante WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "Timestamp":
                        sql = "SELECT ID, Timestamp FROM tAssinaturaBancoIngressoComprovante WHERE " + FiltroSQL + " ORDER BY Timestamp";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tAssinaturaBancoIngressoComprovante WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tAssinaturaBancoIngressoComprovante WHERE " + FiltroSQL + " ORDER BY UsuarioID";
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

    #region "AssinaturaBancoIngressoComprovanteException"

    [Serializable]
    public class AssinaturaBancoIngressoComprovanteException : Exception
    {

        public AssinaturaBancoIngressoComprovanteException() : base() { }

        public AssinaturaBancoIngressoComprovanteException(string msg) : base(msg) { }

        public AssinaturaBancoIngressoComprovanteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}