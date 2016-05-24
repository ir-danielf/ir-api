    using System;
    using System.Globalization;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.DataAccess.Model
{
    public partial class Login
    {
        public tCliente Cliente { get; set; }

        public DateTime UltimoAcessoAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(UltimoAcesso, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { UltimoAcesso = value.ToString("yyyyMMddHHmmss"); } 
        }

        public DateTime DataCadastroAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(DataCadastro, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { DataCadastro = value.ToString("yyyyMMddHHmmss"); }
        }

        public bool AtivoAsBool 
        {
            get { return (string.IsNullOrEmpty(Ativo) || (Ativo == "T")); }
            set { Ativo = (value) ? "T" : "F"; } 
        }

        public enumClienteStatus StatusAtualAsEnum
        {

            get { 
                try
                {
                    return (enumClienteStatus)StatusAtual[0]; 
                }
                catch
                {
                    return enumClienteStatus.liberado;
                }
                
            }
            set { StatusAtual = ((char)value).ToString(); }
        }

        public string BiletoToken { get; set; }


        public string AccountKitAccessToken { get; set; }
    }
}