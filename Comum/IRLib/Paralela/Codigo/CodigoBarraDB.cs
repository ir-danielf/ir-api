/******************************************************
* Arquivo CodigoBarraDB.cs
* Gerado em: 11/04/2007
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "CodigoBarra_B"

    public abstract class CodigoBarra_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public eventocodigo EventoCodigo = new eventocodigo();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public apresentacaocodigo ApresentacaoCodigo = new apresentacaocodigo();
        public setorid SetorID = new setorid();
        public setorcodigo SetorCodigo = new setorcodigo();
        public precoid PrecoID = new precoid();
        public precocodigo PrecoCodigo = new precocodigo();
        public ativo Ativo = new ativo();

        public CodigoBarra_B() { }

        // passar o Usuario logado no sistema
        public CodigoBarra_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CodigoBarra
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCodigoBarra WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.EventoCodigo.ValorBD = bd.LerString("EventoCodigo");
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.ApresentacaoCodigo.ValorBD = bd.LerString("ApresentacaoCodigo");
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.SetorCodigo.ValorBD = bd.LerString("SetorCodigo");
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.PrecoCodigo.ValorBD = bd.LerString("PrecoCodigo");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
        /// Preenche todos os atributos de CodigoBarra do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCodigoBarra WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.EventoCodigo.ValorBD = bd.LerString("EventoCodigo");
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.ApresentacaoCodigo.ValorBD = bd.LerString("ApresentacaoCodigo");
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.SetorCodigo.ValorBD = bd.LerString("SetorCodigo");
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.PrecoCodigo.ValorBD = bd.LerString("PrecoCodigo");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
                sql.Append("INSERT INTO cCodigoBarra (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCodigoBarra (ID, Versao, EventoID, EventoCodigo, ApresentacaoID, ApresentacaoCodigo, SetorID, SetorCodigo, PrecoID, PrecoCodigo, Ativo) ");
                sql.Append("SELECT ID, @V, EventoID, EventoCodigo, ApresentacaoID, ApresentacaoCodigo, SetorID, SetorCodigo, PrecoID, PrecoCodigo, Ativo FROM tCodigoBarra WHERE ID = @I");
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
        /// Inserir novo(a) CodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCodigoBarra(EventoID, EventoCodigo, ApresentacaoID, ApresentacaoCodigo, SetorID, SetorCodigo, PrecoID, PrecoCodigo, Ativo) ");
                sql.Append("VALUES (@001,'@002',@003,'@004',@005,'@006',@007,'@008','@009') SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.EventoCodigo.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.ApresentacaoCodigo.ValorBD);
                sql.Replace("@005", this.SetorID.ValorBD);
                sql.Replace("@006", this.SetorCodigo.ValorBD);
                sql.Replace("@007", this.PrecoID.ValorBD);
                sql.Replace("@008", this.PrecoCodigo.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);

                this.Control.Versao = 0;
                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                if (this.Control.ID > 0)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return this.Control.ID > 0;

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
        /// Atualiza CodigoBarra
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCodigoBarra WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCodigoBarra SET EventoID = @001, EventoCodigo = '@002', ApresentacaoID = @003, ApresentacaoCodigo = '@004', SetorID = @005, SetorCodigo = '@006', PrecoID = @007, PrecoCodigo = '@008', Ativo = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.EventoCodigo.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.ApresentacaoCodigo.ValorBD);
                sql.Replace("@005", this.SetorID.ValorBD);
                sql.Replace("@006", this.SetorCodigo.ValorBD);
                sql.Replace("@007", this.PrecoID.ValorBD);
                sql.Replace("@008", this.PrecoCodigo.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);

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
        /// Exclui CodigoBarra com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCodigoBarra WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCodigoBarra WHERE ID=" + id;

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
        /// Exclui CodigoBarra
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
            this.EventoCodigo.Limpar();
            this.ApresentacaoID.Limpar();
            this.ApresentacaoCodigo.Limpar();
            this.SetorID.Limpar();
            this.SetorCodigo.Limpar();
            this.PrecoID.Limpar();
            this.PrecoCodigo.Limpar();
            this.Ativo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EventoID.Desfazer();
            this.EventoCodigo.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.ApresentacaoCodigo.Desfazer();
            this.SetorID.Desfazer();
            this.SetorCodigo.Desfazer();
            this.PrecoID.Desfazer();
            this.PrecoCodigo.Desfazer();
            this.Ativo.Desfazer();
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

        public class eventocodigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoCodigo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 3;
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

        public class apresentacaocodigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoCodigo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 3;
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

        public class setorcodigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SetorCodigo";
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
                return base.Valor;
            }

        }

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class precocodigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoCodigo";
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

        public class ativo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
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

                DataTable tabela = new DataTable("CodigoBarra");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("EventoCodigo", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("ApresentacaoCodigo", typeof(string));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("SetorCodigo", typeof(string));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("PrecoCodigo", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "CodigoBarraLista_B"

    public abstract class CodigoBarraLista_B : BaseLista
    {

        private bool backup = false;
        protected CodigoBarra codigoBarra;

        // passar o Usuario logado no sistema
        public CodigoBarraLista_B()
        {
            codigoBarra = new CodigoBarra();
        }

        // passar o Usuario logado no sistema
        public CodigoBarraLista_B(int usuarioIDLogado)
        {
            codigoBarra = new CodigoBarra(usuarioIDLogado);
        }

        public CodigoBarra CodigoBarra
        {
            get { return codigoBarra; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CodigoBarra especifico
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
                    codigoBarra.Ler(id);
                    return codigoBarra;
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
                    sql = "SELECT ID FROM tCodigoBarra";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCodigoBarra";

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
                    sql = "SELECT ID FROM tCodigoBarra";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCodigoBarra";

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
                    sql = "SELECT ID FROM xCodigoBarra";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCodigoBarra";

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
        /// Preenche CodigoBarra corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    codigoBarra.Ler(id);
                else
                    codigoBarra.LerBackup(id);

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

                bool ok = codigoBarra.Excluir();
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
        /// Inseri novo(a) CodigoBarra na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = codigoBarra.Inserir();
                if (ok)
                {
                    lista.Add(codigoBarra.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CodigoBarra carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CodigoBarra");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("EventoCodigo", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("ApresentacaoCodigo", typeof(string));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("SetorCodigo", typeof(string));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("PrecoCodigo", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = codigoBarra.Control.ID;
                        linha["EventoID"] = codigoBarra.EventoID.Valor;
                        linha["EventoCodigo"] = codigoBarra.EventoCodigo.Valor;
                        linha["ApresentacaoID"] = codigoBarra.ApresentacaoID.Valor;
                        linha["ApresentacaoCodigo"] = codigoBarra.ApresentacaoCodigo.Valor;
                        linha["SetorID"] = codigoBarra.SetorID.Valor;
                        linha["SetorCodigo"] = codigoBarra.SetorCodigo.Valor;
                        linha["PrecoID"] = codigoBarra.PrecoID.Valor;
                        linha["PrecoCodigo"] = codigoBarra.PrecoCodigo.Valor;
                        linha["Ativo"] = codigoBarra.Ativo.Valor;
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

                DataTable tabela = new DataTable("RelatorioCodigoBarra");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("EventoCodigo", typeof(string));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("ApresentacaoCodigo", typeof(string));
                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("SetorCodigo", typeof(string));
                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("PrecoCodigo", typeof(string));
                    tabela.Columns.Add("Ativo", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = codigoBarra.EventoID.Valor;
                        linha["EventoCodigo"] = codigoBarra.EventoCodigo.Valor;
                        linha["ApresentacaoID"] = codigoBarra.ApresentacaoID.Valor;
                        linha["ApresentacaoCodigo"] = codigoBarra.ApresentacaoCodigo.Valor;
                        linha["SetorID"] = codigoBarra.SetorID.Valor;
                        linha["SetorCodigo"] = codigoBarra.SetorCodigo.Valor;
                        linha["PrecoID"] = codigoBarra.PrecoID.Valor;
                        linha["PrecoCodigo"] = codigoBarra.PrecoCodigo.Valor;
                        linha["Ativo"] = codigoBarra.Ativo.Valor;
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
                        sql = "SELECT ID, EventoID FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "EventoCodigo":
                        sql = "SELECT ID, EventoCodigo FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY EventoCodigo";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "ApresentacaoCodigo":
                        sql = "SELECT ID, ApresentacaoCodigo FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY ApresentacaoCodigo";
                        break;
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "SetorCodigo":
                        sql = "SELECT ID, SetorCodigo FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY SetorCodigo";
                        break;
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "PrecoCodigo":
                        sql = "SELECT ID, PrecoCodigo FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY PrecoCodigo";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tCodigoBarra WHERE " + FiltroSQL + " ORDER BY Ativo";
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

    #region "CodigoBarraException"

    [Serializable]
    public class CodigoBarraException : Exception
    {

        public CodigoBarraException() : base() { }

        public CodigoBarraException(string msg) : base(msg) { }

        public CodigoBarraException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}