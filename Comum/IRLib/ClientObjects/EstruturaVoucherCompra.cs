using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaVoucherCompra
    {
        public int VendaBilheteriaID;
        public DateTime DataCompra;
        public string Senha;
        public string Operador;
        public string Loja;
        public List<EstruturaIDNome> FormasPagamento;
        public string Cartao;
        public string BIN;
        public string PoliticaTroca;
        public string ProcedimentoRetiradaNome;
        public string ProcedimentoRetiradaDescricao;
        public decimal TotaisIngressos;
        public decimal TotaisTaxaConveniencia;
        public decimal TotaisTaxaEntrega;
        public decimal TotaisGeral;
        
        public List<EstruturaIngressoVoucherCompra> Ingressos;
        public EstruturaCliente Cliente;

    }
    [Serializable]
    public class EstruturaIngressoVoucherCompra
    {
        public string Local;
        public string EventoPacote;        
        public DateTime Data;
        public string Setor;
        public decimal Preco;
        public string PrecoNome;
        public decimal TaxaConveniencia;
        public string LugarAcentoCodigo;
    }    
}
