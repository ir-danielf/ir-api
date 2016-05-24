/******************************************************
* Arquivo EmpresaDB.cs
* Gerado em: 28/01/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "Empresa_B"

    public abstract class Empresa_B : BaseBD
    {

        public regionalid RegionalID = new regionalid();
        public nome Nome = new nome();
        public contatonome ContatoNome = new contatonome();
        public contatocargo ContatoCargo = new contatocargo();
        public endereco Endereco = new endereco();
        public cidade Cidade = new cidade();
        public estado Estado = new estado();
        public cep CEP = new cep();
        public dddtelefone DDDTelefone = new dddtelefone();
        public telefone Telefone = new telefone();
        public dddfax DDDFax = new dddfax();
        public fax Fax = new fax();
        public empresapromove EmpresaPromove = new empresapromove();
        public empresavende EmpresaVende = new empresavende();
        public email Email = new email();
        public website Website = new website();
        public obs Obs = new obs();
        public taxamaximaempresa TaxaMaximaEmpresa = new taxamaximaempresa();
        public bannerpadraosite BannerPadraoSite = new bannerpadraosite();
        public ativo Ativo = new ativo();

        public Empresa_B() { }

        // passar o Usuario logado no sistema
        public Empresa_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Empresa
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEmpresa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.RegionalID.ValorBD = bd.LerInt("RegionalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.ContatoNome.ValorBD = bd.LerString("ContatoNome");
                    this.ContatoCargo.ValorBD = bd.LerString("ContatoCargo");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.DDDFax.ValorBD = bd.LerString("DDDFax");
                    this.Fax.ValorBD = bd.LerString("Fax");
                    this.EmpresaPromove.ValorBD = bd.LerString("EmpresaPromove");
                    this.EmpresaVende.ValorBD = bd.LerString("EmpresaVende");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Website.ValorBD = bd.LerString("Website");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.TaxaMaximaEmpresa.ValorBD = bd.LerDecimal("TaxaMaximaEmpresa").ToString();
                    this.BannerPadraoSite.ValorBD = bd.LerString("BannerPadraoSite");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
        /// Preenche todos os atributos de Empresa do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEmpresa WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.RegionalID.ValorBD = bd.LerInt("RegionalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.ContatoNome.ValorBD = bd.LerString("ContatoNome");
                    this.ContatoCargo.ValorBD = bd.LerString("ContatoCargo");
                    this.Endereco.ValorBD = bd.LerString("Endereco");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.DDDFax.ValorBD = bd.LerString("DDDFax");
                    this.Fax.ValorBD = bd.LerString("Fax");
                    this.EmpresaPromove.ValorBD = bd.LerString("EmpresaPromove");
                    this.EmpresaVende.ValorBD = bd.LerString("EmpresaVende");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.Website.ValorBD = bd.LerString("Website");
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.TaxaMaximaEmpresa.ValorBD = bd.LerDecimal("TaxaMaximaEmpresa").ToString();
                    this.BannerPadraoSite.ValorBD = bd.LerString("BannerPadraoSite");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
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
                sql.Append("INSERT INTO cEmpresa (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEmpresa (ID, Versao, RegionalID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, EmpresaPromove, EmpresaVende, Email, Website, Obs, TaxaMaximaEmpresa, BannerPadraoSite, Ativo) ");
                sql.Append("SELECT ID, @V, RegionalID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, EmpresaPromove, EmpresaVende, Email, Website, Obs, TaxaMaximaEmpresa, BannerPadraoSite, Ativo FROM tEmpresa WHERE ID = @I");
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
        /// Inserir novo(a) Empresa
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEmpresa");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEmpresa(ID, RegionalID, Nome, ContatoNome, ContatoCargo, Endereco, Cidade, Estado, CEP, DDDTelefone, Telefone, DDDFax, Fax, EmpresaPromove, EmpresaVende, Email, Website, Obs, TaxaMaximaEmpresa, BannerPadraoSite, Ativo) ");
                sql.Append("VALUES (@ID,@001,'@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015','@016','@017','@018','@019', '@020')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.RegionalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.ContatoNome.ValorBD);
                sql.Replace("@004", this.ContatoCargo.ValorBD);
                sql.Replace("@005", this.Endereco.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.DDDFax.ValorBD);
                sql.Replace("@012", this.Fax.ValorBD);
                sql.Replace("@013", this.EmpresaPromove.ValorBD);
                sql.Replace("@014", this.EmpresaVende.ValorBD);
                sql.Replace("@015", this.Email.ValorBD);
                sql.Replace("@016", this.Website.ValorBD);
                sql.Replace("@017", this.Obs.ValorBD);
                sql.Replace("@018", this.TaxaMaximaEmpresa.ValorBD);
                sql.Replace("@019", this.BannerPadraoSite.ValorBD);
                sql.Replace("@020", this.Ativo.ValorBD);

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
        /// Atualiza Empresa
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEmpresa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEmpresa SET RegionalID = @001, Nome = '@002', ContatoNome = '@003', ContatoCargo = '@004', Endereco = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', Telefone = '@010', DDDFax = '@011', Fax = '@012', EmpresaPromove = '@013', EmpresaVende = '@014', Email = '@015', Website = '@016', Obs = '@017', TaxaMaximaEmpresa = '@018', BannerPadraoSite = '@019', Ativo = '@020' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.RegionalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.ContatoNome.ValorBD);
                sql.Replace("@004", this.ContatoCargo.ValorBD);
                sql.Replace("@005", this.Endereco.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.DDDFax.ValorBD);
                sql.Replace("@012", this.Fax.ValorBD);
                sql.Replace("@013", this.EmpresaPromove.ValorBD);
                sql.Replace("@014", this.EmpresaVende.ValorBD);
                sql.Replace("@015", this.Email.ValorBD);
                sql.Replace("@016", this.Website.ValorBD);
                sql.Replace("@017", this.Obs.ValorBD);
                sql.Replace("@018", this.TaxaMaximaEmpresa.ValorBD);
                sql.Replace("@019", this.BannerPadraoSite.ValorBD);
                sql.Replace("@020", this.Ativo.ValorBD);

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
        /// Exclui Empresa com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEmpresa WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEmpresa WHERE ID=" + id;

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
        /// Exclui Empresa
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

            this.RegionalID.Limpar();
            this.Nome.Limpar();
            this.ContatoNome.Limpar();
            this.ContatoCargo.Limpar();
            this.Endereco.Limpar();
            this.Cidade.Limpar();
            this.Estado.Limpar();
            this.CEP.Limpar();
            this.DDDTelefone.Limpar();
            this.Telefone.Limpar();
            this.DDDFax.Limpar();
            this.Fax.Limpar();
            this.EmpresaPromove.Limpar();
            this.EmpresaVende.Limpar();
            this.Email.Limpar();
            this.Website.Limpar();
            this.Obs.Limpar();
            this.TaxaMaximaEmpresa.Limpar();
            this.BannerPadraoSite.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.RegionalID.Desfazer();
            this.Nome.Desfazer();
            this.ContatoNome.Desfazer();
            this.ContatoCargo.Desfazer();
            this.Endereco.Desfazer();
            this.Cidade.Desfazer();
            this.Estado.Desfazer();
            this.CEP.Desfazer();
            this.DDDTelefone.Desfazer();
            this.Telefone.Desfazer();
            this.DDDFax.Desfazer();
            this.Fax.Desfazer();
            this.EmpresaPromove.Desfazer();
            this.EmpresaVende.Desfazer();
            this.Email.Desfazer();
            this.Website.Desfazer();
            this.Obs.Desfazer();
            this.TaxaMaximaEmpresa.Desfazer();
            this.BannerPadraoSite.Desfazer();
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

        public class contatonome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContatoNome";
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

        public class contatocargo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContatoCargo";
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

        public class dddfax : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDFax";
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

        public class fax : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Fax";
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

        public class empresapromove : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaPromove";
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

        public class empresavende : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaVende";
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

        public class website : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Website";
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

        public class taxamaximaempresa : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "TaxaMaximaEmpresa";
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

        public class bannerpadraosite : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "BannerPadraoSite";
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

        public class ativo : TextProperty
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

                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("RegionalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("ContatoNome", typeof(string));
                tabela.Columns.Add("ContatoCargo", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDFax", typeof(string));
                tabela.Columns.Add("Fax", typeof(string));
                tabela.Columns.Add("EmpresaPromove", typeof(bool));
                tabela.Columns.Add("EmpresaVende", typeof(bool));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Website", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("TaxaMaximaEmpresa", typeof(decimal));
                tabela.Columns.Add("BannerPadraoSite", typeof(bool));
                tabela.Columns.Add("Ativo", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable PedidosEstoque();

        public abstract DataTable Fornecedores();

        public abstract DataTable Transferencias();

        public abstract DataTable Ajustes();

        public abstract DataTable AjusteMotivos();

        public abstract DataTable Apresentacoes();

        public abstract DataTable Categorias();

        public abstract DataTable Produtos();

        public abstract DataTable Produtos(int categoriaid);

        public abstract DataTable Locais();

        public abstract DataTable LocaisAtivos();

        public abstract DataTable Todas();

        public abstract DataTable Ativas();

        public abstract DataTable Canais();

        public abstract DataTable CanaisAtivos();

        public abstract DataTable Canais(char tipo);

        public abstract DataTable Logins();

        public abstract DataTable Usuarios();

        public abstract DataTable Garcons(bool comregistrozero);

        public abstract DataTable Garcons();

        public abstract DataTable Eventos();

        public abstract DataTable EventosAtivos();

        public abstract DataTable Pacotes();

        public abstract DataTable Precos();

        public abstract DataTable Lojas();

        public abstract DataTable Estoques();

        public abstract DataTable CanaisQueVendem(string registrozero);

        public abstract DataTable EventosQueVendem(string registrozero);

    }
    #endregion

    #region "EmpresaLista_B"

    public abstract class EmpresaLista_B : BaseLista
    {

        private bool backup = false;
        protected Empresa empresa;

        // passar o Usuario logado no sistema
        public EmpresaLista_B()
        {
            empresa = new Empresa();
        }

        // passar o Usuario logado no sistema
        public EmpresaLista_B(int usuarioIDLogado)
        {
            empresa = new Empresa(usuarioIDLogado);
        }

        public Empresa Empresa
        {
            get { return empresa; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Empresa especifico
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
                    empresa.Ler(id);
                    return empresa;
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
                    sql = "SELECT ID FROM tEmpresa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEmpresa";

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
                    sql = "SELECT ID FROM tEmpresa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEmpresa";

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
                    sql = "SELECT ID FROM xEmpresa";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEmpresa";

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
        /// Preenche Empresa corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    empresa.Ler(id);
                else
                    empresa.LerBackup(id);

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

                bool ok = empresa.Excluir();
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
        /// Inseri novo(a) Empresa na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = empresa.Inserir();
                if (ok)
                {
                    lista.Add(empresa.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Empresa carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Empresa");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("RegionalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("ContatoNome", typeof(string));
                tabela.Columns.Add("ContatoCargo", typeof(string));
                tabela.Columns.Add("Endereco", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDFax", typeof(string));
                tabela.Columns.Add("Fax", typeof(string));
                tabela.Columns.Add("EmpresaPromove", typeof(bool));
                tabela.Columns.Add("EmpresaVende", typeof(bool));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("Website", typeof(string));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("TaxaMaximaEmpresa", typeof(decimal));
                tabela.Columns.Add("BannerPadraoSite", typeof(bool));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = empresa.Control.ID;
                        linha["RegionalID"] = empresa.RegionalID.Valor;
                        linha["Nome"] = empresa.Nome.Valor;
                        linha["ContatoNome"] = empresa.ContatoNome.Valor;
                        linha["ContatoCargo"] = empresa.ContatoCargo.Valor;
                        linha["Endereco"] = empresa.Endereco.Valor;
                        linha["Cidade"] = empresa.Cidade.Valor;
                        linha["Estado"] = empresa.Estado.Valor;
                        linha["CEP"] = empresa.CEP.Valor;
                        linha["DDDTelefone"] = empresa.DDDTelefone.Valor;
                        linha["Telefone"] = empresa.Telefone.Valor;
                        linha["DDDFax"] = empresa.DDDFax.Valor;
                        linha["Fax"] = empresa.Fax.Valor;
                        linha["EmpresaPromove"] = empresa.EmpresaPromove.Valor;
                        linha["EmpresaVende"] = empresa.EmpresaVende.Valor;
                        linha["Email"] = empresa.Email.Valor;
                        linha["Website"] = empresa.Website.Valor;
                        linha["Obs"] = empresa.Obs.Valor;
                        linha["TaxaMaximaEmpresa"] = empresa.TaxaMaximaEmpresa.Valor;
                        linha["BannerPadraoSite"] = empresa.BannerPadraoSite.Valor;
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

                DataTable tabela = new DataTable("RelatorioEmpresa");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("RegionalID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("ContatoNome", typeof(string));
                    tabela.Columns.Add("ContatoCargo", typeof(string));
                    tabela.Columns.Add("Endereco", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("DDDTelefone", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("DDDFax", typeof(string));
                    tabela.Columns.Add("Fax", typeof(string));
                    tabela.Columns.Add("EmpresaPromove", typeof(bool));
                    tabela.Columns.Add("EmpresaVende", typeof(bool));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("Website", typeof(string));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("TaxaMaximaEmpresa", typeof(decimal));
                    tabela.Columns.Add("BannerPadraoSite", typeof(bool));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["RegionalID"] = empresa.RegionalID.Valor;
                        linha["Nome"] = empresa.Nome.Valor;
                        linha["ContatoNome"] = empresa.ContatoNome.Valor;
                        linha["ContatoCargo"] = empresa.ContatoCargo.Valor;
                        linha["Endereco"] = empresa.Endereco.Valor;
                        linha["Cidade"] = empresa.Cidade.Valor;
                        linha["Estado"] = empresa.Estado.Valor;
                        linha["CEP"] = empresa.CEP.Valor;
                        linha["DDDTelefone"] = empresa.DDDTelefone.Valor;
                        linha["Telefone"] = empresa.Telefone.Valor;
                        linha["DDDFax"] = empresa.DDDFax.Valor;
                        linha["Fax"] = empresa.Fax.Valor;
                        linha["EmpresaPromove"] = empresa.EmpresaPromove.Valor;
                        linha["EmpresaVende"] = empresa.EmpresaVende.Valor;
                        linha["Email"] = empresa.Email.Valor;
                        linha["Website"] = empresa.Website.Valor;
                        linha["Obs"] = empresa.Obs.Valor;
                        linha["TaxaMaximaEmpresa"] = empresa.TaxaMaximaEmpresa.Valor;
                        linha["BannerPadraoSite"] = empresa.BannerPadraoSite.Valor;
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
                    case "RegionalID":
                        sql = "SELECT ID, RegionalID FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY RegionalID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "ContatoNome":
                        sql = "SELECT ID, ContatoNome FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY ContatoNome";
                        break;
                    case "ContatoCargo":
                        sql = "SELECT ID, ContatoCargo FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY ContatoCargo";
                        break;
                    case "Endereco":
                        sql = "SELECT ID, Endereco FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Endereco";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "DDDTelefone":
                        sql = "SELECT ID, DDDTelefone FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY DDDTelefone";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "DDDFax":
                        sql = "SELECT ID, DDDFax FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY DDDFax";
                        break;
                    case "Fax":
                        sql = "SELECT ID, Fax FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Fax";
                        break;
                    case "EmpresaPromove":
                        sql = "SELECT ID, EmpresaPromove FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY EmpresaPromove";
                        break;
                    case "EmpresaVende":
                        sql = "SELECT ID, EmpresaVende FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY EmpresaVende";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "Website":
                        sql = "SELECT ID, Website FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Website";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "TaxaMaximaEmpresa":
                        sql = "SELECT ID, TaxaMaximaEmpresa FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY TaxaMaximaEmpresa";
                        break;
                    case "BannerPadraoSite":
                        sql = "SELECT ID, BannerPadraoSite FROM tEmpresa WHERE " + FiltroSQL + " ORDER BY BannerPadraoSite";
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

    #region "EmpresaException"

    [Serializable]
    public class EmpresaException : Exception
    {

        public EmpresaException() : base() { }

        public EmpresaException(string msg) : base(msg) { }

        public EmpresaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}