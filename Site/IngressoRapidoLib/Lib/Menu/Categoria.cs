using System.Collections.Generic;

namespace IngressoRapido.Lib.Menu
{
    public class Categoria
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public List<Genero> Generos { get; set; }
    }
}
