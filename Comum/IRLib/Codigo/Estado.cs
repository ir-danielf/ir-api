/**************************************************
* Arquivo: Estado.cs
* Gerado: 14/11/2008
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{

    public class Estado : Estado_B
    {

        public Estado() { }

        public Estado(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable EstruturaTabela()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID", typeof(int));
            dtt.Columns.Add("Sigla", typeof(string));
            dtt.Columns.Add("PaisID", typeof(int));
            dtt.Columns.Add("Nome", typeof(string));
            return dtt;
        }

        public DataTable TabelaPorPais(int paisID)
        {
            try
            {

                DataTable dtt = this.EstruturaTabela();
                DataRow dtr;
                bd.Consulta("SELECT ID, Sigla FROM tEstado (NOLOCK) WHERE PaisID = " + paisID + " ORDER BY Sigla");
                while (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["Sigla"] = bd.LerString("Sigla");
                    dtr["PaisID"] = paisID;
                    dtt.Rows.Add(dtr);
                }
                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable TabelaPorPais(string pais)
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@Pais", Value = pais, DbType = DbType.String });

                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("SELECT tEstado.ID, ");
                strBuilder.Append("tEstado.Sigla, ");
                strBuilder.Append("tEstado.PaisID, ");
                strBuilder.Append("tPais.Nome ");
                strBuilder.Append("FROM tEstado (NOLOCK) ");
                strBuilder.Append("LEFT JOIN tPais (NOLOCK) on tEstado.PaisID = tPais.ID ");
                strBuilder.Append("WHERE tPais.Nome LIKE @Pais + '%' ");

                DataTable dtt = this.EstruturaTabela();
                DataRow dtr;
                bd.Consulta(strBuilder.ToString(), parametros);

                while (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["Sigla"] = bd.LerString("Sigla");
                    dtr["PaisID"] = bd.LerInt("PaisID");
                    dtr["Nome"] = bd.LerString("Nome");
                    dtt.Rows.Add(dtr);
                }
                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable Todos()
        {
            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));
                tabela.Columns.Add("PaisID", typeof(int));

                bd.Consulta("SELECT * FROM tEstado(NOLOCK)");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    linha["PaisID"] = bd.LerInt("PaisID");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable Todos(bool selecione)
        {
            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));
                tabela.Columns.Add("PaisID", typeof(int));

                bd.Consulta("SELECT * FROM tEstado(NOLOCK)");

                if (selecione)
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Sigla"] = "Selecione....";
                    tabela.Rows.Add(linha);


                }


                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    linha["PaisID"] = bd.LerInt("PaisID");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { bd.Fechar(); }
        }

        public DataTable TodosComTaxaProcessamento()
        {
            try
            {
                DataTable tabela = new DataTable("Estado");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Sigla", typeof(string));
                tabela.Columns.Add("PaisID", typeof(int));
                tabela.Columns.Add("ValorTaxa", typeof(decimal));
                tabela.Columns.Add("LimiteMaximoIngressosEstado", typeof(int));
                tabela.Columns.Add("PossuiTaxaProcessamento", typeof(bool));

                bd.Consulta("SELECT * FROM tEstado(NOLOCK)");


                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Sigla"] = bd.LerString("Sigla");
                    linha["PaisID"] = bd.LerInt("PaisID");
                    linha["ValorTaxa"] = bd.LerDecimal("ValorTaxa");
                    linha["LimiteMaximoIngressosEstado"] = bd.LerInt("LimiteMaximoIngressosEstado");
                    linha["PossuiTaxaProcessamento"] = bd.LerBoolean("PossuiTaxaProcessamento");
                    tabela.Rows.Add(linha);
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { bd.Fechar(); }
        }

        public int GetEstadoID(string estado)
        {
            object obj = bd.ConsultaValor("SELECT ID FROM tEstado(NOLOCK) WHERE Sigla = '" + estado + "'");
            return (obj != null) ? (int)obj : 0;
        }
    }

    public class EstadoLista : EstadoLista_B
    {

        public EstadoLista() { }

        public EstadoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
