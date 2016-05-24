/******************************************************
* Arquivo EntregaControleDB.cs
* Gerado em: 06/01/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "EntregaControle_B"

    public abstract class EntregaControle_B : BaseBD
    {

        public entregaid EntregaID = new entregaid();
        public entregaareaid EntregaAreaID = new entregaareaid();
        public periodoid PeriodoID = new periodoid();
        public quantidadeentregas QuantidadeEntregas = new quantidadeentregas();
        public valor Valor = new valor();
        public diastriagem DiasTriagem = new diastriagem();
        public procedimentoentrega ProcedimentoEntrega = new procedimentoentrega();

        public EntregaControle_B() { }

        // passar o Usuario logado no sistema
        public EntregaControle_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de EntregaControle
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEntregaControle WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EntregaID.ValorBD = bd.LerInt("EntregaID").ToString();
                    this.EntregaAreaID.ValorBD = bd.LerInt("EntregaAreaID").ToString();
                    this.PeriodoID.ValorBD = bd.LerInt("PeriodoID").ToString();
                    this.QuantidadeEntregas.ValorBD = bd.LerInt("QuantidadeEntregas").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();
                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");
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
        /// Preenche todos os atributos de EntregaControle do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEntregaControle WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EntregaID.ValorBD = bd.LerInt("EntregaID").ToString();
                    this.EntregaAreaID.ValorBD = bd.LerInt("EntregaAreaID").ToString();
                    this.PeriodoID.ValorBD = bd.LerInt("PeriodoID").ToString();
                    this.QuantidadeEntregas.ValorBD = bd.LerInt("QuantidadeEntregas").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();
                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");
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
                sql.Append("INSERT INTO cEntregaControle (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEntregaControle (ID, Versao, EntregaID, EntregaAreaID, PeriodoID, QuantidadeEntregas, Valor, DiasTriagem, ProcedimentoEntrega) ");
                sql.Append("SELECT ID, @V, EntregaID, EntregaAreaID, PeriodoID, QuantidadeEntregas, Valor, DiasTriagem, ProcedimentoEntrega FROM tEntregaControle WHERE ID = @I");
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
        /// Inserir novo(a) EntregaControle
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEntregaControle");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEntregaControle(ID, EntregaID, EntregaAreaID, PeriodoID, QuantidadeEntregas, Valor, DiasTriagem, ProcedimentoEntrega) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,'@005',@006,'@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EntregaID.ValorBD);
                sql.Replace("@002", this.EntregaAreaID.ValorBD);
                sql.Replace("@003", this.PeriodoID.ValorBD);
                sql.Replace("@004", this.QuantidadeEntregas.ValorBD);
                sql.Replace("@005", this.Valor.ValorBD);
                sql.Replace("@006", this.DiasTriagem.ValorBD);
                sql.Replace("@007", this.ProcedimentoEntrega.ValorBD);

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
        /// Atualiza EntregaControle
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEntregaControle WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEntregaControle SET EntregaID = @001, EntregaAreaID = @002, PeriodoID = @003, QuantidadeEntregas = @004, Valor = '@005', DiasTriagem = @006, ProcedimentoEntrega = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EntregaID.ValorBD);
                sql.Replace("@002", this.EntregaAreaID.ValorBD);
                sql.Replace("@003", this.PeriodoID.ValorBD);
                sql.Replace("@004", this.QuantidadeEntregas.ValorBD);
                sql.Replace("@005", this.Valor.ValorBD);
                sql.Replace("@006", this.DiasTriagem.ValorBD);
                sql.Replace("@007", this.ProcedimentoEntrega.ValorBD);

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
        /// Exclui EntregaControle com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEntregaControle WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEntregaControle WHERE ID=" + id;

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
        /// Exclui EntregaControle
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

            this.EntregaID.Limpar();
            this.EntregaAreaID.Limpar();
            this.PeriodoID.Limpar();
            this.QuantidadeEntregas.Limpar();
            this.Valor.Limpar();
            this.DiasTriagem.Limpar();
            this.ProcedimentoEntrega.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EntregaID.Desfazer();
            this.EntregaAreaID.Desfazer();
            this.PeriodoID.Desfazer();
            this.QuantidadeEntregas.Desfazer();
            this.Valor.Desfazer();
            this.DiasTriagem.Desfazer();
            this.ProcedimentoEntrega.Desfazer();
        }

        public class entregaid : IntegerProperty
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

        public class entregaareaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaAreaID";
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

        public class periodoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PeriodoID";
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

        public class quantidadeentregas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeEntregas";
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

                DataTable tabela = new DataTable("EntregaControle");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EntregaID", typeof(int));
                tabela.Columns.Add("EntregaAreaID", typeof(int));
                tabela.Columns.Add("PeriodoID", typeof(int));
                tabela.Columns.Add("QuantidadeEntregas", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("DiasTriagem", typeof(int));
                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EntregaControleLista_B"

    public abstract class EntregaControleLista_B : BaseLista
    {

        private bool backup = false;
        protected EntregaControle entregaControle;

        // passar o Usuario logado no sistema
        public EntregaControleLista_B()
        {
            entregaControle = new EntregaControle();
        }

        // passar o Usuario logado no sistema
        public EntregaControleLista_B(int usuarioIDLogado)
        {
            entregaControle = new EntregaControle(usuarioIDLogado);
        }

        public EntregaControle EntregaControle
        {
            get { return entregaControle; }
        }

        /// <summary>
        /// Retorna um IBaseBD de EntregaControle especifico
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
                    entregaControle.Ler(id);
                    return entregaControle;
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
                    sql = "SELECT ID FROM tEntregaControle";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEntregaControle";

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
                    sql = "SELECT ID FROM tEntregaControle";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEntregaControle";

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
                    sql = "SELECT ID FROM xEntregaControle";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEntregaControle";

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
        /// Preenche EntregaControle corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    entregaControle.Ler(id);
                else
                    entregaControle.LerBackup(id);

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

                bool ok = entregaControle.Excluir();
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
        /// Inseri novo(a) EntregaControle na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = entregaControle.Inserir();
                if (ok)
                {
                    lista.Add(entregaControle.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de EntregaControle carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("EntregaControle");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EntregaID", typeof(int));
                tabela.Columns.Add("EntregaAreaID", typeof(int));
                tabela.Columns.Add("PeriodoID", typeof(int));
                tabela.Columns.Add("QuantidadeEntregas", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("DiasTriagem", typeof(int));
                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = entregaControle.Control.ID;
                        linha["EntregaID"] = entregaControle.EntregaID.Valor;
                        linha["EntregaAreaID"] = entregaControle.EntregaAreaID.Valor;
                        linha["PeriodoID"] = entregaControle.PeriodoID.Valor;
                        linha["QuantidadeEntregas"] = entregaControle.QuantidadeEntregas.Valor;
                        linha["Valor"] = entregaControle.Valor.Valor;
                        linha["DiasTriagem"] = entregaControle.DiasTriagem.Valor;
                        linha["ProcedimentoEntrega"] = entregaControle.ProcedimentoEntrega.Valor;
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

                DataTable tabela = new DataTable("RelatorioEntregaControle");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EntregaID", typeof(int));
                    tabela.Columns.Add("EntregaAreaID", typeof(int));
                    tabela.Columns.Add("PeriodoID", typeof(int));
                    tabela.Columns.Add("QuantidadeEntregas", typeof(int));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("DiasTriagem", typeof(int));
                    tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EntregaID"] = entregaControle.EntregaID.Valor;
                        linha["EntregaAreaID"] = entregaControle.EntregaAreaID.Valor;
                        linha["PeriodoID"] = entregaControle.PeriodoID.Valor;
                        linha["QuantidadeEntregas"] = entregaControle.QuantidadeEntregas.Valor;
                        linha["Valor"] = entregaControle.Valor.Valor;
                        linha["DiasTriagem"] = entregaControle.DiasTriagem.Valor;
                        linha["ProcedimentoEntrega"] = entregaControle.ProcedimentoEntrega.Valor;
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
                    case "EntregaID":
                        sql = "SELECT ID, EntregaID FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY EntregaID";
                        break;
                    case "EntregaAreaID":
                        sql = "SELECT ID, EntregaAreaID FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY EntregaAreaID";
                        break;
                    case "PeriodoID":
                        sql = "SELECT ID, PeriodoID FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY PeriodoID";
                        break;
                    case "QuantidadeEntregas":
                        sql = "SELECT ID, QuantidadeEntregas FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY QuantidadeEntregas";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "DiasTriagem":
                        sql = "SELECT ID, DiasTriagem FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY DiasTriagem";
                        break;
                    case "ProcedimentoEntrega":
                        sql = "SELECT ID, ProcedimentoEntrega FROM tEntregaControle WHERE " + FiltroSQL + " ORDER BY ProcedimentoEntrega";
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

    #region "EntregaControleException"

    [Serializable]
    public class EntregaControleException : Exception
    {

        public EntregaControleException() : base() { }

        public EntregaControleException(string msg) : base(msg) { }

        public EntregaControleException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}