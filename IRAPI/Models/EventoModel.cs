using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRAPI.Models
{
    public class LugarRequestModel
    {
        /// <summary>
        /// Id do ingresso
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Código do ingresso
        /// </summary>
        public string cd { get; set; }
        /// <summary>
        /// Status do Ingresso (D - Disponivel ; ND - Não Disponivel ; SA - Seu Assento) 
        /// </summary>
        public string st { get; set; }
        /// <summary>
        /// Tipo de lugar
        /// </summary>
        public string tp { get; set; }
        /// <summary>
        /// Total de Assentos
        /// </summary>
        public int tt { get; set; }
        /// <summary>
        /// Reserva de assentos
        /// </summary>
        public int rv { get; set; }
        /// <summary>
        /// Ponto x do local do assento
        /// </summary>
        public string px { get; set; }
        /// <summary>
        /// Ponto Y do local
        /// </summary>
        public int py { get; set; }
    }


    public class GetMapaAssentosRequestModel
    {
        /// <summary>
        /// Url do Mapa de assentos
        /// </summary>
        public string mp { get; set; }

        public object lugares { get; set; }
    }

    public class GetMapaEsquematicoResponseModel
    {
        public string urlMapa { get; set;}
        public List<GetMapaEsquematicoSetorModel> mapaSetor { get; set; }
    }

    public class GetMapaEsquematicoSetorModel
    {
        public string coordenadas{get;set;}
        public int setorID{get;set;}
    }


}