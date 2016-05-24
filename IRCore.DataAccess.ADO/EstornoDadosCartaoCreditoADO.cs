using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System;
using System.Linq;
using Dapper;
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class EstornoDadosCartaoCreditoADO : MasterADO<dbIngresso>
    {
        public EstornoDadosCartaoCreditoADO(MasterADOBase ado = null) : base(ado) { }

        public IQueryable<EstornoDadosCartaoCredito> Listar()
        {
            var queryStr = @"SELECT est.*, vbCancel.Senha ""SenhaCancelamento"", vbVenda.Senha ""SenhaCompra"" 
                FROM		EstornoDadosCartaoCredito est (NOLOCK) 
				INNER JOIN tVendaBilheteria vbCancel (NOLOCK)  on vbCancel.ID = est.VendaBilheteriaIDCancel
				INNER JOIN tVendaBilheteria vbVenda (NOLOCK)  on vbVenda.ID = est.VendaBilheteriaIDVenda
                WHERE est.PlanilhaGerada = 0 AND est.STATUS = 'P'";

            var query = conIngresso.Query<EstornoDadosCartaoCredito>(queryStr);

            return query.ToList().AsQueryable();
        }

        public IQueryable<EstornoDadosCartaoCredito> ListarEstornosManuais()
        {
            var queryStr = @"SELECT est.*, vbCancel.Senha ""SenhaCancelamento"", 
                vbVenda.Senha ""SenhaCompra"" , vbfp.ID AS VendaBilFormPagID,
                vbfp.NSUHost AS NSU, vbfp.NumeroAutorizacao
                FROM		EstornoDadosCartaoCredito est (NOLOCK)
				INNER JOIN tVendaBilheteria vbCancel (NOLOCK)  on vbCancel.ID = est.VendaBilheteriaIDCancel
				INNER JOIN tVendaBilheteria vbVenda (NOLOCK)  on vbVenda.ID = est.VendaBilheteriaIDVenda
				INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) on vbfp.VendaBilheteriaID = vbVenda.ID 
                WHERE est.PlanilhaGerada = 0 AND est.STATUS = 'W' AND (DATEDIFF(day,est.DataInsert,getdate()) > 4)";

            var query = conIngresso.Query<EstornoDadosCartaoCredito>(queryStr);

            return query.ToList().AsQueryable();
        }

        public EstornoDadosCartaoCredito Consultar(int estornoID)
        {
            var queryStr = @"SELECT est.*, vbCancel.Senha ""SenhaCancelamento"", 
                vbVenda.Senha ""SenhaCompra"" , vbfp.ID AS VendaBilFormPagID,
                vbfp.NSUHost AS NSU, vbfp.NumeroAutorizacao
                FROM		EstornoDadosCartaoCredito est (NOLOCK)
				INNER JOIN tVendaBilheteria vbCancel (NOLOCK)  on vbCancel.ID = est.VendaBilheteriaIDCancel
				INNER JOIN tVendaBilheteria vbVenda (NOLOCK)  on vbVenda.ID = est.VendaBilheteriaIDVenda
				INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) on vbfp.VendaBilheteriaID = vbVenda.ID
				WHERE est.ID = @ID";

            var query = conIngresso.Query<EstornoDadosCartaoCredito>(queryStr, new { id = estornoID });

            return query.ToList().FirstOrDefault();
        }

        private bool SetStatus(int EstornoID)
        {
            try
            {
                conIngresso.Query("UPDATE EstornoDadosCartaoCredito SET Status = 'P' WHERE ID = @ID", new
                {
                    ID = EstornoID,
                });
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;
            }
            return true;
        }

        public bool Atualiza(EstornoDadosCartaoCredito obj)
        {
            try
            {
                conIngresso.Query("UPDATE tVendaBilheteriaFormaPagamento SET NSUHost = @NSU, NumeroAutorizacao = @NrAutorizacao WHERE ID = @ID", new
                {
                    ID = obj.VendaBilFormPagID,
                    NSU = obj.NSU,
                    NrAutorizacao = obj.NumeroAutorizacao
                });

                SetStatus(obj.ID);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return false;
            }

            return true;
        }
    }
}
