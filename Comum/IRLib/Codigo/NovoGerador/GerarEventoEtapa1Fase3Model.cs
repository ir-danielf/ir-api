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
    public class GerarEventoEtapa1Fase3Model
    {
        [Display(Name = "Setores")]
        public int  MapaEsquematicoId { get; set; }

        [Display(Name = "Setores")]
        [Required(ErrorMessage = "Você precisa ter pelo menos 1 setor com quantidade !")]
        public List<SetoresNovo> Setores { get; set; }

        //TODO: Trocar o pairvalue de setores pelo abaixo
        [Serializable]
        public class SetoresNovo : IGridPrecoModel
        {
            public SetoresNovo(int setorID, int qtd)
            {
                this.SetorID = setorID;
                this.Quantidade = qtd;
            }

            [StringLength(10)]
            public string NomeSetor { get; set; }

            public SetoresNovo() { }

            public int SetorID { get; set; }
            public int Quantidade { get; set; }


            public Setor.Tipo Tipo { get; set; }

            public int ApresentacaoID { get; set; }

            public int ApresentacaoSetorID { get; set; }

            public bool IngressosGerados { get; set; }

            public override string ToString()
            {
                return string.Format("SetorID={0}, Quantidade={1}, Tipo={2}, Gerou={3}", SetorID, Quantidade, Tipo, IngressosGerados);
            }
        }
       

    }
}
