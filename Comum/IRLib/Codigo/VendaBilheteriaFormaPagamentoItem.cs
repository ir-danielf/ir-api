/**************************************************
* Arquivo: VendaBilheteriaFormaPagamentoItem.cs
* Gerado: 10/09/2008
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Data;
using System.Text;
namespace IRLib
{

    public class VendaBilheteriaFormaPagamentoItem : VendaBilheteriaFormaPagamentoItem_B
    {

        public VendaBilheteriaFormaPagamentoItem() { }

        public VendaBilheteriaFormaPagamentoItem(int usuarioIDLogado) : base() { }

        /// <summary>
        /// ATENÇÃO: ESSA ROTINA NÂO FECHA A CONEXÃO! Quem cuida do fechamento e da finalização da transação é o Caixa.cs da IRLib.
        /// </summary>
        /// <param name="caixaID"></param>
        /// <param name="empresaID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal bool Inserir(int caixaID, int empresaID, BD db)
        {
            try
            {
                StringBuilder sqlInserir = new StringBuilder();

                DataTable tabela = new DataTable();
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                tabela.Columns.Add("DataDeposito", typeof(DateTime));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Dias", typeof(int));
                tabela.Columns.Add("Parcelas", typeof(int));

                string sql = @"SELECT fp.ID AS VendaBilheteriaFormaPagamentoID, fp.DataDeposito, fp.Valor, fp.Dias, tFormaPagamento.Parcelas, vb.Status 
                            FROM tVendaBilheteria AS vb (NOLOCK)
                            INNER JOIN tVendaBilheteriaFormaPagamento AS fp (NOLOCK) ON fp.VendaBilheteriaID = vb.ID
                            INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = fp.FormaPagamentoID
                            WHERE CaixaID = " + caixaID;

                db.Consulta(sql);
                int dias = 0;
                while (db.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    dias = db.LerInt("Dias");
                    linha["VendaBilheteriaFormaPagamentoID"] = db.LerInt("VendaBilheteriaFormaPagamentoID");
                    linha["DataDeposito"] = Convert.ToDateTime(db.LerDateTime("DataDeposito")).AddDays(dias);
                    linha["Valor"] = db.LerDecimal("Valor");

                    // Em caso de cancelamento, o valor deverá ser negativo
                    if (db.LerString("Status") == VendaBilheteria.CANCELADO)
                        linha["Valor"] = Convert.ToDecimal(linha["Valor"]) * -1;

                    linha["Dias"] = dias;
                    linha["Parcelas"] = db.LerInt("Parcelas");
                    tabela.Rows.Add(linha);
                }
                foreach (DataRow linha in tabela.Rows)
                {
                    if ((int)linha["Parcelas"] > 1)
                    {
                        for (int i = 1; i <= (int)linha["Parcelas"]; i++)
                        {
                            DateTime dataDeposito = Convert.ToDateTime(linha["DataDeposito"]);
                            dataDeposito = dataDeposito.AddDays(30 * (i-1));
                            sqlInserir.Append(" INSERT INTO tVendaBilheteriaFormaPagamentoItem (VendaBilheteriaFormaPagamentoID,Parcela,DataDeposito,ValorBruto,EmpresaID)");
                            sqlInserir.Append(" VALUES(" + linha["VendaBilheteriaFormaPagamentoID"] + "," + i + ",'" + dataDeposito.ToString("yyyyMMddHHmmss") + "'," + ((decimal)linha["Valor"] / (int)linha["Parcelas"]).ToString().Replace(',', '.')+", "+empresaID+ ")");
                        }
                    }
                    else
                    {
                        DateTime dataDeposito = Convert.ToDateTime(linha["DataDeposito"]);
                        sqlInserir.Append(" INSERT INTO tVendaBilheteriaFormaPagamentoItem (VendaBilheteriaFormaPagamentoID,Parcela,DataDeposito,ValorBruto,EmpresaID)");
                        sqlInserir.Append(" VALUES(" + linha["VendaBilheteriaFormaPagamentoID"] + ",0,'" + dataDeposito.ToString("yyyyMMddHHmmss") + "'," + ((decimal)linha["Valor"]).ToString().Replace(',', '.') + ", " + empresaID +")");
                    }

                }
                bool retorno= false;
                db.Consulta().Close();
                if (tabela.Rows.Count > 0)
                    retorno = db.Executar(sqlInserir) == tabela.Rows.Count ? true : false;

                return retorno;
                
            }
            catch
            {
                db.DesfazerTransacao();
                throw;
            }



        }


		public bool InserirNovo(BD b)
		{
			bool ok = true;
			if (this.Parcela.Valor > 1)
			{
				for (int i = 1; i <= this.Parcela.Valor; i++)
				{
					DateTime dataDeposito = this.DataDeposito.Valor;

					if (i > 1)
						dataDeposito = dataDeposito.AddDays(30 * i);
					ok &= bd.Executar(" INSERT INTO tVendaBilheteriaFormaPagamentoItem (VendaBilheteriaFormaPagamentoID,Parcela,DataDeposito,ValorBruto,EmpresaID) "+
					 " VALUES(" + this.VendaBilheteriaFormaPagamentoID.Valor + "," + i + ",'" + dataDeposito.ToString("yyyyMMddHHmmss") + "'," + (this.ValorBruto.Valor / this.Parcela.Valor).ToString().Replace(',', '.') + ", " + this.EmpresaID.Valor + ")") == 1;
				}
			}
			else
			{
				DateTime dataDeposito = this.DataDeposito.Valor;
				ok &= bd.Executar(" INSERT INTO tVendaBilheteriaFormaPagamentoItem (VendaBilheteriaFormaPagamentoID,Parcela,DataDeposito,ValorBruto,EmpresaID) "+
					 " VALUES(" + this.VendaBilheteriaFormaPagamentoID.Valor + ",0, '" + dataDeposito.ToString("yyyyMMddHHmmss") + "'," + this.ValorBruto.Valor.ToString().Replace(',', '.') + ", " + this.EmpresaID.Valor + ")") == 1;
			}
			if (!ok)
				ok = false;
			return ok;
		}

        [Obsolete("Do not use! ", true)]
		public void AjustaVendaBilheteriaFormaPagamento()
		{
			string sql = @"SELECT tVendaBilheteriaFormaPagamento.ID,tVendaBilheteria.DataVenda, tVendaBilheteriaFormaPagamento.Dias FROM tVendaBilheteriaFormaPagamento
		                    INNER JOIN tVendaBilheteria ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID ORDER BY tVendaBilheteriaFormaPagamento.ID";
			DataTable tabela = new DataTable();
			tabela.Columns.Add("ID", typeof(int));
			tabela.Columns.Add("DataVenda", typeof(string));
			tabela.Columns.Add("Dias", typeof(int));
			bd.Executar(sql);
			while (bd.Consulta().Read())
			{
				DataRow linha = tabela.NewRow();
				linha["ID"] = bd.LerInt("ID");
				linha["DataVenda"] = bd.LerString("DataVenda");
				linha["Dias"] = bd.LerInt("Dias");
				tabela.Rows.Add(linha);

			}
			DateTime data = new DateTime();
			bd.Fechar();
			bd.IniciarTransacao();
			foreach (DataRow linha in tabela.Rows)
			{
				data = DateTime.ParseExact((string)linha["DataVenda"], "yyyyMMddHHmmss", null);
				data = data.AddDays((int)linha["Dias"]);
				string sql2 = "UPDATE tVendaBilheteriaFormaPagamento SET DataDeposito = '" + data.ToString("yyyyMMddHHmmss") + "' WHERE ID = " + (int)linha["ID"];
				bd.Executar(sql2);
			}


			bd.FinalizarTransacao();
		}

		                
    }

    public class VendaBilheteriaFormaPagamentoItemLista : VendaBilheteriaFormaPagamentoItemLista_B
    {

        public VendaBilheteriaFormaPagamentoItemLista() { }

        public VendaBilheteriaFormaPagamentoItemLista(int usuarioIDLogado) : base() { }

    }

}
