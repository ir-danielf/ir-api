/**************************************************
* Arquivo: AssinaturaBancoIngresso.cs
* Gerado: 01/12/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Assinaturas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IRLib
{

    public class AssinaturaBancoIngresso : AssinaturaBancoIngresso_B
    {
        public AssinaturaBancoIngresso() { }


        public List<Calendario> MontarCalendario(int ano, int mes, int horaMinima, int clienteID)
        {
            try
            {
                List<Calendario> dias = new List<Calendario>();
                int primeiroDiaSemana = 0; //domingo
                int ultimoDiaSemana = 6; //sabado
                int linha = 1;

                List<int> diasSemana = new List<int>();

                DateTime dataInicial = new DateTime(DateTime.Now.Year, mes, 1);

                DateTime dataAtual = dataInicial;

                //Se não comecar no Domingo, precisa adicionar os itens anteriores no dia da semana.
                int dayOfWeek = Convert.ToInt32(dataInicial.DayOfWeek);
                if (primeiroDiaSemana < dayOfWeek)
                    for (int i = 0; i < dayOfWeek; i++)
                        dias.Add(new Calendario() { Line = 1 });

                for (int i = 0; i <= 31; i++)
                {
                    dataAtual = dataInicial.AddDays(i);

                    if (dataAtual.Month != dataInicial.Month)
                        break;

                    linha = dias.Count % 7 == 0 ? linha + 1 : linha;
                    dias.Add(new Calendario()
                    {
                        Value = dataAtual.Day,
                        Line = linha,
                    });
                }

                DateTime dataFinal = new DateTime(ano, mes, dias.LastOrDefault().Value);

                //Se não terminar no Sabado, precisa adicionar os itens posteriores no dia da semana.
                dayOfWeek = Convert.ToInt32(dataFinal.DayOfWeek);
                if (dayOfWeek < ultimoDiaSemana)
                    for (int i = dayOfWeek; i < ultimoDiaSemana; i++)
                        dias.Add(new Calendario() { Line = linha });


                var calendarioDisponivel = this.MontarCalendarioIngressos(ano, mes, horaMinima, clienteID);

                if (calendarioDisponivel == null || calendarioDisponivel.Count == 0)
                    return dias;

                //Se os dias estão disponiveis no calendario de ingressos, disponibilida no linq abaixo
                (from d in dias
                 join c in calendarioDisponivel on d.Value equals c.Day
                 select d)
                 .ToList()
                 .ForEach(delegate(Calendario calendario) { calendario.IsValid = calendario.HasValue = true; });

                return dias;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Calendario> MontarCalendario(int ano, int mes, int horaMinima, int clienteID, int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                List<Calendario> dias = new List<Calendario>();
                int primeiroDiaSemana = 0; //domingo
                int ultimoDiaSemana = 6; //sabado
                int linha = 1;

                List<int> diasSemana = new List<int>();

                DateTime dataInicial = new DateTime(DateTime.Now.Year, mes, 1);

                DateTime dataAtual = dataInicial;

                //Se não comecar no Domingo, precisa adicionar os itens anteriores no dia da semana.
                int dayOfWeek = Convert.ToInt32(dataInicial.DayOfWeek);
                if (primeiroDiaSemana < dayOfWeek)
                    for (int i = 0; i < dayOfWeek; i++)
                        dias.Add(new Calendario() { Line = 1 });

                for (int i = 0; i <= 31; i++)
                {
                    dataAtual = dataInicial.AddDays(i);

                    if (dataAtual.Month != dataInicial.Month)
                        break;

                    linha = dias.Count % 7 == 0 ? linha + 1 : linha;
                    dias.Add(new Calendario()
                    {
                        Value = dataAtual.Day,
                        Line = linha,
                    });
                }

                DateTime dataFinal = new DateTime(ano, mes, dias.LastOrDefault().Value);

                //Se não terminar no Sabado, precisa adicionar os itens posteriores no dia da semana.
                dayOfWeek = Convert.ToInt32(dataFinal.DayOfWeek);
                if (dayOfWeek < ultimoDiaSemana)
                    for (int i = dayOfWeek; i < ultimoDiaSemana; i++)
                        dias.Add(new Calendario() { Line = linha });


                var calendarioDisponivel = this.MontarCalendarioIngressos(ano, mes, horaMinima, clienteID, assinaturaTipoIDBancoIngresso);

                if (calendarioDisponivel == null || calendarioDisponivel.Count == 0)
                    return dias;

                //Se os dias estão disponiveis no calendario de ingressos, disponibilida no linq abaixo
                (from d in dias
                 join c in calendarioDisponivel on d.Value equals c.Day
                 select d)
                 .ToList()
                 .ForEach(delegate(Calendario calendario) { calendario.IsValid = calendario.HasValue = true; });

                return dias;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CalendarioIngressos> MontarCalendarioIngressos(int ano, int mes, int horaMinima, int clienteID)
        {
            try
            {
                List<CalendarioIngressos> lista = new List<CalendarioIngressos>();

                DateTime dataPesquisa = new DateTime(ano, mes, 1);
                dataPesquisa.AddHours(DateTime.Now.Hour);

                DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).AddHours(horaMinima);

                //Tentando ver retroativo, retorna sem nada...
                if (dataInicial.Year <= DateTime.Now.Year && dataInicial.Month < DateTime.Now.Month)
                    return null;
               

                string sql = string.Format(@"
						SELECT DISTINCT 
								ap.ID, ap.Horario
									FROM tAssinaturaBancoIngresso (NOLOCK) abi
									INNER JOIN tApresentacao ap (NOLOCK) ON abi.ApresentacaoID = ap.ID
									LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = abi.ID
								WHERE abi.ClienteID = {2} AND ap.Horario BETWEEN '{0}' AND '{1}' AND left(ap.Horario,6) = '{3}' AND abi.Aprovado = 'T' AND bir.ID IS NULL
								ORDER BY ap.Horario
					   ", dataInicial.ToString("yyyyMMddHHmmss"), dataPesquisa.ToString("yyyyMM") + "31999999", clienteID, dataPesquisa.ToString("yyyyMM"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(new CalendarioIngressos() { ApresentacaoID = bd.LerInt("ID"), Day = bd.LerDateTime("Horario").Day, Available = true });

                //Apresentacao do dia 11/08 deve ser removida de resgates
                if (clienteID == 0 && lista.Where(c => c.ApresentacaoID == 98445).Count() > 0)
                    lista.Remove(lista.Where(c => c.ApresentacaoID == 98445).FirstOrDefault());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<CalendarioIngressos> MontarCalendarioIngressos(int ano, int mes, int horaMinima, int clienteID, int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                List<CalendarioIngressos> lista = new List<CalendarioIngressos>();

                DateTime dataPesquisa = new DateTime(ano, mes, 1);
                dataPesquisa.AddHours(DateTime.Now.Hour);

                DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).AddHours(horaMinima);

                //Tentando ver retroativo, retorna sem nada...
                if (dataInicial.Year <= DateTime.Now.Year && dataInicial.Month < DateTime.Now.Month)
                    return null;


                string sql = string.Format(@"
						SELECT DISTINCT 
								ap.ID, ap.Horario
									FROM tAssinaturaBancoIngresso (NOLOCK) abi
									INNER JOIN tApresentacao ap (NOLOCK) ON abi.ApresentacaoID = ap.ID
									LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = abi.ID
                                    LEFT JOIN tAssinatura asn(NOLOCK) ON abi.AssinaturaID = asn.ID
								WHERE abi.ClienteID = {2} AND ap.Horario BETWEEN '{0}' AND '{1}' AND left(ap.Horario,6) = '{3}' AND abi.Aprovado = 'T' AND bir.ID IS NULL AND asn.AssinaturaTipoID = {4}
								ORDER BY ap.Horario
					   ", dataInicial.ToString("yyyyMMddHHmmss"), dataPesquisa.ToString("yyyyMM") + "31999999", clienteID, dataPesquisa.ToString("yyyyMM") , assinaturaTipoIDBancoIngresso);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(new CalendarioIngressos() { ApresentacaoID = bd.LerInt("ID"), Day = bd.LerDateTime("Horario").Day, Available = true });

                //Apresentacao do dia 11/08 deve ser removida de resgates
                if (clienteID == 0 && lista.Where(c => c.ApresentacaoID == 98445).Count() > 0)
                    lista.Remove(lista.Where(c => c.ApresentacaoID == 98445).FirstOrDefault());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Ingressos para doação!!!
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="mes"></param>
        /// <param name="ano"></param>
        /// <param name="clienteID"></param>
        /// <param name="horaMinima"></param>
        /// <returns></returns>
        public List<DataDoacao> CarregarIngressos(int dia, int mes, int ano, int clienteID, int horaMinima)
        {
            try
            {
                if (clienteID == 0)
                    throw new Exception("Você deve estar logado para efetuar a doação de ingressos!");

                DateTime data = new DateTime(ano, mes, dia);
                DateTime dataIncial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + horaMinima, DateTime.Now.Minute, DateTime.Now.Second);

                List<DataDoacao> lista = new List<DataDoacao>();
                List<IngressoDoacao> listaIngresso = new List<IngressoDoacao>();

                string sql = string.Format(@"
					SELECT 
		                ap.ID AS ApresentacaoID, ap.Horario, IsNull(ap.Programacao, 'Programação não cadastrada!') AS Programacao, IngressoID, s.Nome AS Setor, i.Codigo
	                FROM tAssinaturaBancoIngresso bi (NOLOCK)
	                INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
	                INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
	                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
	                WHERE ap.Horario BETWEEN '{0}' AND '{1}' AND bi.ClienteID = {2} AND ap.Horario like '{3}%'
	                ORDER BY ap.Horario, s.Nome", dataIncial.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"), clienteID, data.ToString("yyyyMMdd"));

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Você não possui nenhum ingresso a ser doado na data selecionada.");

                do
                {
                    if (lista.Where(c => c.ApresentacaoID == bd.LerInt("ApresentacaoID")).Count() == 0)
                    {
                        listaIngresso = new List<IngressoDoacao>();
                        lista.Add(new DataDoacao()
                        {
                            ApresentacaoID = bd.LerInt("ApresentacaoID"),
                            Horario = bd.LerDateTime("Horario").ToString("dd/MM/yyyy à\\s HH:mm"),
                            Programacao = bd.LerString("Programacao"),
                            Ingressos = listaIngresso,
                        });
                    }

                    listaIngresso.Add(new IngressoDoacao()
                    {
                        IngressoID = bd.LerInt("IngressoID"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                    });
                } while (bd.Consulta().Read());
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<DataDoacao> CarregarIngressos(int dia, int mes, int ano, int clienteID, int horaMinima, int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                if (clienteID == 0)
                    throw new Exception("Você deve estar logado para efetuar a doação de ingressos!");

                DateTime data = new DateTime(ano, mes, dia);
                DateTime dataIncial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + horaMinima, DateTime.Now.Minute, DateTime.Now.Second);

                List<DataDoacao> lista = new List<DataDoacao>();
                List<IngressoDoacao> listaIngresso = new List<IngressoDoacao>();

                string sql = string.Format(@"
					SELECT 
		                ap.ID AS ApresentacaoID, ap.Horario, IsNull(ap.Programacao, 'Programação não cadastrada!') AS Programacao, IngressoID, s.Nome AS Setor, i.Codigo
	                FROM tAssinaturaBancoIngresso bi (NOLOCK)
	                INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
	                INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
	                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                    INNER JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.AssinaturaID
	                WHERE ap.Horario BETWEEN '{0}' AND '{1}' AND bi.ClienteID = {2} AND ap.Horario like '{3}%' AND asn.AssinaturaTipoID = {4}
	                ORDER BY ap.Horario, s.Nome", dataIncial.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"), clienteID, data.ToString("yyyyMMdd"),assinaturaTipoIDBancoIngresso);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Você não possui nenhum ingresso a ser doado na data selecionada.");

                do
                {
                    if (lista.Where(c => c.ApresentacaoID == bd.LerInt("ApresentacaoID")).Count() == 0)
                    {
                        listaIngresso = new List<IngressoDoacao>();
                        lista.Add(new DataDoacao()
                        {
                            ApresentacaoID = bd.LerInt("ApresentacaoID"),
                            Horario = bd.LerDateTime("Horario").ToString("dd/MM/yyyy à\\s HH:mm"),
                            Programacao = bd.LerString("Programacao"),
                            Ingressos = listaIngresso,
                        });
                    }

                    listaIngresso.Add(new IngressoDoacao()
                    {
                        IngressoID = bd.LerInt("IngressoID"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                    });
                } while (bd.Consulta().Read());
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscarAssinaturaID(int apresentacaoID)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 AssinaturaID FROM tAssinaturaBancoIngresso (NOLOCK) WHERE ApresentacaoID = " + apresentacaoID));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Reservar(int apresentacaoID, int setorID, int lugarID, int clienteID)
        {
            try
            {
                this.VerificarSaldo(clienteID, 1);

                bd.IniciarTransacao();

                this.Reservar(bd, apresentacaoID, setorID, lugarID, clienteID);

                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Reservar(int apresentacaoID, int setorID, int lugarID, int clienteID, int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                this.VerificarSaldo(clienteID, 1,assinaturaTipoIDBancoIngresso);

                bd.IniciarTransacao();

                this.Reservar(bd, apresentacaoID, setorID, lugarID, clienteID,assinaturaTipoIDBancoIngresso);

                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ReservarQuantidade(int apresentacaoID, int setorID, int quantidade, int clienteID)
        {


            try
            {
                this.VerificarSaldo(clienteID, quantidade);

                List<int> lugares = new List<int>();

                bd.Consulta(string.Format(
                    @"SELECT TOP {0} 
						 i.LugarID
					FROM tAssinaturaBancoIngresso bi (NOLOCK)
					INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
					LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = bi.ID
					WHERE bi.ApresentacaoID = {1} AND i.SetorID = {2} AND bir.ID IS NULL
                    ORDER BY NEWID()",
                quantidade, apresentacaoID, setorID));

                if (!bd.Consulta().Read())
                    throw new Exception("O setor escolhido está esgostado.");

                do
                {
                    lugares.Add(bd.LerInt("LugarID"));
                } while (bd.Consulta().Read());

                if (lugares.Distinct().Count() < quantidade)
                    throw new Exception("A quantidade de lugares disponíveis para o setor selecionado é de: " + lugares.Count + ", não será possível efetuar a reserva de " + quantidade + " lugares neste momento.");

                foreach (int lugarid in lugares)
                    this.Reservar(bd, apresentacaoID, setorID, lugarid, clienteID);

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ReservarQuantidade(int apresentacaoID, int setorID, int quantidade, int clienteID, int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                this.VerificarSaldo(clienteID, quantidade,assinaturaTipoIDBancoIngresso);

                List<int> lugares = new List<int>();

                bd.Consulta(string.Format(
                    @"SELECT TOP {0} 
						 i.LugarID
					FROM tAssinaturaBancoIngresso bi (NOLOCK)
					INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
					LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = bi.ID
                    LEFT JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.AssinaturaID
					WHERE bi.ApresentacaoID = {1} AND i.SetorID = {2} AND bir.ID IS NULL AND asn.AssinaturaTipoID = {3}
                    ORDER BY NEWID()",
                quantidade, apresentacaoID, setorID,assinaturaTipoIDBancoIngresso));

                if (!bd.Consulta().Read())
                    throw new Exception("O setor escolhido está esgostado.");

                do
                {
                    lugares.Add(bd.LerInt("LugarID"));
                } while (bd.Consulta().Read());

                if (lugares.Distinct().Count() < quantidade)
                    throw new Exception("A quantidade de lugares disponíveis para o setor selecionado é de: " + lugares.Count + ", não será possível efetuar a reserva de " + quantidade + " lugares neste momento.");

                foreach (int lugarid in lugares)
                    this.Reservar(bd, apresentacaoID, setorID, lugarid, clienteID,assinaturaTipoIDBancoIngresso);

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void Reservar(BD bd, int apresentacaoID, int setorID, int lugarID, int clienteID)
        {
            if (bd.Executar(string.Format(
                        @"INSERT INTO tAssinaturaBancoIngressoResgate (AssinaturaBancoIngressoID, TimeStamp, ClienteID) 
						SELECT 
							bi.ID, '{0}', {1}
						FROM tAssinaturaBancoIngresso bi (NOLOCK)
						INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
						LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = bi.ID
						WHERE i.ApresentacaoID = {2} AND i.SetorID = {3} AND i.LugarID = {4} AND bir.ID IS NULL",
                        DateTime.Now.ToString("yyyyMMddHHmmss"), clienteID, apresentacaoID, setorID, lugarID)) != 1)
                throw new IngressoException("Não foi possível efetuar a reserva deste ingresso, ele já está sendo resgatado por outro cliente, por favor tente novamente.");
        }

        private void Reservar(BD bd, int apresentacaoID, int setorID, int lugarID, int clienteID, int assinaturaTipoIDBancoIngresso)
        {
            if (bd.Executar(string.Format(
                        @"INSERT INTO tAssinaturaBancoIngressoResgate (AssinaturaBancoIngressoID, TimeStamp, ClienteID) 
						SELECT 
							bi.ID, '{0}', {1}
						FROM tAssinaturaBancoIngresso bi (NOLOCK)
						INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
						LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = bi.ID
                        LEFT JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.AssinaturaID
						WHERE i.ApresentacaoID = {2} AND i.SetorID = {3} AND i.LugarID = {4} AND bir.ID IS NULL AND asn.AssinaturaTipoID = {5}",
                        DateTime.Now.ToString("yyyyMMddHHmmss"), clienteID, apresentacaoID, setorID, lugarID,assinaturaTipoIDBancoIngresso)) != 1)
                throw new IngressoException("Não foi possível efetuar a reserva deste ingresso, ele já está sendo resgatado por outro cliente, por favor tente novamente.");
        }

        public void VerificarSaldo(int clienteID, int quantidade)
        {
            BD bd = new BD();
            try
            {
                int quantidadeCredito = Convert.ToInt32(bd.ConsultaValor("SELECT COUNT(bic.ID) AS Quantidade FROM tAssinaturaBancoIngressoCredito bic (NOLOCK) WHERE ClienteID =" + clienteID));
                if (quantidadeCredito == 0)
                    throw new Exception("Atenção: Seu saldo atual de créditos é 0 (zero), não será possível resgatar nenhum ingresso.");

                int quantidadeResgate = Convert.ToInt32(bd.ConsultaValor("SELECT COUNT(big.ID) AS Quantidade FROM tAssinaturaBancoIngressoResgate big (NOLOCK) WHERE ClienteID =" + clienteID));

                if (quantidadeCredito - quantidadeResgate + quantidade < 0)
                    throw new Exception("Atenção: Você já está resgatando " + quantidadeResgate + " ingresso(s), seu saldo de crédtido atual não permite que você resgate mais " + quantidade + " ingresso(s).");
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void VerificarSaldo(int clienteID, int quantidade, int assinaturaTipoIDBancoIngresso)
        {
            BD bd = new BD();
            try
            {
                int quantidadeCredito = Convert.ToInt32(bd.ConsultaValor(string.Format(@"SELECT COUNT(bic.ID) AS Quantidade FROM tAssinaturaBancoIngressoCredito bic (NOLOCK)
                                                                                                              INNER JOIN tAssinaturaAno ano(NOLOCK) ON ano.ID = bic.AssinaturaAnoID
                                                                                                              INNER JOIN tAssinatura asn(NOLOCK) ON asn.ID = ano.AssinaturaID 
                                                                                                              WHERE bic.ClienteID = {0} AND asn.AssinaturaTipoID = {1}", clienteID,assinaturaTipoIDBancoIngresso)));
                if (quantidadeCredito == 0)
                    throw new Exception("Atenção: Seu saldo atual de créditos é 0 (zero), não será possível resgatar nenhum ingresso.");

                int quantidadeResgate = Convert.ToInt32(bd.ConsultaValor(string.Format(@"SELECT COUNT(big.ID) AS Quantidade FROM tAssinaturaBancoIngressoResgate big (NOLOCK)
                                                                                                INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = big.AssinaturaBancoIngressoID
                                                                                                INNER JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.AssinaturaID
                                                                             WHERE big.ClienteID = {0} AND asn.AssinaturaTipoID = {1} ", clienteID,assinaturaTipoIDBancoIngresso)));

                if (quantidadeCredito - quantidadeResgate + quantidade < 0)
                    throw new Exception("Atenção: Você já está resgatando " + quantidadeResgate + " ingresso(s), seu saldo de crédtido atual não permite que você resgate mais " + quantidade + " ingresso(s).");
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<CarrinhoResgate> Resgate(int clienteID)
        {
            try
            {
                List<CarrinhoResgate> lista = new List<CarrinhoResgate>();

                string sql = string.Format(
                    @"
                        SELECT
                            bir.ID, ap.Horario, s.Nome AS Setor, i.Codigo
                        FROM tAssinaturaBancoIngressoResgate bir (NOLOCK)
                        INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = bir.AssinaturaBancoIngressoID
                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
                        INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                        INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
                        WHERE bir.ClienteID = {0}
                        ORDER BY ap.Horario, s.Nome, i.Codigo", clienteID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Seu carrinho de resgate está vazio.");

                do
                {
                    lista.Add(new CarrinhoResgate
                    {
                        ResgateID = bd.LerInt("ID"),
                        Horario = bd.LerDateTime("Horario").ToString("dd \\de MMMM").ToUpper(),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<CarrinhoResgate> Resgate(int clienteID,int assinaturaTipoIDBancoIngresso)
        {
            try
            {
                List<CarrinhoResgate> lista = new List<CarrinhoResgate>();

                string sql = string.Format(
                    @"
                        SELECT
                            bir.ID, ap.Horario, s.Nome AS Setor, i.Codigo
                        FROM tAssinaturaBancoIngressoResgate bir (NOLOCK)

                        INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = bir.AssinaturaBancoIngressoID
                        INNER JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.assinaturaID
                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
                        INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                        INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
                        WHERE bir.ClienteID = {0} AND asn.AssinaturaTipoID = {1}
                        ORDER BY ap.Horario, s.Nome, i.Codigo", clienteID,assinaturaTipoIDBancoIngresso);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Seu carrinho de resgate está vazio.");

                do
                {
                    lista.Add(new CarrinhoResgate
                    {
                        ResgateID = bd.LerInt("ID"),
                        Horario = bd.LerDateTime("Horario").ToString("dd \\de MMMM").ToUpper(),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void RemoverResgate(int resgateID, int clienteID)
        {
            try
            {
                if (bd.Executar(string.Format("DELETE FROM tAssinaturaBancoIngressoResgate WHERE ID = {0} AND ClienteID = {1}", resgateID, clienteID)) != 1)
                    throw new Exception("Atenção: Este ingresso não está vinculado a você, caso já tenha efetuado a ação de remoção, recarregue a tela e tente novamente.");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void RemoverResgates(int clienteID)
        {
            try
            {
                bd.Executar(string.Format("DELETE FROM tAssinaturaBancoIngressoResgate WHERE ClienteID = {1}", clienteID));
            }
            finally
            {
                bd.Fechar();
            }
        }


        public List<ApresentacaoProgramacao> ListarProgramacao(int AssinaturaTipoIDBancoIngresso, string anoAtual)
        {
            try
            {
                string sql = @"SELECT DISTINCT 
                                a.Nome AS Assinatura, ap.ID AS ApresentacaoID, ap.Horario, ap.Programacao 
                                FROM tAssinaturaBancoIngresso bi (NOLOCK)
                                INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
                                INNER JOIN tAssinatura a (NOLOCK) ON a.ID = bi.AssinaturaID
                                INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                                Where a.AssinaturaTipoID = " + AssinaturaTipoIDBancoIngresso + @" and an.Ano =  " + anoAtual + @"
                                ORDER BY ap.Horario, a.Nome                            
                            ";

                List<ApresentacaoProgramacao> lista = new List<ApresentacaoProgramacao>();
                string programacao = string.Empty;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    programacao = bd.LerString("Programacao");
                    programacao = string.IsNullOrEmpty(programacao) ? "--" : programacao.Length > 50 ? programacao.Substring(0, 49) + "..." : programacao + "...";

                    lista.Add(new ApresentacaoProgramacao()
                    {
                        Assinatura = bd.LerString("Assinatura"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Horario = bd.LerDateTime("Horario"),
                        Programacao = programacao,
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public ApresentacaoProgramacao Programacao(int id)
        {
            try
            {
                string sql = @"SELECT DISTINCT 
                                    a.Nome AS Assinatura, ap.ID AS ApresentacaoID, ap.Horario, ap.Programacao 
                                FROM tAssinaturaBancoIngresso bi (NOLOCK)
                                INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
                                INNER JOIN tAssinatura a (NOLOCK) ON a.ID = bi.AssinaturaID
                                WHERE ap.ID = " + id;

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Apresentação não localizada.");

                return new ApresentacaoProgramacao()
                {
                    Assinatura = bd.LerString("Assinatura"),
                    ApresentacaoID = bd.LerInt("ApresentacaoID"),
                    Horario = bd.LerDateTime("Horario"),
                    Programacao = bd.LerString("Programacao"),
                };

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void SalvarProgramacao(int id, string programacao)
        {
            this.Programacao(id);

            programacao = programacao.Replace("'", "''''");

            try
            {
                if (bd.Executar("UPDATE tApresentacao SET Programacao = '" + programacao + "' WHERE ID = " + id) != 1)
                    throw new Exception("Não foi possível localizar a apresentação selecionada.");
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Programação para inicio do resgate!!!
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="mes"></param>
        /// <param name="ano"></param>
        /// <param name="horaMinima"></param>
        /// <returns></returns>
        public List<IRLib.ClientObjects.EstruturaIDNome> CarregarProgramacoes(int dia, int mes, int ano, int horaMinima, int clienteID)
        {
            try
            {
                

                DateTime data = new DateTime(ano, mes, dia);
                DateTime dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (data == dataAtual)

                {
                    data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).AddHours(horaMinima);
                }
               


                string sql = string.Format(@"
                            SELECT 
                                DISTINCT ap.ID, IsNull(ap.Programacao, 'Programação não cadastrada') AS Programacao
                            FROM tAssinaturaBancoIngresso bi (NOLOCK) 
                            INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
                            WHERE ap.Horario BETWEEN '{0}' AND '{1}'", data.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"));

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível localizar nenhuma apresentação disponível para resgate.");

                List<IRLib.ClientObjects.EstruturaIDNome> lista = new List<IRLib.ClientObjects.EstruturaIDNome>();

                do
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Programacao"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.ClientObjects.EstruturaIDNome> CarregarProgramacoes(int dia, int mes, int ano, int horaMinima, int clienteID, int AssinaturaTipoIDBancoIngresso)
        {
            try
            {


                DateTime data = new DateTime(ano, mes, dia);
                DateTime dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (data == dataAtual)
                {
                    data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).AddHours(horaMinima);
                }



                string sql = string.Format(@"
                            SELECT 
                                DISTINCT ap.ID, IsNull(ap.Programacao, 'Programação não cadastrada') AS Programacao
                            FROM tAssinaturaBancoIngresso bi (NOLOCK) 
                            INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
                            INNER JOIN tAssinatura asn (NOLOCK) ON asn.ID = bi.AssinaturaID
                            WHERE asn.AssinaturaTipoID = {2} AND ap.Horario BETWEEN '{0}' AND '{1}'", data.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"),AssinaturaTipoIDBancoIngresso);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível localizar nenhuma apresentação disponível para resgate.");

                List<IRLib.ClientObjects.EstruturaIDNome> lista = new List<IRLib.ClientObjects.EstruturaIDNome>();

                do
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Programacao"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public static DataTable VerificarSeExisteBancoIngresso(int ingressoID, DataTable tabela)
        {
            BD bd = new BD();
            try
            {
                if (tabela.Rows.Count == 0)
                    return tabela;

                bd.Consulta(@"SELECT TOP 1 
                                ClienteID, c.Nome 
                        FROM tAssinaturaBancoIngresso bi (NOLOCK) 
                        INNER JOIN tCliente c (NOLOCK) ON c.ID = bi.ClienteID
                        WHERE bi.IngressoID = " + ingressoID);

                if (bd.Consulta().Read())
                {
                    try
                    {
                        int clienteID = bd.LerInt("ClienteID");
                        int clienteID_Compra = Convert.ToInt32(tabela.Rows[0]["ClienteID"]);

                        string obs = string.Empty;

                        if (clienteID == 0)
                            obs = "Este ingresso foi doado pelo cliente e ainda não foi resgatado.";
                        else if (clienteID != clienteID_Compra)
                            obs = "Este ingresso foi doado e posteriormente resgatado por: " + bd.LerString("Nome");

                        foreach (DataRow linha in tabela.Rows)
                            linha["Obs"] = obs + Environment.NewLine + linha["Obs"];
                    }
                    catch { }
                }

                return tabela;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public void AlterarCliente(BD bd, int assinaturaClienteID, int assinanteID, int clienteIDAntigo, int clienteID)
        {
            int antigoCliente = 0;
            int novoCliente = 0;

            //Desmembrando uma assinatura já desmembrada
            if (clienteIDAntigo > 0 && clienteID > 0)
            {
                antigoCliente = clienteIDAntigo;
                novoCliente = clienteID;
            }
            //Removendo desmembramento
            else if (clienteIDAntigo == 0 && clienteID > 0)
            {
                antigoCliente = assinanteID;
                novoCliente = clienteID;
            }
            //Desmembrando assinatura normal
            else if (clienteIDAntigo > 0 && clienteID == 0)
            {
                antigoCliente = clienteIDAntigo;
                clienteID = assinanteID;
            }
            else
                throw new Exception("Tentativa de desmembramento inválida.");

            BD bdAux = new BD();
            try
            {
                var obj = new
                {
                    IngressoID = 0,
                    BancoIngressoID = 0,
                    ClienteID = 0,
                    CreditoID = 0,
                    CreditoClienteID = 0,
                    ComprovanteID = 0,
                    ComprovanteClienteID = 0,
                };

                var lista = VendaBilheteria.ToAnonymousList(obj);

                string sql = string.Format(@"SELECT 
                                               DISTINCT bi.ID, bi.IngressoID, bi.ClienteID, bc.ID AS CreditoID, bc.ClienteID AS CreditoClienteID, bic.ID AS ComprovanteID, bic.ClienteID AS ComprovanteClienteID
                                            FROM tIngresso i (NOLOCK)
                                            INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.IngressoID = i.ID
                                            LEFT JOIN tAssinaturaBancoIngressoHistorico bh (NOLOCK) ON bh.AssinaturaBancoIngressoID = bi.ID
                                            LEFT JOIN tAssinaturaBancoIngressoCredito bc (NOLOCK) ON bh.AssinaturaBancoIngressoCreditoID = bc.ID
                                            LEFT JOIN tAssinaturaBancoIngressoComprovante bic (NOLOCK) ON bic.ID = bh.AssianturaBancoIngressoComprovanteID
                                            WHERE i.AssinaturaClienteID = {0} AND (bc.ID IS NULL OR bc.ClienteID = {1}) AND (bc.ID IS NULL OR bc.Utilizado = 'F')
                                            ", assinaturaClienteID, antigoCliente);

                if (!bdAux.Consulta(sql).Read())
                    return;

                do
                {
                    lista.Add(new
                 {
                     IngressoID = bdAux.LerInt("IngressoID"),
                     BancoIngressoID = bdAux.LerInt("ID"),
                     ClienteID = bdAux.LerInt("ClienteID"),
                     CreditoID = bdAux.LerInt("CreditoID"),
                     CreditoClienteID = bdAux.LerInt("CreditoClienteID"),
                     ComprovanteID = bdAux.LerInt("ComprovanteID"),
                     ComprovanteClienteID = bdAux.LerInt("ComprovanteClienteID"),
                 });
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();

                foreach (var item in lista)
                {
                    if (item.ClienteID == antigoCliente)
                        bd.Executar(string.Format("UPDATE tAssinaturaBancoIngresso SET ClienteID = {0} WHERE ID = {1} AND ClienteID = {2}", novoCliente, item.BancoIngressoID, antigoCliente));

                    if (item.CreditoClienteID > 0 && item.CreditoClienteID == antigoCliente)
                        bd.Executar(string.Format("UPDATE tAssinaturaBancoIngressoCredito SET ClienteID = {0} WHERE ID = {1} AND ClienteID = {2}", novoCliente, item.CreditoClienteID, antigoCliente));

                    if (item.ComprovanteID > 0 && item.ComprovanteClienteID == antigoCliente)
                        bd.Executar(string.Format("UPDATE tAssinaturaBancoIngressoComprovante SET ClienteID = {0} WHERE ID = {1} AND ClienteID = {2}", novoCliente, item.ComprovanteID, antigoCliente));
                }
            }
            finally
            {
                bdAux.Fechar();
            }
        }
    }

    public class AssinaturaBancoIngressoLista : AssinaturaBancoIngressoLista_B
    {

        public AssinaturaBancoIngressoLista() { }
    }

}
