using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.TrocaIngresso
{
    public class EstruturaConfirmaTrocaIngresso
    {
        public EstruturaTrocaIngressoPreco DadosTrocaIngressoPreco { get; private set; }

        public EstruturaTrocaIngressoCredito DadosTrocaIngressoCredito { get; private set; }

        public EstruturaConfirmaTrocaIngresso(int modoTroca, EstruturaTrocaIngressoPreco dadosTrocaIngresso)
        {
            DadosTrocaIngressoPreco = dadosTrocaIngresso;
        }

        public EstruturaConfirmaTrocaIngresso(EstruturaTrocaIngressoCredito dadosTrocaIngresso)
        {
            DadosTrocaIngressoCredito = dadosTrocaIngresso;

        }

        public string DescricaoEntrega { get; set; }
    }
}
