/******************************************************
* Arquivo MensagemDB.cs
* Gerado em: 05/09/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Mensagem_B"

    public abstract class Mensagem_B : BaseBD
    {

        public usuarioid UsuarioID = new usuarioid();
        public mensagem Mensagem = new mensagem();
        public titulo Titulo = new titulo();
        public prioriedade Prioriedade = new prioriedade();
        public permanecerate PermanecerAte = new permanecerate();
        public alteradoem AlteradoEm = new alteradoem();
        public ativo Ativo = new ativo();
        public enviadoem EnviadoEm = new enviadoem();
        public iniciarem IniciarEm = new iniciarem();

        public Mensagem_B() { }

        // passar o Usuario logado no sistema
        public Mensagem_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Mensagem
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tMensagem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Mensagem.ValorBD = bd.LerString("Mensagem");
                    this.Titulo.ValorBD = bd.LerString("Titulo");
                    this.Prioriedade.ValorBD = bd.LerInt("Prioriedade").ToString();
                    this.PermanecerAte.ValorBD = bd.LerString("PermanecerAte");
                    this.AlteradoEm.ValorBD = bd.LerString("AlteradoEm");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.EnviadoEm.ValorBD = bd.LerString("EnviadoEm");
                    this.IniciarEm.ValorBD = bd.LerString("IniciarEm");
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
        /// Preenche todos os atributos de Mensagem do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xMensagem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Mensagem.ValorBD = bd.LerString("Mensagem");
                    this.Titulo.ValorBD = bd.LerString("Titulo");
                    this.Prioriedade.ValorBD = bd.LerInt("Prioriedade").ToString();
                    this.PermanecerAte.ValorBD = bd.LerString("PermanecerAte");
                    this.AlteradoEm.ValorBD = bd.LerString("AlteradoEm");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.EnviadoEm.ValorBD = bd.LerString("EnviadoEm");
                    this.IniciarEm.ValorBD = bd.LerString("IniciarEm");
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
                sql.Append("INSERT INTO cMensagem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xMensagem (ID, Versao, UsuarioID, Mensagem, Titulo, Prioriedade, PermanecerAte, AlteradoEm, Ativo, EnviadoEm, IniciarEm) ");
                sql.Append("SELECT ID, @V, UsuarioID, Mensagem, Titulo, Prioriedade, PermanecerAte, AlteradoEm, Ativo, EnviadoEm, IniciarEm FROM tMensagem WHERE ID = @I");
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
        /// Inserir novo(a) Mensagem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cMensagem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tMensagem(ID, UsuarioID, Mensagem, Titulo, Prioriedade, PermanecerAte, AlteradoEm, Ativo, EnviadoEm, IniciarEm) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005','@006','@007','@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.Mensagem.ValorBD);
                sql.Replace("@003", this.Titulo.ValorBD);
                sql.Replace("@004", this.Prioriedade.ValorBD);
                sql.Replace("@005", this.PermanecerAte.ValorBD);
                sql.Replace("@006", this.AlteradoEm.ValorBD);
                sql.Replace("@007", this.Ativo.ValorBD);
                sql.Replace("@008", this.EnviadoEm.ValorBD);
                sql.Replace("@009", this.IniciarEm.ValorBD);

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
        /// Inserir novo(a) Mensagem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cMensagem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tMensagem(ID, UsuarioID, Mensagem, Titulo, Prioriedade, PermanecerAte, AlteradoEm, Ativo, EnviadoEm, IniciarEm) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005','@006','@007','@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.Mensagem.ValorBD);
                sql.Replace("@003", this.Titulo.ValorBD);
                sql.Replace("@004", this.Prioriedade.ValorBD);
                sql.Replace("@005", this.PermanecerAte.ValorBD);
                sql.Replace("@006", this.AlteradoEm.ValorBD);
                sql.Replace("@007", this.Ativo.ValorBD);
                sql.Replace("@008", this.EnviadoEm.ValorBD);
                sql.Replace("@009", this.IniciarEm.ValorBD);

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
        /// Atualiza Mensagem
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cMensagem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMensagem SET UsuarioID = @001, Mensagem = '@002', Titulo = '@003', Prioriedade = @004, PermanecerAte = '@005', AlteradoEm = '@006', Ativo = '@007', EnviadoEm = '@008', IniciarEm = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.Mensagem.ValorBD);
                sql.Replace("@003", this.Titulo.ValorBD);
                sql.Replace("@004", this.Prioriedade.ValorBD);
                sql.Replace("@005", this.PermanecerAte.ValorBD);
                sql.Replace("@006", this.AlteradoEm.ValorBD);
                sql.Replace("@007", this.Ativo.ValorBD);
                sql.Replace("@008", this.EnviadoEm.ValorBD);
                sql.Replace("@009", this.IniciarEm.ValorBD);

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
        /// Atualiza Mensagem
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cMensagem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMensagem SET UsuarioID = @001, Mensagem = '@002', Titulo = '@003', Prioriedade = @004, PermanecerAte = '@005', AlteradoEm = '@006', Ativo = '@007', EnviadoEm = '@008', IniciarEm = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.UsuarioID.ValorBD);
                sql.Replace("@002", this.Mensagem.ValorBD);
                sql.Replace("@003", this.Titulo.ValorBD);
                sql.Replace("@004", this.Prioriedade.ValorBD);
                sql.Replace("@005", this.PermanecerAte.ValorBD);
                sql.Replace("@006", this.AlteradoEm.ValorBD);
                sql.Replace("@007", this.Ativo.ValorBD);
                sql.Replace("@008", this.EnviadoEm.ValorBD);
                sql.Replace("@009", this.IniciarEm.ValorBD);

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
        /// Exclui Mensagem com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cMensagem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tMensagem WHERE ID=" + id;

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
        /// Exclui Mensagem com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cMensagem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tMensagem WHERE ID=" + id;

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
        /// Exclui Mensagem
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

            this.UsuarioID.Limpar();
            this.Mensagem.Limpar();
            this.Titulo.Limpar();
            this.Prioriedade.Limpar();
            this.PermanecerAte.Limpar();
            this.AlteradoEm.Limpar();
            this.Ativo.Limpar();
            this.EnviadoEm.Limpar();
            this.IniciarEm.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.UsuarioID.Desfazer();
            this.Mensagem.Desfazer();
            this.Titulo.Desfazer();
            this.Prioriedade.Desfazer();
            this.PermanecerAte.Desfazer();
            this.AlteradoEm.Desfazer();
            this.Ativo.Desfazer();
            this.EnviadoEm.Desfazer();
            this.IniciarEm.Desfazer();
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

        public class mensagem : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Mensagem";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 600;
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

        public class titulo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Titulo";
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

        public class prioriedade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Prioriedade";
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

        public class permanecerate : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermanecerAte";
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

        public class alteradoem : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "AlteradoEm";
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

        public class enviadoem : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnviadoEm";
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

        public class iniciarem : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "IniciarEm";
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

                DataTable tabela = new DataTable("Mensagem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Mensagem", typeof(string));
                tabela.Columns.Add("Titulo", typeof(string));
                tabela.Columns.Add("Prioriedade", typeof(int));
                tabela.Columns.Add("PermanecerAte", typeof(DateTime));
                tabela.Columns.Add("AlteradoEm", typeof(DateTime));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("EnviadoEm", typeof(DateTime));
                tabela.Columns.Add("IniciarEm", typeof(DateTime));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "MensagemLista_B"

    public abstract class MensagemLista_B : BaseLista
    {

        private bool backup = false;
        protected Mensagem mensagem;

        // passar o Usuario logado no sistema
        public MensagemLista_B()
        {
            mensagem = new Mensagem();
        }

        // passar o Usuario logado no sistema
        public MensagemLista_B(int usuarioIDLogado)
        {
            mensagem = new Mensagem(usuarioIDLogado);
        }

        public Mensagem Mensagem
        {
            get { return mensagem; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Mensagem especifico
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
                    mensagem.Ler(id);
                    return mensagem;
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
                    sql = "SELECT ID FROM tMensagem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMensagem";

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
                    sql = "SELECT ID FROM tMensagem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMensagem";

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
                    sql = "SELECT ID FROM xMensagem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xMensagem";

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
        /// Preenche Mensagem corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    mensagem.Ler(id);
                else
                    mensagem.LerBackup(id);

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

                bool ok = mensagem.Excluir();
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
        /// Inseri novo(a) Mensagem na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = mensagem.Inserir();
                if (ok)
                {
                    lista.Add(mensagem.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Mensagem carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Mensagem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Mensagem", typeof(string));
                tabela.Columns.Add("Titulo", typeof(string));
                tabela.Columns.Add("Prioriedade", typeof(int));
                tabela.Columns.Add("PermanecerAte", typeof(DateTime));
                tabela.Columns.Add("AlteradoEm", typeof(DateTime));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("EnviadoEm", typeof(DateTime));
                tabela.Columns.Add("IniciarEm", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = mensagem.Control.ID;
                        linha["UsuarioID"] = mensagem.UsuarioID.Valor;
                        linha["Mensagem"] = mensagem.Mensagem.Valor;
                        linha["Titulo"] = mensagem.Titulo.Valor;
                        linha["Prioriedade"] = mensagem.Prioriedade.Valor;
                        linha["PermanecerAte"] = mensagem.PermanecerAte.Valor;
                        linha["AlteradoEm"] = mensagem.AlteradoEm.Valor;
                        linha["Ativo"] = mensagem.Ativo.Valor;
                        linha["EnviadoEm"] = mensagem.EnviadoEm.Valor;
                        linha["IniciarEm"] = mensagem.IniciarEm.Valor;
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

                DataTable tabela = new DataTable("RelatorioMensagem");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("Mensagem", typeof(string));
                    tabela.Columns.Add("Titulo", typeof(string));
                    tabela.Columns.Add("Prioriedade", typeof(int));
                    tabela.Columns.Add("PermanecerAte", typeof(DateTime));
                    tabela.Columns.Add("AlteradoEm", typeof(DateTime));
                    tabela.Columns.Add("Ativo", typeof(bool));
                    tabela.Columns.Add("EnviadoEm", typeof(DateTime));
                    tabela.Columns.Add("IniciarEm", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["UsuarioID"] = mensagem.UsuarioID.Valor;
                        linha["Mensagem"] = mensagem.Mensagem.Valor;
                        linha["Titulo"] = mensagem.Titulo.Valor;
                        linha["Prioriedade"] = mensagem.Prioriedade.Valor;
                        linha["PermanecerAte"] = mensagem.PermanecerAte.Valor;
                        linha["AlteradoEm"] = mensagem.AlteradoEm.Valor;
                        linha["Ativo"] = mensagem.Ativo.Valor;
                        linha["EnviadoEm"] = mensagem.EnviadoEm.Valor;
                        linha["IniciarEm"] = mensagem.IniciarEm.Valor;
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
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tMensagem WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "Mensagem":
                        sql = "SELECT ID, Mensagem FROM tMensagem WHERE " + FiltroSQL + " ORDER BY Mensagem";
                        break;
                    case "Titulo":
                        sql = "SELECT ID, Titulo FROM tMensagem WHERE " + FiltroSQL + " ORDER BY Titulo";
                        break;
                    case "Prioriedade":
                        sql = "SELECT ID, Prioriedade FROM tMensagem WHERE " + FiltroSQL + " ORDER BY Prioriedade";
                        break;
                    case "PermanecerAte":
                        sql = "SELECT ID, PermanecerAte FROM tMensagem WHERE " + FiltroSQL + " ORDER BY PermanecerAte";
                        break;
                    case "AlteradoEm":
                        sql = "SELECT ID, AlteradoEm FROM tMensagem WHERE " + FiltroSQL + " ORDER BY AlteradoEm";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tMensagem WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    case "EnviadoEm":
                        sql = "SELECT ID, EnviadoEm FROM tMensagem WHERE " + FiltroSQL + " ORDER BY EnviadoEm";
                        break;
                    case "IniciarEm":
                        sql = "SELECT ID, IniciarEm FROM tMensagem WHERE " + FiltroSQL + " ORDER BY IniciarEm";
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

    #region "MensagemException"

    [Serializable]
    public class MensagemException : Exception
    {

        public MensagemException() : base() { }

        public MensagemException(string msg) : base(msg) { }

        public MensagemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}