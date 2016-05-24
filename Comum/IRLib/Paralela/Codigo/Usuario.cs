/******************************************************
* Arquivo Usuario.cs
* Gerado em: segunda-feira, 28 de março de 2005
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace IRLib.Paralela
{

    public class Usuario : Usuario_B
    {
        public int PerfilID;
        public int LocalID;
        private senhacriptografada SenhaCriptografada;

        public const string Temporario = "T";
        public const string Liberado = "L";
        public const string Bloqueado = "B";

        //algumas regras de senha e login
        public const int LoginTamanhoMinimo = 4;
        public const int LoginTamanhoMaximo = 20;
        public const int SenhaTamanhoMinimo = 4;
        public const int SenhaTamanhoMaximo = 20; //confirmar!!
        //public const int INTERNET_USUARIO_ID = 1657;
        //public const int MOBILE_USUARIO_ID = 19515;
        public const int IMPLANTAR_EVENTO_ESPECIAL_ID = 14;
        public static int INTERNET_USUARIO_ID
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["InternetUsuarioID"]))
                    return 1657;
                else
                    return Convert.ToInt32(ConfigurationManager.AppSettings["InternetUsuarioID"]);

            }
        }

        public bool AcessoSupervisor { get; set; }
        public bool AcessoFinanceiro { get; set; }
        public bool AcessoImplantarEventoEspecial { get; set; }
        public Usuario() { }

        public Usuario(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        ///Retorna true se o perfil logado é Master.
        /// </summary>
        /// <returns></returns>
        public bool Master
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == 1 && this.EmpresaID.Valor == 1);
                }
                else
                    throw new UsuarioException("Não foi possível identificar usuário. Usuário não lido.");
            }

        }

        /// <summary>		
        /// Obter logins de todos os usuarios do banco de dados
        /// </summary>
        /// <returns></returns>
        public override DataTable Logins()
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Login", typeof(string));

                string sql = "SELECT ID, Login FROM tUsuario WHERE Status<>'B' ORDER BY Login";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Login"] = bd.LerString("Login");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        /// Obter todos os usuarios do banco de dados
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tUsuario ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Gerar usuários e seus perfis
        /// </summary>
        /// <param name="info"></param>
        public void Novo(DataSet info)
        {

            try
            {

                ArrayList erros = new ArrayList();

                DataTable usuarios = info.Tables["Usuario"];
                DataTable perfis = info.Tables["Perfil"];

                foreach (DataRow usuario in usuarios.Rows)
                {

                    Usuario usuarioTmp = new Usuario();

                    usuarioTmp.EmpresaID.Valor = (int)usuario["EmpresaID"];

                    usuarioTmp.Sexo.Valor = (string)usuario["Sexo"];

                    usuarioTmp.Nome.Valor = (string)usuario["Nome"];
                    usuarioTmp.Email.Valor = (string)usuario["Email"];
                    usuarioTmp.Login.Valor = (string)usuario["Login"];

                    usuarioTmp.Senha.Valor = (string)usuario["Senha"];
                    usuarioTmp.Validade.Valor = (bool)usuario["Validade"];
                    usuarioTmp.ValidoDe.Valor = (DateTime)usuario["ValidoDe"];
                    usuarioTmp.ValidoAte.Valor = (DateTime)usuario["ValidoAte"];

                    usuarioTmp.Status.Valor = (string)usuario["Status"];

                    bool ok;
                    try
                    {
                        ok = usuarioTmp.Inserir();
                    }
                    catch
                    {
                        ok = false;
                    }

                    if (!ok)
                    {
                        erros.Add(usuarioTmp.Login.Valor);
                    }
                    else
                    {

                        //inserir perfis p/ esse usuarioTmp
                        foreach (DataRow perfil in perfis.Rows)
                        {

                            int perfilID = (int)perfil["PerfilID"];
                            int tipoID = (int)perfil["TipoID"];
                            int tipo = (int)perfil["PerfilTipoID"];

                            Perfil perfilTmp = new Perfil();
                            CTLib.IBaseBD p = perfilTmp.TipoGenerico(tipo, tipoID, perfilID, usuarioTmp.Control.ID);
                            if (p != null)
                                p.Inserir();

                        }

                    }

                }

                if (erros.Count > 0)
                    throw new UsuarioException("Nem todos os usuários foram cadastrados com sucesso. São eles: " + erros.ToString());

            }
            catch (Exception)
            {
                throw;
            }

        }

        public void AtribuirEstrutura(EstruturaUsuario estruturaUsuario)
        {
            this.Control.ID = estruturaUsuario.ID;
            this.Nome.Valor = estruturaUsuario.Nome;
            this.EmpresaID.Valor = estruturaUsuario.EmpresaID;
            this.Sexo.Valor = estruturaUsuario.Sexo;
            this.Email.Valor = estruturaUsuario.Email;
            this.Login.Valor = estruturaUsuario.Login;
            this.Senha.Valor = estruturaUsuario.Senha;
            this.Validade.Valor = estruturaUsuario.Validade;
            this.ValidoDe.Valor = estruturaUsuario.ValidoDe;
            this.ValidoAte.Valor = estruturaUsuario.ValidoAte;
            this.Status.Valor = estruturaUsuario.Status;




        }

        public void Inserir(EstruturaUsuario estruturaUsuario)
        {
            this.AtribuirEstrutura(estruturaUsuario);
            this.Inserir();
        }

        public override bool Inserir()
        {
            try
            {
                //verifica se login ja existe*******************************************
                string s = "SELECT 1 FROM tUsuario WHERE Login='" + this.Login.Valor + "'";
                object login = bd.ConsultaValor(s);
                bd.Fechar();

                bool ok = (login != null);

                if (ok)//usuario ja existe
                    throw new Exception("Usuário não cadastrado. Login já existe.");


                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cUsuario");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;
                this.CodigoTerminal.ValorBD = GerarCodigoTerminal(this.Control.ID);
                InserirControle("I");

                sql = new StringBuilder();
                sql.Append("INSERT INTO tUsuario (ID, EmpresaID, Nome, Sexo, Email, Login, Senha, Status, Validade, ValidoDe, ValidoAte, CodigoTerminal) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Sexo.ValorBD);
                sql.Replace("@004", this.Email.ValorBD);
                sql.Replace("@005", this.Login.ValorBD);
                SenhaCriptografada = new senhacriptografada(this.Senha.Valor);
                sql.Replace("@006", this.SenhaCriptografada.Valor);
                sql.Replace("@007", Temporario);
                sql.Replace("@008", this.Validade.ValorBD);
                sql.Replace("@009", this.ValidoDe.ValorBD);
                sql.Replace("@010", this.ValidoAte.ValorBD);
                sql.Replace("@011", this.CodigoTerminal.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string GerarCodigoTerminal(int usuarioID)
        {
            char primeiro = 'I';
            char segundo = 'R';
            string parteNumerica = "";
            string retorno = "";
            if (usuarioID.ToString().Length <= 6)
            {
                //popula a quantidade de zeros
                for (int i = 0; i < 6 - usuarioID.ToString().Length; i++)
                {
                    parteNumerica += "0";
                }
                parteNumerica += usuarioID.ToString();

                retorno = primeiro.ToString() + segundo.ToString() + parteNumerica;

            }
            else if (usuarioID.ToString().Length > 6)
            {
                //se o usuarioID passar de 6 digitos a parte numerica deve resetar o numero de vezes que passou de 999999
                int numeroDeVezesUltrapassadas = usuarioID / 999999;
                int novoUsuarioID = (usuarioID - 999999 * numeroDeVezesUltrapassadas);

                //popula a quantidade de zeros
                for (int i = 0; i < 6 - novoUsuarioID.ToString().Length; i++)
                {
                    parteNumerica += "0";
                }
                parteNumerica += novoUsuarioID.ToString();

                //se tiver passado deve-se mudar as letras pra ficar com o codigo do terminal unico
                int char1 = 73;// I
                int char2 = 82;// R
                for (int i = 0; i < numeroDeVezesUltrapassadas; i++)
                {
                    if (char1 < 90) //90 é o char 'Z'
                        char1++;
                    else if (char2 < 90)
                        char2++;
                }
                primeiro = (char)char1;
                segundo = (char)char2;

                retorno = primeiro.ToString() + segundo.ToString() + parteNumerica;

            }
            return retorno;

        }

        public void Atualizar(EstruturaUsuario estruturaUsuario)
        {
            this.AtribuirEstrutura(estruturaUsuario);
            this.Atualizar();
        }

        public override bool Atualizar()
        {
            try
            {

                object login = bd.ConsultaValor("SELECT 1 FROM tUsuario WHERE Login='" + this.Login.Valor + "' AND ID<>" + this.Control.ID);

                bd.Fechar();

                bool ok = (login != null);

                if (!ok)
                {

                    string sqlVersao = "SELECT MAX(Versao) FROM cUsuario WHERE ID = " + this.Control.ID;
                    object o = bd.ConsultaValor(sqlVersao);
                    int v = (o != null) ? Convert.ToInt32(o) : 0;
                    this.Control.Versao = v;

                    InserirControle("U");
                    InserirLog();

                    StringBuilder sql = new StringBuilder();
                    sql.Append("UPDATE tUsuario SET EmpresaID = @001, Nome = '@002', Sexo = '@003', Email = '@004', Login = '@005', Status = '@007', Validade = '@008', ValidoDe = '@009', ValidoAte = '@010', CodigoTerminal = '@011' ");
                    sql.Append("WHERE ID=@ID");
                    sql.Replace("@ID", this.Control.ID.ToString());
                    sql.Replace("@001", this.EmpresaID.ValorBD);
                    sql.Replace("@002", this.Nome.ValorBD);
                    sql.Replace("@003", this.Sexo.ValorBD);
                    sql.Replace("@004", this.Email.ValorBD);
                    sql.Replace("@005", this.Login.ValorBD);
                    sql.Replace("@007", this.Status.ValorBD);
                    sql.Replace("@008", this.Validade.ValorBD);
                    sql.Replace("@009", this.ValidoDe.ValorBD);
                    sql.Replace("@010", this.ValidoAte.ValorBD);
                    sql.Replace("@011", GerarCodigoTerminal(this.Control.ID));

                    int x = bd.Executar(sql.ToString());
                    bd.Fechar();

                    bool result = Convert.ToBoolean(x);

                    return result;

                }
                else
                {
                    //usuario ja existe
                    throw new UsuarioException("Usuário não alterado. Login já existe.");

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string HashSenha(string senha)
        {
            string senhaC = "";

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(senha);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                senhaC = Convert.ToBase64String(hash);
            }

            return senhaC;
        }

        /// <summary>		
        ///Se o login e senha estiverem corretos, e se o Status for T ou L,
        ///    lê os dados do usuario e retorna True
        ///        Informe Login
        ///        Informe a Senha
        /// </summary>
        /// <param name="Login">Informe Login</param>
        /// <param name="Senha">Informe a Senha</param>
        /// <returns>Retorna true se valido, false se invalido</returns>
        public override bool Validar()
        {
            //Aqui eh feito o Login

            string senhaC = "";

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(this.Senha.Valor);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                senhaC = Convert.ToBase64String(hash);
            }

            try
            {

                string sql = "SELECT * FROM tUsuario WHERE Login='" + this.Login.ValorBD + "' AND " +
                    "Senha='" + senhaC + "' AND (Status='" + Liberado + "' OR " +
                    "(Status='" + Temporario + "' AND ValidoAte >= '" + DateTime.Now.ToString("yyyyMMdd") + "'))";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Limpar();
                    this.Control.ID = bd.LerInt("ID");
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

                    bd.Fechar();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public int ValidarSupervisor(string Senha, string Login)
        {
            string senhaC = "";
            int UsuarioID = 0;

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(Senha);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                senhaC = Convert.ToBase64String(hash);
            }

            try
            {
                string sql = "SELECT tu.ID FROM tUsuario tu " +
                    "WHERE (tu.Login='" + Login + "' AND " +
                    "tu.Senha='" + senhaC + "' AND (tu.Status='" + Liberado + "' OR " +
                    "tu.Status='" + Temporario + "'))";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    UsuarioID = bd.LerInt("ID");
                    bd.Fechar();
                }
                return UsuarioID;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidarSupervisor(int usuarioID, int CanalID)
        {
            try
            {
                string sql = "SELECT pc.UsuarioID " +
                    "from tPerfilCanal AS pc " +
                    "WHERE pc.UsuarioID =" + usuarioID + " AND pc.PerfilID = 7 " +
                    "AND pc.CanalID = " + CanalID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("UsuarioID");
                    bd.Fechar();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidarSupervisor(int usuarioID)
        {
            try
            {
                string sql = "SELECT pl.UsuarioID " +
                    "from tPerfilLocal AS pl " +
                    "WHERE pl.UsuarioID =" + usuarioID + " AND pl.PerfilID = 16 ";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("UsuarioID");
                    bd.Fechar();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public List<EstruturaUsuario> CarregarListaFiltrada(int regionalID, int empresaID, string nome, string login, string codigoTerminal)
        {
            try
            {
                StringBuilder filter = new StringBuilder();

                if (regionalID > 0)
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" e.RegionalID = {0} ", regionalID);
                }

                if (empresaID > 0)
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" u.EmpresaID = {0} ", empresaID);
                }


                if (!String.IsNullOrEmpty(nome))
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" u.Nome like '%{0}%' ", nome);
                }

                if (!String.IsNullOrEmpty(login))
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" u.Login like '%{0}%' ", login);

                }

                if (!String.IsNullOrEmpty(codigoTerminal))
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" u.CodigoTerminal like '%{0}%' ", codigoTerminal);

                }




                string sql = string.Empty;
                List<EstruturaUsuario> lista = new List<EstruturaUsuario>();

                sql = string.Format(@"SELECT u.ID, u.EmpresaID, u.Nome, u.Sexo, u.Email, u.Login, 
                                    u.Status, u.Validade, u.ValidoDe, u.ValidoAte, u.CodigoTerminal,e.RegionalID
                                    FROM tUsuario u (NOLOCK) inner join tEmpresa e (NOLOCK) ON u.EmpresaID=e.ID 
                                    {0}",
                                        filter.Length > 0 ? "WHERE " + filter.ToString() : string.Empty);


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaUsuario()
                    {
                        ID = bd.LerInt("ID"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        RegionalID = bd.LerInt("RegionalID"),
                        Nome = bd.LerString("Nome"),
                        Sexo = bd.LerString("Sexo"),
                        Email = bd.LerString("Email"),
                        Login = bd.LerString("Login"),
                        Status = bd.LerString("Status"),
                        Validade = bd.LerBoolean("Validade"),
                        ValidoDe = bd.LerDateTime("ValidoDe"),
                        ValidoAte = bd.LerDateTime("ValidoAte"),
                        CodigoTerminal = bd.LerString("CodigoTerminal"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Anti Fraude Login
        /// </summary>
        /// <returns></returns>
        public bool ValidarLoginAntiFraude(int PerfilID)
        {
            //Aqui eh feito o Login

            string senhaC = "";

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(this.Senha.Valor);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                senhaC = Convert.ToBase64String(hash);
            }

            try
            {

                string sql = "SELECT tUsuario.* , tPerfilEspecial.PerfilID FROM tUsuario (NOLOCK)" +
                    " INNER JOIN tPerfilEspecial ON tUsuario.ID = tPerfilEspecial.UsuarioID " +
                    " WHERE (Login='" + this.Login.ValorBD + "' AND " +
                    "Senha='" + senhaC + "' AND (Status='" + Liberado + "' OR " +
                    "Status='" + Temporario + "'))" +
                    " AND PerfilID = " + PerfilID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Limpar();
                    this.Control.ID = bd.LerInt("ID");
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
                    this.PerfilID = bd.LerInt("PerfilID");
                    bd.Fechar();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }


        /// <summary>		
        ///O usuario pode trocar a prohpria senha.
        ///Basta fornecer a nova senha (nao precisa da antiga), uma vez que ele ja esta autenticado
        ///Se o Status estiver como T muda para L
        ///Devolve True se conseguiu trocar a senha (no futuro vai checar critehrios de complexidade)
        /// </summary>
        /// <returns></returns>
        public override bool TrocarSenha(string senha)
        {

            try
            {

                string senhaC = "";

                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(senha);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    senhaC = Convert.ToBase64String(hash);
                }

                string sql = "UPDATE tUsuario SET Senha='" + senhaC + "',Status='" + Liberado + "' WHERE ID=" + this.Control.ID;

                int resp = bd.Executar(sql);
                bd.Cnn.Close();
                bd.Fechar();

                bool ok = (resp != 0);

                if (ok)
                    this.Status.Valor = Liberado;

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Usuarios com o perfil "Segurança" de uma empresa podem trocar a senha de outros usuarios
        ///desta empresa.
        ///Trocar a senha, e mudar o status daquele usuario para T.
        ///O objeto representa o usuario com perfil Segurança, e o usuarioid representa o usuario que
        ///tera a senha trocada. 
        ///Devolve True se conseguiu trocar a senha.
        /// </summary>
        /// <param name="Senha">Informe a Senha</param>
        /// <returns></returns>
        public override bool TrocarSenha(string senha, int usuarioid)
        {

            try
            {

                string senhaC = "";

                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(senha);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    senhaC = Convert.ToBase64String(hash);
                }

                string sql = "UPDATE tUsuario SET Senha='" + senhaC + "'  WHERE ID=" + usuarioid;

                int resp = bd.Executar(sql);

                bd.Fechar();

                bool ok = (resp != 0);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Usuarios com o perfil "Segurança" de uma empresa podem bloquear outros usuarios
        ///desta empresa.
        ///Mudar o status daquele usuario para B.
        ///O objeto representa o usuario com perfil Segurança, e o UsuarioID representa o usuario que
        ///sera bloqueado.
        ///Devolve True se conseguiu bloquear o usuario.
        /// </summary>
        /// <returns></returns>
        public override bool Bloquear(int usuarioid)
        {

            try
            {

                string sql = "UPDATE tUsuario SET Status='+Bloqueado+' WHERE ID=" + usuarioid;

                int resp = bd.Executar(sql);

                bd.Fechar();

                bool ok = (resp != 0);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Usuarios com o perfil "Segurança" de uma empresa podem desbloquear outros usuarios
        ///desta empresa.
        ///Mudar o status daquele usuario para L.
        ///O objeto representa o usuario com perfil Segurança, e o UsuarioID representa o usuario que
        ///sera desbloqueado.
        ///Devolve True se conseguiu desbloquear o usuario.
        /// </summary>
        /// <returns></returns>
        public override bool Desbloquear(int usuarioid)
        {

            try
            {

                string sql = "UPDATE tUsuario SET Status='+Liberado+' WHERE ID=" + usuarioid;

                int resp = bd.Executar(sql);

                bd.Fechar();

                bool ok = (resp != 0);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }
        /// <summary>		
        ///Devolve todos os perfis do usuario
        /// </summary>
        /// <returns></returns>
        private DataTable PerfisEstrutura()
        {

            DataTable tabela = new DataTable("Perfil");
            //DataColumn idUnico = new DataColumn("IDUnico",typeof(int));
            //idUnico.AutoIncrement=true;
            //idUnico.Unique = true;
            //idUnico.AutoIncrementSeed = 1;
            //idUnico.AutoIncrementStep = 1;

            //tabela.Columns.Add(idUnico);
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string)); //nome do perfil
            tabela.Columns.Add("TipoID", typeof(int)); //localID, eventoID, canalID, empresaID...
            tabela.Columns.Add("EmpresaID", typeof(int)); //empresaID
            tabela.Columns.Add("Empresa", typeof(string)); //nome da empresa
            tabela.Columns.Add("Tipo", typeof(int)); //Empresa, Canal, Local..
            tabela.Columns.Add("CanalTipoID", typeof(int)).DefaultValue = 0;
            tabela.Columns.Add("RegionalID", typeof(int)).DefaultValue = 0;

            return tabela;

        }

        /// <summary>		
        ///Devolve todos os perfis do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable Perfis()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                foreach (DataRow linha in PerfisEspecial().Select("", "Nome"))
                    tabela.ImportRow(linha);
                foreach (DataRow linha in PerfisEmpresa().Select("", "Nome"))
                    tabela.ImportRow(linha);
                foreach (DataRow linha in PerfisCanal().Select("", "Nome"))
                    tabela.ImportRow(linha);
                foreach (DataRow linha in PerfisEvento().Select("", "Nome"))
                    tabela.ImportRow(linha);
                foreach (DataRow linha in PerfisLocal().Select("", "Nome"))
                    tabela.ImportRow(linha);
                foreach (DataRow linha in PerfisRegional().Select("", "Nome"))
                    tabela.ImportRow(linha);

                tabela.Columns.Add("IDUnico", typeof(int));

                for (int i = 1; i <= tabela.Rows.Count; i++)
                {
                    tabela.Rows[i - 1]["IDUnico"] = i;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        ///Devolve os perfis de empresa do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable PerfisEmpresa()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT p.ID, p.Nome, e.Nome AS Empresa, e.ID AS EmpresaID, e.Nome AS Empresa " +
                    "FROM tPerfil AS p,tPerfilEmpresa AS pe,tEmpresa AS e WHERE " +
                    "p.ID=pe.PerfilID AND pe.UsuarioID=" + this.Control.ID + " AND " +
                    "e.ID=pe.EmpresaID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Empresa");
                    linha["TipoID"] = bd.LerInt("EmpresaID");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Tipo"] = PerfilTipo.EMPRESA;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Devolve os perfis de local do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable PerfisLocal()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT p.ID,p.Nome,l.Nome AS Local,l.ID AS LocalID, e.Nome AS Empresa, e.ID AS EmpresaID, r.ID AS RegionalID " +
                    "FROM tPerfil AS p, tLocal AS l, tPerfilLocal AS pl, tEmpresa AS e, tRegional r (NOLOCK) " +
                    "WHERE l.ID=pl.LocalID AND e.ID=l.EmpresaID AND " +
                    "p.ID=pl.PerfilID AND pl.UsuarioID=" + this.Control.ID + " AND r.ID = e.RegionalID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Local");
                    linha["TipoID"] = bd.LerInt("LocalID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["Tipo"] = PerfilTipo.LOCAL;
                    linha["RegionalID"] = bd.LerInt("RegionalID");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Devolve os perfis de evento do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable PerfisEvento()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT p.ID,p.Nome,e.Nome AS Evento,e.ID AS EventoID, emp.Nome AS Empresa, emp.ID AS EmpresaID " +
                    "FROM tPerfil AS p,tEvento AS e,tPerfilEvento AS pe, tLocal AS l, tEmpresa AS emp " +
                    "WHERE e.ID=pe.EventoID AND l.ID=e.LocalID AND emp.ID=l.EmpresaID AND " +
                    "p.ID=pe.PerfilID AND pe.UsuarioID=" + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Evento");
                    linha["TipoID"] = bd.LerInt("EventoID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["Tipo"] = PerfilTipo.EVENTO;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Devolve os perfis de canal do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable PerfisCanal()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT p.ID,p.Nome,c.Nome AS Canal,c.ID AS CanalID, e.Nome AS Empresa, e.ID AS EmpresaID, e.RegionalID, c.CanalTipoID " +
                    "FROM tPerfil AS p, tPerfilCanal AS pc, tCanal AS c, tEmpresa AS e " +
                    "WHERE c.ID=pc.CanalID AND e.ID=c.EmpresaID AND " +
                    "p.ID=pc.PerfilID AND pc.UsuarioID=" + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Canal");
                    linha["TipoID"] = bd.LerInt("CanalID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["RegionalID"] = bd.LerInt("RegionalID");
                    linha["CanalTipoID"] = bd.LerInt("CanalTipoID");
                    linha["Tipo"] = PerfilTipo.CANAL;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataTable PerfisCanalVenderInternet(string[] canais)
        {
            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = string.Format(@"SELECT ROW_NUMBER() OVER (ORDER BY p.ID) as ID, p.Nome,c.Nome AS Canal,c.ID AS CanalID, e.Nome AS Empresa, e.ID AS EmpresaID, e.RegionalID, c.CanalTipoID
                FROM tPerfil p (NOLOCK)
                INNER JOIN tPerfilCanal pc (NOLOCK) ON p.ID = pc.PerfilID
                INNER JOIN tCanal c (NOLOCK) ON c.ID = pc.CanalID
                INNER JOIN tEmpresa e (NOLOCK) ON e.ID = c.EmpresaID
                WHERE pc.UsuarioID = {0} AND c.ID IN ({1})", this.Control.ID, Utilitario.ArrayToString(canais));


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Canal");
                    linha["TipoID"] = bd.LerInt("CanalID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["RegionalID"] = bd.LerInt("RegionalID");
                    linha["CanalTipoID"] = bd.LerInt("CanalTipoID");
                    linha["Tipo"] = PerfilTipo.CANAL;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable PerfilSacEspecial()
        {
            try
            {
                DataTable tabela = PerfisEstrutura();

                string sql = @"SELECT p.Nome
                            FROM tPerfil p (NOLOCK)
                            INNER JOIN tPerfilEspecial pc (NOLOCK) ON p.ID = pc.PerfilID
                            WHERE p.ID = " + Perfil.SAC_OPERADOR + " AND pc.UsuarioID = " + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = Perfil.SAC_OPERADOR;
                    linha["Nome"] = bd.LerString("Nome") + " - Especial";
                    linha["TipoID"] = Canal.CANAL_INTERNET;
                    linha["Tipo"] = PerfilTipo.ESPECIAL;
                    linha["Empresa"] = "";
                    linha["EmpresaID"] = 1;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }


        /// <summary>		
        ///Devolve os perfis de especial do usuario
        /// </summary>
        /// <returns></returns>
        public override DataTable PerfisEspecial()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT tPerfil.ID,tPerfil.Nome " +
                    "FROM tPerfil, tPerfilEspecial " +
                    "WHERE tPerfil.ID=tPerfilEspecial.PerfilID AND " +
                    "tPerfilEspecial.UsuarioID=" + this.Control.ID;

                //Empresa empresa = new Empresa();
                //empresa.Ler(this.EmpresaID.Valor);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - Especial";
                    linha["Tipo"] = PerfilTipo.ESPECIAL;
                    linha["Empresa"] = "";//empresa.Nome.Valor;
                    linha["EmpresaID"] = 0;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        ///Devolve os perfis de regional do usuario
        /// </summary>
        /// <returns></returns>
        public DataTable PerfisRegional()
        {

            try
            {

                DataTable tabela = PerfisEstrutura();

                string sql = "SELECT p.ID,p.Nome,r.Nome AS Regional,r.ID AS RegionalID " +
                    "FROM tPerfil AS p, tRegional AS r, tPerfilRegional AS pl " +
                    "WHERE r.ID=pl.RegionalID AND p.ID=pl.PerfilID AND pl.UsuarioID=" + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Regional");
                    linha["TipoID"] = bd.LerInt("RegionalID");
                    linha["Tipo"] = PerfilTipo.REGIONAL;
                    linha["Empresa"] = "";//empresa.Nome.Valor;
                    linha["EmpresaID"] = 0;
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public override DataTable Caixas(string registroZero)
        {
            // Criando DataTable
            DataTable tabela = new DataTable("");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID, tCaixa.DataFechamento " +
                    "FROM tCaixa INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE (tCaixa.UsuarioID = " + this.Control.ID + ") " +
                    "ORDER BY tCaixa.DataAbertura DESC";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    //					// Somente os caixas fechados
                    //					if (bd.LerStringFormatoDataHora("DataFechamento")!= "  /  /       :  ") {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DataAbertura"] = bd.LerString("Nome") + " - " + bd.LerStringFormatoDataHora("DataAbertura");
                    tabela.Rows.Add(linha);
                    //					}
                }
                bd.Fechar();
            }
            catch (Exception)
            {
                throw;
            } // fim de try			
            finally
            {
                bd.Fechar();
            }
            return tabela;
        }



        /// <summary>
        /// Vendas Gerenciais por evento com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total, com porcentagem
        /// Se for por Canal o parâmetro 
        /// apresentacaoID corresponde a usuarioID
        /// eventoID corresponde a lojaID
        /// localID corresponde a canalID
        /// </summary>
        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                {
                    // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os Caixas nos intervalos especificados
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // usuario zero pega todos
                int totaisVendidos = QuantidadeIngressosPorUsuario(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorUsuario(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorUsuario(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorUsuario(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                # endregion
                // Para cada evento no período especificado, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorUsuario(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorUsuario(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorUsuario(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorUsuario(ingressoLogIDsCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "Usuario";
                return tabela;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Eventos por período do Caixa e situacao dos ingressos
        /// </summary>
        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tUsuario.ID, tUsuario.Nome " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("ID");
                        linha["VariacaoLinha"] = obterDados.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                bd.Fechar();
            }
        }
        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorUsuario(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoUsuario = "";
                    if (this.Control.ID > 0)
                        condicaoUsuario = "AND (tCaixa.UsuarioID = " + this.Control.ID + ") ";
                    else
                        condicaoUsuario = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tUsuario.ID) AS QuantidadeIngressos " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoUsuario;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de evento
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos eventos
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de QuantidadeIngressosPorUsuario
        /// <summary>
        /// Obter valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorUsuario(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoUsuario = "";
                    if (this.Control.ID > 0)
                        condicaoUsuario = "AND (tCaixa.UsuarioID = " + this.Control.ID + ") ";
                    else
                        condicaoUsuario = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   SUM(tPreco.Valor) AS Valor " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tUsuario ON tCaixa.UsuarioID = tUsuario.ID INNER JOIN " +
                        "tPreco ON tIngressoLog.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoUsuario;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do evento
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos eventos
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de ValorIngressosPorUsuario

        public DataTable Listagem(int empresaID)
        {
            try
            {
                DataTable tabela = new DataTable("UsuarioListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Login", typeof(string));
                tabela.Columns.Add("Usuário", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Válido de", typeof(string));
                tabela.Columns.Add("Válido até", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0)
                {
                    condicao = "WHERE      (tEmpresa.ID = " + empresaID + ") ";
                }
                else
                {
                    condicao = "";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tUsuario.ID, tUsuario.Nome, tUsuario.Sexo, tUsuario.Email, tUsuario.Login, tUsuario.Status, tUsuario.Validade, tUsuario.ValidoDe, tUsuario.ValidoAte " +
                    "FROM     tUsuario INNER JOIN " +
                    "tEmpresa ON tUsuario.EmpresaID = tEmpresa.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tUsuario.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Usuário"] = bd.LerString("Nome");
                    linha["Sexo"] = bd.LerString("Sexo");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Login"] = bd.LerString("Login");
                    string status = "";
                    switch (bd.LerString("Status"))
                    {
                        case Liberado:
                            status = "Liberado";
                            break;
                        case Temporario:
                            status = "Temporário";
                            break;
                        case Bloqueado:
                            status = "Bloqueado";
                            break;
                    }
                    linha["Status"] = status;
                    if (status == "Temporário" || bd.LerBoolean("Validade"))
                    {
                        linha["Válido de"] = bd.LerStringFormatoData("ValidoDe");
                        linha["Válido até"] = bd.LerStringFormatoData("ValidoAte");
                    }
                    else
                    {
                        linha["Válido de"] = "";
                        linha["Válido até"] = "";
                    }
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de Listagem

        public DataTable ListagemPorRegionalID(int regionalID)
        {
            try
            {
                DataTable tabela = new DataTable("UsuarioListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Login", typeof(string));
                tabela.Columns.Add("Usuário", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("E-mail", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("Válido de", typeof(string));
                tabela.Columns.Add("Válido até", typeof(string));

                // Obtendo dados
                string sql;
                sql =
                    "SELECT   tEmpresa.Nome AS Empresa, tUsuario.ID, tUsuario.Nome, tUsuario.Sexo, tUsuario.Email, tUsuario.Login, tUsuario.Status, tUsuario.Validade, tUsuario.ValidoDe, tUsuario.ValidoAte " +
                    "FROM     tUsuario INNER JOIN " +
                    "tEmpresa ON tUsuario.EmpresaID = tEmpresa.ID " +
                    "WHERE tEmpresa.RegionalId = " + regionalID +
                    " ORDER BY tEmpresa.Nome, tUsuario.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Usuário"] = bd.LerString("Nome");
                    linha["Sexo"] = bd.LerString("Sexo");
                    linha["E-mail"] = bd.LerString("Email");
                    linha["Login"] = bd.LerString("Login");
                    string status = "";
                    switch (bd.LerString("Status"))
                    {
                        case Liberado:
                            status = "Liberado";
                            break;
                        case Temporario:
                            status = "Temporário";
                            break;
                        case Bloqueado:
                            status = "Bloqueado";
                            break;
                    }
                    linha["Status"] = status;
                    if (status == "Temporário" || bd.LerBoolean("Validade"))
                    {
                        linha["Válido de"] = bd.LerStringFormatoData("ValidoDe");
                        linha["Válido até"] = bd.LerStringFormatoData("ValidoAte");
                    }
                    else
                    {
                        linha["Válido de"] = "";
                        linha["Válido até"] = "";
                    }
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de Listagem




        public Usuario LogarAssinatura(string usuario, string senha, int canalID, int assinaturaTipoID)
        {
            try
            {
                string senhaC = "";

                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(senha);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    senhaC = Convert.ToBase64String(hash);
                }

                string sql =
                    string.Format(
                    @"
                        SELECT
                             u.ID, u.Login, u.Nome, p.ID AS PerfilID
                        FROM tUsuario u (NOLOCK)
                        INNER JOIN tPerfilCanal pc (NOLOCK) ON pc.UsuarioID = u.ID
                        INNER JOIN tPerfil p (NOLOCK) ON p.ID = pc.PerfilID
                        WHERE u.Login = '{0}' AND u.Senha = '{1}' AND pc.CanalID = {2}
                             AND (Status='{3}' OR (Status='{4}' AND u.ValidoAte >= '{5}')) AND p.ID = {6}",
                        usuario, senhaC, canalID, Liberado, Temporario, DateTime.Now.ToString("yyyyMMdd"),
                        Perfil.CANAL_BILHETEIRO, Perfil.LOCAL_IMPLANTAREVENTO);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Login/Senha inválidos ou você não possui acesso ao ambiente de assinaturas.");

                this.Control.ID = bd.LerInt("ID");
                this.Nome.Valor = bd.LerString("Nome");
                this.Login.Valor = bd.LerString("Login");
                this.PerfilID = bd.LerInt("PerfilID");

                bd.FecharConsulta();

                this.AcessoSupervisor = Convert.ToInt32(bd.ConsultaValor(
                        string.Format(@"SELECT COUNT(u.ID)
                                FROM tUsuario u (NOLOCK)
                                INNER JOIN tPerfilLocal pl (NOLOCK) ON pl.UsuarioID = u.ID
                                INNER JOIN tPerfil p (NOLOCK) ON p.ID = pl.PerfilID 
                                INNER JOIN tLocal l (NOLOCK) ON l.ID = pl.LocalID
                                INNER JOIN tAssinaturaTipo at (NOLOCK) ON at.LocalID = l.ID
                                WHERE u.ID = {0} AND at.ID = {1} AND p.ID = {2}"
                                , this.Control.ID, assinaturaTipoID, Perfil.LOCAL_IMPLANTAREVENTO))) > 0;

                this.AcessoFinanceiro = Convert.ToInt32(bd.ConsultaValor(
                    string.Format(@"SELECT COUNT(u.ID)
                            FROM tUsuario u (NOLOCK)
                            INNER JOIN tPerfilEmpresa pe (NOLOCK) ON pe.UsuarioID = u.ID
                            INNER JOIN tPerfil p (NOLOCK) ON p.ID = pe.PerfilID
                            INNER JOIN tEmpresa e (NOLOCK) ON pe.EmpresaID = e.ID
                            INNER JOIN tLocal l (NOLOCK) ON l.EmpresaID = e.ID
                            INNER JOIN tAssinaturaTipo at (NOLOCK) ON at.LocalID = l.ID
                            WHERE u.ID = {0} AND at.ID = {1} AND p.ID = {2}",
                            this.Control.ID, assinaturaTipoID, Perfil.EMPRESA_FINANCEIRO))) > 0;

                this.AcessoImplantarEventoEspecial = Convert.ToInt32(bd.ConsultaValor(
                    string.Format(@"SELECT COUNT(u.ID)
                            FROM tUsuario u (NOLOCK)
                            INNER JOIN tPerfilEspecial pe (NOLOCK) ON pe.UsuarioID = u.ID
                            INNER JOIN tPerfil p (NOLOCK) ON p.ID = pe.PerfilID                            
                            WHERE u.ID = {0} AND p.ID = {1}",
                            this.Control.ID, Perfil.ESPECIAL_IMPLANTAREVENTO))) > 0;

                return this;
            }
            finally
            {
                bd.Fechar();
            }
        }


    } // fim da classe

    public class senhacriptografada : Usuario_B.senha
    {

        private string senha;

        public senhacriptografada(string senha)
        {
            this.senha = senha;
        }

        public override string Valor
        {
            get
            {

                string senhaC = "";

                using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                {
                    byte[] pwordData = Encoding.Default.GetBytes(senha);
                    byte[] hash = hashAlg.ComputeHash(pwordData);
                    senhaC = Convert.ToBase64String(hash);
                }


                return senhaC;
            }

        }

    }

    public class UsuarioLista : UsuarioLista_B
    {

        public UsuarioLista() { }

        public UsuarioLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela de todos os campos de usuario carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            DataTable tabela = new DataTable("Usuario");

            try
            {

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Login", typeof(string));
                    tabela.Columns.Add("Empresa", typeof(string));
                    tabela.Columns.Add("Validade", typeof(string));
                    tabela.Columns.Add("De", typeof(string));
                    tabela.Columns.Add("Até", typeof(string));
                    //tabela.Columns.Add("Status", typeof(string)); //nao cabe na tela

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Login"] = usuario.Login.Valor;
                        Empresa empresa = new Empresa();
                        empresa.Ler(usuario.EmpresaID.Valor);
                        linha["Empresa"] = empresa.Nome.Valor;
                        linha["Validade"] = (usuario.Validade.Valor) ? "Sim" : "Nao";
                        linha["De"] = usuario.ValidoDe.Valor.ToString("dd/MM/yyyy");
                        linha["Até"] = usuario.ValidoAte.Valor.ToString("dd/MM/yyyy");
                        //linha["Status"]= usuario.Status.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                {
                    //erro: nao carregou a lista
                    tabela = null;
                }

            }
            catch
            {
                tabela = null;
            }

            return tabela;

        }


        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tUsuario WHERE ID IN (" + IDsAtuais + ") AND Login like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY  EmpresaID, Login");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }





    }

}
