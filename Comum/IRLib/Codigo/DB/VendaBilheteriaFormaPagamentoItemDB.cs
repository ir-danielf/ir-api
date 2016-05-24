/******************************************************
* Arquivo VendaBilheteriaFormaPagamentoItemDB.cs
* Gerado em: 10/09/2008
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "VendaBilheteriaFormaPagamentoItem_B"

    public abstract class VendaBilheteriaFormaPagamentoItem_B : BaseBD
    {

        public vendabilheteriaformapagamentoid VendaBilheteriaFormaPagamentoID = new vendabilheteriaformapagamentoid();
        public parcela Parcela = new parcela();
        public datadeposito DataDeposito = new datadeposito();
        public valorbruto ValorBruto = new valorbruto();
        public empresaid EmpresaID = new empresaid();

        public VendaBilheteriaFormaPagamentoItem_B() { }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteriaFormaPagamento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaFormaPagamentoID.ValorBD = bd.LerInt("VendaBilheteriaFormaPagamentoID").ToString();
                    this.Parcela.ValorBD = bd.LerInt("Parcela").ToString();
                    this.DataDeposito.ValorBD = bd.LerString("DataDeposito");
                    this.ValorBruto.ValorBD = bd.LerDecimal("ValorBruto").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
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
        /// Inserir novo(a) VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM tVendaBilheteriaFormaPagamento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamento(ID, VendaBilheteriaFormaPagamentoID, Parcela, DataDeposito, ValorBruto, EmpresaID) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004',@005)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
                sql.Replace("@002", this.Parcela.ValorBD);
                sql.Replace("@003", this.DataDeposito.ValorBD);
                sql.Replace("@004", this.ValorBruto.ValorBD);
                sql.Replace("@005", this.EmpresaID.ValorBD);

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
        /// Atualiza VendaBilheteriaFormaPagamento
        /// </summary>
        /// <returns></returns>	
        /// 
        [Obsolete("Do not use! ", true)]
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamento SET VendaBilheteriaFormaPagamentoID = @001, Parcela = @002, DataDeposito = '@003', ValorBruto = '@004', EmpresaID = @005 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
                sql.Replace("@002", this.Parcela.ValorBD);
                sql.Replace("@003", this.DataDeposito.ValorBD);
                sql.Replace("@004", this.ValorBruto.ValorBD);
                sql.Replace("@005", this.EmpresaID.ValorBD);

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
        /// Exclui VendaBilheteriaFormaPagamento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamento WHERE ID=" + id;

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
        /// Exclui VendaBilheteriaFormaPagamento
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

            this.VendaBilheteriaFormaPagamentoID.Limpar();
            this.Parcela.Limpar();
            this.DataDeposito.Limpar();
            this.ValorBruto.Limpar();
            this.EmpresaID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.VendaBilheteriaFormaPagamentoID.Desfazer();
            this.Parcela.Desfazer();
            this.DataDeposito.Desfazer();
            this.ValorBruto.Desfazer();
            this.EmpresaID.Desfazer();
        }

        public class vendabilheteriaformapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaFormaPagamentoID";
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

        public class parcela : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Parcela";
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

        public class datadeposito : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataDeposito";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class valorbruto : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorBruto";
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

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
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

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                tabela.Columns.Add("Parcela", typeof(int));
                tabela.Columns.Add("DataDeposito", typeof(DateTime));
                tabela.Columns.Add("ValorBruto", typeof(decimal));
                tabela.Columns.Add("EmpresaID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "VendaBilheteriaFormaPagamentoItemLista_B"

    public abstract class VendaBilheteriaFormaPagamentoItemLista_B : BaseLista
    {

        protected VendaBilheteriaFormaPagamentoItem vendaBilheteriaFormaPagamentoItem;

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoItemLista_B()
        {
            vendaBilheteriaFormaPagamentoItem = new VendaBilheteriaFormaPagamentoItem();
        }

        public VendaBilheteriaFormaPagamentoItem VendaBilheteriaFormaPagamentoItem
        {
            get { return vendaBilheteriaFormaPagamentoItem; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteriaFormaPagamentoItem especifico
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
                    vendaBilheteriaFormaPagamentoItem.Ler(id);
                    return vendaBilheteriaFormaPagamentoItem;
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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamento";

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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamento";

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
        /// Preenche VendaBilheteriaFormaPagamentoItem corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                vendaBilheteriaFormaPagamentoItem.Ler(id);

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

                bool ok = vendaBilheteriaFormaPagamentoItem.Excluir();
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

                        string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamento WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) VendaBilheteriaFormaPagamentoItem na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteriaFormaPagamentoItem.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteriaFormaPagamentoItem.Control.ID);
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
        /// Obtem uma tabela de todos os campos de VendaBilheteriaFormaPagamentoItem carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                tabela.Columns.Add("Parcela", typeof(int));
                tabela.Columns.Add("DataDeposito", typeof(DateTime));
                tabela.Columns.Add("ValorBruto", typeof(decimal));
                tabela.Columns.Add("EmpresaID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteriaFormaPagamentoItem.Control.ID;
                        linha["VendaBilheteriaFormaPagamentoID"] = vendaBilheteriaFormaPagamentoItem.VendaBilheteriaFormaPagamentoID.Valor;
                        linha["Parcela"] = vendaBilheteriaFormaPagamentoItem.Parcela.Valor;
                        linha["DataDeposito"] = vendaBilheteriaFormaPagamentoItem.DataDeposito.Valor;
                        linha["ValorBruto"] = vendaBilheteriaFormaPagamentoItem.ValorBruto.Valor;
                        linha["EmpresaID"] = vendaBilheteriaFormaPagamentoItem.EmpresaID.Valor;
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

                DataTable tabela = new DataTable("RelatorioVendaBilheteriaFormaPagamentoItem");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                    tabela.Columns.Add("Parcela", typeof(int));
                    tabela.Columns.Add("DataDeposito", typeof(DateTime));
                    tabela.Columns.Add("ValorBruto", typeof(decimal));
                    tabela.Columns.Add("EmpresaID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaBilheteriaFormaPagamentoID"] = vendaBilheteriaFormaPagamentoItem.VendaBilheteriaFormaPagamentoID.Valor;
                        linha["Parcela"] = vendaBilheteriaFormaPagamentoItem.Parcela.Valor;
                        linha["DataDeposito"] = vendaBilheteriaFormaPagamentoItem.DataDeposito.Valor;
                        linha["ValorBruto"] = vendaBilheteriaFormaPagamentoItem.ValorBruto.Valor;
                        linha["EmpresaID"] = vendaBilheteriaFormaPagamentoItem.EmpresaID.Valor;
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
                    case "VendaBilheteriaFormaPagamentoID":
                        sql = "SELECT ID, VendaBilheteriaFormaPagamentoID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaFormaPagamentoID";
                        break;
                    case "Parcela":
                        sql = "SELECT ID, Parcela FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY Parcela";
                        break;
                    case "DataDeposito":
                        sql = "SELECT ID, DataDeposito FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY DataDeposito";
                        break;
                    case "ValorBruto":
                        sql = "SELECT ID, ValorBruto FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY ValorBruto";
                        break;
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tVendaBilheteriaFormaPagamento WHERE " + FiltroSQL + " ORDER BY EmpresaID";
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

    #region "VendaBilheteriaFormaPagamentoItemException"

    [Serializable]
    public class VendaBilheteriaFormaPagamentoItemException : Exception
    {

        public VendaBilheteriaFormaPagamentoItemException() : base() { }

        public VendaBilheteriaFormaPagamentoItemException(string msg) : base(msg) { }

        public VendaBilheteriaFormaPagamentoItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}