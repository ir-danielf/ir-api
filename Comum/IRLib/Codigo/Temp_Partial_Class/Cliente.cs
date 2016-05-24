using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Text;

namespace IRLib
{
    public partial class Cliente
    {
        public new enumStatusCPF StatusConsulta
        {
            get { return (enumStatusCPF)Enum.Parse(typeof(enumStatusCPF), base.StatusConsulta.Valor.ToString()); }
            set { base.StatusConsulta.Valor = (int)value; }
        }
        public new enumStatusCPF StatusConsultaEntrega
        {
            get { return (enumStatusCPF)Enum.Parse(typeof(enumStatusCPF), base.StatusConsultaEntrega.Valor.ToString()); }
            set { base.StatusConsultaEntrega.Valor = (int)value; }
        }

        public const string CLIENTE_ID = "clienteID";
        public const string VENDA_ID = "vendaBilheteriaID";
        public const string NOME = "nome";
        public const string ENDERECO = "endereco";
        public const string TELEFONE = "telefone";
        public const string TELEFONE_COMERCIAL = "telefoneComercial";
        public const string TELEFONE_CELULAR = "telefoneCelular";
        public const string EMAIL = "email";
        public const string PREFERENCIA_CONTATO = "preferenciaContato";
        public const string QTD_INGRESSOS_CLIENTE = "qtdIngressosCliente";

        public const string SENHA = "senha";
        public const string VALOR_TOTAL = "valorTotal";
        public const string COMPRA_CANCELADA = "compraCancelada";
        public const string QTD_INGRESSO = "qtdIngresso";
        public const string FORMA_PAGAMENTO = "formaPagamento";

        public const string CODIGO_INGRESSO = "codigoIngresso";
        public const string APRESENTACAO = "apresentacao";
        public const string SETOR = "setor";
        public const string PRECO = "preco";
        public const string CODIGO_BARRAS = "codigoBarras";
        public const string STATUS_INGRESSO = "statusIngresso";

        public const string TAB_CLIENTE = "Cliente";
        public const string TAB_ENDERECO_CLIENTE = "EnderecoCliente";
        public const string TAB_COMPRA = "Compra";
        public const string TAB_INGRESSO = "Ingresso";

        public enum enumStatusCPF
        {
            Confirmado = 1,
            Invalido = 2,
            NaoConsultado = 0,
            FalhaConsulta = 3
        }

        public void SalvarConsultaCPF(string clienteID)
        {
            try
            {

                // Monta a string para salvar apenas os campos de consulta de CPF.
                string sql = string.Format("UPDATE tCliente SET CPFConsultado = '{0}', NomeConsultado = '{1}', StatusConsulta = {2} WHERE ID = {3}",
                    this.CPF.ValorBD, this.NomeConsultado.ValorBD, base.StatusConsulta.ValorBD, clienteID);

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

        public void SalvarConsultaCPFEntrega()
        {
            try
            {
                // Monta a string para salvar apenas os campos de consulta de CPF.
                string sql = string.Format("UPDATE tCliente SET CPFConsultadoEntrega = '{0}', NomeConsultadoEntrega = '{1}', StatusConsultaEntrega = {2} WHERE ID = {3}",
                    this.CPFEntrega.ValorBD, this.NomeConsultadoEntrega.ValorBD, base.StatusConsultaEntrega.ValorBD, this.Control.ID.ToString());

                // True se atualizar uma única linha.
                if (bd.Executar(sql) != 1)
                    throw new ClienteException("Falha ao atualizar os dados do cliente!");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<string> SCPC_RetornaNomes(List<string> cpfs, List<string> nomes)
        {
            try
            {
                if (cpfs.Count != nomes.Count)
                    throw new Exception("O número de cpfs deve ser identico ao número de nomes");

                List<string> retorno = new List<string>();
                IRLib.Utilitario Utilitario = new Utilitario();
                string nomeRet = string.Empty;

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
                string nome = string.Empty;

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
                for (int i = 0; i < cpfs.Count; i++)
                {


                    urlConsulta = String.Empty;
                    nome = nomes[i];

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
                    urlConsulta += cpfs[i];
                    //76-125  
                    //urlConsulta += nomeBD + _s10 + _s10 + _s5 + _s + _s + _s;


                    //// TODO: REmover esse for.
                    //for (int k = 1; k <= 50; k++)
                    //{
                    //    k = nome.Length;
                    //    nome += " ";
                    //}

                    urlConsulta += nome.Length >= 50 ? nome.Substring(0, 50) : nome;



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
                    retorno.Add(nomeRet);
                }

                return retorno;

            }
            catch
            {
                throw;
            }
        }

        public IRLib.Codigo.Brainiac.Retorno ConsultaCPF(string CPF, string nome, ref string nomeRet)
        {
            try
            {
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

        private bool ValidacaoAtiva()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SCPC_Ativo"]);
        }

        public IRLib.Codigo.Brainiac.Retorno CadastroValido()
        {
            if (!ValidacaoAtiva())
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            if (this.Pais.Valor != "Brasil")
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            if (this.CPF.Valor.Length == 0)
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro, Mensagem = "CPF Inválido." };

            if (this.StatusConsulta == enumStatusCPF.Confirmado)
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            string retNomeConsultado = string.Empty;

            int[] clienteID = BuscarClienteCPF(this.CPF.Valor);
            IRLib.Codigo.Brainiac.Retorno retornoBrainiac =
                 CadastroValido(this.CPF.Valor, this.Nome.Valor, this.CPFConsultado.Valor, this.NomeConsultado.Valor, this.StatusConsulta, ref retNomeConsultado);

            switch (retornoBrainiac.TipoRetorno)
            {
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok:
                    // Confirmado, salva no banco.
                    this.CPFConsultado.Valor = this.CPF.Valor;
                    this.NomeConsultado.Valor = retNomeConsultado;
                    this.StatusConsulta = enumStatusCPF.Confirmado;
                    SalvarConsultaCPF(clienteID[1].ToString());
                    break;
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro:
                    // cadastro inválido!
                    this.CPFConsultado.Valor = this.CPF.Valor;
                    this.NomeConsultado.Valor = retNomeConsultado;
                    this.StatusConsulta = enumStatusCPF.Invalido;
                    SalvarConsultaCPF(clienteID[1].ToString());
                    break;
                case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Parcial:
                    break;
                default:
                    throw new Exception("Falha ao consultar o CPF!");
            }

            return retornoBrainiac;
        }

        public IRLib.Codigo.Brainiac.Retorno CadastroEntregaValido()
        {
            if (!ValidacaoAtiva())
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            if (this.Pais.Valor != "Brasil")
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

            // Método só estará disponível para utilização se o objeto estiver populado.
            if (this.Control.ID.Equals(0))
                throw new ArgumentException("Cliente não encontrado para validação!");

            ////Compara os nomes e atribui status;

            string retNomeConsultadoEntrega = string.Empty;
            IRLib.Codigo.Brainiac.Retorno retornoBrainiac = null;

            //Entrou com Nome de entrega Igual ao do Cliente -- Passa direto sem precisar validar novamente com a SCSP
            if (this.CPFEntrega.Valor == this.CPF.Valor &&
                Utilitario.CompararSemAcentos(this.Nome.Valor, this.NomeEntrega.Valor) == 0 &&
                this.StatusConsulta == enumStatusCPF.Confirmado)
            {
                retornoBrainiac =
                    new IRLib.Codigo.Brainiac.Gerenciador().IniciarNomes(this.Nome.Valor, this.NomeEntrega.Valor);

                switch (retornoBrainiac.TipoRetorno)
                {
                    case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok:
                        this.CPFConsultadoEntrega.Valor = this.CPFEntrega.Valor;
                        this.NomeConsultadoEntrega.Valor = this.NomeEntrega.Valor;
                        base.StatusConsultaEntrega.Valor = (int)enumStatusCPF.Confirmado;
                        SalvarConsultaCPFEntrega();
                        break;
                    case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro:
                        base.StatusConsultaEntrega.Valor = (int)enumStatusCPF.Invalido;
                        this.CPFConsultadoEntrega.Valor = this.CPFEntrega.Valor;
                        this.NomeConsultadoEntrega.Valor = this.NomeEntrega.Valor;
                        SalvarConsultaCPFEntrega();
                        break;
                }
                return retornoBrainiac;
            }
            // 1.0 Verifica se os valores de entrega anteriores sao diferentes dos que estao agora, se estiver, 1.1 ou 1.2
            else if (this.CPFEntrega.Valor != this.CPFConsultadoEntrega.Valor ||
                (Utilitario.CompararSemAcentos(this.NomeEntrega.Valor.Trim(), this.NomeConsultadoEntrega.Valor.Trim()) != 0) ||
                this.StatusConsultaEntrega != enumStatusCPF.Confirmado)
            {
                retornoBrainiac = this.CadastroValido(this.CPFEntrega.Valor, this.NomeEntrega.Valor, this.CPFConsultadoEntrega.Valor, this.NomeConsultadoEntrega.Valor,
                    this.StatusConsultaEntrega == enumStatusCPF.NaoConsultado ? enumStatusCPF.NaoConsultado : enumStatusCPF.Invalido, ref retNomeConsultadoEntrega);

                switch (retornoBrainiac.TipoRetorno)
                {
                    //1.1 Verifica o Cadastro forcando o Status Invalido
                    case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok:
                        // Confirmado, salva no banco. sem precisar ir até a SCSP - Ja havia sido consultado.
                        this.CPFConsultadoEntrega.Valor = this.CPFEntrega.Valor;
                        this.NomeConsultadoEntrega.Valor = retNomeConsultadoEntrega;
                        base.StatusConsultaEntrega.Valor = (int)enumStatusCPF.Confirmado;
                        SalvarConsultaCPFEntrega();
                        break;

                    //1.2 Atribui o Status Invalido para mostrar no Retorno
                    case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro:
                        base.StatusConsultaEntrega.Valor = (int)enumStatusCPF.Invalido;
                        if (!string.IsNullOrEmpty(retNomeConsultadoEntrega))
                        {
                            this.CPFConsultadoEntrega.Valor = this.CPFEntrega.Valor;
                            this.NomeConsultadoEntrega.Valor = retNomeConsultadoEntrega;
                        }
                        SalvarConsultaCPFEntrega();
                        break;
                    case IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Parcial:
                        break;
                    default:
                        throw new Exception("Falha ao consultar o CPF!");

                }
                return retornoBrainiac;
            }
            //Utiliziou o mesmo Cliente da compra anterior nesta..
            else if (this.StatusConsultaEntrega == enumStatusCPF.Confirmado)
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };
            //Fez a pesquisa no SCPC no só que colocou nome errado, como esta digitado corretamente, só atribui o status de Confirmado
            else
            {
                this.StatusConsultaEntrega = enumStatusCPF.Confirmado;
                this.SalvarConsultaCPFEntrega();
                return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };
            }
        }

        public bool CamposPreenchidos()
        {
            if (this.Nome.Valor.Length < 6)
                return false;
            else if (this.Pais.Valor == "Brasil" && this.RG.Valor.Length < 5 && this.CNPJ.Valor.Length == 0)
                return false;
            else if (this.Pais.Valor == "Brasil" && !Utilitario.IsCPF(this.CPF.Valor))
                return false;
            else if (this.Telefone.Valor.Length == 0 && this.TelefoneComercial.Valor.Length == 0 && this.Celular.Valor.Length == 0)
                return false;
            else if (this.DDDCelular.Valor.Length == 0 && this.DDDTelefone.Valor.Length == 0 && this.DDDTelefoneComercial.Valor.Length == 0)
                return false;
            else if (this.CEPCliente.Valor.Length < 8)
                return false;
            else if (this.EnderecoCliente.Valor.Length == 0)
                return false;
            else if (this.BairroCliente.Valor.Length == 0)
                return false;
            else if (this.NumeroCliente.Valor.Length == 0)
                return false;
            else if (this.CidadeCliente.Valor.Length == 0)
                return false;
            else if (this.EstadoCliente.Valor.Length == 0)
                return false;
            else if (this.DataNascimento.Valor >= DateTime.Now || this.DataNascimento.Valor <= DateTime.Now.AddYears(-99))
                return false;
            else
                return true;
        }

        public static DataSet EstruturaBuscaRelatorioClientesCompras()
        {

            DataSet ds = new DataSet("RelatorioClientesCompras");

            DataTable tCliente = new DataTable(TAB_CLIENTE);
            DataTable tCompra = new DataTable(TAB_COMPRA);
            DataTable tIngresso = new DataTable(TAB_INGRESSO);

            tCliente.Columns.Add(CLIENTE_ID, typeof(int)).DefaultValue = 0;
            tCliente.Columns.Add(NOME, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(TELEFONE, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(TELEFONE_COMERCIAL, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(TELEFONE_CELULAR, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(EMAIL, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(PREFERENCIA_CONTATO, typeof(string)).DefaultValue = "";
            tCliente.Columns.Add(QTD_INGRESSOS_CLIENTE, typeof(int)).DefaultValue = 0;
            tCliente.Columns.Add(ENDERECO, typeof(string)).DefaultValue = "";


            tCompra.Columns.Add(VENDA_ID, typeof(int)).DefaultValue = 0;
            tCompra.Columns.Add(CLIENTE_ID, typeof(int)).DefaultValue = 0;
            tCompra.Columns.Add(SENHA, typeof(string)).DefaultValue = "";
            tCompra.Columns.Add(VALOR_TOTAL, typeof(string)).DefaultValue = 0.0M;
            tCompra.Columns.Add(COMPRA_CANCELADA, typeof(string)).DefaultValue = "Não";
            tCompra.Columns.Add(QTD_INGRESSO, typeof(int)).DefaultValue = 0;
            tCompra.Columns.Add(FORMA_PAGAMENTO, typeof(string)).DefaultValue = "";

            tIngresso.Columns.Add(VENDA_ID, typeof(int)).DefaultValue = 0;
            tIngresso.Columns.Add(CODIGO_INGRESSO, typeof(string)).DefaultValue = "";
            tIngresso.Columns.Add(APRESENTACAO, typeof(string)).DefaultValue = "";
            tIngresso.Columns.Add(SETOR, typeof(string)).DefaultValue = "";
            tIngresso.Columns.Add(PRECO, typeof(string)).DefaultValue = "";
            tIngresso.Columns.Add(CODIGO_BARRAS, typeof(string)).DefaultValue = "";
            tIngresso.Columns.Add(STATUS_INGRESSO, typeof(string)).DefaultValue = "";


            ds.Tables.Add(tCliente);

            ds.Tables.Add(tCompra);
            ds.Tables.Add(tIngresso);





            //Cliente com Compra
            DataColumn colCliente = tCliente.Columns[CLIENTE_ID];
            DataColumn colCompra = tCompra.Columns[CLIENTE_ID];
            DataRelation dr1 = new DataRelation("CompraXCliente", colCliente, colCompra);
            ForeignKeyConstraint idKeyRestraint1 = new ForeignKeyConstraint(colCliente, colCompra);
            idKeyRestraint1.DeleteRule = Rule.Cascade;
            tCompra.Constraints.Add(idKeyRestraint1);

            //Cliente com Compra

            DataColumn colCompraSenha = tCompra.Columns[VENDA_ID];
            DataColumn colIngresso = tIngresso.Columns[VENDA_ID];
            DataRelation dr2 = new DataRelation("IngressoXCompra", colCompraSenha, colIngresso);
            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colCompraSenha, colIngresso);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tIngresso.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;


            ds.Relations.Add(dr1);
            ds.Relations.Add(dr2);

            return ds;

        }

        public int QtdIngressosComprados(int ClienteID, string filtro)
        {
            int qtd;
            try
            {
                qtd = 0;
                string sql = @"SELECT COUNT(tIngressoLog.ClienteID) as qtd 
								FROM tIngressoLog
								INNER JOIN tIngresso ON (tIngresso.ID = tIngressoLog.IngressoID) 
								" + filtro + " and tIngressoLog.ClienteID = " + ClienteID + @"
								AND Acao = 'V' GROUP BY tIngressoLog.ClienteID";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    qtd = bd.LerInt("qtd");
                }

                return qtd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public DataSet RelatorioClientesCompras(int ApresentacaoID, int EventoID)
        {
            try
            {

                DataSet retorno = EstruturaBuscaRelatorioClientesCompras();
                string sql = "";
                string filtro = "";

                if (ApresentacaoID > 0)
                {
                    filtro = "WHERE ApresentacaoID = " + ApresentacaoID;
                }
                else
                {
                    filtro = "WHERE tIngresso.EventoID = " + EventoID;
                }



                sql = @"SELECT tIngressoLog.ClienteID,CASE WHEN tCliente.CNPJ  <> ''
						THEN tCliente.NomeFantasia
						ELSE tCliente.Nome COLLATE Latin1_General_CI_AI
						END AS Nome,tCliente.DDDTelefone,tCliente.Telefone,tCliente.DDDTelefoneComercial,
						tCliente.TelefoneComercial,tCliente.DDDCelular,tCliente.Celular,tCliente.Email,tCliente.RecebeEmail,
						tCliente.CEPCliente,tCliente.EnderecoCliente,tCliente.NumeroCliente,tCliente.CidadeCliente,tCliente.EstadoCliente,
						tCliente.ComplementoCliente,tCliente.BairroCliente,tCliente.ContatoTipoID
						FROM tIngressoLog
						INNER JOIN tIngresso ON (tIngresso.ID = tIngressoLog.IngressoID)
						INNER JOIN tCliente ON (tIngressoLog.ClienteID=tCliente.ID)  " + filtro + @" 
						 GROUP BY tIngressoLog.ClienteID,tCliente.Nome,tCliente.DDDTelefone,tCliente.Telefone,tCliente.DDDTelefoneComercial,
						tCliente.TelefoneComercial,tCliente.DDDCelular,tCliente.Celular,tCliente.Email,tCliente.RecebeEmail,
						tCliente.CEPCliente,tCliente.EnderecoCliente,tCliente.NumeroCliente,tCliente.CidadeCliente,tCliente.EstadoCliente,
						tCliente.ComplementoCliente,tCliente.BairroCliente,tCliente.ContatoTipoID, tCliente.CNPJ ,tCliente.NomeFantasia";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.Tables[TAB_CLIENTE].NewRow();
                    int clienteID = bd.LerInt("ClienteID");
                    linha[CLIENTE_ID] = clienteID;
                    linha[NOME] = bd.LerString("Nome");
                    string endereco = bd.LerString("EnderecoCliente") + "," + bd.LerString("NumeroCliente") + "," + bd.LerString("ComplementoCliente") + "," + bd.LerString("BairroCliente") + "," + bd.LerString("CidadeCliente") + "," + bd.LerString("EstadoCliente") + "," + bd.LerString("CEPCliente");
                    linha[ENDERECO] = endereco.Replace(",,", ",");
                    linha[TELEFONE] = bd.LerString("DDDTelefone") + "-" + bd.LerString("Telefone");
                    linha[TELEFONE_COMERCIAL] = bd.LerString("DDDTelefoneComercial") + "-" + bd.LerString("TelefoneComercial");
                    linha[TELEFONE_CELULAR] = bd.LerString("DDDCelular") + "-" + bd.LerString("Celular");
                    linha[EMAIL] = bd.LerString("Email");

                    ContatoTipo oContatoTipo = new ContatoTipo();

                    linha[PREFERENCIA_CONTATO] = oContatoTipo.TipoContato(bd.LerInt("ContatoTipoID"));

                    retorno.Tables[TAB_CLIENTE].Rows.Add(linha);
                }

                bd.Fechar();

                foreach (DataRow linha in retorno.Tables[TAB_CLIENTE].Rows)
                {
                    linha[QTD_INGRESSOS_CLIENTE] = QtdIngressosComprados((int)linha[CLIENTE_ID], filtro);

                }



                sql = @"SELECT tIngressoLog.VendaBilheteriaID,tIngressoLog.ClienteID,
						COUNT(tIngressoLog.VendaBilheteriaID) AS QtdIngressos,
						tVendaBilheteria.Senha, 
						ValorTotal, ISNULL(VendaCancelada,'F') AS VendaCancelada
						FROM tIngressoLog
						INNER JOIN tIngresso ON (tIngresso.ID = tIngressoLog.IngressoID)
						INNER JOIN tCliente ON (tIngressoLog.ClienteID=tCliente.ID)
						INNER JOIN tVendaBilheteria ON (tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID) " + filtro + @" 
						and Acao = 'V' GROUP BY tIngressoLog.VendaBilheteriaID,tIngressoLog.ClienteID,tIngressoLog.VendaBilheteriaID,
						tVendaBilheteria.Senha, ValorTotal, VendaCancelada";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.Tables[TAB_COMPRA].NewRow();

                    int clienteID = bd.LerInt("ClienteID");
                    linha[VENDA_ID] = bd.LerInt("VendaBilheteriaID");
                    linha[CLIENTE_ID] = clienteID;
                    linha[SENHA] = bd.LerString("Senha");
                    linha[VALOR_TOTAL] = bd.LerDecimal("ValorTotal").ToString("c");
                    linha[COMPRA_CANCELADA] = bd.LerString("VendaCancelada") == "T" ? "Sim" : "Não";
                    linha[QTD_INGRESSO] = bd.LerInt("QtdIngressos");

                    retorno.Tables[TAB_COMPRA].Rows.Add(linha);
                }

                bd.Fechar();

                VendaBilheteria oVendaBilheteria = new VendaBilheteria();

                foreach (DataRow linha in retorno.Tables[TAB_COMPRA].Rows)
                {
                    linha[FORMA_PAGAMENTO] = oVendaBilheteria.FormasPagametoString((int)linha[VENDA_ID]);

                }

                sql = @"SELECT tIngressoLog.VendaBilheteriaID,tIngresso.Codigo, tApresentacao.Horario, 
						tSetor.Nome as Setor, tPreco.Nome as Preco,tPreco.Valor ,tIngresso.CodigoBarra,tIngresso.Status
						FROM tIngressoLog
						INNER JOIN tIngresso ON (tIngresso.ID = tIngressoLog.IngressoID)
						INNER JOIN tCliente ON (tIngressoLog.ClienteID=tCliente.ID)
						INNER JOIN tApresentacao ON (tIngresso.ApresentacaoID=tApresentacao.ID)
						INNER JOIN tSetor ON (tIngresso.SetorID=tSetor.ID)
						INNER JOIN tPreco ON (tIngressoLog.PrecoID=tPreco.ID)  " + filtro +
                        @"  AND Acao = 'V'  AND tIngressoLog.ClienteID > 0 ";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.Tables[TAB_INGRESSO].NewRow();


                    linha[VENDA_ID] = bd.LerInt("VendaBilheteriaID");
                    linha[CODIGO_INGRESSO] = bd.LerString("Codigo"); ;
                    string dataApresentacao = bd.LerDateTime("Horario").ToString();
                    linha[APRESENTACAO] = dataApresentacao.Remove(dataApresentacao.Length - 3);
                    linha[SETOR] = bd.LerString("Setor");
                    linha[PRECO] = bd.LerString("Preco") + " - " + bd.LerDecimal("Valor").ToString("c");
                    linha[CODIGO_BARRAS] = bd.LerString("CodigoBarra");
                    linha[STATUS_INGRESSO] = Ingresso.StatusDescritivo(bd.LerString("Status"));
                    retorno.Tables[TAB_INGRESSO].Rows.Add(linha);
                }

                bd.Fechar();

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public IRLib.Codigo.Brainiac.Retorno CadastroValido
            (string cpf, string nome, string cpfConsultado, string nomeConsultado, enumStatusCPF status, ref string retNomeConsultado)
        {

            IRLib.Codigo.Brainiac.Retorno retornoBrainiac;

            switch (status)
            {
                // Se o cliente nunca foi consultado, efetua a consulta.
                case enumStatusCPF.NaoConsultado:
                    // Efetua a consulta e verifica o retorno.
                    retornoBrainiac = ConsultaCPF(cpf, nome, ref retNomeConsultado);
                    return retornoBrainiac;// == enumStatusCPF.Confirmado;
                case enumStatusCPF.Invalido:
                    // Se é inválido, significa que já foi consultado, portanto deverá ser verificado se o CPF é diferente do consultado.

                    // Novo CPF é igual ao consultado?
                    if (cpf.Equals(cpfConsultado) && nomeConsultado.Length > 0)
                    {
                        retNomeConsultado = nomeConsultado;
                        // CPF não mudou, portanto basta verificar se o nome é igual ao nome consultado.
                        return
                            new IRLib.Codigo.Brainiac.Gerenciador().IniciarNomes(nomeConsultado, nome);

                    }
                    else
                        // Efetua a consulta novamente, pois o CPF é diferente.
                        return ConsultaCPF(cpf, nome, ref retNomeConsultado);

                case enumStatusCPF.Confirmado:
                    return new IRLib.Codigo.Brainiac.Retorno() { TipoRetorno = IRLib.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.Ok };

                default:
                    throw new ClienteException("Não existe status associado ao cliente!");
            }// fim switch (status)
        }// Fim método.


        public List<IRLib.Assinaturas.Models.Cliente> BuscarClientesAssinatura(
            string login, string nome, string email, string cpfcpj, int pagina, int qtdPorPagina, ref int totalPaginas)
        {
            try
            {
                string filtro = this.FormatarFiltroAssinaturas(login, nome, email, cpfcpj);


                string sql =
                   string.Format(@"
					WITH tbGeral AS (
						SELECT DISTINCT 
							c.ID, c.LoginOSESP, c.Nome, c.CPF, c.Email
						FROM tCliente c (NOLOCK)
						WHERE {0}
						GROUP BY c.ID, c.LoginOSESP, c.Nome, c.CPF, c.Email),
	
					tbCount AS (
						SELECT COUNT(ID) AS Registros FROM tbGeral),
	
					tbOrdenada AS (
						SELECT ID, LoginOSESP, Nome, CPF, Email, ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber'
							FROM tbGeral)
		
					SELECT 
						ID, LoginOSESP, Nome, CPF, Email, RowNumber, Registros FROM tbOrdenada, tbCount 
					WHERE RowNumber >= {1} AND RowNumber < {2} 
					ORDER BY Nome
					", filtro, (pagina - 1) * qtdPorPagina, (pagina) * qtdPorPagina);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível encontrar nenhum registro correspondente ao filtro selecionado.");

                List<IRLib.Assinaturas.Models.Cliente> lista = new List<Assinaturas.Models.Cliente>();

                totalPaginas = bd.LerInt("Registros");
                do
                {
                    lista.Add(new Assinaturas.Models.Cliente()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Login = bd.LerString("LoginOSESP"),
                        Email = bd.LerString("Email"),
                        CPF = bd.LerString("CPF"),
                    });
                } while (bd.Consulta().Read());

                totalPaginas = Convert.ToInt32(totalPaginas / qtdPorPagina);

                if (totalPaginas < qtdPorPagina || totalPaginas % qtdPorPagina == 1)
                    totalPaginas++;

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private string FormatarFiltroAssinaturas(string login, string nome, string email, string cpfcpj)
        {
            login = login.ToSafeString();
            nome = nome.ToSafeString();
            email = email.ToSafeString();
            cpfcpj = cpfcpj.ToSafeString();

            StringBuilder stb = new StringBuilder();
            if (!string.IsNullOrEmpty(login))
                stb.AppendFormat("c.LoginOSESP = '{0}' ", login);

            if (!string.IsNullOrEmpty(nome))
            {
                if (stb.Length > 0)
                    stb.Append("AND ");

                stb.AppendFormat("c.Nome LIKE '%{0}%' ", nome);
            }
            if (!string.IsNullOrEmpty(email))
            {
                if (stb.Length > 0)
                    stb.Append("AND ");

                stb.AppendFormat("c.Email = '{0}' ", email);
            }
            if (!string.IsNullOrEmpty(cpfcpj))
            {
                if (stb.Length > 0)
                    stb.Append("AND ");

                if (cpfcpj.Length == 14)
                    stb.AppendFormat("(c.CNPJ = '{0}') ", cpfcpj);
                else
                    stb.AppendFormat("(c.CPF = '{0}') ", cpfcpj);
            }
            return stb.ToString();
        }

        public EstruturaCadastroCliente BuscaCadastro(int clienteID, string senha)
        {
            try
            {
                EstruturaCadastroCliente cliente = new EstruturaCadastroCliente();

                string sql = @"SELECT tCliente.Nome, CPF, Email, DataNascimento, Sexo, '+55' AS CodPaisTelefoneResidencial, DDDTelefone, Telefone ,
                '+55' AS CodPaisTelefoneComercial , DDDTelefoneComercial ,TelefoneComercial ,'+55' AS CodPaisTelefoneCelular,
                DDDCelular ,Celular ,Endereco ,Numero ,Complemento ,Bairro ,Cidade ,Pais ,CEP ,RecebeEmail ,tct.Nome AS MelhorFormaContato
                FROM tCliente
                LEFT JOIN tContatoTipo tct (NOLOCK) ON tct.ID = tCliente.ContatoTipoID
                WHERE tCliente.ID = " + clienteID;

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível encontrar nenhum registro correspondente ao filtro selecionado.");
                else
                {
                    cliente.ClienteID = clienteID;
                    cliente.Nome = bd.LerString("Nome");
                    cliente.CPF = bd.LerString("CPF");
                    cliente.Email = bd.LerString("Email");
                    cliente.Senha =  senha;
                    cliente.DataNascimento = bd.LerDateTime("DataNascimento").ToString("yyyyMMdd");
                    cliente.Sexo = Convert.ToChar(bd.LerString("Sexo"));
                    cliente.CodPaisTelefoneResidencial = bd.LerString("CodPaisTelefoneResidencial");
                    cliente.DDDTelefoneResidencial = bd.LerString("DDDTelefone");
                    cliente.TelefoneResidencial = bd.LerString("Telefone");
                    cliente.CodPaisTelefoneComercial = bd.LerString("CodPaisTelefoneComercial");
                    cliente.DDDTelefoneComercial = bd.LerString("DDDTelefoneComercial");
                    cliente.TelefoneComercial = bd.LerString("TelefoneComercial");
                    cliente.CodPaisTelefoneCelular = bd.LerString("CodPaisTelefoneCelular");
                    cliente.DDDTelefoneCelular = bd.LerString("DDDCelular");
                    cliente.TelefoneCelular = bd.LerString("Celular");
                    cliente.Logradouro = bd.LerString("Endereco");
                    cliente.Numero = bd.LerString("Numero");
                    cliente.Complemento = bd.LerString("Complemento");
                    cliente.Bairro = bd.LerString("Bairro");
                    cliente.Cidade = bd.LerString("Cidade");
                    cliente.UF = bd.LerString("Pais");
                    cliente.CEP = bd.LerString("CEP");
                    cliente.ReceberEmail = bd.LerBoolean("RecebeEmail");
                    cliente.MelhorFormaContato = bd.LerString("MelhorFormaContato");
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
