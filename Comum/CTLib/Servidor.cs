using System.Configuration;

namespace CTLib {

	/// <summary>
	/// Obtem recursos do servidor.
	/// </summary>
	public class Servidor {

		/// <summary>
		/// Seta e retorna a string de conexao com o banco de dados.
		/// </summary>
		public static string ConexaoBD{
			get{
                string retorno = ConfigurationManager.AppSettings["Conexao"];
				return retorno;
			}
		}

	}

}