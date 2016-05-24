﻿using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstrutraIntegrantesIngresso
    {
        public int IntegranteID { get; set; }
        public string Nome { get; set; }
        public int QuantidadeIntegrantes { get; set; }
        public int QuantidadeMaximaIntegrantes { get; set; }
    }
}
