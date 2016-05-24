using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRCore.Util;

namespace IngressoRapido.Lib
{
    public static class Extensions
    {
        public static int ToInt32(this object valor)
        {
            try
            {
                if (valor == DBNull.Value)
                    return 0;
                return Convert.ToInt32(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para Inteiro não é válida.Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static double ToDouble(this object valor)
        {
            try
            {
                if (valor == DBNull.Value)
                    return 0;

                return Convert.ToDouble(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para Double não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static decimal ToDecimal(this object valor)
        {
            try
            {
                if (valor == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para Decimal não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static bool ToBoolean(this object valor)
        {
            try
            {
                if (valor == DBNull.Value)
                    return false;

                return Convert.ToBoolean(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para Boolean não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static DateTime ToDateTimeFromDB(this object valor)
        {
            try
            {
                return DateTime.ParseExact(valor.ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para DateTime não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static DateTime ToDateTime(this object valor)
        {
            try
            {
                return Convert.ToDateTime(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para DateTime não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }

        public static DateTime ToParseDateTime(this object valor)
        {
            try
            {
                return DateTime.ParseExact(valor.ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para DateTime não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }


        public static Char ToChar(this object valor)
        {
            try
            {
                return Convert.ToChar(valor);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("A conversão do valor: {0} para Char não é válida. Erro Genérico: {1}", valor, ex.Message));
            }
        }


        public static string ToJson(this Carrinho oCarrinho)
        {
            try
            {
                if (oCarrinho.ListaCotaItem == null && oCarrinho.CotaItem != null)
                    (oCarrinho.ListaCotaItem = new List<CotaItem>()).Add(oCarrinho.CotaItem);

                return JsonConvert.SerializeObject(new
                {
                    Ret = 1,
                    Tipo = Convert.ToChar(oCarrinho.TipoLugar),
                    CarrinhoID = oCarrinho.ID,
                    oCarrinho.IngressoID,
                    oCarrinho.LugarID,
                    EventoID = oCarrinho.EventoID.ToString("000000"),
                    oCarrinho.Evento,
                    oCarrinho.ApresentacaoID,
                    Apresentacao = oCarrinho.ApresentacaoDataHora.ToString("dd/MM/yy à\\s HH:mm"),
                    oCarrinho.Local,
                    oCarrinho.Setor,
                    oCarrinho.PrecoID,
                    Preco = oCarrinho.PrecoNome,
                    oCarrinho.Codigo,
                    Valor = oCarrinho.PrecoValor.ToString("c"),
                    Conveniencia = oCarrinho.TaxaConveniencia.ToString("c"),
                    Total = (oCarrinho.PrecoValor + oCarrinho.TaxaConveniencia).ToString("c"),
                    TotalM = oCarrinho.PrecoValor + oCarrinho.TaxaConveniencia,
                    Status = oCarrinho.StatusDetalhado,
                    oCarrinho.SerieID,
                    oCarrinho.PacoteID,
                    oCarrinho.PacoteGrupo,
                    oCarrinho.PacoteNome,
                    oCarrinho.Precos,
                    oCarrinho.ListaCotaItem,
                    TemCota = (oCarrinho.ListaCotaItem != null && oCarrinho.ListaCotaItem.Count() > 0),
                    ExibirDados = (oCarrinho.ListaCotaItem != null && oCarrinho.ListaCotaItem.Count() > 0 && oCarrinho.ListaCotaItem.Where(c => c.ExibirDados).Count() > 0),
                    ExibirTermos = (oCarrinho.ListaCotaItem != null && oCarrinho.ListaCotaItem.Count() > 0 && oCarrinho.ListaCotaItem.Where(c => c.TemTermo).Count() > 0),
                    oCarrinho.SpecialEvent,
                    oCarrinho.TaxaProcessamento,
                    oCarrinho.Estado,
                    oCarrinho.SeguroMondial
                }, Formatting.None);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter o carrinho para o formato de exibição. Erro: " + ex.Message);
            }
        }

        /// <summary>
        /// Os itens do Carrinho vem sempre Desagrupados, independente se tem Cota ou Não, nesta extension, o carrinho será Reagrupado
        /// Somando os valores e adicionando os cotaitem se existirem
        /// </summary>
        /// <param name="lstCarrinho"></param>
        /// <returns></returns>
        public static string ToJson(this List<Carrinho> lstCarrinho)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                if (!string.IsNullOrEmpty(lstCarrinho.FirstOrDefault().PacoteGrupo))
                {
                    foreach (string grupo in lstCarrinho.Select(c => c.PacoteGrupo).Distinct().OrderBy(c => c))
                    {
                        List<Carrinho> listaFiltrada = lstCarrinho.Where(c => c.PacoteGrupo == grupo).ToList();
                        foreach (Carrinho oCarrinho in listaFiltrada)
                        {
                            //Mantem desagrupado
                            oCarrinho.TipoLugar = (oCarrinho.TipoLugar == ((char)Setor.LugarTipo.Cadeira).ToString() ?
                                ((char)Pacote.TipoPacote.Assinatura).ToString() : ((char)Pacote.TipoPacote.Pista).ToString());

                            oCarrinho.PrecoValor = listaFiltrada.Sum(c => c.PrecoValor);
                            oCarrinho.TaxaConveniencia = listaFiltrada.Sum(c => c.TaxaConveniencia);
                            oCarrinho.Total = listaFiltrada.Sum(c => c.PrecoValor) + listaFiltrada.Sum(c => c.TaxaConveniencia);
                            (oCarrinho.ListaCotaItem = new List<CotaItem>()).AddRange(listaFiltrada.Where(c => c.CotaItem != null).Select(c => c.CotaItem));
                            oCarrinho.Estado = listaFiltrada.FirstOrDefault().Estado;
                            oCarrinho.TaxaProcessamento = CarrinhoLista.CalcularTaxaProcessamento(listaFiltrada);
                            stb.Append(oCarrinho.ToJson() + ", ");
                            break;
                        }
                    }
                }
                //É necessáriamente uma mesa fechada
                else
                {
                    foreach (int lugarID in lstCarrinho.Select(c => c.LugarID).Distinct().OrderBy(c => c))
                    {
                        IEnumerable<Carrinho> listaFiltrada = lstCarrinho.Where(c => c.LugarID == lugarID);
                        foreach (Carrinho oCarrinho in listaFiltrada)
                        {
                            oCarrinho.PrecoValor = listaFiltrada.Sum(c => c.PrecoValor);
                            oCarrinho.TaxaConveniencia = listaFiltrada.Sum(c => c.TaxaConveniencia);
                            oCarrinho.Total = listaFiltrada.Sum(c => c.PrecoValor) + listaFiltrada.Sum(c => c.TaxaConveniencia);
                            oCarrinho.ListaCotaItem = new List<CotaItem>();
                            oCarrinho.ListaCotaItem.AddRange(listaFiltrada.Where(c => c.isCota.Length > 0).Select(c => c.CotaItem));
                            stb.Append(oCarrinho.ToJson() + ", ");
                            break;

                        }
                    }
                }
                return stb.Remove(stb.Length - 2, 2).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter o carrinho para o formato de exibição. Erro: " + ex.Message);
            }
        }

        public static string ToCEP(this string valor)
        {
            LogUtil.Debug(string.Format("##RoboAtzSite.ToCEP## CEP: {0}", valor));

            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            valor = valor.Replace("-", string.Empty);

            if (valor.Length == 8)
                return valor;

            LogUtil.Debug(string.Format("O CEP '{0}' não está em um formato correto.", valor));
            throw new Exception(string.Format("O CEP '{0}' não está em um formato correto.", valor));
        }
    }
}
