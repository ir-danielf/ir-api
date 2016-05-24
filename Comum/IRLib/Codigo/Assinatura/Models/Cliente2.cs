using System.ComponentModel.DataAnnotations;

namespace IRLib.Codigo.Assinatura.Models
{
   public class Cliente2
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Nome obrigatório.")]
        [StringLength(100, ErrorMessage = "Nome - Máximo de 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Nome - Somente letras.")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "Nome - Máximo de 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Nome - Somente letras.")]
        public string NomeFantasia { get; set; }

        [StringLength(100, ErrorMessage = "Nome - Máximo de 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Nome - Somente letras.")]
        public string NomeEmpresa { get { return this.Nome; } set { this.Nome = value; } }

        [StringLength(100, ErrorMessage = "Nome - Máximo de 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Nome - Somente letras.")]
        public string RazaoSocial { get; set; }

        [StringLength(8, ErrorMessage = "Máximo de 100 caracteres.")]
        public string Login { get; set; }

        [StringLength(20, ErrorMessage = "Senha - Máximo de 20 caracteres.")]
        public string Sexo { get; set; }

        public string TipoCadastro { get; set; }

        [Required(ErrorMessage = "CPF obrigatório.")]
        [StringLength(14, ErrorMessage = "CPF/CNPJ - Máximo de 14 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "CPF - Somente números.")]
        public string CPF { get; set; }


        [StringLength(14, ErrorMessage = "CNPJ - Máximo de 14 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "CNPJ - Somente números.")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "CEP obrigatório.")]
        [StringLength(8, ErrorMessage = "CEP - Máximo de 8 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "CEP - Somente números.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Endereço obrigatório.")]
        [StringLength(100, ErrorMessage = "Endereço - Máximo de 100 caracteres.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Número obrigatório.")]
        [StringLength(6, ErrorMessage = "Número - Máximo de 6 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número - Somente números.")]
        public string EnderecoNumero { get; set; }

        [StringLength(20, ErrorMessage = "Complemento - Máximo de 20 caracteres.")]
        public string Complemento { get; set; }

        public int CidadeID { get; set; }

        public int EstadoID { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório.")]
        [StringLength(100, ErrorMessage = "Bairro - Máximo de 100 caracteres.")]
        public string Bairro { get; set; }

        [StringLength(200, ErrorMessage = "E-mail - Máximo de 200 caracteres.")]
        public string Email { get; set; }

        [StringLength(2, ErrorMessage = "DDD residencial - Máximo de 2 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "DDD residencial - Somente números.")]
        public string DDDResidencial { get; set; }

        [StringLength(8, ErrorMessage = "Número residencial - Máximo de 8 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número residencial - Somente números.")]
        public string TelResidencial { get; set; }

        [StringLength(2, ErrorMessage = "DDD do celular - Máximo de 2 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "DDD celular - Somente números.")]
        public string DDDTelCelular { get; set; }

        [StringLength(9, ErrorMessage = "Número do celular - Máximo de 9 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número celular - Somente números.")]
        public string TelCelular { get; set; }

        [StringLength(2, ErrorMessage = "DDD Comercial 1 - Máximo de 2 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "DDD Comercial1 - Somente números.")]
        public string DDDTelComercial1 { get; set; }

        [StringLength(8, ErrorMessage = "Número comercial 1 - Máximo de 8 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número comercial1 - Somente números.")]
        public string TelComercial1 { get; set; }

        [StringLength(2, ErrorMessage = "DDD comercial 2 - Máximo de 2 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "DDD comercial2 - Somente números.")]
        public string DDDTelComercial2 { get; set; }

        [StringLength(8, ErrorMessage = "Número comercial 2 - Máximo de 8 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número comercial2 - Somente números.")]
        public string TelComercial2 { get; set; }

        [StringLength(2, ErrorMessage = "DDD do Fax - Máximo de 2 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "DDD Fax - Somente números.")]
        public string DDDTelFax { get; set; }

        [StringLength(8, ErrorMessage = "Número do Fax - Máximo de 8 caracteres.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Número Fax - Somente números.")]
        public string TelFax { get; set; }

        [StringLength(50, ErrorMessage = "Profissão - Máximo de 50 caracteres.")]
        public string Profissao { get; set; }

        [StringLength(20, ErrorMessage = "Máximo de 20 caracteres.")]
        public string SituacaoProfissional { get; set; }


        [StringLength(10, ErrorMessage = "Data de nascimento - Máximo de 10 caracteres.")]
        public string DataNascimento { get; set; }

        public int Pagina { get; set; }


        public string Cidade { get; set; }
        public string Estado { get; set; }


    }
}
