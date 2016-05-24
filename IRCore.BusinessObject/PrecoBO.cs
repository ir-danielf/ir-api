using CTLib;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;

namespace IRCore.BusinessObject
{
    public class PrecoBO : MasterBO<PrecoADO>
    {
        public PrecoBO(MasterADOBase ado) : base(ado) { }

        public PrecoBO() : base(null) { }

        public List<Preco> Listar(int setorId, int apresentacaoId)
        {
            return ado.Listar(setorId, apresentacaoId);
        }

        public PrecoParceiroMidia ConsultarParceiro(int setorId, int apresentacaoId, int parceiroMidiaId)
        {
            return ado.ConsultarParceiro(setorId, apresentacaoId, parceiroMidiaId);
        }

        /// <summary>
        /// Método que cadastra um preço de ingresso para um parceiro de midia, se ainda não existir
        /// </summary>
        /// <param name="ingresso"></param>
        public RetornoModel CadastrarParaIngressoParceiroMidia(tIngresso ingresso, int usuarioID,ParceiroMidia parceiroMidia)
        {

            int lojaId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.lojaIdSistema);
            LojaADO lojaADO = new LojaADO(ado);
            tLoja loja = lojaADO.Consultar(lojaId);

            if (parceiroMidia != null && ingresso.ApresentacaoID != null && ingresso.SetorID != null)
            {
                PrecoParceiroMidia precoParceiro = ConsultarParceiro(ingresso.SetorID.Value, ingresso.ApresentacaoID.Value, parceiroMidia.ID);
                if(precoParceiro == null)
                {

                    tPreco preco = ado.ConsultarPreco(ingresso.SetorID.Value, ingresso.ApresentacaoID.Value, parceiroMidia.ID);
                    tCanalPreco canalPreco = null;

                    if(preco == null)
                    {
                        preco = new tPreco();
                        preco.Nome = parceiroMidia.Nome;
                        preco.Valor = 0;
                        preco.ApresentacaoSetorID = ingresso.ApresentacaoSetorID;
                        preco.ParceiroMidiaID = parceiroMidia.ID;
                        preco.IRVende = "F";
                        preco.CorID = 1;
                        Salvar(preco, usuarioID);

                        try
                        {
                            IRLib.CodigoBarra codigoBarra = new IRLib.CodigoBarra();
                            codigoBarra.Inserir(ingresso.EventoID.Value, ingresso.ApresentacaoID.Value,ingresso.SetorID.Value,preco.ID,new BD());
                        }
                        catch(Exception ex)
                        {
                            LogUtil.Error(ex);
                            Remover(preco, usuarioID);
                            return new RetornoModel() { Sucesso = false, Mensagem = "Erro para gerar o codigo de barras do preco" };
                        }
                    }
                    else
                    {
                        canalPreco = ado.ConsultarCanal(preco.ID, loja.CanalID.Value);
                    }

                    if ((preco != null) && (canalPreco == null))
                    {
                        
                        canalPreco = new tCanalPreco();
                        canalPreco.DataInicio = "";
                        canalPreco.DataFim = "";
                        canalPreco.PrecoID = preco.ID;
                        canalPreco.Quantidade = 0;
                        canalPreco.CanalID = loja.CanalID.Value;
                        Salvar(canalPreco, usuarioID);
                    }
                }
            }
            return new RetornoModel() { Sucesso = true };

        }

        public void Salvar(tPreco preco, int usuarioID)
        {
            ado.Salvar(preco, usuarioID);
        }

        public void Salvar(tCanalPreco canalPreco, int usuarioID)
        {
            ado.Salvar(canalPreco, usuarioID);
        }

        public void Remover(tPreco preco,int usuarioID)
        {
            ado.Remover(preco, usuarioID);
        }

        internal Preco ConsultarMaiorMenorPorApresentacao(int apresentacaoID, bool maior = true)
        {
            return ado.ConsultarMaiorMenorPorApresentacao(apresentacaoID, maior);
        }
    }
}
