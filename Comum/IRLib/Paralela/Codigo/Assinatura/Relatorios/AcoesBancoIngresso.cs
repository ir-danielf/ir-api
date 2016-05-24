using CTLib;
using IRLib.Paralela.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class AcoesBancoIngresso
    {

        BD bd = new BD();

        public IEnumerable<RelatorioBancoIngresso> BuscarAcoes(string Doador, int Resgatado, string ApresentacaoInicio, string DoacaoInicio, string ResgateInicio, string Resgate, string ApresentacaoFim, string DoacaoFim, string ResgateFim)
        {
            try
            {
                List<RelatorioBancoIngresso> lista = new List<RelatorioBancoIngresso>();
                RelatorioBancoIngresso ultimo = new RelatorioBancoIngresso();

                string filtro = this.MontarFiltro(Doador, Resgate, ApresentacaoInicio, ApresentacaoFim);

                bd.Consulta(string.Format(@"
                                SELECT * FROM vwAcoesBancoingresso 
                                {0}
                                ORDER BY Horario ASC, Setor, Codigo, ComprovanteID ASC", filtro.Length > 0 ? "WHERE " + filtro : string.Empty));

                if (!bd.Consulta().Read())
                    throw new Exception("Nenhum resgistro encontrado, tente redefinir o filtro.");


                do
                {
                    if (ultimo.IngressoID == 0 || ultimo.IngressoID != bd.LerInt("IngressoID"))
                    {
                        if (bd.LerString("Acao") == "R")
                        {
                            ultimo = new RelatorioBancoIngresso();
                            continue;
                        }

                        if (ultimo.IngressoID > 0)
                            lista.Add(ultimo);

                        ultimo = new RelatorioBancoIngresso()
                        {
                            IngressoID = bd.LerInt("IngressoID"),
                            Assinatura = bd.LerString("Assinatura"),
                            Setor = bd.LerString("Setor"),
                            Lugar = bd.LerString("Codigo"),
                            Doador = bd.LerString("Cliente"),
                            LoginDoador = bd.LerString("Login"),
                            DataDoacao = bd.LerDateTime("Timestamp"),
                            Apresentacao = bd.LerDateTime("Horario")
                        };
                    }
                    else if (ultimo.IngressoID == bd.LerInt("IngressoID"))
                    {
                        if (bd.LerString("Acao") == "D")
                        {
                            ultimo = new RelatorioBancoIngresso();
                            continue;
                        }

                        ultimo.LoginResgate = bd.LerString("Login");
                        ultimo.Resgate = bd.LerString("Cliente");
                        ultimo.DataResgate = bd.LerDateTime("Timestamp");

                        lista.Add(ultimo);

                        ultimo = new RelatorioBancoIngresso();
                    };


                } while (bd.Consulta().Read());

                if (ultimo != null)
                    lista.Add(ultimo);

                DateTime doacaoInicio = DateTime.MinValue;
                if (!string.IsNullOrEmpty(DoacaoInicio) && Utilitario.IsDateTime(DoacaoInicio, "dd/MM/yyyy"))
                    doacaoInicio = DateTime.ParseExact(DoacaoInicio.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime doacaoFim = DateTime.MaxValue;
                if (!string.IsNullOrEmpty(DoacaoFim) && Utilitario.IsDateTime(DoacaoFim, "dd/MM/yyyy"))
                    doacaoFim = DateTime.ParseExact(DoacaoFim.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);

                DateTime resgateIni = DateTime.MinValue;
                if (!string.IsNullOrEmpty(ResgateInicio) && Utilitario.IsDateTime(ResgateInicio, "dd/MM/yyyy"))
                    resgateIni = DateTime.ParseExact(ResgateInicio.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime resgateF = DateTime.MaxValue;
                if (!string.IsNullOrEmpty(ResgateFim) && Utilitario.IsDateTime(ResgateFim, "dd/MM/yyyy"))
                    resgateF = DateTime.ParseExact(ResgateFim.Replace("/", ""), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);

                return lista.Where(c =>
                            (string.IsNullOrEmpty(Doador) || string.Compare(c.Doador, Doador, true) == 0) &&
                             (string.IsNullOrEmpty(Resgate) || string.Compare(c.Resgate, Resgate, true) == 0) &&
                             ((Resgatado == 1 && !string.IsNullOrEmpty(c.Resgate)) || Resgatado == 2) &&
                             (string.IsNullOrEmpty(DoacaoInicio) || c.DataDoacao.Date >= doacaoInicio.Date) &&
                             (string.IsNullOrEmpty(DoacaoFim) || c.DataDoacao.Date < doacaoFim.Date) &&
                             (string.IsNullOrEmpty(ResgateInicio) || c.DataResgate.Date >= resgateIni.Date) &&
                             (string.IsNullOrEmpty(ResgateFim) || c.DataResgate.Date < resgateF.Date)).ToList();
            }
            finally
            {
                bd.Fechar();
            }

        }

        private string MontarFiltro(string Doador, string Resgate, string ApresentacaoInicio, string ApresentacaoFim)
        {
            StringBuilder stb = new StringBuilder();
            string filtro = string.Empty;
            if (!string.IsNullOrEmpty(Doador) && string.IsNullOrEmpty(Resgate))
                stb.AppendFilter("((Acao = 'D' AND Cliente LIKE '%" + Doador.ToSafeString() + "%') OR Acao = 'R') ");

            if (!string.IsNullOrEmpty(Resgate))
                stb.AppendFilter("((Acao = 'R' AND Cliente LIKE '%" + Resgate.ToSafeString() + "%') OR Acao = 'D') ");

            if (!string.IsNullOrEmpty(ApresentacaoInicio))
            {
                if (!Utilitario.IsDateTime(ApresentacaoInicio, "dd/MM/yyyy"))
                    throw new Exception("Preencha o campo Horário Inicial corretamente");

                stb.AppendFilter("Horario > '" + DateTime.ParseExact(ApresentacaoInicio.Replace("/", ""), "ddMMyyyy", null, System.Globalization.DateTimeStyles.None).ToString("yyyyMMdd") + "000000'");
            }

            if (!string.IsNullOrEmpty(ApresentacaoFim))
            {
                if (!Utilitario.IsDateTime(ApresentacaoFim, "dd/MM/yyyy"))
                    throw new Exception("Preencha o campo Horário Final corretamente");

                stb.AppendFilter("Horario < '" + DateTime.ParseExact(ApresentacaoFim.Replace("/", ""), "ddMMyyyy", null, System.Globalization.DateTimeStyles.None).AddDays(1).ToString("yyyyMMdd") + "000000'");
            }

            return stb.ToString();
        }


    }
}
