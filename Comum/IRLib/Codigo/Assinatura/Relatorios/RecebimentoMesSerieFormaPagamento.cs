using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IRLib.Assinaturas.Relatorios
{
    public class RecebimentoMesSerieFormaPagamento
    {
        public enum EnumFiltro
        {
            [Description("Mensal")]
            Mensal = 1,
            [Description("Por período")]
            Periodo = 2,
            [Description("Total por assinatura")]
            Assinatura = 3,
            [Description("Por Data")]
            PorData = 4,
        }

        public static Dictionary<int, string> ListaFiltros()
        {
            return Utils.Enums.EnumToIntDictionary<EnumFiltro>();
        }

        private BD bd = new BD();

        private List<RecebimentoMes> MontarMeses()
        {
            List<RecebimentoMes> meses = new List<RecebimentoMes>();
            for (int i = 1; i <= 12; i++)
            {
                meses.Add(new RecebimentoMes()
                {
                    Mes = i,
                    MesExibicao = new DateTime(2000, i, 1).ToString("MMMM"),
                    ListaRecebimentoMesSerieFormaPagamento = new List<RecebimentoSerieFormaPagamento>(),
                });
            }
            return meses;
        }

        private List<RecebimentoMes> MontarMeses(DateTime dataInicial, DateTime dataFinal)
        {
            List<RecebimentoMes> meses = new List<RecebimentoMes>();
            int MesAtual = dataInicial.Month;
            int AnoAtual = dataInicial.Year;

            int MesAnoAtual = Convert.ToInt32(AnoAtual + "" + (MesAtual > 9 ? "" + MesAtual : "0" + MesAtual));
            int MesAnoFinal = Convert.ToInt32(dataFinal.Year + "" + (dataFinal.Month > 9 ? "" + dataFinal.Month : "0" + dataFinal.Month));


            do
            {
                meses.Add(new RecebimentoMes()
                {
                    Mes = MesAtual,
                    Ano = AnoAtual,
                    MesExibicao = new DateTime(2000, MesAtual, 1).ToString("MMMM"),
                    ListaRecebimentoMesSerieFormaPagamento = new List<RecebimentoSerieFormaPagamento>(),
                });

                MesAtual++;

                if (MesAtual > 12)
                {
                    MesAtual = 1;
                    AnoAtual++;
                }

                MesAnoAtual = Convert.ToInt32(AnoAtual + "" + (MesAtual > 9 ? "" + MesAtual : "0" + MesAtual));

            } while (MesAnoFinal >= MesAnoAtual);


            return meses;
        }

        private List<RecebimentoMes> LimparMeses(List<RecebimentoMes> Meses)
        {
            return Meses.Where(c => c.ListaRecebimentoMesSerieFormaPagamento.Count > 0).ToList();
        }

        private List<RecebimentoMes> LimparMeses(List<RecebimentoMes> Meses, DateTime dataInicial, DateTime dataFinal)
        {
            return Meses.Where(c => c.ListaRecebimentoMesSerieFormaPagamento.Count > 0).ToList();
        }

        private List<RecebimentoPeriodo> MontarPeriodos(int assinaturaTipoID)
        {

            List<RecebimentoPeriodo> lista = new List<RecebimentoPeriodo>();
            lista.Add(new RecebimentoPeriodo() { Periodo = AssinaturaTipo.EnumPeriodos.Renovacao });
            lista.Add(new RecebimentoPeriodo() { Periodo = AssinaturaTipo.EnumPeriodos.Troca });
            lista.Add(new RecebimentoPeriodo() { Periodo = AssinaturaTipo.EnumPeriodos.Aquisicao });
            lista.Add(new RecebimentoPeriodo() { Periodo = AssinaturaTipo.EnumPeriodos.ForaDePeriodo });

            return lista;

        }

        private List<RecebimentoPeriodo> LimparPeriodos(List<RecebimentoPeriodo> Periodos)
        {
            return Periodos.Where(c => c.ListaRecebimentoMesSerieFormaPagamento.Count > 0).ToList();
        }

        public object RelatorioRecebimentoMes(int assinaturaTipoID, int temporada, EnumFiltro filtro)
        {
            try
            {
                switch (filtro)
                {
                    case EnumFiltro.Mensal:
                    case EnumFiltro.Periodo:
                        bd.Consulta("EXEC TotalFinanceiroPorSerieFormaPagamentoData " + assinaturaTipoID + ", 0, '" + temporada + "'");
                        break;
                    case EnumFiltro.Assinatura:
                        bd.Consulta("EXEC TotalFinanceiroPorSerieFormaPagamentoData " + assinaturaTipoID + ", 1, '" + temporada + "'");
                        break;
                    default:
                        throw new Exception("Filtro inválido!");
                }

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem resultados a serem exibidos.");

                switch (filtro)
                {
                    case EnumFiltro.Mensal:
                        return this.FiltroMensal();
                    case EnumFiltro.Periodo:
                        return this.FiltroPeriodo(assinaturaTipoID);
                    case EnumFiltro.Assinatura:
                        return this.Total();
                    default:
                        throw new Exception("Filtro inválido!");
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public object RelatorioRecebimentoPeriodo(int assinaturaTipoID, int temporada, EnumFiltro filtro, string DataInicial, string DataFinal)
        {
            try
            {



                DateTime dataInicial = DateTime.MaxValue;
                DateTime dataFinal = DateTime.MaxValue;


                if (filtro == EnumFiltro.Mensal)
                {
                    if (!string.IsNullOrEmpty(DataInicial) && Utilitario.IsDateTime(DataInicial, "dd/MM/yyyy"))
                    {
                        var mesInicial = DataInicial.Split('/');
                        mesInicial[0] = "01";
                        dataInicial = DateTime.ParseExact(mesInicial[0] + mesInicial[1] + mesInicial[2], "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    if (!string.IsNullOrEmpty(DataFinal) && Utilitario.IsDateTime(DataFinal, "dd/MM/yyyy"))
                    {
                        var mesFinal = DataFinal.Split('/');
                        mesFinal[0] = "01";
                        string anoNovo = (Convert.ToInt32(mesFinal[2]) + 1).ToString();
                        mesFinal[2] = mesFinal[1] == "12" ? anoNovo : mesFinal[2];

                        int mesNovo = Convert.ToInt32(mesFinal[1]) + 1;
                        mesFinal[1] = mesFinal[1] == "12" ? "01" : mesNovo > 9 ? mesNovo.ToString() : "0" + mesNovo;

                        dataFinal = DateTime.ParseExact(mesFinal[0] + mesFinal[1] + mesFinal[2], "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(-1);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DataInicial) && Utilitario.IsDateTime(DataInicial, "dd/MM/yyyy"))
                        dataInicial = DateTime.ParseExact(DataInicial.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (!string.IsNullOrEmpty(DataFinal) && Utilitario.IsDateTime(DataFinal, "dd/MM/yyyy"))
                        dataFinal = DateTime.ParseExact(DataFinal.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                }


                string strDataInicial = dataInicial.ToString("yyyyMMddHHmmss");

                string strDataFinal = dataFinal.ToString("yyyyMMddHHmmss");



                switch (filtro)
                {
                    case EnumFiltro.Mensal:
                        bd.Consulta("EXEC TotalFinanceiroPorSerieFormaPagamentoDataPorData " + assinaturaTipoID + ", 0, '" + temporada + "' , '" + strDataInicial + "' , '" + strDataFinal + "'");
                        break;
                    case EnumFiltro.PorData:
                        bd.Consulta("EXEC TotalFinanceiroPorSerieFormaPagamentoDataPorData " + assinaturaTipoID + ", 1, '" + temporada + "' , '" + strDataInicial + "' , '" + strDataFinal + "'");
                        break;
                    default:
                        throw new Exception("Filtro inválido!");
                }

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem resultados a serem exibidos.");

                switch (filtro)
                {
                    case EnumFiltro.Mensal:
                        return this.FiltroMensal(dataInicial, dataFinal);
                    case EnumFiltro.PorData:
                        return this.Total();
                    default:
                        throw new Exception("Filtro inválido!");
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        private List<RecebimentoSerieFormaPagamento> Total()
        {
            List<RecebimentoSerieFormaPagamento> assinaturas = new List<RecebimentoSerieFormaPagamento>();
            var assinatura = new RecebimentoSerieFormaPagamento();
            int assinaturaAtual = 0;
            //Cartao
            do
            {
                if (assinaturaAtual != bd.LerInt("ID"))
                {
                    assinaturaAtual = bd.LerInt("ID");
                    assinatura = assinaturas.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                }

                if (assinatura == null)
                {
                    assinatura = new RecebimentoSerieFormaPagamento();
                    assinatura.AssinaturaID = bd.LerInt("ID");
                    assinatura.Assinatura = bd.LerString("Nome");
                    assinaturas.Add(assinatura);
                }

                decimal cartoes = bd.LerDecimal("Cartao");
                decimal faturas = bd.LerDecimal("FaturaAberta");

                if (faturas > 0)
                {
                    assinatura.QuantidadeFaturasAbertas += bd.LerInt("Quantidade");
                }
                else
                {
                    assinatura.QuantidadeCartoes += bd.LerInt("Quantidade");
                }

                assinatura.Cartoes += cartoes;
                assinatura.FaturasAbertas += faturas;
            } while (bd.Consulta().Read());

            bd.Consulta().NextResult();
            //boleto
            if (bd.Consulta().Read())
            {

                do
                {
                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = assinaturas.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        assinaturas.Add(assinatura);
                    }

                    decimal boletosAbertos = bd.LerDecimal("ValorReceber");
                    decimal boletosPagos = bd.LerDecimal("ValorRecebido");

                    if (boletosAbertos > 0)
                    {
                        assinatura.QuantidadeBoletosAbertos += bd.LerInt("Quantidade");
                    }
                    else
                    {
                        assinatura.QuantidadeBoletosPagos += bd.LerInt("Quantidade");
                    }

                    assinatura.BoletosAbertos += boletosAbertos;
                    assinatura.BoletosPagos += boletosPagos;

                } while (bd.Consulta().Read());
            }

            bd.Consulta().NextResult();
            //dinheiro
            if (bd.Consulta().Read())
            {

                do
                {
                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = assinaturas.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        assinaturas.Add(assinatura);
                    }


                    assinatura.QuantidadeDinheiros += bd.LerInt("Quantidade");
                    assinatura.Dinheiros += bd.LerDecimal("Dinheiro");


                } while (bd.Consulta().Read());
            }

            bd.Consulta().NextResult();
            //cheque
            if (bd.Consulta().Read())
            {

                do
                {
                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = assinaturas.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        assinaturas.Add(assinatura);
                    }


                    assinatura.QuantidadeCheques += bd.LerInt("Quantidade");
                    assinatura.Cheques += bd.LerDecimal("Cheque");


                } while (bd.Consulta().Read());
            }



            return assinaturas;
        }

        private List<RecebimentoPeriodo> FiltroPeriodo(int assinaturaTipoID)
        {
            var Periodos = this.MontarPeriodos(assinaturaTipoID);
            var periodo = new RecebimentoPeriodo();
            var assinatura = new RecebimentoSerieFormaPagamento();

            int periodoAtual = 0;
            int assinaturaAtual = 0;
            //cartao
            do
            {
                if (periodoAtual != bd.LerInt("Periodo"))
                {
                    periodoAtual = bd.LerInt("Periodo");
                    periodo = Periodos.Where(c => (int)c.Periodo == bd.LerInt("Periodo")).FirstOrDefault();
                }

                if (assinaturaAtual != bd.LerInt("ID"))
                {
                    assinaturaAtual = bd.LerInt("ID");
                    assinatura = periodo.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                }

                if (assinatura == null)
                {
                    assinatura = new RecebimentoSerieFormaPagamento();
                    assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                    assinatura.Assinatura = bd.LerString("Nome");
                    periodo.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                }

                assinatura.QuantidadeCartoes += bd.LerInt("Quantidade");
                assinatura.Cartoes += bd.LerDecimal("Cartao");

            } while (bd.Consulta().Read());

            bd.Consulta().NextResult();
            //boleto
            if (bd.Consulta().Read())
            {

                do
                {
                    if (periodoAtual != bd.LerInt("Periodo"))
                    {
                        periodoAtual = bd.LerInt("Periodo");
                        periodo = Periodos.Where(c => (int)c.Periodo == bd.LerInt("Periodo")).FirstOrDefault();
                    }

                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = periodo.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        periodo.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                    }

                    decimal boletosAbertos = bd.LerDecimal("ValorReceber");
                    decimal boletosPagos = bd.LerDecimal("ValorRecebido");

                    if (boletosAbertos > 0)
                    {
                        assinatura.QuantidadeBoletosAbertos += bd.LerInt("Quantidade");
                    }
                    else
                    {
                        assinatura.QuantidadeBoletosPagos += bd.LerInt("Quantidade");
                    }

                    assinatura.BoletosAbertos += boletosAbertos;
                    assinatura.BoletosPagos += boletosPagos;


                } while (bd.Consulta().Read());
            }

            bd.Consulta().NextResult();
            //dinheiro
            if (bd.Consulta().Read())
            {

                do
                {
                    if (periodoAtual != bd.LerInt("Periodo"))
                    {
                        periodoAtual = bd.LerInt("Periodo");
                        periodo = Periodos.Where(c => (int)c.Periodo == bd.LerInt("Periodo")).FirstOrDefault();
                    }

                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = periodo.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        periodo.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                    }

                    assinatura.QuantidadeDinheiros += bd.LerInt("Quantidade");
                    assinatura.Dinheiros += bd.LerDecimal("Dinheiro");


                } while (bd.Consulta().Read());
            }


            bd.Consulta().NextResult();
            //cheque
            if (bd.Consulta().Read())
            {

                do
                {
                    if (periodoAtual != bd.LerInt("Periodo"))
                    {
                        periodoAtual = bd.LerInt("Periodo");
                        periodo = Periodos.Where(c => (int)c.Periodo == bd.LerInt("Periodo")).FirstOrDefault();
                    }

                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = periodo.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        periodo.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                    }

                    assinatura.QuantidadeCheques += bd.LerInt("Quantidade");
                    assinatura.Cheques += bd.LerDecimal("Cheque");


                } while (bd.Consulta().Read());
            }

            return this.LimparPeriodos(Periodos);
        }

        private List<RecebimentoMes> FiltroMensal()
        {
            var Meses = this.MontarMeses();
            var mes = new RecebimentoMes();
            var assinatura = new RecebimentoSerieFormaPagamento();

            int mesAtual = 0;
            int assinaturaAtual = 0;

            do
            {
                if (mesAtual != bd.LerDateTime("DataVenda").Month)
                {
                    mesAtual = bd.LerDateTime("DataVenda").Month;
                    mes = Meses.Where(c => c.Mes == bd.LerDateTime("DataVenda").Month).FirstOrDefault();
                }

                if (assinaturaAtual != bd.LerInt("ID"))
                {
                    assinaturaAtual = bd.LerInt("ID");
                    assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                }

                if (assinatura == null)
                {
                    assinatura = new RecebimentoSerieFormaPagamento();
                    assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                    assinatura.Assinatura = bd.LerString("Nome");
                    mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                }

                assinatura.Cartoes += bd.LerDecimal("Cartao");
                assinatura.QuantidadeCartoes += bd.LerInt("Quantidade");

            } while (bd.Consulta().Read());

            bd.Consulta().NextResult();

            if (!bd.Consulta().Read())
                return this.LimparMeses(Meses);

            mesAtual = 0;
            assinaturaAtual = 0;

            do
            {
                if (mesAtual != bd.LerDateTime("DataVenda").Month)
                {
                    mesAtual = bd.LerDateTime("DataVenda").Month;
                    mes = Meses.Where(c => c.Mes == bd.LerDateTime("DataVenda").Month).FirstOrDefault();
                }
                if (assinaturaAtual != bd.LerInt("ID"))
                {
                    assinaturaAtual = bd.LerInt("ID");
                    assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                }
                if (assinatura == null)
                {
                    assinatura = new RecebimentoSerieFormaPagamento();
                    assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                    assinatura.Assinatura = bd.LerString("Nome");
                    mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                }

                decimal boletosAbertos = bd.LerDecimal("ValorReceber");
                decimal boletosPagos = bd.LerDecimal("ValorRecebido");

                if (boletosAbertos > 0)
                {
                    assinatura.QuantidadeBoletosAbertos += bd.LerInt("Quantidade");
                }
                else
                {
                    assinatura.QuantidadeBoletosPagos += bd.LerInt("Quantidade");
                }

                assinatura.BoletosAbertos += boletosAbertos;
                assinatura.BoletosPagos += boletosPagos;
            } while (bd.Consulta().Read());
            return this.LimparMeses(Meses);
        }

        private List<RecebimentoMes> FiltroMensal(DateTime dataInicial, DateTime dataFinal)
        {
            // var Meses = this.MontarMeses();
            var Meses = this.MontarMeses(dataInicial, dataFinal);
            var mes = new RecebimentoMes();
            var assinatura = new RecebimentoSerieFormaPagamento();

            int mesAtual = 0;
            int anoAtual = 0;
            int assinaturaAtual = 0;
            //Cartao
            bool ConsultaErrada = false;

            do
            {
                if (bd.LerString("Codigo") == "C")
                {
                    if (mesAtual != bd.LerDateTime("DataVenda").Month)
                    {
                        mesAtual = bd.LerDateTime("DataVenda").Month;
                        anoAtual = bd.LerDateTime("DataVenda").Year;
                        mes = Meses.Where(c => c.Mes == mesAtual && c.Ano == anoAtual).FirstOrDefault();
                        assinaturaAtual = 0;
                    }

                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                    }
                    assinatura.Cartoes += bd.LerDecimal("Cartao");
                    assinatura.QuantidadeCartoes += bd.LerInt("Quantidade");
                }
                else
                {
                    ConsultaErrada = true;
                    break;

                }
            } while (bd.Consulta().Read());

            if (!ConsultaErrada)
            {
                bd.Consulta().NextResult();
            }

            // Boleto
            if (ConsultaErrada || bd.Consulta().Read())
            {
                ConsultaErrada = false;
                mesAtual = 0;
                assinaturaAtual = 0;

                do
                {
                    if (bd.LerString("Codigo") == "B")
                    {
                        if (mesAtual != bd.LerDateTime("DataVenda").Month)
                        {
                            mesAtual = bd.LerDateTime("DataVenda").Month;
                            anoAtual = bd.LerDateTime("DataVenda").Year;
                            mes = Meses.Where(c => c.Mes == mesAtual && c.Ano == anoAtual).FirstOrDefault();
                            assinaturaAtual = 0;
                        }
                        if (assinaturaAtual != bd.LerInt("ID"))
                        {
                            assinaturaAtual = bd.LerInt("ID");
                            assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                        }
                        if (assinatura == null)
                        {
                            assinatura = new RecebimentoSerieFormaPagamento();
                            assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                            assinatura.Assinatura = bd.LerString("Nome");
                            mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                        }

                        decimal boletosAbertos = bd.LerDecimal("ValorReceber");
                        decimal boletosPagos = bd.LerDecimal("ValorRecebido");

                        if (boletosAbertos > 0)
                        {
                            assinatura.QuantidadeBoletosAbertos += bd.LerInt("Quantidade");
                        }
                        else
                        {
                            assinatura.QuantidadeBoletosPagos += bd.LerInt("Quantidade");
                        }

                        assinatura.BoletosAbertos += boletosAbertos;
                        assinatura.BoletosPagos += boletosPagos;
                    }
                    else
                    {
                        ConsultaErrada = true;
                        break;

                    }
                } while (bd.Consulta().Read());
            }


            if (!ConsultaErrada)
            {
                bd.Consulta().NextResult();
            }

            // Dinheiro
            if (bd.Consulta().Read())
            {
                ConsultaErrada = false;
                mesAtual = 0;
                assinaturaAtual = 0;

                do
                {
                    if (bd.LerString("Codigo") == "D")
                    {
                        if (mesAtual != bd.LerDateTime("DataVenda").Month)
                        {
                            mesAtual = bd.LerDateTime("DataVenda").Month;
                            anoAtual = bd.LerDateTime("DataVenda").Year;
                            mes = Meses.Where(c => c.Mes == mesAtual && c.Ano == anoAtual).FirstOrDefault();
                            assinaturaAtual = 0;
                        }

                        if (assinaturaAtual != bd.LerInt("ID"))
                        {
                            assinaturaAtual = bd.LerInt("ID");
                            assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                        }

                        if (assinatura == null)
                        {
                            assinatura = new RecebimentoSerieFormaPagamento();
                            assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                            assinatura.Assinatura = bd.LerString("Nome");
                            mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                        }

                        assinatura.Dinheiros += bd.LerDecimal("Dinheiro");
                        assinatura.QuantidadeDinheiros += bd.LerInt("Quantidade");
                    }
                    else
                    {
                        ConsultaErrada = true;
                        break;

                    }
                } while (bd.Consulta().Read());
            }

            if (!ConsultaErrada)
            {
                bd.Consulta().NextResult();
            }

            // cheque

            if (bd.Consulta().Read())
            {
                mesAtual = 0;
                assinaturaAtual = 0;
                do
                {
                    if (mesAtual != bd.LerDateTime("DataVenda").Month)
                    {
                        mesAtual = bd.LerDateTime("DataVenda").Month;
                        anoAtual = bd.LerDateTime("DataVenda").Year;
                        mes = Meses.Where(c => c.Mes == mesAtual && c.Ano == anoAtual).FirstOrDefault();
                        assinaturaAtual = 0;
                    }

                    if (assinaturaAtual != bd.LerInt("ID"))
                    {
                        assinaturaAtual = bd.LerInt("ID");
                        assinatura = mes.ListaRecebimentoMesSerieFormaPagamento.Where(c => c.AssinaturaID == bd.LerInt("ID")).FirstOrDefault();
                    }

                    if (assinatura == null)
                    {
                        assinatura = new RecebimentoSerieFormaPagamento();
                        assinatura.AssinaturaID = assinaturaAtual = bd.LerInt("ID");
                        assinatura.Assinatura = bd.LerString("Nome");
                        mes.ListaRecebimentoMesSerieFormaPagamento.Add(assinatura);
                    }

                    assinatura.Cheques += bd.LerDecimal("Cheque");
                    assinatura.QuantidadeCheques += bd.LerInt("Quantidade");
                } while (bd.Consulta().Read());
            }


            return this.LimparMeses(Meses, dataInicial, dataFinal);
        }

    }
}
