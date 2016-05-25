using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "RoboCanalEvento_B"

    public abstract class RoboCanalEvento_B : BaseBD
    {

        public eventoid EventoID = new eventoid();
        public canalid CanalID = new canalid();
        public isfilme IsFilme = new isfilme();
        public usuarioid UsuarioID = new usuarioid();
        public operacao Operacao = new operacao();

        public RoboCanalEvento_B() { }

        /// <summary>
        /// Preenche todos os atributos de RoboCanalEvento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tRoboCanalEvento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.IsFilme.ValorBD = bd.LerString("IsFilme");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
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
        /// Inserir novo(a) RoboCanalEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tRoboCanalEvento(EventoID, CanalID, IsFilme, UsuarioID, Operacao) ");
                sql.Append("VALUES (@001,@002,'@003',@004,'@005'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.IsFilme.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.Operacao.ValorBD);

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
        /// Inserir novo(a) RoboCanalEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tRoboCanalEvento(EventoID, CanalID, IsFilme, UsuarioID, Operacao) ");
            sql.Append("VALUES (@001,@002,'@003',@004,'@005'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.EventoID.ValorBD);
            sql.Replace("@002", this.CanalID.ValorBD);
            sql.Replace("@003", this.IsFilme.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);
            sql.Replace("@005", this.Operacao.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza RoboCanalEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tRoboCanalEvento SET EventoID = @001, CanalID = @002, IsFilme = '@003', UsuarioID = @004, Operacao = '@005' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.IsFilme.ValorBD);
                sql.Replace("@004", this.UsuarioID.ValorBD);
                sql.Replace("@005", this.Operacao.ValorBD);

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
        /// Atualiza RoboCanalEvento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tRoboCanalEvento SET EventoID = @001, CanalID = @002, IsFilme = '@003', UsuarioID = @004, Operacao = '@005' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.EventoID.ValorBD);
            sql.Replace("@002", this.CanalID.ValorBD);
            sql.Replace("@003", this.IsFilme.ValorBD);
            sql.Replace("@004", this.UsuarioID.ValorBD);
            sql.Replace("@005", this.Operacao.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui RoboCanalEvento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tRoboCanalEvento WHERE ID=" + id;

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
        /// Exclui RoboCanalEvento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tRoboCanalEvento WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui RoboCanalEvento
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

            this.EventoID.Limpar();
            this.CanalID.Limpar();
            this.IsFilme.Limpar();
            this.UsuarioID.Limpar();
            this.Operacao.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.EventoID.Desfazer();
            this.CanalID.Desfazer();
            this.IsFilme.Desfazer();
            this.UsuarioID.Desfazer();
            this.Operacao.Desfazer();
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

        public class isfilme : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "IsFilme";
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

        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
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
                    return 1;
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

                DataTable tabela = new DataTable("RoboCanalEvento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("IsFilme", typeof(bool));
                tabela.Columns.Add("UsuarioID", typeof(int));
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

    #region "RoboCanalEventoLista_B"

    public abstract class RoboCanalEventoLista_B : BaseLista
    {

        protected RoboCanalEvento roboCanalEvento;

        // passar o Usuario logado no sistema
        public RoboCanalEventoLista_B()
        {
            roboCanalEvento = new RoboCanalEvento();
        }

        public RoboCanalEvento RoboCanalEvento
        {
            get { return roboCanalEvento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de RoboCanalEvento especifico
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
                    roboCanalEvento.Ler(id);
                    return roboCanalEvento;
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
                    sql = "SELECT ID FROM tRoboCanalEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRoboCanalEvento";

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
                    sql = "SELECT ID FROM tRoboCanalEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tRoboCanalEvento";

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
        /// Preenche RoboCanalEvento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                roboCanalEvento.Ler(id);

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

                bool ok = roboCanalEvento.Excluir();
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

                        string sqlDelete = "DELETE FROM tRoboCanalEvento WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) RoboCanalEvento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = roboCanalEvento.Inserir();
                if (ok)
                {
                    lista.Add(roboCanalEvento.Control.ID);
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
        /// Obtem uma tabela de todos os campos de RoboCanalEvento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("RoboCanalEvento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EventoID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("IsFilme", typeof(bool));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("Operacao", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = roboCanalEvento.Control.ID;
                        linha["EventoID"] = roboCanalEvento.EventoID.Valor;
                        linha["CanalID"] = roboCanalEvento.CanalID.Valor;
                        linha["IsFilme"] = roboCanalEvento.IsFilme.Valor;
                        linha["UsuarioID"] = roboCanalEvento.UsuarioID.Valor;
                        linha["Operacao"] = roboCanalEvento.Operacao.Valor;
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

                DataTable tabela = new DataTable("RelatorioRoboCanalEvento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EventoID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("IsFilme", typeof(bool));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("Operacao", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EventoID"] = roboCanalEvento.EventoID.Valor;
                        linha["CanalID"] = roboCanalEvento.CanalID.Valor;
                        linha["IsFilme"] = roboCanalEvento.IsFilme.Valor;
                        linha["UsuarioID"] = roboCanalEvento.UsuarioID.Valor;
                        linha["Operacao"] = roboCanalEvento.Operacao.Valor;
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
                    case "EventoID":
                        sql = "SELECT ID, EventoID FROM tRoboCanalEvento WHERE " + FiltroSQL + " ORDER BY EventoID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tRoboCanalEvento WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "IsFilme":
                        sql = "SELECT ID, IsFilme FROM tRoboCanalEvento WHERE " + FiltroSQL + " ORDER BY IsFilme";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tRoboCanalEvento WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "Operacao":
                        sql = "SELECT ID, Operacao FROM tRoboCanalEvento WHERE " + FiltroSQL + " ORDER BY Operacao";
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

    #region "RoboCanalEventoException"

    [Serializable]
    public class RoboCanalEventoException : Exception
    {

        public RoboCanalEventoException() : base() { }

        public RoboCanalEventoException(string msg) : base(msg) { }

        public RoboCanalEventoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}