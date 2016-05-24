/******************************************************
* Arquivo UsuarioDB.cs
* Gerado em: 12/03/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Usuario_B"

    public abstract class Usuario_B : BaseBD
    {

        public empresaid EmpresaID = new empresaid();
        public nome Nome = new nome();
        public sexo Sexo = new sexo();
        public email Email = new email();
        public login Login = new login();
        public senha Senha = new senha();
        public status Status = new status();
        public validade Validade = new validade();
        public validode ValidoDe = new validode();
        public validoate ValidoAte = new validoate();
        public codigoterminal CodigoTerminal = new codigoterminal();

        public Usuario_B() { }

        // passar o Usuario logado no sistema
        public Usuario_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Usuario
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tUsuario WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Sexo.ValorBD = bd.LerString("Sexo");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Login.ValorBD = bd.LerString("Login");
                    this.Senha.ValorBD = bd.LerString("Senha");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Validade.ValorBD = bd.LerString("Validade");
                    this.ValidoDe.ValorBD = bd.LerString("ValidoDe");
                    this.ValidoAte.ValorBD = bd.LerString("ValidoAte");
                    this.CodigoTerminal.ValorBD = bd.LerString("CodigoTerminal");
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
        /// Preenche todos os atributos de Usuario do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xUsuario WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Sexo.ValorBD = bd.LerString("Sexo");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Login.ValorBD = bd.LerString("Login");
                    this.Senha.ValorBD = bd.LerString("Senha");
                    this.Status.ValorBD = bd.LerString("Status");
                    this.Validade.ValorBD = bd.LerString("Validade");
                    this.ValidoDe.ValorBD = bd.LerString("ValidoDe");
                    this.ValidoAte.ValorBD = bd.LerString("ValidoAte");
                    this.CodigoTerminal.ValorBD = bd.LerString("CodigoTerminal");
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
                sql.Append("INSERT INTO cUsuario (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xUsuario (ID, Versao, EmpresaID, Nome, Sexo, Email, Login, Senha, Status, Validade, ValidoDe, ValidoAte, CodigoTerminal) ");
                sql.Append("SELECT ID, @V, EmpresaID, Nome, Sexo, Email, Login, Senha, Status, Validade, ValidoDe, ValidoAte, CodigoTerminal FROM tUsuario WHERE ID = @I");
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
        /// Inserir novo(a) Usuario
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cUsuario");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tUsuario(ID, EmpresaID, Nome, Sexo, Email, Login, Senha, Status, Validade, ValidoDe, ValidoAte, CodigoTerminal) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Sexo.ValorBD);
                sql.Replace("@004", this.Email.ValorBD);
                sql.Replace("@005", this.Login.ValorBD);
                sql.Replace("@006", this.Senha.ValorBD);
                sql.Replace("@007", this.Status.ValorBD);
                sql.Replace("@008", this.Validade.ValorBD);
                sql.Replace("@009", this.ValidoDe.ValorBD);
                sql.Replace("@010", this.ValidoAte.ValorBD);
                sql.Replace("@011", this.CodigoTerminal.ValorBD);

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
        /// Atualiza Usuario
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cUsuario WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tUsuario SET EmpresaID = @001, Nome = '@002', Sexo = '@003', Email = '@004', Login = '@005', Senha = '@006', Status = '@007', Validade = '@008', ValidoDe = '@009', ValidoAte = '@010', CodigoTerminal = '@011' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Sexo.ValorBD);
                sql.Replace("@004", this.Email.ValorBD);
                sql.Replace("@005", this.Login.ValorBD);
                sql.Replace("@006", this.Senha.ValorBD);
                sql.Replace("@007", this.Status.ValorBD);
                sql.Replace("@008", this.Validade.ValorBD);
                sql.Replace("@009", this.ValidoDe.ValorBD);
                sql.Replace("@010", this.ValidoAte.ValorBD);
                sql.Replace("@011", this.CodigoTerminal.ValorBD);

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
        /// Exclui Usuario com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cUsuario WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tUsuario WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui Usuario
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                    throw new Exception("O usuário não pode ser excluído porque ainda possui vinculos");
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public override void Limpar()
        {

            this.EmpresaID.Limpar();
            this.Nome.Limpar();
            this.Sexo.Limpar();
            this.Email.Limpar();
            this.Login.Limpar();
            this.Senha.Limpar();
            this.Status.Limpar();
            this.Validade.Limpar();
            this.ValidoDe.Limpar();
            this.ValidoAte.Limpar();
            this.CodigoTerminal.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EmpresaID.Desfazer();
            this.Nome.Desfazer();
            this.Sexo.Desfazer();
            this.Email.Desfazer();
            this.Login.Desfazer();
            this.Senha.Desfazer();
            this.Status.Desfazer();
            this.Validade.Desfazer();
            this.ValidoDe.Desfazer();
            this.ValidoAte.Desfazer();
            this.CodigoTerminal.Desfazer();
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

        public class sexo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sexo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class login : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Login";
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

        public class senha : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Senha";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 60;
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

        public class status : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class validade : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Validade";
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

        public class validode : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValidoDe";
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

        public class validoate : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValidoAte";
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

        public class codigoterminal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoTerminal";
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

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Login", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Validade", typeof(bool));
                tabela.Columns.Add("ValidoDe", typeof(DateTime));
                tabela.Columns.Add("ValidoAte", typeof(DateTime));
                tabela.Columns.Add("CodigoTerminal", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Logins();

        public abstract DataTable Todos();

        public abstract DataTable Perfis();

        public abstract DataTable PerfisEmpresa();

        public abstract DataTable PerfisLocal();

        public abstract DataTable PerfisEvento();

        public abstract DataTable PerfisCanal();

        public abstract DataTable PerfisEspecial();

        public abstract bool Validar();

        public abstract bool TrocarSenha(string senha);

        public abstract bool TrocarSenha(string senha, int usuarioid);

        public abstract bool Bloquear(int usuarioid);

        public abstract bool Desbloquear(int usuarioid);

        public abstract DataTable Caixas(string registrozero);

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorUsuario(string ingressologids);

        public abstract decimal ValorIngressosPorUsuario(string ingressologids);

    }
    #endregion

    #region "UsuarioLista_B"

    public abstract class UsuarioLista_B : BaseLista
    {

        private bool backup = false;
        protected Usuario usuario;

        // passar o Usuario logado no sistema
        public UsuarioLista_B()
        {
            usuario = new Usuario();
        }

        // passar o Usuario logado no sistema
        public UsuarioLista_B(int usuarioIDLogado)
        {
            usuario = new Usuario(usuarioIDLogado);
        }

        public Usuario Usuario
        {
            get { return usuario; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Usuario especifico
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
                    usuario.Ler(id);
                    return usuario;
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
                    sql = "SELECT ID FROM tUsuario";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tUsuario";

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
                    sql = "SELECT ID FROM tUsuario";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tUsuario";

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
                    sql = "SELECT ID FROM xUsuario";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xUsuario";

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
        /// Preenche Usuario corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    usuario.Ler(id);
                else
                    usuario.LerBackup(id);

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

                bool ok = usuario.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
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
        /// Inseri novo(a) Usuario na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = usuario.Inserir();
                if (ok)
                {
                    lista.Add(usuario.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Usuario carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Login", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Validade", typeof(bool));
                tabela.Columns.Add("ValidoDe", typeof(DateTime));
                tabela.Columns.Add("ValidoAte", typeof(DateTime));
                tabela.Columns.Add("CodigoTerminal", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = usuario.Control.ID;
                        linha["EmpresaID"] = usuario.EmpresaID.Valor;
                        linha["Nome"] = usuario.Nome.Valor;
                        linha["Sexo"] = usuario.Sexo.Valor;
                        linha["Email"] = usuario.Email.Valor;
                        linha["Login"] = usuario.Login.Valor;
                        linha["Senha"] = usuario.Senha.Valor;
                        linha["Status"] = usuario.Status.Valor;
                        linha["Validade"] = usuario.Validade.Valor;
                        linha["ValidoDe"] = usuario.ValidoDe.Valor;
                        linha["ValidoAte"] = usuario.ValidoAte.Valor;
                        linha["CodigoTerminal"] = usuario.CodigoTerminal.Valor;
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

                DataTable tabela = new DataTable("RelatorioUsuario");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Sexo", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("Login", typeof(string));
                    tabela.Columns.Add("Senha", typeof(string));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("Validade", typeof(bool));
                    tabela.Columns.Add("ValidoDe", typeof(DateTime));
                    tabela.Columns.Add("ValidoAte", typeof(DateTime));
                    tabela.Columns.Add("CodigoTerminal", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EmpresaID"] = usuario.EmpresaID.Valor;
                        linha["Nome"] = usuario.Nome.Valor;
                        linha["Sexo"] = usuario.Sexo.Valor;
                        linha["Email"] = usuario.Email.Valor;
                        linha["Login"] = usuario.Login.Valor;
                        linha["Senha"] = usuario.Senha.Valor;
                        linha["Status"] = usuario.Status.Valor;
                        linha["Validade"] = usuario.Validade.Valor;
                        linha["ValidoDe"] = usuario.ValidoDe.Valor;
                        linha["ValidoAte"] = usuario.ValidoAte.Valor;
                        linha["CodigoTerminal"] = usuario.CodigoTerminal.Valor;
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
                        sql = "SELECT ID, EmpresaID FROM tUsuario WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Sexo":
                        sql = "SELECT ID, Sexo FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Sexo";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "Login":
                        sql = "SELECT ID, Login FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Login";
                        break;
                    case "Senha":
                        sql = "SELECT ID, Senha FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Senha";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "Validade":
                        sql = "SELECT ID, Validade FROM tUsuario WHERE " + FiltroSQL + " ORDER BY Validade";
                        break;
                    case "ValidoDe":
                        sql = "SELECT ID, ValidoDe FROM tUsuario WHERE " + FiltroSQL + " ORDER BY ValidoDe";
                        break;
                    case "ValidoAte":
                        sql = "SELECT ID, ValidoAte FROM tUsuario WHERE " + FiltroSQL + " ORDER BY ValidoAte";
                        break;
                    case "CodigoTerminal":
                        sql = "SELECT ID, CodigoTerminal FROM tUsuario WHERE " + FiltroSQL + " ORDER BY CodigoTerminal";
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

    #region "UsuarioException"

    [Serializable]
    public class UsuarioException : Exception
    {

        public UsuarioException() : base() { }

        public UsuarioException(string msg) : base(msg) { }

        public UsuarioException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}