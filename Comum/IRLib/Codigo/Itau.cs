using CTLib;
namespace IRLib 
{
	/// <summary>
	/// Summary description for Itau.
	/// </summary>
	public class Itau
	{
		public Itau()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static bool ValidaBin(string bin)
		{
			BD bd = new BD();

			try
			{
				if (bin == null || bin.Length != 6 || !IRLib.Utilitario.ehInteiro(bin))
					return false;

				object aux= bd.ConsultaValor("SELECT Bin FROM ItauBin WHERE Bin = '"+bin+"'");
				string bdBin = aux != null ? aux.ToString() : string.Empty;
				bd.Fechar();
				return bdBin == bin;
			}
			catch
			{
				throw;
			}
			finally
			{
				bd.Fechar();
			}
		}
	}

}