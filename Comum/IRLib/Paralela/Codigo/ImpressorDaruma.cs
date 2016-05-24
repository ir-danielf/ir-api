using System;
using System.Runtime.InteropServices;

namespace IRLib.Paralela
{
    public static class ImpressorDaruma
    {
        public static string CupomImprimir;
        private static string UltimoCupom;
        public static string Porta;

        public static void Imprimir()
        {
            UltimoCupom = CupomImprimir + new String('\n', 8);
            CupomImprimir = String.Empty;

            int retorno = DarumaFramework_DLL.DarumaFramework_Declaracoes.iImprimirTexto_DUAL_DarumaFramework(UltimoCupom, 0);

            if (retorno != 1)
            {
                switch (retorno)
                {
                    case 0:
                        throw new Exception("Impressora de Cupom DESLIGADA!");
                    case -27:
                        throw new Exception("Erro genérico na Impressora de Cupom!");
                    case -50:
                        throw new Exception("Impressora de Cupom Off-Line!");
                    case -51:
                        throw new Exception("Impressora de Cupom SEM PAPEL!");
                    case -52:
                        throw new Exception("Impressora de Cupom INICIALIZANDO!");
                    case -60:
                        throw new Exception("Erro de formatação no Cupom!");
                }
            }
        }

        public static void Reimprimir()
        {
            CupomImprimir = UltimoCupom;
            Imprimir();
        }

        #region Funções de Acesso a Impressora DUAL
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Enter(string cEnter);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Porta(string cPorta);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Espera(string cEspera);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_ModoEscrita(string cEscrita);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Tabulacao(string cTabulacao);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Termica(string cTermica);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_Registry_DUAL_Velocidade(string cVelocidade);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_ImprimirTexto(string cTexto, int cTam);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_ImprimirArquivo(string cArquivo);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_VerificaStatus();
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_StatusGaveta();
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_VerificaDocumento();
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_Autenticar(string Local, string cTexto, string cSec);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_AcionaGaveta();
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_EnviarBMP(string cPath_do_BMP);
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_VerificarGuilhotina();
        [DllImport("Daruma32.dll")]
        private static extern int Daruma_DUAL_ConfigurarGuilhotina(int cFlag, int cLinhasAcionamento);
        #endregion
    }
}
