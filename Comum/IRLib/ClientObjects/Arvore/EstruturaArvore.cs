using System;

namespace IRLib.ClientObjects.Arvore
{
    [Serializable]
    public class EstruturaArvore : ICloneable
    {
        public EstruturaArvore()
        {
            Regionais = new EstruturaArvoreItemLista();
            Empresas = new EstruturaArvoreItemLista();
            Locais = new EstruturaArvoreItemLista();
        }

        public EstruturaArvoreItemLista Regionais { get; set; }
        public EstruturaArvoreItemLista Empresas { get; set; }
        public EstruturaArvoreItemLista Locais { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    [Serializable]
    public class EstruturaArvoreSerie : ICloneable
    {
        public EstruturaArvoreSerie()
        {
            Eventos = new EstruturaArvoreItemLista();
            Apresentacoes = new EstruturaArvoreItemLista();
            Setores = new EstruturaArvoreItemLista();
            Precos = new EstruturaArvoreItemLista();
        }

        public EstruturaArvoreItemLista Eventos { get; set; }
        public EstruturaArvoreItemLista Apresentacoes { get; set; }
        public EstruturaArvoreItemLista Setores { get; set; }
        public EstruturaArvoreItemLista Precos { get; set; }
        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
