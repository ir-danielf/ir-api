/******************************************************
* Arquivo CotaItemDB.cs
* Gerado em: 10/07/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "CotaItem_B"

    public abstract class CotaItem_B : BaseBD
    {

        public cotaid CotaID = new cotaid();
        public precoiniciacom PrecoIniciaCom = new precoiniciacom();
        public quantidade Quantidade = new quantidade();
        public quantidadeporcliente QuantidadePorCliente = new quantidadeporcliente();
        public parceiroid ParceiroID = new parceiroid();
        public validabin ValidaBin = new validabin();
        public textovalidacao TextoValidacao = new textovalidacao();
        public obrigatoriedadeid ObrigatoriedadeID = new obrigatoriedadeid();
        public cpfresponsavel CPFResponsavel = new cpfresponsavel();
        public termo Termo = new termo();
        public termosite TermoSite = new termosite();
        public nominal Nominal = new nominal();
        public quantidadeporcodigo QuantidadePorCodigo = new quantidadeporcodigo();

        public CotaItem_B() { }

        // passar o Usuario logado no sistema
        public CotaItem_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CotaItem
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCotaItem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.PrecoIniciaCom.ValorBD = bd.LerString("PrecoIniciaCom");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.ParceiroID.ValorBD = bd.LerInt("ParceiroID").ToString();
                    this.ValidaBin.ValorBD = bd.LerString("ValidaBin");
                    this.TextoValidacao.ValorBD = bd.LerString("TextoValidacao");
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.Termo.ValorBD = bd.LerString("Termo");
                    this.TermoSite.ValorBD = bd.LerString("TermoSite");
                    this.Nominal.ValorBD = bd.LerString("Nominal");
                    this.QuantidadePorCodigo.ValorBD = bd.LerInt("QuantidadePorCodigo").ToString();
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
        /// Preenche todos os atributos de CotaItem do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCotaItem WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.PrecoIniciaCom.ValorBD = bd.LerString("PrecoIniciaCom");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.ParceiroID.ValorBD = bd.LerInt("ParceiroID").ToString();
                    this.ValidaBin.ValorBD = bd.LerString("ValidaBin");
                    this.TextoValidacao.ValorBD = bd.LerString("TextoValidacao");
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.Termo.ValorBD = bd.LerString("Termo");
                    this.TermoSite.ValorBD = bd.LerString("TermoSite");
                    this.Nominal.ValorBD = bd.LerString("Nominal");
                    this.QuantidadePorCodigo.ValorBD = bd.LerInt("QuantidadePorCodigo").ToString();
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
                sql.Append("INSERT INTO cCotaItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCotaItem (ID, Versao, CotaID, PrecoIniciaCom, Quantidade, QuantidadePorCliente, ParceiroID, ValidaBin, TextoValidacao, ObrigatoriedadeID, CPFResponsavel, Termo, TermoSite, Nominal, QuantidadePorCodigo) ");
                sql.Append("SELECT ID, @V, CotaID, PrecoIniciaCom, Quantidade, QuantidadePorCliente, ParceiroID, ValidaBin, TextoValidacao, ObrigatoriedadeID, CPFResponsavel, Termo, TermoSite, Nominal, QuantidadePorCodigo FROM tCotaItem WHERE ID = @I");
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
        /// Inserir novo(a) CotaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCotaItem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaItem(ID, CotaID, PrecoIniciaCom, Quantidade, QuantidadePorCliente, ParceiroID, ValidaBin, TextoValidacao, ObrigatoriedadeID, CPFResponsavel, Termo, TermoSite, Nominal, QuantidadePorCodigo) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,@004,@005,'@006','@007',@008,'@009','@010','@011','@012',@013)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@005", this.ParceiroID.ValorBD);
                sql.Replace("@006", this.ValidaBin.ValorBD);
                sql.Replace("@007", this.TextoValidacao.ValorBD);
                sql.Replace("@008", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@009", this.CPFResponsavel.ValorBD);
                sql.Replace("@010", this.Termo.ValorBD);
                sql.Replace("@011", this.TermoSite.ValorBD);
                sql.Replace("@012", this.Nominal.ValorBD);
                sql.Replace("@013", this.QuantidadePorCodigo.ValorBD);

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
        /// Atualiza CotaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCotaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCotaItem SET CotaID = @001, PrecoIniciaCom = '@002', Quantidade = @003, QuantidadePorCliente = @004, ParceiroID = @005, ValidaBin = '@006', TextoValidacao = '@007', ObrigatoriedadeID = @008, CPFResponsavel = '@009', Termo = '@010', TermoSite = '@011', Nominal = '@012', QuantidadePorCodigo = @013 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@005", this.ParceiroID.ValorBD);
                sql.Replace("@006", this.ValidaBin.ValorBD);
                sql.Replace("@007", this.TextoValidacao.ValorBD);
                sql.Replace("@008", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@009", this.CPFResponsavel.ValorBD);
                sql.Replace("@010", this.Termo.ValorBD);
                sql.Replace("@011", this.TermoSite.ValorBD);
                sql.Replace("@012", this.Nominal.ValorBD);
                sql.Replace("@013", this.QuantidadePorCodigo.ValorBD);

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
        /// Atualiza CotaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCotaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCotaItem SET CotaID = @001, PrecoIniciaCom = '@002', Quantidade = @003, QuantidadePorCliente = @004, ParceiroID = @005, ValidaBin = '@006', TextoValidacao = '@007', ObrigatoriedadeID = @008, CPFResponsavel = '@009', Termo = '@010', TermoSite = '@011', Nominal = '@012', QuantidadePorCodigo = @013 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@005", this.ParceiroID.ValorBD);
                sql.Replace("@006", this.ValidaBin.ValorBD);
                sql.Replace("@007", this.TextoValidacao.ValorBD);
                sql.Replace("@008", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@009", this.CPFResponsavel.ValorBD);
                sql.Replace("@010", this.Termo.ValorBD);
                sql.Replace("@011", this.TermoSite.ValorBD);
                sql.Replace("@012", this.Nominal.ValorBD);
                sql.Replace("@013", this.QuantidadePorCodigo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui CotaItem com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCotaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCotaItem WHERE ID=" + id;

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
        /// Exclui CotaItem com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCotaItem WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCotaItem WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui CotaItem
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

            this.CotaID.Limpar();
            this.PrecoIniciaCom.Limpar();
            this.Quantidade.Limpar();
            this.QuantidadePorCliente.Limpar();
            this.ParceiroID.Limpar();
            this.ValidaBin.Limpar();
            this.TextoValidacao.Limpar();
            this.ObrigatoriedadeID.Limpar();
            this.CPFResponsavel.Limpar();
            this.Termo.Limpar();
            this.TermoSite.Limpar();
            this.Nominal.Limpar();
            this.QuantidadePorCodigo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.CotaID.Desfazer();
            this.PrecoIniciaCom.Desfazer();
            this.Quantidade.Desfazer();
            this.QuantidadePorCliente.Desfazer();
            this.ParceiroID.Desfazer();
            this.ValidaBin.Desfazer();
            this.TextoValidacao.Desfazer();
            this.ObrigatoriedadeID.Desfazer();
            this.CPFResponsavel.Desfazer();
            this.Termo.Desfazer();
            this.TermoSite.Desfazer();
            this.Nominal.Desfazer();
            this.QuantidadePorCodigo.Desfazer();
        }

        public class cotaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CotaID";
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

        public class precoiniciacom : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoIniciaCom";
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

        public class quantidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Quantidade";
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

        public class quantidadeporcliente : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadePorCliente";
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

        public class parceiroid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ParceiroID";
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

        public class validabin : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValidaBin";
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

        public class textovalidacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TextoValidacao";
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

        public class obrigatoriedadeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObrigatoriedadeID";
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

        public class cpfresponsavel : BooleanProperty
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

        public class termo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Termo";
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

        public class termosite : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TermoSite";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 3000;
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

        public class nominal : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nominal";
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

        public class quantidadeporcodigo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadePorCodigo";
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

                DataTable tabela = new DataTable("CotaItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("ParceiroID", typeof(int));
                tabela.Columns.Add("ValidaBin", typeof(bool));
                tabela.Columns.Add("TextoValidacao", typeof(string));
                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                tabela.Columns.Add("CPFResponsavel", typeof(bool));
                tabela.Columns.Add("Termo", typeof(string));
                tabela.Columns.Add("TermoSite", typeof(string));
                tabela.Columns.Add("Nominal", typeof(bool));
                tabela.Columns.Add("QuantidadePorCodigo", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "CotaItemLista_B"

    public abstract class CotaItemLista_B : BaseLista
    {

        private bool backup = false;
        protected CotaItem cotaItem;

        // passar o Usuario logado no sistema
        public CotaItemLista_B()
        {
            cotaItem = new CotaItem();
        }

        // passar o Usuario logado no sistema
        public CotaItemLista_B(int usuarioIDLogado)
        {
            cotaItem = new CotaItem(usuarioIDLogado);
        }

        public CotaItem CotaItem
        {
            get { return cotaItem; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CotaItem especifico
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
                    cotaItem.Ler(id);
                    return cotaItem;
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
                    sql = "SELECT ID FROM tCotaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCotaItem";

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
                    sql = "SELECT ID FROM tCotaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCotaItem";

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
                    sql = "SELECT ID FROM xCotaItem";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCotaItem";

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
        /// Preenche CotaItem corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    cotaItem.Ler(id);
                else
                    cotaItem.LerBackup(id);

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

                bool ok = cotaItem.Excluir();
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
        /// Inseri novo(a) CotaItem na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = cotaItem.Inserir();
                if (ok)
                {
                    lista.Add(cotaItem.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CotaItem carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CotaItem");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("ParceiroID", typeof(int));
                tabela.Columns.Add("ValidaBin", typeof(bool));
                tabela.Columns.Add("TextoValidacao", typeof(string));
                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                tabela.Columns.Add("CPFResponsavel", typeof(bool));
                tabela.Columns.Add("Termo", typeof(string));
                tabela.Columns.Add("TermoSite", typeof(string));
                tabela.Columns.Add("Nominal", typeof(bool));
                tabela.Columns.Add("QuantidadePorCodigo", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cotaItem.Control.ID;
                        linha["CotaID"] = cotaItem.CotaID.Valor;
                        linha["PrecoIniciaCom"] = cotaItem.PrecoIniciaCom.Valor;
                        linha["Quantidade"] = cotaItem.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = cotaItem.QuantidadePorCliente.Valor;
                        linha["ParceiroID"] = cotaItem.ParceiroID.Valor;
                        linha["ValidaBin"] = cotaItem.ValidaBin.Valor;
                        linha["TextoValidacao"] = cotaItem.TextoValidacao.Valor;
                        linha["ObrigatoriedadeID"] = cotaItem.ObrigatoriedadeID.Valor;
                        linha["CPFResponsavel"] = cotaItem.CPFResponsavel.Valor;
                        linha["Termo"] = cotaItem.Termo.Valor;
                        linha["TermoSite"] = cotaItem.TermoSite.Valor;
                        linha["Nominal"] = cotaItem.Nominal.Valor;
                        linha["QuantidadePorCodigo"] = cotaItem.QuantidadePorCodigo.Valor;
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

                DataTable tabela = new DataTable("RelatorioCotaItem");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("CotaID", typeof(int));
                    tabela.Columns.Add("PrecoIniciaCom", typeof(string));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                    tabela.Columns.Add("ParceiroID", typeof(int));
                    tabela.Columns.Add("ValidaBin", typeof(bool));
                    tabela.Columns.Add("TextoValidacao", typeof(string));
                    tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                    tabela.Columns.Add("CPFResponsavel", typeof(bool));
                    tabela.Columns.Add("Termo", typeof(string));
                    tabela.Columns.Add("TermoSite", typeof(string));
                    tabela.Columns.Add("Nominal", typeof(bool));
                    tabela.Columns.Add("QuantidadePorCodigo", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["CotaID"] = cotaItem.CotaID.Valor;
                        linha["PrecoIniciaCom"] = cotaItem.PrecoIniciaCom.Valor;
                        linha["Quantidade"] = cotaItem.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = cotaItem.QuantidadePorCliente.Valor;
                        linha["ParceiroID"] = cotaItem.ParceiroID.Valor;
                        linha["ValidaBin"] = cotaItem.ValidaBin.Valor;
                        linha["TextoValidacao"] = cotaItem.TextoValidacao.Valor;
                        linha["ObrigatoriedadeID"] = cotaItem.ObrigatoriedadeID.Valor;
                        linha["CPFResponsavel"] = cotaItem.CPFResponsavel.Valor;
                        linha["Termo"] = cotaItem.Termo.Valor;
                        linha["TermoSite"] = cotaItem.TermoSite.Valor;
                        linha["Nominal"] = cotaItem.Nominal.Valor;
                        linha["QuantidadePorCodigo"] = cotaItem.QuantidadePorCodigo.Valor;
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
                    case "CotaID":
                        sql = "SELECT ID, CotaID FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY CotaID";
                        break;
                    case "PrecoIniciaCom":
                        sql = "SELECT ID, PrecoIniciaCom FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY PrecoIniciaCom";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "QuantidadePorCliente":
                        sql = "SELECT ID, QuantidadePorCliente FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY QuantidadePorCliente";
                        break;
                    case "ParceiroID":
                        sql = "SELECT ID, ParceiroID FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY ParceiroID";
                        break;
                    case "ValidaBin":
                        sql = "SELECT ID, ValidaBin FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY ValidaBin";
                        break;
                    case "TextoValidacao":
                        sql = "SELECT ID, TextoValidacao FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY TextoValidacao";
                        break;
                    case "ObrigatoriedadeID":
                        sql = "SELECT ID, ObrigatoriedadeID FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY ObrigatoriedadeID";
                        break;
                    case "CPFResponsavel":
                        sql = "SELECT ID, CPFResponsavel FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY CPFResponsavel";
                        break;
                    case "Termo":
                        sql = "SELECT ID, Termo FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY Termo";
                        break;
                    case "TermoSite":
                        sql = "SELECT ID, TermoSite FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY TermoSite";
                        break;
                    case "Nominal":
                        sql = "SELECT ID, Nominal FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY Nominal";
                        break;
                    case "QuantidadePorCodigo":
                        sql = "SELECT ID, QuantidadePorCodigo FROM tCotaItem WHERE " + FiltroSQL + " ORDER BY QuantidadePorCodigo";
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

    #region "CotaItemException"

    [Serializable]
    public class CotaItemException : Exception
    {

        public CotaItemException() : base() { }

        public CotaItemException(string msg) : base(msg) { }

        public CotaItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}