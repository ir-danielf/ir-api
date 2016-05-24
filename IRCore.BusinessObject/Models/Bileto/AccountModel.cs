using System;
using System.Collections.Generic;
using IRCore.BusinessObject.Models.Bileto.Enums;
using IRCore.DataAccess.Model;

namespace IRCore.BusinessObject.Models.Bileto
{
    public class AccountSignIn
    {
        public string owner { get; set; }

        public string credential { get; set; }

        public int authenticationPlatform { get; set; }
    }

    public class Account
    {
        public string id { get; set; }

        public string tenantId { get; set; }

        public string email { get; set; }

        public string documentType { get; set; }

        public string documentNumber { get; set; }

        public string name { get; set; }

        public DateTime? birthday { get; set; }

        public int? gender { get; set; }

        public string nationality { get; set; }

        public string contactNumber { get; set; }

        public bool allowContact { get; set; }

        public int status { get; set; }

        public static explicit operator Account(Login login)
        {
            return new Account
            {
                tenantId = ((Site)login.SiteID).ToString(),
                email = login.Email,
                documentNumber = login.CPF,
                documentType = DocumentType.CPF.ToString(),
                name = login.Cliente.Nome,
                birthday = login.Cliente.DataNascimentoAsDateTime,
                nationality = login.Cliente.Pais,
                contactNumber = string.Format("{0} {1}", login.Cliente.DDDTelefone, login.Cliente.Telefone)
            };
        }

        public static explicit operator Login(Account account)
        {
            var login = new Login
            {
                Email = account.email,
                Senha = string.Empty,
                Cliente = new tCliente
                {
                    Email = account.email,
                    Nome = account.name,
                    DataNascimento = account.birthday.HasValue ? account.birthday.Value.ToString("yyyyMMdd") : "",
                    Pais = account.nationality,
                    RecebeEmail = account.allowContact ? "T" : "F"
                },
                BiletoUuid = account.id
            };

            if (account.documentType == DocumentType.CPF.ToString())
            {
                login.CPF = login.Cliente.CPF = account.documentNumber.DigitsOnly();
            }
            else if (account.documentType == DocumentType.RG.ToString())
            {
                login.Cliente.RG = account.documentNumber.DigitsOnly();
            }
            else if (account.documentType == DocumentType.CNPJ.ToString())
            {
                login.Cliente.CNPJ = account.documentNumber.DigitsOnly();
            }

            if (account.gender == 1)
            {
                login.Cliente.Sexo = "M";
            }
            else if (account.gender == 2)
            {
                login.Cliente.Sexo = "F";
            }

            return login;
        }
    }

    public class AccountSignUp
    {
        public Account account { get; set; }

        public string credential { get; set; }

        public AuthenticationPlatform authenticationPlatform { get; set; }

        public static explicit operator AccountSignUp(Login login)
        {
            var accountInfo = new AccountSignUp
            {
                account = new Account
                {
                    email = login.Email.ToLower(),
                    documentType = DocumentType.CPF.ToString(),
                    documentNumber = login.CPF.Replace(new[] { ".", "-" }, ""),
                    status = (int)AccountStatus.ACTIVE
                },
                credential = login.Senha,
                authenticationPlatform = AuthenticationPlatform.EMAIL_PASSWORD
            };

            if (!string.IsNullOrEmpty(login.FaceBookUserID))
            {
                accountInfo.authenticationPlatform = AuthenticationPlatform.FACEBOOK;
                accountInfo.credential = login.FaceBookUserToken;
            }

            if (login.Cliente != null)
            {
                if (!string.IsNullOrEmpty(login.Cliente.Pais))
                    accountInfo.account.nationality = login.Cliente.Pais;

                if (!string.IsNullOrEmpty(login.Cliente.Nome))
                    accountInfo.account.name = login.Cliente.Nome;
            }

            return accountInfo;
        }
    }

    public class AccountInfo
    {
        public int tenantId { get; set; }

        public string email { get; set; }

        public string documentType { get; set; }

        public string documentNumber { get; set; }

        public string name { get; set; }

        public DateTime? birthday { get; set; }

        public int? gender { get; set; }

        public string nationality { get; set; }

        public bool allowContact { get; set; }

        public string credential { get; set; }

        public int authenticationPlatform { get; set; }

        public static explicit operator AccountInfo(Login login)
        {
            var accountInfo = new AccountInfo
            {
                tenantId = (int)(Site)login.SiteID,
                email = login.Email.ToLower(),
                documentType = DocumentType.CPF.ToString(),
                documentNumber = login.CPF.DigitsOnly(),
                name = login.Cliente.Nome,
                credential = login.Senha,
                authenticationPlatform = (int)AuthenticationPlatform.EMAIL_PASSWORD
            };

            if (!string.IsNullOrEmpty(login.FaceBookUserID))
            {
                accountInfo.authenticationPlatform = (int)AuthenticationPlatform.FACEBOOK;
                accountInfo.credential = login.FaceBookUserToken;
            }

            if (!string.IsNullOrEmpty(login.Cliente.Pais))
                accountInfo.nationality = login.Cliente.Pais;

            if (login.Cliente.DataNascimentoAsDateTime != DateTime.MinValue)
                accountInfo.birthday = login.Cliente.DataNascimentoAsDateTime;

            if (!string.IsNullOrEmpty(login.Cliente.CNPJ))
            {
                accountInfo.documentType = DocumentType.CNPJ.ToString();
                accountInfo.documentNumber = login.Cliente.CNPJ.DigitsOnly();
                accountInfo.name = login.Cliente.NomeFantasia;
            }

            return accountInfo;
        }
    }

    public class ChangePassword
    {
        public string currentPassword { get; set; }

        public string newPassword { get; set; }
    }

    public class ForgottenPassword
    {
        public string email { get; set; }

        public string redirectURL { get; set; }

        public string externalClient { get; set; }
    }

    public class ExportResponse
    {
        public Account account { get; set; }

        public bool success { get; set; }

        public List<ErrorInfo> errors { get; set; }

        public static explicit operator Login(ExportResponse response)
        {
            return (Login)response.account;
        }
    }
}