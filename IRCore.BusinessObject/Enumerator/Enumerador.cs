using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Enumerator
{
    /// <summary>
    /// Enumeradors dos tipos de mensagens
    /// do Admin
    /// </summary>
    ///
    public enum enumConfiguracaoBO
    {
        usuarioIdSistema,
        lojaIdSistema,
        empresaIdSistema,
        voucherTempoMinExpiracao,
        chaveCriptografiaLogin,
        caminhoEventoImagens,
        caminhoEventoCompartilhar,
        caminhoMapaEsquematico,
        caminhoLugarImagem,
        caminhoSetorFundo,
        caminhoPerspectivaLugarImagem,
        caminhoLocaisImagem,
        caminhoDestaque,
        caminhoEventoMidia,
        caminhoParceiroLogo,
        assinaturaTipoImagemUpload,
        logisticaExcelUpload,
        voucherCSVUpload,
        destaqueUpload,
        parceiroLogoUpload,
        setorFundoUpload,
        eventoDiasUteis,
        eventoFinaisDeSemana,
        eventoMidiaUpload,
        eventoImagemUpload,
        itensPorPaginaCalendarioHome,
        itensPorPaginaListaEventos,
        limiteEventosRelacionados,
        limiteLocaisRelacionados,
        tempoExpReservaCarrinho,
        limiteCarrinho,
        ChaveCriptografiaToken,
        URLImpressao,
        CanaisInternet,
        CanalInternet,
        CanalPOS,
        InternetLojaID,
        POSLojaID,
        InternetUsuarioID,
        POSUsuarioID,
        InicioAmex,
        FimAmex,
        ValorAmex,
        IngressosPorVoucher,
        caminhoAssinaturaTiposImagem,
        PathSalvarPlanilhas
    }
    public enum enumEventoImagemTamanho
    {
        box,
        lista,
        destaque

    }
    public enum enumTipoDias
    {
        [Description("todos os dias")]
        todosDias,
        [Description("dias úteis")]
        diasUteis,
        [Description("finais de semana")]
        finaisDeSemana


    }
    public enum enumTipoPeriodo
    {
        [Description("esta semana")]
        estaSemana = 7,
        [Description("próximos 15 dias")]
        proximos15dias = 15,
        [Description("próximos 30 dias")]
        proximos30dias = 30,
        [Description("próximos 3 meses")]
        proximos3meses = 90
    }
    public enum enumAutenticacaoTipo
    {
        irUsuarioCompartilhado,
        irClienteCompartilhado,
        irClienteMediaPartner
    }
    public enum enumTipoMensagem
    {
        nula,
        info,
        erro,
        sucesso
    }
    public enum enumClienteException
    {
        nenhum,
        usuarioNaoEncontrado,
        senhaInvalida,
        emailInvalido,
        usuarioBloqueado,
        usuarioInativo,
        facebookConection,
        accountKitConection,
        usuarioJaCadastradoComEmail,
        usuarioJaCadastradoComCPF,
        usuarioJaCadastradoComFacebookUserID,
        usuarioCadastradoAccountKit,
        osespNaoEncontrado,
        biletoOffline
    }
    public enum enumUsuarioException
    {
        nenhum,
        usuarioNaoEncontrado,
        senhaInvalida,
        usuarioBloqueado,
        usuarioForaDataValidade,
        usuarioJaCadastradoComLogin
    }
    public enum enumStatusCota
    {
        SemCotas,
        CotasNaoBinVerificadas,
        CotasNaoBinNaoVerificadasSemNominal,
        CotasNaoBinNaoVerificadasComNominal
    }
    public enum enumRastreioStatus
    {
        ManipulacaoSedex = 'X',
        ManipulacaoFlash = 'L',
        EntregueViaSedex = 'S',
        EntregueViaMensageiro = 'F',
        Entregue = 'E'
    }
    public enum enumPerfilNome
    {
        todos,
        master,
        bilheteiro,
        [Description("Controle de Acesso")]
        controleAcesso,
        estoque,
        financeiro,
        [Description("Implantar Evento")]
        implantarEvento,
        logistica,
        mediaPartner,
        regras,
        [Description("Implantar Evento Master")]
        implantarEventoMaster,
        [Description("SAC Operador,Novo SAC Operador,SAC Supervisor,Novo SAC Supervisor")]
        sac,
        [Description("SAC Operador,Novo SAC Operador")]
        sacOperador,
        [Description("SAC Supervisor,Novo SAC Supervisor")]
        sacSupervisor,
        [Description("Segurança")]
        seguranca,
        [Description("SegurançaEspecial")]
        segurancaEspecial,
        supervisor,
        [Description("Supervisor A&B")]
        supervisorAB,
        [Description("Supervisor Logistica")]
        supervisorLogistica,
        [Description("Supervisor WEB")]
        supervisorWEB,
        [Description("Vendas A&B")]
        vendasAB,
        [Description("Media Partner - Relatórios")]
        mediaPartnerRelatorio
    }
    public enum enumPerfilTipo
    {
        todos,
        especial,
        regional,
        empresa,
        canal,
        local,
        evento
    }
    public enum enumAPIRele
    {
        todos,
        master,
        cliente,
        usuario,
        email,
        config,
        venda,
        evento,
        osesp,
        net

    }
    public enum enumCancelamentoLoteFila
    {
        NaFila = 'F',
        AguardandoCliente = 'G',
        EmailEnviado = 'E',
        EstornoCartao = 'S',
        EstornoPayPal = 'P',
        ContatoTelefone = 'T',
        Cancelado = 'C'
    }

    public enum enumOperacoesCancelamento
    {
        OperacaoA = 'A',
        OperacaoB = 'B',
        OperacaoC = 'C',
        OperacaoD = 'D',
        OperacaoE = 'E',
        OperacaoF = 'F',
        OperacaoG = 'G',
        OperacaoZero = 'Z'
    }
    public enum enumStatusNPS
    {
        Aguardando = 'A',
        Processando = 'P',
        ErroEnvio = 'E',
        Sucesso = 'S'
    }

}
