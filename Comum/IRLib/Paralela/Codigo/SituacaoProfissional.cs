/**************************************************
* Arquivo: SituacaoProfissional.cs
* Gerado: 06/10/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class SituacaoProfissional : SituacaoProfissional_B
    {
        public SituacaoProfissional() { }


        public List<EstruturaIDNome> ListaTudo()
        {
            try
            {
                List<EstruturaIDNome> ListaToda = new List<EstruturaIDNome>();

                string SQL = "Select * from tSituacaoProfissional";

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                {
                    ListaToda.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Situacao")
                    });
                }

                return ListaToda;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public int BuscarIDPeloNome(string SituacaoNome)
        {
            try
            {
                string SQL = "Select ID from tSituacaoProfissional Where Situacao = '" + SituacaoNome + "'";
                int ID = 0;

                bd.Consulta(SQL);
                if(bd.Consulta().Read())
                {
                    ID = bd.LerInt("ID");
                }

                return ID;

            }
            catch (Exception)
            {                
                throw;
            }
        }
    }

    public class SituacaoProfissionalLista : SituacaoProfissionalLista_B
    {

        public SituacaoProfissionalLista() { }
    }
}
