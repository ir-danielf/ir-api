using IRCore.DataAccess.Model.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public static class ListEventoMidiaExtensions
    {

        public static Dictionary<enumEventoTipoMidiaTipo, Dictionary<string, List<string>>> ToDictionary(this IEnumerable<EventoMidia> lista)
        {
            return lista
                    .GroupBy(t => t.EventoTipoMidia.TipoAsEnum)
                    .ToDictionary<IGrouping<enumEventoTipoMidiaTipo, EventoMidia>, enumEventoTipoMidiaTipo, Dictionary<string, List<string>>>(
                        t => t.Key,
                        t => t.GroupBy(x => x.EventoTipoMidia.Chave)
                                .ToDictionary<IGrouping<string, EventoMidia>, string, List<string>>(
                                    x => x.Key,
                                    x => x.Select(z => z.Valor).ToList()
                                )
                    );
        }

    }
}
