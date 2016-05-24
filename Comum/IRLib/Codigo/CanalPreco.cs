/**************************************************
* Arquivo: CanalPreco.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib
{

    public class CanalPreco : CanalPreco_B
    {

        public CanalPreco() { }

        public CanalPreco(int usuarioIDLogado) : base(usuarioIDLogado) { }

        private RoboCanalPreco oRoboCanalPreco = new RoboCanalPreco();

        public override DataTable Precos(int canalid)
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tPreco.ID,tPreco.Nome " +
                    "FROM tPreco (NOLOCK),tCanalPreco (NOLOCK) " +
                    "WHERE DataInicio <= '" + System.DateTime.Today.ToString("yyyyMMdd") + "' AND " +
                    "tCanalPreco.PrecoID=tPreco.ID AND tCanalPreco.CanalID=" + canalid + " " +
                    "ORDER BY tPreco.Nome";

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

        public override int Validade()
        {

            try
            {

                int validade; //dias q faltam pra expirar

                string sql = "SELECT ID, DataFim " +
                    "FROM tCanalPreco (NOLOCK) " +
                    "WHERE tCanalPreco.PrecoID=" + this.PrecoID.Valor + " AND tCanalPreco.CanalID=" + this.CanalID.Valor;

                bd.Consulta(sql);

                int id;
                DateTime dataFim;

                if (bd.Consulta().Read())
                {
                    id = bd.LerInt("ID");
                    dataFim = bd.LerDateTime("DataFim");
                    bd.Fechar();
                }
                else
                {
                    bd.Fechar();
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");
                }

                if (dataFim != System.DateTime.MinValue)
                {
                    if (dataFim >= System.DateTime.Today)
                    {
                        TimeSpan intervalo = dataFim - System.DateTime.Today;
                        validade = intervalo.Days + 1;
                    }
                    else
                    {
                        this.Ler(id);
                        validade = 0;
                    }
                }
                else
                {
                    validade = -1;
                }

                return validade;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override int Validade(int canalid, int precoid)
        {

            try
            {

                int validade; //dias q faltam pra expirar

                string sql = "SELECT DataFim " +
                    "FROM tCanalPreco (NOLOCK) " +
                    "WHERE tCanalPreco.PrecoID=" + precoid + " AND tCanalPreco.CanalID=" + canalid;

                bd.Consulta(sql);

                DateTime dataFim;

                if (bd.Consulta().Read())
                {
                    dataFim = bd.LerDateTime("DataFim");
                    bd.Fechar();
                }
                else
                {
                    bd.Fechar();
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");
                }

                if (dataFim != System.DateTime.MinValue)
                {
                    if (dataFim >= System.DateTime.Today)
                    {
                        TimeSpan intervalo = dataFim - System.DateTime.Today;
                        validade = intervalo.Days + 1;
                    }
                    else
                    {
                        validade = 0;
                    }
                }
                else
                {
                    validade = -1;
                }

                return validade;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override DateTime DataValidade()
        {

            try
            {

                DateTime dataFim;

                string sql = "SELECT DataFim " +
                    "FROM tCanalPreco (NOLOCK) " +
                    "WHERE tCanalPreco.PrecoID=" + this.PrecoID.Valor + " AND tCanalPreco.CanalID=" + this.CanalID.Valor;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    dataFim = (DateTime)obj;
                else
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");

                return dataFim;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override DateTime DataValidade(int canalid, int precoid)
        {

            try
            {

                DateTime dataFim;

                string sql = "SELECT DataFim FROM tCanalPreco WHERE " +
                    "tCanalPreco.PrecoID=" + precoid + " AND tCanalPreco.CanalID=" + canalid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    dataFim = (DateTime)obj;
                else
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");

                return dataFim;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override int QuantidadeDisponivel(int canalid, int precoid)
        {

            try
            {

                int qtde;

                string sql = "SELECT ID, Quantidade FROM tCanalPreco WHERE " +
                    "tCanalPreco.PrecoID=" + precoid + " AND tCanalPreco.CanalID=" + canalid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override int QuantidadeDisponivel()
        {

            try
            {

                int qtde;

                string sql = "SELECT Quantidade FROM tCanalPreco WHERE " +
                    "tCanalPreco.PrecoID=" + this.PrecoID.Valor + " AND tCanalPreco.CanalID=" + this.CanalID.Valor;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new CanalPrecoException("O canal não está vendendo esse preço.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override DataTable Canais()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("CanalPrecoID", typeof(int));

                string sql = "SELECT c.ID,c.Nome,cp.ID AS CanalPrecoID, e.Nome AS Empresa " +
                    "FROM tCanal as c, tCanalPreco AS cp, tEmpresa AS e " +
                    "WHERE e.ID=c.EmpresaID AND c.ID=cp.CanalID AND " +
                    "cp.PrecoID=" + this.PrecoID.Valor + " ORDER BY c.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["CanalPrecoID"] = bd.LerInt("CanalPrecoID");
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

        public List<EstruturaIDNome> CanaisLista(int id)
        {

            try
            {

                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();
                EstruturaIDNome aux;

                string sql = "SELECT c.ID,c.Nome " +
                    "FROM tCanal  as c (NOLOCK), tCanalPreco AS cp(NOLOCK), tEmpresa AS e (NOLOCK)" +
                    "WHERE e.ID=c.EmpresaID AND c.ID=cp.CanalID AND " +
                    "cp.PrecoID=" + id + " ORDER BY c.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    aux = new EstruturaIDNome();
                    aux.ID = bd.LerInt("ID");
                    aux.Nome = bd.LerString("Nome");

                    retorno.Add(aux);
                }

                bd.Fechar();

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataTable precosAlteraveis(int eventoID, int apresentacaoID, int setorID, int precoID, int todosID, string precoNome)
        {

            DataTable tabela = new DataTable("Precos");

            tabela.Columns.Add("PrecoID", typeof(int));

            string sql = "";

            //REGRAS 3 listas(setor,apresentacao,preco) = 7 possibilidades
            if (setorID == todosID && apresentacaoID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' " +
                    "ORDER BY tPreco.ID";

            }
            else if (setorID == todosID && apresentacaoID == todosID && precoID != todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tPreco.Nome='" + precoNome + "' AND tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' " +
                    "ORDER BY tPreco.ID";


            }
            else if (setorID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tApresentacao.ID=" + apresentacaoID + " " +
                    "ORDER BY tPreco.ID";

            }
            else if (setorID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.ID =" + apresentacaoID + " AND  tApresentacao.EventoID=" + eventoID + " AND tPreco.Nome = '" + precoNome + "' AND tApresentacao.DisponivelAjuste='T' " +
                    "ORDER BY tPreco.ID";

            }
            else if (apresentacaoID == todosID && precoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "INNER JOIN tSetor ON tSetor.ID = tApresentacaoSetor.SetorID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tSetor.ID=" + setorID + " " +
                    "ORDER BY tPreco.ID";

            }
            else if (apresentacaoID == todosID)
            {
                sql = "SELECT DISTINCT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID " +
                    "WHERE tPreco.Nome='" + precoNome + "' AND tApresentacao.EventoID=" + eventoID + " AND tApresentacao.DisponivelAjuste='T' AND tSetor.ID=" + setorID + " " +
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
            bd = new BD();
            if (sql != "")
            {
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    int precoIDTemp = bd.LerInt("PrecoID");

                    if (precoIDTemp != 0)
                    {
                        DataRow linha = tabela.NewRow();
                        linha["PrecoID"] = precoIDTemp;
                        tabela.Rows.Add(linha);
                    }

                }//while (bd.Consulta().Read())
                bd.Consulta().Close();

            }
            else
            {

                DataRow linha = tabela.NewRow();
                linha["PrecoID"] = this.PrecoID.Valor;
                tabela.Rows.Add(linha);

            }

            return tabela;

        }

        public int[] RemoverMulti(int eventoID, int apresentacaoID, int setorID, int precoID, int todosID, string precoNome)
        {

            try
            {

                DataTable tabelaPrecosAlteraveis = precosAlteraveis(eventoID, apresentacaoID, setorID, precoID, todosID, precoNome);

                Array precosIDs = Array.CreateInstance(typeof(DataRow), tabelaPrecosAlteraveis.Rows.Count);

                tabelaPrecosAlteraveis.Rows.CopyTo(precosIDs, 0);

                string precos = Utilitario.ArrayToString(precosIDs);

                bd.IniciarTransacao();

                //deletar todos os canais do preço
                string sql = "DELETE FROM tCanalPreco WHERE PrecoID in (" + precos + ") AND CanalID=" + this.CanalID.Valor;
                int x = bd.Executar(sql);

                bd.FinalizarTransacao();
                int[] retorno = new int[2];
                retorno[0] = tabelaPrecosAlteraveis.Rows.Count;
                retorno[1] = x;
                return retorno;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
                //} finally {
                //    bd.Fechar();
                //    bd.Cnn.Dispose();
            }

        }

        public void RemoverMulti(List<int> canaisIDS)
        {
            try
            {
                string ids = String.Join(",", canaisIDS);

                using (BD bd = new BD())
                { 
                    //deletar todos os canais do preço
                    string sql = String.Format(@"DELETE FROM tCanalPreco WHERE ID IN ({0})", ids);
                    bd.Executar(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirLog(CTLib.BD database)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCanalPreco (ID, Versao, CanalID, PrecoID, DataInicio, DataFim, Quantidade) ");
                sql.Append("SELECT ID, @V, CanalID, PrecoID, DataInicio, DataFim, Quantidade FROM tCanalPreco WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                database.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Atualizar(CTLib.BD database)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCanalPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", database);
                InserirLog(database);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanalPreco SET CanalID = @001, PrecoID = @002, DataInicio = '@003', DataFim = '@004', Quantidade = @005 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PrecoID.ValorBD);
                sql.Replace("@003", this.DataInicio.ValorBD);
                sql.Replace("@004", this.DataFim.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);

                int x = database.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int[] AdicionarMulti(int eventoID, int apresentacaoID, int setorID, int precoID, int todosID, string precoNome)
        {

            try
            {
                DataTable tabelaPrecosAlteraveis = precosAlteraveis(eventoID, apresentacaoID, setorID, precoID, todosID, precoNome);

                //verificar quais ainda nao existem na tabela
                Array precosIDs = Array.CreateInstance(typeof(DataRow), tabelaPrecosAlteraveis.Rows.Count);
                tabelaPrecosAlteraveis.Rows.CopyTo(precosIDs, 0);
                string precos = Utilitario.ArrayToString(precosIDs);

                string sqlPrecos = "SELECT tPreco.ID AS PrecoID, tCanalPreco.CanalID " +
                    "FROM tPreco (NOLOCK) " +
                    "LEFT JOIN tCanalPreco (NOLOCK) ON tCanalPreco.PrecoID=tPreco.ID AND tCanalPreco.CanalID=" + this.CanalID.Valor + " " +
                    "WHERE tPreco.ID in (" + precos + ")";

                bd.Consulta(sqlPrecos);

                DataTable tabela = new DataTable("Canais");
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["CanalID"] = bd.LerInt("CanalID");
                    tabela.Rows.Add(linha);
                }
                bd.Consulta().Close();

                CanalPreco canalPreco = new CanalPreco(this.Control.UsuarioID);
                canalPreco.Limpar();
                canalPreco.CanalID.Valor = this.CanalID.Valor;
                canalPreco.DataInicio.Valor = this.DataInicio.Valor;
                canalPreco.DataFim.Valor = this.DataFim.Valor;
                canalPreco.Quantidade.Valor = this.Quantidade.Valor;

                bd.IniciarTransacao();

                int countInseridos = 0;

                foreach (DataRow linha in tabela.Rows)
                {
                    int pID = (int)linha["PrecoID"];
                    int cID = (int)linha["CanalID"];

                    if (cID == 0)
                    { //null, pode incluir
                        canalPreco.PrecoID.Valor = pID;
                        canalPreco.Inserir(bd, true);

                        countInseridos++;
                    }

                }//while (bd.Consulta().Read())

                bd.FinalizarTransacao();
                int[] retorno = new int[2];
                retorno[0] = tabela.Rows.Count;
                retorno[1] = countInseridos;
                return retorno;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
                //} finally {
                //    bd.Fechar();
                //    bd.Cnn.Dispose();
            }

        }

        public override DataTable Canais(int precoid)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("DataInicio", typeof(DateTime));
                tabela.Columns.Add("DataFim", typeof(DateTime));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("CanalPrecoID", typeof(int));

                string sql = "SELECT c.ID,c.Nome,cp.ID AS CanalPrecoID, e.Nome AS Empresa, cp.DataInicio, cp.DataFim, cp.Quantidade " +
                    "FROM tCanal as c, tCanalPreco AS cp, tEmpresa AS e " +
                    "WHERE e.ID=c.EmpresaID AND c.ID=cp.CanalID AND " +
                    "cp.PrecoID=" + precoid + " ORDER BY c.Nome";
                bd = new BD();
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["DataInicio"] = bd.LerDateTime("DataInicio");
                    linha["DataFim"] = bd.LerDateTime("DataFim");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["CanalPrecoID"] = bd.LerInt("CanalPrecoID");
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

        public DataTable CanaisIR(int precoid)
        {
            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("DataInicio", typeof(DateTime));
                tabela.Columns.Add("DataFim", typeof(DateTime));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("CanalPrecoID", typeof(int));

                string sql = "SELECT DISTINCT TC.[ID]," +
                                    "TC.[Nome]," +
                                    "CP.[ID] AS CanalPrecoID, " +
                                    "TE.[Nome] AS Empresa, " +
                                    "CP.[DataInicio], " +
                                    "CP.[DataFim], " +
                                    "CP.[Quantidade] " +
                              "FROM [dbo].[tCanal] (NOLOCK) AS TC, " +
                                   "[dbo].[tCanalPreco] AS CP, " +
                                   "[dbo].[tEmpresa] AS TE " +
                             "WHERE TE.[ID] = TC.[EmpresaID] " +
                               "AND TC.[ID] = CP.[CanalID] " +
                               "AND CP.[PrecoID] = " + precoid + " " +
                               "AND TE.[EmpresaVende] = 'T' " +
                               "AND TE.[EmpresaPromove] = 'F' " +
                             "ORDER BY TC.[Nome] ";

                bd = new BD();
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["DataInicio"] = bd.LerDateTime("DataInicio");
                    linha["DataFim"] = bd.LerDateTime("DataFim");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["CanalPrecoID"] = bd.LerInt("CanalPrecoID");
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

        public DataTable CanaisIR(int precoID, int canalID)
        {
            try
            {
                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("DataInicio", typeof(DateTime));
                tabela.Columns.Add("DataFim", typeof(DateTime));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("CanalPrecoID", typeof(int));

                string sql = String.Format("SELECT DISTINCT TC.ID, TC.Nome, CP.ID AS CanalPrecoID, " +
                                           "TE.Nome AS Empresa, CP.DataInicio, CP.DataFim, CP.Quantidade " +
                                           "FROM tCanal (NOLOCK) AS TC " +
                                           "INNER JOIN tCanalPreco (NOLOCK) AS CP ON TC.ID = CP.CanalID " +
                                           "INNER JOIN tEmpresa (NOLOCK) AS TE ON TE.ID = TC.EmpresaID " +
                                           "WHERE  CP.PrecoID = {0} AND CP.CanalID = {1} " +
                                           "AND TE.EmpresaVende = 'T' AND TE.EmpresaPromove = 'F' " +
                                           "ORDER BY TC.Nome ", precoID, canalID);

                using (bd = new BD())
                {
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Empresa"] = bd.LerString("Empresa");
                        linha["DataInicio"] = bd.LerDateTime("DataInicio");
                        linha["DataFim"] = bd.LerDateTime("DataFim");
                        linha["Quantidade"] = bd.LerInt("Quantidade");
                        linha["CanalPrecoID"] = bd.LerInt("CanalPrecoID");
                        tabela.Rows.Add(linha);
                    }
                }

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CanaisIR(List<int> canais, List<int> precos)
        {
            try
            {
                int result = 0;

                string strPrecos = String.Join(",", precos);
                string strCanais = String.Join(",", canais);

                string sql = String.Format("SELECT COUNT(ID) AS val " +
                                           "FROM tCanalPreco (NOLOCK) " +
                                           "WHERE PrecoID IN ({0}) AND CanalID IN ({1}) ", strPrecos, strCanais);

                using (bd = new BD())
                {
                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        result = bd.LerInt("val");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Inserir(bool inserir)
        {
            try
            {
                bd.IniciarTransacao();

                bool resultado = this.Inserir(bd, inserir);

                bd.FinalizarTransacao();


                return resultado;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
        }

        public bool Inserir(BD bd, bool inserir)
        {
            try
            {
                bool result = false;
                
                oRoboCanalPreco.PrecoID.Valor = this.PrecoID.Valor;
                oRoboCanalPreco.CanalID.Valor = this.CanalID.Valor;
                oRoboCanalPreco.DataFim.Valor = this.DataFim.ValorBD;
                oRoboCanalPreco.DataInicio.Valor = this.DataInicio.ValorBD;
                oRoboCanalPreco.Quantidade.Valor = this.Quantidade.Valor;

                if (oRoboCanalPreco.VerificarEventoGeradoDepois(this.CanalID.Valor) && !inserir)
                {
                    oRoboCanalPreco.Operacao.Valor = Convert.ToChar(RoboCanalEvento.operacaobanco.Inserir).ToString();
                    result = oRoboCanalPreco.Inserir(bd);
                }
                else
                {
                    this.Control.Versao = 0;

                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO tCanalPreco(CanalID, PrecoID, DataInicio, DataFim, Quantidade) ");
                    sql.Append("VALUES (@001,@002,'@003','@004',@005); SELECT SCOPE_IDENTITY();");

                    sql.Replace("@001", this.CanalID.ValorBD);
                    sql.Replace("@002", this.PrecoID.ValorBD);
                    sql.Replace("@003", this.DataInicio.ValorBD);
                    sql.Replace("@004", this.DataFim.ValorBD);
                    sql.Replace("@005", this.Quantidade.ValorBD);

                    object x = bd.ConsultaValor(sql.ToString());
                    this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                    result = this.Control.ID > 0;

                    if (result)
                        InserirControle("I", bd);

                    oRoboCanalPreco.Operacao.Valor = Convert.ToChar(RoboCanalEvento.operacaobanco.Deleletar).ToString();
                    result = oRoboCanalPreco.Inserir(bd);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void InserirControle(string acao, CTLib.BD database)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCanalPreco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                database.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override bool Excluir(int id)
        {
            try
            {
                bool resultado = Excluir(id, bd);

                return resultado;
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

        public bool Excluir(int id, BD bd)
        {
            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalPreco WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class CanalPrecoLista : CanalPrecoLista_B
    {

        public CanalPrecoLista() { }

        public CanalPrecoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
