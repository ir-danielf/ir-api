using System;

namespace IRLib.Paralela.Assinaturas.Models.Relatorios
{
    public class Agregado
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public int GrauParentesco { get; set; }
        public DateTime DataNascimento { get; set; }
        public string dataNascimento
        {
            get
            {
                return DataNascimento == DateTime.MinValue ? string.Empty : DataNascimento.ToString("dd/MM/yyyy");
            }
        }
        public string Profissao { get; set; }
        public string SituacaoProfissional { get; set; }
    }
}
