using System;

namespace IRLib.ClientObjects.Excel
{
    [Serializable]
    public class EstruturaExportarExcel
    {
        public string Valor { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }
    }
}
