/******************************************************
* Arquivo EstornoDB.cs
* Gerado em: 19/11/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "Estorno_B"

    public abstract class Estorno_B : BaseBD
    {

        public senhavenda SenhaVenda = new senhavenda();
        public senhacancelamento SenhaCancelamento = new senhacancelamento();
        public numerochamado NumeroChamado = new numerochamado();
        public valorestorno ValorEstorno = new valorestorno();
        public status Status = new status();
        public tipoestorno TipoEstorno = new tipoestorno();
        public datasolicitacao DataSolicitacao = new datasolicitacao();
        public dataprocessamento DataProcessamento = new dataprocessamento();
        public clienteid ClienteID = new clienteid();
        public motivoid MotivoID = new motivoid();

        public Estorno_B() { }

        // passar o Usuario logado no sistema
        public Estorno_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Estorno
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEstorno WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SenhaVenda.ValorBD = bd.LerString("SenhaVenda");
                    this.SenhaCancelamento.ValorBD = bd.LerString("SenhaCancelamento");
                    this.NumeroChamado.ValorBD = bd.LerString("NumeroChamado");
                    this.ValorEstorno.ValorBD = bd.LerDecimal("ValorEstorno").ToString();
                    this.Status.ValorBD = bd.LerString("Status");
                    this.TipoEstorno.ValorBD = bd.LerString("TipoEstorno");
                    this.DataSolicitacao.ValorBD = bd.LerString("DataSolicitacao");
                    this.DataProcessamento.ValorBD = bd.LerString("DataProcessamento");
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();
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
        /// Preenche todos os atributos de Estorno do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEstorno WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SenhaVenda.ValorBD = bd.LerString("SenhaVenda");
                    this.SenhaCancelamento.ValorBD = bd.LerString("SenhaCancelamento");
                    this.NumeroChamado.ValorBD = bd.LerString("NumeroChamado");
                    this.ValorEstorno.ValorBD = bd.LerDecimal("ValorEstorno").ToString();
                    this.Status.ValorBD = bd.LerString("Status");
                    this.TipoEstorno.ValorBD = bd.LerString("TipoEstorno");
                    this.DataSolicitacao.ValorBD = bd.LerString("DataSolicitacao");
                    this.DataProcessamento.ValorBD = bd.LerString("DataProcessamento");
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.MotivoID.ValorBD = bd.LerInt("MotivoID").ToString();
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
                sql.Append("INSERT INTO cEstorno (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEstorno (ID, Versao, SenhaVenda, SenhaCancelamento, NumeroChamado, ValorEstorno, Status, TipoEstorno, DataSolicitacao, DataProcessamento, ClienteID, MotivoID) ");
                sql.Append("SELECT ID, @V, SenhaVenda, SenhaCancelamento, NumeroChamado, ValorEstorno, Status, TipoEstorno, DataSolicitacao, DataProcessamento, ClienteID, MotivoID FROM tEstorno WHERE ID = @I");
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
        /// Inserir novo(a) Estorno
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstorno");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstorno(ID, SenhaVenda, SenhaCancelamento, NumeroChamado, ValorEstorno, Status, TipoEstorno, DataSolicitacao, DataProcessamento, ClienteID, MotivoID) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008',@009,@010)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SenhaVenda.ValorBD);
                sql.Replace("@002", this.SenhaCancelamento.ValorBD);
                sql.Replace("@003", this.NumeroChamado.ValorBD);
                sql.Replace("@004", this.ValorEstorno.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TipoEstorno.ValorBD);
                sql.Replace("@007", this.DataSolicitacao.ValorBD);
                sql.Replace("@008", this.DataProcessamento.ValorBD);
                sql.Replace("@009", this.ClienteID.ValorBD);
                sql.Replace("@010", this.MotivoID.ValorBD);

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
        /// Inserir novo(a) Estorno
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEstorno");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEstorno(ID, SenhaVenda, SenhaCancelamento, NumeroChamado, ValorEstorno, Status, TipoEstorno, DataSolicitacao, DataProcessamento, ClienteID, MotivoID) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008',@009,@010)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SenhaVenda.ValorBD);
                sql.Replace("@002", this.SenhaCancelamento.ValorBD);
                sql.Replace("@003", this.NumeroChamado.ValorBD);
                sql.Replace("@004", this.ValorEstorno.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TipoEstorno.ValorBD);
                sql.Replace("@007", this.DataSolicitacao.ValorBD);
                sql.Replace("@008", this.DataProcessamento.ValorBD);
                sql.Replace("@009", this.ClienteID.ValorBD);
                sql.Replace("@010", this.MotivoID.ValorBD);

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
        /// Atualiza Estorno
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEstorno WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstorno SET SenhaVenda = '@001', SenhaCancelamento = '@002', NumeroChamado = '@003', ValorEstorno = '@004', Status = '@005', TipoEstorno = '@006', DataSolicitacao = '@007', DataProcessamento = '@008', ClienteID = @009, MotivoID = @010 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SenhaVenda.ValorBD);
                sql.Replace("@002", this.SenhaCancelamento.ValorBD);
                sql.Replace("@003", this.NumeroChamado.ValorBD);
                sql.Replace("@004", this.ValorEstorno.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TipoEstorno.ValorBD);
                sql.Replace("@007", this.DataSolicitacao.ValorBD);
                sql.Replace("@008", this.DataProcessamento.ValorBD);
                sql.Replace("@009", this.ClienteID.ValorBD);
                sql.Replace("@010", this.MotivoID.ValorBD);

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
        /// Atualiza Estorno
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEstorno WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEstorno SET SenhaVenda = '@001', SenhaCancelamento = '@002', NumeroChamado = '@003', ValorEstorno = '@004', Status = '@005', TipoEstorno = '@006', DataSolicitacao = '@007', DataProcessamento = '@008', ClienteID = @009, MotivoID = @010 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SenhaVenda.ValorBD);
                sql.Replace("@002", this.SenhaCancelamento.ValorBD);
                sql.Replace("@003", this.NumeroChamado.ValorBD);
                sql.Replace("@004", this.ValorEstorno.ValorBD);
                sql.Replace("@005", this.Status.ValorBD);
                sql.Replace("@006", this.TipoEstorno.ValorBD);
                sql.Replace("@007", this.DataSolicitacao.ValorBD);
                sql.Replace("@008", this.DataProcessamento.ValorBD);
                sql.Replace("@009", this.ClienteID.ValorBD);
                sql.Replace("@010", this.MotivoID.ValorBD);

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
        /// Exclui Estorno com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstorno WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstorno WHERE ID=" + id;

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
        /// Exclui Estorno com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEstorno WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEstorno WHERE ID=" + id;

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
        /// Exclui Estorno
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

            this.SenhaVenda.Limpar();
            this.SenhaCancelamento.Limpar();
            this.NumeroChamado.Limpar();
            this.ValorEstorno.Limpar();
            this.Status.Limpar();
            this.TipoEstorno.Limpar();
            this.DataSolicitacao.Limpar();
            this.DataProcessamento.Limpar();
            this.ClienteID.Limpar();
            this.MotivoID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.SenhaVenda.Desfazer();
            this.SenhaCancelamento.Desfazer();
            this.NumeroChamado.Desfazer();
            this.ValorEstorno.Desfazer();
            this.Status.Desfazer();
            this.TipoEstorno.Desfazer();
            this.DataSolicitacao.Desfazer();
            this.DataProcessamento.Desfazer();
            this.ClienteID.Desfazer();
            this.MotivoID.Desfazer();
        }

        public class senhavenda : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SenhaVenda";
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

        public class senhacancelamento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "SenhaCancelamento";
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

        public class numerochamado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroChamado";
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

        public class valorestorno : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "ValorEstorno";
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

        public class status : TextProperty
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

        public class tipoestorno : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoEstorno";
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

        public class datasolicitacao : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataSolicitacao";
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

        public class dataprocessamento : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataProcessamento";
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

        public class motivoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoID";
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

                DataTable tabela = new DataTable("Estorno");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SenhaVenda", typeof(string));
                tabela.Columns.Add("SenhaCancelamento", typeof(string));
                tabela.Columns.Add("NumeroChamado", typeof(string));
                tabela.Columns.Add("ValorEstorno", typeof(decimal));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("TipoEstorno", typeof(string));
                tabela.Columns.Add("DataSolicitacao", typeof(DateTime));
                tabela.Columns.Add("DataProcessamento", typeof(DateTime));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("MotivoID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "EstornoLista_B"

    public abstract class EstornoLista_B : BaseLista
    {

        private bool backup = false;
        protected Estorno estorno;

        // passar o Usuario logado no sistema
        public EstornoLista_B()
        {
            estorno = new Estorno();
        }

        // passar o Usuario logado no sistema
        public EstornoLista_B(int usuarioIDLogado)
        {
            estorno = new Estorno(usuarioIDLogado);
        }

        public Estorno Estorno
        {
            get { return estorno; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Estorno especifico
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
                    estorno.Ler(id);
                    return estorno;
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
                    sql = "SELECT ID FROM tEstorno";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstorno";

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
                    sql = "SELECT ID FROM tEstorno";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEstorno";

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
                    sql = "SELECT ID FROM xEstorno";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEstorno";

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
        /// Preenche Estorno corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    estorno.Ler(id);
                else
                    estorno.LerBackup(id);

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

                bool ok = estorno.Excluir();
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
        /// Inseri novo(a) Estorno na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = estorno.Inserir();
                if (ok)
                {
                    lista.Add(estorno.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Estorno carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Estorno");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("SenhaVenda", typeof(string));
                tabela.Columns.Add("SenhaCancelamento", typeof(string));
                tabela.Columns.Add("NumeroChamado", typeof(string));
                tabela.Columns.Add("ValorEstorno", typeof(decimal));
                tabela.Columns.Add("Status", typeof(string));
                tabela.Columns.Add("TipoEstorno", typeof(string));
                tabela.Columns.Add("DataSolicitacao", typeof(DateTime));
                tabela.Columns.Add("DataProcessamento", typeof(DateTime));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("MotivoID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = estorno.Control.ID;
                        linha["SenhaVenda"] = estorno.SenhaVenda.Valor;
                        linha["SenhaCancelamento"] = estorno.SenhaCancelamento.Valor;
                        linha["NumeroChamado"] = estorno.NumeroChamado.Valor;
                        linha["ValorEstorno"] = estorno.ValorEstorno.Valor;
                        linha["Status"] = estorno.Status.Valor;
                        linha["TipoEstorno"] = estorno.TipoEstorno.Valor;
                        linha["DataSolicitacao"] = estorno.DataSolicitacao.Valor;
                        linha["DataProcessamento"] = estorno.DataProcessamento.Valor;
                        linha["ClienteID"] = estorno.ClienteID.Valor;
                        linha["MotivoID"] = estorno.MotivoID.Valor;
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

                DataTable tabela = new DataTable("RelatorioEstorno");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("SenhaVenda", typeof(string));
                    tabela.Columns.Add("SenhaCancelamento", typeof(string));
                    tabela.Columns.Add("NumeroChamado", typeof(string));
                    tabela.Columns.Add("ValorEstorno", typeof(decimal));
                    tabela.Columns.Add("Status", typeof(string));
                    tabela.Columns.Add("TipoEstorno", typeof(string));
                    tabela.Columns.Add("DataSolicitacao", typeof(DateTime));
                    tabela.Columns.Add("DataProcessamento", typeof(DateTime));
                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("MotivoID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["SenhaVenda"] = estorno.SenhaVenda.Valor;
                        linha["SenhaCancelamento"] = estorno.SenhaCancelamento.Valor;
                        linha["NumeroChamado"] = estorno.NumeroChamado.Valor;
                        linha["ValorEstorno"] = estorno.ValorEstorno.Valor;
                        linha["Status"] = estorno.Status.Valor;
                        linha["TipoEstorno"] = estorno.TipoEstorno.Valor;
                        linha["DataSolicitacao"] = estorno.DataSolicitacao.Valor;
                        linha["DataProcessamento"] = estorno.DataProcessamento.Valor;
                        linha["ClienteID"] = estorno.ClienteID.Valor;
                        linha["MotivoID"] = estorno.MotivoID.Valor;
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
                    case "SenhaVenda":
                        sql = "SELECT ID, SenhaVenda FROM tEstorno WHERE " + FiltroSQL + " ORDER BY SenhaVenda";
                        break;
                    case "SenhaCancelamento":
                        sql = "SELECT ID, SenhaCancelamento FROM tEstorno WHERE " + FiltroSQL + " ORDER BY SenhaCancelamento";
                        break;
                    case "NumeroChamado":
                        sql = "SELECT ID, NumeroChamado FROM tEstorno WHERE " + FiltroSQL + " ORDER BY NumeroChamado";
                        break;
                    case "ValorEstorno":
                        sql = "SELECT ID, ValorEstorno FROM tEstorno WHERE " + FiltroSQL + " ORDER BY ValorEstorno";
                        break;
                    case "Status":
                        sql = "SELECT ID, Status FROM tEstorno WHERE " + FiltroSQL + " ORDER BY Status";
                        break;
                    case "TipoEstorno":
                        sql = "SELECT ID, TipoEstorno FROM tEstorno WHERE " + FiltroSQL + " ORDER BY TipoEstorno";
                        break;
                    case "DataSolicitacao":
                        sql = "SELECT ID, DataSolicitacao FROM tEstorno WHERE " + FiltroSQL + " ORDER BY DataSolicitacao";
                        break;
                    case "DataProcessamento":
                        sql = "SELECT ID, DataProcessamento FROM tEstorno WHERE " + FiltroSQL + " ORDER BY DataProcessamento";
                        break;
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tEstorno WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "MotivoID":
                        sql = "SELECT ID, MotivoID FROM tEstorno WHERE " + FiltroSQL + " ORDER BY MotivoID";
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

    #region "EstornoException"

    [Serializable]
    public class EstornoException : Exception
    {

        public EstornoException() : base() { }

        public EstornoException(string msg) : base(msg) { }

        public EstornoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}