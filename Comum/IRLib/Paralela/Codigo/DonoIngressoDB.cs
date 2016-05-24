/******************************************************
* Arquivo DonoIngressoDB.cs
* Gerado em: 05/07/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "DonoIngresso_B"

    public abstract class DonoIngresso_B : BaseBD
    {

        public nome Nome = new nome();
        public rg RG = new rg();
        public cpf CPF = new cpf();
        public email Email = new email();
        public telefone Telefone = new telefone();
        public datanascimento DataNascimento = new datanascimento();
        public nomeresponsavel NomeResponsavel = new nomeresponsavel();
        public cpfresponsavel CPFResponsavel = new cpfresponsavel();

        public DonoIngresso_B() { }

        /// <summary>
        /// Preenche todos os atributos de DonoIngresso
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tDonoIngresso WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.NomeResponsavel.ValorBD = bd.LerString("NomeResponsavel");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
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
        /// Inserir novo(a) DonoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                var sql = new StringBuilder();
                sql.Append("INSERT INTO tDonoIngresso(Nome, RG, CPF, Email, Telefone, DataNascimento, NomeResponsavel, CPFResponsavel) ");
                sql.Append("VALUES ('@001','@002','@003','@004','@005','@006','@007','@008'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Email.ValorBD);
                sql.Replace("@005", this.Telefone.ValorBD);
                sql.Replace("@006", this.DataNascimento.ValorBD);
                sql.Replace("@007", this.NomeResponsavel.ValorBD);
                sql.Replace("@008", this.CPFResponsavel.ValorBD);

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
        /// Inserir novo(a) DonoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            var sql = new StringBuilder();
            sql.Append("INSERT INTO tDonoIngresso(Nome, RG, CPF, Email, Telefone, DataNascimento, NomeResponsavel, CPFResponsavel) ");
            sql.Append("VALUES ('@001','@002','@003','@004','@005','@006','@007','@008'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.RG.ValorBD);
            sql.Replace("@003", this.CPF.ValorBD);
            sql.Replace("@004", this.Email.ValorBD);
            sql.Replace("@005", this.Telefone.ValorBD);
            sql.Replace("@006", this.DataNascimento.ValorBD);
            sql.Replace("@007", this.NomeResponsavel.ValorBD);
            sql.Replace("@008", this.CPFResponsavel.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza DonoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tDonoIngresso SET Nome = '@001', RG = '@002', CPF = '@003', Email = '@004', Telefone = '@005', DataNascimento = '@006', NomeResponsavel = '@007', CPFResponsavel = '@008' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Email.ValorBD);
                sql.Replace("@005", this.Telefone.ValorBD);
                sql.Replace("@006", this.DataNascimento.ValorBD);
                sql.Replace("@007", this.NomeResponsavel.ValorBD);
                sql.Replace("@008", this.CPFResponsavel.ValorBD);

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
        /// Atualiza DonoIngresso
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tDonoIngresso SET Nome = '@001', RG = '@002', CPF = '@003', Email = '@004', Telefone = '@005', DataNascimento = '@006', NomeResponsavel = '@007', CPFResponsavel = '@008' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.Nome.ValorBD);
            sql.Replace("@002", this.RG.ValorBD);
            sql.Replace("@003", this.CPF.ValorBD);
            sql.Replace("@004", this.Email.ValorBD);
            sql.Replace("@005", this.Telefone.ValorBD);
            sql.Replace("@006", this.DataNascimento.ValorBD);
            sql.Replace("@007", this.NomeResponsavel.ValorBD);
            sql.Replace("@008", this.CPFResponsavel.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui DonoIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tDonoIngresso WHERE ID=" + id;

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
        /// Exclui DonoIngresso com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tDonoIngresso WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui DonoIngresso
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
            this.RG.Limpar();
            this.CPF.Limpar();
            this.Email.Limpar();
            this.Telefone.Limpar();
            this.DataNascimento.Limpar();
            this.NomeResponsavel.Limpar();
            this.CPFResponsavel.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Nome.Desfazer();
            this.RG.Desfazer();
            this.CPF.Desfazer();
            this.Email.Desfazer();
            this.Telefone.Desfazer();
            this.DataNascimento.Desfazer();
            this.NomeResponsavel.Desfazer();
            this.CPFResponsavel.Desfazer();
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

        public class rg : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "RG";
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

        public class email : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Email";
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

        public class telefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Telefone";
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

        public class datanascimento : DateProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataNascimento";
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
                return base.Valor.ToString("dd/MM/yyyy");
            }

        }

        public class nomeresponsavel : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeResponsavel";
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

        public class cpfresponsavel : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFResponsavel";
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

                DataTable tabela = new DataTable("DonoIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(DateTime));
                tabela.Columns.Add("NomeResponsavel", typeof(string));
                tabela.Columns.Add("CPFResponsavel", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "DonoIngressoLista_B"

    public abstract class DonoIngressoLista_B : BaseLista
    {

        protected DonoIngresso donoIngresso;

        // passar o Usuario logado no sistema
        public DonoIngressoLista_B()
        {
            donoIngresso = new DonoIngresso();
        }

        public DonoIngresso DonoIngresso
        {
            get { return donoIngresso; }
        }

        /// <summary>
        /// Retorna um IBaseBD de DonoIngresso especifico
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
                    donoIngresso.Ler(id);
                    return donoIngresso;
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
                    sql = "SELECT ID FROM tDonoIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tDonoIngresso";

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
                    sql = "SELECT ID FROM tDonoIngresso";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tDonoIngresso";

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
        /// Preenche DonoIngresso corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                donoIngresso.Ler(id);

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

                bool ok = donoIngresso.Excluir();
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

                        string sqlDelete = "DELETE FROM tDonoIngresso WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) DonoIngresso na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = donoIngresso.Inserir();
                if (ok)
                {
                    lista.Add(donoIngresso.Control.ID);
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
        /// Obtem uma tabela de todos os campos de DonoIngresso carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("DonoIngresso");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(DateTime));
                tabela.Columns.Add("NomeResponsavel", typeof(string));
                tabela.Columns.Add("CPFResponsavel", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = donoIngresso.Control.ID;
                        linha["Nome"] = donoIngresso.Nome.Valor;
                        linha["RG"] = donoIngresso.RG.Valor;
                        linha["CPF"] = donoIngresso.CPF.Valor;
                        linha["Email"] = donoIngresso.Email.Valor;
                        linha["Telefone"] = donoIngresso.Telefone.Valor;
                        linha["DataNascimento"] = donoIngresso.DataNascimento.Valor;
                        linha["NomeResponsavel"] = donoIngresso.NomeResponsavel.Valor;
                        linha["CPFResponsavel"] = donoIngresso.CPFResponsavel.Valor;
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

                DataTable tabela = new DataTable("RelatorioDonoIngresso");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("RG", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("DataNascimento", typeof(DateTime));
                    tabela.Columns.Add("NomeResponsavel", typeof(string));
                    tabela.Columns.Add("CPFResponsavel", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = donoIngresso.Nome.Valor;
                        linha["RG"] = donoIngresso.RG.Valor;
                        linha["CPF"] = donoIngresso.CPF.Valor;
                        linha["Email"] = donoIngresso.Email.Valor;
                        linha["Telefone"] = donoIngresso.Telefone.Valor;
                        linha["DataNascimento"] = donoIngresso.DataNascimento.Valor;
                        linha["NomeResponsavel"] = donoIngresso.NomeResponsavel.Valor;
                        linha["CPFResponsavel"] = donoIngresso.CPFResponsavel.Valor;
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
                        sql = "SELECT ID, Nome FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "RG":
                        sql = "SELECT ID, RG FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY RG";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "DataNascimento":
                        sql = "SELECT ID, DataNascimento FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY DataNascimento";
                        break;
                    case "NomeResponsavel":
                        sql = "SELECT ID, NomeResponsavel FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY NomeResponsavel";
                        break;
                    case "CPFResponsavel":
                        sql = "SELECT ID, CPFResponsavel FROM tDonoIngresso WHERE " + FiltroSQL + " ORDER BY CPFResponsavel";
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

    #region "DonoIngressoException"

    [Serializable]
    public class DonoIngressoException : Exception
    {

        public DonoIngressoException() : base() { }

        public DonoIngressoException(string msg) : base(msg) { }

        public DonoIngressoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}