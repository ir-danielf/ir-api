/* ---------------------------------------------------------------
-- Arquivo Perfil.cs
-- Gerado em: segunda-feira, 28 de março de 2005
-- Autor: Celeritas Ltda
---------------------------------------------------------------- */


using CTLib;
using System;
using System.Data;

namespace IRLib.Paralela
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
                return this.Control.ID == SAC_OPERADOR;
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
                        sql = "SELECT ID FROM tPerfilEmpresa WHERE UsuarioID=" + usuarioID + " AND EmpresaID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 2:
                        sql = "SELECT ID FROM tPerfilLocal WHERE UsuarioID=" + usuarioID + " AND LocalID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 3:
                        sql = "SELECT ID FROM tPerfilCanal WHERE UsuarioID=" + usuarioID + " AND CanalID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 4:
                        sql = "SELECT ID FROM tPerfilEvento WHERE UsuarioID=" + usuarioID + " AND EventoID=" + tipoID + " AND PerfilID=" + perfilID; break;
                    case 5:
                        sql = "SELECT ID FROM tPerfilEspecial WHERE UsuarioID=" + usuarioID + " AND PerfilID=" + perfilID; break;
                    default: // 
                        sql = "SELECT ID FROM tPerfilRegional WHERE UsuarioID=" + usuarioID + " AND RegionalID=" + tipoID + " AND PerfilID=" + perfilID; break;
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
