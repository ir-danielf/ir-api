/**************************************************
* Arquivo: ApoliceMondial.cs
* Gerado: 20/12/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib
{

    public class ApoliceMondial : ApoliceMondial_B
    {
        public ApoliceMondial() { }

        public void InserirLista(Mondial.MDLRequestResult[] mdlRequestResult, int vendaBilheteriaID)
        {
            try
            {
                foreach (Mondial.MDLRequestResult item in mdlRequestResult)
                {
                    this.Apolice.Valor = item.PolicyID;
                    this.VendaBilheteriaID.Valor = vendaBilheteriaID;

                    this.Inserir();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string BuscaApolice(int vendaBilheteriaID)
        {
            try
            {
                string apolice = string.Empty;

                string SQL = string.Format(@"SELECT TOP 1 Apolice FROM tApoliceMondial (NOLOCK) 
                            WHERE VendaBilheteriaID = {0} ORDER BY ID", vendaBilheteriaID);

                bd.Consulta(SQL);

                if (bd.Consulta().Read())
                    apolice = bd.LerString("Apolice");

                return apolice;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string BuscaApolice(string SenhaVendaBilheteria)
        {
            try
            {
                string apolice = string.Empty;

                string SQL = string.Format(@"SELECT TOP 1 Apolice FROM tApoliceMondial (NOLOCK) 
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tApoliceMondial.VendaBilheteriaID
                            WHERE tVendaBilheteria.Senha = '{0}' ORDER BY tApoliceMondial.ID", SenhaVendaBilheteria);

                bd.Consulta(SQL);

                if (bd.Consulta().Read())
                    apolice = bd.LerString("Apolice");

                return apolice;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<string> BuscaListaApolice(string SenhaVendaBilheteria)
        {
            try
            {
                List<string> apolice = new List<string>();

                string SQL = string.Format(@"SELECT Apolice FROM tApoliceMondial (NOLOCK) 
                            INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tApoliceMondial.VendaBilheteriaID
                            WHERE tVendaBilheteria.Senha = '{0}' ORDER BY tApoliceMondial.ID", SenhaVendaBilheteria);

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                    apolice.Add(bd.LerString("Apolice"));

                return apolice;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


    }

    public class ApoliceMondialLista : ApoliceMondialLista_B
    {

        public ApoliceMondialLista() { }
    }

}
