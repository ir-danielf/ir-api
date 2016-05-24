

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib.Paralela
{

    #region "Entrega_B"

    public abstract class Entrega_B : BaseBD
    {


        public nome Nome = new nome();

        public prazoentrega PrazoEntrega = new prazoentrega();

        public disponivel Disponivel = new disponivel();

        public procedimentoentrega ProcedimentoEntrega = new procedimentoentrega();

        public enviaalerta EnviaAlerta = new enviaalerta();

        public padrao Padrao = new padrao();

        public permitirimpressaointernet PermitirImpressaoInternet = new permitirimpressaointernet();

        public tipo Tipo = new tipo();

        public ativo Ativo = new ativo();

        public diastriagem DiasTriagem = new diastriagem();

        public de De = new de();

        public ate Ate = new ate();


        public Entrega_B() { }

        // passar o Usuario logado no sistema
        public Entrega_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Entrega
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.PrazoEntrega.ValorBD = bd.LerInt("PrazoEntrega").ToString();

                    this.Disponivel.ValorBD = bd.LerString("Disponivel");

                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");

                    this.EnviaAlerta.ValorBD = bd.LerString("EnviaAlerta");

                    this.Padrao.ValorBD = bd.LerString("Padrao");

                    this.PermitirImpressaoInternet.ValorBD = bd.LerString("PermitirImpressaoInternet");

                    this.Tipo.ValorBD = bd.LerString("Tipo");

                    this.Ativo.ValorBD = bd.LerString("Ativo");

                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();

                    this.De.ValorBD = bd.LerInt("De").ToString();

                    this.Ate.ValorBD = bd.LerInt("Ate").ToString();

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
        /// Preenche todos os atributos de Entrega do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEntrega WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;

                    this.Nome.ValorBD = bd.LerString("Nome");

                    this.PrazoEntrega.ValorBD = bd.LerInt("PrazoEntrega").ToString();

                    this.Disponivel.ValorBD = bd.LerString("Disponivel");

                    this.ProcedimentoEntrega.ValorBD = bd.LerString("ProcedimentoEntrega");

                    this.EnviaAlerta.ValorBD = bd.LerString("EnviaAlerta");

                    this.Padrao.ValorBD = bd.LerString("Padrao");

                    this.PermitirImpressaoInternet.ValorBD = bd.LerString("PermitirImpressaoInternet");

                    this.Tipo.ValorBD = bd.LerString("Tipo");

                    this.Ativo.ValorBD = bd.LerString("Ativo");

                    this.DiasTriagem.ValorBD = bd.LerInt("DiasTriagem").ToString();

                    this.De.ValorBD = bd.LerInt("De").ToString();

                    this.Ate.ValorBD = bd.LerInt("Ate").ToString();

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
                sql.Append("INSERT INTO cEntrega (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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


                sql.Append("INSERT INTO xEntrega (ID, Versao, Nome, PrazoEntrega, Disponivel, ProcedimentoEntrega, EnviaAlerta, Padrao, PermitirImpressaoInternet, Tipo, Ativo, DiasTriagem, De, Ate) ");
                sql.Append("SELECT ID, @V, Nome, PrazoEntrega, Disponivel, ProcedimentoEntrega, EnviaAlerta, Padrao, PermitirImpressaoInternet, Tipo, Ativo, DiasTriagem, De, Ate FROM tEntrega WHERE ID = @I");
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
        /// Inserir novo(a) Entrega
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEntrega");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEntrega(ID, Nome, PrazoEntrega, Disponivel, ProcedimentoEntrega, EnviaAlerta, Padrao, PermitirImpressaoInternet, Tipo, Ativo, DiasTriagem, De, Ate) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003','@004','@005','@006','@007','@008','@009',@010,@011,@012)");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.PrazoEntrega.ValorBD);

                sql.Replace("@003", this.Disponivel.ValorBD);

                sql.Replace("@004", this.ProcedimentoEntrega.ValorBD);

                sql.Replace("@005", this.EnviaAlerta.ValorBD);

                sql.Replace("@006", this.Padrao.ValorBD);

                sql.Replace("@007", this.PermitirImpressaoInternet.ValorBD);

                sql.Replace("@008", this.Tipo.ValorBD);

                sql.Replace("@009", this.Ativo.ValorBD);

                sql.Replace("@010", this.DiasTriagem.ValorBD);

                sql.Replace("@011", this.De.ValorBD);

                sql.Replace("@012", this.Ate.ValorBD);


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
        /// Inserir novo(a) Entrega
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEntrega");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEntrega(ID, Nome, PrazoEntrega, Disponivel, ProcedimentoEntrega, EnviaAlerta, Padrao, PermitirImpressaoInternet, Tipo, Ativo, DiasTriagem, De, Ate) ");
                sql.Append("VALUES (@ID,'@001',@002,'@003','@004','@005','@006','@007','@008','@009',@010,@011,@012)");

                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.PrazoEntrega.ValorBD);

                sql.Replace("@003", this.Disponivel.ValorBD);

                sql.Replace("@004", this.ProcedimentoEntrega.ValorBD);

                sql.Replace("@005", this.EnviaAlerta.ValorBD);

                sql.Replace("@006", this.Padrao.ValorBD);

                sql.Replace("@007", this.PermitirImpressaoInternet.ValorBD);

                sql.Replace("@008", this.Tipo.ValorBD);

                sql.Replace("@009", this.Ativo.ValorBD);

                sql.Replace("@010", this.DiasTriagem.ValorBD);

                sql.Replace("@011", this.De.ValorBD);

                sql.Replace("@012", this.Ate.ValorBD);


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
        /// Atualiza Entrega
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tEntrega SET Nome = '@001', PrazoEntrega = @002, Disponivel = '@003', ProcedimentoEntrega = '@004', EnviaAlerta = '@005', Padrao = '@006', PermitirImpressaoInternet = '@007', Tipo = '@008', Ativo = '@009', DiasTriagem = @010, De = @011, Ate = @012 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.PrazoEntrega.ValorBD);

                sql.Replace("@003", this.Disponivel.ValorBD);

                sql.Replace("@004", this.ProcedimentoEntrega.ValorBD);

                sql.Replace("@005", this.EnviaAlerta.ValorBD);

                sql.Replace("@006", this.Padrao.ValorBD);

                sql.Replace("@007", this.PermitirImpressaoInternet.ValorBD);

                sql.Replace("@008", this.Tipo.ValorBD);

                sql.Replace("@009", this.Ativo.ValorBD);

                sql.Replace("@010", this.DiasTriagem.ValorBD);

                sql.Replace("@011", this.De.ValorBD);

                sql.Replace("@012", this.Ate.ValorBD);


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
        /// Atualiza Entrega
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();

                sql.Append("UPDATE tEntrega SET Nome = '@001', PrazoEntrega = @002, Disponivel = '@003', ProcedimentoEntrega = '@004', EnviaAlerta = '@005', Padrao = '@006', PermitirImpressaoInternet = '@007', Tipo = '@008', Ativo = '@009', DiasTriagem = @010, De = @011, Ate = @012 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                sql.Replace("@001", this.Nome.ValorBD);

                sql.Replace("@002", this.PrazoEntrega.ValorBD);

                sql.Replace("@003", this.Disponivel.ValorBD);

                sql.Replace("@004", this.ProcedimentoEntrega.ValorBD);

                sql.Replace("@005", this.EnviaAlerta.ValorBD);

                sql.Replace("@006", this.Padrao.ValorBD);

                sql.Replace("@007", this.PermitirImpressaoInternet.ValorBD);

                sql.Replace("@008", this.Tipo.ValorBD);

                sql.Replace("@009", this.Ativo.ValorBD);

                sql.Replace("@010", this.DiasTriagem.ValorBD);

                sql.Replace("@011", this.De.ValorBD);

                sql.Replace("@012", this.Ate.ValorBD);


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
        /// Exclui Entrega com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEntrega WHERE ID=" + id;

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
        /// Exclui Entrega com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEntrega WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEntrega WHERE ID=" + id;

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
        /// Exclui Entrega
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

            this.PrazoEntrega.Limpar();

            this.Disponivel.Limpar();

            this.ProcedimentoEntrega.Limpar();

            this.EnviaAlerta.Limpar();

            this.Padrao.Limpar();

            this.PermitirImpressaoInternet.Limpar();

            this.Tipo.Limpar();

            this.Ativo.Limpar();

            this.DiasTriagem.Limpar();

            this.De.Limpar();

            this.Ate.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();

            this.Nome.Desfazer();

            this.PrazoEntrega.Desfazer();

            this.Disponivel.Desfazer();

            this.ProcedimentoEntrega.Desfazer();

            this.EnviaAlerta.Desfazer();

            this.Padrao.Desfazer();

            this.PermitirImpressaoInternet.Desfazer();

            this.Tipo.Desfazer();

            this.Ativo.Desfazer();

            this.DiasTriagem.Desfazer();

            this.De.Desfazer();

            this.Ate.Desfazer();

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


        public class prazoentrega : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrazoEntrega";
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


        public class disponivel : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Disponivel";
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


        public class procedimentoentrega : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ProcedimentoEntrega";
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


        public class enviaalerta : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnviaAlerta";
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


        public class padrao : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Padrao";
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


        public class permitirimpressaointernet : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermitirImpressaoInternet";
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


        public class ativo : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
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


        public class diastriagem : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "DiasTriagem";
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


        public class de : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "De";
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


        public class ate : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ate";
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

                DataTable tabela = new DataTable("Entrega");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("PrazoEntrega", typeof(int));

                tabela.Columns.Add("Disponivel", typeof(bool));

                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                tabela.Columns.Add("EnviaAlerta", typeof(bool));

                tabela.Columns.Add("Padrao", typeof(bool));

                tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                tabela.Columns.Add("Tipo", typeof(string));

                tabela.Columns.Add("Ativo", typeof(bool));

                tabela.Columns.Add("DiasTriagem", typeof(int));

                tabela.Columns.Add("De", typeof(int));

                tabela.Columns.Add("Ate", typeof(int));


                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
    #endregion

    #region "EntregaLista_B"


    public abstract class EntregaLista_B : BaseLista
    {

        private bool backup = false;
        protected Entrega entrega;

        // passar o Usuario logado no sistema
        public EntregaLista_B()
        {
            entrega = new Entrega();
        }

        // passar o Usuario logado no sistema
        public EntregaLista_B(int usuarioIDLogado)
        {
            entrega = new Entrega(usuarioIDLogado);
        }

        public Entrega Entrega
        {
            get { return entrega; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Entrega especifico
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
                    entrega.Ler(id);
                    return entrega;
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
                    sql = "SELECT ID FROM tEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEntrega";

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
                    sql = "SELECT ID FROM tEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEntrega";

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
                    sql = "SELECT ID FROM xEntrega";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEntrega";

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
        /// Preenche Entrega corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    entrega.Ler(id);
                else
                    entrega.LerBackup(id);

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

                bool ok = entrega.Excluir();
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
        /// Inseri novo(a) Entrega na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = entrega.Inserir();
                if (ok)
                {
                    lista.Add(entrega.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Entrega carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Entrega");

                tabela.Columns.Add("ID", typeof(int));

                tabela.Columns.Add("Nome", typeof(string));

                tabela.Columns.Add("PrazoEntrega", typeof(int));

                tabela.Columns.Add("Disponivel", typeof(bool));

                tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                tabela.Columns.Add("EnviaAlerta", typeof(bool));

                tabela.Columns.Add("Padrao", typeof(bool));

                tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                tabela.Columns.Add("Tipo", typeof(string));

                tabela.Columns.Add("Ativo", typeof(bool));

                tabela.Columns.Add("DiasTriagem", typeof(int));

                tabela.Columns.Add("De", typeof(int));

                tabela.Columns.Add("Ate", typeof(int));


                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = entrega.Control.ID;

                        linha["Nome"] = entrega.Nome.Valor;

                        linha["PrazoEntrega"] = entrega.PrazoEntrega.Valor;

                        linha["Disponivel"] = entrega.Disponivel.Valor;

                        linha["ProcedimentoEntrega"] = entrega.ProcedimentoEntrega.Valor;

                        linha["EnviaAlerta"] = entrega.EnviaAlerta.Valor;

                        linha["Padrao"] = entrega.Padrao.Valor;

                        linha["PermitirImpressaoInternet"] = entrega.PermitirImpressaoInternet.Valor;

                        linha["Tipo"] = entrega.Tipo.Valor;

                        linha["Ativo"] = entrega.Ativo.Valor;

                        linha["DiasTriagem"] = entrega.DiasTriagem.Valor;

                        linha["De"] = entrega.De.Valor;

                        linha["Ate"] = entrega.Ate.Valor;

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

                DataTable tabela = new DataTable("RelatorioEntrega");

                if (this.Primeiro())
                {


                    tabela.Columns.Add("Nome", typeof(string));

                    tabela.Columns.Add("PrazoEntrega", typeof(int));

                    tabela.Columns.Add("Disponivel", typeof(bool));

                    tabela.Columns.Add("ProcedimentoEntrega", typeof(string));

                    tabela.Columns.Add("EnviaAlerta", typeof(bool));

                    tabela.Columns.Add("Padrao", typeof(bool));

                    tabela.Columns.Add("PermitirImpressaoInternet", typeof(bool));

                    tabela.Columns.Add("Tipo", typeof(string));

                    tabela.Columns.Add("Ativo", typeof(bool));

                    tabela.Columns.Add("DiasTriagem", typeof(int));

                    tabela.Columns.Add("De", typeof(int));

                    tabela.Columns.Add("Ate", typeof(int));


                    do
                    {
                        DataRow linha = tabela.NewRow();

                        linha["Nome"] = entrega.Nome.Valor;

                        linha["PrazoEntrega"] = entrega.PrazoEntrega.Valor;

                        linha["Disponivel"] = entrega.Disponivel.Valor;

                        linha["ProcedimentoEntrega"] = entrega.ProcedimentoEntrega.Valor;

                        linha["EnviaAlerta"] = entrega.EnviaAlerta.Valor;

                        linha["Padrao"] = entrega.Padrao.Valor;

                        linha["PermitirImpressaoInternet"] = entrega.PermitirImpressaoInternet.Valor;

                        linha["Tipo"] = entrega.Tipo.Valor;

                        linha["Ativo"] = entrega.Ativo.Valor;

                        linha["DiasTriagem"] = entrega.DiasTriagem.Valor;

                        linha["De"] = entrega.De.Valor;

                        linha["Ate"] = entrega.Ate.Valor;

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
                        sql = "SELECT ID, Nome FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;

                    case "PrazoEntrega":
                        sql = "SELECT ID, PrazoEntrega FROM tEntrega WHERE " + FiltroSQL + " ORDER BY PrazoEntrega";
                        break;

                    case "Disponivel":
                        sql = "SELECT ID, Disponivel FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Disponivel";
                        break;

                    case "ProcedimentoEntrega":
                        sql = "SELECT ID, ProcedimentoEntrega FROM tEntrega WHERE " + FiltroSQL + " ORDER BY ProcedimentoEntrega";
                        break;

                    case "EnviaAlerta":
                        sql = "SELECT ID, EnviaAlerta FROM tEntrega WHERE " + FiltroSQL + " ORDER BY EnviaAlerta";
                        break;

                    case "Padrao":
                        sql = "SELECT ID, Padrao FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Padrao";
                        break;

                    case "PermitirImpressaoInternet":
                        sql = "SELECT ID, PermitirImpressaoInternet FROM tEntrega WHERE " + FiltroSQL + " ORDER BY PermitirImpressaoInternet";
                        break;

                    case "Tipo":
                        sql = "SELECT ID, Tipo FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Tipo";
                        break;

                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;

                    case "DiasTriagem":
                        sql = "SELECT ID, DiasTriagem FROM tEntrega WHERE " + FiltroSQL + " ORDER BY DiasTriagem";
                        break;

                    case "De":
                        sql = "SELECT ID, De FROM tEntrega WHERE " + FiltroSQL + " ORDER BY De";
                        break;

                    case "Ate":
                        sql = "SELECT ID, Ate FROM tEntrega WHERE " + FiltroSQL + " ORDER BY Ate";
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

    #region "EntregaException"

    [Serializable]
    public class EntregaException : Exception
    {

        public EntregaException() : base() { }

        public EntregaException(string msg) : base(msg) { }

        public EntregaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}