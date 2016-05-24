/******************************************************
* Arquivo LocalHorarioDB.cs
* Gerado em: 03/07/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "LocalHorario_B"

    public abstract class LocalHorario_B : BaseBD
    {

        public localid LocalID = new localid();
        public diasemana DiaSemana = new diasemana();
        public horainicio HoraInicio = new horainicio();
        public horafim HoraFim = new horafim();

        public LocalHorario_B() { }

        /// <summary>
        /// Preenche todos os atributos de LocalHorario
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tLocalHorario WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.DiaSemana.ValorBD = bd.LerInt("DiaSemana").ToString();
                    this.HoraInicio.ValorBD = bd.LerString("HoraInicio");
                    this.HoraFim.ValorBD = bd.LerString("HoraFim");
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
        /// Inserir novo(a) LocalHorario
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tLocalHorario");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLocalHorario(ID, LocalID, DiaSemana, HoraInicio, HoraFim) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.DiaSemana.ValorBD);
                sql.Replace("@003", this.HoraInicio.ValorBD);
                sql.Replace("@004", this.HoraFim.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Atualiza LocalHorario
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLocalHorario SET LocalID = @001, DiaSemana = @002, HoraInicio = '@003', HoraFim = '@004' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.DiaSemana.ValorBD);
                sql.Replace("@003", this.HoraInicio.ValorBD);
                sql.Replace("@004", this.HoraFim.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui LocalHorario com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tLocalHorario WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui LocalHorario
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
            this.DiaSemana.Limpar();
            this.HoraInicio.Limpar();
            this.HoraFim.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.LocalID.Desfazer();
            this.DiaSemana.Desfazer();
            this.HoraInicio.Desfazer();
            this.HoraFim.Desfazer();
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

        public class horainicio : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "HoraInicio";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class horafim : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "HoraFim";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
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

                DataTable tabela = new DataTable("LocalHorario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("DiaSemana", typeof(int));
                tabela.Columns.Add("HoraInicio", typeof(DateTime));
                tabela.Columns.Add("HoraFim", typeof(DateTime));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "LocalHorarioLista_B"

    public abstract class LocalHorarioLista_B : BaseLista
    {

        protected LocalHorario localHorario;

        // passar o Usuario logado no sistema
        public LocalHorarioLista_B()
        {
            localHorario = new LocalHorario();
        }

        public LocalHorario LocalHorario
        {
            get { return localHorario; }
        }

        /// <summary>
        /// Retorna um IBaseBD de LocalHorario especifico
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
                    localHorario.Ler(id);
                    return localHorario;
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
                    sql = "SELECT ID FROM tLocalHorario";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLocalHorario";

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
                    sql = "SELECT ID FROM tLocalHorario";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLocalHorario";

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
        /// Preenche LocalHorario corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                localHorario.Ler(id);

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

                bool ok = localHorario.Excluir();
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

                        string sqlDelete = "DELETE FROM tLocalHorario WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) LocalHorario na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = localHorario.Inserir();
                if (ok)
                {
                    lista.Add(localHorario.Control.ID);
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
        /// Obtem uma tabela de todos os campos de LocalHorario carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("LocalHorario");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("DiaSemana", typeof(int));
                tabela.Columns.Add("HoraInicio", typeof(DateTime));
                tabela.Columns.Add("HoraFim", typeof(DateTime));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = localHorario.Control.ID;
                        linha["LocalID"] = localHorario.LocalID.Valor;
                        linha["DiaSemana"] = localHorario.DiaSemana.Valor;
                        linha["HoraInicio"] = localHorario.HoraInicio.Valor;
                        linha["HoraFim"] = localHorario.HoraFim.Valor;
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

                DataTable tabela = new DataTable("RelatorioLocalHorario");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("DiaSemana", typeof(int));
                    tabela.Columns.Add("HoraInicio", typeof(DateTime));
                    tabela.Columns.Add("HoraFim", typeof(DateTime));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["LocalID"] = localHorario.LocalID.Valor;
                        linha["DiaSemana"] = localHorario.DiaSemana.Valor;
                        linha["HoraInicio"] = localHorario.HoraInicio.Valor;
                        linha["HoraFim"] = localHorario.HoraFim.Valor;
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
                        sql = "SELECT ID, LocalID FROM tLocalHorario WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "DiaSemana":
                        sql = "SELECT ID, DiaSemana FROM tLocalHorario WHERE " + FiltroSQL + " ORDER BY DiaSemana";
                        break;
                    case "HoraInicio":
                        sql = "SELECT ID, HoraInicio FROM tLocalHorario WHERE " + FiltroSQL + " ORDER BY HoraInicio";
                        break;
                    case "HoraFim":
                        sql = "SELECT ID, HoraFim FROM tLocalHorario WHERE " + FiltroSQL + " ORDER BY HoraFim";
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

    #region "LocalHorarioException"

    [Serializable]
    public class LocalHorarioException : Exception
    {

        public LocalHorarioException() : base() { }

        public LocalHorarioException(string msg) : base(msg) { }

        public LocalHorarioException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}