using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects.Serie
{
    [Serializable]
    public class EstruturaValidacaoSeries
    {
        public List<EstruturaValidacaoSerie> Series { get; set; }
        private EstruturaValidacaoSerie SerieAtual { get; set; }
        private EstruturaMontagem Montagem { get; set; }

        private int SerieIDAtual { get; set; }
        private int ApresentacaoIDAtual { get; set; }

        public EstruturaValidacaoSeries()
        {
            Series = new List<EstruturaValidacaoSerie>();
            Montagem = new EstruturaMontagem();
        }

        public void AdicionarItem(
            string serieNome, int serieID, int eventoID, string evento, int apresentacaoID, string apresentacao,
           int quantidadeMinGrupos, int quantidademaxGrupos,
            int quantidadeMinimaApresentacoes, int quantidadeMaximaApresentacoes,
            int quantidadaeMinIngressosPorApresentacao, int quantidadeMaxIngressosPorApresentacao,
            bool promocional, int quantidadePorPromocional)
        {
            if (!string.IsNullOrEmpty(apresentacao) && apresentacao.Length > 19)
                apresentacao = apresentacao.Substring(0, apresentacao.Length - 3);

            //Mudou a apresentação, precisa verificar se as quantidades estão OK para cada SerieItem
            //Quantidade de Normais por Promocional
            if (ApresentacaoIDAtual != apresentacaoID && ApresentacaoIDAtual > 0)
                this.AplicarItem();

            //É uma nova Série, precisa adicionar o novo item
            if (SerieIDAtual != serieID)
            {
                SerieAtual = new EstruturaValidacaoSerie(
                    serieID, serieNome, quantidadeMinGrupos, quantidademaxGrupos,
                    quantidadeMinimaApresentacoes, quantidadeMaximaApresentacoes,
                    quantidadaeMinIngressosPorApresentacao, quantidadeMaxIngressosPorApresentacao);

                Series.Add(SerieAtual);
            }

            this.Montagem.AdicionarQuantidades(eventoID, evento, apresentacaoID, apresentacao, promocional, quantidadePorPromocional);

            ApresentacaoIDAtual = apresentacaoID;
            SerieIDAtual = serieID;

        }

        public void AplicarItem()
        {
            //A quantidade aplicada é a subtração de Normais por Normais por Promocional, se faltar algo, fica negativo
            if (Montagem.QuantidadeAplicada < 0)
                SerieAtual.MensagensPromocional.Add(string.Format(
                    "Você ainda precisa reservar mais {0} ingressos normais para o Evento: {1} na Apresentação {2}\n",
                    Montagem.QuantidadeAplicada * -1, Montagem.Evento, Montagem.Apresentacao));

            SerieAtual.Adicionar(Montagem.EventoID, Montagem.Evento, Montagem.ApresentacaoID, Montagem.Apresentacao,
                                 Montagem.QuantidadeNormais, Montagem.QuantidadePromocionais);

            this.Montagem = new EstruturaMontagem();
        }

        public void MontarGrupos()
        {
            foreach (var serie in this.Series)
                serie.MontarGrupo();
        }

        public bool ContemErro()
        {
            for (int i = 0; i < this.Series.Count; i++)
                if (this.Series[i].TemErro)
                    return true;

            return false;
        }
    }

    [Serializable]
    public class EstruturaValidacaoSerie
    {
        public EstruturaValidacaoSerie(
            int serieID, string serieNome,
            int quantidadeMinGrupos, int quantidademaxGrupos,
            int quantidadeMinimaApresentacoes, int quantidadeMaximaApresentacoes,
            int quantidadaeMinIngressosPorApresentacao, int quantidadeMaxIngressosPorApresentacao)
        {
            SerieID = serieID;
            SerieNome = serieNome;
            QuantidadeMinimaGrupos = quantidadeMinGrupos;
            QuantidadeMaximaGrupos = quantidademaxGrupos;
            QuantidadeMinimaApresetacoes = quantidadeMinimaApresentacoes;
            QuantidadeMaximaApresentacoes = quantidadeMaximaApresentacoes;
            QuantidadeMinimaIngressosPorApresentacao = quantidadaeMinIngressosPorApresentacao;
            QuantidadeMaximaIngressosPorApresentacao = quantidadeMaxIngressosPorApresentacao;
            this.Grupos = new LinkedList<EstruturaSerieGrupo>();
            this.MensagensPromocional = new List<string>();
        }

        public int SerieID { get; set; }
        public string SerieNome { get; set; }

        public int QuantidadeMinimaGrupos { get; set; }
        public int QuantidadeMaximaGrupos { get; set; }

        public int QuantidadeMinimaApresetacoes { get; set; }
        public int QuantidadeMaximaApresentacoes { get; set; }

        public int QuantidadeMinimaIngressosPorApresentacao { get; set; }
        public int QuantidadeMaximaIngressosPorApresentacao { get; set; }

        public LinkedList<EstruturaSerieGrupo> Grupos { get; set; }
        private LinkedListNode<EstruturaSerieGrupo> Grupo { get; set; }
        public string MensagemGrupo { get; set; }
        public string MensagemApresentacao { get; set; }
        public List<string> MensagensPromocional { get; set; }

        public bool TemErro
        {
            get
            {
                bool temErro =
                 (!string.IsNullOrEmpty(MensagemGrupo) ||
                   !string.IsNullOrEmpty(MensagemApresentacao) ||
                   MensagensPromocional.Count > 0);

                if (temErro)
                    return true;

                Grupo = Grupos.First;

                while (Grupo != null)
                    if (Grupo.Value.TemErro)
                        return true;
                    else
                        Grupo = Grupo.Next;

                return false;
            }
        }

        public void Adicionar(int eventoID, string evento, int apresentacaoID, string apresentacao, int quantidadeNormais, int quantidadePromocionais)
        {



            Grupo = Grupos.First;

            int quantidadeTotal = 0;
            int quantidadeRestante = quantidadeTotal = quantidadeNormais + quantidadePromocionais;

            while (quantidadeRestante > 0)
            {
                if (Grupos.Count == 0)
                {
                    Grupos.AddFirst(new EstruturaSerieGrupo());
                    Grupo = Grupos.First;
                }
                else if (Grupo.Next == null && quantidadeRestante != quantidadeTotal)
                {
                    Grupos.AddAfter(Grupo, new EstruturaSerieGrupo());
                    Grupo = Grupo.Next;
                }
                else if (quantidadeRestante != quantidadeTotal)
                    Grupo = Grupo.Next;


                int q = quantidadeRestante - QuantidadeMaximaIngressosPorApresentacao;

                if (q <= 0)
                    Grupo.Value.Adicionar(eventoID, evento, apresentacaoID, apresentacao, quantidadeRestante, q);
                else
                    Grupo.Value.Adicionar(eventoID, evento, apresentacaoID, apresentacao, QuantidadeMaximaIngressosPorApresentacao, 0);

                quantidadeRestante = q;
            }
        }

        public void MontarGrupo()
        {
            var grupoCompleto = Grupos.First;
            Grupo = grupoCompleto;
            this.VerificarQuantidadeGrupos();
            this.VerificarQuantidadeApresentacoes(grupoCompleto.Value);
            while (Grupo != null && Grupo.Value != null)
            {
                Grupo.Value.Comparar(grupoCompleto.Value, this.QuantidadeMinimaIngressosPorApresentacao);
                Grupo = Grupo.Next;
            }

            Grupo = Grupos.First;
        }

        private void VerificarQuantidadeGrupos()
        {
            if (this.QuantidadeMinimaGrupos > this.Grupos.Count)
                this.MensagemGrupo =
                    string.Format("A quantidade mínima de grupos ainda não foi atingida, por favor, inclua mais {0}.\n",
                    this.Grupos.Count - this.QuantidadeMinimaGrupos == 1 ?
                    "1 grupo" : this.Grupos.Count - this.QuantidadeMinimaGrupos + " grupos");
            else if (this.QuantidadeMaximaGrupos > 0 && this.QuantidadeMaximaGrupos < this.Grupos.Count)
                this.MensagemGrupo =
                    string.Format("A quantidade máxima de grupos foi excedida, por favor remova {0}.\n",
                    this.Grupos.Count - this.QuantidadeMaximaGrupos == 1 ?
                    "1 grupo" : (this.Grupos.Count - this.QuantidadeMaximaGrupos) + " grupos");
        }

        private void VerificarQuantidadeApresentacoes(EstruturaSerieGrupo estruturaSerieGrupo)
        {
            if (this.QuantidadeMinimaApresetacoes > estruturaSerieGrupo.QuantidadeApresentacoes)
                this.MensagemApresentacao = string.Format("A quantidade mínima de apresentações ainda não foi atingida, por favor, reserve mais {0}.\n",
                    this.QuantidadeMinimaApresetacoes - estruturaSerieGrupo.QuantidadeApresentacoes == 1 ?
                    "1 apresentação" : (this.QuantidadeMinimaApresetacoes - estruturaSerieGrupo.QuantidadeApresentacoes) + " apresentações");
            else if (this.QuantidadeMaximaApresentacoes > 0 && this.QuantidadeMaximaApresentacoes < estruturaSerieGrupo.QuantidadeApresentacoes)
                this.MensagemApresentacao =
                    string.Format("A quantidade máxima de apresentações foi excedida. por favor, remova {0}.\n",
                    estruturaSerieGrupo.QuantidadeApresentacoes - this.QuantidadeMaximaApresentacoes == 1 ?
                    "1 apresentação" : (estruturaSerieGrupo.QuantidadeApresentacoes - this.QuantidadeMaximaApresentacoes) + " apresentações");
        }
    }

    [Serializable]
    public class EstruturaSerieGrupo
    {
        public EstruturaSerieGrupo()
        {
            this.Apresentacoes = new List<EstruturaSerieQuantidade>();
        }

        public int QuantidadeApresentacoes { get; set; }

        public List<EstruturaSerieQuantidade> Apresentacoes { get; set; }

        public void Adicionar(int eventoID, string evento, int apresentacaoID, string apresentacao, int quantidade, int quantidadePendente)
        {
            QuantidadeApresentacoes++;
            Apresentacoes.Add(new EstruturaSerieQuantidade()
            {
                EventoID = eventoID,
                Evento = evento,
                ApresentacaoID = apresentacaoID,
                Apresentacao = apresentacao,
                Quantidade = quantidade,
                QuantidadePendente = quantidadePendente
            });
        }

        public void Comparar(EstruturaSerieGrupo grupoCompleto, int qtdMinimaIngressosPorApresentacao)
        {
            foreach (var apresentacao in grupoCompleto.Apresentacoes)
            {
                var encontrado = this.Apresentacoes.Find(delegate(EstruturaSerieQuantidade itemEncontrado)
                {
                    return itemEncontrado.ApresentacaoID == apresentacao.ApresentacaoID;
                });

                //Não tem reservas desta apresentacao, precisa inserir os registros com a quantidade MINIMA pendente
                if (encontrado == null)
                    this.Apresentacoes.Add(new EstruturaSerieQuantidade()
                    {
                        Evento = apresentacao.Evento,
                        EventoID = apresentacao.EventoID,
                        Apresentacao = apresentacao.Apresentacao,
                        ApresentacaoID = apresentacao.ApresentacaoID,
                        QuantidadePendente = qtdMinimaIngressosPorApresentacao,
                    });
                //Possui a apresentação mas a quantidade de ingressos reservados é MENOR do que a mínima aceitavel
                else if (encontrado.Quantidade != apresentacao.Quantidade || encontrado.Quantidade < qtdMinimaIngressosPorApresentacao)
                    encontrado.QuantidadePendente = qtdMinimaIngressosPorApresentacao - encontrado.Quantidade;

            }
        }

        public bool TemErro
        {
            get
            {
                for (int i = 0; i < this.Apresentacoes.Count; i++)
                {
                    if (this.Apresentacoes[i].QuantidadePendente > 0)
                        return true;
                }
                return false;
            }
        }
    }

    [Serializable]
    public class EstruturaSerieQuantidade
    {
        public int EventoID { get; set; }
        public string Evento { get; set; }
        public int ApresentacaoID { get; set; }
        public string Apresentacao { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePendente { get; set; }
    }

    [Serializable]
    public class EstruturaMontagem
    {
        public int EventoID { get; set; }
        public string Evento { get; set; }
        public int ApresentacaoID { get; set; }
        public string Apresentacao { get; set; }

        public int QuantidadeNormais { get; set; }
        public int QuantidadePromocionais { get; set; }
        public int QuantidadeAplicada { get; set; }

        public void AdicionarQuantidades(
            int eventoID, string evento,
            int apresentacaoID, string apresentacao,
            bool promocional, int quantidadePorPromocional)
        {
            this.EventoID = eventoID;
            this.Evento = evento;
            this.ApresentacaoID = apresentacaoID;
            this.Apresentacao = apresentacao;

            if (promocional)
            {
                this.QuantidadePromocionais++;
                this.QuantidadeAplicada -= quantidadePorPromocional;
            }
            else
            {
                this.QuantidadeNormais++;
                this.QuantidadeAplicada++;
            }
        }
    }


    //Classes utilizadas pelo SITE
    public class EstruturaValidacaoSerieSite
    {
        public EstruturaValidacaoSerieSite() { this.Grupos = new List<EstruturaValidacaoGrupoSite>(); MensagensIngressos = new List<string>(); }
        public string MensagemGrupos { get; set; }
        public string MensagemApresentacoes { get; set; }
        public List<string> MensagensIngressos { get; set; }
        public List<EstruturaValidacaoGrupoSite> Grupos { get; set; }

        public EstruturaValidacaoSerieSite Converter(EstruturaValidacaoSeries series)
        {
            if (series == null)
                throw new SerieException("Não foi possível localizar nenhum ingresso reservado para esta série, por favor, reserve os ingressos e tente novamente.");

            var serie = series.Series[0]; //Soh tem 1
            if (!string.IsNullOrEmpty(serie.MensagemGrupo))
                this.MensagemGrupos = serie.MensagemGrupo;
            else
                this.MensagemGrupos = string.Empty;

            if (!string.IsNullOrEmpty(serie.MensagemApresentacao))
                this.MensagemApresentacoes = serie.MensagemApresentacao;
            else
                this.MensagemApresentacoes = string.Empty;

            if (serie.MensagensPromocional.Count > 0)
                foreach (string mensagem in serie.MensagensPromocional)
                    this.MensagensIngressos.Add(mensagem);

            int g = 1;
            var grupoAdicionar = new EstruturaValidacaoGrupoSite(g);

            foreach (var grupo in serie.Grupos)
            {
                foreach (var apresentacao in grupo.Apresentacoes)
                {
                    for (int i = 0; i < apresentacao.Quantidade; i++)
                    {
                        grupoAdicionar.Reservas.Add(new EstruturaValidacaoReservasSite()
                        {
                            Evento = apresentacao.Evento,
                            Apresentacao = apresentacao.Apresentacao,
                            Tipo = 1,
                        });
                        grupoAdicionar.Tipo = 1;
                    }
                    for (int i = 0; i < apresentacao.QuantidadePendente; i++)
                    {
                        grupoAdicionar.Reservas.Add(new EstruturaValidacaoReservasSite()
                        {
                            Evento = apresentacao.Evento,
                            Apresentacao = apresentacao.Apresentacao,
                            Tipo = 2,
                        });
                        grupoAdicionar.Tipo = 2;
                    }
                }

                this.Grupos.Add(grupoAdicionar);
                g++;
                grupoAdicionar = new EstruturaValidacaoGrupoSite(g);
            }

            return this;
        }
    }

    public class EstruturaValidacaoGrupoSite
    {
        public EstruturaValidacaoGrupoSite(int numero) { this.NumeroGrupo = numero; this.Reservas = new List<EstruturaValidacaoReservasSite>(); }
        public int NumeroGrupo { get; set; }
        public int Tipo { get; set; } //1 = OK -- 2 = Erro
        public List<EstruturaValidacaoReservasSite> Reservas { get; set; }
    }

    public class EstruturaValidacaoReservasSite
    {
        public string Evento { get; set; }
        public string Apresentacao { get; set; }
        public int Tipo { get; set; } //1 = Reservado -- 2 = Precisa Reservar
    }
}
