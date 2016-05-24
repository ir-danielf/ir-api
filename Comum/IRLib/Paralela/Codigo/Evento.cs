/**************************************************
* Arquivo: Evento.cs
* Gerado: quarta-feira, 6 de abril de 2005k
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{

    public class Dado : MarshalByRefObject
    {
        // São publics de propósito. Eram structs e não havia necessidade de criar Propriedades.
        public int eventoID;
        public int apresentacaoID;
        public int setorID;
        public int apresentacaoSetorID;
        public int qtd;
        public DateTime horario;
        public int empresaID;
        public int localID;
        public string setorNome;
        public List<string> codigosBarra;

        public Dado(int eventoID, int apresentacaoID, int setorID, int apresentacaoSetorID, DateTime horario, int empresaID, int localID, int qtd, List<string> codigosBarra)
        {//, string setorNome)
            this.eventoID = eventoID;
            this.apresentacaoID = apresentacaoID;
            this.setorID = setorID;
            this.apresentacaoSetorID = apresentacaoSetorID;
            this.qtd = qtd;
            this.horario = horario;
            this.empresaID = empresaID;
            this.localID = localID;
            this.setorNome = string.Empty;
            this.codigosBarra = codigosBarra;
        }
    }

    public class EstruturaEvento : MarshalByRefObject
    {
        ArrayList lista = new ArrayList();

        public int Tamanho
        {
            get
            {
                return this.lista.Count;
            }
        }

        public void Adicionar(Dado item)
        {
            this.lista.Add(item);
        }

        public Dado this[int indice]
        {
            get
            {
                return (Dado)this.lista[indice];
            }
            set
            {
                this.lista[indice] = value;
            }
        }

    }

    public class Evento : Evento_B
    {
        #region Métodos para GridView Financeiro (Renato)
        public DataTable SelectEventosFinanceiro()
        {
            BD bd = new BD();
            DataTable dt = new DataTable();
            dt.Load(bd.Consulta("SELECT ID, Nome, Financeiro FROM tEvento ORDER BY Nome"));
            bd.FecharConsulta();
            DataTable dtRetorno = new DataTable();
            dtRetorno.Columns.Add("ID", typeof(int));
            dtRetorno.Columns.Add("Nome", typeof(string));
            dtRetorno.Columns.Add("Financeiro", typeof(bool));
            foreach (DataRow drow in dt.Rows)
            {
                DataRow novalinha = dtRetorno.NewRow();
                novalinha["ID"] = drow["ID"];
                novalinha["Nome"] = drow["Nome"];
                novalinha["Financeiro"] = ((string)drow["Financeiro"] == "T");
                dtRetorno.Rows.Add(novalinha);
            }
            return dtRetorno;
        }
        public DataTable SelectEventosFinanceiroTrue()
        {
            BD bd = new BD();
            DataTable dt = new DataTable();
            dt.Load(bd.Consulta("SELECT ID, Nome, Financeiro FROM tEvento WHERE Financeiro='T' ORDER BY Nome"));
            bd.FecharConsulta();
            DataTable dtRetorno = new DataTable();
            dtRetorno.Columns.Add("ID", typeof(int));
            dtRetorno.Columns.Add("Nome", typeof(string));
            dtRetorno.Columns.Add("Financeiro", typeof(bool));
            foreach (DataRow drow in dt.Rows)
            {
                DataRow novalinha = dtRetorno.NewRow();
                novalinha["ID"] = drow["ID"];
                novalinha["Nome"] = drow["Nome"];
                novalinha["Financeiro"] = ((string)drow["Financeiro"] == "T");
                dtRetorno.Rows.Add(novalinha);
            }
            return dtRetorno;
        }
        public DataTable SelectEventosFinanceiroFalse()
        {
            BD bd = new BD();
            DataTable dt = new DataTable();
            dt.Load(bd.Consulta("SELECT ID, Nome, Financeiro FROM tEvento WHERE Financeiro='F' ORDER BY Nome"));
            bd.FecharConsulta();
            DataTable dtRetorno = new DataTable();
            dtRetorno.Columns.Add("ID", typeof(int));
            dtRetorno.Columns.Add("Nome", typeof(string));
            dtRetorno.Columns.Add("Financeiro", typeof(bool));
            foreach (DataRow drow in dt.Rows)
            {
                DataRow novalinha = dtRetorno.NewRow();
                novalinha["ID"] = drow["ID"];
                novalinha["Nome"] = drow["Nome"];
                novalinha["Financeiro"] = ((string)drow["Financeiro"] == "T");
                dtRetorno.Rows.Add(novalinha);
            }
            return dtRetorno;
        }
        public void UpdateEventosFinanceiro(int ID, bool Financeiro)
        {
            BD bd = new BD();
            bd.Executar("UPDATE tEvento SET Financeiro = '" + (Financeiro ? "T" : "F") + "' WHERE ID = " + ID);
        }
        #endregion

        public bool ValidarSetores { get; set; }
        public Obrigatoriedade Obrigatoriedade { get; set; }

        public enum PublicarTipo
        {
            [System.ComponentModel.Description("Não publicado")]
            NaoPublicado = 'F',
            [System.ComponentModel.Description("Publicado para venda")]
            PublicadoParaVenda = 'T',
            [System.ComponentModel.Description("Publicado sem venda")]
            PublicadoSemVenda = 'S'
        }

        public enum EnumObrigaCadastroCliente
        {
            [System.ComponentModel.Description("Cadastro Não Obrigatório")]
            CADASTRO_NAO_OBRIGATORIO = 'F',
            [System.ComponentModel.Description("Cadastro Para Entrega")]
            CADASTRO_ENTREGA = 'E',
            [System.ComponentModel.Description("Cadastro Básico")]
            CADASTRO_BASICO = 'B',
            [System.ComponentModel.Description("Cadastro Com ID Única")]
            CADASTRO_ID_UNICA = 'U',
            [System.ComponentModel.Description("Cadastro Com Propriedades Fora do Padrão")]
            CADASTRO_FORA_PADRAO = 'P'

        }
        public enum RetornoProcedureSalvar
        {
            [System.ComponentModel.Description("Inserido com sucesso")]
            INSERIDO_COM_SUCESSO = 1,
            [System.ComponentModel.Description("Atualizado com sucesso")]
            ATUALIZADO_COM_SUCESSO = 2,
            [System.ComponentModel.Description("Alteração de cadastro não permitida")]
            TENTATIVA_ALTERACAO_CONTRATO = 3
        }
        public enum SemVendaMotivo
        {
            [System.ComponentModel.Description("Vendas online")]
            VendaDisponivel = 0,
            [System.ComponentModel.Description("Venda online não disponível")]
            VendaOnlineNaoDisponivel = 1,
            [System.ComponentModel.Description("Venda somente no callcenter")]
            VendaSomenteCallCenter = 2,
            [System.ComponentModel.Description("Vendas encerradas")]
            VendasEncerradas = 3,
            [System.ComponentModel.Description("Vendas não iniciadas")]
            VendasNaoIniciadas = 4,
            [System.ComponentModel.Description("Venda disponível apenas para pacotes")]
            VendaDisponivelApenasParaPacotes = 5,
            [System.ComponentModel.Description("Vendas apenas a partir da data selecionada")]
            VendaDisponivelDeterminadaData = 6
        }



        public const string BaseCalculo_DataApresentacao = "D";
        public const string BaseCalculo_ProximoDomingo = "P";


        public const string TipoCalculoDesconto_DescontoProporcional = "D";
        public const string TipoCalculoDesconto_DinheiroDebitoCredito = "P";


        public const string TipoCalculo_Semana = "S";
        public const string TipoCalculo_Avulso = "A";



        public enum NivelRiscoEvento
        {
            SemRisco = 0,
            ComRisco = 1
        }

        public DataTable PegarEventosInternet(string filtro)
        {
            BD bd = new BD();

            DataTable eventos = new DataTable("Eventos");
            eventos.Columns.Add("ID", typeof(int));
            eventos.Columns.Add("Nome", typeof(string));
            eventos.Columns.Add("Tipo", typeof(string));
            eventos.Columns.Add("Publicar", typeof(string));
            eventos.Columns.Add("Destaque", typeof(bool));
            eventos.Columns.Add("EntregaGratuita", typeof(bool));
            eventos.Columns.Add("RetiradaBilheteria", typeof(bool));
            eventos.Columns.Add("Parcelas", typeof(int));
            eventos.Columns.Add("PalavraChave", typeof(string));

            string sql = "" +
                "SELECT e.ID, e.Nome, et.Nome Tipo, " +
                "CASE " +
                "  WHEN e.Publicar = 'T' THEN 'Publicado para venda' " +
                "  WHEN e.Publicar = 'F' THEN 'Não publicado' " +
                "  ELSE 'Publicado sem venda' " +
                "END AS Publicar, e.Destaque, e.EntregaGratuita, e.RetiradaBilheteria, e.Parcelas, e.PalavraChave " +
                "FROM tEvento e, tEventoTipo et " +
                "WHERE e.EventoTipoID = et.ID AND e.ID IN (SELECT DISTINCT EventoID FROM vwInfoVendaInternet) " +
                "AND " + (filtro.Trim() == string.Empty ? "1=1" : filtro);
            DataRow dr;
            while (bd.Consulta(sql).Read())
            {
                dr = eventos.NewRow();
                dr["ID"] = bd.LerInt("ID");
                dr["Nome"] = bd.LerString("Nome");
                dr["Tipo"] = bd.LerString("Tipo");
                dr["Publicar"] = bd.LerString("Publicar");
                dr["Destaque"] = bd.LerBoolean("Destaque");
                dr["EntregaGratuita"] = bd.LerBoolean("EntregaGratuita");
                dr["RetiradaBilheteria"] = bd.LerBoolean("RetiradaBilheteria");
                dr["Parcelas"] = bd.LerInt("Parcelas");
                dr["PalavraChave"] = bd.LerString("PalavraChave");
                eventos.Rows.Add(dr);
            }

            return eventos;

        }

        public bool VerificaIngressoAgrupado()
        {
            try
            {
                int grupo = 0, classificacao = 0;
                StringBuilder sql = new StringBuilder(@"SELECT TOP 1 EventoID, Grupo, Classificacao FROM tIngresso (NOLOCK) WHERE EventoID = @001");
                sql.Replace("@001", this.Control.ID.ToString());
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    grupo = bd.LerInt("Grupo");
                    classificacao = bd.LerInt("Classificacao");
                }
                return ((grupo == 0) || (classificacao == 0));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Atualiza o evento
        /// kim
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {
                bd.IniciarTransacao();
                if (this.Obrigatoriedade != null)
                {
                    int obrigatoriedadeID = Convert.ToInt32(this.ObrigatoriedadeID.ValorBD);
                    if (obrigatoriedadeID != 0)
                    {
                        this.Obrigatoriedade.Control.ID = Convert.ToInt32(this.ObrigatoriedadeID.ValorBD);
                        this.Obrigatoriedade.Atualizar(bd);
                    }
                    else
                    {
                        this.Obrigatoriedade.Inserir(bd);
                        this.ObrigatoriedadeID.Valor = this.Obrigatoriedade.Control.ID;
                    }
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("   EXEC dbo.salvar_tEvento_Producao @UsuarioID = " + this.Control.UsuarioID.ToString() + ", @EventoID = @ID, @TimeStamp ='" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "', " +
                               "@LocalID = @001, @Nome = @003, @VendaDistribuida = @004, @VersaoImagemIngresso = @005, " +
                               "@VersaoImagemVale = @006, @VersaoImagemVale2 = @007, @VersaoImagemVale3 = @008, @ImpressaoCodigoBarra = @009, @ObrigaCadastroCliente = @010, " +
                               "@DesabilitaAutomatico = @011, @Resenha = @012, @Publicar = @013, @Destaque = @014, @PrioridadeDestaque = @015, @ImagemInternet = @016, " +
                               "@Parcelas = @017, @EntregaGratuita = @018, @RetiradaBilheteria = @019, @Financeiro = @020, @Atencao = @021, @Censura = @022, " +
                               "@EntradaAcompanhada = @023, @PDVSemConveniencia = @024, @RetiradaIngresso = @025, @MeiaEntrada = @026, @Promocoes = @027, " +
                               "@AberturaPortoes = @028, @DuracaoEvento = @029, @Release = @030, @DescricaoPadraoApresentacao = @031, @PublicarSemVendaMotivo = @032, " +
                               "@ContratoID = @033, @PermitirVendaSemContrato = @034,  @DataAberturaVenda = '@036', @EventoSubtipoID = @037, " +
                               "@ObrigatoriedadeID = @038, @EscolherLugarMarcado = '@039', @MapaEsquematicoID = '@040', @PalavraChave = @041, " +
                               "@ExibeQuantidade = @042 , @NivelRisco = @043, @HabilitarRetiradaTodosPDV = '@044', @TipoImpressao = '@045', @TipoCodigoBarra = '@046' , @ImagemDestaque = '@047', @CodigoPos = @048, " +
                               "@BaseCalculo = @049 , @TipoCalculoDesconto = @050 , @TipoCalculo = @051 , @AVCB = @052, @VendaSemAlvara = @053, @FonteImposto = @054, @PorcentagemImposto = @055, @Alvara = @056, " +
                               "@DataEmissaoAlvara = @057, @DataValidadeAlvara = @058, @DataEmissaoAvcb = @059, @DataValidadeAvcb = @060, @Lotacao = @061");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@003", "'" + this.Nome.ValorBD + "'");
                sql.Replace("@004", "'" + this.VendaDistribuida.ValorBD + "'");
                sql.Replace("@005", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@009", "'" + this.ImpressaoCodigoBarra.ValorBD + "'");
                sql.Replace("@010", "'" + this.ObrigaCadastroCliente.ValorBD + "'");
                sql.Replace("@011", "'" + this.DesabilitaAutomatico.ValorBD + "'");
                sql.Replace("@012", "'" + this.Resenha.ValorBD + "'");
                sql.Replace("@013", "'" + this.Publicar.ValorBD + "'");
                sql.Replace("@014", "'" + this.Destaque.ValorBD + "'");
                sql.Replace("@015", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@016", "'" + this.ImagemInternet.ValorBD + "'");
                sql.Replace("@017", this.Parcelas.ValorBD);
                sql.Replace("@018", "'" + this.EntregaGratuita.ValorBD + "'");
                sql.Replace("@019", "'" + this.RetiradaBilheteria.ValorBD + "'");
                sql.Replace("@020", "'" + this.Financeiro.ValorBD + "'");
                sql.Replace("@021", "'" + this.Atencao.ValorBD + "'");
                sql.Replace("@022", "'" + this.Censura.ValorBD + "'");
                sql.Replace("@023", "'" + this.EntradaAcompanhada.ValorBD + "'");
                sql.Replace("@024", "'" + this.PDVSemConveniencia.ValorBD + "'");
                sql.Replace("@025", "'" + this.RetiradaIngresso.ValorBD + "'");
                sql.Replace("@026", "'" + this.MeiaEntrada.ValorBD + "'");
                sql.Replace("@027", "'" + this.Promocoes.ValorBD + "'");
                sql.Replace("@028", "'" + this.AberturaPortoes.ValorBD + "'");
                sql.Replace("@029", "'" + this.DuracaoEvento.ValorBD + "'");
                sql.Replace("@030", "'" + this.Release.ValorBD + "'");
                sql.Replace("@031", "'" + this.DescricaoPadraoApresentacao.ValorBD + "'");
                sql.Replace("@032", "'" + this.PublicarSemVendaMotivo.ValorBD + "'");
                sql.Replace("@033", this.ContratoID.ValorBD);
                sql.Replace("@034", "'" + this.PermitirVendaSemContrato.ValorBD + "'");
                sql.Replace("@036", Convert.ToInt32(PublicarSemVendaMotivo.Valor) == Convert.ToInt32(Evento.SemVendaMotivo.VendaDisponivelDeterminadaData) ? this.DataAberturaVenda.ValorBD : String.Empty);
                sql.Replace("@037", this.EventoSubTipoID.ValorBD);
                sql.Replace("@038", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@039", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@040", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@041", "'" + this.PalavraChave.ValorBD + "'");
                sql.Replace("@042", "'" + this.ExibeQuantidade.ValorBD + "'");
                sql.Replace("@043", this.NivelRisco.ValorBD);
                sql.Replace("@044", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@045", this.TipoImpressao.ValorBD);
                sql.Replace("@046", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@047", this.ImagemDestaque.ValorBD);
                sql.Replace("@048", this.CodigoPos.ValorBD);

                sql.Replace("@049", "'" + this.BaseCalculo.ValorBD + "'");
                sql.Replace("@050", "'" + this.TipoCalculoDesconto.ValorBD + "'");
                sql.Replace("@051", "'" + this.TipoCalculo.ValorBD + "'");

                sql.Replace("@052", "'" + this.AVCB.ValorBD + "'");
                sql.Replace("@053", "'" + this.VendaSemAlvara.ValorBD + "'");
                sql.Replace("@054", "'" + this.FonteImposto.ValorBD + "'");
                sql.Replace("@055", this.PorcentagemImposto.ValorBD);
                sql.Replace("@056", "'" + this.Alvara.ValorBD + "'");

                sql.Replace("@057", "'" + this.DataEmissaoAlvara.ValorBD + "'");
                sql.Replace("@058", "'" + this.DataValidadeAlvara.ValorBD + "'");
                sql.Replace("@059", "'" + this.DataEmissaoAvcb.ValorBD + "'");
                sql.Replace("@060", "'" + this.DataValidadeAvcb.ValorBD + "'");
                sql.Replace("@061", "'" + this.Lotacao.ValorBD + "'");

                int x = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                if (x == (int)RetornoProcedureSalvar.TENTATIVA_ALTERACAO_CONTRATO)
                    throw new Exception("3 - Alteração de contrato não permitida");
                bd.FinalizarTransacao();

                return (x == (int)RetornoProcedureSalvar.ATUALIZADO_COM_SUCESSO);
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Função para verificar se todos os eventos dos ingressos ou
        ///  possuem contrato vinculado ou possuem o campo PermitirVendaSemContrato = 'T'
        /// </summary>
        /// <param name="ingressosID"></param>
        /// <returns></returns>
        public bool VerificaContratoEventoPorIngresso(List<int> ingressosID)
        {
            try
            {
                if (ingressosID.Count == 0)
                    throw new Exception("Falha ao verificar contrato do evento. A lista de ingressos está vazia");

                StringBuilder sql = new StringBuilder("CREATE TABLE #temp (IngressoID INT);");

                //para cada ingresso inclue o ingressoID em tabela temporaria para join futuro
                foreach (int ingressoID in ingressosID)
                {
                    sql.Append("INSERT INTO #temp (IngressoID) VALUES (" + ingressoID + ");");
                }
                sql.Append("SELECT e.PermitirVendaSemContrato,e.ContratoID FROM tIngresso (NOLOCK) " +
                           "INNER JOIN #temp ON #temp.IngressoID = tIngresso.ID " +
                           "INNER JOIN tEvento e (NOLOCK) ON e.ID = tIngresso.EventoID;");

                bd.Consulta(sql);

                int contratoID = 0;
                bool permitirVendaSemContrato = false;
                while (bd.Consulta().Read())
                {
                    contratoID = bd.LerInt("ContratoID");
                    permitirVendaSemContrato = bd.LerBoolean("PermitirVendaSemContrato");

                    if (!permitirVendaSemContrato)
                        if (contratoID == 0)
                            return false;
                }
                return true;


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Verifica na tIngressoLog se o evento possui alguma venda 
        /// </summary>
        /// <param name="eventoID"></param>
        /// <returns></returns>
        public bool PossuiMovimentacaoFinanceir(int eventoID)
        {
            try
            {
                string sql = @"SELECT COUNT(tIngressoLog.ID) FROM tIngresso (NOLOCK)
                            INNER JOIN tIngressoLog (NOLOCK) ON tIngressoLog.IngressoID= tIngresso.ID
                            WHERE EventoID = " + eventoID;

                object ok = bd.ConsultaValor(sql);

                return Convert.ToInt32(ok) > 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataTable FormasPagamentoPorEvento(int eventoID)
        {
            /// Job: 202
            /// Autor: LP
            DataTable dados = new DataTable();
            dados.Columns.Add(new DataColumn("ID", typeof(int)));
            dados.Columns.Add(new DataColumn("Nome", typeof(string)));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tFormaPagamento.ID, " +
                    "   tFormaPagamento.Nome " +
                    "FROM tFormaPagamento (NOLOCK) " +
                    "INNER JOIN tFormaPagamentoEvento (NOLOCK) ON tFormaPagamentoEvento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE " +
                    "   (tFormaPagamentoEvento.EventoID = '" + eventoID + "') " +
                    "ORDER BY " +
                    "   tFormaPagamento.Nome"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow oDataRow = dados.NewRow();
                        oDataRow["ID"] = bd.LerInt("ID");
                        oDataRow["Nome"] = bd.LerString("Nome");
                        dados.Rows.Add(oDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return dados;
        }

        public void GerarCodigos(int EventoID, double porcentagem, int minimoCodigoBarra)
        {
            try
            {
                bd.IniciarTransacao();

                Ingresso oIngresso = new Ingresso();
                CodigoBarra oCodigoBarra = new CodigoBarra();
                CodigoBarraEvento oCodigoBarraEvento = new CodigoBarraEvento();
                List<string> CodigosBarra = new List<string>();

                int quantidade = 0;

                DataTable info = this.ListaApresentacaoQuantidade(EventoID);

                foreach (DataRow dtrAPS in info.Rows)
                {
                    quantidade += Math.Max(Convert.ToInt32(dtrAPS["SetorQtde"]), minimoCodigoBarra);
                }

                quantidade += (int)Math.Round(quantidade * (porcentagem / 100));
                CodigosBarra = oCodigoBarra.BuscarListaBranca(bd, quantidade);

                foreach (DataRow dtrAPS in info.Rows)
                {
                    int qtdCodigosAPS = Math.Max(Convert.ToInt32(dtrAPS["SetorQtde"]) + (int)(Convert.ToInt32(dtrAPS["SetorQtde"]) * (porcentagem / 100)), minimoCodigoBarra);

                    List<string> codigosApresentacaoSetor = new List<string>();
                    codigosApresentacaoSetor = CodigosBarra.Take(qtdCodigosAPS).ToList();
                    CodigosBarra.RemoveAll(c => codigosApresentacaoSetor.Contains(c));

                    oIngresso.AtualizarCodigoBarra((int)dtrAPS["ApresentacaoSetor"], codigosApresentacaoSetor, EventoID);

                    oCodigoBarraEvento.GerarCodigos((int)dtrAPS["ApresentacaoSetor"], codigosApresentacaoSetor, DateTime.Now, EventoID, this.bd);
                }
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable ListaApresentacaoQuantidade(int eventoID)
        {
            DataTable dados = new DataTable();

            dados.Columns.Add(new DataColumn("ApresentacaoSetor", typeof(int)));
            dados.Columns.Add(new DataColumn("SetorQtde", typeof(string)));

            try
            {
                using (IDataReader oDataReader = bd.Consulta(@"SELECT tApresentacaoSetor.ID, COUNT(tIngresso.ID) as Quantidade 
                FROM tApresentacaoSetor
                INNER JOIN tApresentacao ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID
                INNER JOIN tEvento ON tEvento.ID = tApresentacao.EventoID
                INNER JOIN tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID
                WHERE tEvento.ID = " + eventoID + " GROUP BY tApresentacaoSetor.ID"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow oDataRow = dados.NewRow();
                        oDataRow["ApresentacaoSetor"] = bd.LerInt("ID");
                        oDataRow["SetorQtde"] = bd.LerInt("Quantidade");
                        dados.Rows.Add(oDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return dados;
        }


        /// <summary>
        /// Recebe o LocalID e retorna um DataTable com o ID e Nome dos eventos.
        /// Para o Master Especial passar LocalID = 0
        /// </summary>
        /// <param name="localID"></param>
        /// <returns></returns>
        public DataTable EventoIDNome(int localID, bool? disponivelAjuste, bool? disponivelRelatorio, bool? disponivelVenda)
        {
            DataTable retorno = new DataTable();
            retorno.Columns.Add("ID", typeof(int));
            retorno.Columns.Add("Nome", typeof(string));

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT tEvento.ID,tEvento.Nome FROM tEvento (NOLOCK) ");
            sql.Append("INNER JOIN tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
            sql.Append("WHERE @001 @002 @003 @004 ORDER BY tEvento.Nome");

            switch (disponivelAjuste)
            {
                case true:
                    sql.Replace("@001", "tApresentacao.DisponivelAjuste='T'");
                    break;
                case false:
                    sql.Replace("@001", "tApresentacao.DisponivelAjuste='F'");
                    break;
                case null:
                    sql.Replace("@001", "");
                    break;
            }
            switch (disponivelRelatorio)
            {
                case true:
                    if (disponivelAjuste != null)
                        sql.Replace("@002", "AND tApresentacao.DisponivelRelatorio='T'");
                    else
                    {
                        sql.Replace("@002", "tApresentacao.DisponivelRelatorio='T'");
                    }
                    break;

                case false:
                    if (disponivelAjuste != null)
                        sql.Replace("@002", "AND tApresentacao.DisponivelRelatorio='F'");
                    else
                    {
                        sql.Replace("@002", "tApresentacao.DisponivelRelatorio='F'");
                    }
                    break;

                case null:
                    sql.Replace("@002", "");
                    break;
            }

            switch (disponivelVenda)
            {
                case true:
                    if (disponivelAjuste != null || disponivelRelatorio != null)
                        sql.Replace("@003", "AND tApresentacao.DisponivelVenda='T'");
                    else
                    {
                        sql.Replace("@003", "tApresentacao.DisponivelVenda='T'");
                    }
                    break;

                case false:
                    if (disponivelAjuste != null || disponivelRelatorio != null)
                        sql.Replace("@003", "AND tApresentacao.DisponivelVenda='F'");
                    else
                    {
                        sql.Replace("@003", "tApresentacao.DisponivelVenda='F'");
                    }
                    break;

                case null:
                    sql.Replace("@003", "");
                    break;
            }

            if (localID > 0)
            {
                if (disponivelAjuste != null || disponivelRelatorio != null || disponivelVenda != null)
                    sql.Replace("@004", "AND tEvento.LocalID = " + localID.ToString() + " ");
                else
                {
                    sql.Replace("@004", "tEvento.LocalID = " + localID.ToString() + " ");
                }
            }
            else
            {
                sql.Replace("@004", "");
            }
            if (localID == 0 && disponivelAjuste == null && disponivelRelatorio == null && disponivelVenda == null)
            {
                sql.Replace("WHERE", "");
            }
            try
            {

                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    retorno.Rows.Add(linha);
                }
                return retorno;
            }
            catch { throw; }
            finally
            {
                bd.Fechar();
            }



        }

        /// <summary>
        /// Retorna o nome do local e o nome do evento
        /// </summary>
        /// <param name="LocalID"></param>
        /// <param name="EventoID"></param>
        /// <returns></returns>
        public string[] EventoLocalNome(int eventoID)
        {
            string[] retorno = new string[2];
            try
            {
                string sql = "SELECT tEvento.Nome as EventoNome,tLocal.Nome as LocalNome " +
                             "FROM tEvento(nolock),tLocal(nolock) WHERE tEvento.LocalID = tLocal.ID " +
                             "AND tEvento.ID = " + eventoID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno[0] = bd.LerString("LocalNome");
                    retorno[1] = bd.LerString("EventoNome");
                }
                return retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna arvore com nome da Empresa,nome da Regional,nome do Local e nome do Evento
        /// dos setores pendentes
        /// </summary>
        /// <param name="EventoID"></param>
        /// <returns></returns>
        public string[] ArvoreSetorPendente(string eventoID)
        {
            string[] retorno = new string[4];
            try
            {
                string sql = @"SELECT tEmpresa.Nome as EmpresaNome, tRegional.Nome as RegionalNome, 
                                tLocal.Nome as LocalNome ,tEvento.Nome as EventoNome
                                FROM tEvento(nolock)
                                INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID
                                INNER JOIN tEmpresa ON tEmpresa.ID = tLocal.EmpresaID
                                INNER JOIN tRegional ON tRegional.ID = tEmpresa.RegionalID
                                WHERE tEvento.ID = " + eventoID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno[0] = bd.LerString("EmpresaNome");
                    retorno[1] = bd.LerString("RegionalNome");
                    retorno[2] = bd.LerString("LocalNome");
                    retorno[3] = bd.LerString("EventoNome");
                }
                return retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// preenche apenas o LocalID,EventoTipoID e Nome do Evento
        /// </summary>
        /// <param name="id"></param>
        public void LerLocalTipoNome(int id)
        {

            try
            {

                string sql = "SELECT ID,LocalID,Nome FROM tEvento (NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //public event ProgressoEventHandler Processando;

        private int UsuarioIDLogado;

        public bool PossuiEntrega(List<int> listaApresentacao, List<int> Eventos)
        {
            try
            {

                Apresentacao oApresentacao = new Apresentacao();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);

                string eventos = "";
                bool possui = false;
                foreach (int eventoID in Eventos)
                {
                    if (eventos.Length > 0)
                    {
                        eventos += "," + eventoID;
                    }
                    else
                    {
                        eventos += eventoID;
                    }
                }

                string sql = @"SELECT tEntregaControle.EntregaID
                        FROM tEventoEntregaControle
                        INNER JOIN tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        INNER JOIN tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEventoEntregaControle.EventoID IN (" + eventos + @")  AND tEntrega.Tipo IN ('A','N')  AND 
                        CONVERT(VARCHAR, DATEADD(day, (PrazoEntrega + 
			                        CASE WHEN
				                        tEventoEntregaControle.DiasTriagem > 0
					                        THEN tEventoEntregaControle.DiasTriagem
				                        ELSE
					                        CASE WHEN
						                        tEntregaControle.DiasTriagem > 0
						                        THEN tEntregaControle.DiasTriagem
						                        ELSE tEntrega.DiasTriagem
					                        END
				                        END  
		                        ), CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring('" + dataEventoMaisProximo.ToString("yyyyMMdd") + @"',0,9), 112)
                        GROUP BY tEntregaControle.EntregaID 
                        HAVING COUNT(Distinct EventoID) = " + Eventos.Count;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    possui = true;
                }

                bd.Fechar();


                return possui;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<EstruturaParceiroPortoSeguro> ListaEventoPorto(int parceiroID)
        {
            try
            {
                List<EstruturaParceiroPortoSeguro> lRetornoLista = new List<EstruturaParceiroPortoSeguro>();

                string sql = string.Format(@"
                  SELECT DISTINCT
                    e.IR_EventoID ID,
                    e.nome Evento,
                    ISNULL(tEventoTipo.Nome, '') AS Categoria,
                    tl.Nome Local,
                    tl.Cidade,
                    e.Imagem,
                    ta.id ApresentacaoID,
                    tas.SetorID SetorID,
                    CASE WHEN tp.Nome LIKE PrecoIniciaCom + '%'
                                 THEN Valor
                                 ELSE 0
                                END AS Porto,
                                Valor
                    INTO #eventos
                    From SiteIR..Evento e 
                    LEFT JOIN tEventoSubTipo tipo (NOLOCK) ON tipo.ID = e.SubtipoID
                    INNER JOIN tEventoTipo (NOLOCK) ON tEventoTipo.ID = tipo.EventoTipoID
                    INNER JOIN  tapresentacao ta (NOLOCK) ON ta.eventoID = e.IR_EventoID  
                    INNER JOIN  dbo.tApresentacaoSetor tas ON tas.ApresentacaoID = ta.ID   
                    INNER JOIN  dbo.tPreco tp   (NOLOCK) ON tp.ApresentacaoSetorID = tas.ID 
                    INNER JOIN  tcota tc (NOLOCK) ON tc.id = ta.CotaID
                    INNER JOIN  tcotaitem tci (NOLOCK) ON tci.CotaID = tc.ID
                    INNER JOIN  dbo.tLocal tl   (NOLOCK) ON tl.id = e.LocalID  

                    WHERE tci.parceiroid = {0} and e.IR_EventoID != {1} AND ta.DisponivelVenda = 'T'
  

    
                   SELECT ID, Evento, Categoria, Local, Cidade, Imagem,
                                CAST(ROUND((100-((MAX(Porto) * 100)/ MAX(Valor))),0) AS DECIMAL) AS Desconto
                                FROM #eventos
                                GROUP BY ID, Evento, Categoria, Local, Cidade, Imagem
                                HAVING MAX(Porto) > 0
                                ORDER BY Desconto
       
                                DROP TABLE #eventos
                ", parceiroID, Convert.ToInt32(ConfigurationManager.AppSettings["EventoExclude"]));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetornoLista.Add(new EstruturaParceiroPortoSeguro
                        {
                            EventoID = bd.LerInt("ID"),
                            NomeEvento = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(bd.LerString("Evento").ToLower()),
                            Local = bd.LerString("Local"),
                            Cidade = bd.LerString("Cidade"),
                            Categoria = bd.LerString("Categoria"),
                            Imagem = bd.LerString("Imagem"),
                            Desconto = bd.LerString("Desconto"),
                        });
                }
                bd.Fechar();
                return lRetornoLista;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEntregaAgenda> ListaEntrega(List<int> listaApresentacao, List<int> Eventos, string CEP)
        {
            try
            {
                Apresentacao oApresentacao = new Apresentacao();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);
                List<EstruturaEntregaAgenda> lRetorno = new List<EstruturaEntregaAgenda>();

                bd.BulkInsert(Eventos, "#tmpEventos", false, true);

                bd.Consulta("Exec ListaEntrega_Nova " + Eventos.Count + ",'" + CEP + "','" + dataEventoMaisProximo.ToString("yyyyMMdd000000") + "'");

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaEntregaAgenda
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        DiasTriagem = bd.LerInt("DiasTriagem"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega").Replace("<br/>", "\n\r").Replace("<br />", "\n\r"),
                        PrioridadeProcedimento = bd.LerInt("PrioridadeProcedimento"),
                        Tipo = bd.LerString("Tipo"),
                        Valor = bd.LerDecimal("Valor"),
                        Periodo = bd.LerString("Periodo"),
                    });
                }

                bd.Fechar();

                return lRetorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<EstruturaEntregaAgenda> ListaRetirada(List<int> listaApresentacao, List<int> Eventos)
        {
            try
            {
                Apresentacao oApresentacao = new Apresentacao();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);

                List<EstruturaEntregaAgenda> lRetorno = new List<EstruturaEntregaAgenda>();

                bd.BulkInsert(Eventos, "#tmpEventos", false, true);
                bd.Consulta("Exec ListaRetirada3 " + Eventos.Count + ",'" + dataEventoMaisProximo.ToString("yyyyMMdd000000") + "'");

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaEntregaAgenda
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        Tipo = bd.LerString("Tipo"),
                        DiasTriagem = bd.LerInt("DiasTriagem"),
                        Valor = bd.LerInt("Valor"),
                        PrioridadeProcedimento = bd.LerInt("PrioridadeProcedimento")
                    });
                }

                bd.Fechar();

                return lRetorno;
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

        public bool PossuiRetirada(List<int> listaApresentacao, List<int> Eventos)
        {
            try
            {
                string eventos = "";
                bool possui = false;
                Apresentacao oApresentacao = new Apresentacao();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);
                foreach (int eventoID in Eventos)
                {
                    if (eventos.Length > 0)
                    {
                        eventos += "," + eventoID;
                    }
                    else
                    {
                        eventos += eventoID;
                    }
                }

                string sql = @"SELECT tEntregaControle.ID
                        FROM tEventoEntregaControle
                        INNER JOIN tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEventoEntregaControle.EventoID IN (" + eventos + @") 
                        AND tEntrega.Tipo in ('R','B')AND 
                        CONVERT(VARCHAR, DATEADD(day, (PrazoEntrega + 
			                        CASE WHEN
				                        tEventoEntregaControle.DiasTriagem > 0
					                        THEN tEventoEntregaControle.DiasTriagem
				                        ELSE
					                        CASE WHEN
						                        tEntregaControle.DiasTriagem > 0
						                        THEN tEntregaControle.DiasTriagem
						                        ELSE tEntrega.DiasTriagem
					                        END
				                        END  
		                        ), CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring('" + dataEventoMaisProximo.ToString("yyyyMMdd") + @"',0,9), 112)
                        GROUP BY tEntregaControle.ID 
                        HAVING COUNT(Distinct EventoID) = " + Eventos.Count;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    possui = true;
                }

                bd.Fechar();


                return possui;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool PossuiRetirada(List<int> listaApresentacao, List<int> Eventos, string BilheteriaID, string RetiradaID)
        {
            try
            {
                string eventos = "";
                Apresentacao oApresentacao = new Apresentacao();
                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);
                string entregas = BilheteriaID + "," + RetiradaID;
                bool possui = false;
                foreach (int eventoID in Eventos)
                {
                    if (eventos.Length > 0)
                    {
                        eventos += "," + eventoID;
                    }
                    else
                    {
                        eventos += eventoID;
                    }
                }

                string sql = @"SELECT tEntregaControle.ID
                        FROM tEventoEntregaControle
                        INNER JOIN tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEventoEntregaControle.EventoID IN (" + eventos + ")  AND tEntrega.ID IN (" + entregas + @")
                        and tEntrega.Tipo in ('R','B')AND 
                        CONVERT(VARCHAR, DATEADD(day, (PrazoEntrega + 
			                        CASE WHEN
				                        tEventoEntregaControle.DiasTriagem > 0
					                        THEN tEventoEntregaControle.DiasTriagem
				                        ELSE
					                        CASE WHEN
						                        tEntregaControle.DiasTriagem > 0
						                        THEN tEntregaControle.DiasTriagem
						                        ELSE tEntrega.DiasTriagem
					                        END
				                        END  
		                        ), CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring('" + dataEventoMaisProximo.ToString("yyyyMMdd") + @"',0,9), 112)
                        GROUP BY tEntregaControle.ID 
                        HAVING COUNT(Distinct EventoID) = " + Eventos.Count;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    possui = true;
                }

                bd.Fechar();


                return possui;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int PossuiBilheteWeb(List<int> listaApresentacao, List<int> Eventos)
        {
            try
            {
                int entregacontrole = 0;

                Apresentacao oApresentacao = new Apresentacao();

                DateTime dataEventoMaisProximo = oApresentacao.ApresentacaoMaisProxima(listaApresentacao);

                string sql = @"SELECT tEntregaControle.ID
                        FROM tEventoEntregaControle
                        INNER JOIN tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        WHERE tEventoEntregaControle.EventoID IN (" + Utilitario.ArrayToString(Eventos.ToArray()) + @") 
                        AND tEntrega.Tipo in ('R') AND tEntrega.PermitirImpressaoInternet = 'T'
                        AND CONVERT(VARCHAR, DATEADD(day, (PrazoEntrega + 
			                        CASE WHEN
				                        tEventoEntregaControle.DiasTriagem > 0
					                        THEN tEventoEntregaControle.DiasTriagem
				                        ELSE
					                        CASE WHEN
						                        tEntregaControle.DiasTriagem > 0
						                        THEN tEntregaControle.DiasTriagem
						                        ELSE tEntrega.DiasTriagem
					                        END
				                        END  
		                        ), CONVERT(DATETIME, getDate(), 112)), 112) <= CONVERT(DATETIME, Substring('" + dataEventoMaisProximo.ToString("yyyyMMdd") + @"',0,9), 112)
                        GROUP BY tEntregaControle.ID 
                        HAVING COUNT(Distinct EventoID) = " + Eventos.Count;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    entregacontrole = bd.LerInt("ID");

                bd.Fechar();

                return entregacontrole;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private ArrayList canaisSelecionados;// = new int[0];

        public string ValidarSetoresAprovados()
        {
            try
            {
                StringBuilder stbRetorno = new StringBuilder();
                string sql = string.Format(@"SELECT DISTINCT tSetor.Nome FROM
                                            tEvento (NOLOCK)
                                            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
        	                                INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID
	                                        INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID
	                                        WHERE tEvento.ID = {0} AND (tSetor.AprovadoPublicacao IS NULL OR tSetor.AprovadoPublicacao = 'F')
                                            AND tSetor.LugarMarcado <> '{1}'", this.Control.ID, Setor.Pista);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                    stbRetorno.Append(string.Format("\n{0}", bd.LerString("Nome")));

                return stbRetorno.ToString();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Classe utilizada na distribuição de eventos e preços aos canais.
        /// Deverá ser utilizada em conjunto com uma collection (ArrayList).
        /// </summary>
        public class CanalDistribuicao
        {
            private int canalID;
            private int taxaConveniencia;
            private int taxaComissao;


            private bool canalIR;
            private decimal taxaMinima;
            private decimal taxaMaxima;
            private decimal comissaoMinima;
            private decimal comissaoMaxima;

            public decimal ComissaoMaxima
            {
                get { return comissaoMaxima; }
                set { comissaoMaxima = value; }
            }

            public decimal ComissaoMinima
            {
                get { return comissaoMinima; }
                set { comissaoMinima = value; }
            }
            public int TaxaComissao
            {
                get { return taxaComissao; }
                set { taxaComissao = value; }
            }
            public decimal TaxaMinima
            {
                get { return taxaMinima; }
                set { taxaMinima = value; }
            }

            public decimal TaxaMaxima
            {
                get { return taxaMaxima; }
                set { taxaMaxima = value; }
            }

            public int CanalID
            {
                get { return this.canalID; }
                set { this.canalID = value; }
            }

            public int TaxaConveniencia
            {
                get { return this.taxaConveniencia; }
                set { this.taxaConveniencia = value; }
            }

            public bool CanalIR
            {
                get { return this.canalIR; }
                set { this.canalIR = value; }
            }
        }

        public Evento() { }

        public Evento(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.UsuarioIDLogado = usuarioIDLogado;
        }

        /// <summary>
        /// Exclui Evento
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = true;

                //verificar se há ingressos nao disponiveis
                ApresentacaoLista apresentacaoLista = new ApresentacaoLista(UsuarioIDLogado);
                apresentacaoLista.FiltroSQL = "EventoID=" + this.Control.ID;
                apresentacaoLista.Carregar();

                if (apresentacaoLista.Tamanho > 0)
                {

                    ApresentacaoSetorLista apresentacaoSetorLista = new ApresentacaoSetorLista(UsuarioIDLogado);
                    apresentacaoSetorLista.FiltroSQL = "ApresentacaoID in (" + apresentacaoLista + ")";
                    apresentacaoSetorLista.Carregar();

                    if (apresentacaoSetorLista.Tamanho > 0)
                    {
                        apresentacaoSetorLista.Primeiro();
                        do
                        {
                            ok &= apresentacaoSetorLista.ApresentacaoSetor.Remover();
                        } while (apresentacaoSetorLista.Proximo());

                    } // fim if (apresentacaoSetorLista.Tamanho > 0)

                    if (ok)
                        ok = apresentacaoLista.ExcluirTudo();

                } // fim if (apresentacaoLista.Tamanho > 0)

                if (ok)
                {
                    CanalEventoLista canalEventoLista = new CanalEventoLista(UsuarioIDLogado);
                    canalEventoLista.FiltroSQL = "EventoID=" + this.Control.ID;
                    canalEventoLista.Carregar();
                    ok = canalEventoLista.ExcluirTudo();
                    if (ok)
                        ok = base.Excluir(this.Control.ID);
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CarregaTodosEventoEmTemp(string sessionID)
        {
            try
            {
                string sql = string.Empty;

                //limpar a tabela
                sql = "DELETE FROM tIRWebEventos WHERE SessionID = '" + sessionID + "'";

                bd.Executar(sql);

                sql =
                    @"INSERT INTO tIRWebEventos (ID,Nome,SessionID)
					SELECT DISTINCT EventoID AS ID,Nome,'" + sessionID +
                    @"'FROM tEvento (NOLOCK) 
					INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
					WHERE tApresentacao.DisponivelRelatorio = 'T' 
					ORDER BY Nome";

                bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CarregaTodosEventoEmTemp(string sessionID, int eventoID)
        {
            try
            {
                string sql = string.Empty;

                //limpar a tabela
                sql = "DELETE FROM tIRWebEventos WHERE SessionID = '" + sessionID + "'";

                bd.Executar(sql);

                sql =
                    @"INSERT INTO tIRWebEventos (ID,Nome,SessionID)
					SELECT DISTINCT EventoID AS ID,Nome,'" + sessionID +
                    @"'FROM tEvento (NOLOCK)
					INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
					WHERE tApresentacao.DisponivelRelatorio = 'T' AND tEvento.ID = " + eventoID +
                    " ORDER BY Nome";

                bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os eventos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome " +
                    "FROM tEvento (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os eventos com destaque na home do website
        /// </summary>
        /// <returns></returns>
        public override DataTable DestaquesHome()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PrioridadeDestaque", typeof(int));

                string sql = "SELECT DISTINCT EventoID AS ID,Evento AS Nome,PrioridadeDestaque " +
                    "FROM vwInfoVendaInternet " +
                    "WHERE Destaque='T' AND (Publicar = 'T' OR Publicar = 'S') " +
                    "ORDER BY PrioridadeDestaque";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["PrioridadeDestaque"] = bd.LerInt("PrioridadeDestaque");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os eventos
        /// </summary>
        /// <returns></returns>
        public DataTable TodosComImagem(Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT e.ID,e.Nome,e.VersaoImagemIngresso,e.VersaoImagemVale,e.VersaoImagemVale2,e.VersaoImagemVale3 " +
                    "FROM tEvento e (NOLOCK), tApresentacao a (NOLOCK)" +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os eventos
        /// </summary>
        /// <returns></returns>
        public DataTable TodosComImagem()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));

                string sql = "SELECT ID,Nome,VersaoImagemIngresso,VersaoImagemVale,VersaoImagemVale2,VersaoImagemVale3 " +
                    "FROM tEvento (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable TodosDisponivelRelatorio(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                string sql =
                    "SELECT DISTINCT tEvento.Nome, tEvento.ID " +
                    "FROM tEvento (NOLOCK) INNER JOIN " +
                    "tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID " +
                    "WHERE (tApresentacao.DisponivelRelatorio = 'T') " +
                    "ORDER BY tEvento.Nome";
                bd.Consulta(sql);
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable Todos(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome FROM tEvento (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os precos que esse evento usa em suas apresentacoes
        /// </summary>
        /// <returns></returns>
        public DataTable Precos()
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int)).DefaultValue = 0;
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int)).DefaultValue = 0;
                tabela.Columns.Add("Quantidade", typeof(int)).DefaultValue = 0;
                tabela.Columns.Add("QuantidadePorCliente", typeof(int)).DefaultValue = 0;
                tabela.Columns.Add("Impressao", typeof(string)).DefaultValue = Preco.IMPRESSAO_AMBOS;
                tabela.Columns.Add("Principal", typeof(bool)).DefaultValue = false;
                ApresentacaoLista apresentacaoLista = new ApresentacaoLista();
                apresentacaoLista.FiltroSQL = "EventoID=" + this.Control.ID;
                apresentacaoLista.Carregar();

                if (apresentacaoLista.Tamanho > 0)
                {

                    ApresentacaoSetorLista apresentacaoSetorLista = new ApresentacaoSetorLista { FiltroSQL = "ApresentacaoID in (" + apresentacaoLista + ")" };
                    apresentacaoSetorLista.Carregar();

                    if (apresentacaoSetorLista.Tamanho > 0)
                    {

                        //só podemos incluir o Nome nesse select, temos q recuperar os demais dados de outra maneira
                        string sql = "SELECT DISTINCT Nome " +
                            "FROM tPreco " +
                            "WHERE ApresentacaoSetorID in (" + apresentacaoSetorLista + ") " +
                            "ORDER BY Nome";

                        bd.Consulta(sql);
                        while (bd.Consulta().Read())
                        {
                            DataRow linha = tabela.NewRow();
                            linha["Nome"] = bd.LerString("Nome");
                            tabela.Rows.Add(linha);
                        }
                        bd.Fechar();

                        //para completar os outros campos, vamos pegar os preços das apresentacoes
                        foreach (DataRow preco in tabela.Rows)
                        {
                            string precoNome = (string)preco["Nome"];

                            if (!apresentacaoSetorLista.Primeiro())
                                continue;

                            if (precoNome.Contains("'"))
                                precoNome = precoNome.Replace("'", "''");

                            do
                            {

                                int apresentacaoSetorID = apresentacaoSetorLista.ApresentacaoSetor.Control.ID;

                                sql = "SELECT ID,CorID,Quantidade,QuantidadePorCliente " +
                                    "FROM tPreco (NOLOCK) " +
                                    "WHERE Nome='" + precoNome + "' AND ApresentacaoSetorID=" + apresentacaoSetorID;

                                bd.Consulta(sql);
                                if (bd.Consulta().Read())
                                {
                                    preco["ID"] = bd.LerInt("ID");
                                    preco["CorID"] = bd.LerInt("CorID");
                                    preco["Quantidade"] = bd.LerInt("Quantidade");
                                    preco["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                                    if (bd.LerInt("ID") == apresentacaoSetorLista.ApresentacaoSetor.PrincipalPrecoID.Valor)
                                        preco["Principal"] = true;
                                    else
                                        preco["Principal"] = false;

                                    break;
                                }
                                bd.Fechar();

                            } while (apresentacaoSetorLista.Proximo());

                        }

                    }

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Método utilizado para verificar os de acordo com o evento
        /// Utilizado no Quatro Rodas Experience para verificar os Preços disponiveis para a IR
        /// </summary>
        /// <param name="eventosID"></param>
        /// <returns></returns>
        public DataTable Precos(List<int> eventosID)
        {
            try
            {
                string sql = @"SELECT tPreco.ID AS PrecoID,tPreco.Nome AS PrecoNome,
                            substring(Horario,7,2) + '/' + substring(Horario,5,2) + '/' + substring(Horario,0,5) 
                            + ' ' + substring(Horario,9,2) + ':' + substring(Horario,11,2)
                            Horario, tEvento.Nome AS EventoNome,tSetor.Nome AS SetorNome
                            FROM tPreco (NOLOCK)
                            INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID
                            INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID
                            INNER JOIN tApresentacao (NOLOCK) ON  tApresentacao.ID =tApresentacaoSetor.ApresentacaoID
                            INNER JOIN tEvento (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                            WHERE tEvento.ID IN (" + Utilitario.ArrayToString(eventosID.ToArray()) + ")";

                DataTable retorno = new DataTable();
                retorno.Columns.Add("PrecoID", typeof(int));
                retorno.Columns.Add("PrecoNome", typeof(string));
                retorno.Columns.Add("Apresentacao", typeof(string));
                retorno.Columns.Add("EventoNome", typeof(string));
                retorno.Columns.Add("SetorNome", typeof(string));

                DataRow linha;
                bd.FecharConsulta();
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    linha = retorno.NewRow();
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["PrecoNome"] = bd.LerString("PrecoNome");
                    linha["Apresentacao"] = bd.LerString("Horario");
                    linha["EventoNome"] = bd.LerString("EventoNome");
                    linha["SetorNome"] = bd.LerString("SetorNome");
                    retorno.Rows.Add(linha);
                }
                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// SQL para Insert de novo Evento.
        /// Já retorna ID inserido.
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(CTLib.BD database)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                this.Control.Versao = 0;

                sql.Append(@"INSERT INTO tEvento(LocalID, Nome, VendaDistribuida, VersaoImagemIngresso,
                                VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, ImpressaoCodigoBarra,
                                ObrigaCadastroCliente, DesabilitaAutomatico, Resenha, Publicar, Destaque,
                                PrioridadeDestaque, ImagemInternet, Parcelas, EntregaGratuita,
                                RetiradaBilheteria, Financeiro, Atencao, Censura, EntradaAcompanhada,
                                PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes,
                                DuracaoEvento, Release, DescricaoPadraoApresentacao, PublicarSemVendaMotivo,
                                DataAberturaVenda, EventoSubTipoID, ObrigatoriedadeID,
                                EscolherLugarMarcado, MapaEsquematicoID,PalavraChave, TipoImpressao, TipoCodigoBarra, FilmeID, LimiteMaximoIngressosEvento, CodigoPos, HabilitarRetiradaTodosPDV, VendaSemAlvara ) ");
                sql.Append(@"VALUES (@001,'@003','@004',@005,@006,@007,@008,'@009','@010','@011','@012',
                                '@013','@014',@015,'@016',@017,'@018','@019','@020','@021','@022','@023',
                                '@024','@025','@026','@027','@028','@029','@030','@031', '@032', 
                                '@034', @035, @036, '@037', @038,'@039', '@040', '@041', @042,@043, @044, '@045', '@046')");

                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.VendaDistribuida.ValorBD);
                sql.Replace("@005", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@009", this.ImpressaoCodigoBarra.ValorBD);
                sql.Replace("@010", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@011", this.DesabilitaAutomatico.ValorBD);
                sql.Replace("@012", this.Resenha.ValorBD);
                sql.Replace("@013", this.Publicar.ValorBD);
                sql.Replace("@014", this.Destaque.ValorBD);
                sql.Replace("@015", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@016", this.ImagemInternet.ValorBD);
                sql.Replace("@017", this.Parcelas.ValorBD);
                sql.Replace("@018", this.EntregaGratuita.ValorBD);
                sql.Replace("@019", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@020", this.Financeiro.ValorBD);
                sql.Replace("@021", this.Atencao.ValorBD);
                sql.Replace("@022", this.Censura.ValorBD);
                sql.Replace("@023", this.EntradaAcompanhada.ValorBD);
                sql.Replace("@024", this.PDVSemConveniencia.ValorBD);
                sql.Replace("@025", this.RetiradaIngresso.ValorBD);
                sql.Replace("@026", this.MeiaEntrada.ValorBD);
                sql.Replace("@027", this.Promocoes.ValorBD);
                sql.Replace("@028", this.AberturaPortoes.ValorBD);
                sql.Replace("@029", this.DuracaoEvento.ValorBD);
                sql.Replace("@030", this.Release.ValorBD);
                sql.Replace("@031", this.DescricaoPadraoApresentacao.ValorBD);
                sql.Replace("@032", this.PublicarSemVendaMotivo.ValorBD);
                sql.Replace("@034", this.DataAberturaVenda.ValorBD);
                sql.Replace("@035", this.EventoSubTipoID.ValorBD);
                sql.Replace("@036", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@037", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@038", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@039", this.PalavraChave.ValorBD);
                sql.Replace("@040", this.TipoImpressao.ValorBD);
                sql.Replace("@041", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@042", this.FilmeID.ValorBD);
                sql.Replace("@043", this.LimiteMaximoIngressosEvento.ValorBD);
                sql.Replace("@044", this.CodigoPos.ValorBD);
                sql.Replace("@045", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@046", this.VendaSemAlvara.ValorBD);

                sql.Append(";SELECT SCOPE_IDENTITY();");

                object x = database.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                if (result)
                    InserirControle("I", database);

                return result;

            }
            catch
            {
                throw;
            }

        }

        /// <summary>		
        /// Gerar um evento completo de acordo com o dataset.
        /// </summary>
        /// <param name="info">DataSet contendo todas as informações para gerar um evento. (Evento, Preços, CanalPreços, Apresentações e ApresentacaoSetores)</param>
        public EstruturaEvento NovaEstrutura(DataSet info, int empresaID)
        {
            //Objetos instanciados fora do loop para evitar perda de performance

            CTLib.BD bdNovaEsttrutura = new BD(); // Conexão 
            EstruturaEvento estrutura = new EstruturaEvento(); //Estrutura de retorno

            Apresentacao apresentacao = new Apresentacao(UsuarioIDLogado);
            ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(UsuarioIDLogado);
            Preco preco = new Preco(UsuarioIDLogado);
            CanalPreco canalPreco = new CanalPreco(UsuarioIDLogado);
            Setor setor = new Setor(UsuarioIDLogado);
            CodigoBarra oCodigoBarra = new CodigoBarra(UsuarioIDLogado);
            EventoTipos eventoTipos = new EventoTipos(UsuarioIDLogado);

            // DataRow's utilizadas dentro do loop
            DataRow[] linhasApresentacaoSetor = null;
            DataRow[] linhasPreco = null;
            DataRow[] linhasCanalPreco = null;
            DataRow linhaApresentacao = null;

            bdNovaEsttrutura.IniciarTransacao();

            try
            {

                #region Preenche o OBJ de Obrigatoriedade para inserir caso seja "P"
                int obrigatoriedadeID = 0;
                if (Convert.ToChar(info.Tables["Evento"].Rows[0]["ObrigaCadastroCliente"]) == (char)Evento.EnumObrigaCadastroCliente.CADASTRO_FORA_PADRAO)
                {
                    Obrigatoriedade oObrigatoriedade = new Obrigatoriedade(UsuarioIDLogado);
                    oObrigatoriedade.Nome.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[0]["Adicionar"]);
                    oObrigatoriedade.RG.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[1]["Adicionar"]);
                    oObrigatoriedade.CPF.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[2]["Adicionar"]);
                    oObrigatoriedade.Telefone.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[3]["Adicionar"]);
                    oObrigatoriedade.TelefoneComercial.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[4]["Adicionar"]);
                    oObrigatoriedade.Celular.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[5]["Adicionar"]);
                    oObrigatoriedade.DataNascimento.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[6]["Adicionar"]);
                    oObrigatoriedade.Email.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[7]["Adicionar"]);
                    oObrigatoriedade.CEPCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[8]["Adicionar"]);
                    oObrigatoriedade.EnderecoCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[9]["Adicionar"]);
                    oObrigatoriedade.NumeroCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[10]["Adicionar"]);
                    oObrigatoriedade.ComplementoCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[11]["Adicionar"]);
                    oObrigatoriedade.BairroCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[12]["Adicionar"]);
                    oObrigatoriedade.CidadeCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[13]["Adicionar"]);
                    oObrigatoriedade.EstadoCliente.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[14]["Adicionar"]);
                    oObrigatoriedade.NomeEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[15]["Adicionar"]);
                    oObrigatoriedade.CPFEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[16]["Adicionar"]);
                    oObrigatoriedade.RGEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[17]["Adicionar"]);
                    oObrigatoriedade.CEPEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[18]["Adicionar"]);
                    oObrigatoriedade.EnderecoEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[19]["Adicionar"]);
                    oObrigatoriedade.NumeroEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[20]["Adicionar"]);
                    oObrigatoriedade.ComplementoEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[21]["Adicionar"]);
                    oObrigatoriedade.BairroEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[22]["Adicionar"]);
                    oObrigatoriedade.CidadeEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[23]["Adicionar"]);
                    oObrigatoriedade.EstadoEntrega.Valor = Convert.ToBoolean(info.Tables["Obrigatoriedade"].Rows[24]["Adicionar"]);
                    oObrigatoriedade.Inserir(bdNovaEsttrutura);
                    obrigatoriedadeID = oObrigatoriedade.Control.ID;
                }
                #endregion

                this.Nome.Valor = (string)info.Tables["Evento"].Rows[0]["Nome"];
                this.LocalID.Valor = (int)info.Tables["Evento"].Rows[0]["LocalID"];
                this.VendaDistribuida.Valor = (bool)info.Tables["Evento"].Rows[0]["VendaDistribuida"];
                this.VersaoImagemIngresso.Valor = (int)info.Tables["Evento"].Rows[0]["VersaoImagemIngresso"];
                this.VersaoImagemVale.Valor = (int)info.Tables["Evento"].Rows[0]["VersaoImagemVale"];
                this.ImpressaoCodigoBarra.Valor = (bool)info.Tables["Evento"].Rows[0]["ImpressaoCodigoBarra"];
                this.ObrigaCadastroCliente.Valor = (string)info.Tables["Evento"].Rows[0]["ObrigaCadastroCliente"];
                this.DesabilitaAutomatico.Valor = (bool)info.Tables["Evento"].Rows[0]["DesabilitaAutomatico"];
                this.Atencao.Valor = (string)info.Tables["Evento"].Rows[0]["Atencao"];
                this.Censura.Valor = (string)info.Tables["Evento"].Rows[0]["Censura"];
                this.PDVSemConveniencia.Valor = (string)info.Tables["Evento"].Rows[0]["PDVSemConveniencia"];
                this.RetiradaIngresso.Valor = (string)info.Tables["Evento"].Rows[0]["RetiradaIngresso"];
                this.MeiaEntrada.Valor = (string)info.Tables["Evento"].Rows[0]["MeiaEntrada"];
                this.Promocoes.Valor = (string)info.Tables["Evento"].Rows[0]["Promocoes"];
                this.AberturaPortoes.Valor = (string)info.Tables["Evento"].Rows[0]["AberturaPortoes"];
                this.DuracaoEvento.Valor = (string)info.Tables["Evento"].Rows[0]["DuracaoEvento"];
                this.Release.Valor = (string)info.Tables["Evento"].Rows[0]["Release"];
                this.DescricaoPadraoApresentacao.Valor = (string)info.Tables["Evento"].Rows[0]["DescricaoPadraoApresentacao"];
                this.EventoSubTipoID.Valor = Convert.ToInt32(info.Tables["Evento"].Rows[0]["EventoSubtipoID"]);
                this.ObrigatoriedadeID.Valor = obrigatoriedadeID;
                this.MapaEsquematicoID.Valor = Convert.ToInt32(info.Tables["Evento"].Rows[0]["MapaEsquematicoID"]);
                this.PalavraChave.Valor = Convert.ToString(info.Tables["Evento"].Rows[0]["PalavraChave"]);
                this.ExibeQuantidade.Valor = Convert.ToBoolean(info.Tables["Evento"].Rows[0]["ExibeQuantidade"]);
                this.EntregaGratuita.Valor = false;
                this.RetiradaBilheteria.Valor = true;
                this.HabilitarRetiradaTodosPDV.Valor = Convert.ToBoolean(info.Tables["Evento"].Rows[0]["HabilitarRetiradaTodosPDV"]);
                this.TipoImpressao.Valor = info.Tables["Evento"].Rows[0]["TipoImpressao"].ToString();
                this.TipoCodigoBarra.Valor = info.Tables["Evento"].Rows[0]["TipoCodigoBarra"].ToString();
                this.LimiteMaximoIngressosEvento.Valor = Convert.ToInt32(info.Tables["Evento"].Rows[0]["LimiteMaximoIngressosEvento"]);
                this.CodigoPos.Valor = this.GerarCodigoPosEvento(0, false, bdNovaEsttrutura);
                this.VendaSemAlvara.Valor = false;

                if (!this.Inserir(bdNovaEsttrutura))
                    return null;

                bool VenderPos = Convert.ToBoolean(info.Tables["Evento"].Rows[0]["VenderPos"]);

                #region EventoTipoSubtipo AKA Tipos

                foreach (DataRow linha in info.Tables["Tipos"].Rows)
                {
                    eventoTipos.EventoID.Valor = this.Control.ID;
                    eventoTipos.EventoSubtipoID.Valor = Convert.ToInt32(linha["SubtipoID"]);
                    eventoTipos.EventoTipoID.Valor = Convert.ToInt32(linha["TipoID"]);
                    if (!eventoTipos.Inserir(bdNovaEsttrutura))
                        throw new EventoTiposException("Erro ao inserir os tipos e subtipos");
                }
                #endregion

                #region EntregaPadrao

                EventoEntregaControle entrega = new EventoEntregaControle(UsuarioIDLogado);
                Entrega EntregaPadrao = new Entrega();
                List<int> listaEntregaPadrao = EntregaPadrao.GetEntregaPadrao();
                foreach (int EntregaControleID in listaEntregaPadrao)
                {
                    entrega.EventoID.Valor = this.Control.ID;
                    entrega.EntregaControleID.Valor = EntregaControleID;
                    if (!entrega.Inserir(bdNovaEsttrutura))
                        throw new TaxaEntregaEventoPadraoException("Problemas ao inserir a taxa de entrega padrão para o evento.");
                }

                this.MenorPeriodoEntrega(this.Control.ID, bdNovaEsttrutura);

                #endregion

                #region Forma de Pagamento Padrão do Evento
                FormaPagamentoEvento forma = new FormaPagamentoEvento(UsuarioIDLogado);
                FormaPagamento formaPagamentoPadrao = new FormaPagamento(UsuarioIDLogado);
                List<int> listaFormaPagamento = formaPagamentoPadrao.GetFormasPagamentoPadrao();
                foreach (int FormaPagamentoID in listaFormaPagamento)
                {
                    forma.EventoID.Valor = this.Control.ID;
                    forma.FormaPagamentoID.Valor = FormaPagamentoID;
                    if (!forma.Inserir(bdNovaEsttrutura))
                        throw new FormaPagamentoEventoException("Problemas ao inserir a forma de pagamento padrão para o evento.");
                }
                #endregion

                //2. Buscar a taxa default para os canais selecionados pelo usuário.
                string canaisEscolhidos = Utilitario.DataRowsToString(info.Tables["CanalEvento"].Rows, "CanalID");

                List<string> CodigosBarra = new List<string>();
                double porcentagem = Convert.ToDouble(ConfigurationManager.AppSettings["PorcentagemListaBraca"]);
                int minimoCodigoBarra = Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMinimaCodigoBarra"]);

                //Buscar os códigos de barra caso o evento utilize codigo com lista branca
                bool usarListaBranca = Convert.ToChar(info.Tables["Evento"].Rows[0]["TipoCodigoBarra"]) == (char)Enumerators.TipoCodigoBarra.ListaBranca;

                if (usarListaBranca || VenderPos)
                {
                    int quantidade = 0;
                    foreach (DataRow dtrAPS in info.Tables["ApresentacaoSetor"].Rows)
                        quantidade += Math.Max(Convert.ToInt32(dtrAPS["SetorQtde"]), minimoCodigoBarra);

                    quantidade += (int)Math.Round(quantidade * (porcentagem / 100));
                    CodigosBarra = oCodigoBarra.BuscarListaBranca(bdNovaEsttrutura, quantidade);
                }

                // Distribui o evento para os canais escolhidos e para a Ingresso Rápido (caso necessário).
                if (!this.DistribuirCanais(canaisEscolhidos, bdNovaEsttrutura))
                    throw new EventoException("Problemas ao distribuir os eventos!");

                //passo 2 - Inserir todas apresentações
                for (int a = 0; a < info.Tables["Apresentacao"].Rows.Count; a++)
                {
                    linhaApresentacao = info.Tables["Apresentacao"].Rows[a]; // atalho

                    apresentacao.EventoID.Valor = this.Control.ID;
                    try
                    {
                        apresentacao.Horario.Valor = Convert.ToDateTime(linhaApresentacao[Apresentacao.COL_HORARIO_STRING]);
                    }
                    catch
                    {
                        throw new Exception(linhaApresentacao[Apresentacao.COL_HORARIO_STRING].ToString());
                    }
                    apresentacao.DisponivelVenda.Valor = (bool)linhaApresentacao["DisponivelVenda"];
                    apresentacao.DisponivelAjuste.Valor = (bool)linhaApresentacao["DisponivelAjuste"];
                    apresentacao.DisponivelRelatorio.Valor = (bool)linhaApresentacao["DisponivelRelatorio"];
                    apresentacao.Impressao.Valor = (string)linhaApresentacao["Impressao"];
                    apresentacao.DescricaoPadrao.Valor = true;

                    bool apresentacaoOk = apresentacao.Inserir(bdNovaEsttrutura);

                    // Passo 3: Inserir os setores da apresentação em questão.
                    if (!apresentacaoOk)
                        continue;

                    // Busca os setores a serem inseridos para a apresentação em questão.
                    linhasApresentacaoSetor = info.Tables["ApresentacaoSetor"].Select("ApresentacaoID=" + linhaApresentacao["ID"]);

                    for (int i = 0; i < linhasApresentacaoSetor.Length; i++)
                    {
                        apresentacaoSetor.SetorID.Valor = (int)linhasApresentacaoSetor[i]["SetorID"];
                        apresentacaoSetor.ApresentacaoID.Valor = apresentacao.Control.ID;

                        bool apresentacaoSetorOk = apresentacaoSetor.Inserir(bdNovaEsttrutura); // Insert.

                        //incluir todos os preços desse setor
                        if (apresentacaoSetorOk)
                        {

                            int aS = (int)linhasApresentacaoSetor[i]["ID"];
                            linhasPreco = info.Tables["Preco"].Select("ApresentacaoSetorID=" + aS);

                            for (int p = 0; p < linhasPreco.Length; p++)
                            {
                                preco.ApresentacaoSetorID.Valor = apresentacaoSetor.Control.ID;
                                preco.Valor.Valor = (decimal)linhasPreco[p]["Valor"];
                                preco.Nome.Valor = (string)linhasPreco[p]["Nome"];
                                preco.CorID.Valor = (int)linhasPreco[p]["CorID"];
                                preco.Impressao.Valor = (string)linhasPreco[p]["Impressao"];
                                preco.Quantidade.Valor = (int)linhasPreco[p]["Quantidade"];
                                preco.QuantidadePorCliente.Valor = (int)linhasPreco[p]["QuantidadePorCliente"];
                                preco.IRVende = (bool)linhasPreco[p]["IRVende"];

                                bool precoOk = preco.Inserir(this.Control.ID, apresentacaoSetor.SetorID.Valor, apresentacao.Control.ID, !usarListaBranca, bdNovaEsttrutura);

                                if (!precoOk)
                                    continue;

                                //#MapaLugarMarcado
                                if ((bool)linhasPreco[p]["Principal"])
                                    apresentacaoSetor.AtualizarPrecoPrincipal(bdNovaEsttrutura, preco.Control.ID);

                                if (preco.IRVende)
                                    preco.DistribuirCanaisIR(this.canaisSelecionados, bdNovaEsttrutura, this.Control.ID);

                                //Adiciona o preço ao canal específico (escolhido pelo usuário)
                                linhasCanalPreco = info.Tables["CanalPreco"].Select("PrecoID=" + (p + 1));

                                for (int cp = 0; cp < linhasCanalPreco.Length; cp++)
                                {
                                    canalPreco.DataInicio.Limpar();
                                    canalPreco.DataFim.Limpar();

                                    canalPreco.CanalID.Valor = (int)linhasCanalPreco[cp]["CanalID"];
                                    canalPreco.PrecoID.Valor = preco.Control.ID;
                                    canalPreco.Quantidade.Valor = (int)linhasCanalPreco[cp]["Quantidade"];

                                    if (linhasCanalPreco[cp]["DataInicio"] != DBNull.Value)
                                        canalPreco.DataInicio.Valor = (DateTime)linhasCanalPreco[cp]["DataInicio"];

                                    if (linhasCanalPreco[cp]["DataFim"] != DBNull.Value)
                                        canalPreco.DataFim.Valor = (DateTime)linhasCanalPreco[cp]["DataFim"];

                                    canalPreco.Inserir(bdNovaEsttrutura, true);
                                }
                            }//for linhasPreco.Rows.Count
                        }// if (apresentacaoSetorOk)

                        // Adiciona o evento em questão a estrutura a ser retornada.
                        List<string> codigosApresentacaoSetor = new List<string>();

                        double quantidadeAPS = linhasApresentacaoSetor[i]["SetorQtde"] == null || linhasApresentacaoSetor[i]["SetorQtde"] == DBNull.Value ? -1 : (int)linhasApresentacaoSetor[i]["SetorQtde"];

                        if ((usarListaBranca || VenderPos) && quantidadeAPS > 0)
                        {
                            //Busca os X códigos que serão associados aos INGRESSOS! -- Minimo ou a quantidade do setor
                            int qtdCodigosAPS = Math.Max((int)(quantidadeAPS + (quantidadeAPS * (porcentagem / 100))), minimoCodigoBarra);

                            codigosApresentacaoSetor = CodigosBarra.Take(qtdCodigosAPS).ToList();
                            CodigosBarra.RemoveAll(c => codigosApresentacaoSetor.Contains(c));

                            new CodigoBarraEvento().GerarCodigos(apresentacaoSetor.Control.ID, codigosApresentacaoSetor, DateTime.Now, apresentacao.EventoID.Valor, bdNovaEsttrutura);
                        }

                        if (!usarListaBranca && VenderPos)
                            codigosApresentacaoSetor = new List<string>();

                        estrutura.Adicionar(new Dado(apresentacao.EventoID.Valor, apresentacao.Control.ID, apresentacaoSetor.SetorID.Valor, apresentacaoSetor.Control.ID, apresentacao.Horario.Valor, empresaID, this.LocalID.Valor, (int)quantidadeAPS, codigosApresentacaoSetor));
                    }//for linhasApresentacaoSetor// if (apresentacaoOk)
                }//for info.Tables["Apresentacao"].Rows                

                bdNovaEsttrutura.FinalizarTransacao();

                return estrutura;

            }
            catch (SqlException ex)
            {
                bdNovaEsttrutura.DesfazerTransacao();
                throw ex;
            }
            catch (Exception ex)
            {
                bdNovaEsttrutura.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                apresentacao = null;
                apresentacaoSetor = null;
                preco = null;
                canalPreco = null;
                setor = null;

                bdNovaEsttrutura.Fechar();
                bdNovaEsttrutura.Cnn.Dispose();
                bd.Fechar();
                bd.Cnn.Dispose();
            }

        }

        /// <summary>		
        /// Gerar um evento completo de acordo com o dataset.
        /// </summary>
        /// <param name="info">DataSet contendo todas as informações para gerar um evento. (Evento, Preços, CanalPreços, Apresentações e ApresentacaoSetores)</param>
        public override int Novo(DataSet info, int empresaID)
        {

            CTLib.BD lugares = new BD();
            //Objetos instanciados fora do loop para evitar perda de performance
            DataRow linhaApresentacao = null;
            Apresentacao apresentacao = new Apresentacao(UsuarioIDLogado);
            DataRow[] linhasApresentacaoSetor = null;
            ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(UsuarioIDLogado);
            DataRow[] linhasPreco = null;
            Preco preco = new Preco(UsuarioIDLogado);
            DataRow[] linhasCanalPreco = null;
            CanalPreco canalPreco = new CanalPreco(UsuarioIDLogado);
            Setor setor = new Setor();
            Ingresso ingresso = new Ingresso();
            CodigoBarra oCodigoBarra = new CodigoBarra(UsuarioIDLogado);

            try
            {

                bd.IniciarTransacao();

                //fila de ingressos
                //Queue fila = new Queue();

                //passo 1 - inserir evento


                this.Nome.Valor = (string)info.Tables["Evento"].Rows[0]["Nome"];
                this.LocalID.Valor = (int)info.Tables["Evento"].Rows[0]["LocalID"];
                this.VendaDistribuida.Valor = (bool)info.Tables["Evento"].Rows[0]["VendaDistribuida"];
                this.VersaoImagemIngresso.Valor = (int)info.Tables["Evento"].Rows[0]["VersaoImagemIngresso"];
                this.VersaoImagemVale.Valor = (int)info.Tables["Evento"].Rows[0]["VersaoImagemVale"];
                this.ImpressaoCodigoBarra.Valor = (bool)info.Tables["Evento"].Rows[0]["ImpressaoCodigoBarra"];
                this.ObrigaCadastroCliente.Valor = (string)info.Tables["Evento"].Rows[0]["ObrigaCadastroCliente"];
                this.DesabilitaAutomatico.Valor = (bool)info.Tables["Evento"].Rows[0]["DesabilitaAutomatico"];
                this.Resenha.Valor = (string)info.Tables["Evento"].Rows[0]["Resenha"];
                this.EntregaGratuita.Valor = (bool)info.Tables["Evento"].Rows[0]["EntregaGratuita"];
                this.RetiradaBilheteria.Valor = (bool)info.Tables["Evento"].Rows[0]["RetiradaBilheteria"];
                this.Atencao.Valor = (string)info.Tables["Evento"].Rows[0]["Atencao"];
                this.Censura.Valor = (string)info.Tables["Evento"].Rows[0]["Censura"];
                this.EntradaAcompanhada.Valor = (bool)info.Tables["Evento"].Rows[0]["EntradaAcompanhada"];
                this.PDVSemConveniencia.Valor = (string)info.Tables["Evento"].Rows[0]["PDVSemConveniencia"];
                this.RetiradaIngresso.Valor = (string)info.Tables["Evento"].Rows[0]["RetiradaIngresso"];
                this.MeiaEntrada.Valor = (string)info.Tables["Evento"].Rows[0]["MeiaEntrada"];
                this.Promocoes.Valor = (string)info.Tables["Evento"].Rows[0]["Promocoes"];
                this.AberturaPortoes.Valor = (string)info.Tables["Evento"].Rows[0]["AberturaPortoes"];
                this.DuracaoEvento.Valor = (string)info.Tables["Evento"].Rows[0]["DuracaoEvento"];
                this.Release.Valor = (string)info.Tables["Evento"].Rows[0]["Release"];
                this.DescricaoPadraoApresentacao.Valor = (string)info.Tables["Evento"].Rows[0]["DescricaoPadraoApresentacao"];
                this.EventoSubTipoID.Valor = Convert.ToInt32(info.Tables["Evento"].Rows[0]["EventoSubtipoID"]);
                this.MapaEsquematicoID.Valor = Convert.ToInt32(info.Tables["Evento"].Rows[0]["MapaEsquematicoID"]);
                this.PalavraChave.Valor = (string)info.Tables["Evento"].Rows[0]["PalavraChave"];

                if (!this.Inserir(bd))
                    return 0;



                //2. Buscar a taxa default para os canais selecionados pelo usuário.
                string canaisEscolhidos = Utilitario.DataRowsToString(info.Tables["CanalEvento"].Rows, "CanalID");

                if (!this.DistribuirCanais(canaisEscolhidos, bd))
                    throw new EventoException("Problemas ao distribuir os eventos!");



                //passo 2 - inserir apresentacoes, alterando o eventoID



                for (int a = 0; a < info.Tables["Apresentacao"].Rows.Count; a++)
                {

                    linhaApresentacao = info.Tables["Apresentacao"].Rows[a];

                    apresentacao.EventoID.Valor = this.Control.ID;
                    try
                    {
                        apresentacao.Horario.Valor = Convert.ToDateTime(linhaApresentacao[Apresentacao.COL_HORARIO_STRING]);
                    }
                    catch
                    {
                        throw new Exception(linhaApresentacao[Apresentacao.COL_HORARIO_STRING].ToString());
                    }
                    apresentacao.DisponivelVenda.Valor = (bool)linhaApresentacao["DisponivelVenda"];
                    apresentacao.DisponivelAjuste.Valor = (bool)linhaApresentacao["DisponivelAjuste"];
                    apresentacao.DisponivelRelatorio.Valor = (bool)linhaApresentacao["DisponivelRelatorio"];
                    apresentacao.Impressao.Valor = (string)linhaApresentacao["Impressao"];
                    //apresentacao.LocalImagemMapaID.Valor = (int)linhaApresentacao["LocalImagemMapaID"];

                    bool apresentacaoOk = apresentacao.Inserir(bd);
                    //Processando();

                    //incluir todos os setores dessa apresentacao
                    if (!apresentacaoOk)
                        continue;

                    linhasApresentacaoSetor = info.Tables["ApresentacaoSetor"].Select("ApresentacaoID=" + (a + 1));

                    for (int i = 0; i < linhasApresentacaoSetor.Length; i++)
                    {
                        apresentacaoSetor.SetorID.Valor = (int)linhasApresentacaoSetor[i]["SetorID"];
                        apresentacaoSetor.ApresentacaoID.Valor = apresentacao.Control.ID;

                        bool apresentacaoSetorOk = apresentacaoSetor.Inserir(bd);
                        //Processando();

                        //incluir todos os preços desse setor
                        if (apresentacaoSetorOk)
                        {

                            int aS = (int)linhasApresentacaoSetor[i]["ID"];
                            linhasPreco = info.Tables["Preco"].Select("ApresentacaoSetorID=" + aS);

                            for (int p = 0; p < linhasPreco.Length; p++)
                            {

                                preco.ApresentacaoSetorID.Valor = apresentacaoSetor.Control.ID;
                                preco.Valor.Valor = (decimal)linhasPreco[p]["Valor"];
                                preco.Nome.Valor = (string)linhasPreco[p]["Nome"];
                                preco.CorID.Valor = (int)linhasPreco[p]["CorID"];
                                preco.Impressao.Valor = (string)linhasPreco[p]["Impressao"];
                                preco.Quantidade.Valor = (int)linhasPreco[p]["Quantidade"];
                                preco.QuantidadePorCliente.Valor = (int)linhasPreco[p]["QuantidadePorCliente"];
                                bool precoOk =
                                    preco.Inserir(this.Control.ID, apresentacaoSetor.SetorID.Valor, apresentacao.Control.ID,
                                    true, bd);

                                if (!precoOk)
                                    continue;

                                //add Canal X Preço ********************************************************

                                linhasCanalPreco = info.Tables["CanalPreco"].Select("PrecoID=" + (p + 1));

                                for (int cp = 0; cp < linhasCanalPreco.Length; cp++)
                                {
                                    canalPreco.DataInicio.Limpar();
                                    canalPreco.DataFim.Limpar();

                                    canalPreco.CanalID.Valor = (int)linhasCanalPreco[cp]["CanalID"];
                                    canalPreco.PrecoID.Valor = preco.Control.ID;
                                    if (linhasCanalPreco[cp]["DataInicio"] != DBNull.Value)
                                        canalPreco.DataInicio.Valor = (DateTime)linhasCanalPreco[cp]["DataInicio"];
                                    if (linhasCanalPreco[cp]["DataFim"] != DBNull.Value)
                                        canalPreco.DataFim.Valor = (DateTime)linhasCanalPreco[cp]["DataFim"];
                                    canalPreco.Quantidade.Valor = (int)linhasCanalPreco[cp]["Quantidade"];

                                    canalPreco.Inserir(bd, true);

                                }//for linhasCanalPreco// if (precoOk)

                            }//for linhasPreco.Rows.Count

                        }// if (apresentacaoSetorOk)

                        //GERAR INGRESSOS ********************************************************************


                        lugares = setor.Lugares(apresentacaoSetor.SetorID.Valor);
                        int sequencia = ingresso.UltimoCodigoSequencial(apresentacao.Control.ID) + 1;
                        while (lugares.Consulta().Read())
                        {
                            // Verifica se é lugar marcado.
                            if (lugares.LerString("LugarMarcado") != Setor.Pista)
                            {
                                int qtde = lugares.LerInt("Quantidade");

                                ingresso.NovoMarcado(apresentacaoSetor.Control.ID, apresentacao.EventoID.Valor, apresentacao.Control.ID, apresentacaoSetor.SetorID.Valor, empresaID, this.LocalID.Valor, lugares.LerInt("BloqueioID"), lugares.LerInt("ID"), qtde, lugares.LerInt("QuantidadeBloqueada"), lugares.LerString("CodigoPos"), lugares.LerInt("Grupo"), lugares.LerInt("Classificacao"), lugares.LerString("LugarMarcado"), bd, sequencia, null);

                                sequencia += qtde;
                            }
                            else
                            {
                                ingresso.Acrescentar(apresentacaoSetor.Control.ID, apresentacao.EventoID.Valor, apresentacao.Control.ID, apresentacaoSetor.SetorID.Valor, empresaID, this.LocalID.Valor, 0, (int)linhasApresentacaoSetor[i]["SetorQtde"], 1, bd, true, new List<string>()); // Inicia com código 1
                            }
                        }
                        lugares.Consulta().Close(); // Fecha a consulta.

                        //FIM DE GERAR INGRESSOS *************************************************************

                    }//for linhasApresentacaoSetor// if (apresentacaoOk)

                }//for info.Tables["Apresentacao"].Rows


                bd.FinalizarTransacao();

                return this.Control.ID;


            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                apresentacao = null;
                apresentacaoSetor = null;
                preco = null;
                canalPreco = null;
                setor = null;
                ingresso = null;

                lugares.Fechar();
                lugares.Cnn.Dispose();
                bd.Fechar();
                bd.Cnn.Dispose();
            }

        }

        /// <summary>
        /// Método responsável por buscar os canais que serão utilizados na distribuição dos eventos e/ou preços para o canal.
        /// Já implementa VendaDistribuida
        /// </summary>
        /// <param name="canais">Canais selecionados pelo usuário. Separados por vírgula</param>
        /// <returns> Array List object</returns>
        private ArrayList CanaisDistribuicao(string canais)
        {
            try
            {
                this.canaisSelecionados = new ArrayList();

                //1. Verifica se os canais já foram carregados.
                if (this.canaisSelecionados.Count == 0)
                {
                    string sql = string.Empty;

                    //2. Verifica se a venda é distribuída.
                    if (this.VendaDistribuida.Valor)
                    {
                        sql = "SELECT tCanal.ID as CanalID, TaxaConveniencia, EmpresaPromove, EmpresaVende,TaxaMinima, tCanal.TaxaMaxima, TaxaComissao,ComissaoMinima,ComissaoMaxima FROM tEmpresa (NOLOCK), tCanal (NOLOCK) WHERE tCanal.EmpresaID = tEmpresa.ID AND ((EmpresaVende='T' AND EmpresaPromove='F' ) ";
                        //1.1 Venda distribuída e canais próprios?
                        if (canais.Length > 0)
                            sql += " OR tCanal.ID IN (" + canais + ")) ORDER BY tCanal.ID";
                        else
                            sql += ") ORDER BY tCanal.ID";
                    }
                    else
                    {
                        // Apenas canais próprios
                        if (canais.Length == 0)
                            return this.canaisSelecionados;
                        else
                            sql = "SELECT ID AS CanalID, TaxaConveniencia, 'T' AS EmpresaPromove, 'F' AS EmpresaVende,TaxaMinima, tCanal.TaxaMaxima, TaxaComissao,ComissaoMinima,ComissaoMaxima FROM tCanal WHERE ID IN (" + canais + ") ORDER BY tCanal.ID";
                    }


                    //3. Busca todos os canais de acordo com a verificação acima.
                    bd.Consulta(sql);
                    ///--	

                    CanalDistribuicao oCanaisDistribuicao;

                    // 4. Armazena os canais e taxas no arraylist.
                    while (bd.Consulta().Read())
                    {
                        oCanaisDistribuicao = new CanalDistribuicao
                        {
                            CanalID = bd.LerInt("CanalID"),
                            TaxaConveniencia = bd.LerInt("TaxaConveniencia"),
                            CanalIR =
                                !bd.LerBoolean("EmpresaPromove") &&
                                bd.LerBoolean("EmpresaVende"),
                            TaxaMinima = bd.LerDecimal("TaxaMinima"),
                            TaxaMaxima = bd.LerDecimal("TaxaMaxima"),
                            TaxaComissao = bd.LerInt("TaxaComissao"),
                            ComissaoMinima = bd.LerDecimal("ComissaoMinima"),
                            ComissaoMaxima = bd.LerDecimal("ComissaoMaxima")
                        };
                        this.canaisSelecionados.Add(oCanaisDistribuicao);
                    }
                }
                bd.Consulta().Close();

                return this.canaisSelecionados;
            }
            catch
            {
                if (!bd.Consulta().IsClosed)
                    bd.Consulta().Close();
                throw;
            }
        }

        /// <summary>
        /// Distribui esse evento nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        public bool DistribuirCanais(string canais, BD bd)
        {
            return DistribuirCanais(bd, canais, false);
        }

        public bool DistribuirCanais(BD bd, string canais, bool filme)
        {
            try
            {
                string[] CanaisSelecionados = canais.Split(',');

                //1. Busca os canais para distribuição
                ArrayList canaisDistribuicao = this.CanaisDistribuicao(canais);

                //2. Disponibiliza o evento para os canais encontrados.
                CanalEvento canalEvento = new CanalEvento(UsuarioIDLogado);

                foreach (CanalDistribuicao canalInfo in canaisDistribuicao)
                {
                    canalEvento.CanalID.Valor = canalInfo.CanalID;
                    canalEvento.EventoID.Valor = this.Control.ID;
                    canalEvento.TaxaConveniencia.Valor = canalInfo.TaxaConveniencia;
                    canalEvento.TaxaMinima.Valor = canalInfo.TaxaMinima;
                    canalEvento.TaxaMaxima.Valor = canalInfo.TaxaMaxima;
                    canalEvento.TaxaComissao.Valor = canalInfo.TaxaComissao;
                    canalEvento.ComissaoMinima.Valor = canalInfo.ComissaoMinima;
                    canalEvento.ComissaoMaxima.Valor = canalInfo.ComissaoMaxima;
                    canalEvento.Filme = filme;

                    bool inserir = CanaisSelecionados.Contains(canalInfo.CanalID.ToString());

                    if (!canalEvento.Inserir(bd, inserir))
                        throw new CanalEventoException("Problemas ao distribuir o Evento para os Canais");
                }

                return true;
            }
            catch
            {
                if (!bd.Consulta().IsClosed)
                    bd.Consulta().Close();

                return false;
            }
        }

        public void DistribuirCanais(BD bd, string canais, int pEventoID, bool isFilme)
        {
            try
            {
                string[] CanaisSelecionados = canais.Split(',');

                //1. Busca os canais para distribuição
                ArrayList canaisDistribuicao = this.CanaisDistribuicao(canais);

                //2. Disponibiliza o evento para os canais encontrados.
                CanalEvento canalEvento = new CanalEvento(UsuarioIDLogado);

                foreach (CanalDistribuicao canalInfo in canaisDistribuicao)
                {
                    canalEvento.CanalID.Valor = canalInfo.CanalID;
                    canalEvento.EventoID.Valor = pEventoID;
                    canalEvento.TaxaConveniencia.Valor = canalInfo.TaxaConveniencia;
                    canalEvento.TaxaMinima.Valor = canalInfo.TaxaMinima;
                    canalEvento.TaxaMaxima.Valor = canalInfo.TaxaMaxima;
                    canalEvento.TaxaComissao.Valor = canalInfo.TaxaComissao;
                    canalEvento.ComissaoMinima.Valor = canalInfo.ComissaoMinima;
                    canalEvento.ComissaoMaxima.Valor = canalInfo.ComissaoMaxima;
                    canalEvento.Filme = isFilme;

                    bool inserir = CanaisSelecionados.Contains(canalInfo.CanalID.ToString());

                    if (!canalEvento.Inserir(bd, inserir))
                        throw new CanalEventoException("Problemas ao distribuir o Evento para os Canais");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Distribui esse evento nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        public bool DistribuirCanaisIR()
        {

            bool ok = true;

            try
            {
                EmpresaLista empresaLista = new EmpresaLista();
                empresaLista.FiltroSQL = "EmpresaVende='T'";
                empresaLista.FiltroSQL = "EmpresaPromove='F'";
                empresaLista.Carregar();

                // Objetos utilizados dentro do Loop. Instanciados fora para evitar abertura de várias conexões e melhorar performance.
                Canal c = new Canal();
                CanalEventoLista canalEventoLista = new CanalEventoLista();
                CanalEvento canalEvento = new CanalEvento(this.Control.UsuarioID);
                DataTable canais = null;

                if (empresaLista.Primeiro())
                {
                    do
                    {
                        canais = empresaLista.Empresa.Canais(); // TODO: Abertura de conexão dentro de Loop
                        foreach (DataRow canal in canais.Rows)
                        {

                            int canalID = (int)canal["ID"];
                            c.Ler(canalID);

                            canalEventoLista.FiltroSQL = null; // Zera o filtro para não evitar acumulo de instruções.
                            canalEventoLista.FiltroSQL = "CanalID=" + canalID;
                            canalEventoLista.FiltroSQL = "EventoID=" + this.Control.ID;
                            canalEventoLista.Carregar();

                            if (canalEventoLista.Tamanho == 0)
                            {

                                canalEvento.CanalID.Valor = canalID;
                                canalEvento.EventoID.Valor = this.Control.ID;
                                canalEvento.TaxaConveniencia.Valor = c.TaxaConveniencia.Valor;

                                ok &= canalEvento.Inserir(false);
                            }

                        }
                    } while (empresaLista.Proximo());
                }

            }
            catch
            {
                ok = false;
            }

            return ok;


        }

        /// <summary>		
        /// Obter setores de uma ApresentacaoSetor especifica de um evento
        /// </summary>
        /// <returns></returns>
        public DataTable ApresentacoesSetores()
        {

            try
            {

                DataTable tabela = new DataTable("ApresentacaoSetor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));

                string sql = "SELECT tas.* " +
                    "FROM tApresentacaoSetor AS tas, tEvento AS e, tApresentacao AS a " +
                    "WHERE a.ID=tas.ApresentacaoID AND a.EventoID=e.ID AND e.ID=" + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento
        /// </summary>
        /// <returns></returns>
        public override DataTable Apresentacoes()
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esses campos sao importantes para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT DISTINCT Horario, ID " +
                    "FROM tApresentacao " +
                    "WHERE EventoID=" + this.Control.ID + " " +
                    "ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento
        /// </summary>
        /// <returns></returns>
        public DataTable Apresentacoes(int eventoID, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T' " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T' " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T' " : "";


                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esses campos sao importantes para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = eventoID;

                string sql = "SELECT DISTINCT Horario, ID " +
                    "FROM tApresentacao a " +
                    "WHERE EventoID=" + eventoID + " " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public DataTable ApresentacoesDataHora(Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esse campo eh importante para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                string sql = "SELECT Horario, ID " +
                    "FROM tApresentacao WHERE EventoID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    //linha["Horario"]= bd.LerStringFormatoDataHora("Horario");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public override DataTable Apresentacoes(Apresentacao.Disponibilidade disponibilidade)
        {

            DataTable tabela = new DataTable("Apresentacao");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Horario", typeof(string));
            //esse campo eh importante para varias telas
            tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;

            try
            {

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   Horario, " +
                    "   ID " +
                    "FROM tApresentacao (NOLOCK) " +
                    "WHERE EventoID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY Horario"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
                        //linha["Horario"]= bd.LerStringFormatoDataHora("Horario");
                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public DataTable Apresentacoes(Apresentacao.Disponibilidade disponibilidade, int canalid)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esse campo eh importante para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                string sql = "SELECT DISTINCT a.Horario, a.ID " +
                    "FROM tApresentacao AS a, tApresentacaoSetor, tPreco AS p, tSetor AS s, tCanalPreco AS cp " +
                    "WHERE a.EventoID=" + this.Control.ID + " AND tApresentacaoSetor.ApresentacaoID=a.ID AND " +
                    "(cp.DataFim >= '" + hoje + "' or cp.DataFim = '') AND tApresentacaoSetor.SetorID=s.ID AND p.ApresentacaoSetorID=tApresentacaoSetor.ID AND " +
                    "cp.PrecoID=p.ID AND cp.CanalID=" + canalid +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY a.Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
                    //linha["Horario"]= bd.LerStringFormatoDataHora("Horario");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public DataTable ApresentacoesComImagem()
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esse campo eh importante para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));


                string sql = "SELECT Horario, ID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3 " +
                    "FROM tApresentacao WHERE EventoID=" + this.Control.ID +
                    " ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public DataTable ApresentacoesComImagem(Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                //esse campo eh importante para varias telas
                tabela.Columns.Add("EventoID", typeof(int)).DefaultValue = this.Control.ID;
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                string sql = "SELECT Horario, ID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3 " +
                    "FROM tApresentacao WHERE EventoID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public ArrayList Apresentacoes(Apresentacao.Disponibilidade disponibilidade, string dataInicial, string dataFinal)
        {

            try
            {

                // Estrutura de dado de retorno
                ArrayList apresentacoesID = new ArrayList();
                // Verificando a disponibilidade

                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";
                // Consultando no banco

                string sql = "SELECT ID, EventoID, Horario, DisponivelRelatorio " +
                    "FROM tApresentacao " +
                    "WHERE (EventoID = " + this.Control.ID + ") AND (Horario >= '" + dataInicial + "') AND (Horario < '" + dataFinal + "') " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    apresentacoesID.Add(bd.LerInt("ID"));
                }

                bd.Fechar();
                return apresentacoesID;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter o Nome da empresa deste evento
        /// </summary>
        /// <returns></returns>
        public string EmpresaNome()
        {
            try
            {
                string empresaNome = "";
                string sql =
                    "SELECT     tEmpresa.ID, tEmpresa.Nome " +
                    "FROM       tEmpresa INNER JOIN " +
                    "tLocal ON tEmpresa.ID = tLocal.EmpresaID INNER JOIN " +
                    "tEvento ON tLocal.ID = tEvento.LocalID " +
                    "WHERE      (tEvento.ID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    empresaNome = bd.LerString("Nome");
                }
                bd.Fechar();
                return empresaNome;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// Obter o ID da empresa deste evento
        /// </summary>
        /// <returns></returns>
        public int EmpresaID()
        {
            try
            {
                int empresaID = 0;
                string sql =
                    "SELECT     tEmpresa.ID, tEmpresa.Nome " +
                    "FROM       tEmpresa INNER JOIN " +
                    "tLocal ON tEmpresa.ID = tLocal.EmpresaID INNER JOIN " +
                    "tEvento ON tLocal.ID = tEvento.LocalID " +
                    "WHERE      (tEvento.ID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    empresaID = bd.LerInt("ID");
                }
                bd.Fechar();
                return empresaID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter todos os setores desse evento
        /// </summary>
        /// <returns></returns>
        public override DataTable Setores()
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT DISTINCT tSetor.ID, tSetor.Nome " +
                    "FROM tApresentacaoSetor (NOLOCK),tSetor (NOLOCK),tApresentacao (NOLOCK)" +
                    "WHERE tApresentacaoSetor.SetorID=tSetor.ID AND tApresentacao.ID=tApresentacaoSetor.ApresentacaoID AND tApresentacao.EventoID=" + this.Control.ID + " " +
                    "ORDER BY tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os setores desse evento
        /// </summary>
        /// <returns></returns>
        public DataTable Setores(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Setor");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                ApresentacaoLista apresentacoes = new ApresentacaoLista();
                apresentacoes.FiltroSQL = "EventoID=" + this.Control.ID;
                apresentacoes.Carregar();

                string sql = "SELECT DISTINCT s.ID, s.Nome FROM tApresentacaoSetor as tas,tSetor as s " +
                    "WHERE tas.SetorID=s.ID AND tas.ApresentacaoID in (" + apresentacoes + ") " +
                    "ORDER BY s.Nome";

                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter todos os setores desse evento
        /// Em função da apresentacao
        /// </summary>
        /// <returns></returns>
        public DataTable Setores(string registroZero, string apresentacoes, TipoPaganteOuCortesia paganteCondicao)
        {
            try
            {
                // Criando tabela com um registro ou não
                DataTable tabela = new DataTable("Setor");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                // Tratando Condição
                string cortesiaCondicao = "";
                switch (paganteCondicao)
                {
                    case TipoPaganteOuCortesia.Pagante:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0) ";
                        break;
                    case TipoPaganteOuCortesia.Cortesia:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0) ";
                        break;
                    case TipoPaganteOuCortesia.Ambos:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0) ";
                        break;
                }
                #region Obter SetorID
                BD bdID = new BD();
                string sqlID =
                    "SELECT DISTINCT tIngresso.SetorID " +
                    "FROM            tIngressoLog INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID " +
                    "WHERE        (tIngresso.ApresentacaoID IN (" + apresentacoes + ")) " + cortesiaCondicao;
                bdID.Consulta(sqlID);
                bool primeiraVez = true;
                string IDs = "";
                while (bdID.Consulta().Read())
                {
                    if (primeiraVez)
                    {
                        // 1a vez
                        IDs = bdID.LerInt("SetorID").ToString();
                        primeiraVez = false;
                    }
                    else
                        IDs = IDs + "," + bdID.LerInt("SetorID").ToString();
                }
                bdID.Fechar();
                #endregion
                #region Obter Nome de setores e ordenar
                if (IDs != "")
                {
                    BD bdNome = new BD();
                    string sqlNome =
                        "SELECT Nome, ID " +
                        "FROM tSetor " +
                        "WHERE (ID IN (" + IDs + ")) " +
                        "ORDER BY Nome";
                    bdNome.Consulta(sqlNome);
                    while (bdNome.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bdNome.LerInt("ID");
                        linha["Nome"] = bdNome.LerString("Nome");
                        tabela.Rows.Add(linha);
                    }
                    bdNome.Fechar();
                }
                #endregion
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter apresentacoes desse evento
        /// </summary>
        /// <param name="registroZero">Um string que vai conter a descricao do registro zero</param>
        /// <returns></returns>
        public DataTable Apresentacoes(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacoes");

                // Criar DataTable com as colunas
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT Horario, ID FROM tApresentacao " +
                    "WHERE EventoID=" + this.Control.ID + " " +
                    "ORDER BY Horario";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception erro)
            {
                throw erro;
            }

        }

        /// <summary>		
        /// Obter apresentacoes desse evento de acordo com os parametros
        /// </summary>
        /// <returns></returns>
        public DataTable Apresentacoes(string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            //addDisponibilidade(Apresentacao.Disponibilidade d){
            //		disponibilidade |= d;
            //
            //delDisponibilidade(Apresentacao.Disponibilidade d){
            //		disponibilidade &= ~d;

            try
            {
                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                string sql = "SELECT Horario, ID FROM tApresentacao (NOLOCK) " +
                    "WHERE EventoID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter canais desse evento
        /// </summary>
        /// <returns></returns>
        public override DataTable Canais()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ca.ID,ca.Nome FROM tEvento AS e,tCanalEvento AS ce,tCanal AS ca " +
                    "WHERE ca.ID=ce.CanalID AND ce.EventoID=e.ID AND " +
                    "ce.EventoID=" + this.Control.ID + " ORDER BY ca.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter canais desse evento de uma empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(int empresaid)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ca.ID,ca.Nome FROM tEvento AS e,tCanalEvento AS ce,tCanal AS ca " +
                    "WHERE ca.ID=ce.CanalID AND ce.EventoID=e.ID AND ca.EmpresaID=" + empresaid + " AND " +
                    "ce.EventoID=" + this.Control.ID + " ORDER BY ca.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter canais desse evento
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ca.ID,ca.Nome FROM tEvento as e,tCanalEvento as ce,tCanal as ca " +
                    "WHERE ca.ID=ce.CanalID AND ce.EventoID=e.ID AND " +
                    "ce.EventoID=" + this.Control.ID + " ORDER BY ca.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui as Apresentacoes, CanalEvento deste Evento e este Evento
        /// </summary>
        /// <returns>Informa se a exclusão ocorreu com sucesso</returns>
        public override bool ExcluirCascata()
        {
            bool excluiuTodasApresentacoes = true;
            bool excluiuTodosCanalEvento = true;
            bool excluiuTudo = false;
            try
            {
                // Excluir todas Apresentacoes deste Evento
                ApresentacaoLista apresentacaoLista = new ApresentacaoLista();
                apresentacaoLista.FiltroSQL = "EventoID = " + this.Control.ID;
                apresentacaoLista.Carregar();
                apresentacaoLista.Primeiro();
                int contador = 0;
                if (apresentacaoLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodasApresentacoes = excluiuTodasApresentacoes && apresentacaoLista.Apresentacao.ExcluirCascata();
                        contador++;
                        apresentacaoLista.Proximo();
                    } while (excluiuTodasApresentacoes && contador < apresentacaoLista.Tamanho);
                }
                // Excluir todas Apresentacoes deste Evento
                CanalEventoLista canalEventoLista = new CanalEventoLista();
                canalEventoLista.FiltroSQL = "EventoID = " + this.Control.ID;
                canalEventoLista.Carregar();
                canalEventoLista.Primeiro();
                contador = 0;
                if (canalEventoLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodosCanalEvento = excluiuTodosCanalEvento && canalEventoLista.CanalEvento.Excluir();
                        contador++;
                        canalEventoLista.Proximo();
                    } while (excluiuTodosCanalEvento && contador < canalEventoLista.Tamanho);
                }
                // Excluir este evento
                if (excluiuTodasApresentacoes && excluiuTodosCanalEvento)
                    excluiuTudo = this.Excluir();
                //					excluiuTudo= true;
                // Retorna sucesso se as duas operações forem 
                return excluiuTudo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        private string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }

        private string MontaSQLVendasPorEventoSetor(int perfilTipoID, bool comCortesia, int localID, int canalID, int empresaID, string dataInicial, string dataFinal, int lojaID, int usuarioID)
        {
            //int perfilTipoID = 0;

            string sql = "";
            string filtro = "";

            if (empresaID > 0)
            {

                switch (perfilTipoID)
                {
                    case 1: // Empresa (EmpresaID da tIngresso
                    case 5:
                        filtro = " AND (tIngressoLog.EmpresaID = " + empresaID + " OR tIngresso.EmpresaID = " + empresaID + ")";
                        break;
                    case 2: // Local (EmpresaID da tIngresso)
                        filtro = " AND tIngresso.LocalID = " + localID;
                        break;
                    case 3: // Canal (Não precisa filtrar EmpresaID)
                        //filtro = " AND tIngressoLog.CanalID = " + canalID;
                        break;
                    case 4: // Evento (não possui Vendas por canal);
                        break;
                }


            }


            if (canalID > 0)
                filtro += " AND tIngressoLog.CanalID = " + canalID;

            if (usuarioID > 0)
                filtro += " AND tIngressoLog.UsuarioID = " + usuarioID;

            if (lojaID > 0)
                filtro += " AND tIngressoLog.LojaID = " + lojaID;

            if (!comCortesia)
                filtro += " AND tIngressoLog.CortesiaID = 0 ";




            sql = "SELECT  " +
                " tEvento.Nome as EventoNome,tSetor.Nome as SetorNome, " +
                "		CASE tIngressoLog.Acao " +
                "			WHEN 'V' THEN " +
                "				COUNT(tIngressoLog.ID) " +
                "		ELSE " +
                "				COUNT(tIngressoLog.ID) * -1 " +
                "		END as Quantidade, " +
                "		CASE tIngressoLog.Acao " +
                "			WHEN 'V' THEN " +
                "				COUNT(tIngressoLog.ID) * tPreco.Valor " +
                "			ELSE " +
                "				COUNT(tIngressoLog.ID) * tPreco.Valor * -1 " +
                "		END as Valor " +
                "FROM tIngressoLog (NOLOCK) " +
                "INNER JOIN tIngresso (NOLOCK) ON tIngressoLog.IngressoID = tIngresso.ID " +
                "INNER JOIN tPreco (NOLOCK) ON tIngressoLog.PrecoID = tPreco.ID " +
                "INNER JOIN tApresentacao (NOLOCK) ON tIngresso.ApresentacaoID = tApresentacao.ID " +
                "INNER JOIN tSetor (NOLOCK) on tIngresso.SetorID = tSetor.ID " +
                "INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tIngressoLog.CaixaID " +
                "INNER JOIN tEvento (NOLOCK) ON tIngresso.EventoID = tEvento.ID  " +
                "WHERE  " +
                " Acao IN ('C','V') ";




            sql += filtro;

            sql += " AND tApresentacao.DisponivelRelatorio = 'T' " +
                "AND DataAbertura >= '" + dataInicial + "' AND DataAbertura < '" + dataFinal +
                "' Group By tEvento.Nome,tSetor.Nome,tIngressoLog.Acao, tPreco.Valor " +
                " Order By tEvento.Nome,tSetor.Nome";

            return sql;

        }

        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia,
            int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                {
                    // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os Caixas nos intervalos especificados
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia, apresentacaoID, eventoID, localID, empresaID, usuarioID, lojaID, canalID, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // evento zero pega todos
                int totaisVendidos = QuantidadeIngressosPorEvento(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorEvento(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorEvento(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorEvento(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                # endregion
                // Para cada evento no período especificado, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorEvento(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorEvento(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do CANCELADO
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorEvento(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorEvento(ingressoLogIDsCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                //
                tabela.Columns["VariacaoLinha"].ColumnName = "Evento";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tEvento.ID AS EventoID, tEmpresa.Nome + '-' + tEvento.Nome AS Evento " +
                        "FROM tApresentacao INNER JOIN " +
                        "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                        "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                        "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                        "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                        "tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("EventoID");
                        linha["VariacaoLinha"] = obterDados.LerString("Evento");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public override int QuantidadeIngressosPorEvento(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoEvento = "";
                    if (this.Control.ID > 0)
                        condicaoEvento = "AND (tApresentacao.EventoID = " + this.Control.ID + ") ";
                    else
                        condicaoEvento = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tApresentacao.EventoID) AS QuantidadeIngressos " +
                        "FROM     tApresentacao INNER JOIN " +
                        "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                        "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                        "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                        "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                        "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoEvento;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de evento
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos eventos
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorEvento

        public int QuantidadeCortesiasPorCaixa(string dataInicial, string dataFinal)
        {
            try
            {
                int quantidade = 0;
                int quantidadeVendidos = 0;
                int quantidadeCancelados = 0;
                if (dataInicial == "" || dataFinal == "")
                    return 0;
                #region Obtendo quantidade de cortesias vendidos
                BD bdVendidos = new BD();
                string sqlVendidos =
                    "SELECT  COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.Acao, tEvento.ID " +
                    "FROM    tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                    "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID " +
                    "WHERE   (tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') " +
                    "GROUP BY tIngressoLog.Acao, tEvento.ID " +
                    "HAVING  (tIngressoLog.Acao = N'" + IngressoLog.VENDER + "') AND (tEvento.ID = " + this.Control.ID + ") ";
                bdVendidos.Consulta(sqlVendidos);
                if (bdVendidos.Consulta().Read())
                {
                    quantidadeVendidos = bdVendidos.LerInt("Quantidade");
                }
                bdVendidos.Fechar();
                #endregion
                #region Obtendo quantidade de cortesias cancelados
                BD bdCancelados = new BD();
                string sqlCancelados =
                    "SELECT  COUNT(tIngressoLog.ID) AS Quantidade, tIngressoLog.Acao, tEvento.ID " +
                    "FROM    tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                    "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID " +
                    "WHERE   (tCaixa.DataAbertura < '" + dataFinal + "') AND (tCaixa.DataAbertura >= '" + dataInicial + "') " +
                    "GROUP BY tIngressoLog.Acao, tEvento.ID " +
                    "HAVING  (tIngressoLog.Acao = N'" + IngressoLog.CANCELAR + "') AND (tEvento.ID = " + this.Control.ID + ") ";
                bdCancelados.Consulta(sqlCancelados);
                if (bdCancelados.Consulta().Read())
                {
                    quantidadeCancelados = bdCancelados.LerInt("Quantidade");
                }
                bdCancelados.Fechar();
                #endregion
                quantidade = quantidadeVendidos - quantidadeCancelados;
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeCortesiasPorCaixa

        public override decimal ValorIngressosPorEvento(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoEvento = "";
                    if (this.Control.ID > 0)
                        condicaoEvento = "AND (tApresentacao.EventoID = " + this.Control.ID + ") ";
                    else
                        condicaoEvento = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   SUM(tPreco.Valor) AS Valor " +
                        "FROM     tApresentacao INNER JOIN " +
                        "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                        "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                        "tEvento ON tApresentacao.EventoID = tEvento.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tLocal ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                        "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoEvento;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do evento
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos eventos
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorEvento

        public override DataTable BorderoFormaPagamento(string apresentacoes)
        {
            try
            {
                #region Inicializando dados e filtrando condição
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos ou cancelados
                string condicaoVendidos;
                string condicaoCancelados;
                DataTable tabela = LinhasBorderoFormaPagamento(apresentacoes);
                decimal quantidadeTotalTotais = 0;
                decimal quantidadePDVTotais = 0;
                decimal quantidadeBilheteriaTotais = 0;
                decimal quantidadeCallCenterTotais = 0;
                decimal quantidadeInternetTotais = 0;
                decimal valorTotalTotais = 0;
                decimal valorPDVTotais = 0;
                decimal valorBilheteriaTotais = 0;
                decimal valorCallCenterTotais = 0;
                decimal valorInternetTotais = 0;
                string canais = "";
                string empresaNome = "";
                DataTable canaisTabela;
                // Informações sobre Empresa deste Evento
                int empresaID = this.EmpresaID();
                Empresa empresa = new Empresa();
                empresa.Ler(empresaID);
                empresaNome = empresa.Nome.Valor;
                #endregion
                #region Para cada forma de pagamento na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    int formaPagamentoID = Convert.ToInt32(linha["FormaPagamentoID"]);
                    if (formaPagamentoID > 0)
                    {
                        FormaPagamento formaPagamento = new FormaPagamento();
                        formaPagamento.Ler(formaPagamentoID);
                        #region Todos Canais da empresa
                        canaisTabela = empresa.Canais();
                        canais = "";
                        for (int indice = 0; indice < canaisTabela.Rows.Count; indice++)
                        {
                            if (indice == 0)
                                canais = canaisTabela.Rows[indice]["ID"].ToString();
                            else
                                canais = canais + "," + canaisTabela.Rows[indice]["ID"].ToString();
                        }
                        condicaoVendidos = this.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
                        condicaoCancelados = this.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
                        // Quantidade (diferença de vendidos com cancelados)
                        linha["Bilheteria Qtd"] =
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        quantidadeBilheteriaTotais += Convert.ToDecimal(linha["Bilheteria Qtd"]);
                        // Formato
                        linha["Bilheteria Qtd"] = Convert.ToDecimal(linha["Bilheteria Qtd"]).ToString(Utilitario.FormatoMoeda);
                        // Valor (diferença de vendidos com cancelados)
                        linha["Bilheteria Valor"] =
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        valorBilheteriaTotais += Convert.ToDecimal(linha["Bilheteria Valor"]);
                        // Formato
                        linha["Bilheteria Valor"] = Math.Round(Convert.ToDecimal(linha["Bilheteria Valor"]), 2).ToString(Utilitario.FormatoMoeda);
                        #endregion
                        #region Call Center
                        canais = "1";
                        condicaoVendidos = this.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
                        condicaoCancelados = this.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
                        // Quantidade (diferença de vendidos com cancelados)
                        linha["Call Center Qtd"] =
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        quantidadeCallCenterTotais += Convert.ToDecimal(linha["Call Center Qtd"]);
                        // Formato
                        linha["Call Center Qtd"] = Convert.ToDecimal(linha["Call Center Qtd"]).ToString(Utilitario.FormatoMoeda);
                        // Valor (diferença de vendidos com cancelados)
                        linha["Call Center Valor"] =
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        valorCallCenterTotais += Convert.ToDecimal(linha["Call Center Valor"]);
                        // Formato
                        linha["Call Center Valor"] = Math.Round(Convert.ToDecimal(linha["Call Center Valor"]), 2).ToString(Utilitario.FormatoMoeda);
                        #endregion
                        #region Internet
                        canais = "2";
                        condicaoVendidos = this.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
                        condicaoCancelados = this.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
                        // Quantidade (diferença de vendidos com cancelados)
                        linha["Internet Qtd"] =
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        quantidadeInternetTotais += Convert.ToDecimal(linha["Internet Qtd"]);
                        // Formato
                        linha["Internet Qtd"] = Convert.ToDecimal(linha["Internet Qtd"]).ToString(Utilitario.FormatoMoeda);
                        // Valor (diferença de vendidos com cancelados)
                        linha["Internet Valor"] =
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        valorInternetTotais += Convert.ToDecimal(linha["Internet Valor"]);
                        // Formato
                        linha["Internet Valor"] = Math.Round(Convert.ToDecimal(linha["Internet Valor"]), 2).ToString(Utilitario.FormatoMoeda);
                        #endregion
                        #region Totais (corresponde a todos canais que vendem)
                        canais = "";
                        canaisTabela = empresa.CanaisQueVendem(null);
                        for (int indice = 0; indice < canaisTabela.Rows.Count; indice++)
                        {
                            if (indice == 0)
                                canais = canaisTabela.Rows[indice]["ID"].ToString();
                            else
                                canais = canais + "," + canaisTabela.Rows[indice]["ID"].ToString();
                        }
                        condicaoVendidos = this.IngressoLogID(apresentacoes, ingressoLog.Vendidos, canais);
                        condicaoCancelados = this.IngressoLogID(apresentacoes, ingressoLog.Cancelados, canais);
                        //						for (int indice =0; indice< canaisTabela.Rows.Count; indice++) {
                        //							if (indice ==0) 
                        //								canais = canaisTabela.Rows[indice]["ID"].ToString();
                        //							else
                        //								canais = canais +","+canaisTabela.Rows[indice]["ID"].ToString();
                        //						}
                        // Quantidade (diferença de vendidos com cancelados)
                        linha["Total Qtd"] =
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.QuantidadeIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        quantidadeTotalTotais += Convert.ToDecimal(linha["Total Qtd"]);
                        // Formato
                        linha["Total Qtd"] = Convert.ToDecimal(linha["Total Qtd"]).ToString(Utilitario.FormatoMoeda);
                        // Valor (diferença de vendidos com cancelados)
                        linha["Total Valor"] =
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoVendidos) -
                            formaPagamento.ValorIngressosPorFormaPagamento(condicaoCancelados);
                        // Totalizando
                        valorTotalTotais += Convert.ToDecimal(linha["Total Valor"]);
                        // Formato
                        linha["Total Valor"] = Math.Round(Convert.ToDecimal(linha["Total Valor"]), 2).ToString(Utilitario.FormatoMoeda);
                        #endregion
                        #region Pontos de Venda (restante)
                        // Quantidade (diferença de vendidos com cancelados)
                        linha["PDV Qtd"] =
                            Convert.ToDecimal(linha["Total Qtd"]) -
                            Convert.ToDecimal(linha["Bilheteria Qtd"]) -
                            Convert.ToDecimal(linha["Call Center Qtd"]) -
                            Convert.ToDecimal(linha["Internet Qtd"]);
                        // Totalizando
                        quantidadePDVTotais += Convert.ToDecimal(linha["PDV Qtd"]);
                        // Formato
                        linha["PDV Qtd"] = Convert.ToDecimal(linha["PDV Qtd"]).ToString(Utilitario.FormatoMoeda);
                        // Valor (diferença de vendidos com cancelados)
                        linha["PDV Valor"] =
                            Convert.ToDecimal(linha["Total Valor"]) -
                            Convert.ToDecimal(linha["Bilheteria Valor"]) -
                            Convert.ToDecimal(linha["Call Center Valor"]) -
                            Convert.ToDecimal(linha["Internet Valor"]);
                        // Totalizando
                        valorPDVTotais += Convert.ToDecimal(linha["PDV Valor"]);
                        // Formato
                        linha["PDV Valor"] = Math.Round(Convert.ToDecimal(linha["PDV Valor"]), 2).ToString(Utilitario.FormatoMoeda);
                        #endregion
                    }
                } // laço do FormaPagamento
                #endregion
                #region Valor do Total por coluna
                DataRow linhaBranco = tabela.NewRow();
                tabela.Rows.Add(linhaBranco);
                DataRow linhaTotais = tabela.NewRow();
                linhaTotais["FormaPagamento"] = "Totais";
                linhaTotais["Total Qtd"] = quantidadeTotalTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["PDV Qtd"] = quantidadePDVTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Bilheteria Qtd"] = quantidadeBilheteriaTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Call Center Qtd"] = quantidadeCallCenterTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Internet Qtd"] = quantidadeInternetTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Total Valor"] = valorTotalTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["PDV Valor"] = valorPDVTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Bilheteria Valor"] = valorBilheteriaTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Call Center Valor"] = valorCallCenterTotais.ToString(Utilitario.FormatoMoeda);
                linhaTotais["Internet Valor"] = valorInternetTotais.ToString(Utilitario.FormatoMoeda);
                //
                tabela.Rows.Add(linhaTotais);
                tabela.Columns["Bilheteria Qtd"].ColumnName = "Canais prop Qtd";
                tabela.Columns["Bilheteria Valor"].ColumnName = "Canais prop Valor";
                #endregion
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // BorderoFormaPagamento

        public string IngressoLogID(string apresentacoes, string acao, TipoPaganteOuCortesia pagantes)
        {
            try
            {
                string cortesiaCondicao = "";
                switch (pagantes)
                {
                    case TipoPaganteOuCortesia.Pagante:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID = 0)";
                        break;
                    case TipoPaganteOuCortesia.Cortesia:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID > 0)";
                        break;
                    case TipoPaganteOuCortesia.Ambos:
                        cortesiaCondicao = " AND (tIngressoLog.CortesiaID >= 0)";
                        break;
                }
                // Obtendo dados através de SQL
                BD obterDados = new BD();
                string sql =
                    "SELECT    tIngressoLog.ID, tApresentacao.EventoID, tApresentacaoSetor.ApresentacaoID, tIngresso.CortesiaID " +
                    "FROM      tIngressoLog INNER JOIN " +
                    "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                    "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                    "WHERE     (tApresentacao.EventoID = " + this.Control.ID + ") AND (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + "))  AND (tIngressoLog.Acao = " + acao + ") " + cortesiaCondicao;
                obterDados.Consulta(sql);
                bool primeiraVez = true;
                string logs = "";
                while (obterDados.Consulta().Read())
                {
                    if (primeiraVez)
                    {
                        logs = obterDados.LerInt("ID").ToString();
                        primeiraVez = false;
                    }
                    else
                    {
                        logs = logs + "," + obterDados.LerInt("ID").ToString();
                    }
                }
                obterDados.Fechar();
                return logs;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public string IngressoLogID(string apresentacoes, string acao, string canais)
        {
            try
            {
                string logs = "";
                if (canais != "")
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT   tIngressoLog.ID " +
                        "FROM     tEvento (NOLOCK) INNER JOIN " +
                        "tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                        "tApresentacaoSetor (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                        "tIngresso (NOLOCK) ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                        "tIngressoLog (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                        "tVendaBilheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                        "tVendaBilheteria (NOLOCK) ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID INNER JOIN " +
                        "tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID " +
                        "WHERE    (tApresentacao.EventoID = " + this.Control.ID + ") AND (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) AND (tIngressoLog.Acao = " + acao + ") AND (tLoja.CanalID IN (" + canais + ")) ";
                    obterDados.Consulta(sql);
                    bool primeiraVez = true;
                    while (obterDados.Consulta().Read())
                    {
                        if (primeiraVez)
                        {
                            logs = obterDados.LerInt("ID").ToString();
                            primeiraVez = false;
                        }
                        else
                        {
                            logs = logs + "," + obterDados.LerInt("ID").ToString();
                        }
                    }
                    obterDados.Fechar();
                }
                return logs;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public override DataTable LinhasBorderoFormaPagamento(string apresentacoes)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaBorderoPorFormaPagamento();
                // Obtendo dados através de SQL
                BD obterDados = new BD();
                string sql =
                    "SELECT DISTINCT tFormaPagamento.Nome, tVendaBilheteriaFormaPagamento.FormaPagamentoID " +
                    "FROM            tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID INNER JOIN " +
                    "tApresentacaoSetor ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID INNER JOIN " +
                    "tVendaBilheteriaItem ON tIngressoLog.VendaBilheteriaItemID = tVendaBilheteriaItem.ID INNER JOIN " +
                    "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                    "tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID = tVendaBilheteriaFormaPagamento.VendaBilheteriaID INNER JOIN " +
                    "tFormaPagamento ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID " +
                    "WHERE        (tApresentacao.EventoID = " + this.Control.ID + ") AND (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) " +
                    "ORDER BY tFormaPagamento.Nome ";
                obterDados.Consulta(sql);
                while (obterDados.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["FormaPagamentoID"] = obterDados.LerInt("FormaPagamentoID");
                    linha["FormaPagamento"] = obterDados.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                obterDados.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public DataTable Listagem(int localID)
        {
            try
            {
                DataTable tabela = new DataTable("ListagemEvento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Evento", typeof(string));
                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Descrição Resumida", typeof(string));
                tabela.Columns.Add("Descrição Detalhada", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Obtendo dados
                string condicao = "";
                if (localID > 0)
                    condicao = " WHERE        (tEvento.LocalID = " + localID + ") ";
                else
                    condicao = "  ";
                string sql;
                sql =
                    "SELECT        tEmpresa.Nome AS Empresa, tLocal.Nome AS Local, tEvento.Nome AS Evento, tEvento.ID, tEventoTipo.Nome AS EventoTipo, " +
                    "tEvento.DescricaoResumida, tEvento.DescricaoDetalhada, tEvento.Obs " +
                    "FROM            tEvento (NOLOCK) INNER JOIN " +
                    "tEventoTipo (NOLOCK) ON tEvento.EventoTipoID = tEventoTipo.ID INNER JOIN " +
                    "tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID INNER JOIN " +
                    "tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tLocal.Nome, tEvento.Nome, tEventoTipo.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Local"] = bd.LerString("Local");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Tipo"] = bd.LerString("EventoTipo");
                    linha["Descrição Resumida"] = bd.LerString("DescricaoResumida");
                    linha["Descrição Detalhada"] = bd.LerString("DescricaoDetalhada");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de Listagem

        public DataTable EventoPorLocal(int localID, string registroZero)
        {
            try
            {
                DataTable dttEventos = new DataTable();
                dttEventos.Columns.Add("ID", typeof(int));
                dttEventos.Columns.Add("Evento", typeof(string));

                if (registroZero != null)
                    dttEventos.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK) INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID WHERE LocalID = " + localID + " AND tApresentacao.DisponivelAjuste = 'T' ORDER BY Nome ";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = dttEventos.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Evento"] = bd.LerString("Nome");
                    dttEventos.Rows.Add(linha);
                }
                return dttEventos;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> EventoPorLocal(int localID, bool registroZero)
        {
            try
            {
                List<IRLib.Paralela.ClientObjects.EstruturaIDNome> lista = new List<IRLib.Paralela.ClientObjects.EstruturaIDNome>();
                if (registroZero)
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                string sql = string.Format(@"SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK)
                                INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                                WHERE LocalID = {0} AND tApresentacao.DisponivelAjuste = 'T' ORDER BY Nome ", localID);
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public object ApresentacoesSemIngresso(string registroZero)
        {
            DataRow linha;

            // Estrutura de retorno
            DataTable tabela = new DataTable("ApresentacoesSemIngresso");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Horario", typeof(string));

            try
            {
                // Valor padrão
                if (registroZero != null)
                {
                    linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Horario"] = registroZero;
                    tabela.Rows.Add(linha);
                }

                // Se o ID da Evento for preenchido
                if (this.Control.ID > 0)
                {
                    // Preenche as apresentações do evento disponíveis para ajuste e não tenham ingressos gerados
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT DISTINCT " +
                        "	tApresentacao.ID, " +
                        "	tApresentacao.Horario " +
                        "FROM tApresentacao (NOLOCK) " +
                        "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "WHERE " +
                        "	(tApresentacao.EventoID = " + this.Control.ID + ") " +
                        "AND " +
                        "   (tApresentacao.DisponivelAjuste = 'T') " +
                        "AND " +
                        "	(tApresentacaoSetor.IngressosGerados = 'F') " +
                        "AND " +
                        "   (tSetor.LugarMarcado <> '" + Setor.Pista + "') " +
                        "ORDER BY tApresentacao.Horario"))
                    {

                        while (oDataReader.Read())
                        {
                            linha = tabela.NewRow();
                            linha["ID"] = bd.LerInt("ID");
                            linha["Horario"] = bd.LerDateTime("Horario").ToString();
                            tabela.Rows.Add(linha);
                        }
                    }

                    bd.Fechar();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public List<EstruturaTaxasEventoPacotes> PesquisaPacotesEvento(int EventoID)
        {
            List<EstruturaTaxasEventoPacotes> lRetorno = new List<EstruturaTaxasEventoPacotes>();

            try
            {
                string sql = string.Empty;

                sql = "EXEC Proc_IdentificaPacotesPossuemEvento " + EventoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaTaxasEventoPacotes
                    {
                        PacoteID = bd.LerInt("PacoteID"),
                        Quantidade = bd.LerInt("Quantidade"),
                        CanalPacoteId = bd.LerInt("CanalPacoteID"),
                        ID = bd.LerInt("ID"),
                        CanalID = bd.LerInt("CanalID"),
                        EventoID = bd.LerInt("EventoID"),
                        TaxaConveniencia = bd.LerInt("TaxaConveniencia"),
                        TaxaMinima = bd.LerDecimal("TaxaMinima"),
                        TaxaMaxima = bd.LerDecimal("TaxaMaxima"),
                        TaxaComissao = bd.LerInt("TaxaComissao"),
                        ComissaoMinima = bd.LerDecimal("ComissaoMinima"),
                        ComissaoMaxima = bd.LerDecimal("ComissaoMaxima")
                    });
                }
                return lRetorno;
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

        public List<EstruturaEventoNivelRisco> RetornaEventoNivelRisco(string busca)
        {
            List<EstruturaEventoNivelRisco> lRetorno = new List<EstruturaEventoNivelRisco>();
            try
            {
                string sql = string.Empty;

                sql = @"SELECT DISTINCT e.ID AS EventoID, e.Nome AS Evento, l.Nome AS Local , ISNull(e.NivelRisco, 0) AS NivelRisco
                FROM tEvento AS e INNER JOIN tLocal AS l ON e.LocalID = l.ID
                INNER JOIN tApresentacao AS a ON e.ID = a.EventoID
                WHERE e.Nome like '%" + busca + "%' AND a.Horario >= '" + DateTime.Now.ToString("yyyyMMdd") + "000000' ORDER BY e.ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaEventoNivelRisco
                    {
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Evento"),
                        Local = bd.LerString("Local"),
                        NivelRisco = bd.LerInt("NivelRisco")
                    });
                }
                return lRetorno;
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

        public bool AlteraNivelEvento(int EventoID)
        {
            try
            {
                this.Ler(EventoID);

                if (this.NivelRisco.Valor == (int)NivelRiscoEvento.SemRisco)
                    this.NivelRisco.Valor = (int)NivelRiscoEvento.ComRisco;
                else if (this.NivelRisco.Valor == (int)NivelRiscoEvento.ComRisco)
                    this.NivelRisco.Valor = (int)NivelRiscoEvento.SemRisco;

                bool alterado = Atualizar();
                return alterado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetNivelRiscoEvento(string EventoID)
        {
            try
            {
                int nivelRisco = 0;
                string sql = string.Empty;

                sql = @"SELECT DISTINCT NivelRisco FROM tEvento (NOLOCK) WHERE tEvento.ID IN  (" + EventoID + ")";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("NivelRisco") > 0)
                    {
                        nivelRisco = 1;
                    }
                }

                return nivelRisco;
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

        public int GetNivelRiscoEventoPorApresentacaoID(string ApresentacaoID)
        {
            try
            {
                int nivelRisco = 0;
                string sql = string.Empty;

                sql = @"SELECT DISTINCT NivelRisco FROM tEvento (NOLOCK)
                        INNER JOIN tApresentacao (NOLOCK) on tApresentacao.EventoID = tEvento.ID
                        WHERE tApresentacao.ID IN  (" + ApresentacaoID + ")";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("NivelRisco") > 0)
                    {
                        nivelRisco = 1;
                    }
                }

                return nivelRisco;
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

        public List<EstruturaEntregaAgenda> ListaRetiradaVIR()
        {
            try
            {
                List<EstruturaEntregaAgenda> lRetorno = new List<EstruturaEntregaAgenda>();



                string sql = @"select tEntregaControle.EntregaID , 
						tEntregaControle.ID as EntregaControleID , Nome, 
						PrazoEntrega,  
                        CASE WHEN
								LEN(tEventoEntregaControle.ProcedimentoEntrega) > 0
									THEN tEventoEntregaControle.ProcedimentoEntrega
								ELSE
									CASE WHEN
										LEN(tEntregaControle.ProcedimentoEntrega) > 0
										THEN tEntregaControle.ProcedimentoEntrega
										ELSE tEntrega.ProcedimentoEntrega
									END
						END as ProcedimentoEntrega,
                        CASE WHEN
								tEventoEntregaControle.DiasTriagem > 0
									THEN tEventoEntregaControle.DiasTriagem
								ELSE
									CASE WHEN
										tEntregaControle.DiasTriagem > 0
										THEN tEntregaControle.DiasTriagem
										ELSE tEntrega.DiasTriagem
									END
						END as DiasTriagem,                          
                        Tipo,tEntregaControle.Valor
                        from tEventoEntregaControle
                        inner join tEntregaControle on  tEventoEntregaControle.EntregaControleID = tEntregaControle.ID
                        inner join tEntrega on tEntregaControle.EntregaID = tEntrega.ID
                        where tEntrega.Tipo in ('R')
                        group by tEntregaControle.EntregaID,tEntregaControle.ID  , Nome, PrazoEntrega,tEntregaControle.DiasTriagem,tEventoEntregaControle.DiasTriagem,  tEventoEntregaControle.ProcedimentoEntrega ,  
                        tEntrega.ProcedimentoEntrega, Tipo, tEntrega.DiasTriagem,tEntregaControle.Valor,tEntregaControle.ProcedimentoEntrega
                        order by Nome,tEntregaControle.EntregaID";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaEntregaAgenda
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        Tipo = bd.LerString("Tipo"),
                        DiasTriagem = bd.LerInt("DiasTriagem"),
                        Valor = bd.LerInt("Valor")
                    });
                }

                bd.Fechar();

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EstruturaEntregaAgenda> ListaEntregaVIR(string CEP)
        {
            try
            {
                List<EstruturaEntregaAgenda> lRetorno = new List<EstruturaEntregaAgenda>();

                string strCEP = "";
                strCEP = " ( CepInicial < " + CEP + " and CepFinal > " + CEP + " ) ";

                string sql = @"SELECT tEntregaControle.EntregaID , 
                        tEntregaControle.ID as EntregaControleID , 
                        tEntrega.Nome, 
                        PrazoEntrega, 
                        CASE WHEN
		                        tEntregaControle.DiasTriagem > 0
			                        THEN tEntregaControle.DiasTriagem
		                        ELSE
			                        CASE WHEN
				                        tEntregaControle.DiasTriagem > 0
				                        THEN tEntregaControle.DiasTriagem
				                        ELSE tEntrega.DiasTriagem
			                        END
                        END as DiasTriagem,
                        CASE WHEN
		                        LEN(tEntregaControle.ProcedimentoEntrega) > 0
			                        THEN tEntregaControle.ProcedimentoEntrega
		                        ELSE
			                        CASE WHEN
				                        LEN(tEntregaControle.ProcedimentoEntrega) > 0
				                        THEN tEntregaControle.ProcedimentoEntrega
				                        ELSE tEntrega.ProcedimentoEntrega
			                        END
                        END as ProcedimentoEntrega,
                        Tipo, 
                        tEntregaControle.Valor,
                        ISNULL(tEntregaPeriodo.Nome,'') AS Periodo
                        FROM tEntregaControle
                        INNER JOIN tEntrega ON tEntregaControle.EntregaID = tEntrega.ID
                        INNER JOIN tEntregaAreaCep ON tEntregaControle.EntregaAreaID = tEntregaAreaCep.EntregaAreaID
                        LEFT JOIN tEntregaPeriodo ON tEntregaControle.PeriodoID = tEntregaPeriodo.ID
                        where " + (CEP.Length > 0 ? strCEP : "") + @"AND ((tEntregaControle.QuantidadeEntregas > 0 AND Tipo = 'A') OR (Tipo = 'N'))" +
                        @"AND tEntregaControle.EntregaID IN (1,2,5,6)
                        GROUP BY tEntregaControle.EntregaID,tEntregaControle.ID , tEntrega.Nome, PrazoEntrega, tEntregaControle.DiasTriagem,  
                        tEntrega.ProcedimentoEntrega, Tipo, tEntrega.DiasTriagem,tEntregaControle.Valor,tEntregaControle.ProcedimentoEntrega, 
                        tEntregaControle.DiasTriagem ,tEntregaPeriodo.Nome, tEntregaControle.ProcedimentoEntrega
                        ORDER BY tEntregaControle.ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaEntregaAgenda
                    {
                        EntregaID = bd.LerInt("EntregaID"),
                        EntregaControleID = bd.LerInt("EntregaControleID"),
                        Nome = bd.LerString("Nome"),
                        PrazoEntrega = bd.LerInt("PrazoEntrega"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        Tipo = bd.LerString("Tipo"),
                        DiasTriagem = bd.LerInt("DiasTriagem"),
                        Valor = bd.LerDecimal("Valor"),
                        Periodo = bd.LerString("Periodo")

                    });
                }

                bd.Fechar();

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MenorPeriodoEntrega(int eventoID, BD bd)
        {
            try
            {
                int dias = Convert.ToInt32(bd.ConsultaValor(@"SELECT
                                    IsNull(MIN(PrazoEntrega + 
	                                CASE WHEN eec.DiasTriagem > 0
		                                THEN eec.DiasTriagem
		                                ELSE
			                                CASE WHEN ec.DiasTriagem > 0
				                                THEN ec.DiasTriagem
				                                ELSE te.DiasTriagem
			                                END
		                                END), 0)
	                                FROM tEvento e (NOLOCK) 
	                                INNER JOIN tEventoEntregaControle eec (NOLOCK) ON eec.EventoID = e.ID
	                                INNER JOIN tEntregaControle ec (NOLOCK) ON ec.ID = eec.EntregaControleID
	                                INNER JOIN tEntrega te (NOLOCK) ON te.ID = ec.EntregaID
	                                WHERE e.ID = " + eventoID));

                bd.Executar("UPDATE tEvento SET MenorPeriodoEntrega = " + dias + " WHERE ID = " + eventoID);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void MenorPeriodoEntrega(int eventoID)
        {
            try
            {
                int dias = Convert.ToInt32(bd.ConsultaValor(@"SELECT
                                    IsNull(MIN(PrazoEntrega + 
	                                CASE WHEN eec.DiasTriagem > 0
		                                THEN eec.DiasTriagem
		                                ELSE
			                                CASE WHEN ec.DiasTriagem > 0
				                                THEN ec.DiasTriagem
				                                ELSE te.DiasTriagem
			                                END
		                                END), 0)
	                                FROM tEvento e (NOLOCK) 
	                                INNER JOIN tEventoEntregaControle eec (NOLOCK) ON eec.EventoID = e.ID
	                                INNER JOIN tEntregaControle ec (NOLOCK) ON ec.ID = eec.EntregaControleID
	                                INNER JOIN tEntrega te (NOLOCK) ON te.ID = ec.EntregaID
	                                WHERE e.ID = " + eventoID));

                bd.Executar("UPDATE tEvento SET MenorPeriodoEntrega = " + dias + " WHERE ID = " + eventoID);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Enumerators.TipoCodigoBarra BuscarTipoCodigoBarra(int eventoID)
        {
            try
            {
                return (Enumerators.TipoCodigoBarra)Convert.ToChar(bd.ConsultaValor("SELECT IsNull(TipoCodigoBarra, 'E') FROM tEvento (NOLOCK) WHERE ID = " + eventoID));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaEvento> ListarEventosAssinatura(int LocalID, int Ano)
        {

            try
            {
                List<EstruturaAssinaturaEvento> retorno = new List<EstruturaAssinaturaEvento>();

                string sql = @"SELECT DISTINCT tEvento.ID, tEvento.Nome 
                                FROM tEvento (nolock)
                                INNER JOIN tApresentacao (nolock) ON tEvento.ID = tApresentacao.EventoID
                                WHERE LocalID = " + LocalID + " AND Horario BETWEEN '" + Ano + "0000000000' AND '" + Ano + "9999999999' ORDER BY tEvento.Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaAssinaturaEvento
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Incluir = false
                    });
                }

                bd.Fechar();


                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<EstruturaAssinaturaEvento> ListarEventosAssinatura(int LocalID, int Ano, int AssinaturaID)
        {
            try
            {
                List<EstruturaAssinaturaEvento> retorno = new List<EstruturaAssinaturaEvento>();

                string sql = @"SELECT tEvento.ID,tEvento.Nome,
                                CASE WHEN COUNT(tAssinaturaAno.AssinaturaID) > 0
                                    THEN 'T'
                                    ELSE 'F'
                                    END as Incluir
                                FROM tEvento (nolock)
                                INNER JOIN tApresentacao (nolock) ON tEvento.ID = tApresentacao.EventoID
                                LEFT JOIN tAssinaturaItem (nolock) ON tAssinaturaItem.ApresentacaoID = tApresentacao.ID
                                LEFT JOIN tAssinaturaAno (nolock) ON tAssinaturaItem.AssinaturaAnoID = tAssinaturaAno.ID
                                and tAssinaturaAno.AssinaturaID = " + AssinaturaID + @" and Ano = " + Ano + @" 
                                WHERE LocalID = " + LocalID + " AND Horario BETWEEN '" + Ano + "0000000000' AND '" + Ano + @"9999999999' 
                                GROUP BY tEvento.ID,tEvento.Nome ORDER BY tEvento.Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaAssinaturaEvento
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Incluir = bd.LerBoolean("Incluir"),
                    });
                }

                bd.Fechar();


                return retorno;
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

        public List<EstruturaAssinaturaApresentacao> ListarApresentacaoAssinatura(List<int> EventosID, int Ano)
        {
            try
            {
                List<EstruturaAssinaturaApresentacao> retorno = new List<EstruturaAssinaturaApresentacao>();

                string sql = @"SELECT tApresentacao.ID, tEvento.Nome AS Evento,tApresentacao.Horario
                                FROM tEvento (nolock)
                                INNER JOIN tApresentacao (nolock) ON tEvento.ID = tApresentacao.EventoID
                                WHERE tEvento.ID in (" + Utilitario.ArrayToString(EventosID.ToArray()) + ") AND Horario BETWEEN " + Ano + "0000000000 AND " + Ano + @"9999999999 
                                ORDER BY tEvento.Nome, tApresentacao.Horario";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DateTime horario = bd.LerDateTime("Horario");
                    int diaSemana = Convert.ToInt32(horario.DayOfWeek);
                    if (diaSemana == 0) { diaSemana = 7; }
                    string dia = horario.ToString("dd/MM/yy");
                    string hora = horario.ToString("HH:mm");

                    retorno.Add(new EstruturaAssinaturaApresentacao
                    {

                        ID = bd.LerInt("ID"),
                        Evento = bd.LerString("Evento"),
                        Horario = dia + " às " + hora,
                        DiaDaSemana = diaSemana,
                        Incluir = false
                    });
                }

                bd.Fechar();


                return retorno;
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

        public List<EstruturaAssinaturaApresentacao> ListarApresentacaoAssinatura(List<int> EventosID, int Ano, int AssinaturaID)
        {
            try
            {
                List<EstruturaAssinaturaApresentacao> retorno = new List<EstruturaAssinaturaApresentacao>();

                string sql = @"SELECT tApresentacao.ID, tEvento.Nome AS Evento,tApresentacao.Horario,
                                CASE WHEN COUNT(tAssinaturaAno.AssinaturaID) > 0
                                THEN 'T'
                                ELSE 'F'
                                END as Incluir
                                FROM tEvento (nolock)
                                INNER JOIN tApresentacao (nolock) ON tEvento.ID = tApresentacao.EventoID
                                LEFT JOIN tAssinaturaItem (nolock) ON tAssinaturaItem.ApresentacaoID = tApresentacao.ID
                                LEFT JOIN tAssinaturaAno (nolock) ON tAssinaturaItem.AssinaturaAnoID = tAssinaturaAno.ID 
                                AND tAssinaturaAno.AssinaturaID = " + AssinaturaID + @" AND tAssinaturaAno.Ano = " + Ano + @"
                                WHERE tEvento.ID in (" + Utilitario.ArrayToString(EventosID.ToArray()) + ") AND Horario BETWEEN " + Ano + "0000000000 AND " + Ano + @"9999999999 
                                GROUP BY tApresentacao.ID,tEvento.Nome,tApresentacao.Horario ORDER BY tEvento.Nome, tApresentacao.Horario";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DateTime horario = bd.LerDateTime("Horario");
                    int diaSemana = Convert.ToInt32(horario.DayOfWeek);
                    if (diaSemana == 0) { diaSemana = 7; }
                    string dia = horario.ToString("dd/MM/yy");
                    string hora = horario.ToString("HH:mm");

                    retorno.Add(new EstruturaAssinaturaApresentacao
                    {

                        ID = bd.LerInt("ID"),
                        Evento = bd.LerString("Evento"),
                        Horario = dia + " às " + hora,
                        DiaDaSemana = diaSemana,
                        Incluir = bd.LerBoolean("Incluir"),
                    });
                }

                bd.Fechar();


                return retorno;
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

        public decimal BuscarValoresSangria(int CanalID, int CaixaID, int EventoID, DateTime Data, bool supervisor)
        {
            try
            {
                return this.BuscarCaixasSangria(CanalID, CaixaID, EventoID, Data, supervisor).Total;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EstruturaTelaSangria BuscarCaixasSangria(int CanalID, int CaixaID, int EventoID, DateTime Data, bool supervisor)
        {
            try
            {
                EstruturaTelaSangria estRetorno = new EstruturaTelaSangria();
                List<EstruturaSangria> lstAux = new List<EstruturaSangria>();
                List<EstruturaSangria> lstSangria = new List<EstruturaSangria>();
                List<int> lstID = new List<int>();

                string filtro = " AND tLoja.CanalID = " + CanalID;
                string filtroSangria = " tLoja.CanalID = " + CanalID;

                if (EventoID > 0)
                {
                    filtro += " AND tIngresso.EventoID = " + EventoID;
                    filtroSangria += " AND tSangria.EventoID = " + EventoID;
                }

                if (supervisor)
                {
                    filtro += " AND tCaixa.DataAbertura like '" + Data.ToString("yyyyMMdd") + "%' ";
                    filtroSangria += " AND tCaixa.DataAbertura like '" + Data.ToString("yyyyMMdd") + "%' ";
                }
                else
                {
                    filtro += " AND tCaixa.ID = " + CaixaID;
                    filtroSangria += " AND tCaixa.ID = " + CaixaID;
                }

                string sql = @"SELECT tVendaBilheteria.CaixaID,tUsuario.Nome, 
                        ROUND((tVendaBilheteriaFormaPagamento.Valor * (((Sum(tPreco.Valor))* 100)/tVendaBilheteria.ValorTotal))/100, 2)  AS ValorDisponível
                        FROM tIngresso(NOLOCK)
                        INNER JOIN  tVendaBilheteria(NOLOCK)  ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                        INNER JOIN  tPreco(NOLOCK)  ON tPreco.ID = tIngresso.PrecoID
                        INNER JOIN  tVendaBilheteriaFormaPagamento(NOLOCK)  ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
                        INNER JOIN tFormaPagamento(NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID
                        INNER JOIN tCaixa(NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                        INNER JOIN tLoja(NOLOCK) ON tCaixa.LojaID = tLoja.ID
                        INNER JOIN tUsuario(NOLOCK) ON tCaixa.UsuarioID = tUsuario.ID
                        WHERE tFormaPagamento.Tipo = 1 " + filtro +
                        @"GROUP BY tVendaBilheteria.CaixaID,tVendaBilheteriaFormaPagamento.Valor,tUsuario.Nome,tVendaBilheteria.ValorTotal
                        ORDER BY tVendaBilheteria.CaixaID 

                        SELECT tVendaBilheteria.CaixaID,tVendaBilheteria.ValorTotal FROM tVendaBilheteria(NOLOCK)
                        INNER JOIN tSangria(NOLOCK) ON tVendaBilheteria.ID = tSangria.VendaBilheteriaID
                        INNER JOIN tCaixa(NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                        INNER JOIN tLoja(NOLOCK) ON tCaixa.LojaID = tLoja.ID
                        WHERE " + filtroSangria;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lstAux.Add(new EstruturaSangria
                    {
                        CaixaID = bd.LerInt("CaixaID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("ValorDisponível")
                    });
                }

                bd.Consulta().NextResult();

                while (bd.Consulta().Read())
                {
                    lstSangria.Add(new EstruturaSangria
                    {
                        CaixaID = bd.LerInt("CaixaID"),
                        Valor = bd.LerDecimal("ValorTotal")
                    });
                }

                decimal valorCaixa = 0;
                string nomeCaixa = "";

                foreach (int caixaID in lstAux.Select(c => c.CaixaID).Distinct())
                {
                    if (!lstID.Contains(caixaID))
                    {
                        lstID.Add(caixaID);
                        valorCaixa = 0;
                        nomeCaixa = "";

                        valorCaixa = lstAux.Where(c => c.CaixaID == caixaID).Sum(c => c.Valor);
                        valorCaixa -= lstSangria.Where(c => c.CaixaID == caixaID).Sum(c => c.Valor);
                        nomeCaixa = lstAux.FirstOrDefault(c => c.CaixaID == caixaID).Nome;

                        if (valorCaixa > 0)
                        {
                            estRetorno.Lista.Add(new EstruturaSangria
                            {
                                CaixaID = caixaID,
                                Nome = nomeCaixa,
                                Valor = valorCaixa
                            });
                        }
                    }
                }

                bd.Fechar();

                estRetorno.Total = estRetorno.Lista.Sum(c => c.Valor);

                return estRetorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> ListaEventoPorCategoria(int EventoTipoID)
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                string sql = string.Format(@"Select ID, Nome from tEvento Where EventoTipoID = {0} ORDER BY Nome ", EventoTipoID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public int AtualizaImagemDestaque(int EventoID)
        {
            try
            {
                string sql = string.Format("UPDATE tEvento SET ImagemDestaque = '' WHERE ID = {0}", EventoID);

                int x = bd.Executar(sql);

                bd.Fechar();

                return x;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int QuantidadeIngressoSetor(int ApresentacaoSetorID)
        {
            try
            {
                int retorno = 0;

                string sql = "select COUNT(ID) as QtdSetor from tIngresso (nolock) where ApresentacaoSetorID =  " + ApresentacaoSetorID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("QtdSetor");
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int QuantidadeDistribuida(int ApresentacaoSetorID)
        {
            try
            {
                int retorno = 0;

                string sql = "select Sum(Disponivel) as QtdDsitribuida from tGerenciamentoIngressos  (nolock) where ApresentacaoSetorID =" + ApresentacaoSetorID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("QtdDsitribuida");
                }

                return retorno;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool TipoGerenciado(int eventoID)
        {
            try
            {

                string sql = @"SELECT * 
                                FROM tEvento e(NOLOCK)
                                INNER JOIN tEventoSubTipo est(NOLOCK) on e.EventoSubTipoID = est.ID
                                WHERE est.EventoTipoID = 29 AND e.ID = " + eventoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    return true;
                }

                return false;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEventoApresentacao> GetEventoListaPos(int CanalID)
        {
            try
            {
                List<EstruturaEventoApresentacao> retorno = new List<EstruturaEventoApresentacao>();

                string sql = string.Format(@"EXEC bilheteria_EventosPOS {0}, '{1}' ", CanalID, DateTime.Now.ToString("yyyyMMdd"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaEventoApresentacao()
                    {
                        EventoID = bd.LerInt("ID"),
                        EventoNome = bd.LerString("Nome").ToCleanString(),
                        Cidade = bd.LerString("Cidade").ToCleanString(),
                        Estado = bd.LerString("Estado"),
                        Local = bd.LerString("Local").ToCleanString(),
                        Endereco = bd.LerString("Endereco").ToCleanString() + " " + bd.LerString("Numero"),
                        Bairro = bd.LerString("Bairro").ToCleanString(),
                        Horario = bd.LerString("Apresentacao").Substring(2, 2) + "  " + bd.LerString("Apresentacao").Substring(4, 8)
                    });
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaEventoApresentacao GetEventoPrecosListaPos(int EventoID)
        {
            try
            {
                EstruturaEventoApresentacao retorno = new EstruturaEventoApresentacao();

                string sql = string.Format(@"EXEC bilheteria_PrecosPos {0}, '{1}' ", EventoID, DateTime.Now.ToString("yyyyMMdd"));

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar as informações deste evento.");

                retorno.EventoID = bd.LerInt("ID");
                retorno.EventoNome = bd.LerString("Nome").ToCleanString();
                retorno.Horario = bd.LerString("Apresentacao");
                retorno.Cidade = bd.LerString("Cidade").ToCleanString();
                retorno.Estado = bd.LerString("Estado");
                retorno.Local = bd.LerString("Local").ToCleanString();
                retorno.Endereco = bd.LerString("Endereco").ToCleanString() + " " + bd.LerString("Numero");
                retorno.Bairro = bd.LerString("Bairro").ToCleanString();

                do
                {
                    retorno.EstruturaPreco.Add(new EstruturaEventoPrecos()
                    {
                        EventoID = EventoID,
                        PrecoID = bd.LerInt("PrecoID"),
                        Nome = bd.LerString("Setor").ToCleanString() + " - " + bd.LerString("NomePreco").ToCleanString(),
                        Valor = bd.LerDecimal("Valor").ToString().Replace(",", ".")
                    });
                } while (bd.Consulta().Read());

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaEventoApresentacao> GetEventoListaPosCompleta(int CanalID)
        {
            try
            {
                List<EstruturaEventoPrecos> ListaestruturaPreco = new List<EstruturaEventoPrecos>();
                List<EstruturaEventoApresentacao> retorno = new List<EstruturaEventoApresentacao>();
                EstruturaEventoApresentacao estruturaevento = new EstruturaEventoApresentacao();
                EstruturaEventoPrecos estruturaPreco = new EstruturaEventoPrecos();

                string sql = string.Format(@"EXEC bilheteria_EventosUpdatePOS {0}, '{1}' ", CanalID, DateTime.Now.ToString("yyyyMMdd"));

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (retorno.Where(c => c.EventoID == bd.LerInt("ID")).Count() == 0)
                    {
                        estruturaevento.EventoID = bd.LerInt("ID");
                        estruturaevento.EventoNome = bd.LerString("Nome").ToCleanString();
                        estruturaevento.Cidade = bd.LerString("Cidade").ToCleanString();
                        estruturaevento.Estado = bd.LerString("Estado");
                        estruturaevento.Local = bd.LerString("Local").ToCleanString();
                        estruturaevento.Endereco = bd.LerString("Endereco").ToCleanString() + " " + bd.LerString("Numero");
                        estruturaevento.Bairro = bd.LerString("Bairro").ToCleanString();
                        estruturaevento.Horario = "";
                        estruturaevento.EstruturaPreco = ListaestruturaPreco;

                        retorno.Add(estruturaevento);

                        ListaestruturaPreco = new List<EstruturaEventoPrecos>();
                    }

                    estruturaPreco.EventoID = bd.LerInt("ID");
                    estruturaPreco.Nome = bd.LerString("NomePreco");
                    estruturaPreco.PrecoID = bd.LerInt("PrecoID");
                    estruturaPreco.Nome = (DateTime.ParseExact(bd.LerString("Apresentacao"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)).ToString("dd \\de MMMM \\de yyyy") + " - " + bd.LerString("Nome").ToCleanString();
                    estruturaPreco.Valor = bd.LerDecimal("Valor").ToString().Replace(",", ".");

                    ListaestruturaPreco.Add(estruturaPreco);
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaVendaPos BuscaInfosVendaPos(int PrecoID)
        {
            try
            {
                EstruturaVendaPos retorno = new EstruturaVendaPos();

                string sql = string.Format(@"SELECT tEvento.ID AS EventoID,tApresentacao.ID AS ApresentacaoID, tApresentacaoSetor.ID AS ApresentacaoSetorID, tPreco.Nome, tSetor.LugarMarcado, tLocal.ID AS LocalID, tEmpresa.ID AS EmpresaID
                                            FROM tPreco (NOLOCK)
                                            INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID
                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID
                                            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID
                                            INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID
                                            INNER JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
                                            INNER JOIN tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID
                                            WHERE tPreco.ID = {0}", PrecoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno.EventoID = bd.LerInt("EventoID");
                    retorno.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    retorno.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                    retorno.PrecoID = PrecoID;
                    retorno.NomePreco = bd.LerString("Nome");
                    retorno.LugarMarcado = bd.LerString("LugarMarcado");
                    retorno.LocalID = bd.LerInt("LocalID");
                    retorno.EmpresaID = bd.LerInt("EmpresaID");
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int GerarCodigoPosEvento(int NumeroSalvar, bool editando, BD bd)
        {
            try
            {
                bool jaExiste = false;

                if (NumeroSalvar == 0)
                    NumeroSalvar = Convert.ToInt32(bd.ConsultaValor("SELECT IsNull(MAX(CodigoPos), 0) + 1 FROM tEvento (NOLOCK)"));
                else
                    jaExiste = Convert.ToInt32(bd.ConsultaValor("SELECT IsNull(ID, 0) FROM tEvento (NOLOCK) WHERE CodigoPos = " + NumeroSalvar)) > 0;

                if (NumeroSalvar > 999 || (jaExiste && editando))
                    bd.Executar("UPDATE tEvento SET CodigoPos = 0 WHERE CodigoPos = " + NumeroSalvar);

                return NumeroSalvar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GerarCodigoPosEvento(int NumeroSalvar, bool editando)
        {
            try
            {
                bool jaExiste = false;

                if (NumeroSalvar == 0)
                    NumeroSalvar = Convert.ToInt32(bd.ConsultaValor("SELECT IsNull(MAX(CodigoPos), 0) + 1 FROM tEvento (NOLOCK)"));
                else
                    jaExiste = Convert.ToInt32(bd.ConsultaValor("SELECT IsNull(ID, 0) FROM tEvento (NOLOCK) WHERE CodigoPos = " + NumeroSalvar)) > 0;

                if (NumeroSalvar > 999 || (jaExiste && editando))
                    bd.Executar("UPDATE tEvento SET CodigoPos = 0 WHERE CodigoPos = " + NumeroSalvar);

                return NumeroSalvar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable TodosPorLocal(int LocalID, string registroZero)
        {
            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = String.Format(@"SELECT ID,Nome FROM tEvento (NOLOCK) WHERE LocalID = {0} ORDER BY Nome", LocalID);

                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(string acao, BD BD)
        {
            try
            {
                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cEvento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                BD.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public new static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("LocalID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("VendaDistribuida", typeof(bool));

                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));

                tabela.Columns.Add("VersaoImagemVale", typeof(int));

                tabela.Columns.Add("VersaoImagemVale2", typeof(int));

                tabela.Columns.Add("VersaoImagemVale3", typeof(int));

                tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool));

                tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));

                tabela.Columns.Add("DesabilitaAutomatico", typeof(bool));

                tabela.Columns.Add("Resenha", typeof(string));

                tabela.Columns.Add("Publicar", typeof(string));

                tabela.Columns.Add("Destaque", typeof(bool));

                tabela.Columns.Add("PrioridadeDestaque", typeof(int));

                tabela.Columns.Add("ImagemInternet", typeof(string));

                tabela.Columns.Add("Parcelas", typeof(int));

                tabela.Columns.Add("EntregaGratuita", typeof(bool));

                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));

                tabela.Columns.Add("Financeiro", typeof(bool));

                tabela.Columns.Add("Atencao", typeof(string));

                tabela.Columns.Add("Censura", typeof(string));

                tabela.Columns.Add("EntradaAcompanhada", typeof(bool));

                tabela.Columns.Add("PDVSemConveniencia", typeof(string));

                tabela.Columns.Add("RetiradaIngresso", typeof(string));

                tabela.Columns.Add("MeiaEntrada", typeof(string));

                tabela.Columns.Add("Promocoes", typeof(string));

                tabela.Columns.Add("AberturaPortoes", typeof(string));

                tabela.Columns.Add("DuracaoEvento", typeof(string));

                tabela.Columns.Add("Release", typeof(string));

                tabela.Columns.Add("DescricaoPadraoApresentacao", typeof(string));

                tabela.Columns.Add("PublicarSemVendaMotivo", typeof(int));

                tabela.Columns.Add("ContratoID", typeof(int));

                tabela.Columns.Add("PermitirVendaSemContrato", typeof(bool));

                tabela.Columns.Add("LocalImagemMapaID", typeof(int));

                tabela.Columns.Add("DataAberturaVenda", typeof(DateTime));

                tabela.Columns.Add("EventoSubTipoID", typeof(int));

                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));

                tabela.Columns.Add("EscolherLugarMarcado", typeof(bool));

                tabela.Columns.Add("MapaEsquematicoID", typeof(int));

                tabela.Columns.Add("ExibeQuantidade", typeof(bool));

                tabela.Columns.Add("PalavraChave", typeof(string));

                tabela.Columns.Add("NivelRisco", typeof(int));

                tabela.Columns.Add("HabilitarRetiradaTodosPDV", typeof(bool));

                tabela.Columns.Add("TipoImpressao", typeof(string));

                tabela.Columns.Add("TipoCodigoBarra", typeof(string));

                tabela.Columns.Add("FilmeID", typeof(int));

                tabela.Columns.Add("ImagemDestaque", typeof(string));

                tabela.Columns.Add("LimiteMaximoIngressosEvento", typeof(int));

                tabela.Columns.Add("BaseCalculo", typeof(string));

                tabela.Columns.Add("TipoCalculoDesconto", typeof(string));

                tabela.Columns.Add("TipoCalculo", typeof(string));

                tabela.Columns.Add("CodigoPos", typeof(int));

                tabela.Columns.Add("VenderPos", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool VerificarVendas()
        {
            try
            {
                string strSql = string.Format(@"SELECT TOP 1 ID FROM tIngresso ti WHERE ti.EventoID = {0} AND (ti.Status = 'V' OR ti.Status = 'I' OR ti.Status = 'E')", this.Control.ID);

                bd.Consulta(strSql);

                if (bd.Consulta().Read())
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }
    } // fim da classe

    public class EventoLista : EventoLista_B
    {
        private int UsuarioIDLogado;
        public EventoLista() { }

        public EventoLista(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            UsuarioIDLogado = usuarioIDLogado;
        }

        public override DataTable Relatorio()
        {

            DataTable tabela = new DataTable("Evento");

            try
            {

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("DescricaoResumida", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = evento.Nome.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                {
                    //erro: nao carregou a lista
                    tabela = null;
                }

            }
            catch
            {
                tabela = null;
            }

            return tabela;

        }

        public void CarregarDisponivelAjuste(int localID)
        {

            try
            {

                string sql;
                if (localID == 0)
                    sql = "SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK), tApresentacao(NOLOCK) WHERE EventoID = tEvento.ID AND DisponivelAjuste = 'T' ORDER BY tEvento.Nome ";
                else
                    sql = "SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK), tApresentacao(NOLOCK) WHERE EventoID = tEvento.ID AND DisponivelAjuste = 'T' AND LocalID = " + localID + " ORDER BY tEvento.Nome ";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

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

        public DataTable TabelaDisponivelAjuste()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = "SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK), tApresentacao(NOLOCK) WHERE EventoID = tEvento.ID AND DisponivelAjuste = 'T' ORDER BY Nome ";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
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

        public DataTable TabelaDisponivelWEB()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = @"SELECT DISTINCT EventoID AS ID, Evento AS Nome
                        FROM vwInfoVendaInternet 
                        ORDER BY Evento";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
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

        public DataTable TabelaDisponivelAjuste(int regionalID)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = "     SELECT distinct tEvento.ID , LTRIM( RTRIM(  tEvento.Nome  ) ) AS Nome  " +
                        "       FROM tEvento (NOLOCK)" +
                        " INNER JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID " +
                        " INNER JOIN tEmpresa (NOLOCK) ON tLocal.EmpresaID = tEmpresa.ID " +
                        " INNER JOIN tRegional (NOLOCK) ON tEmpresa.RegionalID = tRegional.ID " +
                        " INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoId = tEvento.ID AND tApresentacao.DisponivelAjuste = 'T' " +
                        "      WHERE tRegional.ID = " + regionalID +
                        "   ORDER BY LTRIM( RTRIM(  tEvento.Nome  ) ) ";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
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

        public DataTable TabelaEventosPorLocal(string local)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = "SELECT DISTINCT te.ID, te.Nome " +
                       "FROM " +
                       "tEvento te " +
                       "INNER JOIN tLocal tl ON tl.ID = te.LocalID " +
                       "INNER JOIN tApresentacao ta ON ta.EventoID=te.ID " +
                       "WHERE ta.DisponivelAjuste = 'T' " +
                       "AND tl.ID = " + local;

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
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

        public DataTable TabelaEventosPorTipo(string tipo)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = "SELECT DISTINCT te.ID, te.Nome " +
                       "FROM " +
                       "tEvento te " +
                       "INNER JOIN tLocal tl ON tl.ID = te.LocalID " +
                       "INNER JOIN tApresentacao ta ON ta.EventoID=te.ID " +
                       "WHERE ta.DisponivelAjuste = 'T' " +
                       "AND te.EventoTipoID = " + tipo + "ORDER BY te.Nome";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
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

        public DataTable ListaIDNome()
        {
            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            string sql = @"SELECT DISTINCT EventoID AS ID, Evento AS Nome FROM vwInfoVenda2 ORDER BY Evento";
            while (bd.Consulta(sql).Read())
            {
                DataRow novalinha = tabela.NewRow();
                novalinha["ID"] = bd.LerInt(0);
                novalinha["Nome"] = bd.LerString(1);
                tabela.Rows.Add(novalinha);
            }
            bd.Fechar();
            return tabela;
        }

        public DataTable GetTaxasEntrega(int eventoID)
        {
            try
            {
                DataTable tabela = new DataTable("EventoTaxaEntrega");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT TaxaEntregaID AS ID, Nome
                            FROM tEventoTaxaEntrega (NOLOCK)
                            INNER JOIN tTaxaEntrega (NOLOCK) ON tTaxaEntrega.Id = tEventoTaxaEntrega.TaxaEntregaID
                            WHERE EventoID = " + eventoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void PesquisarEventosInternet(string filtro)
        {
            try
            {
                lista.Clear();

                bd.Consulta("EXEC Proc_EventosInternet @Filtro= '" + filtro.Replace("'", "''") + "'");

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("EventoID"));

                lista.TrimToSize();

                bd.Fechar();

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

        public void PesquisarEventosInternetImais(string filtro)
        {
            try
            {
                lista.Clear();

                bd.Consulta("EXEC proc_eventosInternet_Imais @Filtro= '" + filtro.Replace("'", "''") + "'");

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("EventoID"));

                lista.TrimToSize();

                bd.Fechar();

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

    }
}
