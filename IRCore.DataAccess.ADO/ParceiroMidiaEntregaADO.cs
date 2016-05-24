using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ParceiroMidiaEntregaADO : MasterADO<dbIngresso>
    {
        public ParceiroMidiaEntregaADO(MasterADOBase ado = null) : base(ado, false) { }

        public bool Salvar(int parceiroID, int entregaID, char ativo = 'T', string texto = null)
        {
            string sql = @"INSERT INTO ParceiroMidiaEntrega
                                        (ParceiroMidiaID
                                        ,EntregaID
                                        ,Ativo
                                        ,Texto)
                                VALUES (@parceiroID, @entregaID, @Ativo, @Texto)";
            int result = conIngresso.Execute(sql, new
            {
                parceiroID = parceiroID,
                entregaID = entregaID,
                ativo = ativo,
                texto = texto
            });

            return result > 0;
        }

        public ParceiroMidiaEntrega Buscar(int parceiroMidiaID, int entregaID)
        {
            string sql = String.Format(@"SELECT * FROM ParceiroMidiaEntrega
                                WHERE ParceiroMidiaID = {0} AND EntregaID = {1}", parceiroMidiaID, entregaID);

            return conIngresso.Query<ParceiroMidiaEntrega>(sql).FirstOrDefault();
        }

        public ParceiroMidiaEntrega Consultar(int ID)
        {
            string sql = String.Format(@"SELECT * FROM ParceiroMidiaEntrega
                                WHERE ID = {0}", ID);

            return conIngresso.Query<ParceiroMidiaEntrega>(sql).FirstOrDefault();
        }

        public bool Incluir(ParceiroMidiaEntrega pme)
        {
            string sql = @"INSERT INTO ParceiroMidiaEntrega
                                        (ParceiroMidiaID
                                        ,EntregaID
                                        ,Ativo
                                        ,Texto)
                                VALUES (@parceiroID, @entregaID, @Ativo, @Texto)";

            int result = conIngresso.Execute(sql, new
            {
                parceiroID = pme.ParceiroMidiaID,
                entregaID = pme.EntregaID,
                ativo = pme.Ativo,
                texto = pme.Texto
            });

            return result > 0;
        }

        public bool Alterar(ParceiroMidiaEntrega pme)
        {
            string sql = @"UPDATE ParceiroMidiaEntrega
                           SET EntregaID = @entregaID,
                               Ativo = @Ativo,
                               Texto = @Texto
                           WHERE ID = @ID";

            int result = conIngresso.Execute(sql, new
            {
                parceiroID = pme.ParceiroMidiaID,
                entregaID = pme.EntregaID,
                ativo = pme.Ativo,
                texto = pme.Texto,
                ID = pme.ID
            });

            return result > 0;
        }
        public bool Remover(int parceiroID, int entregaID)
        {
            string sql = @"DELETE FROM ParceiroMidiaEntrega
                           WHERE parceiroMidiaID = @parceiroID and entregaID = @entregaID";

            int result = conIngresso.Execute(sql, new
            {
                parceiroID = parceiroID,
                entregaID = entregaID
            });

            return result > 0;
        }

        public List<tEntrega> ListarEntregas(int parceiroId)
        {
            var queryStr = @"SELECT e.ID, e.Nome,
                             pme.ID, pme.EntregaID, pme.ParceiroMidiaID, pme.Ativo, pme.Texto
                             FROM tEntrega (NOLOCK) e
                             LEFT JOIN ParceiroMidiaEntrega (NOLOCK) pme ON pme.EntregaID = e.ID AND pme.ParceiroMidiaID = @parceiroID";

            return conIngresso.Query<tEntrega, ParceiroMidiaEntrega, tEntrega>(queryStr, addResult, new
            {
                parceiroId = parceiroId
            }).ToList();
        }

        private tEntrega addResult(tEntrega entrega, ParceiroMidiaEntrega parceiroMidiaEntrega)
        {
            if (parceiroMidiaEntrega != null)
            {
                entrega.ParceiroMidiaEntrega = new List<ParceiroMidiaEntrega>();
                entrega.ParceiroMidiaEntrega.Add(parceiroMidiaEntrega);
            }
            return entrega;
        }

        public List<int> ListarEntregaRestricao(int ParceiroMidiaID)
        {
            string sql = @"SELECT EntregaID
                            FROM ParceiroMidiaEntrega (NOLOCK)
                            WHERE ParceiroMidiaID = @parceiroMidiaID
                            AND Ativo = 'F'";
            return conIngresso.Query<int>(sql, new
            {
                ParceiroMidiaID = ParceiroMidiaID
            }).ToList();
        }
    }
}