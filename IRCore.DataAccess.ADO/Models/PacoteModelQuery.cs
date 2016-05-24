using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class PacoteModelQuery
    {
        public Pacote pacote { get; set; }

        public NomenclaturaPacote nomenclatura { get; set; }
    }

    public class PacoteItemModelQuery
    {
        public Pacote pacote { get; set; }

        public NomenclaturaPacote nomenclatura { get; set; }

        public PacoteItem pacoteItem { get; set; }

        public Preco preco { get; set; }

        public Setor setor { get; set; }

        public Apresentacao apresentacao { get; set; }

    }

    public static class PacoteExtensionQuery
    {
        public static Pacote toPacote(this PacoteModelQuery pacoteQuery)
        {
            pacoteQuery.pacote.NomenclaturaPacote = pacoteQuery.nomenclatura;
            return pacoteQuery.pacote;
        }

        public static PacoteItem toPacoteItem(this PacoteItemModelQuery pacoteItemQuery)
        {
            pacoteItemQuery.pacoteItem.Preco = pacoteItemQuery.preco;
            pacoteItemQuery.pacoteItem.Setor = pacoteItemQuery.setor;
            pacoteItemQuery.pacoteItem.Apresentacao = pacoteItemQuery.apresentacao;
            pacoteItemQuery.pacoteItem.Pacote = pacoteItemQuery.pacote;
            pacoteItemQuery.pacoteItem.NomenclaturaPacote = pacoteItemQuery.nomenclatura;
            return pacoteItemQuery.pacoteItem;
        }

    }
}
