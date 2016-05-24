/**************************************************
* Arquivo: VoceSabia.cs
* Gerado: 18/04/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class VoceSabia : VoceSabia_B
    {

        public VoceSabia() { }

        public VoceSabia(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public List<EstruturaVoceSabia> CarregaListaVoceSabia()
        {
            List<EstruturaVoceSabia> lista = new List<EstruturaVoceSabia>();
            EstruturaVoceSabia oVoceSabia;

            try
            {
                string sql = "SELECT ID, Identificacao, Texto FROM tVoceSabia ORDER BY ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    oVoceSabia = new ClientObjects.EstruturaVoceSabia();
                    //popula objeto
                    oVoceSabia.ID = bd.LerInt("ID");
                    oVoceSabia.Identificacao = bd.LerString("Identificacao");
                    oVoceSabia.Texto = bd.LerString("Texto");
                    lista.Add(oVoceSabia);
                }

                return lista;

            }
            catch (Exception ex)
            {
                bd.Fechar();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }


    }

    public class VoceSabiaLista : VoceSabiaLista_B
    {

        public VoceSabiaLista() { }

        public VoceSabiaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }
}
