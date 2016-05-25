using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IngressoRapido.Lib
{
    class IngressoTransferenciaLista : List<IngressoTransferencia>
    {
        public string SessionID { get; set; }
        public int ClienteID { get; set; }
        private DAL oDAL;
        IngressoTransferencia oTransferencia;
        bool adicionarQuantidade = false;

        public bool VerificarIngressos(bool transferencia, EstruturaReservaInternet estruturaReservaInternet)
        {
            try
            {


                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT Distinct PacoteGrupo, IsNull(IR_PacoteID,0) AS IR_PacoteID, Count(PacoteGrupo) as Quantidade from Carrinho (NOLOCK) ");
                stbSQL.Append("INNER JOIN Pacote ON Carrinho.PacoteNome = Pacote.Nome ");
                stbSQL.Append("WHERE ClienteID = 0 AND SessionID = @SessionID AND Status = 'R' ");
                stbSQL.Append("Group by IR_PacoteID, PacoteGrupo ");

                SqlParameter[] parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter("@SessionID", this.SessionID);

                oDAL = new DAL();

                IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString(), parametro);

                while (dr.Read())
                {
                    adicionarQuantidade = this.Where(c => c.IR_PacoteID == Convert.ToInt32(dr["IR_PacoteID"])).Count() == 0;
                    if (adicionarQuantidade)
                    {
                        oTransferencia = new IngressoTransferencia();
                        oTransferencia.IR_PacoteID = Convert.ToInt32(dr["IR_PacoteID"]);
                        this.Add(oTransferencia);
                    }

                    oTransferencia.Quantidade += Convert.ToInt32(dr["Quantidade"]);
                }
                dr.Close();
                oDAL.ConnClose();


                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT (PrecoID), count(precoID) AS Quantidade from Carrinho (NOLOCK) ");
                stbSQL.Append("WHERE ClienteID = 0 AND SessionID = @SessionID AND Status = 'R' AND PacoteGrupo IS NULL ");
                stbSQL.Append("Group by PrecoID");

                SqlParameter[] parametro2 = new SqlParameter[1];
                parametro2[0] = new SqlParameter("@SessionID", this.SessionID);

                oDAL = new DAL();
                IDataReader dr2 = oDAL.SelectToIDataReader(stbSQL.ToString(), parametro2);

                while (dr2.Read())
                {
                    adicionarQuantidade = this.Where(c => c.PrecoID == Convert.ToInt32(dr2["PrecoID"])).Count() == 0;
                    if (adicionarQuantidade)
                    {
                        oTransferencia = new IngressoTransferencia();
                        oTransferencia.PrecoID = Convert.ToInt32(dr2["PrecoID"]);
                        this.Add(oTransferencia);
                    }
                    oTransferencia.Quantidade += Convert.ToInt32(dr2["Quantidade"]);
                }
                dr2.Close();
                oDAL.ConnClose();
                return IngressosQPodeReservar(transferencia, estruturaReservaInternet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }

        }
        private bool IngressosQPodeReservar(bool transferencia, EstruturaReservaInternet estruturaReservaInternet)
        {
            IRLib.Bilheteria oBilheteria = new IRLib.Bilheteria();
            try
            {
                List<IngressoTransferencia> pacotes = this.Where(c => c.IR_PacoteID > 0).ToList();
                List<IngressoTransferencia> avulsos = this.Where(c => c.IR_PacoteID == 0).ToList();

                var x = from c in pacotes
                        select new { PacoteID = c.IR_PacoteID, Quantidade = pacotes.Count(i => i.IR_PacoteID == c.IR_PacoteID) };

                foreach (var item in x)
                {
                    if (item.Quantidade != oBilheteria.GetPacotesQPodeReservar(item.PacoteID, item.Quantidade, this.SessionID, this.ClienteID, estruturaReservaInternet))
                        throw new Exception("Um dos pacotes não pode ser tranferido, limite atingido");
                }

                var y = from c in avulsos
                        select new { PrecoID = c.PrecoID, Quantidade = c.Quantidade, c.SerieID };
                decimal valorPreco = 0;
                foreach (var item in y)
                {
                    if (item.Quantidade != oBilheteria.GetIngressosQPodeReservar(this.ClienteID, this.SessionID, item.PrecoID,
                        item.Quantidade, ref valorPreco, transferencia, item.SerieID, estruturaReservaInternet))
                        throw new Exception("Um dos Preços não pode ser transferido, limite atingido");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
