using System;

namespace IRLib.Paralela.HammerHead
{
    public class EstruturaVenda
    {
        public EstruturaVenda()
        {
            //this.Cliente = new EstruturaCliente();
            //this.FormaPagamento = new EstruturaFormaPagamento();
            //this.Cartao = new EstruturaCartao();
        }
        public int ID { get; set; }
        public string Senha { get; set; }
        public decimal ValorTotal { get; set; }

        public EstruturaFormaPagamento FormaPagamento { get; set; }
        public EstruturaCartao Cartao { get; set; }
        public EstruturaCliente Cliente { get; set; }

        public string IP { get; set; }

        public DateTime DataVenda { get; set; }

        public string NumeroCelular { get; set; }

        public int Score { get; set; }

        public decimal TaxaProcessamento { get; set; }

        public override string ToString()
        {
            return this.Senha + " - " + this.Cliente.Nome;
        }
        public Enumeradores.RetornoAccertify RetornoAccertify { get; set; }

        public string AccertifyForceStatus { get; set; }
    }

    public class EstruturaCliente
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class EstruturaFormaPagamento
    {
        public int ID { get; set; }
        public int BandeiraID { get; set; }
        public int Parcelas { get; set; }
        public int VendaBilheteriaFormaPagamentoID { get; set; }
        public string NotaFiscal { get; set; }
        public string CodigoResposta { get; set; }
        public string NomeCartao { get; set; }
        public decimal Valor { get; set; }

        public string Bandeira { get; set; }
    }

    public class EstruturaCartao
    {
        public string NumeroCartao { get; set; }
        public string DataVencimento { get; set; }
        public string CodigoSeguranca { get; set; }

    }


}
