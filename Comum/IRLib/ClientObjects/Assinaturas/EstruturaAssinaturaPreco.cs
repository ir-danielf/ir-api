using System;
namespace IRLib.ClientObjects.Assinaturas
{
    [Serializable]
    public class EstruturaAssinaturaPreco
    {
        public int AssinaturaClienteID { get; set; }
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}
