using System;
using System.Collections.Generic;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.Model;

namespace IRAPI.Models
{
    public class SessionModel : DadosSessionModel
    {
        public Login Login { get; set; }

        public CompraModel Compra { get; set; }
    }

    public static class SessionModelExtension
    {
        public static int GetSiteId(this SessionModel sessionModel)
        {
            if (sessionModel == null || sessionModel.SiteID == 0)
                return 1;
            return sessionModel.SiteID;
        }
    }

    public class DadosSessionModel
    {
        public int EntregaControleID { get; set; }

        public int ClienteEnderecoID { get; set; }

        public int PDVID { get; set; }

        public int ClienteID { get; set; }

        public string SessionID { get; set; }

        public int UsuarioID { get; set; }

        public int LojaID { get; set; }

        public int CanalID { get; set; }

        public List<int> ValesIngressoID { get; set; }

        public EnderecoTemp EnderecoTemp { get; set; }

        public int SiteID { get; set; }

        public string TokenPayPal { get; set; }

        public string biletoToken { get; set; }

        public string biletoUuid { get; set; }
        public string Guid { get; set; }
    }
}