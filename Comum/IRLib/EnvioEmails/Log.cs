using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.EnvioEmails
{
    public class Log : IDisposable
    {

        BD bd = null;


        public Log()
        {
            bd = new BD();
        }

        public int EnvioID { get; set; }

        public int RegistraInicio(string email, string nome, string tipo)
        {
            
            string sql = string.Format(@"
                                    INSERT INTO Envio (Nome, Email, Tipo) VALUES ('{0}','{1}','{2}');
                                    SELECT SCOPE_IDENTITY(); 
                                    ", nome, email, tipo );


            object x = bd.ConsultaValor(sql);
            this.EnvioID = (x != null) ? Convert.ToInt32(x) : 0;
            return this.EnvioID;
        }

        public bool RegistraPostToken(string post)
        {
            string sql = string.Format("UPDATE Envio SET PostTokenData = '{0}' WHERE EnvioID = {1}", post.ToDB(), this.EnvioID.ToString());
            return bd.Executar(sql) > 0;
        }

        public bool RegistraToken(string token)
        {
            string sql = string.Format("UPDATE Envio SET Token = '{0}' WHERE EnvioID = {1}", token, this.EnvioID.ToString());
            return bd.Executar(sql) > 0;
        }

        public bool RegistraURLSend(string url, string post)
        {
            string sql = string.Format("UPDATE Envio SET URL = '{0}', PostData = '{2}'  WHERE EnvioID  = {1}", url, this.EnvioID.ToString(), post.ToDB());
            return bd.Executar(sql) > 0;
        }


        public bool RegistraResposta(string resposta)
        {
            string sql = string.Format("UPDATE Envio SET Resposta = '{0}', Concluido = 1 WHERE EnvioID  = {1}", resposta, this.EnvioID.ToString());
            return bd.Executar(sql) > 0;
        }

        public bool RegistraErro(string erro)
        {
            string sql = string.Format("UPDATE Envio SET Erro = '{0}' WHERE EnvioID  = {1}", erro, this.EnvioID.ToString());
            return bd.Executar(sql) > 0;
        }


        public void Dispose()
        {

            if (bd != null)
                bd.Fechar();

        }
    }
}
