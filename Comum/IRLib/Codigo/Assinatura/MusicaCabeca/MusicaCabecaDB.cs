/******************************************************
* Arquivo MusicaCabecaDB.cs
* Gerado em: 13/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "MusicaCabeca_B"

    public abstract class MusicaCabeca_B : BaseBD
    {

        public nome Nome = new nome();
        public local Local = new local();
        public data Data = new data();
        public quantidadecota QuantidadeCota = new quantidadecota();
        public quantidadenormal QuantidadeNormal = new quantidadenormal();
        public datalimitecota DataLimiteCota = new datalimitecota();
        public detalhes Detalhes = new detalhes();

        public MusicaCabeca_B() { }

        /// <summary>
        /// Preenche todos os atributos de MusicaCabeca
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tMusicaCabeca WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Local.ValorBD = bd.LerString("Local");
                    this.Data.ValorBD = bd.LerString("Data");
                    this.QuantidadeCota.ValorBD = bd.LerInt("QuantidadeCota").ToString();
                    this.QuantidadeNormal.ValorBD = bd.LerInt("QuantidadeNormal").ToString();
                    this.DataLimiteCota.ValorBD = bd.LerString("DataLimiteCota");
                    this.Detalhes.ValorBD = bd.LerString("Detalhes");
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
        /// Inserir novo(a) MusicaCabeca
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tMusicaCabeca(Nome, Local, Data, QuantidadeCota, QuantidadeNormal, DataLimiteCota, Detalhes) ");
                sql.Append("VALUES ('@001','@002','@003',@004,@005,'@006','@007'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Local.ValorBD);
                sql.Replace("@003", this.Data.ValorBD);
                sql.Replace("@004", this.QuantidadeCota.ValorBD);
                sql.Replace("@005", this.QuantidadeNormal.ValorBD);
                sql.Replace("@006", this.DataLimiteCota.ValorBD);
                sql.Replace("@007", this.Detalhes.ValorBD);

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
        /// Inserir novo(a) MusicaCabeca
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tMusicaCabeca(Nome, Local, Data, QuantidadeCota, QuantidadeNormal, DataLimiteCota, Detalhes) ");
            sql.Append("VALUES ('@001','@002','@003',@004,@005,'@006','@007'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.Local.ValorBD);
            sql.Replace("@003", this.Data.ValorBD);
            sql.Replace("@004", this.QuantidadeCota.ValorBD);
            sql.Replace("@005", this.QuantidadeNormal.ValorBD);
            sql.Replace("@006", this.DataLimiteCota.ValorBD);
            sql.Replace("@007", this.Detalhes.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza MusicaCabeca
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMusicaCabeca SET Nome = '@001', Local = '@002', Data = '@003', QuantidadeCota = @004, QuantidadeNormal = @005, DataLimiteCota = '@006', Detalhes = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.Local.ValorBD);
                sql.Replace("@003", this.Data.ValorBD);
                sql.Replace("@004", this.QuantidadeCota.ValorBD);
                sql.Replace("@005", this.QuantidadeNormal.ValorBD);
                sql.Replace("@006", this.DataLimiteCota.ValorBD);
                sql.Replace("@007", this.Detalhes.ValorBD);

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
        /// Atualiza MusicaCabeca
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tMusicaCabeca SET Nome = '@001', Local = '@002', Data = '@003', QuantidadeCota = @004, QuantidadeNormal = @005, DataLimiteCota = '@006', Detalhes = '@007' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.Local.ValorBD);
            sql.Replace("@003", this.Data.ValorBD);
            sql.Replace("@004", this.QuantidadeCota.ValorBD);
            sql.Replace("@005", this.QuantidadeNormal.ValorBD);
            sql.Replace("@006", this.DataLimiteCota.ValorBD);
            sql.Replace("@007", this.Detalhes.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui MusicaCabeca com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tMusicaCabeca WHERE ID=" + id;

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
        /// Exclui MusicaCabeca com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tMusicaCabeca WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui MusicaCabeca
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

            this.Nome.Limpar();
            this.Local.Limpar();
            this.Data.Limpar();
            this.QuantidadeCota.Limpar();
            this.QuantidadeNormal.Limpar();
            this.DataLimiteCota.Limpar();
            this.Detalhes.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Nome.Desfazer();
            this.Local.Desfazer();
            this.Data.Desfazer();
            this.QuantidadeCota.Desfazer();
            this.QuantidadeNormal.Desfazer();
            this.DataLimiteCota.Desfazer();
            this.Detalhes.Desfazer();
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

        public class local : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Local";
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

        public class data : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "Data";
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

        public class quantidadecota : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeCota";
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

        public class quantidadenormal : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeNormal";
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

        public class datalimitecota : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataLimiteCota";
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

        public class detalhes : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Detalhes";
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

                DataTable tabela = new DataTable("MusicaCabeca");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Data", typeof(DateTime));
                tabela.Columns.Add("QuantidadeCota", typeof(int));
                tabela.Columns.Add("QuantidadeNormal", typeof(int));
                tabela.Columns.Add("DataLimiteCota", typeof(DateTime));
                tabela.Columns.Add("Detalhes", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "MusicaCabecaLista_B"

    public abstract class MusicaCabecaLista_B : BaseLista
    {

        protected MusicaCabeca musicaCabeca;

        // passar o Usuario logado no sistema
        public MusicaCabecaLista_B()
        {
            musicaCabeca = new MusicaCabeca();
        }

        public MusicaCabeca MusicaCabeca
        {
            get { return musicaCabeca; }
        }

        /// <summary>
        /// Retorna um IBaseBD de MusicaCabeca especifico
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
                    musicaCabeca.Ler(id);
                    return musicaCabeca;
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
                    sql = "SELECT ID FROM tMusicaCabeca";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabeca";

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
                    sql = "SELECT ID FROM tMusicaCabeca";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tMusicaCabeca";

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
        /// Preenche MusicaCabeca corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                musicaCabeca.Ler(id);

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

                bool ok = musicaCabeca.Excluir();
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

                        string sqlDelete = "DELETE FROM tMusicaCabeca WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) MusicaCabeca na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = musicaCabeca.Inserir();
                if (ok)
                {
                    lista.Add(musicaCabeca.Control.ID);
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
        /// Obtem uma tabela de todos os campos de MusicaCabeca carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("MusicaCabeca");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Local", typeof(string));
                tabela.Columns.Add("Data", typeof(DateTime));
                tabela.Columns.Add("QuantidadeCota", typeof(int));
                tabela.Columns.Add("QuantidadeNormal", typeof(int));
                tabela.Columns.Add("DataLimiteCota", typeof(DateTime));
                tabela.Columns.Add("Detalhes", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = musicaCabeca.Control.ID;
                        linha["Nome"] = musicaCabeca.Nome.Valor;
                        linha["Local"] = musicaCabeca.Local.Valor;
                        linha["Data"] = musicaCabeca.Data.Valor;
                        linha["QuantidadeCota"] = musicaCabeca.QuantidadeCota.Valor;
                        linha["QuantidadeNormal"] = musicaCabeca.QuantidadeNormal.Valor;
                        linha["DataLimiteCota"] = musicaCabeca.DataLimiteCota.Valor;
                        linha["Detalhes"] = musicaCabeca.Detalhes.Valor;
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

                DataTable tabela = new DataTable("RelatorioMusicaCabeca");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Local", typeof(string));
                    tabela.Columns.Add("Data", typeof(DateTime));
                    tabela.Columns.Add("QuantidadeCota", typeof(int));
                    tabela.Columns.Add("QuantidadeNormal", typeof(int));
                    tabela.Columns.Add("DataLimiteCota", typeof(DateTime));
                    tabela.Columns.Add("Detalhes", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = musicaCabeca.Nome.Valor;
                        linha["Local"] = musicaCabeca.Local.Valor;
                        linha["Data"] = musicaCabeca.Data.Valor;
                        linha["QuantidadeCota"] = musicaCabeca.QuantidadeCota.Valor;
                        linha["QuantidadeNormal"] = musicaCabeca.QuantidadeNormal.Valor;
                        linha["DataLimiteCota"] = musicaCabeca.DataLimiteCota.Valor;
                        linha["Detalhes"] = musicaCabeca.Detalhes.Valor;
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
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Local":
                        sql = "SELECT ID, Local FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY Local";
                        break;
                    case "Data":
                        sql = "SELECT ID, Data FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY Data";
                        break;
                    case "QuantidadeCota":
                        sql = "SELECT ID, QuantidadeCota FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY QuantidadeCota";
                        break;
                    case "QuantidadeNormal":
                        sql = "SELECT ID, QuantidadeNormal FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY QuantidadeNormal";
                        break;
                    case "DataLimiteCota":
                        sql = "SELECT ID, DataLimiteCota FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY DataLimiteCota";
                        break;
                    case "Detalhes":
                        sql = "SELECT ID, Detalhes FROM tMusicaCabeca WHERE " + FiltroSQL + " ORDER BY Detalhes";
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

    #region "MusicaCabecaException"

    [Serializable]
    public class MusicaCabecaException : Exception
    {

        public MusicaCabecaException() : base() { }

        public MusicaCabecaException(string msg) : base(msg) { }

        public MusicaCabecaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}