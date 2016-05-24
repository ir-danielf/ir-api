/******************************************************
* Arquivo EventoBorderoDB.cs
* Gerado em: 28/09/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "EventoBordero_B"

    public abstract class EventoBordero_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public gestorrazaosocial GestorRazaoSocial = new gestorrazaosocial();
        public gestorcpfcnpj GestorCpfCnpj = new gestorcpfcnpj();
        public gestorendereco GestorEndereco = new gestorendereco();
        public produtorrazaosocial ProdutorRazaoSocial = new produtorrazaosocial();
        public produtorcpfcnpj ProdutorCpfCnpj = new produtorcpfcnpj();
        public produtorendereco ProdutorEndereco = new produtorendereco();

        public EventoBordero_B() { }

        // passar o Usuario logado no sistema
        public EventoBordero_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EventoBordero
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEventoBordero WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.GestorRazaoSocial.ValorBD = bd.LerString("GestorRazaoSocial");
                    this.GestorCpfCnpj.ValorBD = bd.LerString("GestorCpfCnpj");
                    this.GestorEndereco.ValorBD = bd.LerString("GestorEndereco");
                    this.ProdutorRazaoSocial.ValorBD = bd.LerString("ProdutorRazaoSocial");
                    this.ProdutorCpfCnpj.ValorBD = bd.LerString("ProdutorCpfCnpj");
                    this.ProdutorEndereco.ValorBD = bd.LerString("ProdutorEndereco");
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
        /// Preenche todos os atributos de EventoBordero do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEventoBordero WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.GestorRazaoSocial.ValorBD = bd.LerString("GestorRazaoSocial");
                    this.GestorCpfCnpj.ValorBD = bd.LerString("GestorCpfCnpj");
                    this.GestorEndereco.ValorBD = bd.LerString("GestorEndereco");
                    this.ProdutorRazaoSocial.ValorBD = bd.LerString("ProdutorRazaoSocial");
                    this.ProdutorCpfCnpj.ValorBD = bd.LerString("ProdutorCpfCnpj");
                    this.ProdutorEndereco.ValorBD = bd.LerString("ProdutorEndereco");
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
                sql.Append("INSERT INTO cEventoBordero (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEventoBordero (ID, Versao, EventoID, GestorRazaoSocial, GestorCpfCnpj, GestorEndereco, ProdutorRazaoSocial, ProdutorCpfCnpj, ProdutorEndereco) ");
                sql.Append("SELECT ID, @V, EventoID, GestorRazaoSocial, GestorCpfCnpj, GestorEndereco, ProdutorRazaoSocial, ProdutorCpfCnpj, ProdutorEndereco FROM tEventoBordero WHERE ID = @I");
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
        /// Inserir novo(a) EventoBordero
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEventoBordero");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoBordero(ID, EventoID, GestorRazaoSocial, GestorCpfCnpj, GestorEndereco, ProdutorRazaoSocial, ProdutorCpfCnpj, ProdutorEndereco) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.GestorRazaoSocial.ValorBD);
                sql.Replace("@003", this.GestorCpfCnpj.ValorBD);
                sql.Replace("@004", this.GestorEndereco.ValorBD);
                sql.Replace("@005", this.ProdutorRazaoSocial.ValorBD);
                sql.Replace("@006", this.ProdutorCpfCnpj.ValorBD);
                sql.Replace("@007", this.ProdutorEndereco.ValorBD);

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
        /// Inserir novo(a) EventoBordero
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEventoBordero");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEventoBordero(ID, EventoID, GestorRazaoSocial, GestorCpfCnpj, GestorEndereco, ProdutorRazaoSocial, ProdutorCpfCnpj, ProdutorEndereco) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.GestorRazaoSocial.ValorBD);
                sql.Replace("@003", this.GestorCpfCnpj.ValorBD);
                sql.Replace("@004", this.GestorEndereco.ValorBD);
                sql.Replace("@005", this.ProdutorRazaoSocial.ValorBD);
                sql.Replace("@006", this.ProdutorCpfCnpj.ValorBD);
                sql.Replace("@007", this.ProdutorEndereco.ValorBD);

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
        /// Atualiza EventoBordero
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEventoBordero WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEventoBordero SET EventoID = @001, GestorRazaoSocial = '@002', GestorCpfCnpj = '@003', GestorEndereco = '@004', ProdutorRazaoSocial = '@005', ProdutorCpfCnpj = '@006', ProdutorEndereco = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.GestorRazaoSocial.ValorBD);
                sql.Replace("@003", this.GestorCpfCnpj.ValorBD);
                sql.Replace("@004", this.GestorEndereco.ValorBD);
                sql.Replace("@005", this.ProdutorRazaoSocial.ValorBD);
                sql.Replace("@006", this.ProdutorCpfCnpj.ValorBD);
                sql.Replace("@007", this.ProdutorEndereco.ValorBD);

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
        /// Atualiza EventoBordero
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEventoBordero WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEventoBordero SET EventoID = @001, GestorRazaoSocial = '@002', GestorCpfCnpj = '@003', GestorEndereco = '@004', ProdutorRazaoSocial = '@005', ProdutorCpfCnpj = '@006', ProdutorEndereco = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.GestorRazaoSocial.ValorBD);
                sql.Replace("@003", this.GestorCpfCnpj.ValorBD);
                sql.Replace("@004", this.GestorEndereco.ValorBD);
                sql.Replace("@005", this.ProdutorRazaoSocial.ValorBD);
                sql.Replace("@006", this.ProdutorCpfCnpj.ValorBD);
                sql.Replace("@007", this.ProdutorEndereco.ValorBD);

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
        /// Exclui EventoBordero com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEventoBordero WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEventoBordero WHERE ID=" + id;

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
        /// Exclui EventoBordero com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEventoBordero WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEventoBordero WHERE ID=" + id;

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
        /// Exclui EventoBordero
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
            this.GestorRazaoSocial.Limpar();
            this.GestorCpfCnpj.Limpar();
            this.GestorEndereco.Limpar();
            this.ProdutorRazaoSocial.Limpar();
            this.ProdutorCpfCnpj.Limpar();
            this.ProdutorEndereco.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EventoID.Desfazer();
            this.GestorRazaoSocial.Desfazer();
            this.GestorCpfCnpj.Desfazer();
            this.GestorEndereco.Desfazer();
            this.ProdutorRazaoSocial.Desfazer();
            this.ProdutorCpfCnpj.Desfazer();
            this.ProdutorEndereco.Desfazer();
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

        public class gestorrazaosocial : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "GestorRazaoSocial";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class gestorcpfcnpj : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "GestorCpfCnpj";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class gestorendereco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "GestorEndereco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class produtorrazaosocial : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProdutorRazaoSocial";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class produtorcpfcnpj : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProdutorCpfCnpj";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class produtorendereco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProdutorEndereco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

                DataTable tabela = new DataTable("EventoBordero");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("GestorRazaoSocial", typeof(string));
                tabela.Columns.Add("GestorCpfCnpj", typeof(string));
                tabela.Columns.Add("GestorEndereco", typeof(string));
                tabela.Columns.Add("ProdutorRazaoSocial", typeof(string));
                tabela.Columns.Add("ProdutorCpfCnpj", typeof(string));
                tabela.Columns.Add("ProdutorEndereco", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EventoBorderoLista_B"

    public abstract class EventoBorderoLista_B : BaseLista
    {

        private bool backup = false;
        protected EventoBordero eventoBordero;

        // passar o Usuario logado no sistema
        public EventoBorderoLista_B()
        {
            eventoBordero = new EventoBordero();
        }

        // passar o Usuario logado no sistema
        public EventoBorderoLista_B(int usuarioIDLogado)
        {
            eventoBordero = new EventoBordero(usuarioIDLogado);
        }

        public EventoBordero EventoBordero
        {
            get { return eventoBordero; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EventoBordero especifico
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
                    eventoBordero.Ler(id);
                    return eventoBordero;
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
                    sql = "SELECT ID FROM tEventoBordero";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEventoBordero";

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
                    sql = "SELECT ID FROM tEventoBordero";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEventoBordero";

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
                    sql = "SELECT ID FROM xEventoBordero";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEventoBordero";

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
        /// Preenche EventoBordero corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    eventoBordero.Ler(id);
                else
                    eventoBordero.LerBackup(id);

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

                bool ok = eventoBordero.Excluir();
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
        /// Inseri novo(a) EventoBordero na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = eventoBordero.Inserir();
                if (ok)
                {
                    lista.Add(eventoBordero.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EventoBordero carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EventoBordero");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("GestorRazaoSocial", typeof(string));
                tabela.Columns.Add("GestorCpfCnpj", typeof(string));
                tabela.Columns.Add("GestorEndereco", typeof(string));
                tabela.Columns.Add("ProdutorRazaoSocial", typeof(string));
                tabela.Columns.Add("ProdutorCpfCnpj", typeof(string));
                tabela.Columns.Add("ProdutorEndereco", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = eventoBordero.Control.ID;
                        linha["EventoID"] = eventoBordero.EventoID.Valor;
                        linha["GestorRazaoSocial"] = eventoBordero.GestorRazaoSocial.Valor;
                        linha["GestorCpfCnpj"] = eventoBordero.GestorCpfCnpj.Valor;
                        linha["GestorEndereco"] = eventoBordero.GestorEndereco.Valor;
                        linha["ProdutorRazaoSocial"] = eventoBordero.ProdutorRazaoSocial.Valor;
                        linha["ProdutorCpfCnpj"] = eventoBordero.ProdutorCpfCnpj.Valor;
                        linha["ProdutorEndereco"] = eventoBordero.ProdutorEndereco.Valor;
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

                DataTable tabela = new DataTable("RelatorioEventoBordero");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("GestorRazaoSocial", typeof(string));
                    tabela.Columns.Add("GestorCpfCnpj", typeof(string));
                    tabela.Columns.Add("GestorEndereco", typeof(string));
                    tabela.Columns.Add("ProdutorRazaoSocial", typeof(string));
                    tabela.Columns.Add("ProdutorCpfCnpj", typeof(string));
                    tabela.Columns.Add("ProdutorEndereco", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = eventoBordero.EventoID.Valor;
                        linha["GestorRazaoSocial"] = eventoBordero.GestorRazaoSocial.Valor;
                        linha["GestorCpfCnpj"] = eventoBordero.GestorCpfCnpj.Valor;
                        linha["GestorEndereco"] = eventoBordero.GestorEndereco.Valor;
                        linha["ProdutorRazaoSocial"] = eventoBordero.ProdutorRazaoSocial.Valor;
                        linha["ProdutorCpfCnpj"] = eventoBordero.ProdutorCpfCnpj.Valor;
                        linha["ProdutorEndereco"] = eventoBordero.ProdutorEndereco.Valor;
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
                        sql = "SELECT ID, EventoID FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "GestorRazaoSocial":
                        sql = "SELECT ID, GestorRazaoSocial FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY GestorRazaoSocial";
                        break;
                    case "GestorCpfCnpj":
                        sql = "SELECT ID, GestorCpfCnpj FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY GestorCpfCnpj";
                        break;
                    case "GestorEndereco":
                        sql = "SELECT ID, GestorEndereco FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY GestorEndereco";
                        break;
                    case "ProdutorRazaoSocial":
                        sql = "SELECT ID, ProdutorRazaoSocial FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY ProdutorRazaoSocial";
                        break;
                    case "ProdutorCpfCnpj":
                        sql = "SELECT ID, ProdutorCpfCnpj FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY ProdutorCpfCnpj";
                        break;
                    case "ProdutorEndereco":
                        sql = "SELECT ID, ProdutorEndereco FROM tEventoBordero WHERE " + FiltroSQL + " ORDER BY ProdutorEndereco";
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

    #region "EventoBorderoException"

    [Serializable]
    public class EventoBorderoException : Exception
    {

        public EventoBorderoException() : base() { }

        public EventoBorderoException(string msg) : base(msg) { }

        public EventoBorderoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}