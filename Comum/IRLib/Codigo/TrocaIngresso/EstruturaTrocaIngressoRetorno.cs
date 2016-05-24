using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IRLib.Codigo.TrocaIngresso
{
    [Serializable]
    public class EstruturaTrocaIngressoRetorno
    {
        public string SenhaTroca { get; set; }

        public int VendaBilheteriaIDTroca { get; set; }

        public DataTable tImpressao { get; set; }

        public string Mensagem { get; set; }

        public bool TrocaComSucesso { get; set; }

    }
}
