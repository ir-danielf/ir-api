/******************************************************
* Arquivo TaxaEntregaDB.cs
* Gerado em: 21/10/2010
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "TaxaEntrega_B"

    public abstract class TaxaEntrega_B : BaseBD
    {

        public regiaoid RegiaoID = new regiaoid();
        public nome Nome = new nome();
        public valor Valor = new valor();
        public prazo Prazo = new prazo();
        public disponivel Disponivel = new disponivel();
        public obs Obs = new obs();
        public estado Estado = new estado();
        public procedimentoentrega ProcedimentoEntrega = new procedimentoentrega();
        public diastriagem DiasTriagem = new diastriagem();
        public padrao Padrao = new padrao();
        public enviaalerta EnviaAlerta = new enviaalerta();
        public permitirimpressaointernet PermitirImpressaoInternet = new permitirimpressaointernet();

        public TaxaEntrega_B() { }

        // passar o Usuario logado no sistema
        public TaxaEntrega_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de TaxaEntrega
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tTaxaEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.RegiaoID.ValorBD = bd.LerInt("RegiaoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Prazo.ValorBD = bd.LerInt("Prazo").ToString();
                    this.Disponivel.ValorBD = bd.LerString("Disponivel");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");
                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();
                    this.Padrao.ValorBD = bd.LerString("Padrao");
                    this.EnviaAlerta.ValorBD = bd.LerString("EnviaAlerta");
                    this.PermitirImpressaoInternet.ValorBD = bd.LerString("PermitirImpressaoInternet");
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
        /// Preenche todos os atributos de TaxaEntrega do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xTaxaEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.RegiaoID.ValorBD = bd.LerInt("RegiaoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Prazo.ValorBD = bd.LerInt("Prazo").ToString();
                    this.Disponivel.ValorBD = bd.LerString("Disponivel");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");
                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();
                    this.Padrao.ValorBD = bd.LerString("Padrao");
                    this.EnviaAlerta.ValorBD = bd.LerString("EnviaAlerta");
                    this.PermitirImpressaoInternet.ValorBD = bd.LerString("PermitirImpressaoInternet");
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
                sql.Append("INSERT INTO cTaxaEntrega (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xTaxaEntrega (ID, Versao, RegiaoID, Nome, Valor, Prazo, Disponivel, Obs, Estado, ProcedimentoEntrega, DiasTriagem, Padrao, EnviaAlerta, PermitirImpressaoInternet) ");
                sql.Append("SELECT ID, @V, RegiaoID, Nome, Valor, Prazo, Disponivel, Obs, Estado, ProcedimentoEntrega, DiasTriagem, Padrao, EnviaAlerta, PermitirImpressaoInternet FROM tTaxaEntrega WHERE ID = @I");
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
        /// Inserir novo(a) TaxaEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cTaxaEntrega");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tTaxaEntrega(ID, RegiaoID, Nome, Valor, Prazo, Disponivel, Obs, Estado, ProcedimentoEntrega, DiasTriagem, Padrao, EnviaAlerta, PermitirImpressaoInternet) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005','@006','@007','@008',@009,'@010','@011','@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.RegiaoID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Prazo.ValorBD);
                sql.Replace("@005", this.Disponivel.ValorBD);
                sql.Replace("@006", this.Obs.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.ProcedimentoEntrega.ValorBD);
                sql.Replace("@009", this.DiasTriagem.ValorBD);
                sql.Replace("@010", this.Padrao.ValorBD);
                sql.Replace("@011", this.EnviaAlerta.ValorBD);
                sql.Replace("@012", this.PermitirImpressaoInternet.ValorBD);

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
        /// Atualiza TaxaEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cTaxaEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tTaxaEntrega SET RegiaoID = @001, Nome = '@002', Valor = '@003', Prazo = @004, Disponivel = '@005', Obs = '@006', Estado = '@007', ProcedimentoEntrega = '@008', DiasTriagem = @009, Padrao = '@010', EnviaAlerta = '@011', PermitirImpressaoInternet = '@012' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.RegiaoID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Prazo.ValorBD);
                sql.Replace("@005", this.Disponivel.ValorBD);
                sql.Replace("@006", this.Obs.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.ProcedimentoEntrega.ValorBD);
                sql.Replace("@009", this.DiasTriagem.ValorBD);
                sql.Replace("@010", this.Padrao.ValorBD);
                sql.Replace("@011", this.EnviaAlerta.ValorBD);
                sql.Replace("@012", this.PermitirImpressaoInternet.ValorBD);

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
        /// Exclui TaxaEntrega com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cTaxaEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tTaxaEntrega WHERE ID=" + id;

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
        /// Exclui TaxaEntrega
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

            this.RegiaoID.Limpar();
            this.Nome.Limpar();
            this.Valor.Limpar();
            this.Prazo.Limpar();
            this.Disponivel.Limpar();
            this.Obs.Limpar();
            this.Estado.Limpar();
            this.ProcedimentoEntrega.Limpar();
            this.DiasTriagem.Limpar();
            this.Padrao.Limpar();
            this.EnviaAlerta.Limpar();
            this.PermitirImpressaoInternet.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.RegiaoID.Desfazer();
            this.Nome.Desfazer();
            this.Valor.Desfazer();
            this.Prazo.Desfazer();
            this.Disponivel.Desfazer();
            this.Obs.Desfazer();
            this.Estado.Desfazer();
            this.ProcedimentoEntrega.Desfazer();
            this.DiasTriagem.Desfazer();
            this.Padrao.Desfazer();
            this.EnviaAlerta.Desfazer();
            this.PermitirImpressaoInternet.Desfazer();
        }

        public class regiaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "RegiaoID";
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
                    return 8;
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

        public class prazo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Prazo";
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

        public class disponivel : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Disponivel";
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

        public class estado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
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

        public class procedimentoentrega : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProcedimentoEntrega";
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

        public class diastriagem : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DiasTriagem";
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

        public class enviaalerta : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnviaAlerta";
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

        public class permitirimpressaointernet : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermitirImpressaoInternet";
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

                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("RegiaoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Prazo", typeof(int));
                tabela.Columns.Add("Disponivel", typeof(bool));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));
                tabela.Columns.Add("DiasTriagem", typeof(int));
                tabela.Columns.Add("Padrao", typeof(bool));
                tabela.Columns.Add("EnviaAlerta", typeof(bool));
                tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "TaxaEntregaLista_B"

    public abstract class TaxaEntregaLista_B : BaseLista
    {

        private bool backup = false;
        protected TaxaEntrega taxaEntrega;

        // passar o Usuario logado no sistema
        public TaxaEntregaLista_B()
        {
            taxaEntrega = new TaxaEntrega();
        }

        // passar o Usuario logado no sistema
        public TaxaEntregaLista_B(int usuarioIDLogado)
        {
            taxaEntrega = new TaxaEntrega(usuarioIDLogado);
        }

        public TaxaEntrega TaxaEntrega
        {
            get { return taxaEntrega; }
        }

        /// <summary>
        /// Retorna um IBaseBD de TaxaEntrega especifico
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
                    taxaEntrega.Ler(id);
                    return taxaEntrega;
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
                    sql = "SELECT ID FROM tTaxaEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tTaxaEntrega";

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
                    sql = "SELECT ID FROM tTaxaEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tTaxaEntrega";

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
                    sql = "SELECT ID FROM xTaxaEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xTaxaEntrega";

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
        /// Preenche TaxaEntrega corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    taxaEntrega.Ler(id);
                else
                    taxaEntrega.LerBackup(id);

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

                bool ok = taxaEntrega.Excluir();
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
        /// Inseri novo(a) TaxaEntrega na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = taxaEntrega.Inserir();
                if (ok)
                {
                    lista.Add(taxaEntrega.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de TaxaEntrega carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("RegiaoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Prazo", typeof(int));
                tabela.Columns.Add("Disponivel", typeof(bool));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));
                tabela.Columns.Add("DiasTriagem", typeof(int));
                tabela.Columns.Add("Padrao", typeof(bool));
                tabela.Columns.Add("EnviaAlerta", typeof(bool));
                tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = taxaEntrega.Control.ID;
                        linha["RegiaoID"] = taxaEntrega.RegiaoID.Valor;
                        linha["Nome"] = taxaEntrega.Nome.Valor;
                        linha["Valor"] = taxaEntrega.Valor.Valor;
                        linha["Prazo"] = taxaEntrega.Prazo.Valor;
                        linha["Disponivel"] = taxaEntrega.Disponivel.Valor;
                        linha["Obs"] = taxaEntrega.Obs.Valor;
                        linha["Estado"] = taxaEntrega.Estado.Valor;
                        linha["ProcedimentoEntrega"] = taxaEntrega.ProcedimentoEntrega.Valor;
                        linha["DiasTriagem"] = taxaEntrega.DiasTriagem.Valor;
                        linha["Padrao"] = taxaEntrega.Padrao.Valor;
                        linha["EnviaAlerta"] = taxaEntrega.EnviaAlerta.Valor;
                        linha["PermitirImpressaoInternet"] = taxaEntrega.PermitirImpressaoInternet.Valor;
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

                DataTable tabela = new DataTable("RelatorioTaxaEntrega");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("RegiaoID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("Prazo", typeof(int));
                    tabela.Columns.Add("Disponivel", typeof(bool));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("ProcedimentoEntrega", typeof(string));
                    tabela.Columns.Add("DiasTriagem", typeof(int));
                    tabela.Columns.Add("Padrao", typeof(bool));
                    tabela.Columns.Add("EnviaAlerta", typeof(bool));
                    tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["RegiaoID"] = taxaEntrega.RegiaoID.Valor;
                        linha["Nome"] = taxaEntrega.Nome.Valor;
                        linha["Valor"] = taxaEntrega.Valor.Valor;
                        linha["Prazo"] = taxaEntrega.Prazo.Valor;
                        linha["Disponivel"] = taxaEntrega.Disponivel.Valor;
                        linha["Obs"] = taxaEntrega.Obs.Valor;
                        linha["Estado"] = taxaEntrega.Estado.Valor;
                        linha["ProcedimentoEntrega"] = taxaEntrega.ProcedimentoEntrega.Valor;
                        linha["DiasTriagem"] = taxaEntrega.DiasTriagem.Valor;
                        linha["Padrao"] = taxaEntrega.Padrao.Valor;
                        linha["EnviaAlerta"] = taxaEntrega.EnviaAlerta.Valor;
                        linha["PermitirImpressaoInternet"] = taxaEntrega.PermitirImpressaoInternet.Valor;
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
                    case "RegiaoID":
                        sql = "SELECT ID, RegiaoID FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY RegiaoID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "Prazo":
                        sql = "SELECT ID, Prazo FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Prazo";
                        break;
                    case "Disponivel":
                        sql = "SELECT ID, Disponivel FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Disponivel";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "ProcedimentoEntrega":
                        sql = "SELECT ID, ProcedimentoEntrega FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY ProcedimentoEntrega";
                        break;
                    case "DiasTriagem":
                        sql = "SELECT ID, DiasTriagem FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY DiasTriagem";
                        break;
                    case "Padrao":
                        sql = "SELECT ID, Padrao FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY Padrao";
                        break;
                    case "EnviaAlerta":
                        sql = "SELECT ID, EnviaAlerta FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY EnviaAlerta";
                        break;
                    case "PermitirImpressaoInternet":
                        sql = "SELECT ID, PermitirImpressaoInternet FROM tTaxaEntrega WHERE " + FiltroSQL + " ORDER BY PermitirImpressaoInternet";
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

    #region "TaxaEntregaException"

    [Serializable]
    public class TaxaEntregaException : Exception
    {

        public TaxaEntregaException() : base() { }

        public TaxaEntregaException(string msg) : base(msg) { }

        public TaxaEntregaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}