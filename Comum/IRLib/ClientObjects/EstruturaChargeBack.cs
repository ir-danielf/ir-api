using System;
using System.ComponentModel.DataAnnotations;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCarta
    {
        public int CartaID { get; set; }
        public string NumeroProcesso { get; set; }
        public string NroCartao { get; set; }

        [Required(ErrorMessage = "Informe o número do cartão.")]
        public string Cartao { get; set; }

        public string Cv { get; set; }

        [Required(ErrorMessage = "Informe a data da compra.")]
        public string DataCompra { get; set; }

        public string DataCarta { get; set; }

        [RegularExpression(@"^[0-9]{1,5}(\,[0-9]{0,2})?$", ErrorMessage = "Informe um valor válido...")]
        public string ValorChargeBack { get; set; }

        [RegularExpression(@"^[0-9]{1,5}(\,[0-9]{0,2})?$", ErrorMessage = "Informe um valor válido...")]
        public string ValorTransacao { get; set; }

        public string AcaoTomadaAF { get; set; }

        public string Origem { get; set; }

        public string Possibilidades { get; set; } //para chargebacks que possuem +1 possiblidade de registro

        public string Senha { get; set; }

        public EstruturaCarta() { }

        public EstruturaCarta(string texto)
        {
            var data = texto.Split(';');

            this.NumeroProcesso = data[0];
            this.NroCartao = data[1]; //Cartão mascarado
            this.Cartao = data[1]; //Hash cartão
            this.Cv = data[2];
            if (data[8] != "Carta")
            {
                this.DataCompra = data[3];
            }
            else
            {
                this.DataCompra = data[3].Substring(6, 4) + data[3].Substring(3, 2) + data[3].Substring(0, 2);
            }
            this.DataCarta = data[4];

            if (string.IsNullOrEmpty(data[5])) { this.ValorChargeBack = 0.ToString(); }
            else { this.ValorChargeBack = data[5]; }

            if (string.IsNullOrEmpty(data[6])) { this.ValorTransacao = 0.ToString(); }
            else { this.ValorTransacao = data[6]; }

            this.AcaoTomadaAF = data[7];
            this.Origem = data[8];

        }
    }

    [Serializable]
    public class EstruturaDetalheChargeBack
    {
        public int id { get; set; }
        public string Cliente { get; set; }
        public string StatusCliente { get; set; }
        public string DataVenda { get; set; }
        public string DataCarta { get; set; }
        public string Senha { get; set; }
        public string ValorTotal { get; set; }
        public string Cartao { get; set; }
        public string Nsuhost { get; set; }
        public string Canal { get; set; }
        public string NroCartao { get; set; }
    }
}
