using System;
using System.Configuration;
using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.ComponentModel;


public static class Extensions
{
    public static string ToSafeString(this string valor)
    {
        return string.IsNullOrEmpty(valor) ? string.Empty : valor.Replace("'", string.Empty);
    }

    public static string ToSafeStringWithQuote(this string valor)
    {
        return string.IsNullOrEmpty(valor) ? string.Empty : valor.Replace("'", "''''");
    }

    public static bool JustNumbers(this string valor)
    {
        var retorno = true;

        foreach (var c in valor)
            retorno &= IsNumber(c.ToString());

        return retorno;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source.IndexOf(toCheck, comp) >= 0;
    }
    public static bool IsNumber(this string valor)
    {
        try
        {
            int.Parse(valor);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static StringBuilder AppendFilter(this StringBuilder filtro, string append)
    {
        return filtro.Length == 0 ? filtro.Append(append) : filtro.Append(" AND " + append);
    }

    public static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
    {
        if (Convert.ToBoolean(ConfigurationManager.AppSettings["IgnoreSslErrors"]))
        {
            // allow any old dodgy certificate...
            return true;
        }
        else
        {
            return policyErrors == SslPolicyErrors.None;
        }
    }

    public static decimal RoundUp(this decimal valor, int decimais)
    {
        var inteiro = Math.Floor(valor);
        var sobra = valor - inteiro;

        if (sobra > 0)
        {
            var sobraString = sobra.ToString();

            if (sobraString.Length > decimais + 3)
            {
                var digito = Convert.ToInt32(sobraString[decimais + 2].ToString());

                if (digito > 1)
                {
                    sobraString = sobraString.Substring(0, decimais + 2) + 9;
                    return decimal.Round(inteiro + Convert.ToDecimal(sobraString), decimais, MidpointRounding.AwayFromZero);
                }
            }

            return decimal.Round(valor, decimais, MidpointRounding.AwayFromZero);
        }
        else
            return valor;
    }

    //Extensão para formata dataHora
    public static string TimeStampFormat(this DateTime data)
    {
        return data.ToString("yyyyMMddHHmmss");
    }

    public static string RemoveAcentos(this string texto)
    {
        string retorno = string.Empty;

        var arraytexto = texto.Normalize(NormalizationForm.FormD).ToCharArray();

        foreach (char letter in arraytexto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                retorno += letter;
        }
        return retorno;
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return (T)attributes[0];
    }
    public static string ToName(this Enum value)
    {
        var attribute = value.GetAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }

}

