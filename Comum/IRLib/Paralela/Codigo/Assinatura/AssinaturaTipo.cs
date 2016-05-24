/**************************************************
* Arquivo: AssinaturaTipo.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{

    public class AssinaturaTipo : AssinaturaTipo_B
    {

        public AssinaturaTipo() { }

        public enum EnumPeriodos
        {
            Renovacao = 1,
            Troca = 2,
            Aquisicao = 3,
            ForaDePeriodo = 4,
        }

        public List<EstruturaIDNome> CarregarTipos(bool registroZero)
        {
            return CarregarTipos(registroZero, null);
        }

        public List<EstruturaIDNome> CarregarTipos(bool registroZero, int? localID)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                if (registroZero)
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                var sql = "SELECT Nome,ID FROM tAssinaturaTipo ";

                if (localID != null)
                    sql += "WHERE LocalID = " + localID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
        public List<EstruturaIDNome> CarregarTipos(int LocalID, int RegionalID, bool registroZero)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                if (registroZero)
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                string filtro = "";
                if (LocalID > 0)
                {
                    filtro = " WHERE l.ID = " + LocalID;

                }
                else if (RegionalID > 0)
                {
                    filtro = " WHERE e.RegionalID = " + RegionalID;

                }

                string sql = string.Format(@"SELECT
	                                    DISTINCT at.ID, at.Nome
	                                    FROM tEmpresa e (NOLOCK)
	                                    INNER JOIN tLocal l (NOLOCK) ON l.EmpresaID = e.ID
	                                    INNER JOIN tAssinatura a (NOLOCK) ON a.LocalID = l.ID
	                                    INNER JOIN tAssinaturaTipo at (NOLOCK) ON at.ID = a.AssinaturaTipoID
	                                     " + filtro);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public IRLib.Paralela.Assinaturas.Models.Fase BuscarFase(int assinaturaTipoID)
        {
            try
            {
                this.Ler(assinaturaTipoID);

                if (this.RenovacaoInicio.Valor.Date > DateTime.Now.Date)
                {
                    //Entrou antes da renovacao
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Invalido,
                        FaseDescricao = "O período de renovações ainda não começou.",
                        DataInicial = null,
                        DataFinal = null,
                    };
                }
                else if (this.RenovacaoFim.Valor.Date >= DateTime.Now.Date)
                {
                    //Entrou na renovacao
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Renovacao,
                        FaseDescricao = "Estamos no período de renovações.",
                        DataInicial = this.RenovacaoInicio.Valor,
                        DataFinal = this.RenovacaoFim.Valor,
                    };
                }
                else if (this.TrocaPrioritariaInicio.Valor.Date > DateTime.Now.Date)
                {
                    //Entre periodo de Renovacao e Troca Propritaria
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Invalido,
                        FaseDescricao = "O período de trocas prioritárias ainda não começou.",
                        DataInicial = null,
                        DataFinal = null,
                    };
                }
                else if (this.TrocaPrioritariaFim.Valor.Date >= DateTime.Now.Date)
                {
                    //Entrou na prioritaria
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.TrocaPrioritaria,
                        FaseDescricao = "Estamos no período de trocas prioritárias.",
                        DataInicial = this.TrocaPrioritariaInicio.Valor,
                        DataFinal = this.TrocaPrioritariaFim.Valor,
                    };
                }
                else if (this.TrocaInicio.Valor.Date > DateTime.Now.Date)
                {
                    //Entre periodo de Troca Prioritaria e Troca
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Invalido,
                        FaseDescricao = "O período de trocas ainda não começou.",
                        DataInicial = null,
                        DataFinal = null,
                    };
                }
                else if (this.TrocaFim.Valor.Date >= DateTime.Now.Date)
                {
                    //Entrou na troca
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Troca,
                        FaseDescricao = "Estamos no período de trocas.",
                        DataInicial = this.TrocaInicio.Valor,
                        DataFinal = this.TrocaFim.Valor,
                    };
                }
                else if (this.NovaAquisicaoInicio.Valor.Date > DateTime.Now.Date)
                {
                    //Entre periodo de Aquisicao e troca
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Invalido,
                        FaseDescricao = "O período de aquisições ainda não começou.",
                        DataInicial = null,
                        DataFinal = null,
                    };
                }
                else if (this.NovaAquisicaoFim.Valor.Date >= DateTime.Now.Date)
                {
                    //Entrou na nova aquisicao
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Aquisicoes,
                        FaseDescricao = "Estamos no período de novas aquisições.",
                        DataInicial = this.NovaAquisicaoInicio.Valor,
                        DataFinal = this.NovaAquisicaoFim.Valor,
                    };
                }
                else
                {
                    //Está fora de todos os periodos
                    return new Assinaturas.Models.Fase()
                    {
                        FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Invalido,
                        FaseDescricao = "O período de novas aquisições está encerrado.",
                        DataInicial = null,
                        DataFinal = null,
                    };
                }

            }
            finally
            {
                bd.Fechar();
            }
        }

        public Assinaturas.Models.AssinaturaTipo BuscarModel(int assinaturaTipoID)
        {
            this.Ler(assinaturaTipoID);
            return new Assinaturas.Models.AssinaturaTipo()
            {
                ID = this.Control.ID,
                Termo = this.Termos.Valor,
                Programacao = this.Programacao.Valor,
                PaginaPrincipal = this.PaginaPrincipal.Valor,
                Precos = this.Precos.Valor
            };
        }

        public Assinaturas.Models.Fase BuscarProximaFase(Assinaturas.Models.Fase fase, int assinaturaTipoID, bool trocaPrioritaria)
        {
            try
            {
                this.Ler(assinaturaTipoID);

                switch (fase.FaseAssinatura)
                {

                    case IRLib.Paralela.Utils.Enums.EnumFaseAssinatura.Renovacao:
                        return new Assinaturas.Models.Fase()
                        {
                            FaseAssinatura = trocaPrioritaria ? Utils.Enums.EnumFaseAssinatura.TrocaPrioritaria : Utils.Enums.EnumFaseAssinatura.Troca,
                            DataInicial = trocaPrioritaria ? this.TrocaPrioritariaInicio.Valor : this.TrocaInicio.Valor,
                            DataFinal = this.TrocaFim.Valor,
                        };
                    case IRLib.Paralela.Utils.Enums.EnumFaseAssinatura.TrocaPrioritaria:
                        return new Assinaturas.Models.Fase()
                        {
                            FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Troca,
                            DataInicial = this.TrocaInicio.Valor,
                            DataFinal = this.TrocaFim.Valor,
                        };
                    case IRLib.Paralela.Utils.Enums.EnumFaseAssinatura.Troca:
                        return new Assinaturas.Models.Fase()
                        {
                            FaseAssinatura = Utils.Enums.EnumFaseAssinatura.Aquisicoes,
                            DataInicial = this.NovaAquisicaoInicio.Valor,
                            DataFinal = this.NovaAquisicaoFim.Valor,
                        };
                    case IRLib.Paralela.Utils.Enums.EnumFaseAssinatura.Invalido:
                    case IRLib.Paralela.Utils.Enums.EnumFaseAssinatura.Aquisicoes:
                    default:
                        return new Assinaturas.Models.Fase();
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Salvar(Assinaturas.Models.Configuracao pConfiguracao, int pTipoAssinaturaID)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();


                StringBuilder sql = new StringBuilder();

                DateTime InicioRenovacao = Convert.ToDateTime(pConfiguracao.DtInicioRenovacao);
                DateTime TerminoRenovacao = Convert.ToDateTime(pConfiguracao.DtTerminoRenovacao);

                DateTime InicioTroca = Convert.ToDateTime(pConfiguracao.DtInicioTroca);
                DateTime TerminoTroca = Convert.ToDateTime(pConfiguracao.DtTerminoTroca);

                DateTime InicioTrocaPrioritaria = Convert.ToDateTime(pConfiguracao.DtInicioTrocaPrioritaria);
                DateTime TerminoTrocaPrioritaria = Convert.ToDateTime(pConfiguracao.DtTerminoTrocaPrioritaria);

                DateTime InicioAquisicao = Convert.ToDateTime(pConfiguracao.DtInicioAquisicao);
                DateTime TerminoAquisicao = Convert.ToDateTime(pConfiguracao.DtTerminoAquisicao);


                sql.Append(@"UPDATE tAssinaturaTipo SET");

                sql.Append("    RenovacaoInicio			= '" + InicioRenovacao.ToString("yyyyMMdd000000") + "',");
                sql.Append("    RenovacaoFim			= '" + TerminoRenovacao.ToString("yyyyMMdd000000") + "',");
                sql.Append("    TrocaInicio				= '" + InicioTroca.ToString("yyyyMMdd000000") + "',");
                sql.Append("    TrocaFim				= '" + TerminoTroca.ToString("yyyyMMdd000000") + "',");
                sql.Append("    TrocaPrioritariaInicio	= '" + InicioTrocaPrioritaria.ToString("yyyyMMdd000000") + "',");
                sql.Append("    TrocaPrioritariaFim		= '" + TerminoTrocaPrioritaria.ToString("yyyyMMdd000000") + "',");
                sql.Append("    NovaAquisicaoInicio		= '" + InicioAquisicao.ToString("yyyyMMdd000000") + "',");
                sql.Append("    NovaAquisicaoFim		= '" + TerminoAquisicao.ToString("yyyyMMdd000000") + "',");
                sql.Append("    Termos					= '" + pConfiguracao.textoTermosCondicoes + "',");
                sql.Append("    PaginaPrincipal			= '" + pConfiguracao.textoPaginaPrincipal + "',");
                sql.Append("    PermiteAgregados		= '" + (pConfiguracao.PermiteAgregados ? "T" : "F") + "',");
                sql.Append("    Layout					= '" + pConfiguracao.LayoutAssinatura + "',");

                if (!String.IsNullOrEmpty(pConfiguracao.Logo))
                    sql.Append("    Logo					= '" + pConfiguracao.Logo + "',");

                sql.Append("    PaginaLogin				= '" + pConfiguracao.textoLogin + "',");
                sql.Append("    PaginaRodape			= '" + pConfiguracao.textoRodape + "',");
                sql.Append("    CanalAcessoID			= '" + pConfiguracao.CanalAcessoID.ToString() + "',");
                sql.Append("    ValorEntregaFixo		= '" + (pConfiguracao.ValorEntregaFixo ? "T" : "F") + "',");

                if (!String.IsNullOrEmpty(pConfiguracao.ValorEntrega))
                    sql.Append("    ValorEntrega		    = '" + pConfiguracao.ValorEntrega.Replace(",", ".") + "',");

                sql.Append("    RetiradaBilheteria		= '" + (pConfiguracao.RetiradaBilheteria ? "T" : "F") + "',");
                sql.Append("    EntregaID   			= '" + pConfiguracao.EntregaID + "'");
                sql.Append(" WHERE  ");
                sql.Append(" ID = " + pTipoAssinaturaID.ToString());

                bd.Executar(sql);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public Assinaturas.Models.Configuracao BuscarConfiguracaoTipoAssinatura(int pAssinaturaTipoID)
        {
            try
            {

                Assinaturas.Models.Configuracao oConfiguracao = new Assinaturas.Models.Configuracao();

                string sql = string.Format(@"SELECT DISTINCT tAssinaturaTipo.* ,tAssinaturaAno.Ano
                                            FROM tAssinatura
                                                INNER JOIN tAssinaturaTipo ON tAssinatura.AssinaturaTipoID = tAssinaturaTipo.ID
                                                INNER JOIN tAssinaturaAno ON tAssinatura.ID = tAssinaturaAno.AssinaturaID
                                            WHERE tAssinaturaTipo.ID = {0}"
                    , pAssinaturaTipoID);

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    oConfiguracao.CanalAcessoID = bd.LerInt("CanalAcessoID");
                    oConfiguracao.DtInicioAquisicao = bd.LerString("NovaAquisicaoInicio");
                    oConfiguracao.DtInicioRenovacao = bd.LerString("RenovacaoInicio");
                    oConfiguracao.DtInicioTroca = bd.LerString("TrocaInicio");
                    oConfiguracao.DtInicioTrocaPrioritaria = bd.LerString("TrocaPrioritariaInicio");
                    oConfiguracao.DtTerminoAquisicao = bd.LerString("NovaAquisicaoFim");
                    oConfiguracao.DtTerminoRenovacao = bd.LerString("RenovacaoFim");
                    oConfiguracao.DtTerminoTroca = bd.LerString("TrocaFim");
                    oConfiguracao.DtTerminoTrocaPrioritaria = bd.LerString("TrocaPrioritariaFim");
                    oConfiguracao.LayoutAssinatura = bd.LerString("Layout");
                    oConfiguracao.Logo = bd.LerString("Logo");
                    oConfiguracao.PermiteAgregados = bd.LerBoolean("PermiteAgregados");
                    oConfiguracao.textoLogin = bd.LerString("PaginaLogin");
                    oConfiguracao.textoPaginaPrincipal = bd.LerString("PaginaPrincipal");
                    oConfiguracao.textoRodape = bd.LerString("PaginaRodape");
                    oConfiguracao.textoTermosCondicoes = bd.LerString("Termos");
                    oConfiguracao.RetiradaBilheteria = bd.LerBoolean("RetiradaBilheteria");
                    oConfiguracao.ValorEntregaFixo = bd.LerBoolean("ValorEntregaFixo");
                    oConfiguracao.ValorEntrega = bd.LerString("ValorEntrega");
                    oConfiguracao.EntregaID = bd.LerString("EntregaID");
                    oConfiguracao.AnoAtivoAssinatura = bd.LerString("Ano");                    
                }
                bd.Fechar();



                var ListaTipoPagamentoID = Enumerable.Repeat(new { FormaPagamentoID = 0, TipoID = 0 }, 0).ToList();

                sql = string.Format(@"SELECT DISTINCT tAssinaturaFormaPagamento.FormaPagamentoID, tfp.FormaPagamentoTipoID
                                    FROM tAssinatura 
                                    INNER JOIN tAssinaturaFormaPagamento ON tAssinatura.ID = tAssinaturaFormaPagamento.AssinaturaID
                                    INNER JOIN tFormaPagamento tfp ON tfp.id = FormaPagamentoID
                                    WHERE AssinaturaTipoID = {0}", pAssinaturaTipoID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                    ListaTipoPagamentoID.Add(new { FormaPagamentoID = bd.LerInt("FormaPagamentoID"), TipoID = bd.LerInt("FormaPagamentoTipoID") });
                bd.Fechar();

                if (ListaTipoPagamentoID.Count > 0)
                {
                    // FormaPagamento Debito
                    if (ListaTipoPagamentoID.FindAll(x => x.TipoID == IRLib.Paralela.FormaPagamento.TIPO_CARTAO_DEBITO).Count > 0)
                        oConfiguracao.AceitaDebito = true;
                    else
                        oConfiguracao.AceitaDebito = false;

                    // FormaPagamento Dinheiro
                    if (ListaTipoPagamentoID.FindAll(x => x.FormaPagamentoID == IRLib.Paralela.FormaPagamento.DINHEIRO).Count > 0)
                        oConfiguracao.AceitaDinheiro = true;
                    else
                        oConfiguracao.AceitaDinheiro = false;

                    // FormaPagamento Cheque
                    if (ListaTipoPagamentoID.FindAll(x => x.FormaPagamentoID == IRLib.Paralela.FormaPagamento.TIPO_CHEQUE_3 || x.FormaPagamentoID == IRLib.Paralela.FormaPagamento.TIPO_CHEQUE_5 || x.FormaPagamentoID == IRLib.Paralela.FormaPagamento.TIPO_CHEQUE_9).Count > 0)
                        oConfiguracao.AceitaCheque = true;
                    else
                        oConfiguracao.AceitaCheque = false;

                }
                else
                {
                    oConfiguracao.AceitaDinheiro = false;
                    oConfiguracao.AceitaDebito = false;
                    oConfiguracao.AceitaCheque = false;
                }


                return oConfiguracao;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaTipoLista : AssinaturaTipoLista_B
    {

        public AssinaturaTipoLista() { }

    }

}
