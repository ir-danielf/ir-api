

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "ResumoHardware_B"

    public abstract class ResumoHardware_B : BaseBD
    {


        public resolucao Resolucao = new resolucao();

        public versaoos VersaoOS = new versaoos();

        public versaoframework VersaoFramework = new versaoframework();

        public loginusuario LoginUsuario = new loginusuario();

        public idmaquina IDMaquina = new idmaquina();


        public ResumoHardware_B() { }

        // passar o Usuario logado no sistema
        public ResumoHardware_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ResumoHardware
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tResumoHardware WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Resolucao.ValorBD = bd.LerString("Resolucao");

                    this.VersaoOS.ValorBD = bd.LerString("VersaoOS");

                    this.VersaoFramework.ValorBD = bd.LerString("VersaoFramework");

                    this.LoginUsuario.ValorBD = bd.LerString("LoginUsuario");

                    this.IDMaquina.ValorBD = bd.LerString("IDMaquina");

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
        /// Preenche todos os atributos de ResumoHardware do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xResumoHardware WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Resolucao.ValorBD = bd.LerString("Resolucao");

                    this.VersaoOS.ValorBD = bd.LerString("VersaoOS");

                    this.VersaoFramework.ValorBD = bd.LerString("VersaoFramework");

                    this.LoginUsuario.ValorBD = bd.LerString("LoginUsuario");

                    this.IDMaquina.ValorBD = bd.LerString("IDMaquina");

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
                sql.Append("INSERT INTO cResumoHardware (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


                sql.Append("INSERT INTO xResumoHardware (ID, Versao, Resolucao, VersaoOS, VersaoFramework, LoginUsuario, IDMaquina) ");
                sql.Append("SELECT ID, @V, Resolucao, VersaoOS, VersaoFramework, LoginUsuario, IDMaquina FROM tResumoHardware WHERE ID = @I");
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
        /// Inserir novo(a) ResumoHardware
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cResumoHardware");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tResumoHardware(ID, Resolucao, VersaoOS, VersaoFramework, LoginUsuario, IDMaquina) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Resolucao.ValorBD);

                sql.Replace("@002", this.VersaoOS.ValorBD);

                sql.Replace("@003", this.VersaoFramework.ValorBD);

                sql.Replace("@004", this.LoginUsuario.ValorBD);

                sql.Replace("@005", this.IDMaquina.ValorBD);


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
        /// Inserir novo(a) ResumoHardware
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cResumoHardware");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tResumoHardware(ID, Resolucao, VersaoOS, VersaoFramework, LoginUsuario, IDMaquina) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Resolucao.ValorBD);

                sql.Replace("@002", this.VersaoOS.ValorBD);

                sql.Replace("@003", this.VersaoFramework.ValorBD);

                sql.Replace("@004", this.LoginUsuario.ValorBD);

                sql.Replace("@005", this.IDMaquina.ValorBD);


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
        /// Atualiza ResumoHardware
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cResumoHardware WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tResumoHardware SET Resolucao = '@001', VersaoOS = '@002', VersaoFramework = '@003', LoginUsuario = '@004', IDMaquina = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Resolucao.ValorBD);

                sql.Replace("@002", this.VersaoOS.ValorBD);

                sql.Replace("@003", this.VersaoFramework.ValorBD);

                sql.Replace("@004", this.LoginUsuario.ValorBD);

                sql.Replace("@005", this.IDMaquina.ValorBD);


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
        /// Atualiza ResumoHardware
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cResumoHardware WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tResumoHardware SET Resolucao = '@001', VersaoOS = '@002', VersaoFramework = '@003', LoginUsuario = '@004', IDMaquina = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Resolucao.ValorBD);

                sql.Replace("@002", this.VersaoOS.ValorBD);

                sql.Replace("@003", this.VersaoFramework.ValorBD);

                sql.Replace("@004", this.LoginUsuario.ValorBD);

                sql.Replace("@005", this.IDMaquina.ValorBD);


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
        /// Exclui ResumoHardware com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cResumoHardware WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tResumoHardware WHERE ID=" + id;

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
        /// Exclui ResumoHardware com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cResumoHardware WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tResumoHardware WHERE ID=" + id;

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
        /// Exclui ResumoHardware
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


            this.Resolucao.Limpar();

            this.VersaoOS.Limpar();

            this.VersaoFramework.Limpar();

            this.LoginUsuario.Limpar();

            this.IDMaquina.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.Resolucao.Desfazer();

            this.VersaoOS.Desfazer();

            this.VersaoFramework.Desfazer();

            this.LoginUsuario.Desfazer();

            this.IDMaquina.Desfazer();

        }


        public class resolucao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Resolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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


        public class versaoos : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoOS";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 18;
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


        public class versaoframework : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoFramework";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 5;
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


        public class loginusuario : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "LoginUsuario";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 25;
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


        public class idmaquina : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "IDMaquina";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 40;
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

                DataTable tabela = new DataTable("ResumoHardware");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Resolucao", typeof(string));

                tabela.Columns.Add("VersaoOS", typeof(string));

                tabela.Columns.Add("VersaoFramework", typeof(string));

                tabela.Columns.Add("LoginUsuario", typeof(string));

                tabela.Columns.Add("IDMaquina", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "ResumoHardwareLista_B"


    public abstract class ResumoHardwareLista_B : BaseLista
    {

        private bool backup = false;
        protected ResumoHardware resumoHardware;

        // passar o Usuario logado no sistema
        public ResumoHardwareLista_B()
        {
            resumoHardware = new ResumoHardware();
        }

        // passar o Usuario logado no sistema
        public ResumoHardwareLista_B(int usuarioIDLogado)
        {
            resumoHardware = new ResumoHardware(usuarioIDLogado);
        }

        public ResumoHardware ResumoHardware
        {
            get { return resumoHardware; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ResumoHardware especifico
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
                    resumoHardware.Ler(id);
                    return resumoHardware;
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
                    sql = "SELECT ID FROM tResumoHardware";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tResumoHardware";

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
                    sql = "SELECT ID FROM tResumoHardware";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tResumoHardware";

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
                    sql = "SELECT ID FROM xResumoHardware";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xResumoHardware";

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
        /// Preenche ResumoHardware corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    resumoHardware.Ler(id);
                else
                    resumoHardware.LerBackup(id);

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

                bool ok = resumoHardware.Excluir();
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
        /// Inseri novo(a) ResumoHardware na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = resumoHardware.Inserir();
                if (ok)
                {
                    lista.Add(resumoHardware.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ResumoHardware carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ResumoHardware");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Resolucao", typeof(string));

                tabela.Columns.Add("VersaoOS", typeof(string));

                tabela.Columns.Add("VersaoFramework", typeof(string));

                tabela.Columns.Add("LoginUsuario", typeof(string));

                tabela.Columns.Add("IDMaquina", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = resumoHardware.Control.ID;

                        linha["Resolucao"] = resumoHardware.Resolucao.Valor;

                        linha["VersaoOS"] = resumoHardware.VersaoOS.Valor;

                        linha["VersaoFramework"] = resumoHardware.VersaoFramework.Valor;

                        linha["LoginUsuario"] = resumoHardware.LoginUsuario.Valor;

                        linha["IDMaquina"] = resumoHardware.IDMaquina.Valor;

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

                DataTable tabela = new DataTable("RelatorioResumoHardware");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("Resolucao", typeof(string));

                    tabela.Columns.Add("VersaoOS", typeof(string));

                    tabela.Columns.Add("VersaoFramework", typeof(string));

                    tabela.Columns.Add("LoginUsuario", typeof(string));

                    tabela.Columns.Add("IDMaquina", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["Resolucao"] = resumoHardware.Resolucao.Valor;

                        linha["VersaoOS"] = resumoHardware.VersaoOS.Valor;

                        linha["VersaoFramework"] = resumoHardware.VersaoFramework.Valor;

                        linha["LoginUsuario"] = resumoHardware.LoginUsuario.Valor;

                        linha["IDMaquina"] = resumoHardware.IDMaquina.Valor;

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

                    case "Resolucao":
                        sql = "SELECT ID, Resolucao FROM tResumoHardware WHERE " + FiltroSQL + " ORDER BY Resolucao";
                        break;

                    case "VersaoOS":
                        sql = "SELECT ID, VersaoOS FROM tResumoHardware WHERE " + FiltroSQL + " ORDER BY VersaoOS";
                        break;

                    case "VersaoFramework":
                        sql = "SELECT ID, VersaoFramework FROM tResumoHardware WHERE " + FiltroSQL + " ORDER BY VersaoFramework";
                        break;

                    case "LoginUsuario":
                        sql = "SELECT ID, LoginUsuario FROM tResumoHardware WHERE " + FiltroSQL + " ORDER BY LoginUsuario";
                        break;

                    case "IDMaquina":
                        sql = "SELECT ID, IDMaquina FROM tResumoHardware WHERE " + FiltroSQL + " ORDER BY IDMaquina";
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

    #region "ResumoHardwareException"

    [Serializable]
    public class ResumoHardwareException : Exception
    {

        public ResumoHardwareException() : base() { }

        public ResumoHardwareException(string msg) : base(msg) { }

        public ResumoHardwareException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}