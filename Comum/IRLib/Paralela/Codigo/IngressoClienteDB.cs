/******************************************************
* Arquivo IngressoClienteDB.cs
* Gerado em: 10/07/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "IngressoCliente_B"

    public abstract class IngressoCliente_B : BaseBD
    {

        public ingressoid IngressoID = new ingressoid();
        public cotaitemid CotaItemID = new cotaitemid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public apresentacaosetorid ApresentacaoSetorID = new apresentacaosetorid();
        public donoid DonoID = new donoid();
        public codigopromocional CodigoPromocional = new codigopromocional();
        public cpf CPF = new cpf();

        public IngressoCliente_B() { }

        /// <summary>
        /// Preenche todos os atributos de IngressoCliente
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tIngressoCliente WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
                    this.CotaItemID.ValorBD = bd.LerInt("CotaItemID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.DonoID.ValorBD = bd.LerInt("DonoID").ToString();
                    this.CodigoPromocional.ValorBD = bd.LerString("CodigoPromocional");
                    this.CPF.ValorBD = bd.LerString("CPF");
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
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Inserir novo(a) IngressoCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            try
            {
                var sql = new StringBuilder();

                sql.Append("INSERT INTO tIngressoCliente(IngressoID, CotaItemID, ApresentacaoID, ApresentacaoSetorID, DonoID, CodigoPromocional, CPF) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,'@006','@007') SELECT IDENTITY_SCOPE();");

                sql.Replace("@001", IngressoID.ValorBD);
                sql.Replace("@002", CotaItemID.ValorBD);
                sql.Replace("@003", ApresentacaoID.ValorBD);
                sql.Replace("@004", ApresentacaoSetorID.ValorBD);
                sql.Replace("@005", DonoID.ValorBD);
                sql.Replace("@006", CodigoPromocional.ValorBD);
                sql.Replace("@007", CPF.ValorBD);

                Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return Control.ID > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Inserir novo(a) IngressoCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {
            try
            {
                var sql = new StringBuilder();

                sql.Append("INSERT INTO tIngressoCliente(IngressoID, CotaItemID, ApresentacaoID, ApresentacaoSetorID, DonoID, CodigoPromocional, CPF) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,'@006','@007') SELECT IDENTITY_SCOPE();");

                sql.Replace("@001", IngressoID.ValorBD);
                sql.Replace("@002", CotaItemID.ValorBD);
                sql.Replace("@003", ApresentacaoID.ValorBD);
                sql.Replace("@004", ApresentacaoSetorID.ValorBD);
                sql.Replace("@005", DonoID.ValorBD);
                sql.Replace("@006", CodigoPromocional.ValorBD);
                sql.Replace("@007", CPF.ValorBD);

                Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return Control.ID > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }


        /// <summary>
        /// Atualiza IngressoCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tIngressoCliente SET IngressoID = @001, CotaItemID = @002, ApresentacaoID = @003, ApresentacaoSetorID = @004, DonoID = @005, CodigoPromocional = '@006', CPF = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.IngressoID.ValorBD);
                sql.Replace("@002", this.CotaItemID.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@005", this.DonoID.ValorBD);
                sql.Replace("@006", this.CodigoPromocional.ValorBD);
                sql.Replace("@007", this.CPF.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Atualiza IngressoCliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tIngressoCliente SET IngressoID = @001, CotaItemID = @002, ApresentacaoID = @003, ApresentacaoSetorID = @004, DonoID = @005, CodigoPromocional = '@006', CPF = '@007' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.IngressoID.ValorBD);
            sql.Replace("@002", this.CotaItemID.ValorBD);
            sql.Replace("@003", this.ApresentacaoID.ValorBD);
            sql.Replace("@004", this.ApresentacaoSetorID.ValorBD);
            sql.Replace("@005", this.DonoID.ValorBD);
            sql.Replace("@006", this.CodigoPromocional.ValorBD);
            sql.Replace("@007", this.CPF.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui IngressoCliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tIngressoCliente WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Exclui IngressoCliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tIngressoCliente WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui IngressoCliente
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

            this.IngressoID.Limpar();
            this.CotaItemID.Limpar();
            this.ApresentacaoID.Limpar();
            this.ApresentacaoSetorID.Limpar();
            this.DonoID.Limpar();
            this.CodigoPromocional.Limpar();
            this.CPF.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.IngressoID.Desfazer();
            this.CotaItemID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.ApresentacaoSetorID.Desfazer();
            this.DonoID.Desfazer();
            this.CodigoPromocional.Desfazer();
            this.CPF.Desfazer();
        }

        public class ingressoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressoID";
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

        public class cotaitemid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CotaItemID";
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

        public class apresentacaosetorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoSetorID";
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

        public class donoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DonoID";
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

        public class codigopromocional : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoPromocional";
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

        public class cpf : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPF";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
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

                DataTable tabela = new DataTable("IngressoCliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("CotaItemID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("DonoID", typeof(int));
                tabela.Columns.Add("CodigoPromocional", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "IngressoClienteLista_B"

    public abstract class IngressoClienteLista_B : BaseLista
    {

        protected IngressoCliente ingressoCliente;

        // passar o Usuario logado no sistema
        public IngressoClienteLista_B()
        {
            ingressoCliente = new IngressoCliente();
        }

        public IngressoCliente IngressoCliente
        {
            get { return ingressoCliente; }
        }

        /// <summary>
        /// Retorna um IBaseBD de IngressoCliente especifico
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
                    ingressoCliente.Ler(id);
                    return ingressoCliente;
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
                    sql = "SELECT ID FROM tIngressoCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoCliente";

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
                    sql = "SELECT ID FROM tIngressoCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tIngressoCliente";

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
        /// Preenche IngressoCliente corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                ingressoCliente.Ler(id);

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

                bool ok = ingressoCliente.Excluir();
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

                    try
                    {
                        string ids = ToString();

                        string sqlDelete = "DELETE FROM tIngressoCliente WHERE ID in (" + ids + ")";

                        int x = bd.Executar(sqlDelete);
                        bd.Fechar();

                        ok = Convert.ToBoolean(x);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

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
        /// Inseri novo(a) IngressoCliente na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = ingressoCliente.Inserir();
                if (ok)
                {
                    lista.Add(ingressoCliente.Control.ID);
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
        /// Obtem uma tabela de todos os campos de IngressoCliente carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("IngressoCliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("CotaItemID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("DonoID", typeof(int));
                tabela.Columns.Add("CodigoPromocional", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = ingressoCliente.Control.ID;
                        linha["IngressoID"] = ingressoCliente.IngressoID.Valor;
                        linha["CotaItemID"] = ingressoCliente.CotaItemID.Valor;
                        linha["ApresentacaoID"] = ingressoCliente.ApresentacaoID.Valor;
                        linha["ApresentacaoSetorID"] = ingressoCliente.ApresentacaoSetorID.Valor;
                        linha["DonoID"] = ingressoCliente.DonoID.Valor;
                        linha["CodigoPromocional"] = ingressoCliente.CodigoPromocional.Valor;
                        linha["CPF"] = ingressoCliente.CPF.Valor;
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

                DataTable tabela = new DataTable("RelatorioIngressoCliente");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("IngressoID", typeof(int));
                    tabela.Columns.Add("CotaItemID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                    tabela.Columns.Add("DonoID", typeof(int));
                    tabela.Columns.Add("CodigoPromocional", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["IngressoID"] = ingressoCliente.IngressoID.Valor;
                        linha["CotaItemID"] = ingressoCliente.CotaItemID.Valor;
                        linha["ApresentacaoID"] = ingressoCliente.ApresentacaoID.Valor;
                        linha["ApresentacaoSetorID"] = ingressoCliente.ApresentacaoSetorID.Valor;
                        linha["DonoID"] = ingressoCliente.DonoID.Valor;
                        linha["CodigoPromocional"] = ingressoCliente.CodigoPromocional.Valor;
                        linha["CPF"] = ingressoCliente.CPF.Valor;
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
                    case "IngressoID":
                        sql = "SELECT ID, IngressoID FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY IngressoID";
                        break;
                    case "CotaItemID":
                        sql = "SELECT ID, CotaItemID FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY CotaItemID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "ApresentacaoSetorID":
                        sql = "SELECT ID, ApresentacaoSetorID FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    case "DonoID":
                        sql = "SELECT ID, DonoID FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY DonoID";
                        break;
                    case "CodigoPromocional":
                        sql = "SELECT ID, CodigoPromocional FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY CodigoPromocional";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tIngressoCliente WHERE " + FiltroSQL + " ORDER BY CPF";
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

    #region "IngressoClienteException"

    [Serializable]
    public class IngressoClienteException : Exception
    {

        public IngressoClienteException() : base() { }

        public IngressoClienteException(string msg) : base(msg) { }

        public IngressoClienteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}