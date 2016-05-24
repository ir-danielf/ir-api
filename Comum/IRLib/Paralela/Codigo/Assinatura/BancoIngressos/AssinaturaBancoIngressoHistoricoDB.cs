/******************************************************
* Arquivo AssinaturaBancoIngressoHistoricoDB.cs
* Gerado em: 06/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "AssinaturaBancoIngressoHistorico_B"

    public abstract class AssinaturaBancoIngressoHistorico_B : BaseBD
    {

        public assinaturabancoingressoid AssinaturaBancoIngressoID = new assinaturabancoingressoid();
        public assinaturabancoingressocreditoid AssinaturaBancoIngressoCreditoID = new assinaturabancoingressocreditoid();
        public assianturabancoingressocomprovanteid AssianturaBancoIngressoComprovanteID = new assianturabancoingressocomprovanteid();

        public AssinaturaBancoIngressoHistorico_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaBancoIngressoHistorico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaBancoIngressoHistorico WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.AssinaturaBancoIngressoID.ValorBD = bd.LerInt("AssinaturaBancoIngressoID").ToString();
                    this.AssinaturaBancoIngressoCreditoID.ValorBD = bd.LerInt("AssinaturaBancoIngressoCreditoID").ToString();
                    this.AssianturaBancoIngressoComprovanteID.ValorBD = bd.LerInt("AssianturaBancoIngressoComprovanteID").ToString();
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
        /// Inserir novo(a) AssinaturaBancoIngressoHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaBancoIngressoHistorico(AssinaturaBancoIngressoID, AssinaturaBancoIngressoCreditoID, AssianturaBancoIngressoComprovanteID) ");
                sql.Append("VALUES (@001,@002,@003); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaBancoIngressoID.ValorBD);
                sql.Replace("@002", this.AssinaturaBancoIngressoCreditoID.ValorBD);
                sql.Replace("@003", this.AssianturaBancoIngressoComprovanteID.ValorBD);

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
        /// Inserir novo(a) AssinaturaBancoIngressoHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaBancoIngressoHistorico(AssinaturaBancoIngressoID, AssinaturaBancoIngressoCreditoID, AssianturaBancoIngressoComprovanteID) ");
            sql.Append("VALUES (@001,@002,@003); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.AssinaturaBancoIngressoID.ValorBD);
            sql.Replace("@002", this.AssinaturaBancoIngressoCreditoID.ValorBD);
            sql.Replace("@003", this.AssianturaBancoIngressoComprovanteID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaBancoIngressoHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaBancoIngressoHistorico SET AssinaturaBancoIngressoID = @001, AssinaturaBancoIngressoCreditoID = @002, AssianturaBancoIngressoComprovanteID = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.AssinaturaBancoIngressoID.ValorBD);
                sql.Replace("@002", this.AssinaturaBancoIngressoCreditoID.ValorBD);
                sql.Replace("@003", this.AssianturaBancoIngressoComprovanteID.ValorBD);

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
        /// Atualiza AssinaturaBancoIngressoHistorico
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaBancoIngressoHistorico SET AssinaturaBancoIngressoID = @001, AssinaturaBancoIngressoCreditoID = @002, AssianturaBancoIngressoComprovanteID = @003 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.AssinaturaBancoIngressoID.ValorBD);
            sql.Replace("@002", this.AssinaturaBancoIngressoCreditoID.ValorBD);
            sql.Replace("@003", this.AssianturaBancoIngressoComprovanteID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaBancoIngressoHistorico com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoHistorico WHERE ID=" + id;

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
        /// Exclui AssinaturaBancoIngressoHistorico com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoHistorico WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaBancoIngressoHistorico
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

            this.AssinaturaBancoIngressoID.Limpar();
            this.AssinaturaBancoIngressoCreditoID.Limpar();
            this.AssianturaBancoIngressoComprovanteID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.AssinaturaBancoIngressoID.Desfazer();
            this.AssinaturaBancoIngressoCreditoID.Desfazer();
            this.AssianturaBancoIngressoComprovanteID.Desfazer();
        }

        public class assinaturabancoingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaBancoIngressoID";
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

        public class assinaturabancoingressocreditoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaBancoIngressoCreditoID";
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

        public class assianturabancoingressocomprovanteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssianturaBancoIngressoComprovanteID";
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

                DataTable tabela = new DataTable("AssinaturaBancoIngressoHistorico");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaBancoIngressoID", typeof(int));
                tabela.Columns.Add("AssinaturaBancoIngressoCreditoID", typeof(int));
                tabela.Columns.Add("AssianturaBancoIngressoComprovanteID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaBancoIngressoHistoricoLista_B"

    public abstract class AssinaturaBancoIngressoHistoricoLista_B : BaseLista
    {

        protected AssinaturaBancoIngressoHistorico assinaturaBancoIngressoHistorico;

        // passar o Usuario logado no sistema
        public AssinaturaBancoIngressoHistoricoLista_B()
        {
            assinaturaBancoIngressoHistorico = new AssinaturaBancoIngressoHistorico();
        }

        public AssinaturaBancoIngressoHistorico AssinaturaBancoIngressoHistorico
        {
            get { return assinaturaBancoIngressoHistorico; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaBancoIngressoHistorico especifico
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
                    assinaturaBancoIngressoHistorico.Ler(id);
                    return assinaturaBancoIngressoHistorico;
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
                    sql = "SELECT ID FROM tAssinaturaBancoIngressoHistorico";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngressoHistorico";

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
                    sql = "SELECT ID FROM tAssinaturaBancoIngressoHistorico";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaBancoIngressoHistorico";

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
        /// Preenche AssinaturaBancoIngressoHistorico corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaBancoIngressoHistorico.Ler(id);

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

                bool ok = assinaturaBancoIngressoHistorico.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaBancoIngressoHistorico WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaBancoIngressoHistorico na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaBancoIngressoHistorico.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaBancoIngressoHistorico.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaBancoIngressoHistorico carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaBancoIngressoHistorico");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("AssinaturaBancoIngressoID", typeof(int));
                tabela.Columns.Add("AssinaturaBancoIngressoCreditoID", typeof(int));
                tabela.Columns.Add("AssianturaBancoIngressoComprovanteID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaBancoIngressoHistorico.Control.ID;
                        linha["AssinaturaBancoIngressoID"] = assinaturaBancoIngressoHistorico.AssinaturaBancoIngressoID.Valor;
                        linha["AssinaturaBancoIngressoCreditoID"] = assinaturaBancoIngressoHistorico.AssinaturaBancoIngressoCreditoID.Valor;
                        linha["AssianturaBancoIngressoComprovanteID"] = assinaturaBancoIngressoHistorico.AssianturaBancoIngressoComprovanteID.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaBancoIngressoHistorico");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("AssinaturaBancoIngressoID", typeof(int));
                    tabela.Columns.Add("AssinaturaBancoIngressoCreditoID", typeof(int));
                    tabela.Columns.Add("AssianturaBancoIngressoComprovanteID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["AssinaturaBancoIngressoID"] = assinaturaBancoIngressoHistorico.AssinaturaBancoIngressoID.Valor;
                        linha["AssinaturaBancoIngressoCreditoID"] = assinaturaBancoIngressoHistorico.AssinaturaBancoIngressoCreditoID.Valor;
                        linha["AssianturaBancoIngressoComprovanteID"] = assinaturaBancoIngressoHistorico.AssianturaBancoIngressoComprovanteID.Valor;
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
                    case "AssinaturaBancoIngressoID":
                        sql = "SELECT ID, AssinaturaBancoIngressoID FROM tAssinaturaBancoIngressoHistorico WHERE " + FiltroSQL + " ORDER BY AssinaturaBancoIngressoID";
                        break;
                    case "AssinaturaBancoIngressoCreditoID":
                        sql = "SELECT ID, AssinaturaBancoIngressoCreditoID FROM tAssinaturaBancoIngressoHistorico WHERE " + FiltroSQL + " ORDER BY AssinaturaBancoIngressoCreditoID";
                        break;
                    case "AssianturaBancoIngressoComprovanteID":
                        sql = "SELECT ID, AssianturaBancoIngressoComprovanteID FROM tAssinaturaBancoIngressoHistorico WHERE " + FiltroSQL + " ORDER BY AssianturaBancoIngressoComprovanteID";
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

    #region "AssinaturaBancoIngressoHistoricoException"

    [Serializable]
    public class AssinaturaBancoIngressoHistoricoException : Exception
    {

        public AssinaturaBancoIngressoHistoricoException() : base() { }

        public AssinaturaBancoIngressoHistoricoException(string msg) : base(msg) { }

        public AssinaturaBancoIngressoHistoricoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}