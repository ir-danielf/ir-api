using System;

namespace IRLib.Codigo.ModuloLogistica
{
    [Serializable]
    public class Enumeradores
    {
        public enum TaxaEntregaTipo
        {
            Nenhum = 0,
            Entrega = 1,
            Retirada = -1,
        }

        public enum SenhaValida
        {
            Valida = 0,
            TamanhoInvalido = 1,
            ContemLetra = 2,
            JaIncluida = 3,
        }

        public enum TipoPesquisa
        {
            Senha = 0,
            Arquivo = 1,
            Coletor = 2,
        }
    }
}
