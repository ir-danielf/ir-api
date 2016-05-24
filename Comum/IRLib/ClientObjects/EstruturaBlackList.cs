using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaBlackList
    {
        public int EventoID;
        public int ApresentacaoID;
        public int SetorID;
        public DateTime DataHoraInclusao;//Momento da Reimpressão, Cancelamento ou Leitura
        public DateTime DataHoraSincronizacao;
        public string CodigoBarra;
        public LeituraCodigo.CodigoResposta Motivo;//Motivo pelo qual está na black list
        public string MotivoCancelamentoReimprecao;
        public string Portaria;
        public int ColetorNumero;

    }
}
