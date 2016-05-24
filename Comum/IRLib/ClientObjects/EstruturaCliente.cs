using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCliente
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int contatotipoid;
        public int ContatoTipoID
        {
            get { return contatotipoid; }
            set { contatotipoid = value; }
        }

        private int clienteindicacaoid;
        public int ClienteIndicacaoID
        {
            get { return clienteindicacaoid; }
            set { clienteindicacaoid = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private bool ativo;
        public bool Ativo
        {
            get { return ativo; }
            set { ativo = value; }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string cpf;
        public string CPF
        {
            get { return cpf; }
            set { cpf = value; }
        }

        private string rg;
        public string RG
        {
            get { return rg; }
            set { rg = value; }
        }

        private string carteiraestudante;
        public string CarteiraEstudante
        {
            get { return carteiraestudante; }
            set { carteiraestudante = value; }
        }

        private string sexo;
        public string Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }

        private DateTime datanascimento;
        public DateTime DataNascimento
        {
            get { return datanascimento; }
            set { datanascimento = value; }
        }

        public int DataNascimentoDia
        {
            set
            {
                datanascimento = datanascimento.AddDays(value - 1);
            }
            get
            {
                return datanascimento.Day;
            }
        }
        public int DataNascimentoMes
        {
            set
            {
                datanascimento = datanascimento.AddMonths(value - 1);
            }
            get
            {
                return datanascimento.Month;
            }
        }
        public int DataNascimentoAno
        {
            set
            {
                datanascimento = datanascimento.AddYears(value - 1);
            }
            get
            {
                return datanascimento.Year;
            }
        }


        private string datanascimentots;
        public string DataNascimentoTS
        {
            get { return datanascimentots; }
            set { datanascimentots = value; }
        }

        private string telefoneresidencialddd;
        public string TelefoneResidencialDDD
        {
            get { return telefoneresidencialddd; }
            set { telefoneresidencialddd = value; }
        }

        private string telefoneresidencial;
        public string TelefoneResidencial
        {
            get { return telefoneresidencial; }
            set { telefoneresidencial = value; }
        }

        private string telefonecelularddd;
        public string TelefoneCelularDDD
        {
            get { return telefonecelularddd; }
            set { telefonecelularddd = value; }
        }

        private string telefonecelular;
        public string TelefoneCelular
        {
            get { return telefonecelular; }
            set { telefonecelular = value; }
        }

        private string telefonecomercialddd;
        public string TelefoneComercialDDD
        {
            get { return telefonecomercialddd; }
            set { telefonecomercialddd = value; }
        }

        private string telefonecomercial;
        public string TelefoneComercial
        {
            get { return telefonecomercial; }
            set { telefonecomercial = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        private string senhaconfirmacao;
        public string SenhaConfirmacao
        {
            get { return senhaconfirmacao; }
            set { senhaconfirmacao = value; }
        }

        private string receberemail;
        public string ReceberEmail
        {
            get { return receberemail; }
            set { receberemail = value; }
        }

        private string cepEntrega;
        public string CEPEntrega
        {
            get { return cepEntrega; }
            set { cepEntrega = value; }
        }

        private string enderecoEntrega;
        public string EnderecoEntrega
        {
            get { return enderecoEntrega; }
            set { enderecoEntrega = value; }
        }

        private string enderecoNumeroEntrega;
        public string EnderecoNumeroEntrega
        {
            get { return enderecoNumeroEntrega; }
            set { enderecoNumeroEntrega = value; }
        }

        private string enderecoComplementoEntrega;
        public string EnderecoComplementoEntrega
        {
            get { return enderecoComplementoEntrega; }
            set { enderecoComplementoEntrega = value; }
        }

        private string bairroEntrega;
        public string BairroEntrega
        {
            get { return bairroEntrega; }
            set { bairroEntrega = value; }
        }

        private string cidadeEntrega;
        public string CidadeEntrega
        {
            get { return cidadeEntrega; }
            set { cidadeEntrega = value; }
        }

        private string estadoEntrega;
        public string EstadoEntrega
        {
            get { return estadoEntrega; }
            set { estadoEntrega = value; }
        }

        private string cepCliente;
        public string CEPCliente
        {
            get { return cepCliente; }
            set { cepCliente = value; }
        }

        private string enderecoCliente;
        public string EnderecoCliente
        {
            get { return enderecoCliente; }
            set { enderecoCliente = value; }
        }

        private string enderecoNumeroCliente;
        public string EnderecoNumeroCliente
        {
            get { return enderecoNumeroCliente; }
            set { enderecoNumeroCliente = value; }
        }

        private string enderecoComplementoCliente;
        public string EnderecoComplementoCliente
        {
            get { return enderecoComplementoCliente; }
            set { enderecoComplementoCliente = value; }
        }

        private string bairroCliente;
        public string BairroCliente
        {
            get { return bairroCliente; }
            set { bairroCliente = value; }
        }

        private string cidadeCliente;
        public string CidadeCliente
        {
            get { return cidadeCliente; }
            set { cidadeCliente = value; }
        }

        private string estadoCliente;
        public string EstadoCliente
        {
            get { return estadoCliente; }
            set { estadoCliente = value; }
        }

        private string observacao;
        public string Observacao
        {
            get { return observacao; }
            set { observacao = value; }
        }

        private string nomeEntrega;
        public string NomeEntrega
        {
            get { return nomeEntrega; }
            set { nomeEntrega = value; }
        }

        private string cpfEntrega;
        public string CpfEntrega
        {
            get { return cpfEntrega; }
            set { cpfEntrega = value; }
        }

        private string rgEntrega;
        public string RgEntrega
        {
            get { return rgEntrega; }
            set { rgEntrega = value; }
        }
        //
        private string statusConsulta;
        public string StatusConsulta
        {
            get { return statusConsulta; }
            set { statusConsulta = value; }
        }

        private string statusConsultaEntrega;
        public string StatusConsultaEntrega
        {
            get { return statusConsultaEntrega; }
            set { statusConsultaEntrega = value; }
        }

        public string Pais { get; set; }
        public string CPFResponsavel { get; set; }
        public string StatusAtual { get; set; }
        public bool PrecisaSenha { get; set; }
        public bool StatusBloqueado { get; set; }
        public bool StatusLiberado { get; set; }
        public int NivelCliente { get; set; }
        public bool BomCliente { get; set; }
        public bool MalCliente { get; set; }

        private string razaoSocial;
        public string RazaoSocial
        {
            get { return razaoSocial; }
            set { razaoSocial = value; }
        }

        private string nomeFantasia;
        public string NomeFantasia
        {
            get { return nomeFantasia; }
            set { nomeFantasia = value; }
        }

        private string cnpj;
        public string CNPJ
        {
            get { return cnpj; }
            set { cnpj = value; }
        }

        private string inscricaoEstadual;
        public string InscricaoEstadual
        {
            get { return inscricaoEstadual; }
            set { inscricaoEstadual = value; }
        }

        public char TipoCadastro { get; set; }

        public bool CanaisEspeciais { get; set; }

        public void SubstituirDados(EstruturaCliente estCliente)
        {
            this.nome = estCliente.nome;
            if (!string.IsNullOrEmpty(estCliente.cpf))
                this.cpf = estCliente.cpf;

            if (!string.IsNullOrEmpty(estCliente.Pais))
                this.Pais = estCliente.Pais;
        }

        public string TokenAcesso { get; set; }

        public string FacebookID { get; set; }

        public string UserInfo { get; set; }

        public List<string> ValidarCampos()
        {
            List<string> mensgens = new List<string>();
            if (this.nome.Length > 50) { mensgens.Add("O campo Nome deve conter no máximo 50 caracteres."); }
            if (this.cpf.Length > 30) { mensgens.Add("O campo CPF ou Document ID deve conter no máximo 30 dígitos."); }
            if (this.rg.Length > 30) { mensgens.Add("O campo RG deve conter no máximo 30 caracteres."); }
            if (this.CPFResponsavel.Length > 30) { mensgens.Add("O campo CPF do Responsável ou Document ID deve conter no máximo 30 dígitos."); }
            if (this.telefoneresidencialddd.Length > 2) { mensgens.Add("O campo DDD - Telefone deve conter 2 dígitos."); }
            if (this.telefoneresidencial.Length > 16) { mensgens.Add("O campo Telefone deve conter no máximo 8 dígitos."); }
            if (this.telefonecelularddd.Length > 2) { mensgens.Add("O campo DDD - Celular deve conter 2 dígitos."); }
            if (this.telefonecelular.Length > 16) { mensgens.Add("O campo Celular deve conter no máximo 8 dígitos."); }
            if (this.email.Length > 200) { mensgens.Add("O campo Email deve conter no máximo 100 caracteres."); }
            if (this.cepCliente.Length > 8) { mensgens.Add("O campo Celular deve conter 8 dígitos."); }
            if (this.enderecoCliente.Length > 60) { mensgens.Add("O campo Endereço deve conter no máximo 60 caracteres."); }
            if (this.enderecoNumeroCliente.Length > 10) { mensgens.Add("O campo Número deve conter no máximo 10 caracteres."); }
            if (this.enderecoComplementoCliente.Length > 20) { mensgens.Add("O campo Complemento deve conter no máximo 20 caracteres."); }
            if (this.bairroCliente.Length > 100) { mensgens.Add("O campo Bairro deve conter no máximo 100 caracteres."); }
            if (this.cidadeCliente.Length > 50) { mensgens.Add("O campo Cidade deve conter no máximo 50 caracteres."); }
            return mensgens;
        }

    }

    [Serializable]
    public class EstruturaRetornoLoginFacebook
    {
        public bool Erro { get; set; }

        public string Mensagem { get; set; }

        public EstruturaCliente Cliente { get; set; }
    }
}
