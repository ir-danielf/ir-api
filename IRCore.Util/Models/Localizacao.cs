using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRCore.Util.Models
{
    public class Localizacao
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CidadePhonetized { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string EnderecoCompleto { get; set; }
        public bool Valido { get; set; }
    }
}