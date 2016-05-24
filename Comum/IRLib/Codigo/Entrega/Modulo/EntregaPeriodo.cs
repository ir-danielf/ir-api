/**************************************************
* Arquivo: EntregaPeriodo.cs
* Gerado: 06/01/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class EntregaPeriodo : EntregaPeriodo_B
    {

        public EntregaPeriodo() { }

        public EntregaPeriodo(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaEntregaPeriodo> Listar(bool primeiro)
        {
            return Listar(primeiro, "Selecione");
        }

        public List<EstruturaEntregaPeriodo> Listar(bool primeiro, string textoExibicao)
        {

            try
            {

                List<EstruturaEntregaPeriodo> lista = new List<EstruturaEntregaPeriodo>();
                string sql = "SELECT * FROM tEntregaPeriodo (NOLOCK)";
                bd.Consulta(sql);

                if (primeiro)
                {
                    lista.Add(new EstruturaEntregaPeriodo
                    {
                        ID = 0,
                        Nome = textoExibicao,
                    });
                }

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEntregaPeriodo
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        HoraInicial = bd.LerString("HoraInicial"),
                        HoraFinal = bd.LerString("HoraFinal")
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

        public List<EstruturaIDNome> ListarEntregaID(int EntregaID, bool primeiro)
        {

            try
            {

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                string sql = @"SELECT DISTINCT p.id,p.nome 
                    FROM tEntregaPeriodo p(nolock)
                    INNER JOIN tEntregaControle c (nolock) ON (c.PeriodoID=p.ID)
                    WHERE EntregaID = " + EntregaID;

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

        public string LerNome(int ID)
        {

            try
            {

                string nome = "-";
                string sql = @"SELECT  
                                p.Nome
                                FROM tEntregaPeriodo p (nolock)
                                where id = " + ID;

                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    nome = bd.LerString("Nome");
                }

                return nome;

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


    public class EntregaPeriodoLista : EntregaPeriodoLista_B
    {

        public EntregaPeriodoLista() { }

        public EntregaPeriodoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
