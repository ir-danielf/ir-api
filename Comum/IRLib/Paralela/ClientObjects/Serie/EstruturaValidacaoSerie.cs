using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects.Serie
{
    [Serializable]
    public class EstruturaVendaSerie
    {
        public EstruturaVendaSerie()
        {
            this.ListaInfoSerie = new List<EstruturaSerieValidacao>();
            this.AtualInfo = new EstruturaSerieValidacao();
        }
        public List<EstruturaSerieValidacao> ListaInfoSerie { get; set; }
        private EstruturaSerieValidacao AtualInfo { get; set; }

        public void AdicionarInformacao(
            string serieNome, int serieID, int QuantidadeMinima, int QuantidadeMaxima, int eventoID, int apresentacaoID,
            string evento, string apresentacao)
        {
            if (serieID != AtualInfo.SerieID)
            {
                AtualInfo = new EstruturaSerieValidacao()
                {
                    SerieNome = serieNome,
                    SerieID = serieID,
                    QuantidadeMaximaApresentacao = QuantidadeMaxima,
                    QuantidadeMinimaApresentacao = QuantidadeMinima
                };

                AtualInfo.Adicionar(eventoID, evento, apresentacaoID, apresentacao);

                ListaInfoSerie.Add(AtualInfo);
            }
            else
                AtualInfo.Adicionar(eventoID, evento, apresentacaoID, apresentacao);

        }

        public string Validar()
        {
            //Verifica Série por Série para ver se algo está incorreto (Quantidade Excedida ou Quantidade Reservada incorreta.
            string MensagemRetorno = string.Empty;
            foreach (EstruturaSerieValidacao serie in ListaInfoSerie)
            {
                serie.Validar();
                MensagemRetorno += serie.MensagemRetorno;
            }

            return MensagemRetorno;
        }
    }
    [Serializable]
    public class EstruturaSerieValidacao
    {
        public EstruturaSerieValidacao()
        {
            this.ListaApresentacoes = new List<EstruturaSerieQuantidades>();
            this.ApresentacaoAtual = new EstruturaSerieQuantidades();
        }

        public int SerieID { get; set; }
        public string SerieNome { get; set; }
        public int QuantidadeMinimaApresentacao { get; set; }
        public int QuantidadeMaximaApresentacao { get; set; }
        public List<EstruturaSerieQuantidades> ListaApresentacoes { get; set; }
        public EstruturaSerieQuantidades ApresentacaoAtual { get; set; }
        public string MensagemRetorno { get; set; }
        public bool StatusOk { get; set; }



        public void Adicionar(int eventoID, string evento, int apresentacaoID, string apresentacao)
        {
            if (apresentacaoID != ApresentacaoAtual.ApresentacaoID)
            {
                ApresentacaoAtual = new EstruturaSerieQuantidades()
                {
                    EventoID = eventoID,
                    Evento = evento,

                    ApresentacaoID = apresentacaoID,
                    Apresentacao = apresentacao,
                    QuantidadeReservada = 1,
                };

                this.ListaApresentacoes.Add(ApresentacaoAtual);
            }
            else
                ApresentacaoAtual.QuantidadeReservada++;

        }

        public void Validar()
        {
            int MaiorQtd = 0;
            int QtdApresentacoes = 0;

            string ErroQuantidadeApresentacao = string.Empty;


            //Vai fazer a verificação de QUantidade por Apresentação, deve possuir a mesma quantidade de ingressos, do contrario, mostrar a mensagem de Erro

            this.Sort();

            //Ultimo da lista é o maior
            MaiorQtd = ListaApresentacoes[ListaApresentacoes.Count - 1].QuantidadeReservada;

            for (int i = ListaApresentacoes.Count - 1; i >= 0; i--)
            {
                QtdApresentacoes++;

                if (ListaApresentacoes[i].QuantidadeReservada == MaiorQtd)
                    continue;

                ErroQuantidadeApresentacao += string.Format("Reserve mais {0} ingresso(s) para o Evento: {1} na apresentação {2}.{3}",
                    MaiorQtd - ListaApresentacoes[i].QuantidadeReservada, ListaApresentacoes[i].Evento,
                    ListaApresentacoes[i].Apresentacao, Environment.NewLine);
            }

            if (this.QuantidadeMaximaApresentacao > 0 && QtdApresentacoes > this.QuantidadeMaximaApresentacao)
            {
                this.StatusOk = false;
                MensagemRetorno += string.Format("A Série: {0} deve possuir no Máximo {1} apresentações distintas em sua reserva.{2}",
                    this.SerieNome, this.QuantidadeMaximaApresentacao, Environment.NewLine);
            }
            else if (QtdApresentacoes < this.QuantidadeMinimaApresentacao)
            {
                this.StatusOk = false;
                MensagemRetorno += string.Format("A Série: {0} deve possuir no Mínimo {1} apresentações distintas em sua reserva.{2}",
                    this.SerieNome, this.QuantidadeMinimaApresentacao, Environment.NewLine);
            }

            if (ErroQuantidadeApresentacao.Length > 0)
            {
                this.StatusOk = false;
                MensagemRetorno += string.Format("Os ingressos reservados para a série: {0} possuem quantidades diferentes por apresentação.{1}{2}{3}",
                    this.SerieNome, Environment.NewLine, Environment.NewLine, ErroQuantidadeApresentacao);

            }
        }

        public void Sort()
        {

            ListaApresentacoes.Sort(
                delegate(EstruturaSerieQuantidades qtd1, EstruturaSerieQuantidades qtd2)
                {
                    return qtd1.QuantidadeReservada.CompareTo(qtd2.QuantidadeReservada);
                });
        }
    }

    [Serializable]
    public class EstruturaSerieQuantidades
    {
        public int EventoID { get; set; }
        public string Evento { get; set; }

        public int ApresentacaoID { get; set; }
        public string Apresentacao { get; set; }

        public int QuantidadeReservada { get; set; }
        public bool Ok { get; set; }
    }
}
