
namespace IRAPI.Providers
{
	using System;
	using System.Threading.Tasks;
	using IRCore.BusinessObject;
	using IRCore.Util;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.Infrastructure;

    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
		public void Create(AuthenticationTokenCreateContext context)
		{
		}

		public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Create(context);
        }

		/// <summary>
		/// Realiza o processo de refresh do token.
		/// </summary>
		/// <param name="context">Contexto da aplicação.</param>
		public void Receive(AuthenticationTokenReceiveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			using (var usuario = new APIUsuarioBO())
			{
				// Define o client info baseado nos headers do contexto.
				var clientInfo = string.Empty;
				if (context.Request != null && context.Request.Headers != null)
				{
					clientInfo = !string.IsNullOrEmpty(context.Request.Headers["ClientInfo"]) ? context.Request.Headers["ClientInfo"] : string.Empty;
				}

				// Consulta o token do usuário, caso o token seja diferente de nulo e o client info não bata com os dados de identificação do token, retorna.
				var token = usuario.ConsultarToken(context.Token);
				if (token != null && !clientInfo.Equals(token.DadosIndentificacao, StringComparison.InvariantCultureIgnoreCase))
				{
					return;
				}

				// Se o token continua nulo, consulta o token utilizando o client info, que contém o nome do usuário.
				if (token == null)
				{
					token = usuario.ConsultarToken(clientInfo, null);
				}
				else if (clientInfo.Equals(token.DadosIndentificacao, StringComparison.InvariantCultureIgnoreCase))
				{
					token.Token = null;
				}

				// Se o objeto token continua null, gera exception, loga e retorna.
				if (token == null)
				{
                    //throw new NullReferenceException("Objeto Token é nulo após tentativa de recuperação.");

                    LogUtil.Debug(string.Format("##RefreshToken.Receive.ERROR## MSG: {0}, CLIENT_INFO {1}, TOKEN {2}", "Objeto Token é nulo após tentativa de recuperação.", clientInfo, context.Token));
                    return;
				}

				if (!string.IsNullOrEmpty(token.Token))
				{
					return;
				}

				// Se o token não é nulo, atualiza o token no storage e no contexto, e redefine o ticket.
				// O processo é o mesmo de criação do token, o token não será gravado em banco neste momento, a variável AtualizarToken da API controller gerencia esse processo na
				// próxima requisição.
				token.DataExpiracao = DateTime.Now.Add(TimeSpan.FromHours(token.APIUsuario.HorasExpiracaoToken));
				usuario.Salvar(token);

				var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
				var oAuthIdentity = usuario.CriarIdentity(token.APIUsuario, userManager.UserTokenProvider);

				var ticket = new AuthenticationTicket(oAuthIdentity, ApplicationOAuthProvider.CreateProperties(token.APIUsuario));
				ticket.Properties.ExpiresUtc = token.DataExpiracao;
				ticket.Properties.IssuedUtc = DateTime.Now;
				context.SetTicket(ticket);
			}
		}

		/// <summary>
		/// Realiza o processo de refresh do token.
		/// </summary>
		/// <param name="context">Contexto da aplicação.</param>
		/// <returns>Retorna uma operação assincrona.</returns>
		public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
			try
			{
				Receive(context);
			}
			catch (ArgumentNullException error)
			{
				LogUtil.Error(error);
			}
			catch (NullReferenceException error)
			{
				LogUtil.Error(error);
			}
        }
    }
}