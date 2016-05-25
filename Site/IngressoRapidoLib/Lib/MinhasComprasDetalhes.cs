using System;

namespace IngressoRapido.Lib
{
    public class MinhasComprasDetalhes
    {
        //Ingresso
        public string Local { get; set; }
        public string Evento { get; set; }
        public DateTime Apresentacao { get; set; }
        public string Setor { get; set; }
        public string NomePreco { get; set; }
        public string CodIngresso { get; set; }
        public int IngressoID { get; set; } 

        //Vale Ingresso
        public int ValeIngressoID { get; set; }
        public string NomeValeIngresso { get; set; }
        public DateTime ValidadeValeIngress { get; set; }
        public string CodigoDeTroca { get; set; }

        //Comum
        public string Status { get; set; }
        public int ClientID { get; set; }
        public string Senha { get; set; }
        public decimal Valor { get; set; }
        public decimal Conveniencia { get; set; }
        public string ILAcao { get; set; }
        public string IStatus { get; set; }
        public string VBStatus { get; set; }
        public int IVBID { get; set; }
        public int VBID { get; set; }



    }
}
