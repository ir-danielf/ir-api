/******************************************************
* Arquivo ContratoPapelDB.cs
* Gerado em: 18/03/2009
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "ContratoPapel_B"

    public abstract class ContratoPapel_B : BaseBD
    {

        public contratoid ContratoID = new contratoid();
        public canaltipoid CanalTipoID = new canaltipoid();
        public ingressonormalvalor IngressoNormalValor = new ingressonormalvalor();
        public preimpressovalor PreImpressoValor = new preimpressovalor();
        public cortesiavalor CortesiaValor = new cortesiavalor();

        public ContratoPapel_B() { }

        // passar o Usuario logado no sistema
        public ContratoPapel_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ContratoPapel
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tContratoPapel WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.CanalTipoID.ValorBD = bd.LerInt("CanalTipoID").ToString();
                    this.IngressoNormalValor.ValorBD = bd.LerDecimal("IngressoNormalValor").ToString();
                    this.PreImpressoValor.ValorBD = bd.LerDecimal("PreImpressoValor").ToString();
                    this.CortesiaValor.ValorBD = bd.LerDecimal("CortesiaValor").ToString();
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
        /// Preenche todos os atributos de ContratoPapel do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xContratoPapel WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.CanalTipoID.ValorBD = bd.LerInt("CanalTipoID").ToString();
                    this.IngressoNormalValor.ValorBD = bd.LerDecimal("IngressoNormalValor").ToString();
                    this.PreImpressoValor.ValorBD = bd.LerDecimal("PreImpressoValor").ToString();
                    this.CortesiaValor.ValorBD = bd.LerDecimal("CortesiaValor").ToString();
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
                sql.Append("INSERT INTO cContratoPapel (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xContratoPapel (ID, Versao, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor) ");
                sql.Append("SELECT ID, @V, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor FROM tContratoPapel WHERE ID = @I");
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
        /// Inserir novo(a) ContratoPapel
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContratoPapel");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContratoPapel(ID, ContratoID, CanalTipoID, IngressoNormalValor, PreImpressoValor, CortesiaValor) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.CanalTipoID.ValorBD);
                sql.Replace("@003", this.IngressoNormalValor.ValorBD);
                sql.Replace("@004", this.PreImpressoValor.ValorBD);
                sql.Replace("@005", this.CortesiaValor.ValorBD);

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
        /// Atualiza ContratoPapel
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cContratoPapel WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContratoPapel SET ContratoID = @001, CanalTipoID = @002, IngressoNormalValor = '@003', PreImpressoValor = '@004', CortesiaValor = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ContratoID.ValorBD);
                sql.Replace("@002", this.CanalTipoID.ValorBD);
                sql.Replace("@003", this.IngressoNormalValor.ValorBD);
                sql.Replace("@004", this.PreImpressoValor.ValorBD);
                sql.Replace("@005", this.CortesiaValor.ValorBD);

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
        /// Exclui ContratoPapel com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cContratoPapel WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tContratoPapel WHERE ID=" + id;

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
        /// Exclui ContratoPapel
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

            this.ContratoID.Limpar();
            this.CanalTipoID.Limpar();
            this.IngressoNormalValor.Limpar();
            this.PreImpressoValor.Limpar();
            this.CortesiaValor.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.ContratoID.Desfazer();
            this.CanalTipoID.Desfazer();
            this.IngressoNormalValor.Desfazer();
            this.PreImpressoValor.Desfazer();
            this.CortesiaValor.Desfazer();
        }

        public class contratoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContratoID";
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

        public class canaltipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalTipoID";
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

        public class ingressonormalvalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressoNormalValor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 7;
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

        public class preimpressovalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "PreImpressoValor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 7;
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

        public class cortesiavalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "CortesiaValor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 7;
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

                DataTable tabela = new DataTable("ContratoPapel");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("CanalTipoID", typeof(int));
                tabela.Columns.Add("IngressoNormalValor", typeof(decimal));
                tabela.Columns.Add("PreImpressoValor", typeof(decimal));
                tabela.Columns.Add("CortesiaValor", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ContratoPapelLista_B"

    public abstract class ContratoPapelLista_B : BaseLista
    {

        private bool backup = false;
        protected ContratoPapel contratoPapel;

        // passar o Usuario logado no sistema
        public ContratoPapelLista_B()
        {
            contratoPapel = new ContratoPapel();
        }

        // passar o Usuario logado no sistema
        public ContratoPapelLista_B(int usuarioIDLogado)
        {
            contratoPapel = new ContratoPapel(usuarioIDLogado);
        }

        public ContratoPapel ContratoPapel
        {
            get { return contratoPapel; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ContratoPapel especifico
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
                    contratoPapel.Ler(id);
                    return contratoPapel;
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
                    sql = "SELECT ID FROM tContratoPapel";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContratoPapel";

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
                    sql = "SELECT ID FROM tContratoPapel";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContratoPapel";

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
                    sql = "SELECT ID FROM xContratoPapel";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xContratoPapel";

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
        /// Preenche ContratoPapel corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    contratoPapel.Ler(id);
                else
                    contratoPapel.LerBackup(id);

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

                bool ok = contratoPapel.Excluir();
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
        /// Inseri novo(a) ContratoPapel na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = contratoPapel.Inserir();
                if (ok)
                {
                    lista.Add(contratoPapel.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ContratoPapel carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ContratoPapel");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("CanalTipoID", typeof(int));
                tabela.Columns.Add("IngressoNormalValor", typeof(decimal));
                tabela.Columns.Add("PreImpressoValor", typeof(decimal));
                tabela.Columns.Add("CortesiaValor", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = contratoPapel.Control.ID;
                        linha["ContratoID"] = contratoPapel.ContratoID.Valor;
                        linha["CanalTipoID"] = contratoPapel.CanalTipoID.Valor;
                        linha["IngressoNormalValor"] = contratoPapel.IngressoNormalValor.Valor;
                        linha["PreImpressoValor"] = contratoPapel.PreImpressoValor.Valor;
                        linha["CortesiaValor"] = contratoPapel.CortesiaValor.Valor;
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

                DataTable tabela = new DataTable("RelatorioContratoPapel");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ContratoID", typeof(int));
                    tabela.Columns.Add("CanalTipoID", typeof(int));
                    tabela.Columns.Add("IngressoNormalValor", typeof(decimal));
                    tabela.Columns.Add("PreImpressoValor", typeof(decimal));
                    tabela.Columns.Add("CortesiaValor", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ContratoID"] = contratoPapel.ContratoID.Valor;
                        linha["CanalTipoID"] = contratoPapel.CanalTipoID.Valor;
                        linha["IngressoNormalValor"] = contratoPapel.IngressoNormalValor.Valor;
                        linha["PreImpressoValor"] = contratoPapel.PreImpressoValor.Valor;
                        linha["CortesiaValor"] = contratoPapel.CortesiaValor.Valor;
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
                    case "ContratoID":
                        sql = "SELECT ID, ContratoID FROM tContratoPapel WHERE " + FiltroSQL + " ORDER BY ContratoID";
                        break;
                    case "CanalTipoID":
                        sql = "SELECT ID, CanalTipoID FROM tContratoPapel WHERE " + FiltroSQL + " ORDER BY CanalTipoID";
                        break;
                    case "IngressoNormalValor":
                        sql = "SELECT ID, IngressoNormalValor FROM tContratoPapel WHERE " + FiltroSQL + " ORDER BY IngressoNormalValor";
                        break;
                    case "PreImpressoValor":
                        sql = "SELECT ID, PreImpressoValor FROM tContratoPapel WHERE " + FiltroSQL + " ORDER BY PreImpressoValor";
                        break;
                    case "CortesiaValor":
                        sql = "SELECT ID, CortesiaValor FROM tContratoPapel WHERE " + FiltroSQL + " ORDER BY CortesiaValor";
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

    #region "ContratoPapelException"

    [Serializable]
    public class ContratoPapelException : Exception
    {

        public ContratoPapelException() : base() { }

        public ContratoPapelException(string msg) : base(msg) { }

        public ContratoPapelException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}