using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaBlackListCartao
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public string ClienteNome { get; set; }
        public string NroCartao { get; set; }
        public string CheckSumCartao { get; set; }
        public int BandeiraID { get; set; }
        public string Bandeira { get; set; }
        public string Status { get; set; }
        public string imgSrc { get; set; }
    }
}
