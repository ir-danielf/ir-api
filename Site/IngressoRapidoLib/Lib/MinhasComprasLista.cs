using CTLib;
using System;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class MinhasComprasLista : List<MinhasCompras>
    {
        private BD oBD = new BD();

        private MinhasCompras oMinhasCompras { get; set; }

        public MinhasComprasLista()
        {
            this.Clear();
        }

        public MinhasComprasLista GetByClientId(int clientID)
        {
            if (clientID == 0)
                throw new Exception("Cliente Inválido.");

            string strSql = string.Format(@"SELECT CN.Nome AS Canal,VB.DataVenda,VB.Senha,CAST(VB.ValorTotal AS DECIMAL(10,3))
                    As ValorTotal, VB.Status, VB.ID AS VendaBilheteriaId, 
                    IsNull(e.PermitirImpressaoInternet, 'F') AS PermitirImpressaoInternet, VB.PagamentoProcessado, VB.VendaCancelada
                    FROM tVendaBilheteria (NOLOCK) VB
                    INNER JOIN tCaixa (NOLOCK)CX ON VB.CaixaID = CX.ID
                    INNER JOIN tLoja L (NOLOCK)ON CX.LojaID = L.ID
                    INNER JOIN tCanal (NOLOCK)CN ON L.CanalID = CN.ID
                    LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = VB.EntregaControleID
                    LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID
                    WHERE VB.ClientEID = {0} AND VB.DataVenda >= '{1}' AND VB.Status = '{2}' AND VB.VendaCancelada = 'F'
                    ORDER BY VB.DataVenda DESC", clientID, DateTime.Now.AddMonths(-6).ToString("yyyyMMdd"), IRLib.VendaBilheteria.PAGO);
            try
            {
                oBD.Consulta(strSql);

                while (oBD.Consulta().Read())
                {
                    this.Add(new MinhasCompras()
                    {                        
                        Canal = oBD.LerString("Canal"),
                        Data = oBD.LerStringFormatoData("DataVenda"),
                        Senha = oBD.LerString("Senha"),
                        ValorTotal = oBD.LerDecimal("ValorTotal"),
                        Status = oBD.LerString("Status"),
                        VendaBilheteriaId = oBD.LerInt("VendaBilheteriaId"),
                        ClientId = clientID,
                        ImprimirInternet = (oBD.LerBoolean("PermitirImpressaoInternet") && !oBD.LerBoolean("VendaCancelada") && oBD.LerString("Status") == IRLib.VendaBilheteria.PAGO && oBD.LerBoolean("PagamentoProcessado")),
                    });
                }

                return this;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                oBD.Fechar();
            }
        }
    }
}
