using System;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.DataAccess.Model
{
    public class InfoLeiMeia
    {
        public InfoLeiMeia()
        {
            this.temMeiaEntrada = true;
        }

        public string nomeEvento { get; set; }

        public List<Aprentacao> apresentacoes { get; set; }

        public int localId { get; set; }

        public bool temMeiaEntrada { get; set; }

        public class Aprentacao
        {
            public int id { get; set; }

            public DateTime dataApresentacao { get; set; }

            public int diaSemana { get; set; }

            public List<Setor> setores { get; set; }

            public int QtdeTotal
            {
                get { return this.setores.Sum(s => s.qtdTotalTickets); }
            }

            public int QtdeTotalMeia { get { return (int)Math.Round(this.QtdeTotal * 0.4, 0); } }

            public bool esgotado
            {
                get
                {
                    var totalIndisp = this.setores.Sum(s => s.qtdTotalMeiaIndisp);
                    return totalIndisp >= this.QtdeTotalMeia;
                }
            }

            public int cotaId { get; set; }
        }

        public class Setor
        {
            public int id { get; set; }

            public string nomeSetor { get; set; }

            public int qtdTotalTickets { get; set; }
            public int qtdTotalMeiaIndisp { get; set; }
        }
    }
}
