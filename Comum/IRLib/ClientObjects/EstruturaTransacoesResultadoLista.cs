using CTLib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaTransacoesResultadoLista : List<EstruturaTransacoesResultado>
    {
        private const string VISA = "Images/bandeira_visa.gif";
        private const string MASTER = "Images/bandeira_master.gif";
        private const string AMEX = "Images/bandeira_amex.gif";
        private const string HIPER = "Images/hipercard_40.jpg";
        private const string AURA = "Images/aura_40.jpg";
        private const string DINERS = "Images/bandeira_diners.gif";
        private const string REDESHOP = "Images/bandeira_visa.gif";
        private const string OUTROS = "Images/bandeira_outros.jpg";

        public string Bandeira { get; set; }
        public string CanalID { get; set; }
        public string EventoID { get; set; }
        public string ApresentacaoID { get; set; }
        public VendaBilheteria.StatusAntiFraude Status { get; set; }
        public string Auth { get; set; }
        public string NSU { get; set; }
        public string IP { get; set; }
        public string Cartao { get; set; }
        public int CartaoID { get; set; }
        public string DataInicial { get; set; }
        public DateTime dtDataInicial { get; set; }
        public string DataFinal { get; set; }
        public DateTime dtDataFinal { get; set; }
        public int ClienteID { get; set; }
        public int Registros { get; set; }
        public bool Reiniciar { get; set; }
        public int IndexAtual { get; set; }
        public int QuantidadePorPagina { get; set; }

        public int Ordenacao1 { get; set; }
        public int Ordenacao2 { get; set; }

        public string Ordenacao { get; set; }

        public bool Completo { get; set; }
        public int Dias { get; set; }
        public bool SemPaginacao { get; set; }

        public EstruturaFiltro FiltroQS { get; set; }

        BD bd = new BD();

        EstruturaTransacoesResultado item;

        public EstruturaTransacoesResultadoLista()
        {
            this.Clear();
        }

        public EstruturaTransacoesResultadoLista CarregarLista()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(DISTINCT log.IngressoID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco ");
                stbSQL.Append(" ,ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo ");
                stbSQL.Append("FROM tVendaBilheteria (NOLOCK) ");
                stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tIngressoLog log (NOLOCK) ON tVendaBilheteria.ID = log.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID ");
                stbSQL.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append("WHERE " + this.MontarFiltroTransacoes());
                stbSQL.Append(" GROUP BY tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status,tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco , tCliente.NomeFantasia , tCliente.CNPJ, tAntiFraudeMotivoTipo.Nome ");
                stbSQL.Append("ORDER BY " + this.Ordenacao);


                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;
                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;
                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];

                        item.Status = bd.LerBoolean("VendaCancelada") ? "C" : bd.LerString("Status");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);
                        item.Motivo = bd.LerString("Motivo");
                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;


                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }

                }
                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaCompraPorEvento()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(DISTINCT log.IngressoID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco ");
                stbSQL.Append(" ,ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo ");
                stbSQL.Append("FROM tVendaBilheteria (NOLOCK) ");
                stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tIngressoLog log (NOLOCK) ON tVendaBilheteria.ID = log.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("INNER JOIN tIngresso (NOLOCK)ON log.IngressoID = tIngresso.ID ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK)ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                stbSQL.Append("INNER JOIN tApresentacao (NOLOCK)ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append(" WHERE ");
                stbSQL.Append(this.MontarFiltroTransacoesEventoCompra());
                stbSQL.Append(" GROUP BY tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status,tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco , tCliente.NomeFantasia , tCliente.CNPJ , tAntiFraudeMotivoTipo.Nome ");

                stbSQL.Append("ORDER BY " + this.Ordenacao);


                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;
                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;
                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];

                        item.Status = bd.LerBoolean("VendaCancelada") ? "C" : bd.LerString("Status");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);
                        item.Motivo = bd.LerString("Motivo");
                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;
                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }

                }
                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaCompraPorFormaEntrega()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(DISTINCT log.IngressoID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco ");
                stbSQL.Append("FROM tVendaBilheteria (NOLOCK) ");
                stbSQL.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tIngressoLog log (NOLOCK) ON tVendaBilheteria.ID = log.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntregaPeriodo (NOLOCK) ON tEntregaControle.PeriodoID = tEntregaPeriodo.ID ");
                stbSQL.Append("LEFT JOIN tEntregaAgenda (NOLOCK) ON tEntregaControle.ID = tEntregaAgenda.EntregaControleID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID ");
                stbSQL.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("INNER JOIN tIngresso (NOLOCK)ON log.IngressoID = tIngresso.ID ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK)ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                stbSQL.Append("INNER JOIN tApresentacao (NOLOCK)ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID ");
                stbSQL.Append("WHERE ");
                stbSQL.Append(this.MontarFiltroTransacoesPorFormaEntrega());
                stbSQL.Append(" GROUP BY tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco , tCliente.NomeFantasia , tCliente.CNPJ ");

                stbSQL.Append("ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;

                while (bd.Consulta().Read())
                {
                    item = new EstruturaTransacoesResultado();
                    item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    item.Nome = bd.LerString("NomeCliente");
                    item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                    item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                    item.Cartao = bd.LerString("NroCartao");

                    item.Bandeira = bd.LerInt("BandeiraID");
                    string[] bandeiras = Bandeiras(item.Bandeira);
                    item.ImagemBandeira = bandeiras[0];
                    item.NomeBandeira = bandeiras[1];

                    item.Status = bd.LerBoolean("VendaCancelada") ? "C" : bd.LerString("Status");

                    string[] status = StatusCompra(item.Status, 1);
                    item.Status = status[0];
                    item.StatusColor = status[1];
                    item.AguardandoVisible = Convert.ToBoolean(status[2]);
                    item.AprovadoVisible = Convert.ToBoolean(status[3]);
                    item.FraudeVisible = Convert.ToBoolean(status[4]);
                    item.EmAnaliseVisible = Convert.ToBoolean(status[5]);

                    item.Entrega = bd.LerString("TaxaEntrega");
                    item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                    pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                    item.Pagina = pagAtual;
                    c++;
                    this.Add(item);
                }
                return this;
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


        public EstruturaTransacoesResultadoLista CarregarListaComprasPendentes()
        {
            string pesquisaWhere = string.Empty;
            try
            {
                if (this.FiltroQS.ListagemCompleta)
                    pesquisaWhere = " WHERE (tVendaBilheteria.Status = '" + VendaBilheteria.AGUARDANDO_APROVACAO + "' OR tVendaBilheteria.Status = '" + VendaBilheteria.EMANALISE + "')  AND (VendaCancelada Is NULL OR VendaCancelada = 'F') AND tVendaBilheteria.DataVenda <= '" + DateTime.Now.AddDays(1).AddDays(-this.FiltroQS.Dias).Date.ToString("yyyyMMdd") + "000000" + "' ";
                else
                    pesquisaWhere = " WHERE tVendaBilheteria.Status = '" + VendaBilheteria.AGUARDANDO_APROVACAO + "' AND (VendaCancelada Is NULL OR VendaCancelada = 'F') AND tVendaBilheteria.DataVenda <= '" + DateTime.Now.AddDays(1).AddDays(-this.FiltroQS.Dias).Date.ToString("yyyyMMdd") + "000000" + "' ";

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tCartao.ID CartaoID, tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(i.ID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda");
                stbSQL.Append(" ,tVendaBilheteria.Status,ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo ");
                stbSQL.Append("FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID ");
                stbSQL.Append("INNER JOIN tIngresso i (NOLOCK) ON tVendaBilheteria.ID = i.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN  tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN  tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append(pesquisaWhere);
                stbSQL.Append("GROUP BY tCartao.ID, tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tCliente.NomeFantasia , tCliente.CNPJ,tAntiFraudeMotivoTipo.Nome ");
                if (Ordenacao.Length > 0)
                    stbSQL.Append("ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;
                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;
                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];

                        item.Status = bd.LerString("Status");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);

                        item.Motivo = bd.LerString("Motivo");

                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;
                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }

                }
                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaFraudesIdentificadas(string Filtros)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("WITH tbGeral AS (SELECT tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(i.ID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.NivelRisco ");
                stbSQL.Append("FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID ");
                stbSQL.Append("INNER JOIN tIngresso i (NOLOCK) ON tVendaBilheteria.ID = i.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("WHERE " + Filtros);
                stbSQL.Append(" GROUP BY tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.NivelRisco) , tCliente.NomeFantasia , tCliente.CNPJ, ");
                stbSQL.Append("tbCount AS (SELECT Count(VendaBilheteriaID) AS Registros FROM tbGeral), ");
                stbSQL.Append("tbOrdenada AS (SELECT  VendaBilheteriaID, NomeCliente, QuantidadeIngressos, ValorTotal, NroCartao, ");
                stbSQL.Append("BandeiraID, TaxaEntrega, DataVenda, Status, NivelRisco, ROW_NUMBER() OVER (ORDER BY " + this.Ordenacao + " ) AS 'RowNumber' ");
                stbSQL.Append("FROM tbGeral) ");
                stbSQL.Append("SELECT VendaBilheteriaID, NomeCliente, QuantidadeIngressos, ValorTotal, NroCartao, ");
                stbSQL.Append("BandeiraID, TaxaEntrega, DataVenda, Status, NivelRisco, RowNumber, Registros ");
                stbSQL.Append("FROM tbOrdenada, tbCount ");
                stbSQL.Append("WHERE RowNumber BETWEEN " + this.IndexAtual + " AND " + (this.IndexAtual + this.QuantidadePorPagina - 1) + " ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    item = new EstruturaTransacoesResultado();
                    item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    item.Nome = bd.LerString("NomeCliente");
                    item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                    item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                    item.Cartao = bd.LerString("NroCartao");

                    item.Bandeira = bd.LerInt("BandeiraID");
                    string[] bandeiras = Bandeiras(item.Bandeira);
                    item.ImagemBandeira = bandeiras[0];
                    item.NomeBandeira = bandeiras[1];

                    item.Status = bd.LerString("Status");


                    string[] status = StatusCompra(item.Status, bd.LerInt("NivelRisco"));
                    item.Status = status[0];
                    item.StatusColor = status[1];
                    item.AguardandoVisible = Convert.ToBoolean(status[2]);
                    item.AprovadoVisible = Convert.ToBoolean(status[3]);
                    item.FraudeVisible = Convert.ToBoolean(status[4]);
                    item.EmAnaliseVisible = Convert.ToBoolean(status[5]);

                    item.Entrega = bd.LerString("TaxaEntrega");
                    string data = bd.LerDateTime("DataVenda").ToString();
                    item.Data = data.Remove(data.Length - 3);
                    item.AprovadoArgument = item.VendaBilheteriaID + ";P";
                    item.FraudeArgument = item.VendaBilheteriaID + ";F";
                    item.AguardandoArgument = item.VendaBilheteriaID + ";A";
                    item.EmAnaliseArgument = item.VendaBilheteriaID + ";N";

                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");
                    this.Add(item);
                }
                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaFraudesNaoCanceladas()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tVendaBilheteria.ID AS VendaBilheteriaID,tVendaBilheteria.Senha, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(i.ID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status ");
                stbSQL.Append(", ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo, Score, RetornoAccertify  ");
                stbSQL.Append("FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID ");
                stbSQL.Append("INNER JOIN tIngresso i (NOLOCK) ON tVendaBilheteria.ID = i.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN  tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN  tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append("WHERE tVendaBilheteria.Status = '" + VendaBilheteria.FRAUDE + "' AND (tVendaBilheteria.VendaCancelada IS NULL OR tVendaBilheteria.VendaCancelada = 'F') ");
                stbSQL.Append("GROUP BY tCliente.Nome, tVendaBilheteria.ID,tVendaBilheteria.Senha, tVendaBilheteria.ValorTotal, tCartao.NroCartao, tAntiFraudeMotivoTipo.Nome,");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status , tCliente.NomeFantasia , tCliente.CNPJ, Score, RetornoAccertify ");
                stbSQL.Append("ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;
                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;

                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];

                        item.Status = bd.LerString("Status");
                        item.Score = bd.LerInt("Score");
                        item.RetornoAccertify = bd.LerString("RetornoAccertify");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);
                        item.Motivo = bd.LerString("Motivo");
                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;
                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }

                }
                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaComprasPorCartao(string Filtros)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("WITH tbGeral AS ( ");

                stbSQL.Append("SELECT ");
                stbSQL.Append("tCartao.NroCartao  ");
                stbSQL.Append(", tCartao.ID CartaoID  ");
                stbSQL.Append(", tCartao.BandeiraID  ");
                stbSQL.Append(", tCartao.CheckSumCartao ");
                stbSQL.Append(", count(distinct tIngresso.VendaBilheteriaID) Compras  ");
                stbSQL.Append(", Count( distinct tIngresso.ID) Ingressos ");
                stbSQL.Append("FROM	tCartao (NOLOCK) ");
                stbSQL.Append("INNER JOIN	tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN	tVendaBilheteria (NOLOCK) ON vbfp.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("INNER JOIN	tIngresso (NOLOCK) ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID ");
                stbSQL.Append("INNER JOIN	tBandeira (NOLOCK) ON tCartao.BandeiraID = tBandeira.ID ");
                stbSQL.Append("INNER JOIN  tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append("LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN  tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN  tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("WHERE 	" + Filtros);
                stbSQL.Append("GROUP BY	tCartao.NroCartao  ");
                stbSQL.Append(", tCartao.BandeiraID  ");
                stbSQL.Append(", tCartao.CheckSumCartao ");
                stbSQL.Append(", tCartao.ID ");
                stbSQL.Append("),");
                stbSQL.Append(" tbCount AS (SELECT Count(NroCartao) AS Registros FROM tbGeral), ");
                stbSQL.Append("tbOrdenada AS ( ");
                stbSQL.Append("SELECT");
                stbSQL.Append("	NroCartao  ");
                stbSQL.Append(", BandeiraID  ");
                stbSQL.Append(", CartaoID  ");
                stbSQL.Append(", CheckSumCartao ");
                stbSQL.Append(", Compras  ");
                stbSQL.Append(", Ingressos ");
                stbSQL.Append(", ROW_NUMBER() OVER (ORDER BY Ingressos DESC ) AS 'RowNumber' ");
                stbSQL.Append(" FROM tbGeral) ");
                stbSQL.Append(" SELECT");
                stbSQL.Append("	NroCartao  ");
                stbSQL.Append(", BandeiraID  ");
                stbSQL.Append(", CartaoID  ");
                stbSQL.Append(", CheckSumCartao ");
                stbSQL.Append(", Compras  ");
                stbSQL.Append(", Ingressos  ");
                stbSQL.Append(", RowNumber, Registros , Sum(Compras) totalGeralCompras , Sum(Ingressos) totalGeralIngressos ");
                stbSQL.Append(" FROM tbOrdenada, tbCount ");

                if (!this.SemPaginacao)
                    stbSQL.Append(" WHERE RowNumber BETWEEN " + this.IndexAtual + " AND " + (this.IndexAtual + this.QuantidadePorPagina - 1) + "");

                stbSQL.Append(" GROUP BY CartaoID, NroCartao , BandeiraID , CheckSumCartao ,Compras , Ingressos  , RowNumber, Registros  ");

                if (this.Ordenacao.Length > 0)
                    stbSQL.Append(" ORDER BY 1 " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    item = new EstruturaTransacoesResultado();
                    item.CheckSumCartao = bd.LerString("CheckSumCartao");
                    item.Cartao = bd.LerString("NroCartao");
                    item.Bandeira = bd.LerInt("BandeiraID");
                    item.ComprasPorCartao = bd.LerInt("Compras");
                    item.ComprasQtdeIngressos = bd.LerInt("Ingressos");
                    item.CartaoID = bd.LerInt("CartaoID");
                    item.Bandeira = bd.LerInt("BandeiraID");
                    string[] bandeiras = Bandeiras(item.Bandeira);
                    item.ImagemBandeira = bandeiras[0];
                    item.NomeBandeira = bandeiras[1];

                    if (this.QuantidadePorPagina != 0)
                        this.Registros = bd.LerInt("Registros");
                    this.Add(item);
                }
                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaComprasPorCartaoDetalhes(string filtros)
        {
            if (this.FiltroQS.CartaoID == 0)
                throw new Exception("CartaoID Inválido.");
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tCartao.ID CartaoID, tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(i.ID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status,tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco ");
                stbSQL.Append(" ,ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo ");
                stbSQL.Append("FROM tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID ");
                stbSQL.Append("INNER JOIN tIngresso i (NOLOCK) ON tVendaBilheteria.ID = i.VendaBilheteriaID ");
                stbSQL.Append(" LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append(" LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append("INNER JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append("INNER JOIN  tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID    ");
                stbSQL.Append("INNER JOIN  tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append("WHERE tCartao.ID =" + this.FiltroQS.CartaoID);

                if (!string.IsNullOrEmpty(filtros))
                    stbSQL.Append(" AND " + filtros + " ");

                stbSQL.Append(" GROUP BY tCartao.ID, tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append("tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status,tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco , tCliente.NomeFantasia , tCliente.CNPJ,tAntiFraudeMotivoTipo.Nome ");
                stbSQL.Append("ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;
                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;
                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];
                        item.Status = bd.LerBoolean("VendaCancelada") ? "C" : bd.LerString("Status");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);
                        item.Motivo = bd.LerString("Motivo");
                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;
                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }
                }

                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public EstruturaTransacoesResultadoLista CarregarListaComprasPorCliente(string filtros)
        {
            if (this.FiltroQS.ClienteID == 0)
                throw new Exception("Cliente Inválido.");
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT IsNull(tCartao.ID,0) AS CartaoID, tVendaBilheteria.ID AS VendaBilheteriaID, ");
                stbSQL.Append("CASE WHEN tCliente.CNPJ IS NULL OR tCliente.CNPJ = '' THEN  tCliente.Nome COLLATE Latin1_General_CI_AI ");
                stbSQL.Append("ELSE tCliente.NomeFantasia COLLATE Latin1_General_CI_AI END AS NomeCliente, ");
                stbSQL.Append("Count(DISTINCT log.IngressoID) AS QuantidadeIngressos, tVendaBilheteria.ValorTotal, IsNull(tCartao.NroCartao, '---') AS NroCartao, ");
                stbSQL.Append("IsNull(tFormaPagamento.BandeiraID, 0) AS BandeiraID, tEntrega.Nome AS TaxaEntrega, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco ");
                stbSQL.Append(" ,ISNULL(tAntiFraudeMotivoTipo.Nome,'')  as Motivo ");
                stbSQL.Append("FROM tVendaBilheteria (NOLOCK) ");
                stbSQL.Append(" INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON tVendaBilheteria.ID = vbfp.VendaBilheteriaID ");
                stbSQL.Append(" INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = vbfp.FormaPagamentoID ");
                stbSQL.Append(" INNER JOIN tIngressoLog log (NOLOCK) ON tVendaBilheteria.ID = log.VendaBilheteriaID ");
                stbSQL.Append(" LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID ");
                stbSQL.Append(" LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID ");
                stbSQL.Append(" INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID ");
                stbSQL.Append(" LEFT JOIN tCartao (NOLOCK) ON vbfp.CartaoID = tCartao.ID ");
                stbSQL.Append(" INNER JOIN  tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID ");
                stbSQL.Append(" INNER JOIN  tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivo ON   tVendaBilheteria.ID = tAntiFraudeMotivo.VendaBilheteriaID ");
                stbSQL.Append("LEFT JOIN tAntiFraudeMotivoTipo on tAntiFraudeMotivo.AntiFraudeMotivoTipoID = tAntiFraudeMotivoTipo.ID ");
                stbSQL.Append("WHERE tCliente.ID =" + this.FiltroQS.ClienteID + " ");

                if (!string.IsNullOrEmpty(filtros))
                    stbSQL.Append(" AND " + filtros + " ");

                stbSQL.Append(" GROUP BY tCartao.ID, tCliente.Nome, tVendaBilheteria.ID, tVendaBilheteria.ValorTotal, tCartao.NroCartao, ");
                stbSQL.Append(" tFormaPagamento.BandeiraID, tEntrega.Nome, tVendaBilheteria.DataVenda, tVendaBilheteria.Status,tVendaBilheteria.VendaCancelada, tVendaBilheteria.NivelRisco, tCliente.NomeFantasia , tCliente.CNPJ, tAntiFraudeMotivoTipo.Nome ");
                stbSQL.Append(" ORDER BY " + this.Ordenacao);

                bd.Consulta(stbSQL.ToString());
                int c = 0;
                int pagAtual = 1;

                int idVendaAnterior = 0;

                while (bd.Consulta().Read())
                {
                    int vendaAtual = bd.LerInt("VendaBilheteriaID");
                    if (idVendaAnterior != vendaAtual)
                    {
                        if (idVendaAnterior > 0)
                        {
                            this.Add(item);
                        }
                        idVendaAnterior = vendaAtual;

                        item = new EstruturaTransacoesResultado();
                        item.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                        item.Nome = bd.LerString("NomeCliente");
                        item.QuantidadeIngressos = bd.LerInt("QuantidadeIngressos");
                        item.ValorTotal = bd.LerDecimal("ValorTotal").ToString("c");
                        item.Cartao = bd.LerString("NroCartao");

                        item.Bandeira = bd.LerInt("BandeiraID");
                        string[] bandeiras = Bandeiras(item.Bandeira);
                        item.ImagemBandeira = bandeiras[0];
                        item.NomeBandeira = bandeiras[1];
                        item.Status = bd.LerBoolean("VendaCancelada") ? "C" : bd.LerString("Status");

                        string[] status = StatusCompra(item.Status, 1);
                        item.Status = status[0];
                        item.StatusColor = status[1];
                        item.AguardandoVisible = Convert.ToBoolean(status[2]);
                        item.AprovadoVisible = Convert.ToBoolean(status[3]);
                        item.FraudeVisible = Convert.ToBoolean(status[4]);
                        item.EmAnaliseVisible = Convert.ToBoolean(status[5]);
                        item.Motivo = bd.LerString("Motivo");
                        item.Entrega = bd.LerString("TaxaEntrega");
                        item.Data = bd.LerDateTime("DataVenda").ToString("dd/MM/yyyy HH:mm");
                        pagAtual = (c % this.QuantidadePorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        item.Pagina = pagAtual;
                        c++;
                    }
                    else
                    {
                        string novoMotivo = bd.LerString("Motivo");

                        if (novoMotivo.Length > 0)
                        {
                            if (item.Motivo.Length > 0)
                            {
                                item.Motivo += "<br/> " + novoMotivo;
                            }
                            else
                            {
                                item.Motivo += novoMotivo;
                            }
                        }
                    }

                }
                if (idVendaAnterior > 0)
                {
                    this.Add(item);
                }

                return this;
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

        public string MontarFiltroTransacoes()
        {
            StringBuilder stbSQL = new StringBuilder();

            if (this.FiltroQS.BandeiraID > 0)
                stbSQL.Append("tFormaPagamento.BandeiraID = " + this.FiltroQS.BandeiraID);
            else
            {
                stbSQL.Append("tFormaPagamento.BandeiraID IN (");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Visa + ", ");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Mastercard + ", ");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Amex + ", ");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Aura + ", ");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Diners + ", ");
                stbSQL.Append((int)IRLib.Cartao.enumBandeira.Hipercard + ") ");
            }

            if (this.FiltroQS.CanalID > 0)
                stbSQL.Append(" AND tLoja.CanalID = " + this.FiltroQS.CanalID);
            else
                stbSQL.Append(" AND tLoja.CanalID IN (1,2,843,845,844)");

            switch ((VendaBilheteria.StatusAntiFraude)Convert.ToChar((this.FiltroQS.Status)))
            {
                case VendaBilheteria.StatusAntiFraude.Aguardando:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada = 'F' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.EmAnalise:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada = 'F' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Aprovado:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada = 'F' or tVendaBilheteria.VendaCancelada is null )");
                    if (!string.IsNullOrEmpty(this.FiltroQS.Cartao))
                        stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado);
                    else
                        stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco);

                    break;
                case VendaBilheteria.StatusAntiFraude.Fraude:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada = 'F' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Todos:
                    if (!string.IsNullOrEmpty(this.FiltroQS.Cartao))
                        stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' OR ");
                    else
                        stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");

                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Cancelado:
                    if (!string.IsNullOrEmpty(this.FiltroQS.Cartao))
                        stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' OR ");
                    else
                        stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");

                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    stbSQL.Append(" AND tVendaBilheteria.VendaCancelada = 'T' ");
                    break;
                default:
                    throw new Exception("O Status a ser pesquisado está inválido.");
            }

            if (!string.IsNullOrEmpty(this.FiltroQS.Auth))
                stbSQL.Append("AND  vbfp.NumeroAutorizacao LIKE '" + this.FiltroQS.Auth.Replace("'", "") + "%' ");

            if (!string.IsNullOrEmpty(this.FiltroQS.NSU))
                stbSQL.Append("AND vbfp.NSUHost LIKE '" + this.FiltroQS.NSU.Replace("'", "") + "%' ");

            if (!string.IsNullOrEmpty(this.FiltroQS.IP))
                stbSQL.Append("AND IP LIKE '" + this.FiltroQS.IP.Replace("'", "") + "%' ");

            string cartaoCripto = string.Empty;

            if (!string.IsNullOrEmpty(this.FiltroQS.Cartao))
                if (this.FiltroQS.Cartao.Length >= 16)
                {
                    using (HashAlgorithm hashAlg = HashAlgorithm.Create("SHA256"))
                    {
                        byte[] pwordData = Encoding.Default.GetBytes(this.FiltroQS.Cartao.Replace("'", ""));
                        byte[] hash = hashAlg.ComputeHash(pwordData);
                        cartaoCripto = Convert.ToBase64String(hash);
                    }
                    stbSQL.Append("AND tCartao.CheckSumCartao = '" + cartaoCripto + "' ");
                }
                else if (this.FiltroQS.Cartao.Length > 6 && this.FiltroQS.Cartao.Length < 16)
                {
                    stbSQL.Append("AND tCartao.NroCartao LIKE '" + this.FiltroQS.Cartao.Replace("'", "").Substring(0, 6) + "%'");
                    stbSQL.Append("AND tCartao.NroCartao LIKE '%" + this.FiltroQS.Cartao.Replace("'", "").Substring(6, this.FiltroQS.Cartao.Length - 6) + "'");
                }
                else if (this.FiltroQS.Cartao.Length == 6)
                    stbSQL.Append("AND tCartao.NroCartao LIKE '" + this.FiltroQS.Cartao.Replace("'", "").Substring(0, 6) + "%'");

            if (!string.IsNullOrEmpty(this.FiltroQS.DataInicial))
                stbSQL.Append(" AND DataVenda >= '" + this.FiltroQS.DataInicial.Replace("'", "") + "' ");

            if (!string.IsNullOrEmpty(this.FiltroQS.DataFinal))
                stbSQL.Append(" AND DataVenda <= '" + this.FiltroQS.DataFinal.Replace("'", "") + "' ");

            return stbSQL.ToString();
        }

        public string MontarFiltroTransacoesEventoCompra()
        {
            StringBuilder stbSQL = new StringBuilder();

            if (this.FiltroQS.EventoID > 0)
                stbSQL.Append(" tApresentacao.EventoID = " + this.FiltroQS.EventoID);

            if (this.FiltroQS.ApresentacaoID > 0)
                stbSQL.Append(" AND tApresentacao.ID = " + this.FiltroQS.ApresentacaoID);
            else
            {
                if (!string.IsNullOrEmpty(this.FiltroQS.DataInicial))
                    stbSQL.Append(" AND tApresentacao.Horario >= '" + this.FiltroQS.DataInicial.Replace("'", "") + "' ");

                if (!string.IsNullOrEmpty(this.FiltroQS.DataFinal))
                    stbSQL.Append(" AND tApresentacao.Horario <= '" + this.FiltroQS.DataFinal.Replace("'", "") + "' ");
            }

            switch ((VendaBilheteria.StatusAntiFraude)(Convert.ToChar(this.FiltroQS.Status)))
            {
                case VendaBilheteria.StatusAntiFraude.Aguardando:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.EmAnalise:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Aprovado:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco);
                    break;
                case VendaBilheteria.StatusAntiFraude.Fraude:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Todos:
                    stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");
                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Cancelado:
                    stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");
                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    stbSQL.Append(" AND tVendaBilheteria.VendaCancelada = 'T' ");
                    break;
                default:
                    throw new Exception("O Status a ser pesquisado está inválido.");
            }
            return stbSQL.ToString();
        }

        public string MontarFiltroTransacoesPorFormaEntrega()
        {
            StringBuilder stbSQL = new StringBuilder();

            if (!string.IsNullOrEmpty(this.FiltroQS.EntregaControleID))
            {
                string[] EntregaControleID = this.FiltroQS.EntregaControleID.Split(';');

                stbSQL.Append(" ( ");
                for (int cont = 0; cont < EntregaControleID.Length - 1; cont++)
                {
                    if (cont == 0)
                        stbSQL.Append(" tEntregaControle.ID = " + EntregaControleID[cont]);
                    else
                        stbSQL.Append(" OR tEntregaControle.ID = " + EntregaControleID[cont]);
                }
                stbSQL.Append(" ) ");
            }

            if (!string.IsNullOrEmpty(this.FiltroQS.DataInicial))
                stbSQL.Append(" AND tEntregaAgenda.Data >= '" + this.FiltroQS.DataInicial.Replace("'", "") + "' ");

            if (!string.IsNullOrEmpty(this.FiltroQS.DataFinal))
                stbSQL.Append(" AND tEntregaAgenda.Data <= '" + this.FiltroQS.DataFinal.Replace("'", "") + "' ");

            switch ((VendaBilheteria.StatusAntiFraude)(Convert.ToChar(this.FiltroQS.Status)))
            {
                case VendaBilheteria.StatusAntiFraude.Aguardando:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.EmAnalise:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Aprovado:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco);
                    break;
                case VendaBilheteria.StatusAntiFraude.Fraude:
                    stbSQL.Append(" AND (tVendaBilheteria.VendaCancelada != 'T' or tVendaBilheteria.VendaCancelada is null )");
                    stbSQL.Append(" AND tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "' ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Todos:
                    stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");
                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    break;
                case VendaBilheteria.StatusAntiFraude.Cancelado:
                    stbSQL.Append(" AND (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' AND tVendaBilheteria.NivelRisco = " + (int)VendaBilheteria.enumNivelRisco.CompraDeRisco + ") OR ");
                    stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
                    stbSQL.Append(" AND tVendaBilheteria.VendaCancelada = 'T' ");
                    break;
                default:
                    throw new Exception("O Status a ser pesquisado está inválido.");
            }
            return stbSQL.ToString();
        }

        public string MontarFiltroOutrasCompras()
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append(" (tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aguardando + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' OR tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Aprovado + "' OR ");
            stbSQL.Append(" tVendaBilheteria.Status = '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "') ");
            return stbSQL.ToString();
        }

        public void MontarOrdenacaoTransacoes()
        {
            #region Switch Primeira Ordenacao
            switch (this.Ordenacao1)
            {
                case 1:
                    this.Ordenacao = " NomeCliente DESC , ";
                    break;
                case 2:
                    this.Ordenacao = " NomeCliente ASC , ";
                    break;
                case 3:
                    this.Ordenacao = " DataVenda ASC , ";
                    break;
                case 4:
                    this.Ordenacao = " DataVenda DESC , ";
                    break;
                case 5:
                    this.Ordenacao = " ValorTotal ASC , ";
                    break;
                case 6:
                    this.Ordenacao = " ValorTotal DESC , ";
                    break;
                case 0:
                    this.Ordenacao = "";
                    break;
                default:
                    this.Ordenacao = " NomeCliente  ASC , ";
                    break;
            }
            #endregion

            #region Switch Segunda Ordenacao

            switch (this.Ordenacao2)
            {
                case 1:
                    if (this.Ordenacao1 == 1 || this.Ordenacao1 == 2 || this.Ordenacao1 == 0)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " NomeCliente DESC ";
                    break;
                case 2:
                    if (this.Ordenacao1 == 1 || this.Ordenacao1 == 2 || this.Ordenacao1 == 0)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " NomeCliente ASC ";
                    break;
                case 3:
                    if (this.Ordenacao1 == 3 || this.Ordenacao1 == 4)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " DataVenda DESC";
                    break;
                case 4:
                    if (this.Ordenacao1 == 3 || this.Ordenacao1 == 4)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " DataVenda ASC";
                    break;

                case 5:
                    if (this.Ordenacao1 == 5 || this.Ordenacao1 == 6)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " ValorTotal ASC ";
                    break;
                case 6:
                    if (this.Ordenacao1 == 5 || this.Ordenacao1 == 6)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " ValorTotal DESC ";
                    break;
                case 0:
                    if (this.Ordenacao.Length > 0)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " NomeCliente DESC ";

                    break;
                default:
                    if (this.Ordenacao1 == 3 || this.Ordenacao1 == 4)
                    {
                        if (this.Ordenacao.Length > 0)
                            this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    }
                    else
                        this.Ordenacao += " DataVenda DESC ";
                    break;
            }
            #endregion
        }

        public void MontarOrdenacaoNova()
        {
            switch (this.FiltroQS.Ordenacao1)
            {
                case 0:
                    this.Ordenacao = "";
                    break;
                case 1:
                    this.Ordenacao = " NomeCliente, ";
                    break;
                case 2:
                    this.Ordenacao = " NomeCliente ASC, ";
                    break;
            }

            switch (this.FiltroQS.Ordenacao2)
            {
                case 0:
                    if (this.Ordenacao.Length > 0)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " NomeCliente ";
                    break;
                case 1:
                    this.Ordenacao += " DataVenda DESC ";
                    break;

                case 2:
                    this.Ordenacao += " DataVenda ASC ";
                    break;
            }
        }

        public void MontarOrdenacao()
        {
            switch (this.Ordenacao1)
            {
                case 2:
                    this.Ordenacao = " NomeCliente ASC, ";
                    break;
                case 0:
                    this.Ordenacao = "";
                    break;

                default:
                    this.Ordenacao = " NomeCliente DESC, ";
                    break;


            }

            switch (this.Ordenacao2)
            {
                case 0:
                    if (this.Ordenacao.Length > 0)
                        this.Ordenacao = this.Ordenacao.Remove(this.Ordenacao.Length - 2, 2);
                    else
                        this.Ordenacao += " NomeCliente DESC ";

                    break;
                case 2:
                    this.Ordenacao += " DataVenda ASC ";
                    break;

                case 4:
                    this.Ordenacao += " DataVenda ASC ";
                    break;
                default:
                    this.Ordenacao += " DataVenda DESC ";
                    break;
            }
        }

        public void MontarOrdenacaoCartao()
        {
            switch (this.Ordenacao1)
            {
                case 2:
                    this.Ordenacao = "ASC";
                    break;
                default:
                    this.Ordenacao = "DESC";
                    break;
            }
        }

        public void MontarOrdenacaoCliente()
        {
            switch (this.Ordenacao1)
            {
                case 2:
                    this.Ordenacao = " DataVenda ASC ";
                    break;
                default:
                    this.Ordenacao = " DataVenda DESC ";
                    break;
            }
        }

        public string[] StatusCompra(string statusC, int nivelRisco)
        {
            string[] retorno = new string[6];


            switch (Convert.ToChar(statusC))
            {
                case (char)VendaBilheteria.StatusAntiFraude.Aguardando:
                    retorno[0] = "Aguardando";
                    retorno[1] = "Yellow";
                    retorno[2] = "false";
                    retorno[3] = "true";
                    retorno[4] = "true";
                    retorno[5] = "true";
                    break;
                case (char)VendaBilheteria.StatusAntiFraude.Fraude:
                    retorno[0] = nivelRisco > 2 ? "Fraude Canc." : "Fraude";
                    retorno[1] = "Red";
                    retorno[2] = nivelRisco > 2 ? "false" : "true";
                    retorno[3] = nivelRisco > 2 ? "false" : "true";
                    retorno[4] = "false";
                    retorno[5] = "true";
                    break;
                case (char)VendaBilheteria.StatusAntiFraude.Aprovado:
                    retorno[0] = "Aprovado";
                    retorno[1] = "Green";
                    retorno[2] = "true";
                    retorno[3] = "false";
                    retorno[4] = "true";
                    retorno[5] = "false";
                    break;
                case (char)VendaBilheteria.StatusAntiFraude.Cancelado:
                    retorno[0] = "Cancelado";
                    retorno[1] = "Brown";
                    retorno[2] = "false";
                    retorno[3] = "false";
                    retorno[4] = "false";
                    retorno[5] = "false";
                    break;
                case (char)VendaBilheteria.StatusAntiFraude.EmAnalise:
                    retorno[0] = "Em Análise";
                    retorno[1] = "Brown";
                    retorno[2] = "true";
                    retorno[3] = "true";
                    retorno[4] = "true";
                    retorno[5] = "false";
                    break;

            }

            return retorno;
        }

        public string[] Bandeiras(int bandeiraID)
        {
            string[] retorno = new string[2];
            string imagem = string.Empty;
            string nome = string.Empty;

            switch (bandeiraID)
            {
                case 2: //VISA
                    imagem = VISA;
                    nome = "Visa";
                    break;
                case 3: //MASTER
                    imagem = MASTER;
                    nome = "Master";
                    break;
                case 5: //AMEX
                    imagem = AMEX;
                    nome = "Amex";
                    break;
                case 9: //REDESHOP
                    imagem = REDESHOP;
                    nome = "Redeshop";
                    break;
                case 12: //HIPER
                    imagem = HIPER;
                    nome = "Hipercard";
                    break;
                case 13: //AURA
                    imagem = AURA;
                    nome = "Aura";
                    break;
                case 4: //DINERS
                    imagem = DINERS;
                    nome = "Diners";
                    break;
                default: //Outros
                    imagem = OUTROS;
                    nome = "Outros";
                    break;
            }
            retorno[0] = imagem;
            retorno[1] = nome;
            return retorno;
        }
    }

    public class EstruturaFiltro
    {
        public string EntregaControleID { get; set; }
        public string IP { get; set; }
        public string NSU { get; set; }
        public string Auth { get; set; }
        public int CanalID { get; set; }
        public int BandeiraID { get; set; }
        public string Cartao { get; set; }
        public int EventoID { get; set; }
        public string DataInicial { get; set; }
        public int Dias { get; set; }
        public string Completo { get; set; }
        public int Ordenacao1 { get; set; }
        public int Ordenacao2 { get; set; }
        public int CartaoID { get; set; }
        public bool ListagemCompleta
        {
            get
            {
                return Completo == "T";
            }
        }
        public string Status { get; set; }
        public string DataFinal { get; set; }
        public int ApresentacaoID { get; set; }
        public int ClienteID { get; set; }
    }

}
