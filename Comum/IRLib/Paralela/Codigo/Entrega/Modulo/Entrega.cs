/**************************************************
* Arquivo: Entrega.cs
* Gerado: 06/01/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.Codigo.ModuloLogistica;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace IRLib.Paralela
{

    public class Entrega : Entrega_B
    {
        public const string NORMAL = "N";
        public const string AGENDADA = "A";
        public const string RETIRADA = "R";
        public const string RETIRADABILHETERIA = "B";

        public Entrega() { }

        public Entrega(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaEntrega> Listar()
        {
            try
            {
                List<EstruturaEntrega> lista = new List<EstruturaEntrega>();
                string sql = @"SELECT
								ID, 
								Nome, 
								PrazoEntrega, 
								Disponivel, 
								ProcedimentoEntrega, 
								EnviaAlerta, 
								Padrao, 
								PermitirImpressaoInternet, 
								Tipo, 
								Ativo, 
								DiasTriagem
								FROM tEntrega ORDER BY Nome";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEntrega
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        Disponivel = bd.LerBoolean("Disponivel"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        EnviaAlerta = bd.LerBoolean("EnviaAlerta"),
                        Padrao = bd.LerBoolean("Padrao"),
                        PermitirImpressaoInternet = bd.LerBoolean("PermitirImpressaoInternet"),
                        Tipo = Convert.ToChar(bd.LerString("Tipo")),
                        Ativo = bd.LerBoolean("Ativo"),
                        DiasTriagem = bd.LerInt("DiasTriagem")
                    });
                }
                return lista;
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


        public List<EstruturaIDNome> ListarIDNome(bool primeiro)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                if (primeiro)
                {
                    lista.Add(new EstruturaIDNome
                    {
                        ID = 0,
                        Nome = "Selecione ... ",

                    });

                }

                string sql = @"SELECT
								ID, 
								Nome
								FROM tEntrega";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),

                    });
                }
                return lista;
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

        public string LerNome(int ID)
        {

            try
            {

                string nome = "-";
                string sql = @"SELECT  
								e.Nome
								FROM tEntrega e (nolock)
								where id = " + ID;

                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    nome = bd.LerString("Nome");
                }

                return nome;

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

        public EstruturaEntrega CarregarEstruturaPeloControleID(int EntregaControleID)
        {

            EstruturaEntrega retorno = new EstruturaEntrega();
            string sql = @"SELECT
								e.ID, 
								e.Nome, 
								PrazoEntrega, 
								Disponivel, 
								e.ProcedimentoEntrega, 
								EnviaAlerta, 
								Padrao, 
								PermitirImpressaoInternet, 
								Tipo, 
								Ativo, 
								e.DiasTriagem,
								ISNULL(ep.Nome,'-') as Periodo
								FROM tEntrega e
								INNER JOIN tEntregaControle ec ON ec.EntregaID = e.ID
								LEFT JOIN tEntregaPeriodo ep ON ec.PeriodoID = ep.ID
								WHERE ec.ID = " + EntregaControleID;

            bd.Consulta(sql);
            if (bd.Consulta().Read())
            {
                retorno.ID = bd.LerInt("ID");
                retorno.Nome = bd.LerString("Nome");
                retorno.PrazoEntrega = bd.LerInt("PrazoEntrega");
                retorno.Disponivel = bd.LerBoolean("Disponivel");
                retorno.ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega");
                retorno.EnviaAlerta = bd.LerBoolean("EnviaAlerta");
                retorno.Padrao = bd.LerBoolean("Padrao");
                retorno.PermitirImpressaoInternet = bd.LerBoolean("PermitirImpressaoInternet");
                retorno.Tipo = Convert.ToChar(bd.LerString("Tipo"));
                retorno.Ativo = bd.LerBoolean("Ativo");
                retorno.DiasTriagem = bd.LerInt("DiasTriagem");
                retorno.Periodo = bd.LerString("Periodo");

            }
            return retorno;

        }

        public bool Agendada(int ID)
        {
            try
            {


                string sql = @"SELECT  
								e.Tipo
								FROM tEntrega e (nolock)
								where id = " + ID;

                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    if (bd.LerString("Tipo") == AGENDADA)
                        return true;
                }

                return false;

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

        public List<DataAgenda> ListarDatas(DateTime dataExibicao, List<int> listaApresentacao, int EntregaID, List<int> listaEventoID, string CEP)
        {
            try
            {
                List<DataAgenda> lista = new List<DataAgenda>();
                List<PeridoAgenda> listaPeriodos = ListarPeriodos(EntregaID, CEP, listaEventoID);
                DateTime dataDia = dataExibicao.AddDays(-1);
                Apresentacao oApresentacao = new Apresentacao();
                Entrega oEntrega = new Entrega();
                EntregaAgenda oEA = new EntregaAgenda();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);
                int diasTriagemMaior = oEntrega.MaiorDiasTriagem(EntregaID, listaEventoID) + 1;

                //Adiciona todos os dias do mês
                for (int i = 0; i < 31; i++)
                {
                    dataDia = dataDia.AddDays(1).Date;
                    if (dataExibicao.Month != dataDia.Month)
                        break;

                    lista.Add(new DataAgenda
                    {
                        Dia = dataDia,
                        DiasTriagem = diasTriagemMaior,
                        DataDoEvento = dataEventoMaisProximo
                    });

                }

                string sql =
                    string.Format(@"SELECT 
							DISTINCT 
								tc.ID AS EntregaControleID, 
								ta.Data, ta.QtdAgendada AS QuantidadeAgendada, 
								tc.QuantidadeEntregas, 
								p.ID AS PeriodoID,
								p.Nome AS Periodo, 
								p.HoraInicial, p.HoraFinal,
								COUNT(DISTINCT eec.EventoID) AS Eventos,
								CASE WHEN
										ta.ID IS NOT NULL AND f.ID IS NOT NULL
											THEN 'T'
											ELSE 'F'
										END AS Feriado
							FROM tEntrega e (NOLOCK)
							INNER JOIN tEntregaControle tc (NOLOCK) ON e.ID = tc.EntregaID
							INNER JOIN tEventoEntregaControle  eec (NOLOCK) ON tc.ID = eec.EntregaControleID
							INNER JOIN tEntregaPeriodo p (NOLOCK) ON p.ID = tc.PeriodoID
							INNER JOIN tEntregaAreaCep tac (NOLOCK)  ON tc.EntregaAreaID = tac.EntregaAreaID
							INNER JOIN tEntregaAgenda ta (NOLOCK) ON ta.EntregaControleID = tc.ID
							LEFT JOIN tFeriado f (NOLOCK) ON SUBSTRING(f.Data, 0, 9) COLLATE Latin1_General_CI_AI = ta.Data COLLATE Latin1_General_CI_AI
							WHERE e.ID = {0} AND eec.EventoID IN ({1}) AND 
								((ta.Data >= '{2}' AND ta.Data <= '{3}') OR (ta.ID IS NULL))
								AND CEPInicial <= '{4}' AND CEPFinal >= '{4}'
							GROUP BY tc.ID, ta.Data, ta.QtdAgendada, QuantidadeEntregas, p.ID, p.Nome, ta.ID, f.ID, p.HoraInicial, p.HoraFinal
							HAVING COUNT(DISTINCT eec.EventoID) = {5}
							ORDER BY ta.Data, p.Nome",
                            EntregaID, Utilitario.ArrayToString(listaEventoID.ToArray()), "20110701", "20110801", CEP, listaEventoID.Count);


                //Aqui estão os dias da agenda que já existem, precisa buscar os dias que ainda não foram criados!!
                List<PeridoAgenda> agendaAux = new List<PeridoAgenda>();
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                    agendaAux.Add(new PeridoAgenda()
                    {
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Disponivel = bd.LerInt("QuantidadeAgendada") < bd.LerInt("QuantidadeEntregas"),
                        Periodo = bd.LerString("Periodo"),
                        PeriodoID = bd.LerInt("PeriodoID"),
                        Data = bd.LerDateTime("Data"),
                    });


                List<PeridoAgenda> naoEncontradas = new List<PeridoAgenda>();
                foreach (DataAgenda item in lista)
                {
                    foreach (PeridoAgenda periodo in listaPeriodos)
                    {
                        //Estes são dias anteriores, nem busca a informação
                        if (item.Dia <= DateTime.Now.AddDays(diasTriagemMaior).Date || item.Dia > dataEventoMaisProximo.Date)
                        {
                            item.ListaPeriodo.Add(new PeridoAgenda()
                            {
                                Periodo = periodo.Periodo,
                                PeriodoID = periodo.PeriodoID,
                                EntregaControleID = periodo.EntregaControleID,
                                Disponivel = false,
                                HoraFinal = periodo.HoraFinal,
                                HoraInicial = periodo.HoraInicial
                            });
                        }
                        //Este dia e periodo ainda não foi adicionado ao banco de dados, buscar para ter a base de dia possível
                        else
                        {
                            var encontrado = agendaAux.Where(c => c.PeriodoID == periodo.PeriodoID && c.Data.Date == item.Dia.Date).FirstOrDefault();
                            if (encontrado == null)
                                naoEncontradas.Add(new PeridoAgenda()
                                {
                                    Data = item.Dia,
                                    Periodo = periodo.Periodo,
                                    PeriodoID = periodo.PeriodoID,
                                    EntregaControleID = periodo.EntregaControleID,
                                    Disponivel = false,
                                    HoraFinal = periodo.HoraFinal,
                                    HoraInicial = periodo.HoraInicial
                                });
                            //Já encontrou, só adiciona no objeto
                            else
                                item.ListaPeriodo.Add(encontrado);
                        }
                    }
                }
                List<PeridoAgenda> agendaAuxAindaNaoCriadas = oEA.CarregarAgendaAindaNaoCriada(EntregaID, CEP, naoEncontradas, listaEventoID, dataEventoMaisProximo.Date);

                if (agendaAuxAindaNaoCriadas != null)
                    foreach (DataAgenda item in lista)
                        item.ListaPeriodo.AddRange(agendaAuxAindaNaoCriadas.Where(c => c.Data == item.Dia).ToList());


                foreach (DataAgenda item in lista)
                {
                    foreach (PeridoAgenda periodo in item.ListaPeriodo)
                    {
                        Feriado oFeriado = new Feriado();

                        bool disponivel = (item.Dia.Date > DateTime.Now.AddDays(item.DiasTriagem));
                        if (disponivel)
                        {
                            disponivel = item.DataDoEvento.Date > item.Dia.Date;
                            if (disponivel)
                            {
                                disponivel = !oFeriado.VerificaFeriado(item.Dia.Date);
                                if (disponivel)
                                {
                                    disponivel = periodo.Disponivel ? oEA.Disponivel(item.Dia, periodo.EntregaControleID) : false;
                                }
                            }
                        }

                        periodo.Disponivel = disponivel;
                    }

                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<DataAgenda> ListarDatasVIR(DateTime dataExibicao, int EntregaID, string CEP, DateTime menorValidade)
        {
            try
            {
                List<DataAgenda> lista = new List<DataAgenda>();
                List<PeridoAgenda> listaPeriodos = ListarPeriodos(EntregaID, CEP);
                DateTime dataDia = dataExibicao.AddDays(-1);
                Entrega oEntrega = new Entrega();

                int diasTriagemMaior = oEntrega.MaiorDiasTriagem(EntregaID) + 1;

                for (int i = 0; i < 31; i++)
                {
                    dataDia = dataDia.AddDays(1).Date;
                    if (dataExibicao.Month == dataDia.Month)
                    {
                        lista.Add(new DataAgenda
                        {
                            Dia = dataDia,
                            DiasTriagem = diasTriagemMaior,
                            DataDoEvento = menorValidade

                        });
                    }
                }


                foreach (DataAgenda item in lista)
                {
                    foreach (PeridoAgenda periodo in listaPeriodos)
                    {
                        EntregaAgenda oEA = new EntregaAgenda();
                        Feriado oFeriado = new Feriado();

                        bool disponivel = (item.Dia.Date > DateTime.Now.AddDays(item.DiasTriagem));
                        if (disponivel)
                        {
                            disponivel = item.DataDoEvento.Date > item.Dia.AddDays(item.DiasTriagem);

                            if (disponivel)
                            {
                                disponivel = !oFeriado.VerificaFeriado(item.Dia.Date);
                                if (disponivel)
                                {
                                    disponivel = periodo.Disponivel ? oEA.Disponivel(item.Dia, periodo.EntregaControleID) : false;
                                }
                            }

                        }




                        item.ListaPeriodo.Add(new PeridoAgenda
                        {
                            Periodo = periodo.Periodo,
                            PeriodoID = periodo.PeriodoID,
                            EntregaControleID = periodo.EntregaControleID,
                            Disponivel = disponivel,
                            HoraFinal = periodo.HoraFinal,
                            HoraInicial = periodo.HoraInicial
                        });
                    }

                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int MaiorDiasTriagem(int EntregaID, List<int> listaEvento)
        {
            try
            {
                string eventos = Utilitario.ArrayToString(listaEvento.ToArray());

                string sql = "";

                sql = string.Format(@"
						SELECT tEntrega.PrazoEntrega +
							   CASE WHEN MAX(tEntregaControle.DiasTriagem) > 0
										 THEN MAX(tEntregaControle.DiasTriagem)
										 ELSE MAX(tEntrega.DiasTriagem)
									  END + MAX(tEventoEntregaControle.DiasTriagem) AS DiasTriagem
						FROM tEntrega (NOLOCK)
						INNER JOIN tEntregaControle (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
						INNER JOIN tEventoEntregaControle (NOLOCK) ON tEntregaControle.ID=tEventoEntregaControle.EntregaControleID
						WHERE tEntrega.ID = {0} AND tEventoEntregaControle.EventoID IN ({1})
						GROUP BY tEntrega.PrazoEntrega ", EntregaID, eventos);

                return Convert.ToInt32(bd.ConsultaValor(sql));
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

        public int MaiorDiasTriagemEntrega(int EntregaID, List<int> listaEvento)
        {
            try
            {
                string eventos = Utilitario.ArrayToString(listaEvento.ToArray());

                string sql = string.Format(@"
						SELECT tEntrega.PrazoEntrega +
							   CASE WHEN MAX(tEntregaControle.DiasTriagem) > 0
										 THEN MAX(tEntregaControle.DiasTriagem)
										 ELSE MAX(tEntrega.DiasTriagem)
									  END + MAX(tEventoEntregaControle.DiasTriagem) AS DiasTriagem
						FROM tEntrega (NOLOCK)
						INNER JOIN tEntregaControle (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
						INNER JOIN tEventoEntregaControle (NOLOCK) ON tEntregaControle.ID=tEventoEntregaControle.EntregaControleID
						WHERE tEntregaControle.ID = {0} AND tEventoEntregaControle.EventoID IN ({1})
						GROUP BY tEntrega.PrazoEntrega ", EntregaID, eventos);

                return Convert.ToInt32(bd.ConsultaValor(sql));
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

        public int MaiorDiasTriagemEntregaVale(int EntregaID)
        {
            try
            {
                string sql = string.Format(@"
						SELECT tEntrega.PrazoEntrega +
							   CASE WHEN MAX(tEntregaControle.DiasTriagem) > 0
										 THEN MAX(tEntregaControle.DiasTriagem)
										 ELSE MAX(tEntrega.DiasTriagem)
									  END AS DiasTriagem
						FROM tEntrega (NOLOCK)
						INNER JOIN tEntregaControle (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
						WHERE tEntregaControle.ID = {0}
						GROUP BY tEntrega.PrazoEntrega ", EntregaID);

                return Convert.ToInt32(bd.ConsultaValor(sql));
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


        private int MaiorDiasTriagem(int EntregaID)
        {
            try
            {
                int prazo = 0, diasTriagem = 0;

                string sql = "";

                sql = @"SELECT MAX(tEntrega.PrazoEntrega) AS Prazo,
						MAX(tEntrega.DiasTriagem) AS DiasTriagem
						FROM tEntrega
						WHERE tEntrega.ID = " + EntregaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    prazo = bd.LerInt("Prazo");
                    diasTriagem = bd.LerInt("DiasTriagem");
                }

                return prazo + diasTriagem;
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

        private List<PeridoAgenda> ListarPeriodos(int EntregaID, string CEP)
        {
            try
            {
                List<PeridoAgenda> listaRetorno = new List<PeridoAgenda>();

                string sql = "";

                sql = @"select  tEntregaControle.ID as EntregaControleID,tEntregaControle.QuantidadeEntregas
						,tEntregaPeriodo.Nome,tEntregaPeriodo.ID as PeriodoID,tEntregaPeriodo.HoraInicial,tEntregaPeriodo.HoraFinal
						from tEntregaControle 
						inner join tEntregaAreaCep on tEntregaControle.EntregaAreaID = tEntregaAreaCep.EntregaAreaID
						left join tEntregaPeriodo on tEntregaControle.PeriodoID = tEntregaPeriodo.ID
						where entregaid = " + EntregaID + @" and 
						(tEntregaAreaCep.CepInicial <= " + CEP + " and tEntregaAreaCep.CepFinal >= " + CEP + ")  ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    listaRetorno.Add(new PeridoAgenda
                    {
                        Periodo = bd.LerString("Nome"),
                        PeriodoID = bd.LerInt("PeriodoID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Disponivel = (bd.LerInt("QuantidadeEntregas") > 0),
                        HoraFinal = bd.LerDateTime("HoraFinal"),
                        HoraInicial = bd.LerDateTime("HoraInicial")
                    });


                }


                return listaRetorno;
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

        private List<PeridoAgenda> ListarPeriodos(int EntregaID, string CEP, List<int> listaEvento)
        {
            try
            {
                List<PeridoAgenda> listaRetorno = new List<PeridoAgenda>();
                string eventos = "";
                foreach (int EventoID in listaEvento)
                {
                    if (eventos.Length > 0)
                    {
                        eventos += "," + EventoID;
                    }
                    else
                    {
                        eventos += EventoID;
                    }
                }

                string sql = "";

                sql = @"select distinct tEntregaControle.ID as EntregaControleID,tEntregaControle.QuantidadeEntregas
					,tEntregaPeriodo.Nome,tEntregaPeriodo.ID as PeriodoID,tEntregaPeriodo.HoraInicial,tEntregaPeriodo.HoraFinal
					from tEntregaControle (nolock) 
					inner join tEntregaAreaCep (nolock)  on tEntregaControle.EntregaAreaID = tEntregaAreaCep.EntregaAreaID
					inner join tEventoEntregaControle (nolock)  on tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
					left join tEntregaPeriodo (nolock)  on tEntregaControle.PeriodoID = tEntregaPeriodo.ID
					where entregaid = " + EntregaID + @" and 
						(tEntregaAreaCep.CepInicial <= " + CEP + " and tEntregaAreaCep.CepFinal > " + CEP + ") and tEventoEntregaControle.EventoID in (" + eventos + ")";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    listaRetorno.Add(new PeridoAgenda
                    {
                        Periodo = bd.LerString("Nome"),
                        PeriodoID = bd.LerInt("PeriodoID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Disponivel = (bd.LerInt("QuantidadeEntregas") > 0),
                        HoraFinal = bd.LerDateTime("HoraFinal"),
                        HoraInicial = bd.LerDateTime("HoraInicial")
                    });
                }

                return listaRetorno;
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

        public List<int> ListaDiasDaSemana(int EntregaControleID)
        {
            try
            {

                List<int> listaDias = new List<int>();

                string sql = "";

                sql = @"select DiaDaSemana from tDiasSemana
					where ControleEntregaID = " + EntregaControleID + " order by DiaDaSemana";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    listaDias.Add(bd.LerInt("DiaDaSemana"));
                }

                bd.Fechar();

                return listaDias;
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal List<int> GetEntregaPadrao()
        {
            try
            {
                List<int> ids = new List<int>();
                bd.Consulta(@"select tEntregaControle.ID 
								from tEntregaControle
								inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
								where Padrao='T' ");
                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("ID"));

                return ids;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEntrega> GetByEventoApresentacao(int EventoID, int ApresentacaoID)
        {
            try
            {
                List<EstruturaEntrega> listaRetorno = new List<EstruturaEntrega>();
                string sql = "";

                sql = @"SELECT DISTINCT 
									tEntrega.ID, 
									tEntrega.Nome, 
									PrazoEntrega, 
									tEntrega.ProcedimentoEntrega AS ProcedimentoEntrega
						FROM tEntregaControle (NOLOCK)
						INNER JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID
						INNER JOIN tEventoEntregaControle (NOLOCK) ON  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
						INNER JOIN tApresentacao ap (NOLOCK) ON ap.EventoID = tEventoEntregaControle.EventoID
						WHERE tEventoEntregaControle.EventoID = " + EventoID + " AND ap.ID = " + ApresentacaoID + @" AND
						(PrazoEntrega = 0 OR CONVERT(VARCHAR, DATEADD(day, PrazoEntrega, CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring(ap.Horario,0,9), 112))
						ORDER BY tEntrega.Nome
					";

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    listaRetorno.Add(new EstruturaEntrega
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega")
                    });
                }


                if (listaRetorno.Count == 0)
                    throw new Exception("Não existem taxas de entrega disponíveis para esta apresentação.");


                return listaRetorno;
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

        public List<EstruturaEntrega> GetByEventoSomentePacotes(int EventoID)
        {

            try
            {
                List<EstruturaEntrega> listaRetorno = new List<EstruturaEntrega>();
                string sql = "";

                sql = @"SELECT DISTINCT 
									tEntrega.ID, 
									tEntrega.Nome, 
									PrazoEntrega, 
									tEntrega.ProcedimentoEntrega AS ProcedimentoEntrega
						FROM tEventoEntregaControle (NOLOCK)
						INNER JOIN tEntregaControle (NOLOCK) ON  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
						INNER JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID
					WHERE tEventoEntregaControle.EventoID = " + EventoID + @" 
					ORDER BY tEntrega.Nome";

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    listaRetorno.Add(new EstruturaEntrega
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega")
                    });
                }


                if (listaRetorno.Count == 0)
                    throw new Exception("Não existem taxas de entrega disponíveis.");


                return listaRetorno;
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

        public static Enumeradores.TaxaEntregaTipo TipoToEnum(string tipo)
        {
            switch (tipo)
            {
                case Entrega.AGENDADA:
                case Entrega.NORMAL:
                    return Enumeradores.TaxaEntregaTipo.Entrega;
                case Entrega.RETIRADABILHETERIA:
                case Entrega.RETIRADA:
                    return Enumeradores.TaxaEntregaTipo.Retirada;
                default:
                    return Enumeradores.TaxaEntregaTipo.Nenhum;
            }
        }

        public List<EstrutraEntregaSimples> CarregarEntregasAssinatura(int entregaID)
        {
            try
            {
                string sql =
                    string.Format(@"
						SELECT DISTINCT
							e.ID AS EntregaID, e.Nome, e.ProcedimentoEntrega
						FROM tEntrega e (NOLOCK)
						WHERE e.ID = {0}
					", entregaID);


                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar nenhuma taxa de entrega.");

                List<EstrutraEntregaSimples> lista = new List<EstrutraEntregaSimples>();

                do
                {
                    lista.Add(new EstrutraEntregaSimples()
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        Nome = bd.LerString("Nome"),
                        Procedimento = bd.LerString("ProcedimentoEntrega"),
                        Valor = Convert.ToDecimal(ConfigurationManager.AppSettings["ValorEntrega"])
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstrutraEntregaSimples> CarregarEntregasAssinatura(
                List<int> lstEntregaID, int clienteID, bool valorEntregaFixo, bool retiradaBilheteria, List<int> entregasExclusivasOperador, string ValorEntrega)
        {
            try
            {

                ClienteEndereco oClienteEndereco = new ClienteEndereco();
                oClienteEndereco.BuscarPorCliente(clienteID);

                var naoCarregar = entregasExclusivasOperador == null || entregasExclusivasOperador.Count == 0 ? "0" : Utilitario.ArrayToString(entregasExclusivasOperador.ToArray());

                string sql =
                    string.Format(@"SELECT DISTINCT e.ID AS EntregaID, e.Nome, e.ProcedimentoEntrega, ec.Valor
                                FROM tEntrega e (NOLOCK)
                                INNER JOIN tEntregaControle ec (NOLOCK) ON ec.EntregaID = e.ID
                                INNER JOIN tEntregaArea ea (NOLOCK) ON ea.ID = ec.EntregaAreaID
                                INNER JOIN tEntregaAreaCep eac (NOLOCK) ON eac.EntregaAreaID = ea.ID
                                WHERE ec.EntregaID in ({0}) AND eac.CepInicial <= '{1} ' AND eac.CepFinal >= '{1}' AND ec.EntregaID NOT IN ({2}) 
        					", Utilitario.ArrayToString(lstEntregaID.ToArray()), oClienteEndereco.CEP.Valor, naoCarregar);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar nenhuma taxa de entrega.");

                List<EstrutraEntregaSimples> lista = new List<EstrutraEntregaSimples>();


                decimal valorEntrega = 0;
                if (valorEntregaFixo)
                    valorEntrega = Convert.ToDecimal(ValorEntrega);

                do
                {
                    lista.Add(new EstrutraEntregaSimples()
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        Nome = bd.LerString("Nome"),
                        Procedimento = bd.LerString("ProcedimentoEntrega"),
                        Valor = valorEntregaFixo ? valorEntrega : bd.LerDecimal("Valor"),
                    });
                } while (bd.Consulta().Read());

                //if (retiradaBilheteria)                
                sql = string.Format(@"SELECT DISTINCT e.ID AS EntregaID, e.Nome, e.ProcedimentoEntrega, ec.Valor
                                FROM tEntrega e (NOLOCK)
                                INNER JOIN tEntregaControle ec (NOLOCK) ON ec.EntregaID = e.ID
                            WHERE ec.EntregaID IN({0}) AND e.Tipo = '{1}' AND ec.EntregaID NOT IN ({2})", Utilitario.ArrayToString(lstEntregaID.ToArray()), Entrega.RETIRADABILHETERIA, naoCarregar);

                if (!bd.Consulta(sql).Read())
                    return lista;

                lista.Insert(0, new EstrutraEntregaSimples()
                {
                    EntregaID = bd.LerInt("EntregaID"),
                    Nome = bd.LerString("Nome"),
                    Procedimento = bd.LerString("ProcedimentoEntrega"),
                    Valor = valorEntregaFixo ? valorEntrega : bd.LerDecimal("Valor"),
                });

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public decimal CarregarValorEntregaAssinatura(int entregaID, string entregaTipo, int clienteID)
        {
            try
            {
                if (entregaTipo == RETIRADABILHETERIA)
                    return Convert.ToDecimal(bd.ConsultaValor("SELECT TOP 1 ec.Valor FROM tEntregaControle ec (NOLOCK) WHERE ec.EntregaID = " + entregaID));

                ClienteEndereco oClienteEndereco = new ClienteEndereco();
                oClienteEndereco.BuscarPorCliente(clienteID);


                return Convert.ToDecimal(bd.ConsultaValor(
                    string.Format(@"SELECT TOP 1 ec.Valor
                                        FROM tEntrega e (NOLOCK)
                                        INNER JOIN tEntregaControle ec (NOLOCK) ON ec.EntregaID = e.ID
                                        INNER JOIN tEntregaArea ea (NOLOCK) ON ea.ID = ec.EntregaAreaID
                                        INNER JOIN tEntregaAreaCep eac (NOLOCK) ON eac.EntregaAreaID = ea.ID
                                        WHERE ec.EntregaID = {0} AND eac.CepInicial <= '{1} ' AND eac.CepFinal >= '{1}'
        					    ", entregaID, oClienteEndereco.CEP.Valor)));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string TipoEntrega(int EntregaControleIDSelecionado)
        {
            try
            {
                string retorno = "";
                string sql = "";

                sql = @"SELECT DISTINCT 
                                    tEntrega.Tipo
                        FROM tEntregaControle (NOLOCK)
                        INNER JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEntregaControle.ID = " + EntregaControleIDSelecionado;

                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    retorno = bd.LerString("Tipo");
                }


                if (retorno.Length <= 0)
                    throw new Exception("Erro ao selecionar taxa de entrega.");


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

        public decimal VerificaValor(int EntregaControleIDSelecionado)
        {
            try
            {
                decimal retorno = 0;
                string sql = "";

                sql = @"SELECT tEntregaControle.Valor
                        FROM tEntregaControle (NOLOCK)
                        WHERE tEntregaControle.ID = " + EntregaControleIDSelecionado;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = bd.LerDecimal("Valor");
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

        public List<EstruturaEntrega> EntregasPorFilme(int filmeID)
        {
            try
            {
                string sql =
                    @"
                        SELECT
	                        DISTINCT et.Nome AS Entrega, et.ProcedimentoEntrega
	                        FROM tFilme f (NOLOCK)
	                        INNER JOIN tEvento e (NOLOCK) ON e.FilmeID = f.FilmeID
	                        INNER JOIN tEventoEntregaControle eec (NOLOCK) ON e.ID = eec.EventoID
	                        INNER JOIN tEntregaControle ec (NOLOCK) ON ec.ID = eec.EntregaControleID
	                        INNER JOIN tEntrega et (NOLOCK) ON et.ID = ec.EntregaID
	                        WHERE f.FilmeID = " + filmeID;

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Nenhuma taxa de entrega encontrada.");

                List<EstruturaEntrega> lista = new List<EstruturaEntrega>();

                do
                {
                    lista.Add(new EstruturaEntrega() { Nome = bd.LerString("Entrega"), ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega").Replace(Environment.NewLine, "<br/>") });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool VerificaCep(int EntregaControleIDSelecionado, int EnderecoID)
        {
            try
            {
                string sql = string.Format(@"DECLARE  @CEP  int
                        SET @CEP = (SELECT CEP FROM tClienteEndereco WHERE ID = {0})
                        SELECT Top 1 tEntregaControle.ID 
                        FROM tEntregaControle
                        INNER JOIN tEntregaAreaCEP on tEntregaControle.EntregaAreaID = tEntregaAreaCEP.EntregaAreaID
                        WHERE tEntregaAreaCEp.CepInicial <= @CEP AND @CEP <= tEntregaAreaCEp.CepFinal AND tEntregaControle.ID = {1}", EnderecoID, EntregaControleIDSelecionado);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return true;
                else
                    throw new Exception("CEP selecionado não possui entrega! Por favor selecione outro, ou tente outro meio de entrega.");
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

        public bool VerificaeTicket(int EntregaControleID)
        {
            try
            {
                string sql = @"SELECT tEntrega.PermitirImpressaoInternet
                FROM tEntregaControle (NOLOCK)
                INNER JOIN tEntrega (NOLOCK) on tEntregaControle.EntregaID = tEntrega.ID
                WHERE tEntregaControle.ID = " + EntregaControleID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return bd.LerBoolean("PermitirImpressaoInternet");

                return false;
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

        public List<EstrutraEntregaSimples> BuscarEntregasMobile(int BilheteriaID, int BilheteWebID, List<int> listaApresentacao, List<int> listaEventos)
        {
            try
            {
                List<EstrutraEntregaSimples> retorno = new List<EstrutraEntregaSimples>();

                string eventos = "";

                Apresentacao oApresentacao = new Apresentacao();

                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);

                string entregas = BilheteriaID + "," + BilheteWebID;

                foreach (int eventoID in listaEventos)
                {
                    if (eventos.Length > 0)
                        eventos += "," + eventoID;
                    else
                        eventos += eventoID;
                }

                string sql = @"SELECT tEntregaControle.ID AS EntregaControleID, tEntregaControle.EntregaID, tEntrega.Nome, tEntrega.ProcedimentoEntrega, tEntregaControle.Valor
                        FROM tEventoEntregaControle (NOLOCK)
                        INNER JOIN tEntregaControle (NOLOCK) ON  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        INNER JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEventoEntregaControle.EventoID IN (" + eventos + ")  AND tEntrega.ID IN (" + entregas +
                        @") AND tEntrega.Tipo in ('R','B') AND CONVERT(VARCHAR, DATEADD(day, (PrazoEntrega + 
			                CASE WHEN
				                tEventoEntregaControle.DiasTriagem > 0
					                THEN tEventoEntregaControle.DiasTriagem
				                ELSE
					                CASE WHEN
						                tEntregaControle.DiasTriagem > 0
						                THEN tEntregaControle.DiasTriagem
						                ELSE tEntrega.DiasTriagem
					                END
				                END  
		                ), CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring('" + dataEventoMaisProximo.ToString("yyyyMMdd") + @"',0,9), 112)
                        GROUP BY tEntregaControle.ID, tEntregaControle.EntregaID, tEntrega.Nome, tEntrega.ProcedimentoEntrega, tEntregaControle.Valor
                        HAVING COUNT(Distinct EventoID) = " + listaEventos.Count;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstrutraEntregaSimples
                    {
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        EntregaID = bd.LerInt("EntregaID"),
                        Nome = bd.LerString("Nome"),
                        Procedimento = bd.LerString("ProcedimentoEntrega"),
                        Valor = bd.LerDecimal("Valor")
                    });
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

        public string BuscaTipo(int EntregaControleID)
        {
            try
            {
                string sql = sql = @"select CASE Tipo
                        when 'B' then 'RETIRADA BILHETERIA'
                        when 'A' then 'ENTREGA AGENDADA'
                        when 'N' then 'ENTREGA NORMAL'
                        when 'R' then 'RETIRADA PDV'
                        End as TipoEntrega
                        from tEntregaControle
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        where tEntregaControle.ID = " + EntregaControleID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return bd.LerString("TipoEntrega");
                else
                    return string.Empty;
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

        public List<Assinaturas.Models.Entrega> ListarAtivas()
        {
            try
            {
                List<Assinaturas.Models.Entrega> retorno = new List<Assinaturas.Models.Entrega>();

                string sql = sql = @"SELECT ID, te.Nome FROM tEntrega te (NOLOCK) WHERE te.Disponivel = 'T' AND te.Ativo  = 'T'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new Assinaturas.Models.Entrega()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome")
                    });
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

        public static List<EstruturaIDNome> EmpresaComEticket(int entregaID)
        {
            using (BD bd = new BD())
            {
                string sql = @"SELECT 
                                DISTINCT EmpresaID, Empresa
                                FROM 
                                DadosEticket
                                ORDER BY Empresa";

                var retorno = new List<EstruturaIDNome>();
                retorno.Add(new EstruturaIDNome());


                while (bd.Consulta(sql).Read())
                    retorno.Add(new EstruturaIDNome { ID = bd.LerInt("EmpresaID"), Nome = bd.LerString("Empresa") });

                return retorno;
            }
        }

        public static List<EstruturaIDNome> LocalComEticket(int entregaID, int empresaID)
        {
            using (BD bd = new BD())
            {
                string sql = @"SELECT 
                                DISTINCT LocalID, Local
                                FROM 
                                DadosEticket
                                WHERE 
                                EmpresaID = " + empresaID + @" 
                                ORDER BY Local";

                var retorno = new List<EstruturaIDNome>();
                retorno.Add(new EstruturaIDNome());

                while (bd.Consulta(sql).Read())
                    retorno.Add(new EstruturaIDNome { ID = bd.LerInt("LocalID"), Nome = bd.LerString("Local") });

                return retorno;
            }
        }

        public static List<EstruturaIDNome> EventoComEticket(int entregaID, int localID)
        {
            using (BD bd = new BD())
            {
                string sql = @"SELECT 
                                DISTINCT EventoID, Evento
                                FROM 
                                DadosEticket
                                WHERE 
                                LocalID = " + localID + @" 
                                ORDER BY Evento";

                var retorno = new List<EstruturaIDNome>();

                retorno.Add(new EstruturaIDNome());

                while (bd.Consulta(sql).Read())
                    retorno.Add(new EstruturaIDNome { ID = bd.LerInt("EventoID"), Nome = bd.LerString("Evento") });

                return retorno;
            }
        }

        public static List<EstruturaIDNome> ApresentacaoComEticket(int entregaID, int eventoID)
        {
            BD bd = new BD();

            string sql = @"SELECT 
                                DISTINCT ApresentacaoID, Data
                                FROM 
                                DadosEticket
                                WHERE 
                                EventoID = " + eventoID + @" 
                                ORDER BY Data";

            var retorno = new List<EstruturaIDNome>();
            retorno.Add(new EstruturaIDNome());


            while (bd.Consulta(sql).Read())
                retorno.Add(new EstruturaIDNome { ID = bd.LerInt("ApresentacaoID"), Nome = bd.LerDateTime("Data").ToString("dd/MM/yyyy HH:mm") });

            return retorno;
        }



        public static List<EstruturaIDNome> EventosComEticket(int entregaID)
        {
            BD bd = new BD();
            string sql = @"SELECT 
                                DISTINCT Empresa, Local, Evento 
                                FROM DadosEticket
                                WHERE Data LIKE '" + DateTime.Now.ToString("yyyyMMdd") + @"%'
                                ORDER BY Empresa, Local, Evento ";

            var retorno = new List<EstruturaIDNome>();

            while (bd.Consulta(sql).Read())
            {
                var nome = bd.LerString("Empresa") + " - " + bd.LerString("Local") + " - " + bd.LerString("Evento");
                retorno.Add(new EstruturaIDNome { Nome = nome });
            }

            return retorno;
        }

        public static void CarregaDadosEticket()
        {

            using (BD bd = new BD())
            {
                bd.Executar("EXEC Eticket_CarregaDados");
            }

        }


        public static List<object> ComprasETicket(int entregaID, int apresentacaoID)
        {
            var retorno = new List<object>();

            using (BD bd = new BD())
            {
                string sql = @"SELECT 
                                DISTINCT tvb.ID, tc.Nome AS Cliente, tc.CPF, tvb.Senha AS Senha
                                FROM tVendaBilheteria tvb (NOLOCK)
                                INNER JOIN tEntregaControle tec  (NOLOCK) ON tec.id = tvb.EntregaControleID
                                INNER JOIN tIngresso ti (NOLOCK) ON ti.VendaBilheteriaID = tvb.ID
                                INNER JOIN tEmpresa te (NOLOCK) ON te.ID = ti.EmpresaID
                                INNER JOIN tLocal tl  (NOLOCK) ON tl.ID = ti.LocalID
                                INNER JOIN tEvento tev ON tev.ID = ti.EventoID
                                INNER JOIN tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID
                                INNER JOIN tEntrega ten (NOLOCK) ON ten.id = tec.EntregaID
                                INNER JOIN tCliente tc (NOLOCK) ON tc.ID = tvb.ClienteID
                                INNER JOIN DadosEticket DE ON de.VendaBilheteriaID = tvb.ID
                                WHERE 
                                EntregaID = 11 AND ti.ApresentacaoID = " + apresentacaoID + @"
                                ORDER BY tc.Nome";


                while (bd.Consulta(sql).Read())
                    retorno.Add(
                            new
                            {
                                VendaBilheteriaID = bd.LerInt("ID"),
                                Cliente = bd.LerString("Cliente"),
                                CPF = bd.LerString("CPF"),
                                Senha = bd.LerString("Senha")
                            }
                            );


            }

            return retorno;
        }







    }

    public class EntregaLista : EntregaLista_B
    {

        public EntregaLista() { }

        public EntregaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
