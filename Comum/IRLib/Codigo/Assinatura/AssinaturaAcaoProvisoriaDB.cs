/******************************************************
* Arquivo AssinaturaAcaoProvisoriaDB.cs
* Gerado em: 10/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaAcaoProvisoria_B"

    public abstract class AssinaturaAcaoProvisoria_B : BaseBD
    {

        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();
        public clienteid ClienteID = new clienteid();
        public precotipoid PrecoTipoID = new precotipoid();
        public acao Acao = new acao();
        public entregaid EntregaID = new entregaid();
        public processado Processado = new processado();
        public agregadoid AgregadoID = new agregadoid();

        public AssinaturaAcaoProvisoria_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaAcaoProvisoria
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaAcaoProvisoria WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.EntregaID.ValorBD = bd.LerInt("EntregaID").ToString();
                    this.Processado.ValorBD = bd.LerString("Processado");
                    this.AgregadoID.ValorBD = bd.LerInt("AgregadoID").ToString();
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
        /// Inserir novo(a) AssinaturaAcaoProvisoria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaAcaoProvisoria(AssinaturaClienteID, ClienteID, PrecoTipoID, Acao, EntregaID, Processado, AgregadoID) ");
                sql.Append("VALUES (@001,@002,@003,'@004',@005,'@006', '@007'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.EntregaID.ValorBD);
                sql.Replace("@006", this.Processado.ValorBD);
                sql.Replace("@007", this.AgregadoID.ValorBD);

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
        /// Inserir novo(a) AssinaturaAcaoProvisoria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaAcaoProvisoria(AssinaturaClienteID, ClienteID, PrecoTipoID, Acao, EntregaID, Processado, AgregadoID) ");
            sql.Append("VALUES (@001,@002,@003,'@004',@005,'@006', '@007'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@002", this.ClienteID.ValorBD);
            sql.Replace("@003", this.PrecoTipoID.ValorBD);
            sql.Replace("@004", this.Acao.ValorBD);
            sql.Replace("@005", this.EntregaID.ValorBD);
            sql.Replace("@006", this.Processado.ValorBD);
            sql.Replace("@007", this.AgregadoID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaAcaoProvisoria
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaAcaoProvisoria SET AssinaturaClienteID = @001, ClienteID = @002, PrecoTipoID = @003, Acao = '@004', EntregaID = @005, Processado = '@006' , AgregadoID='@007'");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@002", this.ClienteID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.EntregaID.ValorBD);
                sql.Replace("@006", this.Processado.ValorBD);
                sql.Replace("@007", this.AgregadoID.ValorBD);

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
        /// Atualiza AssinaturaAcaoProvisoria
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaAcaoProvisoria SET AssinaturaClienteID = @001, ClienteID = @002, PrecoTipoID = @003, Acao = '@004', EntregaID = @005, Processado = '@006', AgregadoID='@007' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@002", this.ClienteID.ValorBD);
            sql.Replace("@003", this.PrecoTipoID.ValorBD);
            sql.Replace("@004", this.Acao.ValorBD);
            sql.Replace("@005", this.EntregaID.ValorBD);
            sql.Replace("@006", this.Processado.ValorBD);
            sql.Replace("@007", this.AgregadoID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaAcaoProvisoria com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaAcaoProvisoria WHERE ID=" + id;

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
        /// Exclui AssinaturaAcaoProvisoria com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaAcaoProvisoria WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaAcaoProvisoria
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

            this.AssinaturaClienteID.Limpar();
            this.ClienteID.Limpar();
            this.PrecoTipoID.Limpar();
            this.Acao.Limpar();
            this.EntregaID.Limpar();
            this.Processado.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
            this.AgregadoID.Limpar();
        }

        public override void Desfazer()
        {

            this.AssinaturaClienteID.Desfazer();
            this.ClienteID.Desfazer();
            this.PrecoTipoID.Desfazer();
            this.Acao.Desfazer();
            this.EntregaID.Desfazer();
            this.Processado.Desfazer();
            this.AgregadoID.Limpar();
        }

        public class assinaturaclienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaClienteID";
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

        public class precotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoTipoID";
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

        public class entregaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaID";
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

        public class agregadoid:IntegerProperty
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

        public class processado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Processado";
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

                DataTable tabela = new DataTable("AssinaturaAcaoProvisoria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("EntregaID", typeof(int));
                tabela.Columns.Add("Processado", typeof(bool));
                tabela.Columns.Add("AgregadoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaAcaoProvisoriaLista_B"

    public abstract class AssinaturaAcaoProvisoriaLista_B : BaseLista
    {

        protected AssinaturaAcaoProvisoria assinaturaAcaoProvisoria;

        // passar o Usuario logado no sistema
        public AssinaturaAcaoProvisoriaLista_B()
        {
            assinaturaAcaoProvisoria = new AssinaturaAcaoProvisoria();
        }

        public AssinaturaAcaoProvisoria AssinaturaAcaoProvisoria
        {
            get { return assinaturaAcaoProvisoria; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaAcaoProvisoria especifico
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
                    assinaturaAcaoProvisoria.Ler(id);
                    return assinaturaAcaoProvisoria;
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
                    sql = "SELECT ID FROM tAssinaturaAcaoProvisoria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaAcaoProvisoria";

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
                    sql = "SELECT ID FROM tAssinaturaAcaoProvisoria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaAcaoProvisoria";

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
        /// Preenche AssinaturaAcaoProvisoria corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaAcaoProvisoria.Ler(id);

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

                bool ok = assinaturaAcaoProvisoria.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaAcaoProvisoria WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaAcaoProvisoria na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaAcaoProvisoria.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaAcaoProvisoria.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaAcaoProvisoria carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaAcaoProvisoria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("EntregaID", typeof(int));
                tabela.Columns.Add("Processado", typeof(bool));
                tabela.Columns.Add("AgregadoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaAcaoProvisoria.Control.ID;
                        linha["AssinaturaClienteID"] = assinaturaAcaoProvisoria.AssinaturaClienteID.Valor;
                        linha["ClienteID"] = assinaturaAcaoProvisoria.ClienteID.Valor;
                        linha["PrecoTipoID"] = assinaturaAcaoProvisoria.PrecoTipoID.Valor;
                        linha["Acao"] = assinaturaAcaoProvisoria.Acao.Valor;
                        linha["EntregaID"] = assinaturaAcaoProvisoria.EntregaID.Valor;
                        linha["Processado"] = assinaturaAcaoProvisoria.Processado.Valor;
                        linha["AgregadoID"] = assinaturaAcaoProvisoria.AgregadoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaAcaoProvisoria");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("PrecoTipoID", typeof(int));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("EntregaID", typeof(int));
                    tabela.Columns.Add("Processado", typeof(bool));
                    tabela.Columns.Add("AgregadoID", typeof(int));
                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["AssinaturaClienteID"] = assinaturaAcaoProvisoria.AssinaturaClienteID.Valor;
                        linha["ClienteID"] = assinaturaAcaoProvisoria.ClienteID.Valor;
                        linha["PrecoTipoID"] = assinaturaAcaoProvisoria.PrecoTipoID.Valor;
                        linha["Acao"] = assinaturaAcaoProvisoria.Acao.Valor;
                        linha["EntregaID"] = assinaturaAcaoProvisoria.EntregaID.Valor;
                        linha["Processado"] = assinaturaAcaoProvisoria.Processado.Valor;
                        linha["AgregadoID"] = assinaturaAcaoProvisoria.AgregadoID.Valor;
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
                    case "AssinaturaClienteID":
                        sql = "SELECT ID, AssinaturaClienteID FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "PrecoTipoID":
                        sql = "SELECT ID, PrecoTipoID FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY PrecoTipoID";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "EntregaID":
                        sql = "SELECT ID, EntregaID FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY EntregaID";
                        break;
                    case "Processado":
                        sql = "SELECT ID, Processado FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY Processado";
                        break;
                    case "AgregadoID":
                        sql = "SELECT ID, AgregadoID FROM tAssinaturaAcaoProvisoria WHERE " + FiltroSQL + " ORDER BY Processado";
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

    #region "AssinaturaAcaoProvisoriaException"

    [Serializable]
    public class AssinaturaAcaoProvisoriaException : Exception
    {

        public AssinaturaAcaoProvisoriaException() : base() { }

        public AssinaturaAcaoProvisoriaException(string msg) : base(msg) { }

        public AssinaturaAcaoProvisoriaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}