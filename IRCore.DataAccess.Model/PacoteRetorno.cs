using System.Collections.Generic;

namespace IRCore.DataAccess.Model
{
    public class PacoteRetorno
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal TaxaConveniencia { get; set; }
        public List<PacoteItem> PacoteItem { get; set; }
    }
}