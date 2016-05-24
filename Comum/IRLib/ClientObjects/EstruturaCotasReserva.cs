using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotasReserva
    {
        public int CotaIDApresentacao { get; set; }
        public int CotaIDApresentacaoSetor { get; set; }
        public int QuantidadePorClienteApresentacaoSetor { get; set; }
        public int QuantidadeApresentacaoSetor { get; set; }
        public int QuantidadeApresentacao { get; set; }
        public int QuantidadePorClienteApresentacao { get; set; }
        public int QuantidadePorClienteCotaItem { get; set; }
        public int QuantidadeCotaItem { get; set; }

        public int getMenorQuantidade()
        {
            if (QuantidadeApresentacao < QuantidadeApresentacaoSetor && QuantidadeApresentacao != 0)
                return QuantidadeApresentacao;
            if (QuantidadeApresentacao > QuantidadeApresentacaoSetor && QuantidadeApresentacaoSetor == 0)
                return QuantidadeApresentacao;
            else if (QuantidadeApresentacaoSetor < QuantidadeApresentacao && QuantidadeApresentacaoSetor != 0)
                return QuantidadeApresentacaoSetor;
            else if (QuantidadeApresentacaoSetor > QuantidadeApresentacao && QuantidadeApresentacao == 0)
                return QuantidadeApresentacaoSetor;
            else if (QuantidadeApresentacaoSetor == QuantidadeApresentacao)
                return QuantidadeApresentacao;
            else return 0;
        }

        public int getMenorQuantidadeCliente()
        {
            if (QuantidadePorClienteApresentacao < QuantidadePorClienteApresentacaoSetor && QuantidadePorClienteApresentacao != 0)
                return QuantidadePorClienteApresentacao;
            else if (QuantidadePorClienteApresentacao > QuantidadePorClienteApresentacaoSetor && QuantidadePorClienteApresentacaoSetor == 0)
                return QuantidadePorClienteApresentacaoSetor;
            else if (QuantidadePorClienteApresentacaoSetor < QuantidadePorClienteApresentacao && QuantidadePorClienteApresentacaoSetor != 0)
                return QuantidadePorClienteApresentacaoSetor;
            else if (QuantidadePorClienteApresentacaoSetor > QuantidadePorClienteApresentacao && QuantidadePorClienteApresentacao == 0)
                return QuantidadePorClienteApresentacaoSetor;
            else if (QuantidadePorClienteApresentacaoSetor == QuantidadePorClienteApresentacao)
                return QuantidadePorClienteApresentacao;
            else return 0;
        }
    }
}
