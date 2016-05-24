

using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class CorreioStatusEntrega : CorreioStatusEntrega_B
    {

        public CorreioStatusEntrega() { }

        public CorreioStatusEntrega(int usuarioIDLogado) : base(usuarioIDLogado) { }

        
        public List<CorreioStatusEntrega> VerificarEntregasPendentes()
        {
            List<CorreioStatusEntrega> CodigosRastreio = new List<CorreioStatusEntrega>();

            try
            {
                bd.Consulta(@"SELECT * 
                        FROM tCorreioStatusEntrega
                        WHERE CodigoRastreio NOT IN 
                        	(SELECT CodigoRastreio
                        	FROM tCorreioStatusEntrega
                        	WHERE DESCRICAO = 'Entregue')");

                while (bd.Consulta().Read())
                {
                    CorreioStatusEntrega oCorreioStatusEntrega = new CorreioStatusEntrega();

                    oCorreioStatusEntrega.Tipo.Valor = bd.LerString("Tipo");
                    oCorreioStatusEntrega.Status.Valor = bd.LerInt("Status");
                    oCorreioStatusEntrega.Data.Valor = bd.LerDateTime("Data");
                    oCorreioStatusEntrega.Hora.Valor = bd.LerString("Hora");
                    oCorreioStatusEntrega.Descricao.Valor = bd.LerString("Descricao");
                    oCorreioStatusEntrega.Local.Valor = bd.LerString("Local");
                    oCorreioStatusEntrega.Codigo.Valor = bd.LerString("Codigo");
                    oCorreioStatusEntrega.Cidade.Valor = bd.LerString("Cidade");
                    oCorreioStatusEntrega.Uf.Valor = bd.LerString("Uf");
                    oCorreioStatusEntrega.Sto.Valor = bd.LerString("Sto");
                    oCorreioStatusEntrega.CodigoRastreio.Valor = bd.LerString("CodigoRastreio");

                    CodigosRastreio.Add(oCorreioStatusEntrega);
                }
            }
            finally
            {
                bd.Consulta().Close();
            }

            return CodigosRastreio;
        }
        


    }

    public class CorreioStatusEntregaLista : CorreioStatusEntregaLista_B
    {

        public CorreioStatusEntregaLista() { }

        public CorreioStatusEntregaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }




    [System.Xml.Serialization.XmlRoot(ElementName = "evento")]
    public class RetornoCorreioStatusEntrega
    {
        public string tipo { get; set; }
        public string status { get; set; }
        public string data { get; set; }
        public string hora { get; set; }
        public string descricao { get; set; }
        public string local { get; set; }
        public string codigo { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string sto { get; set; }
    }



}


