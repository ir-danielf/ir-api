namespace IRAPI.Controllers
{
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;

	/// <summary>
	/// Classe responsável por ferramenta de comunicação com o servidor.
	/// </summary>
	[RoutePrefix("ping")]
	public class PingController : MasterApiController
	{
		/// <summary>
		/// Verifica se o serviço está ativo.
		/// </summary>
		/// <returns>Mensagem HTTP com código 200 - OK.</returns>
		[Route("")]
		[HttpGet]
		public HttpResponseMessage Index()
		{
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}