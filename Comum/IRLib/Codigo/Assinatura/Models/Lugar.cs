
namespace IRLib.Assinaturas.Models
{
    public class Lugar
    {
        //private const string MONTADO = @"'ID': {0}, 'IngressoID': {1} 'Status': '{2}', 'Quantidade': {3}, 'Codigo': '{4}', 'PosX': {5}, 'PosY': {6} ";

        /// <summary>
        /// LugarID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// IngressoID..
        /// </summary>
        public int I { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string S { get; set; }
        /// <summary>
        /// Codigo
        /// </summary>
        public string C { get; set; }
        /// <summary>
        /// PosX
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// PosY
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Quantidade Lugar
        /// </summary>
        public int Q { get; set; }
        /// <summary>
        /// Quantidade DIsponivel
        /// </summary>
        public int D { get; set; }
        /// <summary>
        /// PerspectivalugarID
        /// </summary>
        public string P { get; set; }
    }
}
