/******************************************************
* Arquivo ClienteDB.cs
* Gerado em: 06/10/2011
* Autor: Celeritas Ltda
*******************************************************/

using CTLib;
using System;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    #region "Cliente_B"

    public abstract class Cliente_B : BaseBD
    {

        public nome Nome = new nome();
        public rg RG = new rg();
        public cpf CPF = new cpf();
        public carteiraestudante CarteiraEstudante = new carteiraestudante();
        public sexo Sexo = new sexo();
        public dddtelefone DDDTelefone = new dddtelefone();
        public telefone Telefone = new telefone();
        public dddtelefonecomercial DDDTelefoneComercial = new dddtelefonecomercial();
        public telefonecomercial TelefoneComercial = new telefonecomercial();
        public dddcelular DDDCelular = new dddcelular();
        public celular Celular = new celular();
        public datanascimento DataNascimento = new datanascimento();
        public email Email = new email();
        public recebeemail RecebeEmail = new recebeemail();
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
        public loginosesp LoginOsesp = new loginosesp();
        public estadocliente EstadoCliente = new estadocliente();
        public clienteindicacaoid ClienteIndicacaoID = new clienteindicacaoid();
        public obs Obs = new obs();
        public senha Senha = new senha();
        public ativo Ativo = new ativo();
        public statusatual StatusAtual = new statusatual();
        public nomeentrega NomeEntrega = new nomeentrega();
        public cpfentrega CPFEntrega = new cpfentrega();
        public rgentrega RGEntrega = new rgentrega();
        public cpfconsultado CPFConsultado = new cpfconsultado();
        public nomeconsultado NomeConsultado = new nomeconsultado();
        public statusconsulta StatusConsulta = new statusconsulta();
        public cpfconsultadoentrega CPFConsultadoEntrega = new cpfconsultadoentrega();
        public nomeconsultadoentrega NomeConsultadoEntrega = new nomeconsultadoentrega();
        public statusconsultaentrega StatusConsultaEntrega = new statusconsultaentrega();
        public pais Pais = new pais();
        public cpfresponsavel CPFResponsavel = new cpfresponsavel();
        public contatotipoid ContatoTipoID = new contatotipoid();
        public nivelcliente NivelCliente = new nivelcliente();
        public cnpj CNPJ = new cnpj();
        public nomefantasia NomeFantasia = new nomefantasia();
        public razaosocial RazaoSocial = new razaosocial();
        public inscricaoestadual InscricaoEstadual = new inscricaoestadual();
        public tipocadastro TipoCadastro = new tipocadastro();
        public situacaoprofissionalid SituacaoProfissionalID = new situacaoprofissionalid();
        public profissao Profissao = new profissao();
        public dddtelefonecomercial2 DDDTelefoneComercial2 = new dddtelefonecomercial2();
        public telefonecomercial2 TelefoneComercial2 = new telefonecomercial2();

        public Cliente_B() { }

        // passar o Usuario logado no sistema
        public Cliente_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Cliente
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tCliente WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.CarteiraEstudante.ValorBD = bd.LerString("CarteiraEstudante");
                    this.Sexo.ValorBD = bd.LerString("Sexo");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.DDDTelefoneComercial.ValorBD = bd.LerString("DDDTelefoneComercial");
                    this.TelefoneComercial.ValorBD = bd.LerString("TelefoneComercial");
                    this.DDDCelular.ValorBD = bd.LerString("DDDCelular");
                    this.Celular.ValorBD = bd.LerString("Celular");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.RecebeEmail.ValorBD = bd.LerString("RecebeEmail");
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
                    this.LoginOsesp.ValorBD = bd.LerString("LoginOsesp");
                    this.EstadoCliente.ValorBD = bd.LerString("EstadoCliente");
                    this.ClienteIndicacaoID.ValorBD = bd.LerInt("ClienteIndicacaoID").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Senha.ValorBD = bd.LerString("Senha");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.StatusAtual.ValorBD = bd.LerString("StatusAtual");
                    this.NomeEntrega.ValorBD = bd.LerString("NomeEntrega");
                    this.CPFEntrega.ValorBD = bd.LerString("CPFEntrega");
                    this.RGEntrega.ValorBD = bd.LerString("RGEntrega");
                    this.CPFConsultado.ValorBD = bd.LerString("CPFConsultado");
                    this.NomeConsultado.ValorBD = bd.LerString("NomeConsultado");
                    this.StatusConsulta.ValorBD = bd.LerInt("StatusConsulta").ToString();
                    this.CPFConsultadoEntrega.ValorBD = bd.LerString("CPFConsultadoEntrega");
                    this.NomeConsultadoEntrega.ValorBD = bd.LerString("NomeConsultadoEntrega");
                    this.StatusConsultaEntrega.ValorBD = bd.LerInt("StatusConsultaEntrega").ToString();
                    this.Pais.ValorBD = bd.LerString("Pais");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.ContatoTipoID.ValorBD = bd.LerInt("ContatoTipoID").ToString();
                    this.NivelCliente.ValorBD = bd.LerInt("NivelCliente").ToString();
                    this.CNPJ.ValorBD = bd.LerString("CNPJ");
                    this.NomeFantasia.ValorBD = bd.LerString("NomeFantasia");
                    this.RazaoSocial.ValorBD = bd.LerString("RazaoSocial");
                    this.InscricaoEstadual.ValorBD = bd.LerString("InscricaoEstadual");
                    this.TipoCadastro.ValorBD = bd.LerString("TipoCadastro");
                    this.SituacaoProfissionalID.ValorBD = bd.LerInt("SituacaoProfissionalID").ToString();
                    this.Profissao.ValorBD = bd.LerString("Profissao");
                    this.DDDTelefoneComercial2.ValorBD = bd.LerString("DDDTelefoneComercial2");
                    this.TelefoneComercial2.ValorBD = bd.LerString("TelefoneComercial2");
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
        /// Preenche todos os atributos de Cliente do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xCliente WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.RG.ValorBD = bd.LerString("RG");
                    this.CPF.ValorBD = bd.LerString("CPF");
                    this.CarteiraEstudante.ValorBD = bd.LerString("CarteiraEstudante");
                    this.Sexo.ValorBD = bd.LerString("Sexo");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.DDDTelefoneComercial.ValorBD = bd.LerString("DDDTelefoneComercial");
                    this.TelefoneComercial.ValorBD = bd.LerString("TelefoneComercial");
                    this.DDDCelular.ValorBD = bd.LerString("DDDCelular");
                    this.Celular.ValorBD = bd.LerString("Celular");
                    this.DataNascimento.ValorBD = bd.LerString("DataNascimento");
                    this.Email.ValorBD = bd.LerString("Email");
                    this.RecebeEmail.ValorBD = bd.LerString("RecebeEmail");
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
                    this.LoginOsesp.ValorBD = bd.LerString("LoginOsesp");
                    this.EstadoCliente.ValorBD = bd.LerString("EstadoCliente");
                    this.ClienteIndicacaoID.ValorBD = bd.LerInt("ClienteIndicacaoID").ToString();
                    this.Obs.ValorBD = bd.LerString("Obs");
                    this.Senha.ValorBD = bd.LerString("Senha");
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.StatusAtual.ValorBD = bd.LerString("StatusAtual");
                    this.NomeEntrega.ValorBD = bd.LerString("NomeEntrega");
                    this.CPFEntrega.ValorBD = bd.LerString("CPFEntrega");
                    this.RGEntrega.ValorBD = bd.LerString("RGEntrega");
                    this.CPFConsultado.ValorBD = bd.LerString("CPFConsultado");
                    this.NomeConsultado.ValorBD = bd.LerString("NomeConsultado");
                    this.StatusConsulta.ValorBD = bd.LerInt("StatusConsulta").ToString();
                    this.CPFConsultadoEntrega.ValorBD = bd.LerString("CPFConsultadoEntrega");
                    this.NomeConsultadoEntrega.ValorBD = bd.LerString("NomeConsultadoEntrega");
                    this.StatusConsultaEntrega.ValorBD = bd.LerInt("StatusConsultaEntrega").ToString();
                    this.Pais.ValorBD = bd.LerString("Pais");
                    this.CPFResponsavel.ValorBD = bd.LerString("CPFResponsavel");
                    this.ContatoTipoID.ValorBD = bd.LerInt("ContatoTipoID").ToString();
                    this.NivelCliente.ValorBD = bd.LerInt("NivelCliente").ToString();
                    this.CNPJ.ValorBD = bd.LerString("CNPJ");
                    this.NomeFantasia.ValorBD = bd.LerString("NomeFantasia");
                    this.RazaoSocial.ValorBD = bd.LerString("RazaoSocial");
                    this.InscricaoEstadual.ValorBD = bd.LerString("InscricaoEstadual");
                    this.TipoCadastro.ValorBD = bd.LerString("TipoCadastro");
                    this.SituacaoProfissionalID.ValorBD = bd.LerInt("SituacaoProfissionalID").ToString();
                    this.Profissao.ValorBD = bd.LerString("Profissao");
                    this.DDDTelefoneComercial2.ValorBD = bd.LerString("DDDTelefoneComercial2");
                    this.TelefoneComercial2.ValorBD = bd.LerString("TelefoneComercial2");
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
                sql.Append("INSERT INTO cCliente (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xCliente (ID, Versao, Nome, RG, CPF, CarteiraEstudante, Sexo, DDDTelefone, Telefone, DDDTelefoneComercial, TelefoneComercial, DDDCelular, Celular, DataNascimento, Email, RecebeEmail, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, LoginOsesp, EstadoCliente, ClienteIndicacaoID, Obs, Senha, Ativo, StatusAtual, NomeEntrega, CPFEntrega, RGEntrega, CPFConsultado, NomeConsultado, StatusConsulta, CPFConsultadoEntrega, NomeConsultadoEntrega, StatusConsultaEntrega, Pais, CPFResponsavel, ContatoTipoID, NivelCliente, CNPJ, NomeFantasia, RazaoSocial, InscricaoEstadual, TipoCadastro, SituacaoProfissionalID, Profissao, DDDTelefoneComercial2, TelefoneComercial2) ");
                sql.Append("SELECT ID, @V, Nome, RG, CPF, CarteiraEstudante, Sexo, DDDTelefone, Telefone, DDDTelefoneComercial, TelefoneComercial, DDDCelular, Celular, DataNascimento, Email, RecebeEmail, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, LoginOsesp, EstadoCliente, ClienteIndicacaoID, Obs, Senha, Ativo, StatusAtual, NomeEntrega, CPFEntrega, RGEntrega, CPFConsultado, NomeConsultado, StatusConsulta, CPFConsultadoEntrega, NomeConsultadoEntrega, StatusConsultaEntrega, Pais, CPFResponsavel, ContatoTipoID, NivelCliente, CNPJ, NomeFantasia, RazaoSocial, InscricaoEstadual, TipoCadastro, SituacaoProfissionalID, Profissao, DDDTelefoneComercial2, TelefoneComercial2 FROM tCliente WHERE ID = @I");
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
        /// Inserir novo(a) Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TCLIENTE");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCliente(ID, Nome, RG, CPF, CarteiraEstudante, Sexo, DDDTelefone, Telefone, DDDTelefoneComercial, TelefoneComercial, DDDCelular, Celular, DataNascimento, Email, RecebeEmail, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, LoginOsesp, EstadoCliente, ClienteIndicacaoID, Obs, Senha, Ativo, StatusAtual, NomeEntrega, CPFEntrega, RGEntrega, CPFConsultado, NomeConsultado, StatusConsulta, CPFConsultadoEntrega, NomeConsultadoEntrega, StatusConsultaEntrega, Pais, CPFResponsavel, ContatoTipoID, NivelCliente, CNPJ, NomeFantasia, RazaoSocial, InscricaoEstadual, TipoCadastro, SituacaoProfissionalID, Profissao, DDDTelefoneComercial2, TelefoneComercial2) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027','@028','@029',@030,'@031','@032','@033','@034','@035','@036','@037','@038','@039',@040,'@041','@042',@043,'@044','@045',@046,@047,'@048','@049','@050','@051','@052',@053,'@054','@055','@056')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.CarteiraEstudante.ValorBD);
                sql.Replace("@005", this.Sexo.ValorBD);
                sql.Replace("@006", this.DDDTelefone.ValorBD);
                sql.Replace("@007", this.Telefone.ValorBD);
                sql.Replace("@008", this.DDDTelefoneComercial.ValorBD);
                sql.Replace("@009", this.TelefoneComercial.ValorBD);
                sql.Replace("@010", this.DDDCelular.ValorBD);
                sql.Replace("@011", this.Celular.ValorBD);
                sql.Replace("@012", this.DataNascimento.ValorBD);
                sql.Replace("@013", this.Email.ValorBD);
                sql.Replace("@014", this.RecebeEmail.ValorBD);
                sql.Replace("@015", this.CEPEntrega.ValorBD);
                sql.Replace("@016", this.EnderecoEntrega.ValorBD);
                sql.Replace("@017", this.NumeroEntrega.ValorBD);
                sql.Replace("@018", this.ComplementoEntrega.ValorBD);
                sql.Replace("@019", this.BairroEntrega.ValorBD);
                sql.Replace("@020", this.CidadeEntrega.ValorBD);
                sql.Replace("@021", this.EstadoEntrega.ValorBD);
                sql.Replace("@022", this.CEPCliente.ValorBD);
                sql.Replace("@023", this.EnderecoCliente.ValorBD);
                sql.Replace("@024", this.NumeroCliente.ValorBD);
                sql.Replace("@025", this.ComplementoCliente.ValorBD);
                sql.Replace("@026", this.BairroCliente.ValorBD);
                sql.Replace("@027", this.CidadeCliente.ValorBD);
                sql.Replace("@028", this.LoginOsesp.ValorBD);
                sql.Replace("@029", this.EstadoCliente.ValorBD);
                sql.Replace("@030", this.ClienteIndicacaoID.ValorBD);
                sql.Replace("@031", this.Obs.ValorBD);
                sql.Replace("@032", this.Senha.ValorBD);
                sql.Replace("@033", this.Ativo.ValorBD);
                sql.Replace("@034", this.StatusAtual.ValorBD);
                sql.Replace("@035", this.NomeEntrega.ValorBD);
                sql.Replace("@036", this.CPFEntrega.ValorBD);
                sql.Replace("@037", this.RGEntrega.ValorBD);
                sql.Replace("@038", this.CPFConsultado.ValorBD);
                sql.Replace("@039", this.NomeConsultado.ValorBD);
                sql.Replace("@040", this.StatusConsulta.ValorBD);
                sql.Replace("@041", this.CPFConsultadoEntrega.ValorBD);
                sql.Replace("@042", this.NomeConsultadoEntrega.ValorBD);
                sql.Replace("@043", this.StatusConsultaEntrega.ValorBD);
                sql.Replace("@044", this.Pais.ValorBD);
                sql.Replace("@045", this.CPFResponsavel.ValorBD);
                sql.Replace("@046", this.ContatoTipoID.ValorBD);
                sql.Replace("@047", this.NivelCliente.ValorBD);
                sql.Replace("@048", this.CNPJ.ValorBD);
                sql.Replace("@049", this.NomeFantasia.ValorBD);
                sql.Replace("@050", this.RazaoSocial.ValorBD);
                sql.Replace("@051", this.InscricaoEstadual.ValorBD);
                sql.Replace("@052", this.TipoCadastro.ValorBD);
                sql.Replace("@053", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@054", this.Profissao.ValorBD);
                sql.Replace("@055", this.DDDTelefoneComercial2.ValorBD);
                sql.Replace("@056", this.TelefoneComercial2.ValorBD);

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
        /// Inserir novo(a) Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT NEXT VALUE FOR SEQ_TCLIENTE");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCliente(ID, Nome, RG, CPF, CarteiraEstudante, Sexo, DDDTelefone, Telefone, DDDTelefoneComercial, TelefoneComercial, DDDCelular, Celular, DataNascimento, Email, RecebeEmail, CEPEntrega, EnderecoEntrega, NumeroEntrega, ComplementoEntrega, BairroEntrega, CidadeEntrega, EstadoEntrega, CEPCliente, EnderecoCliente, NumeroCliente, ComplementoCliente, BairroCliente, CidadeCliente, LoginOsesp, EstadoCliente, ClienteIndicacaoID, Obs, Senha, Ativo, StatusAtual, NomeEntrega, CPFEntrega, RGEntrega, CPFConsultado, NomeConsultado, StatusConsulta, CPFConsultadoEntrega, NomeConsultadoEntrega, StatusConsultaEntrega, Pais, CPFResponsavel, ContatoTipoID, NivelCliente, CNPJ, NomeFantasia, RazaoSocial, InscricaoEstadual, TipoCadastro, SituacaoProfissionalID, Profissao, DDDTelefoneComercial2, TelefoneComercial2) ");
                sql.Append("VALUES (@ID,'@001','@002','@003','@004','@005','@006','@007','@008','@009','@010','@011','@012','@013','@014','@015','@016','@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027','@028','@029',@030,'@031','@032','@033','@034','@035','@036','@037','@038','@039',@040,'@041','@042',@043,'@044','@045',@046,@047,'@048','@049','@050','@051','@052',@053,'@054','@055','@056')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.CarteiraEstudante.ValorBD);
                sql.Replace("@005", this.Sexo.ValorBD);
                sql.Replace("@006", this.DDDTelefone.ValorBD);
                sql.Replace("@007", this.Telefone.ValorBD);
                sql.Replace("@008", this.DDDTelefoneComercial.ValorBD);
                sql.Replace("@009", this.TelefoneComercial.ValorBD);
                sql.Replace("@010", this.DDDCelular.ValorBD);
                sql.Replace("@011", this.Celular.ValorBD);
                sql.Replace("@012", this.DataNascimento.ValorBD);
                sql.Replace("@013", this.Email.ValorBD);
                sql.Replace("@014", this.RecebeEmail.ValorBD);
                sql.Replace("@015", this.CEPEntrega.ValorBD);
                sql.Replace("@016", this.EnderecoEntrega.ValorBD);
                sql.Replace("@017", this.NumeroEntrega.ValorBD);
                sql.Replace("@018", this.ComplementoEntrega.ValorBD);
                sql.Replace("@019", this.BairroEntrega.ValorBD);
                sql.Replace("@020", this.CidadeEntrega.ValorBD);
                sql.Replace("@021", this.EstadoEntrega.ValorBD);
                sql.Replace("@022", this.CEPCliente.ValorBD);
                sql.Replace("@023", this.EnderecoCliente.ValorBD);
                sql.Replace("@024", this.NumeroCliente.ValorBD);
                sql.Replace("@025", this.ComplementoCliente.ValorBD);
                sql.Replace("@026", this.BairroCliente.ValorBD);
                sql.Replace("@027", this.CidadeCliente.ValorBD);
                sql.Replace("@028", this.LoginOsesp.ValorBD);
                sql.Replace("@029", this.EstadoCliente.ValorBD);
                sql.Replace("@030", this.ClienteIndicacaoID.ValorBD);
                sql.Replace("@031", this.Obs.ValorBD);
                sql.Replace("@032", this.Senha.ValorBD);
                sql.Replace("@033", this.Ativo.ValorBD);
                sql.Replace("@034", this.StatusAtual.ValorBD);
                sql.Replace("@035", this.NomeEntrega.ValorBD);
                sql.Replace("@036", this.CPFEntrega.ValorBD);
                sql.Replace("@037", this.RGEntrega.ValorBD);
                sql.Replace("@038", this.CPFConsultado.ValorBD);
                sql.Replace("@039", this.NomeConsultado.ValorBD);
                sql.Replace("@040", this.StatusConsulta.ValorBD);
                sql.Replace("@041", this.CPFConsultadoEntrega.ValorBD);
                sql.Replace("@042", this.NomeConsultadoEntrega.ValorBD);
                sql.Replace("@043", this.StatusConsultaEntrega.ValorBD);
                sql.Replace("@044", this.Pais.ValorBD);
                sql.Replace("@045", this.CPFResponsavel.ValorBD);
                sql.Replace("@046", this.ContatoTipoID.ValorBD);
                sql.Replace("@047", this.NivelCliente.ValorBD);
                sql.Replace("@048", this.CNPJ.ValorBD);
                sql.Replace("@049", this.NomeFantasia.ValorBD);
                sql.Replace("@050", this.RazaoSocial.ValorBD);
                sql.Replace("@051", this.InscricaoEstadual.ValorBD);
                sql.Replace("@052", this.TipoCadastro.ValorBD);
                sql.Replace("@053", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@054", this.Profissao.ValorBD);
                sql.Replace("@055", this.DDDTelefoneComercial2.ValorBD);
                sql.Replace("@056", this.TelefoneComercial2.ValorBD);

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
        /// Atualiza Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cCliente WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCliente SET Nome = '@001', RG = '@002', CPF = '@003', CarteiraEstudante = '@004', Sexo = '@005', DDDTelefone = '@006', Telefone = '@007', DDDTelefoneComercial = '@008', TelefoneComercial = '@009', DDDCelular = '@010', Celular = '@011', DataNascimento = '@012', Email = '@013', RecebeEmail = '@014', CEPEntrega = '@015', EnderecoEntrega = '@016', NumeroEntrega = '@017', ComplementoEntrega = '@018', BairroEntrega = '@019', CidadeEntrega = '@020', EstadoEntrega = '@021', CEPCliente = '@022', EnderecoCliente = '@023', NumeroCliente = '@024', ComplementoCliente = '@025', BairroCliente = '@026', CidadeCliente = '@027', LoginOsesp = '@028', EstadoCliente = '@029', ClienteIndicacaoID = @030, Obs = '@031', Senha = '@032', Ativo = '@033', StatusAtual = '@034', NomeEntrega = '@035', CPFEntrega = '@036', RGEntrega = '@037', CPFConsultado = '@038', NomeConsultado = '@039', StatusConsulta = @040, CPFConsultadoEntrega = '@041', NomeConsultadoEntrega = '@042', StatusConsultaEntrega = @043, Pais = '@044', CPFResponsavel = '@045', ContatoTipoID = @046, NivelCliente = @047, CNPJ = '@048', NomeFantasia = '@049', RazaoSocial = '@050', InscricaoEstadual = '@051', TipoCadastro = '@052', SituacaoProfissionalID = @053, Profissao = '@054', DDDTelefoneComercial2 = '@055', TelefoneComercial2 = '@056' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.CarteiraEstudante.ValorBD);
                sql.Replace("@005", this.Sexo.ValorBD);
                sql.Replace("@006", this.DDDTelefone.ValorBD);
                sql.Replace("@007", this.Telefone.ValorBD);
                sql.Replace("@008", this.DDDTelefoneComercial.ValorBD);
                sql.Replace("@009", this.TelefoneComercial.ValorBD);
                sql.Replace("@010", this.DDDCelular.ValorBD);
                sql.Replace("@011", this.Celular.ValorBD);
                sql.Replace("@012", this.DataNascimento.ValorBD);
                sql.Replace("@013", this.Email.ValorBD);
                sql.Replace("@014", this.RecebeEmail.ValorBD);
                sql.Replace("@015", this.CEPEntrega.ValorBD);
                sql.Replace("@016", this.EnderecoEntrega.ValorBD);
                sql.Replace("@017", this.NumeroEntrega.ValorBD);
                sql.Replace("@018", this.ComplementoEntrega.ValorBD);
                sql.Replace("@019", this.BairroEntrega.ValorBD);
                sql.Replace("@020", this.CidadeEntrega.ValorBD);
                sql.Replace("@021", this.EstadoEntrega.ValorBD);
                sql.Replace("@022", this.CEPCliente.ValorBD);
                sql.Replace("@023", this.EnderecoCliente.ValorBD);
                sql.Replace("@024", this.NumeroCliente.ValorBD);
                sql.Replace("@025", this.ComplementoCliente.ValorBD);
                sql.Replace("@026", this.BairroCliente.ValorBD);
                sql.Replace("@027", this.CidadeCliente.ValorBD);
                sql.Replace("@028", this.LoginOsesp.ValorBD);
                sql.Replace("@029", this.EstadoCliente.ValorBD);
                sql.Replace("@030", this.ClienteIndicacaoID.ValorBD);
                sql.Replace("@031", this.Obs.ValorBD);
                sql.Replace("@032", this.Senha.ValorBD);
                sql.Replace("@033", this.Ativo.ValorBD);
                sql.Replace("@034", this.StatusAtual.ValorBD);
                sql.Replace("@035", this.NomeEntrega.ValorBD);
                sql.Replace("@036", this.CPFEntrega.ValorBD);
                sql.Replace("@037", this.RGEntrega.ValorBD);
                sql.Replace("@038", this.CPFConsultado.ValorBD);
                sql.Replace("@039", this.NomeConsultado.ValorBD);
                sql.Replace("@040", this.StatusConsulta.ValorBD);
                sql.Replace("@041", this.CPFConsultadoEntrega.ValorBD);
                sql.Replace("@042", this.NomeConsultadoEntrega.ValorBD);
                sql.Replace("@043", this.StatusConsultaEntrega.ValorBD);
                sql.Replace("@044", this.Pais.ValorBD);
                sql.Replace("@045", this.CPFResponsavel.ValorBD);
                sql.Replace("@046", this.ContatoTipoID.ValorBD);
                sql.Replace("@047", this.NivelCliente.ValorBD);
                sql.Replace("@048", this.CNPJ.ValorBD);
                sql.Replace("@049", this.NomeFantasia.ValorBD);
                sql.Replace("@050", this.RazaoSocial.ValorBD);
                sql.Replace("@051", this.InscricaoEstadual.ValorBD);
                sql.Replace("@052", this.TipoCadastro.ValorBD);
                sql.Replace("@053", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@054", this.Profissao.ValorBD);
                sql.Replace("@055", this.DDDTelefoneComercial2.ValorBD);
                sql.Replace("@056", this.TelefoneComercial2.ValorBD);

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
        /// Atualiza Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cCliente WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCliente SET Nome = '@001', RG = '@002', CPF = '@003', CarteiraEstudante = '@004', Sexo = '@005', DDDTelefone = '@006', Telefone = '@007', DDDTelefoneComercial = '@008', TelefoneComercial = '@009', DDDCelular = '@010', Celular = '@011', DataNascimento = '@012', Email = '@013', RecebeEmail = '@014', CEPEntrega = '@015', EnderecoEntrega = '@016', NumeroEntrega = '@017', ComplementoEntrega = '@018', BairroEntrega = '@019', CidadeEntrega = '@020', EstadoEntrega = '@021', CEPCliente = '@022', EnderecoCliente = '@023', NumeroCliente = '@024', ComplementoCliente = '@025', BairroCliente = '@026', CidadeCliente = '@027', LoginOsesp = '@028', EstadoCliente = '@029', ClienteIndicacaoID = @030, Obs = '@031', Senha = '@032', Ativo = '@033', StatusAtual = '@034', NomeEntrega = '@035', CPFEntrega = '@036', RGEntrega = '@037', CPFConsultado = '@038', NomeConsultado = '@039', StatusConsulta = @040, CPFConsultadoEntrega = '@041', NomeConsultadoEntrega = '@042', StatusConsultaEntrega = @043, Pais = '@044', CPFResponsavel = '@045', ContatoTipoID = @046, NivelCliente = @047, CNPJ = '@048', NomeFantasia = '@049', RazaoSocial = '@050', InscricaoEstadual = '@051', TipoCadastro = '@052', SituacaoProfissionalID = @053, Profissao = '@054', DDDTelefoneComercial2 = '@055', TelefoneComercial2 = '@056' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.RG.ValorBD);
                sql.Replace("@003", this.CPF.ValorBD);
                sql.Replace("@004", this.CarteiraEstudante.ValorBD);
                sql.Replace("@005", this.Sexo.ValorBD);
                sql.Replace("@006", this.DDDTelefone.ValorBD);
                sql.Replace("@007", this.Telefone.ValorBD);
                sql.Replace("@008", this.DDDTelefoneComercial.ValorBD);
                sql.Replace("@009", this.TelefoneComercial.ValorBD);
                sql.Replace("@010", this.DDDCelular.ValorBD);
                sql.Replace("@011", this.Celular.ValorBD);
                sql.Replace("@012", this.DataNascimento.ValorBD);
                sql.Replace("@013", this.Email.ValorBD);
                sql.Replace("@014", this.RecebeEmail.ValorBD);
                sql.Replace("@015", this.CEPEntrega.ValorBD);
                sql.Replace("@016", this.EnderecoEntrega.ValorBD);
                sql.Replace("@017", this.NumeroEntrega.ValorBD);
                sql.Replace("@018", this.ComplementoEntrega.ValorBD);
                sql.Replace("@019", this.BairroEntrega.ValorBD);
                sql.Replace("@020", this.CidadeEntrega.ValorBD);
                sql.Replace("@021", this.EstadoEntrega.ValorBD);
                sql.Replace("@022", this.CEPCliente.ValorBD);
                sql.Replace("@023", this.EnderecoCliente.ValorBD);
                sql.Replace("@024", this.NumeroCliente.ValorBD);
                sql.Replace("@025", this.ComplementoCliente.ValorBD);
                sql.Replace("@026", this.BairroCliente.ValorBD);
                sql.Replace("@027", this.CidadeCliente.ValorBD);
                sql.Replace("@028", this.LoginOsesp.ValorBD);
                sql.Replace("@029", this.EstadoCliente.ValorBD);
                sql.Replace("@030", this.ClienteIndicacaoID.ValorBD);
                sql.Replace("@031", this.Obs.ValorBD);
                sql.Replace("@032", this.Senha.ValorBD);
                sql.Replace("@033", this.Ativo.ValorBD);
                sql.Replace("@034", this.StatusAtual.ValorBD);
                sql.Replace("@035", this.NomeEntrega.ValorBD);
                sql.Replace("@036", this.CPFEntrega.ValorBD);
                sql.Replace("@037", this.RGEntrega.ValorBD);
                sql.Replace("@038", this.CPFConsultado.ValorBD);
                sql.Replace("@039", this.NomeConsultado.ValorBD);
                sql.Replace("@040", this.StatusConsulta.ValorBD);
                sql.Replace("@041", this.CPFConsultadoEntrega.ValorBD);
                sql.Replace("@042", this.NomeConsultadoEntrega.ValorBD);
                sql.Replace("@043", this.StatusConsultaEntrega.ValorBD);
                sql.Replace("@044", this.Pais.ValorBD);
                sql.Replace("@045", this.CPFResponsavel.ValorBD);
                sql.Replace("@046", this.ContatoTipoID.ValorBD);
                sql.Replace("@047", this.NivelCliente.ValorBD);
                sql.Replace("@048", this.CNPJ.ValorBD);
                sql.Replace("@049", this.NomeFantasia.ValorBD);
                sql.Replace("@050", this.RazaoSocial.ValorBD);
                sql.Replace("@051", this.InscricaoEstadual.ValorBD);
                sql.Replace("@052", this.TipoCadastro.ValorBD);
                sql.Replace("@053", this.SituacaoProfissionalID.ValorBD);
                sql.Replace("@054", this.Profissao.ValorBD);
                sql.Replace("@055", this.DDDTelefoneComercial2.ValorBD);
                sql.Replace("@056", this.TelefoneComercial2.ValorBD);

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
        /// Exclui Cliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCliente WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCliente WHERE ID=" + id;

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
        /// Exclui Cliente com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cCliente WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tCliente WHERE ID=" + id;

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
        /// Exclui Cliente
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
            this.CarteiraEstudante.Limpar();
            this.Sexo.Limpar();
            this.DDDTelefone.Limpar();
            this.Telefone.Limpar();
            this.DDDTelefoneComercial.Limpar();
            this.TelefoneComercial.Limpar();
            this.DDDCelular.Limpar();
            this.Celular.Limpar();
            this.DataNascimento.Limpar();
            this.Email.Limpar();
            this.RecebeEmail.Limpar();
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
            this.LoginOsesp.Limpar();
            this.EstadoCliente.Limpar();
            this.ClienteIndicacaoID.Limpar();
            this.Obs.Limpar();
            this.Senha.Limpar();
            this.Ativo.Limpar();
            this.StatusAtual.Limpar();
            this.NomeEntrega.Limpar();
            this.CPFEntrega.Limpar();
            this.RGEntrega.Limpar();
            this.CPFConsultado.Limpar();
            this.NomeConsultado.Limpar();
            this.StatusConsulta.Limpar();
            this.CPFConsultadoEntrega.Limpar();
            this.NomeConsultadoEntrega.Limpar();
            this.StatusConsultaEntrega.Limpar();
            this.Pais.Limpar();
            this.CPFResponsavel.Limpar();
            this.ContatoTipoID.Limpar();
            this.NivelCliente.Limpar();
            this.CNPJ.Limpar();
            this.NomeFantasia.Limpar();
            this.RazaoSocial.Limpar();
            this.InscricaoEstadual.Limpar();
            this.TipoCadastro.Limpar();
            this.SituacaoProfissionalID.Limpar();
            this.Profissao.Limpar();
            this.DDDTelefoneComercial2.Limpar();
            this.TelefoneComercial2.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.Nome.Desfazer();
            this.RG.Desfazer();
            this.CPF.Desfazer();
            this.CarteiraEstudante.Desfazer();
            this.Sexo.Desfazer();
            this.DDDTelefone.Desfazer();
            this.Telefone.Desfazer();
            this.DDDTelefoneComercial.Desfazer();
            this.TelefoneComercial.Desfazer();
            this.DDDCelular.Desfazer();
            this.Celular.Desfazer();
            this.DataNascimento.Desfazer();
            this.Email.Desfazer();
            this.RecebeEmail.Desfazer();
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
            this.LoginOsesp.Desfazer();
            this.EstadoCliente.Desfazer();
            this.ClienteIndicacaoID.Desfazer();
            this.Obs.Desfazer();
            this.Senha.Desfazer();
            this.Ativo.Desfazer();
            this.StatusAtual.Desfazer();
            this.NomeEntrega.Desfazer();
            this.CPFEntrega.Desfazer();
            this.RGEntrega.Desfazer();
            this.CPFConsultado.Desfazer();
            this.NomeConsultado.Desfazer();
            this.StatusConsulta.Desfazer();
            this.CPFConsultadoEntrega.Desfazer();
            this.NomeConsultadoEntrega.Desfazer();
            this.StatusConsultaEntrega.Desfazer();
            this.Pais.Desfazer();
            this.CPFResponsavel.Desfazer();
            this.ContatoTipoID.Desfazer();
            this.NivelCliente.Desfazer();
            this.CNPJ.Desfazer();
            this.NomeFantasia.Desfazer();
            this.RazaoSocial.Desfazer();
            this.InscricaoEstadual.Desfazer();
            this.TipoCadastro.Desfazer();
            this.SituacaoProfissionalID.Desfazer();
            this.Profissao.Desfazer();
            this.DDDTelefoneComercial2.Desfazer();
            this.TelefoneComercial2.Desfazer();
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

        public class carteiraestudante : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CarteiraEstudante";
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

        public class sexo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Sexo";
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

        public class dddtelefonecomercial : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDTelefoneComercial";
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

        public class telefonecomercial : TextProperty
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

        public class dddcelular : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDCelular";
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

        public class celular : TextProperty
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
                    return 9;
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

        public class datanascimento : DateProperty
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
                return base.Valor.ToString("dd/MM/yyyy");
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

        public class recebeemail : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RecebeEmail";
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

        public class cepentrega : TextProperty
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

        public class enderecoentrega : TextProperty
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

        public class numeroentrega : TextProperty
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

        public class complementoentrega : TextProperty
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

        public class bairroentrega : TextProperty
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
                    return 40;
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

        public class cidadeentrega : TextProperty
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

        public class estadoentrega : TextProperty
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

        public class cepcliente : TextProperty
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

        public class enderecocliente : TextProperty
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

        public class numerocliente : TextProperty
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

        public class complementocliente : TextProperty
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

        public class bairrocliente : TextProperty
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
                    return 40;
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

        public class cidadecliente : TextProperty
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

        public class loginosesp : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "LoginOsesp";
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

        public class estadocliente : TextProperty
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

        public class clienteindicacaoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ClienteIndicacaoID";
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

        public class senha : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Senha";
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

        public class statusatual : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "StatusAtual";
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

        public class nomeentrega : TextProperty
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

        public class cpfentrega : TextProperty
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

        public class rgentrega : TextProperty
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

        public class cpfconsultado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFConsultado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 11;
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

        public class nomeconsultado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeConsultado";
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

        public class cpfconsultadoentrega : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CPFConsultadoEntrega";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 11;
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

        public class nomeconsultadoentrega : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeConsultadoEntrega";
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

        public class statusconsultaentrega : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "StatusConsultaEntrega";
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

        public class pais : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Pais";
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

        public class cpfresponsavel : TextProperty
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

        public class contatotipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContatoTipoID";
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

        public class nivelcliente : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NivelCliente";
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

        public class cnpj : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CNPJ";
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

        public class nomefantasia : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NomeFantasia";
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

        public class razaosocial : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "RazaoSocial";
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

        public class inscricaoestadual : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "InscricaoEstadual";
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

        public class tipocadastro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCadastro";
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

        public class situacaoprofissionalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SituacaoProfissionalID";
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

        public class profissao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Profissao";
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

        public class dddtelefonecomercial2 : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDTelefoneComercial2";
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

        public class telefonecomercial2 : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TelefoneComercial2";
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

                DataTable tabela = new DataTable("Cliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("CarteiraEstudante", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDTelefoneComercial", typeof(string));
                tabela.Columns.Add("TelefoneComercial", typeof(string));
                tabela.Columns.Add("DDDCelular", typeof(string));
                tabela.Columns.Add("Celular", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(DateTime));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("RecebeEmail", typeof(bool));
                tabela.Columns.Add("CEPEntrega", typeof(string));
                tabela.Columns.Add("EnderecoEntrega", typeof(string));
                tabela.Columns.Add("NumeroEntrega", typeof(string));
                tabela.Columns.Add("ComplementoEntrega", typeof(string));
                tabela.Columns.Add("BairroEntrega", typeof(string));
                tabela.Columns.Add("CidadeEntrega", typeof(string));
                tabela.Columns.Add("EstadoEntrega", typeof(string));
                tabela.Columns.Add("CEPCliente", typeof(string));
                tabela.Columns.Add("EnderecoCliente", typeof(string));
                tabela.Columns.Add("NumeroCliente", typeof(string));
                tabela.Columns.Add("ComplementoCliente", typeof(string));
                tabela.Columns.Add("BairroCliente", typeof(string));
                tabela.Columns.Add("CidadeCliente", typeof(string));
                tabela.Columns.Add("LoginOsesp", typeof(string));
                tabela.Columns.Add("EstadoCliente", typeof(string));
                tabela.Columns.Add("ClienteIndicacaoID", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("StatusAtual", typeof(string));
                tabela.Columns.Add("NomeEntrega", typeof(string));
                tabela.Columns.Add("CPFEntrega", typeof(string));
                tabela.Columns.Add("RGEntrega", typeof(string));
                tabela.Columns.Add("CPFConsultado", typeof(string));
                tabela.Columns.Add("NomeConsultado", typeof(string));
                tabela.Columns.Add("StatusConsulta", typeof(int));
                tabela.Columns.Add("CPFConsultadoEntrega", typeof(string));
                tabela.Columns.Add("NomeConsultadoEntrega", typeof(string));
                tabela.Columns.Add("StatusConsultaEntrega", typeof(int));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("CPFResponsavel", typeof(string));
                tabela.Columns.Add("ContatoTipoID", typeof(int));
                tabela.Columns.Add("NivelCliente", typeof(int));
                tabela.Columns.Add("CNPJ", typeof(string));
                tabela.Columns.Add("NomeFantasia", typeof(string));
                tabela.Columns.Add("RazaoSocial", typeof(string));
                tabela.Columns.Add("InscricaoEstadual", typeof(string));
                tabela.Columns.Add("TipoCadastro", typeof(string));
                tabela.Columns.Add("SituacaoProfissionalID", typeof(int));
                tabela.Columns.Add("Profissao", typeof(string));
                tabela.Columns.Add("DDDTelefoneComercial2", typeof(string));
                tabela.Columns.Add("TelefoneComercial2", typeof(string));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract int QtdeIngressoCompradoPorApresentacaoSetor(int apresentacaosetorid);

        public abstract int QtdeIngressoCompradoPorApresentacao(int apresentacaoid);

        public abstract int QtdeIngressoCompradoPorPreco(int precoid);

        public abstract int QtdeIngressoCompradoPorPacote(int pacoteid);

        public abstract DataTable Homonimos(string registrozero);

    }
    #endregion

    #region "ClienteLista_B"

    public abstract class ClienteLista_B : BaseLista
    {

        private bool backup = false;
        protected Cliente cliente;

        // passar o Usuario logado no sistema
        public ClienteLista_B()
        {
            cliente = new Cliente();
        }

        // passar o Usuario logado no sistema
        public ClienteLista_B(int usuarioIDLogado)
        {
            cliente = new Cliente(usuarioIDLogado);
        }

        public Cliente Cliente
        {
            get { return cliente; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Cliente especifico
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
                    cliente.Ler(id);
                    return cliente;
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
                    sql = "SELECT ID FROM tCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCliente";

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
                    sql = "SELECT ID FROM tCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCliente";

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
                    sql = "SELECT ID FROM xCliente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xCliente";

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
        /// Preenche Cliente corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    cliente.Ler(id);
                else
                    cliente.LerBackup(id);

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

                bool ok = cliente.Excluir();
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
        /// Inseri novo(a) Cliente na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = cliente.Inserir();
                if (ok)
                {
                    lista.Add(cliente.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Cliente carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Cliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("CarteiraEstudante", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDTelefoneComercial", typeof(string));
                tabela.Columns.Add("TelefoneComercial", typeof(string));
                tabela.Columns.Add("DDDCelular", typeof(string));
                tabela.Columns.Add("Celular", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(DateTime));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("RecebeEmail", typeof(bool));
                tabela.Columns.Add("CEPEntrega", typeof(string));
                tabela.Columns.Add("EnderecoEntrega", typeof(string));
                tabela.Columns.Add("NumeroEntrega", typeof(string));
                tabela.Columns.Add("ComplementoEntrega", typeof(string));
                tabela.Columns.Add("BairroEntrega", typeof(string));
                tabela.Columns.Add("CidadeEntrega", typeof(string));
                tabela.Columns.Add("EstadoEntrega", typeof(string));
                tabela.Columns.Add("CEPCliente", typeof(string));
                tabela.Columns.Add("EnderecoCliente", typeof(string));
                tabela.Columns.Add("NumeroCliente", typeof(string));
                tabela.Columns.Add("ComplementoCliente", typeof(string));
                tabela.Columns.Add("BairroCliente", typeof(string));
                tabela.Columns.Add("CidadeCliente", typeof(string));
                tabela.Columns.Add("LoginOsesp", typeof(string));
                tabela.Columns.Add("EstadoCliente", typeof(string));
                tabela.Columns.Add("ClienteIndicacaoID", typeof(int));
                tabela.Columns.Add("Obs", typeof(string));
                tabela.Columns.Add("Senha", typeof(string));
                tabela.Columns.Add("Ativo", typeof(bool));
                tabela.Columns.Add("StatusAtual", typeof(string));
                tabela.Columns.Add("NomeEntrega", typeof(string));
                tabela.Columns.Add("CPFEntrega", typeof(string));
                tabela.Columns.Add("RGEntrega", typeof(string));
                tabela.Columns.Add("CPFConsultado", typeof(string));
                tabela.Columns.Add("NomeConsultado", typeof(string));
                tabela.Columns.Add("StatusConsulta", typeof(int));
                tabela.Columns.Add("CPFConsultadoEntrega", typeof(string));
                tabela.Columns.Add("NomeConsultadoEntrega", typeof(string));
                tabela.Columns.Add("StatusConsultaEntrega", typeof(int));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("CPFResponsavel", typeof(string));
                tabela.Columns.Add("ContatoTipoID", typeof(int));
                tabela.Columns.Add("NivelCliente", typeof(int));
                tabela.Columns.Add("CNPJ", typeof(string));
                tabela.Columns.Add("NomeFantasia", typeof(string));
                tabela.Columns.Add("RazaoSocial", typeof(string));
                tabela.Columns.Add("InscricaoEstadual", typeof(string));
                tabela.Columns.Add("TipoCadastro", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cliente.Control.ID;
                        linha["Nome"] = cliente.Nome.Valor;
                        linha["RG"] = cliente.RG.Valor;
                        linha["CPF"] = cliente.CPF.Valor;
                        linha["CarteiraEstudante"] = cliente.CarteiraEstudante.Valor;
                        linha["Sexo"] = cliente.Sexo.Valor;
                        linha["DDDTelefone"] = cliente.DDDTelefone.Valor;
                        linha["Telefone"] = cliente.Telefone.Valor;
                        linha["DDDTelefoneComercial"] = cliente.DDDTelefoneComercial.Valor;
                        linha["TelefoneComercial"] = cliente.TelefoneComercial.Valor;
                        linha["DDDCelular"] = cliente.DDDCelular.Valor;
                        linha["Celular"] = cliente.Celular.Valor;
                        linha["DataNascimento"] = cliente.DataNascimento.Valor;
                        linha["Email"] = cliente.Email.Valor;
                        linha["RecebeEmail"] = cliente.RecebeEmail.Valor;
                        linha["CEPEntrega"] = cliente.CEPEntrega.Valor;
                        linha["EnderecoEntrega"] = cliente.EnderecoEntrega.Valor;
                        linha["NumeroEntrega"] = cliente.NumeroEntrega.Valor;
                        linha["ComplementoEntrega"] = cliente.ComplementoEntrega.Valor;
                        linha["BairroEntrega"] = cliente.BairroEntrega.Valor;
                        linha["CidadeEntrega"] = cliente.CidadeEntrega.Valor;
                        linha["EstadoEntrega"] = cliente.EstadoEntrega.Valor;
                        linha["CEPCliente"] = cliente.CEPCliente.Valor;
                        linha["EnderecoCliente"] = cliente.EnderecoCliente.Valor;
                        linha["NumeroCliente"] = cliente.NumeroCliente.Valor;
                        linha["ComplementoCliente"] = cliente.ComplementoCliente.Valor;
                        linha["BairroCliente"] = cliente.BairroCliente.Valor;
                        linha["CidadeCliente"] = cliente.CidadeCliente.Valor;
                        linha["LoginOsesp"] = cliente.LoginOsesp.Valor;
                        linha["EstadoCliente"] = cliente.EstadoCliente.Valor;
                        linha["ClienteIndicacaoID"] = cliente.ClienteIndicacaoID.Valor;
                        linha["Obs"] = cliente.Obs.Valor;
                        linha["Senha"] = cliente.Senha.Valor;
                        linha["Ativo"] = cliente.Ativo.Valor;
                        linha["StatusAtual"] = cliente.StatusAtual.Valor;
                        linha["NomeEntrega"] = cliente.NomeEntrega.Valor;
                        linha["CPFEntrega"] = cliente.CPFEntrega.Valor;
                        linha["RGEntrega"] = cliente.RGEntrega.Valor;
                        linha["CPFConsultado"] = cliente.CPFConsultado.Valor;
                        linha["NomeConsultado"] = cliente.NomeConsultado.Valor;
                        linha["StatusConsulta"] = cliente.StatusConsulta;
                        linha["CPFConsultadoEntrega"] = cliente.CPFConsultadoEntrega.Valor;
                        linha["NomeConsultadoEntrega"] = cliente.NomeConsultadoEntrega.Valor;
                        linha["StatusConsultaEntrega"] = cliente.StatusConsultaEntrega;
                        linha["Pais"] = cliente.Pais.Valor;
                        linha["CPFResponsavel"] = cliente.CPFResponsavel.Valor;
                        linha["ContatoTipoID"] = cliente.ContatoTipoID.Valor;
                        linha["NivelCliente"] = cliente.NivelCliente.Valor;
                        linha["CNPJ"] = cliente.CNPJ.Valor;
                        linha["NomeFantasia"] = cliente.NomeFantasia.Valor;
                        linha["RazaoSocial"] = cliente.RazaoSocial.Valor;
                        linha["InscricaoEstadual"] = cliente.InscricaoEstadual.Valor;
                        linha["TipoCadastro"] = cliente.TipoCadastro.Valor;
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

                DataTable tabela = new DataTable("RelatorioCliente");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("RG", typeof(string));
                    tabela.Columns.Add("CPF", typeof(string));
                    tabela.Columns.Add("CarteiraEstudante", typeof(string));
                    tabela.Columns.Add("Sexo", typeof(string));
                    tabela.Columns.Add("DDDTelefone", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("DDDTelefoneComercial", typeof(string));
                    tabela.Columns.Add("TelefoneComercial", typeof(string));
                    tabela.Columns.Add("DDDCelular", typeof(string));
                    tabela.Columns.Add("Celular", typeof(string));
                    tabela.Columns.Add("DataNascimento", typeof(DateTime));
                    tabela.Columns.Add("Email", typeof(string));
                    tabela.Columns.Add("RecebeEmail", typeof(bool));
                    tabela.Columns.Add("CEPEntrega", typeof(string));
                    tabela.Columns.Add("EnderecoEntrega", typeof(string));
                    tabela.Columns.Add("NumeroEntrega", typeof(string));
                    tabela.Columns.Add("ComplementoEntrega", typeof(string));
                    tabela.Columns.Add("BairroEntrega", typeof(string));
                    tabela.Columns.Add("CidadeEntrega", typeof(string));
                    tabela.Columns.Add("EstadoEntrega", typeof(string));
                    tabela.Columns.Add("CEPCliente", typeof(string));
                    tabela.Columns.Add("EnderecoCliente", typeof(string));
                    tabela.Columns.Add("NumeroCliente", typeof(string));
                    tabela.Columns.Add("ComplementoCliente", typeof(string));
                    tabela.Columns.Add("BairroCliente", typeof(string));
                    tabela.Columns.Add("CidadeCliente", typeof(string));
                    tabela.Columns.Add("LoginOsesp", typeof(string));
                    tabela.Columns.Add("EstadoCliente", typeof(string));
                    tabela.Columns.Add("ClienteIndicacaoID", typeof(int));
                    tabela.Columns.Add("Obs", typeof(string));
                    tabela.Columns.Add("Senha", typeof(string));
                    tabela.Columns.Add("Ativo", typeof(bool));
                    tabela.Columns.Add("StatusAtual", typeof(string));
                    tabela.Columns.Add("NomeEntrega", typeof(string));
                    tabela.Columns.Add("CPFEntrega", typeof(string));
                    tabela.Columns.Add("RGEntrega", typeof(string));
                    tabela.Columns.Add("CPFConsultado", typeof(string));
                    tabela.Columns.Add("NomeConsultado", typeof(string));
                    tabela.Columns.Add("StatusConsulta", typeof(int));
                    tabela.Columns.Add("CPFConsultadoEntrega", typeof(string));
                    tabela.Columns.Add("NomeConsultadoEntrega", typeof(string));
                    tabela.Columns.Add("StatusConsultaEntrega", typeof(int));
                    tabela.Columns.Add("Pais", typeof(string));
                    tabela.Columns.Add("CPFResponsavel", typeof(string));
                    tabela.Columns.Add("ContatoTipoID", typeof(int));
                    tabela.Columns.Add("NivelCliente", typeof(int));
                    tabela.Columns.Add("CNPJ", typeof(string));
                    tabela.Columns.Add("NomeFantasia", typeof(string));
                    tabela.Columns.Add("RazaoSocial", typeof(string));
                    tabela.Columns.Add("InscricaoEstadual", typeof(string));
                    tabela.Columns.Add("TipoCadastro", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = cliente.Nome.Valor;
                        linha["RG"] = cliente.RG.Valor;
                        linha["CPF"] = cliente.CPF.Valor;
                        linha["CarteiraEstudante"] = cliente.CarteiraEstudante.Valor;
                        linha["Sexo"] = cliente.Sexo.Valor;
                        linha["DDDTelefone"] = cliente.DDDTelefone.Valor;
                        linha["Telefone"] = cliente.Telefone.Valor;
                        linha["DDDTelefoneComercial"] = cliente.DDDTelefoneComercial.Valor;
                        linha["TelefoneComercial"] = cliente.TelefoneComercial.Valor;
                        linha["DDDCelular"] = cliente.DDDCelular.Valor;
                        linha["Celular"] = cliente.Celular.Valor;
                        linha["DataNascimento"] = cliente.DataNascimento.Valor;
                        linha["Email"] = cliente.Email.Valor;
                        linha["RecebeEmail"] = cliente.RecebeEmail.Valor;
                        linha["CEPEntrega"] = cliente.CEPEntrega.Valor;
                        linha["EnderecoEntrega"] = cliente.EnderecoEntrega.Valor;
                        linha["NumeroEntrega"] = cliente.NumeroEntrega.Valor;
                        linha["ComplementoEntrega"] = cliente.ComplementoEntrega.Valor;
                        linha["BairroEntrega"] = cliente.BairroEntrega.Valor;
                        linha["CidadeEntrega"] = cliente.CidadeEntrega.Valor;
                        linha["EstadoEntrega"] = cliente.EstadoEntrega.Valor;
                        linha["CEPCliente"] = cliente.CEPCliente.Valor;
                        linha["EnderecoCliente"] = cliente.EnderecoCliente.Valor;
                        linha["NumeroCliente"] = cliente.NumeroCliente.Valor;
                        linha["ComplementoCliente"] = cliente.ComplementoCliente.Valor;
                        linha["BairroCliente"] = cliente.BairroCliente.Valor;
                        linha["CidadeCliente"] = cliente.CidadeCliente.Valor;
                        linha["LoginOsesp"] = cliente.LoginOsesp.Valor;
                        linha["EstadoCliente"] = cliente.EstadoCliente.Valor;
                        linha["ClienteIndicacaoID"] = cliente.ClienteIndicacaoID.Valor;
                        linha["Obs"] = cliente.Obs.Valor;
                        linha["Senha"] = cliente.Senha.Valor;
                        linha["Ativo"] = cliente.Ativo.Valor;
                        linha["StatusAtual"] = cliente.StatusAtual.Valor;
                        linha["NomeEntrega"] = cliente.NomeEntrega.Valor;
                        linha["CPFEntrega"] = cliente.CPFEntrega.Valor;
                        linha["RGEntrega"] = cliente.RGEntrega.Valor;
                        linha["CPFConsultado"] = cliente.CPFConsultado.Valor;
                        linha["NomeConsultado"] = cliente.NomeConsultado.Valor;
                        linha["StatusConsulta"] = cliente.StatusConsulta;
                        linha["CPFConsultadoEntrega"] = cliente.CPFConsultadoEntrega.Valor;
                        linha["NomeConsultadoEntrega"] = cliente.NomeConsultadoEntrega.Valor;
                        linha["StatusConsultaEntrega"] = cliente.StatusConsultaEntrega;
                        linha["Pais"] = cliente.Pais.Valor;
                        linha["CPFResponsavel"] = cliente.CPFResponsavel.Valor;
                        linha["ContatoTipoID"] = cliente.ContatoTipoID.Valor;
                        linha["NivelCliente"] = cliente.NivelCliente.Valor;
                        linha["CNPJ"] = cliente.CNPJ.Valor;
                        linha["NomeFantasia"] = cliente.NomeFantasia.Valor;
                        linha["RazaoSocial"] = cliente.RazaoSocial.Valor;
                        linha["InscricaoEstadual"] = cliente.InscricaoEstadual.Valor;
                        linha["TipoCadastro"] = cliente.TipoCadastro.Valor;
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
                        sql = "SELECT ID, Nome FROM tCliente WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "RG":
                        sql = "SELECT ID, RG FROM tCliente WHERE " + FiltroSQL + " ORDER BY RG";
                        break;
                    case "CPF":
                        sql = "SELECT ID, CPF FROM tCliente WHERE " + FiltroSQL + " ORDER BY CPF";
                        break;
                    case "CarteiraEstudante":
                        sql = "SELECT ID, CarteiraEstudante FROM tCliente WHERE " + FiltroSQL + " ORDER BY CarteiraEstudante";
                        break;
                    case "Sexo":
                        sql = "SELECT ID, Sexo FROM tCliente WHERE " + FiltroSQL + " ORDER BY Sexo";
                        break;
                    case "DDDTelefone":
                        sql = "SELECT ID, DDDTelefone FROM tCliente WHERE " + FiltroSQL + " ORDER BY DDDTelefone";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tCliente WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "DDDTelefoneComercial":
                        sql = "SELECT ID, DDDTelefoneComercial FROM tCliente WHERE " + FiltroSQL + " ORDER BY DDDTelefoneComercial";
                        break;
                    case "TelefoneComercial":
                        sql = "SELECT ID, TelefoneComercial FROM tCliente WHERE " + FiltroSQL + " ORDER BY TelefoneComercial";
                        break;
                    case "DDDCelular":
                        sql = "SELECT ID, DDDCelular FROM tCliente WHERE " + FiltroSQL + " ORDER BY DDDCelular";
                        break;
                    case "Celular":
                        sql = "SELECT ID, Celular FROM tCliente WHERE " + FiltroSQL + " ORDER BY Celular";
                        break;
                    case "DataNascimento":
                        sql = "SELECT ID, DataNascimento FROM tCliente WHERE " + FiltroSQL + " ORDER BY DataNascimento";
                        break;
                    case "Email":
                        sql = "SELECT ID, Email FROM tCliente WHERE " + FiltroSQL + " ORDER BY Email";
                        break;
                    case "RecebeEmail":
                        sql = "SELECT ID, RecebeEmail FROM tCliente WHERE " + FiltroSQL + " ORDER BY RecebeEmail";
                        break;
                    case "CEPEntrega":
                        sql = "SELECT ID, CEPEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY CEPEntrega";
                        break;
                    case "EnderecoEntrega":
                        sql = "SELECT ID, EnderecoEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY EnderecoEntrega";
                        break;
                    case "NumeroEntrega":
                        sql = "SELECT ID, NumeroEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY NumeroEntrega";
                        break;
                    case "ComplementoEntrega":
                        sql = "SELECT ID, ComplementoEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY ComplementoEntrega";
                        break;
                    case "BairroEntrega":
                        sql = "SELECT ID, BairroEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY BairroEntrega";
                        break;
                    case "CidadeEntrega":
                        sql = "SELECT ID, CidadeEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY CidadeEntrega";
                        break;
                    case "EstadoEntrega":
                        sql = "SELECT ID, EstadoEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY EstadoEntrega";
                        break;
                    case "CEPCliente":
                        sql = "SELECT ID, CEPCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY CEPCliente";
                        break;
                    case "EnderecoCliente":
                        sql = "SELECT ID, EnderecoCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY EnderecoCliente";
                        break;
                    case "NumeroCliente":
                        sql = "SELECT ID, NumeroCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY NumeroCliente";
                        break;
                    case "ComplementoCliente":
                        sql = "SELECT ID, ComplementoCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY ComplementoCliente";
                        break;
                    case "BairroCliente":
                        sql = "SELECT ID, BairroCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY BairroCliente";
                        break;
                    case "CidadeCliente":
                        sql = "SELECT ID, CidadeCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY CidadeCliente";
                        break;
                    case "LoginOsesp":
                        sql = "SELECT ID, LoginOsesp FROM tCliente WHERE " + FiltroSQL + " ORDER BY LoginOsesp";
                        break;
                    case "EstadoCliente":
                        sql = "SELECT ID, EstadoCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY EstadoCliente";
                        break;
                    case "ClienteIndicacaoID":
                        sql = "SELECT ID, ClienteIndicacaoID FROM tCliente WHERE " + FiltroSQL + " ORDER BY ClienteIndicacaoID";
                        break;
                    case "Obs":
                        sql = "SELECT ID, Obs FROM tCliente WHERE " + FiltroSQL + " ORDER BY Obs";
                        break;
                    case "Senha":
                        sql = "SELECT ID, Senha FROM tCliente WHERE " + FiltroSQL + " ORDER BY Senha";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tCliente WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    case "StatusAtual":
                        sql = "SELECT ID, StatusAtual FROM tCliente WHERE " + FiltroSQL + " ORDER BY StatusAtual";
                        break;
                    case "NomeEntrega":
                        sql = "SELECT ID, NomeEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY NomeEntrega";
                        break;
                    case "CPFEntrega":
                        sql = "SELECT ID, CPFEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY CPFEntrega";
                        break;
                    case "RGEntrega":
                        sql = "SELECT ID, RGEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY RGEntrega";
                        break;
                    case "CPFConsultado":
                        sql = "SELECT ID, CPFConsultado FROM tCliente WHERE " + FiltroSQL + " ORDER BY CPFConsultado";
                        break;
                    case "NomeConsultado":
                        sql = "SELECT ID, NomeConsultado FROM tCliente WHERE " + FiltroSQL + " ORDER BY NomeConsultado";
                        break;
                    case "StatusConsulta":
                        sql = "SELECT ID, StatusConsulta FROM tCliente WHERE " + FiltroSQL + " ORDER BY StatusConsulta";
                        break;
                    case "CPFConsultadoEntrega":
                        sql = "SELECT ID, CPFConsultadoEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY CPFConsultadoEntrega";
                        break;
                    case "NomeConsultadoEntrega":
                        sql = "SELECT ID, NomeConsultadoEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY NomeConsultadoEntrega";
                        break;
                    case "StatusConsultaEntrega":
                        sql = "SELECT ID, StatusConsultaEntrega FROM tCliente WHERE " + FiltroSQL + " ORDER BY StatusConsultaEntrega";
                        break;
                    case "Pais":
                        sql = "SELECT ID, Pais FROM tCliente WHERE " + FiltroSQL + " ORDER BY Pais";
                        break;
                    case "CPFResponsavel":
                        sql = "SELECT ID, CPFResponsavel FROM tCliente WHERE " + FiltroSQL + " ORDER BY CPFResponsavel";
                        break;
                    case "ContatoTipoID":
                        sql = "SELECT ID, ContatoTipoID FROM tCliente WHERE " + FiltroSQL + " ORDER BY ContatoTipoID";
                        break;
                    case "NivelCliente":
                        sql = "SELECT ID, NivelCliente FROM tCliente WHERE " + FiltroSQL + " ORDER BY NivelCliente";
                        break;
                    case "CNPJ":
                        sql = "SELECT ID, CNPJ FROM tCliente WHERE " + FiltroSQL + " ORDER BY CNPJ";
                        break;
                    case "NomeFantasia":
                        sql = "SELECT ID, NomeFantasia FROM tCliente WHERE " + FiltroSQL + " ORDER BY NomeFantasia";
                        break;
                    case "RazaoSocial":
                        sql = "SELECT ID, RazaoSocial FROM tCliente WHERE " + FiltroSQL + " ORDER BY RazaoSocial";
                        break;
                    case "InscricaoEstadual":
                        sql = "SELECT ID, InscricaoEstadual FROM tCliente WHERE " + FiltroSQL + " ORDER BY InscricaoEstadual";
                        break;
                    case "TipoCadastro":
                        sql = "SELECT ID, TipoCadastro FROM tCliente WHERE " + FiltroSQL + " ORDER BY TipoCadastro";
                        break;
                    case "SituacaoProfissionalID":
                        sql = "SELECT ID, SituacaoProfissionalID FROM tCliente WHERE " + FiltroSQL + " ORDER BY SituacaoProfissionalID";
                        break;
                    case "Profissao":
                        sql = "SELECT ID, Profissao FROM tCliente WHERE " + FiltroSQL + " ORDER BY Profissao";
                        break;
                    case "DDDTelefoneComercial2":
                        sql = "SELECT ID, DDDTelefoneComercial2 FROM tCliente WHERE " + FiltroSQL + " ORDER BY DDDTelefoneComercial2";
                        break;
                    case "TelefoneComercial2":
                        sql = "SELECT ID, TelefoneComercial2 FROM tCliente WHERE " + FiltroSQL + " ORDER BY TelefoneComercial2";
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
}