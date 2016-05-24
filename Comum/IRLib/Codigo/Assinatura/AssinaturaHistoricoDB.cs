/******************************************************
* Arquivo AssinaturaHistoricoDB.cs
* Gerado em: 17/11/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaHistorico_B"

    public abstract class AssinaturaHistorico_B : BaseBD
    {

        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public acao Acao = new acao();
        public status Status = new status();
        public timestamp TimeStamp = new timestamp();
        public usuarioid UsuarioID = new usuarioid();

        public AssinaturaHistorico_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaHistorico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaHistorico WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
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
        /// Inserir novo(a) AssinaturaHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaHistorico(AssinaturaClienteID, VendaBilheteriaID, Acao, Status, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005',@006); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.Status.ValorBD);
                sql.Replace("@005", this.TimeStamp.ValorBD);
                sql.Replace("@006", this.UsuarioID.ValorBD);

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
        /// Inserir novo(a) AssinaturaHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaHistorico(AssinaturaClienteID, VendaBilheteriaID, Acao, Status, TimeStamp, UsuarioID) ");
            sql.Append("VALUES (@001,@002,'@003','@004','@005',@006); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@003", this.Acao.ValorBD);
            sql.Replace("@004", this.Status.ValorBD);
            sql.Replace("@005", this.TimeStamp.ValorBD);
            sql.Replace("@006", this.UsuarioID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaHistorico SET AssinaturaClienteID = @001, VendaBilheteriaID = @002, Acao = '@003', Status = '@004', TimeStamp = '@005', UsuarioID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.Status.ValorBD);
                sql.Replace("@005", this.TimeStamp.ValorBD);
                sql.Replace("@006", this.UsuarioID.ValorBD);

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
        /// Atualiza AssinaturaHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaHistorico SET AssinaturaClienteID = @001, VendaBilheteriaID = @002, Acao = '@003', Status = '@004', TimeStamp = '@005', UsuarioID = @006 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaID.ValorBD);
            sql.Replace("@003", this.Acao.ValorBD);
            sql.Replace("@004", this.Status.ValorBD);
            sql.Replace("@005", this.TimeStamp.ValorBD);
            sql.Replace("@006", this.UsuarioID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaHistorico com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaHistorico WHERE ID=" + id;

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
        /// Exclui AssinaturaHistorico com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaHistorico WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaHistorico
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
            this.VendaBilheteriaID.Limpar();
            this.Acao.Limpar();
            this.Status.Limpar();
            this.TimeStamp.Limpar();
            this.UsuarioID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.AssinaturaClienteID.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.Acao.Desfazer();
            this.Status.Desfazer();
            this.TimeStamp.Desfazer();
            this.UsuarioID.Desfazer();
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

        public class vendabilheteriaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaID";
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

        public class status : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
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

                DataTable tabela = new DataTable("AssinaturaHistorico");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
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

    #region "AssinaturaHistoricoLista_B"

    public abstract class AssinaturaHistoricoLista_B : BaseLista
    {

        protected AssinaturaHistorico assinaturaHistorico;

        // passar o Usuario logado no sistema
        public AssinaturaHistoricoLista_B()
        {
            assinaturaHistorico = new AssinaturaHistorico();
        }

        public AssinaturaHistorico AssinaturaHistorico
        {
            get { return assinaturaHistorico; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaHistorico especifico
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
                    assinaturaHistorico.Ler(id);
                    return assinaturaHistorico;
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
                    sql = "SELECT ID FROM tAssinaturaHistorico";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaHistorico";

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
                    sql = "SELECT ID FROM tAssinaturaHistorico";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaHistorico";

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
        /// Preenche AssinaturaHistorico corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaHistorico.Ler(id);

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

                bool ok = assinaturaHistorico.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaHistorico WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaHistorico na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaHistorico.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaHistorico.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaHistorico carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaHistorico");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("UsuarioID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaHistorico.Control.ID;
                        linha["AssinaturaClienteID"] = assinaturaHistorico.AssinaturaClienteID.Valor;
                        linha["VendaBilheteriaID"] = assinaturaHistorico.VendaBilheteriaID.Valor;
                        linha["Acao"] = assinaturaHistorico.Acao.Valor;
                        linha["Status"] = assinaturaHistorico.Status.Valor;
                        linha["TimeStamp"] = assinaturaHistorico.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaHistorico.UsuarioID.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaHistorico");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("UsuarioID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["AssinaturaClienteID"] = assinaturaHistorico.AssinaturaClienteID.Valor;
                        linha["VendaBilheteriaID"] = assinaturaHistorico.VendaBilheteriaID.Valor;
                        linha["Acao"] = assinaturaHistorico.Acao.Valor;
                        linha["Status"] = assinaturaHistorico.Status.Valor;
                        linha["TimeStamp"] = assinaturaHistorico.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaHistorico.UsuarioID.Valor;
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
                        sql = "SELECT ID, AssinaturaClienteID FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tAssinaturaHistorico WHERE " + FiltroSQL + " ORDER BY UsuarioID";
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

    #region "AssinaturaHistoricoException"

    [Serializable]
    public class AssinaturaHistoricoException : Exception
    {

        public AssinaturaHistoricoException() : base() { }

        public AssinaturaHistoricoException(string msg) : base(msg) { }

        public AssinaturaHistoricoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}