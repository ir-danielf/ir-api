using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaPerspectivaLugar
    {
        public int ID { get; set; }
        public int SetorID { get; set; }
        public string Descricao { get; set; }
        public PerspectivaLugar.EnumAcaoPerspectivaLugar Acao { get; set; }
        public bool Limpar()
        {
            if (Acao == PerspectivaLugar.EnumAcaoPerspectivaLugar.Nenhuma)
                return false;

            return true;
        }
    }
}
