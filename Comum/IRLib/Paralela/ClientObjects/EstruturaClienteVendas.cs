using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
     public class EstruturaClienteVendas
    {
        private EstruturaCliente cliente;
        public EstruturaCliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        private int qtdevendas;
        public int QuantidadeVendas
        {
            get { return qtdevendas; }
            set { qtdevendas = value; }
        }

        private DateTime dataatualizacao;
        public DateTime DataAtualizacao
        {
            get { return dataatualizacao; }
            set { dataatualizacao = value; }
        }

        private bool possuiingressos;
        public bool PossuiIngressos
        {
            get { return possuiingressos; }
            set { possuiingressos = value; }
        }

        private List<EstruturaVenda> vendas;
        public List<EstruturaVenda> Vendas
        {
            get { return vendas; }
            set { vendas = value; }
        }
        
    }

    [Serializable]
    public class EstruturaVenda
    {
        private int vendabilheteriaid;
        public int VendaBilheteriaID
        {
            get { return vendabilheteriaid; }
            set { vendabilheteriaid = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        private DateTime data;
        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
