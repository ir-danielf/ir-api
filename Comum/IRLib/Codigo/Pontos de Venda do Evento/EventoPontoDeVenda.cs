/**************************************************
* Arquivo: EventoPontoDeVenda.cs
* Gerado: 14/07/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System.Collections.Generic;
using System.Linq;

namespace IRLib
{

    public class EventoPontoDeVenda : EventoPontoDeVenda_B
    {

        public EventoPontoDeVenda() { }

        public EventoPontoDeVenda(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public List<EstruturaEventoPontoDeVenda> CarregarPorEvento(int eventoID)
        {
            try
            {
                List<EstruturaEventoPontoDeVenda> lista = new List<EstruturaEventoPontoDeVenda>();
                string sql =
                    string.Format(@"
                        SELECT
	                         pv.ID AS PontoDeVendaID, pv.Nome AS PontoDeVenda, 
	                         IsNull(epv.ID, 0) AS EventoPontoDeVendaID,
	                         e.HabilitarRetiradaTodosPDV
	                        FROM tPontoVenda pv (NOLOCK)
	                        LEFT JOIN tEventoPontoDeVenda epv (NOLOCK) ON pv.ID = epv.PontoDeVendaID AND epv.EventoID = {0}
	                        LEFT JOIN tEvento e (NOLOCK) ON e.ID = {0}
	                        WHERE pv.PermiteRetirada = 'T'
                        ", eventoID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEventoPontoDeVenda()
                    {
                        ID = bd.LerInt("EventoPontoDeVendaID"),
                        PontoDeVendaID = bd.LerInt("PontoDeVendaID"),
                        PontoDeVenda = bd.LerString("PontoDeVenda"),
                        EventoID = eventoID,
                        Disponivel = bd.LerInt("EventoPontoDeVendaID") > 0 || bd.LerBoolean("HabilitarRetiradaTodosPDV"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEventoPontoDeVenda> Salvar(List<EstruturaEventoPontoDeVenda> lista)
        {
            try
            {
                int eventoID = lista[0].EventoID;

                Evento oEvento = new Evento(this.Control.UsuarioID);
                oEvento.Ler(eventoID);

                //Ta tudo disponível
                if (lista.Where(c => !c.Disponivel).Count() == 0)
                {
                    oEvento.HabilitarRetiradaTodosPDV.Valor = true;
                    this.RemoverTudoPorEvento(eventoID);

                }
                else if (lista.Where(c => c.Disponivel).Count() == 0)
                {
                    this.RemoverTudoPorEvento(eventoID);
                    oEvento.HabilitarRetiradaTodosPDV.Valor = false;
                }
                else
                {
                    foreach (var item in lista)
                    {
                        //Não existe e está disponível
                        if (item.ID == 0 && item.Disponivel)
                        {
                            this.EventoID.Valor = item.EventoID;
                            this.PontoDeVendaID.Valor = item.PontoDeVendaID;
                            this.Inserir();
                            item.ID = this.Control.ID;
                            this.Limpar();
                        }
                        //Existe e não está disponivel
                        else if (item.ID > 0 && !item.Disponivel)
                        {
                            this.Excluir(item.ID);
                            item.ID = 0;
                            item.Disponivel = false;
                        }
                    }

                    oEvento.HabilitarRetiradaTodosPDV.Valor = false;
                }
                oEvento.Atualizar();
                return lista;
            }
            finally
            {
                
            }
        }

        public void RemoverTudoPorEvento(int eventoID)
        {
            try
            {
                bd.Executar("DELETE FROM tEventoPontoDeVenda WHERE EventoID = " + eventoID);
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class EventoPontoDeVendaLista : EventoPontoDeVendaLista_B
    {

        public EventoPontoDeVendaLista() { }

        public EventoPontoDeVendaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
