//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class xEmpresa
    {
        public int ID { get; set; }
        public int Versao { get; set; }
        public string Nome { get; set; }
        public string ContatoNome { get; set; }
        public string ContatoCargo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string DDDTelefone { get; set; }
        public string Telefone { get; set; }
        public string DDDFax { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Obs { get; set; }
        public string EmpresaVende { get; set; }
        public string EmpresaPromove { get; set; }
        public Nullable<int> RegionalID { get; set; }
        public Nullable<decimal> TaxaMaximaEmpresa { get; set; }
        public string BannerPadraoSite { get; set; }
    }
}
