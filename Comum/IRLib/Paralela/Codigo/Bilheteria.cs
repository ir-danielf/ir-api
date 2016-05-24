using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.Codigo.ModuloLogistica;
using IRLib.Paralela.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using System.Web.Services.Protocols;
using IRLib.Mondial;

namespace IRLib.Paralela
{
    /// <summary>
    /// Classe Bilheteria. Responsavel por todo o controle de venda de ingresso, pacotes e produtos
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public partial class BilheteriaParalela : MarshalByRefObject
    {
        ArrayList ids = new ArrayList();
        List<int> ValeIngressoReservados = new List<int>();

        #region Constantes
        //colunas da GRID
        public const string TABELA_GRID = "Grid";
        public const string TABELA_RESERVA = "Reserva";
        public const string TABELA_ESTRUTURA_IMPRESSAO = "EstruturaImpressao";
        public const string TABELA_MENSAGEM = "Mensagem";
        public const string TABELA_RESERVA_LUGAR_MARCADO = "ReservaLugarMarcado";
        public const string TABELA_EVENTO = "Evento";
        public const string TABELA_APRESENTACAO = "Apresentacao";
        public const string TABELA_APRESENTACAO_SETOR = "ApresentacaoSetor";
        public const string TABELA_LOCAL = "Local";
        public const string TABELA_EVENTO_TAXA_ENTREGA = "TaxaEntrega";
        public const string TABELA_TAXA_ENTREGA_PADRAO = "TaxaEntregaPadrao";
        public const string TABELA_FORMA_PAGAMENTO = "FormaPagamento";
        public const string TABELA_EVENTO_FORMA_PAGAMENTO = "FormaPagamentoEvento";
        public const string TABELA_EVENTO_REGIAO = "Regiao";
        public const string TABELA_OBRIGATORIEDADE = "Obrigatoriedade";
        public const string TABELA_VALE_INGRESSO_TIPO = "ValeIngressoTipo";
        //public const string TABELA_COTA = "Cota";
        public const string TABELA_COTA_ITEM = "CotaItem";
        public const string TABELA_COTA_ITEM_FORMA_PAGAMENTO = "CotaItemFormaPagamento";
        public const string TABELA_CODIGO_PROMO = "CodigoPromo";

        public const string EMPRESAID = "EmpresaID";
        public const string PACOTEID = "PacoteID";
        public const string INGRESSOID = "IngressoID";
        public const string RESERVAID = "ReservaID";
        public const string APRESENTACAOSETORID = "ApresentacaoSetorID";
        public const string PRECOID = "PrecoID";
        public const string CORTESIAID = "CortesiaID";
        public const string BLOQUEIOID = "BloqueioID";
        public const string APRESENTACAOID = "ApresentacaoID";
        public const string ASSINATURA_CLIENTE_ID = "AssinaturaClienteID";
        public const string LOCALID = "LocalID";
        public const string EVENTO_PACOTE = "Evento/Pacote/Vale Ingresso";
        public const string EVENTO = "Evento";
        public const string PACOTE = "Pacote";
        public const string HORARIO = "Horário";
        public const string SETOR_PRODUTO = "Setor/Produto";
        public const string LUGAR_MARCADO = "LugarMarcado";
        public const string CODIGO = "Cod";
        public const string PRECO = "Preço";
        public const string CORTESIA = "Cortesia";
        public const string VALOR = "Valor";
        public const string CONV = "Conv";
        public const string CONV_MIN = "TaxaMin";
        public const string CONV_MAX = "TaxaMax";
        public const string VALOR_CONV = "Valor Conv";
        public const string TIPO = "Tipo";
        public const string TIPO_LUGAR = "TipoLugar";
        public const string DESC_MSG = "DescMsg";
        public const string LUGARID = "LugarID";
        public const string QTDE_RESERVAR = "QtdeReservar";
        public const string QTDE_RESERVADO = "QtdeReservado";
        public const string RESERVADO = "Reservado";
        public const string STATUS = "Status";
        public const string COD_MSG = "Cod";
        public const string ALERTA_MSG = "Alerta";
        public const string OBRIGA_CADASTRO_CLIENTE = "ObrigaCadastroCliente";
        public const string TAXA_COMISSAO = "TaxaComissao";
        public const string COMISSAO_MAXIMA = "ComissaoMaxima";
        public const string COMISSAO_MINIMA = "ComissaoMinima";
        public const string COMISSAO_VALOR = "ComissaoValor";
        public const string COMISSAO_VALOR_TOTAL = "ComissaoValorTotal";
        public const string EVENTOID = "EventoID";
        public const string TAXAENTREGAID = "TaxaEntregaID";
        public const string CODIGO_SEQUENCIAL = "CodigoSeguencial";
        public const string PRECO_INICIA_COM = "PrecoIniciaCom";
        public const string COTA_ID = "CotaID";
        public const string COTA_ITEM_ID_APS = "CotaItemIDAPS";
        public const string QUANTIDADE_POR_CODIGO = "QuantidadePorCodigo";
        public const string NOMINAL = "Nominal";
        public const string COTA_ITEM_ID = "CotaItemID";
        public const string COTA = "Cota";
        public const string COTA_ITEM = "CotaItem";
        public const string SERIE = "Serie";
        public const string ITEM_PROMOCIONAL = "ItemPromocional";
        public const string QUANTIDADE_POR_PROMOCIONAL = "QuantidadePorPromocional";
        public const string TIPO_CODIGO_BARRA = "TipoCodigoBarra";
        public const string QUANTIDADE_COTA_APS = "QuantidadeCotaAPS";
        public const string QUANTIDADEPORCLIENTE_COTA_APS = "QuantidadePorClienteCotaAPS";

        public const string QUANTIDADE_COTA = "QuantidadeCota";
        public const string QUANTIDADEPORCLIENTE_COTA = "QuantidadePorClienteCota";
        public const string QUANTIDADE = "Quantidade";
        public const string QUANTIDADEPORCLIENTE = "QuantidadePorCliente";
        public const string QUANTIDADE_APRESENTACAO = "QuantidadeApresentacao";
        public const string QUANTIDADEPORCLIENTE_APRESENTACAO = "QuantidadePorClienteApresentacao";
        public const string QUANTIDADE_APRESENTACAO_SETOR = "QuantidadeApresentacaoSetor";
        public const string QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR = "QuantidadePorClienteApresentacaoSetor";
        public const string CLIENTE_ID = "ClienteID";
        public const string DONO_ID = "DonoID";

        public const string OBRIGATORIEDADE_ID = "ObrigatoriedadeID";
        public const string CLIENTE = "Cliente";
        public const string CPF = "CPF";
        public const string PERMITIR_CANCELAMENTO_AVULSO = "PermitirCancelamentoAvulso";
        public const string VALIDA_BIN = "ValidaBin";
        public const string VALIDA_BIN_APS = "ValidaBinAPS";
        public const string CODIGO_PROMO = "CodigoPromo";
        public const string CODIGO_BARRA = "CodigoBarra";
        public const string STATUS_CODIGO_PROMO = "StatusCodigoPromo";
        public const string PARCEIRO_ID = "ParceiroID";
        public const string PARCEIRO_ID_APS = "ParceiroIDAPS";
        public const char TIPO_PACOTE = 'P'; //tipos de reserva
        public const char TIPO_INGRESSO = 'I';
        public const string SPECIALEVENT = "SpecialEvent";
        public const string GERENCIAMENTO_INGRESSOS = "GerenciamentoIngressos";
        public const string GERENCIAMENTO_INGRESSOS_ID = "GerenciamentoIngressosID";
        public const string TERMO = "Termo";
        public const string CPF_RESPONSAVEL = "CPFResponsavel";
        public const string SERIEID = "SerieID";

        public const string MSG_INGRESSO_PRECO = "A quota de ingressos nesse preço já atingiu sua quantidade máxima.";
        public const string MSG_INGRESSO_CANAL = "A quota de ingressos no seu canal já atingiu sua quantidade máxima.";
        public const string MSG_INGRESSO_CLIENTE = "A quota de ingressos por cliente já atingiu sua quantidade máxima.";

        public const string MSG_PACOTE_DISPONIVEL = "A quota de pacotes já atingiu sua quantidade máxima.";
        public const string MSG_PACOTE_CANAL = "A quota de pacotes no seu canal já atingiu sua quantidade máxima.";
        private const int ADM_OSESP_CANAL_ID = 207;
        private const int ADM_OSESP_LOJA_ID = 343;
        public const int INTERNET_EMPRESA_ID = 1;
        public const string INTERNET_MOTIVO_REIMPRESSAO = "Reimpressão Internet";


        private const string GRUPO = "GRUPO";
        private const string CLASSIFICACAO = "Classificacao";

        public const int DINHEIRO = 1;
        public const int VIR = 137;
        #endregion

        //#VIR:
        EstruturaValeIngresso valeIngresso = new EstruturaValeIngresso();

        #region Enumeradores
        public enum CodMensagem
        {
            Nulo,
            Disponivel,
            PorPreco,
            PorCanal,
            PorCliente,
            PorCota
        }

        public enum CodMensagemReserva
        {
            Nulo = -1,
            IngressosEsgotados = 0,
            Sucesso = 1,
            ClienteInexistente = 2,
            PrecoNaoExiste = 3,
            PrecoIndisponivel = 4,
            EventoInexistente = 5,
            ApresentacaoIndisponivel = 6,
            ValidadePreco = 7,
            PorPreco = 8,
            PorCanal = 9,
            PorCliente = 10,
            PacoteNaoExiste = 11,
            QuotaPacotesEsgotada = 12,
            IngressosPacoteEsgotados = 13,
            PacotesEsgotados = 14,
            CotaEsgotada = 15
        }

        public enum CodMensagemVenda
        {
            ErroIndefinido = -1,
            FalhaVenda = 0,
            Sucesso = 1,
            ClienteInexistente = 2,
            ReservaInexistente = 3,
            TaxaEntregaInexistente = 4,
            FormaPagamentoNaoCadastrada = 5,
            ReservasCanceladas = 6,
            ErroSeguro = 7,
        }

        public enum CodMensagemCarregarInternet
        {
            Nulo = -1,
            Sucesso = 1,
            CanalInexistente = 2,
        }

        public enum AlertaMensagem
        {
            Nulo,
            Exclamacao, //exclamacao, informa a msg no retorno
            Alerta //apenas indica... e informa msg quando for reservar novamente,
        }

        #endregion

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }

        private string[] MontaRetornoVendaInternet(string msgCodigo, string senha)
        {
            return new string[] { msgCodigo, senha };
        }

        /// <summary>
        /// Captura o ambiente de reserva
        /// </summary>
        /// <param name="senha">Senha de pré-reserva</param>
        /// <param name="canalID">Canal que está realizando a troca</param>
        /// <returns></returns>
        public DataSet PesquisaPreReserva(string senha, int canalID)
        {
            BD bd = new BD();
            DataSet retorno = estruturaReservas(1);

            // Adiciona a tabela de cliente
            DataTable tabelaCliente = new DataTable("Cliente");
            tabelaCliente.Columns.Add(new DataColumn("ID", typeof(int)));
            retorno.Tables.Add(tabelaCliente);

            // Adiciona a tabela de info do evento
            DataTable tabelaEventoInfo = new DataTable("EventoInfo");
            tabelaEventoInfo.Columns.Add("ID", typeof(int));
            tabelaEventoInfo.Columns.Add("Nome", typeof(string));
            tabelaEventoInfo.Columns.Add("LocalID", typeof(int));
            tabelaEventoInfo.Columns.Add("Atencao", typeof(string));
            tabelaEventoInfo.Columns.Add("ObrigaCadastroCliente", typeof(bool));
            tabelaEventoInfo.Columns.Add("Estado", typeof(string));
            tabelaEventoInfo.Columns.Add("PDVSemConveniencia", typeof(string));
            tabelaEventoInfo.Columns.Add("Censura", typeof(string));
            tabelaEventoInfo.Columns.Add("EntradaAcompanhada", typeof(bool));
            tabelaEventoInfo.Columns.Add("RetiradaIngresso", typeof(string));
            tabelaEventoInfo.Columns.Add("MeiaEntrada", typeof(string));
            tabelaEventoInfo.Columns.Add("Promocoes", typeof(string));
            tabelaEventoInfo.Columns.Add("AberturaPortoes", typeof(string));
            tabelaEventoInfo.Columns.Add("DuracaoEvento", typeof(string));
            tabelaEventoInfo.Columns.Add("Release", typeof(string));
            tabelaEventoInfo.Columns.Add("DescricaoPadrao", typeof(string));
            tabelaEventoInfo.Columns.Add("TaxaConveniencia", typeof(int));
            tabelaEventoInfo.Columns.Add("TaxaMinima", typeof(decimal));
            tabelaEventoInfo.Columns.Add("TaxaMaxima", typeof(decimal));
            retorno.Tables.Add(tabelaEventoInfo);

            int clienteID = 0;

            Servidor servidor = new Servidor();

            senha = senha.Replace("'", "''");

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT i.ID, i.Status, u.Login AS Usuario, e.ImpressaoCodigoBarra, e.Nome AS Evento, i.EventoID, i.CodigoSequencial, vb.ClienteID, " +
                    "a.Horario, a.DisponivelVenda, i.ApresentacaoID, s.Nome AS Setor, s.Acesso, p.ImprimirCarimbo, p.CarimboTexto1, p.CarimboTexto2, s.Produto, s.LugarMarcado AS TipoSetor, c.Nome AS Cortesia, p.Nome AS Preco, " +
                    "a.Impressao AS ApresentacaoImpressao, p.Impressao AS PrecoImpressao, i.PrecoID,i.CortesiaID, " +
                    "i.BloqueioID, p.Valor, i.Codigo, i.CodigoBarra, i.CodigoBarraCliente, i.ApresentacaoSetorID, " +
                    "l.Nome AS Loja, tLocal.ID AS LocalID, tLocal.Nome AS LocalNome, tLocal.Logradouro, tLocal.Numero, tLocal.Cidade AS LocalCidade, tLocal.Estado AS LocalEstado, tLocal.Cep AS LocalCep, e.ObrigaCadastroCliente, " +
                    "	CASE " +
                    "		WHEN i.PacoteID = 0 THEN '" + TIPO_INGRESSO + "' " +
                    "		ELSE '" + TIPO_PACOTE + "'" +
                    "	END AS Tipo, " +
                    "ce.TaxaConveniencia, ce.TaxaMinima, ce.TaxaMaxima, ce.TaxaComissao, ce.ComissaoMinima, ce.ComissaoMaxima, e.TipoCodigoBarra " +
                    "FROM tIngresso i (NOLOCK) " +
                    "INNER JOIN tVendaBilheteria vb (NOLOCK) ON i.VendaBilheteriaID = vb.ID " +
                    "INNER JOIN tApresentacao a (NOLOCK) ON i.ApresentacaoID = a.ID " +
                    "INNER JOIN tSetor s (NOLOCK) ON i.SetorID = s.ID " +
                    "INNER JOIN tEvento e (NOLOCK) ON i.EventoID = e.ID " +
                    "INNER JOIN tUsuario u (NOLOCK) ON i.UsuarioID = u.ID " +
                    "INNER JOIN tLocal (NOLOCK) ON tLocal.ID = e.LocalID " +
                    "INNER JOIN tCanalEvento ce (NOLOCK) ON e.ID =  ce.EventoID AND ce.CanalID = " + canalID + " " +
                    "LEFT JOIN tLoja l (NOLOCK) ON i.LojaID = l.ID " +
                    "LEFT JOIN tCortesia c (NOLOCK) ON i.CortesiaID = c.ID " +
                    "LEFT JOIN tPreco p (NOLOCK) ON i.ApresentacaoSetorID = p.ApresentacaoSetorID AND i.PrecoID = p.ID " +
                    "WHERE (vb.Senha = '" + senha + "') " +
                    "ORDER BY i.Codigo"))
                {
                    DataRow novoItemGrid = null;
                    DataRow novoItemReserva = null;
                    DataRow linha = null;

                    while (oDataReader.Read())
                    {
                        if (bd.LerString("Status") != Ingresso.PRE_RESERVA)
                            throw new BilheteriaException("A senha '" + senha + "' não é referente a uma pré-reserva.");

                        if (!bd.LerBoolean("DisponivelVenda"))
                            throw new BilheteriaException("A apresentação: " + bd.LerDateTime("Horario") + " do evento: " + bd.LerString("Evento") + " não está disponível para venda.");

                        clienteID = bd.LerInt("ClienteID");

                        novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                        novoItemGrid[PRECOID] = 0;
                        novoItemGrid[APRESENTACAOSETORID] = bd.LerInt("ApresentacaoSetorID");
                        novoItemGrid[LOCALID] = bd.LerInt("LocalID");
                        novoItemGrid[OBRIGA_CADASTRO_CLIENTE] = "T";
                        novoItemGrid[EVENTO_PACOTE] = bd.LerString("Evento");
                        novoItemGrid[HORARIO] = bd.LerDateTime("Horario");
                        novoItemGrid[SETOR_PRODUTO] = bd.LerString("Setor");
                        novoItemGrid[CODIGO] = bd.LerString("Codigo");
                        novoItemGrid[LUGAR_MARCADO] = true;
                        novoItemGrid[PRECO] = 0;
                        novoItemGrid[CORTESIA] = 0;
                        novoItemGrid[VALOR] = 0;
                        novoItemGrid[TIPO] = bd.LerString("Tipo");
                        novoItemGrid[TIPO_LUGAR] = bd.LerString("TipoSetor");

                        novoItemGrid[TAXA_COMISSAO] = bd.LerInt("TaxaComissao");
                        novoItemGrid[COMISSAO_MAXIMA] = bd.LerDecimal("ComissaoMinima");
                        novoItemGrid[COMISSAO_MINIMA] = bd.LerDecimal("ComissaoMaxima");

                        novoItemGrid[CONV] = bd.LerInt("TaxaConveniencia");
                        novoItemGrid[CONV_MIN] = bd.LerDecimal("TaxaMinima");
                        novoItemGrid[CONV_MAX] = bd.LerDecimal("TaxaMaxima");

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);

                        novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[TIPO] = bd.LerString("Tipo");
                        novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                        novoItemReserva[LUGARID] = 0;
                        novoItemReserva[PACOTEID] = 0;
                        novoItemReserva[PRECOID] = 0;
                        novoItemReserva[CORTESIAID] = 0;
                        novoItemReserva[BLOQUEIOID] = 0;
                        novoItemReserva[INGRESSOID] = bd.LerInt("ID");
                        novoItemReserva[APRESENTACAOID] = bd.LerInt("ApresentacaoID");
                        novoItemReserva[EVENTOID] = bd.LerInt("EventoID");
                        novoItemReserva[CODIGO_SEQUENCIAL] = bd.LerInt("CodigoSequencial");

                        novoItemReserva[TIPO_CODIGO_BARRA] = bd.LerString(TIPO_CODIGO_BARRA);
                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);


                        linha = retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["IngressoID"] = bd.LerInt("ID");
                        linha["Usuario"] = bd.LerString("Usuario");
                        linha["Evento"] = bd.LerString("Evento");
                        linha["EventoID"] = bd.LerInt("EventoID");
                        linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                        linha["Horario"] = bd.LerDateTime("Horario");
                        linha["HorarioString"] = bd.LerStringFormatoDataHora("Horario");
                        linha["DataVendaString"] = servidor.Agora.ToString(Utilitario.FormatoDataHora);
                        linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        linha["ApresentacaoImpressao"] = bd.LerString("ApresentacaoImpressao");
                        linha["PrecoImpressao"] = bd.LerString("PrecoImpressao");
                        linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                        linha["Setor"] = bd.LerString("Setor");
                        linha["Acesso"] = bd.LerString("Acesso");
                        linha["ImprimirCarimbo"] = bd.LerString("ImprimirCarimbo");
                        linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                        linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                        linha["TipoSetor"] = bd.LerString("TipoSetor");
                        linha["Produto"] = bd.LerBoolean("Produto");
                        linha["Cortesia"] = bd.LerString("Cortesia");
                        linha["CortesiaID"] = bd.LerInt("CortesiaID");
                        linha["BloqueioID"] = bd.LerInt("BloqueioID");
                        linha["Preco"] = bd.LerString("Preco");
                        linha["PrecoID"] = bd.LerInt("PrecoID");
                        linha["Loja"] = bd.LerString("Loja");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        linha["Codigo"] = bd.LerString("Codigo");
                        linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                        linha["CodigoBarraCliente"] = bd.LerString("CodigoBarraCliente");
                        linha["LocalID"] = bd.LerInt("LocalID");
                        linha["LocalNome"] = bd.LerString("LocalNome");
                        linha["LocalEndereco"] = bd.LerString("Logradouro") + ((bd.LerInt("Numero") > 0) ? ", " + bd.LerInt("Numero").ToString() : "s/n");
                        linha["LocalCidade"] = bd.LerString("LocalCidade");
                        linha["LocalEstado"] = bd.LerString("LocalEstado");
                        linha["LocalCep"] = bd.LerString("LocalCep");
                        linha["ObrigaCadastroCliente"] = bd.LerString("ObrigaCadastroCliente");
                        linha["CodigoSequencial"] = bd.LerInt("CodigoSequencial");

                        retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].Rows.Add(linha);

                        // Alimenta as taxas e regiões
                        if (retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Select("EventoID = " + bd.LerInt("EventoID")).Length == 0)
                        {
                            // Busca taxas de entrega do Evento
                            DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas(bd.LerInt("EventoID"));

                            foreach (DataRow item in dtEventoTaxaEntrega.Rows)
                                retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].ImportRow(item);

                            // Busca regiões de entrega do Evento
                            DataTable dtEventoRegiao = new Regiao().GetRegioes(bd.LerInt("EventoID"));

                            foreach (DataRow item in dtEventoRegiao.Rows)
                                retorno.Tables[TABELA_EVENTO_REGIAO].ImportRow(item);
                        }

                        // Alimenta as formas de pagamento
                        if (retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Select("EventoID = " + bd.LerInt("EventoID")).Length == 0)
                        {
                            // Busca formas de pagamento do Evento
                            DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento(bd.LerInt("EventoID"), canalID);

                            foreach (DataRow item in dtEventoFormaPagamento.Rows)
                                retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].ImportRow(item);
                        }

                    }
                }


                if (retorno.Tables[TABELA_RESERVA].Rows.Count > 0)
                {
                    // Lista de eventos da pré-reserva
                    List<int> lstEventoID = new List<int>();

                    // Captura os eventos ids
                    foreach (DataRow dr in retorno.Tables[TABELA_RESERVA].Rows)
                    {
                        if (!lstEventoID.Contains(Convert.ToInt32(dr["EventoID"])))
                            lstEventoID.Add(Convert.ToInt32(dr["EventoID"]));
                    }

                    // Validação se o evento está disponível para o canal
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   COUNT(*) AS TotalEventosCanal " +
                        "FROM " +
                        "tCanalEvento (NOLOCK) " +
                        "WHERE " +
                        "   EventoID IN (" + Utilitario.ArrayToString(lstEventoID.ToArray()) + ") " +
                        "AND " +
                        "   CanalID = " + canalID))
                    {
                        if (oDataReader.Read())
                        {
                            if (bd.LerInt("TotalEventosCanal") != lstEventoID.Count)
                                throw new BilheteriaException("O evento não está disponível para o seu canal.");
                        }
                        else
                            throw new BilheteriaException("O evento não está disponível para o seu canal.");
                    }

                    DataRow novoItemEvento = null;

                    using (IDataReader oDataReader = bd.Consulta("" +
                       "SELECT " +
                       "    tEvento.ID, " +
                       "    tEvento.LocalID, " +
                       "    tEvento.Atencao, " +
                       "    CASE tEvento.ObrigaCadastroCliente " +
                       "        WHEN 'T' THEN 1 " +
                       "        WHEN 'F' THEN 0 " +
                       "        ELSE 0 " +
                       "    END AS ObrigaCadastroCliente, " +
                       "    tLocal.Estado AS Estado, " +
                       "    tEvento.PDVSemConveniencia, " +
                       "    tEvento.Censura, " +
                       "    tEvento.EntradaAcompanhada, " +
                       "    tEvento.RetiradaIngresso, " +
                       "    tEvento.MeiaEntrada, " +
                       "    tEvento.Promocoes, " +
                       "    tEvento.AberturaPortoes, " +
                       "    tEvento.DuracaoEvento, " +
                       "    tEvento.Release, " +
                       "    tEvento.DescricaoPadraoApresentacao AS DescricaoPadrao " +
                    "FROM tEvento (NOLOCK) " +
                    "INNER JOIN tLocal (NOLOCK) ON  tLocal.ID = tEvento.LocalID " +
                    "WHERE tEvento.ID IN ( " + Utilitario.ArrayToString(lstEventoID.ToArray()) + ")"))
                    {
                        while (oDataReader.Read())
                        {
                            // Alimenta as informações do evento
                            if (retorno.Tables["EventoInfo"].Select("ID = " + bd.LerInt("ID")).Length == 0)
                            {
                                novoItemEvento = retorno.Tables["EventoInfo"].NewRow();
                                novoItemEvento["ID"] = bd.LerInt("ID");
                                novoItemEvento["LocalID"] = bd.LerInt("LocalID");
                                novoItemEvento["Atencao"] = bd.LerString("Atencao");
                                novoItemEvento["ObrigaCadastroCliente"] = bd.LerBoolean("ObrigaCadastroCliente");
                                novoItemEvento["Estado"] = bd.LerString("Estado");
                                novoItemEvento["PDVSemConveniencia"] = bd.LerString("PDVSemConveniencia");
                                novoItemEvento["Censura"] = bd.LerString("Censura");
                                novoItemEvento["EntradaAcompanhada"] = bd.LerBoolean("EntradaAcompanhada");
                                novoItemEvento["RetiradaIngresso"] = bd.LerString("RetiradaIngresso");
                                novoItemEvento["MeiaEntrada"] = bd.LerString("MeiaEntrada");
                                novoItemEvento["Promocoes"] = bd.LerString("Promocoes");
                                novoItemEvento["AberturaPortoes"] = bd.LerString("AberturaPortoes");
                                novoItemEvento["DuracaoEvento"] = bd.LerString("DuracaoEvento");
                                novoItemEvento["Release"] = bd.LerString("Release");
                                novoItemEvento["DescricaoPadrao"] = bd.LerString("DescricaoPadrao");
                                novoItemEvento["TaxaConveniencia"] = 0;
                                novoItemEvento["TaxaMinima"] = 0;
                                novoItemEvento["TaxaMaxima"] = 0;
                                retorno.Tables["EventoInfo"].Rows.Add(novoItemEvento);
                            }
                        }
                    }



                    DataRow linhaCliente = retorno.Tables["Cliente"].NewRow();
                    linhaCliente["ID"] = clienteID;
                    retorno.Tables["Cliente"].Rows.Add(linhaCliente);
                }



            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;

        }

        public IRLib.Paralela.ClientObjects.EstruturaEventoInfo CarregarEventoInfo(int eventoID)
        {
            BD bd = new BD();

            try
            {

                IRLib.Paralela.ClientObjects.EstruturaEventoInfo eventoInfo = new IRLib.Paralela.ClientObjects.EstruturaEventoInfo();

                string sql = "SELECT Atencao,DescricaoPadraoApresentacao, PDVSemConveniencia, Censura,DuracaoEvento,RetiradaIngresso,MeiaEntrada,Promocoes,AberturaPortoes,Release " +
                    "FROM tEvento(NOLOCK) " +
                    "WHERE ID= " + eventoID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    eventoInfo.EventoID = eventoID;
                    eventoInfo.Atencao = bd.LerString("Atencao");
                    eventoInfo.PdvsSemTaxa = bd.LerString("PDVSemConveniencia");
                    eventoInfo.Censura = bd.LerString("Censura");
                    eventoInfo.DuracaoEvento = bd.LerString("DuracaoEvento");
                    eventoInfo.RetiradaIngressos = bd.LerString("RetiradaIngresso");
                    eventoInfo.MeiaEntrada = bd.LerString("MeiaEntrada");
                    eventoInfo.Promocoes = bd.LerString("Promocoes");
                    eventoInfo.AberturaPortoes = bd.LerString("AberturaPortoes");
                    eventoInfo.DescricaoPadrao = bd.LerString("DescricaoPadraoApresentacao");
                    eventoInfo.Release = bd.LerString("Release");
                    eventoInfo.Carregou = true;
                }
                bd.Fechar();


                return eventoInfo;

            }
            catch
            {
                throw;
            }

        }
        public IRLib.Paralela.ClientObjects.EstruturaLocalInfo CarregarLocalInfo(int localID)
        {
            BD bd = new BD();

            try
            {

                IRLib.Paralela.ClientObjects.EstruturaLocalInfo localInfo = new IRLib.Paralela.ClientObjects.EstruturaLocalInfo();

                string sql = "SELECT Logradouro,Numero, Complemento, Cep, Bairro, Cidade, Estado, Estacionamento, EstacionamentoObs, ArCondicionado, ComoChegar, HorariosBilheteria, AcessoNecessidadeEspecialObs, AcessoNecessidadeEspecial, IsNull(tPais.Nome, '') AS Pais " +
                    "FROM tLocal(NOLOCK) " +
                    "LEFT JOIN tPais (NOLOCK) ON tLocal.PaisID = tPais.ID " +
                    "WHERE tLocal.ID= " + localID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    localInfo.Endereco = bd.LerString("Logradouro");
                    localInfo.Numero = bd.LerInt("Numero");
                    localInfo.Complemento = bd.LerString("Complemento");
                    localInfo.Cep = bd.LerString("Cep");
                    localInfo.Bairro = bd.LerString("Bairro");
                    localInfo.Cidade = bd.LerString("Cidade");
                    localInfo.Estado = bd.LerString("Estado");
                    localInfo.Estacionamento = bd.LerBoolean("Estacionamento");
                    localInfo.EstacionamentoObs = bd.LerString("EstacionamentoObs");
                    localInfo.ArCondicionado = bd.LerBoolean("ArCondicionado");
                    localInfo.ComoChegar = bd.LerString("ComoChegar");
                    localInfo.HorariosBilheteria = bd.LerString("HorariosBilheteria");
                    localInfo.AcessoNecessidadeEspecialObs = bd.LerString("AcessoNecessidadeEspecialObs");
                    localInfo.AcessoNecessidadeEspecial = bd.LerBoolean("AcessoNecessidadeEspecial");
                    localInfo.Pais = bd.LerString("Pais");
                }
                bd.Fechar();


                return localInfo;

            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Método que retorna lista com os nomes das formas de pagamento do local. Usada para exibir essa informacao na tela de vendas.
        /// </summary>
        /// <param name="localID"></param>
        /// <returns></returns>
        public List<string> CarregarFormasPagamentosLocal(int localID)
        {
            BD bd = new BD();

            try
            {

                List<string> lista = new List<string>();

                string sql = "SELECT tFormaPagamento.Nome " +
                    "FROM tLocalFormaPagamento(NOLOCK) INNER JOIN tFormaPagamento(NOLOCK) ON tFormaPagamento.ID = tLocalFormaPagamento.FormaPagamentoID " +
                    "WHERE LocalID= " + localID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    lista.Add(bd.LerString("Nome"));
                }
                bd.Fechar();


                return lista;

            }
            catch
            {
                throw;
            }

        }

        private DataSet estruturaReservasInternet(int seedReserva)
        {


            if (seedReserva <= 0)
                throw new BilheteriaException("Seed da reserva não pode ser zero ou menor que zero.");

            DataSet ds = new DataSet("Reservas");

            //tabelas em ordem
            DataTable tGrid = new DataTable(TABELA_GRID);											// 1 - Cada item dessa tabela eh uma linha da grid na tela de reserva/venda
            DataTable tReserva = new DataTable(TABELA_RESERVA);									// 2 - Cada item dessa tabela corresponde a um ingresso de um item da grid relacionado pelo campo ReservaID
            //DataTable tImpressao;																			// 3 - Cada item dessa tabela eh um ingresso a ser impresso
            DataTable tMensagem = new DataTable(TABELA_MENSAGEM);									// 4 - Cada item dessa tabela eh uma mensagem a ser informada ao usuario
            DataTable tReservaLugarMarcado = new DataTable(TABELA_RESERVA_LUGAR_MARCADO); // 5 - Cada item dessa tabela corresponde a um lugar reservado (lugar marcado apenas)

            ds.Tables.Add(tGrid);
            ds.Tables.Add(tReserva);
            //ds.Tables.Add(tImpressao);
            ds.Tables.Add(tMensagem);
            ds.Tables.Add(tReservaLugarMarcado);

            //TABELA_GRID
            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = seedReserva; //começa pelo parametro

            tGrid.Columns.Add(colGridReservaID);
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string));
            tGrid.Columns.Add(HORARIO, typeof(string));
            //tGrid.Columns.Add(SETOR_PRODUTO, typeof(string));
            tGrid.Columns.Add(CODIGO, typeof(string));
            tGrid.Columns.Add(PRECO, typeof(string));
            tGrid.Columns.Add(CORTESIA, typeof(string));
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal));
            tGrid.Columns.Add(CONV, typeof(int));//.DefaultValue = 0;
            tGrid.Columns.Add("TaxaConvenienciaValor", typeof(decimal));//.DefaultValue = 0;
            tGrid.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(LUGAR_MARCADO, typeof(bool));
            tGrid.Columns.Add(APRESENTACAOSETORID, typeof(int));
            tGrid.Columns.Add(PRECOID, typeof(int));
            tGrid.Columns.Add(GERENCIAMENTO_INGRESSOS_ID, typeof(int));
            tGrid.Columns.Add(INGRESSOID, typeof(int));
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(APRESENTACAOID, typeof(int));
            tGrid.Columns.Add(PERMITIR_CANCELAMENTO_AVULSO, typeof(bool));
            tGrid.Columns.Add("CodigoProgramacao", typeof(string)).DefaultValue = string.Empty;
            tGrid.Columns.Add("CodigoCinema", typeof(string)).DefaultValue = string.Empty;

            //TABELA_RESERVA
            tReserva.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tReserva.Columns.Add(RESERVAID, typeof(int));
            tReserva.Columns.Add(LUGARID, typeof(int)); //se for LugarMarcado eh o ID do Lugar.
            tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
            tReserva.Columns.Add(INGRESSOID, typeof(int));
            tReserva.Columns.Add(PRECOID, typeof(int));
            tReserva.Columns.Add(GERENCIAMENTO_INGRESSOS_ID, typeof(int));
            tReserva.Columns.Add(APRESENTACAOID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(APRESENTACAOSETORID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(CORTESIAID, typeof(int)).DefaultValue = 0;
            //TABELA_ESTRUTURA_IMPRESSAO
            //tImpressao = Ingresso.EstruturaImpressao(); //impressao

            //TABELA_MENSAGEM
            tMensagem.Columns.Add(ALERTA_MSG, typeof(AlertaMensagem)); //codigo da msg
            tMensagem.Columns.Add(COD_MSG, typeof(CodMensagem)); //codigo da msg
            tMensagem.Columns.Add(DESC_MSG, typeof(string)); //guarda a mensagem
            tMensagem.Columns.Add(TIPO, typeof(char)); //guarda o tipo a q se refere a mensagem (ingresso ou pacote)

            //TABELA_RESERVA_LUGAR_MARCADO
            DataColumn colLugarID = new DataColumn(LUGARID, typeof(int)); //ID do lugar q vai reservar
            colLugarID.Unique = true;
            colLugarID.AutoIncrement = false;

            //			tReservaLugarMarcado.Columns.Add(colLugarID);
            //			tReservaLugarMarcado.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (cadeira, mesa aberta, mesa fechada)
            //			tReservaLugarMarcado.Columns.Add(QTDE_RESERVAR, typeof(int)); //qtde de lugares a reservar
            //			tReservaLugarMarcado.Columns.Add(QTDE_RESERVADO, typeof(int)); //qtde de lugares q foi reservado
            //			tReservaLugarMarcado.Columns.Add(RESERVADO, typeof(bool)); //se foi reservado ou nao. Se QTDE_RESERVAR == QTDE_RESERVADO
            //tReservaLugarMarcado.Columns.Add(STATUS, typeof(string)); //indica o status {Disponivel,Reservado,Vendido,Bloqueado,Impresso}



            ds.EnforceConstraints = true;


            //Grid com a Reserva
            DataColumn colReservaID = tReserva.Columns[RESERVAID];
            DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tReserva.Constraints.Add(idKeyRestraint1);

            //			//Reserva com a Impressao
            //			DataColumn colReservaIngressoID = tReserva.Columns[INGRESSOID];
            //			DataColumn colImpressaoIngressoID = tImpressao.Columns["ID"];
            //			DataRelation dr2 = new DataRelation("ReservaXImpressao", colReservaIngressoID, colImpressaoIngressoID, true);

            //			ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colReservaIngressoID, colImpressaoIngressoID);
            //			idKeyRestraint2.DeleteRule = Rule.Cascade;
            //			tImpressao.Constraints.Add(idKeyRestraint2);


            //ReservaLugarMarcado com a Reserva
            //DataColumn colReservaLugarID = tReserva.Columns[LUGARID];
            //DataRelation dr3 = new DataRelation("ReservaXReservaLugarMarcado",colLugarID, colReservaLugarID, true);

            //ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colLugarID, colReservaLugarID);
            //idKeyRestraint3.DeleteRule = Rule.Cascade;
            //tReserva.Constraints.Add(idKeyRestraint3);

            ds.Relations.Add(dr1);


            //ds.Relations.Add(dr2);
            //ds.Relations.Add(dr3);

            return ds;

        }

        //esse metodo precisa ser rodado no servidor (nao pode ser static)
        private DataSet estruturaReservas(int seedReserva)
        {

            if (seedReserva <= 0)
                throw new BilheteriaException("Seed da reserva não pode ser zero ou menor que zero.");

            DataSet ds = new DataSet("Reservas");

            //tabelas em ordem
            DataTable tGrid = new DataTable(TABELA_GRID);											// 1 - Cada item dessa tabela eh uma linha da grid na tela de reserva/venda
            DataTable tReserva = new DataTable(TABELA_RESERVA);									// 2 - Cada item dessa tabela corresponde a um ingresso de um item da grid relacionado pelo campo ReservaID
            DataTable tImpressao;																			// 3 - Cada item dessa tabela eh um ingresso a ser impresso
            DataTable tMensagem = new DataTable(TABELA_MENSAGEM);									// 4 - Cada item dessa tabela eh uma mensagem a ser informada ao usuario
            DataTable tReservaLugarMarcado = new DataTable(TABELA_RESERVA_LUGAR_MARCADO); // 5 - Cada item dessa tabela corresponde a um lugar reservado (lugar marcado apenas)
            DataTable tEventoTaxaEntrega = new DataTable(TABELA_EVENTO_TAXA_ENTREGA);
            DataTable tEventoRegiao = new DataTable(TABELA_EVENTO_REGIAO);
            DataTable tEventoFormaPagamento = new DataTable(TABELA_EVENTO_FORMA_PAGAMENTO);
            DataTable tCotaItemFormaPagamento = new DataTable(TABELA_COTA_ITEM_FORMA_PAGAMENTO);
            DataTable tCotaItem = new DataTable(TABELA_COTA_ITEM);
            DataTable tObrigatoriedade = new DataTable(TABELA_OBRIGATORIEDADE);

            //TABELA_GRID
            DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
            colGridReservaID.Unique = true;
            colGridReservaID.AutoIncrement = true;
            colGridReservaID.AutoIncrementStep = 1;
            colGridReservaID.AutoIncrementSeed = seedReserva; //começa pelo parametro

            tGrid.Columns.Add(colGridReservaID);
            tGrid.Columns.Add(EVENTO_PACOTE, typeof(string));
            tGrid.Columns.Add(HORARIO, typeof(string));
            tGrid.Columns.Add(SETOR_PRODUTO, typeof(string));
            tGrid.Columns.Add(CODIGO, typeof(string));
            tGrid.Columns.Add(PRECO, typeof(string));
            tGrid.Columns.Add(CORTESIA, typeof(string));
            tGrid.Columns.Add(VALOR, typeof(decimal));
            tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(CONV_MIN, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(CONV_MAX, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
            tGrid.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tGrid.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
            tGrid.Columns.Add(LUGAR_MARCADO, typeof(bool));
            tGrid.Columns.Add(APRESENTACAOSETORID, typeof(int));
            tGrid.Columns.Add(LOCALID, typeof(int));
            tGrid.Columns.Add(PRECOID, typeof(int));
            tGrid.Columns.Add(OBRIGA_CADASTRO_CLIENTE, typeof(string));
            tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
            tGrid.Columns.Add(COMISSAO_MINIMA, typeof(decimal));
            tGrid.Columns.Add(COMISSAO_MAXIMA, typeof(decimal));
            tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
            tGrid.Columns.Add(COTA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(COTA_ITEM_ID_APS, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(COTA, typeof(string));
            tGrid.Columns.Add(CLIENTE, typeof(string));
            tGrid.Columns.Add(PERMITIR_CANCELAMENTO_AVULSO, typeof(bool));
            tGrid.Columns.Add(SERIE, typeof(string)).DefaultValue = " - ";
            tGrid.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int)).DefaultValue = 0;
            tGrid.Columns.Add(LUGARID, typeof(int)).DefaultValue = 0;

            //TABELA_RESERVA
            tReserva.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
            tReserva.Columns.Add(RESERVAID, typeof(int));
            tReserva.Columns.Add(LUGARID, typeof(int)); //se for LugarMarcado eh o ID do Lugar.
            tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
            tReserva.Columns.Add(INGRESSOID, typeof(int));
            tReserva.Columns.Add(PRECOID, typeof(int));
            tReserva.Columns.Add(CORTESIAID, typeof(int));
            tReserva.Columns.Add(BLOQUEIOID, typeof(int));
            tReserva.Columns.Add(APRESENTACAOID, typeof(int));
            tReserva.Columns.Add(EVENTOID, typeof(int));
            tReserva.Columns.Add(CODIGO_SEQUENCIAL, typeof(int));

            tReserva.Columns.Add(COTA_ITEM_ID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(COTA_ITEM_ID_APS, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(QUANTIDADEPORCLIENTE_COTA_APS, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(QUANTIDADE_COTA_APS, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(QUANTIDADEPORCLIENTE_COTA, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(QUANTIDADE_COTA, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(QUANTIDADEPORCLIENTE_APRESENTACAO, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(QUANTIDADE_APRESENTACAO, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(QUANTIDADE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(OBRIGATORIEDADE_ID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(CLIENTE_ID, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(VALIDA_BIN, typeof(bool)).DefaultValue = false;
            tReserva.Columns.Add(VALIDA_BIN_APS, typeof(bool)).DefaultValue = false;

            tReserva.Columns.Add(CODIGO_PROMO, typeof(string));
            tReserva.Columns.Add(STATUS_CODIGO_PROMO, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(APRESENTACAOSETORID, typeof(int)).DefaultValue = 0;

            tReserva.Columns.Add(CLIENTE, typeof(string)).DefaultValue = string.Empty;
            tReserva.Columns.Add(CPF, typeof(string)).DefaultValue = string.Empty;
            tReserva.Columns.Add(SPECIALEVENT, typeof(string)).DefaultValue = string.Empty;
            tReserva.Columns.Add(GERENCIAMENTO_INGRESSOS_ID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(GERENCIAMENTO_INGRESSOS, typeof(string));
            tReserva.Columns.Add(SERIEID, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(NOMINAL, typeof(bool)).DefaultValue = 1;
            tReserva.Columns.Add(ITEM_PROMOCIONAL, typeof(bool)).DefaultValue = false;
            tReserva.Columns.Add(QUANTIDADE_POR_PROMOCIONAL, typeof(int)).DefaultValue = 0;
            tReserva.Columns.Add(TIPO_CODIGO_BARRA, typeof(string)).DefaultValue = string.Empty;
            tReserva.Columns.Add(CODIGO_BARRA, typeof(string)).DefaultValue = string.Empty;
            //TABELA_ESTRUTURA_IMPRESSAO
            tImpressao = Ingresso.EstruturaImpressao(); //impressao

            //TABELA_MENSAGEM
            tMensagem.Columns.Add(ALERTA_MSG, typeof(AlertaMensagem)); //codigo da msg
            tMensagem.Columns.Add(COD_MSG, typeof(CodMensagem)); //codigo da msg
            tMensagem.Columns.Add(DESC_MSG, typeof(string)); //guarda a mensagem
            tMensagem.Columns.Add(TIPO, typeof(char)); //guarda o tipo a q se refere a mensagem (ingresso ou pacote)

            //TABELA_RESERVA_LUGAR_MARCADO
            DataColumn colLugarID = new DataColumn(LUGARID, typeof(int)); //ID do lugar q vai reservar
            colLugarID.Unique = true;
            colLugarID.AutoIncrement = false;

            tReservaLugarMarcado.Columns.Add(colLugarID);
            tReservaLugarMarcado.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (cadeira, mesa aberta, mesa fechada)
            tReservaLugarMarcado.Columns.Add(QTDE_RESERVAR, typeof(int)); //qtde de lugares a reservar
            tReservaLugarMarcado.Columns.Add(QTDE_RESERVADO, typeof(int)); //qtde de lugares q foi reservado
            tReservaLugarMarcado.Columns.Add(RESERVADO, typeof(bool)); //se foi reservado ou nao. Se QTDE_RESERVAR == QTDE_RESERVADO
            //tReservaLugarMarcado.Columns.Add(STATUS, typeof(string)); //indica o status {Disponivel,Reservado,Vendido,Bloqueado,Impresso}

            //TABELA_EVENTO_TAXA_ENTREGA
            tEventoTaxaEntrega.Columns.Add("ID", typeof(int));
            tEventoTaxaEntrega.Columns.Add("Nome", typeof(string));
            tEventoTaxaEntrega.Columns.Add("Valor", typeof(decimal));
            tEventoTaxaEntrega.Columns.Add("RegiaoID", typeof(int));
            tEventoTaxaEntrega.Columns.Add("EventoID", typeof(int));
            tEventoTaxaEntrega.Columns.Add("Prazo", typeof(int));
            tEventoTaxaEntrega.Columns.Add("Estado", typeof(string));
            //tEventoTaxaEntrega.Columns.Add(EVENTOID, typeof(int));
            //tEventoTaxaEntrega.Columns.Add(TAXAENTREGAID, typeof(int));

            //TABELA_EVENTO_REGIAO
            tEventoRegiao.Columns.Add("ID", typeof(int));
            tEventoRegiao.Columns.Add("Nome", typeof(string));
            tEventoRegiao.Columns.Add("EventoID", typeof(int));
            tEventoRegiao.Columns.Add("TaxaEntregaID", typeof(int));

            //TABELA_EVENTO_FORMA_PAGAMENTO
            tEventoFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));
            tEventoFormaPagamento.Columns.Add("EventoID", typeof(int));

            //TABELA COTA_ITEM
            tCotaItem.Columns.Add("ID", typeof(int));
            tCotaItem.Columns.Add("CotaID", typeof(int));
            tCotaItem.Columns.Add("PrecoIniciaCom", typeof(string));
            tCotaItem.Columns.Add("ParceiroID", typeof(int));
            tCotaItem.Columns.Add("ValidaBin", typeof(bool));
            tCotaItem.Columns.Add("TextoValidacao", typeof(string));
            tCotaItem.Columns.Add("ObrigatoriedadeID", typeof(int));
            tCotaItem.Columns.Add("Quantidade", typeof(int));
            tCotaItem.Columns.Add("QuantidadePorCliente", typeof(int));
            tCotaItem.Columns.Add("CPFResponsavel", typeof(bool));
            tCotaItem.Columns.Add("Termo", typeof(string));


            //TABELA COTA ITEM FORMA PAGAMENTO
            tCotaItemFormaPagamento.Columns.Add("CotaItemID", typeof(int));
            tCotaItemFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));


            tObrigatoriedade.Columns.Add("ID", typeof(int));
            tObrigatoriedade.Columns.Add("Nome", typeof(bool));
            tObrigatoriedade.Columns.Add("RG", typeof(bool));
            tObrigatoriedade.Columns.Add("CPF", typeof(bool));
            tObrigatoriedade.Columns.Add("Telefone", typeof(bool));
            tObrigatoriedade.Columns.Add("TelefoneComercial", typeof(bool));
            tObrigatoriedade.Columns.Add("Celular", typeof(bool));
            tObrigatoriedade.Columns.Add("DataNascimento", typeof(bool));
            tObrigatoriedade.Columns.Add("Email", typeof(bool));
            tObrigatoriedade.Columns.Add("CEPCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("EnderecoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("NumeroCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("ComplementoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("BairroCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("CidadeCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("EstadoCliente", typeof(bool));
            tObrigatoriedade.Columns.Add("NomeEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CPFEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("RGEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CEPEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("EnderecoEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("NumeroEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("ComplementoEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("BairroEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("CidadeEntrega", typeof(bool));
            tObrigatoriedade.Columns.Add("EstadoEntrega", typeof(bool));

            ds.Tables.Add(tObrigatoriedade);
            ds.Tables.Add(tGrid);
            ds.Tables.Add(tReserva);
            ds.Tables.Add(tImpressao);
            ds.Tables.Add(tMensagem);
            ds.Tables.Add(tReservaLugarMarcado);
            ds.Tables.Add(tEventoTaxaEntrega);
            ds.Tables.Add(tEventoRegiao);
            ds.Tables.Add(tEventoFormaPagamento);
            ds.Tables.Add(tCotaItemFormaPagamento);
            ds.Tables.Add(tCotaItem);

            //Grid com a Reserva
            DataColumn colReservaID = tReserva.Columns[RESERVAID];
            DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tReserva.Constraints.Add(idKeyRestraint1);

            //Reserva com a Impressao
            DataColumn colReservaIngressoID = tReserva.Columns[INGRESSOID];
            DataColumn colImpressaoIngressoID = tImpressao.Columns["ID"];
            DataRelation dr2 = new DataRelation("ReservaXImpressao", colReservaIngressoID, colImpressaoIngressoID, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colReservaIngressoID, colImpressaoIngressoID);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tImpressao.Constraints.Add(idKeyRestraint2);


            //ReservaLugarMarcado com a Reserva
            //DataColumn colReservaLugarID = tReserva.Columns[LUGARID];
            //DataRelation dr3 = new DataRelation("ReservaXReservaLugarMarcado",colLugarID, colReservaLugarID, true);

            //ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colLugarID, colReservaLugarID);
            //idKeyRestraint3.DeleteRule = Rule.Cascade;
            //tReserva.Constraints.Add(idKeyRestraint3);




            ds.EnforceConstraints = true;

            ds.Relations.Add(dr1);
            ds.Relations.Add(dr2);
            //ds.Relations.Add(dr3);

            return ds;

        }

        //esse metodo precisa ser rodado no cliente (tem q ser static)
        public static DataSet EstruturaReservas()
        {

            try
            {

                DataSet ds = new DataSet("Reservas");

                //tabelas em ordem
                DataTable tGrid = new DataTable(TABELA_GRID);											// 1 - Cada item dessa tabela eh uma linha da grid na tela de reserva/venda
                DataTable tReserva = new DataTable(TABELA_RESERVA);									// 2 - Cada item dessa tabela corresponde a um ingresso de um item da grid relacionado pelo campo ReservaID
                DataTable tImpressao;																			// 3 - Cada item dessa tabela eh um ingresso a ser impresso
                DataTable tMensagem = new DataTable(TABELA_MENSAGEM);									// 4 - Cada item dessa tabela eh uma mensagem a ser informada ao usuario
                DataTable tReservaLugarMarcado = new DataTable(TABELA_RESERVA_LUGAR_MARCADO); // 5 - Cada item dessa tabela corresponde a um lugar reservado (lugar marcado apenas)
                DataTable tEventoTaxaEntrega = new DataTable(TABELA_EVENTO_TAXA_ENTREGA);
                DataTable tEventoRegiao = new DataTable(TABELA_EVENTO_REGIAO);
                DataTable tEventoFormaPagamento = new DataTable(TABELA_EVENTO_FORMA_PAGAMENTO);
                DataTable tCotaItem = new DataTable(TABELA_COTA_ITEM);
                DataTable tCotaItemFormaPagamento = new DataTable(TABELA_COTA_ITEM_FORMA_PAGAMENTO);
                DataTable tObrigatoriedade = new DataTable(TABELA_OBRIGATORIEDADE);

                //TABELA_GRID
                DataColumn colGridReservaID = new DataColumn(RESERVAID, typeof(int));
                colGridReservaID.Unique = true;
                colGridReservaID.AutoIncrement = true;
                colGridReservaID.AutoIncrementStep = 1;
                colGridReservaID.AutoIncrementSeed = 1; //começa pelo parametro

                tGrid.Columns.Add(colGridReservaID);
                tGrid.Columns.Add(LOCALID, typeof(int));
                tGrid.Columns.Add(EVENTO_PACOTE, typeof(string));
                tGrid.Columns.Add(HORARIO, typeof(string));
                tGrid.Columns.Add(SETOR_PRODUTO, typeof(string));
                tGrid.Columns.Add(CODIGO, typeof(string));
                tGrid.Columns.Add(PRECO, typeof(string));
                tGrid.Columns.Add(CORTESIA, typeof(string));
                tGrid.Columns.Add(VALOR, typeof(decimal));
                tGrid.Columns.Add(CONV, typeof(int)).DefaultValue = 0;
                tGrid.Columns.Add(VALOR_CONV, typeof(decimal)).DefaultValue = 0;
                tGrid.Columns.Add(CONV_MIN, typeof(decimal)).DefaultValue = 0;
                tGrid.Columns.Add(CONV_MAX, typeof(decimal)).DefaultValue = 0;
                tGrid.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
                tGrid.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (pista, cadeira, mesa aberta, mesa fechada)
                tGrid.Columns.Add(LUGAR_MARCADO, typeof(bool));
                tGrid.Columns.Add(APRESENTACAOSETORID, typeof(int));
                tGrid.Columns.Add(PRECOID, typeof(int));
                tGrid.Columns.Add(OBRIGA_CADASTRO_CLIENTE, typeof(string));
                tGrid.Columns.Add(TAXA_COMISSAO, typeof(int));
                tGrid.Columns.Add(COMISSAO_MINIMA, typeof(decimal));
                tGrid.Columns.Add(COMISSAO_MAXIMA, typeof(decimal));
                tGrid.Columns.Add(COMISSAO_VALOR, typeof(decimal));
                tGrid.Columns.Add(COTA_ITEM_ID, typeof(int));
                tGrid.Columns.Add(COTA_ITEM_ID_APS, typeof(int));
                tGrid.Columns.Add(COTA, typeof(string));
                tGrid.Columns.Add(CLIENTE, typeof(string)).DefaultValue = " - ";
                tGrid.Columns.Add(CPF, typeof(string)).DefaultValue = string.Empty;
                tGrid.Columns.Add(PERMITIR_CANCELAMENTO_AVULSO, typeof(bool));
                tGrid.Columns.Add(SERIE, typeof(string)).DefaultValue = " - ";
                tGrid.Columns.Add(ASSINATURA_CLIENTE_ID, typeof(int)).DefaultValue = 0;
                tGrid.Columns.Add(LUGARID, typeof(int)).DefaultValue = 0;

                //TABELA_RESERVA
                tReserva.Columns.Add(TIPO, typeof(char)); //guarda o tipo de reserva (ingresso ou pacote)
                tReserva.Columns.Add(RESERVAID, typeof(int));
                tReserva.Columns.Add(LUGARID, typeof(int)); //se for LugarMarcado eh o ID do Lugar.
                tReserva.Columns.Add(PACOTEID, typeof(int)); //Se for Pacote eh o ID do Pacote.
                tReserva.Columns.Add(INGRESSOID, typeof(int));
                tReserva.Columns.Add(PRECOID, typeof(int));
                tReserva.Columns.Add(CORTESIAID, typeof(int));
                tReserva.Columns.Add(BLOQUEIOID, typeof(int));
                tReserva.Columns.Add(APRESENTACAOID, typeof(int));
                tReserva.Columns.Add(EVENTOID, typeof(int));
                tReserva.Columns.Add(CODIGO_SEQUENCIAL, typeof(int));
                tReserva.Columns.Add(COTA_ITEM_ID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(COTA_ITEM_ID_APS, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADE_COTA_APS, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADEPORCLIENTE_COTA_APS, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADE_COTA, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADEPORCLIENTE_COTA, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADE_APRESENTACAO, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADEPORCLIENTE_APRESENTACAO, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(DONO_ID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(OBRIGATORIEDADE_ID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(VALIDA_BIN, typeof(bool)).DefaultValue = false;
                tReserva.Columns.Add(VALIDA_BIN_APS, typeof(bool)).DefaultValue = false;
                tReserva.Columns.Add(CODIGO_PROMO, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(STATUS_CODIGO_PROMO, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(APRESENTACAOSETORID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(CLIENTE, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(CPF, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(SPECIALEVENT, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(GERENCIAMENTO_INGRESSOS_ID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(GERENCIAMENTO_INGRESSOS, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(SERIEID, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(NOMINAL, typeof(bool)).DefaultValue = 1;
                tReserva.Columns.Add(ITEM_PROMOCIONAL, typeof(bool)).DefaultValue = false;
                tReserva.Columns.Add(QUANTIDADE_POR_PROMOCIONAL, typeof(int)).DefaultValue = 0;
                tReserva.Columns.Add(TIPO_CODIGO_BARRA, typeof(string)).DefaultValue = string.Empty;
                tReserva.Columns.Add(CODIGO_BARRA, typeof(string)).DefaultValue = string.Empty;

                //TABELA_ESTRUTURA_IMPRESSAO
                tImpressao = Ingresso.EstruturaImpressao(); //impressao

                //TABELA_MENSAGEM
                tMensagem.Columns.Add(ALERTA_MSG, typeof(AlertaMensagem)); //codigo da msg
                tMensagem.Columns.Add(COD_MSG, typeof(CodMensagem)); //codigo da msg
                tMensagem.Columns.Add(DESC_MSG, typeof(string)); //guarda a mensagem
                tMensagem.Columns.Add(TIPO, typeof(char)); //guarda o tipo a q se refere a mensagem (ingresso ou pacote)

                //TABELA_RESERVA_LUGAR_MARCADO
                DataColumn colLugarID = new DataColumn(LUGARID, typeof(int)); //ID do lugar q vai reservar
                //colLugarID.Unique = true;
                colLugarID.AutoIncrement = false;

                tReservaLugarMarcado.Columns.Add(colLugarID);
                tReservaLugarMarcado.Columns.Add(TIPO_LUGAR, typeof(string)); //guarda o tipo(setor) de ingresso (cadeira, mesa aberta, mesa fechada)
                tReservaLugarMarcado.Columns.Add(QTDE_RESERVAR, typeof(int)); //qtde de lugares a reservar
                tReservaLugarMarcado.Columns.Add(QTDE_RESERVADO, typeof(int)); //qtde de lugares q foi reservado
                tReservaLugarMarcado.Columns.Add(RESERVADO, typeof(bool)); //se foi reservado ou nao. Se QTDE_RESERVAR == QTDE_RESERVADO
                tReservaLugarMarcado.Columns.Add(CODIGO, typeof(string));

                //tReservaLugarMarcado.Columns.Add(STATUS, typeof(string)); //indica o status {Disponivel,Reservado,Vendido,Bloqueado,Impresso}

                //TABELA_EVENTO_TAXA_ENTREGA
                tEventoTaxaEntrega.Columns.Add("ID", typeof(int));
                tEventoTaxaEntrega.Columns.Add("Nome", typeof(string));
                tEventoTaxaEntrega.Columns.Add("Valor", typeof(decimal));
                tEventoTaxaEntrega.Columns.Add("RegiaoID", typeof(int));
                tEventoTaxaEntrega.Columns.Add("EventoID", typeof(int));
                tEventoTaxaEntrega.Columns.Add("Prazo", typeof(int));
                tEventoTaxaEntrega.Columns.Add("Estado", typeof(string));
                //tEventoTaxaEntrega.Columns.Add(EVENTOID, typeof(int));
                //tEventoTaxaEntrega.Columns.Add(TAXAENTREGAID, typeof(int));

                //TABELA_EVENTO_REGIAO
                tEventoRegiao.Columns.Add("ID", typeof(int));
                tEventoRegiao.Columns.Add("Nome", typeof(string));
                tEventoRegiao.Columns.Add("EventoID", typeof(int));
                tEventoRegiao.Columns.Add("TaxaEntregaID", typeof(int));

                //TABELA_EVENTO_FORMA_PAGAMENTO
                tEventoFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));
                tEventoFormaPagamento.Columns.Add("EventoID", typeof(int));

                //TABELA_COTA_ITEM
                tCotaItem.Columns.Add("ID", typeof(int));
                tCotaItem.Columns.Add("CotaID", typeof(int));
                tCotaItem.Columns.Add("PrecoIniciaCom", typeof(string));
                tCotaItem.Columns.Add("ParceiroID", typeof(int));
                tCotaItem.Columns.Add("ValidaBin", typeof(bool));
                tCotaItem.Columns.Add("TextoValidacao", typeof(string));
                tCotaItem.Columns.Add("ObrigatoriedadeID", typeof(int));
                tCotaItem.Columns.Add("Quantidade", typeof(int));
                tCotaItem.Columns.Add("QuantidadePorCliente", typeof(int));

                //TABELA_COTAITEM_FORMA_PAGAMENTO
                tCotaItemFormaPagamento.Columns.Add("CotaItemID", typeof(int));
                tCotaItemFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));

                tObrigatoriedade.Columns.Add("ID", typeof(int));
                tObrigatoriedade.Columns.Add("Nome", typeof(bool));
                tObrigatoriedade.Columns.Add("RG", typeof(bool));
                tObrigatoriedade.Columns.Add("CPF", typeof(bool));
                tObrigatoriedade.Columns.Add("Telefone", typeof(bool));
                tObrigatoriedade.Columns.Add("TelefoneComercial", typeof(bool));
                tObrigatoriedade.Columns.Add("Celular", typeof(bool));
                tObrigatoriedade.Columns.Add("DataNascimento", typeof(bool));
                tObrigatoriedade.Columns.Add("Email", typeof(bool));
                tObrigatoriedade.Columns.Add("CEPCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("EnderecoCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("NumeroCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("ComplementoCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("BairroCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("CidadeCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("EstadoCliente", typeof(bool));
                tObrigatoriedade.Columns.Add("NomeEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("CPFEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("RGEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("CEPEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("EnderecoEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("NumeroEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("ComplementoEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("BairroEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("CidadeEntrega", typeof(bool));
                tObrigatoriedade.Columns.Add("EstadoEntrega", typeof(bool));

                ds.Tables.Add(tGrid);
                ds.Tables.Add(tReserva);
                ds.Tables.Add(tImpressao);
                ds.Tables.Add(tMensagem);
                ds.Tables.Add(tReservaLugarMarcado);
                ds.Tables.Add(tEventoTaxaEntrega);
                ds.Tables.Add(tEventoRegiao);
                ds.Tables.Add(tEventoFormaPagamento);
                ds.Tables.Add(tCotaItem);
                ds.Tables.Add(tCotaItemFormaPagamento);
                ds.Tables.Add(tObrigatoriedade);

                //Grid com a Reserva
                DataColumn colReservaID = tReserva.Columns[RESERVAID];
                DataRelation dr1 = new DataRelation("GridXReserva", colGridReservaID, colReservaID, true);

                ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colGridReservaID, colReservaID);
                idKeyRestraint1.DeleteRule = Rule.Cascade;
                tReserva.Constraints.Add(idKeyRestraint1);

                //Reserva com a Impressao
                DataColumn colReservaIngressoID = tReserva.Columns[INGRESSOID];
                DataColumn colImpressaoIngressoID = tImpressao.Columns["ID"];
                DataRelation dr2 = new DataRelation("ReservaXImpressao", colReservaIngressoID, colImpressaoIngressoID, true);

                ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colReservaIngressoID, colImpressaoIngressoID);
                idKeyRestraint2.DeleteRule = Rule.Cascade;
                tImpressao.Constraints.Add(idKeyRestraint2);


                //ReservaLugarMarcado com a Reserva
                //DataColumn colReservaLugarID = tReserva.Columns[LUGARID];
                //DataRelation dr3 = new DataRelation("ReservaXReservaLugarMarcado",colLugarID, colReservaLugarID, true);

                //ForeignKeyConstraint idKeyRestraint3 = new ForeignKeyConstraint(colLugarID, colReservaLugarID);
                //idKeyRestraint3.DeleteRule = Rule.Cascade;
                //tReserva.Constraints.Add(idKeyRestraint3);




                ds.EnforceConstraints = true;

                ds.Relations.Add(dr1);
                ds.Relations.Add(dr2);
                //ds.Relations.Add(dr3);

                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna uma 'stringao' com os lugares de uma ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        private string mapa(int apresentacaoSetorID, int setorID)
        {

            try
            {

                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
                apresentacaoSetor.SetorID.Valor = setorID;
                apresentacaoSetor.Control.ID = apresentacaoSetorID;

                string mapa = apresentacaoSetor.Mapa();

                return mapa;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna lugares de um mapa
        /// </summary>
        /// <returns></returns>
        public DataTable InfoMapa(int apresentacaoSetorID, int setorID)
        {

            DataTable retorno = new DataTable("Mapa");
            retorno.Columns.Add("ApresentacaoSetorID", typeof(int));
            retorno.Columns.Add("Mapa", typeof(string));

            string sMapa = mapa(apresentacaoSetorID, setorID);

            DataRow linhaMapa = retorno.NewRow();
            linhaMapa["ApresentacaoSetorID"] = apresentacaoSetorID;
            linhaMapa["Mapa"] = sMapa;
            retorno.Rows.Add(linhaMapa);

            return retorno;

        }

        public static DataTable EstruturaFormaPagamento()
        {
            DataTable tabela = new DataTable("FormaPagamento");

            //tabela.Columns.Add("ID", typeof(int)); 
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
            tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));
            tabela.Columns.Add("ValorTotal", typeof(decimal));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Data", typeof(string));
            tabela.Columns.Add("PagamentoProcessado", typeof(bool));
            tabela.Columns.Add("TaxaProcessamentoValor", typeof(decimal)).DefaultValue = 0;
            tabela.Columns.Add("NotaFiscalEstabelecimento", typeof(string));

            return tabela;
        }

        /// <summary>		
        /// Obter as forma de pagamento de uma venda a partir da senha informada
        /// </summary>
        public DataTable FormasPagamento(string senha, ref int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {

                DataTable tabela = EstruturaFormaPagamento();
                tabela.Columns.Add("ID", typeof(int));

                string sql = "SELECT tVendaBilheteria.ID as VendaBilheteriaID, IsNull(tVendaBilheteria.PagamentoProcessado, 'T') AS PagamentoProcessado, tFormaPagamento.ID as FormaPagamentoID, tFormaPagamento.Nome, " +
                             "tVendaBilheteriaFormaPagamento.Valor, tVendaBilheteria.TaxaEntregaValor, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.ValorTotal,  " +
                             "tVendaBilheteria.Status, tVendaBilheteria.DataVenda, tVendaBilheteria.TaxaProcessamentoValor, NotaFiscalEstabelecimento " +
                    "FROM tVendaBilheteria (NOLOCK) " +
                    "LEFT JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID " +
                    "LEFT JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE tVendaBilheteria.Senha='" + senha + "'";
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    vendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    if (vendaBilheteriaID == 0)
                        throw new BilheteriaException("Senha não existe.");

                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["ID"] = bd.LerInt("FormaPagamentoID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                        linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha["ValorTotal"] = bd.LerDecimal("ValorTotal");
                        linha["Status"] = bd.LerString("Status");
                        linha["Data"] = bd.LerStringFormatoDataHora("DataVenda");
                        linha["PagamentoProcessado"] = bd.LerBoolean("PagamentoProcessado");
                        linha["TaxaProcessamentoValor"] = bd.LerDecimal("TaxaProcessamentoValor");
                        linha["NotaFiscalEstabelecimento"] = bd.LerString("NotaFiscalEstabelecimento");

                        tabela.Rows.Add(linha);

                    } while (bd.Consulta().Read());
                    bd.Consulta().Close();
                }
                else
                {
                    try
                    {
                        bd.Consulta().Close();
                    }
                    catch { }
                    throw new BilheteriaException("Senha não existe.");
                }

                return tabela;

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

        public static DataTable EstruturaInfoSenha()
        {
            DataTable tabela = new DataTable("InfoSenha");
            tabela.Columns.Add("ClienteNome", typeof(string));
            tabela.Columns.Add("ClienteID", typeof(int));
            tabela.Columns.Add("TaxaEntregaNome", typeof(string));
            tabela.Columns.Add("TaxaEntregaValor", typeof(decimal));
            tabela.Columns.Add("TaxaConvenienciaValorTotal", typeof(decimal));
            tabela.Columns.Add("ValorTotal", typeof(decimal));
            tabela.Columns.Add("Data", typeof(string));
            return tabela;
        }

        /// <summary>		
        /// Obter as forma de pagamento de uma venda a partir da senha informada
        /// </summary>
        public DataTable InfoSenha(string senha, ref int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {

                DataTable tabela = EstruturaInfoSenha();
                tabela.Columns.Add("ID", typeof(int));

                string sql = @"SELECT tVendaBilheteria.ID as VendaBilheteriaID, 
									  IsNull(tTaxaEntrega.Nome,' ') as TaxaEntregaNome,
									  tCliente.Nome as ClienteNome,
									  tVendaBilheteria.ClienteID, 
									  tVendaBilheteria.TaxaEntregaValor, 
									  tVendaBilheteria.TaxaConvenienciaValorTotal, 
									  tVendaBilheteria.ValorTotal, 
									  tVendaBilheteria.DataVenda 
						FROM tVendaBilheteria (NOLOCK) 
						LEFT JOIN tTaxaEntrega (NOLOCK) 
						ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
						LEFT JOIN tCliente (NOLOCK) 
						ON tCliente.ID = tVendaBilheteria.ClienteID
						WHERE tVendaBilheteria.Senha='" + senha + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    vendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    if (vendaBilheteriaID == 0)
                        throw new BilheteriaException("Senha não existe.");

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteNome"] = bd.LerString("ClienteNome");
                        linha["ClienteID"] = bd.LerInt("ClienteID");
                        linha["TaxaEntregaNome"] = bd.LerString("TaxaEntregaNome");
                        linha["TaxaEntregaValor"] = bd.LerDecimal("TaxaEntregaValor");
                        linha["TaxaConvenienciaValorTotal"] = bd.LerDecimal("TaxaConvenienciaValorTotal");
                        linha["ValorTotal"] = bd.LerDecimal("ValorTotal");
                        linha["Data"] = bd.LerStringFormatoDataHora("DataVenda");
                        tabela.Rows.Add(linha);
                    } while (bd.Consulta().Read());
                    bd.Consulta().Close();
                }
                else
                {
                    try
                    {
                        bd.Consulta().Close();
                    }
                    catch { }
                    throw new BilheteriaException("Senha não existe.");
                }

                return tabela;

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

        /// <summary>
        /// Retorna um 'stringao' com os lugares dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public string StatusLugaresMarcado(string apresentacaoSetorID)
        {
            try
            {
                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
                string mapaStatus = apresentacaoSetor.StatusLugaresMarcados(apresentacaoSetorID);

                return mapaStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um 'stringao' com os lugares dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public string StatusLugares(int apresentacaoSetorID)
        {

            try
            {

                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
                apresentacaoSetor.Control.ID = apresentacaoSetorID;

                string mapaStatus = apresentacaoSetor.StatusLugares();

                return mapaStatus;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int BuscarBloqueios(int apresentacaoSetorID, int bloqueioID)
        {
            BD bd = new BD();
            try
            {

                string sql = "SELECT Count(*) " +
                    "FROM tIngresso " +
                    "WHERE ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.BLOQUEADO + "' AND BloqueioID=" + bloqueioID;

                object obj = bd.ConsultaValor(sql);

                int qtde = (obj != null) ? Convert.ToInt32(obj) : 0;

                bd.Fechar();

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void oIngresso_ReservaPista(int ID)
        {
            ids.Add(ID);
        }
        protected void valeIngresso_Reservar(int ID)
        {
            ValeIngressoReservados.Add(ID);
        }
        /// <summary>
        /// Reserva o vale ingresso. Sistema e Internet.
        /// </summary>
        /// <param name="valeIngressoTipoID"></param>
        /// <param name="quantidadeReservar"></param>
        /// <param name="clienteID"></param>
        /// <param name="canalID"></param>
        /// <param name="lojaID"></param>
        /// <param name="usuarioID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public BindingList<EstruturaRetornoReservaValeIngresso> ReservarValeIngresso(int valeIngressoTipoID, int quantidadeReservar, int clienteID, int canalID, int lojaID, int usuarioID, string sessionID)
        {
            BD bd = new BD();
            ValeIngresso valeIngresso = new ValeIngresso();

            try
            {
                BindingList<EstruturaRetornoReservaValeIngresso> retorno;

                bd.IniciarTransacao();
                //valeIngresso.Processar += new ValeIngresso.ProcessHandler(valeIngresso_Reservar);
                retorno = valeIngresso.Reservar(bd, valeIngressoTipoID, quantidadeReservar, clienteID, canalID, lojaID, usuarioID, sessionID);
                bd.FinalizarTransacao();
                return retorno;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Reserva ingresso, pacote e produto. Retorna um dataSet com as informações dos ingressos reservados.
        /// </summary>
        /// <returns></returns>
        public DataSet ReservarPista(int lojaID, int usuarioID, int canalID, int pacoteID, int qtdePacote, int eventoID, int apresentacaoID, int apresentacaoSetorID, int precoID, string precoNome, int cortesiaID, int bloqueioID,
            int qtdeIngresso, int seedReserva, bool buscaTaxasEntrega, bool buscaFormaPagamento, bool preReserva, int qtdeJaReservadaCota, EstruturaCotasInfo cotasInfo, int qtdeJaReservadaCotaItem, string eventoEspecial, int serieID, bool itemPromocional, int quantidadePorPromocional, int gerenciamentoIngressosID)
        {
            //												                    |-Pacote-------------------|  |-Ingresso-----------------------------------------------------------------------------------------|

            BD bd = new BD();

            bool reservarIngresso = (qtdeIngresso > 0 && apresentacaoSetorID != 0 && (precoID != 0 || preReserva));
            bool reservarPacote = (qtdePacote > 0 && (pacoteID != 0 || preReserva));

            if (!reservarIngresso && !reservarPacote)
                return null;


            //retorna um DataSet com duas tabelas para impressao, uma de ingressos normais e outra de ingressos do pacote
            //e uma tabela de reservas (informaçoes para a Grid)
            DataSet retorno = estruturaReservas(seedReserva);
            Ingresso oIngresso = new Ingresso();
            Cota oCota = new Cota();
            CotaItem oCotaItem = new CotaItem();
            //Ingressos
            #region //reservar os ingressos
            if (reservarIngresso)
            {

                CodMensagem codMsg = CodMensagem.Nulo;
                AlertaMensagem alertaMsg = AlertaMensagem.Nulo;

                try
                {

                    Apresentacao apresentacao = new Apresentacao();
                    apresentacao.Control.ID = apresentacaoID;

                    if (!apresentacao.Venda())
                    {
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Apresentação não está mais a venda. Escolha outra apresentação.", CodMensagemReserva.ApresentacaoIndisponivel);
                    }



                    //qtdes de preço
                    int qtdePrecoDisponivel = 0; //qtde de preços disponiveis, q podem vender, independente do canal
                    int qtdePrecoPorCliente = 0; //qtde de preços q podem vender por cliente independente do canal
                    int qtdePrecoPorCanal = 0; //qtde de preços q podem vender por canal

                    int qtdePrecoVendido = 0; //qtde de ingressos vendidos com esse preço
                    int qtdeCanalVendido = 0; //qtde de ingressos vendidos com esse preço
                    int qtdePrecoVendidaPorCliente = 0; //qtde de ingressos vendidos com esse preço

                    int qtdeIngressosQPodeReservar = qtdeIngresso; //qtde de ingressos a ser reservado

                    int qtdeMaxIngressosQPodeReservarPorPreco = 0; //qtde maxima q pode reservar
                    int qtdeMaxIngressosQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal
                    int qtdeMaxIngressosQPodeReservarPorCliente = 0; //qtde maxima q pode reservar por cliente

                    int qtdeIngressosReservada = 0; //qtde de ingressos realmente reservada

                    //int[] qtdeDiffDisponivel = new int[2] { 0, 0 };

                    //int[] quantidadesMaximasCota = new int[2] { 0, 0 };

                    bool cotaExcedida = false;
                    //bool buscaFormaPagamentoCota = cotaItemID == 0 ? true : false;

                    int validadePreco; //dias q faltam para vender os ingressos (se maior que zero entao pode vender)

                    if (preReserva)
                    {
                        precoID = 0;
                        cortesiaID = 0;
                    }
                    else
                    {
                        //1. alimenta qtdes de preço 
                        Preco preco = new Preco();
                        Preco.InfoToReserva infoPreco = preco.LerToReserva(precoID, canalID, serieID, bd);

                        if (infoPreco.Preco == null || infoPreco.Preco.Control.ID == 0 || (infoPreco.CanalPrecoID == 0 && serieID == 0))
                        {
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Preço não está mais à venda nesse canal. Escolha outro preço.", CodMensagemReserva.PrecoIndisponivel);
                        }
                        else if (serieID > 0 && !infoPreco.DisponivelSerie)
                        {
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Preço não está mais à venda nesse canal ou para a série selecionada. Escolha outro preço.", CodMensagemReserva.PrecoIndisponivel);
                        }

                        qtdePrecoDisponivel = infoPreco.Preco.Quantidade.Valor;
                        qtdePrecoPorCliente = infoPreco.Preco.QuantidadePorCliente.Valor;
                        qtdePrecoVendido = infoPreco.QtdVendidoPreco;
                        qtdeCanalVendido = infoPreco.QtdVendidoCanal;

                        //2. verificar a qtde total de preço independente do canal
                        if (qtdePrecoDisponivel != 0)
                        {
                            //2.1 verificar qtde de ingressos vendidos a esse preço.
                            //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                            if (qtdePrecoVendido >= qtdePrecoDisponivel)
                            {
                                codMsg = CodMensagem.PorPreco;
                                alertaMsg = AlertaMensagem.Exclamacao;
                                throw new BilheteriaException("Esse preço não pode mais ser vendido.\nJá foi toda sua quota de " + qtdePrecoDisponivel + " ingressos!");
                            }

                            qtdeMaxIngressosQPodeReservarPorPreco = qtdePrecoDisponivel - qtdePrecoVendido;

                            if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorPreco)
                                qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorPreco;
                        }

                        //atribuir a qtde de preço ja vendida nao estourou qtde disponivel no canal
                        qtdePrecoPorCanal = infoPreco.QtdDisponivelCanal;	/// canalPreco.QuantidadeDisponivel();
                        //atribuir a data limite para venda dos ingressos
                        validadePreco = infoPreco.ValidadeCanal;	/// canalPreco.Validade();

                        if (validadePreco == 0)
                        {
                            //expirou a data!
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Data de validade para venda desse preço expirou!\n\nValidade: " + infoPreco.DataFim.ToShortDateString() + "\nHoje: " + System.DateTime.Now.ToShortDateString());
                        }

                        //3.1 se Quantidade for zero, esta liberado para vender a vontade!
                        if (qtdePrecoPorCanal != 0)
                        {
                            //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender ingresso(s)
                            if (qtdeCanalVendido >= qtdePrecoPorCanal)
                            {
                                codMsg = CodMensagem.PorCanal;
                                alertaMsg = AlertaMensagem.Exclamacao;
                                throw new BilheteriaException("Esse preço não está mais disponível no seu canal.\nJá foi vendida toda sua quota de                                        " + qtdePrecoPorCanal + " ingressos!");
                            }

                            qtdeMaxIngressosQPodeReservarPorCanal = qtdePrecoPorCanal - qtdePrecoVendido;

                            if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCanal)
                                qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCanal;
                        }

                        //4. verificar a qtde q pode vender por cliente independente do canal
                        if (qtdePrecoPorCliente != 0)
                        {
                            qtdeMaxIngressosQPodeReservarPorCliente = qtdePrecoPorCliente - qtdePrecoVendidaPorCliente;

                            if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCliente)
                                qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCliente;

                        }
                    }

                    #region Encontra CotaItem da Apresentacao
                    if (cotasInfo.TemCotaItem && !cotasInfo.EncontrouCotaItem)
                    {
                        DataTable dtt = new Cota().getDttCotaItemPorCotaID(cotasInfo.CotaID_Apresentacao);
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            //if (precoNome.StartsWith(dtt.Rows[i]["PrecoIniciaCom"].ToString()))
                            if (!precoNome.ToLower().StartsWith(dtt.Rows[i][PRECO_INICIA_COM].ToString().ToLower()))
                            {
                                cotasInfo.Preencher(false, dtt.Rows[i]);
                                cotasInfo.EncontrouCotaItem = true;
                                break;
                            }
                        }
                        retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);
                    }
                    else if (cotasInfo.CotaItemID > 0)
                        cotasInfo.EncontrouCotaItem = true;

                    #endregion

                    #region Encontra CotaItem da ApresentacaoSetor
                    if (cotasInfo.TemCotaItemAPS && !cotasInfo.EncontrouCotaItemAPS && cotasInfo.CotaID_Apresentacao != cotasInfo.CotaID_ApresentacaoSetor)
                    {
                        DataTable dtt = new Cota().getDttCotaItemPorCotaID(cotasInfo.CotaID_ApresentacaoSetor);
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            //if (precoNome.StartsWith(dtt.Rows[i]["PrecoIniciaCom"].ToString()))
                            if (!precoNome.ToLower().StartsWith(dtt.Rows[i][PRECO_INICIA_COM].ToString().ToLower()))
                            {
                                cotasInfo.Preencher(true, dtt.Rows[i]);
                                cotasInfo.EncontrouCotaItemAPS = true;
                                break;
                            }

                        }
                        if (cotasInfo.CotaItemID_APS != cotasInfo.CotaItemID)
                            retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);
                    }
                    else if (cotasInfo.CotaID_ApresentacaoSetor == cotasInfo.CotaID_Apresentacao)
                        cotasInfo.CotaItemID_APS = 0;
                    else if (cotasInfo.CotaItemID > 0)
                        cotasInfo.EncontrouCotaItemAPS = true;
                    #endregion


                    if (cotasInfo.CotaItemID > 0 && cotasInfo.CotaItemID_APS > 0 && cotasInfo.CotaItemID_APS != cotasInfo.CotaItemID
                        && (cotasInfo.ValidaBin != cotasInfo.ValidaBinAPS || cotasInfo.ParceiroID != cotasInfo.ParceiroIDAPS))
                    {
                        codMsg = CodMensagem.PorCota;
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Este preço possui mais de uma promoção cadastrada. Por favor, entre em contato com a equipe de atendimento.");
                    }

                    //Validacao de Cotas
                    if (cotasInfo.CotaItemID > 0 || cotasInfo.CotaItemID_APS > 0)
                    {
                        cotasInfo.QuantidadeJaVendida = new CotaItemControle().getQuantidadeNovo(cotasInfo.CotaItemID, cotasInfo.CotaItemID_APS, apresentacaoID, apresentacaoSetorID);
                        cotasInfo.sumQuantidadeVendidaReservar(qtdeJaReservadaCota, qtdeJaReservadaCotaItem);

                        //Fez a soma dos itens agora faz a validacao
                        while (!cotasInfo.ValidaReserva(qtdeIngressosQPodeReservar) && qtdeIngressosQPodeReservar > 0)
                        {
                            cotaExcedida = true;
                            qtdeIngressosQPodeReservar--;
                        }
                        if (qtdeIngressosQPodeReservar <= 0)
                        {
                            codMsg = CodMensagem.PorCota;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("O Limite de Venda/Reserva do preço " + precoNome + " foi excedido.");
                        }
                    }

                    Enumerators.TipoCodigoBarra tipoCodigoBarra = new Evento().BuscarTipoCodigoBarra(eventoID);

                    ids.Clear(); // Limpa, para não ter acumulo e quebra da coluna UNIQUE.
                    oIngresso.Processar += new IRLib.Paralela.Ingresso.ProcessHandler(oIngresso_ReservaPista);
                    oIngresso.ReservarPista(apresentacaoSetorID, qtdeIngressosQPodeReservar, bloqueioID, usuarioID, lojaID, precoID, cortesiaID, bd, false, eventoID, preReserva, tipoCodigoBarra);

                    ids.TrimToSize(); // Populado pelo no event
                    qtdeIngressosReservada += ids.Count;

                    if (ids.Count == 0)
                    { // Esgotado
                        alertaMsg = AlertaMensagem.Exclamacao;
                        if (bloqueioID == 0)
                            throw new BilheteriaException("Ingressos esgotados!\nAtualize a quantidade dos setores.");
                        else
                            throw new BilheteriaException("Bloqueios esgotados!\nClique em Buscar.");
                    }


                    DataTable reservados = oIngresso.InfoReservados((int[])ids.ToArray(typeof(int)), bd, lojaID);

                    int taxaConveniencia = 0;
                    int taxaComissao = 0;
                    decimal taxaMin = 0;
                    decimal taxaMax = 0;
                    decimal ComissaoMin = 0;
                    decimal ComissaoMax = 0;

                    // Se não for uma pré-reserva, alimentar as taxas e comissões
                    if (!preReserva)
                    {
                        CanalEvento canalEvento = new CanalEvento();

                        DataTable taxas = canalEvento.BuscaTaxasConvenienciaComissao(canalID, eventoID);

                        taxaConveniencia = (int)taxas.Rows[0]["TaxaConveniencia"];
                        taxaComissao = (int)taxas.Rows[0]["TaxaComissao"];
                        taxaMin = Convert.ToDecimal(taxas.Rows[0]["TaxaMinima"]);
                        taxaMax = Convert.ToDecimal(taxas.Rows[0]["TaxaMaxima"]);
                        ComissaoMin = Convert.ToDecimal(taxas.Rows[0]["ComissaoMinima"]);
                        ComissaoMax = Convert.ToDecimal(taxas.Rows[0]["ComissaoMaxima"]);
                    }


                    foreach (DataRow linha in reservados.Rows)
                    {
                        DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                        novoItemGrid[APRESENTACAOSETORID] = apresentacaoSetorID;
                        novoItemGrid[OBRIGA_CADASTRO_CLIENTE] = (string)linha["ObrigaCadastroCliente"];
                        novoItemGrid[EVENTO_PACOTE] = (string)linha["Evento"];
                        novoItemGrid[HORARIO] = (DateTime)linha["Horario"];
                        novoItemGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                        novoItemGrid[CODIGO] = (string)linha["Codigo"];
                        novoItemGrid[LUGAR_MARCADO] = false;
                        novoItemGrid[PRECO] = (string)linha["Preco"];
                        novoItemGrid[PRECOID] = precoID;
                        novoItemGrid[LOCALID] = linha["LocalID"];
                        //if (listaIngressos.Ingresso.Valor == 0) //eh cortesia
                        if (cortesiaID > 0) //eh cortesia
                            novoItemGrid[CORTESIA] = (string)linha["Cortesia"];
                        novoItemGrid[VALOR] = (decimal)linha["Valor"];
                        novoItemGrid[TIPO] = TIPO_INGRESSO;
                        novoItemGrid[TIPO_LUGAR] = Setor.Pista;
                        novoItemGrid[CONV] = taxaConveniencia;

                        if (cotasInfo.CotaItemID > 0 || cotasInfo.CotaItemID_APS > 0)
                        {
                            novoItemGrid[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                            novoItemGrid[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;
                            novoItemGrid[COTA] = "Sim";
                            novoItemGrid[CLIENTE] = " - ";
                        }
                        else
                        {
                            novoItemGrid[COTA_ITEM_ID] = 0;
                            novoItemGrid[COTA] = "Não";
                        }

                        if (serieID > 0)
                            novoItemGrid[SERIE] = "Sim";

                        //popula a taxa de conveniencia de acordo com a taxa minima e maxima
                        if (taxaConveniencia > 0 || taxaMin > 0)
                        {
                            decimal dAux = (taxaConveniencia / 100m) * (decimal)linha["Valor"];
                            if (dAux < taxaMin)
                            {
                                novoItemGrid[VALOR_CONV] = Decimal.Round(taxaMin, 2);
                                novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                linha["TaxaConveniencia"] = taxaConveniencia;
                                linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                            }
                            else if (dAux > taxaMax && taxaMax > 0)
                            {
                                novoItemGrid[VALOR_CONV] = Decimal.Round(taxaMax, 2);
                                novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                linha["TaxaConveniencia"] = taxaConveniencia;
                                linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                            }
                            else
                            {
                                novoItemGrid[VALOR_CONV] = Decimal.Round(dAux, 2);
                                novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                linha["TaxaConveniencia"] = taxaConveniencia;
                                linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                            }
                        }

                        //popula a comissao de acordo com a taxa minima e maxima
                        if (taxaComissao > 0 || ComissaoMin > 0)
                        {
                            decimal dAux = (taxaComissao / 100m) * (decimal)linha["Valor"];
                            if (dAux < ComissaoMin)
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Decimal.Round(ComissaoMin, 2);
                            }
                            else if (dAux > ComissaoMax && ComissaoMax > 0)
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Decimal.Round(ComissaoMax, 2);
                            }
                            else
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Decimal.Round(dAux, 2);
                            }
                        }

                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);

                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[TIPO] = TIPO_INGRESSO;
                        novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                        novoItemReserva[LUGARID] = 0;
                        novoItemReserva[PACOTEID] = 0;
                        novoItemReserva[PRECOID] = precoID;
                        novoItemReserva[CORTESIAID] = cortesiaID;
                        novoItemReserva[BLOQUEIOID] = bloqueioID;
                        novoItemReserva[INGRESSOID] = (int)linha["ID"];
                        novoItemReserva[APRESENTACAOID] = apresentacaoID;
                        novoItemReserva[APRESENTACAOSETORID] = apresentacaoSetorID;
                        novoItemReserva[EVENTOID] = (int)linha["EventoID"];
                        novoItemReserva[CODIGO_SEQUENCIAL] = (int)linha["CodigoSequencial"];
                        novoItemReserva[CLIENTE_ID] = 0;
                        novoItemReserva[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                        novoItemReserva[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;
                        novoItemReserva[QUANTIDADE_COTA] = cotasInfo.QuantidadeCota;
                        novoItemReserva[QUANTIDADEPORCLIENTE_COTA] = cotasInfo.QuantidadePorClienteCota;
                        novoItemReserva[QUANTIDADE_COTA_APS] = cotasInfo.QuantidadeCotaAPS;
                        novoItemReserva[QUANTIDADEPORCLIENTE_COTA_APS] = cotasInfo.QuantidadePorClienteCotaAPS;
                        novoItemReserva[QUANTIDADE_APRESENTACAO] = cotasInfo.QuantidadeApresentacao;
                        novoItemReserva[QUANTIDADE_APRESENTACAO_SETOR] = cotasInfo.QuantidadeApresentacaoSetor;
                        novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO] = cotasInfo.QuantidadePorClienteApresentacao;
                        novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = cotasInfo.QuantidadePorClienteApresentacaoSetor;
                        novoItemReserva[NOMINAL] = cotasInfo.Nominal;

                        if (cotasInfo.ValidaBin || cotasInfo.ValidaBinAPS)
                        {
                            novoItemReserva[VALIDA_BIN] = true;
                            novoItemReserva[VALIDA_BIN_APS] = true;
                        }
                        else
                        {
                            novoItemReserva[VALIDA_BIN] = false;
                            novoItemReserva[VALIDA_BIN_APS] = false;
                        }

                        GerenciamentoIngressos oGi = new GerenciamentoIngressos();
                        string horarioGerenciamento = gerenciamentoIngressosID > 0 ? oGi.BuscaHorario(gerenciamentoIngressosID) : "";

                        novoItemReserva[OBRIGATORIEDADE_ID] = (int)linha[OBRIGATORIEDADE_ID];
                        novoItemReserva[SPECIALEVENT] = eventoEspecial;
                        novoItemReserva[GERENCIAMENTO_INGRESSOS] = horarioGerenciamento;
                        novoItemReserva[GERENCIAMENTO_INGRESSOS_ID] = gerenciamentoIngressosID;
                        novoItemReserva[SERIEID] = serieID;
                        novoItemReserva[QUANTIDADE_POR_PROMOCIONAL] = quantidadePorPromocional;
                        novoItemReserva[ITEM_PROMOCIONAL] = itemPromocional;
                        novoItemReserva[TIPO_CODIGO_BARRA] = (string)linha[TIPO_CODIGO_BARRA];
                        novoItemReserva[CODIGO_BARRA] = linha[CODIGO_BARRA].ToString();

                        linha["GerenciamentoIngressos"] = horarioGerenciamento;
                        linha["GerenciamentoIngressosID"] = gerenciamentoIngressosID;

                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                        retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].ImportRow(linha);
                    }

                    if (cotasInfo.CotaItemID > 0 && cotasInfo.BuscaFormaPagamento)
                    {
                        DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(cotasInfo.CotaItemID, canalID);
                        retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);
                        cotasInfo.BuscaFormaPagamento = false;
                    }

                    if (cotasInfo.CotaItemID > 0 && cotasInfo.BuscaObrigatoriedade)
                    {
                        DataTable dttObrigatoriedade = new Obrigatoriedade().getInformacoesPorCotaItem(cotasInfo.CotaItemID);
                        retorno.Tables[TABELA_OBRIGATORIEDADE].Merge(dttObrigatoriedade);
                        cotasInfo.BuscaObrigatoriedade = false;
                    }

                    if (cotasInfo.CotaItemID_APS > 0 && cotasInfo.BuscaFormaPagamentoAPS && cotasInfo.CotaItemID_APS != cotasInfo.CotaItemID)
                    {
                        DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(cotasInfo.CotaItemID_APS, canalID);
                        retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);
                        cotasInfo.BuscaFormaPagamentoAPS = false;
                    }

                    if (cotasInfo.CotaItemID_APS > 0 && cotasInfo.BuscaObrigatoriedadeAPS && cotasInfo.CotaItemID_APS != cotasInfo.CotaItemID)
                    {
                        DataTable dttObrigatoriedade = new Obrigatoriedade().getInformacoesPorCotaItem(cotasInfo.CotaItemID_APS);
                        retorno.Tables[TABELA_OBRIGATORIEDADE].Merge(dttObrigatoriedade);
                        cotasInfo.BuscaObrigatoriedadeAPS = false;
                    }

                    if (qtdeIngressosReservada > 0 && buscaTaxasEntrega)
                    {
                        // Busca taxas de entrega do Evento
                        DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas(eventoID);
                        retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Merge(dtEventoTaxaEntrega);

                        // Busca regiões de entrega do Evento
                        DataTable dtEventoRegiao = new Regiao().GetRegioes(eventoID);
                        retorno.Tables[TABELA_EVENTO_REGIAO].Merge(dtEventoRegiao);

                        buscaTaxasEntrega = false;
                    }

                    if (qtdeIngressosReservada > 0 && buscaFormaPagamento)
                    {
                        // Busca formas de pagamento do Evento
                        DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento(eventoID, canalID);

                        retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Merge(dtEventoFormaPagamento);
                        buscaFormaPagamento = false;
                    }

                    string msg = null;

                    if (qtdeIngresso != qtdeIngressosReservada)
                    {

                        alertaMsg = AlertaMensagem.Exclamacao;

                        if (qtdeIngressosReservada == 0)
                        {
                            msg = "Ingressos esgotados!\nAtualize a quantidade dos setores.";
                        }
                        else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorPreco)
                        {
                            codMsg = CodMensagem.PorPreco;
                            msg = (qtdeIngressosReservada == 1) ?
                                "Somente um ingresso foi reservado!\nA quota de ingressos nesse preço já estourou." :
                                "Somente " + qtdeIngressosReservada + " ingressos foram reservados!\nA quota de ingressos nesse preço já estourou.";
                        }
                        else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            msg = (qtdeIngressosReservada == 1) ?
                                "Somente um ingresso foi reservado!\nA quota de ingressos no seu canal já estourou." :
                                "Somente " + qtdeIngressosReservada + " ingressos foram reservados!\nA quota de ingressos no seu canal já estourou.";
                        }
                        else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCliente)
                        {
                            codMsg = CodMensagem.PorCliente;
                            msg = (qtdeIngressosReservada == 1) ?
                                "Somente um ingresso foi reservado!\nA quota de ingressos por cliente já estourou." :
                                "Somente " + qtdeIngressosReservada + " ingressos foram reservados!\nA quota de ingressos por cliente já estourou.";
                        }
                        else if (cotaExcedida)
                        {
                            codMsg = CodMensagem.PorPreco;
                            msg = (qtdeIngressosReservada == 1) ?
                           "Somente um lugar foi reservado!\nO limite de ingressos para este preço foi atingido." :
                           "Somente " + qtdeIngressosReservada + " lugares foram reservados!\nO limite de ingressos para este preço foi atingido.";
                        }
                        else
                        {
                            msg = (qtdeIngressosReservada == 1) ?
                                "Somente um ingresso foi reservado!\nNão há mais ingressos disponíveis!" :
                                "Somente " + qtdeIngressosReservada + " ingressos foram reservados!\nNão há mais ingressos disponíveis!";
                        }
                    }
                    else
                    {
                        // emitir alerta caso...
                        alertaMsg = AlertaMensagem.Alerta;

                        if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorPreco)
                        {
                            codMsg = CodMensagem.PorPreco;
                            //msg = "A quota de ingressos nesse preço já atingiu sua quantidade máxima.";
                        }
                        else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            //msg = "A quota de ingressos no seu canal já atingiu sua quantidade máxima.";
                        }
                        else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCliente)
                        {
                            codMsg = CodMensagem.PorCliente;
                            //msg = "A quota de ingressos por cliente já atingiu sua quantidade máxima.";
                        }
                        else if (cotaExcedida)
                        {
                            codMsg = CodMensagem.PorCota;
                        }

                    }

                    if (alertaMsg != AlertaMensagem.Nulo)
                        retorno.Tables[TABELA_MENSAGEM].Rows.Add(new Object[] { alertaMsg, codMsg, msg, TIPO_INGRESSO });

                    if (qtdeIngressosReservada > 0 && buscaTaxasEntrega)
                    {
                        // Busca taxas de entrega do Evento
                        DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas(eventoID);

                        retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Merge(dtEventoTaxaEntrega);

                        // Busca regiões de entrega do Evento
                        DataTable dtEventoRegiao = new Regiao().GetRegioes(eventoID);

                        retorno.Tables[TABELA_EVENTO_REGIAO].Merge(dtEventoRegiao);
                    }

                    if (qtdeIngressosReservada > 0 && buscaFormaPagamento)
                    {
                        // Busca formas de pagamento do Evento
                        DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento(eventoID, canalID);

                        retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Merge(dtEventoFormaPagamento);
                    }
                }
                catch (BilheteriaException ex)
                {
                    retorno.Tables[TABELA_MENSAGEM].Rows.Add(new Object[] { alertaMsg, codMsg, ex.Message, TIPO_INGRESSO });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion

            //PACOTES
            #region //reservar os pacotes
            if (reservarPacote)
            {

                CodMensagem codMsg = CodMensagem.Nulo;
                AlertaMensagem alertaMsg = AlertaMensagem.Nulo;

                try
                {
                    List<int> lstCotaItemID = new List<int>();
                    //qtdes de pacote
                    int qtdePacoteDisponivel = 0; //qtde de pacotes disponiveis, q podem vender, independente do canal
                    int qtdePacotePorCanal = 0; //qtde de pacotes q podem vender por canal
                    int qtdePacoteVendido = 0; //qtde de pacotes vendidos
                    int qtdePacoteQPodeReservar = qtdePacote; //qtde de pacotes a ser reservado

                    int qtdeMaxPacoteQPodeReservar = 0; //qtde maxima q pode reservar
                    int qtdeMaxPacoteQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal

                    int qtdePacoteReservada = 0; //qtde de ingressos realmente reservada

                    Pacote pacote = new Pacote(usuarioID);


                    bool PodeComprar = pacote.VetificaQuantidadePreco(pacoteID, canalID, qtdePacote, bd);

                    if (!PodeComprar)
                    {
                        codMsg = CodMensagem.PorPreco;
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Esse preço não pode ser vendido.\nVerifique a cota de preço do ingressos!");
                    }


                    //1. obter a Quantidade desse pacote

                    Pacote.InfoToReserva infoPacote = pacote.LerToReserva(pacoteID, canalID, bd);

                    if (infoPacote.Pacote.Control.ID == 0)
                    {
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Pacote não existe.");
                    }

                    qtdePacoteDisponivel = infoPacote.Pacote.Quantidade.Valor;
                    qtdePacotePorCanal = infoPacote.QtdDisponivelCanal;
                    qtdePacoteVendido = infoPacote.QtdVendido;

                    //2. verificar a qtde total de pacote independente do canal
                    if (qtdePacoteDisponivel != 0)
                    {
                        //2.1 verificar qtde de pacotes associados a esse preço.
                        //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                        if (qtdePacoteVendido >= qtdePacoteDisponivel)
                        {
                            codMsg = CodMensagem.Disponivel;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Esse pacote não pode mais ser vendido.\nSua quota de " + qtdePacoteDisponivel + " pacotes já foi vendida!");
                        }

                        qtdeMaxPacoteQPodeReservar = qtdePacoteDisponivel - qtdePacoteVendido;

                        if (qtdePacoteQPodeReservar > qtdeMaxPacoteQPodeReservar)
                            qtdePacoteQPodeReservar = qtdeMaxPacoteQPodeReservar;
                    }

                    //se Quantidade for zero, esta liberado para vender a vontade!
                    if (qtdePacotePorCanal != 0)
                    {
                        //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender
                        if (qtdePacoteVendido >= qtdePacotePorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Esse pacote não pode mais ser vendido no seu canal.\nSua quota de " + qtdePacotePorCanal + " pacotes pra esse canal já foi vendida!");
                        }

                        qtdeMaxPacoteQPodeReservarPorCanal = qtdePacotePorCanal - qtdePacoteVendido;

                        if (qtdePacoteQPodeReservar > qtdeMaxPacoteQPodeReservarPorCanal)
                            qtdePacoteQPodeReservar = qtdeMaxPacoteQPodeReservarPorCanal;
                    }

                    int taxaConvenienciaPacote = 0;
                    decimal taxaMinPacote = 0;
                    decimal taxaMaxPacote = 0;
                    int taxaComissaoPacote = 0;
                    decimal comissaoMinPacote = 0;
                    decimal comissaoMaxPacote = 0;
                    decimal comissaoValorTotal = 0;
                    decimal taxaValorTotal = 0;
                    decimal valorPacote = pacote.Valor(bd); //obtem valor total do pacote

                    // Quando é pré-reserva não deve considerar taxas e comissões
                    if (!preReserva)
                    {
                        //int taxaConvenienciaPacote = -1; //ver acima
                        CanalPacote canalPacote = new CanalPacote();

                        //busca as taxas de conveniencia e comissão e popula as variaveis
                        DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(canalID, pacoteID);

                        taxaConvenienciaPacote = (int)taxasPacote.Rows[0]["TaxaConveniencia"];
                        taxaMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMinima"]);
                        taxaMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMaxima"]);
                        taxaComissaoPacote = (int)taxasPacote.Rows[0]["TaxaComissao"];
                        comissaoMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMinima"]);
                        comissaoMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMaxima"]);
                        comissaoValorTotal = 0;
                        taxaValorTotal = 0;
                    }

                    DataTable itensPacote = pacote.ItensParaReserva(bd); //metodo mais lento

                    while (qtdePacoteReservada < qtdePacoteQPodeReservar)
                    {
                        List<EstruturaIngressoCotaItem> lstIngressoCotaItem = new List<EstruturaIngressoCotaItem>();
                        int[] ingressosIDs = pacote.Reservar(bd, itensPacote, ref lstIngressoCotaItem, lojaID); //metodo mais lento
                        if (ingressosIDs == null)
                            throw new BilheteriaException("Pacote esgotado!");
                        DataTable tabela = pacote.InfoReservado(ingressosIDs, bd, lstIngressoCotaItem, lojaID);

                        comissaoValorTotal = taxaValorTotal = 0;

                        //popula a comissão de acordo com a taxa minima e maxima
                        if (taxaComissaoPacote > 0 || comissaoMinPacote > 0)
                        {
                            decimal dAux = (taxaComissaoPacote / 100m) * valorPacote;

                            if (dAux < comissaoMinPacote)
                            {
                                comissaoValorTotal += Decimal.Round(comissaoMinPacote, 2);
                            }
                            else if (dAux > comissaoMaxPacote && comissaoMaxPacote > 0)
                            {
                                comissaoValorTotal += Decimal.Round(comissaoMaxPacote, 2);
                            }
                            else
                            {
                                comissaoValorTotal += Decimal.Round(dAux, 2);
                            }
                        }
                        //popula a conveniencia de acordo com a taxa minima e maxima
                        if (taxaConvenienciaPacote > 0 || taxaMinPacote > 0)
                        {
                            decimal dAux = (taxaConvenienciaPacote / 100m) * valorPacote;

                            if (dAux < taxaMinPacote)
                            {
                                taxaValorTotal += Decimal.Round(taxaMinPacote, 2);
                            }
                            else if (dAux > taxaMaxPacote && taxaMaxPacote > 0)
                            {
                                taxaValorTotal += Decimal.Round(taxaMaxPacote, 2);
                            }
                            else
                            {
                                taxaValorTotal += Decimal.Round(dAux, 2);
                            }
                        }


                        if (ingressosIDs != null)
                        {

                            //adicionar ingressos


                            DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();
                            novoItemGrid[PRECOID] = 0;
                            novoItemGrid[APRESENTACAOSETORID] = 0;
                            novoItemGrid[EVENTO_PACOTE] = pacote.Nome.Valor;
                            novoItemGrid[VALOR] = valorPacote;
                            novoItemGrid[LUGAR_MARCADO] = false;
                            novoItemGrid[TIPO] = TIPO_PACOTE;
                            novoItemGrid[TIPO_LUGAR] = Setor.Pista; //aki eh sempre pista p/ pacote
                            novoItemGrid[CONV] = taxaConvenienciaPacote;
                            novoItemGrid[CONV_MIN] = taxaMinPacote;
                            novoItemGrid[CONV_MAX] = taxaMaxPacote;
                            novoItemGrid[TAXA_COMISSAO] = taxaComissaoPacote;
                            novoItemGrid[PERMITIR_CANCELAMENTO_AVULSO] = pacote.PermitirCancelamentoAvulso.Valor;

                            novoItemGrid[COMISSAO_VALOR] = Decimal.Round(comissaoValorTotal, 2);
                            novoItemGrid[VALOR_CONV] = decimal.Round(taxaValorTotal, 2);


                            bool temCota = false;
                            for (int i = 0; i < lstIngressoCotaItem.Count; i++)
                            {
                                if (lstIngressoCotaItem[i].CotaItemID > 0)
                                {
                                    temCota = true;
                                    if (!lstCotaItemID.Contains(lstIngressoCotaItem[i].CotaItemID))
                                        lstCotaItemID.Add(lstIngressoCotaItem[i].CotaItemID);
                                }
                            }

                            if (temCota)
                            {
                                novoItemGrid[COTA] = "Sim";
                                novoItemGrid[CLIENTE] = " - ";
                            }
                            else
                                novoItemGrid[COTA] = "Não";

                            retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);



                            foreach (DataRow linha in tabela.Rows)
                            {
                                DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();
                                novoItemReserva[TIPO] = TIPO_PACOTE;
                                novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                                novoItemReserva[LUGARID] = 0;
                                novoItemReserva[PACOTEID] = pacoteID;
                                novoItemReserva[PRECOID] = (int)linha["PrecoID"];
                                novoItemReserva[CORTESIAID] = (int)linha["CortesiaID"];
                                novoItemReserva[BLOQUEIOID] = (int)linha["BloqueioID"];
                                novoItemReserva[INGRESSOID] = (int)linha["ID"];
                                novoItemReserva[APRESENTACAOID] = (int)linha[APRESENTACAOID];
                                novoItemReserva[APRESENTACAOSETORID] = (int)linha[APRESENTACAOSETORID];
                                novoItemReserva[EVENTOID] = (int)linha["EventoID"];
                                novoItemReserva[CODIGO_SEQUENCIAL] = (int)linha["CodigoSequencial"];
                                novoItemReserva[OBRIGATORIEDADE_ID] = (int)linha["ObrigatoriedadeID"];
                                novoItemReserva[COTA_ITEM_ID] = (int)linha[COTA_ITEM_ID];

                                novoItemReserva[QUANTIDADE_COTA] = (int)linha[QUANTIDADE_COTA];
                                novoItemReserva[QUANTIDADEPORCLIENTE_COTA] = (int)linha[QUANTIDADEPORCLIENTE_COTA];

                                novoItemReserva[QUANTIDADE_APRESENTACAO] = (int)linha[QUANTIDADE_APRESENTACAO];
                                novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO] = (int)linha[QUANTIDADEPORCLIENTE_APRESENTACAO];

                                novoItemReserva[QUANTIDADE_APRESENTACAO_SETOR] = (int)linha[QUANTIDADE_APRESENTACAO_SETOR];
                                novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = (int)linha[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR];

                                novoItemReserva[VALIDA_BIN] = (bool)linha[VALIDA_BIN];
                                novoItemReserva[NOMINAL] = (bool)linha[NOMINAL];
                                novoItemReserva[TIPO_CODIGO_BARRA] = linha[TIPO_CODIGO_BARRA].ToString();
                                novoItemReserva[CODIGO_BARRA] = linha[CODIGO_BARRA].ToString();
                                retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);

                                //popula a taxa de acordo com a taxa minima e maxima
                                if (taxaConvenienciaPacote > 0 || taxaMinPacote > 0)
                                {
                                    decimal dAux = (taxaConvenienciaPacote / 100m) * (valorPacote / tabela.Rows.Count);

                                    if (dAux < taxaMinPacote)
                                    {
                                        linha["TaxaConveniencia"] = taxaConvenienciaPacote;
                                        linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMinPacote, 2);
                                    }
                                    else if (dAux > taxaMaxPacote && taxaMaxPacote > 0)
                                    {

                                        linha["TaxaConveniencia"] = taxaConvenienciaPacote;
                                        linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMaxPacote, 2);
                                    }
                                    else
                                    {
                                        linha["TaxaConveniencia"] = taxaConvenienciaPacote;
                                        linha["TaxaConvenienciaValor"] = Decimal.Round(dAux, 2);
                                    }
                                }

                                if (buscaTaxasEntrega)
                                {
                                    if (retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Select("EventoID = " + (int)linha["EventoID"]).Length == 0)
                                    {
                                        // Busca taxas de entrega do Evento
                                        DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas((int)linha["EventoID"]);
                                        retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Merge(dtEventoTaxaEntrega);

                                        // Busca regiões de entrega do Evento
                                        DataTable dtEventoRegiao = new Regiao().GetRegioes((int)linha["EventoID"]);
                                        retorno.Tables[TABELA_EVENTO_REGIAO].Merge(dtEventoRegiao);

                                    }


                                }

                                if (buscaFormaPagamento)
                                {
                                    if (retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Select("EventoID = " + (int)linha["EventoID"]).Length == 0)
                                    {
                                        // Busca formas de pagamento do Evento
                                        DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento((int)linha["EventoID"], canalID);
                                        retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Merge(dtEventoFormaPagamento);
                                    }
                                }

                                retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].ImportRow(linha);

                            }

                            qtdePacoteReservada++;

                        }
                        else
                        {
                            break;
                        }

                    } //fim while


                    if (lstCotaItemID.Count > 0)
                    {
                        DataTable dtt = new CotaItem().getDttCotaItemPorCotaID(lstCotaItemID.ToArray());
                        retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);

                        DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(lstCotaItemID.ToArray());
                        retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);

                        DataTable dttObrigatoriedade = new Obrigatoriedade().getInformacoesPorCotaItem(lstCotaItemID.ToArray());
                        retorno.Tables[TABELA_OBRIGATORIEDADE].Merge(dttObrigatoriedade);
                    }

                    string msg = null;

                    if (qtdePacote != qtdePacoteReservada)
                    {

                        alertaMsg = AlertaMensagem.Exclamacao;

                        if (qtdePacoteReservada == 0)
                        {
                            msg = "Ingressos do pacote indisponíveis ou esgotados!\nAtualize a quantidade dos setores.";
                        }
                        else if (qtdePacoteReservada == qtdeMaxPacoteQPodeReservar)
                        {
                            codMsg = CodMensagem.Disponivel;
                            msg = (qtdePacoteReservada == 1) ?
                                "Somente um pacote foi reservado!\nA quota de pacotes já estourou." :
                                "Somente " + qtdePacoteReservada + " pacotes foram reservados!\nA quota de pacotes já estourou.";
                        }
                        else if (qtdePacoteReservada == qtdeMaxPacoteQPodeReservarPorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            msg = (qtdePacoteReservada == 1) ?
                                "Somente um pacote foi reservado!\nA quota de pacotes no seu canal já estourou." :
                                "Somente " + qtdePacoteReservada + " pacotes foram reservados!\nA quota de pacotes nesse canal já estourou.";
                        }
                        else
                        {
                            msg = (qtdePacoteReservada == 1) ?
                                "Somente um pacote foi reservado!\nNão há mais pacotes disponíveis!" :
                                "Somente " + qtdePacoteReservada + " pacotes foram reservados!\nNão há mais pacotes disponíveis!";
                        }

                    }
                    else
                    { //emitir alerta caso...

                        alertaMsg = AlertaMensagem.Alerta;

                        if (qtdePacoteReservada == qtdeMaxPacoteQPodeReservar)
                        {
                            codMsg = CodMensagem.Disponivel;
                            //msg = "A quota de pacotes já atingiu sua quantidade máxima.";
                        }
                        else if (qtdePacoteReservada == qtdeMaxPacoteQPodeReservarPorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            //msg = "A quota de pacotes no seu canal já atingiu sua quantidade máxima.";
                        }

                    }

                    if (alertaMsg != AlertaMensagem.Nulo)
                        retorno.Tables[TABELA_MENSAGEM].Rows.Add(new Object[] { alertaMsg, codMsg, msg, TIPO_PACOTE });

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            #endregion


            return retorno;
        }

        public List<Ingresso> ReservarAssinaturaInternet(int pacoteID, int lugarID, string sessionID, int clienteID, EstruturaReservaInternet estruturaReservaInternet)
        {
            BD bd = new BD();
            try
            {

                CanalPacote oCanalPacote = new CanalPacote();

                Pacote oPacote = new Pacote(estruturaReservaInternet.UsuarioID);

                bool PodeComprar = oPacote.VetificaQuantidadePreco(pacoteID, estruturaReservaInternet.CanalID, 1, bd);

                if (!PodeComprar)
                    throw new BilheteriaException("Esse preço não pode ser vendido.\nVerifique a cota de preço do ingressos!");


                oPacote.Control.ID = pacoteID;
                BilheteriaParalela bilheteria = new BilheteriaParalela();
                decimal pctTaxa = bilheteria.TaxaConvenienciaPacote(pacoteID, estruturaReservaInternet.CanalID);

                DataTable itensPacote = oPacote.ItensParaReserva(bd);

                return oPacote.ReservarAssinaturaInternet(bd, itensPacote, estruturaReservaInternet.UsuarioID, sessionID, clienteID, estruturaReservaInternet.LojaID, 1, pctTaxa, lugarID);

            }
            catch { throw; }
            finally
            {
                bd.Fechar();
            }
        }

        public void ReservarInternetLugarMarcado(ref Ingresso ingresso, EstruturaPrecoReservaSite preco, int serieID, EstruturaReservaInternet estruturaReservaInternet)
        {
            try
            {
                BilheteriaParalela bilheteria = new BilheteriaParalela();

                int apresentacaoSetorID = 0;
                Setor.enumLugarMarcado tipoSetor = new Setor.enumLugarMarcado();
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();
                BD bd = new BD();
                //Busca a ApresentacaoSetorID, já aproveita e busca o tipo do setor, Cota ID da Apresentacao e da ApresentacaoSetor
                string sqlProc = "EXEC sp_getApresentacaoInfo " + ingresso.ApresentacaoID.Valor + ", " + ingresso.SetorID.Valor;

                bd.Consulta(sqlProc);
                if (bd.Consulta().Read())
                {
                    apresentacaoSetorID = bd.LerInt("ID");
                    tipoSetor = (Setor.enumLugarMarcado)(Convert.ToChar(bd.LerString("LugarMarcado")));
                    cotaInfo.CotaID_Apresentacao = bd.LerInt("ApresentacaoCotaID");
                    cotaInfo.CotaID_ApresentacaoSetor = bd.LerInt("ApresentacaoSetorCotaID");
                    cotaInfo.QuantidadeApresentacao = bd.LerInt("QuantidadeApresentacao");
                    cotaInfo.QuantidadeApresentacaoSetor = bd.LerInt("QuantidadeApresentacaoSetor");
                    cotaInfo.QuantidadePorClienteApresentacao = bd.LerInt("QuantidadePorClienteApresentacao");
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = bd.LerInt("QuantidadePorClienteApresentacaoSetor");
                }
                bd.FecharConsulta();

                //verifica se achou a ApresentacaoID
                if (apresentacaoSetorID == 0)
                    throw new Exception("Falha ao buscar a apresentação");

                ingresso.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                ingresso.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                ingresso.LojaID.Valor = estruturaReservaInternet.LojaID;
                ingresso.SerieID.Valor = serieID;
                ingresso.TimeStampReserva.Valor = DateTime.Now.AddMinutes(new ConfigGerenciadorParalela().getValorTempoReserva());

                ingresso.ValidarLugarMarcadoInternet(preco, tipoSetor, cotaInfo, serieID, estruturaReservaInternet);

                ingresso.Reservar(bd, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReservarInternetMesa(ref List<Ingresso> ingressos, EstruturaPrecoReservaSite preco, EstruturaReservaInternet estruturaReserva)
        {
            BD bd = new BD();
            try
            {
                BilheteriaParalela bilheteria = new BilheteriaParalela();

                int apresentacaoSetorID = 0;
                //Setor.enumLugarMarcado tipoSetor = Setor.enumLugarMarcado.MesaFechada
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();

                //Busca a ApresentacaoSetorID, já aproveita e busca o tipo do setor, Cota ID da Apresentacao e da ApresentacaoSetor
                string sqlProc = "EXEC sp_getApresentacaoInfo " + ingressos[0].ApresentacaoID.Valor + ", " + ingressos[0].SetorID.Valor;

                bd.Consulta(sqlProc);
                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar as informações desta apresentação");

                apresentacaoSetorID = bd.LerInt("ID");
                Setor.enumLugarMarcado tipoSetor = (Setor.enumLugarMarcado)(Convert.ToChar(bd.LerString("LugarMarcado")));
                cotaInfo.CotaID_Apresentacao = bd.LerInt("ApresentacaoCotaID");
                cotaInfo.CotaID_ApresentacaoSetor = bd.LerInt("ApresentacaoSetorCotaID");
                cotaInfo.QuantidadeApresentacao = bd.LerInt("QuantidadeApresentacao");
                cotaInfo.QuantidadeApresentacaoSetor = bd.LerInt("QuantidadeApresentacaoSetor");
                cotaInfo.QuantidadePorClienteApresentacao = bd.LerInt("QuantidadePorClienteApresentacao");
                cotaInfo.QuantidadePorClienteApresentacaoSetor = bd.LerInt("QuantidadePorClienteApresentacaoSetor");

                bd.FecharConsulta();

                //verifica se achou a ApresentacaoID
                if (apresentacaoSetorID == 0)
                    throw new Exception("Falha ao buscar a apresentação");

                ingressos[0].ApresentacaoSetorID.Valor = apresentacaoSetorID;
                ingressos[0].UsuarioID.Valor = estruturaReserva.UsuarioID;
                ingressos[0].LojaID.Valor = estruturaReserva.LojaID;
                ingressos[0].ValidarLugarMarcadoInternet(preco, tipoSetor, cotaInfo, 0, estruturaReserva);

                bd.IniciarTransacao();
                foreach (Ingresso ingresso in ingressos)
                {
                    ingresso.CotaItemID = ingressos[0].CotaItemID;
                    ingresso.CotaItemIDAPS = ingressos[0].CotaItemIDAPS;


                    ingresso.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                    ingresso.UsuarioID.Valor = estruturaReserva.UsuarioID;
                    ingresso.LojaID.Valor = estruturaReserva.LojaID;
                    ingresso.PrecoID.Valor = ingressos[0].PrecoID.Valor;
                    ingresso.TxConv = ingressos[0].TxConv;

                    if (!ingresso.Reservar(bd, false))
                        throw new Exception("Não foi possível reservar a mesa.");
                }

                Ingresso.AumentarTempoReservasInternet(ingressos[0].ClienteID.Valor, ingressos[0].SessionID.Valor);

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

        public List<Ingresso> ReservarInternet(string sessionID, int clienteID, int apresentacaoID,
            int setorID, List<EstruturaPrecoReservaSite> precos, int eventoID, int serieID, EstruturaReservaInternet estruturaReserva)
        {
            List<Ingresso> retorno = new List<Ingresso>();
            List<Ingresso> melhoresIngressos = new List<Ingresso>();
            try
            {
                BilheteriaParalela bilheteria = new BilheteriaParalela();
                Ingresso oIngresso = new Ingresso();


                int apresentacaoSetorID = 0;
                Setor.enumLugarMarcado tipoSetor = new Setor.enumLugarMarcado();
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();
                BD bd = new BD();
                //Busca a ApresentacaoSetorID, já aproveita e busca o tipo do setor, Cota ID da Apresentacao e da ApresentacaoSetor
                string sqlProc = "EXEC sp_getApresentacaoInfo " + apresentacaoID + ", " + setorID;

                bd.Consulta(sqlProc);
                if (bd.Consulta().Read())
                {
                    apresentacaoSetorID = bd.LerInt("ID");
                    tipoSetor = (Setor.enumLugarMarcado)(Convert.ToChar(bd.LerString("LugarMarcado")));
                    cotaInfo.CotaID_Apresentacao = bd.LerInt("ApresentacaoCotaID");
                    cotaInfo.CotaID_ApresentacaoSetor = bd.LerInt("ApresentacaoSetorCotaID");
                    cotaInfo.QuantidadeApresentacao = bd.LerInt("QuantidadeApresentacao");
                    cotaInfo.QuantidadeApresentacaoSetor = bd.LerInt("QuantidadeApresentacaoSetor");
                    cotaInfo.QuantidadePorClienteApresentacao = bd.LerInt("QuantidadePorClienteApresentacao");
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = bd.LerInt("QuantidadePorClienteApresentacaoSetor");
                }
                bd.FecharConsulta();
                //verifica se achou a ApresentacaoID
                if (apresentacaoSetorID == 0)
                    throw new Exception("Falha ao buscar a apresentação");

                //Busca os melhores ingressos disponíveis de: Pista, Mesa Aberta, Mesa Fechada e Cadeira.
                melhoresIngressos = oIngresso.MelhoresIngressos(estruturaReserva.UsuarioID, estruturaReserva.LojaID, sessionID, clienteID,
                    eventoID, apresentacaoID, apresentacaoSetorID, setorID, precos, tipoSetor, cotaInfo, serieID, estruturaReserva);
                //Tenta reservar os ingressos contanto que nao seja CORTESIA_SEM_CONVENIENCIA
                foreach (Ingresso ingresso in melhoresIngressos)
                {
                    if (ingresso.ReservarInternet(bd, false))
                        retorno.Add(ingresso);
                    else
                        throw new Exception("Não foi possível reservar o ingresso, por favor tente novamente");
                }
            }
            catch
            {
                throw;
            }
            return retorno;

        }

        public Ingresso[] ReservarPacoteInternet(int pacoteID, int quantidade, string sessionID, int clienteID, EstruturaReservaInternet estruturaReservaInternet)
        {
            /// 1) Buscar os itens do pacote.
            /// 2) Reservar pacote agrupado por apresentacao.
            /// 3) Retornar os ingressos reservados.
            /// 

            BD bd = new BD();

            try
            {
                CanalPacote oCanalPacote = new CanalPacote();

                Pacote oPacote = new Pacote(estruturaReservaInternet.UsuarioID);
                oPacote.Control.ID = pacoteID;

                bool PodeComprar = oPacote.VetificaQuantidadePreco(pacoteID, estruturaReservaInternet.CanalID, quantidade, bd);

                if (!PodeComprar)
                    throw new BilheteriaException("Esse preço não pode ser vendido.\nVerifique a cota de preço do ingressos!");


                BilheteriaParalela bilheteria = new BilheteriaParalela();
                decimal pctTaxa = bilheteria.TaxaConvenienciaPacote(pacoteID, estruturaReservaInternet.CanalID);

                DataTable itensPacote = oPacote.ItensParaReserva(bd);

                return oPacote.ReservarInternet(bd, itensPacote, estruturaReservaInternet.UsuarioID, sessionID, clienteID, estruturaReservaInternet.LojaID, quantidade, pctTaxa);

            }
            catch { throw; }
            finally
            {
                bd.Fechar();
            }
        }

        public Ingresso[] MelhorCadeira(int usuarioID, int lojaID, string sessionID, int clienteID, int eventoID, int apresentacaoSetorID, int[,] precos, bool preReserva, int canalID)
        {
            BD bd = new BD();
            BD bdReserva = new BD();
            CodigoBarra oCodigoBarra = new CodigoBarra();
            Ingresso oIngresso = new Ingresso();

            try
            {
                int qtd = 0;
                List<IRLib.Paralela.ClientObjects.EstruturaPrecoIDValor> preco = new List<IRLib.Paralela.ClientObjects.EstruturaPrecoIDValor>();


                decimal valorPreco = -1;
                for (int i = 0; i < (precos.Length / 2); i++)
                {
                    // verifica se existe quantidade do PrecoID disponivel para venda
                    // if(PrecoQtde  != IngressosQPodeReservar) n reserva nada
                    // Se for pré-reserva ignorar o preço
                    if (!preReserva)
                        if (precos[i, 1] != GetIngressosQPodeReservar(clienteID, precos[i, 0], precos[i, 1], ref valorPreco, canalID))
                            throw new BilheteriaException("Preço Indisponível", CodMensagemReserva.PrecoIndisponivel);

                    qtd += precos[i, 1];
                    IRLib.Paralela.ClientObjects.EstruturaPrecoIDValor precoAux;
                    for (int x = 0; x < precos[i, 1]; x++)
                    {
                        precoAux = new IRLib.Paralela.ClientObjects.EstruturaPrecoIDValor();

                        // Se for pré-reserva ignorar o preço
                        if (preReserva)
                        {
                            precoAux.PrecoID = 0;
                            precoAux.Valor = 0;
                        }
                        else
                        {
                            precoAux.PrecoID = precos[i, 0];
                            precoAux.Valor = valorPreco;
                        }

                        preco.Add(precoAux);
                    }


                }

                /* 
                 * Busca os grupos que possuem mais do que X ingressos.
                 *
                */
                string sql = "SELECT " +
                    "Grupo, COUNT(ID) Quantidade " +
                    "FROM tIngresso (NOLOCK) " +
                    "WHERE  " +
                    "ApresentacaoSetorID = " + apresentacaoSetorID + " AND  " +
                    "Grupo > 0 AND  " +
                    "Classificacao > 0 " +
                    "AND Status = 'D' " +
                    "GROUP BY Grupo " +
                    "HAVING COUNT(ID) >=  " + qtd +
                    " ORDER BY Grupo, Quantidade";

                bd.Consulta(sql);
                ArrayList grupos = new ArrayList();
                while (bd.Consulta().Read())
                    grupos.Add(bd.LerInt("Grupo"));
                bd.FecharConsulta();

                ArrayList ingressos = new ArrayList();

                /// Vai do primeiro ao último grupo.
                //foreach(DictionaryEntry item in grupos)
                int grupo;
                for (int i = 0; i < grupos.Count && ingressos.Count < qtd; i++)
                {
                    grupo = (int)grupos[i];
                    // Busca os lugares do grupo em questão.
                    bd.Consulta("SELECT ID, Codigo, Classificacao, LugarID FROM tIngresso (NOLOCK) " +
                        "WHERE " +
                        "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                        "Grupo = " + grupo + " AND " +
                        "Classificacao > 0 " +
                        "AND Status = '" + Ingresso.DISPONIVEL + "' " +
                        "ORDER BY Classificacao");

                    while (bd.Consulta().Read() && ingressos.Count < qtd)
                    {

                        /// Verifica se a classificação do item anterior + 1 é diferente da atual.
                        /// _____
                        /// 1
                        /// 2
                        /// 3
                        /// -------
                        /// 3 + 1 != da classificacao atual?!
                        ///		Classificação Atual = 4 = OK
                        ///		Classificação Atual = 5 = ERRADO.
                        ///

                        if (ingressos.Count > 0 && ((Ingresso)ingressos[ingressos.Count - 1]).Classificacao.Valor + 1 != bd.LerInt("Classificacao"))
                            ingressos.Clear();

                        ingressos.Add(new Ingresso(bd.LerInt("ID"), bd.LerString("Codigo"), grupo, bd.LerInt("Classificacao"), bd.LerInt("LugarID")));
                    }

                    if (ingressos.Count != qtd) // Não encontrou a qtd suficiente?!
                        ingressos.Clear(); // Limpa os ingressos e passa para a próxima.
                }
                bd.FecharConsulta();

                ArrayList ingressoRetorno = new ArrayList();
                bdReserva.IniciarTransacao();

                try
                {
                    int i = 0;

                    foreach (Ingresso ing in ingressos)
                    {
                        decimal valorConv = this.TaxaConveniencia(eventoID, preco[i].PrecoID, canalID);

                        oIngresso = new Ingresso();
                        oIngresso.Control.ID = ing.Control.ID;
                        oIngresso.PrecoID.Valor = preco[i].PrecoID;
                        decimal valor = preco[i].Valor;
                        oIngresso.UsuarioID.Valor = usuarioID;
                        oIngresso.Codigo.Valor = ing.Codigo.Valor;
                        //oIngresso.CodigoBarra.Valor = oCodigoBarra.GerarCodigoBarra(precoID[i], eventoID);
                        oIngresso.LojaID.Valor = lojaID;
                        oIngresso.ClienteID.Valor = clienteID;
                        oIngresso.SessionID.Valor = sessionID;
                        oIngresso.TimeStampReserva.Valor = DateTime.Now;
                        oIngresso.LugarID.Valor = ing.LugarID.Valor;
                        oIngresso.TxConv = valorConv;
                        oIngresso.Grupo.Valor = ing.Grupo.Valor;
                        oIngresso.Classificacao.Valor = ing.Classificacao.Valor;

                        //se não tiver valor e não tiver conveniencia não deve reservar
                        if (valor == 0 && oIngresso.TxConv == 0)
                        {
                            // Atribui a Cortesia Padrão do Evento/Local - INICIO
                            Cortesia oCortesia = new Cortesia();
                            int CortesiaPadraoID = oCortesia.CortesiaPadraoEvento(eventoID);
                            if (CortesiaPadraoID == 0)
                                throw new Exception("Não foi possível reservar o ingresso. Por favor, tente novamente mais tarde.");
                            oCortesia = null;
                            oIngresso.CortesiaID.Valor = CortesiaPadraoID;
                            // Atribui a Cortesia Padrão do Evento/Local - FIM

                            oIngresso.Status.Valor = Ingresso.CORTESIA_SEM_CONVENIENCIA;
                            ingressoRetorno.Add(oIngresso);
                            break;//break para inserir somente um registro. esse registro de ingresso vai ser utilizado 
                            //como base de info para deletar o preço inválido do banco de dados do site.
                        }
                        else
                        {
                            if (oIngresso.Reservar(bdReserva, preReserva))
                                ingressoRetorno.Add(oIngresso);
                        }

                        i++;
                    }

                    bdReserva.FinalizarTransacao();
                    bd.Fechar();

                    // se algum ingresso foi reservado aumenta o TimeStampReserva de todos ingressos da sessão do cliente
                    if (ingressoRetorno.Count > 0)
                        Ingresso.AumentarTempoReservasInternet(clienteID, sessionID);

                }
                catch (Exception ex)
                {
                    bdReserva.DesfazerTransacao();
                    bdReserva.Fechar();
                    throw ex;
                }
                finally
                {
                    bd.Fechar();
                    bdReserva.Fechar();
                }

                return (Ingresso[])ingressoRetorno.ToArray(typeof(Ingresso));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bdReserva.Fechar();
                bd.Fechar();
            }
        }

        /// <summary>
        /// Verifica se os melhores lugares estao corretos
        /// </summary>
        /// <param name="lugares">tabela com os melhores lugares</param>
        /// <returns></returns>
        private int verificarMelhorLugar(DataTable lugares, int apresentacaoSetorID)
        {
            BD bd = new BD();

            int retorno = 0;
            // -1 = ok
            // -2 = nao ok
            // >0 = codigo do ultimo grupo

            bool agrupados = false;
            bool classificados = true;

            int qtdeR = lugares.Rows.Count;

            if (qtdeR == 0)
            {
                retorno = 0;
                return retorno;
            }

            DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(lugares, GRUPO);

            int g = tabelaTmp.Rows.Count; //qtde de grupos

            DataRow[] linhasGrupo = lugares.Select(GRUPO + "=0");
            DataRow[] linhasClassificacao = lugares.Select(CLASSIFICACAO + "=0");

            //			if (linhasGrupo.Length > 0  && linhasClassificacao.Length > 0){
            //				retorno = -3;
            //				return retorno;
            //			}

            if (linhasGrupo.Length > 0 || linhasClassificacao.Length > 0)
            {
                retorno = -1;
                return retorno;
            }

            //verificar se clasificacao eh sequencial: 1,2,3,4,5
            for (int i = 0; (i + 1) < qtdeR; i++)
            {
                int diff = Math.Abs((int)lugares.Rows[i][CLASSIFICACAO] - (int)lugares.Rows[i + 1][CLASSIFICACAO]);
                if (diff != 1)
                {
                    classificados = false;
                    break;
                }
            }

            if (classificados)
            { //verificar agora agrupados

                if (g > 1)
                { //mais de 1 grupo
                    //verificar se lugares foram dividos em meio d q nenhum lugar fique sozinho
                    agrupados = true;
                    foreach (DataRow grupo in tabelaTmp.Rows)
                    {
                        int Q = (int)lugares.Compute("Count(LugarID)", GRUPO + "=" + (int)grupo[GRUPO]);
                        if (Q == 1)
                        {
                            agrupados = false;
                            break;
                        }
                    }

                    if (agrupados)
                    {
                        //verificar se os grupos sao lado a lado
                        for (int i = 0; (i + 1) < g; i++)
                        {
                            int diff = Math.Abs((int)tabelaTmp.Rows[i][GRUPO] - (int)tabelaTmp.Rows[i + 1][GRUPO]);
                            if (diff != 1)
                            {
                                agrupados = false;
                                break;
                            }
                        }
                    }

                }
                else
                { //ok eh 1 grupo soh
                    agrupados = true;

                }//if (g > 1)

                retorno = (agrupados) ? -1 : -2;

                if (!agrupados)
                { //tentar o select de novo selecionando proximos grupos
                    retorno = -2;
                    //verificar qtde de ingressos, se for ultimos ingressos, voltar essa mesma.
                    string sql = "SELECT Count(ID) " +
                        "FROM tIngresso " +
                        "WHERE Status = 'D' AND ApresentacaoSetorID=" + apresentacaoSetorID;
                    object obj = bd.ConsultaValor(sql);
                    bd.Consulta().Close();
                    int Q = (obj != null) ? (int)obj : 0;
                    if (Q == qtdeR)
                    {
                        agrupados = true;
                        retorno = -1;
                    }
                    else
                    {
                        //pular grupo
                        retorno = (int)tabelaTmp.Rows[g - 1][GRUPO];
                    }
                }

            }
            else
            {
                retorno = (int)tabelaTmp.Rows[g - 1][GRUPO];
                //verificar qtde de ingressos, se for ultimos ingressos, voltar essa mesma.
                string sql = "SELECT Count(ID) " +
                    "FROM tIngresso " +
                    "WHERE Status = 'D' AND ApresentacaoSetorID=" + apresentacaoSetorID;
                object obj = bd.ConsultaValor(sql);
                bd.Consulta().Close();
                int Q = (obj != null) ? (int)obj : 0;
                if (Q == qtdeR)
                {
                    agrupados = true;
                    retorno = -1;
                }
                else
                {
                    //pular grupo
                    retorno = (int)tabelaTmp.Rows[g - 1][GRUPO];
                }
            }//if (classificados){

            return retorno;

        }

        public decimal TaxaConveniencia(int eventoID, int precoID, int canalID)
        {
            BD bd = new BD();
            try
            {
                CanalEvento canalEvento = new CanalEvento();
                int taxaConvenienciaEvento = 0;
                decimal[] taxasMinMax;


                if (canalID == 0)
                {
                    taxaConvenienciaEvento = canalEvento.BuscaTaxaConveniencia(Canal.CANAL_INTERNET, eventoID, bd);
                    taxasMinMax = canalEvento.BuscaTaxasMinMax(Canal.CANAL_INTERNET, eventoID);
                }
                else
                {
                    taxaConvenienciaEvento = canalEvento.BuscaTaxaConveniencia(canalID, eventoID, bd);
                    taxasMinMax = canalEvento.BuscaTaxasMinMax(canalID, eventoID);
                }

                object x = bd.ConsultaValor("SELECT Valor FROM tPreco(NOLOCK) WHERE ID =" + precoID);
                
                decimal valor = x != null ? (decimal)x : 0;
                decimal dAux = (taxaConvenienciaEvento / 100m) * valor;

                if (dAux < taxasMinMax[0] && taxasMinMax[0] > 0)
                    valor = Decimal.Round(taxasMinMax[0], 2);
                else if (dAux > taxasMinMax[1] && taxasMinMax[1] > 0)
                    valor = Decimal.Round(taxasMinMax[1], 2);
                else
                    valor = Decimal.Round(dAux, 2);

                return valor;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public decimal ValorTaxaProcessamento(int eventoID)
        {
            BD bd = new BD();
            try
            {
                return Convert.ToDecimal(
                                   bd.ConsultaValor(
                                       string.Format(@"
											SELECT
												
												CASE WHEN et.PossuiTaxaProcessamento = 'T'
													THEN et.ValorTaxa
													ELSE 0
													END AS Valor
												FROM tEvento e (NOLOCK)
												INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID
												INNER JOIN tEstado et (NOLOCK) ON et.Sigla COLLATE Latin1_General_CI_AI = l.Estado
										WHERE e.ID = {0}",
                               eventoID)));
            }
            finally
            {
                bd.Fechar();
            }

        }

        public decimal TaxaConvenienciaPacote(int PacoteID, int canalID)
        {
            BD bd = new BD();
            try
            {
                CanalPacote canalEvento = new CanalPacote();
                int taxaConvenienciaPacote = 0;
                decimal taxaMin = 0;
                decimal taxaMax = 0;
                decimal valor = 0;

                bd.Consulta(@"SELECT cp.TaxaConveniencia, cp.TaxaMinima,cp.TaxaMaxima, SUM(p.Valor * i.Quantidade) AS Valor 
										FROM tCanalPacote cp (NOLOCK)
										INNER JOIN tPacote pa (NOLOCK) ON pa.ID = cp.PacoteID
										INNER JOIN tPacoteItem i (NOLOCK) ON i.PacoteID = pa.ID
										INNER JOIN tPreco p(NOLOCK) ON p.ID = i.PrecoID
										WHERE pa.ID = " + PacoteID + " AND CanalID =" + canalID +
                                        " GROUP BY cp.TaxaConveniencia, cp.TaxaMinima,cp.TaxaMaxima");

                while (bd.Consulta().Read())
                {
                    taxaConvenienciaPacote = bd.LerInt("TaxaConveniencia");
                    taxaMin = bd.LerDecimal("TaxaMinima");
                    taxaMax = bd.LerDecimal("TaxaMaxima");
                    valor = bd.LerDecimal("Valor");

                }


                decimal dAux = (taxaConvenienciaPacote / 100m) * valor;
                if (dAux < taxaMin && taxaMin > 0)
                {
                    valor = Decimal.Round(taxaMin, 2);
                }
                else if (dAux > taxaMax && taxaMax > 0)
                {
                    valor = Decimal.Round(taxaMax, 2);
                }
                else
                {
                    valor = Decimal.Round(dAux, 2);
                }


                return valor;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }
        public int GetIngressosQPodeReservar(int precoID, int qtde)
        {
            int canalID = 2; //Internet
            BD bd = new BD();

            //qtdes de preço
            int qtdePrecoDisponivel = 0; //qtde de preços disponiveis, q podem vender, independente do canal
            int qtdePrecoPorCliente = 0; //qtde de preços q podem vender por cliente independente do canal
            int qtdePrecoPorCanal = 0; //qtde de preços q podem vender por canal

            int qtdePrecoVendido = 0; //qtde de ingressos vendidos com esse preço
            int qtdeCanalVendido = 0; //qtde de ingressos vendidos para esse canal
            int qtdePrecoVendidaPorCliente = 0; //qtde de ingressos vendidos com esse preço

            int qtdeIngressosQPodeReservar = qtde; //qtde de ingressos a ser reservado

            int qtdeMaxIngressosQPodeReservarPorPreco = 0; //qtde maxima q pode reservar
            int qtdeMaxIngressosQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal
            int qtdeMaxIngressosQPodeReservarPorCliente = 0; //qtde maxima q pode reservar por cliente            

            int validadePreco; //dias q faltam para vender os ingressos (se maior que zero entao pode vender)

            //1. alimenta qtdes de preço
            Preco preco = new Preco();
            //bd.Fechar();
            Preco.InfoToReserva infoPreco = preco.LerToReserva(precoID, canalID, 0, bd);

            qtdePrecoDisponivel = infoPreco.Preco.Quantidade.Valor;
            qtdePrecoPorCliente = infoPreco.Preco.QuantidadePorCliente.Valor;
            qtdePrecoVendido = infoPreco.QtdVendidoPreco;
            qtdeCanalVendido = infoPreco.QtdVendidoCanal;

            //2. verificar a qtde total de preço independente do canal
            if (qtdePrecoDisponivel != 0)
            {
                //2.1 verificar qtde de ingressos vendidos a esse preço.
                //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                if (qtdePrecoVendido >= qtdePrecoDisponivel)
                {
                    //codMsg = CodMensagem.PorPreco;
                    //alertaMsg = AlertaMensagem.Exclamacao;
                    throw new BilheteriaException("Esse preço não pode mais ser vendido.\nJá foi toda sua quota de " + qtdePrecoDisponivel + " ingressos!");
                }

                qtdeMaxIngressosQPodeReservarPorPreco = qtdePrecoDisponivel - qtdePrecoVendido;

                if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorPreco)
                    qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorPreco;
            }


            //atribuir a qtde de preço ja vendida nao estourou qtde disponivel no canal
            qtdePrecoPorCanal = infoPreco.QtdDisponivelCanal;	/// canalPreco.QuantidadeDisponivel();
            //atribuir a data limite para venda dos ingressos
            validadePreco = infoPreco.ValidadeCanal;	/// canalPreco.Validade();

            if (validadePreco == 0)
            {
                //expirou a data!
                //alertaMsg = AlertaMensagem.Exclamacao;
                throw new BilheteriaException("Data de validade para venda desse preço expirou!\n\nValidade: " + infoPreco.DataFim.ToShortDateString() + "\nHoje: " + System.DateTime.Now.ToShortDateString());
            }

            //3.1 se Quantidade for zero, esta liberado para vender a vontade!
            if (qtdePrecoPorCanal != 0)
            {
                //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender ingresso(s)
                if (qtdeCanalVendido >= qtdePrecoPorCanal)
                {
                    //codMsg = CodMensagem.PorCanal;
                    //alertaMsg = AlertaMensagem.Exclamacao;
                    throw new BilheteriaException("Esse preço não está mais disponível no seu canal.\nJá foi vendida toda sua quota de " + qtdePrecoPorCanal + " ingressos!");
                }

                qtdeMaxIngressosQPodeReservarPorCanal = qtdePrecoPorCanal - qtdePrecoVendido;

                if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCanal)
                    qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCanal;
            }

            //4. verificar a qtde q pode vender por cliente independente do canal
            if (qtdePrecoPorCliente != 0)
            {
                qtdeMaxIngressosQPodeReservarPorCliente = qtdePrecoPorCliente - qtdePrecoVendidaPorCliente;

                if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCliente)
                    qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCliente;

            }

            return qtdeIngressosQPodeReservar;
        }
        /// <summary>
        /// Traz do banco a quantidade permitida para o  preço verificando a 
        ///quantidade de ingressos que o cliente tem com os seguintes status:
        ///     a.Reservado
        ///     b.Vendido
        ///     c.Impresso
        ///     d.Entregue
        ///     e.Aguardando Troca
        ///
        ///
        ///kim
        /// </summary>
        public bool IngressoDisponivelPreco(int clienteID, int precoID, BD bd)
        {
            try
            {
                StringBuilder precos = new StringBuilder();

                //1.
                string sql = "SELECT QuantidadePorCliente AS QtdePermitidaPreco,COUNT(i.ID) AS QtdeIngressosCliente " +
                            "FROM tIngresso i(NOLOCK) " +
                            "INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID " +
                            "WHERE " +
                            "i.ClienteID = " + clienteID + " AND p.ID = " + precoID + " " +
                            "AND i.Status IN ('" + (char)Ingresso.StatusIngresso.RESERVADO + "','" + (char)Ingresso.StatusIngresso.VENDIDO +
                            "','" + (char)Ingresso.StatusIngresso.IMPRESSO + "','" + (char)Ingresso.StatusIngresso.ENTREGUE +
                            "','" + (char)Ingresso.StatusIngresso.AGUARDANDO_TROCA + "') " +
                            "GROUP BY  QuantidadePorCliente";
                if (clienteID != 0)
                {
                    bd.Consulta(sql);
                    if (bd.Consulta().Read())
                    {
                        //qtdIngressosCliente = bd.LerInt("QtdeIngressosCliente");
                        //qtdPermitidaPreco = bd.LerInt("QtdePermitidaPreco");
                        if (bd.LerInt("QtdeIngressosCliente") > bd.LerInt("QtdePermitidaPreco") && bd.LerInt("QtdePermitidaPreco") > 0)
                            return false;
                    }
                    return true;
                }
                else
                    return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool IngressoDisponivelPreco(int clienteID, int precoID, string sessionID, BD bd)
        {
            string sql = string.Empty;
            try
            {
                sql = "exec sp_getQuantidadePorCliente " + precoID + ", " + clienteID + ",'" + sessionID + "'";
                int possivel = Convert.ToInt32(bd.ConsultaValor(sql));
                if (possivel == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Método faz todas as verificações de quantidade e disponibilidade do preço.
        /// Caso tenha algum problema joga exception.
        /// Retorna a quantidade de ingressos que pode reservar.
        /// Método exclusivo da internet.
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="precoID"></param>
        /// <param name="qtde"></param>
        /// <param name="valorPreco"></param>
        /// <returns></returns>
        public int GetIngressosQPodeReservar(int clienteID, int precoID, int qtde, ref decimal valorPreco, int canalID)
        {
            try
            {
                BD bd = new BD();

                //qtdes de preço
                int qtdePrecoDisponivel = 0; //qtde de preços disponiveis, q podem vender, independente do canal
                int qtdePrecoPorCliente = 0; //qtde de preços q podem vender por cliente independente do canal
                int qtdePrecoPorCanal = 0; //qtde de preços q podem vender por canal

                int qtdePrecoVendido = 0; //qtde de ingressos vendidos com esse preço
                int qtdeCanalVendido = 0; //qtde de ingressos vendidos com esse preço
                int qtdePrecoVendidaPorCliente = 0; //qtde de ingressos vendidos com esse preço

                int qtdeIngressosQPodeReservar = qtde; //qtde de ingressos a ser reservado

                int qtdeMaxIngressosQPodeReservarPorPreco = 0; //qtde maxima q pode reservar
                int qtdeMaxIngressosQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal
                int qtdeMaxIngressosQPodeReservarPorCliente = 0; //qtde maxima q pode reservar por cliente            

                int validadePreco; //dias q faltam para vender os ingressos (se maior que zero entao pode vender)

                //1. alimenta qtdes de preço
                Preco preco = new Preco();
                //bd.Fechar();
                Preco.InfoToReserva infoPreco = preco.LerToReserva(precoID, canalID, 0, bd);

                //Verifica se o cliente pode reservar esse preco
                bool precoOK = IngressoDisponivelPreco(clienteID, precoID, bd);
                if (!precoOK)
                    throw new BilheteriaException("Esse preço não pode mais ser vendido. Sua cota de reservas para esse preço estourou!");
                valorPreco = infoPreco.Preco.Valor.Valor;
                qtdePrecoDisponivel = infoPreco.Preco.Quantidade.Valor;
                qtdePrecoPorCliente = infoPreco.Preco.QuantidadePorCliente.Valor;
                qtdePrecoVendido = infoPreco.QtdVendidoPreco;
                qtdeCanalVendido = infoPreco.QtdVendidoCanal;

                //2. verificar a qtde total de preço independente do canal
                if (qtdePrecoDisponivel != 0)
                {
                    //2.1 verificar qtde de ingressos vendidos a esse preço.
                    //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                    if (qtdePrecoVendido >= qtdePrecoDisponivel)
                    {
                        //codMsg = CodMensagem.PorPreco;
                        //alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Esse preço não pode mais ser vendido.\nJá foi toda sua quota de " + qtdePrecoDisponivel + " ingressos!");
                    }

                    qtdeMaxIngressosQPodeReservarPorPreco = qtdePrecoDisponivel - qtdePrecoVendido;

                    if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorPreco)
                        qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorPreco;
                }

                //atribuir a qtde de preço ja vendida nao estourou qtde disponivel no canal
                qtdePrecoPorCanal = infoPreco.QtdDisponivelCanal;	
                //atribuir a data limite para venda dos ingressos
                validadePreco = infoPreco.ValidadeCanal;	

                if (validadePreco == 0)
                {
                    //expirou a data!
                    //alertaMsg = AlertaMensagem.Exclamacao;
                    throw new BilheteriaException("Data de validade para venda desse preço expirou!\n\nValidade: " + infoPreco.DataFim.ToShortDateString() + "\nHoje: " + System.DateTime.Now.ToShortDateString());
                }

                //3.1 se Quantidade for zero, esta liberado para vender a vontade!
                if (qtdePrecoPorCanal != 0)
                {
                    //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender ingresso(s)
                    if (qtdeCanalVendido >= qtdePrecoPorCanal)
                    {
                        //codMsg = CodMensagem.PorCanal;
                        //alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Esse preço não está mais disponível no seu canal.\nJá foi vendida toda sua quota de " + qtdePrecoPorCanal + " ingressos!");
                    }

                    qtdeMaxIngressosQPodeReservarPorCanal = qtdePrecoPorCanal - qtdePrecoVendido;

                    if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCanal)
                        qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCanal;
                }

                //4. verificar a qtde q pode vender por cliente independente do canal
                if (qtdePrecoPorCliente != 0)
                {
                    qtdeMaxIngressosQPodeReservarPorCliente = qtdePrecoPorCliente - qtdePrecoVendidaPorCliente;

                    if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCliente)
                        qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCliente;

                }

                return qtdeIngressosQPodeReservar;

            }
            catch
            {

                throw;
            }

        }

        #region Métodos antigos do WSIntegracao

        //public Parceria.Parceiro ValidaBinWeb(string bin)
        //{
        //    Parceria oParceria = new Parceria();
        //    return oParceria.ValidaBin(bin);
        //}

        //public bool CancelarTodasReservasWeb(int clienteID, string sessionID)
        //{
        //    Bilheteria bilheteria = new Bilheteria();
        //    return (bilheteria.CancelarTodasReservasInternet(clienteID, sessionID) == 1);
        //}

        //public Cliente GetByIDWeb(int ID)
        //{
        //    Cliente oCliente = new Cliente();
        //    oCliente.Ler(ID);
        //    if (oCliente.Control.ID == 0)
        //        return null;
        //    else
        //        return oCliente;
        //}


        private int[] CadastrarClienteWeb(int id, string Nome, string RG, string CPF, string CarteiraEstudante, string Sexo, string DDDTelefone, string Telefone, string DDDTelefoneComercial, string TelefoneComercial, string DDDCelular, string Celular, string DataNascimento, string Email, bool RecebeEmail, string CEP, string Endereco, string Numero, string Complemento, string Bairro, string Cidade, string Estado, string CEPCobranca, string EnderecoCobranca, string NumeroCobranca, string ComplementoCobranca, string BairroCobranca, string CidadeCobranca, string EstadoCobranca, int ClienteIndicacaoID, string Obs, string Senha)
        {
            Cliente cliente = new Cliente();
            try
            {
                cliente.Ler(id);
                cliente.Nome.Valor = Nome;
                cliente.RG.Valor = RG;
                cliente.CPF.Valor = CPF;
                cliente.CarteiraEstudante.Valor = CarteiraEstudante;
                cliente.Sexo.Valor = Sexo;
                cliente.DDDTelefone.Valor = DDDTelefone;
                cliente.Telefone.Valor = Telefone;
                cliente.DDDTelefoneComercial.Valor = DDDTelefoneComercial;
                cliente.TelefoneComercial.Valor = TelefoneComercial;
                cliente.DDDCelular.Valor = DDDCelular;
                cliente.Celular.Valor = Celular;
                if (DataNascimento.Length > 0)
                    cliente.DataNascimento.Valor = new DateTime(Convert.ToInt32(DataNascimento.Substring(0, 4)), Convert.ToInt32(DataNascimento.Substring(4, 2)), Convert.ToInt32(DataNascimento.Substring(6, 2))); //Convert.ToDateTime(DataNascimento.Substring(6,2)+"/"+ DataNascimento.Substring(4,2) +"/"+ DataNascimento.Substring(0,4));

                cliente.Email.Valor = Email;
                cliente.RecebeEmail.Valor = RecebeEmail;
                cliente.CEPEntrega.Valor = CEP;
                cliente.EnderecoEntrega.Valor = Endereco;
                cliente.NumeroEntrega.Valor = Numero;
                cliente.ComplementoEntrega.Valor = Complemento;
                cliente.BairroEntrega.Valor = Bairro;
                cliente.CidadeEntrega.Valor = Cidade;
                cliente.EstadoEntrega.Valor = Estado;
                cliente.CEPCliente.Valor = CEPCobranca;
                cliente.EnderecoCliente.Valor = EnderecoCobranca;
                cliente.NumeroCliente.Valor = NumeroCobranca;
                cliente.ComplementoCliente.Valor = ComplementoCobranca;
                cliente.BairroCliente.Valor = BairroCobranca;
                cliente.CidadeCliente.Valor = CidadeCobranca;
                cliente.EstadoCliente.Valor = EstadoCobranca;
                cliente.ClienteIndicacaoID.Valor = ClienteIndicacaoID;
                cliente.Obs.Valor = Obs;
                cliente.Senha.Valor = Senha;
                cliente.StatusAtual.Valor = "L";

                if (id == 0)
                    cliente.Inserir();
                else
                {
                    if (Senha == null || Senha.Length == 0)
                        cliente.Senha.Valor = cliente.Senha.ValorAntigo;
                    cliente.Ativo.Valor = true;
                    cliente.Control.ID = id;
                    cliente.Atualizar();
                }

                return new int[] { (int)Cliente.Infos.Sucesso, cliente.Control.ID };
            }
            catch (ClienteException ex)
            {
                return new int[] { (int)ex.CodigoErro, ex.ID };
            }
            catch
            {
                return new int[] { (int)Cliente.Infos.ErroIndefinido, cliente.Control.ID };
            }

        }
        public int BuscarCupomValidoWeb(string cupom)
        {
            CupomDescontoParalela cupomdesconto = new CupomDescontoParalela();

            try
            {
                cupomdesconto.ValidaCupomApresentacao(new int[] { 1, 2, 3 }, "ado");

                cupomdesconto.ValidarCupom(cupom);

                if (cupomdesconto.Control.ID != 0)
                    return cupomdesconto.Control.ID;
                else
                    return 0;
            }
            catch (CupomDescontoException ex)
            {
                throw new CupomDescontoException(ex.Message);
            }
        }
        public string[] ValidarCupomWeb(int[] apresentacoesID, string cupom)
        {
            CupomDescontoParalela oCupom = new CupomDescontoParalela();
            string[] retorno = new string[2];

            try
            {
                retorno[0] = oCupom.ValidaCupomApresentacao(apresentacoesID, cupom).ToString();
                retorno[1] = string.Empty;

                return retorno;
            }
            catch (Exception ex)
            {
                //throw new CupomDescontoException(ex.Message);
                retorno[0] = "0";
                retorno[1] = ex.Message;

                return retorno;
            }
        }

        #endregion
        public Ingresso[] ReservarPistaInternetNOVO(string sessionID, int apresentacaoSetorID, int clienteID, int[,] precos, int eventoID, EstruturaReservaInternet estruturaReservaInternet)
        {
            BD bd = new BD();
            BD bdBusca = new BD(ConfigurationManager.AppSettings["ConexaoReadOnly"]);
            ArrayList ingressos = new ArrayList();
            Ingresso ing = null;
            CodigoBarra cb = new CodigoBarra();
            BD bdReserva = new BD();

            try
            {
                //int qtd = 0;
                int qtdeIngressosQPodeReservar = 0;

                //precoID, Quantidade
                Dictionary<int, int> precosXqtdes = new Dictionary<int, int>();

                // looop para montar a lista de precos e qtdes
                for (int i = 0; i < precos.Length / 2; i++)
                    precosXqtdes.Add(precos[i, 0], precos[i, 1]);


                decimal valorPreco = -1;
                //Verificar cada precoID para saber a qtde q pode reservar                
                foreach (KeyValuePair<int, int> precoXqtde in precosXqtdes)
                {
                    qtdeIngressosQPodeReservar = GetIngressosQPodeReservar(clienteID, precoXqtde.Key, precoXqtde.Value, ref valorPreco, estruturaReservaInternet.CanalID);

                    bdBusca.Consulta(string.Format("SELECT TOP {0} ID, Codigo, LugarID FROM tIngresso(NOLOCK) WHERE ApresentacaoSetorID = {1} AND Status = '{2}' ORDER BY newid()", qtdeIngressosQPodeReservar, apresentacaoSetorID, Ingresso.DISPONIVEL));
                    //int p = 0;
                    while (bdBusca.Consulta().Read())
                    {
                        decimal valorConv = this.TaxaConveniencia(eventoID, precoXqtde.Key, estruturaReservaInternet.CanalID);
                        ing = new Ingresso();
                        ing.Control.ID = bdBusca.LerInt("ID");
                        ing.PrecoID.Valor = precoXqtde.Key;
                        ing.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                        ing.Codigo.Valor = bdBusca.LerString("Codigo");
                        ing.LojaID.Valor = estruturaReservaInternet.LojaID;
                        ing.ClienteID.Valor = clienteID;
                        ing.SessionID.Valor = sessionID;
                        ing.TimeStampReserva.Valor = DateTime.Now;
                        ing.LugarID.Valor = bdBusca.LerInt("LugarID");
                        ing.TxConv = valorConv;

                        //se não tiver valor e não tiver conveniencia não deve reservar
                        if (valorPreco == 0 && ing.TxConv == 0)
                        {
                            // Atribui a Cortesia Padrão do Evento/Local - INICIO
                            Cortesia oCortesia = new Cortesia();
                            int CortesiaPadraoID = oCortesia.CortesiaPadraoEvento(eventoID);
                            if (CortesiaPadraoID == 0)
                                throw new Exception("Não foi possível reservar o ingresso. Por favor, tente novamente mais tarde.");
                            oCortesia = null;
                            ing.CortesiaID.Valor = CortesiaPadraoID;
                            // Atribui a Cortesia Padrão do Evento/Local - FIM

                            ing.Status.Valor = Ingresso.CORTESIA_SEM_CONVENIENCIA;
                            ingressos.Add(ing);
                            break;//break para inserir somente um registro. esse registro de ingresso vai ser utilizado 
                            //como base de info para deletar o preço inválido do banco de dados do site.
                        }
                        else
                        {
                            if (ing.Reservar(bdReserva, false))
                                ingressos.Add(ing);
                        }
                        //p++;
                    }


                    // se algum ingresso foi reservado aumenta o TimeStampReserva de todos ingressos da sessão do cliente
                    if (ingressos.Count > 0)
                        Ingresso.AumentarTempoReservasInternet(clienteID, sessionID);

                    bdReserva.Fechar();
                    bd.Fechar();
                    bdBusca.Fechar();

                }

                return (Ingresso[])ingressos.ToArray(typeof(Ingresso));
            }
            catch
            {
                throw;
            }
            finally
            {
                bdReserva.Fechar();
                bd.Fechar();
                bdBusca.Fechar();
            }
        }

        public Ingresso[] ReservarPistaInternet(string sessionID, int apresentacaoSetorID, int clienteID, int[,] precos, int eventoID, EstruturaReservaInternet estruturaReservaInternet)
        {
            BD bd = new BD();
            BD bdBusca = new BD();
            ArrayList ingressos = new ArrayList();
            Ingresso ing;
            CodigoBarra cb = new CodigoBarra();
            BD bdReserva = new BD();

            try
            {
                int qtd = 0;
                List<int> precoID = new List<int>();

                for (int i = 0; i < precos.Length / 2; i++)
                {
                    qtd += precos[i, 1];
                    for (int x = 0; x < precos[i, 1]; x++)
                        precoID.Add(precos[i, 0]);
                }

                bdBusca.Consulta(string.Format("SELECT TOP {0} ID, Codigo, LugarID FROM tIngresso(NOLOCK) WHERE ApresentacaoSetorID = {1} AND Status = '{2}' ORDER BY newid()", qtd, apresentacaoSetorID, Ingresso.DISPONIVEL));
                int p = 0;
                while (bdBusca.Consulta().Read())
                {
                    decimal valorConv = this.TaxaConveniencia(eventoID, precoID[p], estruturaReservaInternet.CanalID);
                    ing = new Ingresso();
                    ing.Control.ID = bdBusca.LerInt("ID");
                    ing.PrecoID.Valor = precoID[p];
                    ing.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                    ing.Codigo.Valor = bdBusca.LerString("Codigo");

                    //CodigoBarra oCodigoBarra = new CodigoBarra();
                    //ing.CodigoBarra.Valor = oCodigoBarra.GerarCodigoBarra(precoID[p], eventoID);

                    ing.LojaID.Valor = estruturaReservaInternet.LojaID;
                    ing.ClienteID.Valor = clienteID;
                    ing.SessionID.Valor = sessionID;
                    ing.TimeStampReserva.Valor = DateTime.Now;
                    ing.LugarID.Valor = bdBusca.LerInt("LugarID");
                    ing.TxConv = valorConv;

                    if (ing.Reservar(bdReserva, false))
                        ingressos.Add(ing);

                    p++;
                }
                bdReserva.Fechar();
                bd.Fechar();
                bdBusca.Fechar();

                return (Ingresso[])ingressos.ToArray(typeof(Ingresso));
            }
            catch
            {
                throw;
            }
            finally
            {
                bdReserva.Fechar();
                bd.Fechar();
                bdBusca.Fechar();


            }
        }

        public DataSet ReservarLugarMarcadoAssinatura(int pacoteID, DataTable lugares, int canalID, int lojaID, int usuarioID, int seedReserva, bool buscaTaxasEntrega, bool buscaFormaPagamento, bool preReserva)
        {

            DataSet retorno = estruturaReservas(seedReserva); // Estrutura dos DataTables de retorno.
            int lugarID, ingressoID; // variáveis utilizadas dentro do for
            // String de busca do ingresso.
            string sql = "SELECT ID, CodigoBarra, CodigoSequencial FROM tIngresso(NOLOCK) WHERE ApresentacaoSetorID = {0} AND Status = '{1}' AND LugarID = {2} ";
            object auxID = null;

            BD bd = new BD();
            Ingresso oIngresso = new Ingresso();
            Cota oCota = new Cota();
            CotaItem oCotaItem = new CotaItem();
            CotaItemControle oCotaItemControle = new CotaItemControle();
            ArrayList ids = new ArrayList();
            DataTable itensPacote;
            decimal valorPacote;
            List<int> lstEvento = new List<int>();



            try
            {

                // Busca os itens do pacote e a taxa de conveniência
                Pacote oPacote = new Pacote(usuarioID);


                bool PodeComprar = oPacote.VetificaQuantidadePreco(pacoteID, canalID, lugares.Rows.Count, bd);

                if (!PodeComprar)
                {
                    throw new BilheteriaException("Esse preço não pode ser vendido.\nVerifique a cota de preço do ingressos!");
                }

                oPacote.Control.ID = pacoteID;
                itensPacote = oPacote.ItensParaReservaDePacote(bd); // Itens

                valorPacote = 0;


                List<int> lstCotaItemID = new List<int>();
                int taxaConveniencia = 0;
                int taxaComissao = 0;
                decimal taxaMin = 0;
                decimal taxaMax = 0;
                decimal comissaoMin = 0;
                decimal comissaoMax = 0;
                int apresentacaoIDaux = 0;
                int apresentacaoSetorIDaux = 0;
                int cotaItemID = 0;
                bool mudou = false;
                bool validaBin = false;
                bool encontrouCota = false;
                int[] quantidadeDifDisponivel = new int[2] { 0, 0 };
                int quantidadeMaxima = 0;

                EstruturaPrecoReservaSite ePreco;
                EstruturaCotasReserva eCotaItem;
                List<EstruturaCotaItemReserva> listaCotaItem = new List<EstruturaCotaItemReserva>();
                EstruturaCotasInfo cotasInfo = new EstruturaCotasInfo();

                if (!preReserva)
                {
                    CanalPacote canalPacote = new CanalPacote();
                    DataTable taxas = canalPacote.BuscaTaxasConvenienciaComissao(canalID, pacoteID);

                    valorPacote = (decimal)itensPacote.Compute("SUM(ValorItem)", "1=1");

                    taxaConveniencia = (int)taxas.Rows[0]["TaxaConveniencia"];
                    taxaMin = Convert.ToDecimal(taxas.Rows[0]["TaxaMinima"]);
                    taxaMax = Convert.ToDecimal(taxas.Rows[0]["TaxaMaxima"]);
                    taxaComissao = (int)taxas.Rows[0]["TaxaComissao"];
                    comissaoMin = Convert.ToDecimal(taxas.Rows[0]["ComissaoMinima"]);
                    comissaoMax = Convert.ToDecimal(taxas.Rows[0]["ComissaoMaxima"]);
                }

                foreach (DataRow lugar in lugares.Rows) // lugares
                {
                    lugarID = (int)lugar["LugarID"];

                    bd.IniciarTransacao();

                    #region Insere no grid exibido para o operador
                    DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                    novoItemGrid[EVENTO_PACOTE] = (string)itensPacote.Rows[0]["PacoteNome"];
                    novoItemGrid[SETOR_PRODUTO] = (string)itensPacote.Rows[0]["SetorNome"];
                    novoItemGrid[LUGAR_MARCADO] = true;
                    novoItemGrid[VALOR] = valorPacote;
                    novoItemGrid[TIPO] = TIPO_PACOTE;
                    novoItemGrid[PERMITIR_CANCELAMENTO_AVULSO] = (bool)itensPacote.Rows[0]["PermitirCancelamentoAvulso"];
                    novoItemGrid[TIPO_LUGAR] = lugar["tipoLugar"];
                    novoItemGrid[CONV] = taxaConveniencia;

                    if (novoItemGrid[VALOR_CONV] == DBNull.Value)
                        novoItemGrid[VALOR_CONV] = 0;

                    novoItemGrid[CONV_MIN] = taxaMin;
                    novoItemGrid[CONV_MAX] = taxaMax;

                    novoItemGrid[CODIGO] = lugar[CODIGO];

                    #endregion

                    List<DataRow> novosItensReserva = new List<DataRow>();

                    Random rnd = new Random((int)System.DateTime.Now.Ticks);
                    int numRan = rnd.Next(1, 999999);

                    string pacoteGrupo = numRan.ToString("000000");
                    int codigoSequencial = 0;
                    string codigoBarra = string.Empty;
                    foreach (DataRow itemPacote in itensPacote.Rows)
                    {
                        eCotaItem = new EstruturaCotasReserva();
                        int eventoID = (int)itemPacote["EventoID"];
                        int precoID = (int)itemPacote["PrecoID"];
                        int qtde = (int)itemPacote["Quantidade"];
                        int apresentacaoSetorID = (int)itemPacote["ApresentacaoSetorID"];
                        int apresentacaoID = (int)itemPacote["ApresentacaoID"];
                        string precoNome = itemPacote["PrecoNome"].ToString();
                        Enumerators.TipoCodigoBarra tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(itemPacote["TipoCodigoBarra"]);
                        eCotaItem.CotaIDApresentacao = Convert.ToInt32(itemPacote["ApresentacaoCotaID"]);
                        eCotaItem.CotaIDApresentacaoSetor = Convert.ToInt32(itemPacote["ApresentacaoSetorCotaID"]);
                        eCotaItem.QuantidadeApresentacao = Convert.ToInt32(itemPacote["QuantidadeApresentacao"]);
                        eCotaItem.QuantidadePorClienteApresentacao = Convert.ToInt32(itemPacote["QuantidadePorClienteApresentacao"]);
                        eCotaItem.QuantidadeApresentacaoSetor = Convert.ToInt32(itemPacote["QuantidadeApresentacaoSetor"]);
                        eCotaItem.QuantidadePorClienteApresentacaoSetor = Convert.ToInt32(itemPacote["QuantidadePorClienteApresentacaoSetor"]);


                        ePreco = new EstruturaPrecoReservaSite();
                        ePreco.PrecoNome = precoNome;
                        ePreco.ID = precoID;
                        ePreco.Quantidade = qtde;
                        if (apresentacaoID != apresentacaoIDaux || apresentacaoSetorID != apresentacaoSetorIDaux)
                        {
                            apresentacaoIDaux = apresentacaoID;
                            apresentacaoSetorIDaux = apresentacaoSetorID;
                            mudou = true;
                            listaCotaItem = new List<EstruturaCotaItemReserva>();
                            if (eCotaItem.CotaIDApresentacao != 0 || eCotaItem.CotaIDApresentacaoSetor != 0)
                                listaCotaItem = oCotaItem.getListaCotaItemReserva(eCotaItem.CotaIDApresentacao, eCotaItem.CotaIDApresentacaoSetor);

                        }
                        else
                            mudou = false;


                        //int[] quantidadesMaximas = new int[2] { 0, 0 };
                        //Cotas
                        #region Busca as Cotas do Pacote
                        //Busca as Cotas


                        //Verifica se é possivel reservar o ingresso apartir das cotas geradas p/ a apresentacao/setor
                        //Dispara Exception e nao deixa reservar\
                        if (listaCotaItem.Count != 0 && mudou)
                        {
                            int quantidade = ePreco.Quantidade;


                            for (int i = 0; i < listaCotaItem.Count; i++)
                            {
                                //if (ePreco.PrecoNome.StartsWith(listaCotaItem[i].PrecoIniciaCom))
                                if (ePreco.PrecoNome.ToLower().StartsWith(listaCotaItem[i].PrecoIniciaCom.ToLower()))
                                {
                                    cotasInfo = new EstruturaCotasInfo();
                                    encontrouCota = true;
                                    cotaItemID = listaCotaItem[i].ID;
                                    if (!lstCotaItemID.Contains(listaCotaItem[i].ID))
                                        lstCotaItemID.Add(listaCotaItem[i].ID);

                                    cotasInfo.QuantidadeCota = listaCotaItem[i].Quantidade;
                                    cotasInfo.QuantidadePorClienteCota = listaCotaItem[i].QuantidadePorCliente;
                                    cotasInfo.QuantidadeApresentacao = eCotaItem.QuantidadeApresentacao;
                                    cotasInfo.QuantidadePorClienteApresentacao = eCotaItem.QuantidadePorClienteApresentacao;
                                    cotasInfo.QuantidadeApresentacaoSetor = eCotaItem.QuantidadeApresentacaoSetor;
                                    cotasInfo.QuantidadePorClienteApresentacaoSetor = eCotaItem.QuantidadePorClienteApresentacaoSetor;
                                    cotasInfo.Nominal = listaCotaItem[i].Nominal;
                                    //quantidadesMaximas = this.getQuantidadeMaximaCota(qtds);

                                    quantidadeDifDisponivel = oCotaItemControle.getQuantidade(cotaItemID, apresentacaoID, apresentacaoSetorID);

                                    quantidadeMaxima = cotasInfo.getQuantidadeMaxima();

                                    if ((ePreco.Quantidade + quantidadeDifDisponivel[0] > quantidadeMaxima ||
                                        ePreco.Quantidade + quantidadeDifDisponivel[1] > quantidadeMaxima) && quantidadeMaxima != 0)
                                        throw new BilheteriaException("O Preço: " + ePreco.PrecoNome + " deste pacote atingiu o Limite de Venda/Reserva");

                                    validaBin = listaCotaItem[i].ValidaBin;
                                    break;
                                }
                            }
                        }
                        else if (listaCotaItem.Count == 0)
                        {
                            validaBin = false;
                            cotaItemID = 0;
                        }

                        #endregion


                        // Zera a variável de IngressoID Auxiliar
                        auxID = null;

                        // Buscar o ingresso filtrando por ApresentacaoSetorID, LugarID e Status.
                        bd.Consulta(
                                    string.Format(sql, itemPacote["ApresentacaoSetorID"].ToString(), Ingresso.DISPONIVEL, lugarID));

                        if (bd.Consulta().Read())
                        {
                            auxID = bd.LerInt("ID");
                            codigoSequencial = bd.LerInt("CodigoSequencial");
                            codigoBarra = bd.LerString("CodigoBarra");
                        }
                        bd.Consulta().Close();
                        // Verifica se encontrou algum registro.
                        if (auxID == null)
                            throw new BilheteriaException("Um ou mais itens do pacote não possuem ingressos disponíveis.");
                        else
                            ingressoID = Convert.ToInt32(auxID);

                        if (novoItemGrid[TAXA_COMISSAO] == DBNull.Value)
                            novoItemGrid[TAXA_COMISSAO] = 0;
                        if (novoItemGrid[COMISSAO_VALOR] == DBNull.Value)
                            novoItemGrid[COMISSAO_VALOR] = 0;

                        //popula a comissao de acordo com a taxa minima e maxima
                        if (taxaComissao > 0 || comissaoMin > 0)
                        {
                            decimal dAux = (taxaComissao / 100m) * (valorPacote);
                            if (dAux < comissaoMin)
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(novoItemGrid[COMISSAO_VALOR]) + comissaoMin / itensPacote.Rows.Count;
                            }
                            else if (dAux > comissaoMax && comissaoMax > 0)
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(novoItemGrid[COMISSAO_VALOR]) + comissaoMax / itensPacote.Rows.Count;
                            }
                            else
                            {
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = Convert.ToDecimal(novoItemGrid[COMISSAO_VALOR]) + dAux / itensPacote.Rows.Count;
                            }
                        }
                        //popula a conveniencia de acordo com a taxa minima e maxima
                        if (taxaConveniencia > 0 || taxaMin > 0)
                        {
                            decimal dAux = (taxaConveniencia / 100m) * (valorPacote);
                            if (dAux < taxaMin)
                            {
                                novoItemGrid[CONV] = taxaConveniencia;
                                novoItemGrid[VALOR_CONV] = Convert.ToDecimal(novoItemGrid[VALOR_CONV]) + taxaMin / itensPacote.Rows.Count;
                            }
                            else if (dAux > taxaMax && taxaMax > 0)
                            {
                                novoItemGrid[CONV] = taxaConveniencia;
                                novoItemGrid[VALOR_CONV] = Convert.ToDecimal(novoItemGrid[VALOR_CONV]) + taxaMax / itensPacote.Rows.Count;
                            }
                            else
                            {
                                novoItemGrid[CONV] = taxaConveniencia;
                                novoItemGrid[VALOR_CONV] = Convert.ToDecimal(novoItemGrid[VALOR_CONV]) + dAux / itensPacote.Rows.Count;
                            }
                        }

                        // Enfim, reserva o ingresso.
                        oIngresso.Control.ID = ingressoID;
                        oIngresso.PrecoID.Valor = (int)itemPacote["PrecoID"];
                        oIngresso.UsuarioID.Valor = usuarioID;
                        oIngresso.CortesiaID.Valor = (int)itemPacote["CortesiaID"];
                        oIngresso.PacoteID.Valor = pacoteID;
                        oIngresso.CotaItemID = cotaItemID;
                        oIngresso.LojaID.Valor = lojaID;
                        oIngresso.PacoteGrupo.Valor = pacoteGrupo;
                        oIngresso.CodigoBarra.Valor = tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca ? codigoBarra : string.Empty;

                        if (!oIngresso.Reservar(bd, false))
                            throw new BilheteriaException("Falha ao reservar o ingresso");

                        ids.Add(ingressoID);

                        #region Insere na tabela de itens reservados
                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[TIPO] = TIPO_INGRESSO;
                        novoItemReserva[RESERVAID] = novoItemGrid[RESERVAID];
                        novoItemReserva[LUGARID] = lugarID;
                        novoItemReserva[PACOTEID] = itemPacote["PacoteID"];
                        novoItemReserva[PRECOID] = itemPacote["PrecoID"];
                        novoItemReserva[CORTESIAID] = itemPacote["CortesiaID"];
                        novoItemReserva[BLOQUEIOID] = 0;
                        novoItemReserva[INGRESSOID] = ingressoID;
                        novoItemReserva[APRESENTACAOID] = itemPacote[APRESENTACAOID];
                        novoItemReserva[APRESENTACAOSETORID] = itemPacote[APRESENTACAOSETORID];
                        novoItemReserva[EVENTOID] = itemPacote["EventoID"];
                        novoItemReserva[CODIGO_SEQUENCIAL] = codigoSequencial;
                        novoItemReserva[CLIENTE_ID] = 0;
                        novoItemReserva[COTA_ITEM_ID] = cotaItemID;

                        novoItemReserva[QUANTIDADE_COTA] = cotasInfo.QuantidadeCota;
                        novoItemReserva[QUANTIDADE_APRESENTACAO] = cotasInfo.QuantidadeApresentacao;
                        novoItemReserva[QUANTIDADE_APRESENTACAO_SETOR] = cotasInfo.QuantidadeApresentacaoSetor;
                        novoItemReserva[QUANTIDADEPORCLIENTE_COTA] = cotasInfo.QuantidadePorClienteCota;
                        novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO] = cotasInfo.QuantidadePorClienteApresentacao;
                        novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = cotasInfo.QuantidadePorClienteApresentacaoSetor;
                        novoItemReserva[NOMINAL] = cotasInfo.Nominal;

                        novoItemReserva[VALIDA_BIN] = validaBin;
                        if (!validaBin)
                            novoItemReserva[STATUS_CODIGO_PROMO] = 0;
                        else
                            novoItemReserva[STATUS_CODIGO_PROMO] = 1;
                        novoItemReserva[OBRIGATORIEDADE_ID] = itemPacote["ObrigatoriedadeID"];
                        novoItemReserva[APRESENTACAOSETORID] = itemPacote["ApresentacaoSetorID"];
                        novoItemReserva[TIPO_CODIGO_BARRA] = itemPacote[TIPO_CODIGO_BARRA];
                        novoItemReserva[CODIGO_BARRA] = codigoBarra;
                        novosItensReserva.Add(novoItemReserva);
                        #endregion

                    }// foreach de itens do pacote

                    lugar[RESERVADO] = true;

                    if (encontrouCota)
                    {
                        novoItemGrid[COTA_ITEM_ID] = 0;
                        novoItemGrid[COTA] = "Sim";
                    }
                    else
                    {
                        novoItemGrid[COTA_ITEM_ID] = 0;
                        novoItemGrid[COTA] = "Não";
                    }
                    retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);

                    foreach (DataRow itemReserva in novosItensReserva)
                    {
                        retorno.Tables[TABELA_RESERVA].Rows.Add(itemReserva);
                    }
                    bd.FinalizarTransacao();
                }// foreach de lugares
                foreach (DataRow lugar in lugares.Rows)
                {
                    retorno.Tables[TABELA_RESERVA_LUGAR_MARCADO].ImportRow(lugar);
                }

                bd.Fechar();

                DataTable reservados = (new Ingresso()).InfoReservados((int[])ids.ToArray(typeof(int)), bd, lojaID);

                foreach (DataRow linha in reservados.Rows)
                {
                    if (!lstEvento.Contains(Convert.ToInt32(linha["EventoID"])))
                        lstEvento.Add(Convert.ToInt32(linha["EventoID"]));

                    //popula a taxa de acordo com a taxa minima e maxima
                    if (taxaConveniencia > 0 || taxaMin > 0)
                    {
                        decimal dAux = (taxaConveniencia / 100m) * (valorPacote);

                        if (dAux < taxaMin)
                        {
                            linha["TaxaConveniencia"] = taxaConveniencia;
                            linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMin / itensPacote.Rows.Count, 2);
                        }
                        else if (dAux > taxaMax && taxaMax > 0)
                        {

                            linha["TaxaConveniencia"] = taxaConveniencia;
                            linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMax / itensPacote.Rows.Count, 2);
                        }
                        else
                        {
                            linha["TaxaConveniencia"] = taxaConveniencia;
                            linha["TaxaConvenienciaValor"] = Decimal.Round(dAux / itensPacote.Rows.Count, 2);
                        }
                    }

                    retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].ImportRow(linha);
                }

                if (reservados.Rows.Count > 0 && buscaTaxasEntrega)
                {
                    // Busca taxas de entrega do Evento
                    DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas(lstEvento.ToArray());
                    retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Merge(dtEventoTaxaEntrega);

                    // Busca regiões de entrega do Evento
                    DataTable dtEventoRegiao = new Regiao().GetRegioes(lstEvento.ToArray());
                    retorno.Tables[TABELA_EVENTO_REGIAO].Merge(dtEventoRegiao);

                }

                if (reservados.Rows.Count > 0 && buscaFormaPagamento)
                {
                    // Busca formas de pagamento do Evento
                    foreach (int eventoID in lstEvento)
                    {
                        DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento(eventoID, canalID);
                        retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Merge(dtEventoFormaPagamento);
                    }

                }

                if (lstCotaItemID.Count != 0)
                {
                    DataTable dtt = new CotaItem().getDttCotaItemPorCotaID(lstCotaItemID.ToArray());
                    retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);

                    DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(lstCotaItemID.ToArray());
                    retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);
                }

                return retorno;
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

        /// <summary>
        /// Reserva o pacote. Método utilizado somente para a importaçao de assinaturas da OSESP.
        /// Esse Método está preparado para reservar apenas pacotes de pista.
        /// </summary>
        public DataSet ReservarPacoteImportacaoAssinatura(BD bd, IRLib.Paralela.ClientObjects.EstruturaAssinaturaInfo assinaturaInfo, List<int> ingressosID, int usuarioID, int lojaID, int canalID, int seedReserva)
        {
            try
            {
                Pacote oPacote = new Pacote();
                CanalPacote canalPacote = new CanalPacote();
                StringBuilder sqlReservar = new StringBuilder();
                oPacote.Control.ID = assinaturaInfo.PacoteID;

                //itens do pacote.
                DataTable itensPacote = oPacote.ItensParaReservaDePacote(bd);
                // Estrutura dos DataTables de retorno.
                DataSet retorno = estruturaReservas(seedReserva);

                List<Ingresso> ingressos = new List<Ingresso>();
                Ingresso ingresso; // Ingresso auxiliar.

                int pacoteGrupo = int.Parse(DateTime.Now.ToString("hhMMss"));
                int precoID;
                int cortesiaID;
                int apresentacaoID;
                int eventoID;

                DateTime timeStamp = DateTime.Now;

                // Toda a reserva do pacote deve estar em transação. Tudo ou nada.
                bd.IniciarTransacao();

                pacoteGrupo++;

                foreach (int ingressoID in ingressosID)
                {
                    //Busca as informaçoes do ingresso.
                    string sql = "SELECT Codigo, EventoID, ApresentacaoID, SetorID, PacoteID,ApresentacaoSetorID, LugarID FROM tIngresso(NOLOCK) WHERE ID = " + ingressoID;
                    if (bd.Comando != null && !bd.Consulta().IsClosed)
                        bd.Consulta().Close();//fecha a consulta para garantir a execucao da select
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        //popula o objeto ingresso com as informações nescessarias para a reserva
                        ingresso = new Ingresso();

                        DataRow[] linhaAux = itensPacote.Select("ApresentacaoSetorID = " + bd.LerInt("ApresentacaoSetorID"));
                        precoID = (int)linhaAux[0]["PrecoID"];
                        cortesiaID = (int)linhaAux[0]["CortesiaID"];

                        apresentacaoID = bd.LerInt("ApresentacaoID");
                        eventoID = bd.LerInt("EventoID");

                        string codigo = bd.LerString("Codigo");
                        ingresso.Control.ID = ingressoID;
                        ingresso.Codigo.Valor = codigo;
                        ingresso.EventoID.Valor = eventoID;
                        ingresso.ApresentacaoID.Valor = apresentacaoID;
                        ingresso.SetorID.Valor = bd.LerInt("SetorID");

                        ingresso.PrecoID.Valor = precoID;

                        ingresso.UsuarioID.Valor = usuarioID;
                        ingresso.PacoteGrupo.Valor = pacoteGrupo.ToString();
                        ingresso.LojaID.Valor = lojaID;
                        ingresso.TimeStampReserva.Valor = timeStamp;
                        ingresso.PacoteID.Valor = assinaturaInfo.PacoteID;

                        //insere a string para a reserva dos ingressos
                        sqlReservar.Append(ingresso.StringReservar() + " ");

                        //Popula as tabelas de retorno
                        #region Insere no grid exibido para o operador
                        DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                        decimal valorPacote = (decimal)itensPacote.Compute("SUM(ValorItem)", "1=1");

                        novoItemGrid[EVENTO_PACOTE] = (string)itensPacote.Rows[0]["PacoteNome"];
                        novoItemGrid[SETOR_PRODUTO] = (string)itensPacote.Rows[0]["SetorNome"];
                        novoItemGrid[LUGAR_MARCADO] = false;
                        novoItemGrid[VALOR] = valorPacote;
                        novoItemGrid[TIPO] = TIPO_PACOTE;
                        novoItemGrid[TIPO_LUGAR] = "P"; //esse método só é utilizado para a venda de pacote pista na importacao de assinatura

                        novoItemGrid[CODIGO] = codigo;
                        //adiciona na tabela grid
                        retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);
                        #endregion

                        #region Insere na tabela de itens reservados

                        DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                        novoItemReserva[TIPO] = TIPO_INGRESSO;
                        novoItemReserva[RESERVAID] = novoItemGrid[RESERVAID];
                        novoItemReserva[LUGARID] = bd.LerInt("LugarID");
                        novoItemReserva[PRECOID] = precoID;
                        novoItemReserva[CORTESIAID] = cortesiaID;
                        novoItemReserva[BLOQUEIOID] = 0;
                        novoItemReserva[INGRESSOID] = ingressoID;
                        novoItemReserva[APRESENTACAOID] = apresentacaoID;
                        novoItemReserva[EVENTOID] = eventoID;
                        //adiciona na tabela Reseva
                        retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                        #endregion





                    }//While da consulta de ingressos
                }//for de ingressosID
                if (bd.Comando != null && !bd.Consulta().IsClosed)
                    bd.Consulta().Close();//fecha a consulta para garantir a execucao da select
                int x = bd.Executar(sqlReservar.ToString());
                bool okR = (x == ingressosID.Count);
                if (!okR)
                {
                    bd.DesfazerTransacao();
                    return null;
                }
                bd.FinalizarTransacao();

                return retorno;

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

        /// <summary>
        /// Reserva ingresso com lugar marcado
        /// </summary>
        /// <returns></returns>
        public DataSet ReservarLugarMarcado(DataTable lugares, int lojaID, int usuarioID, int canalID, int eventoID, int apresentacaoID, int apresentacaoSetorID,
            int precoID, string precoNome, int cortesiaID, int seedReserva, bool venderBloqueios, bool buscaTaxasEntrega,
            bool buscaFormaPagamento, bool preReserva, int qtdeJaReservadaCota,
             EstruturaCotasInfo cotasInfo, int qtdeJaReservadaCotaItem, int serieID, bool itemPromocional, int quantidadePorPromocional)
        {
            BD bd = new BD();
            Ingresso oIngresso = new Ingresso();
            Cota oCota = new Cota();
            CotaItemControle oCotaItemControle = new CotaItemControle();
            bool reservar = (lugares.Rows.Count > 0 && apresentacaoSetorID != 0 && (precoID != 0 || preReserva));

            if (!reservar)
                return null;

            // Desconsiderar preço quando é reserva de pré-reserva
            if (preReserva)
            {
                precoID = 0;
                cortesiaID = 0;
            }

            //retorna um DataSet com duas tabelas para impressao, uma de ingressos normais e outra de ingressos do pacote
            //e uma tabela de reservas (informaçoes para a Grid)
            DataSet retorno = estruturaReservas(seedReserva);

            CodMensagem codMsg = CodMensagem.Nulo;
            AlertaMensagem alertaMsg = AlertaMensagem.Nulo;

            try
            {
                Apresentacao apresentacao = new Apresentacao();
                apresentacao.Control.ID = apresentacaoID;

                if (!apresentacao.Venda())
                {
                    alertaMsg = AlertaMensagem.Exclamacao;
                    throw new BilheteriaException("Apresentação não está mais a venda. Escolha outra apresentação.", CodMensagemReserva.ApresentacaoIndisponivel);
                }

                //verificar o tipo de lugar. De acordo com ele, as verificacoes futuras qto a preco sera baseada no tipo de lugar
                string tipoLugar = (string)lugares.Rows[0][TIPO_LUGAR];

                int qtdeIngresso = 0;
                foreach (DataRow lugar in lugares.Rows)
                    qtdeIngresso += (int)lugar[QTDE_RESERVAR];

                //qtdes de preço
                int qtdePrecoDisponivel = 0; //qtde de preços disponiveis, q podem vender, independente do canal
                int qtdePrecoPorCliente = 0; //qtde de preços q podem vender por cliente independente do canal
                int qtdePrecoPorCanal = 0; //qtde de preços q podem vender por canal

                int qtdePrecoVendido = 0; //qtde de ingressos vendidos com esse preço
                int qtdePrecoVendidaPorCliente = 0; //qtde de ingressos vendidos com esse preço

                int qtdeIngressosQPodeReservar = qtdeIngresso; //qtde de ingressos a ser reservado

                int qtdeMaxIngressosQPodeReservarPorPreco = 0; //qtde maxima q pode reservar
                int qtdeMaxIngressosQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal
                int qtdeMaxIngressosQPodeReservarPorCliente = 0; //qtde maxima q pode reservar por cliente
                int qtdeCanalVendido = 0; //Quantidade já vendida do canal
                int qtdeIngressosReservada = 0; //qtde de ingressos realmente reservada

                int validadePreco; //dias q faltam para vender os ingressos (se maior que zero entao pode vender)

                //bool buscaFormaPagamentoCota = cotaItemID == 0 ? true : false;
                bool cotaExcedida = false;

                Evento evento = new Evento();
                evento.Ler(eventoID);

                if (!preReserva)
                {
                    //1. alimenta qtdes de preço
                    //1. alimenta qtdes de preço
                    Preco preco = new Preco();
                    Preco.InfoToReserva infoPreco = preco.LerToReserva(precoID, canalID, serieID, bd);

                    if (infoPreco.Preco == null || infoPreco.Preco.Control.ID == 0 || (infoPreco.CanalPrecoID == 0 && serieID == 0))
                    {
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Preço não está mais à venda nesse canal. Escolha outro preço.", CodMensagemReserva.PrecoIndisponivel);
                    }
                    else if (serieID > 0 && !infoPreco.DisponivelSerie)
                    {
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Preço não está mais à venda nesse canal ou foi removido da série selecionada. Escolha outro preço.", CodMensagemReserva.PrecoIndisponivel);
                    }

                    qtdePrecoDisponivel = infoPreco.Preco.Quantidade.Valor;
                    qtdePrecoPorCliente = infoPreco.Preco.QuantidadePorCliente.Valor;
                    qtdePrecoVendido = infoPreco.QtdVendidoPreco;
                    qtdeCanalVendido = infoPreco.QtdVendidoCanal;

                    //2. verificar a qtde total de preço independente do canal
                    if (qtdePrecoDisponivel != 0)
                    {
                        //2.1 verificar qtde de ingressos vendidos a esse preço.
                        //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                        if (qtdePrecoVendido >= qtdePrecoDisponivel)
                        {
                            codMsg = CodMensagem.PorPreco;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Esse preço não pode mais ser vendido.\nJá foi toda sua quota de " + qtdePrecoDisponivel + " ingressos!");
                        }

                        qtdeMaxIngressosQPodeReservarPorPreco = qtdePrecoDisponivel - qtdePrecoVendido;

                        if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorPreco)
                            qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorPreco;
                    }

                    //atribuir a qtde de preço ja vendida nao estourou qtde disponivel no canal
                    qtdePrecoPorCanal = infoPreco.QtdDisponivelCanal;	/// canalPreco.QuantidadeDisponivel();
                    //atribuir a data limite para venda dos ingressos
                    validadePreco = infoPreco.ValidadeCanal;	/// canalPreco.Validade();

                    if (validadePreco == 0)
                    {
                        //expirou a data!
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Data de validade para venda desse preço expirou!\n\nValidade: " + infoPreco.DataFim.ToShortDateString() + "\nHoje: " + System.DateTime.Now.ToShortDateString());
                    }

                    //3.1 se Quantidade for zero, esta liberado para vender a vontade!
                    if (qtdePrecoPorCanal != 0)
                    {
                        //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender ingresso(s)
                        if (qtdeCanalVendido >= qtdePrecoPorCanal)
                        {
                            codMsg = CodMensagem.PorCanal;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Esse preço não está mais disponível no seu canal.\nJá foi vendida toda sua quota de " + qtdePrecoPorCanal + " ingressos!");
                        }

                        qtdeMaxIngressosQPodeReservarPorCanal = qtdePrecoPorCanal - qtdePrecoVendido;

                        if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCanal)
                            qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCanal;
                    }

                    //4. verificar a qtde q pode vender por cliente independente do canal
                    if (qtdePrecoPorCliente != 0)
                    {
                        qtdeMaxIngressosQPodeReservarPorCliente = qtdePrecoPorCliente - qtdePrecoVendidaPorCliente;

                        if (qtdeIngressosQPodeReservar > qtdeMaxIngressosQPodeReservarPorCliente)
                            qtdeIngressosQPodeReservar = qtdeMaxIngressosQPodeReservarPorCliente;

                    }

                    #region Encontra CotaItem da Apresentacao
                    if (cotasInfo.TemCotaItem && !cotasInfo.EncontrouCotaItem)
                    {
                        DataTable dtt = new Cota().getDttCotaItemPorCotaID(cotasInfo.CotaID_Apresentacao);
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            //if (precoNome.StartsWith(dtt.Rows[i]["PrecoIniciaCom"].ToString()))
                            if (precoNome.ToLower().StartsWith(dtt.Rows[i][PRECO_INICIA_COM].ToString().ToLower()))
                            {
                                cotasInfo.Preencher(false, dtt.Rows[i]);
                                cotasInfo.EncontrouCotaItem = true;
                                break;
                            }
                        }
                        retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);
                    }
                    else if (cotasInfo.CotaItemID > 0)
                        cotasInfo.EncontrouCotaItem = true;
                    #endregion

                    #region Encontra CotaItem da ApresentacaoSetor
                    if (cotasInfo.TemCotaItemAPS && !cotasInfo.EncontrouCotaItemAPS && cotasInfo.CotaID_Apresentacao != cotasInfo.CotaID_ApresentacaoSetor)
                    {
                        DataTable dtt = new Cota().getDttCotaItemPorCotaID(cotasInfo.CotaID_ApresentacaoSetor);
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            //if (precoNome.StartsWith(dtt.Rows[i]["PrecoIniciaCom"].ToString()))
                            if (precoNome.ToLower().StartsWith(dtt.Rows[i][PRECO_INICIA_COM].ToString().ToLower()))
                            {
                                cotasInfo.Preencher(true, dtt.Rows[i]);
                                cotasInfo.EncontrouCotaItemAPS = true;
                                break;
                            }
                        }
                        retorno.Tables[TABELA_COTA_ITEM].Merge(dtt);
                    }
                    else if (cotasInfo.CotaID_ApresentacaoSetor == cotasInfo.CotaID_Apresentacao)
                        cotasInfo.CotaItemID_APS = 0;
                    else if (cotasInfo.CotaItemID > 0)
                        cotasInfo.EncontrouCotaItemAPS = true;
                    #endregion

                    if (cotasInfo.CotaItemID > 0 && cotasInfo.CotaItemID_APS > 0 && cotasInfo.CotaItemID_APS != cotasInfo.CotaItemID
                        && (cotasInfo.ValidaBin != cotasInfo.ValidaBinAPS || cotasInfo.ParceiroID != cotasInfo.ParceiroIDAPS))
                    {
                        codMsg = CodMensagem.PorCota;
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Este preço possui mais de uma promoção cadastrada. Por favor, entre em contato com a equipe de atendimento.");
                    }

                    //Validacao de Cotas
                    if (cotasInfo.CotaItemID > 0 || cotasInfo.CotaItemID_APS > 0)
                    {
                        cotasInfo.QuantidadeJaVendida = new CotaItemControle().getQuantidadeNovo(cotasInfo.CotaItemID, cotasInfo.CotaItemID_APS, apresentacaoID, apresentacaoSetorID);
                        cotasInfo.sumQuantidadeVendidaReservar(qtdeJaReservadaCota, qtdeJaReservadaCotaItem);

                        //Fez a soma dos itens agora faz a validacao
                        while (!cotasInfo.ValidaReserva(qtdeIngressosQPodeReservar) && qtdeIngressosQPodeReservar > 0)
                        {
                            cotaExcedida = true;
                            qtdeIngressosQPodeReservar--;
                        }
                        if (qtdeIngressosQPodeReservar <= 0)
                        {
                            codMsg = CodMensagem.PorCota;
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("O Limite de Venda/Reserva do preço " + precoNome + " foi excedido.");
                        }
                    }

                    if (qtdeIngressosQPodeReservar < qtdeIngresso)
                    {
                        alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Limite de preços esgotado.");

                    }
                    //se a qtdeIngressosQPodeReservar for diferente da qtdeIngresso,
                    //temos q eliminar alguns lugares pra reservar apenas a qtdeIngressosQPodeReservar
                    if (qtdeIngressosQPodeReservar != qtdeIngresso)
                    {

                        int qtdeIngressosAReservar = 0;
                        for (int i = lugares.Rows.Count - 1; i >= 0; i--)
                        {
                            //foreach(DataRow lugar in lugares.Rows) {

                            int qtde = (int)lugares.Rows[i][QTDE_RESERVAR];
                            qtdeIngressosAReservar += qtde;

                            if (qtdeIngressosAReservar > qtdeIngressosQPodeReservar)
                            {
                                //eliminar lugar
                                //qtdeIngressosAReservar -= qtde;
                                //lugar[QTDE_RESERVAR] = 0;
                                //lugar[RESERVADO] = false;
                                //lugar[STATUS] = Ingresso.DISPONIVEL;
                                lugares.Rows.RemoveAt(i);
                            }
                        }
                    }
                }

                //soh os lugares q podemos reservar
                string lugarEsgotado;
                if (tipoLugar == Setor.Cadeira)
                    lugarEsgotado = "Cadeira não está mais disponível.";
                else
                    lugarEsgotado = "Mesa não está mais disponível.";

                foreach (DataRow lugar in lugares.Rows)
                {

                    lugar[QTDE_RESERVADO] = 0;
                    lugar[RESERVADO] = false;

                    int taxaConveniencia = 0;
                    int taxaComissao = 0;
                    decimal taxaMin = 0;
                    decimal taxaMax = 0;
                    decimal ComissaoMin = 0;
                    decimal ComissaoMax = 0;

                    // Em caso de pré-reserva não deve ser consideradas as taxas e comissões
                    if (!preReserva)
                    {
                        CanalEvento canalEvento = new CanalEvento();
                        DataTable taxas = canalEvento.BuscaTaxasConvenienciaComissao(canalID, eventoID);

                        taxaConveniencia = (int)taxas.Rows[0]["TaxaConveniencia"];
                        taxaComissao = (int)taxas.Rows[0]["TaxaComissao"];
                        taxaMin = Convert.ToDecimal(taxas.Rows[0]["TaxaMinima"]);
                        taxaMax = Convert.ToDecimal(taxas.Rows[0]["TaxaMaxima"]);
                        ComissaoMin = Convert.ToDecimal(taxas.Rows[0]["ComissaoMinima"]);
                        ComissaoMax = Convert.ToDecimal(taxas.Rows[0]["ComissaoMaxima"]);
                    }

                    int qtde = (int)lugar[QTDE_RESERVAR];
                    int lugarID = (int)lugar[LUGARID];

                    IngressoLista listaIngressos = new IngressoLista();
                    listaIngressos.FiltroSQL = "ApresentacaoSetorID=" + apresentacaoSetorID;
                    listaIngressos.FiltroSQL = "LugarID=" + lugarID;
                    if (!venderBloqueios)
                        listaIngressos.FiltroSQL = "(Status='" + Ingresso.DISPONIVEL + "')";
                    else
                        listaIngressos.FiltroSQL = "(Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "')";
                    listaIngressos.OrdemSQL = "Codigo";
                    listaIngressos.TamanhoMax = qtde;
                    listaIngressos.NoLock = true;
                    listaIngressos.Carregar();

                    //se for mesa fechada tem q reservar todo o lugar ou nenhum
                    if (tipoLugar != Setor.MesaFechada)
                    {
                        #region cadeira ou mesa-aberta

                        if (!listaIngressos.Primeiro())
                        {
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Lugar não está mais disponível!");
                        }

                        int qtdeIngressosReservadaNesseLugar = 0;

                        listaIngressos.NoLock = false;

                        //iniciar LOOP
                        ArrayList ids = new ArrayList();
                        do
                        {
                            //atribui informacoes aos ingressos antes de chamar metodo reservar
                            listaIngressos.Ingresso.UsuarioID.Valor = usuarioID;
                            listaIngressos.Ingresso.PrecoID.Valor = precoID;
                            listaIngressos.Ingresso.CortesiaID.Valor = cortesiaID;

                            if ((Enumerators.TipoCodigoBarra)Convert.ToChar(evento.TipoCodigoBarra.Valor) != Enumerators.TipoCodigoBarra.ListaBranca)
                                listaIngressos.Ingresso.CodigoBarra.Valor = string.Empty;

                            listaIngressos.Ingresso.LojaID.Valor = lojaID;

                            bool ok = listaIngressos.Ingresso.Reservar(); //faz a reserva

                            if (ok)
                            {
                                //add ingresso reservado

                                qtdeIngressosReservada++;
                                qtdeIngressosReservadaNesseLugar++;
                                ids.Add(listaIngressos.Ingresso.Control.ID);
                            }

                        } while (listaIngressos.Proximo());

                        ids.TrimToSize();
                        if (ids.Count > 0)
                        {

                            DataTable reservados = listaIngressos.Ingresso.InfoReservados((int[])ids.ToArray(typeof(int)), bd, lojaID);
                            bd.Fechar();


                            foreach (DataRow linha in reservados.Rows)
                            {
                                DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                                novoItemGrid[PRECOID] = precoID;
                                novoItemGrid[APRESENTACAOSETORID] = apresentacaoSetorID;
                                novoItemGrid[LOCALID] = linha["LocalID"];
                                novoItemGrid[OBRIGA_CADASTRO_CLIENTE] = (string)linha["ObrigaCadastroCliente"];
                                novoItemGrid[EVENTO_PACOTE] = (string)linha["Evento"];
                                novoItemGrid[HORARIO] = (DateTime)linha["Horario"];
                                novoItemGrid[SETOR_PRODUTO] = (string)linha["Setor"];
                                novoItemGrid[CODIGO] = (string)linha["Codigo"];
                                novoItemGrid[LUGAR_MARCADO] = true;
                                novoItemGrid[PRECO] = (string)linha["Preco"];
                                if ((decimal)linha["Valor"] == 0) //eh cortesia
                                    novoItemGrid[CORTESIA] = (string)linha["Cortesia"];
                                novoItemGrid[VALOR] = (decimal)linha["Valor"];
                                novoItemGrid[TIPO] = TIPO_INGRESSO;
                                novoItemGrid[TIPO_LUGAR] = tipoLugar;
                                novoItemGrid[CONV] = taxaConveniencia;
                                if (cotasInfo.CotaItemID > 0 || cotasInfo.CotaItemID_APS > 0)
                                {
                                    novoItemGrid[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                                    novoItemGrid[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;
                                    novoItemGrid[COTA] = "Sim";
                                    novoItemGrid[CLIENTE] = " - ";
                                }
                                else
                                {
                                    novoItemGrid[COTA_ITEM_ID] = 0;
                                    novoItemGrid[COTA] = "Não";
                                }

                                if (serieID > 0)
                                    novoItemGrid[SERIE] = "Sim";

                                //popula a taxa de conveniencia de acordo com a taxa minima e maxima
                                if (taxaConveniencia > 0 || taxaMin > 0)
                                {
                                    decimal dAux = (taxaConveniencia / 100m) * (decimal)linha["Valor"];
                                    if (dAux < taxaMin)
                                    {
                                        novoItemGrid[VALOR_CONV] = Decimal.Round(taxaMin, 2);
                                        novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                        novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                        linha["TaxaConveniencia"] = taxaConveniencia;
                                        linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                                    }
                                    else if (dAux > taxaMax && taxaMax > 0)
                                    {
                                        novoItemGrid[VALOR_CONV] = Decimal.Round(taxaMax, 2);
                                        novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                        novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                        linha["TaxaConveniencia"] = taxaConveniencia;
                                        linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                                    }
                                    else
                                    {
                                        novoItemGrid[VALOR_CONV] = Decimal.Round(dAux, 2);
                                        novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                        novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                        linha["TaxaConveniencia"] = taxaConveniencia;
                                        linha["TaxaConvenienciaValor"] = (decimal)novoItemGrid[VALOR_CONV];
                                    }
                                }

                                //popula a comissao de acordo com a taxa minima e maxima
                                if (taxaComissao > 0 || ComissaoMin > 0)
                                {
                                    decimal dAux = (taxaComissao / 100m) * (decimal)linha["Valor"];
                                    if (dAux < ComissaoMin)
                                    {
                                        novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                        novoItemGrid[COMISSAO_VALOR] = ComissaoMin;
                                    }
                                    else if (dAux > ComissaoMax && ComissaoMax > 0)
                                    {
                                        novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                        novoItemGrid[COMISSAO_VALOR] = ComissaoMax;
                                    }
                                    else
                                    {
                                        novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                        novoItemGrid[COMISSAO_VALOR] = dAux;
                                    }
                                }

                                retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);

                                DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                                novoItemReserva[TIPO] = TIPO_INGRESSO;
                                novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                                novoItemReserva[LUGARID] = 0;
                                novoItemReserva[PACOTEID] = 0;
                                novoItemReserva[PRECOID] = precoID;
                                novoItemReserva[CORTESIAID] = cortesiaID;
                                novoItemReserva[BLOQUEIOID] = (int)linha["BloqueioID"];
                                novoItemReserva[INGRESSOID] = (int)linha["ID"];
                                novoItemReserva[APRESENTACAOID] = apresentacaoID;
                                novoItemReserva[EVENTOID] = (int)linha["EventoID"];
                                novoItemReserva[CODIGO_SEQUENCIAL] = (int)linha["CodigoSequencial"];
                                novoItemReserva[CLIENTE_ID] = 0;
                                novoItemReserva[OBRIGATORIEDADE_ID] = (int)linha[OBRIGATORIEDADE_ID];
                                novoItemReserva[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                                novoItemReserva[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;

                                novoItemReserva[QUANTIDADE_COTA] = cotasInfo.QuantidadeCota;
                                novoItemReserva[QUANTIDADEPORCLIENTE_COTA] = cotasInfo.QuantidadePorClienteCota;

                                novoItemReserva[QUANTIDADE_COTA_APS] = cotasInfo.QuantidadeCotaAPS;
                                novoItemReserva[QUANTIDADEPORCLIENTE_COTA_APS] = cotasInfo.QuantidadePorClienteCotaAPS;

                                novoItemReserva[QUANTIDADE_APRESENTACAO] = cotasInfo.QuantidadeApresentacao;
                                novoItemReserva[QUANTIDADE_APRESENTACAO_SETOR] = cotasInfo.QuantidadeApresentacaoSetor;

                                novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO] = cotasInfo.QuantidadePorClienteApresentacao;
                                novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = cotasInfo.QuantidadePorClienteApresentacaoSetor;

                                novoItemReserva[NOMINAL] = cotasInfo.Nominal;
                                novoItemReserva[TIPO_CODIGO_BARRA] = linha[TIPO_CODIGO_BARRA].ToString();
                                novoItemReserva[CODIGO_BARRA] = linha[CODIGO_BARRA].ToString();

                                if (cotasInfo.ValidaBin || cotasInfo.ValidaBinAPS)
                                {
                                    novoItemReserva[VALIDA_BIN] = true;
                                    novoItemReserva[VALIDA_BIN_APS] = true;
                                }
                                else
                                {
                                    novoItemReserva[VALIDA_BIN] = false;
                                    novoItemReserva[VALIDA_BIN_APS] = false;
                                }

                                novoItemReserva[OBRIGATORIEDADE_ID] = (int)linha[OBRIGATORIEDADE_ID];
                                novoItemReserva[APRESENTACAOSETORID] = apresentacaoSetorID;
                                novoItemReserva[SERIEID] = serieID;
                                novoItemReserva[ITEM_PROMOCIONAL] = itemPromocional;
                                novoItemReserva[QUANTIDADE_POR_PROMOCIONAL] = quantidadePorPromocional;

                                retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                                retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].ImportRow(linha);
                            }

                            lugar[RESERVADO] = true;
                            lugar[QTDE_RESERVADO] = qtdeIngressosReservadaNesseLugar;
                        }
                        #endregion
                    }
                    else
                    {
                        #region mesa fechada
                        if (listaIngressos.Tamanho != qtde)
                        {
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Mesa não está mais disponível!");
                        }

                        if (!listaIngressos.Primeiro())
                        {
                            alertaMsg = AlertaMensagem.Exclamacao;
                            throw new BilheteriaException("Mesa não está mais disponível!");
                        }

                        int qtdeIngressosReservadaNesseLugar = 0;

                        bool ok = true;

                        ArrayList ids = new ArrayList();
                        do
                        {
                            //atribui informacoes aos ingressos antes de chamar metodo reservar
                            listaIngressos.Ingresso.UsuarioID.Valor = usuarioID;
                            listaIngressos.Ingresso.PrecoID.Valor = precoID;
                            listaIngressos.Ingresso.CortesiaID.Valor = cortesiaID;

                            if ((Enumerators.TipoCodigoBarra)Convert.ToChar(evento.TipoCodigoBarra.Valor) != Enumerators.TipoCodigoBarra.ListaBranca)
                                listaIngressos.Ingresso.CodigoBarra.Valor = string.Empty;

                            ok &= listaIngressos.Ingresso.Reservar(bd, preReserva); //faz a reserva

                            if (ok)
                            {
                                //add ingresso reservado
                                qtdeIngressosReservada++;
                                qtdeIngressosReservadaNesseLugar++;
                                ids.Add(listaIngressos.Ingresso.Control.ID);
                            }
                            else
                            {
                                //nao pode reservar lugar, cancela esse lugar
                                while (listaIngressos.Anterior())
                                {
                                    listaIngressos.Ingresso.CancelarReserva(usuarioID);
                                }

                                //ok = false;
                                qtdeIngressosReservada -= qtdeIngressosReservadaNesseLugar;
                                qtdeIngressosReservadaNesseLugar = 0;
                                //novoItemGrid.Delete();
                                break;
                            }

                        } while (listaIngressos.Proximo());

                        if (ok)
                        {


                            ids.TrimToSize();
                            DataTable reservados = listaIngressos.Ingresso.InfoReservados((int[])ids.ToArray(typeof(int)), bd, lojaID);

                            if (reservados.Rows.Count > 0)
                            {

                                decimal valorMesa = 0;
                                decimal taxaValorMesaFechada = 0;
                                decimal taxaComissaoValorMesaFechada = 0;

                                foreach (DataRow linha in reservados.Rows)
                                {

                                    valorMesa += (decimal)linha["Valor"];
                                    //popula o valor da comissão de acordo com a taxa minima e maxima
                                    if (taxaComissao > 0 || ComissaoMin > 0)
                                    {
                                        decimal dAux = (taxaComissao / 100m) * (decimal)linha["Valor"];

                                        if (dAux < ComissaoMin)
                                        {
                                            taxaComissaoValorMesaFechada += Decimal.Round(ComissaoMin, 2);
                                        }
                                        else if (dAux > ComissaoMax && ComissaoMax > 0)
                                        {
                                            taxaComissaoValorMesaFechada += Decimal.Round(ComissaoMax, 2);
                                        }
                                        else
                                        {
                                            taxaComissaoValorMesaFechada += Decimal.Round(dAux, 2);
                                        }
                                    }
                                    //popula o valor da taxa de acordo com a taxa minima e maxima
                                    if (taxaConveniencia > 0 || taxaMin > 0)
                                    {
                                        decimal dAux = (taxaConveniencia / 100m) * (decimal)linha["Valor"];

                                        if (dAux < taxaMin)
                                        {
                                            taxaValorMesaFechada += Decimal.Round(taxaMin, 2);
                                        }
                                        else if (dAux > taxaMax && taxaMax > 0)
                                        {
                                            taxaValorMesaFechada += Decimal.Round(taxaMax, 2);
                                        }
                                        else
                                        {
                                            taxaValorMesaFechada += Decimal.Round(dAux, 2);
                                        }
                                    }

                                }

                                DataRow novoItemGrid = retorno.Tables[TABELA_GRID].NewRow();

                                novoItemGrid[PRECOID] = precoID;
                                novoItemGrid[APRESENTACAOSETORID] = apresentacaoSetorID;
                                novoItemGrid[EVENTO_PACOTE] = (string)reservados.Rows[0]["Evento"];
                                novoItemGrid[HORARIO] = (DateTime)reservados.Rows[0]["Horario"];
                                novoItemGrid[SETOR_PRODUTO] = (string)reservados.Rows[0]["Setor"];
                                string cod = (string)reservados.Rows[0]["Codigo"];
                                try
                                {

                                    if (cod.IndexOf("-") != -1)
                                        novoItemGrid[CODIGO] = cod.Substring(0, cod.IndexOf("-"));
                                    else
                                        novoItemGrid[CODIGO] = cod;
                                }
                                catch
                                {
                                    novoItemGrid[CODIGO] = cod;
                                }
                                novoItemGrid[LUGAR_MARCADO] = true;
                                novoItemGrid[PRECO] = (string)reservados.Rows[0]["Preco"];
                                if (valorMesa == 0) //eh cortesia
                                    novoItemGrid[CORTESIA] = (string)reservados.Rows[0]["Cortesia"];
                                novoItemGrid[TIPO] = TIPO_INGRESSO;
                                novoItemGrid[TIPO_LUGAR] = Setor.MesaFechada;
                                novoItemGrid[CONV] = taxaConveniencia;
                                novoItemGrid[VALOR] = valorMesa;
                                //Na mesa fechada a taxa mínima e máxima é calculada em cima de cada lugar.
                                novoItemGrid[VALOR_CONV] = Decimal.Round(taxaValorMesaFechada, 2);
                                novoItemGrid[CONV_MIN] = Decimal.Round(taxaMin, 2);
                                novoItemGrid[CONV_MAX] = Decimal.Round(taxaMax, 2);
                                novoItemGrid[TAXA_COMISSAO] = taxaComissao;
                                novoItemGrid[COMISSAO_VALOR] = taxaComissaoValorMesaFechada;
                                if (cotasInfo.CotaItemID > 0 || cotasInfo.CotaItemID_APS > 0)
                                {
                                    novoItemGrid[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                                    novoItemGrid[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;

                                    novoItemGrid[COTA] = "Sim";
                                    novoItemGrid[CLIENTE] = " - ";

                                }
                                else
                                {
                                    novoItemGrid[COTA_ITEM_ID] = 0;
                                    novoItemGrid[COTA] = "Não";
                                }
                                retorno.Tables[TABELA_GRID].Rows.Add(novoItemGrid);

                                foreach (DataRow linha in reservados.Rows)
                                {

                                    DataRow novoItemReserva = retorno.Tables[TABELA_RESERVA].NewRow();

                                    novoItemReserva[TIPO] = TIPO_INGRESSO;
                                    novoItemReserva[RESERVAID] = (int)novoItemGrid[RESERVAID];
                                    novoItemReserva[LUGARID] = lugarID;
                                    novoItemReserva[PACOTEID] = 0;
                                    novoItemReserva[PRECOID] = precoID;
                                    novoItemReserva[CORTESIAID] = cortesiaID;
                                    novoItemReserva[BLOQUEIOID] = (int)linha["BloqueioID"];
                                    novoItemReserva[INGRESSOID] = (int)linha["ID"];
                                    novoItemReserva[APRESENTACAOID] = apresentacaoID;
                                    novoItemReserva[EVENTOID] = (int)linha["EventoID"];
                                    novoItemReserva[CODIGO_SEQUENCIAL] = (int)linha["CodigoSequencial"];
                                    novoItemReserva[CLIENTE_ID] = 0;
                                    novoItemReserva[OBRIGATORIEDADE_ID] = (int)linha["ObrigatoriedadeID"];
                                    novoItemReserva[COTA_ITEM_ID] = cotasInfo.CotaItemID;
                                    novoItemReserva[COTA_ITEM_ID_APS] = cotasInfo.CotaItemID_APS;

                                    novoItemReserva[QUANTIDADE_COTA] = cotasInfo.QuantidadeCota;
                                    novoItemReserva[QUANTIDADEPORCLIENTE_COTA] = cotasInfo.QuantidadePorClienteCota;

                                    novoItemReserva[QUANTIDADE_COTA_APS] = cotasInfo.QuantidadeCotaAPS;
                                    novoItemReserva[QUANTIDADEPORCLIENTE_COTA_APS] = cotasInfo.QuantidadePorClienteCotaAPS;

                                    novoItemReserva[QUANTIDADE_APRESENTACAO] = cotasInfo.QuantidadeApresentacao;
                                    novoItemReserva[QUANTIDADE_APRESENTACAO_SETOR] = cotasInfo.QuantidadeApresentacaoSetor;

                                    novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO] = cotasInfo.QuantidadePorClienteApresentacao;
                                    novoItemReserva[QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = cotasInfo.QuantidadePorClienteApresentacaoSetor;

                                    novoItemReserva[VALIDA_BIN] = cotasInfo.ValidaBin;
                                    novoItemReserva[VALIDA_BIN] = cotasInfo.ValidaBinAPS;

                                    novoItemReserva[NOMINAL] = cotasInfo.Nominal;

                                    novoItemReserva[APRESENTACAOSETORID] = apresentacaoSetorID;
                                    novoItemReserva[TIPO_CODIGO_BARRA] = linha[TIPO_CODIGO_BARRA].ToString();
                                    novoItemReserva[CODIGO_BARRA] = linha[CODIGO_BARRA].ToString();
                                    //popula a taxa de acordo com a taxa minima e maxima
                                    if (taxaConveniencia > 0 || taxaMin > 0)
                                    {
                                        decimal dAux = (taxaConveniencia / 100m) * (decimal)linha["Valor"];

                                        if (dAux < taxaMin)
                                        {
                                            linha["TaxaConveniencia"] = taxaConveniencia;
                                            linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMin, 2);
                                        }
                                        else if (dAux > taxaMax && taxaMax > 0)
                                        {

                                            linha["TaxaConveniencia"] = taxaConveniencia;
                                            linha["TaxaConvenienciaValor"] = Decimal.Round(taxaMax, 2);
                                        }
                                        else
                                        {
                                            linha["TaxaConveniencia"] = taxaConveniencia;
                                            linha["TaxaConvenienciaValor"] = Decimal.Round(dAux, 2);
                                        }
                                    }


                                    retorno.Tables[TABELA_RESERVA].Rows.Add(novoItemReserva);
                                    retorno.Tables[TABELA_ESTRUTURA_IMPRESSAO].ImportRow(linha);
                                }


                            }

                            lugar[QTDE_RESERVADO] = qtdeIngressosReservadaNesseLugar;
                            if (qtdeIngressosReservadaNesseLugar == qtde)
                            {

                                lugar[RESERVADO] = true;
                                //	lugar[STATUS] = Ingresso.RESERVADO;
                            }

                        }//fim ok de add mesa

                        #endregion
                    } //fim if (tipoLugar == Setor.MesaFechada){ 

                }//fim do for

                retorno.Tables.Remove(TABELA_RESERVA_LUGAR_MARCADO);
                retorno.Tables.Add(lugares.Copy());

                string msg = null;

                int qtdePedida = (tipoLugar == Setor.MesaFechada) ? lugares.Rows.Count : qtdeIngresso;
                int qtdeReservada = retorno.Tables[TABELA_GRID].Rows.Count; //qtde de lugares realmente reservada

                if (qtdePedida != qtdeReservada)
                {
                    alertaMsg = AlertaMensagem.Exclamacao;

                    if (qtdeReservada == 0)
                    {
                        msg = lugarEsgotado;
                    }
                    else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorPreco)
                    {
                        codMsg = CodMensagem.PorPreco;
                        msg = (qtdeReservada == 1) ?
                            "Somente um lugar foi reservado!\nA quota de ingressos nesse preço já estourou." :
                            "Somente " + qtdeReservada + " lugares foram reservados!\nA quota de ingressos nesse preço já estourou.";
                    }
                    else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCanal)
                    {
                        codMsg = CodMensagem.PorCanal;
                        msg = (qtdeReservada == 1) ?
                            "Somente um lugar foi reservado!\nA quota de ingressos no seu canal já estourou." :
                            "Somente " + qtdeReservada + " lugares foram reservados!\nA quota de ingressos no seu canal já estourou.";
                    }
                    else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCliente)
                    {
                        codMsg = CodMensagem.PorCliente;
                        msg = (qtdeReservada == 1) ?
                            "Somente um lugar foi reservado!\nA quota de ingressos por cliente já estourou." :
                            "Somente " + qtdeReservada + " lugares foram reservados!\nA quota de ingressos por cliente já estourou.";
                    }
                    else if (cotaExcedida)
                    {
                        codMsg = CodMensagem.PorPreco;
                        msg = (qtdeReservada == 1) ?
                            "Somente um lugar foi reservado!\nO limite de ingressos para este preço foi atingido." :
                            "Somente " + qtdeReservada + " lugares foram reservados!\nO limite de ingressos para este preço foi atingido.";
                    }
                    else
                    {
                        msg = (qtdeReservada == 1) ?
                            "Somente um lugar foi reservado!\nOs demais lugares não estão mais disponíveis." :
                            "Somente " + qtdeReservada + " lugares foram reservados!\nOs demais lugares não estão mais disponíveis.";
                    }
                }
                else
                {
                    //emitir alerta caso...

                    alertaMsg = AlertaMensagem.Alerta;

                    if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorPreco)
                    {
                        codMsg = CodMensagem.PorPreco;
                        //msg = "A quota de ingressos nesse preço já atingiu sua quantidade máxima.";
                    }
                    else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCanal)
                    {
                        codMsg = CodMensagem.PorCanal;
                        //msg = "A quota de ingressos no seu canal já atingiu sua quantidade máxima.";
                    }
                    else if (qtdeIngressosReservada == qtdeMaxIngressosQPodeReservarPorCliente)
                    {
                        codMsg = CodMensagem.PorCliente;
                        //msg = "A quota de ingressos por cliente já atingiu sua quantidade máxima.";
                    }
                }

                if (alertaMsg != AlertaMensagem.Nulo)
                {
                    retorno.Tables[TABELA_MENSAGEM].Rows.Add(new Object[] 
					{ 
						alertaMsg, 
						codMsg, 
						msg, 
						TIPO_INGRESSO 
					});
                }

                if (qtdeIngressosReservada > 0 && buscaTaxasEntrega)
                {
                    // Busca taxas de entrega do Evento
                    DataTable dtEventoTaxaEntrega = new EventoTaxaEntrega().GetTaxas(eventoID);

                    retorno.Tables[TABELA_EVENTO_TAXA_ENTREGA].Merge(dtEventoTaxaEntrega);

                    // Busca regiões de entrega do Evento
                    DataTable dtEventoRegiao = new Regiao().GetRegioes(eventoID);

                    retorno.Tables[TABELA_EVENTO_REGIAO].Merge(dtEventoRegiao);
                }

                if (qtdeIngressosReservada > 0 && buscaFormaPagamento)
                {
                    // Busca formas de pagamento do Evento
                    DataTable dtEventoFormaPagamento = new FormaPagamentoEvento().GetFormasPagamento(eventoID, canalID);

                    retorno.Tables[TABELA_EVENTO_FORMA_PAGAMENTO].Merge(dtEventoFormaPagamento);
                }

                if (cotasInfo.CotaItemID > 0 && cotasInfo.BuscaFormaPagamento)
                {
                    DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(cotasInfo.CotaItemID, canalID);
                    retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);
                    cotasInfo.BuscaFormaPagamento = false;
                }
                if (cotasInfo.CotaItemID > 0 && cotasInfo.BuscaObrigatoriedade)
                {
                    DataTable dttObrigatoriedade = new Obrigatoriedade().getInformacoesPorCotaItem(cotasInfo.CotaItemID);
                    retorno.Tables[TABELA_OBRIGATORIEDADE].Merge(dttObrigatoriedade);
                    cotasInfo.BuscaObrigatoriedade = false;
                }

                if (cotasInfo.CotaItemID_APS > 0 && cotasInfo.BuscaFormaPagamentoAPS && cotasInfo.CotaItemID != cotasInfo.CotaItemID_APS)
                {
                    DataTable dttFormaPagamentoCota = new CotaFormaPagamento().getFormaPagamentoCotaItem(cotasInfo.CotaItemID_APS, canalID);
                    retorno.Tables[TABELA_COTA_ITEM_FORMA_PAGAMENTO].Merge(dttFormaPagamentoCota);
                    cotasInfo.BuscaFormaPagamento = false;
                }
                if (cotasInfo.CotaItemID_APS > 0 && cotasInfo.BuscaObrigatoriedadeAPS && cotasInfo.CotaItemID != cotasInfo.CotaItemID_APS)
                {
                    DataTable dttObrigatoriedade = new Obrigatoriedade().getInformacoesPorCotaItem(cotasInfo.CotaItemID_APS);
                    retorno.Tables[TABELA_OBRIGATORIEDADE].Merge(dttObrigatoriedade);
                    cotasInfo.BuscaObrigatoriedade = false;
                }
            }
            catch (BilheteriaException ex)
            {
                retorno.Tables[TABELA_MENSAGEM].Rows.Add(new Object[] { alertaMsg, codMsg, ex.Message, TIPO_INGRESSO });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;

        }

        /// <summary>
        /// Cancela a reserva dos ingressos passados como parametro. Retorna o sucesso da operacao.
        /// </summary>
        /// <param name="ingressosID">Ingressos a serem cancelados</param>
        /// <returns></returns>
        public int CancelarReservasInternet(int ingressoID, int usuarioID)
        {


            try
            {

                bool ok = false;

                if (ingressoID > 0)
                {

                    //esse metodo vai supor que ingressosIDs contem ingressos reservados. ou seja,
                    //o metodo nao vai verificar se os ingressos estao mesmo com status reservado.
                    //o metodo vai atualizar o status para disponivel e 'zerar' o ingresso.
                    Ingresso ingresso = new Ingresso();

                    ok = ingresso.CancelarReservasInternet(ingressoID, usuarioID);
                }


                if (ok)
                    return 1; //sucesso
                else
                    return 0;

            }
            catch
            {
                return 0;
            }

        }

        public int CancelarReservasVIR(int clienteID, string sessionID)
        {
            try
            {
                ValeIngresso oValeIngresso = new ValeIngresso();
                bool ok = oValeIngresso.CancelarReservasInternet(clienteID, sessionID);

                if (ok)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cancela a reserva dos ingressos passados como parametro. Retorna o sucesso da operacao.
        /// </summary>
        /// <param name="ingressosID">Ingressos a serem cancelados</param>
        /// <returns></returns>
        public int CancelarReservasInternet(int[] ingressos, int usuarioID)
        {
            try
            {
                bool ok = true;

                if (ingressos.Length > 0)
                {
                    Ingresso ingresso = new Ingresso();
                    for (int i = 0; i < ingressos.Length; i++)
                        ok &= ingresso.CancelarReservasInternet(ingressos[i], usuarioID);
                }

                if (ok)
                    return ingressos.Length; //sucesso
                else
                    return -1;

            }
            catch
            {
                return -1;
            }

        }

        /// <summary>
        /// Cancela a reserva dos ingressos passados como parametro. Retorna o sucesso da operacao.
        /// </summary>
        /// <param name="ingressosID">Ingressos a serem cancelados</param>
        /// <returns></returns>
        public int CancelarTodasReservasInternet(int clienteID, string sessionID)
        {
            BD bd = new BD();
            try
            {

                #region Limpar Ingressos SingleTon // INATIVO!!!!
                //                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SingleTonObjectsAtivo"]))
                //                {
                //                    DataTable dttBulk = new DataTable();
                //                    dttBulk.Columns.Add("ID", typeof(int));
                //                    string[] eventoIDs = ConfigurationManager.AppSettings["SpecialEventsID"] != null ? ConfigurationManager.AppSettings["SpecialEventsID"].Split(',') : new string[1];
                //                    DataRow dtr;
                //                    for (int i = 0; i < eventoIDs.Length; i++)
                //                    {
                //                        dtr = dttBulk.NewRow();
                //                        dtr["ID"] = Convert.ToInt32(eventoIDs[i]);
                //                        dttBulk.Rows.Add(dtr);
                //                    }

                //                    if (dttBulk.Rows.Count > 0)
                //                    {

                //                        SingleTonObjects spEvent = SingleTonObjects.RegistrarSpecialEvent();
                //                        if (spEvent.bufferSpecialEvent.IsLoadingQRX)
                //                            Thread.Sleep(5000);

                //                        List<int> precosIDs = new List<int>();

                //                        bd.BulkInsert(dttBulk, "#tempEventos", false, true);

                //                        bd.Consulta(string.Format(@"SELECT PrecoID FROM tIngresso (NOLOCK) 
                //                                    INNER JOIN #tempEventos ON tIngresso.EventoID = #tempEventos.ID
                //                                    WHERE Status = '{0}' AND ClienteID = {1} AND SessionID = '{2}'  ", Ingresso.RESERVADO, clienteID, sessionID));

                //                        while (bd.Consulta().Read())
                //                            precosIDs.Add(bd.LerInt("PrecoID"));

                //                        if (precosIDs.Count > 0)
                //                            spEvent.bufferSpecialEvent.LiberarReserva(precosIDs);

                //                    }
                //                }
                #endregion

                bool ok = false;
                //esse metodo vai supor que ingressosIDs contem ingressos reservados. ou seja,
                //o metodo nao vai verificar se os ingressos estao mesmo com status reservado.
                //o metodo vai atualizar o status para disponivel e 'zerar' o ingresso.
                Ingresso ingresso = new Ingresso();
                ok = ingresso.CancelarTodasReservasInternet(clienteID, sessionID);

                if (ok)
                    return 1; //sucesso
                else
                    return 0;

            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                bd.Fechar();
            }

        }
        /// <summary>
        /// Criado por: Caio Maganha Rosa
        /// Utilização: Transfere todos os ingressos reservados via Internet
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public int TransferirTodasReservasInternet(int clienteID, string sessionID)
        {
            try
            {
                bool ok = false;
                if (clienteID > 0)
                {
                    Ingresso ingresso = new Ingresso();
                    ok = ingresso.TransferirTodasReservasInternet(clienteID, sessionID);

                    if (ok) // Conseguiu Transferir todos os ingressos
                        return 1;
                    else // Cancela operação de transferencia do carrinho
                        return 0;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao transferir os Ingresso - " + ex.Message);
            }
        }

        public int TransferirTodasReservasPaypalExpress(int clienteID, string sessionID)
        {
            try
            {
                bool ok = false;

                Ingresso ingresso = new Ingresso();
                ok = ingresso.TransferirTodasReservasMobile(clienteID, sessionID);

                if (ok) // Conseguiu Transferir todos os ingressos
                    return 1;
                else // Cancela operação de transferencia do carrinho
                    return 0;


            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao transferir os Ingresso - " + ex.Message);
            }
        }



        /// <summary>
        /// Transfere os ValeIngressos para clienteID
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public bool TransferirTodosVIRInternet(int clienteID, string sessionID)
        {
            try
            {
                if (clienteID == 0)
                    throw new Exception("Cliente Não Logado");

                ValeIngresso oValeIngresso = new ValeIngresso();
                oValeIngresso.ClienteID.Valor = clienteID;
                oValeIngresso.SessionID.Valor = sessionID;
                bool ok = oValeIngresso.TransferirReservas();
                return ok;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cancela a reserva dos ingressos passados como parametro. Retorna o sucesso da operacao.
        /// </summary>
        /// <param name="ingressosID">Ingressos a serem cancelados</param>
        /// <returns></returns>
        public bool CancelarReservas(int[] ingressosIDs, int usuarioID)
        {
            try
            {
                bool ok = false;

                if (ingressosIDs != null && ingressosIDs.Length > 0)
                {
                    //esse metodo vai supor que ingressosIDs contem ingressos reservados. ou seja,
                    //o metodo nao vai verificar se os ingressos estao mesmo com status reservado.
                    //o metodo vai atualizar o status para disponivel e 'zerar' o ingresso.
                    Ingresso ingresso = new Ingresso();
                    ingresso.UsuarioID.Valor = usuarioID;

                    ok = ingresso.CancelarReservas(ingressosIDs);
                }


                return ok;

            }
            catch (Exception ex)
            {


                throw ex;
            }

        }

        /// <summary>
        /// Cancela a reserva de todos os ingressos do usuario passados como parametro. Retorna o sucesso da operacao.
        /// </summary>
        /// <returns></returns>
        public bool CancelarReservas(int usuarioID)
        {


            try
            {


                //esse metodo vai supor que ingressosIDs contem ingressos reservados. ou seja,
                //o metodo nao vai verificar se os ingressos estao mesmo com status reservado.
                //o metodo vai atualizar o status para disponivel e 'zerar' o ingresso.

                Ingresso ingresso = new Ingresso();

                bool ok = ingresso.CancelarReservas(usuarioID);

                return ok;

            }
            catch (Exception ex)
            {


                throw ex;
            }

        }

        struct PrecoStruct
        {
            public int ID;
            public int Qtde;
            public int QtdeVendida; //registra a qtde vendida para esse cliente
            public PrecoStruct(int id, int q, int qv)
            {

                ID = id;
                Qtde = q;
                QtdeVendida = qv;
            }
        }
        /// <summary>
        /// Muda o Status do ingresso de Impresso para Vendido.
        /// </summary>
        /// <param name="ingressosIDs">Ingressos para mudar o status</param>
        public void CancelarImpressao(List<IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar> ingressoImpressaoCancelar, int usuarioID, int empresaID, string motivo, int SupervisorID)
        {
            BD bd = new BD();

            try
            {
                if (ingressoImpressaoCancelar.Count > 0 && ingressoImpressaoCancelar != null)
                {
                    bd.IniciarTransacao();

                    int regErros = 0;
                    int regAtualizados = 0;

                    IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();

                    for (int i = 0; i < ingressoImpressaoCancelar.Count; i++)
                    {
                        string sql;
                        int ingressoID = ingressoImpressaoCancelar[i].IngressoID;
                        string codigoBarraNovo = string.Empty;

                        if (ingressoImpressaoCancelar[i].TipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                        {
                            codigoBarraNovo = new CodigoBarra().NovoCodigoBarraListaBranca(bd, ingressoImpressaoCancelar[i].ApresentacaoSetorID);

                            ingressoCodigoBarra.EventoID.Valor = ingressoImpressaoCancelar[i].EventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarraNovo;

                            bd.Executar(ingressoCodigoBarra.StringInserir());
                        }

                        sql = "UPDATE tIngresso SET Status = '" + Ingresso.VENDIDO + "', CodigoBarra = '" + codigoBarraNovo + "' WHERE Status IN ('" + Ingresso.IMPRESSO + "','" + Ingresso.ENTREGUE + "') AND ID = " + ingressoID;

                        int x = bd.Executar(sql);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log

                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.BloqueioID.Valor = 0;
                            ingressoLog.CortesiaID.Valor = 0;
                            ingressoLog.PrecoID.Valor = ingressoImpressaoCancelar[i].PrecoID;
                            ingressoLog.VendaBilheteriaItemID.Valor = 0;
                            ingressoLog.VendaBilheteriaID.Valor = ingressoImpressaoCancelar[i].VendaBilheteriaID;
                            ingressoLog.CaixaID.Valor = 0;
                            ingressoLog.LojaID.Valor = 0;
                            ingressoLog.CanalID.Valor = 0;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.ClienteID.Valor = 0;
                            ingressoLog.Acao.Valor = IngressoLog.ANULAR_IMPRESSAO;
                            ingressoLog.Obs.Valor = motivo;
                            ingressoLog.SupervisorID.Valor = SupervisorID;

                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            x = bd.Executar(sqlIngressoLogV);

                            bool okV = (x == 1);
                            if (!okV)
                                throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                            ingressoCodigoBarra.Limpar();
                            ingressoCodigoBarra.EventoID.Valor = ingressoImpressaoCancelar[i].EventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = ingressoImpressaoCancelar[i].CodigoBarras;
                            ingressoCodigoBarra.BlackList.Valor = Convert.ToBoolean(ingressoImpressaoCancelar[i].BlackList);
                            ingressoCodigoBarra.TimeStamp.Valor = System.DateTime.Now;

                            string sqlingressoCodigoBarra = ingressoCodigoBarra.StringUpdate();
                            x = bd.Executar(sqlingressoCodigoBarra);

                            bool okV2 = (x == 1);
                            if (!okV2)
                                throw new Exception("Status do ingresso não pode ser cancelado.");

                            regAtualizados += 1;
                        }
                        else
                        {
                            //throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                            regErros += 1;
                        }
                    }
                    if (regAtualizados == 0)
                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.");

                    bd.FinalizarTransacao();
                }
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }


        }

        public void CancelarImpressao(BD bd, List<IRLib.Paralela.ClientObjects.IngressoImpressaoCancelar> ingressoImpressaoCancelar, int usuarioID, int empresaID, string motivo, int SupervisorID)
        {
            if (ingressoImpressaoCancelar.Count > 0 && ingressoImpressaoCancelar != null)
            {

                int regErros = 0;
                int regAtualizados = 0;

                IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();

                for (int i = 0; i < ingressoImpressaoCancelar.Count; i++)
                {
                    string sql;
                    int ingressoID = ingressoImpressaoCancelar[i].IngressoID;
                    string codigoBarraNovo = string.Empty;

                    if (ingressoImpressaoCancelar[i].TipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                    {
                        codigoBarraNovo = new CodigoBarra().NovoCodigoBarraListaBranca(bd, ingressoImpressaoCancelar[i].ApresentacaoSetorID);

                        ingressoCodigoBarra.EventoID.Valor = ingressoImpressaoCancelar[i].EventoID;
                        ingressoCodigoBarra.CodigoBarra.Valor = codigoBarraNovo;

                        bd.Executar(ingressoCodigoBarra.StringInserir());
                    }

                    sql = "UPDATE tIngresso SET Status = '" + Ingresso.VENDIDO + "', CodigoBarra = '" + codigoBarraNovo + "' WHERE Status IN ('" + Ingresso.IMPRESSO + "','" + Ingresso.ENTREGUE + "') AND ID = " + ingressoID;

                    int x = bd.Executar(sql);
                    bool ok = (x == 1);
                    if (ok)
                    {
                        //inserir na Log

                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = ingressoID;
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = 0;
                        ingressoLog.PrecoID.Valor = ingressoImpressaoCancelar[i].PrecoID;
                        ingressoLog.VendaBilheteriaItemID.Valor = 0;
                        ingressoLog.VendaBilheteriaID.Valor = ingressoImpressaoCancelar[i].VendaBilheteriaID;
                        ingressoLog.CaixaID.Valor = 0;
                        ingressoLog.LojaID.Valor = 0;
                        ingressoLog.CanalID.Valor = 0;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.ClienteID.Valor = 0;
                        ingressoLog.Acao.Valor = IngressoLog.ANULAR_IMPRESSAO;
                        ingressoLog.Obs.Valor = motivo;
                        ingressoLog.SupervisorID.Valor = SupervisorID;

                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);

                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                        ingressoCodigoBarra.Limpar();
                        ingressoCodigoBarra.EventoID.Valor = ingressoImpressaoCancelar[i].EventoID;
                        ingressoCodigoBarra.CodigoBarra.Valor = ingressoImpressaoCancelar[i].CodigoBarras;
                        ingressoCodigoBarra.BlackList.Valor = ingressoImpressaoCancelar[i].BlackList;
                        ingressoCodigoBarra.TimeStamp.Valor = System.DateTime.Now;

                        string sqlingressoCodigoBarra = ingressoCodigoBarra.StringUpdate();
                        x = bd.Executar(sqlingressoCodigoBarra);

                        bool okV2 = (x == 1);
                        if (!okV2)
                            throw new Exception("Status do ingresso não pode ser cancelado.");

                        regAtualizados += 1;
                    }
                    else
                    {
                        //throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                        regErros += 1;
                    }
                }
                if (regAtualizados == 0)
                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");


            }
        }


        public string CancelarValeIngresso(EstruturaRetornoVendaValeIngresso vendidos, DataTable pagamentos, int caixaID, int lojaID,
            int canalID, int empresaID, int clienteID, int entregaControleID, decimal valorEntrega, decimal valorTotal, int usuarioID,
            string notaFiscalCliente, string notaFiscalEstabelecimento,
            bool cancelaTaxaEntrega, string motivo, int motivoId, int SupervisorID, DateTime data, int vendaBilheteriaID, bool cancelaAdyen, bool cancelaPayPal, string TransactionID)
        {
            int entregaAgendaID = 0;
            if (data != DateTime.MinValue)
                entregaAgendaID = new EntregaAgenda().BuscarID(entregaControleID, data);

            return this.CancelarValeIngresso(vendidos, pagamentos, caixaID, lojaID, canalID, empresaID, clienteID, entregaControleID, valorEntrega,
                valorTotal, usuarioID, notaFiscalCliente, notaFiscalEstabelecimento, cancelaTaxaEntrega, motivo, motivoId, SupervisorID, entregaAgendaID, vendaBilheteriaID, cancelaAdyen, cancelaPayPal, TransactionID);
        }

        /// <summary>
        /// Cancela uma venda
        /// </summary>
        /// <returns></returns>
        public string CancelarValeIngresso(EstruturaRetornoVendaValeIngresso vendidos, DataTable pagamentos, int caixaID, int lojaID,
            int canalID, int empresaID, int clienteID, int entregaControleID, decimal valorEntrega, decimal valorTotal, int usuarioID,
            string notaFiscalCliente, string notaFiscalEstabelecimento,
            bool cancelaTaxaEntrega, string motivo, int motivoId, int SupervisorID, int EntregaAgendaID, int vendaBilheteriaID, bool cancelaAdyen, bool cancelaPayPal, string TransactionID)
        {
            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo.");

            if (lojaID == 0)
                throw new BilheteriaException("Loja nao pode ser nula.");

            if (canalID == 0)
                throw new BilheteriaException("Canal nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            if (cancelaAdyen && !IRLib.ConfiguracaoAdyen.Instancia.Chaves.Ativo.Valor)
                throw new Exception("Não será possível efetuar o cancelamento e estorno desta venda através da Adyen, ela está desativada!");

            BD bd = new BD();

            try
            {


                //VendaBilheteria
                bd.IniciarTransacao();

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                vendaBilheteria.ClienteID.Valor = clienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.Status.Valor = VendaBilheteria.CANCELADO;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                if (cancelaTaxaEntrega)
                {
                    vendaBilheteria.EntregaAgendaID.Valor = EntregaAgendaID;
                    vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                    vendaBilheteria.TaxaEntregaValor.Valor = valorEntrega;
                }

                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.NotaFiscalCliente.Valor = notaFiscalCliente;
                vendaBilheteria.NotaFiscalEstabelecimento.Valor = notaFiscalEstabelecimento;
                vendaBilheteria.PagamentoProcessado.Valor = true;

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();

                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);

                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.");

                string sqlValidade;
                string sqlCodigoTroca;
                foreach (EstruturaImpressaoVir valeIngresso in vendidos.EstruturaImpressaoVir)
                {


                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.");

                    //Valida se deve limpar o codigo de troca e data de validade
                    if (valeIngresso.ValidadeEmDiasImpressao > 0)
                        sqlValidade = "DataExpiracao = '', ";
                    else
                        sqlValidade = string.Empty;//ñ atualiza a validade fixa

                    if (!valeIngresso.CodigoTrocaFixo)
                        sqlCodigoTroca = "CodigoTroca = '',";
                    else
                        sqlCodigoTroca = string.Empty;

                    string sql = "UPDATE tValeIngresso SET ClienteID = 0, " + sqlValidade + sqlCodigoTroca + " UsuarioID=0, LojaID=0,CanalID = 0, CodigoBarra='', " +
                            "VendaBilheteriaID=0,TimeStampReserva = '', Status='" + (char)ValeIngresso.enumStatus.Disponivel + "' " +
                            "WHERE (Status='" + (char)ValeIngresso.enumStatus.Impresso + "' OR Status='" + (char)ValeIngresso.enumStatus.Vendido + "') AND ID=" + valeIngresso.ValeIngressoID;

                    int x = bd.Executar(sql);
                    bool ok = (x == 1);
                    if (ok)
                    {
                        ValeIngressoLog valeIngressoLog = new ValeIngressoLog();
                        valeIngressoLog.TimeStamp.Valor = System.DateTime.Now;
                        valeIngressoLog.ValeIngressoID.Valor = valeIngresso.ValeIngressoID;
                        valeIngressoLog.UsuarioID.Valor = usuarioID;
                        valeIngressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        valeIngressoLog.CaixaID.Valor = caixaID;
                        valeIngressoLog.LojaID.Valor = lojaID;
                        valeIngressoLog.CanalID.Valor = canalID;
                        valeIngressoLog.EmpresaID.Valor = empresaID;
                        valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Cancelar).ToString();
                        valeIngressoLog.CodigoBarra.Valor = valeIngresso.CodigoBarra;
                        valeIngressoLog.Obs.Valor = motivo;
                        valeIngressoLog.MotivoID.Valor = motivoId;
                        valeIngressoLog.SupervisorID.Valor = SupervisorID;

                        string sqlIngressoLogV = valeIngressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                    }
                    else
                    {
                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                    }

                }


                #region Particionamento de formas de pagamento
                //VendaBilheteriaFormaPagamento
                decimal porcentagemTotal = 0;

                for (int i = 0; i < pagamentos.Rows.Count; i++)
                {
                    DataRow pagto = pagamentos.Rows[i];
                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];

                    decimal valorFormaPagamento = 0;
                    Decimal.TryParse(Convert.ToString(pagto[VALOR]), out valorFormaPagamento);
                    vendaBilheteriaFormaPagamento.Valor.Valor = valorFormaPagamento > valorTotal ? valorTotal : valorFormaPagamento;

                    //calcular porcentagem
                    decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / valorTotal;
                    decimal porcentagem = Math.Round(porc, 2);

                    //a porcentagem final tem q dar 100%
                    if (i != (pagamentos.Rows.Count - 1))
                    {

                        porcentagemTotal += porcentagem;
                    }
                    else
                    {
                        //eh a ultima parcela


                        decimal totalTmp = porcentagemTotal + porcentagem;
                        if (totalTmp != 100)
                        {

                            porcentagem = 100 - porcentagemTotal;
                            porcentagemTotal += porcentagem;
                        }
                    }
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.Dias.Valor = pagto["Dias"] != DBNull.Value ? (int)pagto["Dias"] : 0;
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = pagto["TaxaAdm"] != DBNull.Value ? (decimal)pagto["TaxaADm"] : 0.00m;
                    vendaBilheteriaFormaPagamento.IR.Valor = pagto["IR"] != DBNull.Value && (string)pagto["IR"] == "T" ? true : false;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                    string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                    int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                    bool ok = (x >= 1);
                    if (!ok)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");

                }
                #endregion

                if (cancelaTaxaEntrega && EntregaAgendaID > 0)
                    bd.Executar("UPDATE tEntregaAgenda SET QtdAgendada = QtdAgendada - 1 WHERE ID = " + EntregaAgendaID);

                //Cancelamento Adyen
                if (cancelaAdyen)
                {
                    string codigoReferencia = bd.ConsultaValor(@"
								SELECT
									   TOP 1 CodigoRespostaVenda
								FROM tVendaBilheteriaFormaPagamento (NOLOCK)
								WHERE VendaBilheteriaID = " + vendaBilheteriaID + " AND CodigoRespostaVenda IS NOT NULL AND CodigoRespostaVenda <> ''").ToString();

                    new IRLib.Adyen() { CodigoReferencia = codigoReferencia }.CancelarPagamento();
                }

                if (cancelaPayPal)
                {
                    string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
                    string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
                    string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
                    bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

                    this.Refund(Usuario, Senha, Assinatura, TransactionID, AmbienteTestePayPal);
                }

                bd.FinalizarTransacao();

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                string senha = (ret != null) ? Convert.ToString(ret) : null;

                return senha;

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


        /// <summary>
        /// Sobrecarga do método de cancelar, este método é especifico para a VendaBilheteria (Tela de Vendas)
        /// Como não existe EntregaAgendaID, deve buscar no banco e passar para o método sobrecarregado.
        /// </summary>
        /// <param name="itensVendidos"></param>
        /// <param name="ingressosVendidos"></param>
        /// <param name="pagamentos"></param>
        /// <param name="caixaID"></param>
        /// <param name="lojaID"></param>
        /// <param name="canalID"></param>
        /// <param name="empresaID"></param>
        /// <param name="clienteID"></param>
        /// <param name="entregaControleID"></param>
        /// <param name="valorEntrega"></param>
        /// <param name="valorTotal"></param>
        /// <param name="usuarioID"></param>
        /// <param name="NotaFiscalCliente"></param>
        /// <param name="NotaFiscalEstabelecimento"></param>
        /// <param name="cancelaTaxaConv"></param>
        /// <param name="cancelaTaxaEntrega"></param>
        /// <param name="motivo"></param>
        /// <param name="cancelamentoFraude"></param>
        /// <param name="vendaBilheteriaID"></param>
        /// <param name="motivoId"></param>
        /// <param name="SupervisorID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Cancelar(DataTable itensVendidos, DataTable ingressosVendidos, DataTable pagamentos, int caixaID, int lojaID,
       int canalID, int empresaID, int clienteID, int entregaControleID, decimal valorEntrega, decimal valorTotal, int usuarioID,
       string NotaFiscalCliente, string NotaFiscalEstabelecimento, bool cancelaTaxaConv, bool cancelaTaxaEntrega, string motivo,
       bool cancelamentoFraude, int vendaBilheteriaID, int motivoId, int SupervisorID, DateTime data, bool cancelaTaxaProcessamento,
       decimal valorTaxaProcessamento, bool cancelarAdyen, string ApoliceMondial, bool cancelaPayPal, bool cancelasmiles, string TransactionID)
        {
            int entregaAgendaID = 0;
            if (data != DateTime.MinValue)
                entregaAgendaID = new EntregaAgenda().BuscarID(entregaControleID, data);

            return this.Cancelar(itensVendidos, ingressosVendidos, pagamentos, caixaID, lojaID, canalID, empresaID, clienteID, entregaControleID, valorEntrega, valorTotal,
                 usuarioID, NotaFiscalCliente, NotaFiscalEstabelecimento, cancelaTaxaConv,
                 cancelaTaxaEntrega, motivo, cancelamentoFraude, vendaBilheteriaID, motivoId, SupervisorID, entregaAgendaID, cancelaTaxaProcessamento, valorTaxaProcessamento, cancelarAdyen, ApoliceMondial, cancelaPayPal, cancelasmiles, TransactionID);
        }


        private void CancelamentoItensNormais(
            DataRow[] itensNormais, DataTable ingressosVendidos, VendaBilheteria vendaBilheteria, string codigoBarraAntigo,
            bool cancelaTaxaConv, Enumerators.TipoCodigoBarra tipoCodigoBarra, string codigoBarra, BD bd, BD bdConsulta,
            int usuarioID, int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int motivoId, int SupervisorID, string motivo, Assinatura infoAssinatura, int assinaturaClienteID)
        {
            #region Cadeira, Mesa Aberta e pista
            foreach (DataRow item in itensNormais)
            {
                DataRow[] ingressos;


                if (item["VendaBilheteriaItemID"] != DBNull.Value)
                {
                    if (Convert.ToInt32(item["VendaBilheteriaItemID"]) > 0)
                        ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "' AND VendaBilheteriaItemID = " + (int)item["VendaBilheteriaItemID"]);
                    else
                        ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");
                }
                else
                    ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                vendaBilheteriaItem.PacoteID.Valor = 0;
                vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;

                if (cancelaTaxaConv)
                {
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                }
                //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Ingresso;

                string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                if (vendaBilheteriaItem.Control.ID == 0)
                    throw new BilheteriaException("Item de venda não foi gerada.");

                codigoBarraAntigo = ingressos[0][CODIGO_BARRA].ToString();
                if (ingressos[0][TIPO_CODIGO_BARRA] == null || ingressos[0][TIPO_CODIGO_BARRA].ToString().Length == 0)
                    tipoCodigoBarra = Enumerators.TipoCodigoBarra.Estruturado;
                else
                    tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(ingressos[0][TIPO_CODIGO_BARRA]);
                int eventoID = Convert.ToInt32(ingressos[0][EVENTOID]);

                //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                if (codigoBarraAntigo.Length > 0)
                    bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', Sincronizado = 'F', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                {
                    codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(ingressos[0][APRESENTACAOSETORID]));

                    //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                    IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                    ingressoCodigoBarra.EventoID.Valor = eventoID;
                    ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                    bd.Executar(ingressoCodigoBarra.StringInserir());
                }
                else codigoBarra = string.Empty;

                string sql = string.Empty;

                if (assinaturaClienteID > 0 && infoAssinatura.Cancelamento == Assinatura.enumTipoCancelamento.Bloquear)
                {
                    sql = "UPDATE tIngresso SET UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', SerieID = 0, " +
                            "AssinaturaClienteID = 0, VendaBilheteriaID = 0, Status='" + Ingresso.BLOQUEADO + "', BloqueioID =  " + infoAssinatura.DesistenciaBloqueioID.Valor +
                            " WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND ID=" + (int)ingressos[0][INGRESSOID];
                }
                else
                {
                    if (0 == (int)ingressos[0][BLOQUEIOID])
                    {
                        sql = "UPDATE tIngresso SET ClienteID = 0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                            "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "', AssinaturaCLienteID = 0, SerieID = 0 " +
                            "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)ingressos[0][INGRESSOID];
                    }
                    else
                    {
                        sql = "UPDATE tIngresso SET ClienteID = 0, UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                            "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "', AssinaturaCLienteID = 0, SerieID = 0 " +
                            "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.AGUARDANDO_TROCA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)ingressos[0][INGRESSOID];
                    }
                }

                int x = bd.Executar(sql);
                bool ok = (x == 1);
                if (ok)
                {
                    //inserir na Log

                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                    ingressoLog.UsuarioID.Valor = usuarioID;
                    ingressoLog.BloqueioID.Valor = (int)ingressos[0][BLOQUEIOID];
                    ingressoLog.CortesiaID.Valor = (int)ingressos[0][CORTESIAID];
                    ingressoLog.PrecoID.Valor = (int)ingressos[0][PRECOID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    ingressoLog.CaixaID.Valor = caixaID;
                    ingressoLog.LojaID.Valor = lojaID;
                    ingressoLog.CanalID.Valor = canalID;
                    ingressoLog.EmpresaID.Valor = empresaID;
                    ingressoLog.ClienteID.Valor = clienteID;
                    ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                    ingressoLog.Obs.Valor = motivo;
                    ingressoLog.MotivoId.Valor = motivoId;
                    ingressoLog.SupervisorID.Valor = SupervisorID;
                    ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;
                    ingressoLog.AssinaturaClienteID.Valor = assinaturaClienteID;

                    string sqlIngressoLogV = ingressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                    bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)ingressos[0][INGRESSOID]);
                    if (bdConsulta.Consulta().Read())
                    {
                        var cotaItemID = bdConsulta.LerInt("CotaItemID");
                        var apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                        var apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");

                        //IRLib.Codigo.Ingresso oIngresso = new Ingresso();

                        //oIngresso.AdicionarCotaItem((int)ingressos[0][INGRESSOID], cotaItemID);

                        //BD bd2 = new BD();
                        //sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID;
                        //x = bd2.Executar(sql);

                        //if (x < 1)
                        //{
                        //    sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = 0";
                        //    bd2.Executar(sql);
                        //}

                        sql = "DELETE FROM tIngressoCliente WHERE IngressoID = " + (int)ingressos[0][INGRESSOID];
                        bd.Executar(sql);
                    }
                    bdConsulta.FecharConsulta();
                }
                else
                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
            }
            #endregion
        }

        /// <summary>
        /// Cancela uma venda
        /// </summary>
        /// <returns></returns>
        public string Cancelar(DataTable itensVendidos, DataTable ingressosVendidos, DataTable pagamentos, int caixaID, int lojaID,
            int canalID, int empresaID, int clienteID, int entregaControleID, decimal valorEntrega, decimal valorTotal, int usuarioID,
            string NotaFiscalCliente, string NotaFiscalEstabelecimento, bool cancelaTaxaConv,
            bool cancelaTaxaEntrega, string motivo, bool cancelamentoFraude, int vendaBilheteriaID, int motivoId, int SupervisorID, int EntregaAgendaID, bool cancelaTaxaProcessamento,
            decimal valorTaxaProcessamento, bool cancelarAdyen, string ApoliceMondial, bool cancelaPayPal, bool cancelasmiles, string TransactionID)
        {

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo.");

            if (lojaID == 0)
                throw new BilheteriaException("Loja nao pode ser nula.");

            if (canalID == 0)
                throw new BilheteriaException("Canal nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            if (valorTotal > 0 && pagamentos.Rows.Count == 0)
                throw new BilheteriaException("Forma de pagamento não foi adicionada.");

            if (cancelarAdyen && !IRLib.ConfiguracaoAdyen.Instancia.Chaves.Ativo.Valor)
                throw new Exception("Não será possível efetuar o cancelamento da Adyen, ela está inativa.");

            BD bd = new BD();
            BD bdConsulta = new BD();

            int cotaItemID = 0;
            int apresentacaoID = 0;
            int apresentacaoSetorID = 0;

            try
            {
                //VendaBilheteria
                bd.IniciarTransacao();

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                vendaBilheteria.ClienteID.Valor = clienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.Status.Valor = VendaBilheteria.CANCELADO;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria.NotaFiscalCliente.Valor = NotaFiscalCliente;
                vendaBilheteria.NotaFiscalEstabelecimento.Valor = NotaFiscalEstabelecimento;

                if (cancelaTaxaEntrega)
                {
                    vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                    vendaBilheteria.TaxaEntregaValor.Valor = valorEntrega;
                    vendaBilheteria.EntregaAgendaID.Valor = EntregaAgendaID;
                }
                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.TaxaProcessamentoValor.Valor = cancelaTaxaProcessamento ? valorTaxaProcessamento : 0;

                if (cancelaTaxaConv)
                {
                    decimal taxaConvenienciaValorTotal = 0;
                    decimal taxaComissaoValorTotal = 0;
                    decimal comisaoAux = 0;
                    foreach (DataRow item in itensVendidos.Rows)
                    {
                        comisaoAux = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;
                        taxaConvenienciaValorTotal += (decimal)item[VALOR_CONV];
                        taxaComissaoValorTotal += comisaoAux;
                    }
                    vendaBilheteria.TaxaConvenienciaValorTotal.Valor = taxaConvenienciaValorTotal;
                    vendaBilheteria.ComissaoValorTotal.Valor = taxaComissaoValorTotal;
                }
                vendaBilheteria.PagamentoProcessado.Valor = true;
                vendaBilheteria.ValorSeguro.Valor = 0;

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                VendaBilheteriaFormaPagamentoBoleto oFormaPagamentoBoleto = new VendaBilheteriaFormaPagamentoBoleto();
                List<int> lstBoletos = new List<int>();

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.");

                if (vendaBilheteriaID > 0 && !cancelamentoFraude)
                    bd.Executar(string.Format(@"UPDATE tVendaBilheteria SET PagamentoProcessado = 'T', VendaCancelada = 'T', TaxaProcessamentoCancelada = '{0}' WHERE ID = {1}", cancelaTaxaProcessamento ? 'T' : 'F', vendaBilheteriaID));
                else if (vendaBilheteriaID > 0)
                    bd.Executar(string.Format(@"UPDATE tVendaBilheteria SET PagamentoProcessado = 'T', NivelRisco = 3, VendaCancelada = 'T', TaxaProcessamentoCancelada = '{0}' WHERE ID = {1}", cancelaTaxaProcessamento ? 'T' : 'F', vendaBilheteriaID));


                if (cancelaTaxaEntrega && EntregaAgendaID > 0)
                    bd.Executar("UPDATE tEntregaAgenda SET QtdAgendada = QtdAgendada - 1 WHERE ID =" + EntregaAgendaID);

                //VendaBilheteriaItem
                //dividir cada tipo de item de venda em arrays

                //DataRow[] itensNormais = itensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "' AND (" + ASSINATURA_CLIENTE_ID + " <= 0 OR " + ASSINATURA_CLIENTE_ID + " IS NULL )");
                DataRow[] itensNormais = itensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                DataRow[] itensMesaFechada = itensVendidos.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                DataRow[] itensPacote = itensVendidos.Select(TIPO + "='" + TIPO_PACOTE + "'");
                string codigoBarra = string.Empty;
                Enumerators.TipoCodigoBarra tipoCodigoBarra = Enumerators.TipoCodigoBarra.Estruturado;
                string codigoBarraAntigo = string.Empty;

                var assinaturas = TabelaMemoria.DistinctComFiltro(itensVendidos, ASSINATURA_CLIENTE_ID, ASSINATURA_CLIENTE_ID + " > 0");
                Assinatura oAssinatura = new Assinatura(usuarioID);
                AssinaturaCliente oAssinaturaCliente = new AssinaturaCliente(usuarioID);


                if (assinaturas.Rows.Count > 0)
                {
                    var lstAssClienteID = new List<EstruturaAssinaturaBloqueio>();

                    foreach (DataRow linha in assinaturas.Rows)
                    {
                        var assinaturaClienteID = Convert.ToInt32(linha[ASSINATURA_CLIENTE_ID]);

                        var itensCancelar = itensVendidos.Select(ASSINATURA_CLIENTE_ID + "=" + assinaturaClienteID);

                        var infoCancelamento = oAssinaturaCliente.InfoCancelamento(assinaturaClienteID);

                        CancelamentoItensNormais(itensCancelar, ingressosVendidos, vendaBilheteria, codigoBarraAntigo, cancelaTaxaConv, tipoCodigoBarra, codigoBarra, bd, bdConsulta, usuarioID, caixaID, lojaID, canalID, empresaID,
                        clienteID, motivoId, SupervisorID, motivo, infoCancelamento, assinaturaClienteID);


                        lstAssClienteID.Add(new EstruturaAssinaturaBloqueio { AssinaturaClienteID = assinaturaClienteID, ClienteID = clienteID });
                    }

                    oAssinatura.CancelarAssinaturas(bd, lstAssClienteID);
                }
                else
                    CancelamentoItensNormais(itensNormais, ingressosVendidos, vendaBilheteria, codigoBarraAntigo, cancelaTaxaConv, tipoCodigoBarra, codigoBarra, bd, bdConsulta, usuarioID, caixaID, lojaID, canalID, empresaID,
                            clienteID, motivoId, SupervisorID, motivo, null, 0);

                #region Mesa fechada
                foreach (DataRow item in itensMesaFechada)
                {


                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                    vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                    vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;
                    if (cancelaTaxaConv)
                    {

                        vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                    }
                    //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Mesa;

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.");

                    foreach (DataRow i in ingressos)
                    {

                        codigoBarraAntigo = i[CODIGO_BARRA].ToString();
                        tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(i[TIPO_CODIGO_BARRA]);
                        int eventoID = Convert.ToInt32(i[EVENTOID]);

                        //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                        bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                        if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                        {
                            codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(i[APRESENTACAOSETORID]));

                            //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                            IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                            ingressoCodigoBarra.EventoID.Valor = eventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                            bd.Executar(ingressoCodigoBarra.StringInserir());
                        }
                        else codigoBarra = string.Empty;

                        string sql;
                        if (0 == (int)i[BLOQUEIOID])
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "' " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)i[INGRESSOID];
                        }
                        else
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "' " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)i[INGRESSOID];
                        }



                        int x = bd.Executar(sql);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log

                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                            ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                            ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.CaixaID.Valor = caixaID;
                            ingressoLog.LojaID.Valor = lojaID;
                            ingressoLog.CanalID.Valor = canalID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.ClienteID.Valor = clienteID;
                            ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                            ingressoLog.Obs.Valor = motivo;
                            ingressoLog.MotivoId.Valor = motivoId;
                            ingressoLog.SupervisorID.Valor = SupervisorID;
                            ingressoLog.CodigoBarra.Valor = codigoBarraAntigo;

                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            x = bd.Executar(sqlIngressoLogV);
                            bool okV = (x == 1);
                            if (!okV)
                                throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                            bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)i[INGRESSOID]);
                            if (bdConsulta.Consulta().Read())
                            {
                                cotaItemID = bdConsulta.LerInt("CotaItemID");
                                apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                                apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");
                                BD bd2 = new BD();
                                sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID;
                                x = bd2.Executar(sql);

                                if (x < 1)
                                {
                                    sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = 0";
                                    bd2.Executar(sql);
                                }

                                sql = "DELETE FROM tIngressoCliente WHERE IngressoID = " + (int)i[INGRESSOID];
                                bd.Executar(sql);
                            }
                            bdConsulta.FecharConsulta();

                        }
                        else
                        {


                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                        }

                        //Ingresso ingresso = new Ingresso();
                        //ingresso.Ler((int)i[INGRESSOID]);
                        //ingresso.Vender(vendaBilheteriaItem.Control.ID, vendaBilheteria.Control.ID, caixaID, lojaID, canalID, empresaID, clienteID);
                    }

                }
                #endregion

                #region Pacotes
                foreach (DataRow item in itensPacote)
                {


                    DataRow[] ingressosPacote = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = (int)ingressosPacote[0][PACOTEID];
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.CANCELAMENTO;
                    vendaBilheteriaItem.TaxaComissao.Valor = item[TAXA_COMISSAO] != DBNull.Value ? (int)item[TAXA_COMISSAO] : 0;
                    vendaBilheteriaItem.ComissaoValor.Valor = item[COMISSAO_VALOR] != DBNull.Value ? (decimal)item[COMISSAO_VALOR] : 0;
                    if (cancelaTaxaConv)
                    {

                        vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                    }
                    //vendaBilheteriaItem.Tipo.Valor = VendaBilheteriaItem.Pacote;

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.");

                    foreach (DataRow i in ingressosPacote)
                    {

                        codigoBarraAntigo = i[CODIGO_BARRA].ToString();
                        tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(i[TIPO_CODIGO_BARRA]);
                        int eventoID = Convert.ToInt32(i[EVENTOID]);

                        //deve atualizar a tIngressoCodigoBarra para adicionar os ingressos cancelados na black list
                        bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarraAntigo + "' AND EventoID = " + eventoID);

                        if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                        {
                            codigoBarra = new CodigoBarra().NovoCodigoBarraListaBranca(bd, Convert.ToInt32(i[APRESENTACAOSETORID]));

                            //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                            IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                            ingressoCodigoBarra.EventoID.Valor = eventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                            bd.Executar(ingressoCodigoBarra.StringInserir());
                        }
                        else codigoBarra = string.Empty;

                        string sql;
                        if (0 == (int)i[BLOQUEIOID])
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='', PacoteID=0 , PacoteGrupo = '', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.DISPONIVEL + "' " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID = 0 AND ID=" + (int)i[INGRESSOID];
                        }
                        else
                        {
                            sql = "UPDATE tIngresso SET ClienteID = 0,UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='', PacoteID=0 , PacoteGrupo = '', " +
                                "VendaBilheteriaID=0, Status='" + Ingresso.BLOQUEADO + "' " +
                                "WHERE (Status='" + Ingresso.PRE_RESERVA + "' OR Status='" + Ingresso.VENDIDO + "' OR Status='" + Ingresso.IMPRESSO + "' OR Status='" + Ingresso.ENTREGUE + "') AND BloqueioID > 0 AND ID=" + (int)i[INGRESSOID];
                        }

                        int x = bd.Executar(sql);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log

                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.BloqueioID.Valor = (int)i[BLOQUEIOID];
                            ingressoLog.CortesiaID.Valor = (int)i[CORTESIAID];
                            ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.CaixaID.Valor = caixaID;
                            ingressoLog.LojaID.Valor = lojaID;
                            ingressoLog.CanalID.Valor = canalID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.ClienteID.Valor = clienteID;
                            ingressoLog.Acao.Valor = IngressoLog.CANCELAR;
                            ingressoLog.Obs.Valor = motivo;
                            ingressoLog.MotivoId.Valor = motivoId;
                            ingressoLog.SupervisorID.Valor = SupervisorID;

                            if (codigoBarra is string)
                                ingressoLog.CodigoBarra.Valor = (string)codigoBarra;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            x = bd.Executar(sqlIngressoLogV);
                            bool okV = (x == 1);
                            if (!okV)
                                throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                            bdConsulta.Consulta("SELECT CotaItemID, ApresentacaoID, ApresentacaoSetorID  FROM tIngressoCliente (NOLOCK) WHERE IngressoID = " + (int)i[INGRESSOID]);
                            if (bdConsulta.Consulta().Read())
                            {
                                cotaItemID = bdConsulta.LerInt("CotaItemID");
                                apresentacaoID = bdConsulta.LerInt("ApresentacaoID");
                                apresentacaoSetorID = bdConsulta.LerInt("ApresentacaoSetorID");
                                BD bd2 = new BD();
                                sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = " + apresentacaoSetorID;
                                x = bd2.Executar(sql);

                                if (x < 1)
                                {
                                    sql = "UPDATE tCotaItemControleInMemory SET Quantidade = Quantidade - 1 WHERE CotaItemID = " + cotaItemID + " AND ApresentacaoID = " + apresentacaoID + " AND ApresentacaoSetorID = 0";
                                    bd2.Executar(sql);
                                }

                                sql = "DELETE FROM tIngressoCliente WHERE IngressoID = " + (int)i[INGRESSOID];
                                bd.Executar(sql);
                            }
                            bdConsulta.FecharConsulta();

                        }
                        else
                        {


                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                        }
                        //Ingresso ingresso = new Ingresso();
                        //ingresso.Ler((int)i[INGRESSOID]);
                        //ingresso.Vender(vendaBilheteriaItem.Control.ID, vendaBilheteria.Control.ID, caixaID, lojaID, canalID, empresaID, clienteID);
                    }

                }
                #endregion

                #region Particionamento de formas de pagamento
                //VendaBilheteriaFormaPagamento
                decimal porcentagemTotal = 0;

                for (int i = 0; i < pagamentos.Rows.Count; i++)
                {
                    DataRow pagto = pagamentos.Rows[i];
                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];

                    decimal valorFormaPagamento = 0;
                    Decimal.TryParse(Convert.ToString(pagto[VALOR]), out valorFormaPagamento);
                    vendaBilheteriaFormaPagamento.Valor.Valor = valorFormaPagamento > valorTotal ? valorTotal : valorFormaPagamento;

                    //calcular porcentagem
                    decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / valorTotal;
                    decimal porcentagem = Math.Round(porc, 2);

                    //a porcentagem final tem q dar 100%
                    if (i != (pagamentos.Rows.Count - 1))
                    {

                        porcentagemTotal += porcentagem;
                    }
                    else
                    {
                        //eh a ultima parcela


                        decimal totalTmp = porcentagemTotal + porcentagem;
                        if (totalTmp != 100)
                        {

                            porcentagem = 100 - porcentagemTotal;
                            porcentagemTotal += porcentagem;
                        }
                    }
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.Dias.Valor = pagto["Dias"] != DBNull.Value ? (int)pagto["Dias"] : 0;
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = pagto["TaxaAdm"] != DBNull.Value ? (decimal)pagto["TaxaADm"] : 0.00m;
                    vendaBilheteriaFormaPagamento.IR.Valor = pagto["IR"] != DBNull.Value && (string)pagto["IR"] == "T" ? true : false;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                    string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                    int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                    bool ok = (x >= 1);
                    if (!ok)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");

                }
                #endregion

                if (oFormaPagamentoBoleto.VerificaBoleto(vendaBilheteriaID))
                    lstBoletos = oFormaPagamentoBoleto.ProcessaCancelamentoBoleto(bd, vendaBilheteriaID, valorTotal);

                //Cancelamento Adyen
                if (cancelarAdyen)
                {
                    string codigoReferencia = bd.ConsultaValor(@"
								SELECT
									   TOP 1 CodigoRespostaVenda
								FROM tVendaBilheteriaFormaPagamento (NOLOCK)
								WHERE VendaBilheteriaID = " + vendaBilheteriaID + " AND CodigoRespostaVenda IS NOT NULL AND CodigoRespostaVenda <> ''").ToString();

                    if (!new IRLib.Adyen() { CodigoReferencia = codigoReferencia }.CancelarPagamento())
                        throw new Exception("Não foi possível efetuar o cancelamento através da Adyen, por favor tente novamente!");
                }

                if (cancelasmiles)
                {
                    string cancelamento = new PartnerRedemptionGateway().Cancel_Item_Payment_(TransactionID);

                    if (string.IsNullOrEmpty(cancelamento))
                        throw new Exception("Não foi possível efetuar o estorno através da Smiles, por favor tente novamente!");
                }

                if (cancelaPayPal)
                {
                    string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
                    string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
                    string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
                    bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

                    this.Refund(Usuario, Senha, Assinatura, TransactionID, AmbienteTestePayPal);
                }

                if (!string.IsNullOrEmpty(ApoliceMondial))
                {
                    TicketProtectorWebService objService = new TicketProtectorWebService();
                    objService.Url = ConfigurationManager.AppSettings["URLMondial"];

                    string CompanyID = ConfigurationManager.AppSettings["CompanyID"];
                    string AuthenticationCode = ConfigurationManager.AppSettings["AuthenticationCode"];

                    ReturnCancelObject cancelamento = objService.CancelPolicy(CompanyID, AuthenticationCode, ApoliceMondial, string.Empty);

                    if (!cancelamento.Canceled)
                        throw new Exception("Não foi possível efetuar o cancelamento do Seguro Mondial, por favor tente novamente!");
                }

                if (clienteID <= 0)
                {
                    VendaBilheteria oVendaBilheteria = new VendaBilheteria();
                    oVendaBilheteria.LerFormaPagamento(vendaBilheteriaID);
                    clienteID = oVendaBilheteria.ClienteID.Valor;
                }

                if (lstBoletos != null && lstBoletos.Count > 0)
                    oFormaPagamentoBoleto.ReenviaBoleto(lstBoletos, clienteID);

                bd.FinalizarTransacao();

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                string senha = (ret != null) ? Convert.ToString(ret) : null;

                return senha;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bdConsulta.Fechar();
                bd.Fechar();
            }

        }

        public EstruturaRetornoVendaValeIngresso VenderValeIngresso(BindingList<EstruturaRetornoReservaValeIngresso> reservados, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir,
            string NotaFiscalCliente, string NotaFiscalEstabelecimento, int IndiceInstituicaoTransacao,
            int IndiceTipoCartao, int NSUSitef, int NSUHost, int CodigoAutorizacaoCredito, int BIN,
            int ModalidadePagamentoCodigo, string ModalidadePagamentoTexto, int clienteEnderecoIDSelecionado, EstruturaEntregaAgenda entregaSelecionada, int pdvIDSelecionado, string NomeCartao, bool CartaoOutraPessoa)
        {
            //verificar se os dados estão preenchidos
            if (NotaFiscalCliente.Trim() == string.Empty)
                throw new Exception("Falha no envio de dados da transação TEF");

            return venderValeIngresso(reservados, pagamentos, caixaID, lojaID, canalID, empresaID, clienteID, taxaEntregaID, valorEntrega, valorTotal,
                usuarioID, troco, imprimir, NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao, IndiceTipoCartao, NSUSitef,
                NSUHost, CodigoAutorizacaoCredito, BIN, ModalidadePagamentoCodigo, ModalidadePagamentoTexto, clienteEnderecoIDSelecionado, entregaSelecionada, pdvIDSelecionado, NomeCartao, CartaoOutraPessoa);
        }

        public EstruturaRetornoVendaValeIngresso VenderValeIngresso(BindingList<EstruturaRetornoReservaValeIngresso> reservados, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir, string cartao, int clienteEnderecoIDSelecionado, EstruturaEntregaAgenda entregaSelecionada, int pdvIDSelecionado, string NomeCartao, bool CartaoOutraPessoa)
        {
            int numCartao = 0;
            if (cartao != null && cartao.Length >= 6)
                numCartao = int.Parse(cartao.Substring(0, 6));
            return venderValeIngresso(reservados, pagamentos, caixaID, lojaID, canalID, empresaID, clienteID, taxaEntregaID, valorEntrega, valorTotal
                , usuarioID, troco, imprimir, "", "", 0, 0, 0, 0, 0, numCartao, 0, "", clienteEnderecoIDSelecionado, entregaSelecionada, pdvIDSelecionado, NomeCartao, CartaoOutraPessoa);
        }

        private EstruturaRetornoVendaValeIngresso venderValeIngresso(BindingList<EstruturaRetornoReservaValeIngresso> reservados, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir,
            string NotaFiscalCliente, string NotaFiscalEstabelecimento, int IndiceInstituicaoTransacao,
            int IndiceTipoCartao, int NSUSitef, int NSUHost, int CodigoAutorizacaoCredito, int BIN,
            int ModalidadePagamentoCodigo, string ModalidadePagamentoTexto, int clienteEnderecoIDSelecionado, EstruturaEntregaAgenda entregaSelecionada, int pdvIDSelecionado, string NomeCartao, bool CartaoOutraPessoa)
        {
            AntiFraudeMotivo motivo = new AntiFraudeMotivo();
            //Inicializa a estrutura de retorno
            EstruturaRetornoVendaValeIngresso retorno = new EstruturaRetornoVendaValeIngresso();
            retorno.EstruturaImpressaoVir = new List<EstruturaImpressaoVir>();
            EstruturaImpressaoVir retornoItem;

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlTransaction tran = null;

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo.");

            if (lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula.");

            if (canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuário não pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa não pode ser nula.");

            ValeIngressoLog oValeIngressoLog;
            BD bd = new BD();
            CodigoBarra oCodigoBarra = new CodigoBarra();
            ValeIngresso oValeIngresso = new ValeIngresso();
            string codigoTroca = string.Empty;
            string validade = string.Empty;
            DateTime dataVenda = DateTime.Now;
            try
            {
                object idTemp = null;
                string codigoBarra = string.Empty;
                char statusFinal = imprimir ? (char)ValeIngresso.enumStatus.Impresso : (char)ValeIngresso.enumStatus.Vendido;
                conn = (SqlConnection)bd.Cnn;
                tran = conn.BeginTransaction(IsolationLevel.RepeatableRead, "Transaction_X_Venda");
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.Transaction = tran;

                int entregaControleID = 0;
                int entregaAgendaID = 0;
                int pdvID = pdvIDSelecionado;
                int clienteEnderecoID = clienteEnderecoIDSelecionado;

                EntregaAgenda oEA = new EntregaAgenda();
                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    cmd.CommandText = oEA.String();
                    verifAgendado = cmd.ExecuteNonQuery();

                    if (verifAgendado == null)
                        throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    entregaControleID = entregaSelecionada.dataPeriodoSelecionado.EntregaControleID;
                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case "A":

                            throw new BilheteriaException("Venda não foi gerada.");

                        default:
                            entregaControleID = entregaSelecionada.EntregaControleID;
                            break;
                    }
                }
                /// Insere o registro na tVendaBilheteria
                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                vendaBilheteria.ClienteID.Valor = clienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;
                vendaBilheteria.Status.Valor = (string)VendaBilheteria.PAGO;
                vendaBilheteria.DataVenda.Valor = dataVenda;
                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.TaxaEntregaValor.Valor = valorEntrega;
                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.NotaFiscalCliente.Valor = NotaFiscalCliente;
                vendaBilheteria.NotaFiscalEstabelecimento.Valor = NotaFiscalEstabelecimento;
                vendaBilheteria.IndiceInstituicaoTransacao.Valor = IndiceInstituicaoTransacao;
                vendaBilheteria.IndiceTipoCartao.Valor = IndiceTipoCartao;
                vendaBilheteria.NSUSitef.Valor = NSUSitef;
                vendaBilheteria.NSUHost.Valor = NSUHost;
                vendaBilheteria.CodigoAutorizacaoCredito.Valor = CodigoAutorizacaoCredito;
                vendaBilheteria.BIN.Valor = BIN;
                vendaBilheteria.ModalidadePagamentoCodigo.Valor = ModalidadePagamentoCodigo;
                vendaBilheteria.ModalidadePagamentoTexto.Valor = ModalidadePagamentoTexto.Replace("\0", "");
                vendaBilheteria.ComissaoValorTotal.Valor = 0;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = 0;
                vendaBilheteria.QuantidadeImpressoesInternet.Valor = 0;
                vendaBilheteria.PagamentoProcessado.Valor = true;
                vendaBilheteria.NomeCartao.Valor = NomeCartao;

                cmd.CommandText = vendaBilheteria.StringInserir();
                idTemp = cmd.ExecuteScalar();
                if (idTemp == null)
                    throw new BilheteriaException("Venda não foi gerada.");
                else
                    if (Convert.ToInt32(idTemp) <= 0)
                        throw new BilheteriaException("Venda não foi gerada.");

                vendaBilheteria.Control.ID = Convert.ToInt32(idTemp);

                //passa pelos itens reservados registrando nas tabelas necessárias.
                foreach (EstruturaRetornoReservaValeIngresso valeIngresso in reservados)
                {
                    retornoItem = new EstruturaImpressaoVir();
                    retornoItem.ValeIngressoID = valeIngresso.ID;
                    retornoItem.CodigoTrocaFixo = valeIngresso.CodigoTroca.Trim().Length > 0;
                    retornoItem.ValidadeEmDiasImpressao = valeIngresso.ValidadeDias;
                    retornoItem.ValorTipo = valeIngresso.ValorTipo;


                    if (imprimir && valeIngresso.ValidadeDias > 0)
                        retornoItem.ValidadeData = System.DateTime.Now.AddDays(retornoItem.ValidadeEmDiasImpressao);
                    else
                        retornoItem.ValidadeData = valeIngresso.Validade;

                    retornoItem.IndiceBufferValeIngressoTipoID = valeIngresso.IndiceBufferValeIngressoTipoID;
                    retornoItem.ValeIngressoTipoID = valeIngresso.ValeIngressoTipoID;
                    retornoItem.Valor = valeIngresso.Valor;
                    retornoItem.ValeIngressoNome = valeIngresso.Nome;
                    retornoItem.ClientePresenteado = valeIngresso.ClienteNome;
                    retornoItem.Status = (ValeIngresso.enumStatus)statusFinal; //necessário preencher para o registro de impressão

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                    vendaBilheteriaItem.TaxaConveniencia.Valor = 0;
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = 0;
                    vendaBilheteriaItem.TaxaComissao.Valor = 0;
                    vendaBilheteriaItem.ComissaoValor.Valor = 0;

                    cmd.CommandText = vendaBilheteriaItem.StringInserir();
                    idTemp = cmd.ExecuteScalar();
                    if (idTemp == null)
                        throw new BilheteriaException("Item de Venda não foi gerada.");
                    else
                        if (Convert.ToInt32(idTemp) <= 0)
                            throw new BilheteriaException("Item de Venda não foi gerada.");

                    vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);

                    //INFORMAÇÔES GERADAS DE ACORDO COM A IMPRESSÂO
                    //Gera o código de troca, codigo de barra e validade, caso necessário
                    if (imprimir)
                    {
                        //CODIGO TROCA
                        if (valeIngresso.CodigoTroca.Trim().Length == 0)//Codigo dinâmico
                            codigoTroca = oValeIngresso.NovoCodigoTroca();
                        else//Codigo fixo
                            codigoTroca = valeIngresso.CodigoTroca;

                        //CODIGO BARRA
                        codigoBarra = oCodigoBarra.NovoCodigoBarraRandomico(18);
                        
                    }
                    else
                    {
                        //Se não vai imprimir não deve gerar agora.
                        if (valeIngresso.CodigoTroca.Trim().Length == 0)
                            codigoTroca = string.Empty;
                        else//Codigo fixo
                            codigoTroca = valeIngresso.CodigoTroca;

                        //Codigo barra gerado na impressão
                        codigoBarra = string.Empty;                       
                    }

                    //VALIDADE
                    if (valeIngresso.ValidadeDias > 0)//calcula a validade
                        validade = DateTime.Now.AddDays(valeIngresso.ValidadeDias).ToString("yyyyMMdd");
                    else
                        validade = valeIngresso.Validade.ToString("yyyyMMdd");


                    //Atualiza o registro na tValeIngresso
                    // Registra a venda. Se existir o cliente selecionado, atualiza com o ID do Cliente
                    cmd.CommandText = @"EXEC dbo.RegistrarVendatValeIngresso 
											   @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                            vendaBilheteria.Control.ID +
                                            " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID + ", @UsuarioID = " + usuarioID +
                                            " , @ValeIngressoID = " + valeIngresso.ID + ", @ClienteNome = '" + valeIngresso.ClienteNome + "' , @DataExpiracao = '" + validade + "'" +
                                            " , @CodigoTroca = '" + codigoTroca + "'";

                    retornoItem.CodigoTroca = codigoTroca;
                    retornoItem.CodigoBarra = codigoBarra;

                    int codigoImpressao = Convert.ToInt32(cmd.ExecuteScalar());
                    if (codigoImpressao == -1)
                        throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas.(" + valeIngresso.ID + "-" + usuarioID + ")");
                    else
                    {
                        //VALE INGRESSO LOG
                        oValeIngressoLog = new ValeIngressoLog();
                        oValeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Vender).ToString();
                        oValeIngressoLog.TimeStamp.Valor = DateTime.Now;
                        oValeIngressoLog.UsuarioID.Valor = usuarioID;
                        oValeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        oValeIngressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        oValeIngressoLog.EmpresaID.Valor = empresaID;
                        oValeIngressoLog.CaixaID.Valor = caixaID;
                        oValeIngressoLog.LojaID.Valor = lojaID;
                        oValeIngressoLog.CanalID.Valor = canalID;
                        oValeIngressoLog.ValeIngressoID.Valor = valeIngresso.ID;
                        oValeIngressoLog.CodigoTroca.Valor = codigoTroca;
                        oValeIngressoLog.ClienteNome.Valor = valeIngresso.ClienteNome;

                        //Insere o registro de venda
                        cmd.CommandText = oValeIngressoLog.StringInserir();
                        if (cmd.ExecuteNonQuery() != 1)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                        //Se for imprimir já registra a impressão. 
                        if (imprimir)
                        {
                            oValeIngressoLog.CodigoBarra.Valor = codigoBarra;
                            oValeIngressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                            //Insere o registro de Impressão
                            cmd.CommandText = oValeIngressoLog.StringInserir();
                            if (cmd.ExecuteNonQuery() != 1)
                                throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                        }
                    }
                    retorno.EstruturaImpressaoVir.Add(retornoItem);
                    //ADD objeto
                }

                #region Manipulação de formas de pagamento
                //VendaBilheteriaFormaPagamento
                //dividir o valorTotal nas formas de pagamento
                decimal porcentagemTotal = 0;

                troco = Math.Abs(troco);

                for (int i = 0; i < pagamentos.Rows.Count; i++)
                {
                    DataRow pagto = pagamentos.Rows[i];
                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];
                    if (DINHEIRO == (int)pagto["ID"])
                        vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto[VALOR] - troco;
                    //#VIR:
                    else if (valeIngresso.FormaPagamentoID == Convert.ToInt32(pagto["ID"]))
                    {
                        vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto[VALOR] > valorTotal ? valorTotal : (decimal)pagto[VALOR];
                    }
                    else
                        vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto[VALOR];
                    //calcular porcentagem
                    decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / valorTotal;
                    decimal porcentagem = Math.Round(porc, 2);

                    //a porcentagem final tem q dar 100%
                    if (i != (pagamentos.Rows.Count - 1))
                    {
                        porcentagemTotal += porcentagem;
                    }
                    else
                    {
                        //eh a ultima parcela

                        decimal totalTmp = porcentagemTotal + porcentagem;
                        if (totalTmp != 100)
                        {
                            porcentagem = 100 - porcentagemTotal;
                            porcentagemTotal += porcentagem;
                        }
                    }
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.Dias.Valor = (int)pagto["Dias"];
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = (decimal)pagto["TaxaAdm"];
                    vendaBilheteriaFormaPagamento.IR.Valor = (string)pagto["IR"] == "T" ? true : false;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                    //#VIR:
                    object x = null;

                    cmd.CommandText = vendaBilheteriaFormaPagamento.StringInserir();

                    x = cmd.ExecuteNonQuery();
                    if ((int)x < 1)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                }

                #endregion

                tran.Commit();

                cmd.CommandText = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                idTemp = cmd.ExecuteScalar();

                //Preenche as informações restantes
                retorno.Senha = (idTemp != null) ? idTemp.ToString() : null;
                retorno.ClienteID = clienteID;
                retorno.ValorTotalEntrega = valorEntrega;
                retorno.ValorTotalValeIngressos = valorTotal - valorEntrega;
                retorno.DataVenda = dataVenda;
                retorno.VendaBilheteriaID = vendaBilheteria.Control.ID;

                cmd.Dispose();
                conn.Close();
                conn.Close();

                return retorno;
            }
            catch (Exception ex)
            {
                try
                {
                    if (tran != null)
                        tran.Rollback();
                }
                catch (ArgumentException) { } // transação não específicada
                catch (InvalidOperationException) { }// Já foi dado o commit ou o rollback
                catch (Exception ex1)
                {
                    throw new BilheteriaException(ex.Message + Environment.NewLine + ex1.Message);
                }

                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                }
            }
        }

        public EstruturaRetornoVenda Vender(DataTable itensReservados, DataTable ingressosVendidos, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir,
            string NotaFiscalCliente, string NotaFiscalEstabelecimento, int IndiceInstituicaoTransacao,
            int IndiceTipoCartao, int NSUSitef, int NSUHost, int CodigoAutorizacaoCredito, int BIN,
            int ModalidadePagamentoCodigo, string ModalidadePagamentoTexto, int CupomID, Dictionary<int, int> IDsComDesconto, Dictionary<int, int> ingressoXcliente, Canal.TipoDeVenda tipoVenda,
            bool preReserva, bool preReservaEfetivar, string celular, List<EstruturaDonoIngresso> listaDonoIngresso, List<int> VirsID, string codigoTrocaFixo, string CodigoRespostaVenda,
            string mensagemretorno, string horatransacao, string datatransacao, string codigoir,
            string numeroautorizacao, string cupom, string dadosConfirmacaoVenda, string Rede, string CodigoRespostaTransacao, bool AntiFraude, int clienteEnderecoID, EstruturaEntregaAgenda entregaSelecionada, int pdvID, string NomeCartao, bool CartaoOutraPessoa, decimal SeguroMondial)
        {
            //verificar se os dados estão preenchidos
            if (NotaFiscalCliente.Trim() == string.Empty)
                throw new Exception("Falha no envio de dados da transação TEF");


            return vender(itensReservados, ingressosVendidos, pagamentos, caixaID, lojaID, canalID, empresaID,
                    clienteID, taxaEntregaID, valorEntrega, valorTotal, usuarioID, troco, imprimir,
                    NotaFiscalCliente, NotaFiscalEstabelecimento, IndiceInstituicaoTransacao,
                    IndiceTipoCartao, NSUSitef, NSUHost, CodigoAutorizacaoCredito, BIN,
                    ModalidadePagamentoCodigo, ModalidadePagamentoTexto, CupomID, IDsComDesconto, ingressoXcliente, tipoVenda,
                    preReserva, preReservaEfetivar, celular, listaDonoIngresso, VirsID, codigoTrocaFixo, CodigoRespostaVenda,
            mensagemretorno, horatransacao, datatransacao, codigoir, numeroautorizacao, cupom, dadosConfirmacaoVenda, Rede, CodigoRespostaTransacao, AntiFraude, clienteEnderecoID, entregaSelecionada, pdvID, NomeCartao, CartaoOutraPessoa, SeguroMondial);
        }

        public EstruturaRetornoVenda Vender(DataTable itensReservados, DataTable ingressosVendidos, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir, string NumeroCartao, int CupomID, Dictionary<int, int> IDsComDesconto, Dictionary<int, int> ingressoXcliente, Canal.TipoDeVenda tipoVenda,
            bool preReserva, bool preReservaEfetivar, string celular, List<EstruturaDonoIngresso> listaDonoIngresso, List<int> VirsID, string codigoTrocaFixo, int clienteEnderecoID, EstruturaEntregaAgenda entregaSelecionada, int pdvID, string NomeCartao, bool CartaoOutraPessoa, decimal SeguroMondial)
        {
            int NroCartao = 0;
            if (NumeroCartao != null && NumeroCartao.Length >= 6)
                NroCartao = int.Parse(NumeroCartao.Substring(0, 6));
            return vender(itensReservados, ingressosVendidos, pagamentos, caixaID, lojaID, canalID, empresaID, clienteID,
                        taxaEntregaID, valorEntrega, valorTotal, usuarioID, troco, imprimir, "", "", 0, 0, 0, 0, 0, NroCartao, 0, "", CupomID, IDsComDesconto, ingressoXcliente, tipoVenda,
                        preReserva, preReservaEfetivar, celular, listaDonoIngresso, VirsID, codigoTrocaFixo, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, clienteEnderecoID, entregaSelecionada, pdvID, NomeCartao, CartaoOutraPessoa, SeguroMondial);
        }

        /// <summary>
        /// Faz a venda para esses ingressos. Retorna a senha única da venda. Retorna null, caso nao conseguiu vender.
        /// </summary>
        /// <returns></returns>
        /// 

        private EstruturaRetornoVenda vender(DataTable itensReservados, DataTable ingressosVendidos, DataTable pagamentos,
            int caixaID, int lojaID, int canalID, int empresaID, int clienteID, int taxaEntregaID,
            decimal valorEntrega, decimal valorTotal, int usuarioID, decimal troco, bool imprimir,
            string NotaFiscalCliente, string NotaFiscalEstabelecimento, int IndiceInstituicaoTransacao,
            int IndiceTipoCartao, int NSUSitef, int NSUHost, int CodigoAutorizacaoCredito, int BIN,
            int ModalidadePagamentoCodigo, string ModalidadePagamentoTexto, int CupomID, Dictionary<int, int> IDsComDesconto, Dictionary<int, int> ingressoXcliente, Canal.TipoDeVenda tipoVenda,
            bool preReserva, bool preReservaEfetivar, string celular, List<EstruturaDonoIngresso> listaDonoIngresso, List<int> VirIds, string codigoTrocaFixo, string CodigoRespostaVenda,
            string MensagemRetorno, string HoraTransacao, string DataTransacao, string CodigoIR,
            string NumeroAutorizacao, string Cupom, string DadosConfirmacaoVenda, string Rede, string CodigoRespostaTransacao, bool AntiFraude, int clienteEnderecoIDSelecionado, EstruturaEntregaAgenda entregaSelecionada, int pdvIDSelecionado, string NomeCartao, bool CartaoOutraPessoa, decimal SeguroMondial)
        {
            EstruturaRetornoVenda retorno = new EstruturaRetornoVenda();
            retorno.IngressoXCodigoImpressao = new Dictionary<int, int>();

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo.");

            if (lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula.");

            if (canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuário não pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa não pode ser nula.");


            BD bd = new BD();

            try
            {
                bd.IniciarTransacao();

                string statusFinal = string.Empty;

                // Atribui o Status do Ingresso
                if (tipoVenda == Canal.TipoDeVenda.ImpressaoVoucher)
                    statusFinal = ((imprimir && (!preReserva)) ? Ingresso.AGUARDANDO_TROCA : ((preReserva) ? Ingresso.PRE_RESERVA : Ingresso.VENDIDO));
                else
                    statusFinal = ((imprimir && (!preReserva)) ? Ingresso.IMPRESSO : ((preReserva) ? Ingresso.PRE_RESERVA : Ingresso.VENDIDO));

                object idTemp = null;
                string codigoBarra = string.Empty;

                StringBuilder stbSQL = new StringBuilder();

                int entregaControleID = 0;
                int entregaAgendaID = 0;
                int pdvID = pdvIDSelecionado;
                int clienteEnderecoID = clienteEnderecoIDSelecionado;

                EntregaAgenda oEA = new EntregaAgenda();
                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    verifAgendado = bd.Executar(oEA.String());

                    if (verifAgendado == null)
                        throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    entregaControleID = entregaSelecionada.dataPeriodoSelecionado.EntregaControleID;
                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case Entrega.AGENDADA:
                            throw new BilheteriaException("Venda não foi gerada.");
                        default:
                            entregaControleID = entregaSelecionada.EntregaControleID;
                            break;
                    }
                }

                #region Detalhes sobre a venda
                /// Insere o registro na tVendaBilheteria

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                string vendaBilheteriaStatus = ((preReserva) ? VendaBilheteria.PRE_RESERVADO : VendaBilheteria.PAGO);

                vendaBilheteria.ClienteID.Valor = clienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                retorno.Status = vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria.TaxaEntregaID.Valor = taxaEntregaID;

                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.TaxaEntregaValor.Valor = valorEntrega;

                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.NotaFiscalCliente.Valor = NotaFiscalCliente;
                vendaBilheteria.NotaFiscalEstabelecimento.Valor = NotaFiscalEstabelecimento;
                vendaBilheteria.IndiceInstituicaoTransacao.Valor = IndiceInstituicaoTransacao;
                vendaBilheteria.IndiceTipoCartao.Valor = IndiceTipoCartao;
                vendaBilheteria.NSUSitef.Valor = NSUSitef;
                vendaBilheteria.NSUHost.Valor = NSUHost;
                vendaBilheteria.CodigoAutorizacaoCredito.Valor = CodigoAutorizacaoCredito;
                vendaBilheteria.BIN.Valor = BIN;
                vendaBilheteria.ModalidadePagamentoCodigo.Valor = ModalidadePagamentoCodigo;
                vendaBilheteria.ModalidadePagamentoTexto.Valor = ModalidadePagamentoTexto.Replace("\0", "");
                vendaBilheteria.QuantidadeImpressoesInternet.Valor = 0;

                if (celular != string.Empty)
                {
                    string DDD = celular.Substring(0, 2);
                    string numeroCelular = celular.Substring(2, celular.Length - 2);

                    vendaBilheteria.NumeroCelular.Valor = Convert.ToInt32(numeroCelular);
                    vendaBilheteria.DDD.Valor = Convert.ToInt32(DDD);
                }

                // Calcula a tx de conveniência
                decimal taxaConvenienciaValorTotal = 0;
                decimal taxaComissaoValorTotal = 0;

                foreach (DataRow item in itensReservados.Rows)
                {
                    if (item[VALOR_CONV] == DBNull.Value)
                        item[VALOR_CONV] = 0;
                    if (item[COMISSAO_VALOR] == DBNull.Value)
                        item[COMISSAO_VALOR] = 0;

                    taxaConvenienciaValorTotal += (decimal)item[VALOR_CONV];
                    taxaComissaoValorTotal += (decimal)item[COMISSAO_VALOR];
                }

                vendaBilheteria.ComissaoValorTotal.Valor = taxaComissaoValorTotal;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = taxaConvenienciaValorTotal;
                vendaBilheteria.PagamentoProcessado.Valor = true;
                vendaBilheteria.NomeCartao.Valor = NomeCartao;
                vendaBilheteria.ValorSeguro.Valor = SeguroMondial;

                idTemp = bd.ConsultaValor(vendaBilheteria.StringInserir());

                if (idTemp == null)
                    throw new BilheteriaException("Venda não foi gerada.");
                else
                    if (Convert.ToInt32(idTemp) <= 0)
                        throw new BilheteriaException("Venda não foi gerada.");

                vendaBilheteria.Control.ID = Convert.ToInt32(idTemp);
                #endregion

                //Divisão de ingressos por tipo. Manipulação variável.
                DataRow[] itensNormais = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                DataRow[] itensMesaFechada = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                DataRow[] itensPacote = itensReservados.Select(TIPO + "='" + TIPO_PACOTE + "'");

                CotaItemControle oCotaItemControle = new CotaItemControle();
                IngressoCliente oIngressoCliente = new IngressoCliente();
                CodigoBarra oCodigoBarra = new CodigoBarra();
                retorno.CodigosBarra = new Dictionary<int, string>();

                #region Cadeiras, mesa aberta e pista
                foreach (DataRow item in itensNormais)
                {
                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = ((preReserva) ? VendaBilheteriaItem.PRE_RESERVA : VendaBilheteriaItem.VENDA);

                    if (item[CONV] == DBNull.Value)
                        item[CONV] = 0;
                    if (item[VALOR_CONV] == DBNull.Value)
                        item[VALOR_CONV] = 0;
                    if (item[TAXA_COMISSAO] == DBNull.Value)
                        item[TAXA_COMISSAO] = 0;
                    if (item[COMISSAO_VALOR] == DBNull.Value)
                        item[COMISSAO_VALOR] = 0;

                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                    if (idTemp == null)
                        throw new BilheteriaException("Item de Venda não foi gerada.");
                    else
                        if (Convert.ToInt32(idTemp) <= 0)
                            throw new BilheteriaException("Item de Venda não foi gerada.");

                    vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);

                    int clienteIngresso = 0;
                    ingressoXcliente.TryGetValue((int)ingressos[0][INGRESSOID], out clienteIngresso);

                    if (imprimir && tipoVenda != Canal.TipoDeVenda.ImpressaoVoucher && (!preReserva))
                    {
                        int codigoSequencial = ingressos[0][CODIGO_SEQUENCIAL] != DBNull.Value ? (int)ingressos[0][CODIGO_SEQUENCIAL] : 0;

                        //Código de barras é gerado apenas quando há necessidade de registrar a impressão do ingresso.
                        if (Convert.ToChar(ingressos[0][TIPO_CODIGO_BARRA]) == (char)Enumerators.TipoCodigoBarra.Estruturado)
                            codigoBarra = oCodigoBarra.NovoCodigoBarraIngresso((int)ingressos[0][PRECOID], (int)ingressos[0][EVENTOID], codigoSequencial);
                        else
                            codigoBarra = ingressos[0][CODIGO_BARRA].ToString();

                        //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                        if (codigoBarra.Trim().Length > 0)
                        {
                            IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                            ingressoCodigoBarra.EventoID.Valor = (int)ingressos[0][EVENTOID];
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                            if (bd.Executar(ingressoCodigoBarra.StringInserir()) != 1)
                                throw new BilheteriaException("Não foi possivel inserir na tIngressoCodigoBarra.");
                        }
                    }
                    else
                        codigoBarra = ingressos[0][CODIGO_BARRA].ToString(); // Se não for imprimir, codigo de barras igual ao que está no ingresso -- pode ser lista branca!           

                    string SQL = string.Empty;

                    if (preReservaEfetivar)
                        // Registra a efetivação da pré-reserva. Se existir o cliente selecionado, atualiza com o ID do Cliente
                        SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoEfetivarPreReserva 
										   @CodigoBarra = '" + codigoBarra + "', @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                                vendaBilheteria.Control.ID +
                                                " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID +
                                                " , @IngressoID = " + (int)ingressos[0][INGRESSOID] +
                                                " , @PrecoID = " + (int)ingressos[0][PRECOID];
                    else
                        // Registra a venda. Se existir o cliente selecionado, atualiza com o ID do Cliente
                        SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
											   @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                                vendaBilheteria.Control.ID +
                                                " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID + ", @UsuarioID = " + usuarioID +
                                                " , @IngressoID = " + (int)ingressos[0][INGRESSOID] + " , @GerenciamentoIngressosID = " + (int)ingressos[0][GERENCIAMENTO_INGRESSOS_ID];

                    int codigoImpressao = (int)bd.ConsultaValor(SQL);

                    if (codigoImpressao == -1)
                        throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas.(" + (int)ingressos[0][INGRESSOID] + "-" + usuarioID + ")");
                    else
                    {
                        if (statusFinal != Ingresso.VENDIDO && statusFinal != Ingresso.PRE_RESERVA)
                            retorno.IngressoXCodigoImpressao.Add((int)ingressos[0][INGRESSOID], codigoImpressao);

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)ingressos[0][INGRESSOID];
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.BloqueioID.Valor = (int)ingressos[0][BLOQUEIOID];
                        ingressoLog.CortesiaID.Valor = (int)ingressos[0][CORTESIAID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID;
                        ingressoLog.LojaID.Valor = lojaID;
                        ingressoLog.CanalID.Valor = canalID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.ClienteID.Valor = clienteID;
                        ingressoLog.PrecoID.Valor = (int)ingressos[0][PRECOID];
                        ingressoLog.Acao.Valor = ((preReserva) ? IngressoLog.PRE_RESERVA : IngressoLog.VENDER);
                        ingressoLog.GerenciamentoIngressosID.Valor = (int)ingressos[0][GERENCIAMENTO_INGRESSOS_ID];

                        if (bd.Executar(ingressoLog.StringInserir()) != 1)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                        if (imprimir && (!preReserva))
                        {
                            ingressoLog.Acao.Valor = ((tipoVenda == Canal.TipoDeVenda.ImpressaoVoucher) ? IngressoLog.VOUCHER_IMPRESSAO : IngressoLog.IMPRIMIR);
                            ingressoLog.VendaBilheteriaItemID.Valor = 0;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.CodigoBarra.Valor = codigoBarra;

                            if (!retorno.CodigosBarra.ContainsKey((int)ingressos[0][INGRESSOID]))
                                retorno.CodigosBarra.Add((int)ingressos[0][INGRESSOID], codigoBarra);

                            if (bd.Executar(ingressoLog.StringInserir()) != 1)
                                throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                        }
                    }

                    //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                    if ((int)ingressos[0][COTA_ITEM_ID] > 0 || (int)ingressos[0][COTA_ITEM_ID_APS] > 0)
                    {
                        EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)ingressos[0][INGRESSOID]).FirstOrDefault();

                        if (dono != null)
                        {
                            oIngressoCliente.ApresentacaoID.Valor = (int)ingressos[0][APRESENTACAOID];
                            oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingressos[0][APRESENTACAOSETORID];
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;

                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            //Preenche os Objs
                            oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingressos[0][APRESENTACAOSETORID];
                            oCotaItemControle.ApresentacaoID.Valor = (int)ingressos[0][APRESENTACAOID];
                            //oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                            //Incrementa a quantidade da ApresentacaoSetor
                            if (dono.CotaItemIDAPS > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                //Atualizou agora verifica a Quantidade
                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingressos[0][APRESENTACAOSETORID]);

                                if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                            }
                            if (dono.CotaItemID > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                                //Incrementa a quantidade da Apresentacao
                                bd.Executar(oCotaItemControle.StringAtualizarAP());

                                //Atualizou agora verifica a Quantidade
                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingressos[0][APRESENTACAOID]);

                                if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                            }
                        }
                    }

                }
                #endregion

                #region Mesa fechada
                foreach (DataRow item in itensMesaFechada)
                {
                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = ((preReserva) ? VendaBilheteriaItem.PRE_RESERVA : VendaBilheteriaItem.VENDA);

                    if (item[CONV] == DBNull.Value)
                        item[CONV] = 0;
                    if (item[VALOR_CONV] == DBNull.Value)
                        item[VALOR_CONV] = 0;
                    if (item[TAXA_COMISSAO] == DBNull.Value)
                        item[TAXA_COMISSAO] = 0;
                    if (item[COMISSAO_VALOR] == DBNull.Value)
                        item[COMISSAO_VALOR] = 0;


                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item[VALOR_CONV];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                    if (idTemp == null)
                        throw new BilheteriaException("Item de Venda não foi gerada.");
                    else
                        if (Convert.ToInt32(idTemp) <= 0)
                            throw new BilheteriaException("Item de Venda não foi gerada.");

                    vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);

                    foreach (DataRow ingresso in ingressos)
                    {
                        int clienteIngresso = 0;
                        ingressoXcliente.TryGetValue((int)ingresso[INGRESSOID], out clienteIngresso);

                        if (imprimir && tipoVenda != Canal.TipoDeVenda.ImpressaoVoucher && (!preReserva))
                        {
                            // Código de barras é gerado apenas quando há necessidade de registrar a impressão do ingresso.
                            if (Convert.ToChar(ingresso[TIPO_CODIGO_BARRA]) == (char)Enumerators.TipoCodigoBarra.Estruturado)
                                codigoBarra = oCodigoBarra.NovoCodigoBarraIngresso((int)ingresso[PRECOID], (int)ingresso[EVENTOID], (int)ingresso[CODIGO_SEQUENCIAL]);
                            else
                                codigoBarra = ingresso[CODIGO_BARRA].ToString();

                            //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                            if (codigoBarra.Trim().Length > 0)
                            {
                                IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                                ingressoCodigoBarra.EventoID.Valor = (int)ingresso[EVENTOID];
                                ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                                if (bd.Executar(ingressoCodigoBarra.StringInserir()) != 1)
                                    throw new BilheteriaException("Não foi possivel inserir na tIngressoCodigoBarra.");
                            }
                        }
                        else
                            codigoBarra = ingresso[CODIGO_BARRA].ToString();// Se não for imprimir, codigo de barras igual ao que está no ingresso -- pode ser lista branca!

                        string SQL = string.Empty;

                        if (!preReserva)
                            //Registra o ingresso em questão para o cliente e devolve o CodigoImpressao Atualizado
                            SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
											   @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                                vendaBilheteria.Control.ID +
                                                " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID + ", @UsuarioID = " + usuarioID +
                                                " , @IngressoID = " + (int)ingresso[INGRESSOID] + ", @GerenciamentoIngressosID = " + (int)ingresso[GERENCIAMENTO_INGRESSOS_ID];
                        else
                            SQL = string.Empty +
                                "UPDATE " +
                                "   tIngresso " +
                                "SET " +
                                "   LojaID = " + lojaID + ", " +
                                "   CodigoBarra = '" + codigoBarra + "', " +
                                "   VendaBilheteriaID = " + vendaBilheteria.Control.ID + ", " +
                                "   Status = '" + statusFinal + "' " +
                                ((clienteIngresso != 0) ? ", " +
                                "   ClienteID = " + clienteIngresso + " " : "") +
                                "WHERE " +
                                "   (UsuarioID = " + usuarioID + ") " +
                                "AND " +
                                "   (Status = '" + Ingresso.RESERVADO_PRE_RESERVA + "') " +
                                "AND " +
                                "   (ID = " + (int)ingresso[INGRESSOID] + ") " +
                                "AND " +
                                "   (GerenciamentoIngressosID = " + (int)ingresso[GERENCIAMENTO_INGRESSOS_ID] + ")";

                        object oRetornoUpdateIngresso = bd.ConsultaValor(SQL);

                        int codigoImpressao = -1;
                        if (!preReserva)
                        {
                            codigoImpressao = Convert.ToInt32(oRetornoUpdateIngresso);
                            if (codigoImpressao == -1)
                                throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas. (" + (int)ingressos[0][INGRESSOID] + "-" + usuarioID + ")");
                        }

                        if (statusFinal != Ingresso.VENDIDO && statusFinal != Ingresso.PRE_RESERVA)
                            retorno.IngressoXCodigoImpressao.Add((int)ingresso[INGRESSOID], codigoImpressao);

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)ingresso[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.BloqueioID.Valor = (int)ingresso[BLOQUEIOID];
                        ingressoLog.CortesiaID.Valor = (int)ingresso[CORTESIAID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID;
                        ingressoLog.LojaID.Valor = lojaID;
                        ingressoLog.CanalID.Valor = canalID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.ClienteID.Valor = clienteID;
                        ingressoLog.PrecoID.Valor = (int)ingresso[PRECOID];
                        ingressoLog.Acao.Valor = ((preReserva) ? IngressoLog.PRE_RESERVA : IngressoLog.VENDER);

                        if (bd.Executar(ingressoLog.StringInserir()) != 1)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                        if (imprimir && (!preReserva))
                        {
                            ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                            ingressoLog.VendaBilheteriaItemID.Valor = 0;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.CodigoBarra.Valor = codigoBarra;

                            if (!retorno.CodigosBarra.ContainsKey((int)ingresso[INGRESSOID]))
                                retorno.CodigosBarra.Add((int)ingresso[INGRESSOID], codigoBarra);

                            if (bd.Executar(ingressoLog.StringInserir()) != 1)
                                throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                        }

                        //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                        if ((int)ingresso[COTA_ITEM_ID] > 0 || (int)ingresso[COTA_ITEM_ID_APS] > 0)
                        {
                            EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)ingresso[INGRESSOID]).FirstOrDefault();

                            if (dono != null)
                            {
                                oIngressoCliente.ApresentacaoID.Valor = (int)ingresso[APRESENTACAOID];
                                oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingresso[APRESENTACAOSETORID];
                                oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                                oIngressoCliente.DonoID.Valor = dono.DonoID;
                                oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                                oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                                oIngressoCliente.CPF.Valor = dono.CPF;

                                bd.Executar(oIngressoCliente.StringInserir());

                                //Preenche os Objs
                                oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingresso[APRESENTACAOSETORID];
                                oCotaItemControle.ApresentacaoID.Valor = (int)ingresso[APRESENTACAOID];
                                //oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                                //Incrementa a quantidade da ApresentacaoSetor
                                if (dono.CotaItemIDAPS > 0)
                                {
                                    oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                    bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingresso[APRESENTACAOSETORID]);


                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                                }
                                if (dono.CotaItemID > 0)
                                {
                                    oCotaItemControle.CotaItemID.Valor = dono.CotaItemID;

                                    //Incrementa a quantidade da Apresentacao
                                    bd.Executar(oCotaItemControle.StringAtualizarAP());

                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingresso[APRESENTACAOID]);

                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                                }
                            }
                        }
                    }

                }
                #endregion

                #region Pacotes

                int PacoteGrupo = 0;
                int pacoteIDAtual = 0;
                foreach (DataRow item in itensPacote)
                {
                    DataRow[] ingressosPacote = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = (int)ingressosPacote[0][PACOTEID];
                    vendaBilheteriaItem.Acao.Valor = ((preReserva) ? VendaBilheteriaItem.PRE_RESERVA : VendaBilheteriaItem.VENDA);

                    if (pacoteIDAtual != vendaBilheteriaItem.PacoteID.Valor)
                    {
                        PacoteGrupo = 0;
                        pacoteIDAtual = vendaBilheteriaItem.PacoteID.Valor;
                    }
                    PacoteGrupo++;

                    vendaBilheteriaItem.PacoteGrupo.Valor = PacoteGrupo;

                    if (item[CONV] == DBNull.Value)
                        item[CONV] = 0;
                    if (item[VALOR_CONV] == DBNull.Value)
                        item[VALOR_CONV] = 0;
                    if (item[TAXA_COMISSAO] == DBNull.Value)
                        item[TAXA_COMISSAO] = 0;
                    if (item[COMISSAO_VALOR] == DBNull.Value)
                        item[COMISSAO_VALOR] = 0;

                    int conv = (int)item[CONV];
                    decimal valorConv = (decimal)item[VALOR_CONV];
                    int taxaComissao = (int)item[TAXA_COMISSAO];
                    decimal comissaovalor = (decimal)item[COMISSAO_VALOR];


                    if (!(bool)item["PermitirCancelamentoAvulso"])
                    {
                        vendaBilheteriaItem.TaxaConveniencia.Valor = conv;
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = valorConv;
                        vendaBilheteriaItem.TaxaComissao.Valor = taxaComissao;
                        vendaBilheteriaItem.ComissaoValor.Valor = comissaovalor;

                        idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                        if (idTemp == null)
                            throw new BilheteriaException("Item de Venda não foi gerada.");
                        else
                            if (Convert.ToInt32(idTemp) <= 0)
                                throw new BilheteriaException("Item de Venda não foi gerada.");

                        vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                    }


                    foreach (DataRow ingresso in ingressosPacote)
                    {
                        if ((bool)item["PermitirCancelamentoAvulso"])
                        {
                            CanalPacote canalPacote = new CanalPacote();

                            //busca as taxas de conveniencia e comissão e popula as variaveis
                            DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(canalID, vendaBilheteriaItem.PacoteID.Valor);
                            Preco precoIngresso = new Preco();
                            precoIngresso.Ler((int)ingresso["PrecoID"]);

                            int taxaConvenienciaPacote = (int)taxasPacote.Rows[0]["TaxaConveniencia"];
                            decimal taxaMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMinima"]);
                            decimal taxaMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMaxima"]);
                            int taxaComissaoPacote = (int)taxasPacote.Rows[0]["TaxaComissao"];
                            decimal comissaoMinPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMinima"]);
                            decimal comissaoMaxPacote = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMaxima"]);
                            decimal comissaoValorTotal = 0;
                            decimal taxaValorTotal = 0;

                            if (comissaovalor >= taxaComissaoPacote && taxaComissaoPacote != 0)
                                comissaoValorTotal = comissaovalor / ingressosPacote.Count();
                            else
                            {

                                if (taxaComissaoPacote > 0 || comissaoMinPacote > 0)
                                {
                                    decimal dAux = (taxaComissaoPacote / 100m) * precoIngresso.Valor.Valor;

                                    if (dAux < comissaoMinPacote)
                                        comissaoValorTotal += Decimal.Round(comissaoMinPacote, 2);
                                    else if (dAux > comissaoMaxPacote && comissaoMaxPacote > 0)
                                        comissaoValorTotal += Decimal.Round(comissaoMaxPacote, 2);
                                    else
                                        comissaoValorTotal += Decimal.Round(dAux, 2);
                                }
                            }
                            vendaBilheteriaItem.TaxaComissao.Valor = taxaComissaoPacote;

                            vendaBilheteriaItem.ComissaoValor.Valor = comissaoValorTotal;
                            //popula a conveniencia de acordo com a taxa minima e maxima
                            if (valorConv >= taxaMaxPacote && taxaMaxPacote != 0)
                                taxaValorTotal = valorConv / ingressosPacote.Count();
                            else
                            {
                                if (taxaConvenienciaPacote > 0 || taxaMinPacote > 0)
                                {
                                    decimal dAux = (taxaConvenienciaPacote / 100m) * precoIngresso.Valor.Valor;

                                    if (dAux < taxaMinPacote)
                                        taxaValorTotal += Decimal.Round(taxaMinPacote, 2);
                                    else if (dAux > taxaMaxPacote && taxaMaxPacote > 0)
                                        taxaValorTotal += Decimal.Round(taxaMaxPacote, 2);
                                    else
                                        taxaValorTotal += Decimal.Round(dAux, 2);
                                }
                            }

                            vendaBilheteriaItem.TaxaConveniencia.Valor = taxaConvenienciaPacote;
                            vendaBilheteriaItem.TaxaConvenienciaValor.Valor = taxaValorTotal;

                            idTemp = bd.ConsultaValor(vendaBilheteriaItem.StringInserir());

                            if (idTemp == null)
                                throw new BilheteriaException("Item de Venda não foi gerada.");
                            else
                                if (Convert.ToInt32(idTemp) <= 0)
                                    throw new BilheteriaException("Item de Venda não foi gerada.");

                            vendaBilheteriaItem.Control.ID = Convert.ToInt32(idTemp);
                        }

                        string SQL = string.Empty;

                        if (imprimir && tipoVenda != Canal.TipoDeVenda.ImpressaoVoucher && (!preReserva))
                        {
                            // Código de barras é gerado apenas quando há necessidade de registrar a impressão do ingresso.
                            if (Convert.ToChar(ingresso[TIPO_CODIGO_BARRA]) == (char)Enumerators.TipoCodigoBarra.Estruturado)
                                codigoBarra = oCodigoBarra.NovoCodigoBarraIngresso((int)ingresso[PRECOID], (int)ingresso[EVENTOID], (int)ingresso[CODIGO_SEQUENCIAL]);
                            else
                                codigoBarra = ingresso[CODIGO_BARRA].ToString();

                            //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                            if (codigoBarra.Trim().Length > 0)
                            {
                                IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                                ingressoCodigoBarra.EventoID.Valor = (int)ingresso[EVENTOID];
                                ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                                if (bd.Executar(ingressoCodigoBarra.StringInserir()) != 1)
                                    throw new BilheteriaException("Não foi possivel inserir na tIngressoCodigoBarra.");
                            }

                            SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
										   @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                            vendaBilheteria.Control.ID +
                                            " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID + ", @UsuarioID = " + usuarioID +
                                            " , @IngressoID = " + (int)ingresso[INGRESSOID] + ", @GerenciamentoIngressosID = " + (int)ingresso[GERENCIAMENTO_INGRESSOS_ID];
                        }
                        else
                        {
                            codigoBarra = ingresso[CODIGO_BARRA].ToString();

                            SQL = @"EXEC dbo.IncrementarUltimoCodigoImpressoVenda
										   @CodigoBarra = '" + codigoBarra + "' , @LojaID = " + lojaID + ", @VendaBilheteriaID = " +
                                            vendaBilheteria.Control.ID +
                                            " , @Status = '" + statusFinal + "', @ClienteID = " + clienteID + ", @UsuarioID = " + usuarioID +
                                            " , @IngressoID = " + (int)ingresso[INGRESSOID] + ", @GerenciamentoIngressosID = " + (int)ingresso[GERENCIAMENTO_INGRESSOS_ID];
                        }

                        int codigoImpressao = (int)bd.ConsultaValor(SQL);

                        if (codigoImpressao == -1)
                            throw new BilheteriaException("Status do ingresso não pôde ser atualizado. Por favor, refaça todas as reservas.(" + (int)ingresso[INGRESSOID] + "-" + usuarioID + ")");
                        else
                        {
                            if (statusFinal != Ingresso.VENDIDO)
                                retorno.IngressoXCodigoImpressao.Add((int)ingresso[INGRESSOID], codigoImpressao);
                            //inserir na Log
                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = (int)ingresso[INGRESSOID];
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.BloqueioID.Valor = (int)ingresso[BLOQUEIOID];
                            ingressoLog.CortesiaID.Valor = (int)ingresso[CORTESIAID];
                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            ingressoLog.CaixaID.Valor = caixaID;
                            ingressoLog.LojaID.Valor = lojaID;
                            ingressoLog.CanalID.Valor = canalID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.ClienteID.Valor = clienteID;
                            ingressoLog.PrecoID.Valor = (int)ingresso[PRECOID];
                            ingressoLog.Acao.Valor = ((preReserva) ? IngressoLog.PRE_RESERVA : IngressoLog.VENDER);

                            if (bd.Executar(ingressoLog.StringInserir()) != 1)
                                throw new BilheteriaException("Log de venda do ingresso não foi inserido.");

                            if (imprimir && (!preReserva))
                            {
                                ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;

                                ingressoLog.VendaBilheteriaItemID.Valor = 0;
                                ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                ingressoLog.CodigoBarra.Valor = codigoBarra;

                                if (!retorno.CodigosBarra.ContainsKey((int)ingresso[INGRESSOID]))
                                    retorno.CodigosBarra.Add((int)ingresso[INGRESSOID], codigoBarra);

                                if (bd.Executar(ingressoLog.StringInserir()) != 1)
                                    throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                            }
                        }

                        //Incrementa Quantidades Vendidas tCotaItemControle e tIngressoCliente
                        if ((int)ingresso[COTA_ITEM_ID] > 0 || (int)ingresso[COTA_ITEM_ID_APS] > 0)
                        {
                            EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)ingresso[INGRESSOID]).FirstOrDefault();
                            if (dono != null)
                            {
                                //Preenche os Objs
                                oCotaItemControle.ApresentacaoSetorID.Valor = (int)ingresso[APRESENTACAOSETORID];
                                oCotaItemControle.ApresentacaoID.Valor = (int)ingresso[APRESENTACAOID];
                                oCotaItemControle.CotaItemID.Valor = (int)ingresso[COTA_ITEM_ID];
                                oIngressoCliente.ApresentacaoID.Valor = (int)ingresso[APRESENTACAOID];
                                oIngressoCliente.ApresentacaoSetorID.Valor = (int)ingresso[APRESENTACAOSETORID];
                                oIngressoCliente.CotaItemID.Valor = dono.CotaItemID;
                                oIngressoCliente.DonoID.Valor = dono.DonoID;
                                oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                                oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                                oIngressoCliente.CPF.Valor = dono.CPF;

                                bd.Executar(oIngressoCliente.StringInserir());

                                //Incrementa a quantidade da ApresentacaoSetor                                
                                if (bd.Executar(oCotaItemControle.StringAtualizarAPS()) > 0)
                                {
                                    //Atualizou agora verifica a Quantidade
                                    stbSQL = new StringBuilder();
                                    stbSQL.Append("SELECT ");
                                    stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                    stbSQL.Append("THEN 0 ");
                                    stbSQL.Append("ELSE 1 ");
                                    stbSQL.Append("END AS Valido ");
                                    stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                    stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                    stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, ingresso[APRESENTACAOSETORID]);

                                    if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                        throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                                }
                                else
                                {
                                    //Incrementa a quantidade da Apresentacao

                                    if (bd.Executar(oCotaItemControle.StringAtualizarAP()) > 0)
                                    {
                                        //Atualizou agora verifica a Quantidade
                                        stbSQL = new StringBuilder();
                                        stbSQL.Append("SELECT ");
                                        stbSQL.Append("CASE WHEN tApresentacao.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacao.Quantidade ");
                                        stbSQL.Append("THEN 0 ");
                                        stbSQL.Append("ELSE 1 ");
                                        stbSQL.Append("END AS Valido ");
                                        stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                        stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                        stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, ingresso[APRESENTACAOID]);

                                        if (!Convert.ToBoolean(bd.Executar(stbSQL.ToString())))
                                            throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentacao.");
                                    }
                                }
                            }
                        }
                    }

                }
                #endregion

                #region Manipulação de formas de pagamento
                //VendaBilheteriaFormaPagamento
                //dividir o valorTotal nas formas de pagamento
                decimal porcentagemTotal = 0;
                decimal valorAtual = valorTotal;

                troco = Math.Abs(troco);

                ValeIngressoLog valeIngressoLog;
                for (int i = 0; i < pagamentos.Rows.Count; i++)
                {
                    DataRow pagto = pagamentos.Rows[i];
                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];
                    if (DINHEIRO == (int)pagto["ID"])
                        vendaBilheteriaFormaPagamento.Valor.Valor = ((decimal)pagto[VALOR] - troco);

                    //#VIR:
                    else if (VIR == Convert.ToInt32(pagto["ID"]))
                    {
                        vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto[VALOR] > valorAtual ? valorAtual : (decimal)pagto[VALOR];
                        if (Convert.ToInt32(pagto["VirID"]) == 0 && codigoTrocaFixo.Length == 0)
                            throw new Exception("A forma de pagamento Vale ingresso foi informada no entanto o código de troca não foi encontrado.");
                    }
                    else
                        vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto[VALOR];

                    valorAtual = valorAtual - vendaBilheteriaFormaPagamento.Valor.Valor;

                    //calcular porcentagem
                    decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / valorTotal;
                    decimal porcentagem = Math.Round(porc, 2);

                    //a porcentagem final tem q dar 100%
                    if (i != (pagamentos.Rows.Count - 1))
                    {
                        porcentagemTotal += porcentagem;
                    }
                    else
                    {
                        //eh a ultima parcela
                        decimal totalTmp = porcentagemTotal + porcentagem;
                        if (totalTmp != 100)
                        {
                            porcentagem = 100 - porcentagemTotal;
                            porcentagemTotal += porcentagem;
                        }
                    }

                    int CartaoID = Convert.ToInt32(pagto["CartaoID"]);

                    #region BlackList - Insere Cartão na tBlackList

                    Cartao oCartao = new Cartao();

                    if (Convert.ToInt32(pagto["CartaoID"]) == 0 && AntiFraude)
                        if (pagto["NumeroCartao"].ToString().Length > 0 && Convert.ToInt32(pagto["ID"]) > 0 && clienteID > 0)
                            CartaoID = oCartao.InserirCartao(pagto["NumeroCartao"].ToString(), Convert.ToInt32(pagto["ID"]), clienteID, NomeCartao);

                    #endregion

                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.Dias.Valor = (int)pagto["Dias"];
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = (decimal)pagto["TaxaAdm"];
                    vendaBilheteriaFormaPagamento.IR.Valor = (string)pagto["IR"] == "T" ? true : false;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = DateTime.Now.AddDays(vendaBilheteriaFormaPagamento.Dias.Valor);
                    vendaBilheteriaFormaPagamento.CartaoID.Valor = CartaoID;
                    vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = CodigoAutorizacaoCredito.ToString();
                    vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = MensagemRetorno;
                    vendaBilheteriaFormaPagamento.HoraTransacao.Valor = HoraTransacao;
                    vendaBilheteriaFormaPagamento.DataTransacao.Valor = HoraTransacao;
                    vendaBilheteriaFormaPagamento.CodigoIR.Valor = CodigoIR;
                    vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = NumeroAutorizacao;
                    vendaBilheteriaFormaPagamento.NSUHost.Valor = NSUHost.ToString();
                    vendaBilheteriaFormaPagamento.NSUSitef.Valor = NSUSitef.ToString();
                    vendaBilheteriaFormaPagamento.Cupom.Valor = NotaFiscalCliente;
                    vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = DadosConfirmacaoVenda;
                    vendaBilheteriaFormaPagamento.Rede.Valor = Rede;
                    vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = CodigoRespostaTransacao;

                    //#VIR:
                    if (codigoTrocaFixo.Length > 0)
                    {
                        ValeIngresso oValeIngresso = new ValeIngresso();

                        EstruturaTrocaValeIngresso oTrocaFixa = oValeIngresso.ValorIDVIR(codigoTrocaFixo);
                        if (!oTrocaFixa.Encontrado)
                            throw new BilheteriaException("Vale Ingresso não encontrado");

                        vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = oTrocaFixa.ID;

                        string SQL = "UPDATE tValeIngresso SET Status = '" + (char)ValeIngresso.enumStatus.Trocado + "' " +
                                    "WHERE ID = " + oTrocaFixa.ID + " AND Status <> '" + (char)ValeIngresso.enumStatus.Trocado + "'";

                        if (bd.Executar(SQL) != 1)
                            throw new BilheteriaException("Nao foi possivel Trocar o Vale Ingresso - A Alteracao do Status gerou um erro");
                        else
                        {
                            valeIngressoLog = new ValeIngressoLog();
                            valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Trocar).ToString();
                            valeIngressoLog.TimeStamp.Valor = DateTime.Now;
                            valeIngressoLog.ValeIngressoID.Valor = oTrocaFixa.ID;
                            valeIngressoLog.UsuarioID.Valor = usuarioID;
                            valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            valeIngressoLog.VendaBilheteriaItemID.Valor = 0;
                            valeIngressoLog.EmpresaID.Valor = empresaID;
                            valeIngressoLog.CaixaID.Valor = caixaID;
                            valeIngressoLog.LojaID.Valor = lojaID;
                            valeIngressoLog.CanalID.Valor = canalID;
                            valeIngressoLog.CodigoTroca.Valor = codigoTrocaFixo;
                            valeIngressoLog.CodigoBarra.Valor = string.Empty;
                            valeIngressoLog.ClienteNome.Valor = string.Empty;
                            valeIngressoLog.Obs.Valor = string.Empty;


                            if (bd.Executar(valeIngressoLog.StringInserir()) == 0)
                                throw new Exception("Não foi possivel gerar o Log do Vale Ingresso, Troca impossível.");

                            codigoTrocaFixo = string.Empty;
                            VirIds.Clear();
                        }
                    }
                    else if (VirIds.Count > 0)
                    {
                        foreach (int virID in VirIds)
                        {
                            if (virID == (int)pagto["VirID"])
                            {
                                vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = virID;
                                string SQL = "UPDATE tValeIngresso SET Status = '" + (char)ValeIngresso.enumStatus.Trocado + "' " +
                                    "WHERE ID = " + virID + " AND Status <> '" + (char)ValeIngresso.enumStatus.Trocado + "'";

                                if (bd.Executar(SQL) != 1)
                                    throw new BilheteriaException("Nao foi possível Trocar o Vale Ingresso - A mudança do Status gerou um erro.");
                                else
                                {
                                    valeIngressoLog = new ValeIngressoLog();
                                    valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Trocar).ToString();
                                    valeIngressoLog.TimeStamp.Valor = DateTime.Now;
                                    valeIngressoLog.ValeIngressoID.Valor = virID;
                                    valeIngressoLog.UsuarioID.Valor = usuarioID;
                                    valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                    valeIngressoLog.VendaBilheteriaItemID.Valor = 0;
                                    valeIngressoLog.EmpresaID.Valor = empresaID;
                                    valeIngressoLog.CaixaID.Valor = caixaID;
                                    valeIngressoLog.LojaID.Valor = lojaID;
                                    valeIngressoLog.CanalID.Valor = canalID;
                                    valeIngressoLog.CodigoTroca.Valor = string.Empty;
                                    valeIngressoLog.CodigoBarra.Valor = string.Empty;
                                    valeIngressoLog.ClienteNome.Valor = string.Empty;
                                    valeIngressoLog.Obs.Valor = string.Empty;


                                    if (bd.Executar(valeIngressoLog.StringInserir()) == 0)
                                        throw new Exception("Não foi possivel gerar o Log do Vale Ingresso, Troca impossível.");

                                    VirIds.Remove(virID);
                                    break;
                                }
                            }
                        }
                    }
                    else
                        vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = 0;

                    if (bd.Executar(vendaBilheteriaFormaPagamento.StringInserir()) < 1)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");
                }

                #endregion

                if (celular.Length > 0 && vendaBilheteriaStatus == (string)VendaBilheteria.PAGO)
                {
                    new EnviaSMS().EnviaSms(false, vendaBilheteria.Control.ID, celular);
                }

                bd.FinalizarTransacao();

                try
                {
                    retorno.ComprouSeguro = true;

                    if (SeguroMondial > 0)
                    {
                        EstruturaInfoEventoMondial estruturaInfo = new EstruturaInfoEventoMondial();
                        Cliente oCliente = new Cliente();

                        oCliente.Ler(clienteID);

                        TicketProtectorWebService objService = new TicketProtectorWebService();
                        objService.Url = ConfigurationManager.AppSettings["URLMondial"];

                        string ProductID = ConfigurationManager.AppSettings["ProductID"];
                        decimal productCost = SeguroMondial / ingressosVendidos.Rows.Count;

                        MDLPolicyRequest mdlPolicyRequest = new MDLPolicyRequest();

                        mdlPolicyRequest.CompanyID = ConfigurationManager.AppSettings["CompanyID"];
                        mdlPolicyRequest.AuthenticationCode = ConfigurationManager.AppSettings["AuthenticationCode"];

                        mdlPolicyRequest.CreditCardExpMonth = string.Empty;
                        mdlPolicyRequest.CreditCardExpYear = string.Empty;
                        mdlPolicyRequest.CreditCardHolderName = oCliente.Nome.Valor;
                        mdlPolicyRequest.CreditCardNumber = string.Empty;
                        mdlPolicyRequest.CreditCardSecurityCode = string.Empty;

                        mdlPolicyRequest.CreditCardType = string.Empty; // 1: VISA; 2: MASTER; 3: AMEX; 5: HIPERCARD
                        mdlPolicyRequest.PaymentTypeID = 2; // 1: Cartão de Crédito; 2: Pagamento automático

                        // Endereço segurado
                        mdlPolicyRequest.CustomerAddresCompliment = oCliente.ComplementoCliente.Valor;
                        mdlPolicyRequest.CustomerAddress = oCliente.EnderecoCliente.Valor;
                        mdlPolicyRequest.CustomerAddressNumber = oCliente.NumeroCliente.Valor;
                        mdlPolicyRequest.CustomerCity = oCliente.CidadeCliente.Valor;
                        mdlPolicyRequest.CustomerDistrict = oCliente.BairroCliente.Valor;
                        mdlPolicyRequest.CustomerState = oCliente.EstadoCliente.Valor;
                        mdlPolicyRequest.CustomerZipCode = oCliente.CEPCliente.Valor;
                        mdlPolicyRequest.CustomerBirthDate = DateTime.Parse(oCliente.DataNascimento.ToString());

                        // Dados comprador
                        mdlPolicyRequest.CustomerDocumentID = oCliente.CPF.Valor;
                        mdlPolicyRequest.CustomerEmail = oCliente.Email.Valor;
                        mdlPolicyRequest.CustomerMobilePhone = oCliente.Celular.Valor;
                        mdlPolicyRequest.CustomerName = oCliente.Nome.Valor;
                        mdlPolicyRequest.CustomerPhone = oCliente.Telefone.Valor;

                        // Apólice primária, no caso da Ingresso Rápido, sempre passar vazio
                        mdlPolicyRequest.PrimaryPolicyID = string.Empty;

                        // Taxas
                        mdlPolicyRequest.RateOfConvenience = taxaConvenienciaValorTotal;
                        mdlPolicyRequest.RateOfDelivery = valorEntrega;

                        // Identificador da compra na Ingresso Rápido
                        mdlPolicyRequest.ReferenceOrderCode = vendaBilheteria.Control.ID.ToString();

                        mdlPolicyRequest.RequestDate = DateTime.Now;
                        mdlPolicyRequest.RequestOriginID = 1; // No caso da Ingresso Rápido, sempre 1
                        mdlPolicyRequest.RequestStatus = 1; // Booking Path (7): Pronto para cobrança do cartão via gateway
                        // Pagamento automático (1): Compras sem pagamento (faturamento inverso)

                        // Valor
                        mdlPolicyRequest.TotalCost = SeguroMondial; // Valor dos produtos * qtde. de segurados

                        // Instanciar cada segurado.
                        MDLCustomer[] mdlCustomer = new MDLCustomer[ingressosVendidos.Rows.Count];

                        int cont = 0;
                        int ApresentacaoSetorID = 0;

                        foreach (DataRow item in ingressosVendidos.Rows)
                        {
                            if (ApresentacaoSetorID != (int)item["ApresentacaoSetorID"])
                            {
                                estruturaInfo = new ApresentacaoSetor().ConsultaInfoEvento((int)item["ApresentacaoSetorID"]);
                                ApresentacaoSetorID = (int)item["ApresentacaoSetorID"];
                            }

                            mdlCustomer[cont] = new MDLCustomer();
                            mdlCustomer[cont].ClientTicketNumber = Convert.ToString(item["IngressoID"]); // Identificador do Ingresso
                            mdlCustomer[cont].IndividualCost = productCost;

                            // Dados pessoais do cliente 1
                            mdlCustomer[cont].InsuredBirthDate = DateTime.Parse(oCliente.DataNascimento.ToString()); // Data Nascimento
                            mdlCustomer[cont].InsuredDocumentID = oCliente.CPF.Valor; // CPF
                            mdlCustomer[cont].InsuredID = 0; // No caso da ingresso Rápido, sempre 0
                            mdlCustomer[cont].InsuredName = oCliente.Nome.Valor; // Nome do Cliente
                            mdlCustomer[cont].ProductID = ProductID; // Produto Mondial

                            // Dados do Evento
                            mdlCustomer[cont].EventClave = estruturaInfo.ApresentacaoID.ToString(); // Código do Evento. Este é o identificador do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo evento!!
                            mdlCustomer[cont].EventName = estruturaInfo.Evento;
                            mdlCustomer[cont].EventDate = estruturaInfo.Horario;
                            mdlCustomer[cont].EventID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do evento é o campo "EventClave"

                            // Dados do setor
                            mdlCustomer[cont].SectorCode = estruturaInfo.SetorID + "_" + item["PrecoID"]; // Código do Setor. Este é o identificador do setor do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo setor!!
                            mdlCustomer[cont].SectorName = estruturaInfo.Setor;

                            mdlCustomer[cont].SectorPrice = estruturaInfo.Valor;

                            mdlCustomer[cont].EventLocal = estruturaInfo.Local;
                            mdlCustomer[cont].SectorID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do setor é o campo "SectorCode"

                            cont++;
                        }

                        mdlPolicyRequest.CustomerCollection = mdlCustomer;

                        MDLRequestResult[] mdlRequestResult;
                        mdlRequestResult = objService.CreatePolicy(mdlPolicyRequest);

                        if (mdlRequestResult.Length > 0)
                            new ApoliceMondial().InserirLista(mdlRequestResult, vendaBilheteria.Control.ID);
                    }
                }
                catch (Exception)
                {
                    retorno.ComprouSeguro = false;
                }

                string SQLvenda = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                idTemp = bd.ConsultaValor(SQLvenda);

                retorno.Senha = (idTemp != null) ? idTemp.ToString() : null;

                return retorno;
            }
            catch (Exception ex)
            {
                try
                {
                    bd.DesfazerTransacao();
                }
                catch (ArgumentException) { } // transação não específicada
                catch (InvalidOperationException) { }// Já foi dado o commit ou o rollback
                catch (Exception ex1)
                {
                    throw new BilheteriaException(ex.Message + Environment.NewLine + ex1.Message);
                }

                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        ///		Verifica se existe um caixa aberto para o Usuário, se existe verifica se é de hoje, e se não for, fecha o caixa atual e 
        ///		abre um novo caixa.
        /// </summary>
        /// <returns></returns>
        public int VerificaCaixaInternet()
        {
            Servidor servidor = new Servidor();
            Caixa caixa = new Caixa(Usuario.INTERNET_USUARIO_ID);

            bool caixaAberto = caixa.Aberto(Usuario.INTERNET_USUARIO_ID);

            if (caixaAberto)
            {
                if (caixa.LojaID.Valor == Loja.INTERNET_LOJA_ID)
                {
                    if (caixa.DataAbertura.Valor < servidor.Hoje)
                    {
                        caixa.Fechar();
                        caixa.Control.ID = 0;
                    }
                }
                else
                {
                    caixa.Fechar();
                    caixa.Control.ID = 0;
                }
            }

            if (caixa.Control.ID == 0)
                caixa.Abrir(Usuario.INTERNET_USUARIO_ID, Loja.INTERNET_LOJA_ID, 0, 0);

            return caixa.Control.ID;
        }

        /// <summary>
        ///		Verifica se existe um caixa aberto para o Usuário, se existe verifica se é de hoje, e se não for, fecha o caixa atual e 
        ///		abre um novo caixa. PS: MOBILE
        /// </summary>
        /// <returns></returns>
        //public int VerificaCaixaMobile()
        //{
        //    Servidor servidor = new Servidor();
        //    Caixa caixa = new Caixa(MOBILE_USUARIO_ID);

        //    bool caixaAberto = caixa.Aberto(MOBILE_USUARIO_ID);

        //    if (caixaAberto)
        //    {
        //        if (caixa.LojaID.Valor == MOBILE_LOJA_ID)
        //        {
        //            if (caixa.DataAbertura.Valor < servidor.Hoje)
        //            {
        //                caixa.Fechar();
        //                caixa.Control.ID = 0;
        //            }
        //        }
        //        else
        //        {
        //            caixa.Fechar();
        //            caixa.Control.ID = 0;
        //        }
        //    }

        //    if (caixa.Control.ID == 0)
        //        caixa.Abrir(MOBILE_USUARIO_ID, MOBILE_LOJA_ID, 0, 0);

        //    return caixa.Control.ID;
        //}


        /// <summary>
        /// Este metodo carrega todas as tabelas necessarias da bilheteria: locais, eventos, setores, apresentacoes, pacotes...
        /// </summary>
        /// <param name="usuarioID">Usuario logado no sistema</param>
        /// <returns></returns>
        public DataSet Carregar(int usuarioID, int canalID, int empresaID, Canal.TipoDeVenda tipoDeVenda)
        {
            BD bd = null;

            SqlConnection connection = null;

            SqlCommand comm = null;
            SqlDataAdapter adapter = null;

            try
            {
                bd = new BD();
                DateTime inicio = DateTime.Now;
                string hoje = System.DateTime.Today.ToString("yyyyMMdd");
                string sql;

                bd.Fechar();
                if (usuarioID == 0)
                    throw new BilheteriaException("Usuário incorreto!");

                this.CancelarReservas(usuarioID);

                DataSet buffer = new DataSet("Buffer");

                DataTable formasPagamento = new DataTable("FormaPagamento");
                formasPagamento.Columns.Add("ID", typeof(int));
                formasPagamento.Columns.Add("Nome", typeof(string));
                formasPagamento.Columns.Add("Tipo", typeof(int));
                formasPagamento.Columns.Add("Parcelas", typeof(int));
                formasPagamento.Columns.Add("TEFID", typeof(int));
                formasPagamento.Columns.Add("Dias", typeof(int));
                formasPagamento.Columns.Add("TaxaAdm", typeof(decimal));
                formasPagamento.Columns.Add("IR", typeof(string));
                formasPagamento.Columns.Add("Itau", typeof(bool));
                formasPagamento.Columns.Add("BandeiraNome", typeof(string));

                DataTable formasPagamentoEvento = new DataTable("FormaPagamentoEvento");
                formasPagamentoEvento.Columns.Add("FormaPagamentoID", typeof(int));
                formasPagamentoEvento.Columns.Add("EventoID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("Logradouro", typeof(string));
                locais.Columns.Add("Numero", typeof(int));
                locais.Columns.Add("Complemento", typeof(string));
                locais.Columns.Add("Bairro", typeof(string));
                locais.Columns.Add("CEP", typeof(string));
                locais.Columns.Add("Cidade", typeof(string));
                locais.Columns.Add("Estado", typeof(string));
                //não carregar colunas abaixo
                locais.Columns.Add("Estacionamento", typeof(bool));
                locais.Columns.Add("EstacionamentoObs", typeof(string));
                locais.Columns.Add("AcessoNecessidadeEspecial", typeof(bool));
                locais.Columns.Add("AcessoNecessidadeEspecialObs", typeof(string));
                locais.Columns.Add("ArCondicionado", typeof(bool));
                locais.Columns.Add("ComoChegar", typeof(string));
                locais.Columns.Add("HorariosBilheteria", typeof(string));
                locais.Columns.Add("LocalFormasPagamento", typeof(string));
                locais.Columns.Add("Carregou", typeof(bool));

                DataTable horariosLocal = new DataTable("LocalHorario");
                horariosLocal.Columns.Add("LocalID", typeof(int));
                horariosLocal.Columns.Add("DiaSemana", typeof(int));
                horariosLocal.Columns.Add("HoraInicio", typeof(DateTime));
                horariosLocal.Columns.Add("HoraFim", typeof(DateTime));

                DataTable formaPagamentoLocal = new DataTable("LocalFormaPagamento");
                formaPagamentoLocal.Columns.Add("LocalID", typeof(int));
                formaPagamentoLocal.Columns.Add("Nome", typeof(string));

                DataTable eventos = this.EstruturaDttEvento();
                DataTable apresentacoes = this.EstruturaDttApresentacao();
                DataTable setores = this.EstruturaDttSetor();
                DataTable precos = this.EstruturaDttPreco();


                DataTable cortesias = new DataTable("Cortesia");
                cortesias.Columns.Add("ID", typeof(int));
                cortesias.Columns.Add("Nome", typeof(string));
                cortesias.Columns.Add("LocalID", typeof(int));

                DataTable bloqueios = new DataTable("Bloqueio");
                bloqueios.Columns.Add("ID", typeof(int));
                bloqueios.Columns.Add("Nome", typeof(string));
                bloqueios.Columns.Add("CorID", typeof(int));
                bloqueios.Columns.Add("RGB", typeof(string));
                bloqueios.Columns.Add("LocalID", typeof(int));

                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));
                pacotes.Columns.Add("Valor", typeof(decimal));
                pacotes.Columns.Add("LocalID", typeof(int));
                pacotes.Columns.Add("Obs", typeof(string));
                pacotes.Columns.Add("TaxaConveniencia", typeof(int));
                pacotes.Columns.Add("TaxaMinima", typeof(decimal));
                pacotes.Columns.Add("TaxaMaxima", typeof(decimal));
                pacotes.Columns.Add("PermitirCancelamentoAvulso", typeof(bool));

                DataTable pacotesitens = new DataTable("PacoteItem");
                pacotesitens.Columns.Add("PacoteID", typeof(int));
                pacotesitens.Columns.Add("EventoID", typeof(int));
                pacotesitens.Columns.Add("SetorID", typeof(int));
                pacotesitens.Columns.Add("LocalID", typeof(int));
                pacotesitens.Columns.Add("SetorNome", typeof(string));

                DataTable pacotesLugarMarcado = new DataTable("PacoteLugarMarcado");
                pacotesLugarMarcado.Columns.Add("PacoteID", typeof(int));
                pacotesLugarMarcado.Columns.Add("SetorID", typeof(int));
                pacotesLugarMarcado.Columns.Add("ApresentacaoID", typeof(int));
                pacotesLugarMarcado.Columns.Add("ApresentacaoSetorID", typeof(int));
                pacotesLugarMarcado.Columns.Add("LugarMarcado", typeof(string));

                DataTable series = new DataTable("Serie");
                series.Columns.Add("ID", typeof(int));
                series.Columns.Add("Nome", typeof(string));
                series.Columns.Add("QuantidadeMinimaGrupo", typeof(int));
                series.Columns.Add("QuantidadeMaximaGrupo", typeof(int));
                series.Columns.Add("QuantidadeMinimaApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMaximaApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMinimaIngressosPorApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMaximaIngressosPorApresentacao", typeof(int));
                series.Columns.Add("Descricao", typeof(string));
                series.Columns.Add("Regras", typeof(string));

                DataTable mapas = new DataTable("Mapa");
                mapas.Columns.Add("ApresentacaoSetorID", typeof(int));
                mapas.Columns.Add("Mapa", typeof(string));

                DataTable regioes = new DataTable("Regiao");
                regioes.Columns.Add("ID", typeof(int));
                regioes.Columns.Add("Nome", typeof(string));
                regioes.Columns.Add("EventoID", typeof(int));
                regioes.Columns.Add("TaxaEntregaID", typeof(int));

                DataTable taxasEntrega = new DataTable("TaxaEntrega");
                taxasEntrega.Columns.Add("ID", typeof(int));
                taxasEntrega.Columns.Add("Nome", typeof(string));
                taxasEntrega.Columns.Add("Valor", typeof(decimal));
                taxasEntrega.Columns.Add("RegiaoID", typeof(int));
                taxasEntrega.Columns.Add("EventoID", typeof(int));
                taxasEntrega.Columns.Add("Prazo", typeof(int));
                taxasEntrega.Columns.Add("Estado", typeof(string));

                //mesma estrutura da taxa entrega
                DataTable taxaEntregaPadrao = new DataTable("TaxaEntregaPadrao");
                taxaEntregaPadrao.Columns.Add("ID", typeof(int));
                taxaEntregaPadrao.Columns.Add("Nome", typeof(string));
                taxaEntregaPadrao.Columns.Add("Valor", typeof(decimal));
                taxaEntregaPadrao.Columns.Add("RegiaoID", typeof(int));
                taxaEntregaPadrao.Columns.Add("EventoID", typeof(int));
                taxaEntregaPadrao.Columns.Add("Prazo", typeof(int));
                taxaEntregaPadrao.Columns.Add("Estado", typeof(string));

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID", typeof(int));
                valeIngressoTipo.Columns.Add("Nome", typeof(string));
                valeIngressoTipo.Columns.Add("Valor", typeof(decimal));
                valeIngressoTipo.Columns.Add("ValidadeDiasImpressao", typeof(int));
                valeIngressoTipo.Columns.Add("ValidadeData", typeof(string));
                valeIngressoTipo.Columns.Add("ClienteTipo", typeof(string));
                valeIngressoTipo.Columns.Add("ProcedimentoTroca", typeof(string));
                valeIngressoTipo.Columns.Add("SaudacaoNominal", typeof(string));
                valeIngressoTipo.Columns.Add("SaudacaoPadrao", typeof(string));
                valeIngressoTipo.Columns.Add("QuantidadeLimitada", typeof(string));
                valeIngressoTipo.Columns.Add("CodigoTrocaFixo", typeof(string));
                valeIngressoTipo.Columns.Add("Acumulativo", typeof(string));
                valeIngressoTipo.Columns.Add("Quantidade", typeof(string)).DefaultValue = " - ";
                valeIngressoTipo.Columns.Add("ValorPagamento", typeof(decimal));
                valeIngressoTipo.Columns.Add("ValorTipo", typeof(char));
                valeIngressoTipo.Columns.Add("TrocaConveniencia", typeof(bool));
                valeIngressoTipo.Columns.Add("TrocaEntrega", typeof(bool));
                valeIngressoTipo.Columns.Add("TrocaIngresso", typeof(bool));


                DataTable regiaoValeIngresso = new DataTable("RegiaoValeIngresso");
                regiaoValeIngresso.Columns.Add("ID", typeof(int));
                regiaoValeIngresso.Columns.Add("Nome", typeof(string));

                DataTable cotaItem = new DataTable("CotaItem");
                cotaItem.Columns.Add("ID", typeof(int));
                cotaItem.Columns.Add("CotaID", typeof(int));
                cotaItem.Columns.Add("PrecoIniciaCom", typeof(string));
                cotaItem.Columns.Add("ParceiroID", typeof(int));
                cotaItem.Columns.Add("ValidaBin", typeof(bool));
                cotaItem.Columns.Add("TextoValidacao", typeof(string));
                cotaItem.Columns.Add("ObrigatoriedadeID", typeof(int));
                cotaItem.Columns.Add("Quantidade", typeof(int));
                cotaItem.Columns.Add("QuantidadePorCliente", typeof(int));
                cotaItem.Columns.Add("CPFResponsavel", typeof(bool));
                cotaItem.Columns.Add("Termo", typeof(string));
                cotaItem.Columns.Add("Nominal", typeof(bool));
                cotaItem.Columns.Add("QuantidadePorCodigo", typeof(int));

                DataTable cotaItemFormaPagamento = new DataTable("CotaItemFormaPagamento");
                cotaItemFormaPagamento.Columns.Add("CotaItemID", typeof(int));
                cotaItemFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));

                DataTable codigoPromo = new DataTable("CodigoPromo");
                codigoPromo.Columns.Add("CodigoPromoID", typeof(int));
                codigoPromo.Columns.Add("ParceiroID", typeof(int));
                codigoPromo.Columns.Add("Codigo", typeof(string));
                codigoPromo.Columns.Add("TamanhoCodigo", typeof(string));

                DataTable obrigatoriedade = Obrigatoriedade.Estrutura();

                connection = (SqlConnection)bd.Cnn;

                comm = connection.CreateCommand();
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                // Carregar informações de formas de pagamento do canal
                sql = string.Empty +
                    "EXECUTE bilheteria_FormaPagamentoCanal " +
                    "   @EmpresaID = " + empresaID + ", " +
                    "   @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(formasPagamento);

                if (formasPagamento.Rows.Count == 0)
                    throw new BilheteriaException("Verifique se as formas de pagamento foram liberadas para seu canal.\n");
                else
                {
                    DataRow branco = formasPagamento.NewRow();
                    branco["ID"] = 0;
                    branco["Nome"] = string.Empty;
                    formasPagamento.Rows.InsertAt(branco, 0);
                }

                // Regioes utilizadas para o vir
                sql = string.Empty +
                    "EXECUTE RegioesVir ";
                comm.CommandText = sql;
                adapter.Fill(regiaoValeIngresso);

                // Taxas de entrega padrao para usar no vale ingresso
                sql = string.Empty +
                    "EXECUTE TaxasEntregaPadrao";

                comm.CommandText = sql;
                adapter.Fill(taxaEntregaPadrao);


                // Locais
                comm.CommandText = string.Empty +
                    "EXECUTE bilheteria_Locais " +
                    "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                adapter.Fill(locais);

                //Eventos
                comm.CommandText = string.Empty +
                    "EXECUTE bilheteria_Eventos " +
                    "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                adapter.Fill(eventos);


                // Apresentações
                sql = string.Empty +
                    "EXECUTE bilheteria_Apresentacoes " +
                    "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                bd.Consulta(sql);

                DataRow apresentacao = null;

                if (bd.Consulta().Read())
                {
                    do
                    {
                        apresentacao = apresentacoes.NewRow();
                        apresentacoes.Rows.Add(apresentacao);

                        apresentacao["ID"] = bd.LerInt("ID");
                        apresentacao["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
                        apresentacao["EventoID"] = bd.LerInt("EventoID");
                        apresentacao["Descricao"] = bd.LerString("Descricao");
                        apresentacao["DescricaoPadrao"] = bd.LerBoolean("DescricaoPadrao");
                        apresentacao["DescricaoPadraoApresentacao"] = bd.LerString("DescricaoPadraoApresentacao");
                        apresentacao["QuantidadeApresentacao"] = bd.LerInt("QuantidadeApresentacao");
                        apresentacao["QuantidadePorClienteApresentacao"] = bd.LerInt("QuantidadePorClienteApresentacao");
                        apresentacao["ApresentacaoCotaID"] = bd.LerInt("ApresentacaoCotaID");
                    } while (bd.Consulta().Read());
                }

                bd.FecharConsulta();


                // Setores
                comm.CommandText = string.Empty +
                    "EXECUTE bilheteria_Setores " +
                    "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                adapter.Fill(setores);

                // Preços
                comm.CommandText = string.Empty +
                    "EXECUTE bilheteria_Precos " +
                    "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                adapter.Fill(precos);

                // Carrega as cortesias disponíveis.
                sql = string.Empty +
                    "EXECUTE bilheteria_Cortesias " +
                    "   @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(cortesias);

                // Carrega os bloqueios.
                sql = string.Empty +
                    "EXECUTE bilheteria_Bloqueios " +
                    "   @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(bloqueios);

                //Carrega informações de pacotes com lugar marcado
                sql = string.Empty +
                    "EXECUTE bilheteria_PacotesLugarMarcado " +
                    "   @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(pacotesLugarMarcado);

                //Carrega informações dos setores dos pacotes
                sql = string.Empty +
                    "EXECUTE bilheteria_PacotesItem " +
                    "   @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(pacotesitens);

                //Carrega informações dos Vale Ingressos
                sql = string.Empty +
                    "EXECUTE bilheteria_ValeIngressos2 " +
                   "   @CanalID = " + canalID + ", " +
                    "   @Data = '" + hoje + "'";
                comm.CommandText = sql;
                adapter.Fill(valeIngressoTipo);

                // Carrega informações de pacotes.
                sql = string.Empty +
                    "EXECUTE bilheteria_Pacotes " +
                    "   @CanalID = " + canalID;
                bd.Consulta(sql);

                // Popula o datatable manualmente. 
                //TODO: Mudar isso.
                while (bd.Consulta().Read())
                {
                    if (locais.Select("ID=" + bd.LerInt("LocalID")).Length == 0)
                    {
                        DataRow local = locais.NewRow();
                        local["ID"] = bd.LerInt("LocalID");
                        local["Nome"] = bd.LerString("Local");
                        local["Estado"] = bd.LerString("Estado");
                        locais.Rows.Add(local);
                    }

                    DataRow pacote = pacotes.NewRow();
                    pacote["ID"] = bd.LerInt("PacoteID");
                    pacote["Nome"] = bd.LerString("Pacote");
                    pacote["Valor"] = bd.LerDecimal("Valor");
                    pacote["LocalID"] = bd.LerInt("LocalID");
                    pacote["Obs"] = bd.LerString("Obs");
                    pacote["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                    pacote["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                    pacote["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                    pacote["PermitirCancelamentoAvulso"] = false;// bd.LerBoolean("PermitirCancelamentoAvulso");

                    pacotes.Rows.Add(pacote);

                }
                bd.FecharConsulta();

                comm.CommandText = "EXECUTE bilheteria_Obrigatoriedade @CanalID = " + canalID;
                adapter.Fill(obrigatoriedade);

                comm.CommandText = "EXECUTE bilheteria_Series @CanalID = " + canalID;
                adapter.Fill(series);

                bd.Consulta().Close();

                connection.Close();
                bd.Fechar();

                if (locais.Rows.Count == 0 && series.Rows.Count == 0 && pacotes.Rows.Count == 0 && pacotesLugarMarcado.Rows.Count == 0)
                    throw new BilheteriaException("Não existem informações suficientes para venda. Verifique se existem eventos/preços distribuidos para o canal selecionado.");

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);

                buffer.Tables.Add(cortesias);
                buffer.Tables.Add(bloqueios);
                buffer.Tables.Add(pacotes);
                buffer.Tables.Add(pacotesitens);

                buffer.Tables.Add(regioes);
                buffer.Tables.Add(taxasEntrega);

                buffer.Tables.Add(mapas);
                buffer.Tables.Add(cotaItemFormaPagamento);
                buffer.Tables.Add(cotaItem);
                buffer.Tables.Add(codigoPromo);

                buffer.Tables.Add(formasPagamento);
                buffer.Tables.Add(formasPagamentoEvento);

                buffer.Tables.Add(pacotesLugarMarcado);

                buffer.Tables.Add(valeIngressoTipo);
                buffer.Tables.Add(taxaEntregaPadrao);
                buffer.Tables.Add(regiaoValeIngresso);

                buffer.Tables.Add(obrigatoriedade);
                buffer.Tables.Add(series);

                DateTime fim = DateTime.Now;
                TimeSpan tempo = ((TimeSpan)(fim - inicio));

                return buffer;

            }
            catch (BilheteriaException ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }

                if (comm != null)
                {
                    comm.Dispose();
                    comm = null;
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
            }
        }


        public static DataSet CarregarDoServico(int CanalID, bool DadosPeloCache)
        {
            try
            {
                byte[] Dados;

                // Recupera o Array de Bytes do Servico
                // Com Cache
                if (DadosPeloCache)
                    Dados = new IRLib.ServicoDados.ServicoDadosSoapClient().CarregarDadosVendaBilheteria(CanalID);
                // Sem Cache
                else
                    Dados = new IRLib.ServicoDados.ServicoDadosSoapClient().CarregarDadosVendaBilheteriaSemCache(CanalID);

                //Instancia um objeto do tipo XML 
                var guid = Guid.NewGuid();
                var xmlFile = guid + ".xml";

                //Transforma o bite[]  em uma String unica
                string r2 = ZIP.Unzip(Dados);

                //Escreve em um documento XML
                File.WriteAllText(xmlFile, r2);

                //Converte o arquivo XML em um DataSet
                DataSet dataset = new DataSet("Buffer");

                File.WriteAllText("schema.txt", IRLib.Arquivos.SchemaBuffer);

                dataset.ReadXmlSchema("schema.txt");

                dataset.ReadXml(new StringReader(r2));
                dataset.WriteXml(xmlFile);

                if (File.Exists(xmlFile))
                    File.Delete(xmlFile);

                //Retorna o DataSet
                return dataset;

            }
            catch (SoapException sexp)
            {
                throw new BilheteriaException(sexp.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// NOVO # Este metodo carrega todas as tabelas necessarias da bilheteria: locais, eventos, setores, apresentacoes, pacotes...
        /// </summary>        
        /// <returns></returns>
        public RetornoCarregarDados NovoCarregar(int canalID)
        {
            RetornoCarregarDados retorno = new RetornoCarregarDados();
            BD bd = new BD();
            DateTime inicio = DateTime.Now;
            string hoje = System.DateTime.Today.ToString("yyyyMMdd");
            string sql;

            bd.Fechar();
            SqlConnection connection = null;

            SqlCommand comm = null;
            SqlDataAdapter adapter = null;

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable formasPagamento = new DataTable("FormaPagamento");
                formasPagamento.Columns.Add("ID", typeof(int));
                formasPagamento.Columns.Add("Nome", typeof(string));
                formasPagamento.Columns.Add("Tipo", typeof(int));
                formasPagamento.Columns.Add("Parcelas", typeof(int));
                formasPagamento.Columns.Add("TEFID", typeof(int));
                formasPagamento.Columns.Add("Dias", typeof(int));
                formasPagamento.Columns.Add("TaxaAdm", typeof(decimal));
                formasPagamento.Columns.Add("IR", typeof(string));
                formasPagamento.Columns.Add("Itau", typeof(bool));
                formasPagamento.Columns.Add("BandeiraNome", typeof(string));
                formasPagamento.Columns.Add("BandeiraID", typeof(int));

                DataTable formasPagamentoEvento = new DataTable("FormaPagamentoEvento");
                formasPagamentoEvento.Columns.Add("FormaPagamentoID", typeof(int));
                formasPagamentoEvento.Columns.Add("EventoID", typeof(int));

                DataTable series = new DataTable("Serie");
                series.Columns.Add("ID", typeof(int));
                series.Columns.Add("Nome", typeof(string));
                series.Columns.Add("QuantidadeMinimaGrupo", typeof(int));
                series.Columns.Add("QuantidadeMaximaGrupo", typeof(int));
                series.Columns.Add("QuantidadeMinimaApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMaximaApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMinimaIngressosPorApresentacao", typeof(int));
                series.Columns.Add("QuantidadeMaximaIngressosPorApresentacao", typeof(int));
                series.Columns.Add("Descricao", typeof(string));
                series.Columns.Add("Regras", typeof(string));

                DataTable mapas = new DataTable("Mapa");
                mapas.Columns.Add("ApresentacaoSetorID", typeof(int));
                mapas.Columns.Add("Mapa", typeof(string));

                DataTable regioes = new DataTable("Regiao");
                regioes.Columns.Add("ID", typeof(int));
                regioes.Columns.Add("Nome", typeof(string));
                regioes.Columns.Add("EventoID", typeof(int));
                regioes.Columns.Add("TaxaEntregaID", typeof(int));

                DataTable taxasEntrega = new DataTable("TaxaEntrega");
                taxasEntrega.Columns.Add("ID", typeof(int));
                taxasEntrega.Columns.Add("Nome", typeof(string));
                taxasEntrega.Columns.Add("Valor", typeof(decimal));
                taxasEntrega.Columns.Add("RegiaoID", typeof(int));
                taxasEntrega.Columns.Add("EventoID", typeof(int));
                taxasEntrega.Columns.Add("Prazo", typeof(int));
                taxasEntrega.Columns.Add("Estado", typeof(string));

                //mesma estrutura da taxa entrega
                DataTable taxaEntregaPadrao = new DataTable("TaxaEntregaPadrao");
                taxaEntregaPadrao.Columns.Add("ID", typeof(int));
                taxaEntregaPadrao.Columns.Add("Nome", typeof(string));
                taxaEntregaPadrao.Columns.Add("Valor", typeof(decimal));
                taxaEntregaPadrao.Columns.Add("RegiaoID", typeof(int));
                taxaEntregaPadrao.Columns.Add("EventoID", typeof(int));
                taxaEntregaPadrao.Columns.Add("Prazo", typeof(int));
                taxaEntregaPadrao.Columns.Add("Estado", typeof(string));

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID", typeof(int));
                valeIngressoTipo.Columns.Add("Nome", typeof(string));
                valeIngressoTipo.Columns.Add("Valor", typeof(decimal));
                valeIngressoTipo.Columns.Add("ValidadeDiasImpressao", typeof(int));
                valeIngressoTipo.Columns.Add("ValidadeData", typeof(string));
                valeIngressoTipo.Columns.Add("ClienteTipo", typeof(string));
                valeIngressoTipo.Columns.Add("ProcedimentoTroca", typeof(string));
                valeIngressoTipo.Columns.Add("SaudacaoNominal", typeof(string));
                valeIngressoTipo.Columns.Add("SaudacaoPadrao", typeof(string));
                valeIngressoTipo.Columns.Add("QuantidadeLimitada", typeof(string));
                valeIngressoTipo.Columns.Add("CodigoTrocaFixo", typeof(string));
                valeIngressoTipo.Columns.Add("Acumulativo", typeof(string));
                valeIngressoTipo.Columns.Add("Quantidade", typeof(string)).DefaultValue = " - ";
                valeIngressoTipo.Columns.Add("ValorPagamento", typeof(decimal));
                valeIngressoTipo.Columns.Add("ValorTipo", typeof(char));
                valeIngressoTipo.Columns.Add("TrocaConveniencia", typeof(bool));
                valeIngressoTipo.Columns.Add("TrocaEntrega", typeof(bool));
                valeIngressoTipo.Columns.Add("TrocaIngresso", typeof(bool));

                DataTable regiaoValeIngresso = new DataTable("RegiaoValeIngresso");
                regiaoValeIngresso.Columns.Add("ID", typeof(int));
                regiaoValeIngresso.Columns.Add("Nome", typeof(string));

                DataTable cotaItem = new DataTable("CotaItem");
                cotaItem.Columns.Add("ID", typeof(int));
                cotaItem.Columns.Add("CotaID", typeof(int));
                cotaItem.Columns.Add("PrecoIniciaCom", typeof(string));
                cotaItem.Columns.Add("ParceiroID", typeof(int));
                cotaItem.Columns.Add("ValidaBin", typeof(bool));
                cotaItem.Columns.Add("TextoValidacao", typeof(string));
                cotaItem.Columns.Add("ObrigatoriedadeID", typeof(int));
                cotaItem.Columns.Add("Quantidade", typeof(int));
                cotaItem.Columns.Add("QuantidadePorCliente", typeof(int));
                cotaItem.Columns.Add("CPFResponsavel", typeof(bool));
                cotaItem.Columns.Add("Termo", typeof(string));
                cotaItem.Columns.Add("Nominal", typeof(bool));
                cotaItem.Columns.Add("QuantidadePorCodigo", typeof(int));

                DataTable cotaItemFormaPagamento = new DataTable("CotaItemFormaPagamento");
                cotaItemFormaPagamento.Columns.Add("CotaItemID", typeof(int));
                cotaItemFormaPagamento.Columns.Add("FormaPagamentoID", typeof(int));

                connection = (SqlConnection)bd.Cnn;
                comm = connection.CreateCommand();
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                string versao = ConfigurationManager.AppSettings["ConfigVersion"];
                var carregartodos = ConfiguracaoParalela.GetBoolean(ConfiguracaoParalela.Keys.CarregarTodos, versao);

                DataTable locais = new DataTable("Local");
                DataTable eventos = new DataTable("Evento");
                DataTable apresentacoes = new DataTable("Apresentacao");
                DataTable setores = new DataTable("Setor");
                DataTable precos = new DataTable("Preco");
                DataTable cortesias = new DataTable("Cortesia");
                DataTable bloqueios = new DataTable("Bloqueio");
                DataTable obrigatoriedade = new DataTable("Obrigatoriedade");

                DataTable[] tabelas = new DataTable[8] { locais, eventos, apresentacoes, setores, precos, cortesias, bloqueios, obrigatoriedade };

                buffer.Tables.AddRange(tabelas);

                using (var reader = bd.Consulta(string.Format("PR_LISTAR_VENDA_BILHETERIA_NOVA {0}, {1}", canalID, carregartodos)))
                {
                    buffer.Load(reader, LoadOption.PreserveChanges, tabelas);
                }

                locais.PrimaryKey = new DataColumn[] { locais.Columns["ID"] };

                DataTable PacotesLugarMarcado = new DataTable("PacoteLugarMarcado");
                DataTable PacotesItem = new DataTable("PacoteItem");
                DataTable Pacote = new DataTable("Pacote");
                DataTable Local = new DataTable("Local");

                DataTable[] tabelas2 = new DataTable[4] { PacotesLugarMarcado, PacotesItem, Pacote, Local };
                DataSet bufferTemp = new DataSet();

                bufferTemp.Tables.AddRange(tabelas2);

                using (var reader = bd.Consulta(string.Format("PR_LISTAR_VENDA_PACOTE_NOVA {0}, {1}", canalID, carregartodos)))
                {
                    bufferTemp.Load(reader, LoadOption.PreserveChanges, tabelas2);
                }

                buffer.Tables.Add(PacotesLugarMarcado.Copy());
                buffer.Tables.Add(Pacote.Copy());
                buffer.Tables.Add(PacotesItem.Copy());

                Local.PrimaryKey = new DataColumn[] { Local.Columns["ID"] };

                locais.Merge(Local);

                // Carregar informações de formas de pagamento do canal
                sql = "EXECUTE bilheteria_FormaPagamentoCanal @EmpresaID = 0 , @CanalID = " + canalID;
                comm.CommandText = sql;
                adapter.Fill(formasPagamento);

                if (formasPagamento.Rows.Count == 0)
                {
                    retorno.CarregadoComSucesso = false;
                    retorno.Mensagem = "Verifique se as formas de pagamento foram liberadas para seu canal.\n";
                    retorno.Dados = null;
                    return retorno;
                }
                else
                {
                    DataRow branco = formasPagamento.NewRow();
                    branco["ID"] = 0;
                    branco["Nome"] = string.Empty;
                    formasPagamento.Rows.InsertAt(branco, 0);
                }

                // Regioes utilizadas para o vir
                sql = "EXECUTE RegioesVir ";
                comm.CommandText = sql;
                adapter.Fill(regiaoValeIngresso);

                // Taxas de entrega padrao para usar no vale ingresso
                sql = "EXECUTE TaxasEntregaPadrao";

                comm.CommandText = sql;
                adapter.Fill(taxaEntregaPadrao);

                //Carrega informações dos Vale Ingressos
                sql = "EXECUTE bilheteria_ValeIngressos2 @CanalID = " + canalID + ", @Data = '" + hoje + "'";
                comm.CommandText = sql;
                adapter.Fill(valeIngressoTipo);

                comm.CommandText = "EXECUTE bilheteria_Series @CanalID = " + canalID;
                adapter.Fill(series);

                bd.Consulta().Close();

                connection.Close();
                bd.Fechar();

                if (locais.Rows.Count == 0 && series.Rows.Count == 0 && Pacote.Rows.Count == 0 && PacotesLugarMarcado.Rows.Count == 0)
                {
                    retorno.CarregadoComSucesso = false;
                    retorno.Mensagem = "Não existem informações suficientes para venda. Verifique se existem eventos/preços distribuidos para o canal selecionado.";
                    retorno.Dados = null;
                    return retorno;
                }

                buffer.Tables.Add(regioes);
                buffer.Tables.Add(taxasEntrega);

                buffer.Tables.Add(mapas);
                buffer.Tables.Add(cotaItemFormaPagamento);
                buffer.Tables.Add(cotaItem);

                buffer.Tables.Add(formasPagamento);
                buffer.Tables.Add(formasPagamentoEvento);

                buffer.Tables.Add(valeIngressoTipo);
                buffer.Tables.Add(taxaEntregaPadrao);
                buffer.Tables.Add(regiaoValeIngresso);

                buffer.Tables.Add(series);

                DateTime fim = DateTime.Now;
                TimeSpan tempo = ((TimeSpan)(fim - inicio));

                retorno.CarregadoComSucesso = true;
                retorno.Dados = buffer;

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }

                if (comm != null)
                {
                    comm.Dispose();
                    comm = null;
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
            }
        }

        private DataTable EstruturaDttEvento()
        {
            DataTable eventos = new DataTable("Evento");
            eventos.Columns.Add("ID", typeof(int));
            eventos.Columns.Add("Nome", typeof(string));
            eventos.Columns.Add("LocalID", typeof(int));
            eventos.Columns.Add("Atencao", typeof(string));
            eventos.Columns.Add("ObrigaCadastroCliente", typeof(bool));
            eventos.Columns.Add("Estado", typeof(string));
            //não carregar colunas abaixo
            eventos.Columns.Add("PDVSemConveniencia", typeof(string));
            eventos.Columns.Add("Censura", typeof(string));
            eventos.Columns.Add("EntradaAcompanhada", typeof(bool));
            eventos.Columns.Add("RetiradaIngresso", typeof(string));
            eventos.Columns.Add("MeiaEntrada", typeof(string));
            eventos.Columns.Add("Promocoes", typeof(string));
            eventos.Columns.Add("AberturaPortoes", typeof(string));
            eventos.Columns.Add("DuracaoEvento", typeof(string));
            eventos.Columns.Add("Release", typeof(string));
            eventos.Columns.Add("DescricaoPadrao", typeof(string));
            eventos.Columns.Add("TaxaConveniencia", typeof(int));
            eventos.Columns.Add("TaxaMinima", typeof(decimal));
            eventos.Columns.Add("TaxaMaxima", typeof(decimal));
            eventos.Columns.Add("ObrigatoriedadeID", typeof(int));
            //não carregar colunas abaixo
            eventos.Columns.Add("Carregou", typeof(bool));
            eventos.Columns.Add("TipoImpressao", typeof(string));
            return eventos;
        }

        private DataTable EstruturaDttApresentacao()
        {
            DataTable apresentacoes = new DataTable("Apresentacao");
            apresentacoes.Columns.Add("ID", typeof(int));
            apresentacoes.Columns.Add("Horario", typeof(string));
            apresentacoes.Columns.Add("EventoID", typeof(int));
            apresentacoes.Columns.Add("Descricao", typeof(string));
            apresentacoes.Columns.Add("DescricaoPadraoApresentacao", typeof(string));
            apresentacoes.Columns.Add("DescricaoPadrao", typeof(bool));
            apresentacoes.Columns.Add("QuantidadeApresentacao", typeof(int));
            apresentacoes.Columns.Add("QuantidadePorClienteApresentacao", typeof(int));
            apresentacoes.Columns.Add("ApresentacaoCotaID", typeof(int));
            return apresentacoes;
        }

        private DataTable EstruturaDttSetor()
        {
            DataTable setores = new DataTable("Setor");
            setores.Columns.Add("ID", typeof(int));
            setores.Columns.Add("Nome", typeof(string));
            setores.Columns.Add("Acesso", typeof(string));
            setores.Columns.Add("LocalID", typeof(int));
            setores.Columns.Add("Produto", typeof(bool));
            setores.Columns.Add("LugarMarcado", typeof(string));
            setores.Columns.Add("ApresentacaoID", typeof(int));
            setores.Columns.Add("QtdeDisponivel", typeof(int));
            setores.Columns.Add("QtdeBloqueado", typeof(int));
            setores.Columns.Add("ObservacaoImportante", typeof(string));
            setores.Columns.Add("DistanciaPalco", typeof(int));
            setores.Columns.Add("ApresentacaoSetorCotaID", typeof(int));
            setores.Columns.Add("QuantidadeApresentacaoSetor", typeof(int));
            setores.Columns.Add("QuantidadePorClienteApresentacaoSetor", typeof(int));
            return setores;

        }

        private DataTable EstruturaDttPreco()
        {
            DataTable precos = new DataTable("Preco");
            precos.Columns.Add("ID", typeof(int));
            precos.Columns.Add("Nome", typeof(string));
            precos.Columns.Add("CorID", typeof(int));
            precos.Columns.Add("Valor", typeof(decimal));
            precos.Columns.Add("ApresentacaoSetorID", typeof(int));
            precos.Columns.Add("ApresentacaoID", typeof(int));
            precos.Columns.Add("SetorID", typeof(int));
            return precos;
        }

        public DataSet CarregarSerieItem(int SerieID, int CanalID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet();

                List<int> EventosID = new List<int>();
                DataTable eventos = this.EstruturaDttEvento();
                eventos.Columns.Add("SerieID", typeof(int));

                List<int> ApresentacoesID = new List<int>();
                DataTable apresentacoes = this.EstruturaDttApresentacao();
                apresentacoes.Columns.Add("SerieID", typeof(int));

                List<int> SetoresID = new List<int>();
                DataTable setores = this.EstruturaDttSetor();
                setores.Columns.Add("SerieID", typeof(int));

                List<int> PrecosId = new List<int>();
                DataTable precos = this.EstruturaDttPreco();
                precos.Columns.Add("SerieID", typeof(int));
                precos.Columns.Add("Promocional", typeof(bool));
                precos.Columns.Add("QuantidadePorPromocional", typeof(int));

                DataTable formasPagto = new DataTable("FormaPagamento");
                formasPagto.Columns.Add("SerieID", typeof(int));
                formasPagto.Columns.Add("FormaPagamentoID", typeof(int));

                bd.Consulta("EXEC bilheteria_SerieItem @SerieID = " + SerieID + ", @CanalID = " + CanalID);

                bool temItem = false;
                DataRow dtr = null;
                while (bd.Consulta().Read())
                {
                    temItem = true;

                    if (!EventosID.Contains(bd.LerInt("EventoID")))
                    {
                        EventosID.Add(bd.LerInt("EventoID"));
                        dtr = eventos.NewRow();
                        eventos.Rows.Add(dtr);


                        dtr["ID"] = bd.LerInt("EventoID");
                        dtr["Nome"] = bd.LerString("Evento");
                        dtr["LocalID"] = bd.LerInt("LocalID");
                        dtr["Estado"] = bd.LerString("LocalEstado");
                        dtr["Atencao"] = bd.LerString("EventoAtencao");
                        dtr["ObrigaCadastroCliente"] = bd.LerBoolean("ObrigaCadastroCliente");
                        dtr["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        dtr["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                        dtr["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                        dtr["ObrigatoriedadeID"] = bd.LerInt("ObrigatoriedadeID");
                        dtr["SerieID"] = SerieID;
                    }

                    if (!ApresentacoesID.Contains(bd.LerInt("ApresentacaoID")))
                    {
                        ApresentacoesID.Add(bd.LerInt("ApresentacaoID"));

                        dtr = apresentacoes.NewRow();
                        apresentacoes.Rows.Add(dtr);

                        dtr["ID"] = bd.LerInt("ApresentacaoID");
                        dtr["Horario"] = bd.LerStringFormatoSemanaDataHora("Apresentacao");
                        dtr["EventoID"] = bd.LerInt("EventoID");
                        dtr["Descricao"] = bd.LerString("ApresentacaoDescricao");
                        dtr["DescricaoPadrao"] = bd.LerBoolean("ApresentacaoDescricaoPadrao");
                        dtr["DescricaoPadraoApresentacao"] = bd.LerString("DescricaoPadraoApresentacao");
                        dtr["QuantidadeApresentacao"] = bd.LerInt("QuantidadeApresentacao");
                        dtr["QuantidadePorClienteApresentacao"] = bd.LerInt("QuantidadePorClienteApresentacao");
                        dtr["ApresentacaoCotaID"] = bd.LerInt("ApresentacaoCotaID");
                        dtr["SerieID"] = SerieID;
                    }

                    if (setores.Select("ID = " + bd.LerInt("SetorID") + " AND ApresentacaoID = " + bd.LerInt("ApresentacaoID")).Length == 0)
                    {
                        SetoresID.Add(bd.LerInt("SetorID"));
                        dtr = setores.NewRow();
                        setores.Rows.Add(dtr);

                        dtr["ID"] = bd.LerInt("SetorID");
                        dtr["Nome"] = bd.LerString("Setor");
                        dtr["Acesso"] = bd.LerString("Acesso");
                        dtr["DistanciaPalco"] = bd.LerInt("DistanciaPalco");
                        dtr["LocalID"] = bd.LerInt("LocalID");
                        dtr["Produto"] = bd.LerBoolean("Produto");
                        dtr["LugarMarcado"] = bd.LerString("LugarMarcado");
                        dtr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        dtr["ObservacaoImportante"] = bd.LerString("SetorObservacaoImportante");
                        dtr["ApresentacaoSetorCotaID"] = bd.LerInt("ApresentacaoSetorCotaID");
                        dtr["QuantidadeApresentacaoSetor"] = bd.LerInt("Quantidade");
                        dtr["QuantidadePorClienteApresentacaoSetor"] = bd.LerInt("QuantidadePorCliente");
                        dtr["SerieID"] = SerieID;
                    }

                    if (!PrecosId.Contains(bd.LerInt("PrecoID")))
                    {
                        PrecosId.Add(bd.LerInt("PrecoID"));
                        dtr = precos.NewRow();
                        precos.Rows.Add(dtr);

                        dtr["ID"] = bd.LerInt("PrecoID");
                        dtr["Nome"] = bd.LerString("Preco");
                        dtr["CorID"] = bd.LerInt("CorID");
                        dtr["Valor"] = bd.LerDecimal("Valor");
                        dtr["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                        dtr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        dtr["SetorID"] = bd.LerInt("SetorID");
                        dtr["SerieID"] = SerieID;
                        dtr["QuantidadePorPromocional"] = bd.LerInt("QuantidadePorPromocional");
                        dtr["Promocional"] = bd.LerBoolean("Promocional");
                    }
                }

                if (!temItem)
                    throw new Exception("Não existem eventos disponíveis para a série selecionada, por favor, tente outra série.");

                bd.FecharConsulta();
                bd.Consulta(@"SELECT 
								fps.FormaPagamentoID
							FROM tFormaPagamentoSerie fps (NOLOCK)
							INNER JOIN tCanalFormaPagamento cfp (NOLOCK) ON cfp.FormaPagamentoID = fps.FormaPagamentoID
								WHERE fps.SerieID = " + SerieID + " AND cfp.CanalID = " + CanalID);
                while (bd.Consulta().Read())
                {
                    dtr = formasPagto.NewRow();
                    dtr["SerieID"] = SerieID;
                    dtr["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
                    formasPagto.Rows.Add(dtr);
                }


                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);
                buffer.Tables.Add(formasPagto);
                return buffer;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Método responsável por alterar o status de um ou mais vale ingressos, além de alterar o seu respectivo código de barras e inserir o registro de auditoria.
        /// A rotina está preparada para registrar impressões ou reimpressões de tickets conforme parâmetro reimpressao.
        /// </summary>
        /// <param name="ingressosIDs">IDs de ingressos a serem "impressos"</param>
        /// <param name="usuarioID">ID do usuário responsável pela ação</param>
        /// <param name="empresaID">ID da empresa do usuário responsável pela ação</param>
        /// <param name="caixaID">ID do caixa do Usuário - caixa deve estar aberto</param>
        /// <param name="canalID">ID do canal em que o usuário tem o caixa aberto</param>
        /// <param name="lojaID">ID da loja em que o usuário tem o caixa aberto</param>
        /// <param name="reimpressao">Definição do tipo de ação (Para reimpressão = true)</param>
        /// <param name="motivoReimpressao">Motivo da reimpressão (vazio em caso de impressão de ticket)</param>
        /// <returns>Objeto Dictionary composto por ID do ingresso (int) e código de barras gerado (string)</returns>
        public List<EstruturaRetornoRegistroImpressao> RegistrarImpressaoValeIngresso(
            List<EstruturaRegistroImpressaoVir> valeIngressos, int vendaBilheteriaID, int usuarioID, int empresaID, int caixaID, int canalID,
            int lojaID, bool reimpressao, string motivoReimpressao, int SupervisorID, EstruturaLogistica estruturaLogistica)
        {
            #region Consistências
            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo!");

            if (lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula!");

            if (canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo!");
            #endregion

            BD bd = new BD();
            ValeIngressoLog.enumAcao acaoAuditoria;
            List<EstruturaRetornoRegistroImpressao> retorno = new List<EstruturaRetornoRegistroImpressao>();
            EstruturaRetornoRegistroImpressao retornoItem;
            ValeIngresso oValeIngresso = new ValeIngresso();
            ValeIngressoLog oValeIngressoLog;
            CodigoBarra oCodigoBarra = new CodigoBarra();
            DateTime agora = DateTime.Now;
            List<string> lstGerados = new List<string>();
            try
            {
                bd.IniciarTransacao();
                acaoAuditoria = reimpressao ? ValeIngressoLog.enumAcao.Reimprimir : ValeIngressoLog.enumAcao.Imprimir;
                string codigoBarraNovo = string.Empty;
                string codigoTrocaNovo = string.Empty;
                string dataExpiracao = string.Empty;
                foreach (EstruturaRegistroImpressaoVir valeIngresso in valeIngressos)
                {

                    //Gera o novo codigo de barra
                    codigoBarraNovo = oCodigoBarra.NovoCodigoBarraRandomico(18);

                    //Gera o novo Codigo de Troca
                    if (!valeIngresso.CodigoTrocaFixo)
                    {
                        codigoTrocaNovo = oValeIngresso.NovoCodigoTroca();
                        //while (lstGerados.Contains(codigoTrocaNovo))
                        //    codigoTrocaNovo = oValeIngresso.NovoCodigoTroca();

                        //lstGerados.Add(codigoTrocaNovo);
                    }

                    //Atualiza a tValeIngresso com o novo status, Codigo de Barras e Codigo de troca.
                    StringBuilder stbSQL = new StringBuilder();

                    if (reimpressao)
                    {

                        stbSQL.Append("UPDATE tValeIngresso SET Status = '" + ((char)ValeIngresso.enumStatus.Impresso) + "', ");
                        stbSQL.Append("CodigoBarra = '" + codigoBarraNovo + "' ");

                        if (!valeIngresso.CodigoTrocaFixo)
                            stbSQL.Append(", CodigoTroca = '" + codigoTrocaNovo + "' ");

                        stbSQL.Append("WHERE Status = '" + ((char)ValeIngresso.enumStatus.Impresso) + "' ");
                        stbSQL.Append("AND ID = " + valeIngresso.valeIngressoID);
                    }
                    else
                    {
                        stbSQL.Append("UPDATE tValeIngresso SET Status = '" + ((char)ValeIngresso.enumStatus.Impresso) + "', ");
                        stbSQL.Append("CodigoBarra = '" + codigoBarraNovo + "' ");

                        if (!valeIngresso.CodigoTrocaFixo)
                            stbSQL.Append(", CodigoTroca = '" + codigoTrocaNovo + "' ");

                        if (valeIngresso.DiasImpressao > 0)
                        {
                            dataExpiracao = System.DateTime.Now.AddDays(valeIngresso.DiasImpressao).ToString("yyyyMMdd");
                            stbSQL.Append(", DataExpiracao = '" + dataExpiracao + "' ");
                        }
                        stbSQL.Append("WHERE Status = '" + ((char)ValeIngresso.enumStatus.Vendido).ToString() + "' AND ID = " + valeIngresso.valeIngressoID);
                    }
                    bd.Executar(stbSQL.ToString());

                    oValeIngressoLog = new ValeIngressoLog();
                    oValeIngressoLog.Acao.Valor = ((char)acaoAuditoria).ToString();
                    oValeIngressoLog.TimeStamp.Valor = agora;
                    oValeIngressoLog.ValeIngressoID.Valor = valeIngresso.valeIngressoID;
                    oValeIngressoLog.UsuarioID.Valor = usuarioID;
                    oValeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteriaID;
                    oValeIngressoLog.EmpresaID.Valor = empresaID;
                    oValeIngressoLog.CaixaID.Valor = caixaID;
                    oValeIngressoLog.CanalID.Valor = canalID;
                    oValeIngressoLog.LojaID.Valor = lojaID;
                    oValeIngressoLog.SupervisorID.Valor = SupervisorID;

                    if (valeIngresso.CodigoTrocaFixo)
                        oValeIngressoLog.CodigoTroca.Valor = valeIngresso.CodigoTroca;
                    else
                        oValeIngressoLog.CodigoTroca.Valor = codigoTrocaNovo;

                    oValeIngressoLog.CodigoBarra.Valor = codigoBarraNovo;
                    oValeIngressoLog.ClienteNome.Valor = valeIngresso.ClientePresenteado;
                    oValeIngressoLog.Obs.Valor = motivoReimpressao;

                    if (!Convert.ToBoolean(bd.Executar(oValeIngressoLog.StringInserir())))
                        throw new BilheteriaException("Log de impressão do ingresso não foi inserido."); // Falha ao inserir o registro.

                    //Preenche o objeto de retorno
                    retornoItem = new EstruturaRetornoRegistroImpressao();
                    retornoItem.ID = valeIngresso.valeIngressoID;
                    retornoItem.CodigoBarra = codigoBarraNovo;
                    if (valeIngresso.CodigoTrocaFixo)
                        retornoItem.CodigoTrocaVir = valeIngresso.CodigoTroca;
                    else
                        retornoItem.CodigoTrocaVir = codigoTrocaNovo;

                    retorno.Add(retornoItem);
                }

                bd.FinalizarTransacao();

                return retorno;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Método responsável por alterar o status de um ou mais ingressos para impresso além de alterar o seu respectivo código de barras e inserir o registro de auditoria.
        /// A rotina está preparada para registrar impressões ou reimpressões de tickets conforme parâmetro reimpressao.
        /// </summary>
        /// <param name="ingressosIDs">IDs de ingressos a serem "impressos"</param>
        /// <param name="usuarioID">ID do usuário responsável pela ação</param>
        /// <param name="empresaID">ID da empresa do usuário responsável pela ação</param>
        /// <param name="caixaID">ID do caixa do Usuário - caixa deve estar aberto</param>
        /// <param name="canalID">ID do canal em que o usuário tem o caixa aberto</param>
        /// <param name="lojaID">ID da loja em que o usuário tem o caixa aberto</param>
        /// <param name="reimpressao">Definição do tipo de ação (Para reimpressão = true)</param>
        /// <param name="motivoReimpressao">Motivo da reimpressão (vazio em caso de impressão de ticket)</param>
        /// <returns>Objeto Dictionary composto por ID do ingresso (int) e código de barras gerado (string)</returns>
        private List<EstruturaRetornoRegistroImpressao> RegistrarImpressao(int[] ingressosIDs, int usuarioID,
            int empresaID, int caixaID, int canalID, int lojaID, bool reimpressao, string motivoReimpressao, bool impressaoEmLote,
            bool registrarVendaBilheteriaAntiFraude, int quantidadeImpressoesInternet, EstruturaLogistica estruturaLogistica, int SupervisorID, bool ImpressaoPos)
        {
            //TODO: Tentar remover a select de pesquisa no ingresso.

            // Detalhes:
            // Rotina responsável por alterar o status de um ingresso de Vendido para Impresso ou atualizar o código de barras no caso de reimpressão e inserir o log de auditoria.
            // Passos:
            //		1) Consistências de parâmetros. Todos são obrigatórios.
            //		2) Tratamento do tipo de ação a ser executada - Impressão / Reimpressão
            //		3) Abertura de transações.
            //		4) Criação do código de barras com base no precoID.
            //		5) Mudança do status (tIngresso).
            //		6) Auditoria na tIngressoLog.
            //      7) Mudança do status (tVendaBilheteria) case seja necessário --> Apenas telas de Impressao e ImpressaoEmLote
            //      8) Envia o Email para tracking de pedidos
            //		8) Retorno do dicionário combinado de IngressoID x Código de barras gerado.

            #region Consistências

            if (reimpressao == true && SupervisorID == 0)
                throw new BilheteriaException("Impressão precisa ser autorizada pelo Supervisor");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo!");

            if (lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula!");

            if (canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo!");
            #endregion

            BD bd = new BD();

            try
            {
                bd.IniciarTransacao(); // Start da transação no banco de dados.
                List<EstruturaRetornoRegistroImpressao> retorno = new List<EstruturaRetornoRegistroImpressao>(); // repositório de combinações ingressoID X código de barras e CodigoImpressao

                CodigoBarra oCodigoBarra = new CodigoBarra(); // Objeto com métodos para criação do código de barras.

                int ingressosAtualizados = 0;
                int vendaBilheteriaID = 0;

                // Vai do primeiro ao último ingressoID da lista atualizando status, gerando código de barras e inserindo registros de auditoria.
                foreach (int ingressoID in ingressosIDs)
                {
                    // Busca as informações do ingresso.
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   tVendaBilheteria.ID AS VendaBilheteriaID, " +
                        "   tIngresso.PrecoID, " +
                        "   tIngresso.ApresentacaoID, " +
                        "   tIngresso.BloqueioID, " +
                        "   tIngresso.CortesiaID, " +
                        "   tVendaBilheteria.ClienteID, " +
                        "   tIngresso.EventoID, " +
                        "   tIngresso.CodigoSequencial, " +
                        "   tIngresso.CodigoBarra, " +
                        "   tCanal.TipoVenda, " +
                        "   tIngresso.Status AS StatusIngresso, " +
                        "   tIngresso.ApresentacaoSetorID, " +
                        "   tEvento.TipoCodigoBarra " +
                        "FROM " +
                        "   tIngresso (NOLOCK) " +
                        "INNER JOIN " +
                        "   tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                        "INNER JOIN " +
                        "   tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID " +
                        "INNER JOIN " +
                        "   tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID " +
                        "INNER JOIN " +
                        "   tLoja (NOLOCK) On tLoja.ID = tCaixa.LojaID " +
                        "INNER JOIN " +
                        "   tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID " +
                        "WHERE " +
                        "   (tIngresso.ID = " + ingressoID + ") " +
                       "AND " +
                        "    ( " +
                        "        ( " +
                        "	         (tCanal.TipoVenda = '" + Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher).ToString() + "') " +
                        "        AND " +
                        "			 (tIngresso.Status = '" + ((!reimpressao) ? Ingresso.VENDIDO : Ingresso.IMPRESSO) + "' OR tIngresso.Status = '" + Ingresso.AGUARDANDO_TROCA + "' OR tIngresso.Status = '" + Ingresso.ENTREGUE + "'" + ") " +
                        "        ) " +
                        "        OR " +
                        "        ( " +
                        "			 (tCanal.TipoVenda <> '" + Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher).ToString() + "') " +
                        "		 AND  " +
                        "			 (" + ((!reimpressao) ? "tIngresso.Status = '" + Ingresso.VENDIDO + "' OR tIngresso.Status = '" + Ingresso.AGUARDANDO_TROCA + "'" : "tIngresso.Status = '" + Ingresso.IMPRESSO + "' OR tIngresso.Status = '" + Ingresso.ENTREGUE + "'") + ") " +
                        "        ) " +
                        "    ) "))
                    {
                        if (!oDataReader.Read())
                            oDataReader.Close();// Caso nada tenha sido encontrado, fecha a consulta e vai ao próximo item.
                        else
                        {
                            ingressosAtualizados++;

                            // Popula variáveis com informações da consulta.
                            int precoID = bd.LerInt("PrecoID");
                            int eventoID = bd.LerInt("EventoID");
                            int bloqueioID = bd.LerInt("BloqueioID");
                            int cortesiaID = bd.LerInt("CortesiaID");
                            int clienteID = bd.LerInt("ClienteID");
                            vendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                            string codigoBarraAntigo = bd.LerString("CodigoBarra");
                            string statusAtual = bd.LerString("StatusIngresso");
                            string tipoVenda = bd.LerString("TipoVenda");
                            int ApresentacaoID = bd.LerInt("ApresentacaoID");
                            int ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                            string codigoBarra = bd.LerString("CodigoBarra");
                            Enumerators.TipoCodigoBarra tipoCodigoBarra = string.IsNullOrEmpty(bd.LerString("TipoCodigoBarra")) ? Enumerators.TipoCodigoBarra.Estruturado : (Enumerators.TipoCodigoBarra)Convert.ToChar(bd.LerString("TipoCodigoBarra"));
                            oDataReader.Close(); // fecha a consulta.

                            // Define o Novo Status do Ingresso e a Acao do Log
                            string acaoAuditoria = ((reimpressao) ? IngressoLog.REIMPRIMIR : IngressoLog.IMPRIMIR);
                            string statusNovo = statusAtual;

                            if (tipoVenda == Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher).ToString())
                            {
                                if (impressaoEmLote)
                                    statusNovo = Ingresso.IMPRESSO;
                                else
                                {

                                    switch (statusAtual)
                                    {
                                        case Ingresso.VENDIDO:
                                            statusNovo = Ingresso.AGUARDANDO_TROCA;
                                            acaoAuditoria = ((reimpressao) ? IngressoLog.VOUCHER_REIMPRESSAO : IngressoLog.VOUCHER_IMPRESSAO);
                                            break;
                                        case Ingresso.AGUARDANDO_TROCA:
                                            statusNovo = ((reimpressao) ? Ingresso.AGUARDANDO_TROCA : Ingresso.IMPRESSO);
                                            acaoAuditoria = ((reimpressao) ? IngressoLog.VOUCHER_REIMPRESSAO : IngressoLog.IMPRIMIR);
                                            break;
                                    }

                                }
                            }
                            else
                            {
                                statusNovo = Ingresso.IMPRESSO;
                                acaoAuditoria = ((reimpressao) ? IngressoLog.REIMPRIMIR : IngressoLog.IMPRIMIR);
                            }

                            bool AtualizarCodigoBarra = tipoCodigoBarra == Enumerators.TipoCodigoBarra.Estruturado || reimpressao;

                            int codigoImpressao = 0;

                            if (!(((tipoVenda == Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher).ToString()) && (statusAtual == Ingresso.VENDIDO) || (statusAtual == Ingresso.AGUARDANDO_TROCA && reimpressao))) || impressaoEmLote)
                            {
                                //Incrementa o Codigo de impressão e atualiza na tIngresso:
                                codigoImpressao = Convert.ToInt32(bd.ConsultaValor("EXEC IncrementarUltimoCodigoImpresso @ApresentacaoID  = " + ApresentacaoID));

                                if (reimpressao)
                                {
                                    //Update na tIngressoCodigoBarra mudando a coluna BlackList para 'T'
                                    // Isso é nescessário para invalidar codigos de barra ao reimprimir ou cancelar um ingresso.
                                    bd.Executar("UPDATE tIngressoCodigoBarra SET BlackList = 'T', Sincronizado = 'F', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' WHERE CodigoBarra = '" + codigoBarra + "' AND EventoID = " + eventoID);
                                    bd.Consulta().Close(); // fecha a consulta.
                                }

                                // Código de barras - um novo a cada impressão ou reimpressão.
                                if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.Estruturado && !ImpressaoPos)
                                    codigoBarra = oCodigoBarra.NovoCodigoBarraIngresso(precoID, eventoID, codigoImpressao); // Cria um novo código de barras.
                                else if ((tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca && reimpressao) || ImpressaoPos)
                                    codigoBarra = oCodigoBarra.NovoCodigoBarraListaBranca(bd, ApresentacaoSetorID);

                                //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                                if (codigoBarra.Trim().Length > 0)
                                {
                                    IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                                    ingressoCodigoBarra.EventoID.Valor = eventoID;
                                    ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                                    bd.Executar(ingressoCodigoBarra.StringInserir());
                                }

                                retorno.Add(new EstruturaRetornoRegistroImpressao()
                                {
                                    ID = ingressoID,
                                    CodigoBarra = codigoBarra,
                                    CodigoImpressao = codigoImpressao
                                }); // adiciona a combinação à lista.
                            }

                            //Atualiza o ingresso no banco de dados e armazena o número de linhas afetadas na variável linhasAlteradas.
                            // Status do ingresso sempre será impresso independente do tipo de ação.
                            int linhasAlteradas = bd.Executar("UPDATE tIngresso " +
                                "SET " +
                                "  Status = '" + statusNovo + "' " +
                                ((tipoVenda != Convert.ToChar(Canal.TipoDeVenda.ImpressaoVoucher).ToString()) ? " , " +
                                "  CodigoImpressao = " + codigoImpressao + ", " +
                                "  CodigoBarra = '" + codigoBarra + "' " : "") +
                                "WHERE " +
                                "  (Status = '" + statusAtual + "') " +
                                "AND " +
                                "  (ID = " + ingressoID + ")");

                            if (linhasAlteradas != 1) // Erro caso o retorno seja diferente de 1.
                                throw new BilheteriaException("Não foi possível alterar o status do ingresso..");
                            else
                            {
                                // Insert na tIngressoLog.
                                IngressoLog ingressoLog = new IngressoLog();
                                ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaID;
                                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                ingressoLog.IngressoID.Valor = ingressoID;
                                ingressoLog.BloqueioID.Valor = bloqueioID;
                                ingressoLog.CortesiaID.Valor = cortesiaID;
                                ingressoLog.PrecoID.Valor = precoID;
                                ingressoLog.ClienteID.Valor = clienteID;
                                ingressoLog.UsuarioID.Valor = usuarioID;
                                ingressoLog.EmpresaID.Valor = empresaID;
                                ingressoLog.Acao.Valor = acaoAuditoria;
                                ingressoLog.CaixaID.Valor = caixaID;
                                ingressoLog.CanalID.Valor = canalID;
                                ingressoLog.LojaID.Valor = lojaID;
                                ingressoLog.CodigoBarra.Valor = codigoBarra; // Código de barras gerado acima.
                                ingressoLog.Obs.Valor = motivoReimpressao;
                                ingressoLog.CodigoImpressao.Valor = codigoImpressao;
                                ingressoLog.SupervisorID.Valor = SupervisorID;

                                // Busca o insert SQL, executa no banco e converte o retorno (linhas inseridas - 0 ou 1) para bool.
                                if (!Convert.ToBoolean(bd.Executar(ingressoLog.StringInserir())))
                                    throw new BilheteriaException("Log de impressão do ingresso não foi inserido."); // Falha ao inserir o registro.

                            }// Fim if linhasAlteradas
                        }//Fim if consulta.read()
                    }
                }//Fim foreach IngressoIDs

                //Atribui o Impressao Automatica (Executado quando o periodo de triagem expirou)
                if (registrarVendaBilheteriaAntiFraude)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET Status = '{0}', AprovacaoAutomatica = '{1}' WHERE tVendaBilheteria.ID = {2}", (char)VendaBilheteria.StatusAntiFraude.Aprovado, "T", vendaBilheteriaID));

                if (quantidadeImpressoesInternet > 0)
                    bd.Executar(string.Format("UPDATE tVendaBilheteria SET QuantidadeImpressoesInternet = {0} WHERE tVendaBilheteria.ID = {1}", quantidadeImpressoesInternet, vendaBilheteriaID));

                /*
                if (estruturaLogistica != null && estruturaLogistica.TaxaEntregaTipo != Enumeradores.TaxaEntregaTipo.Nenhum)
                {
                    switch (estruturaLogistica.TaxaEntregaTipo)
                    {
                        case Enumeradores.TaxaEntregaTipo.Entrega:
                            ServicoEmail.EnviarEmailAcompanhamentoEntrega(estruturaLogistica.Email, estruturaLogistica.Cliente, estruturaLogistica.VendaBilheteriaID.ToString(), estruturaLogistica.Senha, estruturaLogistica.TaxaEntrega);
                            break;
                        case Enumeradores.TaxaEntregaTipo.Retirada:
                            ServicoEmail.EnviarEmailAcompanhamentoRetirada(estruturaLogistica.Email, estruturaLogistica.Cliente, estruturaLogistica.VendaBilheteriaID.ToString(), estruturaLogistica.Senha, estruturaLogistica.TaxaEntrega);
                            break;
                        case Enumeradores.TaxaEntregaTipo.Nenhum:
                        default:
                            break;
                    }
                }*/
                if (ingressosAtualizados == ingressosIDs.Length)
                    bd.FinalizarTransacao();
                else
                    bd.DesfazerTransacao();

                bd.Fechar();

                return retorno;
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private bool AtualizaCodigoBarraPos(int ingressosID, int usuarioID, int empresaID, int caixaID, int canalID, int lojaID, string motivoReimpressao, int SupervisorID, string codigoBarra, int codigoImpressao)
        {
            #region Consistências

            if (SupervisorID == 0)
                throw new BilheteriaException("Impressão precisa ser autorizada pelo Supervisor");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            if (caixaID == 0)
                throw new BilheteriaException("Caixa não pode ser nulo!");

            if (lojaID == 0)
                throw new BilheteriaException("Loja não pode ser nula!");

            if (canalID == 0)
                throw new BilheteriaException("Canal não pode ser nulo!");
            #endregion

            BD bd = new BD();

            try
            {
                bd.IniciarTransacao();

                CodigoBarra oCodigoBarra = new CodigoBarra();

                int precoID, eventoID = 0;
                int bloqueioID = 0;
                int cortesiaID = 0;
                int clienteID = 0;
                int vendaBilheteriaID = 0;
                int ApresentacaoID = 0;
                int ApresentacaoSetorID = 0;
                int linhasAlteradas;
                string StatusIngresso, statusNovo, acaoAuditoria;
                int ingressosAtualizados = 0;

                bd.Consulta(@"SELECT tVendaBilheteria.ID AS VendaBilheteriaID, tIngresso.PrecoID, tIngresso.ApresentacaoID, 
				tIngresso.BloqueioID, tIngresso.CortesiaID, tVendaBilheteria.ClienteID, tIngresso.EventoID, 
				tIngresso.CodigoSequencial, tIngresso.CodigoBarra, tIngresso.Status AS StatusIngresso, 
				tIngresso.ApresentacaoSetorID, tEvento.TipoCodigoBarra 
				FROM tIngresso (NOLOCK) 
				INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID 
				INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
				INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID 
				INNER JOIN tLoja (NOLOCK) On tLoja.ID = tCaixa.LojaID
				INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
				WHERE tIngresso.ID = " + ingressosID + @" AND tIngresso.Status = '" + Ingresso.VENDIDO + "'");

                if (!bd.Consulta().Read())
                    bd.Consulta().Close();
                else
                {
                    ingressosAtualizados++;

                    // Popula variáveis com informações da consulta.
                    precoID = bd.LerInt("PrecoID");
                    eventoID = bd.LerInt("EventoID");
                    bloqueioID = bd.LerInt("BloqueioID");
                    cortesiaID = bd.LerInt("CortesiaID");
                    clienteID = bd.LerInt("ClienteID");
                    vendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                    StatusIngresso = bd.LerString("StatusIngresso");
                    ApresentacaoID = bd.LerInt("ApresentacaoID");
                    ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");

                    bd.Consulta().Close(); // fecha a consulta.

                    acaoAuditoria = IngressoLog.IMPRIMIR;
                    statusNovo = Ingresso.IMPRESSO;

                    linhasAlteradas = bd.Executar(@"UPDATE tIngresso SET Status = '" + statusNovo + "', CodigoImpressao = " + codigoImpressao + ", CodigoBarra = '" + codigoBarra + "' WHERE ID = " + ingressosID + " AND Status = '" + StatusIngresso + "'");

                    if (linhasAlteradas != 1)
                        throw new BilheteriaException("Não foi possível alterar o status do ingresso..");
                    else
                    {
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteriaID;
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = ingressosID;
                        ingressoLog.BloqueioID.Valor = bloqueioID;
                        ingressoLog.CortesiaID.Valor = cortesiaID;
                        ingressoLog.PrecoID.Valor = precoID;
                        ingressoLog.ClienteID.Valor = clienteID;
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.Acao.Valor = acaoAuditoria;
                        ingressoLog.CaixaID.Valor = caixaID;
                        ingressoLog.CanalID.Valor = canalID;
                        ingressoLog.LojaID.Valor = lojaID;
                        ingressoLog.CodigoBarra.Valor = codigoBarra;
                        ingressoLog.Obs.Valor = motivoReimpressao;
                        ingressoLog.CodigoImpressao.Valor = codigoImpressao;
                        ingressoLog.SupervisorID.Valor = SupervisorID;

                        if (!Convert.ToBoolean(bd.Executar(ingressoLog.StringInserir())))
                            throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                    }
                }

                if (ingressosAtualizados > 0)
                {
                    bd.FinalizarTransacao();
                    return true;
                }
                else
                {
                    bd.DesfazerTransacao();
                    return false;
                }
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

        public List<EstruturaRetornoRegistroImpressao> BuscaCodigoBarrasPos(int[] ingressosIDs)
        {
            BD bd = new BD();

            try
            {
                string codigoBarra = string.Empty;

                int eventoID = 0;
                int ApresentacaoID = 0;
                int ApresentacaoSetorID = 0;
                int codigoImpressao = 0;
                int ingressosAtualizados = 0;

                CodigoBarra oCodigoBarra = new CodigoBarra();

                List<EstruturaRetornoRegistroImpressao> retorno = new List<EstruturaRetornoRegistroImpressao>();
                IngressoCodigoBarra ingressoCodigoBarra;

                EstruturaRetornoRegistroImpressao registro;

                foreach (int ingressoID in ingressosIDs)
                {
                    bd.Consulta(@"SELECT tIngresso.ApresentacaoID, tIngresso.EventoID, tIngresso.ApresentacaoSetorID
					FROM tIngresso (NOLOCK)
					INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
					WHERE tIngresso.ID = " + ingressoID + @" AND tIngresso.Status = '" + Ingresso.RESERVADO + "'");

                    if (!bd.Consulta().Read())
                        bd.Consulta().Close();
                    else
                    {
                        ingressosAtualizados++;

                        eventoID = bd.LerInt("EventoID");
                        ApresentacaoID = bd.LerInt("ApresentacaoID");
                        ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");

                        bd.Consulta().Close();

                        codigoImpressao = Convert.ToInt32(bd.ConsultaValor("EXEC IncrementarUltimoCodigoImpresso @ApresentacaoID  = " + ApresentacaoID));

                        codigoBarra = oCodigoBarra.NovoCodigoBarraListaBranca(bd, ApresentacaoSetorID);

                        if (codigoBarra.Trim().Length > 0)
                        {
                            ingressoCodigoBarra = new IngressoCodigoBarra();
                            ingressoCodigoBarra.EventoID.Valor = eventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                            bd.Executar(ingressoCodigoBarra.StringInserir());
                        }
                        else
                            throw new Exception("Não será possível gerar o código de barras.\nNão existem mais registros na lista branca.\nPor favor, entre em contato com a equipe de atendimento.");

                        registro = new EstruturaRetornoRegistroImpressao();
                        registro.ID = ingressoID;
                        registro.CodigoBarra = codigoBarra;
                        registro.CodigoImpressao = codigoImpressao;
                        retorno.Add(registro);
                    }
                }

                return retorno;
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

        /// <summary>
        /// Método responsável por alterar o status de um ou mais ingressos para impresso além de alterar o seu respectivo código de barras e inserir o registro de auditoria.
        /// </summary>
        /// <param name="ingressosIDs">IDs de ingressos a serem "impressos"</param>
        /// <param name="usuarioID">ID do usuário responsável pela ação</param>
        /// <param name="empresaID">ID da empresa do usuário responsável pela ação</param>
        /// <param name="caixaID">ID do caixa do Usuário - caixa deve estar aberto</param>
        /// <param name="canalID">ID do canal em que o usuário tem o caixa aberto</param>
        /// <param name="lojaID">ID da loja em que o usuário tem o caixa aberto</param>
        /// <returns>Objeto Dictionary composto por ID do ingresso (int) e código de barras gerado (string)</returns>
        public List<EstruturaRetornoRegistroImpressao> RegistrarImpressaoEmLote(int[] ingressosIDs, int usuarioID, int empresaID, int caixaID,
            int canalID, int lojaID, bool registrarVendaBilheteriaAntiFraude, EstruturaLogistica estruturaLogistica, int SupervisorID, bool ImpressaoPos)
        {
            return this.RegistrarImpressao(ingressosIDs, usuarioID, empresaID, caixaID, canalID, lojaID, false, string.Empty, true, registrarVendaBilheteriaAntiFraude, 0, estruturaLogistica, SupervisorID, ImpressaoPos);
        }

        /// <summary>
        /// Método responsável por alterar o status de um ou mais ingressos para impresso além de alterar o seu respectivo código de barras e inserir o registro de auditoria.
        /// </summary>
        /// <param name="ingressosIDs">IDs de ingressos a serem "impressos"</param>
        /// <param name="usuarioID">ID do usuário responsável pela ação</param>
        /// <param name="empresaID">ID da empresa do usuário responsável pela ação</param>
        /// <param name="caixaID">ID do caixa do Usuário - caixa deve estar aberto</param>
        /// <param name="canalID">ID do canal em que o usuário tem o caixa aberto</param>
        /// <param name="lojaID">ID da loja em que o usuário tem o caixa aberto</param>
        /// <returns>Objeto Dictionary composto por ID do ingresso (int) e código de barras gerado (string)</returns>
        public List<EstruturaRetornoRegistroImpressao> RegistrarImpressao(int[] ingressosIDs, int usuarioID, int empresaID, int caixaID, int canalID, int lojaID, bool registrarVendaBilheteriaAntiFraude, int quantidadeImpressoesInternet, EstruturaLogistica estruturaLogistica, int SupervisorID, bool ImpressaoPos)
        {
            return this.RegistrarImpressao(ingressosIDs, usuarioID, empresaID, caixaID, canalID, lojaID, false, string.Empty, false, registrarVendaBilheteriaAntiFraude, quantidadeImpressoesInternet, estruturaLogistica, SupervisorID, ImpressaoPos);
        }

        /// <summary>
        /// Método responsável por registrar a reimpressão de um ou mais ingressos além de alterar o seu respectivo código de barras.
        /// </summary>
        /// <param name="ingressosIDs">IDs de ingressos a serem "reimpressos"</param>
        /// <param name="usuarioID">ID do usuário responsável pela ação</param>
        /// <param name="empresaID">ID da empresa do usuário responsável pela ação</param>
        /// <param name="caixaID">ID do caixa do Usuário - caixa deve estar aberto</param>
        /// <param name="canalID">ID do canal em que o usuário tem o caixa aberto</param>
        /// <param name="lojaID">ID da loja em que o usuário tem o caixa aberto</param>
        /// <param name="motivo">Motivo da Reimpressão</param>
        /// <returns>Objeto Dictionary composto por ID do ingresso (int) e código de barras gerado (string)</returns>
        public List<EstruturaRetornoRegistroImpressao> RegistrarReimpressao(int[] ingressosIDs, int usuarioID, int empresaID, int caixaID, int canalID, int lojaID, string motivo, int quantidadeImpressoesInternet, int SupervisorID, bool ImpressaoPos)
        {
            return this.RegistrarImpressao(ingressosIDs, usuarioID, empresaID, caixaID, canalID, lojaID, true, motivo, false, false, quantidadeImpressoesInternet, null, SupervisorID, ImpressaoPos);
        }

        public static DataTable EstruturaSimples()
        {
            DataTable tabela = new DataTable("Ingresso");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("BloqueioID", typeof(int));
            tabela.Columns.Add("CortesiaID", typeof(int));
            tabela.Columns.Add("ClienteID", typeof(int));
            tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
            tabela.Columns.Add("VendaBilheteriaID", typeof(int));

            return tabela;
        }

        class DadosVendaEntrega
        {
            public int VendaBilheteriaIDVenda = 0;
            public VendaBilheteria VendaBilheteriaEntrega = null;
        }

        /// <summary>
        /// Muda o status dos ingressos vendidos para impressos. Retorna o sucesso da operacao.
        /// </summary>
        /// <returns></returns>
        public void Entregar(int[] ingressosIDs, int usuarioID, int empresaID, string obs)
        {
            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            BD bd = new BD();

            try
            {


                bd.IniciarTransacao();

                foreach (int ingressoID in ingressosIDs)
                {


                    string sql = "SELECT tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, " +
                        "tVendaBilheteria.ClienteID " +
                        "FROM tIngresso " +
                        "INNER JOIN tVendaBilheteria ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID " +
                        "WHERE tIngresso.ID=" + ingressoID + " AND tIngresso.Status='" + Ingresso.IMPRESSO + "'";
                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {

                        int precoID = bd.LerInt("PrecoID");
                        int bloqueioID = bd.LerInt("BloqueioID");
                        int cortesiaID = bd.LerInt("CortesiaID");
                        int clienteID = bd.LerInt("ClienteID");

                        bd.Consulta().Close();

                        string sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.ENTREGUE + "' " +
                            "WHERE Status='" + Ingresso.IMPRESSO + "' AND ID=" + ingressoID;
                        int x = bd.Executar(sqlUpdate);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log


                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.BloqueioID.Valor = bloqueioID;
                            ingressoLog.CortesiaID.Valor = cortesiaID;
                            ingressoLog.PrecoID.Valor = precoID;
                            ingressoLog.ClienteID.Valor = clienteID;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.Obs.Valor = obs.ToLower();
                            ingressoLog.Acao.Valor = IngressoLog.ENTREGAR;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            bool okE = Convert.ToBoolean(bd.Executar(sqlIngressoLogV));
                            if (!okE)
                                throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                        }
                        else
                        {


                            throw new BilheteriaException("Status do ingresso não foi atualizado.");
                        }
                    }
                    else
                    {


                        bd.Consulta().Close();
                    }
                }

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

        /// <summary>
        /// Muda o status dos ingressos reservados para pre-impressos. Retorna o sucesso da operacao.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> EmitirPreImpresso(int[] ingressosIDs, int lojaID, int canalID, int empresaID, int usuarioID)
        {
            if (lojaID == 0)
                throw new BilheteriaException("Loja nao pode ser nula.");

            if (canalID == 0)
                throw new BilheteriaException("Canal nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            BD bd = new BD();
            string codigoBarra = string.Empty;
            CodigoBarra oCodigoBarra = new CodigoBarra();
            try
            {
                bd.IniciarTransacao();
                Dictionary<int, int> retorno = new Dictionary<int, int>();
                foreach (int ingressoID in ingressosIDs)
                {
                    string sql = "SELECT EventoID, PrecoID, BloqueioID, CortesiaID, CodigoSequencial, CodigoBarra, tEvento.TipoCodigoBarra FROM tIngresso (NOLOCK) " +
                            " INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                        "WHERE tIngresso.ID = " + ingressoID;

                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        int precoID = bd.LerInt("PrecoID");
                        int bloqueioID = bd.LerInt("BloqueioID");
                        int cortesiaID = bd.LerInt("CortesiaID");
                        int eventoID = bd.LerInt("EventoID");
                        int codigoSequencial = bd.LerInt("CodigoSequencial");
                        codigoBarra = bd.LerString("CodigoBarra");
                        Enumerators.TipoCodigoBarra tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(bd.LerString("TipoCodigoBarra"));

                        bd.Consulta().Close();

                        if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.Estruturado)
                            codigoBarra = oCodigoBarra.NovoCodigoBarraIngresso(precoID, eventoID, codigoSequencial);

                        //Se gerou o código de barras deve inserir na tIngressoCodigoBarra
                        if (codigoBarra.Trim().Length > 0)
                        {
                            IngressoCodigoBarra ingressoCodigoBarra = new IngressoCodigoBarra();
                            ingressoCodigoBarra.EventoID.Valor = eventoID;
                            ingressoCodigoBarra.CodigoBarra.Valor = codigoBarra;

                            bd.Executar(ingressoCodigoBarra.StringInserir());
                        }

                        sql = string.Format("EXEC dbo.IncrementarUltimoCodigoImpressoPreImpresso '{0}' , {1} , {2}, {3}", codigoBarra, lojaID, ingressoID, usuarioID);

                        int codigoImpressao = Convert.ToInt32(bd.ConsultaValor(sql));

                        if (codigoImpressao == -1)
                            throw new Exception("Não foi possível emitir o pré-impresso, erro ao incrementar o útimo código de impressão");

                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.IngressoID.Valor = ingressoID;
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.BloqueioID.Valor = bloqueioID;
                        ingressoLog.CortesiaID.Valor = cortesiaID;
                        ingressoLog.PrecoID.Valor = precoID;
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.Acao.Valor = IngressoLog.EMISSAO_PREIMPRESSO;
                        ingressoLog.LojaID.Valor = lojaID;
                        ingressoLog.CanalID.Valor = canalID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.CodigoBarra.Valor = codigoBarra;

                        string sqlIngressoLog = ingressoLog.StringInserir();
                        bool okV = Convert.ToBoolean(bd.Executar(sqlIngressoLog));

                        if (!okV)
                            throw new BilheteriaException("Log de pré-impressão do ingresso " + ingressoID + " não foi inserido.");

                        retorno.Add(ingressoID, codigoImpressao);
                    }
                    else
                    {
                        bd.Consulta().Close();
                        throw new BilheteriaException("Pré-impressão do ingresso " + ingressoID + " não foi concluído.");
                    }

                }

                bd.FinalizarTransacao();
                return retorno;
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>		
        /// Bloquear ingressos.
        /// </summary>
        /// <returns></returns>
        public void Bloquear(int bloqueioID, int apresentacaoSetorID, int qtde, int usuarioID, int empresaID, string obs)
        {


            if (bloqueioID == 0)
                throw new BilheteriaException("Bloqueio nao pode ser nulo.");

            if (apresentacaoSetorID == 0)
                throw new BilheteriaException("Apresentacao/Setor nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            BD bd = new BD();

            try
            {


                int[] ingressosIDs = new int[qtde];

                string sql = "SELECT top " + qtde + " ID " +
                    "FROM tIngresso " +
                    "WHERE ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.DISPONIVEL + "'";
                bd.Consulta(sql);

                int i = 0;
                while (bd.Consulta().Read())
                {

                    ingressosIDs[i++] = bd.LerInt("ID");
                }
                bd.Consulta().Close();


                bd.IniciarTransacao();

                foreach (int ingressoID in ingressosIDs)
                {


                    string sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.BLOQUEADO + "', " +
                        "BloqueioID=" + bloqueioID + " " +
                        "WHERE AssinaturaClienteID = 0 AND Status='" + Ingresso.DISPONIVEL + "' AND ID=" + ingressoID;
                    int x = bd.Executar(sqlUpdate);
                    bool ok = (x == 1);
                    if (ok)
                    {
                        //inserir na Log


                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = ingressoID;
                        ingressoLog.BloqueioID.Valor = bloqueioID;
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.Obs.Valor = obs.ToLower();
                        ingressoLog.Acao.Valor = IngressoLog.BLOQUEAR;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        bool okB = Convert.ToBoolean(bd.Executar(sqlIngressoLogV));
                        if (!okB)
                            throw new BilheteriaException("Log de bloqueio do ingresso não foi inserido.");
                    }
                    else
                    {


                        throw new BilheteriaException("Status do ingresso não foi atualizado.");
                    }

                }

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

        /// <summary>		
        /// Desbloquear ingressos.
        /// </summary>
        /// <returns></returns>
        public void Desbloquear(int bloqueioID, int apresentacaoSetorID, int qtde, int usuarioID, int empresaID, string obs)
        {

            if (bloqueioID < 0)
                throw new BilheteriaException("Bloqueio nao pode ser nulo.");

            if (apresentacaoSetorID == 0)
                throw new BilheteriaException("Apresentacao/Setor nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            BD bd = new BD();

            try
            {

                int[] ingressosIDs = new int[qtde];

                string sql = "SELECT top " + qtde + " ID " +
                    "FROM tIngresso " +
                    "WHERE ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.BLOQUEADO + "'";

                if (bloqueioID != 0)
                    sql += " AND BloqueioID=" + bloqueioID;

                bd.Consulta(sql);

                int i = 0;
                while (bd.Consulta().Read())
                    ingressosIDs[i++] = bd.LerInt("ID");

                bd.Consulta().Close();

                //iniciar processo----------
                bd.IniciarTransacao();

                foreach (int ingressoID in ingressosIDs)
                {

                    string sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.DISPONIVEL + "', " +
                        "BloqueioID=0 " +
                        "WHERE AssinaturaClienteID = 0 AND Status='" + Ingresso.BLOQUEADO + "' AND ID=" + ingressoID;
                    int x = bd.Executar(sqlUpdate);
                    bool ok = (x == 1);
                    if (ok)
                    {
                        //inserir na Log

                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = ingressoID;
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.UsuarioID.Valor = usuarioID;
                        ingressoLog.EmpresaID.Valor = empresaID;
                        ingressoLog.Obs.Valor = obs.ToLower();
                        ingressoLog.Acao.Valor = IngressoLog.DESBLOQUEAR;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        bool okB = Convert.ToBoolean(bd.Executar(sqlIngressoLogV));
                        if (!okB)
                            throw new BilheteriaException("Log de bloqueio do ingresso não foi inserido.");

                    }
                    else
                    {
                        throw new BilheteriaException("Status do ingresso não foi atualizado.");
                    }

                }

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

        /// <summary>		
        /// Bloquear ingressos de mapa.
        /// </summary>
        /// <returns></returns>
        public int Bloquear(int[] bloqueioIDs, int apresentacaoSetorID, int[] lugarIDs, int[] qtdes, int usuarioID, int empresaID)
        {

            if (apresentacaoSetorID == 0)
                throw new BilheteriaException("Apresentacao/Setor nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            int qtdeNaoBloqueada = 0;

            BD bd = new BD();

            for (int i = 0; i < lugarIDs.Length; i++)
            {

                try
                {

                    int lugarID = lugarIDs[i];
                    int bloqueioID = bloqueioIDs[i];
                    int qtde = qtdes[i];

                    //desbloquear tudo e depois bloquear.
                    //assim evita de estragar a quantidade e o bloqueioID.

                    bd.IniciarTransacao();

                    string sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.DISPONIVEL + "', " +
                        "BloqueioID=0 " +
                        "WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.BLOQUEADO + "'";

                    bd.Executar(sqlUpdate);

                    ArrayList listaIngressosIDs = new ArrayList();

                    string sql = "SELECT top " + qtde + " ID " +
                        "FROM tIngresso " +
                        "WHERE LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.DISPONIVEL + "'";
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        listaIngressosIDs.Add(bd.LerInt("ID"));
                    }
                    bd.Consulta().Close();

                    listaIngressosIDs.TrimToSize();

                    if (listaIngressosIDs.Count == 0)
                    {
                        throw new BilheteriaException("Ingressos não-disponíveis..");
                    }

                    int[] ingressosIDs = (int[])listaIngressosIDs.ToArray(typeof(int));

                    foreach (int ingressoID in ingressosIDs)
                    {

                        sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.BLOQUEADO + "', " +
                            "BloqueioID=" + bloqueioID + " " +
                            "WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status='" + Ingresso.DISPONIVEL + "' AND ID=" + ingressoID;
                        int x = bd.Executar(sqlUpdate);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log

                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.BloqueioID.Valor = bloqueioID;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.Obs.Valor = string.Empty; //obs.ToLower();
                            ingressoLog.Acao.Valor = IngressoLog.BLOQUEAR;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            bool okB = Convert.ToBoolean(bd.Executar(sqlIngressoLogV));
                            if (!okB)
                                throw new BilheteriaException("Log de bloqueio do ingresso não foi inserido.");
                            //}else {
                            //	throw new BilheteriaException("Status do ingresso não foi atualizado.");

                        }//fim do if (ok)

                    }//fim do for

                    bd.FinalizarTransacao();

                }
                catch
                {
                    bd.DesfazerTransacao();
                    qtdeNaoBloqueada++;
                }

            }//fim do for

            bd.Fechar();

            return qtdeNaoBloqueada;

        }

        /// <summary>		
        /// Desbloquear ingressos de mapa.
        /// </summary>
        /// <returns></returns>
        public int Desbloquear(int apresentacaoSetorID, int[] lugarIDs, int usuarioID, int empresaID)
        {

            if (apresentacaoSetorID == 0)
                throw new BilheteriaException("Apresentacao/Setor nao pode ser nulo.");

            if (usuarioID == 0)
                throw new BilheteriaException("Usuario nao pode ser nulo.");

            if (empresaID == 0)
                throw new BilheteriaException("Empresa nao pode ser nula.");

            int qtdeNaoDesbloqueada = 0;

            BD bd = new BD();

            for (int i = 0; i < lugarIDs.Length; i++)
            {

                try
                {

                    int lugarID = lugarIDs[i];

                    ArrayList listaIngressosIDs = new ArrayList();

                    string sql = "SELECT ID " +
                        "FROM tIngresso " +
                        "WHERE LugarID=" + lugarID + " AND ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.BLOQUEADO + "'";
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        listaIngressosIDs.Add(bd.LerInt("ID"));
                    }
                    bd.Consulta().Close();

                    bd.IniciarTransacao();

                    listaIngressosIDs.TrimToSize();

                    int[] ingressosIDs = (int[])listaIngressosIDs.ToArray(typeof(int));

                    foreach (int ingressoID in ingressosIDs)
                    {

                        string sqlUpdate = "UPDATE tIngresso SET Status='" + Ingresso.DISPONIVEL + "', " +
                            "BloqueioID=0 " +
                            "WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND Status='" + Ingresso.BLOQUEADO + "' AND ID=" + ingressoID;
                        int x = bd.Executar(sqlUpdate);
                        bool ok = (x == 1);
                        if (ok)
                        {
                            //inserir na Log

                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.BloqueioID.Valor = 0;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.Obs.Valor = string.Empty; //obs.ToLower();
                            ingressoLog.Acao.Valor = IngressoLog.DESBLOQUEAR;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            bool okB = Convert.ToBoolean(bd.Executar(sqlIngressoLogV));
                            if (!okB)
                                throw new BilheteriaException("Log de bloqueio do ingresso não foi inserido.");
                            //}else {
                            //	throw new BilheteriaException("Status do ingresso não foi atualizado.");
                        }

                    }

                    bd.FinalizarTransacao();

                }
                catch (Exception)
                {
                    bd.DesfazerTransacao();
                    qtdeNaoDesbloqueada++;
                }

            }//fim do for

            bd.Fechar();

            return qtdeNaoDesbloqueada;

        }

        public DataSet MontaDadosVendaImportacaoAssinatura(int[] ingressosID, int clienteID, int usuarioID, int canalID)
        {

            string sql = string.Empty;
            StringBuilder pacotesID = new StringBuilder();

            DataSet buffer = this.estruturaReservasInternet(1);

            DataTable itensReservados = buffer.Tables[TABELA_GRID]; // Resumo
            DataTable ingressosVendidos = buffer.Tables[TABELA_RESERVA]; // Detalhamento


            // // Informações gerais
            DataTable aux = new DataTable("Aux");

            aux.Columns.Add("IngressoID", typeof(int));
            aux.Columns.Add("Codigo", typeof(string));
            aux.Columns.Add("EventoID", typeof(int));
            aux.Columns.Add("ApresentacaoID", typeof(int));
            aux.Columns.Add("ApresentacaoSetorID", typeof(int));
            aux.Columns.Add("LugarID", typeof(int));
            aux.Columns.Add("SetorID", typeof(int));
            aux.Columns.Add("PrecoID", typeof(int));
            aux.Columns.Add("PacoteID", typeof(int));
            aux.Columns.Add("PacoteGrupo", typeof(string));
            aux.Columns.Add("LugarMarcado", typeof(string));
            aux.Columns.Add("TaxaConveniencia", typeof(decimal));
            aux.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
            aux.Columns.Add("TaxaComissao", typeof(int));
            aux.Columns.Add("ComissaoValor", typeof(decimal));
            aux.Columns.Add("ComissaoMinima", typeof(decimal));
            aux.Columns.Add("ComissaoMaxima", typeof(decimal));
            aux.Columns.Add("Valor", typeof(decimal));



            sql = @"SELECT 
					tIngresso.ID as IngressoID,Codigo,tIngresso.EventoID, ApresentacaoID,tIngresso.ApresentacaoSetorID, SetorID, PrecoID, PacoteID, LugarMarcado, LugarID,tPreco.Valor,TaxaConveniencia, PacoteGrupo,
					(tPreco.Valor * TaxaConveniencia) / 100 AS TaxaConvenienciaValor, tCanalEvento.TaxaMinima, tCanalEvento.TaxaMaxima,tCanalEvento.TaxaComissao,tCanalEvento.ComissaoMinima,tCanalEvento.ComissaoMaxima,(tPreco.Valor * TaxaComissao) / 100 AS ComissaoValor
					FROM tIngresso(NOLOCK)
					INNER JOIN tSetor ON tSetor.ID = tIngresso.SetorID
					INNER JOIN tPreco ON tPreco.ID = tIngresso.PrecoID
					INNER JOIN tCanalEvento ON tCanalEvento.EventoID = tIngresso.EventoID
					WHERE ClienteID = " + clienteID + " AND UsuarioID = " + usuarioID + " AND Status = 'R' AND tCanalEvento.CanalID = " + canalID + " ORDER BY IngressoID";

            BD bd = new BD();

            bd.Consulta(sql);


            buffer.Tables.Add(aux);

            DataRow dr = null;

            while (bd.Consulta().Read())
            {
                dr = aux.NewRow();
                aux.Rows.Add(dr);
                dr["IngressoID"] = bd.LerInt("IngressoID");
                dr["EventoID"] = bd.LerInt("EventoID");
                dr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                dr["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                dr["SetorID"] = bd.LerInt("SetorID");
                dr["PrecoID"] = bd.LerInt("PrecoID");
                dr["PacoteID"] = bd.LerInt("PacoteID");
                dr["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                dr["LugarID"] = bd.LerInt("LugarID");
                dr["Codigo"] = bd.LerString("Codigo");
                dr["LugarMarcado"] = bd.LerString("LugarMarcado");
                decimal taxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                decimal taxaComissao = bd.LerDecimal("TaxaComissao");
                dr["TaxaConveniencia"] = taxaConveniencia;
                dr["TaxaComissao"] = taxaComissao;

                decimal taxaMin = bd.LerDecimal("TaxaMinima");
                decimal taxaMax = bd.LerDecimal("TaxaMaxima");

                decimal dAux = (taxaConveniencia / 100m) * bd.LerDecimal("Valor");
                if (dAux < taxaMin)
                {
                    dr["TaxaConvenienciaValor"] = Decimal.Round(taxaMin, 2);
                }
                else if (dAux > taxaMax && taxaMax > 0)
                {
                    dr["TaxaConvenienciaValor"] = Decimal.Round(taxaMax, 2);
                }
                else
                {
                    dr["TaxaConvenienciaValor"] = bd.LerDecimal("TaxaConvenienciaValor");
                }


                decimal comissaoMin = bd.LerDecimal("ComissaoMinima");
                decimal comissaoMax = bd.LerDecimal("ComissaoMaxima");
                dr["ComissaoMinima"] = comissaoMin;
                dr["ComissaoMaxima"] = comissaoMax;

                decimal dAuxComissao = (taxaComissao / 100m) * bd.LerDecimal("Valor");
                if (dAuxComissao < comissaoMin)
                {
                    dr["ComissaoValor"] = Decimal.Round(comissaoMin, 2);
                }
                else if (dAuxComissao > comissaoMax && comissaoMax > 0)
                {
                    dr["ComissaoValor"] = Decimal.Round(comissaoMax, 2);
                }
                else
                {
                    dr["ComissaoValor"] = bd.LerDecimal("ComissaoValor");
                }

                dr["Valor"] = bd.LerDecimal("Valor");

                if (bd.LerInt("PacoteID") > 0)
                    pacotesID.Append(bd.LerInt("PacoteID").ToString() + ",");
            }

            bd.FecharConsulta();

            // verifica se a quantidade reservada e a quantidade de ingressos passados
            if (aux.Rows.Count != ingressosID.Length)
                throw new BilheteriaException("Quantidade de itens diferente da quantidade reservada no sistema");

            for (int i = 0; i <= aux.Rows.Count - 1; i++)
            {
                if (ingressosID[i].ToString() != aux.Rows[i]["IngressoID"].ToString())
                    throw new BilheteriaException("Quantidade de itens diferente da quantidade reservada no sistema");
            }

            // Buscar as informações de pacotes.
            //if (pacotesID.Length > 0) {
            //    PacoteItemLista oPacoteItemLista = new PacoteItemLista();
            //    oPacoteItemLista.FiltroSQL = "PacoteID IN ("+ pacotesID.ToString() +"-1)";
            //    oPacoteItemLista.Carregar();

            //    infoPacote = oPacoteItemLista.Tabela();
            //    buffer.Tables.Add(infoPacote);	
            //    temPacote = true;
            //}

            DataRow vendido = null;

            int reservaID = 1;
            DataRow linha = null;

            #region Manipula pacotes
            //DataRow linha = null;
            //while (temPacote && aux.Rows.Count > 0) {
            //    foreach(DataRow pacoteItem in infoPacote.Rows) {
            //        vendido = ingressosVendidos.NewRow();

            //        DataRow[] ingressos = aux.Select("PacoteID = "+ pacoteItem["PacoteID"] +" AND EventoID = "+ pacoteItem["EventoID"] +" AND ApresentacaoID = "+ pacoteItem["ApresentacaoID"] +" AND SetorID = "+ pacoteItem["SetorID"] +" AND PrecoID = "+ pacoteItem["PrecoID"]);


            //        if (ingressos.Length > 0) {
            //            DataRow[] resumo = itensReservados.Select(RESERVAID + "=" + reservaID);

            //            if (resumo.Length == 0) {
            //                linha = itensReservados.NewRow();
            //                linha[INGRESSOID] = ingressos[0]["IngressoID"];
            //                linha[RESERVAID] = reservaID;
            //                linha[CODIGO] = ingressos[0]["Codigo"];

            //                linha[CONV] = ingressos[0]["TaxaConveniencia"];
            //                linha["TaxaConvenienciaValor"] = 0; // Será atualizada conforme ingressos. 
            //                linha[TIPO] = TIPO_PACOTE;
            //                linha[TIPO_LUGAR] = ingressos[0]["LugarMarcado"];
            //                if(Convert.ToInt32(ingressos[0]["LugarID"]) > 0)
            //                    linha[LUGAR_MARCADO] = true;
            //                else
            //                    linha[LUGAR_MARCADO] = false;
            //                linha[VALOR] = 0; // Será atualizada conforme ingressos.
            //                linha[APRESENTACAOSETORID] = ingressos[0]["ApresentacaoSetorID"];
            //                linha[PRECOID] = ingressos[0]["PrecoID"];
            //                itensReservados.Rows.Add(linha);						
            //            }

            //            vendido[TIPO] = TIPO_PACOTE;
            //            vendido[RESERVAID] = reservaID;
            //            vendido[LUGARID] = ingressos[0]["LugarID"];
            //            vendido[PACOTEID] = pacoteItem["PacoteID"];
            //            vendido[INGRESSOID] = ingressos[0]["IngressoID"];
            //            vendido[PRECOID] = ingressos[0]["PrecoID"];

            //            //Somatoria para total de conveniencia e total do valor dos ingressos
            //            linha["TaxaConvenienciaValor"] = (decimal)linha["TaxaConvenienciaValor"] + (decimal)ingressos[0]["TaxaConvenienciaValor"];
            //            linha[VALOR] = (decimal)linha[VALOR] + (decimal)ingressos[0]["Valor"];

            //            ingressosVendidos.Rows.Add(vendido);

            //            aux.Rows.Remove(ingressos[0]); // Remove a linha do datatable.
            //        }
            //        else
            //            temPacote = false;
            //    }
            //    reservaID++;
            //}
            #endregion


            //somente a manipulação de pacotes deve ser usada para assinatura
            #region Manipula Pacotes II
            DataTable pacotesReservados = CTLib.TabelaMemoria.DistinctComFiltro(aux, "PacoteGrupo", "PacoteGrupo <> '' AND PacoteGrupo <> '0' AND PacoteID > 0");
            CanalPacote canalPacote = new CanalPacote();
            foreach (DataRow pacote in pacotesReservados.Rows)
            {
                reservaID++;


                DataRow[] ingressos = aux.Select("PacoteGrupo = '" + pacote["PacoteGrupo"] + "'");

                linha = itensReservados.NewRow();

                linha[INGRESSOID] = ingressos[0]["IngressoID"];
                linha[RESERVAID] = reservaID;
                linha[CODIGO] = ingressos[0]["Codigo"];
                linha["TaxaConvenienciaValor"] = 0; // Será atualizada conforme ingressos.
                linha[TAXA_COMISSAO] = ingressos[0]["TaxaComissao"];
                linha[COMISSAO_VALOR] = 0; // Será atualizada conforme ingressos.
                linha[TIPO] = TIPO_PACOTE;
                linha[TIPO_LUGAR] = Setor.Pista;
                linha[LUGAR_MARCADO] = false;
                linha[VALOR] = 0; // Será atualizada conforme ingressos.
                linha[APRESENTACAOSETORID] = ingressos[0]["ApresentacaoSetorID"];
                linha[PRECOID] = ingressos[0]["PrecoID"];

                DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(Canal.CANAL_INTERNET, (int)ingressos[0]["PacoteID"]);

                decimal taxaMin = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMinima"]);
                decimal taxaMax = Convert.ToDecimal(taxasPacote.Rows[0]["TaxaMaxima"]);
                int taxaConveniencia = (int)taxasPacote.Rows[0]["TaxaConveniencia"];
                decimal comissaoMin = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMinima"]);
                decimal comissaoMax = Convert.ToDecimal(taxasPacote.Rows[0]["ComissaoMaxima"]);
                int taxaComissao = (int)taxasPacote.Rows[0]["TaxaComissao"];

                linha[CONV] = taxaConveniencia;

                itensReservados.Rows.Add(linha);

                foreach (DataRow ingresso in ingressos)
                {

                    vendido = ingressosVendidos.NewRow();
                    vendido[TIPO] = TIPO_INGRESSO;
                    vendido[RESERVAID] = reservaID;
                    vendido[LUGARID] = ingresso["LugarID"];
                    vendido[PACOTEID] = ingresso["PacoteID"];
                    vendido[INGRESSOID] = ingresso["IngressoID"];
                    vendido[PRECOID] = ingresso["PrecoID"];

                    //Somatoria para total de conveniencia e total do valor dos ingressos de acordo com taxa min e max
                    decimal dAuxTaxa = (taxaConveniencia / 100m) * (decimal)ingresso[VALOR];

                    if (linha["TaxaConvenienciaValor"] == DBNull.Value)
                        linha["TaxaConvenienciaValor"] = 0;

                    if (dAuxTaxa < taxaMin)
                    {
                        linha["TaxaConvenienciaValor"] = Convert.ToDecimal(linha["TaxaConvenienciaValor"]) + Decimal.Round(taxaMin, 2);
                    }
                    else if (dAuxTaxa > taxaMax && taxaMax > 0)
                    {
                        linha["TaxaConvenienciaValor"] = Convert.ToDecimal(linha["TaxaConvenienciaValor"]) + Decimal.Round(taxaMax, 2);
                    }
                    else
                    {
                        linha["TaxaConvenienciaValor"] = Convert.ToDecimal(linha["TaxaConvenienciaValor"]) + dAuxTaxa;
                    }

                    linha[VALOR] = (decimal)linha[VALOR] + (decimal)ingresso["Valor"];



                    decimal dAux = (taxaComissao / 100m) * (decimal)ingresso[VALOR];
                    if (linha["ComissaoValor"] == DBNull.Value)
                        linha["ComissaoValor"] = 0;

                    if (dAux < comissaoMin)
                    {
                        linha["ComissaoValor"] = Convert.ToDecimal(linha["ComissaoValor"]) + Decimal.Round(comissaoMin, 2);
                    }
                    else if (dAux > comissaoMax && comissaoMax > 0)
                    {
                        linha["ComissaoValor"] = Convert.ToDecimal(linha["ComissaoValor"]) + Decimal.Round(comissaoMax, 2);
                    }
                    else
                    {
                        linha["ComissaoValor"] = Convert.ToDecimal(linha["ComissaoValor"]) + dAux;
                    }


                    ingressosVendidos.Rows.Add(vendido);

                    aux.Rows.Remove(ingresso); // Remove a linha do datatable.
                }



            }
            #endregion

            //Region comentado.
            #region Manipula o restante
            //DataRow reservado = null;
            //foreach (DataRow resto in aux.Rows)
            //{
            //    reservaID++;
            //    reservado = itensReservados.NewRow();
            //    vendido = ingressosVendidos.NewRow();

            //    reservado[INGRESSOID] = resto["IngressoID"];
            //    reservado[RESERVAID] = reservaID;
            //    reservado[CODIGO] = resto["Codigo"];
            //    reservado[VALOR] = resto["Valor"];
            //    reservado[CONV] = resto["TaxaConveniencia"];
            //    reservado["TaxaConvenienciaValor"] = resto["TaxaConvenienciaValor"];
            //    reservado[TAXA_COMISSAO] = resto["TaxaComissao"];
            //    reservado[COMISSAO_VALOR] = resto["ComissaoValor"];
            //    reservado[TIPO_LUGAR] = resto["LugarMarcado"];
            //    if (Convert.ToInt32(resto["LugarID"]) > 0)
            //        reservado[LUGAR_MARCADO] = true;
            //    else
            //        reservado[LUGAR_MARCADO] = false;

            //    reservado[APRESENTACAOSETORID] = resto["ApresentacaoSetorID"];
            //    reservado[PRECOID] = resto["PrecoID"];
            //    reservado[TIPO] = TIPO_INGRESSO;

            //    itensReservados.Rows.Add(reservado);

            //    vendido[RESERVAID] = reservaID;
            //    vendido[TIPO] = TIPO_INGRESSO;
            //    vendido[LUGARID] = resto["LugarID"];
            //    vendido[PACOTEID] = resto["PacoteID"];
            //    vendido[INGRESSOID] = resto["IngressoID"];
            //    vendido[PRECOID] = resto["PrecoID"];
            //    ingressosVendidos.Rows.Add(vendido);
            //}
            #endregion

            return buffer;


        }

        /// <summary>
        /// Retorna os setores que ainda possuem qtde disponivel + qtde disponivel + primeiro preço cadastrado com ingresso disponivel
        /// </summary>
        /// <param name="tabelaSetores">Informe os setoresIDs para retonar as informações de qtde e preço</param>
        /// <param name="lstApresentacoes">Lista de apresentações a serem atualizadas</param>
        /// <returns></returns>
        public DataTable AtualizarSetoresQtdePreco(DataTable tabelaSetores, List<string> lstApresentacoes)
        {
            //* atribui qtde e preco aos setores
            //* permanecer o setor na tabela mesmo se ele nao estiver disponivel

            if (tabelaSetores == null)
                return null;

            BD bd = new BD();

            try
            {
                //Cria uma tabela temporária para guardar os setores que deverão ter suas quantidades atualizadas
                string sql = @"CREATE TABLE #Qtd
								(
									ApresentacaoSetorID INT NULL,
									SetorID INT NOT NULL,
									ApresentacaoID INT NOT NULL,
								)

								CREATE TABLE #QtdFinal
								(
									ApresentacaoSetorID INT NULL,
									ID INT NOT NULL,
									ApresentacaoID INT NOT NULL,
									QtdeBloqueado INT NULL,
									QtdeDisponivel INT NULL,
								)";
                bd.Executar(sql);

                StringBuilder InsertsBD = new StringBuilder();
                DataTable temp = CTLib.TabelaMemoria.Distinct(tabelaSetores);
                DataRow[] linhasSetoresApresentacao = temp.Select("ApresentacaoID IN (" + Utilitario.ArrayToString(lstApresentacoes.ToArray()) + ")");
                //Faz um foreach que cria as strings de inserção para o banco
                foreach (DataRow setor in linhasSetoresApresentacao)
                {
                    InsertsBD.Append("INSERT INTO #Qtd VALUES (NULL, " + (int)setor["ID"] + "," + (int)setor["ApresentacaoID"] + ")" + Environment.NewLine);
                }

                bd.Executar(InsertsBD);

                //Esse update traz as quantidades para a tabela temporária
                sql = @"UPDATE q SET q.ApresentacaoSetorID = aa.ID
						FROM #Qtd q
						INNER JOIN tApresentacaoSetor aa (NOLOCK) ON aa.SetorID = q.SetorID and aa.ApresentacaoID = q.ApresentacaoID 

						CREATE INDEX IX_QTD_X_tIngresso ON #Qtd (ApresentacaoSetorID)
						CREATE INDEX IX_QTD_X_tIngresso2 ON #Qtd (SetorID)
						CREATE INDEX IX_QTD_X_tIngresso3 ON #Qtd (ApresentacaoID)";

                bd.Executar(sql);

                //Esse select traz os dados para que a tabela de setores possa ser atualizada
                bd.Consulta(@"  DECLARE @apresentacaoSetorID INT
								DECLARE @setorID INT
								DECLARE @apresentacaoID INT
								DECLARE @QtdDisp INT
								DECLARE @QtdBloq INT
								SET @apresentacaoSetorID = (SELECT MAX(ApresentacaoSetorID) FROM #Qtd)

								WHILE (@apresentacaoSetorID IS NOT NULL)
									BEGIN
										SET @apresentacaoID = (SELECT Q.ApresentacaoID		
										FROM #Qtd Q
										WHERE Q.ApresentacaoSetorID = @apresentacaoSetorID)

										SET @setorID = (SELECT Q.SetorID
										FROM #Qtd Q		
										WHERE Q.ApresentacaoSetorID = @apresentacaoSetorID)

										SET @QtdDisp = (SELECT ISNULL(CASE WHEN Status = 'D' THEN SUM(1) END,0)
										FROM #Qtd Q
										INNER JOIN tIngresso i (NOLOCK) ON q.ApresentacaoSetorID = i.ApresentacaoSetorID
										WHERE Status IN ('D') AND i.ApresentacaoSetorID = @apresentacaoSetorID
										GROUP BY i.ApresentacaoSetorID, Status, Q.ApresentacaoID, Q.SetorID)

										SET @QtdBloq = (SELECT ISNULL(CASE WHEN Status = 'B' THEN SUM(1) END,0)
										FROM #Qtd Q
										INNER JOIN tIngresso i (NOLOCK) ON q.ApresentacaoSetorID = i.ApresentacaoSetorID
										WHERE Status IN ('B') AND i.ApresentacaoSetorID = @apresentacaoSetorID
										GROUP BY i.ApresentacaoSetorID, Status, Q.ApresentacaoID, Q.SetorID)

										INSERT INTO #QtdFinal (ApresentacaoSetorID, ApresentacaoID, ID, QtdeDisponivel, QtdeBloqueado)
										VALUES(@apresentacaoSetorID,@apresentacaoID, @setorID, @QtdDisp, @QtdBloq)

										DELETE FROM #Qtd WHERE ApresentacaoSetorID = @apresentacaoSetorID

										SET @apresentacaoSetorID = (SELECT MAX(ApresentacaoSetorID) FROM #Qtd)
									END                                

								SELECT * FROM #QtdFinal");

                while (bd.Consulta().Read())
                {
                    //Busca por Setor e Apresentação
                    DataRow[] setor = tabelaSetores.Select("ApresentacaoID=" + bd.LerInt("ApresentacaoID") + " AND ID=" + bd.LerInt("ID"));
                    setor[0]["QtdeDisponivel"] = bd.LerInt("QtdeDisponivel");
                    setor[0]["QtdeBloqueado"] = bd.LerInt("QtdeBloqueado");
                }

                tabelaSetores.AcceptChanges();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
            return tabelaSetores;
        }

        public bool DiasTriagem(int vendaBilheteriaID)
        {
            bool retorno = false;



            return retorno;
        }


        public void ReservarAssinatura(
            BD bdReserva,
            int clienteID, int assinaturaID, int setorID, int lugarID,
            int assinaturaAnoID, EstruturaReservaInternet estruturaIdentificacaoUsuario, int assinaturaClienteID)
        {
            BD bd = new BD();
            Ingresso ingresso = new Ingresso();

            try
            {
                string sql =
                    string.Format(@"
						SELECT
							DISTINCT i.ID, i.ApresentacaoSetorID, i.LugarID, i.Status, i.CodigoBarra
							FROM tAssinatura a (NOLOCK)
							INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
							INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
							INNER JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.SetorID = ai.SetorID
							WHERE a.ID = {0} AND an.ID = {1} AND ai.SetorID = {2} AND i.LugarID = {3}  AND a.Ativo = 'T'
					", assinaturaID, assinaturaAnoID, setorID, lugarID);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Este lugar não está disponível para reserva.");

                do
                {
                    string status = bd.LerString("Status");
                    if (status != Ingresso.DISPONIVEL)
                        throw new IngressoException("Este lugar não está disponível para reservas, por favor tente outro lugar.");

                    ingresso.Limpar();
                    ingresso.Control.ID = bd.LerInt("ID");
                    ingresso.PrecoID.Valor = 0;
                    ingresso.UsuarioID.Valor = estruturaIdentificacaoUsuario.UsuarioID;
                    ingresso.CortesiaID.Valor = 0;
                    ingresso.Status.Valor = Ingresso.RESERVADO;
                    ingresso.CodigoBarra.Valor = bd.LerString("CodigoBarra");
                    ingresso.PacoteID.Valor = 0;
                    ingresso.PacoteGrupo.Valor = string.Empty;
                    ingresso.LojaID.Valor = estruturaIdentificacaoUsuario.LojaID;
                    ingresso.ClienteID.Valor = clienteID;
                    ingresso.TimeStampReserva.Valor = DateTime.Now.AddMinutes(new ConfigGerenciadorParalela().getValorTempoReserva());
                    ingresso.SessionID.Valor = string.Empty;
                    ingresso.AssinaturaClienteID.Valor = assinaturaClienteID;

                    if (bdReserva.Executar(ingresso.StringReservar()) != 1)
                        throw new IngressoException("Não foi possível efetuar a reserva desta assinatura, por favor tente novamente.");

                } while (bd.Consulta().Read());
            }
            finally
            {
                bd.Fechar();
            }
        }


        public string VendaPayPal(string SenhaVenda)
        {
            BD bd = new BD();

            try
            {
                string TransactionID = string.Empty;

                string sql = string.Format(@"SELECT TransactionID From tVendaBilheteriaFormaPagamento (NOLOCK)
				INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID
				WHERE tVendaBilheteria.Senha = '{0}'", SenhaVenda);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    TransactionID = bd.LerString("TransactionID");

                return TransactionID;
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

        public List<EstruturaRetornoVendaPos> ReservarPos(string poscounter, string posnumber, string posticket)
        {
            int[] ingressosIDs = null;
            int usuarioID = 0;

            try
            {
                EstruturaEstadoCidadeSenhaPos estrutura = new Loja().VerificarExistenciaPOS(posnumber);
                List<EstruturaRetornoVendaPos> retornoVenda = new List<EstruturaRetornoVendaPos>();

                usuarioID = estrutura.UsuarioID;

                int qtdReserva = Convert.ToInt32(poscounter);
                int reservaID = 0;

                EstruturaVendaPos estruturaEvento = new Evento().BuscaInfosVendaPos(Convert.ToInt32(posticket));

                DataSet retorno = new DataSet();

                if (estruturaEvento.LugarMarcado == Setor.Pista)
                    retorno = this.ReservarPista(estrutura.LojaID, usuarioID, estrutura.CanalID, 0, 0, estruturaEvento.EventoID, estruturaEvento.ApresentacaoID, estruturaEvento.ApresentacaoSetorID, estruturaEvento.PrecoID, estruturaEvento.NomePreco, 0, 0, qtdReserva, ++reservaID, false, false, false, 0, new EstruturaCotasInfo(), 0, string.Empty, 0, false, 0, 0);
                else
                {
                    Setor.enumLugarMarcado tipoSetor = (Setor.enumLugarMarcado)(Convert.ToChar(estruturaEvento.LugarMarcado));

                    List<IRLib.Paralela.Lugar> lugares = new IRLib.Paralela.Lugar().MelhorLugarMarcado(qtdReserva, estruturaEvento.ApresentacaoSetorID, tipoSetor);

                    int quantidadeMesaAberta = 0;
                    bool mesaAberta = (tipoSetor == IRLib.Paralela.Setor.enumLugarMarcado.MesaAberta);

                    if (mesaAberta)
                    {
                        foreach (IRLib.Paralela.Lugar itemLugar in lugares)
                            quantidadeMesaAberta += itemLugar.Quantidade.Valor;

                        if (quantidadeMesaAberta != qtdReserva)
                        {
                            lugares.Clear();
                            throw new Exception("Não existem mesas com a capacidade de acomodar todas as pessoas juntas");
                        }
                    }
                    else
                        if (lugares.Count != qtdReserva)
                            lugares.Clear();

                    DataTable reserva = new DataTable("ReservaLugarMarcado");

                    reserva.Columns.Add("LugarID", typeof(int));
                    reserva.Columns.Add("QtdeReservar", typeof(int));
                    reserva.Columns.Add("QtdeReservado", typeof(int));
                    reserva.Columns.Add("Reservado", typeof(bool));
                    reserva.Columns.Add("Cod", typeof(int));
                    reserva.Columns.Add("TipoLugar", typeof(string));

                    int tmpQtdeSelecionada = 0;

                    foreach (IRLib.Paralela.Lugar lugar in lugares)
                    {
                        if (!mesaAberta)
                            lugar.Ler(lugar.Control.ID);

                        DataRow novaReserva = reserva.NewRow();
                        novaReserva["LugarID"] = lugar.Control.ID;
                        novaReserva["TipoLugar"] = Convert.ToChar(tipoSetor);

                        tmpQtdeSelecionada += lugar.Quantidade.Valor;
                        novaReserva["QtdeReservar"] = lugar.Quantidade.Valor;

                        reserva.Rows.Add(novaReserva);
                    }

                    retorno = this.ReservarLugarMarcado(reserva, estrutura.LojaID, usuarioID, estrutura.CanalID, estruturaEvento.EventoID, estruturaEvento.ApresentacaoID, estruturaEvento.ApresentacaoSetorID, estruturaEvento.PrecoID, estruturaEvento.NomePreco, 0, qtdReserva, false, false, false, false, 0, new EstruturaCotasInfo(), 0, 0, false, 0);
                }

                ingressosIDs = new int[retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows.Count];

                for (int i = 0; i < retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows.Count; i++)
                    if (string.IsNullOrEmpty(retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i][BilheteriaParalela.CODIGO_BARRA].ToString()))
                        ingressosIDs[i] = (int)retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i]["IngressoID"];

                List<EstruturaRetornoRegistroImpressao> retornoRegistroImpressao = this.BuscaCodigoBarrasPos(ingressosIDs);

                if (retornoRegistroImpressao != null || retornoRegistroImpressao.Count != 0)
                {
                    int contadorIngresso = 1;

                    for (int i = 0; i < retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows.Count; i++)
                    {
                        foreach (EstruturaRetornoRegistroImpressao item in retornoRegistroImpressao)
                        {
                            if (item.ID == Convert.ToInt32(retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i]["IngressoID"]))
                            {
                                retornoVenda.Add(new EstruturaRetornoVendaPos
                                {
                                    Contador = contadorIngresso,
                                    EventoID = Convert.ToInt32(retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i]["EventoID"]),
                                    IngressoID = Convert.ToInt32(retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i]["IngressoID"]),
                                    Senha = Convert.ToString(retorno.Tables[BilheteriaParalela.TABELA_GRID].Rows[i]["Cod"]),
                                    CodigoBarra = item.CodigoBarra,
                                    PrecoID = Convert.ToInt32(retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i]["PrecoID"])
                                });

                                retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i][BilheteriaParalela.CODIGO_BARRA] = item.CodigoBarra;
                                retorno.Tables[BilheteriaParalela.TABELA_RESERVA].Rows[i][BilheteriaParalela.CODIGO_SEQUENCIAL] = item.CodigoImpressao;

                                contadorIngresso++;
                                break;
                            }
                        }
                    }

                    retornoVenda[0].Reserva = retorno;

                    return retornoVenda;
                }
                else
                    throw new Exception("Erro ao reservar.");
            }
            catch (Exception ex)
            {
                if (ingressosIDs != null)
                    this.CancelarReservas(ingressosIDs, usuarioID);

                throw ex;
            }
        }

        public bool VenderPos(DataSet reserva, EstruturaEstadoCidadeSenhaPos estrutura, string ticketbarcode, string poscpf)
        {
            try
            {
                IRLib.Paralela.Cliente oCliente = new IRLib.Paralela.Cliente();
                int[] retornoPesquisaCliente = oCliente.BuscarClienteCPFMobile(poscpf);
                int clienteID = retornoPesquisaCliente[1];
                decimal valorTotalIngressos = 0;
                decimal valorTotalConveniencia = 0;
                decimal taxaMaximaPorEmpresa = 0;
                bool retorno = false;

                DataRow[] item = reserva.Tables[BilheteriaParalela.TABELA_RESERVA].Select("CodigoBarra='" + ticketbarcode + "'");

                foreach (DataRow linha in reserva.Tables[BilheteriaParalela.TABELA_GRID].Select("ReservaID=" + item[0][BilheteriaParalela.RESERVAID].ToString()))
                {
                    EstruturaVendaPos estruturaEvento = new Evento().BuscaInfosVendaPos(Convert.ToInt32(linha[BilheteriaParalela.PRECOID]));

                    taxaMaximaPorEmpresa = Convert.ToDecimal(new Empresa().CarregaTaxa(estruturaEvento.EmpresaID));
                    decimal valor = (decimal)linha[BilheteriaParalela.VALOR];
                    int conv = (int)linha[BilheteriaParalela.CONV];
                    decimal taxaMin = (decimal)linha[BilheteriaParalela.CONV_MIN];
                    decimal taxaMax = (decimal)linha[BilheteriaParalela.CONV_MAX];

                    valorTotalIngressos += valor;

                    decimal dAux = (decimal)linha["Valor Conv"];
                    valorTotalConveniencia += dAux;

                    if (valorTotalConveniencia < taxaMaximaPorEmpresa || taxaMaximaPorEmpresa == 0)
                        linha[BilheteriaParalela.VALOR_CONV] = dAux;
                    else
                    {
                        if ((dAux - (valorTotalConveniencia - taxaMaximaPorEmpresa)) < 0)
                            linha[BilheteriaParalela.VALOR_CONV] = 0;
                        else
                            linha[BilheteriaParalela.VALOR_CONV] = (dAux - (valorTotalConveniencia - taxaMaximaPorEmpresa));
                    }

                    if (taxaMaximaPorEmpresa != 0 && taxaMaximaPorEmpresa < valorTotalConveniencia)
                        valorTotalConveniencia = taxaMaximaPorEmpresa;

                    decimal valorTotalVenda = (valorTotalIngressos + valorTotalConveniencia);

                    DataTable tPagamento = new Loja().BuscaFormaDePagamento(estruturaEvento.EmpresaID, estrutura.CanalID, valorTotalVenda);

                    DataTable tGridLeve = reserva.Tables[BilheteriaParalela.TABELA_GRID].Copy();

                    foreach (DataRow excluir in tGridLeve.Select("ReservaID<>" + item[0][BilheteriaParalela.RESERVAID].ToString()))
                        excluir.Delete();

                    tGridLeve.Columns.Remove(BilheteriaParalela.EVENTO_PACOTE);
                    tGridLeve.Columns.Remove(BilheteriaParalela.HORARIO);
                    tGridLeve.Columns.Remove(BilheteriaParalela.SETOR_PRODUTO);
                    tGridLeve.Columns.Remove(BilheteriaParalela.CODIGO);
                    tGridLeve.Columns.Remove(BilheteriaParalela.PRECO);
                    tGridLeve.Columns.Remove(BilheteriaParalela.CORTESIA);
                    tGridLeve.Columns.Remove(BilheteriaParalela.VALOR);
                    tGridLeve.Columns.Remove(BilheteriaParalela.LUGAR_MARCADO);
                    tGridLeve.Columns.Remove(BilheteriaParalela.CLIENTE);
                    tGridLeve.Columns.Remove(BilheteriaParalela.COTA_ITEM_ID);
                    tGridLeve.Columns.Remove(BilheteriaParalela.COTA);

                    tGridLeve.AcceptChanges();

                    DataTable tReserva = reserva.Tables[BilheteriaParalela.TABELA_RESERVA].Copy(); ;

                    foreach (DataRow excluir in tReserva.Select("CodigoBarra<>'" + ticketbarcode + "'"))
                        excluir.Delete();

                    tReserva.AcceptChanges();

                    int caixaID = VerificaCaixaPos(estrutura.UsuarioID, estrutura.LojaID);

                    EstruturaRetornoVenda retornoVendapos = this.Vender(tGridLeve, tReserva, tPagamento, caixaID, estrutura.LojaID, estrutura.CanalID, estruturaEvento.EmpresaID, clienteID, 0, 0, valorTotalVenda, estrutura.UsuarioID, 0, false,
                      string.Empty, 0, new Dictionary<int, int>(), new Dictionary<int, int>(), Canal.TipoDeVenda.ImpressaoIngresso, false, false, string.Empty, new List<EstruturaDonoIngresso>(), new List<int>(), string.Empty, 0, new EstruturaEntregaAgenda(),
                      0, string.Empty, false, 0);

                    retorno = this.AtualizaCodigoBarraPos((int)item[0][BilheteriaParalela.INGRESSOID], estrutura.UsuarioID, estrutura.EmpresaID, caixaID, estrutura.CanalID, estrutura.LojaID, string.Empty, estrutura.UsuarioID, (string)item[0][BilheteriaParalela.CODIGO_BARRA], (int)item[0][BilheteriaParalela.CODIGO_SEQUENCIAL]);
                }

                return retorno;
            }
            catch
            {
                return false;
            }
        }

        public static int VerificaCaixaPos(int UsuarioID, int LojaID)
        {
            Servidor servidor = new Servidor();
            Caixa caixa = new Caixa(UsuarioID);

            bool caixaAberto = caixa.Aberto(UsuarioID);

            if (caixaAberto)
            {
                if (caixa.LojaID.Valor == LojaID)
                {
                    if (caixa.DataAbertura.Valor < servidor.Hoje)
                    {
                        caixa.Fechar();
                        caixa.Control.ID = 0;
                    }
                }
                else
                {
                    caixa.Fechar();
                    caixa.Control.ID = 0;
                }
            }

            if (caixa.Control.ID == 0)
                caixa.Abrir(UsuarioID, LojaID, 0, 0);

            return caixa.Control.ID;
        }

        public bool CancelarPos(string ticketbarcode, string posnumber)
        {
            try
            {
                EstruturaEstadoCidadeSenhaPos estrutura = new Loja().VerificarExistenciaPOS(posnumber);
                int caixaID = VerificaCaixaPos(estrutura.UsuarioID, estrutura.LojaID);

                CancelamentoGerenciador cancelamento = new CancelamentoGerenciador();
                cancelamento.UsuarioID = estrutura.UsuarioID;
                cancelamento.CaixaID = caixaID;
                cancelamento.CanalID = estrutura.CanalID;
                cancelamento.LocalID = estrutura.LojaID;
                cancelamento.PerfilID = Perfil.SAC_OPERADOR;
                cancelamento.EmpresaID = estrutura.EmpresaID;

                DataSet retorno = cancelamento.PesquisarCodigoBarras(ticketbarcode);

                DataTable tGrid = retorno.Tables[CancelamentoGerenciador.TABELA_GRID];
                DataTable tInfoVenda = retorno.Tables[CancelamentoGerenciador.INFO_VENDA];
                DataTable tGridInfoPacote = retorno.Tables[CancelamentoGerenciador.TABELA_INFO_PACOTE];
                DataTable tReserva = retorno.Tables[CancelamentoGerenciador.TABELA_RESERVA];

                if (tGrid.Rows.Count > 0)
                {
                    DataRow[] disponivelAjuste = tGrid.Select("DisponivelAjuste='F'");

                    if (disponivelAjuste.Length == 0)
                    {
                        decimal valorIngressos = 0;
                        decimal valorConveniencias = 0;

                        for (int i = 0; i < tGrid.Rows.Count; i++)
                        {
                            bool cancelado = (bool)tGrid.Rows[i][CancelamentoGerenciador.CANCELADO];
                            string status = (string)tGrid.Rows[i][CancelamentoGerenciador.STATUS];

                            if (cancelado || status == Ingresso.DISPONIVEL)
                                throw new Exception("Sem ingressos para cancelar!");

                            valorIngressos += (decimal)tGrid.Rows[i][CancelamentoGerenciador.VALOR];
                            valorConveniencias += (decimal)tGrid.Rows[i][CancelamentoGerenciador.VALOR_CONV];
                        }

                        DataTable itensReservados = tGridInfoPacote.Clone();

                        foreach (DataRow item in tGrid.Rows)
                            itensReservados.ImportRow(item);

                        itensReservados.Columns.Remove(CancelamentoGerenciador.EVENTO_PACOTE);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.HORARIO);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.SETOR_PRODUTO);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.CODIGO);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.PRECO);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.CORTESIA);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.BLOQUEIO);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.VALOR);
                        itensReservados.Columns.Remove(CancelamentoGerenciador.CANCELADO);

                        decimal valorTotalVenda = (valorIngressos + valorConveniencias);

                        DataTable tPagamento = new Loja().BuscaFormaDePagamento(estrutura.EmpresaID, estrutura.CanalID, valorTotalVenda);

                        int VendaBilheteriaID = Convert.ToInt32(tInfoVenda.Rows[0]["ID"]);

                        string senhaTmp = this.Cancelar(itensReservados, tReserva, tPagamento, caixaID, estrutura.LojaID, estrutura.CanalID, estrutura.EmpresaID, 0, 0, 0, valorTotalVenda, estrutura.UsuarioID, string.Empty, string.Empty, true,
                            false, "Solicitação do Cliente", tInfoVenda.Rows[0][CancelamentoGerenciador.STATUS_VENDA].ToString() == VendaBilheteria.FRAUDE, VendaBilheteriaID, 12,
                                    estrutura.UsuarioID, 0, false, 0, false, string.Empty, false, false, string.Empty);
                    }
                    else
                        throw new Exception("Evento não disponivel para Ajuste");
                }
                else
                    throw new Exception("Sem ingressos para cancelar!");

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SangriaPos(string posnumber, string eventid, string totalcash, string userpassword)
        {
            try
            {
                EstruturaEstadoCidadeSenhaPos estrutura = new Loja().VerificarExistenciaPOS(posnumber);

                int EventoID = Convert.ToInt32(eventid);
                int CaixaID = VerificaCaixaPos(estrutura.UsuarioID, estrutura.LojaID);
                decimal Valor = Convert.ToDecimal(totalcash.Replace(".", ","));

                EstruturaTelaSangria estSangria = new Evento().BuscarCaixasSangria(estrutura.CanalID, CaixaID, EventoID, DateTime.Now, false);

                foreach (var item in estSangria.Lista)
                    if (item.Valor >= Valor)
                        item.Sangria = Valor;
                    else
                        throw new Exception("Valor não permitido!");

                EstruturaSangriaImpressao ImpSangria = new VendaBilheteria().SangriaCaixa(estSangria, EventoID, 17, estrutura.UsuarioID, estrutura.UsuarioNome, userpassword);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsAssinatura(int pVendaBilheteriaID)
        {
            try
            {
                bool isAssinatura = false;

                BD bd = new BD();

                string sql = @"SELECT * 
								FROM tAssinaturaCliente (NOLOCK) 
								WHERE VendaBIlheteriaID = " + pVendaBilheteriaID;

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                    isAssinatura = true;
                bd.FecharConsulta();

                return isAssinatura;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class RetornoCarregarDados
    {
        public bool CarregadoComSucesso { get; set; }
        public string Mensagem { get; set; }
        public DataSet Dados { get; set; }
    }

    [Serializable]
    public class VendaCanceladaException : ApplicationException
    {
        public int VendaBilheteriaID { get; set; }

        public override string Message
        {
            get
            {
                return string.Format("Venda {0} já cancelada", this.VendaBilheteriaID.ToString());
            }
        }
        public VendaCanceladaException(int vendaID)
        {
            this.VendaBilheteriaID = vendaID;
        }
    }


    [Serializable]
    public class BilheteriaException : Exception
    {
        private BilheteriaParalela.CodMensagemVenda codigoErro = BilheteriaParalela.CodMensagemVenda.ErroIndefinido; // -1 não contém erro.

        private BilheteriaParalela.CodMensagemReserva codigoErroReserva = BilheteriaParalela.CodMensagemReserva.Nulo; // -1 não contém erro.

        private BilheteriaParalela.CodMensagemCarregarInternet codigoErroCarregar = BilheteriaParalela.CodMensagemCarregarInternet.Nulo; // -1 não contém erro.

        public BilheteriaParalela.CodMensagemVenda CodigoErro
        {
            get { return this.codigoErro; }
            set { this.codigoErro = value; }
        }

        public BilheteriaException(string msg, BilheteriaParalela.CodMensagemVenda codigoErro)
            : base(msg)
        {
            this.codigoErro = codigoErro;
        }
        public BilheteriaException(string msg, BilheteriaParalela.CodMensagemReserva codigoErro)
            : base(msg)
        {
            this.codigoErroReserva = codigoErro;
        }
        public BilheteriaException(string msg, BilheteriaParalela.CodMensagemCarregarInternet codigoErro)
            : base(msg)
        {
            this.codigoErroCarregar = codigoErro;
        }

        public BilheteriaException() : base() { }

        public BilheteriaException(string msg) : base(msg) { }

        public BilheteriaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            base.GetObjectData(info, context);
        }

    }

}