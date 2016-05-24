
namespace IRLib.Assinaturas.Models
{
    public class Confirmacao
    {
        public Confirmacao(string titulo, string acao, string resultado)
        {
            this.Titulo = titulo;
            this.Acao = acao;
            this.Resultado = resultado;
        }

        public string Titulo { get; private set; }
        public string Acao { get; private set; }
        public string Resultado { get; private set; }
    }
}
