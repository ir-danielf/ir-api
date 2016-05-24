/******************************************************
* Arquivo ContratoDB.cs
* Gerado em: 16-09-2009
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Contrato_B"

    public abstract class Contrato_B : BaseBD
    {

        public empresaid EmpresaID = new empresaid();
        public usuarioid UsuarioID = new usuarioid();
        public nome Nome = new nome();
        public codigo Codigo = new codigo();
        public datacriacao DataCriacao = new datacriacao();
        public observacoes Observacoes = new observacoes();
        public tiporepasse TipoRepasse = new tiporepasse();
        public tipocomissao TipoComissao = new tipocomissao();
        public tipopapelpagamento TipoPapelPagamento = new tipopapelpagamento();
        public papelcobrancautilizacao PapelCobrancaUtilizacao = new papelcobrancautilizacao();
        public papelcomholografia PapelComHolografia = new papelcomholografia();
        public maximoparcelas MaximoParcelas = new maximoparcelas();

        public Contrato_B() { }

        // passar o Usuario logado no sistema
        public Contrato_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Contrato
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tContrato WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.DataCriacao.ValorBD = bd.LerString("DataCriacao");
                    this.Observacoes.ValorBD = bd.LerString("Observacoes");
                    this.TipoRepasse.ValorBD = bd.LerInt("TipoRepasse").ToString();
                    this.TipoComissao.ValorBD = bd.LerInt("TipoComissao").ToString();
                    this.TipoPapelPagamento.ValorBD = bd.LerInt("TipoPapelPagamento").ToString();
                    this.PapelCobrancaUtilizacao.ValorBD = bd.LerString("PapelCobrancaUtilizacao");
                    this.PapelComHolografia.ValorBD = bd.LerString("PapelComHolografia");
                    this.MaximoParcelas.ValorBD = bd.LerInt("MaximoParcelas").ToString();
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
        /// Preenche todos os atributos de Contrato do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xContrato WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.DataCriacao.ValorBD = bd.LerString("DataCriacao");
                    this.Observacoes.ValorBD = bd.LerString("Observacoes");
                    this.TipoRepasse.ValorBD = bd.LerInt("TipoRepasse").ToString();
                    this.TipoComissao.ValorBD = bd.LerInt("TipoComissao").ToString();
                    this.TipoPapelPagamento.ValorBD = bd.LerInt("TipoPapelPagamento").ToString();
                    this.PapelCobrancaUtilizacao.ValorBD = bd.LerString("PapelCobrancaUtilizacao");
                    this.PapelComHolografia.ValorBD = bd.LerString("PapelComHolografia");
                    this.MaximoParcelas.ValorBD = bd.LerInt("MaximoParcelas").ToString();
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
                sql.Append("INSERT INTO cContrato (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xContrato (ID, Versao, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao, PapelComHolografia, MaximoParcelas) ");
                sql.Append("SELECT ID, @V, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao, PapelComHolografia, MaximoParcelas FROM tContrato WHERE ID = @I");
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
        /// Inserir novo(a) Contrato
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContrato");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContrato(ID, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao, PapelComHolografia, MaximoParcelas) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006',@007,@008,@009,'@010','@011',@012)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Codigo.ValorBD);
                sql.Replace("@005", this.DataCriacao.ValorBD);
                sql.Replace("@006", this.Observacoes.ValorBD);
                sql.Replace("@007", this.TipoRepasse.ValorBD);
                sql.Replace("@008", this.TipoComissao.ValorBD);
                sql.Replace("@009", this.TipoPapelPagamento.ValorBD);
                sql.Replace("@010", this.PapelCobrancaUtilizacao.ValorBD);
                sql.Replace("@011", this.PapelComHolografia.ValorBD);
                sql.Replace("@012", this.MaximoParcelas.ValorBD);

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
        /// Atualiza Contrato
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cContrato WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContrato SET EmpresaID = @001, UsuarioID = @002, Nome = '@003', Codigo = '@004', DataCriacao = '@005', Observacoes = '@006', TipoRepasse = @007, TipoComissao = @008, TipoPapelPagamento = @009, PapelCobrancaUtilizacao = '@010', PapelComHolografia = '@011', MaximoParcelas = @012 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Codigo.ValorBD);
                sql.Replace("@005", this.DataCriacao.ValorBD);
                sql.Replace("@006", this.Observacoes.ValorBD);
                sql.Replace("@007", this.TipoRepasse.ValorBD);
                sql.Replace("@008", this.TipoComissao.ValorBD);
                sql.Replace("@009", this.TipoPapelPagamento.ValorBD);
                sql.Replace("@010", this.PapelCobrancaUtilizacao.ValorBD);
                sql.Replace("@011", this.PapelComHolografia.ValorBD);
                sql.Replace("@012", this.MaximoParcelas.ValorBD);

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
        /// Exclui Contrato com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cContrato WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tContrato WHERE ID=" + id;

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
        /// Exclui Contrato
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

            this.EmpresaID.Limpar();
            this.UsuarioID.Limpar();
            this.Nome.Limpar();
            this.Codigo.Limpar();
            this.DataCriacao.Limpar();
            this.Observacoes.Limpar();
            this.TipoRepasse.Limpar();
            this.TipoComissao.Limpar();
            this.TipoPapelPagamento.Limpar();
            this.PapelCobrancaUtilizacao.Limpar();
            this.PapelComHolografia.Limpar();
            this.MaximoParcelas.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EmpresaID.Desfazer();
            this.UsuarioID.Desfazer();
            this.Nome.Desfazer();
            this.Codigo.Desfazer();
            this.DataCriacao.Desfazer();
            this.Observacoes.Desfazer();
            this.TipoRepasse.Desfazer();
            this.TipoComissao.Desfazer();
            this.TipoPapelPagamento.Desfazer();
            this.PapelCobrancaUtilizacao.Desfazer();
            this.PapelComHolografia.Desfazer();
            this.MaximoParcelas.Desfazer();
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

        public class codigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Codigo";
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

        public class datacriacao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataCriacao";
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

        public class observacoes : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Observacoes";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2000;
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

        public class tiporepasse : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoRepasse";
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

        public class tipocomissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoComissao";
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

        public class tipopapelpagamento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoPapelPagamento";
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

        public class papelcobrancautilizacao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PapelCobrancaUtilizacao";
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

        public class papelcomholografia : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PapelComHolografia";
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

        public class maximoparcelas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MaximoParcelas";
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

                DataTable tabela = new DataTable("Contrato");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("DataCriacao", typeof(DateTime));
                tabela.Columns.Add("Observacoes", typeof(string));
                tabela.Columns.Add("TipoRepasse", typeof(int));
                tabela.Columns.Add("TipoComissao", typeof(int));
                tabela.Columns.Add("TipoPapelPagamento", typeof(int));
                tabela.Columns.Add("PapelCobrancaUtilizacao", typeof(bool));
                tabela.Columns.Add("PapelComHolografia", typeof(bool));
                tabela.Columns.Add("MaximoParcelas", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract ClientObjects.EstruturaContrato GravarContrato(ClientObjects.EstruturaContrato estruturacontrato);

    }
    #endregion

    #region "ContratoLista_B"

    public abstract class ContratoLista_B : BaseLista
    {

        private bool backup = false;
        protected Contrato contrato;

        // passar o Usuario logado no sistema
        public ContratoLista_B()
        {
            contrato = new Contrato();
        }

        // passar o Usuario logado no sistema
        public ContratoLista_B(int usuarioIDLogado)
        {
            contrato = new Contrato(usuarioIDLogado);
        }

        public Contrato Contrato
        {
            get { return contrato; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Contrato especifico
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
                    contrato.Ler(id);
                    return contrato;
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
                    sql = "SELECT ID FROM tContrato";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContrato";

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
                    sql = "SELECT ID FROM tContrato";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContrato";

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
                    sql = "SELECT ID FROM xContrato";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xContrato";

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
        /// Preenche Contrato corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    contrato.Ler(id);
                else
                    contrato.LerBackup(id);

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

                bool ok = contrato.Excluir();
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
        /// Inseri novo(a) Contrato na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = contrato.Inserir();
                if (ok)
                {
                    lista.Add(contrato.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Contrato carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Contrato");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("DataCriacao", typeof(DateTime));
                tabela.Columns.Add("Observacoes", typeof(string));
                tabela.Columns.Add("TipoRepasse", typeof(int));
                tabela.Columns.Add("TipoComissao", typeof(int));
                tabela.Columns.Add("TipoPapelPagamento", typeof(int));
                tabela.Columns.Add("PapelCobrancaUtilizacao", typeof(bool));
                tabela.Columns.Add("PapelComHolografia", typeof(bool));
                tabela.Columns.Add("MaximoParcelas", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = contrato.Control.ID;
                        linha["EmpresaID"] = contrato.EmpresaID.Valor;
                        linha["UsuarioID"] = contrato.UsuarioID.Valor;
                        linha["Nome"] = contrato.Nome.Valor;
                        linha["Codigo"] = contrato.Codigo.Valor;
                        linha["DataCriacao"] = contrato.DataCriacao.Valor;
                        linha["Observacoes"] = contrato.Observacoes.Valor;
                        linha["TipoRepasse"] = contrato.TipoRepasse.Valor;
                        linha["TipoComissao"] = contrato.TipoComissao.Valor;
                        linha["TipoPapelPagamento"] = contrato.TipoPapelPagamento.Valor;
                        linha["PapelCobrancaUtilizacao"] = contrato.PapelCobrancaUtilizacao.Valor;
                        linha["PapelComHolografia"] = contrato.PapelComHolografia.Valor;
                        linha["MaximoParcelas"] = contrato.MaximoParcelas.Valor;
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

                DataTable tabela = new DataTable("RelatorioContrato");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Codigo", typeof(string));
                    tabela.Columns.Add("DataCriacao", typeof(DateTime));
                    tabela.Columns.Add("Observacoes", typeof(string));
                    tabela.Columns.Add("TipoRepasse", typeof(int));
                    tabela.Columns.Add("TipoComissao", typeof(int));
                    tabela.Columns.Add("TipoPapelPagamento", typeof(int));
                    tabela.Columns.Add("PapelCobrancaUtilizacao", typeof(bool));
                    tabela.Columns.Add("PapelComHolografia", typeof(bool));
                    tabela.Columns.Add("MaximoParcelas", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EmpresaID"] = contrato.EmpresaID.Valor;
                        linha["UsuarioID"] = contrato.UsuarioID.Valor;
                        linha["Nome"] = contrato.Nome.Valor;
                        linha["Codigo"] = contrato.Codigo.Valor;
                        linha["DataCriacao"] = contrato.DataCriacao.Valor;
                        linha["Observacoes"] = contrato.Observacoes.Valor;
                        linha["TipoRepasse"] = contrato.TipoRepasse.Valor;
                        linha["TipoComissao"] = contrato.TipoComissao.Valor;
                        linha["TipoPapelPagamento"] = contrato.TipoPapelPagamento.Valor;
                        linha["PapelCobrancaUtilizacao"] = contrato.PapelCobrancaUtilizacao.Valor;
                        linha["PapelComHolografia"] = contrato.PapelComHolografia.Valor;
                        linha["MaximoParcelas"] = contrato.MaximoParcelas.Valor;
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
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tContrato WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tContrato WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tContrato WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Codigo":
                        sql = "SELECT ID, Codigo FROM tContrato WHERE " + FiltroSQL + " ORDER BY Codigo";
                        break;
                    case "DataCriacao":
                        sql = "SELECT ID, DataCriacao FROM tContrato WHERE " + FiltroSQL + " ORDER BY DataCriacao";
                        break;
                    case "Observacoes":
                        sql = "SELECT ID, Observacoes FROM tContrato WHERE " + FiltroSQL + " ORDER BY Observacoes";
                        break;
                    case "TipoRepasse":
                        sql = "SELECT ID, TipoRepasse FROM tContrato WHERE " + FiltroSQL + " ORDER BY TipoRepasse";
                        break;
                    case "TipoComissao":
                        sql = "SELECT ID, TipoComissao FROM tContrato WHERE " + FiltroSQL + " ORDER BY TipoComissao";
                        break;
                    case "TipoPapelPagamento":
                        sql = "SELECT ID, TipoPapelPagamento FROM tContrato WHERE " + FiltroSQL + " ORDER BY TipoPapelPagamento";
                        break;
                    case "PapelCobrancaUtilizacao":
                        sql = "SELECT ID, PapelCobrancaUtilizacao FROM tContrato WHERE " + FiltroSQL + " ORDER BY PapelCobrancaUtilizacao";
                        break;
                    case "PapelComHolografia":
                        sql = "SELECT ID, PapelComHolografia FROM tContrato WHERE " + FiltroSQL + " ORDER BY PapelComHolografia";
                        break;
                    case "MaximoParcelas":
                        sql = "SELECT ID, MaximoParcelas FROM tContrato WHERE " + FiltroSQL + " ORDER BY MaximoParcelas";
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

    #region "ContratoException"

    [Serializable]
    public class ContratoException : Exception
    {

        public ContratoException() : base() { }

        public ContratoException(string msg) : base(msg) { }

        public ContratoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}