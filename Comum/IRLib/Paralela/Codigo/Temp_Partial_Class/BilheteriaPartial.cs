using CTLib;
using IRLib.Mondial;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ExpressCheckout;
using IRLib.Paralela.PayPal.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace IRLib.Paralela
{
    public partial class BilheteriaParalela
    {

        public DataSet CarregarInternet()
        {

            BD bd = new BD();
            SqlConnection connection = null;

            SqlCommand comm = null;
            SqlDataAdapter adapter = null;


            try
            {

                //Carregar eventos do canal
                Canal canal = new Canal();
                canal.Ler(Canal.CANAL_INTERNET);

                if (canal.Control.ID == 0)
                    throw new BilheteriaException("Canal não existe!");

                DataSet buffer = new DataSet("Buffer");

                #region Estrutura Buffer
                DataTable formaPagamentoEvento = new DataTable("FormaPagamentoEvento");
                formaPagamentoEvento.Columns.Add("ID", typeof(int));
                formaPagamentoEvento.Columns.Add("EventoID", typeof(int));
                formaPagamentoEvento.Columns.Add("FormaPagamentoID", typeof(int));

                DataTable locais = new DataTable("Local");
                locais.Columns.Add("ID", typeof(int));
                locais.Columns.Add("Nome", typeof(string));
                locais.Columns.Add("Endereco", typeof(string));
                locais.Columns.Add("CEP", typeof(string));
                locais.Columns.Add("DDDTelefone", typeof(string));
                locais.Columns.Add("Telefone", typeof(string));
                locais.Columns.Add("Cidade", typeof(string));
                locais.Columns.Add("Estado", typeof(string));
                locais.Columns.Add("Obs", typeof(string));
                locais.Columns.Add("ComoChegar", typeof(string));

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("LocalID", typeof(int));
                eventos.Columns.Add("Release", typeof(string));
                eventos.Columns.Add("TipoID", typeof(int));
                eventos.Columns.Add("SubtipoID", typeof(int));
                eventos.Columns.Add("Imagem", typeof(string));
                eventos.Columns.Add("Destaque", typeof(bool));
                eventos.Columns.Add("Prioridade", typeof(int));
                eventos.Columns.Add("Parcelas", typeof(int));
                eventos.Columns.Add("RetiradaBilheteria", typeof(bool));
                eventos.Columns.Add("EntregaGratuita", typeof(bool));
                eventos.Columns.Add("DisponivelAvulso", typeof(bool));
                eventos.Columns.Add("Publicar", typeof(string));
                eventos.Columns.Add("PublicarSemVendaMotivo", typeof(int));
                eventos.Columns.Add("DataAberturaVenda", typeof(string));
                eventos.Columns.Add("LocalImagemMapaID", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Horario", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));

                DataTable setores = new DataTable("Setor");
                setores.Columns.Add("ID", typeof(int));
                setores.Columns.Add("Nome", typeof(string));
                setores.Columns.Add("LugarMarcado", typeof(string));
                setores.Columns.Add("ApresentacaoID", typeof(int));
                setores.Columns.Add("QuantidadeMapa", typeof(int));
                setores.Columns.Add("QtdeDisponivel", typeof(int));


                DataTable precos = new DataTable("Preco");
                precos.Columns.Add("ID", typeof(int));
                precos.Columns.Add("Nome", typeof(string));
                precos.Columns.Add("Valor", typeof(decimal));
                precos.Columns.Add("ApresentacaoID", typeof(int));
                precos.Columns.Add("SetorID", typeof(int));
                precos.Columns.Add("Quantidade", typeof(int));
                precos.Columns.Add("QuantidadePorCliente", typeof(int));
                precos.Columns.Add("Pacote", typeof(bool)); // Define se é um preço de pacote ou não (não é exclusivo).

                // busca os tipos de eventos
                DataTable tipos = new DataTable("Tipo");
                tipos.Columns.Add("ID", typeof(int));
                tipos.Columns.Add("Nome", typeof(string));

                // busca os subtipos dos eventos
                DataTable subtipos = new DataTable("Subtipo");
                subtipos.Columns.Add("ID", typeof(int));
                subtipos.Columns.Add("EventoTipoID", typeof(int));
                subtipos.Columns.Add("Descricao", typeof(string));

                //Busca os tipos e subtipos dos eventos (Leandro Basso)
                DataTable eventoTipos = new DataTable("Tipos");
                eventoTipos.Columns.Add("ID", typeof(int));
                eventoTipos.Columns.Add("EventoID", typeof(int));
                eventoTipos.Columns.Add("EventoTipoID", typeof(int));
                eventoTipos.Columns.Add("EventoSubtipoID", typeof(int));

                // Taxa Entrega
                DataTable taxasEntrega = new DataTable("TaxaEntrega");
                taxasEntrega.Columns.Add("ID", typeof(int));
                taxasEntrega.Columns.Add("Nome", typeof(string));
                taxasEntrega.Columns.Add("Valor", typeof(decimal));
                taxasEntrega.Columns.Add("PrazoEntrega", typeof(int));
                taxasEntrega.Columns.Add("Estado", typeof(string));

                // Taxa Entrega do Evento
                DataTable taxasEventoTaxaEntrega = new DataTable("EventoTaxaEntrega");
                taxasEventoTaxaEntrega.Columns.Add("ID", typeof(int));
                taxasEventoTaxaEntrega.Columns.Add("EventoID", typeof(int));
                taxasEventoTaxaEntrega.Columns.Add("TaxaEntregaID", typeof(int));
                taxasEventoTaxaEntrega.Columns.Add("DetalhesEntrega", typeof(string));


                // Pacotes
                DataTable pacotes = new DataTable("Pacote");
                pacotes.Columns.Add("ID", typeof(int));
                pacotes.Columns.Add("Nome", typeof(string));

                //Itens Pacote
                DataTable pacoteItens = new DataTable("PacoteItem");
                pacoteItens.Columns.Add("ID", typeof(int));
                pacoteItens.Columns.Add("PacoteID", typeof(int));
                pacoteItens.Columns.Add("PrecoID", typeof(int));

                // Banners
                DataTable banners = new DataTable("Banner");
                banners.Columns.Add("ID", typeof(int));
                banners.Columns.Add("Nome", typeof(string));
                banners.Columns.Add("Alt", typeof(string));
                banners.Columns.Add("Url", typeof(string));
                banners.Columns.Add("Target", typeof(int));
                banners.Columns.Add("Localizacao", typeof(int));
                banners.Columns.Add("Posicao", typeof(int));
                banners.Columns.Add("Descricao", typeof(string));

                //ValeIngressoTipo
                DataTable valeingressotipo = new DataTable("ValeIngressoTipo");
                valeingressotipo.Columns.Add("ID", typeof(int));
                valeingressotipo.Columns.Add("Nome", typeof(string));
                valeingressotipo.Columns.Add("Valor", typeof(decimal));
                valeingressotipo.Columns.Add("ValidadeData", typeof(string));
                valeingressotipo.Columns.Add("ValidadeDiasImpressao", typeof(int));
                valeingressotipo.Columns.Add("ProcedimentoTroca", typeof(string));
                valeingressotipo.Columns.Add("Acumulativo", typeof(string));
                valeingressotipo.Columns.Add("ReleaseInternet", typeof(string));

                #endregion

                string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                connection = (SqlConnection)bd.Cnn;

                comm = connection.CreateCommand();
                //comm.CommandTimeout = int.MaxValue;
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                comm.CommandText = "EXEC PopulaEventosInternet";
                comm.ExecuteNonQuery();

                string sql = "";

                // Forma de Pagamento Evento
                comm.CommandText = "" +
                "SELECT DISTINCT " +
                "tFormaPagamentoEvento.ID,  " +
                "tFormaPagamentoEvento.EventoID,  " +
                "tFormaPagamentoEvento.FormaPagamentoID  " +
                "FROM EventosInternet " +
                "INNER JOIN tFormaPagamentoEvento(NOLOCK) ON tFormaPagamentoEvento.EventoID = EventosInternet.EventoID ";



                adapter.Fill(formaPagamentoEvento);

                // Locais
                comm.CommandText = "SELECT DISTINCT LocalID AS ID, Local AS Nome, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, CAST(LocalObs AS VARCHAR(3000)) AS Obs, CAST(ComoChegarInternet AS VARCHAR(3000)) as ComoChegar FROM vwInfoVendaInternet(NOLOCK) WHERE (Publicar = 'T' OR Publicar = 'S')";
                adapter.Fill(locais);

                if (locais.Rows.Count == 0)
                    throw new BilheteriaException("Locais não foram carregados como deveriam ou simplesmente não existem");

                //Eventos
                comm.CommandText = "SELECT DISTINCT " +
                    "EventoID AS ID, Evento AS Nome, LocalID,  CAST(Resenha AS VARCHAR(5000)) AS Release, " +
                    "CASE Destaque WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS Destaque, PrioridadeDestaque AS Prioridade, " +
                    "ImagemInternet AS Imagem, TipoID, SubtipoID, Parcelas,  CASE RetiradaBilheteria WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS RetiradaBilheteria, " +
                    "CASE EntregaGratuita WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS EntregaGratuita, 1 AS DisponivelAvulso, " +
                    "Publicar, PublicarSemVendaMotivo,DataAberturaVenda,LocalImagemMapaID " +
                    "FROM vwInfoVendaInternet (NOLOCK) " +
                    "WHERE (Publicar = '" + Convert.ToChar(Evento.PublicarTipo.PublicadoParaVenda).ToString() + "' OR Publicar = '" + Convert.ToChar(Evento.PublicarTipo.PublicadoSemVenda).ToString() + "')";
                adapter.Fill(eventos);
                if (eventos.Rows.Count == 0)
                    throw new BilheteriaException("Eventos não foram carregados como deveriam ou simplesmente não existem.\nVerifique se eles foram liberados para seu canal.\n");

                // DataRow's usados em Pacotes
                DataRow[] drs = null;
                DataRow dr = null;


                // Eventos Pacotes
                sql = "SELECT DISTINCT " +
                        "EventoID AS ID, Evento AS Nome, LocalID, CAST(Resenha AS VARCHAR(5000)) AS Release, " +
                        "CASE Destaque WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS Destaque, PrioridadeDestaque AS Prioridade, " +
                        "ImagemInternet AS Imagem, TipoID, SubtipoID, Parcelas,  " +
                        "CASE RetiradaBilheteria WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS RetiradaBilheteria, " +
                        "CASE EntregaGratuita WHEN 'T' THEN 1 WHEN 'F' THEN 0 ELSE 0 END AS EntregaGratuita, " +
                        "Publicar, PublicarSemVendaMotivo,DataAberturaVenda, LocalImagemMapaID " +
                        "FROM vwInfoVendaPacoteInternet (NOLOCK) ";

                while (bd.Consulta(sql).Read())
                {
                    drs = eventos.Select("ID = " + bd.LerInt("ID"));
                    if (drs.Length == 0)
                    {

                        //novo registro para inserir
                        dr = eventos.NewRow();
                        eventos.Rows.Add(dr);

                        dr["ID"] = bd.LerInt("ID");
                        dr["Nome"] = bd.LerString("Nome");
                        dr["LocalID"] = bd.LerInt("LocalID");
                        dr["Release"] = bd.LerString("Release");
                        dr["TipoID"] = bd.LerInt("TipoID");
                        dr["SubtipoID"] = bd.LerInt("SubtipoID");
                        dr["Imagem"] = bd.LerString("Imagem");
                        dr["Destaque"] = bd.LerInt("Destaque");
                        dr["Prioridade"] = bd.LerInt("Prioridade");
                        dr["Parcelas"] = bd.LerInt("Parcelas");
                        dr["RetiradaBilheteria"] = bd.LerInt("RetiradaBilheteria");
                        dr["EntregaGratuita"] = bd.LerInt("EntregaGratuita");
                        dr["DisponivelAvulso"] = false;
                        dr["Publicar"] = bd.LerString("Publicar");
                        dr["PublicarSemVendaMotivo"] = bd.LerInt("PublicarSemVendaMotivo");
                        dr["DataAberturaVenda"] = bd.LerString("DataAberturaVenda");
                        dr["LocalImagemMapaID"] = bd.LerInt("LocalImagemMapaID");
                    }

                }
                bd.FecharConsulta();

                // Apresentações
                sql = "SELECT DISTINCT ApresentacaoID AS ID, Apresentacao AS Horario, EventoID FROM vwInfoVendaInternet(NOLOCK) WHERE (Publicar = 'T' or Publicar = 'S')";
                //adapter.Fill(apresentacoes);
                DataRow apresentacao = null;
                while (bd.Consulta(sql).Read())
                {
                    apresentacao = apresentacoes.NewRow();
                    apresentacoes.Rows.Add(apresentacao);

                    apresentacao["ID"] = bd.LerInt("ID");
                    apresentacao["Horario"] = bd.LerString("Horario");
                    apresentacao["EventoID"] = bd.LerInt("EventoID");
                }
                bd.FecharConsulta();

                if (apresentacoes.Rows.Count == 0)
                    throw new BilheteriaException("Apresentações não foram carregadas como deveriam ou simplesmente não existem ou não estão disponíveis para venda.");


                // Apresentacoes Pacotes
                sql = "SELECT DISTINCT ApresentacaoID AS ID, Horario, EventoID FROM vwInfoVendaPacoteInternet(NOLOCK)";
                while (bd.Consulta(sql).Read())
                {
                    drs = apresentacoes.Select("ID = " + bd.LerInt("ID"));
                    if (drs.Length == 0)
                    {
                        //novo registro para inserir
                        dr = apresentacoes.NewRow();
                        apresentacoes.Rows.Add(dr);

                        dr["ID"] = bd.LerInt("ID");
                        dr["Horario"] = bd.LerString("Horario");
                        dr["EventoID"] = bd.LerInt("EventoID");
                    }
                }
                bd.FecharConsulta();


                comm.CommandText = "EXEC PopulaSetoresInternet";
                comm.ExecuteNonQuery();

                sql = @"
					   SELECT 
						DISTINCT vw.ID , Nome, LugarMarcado, ApresentacaoID, ISNULL(MIN(tLugar.Quantidade),0) AS QuantidadeMapa 
						FROM SetoresInternet vw (NOLOCK)  
						LEFT JOIN tLugar (NOLOCK) ON tLugar.SetorID = vw.ID AND tLugar.Quantidade > 0 
						GROUP BY vw.ID, Nome ,LugarMarcado, ApresentacaoID";

                while (bd.Consulta(sql).Read())
                {
                    //novo registro para inserir
                    dr = setores.NewRow();
                    setores.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Nome"] = bd.LerString("Nome");
                    dr["LugarMarcado"] = bd.LerString("LugarMarcado");
                    dr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    dr["QuantidadeMapa"] = bd.LerInt("QuantidadeMapa");
                }



                sql = @"SELECT  
						DISTINCT vw.SetorID AS ID, Setor AS Nome, LugarMarcado, ApresentacaoID, ISNULL(MIN(vw.Quantidade),0) AS QuantidadeMapa 
						FROM vwInfoVendaPacoteInternet vw (NOLOCK)  
						LEFT JOIN tLugar (NOLOCK) ON tLugar.SetorID= vw.SetorID AND vw.Quantidade > 0 
						WHERE (Publicar = 'T' OR Publicar = 'S')
						GROUP BY vw.SetorID, Setor ,LugarMarcado, ApresentacaoID";
                bd.FecharConsulta();
                while (bd.Consulta(sql).Read())
                {
                    //novo registro para inserir
                    dr = setores.NewRow();
                    setores.Rows.Add(dr);

                    dr["ID"] = bd.LerInt("ID");
                    dr["Nome"] = bd.LerString("Nome");
                    dr["LugarMarcado"] = bd.LerString("LugarMarcado");
                    dr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    dr["QuantidadeMapa"] = bd.LerInt("QuantidadeMapa");
                }
                bd.FecharConsulta();

                if (setores.Rows.Count == 0)
                    throw new BilheteriaException("Setores não foram carregados como deveriam.\nVerifique se os preços não foram liberados para seu canal.\n");

                // Preços
                comm.CommandText = "SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID, 0 AS Pacote FROM vwInfoVendaInternet (NOLOCK) WHERE (Publicar = 'T' OR Publicar = 'S')";
                adapter.Fill(precos);
                if (precos.Rows.Count == 0)
                    throw new BilheteriaException("Preços não foram carregados como deveriam ou simplesmente não existem.\nVerifique se eles foram liberados para seu canal.\n");


                // Precos Pacotes
                sql = "SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID FROM vwInfoVendaPacoteInternet (NOLOCK)";
                while (bd.Consulta(sql).Read())
                {
                    drs = precos.Select("ID = " + bd.LerInt("ID"));
                    if (drs.Length == 0)
                    {
                        //novo registro para inserir
                        dr = precos.NewRow();
                        precos.Rows.Add(dr);

                        dr["ID"] = bd.LerInt("ID");
                        dr["Nome"] = bd.LerString("Nome");
                        dr["Valor"] = bd.LerDecimal("Valor");
                        dr["SetorID"] = bd.LerInt("SetorID");
                        dr["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        //dr["Quantidade"] = bd.LerInt("Quantidade");
                        //dr["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                        dr["Pacote"] = 1;
                    }
                    else
                        drs[0]["Pacote"] = 0;
                }
                bd.FecharConsulta();

                //Taxas
                sql = "SELECT ID, Nome, Valor, Prazo as PrazoEntrega, Estado, ProcedimentoEntrega FROM tTaxaEntrega (NOLOCK) WHERE RegiaoID = 4 AND Disponivel = 'T'";
                comm.CommandText = sql;
                adapter.Fill(taxasEntrega);



                // Carrega Taxas de Entrega do Evento que estiver ativo para Internet
                sql = "SELECT DISTINCT tEventoTaxaEntrega.ID, tEventoTaxaEntrega.EventoID, tEventoTaxaEntrega.TaxaEntregaID,tEventoTaxaEntrega.DetalhesEntrega FROM tEventoTaxaEntrega (NOLOCK) INNER JOIN EventosInternet ON EventosInternet.EventoID = tEventoTaxaEntrega.EventoID";
                comm.CommandText = sql;
                adapter.Fill(taxasEventoTaxaEntrega);


                // Busca todos os tipos de eventos
                comm.CommandText = "SELECT ID, Nome FROM tEventoTipo";
                adapter.Fill(tipos);

                //Busca todos os Subtipos de eventos
                comm.CommandText = "SELECT ID, EventoTipoID, Descricao FROM tEventoSubtipo (NOLOCK)";
                adapter.Fill(subtipos);

                ////Busca os tipos e subtipos dos eventos (Leandro Basso)
                comm.CommandText = "SELECT ID ,EventoID ,EventoTipoID ,EventoSubtipoID FROM tEventoTipos (NOLOCK)";
                adapter.Fill(eventoTipos);


                //Pacote 
                sql = "SELECT DISTINCT PacoteID AS ID, Nome FROM vwInfoVendaPacoteInternet (NOLOCK) WHERE Horario > '20070510'";
                comm.CommandText = sql;
                adapter.Fill(pacotes);


                //Pacote Item
                sql = "SELECT DISTINCT PacoteItemID AS ID, PacoteID, EventoID, PrecoID, QuantidadePacoteItens AS Quantidade FROM vwInfoVendaPacoteInternet (NOLOCK) WHERE Horario > '20070510'";
                comm.CommandText = sql;
                adapter.Fill(pacoteItens);


                //Banners
                sql = "SELECT ID, Nome, Img, Alt, Url, Target, Localizacao, Posicao, Descricao FROM tBanner (NOLOCK)";
                comm.CommandText = sql;
                adapter.Fill(banners);

                //ValeIngressoTipo

                sql = "SELECT ID, Nome, Valor, IsNull(ValidadeData, 0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, ProcedimentoTroca, Acumulativo, ReleaseInternet FROM tValeIngressoTipo (NOLOCK) WHERE PublicarInternet='T' ";
                comm.CommandText = sql;
                adapter.Fill(valeingressotipo);

                buffer.Tables.Add(locais);
                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(setores);
                buffer.Tables.Add(precos);
                buffer.Tables.Add(taxasEntrega);
                buffer.Tables.Add(taxasEventoTaxaEntrega);
                buffer.Tables.Add(tipos);
                buffer.Tables.Add(subtipos);
                buffer.Tables.Add(eventoTipos);
                buffer.Tables.Add(pacotes);
                buffer.Tables.Add(pacoteItens);
                buffer.Tables.Add(banners);
                //buffer.Tables.Add(formasPagamento);
                buffer.Tables.Add(formaPagamentoEvento);
                buffer.Tables.Add(valeingressotipo);


                return buffer;

            }
            catch (Exception ex)
            {
                throw new Exception(ex + (comm != null ? comm.CommandText : ""));
            }

        }

        public int GetIngressosQPodeReservar(int clienteID, string sessionID,
            int precoID, int qtde, ref decimal valorPreco, bool transferencia, int serieID, EstruturaReservaInternet estruturaReservaInternet)
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
                Preco.InfoToReserva infoPreco = preco.LerToReserva(precoID, estruturaReservaInternet.CanalID, serieID, bd);

                if (infoPreco.Preco == null || (infoPreco.CanalPrecoID == 0 && serieID == 0 && !transferencia))
                    throw new ApplicationException("Este preço não está mais disponível!");
                else if (serieID > 0 && !infoPreco.DisponivelSerie)
                    throw new ApplicationException("Este preço não está mais disponível para esta série!");

                //Verifica se o cliente pode reservar esse preco
                bool precoOK = IngressoDisponivelPreco(clienteID, precoID, sessionID, bd);
                if (!precoOK)
                    throw new BilheteriaException("O preço selecionado não pode mais ser vendido <br /> O limite de venda por cliente foi excedido!");

                valorPreco = infoPreco.Preco.Valor.Valor;
                qtdePrecoDisponivel = infoPreco.Preco.Quantidade.Valor;
                qtdePrecoPorCliente = infoPreco.Preco.QuantidadePorCliente.Valor;
                qtdePrecoVendido = infoPreco.QtdVendidoPreco;
                qtdeCanalVendido = infoPreco.QtdVendidoCanal;
                //2. verificar a qtde total de preço independente do canal
                if (qtdePrecoDisponivel != 0 && !transferencia)
                {
                    //2.1 verificar qtde de ingressos vendidos a esse preço.
                    //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                    if (qtdePrecoVendido >= qtdePrecoDisponivel)
                    {
                        //codMsg = CodMensagem.PorPreco;
                        //alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Este preço não pode mais ser vendido. <br /> A Quantidade de venda foi excedida!");
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
                    throw new BilheteriaException("Data de validade para venda deste preço expirou!\n\nValidade: " + infoPreco.DataFim.ToShortDateString() + "\nHoje: " + System.DateTime.Now.ToShortDateString());
                }

                //3.1 se Quantidade for zero, esta liberado para vender a vontade!
                if (qtdePrecoPorCanal != 0 && !transferencia)
                {
                    //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender ingresso(s)
                    if (qtdeCanalVendido >= qtdePrecoPorCanal)
                    {
                        //codMsg = CodMensagem.PorCanal;
                        //alertaMsg = AlertaMensagem.Exclamacao;
                        throw new BilheteriaException("Este preço não está mais disponível para este canal. <br /> A Quantidade vendida foi excedida.");
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

        public int GetPacotesQPodeReservar(int pacoteID, int quantidade, string sessionID, int clienteID, EstruturaReservaInternet estruturaReservaInternet)
        {
            BD bd = new BD();
            try
            {
                //qtdes de pacote
                int qtdePacoteDisponivel = 0; //qtde de pacotes disponiveis, q podem vender, independente do canal
                int qtdePacotePorCanal = 0; //qtde de pacotes q podem vender por canal
                int qtdePacoteVendido = 0; //qtde de pacotes vendidos

                int qtdeMaxPacoteQPodeReservar = 0; //qtde maxima q pode reservar
                int qtdeMaxPacoteQPodeReservarPorCanal = 0; //qtde maxima q pode reservar por canal
                int qtdePacoteQPodeReservar = quantidade;

                //1. obter a Quantidade desse pacote
                Pacote pacote = new Pacote();
                Pacote.InfoToReserva infoPacote = pacote.LerToReserva(pacoteID, estruturaReservaInternet.CanalID, bd);

                if (infoPacote.Pacote.Control.ID == 0)
                    throw new BilheteriaException("Pacote não existe.");

                qtdePacoteDisponivel = infoPacote.Pacote.Quantidade.Valor;
                qtdePacotePorCanal = infoPacote.QtdDisponivelCanal;
                qtdePacoteVendido = infoPacote.QtdVendido;

                //2. verificar a qtde total de pacote independente do canal
                if (qtdePacoteDisponivel != 0)
                {
                    //2.1 verificar qtde de pacotes associados a esse preço.
                    //se qtde vendido for maior ou igual a qtde disponivel, nao pode vender
                    if (qtdePacoteVendido >= qtdePacoteDisponivel)
                        throw new BilheteriaException("A Quantidade de venda deste pacote foi excedida.");

                    qtdeMaxPacoteQPodeReservar = qtdePacoteDisponivel - qtdePacoteVendido;

                    if (qtdePacoteQPodeReservar > qtdeMaxPacoteQPodeReservar)
                        qtdePacoteQPodeReservar = qtdeMaxPacoteQPodeReservar;
                }

                //se Quantidade for zero, esta liberado para vender a vontade!
                if (qtdePacotePorCanal != 0)
                {
                    //se qtde vendido for maior ou igual a qtde disponivel no canal, nao pode vender
                    if (qtdePacoteVendido >= qtdePacotePorCanal)
                        throw new BilheteriaException("Este pacote não pode mais ser vendido pela Internet");

                    qtdeMaxPacoteQPodeReservarPorCanal = qtdePacotePorCanal - qtdePacoteVendido;

                    if (qtdePacoteQPodeReservar > qtdeMaxPacoteQPodeReservarPorCanal)
                        qtdePacoteQPodeReservar = qtdeMaxPacoteQPodeReservarPorCanal;
                }
                return qtdePacoteQPodeReservar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet MontaDadosVendaInternet(Dictionary<int, decimal> ingressosID, int clienteID, string sessionID, int canalID, int usuarioID)
        {

            /*
             * PRÓXIMA FASE*****
             * 
             *  1) Verificar quais colunas de itensReservados serão necessárias (retirar algumas da estrutura e dar valor a outras colunas.)
             *  2) Alterar no método de Venda
             */


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
            aux.Columns.Add("CodigoProgramacao", typeof(string));
            aux.Columns.Add("ApresentacaoSetorID", typeof(int));
            aux.Columns.Add("LugarID", typeof(int));
            aux.Columns.Add("SetorID", typeof(int));
            aux.Columns.Add("PrecoID", typeof(int));
            aux.Columns.Add("GerenciamentoIngressosID", typeof(int));
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
            aux.Columns.Add("CortesiaID", typeof(int));
            aux.Columns.Add("CodigoCinema", typeof(string));

            sql = @"SELECT DISTINCT
					tIngresso.ID as IngressoID,Codigo,tIngresso.EventoID, ApresentacaoID,tIngresso.ApresentacaoSetorID, SetorID, PrecoID,GerenciamentoIngressosID, PacoteID, LugarMarcado, LugarID,tPreco.Valor,TaxaConveniencia, PacoteGrupo,
					(tPreco.Valor * TaxaConveniencia) / 100 AS TaxaConvenienciaValor, tCanalEvento.TaxaMinima, tCanalEvento.TaxaMaxima,tCanalEvento.TaxaComissao,tCanalEvento.ComissaoMinima,tCanalEvento.ComissaoMaxima,(tPreco.Valor * TaxaComissao) / 100 AS ComissaoValor,
					tIngresso.CortesiaID, IsNull(ap.CodigoProgramacao, '') AS CodigoProgramacao, IsNull(tPreco.CodigoCinema, '') AS CodigoCinema
					FROM tIngresso (NOLOCK)
					INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                    INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = tIngresso.ApresentacaoID
					INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
					INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID
					WHERE ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "'  AND UsuarioID = " + usuarioID + " AND Status = 'R' AND tCanalEvento.CanalID = " + canalID + " ORDER BY IngressoID";

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
                dr["GerenciamentoIngressosID"] = bd.LerInt("GerenciamentoIngressosID");
                dr["PacoteID"] = bd.LerInt("PacoteID");
                dr["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                dr["LugarID"] = bd.LerInt("LugarID");
                dr["Codigo"] = bd.LerString("Codigo");
                dr["LugarMarcado"] = bd.LerString("LugarMarcado");
                decimal taxaConveniencia = bd.LerDecimal("TaxaConveniencia");
                decimal taxaComissao = bd.LerDecimal("TaxaComissao");
                dr["TaxaConveniencia"] = taxaConveniencia;
                dr["TaxaComissao"] = taxaComissao;
                dr["CortesiaID"] = bd.LerInt("CortesiaID");
                dr["TaxaConvenienciaValor"] = ingressosID[bd.LerInt("IngressoID")];
                dr["CodigoProgramacao"] = bd.LerString("CodigoProgramacao");
                dr["CodigoCinema"] = bd.LerString("CodigoCinema");

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
                    decimal ComissaoValor = bd.LerDecimal("ComissaoValor");
                    if (ComissaoValor * 100m > 1)
                        dr["ComissaoValor"] = ComissaoValor;
                    else
                        dr["ComissaoValor"] = 0.00;
                }

                dr["Valor"] = bd.LerDecimal("Valor");

                if (bd.LerInt("PacoteID") > 0)
                    pacotesID.Append(bd.LerInt("PacoteID").ToString() + ",");
            }

            bd.FecharConsulta();

            // verifica se a quantidade reservada pelo site é a mesma reservada no sistema
            if (aux.Rows.Count != ingressosID.Count)
                throw new BilheteriaException("1Quantidade de itens reservados pelo site é diferente dos reservados no sistema");

            for (int i = 0; i <= aux.Rows.Count - 1; i++)
            {
                if (!ingressosID.Contains(new KeyValuePair<int, decimal>(Convert.ToInt32(aux.Rows[i]["IngressoID"]), ingressosID[Convert.ToInt32(aux.Rows[i]["IngressoID"])])))
                    throw new BilheteriaException("2Quantidade de itens reservados pelo site é diferente dos reservados no sistema");
            }

            DataRow vendido = null;

            int reservaID = 1;
            DataRow linha = null;

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
                linha[TIPO_LUGAR] = ingressos[0]["LugarMarcado"];
                linha[LUGAR_MARCADO] = Convert.ToInt32(ingressos[0]["LugarID"]) > 0;
                linha[VALOR] = 0; // Será atualizada conforme ingressos.
                linha[APRESENTACAOSETORID] = ingressos[0]["ApresentacaoSetorID"];
                linha[APRESENTACAOID] = ingressos[0]["ApresentacaoID"];
                linha[PRECOID] = ingressos[0]["PrecoID"];


                Pacote oP = new Pacote();
                oP.Ler((int)ingressos[0]["PacoteID"]);
                linha[PERMITIR_CANCELAMENTO_AVULSO] = oP.PermitirCancelamentoAvulso.Valor ? true : false;

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
                    vendido[APRESENTACAOID] = ingresso["ApresentacaoID"];
                    vendido[APRESENTACAOSETORID] = ingresso["ApresentacaoSetorID"];
                    vendido[CORTESIAID] = ingresso["CortesiaID"];

                    linha["TaxaConvenienciaValor"] = (decimal)linha["TaxaConvenienciaValor"] + ingressosID[Convert.ToInt32(ingresso["IngressoID"])];

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

            #region Manipula mesas fechada
            DataTable mesasFechada = CTLib.TabelaMemoria.DistinctComFiltro(aux, "LugarMarcado = '" + Setor.MesaFechada + "'", "LugarMarcado", "LugarID");

            foreach (DataRow mesa in mesasFechada.Rows)
            {
                reservaID++;


                DataRow[] ingressos = aux.Select("LugarID = " + mesa["LugarID"]);

                linha = itensReservados.NewRow();

                linha[INGRESSOID] = ingressos[0]["IngressoID"];
                linha[RESERVAID] = reservaID;
                linha[CODIGO] = ingressos[0]["Codigo"];
                linha[CONV] = ingressos[0]["TaxaConveniencia"];
                linha["TaxaConvenienciaValor"] = 0; // Será atualizada conforme ingressos.
                linha[TIPO] = TIPO_INGRESSO;
                linha[TIPO_LUGAR] = ingressos[0]["LugarMarcado"];
                linha[LUGAR_MARCADO] = true;
                linha[VALOR] = 0; // Será atualizada conforme ingressos.
                linha[APRESENTACAOSETORID] = ingressos[0]["ApresentacaoSetorID"];
                linha[APRESENTACAOID] = ingressos[0]["ApresentacaoID"];
                linha[PRECOID] = ingressos[0]["PrecoID"];

                decimal comissaoMin = Convert.ToDecimal(ingressos[0]["ComissaoMinima"]);
                decimal comissaoMax = Convert.ToDecimal(ingressos[0]["ComissaoMaxima"]);
                int taxaComissao = (int)ingressos[0]["TaxaComissao"];
                linha[TAXA_COMISSAO] = taxaComissao;


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
                    vendido[CORTESIAID] = ingresso["CortesiaID"];

                    //Somatoria para total de conveniencia e total do valor dos ingressos
                    linha["TaxaConvenienciaValor"] = (decimal)linha["TaxaConvenienciaValor"] + (decimal)ingresso["TaxaConvenienciaValor"];
                    linha[VALOR] = (decimal)linha[VALOR] + (decimal)ingresso["Valor"];

                    decimal dAux = (taxaComissao / 100m) * (decimal)linha[VALOR];

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
                        linha["ComissaoValor"] = Convert.ToDecimal(linha["ComissaoValor"]) + Convert.ToDecimal(ingresso["ComissaoValor"]);
                    }



                    ingressosVendidos.Rows.Add(vendido);

                    aux.Rows.Remove(ingresso); // Remove a linha do datatable.
                }
            }
            #endregion

            #region Manipula o restante
            DataRow reservado = null;
            foreach (DataRow resto in aux.Rows)
            {
                reservaID++;
                reservado = itensReservados.NewRow();
                vendido = ingressosVendidos.NewRow();

                reservado[INGRESSOID] = resto["IngressoID"];
                reservado[RESERVAID] = reservaID;
                reservado[CODIGO] = resto["Codigo"];
                reservado[VALOR] = resto["Valor"];
                reservado[CONV] = resto["TaxaConveniencia"];
                reservado["TaxaConvenienciaValor"] = resto["TaxaConvenienciaValor"];
                reservado[TAXA_COMISSAO] = resto["TaxaComissao"];
                reservado[COMISSAO_VALOR] = resto["ComissaoValor"];
                reservado[TIPO_LUGAR] = resto["LugarMarcado"];
                if (Convert.ToInt32(resto["LugarID"]) > 0)
                    reservado[LUGAR_MARCADO] = true;
                else
                    reservado[LUGAR_MARCADO] = false;

                reservado[APRESENTACAOSETORID] = resto["ApresentacaoSetorID"];
                reservado[APRESENTACAOID] = resto["ApresentacaoID"];
                reservado[PRECOID] = resto["PrecoID"];
                reservado[GERENCIAMENTO_INGRESSOS_ID] = resto["GerenciamentoIngressosID"];
                reservado[TIPO] = TIPO_INGRESSO;
                reservado["CodigoProgramacao"] = resto["CodigoProgramacao"];
                reservado["CodigoCinema"] = resto["CodigoCinema"];

                itensReservados.Rows.Add(reservado);

                vendido[RESERVAID] = reservaID;
                vendido[TIPO] = TIPO_INGRESSO;
                vendido[LUGARID] = resto["LugarID"];
                vendido[PACOTEID] = resto["PacoteID"];
                vendido[INGRESSOID] = resto["IngressoID"];
                vendido[PRECOID] = resto["PrecoID"];
                vendido[GERENCIAMENTO_INGRESSOS_ID] = resto["GerenciamentoIngressosID"];
                vendido[CORTESIAID] = resto[CORTESIAID];


                ingressosVendidos.Rows.Add(vendido);
            }
            #endregion

            return buffer;


        }

        public ArrayList QuantidadeSetor()
        {
            ArrayList qtds = new ArrayList();
            int[] x = new int[3];



            SqlConnection connection = null;
            SqlCommand comm = null;

            BD bd = new BD();

            try
            {
                connection = (SqlConnection)bd.Cnn;
                comm = connection.CreateCommand();
                comm.CommandTimeout = 600;
                comm.CommandText = @"SELECT 
									i.ApresentacaoID, i.SetorID, COUNT(i.ID) AS QtdDisponivel
									FROM 
									internet_setores a, tIngresso i(NOLOCK)
									WHERE
									i.ApresentacaoID = a.ApresentacaoID AND i.SetorID = a.SetorID AND Status = 'D'
									GROUP BY i.ApresentacaoID, i.SetorID";
                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                    qtds.Add(new int[] { dr.GetInt32(0), dr.GetInt32(1), dr.GetInt32(2) });

                dr.Close();
                connection.Close();
                connection.Dispose();
                return qtds;
            }
            catch (Exception)
            {
                return qtds;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }

        public bool CancelarTodasReservasVIRInternet(int[] virID)
        {
            try
            {
                ValeIngresso oValeIngresso = new ValeIngresso();
                return oValeIngresso.CancelarReservas(virID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate bool EnvioCelularHandler(string celular, string senha, int vendaBilheteriaID);

        private string[] MontaRetornoVendaInternet(string msgCodigo, string senha, long vendaBilheteriaID, string msgCelular, string risco)
        {
            return new string[] { msgCodigo, senha, vendaBilheteriaID.ToString(), msgCelular, risco };
        }

        private string[] MontaRetornoVendaInternet(string msgCodigo, string senha, long vendaBilheteriaID, string msgCelular)
        {
            return new string[] { msgCodigo, senha, vendaBilheteriaID.ToString(), msgCelular };
        }

        public bool MudarPreco(ref List<Ingresso> ingressos, EstruturaPrecoReservaSite preco, EstruturaReservaInternet estruturaReservaInternet, int serieID)
        {
            BD bd = new BD();
            try
            {
                BilheteriaParalela bilheteria = new BilheteriaParalela();

                int apresentacaoSetorID = 0;
                Setor.enumLugarMarcado tipoSetor = new Setor.enumLugarMarcado();
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();

                //Busca a ApresentacaoSetorID, já aproveita e busca o tipo do setor, Cota ID da Apresentacao e da ApresentacaoSetor
                string sqlProc = "EXEC sp_getApresentacaoInfo " + ingressos[0].ApresentacaoID.Valor + ", " + ingressos[0].SetorID.Valor;

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

                ingressos[0].ApresentacaoSetorID.Valor = apresentacaoSetorID;
                ingressos[0].UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                ingressos[0].LojaID.Valor = estruturaReservaInternet.LojaID;

                ingressos[0].ValidarLugarMarcadoInternet(preco, tipoSetor, cotaInfo, serieID, estruturaReservaInternet);

                bd.IniciarTransacao();

                foreach (Ingresso ingresso in ingressos)
                {
                    ingresso.CotaItemID = ingressos[0].CotaItemID;
                    ingresso.CotaItemIDAPS = ingressos[0].CotaItemIDAPS;

                    if (!ingresso.MudarPreco(bd, preco.ID))
                        throw new Exception("Não foi possível atualizar o Preço do ingresso.");
                }
                bd.FinalizarTransacao();
                return true;
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

        public bool EnviarCelular(string celular, string senha, int vendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {
                //Encontra as formas de pagamento
                StringBuilder stbPagamento = new StringBuilder();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT tFormaPagamento.Nome FROM tVendaBilheteriaFormaPagamento (NOLOCK) ");
                stbSQL.Append("INNER JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID ");
                stbSQL.Append("WHERE tVendaBilheteriaFormaPagamento.VendaBilheteriaID = " + vendaBilheteriaID);

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    stbPagamento.Append(bd.LerString("Nome"));
                    stbPagamento.Append(", ");
                }
                if (stbPagamento.Length > 2)
                    stbPagamento.Remove(stbPagamento.Length - 2, 2);

                bd.FecharConsulta();

                //Retorna o resumo da compra preenchendo o obj de entrega via celular
                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT i.ID,i.Codigo, tCliente.NomeEntrega, tCliente.CPFEntrega, tLoja.Nome as Loja, tPreco.Nome AS Preco, ");
                stbSQL.Append("tSetor.Nome AS Setor, tApresentacao.Horario,tEvento.ID AS EventoID, tEvento.Nome AS Evento, tLocal.Nome AS Local, tPacote.Nome AS Pacote ");
                stbSQL.Append("FROM tIngresso i (NOLOCK) ");
                stbSQL.Append("INNER JOIN tCliente (NOLOCK) ON i.ClienteID = tCliente.ID ");
                stbSQL.Append("INNER JOIN tLoja (NOLOCK) ON i.LojaID = tLoja.ID ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON i.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                stbSQL.Append("INNER JOIN tPreco (NOLOCK) ON i.PrecoID = tPreco.ID ");
                stbSQL.Append("INNER JOIN tSetor (NOLOCK) ON i.SetorID = tSetor.ID ");
                stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON i.ApresentacaoID = tApresentacao.ID ");
                stbSQL.Append("INNER JOIN tEvento (NOLOCK) ON i.EventoID = tEvento.ID ");
                stbSQL.Append("INNER JOIN tLocal (NOLOCK) ON i.LocalID = tLocal.ID ");
                stbSQL.Append("LEFT JOIN tPacote (NOLOCK) ON i.PacoteID = tPacote.ID ");
                stbSQL.Append("WHERE i.VendaBilheteriaID = " + vendaBilheteriaID);
                stbSQL.Append(" ORDER BY Evento, Horario, i.ID");

                List<EstruturaEntregaCelular> listaEstrutura = new List<EstruturaEntregaCelular>();
                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    EstruturaEntregaCelular oEstrutura = new EstruturaEntregaCelular();
                    oEstrutura.IngressoID = bd.LerLong("ID");
                    oEstrutura.Codigo = bd.LerString("Codigo");
                    oEstrutura.Nome = bd.LerString("NomeEntrega");
                    oEstrutura.CPF = bd.LerLong("CPFEntrega");
                    oEstrutura.Loja = bd.LerString("Loja");
                    oEstrutura.PrecoNome = bd.LerString("Preco");
                    oEstrutura.Setor = bd.LerString("Setor");
                    oEstrutura.Data = bd.LerDateTime("Horario");
                    oEstrutura.EventoID = bd.LerInt("EventoID");
                    oEstrutura.Evento = bd.LerString("Evento");
                    oEstrutura.Local = bd.LerString("Local");
                    oEstrutura.Pacote = bd.LerString("Pacote");
                    oEstrutura.Pagamento = stbPagamento.ToString();
                    oEstrutura.Senha = senha;
                    listaEstrutura.Add(oEstrutura);
                }
                EntregaCelular oEntrega = new EntregaCelular();

                oEntrega.SenhaVenda = senha;
                oEntrega.ListaEntrega = listaEstrutura;
                oEntrega.Celular = celular;

                return oEntrega.enviarTickets();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.FecharConsulta();
                bd.Fechar();
            }

        }

        public string[] VenderVIRInternet(EstruturaPagamento oPagamento, int[] virsID, string sessionID, List<EstruturaVirNomePresenteado> lstPresenteado, EstruturaReservaInternet estruturaReservaInternet, string celular)
        {

            BD bd = new BD();
            decimal valorTotal = 0;
            string sql = string.Empty;
            ValeIngressoLog valeIngressoLog;

            bool vendaAsync = Convert.ToBoolean(ConfigurationManager.AppSettings["VendaAsync"]);
            string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
            string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
            string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
            bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

            bool ProcessouSitef = false;
            bool ProcessouAdyen = false;
            bool ProcessouPayPal = false;

            try
            {
                Cartao oCartao = new Cartao();
                AntiFraudeMotivo motivo = new AntiFraudeMotivo();
                List<EstruturaRetornoReservaValeIngresso> oReservados = new List<EstruturaRetornoReservaValeIngresso>();
                ValeIngresso oValeIngresso = new ValeIngresso();
                FormaPagamento formaPagamento = new FormaPagamento();
                oReservados = oValeIngresso.ListaReservados(oPagamento.ClienteID, sessionID);

                //caixa para registrar a venda
                int caixaID = 0;
                if (estruturaReservaInternet.CaixaID == 0)
                    caixaID = VerificaCaixaInternet();
                else
                    caixaID = estruturaReservaInternet.CaixaID;

                int empresaID = new Canal().BuscaEmpresaIDporLojaID(estruturaReservaInternet.LojaID);

                //verifica se a quantidade reservada no site é a mesma reservada no sistema
                if (oReservados.Count != virsID.Length)
                    throw new BilheteriaException("Quantidade de VIRs diferente na reserva.", CodMensagemVenda.FalhaVenda);

                for (int i = 0; i < oReservados.Count; i++)
                    valorTotal += oReservados[i].Valor;

                valorTotal += oPagamento.EntregaValor;

                bd.IniciarTransacao();

                #region VendaBilheteria

                int entregaControleID = oPagamento.EntregaControleID;
                int entregaAgendaID = 0;
                int pdvID = oPagamento.PdvID;
                int clienteEnderecoID = oPagamento.EnderecoClienteID;

                EstruturaEntregaAgenda entregaSelecionada = new EstruturaEntregaAgenda();

                EntregaAgenda oEA = new EntregaAgenda();

                entregaSelecionada = oEA.CarregarEntrega(entregaControleID, oPagamento.DataSelecionada);

                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    string cmd = oEA.String();
                    verifAgendado = bd.Executar(cmd);

                    if (verifAgendado == null)
                        throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case "A":

                            throw new BilheteriaException("Venda não foi gerada.");

                        default:
                            break;
                    }
                }

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                vendaBilheteria.ClienteID.Valor = oPagamento.ClienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;

                if (!string.IsNullOrEmpty(celular))
                {
                    string DDD = celular.Substring(0, 2);
                    string numeroCelular = celular.Substring(2, celular.Length - 2);

                    vendaBilheteria.NumeroCelular.Valor = Convert.ToInt32(numeroCelular);
                    vendaBilheteria.DDD.Valor = Convert.ToInt32(DDD);
                }

                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.TaxaEntregaValor.Valor = oPagamento.EntregaValor;

                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = 0;
                vendaBilheteria.ComissaoValorTotal.Valor = 0;
                vendaBilheteria.PagamentoProcessado.Valor = true;

                switch (oPagamento.TipoPagamento)
                {
                    case EstruturaPagamento.enumTipoPagamento.TEF:
                        ProcessouSitef = this.EfetuaPagamentoTef(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Adyen:
                        ProcessouAdyen = this.EfetuaPagamentoAdyen(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Paypal:
                        DoExpressCheckoutPaymentOperation DoCheckOut = PayPalApiFactory.instance.ExpressCheckout(
                                Usuario,
                                Senha,
                                Assinatura
                            ).DoExpressCheckoutPayment(oPagamento.Token, oPagamento.PayerID, (double)oPagamento.ValorTotal, PaymentAction.SALE);

                        DoCheckOut.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                        DoCheckOut.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                        if (AmbienteTestePayPal)
                            DoCheckOut.sandbox().execute();
                        else
                            DoCheckOut.execute();

                        if (DoCheckOut.ResponseNVP.Ack != Ack.SUCCESS)
                            throw new Exception("Não foi possível evetuar o pagamento através do PayPal");
                        else
                        {
                            oPagamento.TransactionID = DoCheckOut.ResponseNVP.TransactionId;
                            oPagamento.CorrelationID = DoCheckOut.ResponseNVP.CorrelationId;
                        }
                        vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalPaypal;
                        ProcessouPayPal = true;
                        break;
                }

                if (oPagamento.IniciouTef)
                    vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.oSitef.CupomFiscal;
                else if (oPagamento.IniciouAdyen)
                {
                    vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalAdyen;
                    vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                }
                else
                    vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalPaypal;

                if (oPagamento.oAdyen.Fraud)
                    vendaBilheteria.AccertifyForceStatus.Valor = ((char)VendaBilheteria.enumForceStatus.Review).ToString();

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                int vendaID = Convert.ToInt32(bd.ConsultaValor(sqlVendaBilheteria));
                vendaBilheteria.Control.ID = vendaID;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                #endregion

                string validade = string.Empty;
                #region VendaBilheteriaItem, ValeIngresso e ValeIngressoLog

                foreach (EstruturaRetornoReservaValeIngresso reservado in oReservados)
                {
                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                    vendaBilheteriaItem.TaxaConveniencia.Valor = 0;
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = 0;
                    vendaBilheteriaItem.TaxaComissao.Valor = 0;
                    vendaBilheteriaItem.ComissaoValor.Valor = 0;

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object itemID = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (itemID != null) ? Convert.ToInt32(itemID) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de Venda não foi gerado.");

                    //UPDATE Na tValeIngresso

                    //VALIDADE
                    if (reservado.ValidadeDias > 0)//calcula a validade
                        validade = string.Empty;
                    else
                        validade = reservado.Validade.ToString("yyyyMMdd");

                    string procSQL = @"EXEC dbo.RegistrarVendatValeIngresso 
											   @CodigoBarra = '' , @LojaID = " + estruturaReservaInternet.LojaID + ", @VendaBilheteriaID = " +
                                           vendaBilheteria.Control.ID +
                                           " , @Status = '" + ValeIngresso.enumStatus.Vendido + "', @ClienteID = " + oPagamento.ClienteID + ", @UsuarioID = " + estruturaReservaInternet.UsuarioID +
                                           " , @ValeIngressoID = " + reservado.ID + ", @ClienteNome = '' , @DataExpiracao = '" + validade + "'" +
                                           " , @CodigoTroca = '" + reservado.CodigoTroca + "'";

                    oValeIngresso.Control.ID = reservado.ID;
                    oValeIngresso.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    //string sqlAlterarStatusVIR = oValeIngresso.StringAtualizarVendaValeIngresso();

                    if (bd.Executar(procSQL) < 1)
                        throw new BilheteriaException("Ocorreu um erro ao efetuar a venda do vale ingresso.");

                    EstruturaVirNomePresenteado IDNome = lstPresenteado.Where(c => c.ID == reservado.ID).FirstOrDefault();

                    sql = "UPDATE tValeIngresso SET ClienteNome = '" + IDNome.NomePresenteado + "' WHERE tValeIngresso.ID = " + reservado.ID;
                    bd.Executar(sql);

                    //VALE INGRESSO LOG
                    valeIngressoLog = new ValeIngressoLog();
                    valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Vender).ToString();
                    valeIngressoLog.TimeStamp.Valor = DateTime.Now;
                    valeIngressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                    valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    valeIngressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    valeIngressoLog.EmpresaID.Valor = empresaID;
                    valeIngressoLog.CaixaID.Valor = caixaID;
                    valeIngressoLog.LojaID.Valor = estruturaReservaInternet.LojaID;
                    valeIngressoLog.CanalID.Valor = estruturaReservaInternet.CanalID;
                    valeIngressoLog.ValeIngressoID.Valor = reservado.ID;
                    //Código de troca fixo?
                    if (reservado.CodigoTroca.Trim().Length > 0)
                        valeIngressoLog.CodigoTroca.Valor = reservado.CodigoTroca;
                    else
                        valeIngressoLog.CodigoTroca.Valor = string.Empty;

                    //Insere o registro de venda
                    string registroInserido = valeIngressoLog.StringInserir();
                    if (bd.Executar(registroInserido) != 1)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.");
                }
                #endregion

                #region Forma de Pagamento
                VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                EstruturaVendaFormaPagamento oFormaPagamento = formaPagamento.getFormaPagamentoVenda(oPagamento.FormaPagamentoID, empresaID);

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.CartaoID == 0)
                    {
                        if (oPagamento.oSitef != null)
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.CodigoSeguranca, oPagamento.oSitef.DataVencimento, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                        else
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.oAdyen.DataValidadeCartao, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                    }
                    else
                    {
                        if (oPagamento.oSitef != null)
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.DataVencimento, oPagamento.oSitef.CodigoSeguranca, oPagamento.NomeCartao);
                        else
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.DataValidadeCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.NomeCartao);
                    }
                }

                vendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                vendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                vendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                vendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);

                vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oPagamento.FormaPagamentoID;
                vendaBilheteriaFormaPagamento.Valor.Valor = valorTotal;
                decimal porcentagemTotal = 100;
                vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagemTotal;
                vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                vendaBilheteriaFormaPagamento.CartaoID.Valor = oPagamento.CartaoID;
                vendaBilheteriaFormaPagamento.JurosValor.Valor = oPagamento.JurosValor;
                vendaBilheteriaFormaPagamento.Coeficiente.Valor = oPagamento.Coeficiente;

                switch (oPagamento.TipoPagamento)
                {
                    case EstruturaPagamento.enumTipoPagamento.TEF:

                        vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oPagamento.oSitef.ID;
                        vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oSitef.CodigoRespostaSitefVenda;
                        vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oPagamento.oSitef.MensagemFinaliza;
                        vendaBilheteriaFormaPagamento.HoraTransacao.Valor = oPagamento.oSitef.HoraTransacao;
                        vendaBilheteriaFormaPagamento.DataTransacao.Valor = oPagamento.oSitef.DataTransacao;
                        vendaBilheteriaFormaPagamento.CodigoIR.Valor = oPagamento.oSitef.CodigoIR;
                        vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oSitef.NumeroAutorizacao;
                        vendaBilheteriaFormaPagamento.NSUHost.Valor = oPagamento.oSitef.NSUHost;
                        vendaBilheteriaFormaPagamento.NSUSitef.Valor = oPagamento.oSitef.NSUSitef;
                        vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.oSitef.CupomFiscal;
                        vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oPagamento.oSitef.DadosConfirmacao;
                        vendaBilheteriaFormaPagamento.Rede.Valor = oPagamento.oSitef.RedeRetorno;
                        vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oPagamento.oSitef.CodigoRespostaSitefFinaliza;
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Adyen:
                        vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oAdyen.CodigoAutenticacao;
                        vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oAdyen.CodigoReferencia;
                        vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Paypal:
                        vendaBilheteriaFormaPagamento.TokenPayPal.Valor = oPagamento.Token;
                        vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor = oPagamento.PayerID;
                        vendaBilheteriaFormaPagamento.TransactionID.Valor = oPagamento.TransactionID;
                        vendaBilheteriaFormaPagamento.CorrelationID.Valor = oPagamento.CorrelationID;
                        vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalPaypal;
                        break;

                }

                string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();

                int y = bd.Executar(sqlVendaBilheteriaFormaPagamento);

                bool p = (y >= 1);

                if (!p)
                    throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);
                #endregion

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.IniciouTef)
                    {
                        if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                    else
                    {
                        if (!oPagamento.oAdyen.CapturarPagamento())
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                }

                bd.FinalizarTransacao();

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                string senha = (ret != null) ? Convert.ToString(ret) : null;

                try
                {
                    if (celular.Length > 0 && vendaBilheteria.Status.Valor == (string)VendaBilheteria.PAGO && vendaBilheteria.PagamentoProcessado.Valor)
                        new EnviaSMS().EnviaSms(true, vendaBilheteria.Control.ID, celular);
                }
                catch (Exception)
                {
                }

                return MontaRetornoVendaInternet(((int)CodMensagemVenda.Sucesso).ToString(), senha, vendaBilheteria.Control.ID, string.Empty);

            }
            catch (BilheteriaException ex)
            {
                bd.DesfazerTransacao();

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF && ProcessouSitef)
                {
                    if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");
                }
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Paypal && ProcessouPayPal)
                    this.Refund(Usuario, Senha, Assinatura, oPagamento.TransactionID, AmbienteTestePayPal);
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen && ProcessouAdyen)
                    oPagamento.oAdyen.CancelarPagamento();

                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string[] VenderWeb(EstruturaPagamento oPagamento, Dictionary<int, decimal> ingressosID, string sessionID, List<int> valeIngresso, string celular,
             List<EstruturaDonoIngresso> listaDonoIngresso, EstruturaReservaInternet estruturaReservaInternet, decimal ValorTotalSeguro, decimal ValorTotalTaxaProcessamento, Cliente oCliente)
        {
            string[] retorno = new string[2];
            try
            {
                oPagamento.ClienteEmail = oCliente.Email.Valor;
                oPagamento.ClienteNome = oCliente.Nome.Valor;
                oPagamento.SessionID = sessionID;

                retorno = VenderInternet(oPagamento, ingressosID, sessionID, valeIngresso, celular, listaDonoIngresso, estruturaReservaInternet, ValorTotalSeguro, ValorTotalTaxaProcessamento, oCliente);
            }
            catch (BilheteriaException ex)
            {
                retorno[0] = ((int)ex.CodigoErro).ToString();
                retorno[1] = ex.Message;
            }
            catch (Exception ex)
            {
                retorno[0] = ((int)BilheteriaParalela.CodMensagemVenda.ErroIndefinido).ToString();
                retorno[1] = ex.Message;
            }

            return retorno;
        }

        public string[] VenderVIRWeb(EstruturaPagamento oPagamento, int[] virsID, string sessionID, List<EstruturaVirNomePresenteado> lstPresenteado, EstruturaReservaInternet estruturaReservaInternet, string celular, Cliente oCliente)
        {
            string[] retorno = new string[2];
            try
            {
                oPagamento.ClienteEmail = oCliente.Email.Valor;
                oPagamento.ClienteNome = oCliente.Nome.Valor;
                oPagamento.SessionID = sessionID;

                //verifica se a taxa de entrega existe
                EntregaControle oEC = new EntregaControle();
                if (!oEC.Existe(oPagamento.EntregaControleID))
                {
                    retorno[0] = ((int)BilheteriaParalela.CodMensagemVenda.TaxaEntregaInexistente).ToString();
                    retorno[1] = string.Empty;
                    return retorno;
                }

                retorno = VenderVIRInternet(oPagamento, virsID, sessionID, lstPresenteado, estruturaReservaInternet, celular);
            }
            catch (BilheteriaException ex)
            {
                retorno[0] = ((int)ex.CodigoErro).ToString();
                retorno[1] = ex.Message;
            }
            catch (Exception ex)
            {
                retorno[0] = ((int)BilheteriaParalela.CodMensagemVenda.ErroIndefinido).ToString();
                retorno[1] = ex.Message;
            }

            return retorno;
        }

        public string[] VenderInternet(EstruturaPagamento oPagamento, Dictionary<int, decimal> ingressosID, string sessionID, List<int> valeIngresso,
            string celular, List<EstruturaDonoIngresso> listaDonoIngresso, EstruturaReservaInternet estruturaReservaInternet, decimal ValorTotalSeguro, decimal valorTotalTaxaProcessamento, Cliente oClienteSelecionado)
        {
            BD bd = new BD();
            decimal valorTotalIngresso = 0;
            decimal valorTotalConveniencia = 0;
            decimal valorTotalComissao = 0;
            decimal valorTotal = 0;
            string sql = string.Empty;

            bool vendaAsync = Convert.ToBoolean(ConfigurationManager.AppSettings["VendaAsync"]);
            string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
            string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
            string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
            bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

            bool ProcessouCinema = false;
            bool ProcessouSitef = false;
            bool ProcessouAdyen = false;
            bool ProcessouPayPal = false;

            DataRow[] cinema = null;
            string senha = string.Empty;
            int vendaBilheteriaID = 0;

            try
            {
                Cartao oCartao = new Cartao();

                StringBuilder stbSQL = new StringBuilder();
                //caixa para registrar a venda
                int caixaID = 0;
                if (estruturaReservaInternet.CaixaID == 0)
                    caixaID = VerificaCaixaInternet();
                else
                    //Verificar o caixa do usuario.
                    caixaID = estruturaReservaInternet.CaixaID;

                if (oPagamento.ClienteID == 0)
                    oPagamento.ClienteID = oClienteSelecionado.Control.ID;

                ValeIngresso oValeIngresso = new ValeIngresso();
                FormaPagamento formaPagamento = new FormaPagamento();
                CotaItemControle oCotaItemControle = new CotaItemControle();
                IngressoCliente oIngressoCliente = new IngressoCliente();
                //busca os dados da reserva para determinado cliente
                DataSet ds = MontaDadosVendaInternet(ingressosID, oPagamento.ClienteID, sessionID, estruturaReservaInternet.CanalID, estruturaReservaInternet.UsuarioID);

                DataTable itensReservados = ds.Tables[TABELA_GRID]; // Resumo
                DataTable ingressosVendidos = ds.Tables[TABELA_RESERVA]; // Detalhamento

                //verifica se a quantidade reservada no site é a mesma reservada no sistema
                if (ingressosVendidos.Rows.Count != ingressosID.Count)
                    throw new BilheteriaException("Quantidade de ingressos diferente da reserva.", CodMensagemVenda.FalhaVenda);

                bd.IniciarTransacao();

                //verifica se existem reservas
                if (itensReservados.Rows.Count == 0 && ingressosVendidos.Rows.Count == 0)
                    throw new BilheteriaException("Não existe reserva para o cliente.", CodMensagemVenda.ReservaInexistente);

                // VendaBilheteria
                int entregaControleID = oPagamento.EntregaControleID;
                int entregaAgendaID = 0;
                int pdvID = oPagamento.PdvID;
                int clienteEnderecoID = oPagamento.EnderecoClienteID;
                int empresaID = new Canal().BuscaEmpresaIDporLojaID(estruturaReservaInternet.LojaID);
                EstruturaEntregaAgenda entregaSelecionada = new EstruturaEntregaAgenda();
                EntregaAgenda oEA = new EntregaAgenda();

                entregaSelecionada = oEA.CarregarEntrega(entregaControleID, oPagamento.DataSelecionada);

                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    string cmd = oEA.String();
                    verifAgendado = bd.Executar(cmd);

                    if (verifAgendado == null)
                        throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");

                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case Entrega.AGENDADA:
                            throw new BilheteriaException("Venda não foi gerada.");
                        default:
                            break;
                    }
                }

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                if (!string.IsNullOrEmpty(celular))
                {
                    string DDD = celular.Substring(0, 2);
                    string numeroCelular = celular.Substring(2, celular.Length - 2);

                    vendaBilheteria.NumeroCelular.Valor = Convert.ToInt32(numeroCelular);
                    vendaBilheteria.DDD.Valor = Convert.ToInt32(DDD);
                }
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;

                vendaBilheteria.ClienteID.Valor = oPagamento.ClienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;

                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.TaxaEntregaValor.Valor = oPagamento.EntregaValor;
                vendaBilheteria.PagamentoProcessado.Valor = true;


                //valor total
                object valorAuxiliar = null;
                object valorAuxiliarComissao = null;
                valorAuxiliar = itensReservados.Compute("SUM(Valor)", "");
                valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                valorAuxiliar = itensReservados.Compute("SUM(TaxaConvenienciaValor)", "");
                valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                valorAuxiliarComissao = itensReservados.Compute("SUM(ComissaoValor)", "");
                valorTotalComissao = valorAuxiliarComissao == System.DBNull.Value ? 0 : (decimal)valorAuxiliarComissao;

                valorTotal = valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor + valorTotalTaxaProcessamento;
                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = valorTotalConveniencia;
                vendaBilheteria.ComissaoValorTotal.Valor = valorTotalComissao;
                vendaBilheteria.ValorSeguro.Valor = ValorTotalSeguro;
                vendaBilheteria.TaxaProcessamentoValor.Valor = valorTotalTaxaProcessamento;
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.IP.Valor = oPagamento.IP;
                vendaBilheteria.QuantidadeImpressoesInternet.Valor = 0;
                vendaBilheteria.NomeCartao.Valor = oPagamento.NomeCartao ?? string.Empty;

                switch (oPagamento.TipoPagamento)
                {
                    case EstruturaPagamento.enumTipoPagamento.TEF:
                        ProcessouSitef = this.EfetuaPagamentoTef(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Adyen:
                        ProcessouAdyen = this.EfetuaPagamentoAdyen(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Paypal:
                        DoExpressCheckoutPaymentOperation DoCheckOut = PayPalApiFactory.instance.ExpressCheckout(
                                Usuario,
                                Senha,
                                Assinatura
                            ).DoExpressCheckoutPayment(oPagamento.Token, oPagamento.PayerID, (double)oPagamento.ValorTotal, PaymentAction.SALE);

                        DoCheckOut.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                        DoCheckOut.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                        if (AmbienteTestePayPal)
                            DoCheckOut.sandbox().execute();
                        else
                            DoCheckOut.execute();

                        if (DoCheckOut.ResponseNVP.Ack != Ack.SUCCESS)
                            throw new Exception("Não foi possível evetuar o pagamento através do PayPal");
                        else
                        {
                            oPagamento.TransactionID = DoCheckOut.ResponseNVP.TransactionId;
                            oPagamento.CorrelationID = DoCheckOut.ResponseNVP.CorrelationId;
                        }
                        vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalPaypal;
                        ProcessouPayPal = true;
                        break;
                    default:
                        break;
                }

                if (oPagamento.IniciouTef)
                    vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.oSitef.CupomFiscal;
                else if (oPagamento.IniciouAdyen)
                {
                    vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalAdyen;
                    vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                }
                else
                {
                    if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Paypal)
                        vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalPaypal;
                    else
                        vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = "";
                }

                if (oPagamento.oAdyen.Fraud)
                    vendaBilheteria.AccertifyForceStatus.Valor = ((char)VendaBilheteria.enumForceStatus.Review).ToString();

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                senha = (ret != null) ? Convert.ToString(ret) : null;
                vendaBilheteriaID = vendaBilheteria.Control.ID;

                DataRow[] itensNormais = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                DataRow[] itensMesaFechada = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                DataRow[] itensPacote = itensReservados.Select(TIPO + "='" + TIPO_PACOTE + "'");

                #region Ingressos Normais
                foreach (DataRow item in itensNormais)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    int x = 0;
                    bool ok = false;
                    stbSQL = new StringBuilder();


                    stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                    stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                    stbSQL.Append("WHERE(AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)item["IngressoID"]);
                    x = bd.Executar(stbSQL.ToString());
                    ok = (x >= 1);

                    if (!ok)
                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                    EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)item[INGRESSOID]).FirstOrDefault();
                    if (dono != null)
                    {
                        oIngressoCliente.ApresentacaoID.Valor = (int)item[APRESENTACAOID];
                        oIngressoCliente.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                        oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                        oIngressoCliente.DonoID.Valor = dono.DonoID;
                        oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                        oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                        oIngressoCliente.CPF.Valor = dono.CPF;

                        bd.Executar(oIngressoCliente.StringInserir());

                        oCotaItemControle.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                        oCotaItemControle.ApresentacaoID.Valor = (int)item[APRESENTACAOID];

                        if (dono.CotaItemIDAPS > 0)
                        {
                            oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                            bd.Executar(oCotaItemControle.StringAtualizarAPS());

                            stbSQL = new StringBuilder();
                            stbSQL.Append("SELECT ");
                            stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                            stbSQL.Append("THEN 0 ");
                            stbSQL.Append("ELSE 1 ");
                            stbSQL.Append("END AS Valido ");
                            stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                            stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, item[APRESENTACAOSETORID]);

                            if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
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
                            stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, item[APRESENTACAOID]);

                            if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                        }
                    }

                    //inserir na Log
                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)item[INGRESSOID];
                    ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario fixo para Internet
                    ingressoLog.BloqueioID.Valor = 0;
                    ingressoLog.CortesiaID.Valor = Convert.ToInt32(ingressos[0][CORTESIAID]);
                    ingressoLog.PrecoID.Valor = (int)item[PRECOID];
                    ingressoLog.GerenciamentoIngressosID.Valor = (int)item[GERENCIAMENTO_INGRESSOS_ID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    ingressoLog.CaixaID.Valor = caixaID; // ABERTURA E FECHAMENTO DE CAIXA DIARIO PARA INTERNET
                    ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja fixa Internet
                    ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal fixo Internet
                    ingressoLog.EmpresaID.Valor = empresaID; // FIXO IR
                    ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                    ingressoLog.Acao.Valor = IngressoLog.VENDER;
                    string sqlIngressoLogV = ingressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                }
                #endregion

                #region Mesa Fechada
                foreach (DataRow item in itensMesaFechada)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    foreach (DataRow i in ingressos)
                    {
                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);

                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                        EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)i[INGRESSOID]).FirstOrDefault();
                        if (dono != null)
                        {
                            oIngressoCliente.ApresentacaoID.Valor = (int)item[APRESENTACAOID];
                            oIngressoCliente.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            oCotaItemControle.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                            oCotaItemControle.ApresentacaoID.Valor = (int)item[APRESENTACAOID];

                            if (dono.CotaItemIDAPS > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, item[APRESENTACAOSETORID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                                //}
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
                                stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, item[APRESENTACAOID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                            }

                        }

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //USUARIO INTERNET
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //CAIXA ABERTO DIARIAMENTE PARA INTERNET
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //Loja Internet
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //Canal Internet
                        ingressoLog.EmpresaID.Valor = empresaID; //Empresa IR
                        ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                #region Pacote
                int PacoteGrupo = 0;
                int pacoteIDAtual = 0;
                foreach (DataRow item in itensPacote)
                {

                    DataRow[] ingressosPacote = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    Pacote oPacote = new Pacote();
                    oPacote.Ler((int)ingressosPacote[0][PACOTEID]);

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = oPacote.Control.ID;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;


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
                    decimal valorConv = (decimal)item["TaxaConvenienciaValor"];
                    int taxaComissao = (int)item[TAXA_COMISSAO];
                    decimal comissaovalor = (decimal)item[COMISSAO_VALOR];

                    if (!(bool)item["PermitirCancelamentoAvulso"])
                    {
                        vendaBilheteriaItem.TaxaConveniencia.Valor = conv;
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = valorConv;
                        vendaBilheteriaItem.TaxaComissao.Valor = taxaComissao;
                        vendaBilheteriaItem.ComissaoValor.Valor = comissaovalor;

                        string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                        object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                        vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                        if (vendaBilheteriaItem.Control.ID == 0)
                            throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                    }

                    foreach (DataRow i in ingressosPacote)
                    {
                        if ((bool)item["PermitirCancelamentoAvulso"])
                        {
                            CanalPacote canalPacote = new CanalPacote();

                            //busca as taxas de conveniencia e comissão e popula as variaveis
                            DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(Canal.CANAL_INTERNET, vendaBilheteriaItem.PacoteID.Valor);
                            Preco precoIngresso = new Preco();
                            precoIngresso.Ler((int)i["PrecoID"]);

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

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                        }

                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);
                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                        EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)i[INGRESSOID]).FirstOrDefault();
                        if (dono != null)
                        {
                            oIngressoCliente.ApresentacaoID.Valor = Convert.ToInt32(i[APRESENTACAOID]);
                            oIngressoCliente.ApresentacaoSetorID.Valor = Convert.ToInt32(i[APRESENTACAOSETORID]);
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            oCotaItemControle.ApresentacaoSetorID.Valor = Convert.ToInt32(i[APRESENTACAOSETORID]);
                            oCotaItemControle.ApresentacaoID.Valor = Convert.ToInt32(i[APRESENTACAOID]);

                            if (dono.CotaItemIDAPS > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, i[APRESENTACAOSETORID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
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
                                stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, i[APRESENTACAOID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                            }
                        }

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //caixa do usuario
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja 
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal 
                        ingressoLog.EmpresaID.Valor = empresaID; //empresa IR
                        ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                #region Cinemas -- Pós processamento de ingressos
                cinema = itensReservados.Select("CodigoProgramacao <> ''");
                if (cinema.Length > 0)
                {
                    this.EfetuarVendaCinema(cinema, estruturaReservaInternet.GUID, vendaBilheteria.Senha.Valor, vendaBilheteria.Control.ID);
                    ProcessouCinema = true;
                }
                #endregion

                VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento;

                EstruturaVendaFormaPagamento oFormaPagamento = formaPagamento.getFormaPagamentoVenda(oPagamento.FormaPagamentoID, empresaID);

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.CartaoID == 0)
                    {
                        if (oPagamento.oSitef != null)
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.CodigoSeguranca, oPagamento.oSitef.DataVencimento, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                        else
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.oAdyen.DataValidadeCartao, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                    }
                    else
                    {
                        if (oPagamento.oSitef != null)
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.DataVencimento, oPagamento.oSitef.CodigoSeguranca, oPagamento.NomeCartao);
                        else
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.DataValidadeCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.NomeCartao);
                    }
                }

                if (valeIngresso.Count > 0)
                {
                    #region Forma de Pagamento Com VIR
                    EstruturaTrocaValeIngresso oTrocaFixa = new EstruturaTrocaValeIngresso();

                    if (oPagamento.CodigoTrocaFixo.Length > 0)
                    {
                        oTrocaFixa = oValeIngresso.ValorIDVIR(oPagamento.CodigoTrocaFixo);
                        if (!oTrocaFixa.Encontrado)
                            throw new BilheteriaException("A Quantidade deste Vale Ingresso foi excedida.");
                    }

                    List<EstruturaTrocaValeIngresso> virs = new List<EstruturaTrocaValeIngresso>();
                    if (oPagamento.CodigoTrocaFixo.Length > 0)
                        virs = new List<EstruturaTrocaValeIngresso>() { oTrocaFixa };
                    else
                        virs = oValeIngresso.ValidarTrocaValeIngresso(valeIngresso);

                    decimal valorTotalVIR = ValeIngresso.ValorPagoVir(virs, oPagamento.EntregaValor, valorTotalConveniencia, valorTotalIngresso); ;
                    decimal porcentagemTotalVIR = Math.Round((valorTotalVIR * 100) / (valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor), 2);

                    if (porcentagemTotalVIR > 100)
                        porcentagemTotalVIR = 100;

                    decimal totalCompraCartao = valorTotal - valorTotalVIR;
                    decimal totalAtual = valorTotal;

                    if (totalCompraCartao > 0)
                    {

                        vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                        string sqlVendaBilheteriaFormaPagamento = String.Empty;

                        vendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                        vendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                        vendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oPagamento.FormaPagamentoID;
                        vendaBilheteriaFormaPagamento.Valor.Valor = totalCompraCartao;
                        vendaBilheteriaFormaPagamento.CartaoID.Valor = oPagamento.CartaoID;
                        vendaBilheteriaFormaPagamento.JurosValor.Valor = oPagamento.JurosValor;
                        vendaBilheteriaFormaPagamento.Coeficiente.Valor = oPagamento.Coeficiente;

                        totalAtual = totalAtual - totalCompraCartao;

                        decimal percCalc = 100 - porcentagemTotalVIR;
                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = percCalc;
                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;

                        switch (oPagamento.TipoPagamento)
                        {
                            case EstruturaPagamento.enumTipoPagamento.TEF:
                                if (vendaAsync)
                                    break;

                                vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oPagamento.oSitef.ID;
                                vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oSitef.CodigoRespostaSitefVenda;
                                vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oPagamento.oSitef.MensagemFinaliza;
                                vendaBilheteriaFormaPagamento.HoraTransacao.Valor = oPagamento.oSitef.HoraTransacao;
                                vendaBilheteriaFormaPagamento.DataTransacao.Valor = oPagamento.oSitef.DataTransacao;
                                vendaBilheteriaFormaPagamento.CodigoIR.Valor = oPagamento.oSitef.CodigoIR;
                                vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oSitef.NumeroAutorizacao;
                                vendaBilheteriaFormaPagamento.NSUHost.Valor = oPagamento.oSitef.NSUHost;
                                vendaBilheteriaFormaPagamento.NSUSitef.Valor = oPagamento.oSitef.NSUSitef;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.oSitef.CupomFiscal;
                                vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oPagamento.oSitef.DadosConfirmacao;
                                vendaBilheteriaFormaPagamento.Rede.Valor = oPagamento.oSitef.RedeRetorno;
                                vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oPagamento.oSitef.CodigoRespostaSitefFinaliza;
                                break;
                            case EstruturaPagamento.enumTipoPagamento.Adyen:
                                vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oAdyen.CodigoAutenticacao;
                                vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oAdyen.CodigoReferencia;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                                break;
                            case EstruturaPagamento.enumTipoPagamento.Paypal:
                                vendaBilheteriaFormaPagamento.TokenPayPal.Valor = oPagamento.Token;
                                vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor = oPagamento.PayerID;
                                vendaBilheteriaFormaPagamento.TransactionID.Valor = oPagamento.TransactionID;
                                vendaBilheteriaFormaPagamento.CorrelationID.Valor = oPagamento.CorrelationID;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalPaypal;
                                break;

                        }
                        sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();

                        if (bd.Executar(sqlVendaBilheteriaFormaPagamento) == 0)
                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);
                    }

                    EstruturaValeIngressoaFormaPagamento retornoPagamentoVIR = new EstruturaValeIngressoaFormaPagamento();
                    retornoPagamentoVIR = oValeIngresso.getFormaPagamentoVIR();

                    ValeIngressoLog valeIngressoLog;
                    foreach (var oEstrutura in virs)
                    {
                        decimal valor = ValeIngresso.ValorPagoVir(new List<EstruturaTrocaValeIngresso>() { oEstrutura }, oPagamento.EntregaValor, valorTotalConveniencia, valorTotalIngresso);
                        decimal porcentagemVIR = Math.Round((valor * 100) / (valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor), 2);

                        if (porcentagemVIR > 100)
                            porcentagemVIR = 100;
                        else if (porcentagemVIR < 0)
                            porcentagemVIR = 0;

                        vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oValeIngresso.FormaDePagamentoID; // ID DO PAGAMENTO VIR
                        vendaBilheteriaFormaPagamento.Valor.Valor = valor > totalAtual ? totalAtual : valor;
                        totalAtual = totalAtual - valor;
                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagemVIR;
                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;

                        string sqlVendaBilheteriaFormaPagamento = String.Empty;

                        vendaBilheteriaFormaPagamento.Dias.Valor = retornoPagamentoVIR.Dias;
                        vendaBilheteriaFormaPagamento.IR.Valor = retornoPagamentoVIR.IR;
                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = retornoPagamentoVIR.TaxaAdm;
                        vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = oEstrutura.ID;

                        sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();

                        int y = bd.Executar(sqlVendaBilheteriaFormaPagamento);

                        bool p = (y >= 1);
                        if (!p)
                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);

                        int alterado = oValeIngresso.AlterarStatus(bd, ValeIngresso.enumStatus.Trocado, oEstrutura.ID);
                        if (alterado != 1)
                            throw new BilheteriaException("A troca do Vale Ingresso gerou um erro, o vale-ingresso utilizado já foi trocado anteriormente.");

                        valeIngressoLog = new ValeIngressoLog();
                        valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Trocar).ToString();
                        valeIngressoLog.TimeStamp.Valor = DateTime.Now;
                        valeIngressoLog.ValeIngressoID.Valor = oEstrutura.ID;
                        valeIngressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                        valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        valeIngressoLog.VendaBilheteriaItemID.Valor = 0;
                        valeIngressoLog.EmpresaID.Valor = empresaID;
                        valeIngressoLog.CaixaID.Valor = caixaID;
                        valeIngressoLog.LojaID.Valor = estruturaReservaInternet.LojaID;
                        valeIngressoLog.CanalID.Valor = estruturaReservaInternet.CanalID;
                        valeIngressoLog.CodigoTroca.Valor = oPagamento.CodigoTrocaFixo;
                        valeIngressoLog.CodigoBarra.Valor = string.Empty;
                        valeIngressoLog.ClienteNome.Valor = string.Empty;
                        valeIngressoLog.Obs.Valor = string.Empty;

                        if (bd.Executar(valeIngressoLog.StringInserir()) == 0)
                            throw new Exception("Não foi possível trocar o Vale ingresso.");

                    }

                    #endregion
                }
                //VendaBilheteriaFormaPagamento SEM VIR
                //Verifica se está comprando cortesias, caso positivo, não insere na VendaBilheteriaFormaPagamento
                else if (oPagamento.FormaPagamentoID > 0 && valorTotal > 0)
                {
                    #region FormaPagamento sem VIR
                    vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                    vendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    vendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oPagamento.FormaPagamentoID;
                    vendaBilheteriaFormaPagamento.Valor.Valor = valorTotal;
                    decimal porcentagemTotal = 100;
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagemTotal;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.CartaoID.Valor = oPagamento.CartaoID;
                    vendaBilheteriaFormaPagamento.JurosValor.Valor = oPagamento.JurosValor;
                    vendaBilheteriaFormaPagamento.Coeficiente.Valor = oPagamento.Coeficiente;

                    string sqlVendaBilheteriaFormaPagamento = String.Empty;

                    switch (oPagamento.TipoPagamento)
                    {
                        case EstruturaPagamento.enumTipoPagamento.TEF:
                            if (vendaAsync)
                                break;

                            vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oPagamento.oSitef.ID;
                            vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oSitef.CodigoRespostaSitefVenda;
                            vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oPagamento.oSitef.MensagemFinaliza;
                            vendaBilheteriaFormaPagamento.HoraTransacao.Valor = oPagamento.oSitef.HoraTransacao;
                            vendaBilheteriaFormaPagamento.DataTransacao.Valor = oPagamento.oSitef.DataTransacao;
                            vendaBilheteriaFormaPagamento.CodigoIR.Valor = oPagamento.oSitef.CodigoIR;
                            vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oSitef.NumeroAutorizacao;
                            vendaBilheteriaFormaPagamento.NSUHost.Valor = oPagamento.oSitef.NSUHost;
                            vendaBilheteriaFormaPagamento.NSUSitef.Valor = oPagamento.oSitef.NSUSitef;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.oSitef.CupomFiscal;
                            vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oPagamento.oSitef.DadosConfirmacao;
                            vendaBilheteriaFormaPagamento.Rede.Valor = oPagamento.oSitef.RedeRetorno;
                            vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oPagamento.oSitef.CodigoRespostaSitefFinaliza;
                            break;
                        case EstruturaPagamento.enumTipoPagamento.Adyen:
                            vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oAdyen.CodigoAutenticacao;
                            vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oAdyen.CodigoReferencia;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                            break;
                        case EstruturaPagamento.enumTipoPagamento.Paypal:
                            vendaBilheteriaFormaPagamento.TokenPayPal.Valor = oPagamento.Token;
                            vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor = oPagamento.PayerID;
                            vendaBilheteriaFormaPagamento.TransactionID.Valor = oPagamento.TransactionID;
                            vendaBilheteriaFormaPagamento.CorrelationID.Valor = oPagamento.CorrelationID;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalPaypal;
                            break;
                    }
                    sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                    if (bd.Executar(sqlVendaBilheteriaFormaPagamento) == 0)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);

                    #endregion
                }

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.IniciouTef)
                    {
                        if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                    else
                    {
                        if (!oPagamento.oAdyen.CapturarPagamento())
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                }

                bd.FinalizarTransacao();

                #region #_#_#_#_#_#_#_# Seguro! Fica fora de transaction! #_#_#_#_#_#_#_#_#_#_#_
                bool SucessoSeguro = true;

                try
                {
                    if (ValorTotalSeguro > 0 && oPagamento.TipoPagamento != EstruturaPagamento.enumTipoPagamento.Paypal)
                    {
                        EstruturaInfoEventoMondial estruturaInfo = new EstruturaInfoEventoMondial();

                        TicketProtectorWebService objService = new TicketProtectorWebService();
                        objService.Url = ConfigurationManager.AppSettings["URLMondial"];

                        string ProductID = ConfigurationManager.AppSettings["ProductID"];
                        decimal productCost = ValorTotalSeguro / ingressosVendidos.Rows.Count;

                        MDLPolicyRequest mdlPolicyRequest = new MDLPolicyRequest();

                        mdlPolicyRequest.CompanyID = ConfigurationManager.AppSettings["CompanyID"];
                        mdlPolicyRequest.AuthenticationCode = ConfigurationManager.AppSettings["AuthenticationCode"];

                        mdlPolicyRequest.CreditCardExpMonth = oPagamento.DataVencimento.Substring(0, 2);
                        mdlPolicyRequest.CreditCardExpYear = "20" + oPagamento.DataVencimento.Substring(2, 2);
                        mdlPolicyRequest.CreditCardHolderName = oClienteSelecionado.Nome.Valor;
                        mdlPolicyRequest.CreditCardNumber = oPagamento.NumeroCartao;
                        mdlPolicyRequest.CreditCardSecurityCode = oPagamento.CodigoSeguranca;

                        int bandeiraID = oCartao.BuscaBandeira(oPagamento.FormaPagamentoID);

                        switch (bandeiraID)
                        {
                            case (int)Cartao.enumBandeira.Visa:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Visa;
                                break;
                            case (int)Cartao.enumBandeira.Mastercard:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Mastercard;
                                break;
                            case (int)Cartao.enumBandeira.Amex:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Amex;
                                break;
                            case (int)Cartao.enumBandeira.Hipercard:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Hipercard;
                                break;
                            default:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Visa;
                                break;
                        }

                        mdlPolicyRequest.CreditCardType = bandeiraID.ToString(); // 1: VISA; 2: MASTER; 3: AMEX; 5: HIPERCARD
                        mdlPolicyRequest.PaymentTypeID = 1; // 1: Cartão de Crédito; 2: Pagamento automático

                        // Endereço segurado
                        mdlPolicyRequest.CustomerAddresCompliment = oClienteSelecionado.ComplementoCliente.Valor;
                        mdlPolicyRequest.CustomerAddress = oClienteSelecionado.EnderecoCliente.Valor;
                        mdlPolicyRequest.CustomerAddressNumber = oClienteSelecionado.NumeroCliente.Valor;
                        mdlPolicyRequest.CustomerCity = oClienteSelecionado.CidadeCliente.Valor;
                        mdlPolicyRequest.CustomerDistrict = oClienteSelecionado.BairroCliente.Valor;
                        mdlPolicyRequest.CustomerState = oClienteSelecionado.EstadoCliente.Valor;
                        mdlPolicyRequest.CustomerZipCode = oClienteSelecionado.CEPCliente.Valor;
                        mdlPolicyRequest.CustomerBirthDate = DateTime.Parse(oClienteSelecionado.DataNascimento.ToString());

                        // Dados comprador
                        mdlPolicyRequest.CustomerDocumentID = oClienteSelecionado.CPF.Valor;
                        mdlPolicyRequest.CustomerEmail = oClienteSelecionado.Email.Valor;
                        mdlPolicyRequest.CustomerMobilePhone = oClienteSelecionado.Celular.Valor;
                        mdlPolicyRequest.CustomerName = oClienteSelecionado.Nome.Valor;
                        mdlPolicyRequest.CustomerPhone = oClienteSelecionado.Telefone.Valor;

                        // Apólice primária, no caso da Ingresso Rápido, sempre passar vazio
                        mdlPolicyRequest.PrimaryPolicyID = string.Empty;

                        // Taxas
                        mdlPolicyRequest.RateOfConvenience = valorTotalConveniencia;
                        mdlPolicyRequest.RateOfDelivery = 0;

                        // Identificador da compra na Ingresso Rápido
                        mdlPolicyRequest.ReferenceOrderCode = vendaBilheteria.Control.ID.ToString();

                        mdlPolicyRequest.RequestDate = DateTime.Now;
                        mdlPolicyRequest.RequestOriginID = 1; // No caso da Ingresso Rápido, sempre 1
                        mdlPolicyRequest.RequestStatus = 7; // Booking Path (7): Pronto para cobrança do cartão via gateway
                        // Pagamento automático (1): Compras sem pagamento (faturamento inverso)

                        // Valor
                        mdlPolicyRequest.TotalCost = ValorTotalSeguro; // Valor dos produtos * qtde. de segurados

                        // Instanciar cada segurado. Neste exemplo, 2 segurados
                        MDLCustomer[] mdlCustomer = new MDLCustomer[ingressosVendidos.Rows.Count];

                        int cont = 0;
                        int ApresentacaoSetorID = 0;

                        foreach (DataRow item in itensReservados.Rows)
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
                            mdlCustomer[cont].InsuredBirthDate = DateTime.Parse(oClienteSelecionado.DataNascimento.ToString()); // Data Nascimento
                            mdlCustomer[cont].InsuredDocumentID = oClienteSelecionado.CPF.Valor; // CPF
                            mdlCustomer[cont].InsuredID = 0; // No caso da ingresso Rápido, sempre 0
                            mdlCustomer[cont].InsuredName = oClienteSelecionado.Nome.Valor; // Nome do Cliente
                            mdlCustomer[cont].ProductID = ProductID; // Produto Mondial

                            // Dados do Evento
                            mdlCustomer[cont].EventClave = estruturaInfo.ApresentacaoID.ToString(); // Código do Evento. Este é o identificador do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo evento!!
                            mdlCustomer[cont].EventName = estruturaInfo.Evento;
                            mdlCustomer[cont].EventDate = estruturaInfo.Horario;
                            mdlCustomer[cont].EventID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do evento é o campo "EventClave"

                            // Dados do setor
                            mdlCustomer[cont].SectorCode = estruturaInfo.SetorID + "_" + item["PrecoID"]; // Código do Setor. Este é o identificador do setor do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo setor!!
                            mdlCustomer[cont].SectorName = estruturaInfo.Setor;

                            decimal ValorIngressoConveniencia = Convert.ToDecimal(item["Valor"]);
                            mdlCustomer[cont].SectorPrice = ValorIngressoConveniencia;

                            mdlCustomer[cont].EventLocal = estruturaInfo.Local;
                            mdlCustomer[cont].SectorID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do setor é o campo "SectorCode"

                            cont++;
                        }

                        mdlPolicyRequest.CustomerCollection = mdlCustomer;

                        MDLRequestResult[] mdlRequestResult;
                        mdlRequestResult = objService.CreatePolicy(mdlPolicyRequest);

                        new ApoliceMondial().InserirLista(mdlRequestResult, vendaBilheteria.Control.ID);
                    }
                }
                catch (Exception)
                {
                    SucessoSeguro = false;
                }
                #endregion

                try
                {
                    if (celular.Length > 0 && vendaBilheteria.Status.Valor == (string)VendaBilheteria.PAGO && vendaBilheteria.PagamentoProcessado.Valor)
                        new EnviaSMS().EnviaSms(true, vendaBilheteria.Control.ID, celular);
                }
                catch (Exception)
                {
                }

                if (SucessoSeguro)
                    return MontaRetornoVendaInternet(((int)CodMensagemVenda.Sucesso).ToString(), senha, vendaBilheteria.Control.ID, celular, "1");
                else
                    return MontaRetornoVendaInternet(((int)CodMensagemVenda.ErroSeguro).ToString(), senha, vendaBilheteria.Control.ID, celular, "1");
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();

                if (ProcessouCinema && cinema != null && cinema.Length > 0)
                    this.CancelarVendaCinema(cinema, estruturaReservaInternet.GUID, senha, vendaBilheteriaID);

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF && ProcessouSitef)
                {
                    if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");
                }
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Paypal && ProcessouPayPal)
                    this.Refund(Usuario, Senha, Assinatura, oPagamento.TransactionID, AmbienteTestePayPal);
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen && ProcessouAdyen)
                    oPagamento.oAdyen.CancelarPagamento();

                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string[] VenderMobile(EstruturaPagamento oPagamento, Dictionary<int, decimal> ingressosID, string sessionID, List<int> valeIngresso,
          string celular, List<EstruturaDonoIngresso> listaDonoIngresso, EstruturaReservaInternet estruturaReservaInternet, decimal ValorTotalSeguro, decimal valorTotalTaxaProcessamento, Cliente oClienteSelecionado)
        {
            BD bd = new BD();
            decimal valorTotalIngresso = 0;
            decimal valorTotalConveniencia = 0;
            decimal valorTotalComissao = 0;
            decimal valorTotal = 0;
            string sql = string.Empty;

            bool vendaAsync = Convert.ToBoolean(ConfigurationManager.AppSettings["VendaAsync"]);
            string Usuario = Convert.ToString(ConfigurationManager.AppSettings["Usuario"]);
            string Senha = Convert.ToString(ConfigurationManager.AppSettings["Senha"]);
            string Assinatura = Convert.ToString(ConfigurationManager.AppSettings["Assinatura"]);
            bool AmbienteTestePayPal = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbienteTestePayPal"]);

            bool ProcessouCinema = false;
            bool ProcessouSitef = false;
            bool ProcessouAdyen = false;
            bool ProcessouPayPal = false;

            DataRow[] cinema = null;
            string senha = string.Empty;
            int vendaBilheteriaID = 0;

            try
            {
                Cartao oCartao = new Cartao();

                StringBuilder stbSQL = new StringBuilder();
                //caixa para registrar a venda
                int caixaID = 0;
                if (estruturaReservaInternet.CaixaID == 0)
                    caixaID = VerificaCaixaInternet();
                else
                    //Verificar o caixa do usuario.
                    caixaID = estruturaReservaInternet.CaixaID;

                if (oPagamento.ClienteID == 0)
                    oPagamento.ClienteID = oClienteSelecionado.Control.ID;

                ValeIngresso oValeIngresso = new ValeIngresso();
                FormaPagamento formaPagamento = new FormaPagamento();
                CotaItemControle oCotaItemControle = new CotaItemControle();
                IngressoCliente oIngressoCliente = new IngressoCliente();
                //busca os dados da reserva para determinado cliente
                DataSet ds = MontaDadosVendaInternet(ingressosID, oPagamento.ClienteID, sessionID, estruturaReservaInternet.CanalID, estruturaReservaInternet.UsuarioID);

                DataTable itensReservados = ds.Tables[TABELA_GRID]; // Resumo
                DataTable ingressosVendidos = ds.Tables[TABELA_RESERVA]; // Detalhamento

                //verifica se a quantidade reservada no site é a mesma reservada no sistema
                if (ingressosVendidos.Rows.Count != ingressosID.Count)
                    throw new BilheteriaException("Quantidade de ingressos diferente da reserva.", CodMensagemVenda.FalhaVenda);

                bd.IniciarTransacao();

                //verifica se existem reservas
                if (itensReservados.Rows.Count == 0 && ingressosVendidos.Rows.Count == 0)
                    throw new BilheteriaException("Não existe reserva para o cliente.", CodMensagemVenda.ReservaInexistente);

                // VendaBilheteria
                int entregaControleID = oPagamento.EntregaControleID;
                int entregaAgendaID = 0;
                int pdvID = oPagamento.PdvID;
                int clienteEnderecoID = oPagamento.EnderecoClienteID;
                int empresaID = new Canal().BuscaEmpresaIDporLojaID(estruturaReservaInternet.LojaID);
                EstruturaEntregaAgenda entregaSelecionada = new EstruturaEntregaAgenda();
                EntregaAgenda oEA = new EntregaAgenda();

                entregaSelecionada = oEA.CarregarEntrega(entregaControleID, oPagamento.DataSelecionada);

                if (oEA.PodeSerAgendado(entregaSelecionada))
                {
                    object verifAgendado = null;

                    string cmd = oEA.String();
                    verifAgendado = bd.Executar(cmd);

                    if (verifAgendado == null)
                        throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");
                    else
                        if (Convert.ToInt32(verifAgendado) <= 0)
                            throw new BilheteriaException("Venda não foi gerada. Verificar a Data de Entrega");

                    entregaAgendaID = oEA.Control.ID;
                }
                else
                {
                    switch (entregaSelecionada.Tipo)
                    {
                        case Entrega.AGENDADA:
                            throw new BilheteriaException("Venda não foi gerada.");
                        default:
                            break;
                    }
                }

                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                if (!string.IsNullOrEmpty(celular))
                {
                    string DDD = celular.Substring(0, 2);
                    string numeroCelular = celular.Substring(2, celular.Length - 2);

                    vendaBilheteria.NumeroCelular.Valor = Convert.ToInt32(numeroCelular);
                    vendaBilheteria.DDD.Valor = Convert.ToInt32(DDD);
                }
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.NivelRisco.Valor = (int)VendaBilheteria.enumNivelRisco.SemRisco;

                vendaBilheteria.ClienteID.Valor = oPagamento.ClienteID;
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;

                vendaBilheteria.ClienteEnderecoID.Valor = clienteEnderecoID;
                vendaBilheteria.EntregaControleID.Valor = entregaControleID;
                vendaBilheteria.EntregaAgendaID.Valor = entregaAgendaID;
                vendaBilheteria.PdvID.Valor = pdvID;
                vendaBilheteria.TaxaEntregaValor.Valor = oPagamento.EntregaValor;
                vendaBilheteria.PagamentoProcessado.Valor = true;


                //valor total
                object valorAuxiliar = null;
                object valorAuxiliarComissao = null;
                valorAuxiliar = itensReservados.Compute("SUM(Valor)", "");
                valorTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                valorAuxiliar = itensReservados.Compute("SUM(TaxaConvenienciaValor)", "");
                valorTotalConveniencia = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                valorAuxiliarComissao = itensReservados.Compute("SUM(ComissaoValor)", "");
                valorTotalComissao = valorAuxiliarComissao == System.DBNull.Value ? 0 : (decimal)valorAuxiliarComissao;

                valorTotal = valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor + valorTotalTaxaProcessamento;
                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = valorTotalConveniencia;
                vendaBilheteria.ComissaoValorTotal.Valor = valorTotalComissao;
                vendaBilheteria.ValorSeguro.Valor = ValorTotalSeguro;
                vendaBilheteria.TaxaProcessamentoValor.Valor = valorTotalTaxaProcessamento;
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.IP.Valor = oPagamento.IP;
                vendaBilheteria.QuantidadeImpressoesInternet.Valor = 0;
                vendaBilheteria.NomeCartao.Valor = oPagamento.NomeCartao ?? string.Empty;

                switch (oPagamento.TipoPagamento)
                {
                    case EstruturaPagamento.enumTipoPagamento.TEF:
                        ProcessouSitef = this.EfetuaPagamentoTef(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Adyen:
                        ProcessouAdyen = this.EfetuaPagamentoAdyen(oPagamento, vendaAsync);
                        break;
                    case EstruturaPagamento.enumTipoPagamento.Paypal:
                        DoExpressCheckoutPaymentOperation DoCheckOut = PayPalApiFactory.instance.ExpressCheckout(
                                Usuario,
                                Senha,
                                Assinatura
                            ).DoExpressCheckoutPayment(oPagamento.Token, oPagamento.PayerID, (double)oPagamento.ValorTotal, PaymentAction.SALE);

                        DoCheckOut.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                        DoCheckOut.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

                        if (AmbienteTestePayPal)
                            DoCheckOut.sandbox().execute();
                        else
                            DoCheckOut.execute();

                        if (DoCheckOut.ResponseNVP.Ack != Ack.SUCCESS)
                            throw new Exception("Não foi possível evetuar o pagamento através do PayPal");
                        else
                        {
                            oPagamento.TransactionID = DoCheckOut.ResponseNVP.TransactionId;
                            oPagamento.CorrelationID = DoCheckOut.ResponseNVP.CorrelationId;
                        }
                        vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalPaypal;
                        ProcessouPayPal = true;
                        break;
                    default:
                        break;
                }

                if (oPagamento.IniciouTef)
                    vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.oSitef.CupomFiscal;
                else if (oPagamento.IniciouAdyen)
                {
                    vendaBilheteria.NotaFiscalCliente.Valor = oPagamento.NotaFiscalAdyen;
                    vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                }
                else
                {
                    if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Paypal)
                        vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = oPagamento.NotaFiscalPaypal;
                    else
                        vendaBilheteria.NotaFiscalCliente.Valor = vendaBilheteria.NotaFiscalEstabelecimento.Valor = "";
                }

                if (oPagamento.oAdyen.Fraud)
                    vendaBilheteria.AccertifyForceStatus.Valor = ((char)VendaBilheteria.enumForceStatus.Review).ToString();

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria (NOLOCK) WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                senha = (ret != null) ? Convert.ToString(ret) : null;
                vendaBilheteriaID = vendaBilheteria.Control.ID;

                DataRow[] itensNormais = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "<>'" + Setor.MesaFechada + "'");
                DataRow[] itensMesaFechada = itensReservados.Select(TIPO + "='" + TIPO_INGRESSO + "' AND " + TIPO_LUGAR + "='" + Setor.MesaFechada + "'");
                DataRow[] itensPacote = itensReservados.Select(TIPO + "='" + TIPO_PACOTE + "'");

                #region Ingressos Normais
                foreach (DataRow item in itensNormais)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    int x = 0;
                    bool ok = false;
                    stbSQL = new StringBuilder();


                    stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                    stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                    stbSQL.Append("WHERE(AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)item["IngressoID"]);
                    x = bd.Executar(stbSQL.ToString());
                    ok = (x >= 1);

                    if (!ok)
                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                    EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)item[INGRESSOID]).FirstOrDefault();
                    if (dono != null)
                    {
                        oIngressoCliente.ApresentacaoID.Valor = (int)item[APRESENTACAOID];
                        oIngressoCliente.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                        oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                        oIngressoCliente.DonoID.Valor = dono.DonoID;
                        oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                        oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                        oIngressoCliente.CPF.Valor = dono.CPF;

                        bd.Executar(oIngressoCliente.StringInserir());

                        oCotaItemControle.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                        oCotaItemControle.ApresentacaoID.Valor = (int)item[APRESENTACAOID];

                        if (dono.CotaItemIDAPS > 0)
                        {
                            oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                            bd.Executar(oCotaItemControle.StringAtualizarAPS());

                            stbSQL = new StringBuilder();
                            stbSQL.Append("SELECT ");
                            stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                            stbSQL.Append("THEN 0 ");
                            stbSQL.Append("ELSE 1 ");
                            stbSQL.Append("END AS Valido ");
                            stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                            stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, item[APRESENTACAOSETORID]);

                            if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
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
                            stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                            stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, item[APRESENTACAOID]);

                            if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                        }
                    }

                    //inserir na Log
                    IngressoLog ingressoLog = new IngressoLog();
                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                    ingressoLog.IngressoID.Valor = (int)item[INGRESSOID];
                    ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario fixo para Internet
                    ingressoLog.BloqueioID.Valor = 0;
                    ingressoLog.CortesiaID.Valor = Convert.ToInt32(ingressos[0][CORTESIAID]);
                    ingressoLog.PrecoID.Valor = (int)item[PRECOID];
                    ingressoLog.GerenciamentoIngressosID.Valor = (int)item[GERENCIAMENTO_INGRESSOS_ID];
                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    ingressoLog.CaixaID.Valor = caixaID; // ABERTURA E FECHAMENTO DE CAIXA DIARIO PARA INTERNET
                    ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja fixa Internet
                    ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal fixo Internet
                    ingressoLog.EmpresaID.Valor = empresaID; // FIXO IR
                    ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                    ingressoLog.Acao.Valor = IngressoLog.VENDER;
                    string sqlIngressoLogV = ingressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                }
                #endregion

                #region Mesa Fechada
                foreach (DataRow item in itensMesaFechada)
                {

                    DataRow[] ingressos = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = 0;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item[CONV];
                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)item["TaxaConvenienciaValor"];
                    vendaBilheteriaItem.TaxaComissao.Valor = (int)item[TAXA_COMISSAO];
                    vendaBilheteriaItem.ComissaoValor.Valor = (decimal)item[COMISSAO_VALOR];

                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (vendaBilheteriaItem.Control.ID == 0)
                        throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);

                    foreach (DataRow i in ingressos)
                    {
                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);

                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                        EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)i[INGRESSOID]).FirstOrDefault();
                        if (dono != null)
                        {
                            oIngressoCliente.ApresentacaoID.Valor = (int)item[APRESENTACAOID];
                            oIngressoCliente.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            oCotaItemControle.ApresentacaoSetorID.Valor = (int)item[APRESENTACAOSETORID];
                            oCotaItemControle.ApresentacaoID.Valor = (int)item[APRESENTACAOID];

                            if (dono.CotaItemIDAPS > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, item[APRESENTACAOSETORID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para o Setor.");
                                //}
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
                                stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, item[APRESENTACAOID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                            }

                        }

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //USUARIO INTERNET
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //CAIXA ABERTO DIARIAMENTE PARA INTERNET
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //Loja Internet
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //Canal Internet
                        ingressoLog.EmpresaID.Valor = empresaID; //Empresa IR
                        ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                #region Pacote
                int PacoteGrupo = 0;
                int pacoteIDAtual = 0;
                foreach (DataRow item in itensPacote)
                {

                    DataRow[] ingressosPacote = ingressosVendidos.Select(RESERVAID + "='" + (int)item[RESERVAID] + "'");

                    Pacote oPacote = new Pacote();
                    oPacote.Ler((int)ingressosPacote[0][PACOTEID]);

                    VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaItem.PacoteID.Valor = oPacote.Control.ID;
                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;


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
                    decimal valorConv = (decimal)item["TaxaConvenienciaValor"];
                    int taxaComissao = (int)item[TAXA_COMISSAO];
                    decimal comissaovalor = (decimal)item[COMISSAO_VALOR];

                    if (!(bool)item["PermitirCancelamentoAvulso"])
                    {
                        vendaBilheteriaItem.TaxaConveniencia.Valor = conv;
                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = valorConv;
                        vendaBilheteriaItem.TaxaComissao.Valor = taxaComissao;
                        vendaBilheteriaItem.ComissaoValor.Valor = comissaovalor;

                        string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                        object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                        vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                        if (vendaBilheteriaItem.Control.ID == 0)
                            throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                    }

                    foreach (DataRow i in ingressosPacote)
                    {
                        if ((bool)item["PermitirCancelamentoAvulso"])
                        {
                            CanalPacote canalPacote = new CanalPacote();

                            //busca as taxas de conveniencia e comissão e popula as variaveis
                            DataTable taxasPacote = canalPacote.BuscaTaxasConvenienciaComissao(Canal.CANAL_INTERNET, vendaBilheteriaItem.PacoteID.Valor);
                            Preco precoIngresso = new Preco();
                            precoIngresso.Ler((int)i["PrecoID"]);

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

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.", CodMensagemVenda.FalhaVenda);
                        }

                        int x = 0;
                        bool ok = false;
                        stbSQL = new StringBuilder();

                        stbSQL.Append("UPDATE tIngresso SET LojaID=" + estruturaReservaInternet.LojaID + ", ");
                        stbSQL.Append("VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.VENDIDO + "' ");
                        stbSQL.Append("WHERE (AssinaturaClienteID IS NULL OR AssinaturaClienteID = 0) AND ClienteID=" + oPagamento.ClienteID + " AND SessionID = '" + sessionID + "' AND Status='" + Ingresso.RESERVADO + "' AND ID=" + (int)i[INGRESSOID]);
                        x = bd.Executar(stbSQL.ToString());
                        ok = (x >= 1);
                        if (!ok)
                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.", CodMensagemVenda.FalhaVenda);

                        EstruturaDonoIngresso dono = listaDonoIngresso.Where(c => c.IngressoID == (int)i[INGRESSOID]).FirstOrDefault();
                        if (dono != null)
                        {
                            oIngressoCliente.ApresentacaoID.Valor = Convert.ToInt32(i[APRESENTACAOID]);
                            oIngressoCliente.ApresentacaoSetorID.Valor = Convert.ToInt32(i[APRESENTACAOSETORID]);
                            oIngressoCliente.CotaItemID.Valor = dono.CotaItemIDAPS > 0 ? dono.CotaItemIDAPS : dono.CotaItemID;
                            oIngressoCliente.DonoID.Valor = dono.DonoID;
                            oIngressoCliente.IngressoID.Valor = dono.IngressoID;
                            oIngressoCliente.CodigoPromocional.Valor = dono.CodigoPromocional;
                            oIngressoCliente.CPF.Valor = dono.CPF;

                            bd.Executar(oIngressoCliente.StringInserir());

                            oCotaItemControle.ApresentacaoSetorID.Valor = Convert.ToInt32(i[APRESENTACAOSETORID]);
                            oCotaItemControle.ApresentacaoID.Valor = Convert.ToInt32(i[APRESENTACAOID]);

                            if (dono.CotaItemIDAPS > 0)
                            {
                                oCotaItemControle.CotaItemID.Valor = dono.CotaItemIDAPS;

                                bd.Executar(oCotaItemControle.StringAtualizarAPS());

                                stbSQL = new StringBuilder();
                                stbSQL.Append("SELECT ");
                                stbSQL.Append("CASE WHEN tApresentacaoSetor.Quantidade > 0 AND tCotaItemControle.Quantidade > tApresentacaoSetor.Quantidade ");
                                stbSQL.Append("THEN 0 ");
                                stbSQL.Append("ELSE 1 ");
                                stbSQL.Append("END AS Valido ");
                                stbSQL.Append("FROM tCotaItemControle (NOLOCK) ");
                                stbSQL.Append("INNER JOIN tApresentacaoSetor ON tCotaItemControle.ApresentacaoSetorID = tApresentacaoSetor.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND tCotaItemControle.ApresentacaoSetorID = {1}", dono.CotaItemIDAPS, i[APRESENTACAOSETORID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
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
                                stbSQL.Append("JOIN tApresentacao ON tCotaItemControle.ApresentacaoID = tApresentacao.ID ");
                                stbSQL.AppendFormat("WHERE tCotaItemControle.CotaItemID = {0} AND ApresentacaoID = {1} AND tCotaItemControle.ApresentacaoSetorID = 0 ", dono.CotaItemID, i[APRESENTACAOID]);

                                if (!Convert.ToBoolean(bd.ConsultaValor(stbSQL.ToString())))
                                    throw new BilheteriaException("Um dos preços especiais na reserva excedeu o limite de venda para a Apresentação.");
                            }
                        }

                        //inserir na Log
                        IngressoLog ingressoLog = new IngressoLog();
                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                        ingressoLog.IngressoID.Valor = (int)i[INGRESSOID];
                        ingressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID; //usuario
                        ingressoLog.BloqueioID.Valor = 0;
                        ingressoLog.CortesiaID.Valor = Convert.ToInt32(i[CORTESIAID]);
                        ingressoLog.PrecoID.Valor = (int)i[PRECOID];
                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        ingressoLog.CaixaID.Valor = caixaID; //caixa do usuario
                        ingressoLog.LojaID.Valor = estruturaReservaInternet.LojaID; //loja 
                        ingressoLog.CanalID.Valor = estruturaReservaInternet.CanalID; //canal 
                        ingressoLog.EmpresaID.Valor = empresaID; //empresa IR
                        ingressoLog.ClienteID.Valor = oPagamento.ClienteID;
                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                        string sqlIngressoLogV = ingressoLog.StringInserir();
                        x = bd.Executar(sqlIngressoLogV);
                        bool okV = (x == 1);
                        if (!okV)
                            throw new BilheteriaException("Log de venda do ingresso não foi inserido.", CodMensagemVenda.FalhaVenda);
                    }
                }
                #endregion

                #region Cinemas -- Pós processamento de ingressos
                cinema = itensReservados.Select("CodigoProgramacao <> ''");
                if (cinema.Length > 0)
                {
                    this.EfetuarVendaCinema(cinema, estruturaReservaInternet.GUID, vendaBilheteria.Senha.Valor, vendaBilheteria.Control.ID);
                    ProcessouCinema = true;
                }
                #endregion

                VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento;

                EstruturaVendaFormaPagamento oFormaPagamento = formaPagamento.getFormaPagamentoVenda(oPagamento.FormaPagamentoID, empresaID);

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.CartaoID == 0)
                    {
                        if (oPagamento.oSitef != null)
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.CodigoSeguranca, oPagamento.oSitef.DataVencimento, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                        else
                            oPagamento.CartaoID = oCartao.InserirCartao(oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.oAdyen.DataValidadeCartao, oFormaPagamento.FormaPagamentoID, oPagamento.ClienteID, oPagamento.NomeCartao);
                    }
                    else
                    {
                        if (oPagamento.oSitef != null)
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oSitef.NumeroCartao, oPagamento.oSitef.DataVencimento, oPagamento.oSitef.CodigoSeguranca, oPagamento.NomeCartao);
                        else
                            oCartao.AtualizarCriptografiaCartao(oPagamento.CartaoID, oPagamento.oAdyen.NumeroCartao, oPagamento.oAdyen.DataValidadeCartao, oPagamento.oAdyen.CodigoVerificacaoCartao, oPagamento.NomeCartao);
                    }
                }

                if (valeIngresso.Count > 0)
                {
                    #region Forma de Pagamento Com VIR
                    EstruturaTrocaValeIngresso oTrocaFixa = new EstruturaTrocaValeIngresso();

                    if (oPagamento.CodigoTrocaFixo.Length > 0)
                    {
                        oTrocaFixa = oValeIngresso.ValorIDVIR(oPagamento.CodigoTrocaFixo);
                        if (!oTrocaFixa.Encontrado)
                            throw new BilheteriaException("A Quantidade deste Vale Ingresso foi excedida.");
                    }

                    List<EstruturaTrocaValeIngresso> virs = new List<EstruturaTrocaValeIngresso>();
                    if (oPagamento.CodigoTrocaFixo.Length > 0)
                        virs = new List<EstruturaTrocaValeIngresso>() { oTrocaFixa };
                    else
                        virs = oValeIngresso.ValidarTrocaValeIngresso(valeIngresso);

                    decimal valorTotalVIR = ValeIngresso.ValorPagoVir(virs, oPagamento.EntregaValor, valorTotalConveniencia, valorTotalIngresso); ;
                    decimal porcentagemTotalVIR = Math.Round((valorTotalVIR * 100) / (valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor), 2);

                    if (porcentagemTotalVIR > 100)
                        porcentagemTotalVIR = 100;

                    decimal totalCompraCartao = valorTotal - valorTotalVIR;
                    decimal totalAtual = valorTotal;

                    if (totalCompraCartao > 0)
                    {

                        vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                        string sqlVendaBilheteriaFormaPagamento = String.Empty;

                        vendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                        vendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                        vendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oPagamento.FormaPagamentoID;
                        vendaBilheteriaFormaPagamento.Valor.Valor = totalCompraCartao;
                        vendaBilheteriaFormaPagamento.CartaoID.Valor = oPagamento.CartaoID;
                        vendaBilheteriaFormaPagamento.JurosValor.Valor = oPagamento.JurosValor;
                        vendaBilheteriaFormaPagamento.Coeficiente.Valor = oPagamento.Coeficiente;

                        totalAtual = totalAtual - totalCompraCartao;

                        decimal percCalc = 100 - porcentagemTotalVIR;
                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = percCalc;
                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;

                        switch (oPagamento.TipoPagamento)
                        {
                            case EstruturaPagamento.enumTipoPagamento.TEF:
                                if (vendaAsync)
                                    break;

                                vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oPagamento.oSitef.ID;
                                vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oSitef.CodigoRespostaSitefVenda;
                                vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oPagamento.oSitef.MensagemFinaliza;
                                vendaBilheteriaFormaPagamento.HoraTransacao.Valor = oPagamento.oSitef.HoraTransacao;
                                vendaBilheteriaFormaPagamento.DataTransacao.Valor = oPagamento.oSitef.DataTransacao;
                                vendaBilheteriaFormaPagamento.CodigoIR.Valor = oPagamento.oSitef.CodigoIR;
                                vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oSitef.NumeroAutorizacao;
                                vendaBilheteriaFormaPagamento.NSUHost.Valor = oPagamento.oSitef.NSUHost;
                                vendaBilheteriaFormaPagamento.NSUSitef.Valor = oPagamento.oSitef.NSUSitef;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.oSitef.CupomFiscal;
                                vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oPagamento.oSitef.DadosConfirmacao;
                                vendaBilheteriaFormaPagamento.Rede.Valor = oPagamento.oSitef.RedeRetorno;
                                vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oPagamento.oSitef.CodigoRespostaSitefFinaliza;
                                break;
                            case EstruturaPagamento.enumTipoPagamento.Adyen:
                                vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oAdyen.CodigoAutenticacao;
                                vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oAdyen.CodigoReferencia;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                                break;
                            case EstruturaPagamento.enumTipoPagamento.Paypal:
                                vendaBilheteriaFormaPagamento.TokenPayPal.Valor = oPagamento.Token;
                                vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor = oPagamento.PayerID;
                                vendaBilheteriaFormaPagamento.TransactionID.Valor = oPagamento.TransactionID;
                                vendaBilheteriaFormaPagamento.CorrelationID.Valor = oPagamento.CorrelationID;
                                vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalPaypal;
                                break;

                        }
                        sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();

                        if (bd.Executar(sqlVendaBilheteriaFormaPagamento) == 0)
                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);
                    }

                    EstruturaValeIngressoaFormaPagamento retornoPagamentoVIR = new EstruturaValeIngressoaFormaPagamento();
                    retornoPagamentoVIR = oValeIngresso.getFormaPagamentoVIR();

                    ValeIngressoLog valeIngressoLog;
                    foreach (var oEstrutura in virs)
                    {
                        decimal valor = ValeIngresso.ValorPagoVir(new List<EstruturaTrocaValeIngresso>() { oEstrutura }, oPagamento.EntregaValor, valorTotalConveniencia, valorTotalIngresso);
                        decimal porcentagemVIR = Math.Round((valor * 100) / (valorTotalIngresso + valorTotalConveniencia + oPagamento.EntregaValor), 2);

                        if (porcentagemVIR > 100)
                            porcentagemVIR = 100;
                        else if (porcentagemVIR < 0)
                            porcentagemVIR = 0;

                        vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                        vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oValeIngresso.FormaDePagamentoID; // ID DO PAGAMENTO VIR
                        vendaBilheteriaFormaPagamento.Valor.Valor = valor > totalAtual ? totalAtual : valor;
                        totalAtual = totalAtual - valor;
                        vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagemVIR;
                        vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;

                        string sqlVendaBilheteriaFormaPagamento = String.Empty;

                        vendaBilheteriaFormaPagamento.Dias.Valor = retornoPagamentoVIR.Dias;
                        vendaBilheteriaFormaPagamento.IR.Valor = retornoPagamentoVIR.IR;
                        vendaBilheteriaFormaPagamento.TaxaAdm.Valor = retornoPagamentoVIR.TaxaAdm;
                        vendaBilheteriaFormaPagamento.ValeIngressoID.Valor = oEstrutura.ID;

                        sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();

                        int y = bd.Executar(sqlVendaBilheteriaFormaPagamento);

                        bool p = (y >= 1);
                        if (!p)
                            throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);

                        int alterado = oValeIngresso.AlterarStatus(bd, ValeIngresso.enumStatus.Trocado, oEstrutura.ID);
                        if (alterado != 1)
                            throw new BilheteriaException("A troca do Vale Ingresso gerou um erro, o vale-ingresso utilizado já foi trocado anteriormente.");

                        valeIngressoLog = new ValeIngressoLog();
                        valeIngressoLog.Acao.Valor = ((char)ValeIngressoLog.enumAcao.Trocar).ToString();
                        valeIngressoLog.TimeStamp.Valor = DateTime.Now;
                        valeIngressoLog.ValeIngressoID.Valor = oEstrutura.ID;
                        valeIngressoLog.UsuarioID.Valor = estruturaReservaInternet.UsuarioID;
                        valeIngressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                        valeIngressoLog.VendaBilheteriaItemID.Valor = 0;
                        valeIngressoLog.EmpresaID.Valor = empresaID;
                        valeIngressoLog.CaixaID.Valor = caixaID;
                        valeIngressoLog.LojaID.Valor = estruturaReservaInternet.LojaID;
                        valeIngressoLog.CanalID.Valor = estruturaReservaInternet.CanalID;
                        valeIngressoLog.CodigoTroca.Valor = oPagamento.CodigoTrocaFixo;
                        valeIngressoLog.CodigoBarra.Valor = string.Empty;
                        valeIngressoLog.ClienteNome.Valor = string.Empty;
                        valeIngressoLog.Obs.Valor = string.Empty;

                        if (bd.Executar(valeIngressoLog.StringInserir()) == 0)
                            throw new Exception("Não foi possível trocar o Vale ingresso.");

                    }

                    #endregion
                }
                //VendaBilheteriaFormaPagamento SEM VIR
                //Verifica se está comprando cortesias, caso positivo, não insere na VendaBilheteriaFormaPagamento
                else if (oPagamento.FormaPagamentoID > 0 && valorTotal > 0)
                {
                    #region FormaPagamento sem VIR
                    vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                    vendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    vendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    vendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    vendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oPagamento.FormaPagamentoID;
                    vendaBilheteriaFormaPagamento.Valor.Valor = valorTotal;
                    decimal porcentagemTotal = 100;
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagemTotal;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    vendaBilheteriaFormaPagamento.CartaoID.Valor = oPagamento.CartaoID;
                    vendaBilheteriaFormaPagamento.JurosValor.Valor = oPagamento.JurosValor;
                    vendaBilheteriaFormaPagamento.Coeficiente.Valor = oPagamento.Coeficiente;

                    string sqlVendaBilheteriaFormaPagamento = String.Empty;

                    switch (oPagamento.TipoPagamento)
                    {
                        case EstruturaPagamento.enumTipoPagamento.TEF:
                            if (vendaAsync)
                                break;

                            vendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oPagamento.oSitef.ID;
                            vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oSitef.CodigoRespostaSitefVenda;
                            vendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oPagamento.oSitef.MensagemFinaliza;
                            vendaBilheteriaFormaPagamento.HoraTransacao.Valor = oPagamento.oSitef.HoraTransacao;
                            vendaBilheteriaFormaPagamento.DataTransacao.Valor = oPagamento.oSitef.DataTransacao;
                            vendaBilheteriaFormaPagamento.CodigoIR.Valor = oPagamento.oSitef.CodigoIR;
                            vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oSitef.NumeroAutorizacao;
                            vendaBilheteriaFormaPagamento.NSUHost.Valor = oPagamento.oSitef.NSUHost;
                            vendaBilheteriaFormaPagamento.NSUSitef.Valor = oPagamento.oSitef.NSUSitef;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.oSitef.CupomFiscal;
                            vendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oPagamento.oSitef.DadosConfirmacao;
                            vendaBilheteriaFormaPagamento.Rede.Valor = oPagamento.oSitef.RedeRetorno;
                            vendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oPagamento.oSitef.CodigoRespostaSitefFinaliza;
                            break;
                        case EstruturaPagamento.enumTipoPagamento.Adyen:
                            vendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oPagamento.oAdyen.CodigoAutenticacao;
                            vendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oPagamento.oAdyen.CodigoReferencia;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalAdyen + Environment.NewLine + "Numero de Autenticação: " + oPagamento.oAdyen.CodigoAutenticacao + Environment.NewLine + "Código de referência: " + oPagamento.oAdyen.CodigoReferencia;
                            break;
                        case EstruturaPagamento.enumTipoPagamento.Paypal:
                            vendaBilheteriaFormaPagamento.TokenPayPal.Valor = oPagamento.Token;
                            vendaBilheteriaFormaPagamento.PayerIDPaypal.Valor = oPagamento.PayerID;
                            vendaBilheteriaFormaPagamento.TransactionID.Valor = oPagamento.TransactionID;
                            vendaBilheteriaFormaPagamento.CorrelationID.Valor = oPagamento.CorrelationID;
                            vendaBilheteriaFormaPagamento.Cupom.Valor = oPagamento.NotaFiscalPaypal;
                            break;
                    }
                    sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                    if (bd.Executar(sqlVendaBilheteriaFormaPagamento) == 0)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.", CodMensagemVenda.FormaPagamentoNaoCadastrada);

                    #endregion
                }

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF || oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen)
                {
                    if (oPagamento.IniciouTef)
                    {
                        if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                    else
                    {
                        if (!oPagamento.oAdyen.CapturarPagamento())
                            throw new Exception("Não foi possível consultar o pagamento, por favor, tente novamente.");
                    }
                }

                bd.FinalizarTransacao();

                #region #_#_#_#_#_#_#_# Seguro! Fica fora de transaction! #_#_#_#_#_#_#_#_#_#_#_
                bool SucessoSeguro = true;

                try
                {
                    if (ValorTotalSeguro > 0 && oPagamento.TipoPagamento != EstruturaPagamento.enumTipoPagamento.Paypal)
                    {
                        EstruturaInfoEventoMondial estruturaInfo = new EstruturaInfoEventoMondial();

                        TicketProtectorWebService objService = new TicketProtectorWebService();
                        objService.Url = ConfigurationManager.AppSettings["URLMondial"];

                        string ProductID = ConfigurationManager.AppSettings["ProductID"];
                        decimal productCost = ValorTotalSeguro / ingressosVendidos.Rows.Count;

                        MDLPolicyRequest mdlPolicyRequest = new MDLPolicyRequest();

                        mdlPolicyRequest.CompanyID = ConfigurationManager.AppSettings["CompanyID"];
                        mdlPolicyRequest.AuthenticationCode = ConfigurationManager.AppSettings["AuthenticationCode"];

                        mdlPolicyRequest.CreditCardExpMonth = oPagamento.DataVencimento.Substring(0, 2);
                        mdlPolicyRequest.CreditCardExpYear = "20" + oPagamento.DataVencimento.Substring(2, 2);
                        mdlPolicyRequest.CreditCardHolderName = oClienteSelecionado.Nome.Valor;
                        mdlPolicyRequest.CreditCardNumber = oPagamento.NumeroCartao;
                        mdlPolicyRequest.CreditCardSecurityCode = oPagamento.CodigoSeguranca;

                        int bandeiraID = oCartao.BuscaBandeira(oPagamento.FormaPagamentoID);

                        switch (bandeiraID)
                        {
                            case (int)Cartao.enumBandeira.Visa:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Visa;
                                break;
                            case (int)Cartao.enumBandeira.Mastercard:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Mastercard;
                                break;
                            case (int)Cartao.enumBandeira.Amex:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Amex;
                                break;
                            case (int)Cartao.enumBandeira.Hipercard:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Hipercard;
                                break;
                            default:
                                bandeiraID = (int)Cartao.enumBandeiraMondial.Visa;
                                break;
                        }

                        mdlPolicyRequest.CreditCardType = bandeiraID.ToString(); // 1: VISA; 2: MASTER; 3: AMEX; 5: HIPERCARD
                        mdlPolicyRequest.PaymentTypeID = 1; // 1: Cartão de Crédito; 2: Pagamento automático

                        // Endereço segurado
                        mdlPolicyRequest.CustomerAddresCompliment = oClienteSelecionado.ComplementoCliente.Valor;
                        mdlPolicyRequest.CustomerAddress = oClienteSelecionado.EnderecoCliente.Valor;
                        mdlPolicyRequest.CustomerAddressNumber = oClienteSelecionado.NumeroCliente.Valor;
                        mdlPolicyRequest.CustomerCity = oClienteSelecionado.CidadeCliente.Valor;
                        mdlPolicyRequest.CustomerDistrict = oClienteSelecionado.BairroCliente.Valor;
                        mdlPolicyRequest.CustomerState = oClienteSelecionado.EstadoCliente.Valor;
                        mdlPolicyRequest.CustomerZipCode = oClienteSelecionado.CEPCliente.Valor;
                        mdlPolicyRequest.CustomerBirthDate = DateTime.Parse(oClienteSelecionado.DataNascimento.ToString());

                        // Dados comprador
                        mdlPolicyRequest.CustomerDocumentID = oClienteSelecionado.CPF.Valor;
                        mdlPolicyRequest.CustomerEmail = oClienteSelecionado.Email.Valor;
                        mdlPolicyRequest.CustomerMobilePhone = oClienteSelecionado.Celular.Valor;
                        mdlPolicyRequest.CustomerName = oClienteSelecionado.Nome.Valor;
                        mdlPolicyRequest.CustomerPhone = oClienteSelecionado.Telefone.Valor;

                        // Apólice primária, no caso da Ingresso Rápido, sempre passar vazio
                        mdlPolicyRequest.PrimaryPolicyID = string.Empty;

                        // Taxas
                        mdlPolicyRequest.RateOfConvenience = valorTotalConveniencia;
                        mdlPolicyRequest.RateOfDelivery = 0;

                        // Identificador da compra na Ingresso Rápido
                        mdlPolicyRequest.ReferenceOrderCode = vendaBilheteria.Control.ID.ToString();

                        mdlPolicyRequest.RequestDate = DateTime.Now;
                        mdlPolicyRequest.RequestOriginID = 1; // No caso da Ingresso Rápido, sempre 1
                        mdlPolicyRequest.RequestStatus = 7; // Booking Path (7): Pronto para cobrança do cartão via gateway
                        // Pagamento automático (1): Compras sem pagamento (faturamento inverso)

                        // Valor
                        mdlPolicyRequest.TotalCost = ValorTotalSeguro; // Valor dos produtos * qtde. de segurados

                        // Instanciar cada segurado. Neste exemplo, 2 segurados
                        MDLCustomer[] mdlCustomer = new MDLCustomer[ingressosVendidos.Rows.Count];

                        int cont = 0;
                        int ApresentacaoSetorID = 0;

                        foreach (DataRow item in itensReservados.Rows)
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
                            mdlCustomer[cont].InsuredBirthDate = DateTime.Parse(oClienteSelecionado.DataNascimento.ToString()); // Data Nascimento
                            mdlCustomer[cont].InsuredDocumentID = oClienteSelecionado.CPF.Valor; // CPF
                            mdlCustomer[cont].InsuredID = 0; // No caso da ingresso Rápido, sempre 0
                            mdlCustomer[cont].InsuredName = oClienteSelecionado.Nome.Valor; // Nome do Cliente
                            mdlCustomer[cont].ProductID = ProductID; // Produto Mondial

                            // Dados do Evento
                            mdlCustomer[cont].EventClave = estruturaInfo.ApresentacaoID.ToString(); // Código do Evento. Este é o identificador do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo evento!!
                            mdlCustomer[cont].EventName = estruturaInfo.Evento;
                            mdlCustomer[cont].EventDate = estruturaInfo.Horario;
                            mdlCustomer[cont].EventID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do evento é o campo "EventClave"

                            // Dados do setor
                            mdlCustomer[cont].SectorCode = estruturaInfo.SetorID + "_" + item["PrecoID"]; // Código do Setor. Este é o identificador do setor do evento na Ingresso Rápido. IMPORTANTE sempre informar o mesmo código para o mesmo setor!!
                            mdlCustomer[cont].SectorName = estruturaInfo.Setor;

                            decimal ValorIngressoConveniencia = Convert.ToDecimal(item["Valor"]);
                            mdlCustomer[cont].SectorPrice = ValorIngressoConveniencia;

                            mdlCustomer[cont].EventLocal = estruturaInfo.Local;
                            mdlCustomer[cont].SectorID = 0; // No caso da Ingresso Rápido, sempre 0, pois o identificador do setor é o campo "SectorCode"

                            cont++;
                        }

                        mdlPolicyRequest.CustomerCollection = mdlCustomer;

                        MDLRequestResult[] mdlRequestResult;
                        mdlRequestResult = objService.CreatePolicy(mdlPolicyRequest);

                        new ApoliceMondial().InserirLista(mdlRequestResult, vendaBilheteria.Control.ID);
                    }
                }
                catch (Exception)
                {
                    SucessoSeguro = false;
                }
                #endregion

                try
                {
                    if (celular.Length > 0 && vendaBilheteria.Status.Valor == (string)VendaBilheteria.PAGO && vendaBilheteria.PagamentoProcessado.Valor)
                        new EnviaSMS().EnviaSms(true, vendaBilheteria.Control.ID, celular);
                }
                catch (Exception)
                {
                }

                if (SucessoSeguro)
                    return MontaRetornoVendaInternet(((int)CodMensagemVenda.Sucesso).ToString(), senha, vendaBilheteria.Control.ID, celular, "1");
                else
                    return MontaRetornoVendaInternet(((int)CodMensagemVenda.ErroSeguro).ToString(), senha, vendaBilheteria.Control.ID, celular, "1");
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();

                if (ProcessouCinema && cinema != null && cinema.Length > 0)
                    this.CancelarVendaCinema(cinema, estruturaReservaInternet.GUID, senha, vendaBilheteriaID);

                if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.TEF && ProcessouSitef)
                {
                    if (oPagamento.oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");
                }
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Paypal && ProcessouPayPal)
                    this.Refund(Usuario, Senha, Assinatura, oPagamento.TransactionID, AmbienteTestePayPal);
                else if (oPagamento.TipoPagamento == EstruturaPagamento.enumTipoPagamento.Adyen && ProcessouAdyen)
                    oPagamento.oAdyen.CancelarPagamento();

                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private bool EfetuaPagamentoTef(EstruturaPagamento oPagamento, bool vendaAsync)
        {
            try
            {
                if (vendaAsync)
                {
                    if (oPagamento.oSitef.IniciaConsulta() != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível realizar o pagamento. Por favor tente novamente!");
                }
                else
                {
                    if (oPagamento.oSitef.IniciaSitef() != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível realizar o pagamento. Por favor tente novamente!");

                    oPagamento.IniciouTef = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("O pagamento foi recusado pela operadora de cartão. Por favor, entre em contato com o seu banco ou tente utilizar um outro cartão.\r\n\n" + ex.Message);
            }
        }

        private bool EfetuaPagamentoAdyen(EstruturaPagamento oPagamento, bool vendaAsync)
        {
            try
            {
                if (!oPagamento.oAdyen.EfetuarPagamento())
                    throw new Exception("Não foi possível efetivar o seu pagamento.");

                oPagamento.IniciouAdyen = true;

                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (oPagamento.oAdyen.Refused || oPagamento.oAdyen.Fraud)
                    {
                        oPagamento.TipoPagamento = EstruturaPagamento.enumTipoPagamento.TEF;

                        if (vendaAsync)
                        {
                            if (oPagamento.oSitef.IniciaConsulta() != Sitef.enumRetornoSitef.Ok)
                                throw new Exception("Não foi possível efetivar o seu pagamento.");
                        }
                        else
                            if (oPagamento.oSitef.IniciaSitef() != Sitef.enumRetornoSitef.Ok)
                                throw new Exception("Não foi possível efetivar o seu pagamento.");

                        oPagamento.IniciouTef = true;

                        return true;
                    }
                    else
                        throw new Exception(ex.Message);
                }
                catch (Exception ex1)
                {
                    if (oPagamento.oAdyen.Refused)
                        throw new Exception("O pagamento foi recusado pela operadora de cartão. Por favor, entre em contato com o seu banco ou tente utilizar um outro cartão.\r\n\n" + ex1.Message);
                    else
                        throw new Exception(ex1.Message);
                }
            }
        }

        private void Refund(string Usuario, string Senha, string Assinatura, string TransactionID, bool AmbienteTestePayPal)
        {
            RefundTransaction Refound = PayPalApiFactory.instance.ExpressCheckout(
                Usuario,
                Senha,
                Assinatura
            ).RefundTransaction(TransactionID);

            Refound.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
            Refound.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;

            if (AmbienteTestePayPal)
                Refound.sandbox().execute();
            else
                Refound.execute();

            if (Refound.ResponseNVP.Ack != Ack.SUCCESS)
                throw new Exception("Não foi possivel realizar o estorno do pagamento junto ao PayPal, tente novamente!");
        }

        private void EfetuarVendaCinema(DataRow[] cinema, string guid, string senha, int vendaBilheteriaID)
        {
            List<string> codigos = new List<string>();

            foreach (var linha in cinema)
            {
                if (!codigos.Contains(linha["CodigoProgramacao"].ToString()))
                    codigos.Add(linha["CodigoProgramacao"].ToString());

            }

            if (codigos.Count == 0)
                return;

            List<IRLib.Paralela.Cinema.Programacao> programacoes = IRLib.Paralela.Cinema.Service.getProgramacoes(codigos);

            var itens = new List<IRLib.Paralela.Cinema.VendaItem>();
            decimal total = 0;

            foreach (var linha in cinema)
            {
                var programacao = programacoes.Where(c => c.IDProg == linha["CodigoProgramacao"].ToString()).FirstOrDefault();
                if (programacao == null)
                    continue;

                total += Convert.ToDecimal(programacao.Ingressos.Where(c => c.CodIngress == linha["CodigoCinema"].ToString()).FirstOrDefault().Valor.Replace(".", ","));

                itens.Add(new Cinema.VendaItem()
                {
                    CodBarra = linha["CodigoBarra"].ToString(),
                    CodIngress = linha["CodigoCinema"].ToString(),
                    IDProg = programacao.IDProg,
                    IDProg_CodSala = programacao.CodSala,
                    IDProg_IdFilme = programacao.IDFilme,
                    IDProg_DataSessao = programacao.DataSessao,
                    Qtd = 1,
                });
            }

            IRLib.Paralela.Cinema.Service.setRealizaVenda(IRLib.Paralela.Cinema.SincronizarCinemas.MontarAuth(), new Cinema.Filtros.SetVenda()
            {
                Itens = itens,
                Infos = "Senha=" + senha,
                Mov_Valor = total.ToString("#.00"),
                NT_ID = vendaBilheteriaID.ToString(),
                NT_CodBar = senha,
            });
        }

        private void CancelarVendaCinema(DataRow[] cinema, string guid, string senha, int vendaBilheteriaID)
        {
            List<string> codigos = new List<string>();

            foreach (var linha in cinema)
            {
                if (!codigos.Contains(linha["CodigoProgramacao"].ToString()))
                    codigos.Add(linha["CodigoProgramacao"].ToString());

            }

            if (codigos.Count == 0)
                return;

            List<IRLib.Paralela.Cinema.Programacao> programacoes = IRLib.Paralela.Cinema.Service.getProgramacoes(codigos);
            decimal total = 0;

            foreach (var linha in cinema)
                total += Convert.ToDecimal(programacoes.Where(c => c.IDProg == linha["CodigoProgramacao"].ToString()).FirstOrDefault().Ingressos.Where(c => c.CodIngress == linha["CodigoCinema"].ToString()).FirstOrDefault().Valor.Replace(".", ","));

            IRLib.Paralela.Cinema.Service.setCancelaVenda(IRLib.Paralela.Cinema.SincronizarCinemas.MontarAuth(), new Cinema.Filtros.SetCancela()
            {
                NT_ID = vendaBilheteriaID.ToString(),
                NT_CodBar = senha,
                Motivo = "Cancelamento por falha no processamento de venda.",
                Mov_Valor = total.ToString("#.00"),
            });

        }

        public List<IRLib.Paralela.Assinaturas.Models.Lugar> BuscarLugaresAssinatura(int assinaturaID, int ano, int setorID, int clienteID, int? lugarID)
        {
            BD bd = new BD();
            try
            {
                List<int> lugares = this.LugaresAssinaturaDoCliente(clienteID, ano, setorID, assinaturaID);

                string sql =
                    string.Format(@"
						SELECT
							 l.ID AS LugarID, i.Status, i.Codigo, i.BloqueioID,
							l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, IsNull(pl.Descricao, '') AS DescricaoPerspectiva
						FROM tAssinaturaAno an (NOLOCK)
						INNER JOIN tAssinaturaItem ai (NOLOCK) ON an.ID = ai.AssinaturaAnoID
						INNER JOIN tIngresso i (NOLOCK) ON i.SetorID = ai.SetorID AND i.ApresentacaoID = ai.ApresentacaoID
						INNER JOIN tLugar l (NOLOCK) ON l.ID = i.LugarID
						LEFT JOIN tPerspectivaLugar pl (NOLOCK)ON  pl.ID = l.PerspectivaLugarID
						WHERE an.AssinaturaID = {0} AND an.Ano = '{1}' AND ai.SetorID = {2}
						GROUP BY
							l.ID, i.Status, i.Codigo, i.BloqueioID,
							l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, pl.Descricao
						ORDER BY l.ID", assinaturaID, ano, setorID);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar os lugares do setor informado.");

                List<IRLib.Paralela.Assinaturas.Models.Lugar> listaLugares = new List<Assinaturas.Models.Lugar>();
                IRLib.Paralela.Assinaturas.Models.Lugar lugarAux = new Assinaturas.Models.Lugar();

                string statusAnterior = Ingresso.DISPONIVEL;
                do
                {
                    if (listaLugares.Where(c => c.ID == bd.LerInt("LugarID")).Count() == 0)
                    {
                        lugarAux = new IRLib.Paralela.Assinaturas.Models.Lugar();
                        listaLugares.Add(lugarAux);
                        statusAnterior = IRLib.Paralela.Ingresso.DISPONIVEL;
                        lugarAux.S = IRLib.Paralela.Ingresso.DISPONIVEL;
                    }

                    lugarAux.ID = bd.LerInt("LugarID");
                    lugarAux.C = bd.LerString("Codigo");
                    lugarAux.X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9);
                    lugarAux.Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9);
                    lugarAux.P = bd.LerInt("PerspectivaLugarID").ToString("000000");
                    lugarAux.Q = 1;

                    if (lugarID != null && lugarAux.ID == lugarID)
                        lugarAux.S = Ingresso.RESERVANDO;
                    //Se pertencer ao cliente, usa o reservado
                    else if (lugares.Contains(lugarAux.ID))
                    {
                        lugarAux.S = Ingresso.RESERVADO;
                    }
                    //Aqui caso o Status seja diferente de D (Disponivel) atribui como indisponivel
                    else if (statusAnterior != bd.LerString("Status"))
                        //Este status só serve para informar que este ingresso não esta disponível, não existe utilizaçao para I no mapa.
                        lugarAux.S = Ingresso.IMPRESSO;

                    lugarAux.D = -1;
                    statusAnterior = bd.LerString("Status");

                } while (bd.Consulta().Read());



                return listaLugares;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Paralela.Assinaturas.Models.Lugar> BuscarLugaresBancoIngresso(int apresentacaoID, int setorID, int clienteID)
        {
            BD bd = new BD();
            try
            {
                string sql =
                   string.Format(@"
						
						SELECT DISTINCT
							 IsNull(bi.ClienteID, -1) AS ClienteID, IsNull(big.ID, 0) AS ResgateID, IsNull(big.ClienteID, 0) AS ClienteResgate, i.ID AS IngressoID, l.ID AS LugarID, i.Codigo, i.BloqueioID,
							    l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, IsNull(pl.Descricao, '') AS DescricaoPerspectiva
						FROM tIngresso i (NOLOCK) 
						INNER JOIN tLugar l (NOLOCK) ON l.ID = i.LugarID
						LEFT JOIN tPerspectivaLugar pl (NOLOCK)ON  pl.ID = l.PerspectivaLugarID
						LEFT JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.IngressoID = i.ID
                        LEFT JOIN tAssinaturaBancoIngressoResgate big (NOLOCK) ON bi.ID = big.AssinaturaBancoIngressoID
						WHERE i.ApresentacaoID = {0} AND i.SetorID = {1} 
						ORDER BY l.ID
						
						", apresentacaoID, setorID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem lugares na apresentação e no setor selecionado.");

                List<IRLib.Paralela.Assinaturas.Models.Lugar> listaLugares = new List<Assinaturas.Models.Lugar>();

                do
                {
                    string status = string.Empty;
                    int cliente = bd.LerInt("ClienteID");
                    switch (cliente)
                    {
                        case 0:
                            //Já estão resgatando este ingresso
                            if (bd.LerInt("ResgateID") > 0)
                            {
                                //Se for o proprio cliente que está resgatando, exibe que é dele.
                                if (bd.LerInt("ClienteResgate") == clienteID)
                                    status = Ingresso.RESERVADO;
                                else
                                    status = Ingresso.IMPRESSO;
                            }
                            else
                                status = Ingresso.DISPONIVEL;
                            break;
                        //Não existe registro no banco de ingressos
                        case -1:
                            status = Ingresso.IMPRESSO;
                            break;
                        default:
                            if (cliente == clienteID)
                                //Se pertencer ao cliente, usa o reservado
                                status = Ingresso.RESERVADO;
                            else
                                //Este status só serve para informar que este ingresso não esta disponível, não existe utilizaçao para I no mapa.
                                status = Ingresso.IMPRESSO;
                            break;
                    }
                    listaLugares.Add(new Assinaturas.Models.Lugar
                    {
                        ID = bd.LerInt("LugarID"),
                        C = bd.LerString("Codigo"),
                        X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9),
                        Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9),
                        P = bd.LerInt("PerspectivaLugarID").ToString("000000"),
                        Q = 1,
                        I = bd.LerInt("IngressoID"),
                        S = status
                    });
                } while (bd.Consulta().Read());

                return listaLugares;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<int> LugaresAssinaturaDoCliente(int clienteID, int ano, int setorID, int assinaturaID)
        {
            BD bd = new BD();
            try
            {
                string sql =
                    string.Format(
                    @"
						SELECT
							ac.LugarID
							FROM tAssinaturaCliente ac (NOLOCK)
							INNER JOIN tAssinaturaAno an (NOLOCK) ON ac.AssinaturaAnoID = an.ID
							WHERE ac.ClienteID = {0} AND an.Ano = '{1}' AND ac.SetorID = {2} AND ac.AssinaturaID = {3} AND
							(ac.Acao = '{4}'  OR ac.Acao = '{5}' OR ac.Acao = '{6}' OR ac.Acao = '{7}') AND ac.Status <> '{8}'
					", clienteID, ano, setorID, assinaturaID,
                     (char)AssinaturaCliente.EnumAcao.Aquisicao, (char)AssinaturaCliente.EnumAcao.Renovar,
                     (char)AssinaturaCliente.EnumAcao.EfetivarTroca, (char)AssinaturaCliente.EnumAcao.Trocar,
                     (char)AssinaturaCliente.EnumStatus.Indisponivel);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    return new List<int>();

                List<int> lugares = new List<int>();
                do
                {
                    lugares.Add(bd.LerInt("LugarID"));
                } while (bd.Consulta().Read());

                return lugares;
            }
            finally
            {
                bd.Fechar();

            }
        }
    }
}