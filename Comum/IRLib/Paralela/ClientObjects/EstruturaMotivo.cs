using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaMotivo
    {
        public int ID { get; set; }
        public string motivo { get; set; }
        public string tipo { get; set; }
        public bool EnviaEmailCliente { get; set; }
        public string ArquivoEmail { get; set; }
    }
}
