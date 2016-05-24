using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEntrega
    {
        public bool Incluir { get; set; }
        
        public int ID { get; set; }
        
        public string Nome { get; set; }
        
        public int PrazoEntrega { get; set; }
        
        public string PrazoExibicao
        {
            get
            {
                switch (Tipo.ToString())
                {
                    case Entrega.RETIRADA:
                    case Entrega.AGENDADA:
                        return string.Format("A partir de {0} dias", this.PrazoEntrega);
                    case Entrega.NORMAL:
                        return string.Format("Em até {0} dias", this.PrazoEntrega);
                    case Entrega.RETIRADABILHETERIA:
                        return " -- ";
                    default:
                        return string.Empty;
                }
            }
        }
        
        public bool Disponivel { get; set; }
        
        public string ProcedimentoEntrega { get; set; }
        
        public bool EnviaAlerta { get; set; }
        
        public bool Padrao { get; set; }
        
        public bool PermitirImpressaoInternet { get; set; }
        
        public char Tipo { get; set; }
        
        public bool Ativo { get; set; }
        
        public int DiasTriagem { get; set; }
        
        public string Periodo { get; set; }
    }
}
