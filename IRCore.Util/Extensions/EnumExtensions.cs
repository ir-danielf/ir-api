using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class EnumExtensions
    {

        public static string Description(this Enum value)
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

        public static string Name(this Enum value)
        {
            return value.ToString();
        }

        public static int ValueAsInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static char ValueAsChar(this Enum value)
        {
            return Convert.ToChar(value);
        }

        public static string ValueAsString(this Enum value)
        {
            return value.ValueAsChar().ToString();
        }

    }
}
