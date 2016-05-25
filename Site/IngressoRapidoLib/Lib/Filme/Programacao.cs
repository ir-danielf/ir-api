using System.Collections.Generic;

namespace IngressoRapido.Lib.Filmes
{
    public class Programacao
    {
        public int LocalID { get; set; }
        public int Distancia { get; set; }
        public string Local { get; set; }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public List<Horarios> Horarios { get; set; }


    }

    public class Horarios
    {
        public string ProgramacaoID { get; set; }
        public string Horario { get; set; }
        public bool Disponivel { get; set; }
    }

    public class Preco
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }

        public decimal Valor { get; set; }

        public int ApresentacaoID { get; set; }

        public int SetorID { get; set; }
    }
}
