using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public class CancelDevolucaoPendenteIngresso : CancelDevolucaoPendenteIngresso_B
    {
        internal string StringInserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql = new StringBuilder();
                sql.EnsureCapacity(800000);
                sql.Append("INSERT INTO tCancelDevolucaoPendenteIngresso(CancelDevolucaoPendenteID, IngressoID, DataInsert)");
                sql.Append("VALUES (@001, @002, getdate());SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.CancelDevolucaoPendenteID.ValorBD);
                sql.Replace("@002", this.IngressoID.ValorBD);
                return sql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
