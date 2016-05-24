using System;

namespace IRLib.ClientObjects
{
    public enum EnumTipoExibicao
    {
        Ok = 'O',
        Pendente = 'P',
    }
    [Serializable]
    public class EstruturaExibicaoGrupos
    {
        public string Evento { get; set; }
        public string Apresentacao { get; set; }
        public EnumTipoExibicao TipoExibicao { get; set; }
    }
}
