using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace IRLib.Paralela.Assinaturas.Models
{
    public class Configuracao
    {

        private const string RegexDateTime = @"^(((((0[1-9])|(1\d)|(2[0-8]))\/((0[1-9])|(1[0-2])))|((31\/((0[13578])|(1[02])))|((29|30)\/((0[1,3-9])|(1[0-2])))))\/((20[0-9][0-9])|(19[0-9][0-9])))|((29\/02\/(19|20)(([02468][048])|([13579][26]))))$  ";

        public int ID { get; set; }


        [Required(ErrorMessage = "Canal Obrigatório.")]
        public int CanalAcessoID { get; set; }


        [Required(ErrorMessage = "Layout Obrigatória.")]
        public string LayoutAssinatura { get; set; }

        public bool PermiteAgregados { get; set; }

        public bool RetiradaBilheteria { get; set; }

        [Required(ErrorMessage = "Logo Obrigatório.")]
        public string Logo { get; set; }


        [Required(ErrorMessage = "Data inicial da Renovação é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data inicial Renovação inválida.")]
        public string DtInicioRenovacao { get; set; }

        [Required(ErrorMessage = "Data término da Renovação é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data término Renovação inválida.")]
        public string DtTerminoRenovacao { get; set; }


        [Required(ErrorMessage = "Data inicial da Troca Prioritária é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data inicial Troca Prioritária inválida.")]
        public string DtInicioTrocaPrioritaria { get; set; }

        [Required(ErrorMessage = "Data término da Troca Prioritária é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data término Troca Prioritária inválida.")]
        public string DtTerminoTrocaPrioritaria { get; set; }


        [Required(ErrorMessage = "Data inicial da Troca é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data inicial Troca inválida.")]
        public string DtInicioTroca { get; set; }

        [Required(ErrorMessage = "Data término da Troca é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data término Troca inválida.")]
        public string DtTerminoTroca { get; set; }


        [Required(ErrorMessage = "Data inicial Novas Aquisições é obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data inicial Novas Aquisições inválida.")]
        public string DtInicioAquisicao { get; set; }

        [Required(ErrorMessage = "Data término Novas Aquisições obrigatória.")]
        [RegularExpression(RegexDateTime, ErrorMessage = "Data término Novas Aquisições inválida.")]
        public string DtTerminoAquisicao { get; set; }


        [Required(ErrorMessage = "Texto do Login Obrigatório.")]
        public string textoLogin { get; set; }

        [Required(ErrorMessage = "Texto Termos e Condições Obrigatório.")]
        public string textoTermosCondicoes { get; set; }

        [Required(ErrorMessage = "Texto da Página Principal Obrigatório.")]
        public string textoPaginaPrincipal { get; set; }

        //[Required(ErrorMessage = "Texto Rodapé Obrigatório.")]
        public string textoRodape { get; set; }

        public bool ValorEntregaFixo { get; set; }
        public string ValorEntrega { get; set; }

        public string EntregaID { get; set; }

        public bool AceitaDinheiro { get; set; }
        public bool AceitaDebito { get; set; }
        public bool AceitaCheque { get; set; }

        public string AnoAtivoAssinatura { get; set; }       
        

    }
}
