using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class PontoVendaBO : MasterBO<PontoVendaADO>
    {
        public PontoVendaBO(MasterADOBase ado = null) : base(ado) { }

        public List<PontoVenda> ListarPdvPermiteRetirada(string cidade, string uf)
        {
            return ado.ListarPdvPermiteRetirada(cidade, uf).Select(Formata).ToList();
        }

        public PontoVenda Formata(PontoVenda pdv)
        {
            var oDiasExtenso = CarregarHorario(pdv.PontoVendaHorario.ToList());

            var strDiaExtenso = "";

            foreach (var dia in oDiasExtenso)
            {
                if (string.IsNullOrEmpty(strDiaExtenso))
                    strDiaExtenso = dia;
                else
                    strDiaExtenso += " <br> " + dia + ".";
            }

            pdv.HorarioFuncionamento = strDiaExtenso;

            pdv.EnderecoCompleto = pdv.Endereco;

            if ((!string.IsNullOrEmpty(pdv.Numero)) && (pdv.Numero != "s/n"))
            {
                pdv.EnderecoCompleto += ", " + pdv.Numero;
            }

            return pdv;
        }

        public List<string> CarregarHorario(List<PontoVendaHorario> objHorarioLista)
        {
            var oDiasExtenso = new List<string>();

            var strDiasSemana = "";

            var horaInicial = "";
            var horaFinal = "";

            var iCont = 0;

            foreach (PontoVendaHorario objHorario in objHorarioLista)
            {
                if (strDiasSemana == "")
                    strDiasSemana = objHorario.DiaSemana.ToString();
                else
                {
                    if (objHorario.HorarioInicial == horaInicial && objHorario.HorarioFinal == horaFinal)
                    {
                        strDiasSemana += "," + objHorario.DiaSemana.ToString();
                    }
                    else
                    {
                        oDiasExtenso.Add(MontarStringHorario(strDiasSemana, horaInicial, horaFinal));
                        strDiasSemana = objHorario.DiaSemana.ToString();
                    }
                }
                horaInicial = objHorario.HorarioInicial;
                horaFinal = objHorario.HorarioFinal;

                iCont++;
            }
            if (objHorarioLista.Count > 0)
                oDiasExtenso.Add(MontarStringHorario(strDiasSemana, horaInicial, horaFinal));

            return oDiasExtenso;
        }

        private string MontarStringHorario(string strDiasSemana, string horaInicial, string horaFinal)
        {
            string[] strDias = strDiasSemana.Split((",").ToCharArray());

            bool blnDias = true;

            string strDiasExtenso = "";
            string PrimeiroDia = "";
            string UltimoDia = "";

            PrimeiroDia = DiaSemana(strDias.GetValue(0).ToString());
            if (strDias.Length > 1)
                UltimoDia = DiaSemana(strDias.GetValue(strDias.Length - 1).ToString());

            if (strDias.Length == 1)
                strDiasExtenso = PrimeiroDia + " das " + horaInicial + " às " + horaFinal;
            if (strDias.Length == 2)
                strDiasExtenso = PrimeiroDia + " e " + UltimoDia + " das " + horaInicial + " às " + horaFinal;
            if (strDias.Length > 2 && strDias.Length != 8)
            {
                for (int i = 1; i < strDias.Length; i++)
                {
                    if (int.Parse(strDias.GetValue(i).ToString()) - 1 != int.Parse(strDias.GetValue(i - 1).ToString()))
                    {
                        blnDias = false;
                        break;
                    }
                }
                if (blnDias)
                    if (UltimoDia != "Feriado")
                        strDiasExtenso = PrimeiroDia + " a " + UltimoDia + " das " + horaInicial + " às " + horaFinal;
                    else
                        strDiasExtenso = CarregarDiasAlternados(strDias) + " das " + horaInicial + " às " + horaFinal;
                else
                    strDiasExtenso = CarregarDiasAlternados(strDias) + " das " + horaInicial + " às " + horaFinal;
            }
            if (strDias.Length == 8)
            {
                if (horaInicial == "00:00" && horaFinal == "00:00")
                    strDiasExtenso = "Atendimento 24 horas.";
                else
                    strDiasExtenso = "Todos os dias das " + horaInicial + " às " + horaFinal;
            }

            return strDiasExtenso;
        }

        private string DiaSemana(string Dia)
        {
            string strDia = "";

            if (Dia == "1") strDia = "Segunda";
            if (Dia == "2") strDia = "Terça";
            if (Dia == "3") strDia = "Quarta";
            if (Dia == "4") strDia = "Quinta";
            if (Dia == "5") strDia = "Sexta";
            if (Dia == "6") strDia = "Sábado";
            if (Dia == "7") strDia = "Domingo";
            if (Dia == "8") strDia = "Feriado";

            return strDia;
        }

        private string CarregarDiasAlternados(string[] strDias)
        {
            string strDiasExtenso = "";
            for (int i = 0; i < strDias.Length; i++)
            {
                if (strDiasExtenso.Trim() == "")
                    strDiasExtenso = DiaSemana(strDias.GetValue(i).ToString());
                else
                {
                    if (i == strDias.Length - 1)
                        strDiasExtenso += " e " + DiaSemana(strDias.GetValue(i).ToString());
                    else
                        strDiasExtenso += ", " + DiaSemana(strDias.GetValue(i).ToString());
                }
            }
            return strDiasExtenso;
        }

        public PontoVenda Consultar(int id)
        {
            return this.Formata(ado.Consultar(id));
        }
    }
}