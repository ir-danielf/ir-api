/******************************************************
* Arquivo VendaBilheteriaItemDB.cs
* Gerado em: 08/02/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "VendaBilheteriaItem_B"

    public abstract class VendaBilheteriaItem_B : BaseBD
    {

        public vendabilheteriaid VendaBilheteriaID = new vendabilheteriaid();
        public pacoteid PacoteID = new pacoteid();
        public acao Acao = new acao();
        public taxaconveniencia TaxaConveniencia = new taxaconveniencia();
        public taxaconvenienciavalor TaxaConvenienciaValor = new taxaconvenienciavalor();
        public taxacomissao TaxaComissao = new taxacomissao();
        public comissaovalor ComissaoValor = new comissaovalor();
        public pacotegrupo PacoteGrupo = new pacotegrupo();

        public VendaBilheteriaItem_B() { }

        // passar o Usuario logado no sistema
        public VendaBilheteriaItem_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de VendaBilheteriaItem
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tVendaBilheteriaItem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaConvenienciaValor.ValorBD = bd.LerDecimal("TaxaConvenienciaValor").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.ComissaoValor.ValorBD = bd.LerDecimal("ComissaoValor").ToString();
                    this.PacoteGrupo.ValorBD = bd.LerInt("PacoteGrupo").ToString();
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
        /// Preenche todos os atributos de VendaBilheteriaItem do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xVendaBilheteriaItem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaID.ValorBD = bd.LerInt("VendaBilheteriaID").ToString();
                    this.PacoteID.ValorBD = bd.LerInt("PacoteID").ToString();
                    this.Acao.ValorBD = bd.LerString("Acao");
                    this.TaxaConveniencia.ValorBD = bd.LerInt("TaxaConveniencia").ToString();
                    this.TaxaConvenienciaValor.ValorBD = bd.LerDecimal("TaxaConvenienciaValor").ToString();
                    this.TaxaComissao.ValorBD = bd.LerInt("TaxaComissao").ToString();
                    this.ComissaoValor.ValorBD = bd.LerDecimal("ComissaoValor").ToString();
                    this.PacoteGrupo.ValorBD = bd.LerInt("PacoteGrupo").ToString();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cVendaBilheteriaItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xVendaBilheteriaItem (ID, Versao, VendaBilheteriaID, PacoteID, Acao, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, ComissaoValor, PacoteGrupo) ");
                sql.Append("SELECT ID, @V, VendaBilheteriaID, PacoteID, Acao, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, ComissaoValor, PacoteGrupo FROM tVendaBilheteriaItem WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) VendaBilheteriaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cVendaBilheteriaItem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tVendaBilheteriaItem(ID, VendaBilheteriaID, PacoteID, Acao, TaxaConveniencia, TaxaConvenienciaValor, TaxaComissao, ComissaoValor, PacoteGrupo) ");
                sql.Append("VALUES (@ID,@001,@002,'@003',@004,'@005',@006,'@007',@008)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@006", this.TaxaComissao.ValorBD);
                sql.Replace("@007", this.ComissaoValor.ValorBD);
                sql.Replace("@008", this.PacoteGrupo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Atualiza VendaBilheteriaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cVendaBilheteriaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaItem SET VendaBilheteriaID = @001, PacoteID = @002, Acao = '@003', TaxaConveniencia = @004, TaxaConvenienciaValor = '@005', TaxaComissao = @006, ComissaoValor = '@007', PacoteGrupo = @008 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaID.ValorBD);
                sql.Replace("@002", this.PacoteID.ValorBD);
                sql.Replace("@003", this.Acao.ValorBD);
                sql.Replace("@004", this.TaxaConveniencia.ValorBD);
                sql.Replace("@005", this.TaxaConvenienciaValor.ValorBD);
                sql.Replace("@006", this.TaxaComissao.ValorBD);
                sql.Replace("@007", this.ComissaoValor.ValorBD);
                sql.Replace("@008", this.PacoteGrupo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui VendaBilheteriaItem com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cVendaBilheteriaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tVendaBilheteriaItem WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui VendaBilheteriaItem
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
            this.PacoteID.Limpar();
            this.Acao.Limpar();
            this.TaxaConveniencia.Limpar();
            this.TaxaConvenienciaValor.Limpar();
            this.TaxaComissao.Limpar();
            this.ComissaoValor.Limpar();
            this.PacoteGrupo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.VendaBilheteriaID.Desfazer();
            this.PacoteID.Desfazer();
            this.Acao.Desfazer();
            this.TaxaConveniencia.Desfazer();
            this.TaxaConvenienciaValor.Desfazer();
            this.TaxaComissao.Desfazer();
            this.ComissaoValor.Desfazer();
            this.PacoteGrupo.Desfazer();
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

        public class pacoteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PacoteID";
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

        public class acao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acao";
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

        public class taxaconveniencia : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConveniencia";
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

        public class taxaconvenienciavalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaConvenienciaValor";
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

        public class taxacomissao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaComissao";
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

        public class comissaovalor : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComissaoValor";
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

        public class pacotegrupo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PacoteGrupo";
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

                DataTable tabela = new DataTable("VendaBilheteriaItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoValor", typeof(decimal));
                tabela.Columns.Add("PacoteGrupo", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract bool UltimaAcaoIgualCancelar();

        public abstract bool ChecarIngressoStatusCancelar();

        public abstract string IngressoIDPorItem();

        public abstract DataTable EventoApresentacaoSetorPreco(string itensid, string status);

        public abstract int PrimeiroIngressoID();

        public abstract decimal ValorIngresso();

        public abstract decimal ValorEntrega();

    }
    #endregion

    #region "VendaBilheteriaItemLista_B"

    public abstract class VendaBilheteriaItemLista_B : BaseLista
    {

        private bool backup = false;
        protected VendaBilheteriaItem vendaBilheteriaItem;

        // passar o Usuario logado no sistema
        public VendaBilheteriaItemLista_B()
        {
            vendaBilheteriaItem = new VendaBilheteriaItem();
        }

        // passar o Usuario logado no sistema
        public VendaBilheteriaItemLista_B(int usuarioIDLogado)
        {
            vendaBilheteriaItem = new VendaBilheteriaItem(usuarioIDLogado);
        }

        public VendaBilheteriaItem VendaBilheteriaItem
        {
            get { return vendaBilheteriaItem; }
        }

        /// <summary>
        /// Retorna um IBaseBD de VendaBilheteriaItem especifico
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
                    vendaBilheteriaItem.Ler(id);
                    return vendaBilheteriaItem;
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
                    sql = "SELECT ID FROM tVendaBilheteriaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaItem";

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
                    sql = "SELECT ID FROM tVendaBilheteriaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tVendaBilheteriaItem";

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
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xVendaBilheteriaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xVendaBilheteriaItem";

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

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche VendaBilheteriaItem corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    vendaBilheteriaItem.Ler(id);
                else
                    vendaBilheteriaItem.LerBackup(id);

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

                bool ok = vendaBilheteriaItem.Excluir();
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

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

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
        /// Inseri novo(a) VendaBilheteriaItem na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = vendaBilheteriaItem.Inserir();
                if (ok)
                {
                    lista.Add(vendaBilheteriaItem.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de VendaBilheteriaItem carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("VendaBilheteriaItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                tabela.Columns.Add("PacoteID", typeof(int));
                tabela.Columns.Add("Acao", typeof(string));
                tabela.Columns.Add("TaxaConveniencia", typeof(int));
                tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                tabela.Columns.Add("TaxaComissao", typeof(int));
                tabela.Columns.Add("ComissaoValor", typeof(decimal));
                tabela.Columns.Add("PacoteGrupo", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = vendaBilheteriaItem.Control.ID;
                        linha["VendaBilheteriaID"] = vendaBilheteriaItem.VendaBilheteriaID.Valor;
                        linha["PacoteID"] = vendaBilheteriaItem.PacoteID.Valor;
                        linha["Acao"] = vendaBilheteriaItem.Acao.Valor;
                        linha["TaxaConveniencia"] = vendaBilheteriaItem.TaxaConveniencia.Valor;
                        linha["TaxaConvenienciaValor"] = vendaBilheteriaItem.TaxaConvenienciaValor.Valor;
                        linha["TaxaComissao"] = vendaBilheteriaItem.TaxaComissao.Valor;
                        linha["ComissaoValor"] = vendaBilheteriaItem.ComissaoValor.Valor;
                        linha["PacoteGrupo"] = vendaBilheteriaItem.PacoteGrupo.Valor;
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

                DataTable tabela = new DataTable("RelatorioVendaBilheteriaItem");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaBilheteriaID", typeof(int));
                    tabela.Columns.Add("PacoteID", typeof(int));
                    tabela.Columns.Add("Acao", typeof(string));
                    tabela.Columns.Add("TaxaConveniencia", typeof(int));
                    tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
                    tabela.Columns.Add("TaxaComissao", typeof(int));
                    tabela.Columns.Add("ComissaoValor", typeof(decimal));
                    tabela.Columns.Add("PacoteGrupo", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaBilheteriaID"] = vendaBilheteriaItem.VendaBilheteriaID.Valor;
                        linha["PacoteID"] = vendaBilheteriaItem.PacoteID.Valor;
                        linha["Acao"] = vendaBilheteriaItem.Acao.Valor;
                        linha["TaxaConveniencia"] = vendaBilheteriaItem.TaxaConveniencia.Valor;
                        linha["TaxaConvenienciaValor"] = vendaBilheteriaItem.TaxaConvenienciaValor.Valor;
                        linha["TaxaComissao"] = vendaBilheteriaItem.TaxaComissao.Valor;
                        linha["ComissaoValor"] = vendaBilheteriaItem.ComissaoValor.Valor;
                        linha["PacoteGrupo"] = vendaBilheteriaItem.PacoteGrupo.Valor;
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
                        sql = "SELECT ID, VendaBilheteriaID FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaID";
                        break;
                    case "PacoteID":
                        sql = "SELECT ID, PacoteID FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY PacoteID";
                        break;
                    case "Acao":
                        sql = "SELECT ID, Acao FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY Acao";
                        break;
                    case "TaxaConveniencia":
                        sql = "SELECT ID, TaxaConveniencia FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY TaxaConveniencia";
                        break;
                    case "TaxaConvenienciaValor":
                        sql = "SELECT ID, TaxaConvenienciaValor FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY TaxaConvenienciaValor";
                        break;
                    case "TaxaComissao":
                        sql = "SELECT ID, TaxaComissao FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY TaxaComissao";
                        break;
                    case "ComissaoValor":
                        sql = "SELECT ID, ComissaoValor FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY ComissaoValor";
                        break;
                    case "PacoteGrupo":
                        sql = "SELECT ID, PacoteGrupo FROM tVendaBilheteriaItem WHERE " + FiltroSQL + " ORDER BY PacoteGrupo";
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

    #region "VendaBilheteriaItemException"

    [Serializable]
    public class VendaBilheteriaItemException : Exception
    {

        public VendaBilheteriaItemException() : base() { }

        public VendaBilheteriaItemException(string msg) : base(msg) { }

        public VendaBilheteriaItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}