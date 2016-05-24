/******************************************************
* Arquivo EstadoDB.cs
* Gerado em: 22/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Estado_B"

    public abstract class Estado_B : BaseBD
    {

        public sigla Sigla = new sigla();
        public paisid PaisID = new paisid();
        public valortaxa ValorTaxa = new valortaxa();
        public limitemaximoingressosestado LimiteMaximoIngressosEstado = new limitemaximoingressosestado();
        public possuitaxaprocessamento PossuiTaxaProcessamento = new possuitaxaprocessamento();

        public Estado_B() { }

        // passar o Usuario logado no sistema
        public Estado_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Estado
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEstado WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Sigla.ValorBD = bd.LerString("Sigla");
                    this.PaisID.ValorBD = bd.LerInt("PaisID").ToString();
                    this.ValorTaxa.ValorBD = bd.LerDecimal("ValorTaxa").ToString();
                    this.LimiteMaximoIngressosEstado.ValorBD = bd.LerInt("LimiteMaximoIngressosEstado").ToString();
                    this.PossuiTaxaProcessamento.ValorBD = bd.LerString("PossuiTaxaProcessamento");
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
        /// Preenche todos os atributos de Estado do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEstado WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Sigla.ValorBD = bd.LerString("Sigla");
                    this.PaisID.ValorBD = bd.LerInt("PaisID").ToString();
                    this.ValorTaxa.ValorBD = bd.LerDecimal("ValorTaxa").ToString();
                    this.LimiteMaximoIngressosEstado.ValorBD = bd.LerInt("LimiteMaximoIngressosEstado").ToString();
                    this.PossuiTaxaProcessamento.ValorBD = bd.LerString("PossuiTaxaProcessamento");
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
                sql.Append("INSERT INTO cEstado (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEstado (ID, Versao, Sigla, PaisID, ValorTaxa, LimiteMaximoIngressosEstado, PossuiTaxaProcessamento) ");
                sql.Append("SELECT ID, @V, Sigla, PaisID, ValorTaxa, LimiteMaximoIngressosEstado, PossuiTaxaProcessamento FROM tEstado WHERE ID = @I");
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
        /// Inserir novo(a) Estado
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstado");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstado(ID, Sigla, PaisID, ValorTaxa, LimiteMaximoIngressosEstado, PossuiTaxaProcessamento) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003',@004,'@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Sigla.ValorBD);
                sql.Replace("@002", this.PaisID.ValorBD);
                sql.Replace("@003", this.ValorTaxa.ValorBD);
                sql.Replace("@004", this.LimiteMaximoIngressosEstado.ValorBD);
                sql.Replace("@005", this.PossuiTaxaProcessamento.ValorBD);

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
        /// Inserir novo(a) Estado
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstado");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstado(ID, Sigla, PaisID, ValorTaxa, LimiteMaximoIngressosEstado, PossuiTaxaProcessamento) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003',@004,'@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Sigla.ValorBD);
                sql.Replace("@002", this.PaisID.ValorBD);
                sql.Replace("@003", this.ValorTaxa.ValorBD);
                sql.Replace("@004", this.LimiteMaximoIngressosEstado.ValorBD);
                sql.Replace("@005", this.PossuiTaxaProcessamento.ValorBD);

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
        /// Atualiza Estado
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEstado WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstado SET Sigla = '@001', PaisID = @002, ValorTaxa = '@003', LimiteMaximoIngressosEstado = @004, PossuiTaxaProcessamento = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Sigla.ValorBD);
                sql.Replace("@002", this.PaisID.ValorBD);
                sql.Replace("@003", this.ValorTaxa.ValorBD);
                sql.Replace("@004", this.LimiteMaximoIngressosEstado.ValorBD);
                sql.Replace("@005", this.PossuiTaxaProcessamento.ValorBD);

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
        /// Atualiza Estado
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEstado WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstado SET Sigla = '@001', PaisID = @002, ValorTaxa = '@003', LimiteMaximoIngressosEstado = @004, PossuiTaxaProcessamento = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Sigla.ValorBD);
                sql.Replace("@002", this.PaisID.ValorBD);
                sql.Replace("@003", this.ValorTaxa.ValorBD);
                sql.Replace("@004", this.LimiteMaximoIngressosEstado.ValorBD);
                sql.Replace("@005", this.PossuiTaxaProcessamento.ValorBD);

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
        /// Exclui Estado com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstado WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstado WHERE ID=" + id;

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
        /// Exclui Estado com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstado WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstado WHERE ID=" + id;

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
        /// Exclui Estado
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

            this.Sigla.Limpar();
            this.PaisID.Limpar();
            this.ValorTaxa.Limpar();
            this.LimiteMaximoIngressosEstado.Limpar();
            this.PossuiTaxaProcessamento.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Sigla.Desfazer();
            this.PaisID.Desfazer();
            this.ValorTaxa.Desfazer();
            this.LimiteMaximoIngressosEstado.Desfazer();
            this.PossuiTaxaProcessamento.Desfazer();
        }

        public class sigla : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sigla";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
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

        public class paisid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PaisID";
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

        public class possuitaxaprocessamento : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PossuiTaxaProcessamento";
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

                DataTable tabela = new DataTable("Estado");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));
                tabela.Columns.Add("PaisID", typeof(int));
                tabela.Columns.Add("ValorTaxa", typeof(decimal));
                tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));
                tabela.Columns.Add("PossuiTaxaProcessamento", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EstadoLista_B"

    public abstract class EstadoLista_B : BaseLista
    {

        private bool backup = false;
        protected Estado estado;

        // passar o Usuario logado no sistema
        public EstadoLista_B()
        {
            estado = new Estado();
        }

        // passar o Usuario logado no sistema
        public EstadoLista_B(int usuarioIDLogado)
        {
            estado = new Estado(usuarioIDLogado);
        }

        public Estado Estado
        {
            get { return estado; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Estado especifico
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
                    estado.Ler(id);
                    return estado;
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
                    sql = "SELECT ID FROM tEstado";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstado";

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
                    sql = "SELECT ID FROM tEstado";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstado";

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
                    sql = "SELECT ID FROM xEstado";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEstado";

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
        /// Preenche Estado corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    estado.Ler(id);
                else
                    estado.LerBackup(id);

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

                bool ok = estado.Excluir();
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
        /// Inseri novo(a) Estado na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = estado.Inserir();
                if (ok)
                {
                    lista.Add(estado.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Estado carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Estado");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));
                tabela.Columns.Add("PaisID", typeof(int));
                tabela.Columns.Add("ValorTaxa", typeof(decimal));
                tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));
                tabela.Columns.Add("PossuiTaxaProcessamento", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = estado.Control.ID;
                        linha["Sigla"] = estado.Sigla.Valor;
                        linha["PaisID"] = estado.PaisID.Valor;
                        linha["ValorTaxa"] = estado.ValorTaxa.Valor;
                        linha["LimiteMaximoIngressosEstado"] = estado.LimiteMaximoIngressosEstado.Valor;
                        linha["PossuiTaxaProcessamento"] = estado.PossuiTaxaProcessamento.Valor;
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

                DataTable tabela = new DataTable("RelatorioEstado");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Sigla", typeof(string));
                    tabela.Columns.Add("PaisID", typeof(int));
                    tabela.Columns.Add("ValorTaxa", typeof(decimal));
                    tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));
                    tabela.Columns.Add("PossuiTaxaProcessamento", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Sigla"] = estado.Sigla.Valor;
                        linha["PaisID"] = estado.PaisID.Valor;
                        linha["ValorTaxa"] = estado.ValorTaxa.Valor;
                        linha["LimiteMaximoIngressosEstado"] = estado.LimiteMaximoIngressosEstado.Valor;
                        linha["PossuiTaxaProcessamento"] = estado.PossuiTaxaProcessamento.Valor;
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
                    case "Sigla":
                        sql = "SELECT ID, Sigla FROM tEstado WHERE " + FiltroSQL + " ORDER BY Sigla";
                        break;
                    case "PaisID":
                        sql = "SELECT ID, PaisID FROM tEstado WHERE " + FiltroSQL + " ORDER BY PaisID";
                        break;
                    case "ValorTaxa":
                        sql = "SELECT ID, ValorTaxa FROM tEstado WHERE " + FiltroSQL + " ORDER BY ValorTaxa";
                        break;
                    case "LimiteMaximoIngressosEstado":
                        sql = "SELECT ID, LimiteMaximoIngressosEstado FROM tEstado WHERE " + FiltroSQL + " ORDER BY LimiteMaximoIngressosEstado";
                        break;
                    case "PossuiTaxaProcessamento":
                        sql = "SELECT ID, PossuiTaxaProcessamento FROM tEstado WHERE " + FiltroSQL + " ORDER BY PossuiTaxaProcessamento";
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

    #region "EstadoException"

    [Serializable]
    public class EstadoException : Exception
    {

        public EstadoException() : base() { }

        public EstadoException(string msg) : base(msg) { }

        public EstadoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}