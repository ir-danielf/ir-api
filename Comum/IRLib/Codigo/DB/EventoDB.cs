/******************************************************
* Arquivo EventoDB.cs
* Gerado em: 06/06/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "Evento_B"

    public abstract class Evento_B : BaseBD
    {

        public localid LocalID = new localid();
        public nome Nome = new nome();
        public vendadistribuida VendaDistribuida = new vendadistribuida();
        public versaoimagemingresso VersaoImagemIngresso = new versaoimagemingresso();
        public versaoimagemvale VersaoImagemVale = new versaoimagemvale();
        public versaoimagemvale2 VersaoImagemVale2 = new versaoimagemvale2();
        public versaoimagemvale3 VersaoImagemVale3 = new versaoimagemvale3();
        public impressaocodigobarra ImpressaoCodigoBarra = new impressaocodigobarra();
        public obrigacadastrocliente ObrigaCadastroCliente = new obrigacadastrocliente();
        public desabilitaautomatico DesabilitaAutomatico = new desabilitaautomatico();
        public resenha Resenha = new resenha();
        public publicar Publicar = new publicar();
        public destaque Destaque = new destaque();
        public prioridadedestaque PrioridadeDestaque = new prioridadedestaque();
        public imageminternet ImagemInternet = new imageminternet();
        public parcelas Parcelas = new parcelas();
        public entregagratuita EntregaGratuita = new entregagratuita();
        public retiradabilheteria RetiradaBilheteria = new retiradabilheteria();
        public financeiro Financeiro = new financeiro();
        public atencao Atencao = new atencao();
        public censura Censura = new censura();
        public entradaacompanhada EntradaAcompanhada = new entradaacompanhada();
        public pdvsemconveniencia PDVSemConveniencia = new pdvsemconveniencia();
        public retiradaingresso RetiradaIngresso = new retiradaingresso();
        public meiaentrada MeiaEntrada = new meiaentrada();
        public promocoes Promocoes = new promocoes();
        public aberturaportoes AberturaPortoes = new aberturaportoes();
        public duracaoevento DuracaoEvento = new duracaoevento();
        public release Release = new release();
        public descricaopadraoapresentacao DescricaoPadraoApresentacao = new descricaopadraoapresentacao();
        public publicarsemvendamotivo PublicarSemVendaMotivo = new publicarsemvendamotivo();
        public contratoid ContratoID = new contratoid();
        public permitirvendasemcontrato PermitirVendaSemContrato = new permitirvendasemcontrato();
        public localimagemmapaid LocalImagemMapaID = new localimagemmapaid();
        public dataaberturavenda DataAberturaVenda = new dataaberturavenda();
        public eventosubtipoid EventoSubTipoID = new eventosubtipoid();
        public obrigatoriedadeid ObrigatoriedadeID = new obrigatoriedadeid();
        public escolherlugarmarcado EscolherLugarMarcado = new escolherlugarmarcado();
        public mapaesquematicoid MapaEsquematicoID = new mapaesquematicoid();
        public exibequantidade ExibeQuantidade = new exibequantidade();
        public palavrachave PalavraChave = new palavrachave();
        public nivelrisco NivelRisco = new nivelrisco();
        public habilitarretiradatodospdv HabilitarRetiradaTodosPDV = new habilitarretiradatodospdv();
        public tipoimpressao TipoImpressao = new tipoimpressao();
        public tipocodigobarra TipoCodigoBarra = new tipocodigobarra();
        public filmeid FilmeID = new filmeid();
        public imagemdestaque ImagemDestaque = new imagemdestaque();
        public limitemaximoingressosevento LimiteMaximoIngressosEvento = new limitemaximoingressosevento();
        public codigopos CodigoPos = new codigopos();
        public venderpos VenderPos = new venderpos();
        public basecalculo BaseCalculo = new basecalculo();
        public tipocalculodesconto TipoCalculoDesconto = new tipocalculodesconto();
        public tipocalculo TipoCalculo = new tipocalculo();
        public alvara Alvara = new alvara();
        public avcb AVCB = new avcb();
        public vendasemalvara VendaSemAlvara = new vendasemalvara();
        public dataemissaoalvara DataEmissaoAlvara = new dataemissaoalvara();
        public datavalidadealvara DataValidadeAlvara = new datavalidadealvara();
        public dataemissaoavcb DataEmissaoAvcb = new dataemissaoavcb();
        public datavalidadeavcb DataValidadeAvcb = new datavalidadeavcb();
        public lotacao Lotacao = new lotacao();
        public fonteimposto FonteImposto = new fonteimposto();
        public porcentagemimposto PorcentagemImposto = new porcentagemimposto();
        public ativo Ativo = new ativo();
        public completarcadastro CompletarCadastro = new completarcadastro();

        public Evento_B() { }

        // passar o Usuario logado no sistema
        public Evento_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Evento
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tEvento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.VendaDistribuida.ValorBD = bd.LerString("VendaDistribuida");
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.ImpressaoCodigoBarra.ValorBD = bd.LerString("ImpressaoCodigoBarra");
                    this.ObrigaCadastroCliente.ValorBD = bd.LerString("ObrigaCadastroCliente");
                    this.DesabilitaAutomatico.ValorBD = bd.LerString("DesabilitaAutomatico");
                    this.Resenha.ValorBD = bd.LerString("Resenha");
                    this.Publicar.ValorBD = bd.LerString("Publicar");
                    this.Destaque.ValorBD = bd.LerString("Destaque");
                    this.PrioridadeDestaque.ValorBD = bd.LerInt("PrioridadeDestaque").ToString();
                    this.ImagemInternet.ValorBD = bd.LerString("ImagemInternet");
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.EntregaGratuita.ValorBD = bd.LerString("EntregaGratuita");
                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");
                    this.Financeiro.ValorBD = bd.LerString("Financeiro");
                    this.Atencao.ValorBD = bd.LerString("Atencao");
                    this.Censura.ValorBD = bd.LerString("Censura");
                    this.EntradaAcompanhada.ValorBD = bd.LerString("EntradaAcompanhada");
                    this.PDVSemConveniencia.ValorBD = bd.LerString("PDVSemConveniencia");
                    this.RetiradaIngresso.ValorBD = bd.LerString("RetiradaIngresso");
                    this.MeiaEntrada.ValorBD = bd.LerString("MeiaEntrada");
                    this.Promocoes.ValorBD = bd.LerString("Promocoes");
                    this.AberturaPortoes.ValorBD = bd.LerString("AberturaPortoes");
                    this.DuracaoEvento.ValorBD = bd.LerString("DuracaoEvento");
                    this.Release.ValorBD = bd.LerString("Release");
                    this.DescricaoPadraoApresentacao.ValorBD = bd.LerString("DescricaoPadraoApresentacao");
                    this.PublicarSemVendaMotivo.ValorBD = bd.LerInt("PublicarSemVendaMotivo").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.PermitirVendaSemContrato.ValorBD = bd.LerString("PermitirVendaSemContrato");
                    this.LocalImagemMapaID.ValorBD = bd.LerInt("LocalImagemMapaID").ToString();
                    this.DataAberturaVenda.ValorBD = bd.LerString("DataAberturaVenda");
                    this.EventoSubTipoID.ValorBD = bd.LerInt("EventoSubTipoID").ToString();
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.EscolherLugarMarcado.ValorBD = bd.LerString("EscolherLugarMarcado");
                    this.MapaEsquematicoID.ValorBD = bd.LerInt("MapaEsquematicoID").ToString();
                    this.ExibeQuantidade.ValorBD = bd.LerString("ExibeQuantidade");
                    this.PalavraChave.ValorBD = bd.LerString("PalavraChave");
                    this.NivelRisco.ValorBD = bd.LerInt("NivelRisco").ToString();
                    this.HabilitarRetiradaTodosPDV.ValorBD = bd.LerString("HabilitarRetiradaTodosPDV");
                    this.TipoImpressao.ValorBD = bd.LerString("TipoImpressao");
                    this.TipoCodigoBarra.ValorBD = bd.LerString("TipoCodigoBarra");
                    this.FilmeID.ValorBD = bd.LerInt("FilmeID").ToString();
                    this.ImagemDestaque.ValorBD = bd.LerString("ImagemDestaque");
                    this.LimiteMaximoIngressosEvento.ValorBD = bd.LerInt("LimiteMaximoIngressosEvento").ToString();
                    this.CodigoPos.ValorBD = bd.LerInt("CodigoPos").ToString();
                    this.VenderPos.ValorBD = bd.LerString("VenderPos");
                    this.BaseCalculo.ValorBD = bd.LerString("BaseCalculo");
                    this.TipoCalculoDesconto.ValorBD = bd.LerString("TipoCalculoDesconto");
                    this.TipoCalculo.ValorBD = bd.LerString("TipoCalculo");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.VendaSemAlvara.ValorBD = bd.LerString("VendaSemAlvara");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.FonteImposto.ValorBD = bd.LerString("FonteImposto");
                    this.PorcentagemImposto.ValorBD = bd.LerDecimal("PorcentagemImposto").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                    this.CompletarCadastro.ValorBD = bd.LerInt("CompletarCadastro").ToString();
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
        /// Preenche todos os atributos de Evento do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xEvento WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.VendaDistribuida.ValorBD = bd.LerString("VendaDistribuida");
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.ImpressaoCodigoBarra.ValorBD = bd.LerString("ImpressaoCodigoBarra");
                    this.ObrigaCadastroCliente.ValorBD = bd.LerString("ObrigaCadastroCliente");
                    this.DesabilitaAutomatico.ValorBD = bd.LerString("DesabilitaAutomatico");
                    this.Resenha.ValorBD = bd.LerString("Resenha");
                    this.Publicar.ValorBD = bd.LerString("Publicar");
                    this.Destaque.ValorBD = bd.LerString("Destaque");
                    this.PrioridadeDestaque.ValorBD = bd.LerInt("PrioridadeDestaque").ToString();
                    this.ImagemInternet.ValorBD = bd.LerString("ImagemInternet");
                    this.Parcelas.ValorBD = bd.LerInt("Parcelas").ToString();
                    this.EntregaGratuita.ValorBD = bd.LerString("EntregaGratuita");
                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");
                    this.Financeiro.ValorBD = bd.LerString("Financeiro");
                    this.Atencao.ValorBD = bd.LerString("Atencao");
                    this.Censura.ValorBD = bd.LerString("Censura");
                    this.EntradaAcompanhada.ValorBD = bd.LerString("EntradaAcompanhada");
                    this.PDVSemConveniencia.ValorBD = bd.LerString("PDVSemConveniencia");
                    this.RetiradaIngresso.ValorBD = bd.LerString("RetiradaIngresso");
                    this.MeiaEntrada.ValorBD = bd.LerString("MeiaEntrada");
                    this.Promocoes.ValorBD = bd.LerString("Promocoes");
                    this.AberturaPortoes.ValorBD = bd.LerString("AberturaPortoes");
                    this.DuracaoEvento.ValorBD = bd.LerString("DuracaoEvento");
                    this.Release.ValorBD = bd.LerString("Release");
                    this.DescricaoPadraoApresentacao.ValorBD = bd.LerString("DescricaoPadraoApresentacao");
                    this.PublicarSemVendaMotivo.ValorBD = bd.LerInt("PublicarSemVendaMotivo").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.PermitirVendaSemContrato.ValorBD = bd.LerString("PermitirVendaSemContrato");
                    this.LocalImagemMapaID.ValorBD = bd.LerInt("LocalImagemMapaID").ToString();
                    this.DataAberturaVenda.ValorBD = bd.LerString("DataAberturaVenda");
                    this.EventoSubTipoID.ValorBD = bd.LerInt("EventoSubTipoID").ToString();
                    this.ObrigatoriedadeID.ValorBD = bd.LerInt("ObrigatoriedadeID").ToString();
                    this.EscolherLugarMarcado.ValorBD = bd.LerString("EscolherLugarMarcado");
                    this.MapaEsquematicoID.ValorBD = bd.LerInt("MapaEsquematicoID").ToString();
                    this.ExibeQuantidade.ValorBD = bd.LerString("ExibeQuantidade");
                    this.PalavraChave.ValorBD = bd.LerString("PalavraChave");
                    this.NivelRisco.ValorBD = bd.LerInt("NivelRisco").ToString();
                    this.HabilitarRetiradaTodosPDV.ValorBD = bd.LerString("HabilitarRetiradaTodosPDV");
                    this.TipoImpressao.ValorBD = bd.LerString("TipoImpressao");
                    this.TipoCodigoBarra.ValorBD = bd.LerString("TipoCodigoBarra");
                    this.FilmeID.ValorBD = bd.LerInt("FilmeID").ToString();
                    this.ImagemDestaque.ValorBD = bd.LerString("ImagemDestaque");
                    this.LimiteMaximoIngressosEvento.ValorBD = bd.LerInt("LimiteMaximoIngressosEvento").ToString();
                    this.CodigoPos.ValorBD = bd.LerInt("CodigoPos").ToString();
                    this.VenderPos.ValorBD = bd.LerString("VenderPos");
                    this.BaseCalculo.ValorBD = bd.LerString("BaseCalculo");
                    this.TipoCalculoDesconto.ValorBD = bd.LerString("TipoCalculoDesconto");
                    this.TipoCalculo.ValorBD = bd.LerString("TipoCalculo");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.VendaSemAlvara.ValorBD = bd.LerString("VendaSemAlvara");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.FonteImposto.ValorBD = bd.LerString("FonteImposto");
                    this.PorcentagemImposto.ValorBD = bd.LerDecimal("PorcentagemImposto").ToString();
                    this.CompletarCadastro.ValorBD = bd.LerInt("CompletarCadastro").ToString();
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
                sql.Append("INSERT INTO cEvento (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xEvento (ID, Versao, LocalID, Nome, VendaDistribuida, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, ImpressaoCodigoBarra, ObrigaCadastroCliente, DesabilitaAutomatico, Resenha, Publicar, Destaque, PrioridadeDestaque, ImagemInternet, Parcelas, EntregaGratuita, RetiradaBilheteria, Financeiro, Atencao, Censura, EntradaAcompanhada, PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes, DuracaoEvento, Release, DescricaoPadraoApresentacao, PublicarSemVendaMotivo, ContratoID, PermitirVendaSemContrato, LocalImagemMapaID, DataAberturaVenda, EventoSubTipoID, ObrigatoriedadeID, EscolherLugarMarcado, MapaEsquematicoID, ExibeQuantidade, PalavraChave, NivelRisco, HabilitarRetiradaTodosPDV, TipoImpressao, TipoCodigoBarra, FilmeID, ImagemDestaque, LimiteMaximoIngressosEvento, CodigoPos, VenderPos, BaseCalculo, TipoCalculoDesconto, TipoCalculo, Alvara, AVCB, VendaSemAlvara, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, CompletarCadastro) ");
                sql.Append("SELECT ID, @V, LocalID, Nome, VendaDistribuida, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, ImpressaoCodigoBarra, ObrigaCadastroCliente, DesabilitaAutomatico, Resenha, Publicar, Destaque, PrioridadeDestaque, ImagemInternet, Parcelas, EntregaGratuita, RetiradaBilheteria, Financeiro, Atencao, Censura, EntradaAcompanhada, PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes, DuracaoEvento, Release, DescricaoPadraoApresentacao, PublicarSemVendaMotivo, ContratoID, PermitirVendaSemContrato, LocalImagemMapaID, DataAberturaVenda, EventoSubTipoID, ObrigatoriedadeID, EscolherLugarMarcado, MapaEsquematicoID, ExibeQuantidade, PalavraChave, NivelRisco, HabilitarRetiradaTodosPDV, TipoImpressao, TipoCodigoBarra, FilmeID, ImagemDestaque, LimiteMaximoIngressosEvento, CodigoPos, VenderPos, BaseCalculo, TipoCalculoDesconto, TipoCalculo, Alvara, AVCB, VendaSemAlvara, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, CompletarCadastro FROM tEvento WHERE ID = @I");
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
        /// Inserir novo(a) Evento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEvento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEvento(ID, LocalID, Nome, VendaDistribuida, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, ImpressaoCodigoBarra, ObrigaCadastroCliente, DesabilitaAutomatico, Resenha, Publicar, Destaque, PrioridadeDestaque, ImagemInternet, Parcelas, EntregaGratuita, RetiradaBilheteria, Financeiro, Atencao, Censura, EntradaAcompanhada, PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes, DuracaoEvento, Release, DescricaoPadraoApresentacao, PublicarSemVendaMotivo, ContratoID, PermitirVendaSemContrato, LocalImagemMapaID, DataAberturaVenda, EventoSubTipoID, ObrigatoriedadeID, EscolherLugarMarcado, MapaEsquematicoID, ExibeQuantidade, PalavraChave, NivelRisco, HabilitarRetiradaTodosPDV, TipoImpressao, TipoCodigoBarra, FilmeID, ImagemDestaque, LimiteMaximoIngressosEvento, CodigoPos, VenderPos, BaseCalculo, TipoCalculoDesconto, TipoCalculo, Alvara, AVCB, VendaSemAlvara, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, CompletarCadastro) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,@005,@006,@007,'@008','@009','@010','@011','@012','@013',@014,'@015',@016,'@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027','@028','@029','@030',@031,@032,'@033',@034,'@035',@036,@037,'@038',@039,'@040','@041',@042,'@043','@044','@045',@046,'@047',@048,@049,'@050','@051','@052','@053','@054','@055','@056','@057','@058','@059','@060',@061,'@062','@063', @064)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@008", this.ImpressaoCodigoBarra.ValorBD);
                sql.Replace("@009", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@010", this.DesabilitaAutomatico.ValorBD);
                sql.Replace("@011", this.Resenha.ValorBD);
                sql.Replace("@012", this.Publicar.ValorBD);
                sql.Replace("@013", this.Destaque.ValorBD);
                sql.Replace("@014", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@015", this.ImagemInternet.ValorBD);
                sql.Replace("@016", this.Parcelas.ValorBD);
                sql.Replace("@017", this.EntregaGratuita.ValorBD);
                sql.Replace("@018", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@019", this.Financeiro.ValorBD);
                sql.Replace("@020", this.Atencao.ValorBD);
                sql.Replace("@021", this.Censura.ValorBD);
                sql.Replace("@022", this.EntradaAcompanhada.ValorBD);
                sql.Replace("@023", this.PDVSemConveniencia.ValorBD);
                sql.Replace("@024", this.RetiradaIngresso.ValorBD);
                sql.Replace("@025", this.MeiaEntrada.ValorBD);
                sql.Replace("@026", this.Promocoes.ValorBD);
                sql.Replace("@027", this.AberturaPortoes.ValorBD);
                sql.Replace("@028", this.DuracaoEvento.ValorBD);
                sql.Replace("@029", this.Release.ValorBD);
                sql.Replace("@030", this.DescricaoPadraoApresentacao.ValorBD);
                sql.Replace("@031", this.PublicarSemVendaMotivo.ValorBD);
                sql.Replace("@032", this.ContratoID.ValorBD);
                sql.Replace("@033", this.PermitirVendaSemContrato.ValorBD);
                sql.Replace("@034", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@035", this.DataAberturaVenda.ValorBD);
                sql.Replace("@036", this.EventoSubTipoID.ValorBD);
                sql.Replace("@037", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@038", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@039", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@040", this.ExibeQuantidade.ValorBD);
                sql.Replace("@041", this.PalavraChave.ValorBD);
                sql.Replace("@042", this.NivelRisco.ValorBD);
                sql.Replace("@043", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@044", this.TipoImpressao.ValorBD);
                sql.Replace("@045", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@046", this.FilmeID.ValorBD);
                sql.Replace("@047", this.ImagemDestaque.ValorBD);
                sql.Replace("@048", this.LimiteMaximoIngressosEvento.ValorBD);
                sql.Replace("@049", this.CodigoPos.ValorBD);
                sql.Replace("@050", this.VenderPos.ValorBD);
                sql.Replace("@051", this.BaseCalculo.ValorBD);
                sql.Replace("@052", this.TipoCalculoDesconto.ValorBD);
                sql.Replace("@053", this.TipoCalculo.ValorBD);
                sql.Replace("@054", this.Alvara.ValorBD);
                sql.Replace("@055", this.AVCB.ValorBD);
                sql.Replace("@056", this.VendaSemAlvara.ValorBD);
                sql.Replace("@057", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@058", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@059", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@060", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@061", this.Lotacao.ValorBD);
                sql.Replace("@062", this.FonteImposto.ValorBD);
                sql.Replace("@063", this.PorcentagemImposto.ValorBD);
                sql.Replace("@064", this.CompletarCadastro.ValorBD);
                    

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
        /// Inserir novo(a) Evento
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cEvento");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tEvento(ID, LocalID, Nome, VendaDistribuida, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, ImpressaoCodigoBarra, ObrigaCadastroCliente, DesabilitaAutomatico, Resenha, Publicar, Destaque, PrioridadeDestaque, ImagemInternet, Parcelas, EntregaGratuita, RetiradaBilheteria, Financeiro, Atencao, Censura, EntradaAcompanhada, PDVSemConveniencia, RetiradaIngresso, MeiaEntrada, Promocoes, AberturaPortoes, DuracaoEvento, Release, DescricaoPadraoApresentacao, PublicarSemVendaMotivo, ContratoID, PermitirVendaSemContrato, LocalImagemMapaID, DataAberturaVenda, EventoSubTipoID, ObrigatoriedadeID, EscolherLugarMarcado, MapaEsquematicoID, ExibeQuantidade, PalavraChave, NivelRisco, HabilitarRetiradaTodosPDV, TipoImpressao, TipoCodigoBarra, FilmeID, ImagemDestaque, LimiteMaximoIngressosEvento, CodigoPos, VenderPos, BaseCalculo, TipoCalculoDesconto, TipoCalculo, Alvara, AVCB, VendaSemAlvara, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, CompletarCadastro) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,@005,@006,@007,'@008','@009','@010','@011','@012','@013',@014,'@015',@016,'@017','@018','@019','@020','@021','@022','@023','@024','@025','@026','@027','@028','@029','@030',@031,@032,'@033',@034,'@035',@036,@037,'@038',@039,'@040','@041',@042,'@043','@044','@045',@046,'@047',@048,@049,'@050','@051','@052','@053','@054','@055','@056','@057','@058','@059','@060',@061,'@062','@063', @064)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@008", this.ImpressaoCodigoBarra.ValorBD);
                sql.Replace("@009", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@010", this.DesabilitaAutomatico.ValorBD);
                sql.Replace("@011", this.Resenha.ValorBD);
                sql.Replace("@012", this.Publicar.ValorBD);
                sql.Replace("@013", this.Destaque.ValorBD);
                sql.Replace("@014", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@015", this.ImagemInternet.ValorBD);
                sql.Replace("@016", this.Parcelas.ValorBD);
                sql.Replace("@017", this.EntregaGratuita.ValorBD);
                sql.Replace("@018", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@019", this.Financeiro.ValorBD);
                sql.Replace("@020", this.Atencao.ValorBD);
                sql.Replace("@021", this.Censura.ValorBD);
                sql.Replace("@022", this.EntradaAcompanhada.ValorBD);
                sql.Replace("@023", this.PDVSemConveniencia.ValorBD);
                sql.Replace("@024", this.RetiradaIngresso.ValorBD);
                sql.Replace("@025", this.MeiaEntrada.ValorBD);
                sql.Replace("@026", this.Promocoes.ValorBD);
                sql.Replace("@027", this.AberturaPortoes.ValorBD);
                sql.Replace("@028", this.DuracaoEvento.ValorBD);
                sql.Replace("@029", this.Release.ValorBD);
                sql.Replace("@030", this.DescricaoPadraoApresentacao.ValorBD);
                sql.Replace("@031", this.PublicarSemVendaMotivo.ValorBD);
                sql.Replace("@032", this.ContratoID.ValorBD);
                sql.Replace("@033", this.PermitirVendaSemContrato.ValorBD);
                sql.Replace("@034", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@035", this.DataAberturaVenda.ValorBD);
                sql.Replace("@036", this.EventoSubTipoID.ValorBD);
                sql.Replace("@037", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@038", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@039", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@040", this.ExibeQuantidade.ValorBD);
                sql.Replace("@041", this.PalavraChave.ValorBD);
                sql.Replace("@042", this.NivelRisco.ValorBD);
                sql.Replace("@043", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@044", this.TipoImpressao.ValorBD);
                sql.Replace("@045", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@046", this.FilmeID.ValorBD);
                sql.Replace("@047", this.ImagemDestaque.ValorBD);
                sql.Replace("@048", this.LimiteMaximoIngressosEvento.ValorBD);
                sql.Replace("@049", this.CodigoPos.ValorBD);
                sql.Replace("@050", this.VenderPos.ValorBD);
                sql.Replace("@051", this.BaseCalculo.ValorBD);
                sql.Replace("@052", this.TipoCalculoDesconto.ValorBD);
                sql.Replace("@053", this.TipoCalculo.ValorBD);
                sql.Replace("@054", this.Alvara.ValorBD);
                sql.Replace("@055", this.AVCB.ValorBD);
                sql.Replace("@056", this.VendaSemAlvara.ValorBD);
                sql.Replace("@057", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@058", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@059", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@060", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@061", this.Lotacao.ValorBD);
                sql.Replace("@062", this.FonteImposto.ValorBD);
                sql.Replace("@063", this.PorcentagemImposto.ValorBD);
                sql.Replace("@064", this.CompletarCadastro.ValorBD);

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
        /// Atualiza Evento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cEvento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEvento SET LocalID = @001, Nome = '@002', VendaDistribuida = '@003', VersaoImagemIngresso = @004, VersaoImagemVale = @005, VersaoImagemVale2 = @006, VersaoImagemVale3 = @007, ImpressaoCodigoBarra = '@008', ObrigaCadastroCliente = '@009', DesabilitaAutomatico = '@010', Resenha = '@011', Publicar = '@012', Destaque = '@013', PrioridadeDestaque = @014, ImagemInternet = '@015', Parcelas = @016, EntregaGratuita = '@017', RetiradaBilheteria = '@018', Financeiro = '@019', Atencao = '@020', Censura = '@021', EntradaAcompanhada = '@022', PDVSemConveniencia = '@023', RetiradaIngresso = '@024', MeiaEntrada = '@025', Promocoes = '@026', AberturaPortoes = '@027', DuracaoEvento = '@028', Release = '@029', DescricaoPadraoApresentacao = '@030', PublicarSemVendaMotivo = @031, ContratoID = @032, PermitirVendaSemContrato = '@033', LocalImagemMapaID = @034, DataAberturaVenda = '@035', EventoSubTipoID = @036, ObrigatoriedadeID = @037, EscolherLugarMarcado = '@038', MapaEsquematicoID = @039, ExibeQuantidade = '@040', PalavraChave = '@041', NivelRisco = @042, HabilitarRetiradaTodosPDV = '@043', TipoImpressao = '@044', TipoCodigoBarra = '@045', FilmeID = @046, ImagemDestaque = '@047', LimiteMaximoIngressosEvento = @048, CodigoPos = @049, VenderPos = '@050', BaseCalculo = '@051', TipoCalculoDesconto = '@052', TipoCalculo = '@053', Alvara = '@054', AVCB = '@055', VendaSemAlvara = '@056', DataEmissaoAlvara = '@057', DataValidadeAlvara = '@058', DataEmissaoAvcb = '@059', DataValidadeAvcb = '@060', Lotacao = @061, FonteImposto = '@062', PorcentagemImposto = '@063', CompletarCadastro = @064 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@008", this.ImpressaoCodigoBarra.ValorBD);
                sql.Replace("@009", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@010", this.DesabilitaAutomatico.ValorBD);
                sql.Replace("@011", this.Resenha.ValorBD);
                sql.Replace("@012", this.Publicar.ValorBD);
                sql.Replace("@013", this.Destaque.ValorBD);
                sql.Replace("@014", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@015", this.ImagemInternet.ValorBD);
                sql.Replace("@016", this.Parcelas.ValorBD);
                sql.Replace("@017", this.EntregaGratuita.ValorBD);
                sql.Replace("@018", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@019", this.Financeiro.ValorBD);
                sql.Replace("@020", this.Atencao.ValorBD);
                sql.Replace("@021", this.Censura.ValorBD);
                sql.Replace("@022", this.EntradaAcompanhada.ValorBD);
                sql.Replace("@023", this.PDVSemConveniencia.ValorBD);
                sql.Replace("@024", this.RetiradaIngresso.ValorBD);
                sql.Replace("@025", this.MeiaEntrada.ValorBD);
                sql.Replace("@026", this.Promocoes.ValorBD);
                sql.Replace("@027", this.AberturaPortoes.ValorBD);
                sql.Replace("@028", this.DuracaoEvento.ValorBD);
                sql.Replace("@029", this.Release.ValorBD);
                sql.Replace("@030", this.DescricaoPadraoApresentacao.ValorBD);
                sql.Replace("@031", this.PublicarSemVendaMotivo.ValorBD);
                sql.Replace("@032", this.ContratoID.ValorBD);
                sql.Replace("@033", this.PermitirVendaSemContrato.ValorBD);
                sql.Replace("@034", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@035", this.DataAberturaVenda.ValorBD);
                sql.Replace("@036", this.EventoSubTipoID.ValorBD);
                sql.Replace("@037", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@038", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@039", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@040", this.ExibeQuantidade.ValorBD);
                sql.Replace("@041", this.PalavraChave.ValorBD);
                sql.Replace("@042", this.NivelRisco.ValorBD);
                sql.Replace("@043", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@044", this.TipoImpressao.ValorBD);
                sql.Replace("@045", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@046", this.FilmeID.ValorBD);
                sql.Replace("@047", this.ImagemDestaque.ValorBD);
                sql.Replace("@048", this.LimiteMaximoIngressosEvento.ValorBD);
                sql.Replace("@049", this.CodigoPos.ValorBD);
                sql.Replace("@050", this.VenderPos.ValorBD);
                sql.Replace("@051", this.BaseCalculo.ValorBD);
                sql.Replace("@052", this.TipoCalculoDesconto.ValorBD);
                sql.Replace("@053", this.TipoCalculo.ValorBD);
                sql.Replace("@054", this.Alvara.ValorBD);
                sql.Replace("@055", this.AVCB.ValorBD);
                sql.Replace("@056", this.VendaSemAlvara.ValorBD);
                sql.Replace("@057", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@058", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@059", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@060", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@061", this.Lotacao.ValorBD);
                sql.Replace("@062", this.FonteImposto.ValorBD);
                sql.Replace("@063", this.PorcentagemImposto.ValorBD);
                sql.Replace("@064", this.CompletarCadastro.ValorBD);

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
        /// Atualiza Evento
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEvento SET LocalID = @001, Nome = '@002', VendaDistribuida = '@003', VersaoImagemIngresso = @004, VersaoImagemVale = @005, VersaoImagemVale2 = @006, VersaoImagemVale3 = @007, ImpressaoCodigoBarra = '@008', ObrigaCadastroCliente = '@009', DesabilitaAutomatico = '@010', Resenha = '@011', Publicar = '@012', Destaque = '@013', PrioridadeDestaque = @014, ImagemInternet = '@015', Parcelas = @016, EntregaGratuita = '@017', RetiradaBilheteria = '@018', Financeiro = '@019', Atencao = '@020', Censura = '@021', EntradaAcompanhada = '@022', PDVSemConveniencia = '@023', RetiradaIngresso = '@024', MeiaEntrada = '@025', Promocoes = '@026', AberturaPortoes = '@027', DuracaoEvento = '@028', Release = '@029', DescricaoPadraoApresentacao = '@030', PublicarSemVendaMotivo = @031, ContratoID = @032, PermitirVendaSemContrato = '@033', LocalImagemMapaID = @034, DataAberturaVenda = '@035', EventoSubTipoID = @036, ObrigatoriedadeID = @037, EscolherLugarMarcado = '@038', MapaEsquematicoID = @039, ExibeQuantidade = '@040', PalavraChave = '@041', NivelRisco = @042, HabilitarRetiradaTodosPDV = '@043', TipoImpressao = '@044', TipoCodigoBarra = '@045', FilmeID = @046, ImagemDestaque = '@047', LimiteMaximoIngressosEvento = @048, CodigoPos = @049, VenderPos = '@050', BaseCalculo = '@051', TipoCalculoDesconto = '@052', TipoCalculo = '@053', Alvara = '@054', AVCB = '@055', VendaSemAlvara = '@056', DataEmissaoAlvara = '@057', DataValidadeAlvara = '@058', DataEmissaoAvcb = '@059', DataValidadeAvcb = '@060', Lotacao = @061, FonteImposto = '@062', PorcentagemImposto = '@063', CompletarCadastro = @064 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@007", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@008", this.ImpressaoCodigoBarra.ValorBD);
                sql.Replace("@009", this.ObrigaCadastroCliente.ValorBD);
                sql.Replace("@010", this.DesabilitaAutomatico.ValorBD);
                sql.Replace("@011", this.Resenha.ValorBD);
                sql.Replace("@012", this.Publicar.ValorBD);
                sql.Replace("@013", this.Destaque.ValorBD);
                sql.Replace("@014", this.PrioridadeDestaque.ValorBD);
                sql.Replace("@015", this.ImagemInternet.ValorBD);
                sql.Replace("@016", this.Parcelas.ValorBD);
                sql.Replace("@017", this.EntregaGratuita.ValorBD);
                sql.Replace("@018", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@019", this.Financeiro.ValorBD);
                sql.Replace("@020", this.Atencao.ValorBD);
                sql.Replace("@021", this.Censura.ValorBD);
                sql.Replace("@022", this.EntradaAcompanhada.ValorBD);
                sql.Replace("@023", this.PDVSemConveniencia.ValorBD);
                sql.Replace("@024", this.RetiradaIngresso.ValorBD);
                sql.Replace("@025", this.MeiaEntrada.ValorBD);
                sql.Replace("@026", this.Promocoes.ValorBD);
                sql.Replace("@027", this.AberturaPortoes.ValorBD);
                sql.Replace("@028", this.DuracaoEvento.ValorBD);
                sql.Replace("@029", this.Release.ValorBD);
                sql.Replace("@030", this.DescricaoPadraoApresentacao.ValorBD);
                sql.Replace("@031", this.PublicarSemVendaMotivo.ValorBD);
                sql.Replace("@032", this.ContratoID.ValorBD);
                sql.Replace("@033", this.PermitirVendaSemContrato.ValorBD);
                sql.Replace("@034", this.LocalImagemMapaID.ValorBD);
                sql.Replace("@035", this.DataAberturaVenda.ValorBD);
                sql.Replace("@036", this.EventoSubTipoID.ValorBD);
                sql.Replace("@037", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@038", this.EscolherLugarMarcado.ValorBD);
                sql.Replace("@039", this.MapaEsquematicoID.ValorBD);
                sql.Replace("@040", this.ExibeQuantidade.ValorBD);
                sql.Replace("@041", this.PalavraChave.ValorBD);
                sql.Replace("@042", this.NivelRisco.ValorBD);
                sql.Replace("@043", this.HabilitarRetiradaTodosPDV.ValorBD);
                sql.Replace("@044", this.TipoImpressao.ValorBD);
                sql.Replace("@045", this.TipoCodigoBarra.ValorBD);
                sql.Replace("@046", this.FilmeID.ValorBD);
                sql.Replace("@047", this.ImagemDestaque.ValorBD);
                sql.Replace("@048", this.LimiteMaximoIngressosEvento.ValorBD);
                sql.Replace("@049", this.CodigoPos.ValorBD);
                sql.Replace("@050", this.VenderPos.ValorBD);
                sql.Replace("@051", this.BaseCalculo.ValorBD);
                sql.Replace("@052", this.TipoCalculoDesconto.ValorBD);
                sql.Replace("@053", this.TipoCalculo.ValorBD);
                sql.Replace("@054", this.Alvara.ValorBD);
                sql.Replace("@055", this.AVCB.ValorBD);
                sql.Replace("@056", this.VendaSemAlvara.ValorBD);
                sql.Replace("@057", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@058", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@059", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@060", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@061", this.Lotacao.ValorBD);
                sql.Replace("@062", this.FonteImposto.ValorBD);
                sql.Replace("@063", this.PorcentagemImposto.ValorBD);
                sql.Replace("@064", this.CompletarCadastro.ValorBD);

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
        /// Exclui Evento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEvento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEvento WHERE ID=" + id;

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
        /// Exclui Evento com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cEvento WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tEvento WHERE ID=" + id;

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
        /// Exclui Evento
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

            this.LocalID.Limpar();
            this.Nome.Limpar();
            this.VendaDistribuida.Limpar();
            this.VersaoImagemIngresso.Limpar();
            this.VersaoImagemVale.Limpar();
            this.VersaoImagemVale2.Limpar();
            this.VersaoImagemVale3.Limpar();
            this.ImpressaoCodigoBarra.Limpar();
            this.ObrigaCadastroCliente.Limpar();
            this.DesabilitaAutomatico.Limpar();
            this.Resenha.Limpar();
            this.Publicar.Limpar();
            this.Destaque.Limpar();
            this.PrioridadeDestaque.Limpar();
            this.ImagemInternet.Limpar();
            this.Parcelas.Limpar();
            this.EntregaGratuita.Limpar();
            this.RetiradaBilheteria.Limpar();
            this.Financeiro.Limpar();
            this.Atencao.Limpar();
            this.Censura.Limpar();
            this.EntradaAcompanhada.Limpar();
            this.PDVSemConveniencia.Limpar();
            this.RetiradaIngresso.Limpar();
            this.MeiaEntrada.Limpar();
            this.Promocoes.Limpar();
            this.AberturaPortoes.Limpar();
            this.DuracaoEvento.Limpar();
            this.Release.Limpar();
            this.DescricaoPadraoApresentacao.Limpar();
            this.PublicarSemVendaMotivo.Limpar();
            this.ContratoID.Limpar();
            this.PermitirVendaSemContrato.Limpar();
            this.LocalImagemMapaID.Limpar();
            this.DataAberturaVenda.Limpar();
            this.EventoSubTipoID.Limpar();
            this.ObrigatoriedadeID.Limpar();
            this.EscolherLugarMarcado.Limpar();
            this.MapaEsquematicoID.Limpar();
            this.ExibeQuantidade.Limpar();
            this.PalavraChave.Limpar();
            this.NivelRisco.Limpar();
            this.HabilitarRetiradaTodosPDV.Limpar();
            this.TipoImpressao.Limpar();
            this.TipoCodigoBarra.Limpar();
            this.FilmeID.Limpar();
            this.ImagemDestaque.Limpar();
            this.LimiteMaximoIngressosEvento.Limpar();
            this.CodigoPos.Limpar();
            this.VenderPos.Limpar();
            this.BaseCalculo.Limpar();
            this.TipoCalculoDesconto.Limpar();
            this.TipoCalculo.Limpar();
            this.Alvara.Limpar();
            this.AVCB.Limpar();
            this.VendaSemAlvara.Limpar();
            this.DataEmissaoAlvara.Limpar();
            this.DataValidadeAlvara.Limpar();
            this.DataEmissaoAvcb.Limpar();
            this.DataValidadeAvcb.Limpar();
            this.Lotacao.Limpar();
            this.FonteImposto.Limpar();
            this.PorcentagemImposto.Limpar();
            this.CompletarCadastro.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.LocalID.Desfazer();
            this.Nome.Desfazer();
            this.VendaDistribuida.Desfazer();
            this.VersaoImagemIngresso.Desfazer();
            this.VersaoImagemVale.Desfazer();
            this.VersaoImagemVale2.Desfazer();
            this.VersaoImagemVale3.Desfazer();
            this.ImpressaoCodigoBarra.Desfazer();
            this.ObrigaCadastroCliente.Desfazer();
            this.DesabilitaAutomatico.Desfazer();
            this.Resenha.Desfazer();
            this.Publicar.Desfazer();
            this.Destaque.Desfazer();
            this.PrioridadeDestaque.Desfazer();
            this.ImagemInternet.Desfazer();
            this.Parcelas.Desfazer();
            this.EntregaGratuita.Desfazer();
            this.RetiradaBilheteria.Desfazer();
            this.Financeiro.Desfazer();
            this.Atencao.Desfazer();
            this.Censura.Desfazer();
            this.EntradaAcompanhada.Desfazer();
            this.PDVSemConveniencia.Desfazer();
            this.RetiradaIngresso.Desfazer();
            this.MeiaEntrada.Desfazer();
            this.Promocoes.Desfazer();
            this.AberturaPortoes.Desfazer();
            this.DuracaoEvento.Desfazer();
            this.Release.Desfazer();
            this.DescricaoPadraoApresentacao.Desfazer();
            this.PublicarSemVendaMotivo.Desfazer();
            this.ContratoID.Desfazer();
            this.PermitirVendaSemContrato.Desfazer();
            this.LocalImagemMapaID.Desfazer();
            this.DataAberturaVenda.Desfazer();
            this.EventoSubTipoID.Desfazer();
            this.ObrigatoriedadeID.Desfazer();
            this.EscolherLugarMarcado.Desfazer();
            this.MapaEsquematicoID.Desfazer();
            this.ExibeQuantidade.Desfazer();
            this.PalavraChave.Desfazer();
            this.NivelRisco.Desfazer();
            this.HabilitarRetiradaTodosPDV.Desfazer();
            this.TipoImpressao.Desfazer();
            this.TipoCodigoBarra.Desfazer();
            this.FilmeID.Desfazer();
            this.ImagemDestaque.Desfazer();
            this.LimiteMaximoIngressosEvento.Desfazer();
            this.CodigoPos.Desfazer();
            this.VenderPos.Desfazer();
            this.BaseCalculo.Desfazer();
            this.TipoCalculoDesconto.Desfazer();
            this.TipoCalculo.Desfazer();
            this.Alvara.Desfazer();
            this.AVCB.Desfazer();
            this.VendaSemAlvara.Desfazer();
            this.DataEmissaoAlvara.Desfazer();
            this.DataValidadeAlvara.Desfazer();
            this.DataEmissaoAvcb.Desfazer();
            this.DataValidadeAvcb.Desfazer();
            this.Lotacao.Desfazer();
            this.FonteImposto.Desfazer();
            this.PorcentagemImposto.Desfazer();
            this.CompletarCadastro.Desfazer();
        }

        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
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

        public class vendadistribuida : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaDistribuida";
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

        public class versaoimagemingresso : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemIngresso";
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

        public class versaoimagemvale : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale";
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

        public class versaoimagemvale2 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale2";
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

        public class versaoimagemvale3 : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VersaoImagemVale3";
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

        public class impressaocodigobarra : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImpressaoCodigoBarra";
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

        public class obrigacadastrocliente : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObrigaCadastroCliente";
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

        public class desabilitaautomatico : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "DesabilitaAutomatico";
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

        public class resenha : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Resenha";
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

        public class publicar : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Publicar";
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

        public class destaque : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Destaque";
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

        public class prioridadedestaque : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PrioridadeDestaque";
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

        public class imageminternet : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImagemInternet";
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

        public class parcelas : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Parcelas";
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

        public class entregagratuita : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntregaGratuita";
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

        public class retiradabilheteria : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RetiradaBilheteria";
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

        public class financeiro : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Financeiro";
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

        public class atencao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Atencao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class censura : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Censura";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 400;
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

        public class entradaacompanhada : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EntradaAcompanhada";
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

        public class pdvsemconveniencia : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PDVSemConveniencia";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class retiradaingresso : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "RetiradaIngresso";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class meiaentrada : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "MeiaEntrada";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class promocoes : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Promocoes";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
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

        public class aberturaportoes : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AberturaPortoes";
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

        public class duracaoevento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DuracaoEvento";
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

        public class release : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Release";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1000;
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

        public class descricaopadraoapresentacao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DescricaoPadraoApresentacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1600;
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

        public class publicarsemvendamotivo : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PublicarSemVendaMotivo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class contratoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContratoID";
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

        public class permitirvendasemcontrato : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "PermitirVendaSemContrato";
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

        public class localimagemmapaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalImagemMapaID";
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

        public class dataaberturavenda : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataAberturaVenda";
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class eventosubtipoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EventoSubTipoID";
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

        public class obrigatoriedadeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ObrigatoriedadeID";
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

        public class escolherlugarmarcado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "EscolherLugarMarcado";
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

        public class mapaesquematicoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MapaEsquematicoID";
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

        public class exibequantidade : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ExibeQuantidade";
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

        public class palavrachave : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "PalavraChave";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1000;
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

        public class nivelrisco : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "NivelRisco";
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

        public class habilitarretiradatodospdv : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "HabilitarRetiradaTodosPDV";
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

        public class tipoimpressao : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoImpressao";
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

        public class tipocodigobarra : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCodigoBarra";
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

        public class filmeid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FilmeID";
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

        public class imagemdestaque : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImagemDestaque";
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

        public class limitemaximoingressosevento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LimiteMaximoIngressosEvento";
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

        public class codigopos : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoPos";
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

        public class venderpos : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "VenderPos";
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

        public class basecalculo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "BaseCalculo";
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

        public class tipocalculodesconto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCalculoDesconto";
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

        public class tipocalculo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCalculo";
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

        public class alvara : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Alvara";
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

        public class avcb : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AVCB";
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

        public class vendasemalvara : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaSemAlvara";
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

        public class dataemissaoalvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAlvara";
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class datavalidadealvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAlvara";
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class dataemissaoavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAvcb";
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class datavalidadeavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAvcb";
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
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class lotacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Lotacao";
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

        public class fonteimposto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "FonteImposto";
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

        public class porcentagemimposto : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "PorcentagemImposto";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 3;
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

        }

        public class completarcadastro : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CompletarCadastro";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("VendaDistribuida", typeof(bool));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool));
                tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));
                tabela.Columns.Add("DesabilitaAutomatico", typeof(bool));
                tabela.Columns.Add("Resenha", typeof(string));
                tabela.Columns.Add("Publicar", typeof(string));
                tabela.Columns.Add("Destaque", typeof(bool));
                tabela.Columns.Add("PrioridadeDestaque", typeof(int));
                tabela.Columns.Add("ImagemInternet", typeof(string));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("EntregaGratuita", typeof(bool));
                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                tabela.Columns.Add("Financeiro", typeof(bool));
                tabela.Columns.Add("Atencao", typeof(string));
                tabela.Columns.Add("Censura", typeof(string));
                tabela.Columns.Add("EntradaAcompanhada", typeof(bool));
                tabela.Columns.Add("PDVSemConveniencia", typeof(string));
                tabela.Columns.Add("RetiradaIngresso", typeof(string));
                tabela.Columns.Add("MeiaEntrada", typeof(string));
                tabela.Columns.Add("Promocoes", typeof(string));
                tabela.Columns.Add("AberturaPortoes", typeof(string));
                tabela.Columns.Add("DuracaoEvento", typeof(string));
                tabela.Columns.Add("Release", typeof(string));
                tabela.Columns.Add("DescricaoPadraoApresentacao", typeof(string));
                tabela.Columns.Add("PublicarSemVendaMotivo", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("PermitirVendaSemContrato", typeof(bool));
                tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                tabela.Columns.Add("DataAberturaVenda", typeof(DateTime));
                tabela.Columns.Add("EventoSubTipoID", typeof(int));
                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                tabela.Columns.Add("EscolherLugarMarcado", typeof(bool));
                tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                tabela.Columns.Add("ExibeQuantidade", typeof(bool));
                tabela.Columns.Add("PalavraChave", typeof(string));
                tabela.Columns.Add("NivelRisco", typeof(int));
                tabela.Columns.Add("HabilitarRetiradaTodosPDV", typeof(bool));
                tabela.Columns.Add("TipoImpressao", typeof(string));
                tabela.Columns.Add("TipoCodigoBarra", typeof(string));
                tabela.Columns.Add("FilmeID", typeof(int));
                tabela.Columns.Add("ImagemDestaque", typeof(string));
                tabela.Columns.Add("LimiteMaximoIngressosEvento", typeof(int));
                tabela.Columns.Add("CodigoPos", typeof(int));
                tabela.Columns.Add("VenderPos", typeof(bool));
                tabela.Columns.Add("BaseCalculo", typeof(string));
                tabela.Columns.Add("TipoCalculoDesconto", typeof(string));
                tabela.Columns.Add("TipoCalculo", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("VendaSemAlvara", typeof(bool));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("FonteImposto", typeof(string));
                tabela.Columns.Add("PorcentagemImposto", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Todos();

        public abstract DataTable DestaquesHome();

        public abstract DataTable Setores();

        public abstract int Novo(DataSet info, int empresaid);

        public abstract DataTable Apresentacoes();

        public abstract DataTable Apresentacoes(Apresentacao.Disponibilidade disponibilidade);

        public abstract DataTable Canais();

        public abstract bool ExcluirCascata();

        public abstract DataTable VendasGerenciais(string datainicial, string datafinal, bool comcortesia, int apresentacaoid, int eventoid, int localid, int empresaid, bool vendascanal, string tipolinha, bool disponivel, bool empresavendeingressos, bool empresapromoveeventos);

        public abstract DataTable LinhasVendasGerenciais(string ingressologids);

        public abstract int QuantidadeIngressosPorEvento(string ingressologids);

        public abstract decimal ValorIngressosPorEvento(string ingressologids);

        public abstract DataTable LinhasBorderoFormaPagamento(string apresentacoes);

        public abstract DataTable BorderoFormaPagamento(string apresentacoes);

    }
    #endregion

    #region "EventoLista_B"

    public abstract class EventoLista_B : BaseLista
    {

        private bool backup = false;
        protected Evento evento;

        // passar o Usuario logado no sistema
        public EventoLista_B()
        {
            evento = new Evento();
        }

        // passar o Usuario logado no sistema
        public EventoLista_B(int usuarioIDLogado)
        {
            evento = new Evento(usuarioIDLogado);
        }

        public Evento Evento
        {
            get { return evento; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Evento especifico
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
                    evento.Ler(id);
                    return evento;
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
                    sql = "SELECT ID FROM tEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEvento";

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
                    sql = "SELECT ID FROM tEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tEvento";

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
                    sql = "SELECT ID FROM xEvento";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xEvento";

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
        /// Preenche Evento corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    evento.Ler(id);
                else
                    evento.LerBackup(id);

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

                bool ok = evento.Excluir();
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
        /// Inseri novo(a) Evento na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = evento.Inserir();
                if (ok)
                {
                    lista.Add(evento.Control.ID);
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
        ///  Obtem uma tabela de todos os campos de Evento carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Evento");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("VendaDistribuida", typeof(bool));
                tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                tabela.Columns.Add("VersaoImagemVale", typeof(int));
                tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool));
                tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));
                tabela.Columns.Add("DesabilitaAutomatico", typeof(bool));
                tabela.Columns.Add("Resenha", typeof(string));
                tabela.Columns.Add("Publicar", typeof(string));
                tabela.Columns.Add("Destaque", typeof(bool));
                tabela.Columns.Add("PrioridadeDestaque", typeof(int));
                tabela.Columns.Add("ImagemInternet", typeof(string));
                tabela.Columns.Add("Parcelas", typeof(int));
                tabela.Columns.Add("EntregaGratuita", typeof(bool));
                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                tabela.Columns.Add("Financeiro", typeof(bool));
                tabela.Columns.Add("Atencao", typeof(string));
                tabela.Columns.Add("Censura", typeof(string));
                tabela.Columns.Add("EntradaAcompanhada", typeof(bool));
                tabela.Columns.Add("PDVSemConveniencia", typeof(string));
                tabela.Columns.Add("RetiradaIngresso", typeof(string));
                tabela.Columns.Add("MeiaEntrada", typeof(string));
                tabela.Columns.Add("Promocoes", typeof(string));
                tabela.Columns.Add("AberturaPortoes", typeof(string));
                tabela.Columns.Add("DuracaoEvento", typeof(string));
                tabela.Columns.Add("Release", typeof(string));
                tabela.Columns.Add("DescricaoPadraoApresentacao", typeof(string));
                tabela.Columns.Add("PublicarSemVendaMotivo", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("PermitirVendaSemContrato", typeof(bool));
                tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                tabela.Columns.Add("DataAberturaVenda", typeof(DateTime));
                tabela.Columns.Add("EventoSubTipoID", typeof(int));
                tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                tabela.Columns.Add("EscolherLugarMarcado", typeof(bool));
                tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                tabela.Columns.Add("ExibeQuantidade", typeof(bool));
                tabela.Columns.Add("PalavraChave", typeof(string));
                tabela.Columns.Add("NivelRisco", typeof(int));
                tabela.Columns.Add("HabilitarRetiradaTodosPDV", typeof(bool));
                tabela.Columns.Add("TipoImpressao", typeof(string));
                tabela.Columns.Add("TipoCodigoBarra", typeof(string));
                tabela.Columns.Add("FilmeID", typeof(int));
                tabela.Columns.Add("ImagemDestaque", typeof(string));
                tabela.Columns.Add("LimiteMaximoIngressosEvento", typeof(int));
                tabela.Columns.Add("CodigoPos", typeof(int));
                tabela.Columns.Add("VenderPos", typeof(bool));
                tabela.Columns.Add("BaseCalculo", typeof(string));
                tabela.Columns.Add("TipoCalculoDesconto", typeof(string));
                tabela.Columns.Add("TipoCalculo", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("VendaSemAlvara", typeof(bool));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("FonteImposto", typeof(string));
                tabela.Columns.Add("PorcentagemImposto", typeof(decimal));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = evento.Control.ID;
                        linha["LocalID"] = evento.LocalID.Valor;
                        linha["Nome"] = evento.Nome.Valor;
                        linha["VendaDistribuida"] = evento.VendaDistribuida.Valor;
                        linha["VersaoImagemIngresso"] = evento.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = evento.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = evento.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = evento.VersaoImagemVale3.Valor;
                        linha["ImpressaoCodigoBarra"] = evento.ImpressaoCodigoBarra.Valor;
                        linha["ObrigaCadastroCliente"] = evento.ObrigaCadastroCliente.Valor;
                        linha["DesabilitaAutomatico"] = evento.DesabilitaAutomatico.Valor;
                        linha["Resenha"] = evento.Resenha.Valor;
                        linha["Publicar"] = evento.Publicar.Valor;
                        linha["Destaque"] = evento.Destaque.Valor;
                        linha["PrioridadeDestaque"] = evento.PrioridadeDestaque.Valor;
                        linha["ImagemInternet"] = evento.ImagemInternet.Valor;
                        linha["Parcelas"] = evento.Parcelas.Valor;
                        linha["EntregaGratuita"] = evento.EntregaGratuita.Valor;
                        linha["RetiradaBilheteria"] = evento.RetiradaBilheteria.Valor;
                        linha["Financeiro"] = evento.Financeiro.Valor;
                        linha["Atencao"] = evento.Atencao.Valor;
                        linha["Censura"] = evento.Censura.Valor;
                        linha["EntradaAcompanhada"] = evento.EntradaAcompanhada.Valor;
                        linha["PDVSemConveniencia"] = evento.PDVSemConveniencia.Valor;
                        linha["RetiradaIngresso"] = evento.RetiradaIngresso.Valor;
                        linha["MeiaEntrada"] = evento.MeiaEntrada.Valor;
                        linha["Promocoes"] = evento.Promocoes.Valor;
                        linha["AberturaPortoes"] = evento.AberturaPortoes.Valor;
                        linha["DuracaoEvento"] = evento.DuracaoEvento.Valor;
                        linha["Release"] = evento.Release.Valor;
                        linha["DescricaoPadraoApresentacao"] = evento.DescricaoPadraoApresentacao.Valor;
                        linha["PublicarSemVendaMotivo"] = evento.PublicarSemVendaMotivo.Valor;
                        linha["ContratoID"] = evento.ContratoID.Valor;
                        linha["PermitirVendaSemContrato"] = evento.PermitirVendaSemContrato.Valor;
                        linha["LocalImagemMapaID"] = evento.LocalImagemMapaID.Valor;
                        linha["DataAberturaVenda"] = evento.DataAberturaVenda.Valor;
                        linha["EventoSubTipoID"] = evento.EventoSubTipoID.Valor;
                        linha["ObrigatoriedadeID"] = evento.ObrigatoriedadeID.Valor;
                        linha["EscolherLugarMarcado"] = evento.EscolherLugarMarcado.Valor;
                        linha["MapaEsquematicoID"] = evento.MapaEsquematicoID.Valor;
                        linha["ExibeQuantidade"] = evento.ExibeQuantidade.Valor;
                        linha["PalavraChave"] = evento.PalavraChave.Valor;
                        linha["NivelRisco"] = evento.NivelRisco.Valor;
                        linha["HabilitarRetiradaTodosPDV"] = evento.HabilitarRetiradaTodosPDV.Valor;
                        linha["TipoImpressao"] = evento.TipoImpressao.Valor;
                        linha["TipoCodigoBarra"] = evento.TipoCodigoBarra.Valor;
                        linha["FilmeID"] = evento.FilmeID.Valor;
                        linha["ImagemDestaque"] = evento.ImagemDestaque.Valor;
                        linha["LimiteMaximoIngressosEvento"] = evento.LimiteMaximoIngressosEvento.Valor;
                        linha["CodigoPos"] = evento.CodigoPos.Valor;
                        linha["VenderPos"] = evento.VenderPos.Valor;
                        linha["BaseCalculo"] = evento.BaseCalculo.Valor;
                        linha["TipoCalculoDesconto"] = evento.TipoCalculoDesconto.Valor;
                        linha["TipoCalculo"] = evento.TipoCalculo.Valor;
                        linha["Alvara"] = evento.Alvara.Valor;
                        linha["AVCB"] = evento.AVCB.Valor;
                        linha["VendaSemAlvara"] = evento.VendaSemAlvara.Valor;
                        linha["DataEmissaoAlvara"] = evento.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = evento.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = evento.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = evento.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = evento.Lotacao.Valor;
                        linha["FonteImposto"] = evento.FonteImposto.Valor;
                        linha["PorcentagemImposto"] = evento.PorcentagemImposto.Valor;
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

                DataTable tabela = new DataTable("RelatorioEvento");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("LocalID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("VendaDistribuida", typeof(bool));
                    tabela.Columns.Add("VersaoImagemIngresso", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale2", typeof(int));
                    tabela.Columns.Add("VersaoImagemVale3", typeof(int));
                    tabela.Columns.Add("ImpressaoCodigoBarra", typeof(bool));
                    tabela.Columns.Add("ObrigaCadastroCliente", typeof(string));
                    tabela.Columns.Add("DesabilitaAutomatico", typeof(bool));
                    tabela.Columns.Add("Resenha", typeof(string));
                    tabela.Columns.Add("Publicar", typeof(string));
                    tabela.Columns.Add("Destaque", typeof(bool));
                    tabela.Columns.Add("PrioridadeDestaque", typeof(int));
                    tabela.Columns.Add("ImagemInternet", typeof(string));
                    tabela.Columns.Add("Parcelas", typeof(int));
                    tabela.Columns.Add("EntregaGratuita", typeof(bool));
                    tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                    tabela.Columns.Add("Financeiro", typeof(bool));
                    tabela.Columns.Add("Atencao", typeof(string));
                    tabela.Columns.Add("Censura", typeof(string));
                    tabela.Columns.Add("EntradaAcompanhada", typeof(bool));
                    tabela.Columns.Add("PDVSemConveniencia", typeof(string));
                    tabela.Columns.Add("RetiradaIngresso", typeof(string));
                    tabela.Columns.Add("MeiaEntrada", typeof(string));
                    tabela.Columns.Add("Promocoes", typeof(string));
                    tabela.Columns.Add("AberturaPortoes", typeof(string));
                    tabela.Columns.Add("DuracaoEvento", typeof(string));
                    tabela.Columns.Add("Release", typeof(string));
                    tabela.Columns.Add("DescricaoPadraoApresentacao", typeof(string));
                    tabela.Columns.Add("PublicarSemVendaMotivo", typeof(int));
                    tabela.Columns.Add("ContratoID", typeof(int));
                    tabela.Columns.Add("PermitirVendaSemContrato", typeof(bool));
                    tabela.Columns.Add("LocalImagemMapaID", typeof(int));
                    tabela.Columns.Add("DataAberturaVenda", typeof(DateTime));
                    tabela.Columns.Add("EventoSubTipoID", typeof(int));
                    tabela.Columns.Add("ObrigatoriedadeID", typeof(int));
                    tabela.Columns.Add("EscolherLugarMarcado", typeof(bool));
                    tabela.Columns.Add("MapaEsquematicoID", typeof(int));
                    tabela.Columns.Add("ExibeQuantidade", typeof(bool));
                    tabela.Columns.Add("PalavraChave", typeof(string));
                    tabela.Columns.Add("NivelRisco", typeof(int));
                    tabela.Columns.Add("HabilitarRetiradaTodosPDV", typeof(bool));
                    tabela.Columns.Add("TipoImpressao", typeof(string));
                    tabela.Columns.Add("TipoCodigoBarra", typeof(string));
                    tabela.Columns.Add("FilmeID", typeof(int));
                    tabela.Columns.Add("ImagemDestaque", typeof(string));
                    tabela.Columns.Add("LimiteMaximoIngressosEvento", typeof(int));
                    tabela.Columns.Add("CodigoPos", typeof(int));
                    tabela.Columns.Add("VenderPos", typeof(bool));
                    tabela.Columns.Add("BaseCalculo", typeof(string));
                    tabela.Columns.Add("TipoCalculoDesconto", typeof(string));
                    tabela.Columns.Add("TipoCalculo", typeof(string));
                    tabela.Columns.Add("Alvara", typeof(string));
                    tabela.Columns.Add("AVCB", typeof(string));
                    tabela.Columns.Add("VendaSemAlvara", typeof(bool));
                    tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                    tabela.Columns.Add("Lotacao", typeof(int));
                    tabela.Columns.Add("FonteImposto", typeof(string));
                    tabela.Columns.Add("PorcentagemImposto", typeof(decimal));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["LocalID"] = evento.LocalID.Valor;
                        linha["Nome"] = evento.Nome.Valor;
                        linha["VendaDistribuida"] = evento.VendaDistribuida.Valor;
                        linha["VersaoImagemIngresso"] = evento.VersaoImagemIngresso.Valor;
                        linha["VersaoImagemVale"] = evento.VersaoImagemVale.Valor;
                        linha["VersaoImagemVale2"] = evento.VersaoImagemVale2.Valor;
                        linha["VersaoImagemVale3"] = evento.VersaoImagemVale3.Valor;
                        linha["ImpressaoCodigoBarra"] = evento.ImpressaoCodigoBarra.Valor;
                        linha["ObrigaCadastroCliente"] = evento.ObrigaCadastroCliente.Valor;
                        linha["DesabilitaAutomatico"] = evento.DesabilitaAutomatico.Valor;
                        linha["Resenha"] = evento.Resenha.Valor;
                        linha["Publicar"] = evento.Publicar.Valor;
                        linha["Destaque"] = evento.Destaque.Valor;
                        linha["PrioridadeDestaque"] = evento.PrioridadeDestaque.Valor;
                        linha["ImagemInternet"] = evento.ImagemInternet.Valor;
                        linha["Parcelas"] = evento.Parcelas.Valor;
                        linha["EntregaGratuita"] = evento.EntregaGratuita.Valor;
                        linha["RetiradaBilheteria"] = evento.RetiradaBilheteria.Valor;
                        linha["Financeiro"] = evento.Financeiro.Valor;
                        linha["Atencao"] = evento.Atencao.Valor;
                        linha["Censura"] = evento.Censura.Valor;
                        linha["EntradaAcompanhada"] = evento.EntradaAcompanhada.Valor;
                        linha["PDVSemConveniencia"] = evento.PDVSemConveniencia.Valor;
                        linha["RetiradaIngresso"] = evento.RetiradaIngresso.Valor;
                        linha["MeiaEntrada"] = evento.MeiaEntrada.Valor;
                        linha["Promocoes"] = evento.Promocoes.Valor;
                        linha["AberturaPortoes"] = evento.AberturaPortoes.Valor;
                        linha["DuracaoEvento"] = evento.DuracaoEvento.Valor;
                        linha["Release"] = evento.Release.Valor;
                        linha["DescricaoPadraoApresentacao"] = evento.DescricaoPadraoApresentacao.Valor;
                        linha["PublicarSemVendaMotivo"] = evento.PublicarSemVendaMotivo.Valor;
                        linha["ContratoID"] = evento.ContratoID.Valor;
                        linha["PermitirVendaSemContrato"] = evento.PermitirVendaSemContrato.Valor;
                        linha["LocalImagemMapaID"] = evento.LocalImagemMapaID.Valor;
                        linha["DataAberturaVenda"] = evento.DataAberturaVenda.Valor;
                        linha["EventoSubTipoID"] = evento.EventoSubTipoID.Valor;
                        linha["ObrigatoriedadeID"] = evento.ObrigatoriedadeID.Valor;
                        linha["EscolherLugarMarcado"] = evento.EscolherLugarMarcado.Valor;
                        linha["MapaEsquematicoID"] = evento.MapaEsquematicoID.Valor;
                        linha["ExibeQuantidade"] = evento.ExibeQuantidade.Valor;
                        linha["PalavraChave"] = evento.PalavraChave.Valor;
                        linha["NivelRisco"] = evento.NivelRisco.Valor;
                        linha["HabilitarRetiradaTodosPDV"] = evento.HabilitarRetiradaTodosPDV.Valor;
                        linha["TipoImpressao"] = evento.TipoImpressao.Valor;
                        linha["TipoCodigoBarra"] = evento.TipoCodigoBarra.Valor;
                        linha["FilmeID"] = evento.FilmeID.Valor;
                        linha["ImagemDestaque"] = evento.ImagemDestaque.Valor;
                        linha["LimiteMaximoIngressosEvento"] = evento.LimiteMaximoIngressosEvento.Valor;
                        linha["CodigoPos"] = evento.CodigoPos.Valor;
                        linha["VenderPos"] = evento.VenderPos.Valor;
                        linha["BaseCalculo"] = evento.BaseCalculo.Valor;
                        linha["TipoCalculoDesconto"] = evento.TipoCalculoDesconto.Valor;
                        linha["TipoCalculo"] = evento.TipoCalculo.Valor;
                        linha["Alvara"] = evento.Alvara.Valor;
                        linha["AVCB"] = evento.AVCB.Valor;
                        linha["VendaSemAlvara"] = evento.VendaSemAlvara.Valor;
                        linha["DataEmissaoAlvara"] = evento.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = evento.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = evento.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = evento.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = evento.Lotacao.Valor;
                        linha["FonteImposto"] = evento.FonteImposto.Valor;
                        linha["PorcentagemImposto"] = evento.PorcentagemImposto.Valor;
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
                    case "LocalID":
                        sql = "SELECT ID, LocalID FROM tEvento WHERE " + FiltroSQL + " ORDER BY LocalID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tEvento WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "VendaDistribuida":
                        sql = "SELECT ID, VendaDistribuida FROM tEvento WHERE " + FiltroSQL + " ORDER BY VendaDistribuida";
                        break;
                    case "VersaoImagemIngresso":
                        sql = "SELECT ID, VersaoImagemIngresso FROM tEvento WHERE " + FiltroSQL + " ORDER BY VersaoImagemIngresso";
                        break;
                    case "VersaoImagemVale":
                        sql = "SELECT ID, VersaoImagemVale FROM tEvento WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale";
                        break;
                    case "VersaoImagemVale2":
                        sql = "SELECT ID, VersaoImagemVale2 FROM tEvento WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale2";
                        break;
                    case "VersaoImagemVale3":
                        sql = "SELECT ID, VersaoImagemVale3 FROM tEvento WHERE " + FiltroSQL + " ORDER BY VersaoImagemVale3";
                        break;
                    case "ImpressaoCodigoBarra":
                        sql = "SELECT ID, ImpressaoCodigoBarra FROM tEvento WHERE " + FiltroSQL + " ORDER BY ImpressaoCodigoBarra";
                        break;
                    case "ObrigaCadastroCliente":
                        sql = "SELECT ID, ObrigaCadastroCliente FROM tEvento WHERE " + FiltroSQL + " ORDER BY ObrigaCadastroCliente";
                        break;
                    case "DesabilitaAutomatico":
                        sql = "SELECT ID, DesabilitaAutomatico FROM tEvento WHERE " + FiltroSQL + " ORDER BY DesabilitaAutomatico";
                        break;
                    case "Resenha":
                        sql = "SELECT ID, Resenha FROM tEvento WHERE " + FiltroSQL + " ORDER BY Resenha";
                        break;
                    case "Publicar":
                        sql = "SELECT ID, Publicar FROM tEvento WHERE " + FiltroSQL + " ORDER BY Publicar";
                        break;
                    case "Destaque":
                        sql = "SELECT ID, Destaque FROM tEvento WHERE " + FiltroSQL + " ORDER BY Destaque";
                        break;
                    case "PrioridadeDestaque":
                        sql = "SELECT ID, PrioridadeDestaque FROM tEvento WHERE " + FiltroSQL + " ORDER BY PrioridadeDestaque";
                        break;
                    case "ImagemInternet":
                        sql = "SELECT ID, ImagemInternet FROM tEvento WHERE " + FiltroSQL + " ORDER BY ImagemInternet";
                        break;
                    case "Parcelas":
                        sql = "SELECT ID, Parcelas FROM tEvento WHERE " + FiltroSQL + " ORDER BY Parcelas";
                        break;
                    case "EntregaGratuita":
                        sql = "SELECT ID, EntregaGratuita FROM tEvento WHERE " + FiltroSQL + " ORDER BY EntregaGratuita";
                        break;
                    case "RetiradaBilheteria":
                        sql = "SELECT ID, RetiradaBilheteria FROM tEvento WHERE " + FiltroSQL + " ORDER BY RetiradaBilheteria";
                        break;
                    case "Financeiro":
                        sql = "SELECT ID, Financeiro FROM tEvento WHERE " + FiltroSQL + " ORDER BY Financeiro";
                        break;
                    case "Atencao":
                        sql = "SELECT ID, Atencao FROM tEvento WHERE " + FiltroSQL + " ORDER BY Atencao";
                        break;
                    case "Censura":
                        sql = "SELECT ID, Censura FROM tEvento WHERE " + FiltroSQL + " ORDER BY Censura";
                        break;
                    case "EntradaAcompanhada":
                        sql = "SELECT ID, EntradaAcompanhada FROM tEvento WHERE " + FiltroSQL + " ORDER BY EntradaAcompanhada";
                        break;
                    case "PDVSemConveniencia":
                        sql = "SELECT ID, PDVSemConveniencia FROM tEvento WHERE " + FiltroSQL + " ORDER BY PDVSemConveniencia";
                        break;
                    case "RetiradaIngresso":
                        sql = "SELECT ID, RetiradaIngresso FROM tEvento WHERE " + FiltroSQL + " ORDER BY RetiradaIngresso";
                        break;
                    case "MeiaEntrada":
                        sql = "SELECT ID, MeiaEntrada FROM tEvento WHERE " + FiltroSQL + " ORDER BY MeiaEntrada";
                        break;
                    case "Promocoes":
                        sql = "SELECT ID, Promocoes FROM tEvento WHERE " + FiltroSQL + " ORDER BY Promocoes";
                        break;
                    case "AberturaPortoes":
                        sql = "SELECT ID, AberturaPortoes FROM tEvento WHERE " + FiltroSQL + " ORDER BY AberturaPortoes";
                        break;
                    case "DuracaoEvento":
                        sql = "SELECT ID, DuracaoEvento FROM tEvento WHERE " + FiltroSQL + " ORDER BY DuracaoEvento";
                        break;
                    case "Release":
                        sql = "SELECT ID, Release FROM tEvento WHERE " + FiltroSQL + " ORDER BY Release";
                        break;
                    case "DescricaoPadraoApresentacao":
                        sql = "SELECT ID, DescricaoPadraoApresentacao FROM tEvento WHERE " + FiltroSQL + " ORDER BY DescricaoPadraoApresentacao";
                        break;
                    case "PublicarSemVendaMotivo":
                        sql = "SELECT ID, PublicarSemVendaMotivo FROM tEvento WHERE " + FiltroSQL + " ORDER BY PublicarSemVendaMotivo";
                        break;
                    case "ContratoID":
                        sql = "SELECT ID, ContratoID FROM tEvento WHERE " + FiltroSQL + " ORDER BY ContratoID";
                        break;
                    case "PermitirVendaSemContrato":
                        sql = "SELECT ID, PermitirVendaSemContrato FROM tEvento WHERE " + FiltroSQL + " ORDER BY PermitirVendaSemContrato";
                        break;
                    case "LocalImagemMapaID":
                        sql = "SELECT ID, LocalImagemMapaID FROM tEvento WHERE " + FiltroSQL + " ORDER BY LocalImagemMapaID";
                        break;
                    case "DataAberturaVenda":
                        sql = "SELECT ID, DataAberturaVenda FROM tEvento WHERE " + FiltroSQL + " ORDER BY DataAberturaVenda";
                        break;
                    case "EventoSubTipoID":
                        sql = "SELECT ID, EventoSubTipoID FROM tEvento WHERE " + FiltroSQL + " ORDER BY EventoSubTipoID";
                        break;
                    case "ObrigatoriedadeID":
                        sql = "SELECT ID, ObrigatoriedadeID FROM tEvento WHERE " + FiltroSQL + " ORDER BY ObrigatoriedadeID";
                        break;
                    case "EscolherLugarMarcado":
                        sql = "SELECT ID, EscolherLugarMarcado FROM tEvento WHERE " + FiltroSQL + " ORDER BY EscolherLugarMarcado";
                        break;
                    case "MapaEsquematicoID":
                        sql = "SELECT ID, MapaEsquematicoID FROM tEvento WHERE " + FiltroSQL + " ORDER BY MapaEsquematicoID";
                        break;
                    case "ExibeQuantidade":
                        sql = "SELECT ID, ExibeQuantidade FROM tEvento WHERE " + FiltroSQL + " ORDER BY ExibeQuantidade";
                        break;
                    case "PalavraChave":
                        sql = "SELECT ID, PalavraChave FROM tEvento WHERE " + FiltroSQL + " ORDER BY PalavraChave";
                        break;
                    case "NivelRisco":
                        sql = "SELECT ID, NivelRisco FROM tEvento WHERE " + FiltroSQL + " ORDER BY NivelRisco";
                        break;
                    case "HabilitarRetiradaTodosPDV":
                        sql = "SELECT ID, HabilitarRetiradaTodosPDV FROM tEvento WHERE " + FiltroSQL + " ORDER BY HabilitarRetiradaTodosPDV";
                        break;
                    case "TipoImpressao":
                        sql = "SELECT ID, TipoImpressao FROM tEvento WHERE " + FiltroSQL + " ORDER BY TipoImpressao";
                        break;
                    case "TipoCodigoBarra":
                        sql = "SELECT ID, TipoCodigoBarra FROM tEvento WHERE " + FiltroSQL + " ORDER BY TipoCodigoBarra";
                        break;
                    case "FilmeID":
                        sql = "SELECT ID, FilmeID FROM tEvento WHERE " + FiltroSQL + " ORDER BY FilmeID";
                        break;
                    case "ImagemDestaque":
                        sql = "SELECT ID, ImagemDestaque FROM tEvento WHERE " + FiltroSQL + " ORDER BY ImagemDestaque";
                        break;
                    case "LimiteMaximoIngressosEvento":
                        sql = "SELECT ID, LimiteMaximoIngressosEvento FROM tEvento WHERE " + FiltroSQL + " ORDER BY LimiteMaximoIngressosEvento";
                        break;
                    case "CodigoPos":
                        sql = "SELECT ID, CodigoPos FROM tEvento WHERE " + FiltroSQL + " ORDER BY CodigoPos";
                        break;
                    case "VenderPos":
                        sql = "SELECT ID, VenderPos FROM tEvento WHERE " + FiltroSQL + " ORDER BY VenderPos";
                        break;
                    case "BaseCalculo":
                        sql = "SELECT ID, BaseCalculo FROM tEvento WHERE " + FiltroSQL + " ORDER BY BaseCalculo";
                        break;
                    case "TipoCalculoDesconto":
                        sql = "SELECT ID, TipoCalculoDesconto FROM tEvento WHERE " + FiltroSQL + " ORDER BY TipoCalculoDesconto";
                        break;
                    case "TipoCalculo":
                        sql = "SELECT ID, TipoCalculo FROM tEvento WHERE " + FiltroSQL + " ORDER BY TipoCalculo";
                        break;
                    case "Alvara":
                        sql = "SELECT ID, Alvara FROM tEvento WHERE " + FiltroSQL + " ORDER BY Alvara";
                        break;
                    case "AVCB":
                        sql = "SELECT ID, AVCB FROM tEvento WHERE " + FiltroSQL + " ORDER BY AVCB";
                        break;
                    case "VendaSemAlvara":
                        sql = "SELECT ID, VendaSemAlvara FROM tEvento WHERE " + FiltroSQL + " ORDER BY VendaSemAlvara";
                        break;
                    case "DataEmissaoAlvara":
                        sql = "SELECT ID, DataEmissaoAlvara FROM tEvento WHERE " + FiltroSQL + " ORDER BY DataEmissaoAlvara";
                        break;
                    case "DataValidadeAlvara":
                        sql = "SELECT ID, DataValidadeAlvara FROM tEvento WHERE " + FiltroSQL + " ORDER BY DataValidadeAlvara";
                        break;
                    case "DataEmissaoAvcb":
                        sql = "SELECT ID, DataEmissaoAvcb FROM tEvento WHERE " + FiltroSQL + " ORDER BY DataEmissaoAvcb";
                        break;
                    case "DataValidadeAvcb":
                        sql = "SELECT ID, DataValidadeAvcb FROM tEvento WHERE " + FiltroSQL + " ORDER BY DataValidadeAvcb";
                        break;
                    case "Lotacao":
                        sql = "SELECT ID, Lotacao FROM tEvento WHERE " + FiltroSQL + " ORDER BY Lotacao";
                        break;
                    case "FonteImposto":
                        sql = "SELECT ID, FonteImposto FROM tEvento WHERE " + FiltroSQL + " ORDER BY FonteImposto";
                        break;
                    case "PorcentagemImposto":
                        sql = "SELECT ID, PorcentagemImposto FROM tEvento WHERE " + FiltroSQL + " ORDER BY PorcentagemImposto";
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

    #region "EventoException"

    [Serializable]
    public class EventoException : Exception
    {

        public EventoException() : base() { }

        public EventoException(string msg) : base(msg) { }

        public EventoException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}