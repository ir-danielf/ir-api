using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotaItemReserva
    {
        public int ID { get; set; }
        public int CotaID { get; set; }
        public string PrecoIniciaCom { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
        public bool isApresentacao { get; set; }
        public string TextoValidacao { get; set; }

        public bool ValidaBin { get; set; }
        public int QuantidadeApresentacao { get; set; }
        public int QuantidadePorClienteApresentacao { get; set; }
        public int QuantidadeApresentacaoSetor { get; set; }
        public int QuantidadePorClienteApresentacaoSetor { get; set; }
        public int ParceiroID { get; set; }

        public bool Nominal { get; set; }
        public int ApresentacaoID  { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int SetorID { get; set; }
        public bool CPFResponsavel { get; set; }
        public bool TemTermo { get; set; }
        public int getMenorValorCliente()
        {
            int[] qtds = new int[3] { QuantidadePorCliente, QuantidadePorClienteApresentacao, QuantidadePorClienteApresentacaoSetor };
            Array.Sort(qtds);

            for (int i = 0; i <= qtds.Length; i++)
            {
                if (i == qtds.Length)
                    return 0;

                if (qtds[i] != 0)
                    return qtds[i];
            }
            return 0;
        }


        public int QuantidadePorCodigo { get; set; }
    }
}
