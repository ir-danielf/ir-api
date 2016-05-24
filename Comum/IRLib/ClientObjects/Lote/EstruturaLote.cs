using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.ClientObjects.Lote
{
    [Serializable]
    public class EstruturaLote
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public int Quantidade { get; set; }
        public DateTime? DataLimite { get; set; }
        public int? LoteAnterior { get; set; }
        public int ApresentacaoSetorID { get; set; }

        public int UsuarioID { get; set; }
    }

    [Serializable]
    public class ControleLote
    {
        public int IdLote { get; set; }
        public int Versao { get; set; }
        public string Acao { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UsuarioID { get; set; }
    }
}
