/**************************************************
* Arquivo: AssinaturaEmailModelo.cs
* Gerado: 19/10/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System.Collections.Generic;

namespace IRLib
{

    public class AssinaturaEmailModelo : AssinaturaEmailModelo_B
    {

        public AssinaturaEmailModelo() { }

        public AssinaturaEmailModelo(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public List<EstruturaIDNome> BuscarModelos(int assinaturaTipoID)
        {
            try
            {
                if (!bd.Consulta(
                    @"SELECT ID, Nome FROM tAssinaturaEmailModelo (NOLOCK) WHERE AssinaturaTipoID = " + assinaturaTipoID + " AND Salvo = 'T' ORDER BY Nome").Read())
                    return new List<EstruturaIDNome>() { new EstruturaIDNome() { ID = 0, Nome = "Crie um modelo." } };

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
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

        public List<EstruturaIDNome> BuscarModelosAssunto(int assinaturaTipoID)
        {
            try
            {
                if (!bd.Consulta(
                    @"SELECT ID, Assunto FROM tAssinaturaEmailModelo (NOLOCK) WHERE AssinaturaTipoID = " + assinaturaTipoID + " AND Salvo = 'F' ORDER BY Nome").Read())
                    return new List<EstruturaIDNome>() { new EstruturaIDNome() { ID = 0, Nome = "Crie um modelo." } };

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Assunto"),
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


    public class AssinaturaEmailModeloLista : AssinaturaEmailModeloLista_B
    {

        public AssinaturaEmailModeloLista() { }

        public AssinaturaEmailModeloLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
