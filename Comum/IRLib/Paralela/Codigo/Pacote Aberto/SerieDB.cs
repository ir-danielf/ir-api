/******************************************************
* Arquivo SerieDB.cs
* Gerado em: 13/12/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Serie_B"

    public abstract class Serie_B : BaseBD
    {

        public titulo Titulo = new titulo();
        public nome Nome = new nome();
        public regionalid RegionalID = new regionalid();
        public localid LocalID = new localid();
        public quantidademinimagrupo QuantidadeMinimaGrupo = new quantidademinimagrupo();
        public quantidademaximagrupo QuantidadeMaximaGrupo = new quantidademaximagrupo();
        public quantidademinimaapresentacao QuantidadeMinimaApresentacao = new quantidademinimaapresentacao();
        public quantidademaximaapresentacao QuantidadeMaximaApresentacao = new quantidademaximaapresentacao();
        public quantidademinimaingressosporapresentacao QuantidadeMinimaIngressosPorApresentacao = new quantidademinimaingressosporapresentacao();
        public quantidademaximaingressosporapresentacao QuantidadeMaximaIngressosPorApresentacao = new quantidademaximaingressosporapresentacao();
        public descricao Descricao = new descricao();
        public regras Regras = new regras();
        public tipo Tipo = new tipo();

        public Serie_B() { }

        // passar o Usuario logado no sistema
        public Serie_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Serie
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tSerie WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Titulo.ValorBD = bd.LerString("Titulo");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RegionalID.ValorBD = bd.LerInt("RegionalID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.QuantidadeMinimaGrupo.ValorBD = bd.LerInt("QuantidadeMinimaGrupo").ToString();
                    this.QuantidadeMaximaGrupo.ValorBD = bd.LerInt("QuantidadeMaximaGrupo").ToString();
                    this.QuantidadeMinimaApresentacao.ValorBD = bd.LerInt("QuantidadeMinimaApresentacao").ToString();
                    this.QuantidadeMaximaApresentacao.ValorBD = bd.LerInt("QuantidadeMaximaApresentacao").ToString();
                    this.QuantidadeMinimaIngressosPorApresentacao.ValorBD = bd.LerInt("QuantidadeMinimaIngressosPorApresentacao").ToString();
                    this.QuantidadeMaximaIngressosPorApresentacao.ValorBD = bd.LerInt("QuantidadeMaximaIngressosPorApresentacao").ToString();
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.Regras.ValorBD = bd.LerString("Regras");
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

        }

        /// <summary>
        /// Preenche todos os atributos de Serie do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xSerie WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Titulo.ValorBD = bd.LerString("Titulo");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RegionalID.ValorBD = bd.LerInt("RegionalID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.QuantidadeMinimaGrupo.ValorBD = bd.LerInt("QuantidadeMinimaGrupo").ToString();
                    this.QuantidadeMaximaGrupo.ValorBD = bd.LerInt("QuantidadeMaximaGrupo").ToString();
                    this.QuantidadeMinimaApresentacao.ValorBD = bd.LerInt("QuantidadeMinimaApresentacao").ToString();
                    this.QuantidadeMaximaApresentacao.ValorBD = bd.LerInt("QuantidadeMaximaApresentacao").ToString();
                    this.QuantidadeMinimaIngressosPorApresentacao.ValorBD = bd.LerInt("QuantidadeMinimaIngressosPorApresentacao").ToString();
                    this.QuantidadeMaximaIngressosPorApresentacao.ValorBD = bd.LerInt("QuantidadeMaximaIngressosPorApresentacao").ToString();
                    this.Descricao.ValorBD = bd.LerString("Descricao");
                    this.Regras.ValorBD = bd.LerString("Regras");
                    this.Tipo.ValorBD = bd.LerString("Tipo");
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
                sql.Append("INSERT INTO cSerie (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xSerie (ID, Versao, Titulo, Nome, RegionalID, LocalID, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao, Descricao, Regras, Tipo) ");
                sql.Append("SELECT ID, @V, Titulo, Nome, RegionalID, LocalID, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao, Descricao, Regras, Tipo FROM tSerie WHERE ID = @I");
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
        /// Inserir novo(a) Serie
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cSerie");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tSerie(ID, Titulo, Nome, RegionalID, LocalID, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao, Descricao, Regras, Tipo) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Titulo.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.RegionalID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.QuantidadeMinimaGrupo.ValorBD);
                sql.Replace("@006", this.QuantidadeMaximaGrupo.ValorBD);
                sql.Replace("@007", this.QuantidadeMinimaApresentacao.ValorBD);
                sql.Replace("@008", this.QuantidadeMaximaApresentacao.ValorBD);
                sql.Replace("@009", this.QuantidadeMinimaIngressosPorApresentacao.ValorBD);
                sql.Replace("@010", this.QuantidadeMaximaIngressosPorApresentacao.ValorBD);
                sql.Replace("@011", this.Descricao.ValorBD);
                sql.Replace("@012", this.Regras.ValorBD);
                sql.Replace("@013", this.Tipo.ValorBD);

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
        /// Inserir novo(a) Serie
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cSerie");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tSerie(ID, Titulo, Nome, RegionalID, LocalID, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao, Descricao, Regras, Tipo) ");
                sql.Append("VALUES (@ID,'@001','@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011','@012','@013')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Titulo.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.RegionalID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.QuantidadeMinimaGrupo.ValorBD);
                sql.Replace("@006", this.QuantidadeMaximaGrupo.ValorBD);
                sql.Replace("@007", this.QuantidadeMinimaApresentacao.ValorBD);
                sql.Replace("@008", this.QuantidadeMaximaApresentacao.ValorBD);
                sql.Replace("@009", this.QuantidadeMinimaIngressosPorApresentacao.ValorBD);
                sql.Replace("@010", this.QuantidadeMaximaIngressosPorApresentacao.ValorBD);
                sql.Replace("@011", this.Descricao.ValorBD);
                sql.Replace("@012", this.Regras.ValorBD);
                sql.Replace("@013", this.Tipo.ValorBD);

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
        /// Atualiza Serie
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cSerie WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tSerie SET Titulo = '@001', Nome = '@002', RegionalID = @003, LocalID = @004, QuantidadeMinimaGrupo = @005, QuantidadeMaximaGrupo = @006, QuantidadeMinimaApresentacao = @007, QuantidadeMaximaApresentacao = @008, QuantidadeMinimaIngressosPorApresentacao = @009, QuantidadeMaximaIngressosPorApresentacao = @010, Descricao = '@011', Regras = '@012', Tipo = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Titulo.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.RegionalID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.QuantidadeMinimaGrupo.ValorBD);
                sql.Replace("@006", this.QuantidadeMaximaGrupo.ValorBD);
                sql.Replace("@007", this.QuantidadeMinimaApresentacao.ValorBD);
                sql.Replace("@008", this.QuantidadeMaximaApresentacao.ValorBD);
                sql.Replace("@009", this.QuantidadeMinimaIngressosPorApresentacao.ValorBD);
                sql.Replace("@010", this.QuantidadeMaximaIngressosPorApresentacao.ValorBD);
                sql.Replace("@011", this.Descricao.ValorBD);
                sql.Replace("@012", this.Regras.ValorBD);
                sql.Replace("@013", this.Tipo.ValorBD);

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
        /// Atualiza Serie
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cSerie WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tSerie SET Titulo = '@001', Nome = '@002', RegionalID = @003, LocalID = @004, QuantidadeMinimaGrupo = @005, QuantidadeMaximaGrupo = @006, QuantidadeMinimaApresentacao = @007, QuantidadeMaximaApresentacao = @008, QuantidadeMinimaIngressosPorApresentacao = @009, QuantidadeMaximaIngressosPorApresentacao = @010, Descricao = '@011', Regras = '@012', Tipo = '@013' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Titulo.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.RegionalID.ValorBD);
                sql.Replace("@004", this.LocalID.ValorBD);
                sql.Replace("@005", this.QuantidadeMinimaGrupo.ValorBD);
                sql.Replace("@006", this.QuantidadeMaximaGrupo.ValorBD);
                sql.Replace("@007", this.QuantidadeMinimaApresentacao.ValorBD);
                sql.Replace("@008", this.QuantidadeMaximaApresentacao.ValorBD);
                sql.Replace("@009", this.QuantidadeMinimaIngressosPorApresentacao.ValorBD);
                sql.Replace("@010", this.QuantidadeMaximaIngressosPorApresentacao.ValorBD);
                sql.Replace("@011", this.Descricao.ValorBD);
                sql.Replace("@012", this.Regras.ValorBD);
                sql.Replace("@013", this.Tipo.ValorBD);

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
        /// Exclui Serie com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cSerie WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tSerie WHERE ID=" + id;

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
        /// Exclui Serie com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cSerie WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tSerie WHERE ID=" + id;

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
        /// Exclui Serie
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

            this.Titulo.Limpar();
            this.Nome.Limpar();
            this.RegionalID.Limpar();
            this.LocalID.Limpar();
            this.QuantidadeMinimaGrupo.Limpar();
            this.QuantidadeMaximaGrupo.Limpar();
            this.QuantidadeMinimaApresentacao.Limpar();
            this.QuantidadeMaximaApresentacao.Limpar();
            this.QuantidadeMinimaIngressosPorApresentacao.Limpar();
            this.QuantidadeMaximaIngressosPorApresentacao.Limpar();
            this.Descricao.Limpar();
            this.Regras.Limpar();
            this.Tipo.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Titulo.Desfazer();
            this.Nome.Desfazer();
            this.RegionalID.Desfazer();
            this.LocalID.Desfazer();
            this.QuantidadeMinimaGrupo.Desfazer();
            this.QuantidadeMaximaGrupo.Desfazer();
            this.QuantidadeMinimaApresentacao.Desfazer();
            this.QuantidadeMaximaApresentacao.Desfazer();
            this.QuantidadeMinimaIngressosPorApresentacao.Desfazer();
            this.QuantidadeMaximaIngressosPorApresentacao.Desfazer();
            this.Descricao.Desfazer();
            this.Regras.Desfazer();
            this.Tipo.Desfazer();
        }

        public class titulo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Titulo";
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
                    return 50;
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

        public class regionalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "RegionalID";
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

        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
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

        public class quantidademinimagrupo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMinimaGrupo";
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

        public class quantidademaximagrupo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMaximaGrupo";
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

        public class quantidademinimaapresentacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMinimaApresentacao";
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

        public class quantidademaximaapresentacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMaximaApresentacao";
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

        public class quantidademinimaingressosporapresentacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMinimaIngressosPorApresentacao";
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

        public class quantidademaximaingressosporapresentacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "QuantidadeMaximaIngressosPorApresentacao";
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
                    return 300;
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

        public class regras : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Regras";
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

                DataTable tabela = new DataTable("Serie");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Titulo", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RegionalID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaGrupo", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaGrupo", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaIngressosPorApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaIngressosPorApresentacao", typeof(int));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("Regras", typeof(string));
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

    #region "SerieLista_B"

    public abstract class SerieLista_B : BaseLista
    {

        private bool backup = false;
        protected Serie serie;

        // passar o Usuario logado no sistema
        public SerieLista_B()
        {
            serie = new Serie();
        }

        // passar o Usuario logado no sistema
        public SerieLista_B(int usuarioIDLogado)
        {
            serie = new Serie(usuarioIDLogado);
        }

        public Serie Serie
        {
            get { return serie; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Serie especifico
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
                    serie.Ler(id);
                    return serie;
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
                    sql = "SELECT ID FROM tSerie";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tSerie";

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
                    sql = "SELECT ID FROM tSerie";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tSerie";

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
                    sql = "SELECT ID FROM xSerie";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xSerie";

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
        /// Preenche Serie corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    serie.Ler(id);
                else
                    serie.LerBackup(id);

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

                bool ok = serie.Excluir();
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
        /// Inseri novo(a) Serie na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = serie.Inserir();
                if (ok)
                {
                    lista.Add(serie.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Serie carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Serie");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Titulo", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RegionalID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaGrupo", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaGrupo", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMinimaIngressosPorApresentacao", typeof(int));
                tabela.Columns.Add("QuantidadeMaximaIngressosPorApresentacao", typeof(int));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("Regras", typeof(string));
                tabela.Columns.Add("Tipo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = serie.Control.ID;
                        linha["Titulo"] = serie.Titulo.Valor;
                        linha["Nome"] = serie.Nome.Valor;
                        linha["RegionalID"] = serie.RegionalID.Valor;
                        linha["LocalID"] = serie.LocalID.Valor;
                        linha["QuantidadeMinimaGrupo"] = serie.QuantidadeMinimaGrupo.Valor;
                        linha["QuantidadeMaximaGrupo"] = serie.QuantidadeMaximaGrupo.Valor;
                        linha["QuantidadeMinimaApresentacao"] = serie.QuantidadeMinimaApresentacao.Valor;
                        linha["QuantidadeMaximaApresentacao"] = serie.QuantidadeMaximaApresentacao.Valor;
                        linha["QuantidadeMinimaIngressosPorApresentacao"] = serie.QuantidadeMinimaIngressosPorApresentacao.Valor;
                        linha["QuantidadeMaximaIngressosPorApresentacao"] = serie.QuantidadeMaximaIngressosPorApresentacao.Valor;
                        linha["Descricao"] = serie.Descricao.Valor;
                        linha["Regras"] = serie.Regras.Valor;
                        linha["Tipo"] = serie.Tipo.Valor;
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

                DataTable tabela = new DataTable("RelatorioSerie");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Titulo", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("RegionalID", typeof(int));
                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("QuantidadeMinimaGrupo", typeof(int));
                    tabela.Columns.Add("QuantidadeMaximaGrupo", typeof(int));
                    tabela.Columns.Add("QuantidadeMinimaApresentacao", typeof(int));
                    tabela.Columns.Add("QuantidadeMaximaApresentacao", typeof(int));
                    tabela.Columns.Add("QuantidadeMinimaIngressosPorApresentacao", typeof(int));
                    tabela.Columns.Add("QuantidadeMaximaIngressosPorApresentacao", typeof(int));
                    tabela.Columns.Add("Descricao", typeof(string));
                    tabela.Columns.Add("Regras", typeof(string));
                    tabela.Columns.Add("Tipo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Titulo"] = serie.Titulo.Valor;
                        linha["Nome"] = serie.Nome.Valor;
                        linha["RegionalID"] = serie.RegionalID.Valor;
                        linha["LocalID"] = serie.LocalID.Valor;
                        linha["QuantidadeMinimaGrupo"] = serie.QuantidadeMinimaGrupo.Valor;
                        linha["QuantidadeMaximaGrupo"] = serie.QuantidadeMaximaGrupo.Valor;
                        linha["QuantidadeMinimaApresentacao"] = serie.QuantidadeMinimaApresentacao.Valor;
                        linha["QuantidadeMaximaApresentacao"] = serie.QuantidadeMaximaApresentacao.Valor;
                        linha["QuantidadeMinimaIngressosPorApresentacao"] = serie.QuantidadeMinimaIngressosPorApresentacao.Valor;
                        linha["QuantidadeMaximaIngressosPorApresentacao"] = serie.QuantidadeMaximaIngressosPorApresentacao.Valor;
                        linha["Descricao"] = serie.Descricao.Valor;
                        linha["Regras"] = serie.Regras.Valor;
                        linha["Tipo"] = serie.Tipo.Valor;
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
                    case "Titulo":
                        sql = "SELECT ID, Titulo FROM tSerie WHERE " + FiltroSQL + " ORDER BY Titulo";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tSerie WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "RegionalID":
                        sql = "SELECT ID, RegionalID FROM tSerie WHERE " + FiltroSQL + " ORDER BY RegionalID";
                        break;
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tSerie WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "QuantidadeMinimaGrupo":
                        sql = "SELECT ID, QuantidadeMinimaGrupo FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMinimaGrupo";
                        break;
                    case "QuantidadeMaximaGrupo":
                        sql = "SELECT ID, QuantidadeMaximaGrupo FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMaximaGrupo";
                        break;
                    case "QuantidadeMinimaApresentacao":
                        sql = "SELECT ID, QuantidadeMinimaApresentacao FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMinimaApresentacao";
                        break;
                    case "QuantidadeMaximaApresentacao":
                        sql = "SELECT ID, QuantidadeMaximaApresentacao FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMaximaApresentacao";
                        break;
                    case "QuantidadeMinimaIngressosPorApresentacao":
                        sql = "SELECT ID, QuantidadeMinimaIngressosPorApresentacao FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMinimaIngressosPorApresentacao";
                        break;
                    case "QuantidadeMaximaIngressosPorApresentacao":
                        sql = "SELECT ID, QuantidadeMaximaIngressosPorApresentacao FROM tSerie WHERE " + FiltroSQL + " ORDER BY QuantidadeMaximaIngressosPorApresentacao";
                        break;
                    case "Descricao":
                        sql = "SELECT ID, Descricao FROM tSerie WHERE " + FiltroSQL + " ORDER BY Descricao";
                        break;
                    case "Regras":
                        sql = "SELECT ID, Regras FROM tSerie WHERE " + FiltroSQL + " ORDER BY Regras";
                        break;
                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tSerie WHERE " + FiltroSQL + " ORDER BY Tipo";
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

    #region "SerieException"

    [Serializable]
    public class SerieException : Exception
    {

        public SerieException() : base() { }

        public SerieException(string msg) : base(msg) { }

        public SerieException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}