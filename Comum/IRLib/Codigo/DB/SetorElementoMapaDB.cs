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

    #region "SetorElementoMapa_B"

    public abstract class SetorElementoMapa_B : BaseBD
    {
        public x X = new x();
        public y Y = new y();
        public z Z = new z();
        public setorid SetorID = new setorid();
        public elementomapaid ElementoMapaID = new elementomapaid();
        public conteudo Conteudo = new conteudo();

        public SetorElementoMapa_B() { }

        public SetorElementoMapa_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de SetorElementoMapa
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM SetorElementoMapa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.X.ValorBD = bd.LerInt("X").ToString();
                    this.Y.ValorBD = bd.LerInt("Y").ToString();
                    this.Z.ValorBD = bd.LerInt("Z").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.ElementoMapaID.ValorBD = bd.LerInt("ElementoMapaID").ToString();
                    this.Conteudo.ValorBD = bd.LerString("Conteudo").ToString();
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
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO SetorElementoMapa(X, Y, Z, SetorID, ElementoMapaID, Conteudo) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,'@006')");

                sql.Replace("@001", this.X.ValorBD);
                sql.Replace("@002", this.Y.ValorBD);
                sql.Replace("@003", this.Z.ValorBD);
                sql.Replace("@004", this.SetorID.ValorBD);
                sql.Replace("@005", this.ElementoMapaID.ValorBD);
                sql.Replace("@006", this.Conteudo.ValorBD);

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
                sql.Append("UPDATE SetorElementoMapa SET X = @001, Y = @002, Z = @003, SetorID = @004, ElementoMapaID = @005, Conteudo = '@006'");
                sql.Append("WHERE ID = @ID");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.X.ValorBD);
                sql.Replace("@002", this.Y.ValorBD);
                sql.Replace("@003", this.Z.ValorBD);
                sql.Replace("@004", this.SetorID.ValorBD);
                sql.Replace("@005", this.ElementoMapaID.ValorBD);
                sql.Replace("@006", this.Conteudo.ValorBD);

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

                string sqlDelete = "DELETE FROM SetorElementoMapa WHERE ID=" + id;

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

            this.X.Limpar();
            this.Y.Limpar();
            this.Z.Limpar();
            this.SetorID.Limpar();
            this.ElementoMapaID.Limpar();
            this.Conteudo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.X.Desfazer();
            this.Y.Desfazer();
            this.Z.Desfazer();
            this.SetorID.Desfazer();
            this.ElementoMapaID.Desfazer();
            this.Conteudo.Desfazer();
        }

        public class x : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "X";
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

        public class y : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Y";
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
        public class z : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Z";
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


        public class setorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SetorID";
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

        public class elementomapaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ElementoMapaID";
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

        public class conteudo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Conteudo";
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

                DataTable tabela = new DataTable("SetorElementoMapa");

                tabela.Columns.Add("X", typeof(int));
                tabela.Columns.Add("Y", typeof(int));
                tabela.Columns.Add("Z", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ElementoMapaID", typeof(int));
                tabela.Columns.Add("Conteudo", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "SetorElementoMapaLista_B"

    public abstract class SetorElementoMapaLista_B : BaseLista
    {

        protected SetorElementoMapa elementoMapa;

        // passar o Usuario logado no sistema
        public SetorElementoMapaLista_B()
        {
            elementoMapa = new SetorElementoMapa();
        }

        public SetorElementoMapa CotaItemFormaPagamento
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
                    sql = "SELECT ID FROM SetorElementoMapa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM SetorElementoMapa";

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
                    sql = "SELECT ID FROM SetorElementoMapa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM SetorElementoMapa";

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

                        string sqlDelete = "DELETE FROM SetorElementoMapa WHERE ID in (" + ids + ")";

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

                DataTable tabela = new DataTable("SetorElementoMapa");

                tabela.Columns.Add("X", typeof(int));
                tabela.Columns.Add("Y", typeof(int));
                tabela.Columns.Add("Z", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ElementoMapaID", typeof(int));
                tabela.Columns.Add("Conteudo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["X"] = elementoMapa.X.ID;
                        linha["Y"] = elementoMapa.Y.Valor;
                        linha["Z"] = elementoMapa.Z.Valor;
                        linha["SetorID"] = elementoMapa.SetorID.Valor;
                        linha["ElementoMapaID"] = elementoMapa.ElementoMapaID.Valor;
                        linha["Conteudo"] = elementoMapa.Conteudo.Valor;
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

                DataTable tabela = new DataTable("SetorElementoMapa");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("X", typeof(int));
                    tabela.Columns.Add("Y", typeof(int));
                    tabela.Columns.Add("Z", typeof(int));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("ElementoMapaID", typeof(int));
                    tabela.Columns.Add("Conteudo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["X"] = elementoMapa.X.ID;
                        linha["Y"] = elementoMapa.Y.Valor;
                        linha["Z"] = elementoMapa.Z.Valor;
                        linha["SetorID"] = elementoMapa.SetorID.Valor;
                        linha["ElementoMapaID"] = elementoMapa.ElementoMapaID.Valor;
                        linha["Conteudo"] = elementoMapa.Conteudo.Valor;
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
                    case "X":
                        sql = "SELECT ID, X FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY X";
                        break;
                    case "Y":
                        sql = "SELECT ID, Y FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Y";
                        break;
                    case "Z":
                        sql = "SELECT ID, Z FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Z";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "ElementoMapaID":
                        sql = "SELECT ID, ElementoMapaID FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY ElementoMapaID";
                        break;
                    case "Conteudo":
                        sql = "SELECT ID, Conteudo FROM ElementoMapa WHERE " + FiltroSQL + " ORDER BY Conteudo";
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

    #region "SetorElementoMapaException"

    [Serializable]
    public class SetorElementoMapaException : Exception
    {

        public SetorElementoMapaException() : base() { }

        public SetorElementoMapaException(string msg) : base(msg) { }

        public SetorElementoMapaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}