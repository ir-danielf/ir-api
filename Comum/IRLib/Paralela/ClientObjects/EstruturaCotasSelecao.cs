﻿using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCotasSelecao
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
    }
}
