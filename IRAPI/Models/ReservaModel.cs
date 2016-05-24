using IRCore.BusinessObject.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRAPI.Models
{
    public class ReservaIngressoRequestModel
    {
        public int precoId { get; set; }
        public int ingressoId { get; set; }
    }


    /// <summary>
    /// Model com informações de quandtidade e ID do pacote.
    /// </summary>
    public class ReservaPacoteQtdModel
    {
        /// <summary>
        /// Quantidade de pacotes.
        /// </summary>
        public int qtd { get; set; }
        /// <summary>
        /// ID do pacote
        /// </summary>
        public int pacoteID { get; set; }
    }

    /// <summary>
    /// Model para request de reserva de pacotes.
    /// </summary>
    public class ReservaPacoteRequestModel
    {
        /// <summary>
        /// Construtor que inicializa a propriedade limparReservaAnterior como false.
        /// </summary>
        public ReservaPacoteRequestModel()
        {
            limparReservaAnterior = false;
        }

        /// <summary>
        /// Lista com quantidade e ID dos pacotes a reservar.
        /// </summary>
        public List<ReservaPacoteQtdModel> pacotes { get; set; }

        /// <summary>
        /// Propriedade para definir se a API limpa ou não a reserva anterior feita pelo usuário.
        /// </summary>
        public bool limparReservaAnterior { get; set; }
    }

    public class ReservaValeIngressoRequestModel
    {
        public string codigo { get; set; }
    }

    public class ReservaIngressosQtdRquestModel
    {

        public ReservaIngressosQtdRquestModel()
        {
            limparReservaAnterior = false;
        }
        public int setorID { get; set; }
        public int apresentacaoID { get; set; }
        public List<ReservaPrecoQtdModel> precos { get; set; }

        public bool limparReservaAnterior { get; set; }

        public bool Validar()
        {
            return setorID != 0 && apresentacaoID != 0 && precos != null && precos.All(a => a.Validar());
        }
    }

    public class ReservaPrecoQtdModel
    {
        public int precoID { get; set; }
        public int quantidade { get; set; }

        public bool Validar()
        {
            return precoID != 0 && quantidade > 0;
        }
    }

    public class ReservaMesaAbertaListRequestModel
    {
        public ReservaMesaAbertaListRequestModel()
        {
            limparReservaAnterior = false;
        }
        public bool limparReservaAnterior { get; set; }
        public List<ReservaMesaAbertaRequestModel> mesas { get; set; }
    }

    public class ReservaMesaAbertaRequestModel
    {
        public int apresentacaoID { get; set; }
        public int quantidade { get; set; }
        public int lugarID { get; set; }
        public int precoID { get; set; }
    }

    public class ReservaMesaFechadaListRequestModel
    {
        public ReservaMesaFechadaListRequestModel()
        {
            limparReservaAnterior = false;
        }
        public bool limparReservaAnterior { get; set; }
        public List<ReservaMesaFechadaRequestModel> mesas { get; set; }
    }

    public class ReservaMesaFechadaRequestModel
    {
        public int apresentacaoID { get; set; }
        public int lugarID { get; set; }
        public int precoID { get; set; }
    }

    public class ReservaTrocaPrecoRequestModel
    {
        public int precoID { get; set; }
    }

    public class ReservaAumentaTempoRequestModel
    {
        public int minutos { get; set; }

    }


    public class ReservaEntregaRequestModel
    {
        public int entregaControleID { get; set; }
        public int clienteEnderecoID { get; set; }
        public int pdvID { get; set; }
    }

    public class ReservaCotaRequestModel
    {
        public int CarrinhoID { get; set; }
        public string CodigoPromocional { get; set; }
        public tDonoIngresso DonoIngresso { get; set; }
    }

    public class EntregaTempModel
    {
        public EnderecoTemp enderecoTemp { get; set; }
        public int pdvID { get; set; }
        public int entregaControleID { get; set; }
    }

    public class ReservaCepModel
    {
        public List<tEntregaControle> entregaControles { get; set; }
        public EnderecoTemp enderecoTemp { get; set; }
    }

    public class OsespIngresso
    {
        public int ingressoID { get; set; }
    }

    public class EnderecoTemp
    {

        public EnderecoTemp(tCEP cep)
        {
            CEP = cep.CEP;
            Endereco = cep.Endereco;
            Cidade = cep.CidadeNome;
            Estado = cep.EstadoSigla;
            Bairro = cep.Bairro;
        }

        public EnderecoTemp() { }

        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }

        public bool Validar()
        {
            return !(String.IsNullOrEmpty(CEP) && String.IsNullOrEmpty(Endereco) && String.IsNullOrEmpty(Numero) && String.IsNullOrEmpty(Cidade) && String.IsNullOrEmpty(Estado) && String.IsNullOrEmpty(Bairro));
        }

        /// <summary>
        /// Validar se tamanho da string é menor ou igual ao da tabela na base de dados
        /// </summary>
        /// <returns></returns>
        public bool TamanhoValidado()
        {
            return (CEP.ValidarTamanho(8) &&
                    Endereco.ValidarTamanho(60) &&
                    Numero.ValidarTamanho(10) &&
                    Cidade.ValidarTamanho(50) &&
                    Estado.ValidarTamanho(2) &&
                    Complemento.ValidarTamanho(20) &&
                    Bairro.ValidarTamanho(100));
        }
    }
}