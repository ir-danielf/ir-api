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

namespace IRCore.DataAccess.ADO
{
    public class UsuarioADO : MasterADO<dbIngresso>
    {
        public UsuarioADO(MasterADOBase ado = null) : base(ado) { }


        public List<tUsuario> ListaUsuariosParceiroMidia(int parceiroMidiaID, int perfilID)
        {
            var sql = @"SELECT     
	                        u.ID
	                        , u.EmpresaID
	                        , u.Nome
	                        , u.Sexo
	                        , u.Email
	                        , u.Login
	                        , u.Senha
	                        , u.Status
	                        , u.Validade
	                        , u.ValidoDe
	                        , u.ValidoAte
	                        , u.CodigoTerminal
	                        , u.ParceiroMidiaAreaID
	                        ,CASE 
		                        WHEN u.ParceiroMidiaAreaID = 0 THEN '(TODOS)' 
		                        ELSE pma.Nome
                             END AS ParceiroMidiaAreaNome
                        FROM         tUsuario AS u WITH (nolock) INNER JOIN
                                              tEmpresa AS e WITH (nolock) ON u.EmpresaID = e.ID INNER JOIN
                                              ParceiroMidia AS pm WITH (nolock) ON pm.EmpresaID = e.ID INNER JOIN
                                              tPerfilEmpresa AS pe ON u.ID = pe.UsuarioID LEFT JOIN
                                              ParceiroMidiaArea AS pma ON u.ParceiroMidiaAreaID = pma.ID
                        WHERE     (pm.ID = @ParceiroMidiaId) AND (u.Status <> 'B') AND (pe.PerfilID = @PerfilID)
                        ORDER BY u.Nome";

            var result = conIngresso.Query<tUsuario>(sql, new
            {
                ParceiroMidiaId = parceiroMidiaID,
                PerfilID = perfilID
            }).ToList();

            return result;
        }

        public tUsuario Consultar(int usuarioId)
        {
            tUsuario result = null;
            
            string sqlUsuario = @"SELECT TOP 1 * FROM tUsuario WHERE ID = @usuarioID";
            string sqlEmpresa = @"SELECT TOP 1 * FROM tEmpresa WHERE ID = @empresaID";
            string sqlParceiroMidia = @"SELECT * FROM ParceiroMidia WHERE EmpresaID = @empresaID";

            result = conIngresso.Query<tUsuario>(sqlUsuario, new { usuarioID = usuarioId }).FirstOrDefault();

            result.tEmpresa = conIngresso.Query<tEmpresa>(sqlEmpresa, new { empresaID = result.EmpresaID }).FirstOrDefault();

            result.tEmpresa.ParceiroMidia = conIngresso.Query<ParceiroMidia>(sqlParceiroMidia, new { empresaID = result.EmpresaID }).ToList();

            //result = ConsultarQuery().Where(t => t.ID == usuarioId).AsNoTracking().FirstOrDefault();

            return result;
        }

        public IQueryable<tUsuario> ConsultarQuery()
        {
            return (from item in dbIngresso.tUsuario
                              .Include(i => i.tEmpresa)
                              .Include(i => i.tEmpresa.ParceiroMidia)
                          select item);
        }

        
        public tUsuario Consultar(string login)
        {
            tUsuario result = null;

            string sqlUsuario = @"SELECT TOP 1 * FROM tUsuario WHERE Login = @login";
            string sqlEmpresa = @"SELECT TOP 1 * FROM tEmpresa WHERE ID = @empresaID";
            string sqlParceiroMidia = @"SELECT * FROM ParceiroMidia WHERE EmpresaID = @empresaID";

            result = conIngresso.Query<tUsuario>(sqlUsuario, new { login = login }).FirstOrDefault();

            if(result != null){
                result.tEmpresa = conIngresso.Query<tEmpresa>(sqlEmpresa, new { empresaID = result.EmpresaID }).FirstOrDefault();
                if (result.tEmpresa!=null)
                {
                    result.tEmpresa.ParceiroMidia = conIngresso.Query<ParceiroMidia>(sqlParceiroMidia, new { empresaID = result.EmpresaID }).ToList();
                }
            }
            //result = ConsultarQuery().Where(t => t.Login == login).AsNoTracking().FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<tUsuario> Listar(int pageNumber, int pageSize, string busca = null, int empresaId = 0)
        {
            return ListarQuery(busca, empresaId).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="status"></param>
        /// <param name="busca"></param>
        /// <param name="parceiroMidiaId"></param>
        /// <returns></returns>
        private IQueryable<tUsuario> ListarQuery(string busca = null, int empresaId = 0)
        {

            IQueryable<tUsuario> result = ConsultarQuery();
            // Filtrar por busca
            if (!string.IsNullOrEmpty(busca))
            {
                result = result.Where(t => (t.Nome.Contains(busca))
                                       || (t.Email.Contains(busca))
                                       || (t.Login.Contains(busca))
                                       || (t.tEmpresa!=null && t.tEmpresa.Nome.Contains(busca)));

            }
            if(empresaId > 0)
            {
                result = result.Where(t => t.EmpresaID == empresaId);
            }

            result = result.OrderBy(t => t.Nome).OrderBy(t => t.Login);
            return result;
        }

    }
}
