using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace IngressoRapido.Lib
{
    public class Evento
    {
        public string URLImagem
        {
            get
            {
                return ConfigurationManager.AppSettings["DiretorioImagensEventos"] == null ? string.Empty : ConfigurationManager.AppSettings["DiretorioImagensEventos"].ToString();
            }
        }
        private const string SUFIXO_THUMB = "thumb";
        public Evento()
        {
        }

        public Evento(int id)
        {
            this.id = id;
        }

        DAL oDAL = new DAL();

        BD oBD = new BD();

        private int id;
        public int Id
        {
            get { return id; }
        }

        private int localID;
        public int LocalID
        {
            get { return localID; }
            set { localID = value; }
        }

        private string nome;
        public string Nome
        {
            get { return Util.ToTitleCase(this.nome); }
            set { nome = value; }
        }

        private int tipoID;
        public int TipoID
        {
            get { return tipoID; }
            set { tipoID = value; }
        }

        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private int subtipoID;
        public int SubtipoID
        {
            get { return subtipoID; }
            set { subtipoID = value; }
        }

        private string subtipo;
        public string Subtipo
        {
            get { return subtipo; }
            set { subtipo = value; }
        }
        public bool ExibeQuantidade { get; set; }
        private string publicar;
        public string Publicar
        {
            get { return publicar; }
            set
            {
                switch (value)
                {
                    case "T":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.PublicadoParaVenda;
                        break;
                    case "S":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.PublicadoSemVenda;
                        break;
                    case "F":
                        this.PublicaoTipo = IRLib.Evento.PublicarTipo.NaoPublicado;
                        break;
                }
                publicar = value;
            }
        }

        public IRLib.Evento.PublicarTipo PublicaoTipo { get; set; }

        private IRLib.Evento.SemVendaMotivo semVendaMotivo;
        public IRLib.Evento.SemVendaMotivo SemVendaMotivo
        {
            get { return semVendaMotivo; }
            set { semVendaMotivo = value; }
        }



        public List<EstruturaEntregaAgenda> ListaEntrega(List<int> listaApresentacao, List<int> Eventos, string CEP)
        {
            IRLib.Evento oE = new IRLib.Evento();
            return oE.ListaEntrega(listaApresentacao, Eventos, CEP);
        }
        public DateTime MenorApresentacao { get; set; }
        public decimal MenorPreco { get; set; }

        public List<EstruturaEntregaAgenda> ListaRetirada(List<int> listaApresentacao, List<int> Eventos)
        {

            IRLib.Evento oE = new IRLib.Evento();
            return oE.ListaRetirada(listaApresentacao, Eventos);

        }

        public bool PossuiEntrega(List<int> listaApresentacao, List<int> listaEventos)
        {
            IRLib.Evento oE = new IRLib.Evento();
            return oE.PossuiEntrega(listaApresentacao, listaEventos);
        }

        public bool PossuiRetirada(List<int> listaApresentacao, List<int> listaEventos)
        {
            IRLib.Evento oE = new IRLib.Evento();

            string BilheteriaID = System.Configuration.ConfigurationManager.AppSettings["IDRetiradaBilheteria"];
            string RetiradaID = System.Configuration.ConfigurationManager.AppSettings["IDRetiradaPdv"];

            return oE.PossuiRetirada(listaApresentacao, listaEventos, BilheteriaID, RetiradaID);
        }

        public int PossuiBilheteWeb(List<int> listaApresentacao, List<int> listaEventos)
        {
            IRLib.Evento oE = new IRLib.Evento();

            return oE.PossuiBilheteWeb(listaApresentacao, listaEventos);
        }

        public List<EstruturaEntregaAgenda> ListaEntregaVIR(string CEP)
        {
            IRLib.Evento oE = new IRLib.Evento();
            return oE.ListaEntregaVIR(CEP);
        }

        public List<EstruturaEntregaAgenda> ListaRetiradaVIR()
        {
            IRLib.Evento oE = new IRLib.Evento();
            return oE.ListaRetiradaVIR();
        }

        private string publicarsvm;
        public string PublicarSemVendaMotivo
        {
            get { return publicarsvm; }
            set
            {
                publicarsvm = value;
                switch (value)
                {
                    case "1":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaOnlineNaoDisponivel;
                        break;
                    case "2":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaSomenteCallCenter;
                        break;
                    case "3":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasEncerradas;
                        break;
                    case "4":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendasNaoIniciadas;
                        break;
                    case "5":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelApenasParaPacotes;
                        break;
                    case "6":
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaDisponivelDeterminadaData;
                        break;
                    default:
                        this.SemVendaMotivo = IRLib.Evento.SemVendaMotivo.VendaOnlineNaoDisponivel;
                        break;
                }

                //switch (value)
                //{
                //    case "1" :
                //        publicarsvm = "Este evento não está disponível para venda via internet."; 
                //        break;
                //    case "2":
                //        publicarsvm = "A compra de ingressos para este evento só pode ser realizada através do callcenter.";
                //        break;
                //    case "3":
                //        publicarsvm = "A venda de ingressos para este evento está encerrada.";
                //        break;
                //    case "4":
                //        publicarsvm = "A venda de ingressos para este evento ainda não foi iniciada.";
                //        break;
                //    case "5":
                //        publicarsvm = "Não está disponível a venda de ingressos avulsos para este evento. Por favor, selecione um item no campo Pacote e clique em OK.";
                //        break;
                //    case "6":
                //        publicarsvm = "Vendas apenas a partir da data selecionada";
                //        break;
                //    default:
                //        publicarsvm = string.Empty;
                //        break;
                //}                
            }
        }

        private string dataAberturaVenda;
        public string DataAberturaVenda
        {
            get { return dataAberturaVenda; }
            set { dataAberturaVenda = value; }
        }

        private string release;
        public string Release
        {
            get { return release; }
            set { release = value; }
        }

        private int parcelas;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        private string imagem;
        public string Imagem
        {
            get { return URLImagem + imagem; }
            set
            {
                if (value != null && value.Length > 0)
                    imagem = value;
                else
                    imagem = "noimage.gif";
            }
        }
        public string Thumb
        {
            get
            {
                return URLImagem + Path.GetFileNameWithoutExtension(this.imagem) + SUFIXO_THUMB + Path.GetExtension(this.imagem);
            }
        }

        private string local;
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string localDestaque;
        public string LocalDestaque
        {
            get { return localDestaque; }
            set { localDestaque = value; }
        }

        public bool EscolherLugarMarcado { get; set; }

        private string horario;
        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        private string localimagemNome;
        public string LocalImagemNome
        {
            get
            {
                if (localimagemNome == string.Empty)
                    return "Images/mapa_ind.jpg";
                else
                    return ConfigurationManager.AppSettings["DiretorioImagensMapas"].ToString() + localimagemNome;
            }
            set { localimagemNome = value; }
        }

        public string Pais { get; set; }

        public ApresentacaoLista ApresentacaoLista { get; set; }

        public Evento GetByID(int id)
        {
            string strSql = "" +
                            "SELECT Evento.IR_EventoID, Evento.Nome, Evento.Imagem, Evento.Release, " +
                            "Evento.Parcelas, Evento.LocalID, IsNull(Tipo.IR_TipoID, 0) AS IR_TipoID, IsNull(Tipo.Nome, '') AS Tipo, " +
                            "IsNull(EventoSubtipo.IR_SubtipoID, 0) AS IR_SubtipoID, IsNull(EventoSubtipo.Descricao, '') AS Subtipo, " +
                            "Evento.Publicar, IsNull(Evento.PublicarSemVendaMotivo, '') AS PublicarSemVendaMotivo, IsNull(DataAberturaVenda, '') AS DataAberturaVenda, " +
                            "LocalImagemNome, EscolherLugarMarcado, ExibeQuantidade " +
                            "FROM Evento (NOLOCK) " +
                            "LEFT JOIN EventoSubtipo (NOLOCK) ON Evento.SubtipoID = EventoSubtipo.IR_SubtipoID " +
                            "LEFT JOIN Tipo (NOLOCK) ON EventoSubtipo.TipoID = Tipo.IR_TipoID " +
                            "WHERE (Evento.IR_EventoID = " + id + ")";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["IR_EventoID"]);
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.Imagem = dr["Imagem"].ToString().Trim();
                        this.Release = dr["Release"].ToString();
                        this.Parcelas = Convert.ToInt32(dr["Parcelas"].ToString());
                        this.localID = Convert.ToInt32(dr["LocalID"].ToString());
                        this.tipoID = Convert.ToInt32(dr["IR_TipoID"]);
                        this.Tipo = Util.LimparTitulo(dr["Tipo"].ToString());
                        this.SubtipoID = Convert.ToInt32(dr["IR_SubtipoID"]);
                        this.Subtipo = Convert.ToString(dr["Subtipo"]);
                        this.Publicar = dr["Publicar"].ToString();
                        this.DataAberturaVenda = dr["DataAberturaVenda"].ToString().Length > 0 ? (DateTime.ParseExact(dr["DataAberturaVenda"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao)).ToShortDateString() : string.Empty;
                        this.PublicarSemVendaMotivo = dr["PublicarSemVendaMotivo"].ToString();
                        this.localimagemNome = dr["LocalImagemNome"].ToString() == "ls000000.gif" ? "mapa_ind.jpg" : Convert.ToString(dr["LocalImagemNome"]);
                        this.EscolherLugarMarcado = Convert.ToBoolean(dr["EscolherLugarMarcado"]);
                        this.ExibeQuantidade = Convert.ToBoolean(dr["ExibeQuantidade"]);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public static string PegaImagemTitulo(int tipoID)
        {
            string caminho = "imagens/";
            switch (tipoID)
            {
                case 1:
                    return caminho + "tit_teatro.gif";
                case 2:
                    return caminho + "tit_shows.gif";
                case 6:
                    return caminho + "tit_esportes.gif";
                case 7:
                    return caminho + "tit_parques.gif";
                case 12:
                    return caminho + "tit_concertos.gif";
                case -1:
                    return caminho + "tit_eventos.gif";
                default:
                    return caminho + "tit_eventos.gif";


            }
        }

        public EstruturaEventoDescricao GetByIDMobile(int EventoID, string HorarioAtual)
        {
            try
            {
                EstruturaEventoDescricao retorno = new EstruturaEventoDescricao();
                retorno.Apresentacoes = new List<EstruturaApresentacaoMobile>();

                string strSql = string.Format(@"SELECT Evento.Nome, Evento.Release , Evento.Imagem, Evento.publicar, Evento.PublicarSemVendaMotivo, Evento.DataAberturaVenda , EventoSubtipo.Descricao as Genero, 
                Tipo.Nome as Categoria, Local.Nome as Local, Local.Endereco, Local.Cidade, Local.Estado, Local.Pais, Local.Cep,
                Apresentacao.Horario, Apresentacao.IR_ApresentacaoID, Apresentacao.UsarEsquematico
                FROM Evento (NOLOCK)
                INNER JOIN Apresentacao (NOLOCK) ON Apresentacao.EventoID = Evento.IR_EventoID
                INNER JOIN EventoSubtipo (NOLOCK) ON EventoSubtipo.IR_SubtipoID = Evento.SubtipoID
                INNER JOIN Tipo (NOLOCK) ON Tipo.IR_TipoID = EventoSubtipo.TipoID
                INNER JOIN Local (NOLOCK) ON Local.IR_LocalID = Evento.LocalID
                WHERE IR_EventoID = {0} 
                  AND (CONVERT(VARCHAR,DATEADD(day, Evento.MenorPeriodoEntrega, Substring('" + HorarioAtual + @"', 0 ,9)), 112) <= Substring(Horario, 0, 9))
                  AND Horario > '" + HorarioAtual + @"' ORDER BY Horario", EventoID);

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (!dr.Read())
                        return retorno;

                    retorno.ID = EventoID;
                    retorno.Nome = dr["Nome"].ToString();
                    retorno.Categoria = dr["Categoria"].ToString();
                    retorno.Genero = dr["Genero"].ToString();
                    retorno.Cidade = dr["Cidade"].ToString();
                    retorno.Descricao = dr["Release"].ToString();
                    retorno.Endereco = dr["Endereco"].ToString();
                    retorno.CEP = dr["Cep"].ToString();
                    retorno.Estado = dr["Estado"].ToString();
                    retorno.Local = dr["Local"].ToString();
                    retorno.Pais = dr["Pais"].ToString();
                    retorno.Imagem = dr["Imagem"].ToString();
                    retorno.Publicar = dr["Publicar"].ToString();
                    retorno.PublicarSemVendaMotivo = dr["PublicarSemVendaMotivo"].ToString();

                    if ((dr["DataAberturaVenda"] != null) && (dr["DataAberturaVenda"].ToString().Length == 14))
                    {
                        retorno.NaoVendaDataInicio = DateTime.ParseExact(dr["DataAberturaVenda"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }

                    do
                    {
                        retorno.Apresentacoes.Add(new EstruturaApresentacaoMobile()
                        {
                            Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                            HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM \\de yyyy à\\s HH:mm"),
                            ID = dr["IR_ApresentacaoID"].ToInt32(),
                            UsarEsquematico = Convert.ToBoolean(dr["UsarEsquematico"])
                        });
                    } while (dr.Read());
                }

                return retorno;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public string CarregarRelease(int eventoID)
        {
            try
            {
                string Release = string.Empty;

                StringBuilder stb = new StringBuilder();
                stb.AppendFormat("SELECT e.Release FROM Evento e (NOLOCK) WHERE e.IR_EventoID = {0}", eventoID);

                using (IDataReader dr = oDAL.SelectToIDataReader(stb.ToString()))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações deste evento.");

                    Release = dr["Release"].ToString();
                }

                return Release;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public AgrupamentoVendaSimples Carregar(int eventoID, bool filtraHorario = true)
        {
            AgrupamentoVendaSimples agrupamento = this.CarregarEvento(eventoID, filtraHorario);

            agrupamento = this.CarregarPacotes(agrupamento, eventoID);

            return agrupamento;
        }

        private AgrupamentoVendaSimples CarregarEvento(int eventoID, bool filtraHorario = true)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, e.Release, e.Publicar, ");
                stb.Append("    ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, ");
                stb.Append("    s.IR_SetorID AS SetorID, s.Nome AS Setor, s.LugarMarcado, s.QtdeDisponivel, ");
                stb.Append("    p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor ");
                stb.Append("FROM Evento e (NOLOCK) ");
                stb.Append("INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID ");
                stb.Append("INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID ");
                stb.Append("INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID ");
                stb.AppendFormat("WHERE e.IR_EventoID = {0} AND p.Pacote = 0 AND p.Serie = 0 AND s.LugarMarcado = 'P' ", eventoID);

                if (filtraHorario)
                    stb.AppendFormat(" AND ap.Horario > '{0}' ", DateTime.Now.AddHours(2).ToString("yyyyMMddHHmmss"));

                stb.Append("ORDER BY ap.Horario ASC, s.Nome, p.Nome ");


                AgrupamentoVendaSimples agrupamento = new AgrupamentoVendaSimples();

                using (IDataReader dr = oDAL.SelectToIDataReader(stb.ToString()))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações deste evento.");

                    agrupamento.Evento.EventoID = dr["EventoID"].ToInt32();
                    agrupamento.Evento.Nome = dr["Evento"].ToString();
                    agrupamento.Evento.DisponivelVenda = (dr["Publicar"].ToString() == "T");
                    agrupamento.Evento.Release = dr["Release"].ToString();

                    ApresentacaoSimples apresentacao = new ApresentacaoSimples();
                    SetorSimples setor = new SetorSimples();

                    do
                    {
                        if (agrupamento.Evento.Apresentacoes.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32()).Count() == 0)
                        {
                            apresentacao = new ApresentacaoSimples()
                            {
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                                HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM \\de yyyy"),
                                Dia = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd"),
                            };

                            agrupamento.Evento.Apresentacoes.Add(apresentacao);
                        }

                        if (apresentacao.Setores.Where(c => c.SetorID == dr["SetorID"].ToInt32()).Count() == 0)
                        {
                            setor = new SetorSimples()
                            {
                                SetorID = dr["SetorID"].ToInt32(),
                                Nome = dr["Setor"].ToString(),
                                LugarTipo = dr["LugarMarcado"].ToString(),
                                QtdeDisponivel = dr["QtdeDisponivel"].ToInt32(),
                            };
                            apresentacao.Setores.Add(setor);
                        }

                        setor.Precos.Add(new PrecoSimples()
                        {
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Nome = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal(),
                        });

                    } while (dr.Read());
                }
                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public AgrupamentoVendaSimples CarregarParadise(int eventoID)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, e.Publicar, ");
                stb.Append("ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, ");
                stb.Append("s.IR_SetorID AS SetorID, sp.GrupoID, sp.Nome as NomeParadise, s.Nome AS Setor, s.LugarMarcado, s.QtdeDisponivel, ");
                stb.Append("p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor ");
                stb.Append("FROM Evento e (NOLOCK) ");
                stb.Append("INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID ");
                stb.Append("INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID ");
                stb.Append("INNER JOIN SetoresParadise sp (NOLOCK) ON sp.SetorID = s.IR_SetorID ");
                stb.Append("INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID ");
                stb.AppendFormat("WHERE e.IR_EventoID = {0} AND p.Pacote = 0 AND p.Serie = 0 AND s.LugarMarcado = 'P' ", eventoID);
                stb.AppendFormat(" AND ap.Horario > '{0}' ", DateTime.Now.AddHours(2).ToString("yyyyMMddHHmmss"));
                stb.Append("ORDER BY ap.Horario ASC, sp.GrupoID, SetorID");

                AgrupamentoVendaSimples agrupamento = new AgrupamentoVendaSimples();

                using (IDataReader dr = oDAL.SelectToIDataReader(stb.ToString()))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações deste evento.");

                    agrupamento.Evento.EventoID = dr["EventoID"].ToInt32();
                    agrupamento.Evento.Nome = dr["Evento"].ToString();
                    agrupamento.Evento.DisponivelVenda = (dr["Publicar"].ToString() == "T");

                    ApresentacaoSimples apresentacao = new ApresentacaoSimples();
                    SetorSimples setor = new SetorSimples();

                    do
                    {
                        if (agrupamento.Evento.Apresentacoes.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32()).Count() == 0)
                        {
                            apresentacao = new ApresentacaoSimples()
                            {
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                                HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM \\de yyyy"),
                                Dia = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd"),
                            };

                            agrupamento.Evento.Apresentacoes.Add(apresentacao);
                        }

                        if (apresentacao.Setores.Where(c => c.GrupoID == dr["GrupoID"].ToInt32()).Count() == 0)
                        {
                            setor = new SetorSimples()
                            {
                                SetorID = dr["SetorID"].ToInt32(),
                                Nome = dr["NomeParadise"].ToString(),
                                LugarTipo = dr["LugarMarcado"].ToString(),
                                GrupoID = dr["GrupoID"].ToInt32(),
                                QtdeDisponivel = dr["QtdeDisponivel"].ToInt32(),
                            };
                            apresentacao.Setores.Add(setor);
                        }

                        setor.Precos.Add(new PrecoSimples()
                        {
                            SetorID = dr["SetorID"].ToInt32(),
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Nome = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal(),
                        });

                    } while (dr.Read());
                }
                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        private AgrupamentoVendaSimples CarregarPacotes(AgrupamentoVendaSimples agrupamento, int eventoID)
        {
            try
            {
                string strSql = "sp_getPacotes3 " + eventoID;

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (!dr.Read())
                        return agrupamento;

                    PacoteSimples pacote = new PacoteSimples();
                    do
                    {

                        if (agrupamento.Pacotes.Where(c => c.PacoteID == dr["PacoteID"].ToInt32()).Count() == 0)
                        {
                            pacote = new PacoteSimples()
                            {
                                PacoteID = dr["IR_PacoteID"].ToInt32(),
                                Nome = dr["PacoteNome"].ToString()
                            };
                            agrupamento.Pacotes.Add(pacote);
                        }

                        pacote.Valor += dr["Valor"].ToDecimal() * dr["Quantidade"].ToInt32();

                    } while (dr.Read());
                }

                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<EstruturaPrecoApresentacaoEvento> CarregaListaPrecos(int EventoID)
        {
            try
            {
                List<EstruturaPrecoApresentacaoEvento> retorno = new List<EstruturaPrecoApresentacaoEvento>();

                string strSql = string.Format(@"SELECT Evento.Nome AS Evento,
                SUBSTRING(Horario,7,2) + '/' + SUBSTRING(Horario,5,2) + '/' + SUBSTRING(Horario,0,5) Apresentacao,
                Setor.Nome AS Setor,  
                Preco.Nome AS Preco, Valor
                FROM Evento(NOLOCK)
                INNER JOIN Apresentacao(NOLOCK) ON Apresentacao.EventoID = Evento.IR_EventoID
                INNER JOIN Setor(NOLOCK) ON Setor.ApresentacaoID = Apresentacao.IR_ApresentacaoID
                INNER JOIN Preco(NOLOCK) ON Preco.ApresentacaoID = Apresentacao.IR_ApresentacaoID AND Preco.SetorID = Setor.IR_SetorID
                WHERE EventoID =  {0}", EventoID);

                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(new EstruturaPrecoApresentacaoEvento()
                        {
                            Nome = dr["Evento"].ToString(),
                            Apresentacao = dr["Apresentacao"].ToString(),
                            Setor = dr["Setor"].ToString(),
                            Preco = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal().ToString("c")
                        });
                    }
                }

                return retorno;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public AgrupamentoVendaSimples CarregarBondinho(int eventoID, string horario, string idioma)
        {
            try
            {
                CultureInfo ci = new CultureInfo(idioma);

                var stb = @"SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, e.Publicar, 
                ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, 
                s.IR_SetorID AS SetorID, s.Nome AS Setor, s.LugarMarcado, 
                p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor, sp.Nome AS Hora  
                FROM Evento e (NOLOCK) 
                INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID 
                INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID 
                INNER JOIN SetoresParadise sp (NOLOCK) ON sp.SetorID = s.IR_SetorID 
                INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID 
                WHERE e.IR_EventoID = " + eventoID + @" AND p.Pacote = 0 AND p.Serie = 0 AND s.LugarMarcado = 'P' 
                AND ap.Horario LIKE '" + horario + @"%' 
                ORDER BY sp.GrupoID, ap.Horario ASC, s.Nome, p.Nome ";

                AgrupamentoVendaSimples agrupamento = new AgrupamentoVendaSimples();

                using (IDataReader dr = oDAL.SelectToIDataReader(stb))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações desta data do evento.");

                    agrupamento.Evento.EventoID = dr["EventoID"].ToInt32();
                    agrupamento.Evento.Nome = dr["Evento"].ToString();
                    agrupamento.Evento.DisponivelVenda = dr["Publicar"].ToString() == "T";

                    ApresentacaoSimples apresentacao = new ApresentacaoSimples();
                    SetorSimples setor = new SetorSimples();

                    do
                    {
                        string horaformatada = string.Empty;

                        if (idioma == "en-US")
                            horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, MMMM d, yyyy", ci);
                        else if (idioma == "es-ES")
                            horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, dd \\de MMMM yyyy", ci);
                        else if (idioma == "pt-BR")
                            horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, dd \\de MMMM \\de yyyy", ci);

                        if (agrupamento.Evento.Apresentacoes.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32() && c.Dia == dr["Hora"].ToString()).Count() == 0)
                        {
                            apresentacao = new ApresentacaoSimples()
                            {
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci),
                                HorarioFormatado = horaformatada,
                                Dia = dr["Hora"].ToString()
                            };

                            agrupamento.Evento.Apresentacoes.Add(apresentacao);
                        }

                        if (apresentacao.Setores.Where(c => c.SetorID == dr["SetorID"].ToInt32()).Count() == 0)
                        {
                            setor = new SetorSimples()
                            {
                                SetorID = dr["SetorID"].ToInt32(),
                                Nome = dr["Setor"].ToString().Trim(),
                                LugarTipo = dr["LugarMarcado"].ToString(),
                                Horario = dr["Hora"].ToString(),
                            };
                            apresentacao.Setores.Add(setor);
                        }

                        setor.Precos.Add(new PrecoSimples()
                        {
                            SetorID = dr["SetorID"].ToInt32(),
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Nome = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal(),
                        });

                    } while (dr.Read());
                }
                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public AgrupamentoVendaSimples CarregarCorcovado(int eventoID, string horario, string idioma)
        {
            try
            {
                CultureInfo ci = new CultureInfo(idioma);

                var stb = @"SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, e.Publicar, 
                ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, 
                s.IR_SetorID AS SetorID, s.Nome AS Setor, s.LugarMarcado, 
                p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor, sp.Nome AS Hora  
                FROM Evento e (NOLOCK) 
                INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID 
                INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID 
                INNER JOIN SetoresParadise sp (NOLOCK) ON sp.SetorID = s.IR_SetorID 
                INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID 
                WHERE e.IR_EventoID = " + eventoID + @" AND p.Pacote = 0 AND p.Serie = 0 AND s.LugarMarcado = 'P' 
                AND ap.Horario LIKE '" + horario + @"%' 
                ORDER BY sp.GrupoID, ap.Horario ASC, s.Nome, p.Nome ";

                AgrupamentoVendaSimples agrupamento = new AgrupamentoVendaSimples();

                using (IDataReader dr = oDAL.SelectToIDataReader(stb))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações desta data do evento.");

                    agrupamento.Evento.EventoID = dr["EventoID"].ToInt32();
                    agrupamento.Evento.Nome = dr["Evento"].ToString();
                    agrupamento.Evento.DisponivelVenda = dr["Publicar"].ToString() == "T";

                    ApresentacaoSimples apresentacao = new ApresentacaoSimples();
                    SetorSimples setor = new SetorSimples();

                    do
                    {
                        if (agrupamento.Evento.Apresentacoes.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32()).Count() == 0)
                        {
                            string horaformatada = string.Empty;

                            if (idioma == "en-US")
                                horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, MMMM d, yyyy", ci);
                            else if (idioma == "es-ES")
                                horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, dd \\de MMMM yyyy", ci);
                            else if (idioma == "pt-BR")
                                horaformatada = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci)).ToString("dddd, dd \\de MMMM \\de yyyy", ci);

                            apresentacao = new ApresentacaoSimples()
                            {
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", ci),
                                HorarioFormatado = horaformatada,
                                Dia = dr["Hora"].ToString()
                            };

                            agrupamento.Evento.Apresentacoes.Add(apresentacao);
                        }

                        if (apresentacao.Setores.Where(c => c.SetorID == dr["SetorID"].ToInt32()).Count() == 0)
                        {
                            setor = new SetorSimples()
                            {
                                SetorID = dr["SetorID"].ToInt32(),
                                Nome = dr["Setor"].ToString().Trim(),
                                LugarTipo = dr["LugarMarcado"].ToString(),
                                Horario = dr["Hora"].ToString(),
                            };
                            apresentacao.Setores.Add(setor);
                        }

                        setor.Precos.Add(new PrecoSimples()
                        {
                            SetorID = dr["SetorID"].ToInt32(),
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Nome = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal(),
                        });

                    } while (dr.Read());
                }
                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<EventoSimples> CarregarFlip(int localID)
        {
            try
            {
                var stb = @"SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, e.Publicar, 
                ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, 
                s.IR_SetorID AS SetorID, s.Nome AS Setor, s.LugarMarcado, s.QtdeDisponivel,
                p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor
                FROM Evento e (NOLOCK) 
                INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID 
                INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID 
                INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID 
                WHERE e.LocalID = " + localID + @" AND p.Pacote = 0 AND p.Serie = 0 AND s.LugarMarcado = 'P' AND e.IR_EventoID <> 28226
                ORDER BY ap.Horario ASC, s.Nome, p.Nome ";

                List<EventoSimples> agrupamento = new List<EventoSimples>();

                int ContEvento = 0;

                using (IDataReader dr = oDAL.SelectToIDataReader(stb))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar as informações desta data do evento.");

                    EventoSimples evento = new EventoSimples();
                    ApresentacaoSimples apresentacao = new ApresentacaoSimples();
                    SetorSimples setor = new SetorSimples();

                    do
                    {
                        if (agrupamento.Where(c => c.EventoID == dr["EventoID"].ToInt32()).Count() == 0)
                        {
                            evento = new EventoSimples()
                            {
                                EventoID = dr["EventoID"].ToInt32(),
                                Nome = dr["Evento"].ToString().Trim(),
                                DisponivelVenda = dr["Publicar"].ToString() == "T",
                            };
                            agrupamento.Add(evento);
                        }

                        if (evento.Apresentacoes.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32()).Count() == 0)
                        {
                            apresentacao = new ApresentacaoSimples()
                            {
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                                HorarioFormatado = (DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dddd, dd \\de MMMM \\de yyyy"),
                                Dia = (++ContEvento).ToString()
                            };

                            evento.Apresentacoes.Add(apresentacao);
                        }

                        if (apresentacao.Setores.Where(c => c.SetorID == dr["SetorID"].ToInt32()).Count() == 0)
                        {
                            setor = new SetorSimples()
                            {
                                SetorID = dr["SetorID"].ToInt32(),
                                Nome = dr["Setor"].ToString().Trim(),
                                LugarTipo = dr["LugarMarcado"].ToString(),
                                QtdeDisponivel = dr["QtdeDisponivel"].ToInt32()
                            };
                            apresentacao.Setores.Add(setor);
                        }

                        setor.Precos.Add(new PrecoSimples()
                        {
                            SetorID = dr["SetorID"].ToInt32(),
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Nome = dr["Preco"].ToString().Trim(),
                            Valor = dr["Valor"].ToDecimal(),
                        });

                    } while (dr.Read());
                }
                return agrupamento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public bool LiberarEventos(List<int> EventoIDs)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("UPDATE Evento ");
                stb.Append("SET Publicar='T', PublicarSemVendaMotivo=0 ");
                stb.AppendFormat("WHERE ID in ({0});", String.Join(",",EventoIDs));

                return oDAL.Execute(stb.ToString()) > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<EventoSimples> CarregarOutrosEventosLocal(int LocalID)
        {
            try
            {
                StringBuilder stb = new StringBuilder();
                stb.Append("SELECT e.IR_EventoID AS EventoID, e.Nome AS Evento, MIN(a.Horario) AS Primeira, MAX(a.Horario) AS Ultima ");
                stb.Append("FROM Evento e (NOLOCK) ");
                stb.Append("INNER JOIN Apresentacao a (NOLOCK) ON a.EventoID = e.IR_EventoID ");
                stb.AppendFormat("WHERE e.LocalID = {0}", LocalID);
                stb.Append("GROUP BY e.IR_EventoID, e.Nome ");

                List<EventoSimples> evento = new List<EventoSimples>();

                using (IDataReader dr = oDAL.SelectToIDataReader(stb.ToString()))
                {
                    while (dr.Read())
                    {
                        evento.Add(new EventoSimples()
                        {
                            EventoID = dr["EventoID"].ToInt32(),
                            Nome = Util.LimparTitulo(dr["Evento"].ToString()),
                            PrimeiraApresentacao = (DateTime.ParseExact(dr["Primeira"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd \\/ MM \\/ yyyy"),
                            UltimaApresentacao = (DateTime.ParseExact(dr["Ultima"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd \\/ MM \\/ yyyy")
                        });
                    }
                }

                return evento;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

    }
}