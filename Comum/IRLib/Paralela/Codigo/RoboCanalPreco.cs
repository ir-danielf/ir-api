using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib.Paralela
{
    public class RoboCanalPreco : RoboCanalPreco_B
    {
        private string canais = ConfiguracaoParalela.GetString(ConfiguracaoParalela.Keys.CanaisDistribuicaoRealTime);

        public enum operacaobanco
        {
            Inserir = 'I',
            Deleletar = 'D'
        }

        public bool Execucao { get; set; }

        public RoboCanalPreco() { }

        public RoboCanalPreco(int usuarioIDLogado) : base() { }

        public int AtualizarDadosPendente()
        {
            try
            {
                var quantidade = bd.ConsultaValor("EXEC PROC_DistribuirPrecos");

                return quantidade == null ? 0 :  Convert.ToInt32(quantidade);
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
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
