using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    //essa estrutura pode ser usada para qq clase que precise retornar ID e Nome.
    public class EstruturaFechamentoCaixa
    {
        private int caixaid;
        public int CaixaID
        {
            get { return caixaid; }
            set { caixaid = value; }
        }

        private int empresaid;
        public int EmpresaID
        {
            get { return empresaid; }
            set { empresaid = value; }
        }
    }
}
