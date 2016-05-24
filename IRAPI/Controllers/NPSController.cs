using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IRAPI.Controllers
{
    [IRAPIAuthorize(enumAPIRele.venda)]
    public class NpsController : MasterApiController
    {
        /// <summary>
        /// Método da API para enviar dados diretamente para a API do NPS.
        /// </summary>
        /// <param name="Name">Nome do cliente que efetuou a compra do Ingresso</param>
        /// <param name="Email">E-mail para onde deverá ser disparado a pesquisa de satisfação</param>
        /// <param name="Delay">Tempo medido em segundos para envio do e-mail, controle este feito pela API</param>
        /// <param name="Canal">Indicar o canal de comunicacao onde foi realizada a compra do ingresso</param>
        [Route("nps/sendtodelighted")]
        [HttpPost]
        public RetornoModel SendDataNPS([FromBody]NpsAdicionarAgendamentoModel rq)
        {
            RetornoModel ret = new RetornoModel();
            ret.Mensagem = "OK";
            ret.Sucesso = true;
            try
            {
                using (var bo = new NetPromoterServiceBO())
                {
                    bo.SendDataNPS(rq.Name, rq.Email, rq.Delay, rq.Canal);
                }
            }
            catch (Exception ex)
            {
                ret.Mensagem = ex.Message;
                ret.Sucesso = false;
            }

            return ret;
        }

        /// <summary>
        /// Método da API que realiza o agendamento para o envio de uma pesquisa de satisfação ao cliente que efetua compra de
        /// ingressos na Ingresso Rápido. Este método apenas armazena o agendamento em uma tabela, um robô irá ler os dados
        /// gravados nesta tabela para chamar uma API que realiza o envio do e-mail com a pesquisa de satisfação.
        /// URL de acesso: nps/adicionar
        /// Método de acesso: POST
        /// </summary>
        /// <param name="Name">Nome do cliente que efetuou a compra do Ingresso</param>
        /// <param name="Email">E-mail para onde deverá ser disparado a pesquisa de satisfação</param>
        /// <param name="Delay">Tempo medido em segundos para envio do e-mail, controle este feito pela API</param>
        /// <param name="Canal">Indicar o canal de comunicacao onde foi realizada a compra do ingresso</param>
        [Route("nps/adicionar")]
        [HttpPost]
        public RetornoModel AdicionarAgendamento([FromBody]NpsAdicionarAgendamentoModel rq)
        {
            RetornoModel ret = new RetornoModel();
            ret.Mensagem = "OK";
            ret.Sucesso = true;
            try
            {
                using (var bo = new NetPromoterServiceBO())
                {
                    bo.AdicionarAgendamento(rq.Name, rq.Email, rq.Delay, rq.Canal);
                }
            }
            catch (Exception ex)
            {
                ret.Mensagem = ex.Message;
                ret.Sucesso = false;
            }

            return ret;
        }

        /// <summary>
        /// Método da API que realiza a alteração de status de envio do agendamento da pesquisa de satisfação
        /// URL de acesso: nps/atualizar
        /// Método de acesso: POST
        /// </summary>
        /// <param name="IdAgendamento">Código do Agendamento</param>
        /// <param name="Status">Informar o status que será atualizado o registro. Status Esperados = [Aguardando, Sucesso]</param>
        [Route("nps/atualizar")]
        [HttpPost]
        public RetornoModel AtualizarAgendamento([FromBody]NpsAtualizarAgendamento rq)
        {
            RetornoModel ret = new RetornoModel();
            ret.Mensagem = "OK";
            ret.Sucesso = true;

            if (rq == null)
            {
                ret.Mensagem = "Erro ao obter os dados para realizar o agendamento";
                ret.Sucesso = false;
                return ret;
            }

            if (rq.IdAgendamento < 0) // Se IdAgendamento for menor que zero, então...
            {
                ret.Mensagem = "IdAgendamento não foi encontrado";
                ret.Sucesso = false;
                return ret;
            }

            try
            {
                // Atualizar o status do agendamento
                using (var bo = new NetPromoterServiceBO())
                {
                    DateTime? DataEnvio = null;
                    if (rq.Status == enumStatusNPS.Sucesso) // Se status for de sucesso, atualizo a data que foi realizado o envio dos dados
                        DataEnvio = DateTime.Now;

                    bo.AtualizarAgendamento(Convert.ToString(rq.IdAgendamento), rq.Status.ValueAsString(), DataEnvio);
                }
            }
            catch (Exception ex)
            {
                ret.Mensagem = ex.Message;
                ret.Sucesso = false;
            }

            return ret;
        }

        /// <summary>
        /// Método da API que realiza a alteração por range inicial e final do status de envio do agendamento da pesquisa de satisfação
        /// URL de acesso: nps/atualizar
        /// Método de acesso: POST
        /// </summary>
        /// <param name="rq">Contêm o IdAgendamentoInicial, IdAgendamentoFinal e o Status que serão utilizados para atualizar os status dos agendamentos</param>
        [Route("nps/atualizarrange")]
        [HttpPost]
        public RetornoModel AtualizarAgendamentoRange([FromBody]NpsAtualizarAgendamentoRange rq)
        {
            RetornoModel ret = new RetornoModel();
            ret.Mensagem = "OK";
            ret.Sucesso = true;

            if (rq == null)
            {
                ret.Mensagem = "Erro ao obter os dados para realizar o agendamento";
                ret.Sucesso = false;
                return ret;
            }

            if (rq.IdAgendamentoInicial < 0 || rq.IdAgendamentoFinal < 0) // Se IdAgendamentoInicial e IdAgendamentoFinal for menor que zero, então...
            {
                ret.Mensagem = "IdAgendamentoInicial/IdAgendamentoFinal não foi encontrado";
                ret.Sucesso = false;
                return ret;
            }

            if (rq.IdAgendamentoFinal > rq.IdAgendamentoInicial) // Testa se final não é maior que o inicial
            {
                ret.Mensagem = "IdAgendamentoFinal não pode ser maior que IdAgendamentoInicial";
                ret.Sucesso = false;
                return ret;
            }

            try
            {
                // Atualizar o status do agendamento
                using (var bo = new NetPromoterServiceBO())
                {
                    DateTime? DataEnvio = null;
                    if (rq.Status == enumStatusNPS.Sucesso) // Se status for de sucesso, atualizo a data que foi realizado o envio dos dados
                        DataEnvio = DateTime.Now;

                    bo.AtualizarAgendamentoRange(Convert.ToString(rq.IdAgendamentoInicial), Convert.ToString(rq.IdAgendamentoFinal), rq.Status.ValueAsString(), DataEnvio);
                }
            }
            catch (Exception ex)
            {
                ret.Mensagem = ex.Message;
                ret.Sucesso = false;
            }

            return ret;
        }

        /// <summary>
        /// Método da API que retorna uma lista contendo todos os agendamentos existentes para o status passado no parâmetro
        /// </summary>
        /// <param name="Status">Status do Agendamento</param>
        [Route("nps/listar")]
        [HttpGet]
        public RetornoModel<List<NpsModel>> ObterAgendamentosPorStatus(enumStatusNPS Status)
        {
            RetornoModel<List<NpsModel>> retorno = new RetornoModel<List<NpsModel>>();
            retorno.Mensagem = "OK";
            retorno.Sucesso = true;
            retorno.Retorno = new List<NpsModel>();

            try
            {
                using (var bo = new NetPromoterServiceBO())
                {
                    var registros = bo.ObterAgendamentosPorStatus(Status.ValueAsString()).ToList();
                    registros.ForEach(a =>
                    {
                        NpsModel nps = new NpsModel();
                        nps.ID = a.ID;
                        nps.Name = a.Name;
                        nps.Email = a.Email;
                        nps.Delay = a.Delay;
                        nps.Status = a.Status;
                        nps.Canal = a.Canal;
                        nps.DataInclusao = a.DataInclusao;
                        nps.DataEnvio = a.DataEnvio;

                        retorno.Retorno.Add(nps);
                    });
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
            }

            return retorno;
        }
    }
}