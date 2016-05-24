using CTLib;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{

    public partial class BufferInternet
    {
        #region Items do Buffer Sistema
        public BufferList<Evento> lstEvento { get; set; }
        public BufferList<Filme> lstFilme { get; set; }
        public BufferList<FormaPagamentoEvento> lstFormaPagamentoEvento { get; set; }
        public BufferList<Local> lstLocal { get; set; }
        public BufferList<Apresentacao> lstApresentacao { get; set; }
        public BufferList<Setor> lstSetor { get; set; }
        public BufferList<Preco> lstPreco { get; set; }
        public BufferList<PrecoParceiroMidia> lstPrecoParceiroMidia { get; set; }
        public BufferList<Tipo> lstTipo { get; set; }
        public BufferList<Subtipo> lstSubtipo { get; set; }
        public BufferList<TipoSubtipo> lstTipoSubtipo { get; set; }
        public BufferList<TaxaEntrega> lstTaxaEntrega { get; set; }
        public BufferList<EventoTaxaEntrega> lstEventoTaxaEntrega { get; set; }
        public BufferList<Pacote> lstPacote { get; set; }
        public BufferList<NomenclaturaPacote> lstNomenclaturaPacote { get; set; }
        public BufferList<PacoteItem> lstPacoteItem { get; set; }
        public BufferList<Banner> lstBanner { get; set; }
        public BufferList<EventoTipoDestaque> lstEventoTipoDestaque { get; set; }
        public BufferList<ValeIngressoTipo> lstValeIngressoTipo { get; set; }
        public BufferList<PontoVenda> lstPontoVenda { get; set; }
        public BufferList<PontoVendaHorario> lstPontoVendaHorario { get; set; }
        public BufferList<PontoVendaXFormaPgto> lstPontoVendaXFormaPgto { get; set; }
        public BufferList<PontoVendaFormaPgto> lstPontoVendaFormaPgto { get; set; }
        public BufferList<Serie> lstSerie { get; set; }
        public BufferList<SerieItem> lstSerieItem { get; set; }
        public BufferList<FormaPagamentoCotaItem> lstFormaPagamentoCotaItem { get; set; }
        public BufferList<FormaPagamentoSerie> lstFormaPagamentoSerie { get; set; }
        //public BufferList<FormaPagamento> lstFormaPagamento { get; set; }
        public BufferList<CotaItem> lstCotaItem { get; set; }
        public BufferList<VoceSabia> lstVoceSabia { get; set; }
        public BufferList<Faq> lstFaq { get; set; }
        #endregion

        public BufferInternet()
        {
            bool RegisterEntries = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveAllLogs"]);

            this.lstEvento = new BufferList<Evento>("IR_EventoID", enumTables.Evento, true, RegisterEntries);
            this.lstFilme = new BufferList<Filme>("IR_FilmeID", enumTables.Filme, true, RegisterEntries);
            this.lstApresentacao = new BufferList<Apresentacao>("IR_ApresentacaoID", enumTables.Apresentacao, true, RegisterEntries);
            this.lstBanner = new BufferList<Banner>("ID", BufferInternet.enumTables.Banner, true, RegisterEntries);
            this.lstEventoTipoDestaque = new BufferList<EventoTipoDestaque>("IR_EventoTipoDestaqueID", BufferInternet.enumTables.EventoTipoDestaque, true, RegisterEntries);
            this.lstEventoTaxaEntrega = new BufferList<EventoTaxaEntrega>("IR_EventoTaxaEntregaID", enumTables.EventoTaxaEntrega, true, RegisterEntries);
            this.lstFormaPagamentoEvento = new BufferList<FormaPagamentoEvento>("IR_FormaPagamentoEventoID", enumTables.FormaPagamentoEvento, false, RegisterEntries);
            this.lstLocal = new BufferList<Local>("IR_LocalID", enumTables.Local, true, RegisterEntries);
            this.lstPacote = new BufferList<Pacote>("IR_PacoteID", enumTables.Pacote, true, RegisterEntries);
            this.lstNomenclaturaPacote = new BufferList<NomenclaturaPacote>("IR_NomenclaturaPacoteID", enumTables.NomenclaturaPacote, true, RegisterEntries);
            this.lstPacoteItem = new BufferList<PacoteItem>("IR_PacoteItemID", enumTables.PacoteItem, true, RegisterEntries);
            this.lstPreco = new BufferList<Preco>("IR_PrecoID", enumTables.Preco, true, RegisterEntries);
            this.lstPrecoParceiroMidia = new BufferList<PrecoParceiroMidia>("IR_PrecoID", enumTables.Preco, true, RegisterEntries);
            this.lstSetor = new BufferList<Setor>("IR_SetorID", enumTables.Setor, true, RegisterEntries);
            this.lstSubtipo = new BufferList<Subtipo>("IR_SubtipoID", BufferInternet.enumTables.EventoSubtipo, true, RegisterEntries);
            this.lstTaxaEntrega = new BufferList<TaxaEntrega>("IR_TaxaEntregaID", BufferInternet.enumTables.TaxaEntrega, true, RegisterEntries);
            this.lstTipo = new BufferList<Tipo>("IR_TipoID", BufferInternet.enumTables.Tipo, true, RegisterEntries);
            this.lstTipoSubtipo = new BufferList<TipoSubtipo>("ID", BufferInternet.enumTables.Tipos, false, RegisterEntries);
            this.lstValeIngressoTipo = new BufferList<ValeIngressoTipo>("IR_ValeIngressoTipoID", BufferInternet.enumTables.ValeIngressoTipo, true, RegisterEntries);
            this.lstPontoVenda = new BufferList<PontoVenda>("IR_PontoVendaID", enumTables.PontoVenda, true, RegisterEntries);
            this.lstPontoVendaHorario = new BufferList<PontoVendaHorario>("IR_PontoVendaHorarioID", enumTables.PontoVendaHorario, true, RegisterEntries);
            this.lstPontoVendaFormaPgto = new BufferList<PontoVendaFormaPgto>("IR_PontoVendaFormaPgtoID", enumTables.PontoVendaFormaPgto, true, RegisterEntries);
            this.lstPontoVendaXFormaPgto = new BufferList<PontoVendaXFormaPgto>("IR_PontoVendaXFormaPgtoID", enumTables.PontoVendaXFormaPgto, false, RegisterEntries);
            this.lstSerie = new BufferList<Serie>("IR_SerieID", enumTables.Serie, true, RegisterEntries);
            this.lstSerieItem = new BufferList<SerieItem>("IR_SerieItemID", enumTables.SerieItem, true, RegisterEntries);
            //this.lstFormaPagamento = new BufferList<FormaPagamento>("IR_FormaPagamentoID", enumTables.FormaPagamento, true);
            this.lstFormaPagamentoCotaItem = new BufferList<FormaPagamentoCotaItem>("IR_FormaPagamentoCotaItemID", enumTables.FormaPagamentoCotaItem, false, RegisterEntries);
            this.lstFormaPagamentoSerie = new BufferList<FormaPagamentoSerie>("IR_FormaPagamentoSerieID", enumTables.FormaPagamentoSerie, false, RegisterEntries);
            this.lstCotaItem = new BufferList<CotaItem>("IR_CotaItemID", enumTables.CotaItem, true, RegisterEntries);
            this.lstVoceSabia = new BufferList<VoceSabia>("IR_VoceSabia", enumTables.VoceSabia, true, RegisterEntries);
            this.lstFaq = new BufferList<Faq>("IR_Faq", enumTables.Faq, true, RegisterEntries);
        }

        public void Carregar()
        {
            BD bd = new BD();
            try
            {
                this.lstEvento.Clear();
                this.lstFilme.Clear();
                this.lstFormaPagamentoEvento.Clear();
                this.lstLocal.Clear();
                this.lstApresentacao.Clear();
                this.lstSetor.Clear();
                this.lstPreco.Clear();
                this.lstPrecoParceiroMidia.Clear();
                this.lstTipo.Clear();
                this.lstSubtipo.Clear();
                //this.lstTaxaEntrega.Clear();
                //this.lstEventoTaxaEntrega.Clear();
                this.lstPacote.Clear();
                this.lstNomenclaturaPacote.Clear();
                this.lstPacoteItem.Clear();
                this.lstBanner.Clear();
                this.lstEventoTipoDestaque.Clear();
                this.lstValeIngressoTipo.Clear();
                this.lstPontoVenda.Clear();
                this.lstPontoVendaFormaPgto.Clear();
                this.lstPontoVendaHorario.Clear();
                this.lstPontoVendaXFormaPgto.Clear();
                this.lstFormaPagamentoCotaItem.Clear();
                this.lstFormaPagamentoSerie.Clear();
                this.lstCotaItem.Clear();
                this.lstVoceSabia.Clear();
                this.lstFaq.Clear();

                bd.Executar("EXEC PopulaEventosInternet");

                StringBuilder stbSQL = new StringBuilder();
                string sql = string.Empty;

                // Forma de Pagamento Evento
                bd.Consulta(@"SELECT DISTINCT
                            tFormaPagamentoEvento.ID, 
                            tFormaPagamentoEvento.EventoID, 
                            tFormaPagamentoEvento.FormaPagamentoID 
                            FROM EventosInternet (NOLOCK) 
                            INNER JOIN tFormaPagamentoEvento(NOLOCK) ON tFormaPagamentoEvento.EventoID = EventosInternet.EventoID 
                            ORDER BY tFormaPagamentoEvento.ID");

                while (bd.Consulta().Read())
                {
                    lstFormaPagamentoEvento.Add(new FormaPagamentoEvento
                    {
                        ID = bd.LerInt("ID"),
                        EventoID = bd.LerInt("EventoID"),
                        FormaPagamentoID = bd.LerInt("FormaPagamentoID")
                    });
                }

                bd.FecharConsulta();
                // Locais
                bd.Consulta("SELECT DISTINCT LocalID AS ID, Local AS Nome, ImagemLocal, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, BannerPadraoSite , CAST(LocalObs AS VARCHAR(3000)) AS Obs, CAST(ComoChegarInternet AS VARCHAR(3000)) as ComoChegar , EmpresaID , TaxaMaximaEmpresa, Pais, CodigoPraca FROM vwInfoVendaInternet(NOLOCK) WHERE (Publicar = 'T' OR Publicar = 'S') ORDER BY LocalID");
                while (bd.Consulta().Read())
                {
                    lstLocal.Add(new Local
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        Endereco = bd.LerString("Endereco"),
                        CEP = bd.LerString("CEP"),
                        DDDTelefone = bd.LerString("DDDTelefone"),
                        Telefone = bd.LerString("Telefone"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Obs = bd.LerString("OBS"),
                        ComoChegar = bd.LerString("ComoChegar"),
                        BannersPadraoSite = bd.LerBoolean("BannerPadraoSite"),
                        TaxaMaximaEmpresa = bd.LerDecimal("TaxaMaximaEmpresa"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        Pais = bd.LerString("Pais"),
                        Imagem = bd.LerString("ImagemLocal"),
                    });
                }

                // Locais dos Pacotes
                bd.Consulta("SELECT DISTINCT LocalID AS ID, Local AS Nome,ImagemLocal, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, BannerPadraoSite , CAST(LocalObs AS VARCHAR(3000)) AS Obs, CAST(ComoChegarInternet AS VARCHAR(3000)) as ComoChegar , EmpresaID , TaxaMaximaEmpresa, Pais FROM vwInfoVendaPacoteInternet (NOLOCK)");
                while (bd.Consulta().Read())
                {
                    if (lstLocal.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;
                    lstLocal.Add(new Local
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        Endereco = bd.LerString("Endereco"),
                        CEP = bd.LerString("CEP"),
                        DDDTelefone = bd.LerString("DDDTelefone"),
                        Telefone = bd.LerString("Telefone"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Obs = bd.LerString("OBS"),
                        ComoChegar = bd.LerString("ComoChegar"),
                        BannersPadraoSite = bd.LerBoolean("BannerPadraoSite"),
                        TaxaMaximaEmpresa = bd.LerDecimal("TaxaMaximaEmpresa"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        Pais = bd.LerString("Pais"),
                        Imagem = bd.LerString("ImagemLocal"),
                        //CodigoPraca = bd.LerString("CodigoPraca"),
                    });
                }

                bd.FecharConsulta();
                //Eventos
                bd.Consulta("SELECT DISTINCT " +
                    "EventoID AS ID, Evento AS Nome, LocalID,  CAST(Resenha AS VARCHAR(5000)) AS Release, " +
                    "Destaque, PrioridadeDestaque AS Prioridade, " +
                    "ImagemInternet AS Imagem, IsNull(ImagemDestaque, '') as ImagemDestaque , TipoID, SubtipoID, Parcelas, RetiradaBilheteria, " +
                    "EntregaGratuita, EscolherLugarMarcado, " +
                    "Publicar, PublicarSemVendaMotivo, DataAberturaVenda, PalavraChave, ExibeQuantidade, BannerPadraoSite, IsNull(MenorPeriodoEntrega, 0) AS MenorPeriodoEntrega, IsNull(FilmeID, 0) AS FilmeID, PossuiTaxaProcessamento, " +
                    "LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado " +
                    "FROM vwInfoVendaInternet (NOLOCK) " +
                    "WHERE (Publicar = '" + Convert.ToChar(IRLib.Paralela.Evento.PublicarTipo.PublicadoParaVenda).ToString() + "' OR Publicar = '" + Convert.ToChar(IRLib.Paralela.Evento.PublicarTipo.PublicadoSemVenda).ToString() + "') " +
                    "ORDER BY EventoID");

                while (bd.Consulta().Read())
                {
                    lstEvento.Add(new Evento
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        LocalID = bd.LerInt("LocalID"),
                        Release = bd.LerString("Release"),
                        Destaque = bd.LerBoolean("Destaque"),
                        Prioridade = bd.LerInt("Prioridade"),
                        EntregaGratuita = bd.LerBoolean("EntregaGratuita"),
                        DisponivelAvulso = true,
                        Publicar = bd.LerString("Publicar"),
                        PublicarSemVendaMotivo = bd.LerInt("PublicarSemVendaMotivo"),
                        DataAberturaVenda = bd.LerString("DataAberturaVenda"),
                        PalavraChave = bd.LerString("PalavraChave"),
                        TipoID = bd.LerInt("TipoID"),
                        SubTipoID = bd.LerInt("SubTipoID"),
                        Parcelas = bd.LerInt("Parcelas"),
                        RetiradaBilheteria = bd.LerBoolean("RetiradaBilheteria"),
                        Imagem = bd.LerString("Imagem"),
                        EscolherLugarMarcado = bd.LerBoolean("EscolherLugarMarcado"),
                        ExibeQuantidade = bd.LerBoolean("ExibeQuantidade"),
                        BannersPadraoSite = bd.LerBoolean("BannerPadraoSite"),
                        MenorPeriodoEntrega = bd.LerInt("MenorPeriodoEntrega"),
                        FilmeID = bd.LerInt("FilmeID"),
                        ImagemDestaque = bd.LerString("ImagemDestaque"),
                        PossuiTaxaProcessamento = bd.LerBoolean("PossuiTaxaProcessamento"),
                        LimiteMaximoIngressosEvento = bd.LerInt("LimiteMaximoIngressosEvento"),
                        LimiteMaximoIngressosEstado = bd.LerInt("LimiteMaximoIngressosEstado"),
                    });
                }
                bd.FecharConsulta();

                // Eventos Pacotes
                bd.Consulta(@"SELECT DISTINCT 
                                EventoID AS ID, Evento AS Nome, LocalID, CAST(Resenha AS VARCHAR(5000)) AS Release, 
                                Destaque, PrioridadeDestaque AS Prioridade, 
                                ImagemInternet AS Imagem, TipoID, SubtipoID, Parcelas,  
                                RetiradaBilheteria, 
                                EntregaGratuita, 
                                Publicar, PublicarSemVendaMotivo,DataAberturaVenda, PalavraChave, ExibeQuantidade, BannerPadraoSite, IsNull(MenorPeriodoEntrega, 0) AS MenorPeriodoEntrega, PossuiTaxaProcessamento,
                                LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado
                                FROM vwInfoVendaPacoteInternet (NOLOCK) ORDER BY EventoID ");

                while (bd.Consulta().Read())
                {
                    if (lstEvento.Where(c => c.ID == bd.LerInt("ID")).Count() == 0)
                    {
                        lstEvento.Add(new Evento
                        {
                            ID = bd.LerInt("ID"),
                            Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                            LocalID = bd.LerInt("LocalID"),
                            Release = bd.LerString("Release"),
                            TipoID = bd.LerInt("TipoID"),
                            SubTipoID = bd.LerInt("SubtipoID"),
                            Imagem = bd.LerString("Imagem"),
                            Destaque = bd.LerBoolean("Destaque"),
                            Prioridade = bd.LerInt("Prioridade"),
                            Parcelas = bd.LerInt("Parcelas"),
                            RetiradaBilheteria = bd.LerBoolean("RetiradaBilheteria"),
                            EntregaGratuita = bd.LerBoolean("EntregaGratuita"),
                            DisponivelAvulso = false,
                            Publicar = bd.LerString("Publicar"),
                            PublicarSemVendaMotivo = bd.LerInt("PublicarSemVendaMotivo"),
                            DataAberturaVenda = bd.LerString("DataAberturaVenda"),
                            PalavraChave = bd.LerString("PalavraChave"),
                            ExibeQuantidade = bd.LerBoolean("ExibeQuantidade"),
                            BannersPadraoSite = bd.LerBoolean("BannerPadraoSite"),
                            MenorPeriodoEntrega = bd.LerInt("MenorPeriodoEntrega"),
                            PossuiTaxaProcessamento = bd.LerBoolean("PossuiTaxaProcessamento"),
                            LimiteMaximoIngressosEvento = bd.LerInt("LimiteMaximoIngressosEvento"),
                            LimiteMaximoIngressosEstado = bd.LerInt("LimiteMaximoIngressosEstado"),
                        });
                    }
                }
                bd.FecharConsulta();

                //Eventos _ Serie
                bd.Consulta(@"
                    SELECT DISTINCT
                            EventoID AS ID, Evento AS Nome, LocalID, CAST(Resenha AS VARCHAR(5000)) AS Release, 
                                Destaque, PrioridadeDestaque AS Prioridade, 
                                ImagemInternet AS Imagem, TipoID, SubtipoID, Parcelas,  
                                RetiradaBilheteria, 
                                EntregaGratuita, 
                                Publicar, PublicarSemVendaMotivo, DataAberturaVenda, PalavraChave, ExibeQuantidade, BannerPadraoSite , IsNull(MenorPeriodoEntrega, 0) AS MenorPeriodoEntrega
                                FROM vwInfoVendaSerieInternet (NOLOCK) 
                            ORDER BY EventoID");

                while (bd.Consulta().Read())
                {
                    if (lstEvento.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstEvento.Add(new Evento()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        LocalID = bd.LerInt("LocalID"),
                        Release = bd.LerString("Release"),
                        TipoID = bd.LerInt("TipoID"),
                        SubTipoID = bd.LerInt("SubtipoID"),
                        Imagem = bd.LerString("Imagem"),
                        Destaque = bd.LerBoolean("Destaque"),
                        Prioridade = bd.LerInt("Prioridade"),
                        Parcelas = bd.LerInt("Parcelas"),
                        RetiradaBilheteria = bd.LerBoolean("RetiradaBilheteria"),
                        EntregaGratuita = bd.LerBoolean("EntregaGratuita"),
                        DisponivelAvulso = false,
                        Publicar = bd.LerString("Publicar"),
                        PublicarSemVendaMotivo = bd.LerInt("PublicarSemVendaMotivo"),
                        DataAberturaVenda = bd.LerString("DataAberturaVenda"),
                        PalavraChave = bd.LerString("PalavraChave"),
                        ExibeQuantidade = bd.LerBoolean("ExibeQuantidade"),
                        BannersPadraoSite = bd.LerBoolean("BannerPadraoSite"),
                        MenorPeriodoEntrega = bd.LerInt("MenorPeriodoEntrega"),
                    });
                }

                bd.FecharConsulta();

                //Filmes
                bd.Consulta(@"
                               SELECT DISTINCT FilmeID, Filme, Sinopse, Duracao, Idade, IdadeJustificativa, IMDB, Dublado FROM vwInfoVendaInternet WHERE FilmeID IS NOT NULL AND FilmeID > 0  ORDER BY FilmeID
                            ");
                while (bd.Consulta().Read())
                {
                    lstFilme.Add(new Filme()
                    {
                        ID = bd.LerInt("FilmeID"),
                        Nome = bd.LerString("Filme"),
                        Sinopse = bd.LerString("Sinopse"),
                        FilmeID = bd.LerInt("FilmeID"),
                        Dublado = bd.LerBoolean("Dublado"),
                        Duracao = bd.LerInt("Duracao"),
                        Idade = bd.LerInt("Idade"),
                        IdadeJusti = bd.LerString("IdadeJustificativa"),
                        IMDB = bd.LerString("IMDB"),
                    });
                }
                bd.FecharConsulta();

                // Apresentações
                bd.Consulta("SELECT DISTINCT ApresentacaoID AS ID, Apresentacao AS Horario, EventoID, IsNull(CodigoProgramacao, '') AS CodigoProgramacao FROM vwInfoVendaInternet(NOLOCK) WHERE (Publicar = 'T' or Publicar = 'S') ORDER BY EventoID, ApresentacaoID");
                while (bd.Consulta().Read())
                {
                    lstApresentacao.Add(new Apresentacao
                    {
                        ID = bd.LerInt("ID"),
                        Horario = bd.LerString("Horario"),
                        EventoID = bd.LerInt("EventoID"),
                        CodigoProgramacao = bd.LerString("CodigoProgramacao"),
                    });
                }
                bd.FecharConsulta();

                // Apresentacoes Pacotes
                bd.Consulta("SELECT DISTINCT ApresentacaoID AS ID, Horario, EventoID FROM vwInfoVendaPacoteInternet(NOLOCK) ORDER BY ApresentacaoID");
                while (bd.Consulta().Read())
                {
                    if (lstApresentacao.Where(c => c.ID == bd.LerInt("ID")).Count() == 0)
                    {
                        lstApresentacao.Add(new Apresentacao
                        {
                            ID = bd.LerInt("ID"),
                            Horario = bd.LerString("Horario"),
                            EventoID = bd.LerInt("EventoID"),
                        });
                    }
                }
                bd.FecharConsulta();
                //Apresentacoes - Serie
                bd.Consulta("SELECT DISTINCT ApresentacaoID AS ID, Horario, EventoID, Programacao FROM vwInfoVendaSerieInternet (NOLOCK) ORDER BY ApresentacaoID");
                while (bd.Consulta().Read())
                {
                    if (lstApresentacao.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstApresentacao.Add(new Apresentacao()
                    {
                        ID = bd.LerInt("ID"),
                        Horario = bd.LerString("Horario"),
                        EventoID = bd.LerInt("EventoID"),
                        Programacao = bd.LerString("Programacao")
                    });
                }
                bd.FecharConsulta();

                bd.Executar("EXEC PopulaSetoresInternet");

                //Setores
                bd.Consulta(@"
                               SELECT 
                                DISTINCT vw.ID , Nome, LugarMarcado, ApresentacaoID, ISNULL(MIN(tLugar.Quantidade),0) AS QuantidadeMapa, AprovadoPublicacao, PrincipalPrecoID, IsNull(NVendeLugar, 'F') AS NVendeLugar
                                FROM SetoresInternet vw (NOLOCK)  
                                LEFT JOIN tLugar tLugar (NOLOCK) ON tLugar.SetorID = vw.ID AND tLugar.Quantidade > 0 
                                GROUP BY vw.ID, Nome ,LugarMarcado, ApresentacaoID, AprovadoPublicacao, PrincipalPrecoID, NVendeLugar ORDER BY vw.ID");

                while (bd.Consulta().Read())
                {
                    lstSetor.Add(new Setor
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        LugarMarcado = bd.LerString("LugarMarcado"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        QuantidadeMapa = bd.LerInt("QuantidadeMapa"),
                        AprovadoPublicacao = bd.LerBoolean("AprovadoPublicacao"),
                        PrincipalPrecoID = bd.LerInt("PrincipalPrecoID"),
                        NVendeLugar = bd.LerBoolean("NVendeLugar"),
                    });
                }

                bd.FecharConsulta();

                //Setores - Pacote
                bd.Consulta(@"SELECT  
                                DISTINCT vw.SetorID AS ID, Setor AS Nome, LugarMarcado, AprovadoPublicacao, ApresentacaoID, ISNULL(MIN(vw.Quantidade),0) AS QuantidadeMapa
                                FROM vwInfoVendaPacoteInternet vw (NOLOCK)  
                                LEFT JOIN tLugar (NOLOCK) ON tLugar.SetorID= vw.SetorID AND vw.Quantidade > 0 
                                WHERE (Publicar = 'T' OR Publicar = 'S')
                                GROUP BY vw.SetorID, Setor, LugarMarcado, AprovadoPublicacao, ApresentacaoID
                                ORDER BY vw.SetorID");
                int setorID = 0;
                int apresentacaoID = 0;

                while (bd.Consulta().Read())
                {
                    setorID = bd.LerInt("ID");
                    apresentacaoID = bd.LerInt("ApresentacaoID");
                    if (lstSetor.Where(c => c.ID.Equals(setorID) && c.ApresentacaoID.Equals(apresentacaoID)).Count() == 0)
                        lstSetor.Add(new Setor
                        {
                            ID = setorID,
                            Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                            LugarMarcado = bd.LerString("LugarMarcado"),
                            ApresentacaoID = apresentacaoID,
                            QuantidadeMapa = bd.LerInt("QuantidadeMapa"),
                            AprovadoPublicacao = bd.LerBoolean("AprovadoPublicacao"),
                            PrincipalPrecoID = 0,
                        });
                }

                bd.FecharConsulta();

                //Setores - Serie
                bd.Consulta(@"SELECT DISTINCT vw.SetorID AS ID, Setor AS Nome, LugarMarcado, AprovadoPublicacao, ApresentacaoID, ISNULL(MIN(vw.Quantidade),0) AS QuantidadeMapa
                                FROM vwInfoVendaSerieInternet vw (NOLOCK)  
                                LEFT JOIN tLugar (NOLOCK) ON tLugar.SetorID= vw.SetorID AND vw.Quantidade > 0 
                                WHERE (Publicar = 'T' OR Publicar = 'S')
                                GROUP BY vw.SetorID, Setor, LugarMarcado, AprovadoPublicacao, ApresentacaoID
                                ORDER BY vw.SetorID");

                while (bd.Consulta().Read())
                {
                    setorID = bd.LerInt("ID");
                    apresentacaoID = bd.LerInt("ApresentacaoID");
                    if (lstSetor.Where(c => c.ID == setorID && c.ApresentacaoID == apresentacaoID).Count() > 0)
                        continue;

                    lstSetor.Add(new Setor()
                    {
                        ID = setorID,
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        LugarMarcado = bd.LerString("LugarMarcado"),
                        ApresentacaoID = apresentacaoID,
                        QuantidadeMapa = bd.LerInt("QuantidadeMapa"),
                        AprovadoPublicacao = bd.LerBoolean("AprovadoPublicacao"),
                        PrincipalPrecoID = 0,
                    });
                }
                bd.FecharConsulta();

                // Preços
                bd.Consulta("SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID, IsNull(CodigoCinema, '') AS CodigoCinema FROM vwInfoVendaInternet (NOLOCK) WHERE (Publicar = 'T' OR Publicar = 'S') ORDER BY PrecoID");

                while (bd.Consulta().Read())
                {
                    lstPreco.Add(new Preco
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                        QuantidadePorCliente = bd.LerInt("QuantidadePorCliente"),
                        SetorID = bd.LerInt("SetorID"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Pacote = false,
                        CodigoCinema = bd.LerString("CodigoCinema"),
                    });
                }
                bd.FecharConsulta();

                // Precos Pacotes
                bd.Consulta("SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID FROM vwInfoVendaPacoteInternet (NOLOCK) ORDER BY PrecoID");
                while (bd.Consulta().Read())
                {
                    if (lstPreco.Where(c => c.ID == bd.LerInt("ID")).Count() == 0)
                    {
                        lstPreco.Add(new Preco
                        {
                            ID = bd.LerInt("ID"),
                            Nome = bd.LerString("Nome"),
                            Valor = bd.LerDecimal("Valor"),
                            SetorID = bd.LerInt("SetorID"),
                            ApresentacaoID = bd.LerInt("ApresentacaoID"),
                            QuantidadePorCliente = bd.LerInt("QuantidadePorCliente"),
                            Pacote = true,
                        });
                    }
                }
                bd.FecharConsulta();


                bd.Consulta("SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID FROM vwInfoVendaSerieInternet (NOLOCK) ORDER BY PrecoID");
                while (bd.Consulta().Read())
                {
                    if (lstPreco.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstPreco.Add(new Preco()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        SetorID = bd.LerInt("SetorID"),
                        QuantidadePorCliente = bd.LerInt("QuantidadePorCliente"),
                        Serie = true,
                    });
                }
                bd.FecharConsulta();

                // Preços Parceiro Midia
                bd.Consulta("SELECT DISTINCT PrecoID AS ID , Preco AS Nome, Valor, Quantidade, QuantidadePorCliente, SetorID, ApresentacaoID, IsNull(CodigoCinema, '') AS CodigoCinema, ParceiroMidiaID FROM vwInfoVendaInternetParceiroMidia (NOLOCK) WHERE (Publicar = 'T' OR Publicar = 'S') ORDER BY PrecoID");

                while (bd.Consulta().Read())
                {
                    lstPrecoParceiroMidia.Add(new PrecoParceiroMidia
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                        QuantidadePorCliente = bd.LerInt("QuantidadePorCliente"),
                        SetorID = bd.LerInt("SetorID"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Pacote = false,
                        CodigoCinema = bd.LerString("CodigoCinema"),
                        ParceiroMidiaID = bd.LerInt("ParceiroMidiaID"),
                    });
                }
                bd.FecharConsulta();


                //Taxas
                bd.Consulta("SELECT ID, Nome, Valor, Prazo as PrazoEntrega, Estado, ProcedimentoEntrega, PermitirImpressaoInternet FROM tTaxaEntrega (NOLOCK) WHERE RegiaoID = 4 AND Disponivel = 'T' ORDER BY ID");

                while (bd.Consulta().Read())
                {
                    lstTaxaEntrega.Add(new TaxaEntrega
                    {
                        ID = bd.LerInt("ID"),
                        Valor = bd.LerDecimal("Valor"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        Estado = bd.LerString("Estado"),
                        Nome = bd.LerString("Nome"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        PermitirImpressaoInternet = bd.LerBoolean("PermitirImpressaoInternet"),
                    });
                }
                bd.FecharConsulta();


                // Carrega Taxas de Entrega do Evento que estiver ativo para Internet
                bd.Consulta(@"SELECT DISTINCT tEventoTaxaEntrega.ID, tEventoTaxaEntrega.EventoID, tEventoTaxaEntrega.TaxaEntregaID,
                                                tEventoTaxaEntrega.DetalhesEntrega FROM tEventoTaxaEntrega (NOLOCK)
                                                INNER JOIN EventosInternet (NOLOCK) ON EventosInternet.EventoID = tEventoTaxaEntrega.EventoID
                                                ORDER BY tEventoTaxaEntrega.ID");
                while (bd.Consulta().Read())
                {
                    lstEventoTaxaEntrega.Add(new EventoTaxaEntrega
                    {
                        ID = bd.LerInt("ID"),
                        EventoID = bd.LerInt("EventoID"),
                        TaxaEntregaID = bd.LerInt("TaxaEntregaID"),
                        DetalhesEntrega = bd.LerString("DetalhesEntrega"),
                    });
                }
                bd.FecharConsulta();

                // Busca todos os tipos de eventos
                bd.Consulta("SELECT ID, Nome FROM tEventoTipo (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstTipo.Add(new Tipo
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome"))
                    });
                }
                bd.FecharConsulta();

                //Busca todos os Subtipos de eventos
                bd.Consulta("SELECT ID, EventoTipoID, Descricao FROM tEventoSubtipo (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstSubtipo.Add(new Subtipo
                    {
                        ID = bd.LerInt("ID"),
                        TipoID = bd.LerInt("EventoTipoID"),
                        Descricao = bd.LerString("Descricao")
                    });
                }
                bd.FecharConsulta();

                //Busca os TipoSubtipos dos eventos
                bd.Consulta("SELECT ID ,EventoID ,EventoTipoID ,EventoSubtipoID FROM tEventoTipos (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstTipoSubtipo.Add(new TipoSubtipo
                    {
                        ID = bd.LerInt("ID"),
                        EventoID = bd.LerInt("EventoID"),
                        EventoTipoID = bd.LerInt("EventoTipoID"),
                        EventoSubTipoID = bd.LerInt("EventoSubtipoID")
                    });
                }
                bd.FecharConsulta();

                //Pacote 
                bd.Consulta("SELECT DISTINCT PacoteID AS ID, Nome,NomenclaturaPacoteID FROM vwInfoVendaPacoteInternet (NOLOCK) WHERE Horario > '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ORDER BY PacoteID");
                while (bd.Consulta().Read())
                {
                    lstPacote.Add(new Pacote
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        NomenclaturaPacoteID = bd.LerInt("NomenclaturaPacoteID")
                    });
                }
                bd.FecharConsulta();

                //Nomenclatura Pacote

                bd.Consulta("SELECT ID, Nome FROM tNomenclaturaPacote (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstNomenclaturaPacote.Add(new NomenclaturaPacote
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome")
                    });
                }
                bd.FecharConsulta();

                //Pacote Item
                bd.Consulta("SELECT DISTINCT PacoteItemID AS ID, PacoteID, EventoID, PrecoID, QuantidadePacoteItens AS Quantidade FROM vwInfoVendaPacoteInternet (NOLOCK) WHERE Horario > '20070510' ORDER BY PacoteItemID");
                while (bd.Consulta().Read())
                {
                    lstPacoteItem.Add(new PacoteItem
                    {
                        ID = bd.LerInt("ID"),
                        PacoteID = bd.LerInt("PacoteID"),
                        PrecoID = bd.LerInt("PrecoID"),
                        EventoID = bd.LerInt("EventoID"),
                        Quantidade = bd.LerInt("Quantidade")
                    });
                }
                bd.FecharConsulta();

                //Serie
                bd.Consulta(@"SELECT DISTINCT 
                                    SerieID AS ID, Titulo, Nome, 
                                    QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, 
                                    QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao,
                                    Descricao, Regras, Tipo
                                FROM vwInfoVendaSerieInternet (NOLOCK) ORDER BY Nome");
                while (bd.Consulta().Read())
                {
                    lstSerie.Add(new Serie()
                    {
                        ID = bd.LerInt("ID"),
                        Titulo = bd.LerString("Titulo"),
                        Nome = bd.LerString("Nome"),
                        Descricao = bd.LerString("Descricao"),
                        Regras = bd.LerString("Regras"),
                        QuantidadeMinimaGrupo = bd.LerInt("QuantidadeMinimaGrupo"),
                        QuantidadeMaximaGrupo = bd.LerInt("QuantidadeMaximaGrupo"),
                        QuantidadeMaximaApresentacao = bd.LerInt("QuantidadeMaximaApresentacao"),
                        QuantidadeMinimaApresentacao = bd.LerInt("QuantidadeMinimaApresentacao"),
                        QuantidadeMinimaIngressosPorApresentacao = bd.LerInt("QuantidadeMinimaIngressosPorApresentacao"),
                        QuantidadeMaximaIngressosPorApresentacao = bd.LerInt("QuantidadeMaximaIngressosPorApresentacao"),
                        Tipo = bd.LerString("Tipo"),
                    });
                }
                bd.FecharConsulta();

                //SerieItem
                bd.Consulta("SELECT DISTINCT SerieItemID AS ID, SerieID, EventoID, PrecoID, ApresentacaoID, SetorID, Promocional, QuantidadePorPromocional FROM vwInfoVendaSerieInternet (NOLOCK) WHERE Horario > '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ORDER BY SerieItemID");

                while (bd.Consulta().Read())
                {
                    lstSerieItem.Add(new SerieItem()
                    {
                        ID = bd.LerInt("ID"),
                        SerieID = bd.LerInt("SerieID"),
                        EventoID = bd.LerInt("EventoID"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        SetorID = bd.LerInt("SetorID"),
                        PrecoID = bd.LerInt("PrecoID"),
                        Promocional = bd.LerBoolean("Promocional"),
                        QuantidadePorPromocional = bd.LerInt("QuantidadePorPromocional"),
                    });
                }

                //Banners
                bd.Consulta("SELECT ID, Nome, Img, Alt, Url, Target, Localizacao, Posicao, Descricao FROM tBanner (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstBanner.Add(new Banner
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Img = bd.LerString("Img"),
                        Alt = bd.LerString("Alt"),
                        Url = bd.LerString("Url"),
                        Target = bd.LerInt("Target"),
                        Localizacao = bd.LerInt("Localizacao"),
                        Posicao = bd.LerInt("Posicao"),
                        Descricao = bd.LerString("Descricao")
                    });
                }
                bd.FecharConsulta();


                //EventoTipoDestaque
                bd.Consulta("SELECT ID, EventoID, EventoTipoID from tEventoTipoDestaque (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstEventoTipoDestaque.Add(new EventoTipoDestaque
                    {
                        ID = bd.LerInt("ID"),
                        EventoID = bd.LerInt("EventoID"),
                        EventoTipoID = bd.LerInt("EventoTipoID")
                    });
                }
                bd.FecharConsulta();

                //ValeIngressoTipo
                bd.Consulta(@"SELECT ID, Nome, Valor, IsNull(ValidadeData, '') AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, 
                            ProcedimentoTroca, Acumulativo, ReleaseInternet ,ValorPagamento,ValorTipo,TrocaIngresso,TrocaConveniencia,TrocaEntrega
                            FROM tValeIngressoTipo (NOLOCK) WHERE PublicarInternet='T' ORDER BY ID ");

                while (bd.Consulta().Read())
                {
                    lstValeIngressoTipo.Add(new ValeIngressoTipo
                    {
                        ID = bd.LerInt("ID"),
                        Nome = Utilitario.LimparTitulo(bd.LerString("Nome")),
                        Valor = bd.LerDecimal("Valor"),
                        ValidadeData = bd.LerString("ValidadeData"),
                        ValidadeDiasImpressao = bd.LerInt("ValidadeDiasImpressao"),
                        ProcedimentoTroca = bd.LerString("ProcedimentoTroca"),
                        Acumulativo = Convert.ToChar(bd.LerString("Acumulativo")),
                        ReleaseInternet = bd.LerString("ReleaseInternet"),
                        ValorPagamento = bd.LerDecimal("ValorPagamento"),
                        ValorTipo = Convert.ToChar(bd.LerString("ValorTipo")),
                        TrocaIngresso = Convert.ToChar(bd.LerString("TrocaIngresso")),
                        TrocaConveniencia = Convert.ToChar(bd.LerString("TrocaConveniencia")),
                        TrocaEntrega = Convert.ToChar(bd.LerString("TrocaEntrega"))

                    });
                }
                bd.FecharConsulta();

                //PontoVenda
                bd.Consulta(@"SELECT ID, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP FROM tPontoVenda (NOLOCK)");
                while (bd.Consulta().Read())
                {
                    lstPontoVenda.Add(new PontoVenda
                    {
                        ID = bd.LerInt("ID"),
                        Local = bd.LerString("Local"),
                        Nome = bd.LerString("Nome"),
                        Endereco = bd.LerString("Endereco"),
                        Numero = bd.LerString("Numero"),
                        Compl = bd.LerString("Compl"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Bairro = bd.LerString("Bairro"),
                        Obs = bd.LerString("Obs"),
                        Referencia = bd.LerString("Referencia"),
                        CEP = bd.LerString("CEP"),
                    });
                }
                bd.FecharConsulta();

                //PontoVendaXFormaPgto
                bd.Consulta("SELECT ID, PontoVendaFormaPgtoID, PontoVendaID FROM tPontoVendaXFormaPgto (NOLOCK)");
                while (bd.Consulta().Read())
                {
                    lstPontoVendaXFormaPgto.Add(new PontoVendaXFormaPgto
                    {
                        ID = bd.LerInt("ID"),
                        PontoVendaFormaPgtoID = bd.LerInt("PontoVendaFormaPgtoID"),
                        PontoVendaID = bd.LerInt("PontoVendaID"),

                    });
                }
                bd.FecharConsulta();

                //PontoVendaHorario
                bd.Consulta("SELECT ID, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana FROM tPontoVendaHorario (NOLOCK)");
                while (bd.Consulta().Read())
                {
                    lstPontoVendaHorario.Add(new PontoVendaHorario
                    {
                        ID = bd.LerInt("ID"),
                        PontoVendaID = bd.LerInt("PontoVendaID"),
                        HorarioFinal = bd.LerString("HorarioFinal"),
                        HorarioInicial = bd.LerString("HorarioInicial"),
                        DiaSemana = bd.LerInt("DiaSemana")

                    });
                }
                bd.FecharConsulta();

                //PontoVendaFormaPgto
                bd.Consulta("SELECT ID, Nome FROM tPontoVendaFormaPgto (NOLOCK)");
                while (bd.Consulta().Read())
                {
                    lstPontoVendaFormaPgto.Add(new PontoVendaFormaPgto
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome")
                    });
                }
                bd.FecharConsulta();

                bd.Consulta(
                    @"
                    SELECT
	                    DISTINCT	
		                    cifp.ID, cifp.CotaItemID, cifp.FormaPagamentoID
	                    FROM tCota c (NOLOCK)
	                    INNER JOIN tCotaItem ci (NOLOCK) ON ci.CotaID = c.ID
	                    INNER JOIN tCotaItemFormaPagamento cifp (NOLOCK) ON cifp.CotaItemID = ci.ID
	                    LEFT JOIN tApresentacaoSetor aps (NOLOCK) ON aps.CotaID = c.ID
	                    INNER JOIN tApresentacao ap (NOLOCK) ON ap.CotaID = c.ID OR ap.ID = aps.ApresentacaoID
	                    WHERE ap.DisponivelVenda = 'T' AND ap.Horario > '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "'");
                while (bd.Consulta().Read())
                {
                    lstFormaPagamentoCotaItem.Add(new FormaPagamentoCotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        CotaItemID = bd.LerInt("CotaItemID"),
                        FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                    });
                }
                bd.FecharConsulta();

                bd.Consulta(@"	
                        SELECT 
		                    DISTINCT tfs.ID, tfs.SerieID, tfs.FormaPagamentoID
	                    FROM vwInfoVendaSerieInternet vw
	                    INNER JOIN tFormaPagamentoSerie tfs (NOLOCK) ON tfs.SerieID = vw.SerieID");
                while (bd.Consulta().Read())
                {
                    lstFormaPagamentoSerie.Add(new FormaPagamentoSerie()
                    {
                        ID = bd.LerInt("ID"),
                        SerieID = bd.LerInt("SerieID"),
                        FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                    });
                }
                bd.FecharConsulta();

                //CotaItem
                bd.Consulta(@"SELECT DISTINCT ci.ID, ci.ValidaBin, ci.Nominal, ci.TermoSite, ci.ParceiroID, ci.TextoValidacao, ci.ObrigatoriedadeID
                              FROM vwInfoVendaInternet vw 
	                          INNER JOIN tCota c (NOLOCK) ON c.ID = vw.CotaID
	                          INNER JOIN tCotaItem ci (NOLOCK) ON ci.CotaID = c.ID");

                while (bd.Consulta().Read())
                {
                    lstCotaItem.Add(new CotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        ValidaBin = bd.LerBoolean("ValidaBin"),
                        Nominal = bd.LerBoolean("Nominal"),
                        Termo = bd.LerString("TermoSite"),
                        ParceiroID = bd.LerInt("ParceiroID"),
                        TextoValidacao = bd.LerString("TextoValidacao"),
                        ObrigatoriedadeID = bd.LerInt("ObrigatoriedadeID"),

                    });
                }
                bd.FecharConsulta();

                //CotaItem ApresentacaoSetor
                bd.Consulta(@"SELECT DISTINCT ci.ID, ci.ValidaBin, ci.Nominal, ci.TermoSite, ci.ParceiroID, ci.TextoValidacao, ci.ObrigatoriedadeID
	                            FROM vwInfoVendaInternet vw 
	                            INNER JOIN tCota c (NOLOCK) ON c.ID = vw.CotaIDAPS
	                            INNER JOIN tCotaItem ci (NOLOCK) ON ci.CotaID = c.ID");

                while (bd.Consulta().Read())
                {
                    if (lstCotaItem.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstCotaItem.Add(new CotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        ValidaBin = bd.LerBoolean("ValidaBin"),
                        Nominal = bd.LerBoolean("Nominal"),
                        Termo = bd.LerString("TermoSite"),
                        ParceiroID = bd.LerInt("ParceiroID"),
                        TextoValidacao = bd.LerString("TextoValidacao"),
                        ObrigatoriedadeID = bd.LerInt("ObrigatoriedadeID"),
                    });
                }
                bd.FecharConsulta();

                //CotaItem Pacote 
                bd.Consulta(@"SELECT DISTINCT ci.ID, ci.ValidaBin, ci.Nominal, ci.TermoSite, ci.ParceiroID, ci.TextoValidacao, ci.ObrigatoriedadeID
	                            FROM vwInfoVendaPacoteInternet vw 
	                            INNER JOIN tCota c (NOLOCK) ON c.ID = vw.CotaID
	                            INNER JOIN tCotaItem ci (NOLOCK) ON ci.CotaID = c.ID");

                while (bd.Consulta().Read())
                {
                    if (lstCotaItem.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstCotaItem.Add(new CotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        ValidaBin = bd.LerBoolean("ValidaBin"),
                        Nominal = bd.LerBoolean("Nominal"),
                        Termo = bd.LerString("TermoSite"),
                        ParceiroID = bd.LerInt("ParceiroID"),
                        TextoValidacao = bd.LerString("TextoValidacao"),
                        ObrigatoriedadeID = bd.LerInt("ObrigatoriedadeID"),

                    });
                }
                bd.FecharConsulta();

                //CotaItem Pacote ApresentacaoSetor
                bd.Consulta(@"SELECT DISTINCT ci.ID, ci.ValidaBin, ci.Nominal, ci.TermoSite, ci.ParceiroID, ci.TextoValidacao, ci.ObrigatoriedadeID
	                            FROM vwInfoVendaPacoteInternet vw 
	                            INNER JOIN tCota c (NOLOCK) ON c.ID = vw.CotaIDAPS
	                            INNER JOIN tCotaItem ci (NOLOCK) ON ci.CotaID = c.ID");

                while (bd.Consulta().Read())
                {
                    if (lstCotaItem.Where(c => c.ID == bd.LerInt("ID")).Count() > 0)
                        continue;

                    lstCotaItem.Add(new CotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        ValidaBin = bd.LerBoolean("ValidaBin"),
                        Nominal = bd.LerBoolean("Nominal"),
                        Termo = bd.LerString("TermoSite"),
                        ParceiroID = bd.LerInt("ParceiroID"),
                        TextoValidacao = bd.LerString("TextoValidacao"),
                        ObrigatoriedadeID = bd.LerInt("ObrigatoriedadeID"),

                    });
                }
                bd.FecharConsulta();


                //VoceSabia
                bd.Consulta("SELECT ID, Identificacao, Texto FROM tVoceSabia (NOLOCK) ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstVoceSabia.Add(new VoceSabia
                    {
                        ID = bd.LerInt("ID"),
                        Identificacao = bd.LerString("Identificacao"),
                        Texto = bd.LerString("Texto")
                    });
                }
                bd.FecharConsulta();

                //Faq
                bd.Consulta("SELECT f.ID, f.Pergunta, f.Resposta, ft.Nome as FaqTipo , f.Tags , f.Exibicao FROM tFaq as f (NOLOCK) INNER JOIN tFaqTipo as ft on ft.ID = f.FaqTipoID WHERE Exibicao = 'W' OR Exibicao = 'A' ORDER BY ID");
                while (bd.Consulta().Read())
                {
                    lstFaq.Add(new Faq
                    {
                        ID = bd.LerInt("ID"),
                        Pergunta = bd.LerString("Pergunta"),
                        Resposta = bd.LerString("Resposta"),
                        FaqTipo = bd.LerString("FaqTipo"),
                        Tag = bd.LerString("Tags"),
                        Exibicao = bd.LerString("Exibicao")
                    });
                }
                bd.FecharConsulta();
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

        public void CarregarSite()
        {
            DAL oDAL = new DAL();
            IDataReader dr;
            try
            {
                string sql = string.Empty;

                // Forma de Pagamento Evento
                lstFormaPagamentoEvento.lstSite = new BufferList<FormaPagamentoEvento>();
                dr = oDAL.SelectToIDataReader("SELECT IR_FormaPagamentoEventoID AS ID, EventoID, FormaPagamentoID FROM FormaPagamentoEvento (NOLOCK) ORDER BY IR_FormaPagamentoEventoID");
                while (dr.Read())
                {
                    lstFormaPagamentoEvento.lstSite.Add(new FormaPagamentoEvento
                   {
                       ID = Convert.ToInt32(dr["ID"]),
                       EventoID = Convert.ToInt32(dr["EventoID"]),
                       FormaPagamentoID = Convert.ToInt32(dr["FormaPagamentoID"]),
                   });
                }
                dr.Close();
                // Locais
                lstLocal.lstSite = new BufferList<Local>();
                dr = oDAL.SelectToIDataReader("SELECT IR_LocalID AS ID, Nome, Imagem, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, OBS, ComoChegar , IsNull(BannersPadraoSite, 0) AS BannersPadraoSite ,TaxaMaximaEmpresa, IsNull(EmpresaID, 0) AS EmpresaID, Pais, CodigoPraca FROM Local (NOLOCK) ORDER BY IR_LocalID");
                while (dr.Read())
                {
                    lstLocal.lstSite.Add(new Local
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Utilitario.LimparTitulo(Convert.ToString(dr["Nome"])),
                        Endereco = Convert.ToString(dr["Endereco"]),
                        CEP = Convert.ToString(dr["CEP"]),
                        DDDTelefone = Convert.ToString(dr["DDDTelefone"]),
                        Telefone = Convert.ToString(dr["Telefone"]),
                        Cidade = Convert.ToString(dr["Cidade"]),
                        Estado = Convert.ToString(dr["Estado"]),
                        Obs = Convert.ToString(dr["OBS"]),
                        ComoChegar = Convert.ToString(dr["ComoChegar"]),
                        BannersPadraoSite = Convert.ToBoolean(dr["BannersPadraoSite"]),
                        TaxaMaximaEmpresa = Convert.ToDecimal(dr["TaxaMaximaEmpresa"]),
                        EmpresaID = Convert.ToInt32(dr["EmpresaID"]),
                        Pais = dr["Pais"].ToString(),
                        Imagem = dr["Imagem"].ToString(),
                        CodigoPraca = dr["CodigoPraca"].ToString(),
                    });
                }
                dr.Close();
                //Eventos
                lstEvento.lstSite = new BufferList<Evento>();
                dr = oDAL.SelectToIDataReader(@"SELECT IR_EventoID AS ID, Nome, LocalID, Release, Destaque, Prioridade, EntregaGratuita, DisponivelAvulso,
                                            Publicar, PublicarSemVendaMotivo, DataAberturaVenda, PalavraChave, TipoID, SubTipoID, Parcelas, ExibeQuantidade,
                                            RetiradaBilheteria, Imagem, IsNull(EscolherLugarMarcado, 0) AS EscolherLugarMarcado, IsNull(BannersPadraoSite, 0) AS BannersPadraoSite, 
                                            IsNull(MenorPeriodoEntrega, 0) AS MenorPeriodoEntrega, IsNull(FilmeID, 0) AS FilmeID, ImagemDestaque, PossuiTaxaProcessamento, LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado
                                            FROM Evento (NOLOCK) ORDER BY IR_EventoID ");

                while (dr.Read())
                {

                    lstEvento.lstSite.Add(new Evento
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Utilitario.LimparTitulo(Convert.ToString(dr["Nome"])),
                        LocalID = Convert.ToInt32(dr["LocalID"]),
                        Release = Convert.ToString(dr["Release"]),
                        Destaque = Convert.ToBoolean(dr["Destaque"]),
                        Prioridade = Convert.ToInt32(dr["Prioridade"]),
                        EntregaGratuita = Convert.ToBoolean(dr["EntregaGratuita"]),
                        DisponivelAvulso = Convert.ToBoolean(dr["DisponivelAvulso"]),
                        Publicar = Convert.ToString(dr["Publicar"]),
                        PublicarSemVendaMotivo = Convert.ToInt32(dr["PublicarSemVendaMotivo"]),
                        DataAberturaVenda = Convert.ToString(dr["DataAberturaVenda"]),
                        PalavraChave = Convert.ToString(dr["PalavraChave"]),
                        TipoID = Convert.ToInt32(dr["TipoID"]),
                        SubTipoID = Convert.ToInt32(dr["SubTipoID"]),
                        Parcelas = Convert.ToInt32(dr["Parcelas"]),
                        RetiradaBilheteria = Convert.ToBoolean(dr["RetiradaBilheteria"]),
                        Imagem = Convert.ToString(dr["Imagem"]),
                        EscolherLugarMarcado = Convert.ToBoolean(dr["EscolherLugarMarcado"]),
                        ExibeQuantidade = Convert.ToBoolean(dr["ExibeQuantidade"]),
                        BannersPadraoSite = Convert.ToBoolean(dr["BannersPadraoSite"]),
                        MenorPeriodoEntrega = Convert.ToInt32(dr["MenorPeriodoEntrega"]),
                        FilmeID = Convert.ToInt32(dr["FilmeID"]),
                        ImagemDestaque = Convert.ToString(dr["ImagemDestaque"]),
                        PossuiTaxaProcessamento = Convert.ToBoolean(dr["PossuiTaxaProcessamento"]),
                        LimiteMaximoIngressosEvento = Convert.ToInt32(dr["LimiteMaximoIngressosEvento"]),
                        LimiteMaximoIngressosEstado = Convert.ToInt32(dr["LimiteMaximoIngressosEstado"])
                    });
                }
                dr.Close();

                //Filmes
                lstFilme.lstSite = new BufferList<Filme>();
                dr = oDAL.SelectToIDataReader(@"
                            SELECT IR_FilmeID AS ID, Nome, Sinopse, Duracao, Idade, IdadeJustificativa, Dublado, IMDB, FilmeID FROM Filme
                                    ");
                while (dr.Read())
                    lstFilme.lstSite.Add(new Filme()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = dr["Nome"].ToString(),
                        Sinopse = dr["Sinopse"].ToString(),
                        Duracao = Convert.ToInt32(dr["Duracao"]),
                        Idade = Convert.ToInt32(dr["Idade"]),
                        IdadeJusti = dr["IdadeJustificativa"].ToString(),
                        Dublado = Convert.ToBoolean(dr["Dublado"]),
                        IMDB = dr["IMDB"].ToString(),
                        FilmeID = Convert.ToInt32(dr["FilmeID"])
                    });
                dr.Close();

                // Apresentações
                lstApresentacao.lstSite = new BufferList<Apresentacao>();
                dr = oDAL.SelectToIDataReader("SELECT IR_ApresentacaoID AS ID, Horario, EventoID, Programacao, IsNull(CodigoProgramacao, '') AS CodigoProgramacao FROM Apresentacao (NOLOCK) IR_ApresentacaoID");
                while (dr.Read())
                {
                    lstApresentacao.lstSite.Add(new Apresentacao
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Horario = Convert.ToString(dr["Horario"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        Programacao = dr["Programacao"].ToString(),
                        CodigoProgramacao = dr["CodigoProgramacao"].ToString(),
                    });
                }
                dr.Close();

                //Setores
                lstSetor.lstSite = new BufferList<Setor>();
                dr = oDAL.SelectToIDataReader("SELECT IR_SetorID AS ID, Nome, LugarMarcado, ApresentacaoID, QuantidadeMapa, IsNull(AprovadoPublicacao, 0) AS AprovadoPublicacao, IsNull(PrincipalPrecoID, 0) AS PrincipalPrecoID, IsNull(NVendeLugar, 0) AS NVendeLugar FROM Setor (NOLOCK) ORDER BY IR_SetorID");
                while (dr.Read())
                {
                    lstSetor.lstSite.Add(new Setor
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        LugarMarcado = Convert.ToString(dr["LugarMarcado"]),
                        ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"]),
                        QuantidadeMapa = Convert.ToInt32(dr["QuantidadeMapa"]),
                        AprovadoPublicacao = Convert.ToBoolean(dr["AprovadoPublicacao"]),
                        PrincipalPrecoID = Convert.ToInt32(dr["PrincipalPrecoID"]),
                        NVendeLugar = Convert.ToBoolean(dr["NVendeLugar"]),
                    });
                }
                dr.Close();

                // Preços
                lstPreco.lstSite = new BufferList<Preco>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PrecoID AS ID, Nome, Valor, QuantidadePorCliente, SetorID, ApresentacaoID, Pacote, Serie, CodigoCinema FROM Preco (NOLOCK) IR_PrecoID");
                while (dr.Read())
                {
                    lstPreco.lstSite.Add(new Preco
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        QuantidadePorCliente = Convert.ToInt32(dr["QuantidadePorCliente"]),
                        SetorID = Convert.ToInt32(dr["SetorID"]),
                        ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"]),
                        Pacote = Convert.ToBoolean(dr["Pacote"]),
                        Serie = Convert.ToBoolean(dr["Serie"]),
                        CodigoCinema = dr["CodigoCinema"].ToString(),
                    });
                }
                dr.Close();

                // Preços Parceiro Midia
                lstPrecoParceiroMidia.lstSite = new BufferList<PrecoParceiroMidia>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PrecoID AS ID, Nome, Valor, SetorID, ApresentacaoID, CodigoCinema, ParceiroMidiaID FROM PrecoParceiroMidia (NOLOCK) IR_PrecoID");
                while (dr.Read())
                {
                    lstPrecoParceiroMidia.lstSite.Add(new PrecoParceiroMidia
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        QuantidadePorCliente = 0,
                        SetorID = Convert.ToInt32(dr["SetorID"]),
                        ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"]),
                        Pacote = false,
                        Serie = false,
                        CodigoCinema = dr["CodigoCinema"].ToString(),
                        ParceiroMidiaID = Convert.ToInt32(dr["ParceiroMidiaID"]),
                    });
                }
                dr.Close();

                //Taxas
                lstTaxaEntrega.lstSite = new BufferList<TaxaEntrega>();
                dr = oDAL.SelectToIDataReader("SELECT IR_TaxaEntregaID AS ID, Nome, Valor, PrazoEntrega, Estado, ProcedimentoEntrega, PermitirImpressaoInternet FROM TaxaEntrega (NOLOCK) ORDER BY IR_TaxaEntregaID");
                while (dr.Read())
                {
                    lstTaxaEntrega.lstSite.Add(new TaxaEntrega
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]),
                        Estado = Convert.ToString(dr["Estado"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        ProcedimentoEntrega = Convert.ToString(dr["ProcedimentoEntrega"]),
                        PermitirImpressaoInternet = Convert.ToBoolean(dr["PermitirImpressaoInternet"]),
                    });
                }
                dr.Close();

                //EventoTaxaEntrega 
                lstEventoTaxaEntrega.lstSite = new BufferList<EventoTaxaEntrega>();
                dr = oDAL.SelectToIDataReader("SELECT IR_EventoTaxaEntregaID AS ID, EventoID, TaxaEntregaID, DetalhesEntrega FROM EventoTaxaEntrega (NOLOCK) ORDER BY IR_EventoTaxaEntregaID");
                while (dr.Read())
                {
                    lstEventoTaxaEntrega.lstSite.Add(new EventoTaxaEntrega
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        TaxaEntregaID = Convert.ToInt32(dr["TaxaEntregaID"]),
                        DetalhesEntrega = Convert.ToString(dr["DetalhesEntrega"]),
                    });
                }
                dr.Close();

                // Tipo
                lstTipo.lstSite = new BufferList<Tipo>();
                dr = oDAL.SelectToIDataReader("SELECT IR_TipoID AS ID, Nome, Obs FROM Tipo (NOLOCK) ORDER BY IR_TipoID");
                while (dr.Read())
                {
                    lstTipo.lstSite.Add(new Tipo
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Utilitario.LimparTitulo(Convert.ToString(dr["Nome"]))
                    });
                }
                dr.Close();

                //Subtipo
                lstSubtipo.lstSite = new BufferList<Subtipo>();
                dr = oDAL.SelectToIDataReader("SELECT IR_SubtipoID AS ID, TipoID AS EventoTipoID, Descricao FROM EventoSubtipo (NOLOCK) ORDER BY IR_SubtipoID");
                while (dr.Read())
                {
                    lstSubtipo.lstSite.Add(new Subtipo
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        TipoID = Convert.ToInt32(dr["EventoTipoID"]),
                        Descricao = Convert.ToString(dr["Descricao"])
                    });
                }
                dr.Close();

                //TipoSubtipo AKA Tipos
                lstTipoSubtipo.lstSite = new BufferList<TipoSubtipo>();
                dr = oDAL.SelectToIDataReader("SELECT ID, EventoID, EventoTipoID, EventoSubtipoID FROM Tipos (NOLOCK) ORDER BY ID");
                while (dr.Read())
                {
                    lstTipoSubtipo.lstSite.Add(new TipoSubtipo
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        EventoTipoID = Convert.ToInt32(dr["EventoTipoID"]),
                        EventoSubTipoID = Convert.ToInt32(dr["EventoSubtipoID"])
                    });
                }
                dr.Close();

                //Pacote 
                lstPacote.lstSite = new BufferList<Pacote>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PacoteID AS ID, Nome, ISNULL(NomenclaturaPacoteID,0) AS NomenclaturaPacoteID FROM Pacote (NOLOCK) ORDER BY IR_PacoteID");
                while (dr.Read())
                {
                    lstPacote.lstSite.Add(new Pacote
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        NomenclaturaPacoteID = Convert.ToInt32(dr["NomenclaturaPacoteID"])
                    });
                }
                dr.Close();


                //Nomenclatura Pacote 
                lstNomenclaturaPacote.lstSite = new BufferList<NomenclaturaPacote>();
                dr = oDAL.SelectToIDataReader("SELECT IR_NomenclaturaPacoteID AS ID, Nome FROM NomenclaturaPacote (NOLOCK) ORDER BY IR_NomenclaturaPacoteID");
                while (dr.Read())
                {
                    lstNomenclaturaPacote.lstSite.Add(new NomenclaturaPacote
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"])
                    });
                }
                dr.Close();

                //Pacote Item
                lstPacoteItem.lstSite = new BufferList<PacoteItem>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PacoteItemID AS ID, PacoteID, EventoID, PrecoID, Quantidade FROM PacoteItem (NOLOCK) ORDER BY IR_PacoteItemID");
                while (dr.Read())
                {
                    lstPacoteItem.lstSite.Add(new PacoteItem
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        PacoteID = Convert.ToInt32(dr["PacoteID"]),
                        PrecoID = Convert.ToInt32(dr["PrecoID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        Quantidade = Convert.ToInt32(dr["Quantidade"])
                    });
                }
                dr.Close();

                //Serie
                lstSerie.lstSite = new BufferList<Serie>();
                dr = oDAL.SelectToIDataReader(@"SELECT IR_SerieID AS ID, Titulo, Nome, 
                        QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, 
                        QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao,
                        Descricao, Regras, Tipo
                        FROM Serie (NOLOCK)");
                while (dr.Read())
                {
                    lstSerie.lstSite.Add(new Serie()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Titulo = dr["Titulo"].ToString(),
                        Nome = dr["Nome"].ToString(),
                        QuantidadeMinimaGrupo = Convert.ToInt32(dr["QuantidadeMinimaGrupo"]),
                        QuantidadeMaximaGrupo = Convert.ToInt32(dr["QuantidadeMaximaGrupo"]),
                        QuantidadeMinimaApresentacao = Convert.ToInt32(dr["QuantidadeMinimaApresentacao"]),
                        QuantidadeMaximaApresentacao = Convert.ToInt32(dr["QuantidadeMaximaApresentacao"]),
                        QuantidadeMinimaIngressosPorApresentacao = Convert.ToInt32(dr["QuantidadeMinimaIngressosPorApresentacao"]),
                        QuantidadeMaximaIngressosPorApresentacao = Convert.ToInt32(dr["QuantidadeMaximaIngressosPorApresentacao"]),
                        Regras = dr["Regras"].ToString(),
                        Descricao = dr["Descricao"].ToString(),
                        Tipo = dr["Tipo"].ToString()
                    });
                }
                dr.Close();

                //SerieItem
                lstSerieItem.lstSite = new BufferList<SerieItem>();
                dr = oDAL.SelectToIDataReader("SELECT IR_SerieItemID AS ID, SerieID, EventoID, ApresentacaoID, SetorID, PrecoID, Promocional, QuantidadePorPromocional FROM SerieItem (NOLOCK)");
                while (dr.Read())
                {
                    lstSerieItem.lstSite.Add(new SerieItem()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        SerieID = Convert.ToInt32(dr["SerieID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"]),
                        SetorID = Convert.ToInt32(dr["SetorID"]),
                        PrecoID = Convert.ToInt32(dr["PrecoID"]),
                        Promocional = Convert.ToBoolean(dr["Promocional"]),
                        QuantidadePorPromocional = Convert.ToInt32(dr["QuantidadePorPromocional"]),
                    });
                }
                dr.Close();

                //Banners
                lstBanner.lstSite = new BufferList<Banner>();
                dr = oDAL.SelectToIDataReader("SELECT ID, Nome, Img, Alt, Url, Target, Localizacao, Posicao, Descricao FROM Banner (NOLOCK) ORDER BY ID");
                while (dr.Read())
                {
                    lstBanner.lstSite.Add(new Banner
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Img = Convert.ToString(dr["Img"]),
                        Alt = Convert.ToString(dr["Alt"]),
                        Url = Convert.ToString(dr["Url"]),
                        Target = Convert.ToInt32(dr["Target"]),
                        Localizacao = Convert.ToInt32(dr["Localizacao"]),
                        Posicao = Convert.ToInt32(dr["Posicao"]),
                        Descricao = Convert.ToString(dr["Descricao"])
                    });
                }
                dr.Close();

                //EventoTipoDestaque
                lstEventoTipoDestaque.lstSite = new BufferList<EventoTipoDestaque>();
                dr = oDAL.SelectToIDataReader("SELECT IR_EventoTipoDestaqueID AS ID, EventoID, EventoTipoID from EventoTipoDestaque (NOLOCK) ORDER BY ID");
                while (dr.Read())
                {
                    lstEventoTipoDestaque.lstSite.Add(new EventoTipoDestaque
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        EventoTipoID = Convert.ToInt32(dr["EventoTipoID"])
                    });
                }
                dr.Close();


                //ValeIngressoTipo
                lstValeIngressoTipo.lstSite = new BufferList<ValeIngressoTipo>();
                dr = oDAL.SelectToIDataReader(@"SELECT IR_ValeIngressoTipoID AS ID, Nome, Valor, ValidadeData, ValidadeDiasImpressao, 
                                                ProcedimentoTroca, Acumulativo, ReleaseInternet,ValorPagamento,
                                                ValorTipo,TrocaIngresso,TrocaConveniencia,TrocaEntrega 
                                                FROM ValeIngressoTipo ORDER BY IR_ValeIngressoTipoID");
                while (dr.Read())
                {
                    lstValeIngressoTipo.lstSite.Add(new ValeIngressoTipo
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        ValidadeData = Convert.ToString(dr["ValidadeData"]),
                        ValidadeDiasImpressao = Convert.ToInt32(dr["ValidadeDiasImpressao"]),
                        ProcedimentoTroca = Convert.ToString(dr["ProcedimentoTroca"]),
                        Acumulativo = Convert.ToChar(Convert.ToString(dr["Acumulativo"])),
                        ReleaseInternet = Convert.ToString(dr["ReleaseInternet"]),
                        ValorPagamento = Convert.ToDecimal(dr["ValorPagamento"]),
                        ValorTipo = Convert.ToChar(dr["ValorTipo"]),
                        TrocaIngresso = Convert.ToChar(Convert.ToString(dr["TrocaIngresso"])),
                        TrocaConveniencia = Convert.ToChar(Convert.ToString(dr["TrocaConveniencia"])),
                        TrocaEntrega = Convert.ToChar(Convert.ToString(dr["TrocaEntrega"]))

                    });
                }
                dr.Close();

                lstPontoVenda.lstSite = new BufferList<PontoVenda>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PontoVendaID AS ID, Local, Nome, Endereco, Numero, Compl, Cidade, Estado, Bairro, Obs, Referencia, CEP FROM PontoVenda (NOLOCK) ");
                while (dr.Read())
                {
                    lstPontoVenda.lstSite.Add(new PontoVenda
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                        Local = Convert.ToString(dr["Local"]),
                        Endereco = Convert.ToString(dr["Endereco"]),
                        Numero = Convert.ToString(dr["Numero"]),
                        Compl = Convert.ToString(dr["Compl"]),
                        Cidade = Convert.ToString(dr["Cidade"]),
                        Estado = Convert.ToString(dr["Estado"]),
                        Bairro = Convert.ToString(dr["Bairro"]),
                        Obs = Convert.ToString(dr["Obs"]),
                        Referencia = Convert.ToString(dr["Referencia"]),
                        CEP = Convert.ToString(dr["CEP"])
                    });
                }
                dr.Close();

                //PontoVendaHorario
                lstPontoVendaHorario.lstSite = new BufferList<PontoVendaHorario>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PontoVendaHorarioID AS ID, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana FROM PontoVendaHorario (NOLOCK)");
                while (dr.Read())
                {
                    lstPontoVendaHorario.lstSite.Add(new PontoVendaHorario
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        PontoVendaID = Convert.ToInt32(dr["PontoVendaID"]),
                        HorarioInicial = Convert.ToString(dr["HorarioInicial"]),
                        HorarioFinal = Convert.ToString(dr["HorarioFinal"]),
                        DiaSemana = Convert.ToInt32(dr["DiaSemana"]),
                    });
                }
                dr.Close();

                //PontoVendaFormaPgto
                lstPontoVendaFormaPgto.lstSite = new BufferList<PontoVendaFormaPgto>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PontoVendaFormaPgtoID AS ID, Nome FROM PontoVendaFormaPgto (NOLOCK)");

                while (dr.Read())
                {
                    lstPontoVendaFormaPgto.lstSite.Add(new PontoVendaFormaPgto
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nome = Convert.ToString(dr["Nome"]),
                    });
                }
                dr.Close();

                //PontoVendaXFormaPgto
                lstPontoVendaXFormaPgto.lstSite = new BufferList<PontoVendaXFormaPgto>();
                dr = oDAL.SelectToIDataReader("SELECT IR_PontoVendaXFormaPgtoID AS ID, PontoVendaFormaPgtoID, PontoVendaID FROM PontoVendaXFormaPgto (NOLOCK)");
                while (dr.Read())
                {
                    lstPontoVendaXFormaPgto.lstSite.Add(new PontoVendaXFormaPgto
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        PontoVendaID = Convert.ToInt32(dr["PontoVendaID"]),
                        PontoVendaFormaPgtoID = Convert.ToInt32(dr["PontoVendaFormaPgtoID"]),
                    });
                }
                dr.Close();

                lstFormaPagamentoCotaItem.lstSite = new BufferList<FormaPagamentoCotaItem>();
                dr = oDAL.SelectToIDataReader("SELECT IR_FormaPagamentoCotaItemID AS ID, CotaItemID, FormaPagamentoID FROM FormaPagamentoCotaItem ");
                while (dr.Read())
                    lstFormaPagamentoCotaItem.lstSite.Add(new FormaPagamentoCotaItem()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        CotaItemID = Convert.ToInt32(dr["CotaItemID"]),
                        FormaPagamentoID = Convert.ToInt32(dr["FormaPagamentoID"]),
                    });
                dr.Close();

                lstFormaPagamentoSerie.lstSite = new BufferList<FormaPagamentoSerie>();
                dr = oDAL.SelectToIDataReader("SELECT IR_FormaPagamentoSerieID AS ID, SerieID, FormaPagamentoID FROM FormaPagamentoSerie ");
                while (dr.Read())
                    lstFormaPagamentoSerie.lstSite.Add(new FormaPagamentoSerie()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        SerieID = Convert.ToInt32(dr["SerieID"]),
                        FormaPagamentoID = Convert.ToInt32(dr["FormaPagamentoID"]),
                    });
                dr.Close();

                lstCotaItem.lstSite = new BufferList<CotaItem>();
                dr = oDAL.SelectToIDataReader(
                            @"SELECT 
                                IR_CotaItemID AS ID, ValidaBin, Nominal, Termo, ParceiroID, TextoValidacao, ObrigatoriedadeID
                               FROM CotaItem (NOLOCK)");

                while (dr.Read())
                    lstCotaItem.lstSite.Add(new CotaItem()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        ValidaBin = Convert.ToBoolean(dr["ValidaBin"]),
                        Nominal = Convert.ToBoolean(dr["Nominal"]),
                        Termo = dr["Termo"].ToString(),
                        ParceiroID = Convert.ToInt32(dr["ParceiroID"]),
                        TextoValidacao = Convert.ToString(dr["TextoValidacao"]),
                        ObrigatoriedadeID = Convert.ToInt32(dr["ObrigatoriedadeID"]),
                    });

                dr.Close();

                //VoceSabia
                lstVoceSabia.lstSite = new BufferList<VoceSabia>();
                dr = oDAL.SelectToIDataReader("SELECT ID, Identificacao, Texto FROM VoceSabia (NOLOCK) ORDER BY ID");
                while (dr.Read())
                {
                    lstVoceSabia.lstSite.Add(new VoceSabia
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Identificacao = Convert.ToString(dr["Identificacao"]),
                        Texto = Convert.ToString(dr["Texto"])
                    });
                }
                dr.Close();

                //Faq
                lstFaq.lstSite = new BufferList<Faq>();
                dr = oDAL.SelectToIDataReader("SELECT IR_Faq AS ID, Pergunta, Resposta, FaqTipo, Tags , Exibicao FROM Faq (NOLOCK) ORDER BY ID");
                while (dr.Read())
                {
                    lstFaq.lstSite.Add(new Faq
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Pergunta = Convert.ToString(dr["Pergunta"]),
                        Resposta = Convert.ToString(dr["Resposta"]),
                        FaqTipo = Convert.ToString(dr["FaqTipo"]),
                        Tag = Convert.ToString(dr["Tags"]),
                        Exibicao = Convert.ToString(dr["Exibicao"])
                    });
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}