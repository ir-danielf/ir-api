using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for Evento
    /// </summary>
    public class Apresentacao
    {
        public const string FORMATO_DATAHORA_COMPLETO = "dddd, dd \\de MMMM \\de yyyy à\\s HH:mm";
        public Apresentacao()
        {
        }

        public Apresentacao(int id)
        {
            this.id = id;
        }

        private DAL oDAL = new DAL();

        private int id;

        public int Id
        {
            set { id = value; }
            get { return id; }
        }



        public SetorLista SetorLista { get; set; }

        private DateTime horario;

        public DateTime Horario
        {
            get { return horario; }
            set { horario = value; }
        }


        public string HorarioFormatado { get; set; }

        private int eventoID;

        public int EventoID
        {
            get { return eventoID; }
            set { eventoID = value; }
        }

        public bool UsarEsquematico { get; set; }

        public Apresentacao GetByID(int id)
        {
            string strSql = "SELECT IR_ApresentacaoID, Horario FROM Apresentacao (NOLOCK) " +
                            "WHERE (EventoID = " + id + ") ORDER BY Horario";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.id = Convert.ToInt32(dr["IR_ApresentacaoID"].ToString());
                    this.Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
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

        public DateTime ApresentacaoMaisProxima(System.Collections.Generic.List<int> listaApresentacao) {
            IRLib.Apresentacao oAp = new IRLib.Apresentacao();
            return oAp.ApresentacaoMaisProxima(listaApresentacao);
        }
    }
}


