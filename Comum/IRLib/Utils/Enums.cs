using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace IRLib.Utils
{
    public static class Enums
    {
        public static string GetDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static char GetChar<T>(T value)
        {
            return Convert.ToChar(value);

        }

        public static T ParseItem<T>(string value) where T : struct, IConvertible
        {
            if (value == null || value.Length != 1)
                throw new ArgumentException("Parâmetro value em ParseItem<T>(string value) deve ter 1 único caractere");

            return (T)Enum.ToObject(typeof(T), Convert.ToChar(value));
        }

        public static T ParseItem<T>(int value) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ParseItem<T>(char value) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ParseEnum<T>(string value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            if (string.IsNullOrEmpty(value)) return defaultValue;

            foreach (T item in Enum.GetValues(typeof(T)))
                if (item.ToString().ToLower().Equals(value.Trim().ToLower())) return item;
            return defaultValue;
        }

        public static T ParseCharEnum<T>(string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value must be filled");

            foreach (T item in Enum.GetValues(typeof(T)))
                if (string.Compare((Convert.ToChar(item)).ToString(), value.Trim().ToLower(), true) == 0)
                    return item;

            throw new ArgumentException("Value not found");
        }

        public static Dictionary<string, string> EnumToDictionary<T>() where T : struct, IConvertible
        {
            return EnumToDictionary<T>(null, null);
        }
        public static Dictionary<string, string> EnumToDictionary<T>(string firstValue, string firstName) where T : struct, IConvertible
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (firstValue != null && firstName != null) dic.Add(firstValue, firstName);

            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            foreach (var val in enumValArray)
            {
                T item = (T)Enum.Parse(enumType, val.ToString());
                var x = ParseEnum<T>(val.ToString(), default(T));
                dic.Add(Convert.ToChar(x).ToString(), GetDescription<T>(x));
            }
            return dic;
        }

        public static List<string> EnumToList<T>() where T : struct, IConvertible
        {
            return EnumToList<T>(null);
        }

        public static List<string> EnumToList<T>(string firstName) where T : struct, IConvertible
        {
            List<string> list = new List<string>();
            if (firstName != null) list.Add(firstName);

            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            foreach (var val in enumValArray)
            {
                T item = (T)Enum.Parse(enumType, val.ToString());
                var x = ParseEnum<T>(val.ToString(), default(T));
                list.Add(GetDescription<T>(x));
            }
            return list;
        }

        public static Dictionary<int, string> EnumToIntDictionary<T>() where T : struct, IConvertible
        {            
            return EnumToIntDictionary<T>(0, null);
        }
        public static Dictionary<int, string> EnumToIntDictionary<T>(int firstValue, string firstName) where T : struct, IConvertible
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            if (firstValue > 0 && firstName != null) dic.Add(firstValue, firstName);

            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            foreach (var val in enumValArray)
            {
                T item = (T)Enum.Parse(enumType, val.ToString());
                var x = ParseEnum<T>(val.ToString(), default(T));
                dic.Add(Convert.ToInt32(x), GetDescription<T>(x));
            }
            return dic;
        }

        public enum EnumFaseAssinatura
        {
            Invalido = 0,
            Renovacao = 1,
            TrocaPrioritaria = 2,
            Troca = 3,
            Aquisicoes = 4,
        }

        public enum EnumTipoMenuAssinatura
        {
            Inicial = 0,
            Comum = 1,
        }

        public enum EnumTipoAmbiente
        {
            Assinaturas = 0,
            BancoIngressos = 1,
            MusicaNaCabeca = 2,
        }

        public enum EnumTipoAssinatura
        {
            OSESP = 1,
            Filarmonica = 2,
            CulturaArtistica = 3,
            SPCD = 4,
            Default = 5,
            SaoPedro = 6,
            JazzSinfonica
        }

        public enum EnumMenuSelecionado
        {
            Inicio,
            Assinaturas,
            Cadastro,
            FaturaSemPagamento,
            HistoricoAcoes,
            Clientes,
            AlterarSenha,
            Programacao,
            Pagamentos,
            Precos,
            Boletos,
            EmailMarketing,
            Relatorios,
            BancoIngressosInicio,
            BancoIngressosDoacao,
            BancoIngressosResgate,
            BancoIngressosComprovante,
            BancoIngressosCreditos,
            BancoIngressosProgramacao,
            MusicaCabecaInscricao,
            MusicaCabecaComprovantes,
            Caixa,
            Configuracao,
            AlterarAgregados
        }

        public enum Parceiros
        {
            PortoSeguro = 117,
            Caras = 233,
            NET = 786
        }
    }
}
