using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaPrecoReservaSite
    {
        private int iD;
        private int gerenciamentoIngressosID;
        private decimal valor;
        private int quantidade;
        private string precoNome;
        private int cotaItemID;
        private int cotaItemIDAPS;
        private int quantidadeMapa;
        private string codigoProgramacao { get; set; }

        public EstruturaPrecoReservaSite() { }

        public EstruturaPrecoReservaSite(int id, decimal valor, int quantidade, string preconome, int cotaitemID, int quantidademapa, int cotaitemIDAPS, string codigoProgramacao, int gerenciamentoIngressosID)
        {
            this.gerenciamentoIngressosID = gerenciamentoIngressosID;
            this.quantidade = quantidade;
            this.valor = valor;
            this.iD = id;
            this.precoNome = preconome;
            this.cotaItemID = cotaitemID;
            this.quantidadeMapa = quantidademapa;
            this.cotaItemIDAPS = cotaitemIDAPS;
            this.codigoProgramacao = codigoProgramacao;
        }

        public EstruturaPrecoReservaSite(int id, decimal valor, int quantidade, string preconome, int cotaitemID, int quantidademapa, int cotaitemIDAPS, string codigoProgramacao)
        {
           
            this.quantidade = quantidade;
            this.valor = valor;
            this.iD = id;
            this.precoNome = preconome;
            this.cotaItemID = cotaitemID;
            this.quantidadeMapa = quantidademapa;
            this.cotaItemIDAPS = cotaitemIDAPS;
            this.codigoProgramacao = codigoProgramacao;
        }

        public EstruturaPrecoReservaSite(int id, decimal valor, int quantidade, string preconome, int cotaitemID, int quantidademapa, int cotaitemIDAPS)
        {
            this.quantidade = quantidade;
            this.valor = valor;
            this.iD = id;
            this.precoNome = preconome;
            this.cotaItemID = cotaitemID;
            this.quantidadeMapa = quantidademapa;
            this.cotaItemIDAPS = cotaitemIDAPS;
        }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public int GerenciamentoIngressosID
        {
            get { return gerenciamentoIngressosID; }
            set { gerenciamentoIngressosID = value; }
        }
        
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }
        public string PrecoNome
        {
            get { return precoNome; }
            set { precoNome = value; }
        }
        public int CotaItemID
        {
            get { return cotaItemID; }
            set { cotaItemID = value; }
        }
        public int QuantidadeMapa
        {
            get { return quantidadeMapa; }
            set { quantidadeMapa = value; }
        }
        public int CotaItemIDAPS
        {
            get { return cotaItemIDAPS; }
            set { cotaItemIDAPS = value; }
        }

        public int QuantidadeReservar()
        {
            return Quantidade * Math.Max(QuantidadeMapa, 1);
        }

        public string CodigoProgramacao { get; set; }

        public string CodigoCinema { get; set; }

        public bool PossuiTaxaProcessamento { get; set; }
    }
}
