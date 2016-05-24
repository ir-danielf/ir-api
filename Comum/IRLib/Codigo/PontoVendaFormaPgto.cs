/**************************************************
* Arquivo: PontoVendaFormaPgto.cs
* Gerado: 27/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib
{

    public class PontoVendaFormaPgto : PontoVendaFormaPgto_B
    {

        public PontoVendaFormaPgto() { }

        public PontoVendaFormaPgto(int usuarioIDLogado) : base(usuarioIDLogado) { }


    }

    public class PontoVendaFormaPgtoLista : PontoVendaFormaPgtoLista_B
    {

        public PontoVendaFormaPgtoLista() { }

        public PontoVendaFormaPgtoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable CarregarFormasPgto()
        {

            try
            {

                DataTable tabela = new DataTable("FormasPgto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tPontoVendaFormaPgto ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        public DataTable CarregarFormasPgtoPorPV(int intPV)
        {
            try
            {

                DataTable tabela = new DataTable("FormasPgto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tPontoVendaFormaPgto.ID, Nome FROM tPontoVendaXFormaPgto" +
                    " INNER JOIN tPontoVendaFormaPgto ON tPontoVendaFormaPgto.ID = tPontoVendaXFormaPgto.PontoVendaFormaPgtoID" +
                    " WHERE PontoVendaID = " + intPV + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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
    }

}
