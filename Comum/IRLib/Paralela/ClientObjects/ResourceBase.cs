using System.Drawing;

namespace IRLib.Paralela
{
    public abstract class ResourceBase
    {
        public Bitmap logo;
        public string empresaNome;
        public string empresaNomeAbreviacao;
        public string siteEmpresa;
        public string telefone;
        public string urlDownload;
        public abstract void PreencheResourceBase();

    }
}
