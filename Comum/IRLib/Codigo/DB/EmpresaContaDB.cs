/******************************************************
* Arquivo EmpresaContaDB.cs
* Gerado em: 31/08/2009
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "EmpresaConta_B"

    public abstract class EmpresaConta_B : BaseBD
    {

        public empresaid EmpresaID = new empresaid();
        public beneficiario Beneficiario = new beneficiario();
        public banco Banco = new banco();
        public agencia Agencia = new agencia();
        public conta Conta = new conta();
        public cpfcnpj CPFCNPJ = new cpfcnpj();
        public contapadrao ContaPadrao = new contapadrao();

        public EmpresaConta_B() { }

        // passar o Usuario logado no sistema
        public EmpresaConta_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EmpresaConta
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEmpresaConta WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.Beneficiario.ValorBD = bd.LerString("Beneficiario");
                    this.Banco.ValorBD = bd.LerString("Banco");
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.CPFCNPJ.ValorBD = bd.LerString("CPFCNPJ");
                    this.ContaPadrao.ValorBD = bd.LerString("ContaPadrao");
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
        /// Preenche todos os atributos de EmpresaConta do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEmpresaConta WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.Beneficiario.ValorBD = bd.LerString("Beneficiario");
                    this.Banco.ValorBD = bd.LerString("Banco");
                    this.Agencia.ValorBD = bd.LerString("Agencia");
                    this.Conta.ValorBD = bd.LerString("Conta");
                    this.CPFCNPJ.ValorBD = bd.LerString("CPFCNPJ");
                    this.ContaPadrao.ValorBD = bd.LerString("ContaPadrao");
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
                sql.Append("INSERT INTO cEmpresaConta (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEmpresaConta (ID, Versao, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao) ");
                sql.Append("SELECT ID, @V, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao FROM tEmpresaConta WHERE ID = @I");
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
        /// Inserir novo(a) EmpresaConta
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEmpresaConta");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEmpresaConta(ID, EmpresaID, Beneficiario, Banco, Agencia, Conta, CPFCNPJ, ContaPadrao) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Beneficiario.ValorBD);
                sql.Replace("@003", this.Banco.ValorBD);
                sql.Replace("@004", this.Agencia.ValorBD);
                sql.Replace("@005", this.Conta.ValorBD);
                sql.Replace("@006", this.CPFCNPJ.ValorBD);
                sql.Replace("@007", this.ContaPadrao.ValorBD);

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
        /// Atualiza EmpresaConta
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEmpresaConta WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEmpresaConta SET EmpresaID = @001, Beneficiario = '@002', Banco = '@003', Agencia = '@004', Conta = '@005', CPFCNPJ = '@006', ContaPadrao = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Beneficiario.ValorBD);
                sql.Replace("@003", this.Banco.ValorBD);
                sql.Replace("@004", this.Agencia.ValorBD);
                sql.Replace("@005", this.Conta.ValorBD);
                sql.Replace("@006", this.CPFCNPJ.ValorBD);
                sql.Replace("@007", this.ContaPadrao.ValorBD);

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
        /// Exclui EmpresaConta com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEmpresaConta WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEmpresaConta WHERE ID=" + id;

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
        /// Exclui EmpresaConta
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

            this.EmpresaID.Limpar();
            this.Beneficiario.Limpar();
            this.Banco.Limpar();
            this.Agencia.Limpar();
            this.Conta.Limpar();
            this.CPFCNPJ.Limpar();
            this.ContaPadrao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EmpresaID.Desfazer();
            this.Beneficiario.Desfazer();
            this.Banco.Desfazer();
            this.Agencia.Desfazer();
            this.Conta.Desfazer();
            this.CPFCNPJ.Desfazer();
            this.ContaPadrao.Desfazer();
        }

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
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

        public class beneficiario : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Beneficiario";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
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

        public class banco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Banco";
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
                    return 10;
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
                    return 10;
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

        public class cpfcnpj : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFCNPJ";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 15;
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

        public class contapadrao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContaPadrao";
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

                DataTable tabela = new DataTable("EmpresaConta");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("Beneficiario", typeof(string));
                tabela.Columns.Add("Banco", typeof(string));
                tabela.Columns.Add("Agencia", typeof(string));
                tabela.Columns.Add("Conta", typeof(string));
                tabela.Columns.Add("CPFCNPJ", typeof(string));
                tabela.Columns.Add("ContaPadrao", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EmpresaContaLista_B"

    public abstract class EmpresaContaLista_B : BaseLista
    {

        private bool backup = false;
        protected EmpresaConta empresaConta;

        // passar o Usuario logado no sistema
        public EmpresaContaLista_B()
        {
            empresaConta = new EmpresaConta();
        }

        // passar o Usuario logado no sistema
        public EmpresaContaLista_B(int usuarioIDLogado)
        {
            empresaConta = new EmpresaConta(usuarioIDLogado);
        }

        public EmpresaConta EmpresaConta
        {
            get { return empresaConta; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EmpresaConta especifico
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
                    empresaConta.Ler(id);
                    return empresaConta;
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
                    sql = "SELECT ID FROM tEmpresaConta";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEmpresaConta";

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
                    sql = "SELECT ID FROM tEmpresaConta";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEmpresaConta";

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
                    sql = "SELECT ID FROM xEmpresaConta";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEmpresaConta";

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
        /// Preenche EmpresaConta corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    empresaConta.Ler(id);
                else
                    empresaConta.LerBackup(id);

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

                bool ok = empresaConta.Excluir();
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
        /// Inseri novo(a) EmpresaConta na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = empresaConta.Inserir();
                if (ok)
                {
                    lista.Add(empresaConta.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EmpresaConta carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EmpresaConta");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("Beneficiario", typeof(string));
                tabela.Columns.Add("Banco", typeof(string));
                tabela.Columns.Add("Agencia", typeof(string));
                tabela.Columns.Add("Conta", typeof(string));
                tabela.Columns.Add("CPFCNPJ", typeof(string));
                tabela.Columns.Add("ContaPadrao", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = empresaConta.Control.ID;
                        linha["EmpresaID"] = empresaConta.EmpresaID.Valor;
                        linha["Beneficiario"] = empresaConta.Beneficiario.Valor;
                        linha["Banco"] = empresaConta.Banco.Valor;
                        linha["Agencia"] = empresaConta.Agencia.Valor;
                        linha["Conta"] = empresaConta.Conta.Valor;
                        linha["CPFCNPJ"] = empresaConta.CPFCNPJ.Valor;
                        linha["ContaPadrao"] = empresaConta.ContaPadrao.Valor;
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

                DataTable tabela = new DataTable("RelatorioEmpresaConta");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("Beneficiario", typeof(string));
                    tabela.Columns.Add("Banco", typeof(string));
                    tabela.Columns.Add("Agencia", typeof(string));
                    tabela.Columns.Add("Conta", typeof(string));
                    tabela.Columns.Add("CPFCNPJ", typeof(string));
                    tabela.Columns.Add("ContaPadrao", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EmpresaID"] = empresaConta.EmpresaID.Valor;
                        linha["Beneficiario"] = empresaConta.Beneficiario.Valor;
                        linha["Banco"] = empresaConta.Banco.Valor;
                        linha["Agencia"] = empresaConta.Agencia.Valor;
                        linha["Conta"] = empresaConta.Conta.Valor;
                        linha["CPFCNPJ"] = empresaConta.CPFCNPJ.Valor;
                        linha["ContaPadrao"] = empresaConta.ContaPadrao.Valor;
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
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "Beneficiario":
                        sql = "SELECT ID, Beneficiario FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY Beneficiario";
                        break;
                    case "Banco":
                        sql = "SELECT ID, Banco FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY Banco";
                        break;
                    case "Agencia":
                        sql = "SELECT ID, Agencia FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY Agencia";
                        break;
                    case "Conta":
                        sql = "SELECT ID, Conta FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY Conta";
                        break;
                    case "CPFCNPJ":
                        sql = "SELECT ID, CPFCNPJ FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY CPFCNPJ";
                        break;
                    case "ContaPadrao":
                        sql = "SELECT ID, ContaPadrao FROM tEmpresaConta WHERE " + FiltroSQL + " ORDER BY ContaPadrao";
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

    #region "EmpresaContaException"

    [Serializable]
    public class EmpresaContaException : Exception
    {

        public EmpresaContaException() : base() { }

        public EmpresaContaException(string msg) : base(msg) { }

        public EmpresaContaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}