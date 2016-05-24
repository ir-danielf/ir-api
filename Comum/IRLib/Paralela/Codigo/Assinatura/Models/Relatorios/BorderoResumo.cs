using System;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class BorderoResumo
    {
        public BorderoResumo() { }
        public List<EstruturaPagantes> ListaPagantes = new List<EstruturaPagantes>();
        public List<EstruturaPagantesSubtotal> ListaPagantesSub = new List<EstruturaPagantesSubtotal>();
        public EstruturaPagantesSubtotal TotalPagantes = new EstruturaPagantesSubtotal();
        public List<EstruturaEstatisticaSetor> ListaEstatisticaSetor = new List<EstruturaEstatisticaSetor>();
        public string Assinatura;
        public string Local;
        public DateTime Emissao
        {
            get
            {
                return DateTime.Now;
            }
        }
    }

    public class EstruturaPagantes
    {
        public EstruturaPagantes() { }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public string Preco { get; set; }
        public string PrecoMedio
        {
            get
            {
                if (Quantidade > 0)
                {
                    var pm = Faturamento / Quantidade;

                    return pm.ToString("c");
                }
                else
                {
                    return "R$ 0,00";
                }
            }
        }
        public int Quantidade { get; set; }
        public decimal Faturamento { get; set; }
        public string FaturamentoValor
        {
            get
            {
                return Faturamento.ToString("c");
            }
        }

    }

    public class EstruturaPagantesSubtotal
    {
        public EstruturaPagantesSubtotal() { }

        public int SetorID { get; set; }
        public string PrecoMedio
        {
            get
            {
                if (Quantidade > 0)
                {
                    var pm = Faturamento / Quantidade;

                    return pm.ToString("c");
                }
                else
                {
                    return "R$ 0,00";
                }
            }
        }
        public int Quantidade { get; set; }
        public decimal Faturamento { get; set; }
        public string FaturamentoValor
        {
            get
            {
                return Faturamento.ToString("c");
            }
        }

    }

    public class EstruturaEstatisticaSetor
    {
        public EstruturaEstatisticaSetor() { }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public int Lotacao { get; set; }
        public int Pagantes { get; set; }
        public int Cortesia { get; set; }
        public string Ocupacao
        {
            get
            {
                var porcentagem = ((Pagantes + Cortesia) * 100) / Lotacao;
                return porcentagem.ToString();
            }
        }

    }


}
