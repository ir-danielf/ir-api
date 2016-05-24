using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib.Paralela
{
    public class AlertaApresentacao
    {
        private BD bd { get; set; }
        public List<EstruturaAlertaApresentacao> lista { get; set; }
        public AlertaApresentacao()
        {
            bd = new BD();
        }

        public void PesquisarApresentacoes()
        {
            try
            {
                this.lista = new List<EstruturaAlertaApresentacao>();
                DateTime horarioInicial = DateTime.Now;
                DateTime horarioFinal = DateTime.Now.AddHours(48);
                int QuantidadeMinima = Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMinimaAlerta"]);

                bd.Consulta("EXEC Proc_AlertarApresentacoes '"
                    + horarioInicial.ToString("yyyyMMddHHmmss") + "', '"
                    + horarioFinal.ToString("yyyyMMddHHmmss") + "', "
                    + QuantidadeMinima);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaAlertaApresentacao()
                    {
                        Regional = bd.LerString("Regional"),
                        Empresa = bd.LerString("Empresa"),
                        Local = bd.LerString("Local"),
                        Evento = bd.LerString("Evento"),
                        Horario = bd.LerDateTime("Apresentacao"),
                        Quantidade = bd.LerInt("Quantidade"),
                    });
                };
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Enviar()
        {
            try
            {
                EnviarEmailParalela enviarEmail = new EnviarEmailParalela();

                Regional reg = new Regional();
                List<EstruturaRegionalEmail> listaRegional = reg.CarregarListaEmail();

                foreach (EstruturaRegionalEmail regEmail in listaRegional.Where(c => !string.IsNullOrEmpty(c.Email)))
                {
                    if (lista.Count == 0)
                        break;

                    enviarEmail.EnviarAlerta(this.lista.Where(c => c.Horario.Subtract(DateTime.Now).TotalHours <= 24 && c.Regional == regEmail.Regional).ToList(), 1, regEmail.Email, regEmail.Regional);
                    enviarEmail.EnviarAlerta(this.lista.Where(c => c.Horario.Subtract(DateTime.Now).TotalHours > 24 && c.Regional == regEmail.Regional).ToList(), 2, regEmail.Email, regEmail.Regional);
                    this.lista.RemoveAll(c => c.Regional == regEmail.Regional);
                }

                enviarEmail.EnviarAlerta(this.lista.Where(c => c.Horario.Subtract(DateTime.Now).TotalHours <= 24).ToList(), 1);
                enviarEmail.EnviarAlerta(this.lista.Where(c => c.Horario.Subtract(DateTime.Now).TotalHours > 24).ToList(), 2);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
