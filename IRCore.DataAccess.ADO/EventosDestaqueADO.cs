using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using IRCore.DataAccess.ADO.Estrutura;
using System.Linq.Expressions;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class EventosDestaqueADO : MasterADO<dbSite>
    {

        private List<Evento> resultListarIn;

        public EventosDestaqueADO(MasterADOBase ado = null) : base(ado) { }

        public IPagedList<Evento> Listar(int pageNumber, int pageSize, int regiaoID, string tipoDestaque, string busca = null)
        {
            return ListarQueryOut(regiaoID, tipoDestaque, busca).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<Evento> ListarIn(int pageNumber, int pageSize, int regiaoID, string tipoDestaque,string busca = null)
        {
            return ListarQueryIn(regiaoID, tipoDestaque, busca).AsNoTracking().ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="regiaoId"></param>
        /// <param name="busca"></param>
        /// <returns>Lista filtrada</returns>
        public List<Evento> ListarOut(int regiaoId, string tipoDestaque, string busca = null)
        {
            return ListarQueryOut(regiaoId, tipoDestaque, busca).AsNoTracking().ToList();
        }

        public List<Evento> ListarIn(int regiaoId,string tipoDestaque, string busca = null)
        {
            string sql = @"
                        SELECT
	                        Eve.ID,Eve.IR_EventoID,Eve.Nome,Eve.LocalID,Eve.TipoID,Eve.Release,Eve.Obs,Eve.Imagem,Eve.Destaque,Eve.Prioridade,Eve.EntregaGratuita,Eve.RetiradaBilheteria,Eve.DisponivelAvulso,Eve.Parcelas,Eve.PublicarSemVendaMotivo,Eve.Publicar,Eve.SubtipoID,Eve.DataAberturaVenda,Eve.LocalImagemMapaID,Eve.LocalImagemNome,Eve.EscolherLugarMarcado,Eve.PalavraChave,Eve.ExibeQuantidade,Eve.BannersPadraoSite,Eve.MenorPeriodoEntrega,Eve.FilmeID,Eve.PermiteVendaPacote,Eve.PossuiTaxaProcessamento,Eve.LimiteMaximoIngressosEvento,Eve.LimiteMaximoIngressosEstado,Eve.ImagemDestaque,DesReg.Ordem,
	                        Apr.ID,Apr.IR_ApresentacaoID,Apr.Horario,Apr.EventoID,Apr.UsarEsquematico,Apr.Programacao,Apr.CodigoProgramacao,Apr.CalcDiaDaSemana, Apr.CalcHorario
                        FROM
	                        Evento(NOLOCK) AS Eve JOIN
	                        Apresentacao AS Apr ON Eve.IR_EventoID = Apr.EventoID JOIN
	                        DestaqueRegiao(NOLOCK) AS DesReg ON Eve.IR_EventoID = DesReg.EventoID";
            if (regiaoId == 0)
            {
                sql += " JOIN Regiao AS Reg ON DesReg.RegiaoID = reg.ID WHERE Reg.IsGeral = 1";
            }
            else
            {
                sql += " WHERE DesReg.RegiaoID = @regiaoId";
            }

            if (!String.IsNullOrEmpty(busca))
            {
                sql += " AND (Eve.Nome LIKE '%"+busca+"%' OR Eve.PalavraChave LIKE '%"+busca+"%')";
            }
            sql += " AND DesReg.Tipo = @tipoDestaque";
            resultListarIn = new List<Evento>();
            conSite.Query<Evento, Apresentacao, int>(sql, addresultListarIn, new { regiaoId = regiaoId, tipoDestaque = tipoDestaque });
            return resultListarIn;
        }

        public List<Evento> ListarPorTipo(int regiaoId, int tipoId = 0)
        {
            IQueryable<Evento> listar = (from item in dbSite.DestaqueRegiao
                                         join itemEvento in dbSite.Evento on item.EventoID equals itemEvento.IR_EventoID
                                         orderby itemEvento.Nome
                                         select itemEvento);
            if(tipoId!=0){
                listar = (from item in listar where item.TipoID == tipoId select item);
            }
            return listar.AsNoTracking().ToList();

        }

        public List<Evento> ListarDestaquesCarrosselMenu()
        {
            string tipoDestaque = enumDestaqueRegiaoTipo.menu.ValueAsString();

            IQueryable<Evento> listar = (from item in dbSite.DestaqueRegiao
                                         join itemEvento in dbSite.Evento on item.EventoID equals itemEvento.IR_EventoID
                                         where item.Tipo == tipoDestaque
                                         orderby itemEvento.Nome
                                         select itemEvento);
            return listar.AsNoTracking().ToList();
        }

        public DestaqueRegiao Consultar(int eventoId, int regiaoId, string tipoDestque)
        {
            return dbSite.DestaqueRegiao.Where(x => x.EventoID == eventoId && x.RegiaoID == regiaoId && x.Tipo == tipoDestque).AsNoTracking().FirstOrDefault();
        }

        public bool AtualizarOrdem(int ID, int Ordem)
        {
            string sql = @"UPDATE DestaqueRegiao SET Ordem = @Ordem WHERE ID = @ID";
            return conSite.Execute(sql, new { ID = ID, Ordem = Ordem}) > 0;
        }

        public int MaiorOrdem(enumDestaqueRegiaoTipo tipo)
        {
            try
            {
                return conSite.Query<int>("SELECT MAX(Ordem) FROM DestaqueRegiao(NOLOCK) WHERE tipo = @tipo", new { tipo = tipo.ValueAsString() }).FirstOrDefault();
            }
            catch
            {
                return 0;
            }
                
        }

        public int getID(int eventoId, int regiaoId, enumDestaqueRegiaoTipo tipoDestaque)
        {
            string sql = @"SELECT DestaqueRegiao.ID FROM DestaqueRegiao(NOLOCK)";
            if (regiaoId == 0)
            {
                sql += " JOIN Regiao AS Reg ON DestaqueRegiao.RegiaoID = reg.ID WHERE Reg.IsGeral = 1";
            }
            else
            {
                sql += " WHERE DestaqueRegiao.RegiaoID = @regiaoId";
            }
            sql += " AND DestaqueRegiao.Tipo = @tipoDestaque AND DestaqueRegiao.EventoID = @eventoId";
            return conSite.Query<int>(sql, new { regiaoId = regiaoId, tipoDestaque = tipoDestaque.ValueAsString(), eventoId = eventoId}).FirstOrDefault();
        }

        private int addresultListarIn(Evento eve, Apresentacao apr)
        {
            if(resultListarIn.Count == 0 || resultListarIn.Where(x => x.IR_EventoID == eve.IR_EventoID).Count() == 0)
            {
                eve.Apresentacao.Add(apr);
                resultListarIn.Add(eve);
            }
            else
            {
                resultListarIn.Where(x => x.IR_EventoID == eve.IR_EventoID).FirstOrDefault().Apresentacao.Add(apr);
            }
            return eve.IR_EventoID;
        }

        private IQueryable<Evento> ListarQueryOut(int regiaoId, string tipoDestaque, string busca = null)
        {
            IQueryable<Evento> listar = (from itemE in dbSite.Evento.Include(t => t.Apresentacao)
                                         where !(from itemDR in dbSite.DestaqueRegiao
                                                 where itemDR.RegiaoID == regiaoId
                                                 && itemDR.Tipo == tipoDestaque
                                                 select itemDR.EventoID).Contains(itemE.IR_EventoID)
                                         select itemE);
            if (!String.IsNullOrEmpty(busca))
            {
                listar = (from itemE in listar
                          where (itemE.Nome.ToLower().Contains(busca.ToLower())
                                || itemE.PalavraChave.ToLower().Contains(busca.ToLower()))
                          select itemE);
            }
            return listar.OrderBy(t => t.Nome);
        }

        private IQueryable<Evento> ListarQueryIn(int regiaoId, string tipoDestaque, string busca = null)
        {
            IQueryable<Evento> listar = (from item in dbSite.Evento.Include(t => t.Apresentacao)
                                         where item.DestaqueRegiao.Any(t => t.Tipo == tipoDestaque)
                                         orderby item.Nome
                                         select item);
            if (regiaoId == 0)
            {
                listar = listar.Where(t => t.DestaqueRegiao.Any(x => x.Regiao.IsGeral ?? false));
            }
            else
            {
                listar = listar.Where(t => t.DestaqueRegiao.Any(x => x.RegiaoID == regiaoId));
            }

            if (!String.IsNullOrEmpty(busca))
            {
                listar.Where(t => (t.Nome.ToLower().Contains(busca.ToLower())
                     || t.PalavraChave.ToLower().Contains(busca.ToLower())));
            }
            return listar;

        }

    }
}
