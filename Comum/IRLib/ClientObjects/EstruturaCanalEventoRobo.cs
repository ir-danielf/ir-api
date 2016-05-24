using System.Collections.Generic;
using System.Linq;


namespace IRLib.ClientObjects
{
    public class EstruturaCanalEventoRobo
    {
        public int ID { get; set; }
        public int _EventoID { get; set; }
        public bool _isFilme { get; set; }
        public string[] _CanaisDistribuicaoID { get; set; }
        public int _UsuarioLogadoID { get; set; }
    }


    public class EstruturaFiltroEventoRobo
    {
        public int ID { get; set; }
        public List<string> Incluir { get; set; }
        public List<string> Excluir { get; set; }

        public bool isFilme { get; set; }

        public EstruturaFiltroEventoRobo()
        {
            Incluir = new List<string>();
            Excluir = new List<string>();
        }

        public List<string> FiltrarExclusoes()
        {
            return Incluir.Where(x => !Excluir.Contains(x)).Distinct().ToList();
        }
    }
}
