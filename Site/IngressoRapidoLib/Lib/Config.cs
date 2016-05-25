using System.Globalization;

namespace IngressoRapido.Lib
{
/// <summary>
/// Summary description for Config
/// </summary>

    public class Config
    {
        private const string cultura = "pt-BR";
        public static readonly CultureInfo CulturaPadrao = new CultureInfo(cultura, true);        
    }
}