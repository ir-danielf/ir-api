

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "CorreioStatusEntrega_B"

    public abstract class CorreioStatusEntrega_B : BaseBD
    {


        public tipo Tipo = new tipo();

        public status Status = new status();

        public data Data = new data();

        public hora Hora = new hora();

        public descricao Descricao = new descricao();

        public local Local = new local();

        public codigo Codigo = new codigo();

        public cidade Cidade = new cidade();

        public uf Uf = new uf();

        public sto Sto = new sto();

        public codigorastreio CodigoRastreio = new codigorastreio();


        public CorreioStatusEntrega_B() { }

        // passar o Usuario logado no sistema
        public CorreioStatusEntrega_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de CorreioStatusEntrega
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCorreioStatusEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Tipo.ValorBD = bd.LerString("Tipo");

                    this.Status.ValorBD = bd.LerInt("Status").ToString();

                    this.Data.ValorBD = bd.LerString("Data");

                    this.Hora.ValorBD = bd.LerString("Hora");

                    this.Descricao.ValorBD = bd.LerString("Descricao");

                    this.Local.ValorBD = bd.LerString("Local");

                    this.Codigo.ValorBD = bd.LerString("Codigo");

                    this.Cidade.ValorBD = bd.LerString("Cidade");

                    this.Uf.ValorBD = bd.LerString("Uf");

                    this.Sto.ValorBD = bd.LerString("Sto");

                    this.CodigoRastreio.ValorBD = bd.LerString("CodigoRastreio");

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
        /// Preenche todos os atributos de CorreioStatusEntrega do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCorreioStatusEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Tipo.ValorBD = bd.LerString("Tipo");

                    this.Status.ValorBD = bd.LerInt("Status").ToString();

                    this.Data.ValorBD = bd.LerString("Data");

                    this.Hora.ValorBD = bd.LerString("Hora");

                    this.Descricao.ValorBD = bd.LerString("Descricao");

                    this.Local.ValorBD = bd.LerString("Local");

                    this.Codigo.ValorBD = bd.LerString("Codigo");

                    this.Cidade.ValorBD = bd.LerString("Cidade");

                    this.Uf.ValorBD = bd.LerString("Uf");

                    this.Sto.ValorBD = bd.LerString("Sto");

                    this.CodigoRastreio.ValorBD = bd.LerString("CodigoRastreio");

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
                sql.Append("INSERT INTO cCorreioStatusEntrega (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


                sql.Append("INSERT INTO xCorreioStatusEntrega (ID, Versao, Tipo, Status, Data, Hora, Descricao, Local, Codigo, Cidade, Uf, Sto, CodigoRastreio) ");
                sql.Append("SELECT ID, @V, Tipo, Status, Data, Hora, Descricao, Local, Codigo, Cidade, Uf, Sto, CodigoRastreio FROM tCorreioStatusEntrega WHERE ID = @I");
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
        /// Inserir novo(a) CorreioStatusEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCorreioStatusEntrega");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCorreioStatusEntrega(ID, Tipo, Status, Data, Hora, Descricao, Local, Codigo, Cidade, Uf, Sto, CodigoRastreio) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Tipo.ValorBD);

                sql.Replace("@002", this.Status.ValorBD);

                sql.Replace("@003", this.Data.ValorBD);

                sql.Replace("@004", this.Hora.ValorBD);

                sql.Replace("@005", this.Descricao.ValorBD);

                sql.Replace("@006", this.Local.ValorBD);

                sql.Replace("@007", this.Codigo.ValorBD);

                sql.Replace("@008", this.Cidade.ValorBD);

                sql.Replace("@009", this.Uf.ValorBD);

                sql.Replace("@010", this.Sto.ValorBD);

                sql.Replace("@011", this.CodigoRastreio.ValorBD);


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
        /// Inserir novo(a) CorreioStatusEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCorreioStatusEntrega");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCorreioStatusEntrega(ID, Tipo, Status, Data, Hora, Descricao, Local, Codigo, Cidade, Uf, Sto, CodigoRastreio) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011')");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Tipo.ValorBD);

                sql.Replace("@002", this.Status.ValorBD);

                sql.Replace("@003", this.Data.ValorBD);

                sql.Replace("@004", this.Hora.ValorBD);

                sql.Replace("@005", this.Descricao.ValorBD);

                sql.Replace("@006", this.Local.ValorBD);

                sql.Replace("@007", this.Codigo.ValorBD);

                sql.Replace("@008", this.Cidade.ValorBD);

                sql.Replace("@009", this.Uf.ValorBD);

                sql.Replace("@010", this.Sto.ValorBD);

                sql.Replace("@011", this.CodigoRastreio.ValorBD);


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
        /// Atualiza CorreioStatusEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCorreioStatusEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tCorreioStatusEntrega SET Tipo = '@001', Status = @002, Data = '@003', Hora = '@004', Descricao = '@005', Local = '@006', Codigo = '@007', Cidade = '@008', Uf = '@009', Sto = '@010', CodigoRastreio = '@011' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Tipo.ValorBD);

                sql.Replace("@002", this.Status.ValorBD);

                sql.Replace("@003", this.Data.ValorBD);

                sql.Replace("@004", this.Hora.ValorBD);

                sql.Replace("@005", this.Descricao.ValorBD);

                sql.Replace("@006", this.Local.ValorBD);

                sql.Replace("@007", this.Codigo.ValorBD);

                sql.Replace("@008", this.Cidade.ValorBD);

                sql.Replace("@009", this.Uf.ValorBD);

                sql.Replace("@010", this.Sto.ValorBD);

                sql.Replace("@011", this.CodigoRastreio.ValorBD);


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
        /// Atualiza CorreioStatusEntrega
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCorreioStatusEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tCorreioStatusEntrega SET Tipo = '@001', Status = @002, Data = '@003', Hora = '@004', Descricao = '@005', Local = '@006', Codigo = '@007', Cidade = '@008', Uf = '@009', Sto = '@010', CodigoRastreio = '@011' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Tipo.ValorBD);

                sql.Replace("@002", this.Status.ValorBD);

                sql.Replace("@003", this.Data.ValorBD);

                sql.Replace("@004", this.Hora.ValorBD);

                sql.Replace("@005", this.Descricao.ValorBD);

                sql.Replace("@006", this.Local.ValorBD);

                sql.Replace("@007", this.Codigo.ValorBD);

                sql.Replace("@008", this.Cidade.ValorBD);

                sql.Replace("@009", this.Uf.ValorBD);

                sql.Replace("@010", this.Sto.ValorBD);

                sql.Replace("@011", this.CodigoRastreio.ValorBD);


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
        /// Exclui CorreioStatusEntrega com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCorreioStatusEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCorreioStatusEntrega WHERE ID=" + id;

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
        /// Exclui CorreioStatusEntrega com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCorreioStatusEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCorreioStatusEntrega WHERE ID=" + id;

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
        /// Exclui CorreioStatusEntrega
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


            this.Tipo.Limpar();

            this.Status.Limpar();

            this.Data.Limpar();

            this.Hora.Limpar();

            this.Descricao.Limpar();

            this.Local.Limpar();

            this.Codigo.Limpar();

            this.Cidade.Limpar();

            this.Uf.Limpar();

            this.Sto.Limpar();

            this.CodigoRastreio.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.Tipo.Desfazer();

            this.Status.Desfazer();

            this.Data.Desfazer();

            this.Hora.Desfazer();

            this.Descricao.Desfazer();

            this.Local.Desfazer();

            this.Codigo.Desfazer();

            this.Cidade.Desfazer();

            this.Uf.Desfazer();

            this.Sto.Desfazer();

            this.CodigoRastreio.Desfazer();

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
                    return 5;
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


        public class status : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Status";
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


        public class data : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "Data";
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


        public class hora : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Hora";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 10;
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


        public class local : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Local";
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


        public class cidade : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cidade";
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


        public class uf : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Uf";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 4;
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


        public class sto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sto";
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


        public class codigorastreio : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoRastreio";
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

                DataTable tabela = new DataTable("CorreioStatusEntrega");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Tipo", typeof(string));

                tabela.Columns.Add("Status", typeof(int));

                tabela.Columns.Add("Data", typeof(DateTime));

                tabela.Columns.Add("Hora", typeof(string));

                tabela.Columns.Add("Descricao", typeof(string));

                tabela.Columns.Add("Local", typeof(string));

                tabela.Columns.Add("Codigo", typeof(string));

                tabela.Columns.Add("Cidade", typeof(string));

                tabela.Columns.Add("Uf", typeof(string));

                tabela.Columns.Add("Sto", typeof(string));

                tabela.Columns.Add("CodigoRastreio", typeof(string));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "CorreioStatusEntregaLista_B"


    public abstract class CorreioStatusEntregaLista_B : BaseLista
    {

        private bool backup = false;
        protected CorreioStatusEntrega correioStatusEntrega;

        // passar o Usuario logado no sistema
        public CorreioStatusEntregaLista_B()
        {
            correioStatusEntrega = new CorreioStatusEntrega();
        }

        // passar o Usuario logado no sistema
        public CorreioStatusEntregaLista_B(int usuarioIDLogado)
        {
            correioStatusEntrega = new CorreioStatusEntrega(usuarioIDLogado);
        }

        public CorreioStatusEntrega CorreioStatusEntrega
        {
            get { return correioStatusEntrega; }
        }

        /// <summary>
        /// Retorna um IBaseBD de CorreioStatusEntrega especifico
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
                    correioStatusEntrega.Ler(id);
                    return correioStatusEntrega;
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
                    sql = "SELECT ID FROM tCorreioStatusEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCorreioStatusEntrega";

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
                    sql = "SELECT ID FROM tCorreioStatusEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCorreioStatusEntrega";

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
                    sql = "SELECT ID FROM xCorreioStatusEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCorreioStatusEntrega";

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
        /// Preenche CorreioStatusEntrega corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    correioStatusEntrega.Ler(id);
                else
                    correioStatusEntrega.LerBackup(id);

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

                bool ok = correioStatusEntrega.Excluir();
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
        /// Inseri novo(a) CorreioStatusEntrega na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = correioStatusEntrega.Inserir();
                if (ok)
                {
                    lista.Add(correioStatusEntrega.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de CorreioStatusEntrega carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CorreioStatusEntrega");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Tipo", typeof(string));

                tabela.Columns.Add("Status", typeof(int));

                tabela.Columns.Add("Data", typeof(DateTime));

                tabela.Columns.Add("Hora", typeof(string));

                tabela.Columns.Add("Descricao", typeof(string));

                tabela.Columns.Add("Local", typeof(string));

                tabela.Columns.Add("Codigo", typeof(string));

                tabela.Columns.Add("Cidade", typeof(string));

                tabela.Columns.Add("Uf", typeof(string));

                tabela.Columns.Add("Sto", typeof(string));

                tabela.Columns.Add("CodigoRastreio", typeof(string));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = correioStatusEntrega.Control.ID;

                        linha["Tipo"] = correioStatusEntrega.Tipo.Valor;

                        linha["Status"] = correioStatusEntrega.Status.Valor;

                        linha["Data"] = correioStatusEntrega.Data.Valor;

                        linha["Hora"] = correioStatusEntrega.Hora.Valor;

                        linha["Descricao"] = correioStatusEntrega.Descricao.Valor;

                        linha["Local"] = correioStatusEntrega.Local.Valor;

                        linha["Codigo"] = correioStatusEntrega.Codigo.Valor;

                        linha["Cidade"] = correioStatusEntrega.Cidade.Valor;

                        linha["Uf"] = correioStatusEntrega.Uf.Valor;

                        linha["Sto"] = correioStatusEntrega.Sto.Valor;

                        linha["CodigoRastreio"] = correioStatusEntrega.CodigoRastreio.Valor;

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

                DataTable tabela = new DataTable("RelatorioCorreioStatusEntrega");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("Tipo", typeof(string));

                    tabela.Columns.Add("Status", typeof(int));

                    tabela.Columns.Add("Data", typeof(DateTime));

                    tabela.Columns.Add("Hora", typeof(string));

                    tabela.Columns.Add("Descricao", typeof(string));

                    tabela.Columns.Add("Local", typeof(string));

                    tabela.Columns.Add("Codigo", typeof(string));

                    tabela.Columns.Add("Cidade", typeof(string));

                    tabela.Columns.Add("Uf", typeof(string));

                    tabela.Columns.Add("Sto", typeof(string));

                    tabela.Columns.Add("CodigoRastreio", typeof(string));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["Tipo"] = correioStatusEntrega.Tipo.Valor;

                        linha["Status"] = correioStatusEntrega.Status.Valor;

                        linha["Data"] = correioStatusEntrega.Data.Valor;

                        linha["Hora"] = correioStatusEntrega.Hora.Valor;

                        linha["Descricao"] = correioStatusEntrega.Descricao.Valor;

                        linha["Local"] = correioStatusEntrega.Local.Valor;

                        linha["Codigo"] = correioStatusEntrega.Codigo.Valor;

                        linha["Cidade"] = correioStatusEntrega.Cidade.Valor;

                        linha["Uf"] = correioStatusEntrega.Uf.Valor;

                        linha["Sto"] = correioStatusEntrega.Sto.Valor;

                        linha["CodigoRastreio"] = correioStatusEntrega.CodigoRastreio.Valor;

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

                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Tipo";
                        break;

                    case "Status":
                        sql = "SELECT ID, Status FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Status";
                        break;

                    case "Data":
                        sql = "SELECT ID, Data FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Data";
                        break;

                    case "Hora":
                        sql = "SELECT ID, Hora FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Hora";
                        break;

                    case "Descricao":
                        sql = "SELECT ID, Descricao FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Descricao";
                        break;

                    case "Local":
                        sql = "SELECT ID, Local FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Local";
                        break;

                    case "Codigo":
                        sql = "SELECT ID, Codigo FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Codigo";
                        break;

                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;

                    case "Uf":
                        sql = "SELECT ID, Uf FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Uf";
                        break;

                    case "Sto":
                        sql = "SELECT ID, Sto FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY Sto";
                        break;

                    case "CodigoRastreio":
                        sql = "SELECT ID, CodigoRastreio FROM tCorreioStatusEntrega WHERE " + FiltroSQL + " ORDER BY CodigoRastreio";
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

    #region "CorreioStatusEntregaException"

    [Serializable]
    public class CorreioStatusEntregaException : Exception
    {

        public CorreioStatusEntregaException() : base() { }

        public CorreioStatusEntregaException(string msg) : base(msg) { }

        public CorreioStatusEntregaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}