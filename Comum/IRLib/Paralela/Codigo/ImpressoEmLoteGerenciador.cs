using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ClientObjects.Arvore;
using IRLib.Paralela.Codigo.ModuloLogistica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    /// <summary>
    /// Gerenciador do ImpressoEmLoteGerenciador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ImpressoEmLoteGerenciadorParalela : MarshalByRefObject
    {

        public const string DATA_VENDA = "DataVenda";
        public const string LOJA = "Loja";
        public const string ENTREGA = "Entrega";
        public const string EVENTO = "Evento";
        public const string HORARIO = "Apresentacao";
        public const string OBS = "Observações";
        public const string QTDE = "Qtd";
        public const string SENHA = "Senha";
        public const string CLIENTE = "Cliente";
        public const string IMPRESSAO = "Impressao";
        public const string IMPRESSO = "Impresso";
        public const string COMPROVANTE_QUANTIDADE = "ComprovanteQuantidade";
        public const string VENDA_BILHETERIA_ID = "VendaBilheteriaID";
        public const string TIPO_VENDA = "TipoVenda";
        public const string BIN = "Bin";
        public const string CLIENTE_STATUS = "ClienteStatus";
        public const string IMPRIMIR = "Imprimir";
        public const string TIPO_TAXA = "TipoTaxa";
        public const string STATUS = "Status";
        public const string NOME_ENTREGA = "NomeEntrega";
        public const string RG_ENTREGA = "RGEntrega";
        public const string NUMERO_ENTREGA = "NumeroEntrega";
        public const string COMPLEMENTO_ENTREGA = "ComplementoEntrega";
        public const string BAIRRO_ENTREGA = "BairroEntrega";
        public const string CEP_ENTREGA = "CEPEntrega";
        public const string ENDERECO_ENTREGA = "EnderecoEntrega";
        public const string CIDADE_ENTREGA = "CidadeEntrega";
        public const string ESTADO_ENTREGA = "EstadoEntrega";

        public const string ENTREGA_NOME = "EntregaNome";
        public const string PERIODO_ENTREGA = "PeriodoEntrega";
        public const string DATA_ENTREGA = "DataEntrega";
        public const string AREA_ENTREGA = "AreaEntrega";
        public const string CODIGO_RASTREIO_CORREIO = "CodigoRastreioCorreio";


        public const string MENOR_APRESENTACAO = "MenorApresentacao";
        private bool SoImprimirDataApresentacao = new ConfigGerenciadorParalela().ImprimirSomenteDataApresentacao();
        //BD bd;
        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        public ImpressoEmLoteGerenciadorParalela()
        {
            //	bd = new BD();
        }

        private DataTable estruturaGrid()
        {

            DataTable grid = new DataTable("Grid");

            grid.Columns.Add(ENTREGA, typeof(string));
            grid.Columns.Add(TIPO_TAXA, typeof(string));
            grid.Columns.Add(LOJA, typeof(string));
            grid.Columns.Add(SENHA, typeof(string));
            grid.Columns.Add(DATA_VENDA, typeof(DateTime));
            grid.Columns.Add(HORARIO, typeof(DateTime));
            grid.Columns.Add(CLIENTE, typeof(string));
            grid.Columns.Add(NOME_ENTREGA, typeof(string));
            grid.Columns.Add(RG_ENTREGA, typeof(string));
            grid.Columns.Add(ENDERECO_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(NUMERO_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(COMPLEMENTO_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(BAIRRO_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(CEP_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(ESTADO_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(CIDADE_ENTREGA, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(QTDE, typeof(int));
            grid.Columns.Add(IMPRESSO, typeof(string)).DefaultValue = "Não";
            grid.Columns.Add(IMPRESSAO, typeof(bool)).DefaultValue = false;
            grid.Columns.Add(IMPRIMIR, typeof(bool)).DefaultValue = false;
            grid.Columns.Add(VENDA_BILHETERIA_ID, typeof(int));
            grid.Columns.Add(TIPO_VENDA, typeof(string));
            grid.Columns.Add(COMPROVANTE_QUANTIDADE, typeof(int));
            grid.Columns.Add(CLIENTE_STATUS, typeof(string)).DefaultValue = string.Empty;
            grid.Columns.Add(STATUS, typeof(string));
            grid.Columns.Add(ENTREGA_NOME, typeof(string));
            grid.Columns.Add(PERIODO_ENTREGA, typeof(string));
            grid.Columns.Add(DATA_ENTREGA, typeof(string));
            grid.Columns.Add(AREA_ENTREGA, typeof(string));
            grid.Columns.Add(CODIGO_RASTREIO_CORREIO, typeof(string));
            grid.Columns.Add(EVENTO, typeof(string));
            return grid;

        }

        /// <summary>
        /// Pesquisa pela vendaID
        /// </summary>
        /// <returns></returns>
        public DataTable ImpressaoVenda(int vendaID, int lojaID)
        {

            BD bd = new BD();
            try
            {
                DataTable tabelaImpressao = Ingresso.EstruturaImpressao(); //carrega tabela com a estrutura de informações que um ingresso deve conter

                bd.Consulta("EXEC Proc_IngressosImpressaoPorVenda4 " + vendaID + ", 0 ," + lojaID);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("VendaBilheteriaID") == vendaID)
                    {
                        DataRow linha = tabelaImpressao.NewRow();

                        linha["ID"] = bd.LerInt("ID");
                        linha["IngressoID"] = bd.LerInt("IngressoID");
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
                        linha["Acesso"] = bd.LerString("Acesso");
                        linha["Produto"] = bd.LerBoolean("Produto");
                        linha["Cortesia"] = bd.LerString("Cortesia");
                        linha["Preco"] = bd.LerString("Preco");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        linha["Codigo"] = bd.LerString("Codigo");
                        linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                        linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                        linha["Loja"] = bd.LerString("Loja");
                        linha["Senha"] = bd.LerString("Senha");
                        linha["TaxaEntregaID"] = bd.LerInt("TaxaEntregaID");
                        linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                        linha["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                        linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        linha["Cliente"] = bd.LerString("Cliente");
                        linha["ClienteCPF"] = bd.LerString("ClienteCPF");
                        linha["ClienteID"] = bd.LerInt("ClienteID");
                        linha["ClienteEnderecoID"] = bd.LerInt("ClienteEnderecoID");
                        linha["Status"] = bd.LerString("Status");
                        linha["VendaBilheteriaID"] = bd.LerInt("VendaBilheteriaID");
                        linha["LocalNome"] = bd.LerString("LocalNome");
                        linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                        linha["LocalCep"] = bd.LerString("LocalCep");
                        linha["LocalCidade"] = bd.LerString("LocalCidade");
                        linha["LocalEstado"] = bd.LerString("LocalEstado");
                        linha["LocalPais"] = bd.LerString("Pais");
                        linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");
                        linha["Bin"] = bd.LerString("Bin");
                        linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                        linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                        linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                        linha["VendaBilheteriaStatus"] = bd.LerString("VendaBilheteriaStatus");
                        linha["EntregaNome"] = bd.LerString("EntregaNome");
                        linha["PeriodoEntrega"] = bd.LerString("PeriodoEntrega");
                        DateTime DataAux = bd.LerDateTime("DataEntrega");
                        linha["DataEntrega"] = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                        linha["AreaEntrega"] = bd.LerString("AreaEntrega");
                        linha["TipoImpressao"] = bd.LerString("TipoImpressao");
                        linha["NomeCartao"] = bd.LerString("NomeCartao");


                        linha["Alvara"] = bd.LerString("Alvara");
                        linha["FonteImposto"] = bd.LerString("FonteImposto");
                        linha["AVCB"] = bd.LerString("AVCB");
                        linha["PorcentagemImposto"] = bd.LerDecimal("PorcentagemImposto");

                        linha["Lotacao"] = bd.LerDecimal("Lotacao");
                        linha["DataEmissaoAlvara"] = bd.LerStringFormatoDataHora("DataEmissaoAlvara");
                        linha["DataValidadeAlvara"] = bd.LerStringFormatoDataHora("DataValidadeAlvara");
                        linha["DataEmissaoAvcb"] = bd.LerStringFormatoDataHora("DataEmissaoAvcb");
                        linha["DataValidadeAvcb"] = bd.LerStringFormatoDataHora("DataValidadeAvcb");

                        tabelaImpressao.Rows.Add(linha);
                    }
                }
                bd.Fechar();

                return tabelaImpressao;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EstruturaImpressaoEtiqueta ImpressaoEtiquetaVenda(int vendaID, int quantidade)
        {
            BD bd = new BD();
            try
            {
                EstruturaImpressaoEtiqueta item = new EstruturaImpressaoEtiqueta();

                string sql = "SELECT tVendaBilheteria.ID, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, tVendaBilheteria.Status, " +
                    "tCliente.NomeEntrega, tCliente.RGEntrega, tCliente.EnderecoEntrega, tCliente.NumeroEntrega, tCliente.CidadeEntrega, tCliente.EstadoEntrega, tCliente.ComplementoEntrega, tCliente.BairroEntrega, tCliente.CEPEntrega, " +
                    "MIN(ap.Horario) AS Horario, IsNull(tx.DiasTriagem, 0) AS DiasTriagem " +
                    "FROM tVendaBilheteria (NOLOCK)" +
                    "LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID " +
                    "LEFT JOIN tTaxaEntrega tx (NOLOCK) ON tx.ID = tVendaBilheteria.TaxaEntregaID " +
                    "INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = tVendaBilheteria.ID " +
                    "INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID " +
                    "WHERE tVendaBilheteria.ID = " + vendaID +
                    " GROUP BY tVendaBilheteria.ID, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda,tVendaBilheteria.Status, IsNull(tx.DiasTriagem, 0), " +
                    "tCliente.NomeEntrega, tCliente.RGEntrega, tCliente.EnderecoEntrega, tCliente.NumeroEntrega, tCliente.CidadeEntrega, " +
                    "tCliente.EstadoEntrega, tCliente.ComplementoEntrega, tCliente.BairroEntrega, tCliente.CEPEntrega ";


                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    return null;

                item.Senha = bd.LerString(SENHA);
                item.Nome = bd.LerString(NOME_ENTREGA);
                item.RG = bd.LerString(RG_ENTREGA);
                item.Endereco = bd.LerString(ENDERECO_ENTREGA);
                item.Numero = bd.LerString(NUMERO_ENTREGA);
                item.Cidade = bd.LerString(CIDADE_ENTREGA);
                item.Estado = bd.LerString(ESTADO_ENTREGA);
                item.Complemento = bd.LerString(COMPLEMENTO_ENTREGA);
                item.Bairro = bd.LerString(BAIRRO_ENTREGA);
                item.CEP = bd.LerString(CEP_ENTREGA);
                item.QuantidadeImprimir = quantidade;


                int diasTriagem = bd.LerInt("DiasTriagem");
                DateTime dataApresentacao = bd.LerDateTime("Horario");
                DateTime dataVenda = bd.LerDateTime("DataVenda");


                if (diasTriagem > 0 && bd.LerString("Status") == VendaBilheteria.AGUARDANDO_APROVACAO)
                {
                    //Somente no dia e está no dia correto
                    if (this.SoImprimirDataApresentacao && dataApresentacao.Subtract(DateTime.Now).Days <= 0)
                        return item;
                    //Somente no dia e não está correto
                    else if (this.SoImprimirDataApresentacao)
                        return null;
                    //Já passou o periodo de triagem
                    else if (dataVenda.AddDays(diasTriagem) <= DateTime.Now)
                        return item;
                    //Quantidade de dias uteis é menor que 2
                    else if (Utilitario.DiasUteisPeriodo(DateTime.Now, dataApresentacao) <= 2)
                        return item;
                    else
                        return null;
                }
                else
                    return item;
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

        public List<EstruturaRetornoVendaValeIngresso> ImpressaoVendaValeIngresso(List<int> vendasID, int valeIngressoTipoID, int empresaID)
        {
            BD bd = new BD();
            try
            {
                List<EstruturaRetornoVendaValeIngresso> lstRetorno = new List<EstruturaRetornoVendaValeIngresso>();
                EstruturaRetornoVendaValeIngresso venda = new EstruturaRetornoVendaValeIngresso();
                venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir item;

                StringBuilder stbSQL = new StringBuilder();
                decimal valorTotal, taxaEntregaValor;
                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;

                for (int i = 0; i < vendasID.Count; i++)
                {
                    //venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();

                    stbSQL = new StringBuilder();

                    stbSQL.Append("SELECT vb.ID, vb.Senha, vb.TaxaConvenienciaValorTotal,vb.ClienteID, vb.TaxaEntregaValor, vb.ValorTotal, ");
                    stbSQL.Append("vb.Status, vb.Senha, vb.DataVenda, c.Nome AS ClienteNome, tCanal.Nome AS Canal, ");
                    stbSQL.Append("tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa ");
                    stbSQL.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    stbSQL.Append("INNER JOIN tUsuario AS u  (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    stbSQL.Append("INNER JOIN tLoja  (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    stbSQL.Append("INNER JOIN tCanal  (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    stbSQL.Append("LEFT JOIN tCliente AS c  (NOLOCK) ON vb.ClienteID=c.ID ");
                    stbSQL.Append("WHERE vb.ID=" + vendasID[i]);


                    bd.Consulta(stbSQL.ToString());

                    if (bd.Consulta().Read())
                    {
                        venda = new EstruturaRetornoVendaValeIngresso();
                        venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                        valorTotal = bd.LerDecimal("ValorTotal");
                        taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");

                        venda.Senha = bd.LerString("Senha");
                        venda.VendaBilheteriaID = bd.LerInt("ID");
                        venda.ClienteID = bd.LerInt("ClienteID");
                        venda.ClienteNome = bd.LerString("ClienteNome");
                        venda.ValorTotalEntrega = taxaEntregaValor;
                        venda.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                        venda.DataVenda = bd.LerDateTime("DataVenda");
                        venda.DataAberturaCaixa = bd.LerDateTime("DataCaixa");
                        venda.ValorTotalVenda = valorTotal;
                        venda.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                        venda.CanalVenda = bd.LerString("Canal");
                        venda.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                    }
                    else
                        throw new Exception("VendaBilheteriaID: " + vendasID[i] + " não foi encontrada");

                    bd.FecharConsulta();
                    stbSQL = new StringBuilder();

                    stbSQL.Append("SELECT vi.ID , vit.CodigoTrocaFixo,vit.SaudacaoNominal, vi.CodigoBarra, vi.CodigoTroca, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("         vit.Valor, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("     INNER JOIN tTaxaEntrega te (NOLOCK) ON te.ID = vb.TaxaEntregaID ");
                    stbSQL.Append("WHERE vi.Status ='" + IngressoLog.VENDER + "' AND viLog.VendaBilheteriaID = " + vendasID[i] + " ");
                    if (valeIngressoTipoID > 0)
                        stbSQL.Append("AND vit.ID = " + valeIngressoTipoID + " ");

                    if (empresaID > 0)
                        stbSQL.Append("AND vit.EmpresaID = " + empresaID + " ");

                    stbSQL.Append("ORDER BY viLog.TimeStamp");

                    bd.Consulta(stbSQL.ToString());

                    while (bd.Consulta().Read())
                    {
                        item = new EstruturaImpressaoVir();
                        item.ValeIngressoID = bd.LerInt("ID");
                        codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                        item.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                        item.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                        validadeData = bd.LerString("ValidadeData");
                        if (validadeData.Trim() != string.Empty)
                            item.ValidadeData = bd.LerDateTime("ValidadeData");
                        else
                        {
                            item.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                            item.ValidadeData = System.DateTime.Now.AddDays(item.ValidadeEmDiasImpressao);
                        }
                        item.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                        item.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                        item.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                        item.ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID");
                        item.Valor = bd.LerDecimal("Valor");
                        item.ValeIngressoNome = bd.LerString("Nome");
                        item.ClientePresenteado = bd.LerString("ClienteNome");

                        switch (Convert.ToChar(bd.LerString("Status")))
                        {
                            case (char)ValeIngresso.enumStatus.Vendido:
                                item.Status = ValeIngresso.enumStatus.Vendido;
                                break;
                            case (char)ValeIngresso.enumStatus.Impresso:
                                item.Status = ValeIngresso.enumStatus.Impresso;
                                break;
                        }
                        item.NomeCartao = bd.LerString("NomeCartao");

                        venda.EstruturaImpressaoVir.Add(item);
                    }
                    bd.FecharConsulta();
                    lstRetorno.Add(venda);
                }
                return lstRetorno;
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

        public List<EstruturaRetornoVendaValeIngresso> ImpressaoVendaValeIngresso(List<int> vendasID)
        {
            BD bd = new BD();
            try
            {
                List<EstruturaRetornoVendaValeIngresso> lstRetorno = new List<EstruturaRetornoVendaValeIngresso>();
                EstruturaRetornoVendaValeIngresso venda = new EstruturaRetornoVendaValeIngresso();
                venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                EstruturaImpressaoVir item;

                StringBuilder stbSQL = new StringBuilder();
                decimal valorTotal, taxaEntregaValor;
                string validadeData = string.Empty;
                string codigoTrocaFixo = string.Empty;

                for (int i = 0; i < vendasID.Count; i++)
                {
                    //venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();

                    stbSQL = new StringBuilder();

                    stbSQL.Append("SELECT vb.ID, vb.Senha, vb.TaxaConvenienciaValorTotal,vb.ClienteID, vb.TaxaEntregaValor, vb.ValorTotal, ");
                    stbSQL.Append("vb.Status, vb.Senha, vb.DataVenda, IsNull(c.Nome, '') AS ClienteNome, IsNull(c.Email, '') AS ClienteEmail, tCanal.Nome AS Canal, ");
                    stbSQL.Append("tCanal.ComprovanteQuantidade, u.Nome AS Vendedor, tCaixa.DataAbertura AS DataCaixa, ");
                    stbSQL.Append("vb.ClienteEnderecoID, IsNull(e.Tipo, '') AS TaxaEntregaTipo, IsNull(e.Nome, '') AS TaxaEntrega, ");
                    stbSQL.Append("e.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                    stbSQL.Append("FROM tVendaBilheteria AS vb (NOLOCK) ");
                    stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON vb.CaixaID=tCaixa.ID ");
                    stbSQL.Append("INNER JOIN tUsuario AS u  (NOLOCK) ON tCaixa.UsuarioID=u.ID ");
                    stbSQL.Append("INNER JOIN tLoja  (NOLOCK) ON tCaixa.LojaID=tLoja.ID ");
                    stbSQL.Append("INNER JOIN tCanal  (NOLOCK) ON tLoja.CanalID=tCanal.ID ");
                    stbSQL.Append("LEFT JOIN tCliente AS c  (NOLOCK) ON vb.ClienteID=c.ID ");
                    stbSQL.Append("LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID ");
                    stbSQL.Append("LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID ");
                    stbSQL.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = tc.EntregaAreaID ");
                    stbSQL.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = tc.PeriodoID ");
                    stbSQL.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");
                    stbSQL.Append("WHERE vb.ID=" + vendasID[i]);


                    bd.Consulta(stbSQL.ToString());

                    if (bd.Consulta().Read())
                    {
                        venda = new EstruturaRetornoVendaValeIngresso();
                        venda.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
                        valorTotal = bd.LerDecimal("ValorTotal");
                        taxaEntregaValor = bd.LerDecimal("TaxaEntregaValor");

                        venda.TaxaEntrega = bd.LerString("TaxaEntrega");
                        venda.TaxaEntregaTipo =
                            bd.LerString("TaxaEntregaTipo") == Entrega.AGENDADA || bd.LerString("TaxaEntregaTipo") == Entrega.NORMAL ?
                            Enumeradores.TaxaEntregaTipo.Entrega : Enumeradores.TaxaEntregaTipo.Retirada;
                        venda.Senha = bd.LerString("Senha");
                        venda.VendaBilheteriaID = bd.LerInt("ID");
                        venda.ClienteID = bd.LerInt("ClienteID");
                        venda.ClienteNome = bd.LerString("ClienteNome");
                        venda.ClienteEmail = bd.LerString("ClienteEmail");
                        venda.ValorTotalEntrega = taxaEntregaValor;
                        venda.ValorTotalValeIngressos = valorTotal - taxaEntregaValor;
                        venda.DataVenda = bd.LerDateTime("DataVenda");
                        venda.DataAberturaCaixa = bd.LerDateTime("DataCaixa");
                        venda.ValorTotalVenda = valorTotal;
                        venda.StatusVenda = Convert.ToChar(bd.LerString("Status"));
                        venda.CanalVenda = bd.LerString("Canal");
                        venda.ClienteEnderecoID = bd.LerInt("ClienteEnderecoID");
                        venda.EntregaNome = bd.LerString("EntregaNome");
                        venda.PeriodoEntrega = bd.LerString("PeriodoEntrega");
                        DateTime DataAux = bd.LerDateTime("DataEntrega");
                        venda.DataEntrega = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                        venda.AreaEntrega = bd.LerString("AreaEntrega");
                        venda.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");


                    }
                    else
                        throw new Exception("VendaBilheteriaID: " + vendasID[i] + " não foi encontrada");

                    bd.FecharConsulta();
                    stbSQL = new StringBuilder();

                    stbSQL.Append("SELECT vi.ID , vit.CodigoTrocaFixo,vit.SaudacaoNominal, vi.CodigoBarra, vi.CodigoTroca, vit.SaudacaoPadrao, vit.ProcedimentoTroca,  vit.ValidadeData, vit.ValidadeDiasImpressao, vit.ID, ");
                    stbSQL.Append("         vit.Valor,vit.ValorTipo, vit.Nome,vi.ValeIngressoTipoID, IsNull(vi.ClienteNome, '') AS ClienteNome, vi.Status , IsNull(vb.NomeCartao, '') AS NomeCartao ");
                    stbSQL.Append("FROM tValeIngressoLog viLog (NOLOCK) ");
                    stbSQL.Append("     INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = viLog.ValeIngressoID ");
                    stbSQL.Append("     INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID ");
                    stbSQL.Append("     INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = viLog.VendaBilheteriaID ");
                    stbSQL.Append("WHERE vi.Status ='" + IngressoLog.VENDER + "' AND vb.ID = " + vendasID[i] + " ");
                    stbSQL.Append("ORDER BY viLog.TimeStamp");

                    bd.Consulta(stbSQL.ToString());

                    while (bd.Consulta().Read())
                    {
                        item = new EstruturaImpressaoVir();
                        item.ValeIngressoID = bd.LerInt("ID");
                        codigoTrocaFixo = bd.LerString("CodigoTrocaFixo");
                        item.CodigoTrocaFixo = codigoTrocaFixo.Length > 0;
                        item.CodigoTroca = codigoTrocaFixo.Length > 0 ? codigoTrocaFixo : bd.LerString("CodigoTroca");
                        validadeData = bd.LerString("ValidadeData");
                        if (validadeData.Trim() != string.Empty)
                            item.ValidadeData = bd.LerDateTime("ValidadeData");
                        else
                        {
                            item.ValidadeEmDiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                            item.ValidadeData = System.DateTime.Now.AddDays(item.ValidadeEmDiasImpressao);
                        }
                        item.SaudacaoNominal = bd.LerString("SaudacaoNominal");
                        item.SaudacaoPadrao = bd.LerString("SaudacaoPadrao");
                        item.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                        item.ValeIngressoTipoID = bd.LerInt("ValeIngressoTipoID");
                        item.Valor = bd.LerDecimal("Valor");
                        item.ValorTipo = Convert.ToChar(bd.LerString("ValorTipo"));
                        item.ValeIngressoNome = bd.LerString("Nome");
                        item.ClientePresenteado = bd.LerString("ClienteNome");

                        switch (Convert.ToChar(bd.LerString("Status")))
                        {
                            case (char)ValeIngresso.enumStatus.Vendido:
                                item.Status = ValeIngresso.enumStatus.Vendido;
                                break;
                            case (char)ValeIngresso.enumStatus.Impresso:
                                item.Status = ValeIngresso.enumStatus.Impresso;
                                break;
                        }
                        item.NomeCartao = bd.LerString("NomeCartao");

                        venda.EstruturaImpressaoVir.Add(item);
                    }
                    bd.FecharConsulta();
                    lstRetorno.Add(venda);
                }
                return lstRetorno;
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
        /// Buscar informações de impressão em lote
        /// </summary>
        /// <returns></returns>
        public DataTable Buscar(int empresaID, int localID, int eventoID, int apresentacaoID, int[] taxasEntrega, int qtdeTotalTaxasEntrega,
            int[] lojasIDs, int qtdeTotalLojas, string filtroInicial, bool imprimirAguardandoTroca, string cepInicial, string cepFinal)
        {

            BD bd = new BD();

            try
            {

                DataTable grid = estruturaGrid();

                DateTime dataVenda;
                DateTime menorApresentacao;
                int DiasTriagem = 0;
                string vbStatus = string.Empty;
                //preparar filtros ********************
                // (empresaID == 0) -> todas
                // (taxasEntregas.Length == 0) -> todas

                string filtro = "";
                if (filtroInicial != "")
                    filtro = "AND tCliente.Nome LIKE '" + filtroInicial + "%'";

                string empresa = "";
                if (empresaID != 0)
                    empresa = "AND tIngresso.EmpresaID=" + empresaID;

                string local = "";
                if (localID != 0)
                    local = "AND tIngresso.LocalID=" + localID;

                string evento = "";
                if (eventoID != 0)
                    evento = "AND tIngresso.EventoID=" + eventoID;

                string apresentacao = "";
                if (apresentacaoID != 0)
                    apresentacao = "AND tIngresso.ApresentacaoID=" + apresentacaoID;

                string entregas = "";
                if (taxasEntrega.Length > 0)
                {
                    if (taxasEntrega.Length != qtdeTotalTaxasEntrega && qtdeTotalTaxasEntrega > 0)
                        entregas = "AND tVendaBilheteria.TaxaEntregaID in (" + Utilitario.ArrayToString(taxasEntrega) + ")";
                    else
                        entregas = "AND tVendaBilheteria.TaxaEntregaID <> 0";
                }
                else
                    entregas = "AND tVendaBilheteria.TaxaEntregaID = 0";

                string lojas = "";
                if (lojasIDs.Length > 0)
                {
                    if (lojasIDs.Length != qtdeTotalLojas && qtdeTotalLojas > 0)
                        lojas = "AND tCaixa.LojaID in (" + Utilitario.ArrayToString(lojasIDs) + ")";
                }

                string statusAntiFraude = " AND tVendaBilheteria.Status <> '" + (char)VendaBilheteria.StatusAntiFraude.Fraude + "' ";
                string statusEmAnalise = " AND tVendaBilheteria.Status <> '" + (char)VendaBilheteria.StatusAntiFraude.EmAnalise + "' ";

                string cep = string.Empty;
                if (cepInicial.Length > 0 && cepFinal.Length > 0)
                    cep = string.Format("AND (tCliente.CEPEntrega >= '{0}' AND tCliente.CEPEntrega <= '{1}') ", cepInicial, cepFinal);
                else if (cepInicial.Length > 0 && cepFinal.Length == 0)
                    cep = string.Format("AND tCliente.CEPEntrega >= '{0}' ", cepInicial);
                else if (cepFinal.Length > 0 && cepInicial.Length == 0)
                    cep = string.Format("AND tCliente.CEPEntrega <= '{0}' ", cepFinal);

                string sql = "SELECT tTaxaEntrega.Nome AS Entrega, tLoja.Nome AS Loja, tIngresso.VendaBilheteriaID, tVendaBilheteria.Senha, " +
                    "tVendaBilheteria.DataVenda, tVendaBilheteria.Status AS StatusVenda, tCliente.Nome AS Cliente, " +
                    "tCliente.EnderecoEntrega+ ' ' + isNull(tCliente.NumeroEntrega,'') + ' ' + isNull(tCliente.ComplementoEntrega,'') AS ClienteEndereco," +
                    "tCliente.CidadeEntrega AS ClienteCidade,tCliente.EstadoEntrega AS ClienteEstado,tCliente.CEPEntrega AS ClienteCEp," +
                    "tEvento.Nome AS Evento, tApresentacao.Horario, Count(tIngresso.ID) AS Qtde, " +
                    "tCanal.TipoVenda, tCanal.ComprovanteQuantidade, IsNull(tCliente.StatusAtual, '') AS ClienteStatus, MIN(tApresentacao.Horario) AS MenorApresentacao, IsNull(tTaxaEntrega.DiasTriagem, 0) AS DiasTriagem " +
                    "FROM tIngresso (NOLOCK) " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID " +
                    "INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID " +
                    "INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID " +
                    "INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID " +
                    "LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tVendaBilheteria.ClienteID " +
                    "LEFT JOIN tTaxaEntrega (NOLOCK) ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID " +
                    "WHERE  " +
                    "   ( " +
                    "tIngresso.Status = '" + Ingresso.VENDIDO + "' " + ((imprimirAguardandoTroca) ? " OR tIngresso.Status = '" + Ingresso.AGUARDANDO_TROCA + "' " : "") +
                    "   ) " +
                    filtro + " " +
                    entregas + " " +
                    empresa + " " +
                    local + " " +
                    evento + " " +
                    apresentacao + " " +
                    lojas + " " +
                    statusAntiFraude +
                    //statusEmAnalise +
                    cep +
                    "GROUP BY tTaxaEntrega.Nome, tLoja.Nome, tIngresso.VendaBilheteriaID, tVendaBilheteria.Senha, tVendaBilheteria.DataVenda, " +
                    "tCliente.Nome, tEvento.Nome, tApresentacao.Horario, tCanal.TipoVenda, tCanal.ComprovanteQuantidade,tVendaBilheteria.Bin, tCliente.StatusAtual, tTaxaEntrega.DiasTriagem, tVendaBilheteria.Status ," +
                    "tCliente.EnderecoEntrega,tCliente.NumeroEntrega,tCliente.ComplementoEntrega," +
                    "tCliente.CidadeEntrega,tCliente.EstadoEntrega,tCliente.CEPEntrega " +
                    "ORDER BY tCliente.Nome ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = grid.NewRow();
                    if (bd.LerString("Entrega") != "")
                        linha[ENTREGA] = bd.LerString("Entrega");
                    linha[LOJA] = bd.LerString("Loja");
                    linha[SENHA] = bd.LerString("Senha");
                    linha[DATA_VENDA] = bd.LerDateTime("DataVenda");

                    if (bd.LerString("Cliente") != string.Empty)
                        linha[CLIENTE] = bd.LerString("Cliente");
                    if (bd.LerString("ClienteEndereco") != string.Empty)
                        linha[ENDERECO_ENTREGA] = bd.LerString("ClienteEndereco");
                    if (bd.LerString("ClienteCidade") != string.Empty)
                        linha[CIDADE_ENTREGA] = bd.LerString("ClienteCidade");
                    if (bd.LerString("ClienteEstado") != string.Empty)
                        linha[ESTADO_ENTREGA] = bd.LerString("ClienteEstado");
                    if (bd.LerString("ClienteCEP") != string.Empty)
                        linha[CEP_ENTREGA] = bd.LerString("ClienteCEP");

                    linha[EVENTO] = bd.LerString("Evento");
                    linha[HORARIO] = bd.LerDateTime("Horario");
                    linha[QTDE] = bd.LerInt("Qtde");
                    linha[VENDA_BILHETERIA_ID] = bd.LerInt("VendaBilheteriaID");
                    linha[TIPO_VENDA] = bd.LerString("TipoVenda");
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                    linha[CLIENTE_STATUS] = bd.LerString("ClienteStatus");
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");


                    DiasTriagem = bd.LerInt("DiasTriagem");
                    dataVenda = bd.LerDateTime("DataVenda");
                    menorApresentacao = bd.LerDateTime("MenorApresentacao");
                    vbStatus = bd.LerString("StatusVenda");

                    if ((vbStatus == VendaBilheteria.AGUARDANDO_APROVACAO || vbStatus == VendaBilheteria.EMANALISE) && DiasTriagem > 0)
                    {
                        if (SoImprimirDataApresentacao)
                        {
                            if (menorApresentacao.Year == DateTime.Now.Year && menorApresentacao.Month == DateTime.Now.Month && menorApresentacao.Day == DateTime.Now.Day)
                            {
                                grid.Rows.Add(linha);
                            }
                        }
                        else
                        {
                            if (dataVenda.AddDays(DiasTriagem) <= DateTime.Now)
                                grid.Rows.Add(linha);
                            else if (Utilitario.DiasUteisPeriodo(DateTime.Now, menorApresentacao) <= 2)
                                grid.Rows.Add(linha);
                            else
                                continue;
                        }
                    }
                    else
                        grid.Rows.Add(linha);
                }
                return grid;

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
        /// Buscar informações de impressão em lote
        /// </summary>
        /// <returns></returns>
        public DataTable Buscar(EstruturaBuscaImpressaoEmLote busca)
        {
            BD bd = new BD();
            try
            {
                DataTable grid = estruturaGrid();
                DateTime dataVenda;
                DateTime menorApresentacao;
                int DiasTriagem = 0;
                bool imprimir = true;
                string vbStatus = string.Empty;

                //preparar filtros ********************

                string inicialCliente = string.Empty;
                if (!string.IsNullOrEmpty(busca.LetraInicial))
                    inicialCliente = " AND c.Nome LIKE '" + busca.LetraInicial + "%'";

                string apresentacao = string.Empty;
                if (busca.IDs.Count > 1)
                    bd.BulkInsert(busca.IDs, "#apresentacoes", false, true);
                else
                    apresentacao = " AND ap.ID = " + busca.IDs.FirstOrDefault();

                string loja = string.Empty;
                if (busca.Lojas.Count > 1)
                    bd.BulkInsert(busca.Lojas, "#lojas", false, true);
                else
                    loja = " AND l.ID = " + busca.Lojas.FirstOrDefault();

                string periodo = busca.PeriodoID > 0 ? //Se buscar periodo vai ignorar as retiradas
                    " AND tc.PeriodoID = " + busca.PeriodoID : string.Empty;

                string data = busca.PesquisarData ? //Se buscar por data também ignora
                    string.Format(
                    " AND ea.Data >= '{0}' AND ea.Data < '{1}' ",
                    busca.DataInicial.ToString("yyyyMMdd"),
                    busca.DataFinal.AddDays(1).ToString("yyyyMMdd")) : string.Empty;

                string area = string.Empty;
                //Se não selecionar nenhuma área, tem o mesmo efeito de selecionar todas, é utilizado porque retirada n tem area
                if (busca.Areas.Count > 1)
                    bd.BulkInsert(busca.Areas, "#areas", false, true);
                else if (busca.Areas.Count == 1)
                    area = " AND tc.EntregaAreaID = " + busca.Areas.FirstOrDefault();

                string somenteAgendadas = busca.SomenteAgendadas ?
                    string.Format(" AND te.Tipo = '{0}'", Entrega.AGENDADA) : string.Empty;

                string taxa = string.Empty;
                if (busca.Taxas.Count > 1)
                    bd.BulkInsert(busca.Taxas, "#taxas", false, true);
                else if (busca.Taxas.Count == 1)
                    taxa = " AND te.ID = " + busca.Taxas.FirstOrDefault();

                string selectQuery = @"SELECT * FROM (
                                SELECT 
                                e.nome as Evento, te.Nome AS Entrega, te.Tipo AS TipoTaxa, l.Nome AS Loja, 
                                vb.ID AS VendaBilheteriaID, vb.Senha, vb.DataVenda, vb.Status, 
                                cn.ComprovanteQuantidade, cn.TipoVenda, Count(i.ID) AS Qtd, 
                                CASE WHEN LEN(c.CNPJ) > 0 THEN c.NomeFantasia ELSE c.Nome COLLATE Latin1_General_CI_AI END AS Cliente, 
                                c.StatusAtual AS ClienteStatus, MIN(ap.Horario) AS MenorApresentacao, 
                                te.DiasTriagem AS EntregaTriagem, tc.DiasTriagem AS ControleTriagem, 
                                c.RG AS ClienteRG, CASE WHEN LEN(c.CNPJ) > 0 THEN c.CNPJ ELSE c.CPF COLLATE Latin1_General_CI_AI END AS ClienteCPF, 
                                c.CEPCliente AS ClienteCEP, c.EnderecoCliente AS ClienteEndereco, c.NumeroCliente AS ClienteNumero, c.ComplementoCliente AS ClienteComplemento, c.BairroCliente AS ClienteBairro, 
                                c.CidadeCliente AS ClienteCidade, c.EstadoCliente AS ClienteEstado, 
                                IsNull(ec.ID, 0) AS ClienteEnderecoID, ec.Nome AS NomeEntrega, ec.RG AS RGEntrega, ec.Endereco AS EnderecoEntrega, ec.Numero AS NumeroEntrega, ec.Complemento AS ComplementoEntrega, 
                                ec.Bairro AS BairroEntrega, ec.Cidade AS CidadeEntrega, ec.Estado AS EstadoEntrega, ec.CEP AS CEPEntrega, 
                                te.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega, vb.CodigoRastreio AS CodigoRastreioCorreio ,
                                ROW_NUMBER() OVER(PARTITION BY vb.ID ORDER BY VB.ID DESC) RowNumber
                                FROM tVendaBilheteria vb (NOLOCK) 
                                INNER JOIN tIngresso i (NOLOCK) ON vb.ID = i.VendaBilheteriaID 
                                INNER JOIN tApresentacao ap (NOLOCK) ON i.ApresentacaoID = ap.ID 
                                INNER JOIN tEvento e (NOLOCK) ON ap.EventoID = e.ID 
                                INNER JOIN tCaixa cx (NOLOCK) ON cx.ID = vb.CaixaID 
                                INNER JOIN tLoja l (NOLOCK) ON l.ID = cx.LojaID 
                                INNER JOIN tCanal cn (NOLOCK) ON cn.ID = l.CanalID 
                                INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID 
                                LEFT JOIN tClienteEndereco ec (NOLOCK) ON ec.ID = vb.ClienteEnderecoID	
                                INNER JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID 
                                INNER JOIN tEntrega te (NOLOCK) ON te.ID = tc.EntregaID 
                                LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = tc.EntregaAreaID 
                                LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = tc.PeriodoID 
                                LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ";

                if (string.IsNullOrEmpty(apresentacao))
                    selectQuery += " INNER JOIN #apresentacoes ON #apresentacoes.ID = ap.ID ";
                if (string.IsNullOrEmpty(loja))
                    selectQuery += " INNER JOIN #lojas ON #lojas.ID = l.ID ";
                if (string.IsNullOrEmpty(area) && busca.Areas.Count > 0)
                    selectQuery += " INNER JOIN #areas ON #areas.ID = tc.EntregaAreaID ";
                if (string.IsNullOrEmpty(taxa) && busca.Taxas.Count > 0)
                    selectQuery += " INNER JOIN #taxas ON #taxas.ID = te.ID ";

                ConfigGerenciadorParalela Config = new ConfigGerenciadorParalela();

                //stb.AppendFormat("  WHERE i.Status = '{0}' AND PagamentoProcessado = 'T' ", Ingresso.VENDIDO);

                selectQuery += string.Format("  WHERE i.Status = '{0}' AND e.ID NOT IN ({1}) AND PagamentoProcessado = 'T' AND c.StatusAtual <> 'B' ", Ingresso.VENDIDO, Config.getEventosNaoImpressos());

                if (!string.IsNullOrEmpty(inicialCliente))
                    selectQuery += inicialCliente;

                if (!string.IsNullOrEmpty(apresentacao))
                    selectQuery += apresentacao;

                if (!string.IsNullOrEmpty(loja))
                    selectQuery += loja;

                if (!string.IsNullOrEmpty(periodo))
                    selectQuery += periodo;

                if (!string.IsNullOrEmpty(data))
                    selectQuery += data;

                if (!string.IsNullOrEmpty(area))
                    selectQuery += area;

                if (!string.IsNullOrEmpty(somenteAgendadas))
                    selectQuery += somenteAgendadas;

                if (!string.IsNullOrEmpty(taxa))
                    selectQuery += taxa;

                string tipoImpressao = busca.TipoImpressao == EstruturaTipoImpressao.TipoImpressao.Laser ? "L" : "T";
                selectQuery += string.Format(" And e.TipoImpressao = '{0}'", tipoImpressao);

                string group = @" GROUP BY 
                e.Nome, te.Nome, te.Tipo, l.Nome, 
                vb.ID, vb.Senha, vb.DataVenda, vb.Status, c.Nome, 
                cn.ComprovanteQuantidade, cn.TipoVenda, 
                c.StatusAtual, 
                te.DiasTriagem, tc.DiasTriagem, 
                ec.ID, ec.Nome, ec.RG, ec.Endereco, ec.Numero, ec.Complemento, 
                ec.Bairro, ec.Cidade, ec.Estado, ec.CEP, c.CNPJ, 
                te.Nome , ep.Nome,  ea.Data , ear.Nome,
                c.NomeFantasia, c.RG, c.CNPJ, c.CPF, c.CEPCliente, c.EnderecoCliente, c.NumeroCliente, 
                c.ComplementoCliente, c.BairroCliente, c.CidadeCliente, c.EstadoCliente, vb.CodigoRastreio";

                selectQuery += group;

                string order = string.Empty;

                if (busca.OrdernacaoAlfabetica)
                    order = " ORDER BY Cliente ";

                else if (busca.OrdernacaoData)
                    order = " ORDER BY DataVenda ";
                else if (busca.OrdenacaoEventoProximo)
                    order = " ORDER By MenorApresentacao";

                selectQuery += ") tabela where RowNumber = 1  " + order;

                bd.Consulta(selectQuery);

                while (bd.Consulta().Read())
                {
                    DataRow linha = grid.NewRow();
                    imprimir = true;

                    if (bd.LerString(CLIENTE_STATUS) == ((char)Cliente.StatusCliente.Bloqueado).ToString())
                    {
                        imprimir = false;
                        continue;
                    }

                    DiasTriagem = bd.LerInt("ControleTriagem");
                    DiasTriagem = DiasTriagem > 0 ? DiasTriagem : bd.LerInt("EntregaTriagem");

                    dataVenda = bd.LerDateTime(DATA_VENDA);
                    menorApresentacao = bd.LerDateTime(MENOR_APRESENTACAO);
                    vbStatus = bd.LerString(STATUS);

                    switch (vbStatus)
                    {
                        case VendaBilheteria.AGUARDANDO_APROVACAO:
                        case VendaBilheteria.EMANALISE:
                            if (SoImprimirDataApresentacao)
                                imprimir &= menorApresentacao.Date == DateTime.Now.Date;
                            else
                            {
                                if (dataVenda.AddDays(DiasTriagem).Date <= DateTime.Now.Date)
                                    imprimir &= true;
                                else if (Utilitario.DiasUteisPeriodo(DateTime.Now, menorApresentacao) <= 2)
                                    imprimir &= true;
                                else
                                    imprimir = false;
                            }
                            break;
                        default:
                            imprimir &= true;
                            break;
                    }

                    if (!imprimir)
                        continue;

                    bool buscarEntrega = bd.LerInt("ClienteEnderecoID") > 0;
                    linha[ENTREGA] = bd.LerString(ENTREGA);
                    linha[TIPO_TAXA] =
                        bd.LerString(TIPO_TAXA) == Entrega.AGENDADA || bd.LerString(TIPO_TAXA) == Entrega.NORMAL ?
                        ((int)Enumeradores.TaxaEntregaTipo.Entrega) : ((int)Enumeradores.TaxaEntregaTipo.Entrega);
                    linha[LOJA] = bd.LerString(LOJA);
                    linha[SENHA] = bd.LerString(SENHA);
                    linha[DATA_VENDA] = bd.LerDateTime(DATA_VENDA);
                    linha[CLIENTE] = bd.LerString(CLIENTE);
                    linha[NOME_ENTREGA] = buscarEntrega ? bd.LerString(NOME_ENTREGA) : bd.LerString("Cliente");
                    linha[RG_ENTREGA] = buscarEntrega ? bd.LerString(RG_ENTREGA) : bd.LerString("ClienteRG");
                    linha[ENDERECO_ENTREGA] = buscarEntrega ? bd.LerString(ENDERECO_ENTREGA) : bd.LerString("ClienteEndereco");
                    linha[NUMERO_ENTREGA] = buscarEntrega ? bd.LerString(NUMERO_ENTREGA) : bd.LerString("ClienteNumero");
                    linha[COMPLEMENTO_ENTREGA] = buscarEntrega ? bd.LerString(COMPLEMENTO_ENTREGA) : bd.LerString("ClienteComplemento");
                    linha[BAIRRO_ENTREGA] = buscarEntrega ? bd.LerString(BAIRRO_ENTREGA) : bd.LerString("ClienteBairro");
                    linha[CIDADE_ENTREGA] = buscarEntrega ? bd.LerString(CIDADE_ENTREGA) : bd.LerString("ClienteCidade");
                    linha[ESTADO_ENTREGA] = buscarEntrega ? bd.LerString(ESTADO_ENTREGA) : bd.LerString("ClienteEstado");
                    linha[CEP_ENTREGA] = buscarEntrega ? bd.LerString(CEP_ENTREGA) : bd.LerString("ClienteCEP");
                    linha[QTDE] = bd.LerInt(QTDE);
                    linha[VENDA_BILHETERIA_ID] = bd.LerInt(VENDA_BILHETERIA_ID);
                    linha[TIPO_VENDA] = bd.LerString(TIPO_VENDA);
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt(COMPROVANTE_QUANTIDADE);
                    linha[CLIENTE_STATUS] = bd.LerString(CLIENTE_STATUS);
                    linha[STATUS] = bd.LerString(STATUS);
                    linha[ENTREGA_NOME] = bd.LerString(ENTREGA_NOME);
                    linha[PERIODO_ENTREGA] = bd.LerString(PERIODO_ENTREGA);
                    DateTime DataAux = bd.LerDateTime(DATA_ENTREGA);
                    linha[DATA_ENTREGA] = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "";
                    linha[AREA_ENTREGA] = bd.LerString(AREA_ENTREGA);
                    linha[COMPROVANTE_QUANTIDADE] = bd.LerInt("ComprovanteQuantidade");
                    linha[IMPRIMIR] = imprimir;
                    linha[CODIGO_RASTREIO_CORREIO] = bd.LerString("CodigoRastreioCorreio");
                    linha[EVENTO] = bd.LerString(EVENTO);
                    linha[HORARIO] = bd.LerDateTime("MenorApresentacao");

                    grid.Rows.Add(linha);
                }



                return grid;

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


        public BindingList<EstruturaRetornoVendaValeIngresso> BuscarValeIngresso(int empresaID, bool todasEmpresas, int valeIngressoTipoID,
                    bool todosVirs, int[] taxasEntrega, int qtdeTotalTaxasEntrega, int[] lojasIDs, int qtdeTotalLojas, string filtroInicial,
                    string cepInicial, string cepFinal)
        {
            BD bd = new BD();
            StringBuilder stbSQL = new StringBuilder();
            try
            {
                BindingList<EstruturaRetornoVendaValeIngresso> retornoVir = new BindingList<EstruturaRetornoVendaValeIngresso>();
                EstruturaRetornoVendaValeIngresso item;

                stbSQL.Append("SELECT IsNull(te.Nome, '') AS Entrega, l.Nome AS Loja, vi.VendaBilheteriaID, vb.Senha,  ");
                stbSQL.Append("vb.DataVenda, IsNull(c.Nome, '') AS ClienteNome, tCanal.TipoVenda, COUNT(vb.ID) AS Quantidade, tCanal.ComprovanteQuantidade ");
                stbSQL.Append("FROM tValeIngresso vi (NOLOCK) ");
                stbSQL.Append("INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vi.ValeIngressoTipoID = vit.ID ");
                stbSQL.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = vi.VendaBilheteriaID ");
                stbSQL.Append("INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = vb.CaixaID ");
                stbSQL.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = tCaixa.LojaID ");
                stbSQL.Append("INNER JOIN tCanal (NOLOCK) ON tCanal.ID = l.CanalID ");
                stbSQL.Append("LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                stbSQL.Append("LEFT JOIN tTaxaEntrega te (NOLOCK) ON te.ID = vb.TaxaEntregaID ");
                stbSQL.Append("WHERE vi.Status = '" + (char)ValeIngresso.enumStatus.Vendido + "' ");

                if (filtroInicial.Length > 0)
                    stbSQL.Append("AND c.Nome LIKE '" + filtroInicial + "%' ");

                if (valeIngressoTipoID > 0 && !todosVirs && !todasEmpresas)
                    stbSQL.Append("AND vit.ID = " + valeIngressoTipoID + " ");

                if (empresaID != 0 && !todasEmpresas)
                    stbSQL.Append("AND vit.EmpresaID=" + empresaID + " ");

                if (taxasEntrega.Length > 0)
                {
                    if (taxasEntrega.Length != qtdeTotalTaxasEntrega && qtdeTotalTaxasEntrega > 0)
                        stbSQL.Append("AND vb.TaxaEntregaID in (" + Utilitario.ArrayToString(taxasEntrega) + ") ");
                    else
                        stbSQL.Append("AND vb.TaxaEntregaID <> 0");
                }
                else
                    stbSQL.Append("AND vb.TaxaEntregaID = 0");

                if (lojasIDs.Length > 0)
                    if (lojasIDs.Length != qtdeTotalLojas && qtdeTotalLojas > 0)
                        stbSQL.Append("AND tCaixa.LojaID IN (" + Utilitario.ArrayToString(lojasIDs) + ") ");


                if (cepInicial.Length > 0 && cepFinal.Length > 0)
                    stbSQL.Append(string.Format("AND (c.CEPEntrega >= '{0}' AND c.CEPEntrega <= '{1}') ", cepInicial, cepFinal));
                else if (cepInicial.Length > 0 && cepFinal.Length == 0)
                    stbSQL.Append(string.Format("AND c.CEPEntrega >= '{0}' ", cepInicial));
                else if (cepFinal.Length > 0 && cepInicial.Length == 0)
                    stbSQL.Append(string.Format("AND c.CEPEntrega <= '{0}' ", cepFinal));

                stbSQL.Append(" GROUP BY te.Nome, l.Nome, vi.VendaBilheteriaID, vb.Senha, vb.DataVenda, c.Nome, tCanal.TipoVenda, tCanal.ComprovanteQuantidade ");


                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    item = new EstruturaRetornoVendaValeIngresso();
                    item.TaxaEntrega = bd.LerString("Entrega");
                    item.Loja = bd.LerString("Loja");
                    item.ID = bd.LerInt("VendaBilheteriaID");
                    item.Senha = bd.LerString("Senha");
                    item.DataVenda = bd.LerDateTime("DataVenda");
                    item.ClienteNome = bd.LerString("ClienteNome");
                    item.TipoVenda = bd.LerString("TipoVenda");
                    item.Quantidade = bd.LerInt("Quantidade");
                    item.ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade");
                    retornoVir.Add(item);
                }


                return retornoVir;
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

        public BindingList<EstruturaRetornoVendaValeIngresso> BuscarValeIngresso(EstruturaBuscaImpressaoEmLote busca)
        {
            BD bd = new BD();
            try
            {
                BindingList<EstruturaRetornoVendaValeIngresso> retornoVir = new BindingList<EstruturaRetornoVendaValeIngresso>();
                EstruturaRetornoVendaValeIngresso item;

                string inicialCliente = string.Empty;
                if (!string.IsNullOrEmpty(busca.LetraInicial))
                    inicialCliente = " AND c.Nome LIKE '" + busca.LetraInicial + "%'";

                string vir = string.Empty;
                if (busca.IDs.Count > 1)
                    bd.BulkInsert(busca.IDs, "#virs", false, true);
                else
                    vir = " AND vit.ID = " + busca.IDs.FirstOrDefault();

                string loja = string.Empty;
                if (busca.Lojas.Count > 1)
                    bd.BulkInsert(busca.Lojas, "#lojas", false, true);
                else
                    loja = " AND l.ID = " + busca.Lojas.FirstOrDefault();

                string periodo = busca.PeriodoID > 0 ? " AND tc.PeriodoID = " + busca.PeriodoID : string.Empty;

                string data = busca.PesquisarData ?
                    string.Format(
                    " AND ea.Data >= '{0}' AND ea.Data < '{1}'",
                    busca.DataInicial.ToString("yyyyMMdd"),
                    busca.DataFinal.AddDays(1).ToString("yyyyMMdd")) : string.Empty;

                string area = string.Empty;
                if (busca.Areas.Count > 1)
                    bd.BulkInsert(busca.Areas, "#areas", false, true);
                else if (busca.Areas.Count == 1)
                    area = " AND tc.EntregaAreaID = " + busca.Areas.FirstOrDefault();

                string somenteAgendadas = busca.SomenteAgendadas ?
                    string.Format(" AND te.Tipo = '{0}'", Entrega.AGENDADA) : string.Empty;

                string taxa = string.Empty;
                if (busca.Taxas.Count > 1)
                    bd.BulkInsert(busca.Taxas, "#taxas", false, true);
                else if (busca.Taxas.Count == 1)
                    taxa = " AND te.ID = " + busca.Taxas.FirstOrDefault();

                StringBuilder stb = new StringBuilder();

                stb.Append("SELECT ");
                stb.Append("    te.Nome AS Entrega, l.Nome AS Loja, ");
                stb.Append("    vb.ID AS VendaBilheteriaID, vb.Senha, vb.DataVenda, ");
                stb.Append("    cn.ComprovanteQuantidade, cn.TipoVenda, Count(vi.ID) AS Quantidade, ");
                stb.Append("    CASE WHEN LEN(c.CNPJ) > 0 THEN c.NomeFantasia ELSE c.Nome COLLATE Latin1_General_CI_AI END AS Cliente, c.StatusAtual AS ClienteStatus, ");
                stb.Append("    c.RG AS ClienteRG, CASE WHEN LEN(c.CNPJ) > 0 THEN c.CNPJ ELSE c.CPF COLLATE Latin1_General_CI_AI END AS ClienteCPF, c.CEPCliente AS ClienteCEP, ");
                stb.Append("    c.EnderecoCliente AS ClienteEndereco, c.NumeroCliente AS ClienteNumero, c.ComplementoCliente AS ClienteComplemento, c.BairroCliente AS ClienteBairro, ");
                stb.Append("    c.CidadeCliente AS ClienteCidade, c.EstadoCliente AS ClienteEstado, ");
                stb.Append("    IsNull(ec.ID, 0) AS ClienteEnderecoID, ec.Nome AS NomeEntrega, ec.RG AS RGEntrega, ec.Endereco AS EnderecoEntrega, ec.Numero AS NumeroEntrega, ec.Complemento AS ComplementoEntrega, ");
                stb.Append("    ec.Bairro AS BairroEntrega, ec.Cidade AS CidadeEntrega, ec.Estado AS EstadoEntrega, ec.CEP AS CEPEntrega, ");
                stb.Append("    te.Nome AS EntregaNome, ep.Nome AS PeriodoEntrega, ea.Data AS DataEntrega, ear.Nome AS AreaEntrega ");
                stb.Append("FROM tVendaBilheteria vb (NOLOCK) ");
                stb.Append("INNER JOIN  tValeIngresso vi (NOLOCK) ON vb.ID = vi.VendaBilheteriaID  ");
                stb.Append("INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vi.ValeIngressoTipoID = vit.ID ");
                stb.Append("INNER JOIN tCaixa cx (NOLOCK) ON cx.ID = vb.CaixaID ");
                stb.Append("INNER JOIN tLoja l (NOLOCK) ON l.ID = cx.LojaID ");
                stb.Append("INNER JOIN tCanal cn (NOLOCK) ON cn.ID = l.CanalID ");
                stb.Append("INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID ");
                stb.Append("LEFT JOIN tClienteEndereco ec (NOLOCK) ON ec.ID = vb.ClienteEnderecoID	");
                stb.Append("INNER JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID ");
                stb.Append("INNER JOIN tEntrega te (NOLOCK) ON te.ID = tc.EntregaID ");
                stb.Append("LEFT JOIN tEntregaArea ear (NOLOCK) ON ear.ID = tc.EntregaAreaID ");
                stb.Append("LEFT JOIN tEntregaPeriodo ep (NOLOCK) ON ep.ID = tc.PeriodoID ");
                stb.Append("LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID ");


                if (string.IsNullOrEmpty(vir))
                    stb.Append(" INNER JOIN #virs ON #virs.ID = vit.ID ");
                if (string.IsNullOrEmpty(loja))
                    stb.Append(" NER JOIN #lojas ON #lojas.ID = l.ID ");
                if (string.IsNullOrEmpty(area) && busca.Areas.Count > 0)
                    stb.Append(" INNER JOIN #areas ON #areas.ID = tc.EntregaAreaID ");
                if (string.IsNullOrEmpty(taxa) && busca.Taxas.Count > 0)
                    stb.Append(" INNER JOIN #taxas ON #taxas.ID = te.ID ");

                stb.Append("WHERE vb.Status <> '" + VendaBilheteria.FRAUDE + "' AND vi.Status = '" + (char)ValeIngresso.enumStatus.Vendido + "' ");

                if (!string.IsNullOrEmpty(inicialCliente))
                    stb.Append(inicialCliente);
                if (!string.IsNullOrEmpty(vir))
                    stb.Append(vir);
                if (!string.IsNullOrEmpty(loja))
                    stb.Append(loja);
                if (!string.IsNullOrEmpty(periodo))
                    stb.Append(periodo);
                if (!string.IsNullOrEmpty(data))
                    stb.Append(data);
                if (!string.IsNullOrEmpty(area))
                    stb.Append(area);
                if (!string.IsNullOrEmpty(somenteAgendadas))
                    stb.Append(somenteAgendadas);
                if (!string.IsNullOrEmpty(taxa))
                    stb.Append(taxa);

                stb.Append(" GROUP BY ");
                stb.Append("    te.Nome, l.Nome, ");
                stb.Append("    vb.ID, vb.Senha, vb.DataVenda, c.Nome, c.StatusAtual, ");
                stb.Append("    cn.ComprovanteQuantidade, cn.TipoVenda, ");
                stb.Append("    te.Nome , ep.Nome , ea.Data , ear.Nome , ");
                stb.Append("    c.NomeFantasia, c.StatusAtual, ");
                stb.Append("    ec.ID, ec.Nome, ec.RG, ec.Endereco, ec.Numero, ec.Complemento, ");
                stb.Append("    ec.Bairro, ec.Cidade, ec.Estado, ec.CEP, c.CNPJ, ");
                stb.Append("    te.Nome , ep.Nome,  ea.Data , ear.Nome, ");
                stb.Append("    c.NomeFantasia, c.RG, c.CNPJ, c.CPF, c.CEPCliente, c.EnderecoCliente, c.NumeroCliente, c.ComplementoCliente, c.BairroCliente, c.CidadeCliente, c.EstadoCliente ");


                bd.Consulta(stb);

                while (bd.Consulta().Read())
                {
                    bool buscarEntrega = bd.LerInt("ClienteEnderecoID") > 0;
                    DateTime DataAux = bd.LerDateTime(DATA_ENTREGA);

                    item = new EstruturaRetornoVendaValeIngresso()
                    {
                        TaxaEntrega = bd.LerString("Entrega"),
                        Loja = bd.LerString("Loja"),
                        ID = bd.LerInt("VendaBilheteriaID"),
                        Senha = bd.LerString("Senha"),
                        DataVenda = bd.LerDateTime("DataVenda"),
                        ClienteNome = bd.LerString("Cliente"),
                        TipoVenda = bd.LerString("TipoVenda"),
                        Quantidade = bd.LerInt("Quantidade"),
                        ComprovanteQuantidade = bd.LerInt("ComprovanteQuantidade"),
                        Imprimir = bd.LerString(CLIENTE_STATUS) != ((char)Cliente.StatusClienteChar.Bloqueado).ToString(),
                        Impresso = false,
                        ClienteStatus = bd.LerString(CLIENTE_STATUS),
                        EntregaNome = bd.LerString(ENTREGA_NOME),
                        PeriodoEntrega = bd.LerString(PERIODO_ENTREGA),
                        DataEntrega = DataAux > DateTime.MinValue ? DataAux.ToString("dd/MM/yyyy") : "",
                        AreaEntrega = bd.LerString(AREA_ENTREGA),
                        EstruturaEntregaVIR = new EstruturaRetornoVendaValeIngressoEntrega()
                                    {
                                        Nome = buscarEntrega ? bd.LerString(NOME_ENTREGA) : bd.LerString("Cliente"),
                                        RG = buscarEntrega ? bd.LerString(RG_ENTREGA) : bd.LerString("ClienteRG"),
                                        Endereco = buscarEntrega ? bd.LerString(ENDERECO_ENTREGA) : bd.LerString("ClienteEndereco"),
                                        Numero = buscarEntrega ? bd.LerString(NUMERO_ENTREGA) : bd.LerString("ClienteNumero"),
                                        Complemento = buscarEntrega ? bd.LerString(COMPLEMENTO_ENTREGA) : bd.LerString("ClienteComplemento"),
                                        Bairro = buscarEntrega ? bd.LerString(BAIRRO_ENTREGA) : bd.LerString("ClienteBairro"),
                                        Cidade = buscarEntrega ? bd.LerString(CIDADE_ENTREGA) : bd.LerString("ClienteCidade"),
                                        Estado = buscarEntrega ? bd.LerString(ESTADO_ENTREGA) : bd.LerString("ClienteEstado"),
                                    },
                    };
                    retornoVir.Add(item);
                }

                return retornoVir;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    [Serializable]
    public class ImpressoEmLoteGerenciadorException : Exception
    {

        public ImpressoEmLoteGerenciadorException() : base() { }

        public ImpressoEmLoteGerenciadorException(string msg) : base(msg) { }

        public ImpressoEmLoteGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}

