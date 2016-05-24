using IRLib.Codigo.ModuloLogistica;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoVendaValeIngresso
    {
        public int ID { get; set; }
        public string Senha { get; set; }
        public int ClienteID { get; set; }
        public int ClienteEnderecoID { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteRG { get; set; }
        public string ClienteEmail { get; set; }
        public string Canal { get; set; }
        public decimal TaxaConvenienciaValorTotal { get; set; }
        public decimal ValorTotalValeIngressos { get; set; }
        public decimal ValorTotalEntrega { get; set; }
        public decimal ValorTotalVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime DataAberturaCaixa { get; set; }
        public char StatusVenda { get; set; }
        public int VendaBilheteriaID { get; set; }
        public string CanalVenda { get; set; }
        public int Quantidade { get; set; }
        public int ComprovanteQuantidade { get; set; }
        public string Vendedor { get; set; }
        public int EntregaControleID { get; set; }
        public string Loja { get; set; }
        public string TaxaEntrega { get; set; }
        public string TipoVenda { get; set; }
        public List<EstruturaImpressaoVir> EstruturaImpressaoVir { get; set; }
        public EstruturaRetornoVendaValeIngressoEntrega EstruturaEntregaVIR { get; set; }
        public bool Impresso { get; set; }
        public bool Imprimir { get; set; }
        public string ClienteStatus { get; set; }
        public int EntregaAgendaID { get; set; }
        public Enumeradores.TaxaEntregaTipo TaxaEntregaTipo { get; set; }
        public string EntregaNome { get; set; }
        public string PeriodoEntrega { get; set; }
        public string DataEntrega { get; set; }
        public string AreaEntrega { get; set; }
        public char ValorTipo { get; set; }
        public string TransactionID { get; set; }

        public string AgregadoNome { get; set; }
        public string AgregadoCPF { get; set; }
        public string AgregadoEmail { get; set; }
        public string AgregadoTelefone { get; set; }

        /// <summary>
        /// Monta e retorna a tabela para a exibição dos ingressos na impressão e reimpressão
        /// </summary>
        /// <returns></returns>
        public DataTable TabelaExibicaoImpressao()
        {
            try
            {
                DataTable retorno = new DataTable();
                retorno.Columns.Add("ValeIngressoID", typeof(int));
                retorno.Columns.Add("Nome", typeof(string));
                retorno.Columns.Add("Valor", typeof(decimal));
                retorno.Columns.Add("ClientePresenteado", typeof(string));

                DataRow linha;
                foreach (EstruturaImpressaoVir item in EstruturaImpressaoVir)
                {
                    linha = retorno.NewRow();
                    linha["ValeIngressoID"] = item.ValeIngressoID;
                    linha["Nome"] = item.ValeIngressoNome;
                    linha["Valor"] = item.Valor;
                    linha["ClientePresenteado"] = item.ClientePresenteado;
                    retorno.Rows.Add(linha);
                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }

    [Serializable]
    public class EstruturaRetornoVendaValeIngressoEntrega
    {
        public string Nome { get; set; }
        public string RG { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
