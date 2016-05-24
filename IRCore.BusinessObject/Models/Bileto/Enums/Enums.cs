namespace IRCore.BusinessObject.Models.Bileto.Enums
{
    public enum Site
    {
        IR = 1,

        ENTRETIX = 2,

        INGRESSO_MAIS = 3
    }

    public enum DocumentType
    {
        CPF,

        RG,

        SOCIAL_SECURITY_NUMBER,

        CNPJ,

        PASSPORT
    }

    public enum AuthenticationPlatform
    {
        EMAIL_PASSWORD = 1,

        FACEBOOK,

        ACCOUNT_KIT
    }

    public enum AccountStatus
    {
        ACTIVE = 1,

        BLOCKED = 2
    }

}