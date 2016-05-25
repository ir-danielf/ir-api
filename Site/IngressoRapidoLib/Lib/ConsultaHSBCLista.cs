using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    public class ConsultaTransacaoHSBC
    {
        public int _numeroPedido { get; set; }
        public string _dataCompra { get; set; }
        public string _status { get; set; }
        public string _senha { get; set; }
        public string _valorTotal { get; set; }

        //private int _numeroPedido;
        //public int NumeroPedido
        //{
        //    get { return _numeroPedido; }
        //    set { _numeroPedido = value; }
        //}

        //private List<string> _dataCompra;
        //public List<string> DataCompra
        //{
        //    get { return _dataCompra; }
        //    set { _dataCompra = value; }
        //}

        //private List<string> _status;
        //public List<string> Status
        //{
        //    get { return _status; }
        //    set { _status = value; }
        //}

        //private List<int> _senha;
        //public List<int> Senha
        //{
        //    get { return _senha; }
        //    set { _senha = value; }
        //}
        public static List<ConsultaTransacaoHSBC> ConsultarCompras(int ClienteID)
        {
            DAL oDAL = new DAL();
            IDataReader dr = null;
            try
            {
                List<ConsultaTransacaoHSBC> listaConsulta = new List<ConsultaTransacaoHSBC>();
                StringBuilder stbConsulta = new StringBuilder();
                stbConsulta.Append("SELECT NumeroPedido, DataCompra, StatusCompra, isNull(Senha,'') AS Senha, ValorTotal ");
                stbConsulta.Append("FROM HSBC WHERE ClienteID=" + ClienteID + " ORDER BY DataCompra Desc");

                dr = oDAL.SelectToIDataReader(stbConsulta.ToString());

                while (dr.Read())
                {
                    ConsultaTransacaoHSBC oConsulta = new ConsultaTransacaoHSBC();
                    oConsulta._numeroPedido = Convert.ToInt32(dr["NumeroPedido"].ToString());
                    oConsulta._dataCompra = dr["DataCompra"].ToString();
                    oConsulta._status = dr["StatusCompra"].ToString();
                    oConsulta._senha = (dr["Senha"].ToString());
                    oConsulta._valorTotal = dr["ValorTotal"].ToString();

                    listaConsulta.Add(oConsulta);
                    

                }
                return listaConsulta;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
