using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaClienteCorreio
    {
        public int ClienteID { get; set; }
        public string Nome { get; set; }
        public string Logradouro { get; set; }
        public string NumeroResidencia { get; set; }
        public string ComplementoResidencia { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Cep { get; set; }
        public string CodigoRastreio { get; set; }
        public int CodigoServicoCorreio { get; set; }
        public int CodigoServicoPostagem { get; set; }

    }
}
