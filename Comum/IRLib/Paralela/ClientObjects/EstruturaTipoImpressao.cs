using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaTipoImpressao
    {
        public enum TipoImpressao
        {
            Ambos = 'A',
            Laser = 'L',
            Termica = 'T',
        }

        public char Tipo { get; set; }
        public string Nome { get; set; }

        public static List<EstruturaTipoImpressao> TiposImpressao()
        {
            List<EstruturaTipoImpressao> lista = new List<EstruturaTipoImpressao>();
            /*lista.Add(
                new EstruturaTipoImpressao() { Tipo = (char)TipoImpressao.Ambos, Nome = "Ambos", });*/
            lista.Add(
                new EstruturaTipoImpressao() { Tipo = (char)TipoImpressao.Laser, Nome = "Laser", });
            lista.Add(
                new EstruturaTipoImpressao() { Tipo = (char)TipoImpressao.Termica, Nome = "Térmica", });
            return lista;
        }

        public static char ToTipo(bool termica, bool laser)
        {
            if (termica && laser)
                return (char)TipoImpressao.Ambos;
            else if (termica)
                return (char)TipoImpressao.Termica;
            else
                return (char)TipoImpressao.Laser;
        }


        public static string ToSQL(bool termica, bool laser)
        {
            if (termica && laser)
                return string.Format("( el.ID IS NULL OR (el.TipoImpressao = 'L' OR e.TipoImpressao = 'L') OR (el.TipoImpressao = 'T' OR e.TipoImpressao = 'T') )", (char)TipoImpressao.Laser, (char)TipoImpressao.Termica);
            else if (termica)
                return string.Format("( el.TipoImpressao = '{0}' OR e.TipoImpressao = '{0}' )", (char)TipoImpressao.Termica);
            else
                return string.Format("( el.TipoImpressao = '{0}' OR e.TipoImpressao = '{0}' )", (char)TipoImpressao.Laser);
        }
    }
}
