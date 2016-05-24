using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaDonoIngresso
    {
        public int IngressoID { get; set; }
        public int DonoID { get; set; }
        public int CotaItemID { get; set; }
        public int CotaItemIDAPS { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeAPS { get; set; }

        public bool UsarCPFResponsavel { get; set; }
        public string CPF { get; set; }
        public string CodigoPromocional { get; set; }
        public int StatusCodigoPromocional { get; set; }
        public bool Nominal { get; set; }
    }

    public class EstruturaDonoIngressoSite
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string NomeResponsavel { get; set; }
        public string CPFResponsavel { get; set; }

        public bool Preenchido
        {
            get
            {
                return !string.IsNullOrEmpty(Nome) || !string.IsNullOrEmpty(CPF) || !string.IsNullOrEmpty(RG) || !string.IsNullOrEmpty(Email) ||
                     !string.IsNullOrEmpty(Telefone) || !string.IsNullOrEmpty(NomeResponsavel) || !string.IsNullOrEmpty(CPFResponsavel);
            }
        }

        public bool CamposValidos()
        {
            if (!string.IsNullOrEmpty(CPF) && !Utilitario.IsCPF(CPF))
                this.Mensagem = "O CPF digitado não é válido.";
            else if (!string.IsNullOrEmpty(Email) && !Utilitario.IsEmail(Email))
                this.Mensagem = "O E-mail digitado não é válido.";
            else if (!string.IsNullOrEmpty(CPFResponsavel) && !Utilitario.IsCPF(CPFResponsavel))
                this.Mensagem = "O CPF do Responsável digitado não é válido.";
            else if (!string.IsNullOrEmpty(DataNascimento) && !Utilitario.IsDateTime(DataNascimento))
                this.Mensagem = "A Data de Nascimento digitada não é válida.";

            return string.IsNullOrEmpty(this.Mensagem);
        }

        public string Mensagem { get; set; }
    }
}
