using CTLib;
using Google.Api.Maps.Service;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IRCore.Util;

namespace IngressoRapido.Lib
{
    public class PontoVenda
    {
        public PontoVenda()
        {
            oDiasExtenso = new List<string>();
        }

        public PontoVenda(int id)
        {
            this.id = id;
        }

        private DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private string local;
        public string Local
        {
            get { return this.local.ToUpper(); }
            set { local = value; }
        }

        private string nome;
        public string Nome
        {
            get { return Util.ToTitleCase(this.nome); }
            set { nome = value; }
        }

        private string endereco;
        public string Endereco
        {
            get { return this.endereco; }
            set { endereco = value; }
        }

        private string numero;
        public string Numero
        {
            get { return this.numero; }
            set { numero = value; }
        }

        private string compl;
        public string Compl
        {
            get { return this.compl; }
            set { compl = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return this.cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return this.estado.ToUpper(); }
            set { estado = value; }
        }

        private string bairro;
        public string Bairro
        {
            get { return Util.ToTitleCase(this.bairro); }
            set { bairro = value; }
        }

        private string referencia;
        public string Referencia
        {
            get { return Util.ToTitleCase(this.referencia); }
            set { referencia = value; }
        }

        private string cep;
        public string CEP
        {
            get { return this.cep; }
            set { cep = value; }
        }

        private string uf;
        public string UF
        {
            get { return this.uf.ToUpper(); }
            set { uf = value; }
        }

        private string info;
        public string Info
        {
            get { return this.info; }
            set { info = value; }
        }

        private string obs;
        public string Obs
        {
            get { return this.obs; }
            set { obs = value; }
        }

        private string horario;
        public string Horario
        {
            get { return this.horario; }
            set { horario = value; }
        }


        private string comochegar;
        public string ComoChegar
        {
            get { return this.comochegar; }
            set { comochegar = value; }
        }

        List<string> oDiasExtenso;

        private string latitude;
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private string longitude;
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public string MontarStringInfoPDV(string strFormaPgto)
        {
            string strPVInfo = "";

            string strDiaExtenso = "";

            foreach (string dia in oDiasExtenso)
            {
                if (strDiaExtenso == "")
                    strDiaExtenso = dia;
                else
                    strDiaExtenso += "<br>" + dia + ".";
            }

            if (strDiaExtenso.Trim() != "" && strFormaPgto.Trim() != "")
                strPVInfo = "<i>Horário de Atendimento:</i><br>" + strDiaExtenso + "<br><br>" + "<i>Formas de Pagamento:</i><br>" + strFormaPgto;
            if (strDiaExtenso.Trim() == "" && strFormaPgto.Trim() != "")
                strPVInfo = "<i>Formas de Pagamento:</i><br>" + strFormaPgto;
            if (strDiaExtenso.Trim() != "" && strFormaPgto.Trim() == "")
                strPVInfo = "<i>Horário de Atendimento:</i><br>" + strDiaExtenso;

            return strPVInfo;
        }

        public string MontarStringHorarioPDV()
        {
            string strPVInfo = "";
            string strDiaExtenso = "";

            foreach (string dia in oDiasExtenso)
            {
                if (strDiaExtenso == "")
                    strDiaExtenso = dia;
                else
                    strDiaExtenso += "," + dia + ".";
            }

            if (strDiaExtenso.Trim() != "")
                strPVInfo = strDiaExtenso;

            return strPVInfo;
        }

        public string MontarStringPagamentoPDV(string strFormaPgto)
        {
            string strPVInfo = "";

            string strDiaExtenso = "";

            if (strDiaExtenso.Trim() == "" && strFormaPgto.Trim() != "")
                strPVInfo = strFormaPgto;

            return strPVInfo;
        }

        public List<PontoVenda> Lista(string estado, string cidade)
        {
            PontoVendaHorarioLista oHorarioLista;
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            List<PontoVenda> retorno = new List<PontoVenda>();

            List<int> EventosID = new CarrinhoLista().CarregarEventosReservados(
                System.Web.HttpContext.Current.Session["ClienteID"].ToInt32(), System.Web.HttpContext.Current.Session.SessionID);


            foreach (DataRow item in pDV.CarregarTabelaPDV(EventosID, estado, cidade).Rows)
            {
                oHorarioLista = new PontoVendaHorarioLista();
                oDiasExtenso = new List<string>();

                oHorarioLista.CarregarHorarioPorPDV(item["ID"].ToInt32());
                CarregarHorario(oHorarioLista);

                retorno.Add(new PontoVenda
                {
                    id = item["ID"].ToInt32(),
                    local = item["Local"].ToString(),
                    nome = item["Nome"].ToString(),
                    endereco = item["Endereco"].ToString(),
                    numero = item["Numero"].ToString(),
                    compl = item["Compl"].ToString(),
                    cidade = item["Cidade"].ToString(),
                    estado = item["Estado"].ToString(),
                    bairro = item["Bairro"].ToString(),
                    horario = MontarStringInfoPDV(),
                    uf = estado,
                    referencia = item["Referencia"].ToString(),
                    cep = item["CEP"].ToString(),
                });
            }

            return retorno;
        }

        public List<PontoVenda> Lista(string estado, string cidade, string EventosID)
        {
            PontoVendaHorarioLista oHorarioLista;
            PontoVendaFormaPgtoLista oFormaPgtoLista;
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            List<PontoVenda> retorno = new List<PontoVenda>();

            string strFormaPgto = "";

            foreach (DataRow item in pDV.CarregarTabelaPDV(EventosID, estado, cidade).Rows)
            {
                oHorarioLista = new PontoVendaHorarioLista();
                oFormaPgtoLista = new PontoVendaFormaPgtoLista();
                oDiasExtenso = new List<string>();

                oHorarioLista.CarregarHorarioPorPDV(item["ID"].ToInt32());
                CarregarHorario(oHorarioLista);
                oFormaPgtoLista.CarregarPDVFormaPgto(item["ID"].ToInt32());

                for (int i = 0; i < oFormaPgtoLista.Count; i++)
                {
                    if (i == 0)
                        strFormaPgto = oFormaPgtoLista[i].Nome;
                    else if (i == oFormaPgtoLista.Count - 1)
                        strFormaPgto += " e " + oFormaPgtoLista[i].Nome + ".";
                    else
                        strFormaPgto += ", " + oFormaPgtoLista[i].Nome;
                }

                retorno.Add(new PontoVenda
                {
                    id = item["ID"].ToInt32(),
                    local = item["Local"].ToString(),
                    nome = item["Nome"].ToString(),
                    endereco = item["Endereco"].ToString(),
                    numero = item["Numero"].ToString(),
                    compl = item["Compl"].ToString(),
                    cidade = item["Cidade"].ToString(),
                    estado = item["Estado"].ToString(),
                    bairro = item["Bairro"].ToString(),
                    horario = MontarStringInfoPDV(strFormaPgto),
                    uf = estado,
                    referencia = item["Referencia"].ToString(),
                    cep = item["CEP"].ToString(),
                });
            }

            return retorno;
        }

        public List<PontoVenda> Lista(string estado, string cidade, bool Geral)
        {
            string strEstado = estado;
            string strFormaPgto = "";
            string strCidade = cidade;

            int intCont = 0;


            PontoVendaLista oPontoVendaLista = new PontoVendaLista();
            PontoVendaHorarioLista oHorarioLista;
            PontoVendaFormaPgtoLista oFormaPgtoLista;

            oPontoVendaLista.CarregarPontoVendaLista(Util.StringToBD(strEstado), Util.StringToBD(strCidade));

            foreach (PontoVenda pv in oPontoVendaLista)
            {
                oHorarioLista = new PontoVendaHorarioLista();
                oFormaPgtoLista = new PontoVendaFormaPgtoLista();

                oDiasExtenso = new List<string>();
                oHorarioLista.CarregarHorarioPorPDV(pv.ID);
                oFormaPgtoLista.CarregarPDVFormaPgto(pv.ID);

                for (int i = 0; i < oFormaPgtoLista.Count; i++)
                {
                    if (i == 0)
                        strFormaPgto = oFormaPgtoLista[i].Nome;
                    else if (i == oFormaPgtoLista.Count - 1)
                        strFormaPgto += " e " + oFormaPgtoLista[i].Nome + ".";
                    else
                        strFormaPgto += ", " + oFormaPgtoLista[i].Nome;
                }

                oPontoVendaLista[intCont].uf = strEstado;

                CarregarHorario(oHorarioLista);
                if (oPontoVendaLista[intCont].Obs.Trim() != "")
                    oPontoVendaLista[intCont].Obs = "<i>Observação:</i><br>" + oPontoVendaLista[intCont].Obs;
                oPontoVendaLista[intCont].Info = MontarStringInfoPDV(strFormaPgto);

                if (oPontoVendaLista[intCont].Numero != null && oPontoVendaLista[intCont].Numero != "" && oPontoVendaLista[intCont].Numero != "s/n")
                {
                    oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                }
                else if (oPontoVendaLista[intCont].Numero == "s/n")
                {
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                    oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                }
                else
                {
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                }

                if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP) && oPontoVendaLista[intCont].CEP != "-")
                    oPontoVendaLista[intCont].CEP = "<br>Cep: " + oPontoVendaLista[intCont].CEP;
                else
                    oPontoVendaLista[intCont].CEP = "";

                if (oPontoVendaLista[intCont].Compl != null && oPontoVendaLista[intCont].Compl != "")
                    oPontoVendaLista[intCont].Endereco += " " + oPontoVendaLista[intCont].Compl;
                if (oPontoVendaLista[intCont].Bairro != null && oPontoVendaLista[intCont].Bairro != "")
                    oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Bairro;
                if (oPontoVendaLista[intCont].Referencia != null && oPontoVendaLista[intCont].Referencia != "")
                    oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Referencia;

                strFormaPgto = "";

                intCont++;
            }



            return oPontoVendaLista;
        }

        public string buscarNome(int pdvID)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();

            return pDV.buscarNome(pdvID);
        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int estadoID)
        {
            List<int> EventosID = new CarrinhoLista().CarregarEventosReservados(
                System.Web.HttpContext.Current.Session["ClienteID"].ToInt32(), System.Web.HttpContext.Current.Session.SessionID);

            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.TabelaPorEstadocomSiglaComPDV(estadoID, EventosID);
        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int EstadoID, bool geral)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.TabelaPorEstadocomSiglaComPDV(EstadoID, geral);
        }

        public DataTable TabelaPorEstadocomSiglaComPDV(int EstadoID, string EventosID)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.TabelaPorEstadocomSiglaComPDV(EstadoID, EventosID);
        }

        public DataTable EstadosComPDV()
        {
            List<int> EventosID = new CarrinhoLista().CarregarEventosReservados(
                System.Web.HttpContext.Current.Session["ClienteID"].ToInt32(), System.Web.HttpContext.Current.Session.SessionID);


            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.EstadosComPDV(EventosID);
        }

        public DataTable EstadosComPDV(bool geral)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.EstadosComPDV(geral);
        }

        public DataTable EstadosComPDV(string EventosID)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            return pDV.EstadosComPDV(EventosID);
        }

        public void CarregarHorario(PontoVendaHorarioLista objHorarioLista)
        {
            string strDiasSemana = "";

            string horaInicial = "";
            string horaFinal = "";

            int iCont = 0;

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
                        MontarStringHorario(strDiasSemana, horaInicial, horaFinal);
                        strDiasSemana = objHorario.DiaSemana.ToString();
                    }
                }
                horaInicial = objHorario.HorarioInicial;
                horaFinal = objHorario.HorarioFinal;

                iCont++;
            }
            if (objHorarioLista.Count > 0)
                MontarStringHorario(strDiasSemana, horaInicial, horaFinal);
        }

        public void MontarStringHorario(string strDiasSemana, string horaInicial, string horaFinal)
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

            oDiasExtenso.Add(strDiasExtenso);
        }

        public string CarregarDiasAlternados(string[] strDias)
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

        public string DiaSemana(string Dia)
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

        public string MontarStringInfoPDV()
        {
            string strPVInfo = "";
            string strDiaExtenso = "";

            foreach (string dia in oDiasExtenso)
            {
                if (strDiaExtenso == "")
                    strDiaExtenso = dia;
                else
                    strDiaExtenso += "<br>" + dia;
            }

            if (strDiaExtenso.Trim() != "")
            {
                strDiaExtenso += ".";
                strPVInfo = strDiaExtenso;
            }

            return strPVInfo;
        }

        public void Carregar(int pdvID)
        {
            IRLib.PontoVenda pDV = new IRLib.PontoVenda();
            pDV.CarregarPDV("ID = " + pdvID);

            this.Nome = pDV.Nome.Valor;
            this.Endereco = pDV.Endereco.Valor;
            this.Numero = pDV.Numero.Valor;
            this.Compl = pDV.Compl.Valor;
        }

        private List<EstruturaPontoDeVenda> Lista()
        {

            string strFormaPgto = "";

            int intCont = 0;

            List<EstruturaPontoDeVenda> lstEstruturaPdv = new List<EstruturaPontoDeVenda>();
            PontoVendaLista oPontoVendaLista = new PontoVendaLista();
            PontoVendaHorarioLista oHorarioLista;
            PontoVendaFormaPgtoLista oFormaPgtoLista;

            oPontoVendaLista.CarregarPontoVendaLista();

            foreach (PontoVenda pv in oPontoVendaLista)
            {
                oHorarioLista = new PontoVendaHorarioLista();
                oFormaPgtoLista = new PontoVendaFormaPgtoLista();

                oDiasExtenso = new List<string>();
                oHorarioLista.CarregarHorarioPorPDV(pv.ID);
                oFormaPgtoLista.CarregarPDVFormaPgto(pv.ID);

                for (int i = 0; i < oFormaPgtoLista.Count; i++)
                {
                    if (i == 0)
                        strFormaPgto = oFormaPgtoLista[i].Nome;
                    else if (i == oFormaPgtoLista.Count - 1)
                        strFormaPgto += " e " + oFormaPgtoLista[i].Nome + ".";
                    else
                        strFormaPgto += ", " + oFormaPgtoLista[i].Nome;
                }

                oPontoVendaLista[intCont].uf = oPontoVendaLista[intCont].Estado;

                CarregarHorario(oHorarioLista);
                if (oPontoVendaLista[intCont].Obs.Trim() != "")
                    oPontoVendaLista[intCont].Obs = "<i>Observação:</i><br>" + oPontoVendaLista[intCont].Obs;
                oPontoVendaLista[intCont].Info = MontarStringInfoPDV(strFormaPgto);

                if (oPontoVendaLista[intCont].Numero != null && oPontoVendaLista[intCont].Numero != "" && oPontoVendaLista[intCont].Numero != "s/n")
                {
                    oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                }
                else if (oPontoVendaLista[intCont].Numero == "s/n")
                {
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                    oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                }
                else
                {
                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                }

                if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP) && oPontoVendaLista[intCont].CEP != "-")
                    oPontoVendaLista[intCont].CEP = "<br>Cep: " + oPontoVendaLista[intCont].CEP;
                else
                    oPontoVendaLista[intCont].CEP = "";


                if (oPontoVendaLista[intCont].Compl != null && oPontoVendaLista[intCont].Compl != "")
                    oPontoVendaLista[intCont].Endereco += " " + oPontoVendaLista[intCont].Compl;
                if (oPontoVendaLista[intCont].Bairro != null && oPontoVendaLista[intCont].Bairro != "")
                    oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Bairro;
                if (oPontoVendaLista[intCont].Referencia != null && oPontoVendaLista[intCont].Referencia != "")
                    oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Referencia;


                string numero = (oPontoVendaLista[intCont].Numero.Length > 0) ? oPontoVendaLista[intCont].Numero + " " : "";
                string compl = (oPontoVendaLista[intCont].Compl.Length > 0) ? oPontoVendaLista[intCont].Compl : "";
                string endereco = (oPontoVendaLista[intCont].Endereco.Length > 0) ? oPontoVendaLista[intCont].Endereco : "";
                string horario = MontarStringHorarioPDV();
                string formaPagamento = MontarStringPagamentoPDV(strFormaPgto);

                lstEstruturaPdv.Add(new EstruturaPontoDeVenda
                {
                    ID = pv.ID,
                    Cidade = (oPontoVendaLista[intCont].Cidade.Length > 0) ? oPontoVendaLista[intCont].Cidade : "",
                    Nome = (oPontoVendaLista[intCont].Nome.Length > 0) ? oPontoVendaLista[intCont].Nome : "",
                    Endereco = endereco,
                    Horario = horario,
                    FormaPagamento = formaPagamento,
                    Estado = oPontoVendaLista[intCont].Estado.Length > 0 ? oPontoVendaLista[intCont].Estado : string.Empty,
                    CEP = pv.CEP,
                    Latitude = pv.Latitude,
                    Longitude = pv.Longitude,
                });

                strFormaPgto = "";
                intCont++;
            }

            return lstEstruturaPdv;
        }

        public List<EstruturaPontoDeVenda> CarregarPontosVenda()
        {
            try
            {
                List<EstruturaPontoDeVenda> ListaPDV = new List<EstruturaPontoDeVenda>();

                ListaPDV = this.Lista();

                return ListaPDV;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EstruturaPontoDeVenda> BuscaTodosEventosPorCoordenada(string Latitude, string Longitude, int distancia)
        {
            try
            {
                string strFormaPgto = "";

                int intCont = 0;

                List<EstruturaPontoDeVenda> lstEstruturaPdv = new List<EstruturaPontoDeVenda>();
                PontoVendaLista oPontoVendaLista = new PontoVendaLista();
                PontoVendaHorarioLista oHorarioLista;
                PontoVendaFormaPgtoLista oFormaPgtoLista;

                bool calcularDistancia = !string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude);

                oPontoVendaLista.CarregarPontoVendaListaComCoordenadas();

                foreach (PontoVenda pv in oPontoVendaLista)
                {
                    oHorarioLista = new PontoVendaHorarioLista();
                    oFormaPgtoLista = new PontoVendaFormaPgtoLista();

                    oDiasExtenso = new List<string>();
                    oHorarioLista.CarregarHorarioPorPDV(pv.ID);
                    oFormaPgtoLista.CarregarPDVFormaPgto(pv.ID);

                    for (int i = 0; i < oFormaPgtoLista.Count; i++)
                    {
                        if (i == 0)
                            strFormaPgto = oFormaPgtoLista[i].Nome;
                        else if (i == oFormaPgtoLista.Count - 1)
                            strFormaPgto += " e " + oFormaPgtoLista[i].Nome + ".";
                        else
                            strFormaPgto += ", " + oFormaPgtoLista[i].Nome;
                    }

                    oPontoVendaLista[intCont].uf = oPontoVendaLista[intCont].Estado;

                    CarregarHorario(oHorarioLista);
                    if (oPontoVendaLista[intCont].Obs.Trim() != "")
                        oPontoVendaLista[intCont].Obs = "<i>Observação:</i><br>" + oPontoVendaLista[intCont].Obs;
                    oPontoVendaLista[intCont].Info = MontarStringInfoPDV(strFormaPgto);

                    if (oPontoVendaLista[intCont].Numero != null && oPontoVendaLista[intCont].Numero != "" && oPontoVendaLista[intCont].Numero != "s/n")
                    {
                        oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                        if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                        else
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                    }
                    else if (oPontoVendaLista[intCont].Numero == "s/n")
                    {
                        if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                        else
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                        oPontoVendaLista[intCont].Endereco += ", " + oPontoVendaLista[intCont].Numero;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP))
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado + " CEP: " + oPontoVendaLista[intCont].CEP;
                        else
                            oPontoVendaLista[intCont].ComoChegar = oPontoVendaLista[intCont].Endereco + "*" + oPontoVendaLista[intCont].Cidade + "/" + oPontoVendaLista[intCont].Estado;
                    }

                    if (!string.IsNullOrEmpty(oPontoVendaLista[intCont].CEP) && oPontoVendaLista[intCont].CEP != "-")
                        oPontoVendaLista[intCont].CEP = "<br>Cep: " + oPontoVendaLista[intCont].CEP;
                    else
                        oPontoVendaLista[intCont].CEP = "";

                    if (oPontoVendaLista[intCont].Compl != null && oPontoVendaLista[intCont].Compl != "")
                        oPontoVendaLista[intCont].Endereco += " " + oPontoVendaLista[intCont].Compl;
                    if (oPontoVendaLista[intCont].Bairro != null && oPontoVendaLista[intCont].Bairro != "")
                        oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Bairro;
                    if (oPontoVendaLista[intCont].Referencia != null && oPontoVendaLista[intCont].Referencia != "")
                        oPontoVendaLista[intCont].Endereco += " - " + oPontoVendaLista[intCont].Referencia;

                    string numero = (oPontoVendaLista[intCont].Numero.Length > 0) ? oPontoVendaLista[intCont].Numero + " " : "";
                    string compl = (oPontoVendaLista[intCont].Compl.Length > 0) ? oPontoVendaLista[intCont].Compl : "";
                    string endereco = (oPontoVendaLista[intCont].Endereco.Length > 0) ? oPontoVendaLista[intCont].Endereco : "";
                    string horario = MontarStringHorarioPDV();
                    string formaPagamento = MontarStringPagamentoPDV(strFormaPgto);

                    int distanciaEvento = 0;

                    if (calcularDistancia)
                        distanciaEvento = IRLib.CEP.CalcularDistancia(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude), Convert.ToDouble(oPontoVendaLista[intCont].Latitude), Convert.ToDouble(oPontoVendaLista[intCont].Longitude));

                    lstEstruturaPdv.Add(new EstruturaPontoDeVenda
                    {
                        ID = pv.ID,
                        Cidade = (oPontoVendaLista[intCont].Cidade.Length > 0) ? oPontoVendaLista[intCont].Cidade : "",
                        Nome = (oPontoVendaLista[intCont].Nome.Length > 0) ? oPontoVendaLista[intCont].Nome : "",
                        Endereco = endereco,
                        Horario = horario,
                        FormaPagamento = formaPagamento,
                        Estado = oPontoVendaLista[intCont].Estado.Length > 0 ? oPontoVendaLista[intCont].Estado : string.Empty,
                        CEP = pv.CEP,
                        Latitude = oPontoVendaLista[intCont].Latitude,
                        Longitude = oPontoVendaLista[intCont].Longitude,
                        Distancia = distanciaEvento
                    });

                    strFormaPgto = "";
                    intCont++;
                }
                if (calcularDistancia)
                    lstEstruturaPdv = lstEstruturaPdv.Where(c => c.Distancia <= distancia).OrderBy(c => c.Distancia).ToList();

                return lstEstruturaPdv;
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

        private List<EstruturaPontoDeVenda> BuscarTodos()
        {
            var strSql =
@"SELECT Id,
       CEP,
       Endereco,
       Cidade,
       Estado
FROM INGRESSOSNOVO..tPontoVenda(NOLOCK)
WHERE(Latitude IS NULL
      OR Longitude IS NULL);";

            var retorno = new List<EstruturaPontoDeVenda>();

            try
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos.BuscandoPontosDeVenda##"));

                oDAL = new DAL();

                using (var dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(new EstruturaPontoDeVenda
                        {
                            ID = dr["Id"].ToInt32(),
                            CEP = dr["CEP"].ToString(),
                            Endereco = dr["Endereco"].ToString(),
                            Cidade = dr["Cidade"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }

                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos.BuscandoPontosDeVenda.SUCCESS##"));

                return retorno;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                LogUtil.Error(string.Format("##RoboAtzSite.BuscarTodos.BuscandoPontosDeVenda.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        private List<EstruturaPontoDeVenda> BuscarTodos_SiteIR()
        {
            var strSql = @"SELECT IR_PontoVendaID, CEP, Endereco, Cidade, Estado FROM PontoVenda WHERE ( Latitude IS NULL OR Longitude IS NULL ) ";

            var retorno = new List<EstruturaPontoDeVenda>();

            try
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos_SiteIR.BuscandoPontosDeVenda##"));

                oDAL = new DAL();

                using (var dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(new EstruturaPontoDeVenda()
                        {
                            ID = dr["IR_PontoVendaID"].ToInt32(),
                            CEP = dr["CEP"].ToString(),
                            Endereco = dr["Endereco"].ToString(),
                            Cidade = dr["Cidade"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }

                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos_SiteIR.BuscandoPontosDeVenda.SUCCESS##"));

                return retorno;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                LogUtil.Error(string.Format("##RoboAtzSite.BuscarTodos_SiteIR.BuscandoPontosDeVenda.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void AtualizarCoordenadas()
        {
            var retorno = this.BuscarTodos();

            try
            {
                foreach (var item in retorno)
                {
                    LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda## NOME: {0}", item.Nome));

                    try
                    {
                        var coordenadas = IRLib.CEP.BuscarCoordenadas(item.CEP.ToCEP(), item.Endereco, item.Cidade, item.Estado);
                        LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda.RetornoBuscaCoordenadas## ITEMID: {0}, NOME: {1}, LATITUDE: {2}, LONGITUDE: {3}", item.ID, item.Nome, coordenadas.Latitude, coordenadas.Longitude));

                        var strSql = string.Format(@"UPDATE INGRESSOSNOVO..tPontoVenda SET Latitude = '{0}', Longitude = '{1}' WHERE Id = {2};", coordenadas.Latitude, coordenadas.Longitude, item.ID);
                        oDAL.Execute(strSql);

                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda.EXCEPTION## MSG: {0}", ex.Message), ex);
                    }

                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void AtualizarCoordenadas_SiteIR()
        {
            var retorno = this.BuscarTodos_SiteIR();

            try
            {
                foreach (var item in retorno)
                {
                    LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda_SiteIR## NOME: {0}", item.Nome));

                    try
                    {
                        var coordenadas = IRLib.CEP.BuscarCoordenadas(item.CEP.ToCEP(), item.Endereco, item.Cidade, item.Estado);
                        LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda_SiteIR.RetornoBuscaCoordenadas## ITEMID: {0}, NOME: {1}, LATITUDE: {2}, LONGITUDE: {3}", item.ID, item.Nome, coordenadas.Latitude, coordenadas.Longitude));

                        var strSql = string.Format(@"UPDATE PontoVenda SET Latitude = '{0}', Longitude = '{1}' WHERE IR_PontoVendaID = {2}", coordenadas.Latitude, coordenadas.Longitude, item.ID);
                        oDAL.Execute(strSql);

                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda_SiteIR.EXCEPTION## MSG: {0}", ex.Message), ex);
                    }

                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasPontosDeVenda_SiteIR.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}
