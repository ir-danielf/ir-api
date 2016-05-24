/**************************************************
* Arquivo: Local.cs
* Gerado: 30/05/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using Google.Api.Maps.Service;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Local : Local_B
    {
        public enum EnumAcoesImagemLocal
        {
            Manter = 0,
            Enviar = 1,
            Remover = 2,
        }


        public Local() { }

        public Local(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Inserir novo(a) Local
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLocal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append(@"INSERT INTO tLocal(ID, EmpresaID, ContratoID, Nome, Contato, Logradouro, Cidade, Estado, CEP, DDDTelefone, Telefone, Bairro, Numero, Estacionamento, 
                EstacionamentoObs, AcessoNecessidadeEspecial, AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, Complemento, ComoChegarInternet, HorariosBilheteria, 
                RetiradaBilheteria, PaisID, ImagemInternet, CodigoPraca, Alvara, AVCB, FonteImposto, PorcentagemImposto, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao) ");

                sql.Append(@"VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011',@012,'@013','@014', 
                '@015','@016','@017','@018','@019','@020','@021','@022',@023,'@024','@025', '@026', '@027', '@028', @029, '@030', '@031', '@032', '@033', @034)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);
                sql.Replace("@025", this.CodigoPraca.ValorBD);

                sql.Replace("@026", this.Alvara.ValorBD);
                sql.Replace("@027", this.AVCB.ValorBD);
                sql.Replace("@028", this.FonteImposto.ValorBD);
                sql.Replace("@029", this.PorcentagemImposto.ValorBD);

                sql.Replace("@030", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@031", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@032", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@033", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@034", this.Lotacao.ValorBD);

                int x = bd.Executar(sql.ToString());

                if (formasPagamento != null)
                    InserirFormasPagamento(formasPagamento);

                if (horarios != null)
                {
                    foreach (LocalHorario oLocalHorario in horarios)
                    {
                        oLocalHorario.LocalID.Valor = this.Control.ID;
                        oLocalHorario.Inserir();
                    }
                }

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

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


        /// <summary>
        /// Atualiza Local
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {
            try
            {
                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cLocal WHERE ID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append(@"UPDATE tLocal SET EmpresaID = @001, ContratoID = @002, Nome = '@003', Contato = '@004', Logradouro = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', 
                Telefone = '@010', Bairro = '@011', Numero = @012, Estacionamento = '@013', EstacionamentoObs = '@014', AcessoNecessidadeEspecial = '@015', AcessoNecessidadeEspecialObs = '@016', ArCondicionado = '@017', 
                ComoChegar = '@018', Complemento = '@019', ComoChegarInternet = '@020', HorariosBilheteria = '@021', RetiradaBilheteria = '@022', PaisID = @023, ImagemInternet = '@024',
                Alvara = '@025', AVCB = '@026', FonteImposto = '@027', PorcentagemImposto = @028, DataEmissaoAlvara = '@029', DataValidadeAlvara = '@030', DataEmissaoAvcb = '@031', DataValidadeAvcb = '@032', Lotacao = @033");


                sql.Append(" WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);

                sql.Replace("@025", this.Alvara.ValorBD);
                sql.Replace("@026", this.AVCB.ValorBD);
                sql.Replace("@027", this.FonteImposto.ValorBD);
                sql.Replace("@028", this.PorcentagemImposto.ValorBD);

                sql.Replace("@029", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@030", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@031", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@032", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@033", this.Lotacao.ValorBD);

                int x = bd.Executar(sql.ToString());

                LimparFormasPagamento();
                LimparHorarios();
                InserirFormasPagamento(formasPagamento);
                foreach (LocalHorario oLocalHorario in horarios)
                {
                    oLocalHorario.LocalID.Valor = this.Control.ID;
                    oLocalHorario.Inserir();
                }

                this.formasPagamento = FormasPagamentoLocal();
                this.horarios = HorariosLocal();

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

        public override void Limpar()
        {
            base.Limpar();
            horarios = new List<LocalHorario>();
            formasPagamento = new DataTable();
        }

        /// <summary>
        /// Preenche todos os atributos de Local
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tLocal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Contato.ValorBD = bd.LerString("Contato");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Numero.ValorBD = bd.LerInt("Numero").ToString();
                    this.Estacionamento.ValorBD = bd.LerString("Estacionamento");
                    this.EstacionamentoObs.ValorBD = bd.LerString("EstacionamentoObs");
                    this.AcessoNecessidadeEspecial.ValorBD = bd.LerString("AcessoNecessidadeEspecial");
                    this.AcessoNecessidadeEspecialObs.ValorBD = bd.LerString("AcessoNecessidadeEspecialObs");
                    this.ArCondicionado.ValorBD = bd.LerString("ArCondicionado");
                    this.ComoChegar.ValorBD = bd.LerString("ComoChegar");
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.ComoChegarInternet.ValorBD = bd.LerString("ComoChegarInternet");
                    this.HorariosBilheteria.ValorBD = bd.LerString("HorariosBilheteria");
                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");
                    this.PaisID.ValorBD = bd.LerInt("PaisID").ToString();
                    this.ImagemInternet.ValorBD = bd.LerString("ImagemInternet");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.FonteImposto.ValorBD = bd.LerString("FonteImposto");
                    this.PorcentagemImposto.ValorBD = bd.LerString("PorcentagemImposto");

                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerString("Lotacao");

                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
                this.formasPagamento = FormasPagamentoLocal();
                this.horarios = HorariosLocal();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// preenche apenas ID,EmpresaID e Nome do Local
        /// </summary>
        /// <param name="id"></param>
        public void LerEmpresaNome(int id)
        {

            try
            {

                string sql = "SELECT ID,EmpresaID,Nome FROM tLocal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
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

        /// <summary>		
        /// Obter todos os locais
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int));
                string sql = "SELECT ID, Nome, EmpresaID FROM tLocal (NOLOCK) ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
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


        public DataTable TodosComEmpresaRegional()
        {
            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("RegionalID", typeof(int));

                string sql = @"SELECT tLocal.ID, tLocal.Nome, EmpresaID, RegionalID FROM tLocal (NOLOCK)
                            INNER JOIN tEmpresa (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID
                            ORDER BY tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["RegionalID"] = bd.LerInt("RegionalID");

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

        /// <summary>
        /// Carrega os locais por empresa        
        /// </summary>
        /// <param name="empresaID">Empresa</param>
        /// <returns></returns>
        public DataTable carregarLocaisPorEmpresa(int empresaID)
        {
            /// Job: 202
            /// Autor: LP
            return carregarLocais(empresaID, 0);
        }

        public DataTable carregarLocaisPorEmpresa(int empresaID, bool registroZero)
        {
            try
            {
                DataTable dados = new DataTable();

                dados = carregarLocais(empresaID, 0);

                if (empresaID == 0)
                    dados.Clear();

                if (registroZero)
                {
                    DataRow oDataRow = dados.NewRow();
                    oDataRow["ID"] = 0;
                    oDataRow["Nome"] = "Selecione...";
                    dados.Rows.InsertAt(oDataRow, 0);
                }

                return dados;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Carrega os locais por id
        /// </summary>
        /// <param name="localID">Local</param>
        /// <returns></returns>
        public DataTable carregarLocaisPorID(int localID)
        {
            /// Job: 202
            /// Autor: LP

            return carregarLocais(0, localID);
        }




        /// <summary>
        /// Carrega os locais por empresa ou local
        /// </summary>
        /// <param name="empresaID">Empresa</param>
        /// <param name="localID">Local</param>
        /// <returns></returns>
        private DataTable carregarLocais(int empresaID, int localID)
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
                    "   tLocal.ID, " +
                    "   tLocal.Nome " +
                    "FROM tLocal " +
                    "INNER JOIN tEvento ON tEvento.LocalID = tLocal.ID " +
                    "INNER JOIN tApresentacao ON tApresentacao.EventoID = tEvento.ID " +
                    "WHERE " +
                    "   (tApresentacao.DisponivelAjuste = 'T') " +
                    ((empresaID != 0) ? "" +
                    "AND " +
                    "   (tLocal.EmpresaID = " + empresaID + ") " : "") +
                    ((localID != 0) ? "" +
                    "AND " +
                    "   (tLocal.ID = " + localID + ") " : "") +
                    "ORDER BY " +
                    "   tLocal.Nome"))
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

        /// <summary>		
        /// Obter todos os locais por regionalId
        /// </summary>
        /// <returns></returns>
        public DataTable TodosPorRegionalID(int regionalID)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("RegionalID", typeof(int));

                string sql = "SELECT tLocal.ID, tLocal.Nome , tEmpresa.ID AS EmpresaID " +
                            "FROM tLocal (NOLOCK) " +
                            "INNER JOIN tEmpresa (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID " +
                            "WHERE tEmpresa.RegionalID = " + regionalID + " " +
                            "ORDER BY tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EmpresaID"] = bd.LerInt("EmpresaID");
                    linha["RegionalID"] = regionalID;
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

        /// <summary>		
        /// Obter todos os locais
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(ArrayList locaisIDs)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome " +
                    "FROM tLocal " +
                    "WHERE ID in (" + Utilitario.ArrayToString(locaisIDs) + ") " +
                    "ORDER BY Nome";

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
        /// Carrega a tabela temporária do banco de dados com os eventos buscados para o Local
        /// </summary>
        public void CarregaEventosEmTemp(int localID, string sessionID)
        {
            try
            {
                string sql = string.Empty;

                //limpar a tabela
                sql = "DELETE FROM tIRWebEventos WHERE SessionID = '" + sessionID + "'";

                bd.Executar(sql);

                sql =
                    @"INSERT INTO tIRWebEventos (ID,Nome,SessionID)
					SELECT DISTINCT e.ID,e.Nome,'" + sessionID +
                    @"' FROM tEvento AS e, tApresentacao AS a 
					WHERE e.ID=a.EventoID AND e.LocalID= " + localID + @" AND a.DisponivelRelatorio = 'T'
					ORDER BY e.Nome";

                bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carrega a tabela temporária do banco de dados com os eventos buscados para o Local
        /// </summary>
        //		public void CarregaEventosEmTemp(int localID,int eventoID)
        //		{
        //			try
        //			{
        //					string sql = string.Empty;
        //				
        //				//limpar a tabela
        //				sql = "DELETE FROM ##temporaria";
        //
        //				bd.Executar(sql);
        //				
        //				sql = 
        //					@"INSERT INTO ##temporaria (ID,Nome)
        //					SELECT DISTINCT e.ID,e.Nome 
        //					FROM tEvento AS e, tApresentacao AS a 
        //					WHERE e.ID=a.EventoID";
        //					if(eventoID > 0)
        //						sql += " AND e.ID = " + eventoID;
        //						
        //					sql += " AND e.LocalID= " + localID + @" AND a.DisponivelRelatorio='T'
        //					ORDER BY e.Nome";
        //
        //				bd.Executar(sql);
        //
        //				bd.Fechar();
        //			}
        //			catch(Exception ex)
        //			{
        //				throw ex;
        //			}
        //		}

        /// <summary>		
        /// Obter pacotes de um Local
        /// </summary>
        /// <returns></returns>
        public override DataTable Cortesias()
        {

            try
            {

                DataTable tabela = new DataTable("Cortesia");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RGB", typeof(string));

                string sql = "SELECT tCortesia.ID,tCortesia.Nome,tCor.RGB " +
                    "FROM tCortesia,tCor " +
                    "WHERE tCortesia.LocalID=" + this.Control.ID + " AND tCor.ID=tCortesia.CorID " +
                    "ORDER BY tCortesia.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RGB"] = bd.LerString("RGB");
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
        /// Obtendo um DataTable das Cortesias de um Local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable Cortesias(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Cortesia");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, 0, "255255255" });

                string sql = "SELECT tCortesia.ID,tCortesia.Nome,tCortesia.CorID, tCor.RGB " +
                    "FROM tCortesia,tCor " +
                    "WHERE tCortesia.LocalID=" + this.Control.ID + " AND tCor.ID=tCortesia.CorID " +
                    "ORDER BY tCortesia.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RGB"] = bd.LerString("RGB");
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
        /// Obter pacotes de um Local
        /// </summary>
        /// <returns></returns>
        public override DataTable Pacotes()
        {

            return Pacotes(null, Apresentacao.Disponibilidade.Nula);

        }

        /// <summary>		
        /// Obter pacotes de um Local
        /// </summary>
        /// <param name="registroZero">Conteúdo do registro Zero</param>
        /// <returns></returns>
        public DataTable Pacotes(string registroZero)
        {

            return Pacotes(registroZero, Apresentacao.Disponibilidade.Nula);

        }

        /// <summary>		
        /// Obter pacotes de um Local
        /// </summary>
        /// <param name="registroZero">Conteúdo do registro Zero</param>
        /// /// <param name="disponibilidade">Disponibilidade da apresentação</param>
        /// <returns></returns>
        public DataTable Pacotes(string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {
            DataTable tabela = new DataTable("Pacote");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND tApresentacao.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND tApresentacao.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND tApresentacao.DisponivelRelatorio='T'" : "";

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tPacote.ID, " +
                    "   tPacote.Nome " +
                    "FROM tPacote (NOLOCK) " +
                    "INNER JOIN tPacoteItem (NOLOCK) ON tPacoteItem.PacoteID = tPacote.ID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tPacoteItem.ApresentacaoID = tApresentacao.ID " +
                    "WHERE " +
                    "   tPacote.LocalID = " + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
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
        /// Obter somente os setores com lugar marcado desse local
        /// </summary>
        /// <returns></returns>
        public override DataTable SetoresLugarMarcado()
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string)); //tipo do lugar marcado

                string sql = "SELECT ID,Nome,LugarMarcado FROM tSetor WHERE LocalID=" + this.Control.ID + " AND " +
                    "LugarMarcado<>'" + Setor.Pista + "' ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter somente os setores com lugar marcado desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable SetoresLugarMarcado(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string)); //tipo do lugar marcado
                tabela.Columns.Add("AprovadoPublicacao", typeof(bool));//Aprovado para selecao de lugares pelo cliente no site
                tabela.Columns.Add("VersaoBackground", typeof(int));
                tabela.Columns.Add("Alterado", typeof(bool)).DefaultValue = false;

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID, Nome, LugarMarcado, AprovadoPublicacao, IsNull(VersaoBackground, 0) AS VersaoBackground " +
                    "FROM tSetor (NOLOCK) WHERE LocalID=" + this.Control.ID + " AND " +
                    "LugarMarcado <> '" + Setor.Pista + "' ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                    linha["AprovadoPublicacao"] = bd.LerBoolean("AprovadoPublicacao");
                    linha["VersaoBackground"] = bd.LerInt("VersaoBackground");
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
        /// Obter setores (Nome, ID, LugarMarcado) desse local
        /// </summary>
        /// <returns></returns>
        public override DataTable Setores()
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("LugarMarcado", typeof(string));

                string sql = "SELECT ID,Nome,Produto,LugarMarcado " +
                    "FROM tSetor " +
                    "WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter setores (Nome, ID, LugarMarcado) desse local
        /// </summary>
        /// <returns></returns>
        public DataTable Setores(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("LugarMarcado", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, false, "" });

                string sql = "SELECT ID,Nome,Produto,LugarMarcado " +
                    "FROM tSetor " +
                    "WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter Eventos desse local
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT e.ID, e.Nome " +
                    "FROM tEvento AS e, tApresentacao AS a " +
                    "WHERE e.ID=a.EventoID AND e.LocalID=" + this.Control.ID +
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


        public DataTable Eventos(string registroZero, int localID)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });


                string sql = "SELECT DISTINCT e.ID, e.Nome " +
                    "FROM tEvento AS e, tApresentacao AS a " +
                    "WHERE e.ID=a.EventoID AND e.LocalID=" + localID +
                    " ORDER BY e.Nome";

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

        public DataTable Eventos(string registroZero, Apresentacao.Disponibilidade disponibilidade, int localID)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("TipoCodigoBarra", typeof(string)).DefaultValue = ((char)Enumerators.TipoCodigoBarra.Estruturado).ToString();

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT e.ID, e.Nome, e.TipoCodigoBarra " +
                    "FROM tEvento AS e (NOLOCK), tApresentacao AS a(NOLOCK) " +
                    "WHERE e.ID=a.EventoID AND e.LocalID=" + localID +
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
                    linha["LocalID"] = localID;
                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
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
        /// Obter setores (Nome, ID, LugarMarcado e Quantidade) desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable Setores(string registroZero, bool comQuantidade)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, false, "", null });

                string sql;

                if (comQuantidade)
                    sql = "SELECT tSetor.ID,tSetor.Nome,tSetor.Produto,tSetor.LugarMarcado,SUM(Quantidade) AS Quantidade " +
                        "FROM tSetor " +
                        "LEFT JOIN tLugar ON tLugar.SetorID=tSetor.ID " +
                        "WHERE tSetor.LocalID=" + this.Control.ID + " " +
                        "GROUP BY tSetor.ID,tSetor.Nome,tSetor.Produto,tSetor.LugarMarcado " +
                        "ORDER BY Nome";
                else
                    sql = "SELECT ID,Nome,LugarMarcado,null as Quantidade FROM tSetor WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
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
        /// Obter Eventos desse local
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(Apresentacao.Disponibilidade disponibilidade)
        {

            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool));
            tabela.Columns.Add("LocalID", typeof(int)).DefaultValue = this.Control.ID;
            tabela.Columns.Add("MapaEsquematicoID", typeof(int)).DefaultValue = 0;
            tabela.Columns.Add("TipoCodigoBarra", typeof(string)).DefaultValue = ((char)Enumerators.TipoCodigoBarra.Estruturado).ToString();
            tabela.Columns.Add("VenderPos", typeof(bool)).DefaultValue = false;

            try
            {

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   e.ID, " +
                    "   e.Nome, " +
                    "   e.ImpressaoCodigoBarra, " +
                    "   e.MapaEsquematicoID, " +
                    "   e.TipoCodigoBarra " +
                    "FROM tEvento AS e (NOLOCK) " +
                    "INNER JOIN tApresentacao AS a (NOLOCK) ON e.ID = a.EventoID " +
                    "WHERE e.LocalID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome"))
                {

                    DataRow linha;
                    while (bd.Consulta().Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["ImpressaoCodigoBarra"] = bd.LerBoolean("ImpressaoCodigoBarra");
                        linha["MapaEsquematicoID"] = bd.LerInt("MapaEsquematicoID");
                        linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");

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
        /// Obter Eventos desse local
        /// </summary>
        /// <returns></returns>
        public override DataTable Eventos()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID,Nome FROM tEvento " +
                    "WHERE LocalID=" + this.Control.ID + " " +
                    "ORDER BY Nome";

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
        /// Obter Eventos desse local
        /// </summary>
        /// <returns></returns>
        public DataTable EventosComImagem()
        {
            return EventosComImagem(Apresentacao.Disponibilidade.Nula);
        }

        /// <summary>		
        /// Obter Eventos desse local que obedeçam a disponibilidade informada.
        /// </summary>
        /// <returns></returns>
        public DataTable EventosComImagem(Apresentacao.Disponibilidade disponibilidade)
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
                tabela.Columns.Add("ImpressaoCodigoBarra", typeof(string));

                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND (DisponivelVenda = 'T')" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND (DisponivelAjuste = 'T')" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND (DisponivelRelatorio = 'T')" : "";

                bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tEvento.ID, " +
                    "   tEvento.Nome, " +
                    "   tEvento.VersaoImagemIngresso, " +
                    "   tEvento.VersaoImagemVale, " +
                    "   tEvento.VersaoImagemVale2, " +
                    "   tEvento.VersaoImagemVale3, " +
                    "   tEvento.ImpressaoCodigoBarra " +
                    "FROM " +
                    "   tEvento " +
                    "INNER JOIN " +
                    "   tApresentacao " +
                    "ON " +
                    "   tEvento.ID = tApresentacao.EventoID " +
                    "WHERE " +
                    "   (tApresentacao.DisponivelAjuste = 'T') " +
                    "AND " +
                    "   (tEvento.LocalID = " + this.Control.ID + ") " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY " +
                    "   tEvento.Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["VersaoImagemIngresso"] = bd.LerInt("VersaoImagemIngresso");
                    linha["VersaoImagemVale"] = bd.LerInt("VersaoImagemVale");
                    linha["VersaoImagemVale2"] = bd.LerInt("VersaoImagemVale2");
                    linha["VersaoImagemVale3"] = bd.LerInt("VersaoImagemVale3");
                    linha["ImpressaoCodigoBarra"] = bd.LerString("ImpressaoCodigoBarra");

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
        /// Obter eventos desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable Eventos(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID,Nome FROM tEvento WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter lojas desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public override DataTable Lojas(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql =
                    "SELECT        tLoja.ID, tLoja.Nome " +
                    "FROM            tEstoque INNER JOIN " +
                    "tLocal ON tEstoque.LocalID = tLocal.ID INNER JOIN " +
                    "tLoja ON tEstoque.ID = tLoja.EstoqueID " +
                    "WHERE        (tLocal.ID = " + this.Control.ID + ")";
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
        } // fim de Lojas


        /// <summary>
        /// Obter bloqueios desse local
        /// </summary>
        /// <returns></returns>
        public override DataTable Bloqueios()
        {

            try
            {

                DataTable tabela = new DataTable("Bloqueio");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LocalID", typeof(int)).DefaultValue = this.Control.ID;
                tabela.Columns.Add("CorID", typeof(int));

                string sql = "SELECT ID,Nome,CorID " +
                    "FROM tBloqueio " +
                    "WHERE LocalID=" + this.Control.ID + " " +
                    "ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
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
        public DataTable Bloqueios(int localID)
        {
            if (localID < 1)
                throw new ApplicationException("O parâmetro localID deve ser maior que zero!");

            this.Control.ID = localID;
            return this.Bloqueios(null);
        }


        /// <summary>
        /// Obter bloqueios desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable Bloqueios(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Bloqueio");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, 0, "255255255" });

                string sql = "SELECT tBloqueio.ID,tBloqueio.Nome,tBloqueio.CorID,tCor.RGB " +
                    "FROM tBloqueio,tCor " +
                    "WHERE tBloqueio.LocalID=" + this.Control.ID + " AND tCor.ID=tBloqueio.CorID " +
                    "ORDER BY tBloqueio.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["RGB"] = bd.LerString("RGB");
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
        /// Obter bloqueios desse local
        /// </summary>
        /// <param name="registroZero">Contehudo do registro Zero</param>
        /// <returns></returns>
        public DataTable BloqueiosAssinaturas(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Bloqueio");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, 0, "255255255" });

                string sql = "SELECT tBloqueio.ID,tBloqueio.Nome,tBloqueio.CorID,tCor.RGB" +
                    "FROM tBloqueio,tCor " +
                    "WHERE tBloqueio.LocalID=" + this.Control.ID + " AND tCor.ID=tBloqueio.CorID AND tBloqueio.Assinatura='T' " +
                    "ORDER BY tBloqueio.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["RGB"] = bd.LerString("RGB");
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
        /// Obter estoques desse local
        /// </summary>
        /// <returns></returns>
        public override DataTable Estoques()
        {

            try
            {

                DataTable tabela = new DataTable("Estoque");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome FROM tEvento WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter estoques desse local
        /// </summary>
        /// <returns></returns>
        public DataTable Estoques(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Estoque");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID,Nome FROM tEstoque WHERE LocalID=" + this.Control.ID + " ORDER BY Nome";

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

        public DataTable Listagem(int empresaID)
        {
            try
            {
                DataTable tabela = new DataTable("LocalListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Empresa", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Contato", typeof(string));
                tabela.Columns.Add("DDD", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Endereço", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("Observação", typeof(string));
                // Condição tratada
                string condicao = "";
                if (empresaID > 0)
                {
                    condicao = "WHERE      (tEmpresa.ID = " + empresaID + ") ";
                }
                else
                {
                    condicao = "";
                }
                // Obtendo dados
                string sql;
                sql =
                    "SELECT     tEmpresa.Nome AS Empresa, tLocal.ID, tLocal.Nome AS Local, tLocal.Contato, tLocal.Endereco, tLocal.Cidade, tLocal.Estado, tLocal.CEP, tLocal.DDDTelefone, tLocal.Telefone, tLocal.Obs " +
                    "FROM       tLocal INNER JOIN " +
                    "tEmpresa ON tLocal.EmpresaID = tEmpresa.ID " +
                    condicao +
                    "ORDER BY tEmpresa.Nome, tLocal.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Empresa"] = bd.LerString("Empresa");
                    linha["Local"] = bd.LerString("Local");
                    linha["Contato"] = bd.LerString("Contato");
                    linha["Endereço"] = bd.LerString("Endereco");
                    linha["Cidade"] = bd.LerString("Cidade");
                    linha["Estado"] = bd.LerString("Estado");
                    linha["CEP"] = bd.LerString("CEP");
                    linha["DDD"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
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

        public DataTable LocalPorEmpresa(int empresaID, string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("LocalListagem");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Local", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID, Nome AS Local FROM tLocal WHERE EmpresaID = " + empresaID + " ORDER BY Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Local"] = bd.LerString("Local");
                    tabela.Rows.Add(linha);
                }
                return tabela;
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


        public DataTable CanaisPerfilLocal(string registroZero, int LocalID)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = @"SELECT DISTINCT ca.ID,ca.Nome 
								FROM tEvento AS e,tCanalEvento AS ce,tCanal AS ca
								WHERE ca.ID=ce.CanalID AND ce.EventoID=e.ID
								AND e.LocalID = " + LocalID + " ORDER BY ca.Nome";


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

        public List<LocalHorario> HorariosLocal()
        {
            BD bd = new BD();
            bd.Consulta("SELECT * FROM tLocalHorario WHERE LocalID = " + this.Control.ID);
            List<LocalHorario> listaRetorno = new List<LocalHorario>();
            LocalHorario oLocalHorario;
            while (bd.Consulta().Read())
            {
                oLocalHorario = new LocalHorario();
                oLocalHorario.LocalID.Valor = bd.LerInt("LocalID");
                oLocalHorario.HoraInicio.Valor = bd.LerDateTime("HoraInicio");
                oLocalHorario.HoraFim.Valor = bd.LerDateTime("HoraFim");
                oLocalHorario.DiaSemana.Valor = bd.LerInt("DiaSemana");
                listaRetorno.Add(oLocalHorario);
            }
            return listaRetorno;
        }

        public DataTable FormasPagamentoNaoLocal(DataTable formasPagamentoLocal)
        {
            BD bd = new BD();
            bd.Consulta(@"SELECT DISTINCT f.ID, f.Nome
                          FROM tFormaPagamento f");
            DataTable tabelaRetorno = new DataTable();
            tabelaRetorno.Columns.Add("ID", typeof(int));
            tabelaRetorno.Columns.Add("Nome", typeof(string));
            DataRow novaLinha;
            while (bd.Consulta().Read())
            {
                if (formasPagamentoLocal.Columns.Count > 0 && formasPagamentoLocal.Select("ID=" + bd.LerInt("ID")).Length > 0)
                    continue;
                novaLinha = tabelaRetorno.NewRow();
                novaLinha["ID"] = bd.LerInt("ID");
                novaLinha["Nome"] = bd.LerString("Nome");
                tabelaRetorno.Rows.Add(novaLinha);
            }
            return tabelaRetorno;
        }

        public DataTable FormasPagamentoLocal()
        {
            BD bd = new BD();
            bd.Consulta("SELECT f.ID, f.Nome FROM tLocalFormaPagamento lf, tFormaPagamento f WHERE f.ID = lf.FormaPagamentoID AND lf.LocalID = " + this.Control.ID);
            DataTable tabelaRetorno = new DataTable();
            tabelaRetorno.Columns.Add("ID", typeof(int));
            tabelaRetorno.Columns.Add("Nome", typeof(string));
            DataRow novaLinha;
            while (bd.Consulta().Read())
            {
                novaLinha = tabelaRetorno.NewRow();
                novaLinha["ID"] = bd.LerInt("ID");
                novaLinha["Nome"] = bd.LerString("Nome");
                tabelaRetorno.Rows.Add(novaLinha);
            }
            return tabelaRetorno;
        }

        public void LimparHorarios()
        {
            BD bd = new BD();
            bd.Executar("DELETE FROM tLocalHorario WHERE LocalID = " + this.Control.ID);
        }

        public void LimparFormasPagamento()
        {
            BD bd = new BD();
            bd.Executar("DELETE FROM tLocalFormaPagamento WHERE LocalID = " + this.Control.ID);
        }

        DataTable formasPagamento;

        public DataTable FormasPagamento
        {
            get { return formasPagamento; }
            set { formasPagamento = value; }
        }
        List<LocalHorario> horarios;

        public List<LocalHorario> Horarios
        {
            get { return horarios; }
            set { horarios = value; }
        }

        public void InserirFormasPagamento(DataTable formasPagamento)
        {
            StringBuilder sql = new StringBuilder();
            BD bd = new BD();

            foreach (DataRow dr in formasPagamento.Rows)
                sql.AppendLine("INSERT INTO tLocalFormaPagamento (LocalID,FormaPagamentoID) VALUES (" + this.Control.ID + "," + dr["ID"] + ")");

            if (sql.ToString().Trim().Length != 0)
                bd.Executar(sql);
        }

        public object Todos(Apresentacao.Disponibilidade disponibilidade)
        {
            try
            {

                DataTable tabela = new DataTable("Local");

                // Verificando a disponibilidade
                string disponivelVenda = (disponibilidade == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = (disponibilidade == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = (disponibilidade == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT tLocal.ID, tLocal.Nome FROM tLocal 
                                INNER JOIN tEvento ON tLocal.ID = tEvento.LocalID
                                INNER JOIN tApresentacao ON tApresentacao.EventoID = tEvento.ID " +
                                disponivelAjuste +
                                disponivelRelatorio +
                                disponivelVenda +
                                " ORDER BY tLocal.Nome";

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
        public DataTable LocalIDNome(int localID)
        {
            try
            {
                string sql = "SELECT Nome From tLocal (NOLOCK) WHERE ID = " + localID;

                DataTable retorno = new DataTable();
                retorno.Columns.Add("ID", typeof(int));
                retorno.Columns.Add("Nome", typeof(string));

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = retorno.NewRow();
                    linha["ID"] = localID;
                    linha["Nome"] = bd.LerString("Nome");
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
        public DataTable LocalIDNome(int localID, Apresentacao.Disponibilidade disponibilidade)
        {
            try
            {
                // Verificando a disponibilidade
                string disponivelVenda = (disponibilidade == Apresentacao.Disponibilidade.Vender) ? " AND DisponivelVenda='T'" : "";
                string disponivelAjuste = (disponibilidade == Apresentacao.Disponibilidade.Ajustar) ? " AND DisponivelAjuste='T'" : "";
                string disponivelRelatorio = (disponibilidade == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND DisponivelRelatorio='T'" : "";

                string sql = @"SELECT DISTINCT tLocal.Nome From tLocal (NOLOCK) 
                                INNER JOIN tEvento (NOLOCK) ON tEvento.LocalID = tLocal.ID
                                INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
                                disponivelVenda +
                                disponivelAjuste +
                                disponivelRelatorio +
                                " WHERE  tLocal.ID = " + localID;

                DataTable retorno = new DataTable();
                retorno.Columns.Add("ID", typeof(int));
                retorno.Columns.Add("Nome", typeof(string));

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    DataRow linha = retorno.NewRow();
                    linha["ID"] = localID;
                    linha["Nome"] = bd.LerString("Nome");
                    retorno.Rows.Add(linha);
                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable EventosSemIngresso(string registroZero)
        {
            DataRow linha;

            // Estrutura de retorno
            DataTable tabela = new DataTable("EventosSemIngresso");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TipoCodigoBarra", typeof(string));

            try
            {
                // Valor padrão
                if (registroZero != null)
                {
                    linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = registroZero;
                    linha["TipoCodigoBarra"] = string.Empty;
                    tabela.Rows.Add(linha);
                }

                // Se o ID do Local for preenchido
                if (this.Control.ID > 0)
                {
                    // Preenche as apresentações do local que estejam disponíveis para ajuste e não tenham ingressos gerados.
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT DISTINCT " +
                        "	tEvento.ID, " +
                        "	tEvento.Nome, " +
                        "   tEvento.TipoCodigoBarra " +
                        "FROM tEvento (NOLOCK) " +
                        "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
                        "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID " +
                        "INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "WHERE " +
                        "   (tEvento.LocalID = " + this.Control.ID + ") " +
                        "AND " +
                        "   (tApresentacao.DisponivelAjuste = 'T') " +
                        "AND " +
                        "	(tApresentacaoSetor.IngressosGerados = 'F') " +
                        "AND " +
                        "   (tSetor.LugarMarcado <> '" + Setor.Pista + "') " +
                        "ORDER BY tEvento.Nome"))
                    {
                        while (oDataReader.Read())
                        {
                            linha = tabela.NewRow();
                            linha["ID"] = bd.LerInt("ID");
                            linha["Nome"] = bd.LerString("Nome");
                            linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
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

        public DataTable CarregarLocaisInternet()
        {
            try
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add("ID", typeof(int));
                dtt.Columns.Add("Nome", typeof(string));

                DataRow dtr = null;

                bd.Consulta("EXEC proc_LocaisInternet");

                while (bd.Consulta().Read())
                {
                    dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("LocalID");
                    dtr["Nome"] = bd.LerString("Local");
                    dtt.Rows.Add(dtr);
                }

                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public Estado EstadoLocalPorID(int id)
        {
            Estado estado = new Estado();
            //this.Estado.Valor
            try
            {
                string sql = @"select top 1 est.id from testado (nolock) est inner join tlocal (nolock) loc on 
                              est.sigla COLLATE DATABASE_DEFAULT = loc.estado COLLATE DATABASE_DEFAULT 
                                where loc.id = " + id;
            }
            finally
            {
                bd.Fechar();
            }
            return estado;
        }

        public bool Atualizar(int LocalID, string ImagemNome)
        {
            this.Ler(LocalID);
            this.ImagemInternet.Valor = ImagemNome;
            return this.Atualizar();
        }

        public object TodosComEmpresa(int EmpresaID, string RegistroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (RegistroZero != null)
                    tabela.Rows.Add(new Object[] { "0", RegistroZero });

                string sql = String.Format(@"SELECT tLocal.ID, tLocal.Nome
                            FROM tLocal (NOLOCK)
                            WHERE tLocal.EmpresaID = {0}
                            ORDER BY tLocal.Nome", EmpresaID);

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
            finally
            {
                bd.Fechar();
            }



        }

        public object Apresentacoes(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Apresentacoes");

                // Criar DataTable com as colunas
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = String.Format(@"
                    SELECT tApresentacao.Horario, tApresentacao.ID 
                    FROM tApresentacao
                        INNER JOIN tEvento ON tApresentacao.EventoID = tEvento.ID
                    WHERE LocalID = {0}
                    ORDER BY Horario", this.Control.ID);

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

        public void AtualizarCoordenadas()
        {
            try
            {
                List<EstruturaPontoDeVenda> retorno = this.BuscarTodos();

                foreach (EstruturaPontoDeVenda item in retorno)
                {
                    GeographicPosition coordenadas = IRLib.Paralela.CEP.BuscarCoordenadas(item.CEP.ToCEP(), item.Endereco, item.Cidade, item.Estado);

                    if (coordenadas != null)
                    {
                        string strSql = string.Format(@"UPDATE tLocal SET Latitude = '{0}', Longitude = '{1}' WHERE ID = {2}", coordenadas.Latitude.ToString(), coordenadas.Longitude.ToString(), item.ID);

                        bd.Executar(strSql);
                    }
                }
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

        private List<EstruturaPontoDeVenda> BuscarTodos()
        {
            try
            {
                List<EstruturaPontoDeVenda> retorno = new List<EstruturaPontoDeVenda>();

                string strSql = @"SELECT tl.ID, tl.Logradouro, tl.Numero, tl.CEP, tl.Cidade, tl.Estado, tl.Latitude, tl.Longitude FROM tLocal tl WHERE LEN(tl.Logradouro) > 1 AND LEN(tl.CEP) = 8 AND ( Latitude IS NULL OR Longitude IS NULL )";

                bd.Consulta(strSql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaPontoDeVenda()
                    {
                        ID = bd.LerInt("ID"),
                        CEP = bd.LerString("CEP"),
                        Numero = bd.LerInt("Numero"),
                        Endereco = bd.LerString("Logradouro"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado")
                    });
                }

                return retorno;
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

        public bool VerificarVendas()
        {
            try
            {
                string strSql = string.Format(@"SELECT TOP 1 ID FROM tIngresso ti WHERE ti.LocalID = {0} AND (ti.Status = 'V' OR ti.Status = 'I' OR ti.Status = 'E')", this.Control.ID);

                bd.Consulta(strSql);

                if (bd.Consulta().Read())
                    return true;
                else
                    return false;

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
    }

    public class LocalLista : LocalLista_B
    {
        public LocalLista() { }

        public LocalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioLocal");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Empresa", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Contato", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        Empresa e = new Empresa();
                        e.Ler(local.EmpresaID.Valor);
                        linha["Empresa"] = e.Nome.Valor;
                        linha["Nome"] = local.Nome.Valor;
                        linha["Contato"] = local.Contato.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ToSelectIN()
        {
            System.Text.StringBuilder retorno = new System.Text.StringBuilder();
            for (int i = 0; i <= this.lista.Count - 1; i++)
                retorno.Append(this.lista[i] + ",");

            if (retorno.Length > 0)
                return retorno.ToString().Substring(0, retorno.Length - 1);
            else
                return string.Empty;




        }
        public DataTable LocaisDisponivelAjuste()
        {
            //Retorna um DataTable com os Locais que tenham pelo menos
            //uma apresentacao com DisponivelAjuste = T
            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT " +
                            "DISTINCT tl.ID, tl.Nome " +
                            "FROM " +
                            "tLocal tl " +
                            "INNER JOIN tEvento te ON te.LocalID=tl.ID " +
                            "INNER JOIN tApresentacao ta ON ta.EventoID=te.ID " +
                            "WHERE ta.DisponivelAjuste='T' ORDER BY tl.Nome ";

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

        public DataTable LocaisDisponivelAjuste(int regionalID)
        {
            //Retorna um DataTable com os Locais que tenham pelo menos
            //uma apresentacao com DisponivelAjuste = T da regionalID definida
            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT " +
                            "DISTINCT tl.ID, tl.Nome " +
                            "FROM " +
                            "tLocal tl " +
                            "INNER JOIN tEvento te ON te.LocalID=tl.ID " +
                            "INNER JOIN tApresentacao ta ON ta.EventoID=te.ID " +
                            "INNER JOIN tEmpresa(NOLOCK) ON tEmpresa.ID = tl.EmpresaID " +
                            "WHERE ta.DisponivelAjuste='T' AND tEmpresa.RegionalID = " + regionalID + " ORDER BY tl.Nome ";

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

        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tLocal WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY EmpresaID, Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
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
    }
}
