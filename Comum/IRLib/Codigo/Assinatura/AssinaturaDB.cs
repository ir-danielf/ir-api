/******************************************************
* Arquivo AssinaturaDB.cs
* Gerado em: 17/10/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Assinatura_B"

    public abstract class Assinatura_B : BaseBD
    {

        public nome Nome = new nome();
        public tipocancelamento TipoCancelamento = new tipocancelamento();
        public assinaturatipoid AssinaturaTipoID = new assinaturatipoid();
        public localid LocalID = new localid();
        public ativo Ativo = new ativo();
        public bloqueioid BloqueioID = new bloqueioid();
        public desistenciabloqueioid DesistenciaBloqueioID = new desistenciabloqueioid();
        public extintobloqueioid ExtintoBloqueioID = new extintobloqueioid();
        public alertaassinante AlertaAssinante = new alertaassinante();

        public Assinatura_B() { }

        // passar o Usuario logado no sistema
        public Assinatura_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Assinatura
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tAssinatura WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.TipoCancelamento.ValorBD = bd.LerString("TipoCancelamento");
                    this.AssinaturaTipoID.ValorBD = bd.LerInt("AssinaturaTipoID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.DesistenciaBloqueioID.ValorBD = bd.LerInt("DesistenciaBloqueioID").ToString();
                    this.ExtintoBloqueioID.ValorBD = bd.LerInt("ExtintoBloqueioID").ToString();
                    this.AlertaAssinante.ValorBD = bd.LerString("AlertaAssinante");
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
        /// Preenche todos os atributos de Assinatura do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xAssinatura WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.TipoCancelamento.ValorBD = bd.LerString("TipoCancelamento");
                    this.AssinaturaTipoID.ValorBD = bd.LerInt("AssinaturaTipoID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.DesistenciaBloqueioID.ValorBD = bd.LerInt("DesistenciaBloqueioID").ToString();
                    this.ExtintoBloqueioID.ValorBD = bd.LerInt("ExtintoBloqueioID").ToString();
                    this.AlertaAssinante.ValorBD = bd.LerString("AlertaAssinante");
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
                sql.Append("INSERT INTO cAssinatura (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xAssinatura (ID, Versao, Nome, TipoCancelamento, AssinaturaTipoID, LocalID, Ativo, BloqueioID, DesistenciaBloqueioID, ExtintoBloqueioID, AlertaAssinante) ");
                sql.Append("SELECT ID, @V, Nome, TipoCancelamento, AssinaturaTipoID, LocalID, Ativo, BloqueioID, DesistenciaBloqueioID, ExtintoBloqueioID, AlertaAssinante FROM tAssinatura WHERE ID = @I");
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
        /// Inserir novo(a) Assinatura
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinatura");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinatura(ID, Nome, TipoCancelamento, AssinaturaTipoID, LocalID, Ativo, BloqueioID, DesistenciaBloqueioID, ExtintoBloqueioID, AlertaAssinante) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,@004,'@005',@006,@007,@008,'@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.TipoCancelamento.ValorBD);
                sql.Replace("@003", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.DesistenciaBloqueioID.ValorBD);
                sql.Replace("@008", this.ExtintoBloqueioID.ValorBD);
                sql.Replace("@009", this.AlertaAssinante.ValorBD);

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
        /// Inserir novo(a) Assinatura
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cAssinatura");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tAssinatura(ID, Nome, TipoCancelamento, AssinaturaTipoID, LocalID, Ativo, BloqueioID, DesistenciaBloqueioID, ExtintoBloqueioID, AlertaAssinante) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,@004,'@005',@006,@007,@008,'@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.TipoCancelamento.ValorBD);
                sql.Replace("@003", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.DesistenciaBloqueioID.ValorBD);
                sql.Replace("@008", this.ExtintoBloqueioID.ValorBD);
                sql.Replace("@009", this.AlertaAssinante.ValorBD);

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
        /// Atualiza Assinatura
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinatura WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinatura SET Nome = '@001', TipoCancelamento = '@002', AssinaturaTipoID = @003, LocalID = @004, Ativo = '@005', BloqueioID = @006, DesistenciaBloqueioID = @007, ExtintoBloqueioID = @008, AlertaAssinante = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.TipoCancelamento.ValorBD);
                sql.Replace("@003", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.DesistenciaBloqueioID.ValorBD);
                sql.Replace("@008", this.ExtintoBloqueioID.ValorBD);
                sql.Replace("@009", this.AlertaAssinante.ValorBD);

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
        /// Atualiza Assinatura
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cAssinatura WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tAssinatura SET Nome = '@001', TipoCancelamento = '@002', AssinaturaTipoID = @003, LocalID = @004, Ativo = '@005', BloqueioID = @006, DesistenciaBloqueioID = @007, ExtintoBloqueioID = @008, AlertaAssinante = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.TipoCancelamento.ValorBD);
                sql.Replace("@003", this.AssinaturaTipoID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.Ativo.ValorBD);
                sql.Replace("@006", this.BloqueioID.ValorBD);
                sql.Replace("@007", this.DesistenciaBloqueioID.ValorBD);
                sql.Replace("@008", this.ExtintoBloqueioID.ValorBD);
                sql.Replace("@009", this.AlertaAssinante.ValorBD);

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
        /// Exclui Assinatura com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinatura WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinatura WHERE ID=" + id;

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
        /// Exclui Assinatura com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cAssinatura WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tAssinatura WHERE ID=" + id;

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
        /// Exclui Assinatura
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
            this.TipoCancelamento.Limpar();
            this.AssinaturaTipoID.Limpar();
            this.LocalID.Limpar();
            this.Ativo.Limpar();
            this.BloqueioID.Limpar();
            this.DesistenciaBloqueioID.Limpar();
            this.ExtintoBloqueioID.Limpar();
            this.AlertaAssinante.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.TipoCancelamento.Desfazer();
            this.AssinaturaTipoID.Desfazer();
            this.LocalID.Desfazer();
            this.Ativo.Desfazer();
            this.BloqueioID.Desfazer();
            this.DesistenciaBloqueioID.Desfazer();
            this.ExtintoBloqueioID.Desfazer();
            this.AlertaAssinante.Desfazer();
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

        public class tipocancelamento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCancelamento";
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

        public class assinaturatipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "AssinaturaTipoID";
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

        public class ativo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
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

        public class desistenciabloqueioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DesistenciaBloqueioID";
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

        public class extintobloqueioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ExtintoBloqueioID";
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

        public class alertaassinante : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AlertaAssinante";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 400;
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

                DataTable tabela = new DataTable("Assinatura");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("TipoCancelamento", typeof(string));
                tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("DesistenciaBloqueioID", typeof(int));
                tabela.Columns.Add("ExtintoBloqueioID", typeof(int));
                tabela.Columns.Add("AlertaAssinante", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "AssinaturaLista_B"

    public abstract class AssinaturaLista_B : BaseLista
    {

        private bool backup = false;
        protected Assinatura assinatura;

        // passar o Usuario logado no sistema
        public AssinaturaLista_B()
        {
            assinatura = new Assinatura();
        }

        // passar o Usuario logado no sistema
        public AssinaturaLista_B(int usuarioIDLogado)
        {
            assinatura = new Assinatura(usuarioIDLogado);
        }

        public Assinatura Assinatura
        {
            get { return assinatura; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Assinatura especifico
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
                    assinatura.Ler(id);
                    return assinatura;
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
                    sql = "SELECT ID FROM tAssinatura";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinatura";

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
                    sql = "SELECT ID FROM tAssinatura";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tAssinatura";

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
                    sql = "SELECT ID FROM xAssinatura";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xAssinatura";

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
        /// Preenche Assinatura corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    assinatura.Ler(id);
                else
                    assinatura.LerBackup(id);

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

                bool ok = assinatura.Excluir();
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
        /// Inseri novo(a) Assinatura na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = assinatura.Inserir();
                if (ok)
                {
                    lista.Add(assinatura.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Assinatura carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Assinatura");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("TipoCancelamento", typeof(string));
                tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("DesistenciaBloqueioID", typeof(int));
                tabela.Columns.Add("ExtintoBloqueioID", typeof(int));
                tabela.Columns.Add("AlertaAssinante", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = assinatura.Control.ID;
                        linha["Nome"] = assinatura.Nome.Valor;
                        linha["TipoCancelamento"] = assinatura.TipoCancelamento.Valor;
                        linha["AssinaturaTipoID"] = assinatura.AssinaturaTipoID.Valor;
                        linha["LocalID"] = assinatura.LocalID.Valor;
                        linha["Ativo"] = assinatura.Ativo.Valor;
                        linha["BloqueioID"] = assinatura.BloqueioID.Valor;
                        linha["DesistenciaBloqueioID"] = assinatura.DesistenciaBloqueioID.Valor;
                        linha["ExtintoBloqueioID"] = assinatura.ExtintoBloqueioID.Valor;
                        linha["AlertaAssinante"] = assinatura.AlertaAssinante.Valor;
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

                DataTable tabela = new DataTable("RelatorioAssinatura");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("TipoCancelamento", typeof(string));
                    tabela.Columns.Add("AssinaturaTipoID", typeof(int));
                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("Ativo", typeof(bool));
                    tabela.Columns.Add("BloqueioID", typeof(int));
                    tabela.Columns.Add("DesistenciaBloqueioID", typeof(int));
                    tabela.Columns.Add("ExtintoBloqueioID", typeof(int));
                    tabela.Columns.Add("AlertaAssinante", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = assinatura.Nome.Valor;
                        linha["TipoCancelamento"] = assinatura.TipoCancelamento.Valor;
                        linha["AssinaturaTipoID"] = assinatura.AssinaturaTipoID.Valor;
                        linha["LocalID"] = assinatura.LocalID.Valor;
                        linha["Ativo"] = assinatura.Ativo.Valor;
                        linha["BloqueioID"] = assinatura.BloqueioID.Valor;
                        linha["DesistenciaBloqueioID"] = assinatura.DesistenciaBloqueioID.Valor;
                        linha["ExtintoBloqueioID"] = assinatura.ExtintoBloqueioID.Valor;
                        linha["AlertaAssinante"] = assinatura.AlertaAssinante.Valor;
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
                        sql = "SELECT ID, Nome FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "TipoCancelamento":
                        sql = "SELECT ID, TipoCancelamento FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY TipoCancelamento";
                        break;
                    case "AssinaturaTipoID":
                        sql = "SELECT ID, AssinaturaTipoID FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY AssinaturaTipoID";
                        break;
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    case "BloqueioID":
                        sql = "SELECT ID, BloqueioID FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY BloqueioID";
                        break;
                    case "DesistenciaBloqueioID":
                        sql = "SELECT ID, DesistenciaBloqueioID FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY DesistenciaBloqueioID";
                        break;
                    case "ExtintoBloqueioID":
                        sql = "SELECT ID, ExtintoBloqueioID FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY ExtintoBloqueioID";
                        break;
                    case "AlertaAssinante":
                        sql = "SELECT ID, AlertaAssinante FROM tAssinatura WHERE " + FiltroSQL + " ORDER BY AlertaAssinante";
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

    #region "AssinaturaException"

    [Serializable]
    public class AssinaturaException : Exception
    {

        public AssinaturaException() : base() { }

        public AssinaturaException(string msg) : base(msg) { }

        public AssinaturaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}