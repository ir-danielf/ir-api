using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public class EstornoDadosCartaoCredito : EstornoDadosCartaoCredito_B
    {
        internal string StringInserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO EstornoDadosCartaoCredito(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, Bandeira, Cartao, Valor, Cliente, CPFCliente, CancelamentoPor, Email, Status, DataInsert) ");
                sql.Append("VALUES (@000, @001, @002 , '@003' , '@004', @005, '@006', '@007', '@008', '@009', '@010' , GETDATE()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@003", this.Bandeira.ValorBD);
                sql.Replace("@004", this.Cartao.ValorBD);
                sql.Replace("@005", this.Valor.ValorBD);
                sql.Replace("@006", this.Cliente.ValorBD);
                sql.Replace("@007", this.CPFCliente.ValorBD);
                sql.Replace("@008", this.CancelamentoPor.ValorBD);
                sql.Replace("@009", this.Email.ValorBD);
                sql.Replace("@010", this.Status.ValorBD);


                return sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
