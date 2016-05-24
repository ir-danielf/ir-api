using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    public class ValeIngressoTipoLista : List<ValeIngressoTipo>
    {
        DAL oDAL = new DAL();
        ValeIngressoTipo oValeIngresso;

        public ValeIngressoTipoLista()
        {
            this.Clear();
        }
        private int registros;
        public int Registros
        {
            get { return registros; }
            set { registros = value; }

        }

        public static ValeIngressoTipoLista Pesquisar(int startRowIndex, int numRows)
        {
            DAL oDAL = new DAL();
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("WITH tbGeral AS ( SELECT DISTINCT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo, ValorPagamento,ValorTipo,TrocaConveniencia,TrocaEntrega,TrocaIngresso FROM ValeIngressoTipo (NOLOCK) ), ");
                stbSQL.Append("tbCount AS (SELECT Count(IR_ValeIngressoTipoID) as Registros FROM tbGeral), ");
                stbSQL.Append("tbOrdenada AS( SELECT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo, ValorPagamento,ValorTipo,TrocaConveniencia,TrocaEntrega,TrocaIngresso , ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber' FROM tbGeral) ");
                stbSQL.Append("SELECT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo, RowNumber, Registros, ValorPagamento,ValorTipo,TrocaConveniencia,TrocaEntrega,TrocaIngresso FROM tbOrdenada, tbCount ");
                stbSQL.Append("WHERE RowNumber between " + startRowIndex + " and " + (startRowIndex + numRows - 1) + " ORDER BY Nome");

                ValeIngressoTipoLista listaValeIngresso = new ValeIngressoTipoLista();
                ValeIngressoTipo oValeIngresso;

                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    while (dr.Read())
                    {
                        oValeIngresso = new ValeIngressoTipo(Convert.ToInt32(dr["IR_ValeIngressoTipoID"]));
                        oValeIngresso.Nome = dr["Nome"].ToString();
                        oValeIngresso.ValorPagamento = Convert.ToDecimal(dr["ValorPagamento"]);
                        switch (dr["Acumulativo"].ToString())
                        {
                            case "T":
                                oValeIngresso.Acumulativo = "Acumulativo: Sim";
                                break;
                            case "F":
                                oValeIngresso.Acumulativo = "Acumulativo: Não";
                                break;
                        }

                        oValeIngresso.ValorTipo = Convert.ToChar(dr["ValorTipo"].ToString());
                        switch(oValeIngresso.ValorTipo) {
                            case 'V':
                            oValeIngresso.Valor = "Na Troca: R$" + Convert.ToDecimal(dr["Valor"]);
                            break;
                            case 'P':
                            oValeIngresso.Valor = "Na Troca: " + Convert.ToInt32(dr["Valor"]) + "%";
                            break;
                        }

         
                        oValeIngresso.TrocaConveniencia = Convert.ToChar(dr["TrocaConveniencia"]) == 'T';
                        oValeIngresso.TrocaEntrega = Convert.ToChar(dr["TrocaEntrega"]) == 'T';
                        oValeIngresso.TrocaIngresso = Convert.ToChar(dr["TrocaIngresso"]) == 'T';


                        if (Convert.ToInt32(dr["ValidadeDiasImpressao"]) != 0)
                            oValeIngresso.ValidadeData = "Válido por: " + Convert.ToInt32(dr["ValidadeDiasImpressao"]) + " dia(s) após impressão";
                        else
                            oValeIngresso.ValidadeData = "Válido até: " + DateTime.ParseExact(dr["ValidadeData"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        if (numRows != 0)
                            listaValeIngresso.registros = Convert.ToInt32(dr["Registros"]);
                        listaValeIngresso.Add(oValeIngresso);
                    }
                }
                return listaValeIngresso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public ValeIngressoTipoLista CarregarLista(string busca, int startRowIndex, int quantidade)
        {
            DAL oDAL = new DAL();
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("WITH tbGeral AS ( SELECT DISTINCT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, ");
                stbSQL.Append("IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo,ValorPagamento, ValorTipo FROM ValeIngressoTipo (NOLOCK)  ");
                if (busca.Length > 0)
                    stbSQL.Append("WHERE " + busca + "), ");
                else
                    stbSQL.Append("),");

                stbSQL.Append("tbCount AS (SELECT Count(IR_ValeIngressoTipoID) as Registros FROM tbGeral), ");

                stbSQL.Append("tbOrdenada AS( SELECT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo, ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber', ValorPagamento, ValorTipo FROM tbGeral) ");
                stbSQL.Append("SELECT IR_ValeIngressoTipoID, Nome, Valor, IsNull(ValidadeData,0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, Acumulativo, RowNumber, Registros, ValorPagamento, ValorTipo FROM tbOrdenada, tbCount ");

                stbSQL.Append("WHERE RowNumber BETWEEN " + startRowIndex + " AND " + (startRowIndex + quantidade - 1));
                if (busca.Length > 0)
                    stbSQL.Append(" AND " + busca + " ");
                stbSQL.Append(" ORDER BY Nome");

                ValeIngressoTipo oValeIngresso;

                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    while (dr.Read())
                    {
                        oValeIngresso = new ValeIngressoTipo(Convert.ToInt32(dr["IR_ValeIngressoTipoID"]));
                        oValeIngresso.Nome = dr["Nome"].ToString();

                        oValeIngresso.ValorPagamento = Convert.ToDecimal(dr["ValorPagamento"]);

                        switch (dr["Acumulativo"].ToString())
                        {
                            case "T":
                                oValeIngresso.Acumulativo = "Acumulativo: Sim";
                                break;
                            case "F":
                                oValeIngresso.Acumulativo = "Acumulativo: Não";
                                break;
                        }

                        oValeIngresso.ValorTipo = Convert.ToChar(dr["ValorTipo"].ToString());
                        switch(oValeIngresso.ValorTipo) {
                            case 'V':
                            oValeIngresso.Valor = "Na Troca: R$" + Convert.ToDecimal(dr["Valor"]);
                            break;
                            case 'P':
                            oValeIngresso.Valor = "Na Troca: " + Convert.ToInt32(dr["Valor"]) + "%";
                            break;
                        }

                        if (Convert.ToInt32(dr["ValidadeDiasImpressao"]) != 0)
                            oValeIngresso.ValidadeData = "Válido por: " + Convert.ToInt32(dr["ValidadeDiasImpressao"]) + " dia(s) após impressão";
                        else
                            oValeIngresso.ValidadeData = "Válido até: " + DateTime.ParseExact(dr["ValidadeData"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

                        if (quantidade != 0)
                            this.registros = Convert.ToInt32(dr["Registros"]);
                        this.Add(oValeIngresso);
                    }
                }
                return this;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDAL.ConnClose();
            }


        }

        public ValeIngressoTipoLista CarregarListaPorNome(string clausula, int startRowIndex, int quantidade)
        {
            clausula = "Nome LIKE '%" + clausula.Replace("'", "") + "%'";
            return this.CarregarLista(clausula, startRowIndex, quantidade);
        }
    }
}
