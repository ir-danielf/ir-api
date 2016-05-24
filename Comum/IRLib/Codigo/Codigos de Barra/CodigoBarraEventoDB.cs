/******************************************************
* Arquivo CodigoBarraEventoDB.cs
* Gerado em: 23/09/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "CodigoBarraEvento_B"

    public abstract class CodigoBarraEvento_B : BaseBD
    {

        public apresentacaosetorid ApresentacaoSetorID = new apresentacaosetorid();
        public codigo Codigo = new codigo();
        public datainclusao DataInclusao = new datainclusao();
        public status Status = new status();
        public utilizado Utilizado = new utilizado();
        public eventoid EventoID = new eventoid();

        public CodigoBarraEvento_B() { }

        /// <summary>
        /// Preenche todos os atributos de CodigoBarraEvento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCodigoBarraEvento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.DataInclusao.ValorBD = bd.LerString("DataInclusao");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Utilizado.ValorBD = bd.LerString("Utilizado");
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
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
        /// Inserir novo(a) CodigoBarraEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tCodigoBarraEvento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCodigoBarraEvento(ID, ApresentacaoSetorID, Codigo, DataInclusao, Status, Utilizado, EventoID) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005',@006)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.DataInclusao.ValorBD);
                sql.Replace("@004", this.Status.ValorBD);
                sql.Replace("@005", this.Utilizado.ValorBD);
                sql.Replace("@006", this.EventoID.ValorBD);

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
        /// Atualiza CodigoBarraEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCodigoBarraEvento SET ApresentacaoSetorID = @001, Codigo = '@002', DataInclusao = '@003', Status = '@004', Utilizado = '@005', EventoID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.DataInclusao.ValorBD);
                sql.Replace("@004", this.Status.ValorBD);
                sql.Replace("@005", this.Utilizado.ValorBD);
                sql.Replace("@006", this.EventoID.ValorBD);

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
        /// Exclui CodigoBarraEvento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tCodigoBarraEvento WHERE ID=" + id;

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
        /// Exclui CodigoBarraEvento
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

            this.ApresentacaoSetorID.Limpar();
            this.Codigo.Limpar();
            this.DataInclusao.Limpar();
            this.Status.Limpar();
            this.Utilizado.Limpar();
            this.EventoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ApresentacaoSetorID.Desfazer();
            this.Codigo.Desfazer();
            this.DataInclusao.Desfazer();
            this.Status.Desfazer();
            this.Utilizado.Desfazer();
            this.EventoID.Desfazer();
        }

        public class apresentacaosetorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoSetorID";
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

        public class codigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Codigo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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

        public class datainclusao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataInclusao";
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

        public class status : BooleanProperty
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

        public class utilizado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Utilizado";
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

                DataTable tabela = new DataTable("CodigoBarraEvento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("DataInclusao", typeof(DateTime));
                tabela.Columns.Add("Status", typeof(bool));
                tabela.Columns.Add("Utilizado", typeof(bool));
                tabela.Columns.Add("EventoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "CodigoBarraEventoLista_B"

    public abstract class CodigoBarraEventoLista_B : BaseLista
    {

        protected CodigoBarraEvento codigoBarraEvento;

        // passar o Usuario logado no sistema
        public CodigoBarraEventoLista_B()
        {
            codigoBarraEvento = new CodigoBarraEvento();
        }

        public CodigoBarraEvento CodigoBarraEvento
        {
            get { return codigoBarraEvento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CodigoBarraEvento especifico
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
                    codigoBarraEvento.Ler(id);
                    return codigoBarraEvento;
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
                    sql = "SELECT ID FROM tCodigoBarraEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCodigoBarraEvento";

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
                    sql = "SELECT ID FROM tCodigoBarraEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCodigoBarraEvento";

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
        /// Preenche CodigoBarraEvento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                codigoBarraEvento.Ler(id);

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

                bool ok = codigoBarraEvento.Excluir();
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

                        string sqlDelete = "DELETE FROM tCodigoBarraEvento WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) CodigoBarraEvento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = codigoBarraEvento.Inserir();
                if (ok)
                {
                    lista.Add(codigoBarraEvento.Control.ID);
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
        /// Obtem uma tabela de todos os campos de CodigoBarraEvento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CodigoBarraEvento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("DataInclusao", typeof(DateTime));
                tabela.Columns.Add("Status", typeof(bool));
                tabela.Columns.Add("Utilizado", typeof(bool));
                tabela.Columns.Add("EventoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = codigoBarraEvento.Control.ID;
                        linha["ApresentacaoSetorID"] = codigoBarraEvento.ApresentacaoSetorID.Valor;
                        linha["Codigo"] = codigoBarraEvento.Codigo.Valor;
                        linha["DataInclusao"] = codigoBarraEvento.DataInclusao.Valor;
                        linha["Status"] = codigoBarraEvento.Status.Valor;
                        linha["Utilizado"] = codigoBarraEvento.Utilizado.Valor;
                        linha["EventoID"] = codigoBarraEvento.EventoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioCodigoBarraEvento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                    tabela.Columns.Add("Codigo", typeof(string));
                    tabela.Columns.Add("DataInclusao", typeof(DateTime));
                    tabela.Columns.Add("Status", typeof(bool));
                    tabela.Columns.Add("Utilizado", typeof(bool));
                    tabela.Columns.Add("EventoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ApresentacaoSetorID"] = codigoBarraEvento.ApresentacaoSetorID.Valor;
                        linha["Codigo"] = codigoBarraEvento.Codigo.Valor;
                        linha["DataInclusao"] = codigoBarraEvento.DataInclusao.Valor;
                        linha["Status"] = codigoBarraEvento.Status.Valor;
                        linha["Utilizado"] = codigoBarraEvento.Utilizado.Valor;
                        linha["EventoID"] = codigoBarraEvento.EventoID.Valor;
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
                    case "ApresentacaoSetorID":
                        sql = "SELECT ID, ApresentacaoSetorID FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    case "Codigo":
                        sql = "SELECT ID, Codigo FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY Codigo";
                        break;
                    case "DataInclusao":
                        sql = "SELECT ID, DataInclusao FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY DataInclusao";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "Utilizado":
                        sql = "SELECT ID, Utilizado FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY Utilizado";
                        break;
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tCodigoBarraEvento WHERE " + FiltroSQL + " ORDER BY EventoID";
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

    #region "CodigoBarraEventoException"

    [Serializable]
    public class CodigoBarraEventoException : Exception
    {

        public CodigoBarraEventoException() : base() { }

        public CodigoBarraEventoException(string msg) : base(msg) { }

        public CodigoBarraEventoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}