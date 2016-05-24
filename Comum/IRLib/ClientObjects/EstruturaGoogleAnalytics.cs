using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaGoogleAnalytics
    {
        private int vendabilheteriaid;
        public int VendaBilheteriaID
        {
            get { return vendabilheteriaid; }
            set { vendabilheteriaid = value; }
        }

        private string valortotal;
        public string ValorTotal
        {
            get { return valortotal; }
            set { valortotal = value; }
        }

        private string valorentrega;
        public string ValorEntrega
        {
            get { return valorentrega; }
            set { valorentrega = value; }
        }

        private string valortaxa;
        public string ValorTaxa
        {
            get { return valortaxa; }
            set { valortaxa = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string pais;
        public string Pais
        {
            get { return pais; }
            set { pais = value; }
        }

        public List<EstruturaItemIngresso> ItensIngresso { get; set; }
        public List<EstruturaItemValeIngresso> ItensVale { get; set; }  
    }

    [Serializable]
    public class EstruturaItemIngresso
    {
        private int eventoid;
        public int EventoID
        {
            get { return eventoid; }
            set { eventoid = value; }
        }

        private string eventonome;
        public string EventoNome
        {
            get { return eventonome; }
            set { eventonome = value; }
        }

        private int ingressoid;
        public int IngressoID
        {
            get { return ingressoid; }
            set { ingressoid = value; }
        }

        private string valoringresso;
        public string ValorIngresso
        {
            get { return valoringresso; }
            set { valoringresso = value; }
        }

        private string ingressotipo;
        public string IngressoTipo
        {
            get { return ingressotipo; }
            set { ingressotipo = value; }
        }
    }

    [Serializable]
    public class EstruturaItemValeIngresso
    {

        private int valeingressoid;
        public int ValeIngressoID
        {
            get { return valeingressoid; }
            set { valeingressoid = value; }
        }

        private string valorvaleingresso;
        public string ValorValeIngresso
        {
            get { return valorvaleingresso; }
            set { valorvaleingresso = value; }
        }

        private string valeingressotipo;
        public string ValeIngressoTipo
        {
            get { return valeingressotipo; }
            set { valeingressotipo = value; }
        }
    }

}
