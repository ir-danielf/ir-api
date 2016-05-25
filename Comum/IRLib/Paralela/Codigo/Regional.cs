/**************************************************
* Arquivo: Regional.cs
* Gerado: 03/07/2008
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ClientObjects.Arvore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace IRLib.Paralela
{

    public class Regional : Regional_B
    {
        public List<EstruturaRegionalArea> Areas { get; set; }

        public Regional() { }

        public Regional(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter todas as imagens de mapas
        /// </summary>
        /// <returns></returns>
        public DataTable Todos()
        {
            return Todos(null);
        }

        /// <summary>
        /// Obter todas as imagens de mapas
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Regionais");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT r.ID, r.Nome FROM tRegional r(NOLOCK)                                
                                ORDER By r.Nome";

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { "0", registroZero });

                bd.Consulta(sql);



                DataRow dr = null;

                while (bd.Consulta().Read())
                {
                    dr = tabela.NewRow();
                    dr["ID"] = bd.LerInt("ID");
                    dr["Nome"] = bd.LerString("Nome");

                    tabela.Rows.Add(dr);
                }

                return tabela;


            }
            finally { bd.Fechar(); }
        }

        public DataTable Locais(int regionalID)
        {
            string sql = @"SELECT DISTINCT l.ID, l.Nome FROM tRegional r
                        INNER JOIN tEmpresa e ON r.ID = e.RegionalID
                        INNER JOIN tLocal l ON l.EmpresaID = e.ID
                        WHERE r.ID = " + regionalID;
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)bd.Cnn);
            da.SelectCommand = cmd;
            DataTable retorno = new DataTable("Local");
            da.Fill(retorno);
            return retorno;
        }

        public DataTable Empresas(int regionalID, string registroZero)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            if (registroZero != null)
                tabela.Rows.Add(new Object[] { 0, registroZero });
            string sql = "SELECT ID, Nome FROM tEmpresa WHERE RegionalID = " + regionalID + "ORDER BY Nome";
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

        public DataTable Empresas(int regionalID)
        {
            return Empresas(regionalID, null);
        }

        /// <summary>		
        /// Obter Eventos desse Regional
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND (ap.DisponivelVenda='T')" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND (ap.DisponivelAjuste='T')" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND (ap.DisponivelRelatorio='T')" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "  ev.ID, " +
                    "  ev.Nome " +
                    "FROM " +
                    "  tEvento AS ev " +
                    "INNER JOIN " +
                    "   tApresentacao AS ap " +
                    "ON " +
                    "   ev.ID = ap.EventoID " +
                    "INNER JOIN " +
                    "   tLocal AS lo " +
                    "ON " +
                    "   lo.ID = ev.LocalID " +
                    "INNER JOIN " +
                    "   tEmpresa AS em " +
                    "ON " +
                    "   em.ID = lo.EmpresaID " +
                    "WHERE " +
                    "  (em.RegionalID = " + this.Control.ID + ") " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY " +
                    "  ev.Nome"))
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> CarregarLista(bool registroZero)
        {
            try
            {
                List<IRLib.Paralela.ClientObjects.EstruturaIDNome> lista = new List<IRLib.Paralela.ClientObjects.EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.Paralela.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Todas" });

                bd.Consulta("SELECT ID, Nome FROM tRegional (NOLOCK) ORDER BY Nome");

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

        /// <summary>		
        /// Obter Regional e Emails
        /// </summary>
        /// <returns></returns>
        public List<IRLib.Paralela.EstruturaRegionalEmail> CarregarListaEmail()
        {
            try
            {
                List<IRLib.Paralela.EstruturaRegionalEmail> lista = new List<IRLib.Paralela.EstruturaRegionalEmail>();

                bd.Consulta(@"SELECT Nome, EmailAlertaRetirada FROM tRegional (NOLOCK)
                                WHERE EmailAlertaRetirada IS NOT NULL
                                ORDER BY Nome ");

                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.Paralela.EstruturaRegionalEmail()
                    {
                        Regional = bd.LerString("Nome"),
                        Email = bd.LerString("EmailAlertaRetirada"),
                    });
                }

                return lista;
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
        /// Obter Eventos desse Regional
        /// </summary>
        /// <returns></returns>
        public DataTable Empresas(int regionalID, string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("RegionalID", typeof(int));
            try
            {

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND (ap.DisponivelVenda='T')" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND (ap.DisponivelAjuste='T')" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND (ap.DisponivelRelatorio='T')" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "  em.ID, " +
                    "  em.Nome " +
                    "FROM " +
                    "  tEvento AS ev " +
                    "INNER JOIN " +
                    "   tApresentacao AS ap " +
                    "ON " +
                    "   ev.ID = ap.EventoID " +
                    "INNER JOIN " +
                    "   tLocal AS lo " +
                    "ON " +
                    "   lo.ID = ev.LocalID " +
                    "INNER JOIN " +
                    "   tEmpresa AS em " +
                    "ON " +
                    "   em.ID = lo.EmpresaID " +
                    "WHERE " +
                    "  (em.RegionalID = " + regionalID + ") " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY " +
                    "  em.Nome,em.ID"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["RegionalID"] = regionalID;
                        tabela.Rows.Add(linha);
                    }
                }

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

            return tabela;

        }
        /// <summary>		
        /// Obter Eventos desse Regional
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(Apresentacao.Disponibilidade disponibilidade)
        {
            return Eventos(null, disponibilidade);
        }

        /// <summary>
        /// Carrega a tabela temporária do banco de dados com os eventos buscados para o Regional
        /// </summary>
        public void CarregaEventosEmTemp(int regionalID, string sessionID)
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
                    @"' FROM tEvento AS e, tApresentacao AS a, tLocal AS lo, tEmpresa AS em  
					WHERE e.ID=a.EventoID AND lo.ID = e.LocalID AND em.ID = lo.EmpresaID AND em.RegionalID= " + regionalID + @" AND a.DisponivelRelatorio = 'T'
					ORDER BY e.Nome";

                bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter canais dessa regional
        /// </summary>
        /// <returns></returns>
        public DataTable Canais()
        {
            return Canais(null);
        }

        /// <summary>		
        /// Obter canais dessa regional
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(string registroZero)
        {

            DataTable tabela = new DataTable("Canal");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

            if (registroZero != null)
                tabela.Rows.Add(new Object[] { 0, registroZero, 0, 0 });

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome, " +
                    "   tCanal.TaxaConveniencia " +
                    "FROM " +
                    "   tCanal " +
                    "INNER JOIN " +
                    "   tEmpresa " +
                    "ON " +
                    "   tEmpresa.ID = tCanal.EmpresaID " +
                    "WHERE " +
                    "   (tEmpresa.RegionalID = " + this.Control.ID + ") " +
                    "ORDER BY " +
                    "   tCanal.Nome"))
                {

                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        tabela.Rows.Add(linha);
                    }
                }

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

            return tabela;

        }

        public DataTable DetalhesPorID(int RegionalID)
        {
            try
            {
                DataTable dtt = new DataTable("Regionais");
                dtt.Columns.Add("ID", typeof(int));
                dtt.Columns.Add("Nome", typeof(string));

                //Caso necessário, incluir os outros campos aqui!!
                bd.Consulta("SELECT ID, Nome FROM tRegional (NOLOCK) WHERE ID = " + RegionalID);

                if (bd.Consulta().Read())
                {
                    DataRow dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["Nome"] = bd.LerString("Nome");

                    dtt.Rows.Add(dtr);
                }

                return dtt;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaArvore CarregarArvore(bool CarregarRegistroZero, int RegionalID)
        {
            try
            {
                EstruturaArvore arvore = new EstruturaArvore();
                string sql =
                    string.Format(@"SELECT 
                        r.ID AS RegionalID, r.Nome AS Regional,
                        e.ID as EmpresaID, e.Nome AS Empresa,
                        l.ID AS LocalID, l.Nome AS Local
                    FROM tRegional r (NOLOCK)
                    INNER JOIN tEmpresa e (NOLOCK) ON e.RegionalID = r.ID
                    INNER JOIN tLocal l (NOLOCK) ON l.EmpresaID = e.ID
                    {0}
                    ORDER BY r.Nome, e.Nome, l.Nome", RegionalID > 0 ? "WHERE r.ID = " + RegionalID : string.Empty);

                bd.Consulta(sql);

                if (CarregarRegistroZero)
                {
                    arvore.Regionais.Add(new EstruturaArvoreItem() { ID = 0, Nome = "Selecione...", VinculoID = 0, });
                    arvore.Empresas.Add(new EstruturaArvoreItem() { ID = 0, Nome = "Selecione...", VinculoID = 0 });
                    arvore.Locais.Add(new EstruturaArvoreItem() { ID = 0, Nome = "Selecione...", VinculoID = 0 });
                }
                while (bd.Consulta().Read())
                {
                    if (arvore.Regionais.Where(c => c.ID == bd.LerInt("RegionalID")).Count() == 0)
                        arvore.Regionais.Add(new EstruturaArvoreItem() { ID = bd.LerInt("RegionalID"), Nome = bd.LerString("Regional"), });

                    if (arvore.Empresas.Where(c => c.ID == bd.LerInt("EmpresaID")).Count() == 0)
                        arvore.Empresas.Add(new EstruturaArvoreItem() { ID = bd.LerInt("EmpresaID"), Nome = bd.LerString("Empresa"), VinculoID = bd.LerInt("RegionalID"), });

                    if (arvore.Locais.Where(c => c.ID == bd.LerInt("LocalID")).Count() == 0)
                        arvore.Locais.Add(new EstruturaArvoreItem() { ID = bd.LerInt("LocalID"), Nome = bd.LerString("Local"), VinculoID = bd.LerInt("EmpresaID"), });
                }


                return arvore;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataSet EstruturaArvore()
        {
            DataSet ds = new DataSet();
            DataTable regionais = new DataTable("Regionais");
            regionais.Columns.Add("ID", typeof(int));
            regionais.Columns.Add("Nome", typeof(string));

            DataTable empresas = new DataTable("Empresas");
            empresas.Columns.Add("ID", typeof(int));
            empresas.Columns.Add("Nome", typeof(string));
            empresas.Columns.Add("RegionalID", typeof(int));

            DataTable locais = new DataTable("Locais");
            locais.Columns.Add("ID", typeof(int));
            locais.Columns.Add("Nome", typeof(string));
            locais.Columns.Add("EmpresaID", typeof(int));


            DataTable eventos = new DataTable("Eventos");
            eventos.Columns.Add("ID", typeof(int));
            eventos.Columns.Add("Nome", typeof(string));
            eventos.Columns.Add("LocalID", typeof(int));

            DataTable apresentacoes = new DataTable("Apresentacoes");
            apresentacoes.Columns.Add("ID", typeof(int));
            apresentacoes.Columns.Add("Horario", typeof(string));
            apresentacoes.Columns.Add("EventoID", typeof(int));

            DataTable setores = new DataTable("Setores");
            setores.Columns.Add("ID", typeof(int));
            setores.Columns.Add("Nome", typeof(string));
            setores.Columns.Add("ApresentacaoID", typeof(int));

            DataTable precos = new DataTable("Precos");
            precos.Columns.Add("ID", typeof(int));
            precos.Columns.Add("Nome", typeof(string));
            precos.Columns.Add("ApresentacaoID", typeof(int));
            precos.Columns.Add("SetorID", typeof(int));
            precos.Columns.Add("ApresentacaoSetorID", typeof(int));


            ds.Tables.Add(regionais);
            ds.Tables.Add(empresas);
            ds.Tables.Add(locais);
            ds.Tables.Add(eventos);
            ds.Tables.Add(apresentacoes);
            ds.Tables.Add(setores);
            ds.Tables.Add(precos);
            return ds;
        }

        public override void Ler(int id)
        {
            this.Areas = new RegionalArea().Listar(id);
            base.Ler(id);
        }

        public override bool Inserir(BD bd)
        {
            try
            {
                if (!this.Inserir())
                    throw new Exception("Não foi possível inserir o registro.");

                new RegionalArea(this.Control.UsuarioID).Gerenciar(bd, this.Control.ID, this.Areas);

                return true;
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

        public override bool Atualizar()
        {
            try
            {
                bd.IniciarTransacao();

                if (!this.Atualizar(bd))
                    throw new Exception("Não foi possível atualizar o registro.");

                bd.FinalizarTransacao();
                return true;
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

        public override bool Atualizar(BD bd)
        {
            new RegionalArea(this.Control.UsuarioID).Gerenciar(bd, this.Control.ID, this.Areas);
            return base.Atualizar(bd);
        }

        public override bool Excluir()
        {
            try
            {
                bd.IniciarTransacao();

                bool ok = this.Excluir(bd, this.Control.ID);

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

        public override bool Excluir(BD bd, int id)
        {
            new RegionalArea(this.Control.UsuarioID).ExcluirPorRegional(bd, this.Control.ID);
            return base.Excluir(bd, id);
        }

    }

    public class RegionalLista : RegionalLista_B
    {

        public RegionalLista() { }

        public RegionalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }




    }



}
