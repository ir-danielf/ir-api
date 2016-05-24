using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class ParceiroMidiaModelQuery
    {
        public ParceiroMidia parceiroMedia { get; set; }

        public List<ParceiroMidiaApresentacaoSetorModelQuery> apresentacaoSetorIDs { get; set; }

        public tEmpresa empresa { get; set; }

    }


    public class ParceiroMidiaApresentacaoSetorModelQuery
    {
        public int apresentacaoID { get; set; }

        public int setorID { get; set; }

    }


    public static class ParceiroMidiaExtensionQuery
    {
        public static ParceiroMidia toParceiroMidia(this ParceiroMidiaModelQuery parceiroMidia)
        {
            parceiroMidia.parceiroMedia.ApresentacaoSetorIDs = parceiroMidia.apresentacaoSetorIDs.Select(t => new Setor()
            {
                IR_SetorID = t.setorID,
                ApresentacaoID = t.apresentacaoID
            }).ToList();


            parceiroMidia.parceiroMedia.tEmpresa = parceiroMidia.empresa;

            return parceiroMidia.parceiroMedia;
        }


    }
}
