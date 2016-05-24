using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model.Enumerator
{
    public enum enumLugarMarcado
    {
        mesaAberta = 'A', 
        cadeira = 'C',
        mesaFechada = 'M',
        pista = 'P'
    }
    public enum enumConfiguracaoModel
    {

        siteTiposOrdem,
        siteTiposCor
    }
    public enum enumVoucherStatus
    {
        nenhum = '_',
        disponivel = 'D',
        inativo = 'I',
        capturado = 'C',
        expirado = 'E'
    }

    public enum enumEntregaTipo
    {
        nenhum = '_',
        entregaEmCasaAgendada = 'A',
        entregaEmCasaNormal = 'N',
        retiradaPDV = 'R',
        retiradaBilheteria = 'B'
    }

    public enum enumIngressoStatus
    {
        virTrocado = '*',
        aguardandoTroca = 'A',
        bloqueado = 'B',
        disponivel = 'D',
        entregue = 'E',
        impresso = 'I',
        preReserva = 'N',
        preImpresso = 'P',
        reservado = 'R',
        cancelado = 'C',
        vendido = 'V',
        cortesiaSemConveniencia = 'X',
        reservadoPreReserva = 'M',
        reservadoPreReservaSite = 'S'
    }

    public enum enumCarrinhoStatus
    {
        deletado = 'D',
        expirado = 'E',
        reservado = 'R',
        vendido = 'V',
        vendidoEnviouEmail = 'W', 
        cortesiaSemConveniencia = 'X'
    }

    public enum enumVendaBilheteriaStatus
    {
        aguardandoPagamento = 'A',
       pago = 'P',
       cancelado = 'C',
       entregue = 'E',
       reimpresso = 'R',
       preReservado = 'M',
       aguardandoAprovacao = 'A',
       fraude = 'F',
       emAnalise = 'N',
       vendido = 'V',
       recusado = 'R'
    }


    public enum enumTipoLugar
    {
        Cadeira = 'C',
        MesaAberta = 'A',
        MesaFechada = 'M',
        Pista = 'P'
    }

    public enum enumClienteStatus
    {
        bloqueado = 'B',
        liberado = 'L'
    }

    public enum enumUsuarioStatus
    {
        bloqueado = 'B',
        liberado = 'L',
        transferido = 'T'
    }

    public enum enumDestaqueRegiaoTipo
    {
        home = 'H',
        menu = 'M'
    }
    public enum enumNewsAssinantes
    {
        ativo = 1,
        inativo = 0
    }

    public enum enumEventoTipoMidiaTipo
    {
        arquivo = 0,
        texto = 1
    }

    public enum enumTipoReserva
    {
        valeIngresso,
        ingresso
    }

    public enum enumEventoSemVendaMotivo
    {
        vendaDisponivel = 0,
        vendaOnlineNaoDisponivel = 1,
        vendaSomenteCallCenter = 2,
        vendasEncerradas = 3,
        vendasNaoIniciadas = 4,
        vendaDisponivelApenasParaPacotes = 5,
        vendaDisponivelDeterminadaData = 6
    }

    public enum enumFormaPagamento
    {
        Amex,
        Aura,
        Diners,
        Elo,
        EloCultura,
        Hipercard,
        MilagemSmiles,
        PayPal,
        RedecardCredito,
        ValeCultura,
        VisaCredito,
        VisaElectron,
        HSBCAVista,
        HSBCTransferencia,
        HSBCParcelado,
        ItauShopline,
        Bradesco,
        Cortesia,
        VisaElectron_DESATIVADO,
        Nenhuma,
        Redeshop,
        Dinheiro,
    }

    public enum enumAPITipoAcesso
    {
        app = 0,
        webServer = 1,
        webClient = 2
    }

    public enum enumStatusEstorno
    {
        Automatico = 'A',
        Solicitado = 'S',
        Pendente = 'P',
        Cancelado = 'C'
    }
}
