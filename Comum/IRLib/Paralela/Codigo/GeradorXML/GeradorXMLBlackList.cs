using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace IRLib.Paralela
{
    public class GeradorXMLBlackList
    {
        private BD bd = new BD();

        public bool Execucao { get; set; }

        private int Issuer { get; set; }
        private int Receiver { get; set; }
        private int ID { get; set; }
        private int EventID { get; set; }
        private int LocalID { get; set; }
        public string Nome { get; set; }
        private string Version { get; set; }
        private string AreaID { get; set; }
        private string PersonCategoryID { get; set; }
        private string SeassonPassID { get; set; }
        private string Coding { get; set; }
        private string Host { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Workingdirectory { get; set; }
        private int TicketTypeID { get; set; }

        List<int> EventoIDsBlack { get; set; }

        public ServicoFonteNova service;

        public GeradorXMLBlackList() { }

        public GeradorXMLBlackList(ConfigSection config)
        {
            this.Nome = config.Nome.Valor;
            this.LocalID = config.LocalID.Valor;
            this.ID = config.ID.Valor;
            this.EventID = config.EventID.Valor;
            this.Issuer = config.Issuer.Valor;
            this.Receiver = config.Receiver.Valor;
            this.Version = config.Version.Valor;
            this.AreaID = config.AreaID.Valor;
            this.PersonCategoryID = config.PersonCategoryID.Valor;
            this.SeassonPassID = config.SeassonPassID.Valor;
            this.Coding = config.Coding.Valor;
            this.Host = config.Host.Valor;
            this.Username = config.Username.Valor;
            this.Password = config.Password.Valor;
            this.Workingdirectory = config.Workingdirectory.Valor;
            this.TicketTypeID = config.TicketTypeID.Valor;
        }

        public List<KeyValuePair<string, XElement>> Buscar_BlacklistRecord()
        {
            BD bd = new BD();

            try
            {
                EventoIDsBlack = new List<int>();

                string SQL = string.Format(@"SELECT DISTINCT te.ID 
                FROM tEvento AS te (NOLOCK)
                INNER JOIN tApresentacao ta (NOLOCK) ON te.ID = ta.EventoID
                WHERE te.LocalID IN ({0}) AND ta.Horario >= '{1}%'", this.LocalID, DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                    EventoIDsBlack.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                if (EventoIDsBlack.Count > 0)
                {
                    BlacklistRecord black = new BlacklistRecord();

                    black.Header.Version = Version;
                    black.Header.Issuer = Issuer;
                    black.Header.Receiver = Receiver;
                    black.Header.ID = ID;

                    black.Expire = DateTime.Now;
                    black.Action = "U";
                    black.BlockingReason = 1;
                    black.BlockingType = 0;

                    black.DisplayMessage = "CANCELADO";
                    black.Comment = "CANCELLED";

                    int id = 0;

                    DateTime Horarioinicial = new DateTime();
                    DateTime Horariofinal = new DateTime();

                    for (int cont = 0; cont < EventoIDsBlack.Count; cont++)
                    {
                        SQL = string.Format(@"SELECT DISTINCT ticb.ID, ticb.CodigoBarra, SUBSTRING(ta.Horario,1,8) AS Horario, ta.Horario AS HorarioCompleto, 
                        tpt.ID AS PrecoTipoID, tash.HorarioInicial, tash.HorarioFinal, ticb.EventoID,
                        tgi.Horario as HoraGerenciada, ISNULL(tgi.PrecoTipoID, 0) AS GerencimanentoID
                        FROM tIngressoCodigoBarra ticb (NOLOCK) 
                        INNER JOIN tIngressoLog til (NOLOCK) ON til.CodigoBarra = ticb.CodigoBarra
                        INNER JOIN tIngresso ti (NOLOCK) ON ti.ID = til.IngressoID AND ti.EventoID = ticb.EventoID
                        INNER JOIN tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID AND ta.EventoID = ticb.EventoID
                        INNER JOIN tPreco tp (NOLOCK) ON tp.ID = til.PrecoID AND tp.ApresentacaoSetorID = ti.ApresentacaoSetorID
                        LEFT JOIN tPrecoTipo tpt (NOLOCK) ON tpt.ID = tp.PrecoTipoID
                        LEFT JOIN tGerenciamentoIngressos tgi (NOLOCK) ON tgi.ID = ti.GerenciamentoIngressosID
                        LEFT JOIN tAuxiliarSetoresHorario tash (NOLOCK) ON tash.SetorID = ti.SetorID
                        WHERE ticb.EventoID = {0} AND ticb.BlackList = 'T' AND (ticb.Sincronizado = 'T' OR ticb.Sincronizado = 'F')", EventoIDsBlack[cont]);

                        bd.Consulta(SQL);

                        while (bd.Consulta().Read())
                        {
                            black.From = DateTime.Now;
                            black.To = black.From.AddMonths(1);

                            id = bd.LerInt("ID");

                            switch (bd.LerInt("GerencimanentoID"))
                            {
                                case GerenciamentoIngressos.PRECOHORAMARCADA:
                                    Horarioinicial = Utilitario.String2DateTime(DateTime.Now.ToString("yyyyMMdd") + bd.LerString("HoraGerenciada"));
                                    Horariofinal = Horarioinicial.AddHours(1);
                                    break;
                                default:
                                    string horarioinicial = bd.LerString("HorarioInicial");

                                    if (string.IsNullOrEmpty(horarioinicial))
                                        Horarioinicial = Utilitario.String2DateTime(bd.LerString("HorarioCompleto"));
                                    else
                                        Horarioinicial = Utilitario.String2DateTime(bd.LerString("Horario") + horarioinicial);

                                    string horariofinal = bd.LerString("HorarioFinal");

                                    if (string.IsNullOrEmpty(horariofinal))
                                        Horariofinal = Utilitario.String2DateTime(bd.LerString("HorarioCompleto")).AddHours(6.5);
                                    else
                                        Horariofinal = Utilitario.String2DateTime(bd.LerString("Horario") + horariofinal);
                                    break;
                            }

                            black.Black_WhitelistRecord.Add(new WhitelistRecord_BlackListRecord()
                            {
                                Coding = this.Coding,
                                UTID = bd.LerString("CodigoBarra"),
                                Permission = new Permission_BlackListRecord()
                                {
                                    UPID = id
                                },
                                TSProperty = new TSProperty_BlackListRecord()
                                {
                                    AreaID = this.AreaID,
                                    EventoID = this.EventID == 0 ? bd.LerInt("EventoID").ToString() : this.EventID.ToString("00"),
                                    TicketTypeID = this.TicketTypeID == 0 ? bd.LerInt("PrecoTipoID").ToString("00") : this.TicketTypeID.ToString("00"),
                                    SeasonTicketID = this.SeassonPassID,
                                    From = Horarioinicial,
                                    To = Horariofinal
                                }
                            });
                        }
                        bd.FecharConsulta();
                    }

                    return this.Gerar_BlacklistRecord(black);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaFonteNova> Buscar_BlacklistRecordFonte()
        {
            BD bd = new BD();

            try
            {
                List<EstruturaFonteNova> retorno = new List<EstruturaFonteNova>();
                EventoIDsBlack = new List<int>();

                string SQL = string.Format(@"SELECT DISTINCT te.ID 
                FROM tEvento AS te (NOLOCK)
                INNER JOIN tApresentacao ta (NOLOCK) ON te.ID = ta.EventoID
                WHERE te.LocalID IN ({0}) AND ta.Horario >= '{1}%'", this.LocalID, DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                    EventoIDsBlack.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                if (EventoIDsBlack.Count > 0)
                {
                    SQL = string.Format(@"
                    SELECT DISTINCT ti.ID, ticb.CodigoBarra, te.Nome AS Evento, tasf.aIdEvento
                    FROM tIngressoCodigoBarra ticb (NOLOCK) 
                    INNER JOIN tIngressoLog til (NOLOCK) ON til.CodigoBarra = ticb.CodigoBarra
                    INNER JOIN tIngresso ti (NOLOCK) ON ti.ID = til.IngressoID AND ti.EventoID = ticb.EventoID
                    INNER JOIN tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID AND ta.EventoID = ticb.EventoID
                    INNER JOIN tPreco tp (NOLOCK) ON tp.ID = til.PrecoID AND tp.ApresentacaoSetorID = ti.ApresentacaoSetorID
                    INNER JOIN tSetor ts (NOLOCK) ON ts.ID = ti.SetorID
                    LEFT JOIN tLugar tl (NOLOCK) ON tl.ID = ti.LugarID
                    LEFT JOIN tCliente tc (NOLOCK) ON tc.ID = ti.ClienteID
                    INNER JOIN tAuxiliarSetoresFonte tasf (NOLOCK) ON tasf.SetorID = ti.SetorID AND tasf.PrecoID = ti.PrecoID
                    INNER JOIN tEvento te (NOLOCK) ON te.ID = ta.EventoID
                    WHERE ticb.EventoID IN ({0}) AND ticb.BlackList = 'T' AND ticb.Sincronizado = 'T'", Utilitario.ArrayToString(EventoIDsBlack.ToArray()));

                    bd.Consulta(SQL);

                    while (bd.Consulta().Read())
                    {
                        retorno.Add(new EstruturaFonteNova()
                        {
                            CodigoBarra = bd.LerString("CodigoBarra"),
                            Evento = bd.LerString("Evento"),
                            aIdEvento = bd.LerString("aIdEvento")
                        });
                    }

                    return retorno;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private bool AtualizarListaCodigoBarraIngresso(int IngressoCodigoBarraID, BD bdenviado)
        {
            try
            {
                bdenviado.FecharConsulta();

                return bdenviado.Executar(@"UPDATE tIngressoCodigoBarra SET Sincronizado = 'TT' WHERE ID = " + IngressoCodigoBarraID) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<KeyValuePair<string, XElement>> Gerar_BlacklistRecord(BlacklistRecord black)
        {
            try
            {
                List<KeyValuePair<string, XElement>> retorno = new List<KeyValuePair<string, XElement>>();

                foreach (WhitelistRecord_BlackListRecord item in black.Black_WhitelistRecord)
                {
                    XElement xml = new XElement(black.tSData);
                    xml.Add(new XElement(black.header,
                        new XElement(black.Header.version, black.Header.Version),
                        new XElement(black.Header.iD, black.Header.ID),
                        new XElement(black.Header.issuer, black.Header.Issuer),
                        new XElement(black.Header.receiver, black.Header.Receiver)));

                    xml.Add(new XElement(black.blacklistRecord, new XAttribute(black.expire, black.To.ToString("s")),
                    new XElement(black.action, black.Action),
                    new XElement(black.whitelistRecord, new XAttribute(black.expire, black.To.ToString("s")),
                        new XElement(black.action, black.Action),
                        new XElement(item.utid, item.UTID),
                        new XElement(item.coding, item.Coding),
                        new XElement(item.permission,
                            new XElement(item.Permission.upid, item.Permission.UPID),
                            new XElement(item.tsproperty, new XAttribute(item.TSProperty.type, item.TSProperty.Type_Event),
                                new XElement(item.TSProperty.id, item.TSProperty.EventoID)),
                        new XElement(item.tsproperty, new XAttribute(item.TSProperty.type, item.TSProperty.Type_Area),
                            new XElement(item.TSProperty.id, item.TSProperty.AreaID)),
                        new XElement(item.tsproperty, new XAttribute(item.TSProperty.type, item.TSProperty.Type_TicketType),
                            new XElement(item.TSProperty.id, item.TSProperty.TicketTypeID)),
                            new XElement(item.tsproperty, new XAttribute(item.TSProperty.type, item.TSProperty.Type_SeasonTicket),
                            new XElement(item.TSProperty.id, item.TSProperty.SeasonTicketID),
                            new XElement(item.tsproperty, string.Empty,
                                new XAttribute(item.TSProperty.type, item.TSProperty.Type_Validity),
                                new XAttribute(item.TSProperty.from, item.TSProperty.From.ToString("s")),
                                new XAttribute(item.TSProperty.to, item.TSProperty.To.ToString("s")))))),

                    new XElement(black.blockingtype, black.BlockingType),
                    new XElement(black.blockingreason, black.BlockingReason),
                    new XElement(black.from, black.From.ToString("s")),
                    new XElement(black.to, black.To.ToString("s")),
                    new XElement(black.displaymessage, black.DisplayMessage),
                    new XElement(black.comment, black.Comment)));

                    retorno.Add(new KeyValuePair<string, XElement>(item.UTID, xml));
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SalvarRespostaBlack(string Key, XElement Value, string XmlRetorno, string XmlTipo, bool enviado)
        {
            try
            {
                StringBuilder sql;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLogEnvioXml (TimeStamp, XmlEnviado, XmlRetorno, XmlTipo, CodigoBarra) ");
                sql.Append("VALUES ('@000', '@001', '@002', '@003', '@004')");
                sql.Replace("@000", DateTime.Now.ToString("yyyyMMddHHmmss"));
                sql.Replace("@001", Value.ToString());
                sql.Replace("@002", XmlRetorno);
                sql.Replace("@003", XmlTipo);
                sql.Replace("@004", Key);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result == false)
                    return false;

                bd.FecharConsulta();

                object id = bd.ConsultaValor("SELECT ID FROM tIngressoCodigoBarra (NOLOCK) WHERE CodigoBarra = '" + Key + "' AND BlackList = 'T' AND EventoID IN (" + Utilitario.ArrayToString(EventoIDsBlack.ToArray()) + ")");

                if (enviado)
                {
                    int ID = Convert.ToInt32(id);

                    if (ID > 0)
                        this.AtualizarListaCodigoBarraIngresso(ID, bd);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SalvarRespostaBlack(string Key, string Value, string XmlRetorno, string XmlTipo, bool enviado)
        {
            try
            {
                StringBuilder sql;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLogEnvioXml (TimeStamp, XmlEnviado, XmlRetorno, XmlTipo, CodigoBarra) ");
                sql.Append("VALUES ('@000', '@001', '@002', '@003', '@004')");
                sql.Replace("@000", DateTime.Now.ToString("yyyyMMddHHmmss"));
                sql.Replace("@001", Value);
                sql.Replace("@002", XmlRetorno);
                sql.Replace("@003", XmlTipo);
                sql.Replace("@004", Key);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result == false)
                    return false;

                bd.FecharConsulta();

                object id = bd.ConsultaValor("SELECT ID FROM tIngressoCodigoBarra (NOLOCK) WHERE CodigoBarra = '" + Key + "' AND BlackList = 'T' AND EventoID IN (" + Utilitario.ArrayToString(EventoIDsBlack.ToArray()) + ")");

                if (enviado)
                {
                    int ID = Convert.ToInt32(id);

                    if (ID > 0)
                        this.AtualizarListaCodigoBarraIngresso(ID, bd);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EnviarCodigo(List<EstruturaFonteNova> lista)
        {
            try
            {
                string nomeEvento = string.Empty;

                foreach (var item in lista)
                {
                    bool enviado = false;

                    if (!service.VerificarCodigoAcessou(item.CodigoBarra))
                        enviado = service.Bloquear(item.CodigoBarra, item.aIdEvento, "INGRESSO", "CANCELADO", string.Empty, string.Empty);

                    this.SalvarRespostaBlack(item.CodigoBarra, "Bloqueado", enviado == true ? "Enviado" : "Falhou", "BlackList", enviado);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.FechaConexao();
            }
        }

        public bool MatemServicoAtivo()
        {
            return service.VerificarCodigoAcessou(string.Empty);
        }

        public void FechaConexao()
        {
            this.bd.Fechar();
        }

        public bool EnviarFTP(string Key, MemoryStream stream)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(Host + Workingdirectory + "bl" + Key + ".imp");

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                stream.Position = 0;

                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                //Upload file
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
