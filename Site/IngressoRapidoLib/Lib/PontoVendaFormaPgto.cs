
namespace IngressoRapido.Lib
{
    public class PontoVendaFormaPgto
    {

        public PontoVendaFormaPgto()
        {
        }

        public PontoVendaFormaPgto(int id)
        {
            this.id = id;
        }

        //private DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private string nome;
        public string Nome
        {
            get { return this.nome; }
            set { nome = value; }
        }
    }
}
