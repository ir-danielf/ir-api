using System;

namespace IRLib.Paralela.ClientObjects.Assinaturas
{
    [Serializable]
    public class EstruturaAssinaturaTipo
    {
        public int AssinaturaTipoID { get; set; }
        public DateTime RenovacaoInicio { get; set; }
        public DateTime RenovacaoFim { get; set; }
        public DateTime TrocaPrioritariaInicio { get; set; }
        public DateTime TrocaPrioritariaFim { get; set; }
        public DateTime TrocaInicio { get; set; }
        public DateTime AquisicaoInicio { get; set; }
        public DateTime AquisicaoFim { get; set; }
    }
}
