using CTLib;
using System;
using System.Data;
using System.Data.SqlClient;

namespace IngressoRapido.Lib
{
    public class ControleNav
    {
        private int id;
        public int Id
        {
            get { return id; }
        }

        private int idcliente;
        public int IdCliente
        {
            get { return idcliente; }
            set { idcliente = value; }
        }

        private string nomepagina;
        public string NomePagina
        {
            get { return nomepagina; }
            set { nomepagina = value; }
        }

        private int idevento;
        public int IdEvento
        {
            get { return idevento; }
            set { idevento = value; }
        }

        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private string url;
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        private DateTime ts;
        public DateTime TS
        {
            get { return ts; }
            set { ts = value; } 
        }

        private string tagorigem;
        public string TagOrigem
        {
            get { return tagorigem; }
            set { tagorigem = value; }
        }

        DAL oDAL = new DAL();

        public void InserirNavegacao(ControleNav Controle)
        {
            string strSql = "INSERT into dbo.ControleNav (IdCliente, NomePagina, IdEvento, Tipo, Url, TimeStamp, TagOrigem) values (" + Controle.IdCliente + ", '" + Controle.NomePagina + "', " + Controle.IdEvento + ", '" + Controle.Tipo + "', '" + Controle.URL + "', '" + Controle.TS.ToString("yyyyMMddHHmmss") + "', '" + Controle.TagOrigem + "')";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                oDAL.ConnClose();
            }
            catch (SqlException ex)
            {
                oDAL.ConnClose();
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
                //exception geral
            }
        }
    }
}
