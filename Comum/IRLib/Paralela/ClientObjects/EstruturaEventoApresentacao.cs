using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEventoApresentacao
    {
        public int EventoID { get; set; }
        public string EventoNome { get; set; }
        public string EventoTipo { get; set; }
        public string Horario { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Local { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public List<EstruturaEventoPrecos> EstruturaPreco = new List<EstruturaEventoPrecos>();
    }

    [Serializable]
    public class EstruturaEventoPrecos
    {
        public int EventoID { get; set; }
        public int PrecoID { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}
