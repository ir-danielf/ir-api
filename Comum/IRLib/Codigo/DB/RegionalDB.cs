/******************************************************
* Arquivo RegionalDB.cs
* Gerado em: 10/05/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Regional_B"

    public abstract class Regional_B : BaseBD
    {

        public nome Nome = new nome();
        public cnpj CNPJ = new cnpj();
        public cep CEP = new cep();
        public logradouro Logradouro = new logradouro();
        public numero Numero = new numero();
        public complemento Complemento = new complemento();
        public bairro Bairro = new bairro();
        public cidade Cidade = new cidade();
        public estados Estados = new estados();
        public pais Pais = new pais();
        public dddtelefone DDDTelefone = new dddtelefone();
        public telefone Telefone = new telefone();
        public emailalertaretirada EmailAlertaRetirada = new emailalertaretirada();

        public Regional_B() { }

        // passar o Usuario logado no sistema
        public Regional_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Regional
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tRegional WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CNPJ.ValorBD = bd.LerString("CNPJ");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Numero.ValorBD = bd.LerInt("Numero").ToString();
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estados.ValorBD = bd.LerString("Estados");
                    this.Pais.ValorBD = bd.LerString("Pais");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.EmailAlertaRetirada.ValorBD = bd.LerString("EmailAlertaRetirada");
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
        /// Preenche todos os atributos de Regional do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xRegional WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CNPJ.ValorBD = bd.LerString("CNPJ");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Numero.ValorBD = bd.LerInt("Numero").ToString();
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estados.ValorBD = bd.LerString("Estados");
                    this.Pais.ValorBD = bd.LerString("Pais");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.EmailAlertaRetirada.ValorBD = bd.LerString("EmailAlertaRetirada");
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
                sql.Append("INSERT INTO cRegional (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xRegional (ID, Versao, Nome, CNPJ, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, Estados, Pais, DDDTelefone, Telefone, EmailAlertaRetirada) ");
                sql.Append("SELECT ID, @V, Nome, CNPJ, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, Estados, Pais, DDDTelefone, Telefone, EmailAlertaRetirada FROM tRegional WHERE ID = @I");
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
        /// Inserir novo(a) Regional
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cRegional");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tRegional(ID, Nome, CNPJ, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, Estados, Pais, DDDTelefone, Telefone, EmailAlertaRetirada) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004',@005,'@006','@007','@008','@009','@010','@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CNPJ.ValorBD);
                sql.Replace("@003", this.CEP.ValorBD);
                sql.Replace("@004", this.Logradouro.ValorBD);
                sql.Replace("@005", this.Numero.ValorBD);
                sql.Replace("@006", this.Complemento.ValorBD);
                sql.Replace("@007", this.Bairro.ValorBD);
                sql.Replace("@008", this.Cidade.ValorBD);
                sql.Replace("@009", this.Estados.ValorBD);
                sql.Replace("@010", this.Pais.ValorBD);
                sql.Replace("@011", this.DDDTelefone.ValorBD);
                sql.Replace("@012", this.Telefone.ValorBD);
                sql.Replace("@013", this.EmailAlertaRetirada.ValorBD);

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
        /// Inserir novo(a) Regional
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cRegional");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tRegional(ID, Nome, CNPJ, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, Estados, Pais, DDDTelefone, Telefone, EmailAlertaRetirada) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004',@005,'@006','@007','@008','@009','@010','@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CNPJ.ValorBD);
                sql.Replace("@003", this.CEP.ValorBD);
                sql.Replace("@004", this.Logradouro.ValorBD);
                sql.Replace("@005", this.Numero.ValorBD);
                sql.Replace("@006", this.Complemento.ValorBD);
                sql.Replace("@007", this.Bairro.ValorBD);
                sql.Replace("@008", this.Cidade.ValorBD);
                sql.Replace("@009", this.Estados.ValorBD);
                sql.Replace("@010", this.Pais.ValorBD);
                sql.Replace("@011", this.DDDTelefone.ValorBD);
                sql.Replace("@012", this.Telefone.ValorBD);
                sql.Replace("@013", this.EmailAlertaRetirada.ValorBD);

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
        /// Atualiza Regional
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cRegional WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tRegional SET Nome = '@001', CNPJ = '@002', CEP = '@003', Logradouro = '@004', Numero = @005, Complemento = '@006', Bairro = '@007', Cidade = '@008', Estados = '@009', Pais = '@010', DDDTelefone = '@011', Telefone = '@012', EmailAlertaRetirada = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CNPJ.ValorBD);
                sql.Replace("@003", this.CEP.ValorBD);
                sql.Replace("@004", this.Logradouro.ValorBD);
                sql.Replace("@005", this.Numero.ValorBD);
                sql.Replace("@006", this.Complemento.ValorBD);
                sql.Replace("@007", this.Bairro.ValorBD);
                sql.Replace("@008", this.Cidade.ValorBD);
                sql.Replace("@009", this.Estados.ValorBD);
                sql.Replace("@010", this.Pais.ValorBD);
                sql.Replace("@011", this.DDDTelefone.ValorBD);
                sql.Replace("@012", this.Telefone.ValorBD);
                sql.Replace("@013", this.EmailAlertaRetirada.ValorBD);

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
        /// Atualiza Regional
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cRegional WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tRegional SET Nome = '@001', CNPJ = '@002', CEP = '@003', Logradouro = '@004', Numero = @005, Complemento = '@006', Bairro = '@007', Cidade = '@008', Estados = '@009', Pais = '@010', DDDTelefone = '@011', Telefone = '@012', EmailAlertaRetirada = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CNPJ.ValorBD);
                sql.Replace("@003", this.CEP.ValorBD);
                sql.Replace("@004", this.Logradouro.ValorBD);
                sql.Replace("@005", this.Numero.ValorBD);
                sql.Replace("@006", this.Complemento.ValorBD);
                sql.Replace("@007", this.Bairro.ValorBD);
                sql.Replace("@008", this.Cidade.ValorBD);
                sql.Replace("@009", this.Estados.ValorBD);
                sql.Replace("@010", this.Pais.ValorBD);
                sql.Replace("@011", this.DDDTelefone.ValorBD);
                sql.Replace("@012", this.Telefone.ValorBD);
                sql.Replace("@013", this.EmailAlertaRetirada.ValorBD);

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
        /// Exclui Regional com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cRegional WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tRegional WHERE ID=" + id;

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
        /// Exclui Regional com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cRegional WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tRegional WHERE ID=" + id;

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
        /// Exclui Regional
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

            this.Nome.Limpar();
            this.CNPJ.Limpar();
            this.CEP.Limpar();
            this.Logradouro.Limpar();
            this.Numero.Limpar();
            this.Complemento.Limpar();
            this.Bairro.Limpar();
            this.Cidade.Limpar();
            this.Estados.Limpar();
            this.Pais.Limpar();
            this.DDDTelefone.Limpar();
            this.Telefone.Limpar();
            this.EmailAlertaRetirada.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.CNPJ.Desfazer();
            this.CEP.Desfazer();
            this.Logradouro.Desfazer();
            this.Numero.Desfazer();
            this.Complemento.Desfazer();
            this.Bairro.Desfazer();
            this.Cidade.Desfazer();
            this.Estados.Desfazer();
            this.Pais.Desfazer();
            this.DDDTelefone.Desfazer();
            this.Telefone.Desfazer();
            this.EmailAlertaRetirada.Desfazer();
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
                    return 80;
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

        public class cnpj : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CNPJ";
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

        public class cep : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CEP";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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
                    return 80;
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

        public class numero : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Numero";
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

        public class complemento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Complemento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
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

        public class cidade : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cidade";
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

        public class estados : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estados";
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

        public class pais : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Pais";
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

        public class dddtelefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDTelefone";
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

        public class telefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Telefone";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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

        public class emailalertaretirada : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmailAlertaRetirada";
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

                DataTable tabela = new DataTable("Regional");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CNPJ", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Numero", typeof(int));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estados", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("EmailAlertaRetirada", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "RegionalLista_B"

    public abstract class RegionalLista_B : BaseLista
    {

        private bool backup = false;
        protected Regional regional;

        // passar o Usuario logado no sistema
        public RegionalLista_B()
        {
            regional = new Regional();
        }

        // passar o Usuario logado no sistema
        public RegionalLista_B(int usuarioIDLogado)
        {
            regional = new Regional(usuarioIDLogado);
        }

        public Regional Regional
        {
            get { return regional; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Regional especifico
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
                    regional.Ler(id);
                    return regional;
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
                    sql = "SELECT ID FROM tRegional";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRegional";

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
                    sql = "SELECT ID FROM tRegional";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRegional";

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
                    sql = "SELECT ID FROM xRegional";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xRegional";

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
        /// Preenche Regional corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    regional.Ler(id);
                else
                    regional.LerBackup(id);

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

                bool ok = regional.Excluir();
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
        /// Inseri novo(a) Regional na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = regional.Inserir();
                if (ok)
                {
                    lista.Add(regional.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Regional carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Regional");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CNPJ", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Numero", typeof(int));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estados", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("EmailAlertaRetirada", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = regional.Control.ID;
                        linha["Nome"] = regional.Nome.Valor;
                        linha["CNPJ"] = regional.CNPJ.Valor;
                        linha["CEP"] = regional.CEP.Valor;
                        linha["Logradouro"] = regional.Logradouro.Valor;
                        linha["Numero"] = regional.Numero.Valor;
                        linha["Complemento"] = regional.Complemento.Valor;
                        linha["Bairro"] = regional.Bairro.Valor;
                        linha["Cidade"] = regional.Cidade.Valor;
                        linha["Estados"] = regional.Estados.Valor;
                        linha["Pais"] = regional.Pais.Valor;
                        linha["DDDTelefone"] = regional.DDDTelefone.Valor;
                        linha["Telefone"] = regional.Telefone.Valor;
                        linha["EmailAlertaRetirada"] = regional.EmailAlertaRetirada.Valor;
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

                DataTable tabela = new DataTable("RelatorioRegional");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("CNPJ", typeof(string));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("Logradouro", typeof(string));
                    tabela.Columns.Add("Numero", typeof(int));
                    tabela.Columns.Add("Complemento", typeof(string));
                    tabela.Columns.Add("Bairro", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estados", typeof(string));
                    tabela.Columns.Add("Pais", typeof(string));
                    tabela.Columns.Add("DDDTelefone", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("EmailAlertaRetirada", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = regional.Nome.Valor;
                        linha["CNPJ"] = regional.CNPJ.Valor;
                        linha["CEP"] = regional.CEP.Valor;
                        linha["Logradouro"] = regional.Logradouro.Valor;
                        linha["Numero"] = regional.Numero.Valor;
                        linha["Complemento"] = regional.Complemento.Valor;
                        linha["Bairro"] = regional.Bairro.Valor;
                        linha["Cidade"] = regional.Cidade.Valor;
                        linha["Estados"] = regional.Estados.Valor;
                        linha["Pais"] = regional.Pais.Valor;
                        linha["DDDTelefone"] = regional.DDDTelefone.Valor;
                        linha["Telefone"] = regional.Telefone.Valor;
                        linha["EmailAlertaRetirada"] = regional.EmailAlertaRetirada.Valor;
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
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tRegional WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "CNPJ":
                        sql = "SELECT ID, CNPJ FROM tRegional WHERE " + FiltroSQL + " ORDER BY CNPJ";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tRegional WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "Logradouro":
                        sql = "SELECT ID, Logradouro FROM tRegional WHERE " + FiltroSQL + " ORDER BY Logradouro";
                        break;
                    case "Numero":
                        sql = "SELECT ID, Numero FROM tRegional WHERE " + FiltroSQL + " ORDER BY Numero";
                        break;
                    case "Complemento":
                        sql = "SELECT ID, Complemento FROM tRegional WHERE " + FiltroSQL + " ORDER BY Complemento";
                        break;
                    case "Bairro":
                        sql = "SELECT ID, Bairro FROM tRegional WHERE " + FiltroSQL + " ORDER BY Bairro";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tRegional WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estados":
                        sql = "SELECT ID, Estados FROM tRegional WHERE " + FiltroSQL + " ORDER BY Estados";
                        break;
                    case "Pais":
                        sql = "SELECT ID, Pais FROM tRegional WHERE " + FiltroSQL + " ORDER BY Pais";
                        break;
                    case "DDDTelefone":
                        sql = "SELECT ID, DDDTelefone FROM tRegional WHERE " + FiltroSQL + " ORDER BY DDDTelefone";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tRegional WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "EmailAlertaRetirada":
                        sql = "SELECT ID, EmailAlertaRetirada FROM tRegional WHERE " + FiltroSQL + " ORDER BY EmailAlertaRetirada";
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

    #region "RegionalException"

    [Serializable]
    public class RegionalException : Exception
    {

        public RegionalException() : base() { }

        public RegionalException(string msg) : base(msg) { }

        public RegionalException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}