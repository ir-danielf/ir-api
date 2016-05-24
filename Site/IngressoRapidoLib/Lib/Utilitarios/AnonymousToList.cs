using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public static class AnonymousList
    {
        public static List<T> ToAnonymousList<T>(T ItemType)
        {
            return new List<T>();
        }
    }
}
