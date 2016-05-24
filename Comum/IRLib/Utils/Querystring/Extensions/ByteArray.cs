using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }
        
        public static string ToUTF8String(this byte[] data)
        {
            return new UTF8Encoding().GetString(data);
        }

        public static byte[] ToByteArray(this string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
        }

        public static byte[] ToByteArrayUTF8(this string str)
        {
            return new UTF8Encoding().GetBytes(str);
        }

        public static byte[] ToByteArrayBase64(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
