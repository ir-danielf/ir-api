/**************************************************
* Arquivo: Apresentacao.cs
* Gerado: 11/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ClientObjects.Arvore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    public class Apresentacao : Apresentacao_B
    {
        public const string IMPRESSAO_APRESENTACAO = "A";
        public const string IMPRESSAO_SOMENTE_DATA_APRESENTACAO = "S";
        public const string IMPRESSAO_VENDA = "V";
        public const string IMPRESSAO_NADA = "N";

        public const string COL_HORARIO_STRING = "HorarioString"; // Nome da coluna horario quando tipo = string e não dateTime. Evitar problemas de horário de verão.

        private int UsuarioIDLogado;

        [Flags]
        public enum Disponibilidade
        {
            Nula = 0,
            Vender = 1,
            Ajustar = 2,
            GerarRelatorio = 4
        } //nao alterar as valores das flags!!!!!!!!!!!!!!!!!!

        //addDisponibilidade(Apresentacao.Disponibilidade d){
        //		disponibilidade |= d;
        //
        //delDisponibilidade(Apresentacao.Disponibilidade d){
        //		disponibilidade &= ~d;

        public enum TipoDesabilitaAutomatico
        {
            Apresentacoes = 0,
            Relatorios = 1,
            Ajuste = 2
        }

        public Apresentacao() { }

        public void ApresentacoesPorFiltro()
        {

        }

        public void LerNolock(int id)
        {

            try
            {
                string sql = "SELECT * FROM tApresentacao (NOLOCK) WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.Horario.ValorBD = bd.LerString("Horario");
                    this.DisponivelVenda.ValorBD = bd.LerString("DisponivelVenda");
                    this.DisponivelAjuste.ValorBD = bd.LerString("DisponivelAjuste");
                    this.DisponivelRelatorio.ValorBD = bd.LerString("DisponivelRelatorio");
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.Impressao.ValorBD = bd.LerString("Impressao");
                    this.DescricaoPadrao.ValorBD = bd.LerString("DescricaoPadrao");
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.UltimoCodigoImpresso.ValorBD = bd.LerInt("UltimoCodigoImpresso").ToString();
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                }
                else
                {
                    this.Limpar();
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
        }

        #region Métodos de Manipulação do Pacote

        #region Atualizar

        /// <summary>
        /// Atualiza Apresentacao
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacao SET EventoID = @001, Horario = '@002', DisponivelVenda = '@003', DisponivelAjuste = '@004', DisponivelRelatorio = '@005', VersaoImagemIngresso = @006, VersaoImagemVale = @007, VersaoImagemVale2 = @008, VersaoImagemVale3 = @009, Impressao = '@010', DescricaoPadrao = '@012', Descricao = '@013', UltimoCodigoImpresso = @014, Quantidade = @015, QuantidadePorCliente = @016, CotaID = @017 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.UltimoCodigoImpresso.ValorBD);
                sql.Replace("@015", this.Quantidade.ValorBD);
                sql.Replace("@016", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@017", this.CotaID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Controle e Log

        internal void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cApresentacao (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO xApresentacao (ID, Versao, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID) ");
                sql.Append("SELECT ID, @V, EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, LocalImagemMapaID, DescricaoPadrao, Descricao, UltimoCodigoImpresso, CotaID, Quantidade, QuantidadePorCliente, MapaEsquematicoID FROM tApresentacao WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #endregion

        public List<EstruturaManutencaoApresentacaoPeriodo> CarregaListaApresentacaoPeriodo(int regionalID, int empresaID, int localID, int eventoID, string disponivelRelatorio, string disponivelVenda, string disponivelAjuste, string periodoInicial, string periodoFinal)
        {
            try
            {
                List<EstruturaManutencaoApresentacaoPeriodo> retorno = new List<EstruturaManutencaoApresentacaoPeriodo>();

                string strSQL = string.Empty;

                string regional = "";
                if (regionalID != 0)
                    regional = "r.ID = " + regionalID;

                string empresa = "";
                if (empresaID != 0)
                    empresa = "and em.ID = " + empresaID;

                string local = "";
                if (localID != 0)
                    local = "and l.ID = " + localID;

                string evento = "";
                if (eventoID != 0)
                    evento = "and e.ID = " + eventoID;

                string relatorio = "";
                switch (disponivelRelatorio)
                {
                    case "Ambos":
                        relatorio = "";
                        break;
                    case "Não":
                        relatorio = "and DisponivelRelatorio = 'F'";
                        break;
                    case "Sim":
                        relatorio = "and DisponivelRelatorio = 'T'";
                        break;
                }

                string Ajuste = "";
                switch (disponivelAjuste)
                {
                    case "Ambos":
                        break;
                    case "Não":
                        Ajuste = "and DisponivelAjuste = 'F'";
                        break;
                    case "Sim":
                        Ajuste = "and DisponivelAjuste = 'T'";
                        break;
                }

                string Venda = "";
                switch (disponivelVenda)
                {
                    case "Ambos":
                        break;
                    case "Não":
                        Venda = "and DisponivelVenda = 'F'";
                        break;
                    case "Sim":
                        Venda = "and DisponivelVenda = 'T'";
                        break;
                }

                string horario = "AND (a.Horario > '" + periodoInicial + "' AND a.Horario < '" + periodoFinal + "')";

                strSQL = "" + "Select a.ID, l.Nome as Local , e.Nome as Evento , a.Horario , a.DisponivelAjuste, " +
                    "a.DisponivelRelatorio , DisponivelVenda from tApresentacao (NOLOCK) as a" +
                    " inner join tEvento (NOLOCK) as e on e.ID = a.EventoID" +
                    " inner join tLocal (NOLOCK) as l on l.ID = e.LocalID" +
                    " inner join tEmpresa (NOLOCK) as em on em.ID = l.EmpresaID" +
                    " inner join tRegional (NOLOCK) as r on r.ID = em.RegionalID" +
                    " where" +
                    " " + regional +
                    " " + empresa +
                    " " + local +
                    " " + evento +
                    " " + relatorio +
                    " " + Ajuste +
                    " " + Venda +
                    " " + horario +
                    " ORDER BY ID";

                bd.Consulta(strSQL);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaManutencaoApresentacaoPeriodo()
                    {
                        Id = bd.LerInt("ID"),
                        Local = bd.LerString("Local"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = bd.LerDateTime("Horario"),
                        DisponivelAjuste = bd.LerBoolean("DisponivelAjuste"),
                        DisponivelRelatorio = bd.LerBoolean("DisponivelRelatorio"),
                        DisponivelVenda = bd.LerBoolean("DisponivelVenda"),
                    });
                }

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

        public bool AtualizarApresentacao()
        {
            try
            {
                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacao WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacao SET DisponivelVenda = '@001', DisponivelAjuste = '@002', DisponivelRelatorio = '@003'");
                sql.Append(" WHERE ID = @ID ");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.DisponivelVenda.ValorBD);
                sql.Replace("@002", this.DisponivelAjuste.ValorBD);
                sql.Replace("@003", this.DisponivelRelatorio.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;
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

        public int DesabilitaAutomatico(TipoDesabilitaAutomatico tipo)
        {
            BD bdAutomatico = null;
            DateTime dataHora = DateTime.Now;

            // Variável para armazenar as apresentações a serem atualizadas
            List<ClientObjects.EstruturaApresentacao> lstApresentacoes = new List<IRLib.Paralela.ClientObjects.EstruturaApresentacao>();

            string strSQL = string.Empty;

            try
            {

                if (this.Control.UsuarioID == 0)
                    throw new Exception("É necessário informar o usuário que está realizando a transação.");

                bdAutomatico = new BD();

                //bd.IniciarTransacao();

                // Define a Consulta que será executada para capturar a massa de apresentações
                switch (tipo)
                {
                    case TipoDesabilitaAutomatico.Apresentacoes:
                        strSQL = "" +
                            "    SELECT " +
                            "        tApresentacao.* " +
                            "    FROM " +
                            "        tApresentacao (NOLOCK) " +
                            "    INNER JOIN " +
                            "        tEvento (NOLOCK) " +
                            "    ON " +
                            "        tEvento.ID = tApresentacao.EventoID " +
                            "    WHERE " +
                            "        tEvento.DesabilitaAutomatico = 'T' " +
                            "    AND " +
                            "        tApresentacao.Horario < '" + dataHora.AddHours(-4).ToString("yyyyMMddHHmm") + "00' " +
                            "    AND " +
                            "        tApresentacao.DisponivelVenda = 'T' " +
                            "    ORDER BY  " +
                            "        tApresentacao.Horario ";
                        break;
                    case TipoDesabilitaAutomatico.Relatorios:
                        strSQL = "" +
                            "    SELECT " +
                            "        tApresentacao.* " +
                            "    FROM " +
                            "        tApresentacao (NOLOCK) " +
                            "    INNER JOIN " +
                            "        tEvento (NOLOCK) " +
                            "    ON " +
                            "        tEvento.ID = tApresentacao.EventoID " +
                            "    WHERE " +
                            "        tEvento.DesabilitaAutomatico = 'T' " +
                            "    AND " +
                            "        tApresentacao.Horario < '" + dataHora.AddMonths(-12).ToString("yyyyMMddHHmm") + "00' " +
                            "    AND " +
                            "        tApresentacao.DisponivelRelatorio = 'T' " +
                            "    ORDER BY  " +
                            "        tApresentacao.Horario ";
                        break;
                    case TipoDesabilitaAutomatico.Ajuste:
                        strSQL = "" +
                            "    SELECT " +
                            "        tApresentacao.* " +
                            "    FROM " +
                            "        tApresentacao (NOLOCK) " +
                            "    INNER JOIN " +
                            "        tEvento (NOLOCK) " +
                            "    ON " +
                            "        tEvento.ID = tApresentacao.EventoID " +
                            "    WHERE " +
                            "        tEvento.DesabilitaAutomatico = 'T' " +
                            "    AND " +
                            "        tApresentacao.Horario < '" + dataHora.AddHours(-48).ToString("yyyyMMddHHmm") + "00' " +
                            "    AND " +
                            "        tApresentacao.DisponivelAjuste = 'T' " +
                            "    ORDER BY  " +
                            "        tApresentacao.Horario ";
                        break;
                }


                // Captura as apresentações a serem atualizadas
                using (IDataReader oDataReader = bdAutomatico.Consulta(strSQL))
                {
                    ClientObjects.EstruturaApresentacao apresentacao;
                    while (oDataReader.Read())
                    {
                        apresentacao = new IRLib.Paralela.ClientObjects.EstruturaApresentacao();

                        apresentacao.ID = bdAutomatico.LerInt("ID");
                        apresentacao.EventoID = bdAutomatico.LerInt("EventoID");
                        apresentacao.Horario = bdAutomatico.LerDateTime("Horario");
                        // Dependendo do Tipo de Consulta Determinados Campos serão gravados como False
                        switch (tipo)
                        {
                            case TipoDesabilitaAutomatico.Apresentacoes:
                                apresentacao.DisponivelVenda = false;
                                apresentacao.DisponivelRelatorio = bdAutomatico.LerBoolean("DisponivelRelatorio");
                                apresentacao.DisponivelAjuste = bdAutomatico.LerBoolean("DisponivelAjuste");
                                break;
                            case TipoDesabilitaAutomatico.Relatorios:
                                apresentacao.DisponivelVenda = bdAutomatico.LerBoolean("DisponivelVenda");
                                apresentacao.DisponivelAjuste = bdAutomatico.LerBoolean("DisponivelAjuste");
                                apresentacao.DisponivelRelatorio = false;
                                break;
                            case TipoDesabilitaAutomatico.Ajuste:
                                apresentacao.DisponivelVenda = bdAutomatico.LerBoolean("DisponivelVenda");
                                apresentacao.DisponivelRelatorio = bdAutomatico.LerBoolean("DisponivelRelatorio");
                                apresentacao.DisponivelAjuste = false;
                                break;
                        }
                        apresentacao.VersaoImagemIngresso = bdAutomatico.LerInt("VersaoImagemIngresso");
                        apresentacao.VersaoImagemVale = bdAutomatico.LerInt("VersaoImagemVale");
                        apresentacao.VersaoImagemVale2 = bdAutomatico.LerInt("VersaoImagemVale2");
                        apresentacao.VersaoImagemVale3 = bdAutomatico.LerInt("VersaoImagemVale3");
                        apresentacao.Impressao = bdAutomatico.LerString("Impressao");
                        apresentacao.LocalImagemMapaID = bdAutomatico.LerInt("LocalImagemMapaID");
                        apresentacao.DescricaoPadrao = bdAutomatico.LerBoolean("DescricaoPadrao");
                        apresentacao.Descricao = bdAutomatico.LerString("Descricao");
                        apresentacao.UltimoCodigoImpresso = bdAutomatico.LerInt("UltimoCodigoImpresso");
                        apresentacao.CotaID = bdAutomatico.LerInt("CotaID");
                        apresentacao.MapaEsquematicoID = bdAutomatico.LerInt("MapaEsquematicoID");
                        apresentacao.Quantidade = bdAutomatico.LerInt("Quantidade");
                        apresentacao.QuantidadePorCliente = bdAutomatico.LerInt("QuantidadePorCliente");
                        lstApresentacoes.Add(apresentacao);
                    }
                }

                // Fecha a conexão
                bdAutomatico.Fechar();

                // Atualiza as apresentações capturadas
                foreach (ClientObjects.EstruturaApresentacao item in lstApresentacoes)
                {
                    this.Control.ID = item.ID;
                    this.EventoID.Valor = item.EventoID;
                    this.Horario.Valor = item.Horario;
                    this.DisponivelVenda.Valor = item.DisponivelVenda;
                    this.DisponivelRelatorio.Valor = item.DisponivelRelatorio;
                    this.DisponivelAjuste.Valor = item.DisponivelAjuste;
                    this.VersaoImagemIngresso.Valor = item.VersaoImagemIngresso;
                    this.VersaoImagemVale.Valor = item.VersaoImagemVale;
                    this.VersaoImagemVale2.Valor = item.VersaoImagemVale2;
                    this.VersaoImagemVale3.Valor = item.VersaoImagemVale3;
                    this.Impressao.Valor = item.Impressao;
                    this.DescricaoPadrao.Valor = item.DescricaoPadrao;
                    this.Descricao.Valor = item.Descricao;
                    this.CotaID.Valor = item.CotaID;
                    this.MapaEsquematicoID.Valor = item.MapaEsquematicoID;
                    this.Quantidade.Valor = item.Quantidade;
                    this.QuantidadePorCliente.Valor = item.QuantidadePorCliente;
                    if (!this.Atualizar(bdAutomatico))
                        throw new ApresentacaoException("Não foi possível atualizar a Apresentação");
                }

                // Finaliza a transação
                //bd.FinalizarTransacao();

                // Fecha a conexão
                bdAutomatico.Fechar();

            }
            finally
            {
                bd.Fechar();

                if (bdAutomatico != null)
                {
                    bdAutomatico.Fechar();
                    bdAutomatico = null;
                }
            }

            return lstApresentacoes.Count;
        }

        public void SalvarListagem(List<IRLib.Paralela.ClientObjects.ApresentacaoManutencaoListagem> listagem)
        {
            bd.IniciarTransacao();
            StringBuilder sql = new StringBuilder();
            string disponivelVenda = "";
            string disponivelAjuste = "";
            string disponivelRelatorio = "";
            try
            {
                for (int i = 0; i < listagem.Count; i++)
                {
                    disponivelVenda = (listagem[i].DisponivelVenda) ? "T" : "F";
                    disponivelAjuste = (listagem[i].DisponivelAjuste) ? "T" : "F";
                    disponivelRelatorio = (listagem[i].DisponivelRelatorio) ? "T" : "F";

                    sql.Append(" UPDATE tApresentacao SET DisponivelAjuste = '" + disponivelAjuste + "',");
                    sql.Append(" DisponivelRelatorio = '" + disponivelRelatorio + "', DisponivelVenda = '" + disponivelVenda + "' ");
                    sql.Append(" WHERE ID = " + listagem[i].ID + " ");

                }
                bd.Executar(sql.ToString());
                bd.FinalizarTransacao();
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<Preco> PrecosApresentacao()
        {
            return PrecosApresentacao(this.Control.ID);
        }

        public bool PossuiClassificacao(int apresentacaoSetorID)
        {
            bool verifica = false;
            string sqlVerifica = "SELECT " +
                               "Grupo,LugarID " +
                               "FROM tIngresso (NOLOCK)  " +
                               "WHERE  " +
                               "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                               "Grupo > 0 AND  " +
                               "Classificacao > 0  " +
                               "ORDER BY Grupo, Classificacao";

            bd.Consulta(sqlVerifica);

            while (bd.Consulta().Read())
            {
                verifica = true;
            }

            bd.FecharConsulta();



            return verifica;


        }

        public List<Preco> PrecosApresentacao(int apresentacaoID)
        {
            List<Preco> listaPrecos = new List<Preco>();
            Preco precoAdicionar;
            BD bd = new BD();
            bd.Consulta(@"SELECT DISTINCT p.ID, p.Nome FROM tApresentacaoSetor aps
                        INNER JOIN tPreco p ON p.ApresentacaoSetorID = aps.ID
                        WHERE aps.ApresentacaoID = " + apresentacaoID);

            while (bd.Consulta().Read())
            {
                bool achou = false;
                //esse foreach verifica se existe um preco com o mesmo nome
                //pois como o ID também é trazido do banco, o distinct fica inútil

                foreach (Preco preco in listaPrecos)
                {
                    if (preco.Nome.Valor == bd.LerString("Nome"))
                    {
                        achou = true;
                        break;
                    }
                }
                if (!achou)
                {
                    precoAdicionar = new Preco();
                    precoAdicionar.Control.ID = bd.LerInt("ID");
                    precoAdicionar.Nome.Valor = bd.LerString("Nome");
                    listaPrecos.Add(precoAdicionar);
                }
            }
            return listaPrecos;

        }

        public List<ClientObjects.EstruturaIDNome> PrecosApresentacao(List<int> apresentacaoID)
        {
            List<ClientObjects.EstruturaIDNome> listaPrecos = new List<ClientObjects.EstruturaIDNome>();
            ClientObjects.EstruturaIDNome precoAdicionar;
            BD bd = new BD();

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   p.ID, " +
                    "   p.Nome " +
                    "FROM tApresentacaoSetor (NOLOCK) aps " +
                    "INNER JOIN tPreco (NOLOCK) p ON p.ApresentacaoSetorID = aps.ID " +
                    "WHERE " +
                    "  aps.ApresentacaoID IN (" + Utilitario.ArrayToString(apresentacaoID.ToArray()) + ") " +
                    "ORDER BY p.Nome,p.ID"))
                {
                    while (bd.Consulta().Read())
                    {
                        bool achou = false;
                        //esse foreach verifica se existe um preco com o mesmo nome
                        //pois como o ID também é trazido do banco, o distinct fica inútil

                        foreach (ClientObjects.EstruturaIDNome preco in listaPrecos)
                        {
                            if (preco.Nome == bd.LerString("Nome"))
                            {
                                achou = true;
                                break;
                            }
                        }
                        if (!achou)
                        {
                            precoAdicionar = new ClientObjects.EstruturaIDNome();
                            precoAdicionar.ID = bd.LerInt("ID");
                            precoAdicionar.Nome = bd.LerString("Nome");
                            listaPrecos.Add(precoAdicionar);
                        }
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

            return listaPrecos;

        }

        public string DesabilitaAutomatico()
        {
            StringBuilder retorno = new StringBuilder();
            try
            {

                List<Apresentacao> lista = new List<Apresentacao>();
                DateTime dataHora = new DateTime();
                dataHora = DateTime.Now;
                dataHora = dataHora.AddHours(5);

                string sql = "SELECT * FROM tApresentacao (NOLOCK) t1 " +
                                "INNER JOIN tEvento(NOLOCK) t2 ON t1.EventoID=t2.ID " +
                                "WHERE " +
                                "t2.DesabilitaAutomatico='T' AND t1.Horario <= '" + dataHora.ToString("yyyyMMddHHmmss") +
                                "' AND DisponivelVenda='T'";
                BD bd = new BD();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    Apresentacao apresentacao = new Apresentacao(-1);
                    apresentacao.Ler(bd.LerInt("ID"));
                    retorno.Append("\r\n" + bd.LerString("Nome"));
                    apresentacao.DisponivelVenda.Valor = false;
                    lista.Add(apresentacao);

                }
                bd.Fechar();

                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Atualizar();
                }
                return retorno.ToString();
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

        public DataTable TiposImpressao()
        {

            try
            {

                DataTable tabela = new DataTable("TipoImpressao");

                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));

                DataRow linha;

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_APRESENTACAO;
                linha["Nome"] = "Data da Apresentação + Horário";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_SOMENTE_DATA_APRESENTACAO;
                linha["Nome"] = "Somente Data da Apresentação";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_VENDA;
                linha["Nome"] = "Data da Venda";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_NADA;
                linha["Nome"] = "Nada";
                tabela.Rows.Add(linha);

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Apresentacao(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.UsuarioIDLogado = usuarioIDLogado;
        }

        public bool Ajuste()
        {

            try
            {

                string sql = "SELECT 1 FROM tApresentacao (NOLOCK) " +
                    "WHERE ID=" + this.Control.ID + " AND DisponivelAjuste='T'";

                object ret = bd.ConsultaValor(sql);

                bd.Fechar();

                return (ret != null);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Relatorio()
        {

            try
            {

                string sql = "SELECT 1 FROM tApresentacao (NOLOCK) " +
                    "WHERE ID=" + this.Control.ID + " AND DisponivelRelatorio='T'";

                object ret = bd.ConsultaValor(sql);

                bd.Fechar();

                return (ret != null);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Venda()
        {

            try
            {

                string sql = "SELECT DisponivelVenda FROM tApresentacao (NOLOCK) " +
                    "WHERE ID=" + this.Control.ID;

                object ret = bd.ConsultaValor(sql);

                bd.Fechar();

                return (Convert.ToString(ret).Equals("T"));

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

        public override bool Excluir()
        {

            try
            {

                bool ok = true;

                ApresentacaoSetorLista apresentacaoSetorLista = new ApresentacaoSetorLista(UsuarioIDLogado);
                apresentacaoSetorLista.FiltroSQL = "ApresentacaoID=" + this.Control.ID;
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
                    ok = base.Excluir(this.Control.ID);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public enum CamposInfoAddApresentacao
        {
            EventoID,
            EmpresaID,
            LocalID
        }

        public enum CamposCanaisPreco
        {
            PrecoID,
            CanalID,
            DataInicio,
            DataFim,
            Quantidade
        }

        private DataTable CarregaInfoEvento(DataTable apresentacao)
        {

            if (apresentacao.Rows.Count == 0)
                throw new ApresentacaoException("Não existem apresentações a serem criadas.");

            // Converte para o IN
            string eventos = Utilitario.DataRowsToString(apresentacao.Rows, "EventoID");

            //Busca as infos
            string sql = @"	SELECT tEvento.ID as EventoID, tLocal.ID as LocalID, tLocal.EmpresaID as EmpresaID 
							FROM tEvento (NOLOCK), tLocal (NOLOCK)
							WHERE 
							tEvento.LocalID = tLocal.ID AND 
							tEvento.ID IN (" + eventos + @")
							ORDER BY tEvento.ID";

            bd.Consulta(sql);

            DataTable tabela = new DataTable("InfoEventos");
            tabela.Columns.Add(CamposInfoAddApresentacao.EventoID.ToString(), typeof(int));
            tabela.Columns.Add(CamposInfoAddApresentacao.LocalID.ToString(), typeof(int));
            tabela.Columns.Add(CamposInfoAddApresentacao.EmpresaID.ToString(), typeof(int));
            DataRow linha = null;
            while (bd.Consulta().Read())
            {
                linha = tabela.NewRow();
                linha[CamposInfoAddApresentacao.EventoID.ToString()] = bd.LerInt(CamposInfoAddApresentacao.EventoID.ToString());
                linha[CamposInfoAddApresentacao.LocalID.ToString()] = bd.LerInt(CamposInfoAddApresentacao.LocalID.ToString());
                linha[CamposInfoAddApresentacao.EmpresaID.ToString()] = bd.LerInt(CamposInfoAddApresentacao.EmpresaID.ToString());
                tabela.Rows.Add(linha);
            }
            bd.Consulta().Close();
            return tabela;
        }

        private DataTable CarregaCanaisPreco(DataTable precos)
        {

            string precosID = Utilitario.DataRowsToString(precos.Rows, "ID");

            string sql = "SELECT PrecoID, CanalID, DataInicio, DataFim, Quantidade FROM tCanalPreco (NOLOCK) WHERE PrecoID IN (" + precosID + ") ORDER BY PrecoID, CanalID";

            bd.Consulta(sql);

            DataTable tabela = new DataTable("CanaisPreco");
            tabela.Columns.Add(CamposCanaisPreco.PrecoID.ToString(), typeof(int));
            tabela.Columns.Add(CamposCanaisPreco.CanalID.ToString(), typeof(int));
            tabela.Columns.Add(CamposCanaisPreco.DataInicio.ToString(), typeof(DateTime));
            tabela.Columns.Add(CamposCanaisPreco.DataFim.ToString(), typeof(DateTime));
            tabela.Columns.Add(CamposCanaisPreco.Quantidade.ToString(), typeof(int));
            DataRow linha = null;
            while (bd.Consulta().Read())
            {
                linha = tabela.NewRow();
                linha[CamposCanaisPreco.PrecoID.ToString()] = bd.LerInt(CamposCanaisPreco.PrecoID.ToString());
                linha[CamposCanaisPreco.CanalID.ToString()] = bd.LerInt(CamposCanaisPreco.CanalID.ToString());
                linha[CamposCanaisPreco.DataInicio.ToString()] = bd.LerDateTime(CamposCanaisPreco.DataInicio.ToString());
                linha[CamposCanaisPreco.DataFim.ToString()] = bd.LerDateTime(CamposCanaisPreco.DataFim.ToString());
                linha[CamposCanaisPreco.Quantidade.ToString()] = bd.LerInt(CamposCanaisPreco.Quantidade.ToString());
                tabela.Rows.Add(linha);
            }
            bd.Consulta().Close();
            return tabela;
        }

        public override int[] Novo(DataSet info)
        {
            // TODO: Encontrar melhor solução.
            CTLib.BD lugares = new BD(); // Conexão específica para a busca de lugares. Não há abertura de múltiplas conexões.
            try
            {

                bd.IniciarTransacao();

                int qtdeApresentacoes = info.Tables["Apresentacao"].Rows.Count;

                int[] retorno = new int[qtdeApresentacoes]; //id das apresentacoes

                Ingresso ingresso = new Ingresso();

                DataRow linhaApresentacao = null;
                Apresentacao apresentacao = new Apresentacao(UsuarioIDLogado);

                DataRow[] linhasApresentacaoSetor = null;
                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(UsuarioIDLogado);

                DataRow[] linhasPreco = null;
                Preco preco = new Preco(UsuarioIDLogado);
                CanalPrecoLista canalPrecoLista = new CanalPrecoLista(UsuarioIDLogado);
                CanalPreco canalPreco = new CanalPreco(UsuarioIDLogado);
                CodigoBarra oCodigoBarra = new CodigoBarra(UsuarioIDLogado);

                Setor setor = new Setor(UsuarioIDLogado);

                DataRow[] detalheEvento = null;
                DataRow[] canaisPreco = null;
                int localID, empresaID;

                DataTable infoEvento = this.CarregaInfoEvento(info.Tables["Apresentacao"]);
                info.Tables.Add(this.CarregaCanaisPreco(info.Tables["Preco"]));

                //#MapaLugarMarcado
                //DTT responsavel por preencher os Precos que sao Principais, existem multiplos.
                DataTable dttPrecosPrincipais = new DataTable("PrecosPrincipais");
                dttPrecosPrincipais.Columns.Add("ID", typeof(int));
                dttPrecosPrincipais.Columns.Add("ApresentacaoSetorID", typeof(int));
                dttPrecosPrincipais.Columns.Add("Prioridade", typeof(bool));

                List<string> CodigosBarra = new List<string>();

                bool VenderPos = Convert.ToBoolean(info.Tables["Evento"].Rows[0]["VenderPos"]);

                double porcentagem = Convert.ToDouble(ConfigurationManager.AppSettings["PorcentagemListaBraca"]);

                //Buscar os códigos de barra caso o evento utilize codigo com lista branca
                bool usarListaBranca = Convert.ToChar(info.Tables["Evento"].Rows[0]["TipoCodigoBarra"]) == (char)Enumerators.TipoCodigoBarra.ListaBranca;
                if (usarListaBranca || VenderPos)
                {
                    int quantidade = Convert.ToInt32(info.Tables["ApresentacaoSetor"].Compute("SUM(SetorQtde)", "1=1"));

                    quantidade += (int)Math.Round(quantidade * (porcentagem / 100));
                    CodigosBarra = oCodigoBarra.BuscarListaBranca(bd, quantidade);
                }


                for (int a = 0; a < qtdeApresentacoes; a++)
                {
                    dttPrecosPrincipais.Clear();

                    linhaApresentacao = info.Tables["Apresentacao"].Rows[a];

                    apresentacao.EventoID.Valor = (int)linhaApresentacao["EventoID"];
                    apresentacao.Horario.Valor = Convert.ToDateTime(linhaApresentacao[Apresentacao.COL_HORARIO_STRING]);
                    apresentacao.DisponivelVenda.Valor = (bool)linhaApresentacao["DisponivelVenda"];
                    apresentacao.DisponivelAjuste.Valor = (bool)linhaApresentacao["DisponivelAjuste"];
                    apresentacao.DisponivelRelatorio.Valor = (bool)linhaApresentacao["DisponivelRelatorio"];
                    apresentacao.Impressao.Valor = (string)linhaApresentacao["Impressao"];
                    apresentacao.MapaEsquematicoID.Valor = (int)linhaApresentacao["MapaEsquematicoID"];
                    if (!apresentacao.Inserir(bd))
                        throw new ApresentacaoException("Falha ao adicionar a apresentacao " + apresentacao.Horario.Valor.ToString("dd/MM/YY hh:mm"));

                    retorno[a] = apresentacao.Control.ID;
                    //Processando();

                    detalheEvento = infoEvento.Select(CamposInfoAddApresentacao.EventoID + " = " + linhaApresentacao["EventoID"]);
                    if (detalheEvento.Length == 0)
                        throw new ApresentacaoException("Os detalhes do Evento não foram encontrados!");

                    localID = (int)detalheEvento[0][CamposInfoAddApresentacao.LocalID.ToString()];
                    empresaID = (int)detalheEvento[0][CamposInfoAddApresentacao.EmpresaID.ToString()];

                    //incluir todos os setores dessa apresentacao

                    linhasApresentacaoSetor = info.Tables["ApresentacaoSetor"].Select("ApresentacaoID=" + (a + 1));

                    for (int i = 0; i < linhasApresentacaoSetor.Length; i++)
                    {
                        // Insere apresentacaoSetor
                        apresentacaoSetor.SetorID.Valor = (int)linhasApresentacaoSetor[i]["SetorID"];
                        apresentacaoSetor.ApresentacaoID.Valor = apresentacao.Control.ID;

                        apresentacaoSetor.Inserir(bd);

                        //incluir todos os preços desse setor
                        int aS = (int)linhasApresentacaoSetor[i]["ID"];
                        linhasPreco = info.Tables["Preco"].Select("ApresentacaoSetorID=" + aS);

                        // Informações de preços préviamente selecionadas.
                        for (int p = 0; p < linhasPreco.Length; p++)
                        {
                            // Insere novo preço.
                            preco.ApresentacaoSetorID.Valor = apresentacaoSetor.Control.ID;
                            preco.Valor.Valor = (decimal)linhasPreco[p]["Valor"];
                            preco.Nome.Valor = (string)linhasPreco[p]["Nome"];
                            preco.CorID.Valor = (int)linhasPreco[p]["CorID"];
                            preco.Impressao.Valor = (string)linhasPreco[p]["Impressao"];
                            preco.Quantidade.Valor = (int)linhasPreco[p]["Quantidade"];
                            preco.QuantidadePorCliente.Valor = (int)linhasPreco[p]["QuantidadePorCliente"];

                            preco.Inserir(apresentacao.EventoID.Valor, apresentacaoSetor.SetorID.Valor, apresentacao.Control.ID, !usarListaBranca, bd);

                            //#MapaLugarMarcado
                            //Adiciona os precos principais no DataTable auxiliar
                            //Estes precos sao utilizando na selecao de lugar marcado do Site
                            if (Convert.ToBoolean(linhasPreco[p]["Principal"]))
                            {
                                DataRow dtr = dttPrecosPrincipais.NewRow();
                                dtr["ID"] = preco.Control.ID;
                                dtr["ApresentacaoSetorID"] = apresentacaoSetor.Control.ID;
                                if (string.Compare(linhasPreco[p]["Nome"].ToString().ToLower(), "inteira") == 0)
                                    dtr["Prioridade"] = true;
                                dttPrecosPrincipais.Rows.Add(dtr);
                            }

                            lugares.FecharConsulta();
                            //add Canal X Preço ********************************************************
                            //descobrir quais canais esse preco esta distribuido
                            int precoID = (int)linhasPreco[p]["ID"];

                            canaisPreco = info.Tables["CanaisPreco"].Select(CamposCanaisPreco.PrecoID + " = " + precoID);

                            foreach (DataRow linhaPreco in canaisPreco)
                            {
                                canalPreco.DataInicio.Limpar();
                                canalPreco.DataFim.Limpar();

                                // Insere os preços para os canais.
                                canalPreco.CanalID.Valor = (int)linhaPreco[CamposCanaisPreco.CanalID.ToString()];
                                canalPreco.PrecoID.Valor = preco.Control.ID;
                                canalPreco.Quantidade.Valor = (int)linhaPreco[CamposCanaisPreco.Quantidade.ToString()]; ;

                                if ((DateTime)linhaPreco[CamposCanaisPreco.DataInicio.ToString()] != DateTime.MinValue)
                                    canalPreco.DataInicio.Valor = (DateTime)linhaPreco[CamposCanaisPreco.DataInicio.ToString()];

                                if ((DateTime)linhaPreco[CamposCanaisPreco.DataFim.ToString()] != DateTime.MinValue)
                                    canalPreco.DataFim.Valor = (DateTime)linhaPreco[CamposCanaisPreco.DataFim.ToString()];

                                canalPreco.Inserir(bd, false);
                            }
                        }//for linhasPreco.Rows.Count

                        List<string> codigosApresentacaoSetor = new List<string>();

                        double quantidadeAPS = linhasApresentacaoSetor[i]["SetorQtde"] == null || linhasApresentacaoSetor[i]["SetorQtde"] == DBNull.Value ? -1 : (int)linhasApresentacaoSetor[i]["SetorQtde"];
                        if ((usarListaBranca || VenderPos) && quantidadeAPS > 0)
                        {
                            //Busca os X códigos que serão associados aos INGRESSOS!
                            codigosApresentacaoSetor = CodigosBarra.Take((int)quantidadeAPS).ToList();
                            CodigosBarra.RemoveAll(c => codigosApresentacaoSetor.Contains(c));

                            //Cria os X% de ingressos de contingencia
                            int quantidadeContigencia = (int)Math.Round((quantidadeAPS * (porcentagem / 100d)));
                            List<string> codigosContingencia = CodigosBarra.Take(quantidadeContigencia).ToList();
                            CodigosBarra.RemoveAll(c => codigosContingencia.Contains(c));
                            new CodigoBarraEvento().GerarCodigos(apresentacaoSetor.Control.ID, codigosContingencia, DateTime.Now, apresentacao.EventoID.Valor, bd);
                        }

                        //GERAR INGRESSOS ********************************************************************
                        lugares = setor.Lugares(apresentacaoSetor.SetorID.Valor);
                        int sequencia = ingresso.UltimoCodigoSequencial(apresentacao.Control.ID) + 1;
                        List<string> codigosLugar = new List<string>();

                        while (lugares.Consulta().Read())
                        {
                            // Verifica se é lugar marcado.
                            if (lugares.LerString("LugarMarcado") != Setor.Pista)
                            {
                                int qtde = lugares.LerInt("Quantidade");
                                codigosLugar = new List<string>();

                                if (usarListaBranca)
                                {
                                    codigosLugar = codigosApresentacaoSetor.Take(qtde).ToList();
                                    codigosApresentacaoSetor.RemoveAll(c => codigosLugar.Contains(c));
                                }

                                ingresso.NovoMarcado(apresentacaoSetor.Control.ID, apresentacao.EventoID.Valor, apresentacao.Control.ID, apresentacaoSetor.SetorID.Valor, empresaID, localID, lugares.LerInt("BloqueioID"), lugares.LerInt("ID"), qtde, lugares.LerInt("QuantidadeBloqueada"), lugares.LerString("Codigo"), lugares.LerInt("Grupo"), lugares.LerInt("Classificacao"), lugares.LerString("LugarMarcado"), bd, sequencia, codigosLugar.ToArray());
                                sequencia += qtde;
                            }
                            else
                            {
                                codigosLugar = new List<string>();
                                if (usarListaBranca)
                                {
                                    codigosLugar = codigosApresentacaoSetor.Take((int)quantidadeAPS).ToList();
                                    codigosApresentacaoSetor.RemoveAll(c => codigosLugar.Contains(c));
                                }
                                ingresso.Acrescentar(apresentacaoSetor.Control.ID, apresentacao.EventoID.Valor, apresentacao.Control.ID, apresentacaoSetor.SetorID.Valor, empresaID, localID, 0, (int)quantidadeAPS, 1, bd, true, codigosLugar); // Inicia com código 1
                            }
                        }

                        // Atualiza o campo informando que os ingressos foram gerados
                        apresentacaoSetor.AtualizarIngressosGerados(bd);

                        lugares.Consulta().Close(); // Fecha a consulta, mas conexão continua aberta.
                        //FIM DE GERAR INGRESSOS *************************************************************

                        //#MapaLugarMarcado
                        //Encontrou os precos principais, podem ser multiplos já que são populados por Precos já existentes
                        //Ou seja, APS X tem Preco Principal como Inteira e APS Y tem o Meia como principal
                        if (dttPrecosPrincipais.Rows.Count > 0)
                        {
                            DataRow[] prioridades = dttPrecosPrincipais.Select("Prioridade = " + true);
                            if (prioridades.Length > 0)
                                apresentacaoSetor.AtualizarPrecoPrincipal(bd, Convert.ToInt32(prioridades[0]["ID"]));
                            else
                                apresentacaoSetor.AtualizarPrecoPrincipal(bd, Convert.ToInt32(dttPrecosPrincipais.Rows[0]["ID"]));

                            dttPrecosPrincipais.Clear();
                        }
                    }//for linhasApresentacaoSetor

                }//for info.Tables["Apresentacao"].Rows


                bd.FinalizarTransacao();
                return retorno;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
                bd.Cnn.Dispose();
                lugares.Fechar();
                lugares.Cnn.Dispose();
                info.Dispose();
            }

        }

        public override DataTable PorcentagemIngressosStatus(string apresentacoes)
        {

            DataTable tabela = Utilitario.EstruturaPorcentagemIngressosStatus();
            try
            {
                DataTable quantidadeIngressosStatus = QuantidadeIngressosStatus(apresentacoes);
                decimal total = TotalIngressos(apresentacoes);
                foreach (DataRow linha in quantidadeIngressosStatus.Rows)
                {
                    DataRow linhaPorcentagem = tabela.NewRow();
                    linhaPorcentagem["ApresentacaoSetorID"] = 0;
                    linhaPorcentagem["ApresentacaoID"] = 0;
                    linhaPorcentagem["SetorID"] = 0;
                    linhaPorcentagem["Status"] = linha["Status"];
                    linhaPorcentagem["Quantidade"] = linha["Quantidade"];
                    linhaPorcentagem["CortesiaID"] = linha["CortesiaID"];
                    linhaPorcentagem["Porcentagem"] = (decimal)(Convert.ToDecimal(linha["Quantidade"]) / total) * 100;
                    tabela.Rows.Add(linhaPorcentagem);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataTable QuantidadeIngressosStatus(string apresentacoes)
        {

            DataTable tabela = Utilitario.EstruturaQuantidadeIngressosStatus();
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT      COUNT(tIngresso.ID) AS Quantidade, tApresentacaoSetor.ApresentacaoID, tIngresso.Status, tIngresso.CortesiaID " +
                    "FROM        tIngresso INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "GROUP BY tApresentacaoSetor.ApresentacaoID, tIngresso.Status, tIngresso.CortesiaID " +
                    "HAVING        (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = bd.LerString("Status");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
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

        public override int TotalIngressos(string apresentacoes)
        {
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT       COUNT(tIngresso.ID) AS Quantidade " +
                    "FROM         tIngresso INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "WHERE        (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = -1;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }

        public int QuantidadePorStatus(string status, bool comCortesia, int apresentacaoID)
        {
            string condicaoCortesia = "";
            if (comCortesia)
                condicaoCortesia = " AND (tIngresso.CortesiaID >= 0) ";
            else
                condicaoCortesia = " AND (tIngresso.CortesiaID = 0) ";
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        ApresentacaoID, COUNT(ID) AS Quantidade " +
                    "FROM          tIngresso " +
                    "WHERE        (Status IN (" + status + ")) " + condicaoCortesia +
                    "GROUP BY	   ApresentacaoID " +
                    "HAVING        (ApresentacaoID = " + apresentacaoID + ")";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = 0;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }		// QuantidadePorStatus

        public int QuantidadePorStatus(string status, bool comCortesia)
        {
            string condicaoCortesia = "";
            if (comCortesia)
                condicaoCortesia = " AND (tIngresso.CortesiaID >= 0) ";
            else
                condicaoCortesia = " AND (tIngresso.CortesiaID = 0) ";
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        ApresentacaoID, COUNT(ID) AS Quantidade " +
                    "FROM          tIngresso " +
                    "WHERE        (Status IN (" + status + ")) " + condicaoCortesia +
                    "GROUP BY	   ApresentacaoID " +
                    "HAVING        (ApresentacaoID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = 0;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }		// QuantidadePorStatus

        public decimal FaturamentoPorStatus(string status, bool comCortesia, int apresentacaoID)
        {
            string condicaoCortesia = "";
            if (comCortesia)
                condicaoCortesia = " AND (tIngresso.CortesiaID >= 0) ";
            else
                condicaoCortesia = " AND (tIngresso.CortesiaID = 0) ";
            decimal total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        ApresentacaoID, SUM(tPreco.Valor) AS Faturamento " +
                    "FROM          tIngresso " +
                    "INNER JOIN tPreco ON tIngresso.PrecoID = tPreco.ID " +
                    "WHERE        (Status IN (" + status + ")) " + condicaoCortesia +
                    "GROUP BY  ApresentacaoID " +
                    "HAVING         (ApresentacaoID = " + apresentacaoID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerDecimal("Faturamento");
                }
                else
                {
                    total = 0;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }		// FaturamentoPorStatus

        public override DataTable Setores()
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT s.ID, s.Nome FROM tApresentacaoSetor as tas,tSetor as s " +
                    "WHERE tas.SetorID=s.ID AND tas.ApresentacaoID=" + this.Control.ID + " " +
                    "ORDER BY s.Nome";

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

        public DataTable Setores(int apresentacaoID, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T' " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T' " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T' " : "";

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int));

                string sql = "SELECT s.ID, s.Nome FROM tApresentacaoSetor as tas,tSetor as s, tApresentacao as a " +
                    "WHERE tas.SetorID=s.ID AND tas.ApresentacaoID=" + apresentacaoID + " AND a.ID=tas.ApresentacaoID " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY s.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["ApresentacaoID"] = apresentacaoID;
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

        public DataTable Setores(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT s.ID, s.Nome FROM tApresentacaoSetor as tas,tSetor as s " +
                    "WHERE tas.SetorID=s.ID AND tas.ApresentacaoID=" + this.Control.ID + " " +
                    "ORDER BY s.Nome";

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

        public override int QuantidadeDisponivel(int setorid)
        {

            try
            {

                string sql = "SELECT Count(i.ID) AS Qtde FROM tIngresso as i,tApresentacaoSetor as aps " +
                    "WHERE i.Status='" + Ingresso.DISPONIVEL + "' AND aps.ID=i.ApresentacaoSetorID AND " +
                    "aps.ApresentacaoID=" + this.Control.ID + " AND aps.SetorID=" + setorid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                int qtde = (obj != null) ? Convert.ToInt32(obj) : 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override DataTable VendaProduto(int caixaID)
        {
            DataTable tabela = VendaProdutoEstrutura();
            try
            {
                // Obtendo dados através de SQL
                BD bd = new BD();
                string sql =
                    "SELECT    tProduto.Nome AS Produto, SUM(tComandaItem.Quantidade) AS Quantidade, tComandaItem.PrecoVenda AS Valor, " +
                    "tProdutoCategoria.Nome AS Categoria, tComanda.CaixaID, tCaixa.ApresentacaoID " +
                    "FROM      tProduto INNER JOIN " +
                    "tProdutoCategoria ON tProduto.ProdutoCategoriaID = tProdutoCategoria.ID INNER JOIN " +
                    "tComandaItem ON tProduto.ID = tComandaItem.ProdutoID INNER JOIN " +
                    "tComanda ON tComandaItem.ComandaID = tComanda.ID INNER JOIN " +
                    "tVenda ON tComanda.VendaID = tVenda.ID INNER JOIN " +
                    "tCaixa ON tVenda.CaixaID = tCaixa.ID " +
                    "GROUP BY tProduto.Nome, tProdutoCategoria.Nome, tComanda.CaixaID, tCaixa.ApresentacaoID, tComandaItem.PrecoVenda " +
                    "HAVING      (tComanda.CaixaID = '" + caixaID + "') AND (tCaixa.ApresentacaoID = '" + this.Control.ID + "') " +
                    "ORDER BY tProdutoCategoria.Nome, tProduto.Nome ";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Categoria"] = bd.LerString("Categoria");
                    linha["Produto"] = bd.LerString("Produto");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao retornar DataTable de Venda por Produto (de um caixa)!!");
                tabela = null;
            }
            // retorna DataTable
            return tabela;
        } // fim do mehtodo VendaProduto

        public DataTable VendaProdutoEstrutura()
        {
            DataTable tabela = new DataTable("VendaProduto");
            try
            {
                // Criar DataTable com as colunas
                tabela.Columns.Add("Categoria", typeof(string));
                tabela.Columns.Add("Produto", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
            }
            catch (Exception erro)
            {
                throw erro;
            }
            // retorna DataTable
            return tabela;
        } // fim do mehtodo VendaProduto

        public override DataTable VendaProduto()
        {
            DataTable tabela = VendaProdutoEstrutura();
            try
            {
                // Obtendo dados através de SQL
                BD bd = new BD();
                string sql =
                    "SELECT    tProduto.Nome AS Produto, SUM(tComandaItem.Quantidade) AS Quantidade, tComandaItem.PrecoVenda AS Valor, " +
                    "tProdutoCategoria.Nome AS Categoria, tComanda.CaixaID, tCaixa.ApresentacaoID " +
                    "FROM      tProduto INNER JOIN " +
                    "tProdutoCategoria ON tProduto.ProdutoCategoriaID = tProdutoCategoria.ID INNER JOIN " +
                    "tComandaItem ON tProduto.ID = tComandaItem.ProdutoID INNER JOIN " +
                    "tComanda ON tComandaItem.ComandaID = tComanda.ID INNER JOIN " +
                    "tVenda ON tComanda.VendaID = tVenda.ID INNER JOIN " +
                    "tCaixa ON tVenda.CaixaID = tCaixa.ID " +
                    "GROUP BY tProduto.Nome, tProdutoCategoria.Nome, tComanda.CaixaID, tCaixa.ApresentacaoID, tComandaItem.PrecoVenda " +
                    "HAVING     (tCaixa.ApresentacaoID = '" + this.Control.ID + "')" +
                    "ORDER BY tProdutoCategoria.Nome, tProduto.Nome ";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Categoria"] = bd.LerString("Categoria");
                    linha["Produto"] = bd.LerString("Produto");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao retornar DataTable de Venda por Produto (todos caixas)!!");
                tabela = null;
            }
            // retorna DataTable
            return tabela;
        } // fim do mehtodo VendaProduto

        public override DataTable Caixas()
        {
            // Criando DataTable
            DataTable tabela = new DataTable("");
            try
            {
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("DataAbertura", typeof(string));
                // Executando comando SQL
                BD bd = new BD();
                string sql =
                    "SELECT tCaixa.ID, tCaixa.DataAbertura, tUsuario.Nome, tCaixa.UsuarioID, tCaixa.DataFechamento " +
                    "FROM tCaixa INNER JOIN " +
                    "tUsuario ON tCaixa.UsuarioID = tUsuario.ID " +
                    "WHERE (tCaixa.ApresentacaoID = " + this.Control.ID + ") " +
                    "ORDER BY tUsuario.Nome ";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["DataAbertura"] = bd.LerString("Nome") + " - " + bd.LerStringFormatoDataHora("DataAbertura");
                    // Somente os caixas fechados
                    if (bd.LerStringFormatoDataHora("DataFechamento") != "  /  /       :  ")
                    {
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao retornar DataTable Caixas na classe Apresentacao!!");
                tabela = null;
            }
            return tabela;
        }

        public DataTable EstruturaFechamento()
        {
            DataTable tabela = new DataTable("Fechamento");
            // Criar DataTable com as colunas
            tabela.Columns.Add("SaldoInicial", typeof(decimal));
            tabela.Columns.Add("DataAbertura", typeof(string));
            tabela.Columns.Add("DataFechamento", typeof(string));
            tabela.Columns.Add("FormaPagamento", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            return tabela;
        }

        public override DataTable Fechamento()
        {
            DataTable tabela = EstruturaFechamento();
            try
            {
                // Obtendo dados através de SQL
                BD bd = new BD();
                string sql =
                    "SELECT     SUM(tVendaPagamento.Valor) AS Valor, tFormaPagamento.Nome AS FormaPagamento, tCaixa.DataAbertura, tCaixa.DataFechamento " +
                    "FROM        tVendaPagamento INNER JOIN " +
                    "tVenda ON tVendaPagamento.VendaID = tVenda.ID INNER JOIN " +
                    "tCaixa ON tVenda.CaixaID = tCaixa.ID INNER JOIN " +
                    "tFormaPagamento ON tVendaPagamento.FormaPagamentoID = tFormaPagamento.ID INNER JOIN " +
                    "tApresentacao ON tCaixa.ApresentacaoID = tApresentacao.ID " +
                    "WHERE     (tApresentacao.ID = '" + this.Control.ID + "') " +
                    "GROUP BY tFormaPagamento.Nome, tCaixa.DataAbertura, tCaixa.DataFechamento	";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["DataAbertura"] = bd.LerStringFormatoDataHora("DataAbertura");
                    linha["DataFechamento"] = bd.LerStringFormatoDataHora("DataFechamento");
                    linha["FormaPagamento"] = bd.LerString("FormaPagamento");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    //					linha["SaldoInicial"]= bd.LerDecimal("SaldoInicial");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao retornar DataTable de Fechamento de Caixa por Apresentacao!!");
                tabela = null;
            }
            // retorna DataTable
            return tabela;
        } // fim do mehtodo Fechamento

        public override DataTable Fechamento(int caixaID)
        {
            DataTable tabela = new DataTable("");
            try
            {
                // Criar DataTable com as colunas
                tabela.Columns.Add("SaldoInicial", typeof(decimal));
                tabela.Columns.Add("DataAbertura", typeof(string));
                tabela.Columns.Add("DataFechamento", typeof(string));
                tabela.Columns.Add("FormaPagamento", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                // Obtendo dados através de SQL
                BD bd = new BD();
                string sql =
                    "SELECT     SUM(tVendaPagamento.Valor) AS Valor, tFormaPagamento.Nome AS FormaPagamento, tCaixa.DataAbertura, tCaixa.DataFechamento " +
                    "FROM        tVendaPagamento INNER JOIN " +
                    "tVenda ON tVendaPagamento.VendaID = tVenda.ID INNER JOIN " +
                    "tCaixa ON tVenda.CaixaID = tCaixa.ID INNER JOIN " +
                    "tFormaPagamento ON tVendaPagamento.FormaPagamentoID = tFormaPagamento.ID INNER JOIN " +
                    "tApresentacao ON tCaixa.ApresentacaoID = tApresentacao.ID " +
                    "WHERE     (tApresentacao.ID = '" + this.Control.ID + "') AND (tVenda.CaixaID = '" + caixaID + "') " +
                    "GROUP BY tFormaPagamento.Nome, tCaixa.DataAbertura, tCaixa.DataFechamento	";
                bd.Consulta(sql);
                // Alimentando DataTable
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["DataAbertura"] = bd.LerStringFormatoDataHora("DataAbertura");
                    linha["DataFechamento"] = bd.LerStringFormatoDataHora("DataFechamento");
                    linha["FormaPagamento"] = bd.LerString("FormaPagamento");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    //					linha["SaldoInicial"]= bd.LerDecimal("SaldoInicial");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao retornar DataTable de Fechamento de Caixa por Apresentacao!!");
                tabela = null;
            }
            // retorna DataTable
            return tabela;
        } // fim do mehtodo Fechamento

        public override DataTable Ingressos(int canalID, string status)
        {
            DataTable tabela = Ingresso.EstruturaImpressao();
            try
            {
                // Obtendo dados através de SQL, primeiro o lado dos Ingressos
                BD registrosIngressos = new BD();
                string sql =
                    "SELECT DISTINCT " +
                    "tIngresso.ID, tIngresso.Status, tIngresso.Codigo, tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacaoSetor.ApresentacaoID, tSetor.Nome AS Setor, tSetor.Produto,  " +
                    "tCortesia.Nome AS Cortesia, tPreco.Nome AS Preco, tPreco.Valor, tApresentacao.EventoID, MAX(DISTINCT tIngressoLog.ID) AS IngressoLogID, tUsuario.Login AS Usuario " +
                    "FROM            tCortesia RIGHT OUTER JOIN " +
                    "tUsuario INNER JOIN " +
                    "tSetor INNER JOIN " +
                    "tApresentacaoSetor INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID INNER JOIN " +
                    "tEvento INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID ON tSetor.ID = tApresentacaoSetor.SetorID INNER JOIN " +
                    "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                    "tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID ON tUsuario.ID = tIngresso.UsuarioID ON tCortesia.ID = tIngresso.CortesiaID " +
                    "WHERE    (tApresentacao.ID = '" + this.Control.ID + "') " +
                    "GROUP BY tIngresso.ID, tIngresso.Status, tIngresso.Codigo, tEvento.Nome, tApresentacao.Horario, tApresentacaoSetor.ApresentacaoID, tSetor.Nome, tSetor.Produto, tCortesia.Nome, tPreco.Nome,  " +
                    "tPreco.Valor, tApresentacao.EventoID, tUsuario.Login " +
                    "HAVING        (tIngresso.Status in (" + status + ")) " +
                    "ORDER BY MAX(DISTINCT tIngressoLog.ID), tIngresso.Codigo ";
                registrosIngressos.Consulta(sql);
                // Obtendo IDs do Ingresso
                BD soIDsBD = new BD();
                string soIDs =
                    "SELECT DISTINCT tIngresso.ID, tIngresso.Status, tIngresso.Codigo, tApresentacaoSetor.ApresentacaoID " +
                    "FROM      tApresentacaoSetor INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID " +
                    "GROUP BY tIngresso.ID, tIngresso.Status, tIngresso.Codigo, tApresentacaoSetor.ApresentacaoID " +
                    "HAVING        (tIngresso.Status in (" + status + "))  AND (tApresentacaoSetor.ApresentacaoID = " + this.Control.ID + ") " +
                    "ORDER BY tIngresso.Codigo ";
                soIDsBD.Consulta(soIDs);
                string ingressoIDs = "";
                int temp = 0;
                while (soIDsBD.Consulta().Read())
                {
                    if (temp == 0) // 1a vez
                        ingressoIDs = soIDsBD.LerInt("ID").ToString();
                    else
                        ingressoIDs = ingressoIDs + "," + soIDsBD.LerInt("ID").ToString();
                    temp++;
                }
                if (ingressoIDs != "")
                {
                    // Depois Senha e Cliente
                    BD registrosVenda = new BD();
                    sql =
                        //						"SELECT        tVendaBilheteria.Senha, tCliente.Nome, tCliente.ID AS ClienteID, tIngressoLog.ID AS IngressoLogID, tIngressoLog.IngressoID, tIngressoLog.VendaBilheteriaItemID "+
                        //						"FROM          tVendaBilheteriaItem INNER JOIN "+
                        //						"tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN "+
                        //						"tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN "+
                        //						"tCliente ON tVendaBilheteria.ClienteID = tCliente.ID "+
                        //						"WHERE        (tIngressoLog.IngressoID IN ("+ ingressoIDs +")) "+
                        //						"ORDER BY tIngressoLog.ID ";
                        "SELECT  tVendaBilheteria.Senha, tVendaBilheteria.TaxaConvenienciaValorTotal, tVendaBilheteria.TaxaEntregaValor, tCliente.Nome, tCliente.ID AS ClienteID, tIngressoLog.ID AS IngressoLogID, tIngressoLog.IngressoID, tIngressoLog.VendaBilheteriaItemID, tLoja.CanalID, tLoja.Nome AS Loja " +
                        "FROM    tVendaBilheteriaItem INNER JOIN " +
                        "tIngressoLog ON tVendaBilheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID INNER JOIN " +
                        "tVendaBilheteria ON tVendaBilheteriaItem.VendaBilheteriaID = tVendaBilheteria.ID INNER JOIN " +
                        "tCliente ON tVendaBilheteria.ClienteID = tCliente.ID INNER JOIN " +
                        "tCaixa ON tVendaBilheteria.CaixaID = tCaixa.ID INNER JOIN " +
                        "tLoja ON tCaixa.LojaID = tLoja.ID " +
                        "WHERE   (tIngressoLog.IngressoID IN (" + ingressoIDs + ")) " +
                        "ORDER BY tIngressoLog.ID ";
                    registrosVenda.Consulta(sql);
                    // Alimentando DataTable
                    // Ambos estao ordenados pelo LogID

                    while (registrosIngressos.Consulta().Read() && registrosVenda.Consulta().Read())
                    {
                        if (canalID == 0)
                        { // Todos canais
                            DataRow linha = tabela.NewRow();
                            linha["ID"] = registrosIngressos.LerInt("ID");
                            linha["Usuario"] = registrosIngressos.LerString("Usuario");  //nome do Usuario
                            linha["Evento"] = registrosIngressos.LerString("Evento");  //nome do Evento
                            linha["Horario"] = registrosIngressos.LerDateTime("Horario");  //horario da Apresentacao
                            linha["Setor"] = registrosIngressos.LerString("Setor") + "/" + registrosIngressos.LerString("Preco");  //nome do Setor
                            linha["Preco"] = "";  //nome do preco
                            linha["Produto"] = registrosIngressos.LerBoolean("Produto");  //se o setor eh produto ou nao
                            linha["Cortesia"] = registrosIngressos.LerString("Cortesia");  //nome da Cortesia
                            linha["Valor"] = registrosIngressos.LerDecimal("Valor");  //valor do Ingresso
                            linha["Codigo"] = registrosIngressos.LerString("Codigo");  //codigo do Ingresso
                            // Campos usados para as imagens
                            linha["EventoID"] = registrosIngressos.LerInt("EventoID");  //id do Evento
                            linha["ApresentacaoID"] = registrosIngressos.LerInt("ApresentacaoID");  //id da Apresentacao
                            // Parte da Venda
                            linha["Loja"] = registrosVenda.LerString("Loja");  // loja da venda
                            linha["Senha"] = registrosVenda.LerString("Senha");  //senha da venda
                            linha["Cliente"] = registrosVenda.LerString("Nome"); //cliente que comprou o ingresso
                            linha["ClienteID"] = registrosVenda.LerString("ClienteID"); //preciso do ID para passar no outro objeto 
                            //
                            linha["TaxaConvenienciaValorTotal"] = registrosVenda.LerDecimal("TaxaConvenienciaValorTotal");
                            linha["TaxaEntregaValor"] = registrosVenda.LerDecimal("TaxaEntregaValor");
                            linha["HorarioString"] = registrosIngressos.LerStringFormatoDataHora("Horario");
                            tabela.Rows.Add(linha);
                        }
                        else
                        {
                            if (registrosVenda.LerInt("CanalID") == canalID)
                            {
                                DataRow linha = tabela.NewRow();
                                linha["ID"] = registrosIngressos.LerInt("ID");
                                linha["Usuario"] = registrosIngressos.LerString("Usuario");  //nome do Usuario
                                linha["Evento"] = registrosIngressos.LerString("Evento");  //nome do Evento
                                linha["Horario"] = registrosIngressos.LerDateTime("Horario");  //horario da Apresentacao
                                linha["Setor"] = registrosIngressos.LerString("Setor") + "/" + registrosIngressos.LerString("Preco");  //nome do Setor
                                linha["Preco"] = "";  //nome do preco
                                linha["Produto"] = registrosIngressos.LerBoolean("Produto");  //se o setor eh produto ou nao
                                linha["Cortesia"] = registrosIngressos.LerString("Cortesia");  //nome da Cortesia
                                linha["Valor"] = registrosIngressos.LerDecimal("Valor");  //valor do Ingresso
                                linha["Codigo"] = registrosIngressos.LerString("Codigo");  //codigo do Ingresso
                                // Campos usados para as imagens
                                linha["EventoID"] = registrosIngressos.LerInt("EventoID");  //id do Evento
                                linha["ApresentacaoID"] = registrosIngressos.LerInt("ApresentacaoID");  //id da Apresentacao
                                // Parte da Venda
                                linha["Loja"] = registrosVenda.LerString("Loja");  // loja da venda
                                linha["Senha"] = registrosVenda.LerString("Senha");  //senha da venda
                                linha["Cliente"] = registrosVenda.LerString("Nome"); //cliente que comprou o ingresso
                                linha["ClienteID"] = registrosVenda.LerString("ClienteID"); //preciso do ID para passar no outro objeto 
                                //
                                linha["TaxaConvenienciaValorTotal"] = registrosVenda.LerDecimal("TaxaConvenienciaValorTotal");
                                linha["TaxaEntregaValor"] = registrosVenda.LerDecimal("TaxaEntregaValor");
                                linha["HorarioString"] = registrosIngressos.LerStringFormatoDataHora("Horario");
                                tabela.Rows.Add(linha);
                            }
                        }
                    }
                    registrosVenda.Fechar();
                }
                registrosIngressos.Fechar();

            }
            catch (Exception erro)
            {
                throw erro;
            } // fim de try			
            return tabela;
        } // fim de Ingressos

        public override bool ExcluirCascata()
        {
            bool excluiuTodosItens = true;
            bool excluiuTudo = false;
            try
            {
                // Excluir todos ApresentacaoSetor desta Apresentacao
                ApresentacaoSetorLista apresentacaoSetorLista = new ApresentacaoSetorLista();
                apresentacaoSetorLista.FiltroSQL = "ApresentacaoID = " + this.Control.ID;
                apresentacaoSetorLista.Carregar();
                apresentacaoSetorLista.Primeiro();
                int contador = 0;
                if (apresentacaoSetorLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodosItens = excluiuTodosItens && apresentacaoSetorLista.ApresentacaoSetor.ExcluirCascata();
                        contador++;
                        apresentacaoSetorLista.Proximo();
                    } while (excluiuTodosItens && contador < apresentacaoSetorLista.Tamanho);
                }
                // Excluir esta apresentacao
                if (excluiuTodosItens)
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

        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                { // se for por Canal, os parâmetro têm representações diferentes
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
                #region Obter os dados na condição especificada
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);

                foreach (DataRow linha in tabela.Rows)
                {
                    linha["VariacaoLinha"] = Utilitario.DataHoraBDParaDataHoraLegivel(linha["VariacaoLinha"].ToString());
                }
                // Totais antecipado para poder calcular porcentagem no laço
                //				this.Control.ID = 0; // apresentacao zero pega todos
                int totaisVendidos = QuantidadeIngressosPorApresentacao(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorApresentacao(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorApresentacao(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorApresentacao(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada apresentacao na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorApresentacao(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorApresentacao(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
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
                    linha["R$ Vend"] = ValorIngressosPorApresentacao(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorApresentacao(ingressoLogIDsCancelados);
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
                tabela.Columns["VariacaoLinha"].ColumnName = "Apresentacao";
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
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tApresentacao.ID, tApresentacao.Horario " +
                        "FROM    tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ORDER BY tApresentacao.Horario DESC";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("ID");
                        linha["VariacaoLinha"] = obterDados.LerString("Horario");
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

        public override int QuantidadeIngressosPorApresentacao(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoApresentacao = "";
                    if (this.Control.ID > 0)
                        condicaoApresentacao = "AND (tApresentacao.ID = " + this.Control.ID + ") ";
                    else
                        condicaoApresentacao = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tApresentacao.ID) AS QuantidadeIngressos " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoApresentacao;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de apresentacao
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos apresentacoes
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
        } // fim de QuantidadeIngressosPorApresentacao

        public override decimal ValorIngressosPorApresentacao(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoApresentacao = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoApresentacao = "AND (tApresentacao.ID = " + this.Control.ID + ") ";
                    else
                        condicaoApresentacao = " "; // todos se for = zero
                    string sql;
                    sql =
                        "SELECT   SUM(tPreco.Valor) AS Valor " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoApresentacao;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do apresentacao
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos apresentacoes
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
        } // fim de ValorIngressosPorApresentacao

        public DataTable ListagemFiltrada(int localID, int eventoID, Nullable<bool> disponivelAjuste, Nullable<bool> disponivelRelatorio, Nullable<bool> disponivelVenda)
        {
            try
            {
                DataTable tabela = new DataTable("ListagemApresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Evento", typeof(string));
                tabela.Columns.Add("Horário", typeof(string));
                tabela.Columns.Add("Ajuste", typeof(bool));
                tabela.Columns.Add("Relatório", typeof(bool));
                tabela.Columns.Add("Venda", typeof(bool));
                tabela.Columns.Add("Observação", typeof(string));
                // Obtendo dados

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT tEmpresa.Nome AS Empresa, tLocal.Nome AS Local, tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.ID, tEvento.LocalID, tApresentacao.DisponivelAjuste, tApresentacao.DisponivelRelatorio, ");
                sql.Append("tApresentacao.DisponivelVenda, tApresentacao.Obs ");
                sql.Append("FROM tLocal INNER JOIN ");
                sql.Append("tEmpresa ON tLocal.EmpresaID = tEmpresa.ID INNER JOIN ");
                sql.Append("tEvento ON tLocal.ID = tEvento.LocalID INNER JOIN ");
                sql.Append("tApresentacao ON tEvento.ID = tApresentacao.EventoID ");
                sql.Append(" WHERE @001 @002 @003 @004 @005");
                sql.Append("ORDER BY tEmpresa.Nome, tLocal.Nome, tEvento.Nome, tApresentacao.Horario ");

                #region Popula as condições
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

                if (eventoID > 0)
                {
                    if (disponivelAjuste != null || disponivelRelatorio != null || disponivelVenda != null || localID != 0)
                        sql.Replace("@005", "AND tEvento.ID = " + eventoID.ToString() + " ");
                    else
                    {
                        sql.Replace("@005", "tEvento.ID = " + eventoID.ToString() + " ");
                    }
                }
                else
                {
                    sql.Replace("@005", "");
                }

                if (localID == 0 && (eventoID == 0 || eventoID == -1) && disponivelAjuste == null && disponivelRelatorio == null && disponivelVenda == null)
                {
                    sql.Replace("WHERE", "");
                }
                #endregion


                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Local"] = bd.LerString("Local");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Horário"] = bd.LerStringFormatoSemanaDataHora("Horario");
                    linha["Ajuste"] = bd.LerBoolean("DisponivelAjuste");
                    linha["Relatório"] = bd.LerBoolean("DisponivelRelatorio");
                    linha["Venda"] = bd.LerBoolean("DisponivelVenda");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de Listagem

        public DataTable ListagemFiltrada(int localID, int eventoID, Nullable<bool> disponivelAjuste, Nullable<bool> disponivelRelatorio, Nullable<bool> disponivelVenda, DateTime primeiraData, DateTime segundaData)
        {
            try
            {
                DataTable tabela = new DataTable("ListagemApresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Evento", typeof(string));
                tabela.Columns.Add("Horário", typeof(string));
                tabela.Columns.Add("Ajuste", typeof(bool));
                tabela.Columns.Add("Relatório", typeof(bool));
                tabela.Columns.Add("Venda", typeof(bool));
                tabela.Columns.Add("Observação", typeof(string));
                // Obtendo dados

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT tEmpresa.Nome AS Empresa, tLocal.Nome AS Local, tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.ID, tEvento.LocalID, tApresentacao.DisponivelAjuste, tApresentacao.DisponivelRelatorio, ");
                sql.Append("tApresentacao.DisponivelVenda, tApresentacao.Obs ");
                sql.Append("FROM tLocal INNER JOIN ");
                sql.Append("tEmpresa ON tLocal.EmpresaID = tEmpresa.ID INNER JOIN ");
                sql.Append("tEvento ON tLocal.ID = tEvento.LocalID INNER JOIN ");
                sql.Append("tApresentacao ON tEvento.ID = tApresentacao.EventoID ");
                sql.Append(" WHERE @001 @002 @003 @004 @005 @006");
                sql.Append("ORDER BY tEmpresa.Nome, tLocal.Nome, tEvento.Nome, tApresentacao.Horario ");

                #region Popula as condições

                sql.Replace("@001", "Horario BETWEEN '" + primeiraData.ToString("yyyyMMdd") + "000000' AND '" + segundaData.AddDays(1).ToString("yyyyMMdd") + "000000' ");
                switch (disponivelAjuste)
                {
                    case true:
                        sql.Replace("@002", " AND tApresentacao.DisponivelAjuste='T'");
                        break;
                    case false:
                        sql.Replace("@002", " AND tApresentacao.DisponivelAjuste='F'");
                        break;
                    case null:
                        sql.Replace("@002", "");
                        break;
                }
                switch (disponivelRelatorio)
                {
                    case true:
                        sql.Replace("@003", "AND tApresentacao.DisponivelRelatorio='T'");
                        break;
                    case false:
                        sql.Replace("@003", "AND tApresentacao.DisponivelRelatorio='F'");
                        break;
                    case null:
                        sql.Replace("@003", "");
                        break;
                }

                switch (disponivelVenda)
                {
                    case true:
                        sql.Replace("@004", "AND tApresentacao.DisponivelVenda='T'");
                        break;

                    case false:
                        sql.Replace("@004", "AND tApresentacao.DisponivelVenda='F'");
                        break;

                    case null:
                        sql.Replace("@004", "");
                        break;
                }

                if (localID > 0)
                {
                    sql.Replace("@005", "AND tEvento.LocalID = " + localID.ToString() + " ");
                }
                else
                {
                    sql.Replace("@005", "");
                }

                if (eventoID > 0)
                {
                    sql.Replace("@006", "AND tEvento.ID = " + eventoID.ToString() + " ");
                }
                else
                {
                    sql.Replace("@006", "");
                }

                #endregion


                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Local"] = bd.LerString("Local");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Horário"] = bd.LerStringFormatoSemanaDataHora("Horario");
                    linha["Ajuste"] = bd.LerBoolean("DisponivelAjuste");
                    linha["Relatório"] = bd.LerBoolean("DisponivelRelatorio");
                    linha["Venda"] = bd.LerBoolean("DisponivelVenda");
                    linha["Observação"] = bd.LerString("Obs");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        } // fim de Listagem

        public DataTable Listagem(int localID)
        {
            try
            {
                DataTable tabela = new DataTable("ListagemApresentacao");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Evento", typeof(string));
                tabela.Columns.Add("Horário", typeof(string));
                tabela.Columns.Add("Ajuste", typeof(bool));
                tabela.Columns.Add("Relatório", typeof(bool));
                tabela.Columns.Add("Venda", typeof(bool));
                tabela.Columns.Add("Observação", typeof(string));
                // Obtendo dados
                string condicao = "";
                if (localID > 0)
                    condicao = " WHERE        (tEvento.LocalID = " + localID + ") ";
                else
                    condicao = "  ";
                string sql;
                sql =
                    "SELECT        tEmpresa.Nome AS Empresa, tLocal.Nome AS Local, tEvento.Nome AS Evento, tApresentacao.Horario, tApresentacao.ID, tEvento.LocalID, tApresentacao.DisponivelAjuste, tApresentacao.DisponivelRelatorio,  " +
                    "tApresentacao.DisponivelVenda, tApresentacao.Obs " +
                    "FROM            tLocal INNER JOIN " +
                    "tEmpresa ON tLocal.EmpresaID = tEmpresa.ID INNER JOIN " +
                    "tEvento ON tLocal.ID = tEvento.LocalID INNER JOIN " +
                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID " + condicao +
                    "ORDER BY tEmpresa.Nome, tLocal.Nome, tEvento.Nome, tApresentacao.Horario ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Local"] = bd.LerString("Local");
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Horário"] = bd.LerStringFormatoSemanaDataHora("Horario");
                    linha["Ajuste"] = bd.LerBoolean("DisponivelAjuste");
                    linha["Relatório"] = bd.LerBoolean("DisponivelRelatorio");
                    linha["Venda"] = bd.LerBoolean("DisponivelVenda");
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
            finally
            {
                bd.Fechar();
            }
        } // fim de Listagem

        public DataTable ApresentacoesPorEvento(int eventoID, string registroZero)
        {
            try
            {
                DataTable dttApresentacao = new DataTable();
                dttApresentacao.Columns.Add("ApresentacaoID", typeof(int));
                dttApresentacao.Columns.Add("Apresentacao", typeof(string));
                dttApresentacao.Columns.Add("Evento", typeof(string));

                if (registroZero != null)
                    dttApresentacao.Rows.Add(new Object[] { 0, registroZero, registroZero });

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tApresentacao.ID, tApresentacao.Horario, tEvento.Nome AS Evento  FROM tApresentacao (NOLOCK) ");
                stbSQL.Append("INNER JOIN tEvento (NOLOCK) ON tApresentacao.EventoID = tEvento.ID ");
                stbSQL.Append("WHERE EventoID = " + eventoID + " ORDER BY Horario ");


                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    DataRow linha = dttApresentacao.NewRow();
                    linha["ApresentacaoID"] = bd.LerInt("ID");
                    string data = bd.LerDateTime("Horario").ToString();
                    linha["Apresentacao"] = data.Remove(data.Length - 3);
                    linha["Evento"] = bd.LerString("Evento");
                    dttApresentacao.Rows.Add(linha);
                }
                bd.Fechar();
                return dttApresentacao;

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

        public DataTable ApresentacoesPorEventoDisponivel(int eventoID, string registroZero)
        {
            try
            {
                DataTable dttApresentacao = new DataTable();
                dttApresentacao.Columns.Add("ApresentacaoID", typeof(int));
                dttApresentacao.Columns.Add("Apresentacao", typeof(string));
                dttApresentacao.Columns.Add("Evento", typeof(string));

                if (registroZero != null)
                    dttApresentacao.Rows.Add(new Object[] { 0, registroZero, registroZero });

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tApresentacao.ID, tApresentacao.Horario, tEvento.Nome AS Evento  FROM tApresentacao (NOLOCK) ");
                stbSQL.Append("INNER JOIN tEvento (NOLOCK) ON tApresentacao.EventoID = tEvento.ID ");
                stbSQL.Append("WHERE DisponivelAjuste = 'T' AND  EventoID = " + eventoID + " ORDER BY Horario ");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    DataRow linha = dttApresentacao.NewRow();
                    linha["ApresentacaoID"] = bd.LerInt("ID");
                    string data = bd.LerDateTime("Horario").ToString();
                    linha["Apresentacao"] = data.Remove(data.Length - 3);
                    linha["Evento"] = bd.LerString("Evento");
                    dttApresentacao.Rows.Add(linha);
                }
                bd.Fechar();
                return dttApresentacao;

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

        public static DataTable gerarEstruturaCascading()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID");
            dtt.Columns.Add("Apresentacao");
            dtt.Columns.Add("EventoID");
            return dtt;
        }

        public DataTable ApresentacoesPorEventoCascading(int eventoID)
        {
            try
            {
                DataTable dttApresentacao = gerarEstruturaCascading();

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tApresentacao.ID, tApresentacao.Horario FROM tApresentacao (NOLOCK) ");
                stbSQL.Append("INNER JOIN tEvento (NOLOCK) ON tApresentacao.EventoID = tEvento.ID ");
                stbSQL.Append("WHERE EventoID = " + eventoID + " AND tApresentacao.DisponivelAjuste = 'T' ORDER BY Horario ");


                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    DataRow linha = dttApresentacao.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Apresentacao"] = bd.LerDateTime("Horario").ToString();
                    linha["EventoID"] = eventoID;
                    dttApresentacao.Rows.Add(linha);
                }
                bd.Fechar();
                return dttApresentacao;

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

        public bool RemoverDistribuicaoCotaPorApresentacao(int apresentacaoID)
        {
            CotaItemControle oCotaItem = new CotaItemControle();
            try
            {
                bd.IniciarTransacao();

                this.Ler(apresentacaoID);
                this.CotaID.Valor = 0;
                this.Quantidade.Valor = 0;
                this.QuantidadePorCliente.Valor = 0;

                oCotaItem.ApresentacaoID.Valor = apresentacaoID;
                oCotaItem.ApresentacaoSetorID.Valor = 0;
                oCotaItem.ExcluirControlador(bd);

                bool ok = this.Atualizar(bd);

                bd.FinalizarTransacao();
                return ok;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void RemoverDistribuicaoCotaPorApresentacao(int[] apresentacaoID)
        {
            try
            {
                CotaItemControle oCotaItem = new CotaItemControle();
                bd.IniciarTransacao();
                for (int i = 0; i < apresentacaoID.Length; i++)
                {
                    this.Limpar();
                    this.Ler(apresentacaoID[i]);
                    this.CotaID.Valor = 0;
                    this.Quantidade.Valor = 0;
                    this.QuantidadePorCliente.Valor = 0;

                    oCotaItem.ApresentacaoID.Valor = apresentacaoID[i];
                    oCotaItem.ApresentacaoSetorID.Valor = 0;
                    oCotaItem.ExcluirControlador(bd);

                    this.Atualizar(bd);
                }
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }


        }

        public int DistribuirCotasPorLocal(EstruturaCotasDistribuir oDistribuir)
        {
            try
            {
                List<int> lstEventoID = new List<int>();
                string strSQL = "SELECT ID FROM tEvento (NOLOCK) WHERE LocalID = " + oDistribuir.LocalID;
                bd.Consulta(strSQL);
                while (bd.Consulta().Read())
                    lstEventoID.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                for (int i = 0; i < lstEventoID.Count; i++)
                {
                    oDistribuir.EventoID = lstEventoID[i];
                    this.DistribuirCotasPorEvento(oDistribuir, false);
                }

                bd.FinalizarTransacao();

                return 1;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int DistribuirCotasPorEvento(EstruturaCotasDistribuir oDistribuir, bool fecharBD)
        {
            try
            {

                List<int> lstApresentacaoID = new List<int>();
                string strSQL = "SELECT ID FROM tApresentacao (NOLOCK) WHERE EventoID = " + oDistribuir.EventoID + " AND DisponivelAjuste = 'T'";

                bd.Consulta(strSQL);
                while (bd.Consulta().Read())
                    lstApresentacaoID.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                if (fecharBD)
                    bd.IniciarTransacao();

                for (int i = 0; i < lstApresentacaoID.Count; i++)
                {
                    oDistribuir.ApresentacaoID = lstApresentacaoID[i];
                    this.DistribuirCotasPorApresentacao(oDistribuir, false);
                }
                if (fecharBD)
                    bd.FinalizarTransacao();
                return 1;

            }
            catch (Exception)
            {
                if (fecharBD)
                    bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                if (fecharBD)
                    bd.Fechar();
            }
        }

        public int DistribuirCotasPorApresentacao(EstruturaCotasDistribuir oDistribuir, bool fecharBD)
        {
            CotaItemControle oControle;
            int qtd = 0;
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                this.Ler(oDistribuir.ApresentacaoID);
                bd.FecharConsulta();
                if (fecharBD)
                    bd.IniciarTransacao();

                this.CotaID.Valor = oDistribuir.CotaID;
                this.Quantidade.Valor = oDistribuir.Quantidade;
                this.QuantidadePorCliente.Valor = oDistribuir.QuantidadePorCliente;
                this.Atualizar(bd);



                oControle = new CotaItemControle();
                oControle.ApresentacaoID.Valor = oDistribuir.ApresentacaoID;
                oControle.ApresentacaoSetorID.Valor = 0;
                oControle.Quantidade.Valor = 0;
                List<EstruturaCotaItem> lstReplicar = new List<EstruturaCotaItem>();
                //Encontra os itens que serão replicados
                lstReplicar = oControle.ItensParaReplicar(oDistribuir.CotaID, true);



                ////Hint para saber se ja houve distribuicao para aquela apresentacaosetor, se positivo o hint vem como True
                //if (redistribuir)
                //{
                //    if (replicarAntigaVenda)
                //    {
                //Query dos infernos pra encontrar a quantidade ja vendida daquela CotaItemID
                oControle.ExcluirControlador(bd);
                qtd = oControle.InserirControladorReplicando(bd, lstReplicar);
                //    }
                //    else
                //    {
                //        //Setar pra ZERO a quantidade da Cota e dar UPDATE no CotaItemID se necessario?
                //        oControle.ExcluirControlador(bd);
                //        qtd = oControle.InserirControladorPorCotaID(bd, oDistribuir.CotaID);
                //    }
                //}
                //else
                //{
                //    //Insere o Controlador apartir do CotaID ( Ira buscar todos os CotaItems daquela Cota para adicionar um por um.
                //    qtd = oControle.InserirControladorPorCotaID(bd, oDistribuir.CotaID);
                //}
                if (fecharBD)
                    this.bd.FinalizarTransacao();

                return 1;
            }
            catch (Exception)
            {
                if (fecharBD)
                    this.bd.DesfazerTransacao();

                throw;
            }
            finally
            {
                if (fecharBD)
                    bd.Fechar();
            }
        }

        public bool Inserir(CTLib.BD database)
        {

            try
            {
                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacao(EventoID, Horario, DisponivelVenda, DisponivelAjuste, DisponivelRelatorio, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, Impressao, DescricaoPadrao, Descricao, MapaEsquematicoID, CodigoProgramacao) ");
                sql.Append("VALUES (@001,'@002','@003','@004','@005',@006,@007,@008,@009,'@010','@012','@013', @014, '@015')");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.Horario.ValorBD);
                sql.Replace("@003", this.DisponivelVenda.ValorBD);
                sql.Replace("@004", this.DisponivelAjuste.ValorBD);
                sql.Replace("@005", this.DisponivelRelatorio.ValorBD);
                sql.Replace("@006", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale.ValorBD);
                sql.Replace("@008", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@009", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@010", this.Impressao.ValorBD);
                sql.Replace("@012", this.DescricaoPadrao.ValorBD);
                sql.Replace("@013", this.Descricao.ValorBD);
                sql.Replace("@014", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@015", this.CodigoProgramacao.ValorBD);
                sql.Append("; SELECT SCOPE_IDENTITY();");


                object x = database.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = (this.Control.ID > 0);

                if (result)
                    InserirControle("I", database);


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable SetoresPrecos(List<IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX> lstPesquisa)
        {
            DataRow linha;
            DataTable tabela = new DataTable("RaioX");
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Cód. Evento", typeof(string));
            tabela.Columns.Add("Apresentação", typeof(string));
            tabela.Columns.Add("Cód. Apresentação", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Cód. Setor", typeof(string));
            tabela.Columns.Add("Preço", typeof(string));
            tabela.Columns.Add("Cód. Preço", typeof(string));

            string espacamento = new string(' ', 10);
            StringBuilder sqlWhere = new StringBuilder();
            try
            {


                foreach (IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX itemPesquisa in lstPesquisa)
                {
                    if (sqlWhere.Length > 0)
                        sqlWhere.Append(" OR ");

                    sqlWhere.Append("(");

                    sqlWhere.Append(" (tEvento.LocalID = " + itemPesquisa.LocalID + ") ");

                    if (itemPesquisa.EventoID != 0)
                        sqlWhere.Append(" AND (tApresentacao.EventoID = " + itemPesquisa.EventoID + ") ");

                    if (itemPesquisa.ApresentacaoID != 0)
                        sqlWhere.Append(" AND (tApresentacaoSetor.ApresentacaoID = " + itemPesquisa.ApresentacaoID + ") ");

                    if (itemPesquisa.SetorID != 0)
                        sqlWhere.Append(" AND (tApresentacaoSetor.SetorID = " + itemPesquisa.SetorID + ") ");

                    sqlWhere.Append(")");

                }

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tSetor.ID AS SetorID, " +
                    "   tSetor.Nome AS SetorNome, " +
                    "   tPreco.Nome AS PrecoNome, " +
                    "   tPreco.Valor, " +
                    "   tEvento.Nome AS Evento, " +
                    "   EventoCodigo, " +
                    "   tApresentacao.Horario AS Apresentacao, " +
                    "   ApresentacaoCodigo, " +
                    "   SetorCodigo, " +
                    "   PrecoCodigo " +
                    "FROM tEvento (NOLOCK) " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                    "INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                    "INNER JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = ApresentacaoSetorID " +
                    "LEFT JOIN tCodigoBarra cb (NOLOCK) ON tPreco.ID = cb.PrecoID " +
                    "WHERE " +
                        sqlWhere.ToString() +
                    "ORDER BY " +
                    "   tSetor.Nome, " +
                    "   tPreco.Nome, " +
                    "   tPreco.Valor "))
                {
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["Evento"] = bd.LerString("Evento");
                        linha["Cód. Evento"] = bd.LerString("EventoCodigo");
                        linha["Apresentação"] = bd.LerDateTime("Apresentacao").ToString("dd/MM/yyyy HH:mm");
                        linha["Cód. Apresentação"] = bd.LerString("ApresentacaoCodigo");
                        linha["Setor"] = bd.LerString("SetorNome");
                        linha["Cód. Setor"] = bd.LerString("SetorCodigo");
                        linha["Preço"] = bd.LerString("PrecoNome");
                        linha["Cód. Preço"] = bd.LerString("PrecoCodigo");
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

        public DataTable SetoresPrecos(int eventoID, int apresentacaoID, int setorID, ref string eventoCodigo, ref string apresentacaoCodigo)
        {
            try
            {
                string espacamento = new string(' ', 10);

                DataRow linha;
                DataTable tabela = new DataTable("RaioX");
                tabela.Columns.Add("Descricao", typeof(string));

                string sql = string.Empty;

                //se selecionou evento e todas as apresentações
                if (apresentacaoID == 0)
                {
                    sql = @"SELECT tSetor.ID AS SetorID, 
                    tSetor.Nome AS SetorNome, 
                    tPreco.Nome AS PrecoNome, 
                    tPreco.Valor, 
                    EventoCodigo, 
                    ApresentacaoCodigo, 
                    SetorCodigo, 
                    PrecoCodigo 
                    FROM tEvento (NOLOCK)
                    INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID
                    INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID
                    INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID  
                    INNER JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = ApresentacaoSetorID  
                    LEFT JOIN tCodigoBarra cb (NOLOCK) ON tPreco.ID = cb.PrecoID 
                    WHERE tEvento.ID = @eventoID
                    ORDER BY tSetor.Nome, tPreco.Nome, tPreco.Valor".Replace("@eventoID", eventoID.ToString());
                }

                //se selecionou apresentação e todos setores
                if (apresentacaoID != 0 && setorID == 0)
                {
                    sql = " SELECT " +
                           "tSetor.ID AS SetorID, tSetor.Nome AS SetorNome, tPreco.Nome AS PrecoNome, tPreco.Valor, EventoCodigo, ApresentacaoCodigo, SetorCodigo, PrecoCodigo " +
                           "FROM tApresentacaoSetor (NOLOCK) " +
                           "INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID  " +
                           "INNER JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = ApresentacaoSetorID  " +
                           "LEFT JOIN tCodigoBarra cb (NOLOCK) ON tPreco.ID = cb.PrecoID " +
                           "WHERE  " +
                           "tApresentacaoSetor.ApresentacaoID = " + apresentacaoID +
                           "ORDER BY tSetor.Nome, tPreco.Nome, tPreco.Valor";
                }

                if (apresentacaoID != 0 && setorID != 0)
                {
                    sql = @"SELECT tSetor.ID AS SetorID, 
                    tSetor.Nome AS SetorNome, 
                    tPreco.Nome AS PrecoNome, 
                    tPreco.Valor, 
                    EventoCodigo, 
                    ApresentacaoCodigo, 
                    SetorCodigo, 
                    PrecoCodigo 
                    FROM tApresentacaoSetor (NOLOCK) 
                    INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID  
                    INNER JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = ApresentacaoSetorID  
                    LEFT JOIN tCodigoBarra cb (NOLOCK) ON tPreco.ID = cb.PrecoID 
                    WHERE tApresentacaoSetor.ApresentacaoID = @apresentacaoID AND tApresentacaoSetor.SetorID = @setorID
                    ORDER BY tSetor.Nome, tPreco.Nome, tPreco.Valor".Replace("@apresentacaoID", apresentacaoID.ToString()).Replace("@setorID", setorID.ToString());
                }

                bd.Consulta(sql);

                int setorIDAtual, setorIDAnterior = -1;
                string descricao;

                while (bd.Consulta().Read())
                {
                    setorIDAtual = bd.LerInt("SetorID");

                    // É setor Novo. Insere a linha SOMENTE com setor.
                    if (setorIDAtual != setorIDAnterior)
                    {
                        linha = tabela.NewRow();
                        descricao = bd.LerString("SetorNome");

                        if (bd.LerString("SetorCodigo").Length > 0)
                            descricao += string.Format(" - ({0})", bd.LerString("SetorCodigo"));

                        linha["Descricao"] = descricao;
                        tabela.Rows.Add(linha);
                    }

                    linha = tabela.NewRow();
                    descricao = espacamento + bd.LerString("PrecoNome") + " - " + bd.LerDecimal("Valor").ToString("C");
                    if (bd.LerString("PrecoCodigo").Length > 0)
                        descricao += string.Format(" - ({0})", bd.LerString("PrecoCodigo"));

                    linha["Descricao"] = descricao;
                    tabela.Rows.Add(linha);

                    setorIDAnterior = setorIDAtual;
                    eventoCodigo = bd.LerString("EventoCodigo");
                    apresentacaoCodigo = bd.LerString("ApresentacaoCodigo");
                }

                bd.Fechar();
                return tabela;

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

        public void AdicionarSetores(List<int> apresentacoesID, int empresaID, Dictionary<int, int> setoresXqtde, Dictionary<int, List<Preco>> setorXprecos, Enumerators.TipoCodigoBarra tipoCodigoBarra, bool gerarCodigos)
        {
            BD bdLugares = new BD();
            try
            {
                bd.IniciarTransacao();

                //Adiciona os Setores na Apresentação
                ApresentacaoSetor oApresentacaoSetor;
                ApresentacaoSetor oApresentacaoSetorAux = new ApresentacaoSetor(UsuarioIDLogado);
                Apresentacao oApresentacao = new Apresentacao(UsuarioIDLogado);
                Evento oEvento = new Evento(UsuarioIDLogado);
                Setor oSetor = new Setor(UsuarioIDLogado);
                Ingresso oIngresso = new Ingresso();
                CanalPreco oCanalPreco = new CanalPreco(UsuarioIDLogado);

                Preco pAntigo = new Preco(UsuarioIDLogado);
                Preco novoPreco = new Preco(UsuarioIDLogado);
                List<Preco> Precos;

                int quantidadeTotalIngressos = 0;
                foreach (KeyValuePair<int, int> setorQuantidade in setoresXqtde)
                    quantidadeTotalIngressos += setorQuantidade.Value;

                quantidadeTotalIngressos *= apresentacoesID.Count;

                List<string> CodigosBarra = new List<string>();
                double porcentagem = Convert.ToDouble(ConfigurationManager.AppSettings["PorcentagemListaBraca"]);
                int minimoCodigoBarra = Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMinimaCodigoBarra"]);

                bool usarListaBranca = tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca;

                if (usarListaBranca || gerarCodigos)
                {
                    quantidadeTotalIngressos += (int)Math.Round((quantidadeTotalIngressos * (porcentagem / 100)));
                    CodigosBarra = new CodigoBarra().BuscarListaBranca(bd, quantidadeTotalIngressos);
                }

                foreach (int apresentacaoID in apresentacoesID)
                {
                    oApresentacao.Ler(apresentacaoID);
                    oEvento.Ler(oApresentacao.EventoID.Valor);

                    foreach (KeyValuePair<int, int> setorQuantidade in setoresXqtde)
                    {
                        oApresentacaoSetor = new ApresentacaoSetor(UsuarioIDLogado);
                        oApresentacaoSetor.ApresentacaoID.Valor = apresentacaoID;
                        oApresentacaoSetor.SetorID.Valor = setorQuantidade.Key;
                        oApresentacaoSetor.Inserir(bd);

                        int qtdeSetor = setorQuantidade.Value;

                        setorXprecos.TryGetValue(setorQuantidade.Key, out Precos);

                        foreach (Preco precoAntigo in Precos)
                        {
                            novoPreco.Valor.Valor = precoAntigo.Valor.Valor;
                            //no preço antigo é guardado o valor do novo preço e o ID do preço para copiar
                            pAntigo.Ler(precoAntigo.Control.ID);

                            novoPreco.ApresentacaoSetorID.Valor = oApresentacaoSetor.Control.ID;
                            novoPreco.CorID.Valor = pAntigo.CorID.Valor;
                            novoPreco.Impressao.Valor = pAntigo.Impressao.Valor;
                            novoPreco.Nome.Valor = pAntigo.Nome.Valor;
                            novoPreco.Quantidade.Valor = pAntigo.Quantidade.Valor;
                            novoPreco.QuantidadePorCliente.Valor = pAntigo.QuantidadePorCliente.Valor;

                            novoPreco.Inserir(oApresentacao.EventoID.Valor, oApresentacaoSetor.SetorID.Valor, oApresentacao.Control.ID, true, bd);                            

                            foreach (DataRow linhaCanalPreco in precoAntigo.Canais().Rows)
                            {                                
                                oCanalPreco.CanalID.Valor = (int)linhaCanalPreco["ID"];
                                oCanalPreco.PrecoID.Valor = novoPreco.Control.ID;
                                oCanalPreco.Inserir(bd, false);
                            }

                            //#MapaLugarMarcado
                            //Adiciona o Preco ID Novo na APS Nova apartir do "Preco Antigo"
                            //Preco gerado por Base, pode acontecer de ocorrerem multiplos updates,
                            //O ultimo irá prevalecer.
                            oApresentacaoSetorAux.LerNolock(pAntigo.ApresentacaoSetorID.Valor);

                            if (pAntigo.Control.ID == oApresentacaoSetorAux.PrincipalPrecoID.Valor)
                                oApresentacaoSetor.AtualizarPrecoPrincipal(bd, novoPreco.Control.ID);
                        }

                        List<string> codigosApresentacaoSetor = new List<string>();

                        double quantidadeAPS = qtdeSetor;

                        if ((usarListaBranca || gerarCodigos) && quantidadeAPS > 0)
                        {
                            //Busca os X códigos que serão associados aos INGRESSOS! -- Minimo ou a quantidade do setor
                            int qtdCodigosAPS = Math.Max((int)(quantidadeAPS + (quantidadeAPS * (porcentagem / 100))), minimoCodigoBarra);

                            codigosApresentacaoSetor = CodigosBarra.Take(qtdCodigosAPS).ToList();
                            CodigosBarra.RemoveAll(c => codigosApresentacaoSetor.Contains(c));

                            new CodigoBarraEvento().GerarCodigos(oApresentacaoSetor.Control.ID, codigosApresentacaoSetor, DateTime.Now, oApresentacao.EventoID.Valor, bd);
                        }

                        if (!usarListaBranca && gerarCodigos)
                            codigosApresentacaoSetor = new List<string>();

                        //Gerar ingressos
                        bdLugares = oSetor.Lugares(oApresentacaoSetor.SetorID.Valor);
                        int sequencia = oIngresso.UltimoCodigoSequencial(apresentacaoID) + 1;
                        List<string> codigosLugar = new List<string>();

                        while (bdLugares.Consulta().Read())
                        {
                            int qtde = bdLugares.LerInt("Quantidade");

                            if (usarListaBranca || gerarCodigos)
                            {
                                codigosLugar = codigosApresentacaoSetor.Take(qtde).ToList();
                                codigosApresentacaoSetor.RemoveAll(c => codigosLugar.Contains(c));
                            }

                            // Verifica se é lugar marcado.
                            if (bdLugares.LerString("LugarMarcado") != Setor.Pista)
                            {
                                oIngresso.NovoMarcado(oApresentacaoSetor.Control.ID, oApresentacao.EventoID.Valor, oApresentacao.Control.ID, oApresentacaoSetor.SetorID.Valor, empresaID, oEvento.LocalID.Valor, bdLugares.LerInt("BloqueioID"), bdLugares.LerInt("ID"), bdLugares.LerInt("Quantidade"), bdLugares.LerInt("QuantidadeBloqueada"), bdLugares.LerString("Codigo"), bdLugares.LerInt("Grupo"), bdLugares.LerInt("Classificacao"), bdLugares.LerString("LugarMarcado"), bd, sequencia, codigosLugar.ToArray());

                                sequencia += qtde;
                            }
                            else
                            {
                                oIngresso.Acrescentar(oApresentacaoSetor.Control.ID, oApresentacao.EventoID.Valor, oApresentacao.Control.ID, oApresentacaoSetor.SetorID.Valor, empresaID, oEvento.LocalID.Valor, 0, qtdeSetor, 1, bd, true, codigosLugar); // Inicia com código 1
                            }
                        }
                        bdLugares.FecharConsulta();
                    }
                }
                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();

                throw ex;
            }
            finally
            {
                bd.Fechar();
                bdLugares.Fechar();
            }
        }

        public object SetoresSemIngresso(string registroZero)
        {
            DataRow linha;

            // Estrutura de retorno
            DataTable tabela = new DataTable("SetoresSemIngresso");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                // Valor padrão
                if (registroZero != null)
                {
                    linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = registroZero;
                    tabela.Rows.Add(linha);
                }

                // Se o ID da Apresentação for preenchido
                if (this.Control.ID > 0)
                {
                    // Preenche os setores da apresentação que não sejam pista, tenham apresentações disponíveis para ajuste e não tenham ingressos gerados
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT DISTINCT " +
                        "	tSetor.ID, " +
                        "	tSetor.Nome " +
                        "FROM tSetor (NOLOCK) " +
                        "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "INNER JOIN tApresentacao (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "WHERE " +
                        "   (tApresentacao.ID = " + this.Control.ID + ") " +
                        "AND " +
                        "   (tApresentacao.DisponivelAjuste = 'T') " +
                        "AND " +
                        "	(tApresentacaoSetor.IngressosGerados = 'F') " +
                        "AND " +
                        "   (tSetor.LugarMarcado <> '" + Setor.Pista + "') " +
                        "ORDER BY tSetor.Nome"))
                    {

                        while (oDataReader.Read())
                        {
                            linha = tabela.NewRow();
                            linha["ID"] = bd.LerInt("ID");
                            linha["Nome"] = bd.LerString("Nome");
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

        public DataTable ListaBranca(List<IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX> lstPesquisa)
        {
            //estruturta da tabela
            DataTable tabela = new DataTable("PreImpressoEvento");

            tabela.Columns.Add("Senha", typeof(string));
            tabela.Columns.Add("CodigoBarra", typeof(string));
            tabela.Columns.Add("ClienteNome", typeof(string));
            tabela.Columns.Add("SetorNome", typeof(string));
            tabela.Columns.Add("Codigo", typeof(string));
            tabela.Columns.Add("PrecoNome", typeof(string));
            tabela.Columns.Add("Valor", typeof(decimal));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("CortesiaNome", typeof(string));

            string setorID = "";
            string apresentacaoID = "";
            string eventoID = "";
            string localID = "";

            foreach (IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX itemPesquisa in lstPesquisa)
            {
                if (itemPesquisa.ApresentacaoID != 0)
                {
                    if (apresentacaoID.Length > 0)
                        apresentacaoID += "," + itemPesquisa.ApresentacaoID;
                    else
                        apresentacaoID += itemPesquisa.ApresentacaoID;
                }

                if (itemPesquisa.SetorID != 0)
                {
                    if (setorID.Length > 0)
                        setorID += "," + itemPesquisa.SetorID;
                    else
                        setorID += itemPesquisa.SetorID;
                }

                if (itemPesquisa.EventoID != 0)
                {
                    if (eventoID.Length > 0)
                        eventoID += "," + itemPesquisa.EventoID;
                    else
                        eventoID += itemPesquisa.EventoID;
                }

                if (itemPesquisa.LocalID != 0)
                {
                    if (localID.Length <= 0)
                        localID += itemPesquisa.LocalID;

                }
            }

            BD bd = new BD();

            String status = "'" + (char)Ingresso.StatusIngresso.AGUARDANDO_TROCA + "', '" + (char)Ingresso.StatusIngresso.VENDIDO + "', '" +
                            (char)Ingresso.StatusIngresso.IMPRESSO + "', '" + (char)Ingresso.StatusIngresso.ENTREGUE + "', '" + (char)Ingresso.StatusIngresso.PREIMPRESSO + "' ,'" + (char)Ingresso.StatusIngresso.PRE_RESERVA + "'";

            string sql = @"SELECT DISTINCT tVendabilheteria.Senha,
            CASE WHEN LEN(tIngresso.CodigoBarraCliente) > 0
                THEN ISNULL(tIngresso.CodigoBarraCliente , '')
                ELSE ISNULL(tIngresso.CodigoBarra , '')
            END AS CodigoBarra ,
            tCliente.Nome as ClienteNome,tSetor.Nome as SetorNome,tIngresso.Codigo,tPreco.Valor,tIngresso.Status, 
            tPreco.Nome AS PrecoNome, tCortesia.Nome AS CortesiaNome 
            FROM tIngresso (NOLOCK)
            INNER JOIN tIngressolog (NOLOCK) on tIngresso.ID = tIngressolog.IngressoID
            INNER JOIN tVendabilheteria (NOLOCK) on tVendaBilheteria.ID = tIngressolog.VendaBilheteriaID AND tIngresso.VendaBilheteriaID = tIngressolog.VendaBilheteriaID
            LEFT JOIN tCliente (NOLOCK) on tIngressolog.ClienteID = tCliente.ID
            INNER JOIN tSetor (NOLOCK) on tIngresso.SetorID = tSetor.ID
            INNER JOIN tPreco (NOLOCK) on tIngresso.PrecoID = tPreco.ID
            LEFT JOIN tCortesia (NOLOCK) ON tIngresso.CortesiaID = tCortesia.ID
			WHERE tIngresso.Status IN (" + status + ") AND tIngressoLog.Acao = 'V' ";

            if (localID.Length > 0)
                sql += " AND tIngresso.LocalID =   " + localID + "  ";
            if (eventoID.Length > 0)
                sql += " AND tIngresso.EventoID IN ( " + eventoID + " ) ";
            if (apresentacaoID.Length > 0)
                sql += " AND tIngresso.ApresentacaoID IN ( " + apresentacaoID + ") ";
            if (setorID.Length > 0)
                sql += " AND tSetor.ID IN (" + setorID + ") ";
            sql += @"ORDER BY tcliente.Nome,tVendabilheteria.Senha,tIngresso.Codigo,tIngresso.Status";

            bd.Consulta(sql);

            while (bd.Consulta().Read())
            {
                DataRow linhaTabela = tabela.NewRow();

                linhaTabela["Senha"] = bd.LerString("Senha");
                linhaTabela["CodigoBarra"] = "'" + bd.LerString("CodigoBarra");
                linhaTabela["ClienteNome"] = bd.LerString("ClienteNome");
                linhaTabela["SetorNome"] = bd.LerString("SetorNome");
                linhaTabela["Codigo"] = bd.LerString("Codigo");
                linhaTabela["Valor"] = bd.LerDecimal("Valor");
                linhaTabela["Status"] = Ingresso.StatusDescritivo(bd.LerString("Status"));
                linhaTabela["PrecoNome"] = bd.LerString("PrecoNome");
                linhaTabela["CortesiaNome"] = bd.LerString("CortesiaNome");

                tabela.Rows.Add(linhaTabela);
            }

            return tabela;
        }

        public DataTable ListaNegra(IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX estrutura)
        {
            DateTime dataHoraAtual = DateTime.Now;

            //Objeto de retorno
            DataTable blackList = new DataTable("ListaNegraEvento");

            blackList.Columns.Add("EventoNome", typeof(string));
            blackList.Columns.Add("DataHoraInclusao", typeof(DateTime));
            blackList.Columns.Add("DataHoraSincronizacao", typeof(DateTime));
            blackList.Columns.Add("CodigoBarra", typeof(string));

            if (estrutura.EventoID == 0)
                throw new Exception("Selecione um evento para realizar essa busca!");

            string filtroEvento = "WHERE EventoID = " + estrutura.EventoID;
            DataRow blackListItem;

            try
            {
                //Essa query busca os registros da tIngressoCodigoBarra do evento
                //em que o campo BlackList seja true (T)
                string sql = "SELECT TimeStamp, CodigoBarra FROM tIngressoCodigoBarra (NOLOCK) " +
                                filtroEvento +
                                " AND BlackList = 'T'" +
                                " ORDER BY TimeStamp";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    blackListItem = blackList.NewRow();
                    blackListItem["EventoNome"] = estrutura.EventoNome;
                    blackListItem["DataHoraInclusao"] = DateTime.ParseExact(bd.LerString("TimeStamp"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    blackListItem["DataHoraSincronizacao"] = dataHoraAtual;
                    blackListItem["CodigoBarra"] = "'" + bd.LerString("CodigoBarra");

                    blackList.Rows.Add(blackListItem);
                }

                bd.Fechar();

                return blackList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ListaNegra(List<IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX> lstPesquisa)
        {
            //variável para controlar a mudança de ingressos dentro da lista
            int ingressoIDAnterior = 0;
            DateTime dataHoraAtual = DateTime.Now;
            //controle para saber quais códigos entram na black list
            bool codigoBarraCancelado = false;
            string acaoCodigoBarraCancelado = "";

            //Objetos de retorno

            DataTable blackList = new DataTable("PreImpressoEvento");

            blackList.Columns.Add("DataHoraInclusao", typeof(DateTime));
            blackList.Columns.Add("DataHoraSincronizacao", typeof(DateTime));
            blackList.Columns.Add("CodigoBarra", typeof(string));
            blackList.Columns.Add("Motivo", typeof(string));
            blackList.Columns.Add("MotivoCancelamentoReimprecao", typeof(string));
            blackList.Columns.Add("Portaria", typeof(string));
            blackList.Columns.Add("ColetorNumero", typeof(int));

            DataRow blackListItem;

            //esses objeto servem para auxiliar ao ler e filtrar os códigos de barra que devem ir para a black list.
            List<EstruturaIngressoLog_BlackList> leituraBanco = new List<EstruturaIngressoLog_BlackList>();

            string setorID = "";
            string apresentacaoID = "";
            string eventoID = "";
            string localID = "";

            foreach (IRLib.Paralela.ClientObjects.EstruturaApresentacaoRaioX itemPesquisa in lstPesquisa)
            {
                if (itemPesquisa.ApresentacaoID != 0)
                {
                    if (apresentacaoID.Length > 0)
                        apresentacaoID += "," + itemPesquisa.ApresentacaoID;
                    else
                        apresentacaoID += itemPesquisa.ApresentacaoID;
                }

                if (itemPesquisa.SetorID != 0)
                {
                    if (itemPesquisa.SetorID > 0)
                    {
                        if (setorID.Length > 0)
                            setorID += "," + itemPesquisa.SetorID;
                        else
                            setorID += itemPesquisa.SetorID;
                    }
                }

                if (itemPesquisa.EventoID != 0)
                {
                    if (eventoID.Length > 0)
                        eventoID += "," + itemPesquisa.EventoID;
                    else
                        eventoID += itemPesquisa.EventoID;
                }

                if (itemPesquisa.LocalID != 0)
                {
                    if (localID.Length <= 0)
                        localID += itemPesquisa.LocalID;
                }
            }

            string filtroSetor = setorID.Length > 0 ? " AND SetorID IN ( " + setorID + " ) " : "";
            string filtroEvento = eventoID.Length > 0 ? " AND tIngresso.EventoID IN ( " + eventoID + " ) " : "";
            string filtroApresentacao = apresentacaoID.Length > 0 ? " AND tIngresso.ApresentacaoID IN ( " + apresentacaoID + " ) " : "";
            string filtroLocal = localID.Length > 0 ? " WHERE tIngresso.LocalID =   " + localID + "  " : "";

            if (filtroLocal == "" || filtroLocal == null)
                throw new Exception("Selecione um local para realizar essa busca!");

            try
            {
                //Essa query busca todos os registros da IngressoLog desse evento, apresentação e setor
                //de forma ordenada por IngressoID e
                //para que os registros de atualização ou cancelamento de código de barras apareçam primeiro
                string sql = @"SELECT tIngressoLog.IngressoID, tIngressoLog.TimeStamp, tIngressoLog.Acao, tIngressoLog.CodigoBarra, tIngressoLog.Obs  
                                FROM tIngresso (NOLOCK)
                                INNER JOIN tIngressoLog (NOLOCK) ON tIngresso.ID = tIngressoLog.IngressoID"
                                + filtroLocal + filtroEvento + filtroApresentacao + filtroSetor +
                                " ORDER BY IngressoID, tIngressoLog.ID DESC	";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    leituraBanco.Add(new EstruturaIngressoLog_BlackList()
                    {
                        IngressoID = bd.LerInt("IngressoID"),
                        TimeStamp = Convert.ToDateTime(bd.LerStringFormatoDataHora("TimeStamp")),
                        Acao = bd.LerString("Acao"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        Obs = bd.LerString("Obs"),
                    });
                }

                bd.Fechar();

                //Verifica quais códigos de barra devem entrar na black list e quais não devem.
                foreach (EstruturaIngressoLog_BlackList item in leituraBanco)
                {
                    if (ingressoIDAnterior != item.IngressoID)
                        codigoBarraCancelado = false;

                    if ((item.Acao == IngressoLog.CANCELAR || item.Acao == IngressoLog.CANCELAR_PREIMPRESSO || item.Acao == IngressoLog.ANULAR_IMPRESSAO || item.Acao == IngressoLog.REIMPRIMIR) && !codigoBarraCancelado)
                    {
                        //quando o ingresso estiver com uma das ações acima todos os códigos de barra das ações após a ação
                        //atual devem ser incluidos na black list.
                        codigoBarraCancelado = true;
                        acaoCodigoBarraCancelado = item.Acao;
                    }
                    else
                    {
                        if (codigoBarraCancelado)//esse ingresso deve ter os códigos inseridos na Black List
                        {
                            //No caso de pré-Impresso, deve-se pegar o registro de impressão não o de Emisão
                            //apesar do registro de emissão conter o Código de Barra
                            string motivo = "";

                            if (item.CodigoBarra != "" && item.Acao != IngressoLog.EMISSAO_PREIMPRESSO)
                            {
                                blackListItem = blackList.NewRow();
                                blackListItem["CodigoBarra"] = "'" + item.CodigoBarra;
                                blackListItem["DataHoraInclusao"] = item.TimeStamp;
                                blackListItem["DataHoraSincronizacao"] = dataHoraAtual;

                                //preenche o motivo do ingresso estar na black list
                                switch (acaoCodigoBarraCancelado)
                                {
                                    case IngressoLog.CANCELAR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoCancelado.ToString();
                                            break;
                                        }
                                    case IngressoLog.CANCELAR_PREIMPRESSO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.PreImpressoCancelado.ToString();
                                            break;
                                        }
                                    case IngressoLog.ANULAR_IMPRESSAO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.ImpressaoCancelada.ToString();
                                            break;
                                        }
                                    case IngressoLog.REIMPRIMIR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoReimpresso.ToString();
                                            break;
                                        }
                                }

                                blackListItem["Motivo"] = motivo;
                                blackListItem["MotivoCancelamentoReimprecao"] = item.Obs;
                                blackList.Rows.Add(blackListItem);
                            }
                        }
                    }
                    ingressoIDAnterior = item.IngressoID;
                }
                return blackList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime ApresentacaoMaisProxima(List<int> listaApresentacao)
        {
            try
            {
                DateTime apresentacaoMaisProxima = DateTime.Now;
                string apresentacao = "";
                foreach (int ApresentacaoID in listaApresentacao)
                    apresentacao += ApresentacaoID + ",";

                if (apresentacao.Length > 0)
                    apresentacao = apresentacao.Substring(0, apresentacao.Length - 1);


                string sql = "";

                sql = @" SELECT MIN(horario) as horario
                        FROM tApresentacao (NOLOCK)
                        WHERE ID in ( " + apresentacao + " ) ";

                if (bd.Consulta(sql).Read())
                    apresentacaoMaisProxima = bd.LerDateTime("horario");

                return apresentacaoMaisProxima;

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

        public List<EstruturaSelecaoArvoreCompleta> Buscar(int regionalID, int empresaID, int localID, int eventoID)
        {
            try
            {
                List<EstruturaSelecaoArvoreCompleta> lista = new List<EstruturaSelecaoArvoreCompleta>();

                string filtro = string.Empty;
                if (eventoID > 0)
                    filtro = "AND e.ID = " + eventoID;
                else if (localID > 0)
                    filtro = "AND l.ID = " + localID;
                else if (empresaID > 0)
                    filtro = "AND em.ID = " + empresaID;
                else if (regionalID > 0)
                    filtro = "AND r.ID = " + regionalID;


                string consulta =
                    string.Format(
                    @"
                        SELECT 
                            r.ID AS RegionalID, r.Nome AS Regional, 
                            em.ID AS EmpresaID, em.Nome AS Empresa,
                            l.ID AS LocalID, l.Nome AS Local,
                            e.ID AS EventoID, e.Nome AS Evento,
                            ap.ID AS ApresentacaoID, ap.Horario
                        FROM tRegional r (NOLOCK)
                        INNER JOIN tEmpresa em (NOLOCK) ON em.RegionalID = r.ID
                        INNER JOIN tLocal l (NOLOCK) ON l.EmpresaID = em.ID
                        INNER JOIN tEvento e (NOLOCK) ON e.LocalID = l.ID
                        INNER JOIN tApresentacao ap (NOLOCK) ON ap.EventoID = e.ID
                        WHERE ap.DisponivelAjuste='T' {0}
                        ORDER BY r.Nome, e.Nome, l.Nome, ap.Horario
                    ", filtro.Length > 0 ? filtro : string.Empty);


                bd.Consulta(consulta);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaSelecaoArvoreCompleta()
                    {
                        RegionalID = bd.LerInt("RegionalID"),
                        Regional = bd.LerString("Regional"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        Empresa = bd.LerString("Empresa"),
                        LocalID = bd.LerInt("LocalID"),
                        Local = bd.LerString("Local"),
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Evento"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Apresentacao = bd.LerDateTime("Horario").ToString("dddd, dd/MM/yyyy HH:mm"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool VerificarVendas()
        {
            try
            {
                string strSql = string.Format(@"SELECT TOP 1 ID FROM tIngresso ti WHERE ti.ApresentacaoID = {0} AND (ti.Status = 'V' OR ti.Status = 'I' OR ti.Status = 'E')", this.Control.ID);

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
    } // fim da classe Apresentacao

    public class ApresentacaoLista : ApresentacaoLista_B
    {
        public ApresentacaoLista() { }

        public ApresentacaoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela de todos os campos de apresentacao carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            DataTable tabela = new DataTable("Apresentacao");

            try
            {

                if (this.Primeiro())
                {
                    tabela.Columns.Add("Evento", typeof(string));
                    tabela.Columns.Add("Horario", typeof(string));
                    do
                    {
                        DataRow linha = tabela.NewRow();
                        // Local eh chave estrangeira, obter o nome
                        Evento evento = new Evento();
                        evento.Ler(apresentacao.EventoID.Valor);
                        linha["Evento"] = evento.Nome.Valor;
                        //
                        linha["Horario"] = apresentacao.Horario.Valor.ToString("dd/MM/yyyy HH:mm");
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());
                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

            }
            catch
            {
                tabela = null;
            }

            return tabela;

        }
    }
}
