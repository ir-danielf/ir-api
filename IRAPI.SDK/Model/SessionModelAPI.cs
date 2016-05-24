using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRAPI.SDK.Model
{
    public class SessionModelAPI
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string Tipo { get; set; }
        public DadosSessionModelAPI Retorno { get; set; }
    }
    public class DadosSessionModelAPI
    {
        public int EntregaControleID { get; set; }
        public int ClienteEnderecoID { get; set; }
        public int PDVID { get; set; }
        public int ClienteID { get; set; }
        public string SessionID { get; set; }
        public int UsuarioID { get; set; }
        public int LojaID { get; set; }
        public  int CanalID { get; set; }
        public int SiteID { get; set; }
    }
}
