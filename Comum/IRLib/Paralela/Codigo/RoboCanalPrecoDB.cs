using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "RoboCanalPreco_B"

    public abstract class RoboCanalPreco_B : BaseBD
    {

        public precoid PrecoID = new precoid();
        public canalid CanalID = new canalid();
        public datainicio DataInicio = new datainicio();
        public datafim DataFim = new datafim();
        public quantidade Quantidade = new quantidade();
        public operacao Operacao = new operacao();

        public RoboCanalPreco_B() { }

        /// <summary>
        /// Preenche todos os atributos de RoboCanalPreco
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tRoboCanalPreco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.PrecoID.ValorBD = bd.LerInt("PrecoID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.DataInicio.ValorBD = bd.LerString("DataInicio");
                    this.DataFim.ValorBD = bd.LerString("DataFim");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.Operacao.ValorBD = bd.LerString("Operacao");
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
        /// Inserir novo(a) RoboCanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tRoboCanalPreco(PrecoID, CanalID, DataInicio, DataFim, Quantidade, Operacao) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.PrecoID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.DataInicio.ValorBD);
                sql.Replace("@004", this.DataFim.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);
                sql.Replace("@006", this.Operacao.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

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
        /// Inserir novo(a) RoboCanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tRoboCanalPreco(PrecoID, CanalID, DataInicio, DataFim, Quantidade, Operacao) ");
            sql.Append("VALUES (@001,@002,'@003','@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.PrecoID.ValorBD);
            sql.Replace("@002", this.CanalID.ValorBD);
            sql.Replace("@003", this.DataInicio.ValorBD);
            sql.Replace("@004", this.DataFim.ValorBD);
            sql.Replace("@005", this.Quantidade.ValorBD);
            sql.Replace("@006", this.Operacao.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza RoboCanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tRoboCanalPreco SET PrecoID = @001, CanalID = @002, DataInicio = '@003', DataFim = '@004', Quantidade = @005, Operacao = '@006' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.PrecoID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.DataInicio.ValorBD);
                sql.Replace("@004", this.DataFim.ValorBD);
                sql.Replace("@005", this.Quantidade.ValorBD);
                sql.Replace("@006", this.Operacao.ValorBD);

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
        /// Atualiza RoboCanalPreco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tRoboCanalPreco SET PrecoID = @001, CanalID = @002, DataInicio = '@003', DataFim = '@004', Quantidade = @005, Operacao = '@006' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.PrecoID.ValorBD);
            sql.Replace("@002", this.CanalID.ValorBD);
            sql.Replace("@003", this.DataInicio.ValorBD);
            sql.Replace("@004", this.DataFim.ValorBD);
            sql.Replace("@005", this.Quantidade.ValorBD);
            sql.Replace("@006", this.Operacao.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui RoboCanalPreco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tRoboCanalPreco WHERE ID=" + id;

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
        /// Exclui RoboCanalPreco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tRoboCanalPreco WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui RoboCanalPreco
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

            this.PrecoID.Limpar();
            this.CanalID.Limpar();
            this.DataInicio.Limpar();
            this.DataFim.Limpar();
            this.Quantidade.Limpar();
            this.Operacao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.PrecoID.Desfazer();
            this.CanalID.Desfazer();
            this.DataInicio.Desfazer();
            this.DataFim.Desfazer();
            this.Quantidade.Desfazer();
            this.Operacao.Desfazer();
        }

        public class precoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoID";
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

        public class datainicio : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataInicio";
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

        public class datafim : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataFim";
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

        public class operacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Operacao";
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

                DataTable tabela = new DataTable("RoboCanalPreco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("DataInicio", typeof(string));
                tabela.Columns.Add("DataFim", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("Operacao", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "RoboCanalPrecoLista_B"

    public abstract class RoboCanalPrecoLista_B : BaseLista
    {

        protected RoboCanalPreco roboCanalPreco;

        // passar o Usuario logado no sistema
        public RoboCanalPrecoLista_B()
        {
            roboCanalPreco = new RoboCanalPreco();
        }

        public RoboCanalPreco RoboCanalPreco
        {
            get { return roboCanalPreco; }
        }

        /// <summary>
        /// Retorna um IBaseBD de RoboCanalPreco especifico
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
                    roboCanalPreco.Ler(id);
                    return roboCanalPreco;
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
                    sql = "SELECT ID FROM tRoboCanalPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRoboCanalPreco";

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
                    sql = "SELECT ID FROM tRoboCanalPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRoboCanalPreco";

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
        /// Preenche RoboCanalPreco corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                roboCanalPreco.Ler(id);

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

                bool ok = roboCanalPreco.Excluir();
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

                        string sqlDelete = "DELETE FROM tRoboCanalPreco WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) RoboCanalPreco na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = roboCanalPreco.Inserir();
                if (ok)
                {
                    lista.Add(roboCanalPreco.Control.ID);
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
        /// Obtem uma tabela de todos os campos de RoboCanalPreco carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("RoboCanalPreco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("DataInicio", typeof(string));
                tabela.Columns.Add("DataFim", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("Operacao", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = roboCanalPreco.Control.ID;
                        linha["PrecoID"] = roboCanalPreco.PrecoID.Valor;
                        linha["CanalID"] = roboCanalPreco.CanalID.Valor;
                        linha["DataInicio"] = roboCanalPreco.DataInicio.Valor;
                        linha["DataFim"] = roboCanalPreco.DataFim.Valor;
                        linha["Quantidade"] = roboCanalPreco.Quantidade.Valor;
                        linha["Operacao"] = roboCanalPreco.Operacao.Valor;
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

                DataTable tabela = new DataTable("RelatorioRoboCanalPreco");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("PrecoID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("DataInicio", typeof(string));
                    tabela.Columns.Add("DataFim", typeof(string));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("Operacao", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["PrecoID"] = roboCanalPreco.PrecoID.Valor;
                        linha["CanalID"] = roboCanalPreco.CanalID.Valor;
                        linha["DataInicio"] = roboCanalPreco.DataInicio.Valor;
                        linha["DataFim"] = roboCanalPreco.DataFim.Valor;
                        linha["Quantidade"] = roboCanalPreco.Quantidade.Valor;
                        linha["Operacao"] = roboCanalPreco.Operacao.Valor;
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
                    case "PrecoID":
                        sql = "SELECT ID, PrecoID FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY PrecoID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "DataInicio":
                        sql = "SELECT ID, DataInicio FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY DataInicio";
                        break;
                    case "DataFim":
                        sql = "SELECT ID, DataFim FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY DataFim";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "Operacao":
                        sql = "SELECT ID, Operacao FROM tRoboCanalPreco WHERE " + FiltroSQL + " ORDER BY Operacao";
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

    #region "RoboCanalPrecoException"

    [Serializable]
    public class RoboCanalPrecoException : Exception
    {

        public RoboCanalPrecoException() : base() { }

        public RoboCanalPrecoException(string msg) : base(msg) { }

        public RoboCanalPrecoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}