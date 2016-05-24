using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaMapaEsquematicoSetor
    {
        public int ID { get; set; }
        public int MapaID { get; set; }
        public int SetorID { get; set; }
        public string SetorNome { get; set; }
        public List<Point> Pontos { get; set; }
        public bool Fechado { get; set; }

        public MapaEsquematico.EnumMapaEsquematicoInnerAcao InnerAcao { get; set; }

        public string PontosToString()
        {
            StringBuilder stb = new StringBuilder();
            foreach (Point ponto in this.Pontos)
                stb.Append(ponto.X + "," + ponto.Y + ";");

            return stb.ToString().Remove(stb.Length - 1);
        }

        public EstruturaMapaEsquematicoSetor Clone()
        {
            EstruturaMapaEsquematicoSetor clone = new EstruturaMapaEsquematicoSetor
                                                      {
                                                          ID = this.ID,
                                                          MapaID = this.MapaID,
                                                          SetorID = this.SetorID,
                                                          SetorNome = this.SetorNome,
                                                          Fechado = this.Fechado,
                                                          Pontos = new List<Point>(),
                                                          InnerAcao = this.InnerAcao
                                                      };
            foreach (Point pto in this.Pontos)
                clone.Pontos.Add(new Point(pto.X, pto.Y));

            return clone;
        }
    }

}
