using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCodigoBarraValido
    {
        public string EventoCodigo;
        public string ApresentacaoCodigo;
        //setor e preço código respectivamente o evento e a apresetações são sempre únicas.
        public List<EstruturaSetorPrecoCodigoBarra> SetorPrecoCodigo;
    }
    [Serializable]
    public class EstruturaSetorPrecoCodigoBarra
    {
        public int SetorID;
        public string SetorCodigo;
        public string SetorNome;
        
        public string PrecoCodigo;
        public string PrecoNome;
    }
}
