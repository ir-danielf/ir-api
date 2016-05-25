using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for ApresentacaoLista
    /// </summary>
    public class ApresentacaoLista : List<Apresentacao>
    {
        private DAL oDAL = new DAL();
        Apresentacao oApresentacao;

        public ApresentacaoLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Apresentações do tipo Apresentação, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private ApresentacaoLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                strSql = "SELECT IR_ApresentacaoID, Horario, EventoID " +
                            "FROM Apresentacao (NOLOCK) " +
                            "WHERE " + clausula + " AND (Horario > '" + DateTime.Now.AddHours(2).ToString("yyyyMMddHHmm") + "') ORDER BY Horario";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oApresentacao = new Apresentacao(Convert.ToInt32(dr["IR_ApresentacaoID"].ToString()));
                        oApresentacao.Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                        oApresentacao.EventoID = Convert.ToInt32(dr["EventoID"].ToString());

                        this.Add(oApresentacao);
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

        /// <summary>
        /// Encontra as apresentacoes que possuem forma de entrega
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        public ApresentacaoLista CarregarListaComTaxaEntrega(int eventoID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("exec sp_getApresentacoes ");
                stbSQL.Append(eventoID + ", ");
                stbSQL.Append("'" + DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["RemoverApresentacaoHoras"])).ToString("yyyyMMddHHmm") + "'");


                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    while (dr.Read())
                    {
                        this.Add(new Apresentacao(Convert.ToInt32(dr["IR_ApresentacaoID"].ToString()))
                        {
                            Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                            EventoID = Convert.ToInt32(dr["EventoID"].ToString())
                        });

                        this.Add(oApresentacao);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApresentacaoLista CarregarListaStringFormatada(int eventoID, string eventosGambiarra)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("exec sp_getApresentacoes2 ");
                stbSQL.Append(eventoID + ", ");
                stbSQL.Append("'" + DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["RemoverApresentacaoHoras"])).ToString("yyyyMMddHHmm") + "'");

                List<int> eventos = new List<int>();
                foreach (var evento in eventosGambiarra.Split(';'))
                { if (evento.Length == 0) continue; eventos.Add(evento.ToInt32()); };

                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {

                    while (dr.Read())
                    {
                        this.Add(new Apresentacao(Convert.ToInt32(dr["IR_ApresentacaoID"].ToString()))
                        {
                            HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString(eventos.Contains(eventoID) ? "dddd, dd \\de MMMM \\de yyyy" : "dddd, dd \\de MMMM \\de yyyy à\\s HH:mm"),
                            EventoID = Convert.ToInt32(dr["EventoID"].ToString()),
                            UsarEsquematico = Convert.ToBoolean(dr["UsarEsquematico"])
                        });

                    }

                    if (this.Count > 1)
                        this.Insert(0, new Apresentacao(0)
                        {
                            EventoID = 0,
                            HorarioFormatado = "Selecione..."
                        });
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna uma Lista de Apresentacoes do tipo apresentacao, 
        /// a partir de um EventoID
        /// </summary>
        public ApresentacaoLista CarregarDadosPorEventoID(int id)
        {
            return CarregarLista("(EventoID = " + id + ") ");
        }

        /// <summary>
        /// Metodo Obsoleto
        /// </summary>
        //public ApresentacaoLista CarregarDadosporEventoID(int id)
        //{
        //    string strSql = "SELECT IR_ApresentacaoID " +
        //                    "FROM Apresentacao " +
        //                    "WHERE (IR_EventoID = " + id + ") " +
        //                    "ORDER BY Horario";
        //    try
        //    {
        //        IDataReader dr = oDAL.SelectToIDataReader(strSql);

        //        while (dr.Read())
        //        {
        //            // popular propriedades
        //            oApresentacao = new Apresentacao(Convert.ToInt32(dr["IR_ApresentacaoID"].ToString()));
        //            this.Add(oApresentacao);
        //        }

        //        // Fecha conexão da classe DataAccess
        //        oDAL.ConnClose();
        //        return this;
        //    }
        //    catch (Exception ex)
        //    {
        //        oDAL.ConnClose();
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        oDAL.ConnClose();
        //    }
        //}


    }

}
