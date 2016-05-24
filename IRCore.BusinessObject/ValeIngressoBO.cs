using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class ValeIngressoBO : MasterBO<ValeIngressoADO>
    {
        public ValeIngressoBO(MasterADOBase ado) : base(ado) { }
        
        public ValeIngressoBO() : base(null) { }

        /// <summary>
        /// Método que consulta a validade de um Ingresso
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public tValeIngresso ValidarCodigo(string codigo)
        {
            return ado.ValidarCodigo(codigo);
        }

        /// <summary>
        /// Consulta um valeIngresso
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public tValeIngresso Consultar(string codigo)
        {
            return ado.Consultar(codigo);
        }

        /// <summary>
        /// Consulta um valeIngresso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tValeIngresso Consultar(int id)
        {
            return ado.Consultar(id);
        }

        /// <summary>
        /// Retorna a Lista de Vales Ingressos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tValeIngresso> Listar(List<int> ids)
        {
            LogUtil.Debug(string.Format("##ValeIngressoBO.Listar## IDS {0}", String.Join(",", ids)));

            return ado.Listar(ids);
        }

        /// <summary>
        /// Método que Verifica se pode ser inserido um valeIngresso em uma compra.
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="valeIngresso"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> VerificarCompra(CompraModel compra, tValeIngresso valeIngresso = null)
        {
            RetornoModel<CompraModel> retorno = new RetornoModel<CompraModel>();
            retorno.Sucesso = true;
            retorno.Mensagem = "Vale Ingresso pode ser inserido.";
            retorno.Retorno = compra;
            //
            if (compra.CarrinhoItens != null && compra.CarrinhoItens.Count(x => x.CotaItemObject != null && x.CotaItemObject.ValidaBinAsBool) != 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "O carrinho possui um item com cota bin, por isso não podem ser adicionados VIRs.";
            }
            //Teste de valor
            else if (compra.Total == null || compra.Total.ValorTotal == 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "O carrinho está com valor total 0, por isso não podem ser adicionados VIRs.";
            }
            //teste que veirifica se existe um vale não acumulativo inserido
            else if (compra.ValeIngressos != null && compra.ValeIngressos.Count(x => !(x.ValeIngressoTipo.AcumulativoAsBool)) > 0)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "A compra possui um vale ingresso não acumulativo, por isso não podem se adicionados VIRs.";
            }

            else if (valeIngresso != null)
            {
                //verifica se o VIR é não acumulativo e se já tem VIRs na compra
                if (!(valeIngresso.ValeIngressoTipo.AcumulativoAsBool) && compra.ValeIngressos.Count > 0)
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "A compra possui um ou mais vale(s) ingresso(s) e o vale ingresso que está sendo inserido não é acumulativo, por isso não pode ser adicionado.";
                }
                //Faz uma cópia do objeto compra adicionando o vale ingresso passado no parametro. Se o VIR adicionado não afetar o valor da compra não deixa inserir
                else
                {
                    CarrinhoBO carrinhoBO = new CarrinhoBO(ado);
                    CompraModel compraAux = new CompraModel();
                    compraAux.CopyFrom(compra);
                    compraAux.ValeIngressos = compra.ValeIngressos.ToList();
                    compraAux.ValeIngressos.Add(valeIngresso);
                    carrinhoBO.CalcularValores(compraAux);
                    if (compra.Total.ValorTotal == compraAux.Total.ValorTotal)
                    {
                        retorno.Sucesso = false;
                        retorno.Mensagem = "O VIR que está sendo adicionado não altera o valor total da compra, por isso ele não pode ser adicionado.";
                    }

                }
            }

            return retorno;
        }

        /// <summary>
        /// Método que insere um vale ingresso em um objeto compra.
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="valeIngresso"></param>
        /// <returns></returns>
        public RetornoModel<CompraModel> InserirValeIngresso(CompraModel compra, tValeIngresso valeIngresso)
        {
            if (compra.ValeIngressos == null)
                compra.ValeIngressos = new List<tValeIngresso>();

            RetornoModel<CompraModel> retornoModel = VerificarCompra(compra, valeIngresso);
            if (retornoModel.Sucesso)
            {
                CarrinhoBO carrinhoBO = new CarrinhoBO(ado);
                retornoModel.Retorno.ValeIngressos.Add(valeIngresso);
                retornoModel.Retorno = carrinhoBO.CalcularValores(retornoModel.Retorno);
            }
            return retornoModel;
        }

        /// <summary>
        /// Método que remove um vale ingresso de uma compra e retorna a compra com valor recalculado.
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="valeIngresso"></param>
        /// <returns></returns>
        public CompraModel RemoverValeIngresso(CompraModel compra, tValeIngresso valeIngresso)
        {
            CarrinhoBO carrinhoBO = new CarrinhoBO(ado);
            if (compra.ValeIngressos != null && valeIngresso != null)
            {
                compra.ValeIngressos.RemoveAll(x => x.ID == valeIngresso.ID);
                compra = carrinhoBO.CalcularValores(compra);
            }
            return compra;
        }
    }
}
