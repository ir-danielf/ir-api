/**************************************************
* Arquivo: PontoVendaFormaPgto.cs
* Gerado: 27/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib
{

    public class PontoVendaXFormaPgto : PontoVendaXFormaPgto_B
    {

        public PontoVendaXFormaPgto() { }

        public PontoVendaXFormaPgto(int usuarioIDLogado) : base(usuarioIDLogado) { }

        
    }

    public class PontoVendaXFormaPgtoLista : PontoVendaXFormaPgtoLista_B
    {

        public PontoVendaXFormaPgtoLista() { }

        public PontoVendaXFormaPgtoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<PontoVendaXFormaPgto> CarregarPVFormasPgto(int idPV)
        {
            List<PontoVendaXFormaPgto> oPVFormasPgtoLista = new List<PontoVendaXFormaPgto>();
            try
            {
                string sql = "SELECT * FROM tPontoVendaXFormaPgto WHERE PontoVendaID = " + idPV;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVendaXFormaPgto oPVFormasPgto = new PontoVendaXFormaPgto();
                    oPVFormasPgto.Control.ID = bd.LerInt("ID");
                    oPVFormasPgto.PontoVendaID.Valor = bd.LerInt("PontoVendaID");
                    oPVFormasPgto.PontoVendaFormaPgtoID.Valor = bd.LerInt("PontoVendaFormaPgtoID");

                    oPVFormasPgtoLista.Add(oPVFormasPgto);
                }
                bd.Fechar();
                return oPVFormasPgtoLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
