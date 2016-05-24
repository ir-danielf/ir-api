using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class ValeIngressoADO : MasterADO<dbIngresso>
    {
        public ValeIngressoADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Método que valida um vale ingresso pelo código
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public  tValeIngresso ValidarCodigo(string codigo)
        {
            var query = (from item in dbIngresso.tValeIngresso
                                          join tipo in dbIngresso.tValeIngressoTipo on item.ValeIngressoTipoID equals tipo.ID
                                          where item.CodigoTroca == codigo && item.Status == "D"
                                          select new ValeIngressoModelQuery()
                                          {
                                              valeIngresso = item,
                                              valeIngressoTipo = tipo
                                          }).AsNoTracking();
            var valeIngressoModel = query.FirstOrDefault();
            if(valeIngressoModel != null)
                return valeIngressoModel.toValeIngresso();
            return null;
        }

        /// <summary>
        /// Consultar um vale ingresso pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tValeIngresso Consultar(int id)
        {
            var valeIngressoModel = (from item in dbIngresso.tValeIngresso
                                     join tipo in dbIngresso.tValeIngressoTipo on item.ValeIngressoTipoID equals tipo.ID
                                     where item.ID == id
                                     select new ValeIngressoModelQuery()
                                     {
                                         valeIngresso = item,
                                         valeIngressoTipo = tipo
                                     }).AsNoTracking().FirstOrDefault();

            if (valeIngressoModel != null)
                return valeIngressoModel.toValeIngresso();

            return null;
        }

        
        /// <summary>
        /// Consultar um vale ingresso pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tValeIngresso> Listar(List<int> ids)
        {
            string queryString = @"
                                SELECT
	                                Val.ID,Val.ValeIngressoTipoID,Val.CodigoTroca,Val.DataCriacao,Val.DataExpiracao,Val.Status,Val.VendaBilheteriaID,Val.ClienteID,Val.SessionID,Val.TimeStampReserva,Val.LojaID,Val.UsuarioID,Val.CanalID,Val.ClienteNome,Val.CodigoBarra,
	                                Tipo.ID,Tipo.Nome,Tipo.Valor,Tipo.ValidadeDiasImpressao,Tipo.ValidadeData,Tipo.ClienteTipo,Tipo.ProcedimentoTroca,Tipo.SaudacaoPadrao,Tipo.SaudacaoNominal,Tipo.QuantidadeLimitada,Tipo.EmpresaID,Tipo.CodigoTrocaFixo,Tipo.Acumulativo,Tipo.VersaoImagem,Tipo.VersaoImagemInternet,Tipo.ReleaseInternet,Tipo.PublicarInternet,Tipo.UltimoCodigoImpresso,Tipo.TrocaIngresso,Tipo.TrocaConveniencia,Tipo.TrocaEntrega,Tipo.ValorTipo,Tipo.ValorPagamento
                                FROM 
	                                tValeIngresso AS Val (NOLOCK) JOIN
	                                tValeIngressoTipo AS Tipo (NOLOCK) ON Val.ValeIngressoTipoID = Tipo.ID
                                WHERE
	                                Val.ID IN (@IDs)";
            queryString = queryString.Replace("@IDs", String.Join(",", ids));
            var query = conIngresso.Query<tValeIngresso, tValeIngressoTipo, tValeIngresso>(queryString, addResultListar);
            return query.ToList();
        }

        private tValeIngresso addResultListar(tValeIngresso vale, tValeIngressoTipo tipo)
        {
            vale.ValeIngressoTipo = tipo;
            return vale;
        }

        /// <summary>
        /// Consultar um vale ingresso pelo codigo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tValeIngresso Consultar(string codigo)
        {
            var valeIngressoModel = (from item in dbIngresso.tValeIngresso
                                     join tipo in dbIngresso.tValeIngressoTipo on item.ValeIngressoTipoID equals tipo.ID
                                     where item.CodigoTroca == codigo
                                     select new ValeIngressoModelQuery()
                                     {
                                         valeIngresso = item,
                                         valeIngressoTipo = tipo
                                     }).AsNoTracking().FirstOrDefault();

            if (valeIngressoModel != null)
                return valeIngressoModel.toValeIngresso();

            return null;
        }
    }
}
