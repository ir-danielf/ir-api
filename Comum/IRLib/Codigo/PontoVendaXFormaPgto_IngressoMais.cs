/**************************************************
* Arquivo: PontoVendaFormaPgto.cs
* Gerado: 27/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib
{

    public class PontoVendaXFormaPgto_IngressoMais : PontoVendaXFormaPgto_IngressoMais_B
    {

        public PontoVendaXFormaPgto_IngressoMais() { }

        public PontoVendaXFormaPgto_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        
    }

    public class PontoVendaXFormaPgtoLista_IngressoMais : PontoVendaXFormaPgtoLista_IngressoMais_B
    {

        public PontoVendaXFormaPgtoLista_IngressoMais() { }

        public PontoVendaXFormaPgtoLista_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<PontoVendaXFormaPgto_IngressoMais> CarregarPVFormasPgto(int idPV)
        {
            List<PontoVendaXFormaPgto_IngressoMais> oPVFormasPgtoLista = new List<PontoVendaXFormaPgto_IngressoMais>();
            try
            {
                string sql = "SELECT * FROM tPontoVendaXFormaPgto_IngressoMais WHERE PontoVendaID = " + idPV;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVendaXFormaPgto_IngressoMais oPVFormasPgto = new PontoVendaXFormaPgto_IngressoMais();
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
