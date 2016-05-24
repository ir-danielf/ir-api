/******************************************************
* Arquivo DespesasDB.cs
* Gerado em: 01/08/2013
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "Despesas_B"

    public abstract class Despesas_B : BaseBD
    {

        public localid LocalID = new localid();
        public eventoid EventoID = new eventoid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public tipopagamentoid TipoPagamentoID = new tipopagamentoid();
        public nome Nome = new nome();
        public porporcentagem PorPorcentagem = new porporcentagem();
        public valor Valor = new valor();
        public valorminimo ValorMinimo = new valorminimo();
        public obs Obs = new obs();
        public valorliquido ValorLiquido = new valorliquido();
        public tipoformapagamento TipoFormaPagamento = new tipoformapagamento();
        public valoringresso ValorIngresso = new valoringresso();

        public Despesas_B() { }

        // passar o Usuario logado no sistema
        public Despesas_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Despesas
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tDespesas WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.TipoPagamentoID.ValorBD = bd.LerInt("TipoPagamentoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.PorPorcentagem.ValorBD = bd.LerString("PorPorcentagem");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ValorMinimo.ValorBD = bd.LerDecimal("ValorMinimo").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.ValorLiquido.ValorBD = bd.LerString("ValorLiquido");
                    this.TipoFormaPagamento.ValorBD = bd.LerString("TipoFormaPagamento");
                    this.ValorIngresso.ValorBD = bd.LerString("ValorIngresso");
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
        /// Preenche todos os atributos de Despesas do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xDespesas WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.TipoPagamentoID.ValorBD = bd.LerInt("TipoPagamentoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.PorPorcentagem.ValorBD = bd.LerString("PorPorcentagem");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ValorMinimo.ValorBD = bd.LerDecimal("ValorMinimo").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.ValorLiquido.ValorBD = bd.LerString("ValorLiquido");
                    this.TipoFormaPagamento.ValorBD = bd.LerString("TipoFormaPagamento");
                    this.ValorIngresso.ValorBD = bd.LerString("ValorIngresso");
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
                sql.Append("INSERT INTO cDespesas (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xDespesas (ID, Versao, LocalID, EventoID, ApresentacaoID, TipoPagamentoID, Nome, PorPorcentagem, Valor, ValorMinimo, Obs, ValorLiquido, TipoFormaPagamento, ValorIngresso) ");
                sql.Append("SELECT ID, @V, LocalID, EventoID, ApresentacaoID, TipoPagamentoID, Nome, PorPorcentagem, Valor, ValorMinimo, Obs, ValorLiquido, TipoFormaPagamento, ValorIngresso FROM tDespesas WHERE ID = @I");
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
        /// Inserir novo(a) Despesas
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cDespesas");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tDespesas(ID, LocalID, EventoID, ApresentacaoID, TipoPagamentoID, Nome, PorPorcentagem, Valor, ValorMinimo, Obs, ValorLiquido, TipoFormaPagamento, ValorIngresso) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,'@005','@006','@007','@008','@009','@010','@011','@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.TipoPagamentoID.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.PorPorcentagem.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.ValorMinimo.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.ValorLiquido.ValorBD);
                sql.Replace("@011", this.TipoFormaPagamento.ValorBD);
                sql.Replace("@012", this.ValorIngresso.ValorBD);

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
        /// Inserir novo(a) Despesas
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cDespesas");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tDespesas(ID, LocalID, EventoID, ApresentacaoID, TipoPagamentoID, Nome, PorPorcentagem, Valor, ValorMinimo, Obs, ValorLiquido, TipoFormaPagamento, ValorIngresso) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,'@005','@006','@007','@008','@009','@010','@011','@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.TipoPagamentoID.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.PorPorcentagem.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.ValorMinimo.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.ValorLiquido.ValorBD);
                sql.Replace("@011", this.TipoFormaPagamento.ValorBD);
                sql.Replace("@012", this.ValorIngresso.ValorBD);

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
        /// Atualiza Despesas
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cDespesas WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tDespesas SET LocalID = @001, EventoID = @002, ApresentacaoID = @003, TipoPagamentoID = @004, Nome = '@005', PorPorcentagem = '@006', Valor = '@007', ValorMinimo = '@008', Obs = '@009', ValorLiquido = '@010', TipoFormaPagamento = '@011', ValorIngresso = '@012' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.TipoPagamentoID.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.PorPorcentagem.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.ValorMinimo.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.ValorLiquido.ValorBD);
                sql.Replace("@011", this.TipoFormaPagamento.ValorBD);
                sql.Replace("@012", this.ValorIngresso.ValorBD);

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
        /// Atualiza Despesas
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cDespesas WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tDespesas SET LocalID = @001, EventoID = @002, ApresentacaoID = @003, TipoPagamentoID = @004, Nome = '@005', PorPorcentagem = '@006', Valor = '@007', ValorMinimo = '@008', Obs = '@009', ValorLiquido = '@010', TipoFormaPagamento = '@011', ValorIngresso = '@012' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.EventoID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.TipoPagamentoID.ValorBD);
                sql.Replace("@005", this.Nome.ValorBD);
                sql.Replace("@006", this.PorPorcentagem.ValorBD);
                sql.Replace("@007", this.Valor.ValorBD);
                sql.Replace("@008", this.ValorMinimo.ValorBD);
                sql.Replace("@009", this.Obs.ValorBD);
                sql.Replace("@010", this.ValorLiquido.ValorBD);
                sql.Replace("@011", this.TipoFormaPagamento.ValorBD);
                sql.Replace("@012", this.ValorIngresso.ValorBD);

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
        /// Exclui Despesas com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cDespesas WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tDespesas WHERE ID=" + id;

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
        /// Exclui Despesas com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cDespesas WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tDespesas WHERE ID=" + id;

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
        /// Exclui Despesas
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

            this.LocalID.Limpar();
            this.EventoID.Limpar();
            this.ApresentacaoID.Limpar();
            this.TipoPagamentoID.Limpar();
            this.Nome.Limpar();
            this.PorPorcentagem.Limpar();
            this.Valor.Limpar();
            this.ValorMinimo.Limpar();
            this.Obs.Limpar();
            this.ValorLiquido.Limpar();
            this.TipoFormaPagamento.Limpar();
            this.ValorIngresso.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.LocalID.Desfazer();
            this.EventoID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.TipoPagamentoID.Desfazer();
            this.Nome.Desfazer();
            this.PorPorcentagem.Desfazer();
            this.Valor.Desfazer();
            this.ValorMinimo.Desfazer();
            this.Obs.Desfazer();
            this.ValorLiquido.Desfazer();
            this.TipoFormaPagamento.Desfazer();
            this.ValorIngresso.Desfazer();
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

        public class eventoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoID";
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class tipopagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoPagamentoID";
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

        public class porporcentagem : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PorPorcentagem";
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

        public class valor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 5;
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

        public class valorminimo : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorMinimo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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

        public class valorliquido : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorLiquido";
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

        public class tipoformapagamento : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoFormaPagamento";
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

        public class valoringresso : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorIngresso";
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

                DataTable tabela = new DataTable("Despesas");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("TipoPagamentoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PorPorcentagem", typeof(bool));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ValorMinimo", typeof(decimal));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("ValorLiquido", typeof(bool));
                tabela.Columns.Add("TipoFormaPagamento", typeof(bool));
                tabela.Columns.Add("ValorIngresso", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "DespesasLista_B"

    public abstract class DespesasLista_B : BaseLista
    {

        private bool backup = false;
        protected Despesas despesas;

        // passar o Usuario logado no sistema
        public DespesasLista_B()
        {
            despesas = new Despesas();
        }

        // passar o Usuario logado no sistema
        public DespesasLista_B(int usuarioIDLogado)
        {
            despesas = new Despesas(usuarioIDLogado);
        }

        public Despesas Despesas
        {
            get { return despesas; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Despesas especifico
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
                    despesas.Ler(id);
                    return despesas;
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
                    sql = "SELECT ID FROM tDespesas";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tDespesas";

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
                    sql = "SELECT ID FROM tDespesas";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tDespesas";

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
                    sql = "SELECT ID FROM xDespesas";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xDespesas";

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
        /// Preenche Despesas corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    despesas.Ler(id);
                else
                    despesas.LerBackup(id);

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

                bool ok = despesas.Excluir();
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
        /// Inseri novo(a) Despesas na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = despesas.Inserir();
                if (ok)
                {
                    lista.Add(despesas.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Despesas carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Despesas");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("TipoPagamentoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("PorPorcentagem", typeof(bool));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ValorMinimo", typeof(decimal));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("ValorLiquido", typeof(bool));
                tabela.Columns.Add("TipoFormaPagamento", typeof(bool));
                tabela.Columns.Add("ValorIngresso", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = despesas.Control.ID;
                        linha["LocalID"] = despesas.LocalID.Valor;
                        linha["EventoID"] = despesas.EventoID.Valor;
                        linha["ApresentacaoID"] = despesas.ApresentacaoID.Valor;
                        linha["TipoPagamentoID"] = despesas.TipoPagamentoID.Valor;
                        linha["Nome"] = despesas.Nome.Valor;
                        linha["PorPorcentagem"] = despesas.PorPorcentagem.Valor;
                        linha["Valor"] = despesas.Valor.Valor;
                        linha["ValorMinimo"] = despesas.ValorMinimo.Valor;
                        linha["Obs"] = despesas.Obs.Valor;
                        linha["ValorLiquido"] = despesas.ValorLiquido.Valor;
                        linha["TipoFormaPagamento"] = despesas.TipoFormaPagamento.Valor;
                        linha["ValorIngresso"] = despesas.ValorIngresso.Valor;
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

                DataTable tabela = new DataTable("RelatorioDespesas");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("TipoPagamentoID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("PorPorcentagem", typeof(bool));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("ValorMinimo", typeof(decimal));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("ValorLiquido", typeof(bool));
                    tabela.Columns.Add("TipoFormaPagamento", typeof(bool));
                    tabela.Columns.Add("ValorIngresso", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["LocalID"] = despesas.LocalID.Valor;
                        linha["EventoID"] = despesas.EventoID.Valor;
                        linha["ApresentacaoID"] = despesas.ApresentacaoID.Valor;
                        linha["TipoPagamentoID"] = despesas.TipoPagamentoID.Valor;
                        linha["Nome"] = despesas.Nome.Valor;
                        linha["PorPorcentagem"] = despesas.PorPorcentagem.Valor;
                        linha["Valor"] = despesas.Valor.Valor;
                        linha["ValorMinimo"] = despesas.ValorMinimo.Valor;
                        linha["Obs"] = despesas.Obs.Valor;
                        linha["ValorLiquido"] = despesas.ValorLiquido.Valor;
                        linha["TipoFormaPagamento"] = despesas.TipoFormaPagamento.Valor;
                        linha["ValorIngresso"] = despesas.ValorIngresso.Valor;
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
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tDespesas WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tDespesas WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tDespesas WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "TipoPagamentoID":
                        sql = "SELECT ID, TipoPagamentoID FROM tDespesas WHERE " + FiltroSQL + " ORDER BY TipoPagamentoID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tDespesas WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "PorPorcentagem":
                        sql = "SELECT ID, PorPorcentagem FROM tDespesas WHERE " + FiltroSQL + " ORDER BY PorPorcentagem";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tDespesas WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "ValorMinimo":
                        sql = "SELECT ID, ValorMinimo FROM tDespesas WHERE " + FiltroSQL + " ORDER BY ValorMinimo";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tDespesas WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "ValorLiquido":
                        sql = "SELECT ID, ValorLiquido FROM tDespesas WHERE " + FiltroSQL + " ORDER BY ValorLiquido";
                        break;
                    case "TipoFormaPagamento":
                        sql = "SELECT ID, TipoFormaPagamento FROM tDespesas WHERE " + FiltroSQL + " ORDER BY TipoFormaPagamento";
                        break;
                    case "ValorIngresso":
                        sql = "SELECT ID, ValorIngresso FROM tDespesas WHERE " + FiltroSQL + " ORDER BY ValorIngresso";
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

    #region "DespesasException"

    [Serializable]
    public class DespesasException : Exception
    {

        public DespesasException() : base() { }

        public DespesasException(string msg) : base(msg) { }

        public DespesasException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}