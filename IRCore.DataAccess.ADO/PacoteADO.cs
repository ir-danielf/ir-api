using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IRLib;
using Pacote = IRCore.DataAccess.Model.Pacote;
using PacoteItem = IRCore.DataAccess.Model.PacoteItem;

namespace IRCore.DataAccess.ADO
{
    public class PacoteADO : MasterADO<dbSite>
    {

        public PacoteADO(MasterADOBase ado = null) : base(ado) { }

        public Pacote Consultar(int pacoteID)
        {

            return (from pacote in dbSite.Pacote
                    join nomeclatura in dbSite.NomenclaturaPacote on pacote.NomenclaturaPacoteID equals nomeclatura.IR_NomenclaturaPacoteID
                    where pacote.IR_PacoteID == pacoteID
                    select new PacoteModelQuery()
                    {
                        pacote = pacote,
                        nomenclatura = nomeclatura
                    }).AsNoTracking().FirstOrDefault().toPacote();
        }

        private IQueryable<PacoteItemModelQuery> ConsultaItemComMapeamento()
        {
            return (from item in dbSite.PacoteItem
                    join pacote in dbSite.Pacote on item.PacoteID equals pacote.IR_PacoteID
                    join nomeclatura in dbSite.NomenclaturaPacote on pacote.NomenclaturaPacoteID equals nomeclatura.IR_NomenclaturaPacoteID
                    join preco in dbSite.Preco on item.PrecoID equals preco.IR_PrecoID
                    join apresentacao in dbSite.Apresentacao on preco.ApresentacaoID equals apresentacao.IR_ApresentacaoID
                    join setor in dbSite.Setor on new { setorID = preco.SetorID, apresentacaoID = preco.ApresentacaoID.Value } equals new { setorID = setor.IR_SetorID, apresentacaoID = setor.ApresentacaoID }
                    select new PacoteItemModelQuery()
                    {
                        pacote = pacote,
                        pacoteItem = item,
                        nomenclatura = nomeclatura,
                        preco = preco,
                        setor = setor,
                        apresentacao = apresentacao
                    });
        }

        public List<PacoteRetorno> ListarPorEvento(int eventoId, int canalId = 2, bool isPos = false)
        {
            var diffHour = isPos ? 12 : 2;

            var sql =
@"SELECT DISTINCT
       p.Id AS                               ID,
       p.Nome AS                             Nome,
       ( SELECT SUM(Valor)
         FROM tPreco AS preco(NOLOCK)
              INNER JOIN tPacoteItem AS item(NOLOCK) ON preco.Id = item.precoId
         WHERE item.eventoId = @eventoId
               AND item.pacoteId = p.Id ) AS Valor
FROM tPacote AS p(NOLOCK)
     INNER JOIN tCanalPacote AS cp(NOLOCK) ON cp.pacoteId = p.Id
     INNER JOIN tPacoteItem AS i(NOLOCK) ON p.Id = i.PacoteId
     INNER JOIN tApresentacao AS a(NOLOCK) ON i.apresentacaoId = a.Id
WHERE i.eventoId = @eventoId
      AND cp.canalId = @canalId
      AND a.disponivelVenda = 'T'
      AND GETDATE() < DATEADD(hh, {=diffHour}, a.calcHorario);";

            var pacotes = conIngresso.Query<PacoteRetorno>(sql, new { eventoId = eventoId, canalId = canalId, diffHour = diffHour }).ToList();

            var oBilheteria = new Bilheteria();

            foreach (var pacote in pacotes)
            {
                pacote.TaxaConveniencia = oBilheteria.TaxaConvenienciaPacote(pacote.ID, canalId);
            }

            return pacotes;
        }

        public List<PacoteItem> ListarItens(int pacoteID)
        {
            var sql =
@"SELECT DISTINCT
       Id AS         IR_PacoteItemID,
       precoId AS    PrecoID,
       Quantidade AS Quantidade
FROM tPacoteItem(NOLOCK)
WHERE pacoteId = @pacoteId;";

            var query = conIngresso.Query<PacoteItem>(sql, new { pacoteID }).ToList();

            return query;
        }

        public List<PacoteItem> ListarItensEvento(int eventoid)
        {
            return ConsultaItemComMapeamento().Where(t => t.pacoteItem.EventoID == eventoid).OrderBy(t => t.pacote.Nome).AsNoTracking().ToList().Select(t => t.toPacoteItem()).ToList();
        }
    }
}
