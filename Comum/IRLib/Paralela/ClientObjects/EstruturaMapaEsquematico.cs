using System;
using System.Collections.Generic;


namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaMapaEsquematico
    {

        public EstruturaMapaEsquematico()
        {
            this.MapasSetor = new List<EstruturaMapaEsquematicoSetor>();
            this.IdsRemover = new List<int>();
        }
        #region Propriedades
        public int ID { get; set; }
        public int LocalID { get; set; }
        public string Nome { get; set; }
        public List<EstruturaMapaEsquematicoSetor> MapasSetor { get; set; }

        public List<EstruturaMapaEsquematicoSetor> MapasSetorDistintos
        {
            get
            {
                List<int> lst = new List<int>();
                List<EstruturaMapaEsquematicoSetor> ListaDistinta = new List<EstruturaMapaEsquematicoSetor>();
                foreach (EstruturaMapaEsquematicoSetor setor in this.MapasSetor)
                {
                    if (lst.Contains(setor.SetorID))
                        continue;

                    lst.Add(setor.SetorID);
                    ListaDistinta.Add(setor);
                }
                return ListaDistinta;
            }
        }

        public MapaEsquematico.EnumMapaEsquematicoInnerAcao InnerAcao { get; set; }
        public List<int> IdsRemover { get; set; }
        #endregion

        #region Métodos
        public EstruturaMapaEsquematico Clone()
        {
            EstruturaMapaEsquematico clone = new EstruturaMapaEsquematico
                                                 {
                                                     ID = this.ID,
                                                     LocalID = this.LocalID,
                                                     Nome = this.Nome,
                                                     InnerAcao = this.InnerAcao
                                                 };

            foreach (EstruturaMapaEsquematicoSetor itemSetor in this.MapasSetor)
                clone.MapasSetor.Add(itemSetor.Clone());

            return clone;
        }
        public void AtribuirClone(EstruturaMapaEsquematico clone)
        {
            this.Nome = clone.Nome;
            this.InnerAcao = clone.InnerAcao;
            this.MapasSetor.Clear();
            this.MapasSetor.AddRange(clone.MapasSetor);
        }
        public void RemoverTodos()
        {
            for (int i = 0; i < this.MapasSetor.Count; i++)
                this.IdsRemover.Add(this.MapasSetor[i].ID);

            this.MapasSetor.Clear();
        }
        public void MudarAcaoMES(MapaEsquematico.EnumMapaEsquematicoInnerAcao acao)
        {
            foreach (EstruturaMapaEsquematicoSetor itemSetor in this.MapasSetor)
                itemSetor.InnerAcao = acao;
        }
        #endregion
    }
}
