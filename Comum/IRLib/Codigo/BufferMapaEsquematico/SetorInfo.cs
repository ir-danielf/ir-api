using System;
using System.Collections.Generic;
using System.Drawing;

namespace IRLib
{
    [Serializable]
    public class SetorInfo
    {
        public SetorInfo()
        {
            this.Coordenadas = new List<Point>();
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public int QuantidadeMapa { get; set; }
        public List<Point> Coordenadas { get; set; }
        public bool AprovadoPublicacao { get; set; }
        public IRLib.Setor.enumLugarMarcado EnumLugarMarcado { get; set; }
        public int Lotacao { get; set; }
        public int QuantidadeDisponível { get; set; }
        public int QuantidadeBancoIngresso { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public string PrecoPrincipal { get; set; }
        public bool ExibeQuantidade { get; set; }
        public string UltimaAtualizacaoStr
        {
            get
            {
                return UltimaAtualizacao.ToShortTimeString();
            }
        }
    }
}
