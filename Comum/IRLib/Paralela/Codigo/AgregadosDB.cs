

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "Agregados_B"

    public abstract class Agregados_B : BaseBD
    {


        public clienteid ClienteID = new clienteid();

        public nome Nome = new nome();

        public grauparentesco GrauParentesco = new grauparentesco();

        public datanascimento DataNascimento = new datanascimento();

        public profissao Profissao = new profissao();

        public situacaoprofissionalid SituacaoProfissionalID = new situacaoprofissionalid();

        public email Email = new email();


        public Agregados_B() { }

        // passar o Usuario logado no sistema
        public Agregados_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Agregados
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAgregados WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.GrauParentesco.ValorBD = bd.LerInt("GrauParentesco").ToString();

                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");

                    this.Profissao.ValorBD = bd.LerString("Profissao");

                    this.SituacaoProfissionalID.ValorBD = bd.LerInt("SituacaoProfissionalID").ToString();

                    this.Email.ValorBD = bd.LerString("Email");

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
        /// Preenche todos os atributos de Agregados do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xAgregados WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.GrauParentesco.ValorBD = bd.LerInt("GrauParentesco").ToString();

                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");

                    this.Profissao.ValorBD = bd.LerString("Profissao");

                    this.SituacaoProfissionalID.ValorBD = bd.LerInt("SituacaoProfissionalID").ToString();

                    this.Email.ValorBD = bd.LerString("Email");

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
                sql.Append("INSERT INTO cAgregados (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


                sql.Append("INSERT INTO xAgregados (ID, Versao, ClienteID, Nome, GrauParentesco, DataNascimento, Profissao, SituacaoProfissionalID, Email) ");
                sql.Append("SELECT ID, @V, ClienteID, Nome, GrauParentesco, DataNascimento, Profissao, SituacaoProfissionalID, Email FROM tAgregados WHERE ID = @I");
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
        /// Inserir novo(a) Agregados
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TAGREGADOS");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAgregados(ID, ClienteID, Nome, GrauParentesco, DataNascimento, Profissao, SituacaoProfissionalID, Email) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,'@004','@005',@006,'@007')");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.GrauParentesco.ValorBD);
                sql.Replace("@004", this.DataNascimento.ValorBD);
                sql.Replace("@005", this.Profissao.ValorBD);
                sql.Replace("@006", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@007", this.Email.ValorBD);

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
        /// Inserir novo(a) Agregados
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TAGREGADOS");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAgregados(ID, ClienteID, Nome, GrauParentesco, DataNascimento, Profissao, SituacaoProfissionalID, Email) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,'@004','@005',@006,'@007')");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.GrauParentesco.ValorBD);
                sql.Replace("@004", this.DataNascimento.ValorBD);
                sql.Replace("@005", this.Profissao.ValorBD);
                sql.Replace("@006", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@007", this.Email.ValorBD);


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
        /// Atualiza Agregados
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cAgregados WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tAgregados SET ClienteID = @001, Nome = '@002', GrauParentesco = @003, DataNascimento = '@004', Profissao = '@005', SituacaoProfissionalID = @006, Email = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.ClienteID.ValorBD);

                sql.Replace("@002", this.Nome.ValorBD);

                sql.Replace("@003", this.GrauParentesco.ValorBD);

                sql.Replace("@004", this.DataNascimento.ValorBD);

                sql.Replace("@005", this.Profissao.ValorBD);

                sql.Replace("@006", this.SituacaoProfissionalID.ValorBD);

                sql.Replace("@007", this.Email.ValorBD);


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
        /// Atualiza Agregados
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cAgregados WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tAgregados SET ClienteID = @001, Nome = '@002', GrauParentesco = @003, DataNascimento = '@004', Profissao = '@005', SituacaoProfissionalID = @006, Email = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.ClienteID.ValorBD);

                sql.Replace("@002", this.Nome.ValorBD);

                sql.Replace("@003", this.GrauParentesco.ValorBD);

                sql.Replace("@004", this.DataNascimento.ValorBD);

                sql.Replace("@005", this.Profissao.ValorBD);

                sql.Replace("@006", this.SituacaoProfissionalID.ValorBD);

                sql.Replace("@007", this.Email.ValorBD);


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
        /// Exclui Agregados com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAgregados WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAgregados WHERE ID=" + id;

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
        /// Exclui Agregados com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAgregados WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAgregados WHERE ID=" + id;

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
        /// Exclui Agregados
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


            this.ClienteID.Limpar();

            this.Nome.Limpar();

            this.GrauParentesco.Limpar();

            this.DataNascimento.Limpar();

            this.Profissao.Limpar();

            this.SituacaoProfissionalID.Limpar();

            this.Email.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.ClienteID.Desfazer();

            this.Nome.Desfazer();

            this.GrauParentesco.Desfazer();

            this.DataNascimento.Desfazer();

            this.Profissao.Desfazer();

            this.SituacaoProfissionalID.Desfazer();

            this.Email.Desfazer();

        }


        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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


        public class grauparentesco : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "GrauParentesco";
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


        public class datanascimento : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataNascimento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
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

                return base.Valor.ToString("dd/MM/yyyy");

            }

        }


        public class profissao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Profissao";
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


        public class situacaoprofissionalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SituacaoProfissionalID";
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


        public class email : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Email";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

                DataTable tabela = new DataTable("Agregados");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("ClienteID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("GrauParentesco", typeof(int));

                tabela.Columns.Add("DataNascimento", typeof(DateTime));

                tabela.Columns.Add("Profissao", typeof(string));

                tabela.Columns.Add("SituacaoProfissionalID", typeof(int));

                tabela.Columns.Add("Email", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "AgregadosLista_B"


    public abstract class AgregadosLista_B : BaseLista
    {

        private bool backup = false;
        protected Agregados agregados;

        // passar o Usuario logado no sistema
        public AgregadosLista_B()
        {
            agregados = new Agregados();
        }

        // passar o Usuario logado no sistema
        public AgregadosLista_B(int usuarioIDLogado)
        {
            agregados = new Agregados(usuarioIDLogado);
        }

        public Agregados Agregados
        {
            get { return agregados; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Agregados especifico
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
                    agregados.Ler(id);
                    return agregados;
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
                    sql = "SELECT ID FROM tAgregados";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAgregados";

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
                    sql = "SELECT ID FROM tAgregados";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAgregados";

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
                    sql = "SELECT ID FROM xAgregados";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xAgregados";

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
        /// Preenche Agregados corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    agregados.Ler(id);
                else
                    agregados.LerBackup(id);

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

                bool ok = agregados.Excluir();
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
        /// Inseri novo(a) Agregados na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = agregados.Inserir();
                if (ok)
                {
                    lista.Add(agregados.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Agregados carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Agregados");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("ClienteID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("GrauParentesco", typeof(int));

                tabela.Columns.Add("DataNascimento", typeof(DateTime));

                tabela.Columns.Add("Profissao", typeof(string));

                tabela.Columns.Add("SituacaoProfissionalID", typeof(int));

                tabela.Columns.Add("Email", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = agregados.Control.ID;

                        linha["ClienteID"] = agregados.ClienteID.Valor;

                        linha["Nome"] = agregados.Nome.Valor;

                        linha["GrauParentesco"] = agregados.GrauParentesco.Valor;

                        linha["DataNascimento"] = agregados.DataNascimento.Valor;

                        linha["Profissao"] = agregados.Profissao.Valor;

                        linha["SituacaoProfissionalID"] = agregados.SituacaoProfissionalID.Valor;

                        linha["Email"] = agregados.Email.Valor;

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

                DataTable tabela = new DataTable("RelatorioAgregados");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("ClienteID", typeof(int));

                    tabela.Columns.Add("Nome", typeof(string));

                    tabela.Columns.Add("GrauParentesco", typeof(int));

                    tabela.Columns.Add("DataNascimento", typeof(DateTime));

                    tabela.Columns.Add("Profissao", typeof(string));

                    tabela.Columns.Add("SituacaoProfissionalID", typeof(int));

                    tabela.Columns.Add("Email", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["ClienteID"] = agregados.ClienteID.Valor;

                        linha["Nome"] = agregados.Nome.Valor;

                        linha["GrauParentesco"] = agregados.GrauParentesco.Valor;

                        linha["DataNascimento"] = agregados.DataNascimento.Valor;

                        linha["Profissao"] = agregados.Profissao.Valor;

                        linha["SituacaoProfissionalID"] = agregados.SituacaoProfissionalID.Valor;

                        linha["Email"] = agregados.Email.Valor;

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

                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tAgregados WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;

                    case "Nome":
                        sql = "SELECT ID, Nome FROM tAgregados WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;

                    case "GrauParentesco":
                        sql = "SELECT ID, GrauParentesco FROM tAgregados WHERE " + FiltroSQL + " ORDER BY GrauParentesco";
                        break;

                    case "DataNascimento":
                        sql = "SELECT ID, DataNascimento FROM tAgregados WHERE " + FiltroSQL + " ORDER BY DataNascimento";
                        break;

                    case "Profissao":
                        sql = "SELECT ID, Profissao FROM tAgregados WHERE " + FiltroSQL + " ORDER BY Profissao";
                        break;

                    case "SituacaoProfissionalID":
                        sql = "SELECT ID, SituacaoProfissionalID FROM tAgregados WHERE " + FiltroSQL + " ORDER BY SituacaoProfissionalID";
                        break;

                    case "Email":
                        sql = "SELECT ID, Email FROM tAgregados WHERE " + FiltroSQL + " ORDER BY Email";
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

    #region "AgregadosException"

    [Serializable]
    public class AgregadosException : Exception
    {

        public AgregadosException() : base() { }

        public AgregadosException(string msg) : base(msg) { }

        public AgregadosException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}