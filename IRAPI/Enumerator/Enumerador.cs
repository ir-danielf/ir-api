namespace IRAPI.Enumerator
{
    public enum enumConfiguracaoApp
    {
        itensPorPaginEventos,
        limiteLocais,
        //EventoNaoExibir,
        //EventoNaoExibirUsuarioAPI_
    }

    public enum enumTipoRetornoCancelamento
    {
        cancelSeguroIndisponivel,
        cancelEntregaIndisponivel,
        formaDevolucaoIndisponivel,
        devolucaoIngressosExigida,
        senhaInexistente,
        senhaNula,
        loginRequerido,
        requestInvalido,
        ingressoInexistente,
        ingressoCancelado,
        ingressoCanceladoPendente,
        dadosDepositoNulo,
        dadosDepositoPropriedadeNula,
        cancelamentoAbortado,
        canceladoComSucesso,
        cancelamentoPacoteCompleto,
        dadosCartaoPropriedadeNula,
        nenhumIngressoSelecionado,
        devolucaoIngressos
    }
}