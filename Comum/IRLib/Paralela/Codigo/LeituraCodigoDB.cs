/******************************************************
* Arquivo LeituraCodigoDB.cs
* Gerado em: 28/08/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "LeituraCodigo_B"

    public abstract class LeituraCodigo_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public setorid SetorID = new setorid();
        public dataleitura DataLeitura = new dataleitura();
        public codigobarra CodigoBarra = new codigobarra();
        public portaria Portaria = new portaria();
        public codigoresultado CodigoResultado = new codigoresultado();

        public LeituraCodigo_B() { }

        /// <summary>
        /// Preenche todos os atributos de LeituraCodigo
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tLeituraCodigo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.DataLeitura.ValorBD = bd.LerString("DataLeitura");
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.Portaria.ValorBD = bd.LerString("Portaria");
                    this.CodigoResultado.ValorBD = bd.LerInt("CodigoResultado").ToString();
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

        }

        /// <summary>
        /// Inserir novo(a) LeituraCodigo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tLeituraCodigo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLeituraCodigo(ID, EventoID, ApresentacaoID, SetorID, DataLeitura, CodigoBarra, Portaria, CodigoResultado) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005','@006',@007)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.SetorID.ValorBD);
                sql.Replace("@004", this.DataLeitura.ValorBD);
                sql.Replace("@005", this.CodigoBarra.ValorBD);
                sql.Replace("@006", this.Portaria.ValorBD);
                sql.Replace("@007", this.CodigoResultado.ValorBD);

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
        /// Atualiza LeituraCodigo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLeituraCodigo SET EventoID = @001, ApresentacaoID = @002, SetorID = @003, DataLeitura = '@004', CodigoBarra = '@005', Portaria = '@006', CodigoResultado = @007 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.SetorID.ValorBD);
                sql.Replace("@004", this.DataLeitura.ValorBD);
                sql.Replace("@005", this.CodigoBarra.ValorBD);
                sql.Replace("@006", this.Portaria.ValorBD);
                sql.Replace("@007", this.CodigoResultado.ValorBD);

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
        /// Exclui LeituraCodigo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tLeituraCodigo WHERE ID=" + id;

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
        /// Exclui LeituraCodigo
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
            this.ApresentacaoID.Limpar();
            this.SetorID.Limpar();
            this.DataLeitura.Limpar();
            this.CodigoBarra.Limpar();
            this.Portaria.Limpar();
            this.CodigoResultado.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.EventoID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.SetorID.Desfazer();
            this.DataLeitura.Desfazer();
            this.CodigoBarra.Desfazer();
            this.Portaria.Desfazer();
            this.CodigoResultado.Desfazer();
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class dataleitura : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataLeitura";
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
                return base.Valor;
            }

        }

        public class portaria : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Portaria";
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
                return base.Valor;
            }

        }

        public class codigoresultado : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoResultado";
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

                DataTable tabela = new DataTable("LeituraCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("DataLeitura", typeof(DateTime));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("Portaria", typeof(string));
                tabela.Columns.Add("CodigoResultado", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "LeituraCodigoLista_B"

    public abstract class LeituraCodigoLista_B : BaseLista
    {

        protected LeituraCodigo leituraCodigo;

        // passar o Usuario logado no sistema
        public LeituraCodigoLista_B()
        {
            leituraCodigo = new LeituraCodigo();
        }

        public LeituraCodigo LeituraCodigo
        {
            get { return leituraCodigo; }
        }

        /// <summary>
        /// Retorna um IBaseBD de LeituraCodigo especifico
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
                    leituraCodigo.Ler(id);
                    return leituraCodigo;
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
                    sql = "SELECT ID FROM tLeituraCodigo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLeituraCodigo";

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
                    sql = "SELECT ID FROM tLeituraCodigo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLeituraCodigo";

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
        /// Preenche LeituraCodigo corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                leituraCodigo.Ler(id);

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

                bool ok = leituraCodigo.Excluir();
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

                        string sqlDelete = "DELETE FROM tLeituraCodigo WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) LeituraCodigo na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = leituraCodigo.Inserir();
                if (ok)
                {
                    lista.Add(leituraCodigo.Control.ID);
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
        /// Obtem uma tabela de todos os campos de LeituraCodigo carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("LeituraCodigo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("DataLeitura", typeof(DateTime));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("Portaria", typeof(string));
                tabela.Columns.Add("CodigoResultado", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = leituraCodigo.Control.ID;
                        linha["EventoID"] = leituraCodigo.EventoID.Valor;
                        linha["ApresentacaoID"] = leituraCodigo.ApresentacaoID.Valor;
                        linha["SetorID"] = leituraCodigo.SetorID.Valor;
                        linha["DataLeitura"] = leituraCodigo.DataLeitura.Valor;
                        linha["CodigoBarra"] = leituraCodigo.CodigoBarra.Valor;
                        linha["Portaria"] = leituraCodigo.Portaria.Valor;
                        linha["CodigoResultado"] = leituraCodigo.CodigoResultado.Valor;
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

                DataTable tabela = new DataTable("RelatorioLeituraCodigo");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("DataLeitura", typeof(DateTime));
                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("Portaria", typeof(string));
                    tabela.Columns.Add("CodigoResultado", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = leituraCodigo.EventoID.Valor;
                        linha["ApresentacaoID"] = leituraCodigo.ApresentacaoID.Valor;
                        linha["SetorID"] = leituraCodigo.SetorID.Valor;
                        linha["DataLeitura"] = leituraCodigo.DataLeitura.Valor;
                        linha["CodigoBarra"] = leituraCodigo.CodigoBarra.Valor;
                        linha["Portaria"] = leituraCodigo.Portaria.Valor;
                        linha["CodigoResultado"] = leituraCodigo.CodigoResultado.Valor;
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
                        sql = "SELECT ID, EventoID FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "DataLeitura":
                        sql = "SELECT ID, DataLeitura FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY DataLeitura";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "Portaria":
                        sql = "SELECT ID, Portaria FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY Portaria";
                        break;
                    case "CodigoResultado":
                        sql = "SELECT ID, CodigoResultado FROM tLeituraCodigo WHERE " + FiltroSQL + " ORDER BY CodigoResultado";
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

    #region "LeituraCodigoException"

    [Serializable]
    public class LeituraCodigoException : Exception
    {

        public LeituraCodigoException() : base() { }

        public LeituraCodigoException(string msg) : base(msg) { }

        public LeituraCodigoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}