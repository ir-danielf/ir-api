using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRegistroEntrega
    {
        public int VendaBilheteriaID { get; set; }
        public string Senha { get; set; }
        public string TaxaEntrega { get; set; }
        public DateTime DataVenda { get; set; }
        public string Cliente { get; set; }
        public string StatusVenda { get; set; }
        public string Email { get; set; }
    }
}
