using CTLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace IRLib
{

    public sealed class Utilitario
    {
        public const string FormatoMoeda = "###,##0.00";
        public const string FormatoPorcentagem1Casa = "#00.0";
        public const string FormatoDataHora = "dd/MM/yyyy HH:mm";
        public const string FormatoData = "dd/MM/yyyy";

        public static string AplicaFormatoMoeda(decimal valor)
        {
            return string.Format("{0:N}", valor);
        }

        public static DateTime ProximoDiaDeSemana(DateTime datahora, DayOfWeek diadasemana)
        {
            while (datahora.DayOfWeek != diadasemana)
            {
                datahora = datahora.AddDays(1);
            }

            return datahora;
        }

        public static DateTime String2DateTime(string datahora)
        {

            DateTime retorno;

            if (datahora.Trim() == "") //se nao tiver data vai retornar DateTime.MinValue
                return System.DateTime.MinValue;
            else if (datahora.Length >= 8)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                string hora = "0";
                string minuto = "0";
                if (datahora.Length >= 12)
                { //o metodo trata Data ou DataHora...
                    hora = datahora.Substring(8, 2);
                    minuto = datahora.Substring(10, 2);
                }

                try
                {
                    retorno = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);
                }
                catch
                {
                    throw new Exception("Data inválida!");
                }

                return retorno;

            }
            else
            {
                return System.DateTime.MinValue;
            }

        }

        public static string DataHoraBDParaDataHoraLegivel(string datahora)
        {
            string ano = datahora.Substring(0, 4);
            string mes = datahora.Substring(4, 2);
            string dia = datahora.Substring(6, 2);
            string hora = datahora.Substring(8, 2);
            string minuto = datahora.Substring(10, 2);
            string resp = dia + "/" + mes + "/" + ano + " " + hora + ":" + minuto;
            return resp;
        }

        public static DataTable EstruturaPagamentoPacrelas()
        {
            DataTable tabela = new DataTable("PagamentoPacrelas");
            tabela.Columns.Add("FormaPagamentoID", typeof(int));
            tabela.Columns.Add("Pagamento", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaPorcentagemIngressosStatus()
        {
            DataTable tabela = EstruturaQuantidadeIngressosStatus();
            tabela.Columns.Add("Porcentagem", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaQuantidadeIngressosStatus()
        {
            DataTable tabela = new DataTable("QuantidadeIngressosStatus");
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("CortesiaID", typeof(int));
            return tabela;
        }

        public static DataTable EstruturaQuantidadeIngressosPorEvento()
        {
            DataTable tabela = new DataTable("QuantidadeIngressosPorEvento");
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("QuantidadeIngressos", typeof(int));
            return tabela;
        }

        public static DataTable EstruturaValorIngressosPorEvento()
        {
            DataTable tabela = new DataTable("ValorIngressosPorEvento");
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("ValorIngressos", typeof(int));
            return tabela;
        }

        public static void AcrescentaCampoPorcentagem(ref DataTable tabela)
        {
            tabela.Columns.Add("Porcentagem", typeof(decimal));
        }

        public static DataTable EstruturaVendasGerenciais()
        {
            DataTable tabela = new DataTable("VendasGerenciais");
            tabela.Columns.Add("VariacaoLinhaID", typeof(int));
            tabela.Columns.Add("VariacaoLinha", typeof(string));
            // Quantidade e Valor
            tabela.Columns.Add("Qtd Total", typeof(decimal));
            tabela.Columns.Add("R$ Total", typeof(string));
            tabela.Columns.Add("Qtd Vend", typeof(decimal));
            tabela.Columns.Add("R$ Vend", typeof(string));
            tabela.Columns.Add("Qtd Canc", typeof(decimal));
            tabela.Columns.Add("R$ Canc", typeof(string));
            tabela.Columns.Add("R$ Conveniência", typeof(decimal));
            tabela.Columns.Add("R$ Comissão", typeof(decimal));
            tabela.Columns.Add("Total", typeof(decimal));


            return tabela;
        }

        public static DataTable EstruturaTaxaEntrega()
        {
            DataTable tabela = new DataTable("tabelaTaxaEntrega");
            tabela.Columns.Add("Taxa de Entrega", typeof(string));
            tabela.Columns.Add("Valor", typeof(string));

            return tabela;
        }

        public static DataTable EstruturaCaixaParaConferencia()
        {
            DataTable tabela = new DataTable("CaixaParaConferencia");
            tabela.Columns.Add("FormaPagamentoID", typeof(int));
            tabela.Columns.Add("Formas de Recebimento", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(string));
            tabela.Columns.Add("R$ Ingressos", typeof(string));
            tabela.Columns.Add("R$ Conveniência", typeof(string));
            tabela.Columns.Add("R$ Total", typeof(string));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("CanalID", typeof(int));
            return tabela;
        }

        public static DataTable EstruturaEventoApresentacao()
        {
            // SetorID, Setor, Lotacao, Cortesia, Pagantes, Publico, Ocupacao
            DataTable tabela = new DataTable("EventoApresentacao");
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("ApresentaçãoID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Ingressos", typeof(int));
            tabela.Columns.Add("Faturamento", typeof(string));
            tabela.Columns.Add("Preço Médio", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaEventoApresentacaoSetor()
        {
            // SetorID, Setor, Lotacao, Cortesia, Pagantes, Publico, Ocupacao
            DataTable tabela = new DataTable("EventoApresentacaoSetor");
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("ApresentaçãoID", typeof(int));
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Ingressos", typeof(int));
            tabela.Columns.Add("Faturamento", typeof(string));
            tabela.Columns.Add("Preço Médio", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaSetorLotacao()
        {
            // SetorID, Setor, Lotacao, Cortesia, Pagantes, Publico, Ocupacao
            DataTable tabela = new DataTable("SetorLotacao");
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Lotação", typeof(int));
            tabela.Columns.Add("Bloqueados", typeof(int));
            tabela.Columns.Add("Cortesia", typeof(int));
            tabela.Columns.Add("Pagantes", typeof(int));
            tabela.Columns.Add("Público", typeof(int));
            tabela.Columns.Add("Pré-Impressos", typeof(int));
            tabela.Columns.Add("Disponível", typeof(int));
            tabela.Columns.Add("% ocupação", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaArrecadacaoDespesa()
        {
            DataTable tabela = new DataTable("ArrecadacaoDespesa");
            //			tabela.Columns.Add("PorValor", typeof(bool));
            tabela.Columns.Add("Descricao", typeof(string));
            tabela.Columns.Add("Valor/%", typeof(decimal));
            tabela.Columns.Add("Despesa", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaBorderoApresentacaoSetorPreco()
        {
            DataTable tabela = new DataTable("BorderoApresentacaoSetorPreco");
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("CortesiaID", typeof(int));
            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preço", typeof(string));
            tabela.Columns.Add("Valor", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Faturamento", typeof(string));
            tabela.Columns.Add("Cortesia", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaBorderoSetorPreco()
        {
            DataTable tabela = new DataTable("BorderoSetorPreco");
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("CortesiaID", typeof(int));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preço", typeof(string));
            tabela.Columns.Add("Valor", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Faturamento", typeof(string));
            tabela.Columns.Add("Cortesia", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaBorderoPorFormaRecebimento()
        {
            DataTable tabela = new DataTable("BorderoPorFormaRecebimento");
            tabela.Columns.Add("FormaPagamentoID", typeof(int));
            tabela.Columns.Add("Formas de Recebimento", typeof(string));
            tabela.Columns.Add("Total Qtd", typeof(decimal));
            tabela.Columns.Add("Total Valor", typeof(string));
            tabela.Columns.Add("Canais Prop Qtd", typeof(decimal));
            tabela.Columns.Add("Canais Prop Valor", typeof(string));
            tabela.Columns.Add("Outros Canais Qtd", typeof(decimal));
            tabela.Columns.Add("Outros Canais Valor", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaBorderoPorFormaPagamento()
        {
            DataTable tabela = new DataTable("BorderoPorFormaPagamento");
            tabela.Columns.Add("FormaPagamentoID", typeof(int));
            tabela.Columns.Add("FormaPagamento", typeof(string));
            tabela.Columns.Add("Total Qtd", typeof(decimal));
            tabela.Columns.Add("Total Valor", typeof(decimal));
            tabela.Columns.Add("Bilheteria Qtd", typeof(decimal));
            tabela.Columns.Add("Bilheteria Valor", typeof(decimal));
            tabela.Columns.Add("Call Center Qtd", typeof(decimal));
            tabela.Columns.Add("Call Center Valor", typeof(decimal));
            tabela.Columns.Add("Internet Qtd", typeof(decimal));
            tabela.Columns.Add("Internet Valor", typeof(decimal));
            tabela.Columns.Add("PDV Qtd", typeof(decimal));
            tabela.Columns.Add("PDV Valor", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaVendasDetalhe()
        {
            DataTable tabela = new DataTable("VendasDetalhe");
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Senha", typeof(string));
            tabela.Columns.Add("DataVenda", typeof(string));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Setor/Produto", typeof(string));
            tabela.Columns.Add("Ingresso", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("Entrega", typeof(decimal));
            tabela.Columns.Add("Conveniência", typeof(decimal));
            tabela.Columns.Add("Total", typeof(decimal));
            return tabela;
        }

        public static DataTable EstruturaReimpressos()
        {
            DataTable tabela = new DataTable("VendasDetalhe");
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Senha", typeof(string));
            tabela.Columns.Add("DataVenda", typeof(string));
            tabela.Columns.Add("Evento", typeof(string));
            //			tabela.Columns.Add("Apresentação", typeof(string));
            //			tabela.Columns.Add("Setor/Produto", typeof(string)); 
            //			tabela.Columns.Add("Ingresso", typeof(string)); 
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Motivo", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaPreImpressoCanal()
        {
            DataTable tabela = new DataTable("PreImpressoCanalFinal");

            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preço", typeof(string));
            tabela.Columns.Add("Unitário", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(decimal));
            tabela.Columns.Add("Valor", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaPreImpressoEvento()
        {
            DataTable tabela = new DataTable("PreImpressoEventoFinal");

            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Canal", typeof(string));
            tabela.Columns.Add("Loja", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(decimal));
            tabela.Columns.Add("Valor", typeof(string));
            return tabela;
        }

        public static DataTable EstruturaEstatisticaBloqueios()
        {
            DataTable tabela = new DataTable("EstatisticaBloqueios");

            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Bloqueio", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(string));
            return tabela;
        }

        public static string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }

        public static bool diaUtil(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return true;
            }
        }

        public static int DiasUteisPeriodo(DateTime inicio, DateTime fim)
        {
            // Buscar os feriados no período informado.
            //string 
            // Popular lista de feriados.
            List<DateTime> feriados = new List<DateTime>();

            feriados = BuscaFeriados(inicio, fim);
            feriados.Add(DateTime.Now.AddDays(1));

            int diasUteisPeriodo = 0;
            DateTime dataAtual = inicio;

            while (dataAtual <= fim)
            {
                if (diaUtil(dataAtual))
                {
                    // Data atual está dentro da lista de feriado.
                    bool ehFeriado = feriados.FindAll(
                            delegate(DateTime d)
                            {
                                return d.Day == dataAtual.Day && d.Month == dataAtual.Month && d.Year == dataAtual.Year;
                            }
                        ).Count > 0;
                    if (!ehFeriado)
                        diasUteisPeriodo++;
                }
                dataAtual = dataAtual.AddDays(1);
            }

            //Console.WriteLine("=========== {0} dias úteis ===========", diasUteisPeriodo.ToString());
            return diasUteisPeriodo;

        }

        public static List<DateTime> BuscaFeriados(DateTime inicio, DateTime fim)
        {
            try
            {
                BD bd = new BD();
                List<DateTime> retorno = new List<DateTime>();

                System.Text.StringBuilder sql = new System.Text.StringBuilder();

                sql.Append("SELECT Data FROM tFeriado (NOLOCK) WHERE Data >= '" + inicio + "' AND Data <= '" + fim + "'");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(bd.LerDateTime("Data"));
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        public static DataTable UnirTabelas(DataTable tabela1, DataTable tabela2)
        {

            try
            {

                foreach (DataRow linha in tabela2.Rows)
                    tabela1.ImportRow(linha);

                return tabela1;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ArrayToString(DataRow[] linhas)
        {

            try
            {

                string arrayString;

                if (linhas == null)
                {
                    throw new Exception("A lista esta nula.");

                }
                else if (linhas.Length > 0)
                {

                    StringBuilder arrayBuffer = new StringBuilder();

                    foreach (DataRow linha in linhas)
                        arrayBuffer.Append((int)linha[0] + ",");

                    arrayString = arrayBuffer.ToString();
                    arrayString = arrayString.Substring(0, arrayString.Length - 1);

                }
                else
                {
                    throw new Exception("A lista esta vazia.");
                }

                return arrayString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ArrayToString(ArrayList[] arrayLists)
        {

            try
            {

                string arrayString;

                if (arrayLists == null)
                    throw new Exception("Não há lista alguma.");

                StringBuilder arrayBuffer = new StringBuilder();

                foreach (ArrayList arrayList in arrayLists)
                {
                    arrayBuffer.Append(ArrayToString(arrayList) + ",");
                }
                arrayString = arrayBuffer.ToString();
                arrayString = arrayString.Substring(0, arrayString.Length - 1);

                return arrayString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ArrayToString(ArrayList arrayList)
        {

            try
            {

                string arrayString;

                if (arrayList == null)
                {
                    throw new Exception("A lista esta nula.");

                }
                else if (arrayList.Count > 0)
                {

                    arrayList.TrimToSize();

                    int[] array = (int[])arrayList.ToArray(typeof(int));

                    StringBuilder arrayBuffer = new StringBuilder();

                    for (int i = 0; i < array.Length; i++)
                        arrayBuffer.Append(array[i] + ",");

                    arrayString = arrayBuffer.ToString();
                    arrayString = arrayString.Substring(0, arrayString.Length - 1);

                }
                else
                {
                    throw new Exception("A lista esta vazia.");
                }

                return arrayString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ArrayToString(int[] array)
        {
            if (array == null || array.Length.Equals(0))
                return string.Empty;

            StringBuilder arrayBuffer = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
                arrayBuffer.Append(array[i] + ",");

            return arrayBuffer.ToString().Substring(0, arrayBuffer.Length - 1);
        }

        public static string ArrayToString(Array array)
        {

            try
            {

                string arrayString;

                if (array == null)
                {
                    throw new Exception("A lista esta nula.");

                }
                else if (array.Length > 0)
                {

                    StringBuilder arrayBuffer = new StringBuilder();

                    for (int i = 0; i < array.Length; i++)
                    {
                        DataRow linha = array.GetValue(i) as DataRow;
                        arrayBuffer.Append((int)linha[0] + ",");
                    }

                    arrayString = arrayBuffer.ToString();
                    arrayString = arrayString.Substring(0, arrayString.Length - 1);

                }
                else
                {
                    throw new Exception("A lista esta vazia.");
                }

                return arrayString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ArrayToString(string[] array)
        {

            try
            {

                string arrayString;

                if (array == null)
                {
                    throw new Exception("A lista está nula.");

                }
                else if (array.Length > 0)
                {

                    StringBuilder arrayBuffer = new StringBuilder();

                    for (int i = 0; i < array.Length; i++)
                        arrayBuffer.Append(array[i] + ",");

                    arrayString = arrayBuffer.ToString();
                    arrayString = arrayString.Substring(0, arrayString.Length - 1);

                }
                else
                {
                    throw new Exception("A lista está vazia.");
                }

                return arrayString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string FormatoSemanaDataHora(string datahora)
        {
            if (datahora.Trim() == "")
                return "";

            else if (datahora.Length >= 12)
            {
                string ano = datahora.Substring(0, 4);
                string mes = datahora.Substring(4, 2);
                string dia = datahora.Substring(6, 2);
                DateTime data = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia));
                string diaSemanaImp;
                switch (data.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        diaSemanaImp = "segunda,";
                        break;
                    case DayOfWeek.Tuesday:
                        diaSemanaImp = "terça,     ";
                        break;
                    case DayOfWeek.Wednesday:
                        diaSemanaImp = "quarta,   ";
                        break;
                    case DayOfWeek.Thursday:
                        diaSemanaImp = "quinta,    ";
                        break;
                    case DayOfWeek.Friday:
                        diaSemanaImp = "sexta,     ";
                        break;
                    case DayOfWeek.Saturday:
                        diaSemanaImp = "sábado,  ";
                        break;
                    default:
                        diaSemanaImp = "domingo,";
                        break;
                }
                string hora = datahora.Substring(8, 2);
                string minuto = datahora.Substring(10, 2);

                string resp = diaSemanaImp + " " + dia + "/" + mes + "/" + ano + " " + hora + ":" + minuto;

                return resp;

            }
            else
            {
                return "";
            }

        }

        public static string DataRowsToString(DataRowCollection linhas, string coluna)
        {
            System.Text.StringBuilder retorno = new StringBuilder();

            foreach (DataRow item in linhas)
                retorno.Append(item[coluna] + ",");

            if (retorno.Length > 0)
                return retorno.ToString().Substring(0, retorno.Length - 1);
            else
                return string.Empty;


        }

        public static bool ehInteiro(object valor)
        {
            try
            {
                int.Parse(valor.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsDateTime(string value)
        {
            try
            {
                System.DateTime.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool IsDateTime(string value, string format)
        {
            try
            {
                System.DateTime.ParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool IsDecimal(string number)
        {
            try
            {
                Decimal.Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCPF(string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;
                cpf = cpf.Trim();
                cpf = cpf.Replace("'", "").Replace("-", "").Replace(",", "").Replace(".", "").Replace(" ", "").Replace("/", "").Replace("*", "");
                if (cpf.Length != 11)
                    return false;
                tempCpf = cpf.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                resto = soma % 11; if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11; if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto; digito = digito + resto.ToString();
                return cpf.EndsWith(digito);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsInt(string number)
        {
            try
            {
                int.Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            try
            {
                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                if (cnpj.Length != 14)
                    return false;

                tempCnpj = cnpj.Substring(0, 12);
                soma = 0;

                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                resto = (soma % 11);

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;

                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto = (soma % 11);

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cnpj.EndsWith(digito);
            }
            catch
            {
                return false;
            }

        }

        public static bool IsEmail(string email)
        {
            System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return oRegex.IsMatch(email);
        }

        public static bool IsLetrasNumeros(string texto)
        {
            System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            return !oRegex.IsMatch(texto);
        }

        public static bool IsNumeros(string texto)
        {
            System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !oRegex.IsMatch(texto);
        }

        public static bool IsLetras(string texto)
        {
            System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex("[^a-zA-Z]");
            return !oRegex.IsMatch(texto);
        }

        public static string LimparTitulo(string texto)
        {
            if (texto.StartsWith("'") || texto.StartsWith("!") || texto.StartsWith(".") || texto.StartsWith("_"))
            {
                do
                {
                    texto = texto.Remove(0, 1);
                } while (texto.StartsWith("'") || texto.StartsWith("!") || texto.StartsWith(".") || texto.StartsWith("_"));
            }
            if (texto.EndsWith("'") || texto.EndsWith("."))
            {
                do
                {
                    texto = texto.Remove(texto.Length - 1, 1);
                } while (texto.EndsWith("'") || texto.EndsWith("."));
            }
            return texto;
        }

        public static string HTTPGetPage(string valores)
        {
            /*
             * EXEMPLO DE UTILIZAÇÃO
            string fonte = Util.HTTPGetPage("https://comercio.locaweb.com.br/comercio.comp?merchid=ingressorapido&price=0001&damount=Easasd&tid=73489720018193711001&orderid=1&order=Teste&bin=455187&authenttype=1&visa_antipopup=0&PosicaoDadosVisanet=0");
            Response.Write(fonte);                        
            */

            WebRequest mywebReq = null;
            WebResponse mywebResp = null;
            StreamReader sr = null;
            string strHTML;

            try
            {
                mywebReq = WebRequest.Create(valores);
                mywebResp = mywebReq.GetResponse();
                sr = new StreamReader(mywebResp.GetResponseStream());
                strHTML = sr.ReadToEnd();
                return strHTML;
            }
            catch (WebException)
            {
                throw;
            }
            finally
            {
                if (mywebResp != null)
                    mywebResp.Close();
                if (sr != null)
                {
                    sr.Close();

                    sr.Dispose();
                }
            }
        }

        public static int CompararSemAcentos(string a, string b)
        {
            CompareInfo ci = new CultureInfo("pt-BR").CompareInfo;
            CompareOptions co = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;
            return ci.Compare(a.Trim(), b.Trim(), co);
        }

        public static bool isValidUrl(string url)
        {
            string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public static string RetornaURLAjax(string urlCompleta, string pagina)
        {
            string retorno = string.Empty;

            string[] paginaRemover = pagina.Split('/');

            retorno = urlCompleta.Substring(0, urlCompleta.IndexOf(paginaRemover[paginaRemover.Length - 2]));

            return retorno;
        }

        public static string RetornaURL(string urlCompleta, string pagina)
        {
            string retorno = string.Empty;

            string[] paginaRemover = pagina.Split('/');

            retorno = urlCompleta.Substring(0, urlCompleta.IndexOf(paginaRemover[paginaRemover.Length - 1]));

            return retorno;
        }

        public static string HTTPPostXML(string url, string xml, string usuario, string senha)
        {

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            request.ContentType = "text/xml";
            request.ContentLength = byteArray.Length;

            string credentials = String.Format("{0}:{1}", usuario, senha);
            byte[] bytes = Encoding.ASCII.GetBytes(credentials);
            string base64 = Convert.ToBase64String(bytes);
            string authorization = String.Concat("Basic ", base64);
            request.Headers.Add("Authorization", authorization);

            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            string responseFromServer = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                    responseFromServer = reader.ReadToEnd();
            }

            dataStream.Close();
            return responseFromServer;
        }

        public static bool IsCartaoValido(string cartao)
        {
            if (cartao.Length < 14 || cartao.Length > 19)
                return false;

            char[] digitosCartao = cartao.ToCharArray();
            bool flagCheck = false;
            int digitosSoma = 0;

            for (int i = digitosCartao.Length - 1; i >= 0; i--)
            {
                if (digitosCartao[i] >= 48 && digitosCartao[i] <= 57)
                {
                    int digitoCartao = (int)Char.GetNumericValue(digitosCartao[i]);

                    if (flagCheck)
                    {
                        digitoCartao *= 2;
                        digitoCartao = (digitoCartao > 9) ? digitoCartao - 9 : digitoCartao;
                    }

                    digitosSoma += digitoCartao;
                    flagCheck = !flagCheck;
                }
                else
                {
                    return false;
                }
            }

            return (digitosSoma % 10) == 0;
        }
    }

    public static class ExtensionsIRLib
    {
        public static string AsCEP(this object valor)
        {
            try
            {
                if (valor == DBNull.Value)
                    return string.Empty;


                string val = valor.ToString().RemoveAcentos();

                if (val.Length >= 8 && !val.Contains("-"))
                    val = val.Insert(5, "-");

                return val;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ToCEP(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            valor = valor.Replace("-", string.Empty);

            if (valor.Length != 8)
                throw new Exception("O campo CEP deve estar em um formato correto.");

            return valor;
        }

        public static string ToDB(this string valor)
        {
            return valor.Replace("'", "''");

        }
    }
}