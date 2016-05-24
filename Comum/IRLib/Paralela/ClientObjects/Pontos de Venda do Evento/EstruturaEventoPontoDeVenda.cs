using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEventoPontoDeVenda
    {
        public int ID { get; set; }
        public int EventoID { get; set; }
        public int PontoDeVendaID { get; set; }
        public bool Disponivel { get; set; }
        public string PontoDeVenda { get; set; }
        
    }
}
