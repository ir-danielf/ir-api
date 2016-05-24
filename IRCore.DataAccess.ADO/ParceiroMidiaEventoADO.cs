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
using IRCore.DataAccess.ADO.Models;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ParceiroMidiaEventoADO : MasterADO<dbIngresso>
    {
        public ParceiroMidiaEventoADO(MasterADOBase ado = null) : base(ado, false) { }


        /// <summary>
        /// Retorna o ParceiroMidiaEvento apartir do evento e parceiro
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="parceiroID"></param>
        /// <returns></returns>
        public ParceiroMidiaEvento Consultar(int eventoID, int parceiroID)
        {
            string sql = @"SELECT ID,ParceiroMidiaID,EventoID,IngressosPorVoucher,PrazoLiberacao
                            FROM ParceiroMidiaEvento
                            where EventoID = @eventoID AND ParceiroMidiaID = @parceiroID";
            var query = conIngresso.Query<ParceiroMidiaEvento>(sql, new
            {
                eventoID = eventoID,
                parceiroID = parceiroID
            });

            return query.FirstOrDefault();
        }

    }
}
