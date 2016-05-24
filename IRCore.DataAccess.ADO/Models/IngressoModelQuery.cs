using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class IngressoBloqueioModelQuery
    {
        public tIngresso ingresso { get; set; }

        public tBloqueio bloqueio { get; set; }

    }

    public static class IngressoExtensionQuery
    {
        public static tIngresso toIngresso(this IngressoBloqueioModelQuery ingressoQuery)
        {

            ingressoQuery.ingresso.Bloqueio = ingressoQuery.bloqueio;
            return ingressoQuery.ingresso;
        }

    }

}
