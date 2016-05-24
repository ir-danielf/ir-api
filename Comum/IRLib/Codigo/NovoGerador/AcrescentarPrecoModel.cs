using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.NovoGerador
{
    [Serializable]
    public class AcrescentarPrecoModel
    {

        public List<SetorPrecoModel> setores { get; set; }
        public List<ApresentacaoSetorPrecoModel> apresentacaoSetores { get; set; }

    }

    [Serializable]
    public class ApresentacaoSetorPrecoModel
    {
        public ApresentacaoSetorPrecoModel()
        {
            this.Status = new StatusCriacao();
        }

        public int PrecoID { get; set; }
        public int ApresentacaoSetorID { get; set; }

        public int ApresentacaoID { get; set; }

        public string ApresentacaNome { get; set; }

        public int SetorID { get; set; }

        public string SetorNome { get; set; }

        public string PrecoNome { get; set; }

        public bool VendeIR { get; set; }

        public bool VendeCanaisProprios { get; set; }

        public bool ImprimeNome { get; set; }

        public bool ImprimeValor { get; set; }

        public decimal Valor { get; set; }

        public Preco.TipoImpressao Impressao
        {
            get
            {
                if (this.ImprimeNome && this.ImprimeValor)
                    return Preco.TipoImpressao.Ambos;
                else if (this.ImprimeNome)
                    return Preco.TipoImpressao.Nome;
                else
                    return Preco.TipoImpressao.Valor;
            }
        }

        public StatusCriacao Status { get; set; } 


        

    }
}

