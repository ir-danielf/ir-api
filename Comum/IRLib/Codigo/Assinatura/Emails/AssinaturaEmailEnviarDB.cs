﻿/******************************************************
* Arquivo AssinaturaEmailEnviarDB.cs
* Gerado em: 19/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "AssinaturaEmailEnviar_B"

    public abstract class AssinaturaEmailEnviar_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public assinaturaemailmodeloid AssinaturaEmailModeloID = new assinaturaemailmodeloid();
        public enviado Enviado = new enviado();
        public dataenvio DataEnvio = new dataenvio();
        public erro Erro = new erro();

        public AssinaturaEmailEnviar_B() { }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaEmailEnviar
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaEmailEnviar WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.AssinaturaEmailModeloID.ValorBD = bd.LerInt("AssinaturaEmailModeloID").ToString();
                    this.Enviado.ValorBD = bd.LerString("Enviado");
                    this.DataEnvio.ValorBD = bd.LerString("DataEnvio");
                    this.Erro.ValorBD = bd.LerString("Erro");
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
        /// Inserir novo(a) AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaEmailEnviar(ClienteID, AssinaturaEmailModeloID, Enviado, DataEnvio, Erro) ");
                sql.Append("VALUES (@001,@002,'@003','@004','@005'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AssinaturaEmailModeloID.ValorBD);
                sql.Replace("@003", this.Enviado.ValorBD);
                sql.Replace("@004", this.DataEnvio.ValorBD);
                sql.Replace("@005", this.Erro.ValorBD);

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
        /// Inserir novo(a) AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tAssinaturaEmailEnviar(ClienteID, AssinaturaEmailModeloID, Enviado, DataEnvio, Erro) ");
            sql.Append("VALUES (@001,@002,'@003','@004','@005'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AssinaturaEmailModeloID.ValorBD);
            sql.Replace("@003", this.Enviado.ValorBD);
            sql.Replace("@004", this.DataEnvio.ValorBD);
            sql.Replace("@005", this.Erro.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinaturaEmailEnviar SET ClienteID = @001, AssinaturaEmailModeloID = @002, Enviado = '@003', DataEnvio = '@004', Erro = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.AssinaturaEmailModeloID.ValorBD);
                sql.Replace("@003", this.Enviado.ValorBD);
                sql.Replace("@004", this.DataEnvio.ValorBD);
                sql.Replace("@005", this.Erro.ValorBD);

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
        /// Atualiza AssinaturaEmailEnviar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tAssinaturaEmailEnviar SET ClienteID = @001, AssinaturaEmailModeloID = @002, Enviado = '@003', DataEnvio = '@004', Erro = '@005' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.AssinaturaEmailModeloID.ValorBD);
            sql.Replace("@003", this.Enviado.ValorBD);
            sql.Replace("@004", this.DataEnvio.ValorBD);
            sql.Replace("@005", this.Erro.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui AssinaturaEmailEnviar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tAssinaturaEmailEnviar WHERE ID=" + id;

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
        /// Exclui AssinaturaEmailEnviar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tAssinaturaEmailEnviar WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui AssinaturaEmailEnviar
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
            this.AssinaturaEmailModeloID.Limpar();
            this.Enviado.Limpar();
            this.DataEnvio.Limpar();
            this.Erro.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.AssinaturaEmailModeloID.Desfazer();
            this.Enviado.Desfazer();
            this.DataEnvio.Desfazer();
            this.Erro.Desfazer();
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

        public class assinaturaemailmodeloid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaEmailModeloID";
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

        public class enviado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Enviado";
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

        public class dataenvio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEnvio";
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

        public class erro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Erro";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
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

                DataTable tabela = new DataTable("AssinaturaEmailEnviar");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaEmailModeloID", typeof(int));
                tabela.Columns.Add("Enviado", typeof(bool));
                tabela.Columns.Add("DataEnvio", typeof(DateTime));
                tabela.Columns.Add("Erro", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaEmailEnviarLista_B"

    public abstract class AssinaturaEmailEnviarLista_B : BaseLista
    {

        protected AssinaturaEmailEnviar assinaturaEmailEnviar;

        // passar o Usuario logado no sistema
        public AssinaturaEmailEnviarLista_B()
        {
            assinaturaEmailEnviar = new AssinaturaEmailEnviar();
        }

        public AssinaturaEmailEnviar AssinaturaEmailEnviar
        {
            get { return assinaturaEmailEnviar; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaEmailEnviar especifico
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
                    assinaturaEmailEnviar.Ler(id);
                    return assinaturaEmailEnviar;
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
                    sql = "SELECT ID FROM tAssinaturaEmailEnviar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailEnviar";

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
                    sql = "SELECT ID FROM tAssinaturaEmailEnviar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaEmailEnviar";

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
        /// Preenche AssinaturaEmailEnviar corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                assinaturaEmailEnviar.Ler(id);

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

                bool ok = assinaturaEmailEnviar.Excluir();
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

                        string sqlDelete = "DELETE FROM tAssinaturaEmailEnviar WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) AssinaturaEmailEnviar na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaEmailEnviar.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaEmailEnviar.Control.ID);
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
        /// Obtem uma tabela de todos os campos de AssinaturaEmailEnviar carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaEmailEnviar");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("AssinaturaEmailModeloID", typeof(int));
                tabela.Columns.Add("Enviado", typeof(bool));
                tabela.Columns.Add("DataEnvio", typeof(DateTime));
                tabela.Columns.Add("Erro", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaEmailEnviar.Control.ID;
                        linha["ClienteID"] = assinaturaEmailEnviar.ClienteID.Valor;
                        linha["AssinaturaEmailModeloID"] = assinaturaEmailEnviar.AssinaturaEmailModeloID.Valor;
                        linha["Enviado"] = assinaturaEmailEnviar.Enviado.Valor;
                        linha["DataEnvio"] = assinaturaEmailEnviar.DataEnvio.Valor;
                        linha["Erro"] = assinaturaEmailEnviar.Erro.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinaturaEmailEnviar");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("AssinaturaEmailModeloID", typeof(int));
                    tabela.Columns.Add("Enviado", typeof(bool));
                    tabela.Columns.Add("DataEnvio", typeof(DateTime));
                    tabela.Columns.Add("Erro", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = assinaturaEmailEnviar.ClienteID.Valor;
                        linha["AssinaturaEmailModeloID"] = assinaturaEmailEnviar.AssinaturaEmailModeloID.Valor;
                        linha["Enviado"] = assinaturaEmailEnviar.Enviado.Valor;
                        linha["DataEnvio"] = assinaturaEmailEnviar.DataEnvio.Valor;
                        linha["Erro"] = assinaturaEmailEnviar.Erro.Valor;
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
                        sql = "SELECT ID, ClienteID FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "AssinaturaEmailModeloID":
                        sql = "SELECT ID, AssinaturaEmailModeloID FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY AssinaturaEmailModeloID";
                        break;
                    case "Enviado":
                        sql = "SELECT ID, Enviado FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY Enviado";
                        break;
                    case "DataEnvio":
                        sql = "SELECT ID, DataEnvio FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY DataEnvio";
                        break;
                    case "Erro":
                        sql = "SELECT ID, Erro FROM tAssinaturaEmailEnviar WHERE " + FiltroSQL + " ORDER BY Erro";
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

    #region "AssinaturaEmailEnviarException"

    [Serializable]
    public class AssinaturaEmailEnviarException : Exception
    {

        public AssinaturaEmailEnviarException() : base() { }

        public AssinaturaEmailEnviarException(string msg) : base(msg) { }

        public AssinaturaEmailEnviarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}