using System.Text;

namespace IRLib.Paralela
{
    public class CupomDescontoLog : CupomDescontoLog_B
    {
        /// <summary>
        /// Inserir novo(a) CupomDescontoLog
        /// </summary>
        /// <returns></returns>	
        public string StringInserir()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tCupomDescontoLog(CupomID, ApresentacaoID, VendaBilheteriaID) ");
            sql.Append("VALUES (@001,@002,@003)");

            sql.Replace("@001", this.CupomID.ValorBD);
            sql.Replace("@002", this.ApresentacaoID.ValorBD);
            sql.Replace("@003", this.VendaBilheteriaID.ValorBD);
            return sql.ToString();
        }
    }
}
