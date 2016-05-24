using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Linq;
using Dapper;
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class LoginADO : MasterADO<dbSite>
    {
        private readonly int siteId;

        public LoginADO(MasterADOBase ado = null)
            : this(1, ado)
        {
        }

        public LoginADO(int siteId, MasterADOBase ado = null)
            : base(ado)
        {
            this.siteId = siteId;
        }

        public Login Consultar(int _clienteId)
        {
            var sql = @"SELECT * FROM Login(NOLOCK) WHERE ClienteID = @ClienteID";

            var query = conSite.Query<Login>(sql, new { ClienteID = _clienteId });
            var result = query.FirstOrDefault();

            return result;
        }

        public Login ConsultarOSESP(int _clienteId)
        {
            var queryStr = @"SELECT l.* FROM Login(NOLOCK) AS l JOIN API_Osesp_Assinantes(NOLOCK) AS a ON a.ClienteID = l.ClienteID WHERE l.ClienteID = @ClienteID";

            var query = conSite.Query<Login>(queryStr, new { ClienteID = _clienteId });
            var result = query.FirstOrDefault();
            return result;
        }

        public Login ConsultarUsername(string username)
        {
            var sql = @"SELECT l.* FROM Login(NOLOCK) AS l WHERE(email = @username OR Cpf = @username) AND SiteId = @siteId";

            var query = conSite.Query<Login>(sql, new { username, siteId = siteId });

            return query.FirstOrDefault();
        }

        public Login ConsultarEmailCPF(string email, string cpf)
        {
            const string queryString = @"SELECT * FROM Login(NOLOCK) l WHERE l.Email = @email AND l.CPF = @cpf AND l.SiteID = @siteId";

            var query = conSite.Query<Login>(queryString, new { email = email, cpf = cpf.Replace(".", "").Replace("-", ""), siteId = siteId });

            return query.FirstOrDefault();
        }

        public Login ConsultarEmail(string email)
        {
            const string queryString = @"SELECT l.* FROM Login(NOLOCK) AS l WHERE l.Email = @email AND l.SiteId = @siteId";

            var query = conSite.Query<Login>(queryString, new { email, siteId = siteId });

            return query.FirstOrDefault();
        }

        public Login ConsultarCPF(string cpf)
        {
            const string queryString = @"SELECT l.* FROM Login(NOLOCK) AS l WHERE l.CPF = @cpf AND l.SiteId = @siteId";

            var query = conSite.Query<Login>(queryString, new { cpf, siteId = siteId });

            return query.FirstOrDefault();
        }

        public Login ConsultarFacebook(string _faceBookUserId)
        {
            const string sql = @"SELECT l.* FROM Login(NOLOCK) AS l WHERE l.FaceBookUserID = @faceBookUserID AND l.SiteId = @siteId";

            var query = conSite.Query<Login>(sql, new { faceBookUserID = _faceBookUserId, siteId = siteId });

            return query.FirstOrDefault();
        }

        public Login ConsultarAccountKit(string id)
        {
            var sql = @"SELECT TOP 1
                            ID,
                            ClienteID,
                            CPF,
                            Senha,
                            Email,
                            DataCadastro,
                            UltimoAcesso,
                            Ativo,
                            StatusAtual,
                            FaceBookUserID,
                            FaceBookUserToken,
                            FaceBookUserInfos,
                            AccountKitId,
                            SiteID
                        FROM Login ( NOLOCK )
                        WHERE AccountKitId = @accountKitId
                        AND SiteId = @siteId
                       ";

            var query = conSite.Query<Login>(sql, new { accountKitId = id, siteId = this.siteId });

            return query.FirstOrDefault();
        }
        
        public bool AtualizarUltimoAcesso(Login login)
        {

            try
            {
                return (conSite.Execute("UPDATE Login SET UltimoAcesso = @UltimoAcesso WHERE ID = @id", new
                {
                    id = login.ID,
                    ultimoAcesso = login.UltimoAcesso
                }) > 0);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;

            }
        }


        public bool AtualizarUltimoAcesso(Login login, int apiUsuarioId)
        {

            try
            {
                return (conSite.Execute("UPDATE Login SET UltimoAcesso = @UltimoAcesso, LoginAPIUsuarioId = @apiLoginId  WHERE ID = @id", new
                {
                    id = login.ID,
                    ultimoAcesso = login.UltimoAcesso,
                    apiLoginId = apiUsuarioId
                }) > 0);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;

            }
        }

        public bool BloquearLogin(int ClienteID)
        {
            var sql = @"UPDATE Login SET StatusAtual = 'B' WHERE ClienteID = @ClienteID";

            var response = conSite.Execute(sql, new { ClienteID });

            return response > 0;
        }
    }
}
