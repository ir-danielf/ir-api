/******************************************************
* Arquivo ApresentacaoSetorDB.cs
* Gerado em: 08/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "ApresentacaoSetor_B"

    public abstract class ApresentacaoSetor_B : BaseBD
    {

        public setorid SetorID = new setorid();
        public apresentacaoid ApresentacaoID = new apresentacaoid();
        public versaoimagemingresso VersaoImagemIngresso = new versaoimagemingresso();
        public versaoimagemvale VersaoImagemVale = new versaoimagemvale();
        public versaoimagemvale2 VersaoImagemVale2 = new versaoimagemvale2();
        public versaoimagemvale3 VersaoImagemVale3 = new versaoimagemvale3();
        public ingressosgerados IngressosGerados = new ingressosgerados();
        public cotaid CotaID = new cotaid();
        public quantidade Quantidade = new quantidade();
        public quantidadeporcliente QuantidadePorCliente = new quantidadeporcliente();
        public principalprecoid PrincipalPrecoID = new principalprecoid();
        public nvendelugar NVendeLugar = new nvendelugar();

        public ApresentacaoSetor_B() { }

        // passar o Usuario logado no sistema
        public ApresentacaoSetor_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ApresentacaoSetor
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tApresentacaoSetor WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.IngressosGerados.ValorBD = bd.LerString("IngressosGerados");
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.PrincipalPrecoID.ValorBD = bd.LerInt("PrincipalPrecoID").ToString();
                    this.NVendeLugar.ValorBD = bd.LerString("NVendeLugar");
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
        /// Preenche todos os atributos de ApresentacaoSetor do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xApresentacaoSetor WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.IngressosGerados.ValorBD = bd.LerString("IngressosGerados");
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.PrincipalPrecoID.ValorBD = bd.LerInt("PrincipalPrecoID").ToString();
                    this.NVendeLugar.ValorBD = bd.LerString("NVendeLugar");
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
                sql.Append("INSERT INTO cApresentacaoSetor (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xApresentacaoSetor (ID, Versao, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID, NVendeLugar) ");
                sql.Append("SELECT ID, @V, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID, NVendeLugar FROM tApresentacaoSetor WHERE ID = @I");
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
        /// Inserir novo(a) ApresentacaoSetor
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cApresentacaoSetor");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacaoSetor(ID, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID, NVendeLugar) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,@005,@006,'@007',@008,@009,@010,@011,'@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);
                sql.Replace("@012", this.NVendeLugar.ValorBD);

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
        /// Inserir novo(a) ApresentacaoSetor
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cApresentacaoSetor");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacaoSetor(ID, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID, NVendeLugar) ");
                sql.Append("VALUES (@ID,@001,@002,@003,@004,@005,@006,'@007',@008,@009,@010,@011,'@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);
                sql.Replace("@012", this.NVendeLugar.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza ApresentacaoSetor
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacaoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacaoSetor SET SetorID = @001, ApresentacaoID = @002, VersaoImagemIngresso = @003, VersaoImagemVale = @004, VersaoImagemVale2 = @005, VersaoImagemVale3 = @006, IngressosGerados = '@007', CotaID = @008, Quantidade = @009, QuantidadePorCliente = @010, PrincipalPrecoID = @011, NVendeLugar = '@012' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);
                sql.Replace("@012", this.NVendeLugar.ValorBD);

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
        /// Atualiza ApresentacaoSetor
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacaoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacaoSetor SET SetorID = @001, ApresentacaoID = @002, VersaoImagemIngresso = @003, VersaoImagemVale = @004, VersaoImagemVale2 = @005, VersaoImagemVale3 = @006, IngressosGerados = '@007', CotaID = @008, Quantidade = @009, QuantidadePorCliente = @010, PrincipalPrecoID = @011, NVendeLugar = '@012' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);
                sql.Replace("@012", this.NVendeLugar.ValorBD);

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
        /// Exclui ApresentacaoSetor com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cApresentacaoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tApresentacaoSetor WHERE ID=" + id;

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
        /// Exclui ApresentacaoSetor com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cApresentacaoSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tApresentacaoSetor WHERE ID=" + id;

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
        /// Exclui ApresentacaoSetor
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

            this.SetorID.Limpar();
            this.ApresentacaoID.Limpar();
            this.VersaoImagemIngresso.Limpar();
            this.VersaoImagemVale.Limpar();
            this.VersaoImagemVale2.Limpar();
            this.VersaoImagemVale3.Limpar();
            this.IngressosGerados.Limpar();
            this.CotaID.Limpar();
            this.Quantidade.Limpar();
            this.QuantidadePorCliente.Limpar();
            this.PrincipalPrecoID.Limpar();
            this.NVendeLugar.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.SetorID.Desfazer();
            this.ApresentacaoID.Desfazer();
            this.VersaoImagemIngresso.Desfazer();
            this.VersaoImagemVale.Desfazer();
            this.VersaoImagemVale2.Desfazer();
            this.VersaoImagemVale3.Desfazer();
            this.IngressosGerados.Desfazer();
            this.CotaID.Desfazer();
            this.Quantidade.Desfazer();
            this.QuantidadePorCliente.Desfazer();
            this.PrincipalPrecoID.Desfazer();
            this.NVendeLugar.Desfazer();
        }

        public class setorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SetorID";
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

        public class apresentacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoID";
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

        public class versaoimagemingresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemIngresso";
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

        public class versaoimagemvale : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale";
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

        public class versaoimagemvale2 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale2";
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

        public class versaoimagemvale3 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale3";
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

        public class ingressosgerados : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "IngressosGerados";
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

        public class principalprecoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrincipalPrecoID";
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

        public class nvendelugar : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "NVendeLugar";
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

                DataTable tabela = new DataTable("ApresentacaoSetor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("IngressosGerados", typeof(bool));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("PrincipalPrecoID", typeof(int));
                tabela.Columns.Add("NVendeLugar", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract int ApresentacaoSetorID(int apresentacaoid, int setorid);

        public abstract string Mapa();

        public abstract string StatusLugares();

        public abstract DataTable Setores(int apresentacaoid);

        public abstract DataTable SetoresMarcados(int apresentacaoid);

        public abstract DataTable SetoresNaoMarcados(int apresentacaoid);

        public abstract DataTable Apresentacoes(int setorid);

        public abstract DataTable Precos();

        public abstract DataTable Precos(int apresentacaoid, int setorid);

        public abstract DataTable PrecosPorCanal(int canalid, int apresentacaoid, int setorid);

        public abstract bool ExcluirCascata();

        public abstract DataTable VendaItem(string apresentacaosetorids);

        public abstract DataTable PorcentagemIngressosStatus();

        public abstract DataTable QuantidadeIngressosStatus();

        public abstract int TotalIngressos();

    }
    #endregion

    #region "ApresentacaoSetorLista_B"

    public abstract class ApresentacaoSetorLista_B : BaseLista
    {

        private bool backup = false;
        protected ApresentacaoSetor apresentacaoSetor;

        // passar o Usuario logado no sistema
        public ApresentacaoSetorLista_B()
        {
            apresentacaoSetor = new ApresentacaoSetor();
        }

        // passar o Usuario logado no sistema
        public ApresentacaoSetorLista_B(int usuarioIDLogado)
        {
            apresentacaoSetor = new ApresentacaoSetor(usuarioIDLogado);
        }

        public ApresentacaoSetor ApresentacaoSetor
        {
            get { return apresentacaoSetor; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ApresentacaoSetor especifico
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
                    apresentacaoSetor.Ler(id);
                    return apresentacaoSetor;
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
                    sql = "SELECT ID FROM tApresentacaoSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tApresentacaoSetor";

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
                    sql = "SELECT ID FROM tApresentacaoSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tApresentacaoSetor";

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
                    sql = "SELECT ID FROM xApresentacaoSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xApresentacaoSetor";

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
        /// Preenche ApresentacaoSetor corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    apresentacaoSetor.Ler(id);
                else
                    apresentacaoSetor.LerBackup(id);

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

                bool ok = apresentacaoSetor.Excluir();
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
        /// Inseri novo(a) ApresentacaoSetor na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = apresentacaoSetor.Inserir();
                if (ok)
                {
                    lista.Add(apresentacaoSetor.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ApresentacaoSetor carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ApresentacaoSetor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("IngressosGerados", typeof(bool));
                tabela.Columns.Add("CotaID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("PrincipalPrecoID", typeof(int));
                tabela.Columns.Add("NVendeLugar", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = apresentacaoSetor.Control.ID;
                        linha["SetorID"] = apresentacaoSetor.SetorID.Valor;
                        linha["ApresentacaoID"] = apresentacaoSetor.ApresentacaoID.Valor;
                        linha["VersaoImagemIngresso"] = apresentacaoSetor.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = apresentacaoSetor.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = apresentacaoSetor.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = apresentacaoSetor.VersaoImagemVale3.Valor;
                        linha["IngressosGerados"] = apresentacaoSetor.IngressosGerados.Valor;
                        linha["CotaID"] = apresentacaoSetor.CotaID.Valor;
                        linha["Quantidade"] = apresentacaoSetor.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = apresentacaoSetor.QuantidadePorCliente.Valor;
                        linha["PrincipalPrecoID"] = apresentacaoSetor.PrincipalPrecoID.Valor;
                        linha["NVendeLugar"] = apresentacaoSetor.NVendeLugar.Valor;
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

                DataTable tabela = new DataTable("RelatorioApresentacaoSetor");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("ApresentacaoID", typeof(int));
                    tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                    tabela.Columns.Add("IngressosGerados", typeof(bool));
                    tabela.Columns.Add("CotaID", typeof(int));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                    tabela.Columns.Add("PrincipalPrecoID", typeof(int));
                    tabela.Columns.Add("NVendeLugar", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["SetorID"] = apresentacaoSetor.SetorID.Valor;
                        linha["ApresentacaoID"] = apresentacaoSetor.ApresentacaoID.Valor;
                        linha["VersaoImagemIngresso"] = apresentacaoSetor.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = apresentacaoSetor.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = apresentacaoSetor.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = apresentacaoSetor.VersaoImagemVale3.Valor;
                        linha["IngressosGerados"] = apresentacaoSetor.IngressosGerados.Valor;
                        linha["CotaID"] = apresentacaoSetor.CotaID.Valor;
                        linha["Quantidade"] = apresentacaoSetor.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = apresentacaoSetor.QuantidadePorCliente.Valor;
                        linha["PrincipalPrecoID"] = apresentacaoSetor.PrincipalPrecoID.Valor;
                        linha["NVendeLugar"] = apresentacaoSetor.NVendeLugar.Valor;
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
                    case "SetorID":
                        sql = "SELECT ID, SetorID FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "ApresentacaoID":
                        sql = "SELECT ID, ApresentacaoID FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY ApresentacaoID";
                        break;
                    case "VersaoImagemIngresso":
                        sql = "SELECT ID, VersaoImagemIngresso FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY VersaoImagemIngresso";
                        break;
                    case "VersaoImagemVale":
                        sql = "SELECT ID, VersaoImagemVale FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale";
                        break;
                    case "VersaoImagemVale2":
                        sql = "SELECT ID, VersaoImagemVale2 FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale2";
                        break;
                    case "VersaoImagemVale3":
                        sql = "SELECT ID, VersaoImagemVale3 FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale3";
                        break;
                    case "IngressosGerados":
                        sql = "SELECT ID, IngressosGerados FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY IngressosGerados";
                        break;
                    case "CotaID":
                        sql = "SELECT ID, CotaID FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY CotaID";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "QuantidadePorCliente":
                        sql = "SELECT ID, QuantidadePorCliente FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY QuantidadePorCliente";
                        break;
                    case "PrincipalPrecoID":
                        sql = "SELECT ID, PrincipalPrecoID FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY PrincipalPrecoID";
                        break;
                    case "NVendeLugar":
                        sql = "SELECT ID, NVendeLugar FROM tApresentacaoSetor WHERE " + FiltroSQL + " ORDER BY NVendeLugar";
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

    #region "ApresentacaoSetorException"

    [Serializable]
    public class ApresentacaoSetorException : Exception
    {

        public ApresentacaoSetorException() : base() { }

        public ApresentacaoSetorException(string msg) : base(msg) { }

        public ApresentacaoSetorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}