using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public class EstornoDadosDinheiro : EstornoDadosDinheiro_B
    {
        internal string StringInserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.EnsureCapacity(800000);
                sql.Append("INSERT INTO EstornoDadosDinheiro(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, Valor, Cliente, CancelamentoPor, Email, DataInsert) ");
                sql.Append("VALUES (@000, @001, @002, @003, '@004', '@005', '@006', GETDATE()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Cliente.ValorBD);
                sql.Replace("@005", this.CancelamentoPor.ValorBD);
                sql.Replace("@006", this.Email.ValorBD);

                return sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
