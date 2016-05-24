
namespace IRLib.HammerHead
{
    public class Enumeradores
    {
        public enum RetornoProcessamento
        {
            Processado = 1,
            CartaoInvalido = 2,
            Timeout = 3,
            CancelarAccertify = 4,
            AguardarAccertify = 5,
            SolicitarDocumentos = 6,
            VendaCancelada = 7,
            CancelarSemFraude = 8,
            Chargeback = 9,
            VendaJaCancelada,
            Bypass
        }

        public enum TipoEntrada
        {
            Sucesso,
            Erro,
            Informacao,
            Alerta
        }

        public enum RetornoAccertify
        {
            Aceitar = 0,
            AguardarReview = 1,
            CancelarAltoRisco = 2,
            CancelarTempoLimiteExcedido = 3,
            CancelarVendaInvalida = 4,
            Indefinido = 5,
            AcompanhamentoComCliente = 6,
            CancelarSemFraude = 7,
            Bypass = 8,
            Chargeback = 9,
            VendaJaCancelada = 10
        }
		
        public enum Site
        {
            IngressoRapido = 1,
            Entretix = 2
        }		
    }
}
