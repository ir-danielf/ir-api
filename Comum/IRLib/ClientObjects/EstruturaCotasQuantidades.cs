using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotasQuantidades
    {
        public int QuantidadeCota { get; set; }
        public int QuantidadeApresentacao { get; set; }
        public int QuantidadeApresentacaoSetor { get; set; }
    }
}
