using Newtonsoft.Json;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class SerieItemLista : List<SerieItem>
    {

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
