using CTLib;
using System;
using System.ComponentModel.DataAnnotations;

namespace IRLib.Codigo.NovoGerador
{
    [Serializable]
    public class GerarEventoEtapa3Fase1Model
    {
        /// <summary>
        /// Informações de Borderô
        /// </summary>
     
        public string RazaoSocial { get; set; }

       
        public string CpfCnpj { get; set; }

       
        public string Endereco { get; set; }
        public char ExigirCadastramentoCliente { get; set; }

        /// <summary>
        /// Orientações para Atendimento via Call Center.
        /// </summary>
        public string Atencao { get; set; }
        public string MeiaEntrada { get; set; }
        public string Promocoes { get; set; }
        public string AberturaPortoes { get; set; }
        public string DuracaoEvento { get; set; }
        public string ResumoEvento { get; set; }
        public string PontoEventoSemTaxa { get; set; }
        public string Censura { get; set; }
        public string RetiradaIngresso { get; set; }
        public string DescricaoPadraoApresentacao { get; set; }

    }
}
