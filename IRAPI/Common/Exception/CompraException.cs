
namespace IRAPI.Common.Exception
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Captura erros do tipo CompraException.
	/// </summary>
	[Serializable]
	public class CompraException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the CompraException class.
		/// </summary>
		public CompraException() :
			base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the CompraException class.
		/// </summary>
		/// <param name="message">A message to include in the exception.</param>
		public CompraException(string message) :
			base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CompraException class.
		/// </summary>
		/// <param name="message">A message to include in the exception.</param>
		/// <param name="innerException">An exception to include in the exception.</param>
		public CompraException(string message, Exception innerException) :
			base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CompraException class.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		/// <exception cref="ArgumentNullException">The info parameter is null.</exception>
		/// <exception cref="SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
		protected CompraException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
