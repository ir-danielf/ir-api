using CTLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    [Serializable]
    public class GerarEventoEtapa2Fase1Model
    {
        public GerarEventoEtapa2Fase1Model()
        {
            this.Precos = new List<PrecoVendaModel>();

        }
        [Display(Name = "Preços")] 
        [Required(ErrorMessage = "Pelo menos 1 preço é obrigatório !")]
        public List<PrecoVendaModel> Precos { get; set; }
    }

    [Serializable]
    public class PrecoVendaModel : IGridPrecoModel
    {
        public PrecoVendaModel()
        {
            this.Status = new StatusCriacao();
        }

        [Display(Name = "Nome do Preço")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Nome do Preço é obrigatório !")]
        public string Descricao { get; set; }

        public bool Principal { get; set; }

        public bool VendeIR { get; set; }

        public bool VendeCanaisProprios { get; set; }

        public bool ImprimeNome { get; set; }

        public bool ImprimeValor { get; set; }

        public List<SetorPrecoModel> SetorValores { get; set; }

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

    [Serializable]
    public class PrecoDistribuicao 
    {
        public PrecoDistribuicao(int precoID, bool ir, bool canaisProprios)
        {
            this.PrecoID = precoID;
            this.IR = ir;
            this.CanaisProprios = canaisProprios;
            this.StatusDistribuicaoIR = new StatusCriacao();
            this.StatusDistribuicaoPropria = new StatusCriacao();
        }
        public int PrecoID { get; set; }
        public bool IR { get; set; }
        public bool CanaisProprios { get; set; }
        public StatusCriacao StatusDistribuicaoIR { get; set; }
        public StatusCriacao StatusDistribuicaoPropria { get; set; }
        public StatusCriacao StatusCriacaoCodigoBarras { get; set; }

    }

    [Serializable]
    public class SetorPrecoModel 
    {
        public SetorPrecoModel()
        {
            this.Status = new StatusCriacao();
        }
        public int SetorID { get; set; }

        [StringLength(10)]
        public string SetorNome { get; set; }

        public decimal? Valor { get; set; }

        public StatusCriacao Status { get; set; }

        public int PrecoID { get; set; }

    }
}
