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
    
    public partial class tCanal
    {
        public tCanal()
        {
            this.tLoja = new HashSet<tLoja>();
            this.tCanalPreco = new HashSet<tCanalPreco>();
        }
    
        public int ID { get; set; }
        public Nullable<int> EmpresaID { get; set; }
        public string Nome { get; set; }
        public string Comprovante { get; set; }
        public string Obs { get; set; }
        public Nullable<int> CanalTipoID { get; set; }
        public string ClientePresente { get; set; }
        public Nullable<int> TaxaConveniencia { get; set; }
        public string OpcaoImprimirSemPreco { get; set; }
        public string Cartao { get; set; }
        public string NaoCartao { get; set; }
        public Nullable<decimal> TaxaMinima { get; set; }
        public Nullable<decimal> TaxaMaxima { get; set; }
        public string ObrigaCadastroCliente { get; set; }
        public Nullable<int> TaxaComissao { get; set; }
        public Nullable<decimal> ComissaoMinima { get; set; }
        public Nullable<decimal> ComissaoMaxima { get; set; }
        public Nullable<int> Comissao { get; set; }
        public string ConfirmacaoPorEmail { get; set; }
        public string TipoVenda { get; set; }
        public string PoliticaTroca { get; set; }
        public int ComprovanteQuantidade { get; set; }
        public Nullable<int> ObrigatoriedadeID { get; set; }
        public string EnviaSms { get; set; }
        public string TEFF { get; set; }
        public string NroEstabelecimento { get; set; }
        public string SiglaTipo { get; set; }
        public string SiglaPagamento { get; set; }
        public Nullable<bool> ResponsabilidadeDinheiroCliente { get; set; }
        public string Responsavel { get; set; }
        public string Recolhimento { get; set; }
        public Nullable<bool> Sangria { get; set; }
        public Nullable<int> RepasseID { get; set; }
    
        public virtual tEmpresa tEmpresa { get; set; }
        public virtual ICollection<tLoja> tLoja { get; set; }
        public virtual ICollection<tCanalPreco> tCanalPreco { get; set; }
    }
}
