using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib.Paralela
{
    public class RoboCanalEvento : RoboCanalEvento_B
    {
        private string canais = ConfiguracaoParalela.GetString(ConfiguracaoParalela.Keys.CanaisDistribuicaoRealTime);

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
                var quantidade = bd.ConsultaValor("EXEC PROC_DistribuirEventos");

                return quantidade == null ? 0 : Convert.ToInt32(quantidade);   
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }       

        public bool VerificarEventoGeradoDepois(int pCanalID)
        {
            string[] CanaisDistribuicaoRealTime = null;

            if (!string.IsNullOrEmpty(this.canais))
                CanaisDistribuicaoRealTime = this.canais.Split(',');

            bool IsGeradoDepois = true;

            for (int i = 0; i < CanaisDistribuicaoRealTime.Length; i++)
            {
                if (pCanalID.ToString() == CanaisDistribuicaoRealTime[i])
                {
                    IsGeradoDepois = false;
                    break;
                }
            }

            return IsGeradoDepois;
        }
    }
}
