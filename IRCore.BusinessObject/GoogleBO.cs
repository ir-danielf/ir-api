using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IRCore.DataAccess.Model;
using IRCore.Util;

namespace IRCore.BusinessObject
{
    public class GoogleBO
    {
        public List<GoogleMarkup> GetMarkups(int eventoId)
        {
            try
            {
                var eventoBO = new EventoBO();
                var evento = eventoBO.Consultar(eventoId);

                if (evento == null)
                {
                    return null;
                }

                LogUtil.Debug(string.Format("##Google.GoogleMarkups.GetMarkup## EVENTO {0}", eventoId));

                #region [ Event Types - http://schema.org | SiteIR DataBase]

                var eventsType = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Clássicos", "MusicEvent"),
                    new KeyValuePair<string, string>("Concerto", "MusicEvent"),
                    new KeyValuePair<string, string>("Jazz", "MusicEvent"),
                    new KeyValuePair<string, string>("Ópera", "MusicEvent"),
                    new KeyValuePair<string, string>("Show", "MusicEvent"),
                    new KeyValuePair<string, string>("Futebol", "SportsEvent"),
                    new KeyValuePair<string, string>("Esporte", "SportsEvent"),
                    new KeyValuePair<string, string>("Cinema", "ScreeningEvent"),
                    new KeyValuePair<string, string>("Dança", "DanceEvent"),
                    new KeyValuePair<string, string>("Especiais", "SocialEvent"),
                    new KeyValuePair<string, string>("Festas", "SocialEvent"),
                    new KeyValuePair<string, string>("Infantil e Familiares", "ChildrensEvent"),
                    new KeyValuePair<string, string>("Teatro", "TheaterEvent")
                };

                var locationsType = new List<string>
                {
                    "Place",
                    "EventVenue"
                };

                var offersType = new List<string>
                {
                    "Offer",
                    "AggregateOffer"
                };

                #endregion

                var urlEvento = string.Format("https://www.ingressorapido.com.br/compras/?id={0}", eventoId);

                #region [ Address ]

                var address = new GoogleMarkup.Address
                {
                    type = "PostalAddress"
                };

                if (!string.IsNullOrWhiteSpace(evento.Local.Endereco))
                {
                    address.streetAddress = evento.Local.Endereco;
                }

                if (!string.IsNullOrWhiteSpace(evento.Local.Cidade))
                {
                    address.addressLocality = evento.Local.Cidade;
                }

                if (!string.IsNullOrWhiteSpace(evento.Local.Estado))
                {
                    address.addressRegion = evento.Local.Estado;
                }

                if (!string.IsNullOrWhiteSpace(evento.Local.CEP))
                {
                    address.postalCode = evento.Local.CEP;
                }

                if (!string.IsNullOrWhiteSpace(evento.Local.Pais) && evento.Local.Pais == "Brasil")
                {
                    address.addressCountry = "BR";
                }

                var location = new GoogleMarkup.Location
                {
                    name = evento.Local.Nome,
                    address = address,
                    type = evento.Apresentacao.Count > 1
                        ? locationsType[0]
                        : locationsType[1]
                };

                #endregion

                var setorBO = new SetorBO();

                var markups = (from apresentacao in evento.Apresentacao
                               let setores = setorBO.Listar(apresentacao.IR_ApresentacaoID)
                               where setores.Any()
                               let offers = (from setor in setores
                                             from preco in setor.Preco
                                             let nomePreco = preco.Nome
                                             let valor = preco.Valor
                                             where valor != null
                                             select new GoogleMarkup.Offer
                                             {
                                                 type = offersType[0],
                                                 name = string.Format("{0} - {1}", setor.Nome, preco.Nome),
                                                 price = valor.Value.ToString(CultureInfo.InvariantCulture),
                                                 priceCurrency = "BRL",
                                                 url = urlEvento,
                                                 category = nomePreco.ToUpper().Contains("INTEIRA") ? "primary" : "premium",
                                                 availability = setor.QtdeDisponivel > 0 ? "http://schema.org/InStock" : "http://schema.org/SoldOut"
                                             }).ToList()
                               select new GoogleMarkup
                               {
                                   context = "http://schema.org",
                                   name = evento.Nome,
                                   location = location,
                                   offers = offers,
                                   startDate = apresentacao.CalcHorario.ToString(),
                                   url = urlEvento,
                                   type = !string.IsNullOrWhiteSpace(evento.Tipo.Nome)
                                   ? eventsType.FirstOrDefault(x => x.Key.Equals(evento.Tipo.Nome)).Value
                                   : null
                               }).ToList();

                LogUtil.Debug(string.Format("##Google.GoogleMarkups.GetMarkup.SUCCESS## EVENTO {0} ", eventoId));
                return markups;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##Google.GoogleMarkups.EXCEPTION## EVENTO {0}, MSG {1}, STACKTRACE {2}", eventoId, ex.Message, ex.StackTrace));

                return null;
            }
        }
    }
}
