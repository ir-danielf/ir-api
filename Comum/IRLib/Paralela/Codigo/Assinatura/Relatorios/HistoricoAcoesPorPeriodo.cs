using CTLib;
using IRLib.Paralela.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class HistoricoAcoesPorPeriodo
    {
        public List<HistoricoPorPeriodo> FotoAtual(int assinaturaTipoID)
        {
            BD bd = new BD();
            try
            {
                AssinaturaTipo oAssinaturaTipo = new AssinaturaTipo();
                oAssinaturaTipo.Ler(assinaturaTipoID);

                List<HistoricoPorPeriodo> lista = new List<HistoricoPorPeriodo>();
                if (DateTime.Now.Date < oAssinaturaTipo.RenovacaoInicio.Valor)
                    return null;

                bd.Executar("EXEC historicoAcoesPorPeriodo " + assinaturaTipoID + ", '" + oAssinaturaTipo.RenovacaoInicio.Valor.ToString("yyyyMMddHHmmss") + "', '" + oAssinaturaTipo.RenovacaoFim.Valor.AddDays(1).ToString("yyyyMMddHHmmss") + "', '" + (DateTime.Now.Year + 1) + "'");
                if (bd.Consulta().Read())
                    lista.Add(new HistoricoPorPeriodo()
                    {
                        Periodo = 1,
                        Historico = new HistoricoAcoes()
                        {
                            Renovados = bd.LerInt("Renovados"),
                            TrocasSinalizadas = bd.LerInt("TrocasSinalizadas"),
                            Aquisicoes = bd.LerInt("Aquisicoes"),
                            Desistencias = bd.LerInt("Desistencias"),
                        },
                    });

                bd.FecharConsulta();

                if (DateTime.Now.Date < oAssinaturaTipo.TrocaPrioritariaInicio.Valor)
                    return lista;

                bd.Executar("EXEC historicoAcoesPorPeriodo " + assinaturaTipoID + ", '" + oAssinaturaTipo.TrocaPrioritariaInicio.Valor.ToString("yyyyMMddHHmmss") + "', '" + oAssinaturaTipo.TrocaFim.Valor.AddDays(1).ToString("yyyyMMddHHmmss") + "', '" + (DateTime.Now.Year + 1) + "'");
                if (bd.Consulta().Read())
                    lista.Add(new HistoricoPorPeriodo()
                    {
                        Periodo = 2,
                        Historico = new HistoricoAcoes()
                        {
                            Renovados = bd.LerInt("Renovados"),
                            TrocasSinalizadas = bd.LerInt("TrocasSinalizadas"),
                            Aquisicoes = bd.LerInt("Aquisicoes"),
                            Desistencias = bd.LerInt("Desistencias"),
                            TrocasEfetuadas = bd.LerInt("TrocasEfetuadas"),
                        },
                    });

                bd.FecharConsulta();

                if (DateTime.Now.Date < oAssinaturaTipo.NovaAquisicaoInicio.Valor)
                    return lista;

                bd.Executar("EXEC historicoAcoesPorPeriodo " + assinaturaTipoID + ", '" + oAssinaturaTipo.NovaAquisicaoInicio.Valor.ToString("yyyyMMddHHmmss") + "', '" + oAssinaturaTipo.NovaAquisicaoFim.Valor.AddDays(1).ToString("yyyyMMddHHmmss") + "', '" + (DateTime.Now.Year + 1) + "'");
                if (bd.Consulta().Read())
                    lista.Add(new HistoricoPorPeriodo()
                    {
                        Periodo = 3,
                        Historico = new HistoricoAcoes()
                        {
                            Renovados = bd.LerInt("Renovados"),
                            TrocasSinalizadas = bd.LerInt("TrocasSinalizadas"),
                            Aquisicoes = bd.LerInt("Aquisicoes"),
                            Desistencias = bd.LerInt("Desistencias"),
                            TrocasEfetuadas = bd.LerInt("TrocasEfetuadas"),
                        },
                    });

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
