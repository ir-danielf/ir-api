using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IRCore.BusinessObject
{
    public class ParceiroBO : MasterBO<ParceiroADO>
    {
        public ParceiroBO(MasterADOBase ado = null) : base(ado) { }

        /// <summary>
        /// Método que consulta um Parceiro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tParceiro Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public List<tBloqueio> BloqueiosParceiro(int ParceiroMediaID,string LocaisID)
        {

            return new ParceiroADO().BloqueiosParceiro(ParceiroMediaID, LocaisID);
        }

        public void InserirLocalParceiroMidia(string locaisIds, int parceiroMediaID, int usuarioID)
        {
            ado.InserirLocalParceiroMidia(locaisIds, parceiroMediaID, usuarioID);
        }
        public void InserirBloqueioParceiroMidia(string bloqueiosIds, int parceiroMediaID, int usuarioID)
        {
            ado.InserirBloqueioParceiroMidia(bloqueiosIds, parceiroMediaID, usuarioID);
        }

    }
}
