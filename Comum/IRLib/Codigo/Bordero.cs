using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;	

namespace IRLib
{
	public enum TipoPaganteOuCortesia 
	{
		Pagante,
		Cortesia,
		Ambos
	}

	/// <summary>
	/// Usado nos relatórios de Borderô
	/// </summary>
	public class Bordero 
	{


		# region Propriedades Locais
		private bool agruparApresentacoes = true;
		private Evento evento = new Evento();
        private string canais = "";
        private bool canaisIR = false;
		private int apresentacaoID = 0;
		private string dataInicial = "";
		private string dataFinal = "";
		private DateTime dataInicialDateTime;
		private DateTime dataFinalDateTime;
		private string apresentacoes = "";
		private string eventos = "";
		decimal totalFaturamento = 0; //mudar isso usado no resumo e por canal
		decimal publicoTotal = 0;//mudar isso usado no resumo e por canal
		private bool faturamentoConsolidadoComSetor = false;
		private bool faturamentoConsolidadoComCortesia = false;
		private DataTable faturamentoConsolidadoTabela = new DataTable();
		private DataTable arrecadacaoDespesaTabela = Utilitario.EstruturaArrecadacaoDespesa();
		private DataTable paganteTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable cortesiaTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable empresaTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable restanteTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable totalGeralTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable totalCortesiaTabela = Utilitario.EstruturaBorderoSetorPreco();
		private DataTable estatisticaSetorTabela = Utilitario.EstruturaBorderoSetorPreco();
		# endregion 
		# region Propriedades Globais
		public bool AgruparApresentacoes 
		{
			set 
			{
				agruparApresentacoes = value ; 
			}
		}
		public DataTable FaturamentoConsolidadoTabela 
		{
			get 
			{
				return faturamentoConsolidadoTabela; 
			}
		}
		public DataTable ArrecadacaoDespesaTabela 
		{
			get 
			{
				return arrecadacaoDespesaTabela; 
			}
		}
		public DataTable PaganteTabela 
		{
			get 
			{
				return paganteTabela; 
			}
		}
		public DataTable CortesiaTabela 
		{
			get 
			{
				return cortesiaTabela; 
			}
		}
		public DataTable EmpresaTabela 
		{
			get 
			{
				return empresaTabela; 
			}
		}
		public DataTable RestanteTabela 
		{
			get 
			{
				return restanteTabela; 
			}
		}
		public DataTable TotalGeralTabela 
		{
			get 
			{
				return totalGeralTabela; 
			}
		}
		public DataTable TotalCortesiaTabela 
		{
			get 
			{
				return totalCortesiaTabela; 
			}
		}
		public DataTable EstatisticaSetorTabela 
		{
			get 
			{
				return estatisticaSetorTabela; 
			}
		}
		public int ApresentacaoID 
		{
			set 
			{
				apresentacaoID = value; 
			}
		}
		public int EventoID 
		{
			set 
			{
				evento.Ler(value); 
			}
		}
		public string Apresentacoes 
		{
			set 
			{
				apresentacoes = value; 
			}
		}
		public string Eventos 
		{
			set 
			{
				eventos = value; 
			}
		}
        public string Canais
        {
            set { canais = value; }
        }
        public bool CanaisIR
        {
            get { return canaisIR; }
            set { canaisIR = value; }
        }
		public string DataFinal 
		{
			set 
			{
				dataFinal = value;
			}
		}
		public string DataInicial 
		{
			set 
			{
				dataInicial = value;
			}
		}

		public DateTime DataInicialDateTime 
		{
			get 
			{
				return dataInicialDateTime;
			}
		}

		public DateTime DataFinalDateTime 
		{
			get 
			{
				return dataFinalDateTime;
			}
		}
		public bool FaturamentoConsolidadoComSetor 
		{
			set 
			{
				faturamentoConsolidadoComSetor = value; 
			}
		}
		public bool FaturamentoConsolidadoComCortesia 
		{
			set 
			{
				faturamentoConsolidadoComCortesia = value; 
			}
		}
		# endregion 

		public Bordero() 
		{
		}

		public void Resumo(int apresentacaoID, string dataInicial,string dataFinal,int numApresentacoes)
		{
			#region Pagantes

			/***Variaveis***/
			string setorAnterior = String.Empty;
			string setorAtual = String.Empty;
			//subtotais
			decimal subTotalQuantidade = 0;
			decimal subTotalFaturamento = 0;
			decimal totalQuantidade = 0;
			decimal totalFaturamento = 0;

			IngressoLog ingressoLog = new IngressoLog();
			int eventoID = evento.Control.ID; 

			//Estrutura da tabela Pagante
			DataTable tabelaPagante = new DataTable("tabelaGetDataPagante");

			if(!agruparApresentacoes)
			{
				tabelaPagante.Columns.Add("ApresentacaoID",typeof(int));
				tabelaPagante.Columns.Add("Horario",typeof(string));
			}
			tabelaPagante.Columns.Add("Setor",typeof(string));
			tabelaPagante.Columns.Add("Preco",typeof(string));
			tabelaPagante.Columns.Add("Valor",typeof(decimal));
			tabelaPagante.Columns.Add("Quantidade",typeof(int));
			tabelaPagante.Columns.Add("Faturamento",typeof(decimal));


			

			//Carrega os dados no datatable
			tabelaPagante = getDataResumoPagante(eventoID,apresentacaoID,dataInicial,dataFinal);
			
			//tabela final pagantes
			if(!agruparApresentacoes)
				paganteTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
			else
				paganteTabela = Utilitario.EstruturaBorderoSetorPreco();
			
			

			//tabela final estatistica por setor
			//DataTable tabelaFinalEstatistica = null;

			//tabela final arrecadacao
			DataTable tabelaFinalArrecadacao = Utilitario.EstruturaArrecadacaoDespesa();

			/******Carregando as tabelas finais******/

			for (int i = 0; i <= tabelaPagante.Rows.Count - 1; i++)
			{
				DataRow linha = tabelaPagante.Rows[i];
				setorAtual = (string) linha["Setor"];
			
				if(i > 0)
				{
					if(setorAtual != setorAnterior)
					{
						//subtotal
						DataRow linhaSubtotal = paganteTabela.NewRow();

						linhaSubtotal["Setor"] = "<div style='text-align:left'>SubTotal</div>";
						if(subTotalQuantidade == 0)
							linhaSubtotal["Valor"] = Utilitario.AplicaFormatoMoeda(0);
						else
							linhaSubtotal["Valor"] = Utilitario.AplicaFormatoMoeda((subTotalFaturamento/subTotalQuantidade));
						linhaSubtotal["Quantidade"] = subTotalQuantidade;
						linhaSubtotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(subTotalFaturamento);

						paganteTabela.Rows.Add(linhaSubtotal);

						//zera subTotal
						subTotalQuantidade = 0;
						subTotalFaturamento = 0;

						//linha em branco
						DataRow linhaBranco = paganteTabela.NewRow();
						paganteTabela.Rows.Add(linhaBranco);

					}

				}
			
				DataRow linhaPaganteFinal = paganteTabela.NewRow();

				if(!agruparApresentacoes)
				{
					linhaPaganteFinal["Apresentação"] = "<div style='text-align:left'>" + linha["Horario"] + "</div>" ;
				}
				linhaPaganteFinal["Setor"] = "<div style='text-align:left'>" + linha["Setor"] + "</div>";
				linhaPaganteFinal["Preço"] = "<div style='text-align:left'>" + linha["Preco"] + "</div>";
				if(!agruparApresentacoes)
					linhaPaganteFinal["Valor"] = (Convert.ToDecimal(linha["Faturamento"])/Convert.ToDecimal(linha["Quantidade"])).ToString(Utilitario.FormatoMoeda);
				else
					linhaPaganteFinal["Valor"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linha["Valor"]));  
				linhaPaganteFinal["Quantidade"] = linha["Quantidade"];
				linhaPaganteFinal["Faturamento"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linha["Faturamento"]));

				paganteTabela.Rows.Add(linhaPaganteFinal);

				setorAnterior = setorAtual;

				//linha subtotal
				subTotalQuantidade = subTotalQuantidade + Convert.ToDecimal(linhaPaganteFinal["Quantidade"]);
				subTotalFaturamento = subTotalFaturamento + Convert.ToDecimal(linhaPaganteFinal["Faturamento"]);
				//linha Total
				totalQuantidade = totalQuantidade + Convert.ToDecimal(linhaPaganteFinal["Quantidade"]);
				totalFaturamento = totalFaturamento + Convert.ToDecimal(linhaPaganteFinal["Faturamento"]);


				//Linha Subtotal
				if(i == tabelaPagante.Rows.Count - 1)
				{
					DataRow linhaSubtotal = paganteTabela.NewRow();

					linhaSubtotal["Setor"] = "<div style='text-align:left'>Subtotal</div>";
					//para tratar divisão por zero
					if(subTotalQuantidade == 0)
						linhaSubtotal["Valor"] = 0;
					else
						linhaSubtotal["Valor"] = Utilitario.AplicaFormatoMoeda((subTotalFaturamento/subTotalQuantidade));
					linhaSubtotal["Quantidade"] = subTotalQuantidade;
					linhaSubtotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(subTotalFaturamento);

					paganteTabela.Rows.Add(linhaSubtotal);

					//linha em branco
					DataRow linhaBranco = paganteTabela.NewRow();
					paganteTabela.Rows.Add(linhaBranco);

				}
				
			}

			//Linha Total
			DataRow linhaTotal = paganteTabela.NewRow();

			linhaTotal["Setor"] = "<div style='text-align:left'>Total</div>";
			//para tratar divisão por zero
			if(totalQuantidade == 0)
				linhaTotal["Valor"] = 0;
			else
				linhaTotal["Valor"] = Utilitario.AplicaFormatoMoeda((totalFaturamento/totalQuantidade));
			linhaTotal["Quantidade"] = totalQuantidade;
			linhaTotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(totalFaturamento);

			paganteTabela.Rows.Add(linhaTotal);

			#endregion
			#region cortesias

			/***Variaveis***/
			setorAnterior = String.Empty;
			setorAtual = String.Empty;
			//subtotais
			subTotalQuantidade = 0;
			totalQuantidade = 0;

			//Estrutura da tabela Cortesia
			DataTable tabelaCortesia = new DataTable("tabelaGetDataCortesia");

			if(!agruparApresentacoes)
			{
				tabelaCortesia.Columns.Add("ApresentacaoID",typeof(int));
				tabelaCortesia.Columns.Add("Horario",typeof(string));
			}
			tabelaCortesia.Columns.Add("SetorNome",typeof(string));
			tabelaCortesia.Columns.Add("PrecoNome",typeof(string));
			tabelaCortesia.Columns.Add("Quantidade",typeof(decimal));
			tabelaCortesia.Columns.Add("Valor",typeof(decimal));
			tabelaCortesia.Columns.Add("Cortesia",typeof(string));

			tabelaCortesia = getDataResumoCortesia(eventoID,apresentacaoID,dataInicial,dataFinal);

			//tabela final cortesia
			if(!agruparApresentacoes)
				cortesiaTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
			else
				cortesiaTabela = Utilitario.EstruturaBorderoSetorPreco();

			/******Carregando as tabelas finais******/

			for (int i = 0; i <= tabelaCortesia.Rows.Count - 1; i++)
			{
				DataRow linhaCortesia = tabelaCortesia.Rows[i];
				setorAtual = (string) linhaCortesia["Setor"];
			
				if(i > 0)
				{
					if(setorAtual != setorAnterior)
					{
						//subtotal
						DataRow linhaSubtotalCortesia = cortesiaTabela.NewRow();

						linhaSubtotalCortesia["Setor"] = "<div style='text-align:left'>Subtotal</div>";
						linhaSubtotalCortesia["Valor"] = 0;
						linhaSubtotalCortesia["Quantidade"] = subTotalQuantidade;
						linhaSubtotalCortesia["Faturamento"] = 0;


						cortesiaTabela.Rows.Add(linhaSubtotalCortesia);

						//zera subTotal
						subTotalQuantidade = 0;

						//linha em branco
						DataRow linhaBrancoCortesia = cortesiaTabela.NewRow();
						cortesiaTabela.Rows.Add(linhaBrancoCortesia);

					}

				}

				DataRow linhaCortesiaFinal = cortesiaTabela.NewRow();

				if(!agruparApresentacoes)
				{
					linhaCortesiaFinal["Apresentação"] = "<div style='text-align:left'>" + linhaCortesia["Horario"] + "</div>" ;
				}
				linhaCortesiaFinal["Setor"] = "<div style='text-align:left'>" + linhaCortesia["Setor"] + "</div>";
				linhaCortesiaFinal["Preço"] = "<div style='text-align:left'>" + linhaCortesia["Preco"] + "</div>";
				linhaCortesiaFinal["Valor"] = 0;
				linhaCortesiaFinal["Quantidade"] = linhaCortesia["Quantidade"];
				linhaCortesiaFinal["Faturamento"] = 0;
				linhaCortesiaFinal["Cortesia"] = "<div style='text-align:left'>" + linhaCortesia["Cortesia"] + "</div>";


				cortesiaTabela.Rows.Add(linhaCortesiaFinal);

				setorAnterior = setorAtual;

				//linha subtotal
				subTotalQuantidade = subTotalQuantidade + Convert.ToDecimal(linhaCortesiaFinal["Quantidade"]);
				//linha Total
				totalQuantidade = totalQuantidade + Convert.ToDecimal(linhaCortesiaFinal["Quantidade"]);


				//Linha Subtotal
				if(i == tabelaCortesia.Rows.Count - 1)
				{
					DataRow linhaSubtotalCortesia = cortesiaTabela.NewRow();

					linhaSubtotalCortesia["Setor"] = "<div style='text-align:left'>Subtotal</div>";
					linhaSubtotalCortesia["Valor"] = 0;
					linhaSubtotalCortesia["Quantidade"] = subTotalQuantidade;
					linhaSubtotalCortesia["Faturamento"] = 0;

					cortesiaTabela.Rows.Add(linhaSubtotalCortesia);

					//linha em branco
					DataRow linhaBrancoCortesia = cortesiaTabela.NewRow();
					cortesiaTabela.Rows.Add(linhaBrancoCortesia);

				}
				
			}

			//Linha Total
			DataRow linhaTotalCortesia = cortesiaTabela.NewRow();

			linhaTotalCortesia["Setor"] = "<div style='text-align:left'>Total</div>";
			linhaTotalCortesia["Valor"] = 0;
			linhaTotalCortesia["Quantidade"] = totalQuantidade;
			linhaTotalCortesia["Faturamento"] = 0;

			cortesiaTabela.Rows.Add(linhaTotalCortesia);
		


			#endregion
			#region Estatistica Por Setor

			/***Variaveis***/
			string setorNome = string.Empty;
			object valorAuxiliar = 0;
			decimal publico = 0;
			decimal pagantes = 0;
			decimal bloqueados = 0;
			decimal cortesias = 0;
			int lotacao = 0;
			decimal preImpressos = 0;
			decimal preImpressosTotal = 0;
			decimal pagantesTotal = 0;
			decimal bloqueadosTotal = 0;
			decimal cortesiasTotal = 0;
			decimal LotacaoTotal = 0;
			object countVerificaSetor = 0;
			int verificaSetor = 0;
			int setorIDSetores = 0;
			publicoTotal = 0;

				
			eventoID = evento.Control.ID; 

			//Estrutura da tabela Setores
			DataTable tabelaSetores = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
			tabelaSetores.Columns.Add("ID",typeof(int));
			tabelaSetores.Columns.Add("Nome",typeof(string));


			//Estrutura da tabela Estatistica por setor
			DataTable tabelaEstatistica = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
			tabelaEstatistica.Columns.Add("SetorID",typeof(int));
			tabelaEstatistica.Columns.Add("Setor",typeof(string));
			tabelaEstatistica.Columns.Add("Ingressos",typeof(decimal));
			tabelaEstatistica.Columns.Add("CortesiaID",typeof(int));
			tabelaEstatistica.Columns.Add("Status",typeof(string));
				
			tabelaSetores = getSetores(eventoID,apresentacaoID);
			tabelaEstatistica = getDataResumoEstatisticaPorSetor(eventoID,apresentacaoID,dataInicial,dataFinal);
			estatisticaSetorTabela = Utilitario.EstruturaSetorLotacao();
			
			//Carregando a tabela final

			for (int i = 0; i <= tabelaSetores.Rows.Count - 1; i++)
			{
				DataRow linhaSetor = tabelaSetores.Rows[i];
				setorIDSetores = (int)linhaSetor["ID"];
				setorNome = (string) linhaSetor["Nome"];
				bloqueados = 0;
				cortesias = 0;
				pagantes = 0;
				preImpressos = 0;
				publico = 0;
					
					
					

				//if(setorAtual != setorAnterior)
				//{
				//Procura o setor na tabela estatística
				countVerificaSetor = tabelaEstatistica.Compute("COUNT(SetorID)","SetorID = " + setorIDSetores); 
				verificaSetor = countVerificaSetor == DBNull.Value ? 0 : (int)countVerificaSetor;

				if(verificaSetor > 0)
				{
					//Compute

					//lotacao
					lotacao = LotacaoPorSetorApresentacoes(setorIDSetores);

					//qtde total de ingressos bloqueados para o setor 
					valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)","SetorID = '" + setorIDSetores + "' and status = 'B' ");
					bloqueados = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//qtde total de cortesias para o setor 
                    valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)", "SetorID = '" + setorIDSetores + "' and (status = 'A' or status = 'V' or status = 'I' or status = 'E') and CortesiaID > 0 ");
					cortesias = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

					//qtde total de publico pagantes para o setor
                    valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)", "SetorID = '" + setorIDSetores + "' and (status = 'A' or status = 'V' or status = 'I' or status = 'E') and CortesiaID = 0 ");
					pagantes = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
					
					//qtde total de pré-impressos para o setor
					valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)","SetorID = '" + setorIDSetores + "' and status = 'P' ");
					preImpressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;
					
					//qtde total de publico para o setor
					publico = pagantes + cortesias;   /* <- <- PRE IMPRESSOS AQUI */

					//Valores para linha de totais
					publicoTotal = publicoTotal + publico;
					preImpressosTotal = preImpressosTotal + preImpressos;
					pagantesTotal = pagantesTotal + pagantes;
					bloqueadosTotal = bloqueadosTotal + bloqueados ;
					cortesiasTotal = cortesiasTotal + cortesias ;
					//ocupacaoTotal = ocupacaoTotal + ((100 * publico)/lotacao);
					LotacaoTotal = LotacaoTotal + lotacao;

					DataRow linhaEstatisticaFinal = estatisticaSetorTabela.NewRow();

					linhaEstatisticaFinal["Setor"] = "<div style='text-align:left'>" + setorNome + "</div>";
					linhaEstatisticaFinal["Lotação"] = lotacao;
					linhaEstatisticaFinal["Bloqueados"] = bloqueados;
					linhaEstatisticaFinal["Cortesia"] = cortesias;
					linhaEstatisticaFinal["Pagantes"] = pagantes;
					linhaEstatisticaFinal["Público"] = publico;
					linhaEstatisticaFinal["Pré-Impressos"] = preImpressos;
					linhaEstatisticaFinal["Disponível"] = lotacao - (bloqueados + publico + preImpressos);
					if(lotacao == 0)
						linhaEstatisticaFinal["% ocupação"] = Math.Round(0.0);
					else	
						linhaEstatisticaFinal["% ocupação"] = Math.Round(((100 * publico)/lotacao),1);


					estatisticaSetorTabela.Rows.Add(linhaEstatisticaFinal);
				}
				else // setores que não tem venda
				{
					//lotacao
					lotacao = LotacaoPorSetorApresentacoes(setorIDSetores);
					LotacaoTotal = LotacaoTotal + lotacao;

					DataRow linhaEstatisticaFinal = estatisticaSetorTabela.NewRow();

					linhaEstatisticaFinal["Setor"] = "<div style='text-align:left'>" + setorNome + "</div>";
					linhaEstatisticaFinal["Lotação"] = lotacao;
					linhaEstatisticaFinal["Bloqueados"] = bloqueados;
					linhaEstatisticaFinal["Cortesia"] = cortesias;
					linhaEstatisticaFinal["Pagantes"] = pagantes;
					linhaEstatisticaFinal["Público"] = publico;
					linhaEstatisticaFinal["Pré-Impressos"] = preImpressos;
					linhaEstatisticaFinal["Disponível"] = lotacao - (bloqueados + publico + preImpressos);
					linhaEstatisticaFinal["% ocupação"] = 0;

					estatisticaSetorTabela.Rows.Add(linhaEstatisticaFinal);
				}
			
			}
			//linha de totais
			DataRow linhaBrancoEstatistica = estatisticaSetorTabela.NewRow();
			estatisticaSetorTabela.Rows.Add(linhaBrancoEstatistica);

			DataRow linhaEstatisticaTotal = estatisticaSetorTabela.NewRow();

			linhaEstatisticaTotal["Setor"] = "<div style='text-align:left'>Total</div>";
			linhaEstatisticaTotal["Lotação"] = LotacaoTotal;
			linhaEstatisticaTotal["Bloqueados"] = bloqueadosTotal;
			linhaEstatisticaTotal["Cortesia"] = cortesiasTotal;
			linhaEstatisticaTotal["Pagantes"] = pagantesTotal;
			linhaEstatisticaTotal["Público"] = publicoTotal;
			linhaEstatisticaTotal["Pré-Impressos"] = preImpressosTotal;
			linhaEstatisticaTotal["Disponível"] = LotacaoTotal - (bloqueadosTotal + publicoTotal + preImpressosTotal);
			if(LotacaoTotal == 0)
				linhaEstatisticaTotal["% ocupação"] = Math.Round(0.0);
			else
				linhaEstatisticaTotal["% ocupação"] = Math.Round(((100 * publicoTotal)/LotacaoTotal),1);


			estatisticaSetorTabela.Rows.Add(linhaEstatisticaTotal);


			#endregion
			#region Arrecadação

			//Variaveis
			string PorValor = string.Empty;
			decimal totalDespesas = 0;
			int totalImpressao = 0;

			//Estrutura da tabela Despesas do Evento
			DataTable tabelaDespesas = new DataTable("tabelaGetDataArrecadacao");
			
			tabelaDespesas.Columns.Add("Nome",typeof(string));
			tabelaDespesas.Columns.Add("PorValor",typeof(string));
			tabelaDespesas.Columns.Add("Porcentagem",typeof(decimal));
			tabelaDespesas.Columns.Add("ValorMinimo",typeof(decimal));

			tabelaDespesas = getDataArrecadacao(eventoID);
			arrecadacaoDespesaTabela = Utilitario.EstruturaArrecadacaoDespesa();

			//Carregando a tabela final

			//Arrecadação
			DataRow linhaArrecadacao = arrecadacaoDespesaTabela.NewRow();
			linhaArrecadacao["Descricao"] = "<div style='text-align:left'>Arrecadação Bruta</div>";
			linhaArrecadacao["Despesa"] = totalFaturamento;
			arrecadacaoDespesaTabela.Rows.Add(linhaArrecadacao);

			//Despesas
			for (int i = 0; i <= tabelaDespesas.Rows.Count - 1; i++)
			{
				DataRow linhaDespesas = tabelaDespesas.Rows[i];
				PorValor = (string)linhaDespesas["PorValor"];

				//despesas na tabelafinal

				DataRow LinhaDespesasFinal = arrecadacaoDespesaTabela.NewRow();
				
				LinhaDespesasFinal["Descricao"] = "<div style='text-align:left'>" + linhaDespesas["Nome"] + "</div>";
				if(PorValor == "T")
				{
					//Obtem a quantidade de impressos
					Ingresso oIngresso = new Ingresso();
					if(apresentacaoID > 0)
						totalImpressao = oIngresso.TotalImpressoApresentacao(apresentacaoID);
					else 
						totalImpressao = oIngresso.TotalImpressoApresentacoes(apresentacoes); 


					LinhaDespesasFinal["Valor/%"] = linhaDespesas["Porcentagem"];
					LinhaDespesasFinal["Despesa"] = (Convert.ToDecimal(linhaDespesas["Porcentagem"]) * publicoTotal).ToString(Utilitario.FormatoMoeda);
				}
				else
				{
					string porcentagem = linhaDespesas["Porcentagem"].ToString();
					LinhaDespesasFinal["Valor/%"] = porcentagem;// + "%" ;
					decimal despesa = Convert.ToDecimal(linhaDespesas["Porcentagem"]) * totalFaturamento/100;
					decimal valorMinimo = Convert.ToDecimal(linhaDespesas["ValorMinimo"]) * numApresentacoes;

					if(despesa > valorMinimo)
						LinhaDespesasFinal["Despesa"] = despesa.ToString(Utilitario.FormatoMoeda);
					else 
						LinhaDespesasFinal["Despesa"] = valorMinimo.ToString(Utilitario.FormatoMoeda);

				}

				arrecadacaoDespesaTabela.Rows.Add(LinhaDespesasFinal);

				//total de despesas
				totalDespesas = totalDespesas + Convert.ToDecimal(LinhaDespesasFinal["Despesa"]);

			}

			//Arrecadação Líquida

			DataRow LinhaArrecadacaoLiquida = arrecadacaoDespesaTabela.NewRow();

			LinhaArrecadacaoLiquida["Descricao"] = "<div style='text-align:left'>Arrecadação Líquida</div>";
			LinhaArrecadacaoLiquida["Despesa"] = (totalFaturamento - totalDespesas).ToString(Utilitario.FormatoMoeda);

			arrecadacaoDespesaTabela.Rows.Add(LinhaArrecadacaoLiquida);


			#endregion 

		}
		
		

		
		public void PorCanal(int apresentacaoID, string dataInicial,string dataFinal,int eventoID,int numApresentacoes) 
		{
			try
			{
				// Informações sobre Empresa deste Evento
				int empresaID = evento.EmpresaID();
				Empresa empresa = new Empresa();
				empresa.Ler(empresaID);
				string empresaNome =empresa.Nome.Valor; 
				// Setores conforme o filtro
				DataTable setoresTabelaPagantes = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Pagante); 	
				DataTable canaisTabela;
				string canais="";
				IngressoLog ingressoLog = new IngressoLog(); 
				if (setoresTabelaPagantes.Rows.Count>0)
				{
					#region Por empresa
					canais="";
					canaisTabela = empresa.Canais();
					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
					{
						if (indice ==0) 
							canais = canaisTabela.Rows[indice]["ID"].ToString();
						else
							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
					}
					//se o canal estiver vazio, inserir "0"
					canais = canais == "" ? "0" : canais;
					
					//empresaTabela = ObterTabelaPorSetorPrecoCanais(setoresTabelaPagantes, canais, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
					empresaTabela = ObterTabelaPorSetorPrecoCanais(apresentacaoID,canais,dataInicial,dataFinal);
					#endregion
					#region IR (restante)
					canais = empresa.CanaisIR();
					//se o canal estiver vazio, inserir "0"
					canais = canais == "" ? "0" : canais;

					restanteTabela = ObterTabelaPorSetorPrecoCanais(apresentacaoID,canais,dataInicial,dataFinal);
					#endregion
					#region Total
					canais="";
					canaisTabela = empresa.CanaisQueVendem(null);
					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
					{
						if (indice ==0) 
							canais = canaisTabela.Rows[indice]["ID"].ToString();
						else
							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
					}
					//se o canal estiver vazio, inserir "0"
					canais = canais == "" ? "0" : canais;
					totalGeralTabela = ObterTabelaPorSetorPrecoCanais(apresentacaoID,canais,dataInicial,dataFinal);
					#endregion
				}
				//
				DataTable setoresTabelaCortesia = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Cortesia); 	
				canais="";
				canaisTabela = empresa.CanaisQueVendem(null);
				for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
				{
					if (indice ==0) 
						canais = canaisTabela.Rows[indice]["ID"].ToString();
					else
						canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
				}
				//se o canal estiver vazio, inserir "0"
				canais = canais == "" ? "0" : canais;

				// Total de Cortesias
				if (setoresTabelaCortesia.Rows.Count>0) 
				{
					totalCortesiaTabela = ObterTabelaPorSetorPrecoCanaisCortesia(apresentacaoID,canais,dataInicial,dataFinal);
				}
				// Estatística por setor
				estatisticaSetorTabela = ObterTabelaPorSetorLotacaoCanais(apresentacaoID,canais,dataInicial,dataFinal);

				//Arrecadação
				#region Arrecadação

	

				//Variaveis
				string PorValor = string.Empty;
				decimal totalDespesas = 0;
				int totalImpressao = 0;

				//Estrutura da tabela Despesas do Evento
				DataTable tabelaDespesas = new DataTable("tabelaGetDataArrecadacao");
			
				tabelaDespesas.Columns.Add("Nome",typeof(string));
				tabelaDespesas.Columns.Add("PorValor",typeof(string));
				tabelaDespesas.Columns.Add("Porcentagem",typeof(decimal));
				tabelaDespesas.Columns.Add("ValorMinimo",typeof(decimal));


				tabelaDespesas = getDataArrecadacao(eventoID);
				arrecadacaoDespesaTabela = Utilitario.EstruturaArrecadacaoDespesa();

				//Carregando a tabela final

				//Arrecadação
				DataRow linhaArrecadacao = arrecadacaoDespesaTabela.NewRow();
				linhaArrecadacao["Descricao"] = "<div style='text-align:left'>Arrecadação Bruta</div>";
				linhaArrecadacao["Despesa"] = totalFaturamento;
				arrecadacaoDespesaTabela.Rows.Add(linhaArrecadacao);

				//Despesas
				for (int i = 0; i <= tabelaDespesas.Rows.Count - 1; i++)
				{
					DataRow linhaDespesas = tabelaDespesas.Rows[i];
					PorValor = (string)linhaDespesas["PorValor"];

					//despesas na tabelafinal

					DataRow LinhaDespesasFinal = arrecadacaoDespesaTabela.NewRow();
				
					LinhaDespesasFinal["Descricao"] = "<div style='text-align:left'>" + linhaDespesas["Nome"] + "</div>";
					if(PorValor == "T")
					{
						//Obtem a quantidade de impressos
						Ingresso oIngresso = new Ingresso();
						if(apresentacaoID > 0)
							totalImpressao = oIngresso.TotalImpressoApresentacao(apresentacaoID);
						else 
							totalImpressao = oIngresso.TotalImpressoApresentacoes(apresentacoes); 

						LinhaDespesasFinal["Valor/%"] = linhaDespesas["Porcentagem"];
						LinhaDespesasFinal["Despesa"] = (Convert.ToDecimal(linhaDespesas["Porcentagem"]) * publicoTotal).ToString(Utilitario.FormatoMoeda);
					}
					else
					{
						string porcentagem = linhaDespesas["Porcentagem"].ToString();
						LinhaDespesasFinal["Valor/%"] = porcentagem;
						decimal despesa = Convert.ToDecimal(linhaDespesas["Porcentagem"]) * totalFaturamento/100;
						decimal valorMinimo = Convert.ToDecimal(linhaDespesas["ValorMinimo"]) * numApresentacoes;

						if(despesa > valorMinimo)
							LinhaDespesasFinal["Despesa"] = despesa.ToString(Utilitario.FormatoMoeda);
						else 
							LinhaDespesasFinal["Despesa"] = valorMinimo.ToString(Utilitario.FormatoMoeda);
					}

					arrecadacaoDespesaTabela.Rows.Add(LinhaDespesasFinal);

					//total de despesas
					totalDespesas = totalDespesas + Convert.ToDecimal(LinhaDespesasFinal["Despesa"]);

				}

				//Arrecadação Líquida

				DataRow LinhaArrecadacaoLiquida = arrecadacaoDespesaTabela.NewRow();

				LinhaArrecadacaoLiquida["Descricao"] = "<div style='text-align:left'>Arrecadação Líquida</div>";
				LinhaArrecadacaoLiquida["Despesa"] = (totalFaturamento - totalDespesas).ToString(Utilitario.FormatoMoeda);

				arrecadacaoDespesaTabela.Rows.Add(LinhaArrecadacaoLiquida);


				
				/*			EventoDespesaLista eventoDespesaLista = new EventoDespesaLista();
							eventoDespesaLista.FiltroSQL = "EventoID= " +evento.Control.ID;
							eventoDespesaLista.OrdemSQL = "PorValor";
							eventoDespesaLista.Carregar();
							eventoDespesaLista.Primeiro();
							arrecadacaoDespesaTabela = Utilitario.EstruturaArrecadacaoDespesa();;
							if (eventoDespesaLista.Tamanho>0) 
							{
								decimal formato ;
								decimal somaDespesas =0;
								decimal valorPagantes = 0;
								if(totalGeralTabela.Rows.Count > 0)
								{
									valorPagantes = (decimal) totalGeralTabela.Rows[totalGeralTabela.Rows.Count-1]["Faturamento"];
								}
								DataRow arrecadacaoDespesaLinha;
								arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
								arrecadacaoDespesaLinha["Descricao"] = "Arrecadação Bruta";
								arrecadacaoDespesaLinha["Despesa"] = valorPagantes.ToString(Utilitario.FormatoMoeda);
								arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
								for (int contador = 0; contador< eventoDespesaLista.Tamanho ;contador++) 
								{
									arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
									arrecadacaoDespesaLinha["Descricao"] = eventoDespesaLista.EventoDespesa.Nome.Valor;
									arrecadacaoDespesaLinha["Valor/%"] = eventoDespesaLista.EventoDespesa.Porcentagem.Valor;
									if (eventoDespesaLista.EventoDespesa.PorValor.Valor) 
									{
										Ingresso oIngresso = new Ingresso();
										int totalImpressao = oIngresso.TotalImpresso(evento.Control.ID);

										formato = eventoDespesaLista.EventoDespesa.Porcentagem.Valor*totalImpressao ;
									} 
									else 
									{
										formato = eventoDespesaLista.EventoDespesa.Porcentagem.Valor*valorPagantes/100;
									}
									arrecadacaoDespesaLinha["Despesa"] = formato.ToString(Utilitario.FormatoMoeda);
									somaDespesas += formato;
									arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
									eventoDespesaLista.Proximo();
								}
								arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
								arrecadacaoDespesaLinha["Descricao"] = "Arrecadação Líquida";
								formato = valorPagantes - somaDespesas;
								arrecadacaoDespesaLinha["Despesa"] = formato.ToString(Utilitario.FormatoMoeda);
								arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
							}*/
				#endregion

			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // PorCanal


		public DataTable ConferenciaIngressos(int ApresentacaoID, int SetorID)
		{
			//estruturta da tabela
			DataTable tabela = new DataTable("PreImpressoEvento");
			
			
			tabela.Columns.Add("Senha",typeof(string));
			tabela.Columns.Add("CodigoBarra",typeof(string));
			tabela.Columns.Add("ClienteNome",typeof(string));
			tabela.Columns.Add("SetorNome",typeof(string));
			tabela.Columns.Add("Codigo",typeof(string));
			tabela.Columns.Add("PrecoNome", typeof(string));
			tabela.Columns.Add("Valor",typeof(decimal));
			tabela.Columns.Add("Status",typeof(string));
			tabela.Columns.Add("CortesiaNome", typeof(string));

			BD bd = new BD();

            String status = "'" + (char)Ingresso.StatusIngresso.AGUARDANDO_TROCA + "', '" + (char)Ingresso.StatusIngresso.VENDIDO + "', '" +
                            (char)Ingresso.StatusIngresso.IMPRESSO + "', '" + (char)Ingresso.StatusIngresso.ENTREGUE + "', '" + (char)Ingresso.StatusIngresso.PREIMPRESSO + "' ,'" + (char)Ingresso.StatusIngresso.PRE_RESERVA + "'";

            string sql = @"SELECT tVendabilheteria.Senha,tIngresso.CodigoBarra,tCliente.Nome as ClienteNome,tSetor.Nome as SetorNome,tIngresso.Codigo,tPreco.Valor,tIngresso.Status, tPreco.Nome AS PrecoNome, tCortesia.Nome AS CortesiaNome 
							FROM tIngresso (NOLOCK)
							INNER JOIN tIngressolog (NOLOCK) on tIngresso.ID = tIngressolog.IngressoID
							INNER JOIN tVendabilheteria (NOLOCK) on tVendaBilheteria.ID = tIngressolog.VendaBilheteriaID AND tIngresso.VendaBilheteriaID = tIngressolog.VendaBilheteriaID
							LEFT JOIN tCliente (NOLOCK) on tIngressolog.ClienteID = tCliente.ID
							INNER JOIN tSetor (NOLOCK) on tIngresso.SetorID = tSetor.ID
							INNER JOIN tPreco (NOLOCK) on tIngresso.PrecoID = tPreco.ID
							LEFT JOIN tCortesia (NOLOCK) ON tIngresso.CortesiaID = tCortesia.ID

							WHERE tIngresso.Status IN (" + status + ") AND tIngressoLog.Acao = 'V' AND tIngresso.ApresentacaoID = " + ApresentacaoID;
			if(SetorID > 0)
				sql += " AND tSetor.ID = " + SetorID;
			sql += @"ORDER BY tcliente.Nome,tVendabilheteria.Senha,tIngresso.Codigo,tIngresso.Status";
			
			bd.Consulta(sql);

			while(bd.Consulta().Read()) 
			{
				DataRow linhaTabela = tabela.NewRow();

				
				linhaTabela["Senha"]= bd.LerString("Senha");
				linhaTabela["CodigoBarra"]=bd.LerString("CodigoBarra");
				linhaTabela["ClienteNome"]= bd.LerString("ClienteNome");
				linhaTabela["SetorNome"]=bd.LerString("SetorNome");
				linhaTabela["Codigo"]=bd.LerString("Codigo");
				linhaTabela["Valor"]=bd.LerDecimal("Valor");
				linhaTabela["Status"]= Ingresso.StatusDescritivo(bd.LerString("Status"));
				linhaTabela["PrecoNome"]= bd.LerString("PrecoNome");
				linhaTabela["CortesiaNome"]= bd.LerString("CortesiaNome");
				
			
				
				tabela.Rows.Add(linhaTabela);
			}

			return tabela;


		}

	
		#region Bloqueios
		
		public DataTable GetDataEstatisticaBloqueios(int ApresentacaoID)
		{
			//estruturta da tabela
			DataTable tabela = new DataTable("Bloqueios");
			
			tabela.Columns.Add("SetorID",typeof(int));
			tabela.Columns.Add("SetorNome",typeof(string));
			tabela.Columns.Add("BloqueioID",typeof(int));
			tabela.Columns.Add("BloqueioNome",typeof(string));
			tabela.Columns.Add("Quantidade",typeof(decimal));

			BD bd = new BD();

			string sql = @"SELECT tSetor.ID AS SetorID,tSetor.Nome AS SetorNome,tBloqueio.ID AS BloqueioID,tBloqueio.Nome AS BloqueioNome,COUNT(tIngresso.ID) AS Quantidade 
							FROM tIngresso (NOLOCK)
							INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
							INNER JOIN tBloqueio (NOLOCK) ON tBloqueio.ID = tIngresso.BloqueioID
							INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
							WHERE tIngresso.ApresentacaoID = " + ApresentacaoID + @" AND tIngresso.BloqueioID > 0 AND tApresentacao.DisponivelRelatorio = 'T'
							GROUP BY tSetor.ID,tSetor.Nome,tBloqueio.ID,tBloqueio.Nome 
							ORDER BY tSetor.Nome,tBloqueio.Nome";

			
			bd.Consulta(sql);

			while(bd.Consulta().Read()) 
			{
				DataRow linhaTabela = tabela.NewRow();

				
				linhaTabela["SetorID"]= bd.LerInt("SetorID");
				linhaTabela["SetorNome"]=bd.LerString("SetorNome");
				linhaTabela["BloqueioID"]= bd.LerInt("BloqueioID");
				linhaTabela["BloqueioNome"]=bd.LerString("BloqueioNome");
				linhaTabela["Quantidade"]=bd.LerDecimal("Quantidade");
				
				tabela.Rows.Add(linhaTabela);
			}

			return tabela;

		}

		public DataTable EstatisticaBloqueios(int apresentacaoID)
		{
			//variaveis
			object valorAuxiliar = 0;
			decimal qtdIngresso = 0;
			decimal qtdTotalIngresso = 0;
//			decimal valorTotal = 0;
//			int SetorID = 0;
//			int BloqueioID = 0;
			string SetorNome = string.Empty;
			string BloqueioNome = string.Empty;
			DataRow linhaFinal = null;
			DataTable dtSetores;
			DataTable dtBloqueios;
			DataRow linhaSetor;
			DataRow linhaBloqueio;


			DataTable tabela = GetDataEstatisticaBloqueios(apresentacaoID);

			//tabela que carrega o Grid
			DataTable tabelaFinal = Utilitario.EstruturaEstatisticaBloqueios();

			string[] colunasSetores = new string[2];

			colunasSetores[0] = "SetorID";
			colunasSetores[1] = "SetorNome";

			dtSetores = CTLib.TabelaMemoria.DistinctSort(tabela,"SetorNome","1=1",colunasSetores);

			for(int i=0;i<=dtSetores.Rows.Count - 1;i++)
			{
				linhaSetor = dtSetores.Rows[i];
				
				//insere nome do Setor
				linhaFinal = tabelaFinal.NewRow();

				tabelaFinal.Rows.Add(linhaFinal);

				linhaFinal["Setor"] = "<div style='text-align:left';>" + linhaSetor["SetorNome"] + "</div>";

				string[] colunasBloqueios = new string[3];

				colunasBloqueios[0] = "BloqueioID";
				colunasBloqueios[1] = "BloqueioNome";
				colunasBloqueios[2] = "Quantidade";

				dtBloqueios = CTLib.TabelaMemoria.DistinctSort(tabela,"BloqueioNome","SetorID = " + linhaSetor["SetorID"],colunasBloqueios);

				for(int j=0;j<=dtBloqueios.Rows.Count - 1;j++)
				{
					linhaBloqueio = dtBloqueios.Rows[j];
				
					//insere nome do Setor
					linhaFinal = tabelaFinal.NewRow();
					linhaFinal["Bloqueio"] = "<div style='text-align:left';>" + linhaBloqueio["BloqueioNome"] + "</div>";
					//Quantidade de cada Bloqueio
					linhaFinal["Quantidade"] = linhaBloqueio["Quantidade"];
					
					tabelaFinal.Rows.Add(linhaFinal);
					
					//para linha de totais
					qtdIngresso = qtdIngresso + (decimal)linhaBloqueio["Quantidade"];

				}
                
				//insere a linha total do Evento
				DataRow linhaTotalSetor = tabelaFinal.NewRow();

				linhaTotalSetor["Setor"] = "<div style='font-weight:bold;text-align:left'>Total Setor " + linhaSetor["SetorNome"] + "</div>";
				linhaTotalSetor["Quantidade"] = qtdIngresso.ToString();
				
				tabelaFinal.Rows.Add(linhaTotalSetor);
                qtdIngresso = 0;

				//total geral
				qtdTotalIngresso = qtdTotalIngresso + qtdIngresso;
			}
			//insere a linha total 
			DataRow linhaTotal = tabelaFinal.NewRow();

			linhaTotal["Setor"] = "<div style='font-weight:bold;text-align:left'>Total</div>";
			linhaTotal["Quantidade"] = "<div style='font-weight:bold'>" + qtdTotalIngresso.ToString() + "</div>";
				
			tabelaFinal.Rows.Add(linhaTotal);

			return tabelaFinal;
		}

		#endregion




		private DataTable LinhasSetorPrecoAgruparApresentacao(int setorID, TipoPaganteOuCortesia paganteCondicao)
		{
			try
			{
				string cortesiaCondicao ="";
				switch (paganteCondicao)
				{
					case TipoPaganteOuCortesia.Pagante:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0) ";
						break;
					case TipoPaganteOuCortesia.Cortesia:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0) ";
						break;
					case TipoPaganteOuCortesia.Ambos:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0) ";
						break;
				}
				DataTable tabela = Utilitario.EstruturaBorderoSetorPreco();
				// Obtendo dados através de SQL
				BD obterDados = new BD();
				string sql =
					"SELECT DISTINCT tSetor.Nome + tPreco.Nome + tCortesia.Nome AS Unico, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tIngresso.SetorID, tCortesia.Nome AS Cortesia, tIngressoLog.CortesiaID "+
					"FROM            tIngressoLog INNER JOIN "+
					"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID INNER JOIN "+
					"tSetor ON tIngresso.SetorID = tSetor.ID LEFT OUTER JOIN "+
					"tCortesia ON tIngressoLog.CortesiaID = tCortesia.ID "+
					"WHERE        (tIngresso.ApresentacaoID IN ("+apresentacoes+")) AND (tIngresso.SetorID = "+setorID+") "+ cortesiaCondicao +" "+					
					"ORDER BY tSetor.Nome, tPreco.Nome ";
				obterDados.Consulta(sql);
				while(obterDados.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["SetorID"]= obterDados.LerInt("SetorID");
					linha["CortesiaID"]= obterDados.LerInt("CortesiaID");
					linha["Setor"]= obterDados.LerString("Setor");
					linha["Preço"]= obterDados.LerString("Preco");
					linha["Cortesia"]= obterDados.LerString("Cortesia");
					tabela.Rows.Add(linha);
				}
				obterDados.Fechar();
				return tabela;
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		}

		private DataTable LinhasSetorPrecoApresentacao(int setorID, TipoPaganteOuCortesia paganteCondicao)
		{
			try
			{
				string cortesiaCondicao ="";
				switch (paganteCondicao)
				{
					case TipoPaganteOuCortesia.Pagante:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0) ";
						break;
					case TipoPaganteOuCortesia.Cortesia:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0) ";
						break;
					case TipoPaganteOuCortesia.Ambos:
						cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0) ";
						break;
				}
				DataTable tabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
				// Obtendo dados através de SQL
				BD obterDados = new BD();
				string sql =
					"SELECT DISTINCT  "+
					"tSetor.Nome + tPreco.Nome + tCortesia.Nome AS Unico, tApresentacao.Horario, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tIngresso.SetorID, tPreco.ID AS PrecoID,  "+
					"tCortesia.Nome AS Cortesia, tIngressoLog.CortesiaID, tIngresso.ApresentacaoID "+
					"FROM            tCortesia RIGHT OUTER JOIN "+
					"tApresentacao INNER JOIN "+
					"tIngressoLog INNER JOIN "+
					"tSetor INNER JOIN "+
					"tIngresso ON tSetor.ID = tIngresso.SetorID ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID ON tApresentacao.ID = tIngresso.ApresentacaoID ON tCortesia.ID = tIngressoLog.CortesiaID "+
					"WHERE        (tIngresso.SetorID = "+setorID+") AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) "+ cortesiaCondicao +" "+
					"ORDER BY tApresentacao.Horario, tSetor.Nome, tPreco.Nome ";
				obterDados.Consulta(sql);
				while(obterDados.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["SetorID"]= obterDados.LerInt("SetorID");
					linha["PrecoID"]= obterDados.LerInt("PrecoID");
					linha["CortesiaID"]= obterDados.LerInt("CortesiaID");
					linha["Apresentação"]= obterDados.LerStringFormatoSemanaDataHora("Horario");
					linha["Setor"]= obterDados.LerString("Setor");
					linha["Preço"]= obterDados.LerString("Preco");
					linha["Cortesia"]= obterDados.LerString("Cortesia");
					tabela.Rows.Add(linha);
				}
				obterDados.Fechar();
				return tabela;
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		}

		#region GetDataResumo

		//*****Obtem os dados para a tabela de pagantes do borderô resumo
		public DataTable getDataResumoPagante(int eventoID,int apresentacaoID,string dataInicial,string dataFinal)
		{
			
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaPagante = new DataTable("tabelaGetDataPagante");
			
			//Estrutura da tabela Pagante

			if(!agruparApresentacoes)
			{
				tabelaPagante.Columns.Add("ApresentacaoID",typeof(int));
				tabelaPagante.Columns.Add("Horario",typeof(string));
			}
			tabelaPagante.Columns.Add("Setor",typeof(string));
			tabelaPagante.Columns.Add("Preco",typeof(string));
			tabelaPagante.Columns.Add("Valor",typeof(decimal));
			tabelaPagante.Columns.Add("Quantidade",typeof(int));
			tabelaPagante.Columns.Add("Faturamento",typeof(decimal));
	

			BD bd = new BD();
			string sql  = string.Empty;
			
			if(agruparApresentacoes)
			{
				sql =
                    @"SELECT tsetor.nome as Setor,tpreco.nome as Preco,tpreco.valor as Valor,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Faturamento
				FROM tEvento (NOLOCK)
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID = " + eventoID ;
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID = 0
						GROUP BY tsetor.nome,tpreco.nome,tpreco.valor
						ORDER BY tsetor.nome,tpreco.nome";
			}
			else
			{
				sql =
                    @"SELECT tapresentacao.ID as ApresentacaoID,tapresentacao.Horario,tsetor.nome as Setor,tpreco.nome as Preco,tpreco.valor as Valor,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Faturamento
				FROM tEvento (NOLOCK)
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID in (" + eventoID + @")";
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID = 0
						GROUP BY tapresentacao.ID,tapresentacao.horario,tsetor.nome,tpreco.nome,tpreco.valor
						ORDER BY tsetor.nome,tapresentacao.horario,tpreco.nome";
			}
			
						
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaPagante.NewRow();

				if(!agruparApresentacoes)
				{
					linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linhaTabela["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
				}
				linhaTabela["Setor"] = bd.LerString("Setor");
				linhaTabela["Preco"] = bd.LerString("Preco");
				linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
				linhaTabela["Valor"] = bd.LerDecimal("Valor");
				linhaTabela["Faturamento"] = bd.LerDecimal("Faturamento");



				tabelaPagante.Rows.Add(linhaTabela);
			}

			return tabelaPagante;			
		
		}//fim get data Pagante

		//*****Obtem os dados para a tabela de Cortesias do borderô resumo
		public DataTable getDataResumoCortesia(int eventoID,int apresentacaoID,string dataInicial,string dataFinal)
		{
			
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaCortesia = new DataTable("tabelaGetDataCortesia");
			
			//Estrutura da tabela Cortesia
			if(!agruparApresentacoes)
			{
				tabelaCortesia.Columns.Add("ApresentacaoID",typeof(int));
				tabelaCortesia.Columns.Add("Horario",typeof(string));
			}
			tabelaCortesia.Columns.Add("Setor",typeof(string));
			tabelaCortesia.Columns.Add("Preco",typeof(string));
			tabelaCortesia.Columns.Add("Quantidade",typeof(decimal));
			tabelaCortesia.Columns.Add("Valor",typeof(decimal));
			tabelaCortesia.Columns.Add("Cortesia",typeof(string));

			
			

			BD bd = new BD();
			string sql  = string.Empty;
				
			if(agruparApresentacoes)
			{
				sql =
                    @"SELECT tsetor.nome as Setor,tpreco.nome as Preco,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Valor,tCortesia.Nome as Cortesia
				FROM tEvento (NOLOCK)
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				INNER JOIN tCortesia (NOLOCK) on tCortesia.ID = tIngresso.CortesiaID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID in (" + eventoID + @")";
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID > 0
						GROUP BY tsetor.nome,tpreco.nome,tpreco.valor,tCortesia.Nome
						ORDER BY tsetor.nome,tpreco.nome";
			}
			else
			{
				sql =
                    @"SELECT tapresentacao.ID as ApresentacaoID,tapresentacao.Horario,tsetor.nome as Setor,tpreco.nome as Preco,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Valor,tCortesia.Nome as Cortesia
				FROM tEvento
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				INNER JOIN tCortesia (NOLOCK) on tCortesia.ID = tIngresso.CortesiaID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID in (" + eventoID + @")";
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID > 0
						GROUP BY tapresentacao.ID,tapresentacao.horario,tsetor.nome,tpreco.nome,tCortesia.Nome
						ORDER BY tsetor.nome,tapresentacao.horario,tpreco.nome";
			}
						
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaCortesia.NewRow();

				if(!agruparApresentacoes)
				{
					linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linhaTabela["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
				}
				linhaTabela["Setor"] = bd.LerString("Setor");
				linhaTabela["Preco"] = bd.LerString("Preco");
				linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
				linhaTabela["Valor"] = bd.LerDecimal("Valor");
				linhaTabela["Cortesia"] = bd.LerString("Cortesia");

				tabelaCortesia.Rows.Add(linhaTabela);

			}

			return tabelaCortesia;			
		
		}//fim get data Cortesia

		//getdata estatistica por setor

		//*****Obtem os dados para a tabela de Estatística por setor do borderô resumo
		public DataTable getDataResumoEstatisticaPorSetor(int eventoID,int apresentacaoID,string dataInicial,string dataFinal)
		{
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaEstatistica = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
			//Estrutura da tabela Estatistica Por Setor
			tabelaEstatistica.Columns.Add("SetorID",typeof(int));
			tabelaEstatistica.Columns.Add("Setor",typeof(string));
			tabelaEstatistica.Columns.Add("Ingressos",typeof(decimal));
			tabelaEstatistica.Columns.Add("CortesiaID",typeof(int));
			tabelaEstatistica.Columns.Add("Status",typeof(string));
			
			BD bd = new BD();
				
			string sql = @"SELECT tsetor.ID as SetorID,tsetor.nome as Setor,count(tingresso.ID) as Ingressos,tingresso.CortesiaID,tingresso.Status
							FROM tEvento (NOLOCK)
							INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
							INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
							INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
							WHERE tingresso.status in ('A','V','I','E','B','P')
							and tEvento.ID in (" + eventoID + @")";
			if (apresentacaoID > 0) //se for selecionado 1 apresentacao
				sql += @" and apresentacaoID = " + apresentacaoID;
			else
				sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + "'";
					
			sql += @" and tapresentacao.disponivelrelatorio = 'T'
									GROUP BY tsetor.ID,tsetor.nome,tingresso.CortesiaID,tingresso.Status
									ORDER BY tsetor.nome,tingresso.CortesiaID";
						
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaEstatistica.NewRow();

				linhaTabela["SetorID"] = bd.LerInt("SetorID");
				linhaTabela["Setor"] = bd.LerString("Setor");
				linhaTabela["Ingressos"] = bd.LerString("Ingressos");
				linhaTabela["CortesiaID"] = bd.LerInt("CortesiaID");
				linhaTabela["Status"] = bd.LerString("Status");
				

				tabelaEstatistica.Rows.Add(linhaTabela);
			}

			return tabelaEstatistica;			
		
		}//fim get data Estatistica Por Setor 

		public DataTable getDataArrecadacao(int eventoID)
		{
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaDespesas = new DataTable("tabelaGetDataArrecadacao");
			
			//Estrutura da tabela Despesas do Evento
			tabelaDespesas.Columns.Add("Nome",typeof(string));
			tabelaDespesas.Columns.Add("PorValor",typeof(string));
			tabelaDespesas.Columns.Add("Porcentagem",typeof(decimal));
			tabelaDespesas.Columns.Add("ValorMinimo",typeof(decimal));

			BD bd = new BD();

			string sql = "SELECT Nome,PorValor,Porcentagem,ValorMinimo FROM tEventoDespesa (NOLOCK) WHERE EventoID = " + eventoID; 
			
				
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaDespesas.NewRow();

				linhaTabela["Nome"] = bd.LerString("Nome");
				linhaTabela["PorValor"] = bd.LerString("PorValor");
				linhaTabela["Porcentagem"] = bd.LerString("Porcentagem");
				linhaTabela["ValorMinimo"] = bd.LerDecimal("ValorMinimo");
			
				tabelaDespesas.Rows.Add(linhaTabela);
			}
			return tabelaDespesas;
		}

		#endregion

		#region GetDataPorCanal

		public DataTable getDataPorCanal(int eventoID,string canais,int apresentacaoID,string dataInicial,string dataFinal)
		{
			
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaSetorPreco = new DataTable("tabelaGetSetorPreco");
			
			//Estrutura da tabela 

			if(!agruparApresentacoes)
			{
				tabelaSetorPreco.Columns.Add("ApresentacaoID",typeof(int));
				tabelaSetorPreco.Columns.Add("Horario",typeof(string));
			}
			tabelaSetorPreco.Columns.Add("Setor",typeof(string));
			tabelaSetorPreco.Columns.Add("Preco",typeof(string));
			tabelaSetorPreco.Columns.Add("Valor",typeof(decimal));
			tabelaSetorPreco.Columns.Add("Quantidade",typeof(int));
			tabelaSetorPreco.Columns.Add("Faturamento",typeof(decimal));
	

			BD bd = new BD();
			string sql  = string.Empty;
			
			if(agruparApresentacoes)
			{
				sql =
                    @"SELECT tsetor.nome as Setor,tpreco.nome as Preco,tpreco.valor as Valor,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Faturamento
				FROM tEvento (NOLOCK)
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID 
				INNER JOIN tloja (NOLOCK) on tingresso.lojaID = tloja.id 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID = " + eventoID ;
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID = 0 and tLoja.CanalID in (" + canais + @")
						GROUP BY tsetor.nome,tpreco.nome,tpreco.valor
						ORDER BY tsetor.nome,tpreco.nome";
			}
			else
			{
				sql =
                    @"SELECT tapresentacao.ID as ApresentacaoID,tapresentacao.Horario,tsetor.nome as Setor,tpreco.nome as Preco,tpreco.valor as Valor,count(tingresso.ID) as Quantidade,sum(tpreco.valor) as Faturamento
				FROM tEvento (NOLOCK)
				INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
				INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID
				INNER JOIN tloja (NOLOCK) on tingresso.lojaID = tloja.id 
				INNER JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
				INNER JOIN tpreco (NOLOCK) on tpreco.ID = tingresso.precoID
				WHERE (tingresso.status = 'A' or tingresso.status = 'V' or tingresso.status = 'I' or tingresso.status = 'E') 
				and tEvento.ID in (" + eventoID + @")";
				if (apresentacaoID > 0) //se for selecionado 1 apresentacao
					sql += @" and apresentacaoID = " + apresentacaoID;
				else
					sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + @"'";
				
				sql += @" and tapresentacao.disponivelrelatorio = 'T' and tIngresso.cortesiaID = 0 and tLoja.CanalID in (" + canais + @")
						GROUP BY tapresentacao.ID,tapresentacao.horario,tsetor.nome,tpreco.nome,tpreco.valor
						ORDER BY tsetor.nome,tapresentacao.horario,tpreco.nome";
			}
			
						
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaSetorPreco.NewRow();

				if(!agruparApresentacoes)
				{
					linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linhaTabela["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
				}
				linhaTabela["Setor"] = bd.LerString("Setor");
				linhaTabela["Preco"] = bd.LerString("Preco");
				linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
				linhaTabela["Valor"] = bd.LerDecimal("Valor");
				linhaTabela["Faturamento"] = bd.LerDecimal("Faturamento");



				tabelaSetorPreco.Rows.Add(linhaTabela);
			}

			return tabelaSetorPreco;			
		
		}//fim get Por Canal

		//obtem todos os setores de uma apresentacao
		public DataTable getSetores(int eventoID,int apresentacaoID)
		{
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaSetores = new DataTable("tabelaSetores");
			
			//Estrutura da tabela Setores
			tabelaSetores.Columns.Add("ID",typeof(int));
			tabelaSetores.Columns.Add("Nome",typeof(string));
			
			BD bd = new BD();

			string sql = @"
							SELECT DISTINCT tsetor.ID,tsetor.Nome FROM tsetor (NOLOCK)
							INNER JOIN tapresentacaosetor (NOLOCK) on tapresentacaosetor.setorID = tsetor.id
							INNER JOIN tapresentacao (NOLOCK) on tapresentacao.ID = tapresentacaosetor.apresentacaoID
							INNER JOIN tevento (NOLOCK) on tevento.ID = tapresentacao.eventoID
							WHERE tEvento.ID = " + eventoID + " and tapresentacao.disponivelrelatorio = 'T'";
							
			if(apresentacaoID > 0)
				sql += " and tapresentacao.id = " + apresentacaoID;

			bd.Consulta(sql);


			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaSetores.NewRow();

				linhaTabela["ID"] = bd.LerInt("ID");
				linhaTabela["Nome"] = bd.LerString("Nome");

				tabelaSetores.Rows.Add(linhaTabela);
			}

			return tabelaSetores;


		}


		//*****Obtem os dados para a tabela de Estatística por setor do borderô Por Canal
		public DataTable getDataPorCanalEstatisticaPorSetor(int eventoID,string canais,int apresentacaoID,string dataInicial,string dataFinal)
		{
			//DataTable que recebe os dados da consulta ao Banco
			DataTable tabelaEstatistica = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
			//Estrutura da tabela Estatistica Por Setor
			tabelaEstatistica.Columns.Add("SetorID",typeof(int));
			tabelaEstatistica.Columns.Add("Setor",typeof(string));
			tabelaEstatistica.Columns.Add("Ingressos",typeof(decimal));
			tabelaEstatistica.Columns.Add("CortesiaID",typeof(int));
			tabelaEstatistica.Columns.Add("Status",typeof(string));
			
			BD bd = new BD();
				
			string sql = @"SELECT tsetor.ID as SetorID,tsetor.nome as Setor,count(tingresso.ID) as Ingressos,tingresso.CortesiaID,tingresso.Status
							FROM tEvento (NOLOCK)
							INNER JOIN tapresentacao (NOLOCK) on tapresentacao.eventoID = tevento.ID
							INNER JOIN tingresso (NOLOCK) on tapresentacao.ID = tingresso.apresentacaoID
							LEFT JOIN tsetor (NOLOCK) on tsetor.ID = tingresso.setorID
							WHERE tingresso.status in ('A','V','I','E','B','P')
							and tEvento.ID = " + eventoID;
			if (apresentacaoID > 0) //se for selecionado 1 apresentacao
				sql += @" and apresentacaoID = " + apresentacaoID;
			else
				sql += @" and tapresentacao.horario >= '" + dataInicial + @"' and tapresentacao.horario < '" + dataFinal + "'";
									
			sql += @" and tapresentacao.disponivelrelatorio = 'T'
									GROUP BY tsetor.ID,tsetor.nome,tingresso.CortesiaID,tingresso.Status
									ORDER BY tsetor.nome,tingresso.CortesiaID";
						
			bd.Consulta(sql);

			while(bd.Consulta().Read())
			{
				DataRow linhaTabela = tabelaEstatistica.NewRow();

				linhaTabela["SetorID"] = bd.LerInt("SetorID");
				linhaTabela["Setor"] = bd.LerString("Setor");
				linhaTabela["Ingressos"] = bd.LerString("Ingressos");
				linhaTabela["CortesiaID"] = bd.LerInt("CortesiaID");
				linhaTabela["Status"] = bd.LerString("Status");
				

				tabelaEstatistica.Rows.Add(linhaTabela);
			}

			return tabelaEstatistica;			
		
		}//fim get data Estatistica Por Setor Canal



		#endregion


		public DataTable ObterTabelaPorSetorPrecoCanais(int apresentacaoID,string canaisInformados, string dataInicial,string dataFinal) 
		{
			#region Pagantes

			/***Variaveis***/
			string setorAnterior = String.Empty;
			string setorAtual = String.Empty;
			//subtotais
			decimal subTotalQuantidade = 0;
			decimal subTotalFaturamento = 0;
			totalFaturamento = 0;
			decimal totalQuantidade = 0;
			

			IngressoLog ingressoLog = new IngressoLog();
			int eventoID = evento.Control.ID; 

			//Estrutura da tabela Pagante
			DataTable tabelaPagante = new DataTable("tabelaGetDataPagante");

			if(!agruparApresentacoes)
			{
				tabelaPagante.Columns.Add("ApresentacaoID",typeof(int));
				tabelaPagante.Columns.Add("Horario",typeof(string));
			}
			tabelaPagante.Columns.Add("Setor",typeof(string));
			tabelaPagante.Columns.Add("Preco",typeof(string));
			tabelaPagante.Columns.Add("Valor",typeof(decimal));
			tabelaPagante.Columns.Add("Quantidade",typeof(int));
			tabelaPagante.Columns.Add("Faturamento",typeof(decimal));


			

			//Carrega os dados no datatable
			tabelaPagante = getDataPorCanal(eventoID,canaisInformados,apresentacaoID,dataInicial,dataFinal);
			
			//tabela final pagantes
			if(!agruparApresentacoes)
				paganteTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
			else
				paganteTabela = Utilitario.EstruturaBorderoSetorPreco();
			
			

			//tabela final estatistica por setor
			//DataTable tabelaFinalEstatistica = null;

			//tabela final arrecadacao
			DataTable tabelaFinalArrecadacao = Utilitario.EstruturaArrecadacaoDespesa();

			/******Carregando as tabelas finais******/

			for (int i = 0; i <= tabelaPagante.Rows.Count - 1; i++)
			{
				DataRow linha = tabelaPagante.Rows[i];
				setorAtual = (string) linha["Setor"];
			
				if(i > 0)
				{
					if(setorAtual != setorAnterior)
					{
						//subtotal
						DataRow linhaSubtotal = paganteTabela.NewRow();

						linhaSubtotal["Setor"] = "<div style='text-align:left'>SubTotal</div>";
						linhaSubtotal["Valor"] = Utilitario.AplicaFormatoMoeda((subTotalFaturamento/subTotalQuantidade));
						linhaSubtotal["Quantidade"] = subTotalQuantidade;
						linhaSubtotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(subTotalFaturamento);

						paganteTabela.Rows.Add(linhaSubtotal);

						//zera subTotal
						subTotalQuantidade = 0;
						subTotalFaturamento = 0;

						//linha em branco
						DataRow linhaBranco = paganteTabela.NewRow();
						paganteTabela.Rows.Add(linhaBranco);

					}

				}
			
				DataRow linhaPaganteFinal = paganteTabela.NewRow();

				if(!agruparApresentacoes)
				{
					linhaPaganteFinal["Apresentação"] = "<div style='text-align:left'>" + linha["Horario"] + "</div>" ;
				}
				linhaPaganteFinal["Setor"] = "<div style='text-align:left'>" + linha["Setor"] + "</div>";
				linhaPaganteFinal["Preço"] = "<div style='text-align:left'>" + linha["Preco"] + "</div>";
				if(!agruparApresentacoes)
					linhaPaganteFinal["Valor"] = (Convert.ToDecimal(linha["Faturamento"])/Convert.ToDecimal(linha["Quantidade"])).ToString(Utilitario.FormatoMoeda);
				else
					linhaPaganteFinal["Valor"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linha["Valor"]));  
				linhaPaganteFinal["Quantidade"] = linha["Quantidade"];
				linhaPaganteFinal["Faturamento"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linha["Faturamento"]));

				paganteTabela.Rows.Add(linhaPaganteFinal);

				setorAnterior = setorAtual;

				//linha subtotal
				subTotalQuantidade = subTotalQuantidade + Convert.ToDecimal(linhaPaganteFinal["Quantidade"]);
				subTotalFaturamento = subTotalFaturamento + Convert.ToDecimal(linhaPaganteFinal["Faturamento"]);
				//linha Total
				totalQuantidade = totalQuantidade + Convert.ToDecimal(linhaPaganteFinal["Quantidade"]);
				totalFaturamento = totalFaturamento + Convert.ToDecimal(linhaPaganteFinal["Faturamento"]);


				//Linha Subtotal
				if(i == tabelaPagante.Rows.Count - 1)
				{
					DataRow linhaSubtotal = paganteTabela.NewRow();

					linhaSubtotal["Setor"] = "<div style='text-align:left'>Subtotal</div>";
					//para tratar divisão por zero
					if(subTotalQuantidade == 0)
						linhaSubtotal["Valor"] = 0;
					else
						linhaSubtotal["Valor"] = Utilitario.AplicaFormatoMoeda((subTotalFaturamento/subTotalQuantidade));
					linhaSubtotal["Quantidade"] = subTotalQuantidade;
					linhaSubtotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(subTotalFaturamento);

					paganteTabela.Rows.Add(linhaSubtotal);

					//linha em branco
					DataRow linhaBranco = paganteTabela.NewRow();
					paganteTabela.Rows.Add(linhaBranco);

				}
				
			}

			//Linha Total
			DataRow linhaTotal = paganteTabela.NewRow();

			linhaTotal["Setor"] = "<div style='text-align:left'>Total</div>";
			//para tratar divisão por zero
			if(totalQuantidade == 0)
				linhaTotal["Valor"] = 0;
			else
				linhaTotal["Valor"] = Utilitario.AplicaFormatoMoeda((totalFaturamento/totalQuantidade));
			linhaTotal["Quantidade"] = totalQuantidade;
			linhaTotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(totalFaturamento);

			paganteTabela.Rows.Add(linhaTotal);

			return paganteTabela;

			#endregion

		}


		public DataTable ObterTabelaPorSetorPrecoCanaisCortesia(int apresentacaoID,string canaisInformados, string dataInicial,string dataFinal) 
		{
		
			#region cortesias

			/***Variaveis***/
			string setorAnterior = String.Empty;
			string setorAtual = String.Empty;
			//subtotais
			decimal subTotalQuantidade = 0;
			decimal totalQuantidade = 0;

			IngressoLog ingressoLog = new IngressoLog();
			int eventoID = evento.Control.ID;

			//Estrutura da tabela Cortesia
			DataTable tabelaCortesia = new DataTable("tabelaGetDataCortesia");

			if(!agruparApresentacoes)
			{
				tabelaCortesia.Columns.Add("ApresentacaoID",typeof(int));
				tabelaCortesia.Columns.Add("Horario",typeof(string));
			}
			tabelaCortesia.Columns.Add("SetorNome",typeof(string));
			tabelaCortesia.Columns.Add("PrecoNome",typeof(string));
			tabelaCortesia.Columns.Add("Quantidade",typeof(decimal));
			tabelaCortesia.Columns.Add("Valor",typeof(decimal));
			tabelaCortesia.Columns.Add("Cortesia",typeof(string));

			tabelaCortesia = getDataResumoCortesia(eventoID,apresentacaoID,dataInicial,dataFinal);

			//tabela final cortesia
			if(!agruparApresentacoes)
				cortesiaTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
			else
				cortesiaTabela = Utilitario.EstruturaBorderoSetorPreco();

			/******Carregando as tabelas finais******/

			for (int i = 0; i <= tabelaCortesia.Rows.Count - 1; i++)
			{
				DataRow linhaCortesia = tabelaCortesia.Rows[i];
				setorAtual = (string) linhaCortesia["Setor"];
			
				if(i > 0)
				{
					if(setorAtual != setorAnterior)
					{
						//subtotal
						DataRow linhaSubtotalCortesia = cortesiaTabela.NewRow();

						linhaSubtotalCortesia["Setor"] = "<div style='text-align:left'>Subtotal</div>";
						linhaSubtotalCortesia["Valor"] = 0;
						linhaSubtotalCortesia["Quantidade"] = subTotalQuantidade;
						linhaSubtotalCortesia["Faturamento"] = 0;


						cortesiaTabela.Rows.Add(linhaSubtotalCortesia);

						//zera subTotal
						subTotalQuantidade = 0;

						//linha em branco
						DataRow linhaBrancoCortesia = cortesiaTabela.NewRow();
						cortesiaTabela.Rows.Add(linhaBrancoCortesia);

					}

				}

				DataRow linhaCortesiaFinal = cortesiaTabela.NewRow();

				if(!agruparApresentacoes)
				{
					linhaCortesiaFinal["Apresentação"] = "<div style='text-align:left'>" + linhaCortesia["Horario"] + "</div>" ;
				}
				linhaCortesiaFinal["Setor"] = "<div style='text-align:left'>" + linhaCortesia["Setor"] + "</div>";
				linhaCortesiaFinal["Preço"] = "<div style='text-align:left'>" + linhaCortesia["Preco"] + "</div>";
				linhaCortesiaFinal["Valor"] = 0;
				linhaCortesiaFinal["Quantidade"] = linhaCortesia["Quantidade"];
				linhaCortesiaFinal["Faturamento"] = 0;
				linhaCortesiaFinal["Cortesia"] = "<div style='text-align:left'>" + linhaCortesia["Cortesia"] + "</div>";


				cortesiaTabela.Rows.Add(linhaCortesiaFinal);

				setorAnterior = setorAtual;

				//linha subtotal
				subTotalQuantidade = subTotalQuantidade + Convert.ToDecimal(linhaCortesiaFinal["Quantidade"]);
				//linha Total
				totalQuantidade = totalQuantidade + Convert.ToDecimal(linhaCortesiaFinal["Quantidade"]);


				//Linha Subtotal
				if(i == tabelaCortesia.Rows.Count - 1)
				{
					DataRow linhaSubtotalCortesia = cortesiaTabela.NewRow();

					linhaSubtotalCortesia["Setor"] = "<div style='text-align:left'>Subtotal</div>";
					linhaSubtotalCortesia["Valor"] = 0;
					linhaSubtotalCortesia["Quantidade"] = subTotalQuantidade;
					linhaSubtotalCortesia["Faturamento"] = 0;

					cortesiaTabela.Rows.Add(linhaSubtotalCortesia);

					//linha em branco
					DataRow linhaBrancoCortesia = cortesiaTabela.NewRow();
					cortesiaTabela.Rows.Add(linhaBrancoCortesia);

				}
				
			}

			//Linha Total
			DataRow linhaTotalCortesia = cortesiaTabela.NewRow();

			linhaTotalCortesia["Setor"] = "<div style='text-align:left'>Total</div>";
			linhaTotalCortesia["Valor"] = 0;
			linhaTotalCortesia["Quantidade"] = totalQuantidade;
			linhaTotalCortesia["Faturamento"] = 0;

			cortesiaTabela.Rows.Add(linhaTotalCortesia);

			return cortesiaTabela;
	

			#endregion

		}

		//Recebe os dados e trata o datatable



		/*public DataTable ObterTabelaPorSetorPrecoCanais(DataTable setoresTabela, string canaisInformados, TipoPaganteOuCortesia paganteOuCortesia, bool agruparApresentacoes) 
		{
			try{
		#region Dados iniciais
				// Para totalizar
				DataTable canalTabela = new DataTable();
				if (agruparApresentacoes) {
					canalTabela = Utilitario.EstruturaBorderoSetorPreco();
				} else {
					canalTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
				}

				if (setoresTabela.Rows.Count == 0)
					return canalTabela;


				int quantidadeCondicao = 0;
				decimal valorCondicao = 0;
				decimal faturamentoCondicao = 0;
		#endregion 
		#region Para cada setor por Condicao
				// Para cada setor, obter os valores por preço e totalizar
				foreach (DataRow linhaSetor in setoresTabela.Rows) {
					DataTable setorPrecoTabelaCondicao = new DataTable();
					int setorID = Convert.ToInt32(linhaSetor["ID"]);
					if (agruparApresentacoes) {
						setorPrecoTabelaCondicao = LinhasSetorPrecoAgruparApresentacao(setorID, paganteOuCortesia);
					} else {
						setorPrecoTabelaCondicao = LinhasSetorPrecoApresentacao(setorID, paganteOuCortesia);
					}
					if (setorPrecoTabelaCondicao.Rows.Count>0) {
						// Variáveis para totais por setor
						int quantidadeSetorCondicao = 0;
						decimal valorSetorCondicao = 0;
						decimal faturamentoSetorCondicao = 0;
		#region Contabilizar para cada preço do setor
						foreach (DataRow linha in setorPrecoTabelaCondicao.Rows) {
							int quantidade = 0;
							decimal faturamento = 0;
							int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
							if (agruparApresentacoes) {
								// Agrupa por Apresentações
								string precoNome = Convert.ToString(linha["Preço"]);
								QuantidadeFaturamentoPorSetorPrecoNomeCanais(setorID, precoNome, cortesiaID, canaisInformados, out quantidade, out faturamento);
							} else {
								// Por PrecoID o que discrimina por Apresentação e Setor
								int precoID = Convert.ToInt32(linha["PrecoID"]);
								QuantidadeFaturamentoPorApresentacaoSetorPrecoCanais(setorID, precoID, cortesiaID, canaisInformados, out quantidade, out faturamento);
							}
							linha["Quantidade"] = quantidade;
							linha["Faturamento"] = faturamento;
							if (Convert.ToInt32(linha["Quantidade"]) > 0)
								linha["Valor"] = Convert.ToDecimal(linha["Faturamento"]) / (decimal) Convert.ToInt32(linha["Quantidade"]);
							else
								linha["Valor"] = 0;
							quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
							faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
							linha["Valor"] = Convert.ToDecimal(linha["Valor"]).ToString(Utilitario.FormatoMoeda);
							linha["Faturamento"] =  Convert.ToDecimal(linha["Faturamento"]).ToString(Utilitario.FormatoMoeda);
						} // laço do Preco
		#endregion
		#region Total por setor
						DataRow linhaTotalPorSetorCondicao = setorPrecoTabelaCondicao.NewRow();
						linhaTotalPorSetorCondicao["Setor"]= "SubTotal";
						if (quantidadeSetorCondicao>0)
							valorSetorCondicao = faturamentoSetorCondicao/quantidadeSetorCondicao;
						else
							valorSetorCondicao =0;
						linhaTotalPorSetorCondicao["Valor"] = valorSetorCondicao.ToString(Utilitario.FormatoMoeda); // preço médio por setor
						linhaTotalPorSetorCondicao["Quantidade"] = quantidadeSetorCondicao;
						linhaTotalPorSetorCondicao["Faturamento"] = faturamentoSetorCondicao.ToString(Utilitario.FormatoMoeda);
						setorPrecoTabelaCondicao.Rows.Add(linhaTotalPorSetorCondicao);
						// Linha em branco
						if (quantidadeSetorCondicao>0) {
							DataRow linhaBrancoSetor = setorPrecoTabelaCondicao.NewRow();
							linhaBrancoSetor["Quantidade"] = -1;
							setorPrecoTabelaCondicao.Rows.Add(linhaBrancoSetor);
						}
						// Guardando
						//						valorCondicao += valorSetorCondicao;
						quantidadeCondicao += quantidadeSetorCondicao;
						faturamentoCondicao += faturamentoSetorCondicao;
						// Diversos setores por Condicao em um DataTable
						foreach(DataRow umaLinha in setorPrecoTabelaCondicao.Rows) {
							if(Convert.ToInt32(umaLinha["Quantidade"])>0)
								canalTabela.ImportRow(umaLinha);
							if(Convert.ToInt32(umaLinha["Quantidade"])==-1) {
								DataRow linhaSubtotalBranco = canalTabela.NewRow(); 
								canalTabela.Rows.Add(linhaSubtotalBranco);
							}
						}
		#endregion
					}
				}
		#endregion 
		#region Total por Condicao
				if (quantidadeCondicao>0) {
					DataRow linhaTotalCondicao = canalTabela.NewRow();
					linhaTotalCondicao["Setor"]= "Total";
					valorCondicao = faturamentoCondicao/quantidadeCondicao; // preço médio por canal
					linhaTotalCondicao["Valor"] = valorCondicao.ToString(Utilitario.FormatoMoeda);
					linhaTotalCondicao["Quantidade"] = quantidadeCondicao;
					linhaTotalCondicao["Faturamento"] = faturamentoCondicao;
					
					canalTabela.Rows.Add(linhaTotalCondicao);
				}
				return canalTabela;
		#endregion 
			}catch(Exception erro){	
				throw erro;
			}
		} // ObterTabelaPorSetorPrecoCanais */
		/// <summary>
		/// Tem que ser preço nome e não precoID, pois acaba separando por apresentação, e o objetivo é agrupar apresentações
		/// </summary>
		private void QuantidadeFaturamentoPorSetorPrecoNomeCanais(int setorID, string preco, int cortesiaID, string canaisInformados, out int quantidade, out decimal faturamento)
		{
			try
			{
				quantidade=0;
				faturamento=0;
				if (cortesiaID<0 || setorID<=0 || preco =="" || canaisInformados=="" ) 
				{
					return;
				}
				// Vendidos
				int quantidadeVendidos = 0;
				decimal faturamentoVendidos = 0;
				BD bdVendidos = new BD();
				string sqlVendidos = 
					"SELECT        tIngresso.SetorID, tIngressoLog.CortesiaID, COUNT(tIngressoLog.ID) AS Quantidade, tPreco.Nome, SUM(tPreco.Valor) AS Faturamento "+
					"FROM            tIngresso INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngressoLog.Acao = N'"+IngressoLog.VENDER+"') AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) "+
					"GROUP BY tIngresso.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING        (tPreco.Nome = N'"+preco.Trim()+"') AND (tIngresso.SetorID = "+setorID+") AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				bdVendidos.Consulta(sqlVendidos);
				if (bdVendidos.Consulta().Read()) 
				{
					quantidadeVendidos = bdVendidos.LerInt("Quantidade");
					faturamentoVendidos = bdVendidos.LerDecimal("Faturamento");
				}
				bdVendidos.Fechar();
				// Cancelados
				int quantidadeCancelados = 0;
				decimal faturamentoCancelados = 0;
				BD bdCancelados = new BD();
				string sqlCancelados = 
					"SELECT        tIngresso.SetorID, tIngressoLog.CortesiaID, COUNT(tIngressoLog.ID) AS Quantidade, tPreco.Nome, SUM(tPreco.Valor) AS Faturamento "+
					"FROM            tIngresso INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngressoLog.Acao = N'"+IngressoLog.CANCELAR+"') AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) "+
					"GROUP BY tIngresso.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING        (tPreco.Nome = N'"+preco.Trim()+"') AND (tIngresso.SetorID = "+setorID+") AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				bdCancelados.Consulta(sqlCancelados);
				if (bdCancelados.Consulta().Read()) 
				{
					quantidadeCancelados = bdCancelados.LerInt("Quantidade");
					faturamentoCancelados = bdCancelados.LerDecimal("Faturamento");
				}
				bdCancelados.Fechar();
				//
				quantidade = quantidadeVendidos - quantidadeCancelados;
				faturamento = faturamentoVendidos - faturamentoCancelados;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de 

		private void QuantidadeFaturamentoPorApresentacaoSetorPrecoCanais(int setorID, int precoID, int cortesiaID, string canaisInformados, out int quantidade, out decimal faturamento)
		{
			try
			{
				quantidade=0;
				faturamento=0;
				if (cortesiaID<0 || setorID<=0 || precoID <=0 || canaisInformados=="" ) 
				{
					return;
				}
				// Vendidos
				int quantidadeVendidos = 0;
				decimal faturamentoVendidos = 0;
				BD bdVendidos = new BD();
				string sqlVendidos = 
					"SELECT        tIngresso.SetorID, tIngresso.PrecoID, COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.CortesiaID, SUM(tPreco.Valor) AS Faturamento "+
					"FROM            tIngresso INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) AND (tIngressoLog.Acao = N'"+IngressoLog.VENDER+"') "+
					"GROUP BY tIngresso.SetorID, tIngresso.PrecoID, tIngressoLog.CortesiaID "+
					"HAVING        (tIngresso.PrecoID = "+precoID+") AND (tIngressoLog.CortesiaID = "+cortesiaID+") AND (tIngresso.SetorID = "+setorID+") ";
				bdVendidos.Consulta(sqlVendidos);
				if (bdVendidos.Consulta().Read()) 
				{
					quantidadeVendidos = bdVendidos.LerInt("Quantidade");
					faturamentoVendidos = bdVendidos.LerDecimal("Faturamento");
				}
				bdVendidos.Fechar();
				// Cancelados
				int quantidadeCancelados = 0;
				decimal faturamentoCancelados = 0;
				BD bdCancelados = new BD();
				string sqlCancelados = 
					"SELECT        tIngresso.SetorID, tIngresso.PrecoID, COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.CortesiaID, SUM(tPreco.Valor) AS Faturamento "+
					"FROM            tIngresso INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) AND (tIngressoLog.Acao = N'"+IngressoLog.CANCELAR+"') "+
					"GROUP BY tIngresso.SetorID, tIngresso.PrecoID, tIngressoLog.CortesiaID "+
					"HAVING        (tIngresso.PrecoID = "+precoID+") AND (tIngressoLog.CortesiaID = "+cortesiaID+") AND (tIngresso.SetorID = "+setorID+") ";
				bdCancelados.Consulta(sqlCancelados);
				if (bdCancelados.Consulta().Read()) 
				{
					quantidadeCancelados = bdCancelados.LerInt("Quantidade");
					faturamentoCancelados = bdCancelados.LerDecimal("Faturamento");
				}
				bdCancelados.Fechar();
				//
				quantidade = quantidadeVendidos - quantidadeCancelados;
				faturamento = faturamentoVendidos - faturamentoCancelados;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de 

		private int CortesiasPorApresentacaoSetorCanal(string venderCancelar, int setorID, string canaisInformados)
		{
			try
			{
				int qtde=0;
				if (venderCancelar!="") 
				{
					BD bd = new BD();
					string sql = 
						"SELECT        COUNT(tIngressoLog.ID) AS Quantidade, tIngresso.SetorID "+
						"FROM          tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID "+
						"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) AND (tIngressoLog.Acao = N'"+ venderCancelar +"') AND "+
						"(tIngressoLog.CortesiaID > 0) "+
						"GROUP BY tIngresso.SetorID "+
						"HAVING        (tIngresso.SetorID = "+setorID+") ";
					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private int PagantesPorApresentacaoSetorCanal(string venderCancelar, int setorID, string canaisInformados)
		{
			try
			{
				int qtde=0;
				if (venderCancelar!="") 
				{
					BD bd = new BD();
					string sql =  
						"SELECT        COUNT(tIngressoLog.ID) AS Quantidade, tIngresso.SetorID "+
						"FROM          tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID "+
						"WHERE        (tIngressoLog.CanalID IN ("+canaisInformados+")) AND (tIngresso.ApresentacaoID IN ("+apresentacoes+")) AND (tIngressoLog.Acao = N'"+ venderCancelar +"') AND "+
						"(tIngressoLog.CortesiaID = 0) "+
						"GROUP BY tIngresso.SetorID "+
						"HAVING        (tIngresso.SetorID = "+setorID+") ";
					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataTable ObterTabelaPorSetorLotacaoCanais(int apresentacaoID,string canaisInformados, string dataInicial,string dataFinal) 
		{
			try
			{
				#region Estatistica Por Setor

				/***Variaveis***/
				string setorAnterior = string.Empty;
				string setorAtual = string.Empty;
				string setorNome = string.Empty;
				object valorAuxiliar = 0;
				decimal publico = 0;
				decimal pagantes = 0;
				decimal bloqueados = 0;
				decimal cortesias = 0;
				decimal preImpressos = 0;
				int lotacao = 0;
				decimal preImpressosTotal = 0;
				decimal pagantesTotal = 0;
				decimal bloqueadosTotal = 0;
				decimal cortesiasTotal = 0;
				decimal LotacaoTotal = 0;
				object countVerificaSetor = 0;
				int verificaSetor = 0;
				int setorIDSetores = 0;
				publicoTotal = 0;

				
				int eventoID = evento.Control.ID; 

				//Estrutura da tabela Setores
				DataTable tabelaSetores = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
				tabelaSetores.Columns.Add("ID",typeof(int));
				tabelaSetores.Columns.Add("Nome",typeof(string));


				//Estrutura da tabela Estatistica por setor
				DataTable tabelaEstatistica = new DataTable("tabelaGetDataEstatisticaPorSetor");
			
				tabelaEstatistica.Columns.Add("SetorID",typeof(int));
				tabelaEstatistica.Columns.Add("Setor",typeof(string));
				tabelaEstatistica.Columns.Add("Ingressos",typeof(decimal));
				tabelaEstatistica.Columns.Add("CortesiaID",typeof(int));
				tabelaEstatistica.Columns.Add("Status",typeof(string));
				
				tabelaSetores = getSetores(eventoID,apresentacaoID);
				tabelaEstatistica = getDataPorCanalEstatisticaPorSetor(eventoID,canaisInformados,apresentacaoID,dataInicial,dataFinal);
				estatisticaSetorTabela = Utilitario.EstruturaSetorLotacao();
			
				//Carregando a tabela final

				for (int i = 0; i <= tabelaSetores.Rows.Count - 1; i++)
				{
					DataRow linhaSetor = tabelaSetores.Rows[i];
					setorIDSetores = (int)linhaSetor["ID"];
					setorNome = (string) linhaSetor["Nome"];
					bloqueados = 0;
					cortesias = 0;
					pagantes = 0;
					publico = 0;
					preImpressos = 0;
					
					
					

					//if(setorAtual != setorAnterior)
					//{
					//Procura o setor na tabela estatística
					countVerificaSetor = tabelaEstatistica.Compute("COUNT(SetorID)","SetorID = " + setorIDSetores); 
					verificaSetor = countVerificaSetor == DBNull.Value ? 0 : (int)countVerificaSetor;

					if(verificaSetor > 0)
					{
						//Compute

						//lotacao
						lotacao = LotacaoPorSetorApresentacoes(setorIDSetores);

						//qtde total de ingressos bloqueados para o setor 
						valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)","SetorID = '" + setorIDSetores + "' and status = 'B' ");
						bloqueados = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//qtde total de cortesias para o setor 
                        valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)", "SetorID = '" + setorIDSetores + "' and (status = 'A' or status = 'V' or status = 'I' or status = 'E') and CortesiaID > 0 ");
						cortesias = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//qtde total de publico pagante para o setor
                        valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)", "SetorID = '" + setorIDSetores + "' and (status = 'A' or status = 'V' or status = 'I' or status = 'E') and CortesiaID = 0 ");
						pagantes = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//qtde total de pré-impressos para o setor
						valorAuxiliar = tabelaEstatistica.Compute("SUM(Ingressos)","SetorID = '" + setorIDSetores + "' and status = 'P' ");
						preImpressos = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//qtde total de publico para o setor
						publico = pagantes + cortesias;

						//Valores para linha de totais
						publicoTotal = publicoTotal + publico;
						pagantesTotal = pagantesTotal + pagantes;
						bloqueadosTotal = bloqueadosTotal + bloqueados ;
						cortesiasTotal = cortesiasTotal + cortesias ;
						//ocupacaoTotal = ocupacaoTotal + ((100 * publico)/lotacao);
						LotacaoTotal = LotacaoTotal + lotacao;
						preImpressosTotal = preImpressosTotal + preImpressos;

						DataRow linhaEstatisticaFinal = estatisticaSetorTabela.NewRow();

						linhaEstatisticaFinal["Setor"] = "<div style='text-align:left'>" + setorNome + "</div>";
						linhaEstatisticaFinal["Lotação"] = lotacao;
						linhaEstatisticaFinal["Bloqueados"] = bloqueados;
						linhaEstatisticaFinal["Cortesia"] = cortesias;
						linhaEstatisticaFinal["Pagantes"] = pagantes;
						linhaEstatisticaFinal["Público"] = publico;
						linhaEstatisticaFinal["Pré-Impressos"] = preImpressos;
						linhaEstatisticaFinal["Disponível"] = lotacao - (bloqueados + publico + preImpressos);
						if(lotacao == 0)
							linhaEstatisticaFinal["% ocupação"] = 0;

						else
							linhaEstatisticaFinal["% ocupação"] = Math.Round(((100 * publico)/lotacao),1);

						estatisticaSetorTabela.Rows.Add(linhaEstatisticaFinal);
					}
					else // setores que não tem venda
					{
						//lotacao
						lotacao = LotacaoPorSetorApresentacoes(setorIDSetores);
						LotacaoTotal = LotacaoTotal + lotacao;

						DataRow linhaEstatisticaFinal = estatisticaSetorTabela.NewRow();

						linhaEstatisticaFinal["Setor"] = "<div style='text-align:left'>" + setorNome + "</div>";
						linhaEstatisticaFinal["Lotação"] = lotacao;
						linhaEstatisticaFinal["Bloqueados"] = bloqueados;
						linhaEstatisticaFinal["Cortesia"] = cortesias;
						linhaEstatisticaFinal["Pagantes"] = pagantes;
						linhaEstatisticaFinal["Público"] = publico;
						linhaEstatisticaFinal["Pré-Impressos"] = preImpressos;
						linhaEstatisticaFinal["Disponível"] = lotacao - (bloqueados + publico + preImpressos);
						linhaEstatisticaFinal["% ocupação"] = 0;

						estatisticaSetorTabela.Rows.Add(linhaEstatisticaFinal);
					}
			
				}
				//linha de totais
				DataRow linhaBrancoEstatistica = estatisticaSetorTabela.NewRow();
				estatisticaSetorTabela.Rows.Add(linhaBrancoEstatistica);

				DataRow linhaEstatisticaTotal = estatisticaSetorTabela.NewRow();

				linhaEstatisticaTotal["Setor"] = "<div style='text-align:left'>Total</div>";
				linhaEstatisticaTotal["Lotação"] = LotacaoTotal;
				linhaEstatisticaTotal["Bloqueados"] = bloqueadosTotal;
				linhaEstatisticaTotal["Cortesia"] = cortesiasTotal;
				linhaEstatisticaTotal["Pagantes"] = pagantesTotal;
				linhaEstatisticaTotal["Público"] = publicoTotal;
				linhaEstatisticaTotal["Pré-Impressos"] = preImpressosTotal;
				linhaEstatisticaTotal["Disponível"] = LotacaoTotal - (bloqueadosTotal + publicoTotal + preImpressosTotal);
				if(LotacaoTotal == 0)
					linhaEstatisticaTotal["% ocupação"] = 0;

				else
					linhaEstatisticaTotal["% ocupação"] = Math.Round(((100 * publicoTotal)/LotacaoTotal),1);

				estatisticaSetorTabela.Rows.Add(linhaEstatisticaTotal);

				return estatisticaSetorTabela;


				#endregion


			}
			catch(Exception erro)
			{
				throw erro;
			}
		} //

		/*public DataTable ObterTabelaPorSetorLotacaoCanais1(string canaisInformados) {
			try{
				DataTable setorLotacaoTabela = LinhasSetorLotacao();
				int lotacaoSetor =0;
				int bloqueadosSetor =0;
				int cortesiaSetor =0;
				int pagantesSetor =0;
				int publicoSetor =0;
				//				decimal ocupacaoSetor =0;
		#region Contabilizar para cada setor
				// Para cada Setor, obter lotação, quantidade em cortesia, em pagantes, em público e % de ocupação
				foreach (DataRow linha in setorLotacaoTabela.Rows) {
					int setorID = Convert.ToInt32(linha["SetorID"]);
					Setor setor = new Setor();
					setor.Ler(setorID);
					if (setor.LugarMarcado.Valor != Setor.Pista)
						linha["Lotação"] = setor.Quantidade();
					else
						linha["Lotação"] = LotacaoPorSetorApresentacoes(setorID);
					linha["Bloqueados"] = BloqueadosPorSetorApresentacoes(setorID);
					linha["Cortesia"] = 
						CortesiasPorApresentacaoSetorCanal(IngressoLog.VENDER, setorID, canaisInformados) - 
						CortesiasPorApresentacaoSetorCanal(IngressoLog.CANCELAR, setorID, canaisInformados); 
					linha["Pagantes"] = 
						PagantesPorApresentacaoSetorCanal(IngressoLog.VENDER, setorID, canaisInformados) - 
						PagantesPorApresentacaoSetorCanal(IngressoLog.CANCELAR, setorID, canaisInformados); 
					linha["Público"] = Convert.ToInt32(linha["Pagantes"])+Convert.ToInt32(linha["Cortesia"]);
					decimal ocupacaoSetorFormato = (decimal) Convert.ToInt32(linha["Público"])/ (decimal) Convert.ToInt32(linha["Lotação"])*100; 
					linha["% ocupação"] = ocupacaoSetorFormato.ToString(Utilitario.FormatoPorcentagem1Casa);
					// Armazenando para total
					lotacaoSetor += Convert.ToInt32(linha["Lotação"]);
					bloqueadosSetor += Convert.ToInt32(linha["Bloqueados"]);
					cortesiaSetor += Convert.ToInt32(linha["Cortesia"]);
					pagantesSetor += Convert.ToInt32(linha["Pagantes"]);
					publicoSetor += Convert.ToInt32(linha["Público"]);
					//					ocupacaoSetor += Convert.ToDecimal(linha["% ocupação"]);
				} // laço do Setor
		#region Total 
				// Linha em branco
				DataRow linhaBrancoCanal = setorLotacaoTabela.NewRow();
				setorLotacaoTabela.Rows.Add(linhaBrancoCanal);
				//
				DataRow linhaTotal = setorLotacaoTabela.NewRow();
				linhaTotal["Setor"] = "Total";
				linhaTotal["Lotação"] = lotacaoSetor;
				linhaTotal["Bloqueados"] = bloqueadosSetor;
				linhaTotal["Cortesia"] = cortesiaSetor;
				linhaTotal["Pagantes"] = pagantesSetor;
				linhaTotal["Público"] = publicoSetor;
				decimal ocupacaoSetorFormatoTotal = (decimal) Convert.ToInt32(linhaTotal["Público"])/ (decimal) Convert.ToInt32(linhaTotal["Lotação"])*100; 
				linhaTotal["% ocupação"] = ocupacaoSetorFormatoTotal.ToString(Utilitario.FormatoPorcentagem1Casa);
				setorLotacaoTabela.Rows.Add(linhaTotal);
				return setorLotacaoTabela;
		#endregion 
		#endregion
			}catch(Exception erro){	
				throw erro;
			}
		} // */

		/// <summary>		
		/// Obtem a quantidade total de lugares desse setor de lugar nao marcado
		/// </summary>
		/// <returns></returns>
		private int LotacaoPorSetorApresentacoes(int setorID)
		{
			try
			{
				int qtde=0;
				string[] apresentacoesVetor = apresentacoes.Split(',');
				int umaApresentacao ;
				for (int contador =0 ;contador< apresentacoesVetor.Length; contador++) 
				{
					umaApresentacao = Convert.ToInt32(apresentacoesVetor[contador]);
					Setor setor = new Setor();
					setor.Ler(setorID);
					qtde += setor.Quantidade(umaApresentacao);
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>		
		/// Obtem a quantidade total de lugares desse setor de lugar nao marcado
		/// </summary>
		/// <returns></returns>
		private int BloqueadosPorSetorApresentacoes(int setorID)
		{
			try
			{
				int qtde=0;
				string[] apresentacoesVetor = apresentacoes.Split(',');
				int umaApresentacao ;
				for (int contador =0 ;contador< apresentacoesVetor.Length; contador++) 
				{
					umaApresentacao = Convert.ToInt32(apresentacoesVetor[contador]);
					Setor setor = new Setor();
					setor.Ler(setorID);
					qtde += setor.QuantidadeIngressosBloqueados(umaApresentacao);
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Linhas de Formas de Recebimento em função do Evento e Apresentacaoes
		/// </summary>
		public DataTable LinhasBorderoFormaRecebimento(int eventoID,int apresentacaoID, string dataInicial,string dataFinal,string vendaInicial,string vendaFinal)
		{
			try
			{

				string sql;
				BD obterDados = new BD();

			
				DataTable tabela = new DataTable();

				tabela.Columns.Add("Acao", typeof(string));
				tabela.Columns.Add("Valor", typeof(decimal));
				tabela.Columns.Add("Quantidade", typeof(decimal));
				tabela.Columns.Add("CanalID", typeof(int));
				tabela.Columns.Add("FormaPagamentoID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));

				// Obtendo dados através de SQL

                sql = @"SELECT CanalID,FormaPagamentoID,Nome,SUM(Valor) AS Valor, SUM(Quantidade) AS Quantidade FROM (
                        SELECT tLoja.CanalID, 0 AS FormaPagamentoID, 'Cortesia' AS Nome, 0 As Valor,        
                        COUNT(tIngresso.ID) AS Quantidade
                        FROM tIngresso (nolock)         
                        INNER JOIN tVendaBilheteria (nolock) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID 
                        INNER JOIN tApresentacao (nolock) ON tApresentacao.ID = tIngresso.ApresentacaoID         
                        INNER JOIN tCaixa ON tCaixa.ID = tVendaBilheteria.CaixaID
                        INNER JOIN tLoja ON tLoja.ID = tCaixa.LojaID
                        WHERE tIngresso.CortesiaID > 0 AND tApresentacao.DisponivelRelatorio = 'T'";
                        if(apresentacaoID > 0)
                            sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;
                        else
                            sql += " AND tIngresso.EventoID = " + eventoID + " AND (Horario >= '" +  dataInicial + "' AND Horario < '" + dataFinal + "')";

                        sql += @" GROUP BY tLoja.CanalID
                            UNION ALL ";




                sql += @"SELECT tLoja.CanalID AS CanalID,FormaPagamentoID,tFormaPagamento.Nome, 

							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)
								ELSE
									SUM(tPreco.Valor * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
								END
							AS Valor,         
							CASE tIngressoLog.Acao 
								WHEN 'V' THEN
									CASE 
										WHEN (FormaPagamentoID IS NULL) THEN 
											SUM(1)
										ELSE 
											SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / 100)

									END
								ELSE
									CASE 		
										WHEN (FormaPagamentoID IS NULL) THEN SUM(-1)
										ELSE SUM(1 * isnull(dbo.Dividir(CAST(tVendaBilheteriaFormaPagamento.valor as float)*CAST(100 as float),CAST(tVendaBilheteria.ValorTotal as float)), 100) / -100)
								END
							END
							AS Quantidade

                        FROM tVendaBilheteria (NOLOCK)
                        INNER JOIN tCaixa caixas (NOLOCK) ON caixas.ID = tVendaBilheteria.CaixaID
                        INNER JOIN tLoja (NOLOCK) ON tLoja.ID = caixas.LojaID
                        INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
                        LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
                        LEFT JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = FormaPagamentoID
                        INNER JOIN tIngressoLog (NOLOCK) ON  tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID
                        INNER JOIN tPreco (nolock) ON tIngressoLog.precoID = tpreco.ID     
                        INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID
                        INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                        WHERE tIngressoLog.Acao IN ('V','C') AND tIngressoLog.CortesiaID = 0  ";
                
                        if (apresentacaoID > 0)
                            sql += " AND tIngresso.ApresentacaoID = " + apresentacaoID;
                        else
                            sql += " AND tIngresso.EventoID = " + eventoID + " AND (Horario >= '" + dataInicial + "' AND Horario < '" + dataFinal + "')";

                        sql += @"GROUP BY tFormaPagamento.ID, tFormaPagamento.Nome, tLoja.CanalID,tIngressoLog.Acao,FormaPagamentoID,
                                tVendaBilheteriaFormaPagamento.Valor, tIngressoLog.ID
                            ) AS tabela GROUP BY CanalID,FormaPagamentoID,Nome
                            ORDER BY Nome";

                        

				obterDados.Consulta(sql);
				while(obterDados.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["Valor"]= obterDados.LerDecimal("Valor");
					linha["Quantidade"]= obterDados.LerDecimal("Quantidade");
					linha["CanalID"]= obterDados.LerInt("CanalID");
					linha["FormaPagamentoID"]= obterDados.LerInt("FormaPagamentoID");
					linha["Nome"]= obterDados.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				obterDados.Fechar();
				return tabela;
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		}

		/// <summary>
		/// Linhas de Formas de Recebimento em função do Evento e Apresentacaoes
		/// </summary>
		/*	public DataTable LinhasBorderoFormaRecebimento1()
			{
				try
				{
					DataTable tabela = Utilitario.EstruturaBorderoPorFormaRecebimento();
					// Obtendo dados através de SQL
					BD obterDados = new BD();
					string sql =	
						"SELECT DISTINCT tFormaPagamento.Nome, tVendaBilheteriaFormaPagamento.FormaPagamentoID "+
						"FROM            tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
						"tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN "+
						"tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN "+
						"tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN "+
						"tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID "+
						"WHERE        (tIngresso.ApresentacaoID IN ("+ apresentacoes +")) AND (tIngresso.EventoID = "+ evento.Control.ID +") "+
						"ORDER BY tFormaPagamento.Nome ";
					obterDados.Consulta(sql);
					while(obterDados.Consulta().Read())
					{
						DataRow linha = tabela.NewRow();
						linha["FormaRecebimentoID"]= obterDados.LerInt("FormaPagamentoID");
						linha["Formas de Recebimento"]= obterDados.LerString("Nome");
						tabela.Rows.Add(linha);
					}
					obterDados.Fechar();
					return tabela;
				}
				catch(Exception erro)
				{	
					throw erro;
				}
			}*/
		/// <summary>
		/// Usado no Bordero por FormaRecebimento
		/// </summary>
		public void PorFormaRecebimento(int eventoID,int apresentacaoID, string dataInicial,string dataFinal,string vendaInicial,string vendaFinal) 
		{
			try
			{
				#region Inicializando dados e filtrando condição
				IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos ou cancelados
				//Esta tabela tem a mesma estrutura do Grid e será usada para carregá-lo
				DataTable tabelafinal = Utilitario.EstruturaBorderoPorFormaRecebimento();
				
				//Esta tabela terá a estrutura que será retornada pelo select no BD
				DataTable tabela = LinhasBorderoFormaRecebimento(eventoID,apresentacaoID, dataInicial,dataFinal,vendaInicial,vendaFinal); 	
				
				decimal linhaTotaisQtd = 0;
				decimal linhaTotaisValor= 0;
				decimal linhaTotaisQtdCanaisProp = 0;
				decimal linhaTotaisValorCanaisProp = 0;
				decimal linhaTotaisQtdOutrosCanais = 0;
				decimal linhaTotaisValorOutrosCanais = 0;
				DataRow dr;

				string canaisEmpresa=string.Empty;
				string canaisQueVendem=string.Empty;
				string empresaNome=string.Empty;
				DataTable canaisTabela;

				// Informações sobre Empresa deste Evento
				Empresa empresa = new Empresa();
				empresa.Ler(evento.EmpresaID());
				empresaNome = empresa.Nome.Valor;
				#endregion
				#region Para cada forma de pagamento na condição especificada, calcular
				
				//busca os canais próprios
				canaisTabela = empresa.Canais();
				canaisEmpresa="";
				for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
				{
					if (indice ==0) 
						canaisEmpresa = canaisTabela.Rows[indice]["ID"].ToString();
					else
						canaisEmpresa = canaisEmpresa +","+canaisTabela.Rows[indice]["ID"].ToString();
				}

				//busca todos os canais que vendem
				canaisTabela = empresa.CanaisQueVendem(null);
				for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
				{
					if (indice ==0) 
						canaisQueVendem = canaisTabela.Rows[indice]["ID"].ToString();
					else
						canaisQueVendem = canaisQueVendem +","+canaisTabela.Rows[indice]["ID"].ToString();
				}

				object valorAuxiliar = null; //usado para validar o retorno dos select no DataTable

				foreach (DataRow linha in tabela.Rows) 
				{
					
					//verifica se há formas de pagamento repetidas
					int formaPagamentoID = Convert.ToInt32(linha["formaPagamentoID"]);

					DataRow [] linhas = tabelafinal.Select("FormaPagamentoID="+formaPagamentoID);

					if(linhas.Length == 0)
					{	

						dr = tabelafinal.NewRow();

						dr["formaPagamentoID"] = formaPagamentoID;
					
						// ### Totais por linha ###

						//qtde total de ingressos vendido 
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "FormaPagamentoID="+formaPagamentoID);
						decimal qtdTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						//qtde total de ingressos vendido em dinheiro
						valorAuxiliar = tabela.Compute("SUM(Valor)", "FormaPagamentoID="+formaPagamentoID);
						decimal valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


						// ### Canais próprios
						
						// Canais próprios Qtd
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "CanalID in ("+ canaisEmpresa +") AND FormaPagamentoID="+formaPagamentoID);
						decimal qtdCanaisProp = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						// Canais próprios Valor
						valorAuxiliar = tabela.Compute("SUM(Valor)", "CanalID in ("+ canaisEmpresa +") AND FormaPagamentoID="+formaPagamentoID);
						decimal valorCanaisProp = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

						// ### Outros canais ###

						// Outros Canais Qtd
						valorAuxiliar = tabela.Compute("SUM(Quantidade)", "CanalID in ("+ canaisQueVendem +") AND FormaPagamentoID="+formaPagamentoID);
						decimal qtdOutrosCanais = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar - qtdCanaisProp;

						// Outros Canais Valor
						valorAuxiliar = tabela.Compute("SUM(Valor)", "CanalID in ("+ canaisQueVendem +") AND FormaPagamentoID="+formaPagamentoID);
						decimal valorOutrosCanais = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar - valorCanaisProp;

						// descrição da forma de pagamento
						dr["Formas de Recebimento"] = "<div style='text-align:left'>" + tabela.Select("FormaPagamentoID="+formaPagamentoID)[0]["nome"] + "</div>";
						dr["Total Qtd"] = qtdTotal.ToString(Utilitario.FormatoMoeda);
						dr["Total Valor"] = Utilitario.AplicaFormatoMoeda(valorTotal);
						dr["Canais Prop Qtd"] = qtdCanaisProp.ToString(Utilitario.FormatoMoeda);
						dr["Canais Prop Valor"] = Utilitario.AplicaFormatoMoeda(valorCanaisProp);
						dr["Outros Canais Qtd"] = qtdOutrosCanais.ToString(Utilitario.FormatoMoeda);
						dr["Outros Canais Valor"] = Utilitario.AplicaFormatoMoeda(valorOutrosCanais);
						
						//Linha de totais
						linhaTotaisQtd = linhaTotaisQtd + qtdTotal;
						linhaTotaisValor = linhaTotaisValor + valorTotal;
						linhaTotaisQtdCanaisProp = linhaTotaisQtdCanaisProp + qtdCanaisProp;
						linhaTotaisValorCanaisProp = linhaTotaisValorCanaisProp + valorCanaisProp;
						linhaTotaisQtdOutrosCanais = linhaTotaisQtdOutrosCanais + qtdOutrosCanais;
						linhaTotaisValorOutrosCanais = linhaTotaisValorOutrosCanais + valorOutrosCanais;

						tabelafinal.Rows.Add(dr);
						
					}
				
				}// laço do FormaRecebimento
				#endregion 
				#region Valor do Total por coluna
				/*DataRow linhaBranco = tabela.NewRow();
					tabela.Rows.Add(linhaBranco);
					DataRow linhaTotais = tabela.NewRow();
					linhaTotais["Formas de Recebimento"]= "Totais";
					linhaTotais["Total Qtd"]= quantidadeTotalTotais.ToString(Utilitario.FormatoMoeda);
					linhaTotais["Bilheteria Qtd"]= quantidadeBilheteriaTotais.ToString(Utilitario.FormatoMoeda);
					linhaTotais["Call Center Qtd"] = quantidadeoutrosCanaisTotais.ToString(Utilitario.FormatoMoeda);
					linhaTotais["Total Valor"]= valorTotalTotais.ToString(Utilitario.FormatoMoeda);
					linhaTotais["Bilheteria Valor"]= valorBilheteriaTotais.ToString(Utilitario.FormatoMoeda);
					linhaTotais["Call Center Valor"] = valorOutrosCanaisTotais.ToString(Utilitario.FormatoMoeda);			
					//
					tabela.Rows.Add(linhaTotais);
					tabela.Columns["Bilheteria Qtd"].ColumnName = "Canais prop Qtd";
					tabela.Columns["Bilheteria Valor"].ColumnName = "Canais prop Valor";
					tabela.Columns["Call Center Qtd"].ColumnName = "Outros Canais Qtd";
					tabela.Columns["Call Center Valor"].ColumnName = "Outros Canais Valor";

					tabela.Columns.Remove("Internet Qtd");
					tabela.Columns.Remove("Internet Valor");
					tabela.Columns.Remove("PDV Qtd");
					tabela.Columns.Remove("PDV Valor");*/

				#endregion

				dr = tabelafinal.NewRow();					
					
				tabelafinal.Rows.Add(dr); //linha vazia - espaçamento
					
				dr = tabelafinal.NewRow(); //linha com os totais de cada coluna

				dr["Formas de Recebimento"] = "<div style='text-align:left'>Totais</div>";
				dr["Total Qtd"] = (linhaTotaisQtd).ToString(Utilitario.FormatoMoeda);
				dr["Total Valor"] = Utilitario.AplicaFormatoMoeda(linhaTotaisValor);
				dr["Canais Prop Qtd"] = (linhaTotaisQtdCanaisProp).ToString(Utilitario.FormatoMoeda);
				dr["Canais Prop Valor"] = Utilitario.AplicaFormatoMoeda(linhaTotaisValorCanaisProp);
				dr["Outros Canais Qtd"] = (linhaTotaisQtdOutrosCanais).ToString(Utilitario.FormatoMoeda);
				dr["Outros Canais Valor"] = Utilitario.AplicaFormatoMoeda(linhaTotaisValorOutrosCanais);

				tabelafinal.Rows.Add(dr);
				empresaTabela = tabelafinal.Copy();
			}
			
			catch(Exception erro)
			{	
				throw erro;
			}
		
		} // BorderoFormaRecebimento

		/// <summary>
		/// retorna o periodo de vendas para verificar o limite do Por Forma de Recebimento
		/// </summary>
		public int VerificaPeriodo(int apresentacaoID,int eventoID,string dataInicial,string dataFinal, int empresaID,Perfil perfil)
		{
			BD bd = new BD();

			string inicio;
			string fim;

			string sql = @"SELECT MIN(tIngressoLog.timestamp) AS Inicio,MAX(tIngressoLog.timestamp) AS Fim FROM tApresentacao (NOLOCK) 
						INNER JOIN tIngresso (NOLOCK) ON tIngresso.ApresentacaoID = tApresentacao.ID
						INNER JOIN tIngressoLog (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID
						WHERE";
			// Selecionou apresentação?
			if(apresentacaoID > 0)
				sql += " tIngresso.ApresentacaoID = " + apresentacaoID;
			else
			{
				/// Apresentações por período.

				// Selecionou Evento?! Se sim, filtra apenas por ele.
				if(eventoID > 0)
					sql += " tIngresso.EventoID = " + eventoID + " AND" ; 

				//Sempre filtra a data inicial.
				sql += " (Horario >= '" + dataInicial + "' AND Horario < '" + dataFinal + "')";
				
				if(perfil.Control.ID == Perfil.EMPRESA_FINANCEIRO) 
				{
					sql += " AND tIngresso.EmpresaID = "+ empresaID;
				}
				if(perfil.Control.ID == Perfil.LOCAL_FINANCEIRO)
				{
					sql += " AND tIngresso.LocalID = "+ perfil.LocalID;
				}
				
			}

				
			bd.Consulta(sql);

			if(bd.Consulta().Read())
			{
				inicio = bd.LerString("Inicio");
				fim = bd.LerString("Fim");

				if((inicio != null && inicio != "") || (fim != null && fim != ""))
				{
			
					int anoInicio = Convert.ToInt32(inicio.Substring(0,4)); 
					int mesInicio = Convert.ToInt32(inicio.Substring(4,2));
					int diaInicio = Convert.ToInt32(inicio.Substring(6,2));
					int timeInicio = Convert.ToInt32(inicio.Substring(8,6));

					int anoFim = Convert.ToInt32(fim.Substring(0,4)); 
					int mesFim = Convert.ToInt32(fim.Substring(4,2));
					int diaFim = Convert.ToInt32(fim.Substring(6,2));
					int timeFim = Convert.ToInt32(inicio.Substring(8,6));

					DateTime dataInicio = new DateTime(anoInicio,mesInicio,diaInicio);
					DateTime dataFim = new DateTime(anoFim,mesFim,diaFim);

                    dataInicialDateTime = dataInicio;
                    dataFinalDateTime = dataFim;
                    return 1; //Período Válido

				}
				return 3; // Não doram encontradas vendas

			}
			else
				return 3; //Não foram encontradas vendas
		}

		/// <summary>		
		/// Obter as linhas em função de tApresentacao.ID
		/// </summary>
		/// <returns></returns>
		private DataTable LinhasApresentacoes(bool comCortesia)
		{
			string condicaoCortesia="";
			if (comCortesia)
				condicaoCortesia =" AND (tIngresso.CortesiaID >= 0) ";
			else
				condicaoCortesia =" AND (tIngresso.CortesiaID = 0) ";
			try
			{
				DataTable tabela = Utilitario.EstruturaEventoApresentacao();
				BD bd = new BD();
				string sql = 
					@"SELECT DISTINCT tApresentacao.Horario, tApresentacao.EventoID, tApresentacao.ID as ApresentacaoID, tEvento.Nome AS Evento 
					FROM     tEvento 
                         INNER JOIN tApresentacao ON tEvento.ID = tApresentacao.EventoID 
						 INNER JOIN tIngresso ON tApresentacao.ID = tIngresso.ApresentacaoID 

					WHERE  (tApresentacao.ID IN ("+apresentacoes+")) "+condicaoCortesia +
					"ORDER BY tApresentacao.Horario ";
				bd.Consulta(sql);
				while(bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["EventoID"]= bd.LerInt("EventoID");
					linha["Evento"]= bd.LerString("Evento");
					linha["ApresentaçãoID"]= bd.LerInt("ApresentacaoID");
					linha["Apresentação"]= bd.LerStringFormatoDataHora("Horario");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
				return tabela;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}	// fim de LinhasApresentacoes		
		/// <summary>		
		/// Obter as linhas (com setor) em função de tApresentacao.ID
		/// </summary>
		/// <returns></returns>
		private DataTable LinhasApresentacoesSetores(bool comCortesia)
		{
			string condicaoCortesia="";
			if (comCortesia)
				condicaoCortesia =" AND (tIngresso.CortesiaID >= 0) ";
			else
				condicaoCortesia =" AND (tIngresso.CortesiaID = 0) ";
			try
			{
				DataTable tabela = Utilitario.EstruturaEventoApresentacaoSetor();
				BD bd = new BD();
				string sql = 
					@"
						SELECT 
						DISTINCT  
						tApresentacao.Horario + ' - ' + tSetor.Nome AS ApresentcaoSetor, tApresentacao.EventoID, tSetor.ID as SetorID, 
						tApresentacao.ID as ApresentacaoID, tEvento.Nome AS Evento, tApresentacao.Horario, tSetor.Nome AS Setor 
						FROM tEvento 
						INNER JOIN tApresentacao ON tEvento.ID = tApresentacao.EventoID 
						INNER JOIN tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID 
						INNER JOIN tSetor ON tApresentacaoSetor.SetorID = tSetor.ID 
						INNER JOIN tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID 
						WHERE  
						(tApresentacao.ID IN ("+ apresentacoes +@"))  
						" + condicaoCortesia + @"
						ORDER BY tApresentacao.Horario + ' - ' + tSetor.Nome ";
	
				bd.Consulta(sql);
				while(bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["EventoID"]= bd.LerInt("EventoID");
					linha["Evento"]= bd.LerString("Evento");
					linha["ApresentaçãoID"]= bd.LerInt("ApresentacaoID");
					linha["Apresentação"]= bd.LerStringFormatoDataHora("Horario");
					linha["SetorID"]= bd.LerInt("SetorID");
					linha["Setor"]= bd.LerString("Setor");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
				return tabela;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}	// fim de LinhasApresentacoesSetores


		public DataTable FaturamentoConsolidado(
			int usuarioID, int apresentacaoID, int eventoID,
			bool comCortesia, bool comSetor, 
			string dataInicial, string dataFinal
			)
		{


			// String que copiará todos os dados relacionados para uma tabela temporária.
			// Após isso será feito um select agrupando Faturamento, Média e Quantidade.
			StringBuilder sql = new StringBuilder(@"SELECT ");
			sql.Append(usuarioID);
			sql.Append	(	@" as UsuarioID, tEvento.Nome as Evento, ApresentacaoID, Horario , tIngresso.ID As IngressoID, PrecoID INTO #Consol FROM tIngresso 
							INNER JOIN tApresentacao ON tApresentacao.ID = ApresentacaoID
							INNER JOIN tEvento ON tEvento.ID = tIngresso.EventoID
							WHERE 
							DisponivelRelatorio = 'T' AND DisponivelAjuste = 'T' AND DisponivelVenda = 'T'
							AND 
							(Status IN ('A','E','V','I')) "
				);

			
			if (comCortesia)
				sql.Append(" AND CortesiaID = 0");
			else
				sql.Append(" AND CortesiaID > 0");


			if (apresentacaoID <= 0)
			{
				sql.Append(" AND Horario >= '");
				sql.Append(dataInicial);
				sql.Append("' AND Horario < '");
				sql.Append(dataFinal);
				sql.Append("' " );
			}


			if (eventoID > 0)
			{
				sql.Append(" AND tIngresso.EventoID = ");
				sql.Append(eventoID);
			}

			sql.Append(	@" ; SELECT Evento, Horario, Count(IngressoID) as Quantidade, SUM(Valor) as Faturamento, SUM(Valor) / Count(IngressoID) as 'Média' From #Consol
						INNER JOIN tPreco ON tPreco.ID = PrecoID
						WHERE UsuarioID =");
			
			sql.Append(usuarioID);
			sql.Append( @" GROUP BY ApresentacaoID,Horario, Evento
						ORDER BY EVENTO, Horario");

			
			return null;
		}

		public DataTable getData(int eventoID,string sessionID)
		{
			try
			{
				BD bd = new BD();
				string sql;
			
				//DataTable que recebe os dados da consulta ao Banco
				DataTable tabela = new DataTable("tabelaGetData");
			
				//Estrutura da tabela que receberá os dados do select
				tabela.Columns.Add("EventoID",typeof(int));
				tabela.Columns.Add("EventoNome",typeof(string));
				tabela.Columns.Add("ApresentacaoID",typeof(int));
				tabela.Columns.Add("Horario",typeof(DateTime));
				if(faturamentoConsolidadoComSetor)
					tabela.Columns.Add("SetorNome",typeof(string));
				tabela.Columns.Add("Valor",typeof(decimal));
				tabela.Columns.Add("Ingressos",typeof(decimal));

		

				
			
				if(faturamentoConsolidadoComSetor)
				{
					sql =
                        @"SELECT tEvento.ID AS EventoID,tEvento.Nome AS EventoNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,tSetor.Nome AS SetorNome,COUNT(tIngresso.ID) AS Ingressos,SUM(tPreco.Valor) AS Valor
				            FROM tEvento (NOLOCK)
				            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
				            INNER JOIN tIngresso (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID 
				            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
				            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                            INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                            INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
				            INNER JOIN tIRWebEventos on tIRWebEventos.ID = tEvento.ID and tIRWebEventos.SessionID = '" + sessionID + "'";

                    if (canaisIR)
                        sql += " INNER JOIN  tEmpresa ON EmpresaVende = 'T' AND EmpresaPromove = 'F'";


                    sql += @" WHERE (tIngresso.Status = 'A' or tIngresso.Status = 'V' or tIngresso.Status = 'I' OR tIngresso.Status = 'E')";
				
					if(eventoID > 0)
						sql += " AND tEvento.ID = " + eventoID;
				
					if (apresentacaoID > 0) //se for selecionado 1 apresentacao
						sql += @" AND apresentacaoID = " + apresentacaoID;
					else
						sql += @" AND tApresentacao.Horario >= '" + dataInicial + @"' AND tApresentacao.Horario < '" + dataFinal + @"'";
					
					sql += @" AND tApresentacao.DisponivelRelatorio = 'T'";
				
					if(!faturamentoConsolidadoComCortesia) //se não incluir cortesias
						sql += @" AND tIngresso.CortesiaID = 0";

                    if (this.canais != string.Empty)
                        sql += "AND tLoja.CanalID IN (" + canais + ")";


					sql += @" GROUP BY tEvento.ID,tEvento.nome,tApresentacao.ID,tApresentacao.Horario,tSetor.Nome
				ORDER BY tEvento.Nome,tApresentacao.Horario,tSetor.Nome";
				}

				else
				{

                    sql =
                        @"SELECT tEvento.ID AS EventoID,tEvento.Nome AS EventoNome,tApresentacao.ID AS ApresentacaoID,tApresentacao.Horario,COUNT(tIngresso.ID) AS Ingressos,SUM(tPreco.Valor) AS Valor
				            FROM tEvento (NOLOCK)
				            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
				            INNER JOIN tIngresso (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID 
				            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                            INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                            INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
				            INNER JOIN tIRWebEventos ON tIRWebEventos.ID = tEvento.ID AND tIRWebEventos.SessionID = '" + sessionID + "'";

                    if (this.CanaisIR)
                        sql += " INNER JOIN tEmpresa ON EmpresaVende = 'T' AND EmpresaPromove = 'F'";

                    sql += @" WHERE (tIngresso.Status = 'A' OR tIngresso.Status = 'V' OR tIngresso.Status = 'I' OR tIngresso.Status = 'E')";
				
					if(eventoID > 0)
						sql += " AND tEvento.ID = " + eventoID;
				
				
					if(apresentacaoID > 0) //se for selecionado 1 apresentacao
						sql += @" AND ApresentacaoID = " + apresentacaoID;
					else
						sql += @" AND tApresentacao.Horario >= '" + dataInicial + @"' AND tApresentacao.Horario < '" + dataFinal + @"'"; 
					
					sql += @" AND tApresentacao.DisponivelRelatorio = 'T'";
				
					if(!faturamentoConsolidadoComCortesia) //se não incluir cortesias
						sql += @" and tIngresso.CortesiaID = 0";

                    if (this.canais != string.Empty) 
                        sql += "AND tLoja.CanalID IN (" + canais + ")";

					sql += @" GROUP BY tEvento.ID,tEvento.nome,tApresentacao.ID,tApresentacao.Horario
				ORDER BY tEvento.Nome,tApresentacao.Horario";
				}
				

				bd.Consulta(sql);

				while(bd.Consulta().Read())
				{
					DataRow linhaTabela = tabela.NewRow();

					linhaTabela["EventoID"] = bd.LerInt("EventoID");
					linhaTabela["EventoNome"] = bd.LerString("EventoNome");
					linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
					linhaTabela["Horario"] = bd.LerDateTime("Horario");
					if(faturamentoConsolidadoComSetor)
						linhaTabela["SetorNome"] = bd.LerString("SetorNome");
					linhaTabela["Valor"] = bd.LerDecimal("Valor");
					linhaTabela["Ingressos"] = bd.LerDecimal("Ingressos");

					tabela.Rows.Add(linhaTabela);

				}

				return tabela;	
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
		
		}//fim get data
		
		/// <summary>
        /// Limpa Tabela Temporária de Relatório
		/// </summary>
		public void LimpaEventoTemporaria(string sessionID)
		{
			BD bd = new BD();
            try
            {
                //string sql = "CRIA_TEMP_EVENTOS '" + sessionID + "'";
                bd.Executar("" +
                    "DELETE FROM " +
                    "  tIRWebEventos " +
                    "WHERE " +
                    "  SessionID = '" + sessionID + "'");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao limpar tabela temporária: " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

		}

		/// <summary>
		/// Utilizado no Faturamento Consolidado (cálculos)
		/// </summary>
		private DataTable ObterFaturamentoConsolidado(int eventoID,string sessionID) 
		{
			try
			{
				#region Dados Iniciais
				int quantidadeDoRelatorio = 0;
				decimal faturamentoDoRelatorio = 0;
				int quantidadeDoEvento = 0;
				decimal faturamentoDoEvento = 0;
				//DataTable tabelaDeCadaEvento = null;
				DataTable tabelaFinalRelatorio = null;
				int eventoIDAnterior = 0;
				int eventoIDAtual = 0;
				if (faturamentoConsolidadoComSetor) 
				{
					tabelaFinalRelatorio = Utilitario.EstruturaEventoApresentacaoSetor();
				} 
				else 
				{
					tabelaFinalRelatorio = Utilitario.EstruturaEventoApresentacao();
				}
				#endregion 
				#region Para cada Evento obter (IDs) as apresentações no período especificado, respeitando o disponível relatório
//				foreach (string eventoString in eventosVetor) 
//				{
//					#region Obter as apresentações que estão na condição
//					evento.Ler(Convert.ToInt32(eventoString));
//					ArrayList apresentacoesLista;
//					if (apresentacaoID ==-1) 
//					{ // por período
//						apresentacoesLista = evento.Apresentacoes(Apresentacao.Disponibilidade.GerarRelatorio, dataInicial, dataFinal);
//					}
//					else
//					{ // apresentação especificada
//						apresentacoesLista = new ArrayList();
//						apresentacoesLista.Add(apresentacaoID);
//					}
//					apresentacoes = CTLib.Utilitario.ListaInteiroParaString(apresentacoesLista);
//				}

				//carregar o dataTable com os dados do banco - NOVO


				//string strEventos = CTLib.Utilitario.VetorStringParaString(eventosVetor);

				DataTable tabela = getData(eventoID,sessionID);

				DataRow linha = null;


				for (int i = 0; i <= tabela.Rows.Count - 1; i++)
				{

					linha = tabela.Rows[i];
					eventoIDAtual = (int) linha["EventoID"];

					if((eventoIDAtual != eventoIDAnterior) && i > 0)
					{
							
						//linha total do evento
						DataRow linhaTotalEvento = tabelaFinalRelatorio.NewRow();

						linhaTotalEvento["Evento"] = "<div style='text-align:left'>Total</div>";
						linhaTotalEvento["Ingressos"] = quantidadeDoEvento; 
						linhaTotalEvento["Faturamento"] = Utilitario.AplicaFormatoMoeda(faturamentoDoEvento);
						linhaTotalEvento["Preço Médio"] = Utilitario.AplicaFormatoMoeda(Math.Round(faturamentoDoEvento/quantidadeDoEvento,2));

						tabelaFinalRelatorio.Rows.Add(linhaTotalEvento);

						//linha em branco
						DataRow linhaBranco = tabelaFinalRelatorio.NewRow();
						tabelaFinalRelatorio.Rows.Add(linhaBranco);
							
						//zera os totais do evento
						quantidadeDoEvento = 0;
						faturamentoDoEvento = 0;

					}

					DataRow linhaTabelaFinal = tabelaFinalRelatorio.NewRow();
						
					linhaTabelaFinal["EventoID"] = linha["EventoID"];
					linhaTabelaFinal["ApresentaçãoID"] = linha["ApresentacaoID"];
					linhaTabelaFinal["Evento"] = "<div style='text-align:left'>" + linha["EventoNome"] + "</div>";
					linhaTabelaFinal["Apresentação"] =  "<div style='text-align:left'>" + linha["horario"].ToString() + "</div>";
					if(faturamentoConsolidadoComSetor)
						linhaTabelaFinal["Setor"] = "<div style='text-align:left'>" + linha["SetorNome"] + "</div>";
					linhaTabelaFinal["Ingressos"] = linha["Ingressos"];
					linhaTabelaFinal["Faturamento"] = Utilitario.AplicaFormatoMoeda(Convert.ToDecimal(linha["Valor"]));
					linhaTabelaFinal["Preço Médio"] = Utilitario.AplicaFormatoMoeda(Math.Round(Convert.ToDecimal(linha["Valor"])/Convert.ToDecimal(linha["Ingressos"]),2));


					tabelaFinalRelatorio.Rows.Add(linhaTabelaFinal);
						
					//totais dos eventos
					quantidadeDoEvento = quantidadeDoEvento + Convert.ToInt32(linha["Ingressos"]);
					faturamentoDoEvento = faturamentoDoEvento + Convert.ToDecimal(linha["Valor"]);
					//totais gerais
					quantidadeDoRelatorio = quantidadeDoRelatorio + Convert.ToInt32(linha["Ingressos"]);
					faturamentoDoRelatorio = faturamentoDoRelatorio + Convert.ToDecimal(linha["Valor"]);
 

						

					eventoIDAnterior = eventoIDAtual;


					if(i == tabela.Rows.Count - 1)
					{
						//linha total do evento
						DataRow linhaTotalEvento = tabelaFinalRelatorio.NewRow();

						linhaTotalEvento["Evento"] = "<div style='text-align:left'>Total</div>";
						linhaTotalEvento["Ingressos"] = quantidadeDoEvento; 
						linhaTotalEvento["Faturamento"] = Utilitario.AplicaFormatoMoeda(faturamentoDoEvento); 
						linhaTotalEvento["Preço Médio"] = Utilitario.AplicaFormatoMoeda(Math.Round(faturamentoDoEvento/quantidadeDoEvento,2));

						tabelaFinalRelatorio.Rows.Add(linhaTotalEvento);

						//linha em branco
						DataRow linhaBranco = tabelaFinalRelatorio.NewRow();
						tabelaFinalRelatorio.Rows.Add(linhaBranco);

						//linha Total
						DataRow linhaTotal = tabelaFinalRelatorio.NewRow();

						linhaTotal["Evento"] = "<div style='text-align:left'>Total Final</div>";
						linhaTotal["Ingressos"] = quantidadeDoRelatorio; 
						linhaTotal["Faturamento"] = Utilitario.AplicaFormatoMoeda(faturamentoDoRelatorio); 
						linhaTotal["Preço Médio"] = Utilitario.AplicaFormatoMoeda(Math.Round(faturamentoDoRelatorio/quantidadeDoRelatorio,2));

						tabelaFinalRelatorio.Rows.Add(linhaTotal);

					}
						
				}
							
				#endregion 
				/*
					// Se as apresentações estiverem dentro do período especificado
					if (apresentacoes !="") {
						// As linhas podem ser em função da Apresentação ou Apresentação e Setor
						// Parâmetro de entrada são as apresentações selecionada (isso respeita disponível relatório)
						int apresentacaoIDAtual;
						if (faturamentoConsolidadoComSetor) {
							tabelaDeCadaEvento = LinhasApresentacoesSetores(faturamentoConsolidadoComCortesia); 	
				#region Para cada linha obter quantidade e valor do faturamento (com setor)
							foreach (DataRow linhaDoEvento in tabelaDeCadaEvento.Rows) {
								int setorID = Convert.ToInt32(linhaDoEvento["SetorID"]);
								apresentacaoIDAtual = Convert.ToInt32(linhaDoEvento["ApresentaçãoID"]);
								ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
								int apresentacaoSetorID = apresentacaoSetor.ApresentacaoSetorID(apresentacaoIDAtual, setorID);
								//apresentacaoSetor.Ler(apresentacaoSetorID);
								linhaDoEvento["Ingressos"] = 
									apresentacaoSetor.QuantidadePorStatus(Ingresso.Resultado, faturamentoConsolidadoComCortesia, apresentacaoSetorID);
								linhaDoEvento["Faturamento"] = 
									apresentacaoSetor.FaturamentoPorStatus(Ingresso.Resultado, faturamentoConsolidadoComCortesia, apresentacaoSetorID);
								formato = 
									CTLib.Operador.DividirPorZero(
									Convert.ToDecimal(linhaDoEvento["Faturamento"]),
									(decimal) Convert.ToInt32(linhaDoEvento["Ingressos"])
									);
								linhaDoEvento["Preço Médio"] = formato.ToString(Utilitario.FormatoMoeda);
								quantidadeDoEvento += Convert.ToInt32(linhaDoEvento["Ingressos"]);
								faturamentoDoEvento += Convert.ToDecimal(linhaDoEvento["Faturamento"]);
							}
				#endregion
						} else {
							tabelaDeCadaEvento = LinhasApresentacoes(faturamentoConsolidadoComCortesia); 	
				#region Para cada linha obter quantidade e valor do faturamento (sem setor)
							foreach (DataRow linhaDoEvento in tabelaDeCadaEvento.Rows) {
								apresentacaoIDAtual = Convert.ToInt32(linhaDoEvento["ApresentaçãoID"]);
								Apresentacao apresentacao = new Apresentacao();
								//apresentacao.Ler(apresentacaoIDAtual);
								linhaDoEvento["Ingressos"] = 
									apresentacao.QuantidadePorStatus(Ingresso.Resultado, faturamentoConsolidadoComCortesia, apresentacaoIDAtual);
								linhaDoEvento["Faturamento"] = 
									apresentacao.FaturamentoPorStatus(Ingresso.Resultado, faturamentoConsolidadoComCortesia, apresentacaoIDAtual);
								formato = 
									CTLib.Operador.DividirPorZero(
									Convert.ToDecimal(linhaDoEvento["Faturamento"]),
									(decimal) Convert.ToInt32(linhaDoEvento["Ingressos"])
									);
								linhaDoEvento["Preço Médio"] = formato.ToString(Utilitario.FormatoMoeda);
								quantidadeDoEvento += Convert.ToInt32(linhaDoEvento["Ingressos"]);
								faturamentoDoEvento += Convert.ToDecimal(linhaDoEvento["Faturamento"]);
							}
				#endregion
						}
				#region Rodapé do Evento
						DataRow linhaTotalPorEvento = tabelaDeCadaEvento.NewRow();
						linhaTotalPorEvento["Evento"] = "Total";
						linhaTotalPorEvento["Ingressos"] = quantidadeDoEvento;
						linhaTotalPorEvento["Faturamento"] = faturamentoDoEvento;
						formato = 
							CTLib.Operador.DividirPorZero(
							Convert.ToDecimal(linhaTotalPorEvento["Faturamento"]),
							(decimal) Convert.ToInt32(linhaTotalPorEvento["Ingressos"])
							);
						linhaTotalPorEvento["Preço Médio"] = formato.ToString(Utilitario.FormatoMoeda);
						tabelaDeCadaEvento.Rows.Add(linhaTotalPorEvento);
						quantidadeDoRelatorio += quantidadeDoEvento;
						faturamentoDoRelatorio += faturamentoDoEvento;
						quantidadeDoEvento = 0;
						faturamentoDoEvento = 0;
						// Linha em branco
						DataRow linhaBrancoTotalPorEvento = tabelaDeCadaEvento.NewRow();
						linhaBrancoTotalPorEvento["Ingressos"] = -1;
						tabelaDeCadaEvento.Rows.Add(linhaBrancoTotalPorEvento);
				#endregion
						foreach(DataRow umaLinha in tabelaDeCadaEvento.Rows) {
							if (Convert.ToInt32(umaLinha["Ingressos"]) !=0) { // Não inserir a linha se for zero
								if (Convert.ToInt32(umaLinha["Ingressos"]) ==-1) { 
									// Mas tem que inserir a linha em branco
									DataRow linhaBranco = tabelaFinalRelatorio.NewRow();
									tabelaFinalRelatorio.Rows.Add(linhaBranco);
								}else
									tabelaFinalRelatorio.ImportRow(umaLinha);
							}
						}
					}
				}*/
				//#endregion
				/*
				DataRow linhaTotalDoRelatorio = tabelaFinalRelatorio.NewRow();
				linhaTotalDoRelatorio["Evento"] = "Total Final";
				linhaTotalDoRelatorio["Ingressos"] = quantidadeDoRelatorio;
				linhaTotalDoRelatorio["Faturamento"] = faturamentoDoRelatorio;
				formato = 
					CTLib.Operador.DividirPorZero(
					Convert.ToDecimal(linhaTotalDoRelatorio["Faturamento"]),
					(decimal) Convert.ToInt32(linhaTotalDoRelatorio["Ingressos"])
					);
				linhaTotalDoRelatorio["Preço Médio"] = formato.ToString(Utilitario.FormatoMoeda);
				tabelaFinalRelatorio.Rows.Add(linhaTotalDoRelatorio);*/
				return tabelaFinalRelatorio;
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // SetorPrecoTabela
		/// <summary>
		/// Usado no Faturamento Consolidado
		/// </summary>
		public void FaturamentoConsolidado(int eventoID,string sessionID) 
		{
			try
			{

//				string[] eventosVetor;
//				if (eventos!="")
//				{ // todos eventos
//					eventosVetor = eventos.Split(',');
//				} 
//				else 
//				{
//					eventosVetor = new string[1];
//					eventosVetor[0] = evento.Control.ID.ToString();
//				}
				
				faturamentoConsolidadoTabela = ObterFaturamentoConsolidado(eventoID,sessionID);
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // FaturamentoConsolidado
		/// <summary>
		/// Usado no Bordero Resumo
		/// </summary>
		public void ResumoBackup() 
		{
			try
			{
				// Setores conforme o filtro
				DataTable setoresTabela;
				//				string condicaoVendidos="";
				//				string condicaoCancelados="";
				IngressoLog ingressoLog = new IngressoLog(); 
				#region Por pagantes
				// Obter em função de vendidos ou cancelados
				setoresTabela = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Pagante); 	
				decimal valorPagantes = 0;
				if (setoresTabela.Rows.Count>0)
				{
					//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, TipoPaganteOuCortesia.Pagante); 
					//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, TipoPaganteOuCortesia.Pagante);
					paganteTabela = ObterTabelaPorSetorPrecoSemLogID(setoresTabela, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
					if (paganteTabela.Rows.Count >0)
						valorPagantes = (decimal) paganteTabela.Rows[paganteTabela.Rows.Count-1]["Faturamento"];
				}
				#endregion
				#region Por cortesia
				// Obter em função de vendidos ou cancelados
				setoresTabela = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Cortesia); 	
				if (setoresTabela.Rows.Count>0)
				{
					//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, TipoPaganteOuCortesia.Cortesia); 
					//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, TipoPaganteOuCortesia.Cortesia);
					cortesiaTabela = ObterTabelaPorSetorPrecoSemLogID(setoresTabela, TipoPaganteOuCortesia.Cortesia, agruparApresentacoes);
				}
				#endregion
				#region Estatística por Setor
				int totalPublico =0;
				//				if (condicaoVendidos!="" || condicaoCancelados!="") {
				//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, TipoPaganteOuCortesia.Ambos); 
				//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, TipoPaganteOuCortesia.Ambos);
				estatisticaSetorTabela = ObterTabelaPorSetorLotacaoSemLogID(); 
				totalPublico = (int) estatisticaSetorTabela.Rows[estatisticaSetorTabela.Rows.Count-1]["Público"];
				//				}
				#endregion
				#region Arrecadação
				EventoDespesaLista eventoDespesaLista = new EventoDespesaLista();
				eventoDespesaLista.FiltroSQL = "EventoID= " +evento.Control.ID;
				eventoDespesaLista.OrdemSQL = "PorValor";
				eventoDespesaLista.Carregar();
				eventoDespesaLista.Primeiro();
				arrecadacaoDespesaTabela = Utilitario.EstruturaArrecadacaoDespesa();;
				if (eventoDespesaLista.Tamanho>0) 
				{
					decimal formato ;
					decimal somaDespesas =0;
					DataRow arrecadacaoDespesaLinha;
					arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
					arrecadacaoDespesaLinha["Descricao"] = "Arrecadação Bruta";
					arrecadacaoDespesaLinha["Despesa"] = valorPagantes.ToString(Utilitario.FormatoMoeda);
					arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
					for (int contador = 0; contador< eventoDespesaLista.Tamanho ;contador++) 
					{
						arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
						arrecadacaoDespesaLinha["Descricao"] = eventoDespesaLista.EventoDespesa.Nome.Valor;
						arrecadacaoDespesaLinha["Valor/%"] = eventoDespesaLista.EventoDespesa.Porcentagem.Valor;
						if (eventoDespesaLista.EventoDespesa.PorValor.Valor)
						{
							Ingresso oIngresso = new Ingresso();
							int totalImpressao = oIngresso.TotalImpresso(evento.Control.ID);
							formato = eventoDespesaLista.EventoDespesa.Porcentagem.Valor*totalImpressao;
						} 
						else 
						{
							formato = eventoDespesaLista.EventoDespesa.Porcentagem.Valor*valorPagantes/100;
						}
						arrecadacaoDespesaLinha["Despesa"] = formato.ToString(Utilitario.FormatoMoeda);
						somaDespesas += formato;
						arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
						eventoDespesaLista.Proximo();
					}
					arrecadacaoDespesaLinha = arrecadacaoDespesaTabela.NewRow();
					arrecadacaoDespesaLinha["Descricao"] = "Arrecadação Líquida";
					formato = valorPagantes - somaDespesas;
					arrecadacaoDespesaLinha["Despesa"] = formato.ToString(Utilitario.FormatoMoeda);
					arrecadacaoDespesaTabela.Rows.Add(arrecadacaoDespesaLinha);
				}
				#endregion
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // Resumo
		/// <summary>
		/// Usado no Bordero por Canal
		/// </summary>
		//		public void PorCanalNovo() {
		//			try{
		//				// Informações sobre Empresa deste Evento
		//				int empresaID = evento.EmpresaID();
		//				Empresa empresa = new Empresa();
		//				empresa.Ler(empresaID);
		//				string empresaNome =empresa.Nome.Valor; 
		//				// Setores conforme o filtro
		//				DataTable setoresTabela = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Pagante); 	
		//				DataTable canaisTabela;
		//				string condicaoVendidos;
		//				string condicaoCancelados;
		//				string canais;
		//				IngressoLog ingressoLog = new IngressoLog(); 
		//				if (setoresTabela.Rows.Count>0){
		//					#region Por empresa
		//					canais="";
		//					canaisTabela = empresa.Canais();
		//					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) {
		//						if (indice ==0) 
		//							canais = canaisTabela.Rows[indice]["ID"].ToString();
		//						else
		//							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
		//					}			
		//					// Obter em função de vendidos ou cancelados
		////					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
		////					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
		//					empresaTabela = ObterTabelaPorSetorPrecoCanais(setoresTabela, canais, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
		//					#endregion
		//					#region IR (restante)
		//					canais = empresa.CanaisIR();
		//					// Obter em função de vendidos ou cancelados
		//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
		//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
		//					restanteTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
		//					#endregion
		//					#region Total
		//					canais="";
		//					canaisTabela = empresa.CanaisQueVendem(null);
		//					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) {
		//						if (indice ==0) 
		//							canais = canaisTabela.Rows[indice]["ID"].ToString();
		//						else
		//							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
		//					}			
		//					// Obter em função de vendidos ou cancelados
		//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
		//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
		//					totalGeralTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
		//					#endregion
		//					#region Total de Cortesias
		//					// Obter em função de vendidos ou cancelados
		//					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
		//					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
		//					totalCortesiaTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Cortesia, agruparApresentacoes);
		//					#endregion
		//					#region Estatística por Setor
		//					estatisticaSetorTabela = ObterTabelaPorSetorLotacao(condicaoVendidos, condicaoCancelados);
		//					#endregion
		//				}
		//			}catch(Exception erro){	
		//				throw erro;
		//			}
		//		} // PorCanal
		public void PorCanalBackup() 
		{
			try
			{
				// Informações sobre Empresa deste Evento
				int empresaID = evento.EmpresaID();
				Empresa empresa = new Empresa();
				empresa.Ler(empresaID);
				string empresaNome =empresa.Nome.Valor; 
				// Setores conforme o filtro
				DataTable setoresTabela = evento.Setores(null, apresentacoes, TipoPaganteOuCortesia.Pagante); 	
				DataTable canaisTabela;
				string condicaoVendidos;
				string condicaoCancelados;
				string canais;
				IngressoLog ingressoLog = new IngressoLog(); 
				if (setoresTabela.Rows.Count>0)
				{
					#region Por empresa
					canais="";
					canaisTabela = empresa.Canais();
					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
					{
						if (indice ==0) 
							canais = canaisTabela.Rows[indice]["ID"].ToString();
						else
							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
					}			
					// Obter em função de vendidos ou cancelados
					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
					empresaTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
					#endregion
					#region IR (restante)
					canais = empresa.CanaisIR();
					// Obter em função de vendidos ou cancelados
					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
					restanteTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
					#endregion
					#region Total
					canais="";
					canaisTabela = empresa.CanaisQueVendem(null);
					for (int indice =0; indice< canaisTabela.Rows.Count; indice++) 
					{
						if (indice ==0) 
							canais = canaisTabela.Rows[indice]["ID"].ToString();
						else
							canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
					}			
					// Obter em função de vendidos ou cancelados
					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
					totalGeralTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Pagante, agruparApresentacoes);
					#endregion
					#region Total de Cortesias
					// Obter em função de vendidos ou cancelados
					condicaoVendidos= evento.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
					condicaoCancelados= evento.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
					totalCortesiaTabela = ObterTabelaPorSetorPreco(setoresTabela, condicaoVendidos, condicaoCancelados, TipoPaganteOuCortesia.Cortesia, agruparApresentacoes);
					#endregion
					#region Estatística por Setor
					estatisticaSetorTabela = ObterTabelaPorSetorLotacao(condicaoVendidos, condicaoCancelados);
					#endregion
				}
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // PorCanal
		/// <summary>
		/// Obter faturamento de ingressos em função do Setor e Nome do Preco
		/// </summary>
		/// <returns></returns>
		private decimal ValorSetorPrecoNome(int setorID, string preco, int cortesiaID, string condicao)
		{
			try
			{
				decimal valor=0;
				if (cortesiaID<0 || setorID<=0 || preco =="" || condicao=="") 
				{
					return valor;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia;
				if  (cortesiaID==0) 
				{
					condicaoCortesia = " AND (tIngressoLog.CortesiaID = 0) ";
				} 
				else 
				{
					condicaoCortesia = " AND (tIngressoLog.CortesiaID > 0) ";
				}
				string sql = 
					"SELECT        tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome, SUM(tPreco.Valor) AS Faturamento "+
					"FROM            tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.ID IN ("+ condicao +")) "+
					"GROUP BY tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") AND (tPreco.Nome = '"+preco.Trim()+"') " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					valor = bd.LerDecimal("Faturamento");
				}
				bd.Fechar();
				return valor;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de ValorSetorPrecoNome
		private decimal ValorSetorPrecoNomeSemLogID(int setorID, string preco, int cortesiaID, string venderCancelar)
		{
			try
			{
				decimal valor=0;
				if (cortesiaID<0 || setorID<=0 || preco =="" || apresentacoes=="" || venderCancelar=="") 
				{
					return valor;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia;
				if  (cortesiaID==0) 
				{
					condicaoCortesia = " AND (tIngressoLog.CortesiaID = 0) ";
				} 
				else 
				{
					condicaoCortesia = " AND (tIngressoLog.CortesiaID > 0) ";
				}
				string sql = 
					"SELECT    tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome, SUM(tPreco.Valor) AS Faturamento "+
					"FROM     tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.Acao = '"+ venderCancelar +"') "+
					"GROUP BY tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING       (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND  (tApresentacaoSetor.SetorID = "+setorID+") AND (tPreco.Nome = '"+preco.Trim()+"') " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					valor = bd.LerDecimal("Faturamento");
				}
				bd.Fechar();
				return valor;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de ValorSetorPrecoNomeSemLogID
		/// <summary>
		/// Obter quantidade de ingressos em função do Setor e Nome do Preco
		/// Considera somente os vendidos-cancelados
		/// </summary>
		/// <returns></returns>
		//		private int QuantidadeSetorPrecoNomeSemLogIDCanais(int setorID, string preco, int cortesiaID, string venderCancelar, string canaisInformados){
		//			try{
		//				int quantidade=0;
		//				if (cortesiaID<0 || setorID<=0 || preco =="" || apresentacoes=="" || venderCancelar=="" || canaisInformados=="") {
		//					return quantidade;
		//				}
		////				string[] canaisVetor = canaisInformados.Split(',');
		////				foreach (string canalAtual in canaisVetor)
		//				BD bd = new BD();
		//				// Obtendo dados
		//				string condicaoCortesia = " AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
		//				string sql = 
		//					"SELECT   tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, COUNT(tIngressoLog.ID) AS Quantidade, tPreco.Nome "+
		//					"FROM     tApresentacaoSetor INNER JOIN "+
		//					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
		//					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
		//					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
		//					"WHERE   (tIngressoLog.Acao = '"+ venderCancelar +"') "+
		//					"GROUP BY tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
		//					"HAVING       (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND  (tApresentacaoSetor.SetorID = "+setorID+") AND (tPreco.Nome = '"+preco.Trim()+"') " +condicaoCortesia;
		//				bd.Consulta(sql);
		//				// Dados por setor e preco
		//				if (bd.Consulta().Read()) {
		//					quantidade = bd.LerInt("Quantidade");
		//				}
		//				bd.Fechar();
		//				return quantidade;
		//			}catch(Exception erro){
		//				throw erro;
		//			}
		//		} // fim de 
		private int QuantidadeSetorPrecoNomeSemLogID(int setorID, string preco, int cortesiaID, string venderCancelar)
		{
			try
			{
				int quantidade=0;
				if (cortesiaID<0 || setorID<=0 || preco =="" || apresentacoes=="" || venderCancelar=="") 
				{
					return quantidade;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia = " AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				string sql = 
					"SELECT   tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, COUNT(tIngressoLog.ID) AS Quantidade, tPreco.Nome "+
					"FROM     tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.Acao = '"+ venderCancelar +"') "+
					"GROUP BY tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING       (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND  (tApresentacaoSetor.SetorID = "+setorID+") AND (tPreco.Nome = '"+preco.Trim()+"') " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					quantidade = bd.LerInt("Quantidade");
				}
				bd.Fechar();
				return quantidade;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de 
		private int QuantidadeSetorPrecoNome(int setorID, string preco, int cortesiaID, string condicao)
		{
			try
			{
				int quantidade=0;
				if (cortesiaID<0 || setorID<=0 || preco =="" || condicao=="") 
				{
					return quantidade;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia = " AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				string sql = 
					"SELECT   tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, COUNT(tIngressoLog.ID) AS Quantidade, tPreco.Nome "+
					"FROM     tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.ID IN ("+ condicao +")) "+
					"GROUP BY tApresentacaoSetor.SetorID, tIngressoLog.CortesiaID, tPreco.Nome "+
					"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") AND (tPreco.Nome = '"+preco.Trim()+"') " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					quantidade = bd.LerInt("Quantidade");
				}
				bd.Fechar();
				return quantidade;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de 
		/// <summary>
		/// Obter quantidade de ingressos em função do SetorPreco
		/// Ao definir precoID acaba definindo ApresentacaoID implicitamente
		/// Na verdade setorID é desncessário
		/// Considera somente os vendidos-cancelados
		/// </summary>
		/// <returns></returns>
		private int QuantidadeApresentacaoSetorPrecoSemLogID(int setorID, int precoID, int cortesiaID, string apresentacoes, string venderCancelar)
		{
			try
			{
				int quantidade=0;
				if (cortesiaID<0 || setorID<=0 || precoID<=0 || apresentacoes=="" || venderCancelar=="") 
				{
					return quantidade;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia = " AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				string sql = 
					"SELECT    tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngresso.PrecoID, COUNT(tPreco.Nome) AS Quantidade, tIngressoLog.CortesiaID "+
					"FROM         tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.Acao = '"+ venderCancelar +"') "+
					"GROUP BY tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngresso.PrecoID, tIngressoLog.CortesiaID "+
					"HAVING       (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND  (tApresentacaoSetor.SetorID = "+setorID+") AND (tIngresso.PrecoID = "+precoID+") " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					quantidade = bd.LerInt("Quantidade");
				}
				bd.Fechar();
				return quantidade;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de QuantidadeApresentacaoSetorPrecoSemLogID
		private int QuantidadeApresentacaoSetorPreco(int setorID, int precoID, int cortesiaID, string condicao)
		{
			try
			{
				int quantidade=0;
				if (cortesiaID<0 || setorID<=0 || precoID<=0 || condicao=="") 
				{
					return quantidade;
				}
				BD bd = new BD();
				// Obtendo dados
				string condicaoCortesia = " AND (tIngressoLog.CortesiaID = "+ cortesiaID +") ";
				string sql = 
					"SELECT     tApresentacaoSetor.SetorID, tIngresso.PrecoID, COUNT(tPreco.Nome) AS Quantidade, tIngressoLog.CortesiaID "+
					"FROM         tApresentacaoSetor INNER JOIN "+
					"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
					"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
					"tPreco ON tIngressoLog.PrecoID = tPreco.ID "+
					"WHERE   (tIngressoLog.ID IN ("+ condicao +")) "+
					"GROUP BY tApresentacaoSetor.SetorID, tIngresso.PrecoID, tIngressoLog.CortesiaID "+
					"HAVING      (tApresentacaoSetor.SetorID = "+setorID+") AND (tIngresso.PrecoID = "+precoID+") " +condicaoCortesia;
				bd.Consulta(sql);
				// Dados por setor e preco
				if (bd.Consulta().Read()) 
				{
					quantidade = bd.LerInt("Quantidade");
				}
				bd.Fechar();
				return quantidade;
			}
			catch(Exception erro)
			{
				throw erro;
			}
		} // fim de QuantidadeSetorPreco
		/// <summary>
		/// Utilizado no Borderô quando as linhas são compostas por Setor e Preço
		/// </summary>
		/// <param name="setoresTabela"></param>
		/// <param name="condicaoVendidos"></param>
		/// <param name="condicaoCancelados"></param>
		public DataTable ObterTabelaPorSetorPrecoSemLogID(DataTable setoresTabela, TipoPaganteOuCortesia paganteOuCortesia, bool agruparApresentacoes) 
		{
			try
			{
				#region Dados iniciais
				// Para totalizar
				DataTable canalTabela = new DataTable();
				if (agruparApresentacoes) 
				{
					canalTabela = Utilitario.EstruturaBorderoSetorPreco();
				} 
				else 
				{
					canalTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
				}
				int quantidadeCondicao = 0;
				decimal valorCondicao = 0;
				decimal faturamentoCondicao = 0;
				#endregion 
				#region Para cada setor por Condicao
				// Para cada setor, obter os valores por preço e totalizar
				foreach (DataRow linhaSetor in setoresTabela.Rows) 
				{
					DataTable setorPrecoTabelaCondicao = new DataTable();
					int setorID = Convert.ToInt32(linhaSetor["ID"]);
					if (agruparApresentacoes) 
					{
						setorPrecoTabelaCondicao = LinhasSetorPrecoAgruparApresentacao(setorID, paganteOuCortesia);
					} 
					else 
					{
						setorPrecoTabelaCondicao = LinhasSetorPrecoApresentacao(setorID, paganteOuCortesia);
					}
					if (setorPrecoTabelaCondicao.Rows.Count>0) 
					{
						// Variáveis para totais por setor
						int quantidadeSetorCondicao = 0;
						decimal valorSetorCondicao = 0;
						decimal faturamentoSetorCondicao = 0;
						#region Contabilizar para cada preço do setor
						foreach (DataRow linha in setorPrecoTabelaCondicao.Rows) 
						{
							if (agruparApresentacoes) 
							{
								#region Agrupa por Apresentações
								// Terei o preço médio por Tipo de Preço
								string precoNome = Convert.ToString(linha["Preço"]);
								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
								// Calcule qtdade e valor
								linha["Quantidade"] = 
									QuantidadeSetorPrecoNomeSemLogID(setorID, precoNome, cortesiaID, IngressoLog.VENDER) -
									QuantidadeSetorPrecoNomeSemLogID(setorID, precoNome, cortesiaID, IngressoLog.CANCELAR);
								linha["Faturamento"] = 
									ValorSetorPrecoNomeSemLogID(setorID, precoNome, cortesiaID, IngressoLog.VENDER) -
									ValorSetorPrecoNomeSemLogID(setorID, precoNome, cortesiaID, IngressoLog.CANCELAR);
								if (Convert.ToInt32(linha["Quantidade"]) > 0)
									linha["Valor"] = Convert.ToDecimal(linha["Faturamento"]) / (decimal) Convert.ToInt32(linha["Quantidade"]);
								else
									linha["Valor"] = 0;
								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
								#endregion
							} 
							else 
							{
								#region Por PrecoID o que discrimina por Apresentação e Setor
								int precoID = Convert.ToInt32(linha["PrecoID"]);
								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
								// Calcule qtdade e valor
								linha["Quantidade"] = 
									QuantidadeApresentacaoSetorPrecoSemLogID(setorID, precoID, cortesiaID, apresentacoes, IngressoLog.VENDER)-
									QuantidadeApresentacaoSetorPrecoSemLogID(setorID, precoID, cortesiaID, apresentacoes, IngressoLog.CANCELAR);
								Preco preco = new Preco();
								preco.Ler(precoID);
								linha["Valor"] = preco.Valor.Valor;
								linha["Faturamento"] = Convert.ToDecimal(linha["Valor"]) * Convert.ToInt32(linha["Quantidade"]);
								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
								#endregion
							}
							linha["Valor"] = Convert.ToDecimal(linha["Valor"]).ToString(Utilitario.FormatoMoeda);
							linha["Faturamento"] =  Convert.ToDecimal(linha["Faturamento"]).ToString(Utilitario.FormatoMoeda);
						} // laço do Preco
						#endregion
						#region Total por setor
						DataRow linhaTotalPorSetorCondicao = setorPrecoTabelaCondicao.NewRow();
						linhaTotalPorSetorCondicao["Setor"]= "SubTotal";
						if (quantidadeSetorCondicao>0)
							valorSetorCondicao = faturamentoSetorCondicao/quantidadeSetorCondicao;
						else
							valorSetorCondicao =0;
						linhaTotalPorSetorCondicao["Valor"] = valorSetorCondicao.ToString(Utilitario.FormatoMoeda); // preço médio por setor
						linhaTotalPorSetorCondicao["Quantidade"] = quantidadeSetorCondicao;
						linhaTotalPorSetorCondicao["Faturamento"] = faturamentoSetorCondicao.ToString(Utilitario.FormatoMoeda);
						setorPrecoTabelaCondicao.Rows.Add(linhaTotalPorSetorCondicao);
						// Linha em branco
						if (quantidadeSetorCondicao>0) 
						{
							DataRow linhaBrancoSetor = setorPrecoTabelaCondicao.NewRow();
							linhaBrancoSetor["Quantidade"] = -1;
							setorPrecoTabelaCondicao.Rows.Add(linhaBrancoSetor);
						}
						// Guardando
						//						valorCondicao += valorSetorCondicao;
						quantidadeCondicao += quantidadeSetorCondicao;
						faturamentoCondicao += faturamentoSetorCondicao;
						// Diversos setores por Condicao em um DataTable
						foreach(DataRow umaLinha in setorPrecoTabelaCondicao.Rows) 
						{
							if(Convert.ToInt32(umaLinha["Quantidade"])>0)
								canalTabela.ImportRow(umaLinha);
							if(Convert.ToInt32(umaLinha["Quantidade"])==-1) 
							{
								DataRow linhaSubtotalBranco = canalTabela.NewRow(); 
								canalTabela.Rows.Add(linhaSubtotalBranco);
							}
						}
						#endregion
					}
				}
				#endregion 
				#region Total por Condicao
				if (quantidadeCondicao>0) 
				{
					DataRow linhaTotalCondicao = canalTabela.NewRow();
					linhaTotalCondicao["Setor"]= "Total";
					valorCondicao = faturamentoCondicao/quantidadeCondicao; // preço médio por canal
					linhaTotalCondicao["Valor"] = valorCondicao.ToString(Utilitario.FormatoMoeda);
					linhaTotalCondicao["Quantidade"] = quantidadeCondicao;
					linhaTotalCondicao["Faturamento"] = faturamentoCondicao;
					canalTabela.Rows.Add(linhaTotalCondicao);
				}
				return canalTabela;
				#endregion 
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // ObterTabelaPorSetorPrecoSemLogID
		//		public DataTable ObterTabelaPorSetorPrecoCanais(DataTable setoresTabela, string canaisInformados, TipoPaganteOuCortesia paganteOuCortesia, bool agruparApresentacoes) {
		//			try{
		//				string condicaoVendidos="";
		//				string condicaoCancelados="";
		//				#region Dados iniciais
		//				// Para totalizar
		//				DataTable canalTabela = new DataTable();
		//				if (agruparApresentacoes) {
		//					canalTabela = Utilitario.EstruturaBorderoSetorPreco();
		//				} else {
		//					canalTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
		//				}
		//				int quantidadeCondicao = 0;
		//				decimal valorCondicao = 0;
		//				decimal faturamentoCondicao = 0;
		//				#endregion 
		//				#region Para cada setor por Condicao
		//				// Para cada setor, obter os valores por preço e totalizar
		//				foreach (DataRow linhaSetor in setoresTabela.Rows) {
		//					DataTable setorPrecoTabelaCondicao = new DataTable();
		//					int setorID = Convert.ToInt32(linhaSetor["ID"]);
		//					if (agruparApresentacoes) {
		//						setorPrecoTabelaCondicao = LinhasSetorPrecoAgruparApresentacao(setorID, paganteOuCortesia);
		//					} else {
		//						setorPrecoTabelaCondicao = LinhasSetorPrecoApresentacao(setorID, paganteOuCortesia);
		//					}
		//					if (setorPrecoTabelaCondicao.Rows.Count>0) {
		//						// Variáveis para totais por setor
		//						int quantidadeSetorCondicao = 0;
		//						decimal valorSetorCondicao = 0;
		//						decimal faturamentoSetorCondicao = 0;
		//						#region Contabilizar para cada preço do setor
		//						foreach (DataRow linha in setorPrecoTabelaCondicao.Rows) {
		//							if (agruparApresentacoes) {
		//								#region Agrupa por Apresentações
		//								// Terei o preço médio por Tipo de Preço
		//								string precoNome = Convert.ToString(linha["Preço"]);
		//								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
		//								// Calcule qtdade e valor
		//								linha["Quantidade"] = 
		//									QuantidadeSetorPrecoNomeSemLogIDCanais(setorID, precoNome, cortesiaID, IngressoLog.VENDER, canaisInformados) -
		//									QuantidadeSetorPrecoNomeSemLogIDCanais(setorID, precoNome, cortesiaID, IngressoLog.CANCELAR, canaisInformados);
		//								linha["Faturamento"] = 
		//									ValorSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoVendidos) -
		//									ValorSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoCancelados);
		//								if (Convert.ToInt32(linha["Quantidade"]) > 0)
		//									linha["Valor"] = Convert.ToDecimal(linha["Faturamento"]) / (decimal) Convert.ToInt32(linha["Quantidade"]);
		//								else
		//									linha["Valor"] = 0;
		//								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
		//								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
		//								#endregion
		//							} else {
		//								#region Por PrecoID o que discrimina por Apresentação e Setor
		//								int precoID = Convert.ToInt32(linha["PrecoID"]);
		//								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
		//								// Calcule qtdade e valor
		//								linha["Quantidade"] = QuantidadeApresentacaoSetorPreco(setorID, precoID, cortesiaID, condicaoVendidos) -
		//									QuantidadeApresentacaoSetorPreco(setorID, precoID, cortesiaID, condicaoCancelados);
		//								Preco preco = new Preco();
		//								preco.Ler(precoID);
		//								linha["Valor"] = preco.Valor.Valor;
		//								linha["Faturamento"] = Convert.ToDecimal(linha["Valor"]) * Convert.ToInt32(linha["Quantidade"]);
		//								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
		//								//							valorSetorCondicao += Convert.ToDecimal(linha["Valor"]);
		//								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
		//								#endregion
		//							}
		//							linha["Valor"] = Convert.ToDecimal(linha["Valor"]).ToString(Utilitario.FormatoMoeda);
		//							linha["Faturamento"] =  Convert.ToDecimal(linha["Faturamento"]).ToString(Utilitario.FormatoMoeda);
		//						} // laço do Preco
		//						#endregion
		//						#region Total por setor
		//						DataRow linhaTotalPorSetorCondicao = setorPrecoTabelaCondicao.NewRow();
		//						linhaTotalPorSetorCondicao["Setor"]= "SubTotal";
		//						if (quantidadeSetorCondicao>0)
		//							valorSetorCondicao = faturamentoSetorCondicao/quantidadeSetorCondicao;
		//						else
		//							valorSetorCondicao =0;
		//						linhaTotalPorSetorCondicao["Valor"] = valorSetorCondicao.ToString(Utilitario.FormatoMoeda); // preço médio por setor
		//						linhaTotalPorSetorCondicao["Quantidade"] = quantidadeSetorCondicao;
		//						linhaTotalPorSetorCondicao["Faturamento"] = faturamentoSetorCondicao.ToString(Utilitario.FormatoMoeda);
		//						setorPrecoTabelaCondicao.Rows.Add(linhaTotalPorSetorCondicao);
		//						// Linha em branco
		//						if (quantidadeSetorCondicao>0) {
		//							DataRow linhaBrancoSetor = setorPrecoTabelaCondicao.NewRow();
		//							linhaBrancoSetor["Quantidade"] = -1;
		//							setorPrecoTabelaCondicao.Rows.Add(linhaBrancoSetor);
		//						}
		//						// Guardando
		//						//						valorCondicao += valorSetorCondicao;
		//						quantidadeCondicao += quantidadeSetorCondicao;
		//						faturamentoCondicao += faturamentoSetorCondicao;
		//						// Diversos setores por Condicao em um DataTable
		//						foreach(DataRow umaLinha in setorPrecoTabelaCondicao.Rows) {
		//							if(Convert.ToInt32(umaLinha["Quantidade"])>0)
		//								canalTabela.ImportRow(umaLinha);
		//							if(Convert.ToInt32(umaLinha["Quantidade"])==-1) {
		//								DataRow linhaSubtotalBranco = canalTabela.NewRow(); 
		//								canalTabela.Rows.Add(linhaSubtotalBranco);
		//							}
		//						}
		//						#endregion
		//					}
		//				}
		//				#endregion 
		//				#region Total por Condicao
		//				if (quantidadeCondicao>0) {
		//					DataRow linhaTotalCondicao = canalTabela.NewRow();
		//					linhaTotalCondicao["Setor"]= "Total";
		//					valorCondicao = faturamentoCondicao/quantidadeCondicao; // preço médio por canal
		//					linhaTotalCondicao["Valor"] = valorCondicao.ToString(Utilitario.FormatoMoeda);
		//					linhaTotalCondicao["Quantidade"] = quantidadeCondicao;
		//					linhaTotalCondicao["Faturamento"] = faturamentoCondicao;
		//					canalTabela.Rows.Add(linhaTotalCondicao);
		//				}
		//				return canalTabela;
		//				#endregion 
		//			}catch(Exception erro){	
		//				throw erro;
		//			}
		//		} // ObterTabelaPorSetorPrecoCanais
		public DataTable ObterTabelaPorSetorPreco(DataTable setoresTabela, string condicaoVendidos, string condicaoCancelados, TipoPaganteOuCortesia paganteOuCortesia, bool agruparApresentacoes) 
		{
			try
			{
				#region Dados iniciais
				// Para totalizar
				DataTable canalTabela = new DataTable();
				if (agruparApresentacoes) 
				{
					canalTabela = Utilitario.EstruturaBorderoSetorPreco();
				} 
				else 
				{
					canalTabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
				}
				int quantidadeCondicao = 0;
				decimal valorCondicao = 0;
				decimal faturamentoCondicao = 0;
				#endregion 
				#region Para cada setor por Condicao
				// Para cada setor, obter os valores por preço e totalizar
				foreach (DataRow linhaSetor in setoresTabela.Rows) 
				{
					DataTable setorPrecoTabelaCondicao = new DataTable();
					int setorID = Convert.ToInt32(linhaSetor["ID"]);
					if (agruparApresentacoes) 
					{
						setorPrecoTabelaCondicao = LinhasSetorPrecoAgruparApresentacao(setorID, paganteOuCortesia);
					} 
					else 
					{
						setorPrecoTabelaCondicao = LinhasSetorPrecoApresentacao(setorID, paganteOuCortesia);
					}
					if (setorPrecoTabelaCondicao.Rows.Count>0) 
					{
						// Variáveis para totais por setor
						int quantidadeSetorCondicao = 0;
						decimal valorSetorCondicao = 0;
						decimal faturamentoSetorCondicao = 0;
						#region Contabilizar para cada preço do setor
						foreach (DataRow linha in setorPrecoTabelaCondicao.Rows) 
						{
							if (agruparApresentacoes) 
							{
								#region Agrupa por Apresentações
								// Terei o preço médio por Tipo de Preço
								string precoNome = Convert.ToString(linha["Preço"]);
								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
								// Calcule qtdade e valor
								linha["Quantidade"] = 
									QuantidadeSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoVendidos) -
									QuantidadeSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoCancelados);
								linha["Faturamento"] = 
									ValorSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoVendidos) -
									ValorSetorPrecoNome(setorID, precoNome, cortesiaID, condicaoCancelados);
								if (Convert.ToInt32(linha["Quantidade"]) > 0)
									linha["Valor"] = Convert.ToDecimal(linha["Faturamento"]) / (decimal) Convert.ToInt32(linha["Quantidade"]);
								else
									linha["Valor"] = 0;
								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
								#endregion
							} 
							else 
							{
								#region Por PrecoID o que discrimina por Apresentação e Setor
								int precoID = Convert.ToInt32(linha["PrecoID"]);
								int cortesiaID = Convert.ToInt32(linha["CortesiaID"]);
								// Calcule qtdade e valor
								linha["Quantidade"] = QuantidadeApresentacaoSetorPreco(setorID, precoID, cortesiaID, condicaoVendidos) -
									QuantidadeApresentacaoSetorPreco(setorID, precoID, cortesiaID, condicaoCancelados);
								Preco preco = new Preco();
								preco.Ler(precoID);
								linha["Valor"] = preco.Valor.Valor;
								linha["Faturamento"] = Convert.ToDecimal(linha["Valor"]) * Convert.ToInt32(linha["Quantidade"]);
								quantidadeSetorCondicao += Convert.ToInt32(linha["Quantidade"]);
								//							valorSetorCondicao += Convert.ToDecimal(linha["Valor"]);
								faturamentoSetorCondicao += Convert.ToDecimal(linha["Faturamento"]);
								#endregion
							}
							linha["Valor"] = Convert.ToDecimal(linha["Valor"]).ToString(Utilitario.FormatoMoeda);
							linha["Faturamento"] =  Convert.ToDecimal(linha["Faturamento"]).ToString(Utilitario.FormatoMoeda);
						} // laço do Preco
						#endregion
						#region Total por setor
						DataRow linhaTotalPorSetorCondicao = setorPrecoTabelaCondicao.NewRow();
						linhaTotalPorSetorCondicao["Setor"]= "SubTotal";
						if (quantidadeSetorCondicao>0)
							valorSetorCondicao = faturamentoSetorCondicao/quantidadeSetorCondicao;
						else
							valorSetorCondicao =0;
						linhaTotalPorSetorCondicao["Valor"] = valorSetorCondicao.ToString(Utilitario.FormatoMoeda); // preço médio por setor
						linhaTotalPorSetorCondicao["Quantidade"] = quantidadeSetorCondicao;
						linhaTotalPorSetorCondicao["Faturamento"] = faturamentoSetorCondicao.ToString(Utilitario.FormatoMoeda);
						setorPrecoTabelaCondicao.Rows.Add(linhaTotalPorSetorCondicao);
						// Linha em branco
						if (quantidadeSetorCondicao>0) 
						{
							DataRow linhaBrancoSetor = setorPrecoTabelaCondicao.NewRow();
							linhaBrancoSetor["Quantidade"] = -1;
							setorPrecoTabelaCondicao.Rows.Add(linhaBrancoSetor);
						}
						// Guardando
						//						valorCondicao += valorSetorCondicao;
						quantidadeCondicao += quantidadeSetorCondicao;
						faturamentoCondicao += faturamentoSetorCondicao;
						// Diversos setores por Condicao em um DataTable
						foreach(DataRow umaLinha in setorPrecoTabelaCondicao.Rows) 
						{
							if(Convert.ToInt32(umaLinha["Quantidade"])>0)
								canalTabela.ImportRow(umaLinha);
							if(Convert.ToInt32(umaLinha["Quantidade"])==-1) 
							{
								DataRow linhaSubtotalBranco = canalTabela.NewRow(); 
								canalTabela.Rows.Add(linhaSubtotalBranco);
							}
						}
						#endregion
					}
				}
				#endregion 
				#region Total por Condicao
				if (quantidadeCondicao>0) 
				{
					DataRow linhaTotalCondicao = canalTabela.NewRow();
					linhaTotalCondicao["Setor"]= "Total";
					valorCondicao = faturamentoCondicao/quantidadeCondicao; // preço médio por canal
					linhaTotalCondicao["Valor"] = valorCondicao.ToString(Utilitario.FormatoMoeda);
					linhaTotalCondicao["Quantidade"] = quantidadeCondicao;
					linhaTotalCondicao["Faturamento"] = faturamentoCondicao;
					canalTabela.Rows.Add(linhaTotalCondicao);
				}
				return canalTabela;
				#endregion 
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // SetorPrecoTabela

		//		/// <summary>
		//		/// Linhas formadas por Setor e Preço usado no Borderô
		//		/// Com apresentações agrupadas, valor representa preço médio
		//		/// </summary>
		//		private DataTable LinhasSetorPrecoAgruparApresentacao(int setorID, TipoPaganteOuCortesia paganteCondicao){
		//			try{
		//				string cortesiaCondicao ="";
		//				switch (paganteCondicao){
		//					case TipoPaganteOuCortesia.Pagante:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0) ";
		//						break;
		//					case TipoPaganteOuCortesia.Cortesia:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0) ";
		//						break;
		//					case TipoPaganteOuCortesia.Ambos:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0) ";
		//						break;
		//				}
		//				DataTable tabela = Utilitario.EstruturaBorderoSetorPreco();
		//				// Obtendo dados através de SQL
		//				BD obterDados = new BD();
		//				string sql =
		//					"SELECT DISTINCT  "+
		//													"tSetor.Nome + tPreco.Nome + tCortesia.Nome AS Unico, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tApresentacaoSetor.SetorID, tCortesia.Nome AS Cortesia,  "+
		//													"tIngressoLog.CortesiaID "+
		//					"FROM            tEvento INNER JOIN "+
		//													"tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN "+
		//													"tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN "+
		//													"tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN "+
		//													"tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN "+
		//													"tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN "+
		//													"tPreco ON tIngressoLog.PrecoID = tPreco.ID LEFT OUTER JOIN "+
		//													"tCortesia ON tIngressoLog.CortesiaID = tCortesia.ID "+
		////										"WHERE        (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND (tApresentacaoSetor.SetorID = "+setorID+") "+ cortesiaCondicao +" "+
		//										"WHERE        ("+Caracteres.ConverterINemOR("tApresentacaoSetor.ApresentacaoID", apresentacoes)+") AND (tApresentacaoSetor.SetorID = "+setorID+") "+ cortesiaCondicao +" "+					"ORDER BY tSetor.Nome, tPreco.Nome ";
		//				obterDados.Consulta(sql);
		//				while(obterDados.Consulta().Read()){
		//					DataRow linha = tabela.NewRow();
		//					linha["SetorID"]= obterDados.LerInt("SetorID");
		//					linha["CortesiaID"]= obterDados.LerInt("CortesiaID");
		//					linha["Setor"]= obterDados.LerString("Setor");
		//					linha["Preço"]= obterDados.LerString("Preco");
		//					linha["Cortesia"]= obterDados.LerString("Cortesia");
		//					tabela.Rows.Add(linha);
		//				}
		//				obterDados.Fechar();
		//				return tabela;
		//			}catch(Exception erro){	
		//				throw erro;
		//			}
		//		}
		//		/// <summary>
		//		/// Linhas formadas por Setor e Preço usado no Borderô
		//		/// </summary>
		//		private DataTable LinhasSetorPrecoApresentacao(int setorID, TipoPaganteOuCortesia paganteCondicao){
		//			try{
		//				string cortesiaCondicao ="";
		//				switch (paganteCondicao){
		//					case TipoPaganteOuCortesia.Pagante:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0) ";
		//						break;
		//					case TipoPaganteOuCortesia.Cortesia:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0) ";
		//						break;
		//					case TipoPaganteOuCortesia.Ambos:
		//						cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0) ";
		//						break;
		//				}
		//				DataTable tabela = Utilitario.EstruturaBorderoApresentacaoSetorPreco();
		//				// Obtendo dados através de SQL
		//				BD obterDados = new BD();
		//				string sql =
		//					"SELECT DISTINCT "+
		//                      "tSetor.Nome + tPreco.Nome + tCortesia.Nome AS Unico, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tApresentacaoSetor.SetorID,  "+
		//                      "tPreco.ID AS PrecoID, tCortesia.Nome AS Cortesia, tIngressoLog.CortesiaID, tApresentacaoSetor.ApresentacaoID, tApresentacao.Horario "+
		//					"FROM         tCortesia RIGHT OUTER JOIN "+
		//                      "tEvento INNER JOIN "+
		//                      "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN "+
		//                      "tIngressoLog INNER JOIN "+
		//                      "tSetor INNER JOIN "+
		//                      "tApresentacaoSetor ON tSetor.ID = tApresentacaoSetor.SetorID INNER JOIN "+
		//                      "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
		//                      "tPreco ON tIngressoLog.PrecoID = tPreco.ID ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID ON  "+
		//                      "tCortesia.ID = tIngressoLog.CortesiaID "+
		//					"WHERE     (tApresentacaoSetor.ApresentacaoID IN ("+apresentacoes+")) AND (tApresentacaoSetor.SetorID = "+setorID+") "+ cortesiaCondicao +" "+
		//					"ORDER BY tSetor.Nome, tPreco.Nome ";
		//				obterDados.Consulta(sql);
		//				while(obterDados.Consulta().Read()){
		//					DataRow linha = tabela.NewRow();
		//					linha["SetorID"]= obterDados.LerInt("SetorID");
		//					linha["PrecoID"]= obterDados.LerInt("PrecoID");
		//					linha["CortesiaID"]= obterDados.LerInt("CortesiaID");
		//					linha["Apresentação"]= obterDados.LerStringFormatoSemanaDataHora("Horario");
		//					linha["Setor"]= obterDados.LerString("Setor");
		//					linha["Preço"]= obterDados.LerString("Preco");
		//					linha["Cortesia"]= obterDados.LerString("Cortesia");
		//					tabela.Rows.Add(linha);
		//				}
		//				obterDados.Fechar();
		//				return tabela;
		//			}catch(Exception erro){	
		//				throw erro;
		//			}
		//		}
		/// <summary>		
		/// Obter os setores com lotação de certos eventos
		/// </summary>
		/// <returns></returns>
		private DataTable LinhasSetorLotacao()
		{
			try
			{
				DataTable tabela = Utilitario.EstruturaSetorLotacao();
				BD bd = new BD();
				string sql = "SELECT DISTINCT s.ID, s.Nome FROM tApresentacaoSetor as tas,tSetor as s "+
					"WHERE tas.SetorID=s.ID AND tas.ApresentacaoID in ("+apresentacoes+") "+
					"ORDER BY s.Nome";
				bd.Consulta(sql);
				while(bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["SetorID"]= bd.LerInt("ID");
					linha["Setor"]= bd.LerString("Nome");
					tabela.Rows.Add(linha);
				}
				bd.Fechar();
				return tabela;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}	// fim de LinhasSetorLotacao
		/// <summary>		
		/// Obtem a quantidade total de cortesias nesse setor 
		/// </summary>
		/// <returns></returns>
		private int CortesiasSemPorIngressoLogID(string venderCancelar, int setorID)
		{
			try
			{
				int qtde=0;
				if (venderCancelar!="") 
				{
					BD bd = new BD();
					string sql = 
						"SELECT   COUNT(tIngressoLog.ID) AS Quantidade, tApresentacaoSetor.SetorID "+
						"FROM     tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
						"tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID "+
						"WHERE (tIngressoLog.Acao = '"+ venderCancelar +"') AND (tIngressoLog.CortesiaID > 0) "+
						"GROUP BY tApresentacaoSetor.SetorID, tApresentacaoSetor.ApresentacaoID "+
						"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") AND (tApresentacaoSetor.ApresentacaoID IN("+apresentacoes+")) ";

					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");;
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		private int CortesiasPorIngressoLogID(string condicao, int setorID)
		{
			try
			{
				int qtde=0;
				if (condicao!="") 
				{
					BD bd = new BD();
					string sql = 
						"SELECT        COUNT(tIngressoLog.ID) AS Quantidade, tApresentacaoSetor.SetorID "+
						"FROM            tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
						"tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID "+
						"WHERE (tIngressoLog.ID IN ("+ condicao +")) AND (tIngressoLog.CortesiaID > 0) "+
						"GROUP BY tApresentacaoSetor.SetorID "+
						"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") ";
					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");;
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>		
		/// Obtem a quantidade total de pagantes nesse setor 
		/// Os pagantes são vendidos sem incluir as cortesias
		/// </summary>
		/// <returns></returns>
		private int PagantesSemPorIngressoLogID(string venderCancelar, int setorID)
		{
			try
			{
				int qtde=0;
				if (venderCancelar!="") 
				{
					BD bd = new BD();
					string sql = 
						"SELECT  COUNT(tIngressoLog.ID) AS Quantidade, tApresentacaoSetor.SetorID "+
						"FROM    tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
						"tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID "+
						"WHERE (tIngressoLog.Acao = '"+ venderCancelar +"') AND (tIngressoLog.CortesiaID = 0) "+
						"GROUP BY tApresentacaoSetor.SetorID, tApresentacaoSetor.ApresentacaoID "+
						"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") AND (tApresentacaoSetor.ApresentacaoID IN("+apresentacoes+")) ";
					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		private int PagantesPorIngressoLogID(string condicao, int setorID)
		{
			try
			{
				int qtde=0;
				if (condicao!="") 
				{
					BD bd = new BD();
					string sql = 
						"SELECT  COUNT(tIngressoLog.ID) AS Quantidade, tApresentacaoSetor.SetorID "+
						"FROM    tIngressoLog INNER JOIN "+
						"tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN "+
						"tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID "+
						"WHERE (tIngressoLog.ID IN ("+ condicao +")) AND (tIngressoLog.CortesiaID = 0) "+
						"GROUP BY tApresentacaoSetor.SetorID "+
						"HAVING        (tApresentacaoSetor.SetorID = "+setorID+") ";
					bd.Consulta(sql);
					if (bd.Consulta().Read())
					{
						qtde = bd.LerInt("Quantidade");
					}
					bd.Fechar();
				}
				return qtde;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// Utilizado no Borderô quando as linhas são compostas por Setor e Preço
		/// </summary>
		/// <param name="setoresTabela"></param>
		/// <param name="condicaoVendidos"></param>
		/// <param name="condicaoCancelados"></param>
		public DataTable ObterTabelaPorSetorLotacaoSemLogID() 
		{
			try
			{
				DataTable setorLotacaoTabela = LinhasSetorLotacao();
				int lotacaoSetor =0;
				int bloqueadosSetor =0;
				int cortesiaSetor =0;
				int pagantesSetor =0;
				int publicoSetor =0;
				//				decimal ocupacaoSetor =0;
				#region Contabilizar para cada setor
				// Para cada Setor, obter lotação, quantidade em cortesia, em pagantes, em público e % de ocupação
				foreach (DataRow linha in setorLotacaoTabela.Rows) 
				{
					int setorID = Convert.ToInt32(linha["SetorID"]);
					Setor setor = new Setor();
					setor.Ler(setorID);
					if (setor.LugarMarcado.Valor != Setor.Pista)
						linha["Lotação"] = setor.Quantidade();
					else
						linha["Lotação"] = LotacaoPorSetorApresentacoes(setorID);
					linha["Bloqueados"] = BloqueadosPorSetorApresentacoes(setorID);
					linha["Cortesia"] = 
						CortesiasSemPorIngressoLogID(IngressoLog.VENDER, setorID) - 
						CortesiasSemPorIngressoLogID(IngressoLog.CANCELAR, setorID); 
					linha["Pagantes"] = 
						PagantesSemPorIngressoLogID(IngressoLog.VENDER, setorID) - 
						PagantesSemPorIngressoLogID(IngressoLog.CANCELAR, setorID); 
					linha["Público"] = Convert.ToInt32(linha["Pagantes"])+Convert.ToInt32(linha["Cortesia"]);
					decimal ocupacaoSetorFormato = (decimal) Convert.ToInt32(linha["Público"])/ (decimal) Convert.ToInt32(linha["Lotação"])*100; 
					linha["% ocupação"] = ocupacaoSetorFormato.ToString(Utilitario.FormatoPorcentagem1Casa);
					// Armazenando para total
					lotacaoSetor += Convert.ToInt32(linha["Lotação"]);
					bloqueadosSetor += Convert.ToInt32(linha["Bloqueados"]);
					cortesiaSetor += Convert.ToInt32(linha["Cortesia"]);
					pagantesSetor += Convert.ToInt32(linha["Pagantes"]);
					publicoSetor += Convert.ToInt32(linha["Público"]);
					//					ocupacaoSetor += Convert.ToDecimal(linha["% ocupação"]);
				} // laço do Setor
				#region Total 
				// Linha em branco
				DataRow linhaBrancoCanal = setorLotacaoTabela.NewRow();
				setorLotacaoTabela.Rows.Add(linhaBrancoCanal);
				//
				DataRow linhaTotal = setorLotacaoTabela.NewRow();
				linhaTotal["Setor"] = "Total";
				linhaTotal["Lotação"] = lotacaoSetor;
				linhaTotal["Bloqueados"] = bloqueadosSetor;
				linhaTotal["Cortesia"] = cortesiaSetor;
				linhaTotal["Pagantes"] = pagantesSetor;
				linhaTotal["Público"] = publicoSetor;
				decimal ocupacaoSetorFormatoTotal = (decimal) Convert.ToInt32(linhaTotal["Público"])/ (decimal) Convert.ToInt32(linhaTotal["Lotação"])*100; 
				linhaTotal["% ocupação"] = ocupacaoSetorFormatoTotal.ToString(Utilitario.FormatoPorcentagem1Casa);
				setorLotacaoTabela.Rows.Add(linhaTotal);
				return setorLotacaoTabela;
				#endregion 
				#endregion
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // 

		public DataTable ObterTabelaPorSetorLotacao(string condicaoVendidos, string condicaoCancelados) 
		{
			try
			{
				DataTable setorLotacaoTabela = LinhasSetorLotacao();
				int lotacaoSetor =0;
				int bloqueadosSetor =0;
				int cortesiaSetor =0;
				int pagantesSetor =0;
				int publicoSetor =0;
				//				decimal ocupacaoSetor =0;
				#region Contabilizar para cada setor
				// Para cada Setor, obter lotação, quantidade em cortesia, em pagantes, em público e % de ocupação
				foreach (DataRow linha in setorLotacaoTabela.Rows) 
				{
					int setorID = Convert.ToInt32(linha["SetorID"]);
					Setor setor = new Setor();
					setor.Ler(setorID);
					if (setor.LugarMarcado.Valor != Setor.Pista)
						linha["Lotação"] = setor.Quantidade();
					else
						linha["Lotação"] = LotacaoPorSetorApresentacoes(setorID);
					linha["Bloqueados"] = BloqueadosPorSetorApresentacoes(setorID);
					linha["Cortesia"] = 
						CortesiasPorIngressoLogID(condicaoVendidos, setorID) - 
						CortesiasPorIngressoLogID(condicaoCancelados, setorID); 
					linha["Pagantes"] = 
						PagantesPorIngressoLogID(condicaoVendidos, setorID) - 
						PagantesPorIngressoLogID(condicaoCancelados, setorID); 
					linha["Público"] = Convert.ToInt32(linha["Pagantes"])+Convert.ToInt32(linha["Cortesia"]);
					decimal ocupacaoSetorFormato = (decimal) Convert.ToInt32(linha["Público"])/ (decimal) Convert.ToInt32(linha["Lotação"])*100; 
					linha["% ocupação"] = ocupacaoSetorFormato.ToString(Utilitario.FormatoPorcentagem1Casa);
					// Armazenando para total
					lotacaoSetor += Convert.ToInt32(linha["Lotação"]);
					bloqueadosSetor += Convert.ToInt32(linha["Bloqueados"]);
					cortesiaSetor += Convert.ToInt32(linha["Cortesia"]);
					pagantesSetor += Convert.ToInt32(linha["Pagantes"]);
					publicoSetor += Convert.ToInt32(linha["Público"]);
					//					ocupacaoSetor += Convert.ToDecimal(linha["% ocupação"]);
				} // laço do Setor
				#region Total 
				// Linha em branco
				DataRow linhaBrancoCanal = setorLotacaoTabela.NewRow();
				setorLotacaoTabela.Rows.Add(linhaBrancoCanal);
				//
				DataRow linhaTotal = setorLotacaoTabela.NewRow();
				linhaTotal["Setor"] = "Total";
				linhaTotal["Lotação"] = lotacaoSetor;
				linhaTotal["Bloqueados"] = bloqueadosSetor;
				linhaTotal["Cortesia"] = cortesiaSetor;
				linhaTotal["Pagantes"] = pagantesSetor;
				linhaTotal["Público"] = publicoSetor;
				decimal ocupacaoSetorFormatoTotal = (decimal) Convert.ToInt32(linhaTotal["Público"])/ (decimal) Convert.ToInt32(linhaTotal["Lotação"])*100; 
				linhaTotal["% ocupação"] = ocupacaoSetorFormatoTotal.ToString(Utilitario.FormatoPorcentagem1Casa);
				setorLotacaoTabela.Rows.Add(linhaTotal);
				return setorLotacaoTabela;
				#endregion 
				#endregion
			}
			catch(Exception erro)
			{	
				throw erro;
			}
		} // 

        /// <summary>
        /// Cruza os Códigos de Barra do Banco com a Lista de Códigos, para identificar presença
        /// </summary>
        /// <param name="apresentacaoID">Apresentação</param>
        /// <param name="SessionID">ID da Sessão</param>
        /// <param name="lstCodigos">Lista de Códigos</param>
        /// <returns></returns>
        public DataTable RelatorioPresenca(int apresentacaoID, string SessionID, List<string> lstCodigos)
        {
            DataTable dtRelatorioPresenca = new DataTable("RelatorioPresenca");
            dtRelatorioPresenca.Columns.Add(new DataColumn("Senha", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("CodigoBarra", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("ClienteNome", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("SetorNome", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("CodigoIngresso", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("Status", typeof(string)));
            dtRelatorioPresenca.Columns.Add(new DataColumn("Presenca", typeof(string)));

            DataTable dtCodigos = new DataTable("Codigos");
            dtCodigos.Columns.Add(new DataColumn("SessionID", typeof(string)));
            dtCodigos.Columns.Add(new DataColumn("Codigo", typeof(string)));
            

            BD bd = new BD();
            DataRow oDataRow;
            try
            {
                // Alimenta o DataTable com os Códigos
                foreach (string Codigo in lstCodigos)
                {
                    oDataRow = dtCodigos.NewRow();
                    oDataRow["Codigo"] = Codigo;
                    oDataRow["SessionID"] = SessionID;

                    dtCodigos.Rows.Add(oDataRow);
                }

                // Alimenta a Tabela Temporária
                System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy((System.Data.SqlClient.SqlConnection) bd.Cnn , System.Data.SqlClient.SqlBulkCopyOptions.TableLock | System.Data.SqlClient.SqlBulkCopyOptions.FireTriggers | System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = "RelatorioPresencaCodigos";
                bulkCopy.WriteToServer(dtCodigos);


                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tVendaBilheteria.Senha, " +
                    "   tIngresso.CodigoBarra, " +
                    "   tCliente.Nome AS ClienteNome, " +
                    "   tSetor.Nome AS SetorNome, " +
                    "   tIngresso.Codigo AS CodigoIngresso, " +
                    "   tPreco.Valor, " +
                    "   tIngresso.Status, " +
                    "   CASE WHEN RelatorioPresencaCodigos.Codigo IS NULL THEN 'Não' " + 
                    "   ELSE 'Sim' " + 
                    "   END AS Presenca " + 
                    "FROM tIngresso " +
                    "INNER JOIN tSetor ON tIngresso.SetorID = tSetor.ID " +
                    "LEFT OUTER JOIN tPreco ON tIngresso.PrecoID = tPreco.ID " +
                    "LEFT OUTER JOIN tCliente ON tIngresso.ClienteID = tCliente.ID " +
                    "LEFT OUTER JOIN tVendaBilheteria ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "LEFT OUTER JOIN RelatorioPresencaCodigos ON RelatorioPresencaCodigos.Codigo = tIngresso.CodigoBarra AND RelatorioPresencaCodigos.SessionID = '" + SessionID + "' " +
                    "WHERE " +
                    "   (tIngresso.ApresentacaoID = " + apresentacaoID + ")"))
                {
                    while (oDataReader.Read())
                    {
                        oDataRow = dtRelatorioPresenca.NewRow();
                        oDataRow["Senha"] = bd.LerString("Senha");
                        oDataRow["CodigoBarra"] = bd.LerString("CodigoBarra");
                        oDataRow["ClienteNome"] = bd.LerString("ClienteNome");
                        oDataRow["SetorNome"] = bd.LerString("SetorNome");
                        oDataRow["CodigoIngresso"] = bd.LerString("CodigoIngresso");
                        oDataRow["Valor"] = bd.LerDecimal("Valor");
                        oDataRow["Status"] = bd.LerString("Status");
                        oDataRow["Presenca"] = bd.LerString("Presenca");                        
                        dtRelatorioPresenca.Rows.Add(oDataRow);
                    }
                }

                bd.Fechar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtCodigos.Dispose();
                dtCodigos = null;

                bd.Fechar();
            }

            return dtRelatorioPresenca;
        }

        /// <summary>
        /// Limpa o ambiente do relatório, tanto banco como arquivos
        /// </summary>
        /// <param name="SessionID">ID da Sessão</param>
        /// <param name="lstCodigos">Caminho do Arquivo</param>
        /// <returns></returns>
        public void RelatorioPresencaLimpar(string SessionID, string arquivo)
        {
            BD bd = new BD();
            try
            {
                bd.Executar("" +
                    "DELETE FROM " +
                    "   RelatorioPresencaCodigos " +
                    "WHERE " +
                    "   SessionID = '" + SessionID + "'");

                if (System.IO.File.Exists(arquivo))
                    System.IO.File.Delete(arquivo);
            }
            catch
            {
            }
            finally
            {
                bd.Fechar();
            }


        }


	} // fim de classe
} // fim de namespace
