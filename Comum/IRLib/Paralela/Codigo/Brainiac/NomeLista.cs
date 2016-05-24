using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela.Codigo.Brainiac
{
    public class NomeLista : List<Nome>
    {
        private List<Nome> NomesDigitados { get; set; }

        public NomeLista()
        {
            NomesDigitados = new List<Nome>();
        }

        public void InserirItem(string Nome)
        {
            this.NomesDigitados.Insert(
                this.NomesDigitados.Count,
                new Nome() { Indice = this.NomesDigitados.Count, StrNome = Nome, });
        }

        public void Iniciar()
        {
            foreach (Nome nome in this)
            {
                Nome nomeDigitado = this.NomesDigitados
                    .Where(c => c.Indice >= nome.Indice && c.EncontrarCusto(nome.StrNome)).FirstOrDefault();

                //Não achou nenhum deve aplicar nome com Erro masked ou é o mesmo Aplica a mascara
                if (nomeDigitado == null || nome.Indice == nomeDigitado.Indice)
                {
                    //Inserção!! Está Incluindo o Sobrenome na lista
                    if (nomeDigitado == null && this.NomesDigitados.Count < this.Count)
                    {
                        this.NomesDigitados.Where(c => c.Indice >= nome.Indice)
                            .ToList().ForEach(c => c.AvancarIndice());

                        nomeDigitado = new Nome() { Indice = nome.Indice, StrNome = string.Empty };
                        this.NomesDigitados.Add(nomeDigitado);
                    }
                    //Alteração
                    else
                        nomeDigitado = this.NomesDigitados.Where(c => c.Indice == nome.Indice).FirstOrDefault();

                    //Aplica no Nome correto no digitado -- Irá gerar o resultado
                    nomeDigitado.Aplicar(nome.StrNome);
                }
                else if (nomeDigitado.Indice > nome.Indice)
                {
                    this.NomesDigitados.Where(c => c.Indice >= nome.Indice)
                        .ToList().ForEach(c => c.AvancarIndice());

                    nomeDigitado.Indice = nome.Indice;
                    //Aplica no Nome correto no digitado -- Irá gerar o resultado
                    nomeDigitado.Aplicar(nome.StrNome);
                }
            }
            this.ReOrdernarDigitados();

        }

        public void ReOrdernarDigitados()
        {
            int indiceAnterior = 0;
            List<Nome> novaLista = new List<Nome>();

            foreach (Nome digitado in this.NomesDigitados.OrderBy(c => c.Indice))
            {
                digitado.Indice = indiceAnterior++;
                novaLista.Add(digitado);
            }
            this.NomesDigitados = novaLista;


        }

        public bool Comparar()
        {
            if (this.Count != this.NomesDigitados.Count)
                return false;

            foreach (Nome nome in this)
            {
                if (string.Compare(nome.StrNome,
                    this.NomesDigitados.Where(c => c.Indice == nome.Indice).FirstOrDefault().StrNome) != 0)
                    return false;
            }
            return true;
        }

        public void QuantidadeErros()
        {
            this.
                NomesDigitados.
                Sum(c => c.QuantidadeErros());
        }

        public float EncontrarQuantidadeAceitos()
        {
            this.QuantidadeErros();

            this.NomesDigitados
                    .ForEach(c => c.AplicarPorcentagem());


            //Mais de w%? em cada Nome
            int QuantidadeAceitos = this.NomesDigitados
                                        .Where(c => c.PorcentagemAcerto >= Porcentagens.Instancia.Elementos.PorcentagemAcertosPorNome)
                                        .Count();

            return (QuantidadeAceitos * 100) / this.Count;

        }

        public int EncontrarSobras()
        {
            return
                this.Count - this.NomesDigitados.Count;
        }

        public float EncontrarNomeRelevanteAceito()
        {
            Nome nomeRelevante = this.NomesDigitados.Where(c => c.Indice == 0).FirstOrDefault();

            float porc =
                (nomeRelevante.Erros * 100) / nomeRelevante.Resultado.Length;

            return 100 - porc;

        }
    }
}
