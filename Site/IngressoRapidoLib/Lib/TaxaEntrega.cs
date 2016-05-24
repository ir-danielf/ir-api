using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for Evento
    /// </summary>
    /// 
    [DataContract]
    public class TaxaEntrega
    {
        private DAL oDAL = new DAL();

        public TaxaEntrega() { }

        public TaxaEntrega(int ID)
        {
            this.ID = ID;
        }

        [DataMember]
        public int ID { get; set; }

        public int IR_ID { get; set; }

        public string TaxaID_TaxaValor
        {
            get
            {
                return IR_ID + ";" + Valor;
            }
        }

        public string TaxaID_TaxaValor_Procedimento
        {
            get
            {
                return this.TaxaID_TaxaValor + ";" + this.ProcedimentoEntrega;
            }
        }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string ValorFor { get { return this.Valor.ToString("c"); } }

        public decimal Valor { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public string PrazoEntregaFor
        {
            get
            {
                if (PrazoEntrega > 0)
                    return string.Format("{0} dias", this.PrazoEntrega);
                else
                    return " -- ";
            }
        }

        public int PrazoEntrega { get; set; }

        [DataMember]
        public string ProcedimentoEntrega { get; set; }

        public bool PermitirImpressaoInternet { get; set; }

        public TaxaEntrega GetByID(int id)
        {
            string strSql = "SELECT Nome, Valor, Estado, PrazoEntrega, ProcedimentoEntrega, PermitirImpressaoInternet FROM TaxaEntrega (NOLOCK) " +
                            "WHERE (IR_TaxaEntregaID = " + id + ")";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.IR_ID = id;
                    this.Nome = Util.LimparTitulo(dr.GetString(0));
                    this.Valor = dr.GetDecimal(1);
                    this.Estado = Util.LimparTitulo(dr.GetString(2));
                    this.PrazoEntrega = dr.GetInt32(3);
                    this.ProcedimentoEntrega = dr.GetString(4);
                    this.PermitirImpressaoInternet = dr.GetBoolean(5);
                }
                return this;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public TaxaEntrega TaxaCompra(int clienteID, string estado, string sessionID, bool entregaGratuita, int taxaID)
        {
            string strSql = string.Empty;

            TaxaEntrega oTaxaEntrega = new TaxaEntrega();

            Carrinho oCarrinho = new Carrinho();
            DateTime menorApresentacao = oCarrinho.DataMaisProxima(clienteID, sessionID);

            strSql = "sp_GetTaxasEntrega1 " + clienteID + ", '" + sessionID + "', " + (entregaGratuita ? 1 : 0) + ", '" + menorApresentacao.ToString("yyyyMMdd") + "', '" + DateTime.Now.ToString("yyyyMMdd") + "','" + estado.ToUpper() + "'";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql, CommandType.StoredProcedure))
                {
                    if (!dr.Read())
                        throw new ApplicationException("Taxa Inexistente");

                    if ((int)dr["IR_ID"] != taxaID)
                        throw new ApplicationException("Taxa Inexistente");

                    this.ID = Convert.ToInt32(dr["ID"]);
                    this.IR_ID = Convert.ToInt32(dr["IR_ID"]);
                    this.Valor = Convert.ToDecimal(dr["Valor"]);
                    this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                    this.PrazoEntrega = Convert.ToInt32(dr["PrazoEntrega"]);
                    this.PermitirImpressaoInternet = Convert.ToBoolean(dr["PermitirImpressaoInternet"]);
                }

                return this;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}