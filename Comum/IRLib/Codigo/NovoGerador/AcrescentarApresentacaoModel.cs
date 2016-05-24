using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.NovoGerador
{
    [Serializable]
    public class AcrescentarApresentacaoModel
    {
        public List<GerarEventoEtapa1Fase3Model.SetoresNovo> setores { get; set; }
        public List<PrecoVendaModel> precos { get; set; }

    }
}
