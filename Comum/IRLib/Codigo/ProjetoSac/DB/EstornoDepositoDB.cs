/******************************************************
* Arquivo EstornoDepositoDB.cs
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

    #region "EstornoDeposito_B"

    public abstract class EstornoDeposito_B : BaseBD
    {

        public estornoid EstornoID = new estornoid();
        public bancoid BancoID = new bancoid();
        public agencia Agencia = new agencia();
        public conta Conta = new conta();
        public nome Nome = new nome();
        public cpf CPF = new cpf();
        public depositocontacorrente DepositoContaCorrente = new depositocontacorrente();

        public EstornoDeposito_B() { }

        // passar o Usuario logado no sistema
        public EstornoDeposito_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EstornoDeposito
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEstornoDeposito WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstornoID.ValorBD = bd.LerInt("EstornoID").ToString();
                    this.BancoID.ValorBD = bd.LerInt("BancoID").ToString();
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.DepositoContaCorrente.ValorBD = bd.LerString("DepositoContaCorrente");
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
        /// Preenche todos os atributos de EstornoDeposito do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEstornoDeposito WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstornoID.ValorBD = bd.LerInt("EstornoID").ToString();
                    this.BancoID.ValorBD = bd.LerInt("BancoID").ToString();
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.DepositoContaCorrente.ValorBD = bd.LerString("DepositoContaCorrente");
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
                sql.Append("INSERT INTO cEstornoDeposito (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEstornoDeposito (ID, Versao, EstornoID, BancoID, Agencia, Conta, Nome, CPF, DepositoContaCorrente) ");
                sql.Append("SELECT ID, @V, EstornoID, BancoID, Agencia, Conta, Nome, CPF, DepositoContaCorrente FROM tEstornoDeposito WHERE ID = @I");
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
        /// Inserir novo(a) EstornoDeposito
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {           
            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstornoDeposito");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstornoDeposito(ID, EstornoID, BancoID, Agencia, Conta, Nome, CPF, DepositoContaCorrente) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.BancoID.ValorBD);
                sql.Replace("@003", this.Agencia.ValorBD);
                sql.Replace("@004", this.Conta.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.CPF.ValorBD);
                sql.Replace("@007", this.DepositoContaCorrente.ValorBD);

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
        /// Inserir novo(a) EstornoDeposito
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstornoDeposito");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstornoDeposito(ID, EstornoID, BancoID, Agencia, Conta, Nome, CPF, DepositoContaCorrente) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.BancoID.ValorBD);
                sql.Replace("@003", this.Agencia.ValorBD);
                sql.Replace("@004", this.Conta.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.CPF.ValorBD);
                sql.Replace("@007", this.DepositoContaCorrente.ValorBD);

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
        /// Atualiza EstornoDeposito
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEstornoDeposito WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstornoDeposito SET EstornoID = @001, BancoID = @002, Agencia = '@003', Conta = '@004', Nome = '@005', CPF = '@006', DepositoContaCorrente = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.BancoID.ValorBD);
                sql.Replace("@003", this.Agencia.ValorBD);
                sql.Replace("@004", this.Conta.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.CPF.ValorBD);
                sql.Replace("@007", this.DepositoContaCorrente.ValorBD);

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
        /// Atualiza EstornoDeposito
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEstornoDeposito WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstornoDeposito SET EstornoID = @001, BancoID = @002, Agencia = '@003', Conta = '@004', Nome = '@005', CPF = '@006', DepositoContaCorrente = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstornoID.ValorBD);
                sql.Replace("@002", this.BancoID.ValorBD);
                sql.Replace("@003", this.Agencia.ValorBD);
                sql.Replace("@004", this.Conta.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.CPF.ValorBD);
                sql.Replace("@007", this.DepositoContaCorrente.ValorBD);

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
        /// Exclui EstornoDeposito com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstornoDeposito WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstornoDeposito WHERE ID=" + id;

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
        /// Exclui EstornoDeposito com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstornoDeposito WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstornoDeposito WHERE ID=" + id;

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
        /// Exclui EstornoDeposito
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
            this.BancoID.Limpar();
            this.Agencia.Limpar();
            this.Conta.Limpar();
            this.Nome.Limpar();
            this.CPF.Limpar();
            this.DepositoContaCorrente.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EstornoID.Desfazer();
            this.BancoID.Desfazer();
            this.Agencia.Desfazer();
            this.Conta.Desfazer();
            this.Nome.Desfazer();
            this.CPF.Desfazer();
            this.DepositoContaCorrente.Desfazer();
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

        public class bancoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BancoID";
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

        public class agencia : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Agencia";
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

        public class conta : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Conta";
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

        public class cpf : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPF";
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

        public class depositocontacorrente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DepositoContaCorrente";
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

                DataTable tabela = new DataTable("EstornoDeposito");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstornoID", typeof(int));
                tabela.Columns.Add("BancoID", typeof(int));
                tabela.Columns.Add("Agencia", typeof(string));
                tabela.Columns.Add("Conta", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("DepositoContaCorrente", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EstornoDepositoLista_B"

    public abstract class EstornoDepositoLista_B : BaseLista
    {

        private bool backup = false;
        protected EstornoDeposito estornoDeposito;

        // passar o Usuario logado no sistema
        public EstornoDepositoLista_B()
        {
            estornoDeposito = new EstornoDeposito();
        }

        // passar o Usuario logado no sistema
        public EstornoDepositoLista_B(int usuarioIDLogado)
        {
            estornoDeposito = new EstornoDeposito(usuarioIDLogado);
        }

        public EstornoDeposito EstornoDeposito
        {
            get { return estornoDeposito; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EstornoDeposito especifico
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
                    estornoDeposito.Ler(id);
                    return estornoDeposito;
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
                    sql = "SELECT ID FROM tEstornoDeposito";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstornoDeposito";

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
                    sql = "SELECT ID FROM tEstornoDeposito";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstornoDeposito";

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
                    sql = "SELECT ID FROM xEstornoDeposito";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEstornoDeposito";

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
        /// Preenche EstornoDeposito corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    estornoDeposito.Ler(id);
                else
                    estornoDeposito.LerBackup(id);

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

                bool ok = estornoDeposito.Excluir();
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
        /// Inseri novo(a) EstornoDeposito na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = estornoDeposito.Inserir();
                if (ok)
                {
                    lista.Add(estornoDeposito.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EstornoDeposito carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EstornoDeposito");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstornoID", typeof(int));
                tabela.Columns.Add("BancoID", typeof(int));
                tabela.Columns.Add("Agencia", typeof(string));
                tabela.Columns.Add("Conta", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("DepositoContaCorrente", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = estornoDeposito.Control.ID;
                        linha["EstornoID"] = estornoDeposito.EstornoID.Valor;
                        linha["BancoID"] = estornoDeposito.BancoID.Valor;
                        linha["Agencia"] = estornoDeposito.Agencia.Valor;
                        linha["Conta"] = estornoDeposito.Conta.Valor;
                        linha["Nome"] = estornoDeposito.Nome.Valor;
                        linha["CPF"] = estornoDeposito.CPF.Valor;
                        linha["DepositoContaCorrente"] = estornoDeposito.DepositoContaCorrente.Valor;
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

                DataTable tabela = new DataTable("RelatorioEstornoDeposito");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EstornoID", typeof(int));
                    tabela.Columns.Add("BancoID", typeof(int));
                    tabela.Columns.Add("Agencia", typeof(string));
                    tabela.Columns.Add("Conta", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));
                    tabela.Columns.Add("DepositoContaCorrente", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EstornoID"] = estornoDeposito.EstornoID.Valor;
                        linha["BancoID"] = estornoDeposito.BancoID.Valor;
                        linha["Agencia"] = estornoDeposito.Agencia.Valor;
                        linha["Conta"] = estornoDeposito.Conta.Valor;
                        linha["Nome"] = estornoDeposito.Nome.Valor;
                        linha["CPF"] = estornoDeposito.CPF.Valor;
                        linha["DepositoContaCorrente"] = estornoDeposito.DepositoContaCorrente.Valor;
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
                        sql = "SELECT ID, EstornoID FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY EstornoID";
                        break;
                    case "BancoID":
                        sql = "SELECT ID, BancoID FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY BancoID";
                        break;
                    case "Agencia":
                        sql = "SELECT ID, Agencia FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY Agencia";
                        break;
                    case "Conta":
                        sql = "SELECT ID, Conta FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY Conta";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "DepositoContaCorrente":
                        sql = "SELECT ID, DepositoContaCorrente FROM tEstornoDeposito WHERE " + FiltroSQL + " ORDER BY DepositoContaCorrente";
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

    #region "EstornoDepositoException"

    [Serializable]
    public class EstornoDepositoException : Exception
    {

        public EstornoDepositoException() : base() { }

        public EstornoDepositoException(string msg) : base(msg) { }

        public EstornoDepositoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}