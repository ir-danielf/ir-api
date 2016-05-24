using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoDescricao
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string imagem;
        public string Imagem
        {
            get { return imagem; }
            set { imagem = value; }
        }

        private string local;
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string pais;
        public string Pais
        {
            get { return pais; }
            set { pais = value; }

        }

        private string categoria;
        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        private string genero;
        public string Genero
        {
            get { return genero; }
            set { genero = value; }
        }

        private string endereco;
        public string Endereco
        {
            get { return endereco; }
            set { endereco = value; }
        }

        private string cep;
        public string CEP
        {
            get { return cep; }
            set { cep = value; }
        }

        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        private string publicar;
        public string Publicar
        {
            get { return publicar; }
            set
            {
                switch (value)
                {
                    case "T":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.PublicadoParaVenda;
                        break;
                    case "S":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.PublicadoSemVenda;
                        break;
                    case "F":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.NaoPublicado;
                        break;
                }
                publicar = value;
            }
        }

        public IRLib.Evento.PublicarTipo PublicaoTipo { get; set; }

        private IRLib.Evento.SemVendaMotivo semVendaMotivo;
        public IRLib.Evento.SemVendaMotivo SemVendaMotivo
        {
            get { return semVendaMotivo; }
            set { semVendaMotivo = value; }
        }

        public IRLib.Evento.SemVendaMotivo NaoVendaMotivo
        {
            get { return semVendaMotivo; }
            set { semVendaMotivo = value; }
        }

        private string publicarsvm;
        public string PublicarSemVendaMotivo
        {
            get { return publicarsvm; }
            set
            {
                publicarsvm = value;
                switch (value)
                {
                    case "1":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaOnlineNaoDisponivel;
                        break;
                    case "2":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaSomenteCallCenter;
                        break;
                    case "3":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasEncerradas;
                        break;
                    case "4":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasNaoIniciadas;
                        break;
                    case "5":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelApenasParaPacotes;
                        break;
                    case "6":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelDeterminadaData;
                        break;
                    default:
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivel;
                        break;
                }
            }
        }

        private List<EstruturaApresentacaoMobile> apresentacoes;
        public List<EstruturaApresentacaoMobile> Apresentacoes
        {
            get { return apresentacoes; }
            set { apresentacoes = value; }
        }

        public List<EstruturaFormasPagamentoMobile> FormaDePagamentos { get; set; }

        public DateTime? NaoVendaDataInicio { get; set; }
    }

    [Serializable]
    public enum BandeirasMobile
    {
        Amex,
        Diners,
        RedecardCredito,
        VisaCredito,
        VisaElectron,
        HSBC,
        ItauDebito,
        Aura,
        HiperCard,
        PayPal
    }
    
    [Serializable]
    public class EstruturaFormasPagamentoMobile
    {
        public int Parcelas { get; set; }
        public BandeirasMobile Bandeira { get; set; }
    }

    [Serializable]
    public class EstruturaApresentacaoMobile
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime horario;
        public DateTime Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        private string horarioFormatado;
        public string HorarioFormatado
        {
            get { return horarioFormatado; }
            set { horarioFormatado = value; }
        }

        private bool usarEsquematico;
        public bool UsarEsquematico
        {
            get { return usarEsquematico; }
            set { usarEsquematico = value; }
        }

        private int ir_apresentacaoID;
        public int IR_ApresentacaoID
        {
            get { return ir_apresentacaoID; }
            set { ir_apresentacaoID = value; }
        }
    }
}
