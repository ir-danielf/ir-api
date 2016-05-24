using System;

namespace IRLib.ClientObjects
{
    public class EstruturaListaRepasse
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int empresaid;
        public int EmpresaID
        {
            get { return empresaid; }
            set { empresaid = value; }
        }

        private int localid;
        public int LocalID
        {
            get { return localid; }
            set { localid = value; }
        }

        private int eventoid;
        public int EventoID
        {
            get { return eventoid; }
            set { eventoid = value; }
        }

        private string observacoes;
        public string Observacoes
        {
            get { return observacoes; }
            set { observacoes = value; }
        }

        private DateTime datacompetencia;
        public DateTime DataCompetencia
        {
            get { return datacompetencia; }
            set { datacompetencia = value; }
        }

        private DateTime dataregimecaixa;
        public DateTime DataRegimeCaixa
        {
            get { return dataregimecaixa; }
            set { dataregimecaixa = value; }
        }

        private DateTime datacriacao;
        public DateTime DataCriacao
        {
            get { return datacriacao; }
            set { datacriacao = value; }
        }

        private Repasse.eTipo tipo;
        public Repasse.eTipo Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private decimal valortotal;
        public decimal ValorTotal
        {
            get { return valortotal; }
            set { valortotal = value; }
        }
    }
}
