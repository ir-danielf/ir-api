using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects.Arvore
{
    [Serializable]
    public class EstruturaSelecaoArvore
    {
        public int RegionalID { get; set; }
        public int EmpresaID { get; set; }
        public int LocalID { get; set; }
        public int EventoID { get; set; }
        public int ApresentacaoID { get; set; }
        public int SetorID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int PrecoID { get; set; }
    }

    [Serializable]
    public class EstruturaSelecaoArvoreCompleta
    {
        public int RegionalID { get; set; }
        public string Regional { get; set; }
        public int EmpresaID { get; set; }
        public string Empresa { get; set; }
        public int LocalID { get; set; }
        public string Local { get; set; }
        public int EventoID { get; set; }
        public string Evento { get; set; }
        public int ApresentacaoID { get; set; }
        public string Apresentacao { get; set; }
        public int SetorID { get; set; }
        public string Setor { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int PrecoID { get; set; }
        public string Preco { get; set; }
    }

    [Serializable]
    public class EstruturaSelecaoArvoreLoja
    {
        public int EmpresaID { get; set; }
        public string Empresa { get; set; }
        public int CanalID { get; set; }
        public string Canal { get; set; }
        public int LojaID { get; set; }
        public string Loja { get; set; }
    }

    [Serializable]
    public class EstruturaSelecaoArvoreValeIngresso
    {
        public int ValeIngressoTipoID { get; set; }
        public string Empresa { get; set; }
        public string ValeIngressoTipo { get; set; }
    }

    [Serializable]
    public class EstruturaBuscaImpressaoEmLote
    {
        public EstruturaBuscaImpressaoEmLote()
        {
            IDs = new List<int>();
            Lojas = new List<int>();
            Areas = new List<int>();
            Taxas = new List<int>();
        }
        //Tab 1
        public string LetraInicial { get; set; }
        public List<int> IDs { get; set; } //ApresentacaoID ou ValeIngressoTipoID
        public List<int> Lojas { get; set; }
        public bool OrdernacaoAlfabetica { get; set; }
        public bool OrdernacaoData { get; set; }
        public bool OrdenacaoEventoProximo { get; set; }
        public EstruturaTipoImpressao.TipoImpressao TipoImpressao { get; set; }

        //Tab 2
        public int PeriodoID { get; set; }
        public bool PesquisarData { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public List<int> Areas { get; set; }
        public bool SomenteAgendadas { get; set; }
        public List<int> Taxas { get; set; }
        public int ParceiroMidiaID { get; set; }

    }
}
