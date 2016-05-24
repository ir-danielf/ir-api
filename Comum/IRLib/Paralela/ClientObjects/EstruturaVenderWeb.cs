using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaVenderWeb
    {
        public bool Sucesso { get; set; }
        public string Senha { get; set; }
        public string ScriptSucesso { get; set; }
        public string MensagemErro { get; set; }
    }
}
