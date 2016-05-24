/******************************************************
* Arquivo EscalonamentoDB.cs
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

    #region "Escalonamento_B"

    public abstract class Escalonamento_B : BaseBD
    {

        public vendaid VendaID = new vendaid();
        public usuarioid UsuarioID = new usuarioid();
        public nrochamado NroChamado = new nrochamado();
        public timestamp TimeStamp = new timestamp();
        public motivoid MotivoID = new motivoid();
        public tipo Tipo = new tipo();

        public Escalonamento_B() { }

        /// <summary>
        /// Preenche todos os atributos de Escalonamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEscalonamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaID.ValorBD = bd.LerInt("VendaID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.NroChamado.ValorBD = bd.LerString("NroChamado");
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();
                    this.Tipo.ValorBD = bd.LerString("Tipo");
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
        /// Inserir novo(a) Escalonamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tEscalonamento(VendaID, UsuarioID, NroChamado, TimeStamp, MotivoID, Tipo) ");
                sql.Append("VALUES (@001,@002,'@003','@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.NroChamado.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.MotivoID.ValorBD);
                sql.Replace("@006", this.Tipo.ValorBD);

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
        /// Inserir novo(a) Escalonamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tEscalonamento(VendaID, UsuarioID, NroChamado, TimeStamp, MotivoID, Tipo) ");
            sql.Append("VALUES (@001,@002,'@003','@004',@005,'@006'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.VendaID.ValorBD);
            sql.Replace("@002", this.UsuarioID.ValorBD);
            sql.Replace("@003", this.NroChamado.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.MotivoID.ValorBD);
            sql.Replace("@006", this.Tipo.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza Escalonamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEscalonamento SET VendaID = @001, UsuarioID = @002, NroChamado = '@003', TimeStamp = '@004', MotivoID = @005, Tipo = '@006' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.NroChamado.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.MotivoID.ValorBD);
                sql.Replace("@006", this.Tipo.ValorBD);

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
        /// Atualiza Escalonamento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tEscalonamento SET VendaID = @001, UsuarioID = @002, NroChamado = '@003', TimeStamp = '@004', MotivoID = @005, Tipo = '@006' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.VendaID.ValorBD);
            sql.Replace("@002", this.UsuarioID.ValorBD);
            sql.Replace("@003", this.NroChamado.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.MotivoID.ValorBD);
            sql.Replace("@006", this.Tipo.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui Escalonamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tEscalonamento WHERE ID=" + id;

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
        /// Exclui Escalonamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tEscalonamento WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui Escalonamento
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

            this.VendaID.Limpar();
            this.UsuarioID.Limpar();
            this.NroChamado.Limpar();
            this.TimeStamp.Limpar();
            this.MotivoID.Limpar();
            this.Tipo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.VendaID.Desfazer();
            this.UsuarioID.Desfazer();
            this.NroChamado.Desfazer();
            this.TimeStamp.Desfazer();
            this.MotivoID.Desfazer();
            this.Tipo.Desfazer();
        }

        public class vendaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaID";
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

        public class nrochamado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroChamado";
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

        public class motivoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoID";
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

        public class tipo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Tipo";
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

                DataTable tabela = new DataTable("Escalonamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("NroChamado", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("MotivoID", typeof(int));
                tabela.Columns.Add("Tipo", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "EscalonamentoLista_B"

    public abstract class EscalonamentoLista_B : BaseLista
    {

        protected Escalonamento escalonamento;

        // passar o Usuario logado no sistema
        public EscalonamentoLista_B()
        {
            escalonamento = new Escalonamento();
        }

        public Escalonamento Escalonamento
        {
            get { return escalonamento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Escalonamento especifico
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
                    escalonamento.Ler(id);
                    return escalonamento;
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
                    sql = "SELECT ID FROM tEscalonamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEscalonamento";

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
                    sql = "SELECT ID FROM tEscalonamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEscalonamento";

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
        /// Preenche Escalonamento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                escalonamento.Ler(id);

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

                bool ok = escalonamento.Excluir();
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

                        string sqlDelete = "DELETE FROM tEscalonamento WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) Escalonamento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = escalonamento.Inserir();
                if (ok)
                {
                    lista.Add(escalonamento.Control.ID);
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
        /// Obtem uma tabela de todos os campos de Escalonamento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Escalonamento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaID", typeof(int));
                tabela.Columns.Add("UsuarioID", typeof(int));
                tabela.Columns.Add("NroChamado", typeof(string));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("MotivoID", typeof(int));
                tabela.Columns.Add("Tipo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = escalonamento.Control.ID;
                        linha["VendaID"] = escalonamento.VendaID.Valor;
                        linha["UsuarioID"] = escalonamento.UsuarioID.Valor;
                        linha["NroChamado"] = escalonamento.NroChamado.Valor;
                        linha["TimeStamp"] = escalonamento.TimeStamp.Valor;
                        linha["MotivoID"] = escalonamento.MotivoID.Valor;
                        linha["Tipo"] = escalonamento.Tipo.Valor;
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

                DataTable tabela = new DataTable("RelatorioEscalonamento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaID", typeof(int));
                    tabela.Columns.Add("UsuarioID", typeof(int));
                    tabela.Columns.Add("NroChamado", typeof(string));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("MotivoID", typeof(int));
                    tabela.Columns.Add("Tipo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaID"] = escalonamento.VendaID.Valor;
                        linha["UsuarioID"] = escalonamento.UsuarioID.Valor;
                        linha["NroChamado"] = escalonamento.NroChamado.Valor;
                        linha["TimeStamp"] = escalonamento.TimeStamp.Valor;
                        linha["MotivoID"] = escalonamento.MotivoID.Valor;
                        linha["Tipo"] = escalonamento.Tipo.Valor;
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
                    case "VendaID":
                        sql = "SELECT ID, VendaID FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY VendaID";
                        break;
                    case "UsuarioID":
                        sql = "SELECT ID, UsuarioID FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY UsuarioID";
                        break;
                    case "NroChamado":
                        sql = "SELECT ID, NroChamado FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY NroChamado";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "MotivoID":
                        sql = "SELECT ID, MotivoID FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY MotivoID";
                        break;
                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tEscalonamento WHERE " + FiltroSQL + " ORDER BY Tipo";
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

    #region "EscalonamentoException"

    [Serializable]
    public class EscalonamentoException : Exception
    {

        public EscalonamentoException() : base() { }

        public EscalonamentoException(string msg) : base(msg) { }

        public EscalonamentoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}