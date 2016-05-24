using System;

namespace IRLib.Paralela.Codigo.Brainiac
{
    [Serializable]
    public class Retorno
    {
        public string Mensagem { get; set; }
        public float Porcentagem { get; set; }
        public float PorcentagemSobra { get; set; }
        public Enumeradores.EnumTipoRetorno TipoRetorno { get; set; }
    }
}
