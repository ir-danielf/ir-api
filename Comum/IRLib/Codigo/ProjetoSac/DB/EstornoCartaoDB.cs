/******************************************************
* Arquivo EstornoCartaoDB.cs
* Gerado em: 07/11/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "EstornoCartao_B"

    public abstract class EstornoCartao_B : BaseBD
    {

        public estornoid EstornoID = new estornoid();
        public redeid RedeID = new redeid();
        public numerocartao NumeroCartao = new numerocartao();
        public nsu NSU = new nsu();
        public nome Nome = new nome();
        public estabeleciomentoid EstabeleciomentoID = new estabeleciomentoid();

        public EstornoCartao_B() { }

        // passar o Usuario logado no sistema
        public EstornoCartao_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EstornoCartao
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEstornoCartao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstornoID.ValorBD = bd.LerInt("EstornoID").ToString();
                    this.RedeID.ValorBD = bd.LerInt("RedeID").ToString();
                    this.NumeroCartao.ValorBD = bd.LerString("NumeroCartao");
                    this.NSU.ValorBD = bd.LerString("NSU");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.EstabeleciomentoID.ValorBD = bd.LerInt("EstabeleciomentoID").ToString();
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
        /// Preenche todos os atributos de EstornoCartao do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEstornoCartao WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstornoID.ValorBD = bd.LerInt("EstornoID").ToString();
                    this.RedeID.ValorBD = bd.LerInt("RedeID").ToString();
                    this.NumeroCartao.ValorBD = bd.LerString("NumeroCartao");
                    this.NSU.ValorBD = bd.LerString("NSU");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.EstabeleciomentoID.ValorBD = bd.LerInt("EstabeleciomentoID").ToString();
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
                sql.Append("INSERT INTO cEstornoCartao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEstornoCartao (ID, Versao, EstornoID, RedeID, NumeroCartao, NSU, Nome, EstabeleciomentoID) ");
                sql.Append("SELECT ID, @V, EstornoID, RedeID, NumeroCartao, NSU, Nome, EstabeleciomentoID FROM tEstornoCartao WHERE ID = @I");
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
        /// Inserir novo(a) EstornoCartao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstornoCartao");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstornoCartao(ID, EstornoID, RedeID, NumeroCartao, NSU, Nome, EstabeleciomentoID) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005',@006)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.RedeID.ValorBD);
                sql.Replace("@003", this.NumeroCartao.ValorBD);
                sql.Replace("@004", this.NSU.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.EstabeleciomentoID.ValorBD);

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
        /// Inserir novo(a) EstornoCartao
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstornoCartao");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstornoCartao(ID, EstornoID, RedeID, NumeroCartao, NSU, Nome, EstabeleciomentoID) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005',@006)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.RedeID.ValorBD);
                sql.Replace("@003", this.NumeroCartao.ValorBD);
                sql.Replace("@004", this.NSU.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.EstabeleciomentoID.ValorBD);

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
        /// Atualiza EstornoCartao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEstornoCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstornoCartao SET EstornoID = @001, RedeID = @002, NumeroCartao = '@003', NSU = '@004', Nome = '@005', EstabeleciomentoID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.RedeID.ValorBD);
                sql.Replace("@003", this.NumeroCartao.ValorBD);
                sql.Replace("@004", this.NSU.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.EstabeleciomentoID.ValorBD);

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
        /// Atualiza EstornoCartao
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEstornoCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstornoCartao SET EstornoID = @001, RedeID = @002, NumeroCartao = '@003', NSU = '@004', Nome = '@005', EstabeleciomentoID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.RedeID.ValorBD);
                sql.Replace("@003", this.NumeroCartao.ValorBD);
                sql.Replace("@004", this.NSU.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.EstabeleciomentoID.ValorBD);

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
        /// Exclui EstornoCartao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstornoCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstornoCartao WHERE ID=" + id;

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
        /// Exclui EstornoCartao com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstornoCartao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstornoCartao WHERE ID=" + id;

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
        /// Exclui EstornoCartao
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

            this.EstornoID.Limpar();
            this.RedeID.Limpar();
            this.NumeroCartao.Limpar();
            this.NSU.Limpar();
            this.Nome.Limpar();
            this.EstabeleciomentoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EstornoID.Desfazer();
            this.RedeID.Desfazer();
            this.NumeroCartao.Desfazer();
            this.NSU.Desfazer();
            this.Nome.Desfazer();
            this.EstabeleciomentoID.Desfazer();
        }

        public class estornoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstornoID";
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

        public class redeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "RedeID";
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

        public class numerocartao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroCartao";
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

        public class nsu : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NSU";
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

        public class estabeleciomentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstabeleciomentoID";
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

                DataTable tabela = new DataTable("EstornoCartao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstornoID", typeof(int));
                tabela.Columns.Add("RedeID", typeof(int));
                tabela.Columns.Add("NumeroCartao", typeof(string));
                tabela.Columns.Add("NSU", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EstabeleciomentoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EstornoCartaoLista_B"

    public abstract class EstornoCartaoLista_B : BaseLista
    {

        private bool backup = false;
        protected EstornoCartao estornoCartao;

        // passar o Usuario logado no sistema
        public EstornoCartaoLista_B()
        {
            estornoCartao = new EstornoCartao();
        }

        // passar o Usuario logado no sistema
        public EstornoCartaoLista_B(int usuarioIDLogado)
        {
            estornoCartao = new EstornoCartao(usuarioIDLogado);
        }

        public EstornoCartao EstornoCartao
        {
            get { return estornoCartao; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EstornoCartao especifico
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
                    estornoCartao.Ler(id);
                    return estornoCartao;
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
                    sql = "SELECT ID FROM tEstornoCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstornoCartao";

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
                    sql = "SELECT ID FROM tEstornoCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstornoCartao";

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
                    sql = "SELECT ID FROM xEstornoCartao";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEstornoCartao";

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
        /// Preenche EstornoCartao corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    estornoCartao.Ler(id);
                else
                    estornoCartao.LerBackup(id);

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

                bool ok = estornoCartao.Excluir();
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
        /// Inseri novo(a) EstornoCartao na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = estornoCartao.Inserir();
                if (ok)
                {
                    lista.Add(estornoCartao.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EstornoCartao carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EstornoCartao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstornoID", typeof(int));
                tabela.Columns.Add("RedeID", typeof(int));
                tabela.Columns.Add("NumeroCartao", typeof(string));
                tabela.Columns.Add("NSU", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EstabeleciomentoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = estornoCartao.Control.ID;
                        linha["EstornoID"] = estornoCartao.EstornoID.Valor;
                        linha["RedeID"] = estornoCartao.RedeID.Valor;
                        linha["NumeroCartao"] = estornoCartao.NumeroCartao.Valor;
                        linha["NSU"] = estornoCartao.NSU.Valor;
                        linha["Nome"] = estornoCartao.Nome.Valor;
                        linha["EstabeleciomentoID"] = estornoCartao.EstabeleciomentoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioEstornoCartao");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EstornoID", typeof(int));
                    tabela.Columns.Add("RedeID", typeof(int));
                    tabela.Columns.Add("NumeroCartao", typeof(string));
                    tabela.Columns.Add("NSU", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("EstabeleciomentoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EstornoID"] = estornoCartao.EstornoID.Valor;
                        linha["RedeID"] = estornoCartao.RedeID.Valor;
                        linha["NumeroCartao"] = estornoCartao.NumeroCartao.Valor;
                        linha["NSU"] = estornoCartao.NSU.Valor;
                        linha["Nome"] = estornoCartao.Nome.Valor;
                        linha["EstabeleciomentoID"] = estornoCartao.EstabeleciomentoID.Valor;
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
                    case "EstornoID":
                        sql = "SELECT ID, EstornoID FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY EstornoID";
                        break;
                    case "RedeID":
                        sql = "SELECT ID, RedeID FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY RedeID";
                        break;
                    case "NumeroCartao":
                        sql = "SELECT ID, NumeroCartao FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY NumeroCartao";
                        break;
                    case "NSU":
                        sql = "SELECT ID, NSU FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY NSU";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "EstabeleciomentoID":
                        sql = "SELECT ID, EstabeleciomentoID FROM tEstornoCartao WHERE " + FiltroSQL + " ORDER BY EstabeleciomentoID";
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

    #region "EstornoCartaoException"

    [Serializable]
    public class EstornoCartaoException : Exception
    {

        public EstornoCartaoException() : base() { }

        public EstornoCartaoException(string msg) : base(msg) { }

        public EstornoCartaoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}