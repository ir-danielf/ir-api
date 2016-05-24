using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DarumaFramework_DLL
{
    public class DarumaFramework_Declaracoes
    {
        public static int iRetorno;
        public static string sBuffer = string.Empty;
        public static string Str_LabelInputBox, Str_TextoInputBox, Str_Retorno_InputBox;
        public static string Str_Aleatorio;
        public static int iAleatorio;

        public static string InputBox(string LB_stringInputBox, string TB_InputBox)
        {
            Str_LabelInputBox = string.Empty;
            Str_TextoInputBox = string.Empty;
            Str_Retorno_InputBox = string.Empty;

            Str_LabelInputBox = LB_stringInputBox;
            Str_TextoInputBox = TB_InputBox;
            return Str_Retorno_InputBox;
        }

        public static void DarumaFramework_Mostrar_Retorno(int intRetorno)
        {
            int Int_NumErro = 0;
            int Int_NumAviso = 0;
            string Str_Msg_Retorno_Metodo = string.Empty;
            StringBuilder Str_Msg_NumErro = new StringBuilder(200);
            StringBuilder Str_Msg_NumAviso = new StringBuilder(200);

            if (intRetorno != 1)
            {

                switch (intRetorno.ToString())
                {

                    case "0": Str_Msg_Retorno_Metodo = "[0] - Erro durante a execução";
                        break;
                    case "-1": Str_Msg_Retorno_Metodo = "[-1] - Erro do Método";
                        break;
                    case "-2": Str_Msg_Retorno_Metodo = "[-2] - Parâmetro incorreto";
                        break;
                    case "-3": Str_Msg_Retorno_Metodo = "[-3] - Alíquota (Situação tributária) não programada";
                        break;
                    case "-4": Str_Msg_Retorno_Metodo = "[-4] - Chave do Registry não encontrada";
                        break;
                    case "-5": Str_Msg_Retorno_Metodo = "[-5] - Erro ao Abrir a porta de Comunicação";
                        break;
                    case "-6": Str_Msg_Retorno_Metodo = "[-6] - Impressora Desligada";
                        break;
                    case "-7": Str_Msg_Retorno_Metodo = "[-7] - Erro no Número do Banco";
                        break;
                    case "-8": Str_Msg_Retorno_Metodo = "[-8] - Erro ao Gravar as informações no arquivo de Status ou de Retorno de Info";
                        break;
                    case "-9": Str_Msg_Retorno_Metodo = "[-9] - Erro ao Fechar a porta de Comunicação";
                        break;
                    case "10": Str_Msg_Retorno_Metodo = "[10] - Se o ECF não tem forma de pagamento e não permite cadastrar esta forma";
                        break;
                    case "24": Str_Msg_Retorno_Metodo = "[24] - Forma de Pagamento não Programada";
                        break;
                    case "25": Str_Msg_Retorno_Metodo = "[25] - Totalizador nao ECF Não Vinculado nao Programado";
                        break;
                    case "27": Str_Msg_Retorno_Metodo = "[27] - Foi Detectado Erro ou Warning na Impressora";
                        break;
                    case "28": Str_Msg_Retorno_Metodo = "[28] - Time-Out";
                        break;
                    case "40": Str_Msg_Retorno_Metodo = "[40] - Tag XML Inválida";
                        break;
                    case "50": Str_Msg_Retorno_Metodo = "[50] - Problemas ao Criar Chave no Registry";
                        break;
                    case "51": Str_Msg_Retorno_Metodo = "[51] - Erro ao Gravar LOG";
                        break;
                    case "52": Str_Msg_Retorno_Metodo = "[52] - Erro ao abrir arquivo";
                        break;
                    case "53": Str_Msg_Retorno_Metodo = "[53] - Fim de arquivo";
                        break;
                    case "60": Str_Msg_Retorno_Metodo = "[60] - Erro na tag de formatacao DHTML";
                        break;
                    case "90": Str_Msg_Retorno_Metodo = "[90] - Erro Configurar a Porta de Comunicação";
                        break;
                    case "99": Str_Msg_Retorno_Metodo = "[99] - Parâmetro inválido ou ponteiro nulo de parâmetro";
                        break;

                    default: // Se o Retorno ok 
                        Str_Msg_Retorno_Metodo = "[" + iRetorno.ToString() + "] - Retorno Desconhecido!";
                        break;
                }
                DarumaFramework_DLL.DarumaFramework_Declaracoes.rStatusUltimoCmdInt_ECF_Daruma(ref Int_NumErro, ref Int_NumAviso);
                eInterpretarErro_ECF_Daruma(Int_NumErro, Str_Msg_NumErro);
                eInterpretarAviso_ECF_Daruma(Int_NumAviso, Str_Msg_NumAviso);

                System.Windows.Forms.MessageBox.Show("Retorno do Metodo = "
                + Str_Msg_Retorno_Metodo + "\r\n"
                + "Num.Erro = " + Str_Msg_NumErro + "\r\n"
                + "Num.Aviso= " + Str_Msg_NumAviso, "Daruma Framework Retorno do Metodo");
            }
            else
            {

                System.Windows.Forms.MessageBox.Show("Retorno do Metodo = ["
                + iRetorno + "]Métododo Executado com Sucesso!!" + "\r\n"
                + "Num.Erro = " + "[0] Sem erros" + "\r\n"
                + "Num.Aviso= " + "[0] Sem avisos", " Daruma Framework Retorno do Metodo");
            }
        }

        #region Métodos DarumaFramework

        [DllImport("DarumaFrameWork.dll")]
        public static extern int eDefinirProduto_Daruma(System.String sProduto);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int regRetornaValorChave_DarumaFramework(string sProduto, string sChave, [MarshalAs(UnmanagedType.VBByRefStr)] ref string szRetorno);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int regSintegra_ECF_Daruma(string sChave, string sValor);

        #endregion

        #region Métodos DUAL

        public static string NaoFiscal_Mostrar_Retorno(int iRetorno)
        {
            string vRet = string.Empty;

            if (iRetorno == 1 | iRetorno == 0 | iRetorno == -27 | iRetorno == -50 | iRetorno == 51 | iRetorno == 52)
                switch (iRetorno)
                {
                    case 0: vRet = "0(zero) - Impressora Desligada!";
                        break;
                    //case 1: TB_Status.Text = "1(um) - Impressora OK!";
                    //    break;
                    case -50: vRet = "-50 - Impressora OFF-LINE!";
                        break;
                    case -51: vRet = "-51 - Impressora Sem Papel!";
                        break;
                    case -27: vRet = "-27 - Erro Generico!";
                        break;
                    case -52: vRet = "-52 - Impressora inicializando!";
                        break;
                }

            else
            {
                vRet = "Retorno não esperado!";
            }

            return vRet;
        }
        //*************Métodos para Impressoras Dual*************

        [DllImport("DarumaFrameWork.dll")]
        public static extern int iEnviarBMP_DUAL_DarumaFramework(string stArqOrigem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iImprimirArquivo_DUAL_DarumaFramework(string stPath);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAcionarGaveta_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusGaveta_DUAL_DarumaFramework(ref int iStatusGaveta);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusDocumento_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusImpressora_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regVelocidade_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regTermica_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regTabulacao_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regPortaComunicacao_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regModoGaveta_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regLinhasGuilhotina_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regEnterFinal_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAguardarProcesso_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iImprimirTexto_DUAL_DarumaFramework(string stTexto, int iTam);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iAutenticarDocumento_DUAL_DarumaFramework(string stTexto, string stLocal, string stTimeOut);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCodePageAutomatico_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regZeroCortado_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eBuscarPortaVelocidade_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eCarregarBitmapPromocional_ECF_Daruma(string pszPathLogo, string pszIndiceLogo, string pszOrientacao);

        #endregion

        #region Métodos TA2000

        //*************Método TA2000*************

        //[DllImport("DarumaFrameWork.dll")]
        //public static extern int iEnviarDadosFormatados_TA2000_Daruma(string szTexto, string szRetorno);

        // Declaracao da Variavel por Referencia
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iEnviarDadosFormatados_TA2000_Daruma(System.String szTexto, [MarshalAs(UnmanagedType.VBByRefStr)] ref string szRetorno);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regPorta_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAuditoria_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMensagemBoasVindasLinha1_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMensagemBoasVindasLinha2_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMarcadorOpcao_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMascara_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMascaraLetra_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMascaraNumero_TA2000_Daruma(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMascaraEco_TA2000_Daruma(System.String stParametro);

        #endregion

        #region Métodos Modem


        [DllImport("DarumaFrameWork.dll")]
        public static extern int regLerApagar_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regPorta_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regThread_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regVelocidade_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regTempoAlertar_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCaptionWinAPP_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regBandejaInicio_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eInicializar_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eTrocarBandeja_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eApagarSms_MODEM_DarumaFramework(System.String iNumeroSMS);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rListarSms_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rNivelSinalRecebido_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rReceberSms_MODEM_DarumaFramework([MarshalAs(UnmanagedType.VBByRefStr)] ref string sIndiceSMS, [MarshalAs(UnmanagedType.VBByRefStr)] ref string sNumFone, [MarshalAs(UnmanagedType.VBByRefStr)] ref string sData, [MarshalAs(UnmanagedType.VBByRefStr)] ref string sHora, [MarshalAs(UnmanagedType.VBByRefStr)] ref string sMsg);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarImei_MODEM_DarumaFramework([MarshalAs(UnmanagedType.VBByRefStr)] ref string sImei);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarOperadora_MODEM_DarumaFramework([MarshalAs(UnmanagedType.VBByRefStr)] ref string sOperadora);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int tEnviarSms_MODEM_DarumaFramework(System.String sNumeroTelefone, System.String sMensagem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int tEnviarDadosCsd_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rReceberDadosCsd_MODEM_DarumaFramework(System.String sParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAtivarConexaoCsd_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eFinalizarChamadaCsd_MODEM_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eRealizarChamadaCsd_MODEM_DarumaFramework(System.String sParametro);
        #endregion

        #region Métodos Impressora Fiscal
        // --- Especiais - Inicio --- 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eInterpretarErro_ECF_Daruma(int iErro, StringBuilder pszDescErro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eInterpretarAviso_ECF_Daruma(int iAviso, StringBuilder pszDescAviso);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAguardarRecepcao_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAuditar_Daruma(string cAuditoria, int iFlag);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAuditar(string pszAuditoria, int iFlag);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eCancelaComunicacao_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eDefinirProduto(string pszProduto);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eDefinirModoRegistro_Daruma(int intiTipo);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eVerificarVersaoDLL_Daruma(StringBuilder pszRet);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eVerificarVersaoDLL_Daruma(string pszRet);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regLogin_Daruma(string pszPDV);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regLogin(string pszPDV);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regRetornaValorChave_DarumaFramework(string pszProduto, string pszChave, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regRetornaValorChave(string pszProduto, string pszChave, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAlteraValorChave_DarumaFramework(string pszProduto, string pszChave, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAlteraValorChave(string pszProduto, string pszChave, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAlterarValor_Daruma(string pszChave, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAtoCotepe_Daruma(string pszChave, string pszValor);


        // --- Especiais - Fim ---


        //FUNCOES ECF 
        // Abertura de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFAbrir_ECF_Daruma(string pszCPF, string pszNome, string pszEndereco);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFAbrirPadrao_ECF_Daruma();

        // Registro de item
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFVender_ECF_Daruma(string pszCargaTributaria, string pszQuantidade, string pszPrecoUnitario, string pszTipoDescAcresc, string pszValorDescAcresc, string pszCodigoItem, string pszUnidadeMedida, string pszDescricaoItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFVenderSemDesc_ECF_Daruma(string pszCargaTributaria, string pszQuantidade, string pszPrecoUnitario, string pszCodigoItem, string pszUnidadeMedida, string pszDescricaoItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFVenderResumido_ECF_Daruma(string pszCargaTributaria, string pszPrecoUnitario, string pszCodigoItem, string pszDescricaoItem);

        // Desconto ou acrescimo  em item de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int fnCFLancarDescAcrescItem_ECF_Daruma(string pszNumItem, string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFLancarAcrescimoItem_ECF_Daruma(string pszNumItem, string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFLancarDescontoItem_ECF_Daruma(string pszNumItem, string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFLancarAcrescimoUltimoItem_ECF_Daruma(string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFLancarDescontoUltimoItem_ECF_Daruma(string pszTipoDescAcresc, string pszValorDescAcresc);

        // Cancelamento total de item em cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarItem_ECF_Daruma(string pszNumItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarUltimoItem_ECF_Daruma();

        // Cancelamento parcial de item em cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarItemParcial_ECF_Daruma(string pszNumItem, string pszQuantidade);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarUltimoItemParcial_ECF_Daruma(string pszQuantidade);

        // Cancelamento de desconto em item
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarDescontoItem_ECF_Daruma(string pszNumItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarDescUltimoItem_ECF_Daruma();

        // Totalizacao de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFTotalizarCupom_ECF_Daruma(string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFTotalizarCupomPadrao_ECF_Daruma();

        //Cancelamento de desconto e acrescimo em subtotal de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarDescontoSubtotal_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelarAcrescimoSubtotal_ECF_Daruma();

        //Descricao do meios de pagamento de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEfetuarPagamentoPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEfetuarPagamentoFormatado_ECF_Daruma(string pszFormaPgto, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEfetuarPagamento_ECF_Daruma(string pszFormaPgto, string pszValor, string pszInfoAdicional);

        //Saldo a Pagar
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCFSaldoAPagar_ECF_Daruma(StringBuilder pszValor);

        //SubTotal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCFSubTotal_ECF_Daruma(StringBuilder pszValor);

        //Encerramento de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEncerrarPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEncerrarConfigMsg_ECF_Daruma(string pszMensagem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEncerrar_ECF_Daruma(string pszCupomAdicional, string pszMensagem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEncerrarResumido_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFEmitirCupomAdicional_ECF_Daruma();

        //Cancelamento de cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFCancelar_ECF_Daruma();

        //Status Cupom Fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCFVerificarStatus_ECF_Daruma(StringBuilder cStatusCF, ref int piStatusCF);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCFVerificarStatusInt_ECF_Daruma(ref int iStatusCF);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCFVerificarStatusStr_ECF_Daruma(StringBuilder cStatusCF);

        //Identificar consumidor radape do Cupom fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFIdentificarConsumidor_ECF_Daruma(string pszNome, string pszEndereco, string pszDoc);

        //Cupom Mania
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCMEfetuarCalculo_ECF_Daruma(StringBuilder pszISS, StringBuilder pszICMS);

        //Bilhete Passagem
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFBPAbrir_ECF_Daruma(string pszOrigem, string pszDestino, string pszUFDestino, string pszPercurso, string pszPrestadora, string pszPlataforma, string pszPoltrona, string pszModalidadetransp, string pszCategoriaTransp, string pszDataEmbarque, string pszRGPassageiro, string pszNomePassageiro, string pszEnderecoPassageiro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCFBPVender_ECF_Daruma(string pszAliquota, string pszValor, string pszTipoDescAcresc, string pszValorDescAcresc, string pszDescricao);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confCFBPProgramarUF_ECF_Daruma(string pszUF);

        //Download Memórias	
        //Espelho MFD 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rEfetuarDownloadMFD_ECF_Daruma(string pszTipo, string pszInicial, string pszFinal, string pszNomeArquivo);

        //Espelho MFD 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rGerarEspelhoMFD_ECF_Daruma(string pszTipo, string pszInicial, string pszFinal);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int rEfetuarDownloadMF_ECF_Daruma(string pszNomeArquivo);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int rEfetuarDownloadTDM_ECF_Daruma(string pszTipo, string pszInicial, string pszFinal, string pszNomeArquivo);

        // Relatorios PAF-ECF
        //Relatório PAF-ECF ON-line	
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rGerarRelatorio_ECF_Daruma(string pszRelatorio, string pszTipo, string pszInicial, string pszFinal);

        //Relatório PAF-ECF Off-line
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rGerarRelatorioOffline_ECF_Daruma(string szRelatorio, string szTipo, string szInicial, string szFinal, string szArquivo_MF, string szArquivo_MFD, string szArquivo_INF);

        //RSA - EAD PAF-ECF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rAssinarRSA_ECF_Daruma(string pszPathArquivo, string pszChavePrivada, [MarshalAs(UnmanagedType.VBByRefStr)] ref string pszAssinaturaGerada);

        //MD5	
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCalcularMD5_ECF_Daruma(string pszPathArquivo, StringBuilder pszMD5GeradoHex, StringBuilder pszMD5GeradoAscii);

        //Codigo de Barras
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iImprimirCodigoBarras_ECF_Daruma(string pszTipo, string pszCodigo, string pszLargura, string pszAltura, string pszPosicao);

        // --- ECF - Relatorio Gerencial - Inicio --- 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iRGAbrir_ECF_Daruma(string pszNomeRG);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iRGAbrirIndice_ECF_Daruma(int iIndiceRG);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iRGAbrirPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iRGImprimirTexto_ECF_Daruma(string pszTexto);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iRGFechar_ECF_Daruma();
        // --- ECF - Relatorio Gerencial - Fim --- 

        // --- ECF - Comprovante de CD - Inicio --- 
        // Abertura de comprovante de credito e debito
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDAbrir_ECF_Daruma(string pszFormaPgto, string pszParcelas, string pszDocOrigem, string pszValor, string pszCPF, string pszNome, string pszEndereco);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDAbrirSimplificado_ECF_Daruma(string pszFormaPgto, string pszParcelas, string pszDocOrigem, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDAbrirPadrao_ECF_Daruma();

        // Impressao de texto no comprovante de credito e debito
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDImprimirTexto_ECF_Daruma(string pszTexto);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDImprimirArquivo_ECF_Daruma(string pszArqOrigem);

        // Fechamento de texto no comprovante de credito e debito
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDFechar_ECF_Daruma();

        // Estorno de comprovante de credito e debito
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDEstornarPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCCDEstornar_ECF_Daruma(string pszCOO, string pszCPF, string pszNome, string pszEndereco);

        //Métodos para TEF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iTEF_ImprimirResposta_ECF_Daruma(String szArquivo, Boolean bTravarTeclado);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iTEF_ImprimirRespostaCartao_ECF_Daruma(string szArquivo, Boolean bTravarTeclado, string szForma, string szValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iTEF_Fechar_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eTEF_EsperarArquivo_ECF_Daruma(String szArquivo, int iTempo, Boolean bTravar);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eTEF_TravarTeclado_ECF_Daruma(Boolean bTravar);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eTEF_SetarFoco_ECF_Daruma(String szNomeTela);

        // --- ECF - Leitura Memoria Fiscal - Inicio --- 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iMFLerSerial_ECF_Daruma(string pszInicial, string pszFinal);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iMFLer_ECF_Daruma(string pszInicial, string pszFinal);

        // --- ECF - Comprovante não fiscal - Inicio --- 
        // Abertura de comprovante nao fiscal
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFAbrir_ECF_Daruma(string pszCPF, string pszNome, string pszEndereco);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFAbrirPadrao_ECF_Daruma();

        // Recebimento de itens
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFReceber_ECF_Daruma(string pszIndice, string pszValor, string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFReceberSemDesc_ECF_Daruma(string pszIndice, string pszValor);

        //Cancelamento de item
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarItem_ECF_Daruma(string pszNumItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarUltimoItem_ECF_Daruma();

        //Cancelamento de acrescimo em item
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarAcrescimoItem_ECF_Daruma(string pszNumItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarAcrescUltimoItem_ECF_Daruma();

        // Cancelamento de desconto em item
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarDescontoItem_ECF_Daruma(string pszNumItem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarDescUltimoItem_ECF_Daruma();

        // Totalizacao de CNF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFTotalizarComprovante_ECF_Daruma(string pszTipoDescAcresc, string pszValorDescAcresc);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFTotalizarComprovPadrao_ECF_Daruma();

        // Cancelamento de desconto e acrescimo em subtotal de CNF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarAcrescimoSubtotal_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelarDescontoSubtotal_ECF_Daruma();


        // Descricao do meios de pagamento de CNF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFEfetuarPagamento_ECF_Daruma(string pszFormaPgto, string pszValor, string pszInfoAdicional);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFEfetuarPagamentoFormatado_ECF_Daruma(string pszFormaPgto, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFEfetuarPagamentoPadrao_ECF_Daruma();

        // Encerramento de CNF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFEncerrar_ECF_Daruma(string pszMensagem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFEncerrarPadrao_ECF_Daruma();

        //Cancelamento de CNF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iCNFCancelar_ECF_Daruma();

        // --- ECF - Comprovante não fiscal - Fim ---

        // --- ECF - Funcoes Gerais - Inicio --- 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iEjetarCheque_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iEstornarPagamento_ECF_Daruma(string pszFormaPgtoEstornado, string pszFormaPgtoEfetivado, string pszValor, string pszInfoAdicional);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAcionarGuilhotina_ECF_Daruma(string pszTipoCorte);

        // Leitura X
        [DllImport("DarumaFrameWork.dll")]
        public static extern int fnLeituraX_ECF_Daruma(int iTipo, string pszCaminho);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iLeituraX_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLeituraX_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLeituraXCustomizada_ECF_Daruma(string pszCaminho);

        // Sangria
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iSangriaPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iSangria_ECF_Daruma(string pszValor, string pszMensagem);

        // Suprimento
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iSuprimentoPadrao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iSuprimento_ECF_Daruma(string pszValor, string pszMensagem);

        // Reducao Z
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iReducaoZ_ECF_Daruma(string NamelessParameter1, string NamelessParameter2);

        // Acionamento da Gaveta do ECF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAbrirGaveta_ECF_Daruma();

        // Programação do ECF
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confCadastrarPadrao_ECF_Daruma(string pszCadastrar, string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confCadastrar_ECF_Daruma(string pszCadastrar, string pszValor, string pszSeparador);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confHabilitarHorarioVerao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confDesabilitarHorarioVerao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confProgramarOperador_ECF_Daruma(string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confProgramarIDLoja_ECF_Daruma(string pszValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confProgramarAvancoPapel_ECF_Daruma(string pszSepEntreLinhas, string pszSepEntreDoc, string pszLinhasGuilhotina, string pszGuilhotina, string pszImpClicheAntecipada);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confHabilitarModoPreVenda_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int confDesabilitarModoPreVenda_ECF_Daruma();


        // Funcoes - Retorno
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerAliquotas_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cAliquotas);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerMeiosPagto_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszRelatorios);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerRG_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszRelatorios);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerDecimais_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszDecimalQtde, [MarshalAs(UnmanagedType.VBByRefStr)]  ref string pszDecimalValor, ref int piDecimalQtde, ref int piDecimalValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerDecimaisInt_ECF_Daruma(ref int piDecimalQtde, ref int piDecimalValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rLerDecimaisStr_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszDecimalQtde, [MarshalAs(UnmanagedType.VBByRefStr)]  ref string pszDecimalValor);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rDataHoraImpressora_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszData, [MarshalAs(UnmanagedType.VBByRefStr)] ref string pszHora);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rVerificarImpressoraLigada_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusImpressora_ECF_Daruma(StringBuilder pszStatus);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusImpressoraStr_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszStatus);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusImpressoraInt_ECF_Daruma(ref int piStatusEcf);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida_ECF_Daruma(int NamelessParameter1, [MarshalAs(UnmanagedType.VBByRefStr)]  ref string NamelessParameter2);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida1_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cInfoEx);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida2_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cInfoEx);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida3_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cInfoEx);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida4_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cInfoEx);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rInfoEstentida5_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cInfoEx);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rVerificarReducaoZ_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string zPendente);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusUltimoCmd_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszErro, [MarshalAs(UnmanagedType.VBByRefStr)] ref string pszAviso, ref int piErro, ref int piAviso);
        //--------------------

        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusUltimoCmdInt_ECF_Daruma(ref int piErro, ref int piAviso);
        //--------------------- 

        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusUltimoCmdStr_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string cErro, [MarshalAs(UnmanagedType.VBByRefStr)] ref string cAviso);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarInformacao_ECF_Daruma(string pszIndice, [MarshalAs(UnmanagedType.VBByRefStr)] ref string pszRetornar);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarNumeroSerieCodificado_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszSerialCriptografado);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rVerificarNumeroSerieCodificado_ECF_Daruma(string pszSerialCriptografado);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rCarregarNumeroSerie_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszSerial);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarDadosReducaoZ_ECF_Daruma([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszDados);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRegistrarNumeroSerie_ECF_Daruma();

        [DllImport("DarumaFrameWork.dll")]
        public static extern int rRetornarGTCodificado_ECF_Daruma(StringBuilder pszGTCodificado);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rVerificarGTCodificado_ECF_Daruma(string pszGTCodificado);



        // --- ECF - Funcoes Gerais - Fim ---

        // --- ECF - Especiais - Inicio --- 

        [DllImport("DarumaFrameWork.dll")]
        public static extern int eAguardarCompactacao_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eBuscarPortaVelocidade_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eEnviarComando_ECF_Daruma(string cComando, int iTamanhoComando, int iType);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eRetornarAviso_ECF_Daruma();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eRetornarErro_ECF_Daruma();
        // --- ECF - Especiais - Fim --- 


        // --- ECF - Registro - Inicio --- 
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCCDDocOrigem_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCCDFormaPgto_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCCDLinhasTEF_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCCDParcelas_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCCDValor_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFFormaPgto_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFMensagemPromocional_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFQuantidade_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFTamanhoMinimoDescricao_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFTipoDescAcresc_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFUnidadeMedida_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFValorDescAcresc_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFCupomAdicional_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFCupomMania(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFCupomAdicionalDllConfig_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCFCupomAdicionalDllTitulo_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeXLinha1_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeXLinha2_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeXLinha3_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeYLinha1_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeYLinha2_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regChequeYLinha3_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCompatStatusFuncao_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regMaxFechamentoAutomatico_ECF_Daruma(string pszParametro);

        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFAguardarImpressao_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFArquivoLeituraX_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFAuditoria_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFCaracterSeparador_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFMaxFechamentoAutomatico_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFReceberAvisoEmArquivo_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFReceberErroEmArquivo_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFReceberInfoEstendida_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regECFReceberInfoEstendidaEmArquivo_ECF_Daruma(string pszParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAtocotepe_ECF_Daruma(string pszParametro1, string pszParametro2);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int eDefinirModoRegistro_Daruma(string pszParametro);


        // --- ECF - Registro - Fim --- 

        #endregion
    }
}
