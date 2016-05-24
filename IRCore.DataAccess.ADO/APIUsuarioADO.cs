using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using System.Linq.Expressions;
using Dapper;
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class APIUsuarioADO : MasterADO<dbIngresso>
    {
        public APIUsuarioADO(MasterADOBase ado = null) : base(ado) { }

        public APIUsuario Consultar(int id)
        {
            const string queryStr = @"Select top 1
                    au.ID, au.Login, au.Senha, au.Ativo, au.TipoAcesso, au.DominiosOrigem, au.IPOrigem, au.UsuarioID, au.LojaID, au.CanalID, au.HorasExpiracaoToken, au.SiteID
                FROM APIUsuario (nolock) au
                WHERE au.ID = @id";

            var query = conIngresso.Query<APIUsuario>(queryStr, new { id });
            return query.FirstOrDefault();
        }

        public APIUsuario Consultar(string login)
        {
            const string queryStr = @"
                  Select TOP 1 
                       au.ID, au.Login, au.Senha, au.Ativo, au.TipoAcesso, au.DominiosOrigem, au.IPOrigem, au.UsuarioID, au.LojaID, au.CanalID, au.HorasExpiracaoToken, au.SiteID
                  FROM APIUsuario (nolock) au
                  WHERE au.Login = @login";
            var query = conIngresso.Query<APIUsuario>(queryStr, new { login });
            return query.FirstOrDefault();
        }

        public List<String> ConsultarPermissoes(int APIUsuarioID)
        {
            var queryStr = @"
                  Select
                    r.RoleName
                    FROM APIRoleUsuario (nolock) ru
                    JOIN APIRole (nolock) r on r.ID = ru.APIRoleID
                  WHERE ru.APIUsuarioID = @APIUsuarioID";
            return conIngresso.Query<String>(queryStr, new { APIUsuarioID = APIUsuarioID }).ToList();
        }

        public APIUsuarioToken ConsultarToken(string token)
        {
            var queryStr = @"
                Select TOP 1 
                    aut.ID, aut.APIUsuarioID, aut.Token, aut.Ativo, aut.DadosIndentificacao, aut.DadosSession, aut.DataExpiracao,
                    au.ID, au.Login, au.Senha, au.Ativo, au.TipoAcesso, au.DominiosOrigem, au.IPOrigem, au.UsuarioID, au.LojaID, au.CanalID, au.HorasExpiracaoToken
                FROM APIUsuarioToken (nolock) aut
                INNER JOIN APIUsuario (nolock) au ON aut.APIUsuarioID = au.ID
                WHERE aut.token = @token";
            var query = conIngresso.Query<APIUsuarioToken, APIUsuario, APIUsuarioToken>(queryStr, (APIUsuarioToken, APIUsuario) =>
            {
                APIUsuarioToken.APIUsuario = APIUsuario;
                return APIUsuarioToken;
            }, new
            {
                token = token
            });
            return query.FirstOrDefault();
        }

        public APIUsuarioToken ConsultarToken(string dadosIndentificacao, string login)
        {
            var queryStr = @"
                Select TOP 1 
                       aut.ID, aut.APIUsuarioID, aut.Token, aut.Ativo, aut.DadosIndentificacao, aut.DadosSession, aut.DataExpiracao,
                       au.ID, au.Login, au.Senha, au.Ativo, au.TipoAcesso, au.DominiosOrigem, au.IPOrigem, au.UsuarioID, au.LojaID, au.CanalID, au.HorasExpiracaoToken
                FROM APIUsuarioToken (nolock) aut
                INNER JOIN APIUsuario (nolock) au ON aut.APIUsuarioID = au.ID
                WHERE aut.DadosIndentificacao = @dadosIndentificacao";


            if (!string.IsNullOrEmpty(login))
            {
                queryStr += " AND au.Login = @login";
            }

            var query = conIngresso.Query<APIUsuarioToken, APIUsuario, APIUsuarioToken>(queryStr, (APIUsuarioToken, APIUsuario) =>
            {
                APIUsuarioToken.APIUsuario = APIUsuario;
                return APIUsuarioToken;
            }, new
            {
                dadosIndentificacao = dadosIndentificacao,
                login = login
            });
            return query.FirstOrDefault();

        }

        public void ClearToken(string dadosIndentificacao, int apiUsuarioID)
        {
            conIngresso.Execute("DELETE FROM APIUsuarioToken WHERE DadosIndentificacao = @dadosIndentificacao AND APIUsuarioID = @apiUsuarioID", new
            {
                dadosIndentificacao = dadosIndentificacao,
                apiUsuarioID = apiUsuarioID
            });

        }

        public bool UpdateToken(APIUsuarioToken apiUsuario)
        {
            List<string> updates = new List<string>();

            if (apiUsuario.Token != null)
            {
                updates.Add("Token = @Token");
            }
            if (apiUsuario.DataExpiracao != null)
            {
                updates.Add("DataExpiracao = @DataExpiracao");
            }
            if (apiUsuario.DadosSession != null)
            {
                updates.Add("DadosSession = @DadosSession");
            }
            if (apiUsuario.DadosIndentificacao != null)
            {
                updates.Add("DadosIndentificacao = @DadosIndentificacao");
            }
            if (apiUsuario.Ativo != null)
            {
                updates.Add("Ativo = @Ativo");
            }
            if (apiUsuario.APIUsuarioID != null)
            {
                updates.Add("APIUsuarioID = @APIUsuarioID");
            }

            try
            {
                conIngresso.Query("UPDATE APIUsuarioToken SET " + String.Join(",", updates) + " WHERE ID = @ID", apiUsuario);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;

            }

            return true;
        }

        public bool InsertToken(APIUsuarioToken apiUsuario)
        {
            List<string> inserts = new List<string>();

            if (apiUsuario.Token != null)
            {
                inserts.Add("Token");
            }
            if (apiUsuario.DataExpiracao != null)
            {
                inserts.Add("DataExpiracao");
            }
            if (apiUsuario.DadosSession != null)
            {
                inserts.Add("DadosSession");
            }
            if (apiUsuario.DadosIndentificacao != null)
            {
                inserts.Add("DadosIndentificacao");
            }
            if (apiUsuario.Ativo != null)
            {
                inserts.Add("Ativo");
            }
            if (apiUsuario.APIUsuarioID != null)
            {
                inserts.Add("APIUsuarioID");
            }

            try
            {
                conIngresso.Query("Insert into APIUsuarioToken(" + String.Join(",", inserts) + ") values (@" + String.Join(", @", inserts) + ")", apiUsuario);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;

            }

            return true;
        }

        public bool AtualizarSessao(APIUsuarioToken apiUsuario, bool salvaToken = false)
        {
            LogUtil.Debug(string.Format("##Master.ExecuteAsync.AtualizandoSessionData## SESSION_DATA {0}", apiUsuario.DadosSession));

            var strSalvarToken = string.Empty;
            if (salvaToken)
            {
                strSalvarToken = " Token = @Token, ";
            }

            try
            {
                var query = string.Format(@"
UPDATE APIUsuarioToken 
SET 
    {0} 
    DadosSession = @DadosSession, 
    ClienteID = @ClienteID 
WHERE ID = @ID", strSalvarToken);

                conIngresso.Query(query, new
                {
                    apiUsuario.ID,
                    apiUsuario.DadosSession,
                    apiUsuario.Token,
                    apiUsuario.ClienteID
                });
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Master.ExecuteAsync.AtualizandoSessionData.EXCEPTION## SESSION_DATA {0}, MSG {1}", apiUsuario.DadosSession, ex.Message), ex);
                return false;
            }

            LogUtil.Debug(string.Format("##Master.ExecuteAsync.AtualizandoSessionData.SUCCESS## SESSION_DATA {0}", apiUsuario.DadosSession));
            return true;
        }

        public List<int> BuscarAPIUsuarioTokens(int clienteId)
        {
            LogUtil.Debug(string.Format("##APIUsuarioADO.BuscarAPIUsuarioToken## CLIENTEID {0}", clienteId));

            try
            {
                var queryStr = @"
SELECT ID
FROM APIUsuarioToken (NOLOCK)
  WHERE DataExpiracao > GETDATE() 
  AND DadosSession LIKE @cliente
ORDER BY ID DESC
";
                var cliente = string.Format("%:{0}%", clienteId);

                var result = conIngresso.Query<int>(queryStr, new
                {
                    cliente
                }).ToList();

                LogUtil.Debug(string.Format("##APIUsuarioADO.BuscarAPIUsuarioToken.SUCCESS## CLIENTEID {0}", clienteId));

                return result;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##APIUsuarioADO.BuscarAPIUsuarioToken.EXCEPTION## CLIENTEID {0}, MSG {1}", clienteId, ex.Message), ex);

                return new List<int>();
            }
        }

        public bool DesativarTokens(List<int> tokenIds)
        {
            LogUtil.Debug(string.Format("##APIUsuarioADO.DesativarTokens## TOKENIDS {0}", string.Join(", ", tokenIds)));

            try
            {
                var queryStr = @"
UPDATE APIUsuarioToken 
SET 
    Ativo = 0, 
    DataExpiracao = GETDATE() 
WHERE ID IN @tokenIds";

                var result = conIngresso.Execute(queryStr, new
                {
                    tokenIds
                });

                LogUtil.Debug(string.Format("##APIUsuarioADO.DesativarTokens.SUCCESS## TOKENIDS {0}", string.Join(", ", tokenIds)));

                return result == tokenIds.Count;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##APIUsuarioADO.DesativarTokens.EXCEPTION## TOKENIDS {0}, MSG {1}", string.Join(", ", tokenIds), ex.Message), ex);
                return false;
            }
        }

        public bool ExpirarToken(int ClienteID)
        {
            LogUtil.Debug(string.Format("##APIUsuarioADO.ExpirarToken## CLIENTEID {0}", ClienteID));

            try
            {
                var sql = @"UPDATE APIUsuarioToken SET Ativo = 0 WHERE ClienteID = @ClienteID AND Ativo = 1 AND DataExpiracao > GETDATE()";

                var response = conIngresso.Execute(sql, new { ClienteID });

                LogUtil.Debug(string.Format("##APIUsuarioADO.ExpirarToken.SUCCESS## CLIENTEID {0}", ClienteID));

                return response > 0;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##APIUsuarioADO.ExpirarToken.EXCEPTION## CLIENTEID {0}, MSG {1}", ClienteID, ex.Message), ex);
                return false;
            }
        }
    }
}
