using System;

namespace IRLib.Paralela.ClientObjects {
    [Serializable]
    public class EstruturaTransacoesResultado {
        public int VendaBilheteriaID { get; set; }
        public string Nome { get; set; }
        public int QuantidadeIngressos { get; set; }
        public string ValorTotal { get; set; }
        public string Cartao { get; set; }
        public int CartaoID { get; set; }
        public int Bandeira { get; set; }
        public string NomeBandeira { get; set; }
        public string ImagemBandeira { get; set; }
        public string Entrega { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public bool AguardandoVisible { get; set; }
        public bool AprovadoVisible { get; set; }
        public bool FraudeVisible { get; set; }
        public bool EmAnaliseVisible { get; set; }
        public int ComprasPorCartao { get; set; }
        public int ComprasQtdeIngressos { get; set; }
        public string CheckSumCartao { get; set; }
        public string Senha { get; set; }
        public string AguardandoArgument { get; set; }
        public string AprovadoArgument { get; set; }
        public string FraudeArgument { get; set; }
        public string EmAnaliseArgument { get; set; }
        public int NivelRisco { get; set; }
        public int TotalCompras { get; set; }
        public int TotalIngressos { get; set; }
        public string Motivo { get; set; }
        public int Score { get; set; }
        public int Pagina { get; set; }

        private string _retornoAccertify = string.Empty;

        public string RetornoAccertify
        {
            get
            {
                if (_retornoAccertify.Length > 0)
                    return _retornoAccertify;
                else
                    return "";
            }
            set
            {
                _retornoAccertify = value;
            }
        }
        
    }
}
