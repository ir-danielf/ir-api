
using System.Collections.Generic;
namespace IRLib.Codigo.Util
{
    public class ListaEtapasFase
    {
        public List<EtapasFases> lista()
        {
            var listaEtapas = new List<EtapasFases>();

            //Etapa Um
            listaEtapas.Add(new EtapasFases() { id = 1, etapa = 1, fase = 1, faseAtual = false });
            listaEtapas.Add(new EtapasFases() { id = 2, etapa = 1, fase = 2, faseAtual = false });
            listaEtapas.Add(new EtapasFases() { id = 3, etapa = 1, fase = 3, faseAtual = false });

            //Etapa Dois
            listaEtapas.Add(new EtapasFases() { id = 4, etapa = 2, fase = 1, faseAtual = false });

            //Etapa Tres
            listaEtapas.Add(new EtapasFases() { id = 5, etapa = 3, fase = 1, faseAtual = false });
            listaEtapas.Add(new EtapasFases() { id = 6, etapa = 3, fase = 2, faseAtual = false });

            return listaEtapas;
        }
    }

    public class EtapasFases
    {
        public int id { get; set; }
        public int etapa { get; set; }
        public int fase { get; set; }
        public bool faseAtual { get; set; }
    }
}
