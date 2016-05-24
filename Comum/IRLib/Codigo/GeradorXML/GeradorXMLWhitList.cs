using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace IRLib
{
    public class GeradorXMLWhitList
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

        List<int> EventoIDsWhite { get; set; }

        public ServicoFonteNova service;

        public GeradorXMLWhitList() { }

        public GeradorXMLWhitList(ConfigSection config)
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

        public List<KeyValuePair<string, XElement>> Buscar_TSData_WhitelistRecord()
        {
            BD bd = new BD();

            try
            {
                EventoIDsWhite = new List<int>();

                string SQL = string.Format(@"SELECT DISTINCT te.ID 
                FROM tEvento AS te (NOLOCK)
                INNER JOIN tApresentacao ta (NOLOCK) ON te.ID = ta.EventoID
                WHERE te.LocalID IN ({0}) AND ta.Horario >= '{1}%'", this.LocalID, DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                    EventoIDsWhite.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                if (EventoIDsWhite.Count > 0)
                {
                    TSData_WhitelistRecord tsData = new TSData_WhitelistRecord();

                    tsData.Header.Version = Version;
                    tsData.Header.Issuer = Issuer;
                    tsData.Header.Receiver = Receiver;
                    tsData.Header.ID = ID;

                    for (int cont = 0; cont < EventoIDsWhite.Count; cont++)
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
                        WHERE ticb.EventoID = {0} AND ta.Horario >= '{1}%' AND ticb.Sincronizado = 'F'", EventoIDsWhite[cont], DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));

                        bd.Consulta(SQL);

                        while (bd.Consulta().Read())
                        {
                            DateTime Horarioinicial = new DateTime();
                            DateTime Horariofinal = new DateTime();

                            switch (bd.LerInt("GerencimanentoID"))
                            {
                                case GerenciamentoIngressos.PRECOHORAMARCADA:
                                    Horarioinicial = Utilitario.String2DateTime(DateTime.Now.ToString("yyyyMMdd") + bd.LerString("HoraGerenciada"));
                                    Horariofinal = Horarioinicial.AddHours(1);
                                    break;
                                default:
                                    string horarioinicial = bd.LerString("HorarioInicial");
                                    string horariofinal = bd.LerString("HorarioFinal");

                                    if (string.IsNullOrEmpty(horarioinicial))
                                        Horarioinicial = Utilitario.String2DateTime(bd.LerString("HorarioCompleto"));
                                    else
                                        Horarioinicial = Utilitario.String2DateTime(bd.LerString("Horario") + horarioinicial);

                                    if (string.IsNullOrEmpty(horariofinal))
                                        Horariofinal = Horarioinicial.AddHours(7.5);
                                    else
                                        Horariofinal = Utilitario.String2DateTime(bd.LerString("Horario") + horariofinal);
                                    break;
                            }

                            tsData.WhitelistRecord.Add(new Whitelist_WhitelistRecord()
                            {
                                Action = "U",
                                Coding = this.Coding,
                                UTID = bd.LerString("CodigoBarra"),

                                Permission = new Permission_WhitelistRecord()
                                {
                                    UPID = bd.LerInt("ID"),
                                    From = Horarioinicial,
                                    To = Horariofinal,

                                    TSProperty = new TSProperty_WhitelistRecord()
                                    {
                                        AreaID = this.AreaID,
                                        EventoID = this.EventID == 0 ? bd.LerInt("EventoID").ToString() : this.EventID.ToString("00"),
                                        PersonCategoryID = this.PersonCategoryID,
                                        SeassonPassID = this.SeassonPassID,
                                        TicketTypeID = this.TicketTypeID == 0 ? bd.LerInt("PrecoTipoID").ToString("00") : this.TicketTypeID.ToString("00")
                                    }
                                }
                            });
                        }

                        bd.FecharConsulta();
                    }

                    return this.Gerar_TSData_WhitelistRecord(tsData);
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

        public List<EstruturaFonteNova> WhitelistRecordFonte()
        {
            BD bd = new BD();

            try
            {
                List<EstruturaFonteNova> retorno = new List<EstruturaFonteNova>();
                EventoIDsWhite = new List<int>();

                string SQL = string.Format(@"SELECT DISTINCT te.ID 
                FROM tEvento AS te (NOLOCK)
                INNER JOIN tApresentacao ta (NOLOCK) ON te.ID = ta.EventoID
                WHERE te.LocalID IN ({0}) AND ta.Horario >= '{1}%'", this.LocalID, DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                    EventoIDsWhite.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                if (EventoIDsWhite.Count > 0)
                {
                    SQL = string.Format(@"
                    SELECT DISTINCT ti.ID, ticb.CodigoBarra,ti.Codigo, 
                    ts.LugarMarcado, tp.Valor, ti.VendaBilheteriaID, ISNULL(tc.ID, 0) AS ClienteID, 
                    ISNULL(tc.Nome, '') AS Cliente, tasf.Chave, te.Nome AS Evento, tasf.aIdEvento
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
                    WHERE ticb.EventoID IN ({0}) AND ticb.Sincronizado = 'F'", Utilitario.ArrayToString(EventoIDsWhite.ToArray()));

                    bd.Consulta(SQL);

                    while (bd.Consulta().Read())
                    {
                        retorno.Add(new EstruturaFonteNova()
                        {
                            ID = bd.LerInt("ID"),
                            CodigoBarra = bd.LerString("CodigoBarra"),
                            Codigo = bd.LerString("Codigo"),
                            LugarMarcado = bd.LerString("LugarMarcado"),
                            Valor = bd.LerDecimal("Valor"),
                            VendaBilheteriaID = bd.LerInt("VendaBilheteriaID"),
                            ClienteID = bd.LerInt("ClienteID"),
                            Cliente = bd.LerString("Cliente"),
                            Chave = bd.LerString("Chave"),
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

                return bdenviado.Executar(@"UPDATE tIngressoCodigoBarra SET Sincronizado = 'T' WHERE Sincronizado = 'F' AND ID = " + IngressoCodigoBarraID) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<KeyValuePair<string, XElement>> Gerar_TSData_WhitelistRecord(TSData_WhitelistRecord tsData)
        {
            try
            {
                List<KeyValuePair<string, XElement>> retorno = new List<KeyValuePair<string, XElement>>();

                foreach (Whitelist_WhitelistRecord white in tsData.WhitelistRecord)
                {
                    XElement xml = new XElement(tsData.tSData);

                    xml.Add(new XElement(tsData.header,
                        new XElement(tsData.Header.version, tsData.Header.Version),
                        new XElement(tsData.Header.issuer, tsData.Header.Issuer),
                        new XElement(tsData.Header.receiver, tsData.Header.Receiver),
                        new XElement(tsData.Header.iD, tsData.Header.ID)));

                    xml.Add(new XElement(tsData.whitelistRecord, new XAttribute(tsData.Header.expire, white.Permission.To.AddMonths(1).ToString("s")),
                    new XElement(white.action, white.Action),
                    new XElement(white.utid, white.UTID),
                    new XElement(white.coding, white.Coding),
                    new XElement(white.permission, new XElement(white.Permission.upid, white.Permission.UPID),
                            new XElement(white.Permission.tsproperty, new XAttribute(white.Permission.type, white.Permission.Type_Event),
                                new XElement(white.Permission.TSProperty.id, white.Permission.TSProperty.EventoID)),
                            new XElement(white.Permission.tsproperty, new XAttribute(white.Permission.type, white.Permission.Type_Area),
                                new XElement(white.Permission.TSProperty.id, white.Permission.TSProperty.AreaID)),
                            new XElement(white.Permission.tsproperty, new XAttribute(white.Permission.type, white.Permission.Type_TicketType),
                                new XElement(white.Permission.TSProperty.id, white.Permission.TSProperty.TicketTypeID)),
                            new XElement(white.Permission.tsproperty, new XAttribute(white.Permission.type, white.Permission.Type_SeasonTicket),
                                new XElement(white.Permission.TSProperty.id, white.Permission.TSProperty.SeassonPassID)),
                            new XElement(white.Permission.tsproperty, string.Empty,
                                new XAttribute(white.Permission.type, white.Permission.Type_Validity),
                                new XAttribute(white.Permission.from, white.Permission.From.ToString("s")),
                                new XAttribute(white.Permission.to, white.Permission.To.ToString("s"))))));

                    retorno.Add(new KeyValuePair<string, XElement>(white.UTID, xml));
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SalvarRespostaWhite(string Key, XElement Value, string XmlRetorno, string XmlTipo, bool enviado)
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

                object id = bd.ConsultaValor("SELECT ID FROM tIngressoCodigoBarra (NOLOCK) WHERE CodigoBarra = '" + Key + "' AND BlackList = 'F' AND EventoID IN (" + Utilitario.ArrayToString(EventoIDsWhite.ToArray()) + ")");

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

        public bool SalvarRespostaWhite(string Key, string Value, string XmlRetorno, string XmlTipo, bool enviado)
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

                object id = bd.ConsultaValor("SELECT ID FROM tIngressoCodigoBarra (NOLOCK) WHERE CodigoBarra = '" + Key + "' AND BlackList = 'F' AND EventoID IN (" + Utilitario.ArrayToString(EventoIDsWhite.ToArray()) + ")");

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

        public bool SalvarRespostaWhiteFonte(string Key, string Value, string XmlRetorno, string XmlTipo, bool enviado)
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

                object id = bd.ConsultaValor("SELECT ID FROM tIngressoCodigoBarra (NOLOCK) WHERE CodigoBarra = '" + Key + "' AND EventoID IN (" + Utilitario.ArrayToString(EventoIDsWhite.ToArray()) + ")");

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
                string inserido = string.Empty;
                string nomeEvento = string.Empty;

                foreach (var item in lista)
                {
                    if (item.LugarMarcado == Setor.Pista)
                        inserido = service.Inserir(item.Chave, item.CodigoBarra, null);
                    else
                    {
                        Extra extra = new Extra()
                        {
                            assento = item.Codigo,
                            bloco = item.Codigo,
                            fila = string.Empty,
                            matricula = item.ClienteID.ToString(),
                            nmpessoa = item.Cliente,
                            localizador = item.VendaBilheteriaID.ToString(),
                            valor = item.Valor.ToString()
                        };

                        inserido = service.Inserir(item.Chave, item.CodigoBarra, extra);
                    }

                    bool enviado = !string.IsNullOrEmpty(inserido);

                    this.SalvarRespostaWhiteFonte(item.CodigoBarra, inserido, enviado == true ? "Enviado" : "Falhou", "WhiteList", enviado);
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
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(Host + Workingdirectory + Key + ".imp");

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
