using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEnvioSMS
    {
        public string Nome { get; set; }
        public string Numero { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public decimal ValorTotal { get; set; }
        public string DataVenda { get; set; }
    }
}
