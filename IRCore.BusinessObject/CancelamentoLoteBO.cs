using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class CancelamentoLoteBO : MasterBO<CancelamentoLoteADO>
    {
        public CancelamentoLoteBO(MasterADOBase ado) : base(ado) { }
        public CancelamentoLoteBO() : base(null) { }

        public tCancelamentoLote Consultar(int ID)
        {
            return ado.Consultar(ID);
        }
        public tCancelamentoLote ConsultarComApresentacoes(int ID)
        {
            return ado.ConsultarComApresentacoes(ID);
        }
        public tCancelamentoLote ConsultarProximoSolicitado()
        {
            return ado.ConsultarProximoSolicitado();
        }
        public bool Atualizar(tCancelamentoLote CancelamentoLote)
        {
            return ado.Atualizar(CancelamentoLote);
        }
        public RetornoModel<string> ConfirmarCancelamento(List<int> apresentacoesID, int eventoID, int motivoID, string motivo,int usuarioID)
        {
            DateTime now = DateTime.Now;
            tCancelamentoLote cancelLote = new tCancelamentoLote 
            {
                CancelamentoLoteModeloMotivoID = motivoID,
                DataCancelamento = now,
                DataMovimentacao = now,
                EventoID = eventoID,
                MotivoCancelamento = motivo,
                UsuarioID = usuarioID,
                CodigoCancelamento = now.ToString("yyMMddHHmmss")
            };
            
            try
            {
                string codigoCancelamento = ado.GerarCancelamento(cancelLote, apresentacoesID);
                
                if(!string.IsNullOrEmpty(codigoCancelamento))
                    return new RetornoModel<string>{Mensagem = "OK", Retorno = codigoCancelamento, Sucesso = true};
                else
                    throw new Exception();
            }
            catch
            {
                return new RetornoModel<string> { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar o cancelamento", Retorno = string.Empty };
            }
        }

        public CancelamentoRelatorioDadosBasicos ConsultarCancelamentoRelatorioDadosBasicos(List<int> apresentacoesID = null, string codigoCancelamento = null, int cancelamentoID = 0)
        {
            return ado.ConsultarCancelamentoRelatorioDadosBasicos(apresentacoesID, codigoCancelamento, cancelamentoID);
        }

        public CancelamentoRelatorioDadosTotais ConsultarCancelamentoRelatorioDadosTotalizadores(List<int> apresentacoesID)
        {
            return ado.ConsultarCancelamentoRelatorioDadosTotalizadores(apresentacoesID);
        }

        public List<CancelamentoLoteRelatorioDadosPorApresentacao> ConsultarCancelamentoRelatorioTotalizadoresPorApresentacao(List<int> apresentacoesID)
        {
            ApresentacaoBO aprBO = new ApresentacaoBO();
            List<InformacaoVendaBasicasCancelarMassa> informacoesVenda = aprBO.CarregarInformacoesVenda(apresentacoesID);
            List<CancelamentoLoteRelatorioDadosPorApresentacao> retorno = new List<CancelamentoLoteRelatorioDadosPorApresentacao>();
            foreach (var item in informacoesVenda)
            {
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                    {
                        Horario = item.Horario,
                        Ordem = 0,
                        TituloLinha = "Data",
                        Valor = 0
                    });
                
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 1,
                    TituloLinha = "Com Identificação",
                    Valor = item.Identificados
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 2,
                    TituloLinha = "Sem Identificação",
                    Valor = item.NaoIdentificados
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 3,
                    TituloLinha = "Canal Presencial",
                    Valor = item.Presencial
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 4,
                    TituloLinha = "Canal Remoto",
                    Valor = item.Remoto
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 5,
                    TituloLinha = "Ingresso Impresso",
                    Valor = item.Impressos
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 6,
                    TituloLinha = "Ingresso Não Impresso",
                    Valor = item.NaoImpressos
                });
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                {
                    Horario = item.Horario,
                    Ordem = 8,
                    TituloLinha = "Total",
                    Valor = item.NaoImpressos + item.Impressos
                });
            }

            List<InformacaoVendaFormasPagamento> formasPagamento = aprBO.CarregarInformacoesVendaFormasPagamento(apresentacoesID);
            foreach (var item in formasPagamento)
            {
                retorno.Add(new CancelamentoLoteRelatorioDadosPorApresentacao
                    {
                        Horario = item.Horario,
                        Ordem = 10,
                        TituloLinha = item.FormaPagamento,
                        Valor = item.Quantidade
                    });
            }

            return retorno;
        }

        public List<CancelamentoRelatorioDadosOperacoes> ConsultarCancelamentoRelatorioOperacoes(List<int>apresentacoesID = null, string codigoCancelamento = null, int cancelamentoID = 0)
        {
            ApresentacaoBO aprBO = new ApresentacaoBO();
            
            List<CancelamentoRelatorioDadosOperacoes> retorno = new List<CancelamentoRelatorioDadosOperacoes>();
            if(apresentacoesID != null)
            {

                retorno = aprBO.CarregarInformacoesVendaPorOperacao(apresentacoesID).Select(x=>new CancelamentoRelatorioDadosOperacoes 
                {
                    Operacao = x.Operacao,
                    Total = x.Total,
                    Resolvido = 0,
                    Descricao = x.Descricao,
                    Cancelamento = x.Cancelamento,
                    AcaoIR = x.AcaoIR
                }).OrderBy(x=>x.Operacao).ToList();
               
            }
            else
            {
                retorno = aprBO.CarregarInformacoesVendaPorOperacao(cancelamentoID, codigoCancelamento).OrderBy(x=>x.Operacao).ToList();
            }
            return retorno;
        }

        public List<CancelamentoRelatorioMatrizApresentacoes> ConsultarMatrizApresentacoes(List<int>apresentacoesID = null,string codigoCancelamento = null,int cancelamentoID = 0)
        {
            List<DateTime> datas = new List<DateTime>();
            List<CancelamentoRelatorioMatrizApresentacoes> retorno = new List<CancelamentoRelatorioMatrizApresentacoes>();
           if(apresentacoesID != null)
           {
               ApresentacaoBO aprBO = new ApresentacaoBO();
               datas = aprBO.ConsultarDataApresentacoes(apresentacoesID);
           }
           else
           {
               datas = ado.ConsultarDataApresentacoes(codigoCancelamento, cancelamentoID);

           }


            int cont = 0;
            CancelamentoRelatorioMatrizApresentacoes cancelamentoApresentacoes = null;
            while (cont < datas.Count)
            {
                if (cont % 4 == 0)
                {
                    cancelamentoApresentacoes = new CancelamentoRelatorioMatrizApresentacoes();
                    retorno.Add(cancelamentoApresentacoes);
                }
                switch (cont % 4)
                {
                    case 0:
                        cancelamentoApresentacoes.Apresentacao1 = datas[cont].ToString("ddd, dd/MM/yyyy HH:mm");
                        break;
                    case 1:
                        cancelamentoApresentacoes.Apresentacao2 = datas[cont].ToString("ddd, dd/MM/yyyy HH:mm");
                        break;
                    case 2:
                        cancelamentoApresentacoes.Apresentacao3 = datas[cont].ToString("ddd, dd/MM/yyyy HH:mm");
                        break;
                    case 3:
                        cancelamentoApresentacoes.Apresentacao4 = datas[cont].ToString("ddd, dd/MM/yyyy HH:mm");
                        break;
                }
                cont++;
            }
            return retorno;
        }



        public string CarregarDescricaoEmailOperacao(string operacao)
        {
            return ado.CarregarDescricaoEmailOperacao(operacao);
        }
    }
}
