using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaSangria
    {
        public int CaixaID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string ValorEmReal
        {
            get
            {
                return Valor.ToString("c");
            }
        }
        public decimal Sangria { get; set; }
        public string Senha { get; set; }
        public int VendaBilheteriaID { get; set; }
    }

    [Serializable]
    public class EstruturaTelaSangria
    {
        public List<EstruturaSangria> Lista = new List<EstruturaSangria>();
        public decimal Total { get; set; }
        public string TotalEmReal
        {
            get
            {
                return Total.ToString("c");
            }
        }
    }

    [Serializable]
    public class EstruturaSangriaImpressao
    {
        public string Senha { get; set; }
        public decimal Valor { get; set; }
        public string Responsavel { get; set; }
        public string ValorEmReal
        {
            get
            {
                return Valor.ToString("c");
            }
        }
        public string Identificacao { get; set; }
        public string Usuario { get; set; }
        public string Motivo { get; set; }
        public string Evento { get; set; }
        public int EventoID { get; set; }
        public DateTime Data { get; set; }

    }
}
