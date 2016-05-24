using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib
{
    public class RoboCanalEvento : RoboCanalEvento_B
    {
        private string canais = Configuracao.GetString(Configuracao.Keys.CanaisDistribuicaoRealTime, ConfigurationManager.AppSettings["ConfigVersion"]);

        public enum operacaobanco
        {
            Inserir = 'I',
            Deleletar = 'D'
        }

        public bool Execucao { get; set; }

        public RoboCanalEvento() { }

        public RoboCanalEvento(int usuarioIDLogado) : base() { }

        public int AtualizarEventosPendente()
        {
            try
            {
                this.Execucao = true;

                var quantidade = bd.ConsultaValor("EXEC PROC_DistribuirEventos");

                return quantidade == null ? 0 : Convert.ToInt32(quantidade);   
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                this.Execucao = false;
                bd.Fechar();
            }
        }       

        public bool VerificarEventoGeradoDepois(int pCanalID)
        {
            string[] CanaisIDGeradoNoCadastro = null;

            if (!string.IsNullOrEmpty(this.canais))
                CanaisIDGeradoNoCadastro = this.canais.Split(',');

            bool IsGeradoDepois = true;

            for (int i = 0; i < CanaisIDGeradoNoCadastro.Length; i++)
            {
                if (pCanalID.ToString() == CanaisIDGeradoNoCadastro[i])
                {
                    IsGeradoDepois = false;
                    break;
                }
            }

            return IsGeradoDepois;
        }
    }
}
