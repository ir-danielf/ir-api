using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class ValeIngressoModelQuery
    {
        public tValeIngresso valeIngresso { get; set; }
        public tValeIngressoTipo valeIngressoTipo { get; set; }

    }

    public static class ValeIngressoExtensionQuery
    {
        public static tValeIngresso toValeIngresso(this ValeIngressoModelQuery valeIngressoQuery)
        {
            valeIngressoQuery.valeIngresso.ValeIngressoTipo = valeIngressoQuery.valeIngressoTipo;
            return valeIngressoQuery.valeIngresso;
        }

    }

}
