/******************************************************
* Arquivo CotaItemFormaPagamentoDB.cs
* Gerado em: 14/01/2010
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "ElementoMapa_B"

    public abstract class ElementoMapa_B : BaseBD
    {
        public nome Nome = new nome();
        public valor Valor = new valor();
        public listar Listar = new listar();
        public tipo Tipo = new tipo();

        public ElementoMapa_B() { }
        public ElementoMapa_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }


        /// <summary>
        /// Preenche todos os atributos de CotaItemFormaPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM ElementoMapa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome").ToString();
                    this.Valor.ValorBD = bd.LerString("Valor").ToString();
                    this.Listar.ValorBD = bd.LerString("Listar").ToString();
                    this.Tipo.ValorBD = bd.LerString("Tipo").ToString();
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
        /// Inserir novo(a) CotaItemFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public int InserirRetorno()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO ElementoMapa(Nome, Valor, Listar, Tipo) ");
                sql.Append("OUTPUT INSERTED.ID ");
                sql.Append("VALUES ('@001','@002',@003,'@004')");

                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.Listar.ValorBD);
                sql.Replace("@004", this.Tipo.ValorBD);

                int x = (int)bd.ConsultaValor(sql.ToString());
                bd.Fechar();

                //bool result = Convert.ToBoolean(x);

                return x;

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
        /// Inserir novo(a) CotaItemFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO ElementoMapa(Nome, Valor, Listar, Tipo) ");
                sql.Append("VALUES ('@001','@002',@003,'@004')");

                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.Listar.ValorBD);
                sql.Replace("@004", this.Tipo.ValorBD);

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
        /// Atualiza CotaItemFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE ElementoMapa SET Nome = '@001', Valor = '@002', Listar = @003, Tipo = '@004'");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.Listar.ValorBD);
                sql.Replace("@004", this.Tipo.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui CotaItemFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM ElementoMapa WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui CotaItemFormaPagamento
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
            this.Valor.Limpar();
            this.Listar.Limpar();
            this.Tipo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Nome.Desfazer();
            this.Valor.Desfazer();
            this.Listar.Desfazer();
            this.Tipo.Desfazer();
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
                    return 0;
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
                return base.Valor.ToString();
            }

        }

        public class valor : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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
                return base.Valor.ToString();
            }

        }

        public class listar : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Listar";
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

        public class tipo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Tipo";
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

                DataTable tabela = new DataTable("ElementoMapa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Largura", typeof(int));
                tabela.Columns.Add("Altura", typeof(int));
                tabela.Columns.Add("Valor", typeof(string));
                tabela.Columns.Add("Listar", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "CotaItemFormaPagamentoLista_B"

    public abstract class ElementoMapaLista_B : BaseLista
    {

        protected ElementoMapa elementoMapa;

        // passar o Usuario logado no sistema
        public ElementoMapaLista_B()
        {
            elementoMapa = new ElementoMapa();
        }

        public ElementoMapa CotaItemFormaPagamento
        {
            get { return elementoMapa; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CotaItemFormaPagamento especifico
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
                    elementoMapa.Ler(id);
                    return elementoMapa;
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
                    sql = "SELECT ID FROM ElementoMapa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM ElementoMapa";

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
                    sql = "SELECT ID FROM ElementoMapa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM ElementoMapa";

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
        /// Preenche CotaItemFormaPagamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                elementoMapa.Ler(id);

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

                bool ok = elementoMapa.Excluir();
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

                        string sqlDelete = "DELETE FROM ElementoMapa WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) CotaItemFormaPagamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = elementoMapa.Inserir();
                if (ok)
                {
                    lista.Add(elementoMapa.Control.ID);
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
        /// Obtem uma tabela de todos os campos de CotaItemFormaPagamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ElementoMapa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(string));
                tabela.Columns.Add("Listar", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = elementoMapa.Control.ID;
                        linha["Nome"] = elementoMapa.Nome.Valor;
                        linha["Valor"] = elementoMapa.Valor.Valor;
                        linha["Listar"] = elementoMapa.Listar.Valor;
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

                DataTable tabela = new DataTable("ElementoMapa");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Valor", typeof(string));
                    tabela.Columns.Add("Listar", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = elementoMapa.Control.ID;
                        linha["Nome"] = elementoMapa.Nome.Valor;
                        linha["Valor"] = elementoMapa.Valor.Valor;
                        linha["Listar"] = elementoMapa.Listar.Valor;
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
                        sql = "SELECT ID, Nome FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Largura":
                        sql = "SELECT ID, Largura FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Largura";
                        break;
                    case "Altura":
                        sql = "SELECT ID, Altura FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Altura";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "Listar":
                        sql = "SELECT ID, Listar FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Listar";
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

    #region "ElementoMapaException"

    [Serializable]
    public class ElementoMapaException : Exception
    {

        public ElementoMapaException() : base() { }

        public ElementoMapaException(string msg) : base(msg) { }

        public ElementoMapaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}