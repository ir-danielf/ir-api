using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public class Etapa1MantEventoModel
    {
        public string eventoNome {get;set;}
        public string fonteImposto {get;set;}
        public decimal percentImposto {get;set;}
        public string AVCBCodigo {get;set;}
        public DateTime AVCBEmissao {get;set;}
        public DateTime AVCBValidade { get; set; }
        public string alvaraCodigo {get;set;}
        public DateTime alvaraEmissao { get; set; }
        public int alvaraLotacao {get;set;}
        public DateTime alvaraValidade { get; set; }

        public object mapasEsquematico { get; set; }

        public string localNome { get; set; }

        public bool alvaraVenda { get; set; }

        public int generoID { get; set; }

        public System.Data.DataTable tabelaTipo { get; set; }

        public System.Data.DataTable tabelaSubtipo { get; set; }

        public int categoriaID { get; set; }

        public int mapaEsquematicoID { get; set; }

        public string ativo { get; set; }

    }

    public class Etapa2MantEventoModel
    {

    }

    public class Etapa3MantEventoModel
    {

        public string localNome { get; set; }

        public string borderoEnderecoProdutor { get; set; }

        public string borderoCpfCnpjProdutor { get; set; }

        public string borderoRazaoProdutor { get; set; }

        public string aberturaPortoes { get; set; }

        public string atencao { get; set; }

        public string duracaoEvento { get; set; }

        public string meiaEntrada { get; set; }

        public string PDVSemConveniencia { get; set; }

        public string promocoes { get; set; }

        public string retiradaIngresso { get; set; }
        public string censura { get; set; }

        public char obrigaCadastro { get; set; }
    }

    public class Etapa4MantEventoModel
    {
        public string resenha { get; set; }
        public Evento_B.imagemdestaque imagemDivulgacao { get; set; }

        public Evento_B.imageminternet imagemTicket { get; set; }
    }
}
