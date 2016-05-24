/******************************************************
* Arquivo LojaDB.cs
* Gerado em: 14/11/2012
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Loja_B"

    public abstract class Loja_B : BaseBD
    {

        public estoqueid EstoqueID = new estoqueid();
        public canalid CanalID = new canalid();
        public nome Nome = new nome();
        public endereco Endereco = new endereco();
        public cidade Cidade = new cidade();
        public estado Estado = new estado();
        public cep CEP = new cep();
        public dddtelefone DDDTelefone = new dddtelefone();
        public telefone Telefone = new telefone();
        public email Email = new email();
        public teftipo TEFTipo = new teftipo();
        public nroestabelecimento NroEstabelecimento = new nroestabelecimento();
        public obs Obs = new obs();
        public numeropos NumeroPOS = new numeropos();
        public usuarioposid UsuarioPosID = new usuarioposid();

        public Loja_B() { }

        // passar o Usuario logado no sistema
        public Loja_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Loja
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tLoja WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstoqueID.ValorBD = bd.LerInt("EstoqueID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.TEFTipo.ValorBD = bd.LerString("TEFTipo");
                    this.NroEstabelecimento.ValorBD = bd.LerString("NroEstabelecimento");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.NumeroPOS.ValorBD = bd.LerString("NumeroPOS");
                    this.UsuarioPosID.ValorBD = bd.LerInt("UsuarioPosID").ToString();
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
        /// Preenche todos os atributos de Loja do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xLoja WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EstoqueID.ValorBD = bd.LerInt("EstoqueID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.TEFTipo.ValorBD = bd.LerString("TEFTipo");
                    this.NroEstabelecimento.ValorBD = bd.LerString("NroEstabelecimento");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.NumeroPOS.ValorBD = bd.LerString("NumeroPOS");
                    this.UsuarioPosID.ValorBD = bd.LerInt("UsuarioPosID").ToString();
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
                sql.Append("INSERT INTO cLoja (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xLoja (ID, Versao, EstoqueID, CanalID, Nome, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, TEFTipo, NroEstabelecimento, Obs, NumeroPOS, UsuarioPosID) ");
                sql.Append("SELECT ID, @V, EstoqueID, CanalID, Nome, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, TEFTipo, NroEstabelecimento, Obs, NumeroPOS, UsuarioPosID FROM tLoja WHERE ID = @I");
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
        /// Inserir novo(a) Loja
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLoja");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLoja(ID, EstoqueID, CanalID, Nome, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, TEFTipo, NroEstabelecimento, Obs, NumeroPOS, UsuarioPosID) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014',@015)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstoqueID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Endereco.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.CEP.ValorBD);
                sql.Replace("@008", this.DDDTelefone.ValorBD);
                sql.Replace("@009", this.Telefone.ValorBD);
                sql.Replace("@010", this.Email.ValorBD);
                sql.Replace("@011", this.TEFTipo.ValorBD);
                sql.Replace("@012", this.NroEstabelecimento.ValorBD);
                sql.Replace("@013", this.Obs.ValorBD);
                sql.Replace("@014", this.NumeroPOS.ValorBD);
                sql.Replace("@015", this.UsuarioPosID.ValorBD);

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
        /// Inserir novo(a) Loja
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLoja");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLoja(ID, EstoqueID, CanalID, Nome, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, Email, TEFTipo, NroEstabelecimento, Obs, NumeroPOS, UsuarioPosID) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014',@015)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstoqueID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Endereco.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.CEP.ValorBD);
                sql.Replace("@008", this.DDDTelefone.ValorBD);
                sql.Replace("@009", this.Telefone.ValorBD);
                sql.Replace("@010", this.Email.ValorBD);
                sql.Replace("@011", this.TEFTipo.ValorBD);
                sql.Replace("@012", this.NroEstabelecimento.ValorBD);
                sql.Replace("@013", this.Obs.ValorBD);
                sql.Replace("@014", this.NumeroPOS.ValorBD);
                sql.Replace("@015", this.UsuarioPosID.ValorBD);

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
        /// Atualiza Loja
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cLoja WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLoja SET EstoqueID = @001, CanalID = @002, Nome = '@003', Endereco = '@004', Cidade = '@005', Estado = '@006', CEP = '@007', DDDTelefone = '@008', Telefone = '@009', Email = '@010', TEFTipo = '@011', NroEstabelecimento = '@012', Obs = '@013', NumeroPOS = '@014', UsuarioPosID = @015 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstoqueID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Endereco.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.CEP.ValorBD);
                sql.Replace("@008", this.DDDTelefone.ValorBD);
                sql.Replace("@009", this.Telefone.ValorBD);
                sql.Replace("@010", this.Email.ValorBD);
                sql.Replace("@011", this.TEFTipo.ValorBD);
                sql.Replace("@012", this.NroEstabelecimento.ValorBD);
                sql.Replace("@013", this.Obs.ValorBD);
                sql.Replace("@014", this.NumeroPOS.ValorBD);
                sql.Replace("@015", this.UsuarioPosID.ValorBD);

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
        /// Atualiza Loja
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cLoja WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLoja SET EstoqueID = @001, CanalID = @002, Nome = '@003', Endereco = '@004', Cidade = '@005', Estado = '@006', CEP = '@007', DDDTelefone = '@008', Telefone = '@009', Email = '@010', TEFTipo = '@011', NroEstabelecimento = '@012', Obs = '@013', NumeroPOS = '@014', UsuarioPosID = @015 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EstoqueID.ValorBD);
                sql.Replace("@002", this.CanalID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Endereco.ValorBD);
                sql.Replace("@005", this.Cidade.ValorBD);
                sql.Replace("@006", this.Estado.ValorBD);
                sql.Replace("@007", this.CEP.ValorBD);
                sql.Replace("@008", this.DDDTelefone.ValorBD);
                sql.Replace("@009", this.Telefone.ValorBD);
                sql.Replace("@010", this.Email.ValorBD);
                sql.Replace("@011", this.TEFTipo.ValorBD);
                sql.Replace("@012", this.NroEstabelecimento.ValorBD);
                sql.Replace("@013", this.Obs.ValorBD);
                sql.Replace("@014", this.NumeroPOS.ValorBD);
                sql.Replace("@015", this.UsuarioPosID.ValorBD);

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
        /// Exclui Loja com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLoja WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLoja WHERE ID=" + id;

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
        /// Exclui Loja com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLoja WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLoja WHERE ID=" + id;

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
        /// Exclui Loja
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

            this.EstoqueID.Limpar();
            this.CanalID.Limpar();
            this.Nome.Limpar();
            this.Endereco.Limpar();
            this.Cidade.Limpar();
            this.Estado.Limpar();
            this.CEP.Limpar();
            this.DDDTelefone.Limpar();
            this.Telefone.Limpar();
            this.Email.Limpar();
            this.TEFTipo.Limpar();
            this.NroEstabelecimento.Limpar();
            this.Obs.Limpar();
            this.NumeroPOS.Limpar();
            this.UsuarioPosID.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EstoqueID.Desfazer();
            this.CanalID.Desfazer();
            this.Nome.Desfazer();
            this.Endereco.Desfazer();
            this.Cidade.Desfazer();
            this.Estado.Desfazer();
            this.CEP.Desfazer();
            this.DDDTelefone.Desfazer();
            this.Telefone.Desfazer();
            this.Email.Desfazer();
            this.TEFTipo.Desfazer();
            this.NroEstabelecimento.Desfazer();
            this.Obs.Desfazer();
            this.NumeroPOS.Desfazer();
            this.UsuarioPosID.Desfazer();
        }

        public class estoqueid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstoqueID";
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

        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
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
                    return 70;
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

        public class dddtelefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDTelefone";
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

        public class telefone : TextProperty
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

        public class email : TextProperty
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

        public class teftipo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TEFTipo";
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

        public class nroestabelecimento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NroEstabelecimento";
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

        public class numeropos : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroPOS";
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

        public class usuarioposid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioPosID";
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

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstoqueID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("TEFTipo", typeof(string));
                tabela.Columns.Add("NroEstabelecimento", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("NumeroPOS", typeof(string));
                tabela.Columns.Add("UsuarioPosID", typeof(int));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Papeis();

        public abstract DataTable Caixas(string registrozero);

        public abstract DataTable Caixas(string registrozero, int usuarioid);

        public abstract DataTable Usuarios(string registrozero);

        public abstract int QtdeIngressos();

        public abstract int QtdeIngressos(int apresentacaosetorid);

        public abstract int QtdeIngressos(int apresentacaosetorid, int precoid);

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorLoja(string ingressologids);

        public abstract decimal ValorIngressosPorLoja(string ingressologids);

    }
    #endregion

    #region "LojaLista_B"

    public abstract class LojaLista_B : BaseLista
    {

        private bool backup = false;
        protected Loja loja;

        // passar o Usuario logado no sistema
        public LojaLista_B()
        {
            loja = new Loja();
        }

        // passar o Usuario logado no sistema
        public LojaLista_B(int usuarioIDLogado)
        {
            loja = new Loja(usuarioIDLogado);
        }

        public Loja Loja
        {
            get { return loja; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Loja especifico
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
                    loja.Ler(id);
                    return loja;
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
                    sql = "SELECT ID FROM tLoja";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLoja";

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
                    sql = "SELECT ID FROM tLoja";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLoja";

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
                    sql = "SELECT ID FROM xLoja";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xLoja";

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
        /// Preenche Loja corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    loja.Ler(id);
                else
                    loja.LerBackup(id);

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

                bool ok = loja.Excluir();
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
        /// Inseri novo(a) Loja na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = loja.Inserir();
                if (ok)
                {
                    lista.Add(loja.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Loja carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Loja");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EstoqueID", typeof(int));
                tabela.Columns.Add("CanalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("TEFTipo", typeof(string));
                tabela.Columns.Add("NroEstabelecimento", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("NumeroPOS", typeof(string));
                tabela.Columns.Add("UsuarioPosID", typeof(int));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = loja.Control.ID;
                        linha["EstoqueID"] = loja.EstoqueID.Valor;
                        linha["CanalID"] = loja.CanalID.Valor;
                        linha["Nome"] = loja.Nome.Valor;
                        linha["Endereco"] = loja.Endereco.Valor;
                        linha["Cidade"] = loja.Cidade.Valor;
                        linha["Estado"] = loja.Estado.Valor;
                        linha["CEP"] = loja.CEP.Valor;
                        linha["DDDTelefone"] = loja.DDDTelefone.Valor;
                        linha["Telefone"] = loja.Telefone.Valor;
                        linha["Email"] = loja.Email.Valor;
                        linha["TEFTipo"] = loja.TEFTipo.Valor;
                        linha["NroEstabelecimento"] = loja.NroEstabelecimento.Valor;
                        linha["Obs"] = loja.Obs.Valor;
                        linha["NumeroPOS"] = loja.NumeroPOS.Valor;
                        linha["UsuarioPosID"] = loja.UsuarioPosID.Valor;
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

                DataTable tabela = new DataTable("RelatorioLoja");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EstoqueID", typeof(int));
                    tabela.Columns.Add("CanalID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Endereco", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("DDDTelefone", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("TEFTipo", typeof(string));
                    tabela.Columns.Add("NroEstabelecimento", typeof(string));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("NumeroPOS", typeof(string));
                    tabela.Columns.Add("UsuarioPosID", typeof(int));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EstoqueID"] = loja.EstoqueID.Valor;
                        linha["CanalID"] = loja.CanalID.Valor;
                        linha["Nome"] = loja.Nome.Valor;
                        linha["Endereco"] = loja.Endereco.Valor;
                        linha["Cidade"] = loja.Cidade.Valor;
                        linha["Estado"] = loja.Estado.Valor;
                        linha["CEP"] = loja.CEP.Valor;
                        linha["DDDTelefone"] = loja.DDDTelefone.Valor;
                        linha["Telefone"] = loja.Telefone.Valor;
                        linha["Email"] = loja.Email.Valor;
                        linha["TEFTipo"] = loja.TEFTipo.Valor;
                        linha["NroEstabelecimento"] = loja.NroEstabelecimento.Valor;
                        linha["Obs"] = loja.Obs.Valor;
                        linha["NumeroPOS"] = loja.NumeroPOS.Valor;
                        linha["UsuarioPosID"] = loja.UsuarioPosID.Valor;
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
                    case "EstoqueID":
                        sql = "SELECT ID, EstoqueID FROM tLoja WHERE " + FiltroSQL + " ORDER BY EstoqueID";
                        break;
                    case "CanalID":
                        sql = "SELECT ID, CanalID FROM tLoja WHERE " + FiltroSQL + " ORDER BY CanalID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tLoja WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Endereco":
                        sql = "SELECT ID, Endereco FROM tLoja WHERE " + FiltroSQL + " ORDER BY Endereco";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tLoja WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tLoja WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tLoja WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "DDDTelefone":
                        sql = "SELECT ID, DDDTelefone FROM tLoja WHERE " + FiltroSQL + " ORDER BY DDDTelefone";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tLoja WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tLoja WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "TEFTipo":
                        sql = "SELECT ID, TEFTipo FROM tLoja WHERE " + FiltroSQL + " ORDER BY TEFTipo";
                        break;
                    case "NroEstabelecimento":
                        sql = "SELECT ID, NroEstabelecimento FROM tLoja WHERE " + FiltroSQL + " ORDER BY NroEstabelecimento";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tLoja WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "NumeroPOS":
                        sql = "SELECT ID, NumeroPOS FROM tLoja WHERE " + FiltroSQL + " ORDER BY NumeroPOS";
                        break;
                    case "UsuarioPosID":
                        sql = "SELECT ID, UsuarioPosID FROM tLoja WHERE " + FiltroSQL + " ORDER BY UsuarioPosID";
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

    #region "LojaException"

    [Serializable]
    public class LojaException : Exception
    {

        public LojaException() : base() { }

        public LojaException(string msg) : base(msg) { }

        public LojaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}