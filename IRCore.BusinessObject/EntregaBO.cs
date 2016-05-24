using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class EntregaBO : MasterBO<EntregaADO>
    {
        public EntregaBO(MasterADOBase ado) : base(ado) { }
        public EntregaBO() : base(null) { }

        /// <summary>
        /// Método que lista as entregas disponiveis para umaa lista de carrinho
        /// </summary>
        /// <param name="listCarrinho"></param>
        /// <returns></returns>
        public List<tEntregaControle> ListarCarrinho(List<Carrinho> listCarrinho)
        {
            List<int> eventosIds = listCarrinho.Where(x => x.EventoID != null).Select(x => x.EventoID.Value).Distinct().ToList();
            List<tEntregaControle> entregaControles = ListarInEventos(eventosIds);
            DateTime proximaApresentacao = listCarrinho.OrderBy(x => x.ApresentacaoDataHoraAsDateTime).Select(x => x.ApresentacaoDataHoraAsDateTime).FirstOrDefault().Date;
            entregaControles = FiltrarDatasViaveis(entregaControles, proximaApresentacao);
            return entregaControles;
        }

        /// <summary>
        /// Método que lista as entregas disponiveis para um objeto compra
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        public CompraModel Carregar(CompraModel compra)
        {
            LogUtil.Debug(string.Format("##EntregaBO.Carregar## SESSION {0}", compra.SessionID));

            compra.EntregaControles = ListarCarrinho(compra.CarrinhoItens);
            if (compra.Login != null)
                compra.Login = CarregarEnderecos(compra.EntregaControles, compra.Login);
            compra.EntregaComEndereco = (compra.EntregaControles.Any(t => t.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaNormal || t.Entrega.TipoAsEnum == enumEntregaTipo.entregaEmCasaAgendada));

            LogUtil.Debug(string.Format("##EntregaBO.Carregar.SUCCESS## SESSION {0}, ENTREGAS {1}, ENTR_COM_END '{2}'", compra.SessionID, compra.EntregaControles.Count, compra.EntregaComEndereco));
            return compra;
        }

        /// <summary>
        /// Método que carrega todas entrega controles com entrega assoliada para uma lista de id de eventos
        /// </summary>
        /// <param name="eventosIds"></param>
        /// <returns></returns>
        public List<tEntregaControle> ListarInEventos(List<int> eventosIds)
        {
            var entregaControles = ado.ListarInEventos(eventosIds);
            return entregaControles;
        }

        /// <summary>
        /// Filtrar as entregas controles de acordo com a viabilidade da sua data
        /// </summary>
        /// <param name="entregaControles"></param>
        /// <param name="proximaApresentacao"></param>
        /// <returns></returns>
        public List<tEntregaControle> FiltrarDatasViaveis(List<tEntregaControle> entregaControles, DateTime proximaApresentacao)
        {

            //Consulta a diferença em dias sem contar finais de semana
            int diasUteis = Convert.ToInt32(DateTime.Today.DiffWorkDays(proximaApresentacao).TotalDays);
            //Lista de int que representa os dias de semana
            List<int> dias = new List<int>() { 1, 2, 3, 4, 5 };
            //Método que retorna o numero de feriados entre duas datas
            int feriados = ado.CountFeriados(DateTime.Today, proximaApresentacao, dias);
            diasUteis -= feriados;
            if (diasUteis < 0)
                diasUteis = 0;
            entregaControles.RemoveAll(x => x.Entrega.PrazoEntrega > diasUteis);
            return entregaControles;
        }

        /// <summary>
        /// </summary>
        /// <param name="entregasControles"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public Login CarregarEnderecos(List<tEntregaControle> entregaControles, Login login)
        {
            ClienteBO clienteBO = new ClienteBO(login.SiteID, ado);
            if (login.Cliente.EnderecoList == null)
            {
                login.Cliente = clienteBO.CarregarEnderecos(login.Cliente);
            }
            else
            {
                login.Cliente = clienteBO.CarregarEnderecoCadastro(login.Cliente);
            }
            for (int i = 0; i < login.Cliente.EnderecoList.Count; i++)
            {
                login.Cliente.EnderecoList[i] = VerificarEndereco(entregaControles, login.Cliente.EnderecoList[i]);
            }
            return login;
        }

        public tClienteEndereco VerificarEndereco(List<tEntregaControle> entregaControles, tClienteEndereco endereco)
        {

            if (ado.VerificarCEPBlackList(endereco.CEP) == null)
            {
                endereco.EntregaArea = ado.ConsultarEntregaArea(endereco.CEP);
                if (endereco.EntregaArea.Count > 0)
                    endereco.EntregaControles = entregaControles.Where(x => endereco.EntregaArea.Any(y => y.ID == x.EntregaAreaID)).ToList();
            }
            else
            {
                endereco.EntregaControles = new List<tEntregaControle>();
            }
            return endereco;
        }
        public List<tEntregaControle> FiltrarEntregasPorCEP(List<tEntregaControle> entregaControles, string CEP)
        {
            List<tEntregaControle> retorno = null;
            if (ado.VerificarCEPBlackList(CEP) == null)
            {
                var entregaArea = ado.ConsultarEntregaArea(CEP);
                if (entregaArea.Count > 0)
                    retorno = entregaControles.Where(x => entregaArea.Any(y => y.ID == x.EntregaAreaID)).ToList();
            }
            else
            {
                retorno = new List<tEntregaControle>();
            }
            return retorno;
        }

    }
}
