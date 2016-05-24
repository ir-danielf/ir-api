/**************************************************
* Arquivo: CodigoPromo.cs
* Gerado: 04/12/2009
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IRLib
{

    public class CodigoPromo : CodigoPromo_B
    {

        public CodigoPromo() { }

        public CodigoPromo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public bool ValidarCodigoPromo(int cotaItemID, int parceiroID, string codigoPromo)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tCodigoPromo (NOLOCK) ");
                stbSQL.Append("WHERE ParceiroID = " + parceiroID + " AND ");
                stbSQL.Append("Codigo LIKE '" + codigoPromo.Replace("'", "") + "'");

                int qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));

                return qtd != 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string ValidarCodigoPromo(List<EstruturaCodigoPromoValidacao> lstCodigoPromo)
        {
            try
            {
                StringBuilder stbSQL;
                int qtd = 0;
                int QuantidadeAssociada = 0;
                int QuantidadeUtilizada = 0;

                foreach (EstruturaCodigoPromoValidacao item in lstCodigoPromo)
                {
                    if (item.Tipo == Enumerators.TipoParceiro.CodigoExterno)
                    {
                        IntegracaoParceiro oParceiro = new IntegracaoParceiro();

                        if (item.ParceiroID == (int)Utils.Enums.Parceiros.PortoSeguro)
                        {
                            if (!oParceiro.ValidaCliente(item.Url, item.CodigoValidacao))
                                return ("O código " + item.CodigoValidacao + " não pode ser utilizado.");
                        }
                        else if (item.ParceiroID == (int)Utils.Enums.Parceiros.Caras)
                        {
                            if (!oParceiro.ValidaClienteCodigo(item.Url, item.CodigoValidacao, item.Identificacao))
                                return ("O código " + item.CodigoValidacao + " não pode ser utilizado.");
                        }
                        else if(item.ParceiroID == (int)Utils.Enums.Parceiros.NET)
                        {
                            if (!oParceiro.ValidaClienteNET(item.CodigoValidacao, item.Url))
                                return ("O código " + item.CodigoValidacao + " não pode ser utilizado.");
                        }
                    }
                    else
                    {
                        stbSQL = new StringBuilder();
                        stbSQL.Append("SELECT Count(ID) FROM tCodigoPromo (NOLOCK) ");
                        stbSQL.Append("WHERE ParceiroID = " + item.ParceiroID + " AND ");
                        stbSQL.Append("Codigo = '" + item.CodigoValidacao.Replace("'", string.Empty) + "'");

                        qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                        bd.FecharConsulta();
                        if (qtd < 1)
                            return ("O Código Promocional " + item.CodigoValidacao + " não é valido.");

                        bd.FecharConsulta();
                    }

                    //Faz a validacao da quantidade
                    if (item.ApresentacaoID > 0)
                    {
                        QuantidadeAssociada = lstCodigoPromo.Where(c => c.ApresentacaoID == item.ApresentacaoID && string.Compare(item.CodigoValidacao, c.CodigoValidacao) == 0).Count();

                        if (item.QuantidadePorCodigo > 0 && QuantidadeAssociada > item.QuantidadePorCodigo)
                            return ("O Código Promocional '" + item.CodigoValidacao + "' não pode ser utilizado mais de " + item.QuantidadePorCodigo + " vez(es) para a mesma apresentação.");

                        stbSQL = new StringBuilder();
                        stbSQL.Append("SELECT COUNT(ID) FROM tIngressoCliente (NOLOCK) WHERE ApresentacaoID = " + item.ApresentacaoID);
                        stbSQL.Append(" AND CodigoPromocional = '" + item.CodigoValidacao.Replace("'", string.Empty) + "' AND IngressoID <> " + item.IngressoID);

                        QuantidadeUtilizada = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));

                        if (item.QuantidadePorCodigo > 0 && QuantidadeAssociada + QuantidadeUtilizada > item.QuantidadePorCodigo)
                            return ("O Código Promocional '" + item.CodigoValidacao + "' não pode ser utilizado outras vezes para a apresentação selecionada.");

                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool ValidarBIN(int BIN, int parceiroID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tBin (NOLOCK) ");
                stbSQL.Append("WHERE ParceiroID = " + parceiroID + " AND ");
                stbSQL.Append("BIN = '" + BIN + "'");

                int qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));

                return qtd != 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class CodigoPromoLista : CodigoPromoLista_B
    {

        public CodigoPromoLista() { }

        public CodigoPromoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
