using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using IRLib;

namespace IRCore.BusinessObject
{
    public class IngressoBO : MasterBO<IngressoADO>
    {
        public IngressoBO(MasterADOBase ado) : base(ado) { }
        public IngressoBO() : base(null) { }

        /// <summary>
        /// Consulta a quantidade de ingressos por evento e apresentação.
        /// </summary>
        /// <param name="eventoId">Código do evento.</param>
        /// <param name="clienteId">Código do cliente.</param>
        /// <param name="apresentacaoId">Código da aprensetação.</param>
        /// <exception cref="InvalidOperationException">Operação inválida se o número de ingressos não estiver dentro da regra.</exception>
        public void ConsultarTotalIngressoPorClienteApresentacao(int eventoId, int clienteId, int apresentacaoId, string sessionId, int quantidadeTotalDeIngressosPorEvento = 16, int quantidadeTotalDeIngressosPorApresentacao = 4, int quantidadeTotalDeApresentacoesPorEvento = 4, string textoTermoValidacao = "ingressos")
        {
            ado.ConsultarTotalIngressoPorClienteApresentacao(eventoId, clienteId, apresentacaoId, sessionId, quantidadeTotalDeIngressosPorEvento, quantidadeTotalDeIngressosPorApresentacao, quantidadeTotalDeApresentacoesPorEvento, textoTermoValidacao);
        }

        public void Salvar(tIngresso ingresso, bool updateDataBase = true)
        {
            ado.Salvar(ingresso, updateDataBase);
        }

        public void Salvar()
        {
            ado.Salvar();
        }

        public tIngresso Consultar(int id)
        {
            return ado.Consultar(id);
        }

        public List<tIngresso> ListarParceiroStatus(int parceiroId, int setorId, int apresentacaoId, enumIngressoStatus status, int qtd)
        {
            return ado.ListarParceiroStatus(parceiroId, setorId, apresentacaoId, status, qtd);
        }

        public bool LiberarReserva(List<tIngresso> listaReserva)
        {
            CotaItemControle cotaItemControle = new CotaItemControle();

            foreach (var ingresso in listaReserva)
            {
                if (ingresso.ApresentacaoID != null && ingresso.CotaItemID != null)
                {
                    var apresentacaoId = ingresso.ApresentacaoID ?? 0;
                    var cotaItemId = ingresso.CotaItemID ?? 0;
                    cotaItemControle.DecrementarControladorApresentacao(apresentacaoId, cotaItemId);
                }

                ingresso.UsuarioID = null;
                ingresso.SessionID = null;
                ingresso.ClienteID = null;
                ingresso.StatusAsEnum = (ingresso.ParceiroMidiaID != null ? enumIngressoStatus.bloqueado : enumIngressoStatus.disponivel);
                ingresso.CotaItemID = null;
            }

            return ado.RemoverReserva(listaReserva);
        }

        /// <summary>
        /// Lista ingressos pela apresentação, setor e status
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<tIngresso> Listar(int apresentacaoID, int setorID, string busca, enumIngressoStatus status, int notParceiroMidiaID = 0, int quantidade = 0)
        {
            return ado.ListarParceiroMidia(apresentacaoID, setorID, busca, status, notParceiroMidiaID, quantidade);
        }


        public List<BloqueioResult> ListarIngressoParceiroMediaApresentacao(int parceiroMediaID)
        {
            return ado.ListarIngressoParceiroMediaApresentacao(parceiroMediaID);
        }
        

        /// <summary>
        /// Obtem total de ingressos
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <param name="busca"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int ObterTotalIngressos(int apresentacaoID, int setorID, string busca, enumIngressoStatus status, int quantidade)
        {
            return ado.ObterTotalIngressos(apresentacaoID, setorID, busca, status, quantidade);
        }

        public List<tIngresso> Listar(List<int> ingressosIds)
        {
            return ado.Listar(ingressosIds);
        }

        public List<tIngresso> RelacionarBloqueioParceiroMidia(List<int> ingressosIds, int parceiroMidiaId, int usuarioID)
        {
            List<tIngresso> retorno = new List<tIngresso>();

            bool precoIngressoParceiroMidiaNaoVerificado = true;
            foreach (int item in ingressosIds)
            {
                tIngresso ingresso = Consultar(item);
                if (ingresso.StatusAsEnum == enumIngressoStatus.bloqueado)
                {
                    //OBS: Esse bloco é executado apenas se o preço do ingresso ainda não tiver sido verificado, pois todos os ingressos possuem o mesmo setor e apresentação
                    if (precoIngressoParceiroMidiaNaoVerificado)
                    {
                        PrecoBO precoBO = new PrecoBO(ado);
                        ParceiroMidiaBO parceiroBO = new ParceiroMidiaBO(ado);
                        ParceiroMidia parceiro = parceiroBO.Consultar(parceiroMidiaId);
                        precoBO.CadastrarParaIngressoParceiroMidia(ingresso, usuarioID, parceiro);
                        precoIngressoParceiroMidiaNaoVerificado = false;
                    }
                    ingresso.ParceiroMidiaID = parceiroMidiaId;
                    Salvar(ingresso, false);
                    retorno.Add(ingresso);
                }
            }
            Salvar();

            return retorno;
        }
        public void DesassociarParceiroMidia(int parceiroMidiaId, string acao, int usuarioID, string ingressosIds)
        {
            ado.DesassociarParceiroMidia(parceiroMidiaId, acao, usuarioID, ingressosIds);
        }

        public void RelacionarBloqueioApresentacaoParceiroMidia(int parceiroMidiaId, int usuarioID, string ingressosIds)
        {
            ado.RelacionarBloqueioApresentacaoParceiroMidia(parceiroMidiaId, usuarioID, ingressosIds);
        }

        /// <summary>
        /// Método que lista os ingressos de um evento pela busca
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        public List<tIngresso> ListarBloqueados(int eventoID, int parceiroMidiaID)
        {
            return ado.ListarBloqueados(eventoID, parceiroMidiaID);
        }


        public bool BloquearIngressoOSESP(int IngressoID, int UsuarioID, int PluID, int PluUtilizado, int BloqueioPadrao)
        {
            return ado.BloquearIngressoOSESP(IngressoID, UsuarioID, PluID, PluUtilizado, BloqueioPadrao);
        }

        public bool DesbloquearIngressoOSESP(int IngressoID, int UsuarioID, int PluID, int PluUtilizadoID, int BloqueioPadrao)
        {
            return ado.DesbloquearIngressoOSESP(IngressoID, UsuarioID, PluID, PluUtilizadoID, BloqueioPadrao);
        }
        public tIngresso BuscaRetornoOsesp(int IngressoID)
        {
            return ado.BuscaRetornoOsesp(IngressoID);
        }

        public Dictionary<int, string> BuscarCodigosBarras(int[] ingressosId)
        {
            return ado.BuscarCodigosBarras(ingressosId);
        }

        public void AtualizarCodigoBarraIngressos(List<Carrinho> carrinhoItens, int usuarioID, int caixaID, int canalID, int lojaID)
        {
            var ingressosId = carrinhoItens.Select(x => (int)x.IngressoID).ToArray();

            var usuarioBO = new UsuarioBO(ado);
            var empresaID = usuarioBO.Consultar(usuarioID).EmpresaID;
            var bilheteria = new Bilheteria();
            bilheteria.RegistrarImpressao(ingressosId, usuarioID, (int)empresaID, caixaID, canalID, lojaID, false, 0, null, 0, false);
            var codigosBarra = BuscarCodigosBarras(ingressosId);
            foreach (var item in carrinhoItens)
            {
                item.CodigoBarra = codigosBarra[(int)item.IngressoID];
            }
        }
        
        public string BuscaNomeBloqueio(int BloqueioID)
        {
            return ado.BuscaNomeBloqueio(BloqueioID);
        }
    }
}
