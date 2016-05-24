/******************************************************
* Arquivo ObrigatoriedadeDB.cs
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

    #region "Obrigatoriedade_B"

    public abstract class Obrigatoriedade_B : BaseBD
    {

        public nome Nome = new nome();
        public rg RG = new rg();
        public cpf CPF = new cpf();
        public telefone Telefone = new telefone();
        public telefonecomercial TelefoneComercial = new telefonecomercial();
        public celular Celular = new celular();
        public datanascimento DataNascimento = new datanascimento();
        public email Email = new email();
        public cepentrega CEPEntrega = new cepentrega();
        public enderecoentrega EnderecoEntrega = new enderecoentrega();
        public numeroentrega NumeroEntrega = new numeroentrega();
        public complementoentrega ComplementoEntrega = new complementoentrega();
        public bairroentrega BairroEntrega = new bairroentrega();
        public cidadeentrega CidadeEntrega = new cidadeentrega();
        public estadoentrega EstadoEntrega = new estadoentrega();
        public cepcliente CEPCliente = new cepcliente();
        public enderecocliente EnderecoCliente = new enderecocliente();
        public numerocliente NumeroCliente = new numerocliente();
        public complementocliente ComplementoCliente = new complementocliente();
        public bairrocliente BairroCliente = new bairrocliente();
        public cidadecliente CidadeCliente = new cidadecliente();
        public estadocliente EstadoCliente = new estadocliente();
        public nomeentrega NomeEntrega = new nomeentrega();
        public cpfentrega CPFEntrega = new cpfentrega();
        public rgentrega RGEntrega = new rgentrega();
        public cpfresponsavel CPFResponsavel = new cpfresponsavel();
        public nomeresponsavel NomeResponsavel = new nomeresponsavel();

        public Obrigatoriedade_B() { }

        // passar o Usuario logado no sistema
        public Obrigatoriedade_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Obrigatoriedade
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tObrigatoriedade WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.TelefoneComercial.ValorBD = bd.LerString("TelefoneComercial");
                    this.Celular.ValorBD = bd.LerString("Celular");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.CEPEntrega.ValorBD = bd.LerString("CEPEntrega");
                    this.EnderecoEntrega.ValorBD = bd.LerString("EnderecoEntrega");
                    this.NumeroEntrega.ValorBD = bd.LerString("NumeroEntrega");
                    this.ComplementoEntrega.ValorBD = bd.LerString("ComplementoEntrega");
                    this.BairroEntrega.ValorBD = bd.LerString("BairroEntrega");
                    this.CidadeEntrega.ValorBD = bd.LerString("CidadeEntrega");
                    this.EstadoEntrega.ValorBD = bd.LerString("EstadoEntrega");
                    this.CEPCliente.ValorBD = bd.LerString("CEPCliente");
                    this.EnderecoCliente.ValorBD = bd.LerString("EnderecoCliente");
                    this.NumeroCliente.ValorBD = bd.LerString("NumeroCliente");
                    this.ComplementoCliente.ValorBD = bd.LerString("ComplementoCliente");
                    this.BairroCliente.ValorBD = bd.LerString("BairroCliente");
                    this.CidadeCliente.ValorBD = bd.LerString("CidadeCliente");
                    this.EstadoCliente.ValorBD = bd.LerString("EstadoCliente");
                    this.NomeEntrega.ValorBD = bd.LerString("NomeEntrega");
                    this.CPFEntrega.ValorBD = bd.LerString("CPFEntrega");
                    this.RGEntrega.ValorBD = bd.LerString("RGEntrega");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.NomeResponsavel.ValorBD = bd.LerString("NomeResponsavel");
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
        /// Preenche todos os atributos de Obrigatoriedade do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xObrigatoriedade WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.TelefoneComercial.ValorBD = bd.LerString("TelefoneComercial");
                    this.Celular.ValorBD = bd.LerString("Celular");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.CEPEntrega.ValorBD = bd.LerString("CEPEntrega");
                    this.EnderecoEntrega.ValorBD = bd.LerString("EnderecoEntrega");
                    this.NumeroEntrega.ValorBD = bd.LerString("NumeroEntrega");
                    this.ComplementoEntrega.ValorBD = bd.LerString("ComplementoEntrega");
                    this.BairroEntrega.ValorBD = bd.LerString("BairroEntrega");
                    this.CidadeEntrega.ValorBD = bd.LerString("CidadeEntrega");
                    this.EstadoEntrega.ValorBD = bd.LerString("EstadoEntrega");
                    this.CEPCliente.ValorBD = bd.LerString("CEPCliente");
                    this.EnderecoCliente.ValorBD = bd.LerString("EnderecoCliente");
                    this.NumeroCliente.ValorBD = bd.LerString("NumeroCliente");
                    this.ComplementoCliente.ValorBD = bd.LerString("ComplementoCliente");
                    this.BairroCliente.ValorBD = bd.LerString("BairroCliente");
                    this.CidadeCliente.ValorBD = bd.LerString("CidadeCliente");
                    this.EstadoCliente.ValorBD = bd.LerString("EstadoCliente");
                    this.NomeEntrega.ValorBD = bd.LerString("NomeEntrega");
                    this.CPFEntrega.ValorBD = bd.LerString("CPFEntrega");
                    this.RGEntrega.ValorBD = bd.LerString("RGEntrega");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.NomeResponsavel.ValorBD = bd.LerString("NomeResponsavel");
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
                sql.Append("INSERT INTO cObrigatoriedade (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xObrigatoriedade (ID, Versao, Nome, RG, CPF, Telefone, TelefoneComercial, Celular, DataNascimento, Email, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, EstadoCliente, NomeEntrega, CPFEntrega, RGEntrega, CPFResponsavel, NomeResponsavel) ");
                sql.Append("SELECT ID, @V, Nome, RG, CPF, Telefone, TelefoneComercial, Celular, DataNascimento, Email, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, EstadoCliente, NomeEntrega, CPFEntrega, RGEntrega, CPFResponsavel, NomeResponsavel FROM tObrigatoriedade WHERE ID = @I");
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
        /// Inserir novo(a) Obrigatoriedade
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cObrigatoriedade");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tObrigatoriedade(ID, Nome, RG, CPF, Telefone, TelefoneComercial, Celular, DataNascimento, Email, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, EstadoCliente, NomeEntrega, CPFEntrega, RGEntrega, CPFResponsavel, NomeResponsavel) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.TelefoneComercial.ValorBD);
                sql.Replace("@006", this.Celular.ValorBD);
                sql.Replace("@007", this.DataNascimento.ValorBD);
                sql.Replace("@008", this.Email.ValorBD);
                sql.Replace("@009", this.CEPEntrega.ValorBD);
                sql.Replace("@010", this.EnderecoEntrega.ValorBD);
                sql.Replace("@011", this.NumeroEntrega.ValorBD);
                sql.Replace("@012", this.ComplementoEntrega.ValorBD);
                sql.Replace("@013", this.BairroEntrega.ValorBD);
                sql.Replace("@014", this.CidadeEntrega.ValorBD);
                sql.Replace("@015", this.EstadoEntrega.ValorBD);
                sql.Replace("@016", this.CEPCliente.ValorBD);
                sql.Replace("@017", this.EnderecoCliente.ValorBD);
                sql.Replace("@018", this.NumeroCliente.ValorBD);
                sql.Replace("@019", this.ComplementoCliente.ValorBD);
                sql.Replace("@020", this.BairroCliente.ValorBD);
                sql.Replace("@021", this.CidadeCliente.ValorBD);
                sql.Replace("@022", this.EstadoCliente.ValorBD);
                sql.Replace("@023", this.NomeEntrega.ValorBD);
                sql.Replace("@024", this.CPFEntrega.ValorBD);
                sql.Replace("@025", this.RGEntrega.ValorBD);
                sql.Replace("@026", this.CPFResponsavel.ValorBD);
                sql.Replace("@027", this.NomeResponsavel.ValorBD);

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
        /// Atualiza Obrigatoriedade
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cObrigatoriedade WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tObrigatoriedade SET Nome = '@001', RG = '@002', CPF = '@003', Telefone = '@004', TelefoneComercial = '@005', Celular = '@006', DataNascimento = '@007', Email = '@008', CEPEntrega = '@009', EnderecoEntrega = '@010', NumeroEntrega = '@011', ComplementoEntrega = '@012', BairroEntrega = '@013', CidadeEntrega = '@014', EstadoEntrega = '@015', CEPCliente = '@016', EnderecoCliente = '@017', NumeroCliente = '@018', ComplementoCliente = '@019', BairroCliente = '@020', CidadeCliente = '@021', EstadoCliente = '@022', NomeEntrega = '@023', CPFEntrega = '@024', RGEntrega = '@025', CPFResponsavel = '@026', NomeResponsavel = '@027' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.TelefoneComercial.ValorBD);
                sql.Replace("@006", this.Celular.ValorBD);
                sql.Replace("@007", this.DataNascimento.ValorBD);
                sql.Replace("@008", this.Email.ValorBD);
                sql.Replace("@009", this.CEPEntrega.ValorBD);
                sql.Replace("@010", this.EnderecoEntrega.ValorBD);
                sql.Replace("@011", this.NumeroEntrega.ValorBD);
                sql.Replace("@012", this.ComplementoEntrega.ValorBD);
                sql.Replace("@013", this.BairroEntrega.ValorBD);
                sql.Replace("@014", this.CidadeEntrega.ValorBD);
                sql.Replace("@015", this.EstadoEntrega.ValorBD);
                sql.Replace("@016", this.CEPCliente.ValorBD);
                sql.Replace("@017", this.EnderecoCliente.ValorBD);
                sql.Replace("@018", this.NumeroCliente.ValorBD);
                sql.Replace("@019", this.ComplementoCliente.ValorBD);
                sql.Replace("@020", this.BairroCliente.ValorBD);
                sql.Replace("@021", this.CidadeCliente.ValorBD);
                sql.Replace("@022", this.EstadoCliente.ValorBD);
                sql.Replace("@023", this.NomeEntrega.ValorBD);
                sql.Replace("@024", this.CPFEntrega.ValorBD);
                sql.Replace("@025", this.RGEntrega.ValorBD);
                sql.Replace("@026", this.CPFResponsavel.ValorBD);
                sql.Replace("@027", this.NomeResponsavel.ValorBD);

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
        /// Atualiza Obrigatoriedade
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cObrigatoriedade WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tObrigatoriedade SET Nome = '@001', RG = '@002', CPF = '@003', Telefone = '@004', TelefoneComercial = '@005', Celular = '@006', DataNascimento = '@007', Email = '@008', CEPEntrega = '@009', EnderecoEntrega = '@010', NumeroEntrega = '@011', ComplementoEntrega = '@012', BairroEntrega = '@013', CidadeEntrega = '@014', EstadoEntrega = '@015', CEPCliente = '@016', EnderecoCliente = '@017', NumeroCliente = '@018', ComplementoCliente = '@019', BairroCliente = '@020', CidadeCliente = '@021', EstadoCliente = '@022', NomeEntrega = '@023', CPFEntrega = '@024', RGEntrega = '@025', CPFResponsavel = '@026', NomeResponsavel = '@027' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.Telefone.ValorBD);
                sql.Replace("@005", this.TelefoneComercial.ValorBD);
                sql.Replace("@006", this.Celular.ValorBD);
                sql.Replace("@007", this.DataNascimento.ValorBD);
                sql.Replace("@008", this.Email.ValorBD);
                sql.Replace("@009", this.CEPEntrega.ValorBD);
                sql.Replace("@010", this.EnderecoEntrega.ValorBD);
                sql.Replace("@011", this.NumeroEntrega.ValorBD);
                sql.Replace("@012", this.ComplementoEntrega.ValorBD);
                sql.Replace("@013", this.BairroEntrega.ValorBD);
                sql.Replace("@014", this.CidadeEntrega.ValorBD);
                sql.Replace("@015", this.EstadoEntrega.ValorBD);
                sql.Replace("@016", this.CEPCliente.ValorBD);
                sql.Replace("@017", this.EnderecoCliente.ValorBD);
                sql.Replace("@018", this.NumeroCliente.ValorBD);
                sql.Replace("@019", this.ComplementoCliente.ValorBD);
                sql.Replace("@020", this.BairroCliente.ValorBD);
                sql.Replace("@021", this.CidadeCliente.ValorBD);
                sql.Replace("@022", this.EstadoCliente.ValorBD);
                sql.Replace("@023", this.NomeEntrega.ValorBD);
                sql.Replace("@024", this.CPFEntrega.ValorBD);
                sql.Replace("@025", this.RGEntrega.ValorBD);
                sql.Replace("@026", this.CPFResponsavel.ValorBD);
                sql.Replace("@027", this.NomeResponsavel.ValorBD);

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
        /// Exclui Obrigatoriedade com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cObrigatoriedade WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tObrigatoriedade WHERE ID=" + id;

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
        /// Exclui Obrigatoriedade com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cObrigatoriedade WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tObrigatoriedade WHERE ID=" + id;

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
        /// Exclui Obrigatoriedade
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
            this.RG.Limpar();
            this.CPF.Limpar();
            this.Telefone.Limpar();
            this.TelefoneComercial.Limpar();
            this.Celular.Limpar();
            this.DataNascimento.Limpar();
            this.Email.Limpar();
            this.CEPEntrega.Limpar();
            this.EnderecoEntrega.Limpar();
            this.NumeroEntrega.Limpar();
            this.ComplementoEntrega.Limpar();
            this.BairroEntrega.Limpar();
            this.CidadeEntrega.Limpar();
            this.EstadoEntrega.Limpar();
            this.CEPCliente.Limpar();
            this.EnderecoCliente.Limpar();
            this.NumeroCliente.Limpar();
            this.ComplementoCliente.Limpar();
            this.BairroCliente.Limpar();
            this.CidadeCliente.Limpar();
            this.EstadoCliente.Limpar();
            this.NomeEntrega.Limpar();
            this.CPFEntrega.Limpar();
            this.RGEntrega.Limpar();
            this.CPFResponsavel.Limpar();
            this.NomeResponsavel.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.RG.Desfazer();
            this.CPF.Desfazer();
            this.Telefone.Desfazer();
            this.TelefoneComercial.Desfazer();
            this.Celular.Desfazer();
            this.DataNascimento.Desfazer();
            this.Email.Desfazer();
            this.CEPEntrega.Desfazer();
            this.EnderecoEntrega.Desfazer();
            this.NumeroEntrega.Desfazer();
            this.ComplementoEntrega.Desfazer();
            this.BairroEntrega.Desfazer();
            this.CidadeEntrega.Desfazer();
            this.EstadoEntrega.Desfazer();
            this.CEPCliente.Desfazer();
            this.EnderecoCliente.Desfazer();
            this.NumeroCliente.Desfazer();
            this.ComplementoCliente.Desfazer();
            this.BairroCliente.Desfazer();
            this.CidadeCliente.Desfazer();
            this.EstadoCliente.Desfazer();
            this.NomeEntrega.Desfazer();
            this.CPFEntrega.Desfazer();
            this.RGEntrega.Desfazer();
            this.CPFResponsavel.Desfazer();
            this.NomeResponsavel.Desfazer();
        }

        public class nome : BooleanProperty
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

        public class rg : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RG";
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

        public class cpf : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPF";
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

        public class telefone : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Telefone";
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

        public class telefonecomercial : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "TelefoneComercial";
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

        public class celular : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Celular";
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

        public class datanascimento : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataNascimento";
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

        public class email : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Email";
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

        public class cepentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CEPEntrega";
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

        public class enderecoentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnderecoEntrega";
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

        public class numeroentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroEntrega";
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

        public class complementoentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComplementoEntrega";
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

        public class bairroentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "BairroEntrega";
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

        public class cidadeentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CidadeEntrega";
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

        public class estadoentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstadoEntrega";
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

        public class cepcliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CEPCliente";
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

        public class enderecocliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnderecoCliente";
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

        public class numerocliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroCliente";
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

        public class complementocliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComplementoCliente";
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

        public class bairrocliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "BairroCliente";
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

        public class cidadecliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CidadeCliente";
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

        public class estadocliente : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstadoCliente";
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

        public class nomeentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeEntrega";
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

        public class cpfentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFEntrega";
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

        public class rgentrega : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RGEntrega";
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

        public class nomeresponsavel : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeResponsavel";
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

                DataTable tabela = new DataTable("Obrigatoriedade");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(bool));
                tabela.Columns.Add("RG", typeof(bool));
                tabela.Columns.Add("CPF", typeof(bool));
                tabela.Columns.Add("Telefone", typeof(bool));
                tabela.Columns.Add("TelefoneComercial", typeof(bool));
                tabela.Columns.Add("Celular", typeof(bool));
                tabela.Columns.Add("DataNascimento", typeof(bool));
                tabela.Columns.Add("Email", typeof(bool));
                tabela.Columns.Add("CEPEntrega", typeof(bool));
                tabela.Columns.Add("EnderecoEntrega", typeof(bool));
                tabela.Columns.Add("NumeroEntrega", typeof(bool));
                tabela.Columns.Add("ComplementoEntrega", typeof(bool));
                tabela.Columns.Add("BairroEntrega", typeof(bool));
                tabela.Columns.Add("CidadeEntrega", typeof(bool));
                tabela.Columns.Add("EstadoEntrega", typeof(bool));
                tabela.Columns.Add("CEPCliente", typeof(bool));
                tabela.Columns.Add("EnderecoCliente", typeof(bool));
                tabela.Columns.Add("NumeroCliente", typeof(bool));
                tabela.Columns.Add("ComplementoCliente", typeof(bool));
                tabela.Columns.Add("BairroCliente", typeof(bool));
                tabela.Columns.Add("CidadeCliente", typeof(bool));
                tabela.Columns.Add("EstadoCliente", typeof(bool));
                tabela.Columns.Add("NomeEntrega", typeof(bool));
                tabela.Columns.Add("CPFEntrega", typeof(bool));
                tabela.Columns.Add("RGEntrega", typeof(bool));
                tabela.Columns.Add("CPFResponsavel", typeof(bool));
                tabela.Columns.Add("NomeResponsavel", typeof(bool));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ObrigatoriedadeLista_B"

    public abstract class ObrigatoriedadeLista_B : BaseLista
    {

        private bool backup = false;
        protected Obrigatoriedade obrigatoriedade;

        // passar o Usuario logado no sistema
        public ObrigatoriedadeLista_B()
        {
            obrigatoriedade = new Obrigatoriedade();
        }

        // passar o Usuario logado no sistema
        public ObrigatoriedadeLista_B(int usuarioIDLogado)
        {
            obrigatoriedade = new Obrigatoriedade(usuarioIDLogado);
        }

        public Obrigatoriedade Obrigatoriedade
        {
            get { return obrigatoriedade; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Obrigatoriedade especifico
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
                    obrigatoriedade.Ler(id);
                    return obrigatoriedade;
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
                    sql = "SELECT ID FROM tObrigatoriedade";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tObrigatoriedade";

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
                    sql = "SELECT ID FROM tObrigatoriedade";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tObrigatoriedade";

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
                    sql = "SELECT ID FROM xObrigatoriedade";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xObrigatoriedade";

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
        /// Preenche Obrigatoriedade corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    obrigatoriedade.Ler(id);
                else
                    obrigatoriedade.LerBackup(id);

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

                bool ok = obrigatoriedade.Excluir();
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
        /// Inseri novo(a) Obrigatoriedade na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = obrigatoriedade.Inserir();
                if (ok)
                {
                    lista.Add(obrigatoriedade.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Obrigatoriedade carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Obrigatoriedade");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(bool));
                tabela.Columns.Add("RG", typeof(bool));
                tabela.Columns.Add("CPF", typeof(bool));
                tabela.Columns.Add("Telefone", typeof(bool));
                tabela.Columns.Add("TelefoneComercial", typeof(bool));
                tabela.Columns.Add("Celular", typeof(bool));
                tabela.Columns.Add("DataNascimento", typeof(bool));
                tabela.Columns.Add("Email", typeof(bool));
                tabela.Columns.Add("CEPEntrega", typeof(bool));
                tabela.Columns.Add("EnderecoEntrega", typeof(bool));
                tabela.Columns.Add("NumeroEntrega", typeof(bool));
                tabela.Columns.Add("ComplementoEntrega", typeof(bool));
                tabela.Columns.Add("BairroEntrega", typeof(bool));
                tabela.Columns.Add("CidadeEntrega", typeof(bool));
                tabela.Columns.Add("EstadoEntrega", typeof(bool));
                tabela.Columns.Add("CEPCliente", typeof(bool));
                tabela.Columns.Add("EnderecoCliente", typeof(bool));
                tabela.Columns.Add("NumeroCliente", typeof(bool));
                tabela.Columns.Add("ComplementoCliente", typeof(bool));
                tabela.Columns.Add("BairroCliente", typeof(bool));
                tabela.Columns.Add("CidadeCliente", typeof(bool));
                tabela.Columns.Add("EstadoCliente", typeof(bool));
                tabela.Columns.Add("NomeEntrega", typeof(bool));
                tabela.Columns.Add("CPFEntrega", typeof(bool));
                tabela.Columns.Add("RGEntrega", typeof(bool));
                tabela.Columns.Add("CPFResponsavel", typeof(bool));
                tabela.Columns.Add("NomeResponsavel", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = obrigatoriedade.Control.ID;
                        linha["Nome"] = obrigatoriedade.Nome.Valor;
                        linha["RG"] = obrigatoriedade.RG.Valor;
                        linha["CPF"] = obrigatoriedade.CPF.Valor;
                        linha["Telefone"] = obrigatoriedade.Telefone.Valor;
                        linha["TelefoneComercial"] = obrigatoriedade.TelefoneComercial.Valor;
                        linha["Celular"] = obrigatoriedade.Celular.Valor;
                        linha["DataNascimento"] = obrigatoriedade.DataNascimento.Valor;
                        linha["Email"] = obrigatoriedade.Email.Valor;
                        linha["CEPEntrega"] = obrigatoriedade.CEPEntrega.Valor;
                        linha["EnderecoEntrega"] = obrigatoriedade.EnderecoEntrega.Valor;
                        linha["NumeroEntrega"] = obrigatoriedade.NumeroEntrega.Valor;
                        linha["ComplementoEntrega"] = obrigatoriedade.ComplementoEntrega.Valor;
                        linha["BairroEntrega"] = obrigatoriedade.BairroEntrega.Valor;
                        linha["CidadeEntrega"] = obrigatoriedade.CidadeEntrega.Valor;
                        linha["EstadoEntrega"] = obrigatoriedade.EstadoEntrega.Valor;
                        linha["CEPCliente"] = obrigatoriedade.CEPCliente.Valor;
                        linha["EnderecoCliente"] = obrigatoriedade.EnderecoCliente.Valor;
                        linha["NumeroCliente"] = obrigatoriedade.NumeroCliente.Valor;
                        linha["ComplementoCliente"] = obrigatoriedade.ComplementoCliente.Valor;
                        linha["BairroCliente"] = obrigatoriedade.BairroCliente.Valor;
                        linha["CidadeCliente"] = obrigatoriedade.CidadeCliente.Valor;
                        linha["EstadoCliente"] = obrigatoriedade.EstadoCliente.Valor;
                        linha["NomeEntrega"] = obrigatoriedade.NomeEntrega.Valor;
                        linha["CPFEntrega"] = obrigatoriedade.CPFEntrega.Valor;
                        linha["RGEntrega"] = obrigatoriedade.RGEntrega.Valor;
                        linha["CPFResponsavel"] = obrigatoriedade.CPFResponsavel.Valor;
                        linha["NomeResponsavel"] = obrigatoriedade.NomeResponsavel.Valor;
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

                DataTable tabela = new DataTable("RelatorioObrigatoriedade");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(bool));
                    tabela.Columns.Add("RG", typeof(bool));
                    tabela.Columns.Add("CPF", typeof(bool));
                    tabela.Columns.Add("Telefone", typeof(bool));
                    tabela.Columns.Add("TelefoneComercial", typeof(bool));
                    tabela.Columns.Add("Celular", typeof(bool));
                    tabela.Columns.Add("DataNascimento", typeof(bool));
                    tabela.Columns.Add("Email", typeof(bool));
                    tabela.Columns.Add("CEPEntrega", typeof(bool));
                    tabela.Columns.Add("EnderecoEntrega", typeof(bool));
                    tabela.Columns.Add("NumeroEntrega", typeof(bool));
                    tabela.Columns.Add("ComplementoEntrega", typeof(bool));
                    tabela.Columns.Add("BairroEntrega", typeof(bool));
                    tabela.Columns.Add("CidadeEntrega", typeof(bool));
                    tabela.Columns.Add("EstadoEntrega", typeof(bool));
                    tabela.Columns.Add("CEPCliente", typeof(bool));
                    tabela.Columns.Add("EnderecoCliente", typeof(bool));
                    tabela.Columns.Add("NumeroCliente", typeof(bool));
                    tabela.Columns.Add("ComplementoCliente", typeof(bool));
                    tabela.Columns.Add("BairroCliente", typeof(bool));
                    tabela.Columns.Add("CidadeCliente", typeof(bool));
                    tabela.Columns.Add("EstadoCliente", typeof(bool));
                    tabela.Columns.Add("NomeEntrega", typeof(bool));
                    tabela.Columns.Add("CPFEntrega", typeof(bool));
                    tabela.Columns.Add("RGEntrega", typeof(bool));
                    tabela.Columns.Add("CPFResponsavel", typeof(bool));
                    tabela.Columns.Add("NomeResponsavel", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = obrigatoriedade.Nome.Valor;
                        linha["RG"] = obrigatoriedade.RG.Valor;
                        linha["CPF"] = obrigatoriedade.CPF.Valor;
                        linha["Telefone"] = obrigatoriedade.Telefone.Valor;
                        linha["TelefoneComercial"] = obrigatoriedade.TelefoneComercial.Valor;
                        linha["Celular"] = obrigatoriedade.Celular.Valor;
                        linha["DataNascimento"] = obrigatoriedade.DataNascimento.Valor;
                        linha["Email"] = obrigatoriedade.Email.Valor;
                        linha["CEPEntrega"] = obrigatoriedade.CEPEntrega.Valor;
                        linha["EnderecoEntrega"] = obrigatoriedade.EnderecoEntrega.Valor;
                        linha["NumeroEntrega"] = obrigatoriedade.NumeroEntrega.Valor;
                        linha["ComplementoEntrega"] = obrigatoriedade.ComplementoEntrega.Valor;
                        linha["BairroEntrega"] = obrigatoriedade.BairroEntrega.Valor;
                        linha["CidadeEntrega"] = obrigatoriedade.CidadeEntrega.Valor;
                        linha["EstadoEntrega"] = obrigatoriedade.EstadoEntrega.Valor;
                        linha["CEPCliente"] = obrigatoriedade.CEPCliente.Valor;
                        linha["EnderecoCliente"] = obrigatoriedade.EnderecoCliente.Valor;
                        linha["NumeroCliente"] = obrigatoriedade.NumeroCliente.Valor;
                        linha["ComplementoCliente"] = obrigatoriedade.ComplementoCliente.Valor;
                        linha["BairroCliente"] = obrigatoriedade.BairroCliente.Valor;
                        linha["CidadeCliente"] = obrigatoriedade.CidadeCliente.Valor;
                        linha["EstadoCliente"] = obrigatoriedade.EstadoCliente.Valor;
                        linha["NomeEntrega"] = obrigatoriedade.NomeEntrega.Valor;
                        linha["CPFEntrega"] = obrigatoriedade.CPFEntrega.Valor;
                        linha["RGEntrega"] = obrigatoriedade.RGEntrega.Valor;
                        linha["CPFResponsavel"] = obrigatoriedade.CPFResponsavel.Valor;
                        linha["NomeResponsavel"] = obrigatoriedade.NomeResponsavel.Valor;
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
                        sql = "SELECT ID, Nome FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "RG":
                        sql = "SELECT ID, RG FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY RG";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "TelefoneComercial":
                        sql = "SELECT ID, TelefoneComercial FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY TelefoneComercial";
                        break;
                    case "Celular":
                        sql = "SELECT ID, Celular FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY Celular";
                        break;
                    case "DataNascimento":
                        sql = "SELECT ID, DataNascimento FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY DataNascimento";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "CEPEntrega":
                        sql = "SELECT ID, CEPEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CEPEntrega";
                        break;
                    case "EnderecoEntrega":
                        sql = "SELECT ID, EnderecoEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY EnderecoEntrega";
                        break;
                    case "NumeroEntrega":
                        sql = "SELECT ID, NumeroEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY NumeroEntrega";
                        break;
                    case "ComplementoEntrega":
                        sql = "SELECT ID, ComplementoEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY ComplementoEntrega";
                        break;
                    case "BairroEntrega":
                        sql = "SELECT ID, BairroEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY BairroEntrega";
                        break;
                    case "CidadeEntrega":
                        sql = "SELECT ID, CidadeEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CidadeEntrega";
                        break;
                    case "EstadoEntrega":
                        sql = "SELECT ID, EstadoEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY EstadoEntrega";
                        break;
                    case "CEPCliente":
                        sql = "SELECT ID, CEPCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CEPCliente";
                        break;
                    case "EnderecoCliente":
                        sql = "SELECT ID, EnderecoCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY EnderecoCliente";
                        break;
                    case "NumeroCliente":
                        sql = "SELECT ID, NumeroCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY NumeroCliente";
                        break;
                    case "ComplementoCliente":
                        sql = "SELECT ID, ComplementoCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY ComplementoCliente";
                        break;
                    case "BairroCliente":
                        sql = "SELECT ID, BairroCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY BairroCliente";
                        break;
                    case "CidadeCliente":
                        sql = "SELECT ID, CidadeCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CidadeCliente";
                        break;
                    case "EstadoCliente":
                        sql = "SELECT ID, EstadoCliente FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY EstadoCliente";
                        break;
                    case "NomeEntrega":
                        sql = "SELECT ID, NomeEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY NomeEntrega";
                        break;
                    case "CPFEntrega":
                        sql = "SELECT ID, CPFEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CPFEntrega";
                        break;
                    case "RGEntrega":
                        sql = "SELECT ID, RGEntrega FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY RGEntrega";
                        break;
                    case "CPFResponsavel":
                        sql = "SELECT ID, CPFResponsavel FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY CPFResponsavel";
                        break;
                    case "NomeResponsavel":
                        sql = "SELECT ID, NomeResponsavel FROM tObrigatoriedade WHERE " + FiltroSQL + " ORDER BY NomeResponsavel";
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

    #region "ObrigatoriedadeException"

    [Serializable]
    public class ObrigatoriedadeException : Exception
    {

        public ObrigatoriedadeException() : base() { }

        public ObrigatoriedadeException(string msg) : base(msg) { }

        public ObrigatoriedadeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}