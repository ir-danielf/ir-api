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
    public class APIUsuarioBO : MasterBO<APIUsuarioADO>
    {

        public APIUsuarioBO(MasterADOBase ado = null) : base(ado) { }

        public APIUsuarioToken ConsultarToken(string token)
        {
            return ado.ConsultarToken(token);
        }

        public APIUsuarioToken ConsultarToken(string dadosIndentificacao, string login)
        {
            return ado.ConsultarToken(dadosIndentificacao, login);
        }

        public APIUsuario Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public APIUsuario Consultar(string login)
        {
            return ado.Consultar(login);
        }

        public List<String> ConsultarPermissoes(int APIUsuarioID)
        {
            return ado.ConsultarPermissoes(APIUsuarioID);
        }

        public bool Salvar(APIUsuario apiUsuario)
        {
            return ado.Salvar(apiUsuario);
        }

        public bool Salvar(APIUsuarioToken apiUsuario, bool removeClientInfo = false)
        {
            if(removeClientInfo)
            {
                ado.ClearToken(apiUsuario.DadosIndentificacao, apiUsuario.APIUsuarioID);
            }
            if(apiUsuario.ID > 0){
                return ado.UpdateToken(apiUsuario);
            }else{
                return ado.InsertToken(apiUsuario);
            }
        }

        public bool AtualizarSessao(APIUsuarioToken apiUsuario, bool salvaToken = false)
        {
            return ado.AtualizarSessao(apiUsuario, salvaToken);
        }

        public List<int> BuscarTokenIds(int clienteId)
        {
            var result = ado.BuscarAPIUsuarioTokens(clienteId);
            return result;
        }

        public bool DesativarTokens(List<int> tokenIds)
        {
            var result = ado.DesativarTokens(tokenIds);
            return result;
        }

        public bool ExpirarToken(int ClienteID)
        {
            var expirou = ado.ExpirarToken(ClienteID);
            return expirou;
        }
    }
}
