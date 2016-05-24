/**************************************************
* Arquivo: Preco.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Linq;

namespace IRLib
{

    public class Preco : Preco_B
    {
        private RoboCanalPreco oRoboCanalPreco = new RoboCanalPreco();

        public bool IRVende { get; set; }
        public const string IMPRESSAO_NOME = "N";
        public const string IMPRESSAO_VALOR = "$";
        public const string IMPRESSAO_AMBOS = "A";

        /// <summary>
        /// Lista de IDs dos canais onde o preço esta distribuido
        /// </summary>
        public List<int> ListaCanalIDs { get; private set; }

        public enum TipoImpressao
        {
            Nome = 'N',
            Valor = '$',
            Ambos = 'A'
        }

        public Preco() { }

        #region Métodos de Manipulação do Pacote Item

        #region Inserir


        public void AssociaCotaItemIngresso(int ApresentacaoID, int UsuarioID)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("EXEC AssociaCotaItemIngresso @001,@002");
                sql.Replace("@001", ApresentacaoID.ToString());
                sql.Replace("@002", UsuarioID.ToString());
                bd.Executar(sql);
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
        public void AdicionarCotaItemID(int PrecoID, int ApresentacaoID, string PrecoInicia)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("EXEC sp_AdicionaCotaItemPreco @001,@002,'@003'");
                sql.Replace("@001", PrecoID.ToString());
                sql.Replace("@002", ApresentacaoID.ToString());
                sql.Replace("@003", PrecoInicia);
                bd.Executar(sql);
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
        private bool Inserir(BD bd)
        {
            try
            {

                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tPreco(Nome, CorID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende, ImprimirCarimbo, CarimboTexto1, CarimboTexto2, PrecoTipoID, CodigoCinema) ");
                sql.Append("VALUES ('@001', @002,'@003',@004,@005,@006,'@007','@008','@009','@010','@011','@012', '@013');SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);
                sql.Replace("@006", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@007", this.Impressao.ValorBD);
                sql.Replace("@008", string.Empty);
                sql.Replace("@009", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@010", this.CarimboTexto1.ValorBD);
                sql.Replace("@011", this.CarimboTexto2.ValorBD);
                sql.Replace("@011", this.CarimboTexto2.ValorBD);
                sql.Replace("@012", this.PrecoTipoID.ValorBD);
                sql.Replace("@013", this.CodigoCinema.ValorBD);
                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                if (result)
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Atualizar

        /// <summary>
        /// Atualiza Preco
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPreco SET Nome = '@001', CorID = @002, Valor = '@003', ApresentacaoSetorID = @004, Quantidade = @005, QuantidadePorCliente = @006, Impressao = '@007', IRVende = '@008', ImprimirCarimbo = '@009', CarimboTexto1 = '@010', CarimboTexto2 = '@011', PrecoTipoID = @012 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);
                sql.Replace("@006", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@007", this.Impressao.ValorBD);
                sql.Replace("@008", string.Empty);
                sql.Replace("@009", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@010", this.CarimboTexto1.ValorBD);
                sql.Replace("@011", this.CarimboTexto2.ValorBD);
                sql.Replace("@012", this.PrecoTipoID.ValorBD);


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

        protected internal void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xPreco (ID, Versao, Nome, CorID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende) ");
                sql.Append("SELECT ID, @V, Nome, CorID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende FROM tPreco WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected internal void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cPreco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        #endregion

        #endregion

        public bool CarregarListaCanaisPorPrecoID(int precoId)
        {

            try
            {
                ListaCanalIDs = new List<int>();

                string sql = "SELECT CanalID FROM tCanalPreco WHERE PrecoID = " + precoId;
                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    ListaCanalIDs.Add(bd.LerInt("CanalID"));
                }

                bd.Fechar();

                return true;

            }
            catch (Exception ex)
            {
                return false;
                // throw ex;
            }

        }


        public Preco LerPorApresentacaoSetorID(int id)
        {

            try
            {

                string sql = "SELECT * FROM tPreco WHERE Nome = 'ClienteVivo' AND ApresentacaoSetorID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = Convert.ToInt32(bd.LerString("ID"));
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CorID.ValorBD = bd.LerInt("CorID").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.Impressao.ValorBD = bd.LerString("Impressao");
                }
                else
                {
                    this.Limpar();
                }

                bd.Fechar();

                return this;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable TiposImpressao()
        {

            try
            {

                DataTable tabela = new DataTable("TipoImpressao");

                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));

                DataRow linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_AMBOS;
                linha["Nome"] = "Ambos";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_NOME;
                linha["Nome"] = "Nome";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = IMPRESSAO_VALOR;
                linha["Nome"] = "Valor";
                tabela.Rows.Add(linha);

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Preco(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Exclui Preco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                //verificar se há ingressos vendidos com esse preço.
                IngressoLista lista = new IngressoLista();
                lista.FiltroSQL = "PrecoID=" + id;
                lista.TamanhoMax = 1;
                lista.Carregar();

                if (lista.Tamanho > 0)
                { //nao pode excluir
                    throw new PrecoException("Preço não pode ser excluído pois já existem ingressos a esse preço! Desative esse preço em todos os canais para deixar de vendê-lo.");
                }

                CanalPrecoLista listaCanalPreco = new CanalPrecoLista();
                listaCanalPreco.FiltroSQL = "PrecoID=" + id;
                bool ok = listaCanalPreco.ExcluirTudo();

                if (ok)
                    return base.Excluir(id);
                else
                    throw new PrecoException("Não foi possível excluir os canais em que esse preço está associado.");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui Preco
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            return this.Excluir(this.Control.ID);

        }

        public string ExcluirPrecoManutencao(int id)
        {
            string mensagemRetorno = "";
            try
            {
                int qtde = 0;
                string sql = "SELECT COUNT(ID) AS QTDE FROM tIngressoLog WHERE PrecoID = " + id;
                while (bd.Consulta(sql).Read())
                {
                    qtde = bd.LerInt("QTDE");
                }

                if (qtde > 0)
                {
                    mensagemRetorno = "Preço não pode ser excluído pois já existem ingressos a esse preço! Desative esse preço em todos os canais para deixar de vendê-lo.";
                }
                else
                {
                    bd.IniciarTransacao();

                    this.Control.ID = id;

                    string sqlSelect = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                    object obj = bd.ConsultaValor(sqlSelect);
                    int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                    this.Control.Versao = versao;

                    InserirControle("D");
                    InserirLog();

                    string sqlDelete = "DELETE FROM tPreco WHERE ID=" + id;

                    int x = bd.Executar(sqlDelete);

                    if (x == 1)
                    {
                        mensagemRetorno = "Preço excluído com sucesso!";
                    }

                    bd.FinalizarTransacao();
                }
            }
            catch (Exception ex)
            {
                mensagemRetorno = ex.Message;
            }

            return mensagemRetorno;
        }

        public decimal GetByID(int precoID)
        {

            decimal valor = 0;

            try
            {

                object obj = bd.ConsultaValor("" +
                    "SELECT " +
                    "   tPreco.Valor " +
                    "FROM " +
                    "   tPreco " +
                    "WHERE " +
                    "   tPreco.ID = " + precoID);

                valor = (obj != null) ? Convert.ToDecimal(obj) : 0;

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

            return valor;
        }

        public override DataTable Precos(string registroZero, int canalID, int apresentacaoSetorID)
        {
            try
            {
                DataTable tabela = new DataTable("Preco");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                string sql =


                    "SELECT     tPreco.ID, tPreco.Nome " +
                    "FROM       tPreco INNER JOIN " +
                    "tCanalPreco ON tPreco.ID = tCanalPreco.PrecoID INNER JOIN " +
                    "tApresentacaoSetor ON tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "WHERE        (tCanalPreco.CanalID = " + canalID + ") AND (tApresentacaoSetor.ID = " + apresentacaoSetorID + ") " +
                    "ORDER BY tPreco.Nome ";
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
        /// Obtem os canais deste preço
        /// </summary>
        /// <returns></returns>
        public override DataTable Canais()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT DISTINCT tCanal.ID, tCanal.Nome " +
                    "FROM tCanal, tCanalPreco " +
                    "WHERE tCanal.ID=tCanalPreco.CanalID AND tCanalPreco.PrecoID=" + this.Control.ID + " " +
                    "ORDER BY tCanal.Nome";

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
        /// Obtem os canais ativos deste preço
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisAtivos()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT DISTINCT tCanal.ID, tCanal.Nome " +
                    "FROM tCanal INNER JOIN tCanalPreco ON tCanal.ID=tCanalPreco.CanalID " +
                    "WHERE tCanal.Ativo = 'T' AND tCanalPreco.PrecoID=" + this.Control.ID + " " +
                    "ORDER BY tCanal.Nome";

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
        /// Obtem os canais deste preço dada uma empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(int empresaID)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT DISTINCT tCanal.ID, tCanal.Nome " +
                    "FROM tCanal, tCanalPreco " +
                    "WHERE tCanal.ID=tCanalPreco.CanalID AND tCanalPreco.PrecoID=" + this.Control.ID + " AND " +
                    "tCanal.EmpresaID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome";

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
        /// Obtem os canais deste preço dada uma empresa
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisAtivos(int empresaID)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT DISTINCT tCanal.ID, tCanal.Nome " +
                    "FROM tCanal INNER JOIN tCanalPreco " +
                    "ON tCanal.ID=tCanalPreco.CanalID WHERE tCanal.Ativo = 'T' AND tCanalPreco.PrecoID=" + this.Control.ID + " AND " +
                    "tCanal.EmpresaID=" + empresaID + " " +
                    "ORDER BY tCanal.Nome";

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
        /// Obtem os canais deste preço dada uma empresa
        /// </summary>
        /// <returns></returns>
        public List<int> FiltroPorTipoCanal(List<int> precosIDs, EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco tipoFiltragem)
        {

            try
            {
                string precos = String.Join(",", precosIDs);

                string versao = ConfigurationManager.AppSettings["ConfigVersion"];
                string canalCallCenter = Configuracao.Get("CanaisDistribuicaoCC", versao);
                string canalSite = Configuracao.Get("CanaisDistribuicaoWeb", versao);

                List<int> result = new List<int>();

                string sql = String.Format("SELECT DISTINCT tPreco.ID " +
                                           "FROM tPreco " +
                                           "INNER JOIN tCanalPreco ON tPreco.ID=tCanalPreco.PrecoID " +
                                           "INNER JOIN tCanal ON tCanal.ID=tCanalPreco.CanalID " +
                                           "WHERE tPreco.ID IN ({0}) ", precos);

                switch (tipoFiltragem)
                {
                    case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.PontoVenda:
                        sql = sql + String.Format(" AND tCanal.ID NOT IN ({0}, {1})", canalCallCenter, canalSite);
                        break;

                    case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.CallCenter:
                        sql = sql + String.Format(" AND tCanal.ID IN ({0})", canalCallCenter);
                        break;

                    case EstruturaCanaisPreco.EnumTipoFiltragemCanaisPreco.Internet:
                        sql = sql + String.Format(" AND tCanal.ID IN ({0})", canalSite);
                        break;

                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    result.Add(bd.LerInt("ID"));
                }

                return result;

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

        public DataTable precosAlteraveis(int eventoID, int setorID, int apresentacaoID, int precoID, int todosID)
        {

            DataTable tabela = new DataTable("Precos");

            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
            tabela.Columns.Add("Valor", typeof(decimal));

            string sql = string.Empty;

            //REGRAS 3 listas(setor,apresentacao,preco) = 7 possibilidades
            if (setorID == todosID && apresentacaoID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' " +
                    "ORDER BY tPreco.ID";

            }
            else if (setorID == todosID && apresentacaoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tPreco.Nome='" + this.Nome.ValorAntigo + "' " +
                    "ORDER BY tPreco.ID";


            }
            else if (setorID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tApresentacao.ID=" + apresentacaoID + " " +
                    "ORDER BY tPreco.ID";

            }
            else if (setorID == todosID && apresentacaoID != todosID)
            {
                sql = @"SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor 
                FROM tPreco (NOLOCK) INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID 
                INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID 
                WHERE tApresentacao.EventoID=" + eventoID + @" AND tApresentacao.DisponivelAjuste='T' AND tApresentacao.ID=" + apresentacaoID +
                @" AND tPreco.Nome='" + this.Nome.ValorAntigo + @"' ORDER BY tPreco.ID";
            }
            else if (setorID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tPreco.Nome='" + this.Nome.ValorAntigo + "' " +
                    "ORDER BY tPreco.ID";

            }
            else if (apresentacaoID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "INNER JOIN tSetor ON tSetor.ID = tApresentacaoSetor.SetorID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tSetor.ID=" + setorID + " " +
                    "ORDER BY tPreco.ID";

            }
            else if (apresentacaoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tSetor.ID=" + setorID + " AND tPreco.Nome='" + this.Nome.ValorAntigo + "' " +
                    "ORDER BY tPreco.ID";

            }
            else if (precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID, tPreco.ApresentacaoSetorID, tPreco.Valor " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.ID=" + apresentacaoID + " AND tApresentacao.DisponivelAjuste='T' AND tSetor.ID=" + setorID + " " +
                    "ORDER BY tPreco.ID";
            }

            if (sql != string.Empty)
            {
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    int precoIDTemp = bd.LerInt("PrecoID");
                    int apresentacaoSetorIDTemp = bd.LerInt("ApresentacaoSetorID");
                    decimal valorTemp = bd.LerDecimal("Valor");

                    if (precoIDTemp != 0 && apresentacaoSetorIDTemp != 0)
                    {
                        DataRow linha = tabela.NewRow();
                        linha["PrecoID"] = precoIDTemp;
                        linha["ApresentacaoSetorID"] = apresentacaoSetorIDTemp;
                        linha["Valor"] = valorTemp;
                        tabela.Rows.Add(linha);
                    }

                }//while (bd.Consulta().Read())
                bd.Consulta().Close();

            }
            else
            {

                //				//valor não pode, tem q pegar do banco de dados!
                //				sql = "SELECT Valor FROM tPreco WHERE ID="+this.Control.ID;
                //				object ret = bd.ConsultaValor(sql);
                //				bd.Consulta().Close();
                //				decimal valor = (ret!=null) ? (decimal)ret : 0;

                //nao precisa pegar do banco de dados.. pode pegar de this.Valor.ValorAntigo

                DataRow linha = tabela.NewRow();
                linha["PrecoID"] = this.Control.ID;
                linha["ApresentacaoSetorID"] = this.ApresentacaoSetorID.Valor;
                linha["Valor"] = this.Valor.ValorAntigo;
                tabela.Rows.Add(linha);

            }

            return tabela;

        }

        public DataTable precosPorSetorApresentacao(int apresentacaoSetorID, int idLote)
        {
            try
            {
                DataTable dtPreco = new DataTable("dtPreco");
                dtPreco.Columns.Add("ID");
                dtPreco.Columns.Add("Nome do Preço");
                dtPreco.Columns.Add("Valor");
                dtPreco.Columns.Add("Lote Relacionado");
                dtPreco.Columns.Add("NomeValor");

                List<SqlParameter> parametros = new List<SqlParameter>();

                string sql = @"SELECT tPreco.ID, tPreco.Nome AS PrecoNome, tPreco.Valor, tLote.Nome AS LoteNome 
                                FROM tPreco (NOLOCK) 
                                LEFT JOIN tLote (NOLOCK) ON tLote.ID = tPreco.LoteID
                                LEFT JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID =  tPreco.ApresentacaoSetorID
                            WHERE tApresentacaoSetor.ID = @apresentacaoSetorID AND tPreco.ID NOT IN (SELECT PrecoID FROM tPacoteItem)";

                parametros.Add(new SqlParameter() { ParameterName = "@apresentacaoSetorID", Value = apresentacaoSetorID, DbType = System.Data.DbType.Int32 });

                if (idLote > 0)
                {
                    sql += "AND tLote.ID = @LoteId";
                    parametros.Add(new SqlParameter() { ParameterName = "@LoteId", Value = idLote, DbType = System.Data.DbType.Int32 });

                }

                CTLib.BD bd = new BD();

                bd.Consulta(sql, parametros);


                while (bd.Consulta().Read())
                {
                    DataRow row = dtPreco.NewRow();
                    row["ID"] = bd.LerInt("ID");
                    row["Nome do Preço"] = bd.LerString("PrecoNome");
                    row["Valor"] = bd.LerDecimal("Valor");
                    row["Lote Relacionado"] = bd.LerString("LoteNome");
                    row["NomeValor"] = bd.LerString("PrecoNome") + " (R$ " + bd.LerDecimal("Valor").ToString() + ")";
                    dtPreco.Rows.Add(row);
                }

                return dtPreco;
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

        public DataTable precosPorSetorApresentacaoLote(int apresentacaoSetorID, int loteID, string filtraNome = null)
        {
            try
            {
                DataTable dtPreco = new DataTable("dtPreco");
                dtPreco.Columns.Add("ID");
                dtPreco.Columns.Add("Nome");
                dtPreco.Columns.Add("Valor");
                dtPreco.Columns.Add("NomeValor");
                dtPreco.Columns.Add("LoteID");

                string sql = String.Format(@"SELECT tPreco.ID, tPreco.Nome AS PrecoNome, tPreco.Valor, tPreco.LoteID
                                FROM tPreco (NOLOCK) 
                                LEFT JOIN tLote (NOLOCK) ON tLote.ID = tPreco.LoteID
                            WHERE tPreco.ApresentacaoSetorID = {0} AND (tPreco.ParceiroMidiaID = 0 OR tPreco.ParceiroMidiaID IS NULL) AND tPreco.ID NOT IN (SELECT PrecoID FROM tPacoteItem)", apresentacaoSetorID);

                if (!String.IsNullOrEmpty(filtraNome))
                {
                    sql += String.Format(" AND tPreco.Nome LIKE '%{0}%'", filtraNome);
                }

                if (loteID > 0)
                {
                    sql += String.Format(" AND (tLote.ID = {0} OR tLote.ID IS NULL)", loteID);
                }
                else
                {
                    sql += " AND tLote.ID IS NULL";
                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow row = dtPreco.NewRow();
                    row["ID"] = bd.LerInt("ID");
                    row["Nome"] = bd.LerString("PrecoNome");
                    row["Valor"] = bd.LerDecimal("Valor");
                    row["NomeValor"] = bd.LerString("PrecoNome") + " (R$ " + bd.LerDecimal("Valor").ToString() + ")";
                    row["LoteID"] = bd.LerInt("LoteID");
                    dtPreco.Rows.Add(row);
                }

                return dtPreco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PrecosPorLote(int loteID)
        {
            try
            {
                DataTable dtPreco = new DataTable("dtPreco");
                dtPreco.Columns.Add("ID");
                dtPreco.Columns.Add("Nome");
                dtPreco.Columns.Add("Valor");
                dtPreco.Columns.Add("NomeValor");
                dtPreco.Columns.Add("LoteID");

                string sql = String.Format(@"SELECT ID, Nome AS PrecoNome, Valor, LoteID
                                             FROM tPreco (NOLOCK) 
                                             WHERE LoteID = {0}", loteID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow row = dtPreco.NewRow();
                    row["ID"] = bd.LerInt("ID");
                    row["Nome"] = bd.LerString("PrecoNome");
                    row["Valor"] = bd.LerDecimal("Valor");
                    row["NomeValor"] = bd.LerString("PrecoNome") + " (R$ " + bd.LerDecimal("Valor").ToString() + ")";
                    row["LoteID"] = bd.LerInt("LoteID");
                    dtPreco.Rows.Add(row);
                }

                return dtPreco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable carregaTaxaConveniencia(int canalId, int eventoId)
        {
            if (canalId > 0 && eventoId > 0)
            {
                try
                {
                    string strQuery = @"SELECT 
                                            TaxaConveniencia
                                            ,TaxaMinima
                                            ,TaxaMaxima
                                        FROM 
                                            tCAnalEvento (NOLOCK) 
                                        WHERE 
                                            CanalID = @CanalID 
                                            AND EventoID = @EventoID ";

                    List<SqlParameter> parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter() { ParameterName = "@CanalID", Value = canalId, DbType = DbType.Int32 });
                    parametros.Add(new SqlParameter() { ParameterName = "@EventoID", Value = eventoId, DbType = DbType.Int32 });

                    CTLib.BD bd = new BD();
                    return bd.QueryToTable(strQuery, parametros);
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
            else
                return null;
        }

        /// <summary>
        /// Atualiza preco(s)
        /// </summary>
        /// <returns></returns>
        public void Atualizar(int eventoID, int setorID, int apresentacaoID, int precoID,
                              int todosID, int[] canaisSelecionados, string canaisNaoSelecionados)
        {
            try
            {
                int apresentacaoSetorIDaux = 0;
                bool recarregarAPS = false;

                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(this.Control.UsuarioID);
                Preco preco = new Preco(this.Control.UsuarioID);
                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);

                DataTable tabelaPrecosAlteraveis = precosAlteraveis(eventoID, setorID, apresentacaoID, precoID, todosID);

                //BD bd = new BD(); ..para que criar outra conexao aqui

                Array precosIDs = Array.CreateInstance(typeof(DataRow), tabelaPrecosAlteraveis.Rows.Count);

                tabelaPrecosAlteraveis.Rows.CopyTo(precosIDs, 0);

                string precos = Utilitario.ArrayToString(precosIDs);

                bool alterouValor = (this.Valor.Valor != this.Valor.ValorAntigo);

                string sqlEmpresa = "SELECT tEmpresa.ID AS EmpresaID, tCanal.ID AS CanalID " +
                    "FROM tEmpresa,tCanal " +
                    "WHERE tEmpresa.ID=tCanal.EmpresaID AND " +
                    "tEmpresa.EmpresaVende='T' AND tEmpresa.EmpresaPromove='F'";
                bd.Consulta(sqlEmpresa);

                ArrayList listaCanais = new ArrayList();

                while (bd.Consulta().Read())
                {
                    int canalID = (int)bd.LerInt("CanalID");
                    listaCanais.Add(canalID);
                }

                listaCanais.TrimToSize();

                bd.Consulta().Close();

                bd.IniciarTransacao();

                if (!string.IsNullOrEmpty(canaisNaoSelecionados))
                {
                    List<int> IDPrecoSelecionado = new List<int>();

                    string sql = "Select ID FROM tCanalPreco WHERE PrecoID in (" + precos + ") AND CanalID in (" + canaisNaoSelecionados + ")";

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                        IDPrecoSelecionado.Add(bd.LerInt("ID"));

                    bd.Fechar();

                    foreach (int CanalPrecoID in IDPrecoSelecionado)
                        canalPreco.Excluir(CanalPrecoID);
                }

                //mudar as propriedades do preco e seus canais
                foreach (DataRow precoAlteravel in tabelaPrecosAlteraveis.Rows)
                {
                    //atualiza preço a preço
                    preco.Limpar();
                    preco.Control.ID = (int)precoAlteravel["PrecoID"];

                    foreach (int canalID in canaisSelecionados)
                    {
                        canalPreco.Limpar();

                        string sql = "SELECT 1 FROM tCanalPreco (NOLOCK) WHERE PrecoID=" + preco.Control.ID + " AND CanalID=" + canalID;

                        bd.Consulta(sql);

                        if (!bd.Consulta().Read())
                        {
                            bd.Consulta().Close();
                            canalPreco.PrecoID.Valor = preco.Control.ID;
                            canalPreco.CanalID.Valor = canalID;
                            canalPreco.Inserir(bd, true);
                        }
                        else
                            bd.Consulta().Close();
                    }

                    decimal valorBanco = (decimal)precoAlteravel["Valor"];

                    bool valorOk = (valorBanco == this.Valor.ValorAntigo);

                    if ((precoID != todosID) && valorOk)
                    {

                        if (alterouValor)
                        { //se alterou valor, verificar se pode alterar valor
                            try
                            {
                                //verificar se há ingressos vendidos com esse preço.
                                string sql = "SELECT top 1 PrecoID FROM tIngresso (NOLOCK) WHERE PrecoID=" + preco.Control.ID;
                                bd.Consulta(sql);
                                if (!bd.Consulta().Read()) //pode editar valor
                                    preco.Valor.Valor = this.Valor.Valor; //alterar valor
                                else
                                    preco.Valor.Valor = (decimal)precoAlteravel["Valor"]; //manter o valor atual
                                bd.Consulta().Close();
                            }
                            catch
                            {
                                preco.Valor.Valor = (decimal)precoAlteravel["Valor"]; //manter o valor atual
                            }
                        }
                        else
                            preco.Valor.Valor = (decimal)precoAlteravel["Valor"]; //manter o valor atual

                        if (apresentacaoSetorIDaux != (int)precoAlteravel["ApresentacaoSetorID"])
                        {
                            recarregarAPS = true;
                            apresentacaoSetorIDaux = (int)precoAlteravel["ApresentacaoSetorID"];
                        }
                        else
                            recarregarAPS = false;

                        preco.ApresentacaoSetorID.Valor = (int)precoAlteravel["ApresentacaoSetorID"];
                        preco.Nome.Valor = this.Nome.Valor;
                        preco.CorID.Valor = this.CorID.Valor;
                        preco.Quantidade.Valor = this.Quantidade.Valor;
                        preco.QuantidadePorCliente.Valor = this.QuantidadePorCliente.Valor;
                        preco.Impressao.Valor = this.Impressao.Valor;
                        preco.ImprimirCarimbo.Valor = this.ImprimirCarimbo.Valor;
                        preco.CarimboTexto1.Valor = this.CarimboTexto1.Valor;
                        preco.CarimboTexto2.Valor = this.CarimboTexto2.Valor;

                        if (this.PrecoTipoID.Valor > 0)
                            preco.PrecoTipoID.Valor = this.PrecoTipoID.Valor;

                        preco.Atualizar(bd);
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
                bd.Cnn.Dispose();
            }

        }

        /// <summary>
        /// Atualiza preco(s)
        /// </summary>
        /// <returns></returns>
        public void AtualizarCanais(int[] canaisSelecionados, int[] canaisNaoSelecionados)
        {

            try
            {

                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(this.Control.UsuarioID);
                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);

                string sqlEmpresa = "SELECT tEmpresa.ID AS EmpresaID, tCanal.ID AS CanalID " +
                    "FROM tEmpresa,tCanal " +
                    "WHERE tEmpresa.ID=tCanal.EmpresaID AND " +
                    "tEmpresa.EmpresaVende='T' AND tEmpresa.EmpresaPromove='F'";
                bd.Consulta(sqlEmpresa);

                ArrayList listaCanais = new ArrayList();

                while (bd.Consulta().Read())
                {
                    int canalID = (int)bd.LerInt("CanalID");
                    listaCanais.Add(canalID);
                }

                listaCanais.TrimToSize();

                bd.Consulta().Close();

                //bd.IniciarTransacao();

                if (canaisNaoSelecionados.Length > 0)
                {
                    List<int> IDPrecoSelecionado = new List<int>();

                    string sql = "Select ID FROM tCanalPreco WHERE PrecoID = " + this.Control.ID + " AND CanalID in (" + String.Join(",", canaisNaoSelecionados) + ")";

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                        IDPrecoSelecionado.Add(bd.LerInt("ID"));

                    bd.Fechar();

                    foreach (int CanalPrecoID in IDPrecoSelecionado)
                        canalPreco.Excluir(CanalPrecoID);
                }

                foreach (int canalID in canaisSelecionados)
                {
                    canalPreco.Limpar();

                    string sql = "SELECT 1 FROM tCanalPreco (NOLOCK) WHERE PrecoID=" + this.Control.ID + " AND CanalID=" + canalID;

                    bd.Consulta(sql);

                    if (!bd.Consulta().Read())
                    {
                        canalPreco.PrecoID.Valor = this.Control.ID;
                        canalPreco.CanalID.Valor = canalID;
                        canalPreco.Inserir(bd, true);
                    }

                    bd.Consulta().Close();
                }
                //bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                //bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Cnn.Dispose();
            }

        }

        public static int DistribuirParaCanaisIR(BD bd, int precoID)
        {
            bd.ExecutarComParametros(@"
                            INSERT INTO tCanalPreco(CanalID,PrecoID,DataInicio)
                            SELECT CanalID, @PrecoID, '' FROM vwCanaisIR", new SqlParameter("@PrecoID", precoID));

            return Convert.ToInt32(bd.ConsultaValor("SELECT @@ROWCOUNT"));
        }

        public static int DistribuirParaCanaisProprios(BD bd, int precoID, int empresaID)
        {
            bd.ExecutarComParametros(@"
                            INSERT INTO tCanalPreco(CanalID,PrecoID,DataInicio)
                            SELECT CanalID, @PrecoID, '' FROM vwCanaisProprios WHERE EmpresaID = @EmpresaID",
                                                                                                        new SqlParameter("@PrecoID", precoID),
                                                                                                        new SqlParameter("@EmpresaID", empresaID));

            return Convert.ToInt32(bd.ConsultaValor("SELECT @@ROWCOUNT"));
        }


        /// <summary>
        /// Distribui esse preço nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        public bool DistribuirCanaisIR(ArrayList canais, CTLib.BD database, int pEventoID)
        {
            try
            {
                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);

                foreach (Evento.CanalDistribuicao canalInfo in canais)
                {
                    canalPreco.CanalID.Valor = canalInfo.CanalID;
                    canalPreco.PrecoID.Valor = this.Control.ID;
                    canalPreco.DataInicio.Limpar();
                    canalPreco.DataFim.Limpar();
                    canalPreco.Quantidade.Valor = 0;

                    if (!canalPreco.Inserir(database, false))
                        throw new PrecoException("Falha ao distribuir os preços para IR");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Distribui esse preco nos canais da Ingresso Rapido (Devolve o sucesso da operacao)
        /// </summary>
        public override bool AtualizarCanaisIR()
        {
            throw new PrecoException("O AtualizarCanaisIR será descartado.");
        }


        /// <summary>
        /// Inserir esse preco nos canais da Ingresso Rapido (Devolve o sucesso da operacao)
        /// </summary>
        public bool InserirCanaisIR(CTLib.BD bd)
        {

            bool ok = true;

            try
            {

                string sql = "UPDATE tPreco SET IRVende='T' " +
                    "WHERE ID=" + this.Control.ID;
                bd.Executar(sql);

                BD bdTemp = new BD();
                string sqlEmpresa = @"SELECT tEmpresa.ID AS EmpresaID, tCanal.ID AS CanalID
                        FROM tEmpresa (NOLOCK),tCanal (NOLOCK)
                        WHERE tEmpresa.ID=tCanal.EmpresaID AND tEmpresa.EmpresaVende='T' AND tEmpresa.EmpresaPromove='F'";
                bdTemp.Consulta(sqlEmpresa);

                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);
                canalPreco.DataInicio.Limpar();
                canalPreco.DataFim.Limpar();
                canalPreco.Quantidade.Valor = 0;
                canalPreco.PrecoID.Valor = this.Control.ID;

                while (bdTemp.Consulta().Read())
                {
                    canalPreco.CanalID.Valor = (int)bdTemp.LerInt("CanalID"); ;
                    ok &= canalPreco.Inserir(bd, false);
                }

                bdTemp.Consulta().Close();
            }
            catch
            {
                ok = false;
            }

            return ok;

        }

        /// <summary>		
        /// Obtem a quantidade de ingressos vendidos ou reservados com esse preço. Se nao foi vendido nenhum ingresso, retorna zero.
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeVendido()
        {

            try
            {

                string sql = "SELECT Count(ID) AS Qtde " +
                    "FROM tIngresso " +
                    "WHERE PrecoID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                int qtde = (obj != null) ? (int)obj : 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui os itens de pacote deste preço e esse preço
        /// </summary>
        /// <returns>Informa se a exclusão ocorreu com sucesso</returns>
        public override bool ExcluirCascata()
        {
            bool excluiuTodosItens = true;
            bool excluiuTudo = false;
            try
            {
                // Excluir todos itens de pacote deste preço
                PacoteItemLista pacoteItemLista = new PacoteItemLista();
                pacoteItemLista.FiltroSQL = "PrecoID = " + this.Control.ID;
                pacoteItemLista.Carregar();
                pacoteItemLista.Primeiro();
                int contador = 0;
                if (pacoteItemLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodosItens = excluiuTodosItens && pacoteItemLista.PacoteItem.Excluir();
                        contador++;
                        pacoteItemLista.Proximo();
                    } while (excluiuTodosItens && contador < pacoteItemLista.Tamanho);
                }
                // Excluir este preço
                if (excluiuTodosItens)
                    excluiuTudo = this.Excluir();
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

        /// <summary>
        /// Vendas Gerenciais por preco com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total
        /// Com porcentagem
        /// </summary>
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
                    string horario = linha["VariacaoLinha"].ToString().Substring(0, 12);
                    string restante = linha["VariacaoLinha"].ToString().Substring(14); // ignore os segundos

                    linha["VariacaoLinha"] = Utilitario.DataHoraBDParaDataHoraLegivel(horario) + restante;
                }
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // preco zero pega todos
                int totaisVendidos = QuantidadeIngressosPorPreco(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorPreco(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorPreco(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorPreco(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada preco na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorPreco(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorPreco(ingressoLogIDsCancelados);
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
                    linha["R$ Vend"] = ValorIngressosPorPreco(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorPreco(ingressoLogIDsCancelados);
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
                tabela.Columns["VariacaoLinha"].ColumnName = "Preco";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Precos por definido pelos IngressoLogIDs
        /// </summary>
        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != string.Empty)
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tApresentacao.Horario + ' - ' + tSetor.Nome + ' - ' + tPreco.Nome AS HorarioSetorPreco, tPreco.ID AS PrecoID " +
                        "FROM            tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " +
                        "ORDER BY tApresentacao.Horario + ' - ' + tSetor.Nome + ' - ' + tPreco.Nome DESC";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("PrecoID");
                        linha["VariacaoLinha"] = obterDados.LerString("HorarioSetorPreco");
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
        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorPreco(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != string.Empty)
                {
                    // Trantando a condição
                    string condicaoPreco = string.Empty;
                    if (this.Control.ID > 0)
                        condicaoPreco = "AND (tPreco.ID = " + this.Control.ID + ") ";
                    else
                        condicaoPreco = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tPreco.Valor) AS QuantidadeIngressos " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoPreco;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de preco
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos precos
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
        } // fim de QuantidadeIngressosPorPreco
        /// <summary>
        /// Obter valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorPreco(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != string.Empty)
                {
                    string condicaoPreco = string.Empty;
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoPreco = "AND (tPreco.ID = " + this.Control.ID + ") ";
                    else
                        condicaoPreco = " "; // todos se for = zero
                    string sql;
                    sql =
                        "SELECT   SUM(tPreco.Valor) AS Valor " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID INNER JOIN " +
                        "tApresentacao ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoPreco;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do preco
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos precos
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
        } // fim de ValorIngressosPorPreco

        /// <summary>
        /// Inserir novo(a) Preco (Usuado na tela Preço)
        /// </summary>
        /// <returns></returns>	
        public int Inserir(int eventoID, int setorID, int[] iHorarios, int todosID, bool codigoBarraEstruturado, int[] canaisSelecionados)
        {
            BD bd = new BD();

            try
            {
                ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor(this.Control.UsuarioID);
                int qtde = 0; //quantidade de precos incluidos

                ArrayList setoresIDs = new ArrayList();

                BD bdTemp = new BD();

                if (setorID == todosID)
                {

                    string sql = "SELECT DISTINCT tApresentacaoSetor.SetorID " +
                        "FROM tApresentacaoSetor " +
                        "INNER JOIN tApresentacao ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                        "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' ";
                    bdTemp.Consulta(sql);

                    while (bdTemp.Consulta().Read())
                        setoresIDs.Add(bdTemp.LerInt("SetorID"));

                    bdTemp.Fechar();
                }
                else
                    setoresIDs.Add(setorID);

                setoresIDs.TrimToSize();

                bd.IniciarTransacao();

                string sSetor = Utilitario.ArrayToString(setoresIDs);
                string sApresentacao = Utilitario.ArrayToString(iHorarios);

                string sqlConsulta = @"SELECT tApresentacaoSetor.ID AS ApresentacaoSetorID, tApresentacaoSetor.SetorID, 
                        tApresentacaoSetor.ApresentacaoID  
                        FROM tApresentacaoSetor (NOLOCK)
                        RIGHT JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID=tSetor.ID 
                        RIGHT JOIN tApresentacao (NOLOCK) ON tApresentacaoSetor.ApresentacaoID=tApresentacao.ID
                        WHERE tApresentacao.ID in (" + sApresentacao + @") AND tApresentacao.DisponivelAjuste='T' 
                        AND tSetor.ID in (" + sSetor + ")";
                bdTemp.Consulta(sqlConsulta);

                while (bdTemp.Consulta().Read())
                {
                    int setorIDTemp = (int)bdTemp.LerInt("SetorID");
                    int apresentacaoIDTemp = (int)bdTemp.LerInt("ApresentacaoID");
                    int apresentacaoSetorIDTemp = (int)bdTemp.LerInt("ApresentacaoSetorID");

                    if (apresentacaoSetorIDTemp != 0)
                    { //nao eh null, pode incluir
                        this.ApresentacaoSetorID.Valor = apresentacaoSetorIDTemp;
                        if (this.Inserir(eventoID, setorIDTemp, apresentacaoIDTemp, codigoBarraEstruturado, canaisSelecionados, bd))
                        {
                            qtde++;
                        }
                    }

                }//while (bd.Consulta().Read())
                bdTemp.Fechar();


                bd.FinalizarTransacao();

                return qtde;

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
            }

        }

        /// <summary>
        /// Inserir novo(a) Preco (Usuado na classe Evento)
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(int eventoID, int setorID, int apresentacaoID, bool codigoBarraEstruturado, BD bd)
        {
            return this.inserir(eventoID, setorID, apresentacaoID, codigoBarraEstruturado, new int[] { }, bd);
        }

        public bool Inserir(int eventoID, int setorID, int apresentacaoID, bool codigoBarraEstruturado, int[] canaisSelecionados, BD bd)
        {
            return this.inserir(eventoID, setorID, apresentacaoID, codigoBarraEstruturado, canaisSelecionados, bd);
        }

        /// <summary>
        /// Inserir novo(a) Preco
        /// </summary>
        /// <returns></returns>	
        private bool inserir(int eventoID, int setorID, int apresentacaoID, bool codigoBarraEstruturado, int[] canaisSelecionados, BD bd)
        {
            try
            {
                bool ok = this.Inserir(bd);

                if (ok)
                {
                    if (codigoBarraEstruturado)
                    {
                        CodigoBarra codigoBarra = new CodigoBarra();
                        codigoBarra.Inserir(eventoID, apresentacaoID, setorID, this.Control.ID, bd);
                    }

                    if (canaisSelecionados.Length == 0)
                        return ok;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override bool Inserir()
        {
            return base.Inserir();
        }


        /// <summary>
        /// Classe utilizada para o agrupamento de informações relacionadas ao preço para envio à reserva.
        /// </summary>
        public class InfoToReserva
        {
            private Preco preco;
            private int qtdVendidoPreco = 0;
            private int qtdVendidoCanal = 0;
            private int qtdDisponivelCanal = 0; // Disponível para o canal e não ainda disponível para o canal.
            private int validadeCanal = 0; // Validade do preço para o canal. Corresponde ao número de dias restante.
            private DateTime dataFim;

            public bool DisponivelSerie { get; set; }
            public int CanalPrecoID { get; set; }

            /// <summary>
            /// Informações sobre o preço.
            /// </summary>
            public Preco Preco
            {
                get { return this.preco; }
                set { this.preco = value; }
            }

            /// <summary>
            /// Quantidade vendida ou reservada do preço.
            /// </summary>
            public int QtdVendidoPreco
            {
                get { return this.qtdVendidoPreco; }
                set { this.qtdVendidoPreco = value; }
            }
            /// <summary>
            /// Quantidade vendida ou reservada do Canal.
            /// </summary>
            public int QtdVendidoCanal
            {
                get { return qtdVendidoCanal; }
                set { qtdVendidoCanal = value; }
            }

            /// <summary>
            /// Quantidade disponibilizada para o Canal.
            /// OBS.: Não é o que o canal ainda pode vender (permitido - vendidos) e sim o que foi permitido na criação do preço.
            /// </summary>
            public int QtdDisponivelCanal
            {
                get { return this.qtdDisponivelCanal; }
                set { this.qtdDisponivelCanal = value; }
            }

            /// <summary>
            /// Quantidade de dias restantes para que o preço seja desativado do canal.
            /// </summary>
            public int ValidadeCanal
            {
                get { return this.validadeCanal; }
                set { this.validadeCanal = value; }
            }

            /// <summary>
            /// Data em que o preço expirará para o canal em questão.
            /// </summary>
            public DateTime DataFim
            {
                get { return this.dataFim; }
                set { this.dataFim = value; }
            }

        }

        /// <summary>
        /// Método responsável por efetuar uma busca específica para a Reserva de ingressos.
        /// - Quantidade do preço (tPreco)
        /// - Quantidade por cliente (tPreco)
        /// - Quantidade vendido por preco(tIngresso)
        /// - Quantidade vendido por canal(tIngresso)
        /// - Quantidade disponível para o canal (tCanalPreco)
        /// - Validade do preço para o Canal (tCanalPreco)
        /// - Data em que o preço deverá expirar para o Canal (tCanalPreco)
        /// </summary>
        /// <param name="precoID">ID do preço</param>
        /// <param name="database">Objeto DB. Utilizado para evitar abertura de várias conexões.</param>
        /// <returns>Retorna um InfoToReserva contendo toda a informação necessária para a reserva.</returns>
        public InfoToReserva LerToReserva(int precoID, int canalID, int serieID, CTLib.BD database)
        {
            try
            {

                //TODO: Monitorar essa SELECT. Ela não poderá de forma alguma demorar.

                // Verifica se o preço existe e busca a qtd vendida.
                // Elimina necessidade de executar o método Ler e depois uma select específica para busca de Qtd vendida

                string sql = string.Format("EXEC LerToReserva2011 {0}, {1}, {2}", precoID.ToString(), canalID.ToString(), serieID);

                database.Consulta(sql);

                this.Limpar(); // Limpa o objeto para que o mesmo seja "repopulado".

                InfoToReserva info = new InfoToReserva();
                if (database.Consulta().Read())
                {
                    this.Control.ID = precoID;
                    this.Quantidade.Valor = database.LerInt("PrecoQuantidade");
                    this.QuantidadePorCliente.Valor = database.LerInt("QuantidadePorCliente");
                    this.Valor.Valor = database.LerDecimal("Valor");
                    info.Preco = this;

                    info.CanalPrecoID = database.LerInt("CanalPrecoID");
                    info.QtdDisponivelCanal = database.LerInt("CanalPrecoQuantidade");

                    DateTime dataFim = database.LerDateTime("DataFim");
                    info.DataFim = dataFim;
                    int validade;

                    //***** Verifica validade ****
                    if (dataFim != System.DateTime.MinValue)
                    { // Se possuir data de validade
                        if (dataFim >= System.DateTime.Today)
                        { // Verifica se ainda não chegou a validade
                            // Calcula número de dias restante
                            TimeSpan intervalo = dataFim - System.DateTime.Today;
                            validade = intervalo.Days + 1;
                        }
                        else
                            validade = 0; // Expirou!
                    }
                    else
                        validade = -1; // Nunca expira.
                    //***** Fim verificação ****
                    info.ValidadeCanal = validade;

                }// Fim if database.Consulta().Read.

                if (database.Consulta().NextResult() && database.Consulta().Read())
                    info.QtdVendidoPreco = database.LerInt("QuantidadeVendidoPreco");

                if (database.Consulta().NextResult() && database.Consulta().Read())
                    info.QtdVendidoCanal = database.LerInt("QuantidadeVendidoCanal");

                if (database.Consulta().NextResult() && database.Consulta().Read())
                    info.DisponivelSerie = database.LerBoolean("Disponivel") && serieID > 0;
                else if (serieID > 0)
                    info.DisponivelSerie = false;

                database.FecharConsulta();
                return info;
            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta(); // Aqui eu posso fazer isso, pois não existem transações para reserva.
            }

        }// Fim método



        public int QuantidadeVendidoPorClienteID(int clienteID)
        {
            BD bd = new BD();
            try
            {
                object retorno = bd.ConsultaValor("SELECT COUNT(ID) FROM tIngresso (NOLOCK) WHERE ClienteID = " + clienteID + " AND PrecoID=" + this.Control.ID + " AND Status IN ('V','I','E')");

                if (retorno is int)
                    return (int)retorno;
                else
                    return 0;
            }
            catch
            { throw; }
            finally
            {
                bd.Fechar();
            }
        }

        public int QdteCliente(int[] precosID)
        {
            BD bd = new BD();
            try
            {
                object retorno = bd.ConsultaValor("SELECT MAX(QuantidadePorCliente) FROM tPreco WHERE ID IN (" + Utilitario.ArrayToString(precosID) + ")");
                if (retorno is int)
                    return (int)retorno;
                else
                    return 0;
            }
            catch
            { throw; }
            finally
            {
                bd.Fechar();
            }
        }

        public int QdteCliente(int precoID)
        {
            BD bd = new BD();
            try
            {
                object retorno = bd.ConsultaValor("SELECT MAX(QuantidadePorCliente) FROM tPreco WHERE ID = " + precoID);
                if (retorno is int)
                    return (int)retorno;
                else
                    return 0;
            }
            catch
            { throw; }
            finally
            {
                bd.Fechar();
            }
        }


        public List<EstruturaCanaisPreco> RetornaCanaisVendaIR()
        {

            List<EstruturaCanaisPreco> canaisPreco = new List<EstruturaCanaisPreco>();
            string cmdSelect = string.Empty;

            bd = new CTLib.BD();

            try
            {
                cmdSelect = "SELECT TC.[ID] AS CanalID " +
                              "FROM [dbo].[tEmpresa] TE, " +
                                   "[dbo].[tCanal] TC " +
                             "WHERE TE.[ID] = TC.[EmpresaID] " +
                               "AND TE.[EmpresaVende] = 'T' " +
                               "AND TE.[EmpresaPromove] = 'F'";

                bd.Consulta(cmdSelect);

                while (bd.Consulta().Read())
                    canaisPreco.Add(new EstruturaCanaisPreco() { CanalID = bd.LerInt("CanalID") });

                return canaisPreco;
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

        public bool TemIngressoPorPrecoID(List<int> ids)
        {
            bd = new CTLib.BD();
            string precosIds = Utilitario.ArrayToString(ids.ToArray());

            try
            {
                string sql = @"SELECT COUNT(ID) FROM tIngresso (NOLOCK) WHERE PrecoID in (" + precosIds +
                    ") AND Status IN ('V', 'I')";

                object retorno = bd.ConsultaValor(sql);
                if (retorno is int)
                    return (int)retorno > 0;
                else
                    return false;

                //bd.Consulta(sql);

                //return bd.Consulta().Read();
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
        public bool DistribuirCanaisPrecoIR(List<EstruturaCanaisPreco> canaisPreco, bool inserir = false)
        {
            try
            {
                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);

                foreach (EstruturaCanaisPreco canalInfo in canaisPreco)
                {
                    canalPreco.CanalID.Valor = canalInfo.CanalID;
                    canalPreco.PrecoID.Valor = canalInfo.PrecoID;
                    canalPreco.DataInicio.Limpar();
                    canalPreco.DataFim.Limpar();
                    canalPreco.Quantidade.Valor = 0;

                    if (!canalPreco.Inserir(inserir))
                        throw new PrecoException("Falha ao distribuir os preços para IR");
                }

                return true;
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


        public bool RemoverCanaisPrecoIR(List<int> listPrecoID, bool removeVendeSite, bool removeVendeCallCenter, EstruturaCanaisPreco.EnumAcaoCanaisPreco tipoAcao)
        {
            bool executou = false;
            bd = new BD();
            string sql = string.Empty;

            try
            {
                int canalCallCenter = Canal.CANAL_CALL_CENTER;
                int canalSite = Canal.CANAL_INTERNET;

                string rm = string.Empty;

                if (removeVendeCallCenter && tipoAcao != EstruturaCanaisPreco.EnumAcaoCanaisPreco.Remover)
                    rm = canalCallCenter.ToString();
                else if (!removeVendeCallCenter && tipoAcao == EstruturaCanaisPreco.EnumAcaoCanaisPreco.Remover)
                    rm = canalCallCenter.ToString();

                if (removeVendeSite && tipoAcao != EstruturaCanaisPreco.EnumAcaoCanaisPreco.Remover)
                    rm += rm.Length > 0 ? ", " + canalSite.ToString() : canalSite.ToString();
                else if (!removeVendeSite && tipoAcao == EstruturaCanaisPreco.EnumAcaoCanaisPreco.Remover)
                    rm += rm.Length > 0 ? ", " + canalSite.ToString() : canalSite.ToString();

                CanalPreco oCanalPreco = new CanalPreco();

                foreach (int precoID in listPrecoID)
                {
                    List<int> listaIdCanalPreco = new List<int>();
                    List<int> CanaisIDDeletar = new List<int>();

                    sql = string.Format(@"SELECT cp.ID FROM tCanalPreco cp (NOLOCK)
                    INNER JOIN tCanal c (NOLOCK) ON cp.CanalID = c.ID
                    INNER JOIN tEmpresa e (NOLOCK) ON c.EmpresaID = e.ID
                    WHERE e.EmpresaPromove = 'F' AND e.EmpresaVende = 'T' AND cp.PrecoID = {0} ", precoID);

                    switch (tipoAcao)
                    {
                        //Acao de Remover, deve excluir os canais CC e Internet Caso marcados
                        case EstruturaCanaisPreco.EnumAcaoCanaisPreco.Remover:
                            if (rm.Length > 0)
                                sql += string.Format(" AND CanalID NOT IN ({0}) ", rm);
                            break;
                        //Acao de Manter/Distribuir, deve Incluir somente os canais CC e Internet caso marcados
                        case EstruturaCanaisPreco.EnumAcaoCanaisPreco.Distribuir:
                        case EstruturaCanaisPreco.EnumAcaoCanaisPreco.Manter:
                            if (rm.Length > 0)
                                sql += string.Format(" AND CanalID IN ({0}) ", rm);
                            break;
                    }

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                        listaIdCanalPreco.Add(bd.LerInt("ID"));

                    bd.FecharConsulta();

                    bd.Consulta(@"SELECT DISTINCT tCanal.ID FROM tEmpresa (NOLOCK) INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID WHERE EmpresaVende = 'T' AND EmpresaPromove = 'F' AND tCanal.EmpresaID = tEmpresa.ID");

                    while (bd.Consulta().Read())
                        CanaisIDDeletar.Add(bd.LerInt("ID"));

                    foreach (int IDCanalPreco in listaIdCanalPreco)
                        oCanalPreco.Excluir(IDCanalPreco);

                    foreach (int CanalID in CanaisIDDeletar)
                    {
                        oRoboCanalPreco.CanalID.Valor = CanalID;
                        oRoboCanalPreco.PrecoID.Valor = precoID;
                        oRoboCanalPreco.Operacao.Valor = Convert.ToChar(RoboCanalPreco.operacaobanco.Deletar).ToString();
                        oRoboCanalPreco.Inserir(bd);
                    }
                }

                return executou;
            }
            catch (Exception ex)
            {
                executou = false;
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool RemoverCanaisPrecoIR(List<int> listPrecoID)
        {
            bool executou = false;
            bd = new BD();
            string sql = string.Empty;

            try
            {

                CanalPreco oCanalPreco = new CanalPreco();

                foreach (int precoID in listPrecoID)
                {
                    List<int> listaIdCanalPreco = new List<int>();
                    List<int> CanaisIDDeletar = new List<int>();

                    sql = string.Format(@"SELECT cp.ID FROM tCanalPreco cp (NOLOCK)
                    INNER JOIN tCanal c (NOLOCK) ON cp.CanalID = c.ID
                    INNER JOIN tEmpresa e (NOLOCK) ON c.EmpresaID = e.ID
                    WHERE e.EmpresaPromove = 'F' AND e.EmpresaVende = 'T' AND cp.PrecoID = {0} ", precoID);

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                        listaIdCanalPreco.Add(bd.LerInt("ID"));

                    bd.FecharConsulta();

                    bd.Consulta(@"SELECT DISTINCT tCanal.ID FROM tEmpresa (NOLOCK) INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID WHERE EmpresaVende = 'T' AND EmpresaPromove = 'F' AND tCanal.EmpresaID = tEmpresa.ID");

                    while (bd.Consulta().Read())
                        CanaisIDDeletar.Add(bd.LerInt("ID"));

                    foreach (int IDCanalPreco in listaIdCanalPreco)
                        oCanalPreco.Excluir(IDCanalPreco);

                    foreach (int CanalID in CanaisIDDeletar)
                    {
                        oRoboCanalPreco.CanalID.Valor = CanalID;
                        oRoboCanalPreco.PrecoID.Valor = precoID;
                        oRoboCanalPreco.Operacao.Valor = Convert.ToChar(RoboCanalPreco.operacaobanco.Deletar).ToString();
                        oRoboCanalPreco.Inserir(bd);
                    }
                }

                return executou;
            }
            catch (Exception ex)
            {
                executou = false;
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaPreco> CarregarPrecosAssinatura(List<int> ApresentacoesID, List<int> SetoresID)
        {
            try
            {
                string apresentacao = Utilitario.ArrayToString(ApresentacoesID.ToArray());
                string setores = Utilitario.ArrayToString(SetoresID.ToArray());

                List<EstruturaAssinaturaPreco> retorno = new List<EstruturaAssinaturaPreco>();

                string sql = @"SELECT  COUNT(DISTINCT SetorID) AS qtdSetores , COUNT(DISTINCT ApresentacaoID) AS qtdApresentacoes ,
                    ISNULL(tPreco.PrecoTipoID,0) as PrecoTipoID, tPrecoTipo.Nome
                    FROM tPreco (NOLOCK) 
                    INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID
                    LEFT JOIN tPrecoTipo (NOLOCK)  ON tPreco.PrecoTipoID = tPrecoTipo.ID 
                    WHERE ApresentacaoID IN (" + apresentacao + ") AND SetorID IN (" + setores + @") 
                    GROUP BY tPreco.PrecoTipoID, tPrecoTipo.Nome ORDER BY tPrecoTipo.Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    int precoTipoID = bd.LerInt("PrecoTipoID");
                    if (precoTipoID > 0)
                    {
                        retorno.Add(new EstruturaAssinaturaPreco
                        {
                            ID = precoTipoID,
                            Nome = bd.LerString("Nome"),
                            QtdApresentacao = bd.LerInt("qtdApresentacoes"),
                            QtdSetores = bd.LerInt("qtdSetores"),
                            Incluir = false
                        });
                    }
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

        public List<EstruturaAssinaturaPreco> CarregarPrecosAssinatura(List<int> ApresentacoesID, List<int> SetoresID, int AssinaturaID, int Ano)
        {
            try
            {
                string apresentacao = Utilitario.ArrayToString(ApresentacoesID.ToArray());
                string setores = Utilitario.ArrayToString(SetoresID.ToArray());

                List<EstruturaAssinaturaPreco> retorno = new List<EstruturaAssinaturaPreco>();

                string sql = @"SELECT  COUNT(DISTINCT tApresentacaoSetor.SetorID) AS qtdSetores , COUNT(DISTINCT tApresentacaoSetor.ApresentacaoID) AS qtdApresentacoes ,
                    tPreco.PrecoTipoID, tPrecoTipo.Nome
                    ,CASE WHEN COUNT(tAssinaturaAno.AssinaturaID) > 0
                    THEN 'T'
                    ELSE 'F'
                    END as Incluir
                    FROM tPreco (NOLOCK) 
                    INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID
                    INNER JOIN tPrecoTipo (NOLOCK)  ON tPreco.PrecoTipoID = tPrecoTipo.ID 
                    LEFT JOIN tAssinaturaItem (nolock) ON tPreco.PrecoTipoID = tAssinaturaItem.PrecoTipoID
					LEFT JOIN tAssinaturaAno (nolock) ON tAssinaturaAno.ID  = tAssinaturaItem.AssinaturaAnoID 
					and tAssinaturaAno.Ano = " + Ano + @" and tAssinaturaAno.AssinaturaID = " + AssinaturaID + @" 
                    WHERE tApresentacaoSetor.ApresentacaoID IN (" + apresentacao + ") AND tApresentacaoSetor.SetorID IN (" + setores + @") 
                    GROUP BY tPreco.PrecoTipoID, tPrecoTipo.Nome ORDER BY tPrecoTipo.Nome ";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaAssinaturaPreco
                    {
                        ID = bd.LerInt("PrecoTipoID"),
                        Nome = bd.LerString("Nome"),
                        QtdApresentacao = bd.LerInt("qtdApresentacoes"),
                        QtdSetores = bd.LerInt("qtdSetores"),
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

        public DataTable PrecosAfetados(int cotaID, List<int> Apresentacoes)
        {
            try
            {
                string apres = string.Join(",", Apresentacoes.ToArray());

                DataTable tabela = new DataTable();

                tabela.Columns.Add("NomePreco", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("ID", typeof(int));

                string sql = @"SELECT DISTINCT tPreco.ID,tPreco.Nome, tCotaItem.PrecoIniciaCom FROM tPreco (NOLOCK)
                                    INNER JOIN tApresentacaoSetor (NOLOCK) ON tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID
                                    INNER JOIN tCotaItem (NOLOCK) ON RTRIM(LTRIM(tPreco.Nome)) LIKE RTrim(LTRIM(tCotaItem.PrecoIniciaCom)) + '%' 
                                    WHERE tApresentacaoSetor.ApresentacaoID IN (" + apres + ") AND tCotaItem.CotaID = @CotaID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@CotaID", Value = cotaID, DbType = DbType.Int32 });

                bd.Consulta(sql, parametros);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();

                    linha["NomePreco"] = bd.LerString("Nome");
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    linha["ID"] = bd.LerInt("ID");
                    tabela.Rows.Add(linha);
                }

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

        public DataTable PrecosAfetados(int cotaID, Dictionary<int, string> TipoRegra)
        {
            try
            {
                DataTable tabela = new DataTable();

                tabela.Columns.Add("NomePreco", typeof(string));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));

                string sql = @"SELECT Tipo, PrecoIniciaCom
                                FROM tCotaItem (nolock)
                                WHERE tCotaItem.CotaID = @CotaID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter() { ParameterName = "@CotaID", Value = cotaID, DbType = DbType.Int32 });

                bd.Consulta(sql, parametros);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();

                    linha["NomePreco"] = TipoRegra.FirstOrDefault(o => o.Key == bd.LerInt("Tipo")).Value;
                    linha["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");

                    tabela.Rows.Add(linha);
                }

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

        public bool ValidarPrecoLoteAtivo(int precoID, int quantidade)
        {
            try
            {
                string sql = @"SELECT dbo.PrecoLoteAtivo(@PrecoID,@Quantidade)";

                List<SqlParameter> parametros = new List<SqlParameter>();
                sql = sql.Replace("@PrecoID", precoID == 0 ? this.Control.ID.ToString() : precoID.ToString());
                sql = sql.Replace("@Quantidade", quantidade.ToString());

                return Convert.ToInt32(bd.ConsultaValor(sql)) > 0;
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

        public bool ValidarMesmoLote(int precoIDOld, int precoIDNew = 0)
        {
            try
            {
                string sql = @"SELECT dbo.PrecoMesmoLote(@PrecoIDOLD, @PrecoIDNEW)";

                List<SqlParameter> parametros = new List<SqlParameter>();
                sql = sql.Replace("@PrecoIDOLD", precoIDOld.ToString());
                sql = sql.Replace("@PrecoIDNEW", precoIDNew.ToString());

                return Convert.ToInt32(bd.ConsultaValor(sql)) > 0;
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
    } // fim da classe

    public class PrecoLista : PrecoLista_B
    {

        public PrecoLista() { }

        public PrecoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }
}
