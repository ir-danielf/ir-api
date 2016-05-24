using System.ComponentModel.DataAnnotations;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class Agregados
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "Nome obrigatório.")]
        [StringLength(100, ErrorMessage = "Nome - Máximo de 100 caracteres")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Nome - Somente letras")]
        public string Nome { get; set; }

        [StringLength(50, ErrorMessage = "Profissão - Máximo de 50 caracteres")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ ]+$", ErrorMessage = "Profissão - Somente letras")]
        public string Profissao { get; set; }

        [StringLength(20, ErrorMessage = "Máximo de 20 caracteres")]
        public string SituacaoProfissional { get; set; }


        [StringLength(10, ErrorMessage = "Data de nascimento - Máximo de 10 caracteres")]
        public string DataNascimento { get; set; }

        public string Email { get; set; }

        public int grauParentescoID { get; set; }

    }
}
