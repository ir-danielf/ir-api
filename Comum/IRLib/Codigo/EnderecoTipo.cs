/**************************************************
* Arquivo: EnderecoTipo.cs
* Gerado: 25/03/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class EnderecoTipo : EnderecoTipo_B
    {

        public EnderecoTipo() { }

        public List<EstruturaIDNome> Listar(bool primeiro)
        {

            try
            {

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                string sql = @"SELECT id,nome 
                    FROM tEnderecoTipo";

                bd.Consulta(sql);

                if (primeiro)
                {
                    lista.Add(new EstruturaIDNome
                    {
                        ID = 0,
                        Nome = "Selecione",
                    });
                }

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });

                }

                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }


    }

    public class EnderecoTipoLista : EnderecoTipoLista_B
    {

        public EnderecoTipoLista() { }


    }

}
