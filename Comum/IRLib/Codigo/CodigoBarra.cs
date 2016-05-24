/**************************************************
* Arquivo: CodigoBarra.cs
* Gerado: 11/04/2007
* Autor: Celeritas Ltda
***************************************************/

using Bokai.Barcodes;
using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace IRLib
{

    public class CodigoBarra : CodigoBarra_B
    {

        private int TENTATIVAS_CODIGO_UNICO = Convert.ToInt32(ConfigurationManager.AppSettings["TENTATIVAS_CODIGO_UNICO"]);
        // constantes utilizadas na criação de códigos de barra e estrutura de códigos por preço.
        private const int TAMANHO_RANDOMICO = 2;	//número de caracteres que o trecho randômico de um código deverá ter.
        private const int TAMANHO_CODIGO = 16;		//número de caracteres total do código de barras.
        private const int TAMANHO_CODIGO_ANTIGO = 18;		//número de caracteres total do código de barras.
        private const int RANDOMICO_MAX = 99; // Limite máximo para criação do trecho randômico.
        public const int EVENTO_MAX = 999;			// Limite máximo para criação do código do Evento.
        public const int APRESENTACAO_MAX = 99;	// Limite máximo para criação do código do Apresentação.
        public const int SETOR_MAX = 99;			// Limite máximo para criação do código do Setor.
        public const int PRECO_MAX = 99;			// Limite máximo para criação do código do Preço.

        private const int TAMANHO_CODIGO_SEQUENCIAL = 5;//número de caracteres total do código sequencial.
        private const int TAMANHO_CODIGO_SEQUENCIAL_ANTIGO = 6;//número de caracteres total do código sequencial.

        public const string CODIGO_SEQUENCIAL_MAX_VALOR = "00000"; //Utilizado para o ToString
        public const string CODIGO_SEQUENCIAL_MAX_VALOR_ANTIGO = "000000"; //Utilizado para o ToString

        public DataTable tableCodigoBarra = new DataTable("CodigoBarra");

        private void Initialize()
        {
            if (TENTATIVAS_CODIGO_UNICO.Equals(0))
                TENTATIVAS_CODIGO_UNICO = 30;
        }

        public CodigoBarra()
        {

            Initialize();
        }


        public CodigoBarra(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            Initialize();
        }


        #region Gerar código de barras para ingresso
        /// <summary>
        /// Método responsável por criar um novo código de barras estruturado e codificado e único para o evento em questão.
        /// </summary>
        /// <param name="precoID">ID do preço do ingresso.</param>
        /// <param name="eventoID">ID do evento do ingresso.</param>
        /// <returns>Objeto string contendo o código de 18 posições e codificado.</returns>
        public string NovoCodigoBarraIngresso(int precoID, int eventoID, int codigoImpressao)
        {
            // Passos:
            // 1) Valida os parâmetros enviados.
            // 2) Criação de código de barras estruturado.
            // 3) Codificação do código de barras gerado no passo 2.
            //	4) Check de consistência no banco de dados. O código não deverá se repetir para o mesmo evento.
            // 5) Check = ok, retorna o código gerado.
            // 6) Check = !ok, volta ao passo 1.

            try
            {
                // Valida os parâmetros.
                if (precoID == 0) throw new CodigoBarraException(string.Format("ID do preço inválido ({0}).", precoID));
                if (eventoID == 0) throw new CodigoBarraException(string.Format("ID do evento inválido ({0}).", eventoID));

                var tentativas = 0;


                string codigoBarra; // variável para armazenamento do código de barras.
                do
                {
                    if (tentativas > TENTATIVAS_CODIGO_UNICO)
                        throw new ApplicationException(string.Format("Não foi possível gerar novo código de barras! Tente novamente. {0} / {1} / {2}", eventoID, precoID, codigoImpressao));

                    // gera o código de barras limpo, codifica e armazena na variável.
                    codigoBarra = this.NovoCodigoBarraLimpo(precoID, codigoImpressao);
                    codigoBarra = this.CodificaCodigoBarra(codigoBarra);
                    tentativas++;

                }
                // Verifica se o código gerado já existe no banco de dados para o evento em questão.
                while (!CodigoBarraUnico(codigoBarra, eventoID));

                // Retorna o código de barras codificado e único para o eventoID em questão.
                return codigoBarra;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
        /// <summary>
        /// Método responsável por criar um novo código de barras sem estrutura....apenas randomico
        /// UTILIZA APENAS PARA O VALE INGRESSO. VERIFICA SE JÁ EXISTE VALE COM ESSE CODIGO
        /// </summary>
        /// <param name="precoID">ID do preço do ingresso.</param>
        /// <param name="eventoID">ID do evento do ingresso.</param>
        /// <returns>Objeto string contendo o código de 18 posições e codificado.</returns>
        public string NovoCodigoBarraRandomico(int tamanho)
        {
            try
            {
                Random rand = new Random();
                StringBuilder codigo;
                do
                {
                    codigo = new StringBuilder();
                    // Valida os parâmetros.
                    for (int i = 0; i < tamanho; i++)
                    {
                        codigo.Append(rand.Next(1, 10).ToString());
                    }


                } while (!CodigoBarraValeIngressoUnico(codigo.ToString()));
                if (codigo.Length == tamanho)
                    return codigo.ToString();
                else
                    throw new Exception("Erro ao gerar o código de barras. O tamanho é diferente do definido");

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Método responsável por validar a existência de um código de barras para um evento.
        /// </summary>
        /// <param name="codigoBarra">Código de barras a ser analisado</param>
        /// <param name="eventoID">ID do evento.</param>
        /// <returns>Objeto bool onde true = código único</returns>
        private bool CodigoBarraUnico(string codigoBarra, int eventoID)
        {
            try
            {
                // Verifica se foi encontrado algum ingresso com o código de barras que seja para o evento em questão.
                return !bd.Consulta(
                    "SELECT 1 FROM tIngressoCodigoBarra(NOLOCK) WHERE EventoID = " + eventoID + " AND CodigoBarra = '" + codigoBarra + "'"
                    ).Read();
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Método responsável por validar a existência de um código de barras para um evento.
        /// </summary>
        /// <param name="codigoBarra">Código de barras a ser analisado</param>
        /// <param name="eventoID">ID do evento.</param>
        /// <returns>Objeto bool onde true = código único</returns>
        private bool CodigoBarraValeIngressoUnico(string codigoBarra)
        {
            try
            {
                // Verifica se foi encontrado algum ingresso com o código de barras que seja para o evento em questão.
                return !bd.Consulta("SELECT 1 FROM tValeIngresso(NOLOCK) WHERE CodigoBarra = '" + codigoBarra + "'").Read();
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Gera um novo código de barras estruturado e sem codificação.
        /// </summary>
        /// <param name="precoID">ID do preço do ingresso.</param>
        /// <returns>Objeto string contendo o código de barras gerado.</returns>
        private string NovoCodigoBarraLimpo(int precoID, int codigoImpressao)
        {
            // Passos:
            // 1) Montagem do código de barras estruturado - oriundo do banco de dados - CodigoBarra.EstruturaCodigoBarra()
            // 2) Criação de códigos randômicos - quantidade de caracteres definido em TAMANHO_RANDOMICO
            // 3) Concatena os códigos gerados e retorna.

            // Gera o código estruturado com base no precoID
            string codigoEstruturado = this.EstruturaCodigoBarra(precoID);// variável para armazenamento do código estrutudado (oriundo do database).
            bool codigoAntigo = codigoEstruturado.Length == TAMANHO_CODIGO_ANTIGO - TAMANHO_RANDOMICO - TAMANHO_CODIGO_SEQUENCIAL_ANTIGO; //Precisa verificar se o tipo do código é antigo (18) ou novo (16) para gerar o sequencial
            string trechoRandomico; // variável para armazenamento do trecho de código randômico.
            string codigoSequencialNovo = codigoAntigo ? codigoImpressao.ToString(CODIGO_SEQUENCIAL_MAX_VALOR_ANTIGO) : codigoImpressao.ToString(CODIGO_SEQUENCIAL_MAX_VALOR);//Se for antigo, gera com 6 posições, do contrario com 5

            // Cria o trecho randômico - Tamanho definido em TAMANHO_RANDOMICO.
            Random rnd = new Random((int)System.DateTime.Now.Ticks);
            // Caso o número seja menor do que o definido, serão inseridos zeros à esquerda.            
            trechoRandomico = rnd.Next(1, RANDOMICO_MAX).ToString(new string('0', TAMANHO_RANDOMICO));

            string codigoLimpo = codigoSequencialNovo + trechoRandomico + codigoEstruturado; // Concatena as duas partes para codificação.

            // Double-check no tamanho do código de barras.
            if (codigoLimpo.Length != TAMANHO_CODIGO && codigoLimpo.Length != TAMANHO_CODIGO_ANTIGO)
                throw new CodigoBarraException("O Código de barras deve ter " + TAMANHO_CODIGO + " ou " + TAMANHO_CODIGO_ANTIGO + " caracteres - Erro (4)");

            return codigoLimpo;

        }

        /// <summary>
        /// Método responsável por codificar um código de barras.
        /// </summary>
        /// <param name="codigoLimpo">Código de barras sem criptografia</param>
        /// <returns>Objeto string contendo o código.</returns>
        private string CodificaCodigoBarra(string codigoLimpo)
        {
            // 1) Codificação do código de barras:
            //		1.1) Primeiro caractere sempre se repete.
            //		1.2) Aplica-se a fórmula oListCaesarValue.Codificar() entre o caractere atual sem codificação e o anterior com codificação.
            //			1.2.1) No caso do segundo caractere, será aplicada a fórmula entre o primeiro e o segundo - ambos sem codificação.
            //	2) Feito o check de consistência do código de barras (é possível codificar e decodificar sem perda de dados?)
            //	3) Retorno do código de barras gerado.

            StringBuilder codigoCriptografado = new StringBuilder(TAMANHO_CODIGO); // variável para armazenamento do código final.
            ListCaesarValue oListCaesarValue = new ListCaesarValue(); // objeto de pesquisa "de-para" = resultado.

            string resultado = codigoLimpo[0].ToString(); // variável para armazenamento do resultado da busca (de-para) - inicia-se com o primeiro caractere
            codigoCriptografado.Append(resultado); // adiciona o caractere à string codificada.

            for (int i = 1; i < codigoLimpo.Length; i++) // For de análise do código de barras (1 a 17). Caractere de posição 0 foi tratado acima.
            {
                //Efetua a codificação e armazena o caractere de retorno em Resultado
                resultado = oListCaesarValue.Codificar(resultado, codigoLimpo[i].ToString());
                if (resultado == null) //verificar se algo foi encontrado.
                    throw new CodigoBarraException(
                        string.Format("Não existe combinação entre os códigos {0} e {1} - Erro (4)", resultado, codigoLimpo[i]));

                // adiciona o caracteres codificado à string codificada.
                codigoCriptografado.Append(resultado);
            } // fim for de código limpo

            // Faz o check de decodificação.
            if (this.Check(codigoCriptografado.ToString(), codigoLimpo))
                return codigoCriptografado.ToString();
            else
                throw new CodigoBarraException(
                    string.Format("Decodificação impossível para o código {0} - Erro (5)", codigoCriptografado.ToString()));
        }

        /// <summary>
        /// Método responsável por retornar a estrutura de um código de barras conforme o preço selecionado.
        /// </summary>
        /// <param name="precoID">ID do Preço do ingresso</param>
        /// <returns>Estrutura de 10 caracteres do código de barras.</returns>
        private string EstruturaCodigoBarra(int precoID)
        {
            try
            {
                // Verifica na tabela tCodigoBarra se existe algum registro com precoID igual ao parâmetro enviado.
                bd.Consulta("SELECT EventoCodigo, ApresentacaoCodigo, SetorCodigo, PrecoCodigo FROM tCodigoBarra(NOLOCK) WHERE PrecoID = " + precoID);
                if (!bd.Consulta().Read()) // Se não existir, lança uma exception;
                    throw new CodigoBarraException(string.Format("Não existe código de barras estrutudado para o preço em questão - ({0})", precoID));
                else
                {
                    // Concatena os códigos encontrados.
                    string codigoEstruturado = bd.LerString("EventoCodigo") + bd.LerString("ApresentacaoCodigo") + bd.LerString("SetorCodigo") + bd.LerString("PrecoCodigo");
                    // Consiste o tamanho correto do código estruturado.
                    if (codigoEstruturado.Length == TAMANHO_CODIGO - TAMANHO_RANDOMICO - TAMANHO_CODIGO_SEQUENCIAL || codigoEstruturado.Length == TAMANHO_CODIGO_ANTIGO - TAMANHO_RANDOMICO - TAMANHO_CODIGO_SEQUENCIAL_ANTIGO)
                        return codigoEstruturado;
                    else
                        throw new CodigoBarraException(string.Format("O código de barras {0} - {1} não é válido", codigoEstruturado, precoID));
                }
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

        /// <summary>
        /// Método responsável por validar a consistência da codificação de um determinado código de barras.
        /// </summary>
        /// <param name="codificado">String com código de barras codificado</param>
        /// <param name="codigoLimpo">String com código de barras Limpo</param>
        /// <returns></returns>
        public bool Check(string codificado, string codigoLimpo)
        {
            // Detalhes da rotina:
            // Aplicar a decodificação de um determinado código de barras a fim de verificar se é possível efetuar o processo de volta.
            // A rotina de decodificação consiste em aplicar uma fórmula definida em ListCaesarValue.Decodificar onde são enviados dois parâmetros do tipo string. Apenas um valor é retornado (decodificado).
            // Os dois parâmetros são sempre o atual e o anterior, sendo que a ordem deverá ser do último para o primeiro:
            //		Y~Z = 0
            //		X~Y = 1
            //		V~X = 2
            //		Código descriptografado: 210
            //	Além da fórmula, o primeiro caractere (esquerda-direita) deverá ser simplesmente copiado:
            //		Codificado=2103
            //		Fórmula:
            //			0~3 = 1
            //			1~0 = 7
            //			2~1 = 9
            //			Código decodificado: 2971
            //	Conforme os caracteres são decodificados, deverão ser inseridos no ínicio da string (os primeiros a serem tratados serão os últimos na string).

            string valorDecodificado = string.Empty; // variável para armazenamento temporário do valor calculado.
            ListCaesarValue oListCaesarValue = new ListCaesarValue(); // objeto de pesquisa "de-para" = resultado (decodificação)
            StringBuilder decodificado = new StringBuilder(18); // objeto para armazenamento do código decodificado.

            // for para decodificação de caracteres. Passará pelas posições [Length-2 até 0]. 
            // O for se inicia na penúltima posição do código, pois a decodificação é aplicada entre o penúltimo e o posterior (penúltimo x último) = valor.
            for (int i = codificado.Length - 2; i >= 0; i--)
            {
                // Aplica-se a decodificação entre o caractere da posição atual e o "anterior" (i+1), ou seja, Y~Z=valorDecodificado.
                valorDecodificado = oListCaesarValue.Decodificar(codificado[i].ToString(), codificado[i + 1].ToString());
                if (valorDecodificado == null) //verificar se algo foi encontrado.
                    throw new CodigoBarraException(
                        string.Format("Não existe combinação entre os códigos {0} e {1} - Erro (4)", codificado[i], valorDecodificado));

                decodificado.Insert(0, valorDecodificado); // Insere o valor decodificado na primeira posição, afinal estamos indo do último ao primeiro.
            }

            decodificado.Insert(0, codificado[0]); // O primeiro caractere se repete no código com e sem criptografia.

            // Verifica se o código decodificado é igual ao código limpo. Se sim, codificação do código de barras é válido.
            return decodificado.ToString().Equals(codigoLimpo);
        }
        /// <summary>
        /// Método que retorna a estrutura para a validação do codigo de barra de acordo com EventoID,ApresentacaoID e SetorID.
        /// Utilizado no controle de acesso.
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        public EstruturaCodigoBarraValido RetornaEstruturaCodigoBarraValido(int eventoID, int apresentacaoID, int setorID)
        {
            try
            {
                EstruturaCodigoBarraValido retorno = new EstruturaCodigoBarraValido();
                retorno.SetorPrecoCodigo = new List<EstruturaSetorPrecoCodigoBarra>();
                EstruturaSetorPrecoCodigoBarra setorEPreco;

                string filtroSetor = " AND SetorID = " + setorID;
                if (setorID == 0)
                    filtroSetor = "";

                string sql = @"SELECT DISTINCT EventoCodigo,ApresentacaoCodigo,SetorCodigo,PrecoCodigo, tPreco.Nome AS PrecoNome, tSetor.Nome AS SetorNome, tSetor.ID AS SetorID
                                FROM tCodigoBarra (NOLOCK)
                                INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tCodigoBarra.SetorID
                                INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tCodigoBarra.PrecoID
                                WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID + filtroSetor;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    if (retorno.EventoCodigo == null || retorno.ApresentacaoCodigo == null)
                    {
                        retorno.EventoCodigo = bd.LerString("EventoCodigo");
                        retorno.ApresentacaoCodigo = bd.LerString("ApresentacaoCodigo");
                    }
                    setorEPreco = new EstruturaSetorPrecoCodigoBarra();
                    setorEPreco.PrecoCodigo = bd.LerString("PrecoCodigo");
                    setorEPreco.SetorCodigo = bd.LerString("SetorCodigo");
                    setorEPreco.PrecoNome = bd.LerString("PrecoNome");
                    setorEPreco.SetorNome = bd.LerString("SetorNome");
                    setorEPreco.SetorID = bd.LerInt("SetorID");
                    retorno.SetorPrecoCodigo.Add(setorEPreco);
                }
                return retorno;
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
        /// <summary>
        /// Decodifica o codigo de barras criptografado. Método criado para validação de códigos
        /// </summary>
        /// <param name="codificado"></param>
        /// <returns></returns>
        public string RetornaCodigoLimpo(string codificado)
        {
            // Detalhes da rotina:
            // Aplicar a decodificação de um determinado código de barras a fim de verificar se é possível efetuar o processo de volta.
            // A rotina de decodificação consiste em aplicar uma fórmula definida em ListCaesarValue.Decodificar onde são enviados dois parâmetros do tipo string. Apenas um valor é retornado (decodificado).
            // Os dois parâmetros são sempre o atual e o anterior, sendo que a ordem deverá ser do último para o primeiro:
            //		Y~Z = 0
            //		X~Y = 1
            //		V~X = 2
            //		Código descriptografado: 210
            //	Além da fórmula, o primeiro caractere (esquerda-direita) deverá ser simplesmente copiado:
            //		Codificado=2103
            //		Fórmula:
            //			0~3 = 1
            //			1~0 = 7
            //			2~1 = 9
            //			Código decodificado: 2971
            //	Conforme os caracteres são decodificados, deverão ser inseridos no ínicio da string (os primeiros a serem tratados serão os últimos na string).

            string valorDecodificado = string.Empty; // variável para armazenamento temporário do valor calculado.
            ListCaesarValue oListCaesarValue = new ListCaesarValue(); // objeto de pesquisa "de-para" = resultado (decodificação)
            StringBuilder decodificado = new StringBuilder(18); // objeto para armazenamento do código decodificado.

            // for para decodificação de caracteres. Passará pelas posições [Length-2 até 0]. 
            // O for se inicia na penúltima posição do código, pois a decodificação é aplicada entre o penúltimo e o posterior (penúltimo x último) = valor.
            for (int i = codificado.Length - 2; i >= 0; i--)
            {
                // Aplica-se a decodificação entre o caractere da posição atual e o "anterior" (i+1), ou seja, Y~Z=valorDecodificado.
                valorDecodificado = oListCaesarValue.Decodificar(codificado[i].ToString(), codificado[i + 1].ToString());
                if (valorDecodificado == null) //verificar se algo foi encontrado.
                    throw new CodigoBarraException(
                        string.Format("Não existe combinação entre os códigos {0} e {1} - Erro (4)", codificado[i], valorDecodificado));

                decodificado.Insert(0, valorDecodificado); // Insere o valor decodificado na primeira posição, afinal estamos indo do último ao primeiro.
            }

            decodificado.Insert(0, codificado[0]); // O primeiro caractere se repete no código com e sem criptografia.

            // Verifica se o código decodificado é igual ao código limpo. Se sim, codificação do código de barras é válido.
            return decodificado.ToString();
        }
        #endregion

        [Obsolete("Função criada pelo Renato. Foi descontinuada. Por favor, não use.", false)]
        public DataTable PreencherCodigosBarraPreImpresso(DataTable tImpressao)
        {
            BD bd = new BD();
            try
            {
                foreach (DataRow linha in tImpressao.Rows)
                {
                    int ingressoID = (int)linha["IngressoID"];
                    if (ingressoID == 0)
                        ingressoID = (int)linha["ID"];

                    object retorno = bd.ConsultaValor("SELECT CodigoBarra FROM tIngresso(NOLOCK) WHERE ID = " + ingressoID);
                    if (retorno is string)
                        linha["CodigoBarra"] = (string)retorno;
                }
                return tImpressao.Copy();
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

        /// <summary>
        ///  Método de busca no banco dos códigos de barra do evento,apresentação e setor que pertencem a White List
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        public List<EstruturaWhiteList> WhiteList(int eventoID, int apresentacaoID, int setorID)
        {
            string filtroSetor = "AND SetorID= " + setorID;
            if (setorID == 0)//todos os setores
                filtroSetor = "";
            try
            {
                //Objetos de retorno
                List<EstruturaWhiteList> whiteList = new List<EstruturaWhiteList>();
                EstruturaWhiteList whiteListItem;

                string sql = @"SELECT CodigoBarra, CodigoBarraCliente FROM tIngresso 
                              WHERE tIngresso.EventoID= " + eventoID + " AND tIngresso.ApresentacaoID= " + apresentacaoID + " " + filtroSetor +
                              " AND (CodigoBarra <> '' OR CodigoBarraCliente <> '')";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    whiteListItem = new EstruturaWhiteList();
                    whiteListItem.CodigoBarra = bd.LerString("CodigoBarra");
                    whiteListItem.CodigoBarraCliente = bd.LerString("CodigoBarraCliente");
                    whiteList.Add(whiteListItem);
                }
                return whiteList;

            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Devolve a o codigo da apresentação de acordo com a apresentaçãoID
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public string RetornaApresentacaoCodigo(int apresentacaoID)
        {
            try
            {
                string sql = "SELECT DISTINCT ApresentacaoCodigo FROM tCodigoBarra WHERE ApresentacaoID = " + apresentacaoID;
                object aux = bd.ConsultaValor(sql).ToString();
                return Convert.ToString(aux);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Método de busca no banco dos códigos de barra do evento,apresentação e setor que pertencem a Black List
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns>Lista de objetos EstruturaBlackList</returns>
        public List<IRLib.ClientObjects.EstruturaBlackList> BlackList(int eventoID, int apresentacaoID, int setorID)
        {
            //variável para controlar a mudança de ingressos dentro da lista
            int ingressoIDAnterior = 0;
            DateTime dataHoraAtual = DateTime.Now;
            //controle para saber quais códigos entram na black list
            bool codigoBarraCancelado = false;
            string acaoCodigoBarraCancelado = "";

            //Objetos de retorno
            List<EstruturaBlackList> blackList = new List<EstruturaBlackList>();
            EstruturaBlackList blackListItem;

            //esses objeto servem para auxiliar ao ler e filtrar os códigos de barra que devem ir para a black list.
            List<EstruturaIngressoLog_BlackList> leituraBanco = new List<EstruturaIngressoLog_BlackList>();
            EstruturaIngressoLog_BlackList leituraBancoAux;

            string filtroSetor = "AND SetorID= " + setorID;
            if (setorID == 0)//todos os setores
                filtroSetor = "";

            try
            {
                //Essa query busca todos os registros da IngressoLog desse evento, apresentação e setor
                //de forma ordenada por IngressoID e
                //para que os registros de atualização ou cancelamento de código de barras apareçam primeiro
                string sql = @"SELECT tIngressoLog.IngressoID, tIngressoLog.TimeStamp, tIngressoLog.Acao,tIngressoLog.CodigoBarra,tIngressoLog.Obs  FROM tIngresso
                                INNER JOIN tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID
                                WHERE tIngresso.EventoID= " + eventoID + " AND tIngresso.ApresentacaoID= " + apresentacaoID + " " + filtroSetor +
                                " ORDER BY IngressoID,TimeStamp DESC";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    leituraBancoAux = new EstruturaIngressoLog_BlackList();
                    leituraBancoAux.IngressoID = bd.LerInt("IngressoID");
                    leituraBancoAux.TimeStamp = Convert.ToDateTime(bd.LerStringFormatoDataHora("TimeStamp"));
                    leituraBancoAux.Acao = bd.LerString("Acao");
                    leituraBancoAux.CodigoBarra = bd.LerString("CodigoBarra");
                    leituraBancoAux.Obs = bd.LerString("Obs");
                    leituraBanco.Add(leituraBancoAux);
                }
                //Verifica quais códigos de barra devem entrar na black list e quais não devem.
                foreach (EstruturaIngressoLog_BlackList item in leituraBanco)
                {
                    if (ingressoIDAnterior != item.IngressoID)
                        codigoBarraCancelado = false;

                    if ((item.Acao == IngressoLog.CANCELAR || item.Acao == IngressoLog.CANCELAR_PREIMPRESSO ||
                        item.Acao == IngressoLog.ANULAR_IMPRESSAO || item.Acao == IngressoLog.REIMPRIMIR) && !codigoBarraCancelado)
                    {
                        //quando o ingresso estiver com uma das ações acima todos os códigos de barra das ações após a ação
                        //atual devem ser incluidos na black list.
                        codigoBarraCancelado = true;
                        acaoCodigoBarraCancelado = item.Acao;
                    }
                    else
                    {
                        if (codigoBarraCancelado)//esse ingresso deve ter os códigos inseridos na Black List
                        {
                            //No caso de pré-Impresso, deve-se pegar o registro de impressão não o de Emisão
                            //apesar do registro de emissão conter o Código de Barra
                            if (item.CodigoBarra != "" && item.Acao != IngressoLog.EMISSAO_PREIMPRESSO)
                            {
                                blackListItem = new EstruturaBlackList();
                                blackListItem.ApresentacaoID = apresentacaoID;
                                blackListItem.CodigoBarra = item.CodigoBarra;
                                blackListItem.DataHoraInclusao = item.TimeStamp;
                                blackListItem.DataHoraSincronizacao = dataHoraAtual;

                                LeituraCodigo.CodigoResposta motivo = new LeituraCodigo.CodigoResposta();
                                //preenche o motivo do ingresso estar na black list
                                switch (acaoCodigoBarraCancelado)
                                {
                                    case IngressoLog.CANCELAR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoCancelado;
                                            break;
                                        }
                                    case IngressoLog.CANCELAR_PREIMPRESSO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.PreImpressoCancelado;
                                            break;
                                        }
                                    case IngressoLog.ANULAR_IMPRESSAO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.ImpressaoCancelada;
                                            break;
                                        }
                                    case IngressoLog.REIMPRIMIR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoReimpresso;
                                            break;
                                        }

                                }

                                blackListItem.Motivo = motivo;
                                blackListItem.EventoID = eventoID;
                                blackListItem.MotivoCancelamentoReimprecao = item.Obs;
                                blackListItem.SetorID = setorID;
                                blackList.Add(blackListItem);
                            }
                        }

                    }
                    ingressoIDAnterior = item.IngressoID;
                }
                return blackList;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        ///Carrega os registros da tLeituraCodigo e da tIngressoLog para formar a black list
        ///O retorno é a estrutura nescessária para a atualização da black list.
        /// </summary>
        /// <param name="listaInserir"></param>
        /// <returns>Sucesso de todos os inserts</returns>
        public List<EstruturaBlackList> CarregarBlackList(int eventoID, int apresentacaoID, int setorID)
        {
            try
            {
                List<EstruturaBlackList> retorno = BlackList(eventoID, apresentacaoID, setorID);
                EstruturaBlackList retornoItem;
                DateTime agora = DateTime.Now;


                string filtroSetor = " AND SetorID= " + setorID;
                if (setorID == 0)//todos os setores
                    filtroSetor = " ";
                //Primeiro traz os itens inseridos na tLeituraCodigo depois da ultima atualização
                bd.Comando = "";
                bd.Consulta(@"SELECT DataLeitura, CodigoBarra, Portaria, CodigoResultado, Coletor
                            FROM tLeituraCodigo(NOLOCK) WHERE EventoID =" + eventoID + " AND ApresentacaoID =" + apresentacaoID + filtroSetor);
                while (bd.Consulta().Read())
                {
                    retornoItem = new EstruturaBlackList();
                    retornoItem.ApresentacaoID = apresentacaoID;
                    retornoItem.CodigoBarra = bd.LerString("CodigoBarra");
                    retornoItem.ColetorNumero = bd.LerInt("Coletor");
                    retornoItem.DataHoraInclusao = bd.LerDateTime("DataLeitura");
                    retornoItem.DataHoraSincronizacao = agora;
                    retornoItem.EventoID = eventoID;
                    retornoItem.Motivo = (LeituraCodigo.CodigoResposta)bd.LerInt("CodigoResultado");
                    retornoItem.Portaria = bd.LerString("Portaria");
                    retornoItem.SetorID = setorID;
                    retorno.Add(retornoItem);

                }

                return retorno;
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

        /// <summary>
        /// Método de busca no banco dos códigos de barra do evento,apresentação e setor que pertencem a Black List
        /// Sobrecarga para trazer somente os registros inseridos após a ultima atualização do buffer local de black list
        /// </summary>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <returns>Lista de objetos EstruturaBlackList</returns>
        public List<IRLib.ClientObjects.EstruturaBlackList> BlackList(int eventoID, int apresentacaoID, int setorID, DateTime ultimaSincronizacao)
        {
            //variável para controlar a mudança de ingressos dentro da lista
            int ingressoIDAnterior = 0;
            DateTime dataHoraAtual = DateTime.Now;
            //controle para saber quais códigos entram na black list
            bool codigoBarraCancelado = false;
            string acaoCodigoBarraCancelado = "";

            //Objetos de retorno
            List<EstruturaBlackList> blackList = new List<EstruturaBlackList>();
            EstruturaBlackList blackListItem;

            //esses objeto servem para auxiliar ao ler e filtrar os códigos de barra que devem ir para a black list.
            List<EstruturaIngressoLog_BlackList> leituraBanco = new List<EstruturaIngressoLog_BlackList>();
            EstruturaIngressoLog_BlackList leituraBancoAux;

            string filtroSetor = "AND SetorID= " + setorID;
            if (setorID == 0)//todos os setores
                filtroSetor = "";

            try
            {
                //Essa query busca todos os registros da IngressoLog desse evento, apresentação e setor
                //de forma ordenada por IngressoID e
                //para que os registros de atualização ou cancelamento de código de barras apareçam primeiro
                string sql = @"SELECT tIngressoLog.IngressoID, tIngressoLog.TimeStamp, tIngressoLog.Acao,tIngressoLog.CodigoBarra,tIngressoLog.Obs  FROM tIngresso
                                INNER JOIN tIngressoLog ON tIngresso.ID = tIngressoLog.IngressoID
                                WHERE tIngresso.EventoID= " + eventoID + " AND tIngresso.ApresentacaoID= " + apresentacaoID + " " + filtroSetor + "AND TimeStamp > '" + ultimaSincronizacao.ToString("yyyyMMddHHmmss") + "'" +
                                " ORDER BY IngressoID,TimeStamp DESC";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    leituraBancoAux = new EstruturaIngressoLog_BlackList();
                    leituraBancoAux.IngressoID = bd.LerInt("IngressoID");
                    leituraBancoAux.TimeStamp = Convert.ToDateTime(bd.LerStringFormatoDataHora("TimeStamp"));
                    leituraBancoAux.Acao = bd.LerString("Acao");
                    leituraBancoAux.CodigoBarra = bd.LerString("CodigoBarra");
                    leituraBancoAux.Obs = bd.LerString("Obs");
                    leituraBanco.Add(leituraBancoAux);
                }
                //Verifica quais códigos de barra devem entrar na black list e quais não devem.
                foreach (EstruturaIngressoLog_BlackList item in leituraBanco)
                {
                    if (ingressoIDAnterior != item.IngressoID)
                        codigoBarraCancelado = false;

                    if ((item.Acao == IngressoLog.CANCELAR || item.Acao == IngressoLog.CANCELAR_PREIMPRESSO ||
                        item.Acao == IngressoLog.ANULAR_IMPRESSAO || item.Acao == IngressoLog.REIMPRIMIR) && !codigoBarraCancelado)
                    {
                        //quando o ingresso estiver com uma das ações acima todos os códigos de barra das ações após a ação
                        //atual devem ser incluidos na black list.
                        codigoBarraCancelado = true;
                        acaoCodigoBarraCancelado = item.Acao;
                    }
                    else
                    {
                        if (codigoBarraCancelado)//esse ingresso deve ter os códigos inseridos na Black List
                        {
                            //No caso de pré-Impresso, deve-se pegar o registro de impressão não o de Emisão
                            //apesar do registro de emissão conter o Código de Barra
                            if (item.CodigoBarra != "" && item.Acao != IngressoLog.EMISSAO_PREIMPRESSO)
                            {
                                blackListItem = new EstruturaBlackList();
                                blackListItem.ApresentacaoID = apresentacaoID;
                                blackListItem.CodigoBarra = item.CodigoBarra;
                                blackListItem.DataHoraInclusao = item.TimeStamp;
                                blackListItem.DataHoraSincronizacao = dataHoraAtual;

                                LeituraCodigo.CodigoResposta motivo = new LeituraCodigo.CodigoResposta();
                                //preenche o motivo do ingresso estar na black list
                                switch (acaoCodigoBarraCancelado)
                                {
                                    case IngressoLog.CANCELAR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoCancelado;
                                            break;
                                        }
                                    case IngressoLog.CANCELAR_PREIMPRESSO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.PreImpressoCancelado;
                                            break;
                                        }
                                    case IngressoLog.ANULAR_IMPRESSAO:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.ImpressaoCancelada;
                                            break;
                                        }
                                    case IngressoLog.REIMPRIMIR:
                                        {
                                            motivo = LeituraCodigo.CodigoResposta.IngressoReimpresso;
                                            break;
                                        }

                                }

                                blackListItem.Motivo = motivo;
                                blackListItem.EventoID = eventoID;
                                blackListItem.MotivoCancelamentoReimprecao = item.Obs;
                                blackListItem.SetorID = setorID;
                                blackList.Add(blackListItem);
                            }
                        }

                    }
                    ingressoIDAnterior = item.IngressoID;
                }
                return blackList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Códigos não validados pelo Evandro
        public void AtivarCodigos(int eventoID, int LocalID)
        {
            try
            {
                BD bdApresentacao = new BD();
                BD bdSetor = new BD();
                BD bdPreco = new BD();

                string sql = "SELECT ID FROM tApresentacao WHERE EventoID = " + eventoID;
                bdApresentacao.Consulta(sql);

                bd.IniciarTransacao();
                while (bdApresentacao.Consulta().Read())
                { // Apresentações.
                    sql = "SELECT ID, SetorID FROM tApresentacaoSetor WHERE ApresentacaoID = " + bdApresentacao.LerInt("ID");
                    bdSetor.Consulta(sql);

                    while (bdSetor.Consulta().Read())
                    {// Setores
                        sql = "SELECT ID FROM tPreco WHERE ApresentacaoSetorID = " + bdSetor.LerInt("ID");
                        bdPreco.Consulta(sql);

                        while (bdPreco.Consulta().Read()) // Preços
                            this.Inserir(eventoID, bdApresentacao.LerInt("ID"), bdSetor.LerInt("SetorID"), bdPreco.LerInt("ID"), bd);
                    }
                }

                bd.FinalizarTransacao();
                bd.Fechar();

            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void DesativarCodigos(int eventoID)
        {
            try
            {
                string sql = "UPDATE tCodigoBarra SET Ativo = 'F' WHERE EventoID = " + eventoID;
                bd.IniciarTransacao();
                bd.Executar(sql);
                bd.FinalizarTransacao();
                bd.Fechar();

            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        protected void InserirControle(string acao, BD database)
        {

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append("INSERT INTO cCodigoBarra (ID, Versao, Acao, TimeStamp, UsuarioID) ");
            sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
            sql.Replace("@ID", this.Control.ID.ToString());

            if (!acao.Equals("I"))
                this.Control.Versao++;

            sql.Replace("@V", this.Control.Versao.ToString());
            sql.Replace("@A", acao);
            sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            sql.Replace("@U", this.Control.UsuarioID.ToString());

            database.Executar(sql.ToString());

        }

        private bool Inserir(BD database)
        {
            try
            {
                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCodigoBarra(EventoID, EventoCodigo, ApresentacaoID, ApresentacaoCodigo, SetorID, SetorCodigo, PrecoID, PrecoCodigo, Ativo) ");
                sql.Append("VALUES (@001,'@002',@003,'@004',@005,'@006',@007,'@008','@009');SELECT SCOPE_IDENTITY();");

                if (SetorCodigo.Valor.Length != 2)
                    throw new Exception("Falha ao gerar código de barra!");

                sql.Replace("@001", this.EventoID.ValorBD);
                sql.Replace("@002", this.EventoCodigo.ValorBD);
                sql.Replace("@003", this.ApresentacaoID.ValorBD);
                sql.Replace("@004", this.ApresentacaoCodigo.ValorBD);
                sql.Replace("@005", this.SetorID.ValorBD);
                sql.Replace("@006", this.SetorCodigo.ValorBD);
                sql.Replace("@007", this.PrecoID.ValorBD);
                sql.Replace("@008", this.PrecoCodigo.ValorBD);
                sql.Replace("@009", this.Ativo.ValorBD);

                object x = database.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = (this.Control.ID > 0);

                if (result)
                    InserirControle("I", database);


                return result;

            }
            catch
            {
                throw;
            }
        }

        private bool Inserir(int eventoID, int apresentacaoID, int setorID, int precoID, string eventoCodigo, string apresentacaoCodigo, string setorCodigo, string precoCodigo, BD database)
        {
            this.EventoID.Valor = eventoID;
            this.ApresentacaoID.Valor = apresentacaoID;
            this.SetorID.Valor = setorID;
            this.PrecoID.Valor = precoID;

            this.EventoCodigo.Valor = eventoCodigo;
            this.ApresentacaoCodigo.Valor = apresentacaoCodigo;
            this.SetorCodigo.Valor = setorCodigo;
            this.PrecoCodigo.Valor = precoCodigo;

            this.Ativo.Valor = true;
            return Inserir(database);
        }

        public bool Inserir(int eventoID, int apresentacaoID, int setorID, int precoID)
        {
            return this.Inserir(eventoID, apresentacaoID, setorID, precoID, new BD());
        }

        public bool Inserir(int eventoID, int apresentacaoID, int setorID, int precoID, BD db)
        {
            bool gerarcodigoAntigo = new ConfigGerenciador().GerarCodigoAntigo();

            string SQLEvento = "SELECT TOP 1 EventoCodigo FROM tCodigoBarra (NOLOCK) WHERE EventoID=" + eventoID;
            string SQLApresentacao = "SELECT TOP 1 ApresentacaoCodigo FROM tCodigoBarra (NOLOCK) WHERE ApresentacaoID=" + apresentacaoID + " AND EventoID = " + eventoID;
            string SQLSetor = "SELECT TOP 1 SetorCodigo FROM tCodigoBarra (NOLOCK) WHERE SetorID=" + setorID + " AND ApresentacaoID = " + apresentacaoID;
            string SQLPreco = "SELECT TOP 1 PrecoCodigo FROM tCodigoBarra (NOLOCK) WHERE PrecoID=" + precoID + " AND SetorID = " + setorID;

            string SQLApresentacaoTamanho = "SELECT TOP 1 ApresentacaoCodigo FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID;

            object aux = db.ConsultaValor(SQLPreco);
            int PrecoCodigoAntigo = aux == null ? -1 : int.Parse((string)aux);

            aux = db.ConsultaValor(SQLSetor);
            int SetorCodigoAntigo = aux == null ? -1 : int.Parse((string)aux);

            aux = db.ConsultaValor(SQLApresentacao);
            int ApresentacaoCodigoAntigo = aux == null ? -1 : int.Parse((string)aux);

            aux = db.ConsultaValor(SQLApresentacaoTamanho);

            string ApresentacaoCodigoTamanho = "00";

            if (aux != null)
            {
                if (((string)aux).Length < 3)
                    ApresentacaoCodigoTamanho = "00";
                else
                    ApresentacaoCodigoTamanho = "000";
            }
            else
            {
                if (gerarcodigoAntigo)
                    ApresentacaoCodigoTamanho = "000";
                else
                    ApresentacaoCodigoTamanho = "00";
            }


            aux = db.ConsultaValor(SQLEvento);
            int EventoCodigoAntigo = aux == null ? -1 : int.Parse((string)aux);

            if (PrecoCodigoAntigo >= 0)
            {
                return true;
            }
            else
            {
                if (SetorCodigoAntigo >= 0)
                {
                    IDataReader drNovoPreco = db.Consulta("SELECT MAX(PrecoCodigo) NovoPreco FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID);

                    int NovoPreco = 0;

                    if (drNovoPreco.Read())
                    {
                        if (drNovoPreco["NovoPreco"] != null && drNovoPreco["NovoPreco"] != DBNull.Value)
                        {
                            NovoPreco = int.Parse(((string)drNovoPreco["NovoPreco"])) + 1;
                        }
                    }

                    if (NovoPreco > PRECO_MAX)
                        throw new Exception("O número cadastrado de preços é superior ao limite estabelecido!");

                    drNovoPreco.Close();

                    CodigoBarraEvento codigoBarraEvento = new CodigoBarraEvento();
                    codigoBarraEvento.Inserir(bd);


                    return this.Inserir(eventoID, apresentacaoID, setorID, precoID, EventoCodigoAntigo.ToString("000"), ApresentacaoCodigoAntigo.ToString(ApresentacaoCodigoTamanho), SetorCodigoAntigo.ToString("00"), NovoPreco.ToString("00"), db);
                }
                else
                {
                    if (ApresentacaoCodigoAntigo >= 0)
                    {
                        IDataReader drNovoSetor = db.Consulta("SELECT MAX(SetorCodigo) NovoSetor FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID);
                        int NovoSetor = 0;
                        if (drNovoSetor.Read())
                        {
                            if (drNovoSetor["NovoSetor"] != null && drNovoSetor["NovoSetor"] != DBNull.Value)
                                NovoSetor = int.Parse(((string)drNovoSetor["NovoSetor"])) + 1;
                        }
                        if (NovoSetor > SETOR_MAX)
                            throw new Exception("O número cadastrado de setores é superior ao limite estabelecido!");
                        drNovoSetor.Close();

                        IDataReader drNovoPreco = db.Consulta("SELECT MAX(PrecoCodigo) NovoPreco FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID);
                        int NovoPreco = 0;
                        if (drNovoPreco.Read())
                        {
                            if (drNovoPreco["NovoPreco"] != null && drNovoPreco["NovoPreco"] != DBNull.Value)
                            {
                                NovoPreco = int.Parse(((string)drNovoPreco["NovoPreco"])) + 1;
                            }
                        }
                        if (NovoPreco > PRECO_MAX)
                            throw new Exception("O número cadastrado de preços é superior ao limite estabelecido!");
                        drNovoPreco.Close();

                        return this.Inserir(eventoID, apresentacaoID, setorID, precoID, EventoCodigoAntigo.ToString("000"), ApresentacaoCodigoAntigo.ToString(ApresentacaoCodigoTamanho), NovoSetor.ToString("00"), NovoPreco.ToString("00"), db);
                    }
                    else
                    {
                        if (EventoCodigoAntigo >= 0)
                        {
                            IDataReader drNovoApresentacao = db.Consulta("SELECT MAX(ApresentacaoCodigo) NovoApresentacao " + "FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID);
                            int NovoApresentacao = 0;
                            if (drNovoApresentacao.Read())
                            {
                                if (drNovoApresentacao["NovoApresentacao"] != null && drNovoApresentacao["NovoApresentacao"] != DBNull.Value)
                                {
                                    NovoApresentacao = int.Parse(((string)drNovoApresentacao["NovoApresentacao"])) + 1;
                                    if (NovoApresentacao > APRESENTACAO_MAX)
                                        NovoApresentacao = 0;
                                }
                            }
                            drNovoApresentacao.Close();

                            IDataReader drNovoSetor = db.Consulta("SELECT MAX(SetorCodigo) NovoSetor FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID);
                            int NovoSetor = 0;
                            if (drNovoSetor.Read())
                            {
                                if (drNovoSetor["NovoSetor"] != null && drNovoSetor["NovoSetor"] != DBNull.Value)
                                {
                                    NovoSetor = int.Parse(((string)drNovoSetor["NovoSetor"])) + 1;
                                }
                            }
                            if (NovoSetor > SETOR_MAX)
                                throw new Exception("O número cadastrado de setores é superior ao limite estabelecido!");
                            drNovoSetor.Close();

                            IDataReader drNovoPreco = db.Consulta("SELECT MAX(PrecoCodigo) NovoPreco FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID);

                            int NovoPreco = 0;
                            if (drNovoPreco.Read())
                            {
                                if (drNovoPreco["NovoPreco"] != null && drNovoPreco["NovoPreco"] != DBNull.Value)
                                {
                                    NovoPreco = int.Parse(((string)drNovoPreco["NovoPreco"])) + 1;
                                }
                            }
                            if (NovoPreco > PRECO_MAX)
                                throw new Exception("O número cadastrado de preços é superior ao limite estabelecido!");
                            drNovoPreco.Close();

                            return this.Inserir(eventoID, apresentacaoID, setorID, precoID, EventoCodigoAntigo.ToString("000"), NovoApresentacao.ToString(ApresentacaoCodigoTamanho), NovoSetor.ToString("00"), NovoPreco.ToString("00"), db);
                        }
                        //Aqui está gerando uma nova estrutura
                        //Utilizando a tabela auxiliar tCodigoConfig
                        else
                        {
                            IDataReader drNovoEvento = db.Consulta("EXEC sp_GerarNovaEstruturaCodigoConfig");
                            int NovoEvento = 0;
                            if (drNovoEvento.Read())
                                NovoEvento = Convert.ToInt32(drNovoEvento["Codigo"]);

                            drNovoEvento.Close();

                            IDataReader drNovoApresentacao = db.Consulta("SELECT MAX(ApresentacaoCodigo) NovoApresentacao " + "FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID);
                            int NovoApresentacao = 0;
                            if (drNovoApresentacao.Read())
                            {
                                if (drNovoApresentacao["NovoApresentacao"] != null && drNovoApresentacao["NovoApresentacao"] != DBNull.Value)
                                {
                                    NovoApresentacao = int.Parse(((string)drNovoApresentacao["NovoApresentacao"])) + 1;
                                    if (NovoApresentacao > APRESENTACAO_MAX)
                                        NovoApresentacao = 0;
                                }
                            }
                            drNovoApresentacao.Close();

                            IDataReader drNovoSetor = db.Consulta("SELECT MAX(SetorCodigo) NovoSetor FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID);
                            int NovoSetor = 0;
                            if (drNovoSetor.Read())
                            {
                                if (drNovoSetor["NovoSetor"] != null && drNovoSetor["NovoSetor"] != DBNull.Value)
                                {
                                    NovoSetor = int.Parse(((string)drNovoSetor["NovoSetor"])) + 1;
                                }
                            }
                            if (NovoSetor > SETOR_MAX)
                                throw new Exception("O número cadastrado de setores é superior ao limite estabelecido!");
                            drNovoSetor.Close();

                            IDataReader drNovoPreco = db.Consulta("SELECT MAX(PrecoCodigo) NovoPreco FROM tCodigoBarra (NOLOCK) WHERE EventoID = " + eventoID + " AND ApresentacaoID = " + apresentacaoID + " AND SetorID = " + setorID);
                            int NovoPreco = 0;
                            if (drNovoPreco.Read())
                            {
                                if (drNovoPreco["NovoPreco"] != null && drNovoPreco["NovoPreco"] != DBNull.Value)
                                {
                                    NovoPreco = int.Parse(((string)drNovoPreco["NovoPreco"])) + 1;
                                }
                            }
                            if (NovoPreco > PRECO_MAX)
                                throw new Exception("O número cadastrado de preços é superior ao limite estabelecido!");
                            drNovoPreco.Close();

                            return this.Inserir(eventoID, apresentacaoID, setorID, precoID, NovoEvento.ToString("000"), NovoApresentacao.ToString(ApresentacaoCodigoTamanho), NovoSetor.ToString("00"), NovoPreco.ToString("00"), db);
                        }
                    }
                }
            }
        }

        public void GerarCodigoBarraTODOSEventos()
        {
            try
            {
                CTLib.BD banco = new CTLib.BD();
                string select = @"SELECT DISTINCT e.LocalID, e.ID EventoID, a.ID ApresentacaoID, s.ID SetorID, p.ID PrecoID FROM tEvento e (NOLOCK)
                                INNER JOIN tApresentacao a (NOLOCK) ON a.EventoID = e.ID
                                INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = a.ID
                                INNER JOIN tSetor s (NOLOCK) ON s.ID = aps.SetorID
                                INNER JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = aps.ID
                                WHERE e.ID IN (SELECT DISTINCT EventoID FROM vwInfoVenda2 (NOLOCK))";
                System.Data.IDataReader dr = banco.Consulta(select);
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(dr);
                dr.Close();
                banco.FecharConsulta();
                StringBuilder sb = new StringBuilder();

                CodigoBarra cb = new CodigoBarra();

                foreach (System.Data.DataRow drow in dt.Rows)
                    cb.Inserir((int)drow["EventoID"], (int)drow["ApresentacaoID"], (int)drow["SetorID"], (int)drow["PrecoID"], banco);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public void GerarCodigoBarraEvento(int EventoID)
        {
            try
            {
                BD banco = new BD();

                IDataReader dr = banco.Consulta(@"SELECT e.ID EventoID, a.ID ApresentacaoID, s.ID SetorID, p.ID PrecoID FROM tEvento e
                    INNER JOIN tApresentacao a (NOLOCK) ON a.EventoID = e.ID
                    INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = a.ID
                    INNER JOIN tSetor s (NOLOCK) ON s.ID = aps.SetorID
                    INNER JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = aps.ID
                    WHERE e.ID = " + EventoID);
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(dr);
                dr.Close();
                banco.FecharConsulta();
                StringBuilder sb = new StringBuilder();
                CodigoBarra cb = new CodigoBarra();

                foreach (System.Data.DataRow drow in dt.Rows)
                    cb.Inserir((int)drow["EventoID"], (int)drow["ApresentacaoID"], (int)drow["SetorID"], (int)drow["PrecoID"], banco);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public DataTable RelatorioCodigosValidos(int eventoID, int apresentacaoID, int setorID)
        {
            if (eventoID == 0)
                throw new Exception("Nenhum evento foi selecionado!");

            string sql = @"SELECT DISTINCT CodigoBarra FROM tIngresso (NOLOCK)
                WHERE Status = 'I' AND
                CodigoBarra <> '' AND
                EventoID = " + eventoID;
            if (apresentacaoID != 0)
                sql = sql + "AND ApresentacaoID = " + apresentacaoID;
            if (setorID != 0)
                sql = sql + "AND SetorID = " + setorID;

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            SqlCommand oCommand = new SqlCommand(sql, (SqlConnection)bd.Cnn);
            oAdapter.SelectCommand = oCommand;
            DataTable oDataTable = new DataTable("CodigoBarra");
            oAdapter.Fill(oDataTable);

            return oDataTable;
        }

        public DataTable RelatorioCodigosInvalidos(int eventoID, int apresentacaoID, int setorID)
        {
            if (eventoID == 0)
                throw new Exception("Nenhum evento foi selecionado!");

            string sql = @"SELECT DISTINCT il.CodigoBarra FROM tIngressoLog il (NOLOCK)  
            INNER JOIN tIngresso i (NOLOCK) ON il.CodigoBarra <> i.CodigoBarra AND i.ID = il.IngressoID
            WHERE il.CodigoBarra <> '' AND i.EventoID = @eventoID ";
            if (apresentacaoID != 0)
                sql = sql + "AND i.ApresentacaoID = @apresentacaoID ";
            if (setorID != 0)
                sql = sql + "AND i.SetorID = @setorID ";
            sql = sql + @"UNION
            SELECT DISTINCT il.CodigoBarra FROM tIngressoLog il (NOLOCK)
            INNER JOIN tIngresso i (NOLOCK) ON i.ID = il.IngressoID
            WHERE il.CodigoBarra <> '' AND i.EventoID = 3288  AND il.Acao = 'C' ";
            if (apresentacaoID != 0)
                sql = sql + "AND i.ApresentacaoID = @apresentacaoID ";
            if (setorID != 0)
                sql = sql + "AND i.SetorID = @setorID ";

            sql = sql.Replace("@eventoID", eventoID.ToString()).Replace("@apresentacaoID", apresentacaoID.ToString()).Replace("@setorID", setorID.ToString());

            SqlDataAdapter oAdapter = new SqlDataAdapter();
            SqlCommand oCommand = new SqlCommand(sql, (SqlConnection)bd.Cnn);
            oAdapter.SelectCommand = oCommand;
            DataTable oDataTable = new DataTable("CodigoBarra");
            oAdapter.Fill(oDataTable);

            return oDataTable;
        }

        public bool ExisteLog(string codigoBarra)
        {
            object retorno = bd.ConsultaValor("SELECT COUNT(ID) FROM tIngressoLog WHERE CodigoBarra = '" + codigoBarra + "'");
            if (retorno is int)
                if ((int)retorno > 0)
                    return true;
                else
                    return false;
            else
                return false;
        }

        public string DecodificarCodigoBarra(string codigoBarra)
        {
            string codigoDecriptado = string.Empty;
            int anterior = -1;
            int atual = -1;
            int resultado = -1;

            ListCaesarValue oListCaesarValue = new ListCaesarValue();

            for (int i = codigoBarra.Length - 1; i >= 0; i--)
            {
                //primeira iteração
                if (i == codigoBarra.Length - 1)
                {
                    anterior = Convert.ToInt32(codigoBarra[i].ToString());
                    continue;
                }

                //última iteração
                if (i == 0)
                {
                    codigoDecriptado = codigoBarra[i] + codigoDecriptado;
                    break;
                }

                atual = Convert.ToInt32(codigoBarra[i].ToString());

                foreach (CaesarValue oCaesarValue in oListCaesarValue)
                {
                    if (oCaesarValue.Resultado == anterior.ToString() && oCaesarValue.Valor1 == atual.ToString())
                    {
                        //resultado = oCaesarValue.Valor2;
                        codigoDecriptado = resultado + codigoDecriptado;
                        anterior = atual;
                        resultado = -1;
                        break;
                    }
                }
            }
            return codigoDecriptado;



            /*string codigoCriptado = codigoBarra;
            string codigoDecriptado = string.Empty;
            int anterior = -1;
            int atual = -1;
            int resultado = -1;

            ListCaesarValue oListCaesarValue = new ListCaesarValue();

            for (int i = codigoCriptado.Length-1; i >=0; i--)
            {
                if (i == codigoCriptado.Length - 1)
                {
                    anterior = int.Parse(codigoCriptado[i].ToString());
                    continue;
                }
                if (i == 0)
                {
                    anterior = int.Parse(codigoCriptado[i].ToString());
                    codigoDecriptado = anterior.ToString() + codigoDecriptado;
                    continue;
                }
                
                resultado = -1;
                atual = int.Parse(codigoCriptado[i].ToString());

                foreach (CaesarValue oCaesarValue in oListCaesarValue)
                {
                    if (oCaesarValue.valor1 == atual && oCaesarValue.resultado == anterior)
                    {
                        resultado = oCaesarValue.valor2;
                        break;
                    }
                }
            
                if (resultado == -1)
                    throw new Exception("Erro na decriptação do código de barras!");

                codigoDecriptado = resultado.ToString() + codigoDecriptado;

                anterior = resultado;
                atual = -1;
            }
            return codigoDecriptado;*/
        }
        #endregion

        public Image GerarCodigoBarraInternet(string Code)
        {
            Bitmap fig = new Bitmap(125, 380);
            Graphics graph = Graphics.FromImage(fig);

            Barcode barcode = new Barcode();
            barcode.BarcodeType = BarcodeType.INTERLEAVED2OF5;
            barcode.Orientation = BarcodeOrientation.RightFacing;
            barcode.Font = new Font("Arial", 11);
            barcode.Data = Code;
            barcode.TextPosition = BarcodeTextPosition.Below;
            barcode.VerticalAlignment = BarcodeVerticalAlignment.Top;
            barcode.UseDefaultAddOnData();

            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            barcode.Draw(graph, new RectangleF(0, 0, 130f, 400f), GraphicsUnit.Inch, 0.01f, 0, null);

            return fig;
        }

        public Image GerarCodigoBarraInternetValeIngresso(string Code)
        {
            Bitmap fig = new Bitmap(380, 125);
            Graphics graph = Graphics.FromImage(fig);

            Barcode barcode = new Barcode();
            barcode.BarcodeType = BarcodeType.INTERLEAVED2OF5;
            barcode.Orientation = BarcodeOrientation.BottomFacing;
            barcode.Font = new Font("Arial", 8);
            barcode.Data = Code;
            barcode.TextPosition = BarcodeTextPosition.Below;
            barcode.VerticalAlignment = BarcodeVerticalAlignment.Top;
            barcode.UseDefaultAddOnData();

            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            barcode.Draw(graph, new RectangleF(0, 0, 400f, 130f), GraphicsUnit.Inch, 0.01f, 0, null);

            return fig;
        }

        public static DataTable TiposCodigoBarra()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("Tipo", typeof(char));
            dtt.Columns.Add("Nome", typeof(string));

            dtt.Rows.Add((char)Enumerators.TipoCodigoBarra.Estruturado, "Estruturado");
            dtt.Rows.Add((char)Enumerators.TipoCodigoBarra.ListaBranca, "Lista Branca");

            return dtt;
        }

        public List<string> BuscarListaBranca(int quantidade)
        {
            try
            {
                return BuscarListaBranca(bd, quantidade);
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<string> BuscarListaBranca(BD bd, int quantidade)
        {
            List<string> codigos = new List<string>();
            DataTable dttBulk = new DataTable();
            dttBulk.Columns.Add("ID", typeof(int));

            bd.Consulta(@"SELECT TOP " + quantidade + " ID, CodigoBarra FROM tListaBrancaCompleta (NOLOCK) WHERE Utilizado = 'F' ORDER BY NEWID()");

            while (bd.Consulta().Read())
            {
                dttBulk.Rows.Add(bd.LerInt("ID"));
                codigos.Add(bd.LerString("CodigoBarra"));
            }

            bd.Consulta().Close();

            if (quantidade != codigos.Count)
                throw new Exception("Não será possível gerar os ingressos deste evento.\nA lista branca de código de barras não possui a quantidade necessária de códigos a serem utilizados.\nContate a equipe de atendimento.");

            bd.BulkInsert(dttBulk, "#tmpCodigos", false, true);

            bd.Executar(@"UPDATE c SET c.Utilizado = 'T', DataUtilizado = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
            @"FROM tListaBrancaCompleta c 
            INNER JOIN #tmpCodigos ON c.ID = #tmpCodigos.ID
            DROP TABLE #tmpCodigos ");

            return codigos;
        }

        public string NovoCodigoBarraListaBranca(BD bd, int ApresentacaoSetorID)
        {
            if (bd.ConsultaAberta)
                bd.Consulta().Close();

            bd.Consulta("SELECT TOP 1 ID, Codigo FROM tCodigoBarraEvento (NOLOCK) WHERE Utilizado = 'F' AND ApresentacaoSetorID = " + ApresentacaoSetorID + " ORDER BY NEWID()");
            if (!bd.Consulta().Read())
                throw new Exception("Não será possível gerar o código de barras.\nNão existem mais registros na lista branca.\nPor favor, entre em contato com a equipe de atendimento.");

            int id = bd.LerInt("ID");
            string codigoBarra = bd.LerString("Codigo");

            bd.Consulta().Close();
            bd.Executar("UPDATE tCodigoBarraEvento SET Utilizado = 'T' WHERE ID = " + id + " AND ApresentacaoSetorID = " + ApresentacaoSetorID);
            return codigoBarra;
        }

        public bool AtualizaCodigoBarraListaBranca(BD bd, string CodigoBarra, int apresentacaosetorid)
        {
            int x = bd.Executar("UPDATE tCodigoBarraEvento SET Utilizado = 'T' WHERE Codigo = '" + CodigoBarra + "' AND ApresentacaoSetorID = " + apresentacaosetorid);

            bool ok = Convert.ToBoolean(x);

            return ok;
        }


        public static bool Valido(string codigo)
        {
            return codigo.Length == 12 || codigo.Length == 16 || codigo.Length == 18 || codigo.Length == 14;
        }
    }

    public class CaesarValue
    {
        public string Valor1;
        public string Valor2;
        public string Resultado;

        public CaesarValue(string valor1, string valor2, string resultado)
        {
            Valor1 = valor1;
            Valor2 = valor2;
            Resultado = resultado;
        }
    }

    public class ListCaesarValue : List<CaesarValue>
    {
        public ListCaesarValue()
            : base()
        {
            this.Add(new CaesarValue("0", "0", "5"));
            this.Add(new CaesarValue("0", "1", "7"));
            this.Add(new CaesarValue("0", "2", "1"));
            this.Add(new CaesarValue("0", "3", "9"));
            this.Add(new CaesarValue("0", "4", "0"));
            this.Add(new CaesarValue("0", "5", "4"));
            this.Add(new CaesarValue("0", "6", "2"));
            this.Add(new CaesarValue("0", "7", "6"));
            this.Add(new CaesarValue("0", "8", "8"));
            this.Add(new CaesarValue("0", "9", "3"));
            this.Add(new CaesarValue("1", "0", "5"));
            this.Add(new CaesarValue("1", "1", "2"));
            this.Add(new CaesarValue("1", "2", "1"));
            this.Add(new CaesarValue("1", "3", "7"));
            this.Add(new CaesarValue("1", "4", "4"));
            this.Add(new CaesarValue("1", "5", "0"));
            this.Add(new CaesarValue("1", "6", "6"));
            this.Add(new CaesarValue("1", "7", "8"));
            this.Add(new CaesarValue("1", "8", "9"));
            this.Add(new CaesarValue("1", "9", "3"));
            this.Add(new CaesarValue("2", "0", "6"));
            this.Add(new CaesarValue("2", "1", "3"));
            this.Add(new CaesarValue("2", "2", "1"));
            this.Add(new CaesarValue("2", "3", "0"));
            this.Add(new CaesarValue("2", "4", "5"));
            this.Add(new CaesarValue("2", "5", "9"));
            this.Add(new CaesarValue("2", "6", "8"));
            this.Add(new CaesarValue("2", "7", "7"));
            this.Add(new CaesarValue("2", "8", "4"));
            this.Add(new CaesarValue("2", "9", "2"));
            this.Add(new CaesarValue("3", "0", "7"));
            this.Add(new CaesarValue("3", "1", "1"));
            this.Add(new CaesarValue("3", "2", "8"));
            this.Add(new CaesarValue("3", "3", "6"));
            this.Add(new CaesarValue("3", "4", "3"));
            this.Add(new CaesarValue("3", "5", "0"));
            this.Add(new CaesarValue("3", "6", "4"));
            this.Add(new CaesarValue("3", "7", "2"));
            this.Add(new CaesarValue("3", "8", "9"));
            this.Add(new CaesarValue("3", "9", "5"));
            this.Add(new CaesarValue("4", "0", "4"));
            this.Add(new CaesarValue("4", "1", "7"));
            this.Add(new CaesarValue("4", "2", "6"));
            this.Add(new CaesarValue("4", "3", "8"));
            this.Add(new CaesarValue("4", "4", "3"));
            this.Add(new CaesarValue("4", "5", "2"));
            this.Add(new CaesarValue("4", "6", "9"));
            this.Add(new CaesarValue("4", "7", "5"));
            this.Add(new CaesarValue("4", "8", "1"));
            this.Add(new CaesarValue("4", "9", "0"));
            this.Add(new CaesarValue("5", "0", "8"));
            this.Add(new CaesarValue("5", "1", "6"));
            this.Add(new CaesarValue("5", "2", "3"));
            this.Add(new CaesarValue("5", "3", "7"));
            this.Add(new CaesarValue("5", "4", "4"));
            this.Add(new CaesarValue("5", "5", "2"));
            this.Add(new CaesarValue("5", "6", "0"));
            this.Add(new CaesarValue("5", "7", "9"));
            this.Add(new CaesarValue("5", "8", "5"));
            this.Add(new CaesarValue("5", "9", "1"));
            this.Add(new CaesarValue("6", "0", "1"));
            this.Add(new CaesarValue("6", "1", "4"));
            this.Add(new CaesarValue("6", "2", "3"));
            this.Add(new CaesarValue("6", "3", "8"));
            this.Add(new CaesarValue("6", "4", "5"));
            this.Add(new CaesarValue("6", "5", "7"));
            this.Add(new CaesarValue("6", "6", "0"));
            this.Add(new CaesarValue("6", "7", "9"));
            this.Add(new CaesarValue("6", "8", "6"));
            this.Add(new CaesarValue("6", "9", "2"));
            this.Add(new CaesarValue("7", "0", "3"));
            this.Add(new CaesarValue("7", "1", "6"));
            this.Add(new CaesarValue("7", "2", "1"));
            this.Add(new CaesarValue("7", "3", "4"));
            this.Add(new CaesarValue("7", "4", "5"));
            this.Add(new CaesarValue("7", "5", "0"));
            this.Add(new CaesarValue("7", "6", "9"));
            this.Add(new CaesarValue("7", "7", "7"));
            this.Add(new CaesarValue("7", "8", "8"));
            this.Add(new CaesarValue("7", "9", "2"));
            this.Add(new CaesarValue("8", "0", "3"));
            this.Add(new CaesarValue("8", "1", "5"));
            this.Add(new CaesarValue("8", "2", "8"));
            this.Add(new CaesarValue("8", "3", "1"));
            this.Add(new CaesarValue("8", "4", "6"));
            this.Add(new CaesarValue("8", "5", "9"));
            this.Add(new CaesarValue("8", "6", "2"));
            this.Add(new CaesarValue("8", "7", "4"));
            this.Add(new CaesarValue("8", "8", "7"));
            this.Add(new CaesarValue("8", "9", "0"));
            this.Add(new CaesarValue("9", "0", "8"));
            this.Add(new CaesarValue("9", "1", "2"));
            this.Add(new CaesarValue("9", "2", "9"));
            this.Add(new CaesarValue("9", "3", "3"));
            this.Add(new CaesarValue("9", "4", "6"));
            this.Add(new CaesarValue("9", "5", "1"));
            this.Add(new CaesarValue("9", "6", "4"));
            this.Add(new CaesarValue("9", "7", "7"));
            this.Add(new CaesarValue("9", "8", "0"));
            this.Add(new CaesarValue("9", "9", "5"));

        }

        public string Codificar(string valor1, string valor2)
        {
            foreach (CaesarValue oCaesarValue in this)
            {
                if (oCaesarValue.Valor1 == valor1 && oCaesarValue.Valor2 == valor2)
                    return oCaesarValue.Resultado;
            }
            return null;
        }
        public string Decodificar(string valor1, string resultado)
        {
            foreach (CaesarValue oCaesarValue in this)
            {
                if (oCaesarValue.Valor1 == valor1 && oCaesarValue.Resultado == resultado)
                    return oCaesarValue.Valor2;
            }
            return null;
        }
    }

    public class CodigoBarraLista : CodigoBarraLista_B
    {

        public CodigoBarraLista() { }

        public CodigoBarraLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
