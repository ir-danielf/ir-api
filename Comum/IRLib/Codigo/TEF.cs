using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace IRLib
{
    public class TEF
    {
        #region Atributos e Propriedades
        private static bool _configurado = false;

        public static bool Configurado
        {
            get { return _configurado; }
        }
        #endregion

        #region Funções C# para Transações

        public static int Configura(string endereco, string loja, string terminal)
        {
            byte[] _endereco = Encoding.ASCII.GetBytes(endereco + "\0");
            byte[] _loja = Encoding.ASCII.GetBytes(loja + "\0");
            byte[] _terminal = Encoding.ASCII.GetBytes(terminal + "\0");

            try
            {
                int result = TEF.ConfiguraIntSiTefInterativo(_endereco, _loja, _terminal, 0);

                _configurado = (result == 0);

                return result;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return -999;
        }
        #endregion

        #region Comunicação entre C# e DLL
        public static void FinalizaFuncao(bool Confirma, string NumeroCupomFiscal, DateTime DataHoraFiscal)
        {
            try
            {
                byte[] _NumeroCupomFiscal = Encoding.UTF8.GetBytes(NumeroCupomFiscal);
                byte[] _DataFiscal = Encoding.UTF8.GetBytes(DataHoraFiscal.ToString("yyyyMMdd"));
                byte[] _HoraFiscal = Encoding.UTF8.GetBytes(DataHoraFiscal.ToString("HHmmss"));
                int _Confirma = 0;
                if (Confirma)
                    _Confirma = 1;
                else
                    _Confirma = 0;
                FinalizaTransacaoSiTefInterativo(_Confirma, _NumeroCupomFiscal,
                _DataFiscal, _HoraFiscal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw ex;
            }
        }
        public static int IniciaFuncao(int Funcao, decimal Valor, string CuponFiscal,
    string DataFiscal, string Horario, string Operador, string ParamAdic)
        {
            byte[] _Valor = Encoding.ASCII.GetBytes(Valor.ToString("N") + "\0");//deve ser passado com duas casas decimais em char*
            byte[] _CuponFiscal = Encoding.ASCII.GetBytes(CuponFiscal + "\0"); //número do cuponfiscal
            byte[] _DataFiscal = Encoding.ASCII.GetBytes(DataFiscal + "\0"); //data no formato AAAAMMDD
            byte[] _Horario = Encoding.ASCII.GetBytes(Horario + "\0"); //hora no formato HHMMSS
            byte[] _Operador = Encoding.ASCII.GetBytes(Operador + "\0"); //identificacao do operador
            byte[] _ParamAdic = Encoding.ASCII.GetBytes(ParamAdic + "\0");//limita menus de navegação (vide lista)     
            int retorno = TEF.IniciaFuncaoSiTefInterativo(Funcao, _Valor, _CuponFiscal,
                _DataFiscal, _Horario, _Operador, _ParamAdic);
            return retorno;
        }

        public static int ContinuaFuncao(ref long Comando, ref long TipoCampo, ref int TamMinimo, ref int TamMaximo, ref string Buffer,
            int TamBuffer, int Continua)
        {
            Int64[] _Comando = new Int64[1];
            Int64[] _TipoCampo = new Int64[1];
            Int16[] _TamMinimo = new Int16[1];
            Int16[] _TamMaximo = new Int16[1];
            _Comando[0] = Convert.ToInt64(Comando);
            _TipoCampo[0] = Convert.ToInt64(TipoCampo);
            _TamMinimo[0] = Convert.ToInt16(TamMinimo);
            _TamMaximo[0] = Convert.ToInt16(TamMaximo);

            byte[] _Buffer = new byte[20000];
            byte[] aux = Encoding.UTF8.GetBytes(Buffer.Split("\0".ToCharArray())[0]);
            for (int i = 0; i < aux.Length; i++)
            {
                _Buffer[i] = aux[i];
            }

            int retorno = ContinuaFuncaoSiTefInterativo(_Comando, _TipoCampo, _TamMinimo,
                _TamMaximo, _Buffer, TamBuffer, Continua);
            Comando = Convert.ToInt64(_Comando[0]);
            TipoCampo = (int)_TipoCampo[0];
            TamMinimo = Convert.ToInt32(_TamMinimo[0]);
            TamMaximo = Convert.ToInt32(_TamMaximo[0]);
            Buffer = Encoding.UTF8.GetString(_Buffer);
            return retorno;
        }
        public static int LeSimNaoPinPad(string Mensagem)
        {
            byte[] _Mensagem = Encoding.ASCII.GetBytes(Mensagem + "\0");
            return LeSimNaoPinPad(_Mensagem);
        }
        

        #endregion

        #region DLL Imports
        [DllImport("CliSiTef32I.dll", EntryPoint = "ConfiguraIntSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ConfiguraIntSiTefInterativo(
            [MarshalAs(UnmanagedType.LPArray)] byte[] pEnderecoIP,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pCodigoLoja,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pNumeroTerminal,
            short ConfiguraResultado
            );

        [DllImport("CliSiTef32I.dll", EntryPoint = "FechaPinPad", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int FechaPinPad();

        [DllImport("CliSiTef32I.dll", EntryPoint = "LeCartaoDireto", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int LeCartaoDireto(
            [MarshalAs(UnmanagedType.LPArray)] byte[] pMsgDisplay,
            [MarshalAs(UnmanagedType.LPArray)] byte[] trilha1,
            [MarshalAs(UnmanagedType.LPArray)] byte[] trilha2
            );

        //new
        [DllImport("CliSiTef32I.dll", EntryPoint = "IniciaFuncaoAASiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int IniciaFuncaoAASiTefInterativo(
            int Funcao,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Valor,
            [MarshalAs(UnmanagedType.LPArray)] byte[] CuponFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] DataFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Horario,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Operador,
            [MarshalAs(UnmanagedType.LPArray)] byte[] ParamAdic,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Produtos);

        [DllImport("CliSiTef32I.dll", EntryPoint = "ContinuaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ContinuaFuncaoSiTefInterativo(
            [MarshalAs(UnmanagedType.LPArray)] Int64[] Comando,
            [MarshalAs(UnmanagedType.LPArray)] Int64[] TipoCampo,
            [MarshalAs(UnmanagedType.LPArray)] Int16[] TamMinimo,
            [MarshalAs(UnmanagedType.LPArray)] Int16[] TamMaximo,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Buffer,
            int TamBuffer,
            int Continua
            );
        [DllImport("CliSiTef32I.dll", EntryPoint = "IniciaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int IniciaFuncaoSiTefInterativo(
            int Funcao,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Valor,
            [MarshalAs(UnmanagedType.LPArray)] byte[] CuponFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] DataFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Horario,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Operador,
            [MarshalAs(UnmanagedType.LPArray)] byte[] ParamAdic
            );
        [DllImport("CliSiTef32I.dll", EntryPoint = "FinalizaTransacaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern void FinalizaTransacaoSiTefInterativo(
            int Confirma,
            [MarshalAs(UnmanagedType.LPArray)] byte[] NumeroCuponFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] DataFiscal,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Horario
            );
        [DllImport("CliSiTef32I.dll", EntryPoint = "AbrePinPad", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int AbrePinPad();
        [DllImport("CliSiTef32I.dll", EntryPoint = "LeSimNaoPinPad", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int LeSimNaoPinPad(
            [MarshalAs(UnmanagedType.LPArray)] byte[] Mensagem
            );
        [DllImport("CliSiTef32I.dll", EntryPoint = "VerificaPresencaPinPad", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int VerificaPresencaPinPad();
        #endregion
    }

}
