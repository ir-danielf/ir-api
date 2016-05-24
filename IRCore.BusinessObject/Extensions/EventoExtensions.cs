using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public static class EventoExtensions
    {
        public static string GetImagemPath(this Evento evento, enumEventoImagemTamanho tamanho)
        {
            // TODO: Utilizar o parametro de tamanho para pegar a imagem certa
            return ConfiguracaoAppUtil.Get(enumConfiguracaoBO.caminhoEventoImagens) + "eI" + evento.IR_EventoID.ToString("D6") + ".jpg";
        }

        public static Evento toEvento(this tEvento tEvento)
        {
            return new Evento()
            {
                                Apresentacao = null,
                                BannersPadraoSite = null,
                                DataAberturaVenda = tEvento.DataAberturaVenda,
                                Destaque = tEvento.Destaque == "T"?true : false,
                                DestaqueRegiao = null,
                                DisponivelAvulso = null,
                                EntregaGratuita = tEvento.EntregaGratuita == "T" ? true : false,
                                EscolherLugarMarcado = tEvento.EscolherLugarMarcado == "T" ? true : false,
                                EventoMidias = tEvento.EventoMidia.ToDictionary(),
                                ExibeQuantidade =tEvento.ExibeQuantidade == "T"?true : false,
                                FilmeID = tEvento.FilmeID,
                                ID = tEvento.ID,
                                Imagem = tEvento.ImagemInternet,
                                ImagemDestaque = tEvento.ImagemDestaque,
                                IR_EventoID = tEvento.ID,
                                LimiteMaximoIngressosEstado = 0,
                                LimiteMaximoIngressosEvento = tEvento.LimiteMaximoIngressosEvento??0,
                                //Carregar
                                Local = tEvento.tLocal.toLocal(),
                                LocalID = tEvento.LocalID??0,
                                LocalImagemMapaID = tEvento.LocalImagemMapaID,
                                LocalImagemNome = string.Empty,
                                MaiorPreco = null,
                                MenorPeriodoEntrega = tEvento.MenorPeriodoEntrega,
                                MenorPreco = null,
                                NewsAssinante = null,
                                Nome = tEvento.Nome,
                                Obs = tEvento.Obs,
                                PalavraChave = tEvento.PalavraChave,
                                Parcelas = tEvento.Parcelas,
                                PermiteVendaPacote =  tEvento.PermiteVendaPacote == "T"? true : false,
                                PossuiTaxaProcessamento = false,
                                PrimeiraApresentacao = null,
                                Prioridade = tEvento.PrioridadeDestaque,
                                Publicar = tEvento.Publicar,
                                PublicarSemVendaMotivo = tEvento.PublicarSemVendaMotivo,
                                QtdeDisponivel = 0,
                                QuantidadeApresentacoes = tEvento.QuantidadeApresentacoes,
                                Release = tEvento.Release,
                                RetiradaBilheteria = null,
                                Subtipo = null,
                                SubtipoID = tEvento.EventoSubTipoID,
                                Tipo = null,
                                TipoID = tEvento.EventoTipoID,
                                UltimaApresentacao = null,
								EntradaFranca = tEvento.EntradaFranca,
								OcultarHoraApresentacao = tEvento.OcultarHoraApresentacao
                            };
        }
    }
}
