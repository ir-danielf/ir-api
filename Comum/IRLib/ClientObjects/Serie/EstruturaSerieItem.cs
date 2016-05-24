using System;
using System.Drawing;

namespace IRLib.ClientObjects.Serie
{
    [Serializable]
    public class EstruturaSerieItem
    {
        public Enumerators.TipoAcaoPreco Acao { get; set; }
        public Bitmap ImagemAcao { get; set; } //As Imagens são inseridas no Client!!!
        public int SerieItemID { get; set; }

        public int EventoID { get; set; }
        public string Evento { get; set; }

        public int ApresentacaoID { get; set; }
        public string Horario { get; set; }

        public int SetorID { get; set; }
        public string Setor { get; set; }


        public int ApresentacaoSetorID { get; set; }

        public int PrecoID { get; set; }
        public string Preco { get; set; }

        public string Valor { get; set; }
        public int CorID { get; set; }

        public bool Promocional { get; set; }

        public string PromocionalExibicao
        {
            get { return Promocional ? "Sim" : "Não"; }


        }
        public int QuantidadePorPromocional { get; set; }
    }
}
