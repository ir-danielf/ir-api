using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaValeIngressoWizard
    {

        public EstruturaValeIngressoTipo ValeIngressoTipo { get; set; }
        //Canais distribuidos
        public List<EstruturaCanalValeIngresso> Canais { get; set; }
        //Quantidade para adicionar à tValeIngresso
        public int AdicionarQuantidade { get; set; }
        //Ação utilizada para distribuir os canais na hora de salvar o VIR
        public IRLib.Paralela.ValeIngressoTipo.EnumAcaoCanais acaoCanaisIR { get; set; }
        //Status da distribuição Call Center + Internet
        public bool DistribuidoCanalCallCenter { get; set; }
        public bool DistribuidoCanalInternet { get; set; }
        //Quantidades
        public int QtdeCanaisIRDistribuidos { get; set; }
        public int QtdeCanaisIRNaoDistribuidos { get; set; }
        public int QtdeValeIngressoDisponivel { get; set; }
    }
}
