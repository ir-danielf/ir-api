/******************************************************
* Arquivo PontoVendaDB.cs
* Gerado em: 20/07/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "PontoVenda_B"

    public abstract class PontoVenda_B : BaseBD
    {

        public local Local = new local();
        public nome Nome = new nome();
        public endereco Endereco = new endereco();
        public numero Numero = new numero();
        public compl Compl = new compl();
        public cidade Cidade = new cidade();
        public estado Estado = new estado();
        public bairro Bairro = new bairro();
        public obs Obs = new obs();
        public referencia Referencia = new referencia();
        public cep CEP = new cep();
        public permiteretirada PermiteRetirada = new permiteretirada();
        public ir IR = new ir();

        public PontoVenda_B() { }

        // passar o Usuario logado no sistema
        public PontoVenda_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de PontoVenda
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tPontoVenda WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Local.ValorBD = bd.LerString("Local");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Numero.ValorBD = bd.LerString("Numero");
                    this.Compl.ValorBD = bd.LerString("Compl");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Referencia.ValorBD = bd.LerString("Referencia");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.PermiteRetirada.ValorBD = bd.LerString("PermiteRetirada");
                    this.IR.ValorBD = bd.LerString("IR");
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
        /// Preenche todos os atributos de PontoVenda do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xPontoVenda WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Local.ValorBD = bd.LerString("Local");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Numero.ValorBD = bd.LerString("Numero");
                    this.Compl.ValorBD = bd.LerString("Compl");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Referencia.ValorBD = bd.LerString("Referencia");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.PermiteRetirada.ValorBD = bd.LerString("PermiteRetirada");
                    this.IR.ValorBD = bd.LerString("IR");
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
                sql.Append("INSERT INTO cPontoVenda (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xPontoVenda (ID, Versao, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada, IR) ");
                sql.Append("SELECT ID, @V, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada, IR FROM tPontoVenda WHERE ID = @I");
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
        /// Inserir novo(a) PontoVenda
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPontoVenda");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPontoVenda(ID, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada, IR) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Local.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Compl.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.Referencia.ValorBD);
                sql.Replace("@011", this.CEP.ValorBD);
                sql.Replace("@012", this.PermiteRetirada.ValorBD);
                sql.Replace("@013", this.IR.ValorBD);

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
        /// Inserir novo(a) PontoVenda
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPontoVenda");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPontoVenda(ID, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada, IR) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Local.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Compl.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.Referencia.ValorBD);
                sql.Replace("@011", this.CEP.ValorBD);
                sql.Replace("@012", this.PermiteRetirada.ValorBD);
                sql.Replace("@013", this.IR.ValorBD);

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
        /// Atualiza PontoVenda
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cPontoVenda WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPontoVenda SET Local = '@001', Nome = '@002', Endereco = '@003', Numero = '@004', Compl = '@005', Cidade = '@006', Estado = '@007', Bairro = '@008', Obs = '@009', Referencia = '@010', CEP = '@011', PermiteRetirada = '@012', IR = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Local.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Compl.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.Referencia.ValorBD);
                sql.Replace("@011", this.CEP.ValorBD);
                sql.Replace("@012", this.PermiteRetirada.ValorBD);
                sql.Replace("@013", this.IR.ValorBD);

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
        /// Atualiza PontoVenda
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cPontoVenda WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPontoVenda SET Local = '@001', Nome = '@002', Endereco = '@003', Numero = '@004', Compl = '@005', Cidade = '@006', Estado = '@007', Bairro = '@008', Obs = '@009', Referencia = '@010', CEP = '@011', PermiteRetirada = '@012', IR = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Local.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Compl.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.Referencia.ValorBD);
                sql.Replace("@011", this.CEP.ValorBD);
                sql.Replace("@012", this.PermiteRetirada.ValorBD);
                sql.Replace("@013", this.IR.ValorBD);

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
        /// Exclui PontoVenda com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPontoVenda WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPontoVenda WHERE ID=" + id;

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
        /// Exclui PontoVenda com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPontoVenda WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPontoVenda WHERE ID=" + id;

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
        /// Exclui PontoVenda
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

            this.Local.Limpar();
            this.Nome.Limpar();
            this.Endereco.Limpar();
            this.Numero.Limpar();
            this.Compl.Limpar();
            this.Cidade.Limpar();
            this.Estado.Limpar();
            this.Bairro.Limpar();
            this.Obs.Limpar();
            this.Referencia.Limpar();
            this.CEP.Limpar();
            this.PermiteRetirada.Limpar();
            this.IR.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Local.Desfazer();
            this.Nome.Desfazer();
            this.Endereco.Desfazer();
            this.Numero.Desfazer();
            this.Compl.Desfazer();
            this.Cidade.Desfazer();
            this.Estado.Desfazer();
            this.Bairro.Desfazer();
            this.Obs.Desfazer();
            this.Referencia.Desfazer();
            this.CEP.Desfazer();
            this.PermiteRetirada.Desfazer();
            this.IR.Desfazer();
        }

        public class local : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Local";
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
                    return 150;
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
                    return 200;
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

        public class numero : TextProperty
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

        public class compl : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Compl";
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

        public class estado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estado";
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

        public class obs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Obs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 300;
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

        public class referencia : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Referencia";
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

        public class permiteretirada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermiteRetirada";
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

        public class ir : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "IR";
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

                DataTable tabela = new DataTable("PontoVenda");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Numero", typeof(string));
                tabela.Columns.Add("Compl", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Referencia", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("PermiteRetirada", typeof(bool));
                tabela.Columns.Add("IR", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "PontoVendaLista_B"

    public abstract class PontoVendaLista_B : BaseLista
    {

        private bool backup = false;
        protected PontoVenda pontoVenda;

        // passar o Usuario logado no sistema
        public PontoVendaLista_B()
        {
            pontoVenda = new PontoVenda();
        }

        // passar o Usuario logado no sistema
        public PontoVendaLista_B(int usuarioIDLogado)
        {
            pontoVenda = new PontoVenda(usuarioIDLogado);
        }

        public PontoVenda PontoVenda
        {
            get { return pontoVenda; }
        }

        /// <summary>
        /// Retorna um IBaseBD de PontoVenda especifico
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
                    pontoVenda.Ler(id);
                    return pontoVenda;
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
                    sql = "SELECT ID FROM tPontoVenda";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPontoVenda";

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
                    sql = "SELECT ID FROM tPontoVenda";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPontoVenda";

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
                    sql = "SELECT ID FROM xPontoVenda";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xPontoVenda";

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
        /// Preenche PontoVenda corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    pontoVenda.Ler(id);
                else
                    pontoVenda.LerBackup(id);

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

                bool ok = pontoVenda.Excluir();
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
        /// Inseri novo(a) PontoVenda na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = pontoVenda.Inserir();
                if (ok)
                {
                    lista.Add(pontoVenda.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de PontoVenda carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("PontoVenda");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Numero", typeof(string));
                tabela.Columns.Add("Compl", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Referencia", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("PermiteRetirada", typeof(bool));
                tabela.Columns.Add("IR", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = pontoVenda.Control.ID;
                        linha["Local"] = pontoVenda.Local.Valor;
                        linha["Nome"] = pontoVenda.Nome.Valor;
                        linha["Endereco"] = pontoVenda.Endereco.Valor;
                        linha["Numero"] = pontoVenda.Numero.Valor;
                        linha["Compl"] = pontoVenda.Compl.Valor;
                        linha["Cidade"] = pontoVenda.Cidade.Valor;
                        linha["Estado"] = pontoVenda.Estado.Valor;
                        linha["Bairro"] = pontoVenda.Bairro.Valor;
                        linha["Obs"] = pontoVenda.Obs.Valor;
                        linha["Referencia"] = pontoVenda.Referencia.Valor;
                        linha["CEP"] = pontoVenda.CEP.Valor;
                        linha["PermiteRetirada"] = pontoVenda.PermiteRetirada.Valor;
                        linha["IR"] = pontoVenda.IR.Valor;
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

                DataTable tabela = new DataTable("RelatorioPontoVenda");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Local", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Endereco", typeof(string));
                    tabela.Columns.Add("Numero", typeof(string));
                    tabela.Columns.Add("Compl", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("Bairro", typeof(string));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("Referencia", typeof(string));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("PermiteRetirada", typeof(bool));
                    tabela.Columns.Add("IR", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Local"] = pontoVenda.Local.Valor;
                        linha["Nome"] = pontoVenda.Nome.Valor;
                        linha["Endereco"] = pontoVenda.Endereco.Valor;
                        linha["Numero"] = pontoVenda.Numero.Valor;
                        linha["Compl"] = pontoVenda.Compl.Valor;
                        linha["Cidade"] = pontoVenda.Cidade.Valor;
                        linha["Estado"] = pontoVenda.Estado.Valor;
                        linha["Bairro"] = pontoVenda.Bairro.Valor;
                        linha["Obs"] = pontoVenda.Obs.Valor;
                        linha["Referencia"] = pontoVenda.Referencia.Valor;
                        linha["CEP"] = pontoVenda.CEP.Valor;
                        linha["PermiteRetirada"] = pontoVenda.PermiteRetirada.Valor;
                        linha["IR"] = pontoVenda.IR.Valor;
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
                    case "Local":
                        sql = "SELECT ID, Local FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Local";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Endereco":
                        sql = "SELECT ID, Endereco FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Endereco";
                        break;
                    case "Numero":
                        sql = "SELECT ID, Numero FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Numero";
                        break;
                    case "Compl":
                        sql = "SELECT ID, Compl FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Compl";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "Bairro":
                        sql = "SELECT ID, Bairro FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Bairro";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "Referencia":
                        sql = "SELECT ID, Referencia FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY Referencia";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "PermiteRetirada":
                        sql = "SELECT ID, PermiteRetirada FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY PermiteRetirada";
                        break;
                    case "IR":
                        sql = "SELECT ID, IR FROM tPontoVenda WHERE " + FiltroSQL + " ORDER BY IR";
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

    #region "PontoVendaException"

    [Serializable]
    public class PontoVendaException : Exception
    {

        public PontoVendaException() : base() { }

        public PontoVendaException(string msg) : base(msg) { }

        public PontoVendaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}