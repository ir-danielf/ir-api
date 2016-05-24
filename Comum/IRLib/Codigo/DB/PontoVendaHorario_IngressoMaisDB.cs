/******************************************************
* Arquivo HorarioDB.cs
* Gerado em: 28/11/2007
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Horario_B"

    public abstract class PontoVendaHorario_IngressoMais_B : BaseBD
    {

        public pontovendaid PontoVendaID = new pontovendaid();
        public horarioinicial HorarioInicial = new horarioinicial();
        public horariofinal HorarioFinal = new horariofinal();
        public diasemana DiaSemana = new diasemana();

        public PontoVendaHorario_IngressoMais_B() { }

        // passar o Usuario logado no sistema
        public PontoVendaHorario_IngressoMais_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Horario
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tPontoVendaHorario_IngressoMais WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.PontoVendaID.ValorBD = bd.LerInt("PontoVendaID").ToString();
                    this.HorarioInicial.ValorBD = bd.LerString("HorarioInicial");
                    this.HorarioFinal.ValorBD = bd.LerString("HorarioFinal");
                    this.DiaSemana.ValorBD = bd.LerInt("DiaSemana").ToString();
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
        /// Preenche todos os atributos de Horario do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xPontoVendaHorario_IngressoMais WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.PontoVendaID.ValorBD = bd.LerInt("PontoVendaID").ToString();
                    this.HorarioInicial.ValorBD = bd.LerString("HorarioInicial");
                    this.HorarioFinal.ValorBD = bd.LerString("HorarioFinal");
                    this.DiaSemana.ValorBD = bd.LerInt("DiaSemana").ToString();
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
                sql.Append("INSERT INTO cPontoVendaHorario_IngressoMais (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xPontoVendaHorario_IngressoMais (ID, Versao, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana) ");
                sql.Append("SELECT ID, @V, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana FROM tPontoVendaHorario_IngressoMais WHERE ID = @I");
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
        /// Inserir novo(a) Horario
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPontoVendaHorario_IngressoMais");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPontoVendaHorario_IngressoMais(ID, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.PontoVendaID.ValorBD);
                sql.Replace("@002", this.HorarioInicial.ValorBD);
                sql.Replace("@003", this.HorarioFinal.ValorBD);
                sql.Replace("@004", this.DiaSemana.ValorBD);

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
        /// Atualiza Horario
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cPontoVendaHorario_IngressoMais WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPontoVendaHorario_IngressoMais SET PontoVendaID = @001, HorarioInicial = '@002', HorarioFinal = '@003', DiaSemana = @004 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.PontoVendaID.ValorBD);
                sql.Replace("@002", this.HorarioInicial.ValorBD);
                sql.Replace("@003", this.HorarioFinal.ValorBD);
                sql.Replace("@004", this.DiaSemana.ValorBD);

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
        /// Exclui Horario com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPontoVendaHorario_IngressoMais WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPontoVendaHorario_IngressoMais WHERE ID=" + id;

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
        /// Exclui Horario
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

            this.PontoVendaID.Limpar();
            this.HorarioInicial.Limpar();
            this.HorarioFinal.Limpar();
            this.DiaSemana.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.PontoVendaID.Desfazer();
            this.HorarioInicial.Desfazer();
            this.HorarioFinal.Desfazer();
            this.DiaSemana.Desfazer();
        }

        public class pontovendaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PontoVendaID";
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

        public class horarioinicial : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "HorarioInicial";
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

        public class horariofinal : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "HorarioFinal";
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

        public class diasemana : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DiaSemana";
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

                DataTable tabela = new DataTable("Horario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("PontoVendaID", typeof(int));
                tabela.Columns.Add("HorarioInicial", typeof(string));
                tabela.Columns.Add("HorarioFinal", typeof(string));
                tabela.Columns.Add("DiaSemana", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "HorarioLista_B"

    public abstract class PontoVendaHorarioLista_IngressoMais_B : BaseLista
    {

        private bool backup = false;
        protected PontoVendaHorario_IngressoMais horario;

        // passar o Usuario logado no sistema
        public PontoVendaHorarioLista_IngressoMais_B()
        {
            horario = new PontoVendaHorario_IngressoMais();
        }

        // passar o Usuario logado no sistema
        public PontoVendaHorarioLista_IngressoMais_B(int usuarioIDLogado)
        {
            horario = new PontoVendaHorario_IngressoMais(usuarioIDLogado);
        }

        public PontoVendaHorario_IngressoMais Horario
        {
            get { return horario; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Horario especifico
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
                    horario.Ler(id);
                    return horario;
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
                    sql = "SELECT ID FROM tPontoVendaHorario_IngressoMais";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPontoVendaHorario_IngressoMais";

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
                    sql = "SELECT ID FROM tPontoVendaHorario_IngressoMais";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPontoVendaHorario_IngressoMais";

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
                    sql = "SELECT ID FROM xPontoVendaHorario_IngressoMais";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xPontoVendaHorario_IngressoMais";

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
        /// Preenche Horario corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    horario.Ler(id);
                else
                    horario.LerBackup(id);

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

                bool ok = horario.Excluir();
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
        /// Inseri novo(a) Horario na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = horario.Inserir();
                if (ok)
                {
                    lista.Add(horario.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Horario carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Horario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("PontoVendaID", typeof(int));
                tabela.Columns.Add("HorarioInicial", typeof(string));
                tabela.Columns.Add("HorarioFinal", typeof(string));
                tabela.Columns.Add("DiaSemana", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = horario.Control.ID;
                        linha["PontoVendaID"] = horario.PontoVendaID.Valor;
                        linha["HorarioInicial"] = horario.HorarioInicial.Valor;
                        linha["HorarioFinal"] = horario.HorarioFinal.Valor;
                        linha["DiaSemana"] = horario.DiaSemana.Valor;
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

                DataTable tabela = new DataTable("RelatorioHorario");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("PontoVendaID", typeof(int));
                    tabela.Columns.Add("HorarioInicial", typeof(string));
                    tabela.Columns.Add("HorarioFinal", typeof(string));
                    tabela.Columns.Add("DiaSemana", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["PontoVendaID"] = horario.PontoVendaID.Valor;
                        linha["HorarioInicial"] = horario.HorarioInicial.Valor;
                        linha["HorarioFinal"] = horario.HorarioFinal.Valor;
                        linha["DiaSemana"] = horario.DiaSemana.Valor;
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
                    case "PontoVendaID":
                        sql = "SELECT ID, PontoVendaID FROM tPontoVendaHorario_IngressoMais WHERE " + FiltroSQL + " ORDER BY PontoVendaID";
                        break;
                    case "HorarioInicial":
                        sql = "SELECT ID, HorarioInicial FROM tPontoVendaHorario_IngressoMais WHERE " + FiltroSQL + " ORDER BY HorarioInicial";
                        break;
                    case "HorarioFinal":
                        sql = "SELECT ID, HorarioFinal FROM tPontoVendaHorario_IngressoMais WHERE " + FiltroSQL + " ORDER BY HorarioFinal";
                        break;
                    case "DiaSemana":
                        sql = "SELECT ID, DiaSemana FROM tPontoVendaHorario_IngressoMais WHERE " + FiltroSQL + " ORDER BY DiaSemana";
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

    #region "HorarioException"

    [Serializable]
    public class HorarioException_IngressoMais : Exception
    {

        public HorarioException_IngressoMais() : base() { }

        public HorarioException_IngressoMais(string msg) : base(msg) { }

        public HorarioException_IngressoMais(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}