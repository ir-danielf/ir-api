/******************************************************
* Arquivo CompraTemporariaDB.cs
* Gerado em: 05/05/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using IRCore.Util;

namespace IRLib
{

    #region "CompraTemporaria_B"

    public abstract class CompraTemporaria_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public formapagamentoid FormaPagamentoID = new formapagamentoid();
        public enderecoid EnderecoID = new enderecoid();
        public pdvselecionado PDVSelecionado = new pdvselecionado();
        public entregacontroleidselecionado EntregaControleIDSelecionado = new entregacontroleidselecionado();
        public dataselecionada DataSelecionada = new dataselecionada();
        public entregavalor EntregaValor = new entregavalor();
        public parcelas Parcelas = new parcelas();
        public valortotal ValorTotal = new valortotal();
        public bandeira Bandeira = new bandeira();
        public sessionid SessionID = new sessionid();
        public bin BIN = new bin();
        public codigotrocafixo CodigoTrocaFixo = new codigotrocafixo();
        public somentevir SomenteVir = new somentevir();
        public somentecortesias SomenteCortesias = new somentecortesias();

        public CompraTemporaria_B() { }

        /// <summary>
        /// Preenche todos os atributos de CompraTemporaria
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCompraTemporaria WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.FormaPagamentoID.ValorBD = bd.LerInt("FormaPagamentoID").ToString();
                    this.EnderecoID.ValorBD = bd.LerInt("EnderecoID").ToString();
                    this.PDVSelecionado.ValorBD = bd.LerInt("PDVSelecionado").ToString();
                    this.EntregaControleIDSelecionado.ValorBD = bd.LerInt("EntregaControleIDSelecionado").ToString();
                    this.DataSelecionada.ValorBD = bd.LerString("DataSelecionada");
                    this.EntregaValor.ValorBD = bd.LerDecimal("EntregaValor").ToString();
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.ValorTotal.ValorBD = bd.LerDecimal("ValorTotal").ToString();
                    this.Bandeira.ValorBD = bd.LerString("Bandeira");
                    this.SessionID.ValorBD = bd.LerString("SessionID");
                    this.BIN.ValorBD = bd.LerInt("BIN").ToString();
                    this.CodigoTrocaFixo.ValorBD = bd.LerString("CodigoTrocaFixo");
                    this.SomenteVir.ValorBD = bd.LerString("SomenteVir");
                    this.SomenteCortesias.ValorBD = bd.LerString("SomenteCortesias");
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
        /// Inserir novo(a) CompraTemporaria
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            LogUtil.Debug(string.Format("##CompraTemporariaDB.Inserindo## SESSION {0}", this.SessionID.ValorBD));

            try
            {
                var sql = new StringBuilder();
                sql.Append("INSERT INTO tCompraTemporaria(ClienteID, FormaPagamentoID, EnderecoID, PDVSelecionado, EntregaControleIDSelecionado, DataSelecionada, EntregaValor, Parcelas, ValorTotal, Bandeira, SessionID, BIN, CodigoTrocaFixo, SomenteVir, SomenteCortesias) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,'@006','@007',@008,'@009','@010','@011',@012,'@013','@014','@015'); SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.EnderecoID.ValorBD);
                sql.Replace("@004", this.PDVSelecionado.ValorBD);
                sql.Replace("@005", this.EntregaControleIDSelecionado.ValorBD);
                sql.Replace("@006", this.DataSelecionada.ValorBD);
                sql.Replace("@007", this.EntregaValor.ValorBD);
                sql.Replace("@008", this.Parcelas.ValorBD);
                sql.Replace("@009", this.ValorTotal.ValorBD);
                sql.Replace("@010", this.Bandeira.ValorBD);
                sql.Replace("@011", this.SessionID.ValorBD);
                sql.Replace("@012", this.BIN.ValorBD);
                sql.Replace("@013", this.CodigoTrocaFixo.ValorBD);
                sql.Replace("@014", this.SomenteVir.ValorBD);
                sql.Replace("@015", this.SomenteCortesias.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                LogUtil.Debug(string.Format("##CompraTemporariaDB.Inserindo.SUCCESS## SESSION {0}, MSG {1}", this.SessionID.ValorBD, "Inserido: " + (this.Control.ID > 0)));

                return this.Control.ID > 0;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CompraTemporariaDB.Inserindo.EXCEPTION## SESSION {0}, MSG {1}", this.SessionID.ValorBD, ex.Message), ex);

                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }

        /// <summary>
        /// Atualiza CompraTemporaria
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCompraTemporaria SET ClienteID = @001, FormaPagamentoID = @002, EnderecoID = @003, PDVSelecionado = @004, EntregaControleIDSelecionado = @005, DataSelecionada = '@006', EntregaValor = '@007', Parcelas = @008, ValorTotal = '@009', Bandeira = '@010', SessionID = '@011', BIN = @012, CodigoTrocaFixo = '@013', SomenteVir = '@014', SomenteCortesias = '@015' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.FormaPagamentoID.ValorBD);
                sql.Replace("@003", this.EnderecoID.ValorBD);
                sql.Replace("@004", this.PDVSelecionado.ValorBD);
                sql.Replace("@005", this.EntregaControleIDSelecionado.ValorBD);
                sql.Replace("@006", this.DataSelecionada.ValorBD);
                sql.Replace("@007", this.EntregaValor.ValorBD);
                sql.Replace("@008", this.Parcelas.ValorBD);
                sql.Replace("@009", this.ValorTotal.ValorBD);
                sql.Replace("@010", this.Bandeira.ValorBD);
                sql.Replace("@011", this.SessionID.ValorBD);
                sql.Replace("@012", this.BIN.ValorBD);
                sql.Replace("@013", this.CodigoTrocaFixo.ValorBD);
                sql.Replace("@014", this.SomenteVir.ValorBD);
                sql.Replace("@015", this.SomenteCortesias.ValorBD);

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
        /// Exclui CompraTemporaria com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tCompraTemporaria WHERE ID=" + id;

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
        /// Exclui CompraTemporaria
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
            this.FormaPagamentoID.Limpar();
            this.EnderecoID.Limpar();
            this.PDVSelecionado.Limpar();
            this.EntregaControleIDSelecionado.Limpar();
            this.DataSelecionada.Limpar();
            this.EntregaValor.Limpar();
            this.Parcelas.Limpar();
            this.ValorTotal.Limpar();
            this.Bandeira.Limpar();
            this.SessionID.Limpar();
            this.BIN.Limpar();
            this.CodigoTrocaFixo.Limpar();
            this.SomenteVir.Limpar();
            this.SomenteCortesias.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.ClienteID.Desfazer();
            this.FormaPagamentoID.Desfazer();
            this.EnderecoID.Desfazer();
            this.PDVSelecionado.Desfazer();
            this.EntregaControleIDSelecionado.Desfazer();
            this.DataSelecionada.Desfazer();
            this.EntregaValor.Desfazer();
            this.Parcelas.Desfazer();
            this.ValorTotal.Desfazer();
            this.Bandeira.Desfazer();
            this.SessionID.Desfazer();
            this.BIN.Desfazer();
            this.CodigoTrocaFixo.Desfazer();
            this.SomenteVir.Desfazer();
            this.SomenteCortesias.Desfazer();
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

        public class formapagamentoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaPagamentoID";
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

        public class enderecoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnderecoID";
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

        public class pdvselecionado : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PDVSelecionado";
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

        public class entregacontroleidselecionado : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaControleIDSelecionado";
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

        public class dataselecionada : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataSelecionada";
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

        public class entregavalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaValor";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class parcelas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Parcelas";
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

        public class valortotal : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorTotal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class bandeira : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Bandeira";
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

        public class sessionid : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SessionID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 150;
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

        public class bin : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BIN";
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

        public class codigotrocafixo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoTrocaFixo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 18;
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

        public class somentevir : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "SomenteVir";
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

        public class somentecortesias : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "SomenteCortesias";
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

                DataTable tabela = new DataTable("CompraTemporaria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("EnderecoID", typeof(int));
                tabela.Columns.Add("PDVSelecionado", typeof(int));
                tabela.Columns.Add("EntregaControleIDSelecionado", typeof(int));
                tabela.Columns.Add("DataSelecionada", typeof(string));
                tabela.Columns.Add("EntregaValor", typeof(decimal));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("ValorTotal", typeof(decimal));
                tabela.Columns.Add("Bandeira", typeof(string));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("BIN", typeof(int));
                tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                tabela.Columns.Add("SomenteVir", typeof(bool));
                tabela.Columns.Add("SomenteCortesias", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "CompraTemporariaLista_B"

    public abstract class CompraTemporariaLista_B : BaseLista
    {

        protected CompraTemporaria compraTemporaria;

        // passar o Usuario logado no sistema
        public CompraTemporariaLista_B()
        {
            compraTemporaria = new CompraTemporaria();
        }

        public CompraTemporaria CompraTemporaria
        {
            get { return compraTemporaria; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CompraTemporaria especifico
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
                    compraTemporaria.Ler(id);
                    return compraTemporaria;
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
                    sql = "SELECT ID FROM tCompraTemporaria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCompraTemporaria";

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
                    sql = "SELECT ID FROM tCompraTemporaria";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCompraTemporaria";

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
        /// Preenche CompraTemporaria corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                compraTemporaria.Ler(id);

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

                bool ok = compraTemporaria.Excluir();
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

                        string sqlDelete = "DELETE FROM tCompraTemporaria WHERE ID in (" + ids + ")";

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
        /// Inseri novo(a) CompraTemporaria na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = compraTemporaria.Inserir();
                if (ok)
                {
                    lista.Add(compraTemporaria.Control.ID);
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
        /// Obtem uma tabela de todos os campos de CompraTemporaria carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CompraTemporaria");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("FormaPagamentoID", typeof(int));
                tabela.Columns.Add("EnderecoID", typeof(int));
                tabela.Columns.Add("PDVSelecionado", typeof(int));
                tabela.Columns.Add("EntregaControleIDSelecionado", typeof(int));
                tabela.Columns.Add("DataSelecionada", typeof(string));
                tabela.Columns.Add("EntregaValor", typeof(decimal));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("ValorTotal", typeof(decimal));
                tabela.Columns.Add("Bandeira", typeof(string));
                tabela.Columns.Add("SessionID", typeof(string));
                tabela.Columns.Add("BIN", typeof(int));
                tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                tabela.Columns.Add("SomenteVir", typeof(bool));
                tabela.Columns.Add("SomenteCortesias", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = compraTemporaria.Control.ID;
                        linha["ClienteID"] = compraTemporaria.ClienteID.Valor;
                        linha["FormaPagamentoID"] = compraTemporaria.FormaPagamentoID.Valor;
                        linha["EnderecoID"] = compraTemporaria.EnderecoID.Valor;
                        linha["PDVSelecionado"] = compraTemporaria.PDVSelecionado.Valor;
                        linha["EntregaControleIDSelecionado"] = compraTemporaria.EntregaControleIDSelecionado.Valor;
                        linha["DataSelecionada"] = compraTemporaria.DataSelecionada.Valor;
                        linha["EntregaValor"] = compraTemporaria.EntregaValor.Valor;
                        linha["Parcelas"] = compraTemporaria.Parcelas.Valor;
                        linha["ValorTotal"] = compraTemporaria.ValorTotal.Valor;
                        linha["Bandeira"] = compraTemporaria.Bandeira.Valor;
                        linha["SessionID"] = compraTemporaria.SessionID.Valor;
                        linha["BIN"] = compraTemporaria.BIN.Valor;
                        linha["CodigoTrocaFixo"] = compraTemporaria.CodigoTrocaFixo.Valor;
                        linha["SomenteVir"] = compraTemporaria.SomenteVir.Valor;
                        linha["SomenteCortesias"] = compraTemporaria.SomenteCortesias.Valor;
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

                DataTable tabela = new DataTable("RelatorioCompraTemporaria");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("FormaPagamentoID", typeof(int));
                    tabela.Columns.Add("EnderecoID", typeof(int));
                    tabela.Columns.Add("PDVSelecionado", typeof(int));
                    tabela.Columns.Add("EntregaControleIDSelecionado", typeof(int));
                    tabela.Columns.Add("DataSelecionada", typeof(string));
                    tabela.Columns.Add("EntregaValor", typeof(decimal));
                    tabela.Columns.Add("Parcelas", typeof(int));
                    tabela.Columns.Add("ValorTotal", typeof(decimal));
                    tabela.Columns.Add("Bandeira", typeof(string));
                    tabela.Columns.Add("SessionID", typeof(string));
                    tabela.Columns.Add("BIN", typeof(int));
                    tabela.Columns.Add("CodigoTrocaFixo", typeof(string));
                    tabela.Columns.Add("SomenteVir", typeof(bool));
                    tabela.Columns.Add("SomenteCortesias", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = compraTemporaria.ClienteID.Valor;
                        linha["FormaPagamentoID"] = compraTemporaria.FormaPagamentoID.Valor;
                        linha["EnderecoID"] = compraTemporaria.EnderecoID.Valor;
                        linha["PDVSelecionado"] = compraTemporaria.PDVSelecionado.Valor;
                        linha["EntregaControleIDSelecionado"] = compraTemporaria.EntregaControleIDSelecionado.Valor;
                        linha["DataSelecionada"] = compraTemporaria.DataSelecionada.Valor;
                        linha["EntregaValor"] = compraTemporaria.EntregaValor.Valor;
                        linha["Parcelas"] = compraTemporaria.Parcelas.Valor;
                        linha["ValorTotal"] = compraTemporaria.ValorTotal.Valor;
                        linha["Bandeira"] = compraTemporaria.Bandeira.Valor;
                        linha["SessionID"] = compraTemporaria.SessionID.Valor;
                        linha["BIN"] = compraTemporaria.BIN.Valor;
                        linha["CodigoTrocaFixo"] = compraTemporaria.CodigoTrocaFixo.Valor;
                        linha["SomenteVir"] = compraTemporaria.SomenteVir.Valor;
                        linha["SomenteCortesias"] = compraTemporaria.SomenteCortesias.Valor;
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
                        sql = "SELECT ID, ClienteID FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "FormaPagamentoID":
                        sql = "SELECT ID, FormaPagamentoID FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY FormaPagamentoID";
                        break;
                    case "EnderecoID":
                        sql = "SELECT ID, EnderecoID FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY EnderecoID";
                        break;
                    case "PDVSelecionado":
                        sql = "SELECT ID, PDVSelecionado FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY PDVSelecionado";
                        break;
                    case "EntregaControleIDSelecionado":
                        sql = "SELECT ID, EntregaControleIDSelecionado FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY EntregaControleIDSelecionado";
                        break;
                    case "DataSelecionada":
                        sql = "SELECT ID, DataSelecionada FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY DataSelecionada";
                        break;
                    case "EntregaValor":
                        sql = "SELECT ID, EntregaValor FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY EntregaValor";
                        break;
                    case "Parcelas":
                        sql = "SELECT ID, Parcelas FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY Parcelas";
                        break;
                    case "ValorTotal":
                        sql = "SELECT ID, ValorTotal FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY ValorTotal";
                        break;
                    case "Bandeira":
                        sql = "SELECT ID, Bandeira FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY Bandeira";
                        break;
                    case "SessionID":
                        sql = "SELECT ID, SessionID FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY SessionID";
                        break;
                    case "BIN":
                        sql = "SELECT ID, BIN FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY BIN";
                        break;
                    case "CodigoTrocaFixo":
                        sql = "SELECT ID, CodigoTrocaFixo FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY CodigoTrocaFixo";
                        break;
                    case "SomenteVir":
                        sql = "SELECT ID, SomenteVir FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY SomenteVir";
                        break;
                    case "SomenteCortesias":
                        sql = "SELECT ID, SomenteCortesias FROM tCompraTemporaria WHERE " + FiltroSQL + " ORDER BY SomenteCortesias";
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

    #region "CompraTemporariaException"

    [Serializable]
    public class CompraTemporariaException : Exception
    {

        public CompraTemporariaException() : base() { }

        public CompraTemporariaException(string msg) : base(msg) { }

        public CompraTemporariaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}