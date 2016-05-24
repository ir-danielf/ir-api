/******************************************************
* Arquivo GerenciamentoIngressosDB.cs
* Gerado em: 27/08/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "GerenciamentoIngressos_B"

    public abstract class GerenciamentoIngressos_B : BaseBD
    {

        public apresentacaosetorid ApresentacaoSetorID = new apresentacaosetorid();
        public precotipoid PrecoTipoID = new precotipoid();
        public disponivel Disponivel = new disponivel();
        public horario Horario = new horario();
        public label Label = new label();

        public GerenciamentoIngressos_B() { }

        // passar o Usuario logado no sistema
        public GerenciamentoIngressos_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de GerenciamentoIngressos
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tGerenciamentoIngressos WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                    this.Disponivel.ValorBD = bd.LerInt("Disponivel").ToString();
                    this.Horario.ValorBD = bd.LerString("Horario");
                    this.Label.ValorBD = bd.LerString("Label");
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
        /// Preenche todos os atributos de GerenciamentoIngressos do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xGerenciamentoIngressos WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                    this.Disponivel.ValorBD = bd.LerInt("Disponivel").ToString();
                    this.Horario.ValorBD = bd.LerString("Horario");
                    this.Label.ValorBD = bd.LerString("Label");
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
                sql.Append("INSERT INTO cGerenciamentoIngressos (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xGerenciamentoIngressos (ID, Versao, ApresentacaoSetorID, PrecoTipoID, Disponivel, Horario, Label) ");
                sql.Append("SELECT ID, @V, ApresentacaoSetorID, PrecoTipoID, Disponivel, Horario, Label FROM tGerenciamentoIngressos WHERE ID = @I");
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
        /// Inserir novo(a) GerenciamentoIngressos
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cGerenciamentoIngressos");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tGerenciamentoIngressos(ID, ApresentacaoSetorID, PrecoTipoID, Disponivel, Horario, Label) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoTipoID.ValorBD);
                sql.Replace("@003", this.Disponivel.ValorBD);
                sql.Replace("@004", this.Horario.ValorBD);
                sql.Replace("@005", this.Label.ValorBD);

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
        /// Inserir novo(a) GerenciamentoIngressos
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cGerenciamentoIngressos");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tGerenciamentoIngressos(ID, ApresentacaoSetorID, PrecoTipoID, Disponivel, Horario, Label) ");
                sql.Append("VALUES (@ID,@001,@002,@003,'@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoTipoID.ValorBD);
                sql.Replace("@003", this.Disponivel.ValorBD);
                sql.Replace("@004", this.Horario.ValorBD);
                sql.Replace("@005", this.Label.ValorBD);

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
        /// Atualiza GerenciamentoIngressos
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cGerenciamentoIngressos WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tGerenciamentoIngressos SET ApresentacaoSetorID = @001, PrecoTipoID = @002, Disponivel = @003, Horario = '@004', Label = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoTipoID.ValorBD);
                sql.Replace("@003", this.Disponivel.ValorBD);
                sql.Replace("@004", this.Horario.ValorBD);
                sql.Replace("@005", this.Label.ValorBD);

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
        /// Atualiza GerenciamentoIngressos
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cGerenciamentoIngressos WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tGerenciamentoIngressos SET ApresentacaoSetorID = @001, PrecoTipoID = @002, Disponivel = @003, Horario = '@004', Label = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@002", this.PrecoTipoID.ValorBD);
                sql.Replace("@003", this.Disponivel.ValorBD);
                sql.Replace("@004", this.Horario.ValorBD);
                sql.Replace("@005", this.Label.ValorBD);

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
        /// Exclui GerenciamentoIngressos com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cGerenciamentoIngressos WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tGerenciamentoIngressos WHERE ID=" + id;

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
        /// Exclui GerenciamentoIngressos com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cGerenciamentoIngressos WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tGerenciamentoIngressos WHERE ID=" + id;

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
        /// Exclui GerenciamentoIngressos
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
            this.PrecoTipoID.Limpar();
            this.Disponivel.Limpar();
            this.Horario.Limpar();
            this.Label.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.ApresentacaoSetorID.Desfazer();
            this.PrecoTipoID.Desfazer();
            this.Disponivel.Desfazer();
            this.Horario.Desfazer();
            this.Label.Desfazer();
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

        public class precotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoTipoID";
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

        public class disponivel : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Disponivel";
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

        public class horario : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Horario";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4;
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

        public class label : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Label";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
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

                DataTable tabela = new DataTable("GerenciamentoIngressos");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Disponivel", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                tabela.Columns.Add("Label", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "GerenciamentoIngressosLista_B"

    public abstract class GerenciamentoIngressosLista_B : BaseLista
    {

        private bool backup = false;
        protected GerenciamentoIngressos gerenciamentoIngressos;

        // passar o Usuario logado no sistema
        public GerenciamentoIngressosLista_B()
        {
            gerenciamentoIngressos = new GerenciamentoIngressos();
        }

        // passar o Usuario logado no sistema
        public GerenciamentoIngressosLista_B(int usuarioIDLogado)
        {
            gerenciamentoIngressos = new GerenciamentoIngressos(usuarioIDLogado);
        }

        public GerenciamentoIngressos GerenciamentoIngressos
        {
            get { return gerenciamentoIngressos; }
        }

        /// <summary>
        /// Retorna um IBaseBD de GerenciamentoIngressos especifico
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
                    gerenciamentoIngressos.Ler(id);
                    return gerenciamentoIngressos;
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
                    sql = "SELECT ID FROM tGerenciamentoIngressos";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tGerenciamentoIngressos";

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
                    sql = "SELECT ID FROM tGerenciamentoIngressos";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tGerenciamentoIngressos";

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
                    sql = "SELECT ID FROM xGerenciamentoIngressos";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xGerenciamentoIngressos";

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
        /// Preenche GerenciamentoIngressos corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    gerenciamentoIngressos.Ler(id);
                else
                    gerenciamentoIngressos.LerBackup(id);

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

                bool ok = gerenciamentoIngressos.Excluir();
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
        /// Inseri novo(a) GerenciamentoIngressos na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = gerenciamentoIngressos.Inserir();
                if (ok)
                {
                    lista.Add(gerenciamentoIngressos.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de GerenciamentoIngressos carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("GerenciamentoIngressos");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Disponivel", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                tabela.Columns.Add("Label", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = gerenciamentoIngressos.Control.ID;
                        linha["ApresentacaoSetorID"] = gerenciamentoIngressos.ApresentacaoSetorID.Valor;
                        linha["PrecoTipoID"] = gerenciamentoIngressos.PrecoTipoID.Valor;
                        linha["Disponivel"] = gerenciamentoIngressos.Disponivel.Valor;
                        linha["Horario"] = gerenciamentoIngressos.Horario.Valor;
                        linha["Label"] = gerenciamentoIngressos.Label.Valor;
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

                DataTable tabela = new DataTable("RelatorioGerenciamentoIngressos");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                    tabela.Columns.Add("PrecoTipoID", typeof(int));
                    tabela.Columns.Add("Disponivel", typeof(int));
                    tabela.Columns.Add("Horario", typeof(string));
                    tabela.Columns.Add("Label", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ApresentacaoSetorID"] = gerenciamentoIngressos.ApresentacaoSetorID.Valor;
                        linha["PrecoTipoID"] = gerenciamentoIngressos.PrecoTipoID.Valor;
                        linha["Disponivel"] = gerenciamentoIngressos.Disponivel.Valor;
                        linha["Horario"] = gerenciamentoIngressos.Horario.Valor;
                        linha["Label"] = gerenciamentoIngressos.Label.Valor;
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
                        sql = "SELECT ID, ApresentacaoSetorID FROM tGerenciamentoIngressos WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    case "PrecoTipoID":
                        sql = "SELECT ID, PrecoTipoID FROM tGerenciamentoIngressos WHERE " + FiltroSQL + " ORDER BY PrecoTipoID";
                        break;
                    case "Disponivel":
                        sql = "SELECT ID, Disponivel FROM tGerenciamentoIngressos WHERE " + FiltroSQL + " ORDER BY Disponivel";
                        break;
                    case "Horario":
                        sql = "SELECT ID, Horario FROM tGerenciamentoIngressos WHERE " + FiltroSQL + " ORDER BY Horario";
                        break;
                    case "Label":
                        sql = "SELECT ID, Label FROM tGerenciamentoIngressos WHERE " + FiltroSQL + " ORDER BY Label";
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

    #region "GerenciamentoIngressosException"

    [Serializable]
    public class GerenciamentoIngressosException : Exception
    {

        public GerenciamentoIngressosException() : base() { }

        public GerenciamentoIngressosException(string msg) : base(msg) { }

        public GerenciamentoIngressosException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}