using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects.Serie
{


    [Serializable]
    public class EstruturaSerie : ICloneable
    {
        


        public int ID { get; set; }

        public int RegionalID { get; set; }
        public string Regional { get; set; }

        public int EmpresaID { get; set; }

        public int LocalID { get; set; }
        public string Local { get; set; }

        public string Titulo { get; set; }
        public string Nome { get; set; }


        public int QuantidadeMinimaGrupo { get; set; }
        public int QuantidadeMaximaGrupo { get; set; }

        public int QuantidadeMinimaApresentacao { get; set; }
        public int QuantidadeMaximaApresentacao { get; set; }

        public int QuantidadeMinimaIngressosPorApresentacao { get; set; }
        public int QuantidadeMaximaIngressosPorApresentacao { get; set; }

        public string Regras { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
    [Serializable]
    public class EstruturaPreenchimentoSerie : ICloneable
    {
        public EstruturaPreenchimentoSerie()
        {
            eCanaisIR = new EstruturaCanaisIR();
            lstPrecosDisponiveis = new List<EstruturaSerieItem>();
            lstCanaisDisponiveis = new List<EstruturaCanalSerie>();
            lstFormasPagamentoDisponiveis = new List<EstruturaFormaPagamentoSerie>();
        }

        public bool HabilitarEdicao { get; set; }
        public List<EstruturaSerieItem> lstPrecosDisponiveis { get; set; }
        public List<EstruturaCanalSerie> lstCanaisDisponiveis { get; set; }
        public List<EstruturaFormaPagamentoSerie> lstFormasPagamentoDisponiveis { get; set; }
        public EstruturaCanaisIR eCanaisIR { get; set; }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    [Serializable]
    public class EstruturaCanaisIR
    {
        public EstruturaCanaisIR()
        {
            this.Acao = Enumerators.TipoAcaoCanalIR.Manter;
        }
        public Enumerators.TipoAcaoCanalIR Acao { get; set; }
        public int QuantidadeDisponivelIR { get; set; }
        public int QuantidadeMaximaIR { get; set; }
        public bool DisponivelInternet { get; set; }
        public bool DisponivelCallcenter { get; set; }
    }

}
