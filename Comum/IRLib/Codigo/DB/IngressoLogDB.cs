/******************************************************
* Arquivo IngressoLogDB.cs
* Gerado em: 06/02/2013
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "IngressoLog_B"

    public abstract class IngressoLog_B : BaseBD
    {

        public ingressoid IngressoID = new ingressoid();
        public usuarioid UsuarioID = new usuarioid();
        public timestamp TimeStamp = new timestamp();
        public acao Acao = new acao();
        public precoid PrecoID = new precoid();
        public cortesiaid CortesiaID = new cortesiaid();
        public bloqueioid BloqueioID = new bloqueioid();
        public vendabilheteriaitemid VendaBilheteriaItemID = new vendabilheteriaitemid();
        public obs Obs = new obs();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public caixaid CaixaID = new caixaid();
        public lojaid LojaID = new lojaid();
        public canalid CanalID = new canalid();
        public empresaid EmpresaID = new empresaid();
        public clienteid ClienteID = new clienteid();
        public codigobarra CodigoBarra = new codigobarra();
        public codigoimpressao CodigoImpressao = new codigoimpressao();
        public motivoid MotivoId = new motivoid();
        public supervisorid SupervisorID = new supervisorid();
        public gerenciamentoingressosid GerenciamentoIngressosID = new gerenciamentoingressosid();
        public assinaturaclienteid AssinaturaClienteID = new assinaturaclienteid();

        public IngressoLog_B() { }

        // passar o Usuario logado no sistema
        public IngressoLog_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de IngressoLog
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tIngressoLog WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.CortesiaID.ValorBD = bd.LerInt("CortesiaID").ToString();
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.VendaBilheteriaItemID.ValorBD = bd.LerInt("VendaBilheteriaItemID").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.CodigoImpressao.ValorBD = bd.LerInt("CodigoImpressao").ToString();
                    this.MotivoId.ValorBD = bd.LerString("MotivoId");
                    this.SupervisorID.ValorBD = bd.LerInt("SupervisorID").ToString();
                    this.GerenciamentoIngressosID.ValorBD = bd.LerInt("GerenciamentoIngressosID").ToString();
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
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
        /// Preenche todos os atributos de IngressoLog do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xIngressoLog WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.CortesiaID.ValorBD = bd.LerInt("CortesiaID").ToString();
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.VendaBilheteriaItemID.ValorBD = bd.LerInt("VendaBilheteriaItemID").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.CodigoImpressao.ValorBD = bd.LerInt("CodigoImpressao").ToString();
                    this.MotivoId.ValorBD = bd.LerString("MotivoId");
                    this.SupervisorID.ValorBD = bd.LerInt("SupervisorID").ToString();
                    this.GerenciamentoIngressosID.ValorBD = bd.LerInt("GerenciamentoIngressosID").ToString();
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
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
                sql.Append("INSERT INTO cIngressoLog (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xIngressoLog (ID, Versao, IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID) ");
                sql.Append("SELECT ID, @V, IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID FROM tIngressoLog WHERE ID = @I");
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
        /// Inserir novo(a) IngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tIngressoLog(IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005,@006,@007,@008,'@009',@010,@011,@012,@013,@014,@015,'@016',@017,'@018',@019,@020,@021) SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

                this.Control.Versao = 0;
                this.Control.ID = Convert.ToInt32(bd.ConsultaValor());

                if (this.Control.ID > 0)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return this.Control.ID > 0;
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
        /// Inserir novo(a) IngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tIngressoLog(IngressoID, UsuarioID, TimeStamp, Acao, PrecoID, CortesiaID, BloqueioID, VendaBilheteriaItemID, Obs, VendaBilheteriaID, CaixaID, LojaID, CanalID, EmpresaID, ClienteID, CodigoBarra, CodigoImpressao, MotivoId, SupervisorID, GerenciamentoIngressosID, AssinaturaClienteID) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005,@006,@007,@008,'@009',@010,@011,@012,@013,@014,@015,'@016',@017,'@018',@019,@020,@021)");

                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

                this.Control.Versao = 0;
                this.Control.ID = Convert.ToInt32(bd.ConsultaValor());

                if (this.Control.ID > 0)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return this.Control.ID > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza IngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tIngressoLog SET IngressoID = @001, UsuarioID = @002, TimeStamp = '@003', Acao = '@004', PrecoID = @005, CortesiaID = @006, BloqueioID = @007, VendaBilheteriaItemID = @008, Obs = '@009', VendaBilheteriaID = @010, CaixaID = @011, LojaID = @012, CanalID = @013, EmpresaID = @014, ClienteID = @015, CodigoBarra = '@016', CodigoImpressao = @017, MotivoId = '@018', SupervisorID = @019, GerenciamentoIngressosID = @020, AssinaturaClienteID = @021 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

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
        /// Atualiza IngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tIngressoLog SET IngressoID = @001, UsuarioID = @002, TimeStamp = '@003', Acao = '@004', PrecoID = @005, CortesiaID = @006, BloqueioID = @007, VendaBilheteriaItemID = @008, Obs = '@009', VendaBilheteriaID = @010, CaixaID = @011, LojaID = @012, CanalID = @013, EmpresaID = @014, ClienteID = @015, CodigoBarra = '@016', CodigoImpressao = @017, MotivoId = '@018', SupervisorID = @019, GerenciamentoIngressosID = @020, AssinaturaClienteID = @021 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.TimeStamp.ValorBD);
                sql.Replace("@004", this.Acao.ValorBD);
                sql.Replace("@005", this.PrecoID.ValorBD);
                sql.Replace("@006", this.CortesiaID.ValorBD);
                sql.Replace("@007", this.BloqueioID.ValorBD);
                sql.Replace("@008", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@011", this.CaixaID.ValorBD);
                sql.Replace("@012", this.LojaID.ValorBD);
                sql.Replace("@013", this.CanalID.ValorBD);
                sql.Replace("@014", this.EmpresaID.ValorBD);
                sql.Replace("@015", this.ClienteID.ValorBD);
                sql.Replace("@016", this.CodigoBarra.ValorBD);
                sql.Replace("@017", this.CodigoImpressao.ValorBD);
                sql.Replace("@018", this.MotivoId.ValorBD);
                sql.Replace("@019", this.SupervisorID.ValorBD);
                sql.Replace("@020", this.GerenciamentoIngressosID.ValorBD);
                sql.Replace("@021", this.AssinaturaClienteID.ValorBD);

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
        /// Exclui IngressoLog com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tIngressoLog WHERE ID=" + id;

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
        /// Exclui IngressoLog com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tIngressoLog WHERE ID=" + id;

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
        /// Exclui IngressoLog
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

            this.IngressoID.Limpar();
            this.UsuarioID.Limpar();
            this.TimeStamp.Limpar();
            this.Acao.Limpar();
            this.PrecoID.Limpar();
            this.CortesiaID.Limpar();
            this.BloqueioID.Limpar();
            this.VendaBilheteriaItemID.Limpar();
            this.Obs.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.CaixaID.Limpar();
            this.LojaID.Limpar();
            this.CanalID.Limpar();
            this.EmpresaID.Limpar();
            this.ClienteID.Limpar();
            this.CodigoBarra.Limpar();
            this.CodigoImpressao.Limpar();
            this.MotivoId.Limpar();
            this.SupervisorID.Limpar();
            this.GerenciamentoIngressosID.Limpar();
            this.AssinaturaClienteID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.IngressoID.Desfazer();
            this.UsuarioID.Desfazer();
            this.TimeStamp.Desfazer();
            this.Acao.Desfazer();
            this.PrecoID.Desfazer();
            this.CortesiaID.Desfazer();
            this.BloqueioID.Desfazer();
            this.VendaBilheteriaItemID.Desfazer();
            this.Obs.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.CaixaID.Desfazer();
            this.LojaID.Desfazer();
            this.CanalID.Desfazer();
            this.EmpresaID.Desfazer();
            this.ClienteID.Desfazer();
            this.CodigoBarra.Desfazer();
            this.CodigoImpressao.Desfazer();
            this.MotivoId.Desfazer();
            this.SupervisorID.Desfazer();
            this.GerenciamentoIngressosID.Desfazer();
            this.AssinaturaClienteID.Desfazer();
        }

        public class ingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressoID";
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

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
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

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStamp";
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

        public class acao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class cortesiaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CortesiaID";
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

        public class bloqueioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BloqueioID";
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

        public class vendabilheteriaitemid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaItemID";
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

        public class obs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Obs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class vendabilheteriaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaID";
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

        public class caixaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CaixaID";
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

        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
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

        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
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

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
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

        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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

        public class codigobarra : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoBarra";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 18;
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

        public class codigoimpressao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoImpressao";
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

        public class motivoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoId";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class supervisorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SupervisorID";
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

        public class gerenciamentoingressosid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "GerenciamentoIngressosID";
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

        public class assinaturaclienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaClienteID";
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

                DataTable tabela = new DataTable("IngressoLog");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CortesiaID", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("CodigoImpressao", typeof(int));
                tabela.Columns.Add("MotivoId", typeof(string));
                tabela.Columns.Add("SupervisorID", typeof(int));
                tabela.Columns.Add("GerenciamentoIngressosID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract void Ingresso(Ingresso ingresso);

        public abstract string IngressoIDsPorAcao(string ingressoids);

    }
    #endregion

    #region "IngressoLogLista_B"

    public abstract class IngressoLogLista_B : BaseLista
    {

        private bool backup = false;
        protected IngressoLog ingressoLog;

        // passar o Usuario logado no sistema
        public IngressoLogLista_B()
        {
            ingressoLog = new IngressoLog();
        }

        public IngressoLog IngressoLog
        {
            get { return ingressoLog; }
        }

        /// <summary>
        /// Retorna um IBaseBD de IngressoLog especifico
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
                    ingressoLog.Ler(id);
                    return ingressoLog;
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
                    sql = "SELECT ID FROM tIngressoLog (NOLOCK) ";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoLog (NOLOCK) ";

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
                    sql = "SELECT ID FROM tIngressoLog";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoLog";

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
                    sql = "SELECT ID FROM xIngressoLog";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xIngressoLog";

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
        /// Preenche IngressoLog corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    ingressoLog.Ler(id);
                else
                    ingressoLog.LerBackup(id);

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

                bool ok = ingressoLog.Excluir();
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
        /// Inseri novo(a) IngressoLog na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = ingressoLog.Inserir();
                if (ok)
                {
                    lista.Add(ingressoLog.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de IngressoLog carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("IngressoLog");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CortesiaID", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("CodigoImpressao", typeof(int));
                tabela.Columns.Add("MotivoId", typeof(string));
                tabela.Columns.Add("SupervisorID", typeof(int));
                tabela.Columns.Add("GerenciamentoIngressosID", typeof(int));
                tabela.Columns.Add("AssinaturaClienteID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = ingressoLog.Control.ID;
                        linha["IngressoID"] = ingressoLog.IngressoID.Valor;
                        linha["UsuarioID"] = ingressoLog.UsuarioID.Valor;
                        linha["TimeStamp"] = ingressoLog.TimeStamp.Valor;
                        linha["Acao"] = ingressoLog.Acao.Valor;
                        linha["PrecoID"] = ingressoLog.PrecoID.Valor;
                        linha["CortesiaID"] = ingressoLog.CortesiaID.Valor;
                        linha["BloqueioID"] = ingressoLog.BloqueioID.Valor;
                        linha["VendaBilheteriaItemID"] = ingressoLog.VendaBilheteriaItemID.Valor;
                        linha["Obs"] = ingressoLog.Obs.Valor;
                        linha["VendaBilheteriaID"] = ingressoLog.VendaBilheteriaID.Valor;
                        linha["CaixaID"] = ingressoLog.CaixaID.Valor;
                        linha["LojaID"] = ingressoLog.LojaID.Valor;
                        linha["CanalID"] = ingressoLog.CanalID.Valor;
                        linha["EmpresaID"] = ingressoLog.EmpresaID.Valor;
                        linha["ClienteID"] = ingressoLog.ClienteID.Valor;
                        linha["CodigoBarra"] = ingressoLog.CodigoBarra.Valor;
                        linha["CodigoImpressao"] = ingressoLog.CodigoImpressao.Valor;
                        linha["MotivoId"] = ingressoLog.MotivoId.Valor;
                        linha["SupervisorID"] = ingressoLog.SupervisorID.Valor;
                        linha["GerenciamentoIngressosID"] = ingressoLog.GerenciamentoIngressosID.Valor;
                        linha["AssinaturaClienteID"] = ingressoLog.AssinaturaClienteID.Valor;
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

                DataTable tabela = new DataTable("RelatorioIngressoLog");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("IngressoID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("CortesiaID", typeof(int));
                    tabela.Columns.Add("BloqueioID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("CaixaID", typeof(int));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("CodigoImpressao", typeof(int));
                    tabela.Columns.Add("MotivoId", typeof(string));
                    tabela.Columns.Add("SupervisorID", typeof(int));
                    tabela.Columns.Add("GerenciamentoIngressosID", typeof(int));
                    tabela.Columns.Add("AssinaturaClienteID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["IngressoID"] = ingressoLog.IngressoID.Valor;
                        linha["UsuarioID"] = ingressoLog.UsuarioID.Valor;
                        linha["TimeStamp"] = ingressoLog.TimeStamp.Valor;
                        linha["Acao"] = ingressoLog.Acao.Valor;
                        linha["PrecoID"] = ingressoLog.PrecoID.Valor;
                        linha["CortesiaID"] = ingressoLog.CortesiaID.Valor;
                        linha["BloqueioID"] = ingressoLog.BloqueioID.Valor;
                        linha["VendaBilheteriaItemID"] = ingressoLog.VendaBilheteriaItemID.Valor;
                        linha["Obs"] = ingressoLog.Obs.Valor;
                        linha["VendaBilheteriaID"] = ingressoLog.VendaBilheteriaID.Valor;
                        linha["CaixaID"] = ingressoLog.CaixaID.Valor;
                        linha["LojaID"] = ingressoLog.LojaID.Valor;
                        linha["CanalID"] = ingressoLog.CanalID.Valor;
                        linha["EmpresaID"] = ingressoLog.EmpresaID.Valor;
                        linha["ClienteID"] = ingressoLog.ClienteID.Valor;
                        linha["CodigoBarra"] = ingressoLog.CodigoBarra.Valor;
                        linha["CodigoImpressao"] = ingressoLog.CodigoImpressao.Valor;
                        linha["MotivoId"] = ingressoLog.MotivoId.Valor;
                        linha["SupervisorID"] = ingressoLog.SupervisorID.Valor;
                        linha["GerenciamentoIngressosID"] = ingressoLog.GerenciamentoIngressosID.Valor;
                        linha["AssinaturaClienteID"] = ingressoLog.AssinaturaClienteID.Valor;
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
                    case "IngressoID":
                        sql = "SELECT ID, IngressoID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY IngressoID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "CortesiaID":
                        sql = "SELECT ID, CortesiaID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY CortesiaID";
                        break;
                    case "BloqueioID":
                        sql = "SELECT ID, BloqueioID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY BloqueioID";
                        break;
                    case "VendaBilheteriaItemID":
                        sql = "SELECT ID, VendaBilheteriaItemID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaItemID";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "CaixaID":
                        sql = "SELECT ID, CaixaID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY CaixaID";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "CodigoImpressao":
                        sql = "SELECT ID, CodigoImpressao FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY CodigoImpressao";
                        break;
                    case "MotivoId":
                        sql = "SELECT ID, MotivoId FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY MotivoId";
                        break;
                    case "SupervisorID":
                        sql = "SELECT ID, SupervisorID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY SupervisorID";
                        break;
                    case "GerenciamentoIngressosID":
                        sql = "SELECT ID, GerenciamentoIngressosID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY GerenciamentoIngressosID";
                        break;
                    case "AssinaturaClienteID":
                        sql = "SELECT ID, AssinaturaClienteID FROM tIngressoLog WHERE " + FiltroSQL + " ORDER BY AssinaturaClienteID";
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

    #region "IngressoLogException"

    [Serializable]
    public class IngressoLogException : Exception
    {

        public IngressoLogException() : base() { }

        public IngressoLogException(string msg) : base(msg) { }

        public IngressoLogException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}