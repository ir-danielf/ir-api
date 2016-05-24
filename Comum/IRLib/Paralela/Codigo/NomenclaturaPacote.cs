/**************************************************
* Arquivo: NomenclaturaPacote.cs
* Gerado: 01/03/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class NomenclaturaPacote : NomenclaturaPacote_B
    {

        public NomenclaturaPacote() { }

        public NomenclaturaPacote(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaNomenclaturaPacote> Buscar()
        {

            try
            {

                List<EstruturaNomenclaturaPacote> lista = new List<EstruturaNomenclaturaPacote>();


                string sql = @"SELECT 
                                t.ID,
                                t.Nome
                                FROM tNomenclaturaPacote t(nolock) ";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaNomenclaturaPacote
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


    }

    public class NomenclaturaPacoteLista : NomenclaturaPacoteLista_B
    {

        public NomenclaturaPacoteLista() { }

        public NomenclaturaPacoteLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
