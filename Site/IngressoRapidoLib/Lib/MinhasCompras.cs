using System;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    [Serializable]
    public class MinhasCompras
    {
        public int ID { get; set; }

        public string Canal { get; set; }

        public string Data { get; set; }

        public string Senha { get; set; }

        public decimal ValorTotal { get; set; }

        public int ClientId { get; set; }

        public int VendaBilheteriaId { get; set; }

        public string Status { get; set; }

        public bool ImprimirInternet { get; set; }

        public List<IngressoImpressaoInternet> IngressoImpressaoInternet { get; set; }
    }
}
