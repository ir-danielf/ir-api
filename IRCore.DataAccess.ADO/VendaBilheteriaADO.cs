using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using IRCore.DataAccess.ADO.Models;
using Dapper;
using IRCore.DataAccess.Model.Enumerator;
using IRLib;
using Ingresso = IRCore.DataAccess.Model.Ingresso;
using Local = IRCore.DataAccess.Model.Local;
using PontoVenda = IRCore.DataAccess.Model.PontoVenda;
using ValeIngresso = IRCore.DataAccess.Model.ValeIngresso;

namespace IRCore.DataAccess.ADO
{
    public class VendaBilheteriaADO : MasterADO<dbIngresso>
    {
        public VendaBilheteriaADO(MasterADOBase ado = null) : base(ado) { }
        List<tVendaBilheteria> result = new List<tVendaBilheteria>();
        private List<MeuIngresso> meusIngressos;

        public tVendaBilheteria Consultar(int ID)
        {
            #region strQuery
            var strQuery = @"SELECT TOP 1
	                            tVendaBilheteria.ID
                                ,tVendaBilheteria.CaixaID
                                ,tVendaBilheteria.ClienteID
                                ,tVendaBilheteria.Senha
                                ,tVendaBilheteria.DataVenda
                                ,tVendaBilheteria.Status
                                ,tVendaBilheteria.TaxaEntregaID
                                ,tVendaBilheteria.TaxaEntregaValor
                                ,tVendaBilheteria.TaxaConvenienciaValorTotal
                                ,tVendaBilheteria.ValorTotal
                                ,tVendaBilheteria.Obs
                                ,tVendaBilheteria.NotaFiscalCliente
                                ,tVendaBilheteria.NotaFiscalEstabelecimento
                                ,tVendaBilheteria.IndiceInstituicaoTransacao
                                ,tVendaBilheteria.IndiceTipoCartao
                                ,tVendaBilheteria.NSUSitef
                                ,tVendaBilheteria.NSUHost
                                ,tVendaBilheteria.CodigoAutorizacaoCredito
                                ,tVendaBilheteria.ModalidadePagamentoCodigo
                                ,tVendaBilheteria.ModalidadePagamentoTexto
                                ,tVendaBilheteria.BIN
                                ,tVendaBilheteria.TipoCancelamento
                                ,tVendaBilheteria.ComissaoValorTotal
                                ,tVendaBilheteria.IR
                                ,tVendaBilheteria.DataDeposito
                                ,tVendaBilheteria.NumeroCelular
                                ,tVendaBilheteria.ModelIDCelular
                                ,tVendaBilheteria.FabricanteCelular
                                ,tVendaBilheteria.DDD
                                ,tVendaBilheteria.NivelRisco
                                ,tVendaBilheteria.IP
                                ,tVendaBilheteria.MensagemRetorno
                                ,tVendaBilheteria.HoraTransacao
                                ,tVendaBilheteria.DataTransacao
                                ,tVendaBilheteria.CodigoIR
                                ,tVendaBilheteria.NumeroAutorizacao
                                ,tVendaBilheteria.Cupom
                                ,tVendaBilheteria.DadosConfirmacaoVenda
                                ,tVendaBilheteria.Rede
                                ,tVendaBilheteria.CodigoRespostaTransacao
                                ,tVendaBilheteria.CartaoID
                                ,tVendaBilheteria.Fraude
                                ,tVendaBilheteria.AprovacaoAutomatica
                                ,tVendaBilheteria.QuantidadeImpressoesInternet
                                ,tVendaBilheteria.VendaCancelada
                                ,tVendaBilheteria.MotivoID
                                ,tVendaBilheteria.ClienteEnderecoID
                                ,tVendaBilheteria.EntregaControleID
                                ,tVendaBilheteria.EntregaAgendaID
                                ,tVendaBilheteria.PdvID
                                ,tVendaBilheteria.EmailSincronizado
                                ,tVendaBilheteria.EmailEnviado
                                ,tVendaBilheteria.EmailEnviar
                                ,tVendaBilheteria.FeedbackPosVenda
                                ,tVendaBilheteria.PagamentoProcessado
                                ,tVendaBilheteria.NomeCartao
                                ,tVendaBilheteria.ValorSeguro
                                ,tVendaBilheteria.TaxaProcessamentoValor
                                ,tVendaBilheteria.TaxaProcessamentoCancelada
                                ,tVendaBilheteria.Score
                                ,tVendaBilheteria.RetornoAccertify
                                ,tVendaBilheteria.AccertifyForceStatus
                                ,tVendaBilheteria.VendaBilhereriaIDTroca
                                ,tVendaBilheteria.CodigoRastreio
                                ,tVendaBilheteria.EntregaCancelada
                                ,tVendaBilheteria.ConvenienciaCancelada
                                ,tVendaBilheteria.SeguroCancelado
                                ,tVendaBilheteria.CalcDataVenda
                            FROM
	                            tVendaBilheteria (NOLOCK)
                            WHERE
	                            ID = @ID";
            #endregion

            var result = conIngresso.Query<tVendaBilheteria>(strQuery, new { ID = ID }).FirstOrDefault();

            return result;
        }

        public tVendaBilheteria Consultar(string senha)
        {
            #region strQuery
            var strQuery = @"SELECT TOP 1
	                            tVendaBilheteria.ID
                                ,tVendaBilheteria.CaixaID
                                ,tVendaBilheteria.ClienteID
                                ,tVendaBilheteria.Senha
                                ,tVendaBilheteria.DataVenda
                                ,tVendaBilheteria.Status
                                ,tVendaBilheteria.TaxaEntregaID
                                ,tVendaBilheteria.TaxaEntregaValor
                                ,tVendaBilheteria.TaxaConvenienciaValorTotal
                                ,tVendaBilheteria.ValorTotal
                                ,tVendaBilheteria.Obs
                                ,tVendaBilheteria.NotaFiscalCliente
                                ,tVendaBilheteria.NotaFiscalEstabelecimento
                                ,tVendaBilheteria.IndiceInstituicaoTransacao
                                ,tVendaBilheteria.IndiceTipoCartao
                                ,tVendaBilheteria.NSUSitef
                                ,tVendaBilheteria.NSUHost
                                ,tVendaBilheteria.CodigoAutorizacaoCredito
                                ,tVendaBilheteria.ModalidadePagamentoCodigo
                                ,tVendaBilheteria.ModalidadePagamentoTexto
                                ,tVendaBilheteria.BIN
                                ,tVendaBilheteria.TipoCancelamento
                                ,tVendaBilheteria.ComissaoValorTotal
                                ,tVendaBilheteria.IR
                                ,tVendaBilheteria.DataDeposito
                                ,tVendaBilheteria.NumeroCelular
                                ,tVendaBilheteria.ModelIDCelular
                                ,tVendaBilheteria.FabricanteCelular
                                ,tVendaBilheteria.DDD
                                ,tVendaBilheteria.NivelRisco
                                ,tVendaBilheteria.IP
                                ,tVendaBilheteria.MensagemRetorno
                                ,tVendaBilheteria.HoraTransacao
                                ,tVendaBilheteria.DataTransacao
                                ,tVendaBilheteria.CodigoIR
                                ,tVendaBilheteria.NumeroAutorizacao
                                ,tVendaBilheteria.Cupom
                                ,tVendaBilheteria.DadosConfirmacaoVenda
                                ,tVendaBilheteria.Rede
                                ,tVendaBilheteria.CodigoRespostaTransacao
                                ,tVendaBilheteria.CartaoID
                                ,tVendaBilheteria.Fraude
                                ,tVendaBilheteria.AprovacaoAutomatica
                                ,tVendaBilheteria.QuantidadeImpressoesInternet
                                ,tVendaBilheteria.VendaCancelada
                                ,tVendaBilheteria.MotivoID
                                ,tVendaBilheteria.ClienteEnderecoID
                                ,tVendaBilheteria.EntregaControleID
                                ,tVendaBilheteria.EntregaAgendaID
                                ,tVendaBilheteria.PdvID
                                ,tVendaBilheteria.EmailSincronizado
                                ,tVendaBilheteria.EmailEnviado
                                ,tVendaBilheteria.EmailEnviar
                                ,tVendaBilheteria.FeedbackPosVenda
                                ,tVendaBilheteria.PagamentoProcessado
                                ,tVendaBilheteria.NomeCartao
                                ,tVendaBilheteria.ValorSeguro
                                ,tVendaBilheteria.TaxaProcessamentoValor
                                ,tVendaBilheteria.TaxaProcessamentoCancelada
                                ,tVendaBilheteria.Score
                                ,tVendaBilheteria.RetornoAccertify
                                ,tVendaBilheteria.AccertifyForceStatus
                                ,tVendaBilheteria.VendaBilhereriaIDTroca
                                ,tVendaBilheteria.CodigoRastreio
                                ,tVendaBilheteria.EntregaCancelada
                                ,tVendaBilheteria.ConvenienciaCancelada
                                ,tVendaBilheteria.SeguroCancelado
                                ,tVendaBilheteria.CalcDataVenda
                            FROM
	                            tVendaBilheteria (NOLOCK)
                            WHERE
	                            Senha = @senha";
            #endregion

            var result = conIngresso.Query<tVendaBilheteria>(strQuery, new { senha = senha }).FirstOrDefault();

            return result;
        }

        public tVendaBilheteria ConsultarComIngressosResumido(int ID)
        {
            #region strQuery
            var strQuery = @"SELECT DISTINCT 
                                VB.ID
                                ,VB.Senha
                                ,Cli.ID
                                ,Cli.Nome
                                ,Cli.CPF
                                ,Eve.ID
                                ,Eve.Nome
                                ,Loc.ID
                                ,Loc.Nome
                                ,Ing.ID
                                ,Ing.Codigo
                                ,(CASE WHEN VB.ID = Ing.VendaBilheteriaID THEN Ing.Status ELSE 'C' END) AS Status
                                ,Ing.VendaBilheteriaID
                            FROM 
                                tVendaBilheteria VB (NOLOCK)
                                INNER JOIN tCliente Cli (NOLOCK) ON VB.ClienteID = Cli.ID
                                INNER JOIN tIngressoLog Inl (NOLOCK) ON VB.ID = Inl.VendaBilheteriaID
                                INNER JOIN tIngresso Ing (NOLOCK) ON Inl.IngressoID = Ing.ID
                                INNER JOIN tEvento Eve (NOLOCK) ON Ing.EventoID = Eve.ID
                                INNER JOIN tLocal Loc (NOLOCK) ON Eve.LocalID = Loc.ID
                            WHERE 
                                VB.ID = @ID";
            #endregion

            tVendaBilheteria result = null;
            var query = conIngresso.Query<tVendaBilheteria, tCliente, tEvento, tLocal, tIngresso, int>(strQuery, (venda, cliente, evento, local, ingresso) =>
            {
                if (result == null)
                {
                    result = venda;
                    result.tIngresso = new List<tIngresso>();
                    result.tCliente = cliente;
                }
                ingresso.tEvento = evento;
                ingresso.tEvento.tLocal = local;

                result.tIngresso.Add(ingresso);

                return ingresso.ID;

            }, new { ID = ID });

            return result;
        }

        public IPagedList<tVendaBilheteria> ListarCliente(int pageNumber, int pageSize, int clienteID)
        {

            int startRow = ((pageNumber - 1) * pageSize);
            int endRow = (pageNumber * pageSize);

            #region strQuery1
            string strQuery1 = @"EXEC sp_getBilhetesCompras @startRow, @endRow, @clienteId";
            #endregion

            #region strQuery2
            string strQuery2 = @"SELECT
	                                vbfp.ID
	                                ,vbfp.FormaPagamentoID
	                                ,vbfp.VendaBilheteriaID
	                                ,vbfp.Valor
	                                ,vbfp.Porcentagem
	                                ,vbfp.Dias
	                                ,vbfp.TaxaAdm
	                                ,vbfp.IR
	                                ,vbfp.DataDeposito
	                                ,vbfp.Atualizado
	                                ,vbfp.VIRIngressoID
	                                ,vbfp.VendaBilheteriaFormaPagamentoTEFID
	                                ,vbfp.ValeIngressoID
	                                ,vbfp.MensagemRetorno
	                                ,vbfp.HoraTransacao
	                                ,vbfp.DataTransacao
	                                ,vbfp.CodigoIR
	                                ,vbfp.NumeroAutorizacao
	                                ,vbfp.NSUHost
	                                ,vbfp.NSUSitef
	                                ,vbfp.Cupom
	                                ,vbfp.DadosConfirmacaoVenda
	                                ,vbfp.Rede
	                                ,vbfp.CodigoRespostaTransacao
	                                ,vbfp.CartaoID
	                                ,vbfp.CodigoRespostaVenda
	                                ,vbfp.PayerIDPayPal
	                                ,vbfp.TokenPayPal
	                                ,vbfp.CorrelationID
	                                ,vbfp.TransactionID
	                                ,vbfp.Coeficiente
	                                ,vbfp.JurosValor 
	                                ,c.ID
	                                ,c.ClienteID
	                                ,c.NroCartao
	                                ,c.CheckSumCartao
	                                ,c.BandeiraID
	                                ,c.Status
	                                ,c.Ativo
	                                ,c.CartaoCr
	                                ,c.CVVCr
	                                ,c.DataCr
	                                ,c.NomeCartao
	                                ,vi.ID
	                                ,vi.ValeIngressoTipoID
	                                ,vi.CodigoTroca
	                                ,vi.DataCriacao
	                                ,vi.DataExpiracao
	                                ,vi.Status
	                                ,vi.VendaBilheteriaID
	                                ,vi.ClienteID
	                                ,vi.SessionID
	                                ,vi.TimeStampReserva
	                                ,vi.LojaID
	                                ,vi.UsuarioID
	                                ,vi.CanalID
	                                ,vi.ClienteNome
	                                ,vi.CodigoBarra
	                                ,vit.ID
	                                ,vit.Nome
	                                ,vit.Valor
	                                ,vit.ValidadeDiasImpressao
	                                ,vit.ValidadeData
	                                ,vit.ClienteTipo
	                                ,vit.ProcedimentoTroca
	                                ,vit.SaudacaoPadrao
	                                ,vit.SaudacaoNominal
	                                ,vit.QuantidadeLimitada
	                                ,vit.EmpresaID
	                                ,vit.CodigoTrocaFixo
	                                ,vit.Acumulativo
	                                ,vit.VersaoImagem
	                                ,vit.VersaoImagemInternet
	                                ,vit.ReleaseInternet
	                                ,vit.PublicarInternet
	                                ,vit.UltimoCodigoImpresso
	                                ,vit.TrocaIngresso
	                                ,vit.TrocaConveniencia
	                                ,vit.TrocaEntrega
	                                ,vit.ValorTipo
	                                ,vit.ValorPagamento
                                FROM 
	                                tVendaBilheteriaFormaPagamento (NOLOCK) vbfp
                                    LEFT JOIN tCartao (NOLOCK) c ON vbfp.CartaoID = c.ID
                                    LEFT JOIN tValeIngresso (NOLOCK) vi ON vbfp.ValeIngressoID = vi.ID
                                    LEFT JOIN tValeIngressoTipo (NOLOCK) vit ON vi.ValeIngressoTipoID = vit.ID
                                WHERE 
	                                vbfp.VendaBilheteriaID  = @VendaBilheteriaID";
            #endregion

            #region strQuery3
            string strQuery3 = @"SELECT
	                                i.ID
	                                ,i.ApresentacaoSetorID
	                                ,i.PrecoID
	                                ,i.LugarID
	                                ,i.UsuarioID
	                                ,i.CortesiaID
	                                ,i.BloqueioID
	                                ,i.Codigo
	                                ,i.Status
	                                ,CASE
		                              WHEN (SELECT COUNT(ID) FROM tVendaBilheteria WHERE Status = 'C' AND VendaBilheteriaIDOrigem = i.VendaBilheteriaID) > 0 THEN ''
                                        WHEN i.CodigoBarraCliente <> '' THEN i.CodigoBarraCliente
		                              ELSE i.CodigoBarra
                                     END AS CodigoBarra
	                                ,i.SetorID
	                                ,i.ApresentacaoID
	                                ,i.EventoID
	                                ,i.LocalID
	                                ,i.EmpresaID
	                                ,i.CodigoBarraCliente
	                                ,i.LojaID
	                                ,@VendaBilheteriaID as VendaBilheteriaID
	                                ,i.ClienteID
	                                ,i.PacoteID
	                                ,i.PacoteGrupo
	                                ,i.Classificacao
	                                ,i.Grupo
	                                ,i.SessionID
	                                ,i.TimeStampReserva
	                                ,i.CodigoSequencial
	                                ,i.PrecoExclusivoCodigoID
	                                ,i.CodigoImpressao
	                                ,i.CanalID
	                                ,i.AssinaturaClienteID
	                                ,i.SerieID
	                                ,i.CompraGUID
	                                ,i.GerenciamentoIngressosID
	                                ,i.ParceiroMidiaID
	                                ,e.ID
	                                ,e.LocalID
	                                ,e.EventoTipoID
	                                ,e.Nome
	                                ,e.DescricaoResumida
	                                ,e.DescricaoDetalhada
	                                ,e.VersaoImagemIngresso
	                                ,e.VersaoImagemVale
	                                ,e.Obs
	                                ,e.VendaDistribuida
	                                ,e.VersaoImagemVale2
	                                ,e.VersaoImagemVale3
	                                ,e.ImpressaoCodigoBarra
	                                ,e.ObrigaCadastroCliente
	                                ,e.CodigoBarraEstruturado
	                                ,e.DesabilitaAutomatico
	                                ,e.Resenha
	                                ,e.Publicar
	                                ,e.Destaque
	                                ,e.PrioridadeDestaque
	                                ,e.ImagemInternet
	                                ,e.EntregaGratuita
	                                ,e.RetiradaBilheteria
	                                ,e.Parcelas
	                                ,e.Financeiro
	                                ,e.Atencao
	                                ,e.PDVSemConveniencia
	                                ,e.RetiradaIngresso
	                                ,e.MeiaEntrada
	                                ,e.Promocoes
	                                ,e.AberturaPortoes
	                                ,e.DuracaoEvento
	                                ,e.Release
	                                ,e.DescricaoPadraoApresentacao
	                                ,e.Censura
	                                ,e.EntradaAcompanhada
	                                ,e.PublicarSemVendaMotivo
	                                ,e.ContratoID
	                                ,e.PermitirVendaSemContrato
	                                ,e.EventoSubTipoID
	                                ,e.LocalImagemMapaID
	                                ,e.DataAberturaVenda
	                                ,e.ObrigatoriedadeID
	                                ,e.EscolherLugarMarcado
	                                ,e.MapaEsquematicoID
	                                ,e.PalavraChave
	                                ,e.ExibeQuantidade
	                                ,e.DisponivelCortesiaInternet
	                                ,e.NivelRisco
	                                ,e.TaxaDistribuida
	                                ,e.MenorPeriodoEntrega
	                                ,e.TipoCodigoBarra
	                                ,e.TipoImpressao
	                                ,e.ImagemDestaque
	                                ,e.FilmeID
	                                ,e.PermiteVendaPacote
	                                ,e.LimiteMaximoIngressosEvento
	                                ,e.HabilitarRetiradaTodosPDV
	                                ,e.CodigoPos
	                                ,e.BaseCalculo
	                                ,e.TipoCalculoDesconto
	                                ,e.TipoCalculo
	                                ,e.Alvara
	                                ,e.AVCB
	                                ,e.VendaSemAlvara
	                                ,e.FonteImposto
	                                ,e.PorcentagemImposto
	                                ,e.DataEmissaoAlvara
	                                ,e.DataValidadeAlvara
	                                ,e.DataEmissaoAvcb
	                                ,e.DataValidadeAvcb
	                                ,e.Lotacao
	                                ,e.VenderPos
									,e.EntradaFranca
									,e.OcultarHoraApresentacao
	                                ,a.ID
	                                ,a.EventoID
	                                ,a.Horario
	                                ,a.DisponivelAjuste
	                                ,a.DisponivelRelatorio
	                                ,a.Obs
	                                ,a.DisponivelVenda
	                                ,a.VersaoImagemIngresso
	                                ,a.VersaoImagemVale
	                                ,a.VersaoImagemVale2
	                                ,a.VersaoImagemVale3
	                                ,a.Impressao
	                                ,a.LocalImagemMapaID
	                                ,a.DescricaoPadrao
	                                ,a.Descricao
	                                ,a.UltimoCodigoImpresso
	                                ,a.Quantidade
	                                ,a.QuantidadePorCliente
	                                ,a.CotaID
	                                ,a.MapaEsquematicoID
	                                ,a.Programacao
	                                ,a.CodigoProgramacao
	                                ,a.Sincronizado
	                                ,a.Alvara
	                                ,a.AVCB
	                                ,a.DataEmissaoAlvara
	                                ,a.DataValidadeAlvara
	                                ,a.DataEmissaoAvcb
	                                ,a.DataValidadeAvcb
	                                ,a.Lotacao
	                                ,a.CalcDiaDaSemana
	                                ,a.CalcHorario
	                                ,s.ID
	                                ,s.LocalID
	                                ,s.Nome
	                                ,s.NomeInterno
	                                ,s.Produto
	                                ,s.Descricao
	                                ,s.LugarMarcado
	                                ,s.ObservacaoImportante
	                                ,s.DistanciaPalco
	                                ,s.Acesso
	                                ,s.VersaoBackground
	                                ,s.AprovadoPublicacao
	                                ,s.CodigoSala
	                                ,s.Capacidade
	                                ,s.Linhas
	                                ,s.Colunas
	                                ,p.ID
	                                ,p.ApresentacaoSetorID
	                                ,p.Nome
	                                ,p.CorID
	                                ,p.Quantidade
	                                ,p.QuantidadePorCliente
	                                ,p.Valor
	                                ,p.Impressao
	                                ,p.IRVende
	                                ,p.ImprimirCarimbo
	                                ,p.CarimboTexto1
	                                ,p.CarimboTexto2
	                                ,p.PrecoTipoID
	                                ,p.CodigoCinema
	                                ,p.ParceiroMidiaID
	                                ,l.ID
	                                ,l.EmpresaID
	                                ,l.Nome
	                                ,l.Contato
	                                ,l.Endereco
	                                ,l.Cidade
	                                ,l.Estado
	                                ,l.CEP
	                                ,l.DDDTelefone
	                                ,l.Telefone
	                                ,l.Obs
	                                ,l.Logradouro
	                                ,l.Bairro
	                                ,l.Numero
	                                ,l.Estacionamento
	                                ,l.EstacionamentoObs
	                                ,l.AcessoNecessidadeEspecial
	                                ,l.AcessoNecessidadeEspecialObs
	                                ,l.ArCondicionado
	                                ,l.ComoChegar
	                                ,l.RetiradaBilheteria
	                                ,l.HorariosBilheteria
	                                ,l.ComoChegarInternet
	                                ,l.Complemento
	                                ,l.ContratoID
	                                ,l.PaisID
	                                ,l.ImagemInternet
	                                ,l.CodigoPraca
	                                ,l.Latitude
	                                ,l.Longitude
	                                ,l.Alvara
	                                ,l.AVCB
	                                ,l.FonteImposto
	                                ,l.PorcentagemImposto
	                                ,l.DataEmissaoAlvara
	                                ,l.DataValidadeAlvara
	                                ,l.DataEmissaoAvcb
	                                ,l.DataValidadeAvcb
	                                ,l.Lotacao									
                                FROM 
	                                (SELECT ID, PrecoID FROM tIngresso WHERE VendaBilheteriaID = @VendaBilheteriaID
									UNION 
									SELECT IngressoID as ID, PrecoID FROM tIngressoLog WHERE VendaBilheteriaID = @VendaBilheteriaID) AS INGRE
	                                INNER JOIN tIngresso (NOLOCK) i ON INGRE.id = i.id
                                    INNER JOIN tEvento (NOLOCK) e ON i.EventoID = e.ID
                                    INNER JOIN tApresentacao (NOLOCK) a ON i.ApresentacaoID = a.ID
                                    INNER JOIN tSetor (NOLOCK) s ON i.SetorID = s.ID 
                                    INNER JOIN tPreco (NOLOCK) p ON INGRE.PrecoID = p.ID
                                    INNER JOIN tLocal (NOLOCK) l ON e.LocalID = l.ID
	                              ";
            #endregion

            #region strQuery4
            string strQuery4 = @"SELECT 
	                            i.ID
	                            ,i.ApresentacaoSetorID
	                            ,i.PrecoID
	                            ,i.LugarID
	                            ,i.UsuarioID
	                            ,i.CortesiaID
	                            ,i.BloqueioID
	                            ,i.Codigo
	                            ,i.Status
                                 ,CASE
		                          WHEN (SELECT COUNT(ID) FROM tVendaBilheteria WHERE Status = 'C' AND VendaBilheteriaIDOrigem = i.VendaBilheteriaID) > 0 THEN ''
                                    WHEN i.CodigoBarraCliente <> '' THEN i.CodigoBarraCliente
		                          ELSE i.CodigoBarra
                                 END AS CodigoBarra
	                            ,i.SetorID
	                            ,i.ApresentacaoID
	                            ,i.EventoID
	                            ,i.LocalID
	                            ,i.EmpresaID
	                            ,i.CodigoBarraCliente
	                            ,i.LojaID
	                            ,i.VendaBilheteriaID
	                            ,i.ClienteID
	                            ,i.PacoteID
	                            ,i.PacoteGrupo
	                            ,i.Classificacao
	                            ,i.Grupo
	                            ,i.SessionID
	                            ,i.TimeStampReserva
	                            ,i.CodigoSequencial
	                            ,i.PrecoExclusivoCodigoID
	                            ,i.CodigoImpressao
	                            ,i.CanalID
	                            ,i.AssinaturaClienteID
	                            ,i.SerieID
	                            ,i.CompraGUID
	                            ,i.GerenciamentoIngressosID
	                            ,i.ParceiroMidiaID
	                            ,em.ID
	                            ,em.EventoTipoMidiaID
	                            ,em.Publicado
	                            ,em.EventoID
	                            ,em.Valor
	                            ,etm.ID
	                            ,etm.Chave
	                            ,etm.Nome
	                            ,etm.Instrucao
	                            ,etm.Tipo
                            FROM 
                                tIngresso (NOLOCK) i
                                LEFT JOIN EventoMidia (NOLOCK) em ON em.EventoID = i.EventoID
                                LEFT JOIN EventoTipoMidia (NOLOCK) etm ON em.EventoTipoMidiaID = etm.ID
                            WHERE 
	                            i.ID = @ingressoID";
            #endregion

            #region strCount
            string strCount = @"SELECT COUNT(ID) FROM  (SELECT  vb.ID 
	                            FROM tVendaBilheteria vb (NOLOCK) 
	                            INNER JOIN tIngresso (NOLOCK) I ON vb.id = i.VendaBilheteriaID
	                            WHERE vb.ClienteID in (@clienteID) 

	                            UNION

	                            SELECT vb.ID 
	                            FROM tVendaBilheteria vb (NOLOCK) 
	                            INNER JOIN tIngressoLog (NOLOCK) iL ON vb.id = iL.VendaBilheteriaID
	                            WHERE vb.ClienteID in (@clienteID)  and vb.status = 'C' 
                            ) AS TOTAL";
            #endregion

            result = new List<tVendaBilheteria>();

            var query1 = this.conIngresso.Query<tVendaBilheteria, tEntregaControle, tEntrega, int>(strQuery1, addResult1, new { startRow = startRow, endRow = endRow, clienteID = clienteID });

            foreach (tVendaBilheteria venda in result)
            {
                var query2 = this.conIngresso.Query<tVendaBilheteriaFormaPagamento, tCartao, tValeIngresso, tValeIngressoTipo, int>(strQuery2, addResult2, new { vendaBilheteriaID = venda.ID });
            }

            foreach (tVendaBilheteria venda in result)
            {
                var query3 = this.conIngresso.Query<tIngresso, tEvento, tApresentacao, tSetor, tPreco, tLocal, tIngresso>(strQuery3, addResult3, new { VendaBilheteriaID = venda.ID });
                foreach (var ingresso in query3)
                {
                    var query4 = this.conIngresso.Query<tIngresso, EventoMidia, EventoTipoMidia, tIngresso>(strQuery4, addResult4, new { ingressoID = ingresso.ID });
                }
            }

            return result.ToPagedList(pageNumber, pageSize, (int)this.conIngresso.ExecuteScalar(strCount, new { clienteID = clienteID }));
        }

        #region addResult ListarCliente
        private int addResult1(tVendaBilheteria vendaBilhereria, tEntregaControle entregaControle, tEntrega entrega)
        {
            tVendaBilheteria resultVenda = result.FirstOrDefault(t => t.ID == vendaBilhereria.ID);

            if (resultVenda == null)
            {
                resultVenda = vendaBilhereria;
                resultVenda.EntregaControle = entregaControle;
                if (entregaControle != null)
                {
                    resultVenda.EntregaControle.Entrega = entrega;
                }

                result.Add(resultVenda);
            }

            return resultVenda.ID;
        }

        private int addResult2(tVendaBilheteriaFormaPagamento vbfp, tCartao cartao, tValeIngresso valeIngresso, tValeIngressoTipo valeIngressoTipo)
        {
            tVendaBilheteria resultVenda = result.FirstOrDefault(t => t.ID == vbfp.VendaBilheteriaID);
            vbfp.tCartao = cartao;
            vbfp.tValeIngresso = valeIngresso;
            if (valeIngresso != null)
            {
                vbfp.tValeIngresso.ValeIngressoTipo = valeIngressoTipo;
            }
            resultVenda.tVendaBilheteriaFormaPagamento.Add(vbfp);
            return vbfp.ID;
        }

        private tIngresso addResult3(tIngresso ingresso, tEvento evento, tApresentacao apresentacao, tSetor setor, tPreco preco, tLocal local)
        {
            tVendaBilheteria resultVenda = result.FirstOrDefault(t => t.ID == ingresso.VendaBilheteriaID);
            evento.tLocal = local;
            ingresso.tEvento = evento;
            ingresso.tApresentacao = apresentacao;
            ingresso.tSetor = setor;
            ingresso.tPreco = preco;
            ingresso.tLocal = local;
            resultVenda.tIngresso.Add(ingresso);
            return ingresso;
        }

        private tIngresso addResult4(tIngresso ingresso, EventoMidia eventoMidia, EventoTipoMidia eventoTipoMidia)
        {
            tVendaBilheteria resultVenda = result.FirstOrDefault(t => t.ID == ingresso.VendaBilheteriaID);
            if (resultVenda != null)
            {
                var ingresso2 = resultVenda.tIngresso.FirstOrDefault(t => t.ID == ingresso.ID);
                if (eventoMidia != null)
                {
                    eventoMidia.EventoTipoMidia = eventoTipoMidia;
                    ingresso2.tEvento.EventoMidia.Add(eventoMidia);
                }
            }
            return ingresso;
        }
        #endregion

        public IPagedList<MeuIngresso> GetMeusIngressos(int pageNumber, int pageSize, int clienteID, int canalID)
        {
            var startRow = ((pageNumber - 1) * pageSize);
            var endRow = (pageNumber * pageSize);

            var configGerenciador = new ConfigGerenciador();
            var canaisIdList = configGerenciador.getCanaisMobileID().CommaSeparatedStringToIntList();
            var canalMobile = canaisIdList.Contains(canalID) ? 1 : 0;
            var entregaControleIDMobileTicket = configGerenciador.getEntregaControleIDMobileTicket();

            var siteIR = conSite.Database;

            var sqlMeusIngressos =
@"SELECT
	--get VENDA
	vb.ID                                                                   AS ID
	,vb.Senha                                                               AS SenhaVenda
	,vb.CalcDataVenda                                                       AS DataVenda
	,vb.Status                                                              AS StatusVenda
    ,cancel.StatusCancel                                                    AS StatusDevolucaoPendenteCodigo
	,CASE
	   WHEN (SELECT TOP 1 ID
			 FROM tVendaBilheteria( NOLOCK )
			 WHERE Status = 'C' AND VendaBilheteriaIDOrigem = vb.ID) IS NULL
		  THEN 'N'
	   ELSE 'S'
	END																		AS StatusCancelamentoCodigo
    ,CASE
	    WHEN en.PermitirImpressaoInternet IS NULL
		    THEN 'F'
	    ELSE en.PermitirImpressaoInternet
	END																		AS PermissaoImprimir
	,CASE
	    WHEN (DATEDIFF(DAY, dbo.StringToDatetime(vb.DataVenda), CURRENT_TIMESTAMP) >= 7)
		    THEN 'F'
	    ELSE 'T'
	END																		AS PermissaoCancelarData
	,STUFF((SELECT ',' + CAST(Temp.EventoID AS VARCHAR)
		    FROM
			    (SELECT DISTINCT i.EventoID
				FROM tIngressoLog l ( NOLOCK ) 
				INNER JOIN tIngresso i ( NOLOCK ) ON l.IngressoID = i.ID
				WHERE l.VendaBilheteriaID = vb.ID) Temp
		    FOR XML PATH (''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')  AS EventosID
    ,STUFF((SELECT ',' + ev.Nome
		    FROM tEvento ev ( NOLOCK )
		    WHERE ev.ID IN (SELECT DISTINCT i.EventoID
						    FROM tIngressoLog l ( NOLOCK ) 
							INNER JOIN tIngresso i ( NOLOCK ) ON l.IngressoID = i.ID
						    WHERE l.VendaBilheteriaID = vb.ID)
		    FOR XML PATH (''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')  AS EventosNome
    ,ISNULL(vb.VendaBilheteriaIDOrigem, 0)                                  AS VendaBilheteriaIDOrigem
	--get FORMAPAGAMENTO
	,fp.Nome                                                                AS NomeParcelas
	,fp.Parcelas
	,vb.ValorTotal
	,(SELECT 
		CASE
			WHEN SUM(vbfp.Valor) > 0 THEN SUM(vbfp.Valor) 
			ELSE 0 
		END 
		FROM tVendaBilheteriaFormaPagamento (NOLOCK) vbfp  
		WHERE vbfp.VendaBilheteriaID = vb.ID AND vbfp.ValeIngressoID > 0
	)                                                                       AS ValorDesconto
	,vb.TaxaEntregaValor                                                    AS ValorTaxaEntrega
	,vb.TaxaConvenienciaValorTotal                                          AS ValorTaxaConveniencia
	,vb.ValorTotal - vb.TaxaEntregaValor - vb.TaxaConvenienciaValorTotal    AS ValorIngressos
	--get CARTAO
	,ca.NomeCartao
	,ca.NroCartao                                                           AS NumeroCartao
	--get ENTREGA
	,en.ID
	,en.Tipo
	,en.Nome
	,en.PrazoEntrega
	,en.ProcedimentoEntrega
	,en.PermitirImpressaoInternet
	,en.DiasTriagem
	,CASE
		WHEN vbe.StatusTexto <> 'NULL' OR vbe.StatusTexto <> NULL THEN NULL
		ELSE vbe.CodigoRastreamento 
	END                                                                     AS StatusSedex
	,CASE
		WHEN vbe.StatusTexto = 'NULL' OR vbe.StatusTexto = NULL THEN NULL
		ELSE vbe.StatusTexto
	END                                                                     AS StatusMensageiro
    --get LOCAL
	,l.*
	--get PONTOVENDA
	,pv.*
	--get CLIENTEENDERECO
	,ce.*
	--get INGRESSOS
	,i.ID                                                                   AS ID
	,CASE
		WHEN (SELECT COUNT(ID) FROM tEventoEntregaControle (NOLOCK) WHERE EventoID = i.EventoID AND EntregaControleID NOT IN (@entregaControleIDMobileTicket) ) > 0 AND (@canalMobile = 1)
		  THEN ''
		ELSE i.CodigoBarra
	END																		AS CodigoBarra
	,a.Horario                                                              AS ApresentacaoDataHora
	,a.ID                                                                   AS ApresentacaoID
	,i.Classificacao                                                        AS Classificacao
	,i.ClienteID                                                            AS ClienteID
	,i.Codigo                                                               AS Codigo
	,a.CodigoProgramacao                                                    AS CodigoProgramacao
	,ev.Nome                                                                AS Evento
	,ev.ID                                                                  AS EventoID
	,ev.FilmeID                                                             AS FilmeID
	,i.GerenciamentoIngressosID                                             AS GerenciamentoIngressosID
	,i.Grupo                                                                AS Grupo
	,i.ID                                                                   AS IngressoID
	,l.Nome                                                                 AS Local
	,i.LocalID                                                              AS LocalID
	,i.LugarID                                                              AS LugarID
	,i.PacoteGrupo                                                          AS PacoteGrupo
	,i.PacoteID                                                             AS PacoteID
	,i.PrecoExclusivoCodigoID                                               AS PrecoExclusivoCodigoID
	,i.PrecoID                                                              AS PrecoID
	,p.Nome                                                                 AS PrecoNome
	,p.Valor                                                                AS PrecoValor
	,i.SerieID                                                              AS SerieID
	,i.SessionID                                                            AS SessionID
	,s.Nome                                                                 AS Setor
	,i.SetorID                                                              AS SetorID
	,i.Status                                                               AS Status
	,i.TimeStampReserva                                                     AS TimeStampReserva
	,i.VendaBilheteriaID                                                    AS VendaBilheteriaID
	,CASE
		WHEN a.CalcHorario > DATEADD(HOUR, 3, GETDATE()) THEN 1
		ELSE 0
	END                                                                     AS IngressoValidado
	,CASE
        WHEN LEN(a.Alvara) > 0 THEN a.Alvara
		WHEN LEN(ev.Alvara) > 0 THEN ev.Alvara
		ELSE lc.Alvara
    END																		AS Alvara
	,CASE
		WHEN LEN(a.AVCB) > 0 THEN a.AVCB
		WHEN LEN(ev.AVCB) > 0 THEN ev.AVCB
		ELSE lc.AVCB
	END																		AS AVCB
	,lu.Codigo																AS LugarCodigo
    ,ep.Documento																AS Documento
	,ep.RazaoSocial															AS RazaoSocial
FROM 
(
	SELECT TOP (@endRow) ID, Status, CalcHorario, RowNum, TypeDate
	FROM (
		SELECT ID, CalcHorario, ROW_NUMBER() OVER (ORDER BY TypeDate, RowNum, ID) AS RowNum, TypeDate, Status 
		FROM (
			--Guardando todas vendas com apresentações futuras e ordenando por data crescente.
			SELECT id, MAX(CalcHorario) CalcHorario, ROW_NUMBER() OVER(ORDER BY TypeDate, MAX(CalcHorario)) AS RowNum, TypeDate, Status
			FROM (
				SELECT ID, MAX(CalcHorario) CalcHorario, Status, TypeDate 
				FROM (
					SELECT vb.id, MAX(a.CalcHorario) CalcHorario, CASE WHEN CONVERT( DATE, a.CalcHorario) > = CONVERT(DATE, GETDATE()) THEN 0 ELSE 1 END AS TypeDate, vb.Status
					FROM tVendaBilheteria vb(NOLOCK)
						LEFT JOIN tIngressoLog(NOLOCK) ingl ON vb.id = ingl.VendaBilheteriaID
						LEFT JOIN tIngresso(NOLOCK) ing ON vb.id = ing.VendaBilheteriaID
						LEFT JOIN tIngresso(NOLOCK) i ON i.ID = ingl.IngressoID OR i.ID = ing.ID
						LEFT JOIN tApresentacao(NOLOCK) a ON a.id = i.ApresentacaoID
					WHERE vb.ClienteID = @clienteId AND vb.Status = 'P'
					GROUP BY vb.id, a.CalcHorario, vb.Status
					) AS a
				GROUP BY ID, Status, TypeDate
				) AS TempOrder
			WHERE TempOrder.TypeDate = 0
			GROUP BY TempOrder.ID, TempOrder.CalcHorario, TempOrder.TypeDate, TempOrder.Status
			
			UNION

			--Guardando todas vendas com apresentações passadas ordenando por data decrescente.
			SELECT id, MAX(CalcHorario) CalcHorario, ROW_NUMBER() OVER(ORDER BY TypeDate, MAX(CalcHorario) DESC) AS RowNum, TypeDate, Status
			FROM (
				SELECT ID, MAX(CalcHorario) CalcHorario, Status, TypeDate 
FROM (
					SELECT vb.id, MAX(a.CalcHorario) CalcHorario, CASE WHEN CONVERT( DATE, a.CalcHorario) > = CONVERT(DATE, GETDATE()) THEN 0 ELSE 1 END AS TypeDate, vb.Status
					FROM tVendaBilheteria vb(NOLOCK)
						LEFT JOIN tIngressoLog(NOLOCK) ingl ON vb.id = ingl.VendaBilheteriaID
						LEFT JOIN tIngresso(NOLOCK) ing ON vb.id = ing.VendaBilheteriaID
						LEFT JOIN tIngresso(NOLOCK) i ON i.ID = ingl.IngressoID OR i.ID = ing.ID
						LEFT JOIN tApresentacao(NOLOCK) a ON a.id = i.ApresentacaoID
					WHERE vb.ClienteID = @clienteId AND vb.Status = 'P'
					GROUP BY vb.id, a.CalcHorario, vb.Status
					) AS A
				GROUP BY ID, Status, TypeDate
				) AS TempOrder
			WHERE TempOrder.TypeDate = 1
			GROUP BY TempOrder.ID, TempOrder.CalcHorario, TempOrder.TypeDate, TempOrder.Status
			) AS B
	) AS C
	WHERE RowNum >= @startRow

	UNION 

	SELECT vb.ID, vb.Status, NULL AS CalcHorario, NULL AS RowNum, 2 AS TypeDate
	FROM tVendaBilheteria (NOLOCK) vb
	WHERE vb.ClienteID = @clienteId AND vb.VendaBilheteriaIDOrigem > 0

    ) AS busca
	INNER JOIN  tVendaBilheteria (NOLOCK) vb ON busca.ID = vb.ID

    LEFT JOIN (
				SELECT max(ID) AS ID,
					   VendaBilheteriaIDVenda
				FROM tCancelDevolucaoPendente ( NOLOCK )
				GROUP BY VendaBilheteriaIDVenda
                ) AS cn ON cn.VendaBilheteriaIDVenda = vb.ID
	LEFT JOIN tCancelDevolucaoPendente ( NOLOCK ) AS cancel
	ON cancel.ID = cn.ID AND cancel.StatusCancel IN ('A', 'P', 'D')

	LEFT JOIN	tIngresso (NOLOCK) INGRE ON INGRE.VendaBilheteriaID IN (busca.ID)
	LEFT JOIN	tIngressoLog (NOLOCK) INGRELOG ON busca.Status = 'C' AND INGRELOG.VendaBilheteriaID = busca.ID
	LEFT JOIN	tIngresso (NOLOCK) i ON i.ID = INGRELOG.IngressoID OR i.ID = INGRE.ID 

	LEFT JOIN   tEvento (NOLOCK) ev ON i.EventoID = ev.ID
	LEFT JOIN   tApresentacao (NOLOCK) a ON i.ApresentacaoID = a.ID
	LEFT JOIN   tPreco (NOLOCK) p ON i.PrecoID = p.ID
	LEFT JOIN   tSetor (NOLOCK) s ON i.SetorID = s.ID
	LEFT JOIN	tLugar (NOLOCK) lu ON i.LugarID = lu.ID
	LEFT JOIN	tLocal (NOLOCK) lc ON i.LocalID = lc.ID
	LEFT JOIN	tEmpresa (NOLOCK) ep ON ep.ID = lc.EmpresaID

	LEFT JOIN	tEntregaControle (NOLOCK) ec ON vb.EntregaControleID = ec.ID
	LEFT JOIN	tEntrega (NOLOCK) en ON ec.EntregaID = en.ID

	LEFT JOIN   tVendaBilheteriaEntrega (NOLOCK) vbe ON vbe.ID = (SELECT TOP 1 MAX(vbe2.ID) FROM tVendaBilheteriaEntrega (NOLOCK) vbe2 WHERE vbe2.VendaBilheteriaID = vb.ID)

	LEFT JOIN   tVendaBilheteriaFormaPagamento (NOLOCK) vbfp  ON vbfp.VendaBilheteriaID = vb.ID
	LEFT JOIN   tCartao (NOLOCK) ca ON vbfp.CartaoID = ca.ID
	LEFT JOIN   tFormaPagamento (NOLOCK) fp ON vbfp.FormaPagamentoID = fp.ID

	LEFT JOIN   {{siteIR}}.dbo.Local (NOLOCK) l ON i.LocalID = l.IR_LocalID 
	LEFT JOIN   {{siteIR}}.dbo.PontoVenda (NOLOCK) pv ON vb.PdvID = pv.IR_PontoVendaID
	LEFT JOIN   tClienteEndereco (NOLOCK) ce ON vb.ClienteEnderecoID = ce.ID
ORDER BY TypeDate, RowNum";

            sqlMeusIngressos = sqlMeusIngressos.Replace("{{siteIR}}", siteIR);

            meusIngressos = new List<MeuIngresso>();
            conIngresso.Query<MeuIngresso, MeuIngressoPagamento, MeuIngressoEntrega, Local, PontoVenda, tClienteEndereco, Carrinho, MeuIngresso>(sqlMeusIngressos, GetMeusIngressosMap, new { entregaControleIDMobileTicket, canalMobile, endRow, startRow, clienteID }, splitOn: "NomeParcelas, ID, ID, ID, ID, ID");

            var sqlCountCompras = "SELECT COUNT(tVendaBilheteria.ID) FROM tVendaBilheteria (NOLOCK) WHERE tVendaBilheteria.ClienteID = @clienteID";
            var countCompras = (int)conIngresso.ExecuteScalar(sqlCountCompras, new { clienteID });

            return meusIngressos.ToPagedList(pageNumber, pageSize, countCompras);
        }

        private MeuIngresso GetMeusIngressosMap(MeuIngresso newMeuIngresso, MeuIngressoPagamento pagamento, MeuIngressoEntrega entrega, Local localEvento, PontoVenda pontoVenda, tClienteEndereco clienteEndereco, Carrinho carrinho)
        {
            var meuingresso = this.meusIngressos.FirstOrDefault(v => v.ID == newMeuIngresso.ID);

            if (meuingresso != null)
            {
                carrinho.LocalEvento = localEvento;
                meuingresso.Carrinho.Add(carrinho);
                return null;
            }

            newMeuIngresso.Pagamento = pagamento;
            if (clienteEndereco != null)
            {
                entrega.LocalEvento = (entrega.TipoAsEnum == enumEntregaTipo.retiradaBilheteria) ? localEvento : null;
                entrega.LocalPDV = pontoVenda;
                entrega.LocalClienteEndereco = clienteEndereco;
                newMeuIngresso.Entrega = entrega;
            }

            if (newMeuIngresso.StatusVendaAsEnum == enumVendaBilheteriaStatus.cancelado && newMeuIngresso.VendaBilheteriaIDOrigem > 0)
            {
                meuingresso = this.meusIngressos.FirstOrDefault(v => v.ID == newMeuIngresso.VendaBilheteriaIDOrigem);
                if (meuingresso != null)
                {
                    meuingresso.Cancelada = newMeuIngresso;
                    return null;
                }
            }

            carrinho.LocalEvento = localEvento;
            newMeuIngresso.Carrinho = new List<Carrinho> { carrinho };
            this.meusIngressos.Add(newMeuIngresso);
            return newMeuIngresso;
        }

        public List<AgregadoModel> ConsultarAgregados(int vendaBilheteriaID)
        {
            #region strQuery
            string strQuery = @"SELECT 
	                                ID
                                    ,VendaBilheteriaId
                                    ,Nome
                                    ,CPF
                                    ,Email
                                    ,UsuarioId
                                    ,Data
                                    ,Telefone
                                FROM
	                                tVendaBilheteriaAgregados
                                WHERE
	                                VendaBilheteriaId = @vendaBilheteriaID";
            #endregion

            var result = conIngresso.Query<AgregadoModel>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();

            return result;
        }

        public String PutAgregado(int vendabilheteriaID, string nome, string cpf, string email, string telefone)
        {
            #region strQuery
            string strQuery = @"UPDATE tVendaBilheteria SET  
                                    AgregadoNome = @nome
                                    ,AgregadoCPF = @cpf
                                    ,AgregadoEmail = @email
                                    ,AgregadoTelefone = @telefone
                                WHERE ID = @vendabilheteriaID";
            #endregion

            conIngresso.Execute(strQuery, new { vendabilheteriaID = vendabilheteriaID, nome = nome, cpf = cpf, email = email, telefone = telefone });

            return "OK";
        }

        public string DeleteAgregado(int vendabilheteriaID)
        {
            string strQuery = @"UPDATE tVendaBilheteria SET 
                                    AgregadoNome = NULL
                                    ,AgregadoCPF = NULL
                                    ,AgregadoEmail = NULL
                                    ,AgregadoTelefone = NULL
                                WHERE ID = @vendabilheteriaID";

            conIngresso.Execute(strQuery, new { vendabilheteriaID = vendabilheteriaID });

            return "OK";
        }

        public IPagedList<SenhaCompraModel> ConsultarSenhasCompra(int clienteID, int pageNumber, int pageSize)
        {
            int startRow = ((pageNumber - 1) * pageSize);
            int endRow = (pageNumber * pageSize);

            #region strQuery
            var strQuery = @"
SELECT
	                                vb.ID
	                                ,vb.Senha
	                                ,dbo.StringToDatetime(vb.Datavenda) AS DataCompra
	                                ,ci.ID AS CanalID
	                                ,ci.Nome AS CanalNome
	                                ,li.ID AS LojaID
	                                ,li.Nome AS LojaNome
	                                ,vb.ValorTotal
	                                ,vb.Status AS StatusCodigo
                                    ,cancel.StatusCancel as StatusDevolucaoPendenteCodigo
	                                ,CASE
		                                WHEN (SELECT TOP 1 ID FROM tVendaBilheteria(NOLOCK) WHERE Status = 'C' and VendaBilheteriaIDOrigem = vb.ID) IS NULL
			                                THEN 'N'
			                                ELSE 'S'
		                                END AS StatusCancelamentoCodigo
	                                ,CASE
		                                WHEN e.PermitirImpressaoInternet IS NULL
			                                THEN 'F'
			                                ELSE e.PermitirImpressaoInternet
		                                END AS PermissaoImprimir
                                    ,CASE
		                                WHEN (DATEDIFF(day, dbo.StringToDatetime(vb.DataVenda), CURRENT_TIMESTAMP) >= 7)
			                                THEN 'F'
			                                ELSE 'T'
		                                END AS PermissaoCancelarData
	                                ,STUFF((SELECT ',' + CAST(Temp.EventoID AS VARCHAR) FROM
                                            (SELECT DISTINCT i.EventoID FROM tIngressoLog l (NOLOCK) INNER JOIN tIngresso i (NOLOCK) ON l.IngressoID = i.ID WHERE l.VendaBilheteriaID = vb.ID) Temp
		                                    FOR XML PATH(''), TYPE).value('.','VARCHAR(MAX)') ,1,1, '') AS EventosID
                                FROM
	                                (
                                    SELECT TOP @endRow vb2.ID,ROW_NUMBER() OVER (ORDER BY vb2.ID desc) AS RowNum
	                                FROM tVendaBilheteria (NOLOCK) AS vb2
	                                WHERE vb2.Status IN ('V','P') AND vb2.clienteID = @clienteID
	                                ) AS temp
	                                INNER JOIN tVendaBilheteria (NOLOCK) AS vb on vb.ID = temp.ID
                                    LEFT JOIN (
                                        SELECT max(ID) as ID, VendaBilheteriaIDVenda FROM tCancelDevolucaoPendente (NOLOCK) GROUP BY VendaBilheteriaIDVenda
                                    ) as cn on cn.VendaBilheteriaIDVenda = vb.ID
	                                LEFT JOIN tCancelDevolucaoPendente (NOLOCK) as cancel on cancel.ID = cn.ID and cancel.StatusCancel in ('A','P','D')
	                                LEFT JOIN tEntregaControle (NOLOCK) AS ec ON ec.ID = vb.EntregaControleID
	                                LEFT JOIN tEntrega (NOLOCK) AS e ON e.ID = ec.EntregaID
	                                
                                    LEFT JOIN tCaixa (NOLOCK) AS cx ON cx.ID = vb.CaixaID
	                                LEFT JOIN tLoja (NOLOCK) AS li ON li.ID = cx.LojaID
	                                LEFT JOIN tCanal (NOLOCK) AS ci ON ci.ID = li.CanalID
                                WHERE
	                                temp.RowNum > @startRow
                                GROUP BY
	                                vb.ID
	                                ,vb.Senha
	                                ,vb.DataVenda
	                                ,ci.ID
	                                ,ci.Nome
	                                ,li.ID
	                                ,li.Nome
	                                ,vb.ValorTotal
	                                ,vb.Status
                                    ,cancel.StatusCancel
	                                ,e.PermitirImpressaoInternet
                                ORDER BY vb.DataVenda DESC

                            ".Replace("@startRow", startRow.ToString()).Replace("@endRow", endRow.ToString());
            #endregion

            #region strCount
            string strCount = @"SELECT
			                        COUNT(DISTINCT VB.ID)
		                        FROM
	                                tVendaBilheteria VB (NOLOCK)
                                WHERE
			                        VB.Status IN ('V','P') 
			                        AND VB.clienteID = @clienteID";
            #endregion

            var result = this.conIngresso.Query<SenhaCompraModel>(strQuery, new { clienteID = clienteID }).ToList();

            return result.ToPagedList(pageNumber, pageSize, (int)this.conIngresso.ExecuteScalar(strCount, new { clienteID = clienteID }));
        }

        public SenhaCompraDetalhe ConsultarSenhasCompraDetalhe(int vendaBilheteriaID)
        {
            #region senhaCompraDetalhe
            string strQuery = @"SELECT 
	                                tVendaBilheteria.ID AS 'ID' 
	                                ,tVendaBilheteria.Senha AS 'Senha'
	                                ,dbo.StringToDatetime(tVendaBilheteria.DataVenda) AS 'DataCompra' 
	                                ,tVendaBilheteria.Status AS 'StatusCodigo' 
	                                ,CASE 
		                                WHEN VBC.ID IS NULL
			                                THEN 'N'
			                                ELSE 'S'
		                                END AS 'StatusCancelamentoCodigo'
	                                ,tEntrega.ID AS 'EntregaID' 
	                                ,tEntrega.Nome AS 'EntregaNome' 
	                                ,tEntregaControle.ID AS 'EntregaControleID' 
	                                ,tVendaBilheteria.CaixaID AS 'CaixaID'
	                                ,tVendaBilheteria.Obs AS 'Obs'
	                                ,tVendaBilheteria.NotaFiscalCliente AS 'NotaFiscalCliente'
	                                ,tVendaBilheteria.NotaFiscalEstabelecimento AS 'NotaFiscalEstabelecimento'
	                                ,tVendaBilheteria.ModalidadePagamentoCodigo AS 'ModalidadePagamentoCodigo'
	                                ,tVendaBilheteria.ModalidadePagamentoTexto AS 'ModalidadePagamentoTexto'
	                                ,tVendaBilheteria.QuantidadeImpressoesInternet AS 'QuantidadeImpressoesInternet'
	                                ,tVendaBilheteria.PdvID AS 'PdvID'
	                                ,CASE 
		                                WHEN tVendaBilheteria.PagamentoProcessado = 'T' 
			                                THEN 1 
			                                ELSE 0 
		                                END AS 'PagamentoProcessado'
	                                ,tVendaBilheteria.ValorSeguro AS 'ValorSeguro'
	                                ,tVendaBilheteria.Score AS 'Score'
	                                ,tVendaBilheteria.ClienteEnderecoID AS 'ClienteEnderecoID'
                                FROM 
	                                tVendaBilheteria (NOLOCK)
	                                LEFT JOIN tEntregaControle (NOLOCK) ON tVendaBilheteria.EntregaControleID = tEntregaControle.ID 
	                                LEFT JOIN tEntrega (NOLOCK) ON tEntregaControle.EntregaID = tEntrega.ID 
                                    LEFT JOIN tVendaBilheteria VBC (NOLOCK) ON (SELECT TOP 1 Cancelado.ID FROM tVendaBilheteria Cancelado (NOLOCK) WHERE Cancelado.VendaBilheteriaIDOrigem = tVendaBilheteria.ID) = VBC.ID
                                WHERE
	                                tVendaBilheteria.ID = @vendaBilheteriaID AND tVendaBilheteria.Status != 'C'";

            SenhaCompraDetalhe senhaCompraDetalhe = conIngresso.Query<SenhaCompraDetalhe>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
            #endregion

            if (senhaCompraDetalhe != null)
            {
                #region valeIngresso
                strQuery = @"SELECT
	                            tValeIngresso.ID AS 'ValeIngressoID'
	                            ,tValeIngresso.CodigoTroca AS 'ValeIngressoCodigoTroca'
	                            ,tValeIngresso.CodigoBarra AS 'ValeIngressoCodigoBarra'
                            FROM 
	                            tValeIngresso (NOLOCK)
                            WHERE
	                            tValeIngresso.VendaBilheteriaID = @vendaBilheteriaID";

                senhaCompraDetalhe.ValeIngresso = conIngresso.Query<ValeIngresso>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region voucher
                strQuery = @"SELECT TOP 1
	                            Voucher.ID
	                            ,Voucher.Codigo
                            FROM 
	                            Voucher (NOLOCK)
                            WHERE 
	                            Voucher.VendaBilheteriaID = @vendaBilheteriaID";

                VoucherModel voucher = conIngresso.Query<VoucherModel>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
                if (voucher != null)
                {
                    senhaCompraDetalhe.VoucherID = voucher.ID;
                    senhaCompraDetalhe.VoucherCodigo = voucher.Codigo;
                }
                #endregion

                #region agregado
                strQuery = @"SELECT TOP 1
	                            AgregadoNome AS Nome
                                ,AgregadoCPF AS CPF
                                ,AgregadoEmail AS Email
                                ,AgregadoTelefone AS Telefone
                            FROM 
	                            tVendaBilheteria (NOLOCK)
                            WHERE 
	                            ID = @vendaBilheteriaID";

                senhaCompraDetalhe.Agredado = conIngresso.Query<AgregadoModel>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
                #endregion

                #region senhasCancelamento
                strQuery = @"SELECT 
	                            tVendaBilheteria.Senha
                            FROM
	                            tVendaBilheteria (NOLOCK)
                            WHERE
	                            tVendaBilheteria.VendaBilheteriaIDOrigem = @vendaBilheteriaID";

                senhaCompraDetalhe.SenhasCancelamento = conIngresso.Query<string>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region pagamentos
                strQuery = @"SELECT 
	                            tFormaPagamento.ID
	                            ,tFormaPagamento.Nome AS 'FormaPagamentoNome'
	                            ,tFormaPagamento.Parcelas
	                            ,tVendaBilheteriaFormaPagamento.Valor
                            FROM 
	                            tVendaBilheteriaFormaPagamento (NOLOCK) 
	                            LEFT JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.ID
                            WHERE
	                            tVendaBilheteriaFormaPagamento.VendaBilheteriaID = @vendaBilheteriaID";

                senhaCompraDetalhe.Pagamentos = conIngresso.Query<Pagamento>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region ingresssos
                strQuery = @"SELECT
	                            Ing.ID AS 'ID'
	                            ,Ing.Codigo AS 'Codigo'
	                            ,Lug.ID AS 'LugarID'
	                            ,Pre.ID AS 'PrecoID'
	                            ,Eve.ID AS 'EventoID'
	                            ,Eve.Nome AS 'EventoNome'
	                            ,Loc.ID AS 'LocalID'
	                            ,Loc.Nome AS 'LocalNome'
	                            ,Pre.Nome AS 'PrecoNome'
	                            ,Pre.Valor AS 'PrecoValor'
	                            ,dbo.StringToDatetime(Ing.TimeStampReserva) AS 'TimeStampReserva'
	                            ,VBI.TaxaConveniencia AS 'TaxaConveniencia'
	                            ,Ing.PacoteGrupo AS 'PacoteGrupo'
	                            ,Pac.ID AS 'PacoteID'
	                            ,Sto.ID AS 'SetorID'
	                            ,Sto.Nome As 'SetorNome'
	                            ,Apre.ID AS 'ApresentacaoID'
	                            ,Apre.CalcHorario AS 'ApresentacaoHorario'
                                ,CASE 
		                            WHEN Cancelado.Acao = 'C' 
		                                THEN 'S'
		                                ELSE 'N'
	                                END AS 'StatusCancelamentoCodigo'
                                ,Cancelado.Senha AS 'SenhaCancelamento'
                            FROM
	                            tIngressoLog Inl (NOLOCK)
	                            INNER JOIN tIngresso Ing (NOLOCK) ON Inl.IngressoID = Ing.ID
	                            LEFT JOIN tLugar Lug (NOLOCK) ON Ing.LugarID = Lug.ID
	                            LEFT JOIN tSetor Sto (NOLOCK) ON Ing.SetorID = Sto.ID
	                            LEFT JOIN tEvento Eve (NOLOCK) ON Ing.EventoID = Eve.ID 
	                            LEFT JOIN tLocal Loc (NOLOCK) ON Ing.LocalID = Loc.ID
	                            LEFT JOIN tPreco Pre (NOLOCK) ON Inl.PrecoID = Pre.ID
	                            LEFT JOIN tApresentacao Apre (NOLOCK) ON Ing.ApresentacaoID = Apre.ID
	                            LEFT JOIN tVendaBilheteriaItem VBI (NOLOCK) ON Inl.VendaBilheteriaID = VBI.VendaBilheteriaID AND Inl.VendaBilheteriaItemID = VBI.ID
	                            LEFT JOIN tPacote Pac (NOLOCK) ON Ing.PacoteID = Pac.ID
	                            LEFT JOIN (SELECT 
					                            tIngressoLog.IngressoID
					                            ,tVendaBIlheteriaItem.Acao
					                            ,tVendaBilheteria.Senha
				                            FROM 
					                            tIngressoLog (NOLOCK) 
					                            LEFT JOIN tVendaBIlheteriaItem (NOLOCK) ON tIngressoLog.VendaBilheteriaItemID = tVendaBIlheteriaItem.ID
					                            LEFT JOIN tVendaBilheteria (NOLOCK) ON tIngressoLog.VendaBilheteriaID = tVendaBilheteria.ID
				                            WHERE 
					                            tIngressoLog.VendaBilheteriaID IN (SELECT 
															                            ID 
														                            FROM 
															                            tVendaBilheteria (NOLOCK) 
														                            WHERE 
															                            tVendaBilheteria.VendaBilheteriaIDOrigem = @vendaBilheteriaID)) Cancelado ON Inl.IngressoID = Cancelado.IngressoID
                            WHERE
	                            Inl.VendaBilheteriaID = @vendaBilheteriaID AND Inl.Acao = 'V'";

                senhaCompraDetalhe.Ingressos = conIngresso.Query<Ingresso>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region total
                strQuery = @"SELECT
	                            VB.ValorTotal AS 'ValorTotal'
	                            ,SUM(CASE WHEN VBFD.Valor IS NULL THEN 0 ELSE VBFD.Valor END) AS 'ValorDesconto'
	                            ,VB.TaxaEntregaValor AS 'ValorTaxaEntrega'
	                            ,VB.TaxaConvenienciaValorTotal AS 'ValorTaxaConveniencia'
	                            ,((((VB.ValorTotal + SUM(CASE WHEN VBFD.Valor IS NULL THEN 0 ELSE VBFD.Valor END)) - VB.TaxaEntregaValor) - VB.TaxaConvenienciaValorTotal) - VB.ValorSeguro) AS 'ValorIngressos'
                            FROM 
	                            tVendaBilheteria VB (NOLOCK)
	                            LEFT JOIN tVendaBilheteriaFormaPagamento VBFD (NOLOCK) ON VB.ID = VBFD.VendaBilheteriaID AND VBFD.ValeIngressoID > 0
                            WHERE
	                            VB.ID = @vendaBilheteriaID
                            GROUP BY
	                            VB.ValorTotal
	                            ,VB.TaxaEntregaValor
	                            ,VB.TaxaConvenienciaValorTotal
                                ,VB.ValorSeguro";

                senhaCompraDetalhe.Total = conIngresso.Query<CompraTotal>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
                #endregion
            }

            return senhaCompraDetalhe;
        }

        public SenhaCompraDetalheCancelamento ConsultarSenhasCompraDetalheCancelamento(int vendaBilheteriaID, int canalID)
        {
            #region senhaCompraDetalheCancelamento
            string strQuery = @"SELECT 
	                                VBV.ID AS 'CompraID'
	                                ,VBV.Senha AS 'SenhaCompra'
	                                ,VBV.CalcDataVenda AS 'DataCompra'
	                                ,VBV.CaixaID AS 'CaixaID'
	                                ,VBV.ValorSeguro AS 'ValorSeguro'
	                                ,MAX(CASE WHEN VBC.TaxaConvenienciaValorTotal > 0 THEN 1 ELSE 0 END) AS 'TaxaConvencienciaCancelada'
	                                ,MAX(CASE WHEN VBC.TaxaEntregaValor > 0 THEN 1 ELSE 0 END) AS 'TaxaEntregaCancelada'
	                                ,MAX(CASE WHEN VBC.ValorSeguro > 0 THEN 1 ELSE 0 END) AS 'ValorSeguroCancelado'
                                    ,MAX(CASE WHEN VBC.TaxaConvenienciaValorTotal > 0 THEN VBC.Senha ELSE NULL END) AS 'SenhaTaxaConvencienciaCancelada'
	                                ,MAX(CASE WHEN VBC.TaxaEntregaValor > 0 THEN VBC.Senha ELSE NULL END) AS 'SenhaTaxaEntregaCancelada'
	                                ,MAX(CASE WHEN VBC.ValorSeguro > 0 THEN VBC.Senha ELSE NULL END) AS 'SenhaValorSeguroCancelado'
                                FROM 
	                                tVendaBilheteria VBV (NOLOCK)
	                                LEFT JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
                                WHERE
	                                VBV.ID = @vendaBilheteriaID
                                GROUP BY
	                                VBV.ID
	                                ,VBV.Senha
	                                ,VBV.CalcDataVenda
	                                ,VBV.CaixaID
	                                ,VBV.ValorSeguro";

            SenhaCompraDetalheCancelamento senhaCompraDetalheCancelamento = conIngresso.Query<SenhaCompraDetalheCancelamento>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
            #endregion

            if (senhaCompraDetalheCancelamento != null)
            {
                #region ingressos
                strQuery = @"SELECT
	                            Ing.ID AS 'ID'
	                            ,Ing.Codigo AS 'Codigo'
	                            ,Lug.ID AS 'LugarID'
	                            ,Pre.ID AS 'PrecoID'
	                            ,Eve.ID AS 'EventoID'
	                            ,Eve.Nome AS 'EventoNome'
	                            ,Loc.ID AS 'LocalID'
	                            ,Loc.Nome AS 'LocalNome'
	                            ,Pre.Nome AS 'PrecoNome'
	                            ,Pre.Valor AS 'PrecoValor'
	                            ,dbo.StringToDatetime(Ing.TimeStampReserva) AS 'TimeStampReserva'
	                            ,VBI.TaxaConvenienciaValor AS 'TaxaConveniencia'
	                            ,Ing.PacoteGrupo AS 'PacoteGrupo'
	                            ,Pac.ID AS 'PacoteID'
                                ,CASE
                                    WHEN Pac.PermitirCancelamentoAvulso = 'T'
                                        THEN 1
                                        ELSE 0 
                                    END AS 'PacotePermitirCancelamentoAvulso'
                                ,Pac.Nome AS 'PacoteNome'
	                            ,Sto.ID AS 'SetorID'
	                            ,Sto.Nome As 'SetorNome'
	                            ,Apre.ID AS 'ApresentacaoID'
	                            ,Apre.CalcHorario AS 'ApresentacaoHorario'
                                ,CASE 
		                            WHEN Cancelado.Acao = 'C' 
			                            THEN 1
			                            ELSE 0
		                            END AS 'Cancelado'
	                            ,Cancelado.StatusCancel AS 'StatusCancelamentoCodigo'
                                ,Cancelado.Senha AS 'SenhaCancelamento'
                                ,CASE 
		                            WHEN (SELECT Ing2.ID FROM tIngresso Ing2 (NOLOCK) INNER JOIN tVendaBilheteria VB ON Ing2.VendaBilheteriaID = VB.ID LEFT JOIN tEntregaControle EC ON VB.EntregaControleID = EC.ID WHERE Ing2.ID = Ing.ID AND Ing2.status = 'I' AND EC.ID != 11) IS NULL
			                            THEN 0
			                            ELSE 1
		                            END AS 'TemDevolucao'
                            FROM
	                            tIngressoLog Inl (NOLOCK)
	                            INNER JOIN tIngresso Ing (NOLOCK) ON Inl.IngressoID = Ing.ID
	                            LEFT JOIN tLugar Lug (NOLOCK) ON Ing.LugarID = Lug.ID
	                            LEFT JOIN tSetor Sto (NOLOCK) ON Ing.SetorID = Sto.ID
	                            LEFT JOIN tEvento Eve (NOLOCK) ON Ing.EventoID = Eve.ID 
	                            LEFT JOIN tLocal Loc (NOLOCK) ON Ing.LocalID = Loc.ID
	                            LEFT JOIN tPreco Pre (NOLOCK) ON Inl.PrecoID = Pre.ID
	                            LEFT JOIN tApresentacao Apre (NOLOCK) ON Ing.ApresentacaoID = Apre.ID
	                            LEFT JOIN tVendaBilheteriaItem VBI (NOLOCK) ON Inl.VendaBilheteriaID = VBI.VendaBilheteriaID AND VBI.ID = Inl.VendaBilheteriaItemID
	                            LEFT JOIN tPacote Pac (NOLOCK) ON Ing.PacoteID = Pac.ID
	                            LEFT JOIN (SELECT 
					                            (CASE WHEN tIngressoLog.IngressoID IS NULL THEN CDPI.IngressoID ELSE tIngressoLog.IngressoID END) AS IngressoID
					                            ,CDP.StatusCancel
					                            ,tVendaBIlheteriaItem.Acao
					                            ,tVendaBilheteria.ID
					                            ,tVendaBilheteria.Senha
					                            ,tVendaBilheteria.ValorTotal
					                            ,tVendaBilheteria.ValorSeguro
					                            ,tVendaBilheteria.TaxaConvenienciaValorTotal
					                            ,tVendaBilheteria.TaxaEntregaValor
				                            FROM 
					                            tVendaBilheteria (NOLOCK) 
					                            LEFT JOIN tIngressoLog (NOLOCK) ON tVendaBilheteria.ID = tIngressoLog.VendaBilheteriaID
					                            LEFT JOIN tVendaBIlheteriaItem (NOLOCK) ON tVendaBIlheteriaItem.ID = tIngressoLog.VendaBilheteriaItemID
					                            LEFT JOIN tCancelDevolucaoPendente CDP (NOLOCK) ON tVendaBilheteria.ID = CDP.VendaBilheteriaIDCancel
					                            LEFT JOIN tCancelDevolucaoPendenteIngresso CDPI (NOLOCK) ON CDP.ID = CDPI.CancelDevolucaoPendenteID AND (CDPI.IngressoID = tIngressoLog.IngressoID OR tIngressoLog.IngressoID IS NULL)
				                            WHERE 
                                                (tVendaBilheteria.ValorTotal - tVendaBilheteria.TaxaEntregaValor - tVendaBilheteria.TaxaConvenienciaValorTotal - tVendaBilheteria.ValorSeguro) > 0
                                                AND CDP.StatusCancel <> 'C'
					                            AND tVendaBilheteria.ID IN (SELECT 
												                                ID 
											                                FROM 
												                                tVendaBilheteria (NOLOCK) 
											                                WHERE 
												                                tVendaBilheteria.VendaBilheteriaIDOrigem = @vendaBilheteriaID)) Cancelado ON Inl.IngressoID = Cancelado.IngressoID
                            WHERE
	                            Inl.VendaBilheteriaID = @vendaBilheteriaID AND Inl.Acao = 'V'";

                senhaCompraDetalheCancelamento.Ingressos = conIngresso.Query<IngressoDetalheCancelamento>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region pagamentos
                strQuery = @"SELECT 
	                            FP.ID AS 'ID'
	                            ,FP.Nome AS 'FormaPagamentoNome'
	                            ,FP.Parcelas AS 'Parcelas'
	                            ,VBFP.Valor AS 'Valor'
                                ,FPT.Nome AS 'FormaPagamentoTipo'
                                ,Cli.Nome AS 'NomeTitular'
	                            ,Cli.CPF AS 'CPFTitular'
	                            ,Car.NroCartao AS 'NumeroCartao'
	                            ,Ban.Nome AS 'Bandeira'
                                ,VBFP.TransactionID AS 'TransactionID'
                            FROM 
	                            tVendaBilheteriaFormaPagamento VBFP (NOLOCK) 
	                            LEFT JOIN tFormaPagamento FP (NOLOCK) ON VBFP.FormaPagamentoID = FP.ID
                                LEFT JOIN tFormaPagamentoTipo FPT (NOLOCK) ON FP.FormaPagamentoTipoID = FPT.ID
	                            LEFT JOIN tCartao Car (NOLOCK) ON VBFP.CartaoID = Car.ID
	                            LEFT JOIN tBandeira Ban (NOLOCK) ON Car.BandeiraID = Ban.ID
	                            LEFT JOIN tCliente Cli (NOLOCK) ON Car.ClienteID = Cli.ID
                            WHERE
	                            VBFP.VendaBilheteriaID = @vendaBilheteriaID";

                senhaCompraDetalheCancelamento.Pagamentos = conIngresso.Query<PagamentoDetalheCancelamento>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).ToList();
                #endregion

                #region totalCompra
                strQuery = @"SELECT
	                            VB.ValorTotal AS 'ValorTotal'
	                            ,SUM(CASE WHEN VBFD.Valor IS NULL THEN 0 ELSE VBFD.Valor END) AS 'ValorDesconto'
	                            ,VB.TaxaEntregaValor AS 'ValorTaxaEntrega'
	                            ,VB.TaxaConvenienciaValorTotal AS 'ValorTaxaConveniencia'
	                            ,((((VB.ValorTotal + SUM(CASE WHEN VBFD.Valor IS NULL THEN 0 ELSE VBFD.Valor END)) - VB.TaxaEntregaValor) - VB.TaxaConvenienciaValorTotal) - VB.ValorSeguro) AS 'ValorIngressos'
                            FROM 
	                            tVendaBilheteria VB (NOLOCK)
	                            LEFT JOIN tVendaBilheteriaFormaPagamento VBFD (NOLOCK) ON VB.ID = VBFD.VendaBilheteriaID AND VBFD.ValeIngressoID > 0
                            WHERE
	                            VB.ID = @vendaBilheteriaID
                            GROUP BY
	                            VB.ValorTotal
	                            ,VB.TaxaEntregaValor
	                            ,VB.TaxaConvenienciaValorTotal
                                ,VB.ValorSeguro";

                senhaCompraDetalheCancelamento.TotalCompra = conIngresso.Query<CompraTotal>(strQuery, new { vendaBilheteriaID = vendaBilheteriaID }).FirstOrDefault();
                #endregion

                #region bancos
                strQuery = @"SELECT 
                                Codigo, NomeBanco 
                            FROM ListaBancos (NOLOCK) 
                            WHERE 
                                IRDeposita = 1 
                            ORDER BY NomeBanco";

                senhaCompraDetalheCancelamento.BancosIR = conIngresso.Query<ListaBancos>(strQuery).ToList();
                #endregion

                #region canal
                strQuery = @"SELECT 
                                tCanal.ID
                                ,tCanal.EmpresaID
                                ,tCanal.Nome
                                ,tCanal.Comprovante
                                ,tCanal.Obs
                                ,tCanal.CanalTipoID
                                ,tCanal.ClientePresente
                                ,tCanal.TaxaConveniencia
                                ,tCanal.OpcaoImprimirSemPreco
                                ,tCanal.Cartao
                                ,tCanal.NaoCartao
                                ,tCanal.TaxaMinima
                                ,tCanal.TaxaMaxima
                                ,tCanal.ObrigaCadastroCliente
                                ,tCanal.TaxaComissao
                                ,tCanal.ComissaoMinima
                                ,tCanal.ComissaoMaxima
                                ,tCanal.Comissao
                                ,tCanal.ConfirmacaoPorEmail
                                ,tCanal.TipoVenda
                                ,tCanal.PoliticaTroca
                                ,tCanal.ComprovanteQuantidade
                                ,tCanal.ObrigatoriedadeID
                                ,tCanal.EnviaSms
                                ,tCanal.TEFF
                                ,tCanal.NroEstabelecimento
                                ,tCanal.SiglaTipo
                                ,tCanal.SiglaPagamento
                                ,tCanal.ResponsabilidadeDinheiroCliente
                                ,tCanal.Responsavel
                                ,tCanal.Recolhimento
                                ,tCanal.Sangria
                                ,tCanal.RepasseID
                                ,tCanal.Ativo
                                ,tCanal.NroEstabelecimentoAmex
                                ,tEmpresa.ID
                                ,tEmpresa.Nome
                                ,tEmpresa.ContatoNome
                                ,tEmpresa.ContatoCargo
                                ,tEmpresa.Endereco
                                ,tEmpresa.Cidade
                                ,tEmpresa.Estado
                                ,tEmpresa.CEP
                                ,tEmpresa.DDDTelefone
                                ,tEmpresa.Telefone
                                ,tEmpresa.DDDFax
                                ,tEmpresa.Fax
                                ,tEmpresa.Email
                                ,tEmpresa.Website
                                ,tEmpresa.Obs
                                ,tEmpresa.EmpresaVende
                                ,tEmpresa.EmpresaPromove
                                ,tEmpresa.RegionalID
                                ,tEmpresa.TaxaMaximaEmpresa
                                ,tEmpresa.BannerPadraoSite
                                ,tEmpresa.GrupoID
                                ,tEmpresa.Ativo
                                ,tLoja.ID
                                ,tLoja.EstoqueID
                                ,tLoja.CanalID
                                ,tLoja.Nome
                                ,tLoja.Endereco
                                ,tLoja.Cidade
                                ,tLoja.Estado
                                ,tLoja.CEP
                                ,tLoja.DDDTelefone
                                ,tLoja.Telefone
                                ,tLoja.Email
                                ,tLoja.Obs
                                ,tLoja.TEF
                                ,tLoja.NroEstabelecimento
                                ,tLoja.ComprovanteQuantidade
                                ,tLoja.TEFTipo
                                ,tLoja.numeroPOS
                                ,tLoja.UsuarioPosID
                                ,tLoja.TravaImpressao
				            FROM 
					            tCaixa (NOLOCK)
					            INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID 
					            INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID 
                                INNER JOIN tEmpresa (NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID
				            WHERE 
					            tCaixa.ID = @CaixaID";

                tEmpresa empresa = null;
                tCanal canal = null;
                tLoja loja = null;

                conIngresso.Query<tCanal, tEmpresa, tLoja, int>(strQuery, (can, emp, loj) => { canal = can; empresa = emp; loja = loj; return 1; }, new { CaixaID = senhaCompraDetalheCancelamento.CaixaID });
                #endregion

                senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "N";
                senhaCompraDetalheCancelamento.CancelamentoIndisponivel = false;

                if (senhaCompraDetalheCancelamento.Pagamentos.Count == 1)
                {
                    var pagamento = senhaCompraDetalheCancelamento.Pagamentos[0];

                    if (senhaCompraDetalheCancelamento.DataCompra > DateTime.Today.Subtract(TimeSpan.FromDays(90)))
                    {
                        if (pagamento.FormaPagamentoNome.StartsWith("PayPal"))
                        {
                            senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "D";//TODO: Alterar para P quando o PayPal for ajustado 
                        }
                        else if (pagamento.FormaPagamentoTipo.StartsWith("Crédito") && !string.IsNullOrWhiteSpace(pagamento.NumeroCartao) && !string.IsNullOrWhiteSpace(pagamento.Bandeira))
                        {
                            senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "C";
                        }
                        else
                        {
                            senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "D";
                        }
                    }
                    else if (pagamento.FormaPagamentoTipo.StartsWith("Dinheiro") || senhaCompraDetalheCancelamento.DataCompra < DateTime.Today.Subtract(TimeSpan.FromDays(90)))
                    {
                        senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "D";
                    }
                }
                else
                {
                    senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "D";
                }

                if (senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel == "D")
                {
                    if (empresa.EmpresaPromoveAsBool && canal.ID != canalID)
                    {
                        senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel = "N";
                        senhaCompraDetalheCancelamento.CancelamentoIndisponivel = true;
                        senhaCompraDetalheCancelamento.CancelamentoIndisponivelMotivo = "Esta compra só pode ser cancelada na Bilheteria " + loja.Nome + ".";
                    }
                }

                foreach (IngressoDetalheCancelamento ingresso in senhaCompraDetalheCancelamento.Ingressos)
                {
                    ingresso.CancelamentoIndisponivel = ingresso.Cancelado;
                    if (!ingresso.CancelamentoIndisponivel && ingresso.StatusCancelamentoCodigo != "P")
                    {
                        if (senhaCompraDetalheCancelamento.DataCompra < DateTime.Now.Subtract(TimeSpan.FromDays(7)))
                        {
                            ingresso.CancelamentoIndisponivel = true;
                            ingresso.CancelamentoIndisponivelMotivo = "Já fazem mais de 7 dias da data da compra.";
                        }
                        else if (DateTime.Today >= ingresso.ApresentacaoHorario.Subtract(TimeSpan.FromDays(2)))
                        {
                            if (DateTime.Today > ingresso.ApresentacaoHorario)
                            {
                                ingresso.CancelamentoIndisponivel = true;
                                ingresso.CancelamentoIndisponivelMotivo = "Já passou a data da apresentação.";
                            }
                            else if (senhaCompraDetalheCancelamento.DataCompra < DateTime.Now.Subtract(TimeSpan.FromDays(2)))
                            {
                                ingresso.CancelamentoIndisponivel = true;
                                ingresso.CancelamentoIndisponivelMotivo = "Faltam menos de 48 horas para o evento.";
                            }
                        }
                    }
                    else
                    {
                        if (ingresso.StatusCancelamentoCodigo == "P")
                        {
                            ingresso.CancelamentoIndisponivel = true;
                            ingresso.CancelamentoIndisponivelMotivo = "Aguardando Devolução";
                        }
                        else
                        {
                            ingresso.CancelamentoIndisponivelMotivo = "Ingresso já foi cancelado.";
                        }
                    }
                    ingresso.PodeCancelarTaxaConveniencia = false;
                }

                if (senhaCompraDetalheCancelamento.Ingressos.All(x => x.CancelamentoIndisponivel))
                {
                    senhaCompraDetalheCancelamento.CancelamentoIndisponivel = true;
                    senhaCompraDetalheCancelamento.CancelamentoIndisponivelMotivo = "Não existem ingressos disponiveis para serem cancelados.";
                }

                if (senhaCompraDetalheCancelamento.Ingressos.Any(t => t.StatusCancelamentoCodigo == "P"))
                {
                    senhaCompraDetalheCancelamento.Ingressos.Where(t => !t.CancelamentoIndisponivel).ToList().ForEach((t) => { t.CancelamentoIndisponivel = true; t.CancelamentoIndisponivelMotivo = "Cancelamento indisponivel"; });
                    senhaCompraDetalheCancelamento.CancelamentoIndisponivel = true;
                    senhaCompraDetalheCancelamento.CancelamentoIndisponivelMotivo = "Ja existe um, ou mais, ingressos em processo de cancelamento.";
                }

                if (senhaCompraDetalheCancelamento.CancelamentoIndisponivel)
                {
                    senhaCompraDetalheCancelamento.PodeCancelarEntrega = false;
                    senhaCompraDetalheCancelamento.PodeCancelarSeguro = false;
                }
                else
                {
                    senhaCompraDetalheCancelamento.PodeCancelarEntrega = senhaCompraDetalheCancelamento.TotalCompra.ValorTaxaEntrega > 0 && !senhaCompraDetalheCancelamento.TaxaEntregaCancelada && !senhaCompraDetalheCancelamento.Ingressos.Any(x => x.TemDevolucao);
                    senhaCompraDetalheCancelamento.PodeCancelarSeguro = senhaCompraDetalheCancelamento.ValorSeguro > 0 && !senhaCompraDetalheCancelamento.ValorSeguroCancelado && senhaCompraDetalheCancelamento.Ingressos.Any(x => !x.CancelamentoIndisponivel);
                }

            }
            return senhaCompraDetalheCancelamento;
        }

        public IPagedList<SenhaCancelamentoModel> ConsultarSenhasCancelamento(int pageNumber, int pageSize, int clienteID)
        {
            int startRow = ((pageNumber - 1) * pageSize);
            int endRow = (pageNumber * pageSize);

            #region strQuery
            string strQuery = @"SELECT
	                                SenhasCancelamento.ID
	                                ,SenhasCancelamento.SenhaCancelamento
	                                ,SenhasCancelamento.SenhaCompra
	                                ,SenhasCancelamento.DataCancelamento
	                                ,SenhasCancelamento.DataCompra
	                                ,SenhasCancelamento.CanalID
	                                ,SenhasCancelamento.CanalNome
	                                ,SenhasCancelamento.LojaID
	                                ,SenhasCancelamento.LojaNome
	                                ,SenhasCancelamento.ValorCancelamento
	                                ,SenhasCancelamento.ValorCompra
	                                ,SenhasCancelamento.StatusCancelamento
	                                ,SenhasCancelamento.Ingressos
	                                ,SenhasCancelamento.ValeIngressos
	                                ,SenhasCancelamento.TaxaConveniencia
	                                ,SenhasCancelamento.TaxaEntrega
	                                ,SenhasCancelamento.SeguroMondial
                                FROM
	                                (SELECT TOP @endRow
                                        VBC.ID AS 'ID'
	                                    ,VBC.Senha AS 'SenhaCancelamento'
	                                    ,VBV.Senha AS 'SenhaCompra'
	                                    ,VBC.CalcDataVenda AS 'DataCancelamento'
	                                    ,VBV.CalcDataVenda AS 'DataCompra'
	                                    ,Can.ID AS 'CanalID'
	                                    ,Can.Nome AS 'CanalNome'
	                                    ,Loj.ID AS 'LojaID'
	                                    ,Loj.Nome AS 'LojaNome'
	                                    ,VBC.ValorTotal AS 'ValorCancelamento'
	                                    ,VBV.ValorTotal AS 'ValorCompra'
	                                    ,CDP.StatusCancel AS 'StatusCancelamento'
	                                    ,(SELECT TOP 1 COUNT(VBI.ID) FROM tVendaBilheteriaItem VBI (NOLOCK) WHERE VBI.VendaBilheteriaID = VBC.ID) AS 'Ingressos'
	                                    ,(SELECT TOP 1 COUNT(Val.ID) FROM tValeIngressoLog Val (NOLOCK) WHERE Val.VendaBilheteriaID = VBC.ID AND Val.Acao = 'C') AS 'ValeIngressos'
	                                    ,CAST(CASE WHEN VBC.TaxaConvenienciaValorTotal > 0 THEN 1 ELSE 0 END AS bit) AS 'TaxaConveniencia'
	                                    ,CAST(CASE WHEN VBC.TaxaEntregaValor > 0 THEN 1 ELSE 0 END AS bit) AS 'TaxaEntrega'
	                                    ,CAST(CASE WHEN VBC.ValorSeguro > 0 THEN 1 ELSE 0 END AS bit) AS 'SeguroMondial'
		                                ,ROW_NUMBER() OVER (ORDER BY VBV.ID DESC) AS RowNum 
                                    FROM 
		                                tVendaBilheteria VBV (NOLOCK) 
	                                    INNER JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
	                                    LEFT JOIN tCancelDevolucaoPendente CDP (NOLOCK) ON VBC.ID = CDP.VendaBilheteriaIDCancel
	                                    LEFT JOIN tCaixa Cai (NOLOCK) ON VBC.CaixaID = Cai.ID
	                                    LEFT JOIN tLoja Loj (NOLOCK) ON Cai.LojaID = Loj.ID
	                                    LEFT JOIN tCanal Can (NOLOCK) ON Loj.CanalID = Can.ID
                                    WHERE VBV.Status != 'C' AND VBV.ClienteID = @clienteId) SenhasCancelamento 
                                WHERE 
	                                SenhasCancelamento.RowNum > @startRow".Replace("@startRow", startRow.ToString()).Replace("@endRow", endRow.ToString());
            #endregion

            #region strCount
            string strCount = @"SELECT 
	                                COUNT(tVendaBilheteria.ID)
                                FROM 
	                                tVendaBilheteria (NOLOCK)
                                WHERE 
	                                tVendaBilheteria.Status = 'C'
	                                AND tVendaBilheteria.ClienteID = @clienteId";
            #endregion

            List<SenhaCancelamentoModel> result = conIngresso.Query<SenhaCancelamentoModel>(strQuery, new { clienteID = clienteID }).ToList();

            return result.ToPagedList(pageNumber, pageSize, (int)conIngresso.ExecuteScalar(strCount, new { clienteID = clienteID }));
        }

        public SenhaCancelamentoDetalhe ConsultarSenhasCancelamentoDetalhe(int vendabilheteriaID)
        {
            #region senhaCancelamentoDetalhe
            string strQuery = @"SELECT 
	                                VBC.ID AS 'CancelamentoID'
	                                ,VBV.ID AS 'CompraID' 
	                                ,VBV.Senha AS 'SenhaCompra'
	                                ,VBC.Senha AS 'SenhaCancelamento'
	                                ,VBV.CalcDataVenda AS 'DataCompra' 
	                                ,VBC.CalcDataVenda AS 'DataCancelamento' 
	                                ,CDP.StatusCancel AS 'StatusCodigo' 
	                                ,VBC.CaixaID AS 'CaixaID'
	                                ,VBC.Obs AS 'Obs'
	                                ,VBV.ValorSeguro AS 'ValorSeguroCompra'
	                                ,VBC.ValorSeguro AS 'ValorSeguroCancelamento'
                                FROM 
	                                tVendaBilheteria VBV (NOLOCK)
	                                INNER JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
	                                LEFT JOIN tCancelDevolucaoPendente CDP (NOLOCK) ON VBC.ID = CDP.VendaBilheteriaIDCancel
                                WHERE
	                                VBC.ID = @vendaBilheteriaID";

            SenhaCancelamentoDetalhe senhaCancelamentoDetalhe = conIngresso.Query<SenhaCancelamentoDetalhe>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).FirstOrDefault();
            #endregion

            if (senhaCancelamentoDetalhe != null)
            {
                #region ingressosCancelados
                strQuery = @"SELECT
	                            Ing.ID AS 'ID'
	                            ,Ing.Codigo AS 'Codigo'
	                            ,Lug.ID AS 'LugarID'
	                            ,Pre.ID AS 'PrecoID'
	                            ,Eve.ID AS 'EventoID'
	                            ,Eve.Nome AS 'EventoNome'
	                            ,Loc.ID AS 'LocalID'
	                            ,Loc.Nome AS 'LocalNome'
	                            ,Pre.Nome AS 'PrecoNome'
	                            ,Pre.Valor AS 'PrecoValor'
	                            ,dbo.StringToDatetime(Ing.TimeStampReserva) AS 'TimeStampReserva'
	                            ,VBI.TaxaConveniencia AS 'TaxaConveniencia'
	                            ,Ing.PacoteGrupo AS 'PacoteGrupo'
	                            ,Pac.ID AS 'PacoteID'
	                            ,Sto.ID AS 'SetorID'
	                            ,Sto.Nome AS 'SetorNome'
	                            ,Apre.ID AS 'ApresentacaoID'
	                            ,Apre.CalcHorario AS 'ApresentacaoHorario'
                            FROM
	                            tVendaBilheteria VB
	                            INNER JOIN tIngressoLog Inl (NOLOCK) ON VB.ID = Inl.VendaBilheteriaID
	                            INNER JOIN tIngresso Ing (NOLOCK) ON Inl.IngressoID = Ing.ID
	                            LEFT JOIN tLugar Lug (NOLOCK) ON Ing.LugarID = Lug.ID
	                 	        LEFT JOIN tSetor Sto (NOLOCK) ON Ing.SetorID = Sto.ID
	                            LEFT JOIN tEvento Eve (NOLOCK) ON Ing.EventoID = Eve.ID 
	                            LEFT JOIN tLocal Loc (NOLOCK) ON Ing.LocalID = Loc.ID
	                            LEFT JOIN tPreco Pre (NOLOCK) ON Inl.PrecoID = Pre.ID
	                            LEFT JOIN tVendaBilheteriaItem VBI (NOLOCK) ON Inl.VendaBilheteriaID = VBI.VendaBilheteriaID AND VBI.ID = Inl.VendaBilheteriaItemID
	                            LEFT JOIN tPacote Pac (NOLOCK) ON Ing.PacoteID = Pac.ID
	                            LEFT JOIN tApresentacao Apre (NOLOCK) ON Ing.ApresentacaoID = Apre.ID
                            WHERE
	                            VB.ID = @vendabilheteriaID AND Inl.Acao = 'C'";

                senhaCancelamentoDetalhe.IngressosCancelados = conIngresso.Query<Ingresso>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).ToList();
                #endregion

                #region totalCompra
                strQuery = @"SELECT
	                            VBV.ValorTotal AS 'ValorTotal' 
	                            ,SUM(CASE WHEN VBVFD.Valor IS NULL THEN 0 ELSE VBVFD.Valor END) AS 'ValorDesconto'
	                            ,VBV.TaxaEntregaValor AS 'ValorTaxaEntrega'
	                            ,VBV.TaxaConvenienciaValorTotal AS 'ValorTaxaConveniencia'
	                            ,((((VBV.ValorTotal + SUM(CASE WHEN VBVFD.Valor IS NULL THEN 0 ELSE VBVFD.Valor END)) - VBV.TaxaEntregaValor) - VBV.TaxaConvenienciaValorTotal) - VBV.ValorSeguro) AS 'ValorIngressos'
                            FROM 
	                            tVendaBilheteria VBV (NOLOCK)
	                            INNER JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
	                            LEFT JOIN tVendaBilheteriaFormaPagamento VBVFD (NOLOCK) ON VBV.ID = VBVFD.VendaBilheteriaID AND VBVFD.ValeIngressoID > 0
                            WHERE
	                            VBC.ID = @vendabilheteriaID
                            GROUP BY
	                            VBV.ValorTotal
	                            ,VBV.TaxaEntregaValor
	                            ,VBV.TaxaConvenienciaValorTotal
                                ,VBV.ValorSeguro";

                senhaCancelamentoDetalhe.TotalCompra = conIngresso.Query<CompraTotal>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).FirstOrDefault();
                #endregion

                #region totalCancelado
                strQuery = @"SELECT
	                            VBC.ValorTotal AS 'ValorTotal' 
	                            ,SUM(CASE WHEN VBCFD.Valor IS NULL THEN 0 ELSE VBCFD.Valor END) AS 'ValorDesconto'
	                            ,VBC.TaxaEntregaValor AS 'ValorTaxaEntrega'
	                            ,VBC.TaxaConvenienciaValorTotal AS 'ValorTaxaConveniencia'
	                            ,((((VBC.ValorTotal + SUM(CASE WHEN VBCFD.Valor IS NULL THEN 0 ELSE VBCFD.Valor END)) - VBC.TaxaEntregaValor) - VBC.TaxaConvenienciaValorTotal) - VBC.ValorSeguro) AS 'ValorIngressos'
                            FROM 
	                            tVendaBilheteria VBV (NOLOCK)
	                            INNER JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
	                            LEFT JOIN tVendaBilheteriaFormaPagamento VBCFD (NOLOCK) ON VBC.ID = VBCFD.VendaBilheteriaID AND VBCFD.ValeIngressoID > 0
                            WHERE
	                            VBC.ID = @vendabilheteriaID
                            GROUP BY
	                            VBC.ValorTotal 
	                            ,VBC.TaxaEntregaValor
	                            ,VBC.TaxaConvenienciaValorTotal
                                ,VBC.ValorSeguro";

                senhaCancelamentoDetalhe.TotalCancelado = conIngresso.Query<CompraTotal>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).FirstOrDefault();
                #endregion

                #region canal
                strQuery = @"SELECT 
                                tCanal.Nome AS 'Canal'
                            FROM 
	                            tVendaBilheteria (NOLOCK) 
	                            INNER JOIN tCaixa (NOLOCK) ON tVendaBilheteria.CaixaID = tCaixa.ID
	                            INNER JOIN tLoja (NOLOCK) ON tCaixa.LojaID = tLoja.ID
	                            INNER JOIN tCanal (NOLOCK) ON tLoja.CanalID = tCanal.ID
                            WHERE 
	                            tVendaBilheteria.ID = @VendaBilheteriaID
                                AND tVendaBilheteria.Status = 'C'";

                String canal = conIngresso.ExecuteScalar<String>(strQuery, new { vendabilheteriaID = vendabilheteriaID });
                #endregion

                #region historico
                senhaCancelamentoDetalhe.Historico = new List<Historico>();

                senhaCancelamentoDetalhe.Historico.Add(new Historico
                {
                    DataMovimentacao = senhaCancelamentoDetalhe.DataCancelamento,
                    Movimentacao = String.Format("Soliticação de Cancelamento. Canal de Cancelamento: {0}", canal)
                });

                strQuery = @"SELECT 
	                            Inl.IngressoID
	                            ,dbo.StringToDateTime(Inl.TimeStamp) AS TimeStamp
                                ,Inl.Acao
	                            ,CASE 
                                    WHEN (SELECT COUNT(tIngressoLog.ID) FROM tIngressoLog (NOLOCK) WHERE tIngressoLog.IngressoID = Inl.IngressoID AND tIngressoLog.Acao = 'I' AND tIngressoLog.VendaBilheteriaID = VBV.ID) > 0
		                                THEN 1
		                                ELSE 0
		                            END AS Impresso
                            FROM 
	                            tVendaBilheteria VBV (NOLOCK)
	                            INNER JOIN tVendaBilheteria VBC (NOLOCK) ON VBV.ID = VBC.VendaBilheteriaIDOrigem
	                            INNER JOIN tIngressoLog Inl (NOLOCK) ON VBC.ID = Inl.VendaBilheteriaID
                            WHERE 
	                            VBC.ID = @vendabilheteriaID
	                            AND Inl.Acao = 'C'";
                List<ProtocoloCancelamento> protocoloCancelamento = conIngresso.Query<ProtocoloCancelamento>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).ToList();

                DateTime data = DateTime.MinValue;
                bool flag;
                if (protocoloCancelamento.Count > 0)
                {
                    flag = true;
                    if (protocoloCancelamento.Where(x => x.Impresso == true).Count() > 0)
                    {
                        data = protocoloCancelamento.Where(x => x.Acao == "C").LastOrDefault().TimeStamp;
                    }
                }
                else
                {
                    flag = false;
                }

                if (data > DateTime.MinValue)
                {
                    senhaCancelamentoDetalhe.Historico.Add(new Historico
                    {
                        DataMovimentacao = senhaCancelamentoDetalhe.DataCancelamento,
                        Movimentacao = "Devolução Física de Ingresso feita a ingresso rápido."
                    });
                }

                if (flag)
                {
                    strQuery = @"SELECT 
	                                DepositoBancario.ID
	                                ,DepositoBancario.VendaBilheteriaIDVenda
	                                ,DepositoBancario.DataDeposito
	                                ,DepositoBancario.Banco
	                                ,DepositoBancario.Agencia
	                                ,DepositoBancario.Conta
	                                ,DepositoBancario.Valor
	                                ,DepositoBancario.CPFCorrentista
	                                ,DepositoBancario.NomeCorrentista
	                                ,DepositoBancario.DataInsert
	                                ,DepositoBancario.Email
	                                ,DepositoBancario.CancelamentoPor
	                                ,DepositoBancario.Status
                                FROM 
	                                EstornoDadosDepositoBancario DepositoBancario (NOLOCK)
                                WHERE 
	                                DepositoBancario.VendaBilheteriaIDCancel =  @vendabilheteriaID";
                    List<EstornoDepositoBancario> estornoDepositoBancario = conIngresso.Query<EstornoDepositoBancario>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).ToList();

                    if (estornoDepositoBancario.Count > 0)
                    {
                        senhaCancelamentoDetalhe.Historico.Add(new Historico
                        {
                            DataMovimentacao = estornoDepositoBancario.FirstOrDefault().DataInsert,
                            Movimentacao = String.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Depósito bancário junto ao banco {0}, Agência {1}, Conta Corrente {2}.", estornoDepositoBancario.FirstOrDefault().Banco, estornoDepositoBancario.FirstOrDefault().Agencia, estornoDepositoBancario.FirstOrDefault().Conta)
                        });
                    }

                    strQuery = @"SELECT	
	                                CartaoCredito.ID
	                                ,CartaoCredito.VendaBilheteriaIDVenda
	                                ,CartaoCredito.Bandeira
	                                ,CartaoCredito.Cartao
	                                ,CartaoCredito.Valor
	                                ,CartaoCredito.Cliente
	                                ,CartaoCredito.CPFCliente
	                                ,CartaoCredito.CancelamentoPor
	                                ,CartaoCredito.DataInsert
	                                ,CartaoCredito.DataEnvio
	                                ,CartaoCredito.Email
	                                ,CartaoCredito.Status
                                FROM 
	                                EstornoDadosCartaoCredito CartaoCredito (NOLOCK)
                                WHERE 
	                                CartaoCredito.VendaBilheteriaIDCancel = @vendabilheteriaID";
                    List<EstornoCartaoCredito> estornoCartaoCredito = conIngresso.Query<EstornoCartaoCredito>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).ToList();

                    if (estornoCartaoCredito.Count > 0)
                    {
                        senhaCancelamentoDetalhe.Historico.Add(new Historico
                        {
                            DataMovimentacao = estornoCartaoCredito.FirstOrDefault().DataInsert,
                            Movimentacao = String.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Estorno de Crédito junto a operadora {0}.{1}{2}.", estornoCartaoCredito.FirstOrDefault().Bandeira, Environment.NewLine, estornoCartaoCredito.FirstOrDefault().StatusCompleto)
                        });
                    }

                    strQuery = @"SELECT 
	                                Dinheiro.ID
	                                ,Dinheiro.VendaBilheteriaIDVenda
	                                ,Dinheiro.Valor
	                                ,Dinheiro.Cliente
	                                ,Dinheiro.CancelamentoPor
	                                ,Dinheiro.Email
	                                ,Dinheiro.DataInsert
                                FROM 
	                                EstornoDadosDinheiro Dinheiro (NOLOCK)
                                WHERE 
	                                Dinheiro.VendaBilheteriaIDCancel = @vendabilheteriaID";
                    List<EstornoDinheiro> estornoDadosDinheiro = conIngresso.Query<EstornoDinheiro>(strQuery, new { vendabilheteriaID = vendabilheteriaID }).ToList();

                    if (estornoDadosDinheiro.Count > 0)
                    {
                        senhaCancelamentoDetalhe.Historico.Add(new Historico
                        {
                            DataMovimentacao = estornoDadosDinheiro.FirstOrDefault().DataInsert,
                            Movimentacao = string.Format("Início de Processo de Devolução Financeira do Ingresso. Forma de Devolução: Espécies direto ao cliente {0}.", estornoDadosDinheiro.FirstOrDefault().Cliente)
                        });
                    }
                }

                #endregion
            }
            return senhaCancelamentoDetalhe;
        }

        public int CheckSenhaCancelamento(string SenhaCancelamento)
        {
            #region strQuery
            var strQuery = @"SELECT 
                                ID 
                            FROM 
                                tVendaBilheteria (NOLOCK)
							WHERE 
                                Senha = @Senha AND Status = 'C'";
            #endregion

            var result = conIngresso.Query<tVendaBilheteria>(strQuery, new { Senha = SenhaCancelamento }).FirstOrDefault();

            return result == null ? 0 : result.ID;
        }

        public int GetSenhaCompra(int senhaCancelID)
        {
            #region strQuery
            var strQuery = @"SELECT 
                                VendaBilheteriaIDOrigem AS ID 
                            FROM 
                                tVendaBilheteria
							WHERE 
                                ID = @senhaCancelID";
            #endregion

            var result = conIngresso.Query<tVendaBilheteria>(strQuery, new { senhaCancelID = senhaCancelID }).FirstOrDefault();

            return result == null ? 0 : result.ID;
        }

        #region Relatórios
        public List<RelatorioModel> ConsultarRelatorioFixoDayDream(int eventoID)
        {

            #region OldstrQuery
            /*string query = string.Format(@"SELECT DISTINCT 
	                                        CASE WHEN LEN(tdo.Nome) > 0
		                                        THEN
			                                        tdo.Nome
	                                        ELSE
		                                        CASE WHEN LEN(tc.ID) > 0
			                                        THEN tc.Nome
		                                        ELSE
			                                        CASE WHEN LEN(tc.CNPJ) > 0
				                                        THEN tc.NomeFantasia
			                                        ELSE    
				                                        tc.Nome  COLLATE Latin1_General_CI_AI
			                                        END
		                                        END    
	                                        END AS Participante,	 
                                        ts.nome AS Atividades, 
		                                        CASE WHEN LEN(tdo.CPF) > 0
			                                        THEN tdo.CPF
		                                        ELSE
			                                        CASE WHEN LEN(tdo.RG) > 0
				                                        THEN tdo.RG
			                                        ELSE
				                                        CASE WHEN LEN(tc.ID) > 0
					                                        THEN       
						                                        CASE WHEN LEN(tic.CPF) > 0
							                                        THEN tic.CPF COLLATE Latin1_General_CI_AI
						                                        ELSE
							                                        tc.CPF COLLATE Latin1_General_CI_AI
						                                        END
				                                        ELSE
					                                        CASE WHEN LEN(tc.CNPJ) > 0
						                                        THEN tc.CNPJ
					                                        ELSE    
						                                        tc.CPF
					                                        END 
				                                        END
			                                        END
		                                        END AS RG,	 
                            CASE WHEN LEN(tdo.Email) > 0 THEN tdo.Email COLLATE Latin1_General_CI_AI ELSE tc.email COLLATE Latin1_General_CI_AI END AS email, 
                            CASE WHEN LEN(tdo.Telefone) > 0 THEN tdo.Telefone ELSE tc.DDDTelefone + '-' + tc.Telefone END AS Telefone, 
                            tc.DDDCelular + '-' + tc.Celular AS Celular,
                            tc2.nome AS ResponsavelCompra, 
                            '''' + ti.codigo AS CodigoIngresso,
                            tvb.senha AS Senha,
                            dbo.DataHoraFormatada(ta.Horario) as DataApresentacao
                            FROM dbo.tVendaBilheteria tvb (NOLOCK) 
                            INNER JOIN dbo.tIngresso ti (NOLOCK) ON ti.VendaBilheteriaID = tvb.ID
                            INNER JOIN dbo.tIngressoCliente tic (NOLOCK) ON tic.IngressoID = ti.ID
                            LEFT JOIN dbo.tCliente tc (NOLOCK) ON tc.id = tic.ClienteID
                            LEFT JOIN tDonoIngresso tdo (NOLOCK) ON tdo.ID = tic.DonoID
                            INNER JOIN dbo.tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID
                            INNER JOIN dbo.tApresentacaoSetor tas (NOLOCK) ON tas.ID = ti.ApresentacaoSetorID
                            INNER JOIN dbo.tSetor ts (NOLOCK) ON ts.ID = tas.SetorID
                            LEFT JOIN dbo.tCliente tc2 (NOLOCK) ON tc2.ID = tvb.ClienteID
                            INNER JOIN dbo.tIngressoLog til (NOLOCK) ON til.ingressoID = ti.id 
                            WHERE ti.EventoID = {0}", eventoID);*/

            #endregion

            #region strQuery
            string query = string.Format(@"SELECT DISTINCT 
                                             CASE WHEN LEN(tdo.Nome) > 0
                                              THEN
                                               tdo.Nome
                                             ELSE
                                              CASE WHEN LEN(tc.ID) > 0
                                               THEN tc.Nome
                                              ELSE
                                               CASE WHEN LEN(tc.CNPJ) > 0
                                                THEN tc.NomeFantasia
                                               ELSE    
                                                tc.Nome  COLLATE Latin1_General_CI_AI
                                               END
                                              END    
                                             END AS Participante,  
                                            ts.nome AS Atividades, 
                                              CASE WHEN LEN(tdo.CPF) > 0
                                               THEN tdo.CPF
                                              ELSE
                                               CASE WHEN LEN(tdo.RG) > 0
                                                THEN tdo.RG
                                               ELSE
                                                CASE WHEN LEN(tc.ID) > 0
                                                 THEN       
                                                  CASE WHEN LEN(tic.CPF) > 0
                                                   THEN tic.CPF COLLATE Latin1_General_CI_AI
                                                  ELSE
                                                   tc.CPF COLLATE Latin1_General_CI_AI
                                                  END
                                                ELSE
                                                 CASE WHEN LEN(tc.CNPJ) > 0
                                                  THEN tc.CNPJ
                                                 ELSE    
                                                  tc.CPF
                                                 END 
                                                END
                                               END
                                              END AS RG,  
                                            CASE WHEN LEN(tdo.Email) > 0 THEN tdo.Email COLLATE Latin1_General_CI_AI ELSE tc.email COLLATE Latin1_General_CI_AI END AS email, 
                                            CASE WHEN LEN(tdo.Telefone) > 0 THEN tdo.Telefone ELSE tc.DDDTelefone + '-' + tc.Telefone END AS Telefone, 
                                            tc.DDDCelular + '-' + tc.Celular AS Celular,
                                            tc2.nome AS ResponsavelCompra, 
                                            '''' + ti.codigo AS CodigoIngresso,
                                            tvb.senha AS Senha,
                                            dbo.DataHoraFormatada(ta.Horario) as DataApresentacao,
                                            (tc3.DDDTelefone + '' + tc3.Telefone) as TelefoneResponsavel,
                                            (tc3.DDDCelular + '' + tc3.Celular) as CelularResponsavel
                                            FROM dbo.tVendaBilheteria tvb (NOLOCK) 
                                            INNER JOIN dbo.tCliente tc3 (NOLOCK) ON tc3.id = tvb.ClienteID
                                            INNER JOIN dbo.tIngresso ti (NOLOCK) ON ti.VendaBilheteriaID = tvb.ID
                                            -- Troquei aqui para left, a original esta com inner
                                            left JOIN dbo.tIngressoCliente tic (NOLOCK) ON tic.IngressoID = ti.ID
                                            LEFT JOIN dbo.tCliente tc (NOLOCK) ON tc.id = tic.ClienteID
                                            LEFT JOIN tDonoIngresso tdo (NOLOCK) ON tdo.ID = tic.DonoID
                                            INNER JOIN dbo.tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID
                                            INNER JOIN dbo.tApresentacaoSetor tas (NOLOCK) ON tas.ID = ti.ApresentacaoSetorID
                                            INNER JOIN dbo.tSetor ts (NOLOCK) ON ts.ID = tas.SetorID
                                            LEFT JOIN dbo.tCliente tc2 (NOLOCK) ON tc2.ID = tvb.ClienteID
                                            INNER JOIN dbo.tIngressoLog til (NOLOCK) ON til.ingressoID = ti.id 
                                            WHERE ti.EventoID = {0}", eventoID);
            #endregion

            var result = dbIngresso.Database.SqlQuery<RelatorioModel>(query).ToList();

            return result;
        }


        public List<RelatorioFilarmonicaModel> ConsultarRelatorioFilarmonica(int assinaturaTipoID)
        {
            #region strQuery
            string strQuery = @"SELECT 
	                                dbo.Datahoraformatada(vb.datavenda) AS 'Data Venda'
	                                ,cli.Nome AS 'Cliente'
	                                ,cli.Cpf
	                                ,Cli.RG
	                                ,Cli.Email
	                                ,tass.nome AS 'T. Assinatura'
	                                ,tset.nome AS Setor
	                                ,tlug.Codigo AS 'Codigo'
	                                ,COALESCE(tx.Nome, '') AS 'Forma de Entrega'
	                                ,COALESCE(prectip.nome, '') AS 'Preço'
	                                ,CASE acao                              
		                                WHEN 'D' THEN 'Desistencia'
	                                    WHEN 'R' THEN 'Renovado'
	                                    WHEN 'E' THEN 'Troca Efetiva'
	                                    WHEN 'N' THEN 'Aquisição' 
	                                    WHEN 'T' THEN 'Troca Sinalizada'
	                                    WHEN 'A' THEN 'Aguardando Ação'
	                                    END AS 'Ação'
	                                ,CASE tac.Status
	                                    WHEN 'A' THEN 'Aguardando Pagamento'
	                                    WHEN 'S' THEN 'Renovado sem pagamento'
	                                    WHEN 'R' THEN 'Renovado'
	                                    WHEN 'T' THEN 'Trocado'
	                                    WHEN 'Z' THEN 'Troca Sinalizada'
	                                    WHEN 'D' THEN 'Desistencia'
	                                    WHEN 'I' THEN 'Indisponivel'
	                                    WHEN 'N' THEN 'Nova Aquisição'
	                                    END AS Status
	                                ,COALESCE(vb.Senha, '') AS 'Senha de Venda'
	                                ,dbo.DataFormatada(TimeStamp) AS 'Data'
	                                ,cli.EnderecoCliente AS 'Endereço'
	                                ,cli.NumeroCliente AS 'Número'
	                                ,cli.complementocliente AS 'Complemento'
	                                ,cli.BairroCliente AS 'Bairro'
	                                ,cli.CepCliente AS 'CEP'
	                                ,cli.EstadoCliente AS 'Estado'
	                                ,cli.CidadeCliente AS 'Cidade'
                                    ,cli.DDDTelefone + '-' + cli.Telefone AS Telefone
	                                ,cli.DDDTelefoneComercial + '-' + cli.TelefoneComercial AS 'Telefone Comercial'
	                                ,cli.DDDCelular + '-' + cli.Celular AS 'Celular'
	                                ,cli.Profissao
	                                ,dbo.DataFormatada(cli.DataNascimento) AS 'Data de Nascimento'
	                                ,cli.Sexo
                                FROM 
	                                tassinaturacliente tac (NOLOCK)
                                    LEFT JOIN tcliente cli (NOLOCK) ON tac.ClienteID = cli.ID
                                    LEFT JOIN tassinatura tass (NOLOCK) ON tass.id = tac.assinaturaid
                                    LEFT JOIN tAssinaturaTipo tap (NOLOCK) ON tap.ID = tass.AssinaturaTipoID
                                    LEFT JOIN tsetor tset (NOLOCK) ON tset.id = tac.setorid
                                    LEFT JOIN tlugar tlug (NOLOCK) ON tlug.id = tac.lugarid
                                    LEFT JOIN tvendabilheteria vb (NOLOCK) ON vb.id = tac.vendabilheteriaid
                                    LEFT JOIN tentregacontrole txc (NOLOCK) ON txc.ID = vb.entregacontroleid
                                    LEFT JOIN tentrega tx (NOLOCK) ON tx.id = txc.entregaid
                                    LEFT JOIN tprecotipo prectip (NOLOCK) ON prectip.id = tac.precotipoid
                                WHERE 
	                                acao IN ('E','N','R', 'D', 'T') 
	                                AND AssinaturaTipoID = @assinaturaTipoID 
	                                AND cli.nome IS NOT NULL
                                ORDER BY 
	                                Timestamp, tac.assinaturaID ASC";
            #endregion

            var result = dbIngresso.Database.SqlQuery<RelatorioFilarmonicaModel>(strQuery, new { assinaturaTipoID = assinaturaTipoID }).ToList();
            return result;
        }

        public List<RelatorioRio> ConsultarRelatorioRio(string MesAno)
        {
            #region strQuery
            string strQuery = @"SELECT DISTINCT 
                                tc.Nome 
                                ,''''+ tc.CPF AS 'CPF' 
                                ,tc.DDDTelefone + tc.Telefone AS 'Telefone'
                                ,tc.Email
                                ,te.Nome AS 'Evento'
                                ,dbo.DataHoraFormatada(ta.Horario) AS 'Apresentaçao'
                                ,ts.Nome AS 'Setor'
                                ,tp.Nome AS 'Preço'
                                ,dbo.NumeroFormatado(tvb.ValorTotal) AS 'VlPreço'
                                ,ti.Codigo AS 'CodigoIngresso' 
                            FROM 
                                tVendaBilheteria tvb 
                                LEFT JOIN tCliente tc (NOLOCK) on tc.ID = tvb.ClienteID
                                INNER JOIN tIngresso ti (NOLOCK) on ti.VendaBilheteriaID = tvb.ID and ti.ClienteID = tc.ID
                                INNER JOIN tEvento te (NOLOCK) on te.ID = ti.EventoID
                                INNER JOIN tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID
                                INNER JOIN tPreco tp (NOLOCK) ON tp.ID = ti.PrecoID
                                INNER JOIN tEmpresa tem (NOLOCK) ON tem.ID = ti.EmpresaID
                                INNER JOIN tSetor ts (NOLOCK) ON ts.ID = ti.SetorID
                            WHERE 
                                tem.ID = 1023 
                                AND ta.Horario LIKE @MesAno";
            #endregion

            var result = conIngresso.Query<RelatorioRio>(strQuery, new { MesAno = MesAno }).ToList();
            return result;
        }

        public List<RelatorioGlobo> ConsultarRelatorioGlobo(string MesAno)
        {
            #region strQuery
            string strQuery = @"SELECT DISTINCT 
                                tp.Nome AS 'Preço' 
                                ,dbo.DataHoraFormatada(tvb.DataVenda) AS 'Data' 
                                ,te.Nome AS 'Evento'
                                ,dbo.DataHoraFormatada(ta.Horario) AS 'ApresentacaoHorario'
                                ,dbo.NumeroFormatado(tvb.ValorTotal) AS 'Valor'
                                ,tc.Nome  
                                ,tc.CPF 
                                ,tc.Email 
                                ,tc.DDDTelefone + tc.Telefone AS 'Telefone' 
                            FROM 
                                tVendaBilheteria tvb   
                                LEFT JOIN tCliente tc (nolock) on tc.ID = tvb.ClienteID
                                INNER JOIN tIngresso ti (nolock) on ti.VendaBilheteriaID = tvb.ID and ti.ClienteID = tc.ID
                                INNER JOIN tEvento te (nolock) on te.ID = ti.EventoID
                                INNER JOIN tApresentacao ta (NOLOCK) ON ta.ID = ti.ApresentacaoID
                                INNER JOIN tPreco tp (NOLOCK) ON tp.ID = ti.PrecoID
                                INNER JOIN tEmpresa tem (NOLOCK) ON tem.ID = ti.EmpresaID
                            WHERE 
                                tem.ID = 1023 
                                AND tp.Nome LIKE '%Globo%' 
                                AND tvb.datavenda LIKE @MesAno";
            #endregion

            var result = conIngresso.Query<RelatorioGlobo>(strQuery, new { MesAno = MesAno }).ToList();
            return result;
        }

        public List<RelatorioPortoSeguro> ConsultarRelatorioPortoSeguro(string MesAno)
        {
            #region strQuery
            string strQuery = @"SELECT
	                            tCliente.Nome
	                            ,'''' + tCliente.CPF AS CPF
	                            ,tCliente.DDDTelefone + '-' + tCliente.Telefone AS Telefone
	                            ,tCliente.Email
	                            ,tEvento.Nome AS Evento
	                            ,dbo.DataHoraFormatada(Horario) AS ApresentacaoHorario
                                ,dbo.DataHoraFormatada(DataVenda) AS DataVenda 
	                            ,tSetor.Nome AS NomeSetor
	                            ,tPreco.Nome AS Preco
	                            ,dbo.NumeroFormatado(tPreco.Valor) AS VlrPreco
	                            ,'''' + tIngresso.Codigo AS CodigoIngresso
                            FROM 
	                            tIngressoCliente (NOLOCK)
	                            LEFT JOIN tCliente (NOLOCK) ON tCliente.ID = tIngressoCliente.ClienteID
	                            INNER JOIN tIngresso (NOLOCK) ON tIngresso.ID = tIngressoCliente.IngressoID
								INNER JOIN tVendaBilheteria tvb (NOLOCK) ON tvb.id = tIngresso.VendaBilheteriaID
	                            INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngressoCliente.ApresentacaoID
	                            INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
	                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
	                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
	                            LEFT JOIN tCotaItem (NOLOCK) ON tCotaItem.ID = tIngressoCliente.CotaItemID
	                            LEFT JOIN tCota (NOLOCK) ON tCota.ID = tCotaItem.CotaID
                            WHERE 
	                            tPreco.Nome LIKE '%porto%' 
	                            AND tApresentacao.Horario >= @MesAno
                            ORDER BY 
	                            tCliente.Nome";
            #endregion

            var result = conIngresso.Query<RelatorioPortoSeguro>(strQuery, new { MesAno = MesAno }).ToList();
            return result;
        }

        public List<RelatorioValeIngresso> ConsultarRelatorioValeIngresso(string MesAno)
        {
            #region strQuery
            string strQuery = @"SELECT 
	                            e.Nome AS TaxaEntrega
	                            ,ISNULL(a.Nome, ' ') AS Area 
	                            ,ISNULL(p.Nome, ' ') AS Periodo 
	                            ,COUNT(vb.ID) AS Quantidade 
	                            ,dbo.NumeroFormatado(SUM(vb.TaxaEntregaValor)) AS ValorTotalTaxa
                            FROM 
	                            tVendaBilheteria vb (NOLOCK) 
	                            INNER JOIN tEntregaControle c (NOLOCK) ON c.ID = vb.EntregaControleID
	                            INNER JOIN tEntrega e (NOLOCK) ON e.ID = c.EntregaID
	                            LEFT JOIN tEntregaArea a (NOLOCK) ON a.ID = c.EntregaAreaID
	                            LEFT JOIN tEntregaPeriodo p (NOLOCK) ON p.ID = c.PeriodoID
                            WHERE 
	                            DataVenda LIKE @MesAno 
                            GROUP BY 
	                            e.Nome
	                            ,a.Nome
	                            ,p.Nome";
            #endregion

            var result = conIngresso.Query<RelatorioValeIngresso>(strQuery, new { MesAno = MesAno }).ToList();
            return result;
        }

        public List<RelatorioVendasCanal> ConsultarRelatorioVendasCanal(string MesAno)
        {
            string parametro = MesAno + "%";
            #region strQuery
            string strQuery = @"SELECT 
                                tcn.nome AS Canal,
                                COUNT(DISTINCT tvb.id) AS QuantidadeVendas,
                                COUNT(DISTINCT ti.id) AS QuantidadeIngresso
                                FROM tVendaBilheteria tvb (NOLOCK)
                                LEFT JOIN tCliente tc (NOLOCK)ON tc.ID = tvb.ClienteID
                                INNER JOIN tIngresso ti (NOLOCK)ON ti.VendaBilheteriaID = tvb.ID
                                INNER JOIN dbo.tCaixa tc2 (NOLOCK) ON tc2.ID = tvb.CaixaID
                                INNER JOIN dbo.tLoja tl (NOLOCK) ON tl.ID = tc2.LojaID
                                INNER JOIN dbo.tCanal tcn(NOLOCK) ON tcn.ID = tl.CanalID
                                WHERE  tvb.DataVenda like @parametro
                                GROUP BY tcn.nome, tcn.id
                                ORDER BY tcn.nome";
            #endregion

            var result = conIngresso.Query<RelatorioVendasCanal>(strQuery, new { parametro = parametro }).ToList();
            return result;
        }

        public List<RelatorioValeIngressoVenda> ConsultarRelatorioValeIngressoVenda(string MesAno)
        {
            string parametro = MesAno + "%";
            #region strQuery
            string strQuery = @"SELECT
		                            c.Nome AS Canal,
	                            SUM(CASE WHEN vil.Acao = 'V'
		                            THEN 1
		                            ELSE -1
	                            END) AS Quantidade, 
	                            dbo.NumeroFormatado (SUM(CASE WHEN vil.Acao = 'V'
		                            THEN vit.Valor
		                            ELSE -vit.Valor
		                            END)) AS Total
	                            FROM tVendaBilheteria vb (NOLOCK)
	                            INNER JOIN tValeIngressoLog vil (NOLOCK) ON vil.VendaBilheteriaID = vb.ID
	                            INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = vil.ValeIngressoID
	                            INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID
	                            INNER JOIN tCaixa cx (NOLOCK) ON cx.ID = vb.CaixaID
	                            INNER JOIN tLoja l (NOLOCK) ON l.ID = cx.LojaID
	                            INNER JOIN tCanal c (NOLOCK) ON c.ID = l.CanalID
	                            WHERE Acao IN('V', 'C') AND DataVenda LIKE @parametro
	                            GROUP BY c.Nome";
            #endregion

            var result = conIngresso.Query<RelatorioValeIngressoVenda>(strQuery, new { parametro = parametro }).ToList();
            return result;
        }

        public List<RelatorioValeIngressoTroca> ConsultarRelatorioValeIngressoTroca(string MesAno)
        {
            string parametro = MesAno + "%";
            #region strQuery
            string strQuery = @"SELECT
	                            vit.Nome,
	                            SUM(CASE WHEN vil.Acao = 'T'
		                            THEN 1
		                            ELSE -1
		                            END) AS Quantidade, 	
	                            dbo.NumeroFormatado (SUM(CASE WHEN vil.Acao = 'T'
		                            THEN vit.Valor
		                            ELSE -vit.Valor
		                            END)) AS Valor
	                            FROM tVendaBilheteria vb (NOLOCK)
	                            INNER JOIN tValeIngressoLog vil (NOLOCK) ON vil.VendaBilheteriaID = vb.ID
	                            INNER JOIN tValeIngresso vi (NOLOCK) ON vi.ID = vil.ValeIngressoID
	                            INNER JOIN tValeIngressoTipo vit (NOLOCK) ON vit.ID = vi.ValeIngressoTipoID
	                            INNER JOIN tVendaBilheteriaFormaPagamento tvbfp ON tvbfp.VendaBilheteriaID = vb.ID
	                            WHERE Acao IN('T') AND DataVenda LIKE @parametro
	                            GROUP BY vit.Nome, tvbfp.FormaPagamentoID";
            #endregion

            var result = conIngresso.Query<RelatorioValeIngressoTroca>(strQuery, new { parametro = parametro }).ToList();
            return result;
        }

        public List<RelatorioTaxaEntrega> ConsultarRelatorioTaxaEntrega(string MesAno)
        {
            string parametro = MesAno + "%";
            #region strQuery
            string strQuery = @"SELECT e.Nome AS TaxaEntrega,isnull (a.Nome, ' ') AS Area,
                                    COUNT(vb.ID) AS Quantidade ,dbo.NumeroFormatado(SUM (vb.TaxaEntregaValor)) as ValorTotalTaxa
                                    FROM tVendaBilheteria vb(NOLOCK) 
                                    INNER JOIN tEntregaControle c (NOLOCK) ON c.ID = vb.EntregaControleID
                                    INNER JOIN tEntrega e (NOLOCK) ON e.ID = c.EntregaID
                                    LEFT JOIN tEntregaArea a (NOLOCK) ON a.ID = c.EntregaAreaID
                                    LEFT JOIN tEntregaPeriodo p (NOLOCK) ON p.ID = c.PeriodoID
                                    WHERE DataVenda LIKE @parametro
                                    GROUP BY e.Nome,a.Nome";
            #endregion

            var result = conIngresso.Query<RelatorioTaxaEntrega>(strQuery, new { parametro = parametro }).ToList();
            return result;
        }
        #endregion
    }
}