using System;

namespace IRLib.ClientObjects.Assinaturas
{
    [Serializable]
    public class EstruturaAssinaturaAcao
    {
        public string AssinaturaClienteID { get; set; }
        public string Acao { get; set; }
        public string PrecoTipo { get; set; }
        public int AgregadoID { get; set; }
        public int Assinatura { get; set; }
    }
}
