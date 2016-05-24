namespace IRCore.DataAccess.Model
{
    using System.Collections.Generic;

    public partial class PontoVenda
    {
        public PontoVenda()
        {
            this.PontoVendaHorario = new HashSet<PontoVendaHorario>();
            this.PontoVendaXFormaPgto = new HashSet<PontoVendaXFormaPgto>();
        }

        public int ID { get; set; }

        public int IR_PontoVendaID { get; set; }

        public string Local { get; set; }

        public string Nome { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Compl { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Bairro { get; set; }

        public string Obs { get; set; }

        public string Referencia { get; set; }

        public string CEP { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string PermiteRetirada { get; set; }

        public virtual ICollection<PontoVendaHorario> PontoVendaHorario { get; set; }

        public virtual ICollection<PontoVendaXFormaPgto> PontoVendaXFormaPgto { get; set; }
    }
}
