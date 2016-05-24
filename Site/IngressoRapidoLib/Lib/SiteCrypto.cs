using Microsoft.VisualBasic;
using System;


namespace IngressoRapido.Lib
{
    /// <summary>
    /// Classe responsável por gerenciar a criptografia do site Ingresso Rápido.
    /// </summary>

    public class SiteCrypto
    {
        public SiteCrypto()
        {

        }

        /// <summary>
        /// Função de criptografia
        /// </summary>
        /// <param name="StrCripto">Valor a ser criptografado</param>
        /// <param name="BolAcao">E o parâmetro BolAcao é um valor booleano (True ou False) para indicar se deve ser criptografado (True) ou descriptografado (False)</param>
        public string Cripto(string StrCripto, bool BolAcao)
        {
            if (BolAcao)
                return EncodeBase64(StrCripto);
            else
                return DecodeBase64(StrCripto);

        }

        private int MyASC(string OneChar)
        {
            if (OneChar == "")
                return 0;
            else
                return Strings.Asc(OneChar);
        }

        private long CLng(object valor)
        {
            return long.Parse(valor.ToString());
        }

        private int octToDec(string valor)
        {
            int tamanho = valor.Length;


            string temp;

            int retorno = 0;
            int x;

            for (int i = 1; i <= tamanho; i++)
            {

                temp = valor.Substring(tamanho - i, 1);

                x = Convert.ToInt16(temp);

                retorno = retorno + (int)(x * Math.Pow(8, i - 1));
            }
            return retorno;

        }
        private string EncodeBase64(string inData)
        {

            string Base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            //int count;

            string sOut = string.Empty;

            string nGroup;
            string pOut;
            //int sGroup;

            for (int i = 1; i <= inData.Length; i = i + 3)// ou i +2 ?!
            {

                nGroup = (65536 * Strings.Asc(Strings.Mid(inData, i, 1)) + 256 * MyASC(Strings.Mid(inData, i + 1, 1)) + MyASC(Strings.Mid(inData, i + 2, 1))).ToString();

                nGroup = Conversion.Oct(nGroup);
                nGroup = new String('0', 8 - Strings.Len(nGroup)) + nGroup;



                pOut = Strings.Mid(Base64, this.octToDec(Strings.Mid(nGroup, 1, 2)) + 1, 1) +
                    Strings.Mid(Base64, this.octToDec(Strings.Mid(nGroup, 3, 2)) + 1, 1) +
                    Strings.Mid(Base64, this.octToDec(Strings.Mid(nGroup, 5, 2)) + 1, 1) +
                    Strings.Mid(Base64, this.octToDec(Strings.Mid(nGroup, 7, 2)) + 1, 1);

                sOut = sOut + pOut;

                if ((i + 2) % 57 == 0)
                    sOut = sOut + "\n";
            }

            switch (inData.Length % 3)
            {
                case 1:
                    sOut = sOut.Substring(0, sOut.Length - 2) + "==";
                    break;
                case 2:
                    sOut = sOut.Substring(0, sOut.Length - 1) + "=";
                    break;
            }

            return sOut;
        }

        private string DecodeBase64(string base64String)
        {
            string Base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            int dataLength;
            string sOut = "";

            base64String = base64String.Replace("\n", "");
            base64String = base64String.Replace("\t", "");
            base64String = base64String.Replace(" ", "");

            dataLength = base64String.Length;

            if (dataLength % 4 != 0)
                return string.Empty;

            for (int groupBegin = 1; groupBegin < dataLength; groupBegin += 4)
            {

                int numDataBytes;
                string thisChar;
                int thisData;
                string nGroup;
                string pOut;
                numDataBytes = 3;
                nGroup = "0";

                for (int CharCounter = 0; CharCounter <= 3; CharCounter++)
                {
                    thisChar = Strings.Mid(base64String, groupBegin + CharCounter, 1);

                    if (thisChar == "=")
                    {
                        numDataBytes = numDataBytes - 1;
                        thisData = 0;
                    }
                    else
                        thisData = Base64.IndexOf(thisChar);

                    if (thisData == -1)
                        return string.Empty;

                    nGroup = (64 * Convert.ToInt32(nGroup) + thisData).ToString();
                }

                nGroup = Microsoft.VisualBasic.Conversion.Hex(nGroup);

                nGroup = new String('0', 6 - nGroup.Length) + nGroup;

                pOut = Strings.Chr(Convert.ToByte(this.HexToDec(Strings.Mid(nGroup, 1, 2)))).ToString() +
                    Strings.Chr(Convert.ToByte(this.HexToDec(Strings.Mid(nGroup, 3, 2)))).ToString() +
                     Strings.Chr(Convert.ToByte(this.HexToDec(Strings.Mid(nGroup, 5, 2)))).ToString();


                sOut += pOut.ToString().Substring(0, numDataBytes);
            }
            return sOut;

        }

        private int HexToDec(string valor)
        {
            uint uiDecimal = 0;

            try
            {
                uiDecimal = System.Convert.ToUInt32(valor, 16);
            }

            catch (System.OverflowException exception)
            {
                throw new ApplicationException();
            }

            return Convert.ToInt32(uiDecimal);
        }
    }
}