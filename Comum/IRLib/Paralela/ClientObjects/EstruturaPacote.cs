using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaPacote
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private bool permitircancelamentoavulso;
        public bool PermitirCancelamentoAvulso
        {
            get { return permitircancelamentoavulso; }
            set { permitircancelamentoavulso = value; }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private int nomenclaturapacoteid;
        public int NomenclaturaPacoteID
        {
            get { return nomenclaturapacoteid; }
            set { nomenclaturapacoteid = value; }
        }

        private int localid;
        public int LocalID
        {
            get { return localid; }
            set { localid = value; }
        }

        private int quantidade;
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private string observacao;
        public string Observacao
        {
            get { return observacao; }
            set { observacao = value; }
        }

        private bool irvende;
        public bool IRVende
        {
            get { return irvende; }
            set { irvende = value; }
        }

        private List<EstruturaPacotePreco> pacotePreco;
        public List<EstruturaPacotePreco> Precos
        {
            get { return pacotePreco; }
            set { pacotePreco = value; }
        }

        private List<EstruturaPacoteItem> pacoteItem;
        public List<EstruturaPacoteItem> Itens
        {
            get { return pacoteItem; }
            set { pacoteItem = value; }
        }

        private List<EstruturaPacoteCanal> pacoteCanal;
        public List<EstruturaPacoteCanal> Canais
        {
            get { return pacoteCanal; }
            set { pacoteCanal = value; }
        }

    }

    [Serializable]
    public class EstruturaPacotePreco
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

        private int corid;
        public int CorID
        {
            get { return corid; }
            set { corid = value; }
        }

        private string cornome;
        public string CorNome
        {
            get { return cornome; }
            set { cornome = value; }
        }

        private string corrgb;
        public string CorRGB
        {
            get { return corrgb; }
            set { corrgb = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
    }

    [Serializable]
    public class EstruturaPacoteItem
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int eventoid;
        public int EventoID
        {
            get { return eventoid; }
            set { eventoid = value; }
        }

        private string eventonome;
        public string EventoNome
        {
            get { return eventonome; }
            set { eventonome = value; }
        }

        private int apresentacaoid;
        public int ApresentacaoID
        {
            get { return apresentacaoid; }
            set { apresentacaoid = value; }
        }

        private string apresentacaohorario;
        public string ApresentacaoHorario
        {
            get { return apresentacaohorario; }
            set { apresentacaohorario = value; }
        }

        private int setorid;
        public int SetorID
        {
            get { return setorid; }
            set { setorid = value; }
        }

        private string setornome;
        public string SetorNome
        {
            get { return setornome; }
            set { setornome = value; }
        }

        private string setorlugarmarcado;
        public string SetorLugarMarcado
        {
            get { return setorlugarmarcado; }
            set { setorlugarmarcado = value; }
        }

        private int apresentacaosetorid;
        public int ApresentacaoSetorID
        {
            get { return apresentacaosetorid; }
            set { apresentacaosetorid = value; }
        }

        private int precoid;
        public int PrecoID
        {
            get { return precoid; }
            set { precoid = value; }
        }

        private string preconome;
        public string PrecoNome
        {
            get { return preconome; }
            set { preconome = value; }
        }

        private decimal precovalor;
        public decimal PrecoValor
        {
            get { return precovalor; }
            set { precovalor = value; }
        }

        private int cortesiaid;
        public int CortesiaID
        {
            get { return cortesiaid; }
            set { cortesiaid = value; }
        }

        private string cortesianome;
        public string CortesiaNome
        {
            get { return cortesianome; }
            set { cortesianome = value; }
        }

        private int qtde;
        public int Quantidade
        {
            get { return qtde; }
            set { qtde = value; }
        }
    }

    [Serializable]
    public class EstruturaPacoteCanal
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int canalid;
        public int CanalID
        {
            get { return canalid; }
            set { canalid = value; }
        }

        private string canalnome;
        public string CanalNome
        {
            get { return canalnome; }
            set { canalnome = value; }
        }

        private int qtde;
        public int Quantidade
        {
            get { return qtde; }
            set { qtde = value; }
        }

        private int taxaconveniencia;
        public int TaxaConveniencia
        {
            get { return taxaconveniencia; }
            set { taxaconveniencia = value; }
        }

        private decimal taxaminima;
        public decimal TaxaMinima
        {
            get { return taxaminima; }
            set { taxaminima = value; }
        }

        private decimal taxamaxima;
        public decimal TaxaMaxima
        {
            get { return taxamaxima; }
            set { taxamaxima = value; }
        }  

        private bool selecionado;
        public bool Selecionado
        {
            get { return selecionado; }
            set { selecionado = value; }
        }
    }
}
