using System;
using System.Collections.Generic;

namespace IRLib.Codigo.TransporteModels
{
    [Serializable]
    public class ReembolsoModel
    {
        public List<EstornoDadosCartaoCredito> ListaCartaoCredito { get; set; }
        public List<EstornoDadosDepositoBancario> ListaDepositoBancario { get; set; }
        public List<EstornoDadosDinheiro> ListaEstornoDinheiro { get; set; }
    }

    [Serializable]
    public class EstornoDadosCartaoCredito
    {
        public int ID { get; set; }
        public int IdVenda { get; set; }
        public string Bandeira { get; set; }
        public string Cartao { get; set; }
        public decimal Valor { get; set; }
        public string Cliente { get; set; }
        public string CPF { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

    }

    [Serializable]
    public class EstornoDadosDepositoBancario
    {
        public int ID { get; set; }
        public int IdVenda { get; set; }
        public DateTime DataDeposito { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public int Conta { get; set; }
        public decimal Valor { get; set; }
        public string CPF { get; set; }
        public string Cliente { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Email { get; set; }
        public string MotivoCancelamento { get; set; }
        public string Status { get; set; }
    }

    [Serializable]
    public class EstornoDadosDinheiro
    {
        public int ID { get; set; }
        public int IdVenda { get; set; }
        public string Cliente { get; set; }
        public string MotivoCancelamento { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
