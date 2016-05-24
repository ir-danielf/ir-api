/**************************************************
* Arquivo: CampingSWU.cs
* Gerado: 26/08/2011
* Autor: Celeritas Ltda
***************************************************/


namespace IRLib.Paralela
{

    public class CampingSwu : CampingSwu_B
    {
        public CampingSwu() { }


        public bool VerificaIntegranteExiste(int VendaBilheteriaID, string CPF)
        {
            try
            {
                string sql = string.Empty;
                bool retorno = true;

                sql = @"SELECT ID From tCampingSwu (NOLOCK) Where VendaBilheteriaID = " + VendaBilheteriaID + " AND CPF = '" + CPF + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = false;
                }

                return retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool VerificaIntegranteExiste(string CPF, string Nome)
        {
            try
            {
                string sql = string.Empty;
                bool retorno = true;

                sql = @"SELECT ID From tCampingSwu (NOLOCK) Where CPF = '" + CPF + "' OR Nome LIKE '%" + Nome + "%'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = false;
                }

                return retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class CampingSwuLista : CampingSwu_B
    {
        public CampingSwuLista() { }
    }

}
