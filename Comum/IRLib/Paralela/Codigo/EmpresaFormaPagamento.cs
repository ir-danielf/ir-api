/**************************************************
* Arquivo: EmpresaFormaPagamento.cs
* Gerado: 05/08/2008
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

	public class EmpresaFormaPagamento : EmpresaFormaPagamento_B
	{

		public EmpresaFormaPagamento() { }

		public EmpresaFormaPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }

		/// <summary>		
		/// Obter todas formas de pagamento da empresa
		/// </summary>
		/// <returns></returns>
		public DataTable Todos(int empresaID)
		{

			try
			{
				DataColumn dcIndice = new DataColumn("Indice", typeof(int));
				dcIndice.AutoIncrement = true;
				dcIndice.AutoIncrementSeed = 1;
				dcIndice.AutoIncrementStep = 1;

				DataTable tabela = new DataTable("Local");

				tabela.Columns.Add("ID", typeof(int));
				tabela.Columns.Add("FormaPagamentoID", typeof(int));
				tabela.Columns.Add("Nome", typeof(string));
				tabela.Columns.Add("Parcelas", typeof(int));
				tabela.Columns.Add("Dias", typeof(int));
				tabela.Columns.Add("TaxaAdm", typeof(decimal));
				tabela.Columns.Add("IR", typeof(bool));
				tabela.Columns.Add(dcIndice);

				string sql = "SELECT tEmpresaFormaPagamento.ID,tFormaPagamento.ID AS FormaPagamentoID, tFormaPagamento.Nome,tFormaPagamento.Parcelas,tEmpresaFormaPagamento.Dias,TaxaAdm,tEmpresaFormaPagamento.IR " +
							 " FROM tFormaPagamento " +
							 " LEFT JOIN tEmpresaFormaPagamento ON tFormaPagamento.ID = tEmpresaFormaPagamento.FormaPagamentoID " +
							 " AND tEmpresaFormaPagamento.EmpresaID = " + empresaID + " " +
							 " ORDER BY Nome";

				bd.Consulta(sql);

				while (bd.Consulta().Read())
				{
					DataRow linha = tabela.NewRow();
					linha["ID"] = bd.LerInt("ID");
					linha["FormaPagamentoID"] = bd.LerInt("FormaPagamentoID");
					linha["Nome"] = bd.LerString("Nome");
					linha["Parcelas"] = bd.LerInt("Parcelas");
					linha["Dias"] = bd.LerInt("Dias");
					linha["TaxaAdm"] = bd.LerDecimal("TaxaAdm");
					linha["IR"] = bd.LerBoolean("IR");

					tabela.Rows.Add(linha);
				}
				bd.Fechar();

				return tabela;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Função para salvar as formas de pagamento alteradas na EmpresaFormaPagamento
		/// </summary>
		/// <param name="listagem"></param>
		public void SalvarListagem(List<IRLib.Paralela.ClientObjects.EmpresaFormaPagamentoListagem> listagem)
		{
			StringBuilder sql = new StringBuilder();
			int dias = 0;
			string taxaAdm = "0";
			string ir = "";
			try
			{
				for (int i = 0; i < listagem.Count; i++)
				{
					dias = listagem[i].Dias != 0 ? listagem[i].Dias : 0;
					taxaAdm = listagem[i].TaxaAdm != 0 ? listagem[i].TaxaAdm.ToString().Replace(',', '.') : "0";
					ir = listagem[i].IR == true ? "T" : "F";
					//verifica se deve dar update ou inserir
					object aux = bd.ConsultaValor("SELECT 1 FROM tEmpresaFormaPagamento(NOLOCK) WHERE ID = " + listagem[i].ID);
					if (aux != null && (int)aux > 0)
					{
						sql.Append(" UPDATE tEmpresaFormaPagamento SET Dias = " + dias + " ,");
						sql.Append(" TaxaAdm = '" + taxaAdm + "' , IR = '" + ir + "' ");
						sql.Append(" WHERE ID = " + listagem[i].ID + " ");
					}
					else
						sql.Append("INSERT INTO tEmpresaFormaPagamento (FormaPagamentoID,EmpresaID,Dias,TaxaAdm,IR) VALUES ("+ listagem[i].FormaPagamentoID + "," + listagem[i].EmpresaID + "," + dias + "," + taxaAdm + ",'" + ir + "')");

				}
				bd.IniciarTransacao();
				if (listagem.Count > 0)
					bd.Executar(sql.ToString());
				bd.FinalizarTransacao();
			}
			catch
			{
				bd.DesfazerTransacao();
				throw;
			}
			finally
			{
				bd.Fechar();
			}

		}
	}

	public class EmpresaFormaPagamentoLista : EmpresaFormaPagamentoLista_B
	{

		public EmpresaFormaPagamentoLista() { }

		public EmpresaFormaPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

	}

}
