
using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Linq;

namespace IRLib
{
    public class LocalPrecoBase : LocalPrecoBase_B
    {
        public LocalPrecoBase() { }

        public LocalPrecoBase(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public DataTable CarregarPrecosBase(int localID)
        {
            DataTable tabela = null;
            BD bd = new BD();
            string query = string.Format(@"Select Nome as 'Preco', Desconto, Principal from LocalPrecoBase(NOLOCK)
                                           WHERE LocalPrecoBase.LocalID = {0} ORDER BY Principal Desc", localID);
            try
            {
                tabela = bd.QueryToTable(query);

                if(tabela.Rows.Count > 0 && tabela.AsEnumerable().All(x=>!x.Field<Boolean>("Principal")))
                {
                    DataRow row = tabela.NewRow();
                    row["Preco"] = "Principal";
                    row["Desconto"] = 0;
                    row["Principal"] = true;
                    tabela.Rows.InsertAt(row, 0);
                }

                return tabela;
            }
            catch 
            {
                return null;   
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable loadPrecosLocal(int localID)
        {
            string query = @"SELECT
                                ID
                                ,Nome
                                ,Desconto
                                ,Principal
                             FROM
                                LocalPrecoBase(NOLOCK)
                             WHERE
                                LocalID = " + localID;
            return bd.QueryToTable(query);
        }
    }
}
