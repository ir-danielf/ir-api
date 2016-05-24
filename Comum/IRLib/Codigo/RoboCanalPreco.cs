using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib
{
    public class RoboCanalPreco : RoboCanalPreco_B
    {
        

        public enum operacaobanco
        {
            Inserir = 'I',
            Deletar = 'D'
        }

        public bool Execucao { get; set; }

        public RoboCanalPreco() { }

        public RoboCanalPreco(int usuarioIDLogado) : base() { }

        public int AtualizarDadosPendente()
        {
            try
            {
                this.Execucao = true;

                var quantidade = bd.ConsultaValor("EXEC PROC_DistribuirPrecos");

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
            string canais = Configuracao.GetString(Configuracao.Keys.CanaisDistribuicaoRealTime, ConfigurationManager.AppSettings["ConfigVersion"]);

            if (!string.IsNullOrEmpty(canais))
                CanaisIDGeradoNoCadastro = canais.Split(',');

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
