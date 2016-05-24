
namespace IRCore.Util.Enumerator
{
    public enum enumAdminFiltroEvento
    {
        nenhum = 0,
        statusPublicado = 1,
        statusNãoPublicado = 2,
        statusPublicadoSemVenda = 3,
        incompletoSite = 4,
        incompletoMobile = 5
    }
    public enum enumConfiguracaoGeral
    {
        sessionModelKey,
        itensPorPagina,
        logCategory,
        facebookAppID,
        facebookAppSecret,
        accountKitApiUrl,
        accountKitAppId,
        accountKitAppSecret,
        facebookAppPermissao,
        httpsHabilitado,
        urlDominioReplace,
        cancelamentoMassaDiasLimite,
        emailHost,
        emailPort,
        emailSSL,
        emailUser,
        emailPass,
        emailDefaultSender,
        ttlStaticObjects
    }

    public enum enumReportFormat
    {
        Excel,
        PDF,
        Image,
        HTML,//Nao suportado pelo report viewer
        CSV,//Nao suportado pelo report viewer
        TXT//Nao suportado pelo report viewer
    }
}
