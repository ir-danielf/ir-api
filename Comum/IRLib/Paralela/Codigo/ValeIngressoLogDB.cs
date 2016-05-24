/******************************************************
* Arquivo ValeIngressoLogDB.cs
* Gerado em: 01/04/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "ValeIngressoLog_B"

    public abstract class ValeIngressoLog_B : BaseBD
    {

        public acao Acao = new acao();
        public timestamp TimeStamp = new timestamp();
        public valeingressoid ValeIngressoID = new valeingressoid();
        public usuarioid UsuarioID = new usuarioid();
        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public vendabilheteriaitemid VendaBilheteriaItemID = new vendabilheteriaitemid();
        public empresaid EmpresaID = new empresaid();
        public caixaid CaixaID = new caixaid();
        public lojaid LojaID = new lojaid();
        public canalid CanalID = new canalid();
        public codigotroca CodigoTroca = new codigotroca();
        public codigobarra CodigoBarra = new codigobarra();
        public clientenome ClienteNome = new clientenome();
        public obs Obs = new obs();
        public motivoid MotivoID = new motivoid();
        public supervisorid SupervisorID = new supervisorid();

        public ValeIngressoLog_B() { }

        // passar o Usuario logado no sistema
        public ValeIngressoLog_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ValeIngressoLog
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tValeIngressoLog WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.ValeIngressoID.ValorBD = bd.LerInt("ValeIngressoID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.VendaBilheteriaItemID.ValorBD = bd.LerInt("VendaBilheteriaItemID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.CodigoTroca.ValorBD = bd.LerString("CodigoTroca");
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.ClienteNome.ValorBD = bd.LerString("ClienteNome");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();
                    this.SupervisorID.ValorBD = bd.LerInt("SupervisorID").ToString();
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
        /// Preenche todos os atributos de ValeIngressoLog do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xValeIngressoLog WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.ValeIngressoID.ValorBD = bd.LerInt("ValeIngressoID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.VendaBilheteriaItemID.ValorBD = bd.LerInt("VendaBilheteriaItemID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.CodigoTroca.ValorBD = bd.LerString("CodigoTroca");
                    this.CodigoBarra.ValorBD = bd.LerString("CodigoBarra");
                    this.ClienteNome.ValorBD = bd.LerString("ClienteNome");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();
                    this.SupervisorID.ValorBD = bd.LerInt("SupervisorID").ToString();
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
                sql.Append("INSERT INTO cValeIngressoLog (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xValeIngressoLog (ID, Versao, Acao, TimeStamp, ValeIngressoID, UsuarioID, VendaBilheteriaID, VendaBilheteriaItemID, EmpresaID, CaixaID, LojaID, CanalID, CodigoTroca, CodigoBarra, ClienteNome, Obs, MotivoID, SupervisorID) ");
                sql.Append("SELECT ID, @V, Acao, TimeStamp, ValeIngressoID, UsuarioID, VendaBilheteriaID, VendaBilheteriaItemID, EmpresaID, CaixaID, LojaID, CanalID, CodigoTroca, CodigoBarra, ClienteNome, Obs, MotivoID, SupervisorID FROM tValeIngressoLog WHERE ID = @I");
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
        /// Inserir novo(a) ValeIngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cValeIngressoLog");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tValeIngressoLog(ID, Acao, TimeStamp, ValeIngressoID, UsuarioID, VendaBilheteriaID, VendaBilheteriaItemID, EmpresaID, CaixaID, LojaID, CanalID, CodigoTroca, CodigoBarra, ClienteNome, Obs, MotivoID, SupervisorID) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011','@012','@013','@014',@015,@016)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Acao.ValorBD);
                sql.Replace("@002", this.TimeStamp.ValorBD);
                sql.Replace("@003", this.ValeIngressoID.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@007", this.EmpresaID.ValorBD);
                sql.Replace("@008", this.CaixaID.ValorBD);
                sql.Replace("@009", this.LojaID.ValorBD);
                sql.Replace("@010", this.CanalID.ValorBD);
                sql.Replace("@011", this.CodigoTroca.ValorBD);
                sql.Replace("@012", this.CodigoBarra.ValorBD);
                sql.Replace("@013", this.ClienteNome.ValorBD);
                sql.Replace("@014", this.Obs.ValorBD);
                sql.Replace("@015", this.MotivoID.ValorBD);
                sql.Replace("@016", this.SupervisorID.ValorBD);

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
        /// Atualiza ValeIngressoLog
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cValeIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tValeIngressoLog SET Acao = '@001', TimeStamp = '@002', ValeIngressoID = @003, UsuarioID = @004, VendaBilheteriaID = @005, VendaBilheteriaItemID = @006, EmpresaID = @007, CaixaID = @008, LojaID = @009, CanalID = @010, CodigoTroca = '@011', CodigoBarra = '@012', ClienteNome = '@013', Obs = '@014', MotivoID = @015, SupervisorID = @016 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Acao.ValorBD);
                sql.Replace("@002", this.TimeStamp.ValorBD);
                sql.Replace("@003", this.ValeIngressoID.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@006", this.VendaBilheteriaItemID.ValorBD);
                sql.Replace("@007", this.EmpresaID.ValorBD);
                sql.Replace("@008", this.CaixaID.ValorBD);
                sql.Replace("@009", this.LojaID.ValorBD);
                sql.Replace("@010", this.CanalID.ValorBD);
                sql.Replace("@011", this.CodigoTroca.ValorBD);
                sql.Replace("@012", this.CodigoBarra.ValorBD);
                sql.Replace("@013", this.ClienteNome.ValorBD);
                sql.Replace("@014", this.Obs.ValorBD);
                sql.Replace("@015", this.MotivoID.ValorBD);
                sql.Replace("@016", this.SupervisorID.ValorBD);

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
        /// Exclui ValeIngressoLog com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cValeIngressoLog WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tValeIngressoLog WHERE ID=" + id;

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
        /// Exclui ValeIngressoLog
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

            this.Acao.Limpar();
            this.TimeStamp.Limpar();
            this.ValeIngressoID.Limpar();
            this.UsuarioID.Limpar();
            this.VendaBilheteriaID.Limpar();
            this.VendaBilheteriaItemID.Limpar();
            this.EmpresaID.Limpar();
            this.CaixaID.Limpar();
            this.LojaID.Limpar();
            this.CanalID.Limpar();
            this.CodigoTroca.Limpar();
            this.CodigoBarra.Limpar();
            this.ClienteNome.Limpar();
            this.Obs.Limpar();
            this.MotivoID.Limpar();
            this.SupervisorID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Acao.Desfazer();
            this.TimeStamp.Desfazer();
            this.ValeIngressoID.Desfazer();
            this.UsuarioID.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.VendaBilheteriaItemID.Desfazer();
            this.EmpresaID.Desfazer();
            this.CaixaID.Desfazer();
            this.LojaID.Desfazer();
            this.CanalID.Desfazer();
            this.CodigoTroca.Desfazer();
            this.CodigoBarra.Desfazer();
            this.ClienteNome.Desfazer();
            this.Obs.Desfazer();
            this.MotivoID.Desfazer();
            this.SupervisorID.Desfazer();
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

        public class valeingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValeIngressoID";
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

        public class codigotroca : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoTroca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
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
                    return 20;
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

        public class clientenome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteNome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
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

        public class motivoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoID";
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

                DataTable tabela = new DataTable("ValeIngressoLog");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("ValeIngressoID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("CodigoTroca", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("ClienteNome", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("MotivoID", typeof(int));
                tabela.Columns.Add("SupervisorID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ValeIngressoLogLista_B"

    public abstract class ValeIngressoLogLista_B : BaseLista
    {

        private bool backup = false;
        protected ValeIngressoLog valeIngressoLog;

        // passar o Usuario logado no sistema
        public ValeIngressoLogLista_B()
        {
            valeIngressoLog = new ValeIngressoLog();
        }

        public ValeIngressoLog ValeIngressoLog
        {
            get { return valeIngressoLog; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ValeIngressoLog especifico
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
                    valeIngressoLog.Ler(id);
                    return valeIngressoLog;
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
                    sql = "SELECT ID FROM tValeIngressoLog";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngressoLog";

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
                    sql = "SELECT ID FROM tValeIngressoLog";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tValeIngressoLog";

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
                    sql = "SELECT ID FROM xValeIngressoLog";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xValeIngressoLog";

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
        /// Preenche ValeIngressoLog corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    valeIngressoLog.Ler(id);
                else
                    valeIngressoLog.LerBackup(id);

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

                bool ok = valeIngressoLog.Excluir();
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
        /// Inseri novo(a) ValeIngressoLog na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = valeIngressoLog.Inserir();
                if (ok)
                {
                    lista.Add(valeIngressoLog.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ValeIngressoLog carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ValeIngressoLog");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("ValeIngressoID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("CaixaID", typeof(int));
                tabela.Columns.Add("LojaID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("CodigoTroca", typeof(string));
                tabela.Columns.Add("CodigoBarra", typeof(string));
                tabela.Columns.Add("ClienteNome", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("MotivoID", typeof(int));
                tabela.Columns.Add("SupervisorID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = valeIngressoLog.Control.ID;
                        linha["Acao"] = valeIngressoLog.Acao.Valor;
                        linha["TimeStamp"] = valeIngressoLog.TimeStamp.Valor;
                        linha["ValeIngressoID"] = valeIngressoLog.ValeIngressoID.Valor;
                        linha["UsuarioID"] = valeIngressoLog.UsuarioID.Valor;
                        linha["VendaBilheteriaID"] = valeIngressoLog.VendaBilheteriaID.Valor;
                        linha["VendaBilheteriaItemID"] = valeIngressoLog.VendaBilheteriaItemID.Valor;
                        linha["EmpresaID"] = valeIngressoLog.EmpresaID.Valor;
                        linha["CaixaID"] = valeIngressoLog.CaixaID.Valor;
                        linha["LojaID"] = valeIngressoLog.LojaID.Valor;
                        linha["CanalID"] = valeIngressoLog.CanalID.Valor;
                        linha["CodigoTroca"] = valeIngressoLog.CodigoTroca.Valor;
                        linha["CodigoBarra"] = valeIngressoLog.CodigoBarra.Valor;
                        linha["ClienteNome"] = valeIngressoLog.ClienteNome.Valor;
                        linha["Obs"] = valeIngressoLog.Obs.Valor;
                        linha["MotivoID"] = valeIngressoLog.MotivoID.Valor;
                        linha["SupervisorID"] = valeIngressoLog.SupervisorID.Valor;
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

                DataTable tabela = new DataTable("RelatorioValeIngressoLog");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("ValeIngressoID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("CaixaID", typeof(int));
                    tabela.Columns.Add("LojaID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("CodigoTroca", typeof(string));
                    tabela.Columns.Add("CodigoBarra", typeof(string));
                    tabela.Columns.Add("ClienteNome", typeof(string));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("MotivoID", typeof(int));
                    tabela.Columns.Add("SupervisorID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Acao"] = valeIngressoLog.Acao.Valor;
                        linha["TimeStamp"] = valeIngressoLog.TimeStamp.Valor;
                        linha["ValeIngressoID"] = valeIngressoLog.ValeIngressoID.Valor;
                        linha["UsuarioID"] = valeIngressoLog.UsuarioID.Valor;
                        linha["VendaBilheteriaID"] = valeIngressoLog.VendaBilheteriaID.Valor;
                        linha["VendaBilheteriaItemID"] = valeIngressoLog.VendaBilheteriaItemID.Valor;
                        linha["EmpresaID"] = valeIngressoLog.EmpresaID.Valor;
                        linha["CaixaID"] = valeIngressoLog.CaixaID.Valor;
                        linha["LojaID"] = valeIngressoLog.LojaID.Valor;
                        linha["CanalID"] = valeIngressoLog.CanalID.Valor;
                        linha["CodigoTroca"] = valeIngressoLog.CodigoTroca.Valor;
                        linha["CodigoBarra"] = valeIngressoLog.CodigoBarra.Valor;
                        linha["ClienteNome"] = valeIngressoLog.ClienteNome.Valor;
                        linha["Obs"] = valeIngressoLog.Obs.Valor;
                        linha["MotivoID"] = valeIngressoLog.MotivoID.Valor;
                        linha["SupervisorID"] = valeIngressoLog.SupervisorID.Valor;
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
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "ValeIngressoID":
                        sql = "SELECT ID, ValeIngressoID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY ValeIngressoID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "VendaBilheteriaItemID":
                        sql = "SELECT ID, VendaBilheteriaItemID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaItemID";
                        break;
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "CaixaID":
                        sql = "SELECT ID, CaixaID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY CaixaID";
                        break;
                    case "LojaID":
                        sql = "SELECT ID, LojaID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY LojaID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "CodigoTroca":
                        sql = "SELECT ID, CodigoTroca FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY CodigoTroca";
                        break;
                    case "CodigoBarra":
                        sql = "SELECT ID, CodigoBarra FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY CodigoBarra";
                        break;
                    case "ClienteNome":
                        sql = "SELECT ID, ClienteNome FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY ClienteNome";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "MotivoID":
                        sql = "SELECT ID, MotivoID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY MotivoID";
                        break;
                    case "SupervisorID":
                        sql = "SELECT ID, SupervisorID FROM tValeIngressoLog WHERE " + FiltroSQL + " ORDER BY SupervisorID";
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

    #region "ValeIngressoLogException"

    [Serializable]
    public class ValeIngressoLogException : Exception
    {

        public ValeIngressoLogException() : base() { }

        public ValeIngressoLogException(string msg) : base(msg) { }

        public ValeIngressoLogException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}