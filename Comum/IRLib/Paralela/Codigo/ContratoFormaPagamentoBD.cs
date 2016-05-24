/******************************************************
* Arquivo ContratoFormaPagamentoDB.cs
* Gerado em: 23/04/2009
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "ContratoFormaPagamento_B"

    public abstract class ContratoFormaPagamento_B : BaseBD
    {

        public contratoid ContratoID = new contratoid();
        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public dias Dias = new dias();
        public taxa Taxa = new taxa();

        public ContratoFormaPagamento_B() { }

        // passar o Usuario logado no sistema
        public ContratoFormaPagamento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ContratoFormaPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tContratoFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();
                    this.Taxa.ValorBD = bd.LerDecimal("Taxa").ToString();
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
        /// Preenche todos os atributos de ContratoFormaPagamento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xContratoFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.Dias.ValorBD = bd.LerInt("Dias").ToString();
                    this.Taxa.ValorBD = bd.LerDecimal("Taxa").ToString();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cContratoFormaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xContratoFormaPagamento (ID, Versao, ContratoID, FormaPagamentoID, Dias, Taxa) ");
                sql.Append("SELECT ID, @V, ContratoID, FormaPagamentoID, Dias, Taxa FROM tContratoFormaPagamento WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) ContratoFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContratoFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContratoFormaPagamento(ID, ContratoID, FormaPagamentoID, Dias, Taxa) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.Dias.ValorBD);
                sql.Replace("@004", this.Taxa.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Atualiza ContratoFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cContratoFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContratoFormaPagamento SET ContratoID = @001, FormaPagamentoID = @002, Dias = @003, Taxa = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.Dias.ValorBD);
                sql.Replace("@004", this.Taxa.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui ContratoFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cContratoFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tContratoFormaPagamento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui ContratoFormaPagamento
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

            this.ContratoID.Limpar();
            this.FormaPagamentoID.Limpar();
            this.Dias.Limpar();
            this.Taxa.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.ContratoID.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.Dias.Desfazer();
            this.Taxa.Desfazer();
        }

        public class contratoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContratoID";
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

        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
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

        public class dias : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Dias";
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

        public class taxa : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Taxa";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 5;
                }
            }

            public override decimal Valor
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
                return base.Valor.ToString("###,##0.00");
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

                DataTable tabela = new DataTable("ContratoFormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("Taxa", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ContratoFormaPagamentoLista_B"

    public abstract class ContratoFormaPagamentoLista_B : BaseLista
    {

        private bool backup = false;
        protected ContratoFormaPagamento contratoFormaPagamento;

        // passar o Usuario logado no sistema
        public ContratoFormaPagamentoLista_B()
        {
            contratoFormaPagamento = new ContratoFormaPagamento();
        }

        // passar o Usuario logado no sistema
        public ContratoFormaPagamentoLista_B(int usuarioIDLogado)
        {
            contratoFormaPagamento = new ContratoFormaPagamento(usuarioIDLogado);
        }

        public ContratoFormaPagamento ContratoFormaPagamento
        {
            get { return contratoFormaPagamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ContratoFormaPagamento especifico
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
                    contratoFormaPagamento.Ler(id);
                    return contratoFormaPagamento;
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
                    sql = "SELECT ID FROM tContratoFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContratoFormaPagamento";

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
                    sql = "SELECT ID FROM tContratoFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContratoFormaPagamento";

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
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xContratoFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xContratoFormaPagamento";

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

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche ContratoFormaPagamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    contratoFormaPagamento.Ler(id);
                else
                    contratoFormaPagamento.LerBackup(id);

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

                bool ok = contratoFormaPagamento.Excluir();
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

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

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
        /// Inseri novo(a) ContratoFormaPagamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = contratoFormaPagamento.Inserir();
                if (ok)
                {
                    lista.Add(contratoFormaPagamento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ContratoFormaPagamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ContratoFormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("Taxa", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = contratoFormaPagamento.Control.ID;
                        linha["ContratoID"] = contratoFormaPagamento.ContratoID.Valor;
                        linha["FormaPagamentoID"] = contratoFormaPagamento.FormaPagamentoID.Valor;
                        linha["Dias"] = contratoFormaPagamento.Dias.Valor;
                        linha["Taxa"] = contratoFormaPagamento.Taxa.Valor;
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

                DataTable tabela = new DataTable("RelatorioContratoFormaPagamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ContratoID", typeof(int));
                    tabela.Columns.Add("FormaPagamentoID", typeof(int));
                    tabela.Columns.Add("Dias", typeof(int));
                    tabela.Columns.Add("Taxa", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ContratoID"] = contratoFormaPagamento.ContratoID.Valor;
                        linha["FormaPagamentoID"] = contratoFormaPagamento.FormaPagamentoID.Valor;
                        linha["Dias"] = contratoFormaPagamento.Dias.Valor;
                        linha["Taxa"] = contratoFormaPagamento.Taxa.Valor;
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
                    case "ContratoID":
                        sql = "SELECT ID, ContratoID FROM tContratoFormaPagamento WHERE " + FiltroSQL + " ORDER BY ContratoID";
                        break;
                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tContratoFormaPagamento WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;
                    case "Dias":
                        sql = "SELECT ID, Dias FROM tContratoFormaPagamento WHERE " + FiltroSQL + " ORDER BY Dias";
                        break;
                    case "Taxa":
                        sql = "SELECT ID, Taxa FROM tContratoFormaPagamento WHERE " + FiltroSQL + " ORDER BY Taxa";
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

    #region "ContratoFormaPagamentoException"

    [Serializable]
    public class ContratoFormaPagamentoException : Exception
    {

        public ContratoFormaPagamentoException() : base() { }

        public ContratoFormaPagamentoException(string msg) : base(msg) { }

        public ContratoFormaPagamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}