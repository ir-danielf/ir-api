

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "AssinaturaTipo_B"

    public abstract class AssinaturaTipo_B : BaseBD
    {


        public nome Nome = new nome();

        public renovacaoinicio RenovacaoInicio = new renovacaoinicio();

        public renovacaofim RenovacaoFim = new renovacaofim();

        public trocaprioritariainicio TrocaPrioritariaInicio = new trocaprioritariainicio();

        public trocaprioritariafim TrocaPrioritariaFim = new trocaprioritariafim();

        public trocainicio TrocaInicio = new trocainicio();

        public trocafim TrocaFim = new trocafim();

        public novaaquisicaoinicio NovaAquisicaoInicio = new novaaquisicaoinicio();

        public novaaquisicaofim NovaAquisicaoFim = new novaaquisicaofim();

        public localid LocalID = new localid();

        public permiteagregados PermiteAgregados = new permiteagregados();

        public layout Layout = new layout();

        public logo Logo = new logo();

        public programacao Programacao = new programacao();

        public precos Precos = new precos();

        public termos Termos = new termos();

        public paginaprincipal PaginaPrincipal = new paginaprincipal();

        public paginalogin PaginaLogin = new paginalogin();

        public paginarodape PaginaRodape = new paginarodape();

        public retiradabilheteria RetiradaBilheteria = new retiradabilheteria();

        public valorentrega ValorEntrega = new valorentrega();

        public valorentregafixo ValorEntregaFixo = new valorentregafixo();

        public entregaid EntregaID = new entregaid();


        public AssinaturaTipo_B() { }

        // passar o Usuario logado no sistema
        public AssinaturaTipo_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de AssinaturaTipo
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinaturaTipo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.RenovacaoInicio.ValorBD = bd.LerString("RenovacaoInicio");

                    this.RenovacaoFim.ValorBD = bd.LerString("RenovacaoFim");

                    this.TrocaPrioritariaInicio.ValorBD = bd.LerString("TrocaPrioritariaInicio");

                    this.TrocaPrioritariaFim.ValorBD = bd.LerString("TrocaPrioritariaFim");

                    this.TrocaInicio.ValorBD = bd.LerString("TrocaInicio");

                    this.TrocaFim.ValorBD = bd.LerString("TrocaFim");

                    this.NovaAquisicaoInicio.ValorBD = bd.LerString("NovaAquisicaoInicio");

                    this.NovaAquisicaoFim.ValorBD = bd.LerString("NovaAquisicaoFim");

                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();

                    this.PermiteAgregados.ValorBD = bd.LerString("PermiteAgregados");

                    this.Layout.ValorBD = bd.LerString("Layout");

                    this.Logo.ValorBD = bd.LerString("Logo");

                    this.Programacao.ValorBD = bd.LerString("Programacao");

                    this.Precos.ValorBD = bd.LerString("Precos");

                    this.Termos.ValorBD = bd.LerString("Termos");

                    this.PaginaPrincipal.ValorBD = bd.LerString("PaginaPrincipal");

                    this.PaginaLogin.ValorBD = bd.LerString("PaginaLogin");

                    this.PaginaRodape.ValorBD = bd.LerString("PaginaRodape");

                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");

                    this.ValorEntrega.ValorBD = bd.LerDecimal("ValorEntrega").ToString();

                    this.ValorEntregaFixo.ValorBD = bd.LerString("ValorEntregaFixo");

                    this.EntregaID.ValorBD = bd.LerString("EntregaID");

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
        /// Preenche todos os atributos de AssinaturaTipo do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xAssinaturaTipo WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.RenovacaoInicio.ValorBD = bd.LerString("RenovacaoInicio");

                    this.RenovacaoFim.ValorBD = bd.LerString("RenovacaoFim");

                    this.TrocaPrioritariaInicio.ValorBD = bd.LerString("TrocaPrioritariaInicio");

                    this.TrocaPrioritariaFim.ValorBD = bd.LerString("TrocaPrioritariaFim");

                    this.TrocaInicio.ValorBD = bd.LerString("TrocaInicio");

                    this.TrocaFim.ValorBD = bd.LerString("TrocaFim");

                    this.NovaAquisicaoInicio.ValorBD = bd.LerString("NovaAquisicaoInicio");

                    this.NovaAquisicaoFim.ValorBD = bd.LerString("NovaAquisicaoFim");

                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();

                    this.PermiteAgregados.ValorBD = bd.LerString("PermiteAgregados");

                    this.Layout.ValorBD = bd.LerString("Layout");

                    this.Logo.ValorBD = bd.LerString("Logo");

                    this.Programacao.ValorBD = bd.LerString("Programacao");

                    this.Precos.ValorBD = bd.LerString("Precos");

                    this.Termos.ValorBD = bd.LerString("Termos");

                    this.PaginaPrincipal.ValorBD = bd.LerString("PaginaPrincipal");

                    this.PaginaLogin.ValorBD = bd.LerString("PaginaLogin");

                    this.PaginaRodape.ValorBD = bd.LerString("PaginaRodape");

                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");

                    this.ValorEntrega.ValorBD = bd.LerDecimal("ValorEntrega").ToString();

                    this.ValorEntregaFixo.ValorBD = bd.LerString("ValorEntregaFixo");

                    this.EntregaID.ValorBD = bd.LerString("EntregaID");

                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cAssinaturaTipo (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();


                sql.Append("INSERT INTO xAssinaturaTipo (ID, Versao, Nome, RenovacaoInicio, RenovacaoFim, TrocaPrioritariaInicio, TrocaPrioritariaFim, TrocaInicio, TrocaFim, NovaAquisicaoInicio, NovaAquisicaoFim, LocalID, PermiteAgregados, Layout, Logo, Programacao, Precos, Termos, PaginaPrincipal, PaginaLogin, PaginaRodape, RetiradaBilheteria, ValorEntrega, ValorEntregaFixo, EntregaID) ");
                sql.Append("SELECT ID, @V, Nome, RenovacaoInicio, RenovacaoFim, TrocaPrioritariaInicio, TrocaPrioritariaFim, TrocaInicio, TrocaFim, NovaAquisicaoInicio, NovaAquisicaoFim, LocalID, PermiteAgregados, Layout, Logo, Programacao, Precos, Termos, PaginaPrincipal, PaginaLogin, PaginaRodape, RetiradaBilheteria, ValorEntrega, ValorEntregaFixo, EntregaID FROM tAssinaturaTipo WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) AssinaturaTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaTipo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaTipo(ID, Nome, RenovacaoInicio, RenovacaoFim, TrocaPrioritariaInicio, TrocaPrioritariaFim, TrocaInicio, TrocaFim, NovaAquisicaoInicio, NovaAquisicaoFim, LocalID, PermiteAgregados, Layout, Logo, Programacao, Precos, Termos, PaginaPrincipal, PaginaLogin, PaginaRodape, RetiradaBilheteria, ValorEntrega, ValorEntregaFixo, EntregaID) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009',@010,'@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.RenovacaoInicio.ValorBD);

                sql.Replace("@003", this.RenovacaoFim.ValorBD);

                sql.Replace("@004", this.TrocaPrioritariaInicio.ValorBD);

                sql.Replace("@005", this.TrocaPrioritariaFim.ValorBD);

                sql.Replace("@006", this.TrocaInicio.ValorBD);

                sql.Replace("@007", this.TrocaFim.ValorBD);

                sql.Replace("@008", this.NovaAquisicaoInicio.ValorBD);

                sql.Replace("@009", this.NovaAquisicaoFim.ValorBD);

                sql.Replace("@010", this.LocalID.ValorBD);

                sql.Replace("@011", this.PermiteAgregados.ValorBD);

                sql.Replace("@012", this.Layout.ValorBD);

                sql.Replace("@013", this.Logo.ValorBD);

                sql.Replace("@014", this.Programacao.ValorBD);

                sql.Replace("@015", this.Precos.ValorBD);

                sql.Replace("@016", this.Termos.ValorBD);

                sql.Replace("@017", this.PaginaPrincipal.ValorBD);

                sql.Replace("@018", this.PaginaLogin.ValorBD);

                sql.Replace("@019", this.PaginaRodape.ValorBD);

                sql.Replace("@020", this.RetiradaBilheteria.ValorBD);

                sql.Replace("@021", this.ValorEntrega.ValorBD);

                sql.Replace("@022", this.ValorEntregaFixo.ValorBD);

                sql.Replace("@023", this.EntregaID.ValorBD);


                int x = bd.Executar(sql.ToString());

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
        /// Inserir novo(a) AssinaturaTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinaturaTipo");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinaturaTipo(ID, Nome, RenovacaoInicio, RenovacaoFim, TrocaPrioritariaInicio, TrocaPrioritariaFim, TrocaInicio, TrocaFim, NovaAquisicaoInicio, NovaAquisicaoFim, LocalID, PermiteAgregados, Layout, Logo, Programacao, Precos, Termos, PaginaPrincipal, PaginaLogin, PaginaRodape, RetiradaBilheteria, ValorEntrega, ValorEntregaFixo, EntregaID) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009',@010,'@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.RenovacaoInicio.ValorBD);

                sql.Replace("@003", this.RenovacaoFim.ValorBD);

                sql.Replace("@004", this.TrocaPrioritariaInicio.ValorBD);

                sql.Replace("@005", this.TrocaPrioritariaFim.ValorBD);

                sql.Replace("@006", this.TrocaInicio.ValorBD);

                sql.Replace("@007", this.TrocaFim.ValorBD);

                sql.Replace("@008", this.NovaAquisicaoInicio.ValorBD);

                sql.Replace("@009", this.NovaAquisicaoFim.ValorBD);

                sql.Replace("@010", this.LocalID.ValorBD);

                sql.Replace("@011", this.PermiteAgregados.ValorBD);

                sql.Replace("@012", this.Layout.ValorBD);

                sql.Replace("@013", this.Logo.ValorBD);

                sql.Replace("@014", this.Programacao.ValorBD);

                sql.Replace("@015", this.Precos.ValorBD);

                sql.Replace("@016", this.Termos.ValorBD);

                sql.Replace("@017", this.PaginaPrincipal.ValorBD);

                sql.Replace("@018", this.PaginaLogin.ValorBD);

                sql.Replace("@019", this.PaginaRodape.ValorBD);

                sql.Replace("@020", this.RetiradaBilheteria.ValorBD);

                sql.Replace("@021", this.ValorEntrega.ValorBD);

                sql.Replace("@022", this.ValorEntregaFixo.ValorBD);

                sql.Replace("@023", this.EntregaID.ValorBD);


                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza AssinaturaTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tAssinaturaTipo SET Nome = '@001', RenovacaoInicio = '@002', RenovacaoFim = '@003', TrocaPrioritariaInicio = '@004', TrocaPrioritariaFim = '@005', TrocaInicio = '@006', TrocaFim = '@007', NovaAquisicaoInicio = '@008', NovaAquisicaoFim = '@009', LocalID = @010, PermiteAgregados = '@011', Layout = '@012', Logo = '@013', Programacao = '@014', Precos = '@015', Termos = '@016', PaginaPrincipal = '@017', PaginaLogin = '@018', PaginaRodape = '@019', RetiradaBilheteria = '@020', ValorEntrega = '@021', ValorEntregaFixo = '@022', EntregaID = '@023' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.RenovacaoInicio.ValorBD);

                sql.Replace("@003", this.RenovacaoFim.ValorBD);

                sql.Replace("@004", this.TrocaPrioritariaInicio.ValorBD);

                sql.Replace("@005", this.TrocaPrioritariaFim.ValorBD);

                sql.Replace("@006", this.TrocaInicio.ValorBD);

                sql.Replace("@007", this.TrocaFim.ValorBD);

                sql.Replace("@008", this.NovaAquisicaoInicio.ValorBD);

                sql.Replace("@009", this.NovaAquisicaoFim.ValorBD);

                sql.Replace("@010", this.LocalID.ValorBD);

                sql.Replace("@011", this.PermiteAgregados.ValorBD);

                sql.Replace("@012", this.Layout.ValorBD);

                sql.Replace("@013", this.Logo.ValorBD);

                sql.Replace("@014", this.Programacao.ValorBD);

                sql.Replace("@015", this.Precos.ValorBD);

                sql.Replace("@016", this.Termos.ValorBD);

                sql.Replace("@017", this.PaginaPrincipal.ValorBD);

                sql.Replace("@018", this.PaginaLogin.ValorBD);

                sql.Replace("@019", this.PaginaRodape.ValorBD);

                sql.Replace("@020", this.RetiradaBilheteria.ValorBD);

                sql.Replace("@021", this.ValorEntrega.ValorBD);

                sql.Replace("@022", this.ValorEntregaFixo.ValorBD);

                sql.Replace("@023", this.EntregaID.ValorBD);


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

        /// <summary>
        /// Atualiza AssinaturaTipo
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinaturaTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tAssinaturaTipo SET Nome = '@001', RenovacaoInicio = '@002', RenovacaoFim = '@003', TrocaPrioritariaInicio = '@004', TrocaPrioritariaFim = '@005', TrocaInicio = '@006', TrocaFim = '@007', NovaAquisicaoInicio = '@008', NovaAquisicaoFim = '@009', LocalID = @010, PermiteAgregados = '@011', Layout = '@012', Logo = '@013', Programacao = '@014', Precos = '@015', Termos = '@016', PaginaPrincipal = '@017', PaginaLogin = '@018', PaginaRodape = '@019', RetiradaBilheteria = '@020', ValorEntrega = '@021', ValorEntregaFixo = '@022', EntregaID = '@023' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.RenovacaoInicio.ValorBD);

                sql.Replace("@003", this.RenovacaoFim.ValorBD);

                sql.Replace("@004", this.TrocaPrioritariaInicio.ValorBD);

                sql.Replace("@005", this.TrocaPrioritariaFim.ValorBD);

                sql.Replace("@006", this.TrocaInicio.ValorBD);

                sql.Replace("@007", this.TrocaFim.ValorBD);

                sql.Replace("@008", this.NovaAquisicaoInicio.ValorBD);

                sql.Replace("@009", this.NovaAquisicaoFim.ValorBD);

                sql.Replace("@010", this.LocalID.ValorBD);

                sql.Replace("@011", this.PermiteAgregados.ValorBD);

                sql.Replace("@012", this.Layout.ValorBD);

                sql.Replace("@013", this.Logo.ValorBD);

                sql.Replace("@014", this.Programacao.ValorBD);

                sql.Replace("@015", this.Precos.ValorBD);

                sql.Replace("@016", this.Termos.ValorBD);

                sql.Replace("@017", this.PaginaPrincipal.ValorBD);

                sql.Replace("@018", this.PaginaLogin.ValorBD);

                sql.Replace("@019", this.PaginaRodape.ValorBD);

                sql.Replace("@020", this.RetiradaBilheteria.ValorBD);

                sql.Replace("@021", this.ValorEntrega.ValorBD);

                sql.Replace("@022", this.ValorEntregaFixo.ValorBD);

                sql.Replace("@023", this.EntregaID.ValorBD);


                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui AssinaturaTipo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaTipo WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

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

        /// <summary>
        /// Exclui AssinaturaTipo com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinaturaTipo WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinaturaTipo WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui AssinaturaTipo
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void Limpar()
        {


            this.Nome.Limpar();

            this.RenovacaoInicio.Limpar();

            this.RenovacaoFim.Limpar();

            this.TrocaPrioritariaInicio.Limpar();

            this.TrocaPrioritariaFim.Limpar();

            this.TrocaInicio.Limpar();

            this.TrocaFim.Limpar();

            this.NovaAquisicaoInicio.Limpar();

            this.NovaAquisicaoFim.Limpar();

            this.LocalID.Limpar();

            this.PermiteAgregados.Limpar();

            this.Layout.Limpar();

            this.Logo.Limpar();

            this.Programacao.Limpar();

            this.Precos.Limpar();

            this.Termos.Limpar();

            this.PaginaPrincipal.Limpar();

            this.PaginaLogin.Limpar();

            this.PaginaRodape.Limpar();

            this.RetiradaBilheteria.Limpar();

            this.ValorEntrega.Limpar();

            this.ValorEntregaFixo.Limpar();

            this.EntregaID.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.Nome.Desfazer();

            this.RenovacaoInicio.Desfazer();

            this.RenovacaoFim.Desfazer();

            this.TrocaPrioritariaInicio.Desfazer();

            this.TrocaPrioritariaFim.Desfazer();

            this.TrocaInicio.Desfazer();

            this.TrocaFim.Desfazer();

            this.NovaAquisicaoInicio.Desfazer();

            this.NovaAquisicaoFim.Desfazer();

            this.LocalID.Desfazer();

            this.PermiteAgregados.Desfazer();

            this.Layout.Desfazer();

            this.Logo.Desfazer();

            this.Programacao.Desfazer();

            this.Precos.Desfazer();

            this.Termos.Desfazer();

            this.PaginaPrincipal.Desfazer();

            this.PaginaLogin.Desfazer();

            this.PaginaRodape.Desfazer();

            this.RetiradaBilheteria.Desfazer();

            this.ValorEntrega.Desfazer();

            this.ValorEntregaFixo.Desfazer();

            this.EntregaID.Desfazer();

        }


        public class nome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class renovacaoinicio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "RenovacaoInicio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class renovacaofim : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "RenovacaoFim";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class trocaprioritariainicio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaPrioritariaInicio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class trocaprioritariafim : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaPrioritariaFim";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class trocainicio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaInicio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class trocafim : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TrocaFim";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class novaaquisicaoinicio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "NovaAquisicaoInicio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class novaaquisicaofim : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "NovaAquisicaoFim";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("dd/MM/yyyy HH:mm");

            }

        }


        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString();

            }

        }


        public class permiteagregados : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermiteAgregados";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString();

            }

        }


        public class layout : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Layout";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class logo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Logo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class programacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Programacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class precos : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Precos";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class termos : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Termos";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4000;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class paginaprincipal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PaginaPrincipal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4000;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class paginalogin : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PaginaLogin";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4000;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class paginarodape : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PaginaRodape";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4000;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        public class retiradabilheteria : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RetiradaBilheteria";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString();

            }

        }


        public class valorentrega : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorEntrega";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 18;
                }
            }

            public override decimal Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString("###,##0.00");

            }

        }


        public class valorentregafixo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorEntregaFixo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor.ToString();

            }

        }


        public class entregaid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 200;
                }
            }

            public override string Valor
            {

                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }

            }


            public override string ToString()
            {

                return base.Valor;

            }

        }


        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("AssinaturaTipo");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("RenovacaoInicio", typeof(DateTime));

                tabela.Columns.Add("RenovacaoFim", typeof(DateTime));

                tabela.Columns.Add("TrocaPrioritariaInicio", typeof(DateTime));

                tabela.Columns.Add("TrocaPrioritariaFim", typeof(DateTime));

                tabela.Columns.Add("TrocaInicio", typeof(DateTime));

                tabela.Columns.Add("TrocaFim", typeof(DateTime));

                tabela.Columns.Add("NovaAquisicaoInicio", typeof(DateTime));

                tabela.Columns.Add("NovaAquisicaoFim", typeof(DateTime));

                tabela.Columns.Add("LocalID", typeof(int));

                tabela.Columns.Add("PermiteAgregados", typeof(bool));

                tabela.Columns.Add("Layout", typeof(string));

                tabela.Columns.Add("Logo", typeof(string));

                tabela.Columns.Add("Programacao", typeof(string));

                tabela.Columns.Add("Precos", typeof(string));

                tabela.Columns.Add("Termos", typeof(string));

                tabela.Columns.Add("PaginaPrincipal", typeof(string));

                tabela.Columns.Add("PaginaLogin", typeof(string));

                tabela.Columns.Add("PaginaRodape", typeof(string));

                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));

                tabela.Columns.Add("ValorEntrega", typeof(decimal));

                tabela.Columns.Add("ValorEntregaFixo", typeof(bool));

                tabela.Columns.Add("EntregaID", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "AssinaturaTipoLista_B"


    public abstract class AssinaturaTipoLista_B : BaseLista
    {

        private bool backup = false;
        protected AssinaturaTipo assinaturaTipo;

        // passar o Usuario logado no sistema
        public AssinaturaTipoLista_B()
        {
            assinaturaTipo = new AssinaturaTipo();
        }

        // passar o Usuario logado no sistema
        public AssinaturaTipoLista_B(int usuarioIDLogado)
        {
            assinaturaTipo = new AssinaturaTipo();
        }

        public AssinaturaTipo AssinaturaTipo
        {
            get { return assinaturaTipo; }
        }

        /// <summary>
        /// Retorna um IBaseBD de AssinaturaTipo especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    assinaturaTipo.Ler(id);
                    return assinaturaTipo;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tAssinaturaTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

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

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tAssinaturaTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinaturaTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

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

        }

        /// <summary>
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xAssinaturaTipo";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xAssinaturaTipo";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche AssinaturaTipo corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    assinaturaTipo.Ler(id);
                else
                    assinaturaTipo.LerBackup(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = assinaturaTipo.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) AssinaturaTipo na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinaturaTipo.Inserir();
                if (ok)
                {
                    lista.Add(assinaturaTipo.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Obtem uma tabela de todos os campos de AssinaturaTipo carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("AssinaturaTipo");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("RenovacaoInicio", typeof(DateTime));

                tabela.Columns.Add("RenovacaoFim", typeof(DateTime));

                tabela.Columns.Add("TrocaPrioritariaInicio", typeof(DateTime));

                tabela.Columns.Add("TrocaPrioritariaFim", typeof(DateTime));

                tabela.Columns.Add("TrocaInicio", typeof(DateTime));

                tabela.Columns.Add("TrocaFim", typeof(DateTime));

                tabela.Columns.Add("NovaAquisicaoInicio", typeof(DateTime));

                tabela.Columns.Add("NovaAquisicaoFim", typeof(DateTime));

                tabela.Columns.Add("LocalID", typeof(int));

                tabela.Columns.Add("PermiteAgregados", typeof(bool));

                tabela.Columns.Add("Layout", typeof(string));

                tabela.Columns.Add("Logo", typeof(string));

                tabela.Columns.Add("Programacao", typeof(string));

                tabela.Columns.Add("Precos", typeof(string));

                tabela.Columns.Add("Termos", typeof(string));

                tabela.Columns.Add("PaginaPrincipal", typeof(string));

                tabela.Columns.Add("PaginaLogin", typeof(string));

                tabela.Columns.Add("PaginaRodape", typeof(string));

                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));

                tabela.Columns.Add("ValorEntrega", typeof(decimal));

                tabela.Columns.Add("ValorEntregaFixo", typeof(bool));

                tabela.Columns.Add("EntregaID", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinaturaTipo.Control.ID;

                        linha["Nome"] = assinaturaTipo.Nome.Valor;

                        linha["RenovacaoInicio"] = assinaturaTipo.RenovacaoInicio.Valor;

                        linha["RenovacaoFim"] = assinaturaTipo.RenovacaoFim.Valor;

                        linha["TrocaPrioritariaInicio"] = assinaturaTipo.TrocaPrioritariaInicio.Valor;

                        linha["TrocaPrioritariaFim"] = assinaturaTipo.TrocaPrioritariaFim.Valor;

                        linha["TrocaInicio"] = assinaturaTipo.TrocaInicio.Valor;

                        linha["TrocaFim"] = assinaturaTipo.TrocaFim.Valor;

                        linha["NovaAquisicaoInicio"] = assinaturaTipo.NovaAquisicaoInicio.Valor;

                        linha["NovaAquisicaoFim"] = assinaturaTipo.NovaAquisicaoFim.Valor;

                        linha["LocalID"] = assinaturaTipo.LocalID.Valor;

                        linha["PermiteAgregados"] = assinaturaTipo.PermiteAgregados.Valor;

                        linha["Layout"] = assinaturaTipo.Layout.Valor;

                        linha["Logo"] = assinaturaTipo.Logo.Valor;

                        linha["Programacao"] = assinaturaTipo.Programacao.Valor;

                        linha["Precos"] = assinaturaTipo.Precos.Valor;

                        linha["Termos"] = assinaturaTipo.Termos.Valor;

                        linha["PaginaPrincipal"] = assinaturaTipo.PaginaPrincipal.Valor;

                        linha["PaginaLogin"] = assinaturaTipo.PaginaLogin.Valor;

                        linha["PaginaRodape"] = assinaturaTipo.PaginaRodape.Valor;

                        linha["RetiradaBilheteria"] = assinaturaTipo.RetiradaBilheteria.Valor;

                        linha["ValorEntrega"] = assinaturaTipo.ValorEntrega.Valor;

                        linha["ValorEntregaFixo"] = assinaturaTipo.ValorEntregaFixo.Valor;

                        linha["EntregaID"] = assinaturaTipo.EntregaID.Valor;

                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioAssinaturaTipo");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("Nome", typeof(string));

                    tabela.Columns.Add("RenovacaoInicio", typeof(DateTime));

                    tabela.Columns.Add("RenovacaoFim", typeof(DateTime));

                    tabela.Columns.Add("TrocaPrioritariaInicio", typeof(DateTime));

                    tabela.Columns.Add("TrocaPrioritariaFim", typeof(DateTime));

                    tabela.Columns.Add("TrocaInicio", typeof(DateTime));

                    tabela.Columns.Add("TrocaFim", typeof(DateTime));

                    tabela.Columns.Add("NovaAquisicaoInicio", typeof(DateTime));

                    tabela.Columns.Add("NovaAquisicaoFim", typeof(DateTime));

                    tabela.Columns.Add("LocalID", typeof(int));

                    tabela.Columns.Add("PermiteAgregados", typeof(bool));

                    tabela.Columns.Add("Layout", typeof(string));

                    tabela.Columns.Add("Logo", typeof(string));

                    tabela.Columns.Add("Programacao", typeof(string));

                    tabela.Columns.Add("Precos", typeof(string));

                    tabela.Columns.Add("Termos", typeof(string));

                    tabela.Columns.Add("PaginaPrincipal", typeof(string));

                    tabela.Columns.Add("PaginaLogin", typeof(string));

                    tabela.Columns.Add("PaginaRodape", typeof(string));

                    tabela.Columns.Add("RetiradaBilheteria", typeof(bool));

                    tabela.Columns.Add("ValorEntrega", typeof(decimal));

                    tabela.Columns.Add("ValorEntregaFixo", typeof(bool));

                    tabela.Columns.Add("EntregaID", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["Nome"] = assinaturaTipo.Nome.Valor;

                        linha["RenovacaoInicio"] = assinaturaTipo.RenovacaoInicio.Valor;

                        linha["RenovacaoFim"] = assinaturaTipo.RenovacaoFim.Valor;

                        linha["TrocaPrioritariaInicio"] = assinaturaTipo.TrocaPrioritariaInicio.Valor;

                        linha["TrocaPrioritariaFim"] = assinaturaTipo.TrocaPrioritariaFim.Valor;

                        linha["TrocaInicio"] = assinaturaTipo.TrocaInicio.Valor;

                        linha["TrocaFim"] = assinaturaTipo.TrocaFim.Valor;

                        linha["NovaAquisicaoInicio"] = assinaturaTipo.NovaAquisicaoInicio.Valor;

                        linha["NovaAquisicaoFim"] = assinaturaTipo.NovaAquisicaoFim.Valor;

                        linha["LocalID"] = assinaturaTipo.LocalID.Valor;

                        linha["PermiteAgregados"] = assinaturaTipo.PermiteAgregados.Valor;

                        linha["Layout"] = assinaturaTipo.Layout.Valor;

                        linha["Logo"] = assinaturaTipo.Logo.Valor;

                        linha["Programacao"] = assinaturaTipo.Programacao.Valor;

                        linha["Precos"] = assinaturaTipo.Precos.Valor;

                        linha["Termos"] = assinaturaTipo.Termos.Valor;

                        linha["PaginaPrincipal"] = assinaturaTipo.PaginaPrincipal.Valor;

                        linha["PaginaLogin"] = assinaturaTipo.PaginaLogin.Valor;

                        linha["PaginaRodape"] = assinaturaTipo.PaginaRodape.Valor;

                        linha["RetiradaBilheteria"] = assinaturaTipo.RetiradaBilheteria.Valor;

                        linha["ValorEntrega"] = assinaturaTipo.ValorEntrega.Valor;

                        linha["ValorEntregaFixo"] = assinaturaTipo.ValorEntregaFixo.Valor;

                        linha["EntregaID"] = assinaturaTipo.EntregaID.Valor;

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
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {

                    case "Nome":
                        sql = "SELECT ID, Nome FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;

                    case "RenovacaoInicio":
                        sql = "SELECT ID, RenovacaoInicio FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY RenovacaoInicio";
                        break;

                    case "RenovacaoFim":
                        sql = "SELECT ID, RenovacaoFim FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY RenovacaoFim";
                        break;

                    case "TrocaPrioritariaInicio":
                        sql = "SELECT ID, TrocaPrioritariaInicio FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY TrocaPrioritariaInicio";
                        break;

                    case "TrocaPrioritariaFim":
                        sql = "SELECT ID, TrocaPrioritariaFim FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY TrocaPrioritariaFim";
                        break;

                    case "TrocaInicio":
                        sql = "SELECT ID, TrocaInicio FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY TrocaInicio";
                        break;

                    case "TrocaFim":
                        sql = "SELECT ID, TrocaFim FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY TrocaFim";
                        break;

                    case "NovaAquisicaoInicio":
                        sql = "SELECT ID, NovaAquisicaoInicio FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY NovaAquisicaoInicio";
                        break;

                    case "NovaAquisicaoFim":
                        sql = "SELECT ID, NovaAquisicaoFim FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY NovaAquisicaoFim";
                        break;

                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;

                    case "PermiteAgregados":
                        sql = "SELECT ID, PermiteAgregados FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY PermiteAgregados";
                        break;

                    case "Layout":
                        sql = "SELECT ID, Layout FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Layout";
                        break;

                    case "Logo":
                        sql = "SELECT ID, Logo FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Logo";
                        break;

                    case "Programacao":
                        sql = "SELECT ID, Programacao FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Programacao";
                        break;

                    case "Precos":
                        sql = "SELECT ID, Precos FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Precos";
                        break;

                    case "Termos":
                        sql = "SELECT ID, Termos FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY Termos";
                        break;

                    case "PaginaPrincipal":
                        sql = "SELECT ID, PaginaPrincipal FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY PaginaPrincipal";
                        break;

                    case "PaginaLogin":
                        sql = "SELECT ID, PaginaLogin FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY PaginaLogin";
                        break;

                    case "PaginaRodape":
                        sql = "SELECT ID, PaginaRodape FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY PaginaRodape";
                        break;

                    case "RetiradaBilheteria":
                        sql = "SELECT ID, RetiradaBilheteria FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY RetiradaBilheteria";
                        break;

                    case "ValorEntrega":
                        sql = "SELECT ID, ValorEntrega FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY ValorEntrega";
                        break;

                    case "ValorEntregaFixo":
                        sql = "SELECT ID, ValorEntregaFixo FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY ValorEntregaFixo";
                        break;

                    case "EntregaID":
                        sql = "SELECT ID, EntregaID FROM tAssinaturaTipo WHERE " + FiltroSQL + " ORDER BY EntregaID";
                        break;

                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            try
            {

                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "AssinaturaTipoException"

    [Serializable]
    public class AssinaturaTipoException : Exception
    {

        public AssinaturaTipoException() : base() { }

        public AssinaturaTipoException(string msg) : base(msg) { }

        public AssinaturaTipoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}