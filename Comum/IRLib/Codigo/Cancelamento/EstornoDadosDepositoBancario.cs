using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public class EstornoDadosDepositoBancario : EstornoDadosDepositoBancario_B
    {
        internal string StringInserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.EnsureCapacity(800000);
                sql.Append("INSERT INTO EstornoDadosDepositoBancario(CancelDevolucaoPendenteID, VendaBilheteriaIDCancel, VendaBilheteriaIDVenda, DataDeposito, Banco, Agencia, Conta, Valor, Cliente, CPFCliente, NomeCorrentista, CPFCorrentista, CancelamentoPor, Status, MotivoLancamentoManual, ContaPoupanca, Email, DataInsert, Digito) ");
                sql.Append("VALUES (@000, @001, @002, GETDATE() + 4, '@004', '@005', '@006', @007, '@008', '@009', '@010', '@011', '@012', '@013', '@014', @015, '@016', GETDATE(),'@066'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@000", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@001", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@004", this.Banco.ValorBD);
                sql.Replace("@005", this.Agencia.ValorBD);
                sql.Replace("@006", this.Conta.ValorBD);
                sql.Replace("@066", this.Digito.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.Cliente.ValorBD);
                sql.Replace("@009", this.CPFCliente.ValorBD);
                sql.Replace("@010", this.NomeCorrentista.ValorBD);
                sql.Replace("@011", this.CPFCorrentista.ValorBD);
                sql.Replace("@012", this.CancelamentoPor.ValorBD);
                sql.Replace("@013", this.Status.ValorBD);
                sql.Replace("@014", this.MotivoLancamentoManual.ValorBD);
                sql.Replace("@015", this.ContaPoupanca.Valor? "1":"0");
                sql.Replace("@016", this.Email.ValorBD);
                return sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
