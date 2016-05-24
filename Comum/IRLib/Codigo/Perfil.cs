/* ---------------------------------------------------------------
-- Arquivo Perfil.cs
-- Gerado em: segunda-feira, 28 de março de 2005
-- Autor: Celeritas Ltda
---------------------------------------------------------------- */


using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{

    public class Perfil : Perfil_B
    {

        public const int EMPRESA_SEGURANCA = 1; //tipo emprea
        public const int EMPRESA_FINANCEIRO = 2; //tipo emprea
        public const int EMPRESA_REGRAS = 3; //tipo emprea
        public const int LOCAL_IMPLANTAREVENTO = 4; //tipo local
        public const int LOCAL_FINANCEIRO = 5; //tipo local
        public const int CANAL_FINANCEIRO = 6; //tipo canal
        public const int CANAL_SUPERVISOR = 7; //tipo canal
        public const int CANAL_BILHETEIRO = 8; //tipo canal
        public const int EVENTO_FINANCEIRO = 9; //tipo evento
        public const int ESPECIAL_MASTER = 10; //tipo especial
        public const int LOCAL_VENDASAEB = 11;    //tipo local  
        public const int LOCAL_SUPERVISORAEB = 12; //tipo local
        public const int LOCAL_ESTOQUE = 13; //tipo local
        public const int ESPECIAL_IMPLANTAREVENTO = 14; //tipo especial
        public const int ESPECIAL_FINANCEIRO = 15; //tipo especial
        public const int LOCAL_SUPERVISOR = 16; //tipo local
        public const int ESPECIAL_LOGISTICA = 17; //tipo especial
        public const int EVENTO_SUPERVISOR = 18; //tipo evento
        public const int ESPECIAL_SUPERVISORWEB = 19; //tipo especial
        public const int REGIONAL_IMPLANTAREVENTO = 20; //tipo regional 
        public const int REGIONAL_MASTER = 21; //tipo regional 
        //public const int CONTROLE_ACESSO = 22; //tipo controle de acesso
        public const int EMPRESA_MASTER_CORTESIAS = 23; //tipo master cortesias da empresa
        public const int AREA_CENTRALIZADOR = 24; //tipo centralizador da area
        public const int AREA_USUARIO = 25; //tipo usuario da area
        public const int REGIONAL_FINANCEIRO = 26; //tipo regional financeiro
        public const int CONTROLE_ACESSO = 27; // tipo especial
        public const int SAC_SUPERVISOR = 28; // tipo especial
        public const int LOGISTICA = 29; // tipo Regional
        public const int SEGURANCA_ESPECIAL = 30; // tipo especial
        public const int SAC_OPERADOR = 31; // tipo especial
        public const int MEDIAPARTNER_ESP = 32; // tipo especial
        public const int MEDIAPARTNER_EMP = 33; // tipo Empresa
        public const int SAC_OPERADOR_NOVO = 34; // tipo especial
        public const int SAC_SUPERVISOR_NOVO = 35; // tipo especial

        private int lojaID = 0;
        private int canalID = 0;
        private int localID = 0;
        private int eventoID = 0;
        private int tipoID = 0;
        private int regionalID = 0;

        public Perfil() { }

        public Perfil(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public int CanalID
        {
            get { return canalID; }
            set { canalID = value; }
        }

        public int CanalTipoID { get; set; }
        public int CanalIDSupervisor { get; set; }

        public int LocalID
        {
            get { return localID; }
            set { localID = value; }
        }

        public int EventoID
        {
            get { return eventoID; }
            set { eventoID = value; }
        }

        public int LojaID
        {
            get { return lojaID; }
            set { lojaID = value; }
        }

        public int RegionalID
        {
            get { return regionalID; }
            set { regionalID = value; }
        }

        public int EmpresaID
        {
            get;
            set;
        }

        public bool Master
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == ESPECIAL_MASTER);
                }
                else
                    throw new PerfilException("Não foi possível identificar o perfil. Perfil não lido.");
            }
        }

        public bool RegionalMaster
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == REGIONAL_MASTER);
                }
                else
                    throw new PerfilException("Não foi possível identificar o perfil. Perfil não lido.");
            }

        }

        public bool RegionalImplantarEvento
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == REGIONAL_IMPLANTAREVENTO);
                }
                else
                    throw new PerfilException("Não foi possível identificar o perfil. Perfil não lido.");
            }

        }

        public bool EspecialImplantarEvento
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == ESPECIAL_IMPLANTAREVENTO);
                }
                else
                    throw new PerfilException("Não foi possível identificar o perfil. Perfil não lido.");
            }
        }

        public bool EspecialSupervisorWEB
        {
            get
            {
                if (this.Control.ID != 0)
                {
                    return (this.Control.ID == ESPECIAL_SUPERVISORWEB);
                }
                else
                    throw new PerfilException("Não foi possível identificar o perfil. Perfil não lido.");
            }
        }

        public bool EhSAC
        {
            get
            {
                return this.Control.ID == SAC_SUPERVISOR;
            }
        }

        public bool EhEspecial
        {
            get
            {
                return (this.Control.ID == ESPECIAL_FINANCEIRO) || (this.Control.ID == ESPECIAL_IMPLANTAREVENTO) || (this.Control.ID == ESPECIAL_LOGISTICA) || (this.Control.ID == ESPECIAL_MASTER) || (this.Control.ID == ESPECIAL_SUPERVISORWEB);
            }
        }


        public bool EhLogisticaEspecial
        {
            get
            {
                return this.Control.ID == ESPECIAL_LOGISTICA;
            }
        }


        public bool Bilheteiro
        {
            get
            {
                if (this.Control.ID == 0)
                    throw new Exception("Não foi possível identificar o perfil.");

                return (this.Control.ID == CANAL_BILHETEIRO);

            }
        }

        public bool SupervisorCanal
        {
            get
            {
                if (this.Control.ID == 0)
                    throw new Exception("Não foi possível identificar o perfil.");

                return (this.Control.ID == CANAL_SUPERVISOR);

            }
        }

        public bool LocalImplantarEvento
        {
            get
            {
                if (this.Control.ID == 0)
                    throw new Exception("Não foi possível identificar o perfil.");

                return (this.Control.ID == LOCAL_IMPLANTAREVENTO);

            }
        }

        public bool FinanceiroLocal
        {
            get
            {
                if (this.Control.ID == 0)
                    throw new Exception("Não foi possível identificar o perfil.");

                return (this.Control.ID == LOCAL_FINANCEIRO);

            }
        }



        public int TipoID(int usuarioid)
        {

            if (tipoID != 0)
                return tipoID;

            // *************************
            // Este método é falho
            //	Exemplo, posso ter Usuário com 2 perfis de Locais diferentes, 
            // e o método retorna para LocalID =0 ??

            switch (this.PerfilTipoID.Valor)
            {
                case PerfilTipo.CANAL:
                    PerfilCanalLista perfilCanalLista = new PerfilCanalLista();
                    perfilCanalLista.FiltroSQL = "PerfilID=" + this.Control.ID;
                    perfilCanalLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilCanalLista.Carregar();
                    if (perfilCanalLista.Tamanho == 1)
                    {
                        perfilCanalLista.Primeiro();
                        tipoID = perfilCanalLista.PerfilCanal.CanalID.Valor;
                    }
                    else
                    {
                        tipoID = 0;
                    }
                    break;
                case PerfilTipo.EMPRESA:
                    PerfilEmpresaLista perfilEmpresaLista = new PerfilEmpresaLista();
                    perfilEmpresaLista.FiltroSQL = "PerfilID=" + this.Control.ID;
                    perfilEmpresaLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilEmpresaLista.Carregar();
                    if (perfilEmpresaLista.Tamanho == 1)
                    {
                        perfilEmpresaLista.Primeiro();
                        tipoID = perfilEmpresaLista.PerfilEmpresa.EmpresaID.Valor;
                    }
                    else
                    {
                        tipoID = 0;
                    }
                    break;
                case PerfilTipo.ESPECIAL:
                    tipoID = 5;
                    break;
                case PerfilTipo.REGIONAL:
                    tipoID = 0;
                    break;
                case PerfilTipo.EVENTO:
                    PerfilEventoLista perfilEventoLista = new PerfilEventoLista();
                    perfilEventoLista.FiltroSQL = "PerfilID=" + this.Control.ID;
                    perfilEventoLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilEventoLista.Carregar();
                    if (perfilEventoLista.Tamanho == 1)
                    {
                        perfilEventoLista.Primeiro();
                        tipoID = perfilEventoLista.PerfilEvento.EventoID.Valor;
                    }
                    else
                    {
                        tipoID = 0;
                    }
                    break;
                case PerfilTipo.LOCAL:
                    PerfilLocalLista perfilLocalLista = new PerfilLocalLista();
                    perfilLocalLista.FiltroSQL = "PerfilID=" + this.Control.ID;
                    perfilLocalLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilLocalLista.Carregar();
                    if (perfilLocalLista.Tamanho == 1)
                    {
                        perfilLocalLista.Primeiro();
                        tipoID = perfilLocalLista.PerfilLocal.LocalID.Valor;
                    }
                    else
                    {
                        tipoID = 0;
                    }
                    break;
                default:
                    tipoID = 0;
                    break;
            }

            return tipoID;

        }

        /// <summary>
        /// Retorna um objeto do tipo correto dependendo do tipo passado como parametro e ja adidiona o id
        /// </summary>
        /// <param name="tipo">tipo empresa, por exemplo</param>
        /// <param name="tipoid">id da empresa, por exemplo</param>
        /// <param name="perfilid">id d perfil</param>
        /// <param name="usuarioid">id do usuario</param>
        /// <returns></returns>
        public IBaseBD TipoGenerico(int tipo, int tipoid, int perfilid, int usuarioid)
        {

            IBaseBD perfil;

            //TODO: porque aqui as condicoes sao perfilCanalLista.Tamanho==0 ?????

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    PerfilCanalLista perfilCanalLista = new PerfilCanalLista();
                    perfilCanalLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilCanalLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilCanalLista.FiltroSQL = "CanalID=" + tipoid;
                    perfilCanalLista.Carregar();
                    if (perfilCanalLista.Tamanho == 0)
                    {
                        PerfilCanal can = new PerfilCanal(this.Control.UsuarioID);
                        can.PerfilID.Valor = perfilid;
                        can.UsuarioID.Valor = usuarioid;
                        can.CanalID.Valor = tipoid;
                        perfil = can;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;
                case PerfilTipo.EMPRESA:
                    PerfilEmpresaLista perfilEmpresaLista = new PerfilEmpresaLista();
                    perfilEmpresaLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilEmpresaLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilEmpresaLista.FiltroSQL = "EmpresaID=" + tipoid;
                    perfilEmpresaLista.Carregar();
                    if (perfilEmpresaLista.Tamanho == 0)
                    {
                        PerfilEmpresa emp = new PerfilEmpresa(this.Control.UsuarioID);
                        emp.PerfilID.Valor = perfilid;
                        emp.UsuarioID.Valor = usuarioid;
                        emp.EmpresaID.Valor = tipoid;
                        perfil = emp;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;
                case PerfilTipo.ESPECIAL:
                    PerfilEspecialLista perfilEspecialLista = new PerfilEspecialLista();
                    perfilEspecialLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilEspecialLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilEspecialLista.Carregar();
                    if (perfilEspecialLista.Tamanho == 0)
                    {
                        PerfilEspecial esp = new PerfilEspecial(this.Control.UsuarioID);
                        esp.PerfilID.Valor = perfilid;
                        esp.UsuarioID.Valor = usuarioid;
                        perfil = esp;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;
                case PerfilTipo.EVENTO:
                    PerfilEventoLista perfilEventoLista = new PerfilEventoLista();
                    perfilEventoLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilEventoLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilEventoLista.FiltroSQL = "EventoID=" + tipoid;
                    perfilEventoLista.Carregar();
                    if (perfilEventoLista.Tamanho == 0)
                    {
                        PerfilEvento eve = new PerfilEvento(this.Control.UsuarioID);
                        eve.PerfilID.Valor = perfilid;
                        eve.UsuarioID.Valor = usuarioid;
                        eve.EventoID.Valor = tipoid;
                        perfil = eve;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;
                case PerfilTipo.LOCAL:
                    PerfilLocalLista perfilLocalLista = new PerfilLocalLista();
                    perfilLocalLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilLocalLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilLocalLista.FiltroSQL = "LocalID=" + tipoid;
                    perfilLocalLista.Carregar();
                    if (perfilLocalLista.Tamanho == 0)
                    {
                        PerfilLocal loc = new PerfilLocal(this.Control.UsuarioID);
                        loc.PerfilID.Valor = perfilid;
                        loc.UsuarioID.Valor = usuarioid;
                        loc.LocalID.Valor = tipoid;
                        perfil = loc;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;

                case PerfilTipo.REGIONAL:
                    PerfilRegionalLista perfilRegionalLista = new PerfilRegionalLista();
                    perfilRegionalLista.FiltroSQL = "PerfilID=" + perfilid;
                    perfilRegionalLista.FiltroSQL = "UsuarioID=" + usuarioid;
                    perfilRegionalLista.FiltroSQL = "RegionalID=" + tipoid;
                    perfilRegionalLista.Carregar();
                    if (perfilRegionalLista.Tamanho == 0)
                    {
                        PerfilRegional loc = new PerfilRegional(this.Control.UsuarioID);
                        loc.PerfilID.Valor = perfilid;
                        loc.UsuarioID.Valor = usuarioid;
                        loc.RegionalID.Valor = tipoid;
                        perfil = loc;
                    }
                    else
                    {
                        perfil = null;
                    }
                    break;
                default:
                    perfil = null;
                    break;
            }

            return perfil;

        }


        /// <summary>		
        ///Devolve uma lista de tipos de perfil.
        /// </summary>
        /// <returns></returns>
        public override DataTable Tipos(bool master)
        {

            try
            {

                DataTable tabela = new DataTable("PerfilTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql;

                if (master)
                    sql = "SELECT ID,Nome FROM tPerfilTipo ORDER BY Nome";
                else
                    sql = "SELECT ID,Nome FROM tPerfilTipo WHERE Nome NOT IN ('Especial','Regional') ORDER BY Nome";

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

        public DataTable Tipos(bool master, string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("PerfilTipo");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql;

                if (master)
                    sql = "SELECT ID,Nome FROM tPerfilTipo ORDER BY Nome";
                else
                    sql = "SELECT ID,Nome FROM tPerfilTipo WHERE Nome NOT IN ('Especial','Regional') ORDER BY Nome";

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
        /// Obtem os logins dos usuarios desse perfil especifico
        /// </summary>
        /// <param name="tipo">Tipo de perfil: empresa, canal, especial, evento...</param>
        /// <param name="perfilid">ID do perfil</param>
        /// <param name="id">ID do Tipo</param>
        /// <returns></returns>
        public override DataTable Logins(int tipo, int perfilid, int id)
        {

            DataTable tabela = null;

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    PerfilCanal perfilCanal = new PerfilCanal();
                    tabela = perfilCanal.Logins(perfilid, id);
                    break;
                case PerfilTipo.EMPRESA:
                    PerfilEmpresa perfilEmpresa = new PerfilEmpresa();
                    tabela = perfilEmpresa.Logins(perfilid, id);
                    break;
                case PerfilTipo.ESPECIAL:
                    PerfilEspecial perfilEspecial = new PerfilEspecial();
                    tabela = perfilEspecial.Logins(perfilid);
                    break;
                case PerfilTipo.EVENTO:
                    PerfilEvento perfilEvento = new PerfilEvento();
                    tabela = perfilEvento.Logins(perfilid, id);
                    break;
                case PerfilTipo.LOCAL:
                    PerfilLocal perfilLocal = new PerfilLocal();
                    tabela = perfilLocal.Logins(perfilid, id);
                    break;
                case PerfilTipo.REGIONAL:
                    PerfilRegional perfilRegional = new PerfilRegional();
                    tabela = perfilRegional.Logins(perfilid, id);
                    break;
            }

            return tabela;

        }

        /// <summary>		
        ///Devolve uma lista de todos os perfis
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(bool master)
        {

            try
            {

                DataTable tabela = new DataTable("Perfil");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PerfilTipoID", typeof(int));

                string sql;

                if (master)
                    sql = "SELECT p.ID,p.Nome,p.PerfilTipoID,pt.Nome as Tipo FROM " +
                        "tPerfil as p,tPerfilTipo as pt WHERE pt.ID=p.PerfilTipoID " +
                        "ORDER BY p.Nome";
                else
                    sql = "SELECT p.ID,p.Nome,p.PerfilTipoID,pt.Nome as Tipo FROM " +
                        "tPerfil as p,tPerfilTipo as pt WHERE pt.ID=p.PerfilTipoID AND " +
                        "p.ID<>" + ESPECIAL_MASTER + " AND p.ID<>" + REGIONAL_FINANCEIRO + " ORDER BY p.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Tipo");
                    linha["PerfilTipoID"] = bd.LerInt("PerfilTipoID");
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
        ///Devolve uma lista de todos os perfis
        /// </summary>
        /// <returns></returns>
        public DataTable Todos(bool master, string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Perfil");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PerfilTipoID", typeof(int));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero, 0 });

                string sql;

                if (master)
                    sql = "SELECT p.ID,p.Nome,p.PerfilTipoID,pt.Nome as Tipo FROM " +
                        "tPerfil as p,tPerfilTipo as pt WHERE pt.ID=p.PerfilTipoID " +
                        "ORDER BY p.Nome";
                else
                    sql = "SELECT p.ID,p.Nome,p.PerfilTipoID,pt.Nome as Tipo FROM " +
                        "tPerfil as p,tPerfilTipo as pt WHERE pt.ID=p.PerfilTipoID AND " +
                        "p.ID<>" + ESPECIAL_MASTER + " AND p.ID<>" + REGIONAL_FINANCEIRO + " ORDER BY p.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + " - " + bd.LerString("Tipo");
                    linha["PerfilTipoID"] = bd.LerInt("PerfilTipoID");
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

        public int PegarPerfilTipoID(string perfilID, string tipoID, string usuarioID, int tipo)
        {
            try
            {
                int id = 0;
                string sql = "";

                switch (tipo)
                {
                    case 1:
                        sql = "SELECT ID FROM tPerfilEmpresa(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND EmpresaID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 2:
                        sql = "SELECT ID FROM tPerfilLocal(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND LocalID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 3:
                        sql = "SELECT ID FROM tPerfilCanal(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND CanalID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 4:
                        sql = "SELECT ID FROM tPerfilEvento(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND EventoID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 5:
                        sql = "SELECT ID FROM tPerfilEspecial(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND PerfilID=" + perfilID; break;
                    default: // 
                        sql = "SELECT ID FROM tPerfilRegional(NOLOCK) WHERE UsuarioID=" + usuarioID + " AND RegionalID=" + tipoID + " AND PerfilID=" + perfilID; break;
                }

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    id = bd.LerInt("ID");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int BuscarPorLojaID(int lojaID)
        {
            try
            {
                string sql =
                    @"
                        SELECT TOP 1 
                            p.ID
                        FROM tPerfil p (NOLOCK)
                        INNER JOIN tPerfilCanal pc (NOLOCK) ON pc.PerfilID = p.ID
                        INNER JOIN tCanal c (NOLOCK) ON c.ID = pc.CanalID
                        INNER JOIN tLoja l (NOLOCK) ON l.CanalID = c.ID
                        WHERE l.ID = " + lojaID;

                return Convert.ToInt32(bd.ConsultaValor(sql));
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Obtem usuarios de forma paginada
        /// </summary>
        /// <returns></returns>
        public DataTable TodosPorUsuarioPaginacao(int usuarioID, int perfilID, int tipoPerfilID, int paginaAtual, int totItemPorPagina, out int totalRegistros)
        {
            string filtroIDs = "";

            if (perfilID != 0)
                filtroIDs = "AND (X.PerfilID = @perfilID)";

            if (tipoPerfilID != 0)
                filtroIDs += "AND (X.tipoPerfilID = @tipoPerfilID)";

            string query = @"SELECT RowNumber,
                                ID,
								UsuarioID,
								NomePerfil,
								TipoPerfilID,
								TipoPerfil,
                                VinculacaoID,
								NomeVinculacao FROM (
                                     SELECT ROW_NUMBER() OVER(ORDER BY ID) AS RowNumber,
                                                                ID,
																UsuarioID,
																PerfilID,
																NomePerfil,
																TipoPerfilID,
																TipoPerfil,
                                                                VinculacaoID,
																NomeVinculacao
														FROM (
                                                                SELECT  tPerfilCanal.ID, 
                                                                        tPerfilCanal.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil,
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tCanal.ID AS VinculacaoID,
                                                                        tCanal.Nome AS NomeVinculacao
                                                                FROM tPerfilCanal (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilCanal.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tCanal (NOLOCK) ON tPerfilCanal.CanalID = tCanal.ID
                                                                WHERE tPerfilCanal.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEmpresa.ID,
                                                                        tPerfilEmpresa.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tEmpresa.ID AS VinculacaoID,
                                                                        tEmpresa.Nome AS NomeVinculacao
                                                                FROM tPerfilEmpresa (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEmpresa.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tEmpresa (NOLOCK) ON tPerfilEmpresa.EmpresaID = tEmpresa.ID
                                                                WHERE tPerfilEmpresa.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEspecial.ID, 
                                                                        tPerfilEspecial.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil,
                                                                        tPerfilTipo.ID AS TipoPerfilID, 
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        NULL as VinculacaoID,
                                                                        'Especial' AS NomeVinculacao
                                                                FROM tPerfilEspecial (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEspecial.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                WHERE tPerfilEspecial.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEvento.ID,
                                                                        tPerfilEvento.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tEvento.ID AS VinculacaoID,
                                                                        tEvento.Nome AS NomeVinculacao
                                                                FROM tPerfilEvento (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEvento.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tEvento (NOLOCK) ON tPerfilEvento.EventoID = tEvento.ID
                                                                WHERE tPerfilEvento.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilLocal.ID, 
                                                                        tPerfilLocal.UsuarioID, 
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tLocal.ID AS VinculacaoID,
                                                                        tLocal.Nome AS NomeVinculacao
                                                                FROM tPerfilLocal (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilLocal.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tLocal (NOLOCK) ON tPerfilLocal.LocalID = tLocal.ID
                                                                WHERE tPerfilLocal.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilRegional.ID,
                                                                        tPerfilRegional.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tRegional.ID AS VinculacaoID,
                                                                        tRegional.Nome AS NomeVinculacao
                                                                FROM tPerfilRegional (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilRegional.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tRegional (NOLOCK) ON tPerfilRegional.RegionalID = tRegional.ID
                                                                WHERE tPerfilRegional.UsuarioID = @usuarioID
                                                        ) AS X
                                                        WHERE 1=1 " + filtroIDs + @"
                                       ) AS TBL
                                        WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)
                                        ORDER BY ID;
                                        SELECT COUNT(ID) as totalRegistros FROM (
                                                                SELECT  tPerfilCanal.ID, 
                                                                        tPerfilCanal.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil,
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tCanal.ID AS VinculacaoID,
                                                                        tCanal.Nome AS NomeVinculacao
                                                                FROM tPerfilCanal (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilCanal.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tCanal (NOLOCK) ON tPerfilCanal.CanalID = tCanal.ID
                                                                WHERE tPerfilCanal.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEmpresa.ID,
                                                                        tPerfilEmpresa.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tEmpresa.ID AS VinculacaoID,
                                                                        tEmpresa.Nome AS NomeVinculacao
                                                                FROM tPerfilEmpresa (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEmpresa.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tEmpresa (NOLOCK) ON tPerfilEmpresa.EmpresaID = tEmpresa.ID
                                                                WHERE tPerfilEmpresa.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEspecial.ID, 
                                                                        tPerfilEspecial.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil,
                                                                        tPerfilTipo.ID AS TipoPerfilID, 
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        NULL AS VinculacaoID,
                                                                        'Especial' AS NomeVinculacao
                                                                FROM tPerfilEspecial (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEspecial.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                WHERE tPerfilEspecial.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilEvento.ID,
                                                                        tPerfilEvento.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tEvento.ID AS VinculacaoID,
                                                                        tEvento.Nome AS NomeVinculacao
                                                                FROM tPerfilEvento (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilEvento.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tEvento (NOLOCK) ON tPerfilEvento.EventoID = tEvento.ID
                                                                WHERE tPerfilEvento.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilLocal.ID, 
                                                                        tPerfilLocal.UsuarioID, 
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil, 
                                                                        tLocal.ID AS VinculacaoID,
                                                                        tLocal.Nome AS NomeVinculacao
                                                                FROM tPerfilLocal (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilLocal.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tLocal (NOLOCK) ON tPerfilLocal.LocalID = tLocal.ID
                                                                WHERE tPerfilLocal.UsuarioID = @usuarioID
                                                            UNION
                                                                SELECT  tPerfilRegional.ID,
                                                                        tPerfilRegional.UsuarioID,
																        tPerfil.ID AS PerfilID,
                                                                        tPerfil.Nome AS NomePerfil, 
                                                                        tPerfilTipo.ID AS TipoPerfilID,
                                                                        tPerfilTipo.Nome AS TipoPerfil,
                                                                        tRegional.ID AS VinculacaoID,
                                                                        tRegional.Nome AS NomeVinculacao
                                                                FROM tPerfilRegional (NOLOCK)
                                                                        INNER JOIN tPerfil (NOLOCK) ON tPerfilRegional.PerfilID = tPerfil.ID 
                                                                        INNER JOIN tPerfilTipo (NOLOCK) ON tPerfil.PerfilTipoID = tPerfilTipo.ID
                                                                        INNER JOIN tRegional (NOLOCK) ON tPerfilRegional.RegionalID = tRegional.ID
                                                                WHERE tPerfilRegional.UsuarioID = @usuarioID
                                                        ) AS X
                                                        WHERE 1=1 " + filtroIDs;


            CTLib.BD bd = new BD();

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = usuarioID
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@usuarioID",
                Value = usuarioID
            });

            if (perfilID != 0)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@perfilID",
                    Value = perfilID
                });

            if (tipoPerfilID != 0)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.Int32,
                    ParameterName = "@tipoPerfilID",
                    Value = tipoPerfilID
                });


            DataSet dsResult = bd.QueryToDataSet(query, parametros);

            totalRegistros = Convert.ToInt32(dsResult.Tables[1].Rows[0][0]);

            return dsResult.Tables[0];
        }


        /// <summary>
        /// Obtem perfis de canal que determinado usuário não possui
        /// </summary>
        /// <returns></returns>
        public DataTable CanalNaoUsuarioPaginacao(int usuarioID, string canal, int paginaAtual, int totItemPorPagina, out int totalRegistros, bool mostrarAtivos)
        {
            string filtroIDs = "";

            if (canal != String.Empty)
            {
                canal = "%" + canal + "%";
                //filtroIDs = "AND ((tEvento.Nome LIKE @canal) OR (tEmpresa.Nome LIKE @canal) OR (tLocal.Nome LIKE @canal))";
                filtroIDs = "AND (tCanal.Nome LIKE @canal)";
            }

            if (mostrarAtivos == true)
                filtroIDs += "AND tCanal.Ativo = 'T'";

            string query = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tCanal.ID) AS RowNumber,
								tCanal.ID AS 'ID', tCanal.Nome AS NomeCanal, 
								tEmpresa.Nome AS NomeEmpresa, tRegional.Nome AS NomeRegional FROM tCanal
								INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID
								INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
								WHERE 1=1 
								" + filtroIDs + @"
								) AS TBL
								WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)
								
								SELECT COUNT(tCanal.ID) as totalRegistros FROM tCanal
								INNER JOIN tEmpresa ON tCanal.EmpresaID = tEmpresa.ID
								INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
								WHERE tCanal.ID NOT IN (Select canalID from tPerfilCanal(NOLOCK) WHERE UsuarioID = @usuarioID) " + filtroIDs;


            CTLib.BD bd = new BD();

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = usuarioID
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@usuarioID",
                Value = usuarioID
            });

            if (canal != String.Empty)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.String,
                    ParameterName = "@canal",
                    Value = canal
                });

            DataSet dsResult = bd.QueryToDataSet(query, parametros);

            totalRegistros = Convert.ToInt32(dsResult.Tables[1].Rows[0][0]);

            return dsResult.Tables[0];
        }

        /// <summary>
        /// Obtem perfis de canal que determinado usuário não possui
        /// </summary>
        /// <returns></returns>
        public DataTable EmpresaNaoUsuarioPaginacao(int usuarioID, string canal, int paginaAtual, int totItemPorPagina, out int totalRegistros, bool mostrarAtivos)
        {
            string filtroIDs = "";

            if (canal != String.Empty)
            {
                canal = "%" + canal + "%";
                filtroIDs = "AND ((tEmpresa.Nome LIKE @canal) OR (tRegional.Nome LIKE @canal))";
            }

            if (mostrarAtivos == true)
                filtroIDs += "AND tEmpresa.Ativo = 'T'";

            string query = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tEmpresa.ID) AS RowNumber,
                        tEmpresa.ID, tEmpresa.Nome, tRegional.Nome AS NomeRegional FROM tEmpresa
						INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
						WHERE 1=1 
						" + filtroIDs + @") AS TBL
						WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)
						
						SELECT COUNT(tEmpresa.ID) as totalRegistros FROM tEmpresa
						INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
						WHERE tEmpresa.ID NOT IN (SELECT localID FROM tPerfilLocal(NOLOCK) WHERE UsuarioID = @usuarioID) " + filtroIDs;


            CTLib.BD bd = new BD();

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = usuarioID
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@usuarioID",
                Value = usuarioID
            });

            if (canal != String.Empty)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.String,
                    ParameterName = "@canal",
                    Value = canal
                });

            DataSet dsResult = bd.QueryToDataSet(query, parametros);

            totalRegistros = Convert.ToInt32(dsResult.Tables[1].Rows[0][0]);

            return dsResult.Tables[0];
        }

        /// <summary>
        /// Obtem perfis de canal que determinado usuário não possui
        /// </summary>
        /// <returns></returns>
        public DataTable EventoNaoUsuarioPaginacao(int usuarioID, string canal, int paginaAtual, int totItemPorPagina, out int totalRegistros, bool mostrarAtivos)
        {
            string filtroIDs = "";

            if (canal != String.Empty)
            {
                canal = "%" + canal + "%";
                //filtroIDs = "AND ((tEvento.Nome LIKE @canal) OR (tEmpresa.Nome LIKE @canal) OR (tLocal.Nome LIKE @canal))";
                filtroIDs = "AND (tEvento.Nome LIKE @canal)";
            }

            if (mostrarAtivos == true)
                filtroIDs += "AND tEvento.Ativo = 'T'";

            string query = @"SELECT * FROM (
                                     SELECT ROW_NUMBER() OVER(ORDER BY tEvento.ID) AS RowNumber,
																tEvento.ID AS 'ID',
                                                                tEvento.Nome AS 'Evento',
																tLocal.Nome AS 'Local',
																tEmpresa.Nome AS 'Empresa'
																FROM tEvento (NOLOCK)
																LEFT JOIN tLocal (NOLOCK) on tEvento.LocalID = tLocal.ID
																LEFT JOIN tEmpresa (NOLOCK) on tLocal.EmpresaID = tEmpresa.ID
																where 1=1 
																" + filtroIDs + @"
                                       ) AS TBL
                                        WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)
                                        SELECT COUNT(tEvento.ID) as totalRegistros FROM 
																tEvento (NOLOCK)
																LEFT JOIN tLocal (NOLOCK) on tEvento.LocalID = tLocal.ID
																LEFT JOIN tEmpresa (NOLOCK) on tLocal.EmpresaID = tEmpresa.ID
																where tEvento.ID not in (Select eventoID from tPerfilEvento(NOLOCK) where UsuarioID = @usuarioID)
																" + filtroIDs;


            CTLib.BD bd = new BD();

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = usuarioID
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@usuarioID",
                Value = usuarioID
            });

            if (canal != String.Empty)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.String,
                    ParameterName = "@canal",
                    Value = canal
                });

            DataSet dsResult = bd.QueryToDataSet(query, parametros);

            totalRegistros = Convert.ToInt32(dsResult.Tables[1].Rows[0][0]);

            return dsResult.Tables[0];
        }

        /// <summary>
        /// Obtem perfis de canal que determinado usuário não possui
        /// </summary>
        /// <returns></returns>
        public DataTable LocalNaoUsuarioPaginacao(int usuarioID, string canal, int paginaAtual, int totItemPorPagina, out int totalRegistros, bool mostrarAtivos)
        {
            string filtroIDs = "";

            if (canal != String.Empty)
            {
                canal = "%" + canal + "%";
                filtroIDs = "AND ((tLocal.Nome LIKE @canal) OR (tRegional.Nome LIKE @canal) OR (tEmpresa.Nome LIKE @canal))";
            }

            if (mostrarAtivos == true)
                filtroIDs += "AND tLocal.Ativo = 'T'";

            string query = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tLocal.ID) AS RowNumber,
                        tLocal.ID, tLocal.Nome, tRegional.Nome AS NomeRegional, 
						tEmpresa.Nome AS NomeEmpresa FROM tLocal
						INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID
						INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
						WHERE 1=1 
						" + filtroIDs + @") AS TBL
						WHERE RowNumber BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)

						SELECT COUNT(tLocal.ID) as totalRegistros FROM tLocal
						INNER JOIN tEmpresa ON tLocal.EmpresaID = tEmpresa.ID
						INNER JOIN tRegional ON tEmpresa.RegionalID = tRegional.ID
						WHERE tLocal.ID NOT IN (SELECT localID FROM tPerfilLocal(NOLOCK) WHERE UsuarioID = @usuarioID)" + filtroIDs;


            CTLib.BD bd = new BD();

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@PageNumber",
                Value = paginaAtual
            });


            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@RowspPage",
                Value = totItemPorPagina
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.String,
                Direction = ParameterDirection.ReturnValue,
                ParameterName = "@totalRegistros",
                Value = usuarioID
            });

            parametros.Add(new SqlParameter()
            {
                DbType = DbType.Int32,
                ParameterName = "@usuarioID",
                Value = usuarioID
            });

            if (canal != String.Empty)
                parametros.Add(new SqlParameter()
                {
                    DbType = DbType.String,
                    ParameterName = "@canal",
                    Value = canal
                });

            DataSet dsResult = bd.QueryToDataSet(query, parametros);

            totalRegistros = Convert.ToInt32(dsResult.Tables[1].Rows[0][0]);

            return dsResult.Tables[0];
        }

        //Metodos para carga dos drop do perfil de usuário
       public List<string> listaNomePerfils(bool selecione)
        {
            string sql = "SELECT DISTINCT(Nome) FROM tPerfil(NOLOCK)";

            List<string> retorno = new List<string>();

            if (selecione)
                retorno.Add("Selecione");

            bd.Consulta(sql);

            while (bd.Consulta().Read())
                retorno.Add(bd.LerString("Nome"));

            bd.Fechar();

            return retorno;
        }

        public int BuscaIdPerfil(string nomePerfil, int PerfilTipoId)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("SELECT ID ");
            strBuilder.Append("FROM tPerfil(NOLOCK) ");
            strBuilder.Append("WHERE Nome = @Nome AND PerfilTipoID = @PerfilTipoId");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@Nome", Value = nomePerfil, SqlDbType = SqlDbType.VarChar });
            parametros.Add(new SqlParameter() { ParameterName = "@PerfilTipoId", Value = PerfilTipoId, SqlDbType = SqlDbType.Int });

            bd.Consulta(strBuilder.ToString(), parametros);
            if (bd.Consulta().Read())
                return bd.LerInt("ID");

            bd.Fechar();

            return 0;// int.Parse(bd.QueryToTable(strBuilder.ToString(), parametros).AsEnumerable().FirstOrDefault()["PerfilTipoID"].ToString());
        }

        public Dictionary<int, string> listaTiposPerfilsMaster(string nomePerfil, bool selecione)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("SELECT ID, ");
            strBuilder.Append("Nome ");
            strBuilder.Append("FROM tPerfilTipo(NOLOCK) ");
            strBuilder.Append("WHERE ID IN(SELECT PerfilTipoId FROM tPerfil(NOLOCK) WHERE Nome = @nomePerfil)");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@nomePerfil", Value = nomePerfil, SqlDbType = SqlDbType.VarChar });

            //bd.QueryToTable(strBuilder.ToString(), parametros).AsEnumerable();

            bd.Consulta(strBuilder.ToString(), parametros);
            Dictionary<int, string> lista = new Dictionary<int, string>();

            if (selecione)
                lista.Add(0, "Selecione");

            while (bd.Consulta().Read())
                lista.Add(bd.LerInt("ID"), bd.LerString("Nome"));

            bd.Fechar();

            return lista;
        }

        public Dictionary<int, string> listaTiposPerfils(string nomePerfil, bool selecione)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("SELECT ID, ");
            strBuilder.Append("Nome ");
            strBuilder.Append("FROM tPerfilTipo(NOLOCK) ");
            strBuilder.Append("WHERE ID IN(SELECT PerfilTipoId FROM tPerfil(NOLOCK) WHERE Nome = @nomePerfil) and ID <> 5");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@nomePerfil", Value = nomePerfil, SqlDbType = SqlDbType.VarChar });

            //bd.QueryToTable(strBuilder.ToString(), parametros).AsEnumerable();

            bd.Consulta(strBuilder.ToString(), parametros);
            Dictionary<int, string> lista = new Dictionary<int, string>();

            if (selecione)
                lista.Add(0, "Selecione");

            while (bd.Consulta().Read())
                lista.Add(bd.LerInt("ID"), bd.LerString("Nome"));

            bd.Fechar();

            return lista;
        }
        public Dictionary<int, string> listaRegionais(bool selecione)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("SELECT ID, ");
            strBuilder.Append("Nome ");
            strBuilder.Append("FROM tRegional (NOLOCK)");

            bd.Consulta(strBuilder.ToString());

            Dictionary<int, string> lista = new Dictionary<int, string>();

            if (selecione)
                lista.Add(0, "Selecione");

            while (bd.Consulta().Read())
                lista.Add(bd.LerInt("ID"), bd.LerString("Nome"));

            bd.Fechar();

            return lista;
        }

        public Dictionary<int, string> listaRegional(bool selecione, int regionalID)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("SELECT ID, ");
            strBuilder.Append("Nome ");
            strBuilder.Append("FROM tRegional (NOLOCK) ");
            strBuilder.Append("WHERE ID = @regionalID");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@regionalID", Value = regionalID, SqlDbType = SqlDbType.Int });

            bd.Consulta(strBuilder.ToString(), parametros);

            Dictionary<int, string> lista = new Dictionary<int, string>();

            if (selecione)
                lista.Add(0, "Selecione");

            while (bd.Consulta().Read())
                lista.Add(bd.LerInt("ID"), bd.LerString("Nome"));

            bd.Fechar();

            return lista;
        }

        public bool SalvarPerfilEspecial(int perfilID, int usuarioID)
        {
            string sql = @"Insert into tPerfilEspecial
                           (PerfilID,UsuarioID)
                            VALUES (@perfilID,@usuarioID)";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            int x = bd.Executar(sql, parametros);

            bd.Fechar();

            return x == 1;

        }

        public bool ConsultarPerfilEspecial(int perfilID, int usuarioID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilEspecial(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }
        public bool ConsultarPerfilCanal(int perfilID, int usuarioID, int canalID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilCanal(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID AND canalID = @canalID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@canalID", Value = canalID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }

        public bool ConsultarPerfilEmpresa(int perfilID, int usuarioID, int empresaID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilEmpresa(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID AND EmpresaID = @empresaID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@empresaID", Value = empresaID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }

        public bool ConsultarPerfilEvento(int perfilID, int usuarioID, int eventoID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilEvento(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID AND eventoID = @eventoID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@eventoID", Value = eventoID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }

        public bool ConsultarPerfilLocal(int perfilID, int usuarioID, int localID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilLocal(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID AND localID = @localID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@localID", Value = localID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }

        public bool ConsultarPerfilRegional(int perfilID, int usuarioID, int regionalID)
        {
            int x = 0;
            string sql = @"Select ID from tPerfilRegional(NOLOCK) 
                            where perfilID = @perfilID AND usuarioID = @usuarioID AND regionalID = @regionalID";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter() { ParameterName = "@perfilID", Value = perfilID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@usuarioID", Value = usuarioID, SqlDbType = SqlDbType.Int });
            parametros.Add(new SqlParameter() { ParameterName = "@regionalID", Value = regionalID, SqlDbType = SqlDbType.Int });
            bd.Consulta(sql.ToString(), parametros);
            if (bd.Consulta().Read())
                x = bd.LerInt("ID");

            bd.Fechar();

            return x > 0;
        }
    }



    public class PerfilLista : PerfilLista_B
    {

        public PerfilLista() { }

        public PerfilLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioPerfil");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ID", typeof(int));
                    tabela.Columns.Add("Tipo", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        PerfilTipo perfilTipo = new PerfilTipo();
                        perfilTipo.Ler(perfil.PerfilTipoID.Valor);
                        linha["ID"] = perfil.Control.ID;
                        linha["Tipo"] = perfilTipo.Nome.Valor;
                        linha["Nome"] = perfil.Nome.Valor;
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

        /// <summary>
        /// Retorna um objeto do tipo correto dependendo do tipo passado como parametro e ja adidiona o id
        /// </summary>
        /// <param name="tipo">tipo empresa, por exemplo</param>
        /// <param name="tipoid">id da empresa, por exemplo</param>
        /// <param name="perfilid">id d perfil</param>
        /// <param name="ids">id dos usuarios separados por virgula</param>
        /// <returns></returns>
        public IBaseLista TipoGenerico(int tipo, int tipoid, int perfilid, string ids)
        {

            IBaseLista lista;

            switch (tipo)
            {
                case PerfilTipo.CANAL:
                    PerfilCanalLista canLista = new PerfilCanalLista(this.Perfil.Control.UsuarioID);
                    canLista.FiltroSQL = "PerfilID=" + perfilid + " AND CanalID=" + tipoid + " AND UsuarioID in (" + ids + ")";
                    lista = canLista;
                    break;
                case PerfilTipo.EMPRESA:
                    PerfilEmpresaLista empLista = new PerfilEmpresaLista(this.Perfil.Control.UsuarioID);
                    empLista.FiltroSQL = "PerfilID=" + perfilid + " AND EmpresaID=" + tipoid + " AND UsuarioID in (" + ids + ")";
                    lista = empLista;
                    break;
                case PerfilTipo.ESPECIAL:
                    PerfilEspecialLista espLista = new PerfilEspecialLista(this.Perfil.Control.UsuarioID);
                    espLista.FiltroSQL = "PerfilID=" + perfilid + " AND UsuarioID in (" + ids + ")";
                    lista = espLista;
                    break;
                case PerfilTipo.EVENTO:
                    PerfilEventoLista eveLista = new PerfilEventoLista(this.Perfil.Control.UsuarioID);
                    eveLista.FiltroSQL = "PerfilID=" + perfilid + " AND EventoID=" + tipoid + " AND UsuarioID in (" + ids + ")";
                    lista = eveLista;
                    break;
                case PerfilTipo.LOCAL:
                    PerfilLocalLista locLista = new PerfilLocalLista(this.Perfil.Control.UsuarioID);
                    locLista.FiltroSQL = "PerfilID=" + perfilid + " AND LocalID=" + tipoid + " AND UsuarioID in (" + ids + ")";
                    lista = locLista;
                    break;
                case PerfilTipo.REGIONAL:
                    PerfilRegionalLista regLista = new PerfilRegionalLista(this.Perfil.Control.UsuarioID);
                    regLista.FiltroSQL = "PerfilID=" + perfilid + " AND RegionalID=" + tipoid + " AND UsuarioID in (" + ids + ")";
                    lista = regLista;
                    break;
                default:
                    lista = null;
                    break;
            }

            return lista;

        }
    }
}
