/******************************************************
* Arquivo LugarDB.cs
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

    #region "Lugar_B"

    public abstract class Lugar_B : BaseBD
    {

        public setorid SetorID = new setorid();
        public codigo Codigo = new codigo();
        public quantidade Quantidade = new quantidade();
        public quantidadebloqueada QuantidadeBloqueada = new quantidadebloqueada();
        public posicaox PosicaoX = new posicaox();
        public posicaoy PosicaoY = new posicaoy();
        public simbolo Simbolo = new simbolo();
        public bloqueioid BloqueioID = new bloqueioid();
        public classificacao Classificacao = new classificacao();
        public grupo Grupo = new grupo();
        public obs Obs = new obs();
        public perspectivalugarid PerspectivaLugarID = new perspectivalugarid();
        public codigocinema CodigoCinema = new codigocinema();

        public Lugar_B() { }

        // passar o Usuario logado no sistema
        public Lugar_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Lugar
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tLugar WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadeBloqueada.ValorBD = bd.LerInt("QuantidadeBloqueada").ToString();
                    this.PosicaoX.ValorBD = bd.LerInt("PosicaoX").ToString();
                    this.PosicaoY.ValorBD = bd.LerInt("PosicaoY").ToString();
                    this.Simbolo.ValorBD = bd.LerInt("Simbolo").ToString();
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.Classificacao.ValorBD = bd.LerInt("Classificacao").ToString();
                    this.Grupo.ValorBD = bd.LerInt("Grupo").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.PerspectivaLugarID.ValorBD = bd.LerInt("PerspectivaLugarID").ToString();
                    this.CodigoCinema.ValorBD = bd.LerString("CodigoCinema");
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
        /// Preenche todos os atributos de Lugar do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xLugar WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.Codigo.ValorBD = bd.LerString("Codigo");
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadeBloqueada.ValorBD = bd.LerInt("QuantidadeBloqueada").ToString();
                    this.PosicaoX.ValorBD = bd.LerInt("PosicaoX").ToString();
                    this.PosicaoY.ValorBD = bd.LerInt("PosicaoY").ToString();
                    this.Simbolo.ValorBD = bd.LerInt("Simbolo").ToString();
                    this.BloqueioID.ValorBD = bd.LerInt("BloqueioID").ToString();
                    this.Classificacao.ValorBD = bd.LerInt("Classificacao").ToString();
                    this.Grupo.ValorBD = bd.LerInt("Grupo").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.PerspectivaLugarID.ValorBD = bd.LerInt("PerspectivaLugarID").ToString();
                    this.CodigoCinema.ValorBD = bd.LerString("CodigoCinema");
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
                sql.Append("INSERT INTO cLugar (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xLugar (ID, Versao, SetorID, Codigo, Quantidade, QuantidadeBloqueada, PosicaoX, PosicaoY, Simbolo, BloqueioID, Classificacao, Grupo, Obs, PerspectivaLugarID, CodigoCinema) ");
                sql.Append("SELECT ID, @V, SetorID, Codigo, Quantidade, QuantidadeBloqueada, PosicaoX, PosicaoY, Simbolo, BloqueioID, Classificacao, Grupo, Obs, PerspectivaLugarID, CodigoCinema FROM tLugar WHERE ID = @I");
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
        /// Inserir novo(a) Lugar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLugar");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLugar(ID, SetorID, Codigo, Quantidade, QuantidadeBloqueada, PosicaoX, PosicaoY, Simbolo, BloqueioID, Classificacao, Grupo, Obs, PerspectivaLugarID, CodigoCinema) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011',@012,'@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadeBloqueada.ValorBD);
                sql.Replace("@005", this.PosicaoX.ValorBD);
                sql.Replace("@006", this.PosicaoY.ValorBD);
                sql.Replace("@007", this.Simbolo.ValorBD);
                sql.Replace("@008", this.BloqueioID.ValorBD);
                sql.Replace("@009", this.Classificacao.ValorBD);
                sql.Replace("@010", this.Grupo.ValorBD);
                sql.Replace("@011", this.Obs.ValorBD);
                sql.Replace("@012", this.PerspectivaLugarID.ValorBD);
                sql.Replace("@013", this.CodigoCinema.ValorBD);

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
        /// Inserir novo(a) Lugar
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLugar");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLugar(ID, SetorID, Codigo, Quantidade, QuantidadeBloqueada, PosicaoX, PosicaoY, Simbolo, BloqueioID, Classificacao, Grupo, Obs, PerspectivaLugarID, CodigoCinema) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011',@012,'@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadeBloqueada.ValorBD);
                sql.Replace("@005", this.PosicaoX.ValorBD);
                sql.Replace("@006", this.PosicaoY.ValorBD);
                sql.Replace("@007", this.Simbolo.ValorBD);
                sql.Replace("@008", this.BloqueioID.ValorBD);
                sql.Replace("@009", this.Classificacao.ValorBD);
                sql.Replace("@010", this.Grupo.ValorBD);
                sql.Replace("@011", this.Obs.ValorBD);
                sql.Replace("@012", this.PerspectivaLugarID.ValorBD);
                sql.Replace("@013", this.CodigoCinema.ValorBD);

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
        /// Atualiza Lugar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLugar SET SetorID = @001, Codigo = '@002', Quantidade = @003, QuantidadeBloqueada = @004, PosicaoX = @005, PosicaoY = @006, Simbolo = @007, BloqueioID = @008, Classificacao = @009, Grupo = @010, Obs = '@011', PerspectivaLugarID = @012, CodigoCinema = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadeBloqueada.ValorBD);
                sql.Replace("@005", this.PosicaoX.ValorBD);
                sql.Replace("@006", this.PosicaoY.ValorBD);
                sql.Replace("@007", this.Simbolo.ValorBD);
                sql.Replace("@008", this.BloqueioID.ValorBD);
                sql.Replace("@009", this.Classificacao.ValorBD);
                sql.Replace("@010", this.Grupo.ValorBD);
                sql.Replace("@011", this.Obs.ValorBD);
                sql.Replace("@012", this.PerspectivaLugarID.ValorBD);
                sql.Replace("@013", this.CodigoCinema.ValorBD);

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
        /// Atualiza Lugar
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLugar SET SetorID = @001, Codigo = '@002', Quantidade = @003, QuantidadeBloqueada = @004, PosicaoX = @005, PosicaoY = @006, Simbolo = @007, BloqueioID = @008, Classificacao = @009, Grupo = @010, Obs = '@011', PerspectivaLugarID = @012, CodigoCinema = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.Codigo.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadeBloqueada.ValorBD);
                sql.Replace("@005", this.PosicaoX.ValorBD);
                sql.Replace("@006", this.PosicaoY.ValorBD);
                sql.Replace("@007", this.Simbolo.ValorBD);
                sql.Replace("@008", this.BloqueioID.ValorBD);
                sql.Replace("@009", this.Classificacao.ValorBD);
                sql.Replace("@010", this.Grupo.ValorBD);
                sql.Replace("@011", this.Obs.ValorBD);
                sql.Replace("@012", this.PerspectivaLugarID.ValorBD);
                sql.Replace("@013", this.CodigoCinema.ValorBD);

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
        /// Exclui Lugar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLugar WHERE ID=" + id;

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
        /// Exclui Lugar com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLugar WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLugar WHERE ID=" + id;

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
        /// Exclui Lugar
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
            this.Codigo.Limpar();
            this.Quantidade.Limpar();
            this.QuantidadeBloqueada.Limpar();
            this.PosicaoX.Limpar();
            this.PosicaoY.Limpar();
            this.Simbolo.Limpar();
            this.BloqueioID.Limpar();
            this.Classificacao.Limpar();
            this.Grupo.Limpar();
            this.Obs.Limpar();
            this.PerspectivaLugarID.Limpar();
            this.CodigoCinema.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.SetorID.Desfazer();
            this.Codigo.Desfazer();
            this.Quantidade.Desfazer();
            this.QuantidadeBloqueada.Desfazer();
            this.PosicaoX.Desfazer();
            this.PosicaoY.Desfazer();
            this.Simbolo.Desfazer();
            this.BloqueioID.Desfazer();
            this.Classificacao.Desfazer();
            this.Grupo.Desfazer();
            this.Obs.Desfazer();
            this.PerspectivaLugarID.Desfazer();
            this.CodigoCinema.Desfazer();
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

        public class codigo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Codigo";
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

        public class quantidadebloqueada : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeBloqueada";
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

        public class posicaox : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PosicaoX";
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

        public class posicaoy : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PosicaoY";
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

        public class simbolo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Simbolo";
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

        public class bloqueioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "BloqueioID";
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

        public class classificacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Classificacao";
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

        public class grupo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Grupo";
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

        public class obs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Obs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
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

        public class perspectivalugarid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PerspectivaLugarID";
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

                DataTable tabela = new DataTable("Lugar");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadeBloqueada", typeof(int));
                tabela.Columns.Add("PosicaoX", typeof(int));
                tabela.Columns.Add("PosicaoY", typeof(int));
                tabela.Columns.Add("Simbolo", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("Classificacao", typeof(int));
                tabela.Columns.Add("Grupo", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("PerspectivaLugarID", typeof(int));
                tabela.Columns.Add("CodigoCinema", typeof(string));
                tabela.Columns.Add("PodeExcluir", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Ingressos();

        public abstract bool Bloquear(int bloqueioid);

        public abstract bool Desbloquear();

    }
    #endregion

    #region "LugarLista_B"

    public abstract class LugarLista_B : BaseLista
    {

        private bool backup = false;
        protected Lugar lugar;

        // passar o Usuario logado no sistema
        public LugarLista_B()
        {
            lugar = new Lugar();
        }

        // passar o Usuario logado no sistema
        public LugarLista_B(int usuarioIDLogado)
        {
            lugar = new Lugar(usuarioIDLogado);
        }

        public Lugar Lugar
        {
            get { return lugar; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Lugar especifico
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
                    lugar.Ler(id);
                    return lugar;
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
                    sql = "SELECT ID FROM tLugar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLugar";

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
                    sql = "SELECT ID FROM tLugar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLugar";

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
                    sql = "SELECT ID FROM xLugar";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xLugar";

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
        /// Preenche Lugar corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    lugar.Ler(id);
                else
                    lugar.LerBackup(id);

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

                bool ok = lugar.Excluir();
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
        /// Inseri novo(a) Lugar na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = lugar.Inserir();
                if (ok)
                {
                    lista.Add(lugar.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Lugar carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Lugar");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadeBloqueada", typeof(int));
                tabela.Columns.Add("PosicaoX", typeof(int));
                tabela.Columns.Add("PosicaoY", typeof(int));
                tabela.Columns.Add("Simbolo", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("Classificacao", typeof(int));
                tabela.Columns.Add("Grupo", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("PerspectivaLugarID", typeof(int));
                tabela.Columns.Add("CodigoCinema", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = lugar.Control.ID;
                        linha["SetorID"] = lugar.SetorID.Valor;
                        linha["Codigo"] = lugar.Codigo.Valor;
                        linha["Quantidade"] = lugar.Quantidade.Valor;
                        linha["QuantidadeBloqueada"] = lugar.QuantidadeBloqueada.Valor;
                        linha["PosicaoX"] = lugar.PosicaoX.Valor;
                        linha["PosicaoY"] = lugar.PosicaoY.Valor;
                        linha["Simbolo"] = lugar.Simbolo.Valor;
                        linha["BloqueioID"] = lugar.BloqueioID.Valor;
                        linha["Classificacao"] = lugar.Classificacao.Valor;
                        linha["Grupo"] = lugar.Grupo.Valor;
                        linha["Obs"] = lugar.Obs.Valor;
                        linha["PerspectivaLugarID"] = lugar.PerspectivaLugarID.Valor;
                        linha["CodigoCinema"] = lugar.CodigoCinema.Valor;
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

                DataTable tabela = new DataTable("RelatorioLugar");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("SetorID", typeof(int));
                    tabela.Columns.Add("Codigo", typeof(string));
                    tabela.Columns.Add("Quantidade", typeof(int));
                    tabela.Columns.Add("QuantidadeBloqueada", typeof(int));
                    tabela.Columns.Add("PosicaoX", typeof(int));
                    tabela.Columns.Add("PosicaoY", typeof(int));
                    tabela.Columns.Add("Simbolo", typeof(int));
                    tabela.Columns.Add("BloqueioID", typeof(int));
                    tabela.Columns.Add("Classificacao", typeof(int));
                    tabela.Columns.Add("Grupo", typeof(int));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("PerspectivaLugarID", typeof(int));
                    tabela.Columns.Add("CodigoCinema", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["SetorID"] = lugar.SetorID.Valor;
                        linha["Codigo"] = lugar.Codigo.Valor;
                        linha["Quantidade"] = lugar.Quantidade.Valor;
                        linha["QuantidadeBloqueada"] = lugar.QuantidadeBloqueada.Valor;
                        linha["PosicaoX"] = lugar.PosicaoX.Valor;
                        linha["PosicaoY"] = lugar.PosicaoY.Valor;
                        linha["Simbolo"] = lugar.Simbolo.Valor;
                        linha["BloqueioID"] = lugar.BloqueioID.Valor;
                        linha["Classificacao"] = lugar.Classificacao.Valor;
                        linha["Grupo"] = lugar.Grupo.Valor;
                        linha["Obs"] = lugar.Obs.Valor;
                        linha["PerspectivaLugarID"] = lugar.PerspectivaLugarID.Valor;
                        linha["CodigoCinema"] = lugar.CodigoCinema.Valor;
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
                        sql = "SELECT ID, SetorID FROM tLugar WHERE " + FiltroSQL + " ORDER BY SetorID";
                        break;
                    case "Codigo":
                        sql = "SELECT ID, Codigo FROM tLugar WHERE " + FiltroSQL + " ORDER BY Codigo";
                        break;
                    case "Quantidade":
                        sql = "SELECT ID, Quantidade FROM tLugar WHERE " + FiltroSQL + " ORDER BY Quantidade";
                        break;
                    case "QuantidadeBloqueada":
                        sql = "SELECT ID, QuantidadeBloqueada FROM tLugar WHERE " + FiltroSQL + " ORDER BY QuantidadeBloqueada";
                        break;
                    case "PosicaoX":
                        sql = "SELECT ID, PosicaoX FROM tLugar WHERE " + FiltroSQL + " ORDER BY PosicaoX";
                        break;
                    case "PosicaoY":
                        sql = "SELECT ID, PosicaoY FROM tLugar WHERE " + FiltroSQL + " ORDER BY PosicaoY";
                        break;
                    case "Simbolo":
                        sql = "SELECT ID, Simbolo FROM tLugar WHERE " + FiltroSQL + " ORDER BY Simbolo";
                        break;
                    case "BloqueioID":
                        sql = "SELECT ID, BloqueioID FROM tLugar WHERE " + FiltroSQL + " ORDER BY BloqueioID";
                        break;
                    case "Classificacao":
                        sql = "SELECT ID, Classificacao FROM tLugar WHERE " + FiltroSQL + " ORDER BY Classificacao";
                        break;
                    case "Grupo":
                        sql = "SELECT ID, Grupo FROM tLugar WHERE " + FiltroSQL + " ORDER BY Grupo";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tLugar WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "PerspectivaLugarID":
                        sql = "SELECT ID, PerspectivaLugarID FROM tLugar WHERE " + FiltroSQL + " ORDER BY PerspectivaLugarID";
                        break;
                    case "CodigoCinema":
                        sql = "SELECT ID, CodigoCinema FROM tLugar WHERE " + FiltroSQL + " ORDER BY CodigoCinema";
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

    #region "LugarException"

    [Serializable]
    public class LugarException : Exception
    {

        public LugarException() : base() { }

        public LugarException(string msg) : base(msg) { }

        public LugarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}