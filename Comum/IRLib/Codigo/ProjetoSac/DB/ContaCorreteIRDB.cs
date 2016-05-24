/******************************************************
* Arquivo ContaCorrenteIRDB.cs
* Gerado em: 29/10/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;

using CTLib;

namespace IRLib
{

    #region "ContaCorrenteIR_B"

    public abstract class ContaCorrenteIR_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public timestamp TimeStamp = new timestamp();
        public valor Valor = new valor();
        public descricao Descricao = new descricao();
        public usuarioid UsuarioID = new usuarioid();
        public cancelamentoid CancelamentoID = new cancelamentoid();

        public ContaCorrenteIR_B() { }

        /// <summary>
        /// Preenche todos os atributos de ContaCorrenteIR
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tContaCorrenteIR WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.CancelamentoID.ValorBD = bd.LerInt("CancelamentoID").ToString();
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
        /// Inserir novo(a) ContaCorrenteIR
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tContaCorrenteIR(ClienteID, TimeStamp, Valor, Descricao, UsuarioID, CancelamentoID) ");
                sql.Append("VALUES (@001,'@002','@003','@004',@005,@006); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.TimeStamp.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Descricao.ValorBD);
                sql.Replace("@005", this.UsuarioID.ValorBD);
                sql.Replace("@006", this.CancelamentoID.ValorBD);

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
        /// Inserir novo(a) ContaCorrenteIR
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tContaCorrenteIR(ClienteID, TimeStamp, Valor, Descricao, UsuarioID, CancelamentoID) ");
            sql.Append("VALUES (@001,'@002','@003','@004',@005,@006); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.TimeStamp.ValorBD);
            sql.Replace("@003", this.Valor.ValorBD);
            sql.Replace("@004", this.Descricao.ValorBD);
            sql.Replace("@005", this.UsuarioID.ValorBD);
            sql.Replace("@006", this.CancelamentoID.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza ContaCorrenteIR
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContaCorrenteIR SET ClienteID = @001, TimeStamp = '@002', Valor = '@003', Descricao = '@004', UsuarioID = @005, CancelamentoID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.TimeStamp.ValorBD);
                sql.Replace("@003", this.Valor.ValorBD);
                sql.Replace("@004", this.Descricao.ValorBD);
                sql.Replace("@005", this.UsuarioID.ValorBD);
                sql.Replace("@006", this.CancelamentoID.ValorBD);

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
        /// Atualiza ContaCorrenteIR
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tContaCorrenteIR SET ClienteID = @001, TimeStamp = '@002', Valor = '@003', Descricao = '@004', UsuarioID = @005, CancelamentoID = @006 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.ClienteID.ValorBD);
            sql.Replace("@002", this.TimeStamp.ValorBD);
            sql.Replace("@003", this.Valor.ValorBD);
            sql.Replace("@004", this.Descricao.ValorBD);
            sql.Replace("@005", this.UsuarioID.ValorBD);
            sql.Replace("@006", this.CancelamentoID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui ContaCorrenteIR com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tContaCorrenteIR WHERE ID=" + id;

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
        /// Exclui ContaCorrenteIR com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tContaCorrenteIR WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui ContaCorrenteIR
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

            this.ClienteID.Limpar();
            this.TimeStamp.Limpar();
            this.Valor.Limpar();
            this.Descricao.Limpar();
            this.UsuarioID.Limpar();
            this.CancelamentoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.TimeStamp.Desfazer();
            this.Valor.Desfazer();
            this.Descricao.Desfazer();
            this.UsuarioID.Desfazer();
            this.CancelamentoID.Desfazer();
        }

        public class clienteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteID";
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

        public class timestamp : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "TimeStamp";
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

        public class valor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Valor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 12;
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

        public class descricao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Descricao";
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

        public class cancelamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CancelamentoID";
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

                DataTable tabela = new DataTable("ContaCorrenteIR");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CancelamentoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "ContaCorrenteIRLista_B"

    public abstract class ContaCorrenteIRLista_B : BaseLista
    {

        protected ContaCorrenteIR contaCorrenteIR;

        // passar o Usuario logado no sistema
        public ContaCorrenteIRLista_B()
        {
            contaCorrenteIR = new ContaCorrenteIR();
        }

        public ContaCorrenteIR ContaCorrenteIR
        {
            get { return contaCorrenteIR; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ContaCorrenteIR especifico
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
                    contaCorrenteIR.Ler(id);
                    return contaCorrenteIR;
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
                    sql = "SELECT ID FROM tContaCorrenteIR";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContaCorrenteIR";

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
                    sql = "SELECT ID FROM tContaCorrenteIR";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tContaCorrenteIR";

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
        /// Preenche ContaCorrenteIR corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                contaCorrenteIR.Ler(id);

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

                bool ok = contaCorrenteIR.Excluir();
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

                        string sqlDelete = "DELETE FROM tContaCorrenteIR WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) ContaCorrenteIR na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = contaCorrenteIR.Inserir();
                if (ok)
                {
                    lista.Add(contaCorrenteIR.Control.ID);
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
        /// Obtem uma tabela de todos os campos de ContaCorrenteIR carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ContaCorrenteIR");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("CancelamentoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = contaCorrenteIR.Control.ID;
                        linha["ClienteID"] = contaCorrenteIR.ClienteID.Valor;
                        linha["TimeStamp"] = contaCorrenteIR.TimeStamp.Valor;
                        linha["Valor"] = contaCorrenteIR.Valor.Valor;
                        linha["Descricao"] = contaCorrenteIR.Descricao.Valor;
                        linha["UsuarioID"] = contaCorrenteIR.UsuarioID.Valor;
                        linha["CancelamentoID"] = contaCorrenteIR.CancelamentoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioContaCorrenteIR");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("Descricao", typeof(string));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("CancelamentoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = contaCorrenteIR.ClienteID.Valor;
                        linha["TimeStamp"] = contaCorrenteIR.TimeStamp.Valor;
                        linha["Valor"] = contaCorrenteIR.Valor.Valor;
                        linha["Descricao"] = contaCorrenteIR.Descricao.Valor;
                        linha["UsuarioID"] = contaCorrenteIR.UsuarioID.Valor;
                        linha["CancelamentoID"] = contaCorrenteIR.CancelamentoID.Valor;
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
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "Descricao":
                        sql = "SELECT ID, Descricao FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY Descricao";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "CancelamentoID":
                        sql = "SELECT ID, CancelamentoID FROM tContaCorrenteIR WHERE " + FiltroSQL + " ORDER BY CancelamentoID";
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

    #region "ContaCorrenteIRException"

    [Serializable]
    public class ContaCorrenteIRException : Exception
    {

        public ContaCorrenteIRException() : base() { }

        public ContaCorrenteIRException(string msg) : base(msg) { }

        public ContaCorrenteIRException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}