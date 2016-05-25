using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using IRCore.Util;

namespace System
{
    public static class StringExtensions
    {

        public static string LeftOfIndexOf(this string s, string search)
        {
            int pos = s.IndexOf(search);
            if (pos > -1)
            {
                return s.Substring(0, pos);
            }
            else
            {
                return s;
            }
        }

        public static string LeftOfLastIndexOf(this string s, string search)
        {
            int pos = s.LastIndexOf(search);
            if (pos > -1)
            {
                return s.Substring(0, pos);
            }
            else
            {
                return s;
            }
        }

        public static string RightOfIndexOf(this string s, string search)
        {
            int pos = s.IndexOf(search);
            if (pos > -1)
            {
                return s.Substring(pos + search.Length);
            }
            else
            {
                return s;
            }
        }

        public static string LeftWordsWithLimit(this string s, int limit, string sufix = "...")
        {
            if (s.Length > limit)
            {
                s = s.Substring(0, limit - sufix.Length);
                return s.LeftOfLastIndexOf(" ");

            }
            return s;
        }

        public static string Left(this string s, int count)
        {
            return s.Substring(0, count);
        }

        public static string FormatNumber(this string valor, string format)
        {
            return StringExtensions.FormatNumber(Convert.ToDecimal("1" + valor), "#" + format).Substring(1);
        }
        public static string FormatNumber(int valor, string format)
        {
            return StringExtensions.FormatNumber(Convert.ToDecimal(valor), format);
        }
        public static string FormatNumber(decimal valor, string format)
        {
            return valor.ToString(format);
        }

        public static string Replace(this string valor, string[] oldValues, string newValue)
        {
            string retorno = valor;
            foreach (string oldValue in oldValues)
            {
                retorno = retorno.Replace(oldValue, newValue);
            }
            return retorno;
        }

        public static string ReplaceLast(this string valor, string oldValues, string newValue)
        {
            var pos1 = valor.LastIndexOf(oldValues);
            var pos2 = pos1 + oldValues.Length;
            return valor.Substring(0, pos1) + newValue + valor.Substring(pos2);
        }

        public static string ToCoordinate(this string text)
        {
            return text.Replace(".", "").Replace(",", ".");
        }

        public static string Phonetized(this string text, bool mantemVogais = true)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            //Devemos retirar os caracteres especiais e converter todas as letras para Maiúsculo
            //Eliminamos todos os acentos
            StringBuilder sb = new StringBuilder(text.Trim().ToUpper().RemoveAcentos(false));

            //Substituimos
            sb.Replace("Y", "I");
            sb.Replace("BR", "B").Replace("BL", "B").Replace("PH", "F");
            sb.Replace("GR", "G").Replace("GL", "G").Replace("MG", "G");
            sb.Replace("NG", "G").Replace("RG", "G").Replace("GE", "J");
            sb.Replace("GI", "J").Replace("RJ", "J").Replace("MJ", "J");
            sb.Replace("NJ", "J").Replace("CE", "S").Replace("CI", "S");
            sb.Replace("CH", "S").Replace("CT", "T").Replace("CS", "S");
            sb.Replace("Q", "K").Replace("CA", "K").Replace("CO", "K");
            sb.Replace("CU", "K").Replace("C", "K").Replace("LH", "H");
            sb.Replace("RM", "SM").Replace("N", "M").Replace("GM", "M");
            sb.Replace("MD", "M").Replace("NH", "N").Replace("PR", "P");
            sb.Replace("X", "S").Replace("TS", "S").Replace("C", "S");
            sb.Replace("Z", "S").Replace("RS", "S").Replace("TR", "T");
            sb.Replace("TL", "T").Replace("LT", "T").Replace("RT", "T");
            sb.Replace("ST", "T").Replace("W", "V");

            //Eliminamos as terminações S, Z, R, R, M, N, AO e L
            int tam = sb.Length - 1;
            if (tam > -1)
                if (sb[tam] == 'S' || sb[tam] == 'Z' || sb[tam] == 'R' || sb[tam] == 'M' || sb[tam] == 'N' || sb[tam] == 'L')
                    sb.Remove(tam, 1);
            tam = sb.Length - 2;

            if (tam > -1)
                if (sb[tam] == 'A' && sb[tam + 1] == 'O')
                    sb.Remove(tam, 2);

            //Substituimos L por R e Ç por S;
            sb.Replace("Ç", "S").Replace("L", "R");

            //O BuscaBr diz para eliminamos todas as vogais e o H, 
            //porém ao implementar notamos que não seria necessário 
            //eliminarmos as vogais, isso dificultaria muito a busca dos dados "pós BuscaBR"
            if (!mantemVogais)
                sb.Replace("A", "").Replace("E", "").Replace("I", "").Replace("O", "").Replace("U", "");
            sb.Replace("H", "");

            //Eliminamos todas as letras em duplicidade;
            StringBuilder frasesaida = new StringBuilder();
            if (sb.Length > 0)
            {
                frasesaida.Append(sb[0]);
                for (int i = 1; i <= sb.Length - 1; i += 1)
                    if (frasesaida[frasesaida.Length - 1] != sb[i] || char.IsDigit(sb[i]))
                        frasesaida.Append(sb[i]);
            }

            return frasesaida.ToString();
        }
        public static string RemoveAcentos(this string text, bool includeCedilla)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text = Regex.Replace(text, "[áàãâä]", "a");
            text = Regex.Replace(text, "[ÁÀÃÂÄ]", "A");
            text = Regex.Replace(text, "[èéêë]", "e");
            text = Regex.Replace(text, "[ÈÉÊË]", "E");
            text = Regex.Replace(text, "[îìïí]", "i");
            text = Regex.Replace(text, "[ÎÌÏÍ]", "I");
            text = Regex.Replace(text, "[óòõöô]", "o");
            text = Regex.Replace(text, "[ÓÒÕÖÔ]", "O");
            text = Regex.Replace(text, "[úùüû]", "u");
            text = Regex.Replace(text, "[ÚÙÜÛ]", "U");

            if (includeCedilla)
                text = text.Replace("ç", "c").Replace("Ç", "C");

            return text;
        }

        public static string RemoveAcentos(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(input);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }

        public static string UrlFormatar(this string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text = text.RemoveAcentos();
            text = text.ToLower();

            Regex urlRgx = new Regex(@"[^a-z0-9]+");
            text = urlRgx.Replace(text, "-");

            return text;
        }

        public static DateTime ToDateTime(this string text)
        {
            text = text.Trim();
            if (text == "Now")
            {
                return DateTime.Now;
            }
            else if (text == "Today")
            {
                return DateTime.Today;
            }
            else if (text.Length == 10)
            {
                return DateTime.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                return DateTime.ParseExact(text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        public static bool IsEmail(this string email)
        {
            email = email.Trim().ToLower();
            var isEmail = new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?").IsMatch(email);

            return isEmail;
        }

        public static string EmailAddressHidingSomeCaracters(this string email)
        {
            var addr = new MailAddress(email);
            var username = addr.User;
            var domain = addr.Host;
            var regex = email.Contains(".co")
                ? @"(.{0,1}).+(.{1})+@(.{0,1}).+(.{1}(?:(.co).{0,}))"
                : @"(.{0,1}).+(.{1})+@(.{0,1}).+(.{1}(?:\..{0,}))";

            var replace = string.Format("$1{0}$2@$3{1}$4",
                new string('*', username.Length > 2 ? username.Length - 2 : 1),
                new string('*', (domain.IndexOf('.') > 2 ? domain.IndexOf('.') - 2 : 1)));

            var result = Regex.Replace(email, regex, replace);

            return result;
        }

        public static bool ValidarTamanho(this string valor, int tamanhoMax)
        {
            if (valor == null)
            {
                return true;
            }

            return valor.Length <= tamanhoMax;
        }

        public static string CorrigirTexto(this string text)
        {
            return Spelling.SpellCorrectWord(text);
        }

        public static string DigitsOnly(this string text)
        {
            return !string.IsNullOrWhiteSpace(text) ? new Regex(@"[^\d]").Replace(text, "") : string.Empty;
        }

        public static List<int> CommaSeparatedStringToIntList(this string str)
        {
            var strArray = str.Replace(" ", "").Split(",".ToCharArray());
            var intList = (from id in strArray select Convert.ToInt32(id)).ToList();

            return intList;
        }
    }
}
