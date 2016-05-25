/**************************************************
* Arquivo: Horario.cs
* Gerado: 28/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class PontoVendaHorario_IngressoMais : PontoVendaHorario_IngressoMais_B
    {

        public PontoVendaHorario_IngressoMais() { }

        public PontoVendaHorario_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        
    }

    public class PontoVendaHorarioLista_IngressoMais : PontoVendaHorarioLista_IngressoMais_B
    {

        public PontoVendaHorarioLista_IngressoMais() { }

        public PontoVendaHorarioLista_IngressoMais(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<PontoVendaHorario_IngressoMais> CarregarHorarioPorPV(int id)
        {
            List<PontoVendaHorario_IngressoMais> oHorarioLista = new List<PontoVendaHorario_IngressoMais>();
            try
            {
                string sql = "SELECT * FROM tPontoVendaHorario_IngressoMais WHERE PontoVendaID = " + id + " ORDER BY DiaSemana";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVendaHorario_IngressoMais oHorario = new PontoVendaHorario_IngressoMais();
                    oHorario.Control.ID = bd.LerInt("ID");
                    oHorario.PontoVendaID.Valor = bd.LerInt("PontoVendaID");
                    oHorario.HorarioInicial.Valor = bd.LerString("HorarioInicial");
                    oHorario.HorarioFinal.Valor = bd.LerString("HorarioFinal");
                    oHorario.DiaSemana.Valor = bd.LerInt("DiaSemana");

                    oHorarioLista.Add(oHorario);
                }
                bd.Fechar();
                return oHorarioLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
