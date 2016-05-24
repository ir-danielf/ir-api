using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "FormaPagamentoEstabelecimento_B"

    public abstract class FormaPagamentoEstabelecimento_B : BaseBD
    {


        public nroestabelecimento NroEstabelecimento = new nroestabelecimento();

        public formapagamentoid FormaPagamentoID = new formapagamentoid();

        public redepreferencial RedePreferencial = new redepreferencial();


        public FormaPagamentoEstabelecimento_B() { }

        // passar o Usuario logado no sistema
        public FormaPagamentoEstabelecimento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de FormaPagamentoEstabelecimento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tFormaPagamentoEstabelecimento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.NroEstabelecimento.ValorBD = bd.LerString("NroEstabelecimento");

                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();

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
        /// Inserir novo(a) FormaPagamentoEstabelecimento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tFormaPagamentoEstabelecimento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                bd.IniciarTransacao();

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFormaPagamentoEstabelecimento(NroEstabelecimento, FormaPagamentoID, RedePreferencial) ");
                sql.Append("VALUES ('@001',@002,'@003')");

                sql.Replace("@001", this.NroEstabelecimento.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.RedePreferencial.ValorBD);


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
        /// Inserir novo(a) FormaPagamentoEstabelecimento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tFormaPagamentoEstabelecimento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tFormaPagamentoEstabelecimento( NroEstabelecimento, FormaPagamentoID, RedePreferencial) ");
                sql.Append("VALUES ('@001',@002,'@003')");
                sql.Replace("@001", this.NroEstabelecimento.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.RedePreferencial.ValorBD);

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
        /// Atualiza FormaPagamentoEstabelecimento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {
            try
            {
                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tFormaPagamentoEstabelecimento SET NroEstabelecimento = '@001', FormaPagamentoID = @002, RedePreferencial = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.NroEstabelecimento.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.RedePreferencial.ValorBD);


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
        /// Atualiza FormaPagamentoEstabelecimento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tFormaPagamentoEstabelecimento SET NroEstabelecimento = '@001', FormaPagamentoID = @002, RedePreferencial = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.NroEstabelecimento.ValorBD);

                sql.Replace("@002", this.FormaPagamentoID.ValorBD);

                sql.Replace("@003", this.RedePreferencial.ValorBD);


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
        /// Exclui FormaPagamentoEstabelecimento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {
                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlDelete = "DELETE FROM tFormaPagamentoEstabelecimento WHERE ID=" + id;

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
        /// Exclui FormaPagamentoEstabelecimento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlDelete = "DELETE FROM tFormaPagamentoEstabelecimento WHERE ID=" + id;

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
        /// Exclui FormaPagamentoEstabelecimento
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


            this.NroEstabelecimento.Limpar();

            this.FormaPagamentoID.Limpar();

            this.RedePreferencial.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.NroEstabelecimento.Desfazer();

            this.FormaPagamentoID.Desfazer();

            this.RedePreferencial.Desfazer();

        }


        public class nroestabelecimento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroEstabelecimento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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


        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
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


        public class redepreferencial : TextProperty
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
                    return 4;
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

                DataTable tabela = new DataTable("FormaPagamentoEstabelecimento");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("NroEstabelecimento", typeof(string));

                tabela.Columns.Add("FormaPagamentoID", typeof(int));

                tabela.Columns.Add("RedePreferencial", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "FormaPagamentoEstabelecimentoLista_B"


    public abstract class FormaPagamentoEstabelecimentoLista_B : BaseLista
    {

        private bool backup = false;
        protected FormaPagamentoEstabelecimento formaPagamentoEstabelecimento;

        // passar o Usuario logado no sistema
        public FormaPagamentoEstabelecimentoLista_B()
        {
            formaPagamentoEstabelecimento = new FormaPagamentoEstabelecimento();
        }

        // passar o Usuario logado no sistema
        public FormaPagamentoEstabelecimentoLista_B(int usuarioIDLogado)
        {
            formaPagamentoEstabelecimento = new FormaPagamentoEstabelecimento(usuarioIDLogado);
        }

        public FormaPagamentoEstabelecimento FormaPagamentoEstabelecimento
        {
            get { return formaPagamentoEstabelecimento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de FormaPagamentoEstabelecimento especifico
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
                    formaPagamentoEstabelecimento.Ler(id);
                    return formaPagamentoEstabelecimento;
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
                    sql = "SELECT ID FROM tFormaPagamentoEstabelecimento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFormaPagamentoEstabelecimento";

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
                    sql = "SELECT ID FROM tFormaPagamentoEstabelecimento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tFormaPagamentoEstabelecimento";

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
        /// Preenche FormaPagamentoEstabelecimento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {
                formaPagamentoEstabelecimento.Ler(id);
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

                bool ok = formaPagamentoEstabelecimento.Excluir();
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
        /// Inseri novo(a) FormaPagamentoEstabelecimento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = formaPagamentoEstabelecimento.Inserir();
                if (ok)
                {
                    lista.Add(formaPagamentoEstabelecimento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de FormaPagamentoEstabelecimento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("FormaPagamentoEstabelecimento");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("NroEstabelecimento", typeof(string));

                tabela.Columns.Add("FormaPagamentoID", typeof(int));

                tabela.Columns.Add("RedePreferencial", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = formaPagamentoEstabelecimento.Control.ID;

                        linha["NroEstabelecimento"] = formaPagamentoEstabelecimento.NroEstabelecimento.Valor;

                        linha["FormaPagamentoID"] = formaPagamentoEstabelecimento.FormaPagamentoID.Valor;

                        linha["RedePreferencial"] = formaPagamentoEstabelecimento.RedePreferencial.Valor;

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

                DataTable tabela = new DataTable("RelatorioFormaPagamentoEstabelecimento");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("NroEstabelecimento", typeof(string));

                    tabela.Columns.Add("FormaPagamentoID", typeof(int));

                    tabela.Columns.Add("RedePreferencial", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["NroEstabelecimento"] = formaPagamentoEstabelecimento.NroEstabelecimento.Valor;

                        linha["FormaPagamentoID"] = formaPagamentoEstabelecimento.FormaPagamentoID.Valor;

                        linha["RedePreferencial"] = formaPagamentoEstabelecimento.RedePreferencial.Valor;

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

                    case "NroEstabelecimento":
                        sql = "SELECT ID, NroEstabelecimento FROM tFormaPagamentoEstabelecimento WHERE " + FiltroSQL + " ORDER BY NroEstabelecimento";
                        break;

                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tFormaPagamentoEstabelecimento WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;

                    case "RedePreferencial":
                        sql = "SELECT ID, RedePreferencial FROM tFormaPagamentoEstabelecimento WHERE " + FiltroSQL + " ORDER BY RedePreferencial";
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

    #region "FormaPagamentoEstabelecimentoException"

    [Serializable]
    public class FormaPagamentoEstabelecimentoException : Exception
    {

        public FormaPagamentoEstabelecimentoException() : base() { }

        public FormaPagamentoEstabelecimentoException(string msg) : base(msg) { }

        public FormaPagamentoEstabelecimentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}