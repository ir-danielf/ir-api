using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace IngressoRapido.Lib
{
    [DataContract]
    public class PacoteItem
    {
        public PacoteItem()
        {
        }

        public PacoteItem(int id)
        {
            this.ID = id;
        }

        private DAL oDAL = new DAL();

        public int ID { get; set; }


        public int PacoteID { get; set; }

        public string Pacote { get; set; }

        public int EventoID { get; set; }

        [DataMember]
        public string Evento { get; set; }


        public DateTime Horario { get; set; }

        [DataMember]
        public string HorarioFormatado
        {
            get { return (Horario.ToString("dd/MM/yyyy") + " às " + Horario.ToString("HH:mm")); }
        }

        [DataMember]
        public string Setor { get; set; }

        public int SetorID { get; set; }

        public int PrecoID { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }
        public string Estado { get; set; }

        public PacoteItem GetByID(int id)
        {
            string strSql = "SELECT IR_PacoteItemID, PacoteID, EventoID, PrecoID, Quantidade FROM PacoteItem (NOLOCK) " +
                            "WHERE (IR_PacoteItemID = " + id + ")";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.ID = (int)dr["IR_PacoteItemID"];
                        this.PacoteID = (int)dr["PacoteID"];
                        this.EventoID = (int)dr["EventoID"];
                        this.PrecoID = (int)dr["PrecoID"];
                        this.Quantidade = (int)dr["Quantidade"];
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
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



        public int LimiteMaximoIngressosEvento { get; set; }

        public int LimiteMaximoIngressosEstado { get; set; }

        public bool PossuiTaxaProcessamento { get; set; }
    }
}