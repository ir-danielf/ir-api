using IRLib.ClientObjects;
using System.Collections.Generic;


namespace IngressoRapido.Lib
{
    public class EnderecoTipo
    {


        public List<EstruturaIDNome> Listar(bool primeiro)
        {
            IRLib.EnderecoTipo oET = new IRLib.EnderecoTipo();
            return oET.Listar(primeiro);
        }
    }
}
