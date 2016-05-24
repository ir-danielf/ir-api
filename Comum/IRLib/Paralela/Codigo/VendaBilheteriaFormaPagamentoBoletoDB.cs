/******************************************************
* Arquivo VendaBilheteriaFormaPagamentoBoletoDB.cs
* Gerado em: 25/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "VendaBilheteriaFormaPagamentoBoleto_B"

    public abstract class VendaBilheteriaFormaPagamentoBoleto_B : BaseBD
    {

        public vendabilheteriaformapagamentoid VendaBilheteriaFormaPagamentoID = new vendabilheteriaformapagamentoid();
        public valor Valor = new valor();
        public parcela Parcela = new parcela();
        public timestamp TimeStamp = new timestamp();
        public valorpago ValorPago = new valorpago();
        public datapagamento DataPagamento = new datapagamento();
        public datavencimento DataVencimento = new datavencimento();
        public dataconfirmacao DataConfirmacao = new dataconfirmacao();
        public impresso Impresso = new impresso();

        public VendaBilheteriaFormaPagamentoBoleto_B() { }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaFormaPagamentoBoleto
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteriaFormaPagamentoBoleto WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaFormaPagamentoID.ValorBD = bd.LerInt("VendaBilheteriaFormaPagamentoID").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.Parcela.ValorBD = bd.LerInt("Parcela").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.ValorPago.ValorBD = bd.LerDecimal("ValorPago").ToString();
                    this.DataPagamento.ValorBD = bd.LerString("DataPagamento");
                    this.DataVencimento.ValorBD = bd.LerString("DataVencimento");
                    this.DataConfirmacao.ValorBD = bd.LerString("DataConfirmacao");
                    this.Impresso.ValorBD = bd.LerString("Impresso");
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
        /// Inserir novo(a) VendaBilheteriaFormaPagamentoBoleto
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamentoBoleto(VendaBilheteriaFormaPagamentoID, Valor, Parcela, TimeStamp, ValorPago, DataPagamento, DataVencimento, DataConfirmacao, Impresso) ");
                sql.Append("VALUES (@001,'@002',@003,'@004','@005','@006','@007','@008','@009'); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.Parcela.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.ValorPago.ValorBD);
                sql.Replace("@006", this.DataPagamento.ValorBD);
                sql.Replace("@007", this.DataVencimento.ValorBD);
                sql.Replace("@008", this.DataConfirmacao.ValorBD);
                sql.Replace("@009", this.Impresso.ValorBD);

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
        /// Inserir novo(a) VendaBilheteriaFormaPagamentoBoleto
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tVendaBilheteriaFormaPagamentoBoleto(VendaBilheteriaFormaPagamentoID, Valor, Parcela, TimeStamp, ValorPago, DataPagamento, DataVencimento, DataConfirmacao, Impresso) ");
            sql.Append("VALUES (@001,'@002',@003,'@004','@005','@006','@007','@008','@009'); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());

            sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
            sql.Replace("@002", this.Valor.ValorBD);
            sql.Replace("@003", this.Parcela.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.ValorPago.ValorBD);
            sql.Replace("@006", this.DataPagamento.ValorBD);
            sql.Replace("@007", this.DataVencimento.ValorBD);
            sql.Replace("@008", this.DataConfirmacao.ValorBD);
            sql.Replace("@009", this.Impresso.ValorBD);

            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza VendaBilheteriaFormaPagamentoBoleto
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamentoBoleto SET VendaBilheteriaFormaPagamentoID = @001, Valor = '@002', Parcela = @003, TimeStamp = '@004', ValorPago = '@005', DataPagamento = '@006', DataVencimento = '@007', DataConfirmacao = '@008', Impresso = '@009' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
                sql.Replace("@002", this.Valor.ValorBD);
                sql.Replace("@003", this.Parcela.ValorBD);
                sql.Replace("@004", this.TimeStamp.ValorBD);
                sql.Replace("@005", this.ValorPago.ValorBD);
                sql.Replace("@006", this.DataPagamento.ValorBD);
                sql.Replace("@007", this.DataVencimento.ValorBD);
                sql.Replace("@008", this.DataConfirmacao.ValorBD);
                sql.Replace("@009", this.Impresso.ValorBD);

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
        /// Atualiza VendaBilheteriaFormaPagamentoBoleto
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("UPDATE tVendaBilheteriaFormaPagamentoBoleto SET VendaBilheteriaFormaPagamentoID = @001, Valor = '@002', Parcela = @003, TimeStamp = '@004', ValorPago = '@005', DataPagamento = '@006', DataVencimento = '@007', DataConfirmacao = '@008', Impresso = '@009' ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
            sql.Replace("@002", this.Valor.ValorBD);
            sql.Replace("@003", this.Parcela.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.ValorPago.ValorBD);
            sql.Replace("@006", this.DataPagamento.ValorBD);
            sql.Replace("@007", this.DataVencimento.ValorBD);
            sql.Replace("@008", this.DataConfirmacao.ValorBD);
            sql.Replace("@009", this.Impresso.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui VendaBilheteriaFormaPagamentoBoleto com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamentoBoleto WHERE ID=" + id;

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
        /// Exclui VendaBilheteriaFormaPagamentoBoleto com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamentoBoleto WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui VendaBilheteriaFormaPagamentoBoleto
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
            this.Valor.Limpar();
            this.Parcela.Limpar();
            this.TimeStamp.Limpar();
            this.ValorPago.Limpar();
            this.DataPagamento.Limpar();
            this.DataVencimento.Limpar();
            this.DataConfirmacao.Limpar();
            this.Impresso.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.VendaBilheteriaFormaPagamentoID.Desfazer();
            this.Valor.Desfazer();
            this.Parcela.Desfazer();
            this.TimeStamp.Desfazer();
            this.ValorPago.Desfazer();
            this.DataPagamento.Desfazer();
            this.DataVencimento.Desfazer();
            this.DataConfirmacao.Desfazer();
            this.Impresso.Desfazer();
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

        public class valorpago : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorPago";
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

        public class datapagamento : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataPagamento";
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

        public class datavencimento : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataVencimento";
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

        public class dataconfirmacao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataConfirmacao";
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

        public class impresso : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Impresso";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoBoleto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Parcela", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("ValorPago", typeof(decimal));
                tabela.Columns.Add("DataPagamento", typeof(DateTime));
                tabela.Columns.Add("DataVencimento", typeof(DateTime));
                tabela.Columns.Add("DataConfirmacao", typeof(DateTime));
                tabela.Columns.Add("Impresso", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "VendaBilheteriaFormaPagamentoBoletoLista_B"

    public abstract class VendaBilheteriaFormaPagamentoBoletoLista_B : BaseLista
    {

        protected VendaBilheteriaFormaPagamentoBoleto vendaBilheteriaFormaPagamentoBoleto;

        // passar o Usuario logado no sistema
        public VendaBilheteriaFormaPagamentoBoletoLista_B()
        {
            vendaBilheteriaFormaPagamentoBoleto = new VendaBilheteriaFormaPagamentoBoleto();
        }

        public VendaBilheteriaFormaPagamentoBoleto VendaBilheteriaFormaPagamentoBoleto
        {
            get { return vendaBilheteriaFormaPagamentoBoleto; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteriaFormaPagamentoBoleto especifico
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
                    vendaBilheteriaFormaPagamentoBoleto.Ler(id);
                    return vendaBilheteriaFormaPagamentoBoleto;
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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamentoBoleto";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamentoBoleto";

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
                    sql = "SELECT ID FROM tVendaBilheteriaFormaPagamentoBoleto";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaFormaPagamentoBoleto";

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
        /// Preenche VendaBilheteriaFormaPagamentoBoleto corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                vendaBilheteriaFormaPagamentoBoleto.Ler(id);

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

                bool ok = vendaBilheteriaFormaPagamentoBoleto.Excluir();
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

                        string sqlDelete = "DELETE FROM tVendaBilheteriaFormaPagamentoBoleto WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) VendaBilheteriaFormaPagamentoBoleto na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteriaFormaPagamentoBoleto.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteriaFormaPagamentoBoleto.Control.ID);
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
        /// Obtem uma tabela de todos os campos de VendaBilheteriaFormaPagamentoBoleto carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaFormaPagamentoBoleto");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Parcela", typeof(int));
                tabela.Columns.Add("TimeStamp", typeof(DateTime));
                tabela.Columns.Add("ValorPago", typeof(decimal));
                tabela.Columns.Add("DataPagamento", typeof(DateTime));
                tabela.Columns.Add("DataVencimento", typeof(DateTime));
                tabela.Columns.Add("DataConfirmacao", typeof(DateTime));
                tabela.Columns.Add("Impresso", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteriaFormaPagamentoBoleto.Control.ID;
                        linha["VendaBilheteriaFormaPagamentoID"] = vendaBilheteriaFormaPagamentoBoleto.VendaBilheteriaFormaPagamentoID.Valor;
                        linha["Valor"] = vendaBilheteriaFormaPagamentoBoleto.Valor.Valor;
                        linha["Parcela"] = vendaBilheteriaFormaPagamentoBoleto.Parcela.Valor;
                        linha["TimeStamp"] = vendaBilheteriaFormaPagamentoBoleto.TimeStamp.Valor;
                        linha["ValorPago"] = vendaBilheteriaFormaPagamentoBoleto.ValorPago.Valor;
                        linha["DataPagamento"] = vendaBilheteriaFormaPagamentoBoleto.DataPagamento.Valor;
                        linha["DataVencimento"] = vendaBilheteriaFormaPagamentoBoleto.DataVencimento.Valor;
                        linha["DataConfirmacao"] = vendaBilheteriaFormaPagamentoBoleto.DataConfirmacao.Valor;
                        linha["Impresso"] = vendaBilheteriaFormaPagamentoBoleto.Impresso.Valor;
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

                DataTable tabela = new DataTable("RelatorioVendaBilheteriaFormaPagamentoBoleto");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaBilheteriaFormaPagamentoID", typeof(int));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("Parcela", typeof(int));
                    tabela.Columns.Add("TimeStamp", typeof(DateTime));
                    tabela.Columns.Add("ValorPago", typeof(decimal));
                    tabela.Columns.Add("DataPagamento", typeof(DateTime));
                    tabela.Columns.Add("DataVencimento", typeof(DateTime));
                    tabela.Columns.Add("DataConfirmacao", typeof(DateTime));
                    tabela.Columns.Add("Impresso", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaBilheteriaFormaPagamentoID"] = vendaBilheteriaFormaPagamentoBoleto.VendaBilheteriaFormaPagamentoID.Valor;
                        linha["Valor"] = vendaBilheteriaFormaPagamentoBoleto.Valor.Valor;
                        linha["Parcela"] = vendaBilheteriaFormaPagamentoBoleto.Parcela.Valor;
                        linha["TimeStamp"] = vendaBilheteriaFormaPagamentoBoleto.TimeStamp.Valor;
                        linha["ValorPago"] = vendaBilheteriaFormaPagamentoBoleto.ValorPago.Valor;
                        linha["DataPagamento"] = vendaBilheteriaFormaPagamentoBoleto.DataPagamento.Valor;
                        linha["DataVencimento"] = vendaBilheteriaFormaPagamentoBoleto.DataVencimento.Valor;
                        linha["DataConfirmacao"] = vendaBilheteriaFormaPagamentoBoleto.DataConfirmacao.Valor;
                        linha["Impresso"] = vendaBilheteriaFormaPagamentoBoleto.Impresso.Valor;
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
                        sql = "SELECT ID, VendaBilheteriaFormaPagamentoID FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaFormaPagamentoID";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "Parcela":
                        sql = "SELECT ID, Parcela FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY Parcela";
                        break;
                    case "TimeStamp":
                        sql = "SELECT ID, TimeStamp FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY TimeStamp";
                        break;
                    case "ValorPago":
                        sql = "SELECT ID, ValorPago FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY ValorPago";
                        break;
                    case "DataPagamento":
                        sql = "SELECT ID, DataPagamento FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY DataPagamento";
                        break;
                    case "DataVencimento":
                        sql = "SELECT ID, DataVencimento FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY DataVencimento";
                        break;
                    case "DataConfirmacao":
                        sql = "SELECT ID, DataConfirmacao FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY DataConfirmacao";
                        break;
                    case "Impresso":
                        sql = "SELECT ID, Impresso FROM tVendaBilheteriaFormaPagamentoBoleto WHERE " + FiltroSQL + " ORDER BY Impresso";
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

    #region "VendaBilheteriaFormaPagamentoBoletoException"

    [Serializable]
    public class VendaBilheteriaFormaPagamentoBoletoException : Exception
    {

        public VendaBilheteriaFormaPagamentoBoletoException() : base() { }

        public VendaBilheteriaFormaPagamentoBoletoException(string msg) : base(msg) { }

        public VendaBilheteriaFormaPagamentoBoletoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}