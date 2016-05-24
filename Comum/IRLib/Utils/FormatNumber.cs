using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRLib.Utils
{
    public class FormatNumber
    {
        
        private static string str = "";

         public static void Start()
         {
             str = "";
         }


         public static string soNumber(string valor, int maxLenght, KeyPressEventArgs e)
         {
             // string str = (string.IsNullOrEmpty(valor) || valor == "0" || valor == "0,00") ? "" : valor;

             //MaskedTextBox textBox1 = (MaskedTextBox)sender;
             int KeyCode = e.KeyChar;

             if (!IsNumeric(KeyCode))
             {
                 e.Handled = true;
                 return "";
             }
             else
             {
                 e.Handled = true;
             }
             if (((KeyCode == 8) || (KeyCode == 46)) && (str.Length > 0))
             {
                 str = str.Substring(0, str.Length - 1);
             }
             else if (!((KeyCode == 8) || (KeyCode == 46)))
             {
                 str = str + Convert.ToChar(KeyCode);
             }
             if (str.Length == 0)
             {
                 valor = "";
             }
             if (str.Length > maxLenght)
             {
                 e.Handled = true;
                 return valor;                 
             }
             if (str.Length > 0)
             {
                 valor = str;
             }
             
             //else if (str.Length == 2)
             //{
             //    valor = "0," + str;
             //}
             //else if (str.Length > 2)
             //{
             //    valor = str.Substring(0, str.Length - 2) + "," +
             //                    str.Substring(str.Length - 2);
             //}

             return valor;

         }

        public static string soMoney(string valor, KeyPressEventArgs e)
        {
           // string str = (string.IsNullOrEmpty(valor) || valor == "0" || valor == "0,00") ? "" : valor;

            //MaskedTextBox textBox1 = (MaskedTextBox)sender;
            int KeyCode = e.KeyChar;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return "";
            }
            else
            {
                e.Handled = true;
            }
            if (((KeyCode == 8) || (KeyCode == 46)) && (str.Length > 0))
            {
                str = str.Substring(0, str.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                str = str + Convert.ToChar(KeyCode);
            }
            if (str.Length == 0)
            {
                valor = "";
            }
            if (str.Length == 1)
            {
                valor = "0,0" + str;
            }
            else if (str.Length == 2)
            {
                valor = "0," + str;
            }
            else if (str.Length > 2)
            {
                valor = str.Substring(0, str.Length - 2) + "," +
                                str.Substring(str.Length - 2);
            }

            return valor;

        }

        public static bool IsNumeric(int Val)
        {
            return ((Val >= 48 && Val <= 57) || (Val == 8) || (Val == 46));
        } 
    }
}
