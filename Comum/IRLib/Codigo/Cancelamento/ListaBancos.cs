using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.CancelamentoIngresso
{
    public class ListaBancos : ListaBancos_B
    {
        public DataTable PegarListaBancos()
        {
            DataTable retorno = null;
            BD bd = null;

            try
            {
                retorno = new DataTable("ListaBancos");

                retorno.Columns.Add("ID", typeof(int));
                retorno.Columns.Add("Codigo", typeof(string));
                retorno.Columns.Add("NomeBanco", typeof(string));
                retorno.Columns.Add("IRDeposita", typeof(bool));

                bd = new BD();
                string sql = @"SELECT ID, Codigo, NomeBanco, IRDeposita
                                 FROM ListaBancos
                                WHERE IRDeposita = 1 
                             ORDER BY NomeBanco ASC;";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    linha["NomeBanco"] = bd.LerString("NomeBanco");
                    linha["IRDeposita"] = bd.LerBoolean("IRDeposita");

                    retorno.Rows.Add(linha);
                }
                bd.Fechar();
            }
            finally
            {
                bd.Fechar();
            }
            return retorno;
        }
    }
}
