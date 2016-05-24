/******************************************************
* Arquivo ClienteEnderecoDB.cs
* Gerado em: 07/07/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "ClienteEndereco_B"

    public abstract class ClienteEndereco_B : BaseBD
    {

        public clienteid ClienteID = new clienteid();
        public cep CEP = new cep();
        public endereco Endereco = new endereco();
        public numero Numero = new numero();
        public cidade Cidade = new cidade();
        public estado Estado = new estado();
        public complemento Complemento = new complemento();
        public bairro Bairro = new bairro();
        public nome Nome = new nome();
        public cpf CPF = new cpf();
        public rg RG = new rg();
        public enderecotipoid EnderecoTipoID = new enderecotipoid();
        public enderecoprincipal EnderecoPrincipal = new enderecoprincipal();
        public statusconsulta StatusConsulta = new statusconsulta();

        public ClienteEndereco_B() { }

        // passar o Usuario logado no sistema
        public ClienteEndereco_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de ClienteEndereco
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tClienteEndereco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Numero.ValorBD = bd.LerString("Numero");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.EnderecoTipoID.ValorBD = bd.LerInt("EnderecoTipoID").ToString();
                    this.EnderecoPrincipal.ValorBD = bd.LerString("EnderecoPrincipal");
                    this.StatusConsulta.ValorBD = bd.LerInt("StatusConsulta").ToString();
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
        /// Preenche todos os atributos de ClienteEndereco do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xClienteEndereco WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Numero.ValorBD = bd.LerString("Numero");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.EnderecoTipoID.ValorBD = bd.LerInt("EnderecoTipoID").ToString();
                    this.EnderecoPrincipal.ValorBD = bd.LerString("EnderecoPrincipal");
                    this.StatusConsulta.ValorBD = bd.LerInt("StatusConsulta").ToString();
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
                sql.Append("INSERT INTO cClienteEndereco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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
        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cClienteEndereco (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xClienteEndereco (ID, Versao, ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta) ");
                sql.Append("SELECT ID, @V, ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta FROM tClienteEndereco WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xClienteEndereco (ID, Versao, ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta) ");
                sql.Append("SELECT ID, @V, ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta FROM tClienteEndereco WHERE ID = @I");
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
        /// Inserir novo(a) ClienteEndereco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tClienteEndereco(ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta) ");
                sql.Append("VALUES (@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011',@012,'@013',@014) SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.CEP.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.Complemento.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Nome.ValorBD);
                sql.Replace("@010", this.CPF.ValorBD);
                sql.Replace("@011", this.RG.ValorBD);
                sql.Replace("@012", this.EnderecoTipoID.ValorBD);
                sql.Replace("@013", this.EnderecoPrincipal.ValorBD);
                sql.Replace("@014", this.StatusConsulta.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                this.Control.Versao = 0;

                if (this.Control.ID > 0)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return this.Control.ID > 0;

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
        /// Inserir novo(a) ClienteEndereco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tClienteEndereco(ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal, StatusConsulta) ");
                sql.Append("VALUES (@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011',@012,'@013',@014); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.CEP.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.Complemento.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Nome.ValorBD);
                sql.Replace("@010", this.CPF.ValorBD);
                sql.Replace("@011", this.RG.ValorBD);
                sql.Replace("@012", this.EnderecoTipoID.ValorBD);
                sql.Replace("@013", this.EnderecoPrincipal.ValorBD);
                sql.Replace("@014", this.StatusConsulta.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                this.Control.Versao = 0;

                bool result = (this.Control.ID > 0);

                if (result)
                    InserirControle(bd, "I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza ClienteEndereco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cClienteEndereco (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tClienteEndereco SET ClienteID = @001, CEP = '@002', Endereco = '@003', Numero = '@004', Cidade = '@005', Estado = '@006', Complemento = '@007', Bairro = '@008', Nome = '@009', CPF = '@010', RG = '@011', EnderecoTipoID = @012, EnderecoPrincipal = '@013', StatusConsulta = @014 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.CEP.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.Complemento.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Nome.ValorBD);
                sql.Replace("@010", this.CPF.ValorBD);
                sql.Replace("@011", this.RG.ValorBD);
                sql.Replace("@012", this.EnderecoTipoID.ValorBD);
                sql.Replace("@013", this.EnderecoPrincipal.ValorBD);
                sql.Replace("@014", this.StatusConsulta.ValorBD);

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
        /// Atualiza ClienteEndereco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cClienteEndereco (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tClienteEndereco SET ClienteID = @001, CEP = '@002', Endereco = '@003', Numero = '@004', Cidade = '@005', Estado = '@006', Complemento = '@007', Bairro = '@008', Nome = '@009', CPF = '@010', RG = '@011', EnderecoTipoID = @012, EnderecoPrincipal = '@013', StatusConsulta = @014 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ClienteID.ValorBD);
                sql.Replace("@002", this.CEP.ValorBD);
                sql.Replace("@003", this.Endereco.ValorBD);
                sql.Replace("@004", this.Numero.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.Complemento.ValorBD);
                sql.Replace("@008", this.Bairro.ValorBD);
                sql.Replace("@009", this.Nome.ValorBD);
                sql.Replace("@010", this.CPF.ValorBD);
                sql.Replace("@011", this.RG.ValorBD);
                sql.Replace("@012", this.EnderecoTipoID.ValorBD);
                sql.Replace("@013", this.EnderecoPrincipal.ValorBD);
                sql.Replace("@014", this.StatusConsulta.ValorBD);

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
        /// Exclui ClienteEndereco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cClienteEndereco (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tClienteEndereco WHERE ID=" + id;

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
        /// Exclui ClienteEndereco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cClienteEndereco (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tClienteEndereco WHERE ID=" + id;

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
        /// Exclui ClienteEndereco
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

            this.ClienteID.Limpar();
            this.CEP.Limpar();
            this.Endereco.Limpar();
            this.Numero.Limpar();
            this.Cidade.Limpar();
            this.Estado.Limpar();
            this.Complemento.Limpar();
            this.Bairro.Limpar();
            this.Nome.Limpar();
            this.CPF.Limpar();
            this.RG.Limpar();
            this.EnderecoTipoID.Limpar();
            this.EnderecoPrincipal.Limpar();
            this.StatusConsulta.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.ClienteID.Desfazer();
            this.CEP.Desfazer();
            this.Endereco.Desfazer();
            this.Numero.Desfazer();
            this.Cidade.Desfazer();
            this.Estado.Desfazer();
            this.Complemento.Desfazer();
            this.Bairro.Desfazer();
            this.Nome.Desfazer();
            this.CPF.Desfazer();
            this.RG.Desfazer();
            this.EnderecoTipoID.Desfazer();
            this.EnderecoPrincipal.Desfazer();
            this.StatusConsulta.Desfazer();
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

        public class cep : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CEP";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
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

        public class endereco : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Endereco";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 60;
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

        public class numero : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Numero";
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

        public class estado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
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

        public class complemento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Complemento";
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

        public class bairro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Bairro";
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

        public class cpf : TextProperty
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

        public class rg : TextProperty
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

        public class enderecotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnderecoTipoID";
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

        public class enderecoprincipal : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EnderecoPrincipal";
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

        public class statusconsulta : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "StatusConsulta";
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

                DataTable tabela = new DataTable("ClienteEndereco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Numero", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("EnderecoTipoID", typeof(int));
                tabela.Columns.Add("EnderecoPrincipal", typeof(bool));
                tabela.Columns.Add("StatusConsulta", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
    #endregion

    #region "ClienteEnderecoLista_B"

    public abstract class ClienteEnderecoLista_B : BaseLista
    {

        private bool backup = false;
        protected ClienteEndereco clienteEndereco;

        // passar o Usuario logado no sistema
        public ClienteEnderecoLista_B()
        {
            clienteEndereco = new ClienteEndereco();
        }

        // passar o Usuario logado no sistema
        public ClienteEnderecoLista_B(int usuarioIDLogado)
        {
            clienteEndereco = new ClienteEndereco(usuarioIDLogado);
        }

        public ClienteEndereco ClienteEndereco
        {
            get { return clienteEndereco; }
        }

        /// <summary>
        /// Retorna um IBaseBD de ClienteEndereco especifico
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
                    clienteEndereco.Ler(id);
                    return clienteEndereco;
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
                    sql = "SELECT ID FROM tClienteEndereco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tClienteEndereco";

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
                    sql = "SELECT ID FROM tClienteEndereco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tClienteEndereco";

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
                    sql = "SELECT ID FROM xClienteEndereco";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xClienteEndereco";

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
        /// Preenche ClienteEndereco corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    clienteEndereco.Ler(id);
                else
                    clienteEndereco.LerBackup(id);

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

                bool ok = clienteEndereco.Excluir();
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
        /// Inseri novo(a) ClienteEndereco na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = clienteEndereco.Inserir();
                if (ok)
                {
                    lista.Add(clienteEndereco.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de ClienteEndereco carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("ClienteEndereco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ClienteID", typeof(int));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Numero", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("EnderecoTipoID", typeof(int));
                tabela.Columns.Add("EnderecoPrincipal", typeof(bool));
                tabela.Columns.Add("StatusConsulta", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = clienteEndereco.Control.ID;
                        linha["ClienteID"] = clienteEndereco.ClienteID.Valor;
                        linha["CEP"] = clienteEndereco.CEP.Valor;
                        linha["Endereco"] = clienteEndereco.Endereco.Valor;
                        linha["Numero"] = clienteEndereco.Numero.Valor;
                        linha["Cidade"] = clienteEndereco.Cidade.Valor;
                        linha["Estado"] = clienteEndereco.Estado.Valor;
                        linha["Complemento"] = clienteEndereco.Complemento.Valor;
                        linha["Bairro"] = clienteEndereco.Bairro.Valor;
                        linha["Nome"] = clienteEndereco.Nome.Valor;
                        linha["CPF"] = clienteEndereco.CPF.Valor;
                        linha["RG"] = clienteEndereco.RG.Valor;
                        linha["EnderecoTipoID"] = clienteEndereco.EnderecoTipoID.Valor;
                        linha["EnderecoPrincipal"] = clienteEndereco.EnderecoPrincipal.Valor;
                        linha["StatusConsulta"] = clienteEndereco.StatusConsulta;
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

                DataTable tabela = new DataTable("RelatorioClienteEndereco");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("ClienteID", typeof(int));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("Endereco", typeof(string));
                    tabela.Columns.Add("Numero", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("Complemento", typeof(string));
                    tabela.Columns.Add("Bairro", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));
                    tabela.Columns.Add("RG", typeof(string));
                    tabela.Columns.Add("EnderecoTipoID", typeof(int));
                    tabela.Columns.Add("EnderecoPrincipal", typeof(bool));
                    tabela.Columns.Add("StatusConsulta", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ClienteID"] = clienteEndereco.ClienteID.Valor;
                        linha["CEP"] = clienteEndereco.CEP.Valor;
                        linha["Endereco"] = clienteEndereco.Endereco.Valor;
                        linha["Numero"] = clienteEndereco.Numero.Valor;
                        linha["Cidade"] = clienteEndereco.Cidade.Valor;
                        linha["Estado"] = clienteEndereco.Estado.Valor;
                        linha["Complemento"] = clienteEndereco.Complemento.Valor;
                        linha["Bairro"] = clienteEndereco.Bairro.Valor;
                        linha["Nome"] = clienteEndereco.Nome.Valor;
                        linha["CPF"] = clienteEndereco.CPF.Valor;
                        linha["RG"] = clienteEndereco.RG.Valor;
                        linha["EnderecoTipoID"] = clienteEndereco.EnderecoTipoID.Valor;
                        linha["EnderecoPrincipal"] = clienteEndereco.EnderecoPrincipal.Valor;
                        linha["StatusConsulta"] = clienteEndereco.StatusConsulta;
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
                    case "ClienteID":
                        sql = "SELECT ID, ClienteID FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY ClienteID";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "Endereco":
                        sql = "SELECT ID, Endereco FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Endereco";
                        break;
                    case "Numero":
                        sql = "SELECT ID, Numero FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Numero";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "Complemento":
                        sql = "SELECT ID, Complemento FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Complemento";
                        break;
                    case "Bairro":
                        sql = "SELECT ID, Bairro FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Bairro";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "RG":
                        sql = "SELECT ID, RG FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY RG";
                        break;
                    case "EnderecoTipoID":
                        sql = "SELECT ID, EnderecoTipoID FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY EnderecoTipoID";
                        break;
                    case "EnderecoPrincipal":
                        sql = "SELECT ID, EnderecoPrincipal FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY EnderecoPrincipal";
                        break;
                    case "StatusConsulta":
                        sql = "SELECT ID, StatusConsulta FROM tClienteEndereco WHERE " + FiltroSQL + " ORDER BY StatusConsulta";
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

    #region "ClienteEnderecoException"

    [Serializable]
    public class ClienteEnderecoException : Exception
    {

        public ClienteEnderecoException() : base() { }

        public ClienteEnderecoException(string msg) : base(msg) { }

        public ClienteEnderecoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}