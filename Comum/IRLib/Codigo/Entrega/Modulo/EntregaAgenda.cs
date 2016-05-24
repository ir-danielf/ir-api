/**************************************************
* Arquivo: EntregaAgenda.cs
* Gerado: 16/12/2010
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib
{

    public class EntregaAgenda : EntregaAgenda_B
    {

        public EntregaAgenda() { }

        public EntregaAgenda(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public bool Disponivel(DateTime dateTime, int EntregaControleID)
        {
            try
            {
                bool status = false;
                if (dateTime > DateTime.Now)
                {

                    int diaSemana = Convert.ToInt32(dateTime.DayOfWeek);
                    if (diaSemana == 0)
                        diaSemana = 7;

                    string sql = "";

                    sql = @"SELECT 
                                    DiaDaSemana 
                            FROM tDiasSemana (NOLOCK)
                            WHERE ControleEntregaID = " + EntregaControleID + " and DiaDaSemana = " + diaSemana;

                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                        status = true;

                    bd.FecharConsulta();

                    if (status)
                    {


                        string sql2 = "";

                        sql2 = @"SELECT 
                                    TOP 1 Isnull(QuantidadeEntregas,0) AS Disponivel, Isnull(QtdAgendada,0) AS Agendadas
                                FROM tEntregaControle (NOLOCK)
                                LEFT JOIN tEntregaAgenda (NOLOCK) On tEntregaAgenda.EntregaControleID = tEntregaControle.ID
                                WHERE tEntregaControle.ID = " + EntregaControleID + "AND tEntregaAgenda.Data = " + dateTime.ToString("yyyyMMdd");

                        bd.Consulta(sql2);

                        if (bd.Consulta().Read())
                            if (bd.LerInt("Disponivel") == bd.LerInt("Agendadas") || bd.LerInt("Disponivel") <= 0)
                                status = false;

                        bd.Fechar();
                    }
                }
                return status;
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

        public string String()
        {
            try
            {

                StringBuilder sql = new StringBuilder();
                sql = new StringBuilder();

                if (this.Control.ID <= 0)
                {
                    this.Inserir();
                }

                this.QtdAgendada.Valor++;

                sql.Append(@"UPDATE tEntregaAgenda
                               SET [QtdAgendada] = @001
                             WHERE [ID] = @002");

                sql.Replace("@001", this.QtdAgendada.ValorBD);
                sql.Replace("@002", this.Control.ID.ToString());

                return sql.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Inserir()
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEntregaAgenda(Data, EntregaControleID, QtdAgendada) ");
                sql.Append("VALUES ('@001',@002,@003);  SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.Data.ValorBD);
                sql.Replace("@002", this.EntregaControleID.ValorBD);
                sql.Replace("@003", this.QtdAgendada.ValorBD);

                int id = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                bool result = (id >= 1);

                if (result)
                {
                    this.Control.ID = id;
                    InserirControle("I");
                }

                bd.FinalizarTransacao();

                return result;

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

        public EstruturaEntregaAgenda CarregarEntrega(int EntregaControleID, string DataSelecionada)
        {
            try
            {
                EstruturaEntregaAgenda retorno = new EstruturaEntregaAgenda();
                if (DataSelecionada.Length > 0 && DataSelecionada != "0")
                {

                    retorno.dataPeriodoSelecionado.Dia = DateTime.ParseExact(DataSelecionada, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    retorno.dataPeriodoSelecionado.EntregaControleID = EntregaControleID;
                    retorno.Tipo = "A";
                }
                else
                {

                    retorno.EntregaControleID = EntregaControleID;
                    retorno.Tipo = "B";

                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool PodeSerAgendado(EstruturaEntregaAgenda entregaSelecionada)
        {
            switch (entregaSelecionada.Tipo)
            {
                case "A":
                    if (Disponivel(entregaSelecionada.dataPeriodoSelecionado.Dia, entregaSelecionada.dataPeriodoSelecionado.EntregaControleID))
                    {
                        DateTime data = entregaSelecionada.dataPeriodoSelecionado.Dia;
                        int ecID = entregaSelecionada.dataPeriodoSelecionado.EntregaControleID;
                        int eaID = 0;
                        int qtdAgendada = 0;

                        string sql2 = "";

                        sql2 = @"select ID,QtdAgendada
                                from  tEntregaAgenda (NOLOCK)
                                where EntregaControleID = " + ecID + " and Data = " + data.ToString("yyyyMMdd");


                        bd.Consulta(sql2);

                        if (bd.Consulta().Read())
                        {
                            eaID = bd.LerInt("ID");
                            qtdAgendada = bd.LerInt("QtdAgendada");

                        }
                        bd.Fechar();

                        this.Control.ID = eaID;
                        this.QtdAgendada.Valor = qtdAgendada;
                        this.Data.Valor = data;
                        this.EntregaControleID.Valor = ecID;


                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        public bool VerificaTaxas(List<int> listaEventoID, EstruturaEntregaAgenda entregaSelecionada, EstruturaClienteEndereco clienteEnderecoSelecionado)
        {
            try
            {
                bool retorno = true;

                var eventos = string.Join(",", listaEventoID.Select(n => n.ToString()).ToArray());

                int controleID = entregaSelecionada.EntregaControleID;
                string CEP = clienteEnderecoSelecionado.CEP == null ? "" : clienteEnderecoSelecionado.CEP;
                string strCEP = "and (CepInicial<" + CEP + "and CepFinal >" + CEP + ")";


                string sql = @"select tEntregaControle.EntregaID , tEntregaControle.ID as EntregaControleID , 
                        tEntrega.Nome, PrazoEntrega,  
                        tEntrega.ProcedimentoEntrega, Tipo, 
                        tEntrega.DiasTriagem,tEntregaControle.Valor,ISNULL(tEntregaPeriodo.Nome,'') as Periodo
                        from tEventoEntregaControle
                        inner join tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        inner join tEntregaAreaCep ON tEntregaControle.EntregaAreaID = tEntregaAreaCep.EntregaAreaID
                        left join tEntregaPeriodo ON tEntregaControle.PeriodoID = tEntregaPeriodo.ID
                        where tEventoEntregaControle.EventoID IN (" + eventos + @") and tEntregaControle.ID = " + controleID + @" 
                        and tEntrega.Tipo in ('A','N') " + (CEP.Length > 0 ? strCEP : "") +
                           @"group by tEntregaControle.EntregaID,tEntregaControle.ID  , tEntrega.Nome, PrazoEntrega,  
                        tEntrega.ProcedimentoEntrega, Tipo, tEntrega.DiasTriagem,tEntregaControle.Valor,tEntregaPeriodo.Nome
                        having COUNT(Distinct tEventoEntregaControle.EventoID) = " + listaEventoID.Count + @"
                        order by tEntregaControle.EntregaID";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = false;
                }

                bd.Fechar();


                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int BuscarID(int entregaControleID, DateTime data)
        {
            try
            {
                string sql = @"SELECT ID,QtdAgendada
                                FROM  tEntregaAgenda  (NOLOCK)
                                WHERE EntregaControleID = " + entregaControleID + " and Data = " + data.ToString("yyyyMMdd");

                return Convert.ToInt32(bd.ConsultaValor(sql));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<PeridoAgenda> CarregarAgendaAindaNaoCriada(int EntregaID, string CEP, List<PeridoAgenda> naoEncontradas, List<int> eventosID, DateTime dataEventoMaisProximo)
        {
            try
            {
                List<int> DiasDaSemana = new List<int>();


                List<string> PeriodosDistintos = naoEncontradas.Select(c => c.Periodo).Distinct().ToList();

                bd.Consulta(string.Format(@"
                                SELECT DiaDaSemana, ec.ID AS EntregaControleID
                                FROM tEntrega e (NOLOCK)
                                INNER JOIN tEntregaControle ec (NOLOCK) ON ec.EntregaID = e.ID
                                INNER JOIN tEventoEntregaControle eec (NOLOCK) ON eec.EntregaControleID = ec.ID
                                INNER JOIN tDiasSemana s (NOLOCK) ON s.ControleEntregaID = ec.ID
                                INNER JOIN tEntregaAreaCep tac (NOLOCK)  ON ec.EntregaAreaID = tac.EntregaAreaID
                                WHERE e.ID = {0} AND tac.CepInicial <= '{1}' AND tac.CepFinal >= '{1}' AND eec.EventoID IN ({2})
                                GROUP BY DiaDaSemana, ec.ID
                                HAVING COUNT(DISTINCT eec.EventoID) = {3}
                                ORDER BY  EntregaControleID, DiaDaSemana 
                            ", EntregaID, CEP, Utilitario.ArrayToString(eventosID.ToArray()), eventosID.Count));

                while (bd.Consulta().Read())
                {
                    var datas = naoEncontradas
                        .Where(c => c.EntregaControleID == bd.LerInt("EntregaControleID")
                            && (Convert.ToInt32(c.Data.DayOfWeek) == bd.LerInt("DiaDaSemana") ||
                            (Convert.ToInt32(c.Data.DayOfWeek) == 0 && bd.LerInt("DiaDaSemana") == 7))).ToList();

                    if (datas == null)
                        continue;

                    foreach (var data in datas.Where(c => c.Data.Date < dataEventoMaisProximo.Date))

                        data.Disponivel = true;
                }

                return naoEncontradas;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class EntregaAgendaLista : EntregaAgendaLista_B
    {

        public EntregaAgendaLista() { }

        public EntregaAgendaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
