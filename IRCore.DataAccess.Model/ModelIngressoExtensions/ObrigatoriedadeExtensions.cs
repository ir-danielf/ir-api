namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tObrigatoriedade
    {
        public bool NomeAsBool
        {
            get
            {
                return Nome == "T";
            }
            set
            {
                Nome = value ? "T" : "F";
            }
        }
        public bool RGAsBool
        {
            get
            {
                return RG == "T";
            }
            set
            {
                RG = value ? "T" : "F";
            }
        }
        public bool CPFAsBool
        {
            get
            {
                return CPF == "T";
            }
            set
            {
                CPF = value ? "T" : "F";
            }
        }
        public bool TelefoneAsBool
        {
            get
            {
                return Telefone == "T";
            }
            set
            {
                Telefone = value ? "T" : "F";
            }
        }
        public bool TelefoneComercialAsBool
        {
            get
            {
                return TelefoneComercial == "T";
            }
            set
            {
                TelefoneComercial = value ? "T" : "F";
            }
        }
        public bool CelularAsBool
        {
            get
            {
                return Celular == "T";
            }
            set
            {
                Celular = value ? "T" : "F";
            }
        }
        public bool DataNascimentoAsBool
        {
            get
            {
                return DataNascimento == "T";
            }
            set
            {
               DataNascimento = value ? "T" : "F";
            }
        }
        public bool EmailAsBool
        {
            get
            {
                return Email == "T";
            }
            set
            {
                Email = value ? "T" : "F";
            }
        }
        public bool CEPEntregaAsBool
        {
            get
            {
                return CEPEntrega == "T";
            }
            set
            {
                CEPEntrega = value ? "T" : "F";
            }
        }
        public bool EnderecoEntregaAsBool
        {
            get
            {
                return EnderecoEntrega == "T";
            }
            set
            {
                EnderecoEntrega = value ? "T" : "F";
            }
        }
        public bool NumeroEntregaAsBool
        {
            get
            {
                return NumeroEntrega == "T";
            }
            set
            {
                NumeroEntrega = value ? "T" : "F";
            }
        }
        public bool ComplementoEntregaAsBool
        {
            get
            {
                return ComplementoEntrega == "T";
            }
            set
            {
                ComplementoEntrega = value ? "T" : "F";
            }
        }
        public bool BairroEntregaAsBool
        {
            get
            {
                return BairroEntrega == "T";
            }
            set
            {
                BairroEntrega = value ? "T" : "F";
            }
        }
        public bool CidadeEntregaAsBool
        {
            get
            {
                return CidadeEntrega == "T";
            }
            set
            {
                CidadeEntrega = value ? "T" : "F";
            }
        }
        public bool EstadoEntregaAsBool
        {
            get
            {
                return EstadoEntrega == "T";
            }
            set
            {
                EstadoEntrega = value ? "T" : "F";
            }
        }
        public bool CEPClienteAsBool
        {
            get
            {
                return CEPCliente == "T";
            }
            set
            {
                CEPCliente = value ? "T" : "F";
            }
        }
        public bool EnderecoClienteAsBool
        {
            get
            {
                return EnderecoCliente == "T";
            }
            set
            {
                EnderecoCliente = value ? "T" : "F";
            }
        }
        public bool NumeroClienteAsBool
        {
            get
            {
                return NumeroCliente == "T";
            }
            set
            {
                NumeroCliente = value ? "T" : "F";
            }
        }
        public bool ComplementoClienteAsBool
        {
            get
            {
                return ComplementoCliente == "T";
            }
            set
            {
                ComplementoCliente = value ? "T" : "F";
            }
        }
        public bool BairroClienteAsBool
        {
            get
            {
                return BairroCliente == "T";
            }
            set
            {
                BairroCliente = value ? "T" : "F";
            }
        }
        public bool CidadeClienteAsBool
        {
            get
            {
                return CidadeCliente == "T";
            }
            set
            {
                CidadeCliente = value ? "T" : "F";
            }
        }
        public bool EstadoClienteAsBool
        {
            get
            {
                return EstadoCliente == "T";
            }
            set
            {
                EstadoCliente = value ? "T" : "F";
            }
        }
        public bool NomeEntregaAsBool
        {
            get
            {
                return NomeEntrega == "T";
            }
            set
            {
                NomeEntrega = value ? "T" : "F";
            }
        }
        public bool CPFEntregaAsBool
        {
            get
            {
                return CPFEntrega == "T";
            }
            set
            {
                CPFEntrega = value ? "T" : "F";
            }
        }
        public bool RGEntregaAsBool
        {
            get
            {
                return RGEntrega == "T";
            }
            set
            {
                RGEntrega = value ? "T" : "F";
            }
        }
        public bool CPFResponsavelAsBool
        {
            get
            {
                return CPFResponsavel == "T";
            }
            set
            {
                CPFResponsavel = value ? "T" : "F";
            }
        }
        public bool NomeResponsavelAsBool
        {
            get
            {
                return CPFResponsavel == "T";
            }
            set
            {
                CPFResponsavel = value ? "T" : "F";
            }
        }
    }
}
