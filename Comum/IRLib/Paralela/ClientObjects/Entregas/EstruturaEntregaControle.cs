using System;
using System.Collections.Generic;
using System.Drawing;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaControle : ICloneable
    {
        public string NomeArea { get; set; }
        public string Manter { get; set; }
        public string NomeTaxa { get; set; }
        public string NomePeriodo { get; set; }
        public int ID { get; set; }
        public int EntregaID { get; set; }
        public int EntregaAreaID { get; set; }
        public int PeriodoID { get; set; }
        public int QuantidadeEntregas { get; set; }
        public decimal Valor { get; set; }
        public string UsarDiasTriagemPadrao { get; set; }
        public int DiasTriagem { get; set; }
        public string UsarProcedimentoEntregaPadrao { get; set; }
        public string ProcedimentoEntrega { get; set; }
        public bool Disponivel { get; set; }
        public Bitmap Status { get; set; }
        public List<int> ListaDiasDaSemana = new List<int>();
        public string DiasDaSemana { get; set; }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    public class EstruturaEntregaControleDias 
    {

        public List<int> ListaDiasDaSemana = new List<int>();
        public string DiasDaSemana { get; set; }


       
    }
}
