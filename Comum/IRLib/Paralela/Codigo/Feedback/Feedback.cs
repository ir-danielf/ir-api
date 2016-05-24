using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace IRLib.Paralela
{
	public class Feedback
	{

		public void AtribuirHorarios(int quantidade, int horas)
		{
			BD bd = new BD();
			try
			{
				string busca =
					@"SELECT
						TOP 500 vb.ID, MIN(ap.Horario) AS MenorApresentacao
						FROM tVendaBilheteria vb (NOLOCK)
						INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
						INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
					WHERE vb.Status = 'P' AND vb.DataVenda > '20110000000000' AND (vb.EmailSincronizado IS NULL OR vb.EmailSincronizado = 'F')
					GROUP BY vb.ID
					ORDER BY vb.ID ASC";

				bd.Consulta(busca);


				var data = new
				{
					VendaBilheteriaID = 0,
					Data = DateTime.Now,
				};

				var lista = VendaBilheteria.ToAnonymousList(data);

				while (bd.Consulta().Read())
				{
					lista.Add(new
					{
						VendaBilheteriaID = bd.LerInt("ID"),
						Data = bd.LerDateTime("MenorApresentacao"),
					});
				}

				bd.FecharConsulta();

				foreach (var venda in lista)
					bd.Executar(
						string.Format(@"UPDATE tVendaBilheteria 
							SET EmailSincronizado = '{0}', EmailEnviar = '{1}' 
							WHERE ID = {2}", "T", venda.Data.AddHours(horas).ToString("yyyyMMddHHmmss"), venda.VendaBilheteriaID));
			}
			finally
			{
				bd.Fechar();
			}
		}

		public List<EstruturaFeedback> BuscarVendasFeedback(int quantidade)
		{
			BD bd = new BD();
			try
			{
				string busca =
					string.Format(
                        @"SELECT TOP {0} 
							vb.ID, vb.Senha, vb.DataVenda, c.Nome AS Cliente, c.Email
						  FROM tVendaBilheteria vb (NOLOCK)
						  INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
							WHERE  vb.Status = 'P' AND vb.DataVenda > '20110101000000' AND vb.EmailSincronizado = 'T' AND
							(EmailEnviado IS NULL OR EmailEnviado = 'F') AND 
							EmailEnviar IS NOT NULL AND EmailEnviar < {1}

						 ",
						quantidade, DateTime.Now.ToString("yyyyMMddHHmmss"));

				List<EstruturaFeedback> lista = new List<EstruturaFeedback>();

				bd.Consulta(busca);
				while (bd.Consulta().Read())
				{
					lista.Add(new EstruturaFeedback()
					{
						VendaBilheteriaID = bd.LerInt("ID"),
						SenhaVenda = bd.LerString("Senha"),
						DataVenda = bd.LerDateTime("DataVenda"),
						ClienteNome = bd.LerString("Cliente"),
						ClienteEmail = bd.LerString("Email"),
					});
				}

				return lista;
			}
			finally
			{
				bd.Fechar();
			}
		}

		public void EncaminhaFeedback(List<EstruturaFeedback> lista, int respiro, int quantidade)
		{
			BD bd = new BD();
			try
			{
				EstruturaFeedback feedBack = null;
				EnviarEmailParalela enviarEmail = new EnviarEmailParalela();
				string corpoEmail = new StreamReader(ConfigurationManager.AppSettings["CaminhoFisicoHTMLFeedBack"] + "\\Feedback.htm").ReadToEnd();
				List<int> vendabilheterias = new List<int>();

				while (lista.Count > 0)
				{
					for (int i = 0; i < quantidade; i++)
					{
						feedBack = lista.FirstOrDefault();
						if (feedBack == null)
							break;
						enviarEmail.EnviarFeedback(corpoEmail, feedBack);
						vendabilheterias.Add(feedBack.VendaBilheteriaID);
						lista.Remove(feedBack);
					}
					Thread.Sleep(respiro);
				}

				bd.BulkInsert(vendabilheterias, "#VendasEnviadas", false, true);
				bd.Executar(
					@"
						UPDATE vb SET vb.EmailEnviado = 'T'
						FROM tVendaBilheteria vb
						INNER JOIN #VendasEnviadas ON vb.ID = #VendasEnviadas.ID
					");

			}
			finally
			{
				bd.Fechar();
			}
		}

		public EstruturaFeedback CarregarVenda(int vendaBilheteriaID)
		{
			BD bd = new BD();
			try
			{
				bd.Consulta(
					string.Format(@"SELECT TOP 1 vb.Senha, vb.DataVenda, c.Nome AS Cliente
						FROM tVendaBilheteria vb (NOLOCK)
						INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
						WHERE vb.ID = {0} AND (FeedbackPosVenda IS NULL OR FeedbackPosVenda = '')", vendaBilheteriaID));

				if (!bd.Consulta().Read())
					throw new Exception("Não foi possível encontrar a venda selecionada.");

				else
					return new EstruturaFeedback()
						{
							VendaBilheteriaID = vendaBilheteriaID,
							SenhaVenda = bd.LerString("Senha"),
							DataVenda = bd.LerDateTime("DataVenda"),
							ClienteNome = bd.LerString("Cliente"),
						};
			}
			finally
			{
				bd.Fechar();
			}
		}

		public void EnviarFeedbackCliente(int vendaBilheteriaID, string nomeCliente, string senhaVenda, string feedback)
		{
			BD bd = new BD();
			try
			{
				new EnviarEmailParalela().EnviarFeedbackCliente
					(nomeCliente, senhaVenda, feedback);

				bd.Executar("UPDATE tVendaBilheteria SET FeedbackPosVenda = '" + feedback + "' WHERE ID = " + vendaBilheteriaID);
			}
			finally
			{
				bd.Fechar();
			}
		}
	}

	public class EstruturaFeedback
	{
		public int VendaBilheteriaID { get; set; }
		public DateTime DataVenda { get; set; }
		public string SenhaVenda { get; set; }
		public string ClienteNome { get; set; }
		public string ClienteEmail { get; set; }
	}
}
