using System;
using System.Diagnostics;
using System.IO;

namespace IRLib.Paralela.HammerHead
{
    public static class SalvarLog
    {
        private static CTLib.BD bd = null;

        private static CTLib.BD BD
        {
            get
            {
                if (bd == null)
                    bd = new CTLib.BD();

                return bd;
            }
        }
        public enum Events
        {
            CredencialInvalida = 1000,
            AmbienteTesteAtivo = 1001,
            UtilizacaoLink = 1002,
            Generico = 1003,
            Fila = 1004,
            Transacoes = 1005,
            Accertify = 1006,
            Email = 1007,
            Listener = 1008,
            HammerHead = 1009
        }
        public static void Salvar(string mensagem, Enumeradores.TipoEntrada tipoEntrada)
        {
            Salvar(mensagem, tipoEntrada, Events.Generico);
        }
        public static void Salvar(string mensagem, Enumeradores.TipoEntrada tipoEntrada, Events eventID)
        {
            Salvar(false, DateTime.Now, DateTime.Now, mensagem, string.Empty, tipoEntrada, eventID);
        }

        public static void SalvarErro(string mensagem, string erro, Events eventID)
        {
            Salvar(false, DateTime.Now, DateTime.Now, mensagem, erro, Enumeradores.TipoEntrada.Erro, eventID);
        }

        private static void Salvar(bool hasEnd, DateTime inicio, DateTime fim, string mensagem, string Erro, Enumeradores.TipoEntrada tipo, Events evento)
        {
            try
            {
                var eventID = (int)evento;

                if (!System.Diagnostics.EventLog.SourceExists("HammerHead"))
                    System.Diagnostics.EventLog.CreateEventSource("HammerHead", "Processamento TEF");

                EventLog log = new EventLog("HammerHead");

                if (evento == Events.Listener)
                    log.Source = "Listener";
                else
                    log.Source = "HammerHead";

                log.Log = "Processamento TEF";

                if (tipo == Enumeradores.TipoEntrada.Erro)
                    mensagem = string.Format("Erro ao processar.\n\n{0}\nErro:{1}", mensagem, Erro);

                if (!hasEnd)
                {
                    switch (tipo)
                    {
                        case Enumeradores.TipoEntrada.Sucesso:
                            log.WriteEntry(mensagem, EventLogEntryType.SuccessAudit, eventID);
                            break;
                        case Enumeradores.TipoEntrada.Erro:
                            log.WriteEntry(mensagem, EventLogEntryType.Error, eventID);
                            break;
                        case Enumeradores.TipoEntrada.Alerta:
                            log.WriteEntry(mensagem, EventLogEntryType.Warning, eventID);
                            break;
                        default:
                            log.WriteEntry(mensagem, EventLogEntryType.Information, eventID);
                            break;
                    }

                    if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLogSQL.Valor)
                        BD.Executar(string.Format("INSERT INTO tLogHammerHead VALUES ('{0}','{1}','{2}','{3}','{4}', '{5}')", log.Source, log.Log, mensagem, tipo, evento.ToString(), DateTime.Now.ToString("yyyyMMddHHmmss")));
                }
                else
                {
                    switch (tipo)
                    {
                        case Enumeradores.TipoEntrada.Sucesso:
                            log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                            break;
                        case Enumeradores.TipoEntrada.Erro:
                            log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.SuccessAudit, eventID);
                            break;
                        default:
                            log.WriteEntry(string.Format("Ação executada com sucesso\n{0} em {1} segundos", mensagem, fim.Subtract(inicio).Seconds), EventLogEntryType.Information, eventID);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SalvarLog.EscreveLog(mensagem + "\n " + Erro + "\nException: " + ex.Message);
            }
        }

        private static void EscreveLog(string msg)
        {
            StreamWriter arquivo = null;
            try
            {
                arquivo = new StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "") + "\\Log.txt", true);
                arquivo.WriteLine(msg);
                arquivo.WriteLine(DateTime.Now.ToString());
                arquivo.WriteLine("==============================");
                arquivo.Flush();
                arquivo.Close();
                arquivo.Dispose();
            }
            catch { }
            finally
            {
                if (arquivo != null)
                {
                    try
                    {
                        arquivo.Close();
                        arquivo.Dispose();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}
