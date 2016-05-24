/**************************************************
* Arquivo: ClienteEndereco.cs
* Gerado: 22/03/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Text;

namespace IRLib
{

    public class ClienteEndereco : ClienteEndereco_B
    {
        public new enumStatusCPF StatusConsulta
        {
            get { return (enumStatusCPF)Enum.Parse(typeof(enumStatusCPF), base.StatusConsulta.Valor.ToString()); }
            set { base.StatusConsulta.Valor = (int)value; }
        }

        public enum enumStatusCPF
        {
            Confirmado = 1,
            Invalido = 2,
            NaoConsultado = 0,
            FalhaConsulta = 3
        }

        public ClienteEndereco() { }

        public ClienteEndereco(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaClienteEndereco> ListaEndereco(int ClienteID)
        {
            try
            {
                List<EstruturaClienteEndereco> lRetorno = new List<EstruturaClienteEndereco>();

                string sql = @"select tClienteEndereco.ID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, tClienteEndereco.Nome, CPF, RG, ClienteID,EnderecoTipoID,tEnderecoTipo.nome as EnderecoTipo, EnderecoPrincipal, StatusConsulta
                from tClienteEndereco 
                inner join tEnderecoTipo on tClienteEndereco.EnderecoTipoID = tEnderecoTipo.ID 
                where ClienteID = " + ClienteID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lRetorno.Add(new EstruturaClienteEndereco
                    {

                        ID = bd.LerInt("ID"),
                        CEP = bd.LerString("CEP"),
                        Endereco = bd.LerString("Endereco"),
                        Numero = bd.LerString("Numero"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Complemento = bd.LerString("Complemento"),
                        Bairro = bd.LerString("Bairro"),
                        Nome = bd.LerString("Nome"),
                        CPF = bd.LerString("CPF"),
                        RG = bd.LerString("RG"),
                        ClienteID = bd.LerInt("ClienteID"),
                        EnderecoTipoID = bd.LerInt("EnderecoTipoID"),
                        EnderecoTipo = bd.LerString("EnderecoTipo"),
                        EnderecoPrincipal = bd.LerBoolean("EnderecoPrincipal"),
                        StatusConsulta = (enumStatusCPF)bd.LerInt("StatusConsulta")

                    });
                }

                bd.Fechar();

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int VerificaEnderecoCliente(int ClienteID, string CEP)
        {
            try
            {
                int ID = 0;

                string sql = @"SELECT ID FROM tClienteEndereco where ClienteID = " + ClienteID + "AND CEP = '" + CEP + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    ID = bd.LerInt("ID");
                }

                bd.Fechar();

                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Inserir(EstruturaClienteEndereco estrutura)
        {
            AtribuirEstrutura(estrutura);
            this.Inserir();
        }

        private void DesmarcarPrincipal(int clienteEnderecoIDPadrao, int clienteID)
        {
            bd.Executar(string.Format("UPDATE tClienteEndereco SET EnderecoPrincipal = 'F' WHERE clienteID = {0} AND ID <> {1} AND EnderecoPrincipal = 'T'", clienteID, clienteEnderecoIDPadrao));
        }

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
                //InserirLog();

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
                sql.Replace("@014", ((int)this.StatusConsulta).ToString());

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);
                //if (result && this.EnderecoPrincipal.Valor)
                //    DesmarcarPrincipal(this.Control.ID, this.ClienteID.Valor);
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

        public override bool Inserir()
        {

            try
            {
                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();

                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tClienteEndereco(ClienteID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, EnderecoTipoID, EnderecoPrincipal,StatusConsulta) ");
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
                sql.Replace("@014", ((int)this.StatusConsulta).ToString());

                int id = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

                bool result = (id >= 1);

                if (result)
                {
                    if (this.EnderecoPrincipal.Valor)
                        DesmarcarPrincipal(id, this.ClienteID.Valor);

                    this.Control.ID = id;
                    InserirControle("I");
                }

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

        private void LimparEnderecoPrincipal()
        {
            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tClienteEndereco SET  EnderecoPrincipal = 'F' ");
                sql.Append("WHERE ClienteID = @001");
                sql.Replace("@001", this.ClienteID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bd.FinalizarTransacao();


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

        public void Atualizar(EstruturaClienteEndereco estrutura)
        {
            AtribuirEstrutura(estrutura);
            this.Atualizar();

        }

        private void AtribuirEstrutura(EstruturaClienteEndereco estrutura)
        {
            this.Control.ID = estrutura.ID;
            this.CEP.Valor = estrutura.CEP;
            this.Endereco.Valor = estrutura.Endereco;
            this.Numero.Valor = estrutura.Numero;
            this.Cidade.Valor = estrutura.Cidade;
            this.Estado.Valor = estrutura.Estado;
            this.Complemento.Valor = estrutura.Complemento;
            this.Bairro.Valor = estrutura.Bairro;
            this.Nome.Valor = estrutura.Nome;
            this.CPF.Valor = estrutura.CPF;
            this.RG.Valor = estrutura.RG;
            this.ClienteID.Valor = estrutura.ClienteID;
            this.EnderecoTipoID.Valor = estrutura.EnderecoTipoID;
            this.EnderecoPrincipal.Valor = estrutura.EnderecoPrincipal;
            this.StatusConsulta = estrutura.StatusConsulta;

            if (estrutura.EnderecoPrincipal)
            {
                this.LimparEnderecoPrincipal();
            }

        }

        public bool PossuiAgendada(int EnderecoID)
        {
            try
            {
                bool retorno = false;
                string sql = @"select count(ClienteEnderecoID) as qtdVenda
                            from tVendaBilheteria (nolock)
                            left join tEntregaAgenda on tVendaBilheteria.EntregaAgendaID = tEntregaAgenda.ID
                            where ClienteEnderecoID = " + EnderecoID + " and tEntregaAgenda.Data > '" + DateTime.Now.ToString("yyyyMMdd") + "' ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("qtdVenda") > 0)
                    {
                        retorno = true;
                    }
                }

                bd.Fechar();

                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool PossuiVenda(int EnderecoID)
        {
            try
            {
                bool retorno = false;
                string sql = @"select count(ClienteEnderecoID) as qtdVenda
                            from tVendaBilheteria (nolock)
                            where ClienteEnderecoID = " + EnderecoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (bd.LerInt("qtdVenda") > 0)
                    {
                        retorno = true;
                    }
                }

                bd.Fechar();

                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EstruturaClienteEndereco LerEstrutura(int ClienteEnderecoID)
        {
            EstruturaClienteEndereco retorno = new EstruturaClienteEndereco();

            string sql = @"select tClienteEndereco.ID, CEP, Endereco, Numero, 
                Cidade, Estado, Complemento, Bairro, tClienteEndereco.Nome, 
                ISNULL(CPF,'-') as CPF, RG, ClienteID,EnderecoTipoID,tEnderecoTipo.nome as EnderecoTipo, EnderecoPrincipal, StatusConsulta
                from tClienteEndereco 
                inner join tEnderecoTipo on tClienteEndereco.EnderecoTipoID = tEnderecoTipo.ID 
                where tClienteEndereco.ID = " + ClienteEnderecoID;

            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {

                retorno.ID = bd.LerInt("ID");
                retorno.CEP = bd.LerString("CEP");
                retorno.Endereco = bd.LerString("Endereco");
                retorno.Numero = bd.LerString("Numero");
                retorno.Cidade = bd.LerString("Cidade");
                retorno.Estado = bd.LerString("Estado");
                retorno.Complemento = bd.LerString("Complemento");
                retorno.Bairro = bd.LerString("Bairro");
                retorno.Nome = bd.LerString("Nome");
                retorno.CPF = bd.LerString("CPF");
                retorno.RG = bd.LerString("RG");
                retorno.ClienteID = bd.LerInt("ClienteID");
                retorno.EnderecoTipoID = bd.LerInt("EnderecoTipoID");
                retorno.EnderecoTipo = bd.LerString("EnderecoTipo");
                retorno.EnderecoPrincipal = bd.LerBoolean("EnderecoPrincipal");
                retorno.StatusConsulta = (enumStatusCPF)bd.LerInt("StatusConsulta");

            }

            bd.Fechar();

            return retorno;
        }

        private bool ValidacaoAtiva()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SCPC_Ativo"]);
        }

        public IRLib.Codigo.Brainiac.Retorno CadastroValido(string cpf, string nome, enumStatusCPF status)
        {

            IRLib.Codigo.Brainiac.Retorno retornoBrainiac;

            switch (status)
            {
                // Se o cliente nunca foi consultado, efetua a consulta.
                case enumStatusCPF.NaoConsultado:
                    // Efetua a consulta e verifica o retorno.
                    retornoBrainiac = ConsultaCPF(cpf, nome);
                    return retornoBrainiac;// == enumStatusCPF.Confirmado;

                case enumStatusCPF.Confirmado:
                    return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

                default:
                    throw new ClienteException("Não existe status associado ao cliente!");
            }// fim switch (status)
        }

        public IRLib.Codigo.Brainiac.Retorno ConsultaCPF(string CPF, string nome)
        {
            try
            {

                string nomeRet = "";

                IRLib.Utilitario Utilitario = new Utilitario();

                #region Variaveis
                //Código e senha para acessar o sistema de TESTES
                string codigo_teste = ConfigurationManager.AppSettings["SCPC_codigo_teste"];
                string senha_teste = ConfigurationManager.AppSettings["SCPC_senha_teste"];

                //Código e senha para acessar o sistema de PRODUÇÃO
                string codigo_prod = ConfigurationManager.AppSettings["SCPC_codigo_prod"];
                string senha_prod = ConfigurationManager.AppSettings["SCPC_senha_prod"];

                //URL's para acessar o sistema de TESTES e PRODUÇÃO
                string url_teste = ConfigurationManager.AppSettings["SCPC_url_teste"];
                string url_prod = ConfigurationManager.AppSettings["SCPC_url_prod"];

                string transacao = ConfigurationManager.AppSettings["SCPC_transacao"];
                string versao = ConfigurationManager.AppSettings["SCPC_versao"];
                string indicadorFimTexto = ConfigurationManager.AppSettings["SCPC_indicadorFimTexto"];
                string tipoConsulta = ConfigurationManager.AppSettings["SCPC_tipoConsulta"];

                string _s5 = "     ";
                string _s10 = "          ";
                string urlConsulta = String.Empty;

                // TODO: remover esta linha - temporário para utilização sem SSL.
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);


                /*

                 * Campos para a URL de COnsulta:
                 * 
                * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
                 * Coluna 1: Ordem
                 * Coluna 2: Campo
                 * Coluna 3: Posição Inicial
                 * Coluna 4: Posição Final
                 * Coluna 5: Tam. Byte
                 * Coluna 6: Formato - Tipo
                 * Coluna 7: Formato - Casas Decimais
                 * Coluna 8: Conteúdo
                * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
                |C1||C2                       | |C3|    |C4|    |C5|    |C6|   |C7| |C8|
                01  TRANSAÇÃO                   001     008     008     X           “CSR50001”
                02  VERSÃO                      009     010     002     X           “01” 
                03  RESERVADO SOLICITANTE       011     020     010     X           USO DO SOLICITANTE                                                 (Informar somente letras                                                maiúsculas, sem acentuação ou                                                números)
                04  RESERVADO ACSP              021     040     020     X           USO DA ACSP
                05  CÓDIGO                      041     048     008     N       0   CÓDIGO DE SERVIÇO
                06  SENHA                       049     056     008     X           SENHA DE ACESSO
                07  CONSULTA                    057     064     008     X           “CONF” – Conferencia de Nomes                                                     “SCAD” – Sintese P/ Documento                                                    “FONE” – Sintese P/ Nome + Data
                08  CPF                         065     075     011     N       0   NÚMERO DO CPF COMPLETO
                09  NOME                        076     125     050     X           NOME (Preencher apenas 40 posições)
                10  DATA                        126     133     008     N       0   FORMATO: “DDMMAAAA”
                11  RESERVADO                   134     148     015     X           BRANCOS
                12  INDICADOR DE FIM DE TEXTO   149     150     002     X           X “0DA0” ou X“0D25”
                * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
                Campos obrigatórios: 
                 * Transação, 
                 * Versão, 
                 * Código de Serviço, 
                 * Senha de Acesso,
                 * Consulta e Indicador de Fim de Texto, 
                 * CPF para a consulta “SCAD”, 
                 * Nome e Data de Nascimento 
                 */

                /*
                 EXEMPLO:
                    ....+....1....+....2....+....3....+....4....+....5....+....6....+....7....+....8
                    CSR5000101RRRRRRRRRR                    12345678SENHA  CONSULTA12345678901NOME
                    ....+....9....+....0....+....1....+....2....+....3....+....4....+....5
                                                                 DDMMAAAA
                 */
                #endregion

                #region MontaStringURL
                //Tamanho 150 posições;
                //1-8
                urlConsulta = transacao;
                //9-10
                urlConsulta += versao;
                //11-20
                urlConsulta += _s10;
                //21-40
                urlConsulta += _s10 + _s10;
                //41-48  
                //urlConsulta += codigo_teste;
                urlConsulta += codigo_prod;
                //49-56  
                //urlConsulta += senha_teste;
                urlConsulta += senha_prod;
                //57-64  
                urlConsulta += tipoConsulta;
                //65-75  
                urlConsulta += CPF;
                //76-125  
                //urlConsulta += nomeBD + _s10 + _s10 + _s5 + _s + _s + _s;


                // TODO: REmover esse for.
                for (int i = 1; i <= 50; i++)
                {
                    i = nome.Length;
                    nome += " ";
                }

                urlConsulta += nome.Substring(0, 50);
                //126-133
                urlConsulta += DateTime.Now.ToString("ddMMyyyy");
                //134-148
                urlConsulta += _s10 + _s5;
                //149-150
                urlConsulta += indicadorFimTexto;

                urlConsulta = urlConsulta.Replace(" ", "%20");

                //string urlFinal = url_teste += urlConsulta;
                string urlFinal = url_prod += urlConsulta;

                int totalPosicoes = urlConsulta.Length;

                #endregion

                // TODO: Verificar se deu erro ou não = Algo sobre asteriscos.
                string strUrlRetorno = IRLib.Utilitario.HTTPGetPage(urlFinal).Trim().Replace("<PRE>", "").Replace("</PRE>", "").Replace("\n", "").Replace("\r", "");
                nomeRet = strUrlRetorno.Replace(" ", "$");
                nomeRet = nomeRet.Substring(73, 70).Replace("$$", "").Replace("$", " ").ToUpper().Trim();

                if (nomeRet.StartsWith("*"))
                    return new IRLib.Codigo.Brainiac.Retorno()
                    {
                        TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Parcial,
                        Mensagem = "Falha na Consulta."
                    };

                // Verifica se o nome digitado pelo cliente é igual ao retornado na consulta.
                return
                    new IRLib.Codigo.Brainiac.Gerenciador()
                    .IniciarNomes(nomeRet, nome);

            }
            catch (WebException)
            {
                return new IRLib.Codigo.Brainiac.Retorno()
                {
                    TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro,
                    Mensagem = "Falha na Consulta."
                };
            }
        }

        public void SalvarConsultaCPF(string clienteEnderecoID)
        {
            try
            {

                // Monta a string para salvar apenas os campos de consulta de CPF.
                string sql = string.Format("UPDATE tClienteEndereco SET StatusConsulta = {0} WHERE ID = {1}", base.StatusConsulta.ValorBD, clienteEnderecoID);

                // True se atualizar uma única linha.
                if (bd.Executar(sql) != 1)
                    throw new ClienteException("Falha ao atualizar os dados do cliente!");
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IRLib.Codigo.Brainiac.Retorno CadastroValido()
        {
            if (!ValidacaoAtiva())
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            if (this.CPF.Valor.Length == 0)
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro, Mensagem = "CPF Inválido." };

            if (this.StatusConsulta == enumStatusCPF.Confirmado)
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };


            IRLib.Codigo.Brainiac.Retorno retornoBrainiac =
                 CadastroValido(this.CPF.Valor, this.Nome.Valor, this.StatusConsulta);

            switch (retornoBrainiac.TipoRetorno)
            {
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok:
                    // Confirmado, salva no banco.
                    this.StatusConsulta = enumStatusCPF.Confirmado;
                    SalvarConsultaCPF(this.Control.ID.ToString());
                    break;
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro:
                    // cadastro inválido!
                    this.StatusConsulta = enumStatusCPF.Invalido;
                    SalvarConsultaCPF(this.Control.ID.ToString());
                    break;
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Parcial:
                    break;
                default:
                    throw new Exception("Falha ao consultar o CPF!");
            }

            return retornoBrainiac;
        }

        public void BuscarPorCliente(int clienteID)
        {
            try
            {
                string sql =
                    string.Format(@"
                        SELECT
                            TOP 1 ID, CEP
                        FROM tClienteEndereco ce (NOLOCK) WHERE ClienteID = {0} AND EnderecoPrincipal = 'T'
                    ", clienteID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível encontrar seu endereço principal para efetuar a entrega. Por favor entre em cotanto com a Ingresso Rápido.");

                this.Control.ID = bd.LerInt("ID");
                this.CEP.Valor = bd.LerString("CEP");

            }
            finally
            {
                bd.Fechar();
            }
        }

        public string TipoEndereco(int TipoID)
        {
            try
            {

                string retorno = "";

                string sql = @"SELECT nome 
                    FROM tEnderecoTipo WHERE id = " + TipoID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = bd.LerString("nome");
                }

                return retorno;
            }
            finally
            {

            }
        }
    }

    public class ClienteEnderecoLista : ClienteEnderecoLista_B
    {

        public ClienteEnderecoLista() { }

        public ClienteEnderecoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }




}

