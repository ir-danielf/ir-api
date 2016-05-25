
namespace IngressoRapido.Lib
{
    public class PontoVendaHorario
    {
        public PontoVendaHorario()
        {
        }

        public PontoVendaHorario(int id)
        {
            this.id = id;
        }

        //private DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private int pontovendaid;
        public int PontoVendaID
        {
            get { return this.pontovendaid; }
            set { pontovendaid = value; }
        }

        private string horarioinicial;
        public string HorarioInicial
        {
            get { return this.horarioinicial; }
            set { horarioinicial = value; }
        }

        private string horariofinal;
        public string HorarioFinal
        {
            get { return this.horariofinal; }
            set { horariofinal = value; }
        }

        private int diasemana;
        public int DiaSemana
        {
            get { return this.diasemana; }
            set { diasemana = value; }
        }
    }
}
