/**************************************************
* Arquivo: EventoLoja.cs
* Gerado: 25/07/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class EventoLoja : EventoLoja_B
    {

        public EventoLoja() { }

        public EventoLoja(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaEventoLoja> Carregar(int canalID, int lojaID, string evento, bool termica, bool laser)
        {
            try
            {
                List<EstruturaEventoLoja> lista = new List<EstruturaEventoLoja>();

                string ClausulaEvento = string.Empty;

                if (evento != "")
                    ClausulaEvento = "AND e.Nome Like '%" + evento + "%'";

                string sql =
                    string.Format(@"SELECT DISTINCT
	                        ISNULL(el.ID, 0) AS EventoLojaID,
	                        e.ID AS EventoID, e.Nome AS Evento, 
		                        CASE WHEN el.ID IS NOT NULL
			                        THEN ISNULL(el.TipoImpressao, 'A')
			                        ELSE ISNULL(e.TipoImpressao, 'A')
		                        END AS TipoImpressao	
	                        FROM tEvento e (NOLOCK)
	                        INNER JOIN tCanalEvento ce (NOLOCK) ON e.ID = ce.EventoID
	                        LEFT JOIN tEventoLoja el (NOLOCK) ON e.ID = el.EventoID AND el.LojaID = {0}
	                        WHERE ce.CanalID = {1} {2} AND {3}
                            ORDER BY e.Nome", lojaID, canalID, ClausulaEvento, EstruturaTipoImpressao.ToSQL(termica, laser));


                bd.Consulta(sql);
                while (bd.Consulta().Read())
                    lista.Add(new EstruturaEventoLoja()
                    {
                        ID = bd.LerInt("EventoLojaID"),
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Evento"),
                        LojaID = lojaID,
                        Tipo = (EstruturaTipoImpressao.TipoImpressao)Convert.ToChar(bd.LerString("TipoImpressao")),
                    });

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaEventoLoja Associar(EstruturaEventoLoja eventoLoja)
        {
            try
            {
                Evento oEvento = new Evento();
                oEvento.Ler(eventoLoja.EventoID);

                this.Limpar();

                //É igual 
                if (oEvento.TipoImpressao.Valor == ((char)eventoLoja.Tipo).ToString())
                {
                    //Era diferente e foi alterado para igual, deve excluir o registro
                    if (eventoLoja.ID > 0)
                    {
                        this.Control.ID = eventoLoja.ID;
                        this.Excluir();
                        eventoLoja.ID = 0;
                    }
                    return eventoLoja;
                }

                if (eventoLoja.ID == 0)
                {
                    //Não existe e é diferente do padrão no evento
                    this.EventoID.Valor = eventoLoja.EventoID;
                    this.LojaID.Valor = eventoLoja.LojaID;
                    this.TipoImpressao.Valor = ((char)eventoLoja.Tipo).ToString();
                    this.Inserir();

                    eventoLoja.ID = this.Control.ID;
                    return eventoLoja;
                }
                else
                {
                    this.Control.ID = eventoLoja.ID;
                    this.EventoID.Valor = eventoLoja.EventoID;
                    this.LojaID.Valor = eventoLoja.LojaID;
                    this.TipoImpressao.Valor = ((char)eventoLoja.Tipo).ToString();

                    this.Atualizar();

                    return eventoLoja;
                }
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class EventoLojaLista : EventoLojaLista_B
    {

        public EventoLojaLista() { }

        public EventoLojaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
