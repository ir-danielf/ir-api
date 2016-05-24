/**************************************************
* Arquivo: Ingresso.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace IRLib.Paralela
{

	public partial class Ingresso : Ingresso_B
	{
		public int CotaItemID { get; set; }
		public int CotaItemIDAPS { get; set; }
		public bool Nominal { get; set; }
		public string CodigoPromocional { get; set; }

		public const string AGUARDANDO_TROCA = "A";
		public const string PREIMPRESSO = "P";
		public const string DISPONIVEL = "D";
		public const string BLOQUEADO = "B";
		public const string RESERVADO = "R";
		public const string VENDIDO = "V";
		public const string IMPRESSO = "I";
		public const string ENTREGUE = "E";
		public const string VIR_TROCADO = "*";
		public const string CORTESIA_SEM_CONVENIENCIA = "X";
		public const string RESERVADO_PRE_RESERVA = "M";
		public const string RESERVADO_PRE_RESERVA_SITE = "S";
		public const string PRE_RESERVA = "N";
		public const string Resultado = "'" + ENTREGUE + "','" + VENDIDO + "','" + IMPRESSO + "'"; // vendido - cancelado
		//pacote deve ser tratado como uma unidade indivisÃ­vel, ou seja, todos os ingressos do pacote
		//devem ser reservados e vendidos em conjunto. Esse status permite isso.
		public const string RESERVANDO = "S";

		public const string FORMATO_CODIGO = "00000";
		private const int INTERNET_USUARIO_ID = 1657;
		private const int INTERNET_LOJA_ID = 2;

		private DataRow info; //contem informacoes do ingresso (veja metodo EstruturaImpressao)
		private const string ERRO_INFO = "Execute antes o método InfoVendido ou InfoReservado para, depois, obter informações desse ingresso.";
		private const string ERRO_INFO_V = "Execute antes o método InfoVendido para, depois, obter informações desse ingresso.";

		private IngressoLog ingressoLog;
		private CodigoBarra codigoBarra;

		public enum StatusIngresso
		{
			AGUARDANDO_TROCA = 'A',
			PREIMPRESSO = 'P',
			PRE_RESERVA = 'N',
			DISPONIVEL = 'D',
			BLOQUEADO = 'B',
			RESERVADO = 'R',
			RESERVADO_PRE_RESERVA = 'M',
			VENDIDO = 'V',
			IMPRESSO = 'I',
			ENTREGUE = 'E',
			CORTESIA_SEM_CONVENIENCIA = 'X',
			RESERVADO_PRE_RESERVA_SITE = 'S'
		}

		public Ingresso()
		{
			info = null;
			ingressoLog = new IngressoLog();
			codigoBarra = new CodigoBarra();
		}
		public Ingresso(int ID, string codigo, int grupo, int classificacao, int lugarID)
		{
			this.Control.ID = ID;
			this.Codigo.Valor = codigo;
			this.Grupo.Valor = grupo;
			this.Classificacao.Valor = classificacao;
			this.LugarID.Valor = lugarID;
		}

		/// <summary>
		/// Aumentar tempo de reserva no momento da compra até retornar confirmaçao de pagamento
		/// </summary>
		/// <returns></returns>
		public static int AumentarTempoReservasInternet(int clienteID, string sessionID)
		{
			BD bd = new BD();
			return bd.Executar("UPDATE tIngresso SET TimeStampReserva = '" + DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss") + "' WHERE (Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' OR Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "') AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'");
		}
		/// <summary>
		/// Método utilizado para popular a tIngresso com os ClienteIDs da VendaBilheteria.
		/// kim
		/// </summary>
		public void PopulaClienteID()
		{
			try
			{
				string sql = @"SELECT ID,ClienteID INTO #temp FROM tVendaBilheteria (NOLOCK) WHERE ClienteID > 0;

							SELECT vb.ClienteID, i.ID AS IngressoID INTO #temp2 FROM #temp vb (NOLOCK)
							INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
							WHERE vb.ClienteID >0 AND i.STATUS IN ('R','V','I','E','A','N')

							UPDATE tIngresso SET ClienteID = #temp2.ClienteID FROM tIngresso INNER JOIN #temp2 ON  tIngresso.ID = #temp2.IngressoID";

				bd.Executar(sql);

			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Método utilizado para buscar o ValeIngresso (temporário) da tIngresso
		/// kim
		/// </summary>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public EstruturaValeIngresso PopulaValeIngresso(string codigo)
		{
			try
			{
				EstruturaValeIngresso retorno = new EstruturaValeIngresso();
				//A variavel retorno.FormaPagamentoID é fixa na struct...
				string sql = "SELECT i.ID AS IngressoID, p.Valor,fp.Dias,fp.TaxaAdm,fp.IR  FROM tIngresso i (NOLOCK) " +
							 "INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID " +
							 "LEFT JOIN tEmpresaFormaPagamento fp (NOLOCK) ON fp.EmpresaID = 1 AND fp.FormaPagamentoID = " + retorno.FormaPagamentoID + " " +
							 "WHERE CodigoBarraCliente = '" + codigo + "' AND Status <> '*' AND i.EventoID = " + retorno.EventoVirID;

				bd.Consulta(sql);
				while (bd.Consulta().Read())
				{
					retorno.IngressoID = bd.LerInt("IngressoID");
					retorno.Valor = bd.LerDecimal("Valor");
					retorno.FormaPagamentoDias = bd.LerInt("Dias");
					retorno.FormaPagamentoTaxaAdm = bd.LerDecimal("TaxaAdm");
					retorno.FormaPagamentoIR = bd.LerBoolean("IR");
				}
				return retorno;

			}
			catch (Exception)
			{

				throw;
			}
			finally
			{
				bd.Fechar();
			}

		}


		/// <summary>
		///Método para trazer os ingressos de Pré-Reserva de acordo com a senha da tVendaBilheteria.
		///job 267. kim
		/// </summary>
		/// <param name="senha">Senha da Pré-Reserva</param>
		/// <param name="clienteID">ClienteID da compra.</param>
		/// <returns></returns>
		public List<EstrutruraPreReservaSite> IngressosPreReservaPorSenha(string senha, int clienteID)
		{
			try
			{
				string sql = @"SELECT l.ID as LocalID,l.Nome AS Local,e.ID AS EventoID, e.Nome AS Evento, 
							Horario AS Apresentacao, lg.Quantidade,
							s.Nome AS Setor, i.ApresentacaoID, i.SetorID,s.LugarMarcado AS SetorTipo,i.ID AS IngressoID,i.Codigo,i.LugarID,i.Grupo,i.Classificacao
							FROM tVendaBilheteria vb(NOLOCK)
							INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
							INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
							INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
							INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
							INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
							INNER JOIN tLugar lg (NOLOCK) ON lg.ID = i.LugarID
							WHERE Senha = '" + senha + "' AND vb.ClienteID=" + clienteID + " AND i.Status='" + ((char)Ingresso.StatusIngresso.PRE_RESERVA).ToString() + "'";
				List<EstrutruraPreReservaSite> retorno = new List<EstrutruraPreReservaSite>();
				EstruturaMesaFechadaPreReservaSite estruturaMesaFechadaSite;
				EstrutruraPreReservaSite item = new EstrutruraPreReservaSite();
				bd.Consulta(sql);
				int ultimoLugarIDMesaFechada = -1;
				char setorTipo;
				int lugarID;
				int qtdeInseridaMesaFechada = 0;
				while (bd.Consulta().Read())
				{
					setorTipo = Convert.ToChar(bd.LerString("SetorTipo"));
					lugarID = bd.LerInt("LugarID");

					//mesa fechada?
					if (setorTipo == (char)IRLib.Paralela.Setor.enumLugarMarcado.MesaFechada)
					{
						//primeiro item da mesa fechada mesa fechada
						if (ultimoLugarIDMesaFechada != lugarID)
						{
							//limpa os objetos
							item = new EstrutruraPreReservaSite();
							item.IngressosMesaFechada = new List<EstruturaMesaFechadaPreReservaSite>();
							estruturaMesaFechadaSite = new EstruturaMesaFechadaPreReservaSite();
							//preenche o IngressoID e Codigo do ingresso
							estruturaMesaFechadaSite.IngressoID = bd.LerInt("IngressoID");
							estruturaMesaFechadaSite.Codigo = bd.LerString("Codigo");
							item.IngressosMesaFechada.Add(estruturaMesaFechadaSite);
							qtdeInseridaMesaFechada = 1; //primeiro ingresso da mesa.
							//preenche o objeto que vai ser mostrado no grid
							item.SetorTipo = setorTipo;
							item.LugarID = lugarID;
							item.Apresentacao = bd.LerDateTime("Apresentacao");
							item.ApresentacaoID = bd.LerInt("ApresentacaoID");
							item.EventoID = bd.LerInt("EventoID");
							item.Evento = bd.LerString("Evento");
							item.LocalID = bd.LerInt("LocalID");
							item.Local = bd.LerString("Local");
							item.Setor = bd.LerString("Setor") + " (" + bd.LerInt("Quantidade") + " Lugares )";
							item.SetorID = bd.LerInt("SetorID");
							item.SetorTipo = Convert.ToChar(bd.LerString("SetorTipo"));
							item.Grupo = bd.LerInt("Grupo");
							item.Classificacao = bd.LerInt("Classificacao");
							item.Quantidade = bd.LerInt("Quantidade");

							ultimoLugarIDMesaFechada = lugarID;
						}
						else
						{
							estruturaMesaFechadaSite = new EstruturaMesaFechadaPreReservaSite();
							//preenche o IngressoID e Codigo do ingresso
							estruturaMesaFechadaSite.IngressoID = bd.LerInt("IngressoID");
							estruturaMesaFechadaSite.Codigo = bd.LerString("Codigo");
							item.IngressosMesaFechada.Add(estruturaMesaFechadaSite);
							qtdeInseridaMesaFechada++;//incrementa a qtde inserida para essa mesa.

							//verifica se acabou de inserir os ingressos da mesa atual.
							if (qtdeInseridaMesaFechada == item.Quantidade)
							{
								retorno.Add(item);
							}
							ultimoLugarIDMesaFechada = lugarID;
						}
					}
					else // Lugares normais (diferentes de mesa fechada.
					{
						item = new EstrutruraPreReservaSite();

						item.SetorTipo = setorTipo;
						item.LugarID = lugarID;
						item.Apresentacao = bd.LerDateTime("Apresentacao");
						item.ApresentacaoID = bd.LerInt("ApresentacaoID");
						item.EventoID = bd.LerInt("EventoID");
						item.Evento = bd.LerString("Evento");
						item.LocalID = bd.LerInt("LocalID");
						item.Local = bd.LerString("Local");
						item.Setor = bd.LerString("Setor");
						item.SetorID = bd.LerInt("SetorID");
						item.SetorTipo = setorTipo;
						item.Grupo = bd.LerInt("Grupo");
						item.Classificacao = bd.LerInt("Classificacao");
						item.IngressoID = bd.LerInt("IngressoID");
						item.Codigo = bd.LerString("Codigo");
						item.Quantidade = bd.LerInt("Quantidade");

						retorno.Add(item);
					}
				}

				return retorno;
			}
			catch (Exception)
			{

				throw;
			}



		}

		/// <summary>
		/// Atualizar o tempo de reserva dos ingressos daquela sessão do cliente, quando uma nova reserva for feita
		/// </summary>
		/// <returns></returns>
		public int AtualizaTempoReservasInternet(int clienteID, string sessionID)
		{
			return bd.Executar("UPDATE tIngresso SET TimeStampReserva = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE AND Status = 'R' AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'");
		}
		/// <summary>
		/// função retorna o relatório da quantidade de ingressos agrupados por preços de 50 em 50.
		/// </summary>
		/// <param name="dataInicial"></param>
		/// <param name="dataFinal"></param>
		/// <returns></returns>
		public DataTable EstatisticaTaxaConveniencia(DateTime dataInicial, DateTime dataFinal)
		{
			try
			{
				string inicial = dataInicial.ToString("yyyyMMddHHmmss");
				string final = dataFinal.ToString("yyyyMMddHHmmss");

				DataTable retorno = new DataTable();
				retorno.Columns.Add("Valor", typeof(string));// Essa coluna é string para poder incluir os totais gerais
				retorno.Columns.Add("ComConveniencia", typeof(int));
				retorno.Columns.Add("SemConveniencia", typeof(int));
				retorno.Columns.Add("Total", typeof(int));

				if (inicial == final || dataFinal == DateTime.MinValue)
				{
					inicial = inicial.Substring(0, 8) + "%";
					final = inicial;
				}

				string sql = @"EXEC EstatisticaTaxaConveniencia @DataInicial ='" + inicial + "', @DataFinal ='" + final + "'";

				bd.Consulta(sql);
				DataRow linha;
				while (bd.Consulta().Read())
				{
					linha = retorno.NewRow();
					linha["Valor"] = bd.LerInt("Valor").ToString("C"); //String para possibilitar a inserção da linha de totais
					linha["ComConveniencia"] = bd.LerInt("ComConveniencia");
					linha["SemConveniencia"] = bd.LerInt("SemConveniencia");
					linha["Total"] = bd.LerInt("Total");
					retorno.Rows.Add(linha);
				}
				if (retorno.Rows.Count > 0)
				{
					//insere a linha de totais
					linha = retorno.NewRow();
					linha["Valor"] = "Total: ";
					linha["ComConveniencia"] = (retorno.Compute("SUM(ComConveniencia)", "1=1"));
					linha["SemConveniencia"] = (retorno.Compute("SUM(SemConveniencia)", "1=1"));
					linha["Total"] = (retorno.Compute("SUM(Total)", "1=1"));
					retorno.Rows.Add(linha);
				}
				return retorno;

			}
			catch (Exception)
			{

				throw;
			}
			finally
			{
				bd.Fechar();
			}
		}

		/// <summary>
		/// Recupera o último Codigo de ingresso para um determinado Setor / Apresentação.
		/// Apenas para Pista.
		/// </summary>
		/// <param name="apresentacaoSetorID"></param>
		/// <returns></returns>
		public int UltimoCodigoPista(int apresentacaoSetorID)
		{
			try
			{
				string sql = "SELECT TOP 1 Codigo FROM tIngresso (NOLOCK) WHERE ApresentacaoSetorID = " + apresentacaoSetorID + " AND Status=Status ORDER BY Codigo DESC";
				object x = bd.ConsultaValor(sql);
				return (x == null) ? 0 : Convert.ToInt32(x);

			}
			catch (InvalidCastException)
			{
				return 0;
			}
			catch
			{
				throw;
			}
		}


		public int TotalImpressoApresentacao(int apresentacaoID)
		{
			string sql = @"SELECT COUNT(tIngressolog.ID) FROM tIngressolog 
							INNER JOIN tIngresso on tIngresso.ID = tIngressolog.IngressoID
							WHERE tIngressolog.Acao in('I','R') AND ApresentacaoID = " + apresentacaoID;

			return Convert.ToInt32(bd.ConsultaValor(sql));
		}

		public int TotalImpressoApresentacoes(string apresentacoes)
		{
			string sql = @"SELECT COUNT(tIngressolog.ID) FROM tIngressolog 
							INNER JOIN tIngresso on tIngresso.ID = tIngressolog.IngressoID
							WHERE tIngressolog.Acao in('I','R') AND ApresentacaoID IN (" + apresentacoes + ")";

			return Convert.ToInt32(bd.ConsultaValor(sql));
		}

		public int TotalImpresso(int EventoID)
		{
			string sql = @"	SELECT COUNT(tIngressoLog.ID) FROM tIngresso
							INNER JOIN tIngressoLog ON tIngresso.ID = IngressoID
							WHERE EventoID = " + EventoID + " AND Acao IN ('R', 'I')";

			return Convert.ToInt32(bd.ConsultaValor(sql));
		}
		/// <summary>
		/// Exclui Ingresso
		/// </summary>
		/// <returns></returns>		
		public override bool Excluir()
		{

			try
			{

				bool ok = true;

				IngressoLogLista ingressoLogLista = new IngressoLogLista();
				ingressoLogLista.FiltroSQL = "IngressoID=" + this.Control.ID;
				ingressoLogLista.FiltroSQL = "VendaBilheteriaItemID > 0";
				ingressoLogLista.Carregar(1);
				if (ingressoLogLista.Tamanho > 0)
				{
					throw new IngressoException("Não pode excluir ingresso porque há venda efetuada.");
				}
				else
				{
					ingressoLogLista.FiltroSQL = null;
					ingressoLogLista.FiltroSQL = "IngressoID=" + this.Control.ID;
					ok = ingressoLogLista.ExcluirTudo();
				}

				if (ok)
					ok = base.Excluir(this.Control.ID);

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		/// <summary>
		/// Retorna a tabela com as informaçoes para a reserva de assinatura de acordo com a ApresentacaoID, SetorID e Codigo do Lugar.
		/// ATENÇÂO: Esta função não fecha o bd de propósito para a reutilização do acesso ao banco de dados.
		/// Utilizada no projeto de importaçao de assinaturas da OSESP
		/// </summary>
		/// <param name="apresentacaoID"></param>
		/// <param name="setorID"></param>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public DataTable tabelaLugarAssinatura(int apresentacaoID, int setorID, string codigo)
		{
			try
			{
				//DataTable lugares. Utilizado para a reserva da assinatura
				DataTable tabelaLugar = new DataTable();
				tabelaLugar.Columns.Add("IngressoID", typeof(int));
				tabelaLugar.Columns.Add("LugarID", typeof(int));
				tabelaLugar.Columns.Add("TipoLugar", typeof(string));
				tabelaLugar.Columns.Add("QtdeReservar", typeof(int));
				tabelaLugar.Columns.Add("QtdeReservado", typeof(int));
				tabelaLugar.Columns.Add("Reservado", typeof(bool));
				tabelaLugar.Columns.Add("Cod", typeof(string));

				string filtroCodigo = "AND Codigo = '" + codigo + "' ";
				if (codigo.Trim().Length == 0)
					filtroCodigo = "";


				bd.FecharConsulta();

				bd.Consulta(@"SELECT TOP 1 tIngresso.ID AS IngressoID, LugarID,LugarMarcado FROM tIngresso,tSetor 
								WHERE tIngresso.SetorID = tSetor.ID AND
								ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID + " " + filtroCodigo + " AND tIngresso.Status = 'D'");
				if (bd.Consulta().Read())
				{
					DataRow linha = tabelaLugar.NewRow();

					linha["IngressoID"] = (int)bd.LerInt("IngressoID");
					linha["LugarID"] = (int)bd.LerInt("LugarID");
					linha["TipoLugar"] = (string)bd.LerString("LugarMarcado");
					linha["QtdeReservar"] = 1;
					linha["Cod"] = codigo;

					tabelaLugar.Rows.Add(linha);
				}
				return tabelaLugar;



			}
			catch
			{
				throw;
			}
		}

		//Inicio feito por Fabio ***
		#region Novo

		public delegate void ProcessHandler(int id);
		public event ProcessHandler Processar;

		protected void OnCreate(int id)
		{
			if (Processar != null)
			{
				Processar(id);
			}
		}


		public int Acrescentar(int ApresentacaoSetorID, int EventoID, int ApresentacaoID, int SetorID,
			int EmpresaID, int LocalID, int BloqueioID, int qtd, int codigo, CTLib.BD database, bool trabalhandoTransaction, List<string> CodigosDeBarra)
		{
			int qtdAcrescentada = 0;
			int inicio = 0;
			// Verifica se o código já foi informado
			if (codigo == 0)
				inicio = this.UltimoCodigoPista(ApresentacaoSetorID); // Busca o último código para apresentacaoSetorID

			int codigoSequencial = this.UltimoCodigoSequencial(ApresentacaoID);


			string codigoBarra = string.Empty;

			// Adiciona os ingressos.
			int c = 0;
			for (int i = inicio; i < inicio + qtd; i++)
			{

				codigoBarra = (CodigosDeBarra != null && CodigosDeBarra.Count >= qtd) ? CodigosDeBarra[c] : string.Empty;
				codigoSequencial++;//próximo codigo sequencial
				if (this.NovoNaoMarcado(ApresentacaoSetorID, EventoID, ApresentacaoID, SetorID, EmpresaID, LocalID, BloqueioID, i + 1, database, codigoSequencial, codigoBarra))
				{
					OnCreate(this.Control.ID); // Dispara Event de controle.
					qtdAcrescentada++;
				}
				else
				{
					// Problemas. Por algum motivo o ingresso não foi inserido corretamente.
					if (trabalhandoTransaction)
					{
						// Se implementar Transaction, desfaze-la e lançar IngressoException.
						database.DesfazerTransacao();
						throw new IngressoException("Falha ao adicionar os ingressos!");
					}
					else
					{
						// Se não implementar, simplesmente pára.
						break;
					}
				}
				c++;
			}

			// Atualizar o campo de Ingressos Gerados
			if (qtdAcrescentada > 0)
			{
				ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
				apresentacaoSetor.Control.ID = ApresentacaoSetorID;
				apresentacaoSetor.AtualizarIngressosGerados(database);
			}

			return qtdAcrescentada;
		}

		public int Acrescentar(int ApresentacaoSetorID, int EventoID, int ApresentacaoID, int SetorID, int EmpresaID, int LocalID, int BloqueioID, int qtd, CTLib.BD database, bool trabalhandoTransaction, List<string> CodigosDeBarra)
		{
			return this.Acrescentar(ApresentacaoSetorID, EventoID, ApresentacaoID, SetorID, EmpresaID, LocalID, BloqueioID, qtd, 0, database, trabalhandoTransaction, CodigosDeBarra);
		}

		public int Acrescentar(int ApresentacaoSetorID, int EventoID, int ApresentacaoID, int SetorID, int EmpresaID, int LocalID, int BloqueioID, int qtd, List<string> CodigosDeBarra)
		{
			return this.Acrescentar(ApresentacaoSetorID, EventoID, ApresentacaoID, SetorID, EmpresaID, LocalID, BloqueioID, qtd, 0, bd, false, CodigosDeBarra);
		}

		public List<EstruturaGeracaoIngressos> GerarIngressosApresentacao(int empresaID, int localID, EstruturaGeracaoIngressos item)
		{
			List<EstruturaGeracaoIngressos> listaRetorno = new List<EstruturaGeracaoIngressos>();
			EstruturaGeracaoIngressos itemAdd;
			string setorNome = string.Empty;
			bool blnSemIngresso = false;
			try
			{
				// Preenche a lista final com todas as apresentações e setores que deverão gerar ingressos.

				// Todas as apresentações e setores do evento
				if (item.ApresentacaoID == 0)
				{
					#region Todas as apresentações e setores do evento
					using (IDataReader oDataReader = bd.Consulta("" +
						"SELECT DISTINCT " +
						"   tApresentacao.ID AS ApresentacaoID, " +
						"   tApresentacao.Horario AS ApresentacaoHorario, " +
						"   tSetor.ID AS SetorID, " +
						"   tSetor.Nome AS SetorNome, " +
						"   tApresentacaoSetor.ID AS ApresentacaoSetorID " +
						"FROM tApresentacao (NOLOCK) " +
						"INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
						"INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
						"WHERE " +
						"	(tApresentacao.EventoID = " + item.EventoID + ") " +
						"AND " +
						"   (tApresentacao.DisponivelAjuste = 'T') " +
						"AND " +
						"	(tApresentacaoSetor.IngressosGerados = 'F') " +
						"AND " +
						"   (tSetor.LugarMarcado <> '" + IRLib.Paralela.Setor.Pista + "')"))
					{

						while (oDataReader.Read())
						{
							itemAdd = new EstruturaGeracaoIngressos();
							itemAdd = item;
							itemAdd.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
							itemAdd.ApresentacaoID = bd.LerInt("ApresentacaoID");
							itemAdd.ApresentacaoHorario = bd.LerDateTime("ApresentacaoHorario").ToString();
							itemAdd.SetorID = bd.LerInt("SetorID");
							itemAdd.SetorNome = bd.LerString("SetorNome");
							itemAdd.Status = "";
							itemAdd.StatusErro = "";
							listaRetorno.Add(itemAdd);

							blnSemIngresso = true;
						}

						if (!blnSemIngresso)
						{
							itemAdd = new EstruturaGeracaoIngressos();
							itemAdd = item;
							itemAdd.ApresentacaoHorario = "Todas";
							itemAdd.SetorNome = "Todos";
							itemAdd.Status = "Erro ao gerar ingressos*";
							itemAdd.StatusErro = "Não existem ingresssos a gerar para o evento selecionado.";
							listaRetorno.Add(itemAdd);
						}
					}
					#endregion
				}
				else
				{
					// Todos os setores da apresentação
					if (item.SetorID == 0)
					{
						#region Todos os setores da apresentação
						using (IDataReader oDataReader = bd.Consulta("" +
						"SELECT DISTINCT " +
						"   tSetor.ID AS SetorID, " +
						"   tSetor.Nome AS SetorNome, " +
						"   tApresentacaoSetor.ID AS ApresentacaoSetorID " +
						"FROM tApresentacao (NOLOCK) " +
						"INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
						"INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
						"WHERE " +
						"	(tApresentacao.ID = " + item.ApresentacaoID + ") " +
						"AND " +
						"   (tApresentacao.DisponivelAjuste = 'T') " +
						"AND " +
						"	(tApresentacaoSetor.IngressosGerados = 'F') " +
						"AND " +
						"   (tSetor.LugarMarcado <> '" + IRLib.Paralela.Setor.Pista + "')"))
						{
							while (oDataReader.Read())
							{
								itemAdd = new EstruturaGeracaoIngressos();
								itemAdd = item;
								itemAdd.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
								itemAdd.SetorID = bd.LerInt("SetorID");
								itemAdd.SetorNome = bd.LerString("SetorNome");
								itemAdd.Status = "";
								itemAdd.StatusErro = "";
								listaRetorno.Add(itemAdd);

								blnSemIngresso = true;
							}
							if (!blnSemIngresso)
							{
								itemAdd = new EstruturaGeracaoIngressos();
								itemAdd = item;
								itemAdd.SetorNome = "Todos";
								itemAdd.Status = "Erro ao gerar ingressos*";
								itemAdd.StatusErro = "Não existem ingresssos a gerar para a apresentação selecionada.";
								listaRetorno.Add(itemAdd);
							}
						}
						#endregion
					}
					else
					{
						#region Adiciona o item sem a seleção de todos
						using (IDataReader oDataReader = bd.Consulta("" +
						"SELECT DISTINCT " +
						"   tApresentacaoSetor.ID AS ApresentacaoSetorID " +
						"FROM tApresentacao (NOLOCK) " +
						"INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
						"INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
						"WHERE " +
						"	(tApresentacaoSetor.SetorID = " + item.SetorID + ") " +
						"AND " +
						"	(tApresentacaoSetor.ApresentacaoID = " + item.ApresentacaoID + ") " +
						"AND " +
						"   (tApresentacao.DisponivelAjuste = 'T') " +
						"AND " +
						"	(tApresentacaoSetor.IngressosGerados = 'F') " +
						"AND " +
						"   (tSetor.LugarMarcado <> '" + IRLib.Paralela.Setor.Pista + "')"))
						{
							while (oDataReader.Read())
							{
								itemAdd = new EstruturaGeracaoIngressos();
								itemAdd = item;
								itemAdd.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
								itemAdd.Status = "";
								itemAdd.StatusErro = "";
								listaRetorno.Add(itemAdd);

								blnSemIngresso = true;
							}

							if (!blnSemIngresso)
							{
								itemAdd = new EstruturaGeracaoIngressos();
								itemAdd = item;
								itemAdd.Status = "Erro ao gerar ingressos*";
								itemAdd.StatusErro = "Não existem ingresssos a gerar para o setor selecionado.";
								listaRetorno.Add(itemAdd);
							}
						}
						#endregion
					}
				}


				// Realiza a chamada para geração dos ingressos de apresentação/setor individualmente
				for (int i = 0; i < listaRetorno.Count; i++)
				{
					itemAdd = listaRetorno[i];

					try
					{
						if (itemAdd.StatusErro == "")
						{
							if (this.GerarIngressos(itemAdd.SetorID, itemAdd.ApresentacaoSetorID, itemAdd.EventoID, itemAdd.ApresentacaoID, empresaID, localID, 0, ref setorNome, new List<string>(), Enumerators.TipoCodigoBarra.Estruturado))
							{
								itemAdd.Status = "Gerado com sucesso";
							}
							else
							{
								itemAdd.Status = "Erro ao gerar ingressos*";
								itemAdd.StatusErro = "Não foi possível capturar o detalhamento do erro.";
							}
						}
					}
					catch (Exception ex)
					{
						itemAdd.Status = "Erro ao gerar ingressos*";
						itemAdd.StatusErro = ex.Message;
					}

					listaRetorno[i] = itemAdd;
				}


			}
			catch
			{
				throw;
			}
			finally
			{
				bd.Fechar();
			}

			return listaRetorno;
		}

		/// <summary>
		/// Método responsável por Gerar ingressos com base em um SetorID - Os outros parâmetros são redundantes a fim de eliminar select's pesadas.
		/// </summary>
		/// <param name="setorID"></param>
		/// <param name="apresentacaoSetorID"></param>
		/// <param name="eventoID"></param>
		/// <param name="apresentacaoID"></param>
		/// <param name="empresaID"></param>
		/// <param name="localID"></param>
		/// <param name="qtd"></param>
		/// <returns></returns>
		public bool GerarIngressos(int setorID, int apresentacaoSetorID, int eventoID,
			int apresentacaoID, int empresaID, int localID, int qtd, ref string nomeSetor, List<String> CodigosDeBarra, Enumerators.TipoCodigoBarra tipoCodigoBarra)
		{
			Setor setor = null;
			BD lugares = null;
			try
			{

				// 1. Busca informação sobre o setor.
				setor = new Setor();
				lugares = setor.Lugares(setorID);
				int sequencia = this.UltimoCodigoSequencial(apresentacaoID) + 1;
				bd.IniciarTransacao();

				bool ok = true;
				if (lugares.Consulta().Read())
				{
					nomeSetor = lugares.LerString("SetorNome");

					if (lugares.LerString("LugarMarcado") == IRLib.Paralela.Setor.Pista)
					{ // Se for pista adiciona tudo de uma vez.
						int qtdAdd = this.Acrescentar(apresentacaoSetorID, eventoID, apresentacaoID, setorID, empresaID, localID, 0, qtd, 1, bd, true, CodigosDeBarra); // Inicia com código 1
						ok = (qtd == qtdAdd);
					}
					else
					{
						do
						{
							int qtde = lugares.LerInt("Quantidade");
							List<string> codigos = new List<string>();
							if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
							{
								codigos = CodigosDeBarra.Take(qtde).ToList();
								CodigosDeBarra.RemoveAll(c => codigos.Contains(c));
							}

							ok &= this.NovoMarcado(apresentacaoSetorID, eventoID, apresentacaoID, setorID, empresaID, localID, lugares.LerInt("BloqueioID"),
								lugares.LerInt("ID"), qtde, lugares.LerInt("QuantidadeBloqueada"), lugares.LerString("Codigo"), lugares.LerInt("Grupo"),
								lugares.LerInt("Classificacao"), lugares.LerString("LugarMarcado"), bd, sequencia, codigos.ToArray());
							sequencia += qtde;

						} while (lugares.Consulta().Read());
					}
				}

				// Se conseguiu gerar os ingressos, independente do tipo de lugar, o campo IngressosGerados da tabela tApresentacaoSetor deve ser atualizado para 'T'.
				if (ok)
				{
					ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
					apresentacaoSetor.Control.ID = apresentacaoSetorID;
					apresentacaoSetor.AtualizarIngressosGerados(bd);
				}


				lugares.FecharConsulta();
				lugares.Fechar();
				bd.FinalizarTransacao();
				bd.Fechar();

				return ok;
			}
			catch (Exception ex)
			{
				bd.DesfazerTransacao();
				throw new IngressoException(ex.Message);
			}
			finally
			{
				if (lugares != null)
					lugares.Fechar();

				if (setor != null)
					setor = null;

				bd.Fechar();

			}




		}


		/// <summary>
		/// Método responsável por remover uma quantidade X de ingressos de uma apresentação / Setor.
		/// </summary>
		/// <param name="ApresentacaoSetorID"></param>
		/// <param name="qtd"></param>
		/// <returns>Retorna um inteiro representando a quantidade de ingressos removidos</returns>
		public int Decrescentar(int apresentacaoSetorID, int qtd)
		{

			BD bdDelete = new BD();

			try
			{
				///
				/// 1. Buscar ingressos que disponíveis e que nunca tiveram nenhuma Venda.
				/// 2. Excluir um a um no banco de dados e contabilizar na variável. 
				///		Por ora temos que utilizar isso. No SQL Server 2005, existe a possibilidade de Deletar com TOP.
				/// 

				string sql = "	 SELECT TOP " + qtd + " tIngresso.ID FROM tIngresso (NOLOCK) " +
					"WHERE ApresentacaoSetorID = " + apresentacaoSetorID + " AND Status = '" + Ingresso.DISPONIVEL + "' AND ID NOT IN  " +
					"(SELECT IngressoID FROM tIngressoLog (NOLOCK) WHERE tIngresso.ID = tIngressoLog.IngressoID AND VendaBilheteriaItemID > 0) ORDER BY ID DESC";


				string deleteIngresso = "DELETE FROM tIngresso WHERE ID = {0}; DELETE FROM tIngressoLog WHERE IngressoID = {0}";

				bd.DesfazerTransacao();
				bd.FecharConsulta();

				bd.Consulta(sql);
				int qtdExcluida = 0;

				while (bd.Consulta().Read())
				{
					qtdExcluida += bdDelete.Executar(string.Format(deleteIngresso, bd.LerInt("ID"))) > 0 ? 1 : 0;
				}

				bd.Fechar();
				bdDelete.Fechar();
				return qtdExcluida;

			}
			catch (Exception ex)
			{
				throw new IngressoException("Ocorreu um erro ao DECRESCENTAR Ingressos: \n" + ex.Message);
			}
			finally
			{
				bd.Fechar();
				bdDelete.Fechar();
			}


		}
		/// <summary>
		/// Devolve o ultimo CodigoSequencial de determinada apresentação
		/// ATENÇÂO: ESSE MÉTODO NÂO FECHA O BD PARA UTILIZA-LO MAIS TARDE!!!!
		/// KIM
		/// </summary>
		/// <param name="apresentacaoID"></param>
		/// <returns></returns>
		public int UltimoCodigoSequencial(int apresentacaoID)
		{
			try
			{
				string sql = "SELECT MAX(CodigoSequencial) FROM tIngresso (NOLOCK) WHERE ApresentacaoID = " + apresentacaoID;

				int retorno = Convert.ToInt32(bd.ConsultaValor(sql));

				return retorno;
			}
			catch (Exception)
			{ throw; }
		}

		/// <summary>
		/// Inclui um novo ingresso nao-marcado.
		/// </summary>
		/// <returns></returns>
		public override bool NovoNaoMarcado(
			int apresentacaosetorid, int eventoid, int apresentacaoid,
			int setorid, int empresaid, int localid, int bloqueioid, int codigo, CTLib.BD database, int codigoSequencial, string codigoDeBarra)
		{

			try
			{

				//				Ingresso ingresso = new Ingresso();
				this.VendaBilheteriaID.Valor = 0;
				this.ApresentacaoSetorID.Valor = apresentacaosetorid;
				this.EventoID.Valor = eventoid;
				this.ApresentacaoID.Valor = apresentacaoid;
				this.SetorID.Valor = setorid;
				this.EmpresaID.Valor = empresaid;
				this.LocalID.Valor = localid;
				this.Codigo.Valor = codigo.ToString(Ingresso.FORMATO_CODIGO);
				this.LugarID.Valor = 0; // Lugar ID sempre igual a zero para lugar não marcado. Caso contrário gera erro no carregamento da venda.
				this.CodigoSequencial.Valor = codigoSequencial;
				this.CodigoBarra.Valor = codigoDeBarra;

				if (bloqueioid != 0)
				{
					this.BloqueioID.Valor = bloqueioid;
					this.Status.Valor = Ingresso.BLOQUEADO;
				}
				else
				{
					this.Status.Valor = Ingresso.DISPONIVEL;
					this.BloqueioID.Valor = 0;
				}

				if (!string.IsNullOrEmpty(codigoDeBarra))
					codigoBarra.AtualizaCodigoBarraListaBranca(database, codigoDeBarra);

				return this.Inserir(database);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Inclui um novo ingresso marcado.
		/// </summary>
		/// <returns></returns>
		public override bool NovoMarcado(int apresentacaosetorid, int eventoid, int apresentacaoid,
			int setorid, int empresaid, int localid, int bloqueioid, int lugarid, int qtdlugar, int qtdBloqueada, string codigo, int grupo,
			int classificacao, string tipoSetor, CTLib.BD database, int codigoSequencial, string[] codigosDeBarra)
		{
			try
			{
				bool ok = false;

				if (tipoSetor != IRLib.Paralela.Setor.Cadeira)
				{
					if (qtdlugar > 0)
					{
						if (qtdlugar > 702)
							throw new IngressoException("Lugar (" + lugarid + ") excedeu o limite de ingressos (quantidade).");
						//mesa pode haver ateh letra.Length-1 lugares, ou seja, 702 lugares
						//trabalhar com string deu mais certo que char.
						string[] letra = new string[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
														"aa","ab","ac","ad","ae","af","ag","ah","ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
														"ba","bb","bc","bd","be","bf","bg","bh","bi","bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz",
														"ca","cb","cc","cd","ce","cf","cg","ch","ci","cj","ck","cl","cm","cn","co","cp","cq","cr","cs","ct","cu","cv","cw","cx","cy","cz",
														"da","db","dc","dd","de","df","dg","dh","di","dj","dk","dl","dm","dn","do","dp","dq","dr","ds","dt","du","dv","dw","dx","dy","dz",
														"ea","eb","ec","ed","ee","ef","eg","eh","ei","ej","ek","el","em","en","eo","ep","eq","er","es","et","eu","ev","ew","ex","ey","ez",
														"fa","fb","fc","fd","fe","ff","fg","fh","fi","fj","fk","fl","fm","fn","fo","fp","fq","fr","fs","ft","fu","fv","fw","fx","fy","fz",
														"ga","gb","gc","gd","ge","gf","gg","gh","gi","gj","gk","gl","gm","gn","go","gp","gq","gr","gs","gt","gu","gv","gw","gx","gy","gz",
														"ha","hb","hc","hd","he","hf","hg","hh","hi","hj","hk","hl","hm","hn","ho","hp","hq","hr","hs","ht","hu","hv","hw","hx","hy","hz",
														"ia","ib","ic","id","ie","if","ig","ih","ii","ij","ik","il","im","in","io","ip","iq","ir","is","it","iu","iv","iw","ix","iy","iz",
														"ja","jb","jc","jd","je","jf","jg","jh","ji","jj","jk","jl","jm","jn","jo","jp","jq","jr","js","jt","ju","jv","jw","jx","jy","jz",
														"ka","kb","kc","kd","ke","kf","kg","kh","ki","kj","kk","kl","km","kn","ko","kp","kq","kr","ks","kt","ku","kv","kw","kx","ky","kz",
														"la","lb","lc","ld","le","lf","lg","lh","li","lj","lk","ll","lm","ln","lo","lp","lq","lr","ls","lt","lu","lv","lw","lx","ly","lz",
														"ma","mb","mc","md","me","mf","mg","mh","mi","mj","mk","ml","mm","mn","mo","mp","mq","mr","ms","mt","mu","mv","mw","mx","my","mz",
														"na","nb","nc","nd","ne","nf","ng","nh","ni","nj","nk","nl","nm","nn","no","np","nq","nr","ns","nt","nu","nv","nw","nx","ny","nz",
														"oa","ob","oc","od","oe","of","og","oh","oi","oj","ok","ol","om","on","oo","op","oq","or","os","ot","ou","ov","ow","ox","oy","oz",
														"pa","pb","pc","pd","pe","pf","pg","ph","pi","pj","pk","pl","pm","pn","po","pp","pq","pr","ps","pt","pu","pv","pw","px","py","pz",
														"qa","qb","qc","qd","qe","qf","qg","qh","qi","qj","qk","ql","qm","qn","qo","qp","qq","qr","qs","qt","qu","qv","qw","qx","qy","qz",
														"ra","rb","rc","rd","re","rf","rg","rh","ri","rj","rk","rl","rm","rn","ro","rp","rq","rr","rs","rt","ru","rv","rw","rx","ry","rz",
														"sa","sb","sc","sd","se","sf","sg","sh","si","sj","sk","sl","sm","sn","so","sp","sq","sr","ss","st","su","sv","sw","sx","sy","sz",
														"ta","tb","tc","td","te","tf","tg","th","ti","tj","tk","tl","tm","tn","to","tp","tq","tr","ts","tt","tu","tv","tw","tx","ty","tz",
														"ua","ub","uc","ud","ue","uf","ug","uh","ui","uj","uk","ul","um","un","uo","up","uq","ur","us","ut","uu","uv","uw","ux","uy","uz",
														"va","vb","vc","vd","ve","vf","vg","vh","vi","vj","vk","vl","vm","vn","vo","vp","vq","vr","vs","vt","vu","vv","vw","vx","vy","vz",
														"wa","wb","wc","wd","we","wf","wg","wh","wi","wj","wk","wl","wm","wn","wo","wp","wq","wr","ws","wt","wu","wv","ww","wx","wy","wz",
														"xa","xb","xc","xd","xe","xf","xg","xh","xi","xj","xk","xl","xm","xn","xo","xp","xq","xr","xs","xt","xu","xv","xw","xx","xy","xz",
														"ya","yb","yc","yd","ye","yf","yg","yh","yi","yj","yk","yl","ym","yn","yo","yp","yq","yr","ys","yt","yu","yv","yw","yx","yy","yz",
														"za","zb","zc","zd","ze","zf","zg","zh","zi","zj","zk","zl","zm","zn","zo","zp","zq","zr","zs","zt","zu","zv","zw","zx","zy","zz"};

						ok = true;

						int qtdeBloq = qtdBloqueada;

						for (int x = 0; x < qtdlugar; x++)
						{
							//Ingresso ingresso = new Ingresso();
							this.VendaBilheteriaID.Valor = 0;
							this.ApresentacaoSetorID.Valor = apresentacaosetorid;
							this.EventoID.Valor = eventoid;
							this.ApresentacaoID.Valor = apresentacaoid;
							this.SetorID.Valor = setorid;
							this.EmpresaID.Valor = empresaid;
							this.LocalID.Valor = localid;
							this.LugarID.Valor = lugarid;
							this.Grupo.Valor = grupo;
							this.Classificacao.Valor = classificacao;
							this.CodigoSequencial.Valor = codigoSequencial;
							this.CodigoBarra.Valor = (codigosDeBarra != null && codigosDeBarra.Length > 0) ? codigosDeBarra[x] : string.Empty;

							string cod;
							if (codigo != null && codigo != "")
							{
								try
								{
									cod = codigo + "-" + letra[x].ToUpper();
								}
								catch
								{
									cod = "*ND*-*";
								}
							}
							else
							{
								cod = "*ND*-" + letra[x].ToUpper();
							}
							this.Codigo.Valor = cod;

							if (bloqueioid != 0 && qtdeBloq > 0)
							{
								this.BloqueioID.Valor = bloqueioid;
								this.Status.Valor = Ingresso.BLOQUEADO;
								qtdeBloq--;
							}
							else
							{
								this.Status.Valor = Ingresso.DISPONIVEL;
								this.BloqueioID.Valor = 0;
							}

							ok &= this.Inserir(database);
							codigoSequencial++;

							if (codigosDeBarra != null && codigosDeBarra.Length > 0)
								codigoBarra.AtualizaCodigoBarraListaBranca(database, codigosDeBarra[x]);
						}
					}
					return ok;
				}
				else
				{
					if (qtdlugar == 1)
					{
						//throw new IngressoException("Lugar ("+lugarid+") possui zero ingressos (quantidade).");

						//Ingresso ingresso = new Ingresso();
						this.ApresentacaoSetorID.Valor = apresentacaosetorid;
						this.EventoID.Valor = eventoid;
						this.ApresentacaoID.Valor = apresentacaoid;
						this.SetorID.Valor = setorid;
						this.EmpresaID.Valor = empresaid;
						this.LocalID.Valor = localid;
						this.LugarID.Valor = lugarid;
						this.Grupo.Valor = grupo;
						this.Classificacao.Valor = classificacao;
						this.CodigoSequencial.Valor = codigoSequencial;
						this.CodigoBarra.Valor = (codigosDeBarra != null && codigosDeBarra.Length > 0) ? codigosDeBarra[0] : string.Empty;
						if (codigo != null && codigo != "")
							this.Codigo.Valor = codigo;
						else
							this.Codigo.Valor = "*ND*";

						if (bloqueioid != 0)
						{
							this.BloqueioID.Valor = bloqueioid;
							this.Status.Valor = Ingresso.BLOQUEADO;
						}
						else
						{
							this.Status.Valor = Ingresso.DISPONIVEL;
							this.BloqueioID.Valor = 0;
						}

						ok = this.Inserir(database);

						if (codigosDeBarra != null && codigosDeBarra.Length > 0)
							codigoBarra.AtualizaCodigoBarraListaBranca(database, codigosDeBarra[0]);
					}
				}

				return ok;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		public static void PreencherCodigoSequencial()
		{
			BD bd = new BD();
			try
			{
				System.Collections.Generic.List<int> ids = new System.Collections.Generic.List<int>();
				System.Collections.Generic.List<int> Ingressosids = new System.Collections.Generic.List<int>();
				string sqlIDs = "SELECT DISTINCT ID FROM tApresentacao";
				bd.Executar(sqlIDs);
				int controle = 1;
				string sqlUpdate = "";
				while (bd.Consulta().Read())
				{
					ids.Add(bd.LerInt("ID"));
				}
				bd.Fechar();
				for (int i = 0; i < ids.Count; i++)
				{
					int sequencia = 1;
					controle = 1;
					while (controle == 1)
					{
						sqlUpdate = "UPDATE TOP (1) tIngresso SET CodigoSequencial = " + sequencia + " WHERE ApresentacaoID=" + ids[i] + " AND CodigoSequencial IS NULL ";
						controle = bd.Executar(sqlUpdate);
						sequencia++;
					}
				}
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

		#region Info



		/// <summary>
		/// Retorna uma tabela estatica com os campos necessarios para impressao do ingresso
		/// </summary>
		/// <returns></returns>
		public static DataTable EstruturaImpressao()
		{

			DataTable tabela = new DataTable(BilheteriaParalela.TABELA_ESTRUTURA_IMPRESSAO);

			tabela.Columns.Add("ID", typeof(int)).DefaultValue = 0; //id do Ingressolog
			tabela.Columns.Add("IngressoID", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("Usuario", typeof(string)).DefaultValue = ""; //nome do Usuario
			tabela.Columns.Add("Evento", typeof(string)).DefaultValue = ""; //nome do Evento
			tabela.Columns.Add("Horario", typeof(DateTime)).DefaultValue = new DateTime(); //horario da Apresentacao
			tabela.Columns.Add("Setor", typeof(string)).DefaultValue = ""; //nome do Setor
			tabela.Columns.Add("Acesso", typeof(string)).DefaultValue = ""; //Acesso do setor
			tabela.Columns.Add("TipoSetor", typeof(string)).DefaultValue = ""; //nome do Setor
			tabela.Columns.Add("ImprimirCarimbo", typeof(string)).DefaultValue = ""; //Imprimi carimbo?
			tabela.Columns.Add("CarimboTexto1", typeof(string)).DefaultValue = ""; //primeiro campo do carimbo
			tabela.Columns.Add("CarimboTexto2", typeof(string)).DefaultValue = ""; //segundo campo do carimbo
			tabela.Columns.Add("Produto", typeof(bool)).DefaultValue = false; //se o setor eh produto ou nao
			tabela.Columns.Add("Cortesia", typeof(string)).DefaultValue = ""; //nome da Cortesia
			tabela.Columns.Add("CortesiaID", typeof(int)).DefaultValue = 0; //id da Cortesia
			tabela.Columns.Add("BloqueioID", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("Preco", typeof(string)).DefaultValue = ""; //nome do preco
			tabela.Columns.Add("PrecoID", typeof(int)).DefaultValue = 0; //id do preco
			tabela.Columns.Add("PrecoImpressao", typeof(string)).DefaultValue = ""; //impressao do preco
			tabela.Columns.Add("Valor", typeof(decimal)).DefaultValue = 0; //valor do Ingresso
			tabela.Columns.Add("Codigo", typeof(string)).DefaultValue = ""; //codigo do Ingresso
			tabela.Columns.Add("CodigoBarra", typeof(string)).DefaultValue = ""; //senha da venda
			tabela.Columns.Add("CodigoBarraCliente", typeof(string)).DefaultValue = ""; //senha da venda
			tabela.Columns.Add("Senha", typeof(string)).DefaultValue = ""; //senha da venda
			tabela.Columns.Add("Loja", typeof(string)).DefaultValue = ""; //nome da loja q fez a venda
			tabela.Columns.Add("TipoVenda", typeof(string)).DefaultValue = ""; //tipo de venda do canal (imprime vouher, imprime ingresso, venda remota)
			tabela.Columns.Add("GerenciamentoIngressos", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("GerenciamentoIngressosID", typeof(int)).DefaultValue = 0;

			//Dados do cliente que comprou o ingresso ou do Dono do Ingresso (COTA)
			tabela.Columns.Add("Cliente", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("ClienteRG", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("ClienteID", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("ClienteCPF", typeof(string)).DefaultValue = string.Empty;
			tabela.Columns.Add("ClienteEnderecoID", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("ClienteVenda", typeof(string));
			tabela.Columns.Add("ClienteVendaEmail", typeof(string));
			tabela.Columns.Add("TaxaConveniencia", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal)).DefaultValue = 0;
			tabela.Columns.Add("TaxaMinima", typeof(decimal)).DefaultValue = 0;
			tabela.Columns.Add("TaxaMaxima", typeof(decimal)).DefaultValue = 0;

			//campos usados para as imagens
			tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = 0; //id do Evento
			tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = 0; //id da Apresentacao
			tabela.Columns.Add("ApresentacaoImpressao", typeof(string)).DefaultValue = ""; //impressao da Apresentacao
			tabela.Columns.Add("ApresentacaoSetorID", typeof(int)).DefaultValue = 0; //id da ApresentacaoSetorID

			//apenas para impressao de ingresso (essas sao taxas de total da venda toda e nao por ingresso)
			tabela.Columns.Add("TaxaEntrega", typeof(string));
			tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));
			tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
			tabela.Columns.Add("TaxaEntregaID", typeof(int));
			tabela.Columns.Add("TaxaEntregaTipo", typeof(int));

			tabela.Columns.Add("Status", typeof(string)).DefaultValue = ""; //status do ingresso;
			//tabela.Columns.Add("VendaBilheteriaItemID", typeof(int)).DefaultValue = 0; //VendaBilheteriaItemID
			tabela.Columns.Add("VendaBilheteriaID", typeof(int)).DefaultValue = 0; //VendaBilheteriaID

			tabela.Columns.Add("PacoteID", typeof(int)).DefaultValue = 0; //id do Pacote (caso o ingresso seja de um pacote)
			tabela.Columns.Add("Pacote", typeof(string)).DefaultValue = ""; //nome do Pacote

			// para evitar o bug do clock
			tabela.Columns.Add("HorarioString", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("DataVendaString", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool)).DefaultValue = false;
			tabela.Columns.Add("LocalID", typeof(int));
			tabela.Columns.Add("LocalNome", typeof(string));
			tabela.Columns.Add("LocalEndereco", typeof(string));
			tabela.Columns.Add("LocalCidade", typeof(string));
			tabela.Columns.Add("LocalEstado", typeof(string));
			tabela.Columns.Add("LocalPais", typeof(string));
			tabela.Columns.Add("LocalCep", typeof(string));
			tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));
			tabela.Columns.Add("CodigoSequencial", typeof(int));
			tabela.Columns.Add("CodigoImpressao", typeof(int));
			tabela.Columns.Add("ObrigatoriedadeID", typeof(int)).DefaultValue = 0;
			tabela.Columns.Add("FormasPagamento", typeof(string));
			tabela.Columns.Add("Bin", typeof(string));
			tabela.Columns.Add("DiasTriagem", typeof(int));
			tabela.Columns.Add("VendaBilheteriaStatus", typeof(string)).DefaultValue = string.Empty;
			tabela.Columns.Add("NomeEntrega", typeof(string));
			tabela.Columns.Add("RGEntrega", typeof(string));
			tabela.Columns.Add("EnderecoEntrega", typeof(string));
			tabela.Columns.Add("NumeroEntrega", typeof(string));
			tabela.Columns.Add("ComplementoEntrega", typeof(string));
			tabela.Columns.Add("BairroEntrega", typeof(string));
			tabela.Columns.Add("CidadeEntrega", typeof(string));
			tabela.Columns.Add("EstadoEntrega", typeof(string));
			tabela.Columns.Add("CEPEntrega", typeof(string));

			tabela.Columns.Add("EntregaNome", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("PeriodoEntrega", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("DataEntrega", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("AreaEntrega", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("TipoImpressao", typeof(string)).DefaultValue = ((char)EstruturaTipoImpressao.TipoImpressao.Laser).ToString();
			tabela.Columns.Add("NomeCartao", typeof(string)).DefaultValue = "";
			tabela.Columns.Add("TipoCodigoBarra", typeof(string));

			tabela.Columns.Add("Obs", typeof(string)).DefaultValue = string.Empty;

            tabela.Columns.Add("FonteImposto", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("Alvara", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("AVCB", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("PorcentagemImposto", typeof(decimal)).DefaultValue = 0;

            tabela.Columns.Add("Lotacao", typeof(int));
            tabela.Columns.Add("DataEmissaoAlvara", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("DataValidadeAlvara", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("DataEmissaoAvcb", typeof(string)).DefaultValue = string.Empty;
            tabela.Columns.Add("DataValidadeAvcb", typeof(string)).DefaultValue = string.Empty;

            return tabela;
		}

		/// <summary>
		/// Obtem informações (evento,apresentacao,setor...) desse ingresso vendido
		/// </summary>
		/// <returns></returns>
		public override DataTable InfoVendidos(int[] ingressosids)
		{
			//fabio

			try
			{

				DataTable tabela = EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter
				StringBuilder sb = new StringBuilder();

				sb.Append("SELECT i.ID,l.Nome as Loja, u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.CodigoSequencial,  ");
				sb.Append("i.EventoID,a.Horario,i.ApresentacaoID,i.ApresentacaoSetorID,s.Nome AS Setor,s.Produto, ");
				sb.Append("c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao, ");
				sb.Append("p.Impressao AS PrecoImpressao,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente, ");
				sb.Append("ci.ID AS ClienteID,ci.Nome AS Cliente,ci.RG AS ClienteRG,vb.Senha,vb.TaxaConvenienciaValorTotal, ");
				sb.Append("vb.TaxaEntregaValor,vbi.TaxaConvenienciaValor,vbi.TaxaConveniencia,vb.DataVenda ");
				sb.Append("FROM tUsuario as u ");
				sb.Append("INNER JOIN tIngressoLog as il ON il.UsuarioID=u.ID ");
				sb.Append("INNER JOIN tIngresso as i ON i.ID=il.IngressoID ");
				sb.Append("INNER JOIN tVendaBilheteriaItem as vbi ON il.VendaBilheteriaItemID=vbi.ID ");
				sb.Append("INNER JOIN tVendaBilheteria as vb ON vbi.VendaBilheteriaID=vb.ID ");
				sb.Append("INNER JOIN tPreco as p ON il.PrecoID=p.ID ");
				sb.Append("INNER JOIN tSetor as s ON i.SetorID=s.ID ");
				sb.Append("INNER JOIN tApresentacao as a ON i.ApresentacaoID=a.ID ");
				sb.Append("INNER JOIN tEvento as e ON i.EventoID=e.ID ");
				sb.Append("LEFT JOIN tCliente as ci ON vb.ClienteID=ci.ID ");
				sb.Append("LEFT JOIN tCortesia AS c ON il.CortesiaID=c.ID ");
				sb.Append("LEFT JOIN tLoja AS l ON i.LojaID=l.ID ");
				sb.Append("WHERE il.Acao='" + IngressoLog.VENDER + "' ");
				sb.Append("AND i.ID in (" + Utilitario.ArrayToString(ingressosids) + ") ");
				sb.Append("ORDER BY i.Codigo");
				string sql = sb.ToString();
				//SELECT ANTIGA -> UTILIZAVA *= MODIFICADO POR RENATO
				//   string sql = "SELECT i.ID,l.Nome as Loja, u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,i.ApresentacaoSetorID,s.Nome AS Setor,s.Produto,c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,ci.ID AS ClienteID,ci.Nome AS Cliente,ci.RG AS ClienteRG,vb.Senha,vb.TaxaConvenienciaValorTotal,vb.TaxaEntregaValor,vbi.TaxaConvenienciaValor,vbi.TaxaConveniencia,vb.DataVenda "+
				//   "FROM tLoja as l, tUsuario as u, tIngresso as i, tEvento as e, tApresentacao as a,tSetor as s,tPreco as p, tCortesia as c, tCliente as ci, tVendaBilheteriaItem as vbi, tVendaBilheteria as vb, tIngressoLog as il "+
				//   "WHERE i.LojaID*=l.ID AND i.ID=il.IngressoID AND il.Acao='"+IngressoLog.VENDER+"' AND il.VendaBilheteriaItemID=vbi.ID AND vbi.VendaBilheteriaID=vb.ID AND vb.ClienteID*=ci.ID AND "+
				//   "i.ApresentacaoID=a.ID AND i.SetorID=s.ID AND i.EventoID=e.ID AND il.PrecoID=p.ID AND il.CortesiaID*=c.ID AND il.UsuarioID=u.ID AND i.ID in ("+Utilitario.ArrayToString(ingressosids)+") ORDER BY i.Codigo";

				bd.Consulta(sql);

				while (bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["ID"] = bd.LerInt("ID");
					linha["Usuario"] = bd.LerString("Usuario");
					linha["Evento"] = bd.LerString("Evento");
					linha["EventoID"] = bd.LerInt("EventoID");
					linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
					linha["Horario"] = bd.LerDateTime("Horario");
					linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
					linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
					linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
					linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
					linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
					linha["Setor"] = bd.LerString("Setor");
					linha["Produto"] = bd.LerBoolean("Produto");
					linha["Cortesia"] = bd.LerString("Cortesia");
					linha["Preco"] = bd.LerString("Preco");
					linha["Valor"] = bd.LerDecimal("Valor");
					linha["Codigo"] = bd.LerString("Codigo");
					linha["CodigoBarra"] = bd.LerString("CodigoBarra");
					linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
					linha["Loja"] = bd.LerString("Loja");
					linha["Senha"] = bd.LerString("Senha");
					linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
					linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
					linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
					linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
					linha["Cliente"] = bd.LerString("Cliente");
					linha["ClienteRG"] = bd.LerString("ClienteRG");
					linha["ClienteID"] = bd.LerInt("ClienteID");
					linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();


				this.info = null; //associa ao info do ingresso

				return tabela;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Obtem informações (evento,apresentacao,setor...) desse ingresso vendido
		/// </summary>
		/// <returns></returns>
		public override DataRow InfoVendido()
		{
			//fabio

			try
			{

				DataTable tabelaAux = EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

				DataRow linha = tabelaAux.NewRow();
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT top 1 il.ID, il.IngressoID, i.Status, l.Nome as Loja, u.Login AS Usuario,e.ImpressaoCodigoBarra, i.CodigoSequencial ");
				sb.Append("e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,i.ApresentacaoSetorID,s.Nome AS Setor,s.Produto, ");
				sb.Append("c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao, ");
				sb.Append("p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,ci.ID AS ClienteID,ci.Nome AS Cliente,ci.RG AS ClienteRG, ");
				sb.Append("vb.Senha,vb.TaxaConvenienciaValorTotal,vb.TaxaEntregaValor,vbi.TaxaConvenienciaValor,vbi.TaxaConveniencia, ");
				sb.Append("il.VendaBilheteriaID,vbi.PacoteID,pac.Nome AS Pacote,vb.DataVenda ");
				sb.Append("FROM tIngresso i ");
				sb.Append("INNER JOIN tIngressoLog il ON i.ID=il.IngressoID ");
				sb.Append("INNER JOIN tVendaBilheteriaItem vbi ON il.VendaBilheteriaItemID=vbi.ID ");
				sb.Append("INNER JOIN tVendaBilheteria vb ON vbi.VendaBilheteriaID=vb.ID ");
				sb.Append("INNER JOIN tApresentacao a ON i.ApresentacaoID=a.ID ");
				sb.Append("INNER JOIN tSetor s ON i.SetorID=s.ID ");
				sb.Append("INNER JOIN tEvento e ON i.EventoID=e.ID ");
				sb.Append("INNER JOIN tPreco p ON il.PrecoID=p.ID ");
				sb.Append("INNER JOIN tUsuario u ON il.UsuarioID=u.ID ");
				sb.Append("LEFT JOIN tPacote pac ON vbi.PacoteID=pac.ID ");
				sb.Append("LEFT JOIN tLoja l ON i.LojaID=l.ID ");
				sb.Append("LEFT JOIN tCliente ci ON vb.ClienteID=ci.ID ");
				sb.Append("LEFT JOIN tCortesia c ON il.CortesiaID=c.ID ");
				sb.Append("WHERE  il.Acao='" + IngressoLog.VENDER + "' AND il.IngressoID=" + this.Control.ID + " ");
				sb.Append("ORDER BY il.ID DESC");
				string sql = sb.ToString();
				// SELECT ANTIGO -> UTILIZAVA *= MODIFICADO POR RENATO
				//string sql = "SELECT top 1 il.ID, il.IngressoID, i.Status, l.Nome as Loja, u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,i.ApresentacaoSetorID,s.Nome AS Setor,s.Produto,c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,ci.ID AS ClienteID,ci.Nome AS Cliente,ci.RG AS ClienteRG,vb.Senha,vb.TaxaConvenienciaValorTotal,vb.TaxaEntregaValor,vbi.TaxaConvenienciaValor,vbi.TaxaConveniencia,il.VendaBilheteriaID,vbi.PacoteID,pac.Nome AS Pacote,vb.DataVenda " +
				//    "FROM tLoja as l, tUsuario as u, tIngresso as i, tEvento as e, tApresentacao as a,tSetor as s,tPreco as p, tCortesia as c, tCliente as ci, tVendaBilheteriaItem as vbi, tVendaBilheteria as vb, tIngressoLog as il,tPacote as pac " +
				//    "WHERE vbi.PacoteID*=pac.ID AND i.LojaID*=l.ID AND i.ID=il.IngressoID AND il.Acao='" + IngressoLog.VENDER + "' AND il.VendaBilheteriaItemID=vbi.ID AND vbi.VendaBilheteriaID=vb.ID AND vb.ClienteID*=ci.ID AND " +
				//    "i.ApresentacaoID=a.ID AND i.SetorID=s.ID AND i.EventoID=e.ID AND il.PrecoID=p.ID AND il.CortesiaID*=c.ID AND il.UsuarioID=u.ID AND il.IngressoID=" + this.Control.ID + " ORDER BY il.ID DESC";

				bd.Consulta(sql);

				if (bd.Consulta().Read())
				{
					linha["ID"] = bd.LerInt("ID");
					linha["IngressoID"] = bd.LerInt("IngressoID");
					linha["Usuario"] = bd.LerString("Usuario");
					linha["Evento"] = bd.LerString("Evento");
					linha["Pacote"] = bd.LerString("Pacote");
					linha["EventoID"] = bd.LerInt("EventoID");
					linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
					linha["PacoteID"] = bd.LerInt("PacoteID");
					linha["Horario"] = bd.LerDateTime("Horario");
					linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
					linha["DataVendaString"] = bd.LerStringFormatoDataHora("DataVenda");
					linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
					linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
					linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
					linha["Setor"] = bd.LerString("Setor");
					linha["Produto"] = bd.LerBoolean("Produto");
					linha["Cortesia"] = bd.LerString("Cortesia");
					linha["Preco"] = bd.LerString("Preco");
					linha["Valor"] = bd.LerDecimal("Valor");
					linha["Codigo"] = bd.LerString("Codigo");
					linha["CodigoBarra"] = bd.LerString("CodigoBarra");
					linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
					linha["Loja"] = bd.LerString("Loja");
					linha["Senha"] = bd.LerString("Senha");
					linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
					linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
					linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
					linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
					linha["Cliente"] = bd.LerString("Cliente");
					linha["ClienteRG"] = bd.LerString("ClienteRG");
					linha["ClienteID"] = bd.LerInt("ClienteID");
					linha["Status"] = bd.LerString("Status");
					linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
					linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
				}
				bd.Fechar();

				tabelaAux.Rows.Add(linha);

				this.info = linha; //associa ao info do ingresso

				return linha;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public string Usuario
		{
			get
			{
				if (info != null)
				{
					string usuario = (string)info["Usuario"];
					return usuario;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		public string Evento
		{
			get
			{
				if (info != null)
				{
					string evento = (string)info["Evento"];
					return evento;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		// ************************************************************							
		//		public int EventoID{
		//			get{
		//				if (info!=null){
		//					int eventoID = (int)info["EventoID"];
		//					return eventoID;
		//				}else
		//					throw new IngressoException(ERRO_INFO);
		//			}
		//		}

		public DateTime Horario
		{
			get
			{
				if (info != null)
				{
					DateTime horario = (DateTime)info["Horario"];
					return horario;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		// ************************************************************							
		//		public int ApresentacaoID{
		//			get{
		//				if (info!=null){
		//					int apresentacaoID = (int)info["ApresentacaoID"];
		//					return apresentacaoID;
		//				}else
		//					throw new IngressoException(ERRO_INFO);
		//			}
		//		}

		/*
		public int ApresentacaoSetorID{
			get{
				if (base.ApresentacaoSetorID.Valor==""){
					if (info!=null){
						int apresentacaoSetorID = (int)info["ApresentacaoSetorID"];
						return apresentacaoSetorID;
					}else
						throw new IngressoException(ERRO_INFO);
				}else{
					return base.ApresentacaoSetorID.Valor;
				}
			}
		}
		*/

		public bool Produto
		{
			get
			{
				if (info != null)
				{
					bool produto = (bool)info["Produto"];
					return produto;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		public string Setor
		{
			get
			{
				if (info != null)
				{
					string setor = (string)info["Setor"];
					return setor;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		public string Cortesia
		{
			get
			{
				if (info != null)
				{
					string cortesia = (string)info["Cortesia"];
					return cortesia;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		public string Preco
		{
			get
			{
				if (info != null)
				{
					string preco = (string)info["Preco"];
					return preco;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		public decimal Valor
		{
			get
			{
				if (info != null)
				{
					decimal valor = (decimal)info["Valor"];
					return valor;
				}
				else
				{
					if (this.PrecoID.Valor == 0)
						throw new IngressoException("Ingresso não lido ou não possui preço.");

					try
					{
						decimal valor = 0;

						Preco preco = new Preco();
						preco.Control.ID = this.PrecoID.Valor;

						valor = preco.Valor.Valor;

						return valor;

					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
		}

		//		public string Codigo{
		//			get{
		//				if (base.Codigo.Valor==""){
		//					if (info!=null){
		//						string codigo = (string)info["Codigo"];
		//						return codigo;
		//					}else
		//						throw new IngressoException(ERRO_INFO);
		//				}else{
		//					return base.Codigo.Valor;
		//				}
		//			}
		//		}

		public string Loja
		{
			get
			{
				if (info != null)
				{
					string loja = (string)info["Loja"];
					return loja;
				}
				else
					throw new IngressoException(ERRO_INFO_V);
			}
		}

		public string Senha
		{
			get
			{
				if (info != null)
				{
					string senha = (string)info["Senha"];
					return senha;
				}
				else
					throw new IngressoException(ERRO_INFO_V);
			}
		}

		public string Cliente
		{
			get
			{
				if (info != null)
				{
					string cliente = (string)info["Cliente"];
					return cliente;
				}
				else
					throw new IngressoException(ERRO_INFO);
			}
		}

		//		public int ClienteID
		//		{
		//			get
		//			{
		//				if (info!=null)
		//				{
		//					int clienteID = (int)info["ClienteID"];
		//					return clienteID;
		//				}
		//				else
		//					throw new IngressoException(ERRO_INFO);
		//			}
		//		}

		public int TaxaConveniencia
		{
			get
			{
				if (info != null)
				{
					int taxaConveniencia = (int)info["TaxaConveniencia"];
					return taxaConveniencia;
				}
				else
					throw new IngressoException(ERRO_INFO_V);
			}
		}
		///internet
		private decimal txConv = 0;
		public decimal TxConv
		{
			get { return this.txConv; }
			set { this.txConv = value; }
		}
		public decimal TaxaProcessamentoValor { get; set; }

		public decimal TaxaConvenienciaValor
		{
			get
			{
				if (info != null)
				{
					decimal taxaConvenienciaValor = (decimal)info["TaxaConvenienciaValor"];
					return taxaConvenienciaValor;
				}
				else
					throw new IngressoException(ERRO_INFO_V);
			}
		}

		/// <summary>
		/// Obtem informações (evento,apresentacao,setor...) desse ingresso reservado
		/// </summary>
		/// <returns></returns>
		public DataTable InfoReservadosInternet(int[] ingressosids, CTLib.BD database)
		{
			//fabio

			try
			{

				DataTable tabela = EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

				string sql = @"SELECT 
								i.ID, u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,s.Nome AS Setor,s.Produto,c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao,i.PrecoID,i.CortesiaID,i.BloqueioID,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,i.ApresentacaoSetorID,l.Nome AS Loja 
								FROM 
								tUsuario as u (NOLOCK)
								INNER JOIN tIngresso as i (NOLOCK) ON i.UsuarioID = u.ID
								INNER JOIN tEvento as e (NOLOCK) ON i.EventoID = e.ID
								INNER JOIN tApresentacao as a (NOLOCK) ON i.ApresentacaoID=a.ID
								INNER JOIN tSetor as s(NOLOCK) ON i.SetorID=s.ID 
								INNER JOIN tPreco as p(NOLOCK) ON i.PrecoID=p.ID AND i.ApresentacaoSetorID=p.ApresentacaoSetorID
								LEFT JOIN tCortesia as c(NOLOCK) ON  i.CortesiaID = c.ID
								LEFT JOIN tLoja  AS l (NOLOCK) ON i.LojaID = l.ID
								WHERE i.ID in (" + Utilitario.ArrayToString(ingressosids) + @") 
								ORDER BY i.Codigo";



				database.Consulta(sql);

				Servidor servidor = new Servidor();

				while (database.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["ID"] = database.LerInt("ID");
					linha["Usuario"] = database.LerString("Usuario");
					linha["Evento"] = database.LerString("Evento");
					linha["EventoID"] = database.LerInt("EventoID");
					linha["ImpressaoCodigoBarra"] = database.LerBoolean("ImpressaoCodigoBarra");
					linha["Horario"] = database.LerDateTime("Horario");
					linha["HorarioString"] = database.LerStringFormatoDataHora("Horario");
					linha["DataVendaString"] = servidor.Agora.ToString(Utilitario.FormatoDataHora);
					linha["ApresentacaoID"] = database.LerInt("ApresentacaoID");
					linha["ApresentacaoImpressao"] = database.LerString("ApresentacaoImpressao");
					linha["PrecoImpressao"] = database.LerString("PrecoImpressao");
					linha["ApresentacaoSetorID"] = database.LerInt("ApresentacaoSetorID");
					linha["Setor"] = database.LerString("Setor");
					linha["Produto"] = database.LerBoolean("Produto");
					linha["Cortesia"] = database.LerString("Cortesia");
					linha["CortesiaID"] = database.LerInt("CortesiaID");
					linha["BloqueioID"] = database.LerInt("BloqueioID");
					linha["Preco"] = database.LerString("Preco");
					linha["PrecoID"] = database.LerInt("PrecoID");
					linha["Loja"] = database.LerString("Loja");
					linha["Valor"] = database.LerDecimal("Valor");
					linha["Codigo"] = database.LerString("Codigo");
					linha["CodigoBarra"] = database.LerString("CodigoBarra");
					linha["CodigoBarraCliente"] = database.LerString("CodigoBarraCliente");

					tabela.Rows.Add(linha);
				}

				this.info = null; //associa ao info do ingresso

				return tabela;

			}
			catch
			{
				throw;
			}
			finally
			{
				database.FecharConsulta();
			}
		}


		/// <summary>
		/// Obtem informações (evento,apresentacao,setor...) desse ingresso reservado
		/// </summary>
		/// <returns></returns>
		public override DataTable InfoReservados(int[] ingressosids, CTLib.BD database, int lojaID)
		{
			//fabio

			try
			{

				DateTime inicio = DateTime.Now;

				DataTable tabela = EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT i.ID, u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID, i.CodigoSequencial, ");
				sb.Append("a.Horario,i.ApresentacaoID,s.Nome AS Setor,s.Acesso,p.ImprimirCarimbo,p.CarimboTexto1,p.CarimboTexto2,s.Produto,s.LugarMarcado AS TipoSetor, c.Nome AS Cortesia,p.Nome AS Preco, ");
				sb.Append("a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao,i.PrecoID,i.CortesiaID, ");
				sb.Append("i.BloqueioID,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,i.ApresentacaoSetorID, ");
				sb.Append("l.Nome AS Loja,tLocal.ID AS LocalID, tLocal.Nome AS LocalNome, tLocal.Logradouro, tLocal.Numero, tLocal.Cidade AS LocalCidade,tLocal.Estado AS LocalEstado,tLocal.Cep AS LocalCep,e.ObrigaCadastroCliente, IsNull(e.ObrigatoriedadeID, 0) AS ObrigatoriedadeID, ");
				sb.Append("tPais.Nome AS Pais,i.GerenciamentoIngressosID,  ");
                sb.Append("CASE WHEN el.ID IS NOT NULL THEN el.TipoImpressao ELSE e.TipoImpressao END AS TipoImpressao, e.TipoCodigoBarra, ");
                sb.Append(" CASE WHEN LEN(a.Alvara) > 0 ");
                sb.Append(" THEN a.Alvara ");
                sb.Append(" ELSE ");
                sb.Append(" CASE WHEN LEN (e.Alvara) > 0 ");
                sb.Append(" THEN e.Alvara ");
                sb.Append(" ELSE tLocal.Alvara ");
                sb.Append(" END ");
                sb.Append(" END AS Alvara, ");
                sb.Append(" CASE WHEN LEN (e.FonteImposto) > 0 ");
                sb.Append(" THEN e.FonteImposto ");
                sb.Append(" ELSE tLocal.FonteImposto ");
                sb.Append(" END AS FonteImposto, ");
                sb.Append(" CASE WHEN LEN(a.AVCB) > 0 ");
                sb.Append(" THEN a.AVCB ");
                sb.Append(" ELSE ");
                sb.Append(" CASE WHEN LEN (e.AVCB) > 0 ");
                sb.Append(" THEN e.AVCB ");
                sb.Append(" ELSE tLocal.AVCB ");
                sb.Append(" END ");
                sb.Append(" END AS AVCB, ");
                sb.Append(" CASE WHEN e.PorcentagemImposto > 0 ");
                sb.Append(" THEN e.PorcentagemImposto ");
                sb.Append(" ELSE tLocal.PorcentagemImposto ");
                sb.Append(" END AS PorcentagemImposto, ");

                sb.Append(" CASE WHEN LEN(a.Lotacao) > 0 ");
                sb.Append(" THEN a.Lotacao   ");
                sb.Append(" ELSE CASE WHEN LEN (e.Lotacao) > 0 ");
                sb.Append(" THEN e.Lotacao   ");
                sb.Append(" ELSE tLocal.Lotacao ");
                sb.Append(" END  END AS Lotacao, ");

                sb.Append(" CASE WHEN LEN(a.DataEmissaoAlvara) > 0 ");
                sb.Append(" THEN a.DataEmissaoAlvara   ");
                sb.Append(" ELSE CASE WHEN LEN (e.DataEmissaoAlvara) > 0 ");
                sb.Append(" THEN e.DataEmissaoAlvara ");
                sb.Append(" ELSE tLocal.DataEmissaoAlvara ");
                sb.Append(" END  END AS DataEmissaoAlvara, ");


                sb.Append(" CASE WHEN LEN(a.DataValidadeAlvara) > 0 ");
                sb.Append(" THEN a.DataValidadeAlvara   ");
                sb.Append(" ELSE CASE WHEN LEN (e.DataValidadeAlvara) > 0 ");
                sb.Append(" THEN e.DataValidadeAlvara ");
                sb.Append(" ELSE tLocal.DataValidadeAlvara ");
                sb.Append(" END  END AS DataValidadeAlvara, ");

                sb.Append(" CASE WHEN LEN(a.DataEmissaoAvcb) > 0 ");
                sb.Append(" THEN a.DataEmissaoAvcb   ");
                sb.Append(" ELSE CASE WHEN LEN (e.DataEmissaoAvcb) > 0 ");
                sb.Append(" THEN e.DataEmissaoAvcb ");
                sb.Append(" ELSE tLocal.DataEmissaoAvcb ");
                sb.Append(" END  END AS DataEmissaoAvcb, ");

                sb.Append(" CASE WHEN LEN(a.DataValidadeAvcb) > 0 ");
                sb.Append(" THEN a.DataValidadeAvcb   ");
                sb.Append(" ELSE CASE WHEN LEN (e.DataValidadeAvcb) > 0 ");
                sb.Append(" THEN e.DataValidadeAvcb ");
                sb.Append(" ELSE tLocal.DataValidadeAvcb ");
                sb.Append(" END  END AS DataValidadeAvcb ");

				sb.Append("FROM tIngresso i (NOLOCK) ");
				sb.Append("LEFT OUTER JOIN tPreco p (NOLOCK) ON i.ApresentacaoSetorID=p.ApresentacaoSetorID AND i.PrecoID=p.ID ");
				sb.Append("INNER JOIN tApresentacao a (NOLOCK) ON i.ApresentacaoID=a.ID ");
				sb.Append("INNER JOIN tSetor s (NOLOCK) ON i.SetorID=s.ID ");
				sb.Append("INNER JOIN tEvento e (NOLOCK) ON i.EventoID=e.ID ");
				sb.Append("INNER JOIN tUsuario u (NOLOCK) ON i.UsuarioID=u.ID ");
				sb.Append("INNER JOIN tLocal (NOLOCK) ON tLocal.ID = e.LocalID ");
				sb.Append("INNER JOIN tPais (NOLOCK) ON tLocal.PaisID = tPais.ID ");
				sb.Append("LEFT JOIN tLoja l (NOLOCK) ON i.LojaID = l.ID ");
				sb.Append("LEFT JOIN tCortesia c (NOLOCK) ON i.CortesiaID=c.ID ");
				sb.AppendFormat("LEFT JOIN tEventoLoja el (NOLOCK) ON e.ID = el.EventoID AND el.LojaID = {0} ", lojaID);
				sb.Append("WHERE i.ID in (" + Utilitario.ArrayToString(ingressosids) + ") ORDER BY i.Codigo");
				string sql = sb.ToString();

				database.Consulta(sql);

				Servidor servidor = new Servidor();

				while (database.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["ID"] = database.LerInt("ID");
					linha["IngressoID"] = database.LerInt("ID");
					linha["Usuario"] = database.LerString("Usuario");
					linha["Evento"] = database.LerString("Evento");
					linha["EventoID"] = database.LerInt("EventoID");
					linha["ImpressaoCodigoBarra"] = database.LerBoolean("ImpressaoCodigoBarra");
					linha["Horario"] = database.LerDateTime("Horario");
					linha["HorarioString"] = database.LerStringFormatoDataHora("Horario");
					linha["DataVendaString"] = servidor.Agora.ToString(Utilitario.FormatoDataHora);
					linha["ApresentacaoID"] = database.LerInt("ApresentacaoID");
					linha["ApresentacaoImpressao"] = database.LerString("ApresentacaoImpressao");
					linha["PrecoImpressao"] = database.LerString("PrecoImpressao");
					linha["ApresentacaoSetorID"] = database.LerInt("ApresentacaoSetorID");
					linha["Setor"] = database.LerString("Setor");
					linha["Acesso"] = database.LerString("Acesso");
					linha["ImprimirCarimbo"] = database.LerString("ImprimirCarimbo");
					linha["CarimboTexto1"] = database.LerString("CarimboTexto1");
					linha["CarimboTexto2"] = database.LerString("CarimboTexto2");
					linha["TipoSetor"] = database.LerString("TipoSetor");
					linha["Produto"] = database.LerBoolean("Produto");
					linha["Cortesia"] = database.LerString("Cortesia");
					linha["CortesiaID"] = database.LerInt("CortesiaID");
					linha["BloqueioID"] = database.LerInt("BloqueioID");
					linha["Preco"] = database.LerString("Preco");
					linha["PrecoID"] = database.LerInt("PrecoID");
					linha["Loja"] = database.LerString("Loja");
					linha["Valor"] = database.LerDecimal("Valor");
					linha["Codigo"] = database.LerString("Codigo");
					linha["CodigoBarra"] = database.LerString("CodigoBarra");
					linha["CodigoBarraCliente"] = database.LerString("CodigoBarraCliente");
					linha["LocalID"] = database.LerInt("LocalID");
					linha["LocalNome"] = database.LerString("LocalNome");
					linha["LocalEndereco"] = database.LerString("Logradouro") + ((database.LerInt("Numero") > 0) ? ", " + database.LerInt("Numero").ToString() : "s/n");
					linha["LocalCidade"] = database.LerString("LocalCidade");
					linha["LocalEstado"] = database.LerString("LocalEstado");
					linha["LocalPais"] = database.LerString("Pais");
					linha["LocalCep"] = database.LerString("LocalCep");
					linha["ObrigaCadastroCliente"] = database.LerString("ObrigaCadastroCliente");
					linha["CodigoSequencial"] = database.LerInt("CodigoSequencial");
					linha["ObrigatoriedadeID"] = database.LerInt("ObrigatoriedadeID");
					linha["TipoImpressao"] = database.LerString("TipoImpressao");
					linha["TipoCodigoBarra"] = database.LerString("TipoCodigoBarra");
					linha["GerenciamentoIngressos"] = "";
					linha["GerenciamentoIngressosID"] = 0;

                    linha["Alvara"] = database.LerString("Alvara");
                    linha["FonteImposto"] = database.LerString("FonteImposto");
                    linha["AVCB"] = database.LerString("AVCB");
                    linha["PorcentagemImposto"] = database.LerDecimal("PorcentagemImposto");


                    linha["Lotacao"] = database.LerInt("Lotacao");
                    linha["DataEmissaoAlvara"] = database.LerStringFormatoData("DataEmissaoAlvara");
                    linha["DataValidadeAlvara"] = database.LerStringFormatoData("DataValidadeAlvara");
                    linha["DataEmissaoAvcb"] = database.LerStringFormatoData("DataEmissaoAvcb");
                    linha["DataValidadeAvcb"] = database.LerStringFormatoData("DataValidadeAvcb");

					tabela.Rows.Add(linha);
				}

				DateTime fim = DateTime.Now;
				TimeSpan dif = fim - inicio;


				this.info = null; //associa ao info do ingresso

				return tabela;

			}
			catch
			{
				throw;
			}
			finally
			{
				database.FecharConsulta();
			}
		}

		/// <summary>
		/// Obtem informações (evento,apresentacao,setor...) desse ingresso reservado
		/// </summary>
		/// <returns></returns>
		public override DataRow InfoReservado()
		{
			//fabio

			try
			{

				DataTable tabelaAux = EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

				DataRow linha = tabelaAux.NewRow();
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,s.Nome AS Setor,s.Acesso,i.CodigoSequencial, ");
				sb.Append("s.Produto,c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao, ");
				sb.Append("p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,i.ApresentacaoSetorID,l.Nome AS Loja, tLocal.Nome AS LocalNome, ");
				sb.Append("tLocal.Logradouro, tLocal.Numero ");
				sb.Append("FROM tIngresso i (NOLOCK) ");
				sb.Append("INNER JOIN tPreco p (NOLOCK) ON i.ApresentacaoSetorID=p.ApresentacaoSetorID AND i.PrecoID=p.ID ");
				sb.Append("INNER JOIN tApresentacao a (NOLOCK) ON i.ApresentacaoID=a.ID ");
				sb.Append("INNER JOIN tSetor s (NOLOCK) ON i.SetorID=s.ID ");
				sb.Append("INNER JOIN tEvento e (NOLOCK) ON i.EventoID=e.ID ");
				sb.Append("INNER JOIN tLocal (NOLOCK) ON tLocal.ID = e.LocalID ");
				sb.Append("INNER JOIN tUsuario u (NOLOCK) ON i.UsuarioID=u.ID ");
				sb.Append("LEFT JOIN tLoja l (NOLOCK) ON i.LojaID=l.ID ");
				sb.Append("LEFT JOIN tCortesia c (NOLOCK) ON i.CortesiaID=c.ID ");
				sb.Append("WHERE i.ID=" + this.Control.ID);
				string sql = sb.ToString();
				//string sql = "SELECT u.Login AS Usuario,e.ImpressaoCodigoBarra,e.Nome AS Evento,i.EventoID,a.Horario,i.ApresentacaoID,s.Nome AS Setor,s.Produto,c.Nome AS Cortesia,p.Nome AS Preco,a.Impressao AS ApresentacaoImpressao,p.Impressao AS PrecoImpressao,p.Valor,i.Codigo,i.CodigoBarra,i.CodigoBarraCliente,i.ApresentacaoSetorID,l.Nome AS Loja, tLocal.Nome AS LocalNome, tLocal.Endereco AS LocalEndereco" +
				//    "FROM tUsuario as u (NOLOCK) ,tIngresso as i (NOLOCK), tEvento as e (NOLOCK), tApresentacao as a (NOLOCK),tSetor as s (NOLOCK), tPreco as p (NOLOCK), tCortesia as c (NOLOCK), tLoja AS l (NOLOCK), tLocal  (NOLOCK) " +
				//    "WHERE tLocal.ID = e.LocalID AND i.LojaID*=l.ID AND i.ApresentacaoSetorID=p.ApresentacaoSetorID AND i.ApresentacaoID=a.ID AND i.SetorID=s.ID AND i.EventoID=e.ID AND i.PrecoID=p.ID AND i.CortesiaID*=c.ID AND i.UsuarioID=u.ID AND i.ID=" + this.Control.ID;

				bd.Consulta(sql);

				Servidor servidor = new Servidor();

				if (bd.Consulta().Read())
				{
					linha["ID"] = this.Control.ID;
					linha["Usuario"] = bd.LerString("Usuario");
					linha["Evento"] = bd.LerString("Evento");
					linha["EventoID"] = bd.LerInt("EventoID");
					linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
					linha["Horario"] = bd.LerDateTime("Horario");
					linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
					linha["DataVendaString"] = servidor.Agora.ToString(Utilitario.FormatoDataHora);
					linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
					linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
					linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
					linha["Setor"] = bd.LerString("Setor");
					linha["Acesso"] = bd.LerString("Acesso");
					linha["Produto"] = bd.LerBoolean("Produto");
					linha["Cortesia"] = bd.LerString("Cortesia");
					linha["Preco"] = bd.LerString("Preco");
					linha["Loja"] = bd.LerString("Loja");
					linha["Valor"] = bd.LerDecimal("Valor");
					linha["Codigo"] = bd.LerString("Codigo");
					linha["CodigoBarra"] = bd.LerString("CodigoBarra");
					linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
					linha["LocalNome"] = bd.LerString("LocalNome");
					linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
					linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
				}
				bd.Fechar();

				tabelaAux.Rows.Add(linha);

				this.info = linha; //associa ao info do ingresso

				return linha;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

		#region ReservaPacote

		/// <summary>		
		/// Cancela um pacote de ingressos reservados ou em reserva (resultado eh 'disponivel')
		/// </summary>
		public override void CancelarPacote()
		{

			try
			{

				string sql = "UPDATE tIngresso SET ClienteID=0, PrecoID=0, CortesiaID=0, BloqueioID=0, LojaID=0, PacoteID=0 , PacoteGrupo = '', " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status='" + RESERVANDO + "' AND UsuarioID=" + this.UsuarioID.Valor;

				//nao interessa saber se x linhas foram afetadas. elas podem nao ter sido.
				bd.Executar(sql);
				bd.Fechar();

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Inicia a reserva de um pacote de ingressos (resultado eh 'em reserva')
		/// </summary>
		/// <returns></returns>
		public override bool EmpacotarReserva(int[] ingressosids)
		{

			try
			{


				string ids = Utilitario.ArrayToString(ingressosids);

				string sql = "UPDATE tIngresso SET UsuarioID=@uid, PrecoID=@pid, " +
					"Status='" + RESERVANDO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ID in (" + ids + ") AND Status='" + DISPONIVEL + "'";

				sql = sql.Replace("@uid", this.UsuarioID.ValorBD);
				sql = sql.Replace("@pid", this.PrecoID.ValorBD);

				int qtdeLinhasAfetadas = bd.Executar(sql);
				bd.Fechar();

				bool result = (qtdeLinhasAfetadas == ingressosids.Length);

				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Finaliza a reserva de um pacote de ingressos (resultado eh 'reservado')
		/// </summary>
		/// <returns></returns>
		public override bool ReservarPacote(int[] ingressosids)
		{

			return ReservarPacote(ingressosids, false);

		}

		/// <summary>		
		/// Finaliza a reserva de um pacote de ingressos (resultado eh 'reservado')
		/// </summary>
		/// <returns></returns>
		public bool ReservarPacote(int[] ingressosids, bool preReserva)
		{

			try
			{

				int qtdeLinhasAfetadas = 0;

				foreach (int id in ingressosids)
				{

					this.Control.ID = id;
					CodigoBarra oCodigoBarra = new CodigoBarra();
					//this.CodigoBarra.Valor = oCodigoBarra.GerarCodigoBarra(this.PrecoID.Valor, this.EventoID.Valor);

					string sql = "UPDATE tIngresso SET CodigoBarra='" + this.CodigoBarra.ValorBD + "', Status='" + ((preReserva) ? RESERVADO_PRE_RESERVA : RESERVADO) + "' " +
						"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ID=" + id + " AND Status='" + RESERVANDO + "' AND UsuarioID=" + this.UsuarioID.Valor;

					qtdeLinhasAfetadas += bd.Executar(sql);

				}

				bd.Fechar();

				bool result = (qtdeLinhasAfetadas == ingressosids.Length);
				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		#endregion

		#region Reserva

		/// <summary>		
		/// Cancelar reserva desse ingresso
		/// </summary>
		/// <returns></returns>
		public override bool CancelarReserva(int usuarioid)
		{

			try
			{

				string sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND UsuarioID=" + usuarioid + " AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);

				bool result = Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, BloqueioID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID<>0 AND UsuarioID=" + usuarioid + " AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND ID=" + this.Control.ID;

				x = bd.Executar(sql);

				result |= Convert.ToBoolean(x);

				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Cancelar reserva desse ingresso em lugar marcado em uma determinada ApresentacaoSetor
		/// </summary>
		/// <returns></returns>
		public bool CancelarReserva(int usuarioid, int lugarID, int apresentacaoSetorID)
		{

			try
			{
				string sql = string.Empty;
				int x;
				bool result = true;

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND UsuarioID=" + usuarioid + " AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND Status='" + RESERVADO_PRE_RESERVA + "' AND LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, BloqueioID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID<>0 AND UsuarioID=" + usuarioid + " AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, BloqueioID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID<>0 AND Status='" + RESERVADO_PRE_RESERVA + "' AND LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Cancelar reserva desse ingresso
		/// </summary>
		/// <returns></returns>
		public override bool CancelarReserva()
		{

			try
			{

				string sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);

				bool result = Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID<>0 AND (Status='" + RESERVADO + "' OR Status='" + RESERVADO_PRE_RESERVA + "') AND ID=" + this.Control.ID;

				x = bd.Executar(sql);
				bd.Fechar();

				result |= Convert.ToBoolean(x);

				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Cancelar reserva de todos os ingressos do usuario
		/// </summary>
		/// <returns></returns>

		public override bool CancelarReservas(int usuarioid)
		{

			try
			{
				string sql = string.Empty;
				int x;
				bool result = true;

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND Status='" + RESERVADO + "' AND UsuarioID=" + usuarioid;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID=0 AND Status='" + RESERVADO_PRE_RESERVA + "'";
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID > 0 AND (Status='" + RESERVADO + "' ) AND UsuarioID=" + usuarioid;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND BloqueioID > 0 AND Status='" + RESERVADO_PRE_RESERVA + "'";
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				bd.Fechar();

				return result;

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
		/// <summary>		
		/// Cancelar reserva
		/// </summary>
		/// <returns></returns>

		[System.Runtime.Remoting.Messaging.OneWay]
		public override bool CancelarReservas(int[] ingressosids)
		{
			try
			{

				string ids = Utilitario.ArrayToString(ingressosids);
				bool result = true;
				string sql = string.Empty;
				int x;

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='', PacoteID = 0, PacoteGrupo= '' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + Ingresso.RESERVADO + "' AND  BloqueioID=0 AND ID in (" + ids + ") AND UsuarioID=" + this.UsuarioID.Valor;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + DISPONIVEL + "', CodigoBarra='', PacoteID = 0, PacoteGrupo= '' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + Ingresso.RESERVADO_PRE_RESERVA + "' AND  BloqueioID=0 AND ID in (" + ids + ")";
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "', CodigoBarra='', PacoteID = 0, PacoteGrupo= ''  " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + Ingresso.RESERVADO + "' AND BloqueioID<>0 AND ID in (" + ids + ") AND UsuarioID=" + this.UsuarioID.Valor;
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, " +
					"Status='" + BLOQUEADO + "', CodigoBarra='', PacoteID = 0, PacoteGrupo= ''  " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + Ingresso.RESERVADO_PRE_RESERVA + "' AND BloqueioID<>0 AND ID in (" + ids + ")";
				x = bd.Executar(sql);
				result |= Convert.ToBoolean(x);

				bd.Fechar();

				return result;

			}
			catch
			{
				throw;
			}

		}

		/// <summary>		
		/// Cancelar reserva do ingresso passado como parametro
		/// </summary>
		/// <returns></returns>
		public bool CancelarReservasInternet(int ingressoid, int usuarioID)
		{
			try
			{
				if (usuarioID == 0)
					usuarioID = INTERNET_USUARIO_ID;

				string sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, PacoteID=0, " +
					"PacoteGrupo = '', Status='" + Ingresso.DISPONIVEL + "', SerieID = 0 " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND UsuarioID = " + usuarioID + " AND Status = '" + Ingresso.RESERVADO + "' AND BloqueioID = 0 AND ID = " + ingressoid;
				int x = bd.Executar(sql);

				bool result = Convert.ToBoolean(x);

				string sql2 = "UPDATE tIngresso SET  ClienteID=0, PrecoID=0, CortesiaID=0, PacoteID=0, " +
					"PacoteGrupo = '', Status='" + (char)Ingresso.StatusIngresso.PRE_RESERVA + "', SerieID = 0 " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND UsuarioID = " + usuarioID + " AND Status = '" + (char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE + "' AND BloqueioID = 0 AND ID = " + ingressoid;

				x = bd.Executar(sql2);
				bd.Fechar();

				result |= Convert.ToBoolean(x);

				return result;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Cancelar reserva dos de todos ingressos da sessao do cliente 
		/// Cancela inclusive as Pré-Reservas
		/// </summary>
		/// <returns></returns>
		public bool CancelarTodasReservasInternet(int clienteID, string sessionID)
		{
			try
			{
				//Volta o Status para Disponível ('D')
				string sql = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, PacoteID = 0, PacoteGrupo = '', " +
					"Status='" + ((char)Ingresso.StatusIngresso.DISPONIVEL).ToString() + "', SerieID = 0 " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' AND BloqueioID=0 AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'";
				//Volta o Status para Pré-Reserva ('D')
				string sqlPreReserva = "UPDATE tIngresso SET ClienteID=0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, PacoteID = 0, PacoteGrupo = '', " +
					"Status='" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "', SerieID = 0 " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status = '" + ((char)Ingresso.StatusIngresso.PRE_RESERVA).ToString() + "' AND BloqueioID=0 AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'";

				int retorno = 0;
				retorno = bd.Executar(sql);
				retorno += bd.Executar(sqlPreReserva);
				return retorno > 0;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// Criado por: Caio Maganha Rosa
		/// Utilização: Transfere todos os ingressos reservados pela Internet
		/// </summary>
		/// <param name="clienteID"></param>
		/// <param name="sessionID"></param>
		/// <returns></returns>
		public bool TransferirTodasReservasInternet(int clienteID, string sessionID)
		{
			try
			{
				int ok = 0;
				StringBuilder stbSql = new StringBuilder();

				System.Data.SqlClient.SqlParameter[] Parametros = new System.Data.SqlClient.SqlParameter[2];

				stbSql.Append("UPDATE tIngresso ");
				stbSql.Append("SET ClienteID = " + clienteID + " ");
				stbSql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID = 0 ");
				stbSql.Append("AND SessionID = '" + sessionID + "' AND Status = 'R'");

				ok = bd.Executar(stbSql.ToString());

				if (ok != 0)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


        public bool TransferirTodasReservasMobile(int clienteID, string sessionID)
        {
            try
            {
                int ok = 0;
                StringBuilder stbSql = new StringBuilder();

                System.Data.SqlClient.SqlParameter[] Parametros = new System.Data.SqlClient.SqlParameter[2];

                stbSql.Append("UPDATE tIngresso ");
                stbSql.Append("SET ClienteID = " + clienteID + " ");
                stbSql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID = 0 ");
                stbSql.Append("AND SessionID = '" + sessionID + "' AND Status = 'R'");

                ok = bd.Executar(stbSql.ToString());

                if (ok != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




		/// <summary>
		/// String para reservar ingresso
		/// </summary>
		/// <returns></returns>	
		public string StringReservar()
		{

			try
			{

				StringBuilder sql = new StringBuilder();

				sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008', PacoteID=@010, PacoteGrupo='@011', LojaID = @012, ClienteID = @013, TimeStampReserva='@014', SessionID='@015', AssinaturaClienteID = @016, SerieID = @017 ");
				sql.Append("WHERE (AssinaturaClienteID = 0 OR AssinaturaClienteID IS NULL) AND ID=" + this.Control.ID + " AND (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "')");
				sql.Replace("@002", this.PrecoID.ValorBD);
				sql.Replace("@004", this.UsuarioID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@008", Ingresso.RESERVADO);
				sql.Replace("@010", this.PacoteID.ValorBD);
				sql.Replace("@011", this.PacoteGrupo.ValorBD);
				sql.Replace("@012", this.LojaID.ValorBD);
				sql.Replace("@013", this.ClienteID.ValorBD);
				sql.Replace("@014", this.TimeStampReserva.ValorBD);
				sql.Replace("@015", this.SessionID.ValorBD);
				sql.Replace("@016", this.AssinaturaClienteID.ValorBD);
				sql.Replace("@017", this.SerieID.ValorBD);

				return sql.ToString();

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public string StringReservarInternet()
		{

			try
			{

				StringBuilder sql = new StringBuilder();

				sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008', PacoteID=@010, ClienteID=@011, SerieID = @012 ");
				sql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND  ID=" + this.Control.ID + " AND (Status='" + Ingresso.DISPONIVEL + "' )");
				sql.Replace("@002", this.PrecoID.ValorBD);
				sql.Replace("@004", this.UsuarioID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@008", RESERVADO);
				sql.Replace("@010", this.PacoteID.ValorBD);
				sql.Replace("@011", this.ClienteID.ValorBD);
				sql.Replace("@012", this.SerieID.ValorBD);

				return sql.ToString();

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Reserva esse ingresso individual
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public override bool Reservar()
		{
			return Reservar(bd, false);
		}


		/// <summary>		
		/// Reserva esse ingresso individual
		/// </summary>
		/// <returns></returns>
		public bool Reservar(CTLib.BD database, bool preReservar)
		{
			StringBuilder sql = new StringBuilder();
			if (this.PacoteGrupo.ValorBD == "")//se nao for pacote nao deve atualizar o campo PacoteGrupo
				sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008', LojaID = @010, ClienteID = @011, TimeStampReserva='@012', SessionID='@013', PacoteID='@014', SerieID = @016 ");
			else
				sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008',  LojaID = @010, ClienteID = @011, TimeStampReserva='@012', SessionID='@013', PacoteID='@014', PacoteGrupo=@015, SerieID = @016 ");

			sql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ID=" + this.Control.ID + " AND (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "')");
			sql.Replace("@002", this.PrecoID.ValorBD);
			sql.Replace("@004", this.UsuarioID.ValorBD);
			sql.Replace("@005", this.CortesiaID.ValorBD);
			sql.Replace("@008", ((preReservar) ? RESERVADO_PRE_RESERVA : RESERVADO));
			sql.Replace("@010", this.LojaID.ValorBD);
			sql.Replace("@011", this.ClienteID.ValorBD);
			sql.Replace("@012", this.TimeStampReserva.ValorBD);
			sql.Replace("@013", this.SessionID.ValorBD);
			sql.Replace("@014", this.PacoteID.ValorBD);

			if (this.PacoteGrupo.ValorBD != "")//se nao for pacote nao deve atualizar o campo PacoteGrupo
				sql.Replace("@015", this.PacoteGrupo.ValorBD);

			sql.Replace("@016", this.SerieID.ValorBD);

			int x = database.Executar(sql.ToString());
			return (x == 1);
		}

		public bool ReservarInternet(CTLib.BD database, bool preReservar)
		{

			try
			{

				StringBuilder sql = new StringBuilder();
				if (this.PacoteGrupo.ValorBD == "")//se nao for pacote nao deve atualizar o campo PacoteGrupo
					sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008', LojaID = @010, ClienteID = @011, TimeStampReserva='@012', SessionID='@013', PacoteID='@014', SerieID = @016, CompraGUID = '@017', GerenciamentoIngressosID = @018 ");
				else
					sql.Append("UPDATE tIngresso SET PrecoID=@002, UsuarioID=@004, CortesiaID=@005, Status='@008', LojaID = @010, ClienteID = @011, TimeStampReserva='@012', SessionID='@013', PacoteID='@014', PacoteGrupo=@015, SerieID = @016, CompraGUID = '@017' ");

				if (!preReservar)
					sql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ID=" + this.Control.ID + " AND (Status='" + ((char)Ingresso.StatusIngresso.DISPONIVEL).ToString() + "' OR Status='" + ((char)Ingresso.StatusIngresso.BLOQUEADO).ToString() + "')");
				else//se for pré-reserva o status do where é somente 'N' e deve-se verificar se o ingresso pertence ao cliente.
					sql.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ID=" + this.Control.ID + " AND (Status='" + ((char)Ingresso.StatusIngresso.PRE_RESERVA).ToString() + "') AND ClienteID = " + this.ClienteID.ValorBD + "");

				sql.Replace("@002", this.PrecoID.ValorBD);
				sql.Replace("@004", this.UsuarioID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@008", ((preReservar) ? ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() : ((char)Ingresso.StatusIngresso.RESERVADO).ToString()));
				sql.Replace("@010", this.LojaID.ValorBD);
				sql.Replace("@011", this.ClienteID.ValorBD);
				sql.Replace("@012", this.TimeStampReserva.ValorBD);
				sql.Replace("@013", this.SessionID.ValorBD);
				sql.Replace("@014", this.PacoteID.ValorBD);

				if (this.PacoteGrupo.ValorBD != "")//se nao for pacote nao deve atualizar o campo PacoteGrupo
					sql.Replace("@015", this.PacoteGrupo.ValorBD);
				else
					sql.Replace("@018", this.GerenciamentoIngressosID.ValorBD);

				sql.Replace("@016", this.SerieID.ValorBD);
				sql.Replace("@017", this.CompraGUID.ValorBD);

				int x = database.Executar(sql.ToString());
				return (x == 1);

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public void ReservarPista(int apresentacaoSetorID, int qtd, int bloqueioID, int usuarioID,
								int lojaID, int precoID, int cortesiaID, CTLib.BD database,
								bool implementaTransaction, int eventoID, bool preReservar, Enumerators.TipoCodigoBarra tipoCodigoBarra)
		{
			try
			{
				/// 1) Buscar os ingressos conforme Bloqueio e qtd solicitada
				/// 2) Efetuar a reserva no que for possível.
				/// 

				//System.Collections.ArrayList ids = new System.Collections.ArrayList(qtd);

				string sql = "SELECT TOP " + qtd + " ID, CodigoBarra FROM tIngresso (NOLOCK) WHERE ApresentacaoSetorID=" + apresentacaoSetorID;

				// Verifica se é bloqueio.
				if (bloqueioID == 0)
					sql += " AND Status = '" + Ingresso.DISPONIVEL + "' ";
				else
					sql += " AND Status = '" + Ingresso.BLOQUEADO + "' AND BloqueioID = " + bloqueioID;

				sql += " ORDER BY Codigo"; // Somente ordena.

				// Parâmetros em comum devem ficar fora do Loop (performance)
				this.UsuarioID.Valor = usuarioID;
				this.LojaID.Valor = lojaID;
				this.PrecoID.Valor = precoID;
				this.CortesiaID.Valor = cortesiaID;

				//database.IniciarTransacao();

				if (implementaTransaction)
					bd.IniciarTransacao(); // Apenas para inserir ingresso.

				//Busca e começa a inserir. Efetua em linha para não perder tempo!
				while (database.Consulta(sql).Read())
				{
					this.Control.ID = database.LerInt("ID");
					if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
						this.CodigoBarra.Valor = database.LerString("CodigoBarra");

					if (this.Reservar(bd, false))
						OnCreate(this.Control.ID);
					else
					{
						if (implementaTransaction)
							bd.DesfazerTransacao();
					}
				}
				if (implementaTransaction)
					bd.FinalizarTransacao();
				//return ids;
			}
			catch
			{
				if (implementaTransaction)
					bd.DesfazerTransacao();
				throw;
			}
			finally
			{
				database.FecharConsulta();
				bd.Fechar();
				bd.Cnn.Dispose();
			}
		}
		#endregion

		#region PreImprimir
		/// <summary>		
		/// Mudar o status de 'reservado' para 'pre-impresso'
		/// </summary>
		public override bool PreImprimir(int caixaid, int lojaid, int canalid, int empresaid)
		{

			try
			{

				string sql = "UPDATE tIngresso SET " +
					"Status='" + PREIMPRESSO + "' " +
					"WHERE Status='" + RESERVADO + "' AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);
				bd.Fechar();

				bool ok = Convert.ToBoolean(x);

				if (ok)
				{
					//inserir na Log
					ingressoLog.Ingresso(this);
					ingressoLog.Acao.Valor = IngressoLog.EMISSAO_PREIMPRESSO;
					ingressoLog.CaixaID.Valor = caixaid;
					ingressoLog.LojaID.Valor = lojaid;
					ingressoLog.CanalID.Valor = canalid;
					ingressoLog.EmpresaID.Valor = empresaid;
					ingressoLog.Inserir();
				}

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>		
		/// Nao muda o status, apenas registra a trasnferencia
		/// </summary>
		public override bool TransferirPreImpresso(int caixaid, int lojaid, int canalid, int empresaid)
		{

			try
			{

				string sql = "UPDATE tIngresso SET " +
					"LojaID=" + lojaid + " " +
					"WHERE Status='" + PREIMPRESSO + "' AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);
				bd.Fechar();

				bool ok = Convert.ToBoolean(x);

				if (ok)
				{
					//inserir na Log
					ingressoLog.Ingresso(this);
					ingressoLog.Acao.Valor = IngressoLog.TRANSFERENCIA_PREIMPRESSO;
					ingressoLog.CaixaID.Valor = caixaid;
					ingressoLog.LojaID.Valor = lojaid;
					ingressoLog.CanalID.Valor = canalid;
					ingressoLog.EmpresaID.Valor = empresaid;
					ingressoLog.Inserir();
				}

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

		#region ReImprimir
		/// <summary>		
		/// Mudar o status de 'impresso' para 'reimpresso' e registrar na Log
		/// </summary>
		public override bool ReImprimir(string motivo)
		{
			try
			{
				bool ok;
				if (this.Status.Valor == IMPRESSO)
				{
					ok = this.Atualizar();
					if (ok)
					{
						//inserir na Log
						ingressoLog.Ingresso(this);
						//*********************************** ERRADO
						ingressoLog.VendaBilheteriaItemID.Valor = VendaBilheteriaItemID();
						//******************************************
						ingressoLog.Acao.Valor = IngressoLog.REIMPRIMIR;
						ingressoLog.Obs.Valor = motivo;
						ingressoLog.Inserir();
					}
				}
				else
				{
					ok = false;
				}
				return ok;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region Bloquear

		/// <summary>		
		/// Bloquear ingressos de acordo com as informações do objeto atualmente carregado
		/// </summary>
		/// <returns></returns>
		public override bool Bloquear(int bloqueioid, int usuarioid, int empresaid)
		{

			try
			{

				string sql = "UPDATE tIngresso SET " +
					"BloqueioID=" + bloqueioid + ", " +
					"Status='" + BLOQUEADO + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND  (Status='" + DISPONIVEL + "' OR Status='" + BLOQUEADO + "') AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);
				bd.Fechar();

				bool ok = Convert.ToBoolean(x);

				if (ok)
				{
					//inserir na Log
					this.BloqueioID.Valor = bloqueioid;
					this.Status.Valor = BLOQUEADO;
					ingressoLog.Ingresso(this);
					ingressoLog.UsuarioID.Valor = usuarioid;
					ingressoLog.EmpresaID.Valor = empresaid;
					ingressoLog.Acao.Valor = IngressoLog.BLOQUEAR;
					ingressoLog.Inserir();
				}

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

		#region Desbloquear

		/// <summary>		
		/// Bloquear ingressos de acordo com as informações do objeto atualmente carregado
		/// </summary>
		/// <returns></returns>
		public override bool Desbloquear(int usuarioid, int empresaid)
		{

			try
			{

				string sql = "UPDATE tIngresso SET " +
					"BloqueioID=0, " +
					"Status='" + DISPONIVEL + "' " +
					"WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND (Status='" + DISPONIVEL + "' OR Status='" + BLOQUEADO + "') AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);
				bd.Fechar();

				bool ok = Convert.ToBoolean(x);

				if (ok)
				{
					//inserir na Log

					ingressoLog.Ingresso(this);
					ingressoLog.UsuarioID.Valor = usuarioid;
					ingressoLog.EmpresaID.Valor = empresaid;
					ingressoLog.Acao.Valor = IngressoLog.DESBLOQUEAR;
					ingressoLog.Inserir();
				}

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

		#region ChecarStatusCancelar
		/// <summary>		
		/// Checa status do ingresso de acordo com Cancelar
		/// Ã‰ necessario checar antes de inserir registro de Venda e item
		/// </summary>
		public override bool ChecarStatusCancelar()
		{
			bool statusCancelar = false;
			try
			{
				if (this.Status.Valor == VENDIDO || this.Status.Valor == IMPRESSO || this.Status.Valor == ENTREGUE)
				{
					statusCancelar = true;
				}
				else
				{
					statusCancelar = false;
				}
			}
			catch (Exception erro)
			{
				throw erro;
			}
			return statusCancelar;
		}
		#endregion

		#region Entregar

		public override bool Entregar(int usuarioID, int caixaID, int canalID, int lojaID, int empresaID, string obs)
		{

			try
			{

				string sql = "UPDATE tIngresso SET " +
					"Status='" + ENTREGUE + "' " +
					"WHERE Status='" + IMPRESSO + "' AND ID=" + this.Control.ID;

				int x = bd.Executar(sql);
				bd.Fechar();

				bool ok = Convert.ToBoolean(x);

				if (ok)
				{
					//inserir na Log
					ingressoLog.Ingresso(this.Control.ID);
					ingressoLog.UsuarioID.Valor = usuarioID;
					ingressoLog.CaixaID.Valor = caixaID;
					ingressoLog.CanalID.Valor = canalID;
					ingressoLog.LojaID.Valor = lojaID;
					ingressoLog.EmpresaID.Valor = empresaID;
					ingressoLog.Acao.Valor = IngressoLog.ENTREGAR;
					ingressoLog.Obs.Valor = obs;
					ingressoLog.Inserir();
				}

				return ok;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

		public override int VendaBilheteriaItemID()
		{
			try
			{
				int vendaBilheteriaItemID = -1;
				// Obtendo dados através de SQL
				string sql =
					"SELECT     tIngressoLog.VendaBilheteriaItemID, tIngressoLog.IngressoID, tIngressoLog.ID AS tIngressoLogID " +
					"FROM       tIngresso INNER JOIN " +
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
					"tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID " +
					"WHERE      (tIngresso.ID = '" + this.Control.ID + "')" +
					"ORDER BY tIngressoLog.ID DESC ";
				bd.Consulta(sql);
				// Obtendo ID do Ingresso
				if (bd.Consulta().Read())
					vendaBilheteriaItemID = bd.LerInt("VendaBilheteriaItemID");
				bd.Fechar();
				return vendaBilheteriaItemID;
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try			
		} // fim de VendaBilheteriaItemID

		/// <summary>
		/// Verifica se o ingresso já foi vendido beseado nos logs
		/// </summary>
		/// <returns></returns>
		public override bool VendidoUmaVez()
		{
			bool vendido = true;
			try
			{
				// Obter os Logs deste ingresso cuja Acao=V
				// Fazendo consulta SQL para obter os dados
				BD bd = new BD();
				string sql =
					"SELECT        tIngressoLog.Acao, tIngressoLog.IngressoID " +
					"FROM          tIngresso INNER JOIN " +
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID " +
					"WHERE        (tIngressoLog.IngressoID = " + this.Control.ID + ") AND (tIngressoLog.Acao = N'V')";
				bd.Consulta(sql);
				if (bd.Consulta().Read())
				{
					vendido = true;
				}
				else
				{
					vendido = false;
				}
				// Fechar BD
				bd.Fechar();
				// Se não retornar nenhum registro retorne false
				return vendido;
			}
			catch (Exception ex)
			{
				throw ex;
			} // fim de try
		} // fim de método Vendido

		/// <summary>
		/// Obter total de ingressos por Canal e Evento
		/// </summary>
		/// <returns></returns>
		public override int TotalPorCanalEvento(int canalID, int eventoID)
		{
			int total = 0;
			string condicao = "";
			try
			{
				if (canalID > 0)
				{
					condicao = "HAVING      (tEvento.ID = " + eventoID + ") AND (tCanal.ID = " + canalID + ")";
				}
				else
				{
					// canalID = 0 significa Todos Canais
					condicao = "HAVING      (tEvento.ID = " + eventoID + ") ";
				}
				// Fazendo consulta SQL para obter os dados
				BD bd = new BD();
				string sql =
					"SELECT     COUNT(tIngresso.ID) AS Total, tCanal.ID AS Canal, tEvento.ID AS EventoID " +
					"FROM         tCanal INNER JOIN " +
					"tLoja ON tCanal.ID = tLoja.CanalID INNER JOIN " +
					"tCaixa ON tLoja.ID = tCaixa.LojaID INNER JOIN " +
					"tVendaBilheteria ON tCaixa.ID = tVendaBilheteria.CaixaID INNER JOIN " +
					"tVendaBilheteriaItem ON tVendaBilheteria.ID = tVendaBilheteriaItem.VendaBilheteriaID INNER JOIN " +
					"tIngresso ON tVendaBilheteriaItem.ID = tIngresso.VendaBilheteriaItemID INNER JOIN " +
					"tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
					"tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
					"tEvento ON tApresentacao.EventoID = tEvento.ID " +
					"GROUP BY tCanal.ID, tEvento.ID " +
					condicao;
				bd.Consulta(sql);
				if (bd.Consulta().Read())
				{
					total = bd.LerInt("Total");
				}
				// Fechar BD
				bd.Fechar();
			}
			catch (Exception ex)
			{
				throw ex;
			} // fim de try
			return total;
		} // fim de TotalPorCanalEvento

		/// <summary>
		/// Obter a senha deste Ingresso a partir do codigo do Ingresso e Apresentacao e Setor
		/// </summary>
		/// <returns></returns>
		public override string SenhaVenda(string codigo, int apresentacaoSetorID)
		{
			string senha = "";
			try
			{
				BD bd = new BD();
				string sql =
					"SELECT     tIngressoLog.ID AS IngressoLogID, tVendaBilheteria.Senha, tIngresso.Codigo, tIngresso.ApresentacaoSetorID " +
					"FROM         tIngresso INNER JOIN " +
					"tIngressoLog ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
					"tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
					"tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID " +
					"WHERE     (tIngresso.ApresentacaoSetorID = " + apresentacaoSetorID + ") AND (tIngresso.Codigo = N'" + codigo + "') " +
					"ORDER BY tIngressoLog.ID DESC ";
				bd.Consulta(sql);
				if (bd.Consulta().Read())
				{
					senha = bd.LerString("Senha");
				}
				else
				{
					senha = "";
				}
				bd.Fechar();
			}
			catch
			{
				Debug.Fail("Erro no mehtodo string IRLib.Ingressso.Senha(string codigo, int apresentacaoSetorID)!!");
			} // fim de try
			return senha;
		} // fim de Senha


		/// <summary>
		/// Obter total de ingressos por ApresentacaoSetor e Bloqueio
		/// </summary>
		/// <returns></returns>
		public override int QuantidadePorBloqueio(int apresentacaoSetorID, int bloqueioID)
		{
			int total = 0;
			string condicao = "";
			try
			{
				//				if (bloqueioID > 0) {
				//					condicao = "HAVING      (ApresentacaoSetorID = "+ apresentacaoSetorID +") AND (BloqueioID = "+ bloqueioID +") ";
				//				} else { // canalID = 0 significa Todos Canais
				//					condicao = "HAVING      (ApresentacaoSetorID = "+ apresentacaoSetorID +") AND (BloqueioID >0) ";
				//				}
				// Fazendo consulta SQL para obter os dados
				BD bd = new BD();
				condicao = "HAVING     (Status = 'B') AND (ApresentacaoSetorID = " + apresentacaoSetorID + ") AND (BloqueioID = " + bloqueioID + ") ";
				string sql =
					"SELECT     BloqueioID, ApresentacaoSetorID, COUNT(ID) AS Quantidade " +
					"FROM         tIngresso " +
					"GROUP BY Status, BloqueioID, ApresentacaoSetorID " +
					condicao;
				bd.Consulta(sql);
				if (bd.Consulta().Read())
				{
					total = bd.LerInt("Quantidade");
				}
				// Fechar BD
				bd.Fechar();
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try		
			return total;
		} // fim de QuantidadePorBloqueio

		public static string StatusDescritivo(string letraStatus)
		{

			string resultado;

			switch (letraStatus)
			{
				case PRE_RESERVA:
					resultado = "Pré-reserva"; break;
				case AGUARDANDO_TROCA:
					resultado = "Aguardando Troca"; break;
				case PREIMPRESSO:
					resultado = "Pré-Impresso"; break;
				case DISPONIVEL:
					resultado = "Disponivel"; break;
				case BLOQUEADO:
					resultado = "Bloqueado"; break;
				case RESERVADO:
					resultado = "Reservado"; break;
				case VENDIDO:
					resultado = "Vendido"; break;
				case IMPRESSO:
					resultado = "Impresso"; break;
				case ENTREGUE:
					resultado = "Entregue"; break;
				default:
					resultado = letraStatus; break;
			}
			return resultado;

		}

		public static string StatusDetalhado(string letraStatus, string tipoEntrega)
		{

			string resultado;

			switch (letraStatus)
			{
				case PRE_RESERVA:
					resultado = "Pré-reserva"; break;
				case AGUARDANDO_TROCA:
					resultado = "Aguardando Troca"; break;
				case PREIMPRESSO:
					resultado = "Pré-Impresso"; break;
				case DISPONIVEL:
					resultado = "Disponivel"; break;
				case BLOQUEADO:
					resultado = "Bloqueado"; break;
				case RESERVADO:
					resultado = "Reservado"; break;
				case VENDIDO:
					resultado = "Aguardando Impressão"; break;
				case IMPRESSO:
					switch (tipoEntrega)
					{
						case Entrega.RETIRADA:
						case Entrega.RETIRADABILHETERIA:
							resultado = "Aguardando Retirada";
							break;
						case Entrega.NORMAL:
						case Entrega.AGENDADA:
							resultado = "Em Trânsito";
							break;
						default:
							resultado = "Impresso";
							break;
					}
					break;
				case ENTREGUE:
					resultado = "Entregue"; break;
				default:
					resultado = letraStatus; break;
			}
			return resultado;

		}

		public static string StatusDetalhado(string letraStatus, int tipoEntrega)
		{

			string resultado;

			switch (letraStatus)
			{
				case PRE_RESERVA:
					resultado = "PrÃ©-reserva"; break;
				case AGUARDANDO_TROCA:
					resultado = "Aguardando Troca"; break;
				case PREIMPRESSO:
					resultado = "PrÃ©-Impresso"; break;
				case DISPONIVEL:
					resultado = "Disponivel"; break;
				case BLOQUEADO:
					resultado = "Bloqueado"; break;
				case RESERVADO:
					resultado = "Reservado"; break;
				case VENDIDO:
					resultado = "Aguardando ImpressÃ£o"; break;
				case IMPRESSO:
					switch (tipoEntrega)
					{
						case -1:

							resultado = "Aguardando Retirada";
							break;
						case 0:
							resultado = "Impresso";

							break;
						default:
							resultado = "Em TrÃ¢nsito";
							break;
					}
					break;
				case ENTREGUE:
					resultado = "Entregue"; break;
				default:
					resultado = letraStatus; break;
			}
			return resultado;

		}

		public static System.Drawing.Color StatusCor(string descStatus)
		{

			System.Drawing.Color cor;

			if (descStatus == StatusDescritivo(PREIMPRESSO))
				cor = System.Drawing.Color.Purple;
			else if (descStatus == StatusDescritivo(DISPONIVEL))
				cor = System.Drawing.Color.Green;
			else if (descStatus == StatusDescritivo(BLOQUEADO))
				cor = System.Drawing.Color.Navy;
			else if (descStatus == StatusDescritivo(RESERVADO) || descStatus == StatusDescritivo(RESERVANDO))
				cor = System.Drawing.Color.Orange;
			else if (descStatus == StatusDescritivo(VENDIDO))
				cor = System.Drawing.Color.Red;
			else if (descStatus == StatusDescritivo(IMPRESSO))
				cor = System.Drawing.Color.DarkRed;
			else if (descStatus == StatusDescritivo(ENTREGUE))
				cor = System.Drawing.Color.DarkMagenta;
			else
				cor = System.Drawing.Color.Black;

			return cor;

		}

		/// <summary>
		/// Obtendo um DataTable dos Logs dos Ingressos e dos Itens
		/// </summary>
		/// <returns>DataTable com informações sobre Log</returns>
		public override DataTable Historico()
		{

			try
			{

				DataTable tabela = IngressoHistoricoGerenciadorParalela.EstruturaHistorico();
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT il.ID, il.[TimeStamp], il.Acao, il.Obs, p.Nome AS Preco, p.Valor, b.Nome AS Bloqueio, c.Nome AS Cortesia,il.CodigoBarra, ");
				sb.Append("e.Nome AS Evento, ");
				sb.Append("m.Motivo AS Motivo, ");
				sb.Append("lo.Nome AS Loja, i.Status, u.Nome AS Usuario, vb.Senha, i.ClienteID, ci.Nome AS Cliente, IsNull(te.Prazo, 0) AS PrazoEntrega, ");
				sb.Append("IsNull(us.Nome, ' - ') as  Supervisor , ci.CNPJ, ci.NomeFantasia ");
				sb.Append("FROM tIngresso i (NOLOCK) ");
				sb.Append("INNER JOIN tEvento e (NOLOCK) ON e.ID=i.EventoID ");
				sb.Append("INNER JOIN tIngressoLog il (NOLOCK) ON il.IngressoID=i.ID ");
				sb.Append("INNER JOIN tUsuario u (NOLOCK) ON il.UsuarioID=u.ID ");
				sb.Append("LEFT JOIN tUsuario us (NOLOCK) ON us.ID = il.SupervisorID ");
				sb.Append("LEFT JOIN tLoja lo (NOLOCK) ON il.LojaID=lo.ID ");
				sb.Append("LEFT JOIN tVendaBilheteria vb (NOLOCK) ON il.VendaBilheteriaID=vb.ID ");
				sb.Append("LEFT JOIN tTaxaEntrega te (NOLOCK) ON te.ID = vb.TaxaEntregaID ");
				sb.Append("LEFT JOIN tCliente ci (NOLOCK) ON vb.ClienteID=ci.ID ");
				sb.Append("LEFT JOIN tPreco p (NOLOCK) ON il.PrecoID=p.ID ");
				sb.Append("LEFT JOIN tCortesia c (NOLOCK) ON il.CortesiaID=c.ID ");
				sb.Append("LEFT JOIN tBloqueio b (NOLOCK) ON il.BloqueioID=b.ID ");
				sb.Append("LEFT JOIN tMotivo m (NOLOCK) ON il.MotivoID=m.ID ");
				sb.Append("WHERE i.ID=" + this.Control.ID + " ");
				sb.Append("ORDER BY il.ID, il.[TimeStamp]");
				string sql = sb.ToString();

				bd.Consulta(sql);

				while (bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["IngressoLogID"] = bd.LerInt("ID");
					linha["Data"] = bd.LerStringFormatoDataHora("TimeStamp");
					linha["ClienteID"] = bd.LerInt("ClienteID");
					linha["Cliente"] = bd.LerString("Cliente");
					linha["Senha"] = bd.LerString("Senha");
					linha["Ação"] = IngressoLog.AcaoDescritiva(bd.LerString("Acao"));
					linha["Status"] = StatusDescritivo(bd.LerString("Status"));
					linha["StatusDetalhado"] = StatusDetalhado(bd.LerString("Status"), bd.LerString("PrazoEntrega"));
					if (bd.LerString("Preco") != "")
						linha["Preço"] = bd.LerString("Preco");
					//linha["Pacote"]= bd.LerString("Pacote");
					linha["Valor"] = bd.LerDecimal("Valor");
					if (bd.LerString("Cortesia") != "")
						linha["Cortesia"] = bd.LerString("Cortesia");
					if (bd.LerString("Bloqueio") != "")
						linha["Bloqueio"] = bd.LerString("Bloqueio");
					linha["Usuario"] = bd.LerString("Usuario");
					if (bd.LerString("Loja") != "")
						linha["Loja"] = bd.LerString("Loja");
					linha["Obs"] = bd.LerString("Obs");
					linha["Evento"] = bd.LerString("Evento");
					linha["CodigoBarra"] = bd.LerString("CodigoBarra");
					linha["Motivo"] = bd.LerString("Motivo");
					linha["Supervisor"] = bd.LerString("Supervisor");

					linha["CNPJ"] = bd.LerString("CNPJ");
					linha["NomeFantasia"] = bd.LerString("NomeFantasia");

					//					if (Convert.ToInt32(bd.LerInt("PacoteID")) >0) {
					//						linha["Preco"]= "";
					//						linha["Valor"]= 0;
					//					} else{ 
					//						linha["Preco"]= bd.LerString("Preco");
					//						linha["Valor"]= bd.LerDecimal("Valor");
					//					}
					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Identifica ID do Ingresso pelos campos que realmente a identifica
		/// Apresentacao, Setor e Codigo
		/// Usando apresentacaoSetorID
		/// </summary>
		public void IdentificaPelaApresentacaoSetorID(int apresentacaoSetorID)
		{
			try
			{
				// Obtendo dados através de SQL
				string sql =
					"SELECT        tIngresso.Codigo, tApresentacaoSetor.SetorID, tApresentacaoSetor.ApresentacaoID, tIngresso.ID " +
					"FROM          tApresentacaoSetor INNER JOIN " +
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID " +
					"WHERE        (tApresentacaoSetor.ID = " + apresentacaoSetorID + ") AND (tIngresso.Codigo = '" + this.Codigo.Valor + "')";
				bd.Consulta(sql);
				// Obtendo ID do Ingresso
				if (bd.Consulta().Read())
				{
					this.Ler(bd.LerInt("ID"));
				}
				bd.Fechar();
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try			
		} // fim de IdentificaPelaApresentacaoSetorID
		/// <summary>
		/// Identifica ID do Ingresso pelos campos que realmente a identifica
		/// Apresentacao, Setor e Codigo
		/// </summary>
		public override void Identifica(int apresentacaoID, int setorID)
		{
			try
			{
				// Obtendo dados através de SQL
				/*
				string sql = 
					"SELECT        Codigo, ID "+
					"FROM            tIngresso "+
					"WHERE        (Codigo = '"+this.Codigo.Valor+"')";

				*/

				string sql =
					"SELECT        tIngresso.Codigo, tApresentacaoSetor.SetorID, tApresentacaoSetor.ApresentacaoID, tIngresso.ID " +
					"FROM          tApresentacaoSetor INNER JOIN " +
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID " +
					"WHERE        (tApresentacaoSetor.SetorID = " + setorID + ") AND (tApresentacaoSetor.ApresentacaoID = " + apresentacaoID + ") AND (tIngresso.Codigo = '" + this.Codigo.Valor + "')";

				bd.Consulta(sql);
				// Obtendo ID do Ingresso
				if (bd.Consulta().Read())
				{
					this.Ler(bd.LerInt("ID"));
				}
				else
				{
					throw new ApplicationException("Ingresso não encontrado.");
					/*this.ApresentacaoSetorID.Valor = 1;
					this.PrecoID.Valor = 1;
					this.LugarID.Valor = 1;
					this.UsuarioID.Valor = 1;
					this.CortesiaID.Valor = 1;
					this.BloqueioID.Valor = 1;
//					this.Codigo.Valor = 
					this.Status.Valor = "";
					this.Obs.Valor = "";
					this.Inserir();
//					this.Control.ID = ;
*/
				}
				bd.Fechar();
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try			
		} // fim de Identifica

		public DataTable EstruturaEventoApresentacaoSetor()
		{
			DataTable tabela = new DataTable();
			tabela.Columns.Add("Evento", typeof(string));
			tabela.Columns.Add("Apresentacao", typeof(string));
			tabela.Columns.Add("Setor/Produto", typeof(string));
			tabela.Columns.Add("Preco", typeof(string));
			tabela.Columns.Add("Valor", typeof(decimal));
			tabela.Columns.Add("EventoID", typeof(int));
			tabela.Columns.Add("ApresentacaoID", typeof(int));
			tabela.Columns.Add("SetorID", typeof(int));
			tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
			tabela.Columns.Add("PrecoID", typeof(int));
			return tabela;
		}

		public override DataTable EventoApresentacaoSetor()
		{
			DataTable tabela = EstruturaEventoApresentacaoSetor();
			try
			{
				BD banco = new BD();
				string sql =
					"SELECT        tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tIngresso.ID, " +
					"					tEvento.ID AS EventoID, tApresentacao.ID AS ApresentacaoID, tSetor.ID AS SetorID, tApresentacaoSetor.ID AS ApresentacaoSetorID  " +
					"FROM            tApresentacaoSetor INNER JOIN " +
					"tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
					"tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
					"tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID " +
					"WHERE        (tIngresso.ID = " + this.Control.ID + ") ";
				banco.Consulta(sql);
				// Alimentando DataTable
				if (banco.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["Evento"] = banco.LerString("Evento");
					linha["Apresentacao"] = banco.LerStringFormatoDataHora("Apresentacao");
					linha["Setor/Produto"] = banco.LerString("Setor");
					linha["EventoID"] = banco.LerInt("EventoID");
					linha["ApresentacaoID"] = banco.LerInt("ApresentacaoID");
					linha["SetorID"] = banco.LerInt("SetorID");
					linha["ApresentacaoSetorID"] = banco.LerInt("ApresentacaoSetorID");
					// Preço
					this.Ler(this.Control.ID);
					Preco preco = new Preco();
					preco.Ler(this.PrecoID.Valor);
					linha["Preco"] = preco.Nome.Valor;
					linha["PrecoID"] = preco.Control.ID;
					linha["Valor"] = preco.Valor.Valor;
					tabela.Rows.Add(linha);
				}
				banco.Fechar();
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try
			return tabela;
		}

		/// <summary>
		/// Exclui os logs deste ingresso e este ingresso
		/// </summary>
		/// <returns>Informa se a exclusão ocorreu com sucesso</returns>
		public override bool ExcluirCascata()
		{
			bool excluiuTodosItens = true;
			bool excluiuTudo = false;
			try
			{
				// Excluir todos logs deste ingresso
				IngressoLogLista ingressoLogLista = new IngressoLogLista();
				ingressoLogLista.FiltroSQL = "IngressoID = " + this.Control.ID;
				ingressoLogLista.Carregar();
				ingressoLogLista.Primeiro();
				int contador = 0;
				if (ingressoLogLista.Tamanho > 0)
				{
					do
					{
						excluiuTodosItens = excluiuTodosItens && ingressoLogLista.IngressoLog.Excluir();
						contador++;
						ingressoLogLista.Proximo();
					} while (excluiuTodosItens && contador < ingressoLogLista.Tamanho);
				}
				// Excluir este ingresso
				if (excluiuTodosItens)
					excluiuTudo = this.Excluir();
				// Retorna sucesso se as duas operações forem 
				return excluiuTudo;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public int LojaIDPorVenda(int vendaBilheteriaID)
		{
			object x = bd.ConsultaValor("SELECT TOP 1 LojaID FROM tIngressoLog(NOLOCK) WHERE VendaBilheteriaID = " + vendaBilheteriaID + " AND LojaID <> 0");
			return x == null ? 0 : (int)x;
		}

		public int[] LimpaReservasInternet()
		{
			try
			{
				int delayReserva = int.Parse(ConfigurationManager.AppSettings["delayReserva"]);
				ArrayList ids = new ArrayList();
                DateTime data = DateTime.Now.AddMinutes(-delayReserva);

				// voucher tem metade do tempo da reserva normal
                DateTime dataVoucher = DateTime.Now.AddMinutes(-(delayReserva/2));

				bd.FecharConsulta();

                bd.Consulta("SELECT ID,Status,ParceiroMidiaID FROM tIngresso(NOLOCK) WHERE TimeStampReserva IS NOT NULL AND TimeStampReserva <> '' AND (('" + data.ToString("yyyyMMddHHmmss") + "' > TimeStampReserva AND ParceiroMidiaID is null) OR ('" + dataVoucher.ToString("yyyyMMddHHmmss") + "' > TimeStampReserva AND ParceiroMidiaID is not null)) AND (Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' OR Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "') AND SessionID IS NOT NULL AND SessionID <> '' AND (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) ");

				BD bdUp = new BD();

                bdUp.Executar("UPDATE Voucher SET Status = 'D', ClienteID = null, SessionID = null WHERE DataUso IS NOT NULL AND '" + dataVoucher.ToString("yyyy-MM-dd HH:mm:ss") + "' > DataUso AND Status = '" + ((char)Ingresso.StatusIngresso.BLOQUEADO).ToString() + "' ");

				bool ok;
				string status;
                int parceiroId;
				while (bd.Consulta().Read())
				{
					status = bd.LerString("Status");
                    parceiroId = bd.LerInt("ParceiroMidiaID");
					
					if (status == ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString())
						ok = bdUp.Executar("UPDATE tIngresso SET SessionID = '',  Status = '" + ((char)Ingresso.StatusIngresso.PRE_RESERVA).ToString() + "', PacoteID = 0, PacoteGrupo = '', SerieID = 0 WHERE Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "' AND BloqueioID = 0 AND ID = " + bd.LerInt("ID")) > 0;
					else if (parceiroId > 0)
						ok = bdUp.Executar("UPDATE tIngresso SET SessionID = '', TimeStampReserva = '', ClienteID = 0, Status = '" + ((char)Ingresso.StatusIngresso.BLOQUEADO).ToString() + "', PacoteID = 0, PacoteGrupo = '', SerieID = 0 WHERE Status = '" + ((char)Ingresso.StatusIngresso.BLOQUEADO).ToString() + "' AND BloqueioID = 0 AND ID = " + bd.LerInt("ID")) > 0;
					else
						ok = bdUp.Executar("UPDATE tIngresso SET SessionID = '', TimeStampReserva = '', ClienteID = 0, Status = '" + ((char)Ingresso.StatusIngresso.DISPONIVEL).ToString() + "', PacoteID = 0, PacoteGrupo = '', SerieID = 0 WHERE Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' AND BloqueioID = 0 AND ID = " + bd.LerInt("ID")) > 0;

					if (ok)
						ids.Add(bd.LerInt("ID"));
				}

				return (int[])ids.ToArray(typeof(int));
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

		public List<int> LimparReservasSpecialEventInternet()
		{
			DAL oDal = new DAL();
			BD bdUp = new BD();

			try
			{
				DataTable dttBulk = new DataTable("Bulk");
				dttBulk.Columns.Add("ID", typeof(int)).DefaultValue = 0;
				DataRow dtr;
                foreach (string str in Configuracao.GetString(Configuracao.Keys.SpecialEventsID, ConfigurationManager.AppSettings["ConfigVersion"]).Split(','))
				{
					dtr = dttBulk.NewRow();
					dtr["ID"] = Convert.ToInt32(str);
					dttBulk.Rows.Add(dtr);
				}

				if (dttBulk.Rows.Count == 0)
					throw new Exception("Não foram encontrados ID's dos Eventos Especiais para efetuar a limpeza");

				int delayReserva = int.Parse(ConfigurationManager.AppSettings["delayReserva"]);
				List<int> ids = new List<int>();
				DateTime data = DateTime.Now;

				data = data.AddMinutes(-delayReserva);

				bd.FecharConsulta();


				bd.BulkInsert(dttBulk, "#tempEventoID", false, true);

				bd.Consulta("SELECT tIngresso.ID, Status, PrecoID FROM tIngresso(NOLOCK) INNER JOIN #tempEventoID ON tIngresso.EventoID = #tempEventoID.ID WHERE '" + data.ToString("yyyyMMddHHmmss") + "' > TimeStampReserva AND TimeStampReserva IS NOT NULL AND TimeStampReserva <> '' AND (Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' OR Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "') AND SessionID IS NOT NULL AND SessionID <> '' AND (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0)");



				bool ok;
				string status;
				while (bd.Consulta().Read())
				{
					status = bd.LerString("Status");
					if (status == ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString())
						ok = bdUp.Executar("UPDATE tIngresso SET SessionID = '',  Status = '" + ((char)Ingresso.StatusIngresso.PRE_RESERVA).ToString() + "', PacoteID = 0, PacoteGrupo = '', SerieID = 0 WHERE Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO_PRE_RESERVA_SITE).ToString() + "' AND ID = " + bd.LerInt("ID")) > 0;
					else
						ok = bdUp.Executar("UPDATE tIngresso SET SessionID = '', TimeStampReserva = '', ClienteID = 0, Status = '" + ((char)Ingresso.StatusIngresso.DISPONIVEL).ToString() + "', PacoteID = 0, PacoteGrupo = '', SerieID = 0 WHERE Status = '" + ((char)Ingresso.StatusIngresso.RESERVADO).ToString() + "' AND ID = " + bd.LerInt("ID")) > 0;
					if (ok)
					{
						ids.Add(bd.LerInt("PrecoID"));
						oDal.Execute("UPDATE Carrinho SET Status = 'E' WHERE IngressoID = @IngressoID AND Status = 'R'", new SqlParameter[] { new SqlParameter("@IngressoID", bd.LerInt("ID")) });
					}
				}

				return (ids);
			}
			catch
			{
				throw;
			}
			finally
			{
				bd.Fechar();
				bdUp.Fechar();
				oDal.ConnClose();
			}
		}

		public bool Atualizar(BD bd)
		{
			try
			{
				StringBuilder sql = new StringBuilder();
				sql.Append("UPDATE tIngresso SET ApresentacaoSetorID = @001, PrecoID = @002, LugarID = @003, UsuarioID = @004, CortesiaID = @005, BloqueioID = @006, Codigo = '@007', CodigoBarra = '@008', CodigoBarraCliente = '@009', Status = '@010', SetorID = @011, ApresentacaoID = @012, EventoID = @013, LocalID = @014, EmpresaID = @015, LojaID = @016, VendaBilheteriaID = @017, ClienteID = @018, PacoteID = @019, PacoteGrupo = '@020', Classificacao = @021, Grupo = @022, SessionID = '@023', TimeStampReserva = '@024', SerieID =@025 ");
				sql.Append("WHERE ID = @ID");
				sql.Replace("@ID", this.Control.ID.ToString());
				sql.Replace("@001", this.ApresentacaoSetorID.ValorBD);
				sql.Replace("@002", this.PrecoID.ValorBD);
				sql.Replace("@003", this.LugarID.ValorBD);
				sql.Replace("@004", this.UsuarioID.ValorBD);
				sql.Replace("@005", this.CortesiaID.ValorBD);
				sql.Replace("@006", this.BloqueioID.ValorBD);
				sql.Replace("@007", this.Codigo.ValorBD);
				sql.Replace("@008", this.CodigoBarra.ValorBD);
				sql.Replace("@009", this.CodigoBarraCliente.ValorBD);
				sql.Replace("@010", this.Status.ValorBD);
				sql.Replace("@011", this.SetorID.ValorBD);
				sql.Replace("@012", this.ApresentacaoID.ValorBD);
				sql.Replace("@013", this.EventoID.ValorBD);
				sql.Replace("@014", this.LocalID.ValorBD);
				sql.Replace("@015", this.EmpresaID.ValorBD);
				sql.Replace("@016", this.LojaID.ValorBD);
				sql.Replace("@017", this.VendaBilheteriaID.ValorBD);
				sql.Replace("@018", this.ClienteID.ValorBD);
				sql.Replace("@019", this.PacoteID.ValorBD);
				sql.Replace("@020", this.PacoteGrupo.ValorBD);
				sql.Replace("@021", this.Classificacao.ValorBD);
				sql.Replace("@022", this.Grupo.ValorBD);
				sql.Replace("@023", this.SessionID.ValorBD);
				sql.Replace("@024", this.TimeStampReserva.ValorBD);
				sql.Replace("@025", this.SerieID.ValorBD);

				int x = bd.Executar(sql.ToString());
				bool result = Convert.ToBoolean(x);
				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int LerEventoID(int ingressoID)
		{
			BD bd = new BD();
			try
			{
				object retorno = bd.ConsultaValor("SELECT EventoID FROm tIngresso (NOLOCK) WHERE ID = " + ingressoID);
				if (retorno is int)
					return (int)retorno;
				else
					return 0;
			}
			finally
			{
				bd.Fechar();
			}
		}

		public string VendaTEF(int ingressoID)
		{
			BD bd = new BD();
			try
			{


				object retorno = bd.ConsultaValor(@"SELECT NotaFiscalCliente FROM tVendaBilheteria vb (NOLOCK)
								INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID AND i.ID = @ID".Replace("@ID", ingressoID.ToString()));
				if (retorno is string)
					return (string)retorno;
				else
					return string.Empty;
			}
			finally
			{
				bd.Fechar();
			}
		}

		public override int VerificaCodigoPrecoExclusivo(int codigo)
		{
			int retorno = 0;
			BD bd = new BD();

			try
			{
				retorno = ((int)bd.ConsultaValor(@"SELECT COUNT(*) AS Contador FROM tIngresso (NOLOCK) WHERE PrecoExclusivoCodigoID = " + codigo + " AND Status IN ('V', 'I', 'E')"));

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}

			return retorno;
		}

		public override bool AtualizaCodigoPrecoExclusivo(int ingressosid, int codigo)
		{
			bool retorno = true;
			string strSql = "UPDATE tIngresso SET PrecoExclusivoCodigoID = " + codigo + " WHERE ID = " + ingressosid.ToString();
			BD bd = new BD();

			try
			{
				bd.Executar(strSql);
			}
			catch
			{
				retorno = false;
			}
			finally
			{
				bd.Fechar();
			}

			return retorno;
		}

		public override bool zerarCodigoPrecoExclusivo(int clienteid, string sessionid)
		{
			bool retorno = true;
			try
			{
				bd.Executar("UPDATE tIngresso SET PrecoExclusivoCodigoID = '' WHERE ClienteID = " + clienteid + " AND SessionID = '" + sessionid + "'");
			}
			catch
			{
				retorno = false;
			}

			return retorno;
		}

		public string RetornaNomeLote(int IngressoID)
		{
			try
			{
				string retorno = string.Empty;

				string sql = string.Empty;

				sql = @"SELECT tSetor.Nome AS Lote, tPreco.Nome AS Preco
				FROM tIngresso (NOLOCK)
				INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
				INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
				WHERE tIngresso. ID = " + IngressoID;

				bd.Consulta(sql);

				if (bd.Consulta().Read())
				{
					retorno = bd.LerString("Lote");
					retorno += " - ";
					retorno = bd.LerString("Preco");
				}

				return retorno;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}
		}

		public List<EstrutraIntegrantesIngresso> RetornaListaIntegrantesCamping(int IngressoID, int VendaBilheteriaID)
		{
			try
			{
				List<EstrutraIntegrantesIngresso> retorno = new List<EstrutraIntegrantesIngresso>();

				string sql = string.Empty;

				sql = @"SELECT tCampingSwu.ID as IntegranteID, tQuantidadeCamping.CampingQuantidade AS QuantidadeIntegrantes, tCampingSwu.Nome, tQuantidadeCamping.CampingQuantidade
				FROM tIngresso (NOLOCK)
				INNER JOIN tCampingSwu (NOLOCK) ON tIngresso.ID = tCampingSwu.IngressoID
				INNER JOIN tQuantidadeCamping  (NOLOCK) ON tIngresso.PrecoID = tQuantidadeCamping.PrecoID
				WHERE tIngresso.ID = " + IngressoID + @" AND tIngresso.VendaBilheteriaID = " + VendaBilheteriaID + " AND (tIngresso.Status = 'V' OR tIngresso.Status = 'I')" +
				@" GROUP BY tCampingSwu.ID, tIngresso.VendaBilheteriaID, tCampingSwu.Nome, tQuantidadeCamping.CampingQuantidade";

				bd.Consulta(sql);

				while (bd.Consulta().Read())
				{
					retorno.Add(new EstrutraIntegrantesIngresso()
					{
						IntegranteID = bd.LerInt("IntegranteID"),
						Nome = bd.LerString("Nome"),
						QuantidadeIntegrantes = bd.LerInt("QuantidadeIntegrantes"),
						QuantidadeMaximaIntegrantes = bd.LerInt("CampingQuantidade")
					});
				}

				return retorno;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				bd.Fechar();
			}
		}

		internal void AtualizarCodigoBarra(int apresentacaoSetorID, List<string> codigosApresentacaoSetor, int EventoID)
		{
			try
			{
				Dictionary<int, string> Lista = new Dictionary<int, string>();
				int cont = 0;
				string sql = string.Empty;

				sql = @"select ID from tIngresso (NOLOCK) where EventoID = " + EventoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID + " AND Status = '" + Ingresso.DISPONIVEL + "'";

				bd.Consulta(sql);

				while (bd.Consulta().Read())
				{
					Lista.Add(bd.LerInt("ID"), codigosApresentacaoSetor[cont]);
					cont++;
				}

				bd.Consulta().Close();

				foreach (var item in Lista)
				{
					bd.Executar(@"UPDATE tIngresso SET CodigoBarra = '" + item.Value + "' WHERE ID = " + item.Key);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	} // fim da classe




	public class IngressoLista : IngressoLista_B
	{

		public IngressoLista() { }


		/// <summary>
		/// Obtendo VendaBilheteriaItemID conforme uma lista de ID de Ingressos
		/// Este SELECT é muito especial e complexo, logo deixei ele pronto pra ser usado em outras rotinas
		/// </summary>
		public string ItemIDs()
		{
			string itemIDs = "";
			try
			{
				// Obtendo dados através de SQL
				string sql =
					"SELECT        MAX(tIngressoLog.IngressoID) AS IngressoID, tVendaBilheteriaItem.ID AS VendaBilheteriaItemID " +
					"FROM            tVendaBilheteriaItem INNER JOIN " +
					"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID LEFT OUTER JOIN " +
					"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID " +
					"GROUP BY tVendaBilheteriaItem.ID " +
					"HAVING        (MAX(tIngressoLog.IngressoID) IN (" + this.ToString() + ")) " +
					"ORDER BY tVendaBilheteriaItem.ID ";
				bd.Consulta(sql);
				// Alimentando dados
				if (bd.Consulta().Read())
				{
					itemIDs = bd.LerInt("VendaBilheteriaItemID").ToString();
				}
				while (bd.Consulta().Read())
				{
					itemIDs = itemIDs + "," + bd.LerInt("VendaBilheteriaItemID").ToString(); ;
				}
			}
			catch (Exception erro)
			{
				throw erro;
			} // fim de try			
			return itemIDs;
		} // fim de ItemIDs

		public Ingresso[] Ingressos2Reserva()
		{
			Ingresso[] ingressos = new Ingresso[this.tamanhoMax];
			Ingresso ing;

			string sql;

			if (tamanhoMax == 0)
				sql = "SELECT ID, Codigo FROM tIngresso";
			else
				sql = "SELECT top " + tamanhoMax + " ID FROM tIngresso";

			if (FiltroSQL != null && FiltroSQL.Trim() != "")
				sql += " WHERE " + FiltroSQL.Trim();

			if (OrdemSQL != null && OrdemSQL.Trim() != "")
				sql += " ORDER BY " + OrdemSQL.Trim();

			lista.Clear();

			bd.Consulta(sql);
			int i = 0;
			while (bd.Consulta().Read())
			{
				ing = new Ingresso();
				ing.Control.ID = bd.LerInt("ID");
				ing.Codigo.Valor = bd.LerString("Codigo");
				ingressos[i] = ing;
				i++;
			}


			if (i != this.tamanhoMax)


				lista.TrimToSize();

			return ingressos;

		}
		public DataTable RaioXPoltronas(int apresentacaoID)
		{
			try
			{
				DataTable retorno = new DataTable();
				retorno.Columns.Add("CodigoSequencial", typeof(string));
				retorno.Columns.Add("SetorNome", typeof(string));
				retorno.Columns.Add("SetorCodigo", typeof(string));
				retorno.Columns.Add("CodigoLugar", typeof(string));

				DataRow linha;

				bd.Consulta(@"SELECT DISTINCT i.CodigoSequencial,tSetor.Nome As SetorNome,
							SetorCodigo,i.Codigo AS CodigoLugar FROM tIngresso i(NOLOCK)
							INNER JOIN tCodigoBarra cb(NOLOCK) ON  cb.ApresentacaoID = i.ApresentacaoID AND cb.SetorID = i.SetorID
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = i.SetorID
							INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
							WHERE i.ApresentacaoID =" + apresentacaoID);
				while (bd.Consulta().Read())
				{
					linha = retorno.NewRow();

					linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial").ToString("000000");
					linha["SetorNome"] = bd.LerString("SetorNome");
					linha["SetorCodigo"] = bd.LerString("SetorCodigo");
					linha["CodigoLugar"] = bd.LerString("CodigoLugar");
					retorno.Rows.Add(linha);
				}
				return retorno;
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

