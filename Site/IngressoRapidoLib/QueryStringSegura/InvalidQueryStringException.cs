
namespace IngressoRapido.QueryStringSegura
{
	/// <summary>
	/// Thrown when attempting to decrypt or deserialize an invalid encrypted queryString.
	/// </summary>
	public class InvalidQueryStringException : System.Exception
	{
		public InvalidQueryStringException() : base() {}

        public InvalidQueryStringException(string message) : base(message) {}
	}
}
