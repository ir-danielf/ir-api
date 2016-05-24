using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRLib;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;

namespace IRLib
{

    public class DeclaracoesAuttar
    {


        public  DeclaracoesAuttar(){
            if (Environment.Is64BitOperatingSystem)
            {
                Environment.SetEnvironmentVariable("CTFCLIENT_HOME", @"C:\Program Files (x86)\Auttar\CTFClient", EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("PATH", @"C:\Program Files (x86)\Auttar\CTFClient\bin");
            }
            else
            {
                Environment.SetEnvironmentVariable("CTFCLIENT_HOME", @"C:\Program Files\Auttar\CTFClient", EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("PATH", @"C:\Program Files\Auttar\CTFClient\bin");
            }
        }

        // variável de ambiente necessária para a DLL
      
        [DllImport("ctfclient.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void iniciaClientCTF(StringBuilder resultado, string strTerminal, string strVersaoPDV,
            string strNomePDV, string strNumSites, string strListaIps, string strCriptografia, string strLog,
            string strInterativo, string strParametros);

        [DllImport("ctfclient.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void iniciaTransacaoCTF(StringBuilder resultado, string strOperacao, string strValor, string strDocumento,
            string dataTransacao, string numTrans);

        [DllImport("ctfclient.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void iniciaTransacaoCTFext(StringBuilder resultado, string strOperacao, string strValor, string strDocumento, 
            string dataTransacao, string numTrans, string dados);

        [DllImport("ctfclient.dll", EntryPoint = "continuaTransacaoCTF")]
        static extern void continuaTransacaoCTF(StringBuilder Resultado, StringBuilder Comando,
            StringBuilder Campo, StringBuilder Valor, StringBuilder Tamanho, StringBuilder Display);

        [DllImport("ctfclient.dll", EntryPoint = "finalizaTransacaoCTF")]
        static extern int finalizaTransacaoCTF(StringBuilder resultado, String confirmar, String numTrans, String dataTransacao);

        public int _iniciaClientCTF(StringBuilder resultado, string strTerminal, string strVersaoPDV, string strNomePDV, 
            string strNumSites, string strListaIps, string strCriptografia, string strLog, string strInterativo, string strParametros)
        {
            iniciaClientCTF(resultado, strTerminal, strVersaoPDV, strNomePDV, strNumSites, strListaIps, strCriptografia, strLog, strInterativo, strParametros);

            return Convert.ToInt32(resultado.ToString());
        }

        public int _iniciaTransacaoCTF(string operacao, string valor, string documento, string numTrans, string dataTransacao)
        {
            /*Operações
                101 = Débito
                112 = Crédito
                158 = Cancelamento de Crédito
                159 = Cancelamento de Débito
             */
            var resultado = new StringBuilder("99");
            var strData = dataTransacao;
            var strValor = completaString(valor, 12, '0', true).ToString();
            var strNumTrans = numTrans;
            var strDocumento = completaString(documento, 20, ' ', true).ToString();
            var strDados = "[]";

            if (Environment.Is64BitOperatingSystem)
            {
                Environment.SetEnvironmentVariable("CTFCLIENT_HOME", @"C:\Program Files (x86)\Auttar\CTFClient", EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("PATH", @"C:\Program Files (x86)\Auttar\CTFClient\bin");
            }
            else
            {
                Environment.SetEnvironmentVariable("CTFCLIENT_HOME", @"C:\Program Files\Auttar\CTFClient", EnvironmentVariableTarget.User);
                Environment.SetEnvironmentVariable("PATH", @"C:\Program Files\Auttar\CTFClient\bin");
            }
            
            //iniciaTransacaoCTFext(resultado, operacao, strValor, strDocumento, strData, strNumTrans, strDados);
            iniciaTransacaoCTF(resultado, operacao, strValor, strDocumento, strData, strNumTrans);
            return int.Parse(resultado.ToString());

        }

        public string completaString(string texto, int tamanho, char complemento, Boolean esquerda)
        {
            while (texto.Length < tamanho)
            {
                if (esquerda)
                {
                    texto = complemento + texto;
                }
                else
                {
                    texto = texto + complemento;
                }
            }
            return texto;
        }

        public int _continuaTransacao(StringBuilder Resultado, StringBuilder Comando, StringBuilder Campo, StringBuilder Valor, StringBuilder Tamanho, StringBuilder Display)
        {
            continuaTransacaoCTF(Resultado, Comando, Campo, Valor, Tamanho, Display);
            return int.Parse(Resultado.ToString());
        }

        public int _finalizaTransacao(String confirmar, String numTrans, String dataTransacao)
        {
            var resultado = new StringBuilder("00");
            finalizaTransacaoCTF(resultado, confirmar, numTrans, dataTransacao);
            return int.Parse(resultado.ToString());
        }
    }
}
