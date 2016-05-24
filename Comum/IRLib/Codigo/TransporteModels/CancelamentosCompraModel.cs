using System;

namespace IRLib.Codigo.TransporteModels
{
    [Serializable]
    public class CancelamentosCompraModel
    {
        public int ID { get; set; }
        public string Senha { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Canal { get; set; }
        public string usuario { get; set; }


        public string Status { get; set; }

        public string Chamado { get; set; }

        public string stringCancel { get; set; }
    }
}
