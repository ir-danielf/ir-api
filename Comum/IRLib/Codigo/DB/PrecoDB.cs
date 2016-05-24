/******************************************************
* Arquivo PrecoDB.cs
* Gerado em: 08/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Preco_B"

    public abstract class Preco_B : BaseBD
    {

        public nome Nome = new nome();
        public corid CorID = new corid();
        public precotipoid PrecoTipoID = new precotipoid();
        public valor Valor = new valor();
        public apresentacaosetorid ApresentacaoSetorID = new apresentacaosetorid();
        public quantidade Quantidade = new quantidade();
        public quantidadeporcliente QuantidadePorCliente = new quantidadeporcliente();
        public impressao Impressao = new impressao();
        public irvende IRVende = new irvende();
        public imprimircarimbo ImprimirCarimbo = new imprimircarimbo();
        public carimbotexto1 CarimboTexto1 = new carimbotexto1();
        public carimbotexto2 CarimboTexto2 = new carimbotexto2();
        public codigocinema CodigoCinema = new codigocinema();
        public parceiromidiaid ParceiroMidiaID = new parceiromidiaid();
        public loteid LoteID = new loteid();

        public Preco_B() { }

        // passar o Usuario logado no sistema
        public Preco_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Preco
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tPreco WHERE ID = " + id;
                using (bd.Consulta(sql))
                {
                    if (bd.Consulta().Read())
                    {
                        this.Control.ID = id;
                        this.Nome.ValorBD = bd.LerString("Nome");
                        this.CorID.ValorBD = bd.LerInt("CorID").ToString();
                        this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                        this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                        this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                        this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                        this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                        this.Impressao.ValorBD = bd.LerString("Impressao");
                        this.IRVende.ValorBD = bd.LerString("IRVende");
                        this.ImprimirCarimbo.ValorBD = bd.LerString("ImprimirCarimbo");
                        this.CarimboTexto1.ValorBD = bd.LerString("CarimboTexto1");
                        this.CarimboTexto2.ValorBD = bd.LerString("CarimboTexto2");
                        this.CodigoCinema.ValorBD = bd.LerString("CodigoCinema");
                        this.ParceiroMidiaID.ValorBD = bd.LerInt("ParceiroMidiaID").ToString();
                        this.LoteID.ValorBD = bd.LerInt("LoteID").ToString();
                    }
                    else
                        this.Limpar();

                    bd.Fechar();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche todos os atributos de Preco do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xPreco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CorID.ValorBD = bd.LerInt("CorID").ToString();
                    this.PrecoTipoID.ValorBD = bd.LerInt("PrecoTipoID").ToString();
                    this.Valor.ValorBD = bd.LerDecimal("Valor").ToString();
                    this.ApresentacaoSetorID.ValorBD = bd.LerInt("ApresentacaoSetorID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.Impressao.ValorBD = bd.LerString("Impressao");
                    this.IRVende.ValorBD = bd.LerString("IRVende");
                    this.ImprimirCarimbo.ValorBD = bd.LerString("ImprimirCarimbo");
                    this.CarimboTexto1.ValorBD = bd.LerString("CarimboTexto1");
                    this.CarimboTexto2.ValorBD = bd.LerString("CarimboTexto2");
                    this.CodigoCinema.ValorBD = bd.LerString("CodigoCinema");
                    this.ParceiroMidiaID.ValorBD = bd.LerInt("LerInt").ToString();
                    this.LoteID.ValorBD = bd.LerInt("LoteID").ToString();
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
                sql.Append("INSERT INTO cPreco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xPreco (ID, Versao, Nome, CorID, PrecoTipoID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende, ImprimirCarimbo, CarimboTexto1, CarimboTexto2, CodigoCinema, ParceiroMidiaID, LoteID) ");
                sql.Append("SELECT ID, @V, Nome, CorID, PrecoTipoID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende, ImprimirCarimbo, CarimboTexto1, CarimboTexto2, CodigoCinema, ParceiroMidiaID, LoteID FROM tPreco WHERE ID = @I");
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
        /// Inserir novo(a) Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPreco");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPreco(ID, Nome, CorID, PrecoTipoID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende, ImprimirCarimbo, CarimboTexto1, CarimboTexto2, CodigoCinema, ParceiroMidiaID, LoteID) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,'@004',@005,@006,@007,'@008','@009','@010','@011','@012','@014', @015, @016)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Valor.ValorBD);
                sql.Replace("@005", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@006", this.Quantidade.ValorBD);
                sql.Replace("@007", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@008", this.Impressao.ValorBD);
                sql.Replace("@009", this.IRVende.ValorBD);
                sql.Replace("@010", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@011", this.CarimboTexto1.ValorBD);
                sql.Replace("@012", this.CarimboTexto2.ValorBD);
                sql.Replace("@014", this.CodigoCinema.ValorBD);
                sql.Replace("@015", this.ParceiroMidiaID.ValorBD);
                sql.Replace("@016", this.LoteID.ValorBD);

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
        /// Inserir novo(a) Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPreco");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPreco(ID, Nome, CorID, PrecoTipoID, Valor, ApresentacaoSetorID, Quantidade, QuantidadePorCliente, Impressao, IRVende, ImprimirCarimbo, CarimboTexto1, CarimboTexto2, CodigoCinema, ParceiroMidiaID, LoteID) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,'@004',@005,@006,@007,'@008','@009','@010','@011','@012','@014', @015, @016)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Valor.ValorBD);
                sql.Replace("@005", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@006", this.Quantidade.ValorBD);
                sql.Replace("@007", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@008", this.Impressao.ValorBD);
                sql.Replace("@009", this.IRVende.ValorBD);
                sql.Replace("@010", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@011", this.CarimboTexto1.ValorBD);
                sql.Replace("@012", this.CarimboTexto2.ValorBD);
                sql.Replace("@014", this.CodigoCinema.ValorBD);
                sql.Replace("@015", this.ParceiroMidiaID.ValorBD);
                sql.Replace("@016", this.LoteID.ValorBD);

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
        /// Atualiza Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                string LoteID;

                if (this.LoteID == null || this.LoteID.Valor == 0)
                {
                    LoteID = "null";
                    this.LoteID = new loteid();
                    this.LoteID.Valor = 0;
                }
                else
                    LoteID = this.LoteID.ValorBD;

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPreco SET Nome = '@001', CorID = @002, PrecoTipoID = @003, Valor = '@004', ApresentacaoSetorID = @005, Quantidade = @006, QuantidadePorCliente = @007, Impressao = '@008', IRVende = '@009', ImprimirCarimbo = '@010', CarimboTexto1 = '@011', CarimboTexto2 = '@012', CodigoCinema = '@014', ParceiroMidiaID = @015, LoteID = @016 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.Valor == 0 ? "NULL" : this.CorID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.Valor == 0 ? "NULL" : this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Valor.ValorBD);
                sql.Replace("@005", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@006", this.Quantidade.ValorBD);
                sql.Replace("@007", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@008", this.Impressao.ValorBD);
                sql.Replace("@009", this.IRVende.ValorBD);
                sql.Replace("@010", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@011", this.CarimboTexto1.ValorBD);
                sql.Replace("@012", this.CarimboTexto2.ValorBD);
                sql.Replace("@014", this.CodigoCinema.ValorBD);
                sql.Replace("@015", this.ParceiroMidiaID.Valor == 0 ? "NULL" : this.ParceiroMidiaID.ValorBD);
                sql.Replace("@016", LoteID);

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
        /// Atualiza Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {
            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                string LoteID;

                if (this.LoteID == null)
                {
                    LoteID = "null";
                    this.LoteID = new loteid();
                    this.LoteID.Valor = 0;
                }
                else
                    LoteID = this.LoteID.ValorBD;

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPreco SET Nome = '@001', CorID = @002, PrecoTipoID = @003, Valor = '@004', ApresentacaoSetorID = @005, Quantidade = @006, QuantidadePorCliente = @007, Impressao = '@008', IRVende = '@009', ImprimirCarimbo = '@010', CarimboTexto1 = '@011', CarimboTexto2 = '@012', CodigoCinema = '@014', ParceiroMidiaID = @015, LoteID = @016 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.CorID.ValorBD);
                sql.Replace("@003", this.PrecoTipoID.ValorBD);
                sql.Replace("@004", this.Valor.ValorBD);
                sql.Replace("@005", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@006", this.Quantidade.ValorBD);
                sql.Replace("@007", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@008", this.Impressao.ValorBD);
                sql.Replace("@009", this.IRVende.ValorBD);
                sql.Replace("@010", this.ImprimirCarimbo.ValorBD);
                sql.Replace("@011", this.CarimboTexto1.ValorBD);
                sql.Replace("@012", this.CarimboTexto2.ValorBD);
                sql.Replace("@014", this.CodigoCinema.ValorBD);
                sql.Replace("@015", this.ParceiroMidiaID.Valor == 0 ? "NULL" : this.ParceiroMidiaID.ValorBD);
                sql.Replace("@016", LoteID);

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
        /// Exclui Preco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {
            try
            {
                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPreco WHERE ID=" + id;

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
        /// Exclui Preco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cPreco WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tPreco WHERE ID=" + id;

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
        /// Exclui Preco
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
            this.CorID.Limpar();
            this.PrecoTipoID.Limpar();
            this.Valor.Limpar();
            this.ApresentacaoSetorID.Limpar();
            this.Quantidade.Limpar();
            this.QuantidadePorCliente.Limpar();
            this.Impressao.Limpar();
            this.IRVende.Limpar();
            this.ImprimirCarimbo.Limpar();
            this.CarimboTexto1.Limpar();
            this.CarimboTexto2.Limpar();
            this.CodigoCinema.Limpar();
            this.ParceiroMidiaID.Limpar();
            this.LoteID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.CorID.Desfazer();
            this.PrecoTipoID.Desfazer();
            this.Valor.Desfazer();
            this.ApresentacaoSetorID.Desfazer();
            this.Quantidade.Desfazer();
            this.QuantidadePorCliente.Desfazer();
            this.Impressao.Desfazer();
            this.IRVende.Desfazer();
            this.ImprimirCarimbo.Desfazer();
            this.CarimboTexto1.Desfazer();
            this.CarimboTexto2.Desfazer();
            this.CodigoCinema.Desfazer();
            this.ParceiroMidiaID.Desfazer();
            this.LoteID.Desfazer();
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

        public class corid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CorID";
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

        public class precotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrecoTipoID";
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

        public class apresentacaosetorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ApresentacaoSetorID";
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

        public class impressao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Impressao";
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

        public class irvende : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "IRVende";
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

        public class imprimircarimbo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImprimirCarimbo";
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

        public class carimbotexto1 : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CarimboTexto1";
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

        public class carimbotexto2 : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CarimboTexto2";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 40;
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

        public class codigocinema : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoCinema";
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

        public class parceiromidiaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ParceiroMidiaID";
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

        public class loteid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LoteID";
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

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("Impressao", typeof(string));
                tabela.Columns.Add("IRVende", typeof(bool));
                tabela.Columns.Add("ImprimirCarimbo", typeof(bool));
                tabela.Columns.Add("CarimboTexto1", typeof(string));
                tabela.Columns.Add("CarimboTexto2", typeof(string));
                tabela.Columns.Add("CodigoCinema", typeof(string));
                tabela.Columns.Add("ParceiroMidiaID", typeof(int));
                tabela.Columns.Add("LoteID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract int QuantidadeVendido();

        public abstract bool AtualizarCanaisIR();

        public abstract DataTable Precos(string registrozero, int canalid, int apresentacaosetorid);

        public abstract DataTable Canais();

        public abstract bool ExcluirCascata();

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorPreco(string ingressologids);

        public abstract decimal ValorIngressosPorPreco(string ingressologids);

    }
    #endregion

    #region "PrecoLista_B"

    public abstract class PrecoLista_B : BaseLista
    {

        private bool backup = false;
        protected Preco preco;

        // passar o Usuario logado no sistema
        public PrecoLista_B()
        {
            preco = new Preco();
        }

        // passar o Usuario logado no sistema
        public PrecoLista_B(int usuarioIDLogado)
        {
            preco = new Preco(usuarioIDLogado);
        }

        public Preco Preco
        {
            get { return preco; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Preco especifico
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
                    preco.Ler(id);
                    return preco;
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
                    sql = "SELECT ID FROM tPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPreco";

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
                    sql = "SELECT ID FROM tPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tPreco";

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
                    sql = "SELECT ID FROM xPreco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xPreco";

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
        /// Preenche Preco corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    preco.Ler(id);
                else
                    preco.LerBackup(id);

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

                bool ok = preco.Excluir();
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
        /// Inseri novo(a) Preco na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = preco.Inserir();
                if (ok)
                {
                    lista.Add(preco.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Preco carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("PrecoTipoID", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("Impressao", typeof(string));
                tabela.Columns.Add("IRVende", typeof(bool));
                tabela.Columns.Add("ImprimirCarimbo", typeof(bool));
                tabela.Columns.Add("CarimboTexto1", typeof(string));
                tabela.Columns.Add("CarimboTexto2", typeof(string));
                tabela.Columns.Add("CodigoCinema", typeof(string));
                tabela.Columns.Add("ParceiroMidiaID", typeof(int));
                tabela.Columns.Add("LoteID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = preco.Control.ID;
                        linha["Nome"] = preco.Nome.Valor;
                        linha["CorID"] = preco.CorID.Valor;
                        linha["PrecoTipoID"] = preco.PrecoTipoID.Valor;
                        linha["Valor"] = preco.Valor.Valor;
                        linha["ApresentacaoSetorID"] = preco.ApresentacaoSetorID.Valor;
                        linha["Quantidade"] = preco.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = preco.QuantidadePorCliente.Valor;
                        linha["Impressao"] = preco.Impressao.Valor;
                        linha["IRVende"] = preco.IRVende;
                        linha["ImprimirCarimbo"] = preco.ImprimirCarimbo.Valor;
                        linha["CarimboTexto1"] = preco.CarimboTexto1.Valor;
                        linha["CarimboTexto2"] = preco.CarimboTexto2.Valor;
                        linha["CodigoCinema"] = preco.CodigoCinema.Valor;
                        linha["ParceiroMidiaID"] = preco.ParceiroMidiaID.Valor;
                        linha["LoteID"] = preco.LoteID.Valor;
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

                DataTable tabela = new DataTable("RelatorioPreco");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("CorID", typeof(int));
                    tabela.Columns.Add("PrecoTipoID", typeof(int));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                    tabela.Columns.Add("Impressao", typeof(string));
                    tabela.Columns.Add("IRVende", typeof(bool));
                    tabela.Columns.Add("ImprimirCarimbo", typeof(bool));
                    tabela.Columns.Add("CarimboTexto1", typeof(string));
                    tabela.Columns.Add("CarimboTexto2", typeof(string));
                    tabela.Columns.Add("CodigoCinema", typeof(string));
                    tabela.Columns.Add("ParceiroMidiaID", typeof(int));
                    tabela.Columns.Add("LoteID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = preco.Nome.Valor;
                        linha["CorID"] = preco.CorID.Valor;
                        linha["PrecoTipoID"] = preco.PrecoTipoID.Valor;
                        linha["Valor"] = preco.Valor.Valor;
                        linha["ApresentacaoSetorID"] = preco.ApresentacaoSetorID.Valor;
                        linha["Quantidade"] = preco.Quantidade.Valor;
                        linha["QuantidadePorCliente"] = preco.QuantidadePorCliente.Valor;
                        linha["Impressao"] = preco.Impressao.Valor;
                        linha["IRVende"] = preco.IRVende;
                        linha["ImprimirCarimbo"] = preco.ImprimirCarimbo.Valor;
                        linha["CarimboTexto1"] = preco.CarimboTexto1.Valor;
                        linha["CarimboTexto2"] = preco.CarimboTexto2.Valor;
                        linha["CodigoCinema"] = preco.CodigoCinema.Valor;
                        linha["ParceiroMidiaID"] = preco.ParceiroMidiaID.Valor;
                        linha["LoteID"] = preco.LoteID.Valor;
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
                        sql = "SELECT ID, Nome FROM tPreco WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "CorID":
                        sql = "SELECT ID, CorID FROM tPreco WHERE " + FiltroSQL + " ORDER BY CorID";
                        break;
                    case "PrecoTipoID":
                        sql = "SELECT ID, PrecoTipoID FROM tPreco WHERE " + FiltroSQL + " ORDER BY PrecoTipoID";
                        break;
                    case "Valor":
                        sql = "SELECT ID, Valor FROM tPreco WHERE " + FiltroSQL + " ORDER BY Valor";
                        break;
                    case "ApresentacaoSetorID":
                        sql = "SELECT ID, ApresentacaoSetorID FROM tPreco WHERE " + FiltroSQL + " ORDER BY ApresentacaoSetorID";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tPreco WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "QuantidadePorCliente":
                        sql = "SELECT ID, QuantidadePorCliente FROM tPreco WHERE " + FiltroSQL + " ORDER BY QuantidadePorCliente";
                        break;
                    case "Impressao":
                        sql = "SELECT ID, Impressao FROM tPreco WHERE " + FiltroSQL + " ORDER BY Impressao";
                        break;
                    case "IRVende":
                        sql = "SELECT ID, IRVende FROM tPreco WHERE " + FiltroSQL + " ORDER BY IRVende";
                        break;
                    case "ImprimirCarimbo":
                        sql = "SELECT ID, ImprimirCarimbo FROM tPreco WHERE " + FiltroSQL + " ORDER BY ImprimirCarimbo";
                        break;
                    case "CarimboTexto1":
                        sql = "SELECT ID, CarimboTexto1 FROM tPreco WHERE " + FiltroSQL + " ORDER BY CarimboTexto1";
                        break;
                    case "CarimboTexto2":
                        sql = "SELECT ID, CarimboTexto2 FROM tPreco WHERE " + FiltroSQL + " ORDER BY CarimboTexto2";
                        break;
                    case "CodigoCinema":
                        sql = "SELECT ID, CodigoCinema FROM tPreco WHERE " + FiltroSQL + " ORDER BY CodigoCinema";
                        break;
                    case "ParceiroMidiaID":
                        sql = "SELECT ID, ParceiroMidiaID FROM tPreco WHERE " + FiltroSQL + " ORDER BY ParceiroMidiaID";
                        break;
                    case "LoteID":
                        sql = "SELECT ID, LoteID FROM tPreco WHERE " + FiltroSQL + " ORDER BY LoteID";
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

    #region "PrecoException"

    [Serializable]
    public class PrecoException : Exception
    {

        public PrecoException() : base() { }

        public PrecoException(string msg) : base(msg) { }

        public PrecoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}