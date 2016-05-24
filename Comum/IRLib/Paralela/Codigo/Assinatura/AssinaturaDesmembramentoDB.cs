/******************************************************
* Arquivo AssinaturaDesmembramentoDB.cs
* Gerado em: 04/01/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "AssinaturaDesmembramento_B"

    public abstract class AssinaturaDesmembramento_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public antigoclienteid AntigoClienteID = new antigoclienteid();
        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();
        public timestamp TimeStamp = new timestamp();
        public usuarioid UsuarioID = new usuarioid();
        public motivo Motivo = new motivo();

        public AssinaturaDesmembramento_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaDesmembramento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaDesmembramento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.AntigoClienteID.ValorBD = bd.LerInt("AntigoClienteID").ToString();
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Motivo.ValorBD = bd.LerString("Motivo");
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
        /// Inserir novo(a) AssinaturaDesmembramento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaDesmembramento(ClienteID, AntigoClienteID, AssinaturaClienteID, TimeStamp, UsuarioID, Motivo) ");
                sql.Append("VALUES (@001,@002,@003,'@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AntigoClienteID.ValorBD);
                sql.Replace("@003", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.UsuarioID.ValorBD);
                sql.Replace("@006", this.Motivo.ValorBD);

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
        /// Inserir novo(a) AssinaturaDesmembramento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaDesmembramento(ClienteID, AntigoClienteID, AssinaturaClienteID, TimeStamp, UsuarioID, Motivo) ");
            sql.Append("VALUES (@001,@002,@003,'@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AntigoClienteID.ValorBD);
            sql.Replace("@003", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.UsuarioID.ValorBD);
            sql.Replace("@006", this.Motivo.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaDesmembramento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaDesmembramento SET ClienteID = @001, AntigoClienteID = @002, AssinaturaClienteID = @003, TimeStamp = '@004', UsuarioID = @005, Motivo = '@006' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AntigoClienteID.ValorBD);
                sql.Replace("@003", this.AssinaturaClienteID.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.UsuarioID.ValorBD);
                sql.Replace("@006", this.Motivo.ValorBD);

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
        /// Atualiza AssinaturaDesmembramento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaDesmembramento SET ClienteID = @001, AntigoClienteID = @002, AssinaturaClienteID = @003, TimeStamp = '@004', UsuarioID = @005, Motivo = '@006' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AntigoClienteID.ValorBD);
            sql.Replace("@003", this.AssinaturaClienteID.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.UsuarioID.ValorBD);
            sql.Replace("@006", this.Motivo.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaDesmembramento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaDesmembramento WHERE ID=" + id;

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
        /// Exclui AssinaturaDesmembramento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaDesmembramento WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaDesmembramento
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
            this.AntigoClienteID.Limpar();
            this.AssinaturaClienteID.Limpar();
            this.TimeStamp.Limpar();
            this.UsuarioID.Limpar();
            this.Motivo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.AntigoClienteID.Desfazer();
            this.AssinaturaClienteID.Desfazer();
            this.TimeStamp.Desfazer();
            this.UsuarioID.Desfazer();
            this.Motivo.Desfazer();
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

        public class antigoclienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AntigoClienteID";
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

        public class motivo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Motivo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1000;
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

                DataTable tabela = new DataTable("AssinaturaDesmembramento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AntigoClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Motivo", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaDesmembramentoLista_B"

    public abstract class AssinaturaDesmembramentoLista_B : BaseLista
    {

        protected AssinaturaDesmembramento assinaturaDesmembramento;

        // passar o Usuario logado no sistema
        public AssinaturaDesmembramentoLista_B()
        {
            assinaturaDesmembramento = new AssinaturaDesmembramento();
        }

        public AssinaturaDesmembramento AssinaturaDesmembramento
        {
            get { return assinaturaDesmembramento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaDesmembramento especifico
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
                    assinaturaDesmembramento.Ler(id);
                    return assinaturaDesmembramento;
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
                    sql = "SELECT ID FROM tAssinaturaDesmembramento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaDesmembramento";

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
                    sql = "SELECT ID FROM tAssinaturaDesmembramento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaDesmembramento";

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
        /// Preenche AssinaturaDesmembramento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaDesmembramento.Ler(id);

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

                bool ok = assinaturaDesmembramento.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaDesmembramento WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaDesmembramento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaDesmembramento.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaDesmembramento.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaDesmembramento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaDesmembramento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AntigoClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Motivo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaDesmembramento.Control.ID;
                        linha["ClienteID"] = assinaturaDesmembramento.ClienteID.Valor;
                        linha["AntigoClienteID"] = assinaturaDesmembramento.AntigoClienteID.Valor;
                        linha["AssinaturaClienteID"] = assinaturaDesmembramento.AssinaturaClienteID.Valor;
                        linha["TimeStamp"] = assinaturaDesmembramento.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaDesmembramento.UsuarioID.Valor;
                        linha["Motivo"] = assinaturaDesmembramento.Motivo.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaDesmembramento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("AntigoClienteID", typeof(int));
                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("Motivo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = assinaturaDesmembramento.ClienteID.Valor;
                        linha["AntigoClienteID"] = assinaturaDesmembramento.AntigoClienteID.Valor;
                        linha["AssinaturaClienteID"] = assinaturaDesmembramento.AssinaturaClienteID.Valor;
                        linha["TimeStamp"] = assinaturaDesmembramento.TimeStamp.Valor;
                        linha["UsuarioID"] = assinaturaDesmembramento.UsuarioID.Valor;
                        linha["Motivo"] = assinaturaDesmembramento.Motivo.Valor;
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
                        sql = "SELECT ID, ClienteID FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "AntigoClienteID":
                        sql = "SELECT ID, AntigoClienteID FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY AntigoClienteID";
                        break;
                    case "AssinaturaClienteID":
                        sql = "SELECT ID, AssinaturaClienteID FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "Motivo":
                        sql = "SELECT ID, Motivo FROM tAssinaturaDesmembramento WHERE " + FiltroSQL + " ORDER BY Motivo";
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

    #region "AssinaturaDesmembramentoException"

    [Serializable]
    public class AssinaturaDesmembramentoException : Exception
    {

        public AssinaturaDesmembramentoException() : base() { }

        public AssinaturaDesmembramentoException(string msg) : base(msg) { }

        public AssinaturaDesmembramentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}