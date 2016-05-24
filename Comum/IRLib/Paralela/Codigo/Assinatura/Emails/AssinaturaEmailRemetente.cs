/**************************************************
* Arquivo: AssinaturaEmailRemetente.cs
* Gerado: 19/10/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class AssinaturaEmailRemetente : AssinaturaEmailRemetente_B
    {

        public AssinaturaEmailRemetente() { }

        public AssinaturaEmailRemetente(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public List<EstruturaIDNome>  BuscarModelos(int assinaturaTipoID)
        {
            try
            {
                if (!bd.Consulta(
                    @"SELECT ID, Remetente FROM tAssinaturaEmailRemetente WHERE AssinaturaTipoID = " + assinaturaTipoID + " ORDER BY Remetente").Read())
                    throw new Exception("Não existem remetentes associados a este tipo de assinatura");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Remetente"),
                    });
                } while (bd.Consulta().Read());

                lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Selecione...", });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaEmailRemetenteLista : AssinaturaEmailRemetenteLista_B
    {

        public AssinaturaEmailRemetenteLista() { }

        public AssinaturaEmailRemetenteLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
