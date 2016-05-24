using System.Collections.Generic;

namespace IRCore.BusinessObject.Models
{
    public class RetornoModel<T>
    {
        public bool Sucesso { get; set; }

        public string Mensagem { get; set; }

        public T Retorno { get; set; }
    }

    public class RetornoModel<T, TEnum>
    {
        public bool Sucesso { get; set; }

        public TEnum Tipo { get; set; }

        public string Mensagem { get; set; }

        public T Retorno { get; set; }
    }

    public class RetornoModel
    {
        public bool Sucesso { get; set; }

        public string Mensagem { get; set; }
    }
}