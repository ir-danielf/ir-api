using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class FormaPagamentoBO : MasterBO<FormaPagamentoADO>
    {
        public FormaPagamentoBO(MasterADOBase ado) : base(ado) { }
        public FormaPagamentoBO() : base(null) { }


        public List<FormaPagamento> ListarEvento(int eventoID)
        {
            List<FormaPagamento> formasPagamento = ado.ListarEvento(eventoID);
            if (formasPagamento != null)
            {
                DateTime dataInicial = new DateTime();
                DateTime dataFinal = new DateTime();
                try
                {
                    dataInicial = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InicioAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    dataFinal = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.FimAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    dataInicial = dataFinal = DateTime.MaxValue;
                }
                DateTime dataAtual = DateTime.Now;
                if (dataAtual >= dataInicial && dataAtual <= dataFinal)
                    foreach (var item in formasPagamento.Where(x => x.NomeAsEnum == DataAccess.Model.Enumerator.enumFormaPagamento.Amex))
                        item.Parcelas = Convert.ToByte(item.Parcelas++ < 10 ? item.Parcelas++ : 10);
            }

            return formasPagamento;

        }
        public List<FormaPagamento> ListarEvento(List<int> eventos, decimal valorCompra)
        {
            List<FormaPagamento> formasPagamento = ado.ListarEvento(eventos.Distinct().ToList());
            if (formasPagamento != null)
            {
                DateTime dataInicial = new DateTime();
                DateTime dataFinal = new DateTime();
                try
                {
                    dataInicial = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InicioAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    dataFinal = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.FimAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    dataInicial = dataFinal = DateTime.MaxValue;
                }
                DateTime dataAtual = DateTime.Now;


                //AMEX, Ver se o método da IRLIB já faz essa verificação
                if (dataAtual >= dataInicial && dataAtual <= dataFinal && valorCompra >= ConfiguracaoAppUtil.GetAsDecimal(enumConfiguracaoBO.ValorAmex))
                    foreach (var item in formasPagamento.Where(x => x.NomeAsEnum == DataAccess.Model.Enumerator.enumFormaPagamento.Amex))
                        item.Parcelas = Convert.ToByte(item.Parcelas++ < 10 ? item.Parcelas++ : 10);
            }
            return formasPagamento;
        }

        public List<FormaPagamento> ListarEvento(int ClienteID, string sessionID, decimal valorCompra)
        {
            LogUtil.Debug(string.Format("##FormaPagamentoBO.ObtendoFormaPagamento## SESSION {0}", sessionID));

            DateTime dataInicial = new DateTime();
            DateTime dataFinal = new DateTime();
            try
            {
                dataInicial = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.InicioAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                dataFinal = DateTime.ParseExact(ConfiguracaoAppUtil.Get(enumConfiguracaoBO.FimAmex), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch
            {
                dataInicial = dataFinal = DateTime.MaxValue;
            }
            DateTime dataAtual = DateTime.Now;


            //Carrega as bandeiras dos cartões
            List<IngressoRapido.Lib.FormaPagamento.Bandeiras> formasPagamentoProxy = new IngressoRapido.Lib.FormaPagamento().FormasDePagamentoPrioritarias(ClienteID, sessionID);
            LogUtil.Debug(string.Format("##FormaPagamentoBO.ObtendoFormaPagamento## SESSION {0}, PGTO_COUNT {1}", sessionID, formasPagamentoProxy.Count));

            List<FormaPagamento> formasPagamento = new List<FormaPagamento>();
            foreach (var item in formasPagamentoProxy)
            {
                int maxParcelas = new IngressoRapido.Lib.FormaPagamento().ParcelaMaxima(item, ClienteID, sessionID);

                enumFormaPagamento bandeira = ConvertBandeira(item);

                if (bandeira != enumFormaPagamento.Nenhuma)
                {

                    if (bandeira == enumFormaPagamento.Amex)
                        if (dataAtual >= dataInicial && dataAtual <= dataFinal && valorCompra >= ConfiguracaoAppUtil.GetAsDecimal(enumConfiguracaoBO.ValorAmex))
                            maxParcelas = maxParcelas < 10 ? maxParcelas++ : 10;

                    formasPagamento.Add(new FormaPagamento() { NomeAsEnum = bandeira, Parcelas = ((byte)maxParcelas) });
                }
            }

            LogUtil.Debug(string.Format("##FormaPagamentoBO.ObtendoFormaPagamento.SUCCESS## SESSION {0}, PGTO_COUNT {1}", sessionID, formasPagamento.Count));

            return formasPagamento;
        }

        private enumFormaPagamento ConvertBandeira(IngressoRapido.Lib.FormaPagamento.Bandeiras item)
        {
            enumFormaPagamento bandeira;
            switch (item)
            {
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.Amex:
                    bandeira = enumFormaPagamento.Amex;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.Aura:
                    bandeira = enumFormaPagamento.Aura;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.Diners:
                    bandeira = enumFormaPagamento.Diners;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.Elo:
                    bandeira = enumFormaPagamento.Elo;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.EloCultura:
                    bandeira = enumFormaPagamento.EloCultura;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.HiperCard:
                    bandeira = enumFormaPagamento.Hipercard;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.ItauDebito:
                    bandeira = enumFormaPagamento.ItauShopline;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.PayPal:
                    bandeira = enumFormaPagamento.PayPal;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.RedecardCredito:
                    bandeira = enumFormaPagamento.RedecardCredito;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.ValeCultura:
                    bandeira = enumFormaPagamento.ValeCultura;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.VisaCredito:
                    bandeira = enumFormaPagamento.VisaCredito;
                    break;
                case IngressoRapido.Lib.FormaPagamento.Bandeiras.VisaElectron:
                    bandeira = enumFormaPagamento.VisaElectron;
                    break;
                default:
                    bandeira = enumFormaPagamento.Nenhuma;
                    break;
            }
            return bandeira;
        }

        public List<FormaPagamento> Listar(List<int> ids)
        {
            return ado.Listar(ids.Distinct().ToList());
        }

        public FormaPagamento Consultar(int idFormaPagamento)
        {
            return ado.Consultar(idFormaPagamento);
        }

    }
}
