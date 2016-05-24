using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects.Arvore
{
    [Serializable]
    public class EstruturaArvoreItem
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int VinculoID { get; set; }
        public int VinculoSecundarioID { get; set; }
        public int PaiID { get; set; }
    }
    [Serializable]
    public class EstruturaArvoreItemLista : List<EstruturaArvoreItem>
    {
        public List<EstruturaArvoreItem> Filtrar(int id)
        {
            return this.FindAll(delegate(EstruturaArvoreItem item)
            {
                return item.ID == id || item.ID == 0;
            });
        }

        public List<EstruturaArvoreItem> FiltrarPorVinculo(int vinculoID)
        {
            return this.FindAll(delegate(EstruturaArvoreItem item)
            {
                return item.VinculoID == vinculoID || item.VinculoID == 0;
            });
        }



        public List<EstruturaArvoreItem> FiltrarPorVinculoEPai(int vinculoID, int paiID)
        {
            return this.FindAll(delegate(EstruturaArvoreItem item)
            {
                return item.VinculoID == vinculoID && item.PaiID == paiID;
            });
        }

        public List<EstruturaArvoreItem> FiltrarPorVinculosEPai(int vinculoID, int vinculoSecundarioID, int paiID)
        {
            return this.FindAll(delegate(EstruturaArvoreItem item)
            {
                return item.VinculoID == vinculoID && item.VinculoSecundarioID == vinculoSecundarioID && item.PaiID == paiID;
            });
        }
    }
}
