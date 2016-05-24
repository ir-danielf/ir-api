using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    /// <summary>
    /// id = id do ingresso
    /// cd = código do ingresso
    /// st = Status do assento podendo ser D para disponivel, ND para não disponivel e SA para Seu assento
    /// tp = tipo de lugar
    /// tt = total de assentos
    /// rv = assentos reservados
    /// px = ponto x do local do assento
    /// py = ponto y do local do assento
    /// pl = perspectiva lugar, imagem do setor
    /// </summary>
    public class MapaAcentoModel

    {
        public int id { set; get; }
        public string cd { set; get; }
        public string st { set; get; }
        public string tp { set; get; }
        public string pne { set; get; } // varia entre '' (não-pne), 'A' (acompanhante) e 'C' (cadeirante)
        public int? tt { set; get; }
        public int? rv { set; get; }
        public int? px { set; get; }
        public int? py { set; get; }
        public string bn { set; get; }
        public int bid { set; get; }
        public string pl { set; get; }
    }
}
