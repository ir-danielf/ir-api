using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotasDistribuir
    {
        public int LocalID { get; set; }
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int SetorID { get; set; }
        public int CotaID { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
    }
}
