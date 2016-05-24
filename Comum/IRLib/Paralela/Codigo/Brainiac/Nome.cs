using System;
using System.Linq;

namespace IRLib.Paralela.Codigo.Brainiac
{
    public class Nome
    {
        public int Indice { get; set; }
        public string StrNome { get; set; }
        public string Resultado { get; set; }
        public int Erros { get; set; }
        public float PorcentagemAcerto { get; set; }

        public bool EncontrarCusto(string NomeCorreto)
        {
            int custo = BuscaMenorEsforco.EncontrarDistancia(NomeCorreto, StrNome);

            int QuantidadeMaximaErros =
              (int)Math.Round((float)(NomeCorreto.Length * Porcentagens.Instancia.Elementos.PorcentagemAcertosPorNome) / 100, MidpointRounding.AwayFromZero);

            if (custo > QuantidadeMaximaErros)
                return false;

            return true;
        }

        /// <summary>
        /// Aplicação do Shift-And Aproximado BUFADASSO!
        /// Correto: Caio
        /// Digitado: Kaioabc
        /// Passo 1: #######
        /// Passo 2: #aio###
        /// </summary>
        /// <param name="NomeCorreto"></param>
        /// <returns></returns>
        public void Aplicar(string NomeCorreto)
        {
            int mCorreto = NomeCorreto.Length;
            int mDigitado = StrNome.Length;

            char[] Masc = new char[mCorreto > mDigitado ? mCorreto : mDigitado];
            for (int s = 0; s < Masc.Length; s++)
                Masc[s] = (char)Enumeradores.EnumTipoAcao.Erro;

            char[] Correto = NomeCorreto.ToArray();
            char[] Digitado = StrNome.ToArray();

            //Este eh um array auxiliar para saber se o indice atual pode ser utilizado novamente
            bool[] Utilizaveis = new bool[Masc.Length];

            for (int z = 0; z < Utilizaveis.Length; z++)
                Utilizaveis[z] = true;

            char Rnovo;

            //i é o indice atual no Nome Correto
            int i = 0;

            //w é o atual a ser utilizado no nome digitado
            int w = 0;

            foreach (char atual in Correto)
            {
                Rnovo = (char)Enumeradores.EnumTipoAcao.Erro;

                //j é o inidice atual sendo utilizado no Nome Digitado
                for (int j = w; j <= (j + 2 > mDigitado - 1 ? mDigitado - 1 : j + 2); j++)
                {
                    if (!Utilizaveis[j])
                        continue;

                    if (atual != Digitado[j])
                        continue;

                    //Tem o mesmo INDICE??
                    if (j == i)
                    {
                        Utilizaveis[i] = false;
                        Rnovo = atual;
                        w++;
                        break;
                    }
                    else
                    {
                        Rnovo = Digitado[j];
                        Utilizaveis[j] = false;
                        w++;
                        break;
                    }
                }
                Masc[i] = Rnovo;
                i++;
            }
            string nomeUnmasked = string.Empty;
            foreach (char c in Masc)
                nomeUnmasked += c;

            this.Resultado = nomeUnmasked;
        }

        /// <summary>
        /// Avança o Indice do Nome
        /// Será Reordernado ao finalizar
        /// </summary>
        public void AvancarIndice()
        {
            this.Indice++;
        }

        public int QuantidadeErros()
        {
            this.Erros = this.Resultado.Where(c => c == (char)Enumeradores.EnumTipoAcao.Erro || c == (char)Enumeradores.EnumTipoAcao.Inserir).Count();
            return Erros;
        }

        /// <summary>
        /// Aplicação de Porcentagem de Acertos
        /// </summary>
        public void AplicarPorcentagem()
        {
            this.PorcentagemAcerto =
                (((this.Resultado.Length - this.Resultado
                            .Where(c => c == (char)Enumeradores.EnumTipoAcao.Erro || c == (char)Enumeradores.EnumTipoAcao.Inserir).Count())
                * 100)
                / this.Resultado.Length);


        }
    }
}
