using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.TrocaIngresso
{
    [Serializable]
    public class EstruturaTrocaIngressoCompra
    {
        public int caixaID { get; set; }
        public int lojaID { get; set; }
        public int canalID { get; set; }
        public int empresaID { get; set; }
        public int usuarioID { get; set; }
        public bool imprimir { get; set; }
        public int pdvIdSelecionado { get; set; }
        public int clienteEnderecoId { get; set; }
        public int clienteID { get; set; }
        public int taxaEntregaID { get; set; }
        public int entregaControleID { get; set; }
        public int entregaAgendaID { get; set; }
        public decimal valorEntrega { get; set; }

        public decimal valorTotal { get; set; }

        public string NotaFiscalCliente { get; set; }

        public string NotaFiscalEstabelecimento { get; set; }

        public int IndiceInstituicaoTransacao { get; set; }

        public int IndiceTipoCartao { get; set; }

        public int NSUSitef { get; set; }

        public int NSUHost { get; set; }

        public int CodigoAutorizacaoCredito { get; set; }

        public int BIN { get; set; }

        public int ModalidadePagamentoCodigo { get; set; }
        public string ModalidadePagamentoTexto { get; set; }

        public List<EstruturaDonoIngresso> ListaDonoIngresso { get; set; }

        public DataTable pagamentos { get; set; }

        public string UsuarioLogin { get; set; }
        
        public decimal taxaConvenienciaValorTotal { get; set; }

        public string NomeCartao { get; set; }

        public string celular { get; set; }

        public decimal troco { get; set; }

        public bool AntiFraude { get; set; }

        public string MensagemRetorno { get; set; }

        public string HoraTransacao { get; set; }

        public string CodigoIR { get; set; }

        public string NumeroAutorizacao { get; set; }

        public string DadosConfirmacaoVenda { get; set; }

        public string Rede { get; set; }

        public string CodigoRespostaTransacao { get; set; }
    }
}
