using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstrutruraPreReservaSite
    {
        private int localID;
        private string local;
        private int eventoID;
        private string evento;
        private DateTime apresentacao;
        private string setor;
        private int apresentacaoID;
        private int setorID;
        private char setorTipo;
        private int ingressoID;
        private int lugarID;
        private int classificacao;
        private int grupo;
        private string codigo;
        private int quantidade;

        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        private List<EstruturaMesaFechadaPreReservaSite> ingressosMesaFechada;

        public List<EstruturaMesaFechadaPreReservaSite> IngressosMesaFechada
        {
            get { return ingressosMesaFechada; }
            set { ingressosMesaFechada = value; }
        }

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        public int LugarID
        {
            get { return lugarID; }
            set { lugarID = value; }
        }


        public int Grupo
        {
            get { return grupo; }
            set { grupo = value; }
        }


        public int Classificacao
        {
            get { return classificacao; }
            set { classificacao = value; }
        }

        public int IngressoID
        {
            get { return ingressoID; }
            set { ingressoID = value; }
        }

        public int LocalID
        {
            get { return localID; }
            set { localID = value; }
        }
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        public int EventoID
        {
            get { return eventoID; }
            set { eventoID = value; }
        }
        public string Evento
        {
            get { return evento; }
            set { evento = value; }
        }
        public DateTime Apresentacao
        {
            get { return apresentacao; }
            set { apresentacao = value; }
        }
        public string Setor
        {
            get { return setor; }
            set { setor = value; }
        }
        public int ApresentacaoID
        {
            get { return apresentacaoID; }
            set { apresentacaoID = value; }
        }
        public int SetorID
        {
            get { return setorID; }
            set { setorID = value; }
        }
        public char SetorTipo
        {
            get { return setorTipo; }
            set { setorTipo = value; }
        }
    }
}
