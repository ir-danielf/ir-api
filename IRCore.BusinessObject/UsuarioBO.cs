using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public class UsuarioBO : MasterBO<UsuarioADO>
    {

        public UsuarioBO(MasterADOBase ado = null) : base(ado) { }

        public List<tUsuario> ListaUsuariosParceiroMidia(int parceiroMidiaID, int perfilID)
        {
            return ado.ListaUsuariosParceiroMidia(parceiroMidiaID,perfilID);
        }

        public RetornoModel<tUsuario,enumUsuarioException> Logar(string login, string password)
        {

            tUsuario usuario = Consultar(login);
            if (usuario == null)
            {
                return new RetornoModel<tUsuario,enumUsuarioException>(){Sucesso = false,Mensagem = "Login ou Senha Inválidos",Retorno = usuario,Tipo = enumUsuarioException.usuarioNaoEncontrado};
            }
            else
            {
                if (usuario.Senha == Criptografar(password))
                {
                    return VarificarLogin(usuario);
                }
                else
                {
                    return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = false, Mensagem = "Login ou Senha Inválidos", Retorno = usuario, Tipo = enumUsuarioException.senhaInvalida};
                }
            }
        }

        private RetornoModel<tUsuario,enumUsuarioException> VarificarLogin(tUsuario usuario)
        {
            if (usuario.StatusAsEnum == enumUsuarioStatus.bloqueado)
            {
                return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = false, Mensagem = "Usuário Bloqueado", Retorno = usuario, Tipo = enumUsuarioException.usuarioBloqueado };
            }
            if ((usuario.ValidadeAsBool) && (usuario.ValidoDeAsDateTime != null) && (DateTime.Now < usuario.ValidoDeAsDateTime))
            {
                return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = false, Mensagem = "Este usuário só poderá ser usado após " + usuario.ValidoDeAsDateTime.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), Retorno = usuario, Tipo = enumUsuarioException.usuarioForaDataValidade };
            }
            if ((usuario.ValidadeAsBool) && (usuario.ValidoAteAsDateTime != null) && (DateTime.Now > usuario.ValidoAteAsDateTime))
            {
                return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = false, Mensagem = "Este usuário não tem mais acesso ao sistema desde " + usuario.ValidoAteAsDateTime.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), Retorno = usuario, Tipo = enumUsuarioException.usuarioForaDataValidade };
            }
            return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = true, Retorno = usuario, Mensagem = "OK", Tipo = enumUsuarioException.nenhum };
        }

        public tUsuario Consultar(string login)
        {
            tUsuario usuario = ado.Consultar(login);
            return usuario;
        }

        public tUsuario Consultar(int usuarioId)
        {
            tUsuario usuario = ado.Consultar(usuarioId);
                
            return usuario;
        }

        public RetornoModel<tUsuario,enumUsuarioException> Cadastrar(tUsuario usuario, int usuarioLogadoId)
        {

            tUsuario usuarioByLogin = Consultar(usuario.Login);
            if ((usuarioByLogin != null) && (usuarioByLogin.ID != usuario.ID))
            {
                return new RetornoModel<tUsuario,enumUsuarioException>(){Sucesso = false,Retorno = usuario,Mensagem = "Já existe um usuário cadastrado com este login",Tipo = enumUsuarioException.usuarioJaCadastradoComLogin};
            }
            if (usuario.ID == 0)
            {
                usuario.Senha = Criptografar(usuario.Senha);
            }
            Salvar(usuario, usuarioLogadoId);
            return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = true, Retorno = usuario, Mensagem = "OK", Tipo = enumUsuarioException.nenhum };
        }

        public RetornoModel<tUsuario,enumUsuarioException> MudarSenha(tUsuario usuario, string oldPassword, string newPassword)
        {
            if (usuario.Senha == Criptografar(oldPassword))
            {
                MudarSenha(usuario, newPassword, usuario.ID);
                return new RetornoModel<tUsuario, enumUsuarioException>() { Sucesso = true, Retorno = usuario, Mensagem = "OK", Tipo = enumUsuarioException.nenhum };
            }
            else
            {
                return new RetornoModel<tUsuario,enumUsuarioException>(){Sucesso = false,Retorno = usuario,Mensagem = "A Senha atual não confere",Tipo = enumUsuarioException.senhaInvalida};
            }
        }

        public void MudarSenha(tUsuario usuario, string password, int usuarioLogadoId)
        {
            usuario.Senha = Criptografar(password);
            Salvar(usuario, usuarioLogadoId);
        }

        public void Ativar(int usuarioId, bool ativado, int usuarioLogadoId)
        {
            var usuario = Consultar(usuarioId);
            usuario.StatusAsEnum = ativado?enumUsuarioStatus.liberado:enumUsuarioStatus.bloqueado;
            Salvar(usuario, usuarioLogadoId);
        }

        public void MudarSenha(int usuarioId, string password, int usuarioLogadoId)
        {
            var usuario = Consultar(usuarioId);
            MudarSenha(usuario, password, usuarioLogadoId);
        }

        private string GerarCodigoTerminal(int usuarioID)
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

        /// <summary>
        /// Salvar Usuario
        /// </summary>
        /// <param name="usuario"></param>
        public void Salvar(tUsuario usuario, int usuarioLogadoId)
        {
            if (string.IsNullOrEmpty(usuario.CodigoTerminal))
            {
                int ID = (usuario.ID == 0) ? ado.ControleProximoID<tUsuario>() : usuario.ID;
                usuario.CodigoTerminal = GerarCodigoTerminal(ID);
            }
            ado.Salvar(usuario, usuarioLogadoId);
        }


        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<tUsuario> Listar(int pageNumber, int pageSize, string busca = null)
        {
            return ado.Listar(pageNumber, pageSize, busca, 0);
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public IPagedList<tUsuario> Listar(int pageNumber, int pageSize, string busca = null, int empresaId=0)
        {
            return ado.Listar(pageNumber, pageSize, busca, empresaId);
        }

        public static string Criptografar(string str)
        {
            string hashedPassword = "";

            using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
            {
                byte[] pwordData = Encoding.Default.GetBytes(str);
                byte[] hash = hashAlg.ComputeHash(pwordData);
                hashedPassword = Convert.ToBase64String(hash);
            }
            return hashedPassword;
        }

        public static bool VerfificarCriptografia(string strCriptografada, string str)
        {
            return (strCriptografada.Equals(Criptografar(str)));
        }

        public static bool TemPermissao(ProcListaPrefisUsuario_Result perfil, enumPerfilNome[] perfisNomes, enumPerfilTipo[] perfisTipos)
        {
            if (perfil != null)
            {
                if ((perfisNomes != null) && (perfisNomes.Length > 0) && (perfil.PerfilNome.ToLower() != enumPerfilNome.master.Description().ToLower()) && (!perfisNomes.Any(t => t.Description().ToLower().Split(',').Contains(perfil.PerfilNome.ToLower()))))
                {
                    return false;
                }

                if ((perfisTipos != null) && (perfisTipos.Length > 0))
                {
                    return perfisTipos.Select(t => t.Description().ToLower()).Contains(perfil.PerfilTipo.ToLower());
                }
                return true;
            }

            // Caso o pefil seja nulo, retorna true se os parâmetros passados também ferem null o vazios
            return ((perfisNomes == null) || (perfisNomes.Length == 0) || (perfisTipos == null) || (perfisTipos.Length == 0));
        }

        public static bool TemPermissao(ProcListaPrefisUsuario_Result perfil, enumPerfilNome perfilNome, params enumPerfilTipo[] perfisTipos)
        {
            enumPerfilNome[] perfisNomes = null;
            if (perfilNome != enumPerfilNome.todos)
            {
                perfisNomes = new enumPerfilNome[1] { perfilNome };
            }
            return TemPermissao(perfil, perfisNomes, perfisTipos);
        }

        public static bool TemPermissao(ProcListaPrefisUsuario_Result perfil, enumPerfilTipo perfilTipo, params enumPerfilNome[] perfisNomes)
        {
            enumPerfilTipo[] perfisTipos = null;
            if (perfilTipo != enumPerfilTipo.todos)
            {
                perfisTipos = new enumPerfilTipo[1] { perfilTipo };
            }
            return TemPermissao(perfil, perfisNomes, perfisTipos);
        }

    }
}
