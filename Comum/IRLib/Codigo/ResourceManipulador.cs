using System;
using System.IO;
using System.Reflection;

namespace IRLib
{
    public class ResourceManipulador
    {
        /// <summary>
        /// Lê um determinado resource e devolve o arquivo como string.
        /// </summary>
        /// <param name="resourceNome"></param>
        /// <returns></returns>
        public string LerResource(string resourceNome)
        {
            Assembly assem = this.GetType().Assembly;
            using (Stream stream = assem.GetManifestResourceStream(resourceNome))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao ler o resource especificado. '"
                                             + resourceNome + "'\r\n" + e.ToString());
                }
            }
        }
    }
}
