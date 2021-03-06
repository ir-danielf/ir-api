﻿using IRLib.Paralela.Utils;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class TotalAssinaturasSerie
    {
        public TotalAssinaturasSerie() { }

        public string Assinatura { get; set; }
        public int Quantidade { get; set; }
        public IRLib.Paralela.Assinatura.EnumStatusVisual Status { get; set; }
        public string StatusDescricao
        {
            get
            {
                return Enums.GetDescription<IRLib.Paralela.Assinatura.EnumStatusVisual>(Status);
            }
        }
        public string Bloqueio { get; set; }
        public int BloqueioID { get; set; }
    }
}
