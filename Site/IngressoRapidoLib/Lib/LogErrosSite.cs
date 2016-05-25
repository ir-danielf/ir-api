using CTLib;
using System;

namespace IngressoRapido.Lib
{
    public class LogErrosSite
    {
        DAL oDAL = new DAL();

        public LogErrosSite() { }

        public LogErrosSite(int id)
        {
            this.ID = id;
        }

        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int clienteId = 0;
        public int ClienteID
        {
            get { return clienteId; }
            set { clienteId = value; }
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        private string timeStamp = string.Empty;
        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        private string ip = string.Empty;
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        private string url = string.Empty;
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        private string mensagem = string.Empty;
        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        private string stacktrace = string.Empty;
        public string Stacktrace
        {
            get { return stacktrace; }
            set { stacktrace = value; }
        }

        private string innerexception = string.Empty;
        public string Innerexception
        {
            get { return innerexception; }
            set { innerexception = value; }
        }

        public void GravaLogSite()
        {
            try
            {
                oDAL.Execute(string.Format(@"INSERT INTO LogErrosSite (ClienteID, SessionID, TimeStamp, IP, 
                            URL, Mensagem, Stacktrace, Innerexception)
                            VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", this.ClienteID, this.SessionID, this.timeStamp, this.IP,
                            this.URL, this.Mensagem, this.Stacktrace, this.Innerexception));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}
