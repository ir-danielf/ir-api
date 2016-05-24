
namespace IRLib.ClientObjects
{
    public class EstruturaReservaInternet
    {
        public int CanalID { get; set; }
        public int LojaID { get; set; }
        public int UsuarioID { get; set; }
        public int CaixaID { get; set; }
        public int EmpresaID { get; set; }
        public int PerfilID { get; set; }
        public string Usuario { get; set; }

        public int ClienteID { get; set; }

        public string SessionID { get; set; }

        public string GUID { get; set; }
    }
}
