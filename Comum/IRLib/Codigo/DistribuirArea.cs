/**************************************************
* Arquivo: DistribuirArea.cs
* Gerado: 12/04/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class DistribuirArea : DistribuirArea_B
    {

        public DistribuirArea() { }

        public bool Distribuir(int EntregaID, int AreaID)
        {
            string Data = DateTime.Now.Date.ToString("yyyyMMddHHmm");

            string sql = @" EXEC DistribuiArea " + EntregaID + " , " + AreaID + " , '" + Data +"'   ";

            int alterados = bd.Executar(sql);

            if (alterados == 0)
                return false;
            else
                return true;
        }

        public List<EstruturaIDNome> BuscarTaxas()
        {
            try
            {
                string sql =
                    string.Format(@" SELECT ID, Nome FROM tEntrega ");

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Erro ao buscar entregas");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarAreas()
        {
            try
            {
                string sql =
                    string.Format(@" SELECT ID, Nome FROM tEntregaArea ");

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Erro ao buscar areas");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }


}
