using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Carrinho
    {
        public enumCarrinhoStatus StatusAsEnum
        {
            get 
            {
                char status = Status[0];
                if(Status == "VV")
                {
                    status = 'W';
                }
                return (enumCarrinhoStatus)status; 
            }
            set 
            { 
                Status = ((char)value).ToString();
                if (Status == "W")
                {
                    Status = "VV";
                }
            }
        }

        public DateTime TimeStampAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(TimeStamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { TimeStamp = value.ToString("yyyyMMddHHmmss"); }
        }

        public DateTime ApresentacaoDataHoraAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(ApresentacaoDataHora, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { ApresentacaoDataHora = value.ToString("yyyyMMddHHmmss"); }
        }



        public int FilmeID { get; set; }

        public string CodigoProgramacao { get; set; }

        public bool NVendeLugar { get; set; }
            
        public bool PossuiTaxaProcessamento { get; set; }

        public Setor SetorObject { get; set; }
            
        public Evento EventoObject { get; set; }

        public Apresentacao ApresentacaoObject { get; set; }

        public tCotaItem CotaItemObject { get; set; }

        public List<Preco> Precos { get; set; }

        public bool IngressoValidado { get; set; }

        public Local LocalEvento { get; set; }

        public string Documento { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        public string RazaoSocial{ get; set; }

        public string Alvara { get; set; }

        public string AVCB { get; set; }

        public string Lugar { get; set; }

    }
}
