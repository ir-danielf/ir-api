using System;

namespace IngressoRapido.Lib
{
    [Serializable]
    public class EstruturaMapaMobile
    {
        public EstruturaMapaMobile()
        {
            this.SetorMapa = new SetorM();
        }

        public SetorM SetorMapa { get; set; }
        public IngressoRapido.Lib.LugarLista Lugares { get; set; }
    }

    public class SetorM
    {
        public bool TemImagem { get; set; }
        public string Background { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Imagem { get; set; }
    }

}
