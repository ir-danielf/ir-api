using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaArea
    {
        public bool Incluir { get; set; }
        public string Nome { get; set; }
        public int ID { get; set; }
        public int ControleID { get; set; }
        public int EntregaAreaCepID { get; set; }
        public string CEPInicial { get; set; }
        public string CEPFinal { get; set; }
        public string Procedimento { get; set; }
        public List<EstruturaEntregaAreaCep> ListaCEP { get; set; }
        public List<int> Regionais { get; set; }
    }
}
