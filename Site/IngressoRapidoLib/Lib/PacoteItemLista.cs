using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace IngressoRapido.Lib
{
    [DataContract]
    public class PacoteItemLista : List<PacoteItem>
    {
        private DAL oDAL = new DAL();
        private PacoteItem oPacoteItem;

        public PacoteItemLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Pacotes do tipo Pacote, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private PacoteItemLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                strSql = "SELECT IR_PacoteItemID, PacoteID, Pacote.Nome as Pacote, " +
                       "IR_EventoID AS EventoID, Evento.Nome as Evento, Horario, Setor.Nome AS Setor, Quantidade, Local.Estado, Evento.PossuiTaxaProcessamento, LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado " +
                       "FROM PacoteItem (NOLOCK) " +
                       "INNER JOIN Preco (NOLOCK) ON IR_PrecoID = PacoteItem.PrecoID " +
                       "INNER JOIN Setor (NOLOCK) ON IR_SetorID = Preco.SetorID AND Setor.ApresentacaoID = Preco.ApresentacaoID " +
                       "INNER JOIN Apresentacao (NOLOCK) ON IR_ApresentacaoID = Preco.ApresentacaoID " +
                       "INNER JOIN Evento (NOLOCK) ON IR_EventoID = PacoteItem.EventoID " +
                       "INNER JOIN Pacote (NOLOCK) ON IR_PacoteID = PacoteItem.PacoteID " +
                       "INNER JOIN Local (NOLOCK) ON IR_LocalID = Evento.LocalID " +
                       "WHERE " + clausula;
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oPacoteItem = new PacoteItem((int)dr["IR_PacoteItemID"]);
                        oPacoteItem.PacoteID = (int)dr["PacoteID"];
                        oPacoteItem.EventoID = (int)dr["EventoID"];
                        oPacoteItem.Evento = dr["Evento"].ToString();
                        oPacoteItem.Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        oPacoteItem.Setor = dr["Setor"].ToString();
                        oPacoteItem.Quantidade = (int)dr["Quantidade"];
                        oPacoteItem.Pacote = dr["Pacote"].ToString();
                        oPacoteItem.EventoID = int.Parse(dr["EventoID"].ToString());
                        oPacoteItem.Estado = dr["Estado"].ToString();
                        oPacoteItem.LimiteMaximoIngressosEvento = dr["LimiteMaximoIngressosEvento"].ToInt32();
                        oPacoteItem.LimiteMaximoIngressosEstado = dr["LimiteMaximoIngressosEstado"].ToInt32();
                        oPacoteItem.PossuiTaxaProcessamento = dr["PossuiTaxaProcessamento"].ToBoolean();
                        this.Add(oPacoteItem);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PacoteItemLista CarregarPorPacoteID(int id)
        {
            return CarregarLista("PacoteID = " + id + " ORDER BY Horario");
        }
        public PacoteItemLista CarregarTodos()
        {
            return CarregarLista("1=1");
        }
    }
}
