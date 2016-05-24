/******************************************************
* Arquivo FormaPagamentoDB.cs
* Gerado em: 05/07/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "FormaPagamento_B"

    public abstract class FormaPagamento_B : BaseBD
    {

        public nome Nome = new nome();
        public tipo Tipo = new tipo();
        public formapagamentotipoid FormaPagamentoTipoID = new formapagamentotipoid();
        public parcelas Parcelas = new parcelas();
        public bandeiraid BandeiraID = new bandeiraid();
        public padrao Padrao = new padrao();
        public taxaadministrativa TaxaAdministrativa = new taxaadministrativa();
        public diasrepasse DiasRepasse = new diasrepasse();
        public redepreferencial RedePreferencial = new redepreferencial();
        public ativo Ativo = new ativo();

        public FormaPagamento_B() { }

        // passar o Usuario logado no sistema
        public FormaPagamento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de FormaPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Tipo.ValorBD = bd.LerInt("Tipo").ToString();
                    this.FormaPagamentoTipoID.ValorBD = bd.LerInt("FormaPagamentoTipoID").ToString();
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.Padrao.ValorBD = bd.LerString("Padrao");
                    this.TaxaAdministrativa.ValorBD = bd.LerDecimal("TaxaAdministrativa").ToString();
                    this.DiasRepasse.ValorBD = bd.LerInt("DiasRepasse").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.RedePreferencial.ValorBD = bd.LerString("RedePreferencial");
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
        /// Preenche todos os atributos de FormaPagamento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Tipo.ValorBD = bd.LerInt("Tipo").ToString();
                    this.FormaPagamentoTipoID.ValorBD = bd.LerInt("FormaPagamentoTipoID").ToString();
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.BandeiraID.ValorBD = bd.LerInt("BandeiraID").ToString();
                    this.Padrao.ValorBD = bd.LerString("Padrao");
                    this.TaxaAdministrativa.ValorBD = bd.LerDecimal("TaxaAdministrativa").ToString();
                    this.DiasRepasse.ValorBD = bd.LerInt("DiasRepasse").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.RedePreferencial.ValorBD = bd.LerString("RedePreferencial");
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
                sql.Append("INSERT INTO cFormaPagamento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xFormaPagamento (ID, Versao, Nome, Tipo, FormaPagamentoTipoID, Parcelas, BandeiraID, Padrao, TaxaAdministrativa, DiasRepasse, Ativo, RedePreferencial) ");
                sql.Append("SELECT ID, @V, Nome, Tipo, FormaPagamentoTipoID, Parcelas, BandeiraID, Padrao, TaxaAdministrativa, DiasRepasse, Ativo, RedePreferencial  FROM tFormaPagamento WHERE ID = @I");
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
        /// Inserir novo(a) FormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFormaPagamento(ID, Nome, Tipo, FormaPagamentoTipoID, Parcelas, BandeiraID, Padrao, TaxaAdministrativa, DiasRepasse, Ativo, RedePreferencial) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,@004,@005,'@006','@007',@008,'@009','@010')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.FormaPagamentoTipoID.ValorBD);
                sql.Replace("@004", this.Parcelas.ValorBD);
                sql.Replace("@005", this.BandeiraID.ValorBD);
                sql.Replace("@006", this.Padrao.ValorBD);
                sql.Replace("@007", this.TaxaAdministrativa.ValorBD);
                sql.Replace("@008", this.DiasRepasse.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);
                sql.Replace("@010", this.RedePreferencial.ValorBD);

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
        /// Inserir novo(a) FormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFormaPagamento(ID, Nome, Tipo, FormaPagamentoTipoID, Parcelas, BandeiraID, Padrao, TaxaAdministrativa, DiasRepasse, Ativo) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,@004,@005,'@006','@007',@008,'@009','@010')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.FormaPagamentoTipoID.ValorBD);
                sql.Replace("@004", this.Parcelas.ValorBD);
                sql.Replace("@005", this.BandeiraID.ValorBD);
                sql.Replace("@006", this.Padrao.ValorBD);
                sql.Replace("@007", this.TaxaAdministrativa.ValorBD);
                sql.Replace("@008", this.DiasRepasse.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);
                sql.Replace("@010", this.RedePreferencial.ValorBD);

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
        /// Atualiza FormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tFormaPagamento SET Nome = '@001', Tipo = @002, FormaPagamentoTipoID = @003, Parcelas = @004, BandeiraID = @005, Padrao = '@006', TaxaAdministrativa = '@007', DiasRepasse = @008, Ativo = '@009', RedePreferencial = '@010' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.FormaPagamentoTipoID.ValorBD);
                sql.Replace("@004", this.Parcelas.ValorBD);
                sql.Replace("@005", this.BandeiraID.ValorBD);
                sql.Replace("@006", this.Padrao.ValorBD);
                sql.Replace("@007", this.TaxaAdministrativa.ValorBD);
                sql.Replace("@008", this.DiasRepasse.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);
                sql.Replace("@010", this.RedePreferencial.ValorBD);

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
        /// Atualiza FormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tFormaPagamento SET Nome = '@001', Tipo = @002, FormaPagamentoTipoID = @003, Parcelas = @004, BandeiraID = @005, Padrao = '@006', TaxaAdministrativa = '@007', DiasRepasse = @008, Ativo = '@009', RedePreferencial='@010' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.FormaPagamentoTipoID.ValorBD);
                sql.Replace("@004", this.Parcelas.ValorBD);
                sql.Replace("@005", this.BandeiraID.ValorBD);
                sql.Replace("@006", this.Padrao.ValorBD);
                sql.Replace("@007", this.TaxaAdministrativa.ValorBD);
                sql.Replace("@008", this.DiasRepasse.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);
                sql.Replace("@010", this.RedePreferencial.ValorBD);

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
        /// Exclui FormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tFormaPagamento WHERE ID=" + id;

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
        /// Exclui FormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cFormaPagamento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tFormaPagamento WHERE ID=" + id;

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
        /// Exclui FormaPagamento
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
            this.Tipo.Limpar();
            this.FormaPagamentoTipoID.Limpar();
            this.Parcelas.Limpar();
            this.BandeiraID.Limpar();
            this.Padrao.Limpar();
            this.TaxaAdministrativa.Limpar();
            this.DiasRepasse.Limpar();
            this.Ativo.Limpar();
            this.Control.ID = 0;
            this.RedePreferencial.Limpar();
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.Tipo.Desfazer();
            this.FormaPagamentoTipoID.Desfazer();
            this.Parcelas.Desfazer();
            this.BandeiraID.Desfazer();
            this.Padrao.Desfazer();
            this.TaxaAdministrativa.Desfazer();
            this.DiasRepasse.Desfazer();
            this.RedePreferencial.Desfazer();
            this.Ativo.Desfazer();
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

        public class tipo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Tipo";
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

        public class formapagamentotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoTipoID";
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

        public class parcelas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Parcelas";
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

        public class bandeiraid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BandeiraID";
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

        public class padrao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Padrao";
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

        public class taxaadministrativa : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaAdministrativa";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class diasrepasse : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DiasRepasse";
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

        public class redepreferencial:TextProperty
        {
            public override string Nome
            {
                get
                {
                    return "RedePreferencial";
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

                DataTable tabela = new DataTable("FormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Tipo", typeof(int));
                tabela.Columns.Add("FormaPagamentoTipoID", typeof(int));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("Padrao", typeof(bool));
                tabela.Columns.Add("TaxaAdministrativa", typeof(decimal));
                tabela.Columns.Add("DiasRepasse", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("RedePreferencial", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Todas();

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract decimal ValorIngressosPorFormaPagamento(string ingressologids);

        public abstract decimal QuantidadeIngressosPorFormaPagamento(string ingressologids);

        public abstract decimal ValorConvenienciaPorFormaPagamento(string ingressologids);

        public abstract decimal ValorEntregaPorFormaPagamento(string ingressologids);

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

    }
    #endregion

    #region "FormaPagamentoLista_B"

    public abstract class FormaPagamentoLista_B : BaseLista
    {

        private bool backup = false;
        protected FormaPagamento formaPagamento;

        // passar o Usuario logado no sistema
        public FormaPagamentoLista_B()
        {
            formaPagamento = new FormaPagamento();
        }

        // passar o Usuario logado no sistema
        public FormaPagamentoLista_B(int usuarioIDLogado)
        {
            formaPagamento = new FormaPagamento(usuarioIDLogado);
        }

        public FormaPagamento FormaPagamento
        {
            get { return formaPagamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de FormaPagamento especifico
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
                    formaPagamento.Ler(id);
                    return formaPagamento;
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
                    sql = "SELECT ID FROM tFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFormaPagamento";

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
                    sql = "SELECT ID FROM tFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFormaPagamento";

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
                    sql = "SELECT ID FROM xFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xFormaPagamento";

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
        /// Preenche FormaPagamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    formaPagamento.Ler(id);
                else
                    formaPagamento.LerBackup(id);

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

                bool ok = formaPagamento.Excluir();
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
        /// Inseri novo(a) FormaPagamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = formaPagamento.Inserir();
                if (ok)
                {
                    lista.Add(formaPagamento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de FormaPagamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("FormaPagamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Tipo", typeof(int));
                tabela.Columns.Add("FormaPagamentoTipoID", typeof(int));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("BandeiraID", typeof(int));
                tabela.Columns.Add("Padrao", typeof(bool));
                tabela.Columns.Add("TaxaAdministrativa", typeof(decimal));
                tabela.Columns.Add("DiasRepasse", typeof(int));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("RedePreferencial", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = formaPagamento.Control.ID;
                        linha["Nome"] = formaPagamento.Nome.Valor;
                        linha["Tipo"] = formaPagamento.Tipo.Valor;
                        linha["FormaPagamentoTipoID"] = formaPagamento.FormaPagamentoTipoID.Valor;
                        linha["Parcelas"] = formaPagamento.Parcelas.Valor;
                        linha["BandeiraID"] = formaPagamento.BandeiraID.Valor;
                        linha["Padrao"] = formaPagamento.Padrao.Valor;
                        linha["TaxaAdministrativa"] = formaPagamento.TaxaAdministrativa.Valor;
                        linha["DiasRepasse"] = formaPagamento.DiasRepasse.Valor;
                        linha["Ativo"] = formaPagamento.Ativo.Valor;
                        linha["RedePreferencial"] = formaPagamento.RedePreferencial.Valor;
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

                DataTable tabela = new DataTable("RelatorioFormaPagamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Tipo", typeof(int));
                    tabela.Columns.Add("FormaPagamentoTipoID", typeof(int));
                    tabela.Columns.Add("Parcelas", typeof(int));
                    tabela.Columns.Add("BandeiraID", typeof(int));
                    tabela.Columns.Add("Padrao", typeof(bool));
                    tabela.Columns.Add("TaxaAdministrativa", typeof(decimal));
                    tabela.Columns.Add("DiasRepasse", typeof(int));
                    tabela.Columns.Add("Ativo", typeof(bool));
                    tabela.Columns.Add("RedePreferencial", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = formaPagamento.Nome.Valor;
                        linha["Tipo"] = formaPagamento.Tipo.Valor;
                        linha["FormaPagamentoTipoID"] = formaPagamento.FormaPagamentoTipoID.Valor;
                        linha["Parcelas"] = formaPagamento.Parcelas.Valor;
                        linha["BandeiraID"] = formaPagamento.BandeiraID.Valor;
                        linha["Padrao"] = formaPagamento.Padrao.Valor;
                        linha["TaxaAdministrativa"] = formaPagamento.TaxaAdministrativa.Valor;
                        linha["DiasRepasse"] = formaPagamento.DiasRepasse.Valor;
                        linha["Ativo"] = formaPagamento.Ativo.Valor;
                        linha["RedePreferencial"] = formaPagamento.RedePreferencial.Valor;
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
                        sql = "SELECT ID, Nome FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY Tipo";
                        break;
                    case "FormaPagamentoTipoID":
                        sql = "SELECT ID, FormaPagamentoTipoID FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY FormaPagamentoTipoID";
                        break;
                    case "Parcelas":
                        sql = "SELECT ID, Parcelas FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY Parcelas";
                        break;
                    case "BandeiraID":
                        sql = "SELECT ID, BandeiraID FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY BandeiraID";
                        break;
                    case "Padrao":
                        sql = "SELECT ID, Padrao FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY Padrao";
                        break;
                    case "TaxaAdministrativa":
                        sql = "SELECT ID, TaxaAdministrativa FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY TaxaAdministrativa";
                        break;
                    case "DiasRepasse":
                        sql = "SELECT ID, DiasRepasse FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY DiasRepasse";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tFormaPagamento WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    case "RedePreferencial":
                        sql = "SELECT ID, RedePreferencial, FROM tFormaPagamento where " + FiltroSQL + " ORDER BY RedePreferencial";
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

    #region "FormaPagamentoException"

    [Serializable]
    public class FormaPagamentoException : Exception
    {

        public FormaPagamentoException() : base() { }

        public FormaPagamentoException(string msg) : base(msg) { }

        public FormaPagamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}