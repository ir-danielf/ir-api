using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public static class IngressoExtensions
    {
        public static Carrinho toCarrinho(this tIngresso ing,decimal taxaConvenienciaValor)
        {
            return new Carrinho()
            {
                CodigoBarra = ing.CodigoBarra,
                ApresentacaoDataHora = ing.tApresentacao.Horario,
                ApresentacaoID = ing.ApresentacaoID,
                //Converte tApresentacao para Apresentacao
                ApresentacaoObject = ing.tApresentacao.toApresentacao(),
                Classificacao = ing.Classificacao,
                ClienteID = ing.ClienteID,
                Codigo = ing.Codigo,
                CodigoProgramacao = ing.tApresentacao.CodigoProgramacao,
                Evento = ing.tEvento.Nome,
                EventoID = ing.EventoID,
                //Converte tEventop para Evento
                EventoObject = ing.tEvento.toEvento(),
                FilmeID = ing.tEvento.FilmeID ?? 0,
                GerenciamentoIngressosID = ing.GerenciamentoIngressosID,
                Grupo = ing.Grupo,
                ID = ing.ID,
                IngressoID = ing.ID,
                Local = ing.tLocal.Nome,
                LocalID = ing.LocalID,
                LugarID = ing.LugarID,
                PacoteGrupo = ing.PacoteGrupo,
                PacoteID = ing.PacoteID,
                PrecoExclusivoCodigoID = ing.PrecoExclusivoCodigoID,
                PrecoID = ing.PrecoID,
                PrecoNome = ing.tPreco.Nome,
                Precos = new List<Preco>(),
                PrecoValor = ing.tPreco.Valor,
                SerieID = ing.SerieID,
                SessionID = ing.SessionID,
                Setor = ing.tSetor.Nome,
                SetorID = ing.SetorID,
                //Converte tSetor para Setor
                SetorObject = ing.tSetor.toSetor(),
                Status = ing.Status,
                TimeStamp = ing.TimeStampReserva,
                TaxaConveniencia = taxaConvenienciaValor,
                VendaBilheteriaID = ing.VendaBilheteriaID,
                
                //TODO: Verificar alguns itens quando carregar os VIRs
                Acumulativo = string.Empty,
                CodigoPromocional = string.Empty,
                CotaItemID = null,
                CotaItemIDAPS = null,
                CotaItemObject = null,
                CotaVerificada = null,
                DonoCPF = string.Empty,
                DonoID = null,
                EmpresaID = null,
                IsSpecial = null,
                LocalImagemNome = string.Empty,
                NVendeLugar = false,
                PacoteNome = string.Empty,
                PossuiTaxaProcessamento = false,
                TagOrigem = string.Empty,
                TipoLugar = string.Empty,
                ValeIngressoID = null,
                ValeIngressoNome = string.Empty,
                ValeIngressoTipoID = null,
                ValidadeData = string.Empty,
                ValidadeDiasImpressao = null,
                ValorTaxaProcessamento =  null,
                VoucherID = null,
            };
        }
    }
}
