/**************************************************
* Arquivo: Feriado.cs
* Gerado: 19/09/2008
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib.Paralela
{

    public class Feriado : Feriado_B
    {

        public Feriado() { }

        public Feriado(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable Todos()
        {
            try
            {
                DataTable tabela = new DataTable();

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Data", typeof(DateTime));

                string sql = "SELECT * FROM tFeriado ORDER BY Data";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Data"] = bd.LerStringFormatoData("Data");
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

        internal bool VerificaFeriado(DateTime dateTime)
        {
            try
            {
                bool feriado = false;
                string data = dateTime.ToString("yyyyMMddHHmmss");
                string sql = "SELECT * FROM tFeriado WHERE Data = " + data;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    feriado = true;
                }

                bd.Fechar();

                return feriado;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class FeriadoLista : FeriadoLista_B
    {
        public FeriadoLista() { }

        public FeriadoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }

}
