using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using IRCore.Util;

namespace IRLib.HammerHead
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
            HammerHead = 1009,
            SeparacaoRiscoFraude = 1010,
            HammerHeadEI = 1011
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
            var sw = new Stopwatch();
            sw.Start();

            LogUtil.Debug("##SalvarLog.Salvar.Started##");
            try
            {
                var source = ConfigurationManager.AppSettings["EventViewerSource"];
                var logName = ConfigurationManager.AppSettings["EventViewerLogName"];

                LogUtil.Debug(String.Format("##SalvarLog.Salvar## SOURCE: {0}, LOG_NAME: {1}", source, logName));

                if (tipo == Enumeradores.TipoEntrada.Erro)
                    mensagem = string.Format("Erro ao processar.\n\n{0}\nErro:{1}", mensagem, Erro);

                if (!hasEnd)
                {
                    LogUtil.Debug(String.Format("##SalvarLog.Salvar.BeforeInsert## TEMPO_DECORRIDO: {0} ms",
                        sw.ElapsedMilliseconds));
                    if (ConfiguracaoHammerHead.Instancia.Configuracao.SalvarLogSQL.Valor)
                        BD.Executar(
                            string.Format("INSERT INTO tLogHammerHead VALUES ('{0}','{1}','{2}','{3}','{4}', '{5}')",
                                source, logName, mensagem, tipo, evento.ToString(),
                                DateTime.Now.ToString("yyyyMMddHHmmss")));

                    LogUtil.Debug(String.Format("##SalvarLog.Salvar.AfterInsert## TEMPO_DECORRIDO: {0} ms",
                        sw.ElapsedMilliseconds));
                }
            }
            catch (Exception ex)
            {
                SalvarLog.EscreveLog(mensagem + "\n " + Erro + "\nException: " + ex.Message);
            }
            finally
            {
                sw.Stop();
                LogUtil.Debug(String.Format("##SalvarLog.Salvar.FINISH## TEMPO_DECORRIDO_TOTAL: {0} ms", sw.ElapsedMilliseconds));
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
