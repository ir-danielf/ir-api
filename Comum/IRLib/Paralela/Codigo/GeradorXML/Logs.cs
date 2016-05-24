using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    public class Logs
    {
        public enum enumEntryType
        {
            Sucess,
            Error,
            Information,
        }

        public void Salvar(string mensagem, enumEntryType tipoEntrada)
        {
            Salvar(mensagem, tipoEntrada, 1000);
        }

        public void Salvar(string mensagem, enumEntryType tipoEntrada, int eventID)
        {
            SalvarLog(false, DateTime.Now, DateTime.Now, mensagem, string.Empty, tipoEntrada, eventID);
        }

        public void Salvar(string mensagem, string erro, int eventID)
        {
            SalvarLog(false, DateTime.Now, DateTime.Now, mensagem, erro, enumEntryType.Error, eventID);
        }

        private void SalvarLog(bool hasEnd, DateTime inicio, DateTime fim, string mensagem, string Erro, enumEntryType tipo, int eventID)
        {
            try
            {
                if (!EventLog.SourceExists("Robo Nostradamus"))
                    EventLog.CreateEventSource("Robo Nostradamus", "Robo Nostradamus");

                EventLog log = new EventLog("Robo Nostradamus");
                log.Source = "Robo Nostradamus";
                log.Log = "Robo Nostradamus";

                if (Erro.Length > 0)
                {
                    switch (tipo)
                    {
                        case enumEntryType.Error:
                            log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Error, eventID);
                            break;
                        default:
                            log.WriteEntry(string.Format("Erro gerado na sincronização de informações.\n{0}\nErro:{1}", mensagem, Erro), EventLogEntryType.Warning, eventID);
                            break;
                    }
                }
                else
                {
                    if (!hasEnd)
                    {
                        switch (tipo)
                        {
                            case enumEntryType.Sucess:
                                log.WriteEntry(mensagem, EventLogEntryType.SuccessAudit, eventID);
                                break;
                            case enumEntryType.Error:
                                log.WriteEntry(mensagem, EventLogEntryType.Error, eventID);
                                break;
                            default:
                                log.WriteEntry(mensagem, EventLogEntryType.Information, eventID);
                                break;
                        }
                    }
                    else
                    {
                        switch (tipo)
                        {
                            case enumEntryType.Sucess:
                                log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                break;
                            case enumEntryType.Error:
                                log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                                break;
                            default:
                                log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.Information, eventID);
                                break;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
