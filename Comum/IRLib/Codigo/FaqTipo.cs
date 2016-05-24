/**************************************************
* Arquivo: FaqTipo.cs
* Gerado: 25/02/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class FaqTipo : FaqTipo_B
    {

        public FaqTipo() { }

        public FaqTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaFaqTipo> Buscar()
        {

            try
            {

                List<EstruturaFaqTipo> lista = new List<EstruturaFaqTipo>();
                

                string sql = @"SELECT 
                                t.ID,
                                t.Nome
                                FROM tFaqTipo t(nolock) ";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaFaqTipo
                    {
                        Nome = bd.LerString("Nome"),
                        ID = bd.LerInt("ID"),

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

        public bool PossuiPergunta(int faqTipoID)
        {

            try
            {
                bool verifica = false;
                string sql = @"SELECT ID FROM tFaq (nolock) WHERE FaqTipoID = " + faqTipoID;


                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    verifica = true;

                }

                return verifica;

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

    public class FaqTipoLista : FaqTipoLista_B
    {

        public FaqTipoLista() { }

        public FaqTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
