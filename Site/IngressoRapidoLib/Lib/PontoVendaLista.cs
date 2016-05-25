using CTLib;
using System;
using System.Collections.Generic;
using System.Data;

namespace IngressoRapido.Lib
{
    public class PontoVendaLista : List<PontoVenda>
    {
        DAL oDAL = new DAL();
        PontoVenda objPV;

        public PontoVendaLista()
        {
            this.Clear();
        }


        public PontoVendaLista CarregarPontoVendaLista(string estado, string cidade)
        {
            string strSql = "";

            if (cidade != "")
            {
                strSql = "SELECT * FROM PontoVenda (NOLOCK) WHERE Estado = '" + estado + "' AND Cidade = '" + cidade + "' ORDER BY Local";
            }
            else
            {
                strSql = "SELECT * FROM PontoVenda (NOLOCK) WHERE Estado = '" + estado + "' ORDER BY Local";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        objPV = new PontoVenda(Convert.ToInt32(dr["IR_PontoVendaID"].ToString()));
                        objPV.Nome = dr["Nome"].ToString().Trim();
                        objPV.Local = dr["Local"].ToString().Trim();
                        objPV.Endereco = dr["Endereco"].ToString().Trim();
                        objPV.Numero = dr["Numero"].ToString().Trim();
                        objPV.Compl = dr["Compl"].ToString().Trim();
                        objPV.Cidade = dr["Cidade"].ToString().Trim();
                        objPV.Estado = dr["Estado"].ToString().Trim();
                        objPV.Bairro = dr["Bairro"].ToString().Trim();
                        objPV.Obs = dr["Obs"].ToString().Trim();
                        objPV.Referencia = dr["Referencia"].ToString().Trim();
                        objPV.CEP = dr["CEP"].ToString().Trim();

                        this.Add(objPV);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PontoVendaLista CarregarPontoVendaLista()
        {
            string strSql = "";

            strSql = "SELECT * FROM PontoVenda (NOLOCK) ORDER BY IR_PontoVendaID";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        objPV = new PontoVenda(Convert.ToInt32(dr["IR_PontoVendaID"].ToString()));
                        objPV.Nome = dr["Nome"].ToString().Trim();
                        objPV.Local = dr["Local"].ToString().Trim();
                        objPV.Endereco = dr["Endereco"].ToString().Trim();
                        objPV.Numero = dr["Numero"].ToString().Trim();
                        objPV.Compl = dr["Compl"].ToString().Trim();
                        objPV.Cidade = dr["Cidade"].ToString().Trim();
                        objPV.Estado = dr["Estado"].ToString().Trim();
                        objPV.Bairro = dr["Bairro"].ToString().Trim();
                        objPV.Obs = dr["Obs"].ToString().Trim();
                        objPV.Referencia = dr["Referencia"].ToString().Trim();
                        objPV.CEP = dr["CEP"].ToString().Trim();
                        objPV.Latitude = dr["Latitude"].ToString();
                        objPV.Longitude = dr["Longitude"].ToString();

                        this.Add(objPV);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PontoVendaLista CarregarPontoVendaListaComCoordenadas()
        {
            string strSql = "";

            strSql = "SELECT * FROM PontoVenda (NOLOCK) WHERE LEN(Latitude) > 0 AND LEN(Longitude) > 0 ORDER BY IR_PontoVendaID";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        objPV = new PontoVenda(Convert.ToInt32(dr["IR_PontoVendaID"].ToString()));
                        objPV.Nome = dr["Nome"].ToString().Trim();
                        objPV.Local = dr["Local"].ToString().Trim();
                        objPV.Endereco = dr["Endereco"].ToString().Trim();
                        objPV.Numero = dr["Numero"].ToString().Trim();
                        objPV.Compl = dr["Compl"].ToString().Trim();
                        objPV.Cidade = dr["Cidade"].ToString().Trim();
                        objPV.Estado = dr["Estado"].ToString().Trim();
                        objPV.Bairro = dr["Bairro"].ToString().Trim();
                        objPV.Obs = dr["Obs"].ToString().Trim();
                        objPV.Referencia = dr["Referencia"].ToString().Trim();
                        objPV.CEP = dr["CEP"].ToString().Trim();
                        objPV.Latitude = dr["Latitude"].ToString();
                        objPV.Longitude = dr["Longitude"].ToString();

                        this.Add(objPV);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PontoVendaLista CarregarEstado()
        {
            string strSql = "SELECT DISTINCT Estado FROM PontoVenda (NOLOCK) ORDER BY Estado";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        PontoVenda objPV = new PontoVenda();

                        objPV.UF = dr["Estado"].ToString().Trim();

                        this.Add(objPV);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PontoVendaLista CarregarCidade(string uf)
        {
            string strSql = "SELECT DISTINCT Cidade FROM PontoVenda (NOLOCK) WHERE Estado='" + uf + "' ORDER BY Cidade";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        PontoVenda objPV = new PontoVenda();

                        objPV.Cidade = dr["Cidade"].ToString().Trim();

                        this.Add(objPV);
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}
