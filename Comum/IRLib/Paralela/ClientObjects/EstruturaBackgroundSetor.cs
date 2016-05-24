using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaBackgroundSetor
    {
        public int SetorID { get; set; }
        public string SetorNome { get; set; }
        public string CaminhoImagem { get; set; }
        public bool Novo { get; set; }
        public bool Remover { get; set; }
        public int VersaoBackground { get; set; }
    }
}
