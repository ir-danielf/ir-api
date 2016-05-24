using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoVenda
    {
        private string senha;

        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
        private bool comprouseguro;
        public bool ComprouSeguro
        {
            get { return comprouseguro; }
            set { comprouseguro = value; }
        }

        private Dictionary<int, int> ingressoXCodigoImpressao;

        public Dictionary<int, int> IngressoXCodigoImpressao
        {
            get { return ingressoXCodigoImpressao; }
            set { ingressoXCodigoImpressao = value; }
        }

        private Dictionary<int,string> codigosBarra;
                    
        public Dictionary<int,string> CodigosBarra
        {
            get { return codigosBarra; }
            set { codigosBarra = value; }
        }
        
    }
}
