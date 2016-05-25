using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Ecommerce
{
    public class InfoRetorno
    {
        DAL oDAL = new DAL();

        private int formaPagamentoID = 0;
        public int FormaPagamentoID
        {
            get { return formaPagamentoID; }
            set { formaPagamentoID = value; }
        }

        private int parcelas = 0;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        private string bin = string.Empty;
        public string Bin
        {
            get { return bin; }
            set { bin = value; }
        }

        private int clienteID = 0;
        public int ClienteID
        {
            get { return clienteID; }
            set { clienteID = value; }
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        private decimal total = 0;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        private bool lugaresSeparados = false;

        public bool LugaresSeparados
        {
            get { return lugaresSeparados; }
            set { lugaresSeparados = value; }
        }



        public InfoRetorno BuscaPorTID(string tid, int bandeiraCartao)
        {
            string strSql = string.Empty;

            if (bandeiraCartao == Controle.FP_AMEX)
                strSql = "SELECT FormaPagamentoID, Parcelas, Bin, ClienteID, SessionID, Total, LugaresSeparados FROM Amex WHERE TID = '" + tid + "' ";
            else if (bandeiraCartao == Controle.FP_REDECARD)
                strSql = "SELECT FormaPagamentoID, Parcelas, substring(NR_cartao,0,6) AS Bin, ClienteID, SessionID, Total, LugaresSeparados FROM Redecard WHERE TID = '" + tid + "' ";
            else if (bandeiraCartao == Controle.FP_VISANET)
                strSql = "SELECT FormaPagamentoID, Parcelas, Bin, ClienteID, SessionID, Price AS Total, LugaresSeparados FROM Visanet WHERE TID = '" + tid + "' ";


            using (IDataReader reader = oDAL.SelectToIDataReader(strSql))
            {
                if (!reader.Read())
                    throw new ApplicationException("Informações de retorno não encontradas, refaça a transação por favor.");
                else
                {
                    this.Bin = reader["Bin"].ToString();
                    this.ClienteID = Convert.ToInt32(reader["ClienteID"]);
                    this.FormaPagamentoID = Convert.ToInt32(reader["FormaPagamentoID"]);
                    this.Parcelas = Convert.ToInt32(reader["Parcelas"]);
                    this.SessionID = reader["SessionID"].ToString();
                    this.Total = Convert.ToDecimal(reader["Total"]);
                    this.LugaresSeparados = Convert.ToBoolean(reader["LugaresSeparados"]);
                }
            }
            return this;
        }
    }
}
