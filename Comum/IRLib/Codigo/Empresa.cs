/* ---------------------------------------------------------------
-- Arquivo Empresa.cs
-- Gerado em: segunda-feira, 28 de março de 2005
-- Autor: Celeritas Ltda
---------------------------------------------------------------- */

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace IRLib
{

    public class Empresa : Empresa_B
    {
        public const int INTERNET_EMPRESA_ID = 1;

        private int UsuarioIDLogado;

        public Empresa() { }

        public Empresa(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            UsuarioIDLogado = usuarioIDLogado;
        }

        public int MyProperty { get; set; }

        public override bool Excluir(int id)
        {

            base.Excluir();
            bool excluiuCanais = true,
                 excluiuLocais = true,
                 excluiuUsuarios = true;
            excluiuCanais = DeletaCanais(id);
            excluiuLocais = DeletaLocais(id);
            excluiuUsuarios = DeletaUsuarios(id);

            return (!(!excluiuCanais && !excluiuLocais && !excluiuUsuarios));

        }

        /// <summary>
        /// Carrega a lista de contas da empresa
        /// </summary>
        /// <param name="empresaID">ID da Empresa</param>
        /// <returns></returns>
        public List<ClientObjects.EstruturaEmpresaConta> CarregaListaContas(int empresaID)
        {
            List<ClientObjects.EstruturaEmpresaConta> contas = new List<IRLib.ClientObjects.EstruturaEmpresaConta>();

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tEmpresaConta.ID, " +
                    "   tEmpresa.ID AS EmpresaID, " +
                    "   tEmpresa.Nome AS EmpresaNome, " +
                    "   tEmpresaConta.Beneficiario, " +
                    "   tEmpresaConta.Banco, " +
                    "   tEmpresaConta.Agencia, " +
                    "   tEmpresaConta.Conta, " +
                    "   tEmpresaConta.CPFCNPJ " +
                    "FROM " +
                    "   tEmpresaConta (NOLOCK) " +
                    "INNER JOIN " +
                    "   tEmpresa (NOLOCK) " +
                    "ON " +
                    "   tEmpresa.ID = tEmpresaConta.EmpresaID " +
                    "WHERE " +
                    "   (tEmpresa.ID = " + empresaID + ") " +
                    "ORDER BY " +
                    "   tEmpresaConta.Beneficiario"))
                {
                    ClientObjects.EstruturaEmpresaConta contratoconta;

                    while (oDataReader.Read())
                    {
                        contratoconta = new ClientObjects.EstruturaEmpresaConta();
                        contratoconta.ID = bd.LerInt("ID");
                        contratoconta.EmpresaID = bd.LerInt("EmpresaID");
                        contratoconta.EmpresaNome = bd.LerString("EmpresaNome");
                        contratoconta.Beneficiario = bd.LerString("Beneficiario");
                        contratoconta.Banco = bd.LerString("Banco");
                        contratoconta.Agencia = bd.LerString("Agencia");
                        contratoconta.Conta = bd.LerString("Conta");
                        contratoconta.CPFCNPJ = bd.LerString("CPFCNPJ");

                        contas.Add(contratoconta);
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

            return contas;
        }

        /// <summary>
        /// Captura os prefixos do Tipo Empresa - LP 20090313 - 229
        /// </summary>
        /// <param name="prefixo">Nome do Prefixo</param>
        /// <param name="registroZero">Valor Padrão</param>
        /// <returns></returns>
        public DataTable CarregaPrefixos(string prefixo, string registroZero)
        {

            DataTable tabela = new DataTable("Prefixo");
            tabela.Columns.Add("Prefixo", typeof(string));
            tabela.Columns.Add("Tipo", typeof(string));

            try
            {
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { "", registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tPrefixo.Prefixo " +
                    "FROM " +
                    "   tPrefixo " +
                    "WHERE " +
                    "   (tPrefixo.Tipo = 'E') " +
                    ((prefixo != "") ? "" +
                    "AND " +
                    "   (tPrefixo.Prefixo = '" + prefixo + "') " : "") +
                    "ORDER BY " +
                    "   tPrefixo.Prefixo"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Prefixo"] = bd.LerString("Prefixo");
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
        public List<IRLib.ClientObjects.EstruturaIDNome> CarregarLista(int regionalID, bool registroZero)
        {
            try
            {
                List<IRLib.ClientObjects.EstruturaIDNome> lista = new List<IRLib.ClientObjects.EstruturaIDNome>();

                StringBuilder filter = new StringBuilder();

                if (regionalID > 0)
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" RegionalID = {0} ", regionalID);

                }

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Todos" });

                string sql = string.Format("SELECT ID, Nome FROM tEmpresa (NOLOCK) {0} ORDER BY Nome",
                                        filter.Length > 0 ? "WHERE " + filter.ToString() : string.Empty);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome()
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

        public List<IRLib.ClientObjects.EstruturaIDNome> CarregarListaAtivas(int regionalID, bool registroZero)
        {
            try
            {
                List<IRLib.ClientObjects.EstruturaIDNome> lista = new List<IRLib.ClientObjects.EstruturaIDNome>();

                StringBuilder filter = new StringBuilder();

                if (regionalID > 0)
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" RegionalID = {0} ", regionalID);

                }

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Todos" });

                string sql = string.Format("SELECT ID, Nome FROM tEmpresa (NOLOCK) WHERE Ativo='T' {0} ORDER BY Nome",
                                        filter.Length > 0 ? " AND " + filter.ToString() : string.Empty);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome()
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
        /// Carrega todas as empresas
        /// </summary>
        /// <returns></returns>
        public DataTable carregarEmpresasTodas()
        {
            /// Job: 202
            /// Autor: LP
            return carregarEmpresas(0, 0);
        }

        /// <summary>
        /// Carrega as empresas da regional        
        /// </summary>
        /// <param name="regionalID">Regional</param>
        /// <returns></returns>
        public DataTable carregarEmpresasPorRegional(int regionalID)
        {
            /// Job: 202
            /// Autor: LP
            return carregarEmpresas(regionalID, 0);
        }

        /// <summary>
        /// Carrega a empresa pelo ID        
        /// </summary>
        /// <param name="empresaID">Empresa</param>
        /// <returns></returns>
        public DataTable carregarEmpresasPorID(int empresaID)
        {
            /// Job: 202
            /// Autor: LP
            return carregarEmpresas(0, empresaID);
        }

        public BindingList<EstruturaIDNome> CarregarEmpresas(int ID, string registroZero)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT ID, Nome FROM tEmpresa (NOLOCK) ");
                if (ID > 0)
                    stbSQL.Append("WHERE ID = " + ID + " ");
                stbSQL.Append("ORDER BY Nome");


                bd.Consulta(stbSQL.ToString());

                BindingList<EstruturaIDNome> lista = new BindingList<EstruturaIDNome>();
                EstruturaIDNome item;

                if (registroZero.Length > 0)
                {
                    item = new EstruturaIDNome();
                    item.ID = 0;
                    item.Nome = registroZero;
                    lista.Add(item);
                }

                while (bd.Consulta().Read())
                {
                    item = new EstruturaIDNome();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    lista.Add(item);
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

        public BindingList<EstruturaIDNome> CarregarEmpresasComVir(int ID, string registroZero)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT tEmpresa.ID, tEmpresa.Nome FROM tEmpresa (NOLOCK) INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.EmpresaID = tEmpresa.ID ");
                if (ID > 0)
                    stbSQL.Append("WHERE tEmpresa.ID = " + ID + " ");
                stbSQL.Append("ORDER BY tEmpresa.Nome");


                bd.Consulta(stbSQL.ToString());

                BindingList<EstruturaIDNome> lista = new BindingList<EstruturaIDNome>();
                EstruturaIDNome item;

                if (registroZero.Length > 0)
                {
                    item = new EstruturaIDNome();
                    item.ID = 0;
                    item.Nome = registroZero;
                    lista.Add(item);
                }

                while (bd.Consulta().Read())
                {
                    item = new EstruturaIDNome();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    lista.Add(item);
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

        public BindingList<EstruturaIDNome> CarregarEmpresasAtivasComVir(int ID, string registroZero)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT tEmpresa.ID, tEmpresa.Nome " +
                              "FROM tEmpresa (NOLOCK) " +
                              "INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.EmpresaID = tEmpresa.ID " +
                              "WHERE tEmpresa.Ativo = 'T' ");
                if (ID > 0)
                    stbSQL.Append("AND tEmpresa.ID = " + ID + " ");
                stbSQL.Append("ORDER BY tEmpresa.Nome");


                bd.Consulta(stbSQL.ToString());

                BindingList<EstruturaIDNome> lista = new BindingList<EstruturaIDNome>();
                EstruturaIDNome item;

                if (registroZero.Length > 0)
                {
                    item = new EstruturaIDNome();
                    item.ID = 0;
                    item.Nome = registroZero;
                    lista.Add(item);
                }

                while (bd.Consulta().Read())
                {
                    item = new EstruturaIDNome();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    lista.Add(item);
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

        /// <summary>
        /// Carregar as Empresas de acordo com a Regional ou EmpresaID        
        /// </summary>
        /// <param name="regionalID">Regional</param>
        /// <param name="empresaID">Empresa</param>
        /// <returns>DataTable</returns>
        private DataTable carregarEmpresas(int regionalID, int empresaID)
        {
            /// Job: 202
            /// Autor: LP
            //DataTable dados = new DataTable();
            //dados.Columns.Add(new DataColumn("ID", typeof(int)));
            //dados.Columns.Add(new DataColumn("Nome", typeof(string)));

            //try
            //{
            //    using (IDataReader oDataReader = bd.Consulta("" +
            //        "SELECT DISTINCT " +
            //        "   tEmpresa.ID, " +
            //        "   tEmpresa.Nome " +
            //        "FROM tEmpresa (NOLOCK) " +
            //        "INNER JOIN tLocal (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID " +
            //        "INNER JOIN tEvento (NOLOCK) ON tEvento.LocalID = tLocal.ID " +
            //        "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
            //        "WHERE " +
            //        "   (tApresentacao.DisponivelAjuste = 'T') " +
            //        "AND " +
            //        "  (SUBSTRING(tApresentacao.Horario, 1, 8) >= '" + DateTime.Now.ToString("yyyyMMdd") + " ')" +
            //        ((regionalID != 0) ? "" +
            //        "AND " +
            //        "   (tEmpresa.RegionalID = " + regionalID + ") " : "") +
            //        ((empresaID != 0) ? "" +
            //        "AND " +
            //        "   (tEmpresa.ID = " + empresaID + ") " : "") +
            //        "ORDER BY " +
            //        "   tEmpresa.Nome"))
            //    {
            //        while (oDataReader.Read())
            //        {
            //            DataRow oDataRow = dados.NewRow();
            //            oDataRow["ID"] = bd.LerInt("ID");
            //            oDataRow["Nome"] = bd.LerString("Nome");
            //            dados.Rows.Add(oDataRow);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    bd.Fechar();
            //}

            DataTable dados = new DataTable();
            try
            {
                string selectQuery = @"SELECT DISTINCT 
                         tEmpresa.ID, tEmpresa.Nome 
                        FROM tEmpresa (NOLOCK) 
                        INNER JOIN tLocal (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID 
                        INNER JOIN tEvento (NOLOCK) ON tEvento.LocalID = tLocal.ID 
                        INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID 
                        WHERE (tApresentacao.DisponivelAjuste = 'T') AND (SUBSTRING(tApresentacao.Horario, 1, 8) >= '{0}') 
                        {1} {2}
                        ORDER BY 
                        tEmpresa.Nome";


                string selectRegional = regionalID != 0 ? " AND (tEmpresa.RegionalID = " + regionalID + ") " : "";
                string selectEmpresa = empresaID != 0 ? " AND (tEmpresa.ID = " + empresaID + ") " : "";


                selectQuery = string.Format(selectQuery, DateTime.Now.ToString("yyyyMMdd"), selectRegional, selectEmpresa);

                using (IDataReader oDataReader = bd.Consulta(selectQuery))
                {
                    dados.Load(oDataReader);
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

        public int BuscaEmpresaIDporLojaID(int lojaID)
        {
            BD bd = new BD();
            object retorno = bd.ConsultaValor("SELECT tEmpresa.ID FROM tEmpresa (NOLOCK) INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN tLoja (NOLOCK) ON tLoja.CanalID = tCanal.ID WHERE tLoja.ID = " + lojaID);
            if (retorno is int)
                return (int)retorno;
            throw new Exception("Empresa não encontrada!");
        }

        public string BuscaEmpresaNomeporLojaID(int lojaID)
        {
            BD bd = new BD();
            object retorno = bd.ConsultaValor("SELECT tEmpresa.Nome FROM tEmpresa (NOLOCK) INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID INNER JOIN tLoja (NOLOCK) ON tLoja.CanalID = tCanal.ID WHERE tLoja.ID = " + lojaID);
            return retorno != null ? retorno.ToString() : string.Empty;
        }

        public int BuscaEmpresaIDporLocalID(int localID)
        {
            BD bd = new BD();
            object retorno = bd.ConsultaValor("SELECT tEmpresa.ID FROM tLocal (NOLOCK), tEmpresa (NOLOCK) WHERE tLocal.EmpresaID = tEmpresa.ID AND tLocal.ID = " + localID);
            if (retorno is int)
                return (int)retorno;
            throw new Exception("Empresa não encontrada!");
        }

        private bool DeletaCanais(int id)
        {

            CanalLista canais = new CanalLista();
            canais.FiltroSQL = "EmpresaID = " + id;
            canais.Carregar();
            canais.Primeiro();

            while (canais.Canal.Excluir(canais.Canal.Control.ID))
                canais.Proximo();

            return (canais.Canal.Control.ID != 0);
        }

        private bool DeletaLocais(int id)
        {

            LocalLista locais = new LocalLista(UsuarioIDLogado);
            locais.FiltroSQL = "EmpresaID = " + id;
            locais.Carregar();
            locais.Primeiro();

            while (locais.Local.Excluir(locais.Local.Control.ID))
                locais.Proximo();

            return (locais.Local.Control.ID != 0);
        }

        private bool DeletaUsuarios(int id)
        {

            UsuarioLista usuarios = new UsuarioLista(UsuarioIDLogado);
            usuarios.FiltroSQL = "EmpresaID = " + id;
            usuarios.Carregar();
            usuarios.Primeiro();

            while (usuarios.Usuario.Excluir(usuarios.Usuario.Control.ID))
                usuarios.Proximo();

            return (usuarios.Usuario.Control.ID != 0);

        }

        public DataTable Generica(int tipo)
        {

            DataTable tabela;

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    tabela = Canais();
                    break;
                case PerfilTipo.EMPRESA:
                    tabela = Todas();
                    break;
                case PerfilTipo.ESPECIAL:
                    tabela = null;
                    break;
                case PerfilTipo.EVENTO:
                    tabela = Eventos(Apresentacao.Disponibilidade.Ajustar);
                    break;
                case PerfilTipo.LOCAL:
                    tabela = Locais();
                    break;
                default:
                    tabela = null;
                    break;
            }

            if (tabela != null)
            {
                tabela.Columns.Add("Tipo", typeof(string));

                switch (tipo)
                {
                    case PerfilTipo.CANAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    case PerfilTipo.EMPRESA:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Empresa";
                        break;
                    case PerfilTipo.EVENTO:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Evento";
                        break;
                    case PerfilTipo.LOCAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    default:
                        tabela = null;
                        break;
                }

            }

            return tabela;

        }

        public DataTable GenericaAtivos(int tipo)
        {

            DataTable tabela;

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    tabela = CanaisAtivos();
                    break;
                case PerfilTipo.EMPRESA:
                    tabela = Ativas();
                    break;
                case PerfilTipo.ESPECIAL:
                    tabela = null;
                    break;
                case PerfilTipo.EVENTO:
                    tabela = EventosAtivos(Apresentacao.Disponibilidade.Ajustar);
                    break;
                case PerfilTipo.LOCAL:
                    tabela = LocaisAtivos();
                    break;
                default:
                    tabela = null;
                    break;
            }

            if (tabela != null)
            {
                tabela.Columns.Add("Tipo", typeof(string));

                switch (tipo)
                {
                    case PerfilTipo.CANAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    case PerfilTipo.EMPRESA:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Empresa";
                        break;
                    case PerfilTipo.EVENTO:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Evento";
                        break;
                    case PerfilTipo.LOCAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    default:
                        tabela = null;
                        break;
                }

            }

            return tabela;

        }

        public DataTable Generica(int tipo, bool master)
        {

            DataTable tabela;

            if (!master)
            {
                tabela = Generica(tipo);
                return tabela;
            }

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    Canal canal = new Canal();
                    tabela = canal.Todos();
                    break;
                case PerfilTipo.EMPRESA:
                    tabela = Todas();
                    break;
                case PerfilTipo.ESPECIAL:
                    tabela = null;
                    break;
                case PerfilTipo.EVENTO:
                    Evento evento = new Evento();
                    tabela = evento.Todos();
                    break;
                case PerfilTipo.LOCAL:
                    Local local = new Local();
                    tabela = local.Todos();
                    break;
                default:
                    tabela = null;
                    break;
            }

            if (tabela != null)
            {
                tabela.Columns.Add("Tipo", typeof(string));

                switch (tipo)
                {
                    case PerfilTipo.CANAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    case PerfilTipo.EMPRESA:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Empresa";
                        break;
                    case PerfilTipo.EVENTO:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Evento";
                        break;
                    case PerfilTipo.LOCAL:
                        foreach (DataRow linha in tabela.Rows)
                            linha["Tipo"] = "Canal";
                        break;
                    default:
                        tabela = null;
                        break;
                }

            }

            return tabela;

        }

        /// <summary>		
        /// Obter todas as empresas
        /// </summary>
        /// <returns></returns>
        public DataTable Todas(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Empresa");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql = "SELECT ID, Nome FROM tEmpresa (NOLOCK) ORDER BY Nome";
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
        /// Obter todas as empresas ativas
        /// </summary>
        /// <returns></returns>
        public DataTable Ativas(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Empresa");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql = "SELECT ID, Nome FROM tEmpresa (NOLOCK) WHERE Ativo = 'T' ORDER BY Nome";
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
        /// Seleciona a Empresa de Acordo com o Filtro Indicado
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        private DataTable EmpresaConsulta(int empresaID)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("RegionalID", typeof(int));
            try
            {

                bd.Consulta("" +
                    "SELECT " +
                    "  tEmpresa.ID, " +
                    "  tEmpresa.Nome, " +
                    "  tEmpresa.RegionalID " +
                    "FROM " +
                    "  tEmpresa (NOLOCK) " +
                    "WHERE " +
                    "   (1 = 1) " +
                    ((empresaID != 0) ? "" +
                    "AND " +
                    "   (tEmpresa.ID = " + empresaID + ") " : "") +
                    "ORDER BY " +
                    "  tEmpresa.Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RegionalID"] = bd.LerInt("RegionalID");
                    tabela.Rows.Add(linha);
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
        /// Seleciona a Empresa de Acordo com o Filtro Indicado
        /// </summary>
        /// <param name="empresaID"></param>
        /// <returns></returns>
        private DataTable EmpresaConsulta(int empresaID, bool ativos)
        {
            //bool ativos = IRBilheteria.Referencia.MostrarAtivos();

            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("RegionalID", typeof(int));
            try
            {

                bd.Consulta("" +
                    "SELECT " +
                    "  tEmpresa.ID, " +
                    "  tEmpresa.Nome, " +
                    "  tEmpresa.RegionalID " +
                    "FROM " +
                    "  tEmpresa (NOLOCK) " +
                    "WHERE " +
                    "   (1 = 1) " +
                    ((empresaID != 0) ? "" +
                    "AND " +
                    "   (tEmpresa.ID = " + empresaID + ") " : "") +
                    ((ativos) ? "" +
                    "AND " +
                    "   (tEmpresa.Ativo = 'T') " : "") +
                    "ORDER BY " +
                    "  tEmpresa.Nome");

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RegionalID"] = bd.LerInt("RegionalID");
                    tabela.Rows.Add(linha);
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
        /// Obter todas as empresas
        /// </summary>
        /// <returns></returns>
        public override DataTable Todas()
        {
            return EmpresaConsulta(0);
        }

        public override DataTable Ativas()
        {
            return EmpresaConsulta(0, true);
        }

        /// <summary>		
        /// Obter a empresa com o ID indicado
        /// </summary>
        /// <returns></returns>
        public DataTable EmpresaIDNome(int empresaID)
        {
            return EmpresaConsulta(empresaID);
        }

        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel venda
        /// </summary>
        /// <returns></returns>
        public DataTable TodasDisponivelVenda(int regionalID)
        {
            try
            {
                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                string clausulaRegional = "";

                if (regionalID > 0)
                    clausulaRegional = " AND RegionalID = " + regionalID;

                string sql = @"SELECT DISTINCT tEmpresa.ID, tEmpresa.Nome
                        FROM tEmpresa, tLocal, tEvento, tApresentacao
                        WHERE tLocal.EmpresaID = tEmpresa.ID AND tEvento.ID = tApresentacao.EventoID AND
                        tEvento.LocalID = tLocal.ID AND tApresentacao.DisponivelVenda = 'T' " + clausulaRegional + " ORDER BY tEmpresa.Nome";

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

        /// Obter todas as empresas com apresentações disponivel Relatorio

        /// </summary>

        /// <returns></returns>

        public DataTable TodasDisponivelRelatorio()
        {
            try
            {
                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT tEmpresa.ID, tEmpresa.Nome
                        FROM tEmpresa (NOLOCK), tLocal (NOLOCK), tEvento (NOLOCK), tApresentacao (NOLOCK)
                        WHERE tLocal.EmpresaID = tEmpresa.ID AND tEvento.ID = tApresentacao.EventoID AND
                        tEvento.LocalID = tLocal.ID AND tApresentacao.DisponivelRelatorio = 'T' ORDER BY tEmpresa.Nome";

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
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable TodasDisponivelAjuste()
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tEmpresa.ID, " +
                    "   tEmpresa.Nome " +
                    "FROM " +
                    "   tEmpresa (NOLOCK), " +
                    "   tLocal (NOLOCK), " +
                    "   tEvento (NOLOCK), " +
                    "   tApresentacao (NOLOCK) " +
                    "WHERE " +
                    "   tLocal.EmpresaID = tEmpresa.ID " +
                    "AND " +
                    "   tEvento.ID = tApresentacao.EventoID " +
                    "AND " +
                    "  tEvento.LocalID = tLocal.ID " +
                    "AND " +
                    "  tApresentacao.DisponivelAjuste = 'T' " +
                    "ORDER BY " +
                    "  tEmpresa.Nome"))
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

        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable TodasPacoteDisponivelAjuste()
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tEmpresa.ID, " +
                    "   tEmpresa.Nome " +
                    "FROM " +
                    "   tEmpresa (NOLOCK), " +
                    "   tLocal (NOLOCK), " +
                    "   tEvento (NOLOCK), " +
                    "   tApresentacao (NOLOCK), " +
                    "   tPacote (NOLOCK), " +
                    "   tPacoteItem (NOLOCK) " +
                    "WHERE " +
                    "   tLocal.EmpresaID = tEmpresa.ID " +
                    "AND " +
                    "   tEvento.ID = tApresentacao.EventoID " +
                    "AND " +
                    "  tEvento.LocalID = tLocal.ID " +
                    "AND " +
                    "   tPacote.LocalID = tLocal.ID " +
                    "AND " +
                    "   tPacoteItem.PacoteID = tPacote.ID " +
                    "AND " +
                    "   tPacoteItem.ApresentacaoID = tApresentacao.ID " +
                    "AND " +
                    "  tApresentacao.DisponivelAjuste = 'T' " +
                    "ORDER BY " +
                    "  tEmpresa.Nome"))
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

            
        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable TodasDisponivelAjuste(int pRegionalID, string registroZero)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            if (registroZero != null)
                tabela.Rows.Add(new Object[] { "0", registroZero });

            try
            {
                using (IDataReader oDataReader = bd.Consulta(String.Format(@"SELECT DISTINCT 
                                                                               tEmpresa.ID, 
                                                                               tEmpresa.Nome
                                                                            FROM 
                                                                               tEmpresa (NOLOCK), 
                                                                               tRegional (NOLOCK), 
                                                                               tLocal (NOLOCK), 
                                                                               tEvento (NOLOCK),
                                                                               tApresentacao (NOLOCK) 
                                                                            WHERE 
                                                                               tLocal.EmpresaID = tEmpresa.ID 
                                                                            AND 
                                                                               tEvento.ID = tApresentacao.EventoID 
                                                                            AND 
                                                                              tEvento.LocalID = tLocal.ID 
                                                                            AND   
	                                                                            tEmpresa.RegionalID = tRegional.ID
                                                                            AND   
                                                                              tApresentacao.DisponivelAjuste = 'T' 
                                                                            AND
                                                                              tRegional.Id = {0}
                                                                            ORDER BY 
                                                                            tEmpresa.Nome", pRegionalID)))
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



        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable TodasDisponivelAjuste(int pRegionalID)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta(String.Format(@"SELECT DISTINCT 
                                                                               tEmpresa.ID, 
                                                                               tEmpresa.Nome
                                                                            FROM 
                                                                               tEmpresa (NOLOCK), 
                                                                               tRegional (NOLOCK), 
                                                                               tLocal (NOLOCK), 
                                                                               tEvento (NOLOCK),
                                                                               tApresentacao (NOLOCK) 
                                                                            WHERE 
                                                                               tLocal.EmpresaID = tEmpresa.ID 
                                                                            AND 
                                                                               tEvento.ID = tApresentacao.EventoID 
                                                                            AND 
                                                                              tEvento.LocalID = tLocal.ID 
                                                                            AND   
	                                                                            tEmpresa.RegionalID = tRegional.ID
                                                                            AND   
                                                                              tApresentacao.DisponivelAjuste = 'T' 
                                                                            AND
                                                                              tRegional.Id = {0}
                                                                            ORDER BY 
                                                                            tEmpresa.Nome", pRegionalID)))
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




        /// <summary>       
        /// Obter todas as empresas ativas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable AtivasDisponivelAjuste(int pRegionalID, string registroZero)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            if (registroZero != null)
                tabela.Rows.Add(new Object[] { "0", registroZero });

            try
            {
                using (IDataReader oDataReader = bd.Consulta(String.Format(@"SELECT DISTINCT 
                                                                               tEmpresa.ID, 
                                                                               tEmpresa.Nome
                                                                            FROM 
                                                                               tEmpresa (NOLOCK)
                                                                            INNER JOIN
                                                                               tRegional (NOLOCK)
                                                                            ON
                                                                               tEmpresa.RegionalID = tRegional.ID
                                                                            INNER JOIN
                                                                               tLocal (NOLOCK)
                                                                            ON
                                                                               tLocal.EmpresaID = tEmpresa.ID 
                                                                            INNER JOIN
                                                                               tEvento (NOLOCK)
                                                                            ON
                                                                               tEvento.LocalID = tLocal.ID
                                                                            INNER JOIN
                                                                               tApresentacao (NOLOCK)
                                                                            ON
                                                                               tEvento.ID = tApresentacao.EventoID 
                                                                            WHERE   
                                                                              tApresentacao.DisponivelAjuste = 'T' 
                                                                            AND   
                                                                              tEmpresa.Ativo = 'T' 
                                                                            AND
                                                                              tRegional.Id = {0}
                                                                            ORDER BY 
                                                                            tEmpresa.Nome", pRegionalID)))
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

        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable AtivasDisponivelAjuste(int pRegionalID)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                using (IDataReader oDataReader = bd.Consulta(String.Format(@"SELECT DISTINCT 
                                                                               tEmpresa.ID, 
                                                                               tEmpresa.Nome
                                                                            FROM 
                                                                               tEmpresa (NOLOCK)
                                                                            INNER JOIN
                                                                               tRegional (NOLOCK)
                                                                            ON
                                                                               tEmpresa.RegionalID = tRegional.ID
                                                                            INNER JOIN
                                                                               tLocal (NOLOCK)
                                                                            ON
                                                                               tLocal.EmpresaID = tEmpresa.ID 
                                                                            INNER JOIN
                                                                               tEvento (NOLOCK)
                                                                            ON
                                                                               tEvento.LocalID = tLocal.ID
                                                                            INNER JOIN
                                                                               tApresentacao (NOLOCK)
                                                                            ON
                                                                               tEvento.ID = tApresentacao.EventoID 
                                                                            WHERE   
                                                                              tApresentacao.DisponivelAjuste = 'T' 
                                                                            AND   
                                                                              tEmpresa.Ativo = 'T' 
                                                                            AND
                                                                              tRegional.Id = {0}
                                                                            ORDER BY 
                                                                            tEmpresa.Nome", pRegionalID)))
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

        /// <summary>       
        /// Obter todas as empresas com apresentações disponivel Ajuste
        /// </summary>
        /// <returns></returns>
        public DataTable TodasPacoteDisponivelAjustePorRegional(int RegionalID)
        {
            DataTable tabela = new DataTable("Empresa");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                string sql = String.Format(@"SELECT DISTINCT 
                       tEmpresa.ID, 
                       tEmpresa.Nome 
                    FROM 
                       tEmpresa (NOLOCK), 
                       tLocal (NOLOCK), 
                       tEvento (NOLOCK), 
                       tApresentacao (NOLOCK), 
                       tPacote (NOLOCK), 
                       tPacoteItem (NOLOCK) 
                    WHERE tEmpresa.RegionalID = {0} AND 
                       tLocal.EmpresaID = tEmpresa.ID 
                    AND 
                       tEvento.ID = tApresentacao.EventoID 
                    AND 
                      tEvento.LocalID = tLocal.ID 
                    AND 
                       tPacote.LocalID = tLocal.ID 
                    AND 
                       tPacoteItem.PacoteID = tPacote.ID 
                    AND 
                       tPacoteItem.ApresentacaoID = tApresentacao.ID 
                    AND 
                      tApresentacao.DisponivelAjuste = 'T' 
                    ORDER BY 
                      tEmpresa.Nome", RegionalID);

                using (IDataReader oDataReader = bd.Consulta(sql))
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


        /// <summary>
        /// Preenche os atributos Nome,EmpresaVende,EmpresaPromove - mais usado em relatorios
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerNomePromoveVende(int id)
        {

            try
            {
                string sql = "SELECT ID,Nome,EmpresaPromove,EmpresaVende FROM tEmpresa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.EmpresaPromove.ValorBD = bd.LerString("EmpresaPromove");
                    this.EmpresaVende.ValorBD = bd.LerString("EmpresaVende");
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
        /// Obter todas as empresas
        /// </summary>
        /// <returns></returns>
        public DataTable CarregarEmpresasPorRegionalID(int regionalID, string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RegionalID", typeof(int));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, regionalID });

                string sql = "SELECT ID, Nome FROM tEmpresa WHERE RegionalID = " + regionalID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        }


        /// <summary>		
        /// Obter todas as empresas
        /// </summary>
        /// <returns></returns>
        public DataTable CarregarEmpresasAtivasPorRegionalID(int regionalID, string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RegionalID", typeof(int));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, regionalID });

                string sql = "SELECT ID, Nome FROM tEmpresa WHERE RegionalID = " + regionalID + " AND Ativo = 'T' ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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

        }

        /// <summary>		
        ///Obter pacotes dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Pacotes()
        {
            return this.Pacotes(Apresentacao.Disponibilidade.Nula);
        }

        /// <summary>		
        ///Obter pacotes dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Pacotes(Apresentacao.Disponibilidade disponibilidade)
        {

            DataTable tabela = new DataTable("Pacote");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {


                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND (tApresentacao.DisponivelVenda='T') " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND (tApresentacao.DisponivelAjuste='T') " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND (tApresentacao.DisponivelRelatorio='T') " : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "  tPacote.ID, " +
                    "  tPacote.Nome " +
                    "FROM tPacote (NOLOCK) " +
                    "INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tPacote.LocalID " +
                    "INNER JOIN tPacoteItem (NOLOCK) ON tPacoteItem.PacoteID = tPacote.ID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tPacoteItem.ApresentacaoID = tApresentacao.ID " +
                    "WHERE " +
                    "  tLocal.EmpresaID = " + this.Control.ID + " " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY " +
                    "  tPacote.Nome"))
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


        /// <summary>		
        ///Obter precos dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Precos()
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tPreco.ID, tPreco.Nome FROM tPreco,tLocal " +
                    "WHERE tLocal.ID=tPreco.LocalID AND tLocal.EmpresaID=" + this.Control.ID + " " +
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

        public DataTable Eventos(string registroZero)
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
                string sql = "SELECT e.ID,e.Nome,e.VersaoImagemIngresso,e.VersaoImagemVale,e.VersaoImagemVale2,e.VersaoImagemVale3 FROM tEvento as e,tLocal " +
                    "WHERE tLocal.ID=e.LocalID AND tLocal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY e.Nome";
                bd.Consulta(sql);
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
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
        } // fim de Eventos


        /// <summary>		
        /// Obter Eventos desse local
        /// </summary>
        /// <returns></returns>
        public DataTable Eventos(Apresentacao.Disponibilidade disponibilidade)
        {
            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

            try
            {
                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   e.ID, " +
                    "   e.Nome " +
                    "FROM tEvento (NOLOCK) AS e " +
                    "INNER JOIN tLocal (NOLOCK) AS l ON l.ID = e.LocalID " +
                    "INNER JOIN tApresentacao (NOLOCK) AS a ON e.ID=a.EventoID " +
                    "WHERE l.EmpresaID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome"))
                {
                    DataRow linha;
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
        public DataTable EventosAtivos(Apresentacao.Disponibilidade disponibilidade)
        {
            DataTable tabela = new DataTable("Evento");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

            try
            {
                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   e.ID, " +
                    "   e.Nome " +
                    "FROM tEvento (NOLOCK) AS e " +
                    "INNER JOIN tLocal (NOLOCK) AS l ON l.ID = e.LocalID " +
                    "INNER JOIN tApresentacao (NOLOCK) AS a ON e.ID=a.EventoID " +
                    "WHERE e.Ativo = 'T' AND l.EmpresaID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome"))
                {
                    DataRow linha;
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

        public DataTable Eventos(string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = "SELECT DISTINCT e.ID, e.Nome " +
                    "FROM tEvento AS e,tLocal AS l, tApresentacao AS a " +
                    "WHERE l.ID=e.LocalID AND e.ID=a.EventoID AND l.EmpresaID=" + this.Control.ID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    " ORDER BY e.Nome";

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
        ///Obter eventos dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Eventos()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT e.ID,e.Nome " +
                    "FROM tEvento as e,tLocal " +
                    "WHERE tLocal.ID=e.LocalID AND tLocal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY e.Nome";

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
        ///Obter eventos ativos dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable EventosAtivos()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT e.ID,e.Nome " +
                    "FROM tEvento as e INNER JOIN tLocal " +
                    "ON tLocal.ID = e.LocalID " +
                    "WHERE e.Ativo='T' AND tLocal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY e.Nome";

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

        public DataTable Eventos(int EmpresaID, string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;


                string sql = "SELECT e.ID,e.Nome " +
                    "FROM tEvento as e,tLocal " +
                    "WHERE tLocal.ID=e.LocalID AND tLocal.EmpresaID=" + EmpresaID +
                    "ORDER BY e.Nome";

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
        /// eventos pela empresaID e disponibilidade. Passar null no registroZero para nao adicionar a linha.
        /// </summary>
        /// <param name="EmpresaID"></param>
        /// <param name="registroZero"></param>
        /// <param name="disponibilidade"></param>
        /// <returns></returns>
        public DataTable Eventos(int EmpresaID, string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";


                string sql = "SELECT DISTINCT e.ID,e.Nome " +
                    "FROM tEvento as e,tLocal,tApresentacao as a " +
                    "WHERE tLocal.ID=e.LocalID AND a.EventoID = e.ID AND tLocal.EmpresaID=" + EmpresaID +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY e.Nome";

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

        public DataTable EventosParaQualquerEmpresa(int EmpresaID, string registroZero)
        {
            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT tEvento.ID,tEvento.Nome
								FROM tEmpresa
								INNER JOIN tCanal ON tCanal.EmpresaID = tEmpresa.ID
								INNER JOIN tCanalEvento ON tCanal.ID = tCanalEvento.CanalID
								INNER JOIN tEvento ON tEvento.ID = tCanalEvento.EventoID
								INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID
								WHERE tEmpresa.ID =" + EmpresaID + " OR tLocal.EmpresaID =" + EmpresaID + " ORDER BY tEvento.Nome";

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
        public DataTable EventosParaQualquerEmpresaDisponivelRelatorio(int EmpresaID, string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT tEvento.ID,tEvento.Nome
								FROM tEmpresa
								INNER JOIN tCanal (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID
								INNER JOIN tCanalEvento (NOLOCK) ON tCanal.ID = tCanalEvento.CanalID
								INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tCanalEvento.EventoID
								INNER JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
                                INNER JOIN tApresentacao (NOLOCK) ON tEvento.ID = tApresentacao.EventoID
								WHERE tApresentacao.DisponivelRelatorio = 'T' AND tEmpresa.ID =" + EmpresaID + " OR tLocal.EmpresaID =" + EmpresaID + " ORDER BY tEvento.Nome";

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

        public DataTable EventosParaQualquerEmpresa(int EmpresaID, string registroZero, Apresentacao.Disponibilidade disponibilidade)
        {
            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T'" : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T'" : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T'" : "";

                string sql = @"SELECT DISTINCT tEvento.ID,tEvento.Nome
								FROM tEmpresa
								INNER JOIN tCanal ON tCanal.EmpresaID = tEmpresa.ID
								INNER JOIN tCanalEvento ON tCanal.ID = tCanalEvento.CanalID
								INNER JOIN tEvento ON tEvento.ID = tCanalEvento.EventoID
								INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID
								INNER JOIN tApresentacao a ON a.EventoID = tEvento.ID
								WHERE (tEmpresa.ID =" + EmpresaID + " OR tLocal.EmpresaID =" + EmpresaID + ") " +
                                disponivelVenda +
                                disponivelAjuste +
                                disponivelRelatorio +
                                " ORDER BY tEvento.Nome";

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
        /// Carrega todos os eventos em uma tabela temporária, com ID, Nome e SessionID para a Empresa.
        /// </summary>
        /// <param name="sessionID"></param>
        public void CarregaEventoEmTemp(int empresaID, string sessionID)
        {
            try
            {
                string sql = string.Empty;

                //limpar a tabela
                sql = "DELETE FROM tIRWebEventos WHERE SessionID = '" + sessionID + "'";

                bd.Executar(sql);

                sql =
                    @"INSERT INTO tIRWebEventos (ID,Nome,SessionID)
					SELECT DISTINCT e.ID,e.Nome, '" + sessionID +
                    @"' FROM tEvento AS e,tLocal,tApresentacao AS a 
					WHERE tLocal.ID=e.LocalID AND e.ID = a.EventoID AND tLocal.EmpresaID=" + empresaID + @" AND a.DisponivelRelatorio = 'T'
					ORDER BY e.Nome";

                bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter os pedidos de estoque dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable PedidosEstoque()
        {

            try
            {

                DataTable tabela = new DataTable("EstoquePedido");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Ordem", typeof(int));

                string sql = "SELECT ID, Ordem FROM tEstoquePedido WHERE EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY Ordem";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Ordem"] = bd.LerInt("Ordem");
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
        /// Obter os fornecedores dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Fornecedores()
        {

            try
            {
                DataTable tabela = new DataTable("Fornecedor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tFornecedor WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter as transferencias de estoque dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Transferencias()
        {
            return null;
        }

        /// <summary>		
        /// Obter ajustes de estoque dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Ajustes()
        {
            return null;
        }

        /// <summary>		
        /// Obter motivos de ajustes de estoque dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable AjusteMotivos()
        {

            try
            {

                DataTable tabela = new DataTable("EstoqueAjusteMotivo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tEstoqueAjusteMotivo " +
                    "WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter categorias de produtos dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Categorias()
        {

            try
            {

                DataTable tabela = new DataTable("Categoria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tProdutoCategoria " +
                    "WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter produtos dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Produtos()
        {


            try
            {

                DataTable tabela = new DataTable("Produto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                string sql = "SELECT ID,Nome,PrecoVenda FROM tProduto " +
                    "WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("PrecoVenda");
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
        /// Obter produtos dessa empresa dada uma categoria
        /// </summary>
        /// <returns></returns>
        public override DataTable Produtos(int categoriaid)
        {

            try
            {

                DataTable tabela = new DataTable("Produto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                string sql = "SELECT ID,Nome,PrecoVenda FROM tProduto " +
                    "WHERE ProdutoCategoriaID=" + categoriaid + " AND EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("PrecoVenda");
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
        /// Obter locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Locais(bool comRegistroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (comRegistroZero)
                    tabela.Rows.Add(new Object[] { 0, "" });

                string sql = "SELECT ID,Nome FROM tLocal WHERE EmpresaID=" + this.Control.ID + " " +
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
        /// Obter locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Locais(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID,Nome FROM tLocal WHERE EmpresaID=" + this.Control.ID + " " +
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

        public DataTable Locais(string registroZero, int EmpresaID)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID,Nome FROM tLocal WHERE EmpresaID=" + EmpresaID + " ORDER BY Nome";

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
        /// Traz os locais de uma determinada empresa que estejam disponíveis para relatório
        /// </summary>
        /// <param name="registroZero">O registro que vai estar na primeira linha do 
        /// DataTable de retorno, geralmente usada para popular a primeira linha do ComboBox</param>
        /// <param name="EmpresaID"></param>
        /// <returns></returns>
        public DataTable LocaisDisponivelRelatorio(string registroZero, int EmpresaID)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT DISTINCT tLocal.ID,tLocal.Nome FROM tLocal(NOLOCK),tEvento(NOLOCK),tApresentacao(NOLOCK) WHERE " +
                              "tLocal.ID = tEvento.LocalID AND tEvento.ID = tApresentacao.EventoID AND " +
                              "tApresentacao.DisponivelRelatorio = 'T' AND EmpresaID=" + EmpresaID + " ORDER BY tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
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
        /// <summary>
        /// Traz os locais de uma determinada empresa que estejam disponíveis para relatório
        /// </summary>
        /// <param name="registroZero">O registro que vai estar na primeira linha do 
        /// DataTable de retorno, geralmente usada para popular a primeira linha do ComboBox</param>
        /// <param name="EmpresaID"></param>
        /// <returns></returns>
        public DataTable Locais(string registroZero, int EmpresaID, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " tApresentacao.DisponivelVenda='T' " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " tApresentacao.DisponivelAjuste='T' " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " tApresentacao.DisponivelRelatorio='T' " : "";

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT DISTINCT tLocal.ID,tLocal.Nome FROM tLocal(NOLOCK),tEvento(NOLOCK),tApresentacao(NOLOCK) WHERE " +
                              "tLocal.ID = tEvento.LocalID AND tEvento.ID = tApresentacao.EventoID AND " +
                              disponivelVenda + disponivelAjuste + disponivelRelatorio +
                              " AND EmpresaID=" + EmpresaID + " ORDER BY tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EmpresaID"] = EmpresaID;
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

        /// <summary>		
        /// Obter locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Locais()
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID,Nome FROM tLocal " +
                    "WHERE EmpresaID=" + this.Control.ID + " " +
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
        /// Traz os locais ativos de uma determinada empresa que estejam disponíveis para relatório
        /// </summary>
        /// <param name="registroZero">O registro que vai estar na primeira linha do 
        /// DataTable de retorno, geralmente usada para popular a primeira linha do ComboBox</param>
        /// <param name="EmpresaID"></param>
        /// <returns></returns>
        public DataTable LocaisAtivos(string registroZero, int EmpresaID, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int));

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " tApresentacao.DisponivelVenda='T' " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " tApresentacao.DisponivelAjuste='T' " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " tApresentacao.DisponivelRelatorio='T' " : "";

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT DISTINCT tLocal.ID,tLocal.Nome " +
                              "FROM tLocal(NOLOCK) INNER JOIN tEvento(NOLOCK) ON tLocal.ID = tEvento.LocalID " +
                              "INNER JOIN tApresentacao(NOLOCK) ON tEvento.ID = tApresentacao.EventoID " +
                              "WHERE tLocal.Ativo = 'T' AND " +
                              disponivelVenda + disponivelAjuste + disponivelRelatorio +
                              " AND EmpresaID=" + EmpresaID + " ORDER BY tLocal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["EmpresaID"] = EmpresaID;
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

        /// <summary>		
        /// Obter locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable LocaisAtivos()
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT ID,Nome FROM tLocal " +
                    "WHERE EmpresaID=" + this.Control.ID + " AND Ativo='T' " +
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
        /// Obter eventos que são vendidos por esta empresa (pdv)
        /// Válido só para Relatório disponivel true
        /// </summary>
        /// <returns></returns>
        public override DataTable EventosQueVendem(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                /*string sql = 
                    "SELECT DISTINCT tEvento.Nome, tEvento.ID "+
                    "FROM            tEmpresa INNER JOIN "+
                                                    "tCanal ON tEmpresa.ID = tCanal.ID INNER JOIN "+
                                                    "tCanalEvento ON tCanal.ID = tCanalEvento.CanalID INNER JOIN "+
                                                    "tEvento ON tCanalEvento.EventoID = tEvento.ID INNER JOIN "+
                                                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID "+
                    "WHERE        (tCanal.EmpresaID = "+this.Control.ID+") AND (tApresentacao.DisponivelRelatorio = 'T') "+
                    "UNION "+
                    "SELECT DISTINCT tEvento.Nome, tEvento.ID "+
                    "FROM            tEmpresa INNER JOIN "+
                                                    "tCanal ON tEmpresa.ID = tCanal.EmpresaID INNER JOIN "+
                                                    "tCanalPacote ON tCanal.ID = tCanalPacote.CanalID INNER JOIN "+
                                                    "tPacote ON tCanalPacote.PacoteID = tPacote.ID INNER JOIN "+
                                                    "tLocal ON tPacote.LocalID = tLocal.ID INNER JOIN "+
                                                    "tEvento ON tLocal.ID = tEvento.LocalID INNER JOIN "+
                                                    "tApresentacao ON tEvento.ID = tApresentacao.EventoID "+
					
                    "WHERE        (tEmpresa.ID = "+this.Control.ID+") AND (tApresentacao.DisponivelRelatorio = 'T') ";*/



                string sql = @"(select tevento.id, tevento.nome
				from tevento,tcanalevento,tcanal,tempresa,tapresentacao
				where (tcanalevento.eventoid=tevento.id and tcanalevento.canalid=tcanal.id and tcanal.empresaid=tempresa.id and tapresentacao.eventoid=tevento.id and tapresentacao.disponivelrelatorio='T' and tempresa.id=" + this.Control.ID + @")
				union
				select tevento.id, tevento.nome from tevento,tcanal,tempresa,tapresentacao,tcanalpacote,tpacote,tpacoteitem,tpreco,tapresentacaosetor
				where tcanal.empresaid=tempresa.id and tempresa.id=" + this.Control.ID + @" 
				and tcanalpacote.pacoteid=tpacote.id and tpacote.id=tpacoteitem.pacoteid and tpreco.id=tpacoteitem.precoid and tapresentacaosetor.id=tpreco.apresentacaosetorid and tapresentacaosetor.apresentacaoid=tapresentacao.id and tapresentacao.eventoid=tevento.id and tapresentacao.disponivelrelatorio='T')
				order by tevento.nome";


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
        } // EventosQueVendem
        /// <summary>		
        /// Obter canais que vendem shows desta empresa 
        /// </summary>
        /// <returns></returns>
        public string CanaisIR()
        {
            try
            {
                string canais = "";
                DataTable canaisQueVendemTabela = this.CanaisQueVendem(null);
                DataTable canaisTabela = this.Canais(null);
                bool encontrado = false;
                bool primeiraVez = true;
                for (int indice = 0; indice < canaisQueVendemTabela.Rows.Count; indice++)
                {
                    // Dos que vendem não considerar os da própria empresa que promove
                    foreach (DataRow linhaCanaisTabela in canaisTabela.Rows)
                    {
                        if (Convert.ToInt32(linhaCanaisTabela["ID"]) ==
                            Convert.ToInt32(canaisQueVendemTabela.Rows[indice]["ID"]))
                        {
                            encontrado = true;
                        }
                    }
                    if (!encontrado)
                    {
                        if (primeiraVez)
                        {
                            canais = canaisQueVendemTabela.Rows[indice]["ID"].ToString();
                            primeiraVez = false;
                        }
                        else
                            canais = canais + "," + canaisQueVendemTabela.Rows[indice]["ID"].ToString();
                    }
                    encontrado = false;
                }
                return canais;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } // CanaisQueVendem
        /// <summary>		
        /// Obter canais que vendem shows desta empresa 
        /// Inclui os canais da própria empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable CanaisQueVendem(string registroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Canal");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });
                string sql =
                    "SELECT        tCanal.Nome + ', ' + tEmpresa.Nome AS CanalEmpresa, tCanal.ID " +
                    "FROM            tCanal INNER JOIN " +
                                                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE        (tCanal.EmpresaID = " + this.Control.ID + ") " +
                    "UNION " +
                    "SELECT DISTINCT tCanal.Nome + ', ' + tEmpresa.Nome AS CanalEmpresa, tCanal.ID " +
                    "FROM            tCanal INNER JOIN " +
                                                    "tCanalPacote ON tCanal.ID = tCanalPacote.CanalID INNER JOIN " +
                                                    "tPacote ON tCanalPacote.PacoteID = tPacote.ID INNER JOIN " +
                                                    "tLocal ON tPacote.LocalID = tLocal.ID INNER JOIN " +
                                                    "tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE        (tLocal.EmpresaID = " + this.Control.ID + ") " +
                    "UNION " +
                    "SELECT DISTINCT tCanal.Nome + ', ' + tEmpresa.Nome AS CanalEmpresa, tCanal.ID " +
                    "FROM            tLocal INNER JOIN " +
                                                    "tEvento ON tLocal.ID = tEvento.LocalID INNER JOIN " +
                                                    "tCanalEvento ON tEvento.ID = tCanalEvento.EventoID INNER JOIN " +
                                                    "tCanal ON tCanalEvento.CanalID = tCanal.ID INNER JOIN " +
                                                    "tEmpresa tEmpresa ON tCanal.EmpresaID = tEmpresa.ID " +
                    "WHERE        (tLocal.EmpresaID = " + this.Control.ID + ") " +
                    "ORDER BY tCanal.Nome + ', ' + tEmpresa.Nome ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("CanalEmpresa");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } // CanaisQueVendem
        /// <summary>		
        /// Obter canais dessa empresa
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

                string sql = "SELECT ID,Nome FROM tCanal " +
                    "WHERE EmpresaID=" + this.Control.ID + " " +
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
        /// Obter canais ativos dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisAtivos(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT ID,Nome FROM tCanal " +
                    "WHERE EmpresaID=" + this.Control.ID + " AND Ativo = 'T' " +
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
        /// Obter canais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Canais()
        {

            DataTable tabela = new DataTable("Canal");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "  ID, " +
                    "  Nome, " +
                    "  TaxaConveniencia " +
                    "FROM tCanal (NOLOCK) " +
                    "WHERE " +
                    "  EmpresaID = " + this.Control.ID + " " +
                    "ORDER BY " +
                    "  Nome"))
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

        public override DataTable CanaisAtivos()
        {

            DataTable tabela = new DataTable("Canal");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "  ID, " +
                    "  Nome, " +
                    "  TaxaConveniencia " +
                    "FROM tCanal (NOLOCK) " +
                    "WHERE " +
                    "  EmpresaID = " + this.Control.ID + " AND Ativo = 'T' " +
                    "ORDER BY " +
                    "  Nome"))
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
        /// <summary>		
        /// Obter canais dessa empresa recebendo a EmpresaID
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(int empresaID)
        {

            DataTable tabela = new DataTable("Canal");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("TaxaMinima", typeof(decimal));
            tabela.Columns.Add("TaxaMaxima", typeof(decimal));
            tabela.Columns.Add("TaxaComissao", typeof(int));
            tabela.Columns.Add("ComissaoMinima", typeof(decimal));
            tabela.Columns.Add("ComissaoMaxima", typeof(decimal));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = empresaID;

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   ID, " +
                    "   Nome, " +
                    "   TaxaConveniencia, " +
                    "   TaxaComissao, " +
                    "   TaxaMinima, " +
                    "   TaxaMaxima, " +
                    "   ComissaoMinima, " +
                    "   ComissaoMaxima " +
                    "FROM tCanal (NOLOCK) " +
                    "WHERE EmpresaID=" + empresaID + " " +
                    "ORDER BY Nome"))
                {

                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                        linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                        linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                        linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                        linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
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
        /// Obter canais ativos dessa empresa recebendo a EmpresaID
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisAtivos(int empresaID)
        {

            DataTable tabela = new DataTable("Canal");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("TaxaMinima", typeof(decimal));
            tabela.Columns.Add("TaxaMaxima", typeof(decimal));
            tabela.Columns.Add("TaxaComissao", typeof(int));
            tabela.Columns.Add("ComissaoMinima", typeof(decimal));
            tabela.Columns.Add("ComissaoMaxima", typeof(decimal));
            tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = empresaID;

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   ID, " +
                    "   Nome, " +
                    "   TaxaConveniencia, " +
                    "   TaxaComissao, " +
                    "   TaxaMinima, " +
                    "   TaxaMaxima, " +
                    "   ComissaoMinima, " +
                    "   ComissaoMaxima " +
                    "FROM tCanal (NOLOCK) " +
                    "WHERE Ativo = 'T' AND EmpresaID=" + empresaID + " " +
                    "ORDER BY Nome"))
                {

                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["TaxaConveniencia"] = bd.LerInt("TaxaConveniencia");
                        linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                        linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                        linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                        linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                        linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
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
        /// Obter canais dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Canais2()
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("EmpresaID", typeof(int)).DefaultValue = this.Control.ID;

                string sql = "SELECT tCanal.ID,tCanal.Nome,tEmpresa.Nome AS Empresa " +
                    "FROM tCanal,tEmpresa " +
                    "WHERE tCanal.EmpresaID=tEmpresa.ID AND tCanal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + "/" + bd.LerString("Empresa");
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
        /// Obter canais dessa empresa por tipo do canal
        /// </summary>
        /// <returns></returns>
        public override DataTable Canais(char tipo)
        {

            try
            {

                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID,Nome FROM tLocal " +
                    "WHERE Tipo='" + tipo + "' AND EmpresaID=" + this.Control.ID + " " +
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
        /// Obter logins dos usuarios dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Logins()
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Login", typeof(string));

                string sql = "SELECT ID, Login FROM tUsuario " +
                    "WHERE EmpresaID=" + this.Control.ID + " AND Status<>'B' " +
                    "ORDER BY Login";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Login"] = bd.LerString("Login");
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
        /// Obter logins dos usuarios das empresas por RegionalID
        /// </summary>
        /// <returns></returns>
        public DataTable Logins(int regionalID)
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Login", typeof(string));

                string sql = "SELECT u.ID, u.Login" +
                        " FROM tUsuario u" +
                        " INNER JOIN tEmpresa e ON e.ID = u.EmpresaID" +
                        " WHERE Status<>'B' AND e.RegionalID = " + regionalID +
                        " ORDER BY Login";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Login"] = bd.LerString("Login");
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
        /// Obter usuarios dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Usuarios()
        {

            try
            {

                DataTable tabela = new DataTable("Usuario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tUsuario WHERE EmpresaID=" + this.Control.ID + " " +
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

        public DataTable Usuarios(string comRegistroZero)
        {
            try
            {
                DataTable tabela = new DataTable("Usuario");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                string sql = "SELECT ID, Nome FROM tUsuario WHERE EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY Nome";
                bd.Consulta(sql);
                if (comRegistroZero != "")
                    tabela.Rows.Add(new Object[] { 0, comRegistroZero });
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
        /// Obter garcons dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Garcons(bool comRegistroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Garcon");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                if (comRegistroZero)
                    tabela.Rows.Add(new Object[] { 0, "(não selecionado)" });

                string sql = "SELECT ID, Nome FROM tGarcon WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter garcons dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Garcons()
        {

            try
            {

                DataTable tabela = new DataTable("Garcon");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ID, Nome FROM tGarcon WHERE EmpresaID=" + this.Control.ID + " ORDER BY Nome";

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
        /// Obter as apresentacoes de todos os locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Apresentacoes()
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));

                string sql = "SELECT tApresentacao.ID, tApresentacao.Horario FROM tApresentacao,tLocal,tEvento " +
                    "WHERE tEvento.ID=tApresentacao.EventoID AND tLocal.ID=tEvento.LocalID AND tLocal.EmpresaID=" + this.Control.ID + " ORDER BY tApresentacao.Horario";

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
        /// Obter canais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Lojas()
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tLoja.ID, tLoja.Nome " +
                    "FROM tLoja,tCanal " +
                    "WHERE tCanal.ID=tLoja.CanalID AND tCanal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY tLoja.Nome";

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
        /// Obter canais dessa empresa
        /// </summary>
        /// <returns></returns>
        public DataTable Lojas(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                string sql = "SELECT tLoja.ID, tLoja.Nome " +
                    "FROM tLoja,tCanal " +
                    "WHERE tCanal.ID=tLoja.CanalID AND tCanal.EmpresaID=" + this.Control.ID + " " +
                    "ORDER BY tLoja.Nome";

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
        /// Obter os estoques de todos os locais dessa empresa
        /// </summary>
        /// <returns></returns>
        public override DataTable Estoques()
        {

            try
            {

                DataTable tabela = new DataTable("Estoque");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tEstoque.ID, tEstoque.Nome FROM tEstoque,tLocal " +
                    "WHERE tLocal.ID=tEstoque.LocalID AND tLocal.EmpresaID=" + this.Control.ID + " ORDER BY tEstoque.Nome";

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

        public bool ehPDVIR(int empresaID)
        {
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT EmpresaPromove, EmpresaVende FROM tEmpresa(NOLOCK) WHERE ID = " + empresaID);
                if (!bd.Consulta().Read())
                {
                    bd.Fechar();
                    return false;
                }
                else
                {
                    if (!bd.LerBoolean("EmpresaPromove") && bd.LerBoolean("EmpresaVende"))
                    {
                        bd.Fechar();
                        return true;
                    }
                    else
                    {
                        bd.Fechar();
                        return false;
                    }
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
        }


        public DataTable FormasPagamento(int empresaID)
        {
            string sql = @"SELECT tFormaPagamento.ID, tFormaPagamento.Nome FROM tEmpresaFormaPagamento 
                        INNER JOIN tFormaPagamento ON tFormaPagamento.ID = tEmpresaFormaPagamento.FormaPagamentoID
                        WHERE EmpresaID = " + empresaID;
            BD bd = new BD();
            DataTable retorno = new DataTable();
            retorno.Columns.Add("ID", typeof(int));
            retorno.Columns.Add("Nome", typeof(string));
            DataRow newRow;
            while (bd.Consulta(sql).Read())
            {
                newRow = retorno.NewRow();
                newRow["ID"] = bd.LerInt("ID");
                newRow["Nome"] = bd.LerString("Nome");
                retorno.Rows.Add(newRow);
            }

            return retorno;
        }

        public object FormasPagamento(int empresaID, string registroZero)
        {
            string sql = @"SELECT tFormaPagamento.ID, tFormaPagamento.Nome FROM tEmpresaFormaPagamento 
                        INNER JOIN tFormaPagamento ON tFormaPagamento.ID = tEmpresaFormaPagamento.FormaPagamentoID
                        WHERE EmpresaID = " + empresaID;
            BD bd = new BD();
            DataTable retorno = new DataTable();
            retorno.Columns.Add("ID", typeof(int));
            retorno.Columns.Add("Nome", typeof(string));

            DataRow newRow;

            if (registroZero != null)
            {
                newRow = retorno.NewRow();
                newRow["ID"] = -1;
                newRow["Nome"] = registroZero;
                retorno.Rows.Add(newRow);
            }

            while (bd.Consulta(sql).Read())
            {
                newRow = retorno.NewRow();
                newRow["ID"] = bd.LerInt("ID");
                newRow["Nome"] = bd.LerString("Nome");
                retorno.Rows.Add(newRow);
            }

            return retorno;
        }

        public object Canais(int empresaID, string registroZero)
        {
            DataTable tabela = new DataTable("Canal");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            if (registroZero != null)
                tabela.Rows.Add(new Object[] { 0, registroZero });

            string sql = @"SELECT DISTINCT ID, Nome
                FROM tCanal WHERE EmpresaID=" + empresaID + @"
                ORDER BY Nome";

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

        public List<EstruturaIDNome> CarregarEmpresasVendaRemota(string CanaisID)
        {
            try
            {
                List<EstruturaIDNome> lstIDNome = new List<EstruturaIDNome>();
                StringBuilder stbSQL = new StringBuilder();
                EstruturaIDNome item;

                IRLib.Canal canal = new Canal();

                stbSQL.Append("     SELECT DISTINCT e.ID, e.Nome ");
                stbSQL.Append("       FROM tEmpresa (NOLOCK) e ");
                stbSQL.Append(" INNER JOIN tCanal c (NOLOCK) ON c.EmpresaID = e.ID ");
                stbSQL.Append("      WHERE c.TipoVenda = '" + (char)Canal.TipoDeVenda.VendaRemota + "' ");
                if (CanaisID.Length > 0)
                    stbSQL.Append("        AND c.ID IN (" + CanaisID + ")");
                stbSQL.Append("   ORDER BY e.Nome ");

                item = new EstruturaIDNome();
                item.ID = 0;
                item.Nome = "Selecione...";
                lstIDNome.Add(item);

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    item = new EstruturaIDNome();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    lstIDNome.Add(item);
                }

                return lstIDNome;
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

        public string CarregaTaxa(int empresaID)
        {
            string taxa = string.Empty;
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT TaxaMaximaEmpresa FROM tEmpresa WHERE ID = " + empresaID);

                while (bd.Consulta().Read())
                    taxa = Convert.ToString(bd.LerDecimal("TaxaMaximaEmpresa"));

                return taxa;
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

        public string CarregaTaxaPorLocal(int LocalID)
        {
            string taxa = string.Empty;
            BD bd = new BD();
            try
            {
                bd.Consulta("select TaxaMaximaEmpresa from tEmpresa where ID =  (select EmpresaID from tLocal where ID= " + LocalID + ")");

                while (bd.Consulta().Read())
                    taxa = Convert.ToString(bd.LerDecimal("TaxaMaximaEmpresa"));

                return taxa;
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

    public class EmpresaLista : EmpresaLista_B
    {

        public EmpresaLista() { }

        public EmpresaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioEmpresa");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Contato", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = empresa.Nome.Valor;
                        linha["Contato"] = empresa.ContatoNome.Valor;
                        if (empresa.DDDTelefone.Valor != "")
                            linha["Telefone"] = "(" + empresa.DDDTelefone.Valor + ") " + empresa.Telefone.Valor;
                        else
                            linha["Telefone"] = empresa.Telefone.Valor;

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
        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tEmpresa WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY Nome");

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
