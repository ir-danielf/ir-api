using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class DataAgenda
    {
        public DateTime Dia { get; set; }
        public string DiaString { get { return this.Dia.ToString(); } }
        public int DiasTriagem { get; set; }
        public DateTime DataDoEvento { get; set; }
        public string DataDoEventoString { get { return this.DataDoEvento.ToString(); } }
        public List<PeridoAgenda> ListaPeriodo = new List<PeridoAgenda>();

    }

    [Serializable]
    public class PeridoAgenda
    {
        public string Periodo { get; set; }
        public int PeriodoID { get; set; }
        public int EntregaControleID { get; set; }
        public bool Disponivel { get; set; }
        public DateTime HoraInicial { get; set; }
        public string HoraInicialString { get { return this.HoraInicial.ToString(); } }
        public DateTime HoraFinal { get; set; }
        public string HoraFinalString { get { return this.HoraFinal.ToString(); } }
        public DateTime Data { get; set; }

    }

    [Serializable]
    public class DataPeriodoSelecionado
    {
        public DateTime Dia { get; set; }
        public string DiaString { get { return this.Dia.ToString(); } }
        public string Periodo { get; set; }
        public int PeriodoID { get; set; }
        public int EntregaControleID { get; set; }


    }



}
