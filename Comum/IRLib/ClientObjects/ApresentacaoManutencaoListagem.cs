using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class ApresentacaoManutencaoListagem
    {
        public int ID;
        public bool DisponivelAjuste;
        public bool DisponivelRelatorio;
        public bool DisponivelVenda;
        public bool Cancelada;

        public DateTime Horario { get; set; }
        public int MapaEsquematico { get; set; }
        public string Impressao { get; set; }
    }
}
