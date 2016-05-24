/******************************************************
* Arquivo TaxaProcessamentoDB.cs
* Gerado em: 21/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "TaxaProcessamento_B"

    public abstract class TaxaProcessamento_B : BaseBD
    {

        public estadoid EstadoID = new estadoid();
        public valortaxa ValorTaxa = new valortaxa();
        public limitemaximoingressosestado LimiteMaximoIngressosEstado = new limitemaximoingressosestado();

        public TaxaProcessamento_B() { }

        // passar o Usuario logado no sistema
        public TaxaProcessamento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de TaxaProcessamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tTaxaProcessamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstadoID.ValorBD = bd.LerInt("EstadoID").ToString();
                    this.ValorTaxa.ValorBD = bd.LerDecimal("ValorTaxa").ToString();
                    this.LimiteMaximoIngressosEstado.ValorBD = bd.LerInt("LimiteMaximoIngressosEstado").ToString();
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
        /// Preenche todos os atributos de TaxaProcessamento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xTaxaProcessamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstadoID.ValorBD = bd.LerInt("EstadoID").ToString();
                    this.ValorTaxa.ValorBD = bd.LerDecimal("ValorTaxa").ToString();
                    this.LimiteMaximoIngressosEstado.ValorBD = bd.LerInt("LimiteMaximoIngressosEstado").ToString();
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
                sql.Append("INSERT INTO cTaxaProcessamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xTaxaProcessamento (ID, Versao, EstadoID, ValorTaxa, LimiteMaximoIngressosEstado) ");
                sql.Append("SELECT ID, @V, EstadoID, ValorTaxa, LimiteMaximoIngressosEstado FROM tTaxaProcessamento WHERE ID = @I");
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
        /// Inserir novo(a) TaxaProcessamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cTaxaProcessamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tTaxaProcessamento(ID, EstadoID, ValorTaxa, LimiteMaximoIngressosEstado) ");
                sql.Append("VALUES (@ID,@001,'@002',@003)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.ValorTaxa.ValorBD);
                sql.Replace("@003", this.LimiteMaximoIngressosEstado.ValorBD);

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
        /// Inserir novo(a) TaxaProcessamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cTaxaProcessamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tTaxaProcessamento(ID, EstadoID, ValorTaxa, LimiteMaximoIngressosEstado) ");
                sql.Append("VALUES (@ID,@001,'@002',@003)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.ValorTaxa.ValorBD);
                sql.Replace("@003", this.LimiteMaximoIngressosEstado.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza TaxaProcessamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cTaxaProcessamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tTaxaProcessamento SET EstadoID = @001, ValorTaxa = '@002', LimiteMaximoIngressosEstado = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.ValorTaxa.ValorBD);
                sql.Replace("@003", this.LimiteMaximoIngressosEstado.ValorBD);

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
        /// Atualiza TaxaProcessamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cTaxaProcessamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tTaxaProcessamento SET EstadoID = @001, ValorTaxa = '@002', LimiteMaximoIngressosEstado = @003 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.ValorTaxa.ValorBD);
                sql.Replace("@003", this.LimiteMaximoIngressosEstado.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui TaxaProcessamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cTaxaProcessamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tTaxaProcessamento WHERE ID=" + id;

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
        /// Exclui TaxaProcessamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cTaxaProcessamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tTaxaProcessamento WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui TaxaProcessamento
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

            this.EstadoID.Limpar();
            this.ValorTaxa.Limpar();
            this.LimiteMaximoIngressosEstado.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EstadoID.Desfazer();
            this.ValorTaxa.Desfazer();
            this.LimiteMaximoIngressosEstado.Desfazer();
        }

        public class estadoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstadoID";
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

        public class valortaxa : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorTaxa";
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

        public class limitemaximoingressosestado : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LimiteMaximoIngressosEstado";
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

                DataTable tabela = new DataTable("TaxaProcessamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstadoID", typeof(int));
                tabela.Columns.Add("ValorTaxa", typeof(decimal));
                tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "TaxaProcessamentoLista_B"

    public abstract class TaxaProcessamentoLista_B : BaseLista
    {

        private bool backup = false;
        protected TaxaProcessamento taxaProcessamento;

        // passar o Usuario logado no sistema
        public TaxaProcessamentoLista_B()
        {
            taxaProcessamento = new TaxaProcessamento();
        }

        // passar o Usuario logado no sistema
        public TaxaProcessamentoLista_B(int usuarioIDLogado)
        {
            taxaProcessamento = new TaxaProcessamento(usuarioIDLogado);
        }

        public TaxaProcessamento TaxaProcessamento
        {
            get { return taxaProcessamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de TaxaProcessamento especifico
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
                    taxaProcessamento.Ler(id);
                    return taxaProcessamento;
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
                    sql = "SELECT ID FROM tTaxaProcessamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tTaxaProcessamento";

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
                    sql = "SELECT ID FROM tTaxaProcessamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tTaxaProcessamento";

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
                    sql = "SELECT ID FROM xTaxaProcessamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xTaxaProcessamento";

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
        /// Preenche TaxaProcessamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    taxaProcessamento.Ler(id);
                else
                    taxaProcessamento.LerBackup(id);

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

                bool ok = taxaProcessamento.Excluir();
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
        /// Inseri novo(a) TaxaProcessamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = taxaProcessamento.Inserir();
                if (ok)
                {
                    lista.Add(taxaProcessamento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de TaxaProcessamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("TaxaProcessamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstadoID", typeof(int));
                tabela.Columns.Add("ValorTaxa", typeof(decimal));
                tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = taxaProcessamento.Control.ID;
                        linha["EstadoID"] = taxaProcessamento.EstadoID.Valor;
                        linha["ValorTaxa"] = taxaProcessamento.ValorTaxa.Valor;
                        linha["LimiteMaximoIngressosEstado"] = taxaProcessamento.LimiteMaximoIngressosEstado.Valor;
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

                DataTable tabela = new DataTable("RelatorioTaxaProcessamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EstadoID", typeof(int));
                    tabela.Columns.Add("ValorTaxa", typeof(decimal));
                    tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EstadoID"] = taxaProcessamento.EstadoID.Valor;
                        linha["ValorTaxa"] = taxaProcessamento.ValorTaxa.Valor;
                        linha["LimiteMaximoIngressosEstado"] = taxaProcessamento.LimiteMaximoIngressosEstado.Valor;
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
                    case "EstadoID":
                        sql = "SELECT ID, EstadoID FROM tTaxaProcessamento WHERE " + FiltroSQL + " ORDER BY EstadoID";
                        break;
                    case "ValorTaxa":
                        sql = "SELECT ID, ValorTaxa FROM tTaxaProcessamento WHERE " + FiltroSQL + " ORDER BY ValorTaxa";
                        break;
                    case "LimiteMaximoIngressosEstado":
                        sql = "SELECT ID, LimiteMaximoIngressosEstado FROM tTaxaProcessamento WHERE " + FiltroSQL + " ORDER BY LimiteMaximoIngressosEstado";
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

    #region "TaxaProcessamentoException"

    [Serializable]
    public class TaxaProcessamentoException : Exception
    {

        public TaxaProcessamentoException() : base() { }

        public TaxaProcessamentoException(string msg) : base(msg) { }

        public TaxaProcessamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}