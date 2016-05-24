namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tCliente
    {
        public List<tClienteEndereco> EnderecoList { get; set; }

        public DateTime? DataNascimentoAsDateTime
        {
            get 
            {
                if (string.IsNullOrEmpty(DataNascimento))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return DateTime.ParseExact(DataNascimento, "yyyyMMdd", CultureInfo.InvariantCulture); 
                    }
                    catch
                    {
                        return null;
                    }
                    
                }
            }
            set 
            { 
                if(value == null)
                {
                    DataNascimento = null;
                }
                else
                {
                    DataNascimento = value.Value.ToString("yyyyMMdd"); 
                }
            } 
        }

        public bool RecebeEmailAsBool
        {
            get { return (RecebeEmail != "F"); }
            set { RecebeEmail = (value) ? "T" : "F"; } 
        }

        public bool AtivoAsBool
        {
            get { return (Ativo == "T"); }
            set { Ativo = (value) ? "T" : "F"; }
        }

        //public enumClienteStatus StatusAtualAsEnum
        //{
        //    get { return (enumClienteStatus)StatusAtual[0]; }
        //    set { StatusAtual = ((char)value).ToString(); }
        //}

        private enumClienteStatus _statusAtualAsEnum { get; set; }

        public enumClienteStatus StatusAtualAsEnum
        {
            get
            {
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
    }
}