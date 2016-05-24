namespace IRLib.ClientObjects
{
    public class EstruturaPagamentoExterno
    {
        public int formaPagamentoID { get; set; }
        public decimal valor { get; set; }
        public int parcelas { get; set; }
        public string codigoAutorizacao { get; set; }
        public string NSU { get; set; }
        public string IP { get; set; }
        public string formaPagamento { get; set; }

        public static EstruturaPagamentoExterno Montar(int formaPagamentoID, decimal valor, int parcelas, string codigoAutorizacao, string NSU, string IP, string formaPagamento)
        {
            return new EstruturaPagamentoExterno
            {
                formaPagamentoID = formaPagamentoID,
                valor = valor,
                parcelas = parcelas,
                codigoAutorizacao = codigoAutorizacao,
                NSU = NSU,
                IP = IP,
                formaPagamento = formaPagamento
            };
        }
    }
}