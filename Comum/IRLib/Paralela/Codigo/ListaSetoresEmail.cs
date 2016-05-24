/**************************************************
* Arquivo: ListaSetoresEmail.cs
* Gerado: 07/03/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class ListaSetoresEmail : ListaSetoresEmail_B
    {

        public ListaSetoresEmail() { }


        public List<EstruturaListaSetoreEmail> ListaSetoreEmail()
        {
            try
            {
                List<EstruturaListaSetoreEmail> ListaRetorno = new List<EstruturaListaSetoreEmail>();
                EstruturaListaSetoreEmail estrutura;

                string sql = "SELECT * FROM tListaSetoresEmail ORDER BY ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    estrutura = new EstruturaListaSetoreEmail();
                    estrutura.ID = bd.LerInt("ID");
                    estrutura.Setor = bd.LerString("Setor");
                    estrutura.Email = bd.LerString("Email");
                    estrutura.Responsavel = bd.LerString("Responsavel");

                    ListaRetorno.Add(estrutura);
                }
                bd.Fechar();


                return ListaRetorno;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }

    public class ListaSetoresEmailLista : ListaSetoresEmailLista_B
    {

        public ListaSetoresEmailLista() { }

    }

}
