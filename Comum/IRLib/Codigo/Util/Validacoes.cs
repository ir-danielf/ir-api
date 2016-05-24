using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace IRLib.Codigo.Util
{
    public class Validacoes
    {
        public bool ValidaCPF(string CPF)
        {
            bool numerosiguals = false;
            
            for (int i = 1; i < CPF.Length; i++)
            {
                if (CPF[0] == CPF[i])
                    numerosiguals = true;
                else
                {
                    numerosiguals = false;
                    break;
                }
            }

            if (numerosiguals)
                return false;

            int[] CalcARR = null;
            int Sum = 0;
            int DV1 = 0;
            int DV2 = 0;

            long ParseReturn = 0;
            if (!long.TryParse(removeCaracteres(CPF), out ParseReturn))
                return false;
            else
                CPF = removeCaracteres(CPF);

            if (CPF.Length != 11)
                CPF = string.Format("{0:D11}", long.Parse(CPF));

            CalcARR = new int[11];
            for (int x = 0; x < CalcARR.Length; x++)
                CalcARR[x] = int.Parse(CPF[x].ToString());

            Sum = 0;
            for (int x = 1; x <= 9; x++)
                Sum += CalcARR[x - 1] * (11 - x);

            Math.DivRem(Sum, 11, out DV1);
            DV1 = 11 - DV1;
            DV1 = DV1 > 9 ? 0 : DV1;
            if (DV1 != CalcARR[9])
                return false;

            Sum = 0;
            for (int x = 1; x <= 10; x++) { Sum += CalcARR[x - 1] * (12 - x); } Math.DivRem(Sum, 11, out DV2); DV2 = 11 - DV2; DV2 = DV2 > 9 ? 0 : DV2;
            if (DV2 != CalcARR[10])
                return false;

            return true;
        }

        public bool ValidaCNPJ(string CNPJ)
        {
            int[] CalcARR = null;
            int Sum = 0;
            int Multp = 0;
            int DV1 = 0;
            int DV2 = 0;

            long ParseReturn = 0;
            if (!long.TryParse(removeCaracteres(CNPJ), out ParseReturn))
                return false;
            else
                CNPJ = removeCaracteres(CNPJ);

            if (CNPJ.Length != 14)
                CNPJ = string.Format("{0:D14}", long.Parse(CNPJ));

            CalcARR = new int[14];
            for (int x = 0; x < CalcARR.Length; x++)
                CalcARR[x] = int.Parse(CNPJ[x].ToString());

            Multp = 5;
            Sum = 0;
            for (int x = 0; x < 12; x++)
            {
                Sum += CalcARR[x] * Multp;
                Multp--;
                if (Multp < 2)
                    Multp = 9;
            }

            Math.DivRem(Sum, 11, out DV1);
            if (DV1 < 2)
                DV1 = 0;
            else
                DV1 = 11 - DV1;

            if (DV1 != CalcARR[12])
                return false;

            Multp = 6;
            Sum = 0;
            for (int x = 0; x < 13; x++)
            {
                Sum += CalcARR[x] * Multp;
                Multp--;
                if (Multp < 2)
                    Multp = 9;
            }

            Math.DivRem(Sum, 11, out DV2);
            if (DV2 < 2)
                DV2 = 0;

            else
                DV2 = 11 - DV2;

            if (DV2 != CalcARR[13])
                return false;

            return true;
        }

        public bool ValidaEmail(string Email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return rg.IsMatch(Email);
        }

        public bool ValidaNumeros(string numero)
        {
            Regex reNum = new Regex(@"^\d+$");
            return reNum.Match(removeCaracteres(numero)).Success;
        }

        public string DiaSemana(DateTime data)
        {
            var culture = new System.Globalization.CultureInfo("pt-BR");
            return culture.DateTimeFormat.GetDayName(data.DayOfWeek);
        }

        public string removeCaracteres(string texto)
        {
            List<string> removerCaracter = new List<string>();
            removerCaracter.Add(".");
            removerCaracter.Add(",");
            removerCaracter.Add("/");
            removerCaracter.Add("\\");
            removerCaracter.Add("-");
            removerCaracter.Add("(");
            removerCaracter.Add(")");

            if (!String.IsNullOrEmpty(texto))
            {
                string retorno = texto;
                foreach (var item in removerCaracter)
                    while (retorno.IndexOf(item) != -1) retorno = retorno.Replace(item, string.Empty);
                return retorno;
            }
            else
                return texto;
        }

        public string ShowDialog(string text, string caption, int WidthTextBox = 0, int HeightTextBox = 0, bool Multiline = false, ScrollBars ScrollBars = ScrollBars.None)
        {
            if (WidthTextBox.Equals(0))
                WidthTextBox = 400;

            if (HeightTextBox.Equals(0))
                HeightTextBox = 30;


            Form prompt = new Form();
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;

            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = WidthTextBox, Height = Multiline ? HeightTextBox * 2 : HeightTextBox, Multiline = Multiline, ScrollBars = ScrollBars };

            Button confirmation = new Button() { Text = "Ok", Left = WidthTextBox - 160, Width = 100, Top = Multiline ? ((textBox.Height * 2) + 10) : (textBox.Height + 60) };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            Button cancel = new Button() { Text = "Cancelar", Left = WidthTextBox - 50, Width = 100, Top = Multiline ? ((textBox.Height * 2) + 10) : (textBox.Height + 60) };
            cancel.Click += (sender, e) => { prompt.Close(); };

            prompt.Width = textBox.Width + (textBox.Left * 2);
            prompt.Height = Multiline ? textBox.Height * 4 : textBox.Height * 8;

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }

        public string LerStringFormatoDataHora(string field, bool horas = false)
        {
            if (field.Length >= 8)
            {
                string ano = field.Substring(0, 4);
                string mes = field.Substring(4, 2);
                string dia = field.Substring(6, 2);
                string hora = string.Empty;
                string minuto = string.Empty;

                if (horas)
                {
                    hora = field.Substring(8, 2);
                    minuto = field.Substring(10, 2);
                    return string.Format("{0}/{1}/{2} {3}:{4}", dia, mes, ano, hora, minuto);
                }
                else
                    return string.Format("{0}/{1}/{2}", dia, mes, ano);
            }
            else
                return string.Empty;
        }
    }
}
