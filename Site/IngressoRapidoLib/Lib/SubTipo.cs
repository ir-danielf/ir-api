
using CTLib;

/// <summary>
/// Summary description for Tipo
/// </summary>

namespace IngressoRapido.Lib
{
    public class SubTipo
    {
        public SubTipo()
        {
        }

        public SubTipo(int id)
        {
            this.Id = id;
        }

        DAL oDAL = new DAL();

        public int Id { get; set; }
        public int tipoID { get; set; }
        public string Descricao{get;set;}

    }
}