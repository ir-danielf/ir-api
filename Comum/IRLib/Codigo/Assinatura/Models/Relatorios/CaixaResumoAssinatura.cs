using System.Collections.Generic;

namespace IRLib.Assinaturas.Models
{
    public class CaixaResumoAssinatura
    {
        public CaixaResumoAssinatura() { }
        public List<DetalheCaixaResumo> listaTotal = new List<DetalheCaixaResumo>();
        public List<int> listaCanal = new List<int>();
        public List<int> listaLoja = new List<int>();
        public List<int> listaAssinatura = new List<int>();

    }

    public class DetalheCaixaResumo {

        public int AssinaturaID { get; set; }
        public string AssinaturaNome { get; set; }
        public int CanalID { get; set; }
        public string CanalNome { get; set; }
        public int LojaID { get; set; }
        public string LojaNome { get; set; }
        public int QtdAssinatura { get; set; }
        public decimal ValorTotal { get; set; }
        public string FormaPagamento { get; set; }
        public int SetorID { get; set; }
        public string SetorNome { get; set; }

    }


}
