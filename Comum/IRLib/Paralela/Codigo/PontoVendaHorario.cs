/**************************************************
* Arquivo: Horario.cs
* Gerado: 28/11/2007
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class PontoVendaHorario : PontoVendaHorario_B
    {

        public PontoVendaHorario() { }

        public PontoVendaHorario(int usuarioIDLogado) : base(usuarioIDLogado) { }

        
    }

    public class PontoVendaHorarioLista : PontoVendaHorarioLista_B
    {

        public PontoVendaHorarioLista() { }

        public PontoVendaHorarioLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<PontoVendaHorario> CarregarHorarioPorPV(int id)
        {
            List<PontoVendaHorario> oHorarioLista = new List<PontoVendaHorario>();
            try
            {
                string sql = "SELECT * FROM tPontoVendaHorario WHERE PontoVendaID = " + id + " ORDER BY DiaSemana";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    PontoVendaHorario oHorario = new PontoVendaHorario();
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
