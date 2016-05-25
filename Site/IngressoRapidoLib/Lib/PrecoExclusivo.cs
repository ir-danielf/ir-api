using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Lib
{
    public class PrecoExclusivo
    {
        DAL oDAL = new DAL();
        IRLib.PrecoExclusivo precoExclusivo = new IRLib.PrecoExclusivo();
        IRLib.PrecoExclusivoCodigo precoExclusivoCodigo = new IRLib.PrecoExclusivoCodigo();

        public DataTable Todos()
        {            
            return precoExclusivo.Todos();
        }
       
        public bool isCodigoExclusivoValido(int precoExclusivoID, string Codigo, ref int precoExclusivoCodigoID)
        {
            bool retorno = false;
            int totalUtilizado = 0;
            string strSql = "";
            DataTable dtPrecoExclusivo = null;
            DataTable dtPrecoExclusivoCodigo = null;
            try
            {
                dtPrecoExclusivo = precoExclusivo.ListagemPorPrecoExclusivo(precoExclusivoID);
                if (dtPrecoExclusivo != null && dtPrecoExclusivo.Rows.Count > 0)
                {
                    if (((string)dtPrecoExclusivo.Rows[0]["Ativo"]) == "T")
                    {
                        if (((int)dtPrecoExclusivo.Rows[0]["QuantidadeMaxima"]) == 0)
                        {
                            retorno = true;
                        }
                        else
                        {
                            strSql = "SELECT COUNT(*) FROM Carrinho WHERE PrecoExclusivoCodigoID = " + precoExclusivoID.ToString() + " AND Status = 'R'";
                            totalUtilizado += Convert.ToInt32(oDAL.Scalar(strSql, null));

                            IRLib.Ingresso ingresso = new IRLib.Ingresso();
                            totalUtilizado += ingresso.VerificaCodigoPrecoExclusivo(precoExclusivoID);

                            retorno = (totalUtilizado < ((int)dtPrecoExclusivo.Rows[0]["QuantidadeMaxima"]));
                        }

                        if (retorno)
                        {
                            dtPrecoExclusivoCodigo = precoExclusivoCodigo.ListagemPorCodigo(precoExclusivoID, Codigo);
                            if (dtPrecoExclusivoCodigo != null && dtPrecoExclusivoCodigo.Rows.Count > 0)
                            {
                                precoExclusivoCodigoID = ((int)dtPrecoExclusivoCodigo.Rows[0]["ID"]);
                                retorno = true;
                            }
                            else
                            {
                                retorno = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno = false;
            }
            finally
            {
                if (dtPrecoExclusivo != null)
                {
                    dtPrecoExclusivo.Dispose();
                    dtPrecoExclusivo = null;
                }
                if (dtPrecoExclusivoCodigo != null)
                {
                    dtPrecoExclusivoCodigo.Dispose();
                    dtPrecoExclusivoCodigo = null;
                }
            }

            return retorno;
        }

    }
}

