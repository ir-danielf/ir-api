/******************************************************
* Arquivo CanalPacoteDB.cs
* Gerado em: 17/07/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "CanalPacote_B"

    public abstract class CanalPacote_B : BaseBD
    {

        public canalid CanalID = new canalid();
        public pacoteid PacoteID = new pacoteid();
        public quantidade Quantidade = new quantidade();
        public taxaconveniencia TaxaConveniencia = new taxaconveniencia();
        public taxaminima TaxaMinima = new taxaminima();
        public taxamaxima TaxaMaxima = new taxamaxima();
        public taxacomissao TaxaComissao = new taxacomissao();
        public comissaominima ComissaoMinima = new comissaominima();
        public comissaomaxima ComissaoMaxima = new comissaomaxima();

        public CanalPacote_B() { }

        // passar o Usuario logado no sistema
        public CanalPacote_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CanalPacote
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCanalPacote WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaMinima.ValorBD = bd.LerDecimal("TaxaMinima").ToString();
                    this.TaxaMaxima.ValorBD = bd.LerDecimal("TaxaMaxima").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.ComissaoMinima.ValorBD = bd.LerDecimal("ComissaoMinima").ToString();
                    this.ComissaoMaxima.ValorBD = bd.LerDecimal("ComissaoMaxima").ToString();
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
        /// Preenche todos os atributos de CanalPacote do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCanalPacote WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaMinima.ValorBD = bd.LerDecimal("TaxaMinima").ToString();
                    this.TaxaMaxima.ValorBD = bd.LerDecimal("TaxaMaxima").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.ComissaoMinima.ValorBD = bd.LerDecimal("ComissaoMinima").ToString();
                    this.ComissaoMaxima.ValorBD = bd.LerDecimal("ComissaoMaxima").ToString();
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
                sql.Append("INSERT INTO cCanalPacote (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCanalPacote (ID, Versao, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima) ");
                sql.Append("SELECT ID, @V, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima FROM tCanalPacote WHERE ID = @I");
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
        /// Inserir novo(a) CanalPacote
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCanalPacote");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCanalPacote(ID, CanalID, PacoteID, Quantidade, TaxaConveniencia, TaxaMinima, TaxaMaxima, TaxaComissao, ComissaoMinima, ComissaoMaxima) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,'@005','@006',@007,'@008','@009')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaMinima.ValorBD);
                sql.Replace("@006", this.TaxaMaxima.ValorBD);
                sql.Replace("@007", this.TaxaComissao.ValorBD);
                sql.Replace("@008", this.ComissaoMinima.ValorBD);
                sql.Replace("@009", this.ComissaoMaxima.ValorBD);

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
        /// Atualiza CanalPacote
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCanalPacote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCanalPacote SET CanalID = @001, PacoteID = @002, Quantidade = @003, TaxaConveniencia = @004, TaxaMinima = '@005', TaxaMaxima = '@006', TaxaComissao = @007, ComissaoMinima = '@008', ComissaoMaxima = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CanalID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaMinima.ValorBD);
                sql.Replace("@006", this.TaxaMaxima.ValorBD);
                sql.Replace("@007", this.TaxaComissao.ValorBD);
                sql.Replace("@008", this.ComissaoMinima.ValorBD);
                sql.Replace("@009", this.ComissaoMaxima.ValorBD);

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
        /// Exclui CanalPacote com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCanalPacote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCanalPacote WHERE ID=" + id;

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
        /// Exclui CanalPacote
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

            this.CanalID.Limpar();
            this.PacoteID.Limpar();
            this.Quantidade.Limpar();
            this.TaxaConveniencia.Limpar();
            this.TaxaMinima.Limpar();
            this.TaxaMaxima.Limpar();
            this.TaxaComissao.Limpar();
            this.ComissaoMinima.Limpar();
            this.ComissaoMaxima.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.CanalID.Desfazer();
            this.PacoteID.Desfazer();
            this.Quantidade.Desfazer();
            this.TaxaConveniencia.Desfazer();
            this.TaxaMinima.Desfazer();
            this.TaxaMaxima.Desfazer();
            this.TaxaComissao.Desfazer();
            this.ComissaoMinima.Desfazer();
            this.ComissaoMaxima.Desfazer();
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

        public class pacoteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PacoteID";
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

        public class quantidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Quantidade";
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

        public class taxaconveniencia : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConveniencia";
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

        public class taxaminima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaMinima";
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

        public class taxamaxima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaMaxima";
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

        public class taxacomissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaComissao";
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

        public class comissaominima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoMinima";
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

        public class comissaomaxima : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoMaxima";
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

                DataTable tabela = new DataTable("CanalPacote");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaMinima", typeof(decimal));
                tabela.Columns.Add("TaxaMaxima", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoMinima", typeof(decimal));
                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Pacotes(int canalid);

        public abstract int QuantidadeDisponivel(int canalid, int pacoteid);

        public abstract int QuantidadeDisponivel();

        public abstract DataTable Canais(int pacoteid);

        public abstract int BuscaTaxaConveniencia(int canalid, int pacoteid);

    }
    #endregion

    #region "CanalPacoteLista_B"

    public abstract class CanalPacoteLista_B : BaseLista
    {

        private bool backup = false;
        protected CanalPacote canalPacote;

        // passar o Usuario logado no sistema
        public CanalPacoteLista_B()
        {
            canalPacote = new CanalPacote();
        }

        // passar o Usuario logado no sistema
        public CanalPacoteLista_B(int usuarioIDLogado)
        {
            canalPacote = new CanalPacote(usuarioIDLogado);
        }

        public CanalPacote CanalPacote
        {
            get { return canalPacote; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CanalPacote especifico
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
                    canalPacote.Ler(id);
                    return canalPacote;
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
                    sql = "SELECT ID FROM tCanalPacote";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanalPacote";

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
                    sql = "SELECT ID FROM tCanalPacote";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCanalPacote";

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
                    sql = "SELECT ID FROM xCanalPacote";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCanalPacote";

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
        /// Preenche CanalPacote corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    canalPacote.Ler(id);
                else
                    canalPacote.LerBackup(id);

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

                bool ok = canalPacote.Excluir();
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
        /// Inseri novo(a) CanalPacote na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = canalPacote.Inserir();
                if (ok)
                {
                    lista.Add(canalPacote.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CanalPacote carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CanalPacote");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaMinima", typeof(decimal));
                tabela.Columns.Add("TaxaMaxima", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoMinima", typeof(decimal));
                tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = canalPacote.Control.ID;
                        linha["CanalID"] = canalPacote.CanalID.Valor;
                        linha["PacoteID"] = canalPacote.PacoteID.Valor;
                        linha["Quantidade"] = canalPacote.Quantidade.Valor;
                        linha["TaxaConveniencia"] = canalPacote.TaxaConveniencia.Valor;
                        linha["TaxaMinima"] = canalPacote.TaxaMinima.Valor;
                        linha["TaxaMaxima"] = canalPacote.TaxaMaxima.Valor;
                        linha["TaxaComissao"] = canalPacote.TaxaComissao.Valor;
                        linha["ComissaoMinima"] = canalPacote.ComissaoMinima.Valor;
                        linha["ComissaoMaxima"] = canalPacote.ComissaoMaxima.Valor;
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

                DataTable tabela = new DataTable("RelatorioCanalPacote");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("PacoteID", typeof(int));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("TaxaConveniencia", typeof(int));
                    tabela.Columns.Add("TaxaMinima", typeof(decimal));
                    tabela.Columns.Add("TaxaMaxima", typeof(decimal));
                    tabela.Columns.Add("TaxaComissao", typeof(int));
                    tabela.Columns.Add("ComissaoMinima", typeof(decimal));
                    tabela.Columns.Add("ComissaoMaxima", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CanalID"] = canalPacote.CanalID.Valor;
                        linha["PacoteID"] = canalPacote.PacoteID.Valor;
                        linha["Quantidade"] = canalPacote.Quantidade.Valor;
                        linha["TaxaConveniencia"] = canalPacote.TaxaConveniencia.Valor;
                        linha["TaxaMinima"] = canalPacote.TaxaMinima.Valor;
                        linha["TaxaMaxima"] = canalPacote.TaxaMaxima.Valor;
                        linha["TaxaComissao"] = canalPacote.TaxaComissao.Valor;
                        linha["ComissaoMinima"] = canalPacote.ComissaoMinima.Valor;
                        linha["ComissaoMaxima"] = canalPacote.ComissaoMaxima.Valor;
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
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "PacoteID":
                        sql = "SELECT ID, PacoteID FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY PacoteID";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "TaxaConveniencia":
                        sql = "SELECT ID, TaxaConveniencia FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY TaxaConveniencia";
                        break;
                    case "TaxaMinima":
                        sql = "SELECT ID, TaxaMinima FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY TaxaMinima";
                        break;
                    case "TaxaMaxima":
                        sql = "SELECT ID, TaxaMaxima FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY TaxaMaxima";
                        break;
                    case "TaxaComissao":
                        sql = "SELECT ID, TaxaComissao FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY TaxaComissao";
                        break;
                    case "ComissaoMinima":
                        sql = "SELECT ID, ComissaoMinima FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY ComissaoMinima";
                        break;
                    case "ComissaoMaxima":
                        sql = "SELECT ID, ComissaoMaxima FROM tCanalPacote WHERE " + FiltroSQL + " ORDER BY ComissaoMaxima";
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

    #region "CanalPacoteException"

    [Serializable]
    public class CanalPacoteException : Exception
    {

        public CanalPacoteException() : base() { }

        public CanalPacoteException(string msg) : base(msg) { }

        public CanalPacoteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}