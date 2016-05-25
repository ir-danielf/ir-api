using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace IngressoRapido.Lib
{
    public class PacoteLista : List<Pacote>
    {
        private DAL oDAL = new DAL();
        private Pacote oPacote;

        public PacoteLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Pacotes do tipo Pacote, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private PacoteLista CarregarLista(int EventoID, bool Assinatura)
        {
            string strSql = string.Empty;

            if (EventoID == 0)
                return null;

            try
            {
                strSql = "sp_getPacotes3 " + EventoID;
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql, CommandType.StoredProcedure))
                {
                    PacoteItem oPacoteItem;
                    PacoteItemLista oPacoteItemLista = new PacoteItemLista();

                    while (dr.Read())
                    {
                        bool adicionarPacote = this.Where(c => c.ID == (int)dr["IR_PacoteID"]).Count() == 0;
                        if (adicionarPacote)
                        {

                            oPacoteItemLista = new PacoteItemLista();
                            oPacote = new Pacote(Convert.ToInt32(dr["IR_PacoteID"]));
                            oPacote.Nome = Util.LimparTitulo(dr["PacoteNome"].ToString()).Trim();
                            oPacote.PacoteItemLista = oPacoteItemLista;
                            oPacote.NomenclaturaPacoteID = Convert.ToInt32(dr["NomenclaturaPacoteID"]);
                            oPacote.Tipo = dr["LugarMarcado"].ToString();
                            this.Add(oPacote);
                        }
                        oPacoteItem = new PacoteItem(Convert.ToInt32(dr["IR_PacoteItemID"]));
                        oPacoteItem.Pacote = Util.LimparTitulo(dr["PacoteNome"].ToString()).Trim();
                        oPacoteItem.Evento = Util.LimparTitulo(dr["EventoNome"].ToString()).Trim();
                        oPacoteItem.Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao);

                        oPacoteItem.Valor = Convert.ToDecimal(dr["Valor"]) * Convert.ToDecimal(dr["Quantidade"]);
                        oPacoteItem.Setor = dr["SetorNome"].ToString();
                        //oPacoteItem.SetorID = Convert.ToInt32(dr["SetorID"]);
                        oPacoteItemLista.Add(oPacoteItem);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public PacoteLista CarregarPorEventoID(int id, bool assinatura)
        {
            return CarregarLista(id, assinatura);
        }
    }
}
