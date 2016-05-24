/******************************************************
* Arquivo CEPDB.cs
* Gerado em: 14/11/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "CEP_B"

    public abstract class CEP_B : BaseBD
    {

        public estadoid EstadoID = new estadoid();
        public cidadeid CidadeID = new cidadeid();
        public cep Cep = new cep();
        public estadosigla EstadoSigla = new estadosigla();
        public cidadenome CidadeNome = new cidadenome();
        public logradouro Logradouro = new logradouro();
        public endereco Endereco = new endereco();
        public bairro Bairro = new bairro();

        public CEP_B() { }

        // passar o Usuario logado no sistema
        public CEP_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CEP
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCEP WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstadoID.ValorBD = bd.LerInt("EstadoID").ToString();
                    this.CidadeID.ValorBD = bd.LerInt("CidadeID").ToString();
                    this.Cep.ValorBD = bd.LerString("Cep");
                    this.EstadoSigla.ValorBD = bd.LerString("EstadoSigla");
                    this.CidadeNome.ValorBD = bd.LerString("CidadeNome");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
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
        /// Preenche todos os atributos de CEP do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCEP WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstadoID.ValorBD = bd.LerInt("EstadoID").ToString();
                    this.CidadeID.ValorBD = bd.LerInt("CidadeID").ToString();
                    this.Cep.ValorBD = bd.LerString("Cep");
                    this.EstadoSigla.ValorBD = bd.LerString("EstadoSigla");
                    this.CidadeNome.ValorBD = bd.LerString("CidadeNome");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
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
                sql.Append("INSERT INTO cCEP (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCEP (ID, Versao, EstadoID, CidadeID, Cep, EstadoSigla, CidadeNome, Logradouro, Endereco, Bairro) ");
                sql.Append("SELECT ID, @V, EstadoID, CidadeID, Cep, EstadoSigla, CidadeNome, Logradouro, Endereco, Bairro FROM tCEP WHERE ID = @I");
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
        /// Inserir novo(a) CEP
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCEP");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCEP(ID, EstadoID, CidadeID, Cep, EstadoSigla, CidadeNome, Logradouro, Endereco, Bairro) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.CidadeID.ValorBD);
                sql.Replace("@003", this.Cep.ValorBD);
                sql.Replace("@004", this.EstadoSigla.ValorBD);
                sql.Replace("@005", this.CidadeNome.ValorBD);
                sql.Replace("@006", this.Logradouro.ValorBD);
                sql.Replace("@007", this.Endereco.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);

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
        /// Atualiza CEP
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCEP WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCEP SET EstadoID = @001, CidadeID = @002, Cep = '@003', EstadoSigla = '@004', CidadeNome = '@005', Logradouro = '@006', Endereco = '@007', Bairro = '@008' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstadoID.ValorBD);
                sql.Replace("@002", this.CidadeID.ValorBD);
                sql.Replace("@003", this.Cep.ValorBD);
                sql.Replace("@004", this.EstadoSigla.ValorBD);
                sql.Replace("@005", this.CidadeNome.ValorBD);
                sql.Replace("@006", this.Logradouro.ValorBD);
                sql.Replace("@007", this.Endereco.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);

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
        /// Exclui CEP com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCEP WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCEP WHERE ID=" + id;

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
        /// Exclui CEP
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
            this.CidadeID.Limpar();
            this.Cep.Limpar();
            this.EstadoSigla.Limpar();
            this.CidadeNome.Limpar();
            this.Logradouro.Limpar();
            this.Endereco.Limpar();
            this.Bairro.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EstadoID.Desfazer();
            this.CidadeID.Desfazer();
            this.Cep.Desfazer();
            this.EstadoSigla.Desfazer();
            this.CidadeNome.Desfazer();
            this.Logradouro.Desfazer();
            this.Endereco.Desfazer();
            this.Bairro.Desfazer();
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

        public class cidadeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CidadeID";
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

        public class cep : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cep";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 9;
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

        public class estadosigla : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstadoSigla";
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

        public class cidadenome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CidadeNome";
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

        public class logradouro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Logradouro";
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

        public class endereco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Endereco";
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

        public class bairro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Bairro";
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

                DataTable tabela = new DataTable("CEP");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstadoID", typeof(int));
                tabela.Columns.Add("CidadeID", typeof(int));
                tabela.Columns.Add("Cep", typeof(string));
                tabela.Columns.Add("EstadoSigla", typeof(string));
                tabela.Columns.Add("CidadeNome", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "CEPLista_B"

    public abstract class CEPLista_B : BaseLista
    {

        private bool backup = false;
        protected CEP cEP;

        // passar o Usuario logado no sistema
        public CEPLista_B()
        {
            cEP = new CEP();
        }

        // passar o Usuario logado no sistema
        public CEPLista_B(int usuarioIDLogado)
        {
            cEP = new CEP(usuarioIDLogado);
        }

        public CEP CEP
        {
            get { return cEP; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CEP especifico
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
                    cEP.Ler(id);
                    return cEP;
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
                    sql = "SELECT ID FROM tCEP";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCEP";

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
                    sql = "SELECT ID FROM tCEP";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCEP";

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
                    sql = "SELECT ID FROM xCEP";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCEP";

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
        /// Preenche CEP corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    cEP.Ler(id);
                else
                    cEP.LerBackup(id);

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

                bool ok = cEP.Excluir();
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
        /// Inseri novo(a) CEP na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = cEP.Inserir();
                if (ok)
                {
                    lista.Add(cEP.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CEP carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CEP");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstadoID", typeof(int));
                tabela.Columns.Add("CidadeID", typeof(int));
                tabela.Columns.Add("Cep", typeof(string));
                tabela.Columns.Add("EstadoSigla", typeof(string));
                tabela.Columns.Add("CidadeNome", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cEP.Control.ID;
                        linha["EstadoID"] = cEP.EstadoID.Valor;
                        linha["CidadeID"] = cEP.CidadeID.Valor;
                        linha["Cep"] = cEP.Cep.Valor;
                        linha["EstadoSigla"] = cEP.EstadoSigla.Valor;
                        linha["CidadeNome"] = cEP.CidadeNome.Valor;
                        linha["Logradouro"] = cEP.Logradouro.Valor;
                        linha["Endereco"] = cEP.Endereco.Valor;
                        linha["Bairro"] = cEP.Bairro.Valor;
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

                DataTable tabela = new DataTable("RelatorioCEP");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EstadoID", typeof(int));
                    tabela.Columns.Add("CidadeID", typeof(int));
                    tabela.Columns.Add("Cep", typeof(string));
                    tabela.Columns.Add("EstadoSigla", typeof(string));
                    tabela.Columns.Add("CidadeNome", typeof(string));
                    tabela.Columns.Add("Logradouro", typeof(string));
                    tabela.Columns.Add("Endereco", typeof(string));
                    tabela.Columns.Add("Bairro", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EstadoID"] = cEP.EstadoID.Valor;
                        linha["CidadeID"] = cEP.CidadeID.Valor;
                        linha["Cep"] = cEP.Cep.Valor;
                        linha["EstadoSigla"] = cEP.EstadoSigla.Valor;
                        linha["CidadeNome"] = cEP.CidadeNome.Valor;
                        linha["Logradouro"] = cEP.Logradouro.Valor;
                        linha["Endereco"] = cEP.Endereco.Valor;
                        linha["Bairro"] = cEP.Bairro.Valor;
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
                        sql = "SELECT ID, EstadoID FROM tCEP WHERE " + FiltroSQL + " ORDER BY EstadoID";
                        break;
                    case "CidadeID":
                        sql = "SELECT ID, CidadeID FROM tCEP WHERE " + FiltroSQL + " ORDER BY CidadeID";
                        break;
                    case "Cep":
                        sql = "SELECT ID, Cep FROM tCEP WHERE " + FiltroSQL + " ORDER BY Cep";
                        break;
                    case "EstadoSigla":
                        sql = "SELECT ID, EstadoSigla FROM tCEP WHERE " + FiltroSQL + " ORDER BY EstadoSigla";
                        break;
                    case "CidadeNome":
                        sql = "SELECT ID, CidadeNome FROM tCEP WHERE " + FiltroSQL + " ORDER BY CidadeNome";
                        break;
                    case "Logradouro":
                        sql = "SELECT ID, Logradouro FROM tCEP WHERE " + FiltroSQL + " ORDER BY Logradouro";
                        break;
                    case "Endereco":
                        sql = "SELECT ID, Endereco FROM tCEP WHERE " + FiltroSQL + " ORDER BY Endereco";
                        break;
                    case "Bairro":
                        sql = "SELECT ID, Bairro FROM tCEP WHERE " + FiltroSQL + " ORDER BY Bairro";
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

    #region "CEPException"

    [Serializable]
    public class CEPException : Exception
    {

        public CEPException() : base() { }

        public CEPException(string msg) : base(msg) { }

        public CEPException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}