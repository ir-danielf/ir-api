/******************************************************
* Arquivo SetorDB.cs
* Gerado em: 09/03/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    #region "Setor_B"

    public abstract class Setor_B : BaseBD
    {

        public localid LocalID = new localid();
        public nome Nome = new nome();
        public acesso Acesso = new acesso();
        public nomeinterno NomeInterno = new nomeinterno();
        public lugarmarcado LugarMarcado = new lugarmarcado();
        public produto Produto = new produto();
        public observacaoimportante ObservacaoImportante = new observacaoimportante();
        public distanciapalco DistanciaPalco = new distanciapalco();
        public aprovadopublicacao AprovadoPublicacao = new aprovadopublicacao();
        public versaobackground VersaoBackground = new versaobackground();
        public codigosala CodigoSala = new codigosala();
        public capacidade Capacidade = new capacidade();
        public linhas Linhas = new linhas();
        public colunas Colunas = new colunas();

        public Setor_B() { }

        // passar o Usuario logado no sistema
        public Setor_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Setor
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tSetor WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Acesso.ValorBD = bd.LerString("Acesso");
                    this.NomeInterno.ValorBD = bd.LerString("NomeInterno");
                    this.LugarMarcado.ValorBD = bd.LerString("LugarMarcado");
                    this.Produto.ValorBD = bd.LerString("Produto");
                    this.ObservacaoImportante.ValorBD = bd.LerString("ObservacaoImportante");
                    this.DistanciaPalco.ValorBD = bd.LerInt("DistanciaPalco").ToString();
                    this.AprovadoPublicacao.ValorBD = bd.LerString("AprovadoPublicacao");
                    this.VersaoBackground.ValorBD = bd.LerInt("VersaoBackground").ToString();
                    this.CodigoSala.ValorBD = bd.LerString("CodigoSala");
                    this.Capacidade.ValorBD = bd.LerInt("Capacidade").ToString();
                    this.Linhas.ValorBD = bd.LerInt("Linhas").ToString();
                    this.Colunas.ValorBD = bd.LerInt("Colunas").ToString();
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
        /// Preenche todos os atributos de Setor do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xSetor WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Acesso.ValorBD = bd.LerString("Acesso");
                    this.NomeInterno.ValorBD = bd.LerString("NomeInterno");
                    this.LugarMarcado.ValorBD = bd.LerString("LugarMarcado");
                    this.Produto.ValorBD = bd.LerString("Produto");
                    this.ObservacaoImportante.ValorBD = bd.LerString("ObservacaoImportante");
                    this.DistanciaPalco.ValorBD = bd.LerInt("DistanciaPalco").ToString();
                    this.AprovadoPublicacao.ValorBD = bd.LerString("AprovadoPublicacao");
                    this.VersaoBackground.ValorBD = bd.LerInt("VersaoBackground").ToString();
                    this.CodigoSala.ValorBD = bd.LerString("CodigoSala");
                    this.Capacidade.ValorBD = bd.LerInt("Capacidade").ToString();
                    this.Linhas.ValorBD = bd.LerInt("Linhas").ToString();
                    this.Colunas.ValorBD = bd.LerInt("Colunas").ToString();
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
                sql.Append("INSERT INTO cSetor (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xSetor (ID, Versao, LocalID, Nome, Acesso, NomeInterno, LugarMarcado, Produto, ObservacaoImportante, DistanciaPalco, AprovadoPublicacao, VersaoBackground, CodigoSala, Capacidade, Linhas, Colunas) ");
                sql.Append("SELECT ID, @V, LocalID, Nome, Acesso, NomeInterno, LugarMarcado, Produto, ObservacaoImportante, DistanciaPalco, AprovadoPublicacao, VersaoBackground, CodigoSala, Capacidade, Linhas, Colunas FROM tSetor WHERE ID = @I");
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
        /// Inserir novo(a) Setor
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cSetor");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tSetor(ID, LocalID, Nome, Acesso, NomeInterno, LugarMarcado, Produto, ObservacaoImportante, DistanciaPalco, AprovadoPublicacao, VersaoBackground, CodigoSala, Capacidade, Linhas, Colunas) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007',@008,'@009',@010,'@011',@012,@013,@014)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Acesso.ValorBD);
                sql.Replace("@004", this.NomeInterno.ValorBD);
                sql.Replace("@005", this.LugarMarcado.ValorBD);
                sql.Replace("@006", this.Produto.ValorBD);
                sql.Replace("@007", this.ObservacaoImportante.ValorBD);
                sql.Replace("@008", this.DistanciaPalco.ValorBD);
                sql.Replace("@009", this.AprovadoPublicacao.ValorBD);
                sql.Replace("@010", this.VersaoBackground.ValorBD);
                sql.Replace("@011", this.CodigoSala.ValorBD);
                sql.Replace("@012", this.Capacidade.ValorBD);
                sql.Replace("@013", this.Linhas.ValorBD);
                sql.Replace("@014", this.Colunas.ValorBD);

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
        /// Inserir novo(a) Setor
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cSetor");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tSetor(ID, LocalID, Nome, Acesso, NomeInterno, LugarMarcado, Produto, ObservacaoImportante, DistanciaPalco, AprovadoPublicacao, VersaoBackground, CodigoSala, Capacidade, Linhas, Colunas) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007',@008,'@009',@010,'@011',@012,@013,@014)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Acesso.ValorBD);
                sql.Replace("@004", this.NomeInterno.ValorBD);
                sql.Replace("@005", this.LugarMarcado.ValorBD);
                sql.Replace("@006", this.Produto.ValorBD);
                sql.Replace("@007", this.ObservacaoImportante.ValorBD);
                sql.Replace("@008", this.DistanciaPalco.ValorBD);
                sql.Replace("@009", this.AprovadoPublicacao.ValorBD);
                sql.Replace("@010", this.VersaoBackground.ValorBD);
                sql.Replace("@011", this.CodigoSala.ValorBD);
                sql.Replace("@012", this.Capacidade.ValorBD);
                sql.Replace("@013", this.Linhas.ValorBD);
                sql.Replace("@014", this.Colunas.ValorBD);

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
        /// Atualiza Setor
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tSetor SET LocalID = @001, Nome = '@002', Acesso = '@003', NomeInterno = '@004', LugarMarcado = '@005', Produto = '@006', ObservacaoImportante = '@007', DistanciaPalco = @008, AprovadoPublicacao = '@009', VersaoBackground = @010, CodigoSala = '@011', Capacidade = @012, Linhas = @013, Colunas = @014 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Acesso.ValorBD);
                sql.Replace("@004", this.NomeInterno.ValorBD);
                sql.Replace("@005", this.LugarMarcado.ValorBD);
                sql.Replace("@006", this.Produto.ValorBD);
                sql.Replace("@007", this.ObservacaoImportante.ValorBD);
                sql.Replace("@008", this.DistanciaPalco.ValorBD);
                sql.Replace("@009", this.AprovadoPublicacao.ValorBD);
                sql.Replace("@010", this.VersaoBackground.ValorBD);
                sql.Replace("@011", this.CodigoSala.ValorBD);
                sql.Replace("@012", this.Capacidade.ValorBD);
                sql.Replace("@013", this.Linhas.ValorBD);
                sql.Replace("@014", this.Colunas.ValorBD);

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
        /// Atualiza Setor
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tSetor SET LocalID = @001, Nome = '@002', Acesso = '@003', NomeInterno = '@004', LugarMarcado = '@005', Produto = '@006', ObservacaoImportante = '@007', DistanciaPalco = @008, AprovadoPublicacao = '@009', VersaoBackground = @010, CodigoSala = '@011', Capacidade = @012, Linhas = @013, Colunas = @014 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.Acesso.ValorBD);
                sql.Replace("@004", this.NomeInterno.ValorBD);
                sql.Replace("@005", this.LugarMarcado.ValorBD);
                sql.Replace("@006", this.Produto.ValorBD);
                sql.Replace("@007", this.ObservacaoImportante.ValorBD);
                sql.Replace("@008", this.DistanciaPalco.ValorBD);
                sql.Replace("@009", this.AprovadoPublicacao.ValorBD);
                sql.Replace("@010", this.VersaoBackground.ValorBD);
                sql.Replace("@011", this.CodigoSala.ValorBD);
                sql.Replace("@012", this.Capacidade.ValorBD);
                sql.Replace("@013", this.Linhas.ValorBD);
                sql.Replace("@014", this.Colunas.ValorBD);

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
        /// Exclui Setor com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tSetor WHERE ID=" + id;

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
        /// Exclui Setor com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cSetor WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tSetor WHERE ID=" + id;

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
        /// Exclui Setor
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

            this.LocalID.Limpar();
            this.Nome.Limpar();
            this.Acesso.Limpar();
            this.NomeInterno.Limpar();
            this.LugarMarcado.Limpar();
            this.Produto.Limpar();
            this.ObservacaoImportante.Limpar();
            this.DistanciaPalco.Limpar();
            this.AprovadoPublicacao.Limpar();
            this.VersaoBackground.Limpar();
            this.CodigoSala.Limpar();
            this.Capacidade.Limpar();
            this.Linhas.Limpar();
            this.Colunas.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.LocalID.Desfazer();
            this.Nome.Desfazer();
            this.Acesso.Desfazer();
            this.NomeInterno.Desfazer();
            this.LugarMarcado.Desfazer();
            this.Produto.Desfazer();
            this.ObservacaoImportante.Desfazer();
            this.DistanciaPalco.Desfazer();
            this.AprovadoPublicacao.Desfazer();
            this.VersaoBackground.Desfazer();
            this.CodigoSala.Desfazer();
            this.Capacidade.Desfazer();
            this.Linhas.Desfazer();
            this.Colunas.Desfazer();
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

        public class acesso : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Acesso";
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

        public class nomeinterno : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeInterno";
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

        public class lugarmarcado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "LugarMarcado";
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

        public class produto : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Produto";
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

        public class observacaoimportante : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObservacaoImportante";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class distanciapalco : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DistanciaPalco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4;
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

        public class aprovadopublicacao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "AprovadoPublicacao";
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

        public class versaobackground : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoBackground";
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

        public class codigosala : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoSala";
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

        public class capacidade : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Capacidade";
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

        public class linhas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Linhas";
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

        public class colunas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Colunas";
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

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Acesso", typeof(string));
                tabela.Columns.Add("NomeInterno", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("ObservacaoImportante", typeof(string));
                tabela.Columns.Add("DistanciaPalco", typeof(int));
                tabela.Columns.Add("AprovadoPublicacao", typeof(bool));
                tabela.Columns.Add("VersaoBackground", typeof(int));
                tabela.Columns.Add("CodigoSala", typeof(string));
                tabela.Columns.Add("Capacidade", typeof(int));
                tabela.Columns.Add("Linhas", typeof(int));
                tabela.Columns.Add("Colunas", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract int[] ApresentacoesSetoresIDs();

        public abstract string Mapa();

        public abstract DataTable Lugares();

        public abstract int Quantidade();

        public abstract int Quantidade(int apresentacaoid);

        public abstract int QuantidadeBloqueado(int apresentacaoid);

        public abstract int QuantidadeDisponivel(int apresentacaoid);

        public abstract decimal PrimeiroPrecoDisponivel(int apresentacaoid);

        public abstract DataTable PorcentagemIngressosStatus(string apresentacoes);

        public abstract DataTable QuantidadeIngressosStatus(string apresentacoes);

        public abstract int TotalIngressos(string apresentacoes);

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorSetor(string ingressologids);

        public abstract decimal ValorIngressosPorSetor(string ingressologids);

        public abstract DataTable Todos();

    }
    #endregion

    #region "SetorLista_B"

    public abstract class SetorLista_B : BaseLista
    {

        private bool backup = false;
        protected Setor setor;

        // passar o Usuario logado no sistema
        public SetorLista_B()
        {
            setor = new Setor();
        }

        // passar o Usuario logado no sistema
        public SetorLista_B(int usuarioIDLogado)
        {
            setor = new Setor(usuarioIDLogado);
        }

        public Setor Setor
        {
            get { return setor; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Setor especifico
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
                    setor.Ler(id);
                    return setor;
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
                    sql = "SELECT ID FROM tSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tSetor";

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
                    sql = "SELECT ID FROM tSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tSetor";

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
                    sql = "SELECT ID FROM xSetor";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xSetor";

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
        /// Preenche Setor corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    setor.Ler(id);
                else
                    setor.LerBackup(id);

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

                bool ok = setor.Excluir();
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
        /// Inseri novo(a) Setor na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = setor.Inserir();
                if (ok)
                {
                    lista.Add(setor.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Setor carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Acesso", typeof(string));
                tabela.Columns.Add("NomeInterno", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("ObservacaoImportante", typeof(string));
                tabela.Columns.Add("DistanciaPalco", typeof(int));
                tabela.Columns.Add("AprovadoPublicacao", typeof(bool));
                tabela.Columns.Add("VersaoBackground", typeof(int));
                tabela.Columns.Add("CodigoSala", typeof(string));
                tabela.Columns.Add("Capacidade", typeof(int));
                tabela.Columns.Add("Linhas", typeof(int));
                tabela.Columns.Add("Colunas", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = setor.Control.ID;
                        linha["LocalID"] = setor.LocalID.Valor;
                        linha["Nome"] = setor.Nome.Valor;
                        linha["Acesso"] = setor.Acesso.Valor;
                        linha["NomeInterno"] = setor.NomeInterno.Valor;
                        linha["LugarMarcado"] = setor.LugarMarcado.Valor;
                        linha["Produto"] = setor.Produto.Valor;
                        linha["ObservacaoImportante"] = setor.ObservacaoImportante.Valor;
                        linha["DistanciaPalco"] = setor.DistanciaPalco.Valor;
                        linha["AprovadoPublicacao"] = setor.AprovadoPublicacao.Valor;
                        linha["VersaoBackground"] = setor.VersaoBackground.Valor;
                        linha["CodigoSala"] = setor.CodigoSala.Valor;
                        linha["Capacidade"] = setor.Capacidade.Valor;
                        linha["Linhas"] = setor.Linhas.Valor;
                        linha["Colunas"] = setor.Colunas.Valor;
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

                DataTable tabela = new DataTable("RelatorioSetor");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Acesso", typeof(string));
                    tabela.Columns.Add("NomeInterno", typeof(string));
                    tabela.Columns.Add("LugarMarcado", typeof(string));
                    tabela.Columns.Add("Produto", typeof(bool));
                    tabela.Columns.Add("ObservacaoImportante", typeof(string));
                    tabela.Columns.Add("DistanciaPalco", typeof(int));
                    tabela.Columns.Add("AprovadoPublicacao", typeof(bool));
                    tabela.Columns.Add("VersaoBackground", typeof(int));
                    tabela.Columns.Add("CodigoSala", typeof(string));
                    tabela.Columns.Add("Capacidade", typeof(int));
                    tabela.Columns.Add("Linhas", typeof(int));
                    tabela.Columns.Add("Colunas", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["LocalID"] = setor.LocalID.Valor;
                        linha["Nome"] = setor.Nome.Valor;
                        linha["Acesso"] = setor.Acesso.Valor;
                        linha["NomeInterno"] = setor.NomeInterno.Valor;
                        linha["LugarMarcado"] = setor.LugarMarcado.Valor;
                        linha["Produto"] = setor.Produto.Valor;
                        linha["ObservacaoImportante"] = setor.ObservacaoImportante.Valor;
                        linha["DistanciaPalco"] = setor.DistanciaPalco.Valor;
                        linha["AprovadoPublicacao"] = setor.AprovadoPublicacao.Valor;
                        linha["VersaoBackground"] = setor.VersaoBackground.Valor;
                        linha["CodigoSala"] = setor.CodigoSala.Valor;
                        linha["Capacidade"] = setor.Capacidade.Valor;
                        linha["Linhas"] = setor.Linhas.Valor;
                        linha["Colunas"] = setor.Colunas.Valor;
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
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tSetor WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tSetor WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Acesso":
                        sql = "SELECT ID, Acesso FROM tSetor WHERE " + FiltroSQL + " ORDER BY Acesso";
                        break;
                    case "NomeInterno":
                        sql = "SELECT ID, NomeInterno FROM tSetor WHERE " + FiltroSQL + " ORDER BY NomeInterno";
                        break;
                    case "LugarMarcado":
                        sql = "SELECT ID, LugarMarcado FROM tSetor WHERE " + FiltroSQL + " ORDER BY LugarMarcado";
                        break;
                    case "Produto":
                        sql = "SELECT ID, Produto FROM tSetor WHERE " + FiltroSQL + " ORDER BY Produto";
                        break;
                    case "ObservacaoImportante":
                        sql = "SELECT ID, ObservacaoImportante FROM tSetor WHERE " + FiltroSQL + " ORDER BY ObservacaoImportante";
                        break;
                    case "DistanciaPalco":
                        sql = "SELECT ID, DistanciaPalco FROM tSetor WHERE " + FiltroSQL + " ORDER BY DistanciaPalco";
                        break;
                    case "AprovadoPublicacao":
                        sql = "SELECT ID, AprovadoPublicacao FROM tSetor WHERE " + FiltroSQL + " ORDER BY AprovadoPublicacao";
                        break;
                    case "VersaoBackground":
                        sql = "SELECT ID, VersaoBackground FROM tSetor WHERE " + FiltroSQL + " ORDER BY VersaoBackground";
                        break;
                    case "CodigoSala":
                        sql = "SELECT ID, CodigoSala FROM tSetor WHERE " + FiltroSQL + " ORDER BY CodigoSala";
                        break;
                    case "Capacidade":
                        sql = "SELECT ID, Capacidade FROM tSetor WHERE " + FiltroSQL + " ORDER BY Capacidade";
                        break;
                    case "Linhas":
                        sql = "SELECT ID, Linhas FROM tSetor WHERE " + FiltroSQL + " ORDER BY Linhas";
                        break;
                    case "Colunas":
                        sql = "SELECT ID, Colunas FROM tSetor WHERE " + FiltroSQL + " ORDER BY Colunas";
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

    #region "SetorException"

    [Serializable]
    public class SetorException : Exception
    {

        public SetorException() : base() { }

        public SetorException(string msg) : base(msg) { }

        public SetorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}