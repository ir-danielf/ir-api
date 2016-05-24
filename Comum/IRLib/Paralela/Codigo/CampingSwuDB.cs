/******************************************************
* Arquivo CampingSwuDB.cs
* Gerado em: 31/08/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "CampingSwu_B"

    public abstract class CampingSwu_B : BaseBD
    {

        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public ingressoid IngressoID = new ingressoid();
        public nome Nome = new nome();
        public cpf CPF = new cpf();
        public pais Pais = new pais();
        public email Email = new email();
        public sexo Sexo = new sexo();

        public CampingSwu_B() { }

        /// <summary>
        /// Preenche todos os atributos de CampingSwu
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCampingSwu WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.IngressoID.ValorBD = bd.LerInt("IngressoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.Pais.ValorBD = bd.LerString("Pais");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Sexo.ValorBD = bd.LerString("Sexo");
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
        /// Inserir novo(a) CampingSwu
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tCampingSwu");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCampingSwu(ID, VendaBilheteriaID, IngressoID, Nome, CPF, Pais, Email, Sexo) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.IngressoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.CPF.ValorBD);
                sql.Replace("@005", this.Pais.ValorBD);
                sql.Replace("@006", this.Email.ValorBD);
                sql.Replace("@007", this.Sexo.ValorBD);

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
        /// Atualiza CampingSwu
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCampingSwu SET VendaBilheteriaID = @001, IngressoID = @002, Nome = '@003', CPF = '@004', Pais = '@005', Email = '@006', Sexo = '@007' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.IngressoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.CPF.ValorBD);
                sql.Replace("@005", this.Pais.ValorBD);
                sql.Replace("@006", this.Email.ValorBD);
                sql.Replace("@007", this.Sexo.ValorBD);

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
        /// Exclui CampingSwu com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tCampingSwu WHERE ID=" + id;

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
        /// Exclui CampingSwu
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

            this.VendaBilheteriaID.Limpar();
            this.IngressoID.Limpar();
            this.Nome.Limpar();
            this.CPF.Limpar();
            this.Pais.Limpar();
            this.Email.Limpar();
            this.Sexo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.VendaBilheteriaID.Desfazer();
            this.IngressoID.Desfazer();
            this.Nome.Desfazer();
            this.CPF.Desfazer();
            this.Pais.Desfazer();
            this.Email.Desfazer();
            this.Sexo.Desfazer();
        }

        public class vendabilheteriaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaID";
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
                    return 100;
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

        public class pais : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Pais";
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
                    return 100;
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

        public class sexo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sexo";
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

                DataTable tabela = new DataTable("CampingSwu");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "CampingSwuLista_B"

    public abstract class CampingSwuLista_B : BaseLista
    {

        protected CampingSwu campingSwu;

        // passar o Usuario logado no sistema
        public CampingSwuLista_B()
        {
            campingSwu = new CampingSwu();
        }

        public CampingSwu CampingSwu
        {
            get { return campingSwu; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CampingSwu especifico
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
                    campingSwu.Ler(id);
                    return campingSwu;
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
                    sql = "SELECT ID FROM tCampingSwu";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCampingSwu";

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
                    sql = "SELECT ID FROM tCampingSwu";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCampingSwu";

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
        /// Preenche CampingSwu corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                campingSwu.Ler(id);

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

                bool ok = campingSwu.Excluir();
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

                        string sqlDelete = "DELETE FROM tCampingSwu WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) CampingSwu na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = campingSwu.Inserir();
                if (ok)
                {
                    lista.Add(campingSwu.Control.ID);
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
        /// Obtem uma tabela de todos os campos de CampingSwu carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CampingSwu");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("IngressoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = campingSwu.Control.ID;
                        linha["VendaBilheteriaID"] = campingSwu.VendaBilheteriaID.Valor;
                        linha["IngressoID"] = campingSwu.IngressoID.Valor;
                        linha["Nome"] = campingSwu.Nome.Valor;
                        linha["CPF"] = campingSwu.CPF.Valor;
                        linha["Pais"] = campingSwu.Pais.Valor;
                        linha["Email"] = campingSwu.Email.Valor;
                        linha["Sexo"] = campingSwu.Sexo.Valor;
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

                DataTable tabela = new DataTable("RelatorioCampingSwu");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("IngressoID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));
                    tabela.Columns.Add("Pais", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("Sexo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaBilheteriaID"] = campingSwu.VendaBilheteriaID.Valor;
                        linha["IngressoID"] = campingSwu.IngressoID.Valor;
                        linha["Nome"] = campingSwu.Nome.Valor;
                        linha["CPF"] = campingSwu.CPF.Valor;
                        linha["Pais"] = campingSwu.Pais.Valor;
                        linha["Email"] = campingSwu.Email.Valor;
                        linha["Sexo"] = campingSwu.Sexo.Valor;
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
                    case "VendaBilheteriaID":
                        sql = "SELECT ID, VendaBilheteriaID FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "IngressoID":
                        sql = "SELECT ID, IngressoID FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY IngressoID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "Pais":
                        sql = "SELECT ID, Pais FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY Pais";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "Sexo":
                        sql = "SELECT ID, Sexo FROM tCampingSwu WHERE " + FiltroSQL + " ORDER BY Sexo";
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

    #region "CampingSwuException"

    [Serializable]
    public class CampingSwuException : Exception
    {

        public CampingSwuException() : base() { }

        public CampingSwuException(string msg) : base(msg) { }

        public CampingSwuException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}