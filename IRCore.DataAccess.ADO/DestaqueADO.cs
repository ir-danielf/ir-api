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
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class DestaqueADO : MasterADO<dbSite>
    {
        public DestaqueADO(MasterADOBase ado = null) : base(ado) { }

        public DestaqueConceitual Consultar(int destaqueId)
        {
            return ConsultarQuery(destaqueId).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<DestaqueConceitual> ConsultarQuery(int destaqueId)
        {
            return (from item in dbSite.DestaqueConceitual.Include(t=>t.DestaqueConceitualRegiao)
                    where item.ID == destaqueId
                   select item);
        }


        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="enumvoucherstatus"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<DestaqueConceitual> Listar(int pageNumber, int pageSize, string busca = null, int destaqueId = 0)
        {
            var teste = ListarQuery(busca, destaqueId).AsNoTracking().ToPagedList(pageNumber, pageSize);
            return teste;
        }

        /// <summary>
        /// Query para geração da Lista
        /// </summary>
        /// <param name="status"></param>
        /// <param name="busca"></param>
        /// <param name="parceiroMidiaId"></param>
        /// <returns></returns>
        private IQueryable<DestaqueConceitual> ListarQuery(string busca = null, int destaqueId = 0)
        {

            IQueryable<DestaqueConceitual> result = (from dcr in dbSite.DestaqueConceitual
                                                         .Include(t => t.DestaqueConceitualRegiao)
                                                         .Include(t => t.DestaqueConceitualRegiao.Select(x=>x.Regiao))
                                                      orderby dcr.DestaqueConceitualRegiao.Select(x=>x.Ordem).FirstOrDefault()
                                                      select dcr);
            // Filtrar por busca
            if (!string.IsNullOrEmpty(busca))
            {
                result = result.Where(t => (t.Titulo.Contains(busca))
                                       || (t.Subtitulo.Contains(busca))
                                       || (t.LinkURL.Contains(busca)));

            }
            if (destaqueId > 0)
            {
                result = result.Where(t => t.ID == destaqueId);
            }
            return result;
        }

        public int getNewOrdem(int regiaoID)
        {
            int? value = dbSite.DestaqueConceitualRegiao.Where(t => t.RegiaoID == regiaoID).Max(t => (int?)t.Ordem);
            return Convert.ToInt32(value) + 1;
        }

        public bool AlterarOrdem(string[] IDs,int regiaoID)
        {
            var Ativos = dbSite.DestaqueConceitualRegiao.Where(t => t.RegiaoID == regiaoID && t.DestaqueConceitual.Publicado);
            var N_Ativos = dbSite.DestaqueConceitualRegiao.Where(t => t.RegiaoID == regiaoID && !t.DestaqueConceitual.Publicado).ToList();
            try
            {
                for (int i=0; i < IDs.Length; i++)
                {
                    int temp_id = Convert.ToInt32(IDs[i]);
                    var temp = Ativos.Where(t => t.DestaqueConceitualID == temp_id).FirstOrDefault();
                    temp.Ordem = i;
                    Salvar(temp);
                }
                for (int i=0; i < N_Ativos.Count; i++)
                {
                    var temp = N_Ativos[i];
                    temp.Ordem = IDs.Length + i;
                    Salvar(temp);
                }
                return true;
            }catch(Exception e){
                LogUtil.Error(e);
                return false;
            }
        }
        public List<DestaqueConceitual> Listar(int regiaoID)
        {
            var result = (from dcr in dbSite.DestaqueConceitualRegiao
                          join dc in dbSite.DestaqueConceitual on dcr.DestaqueConceitualID equals dc.ID
                          where dcr.RegiaoID == regiaoID
                          orderby dcr.Ordem
                          select dc);
            return result.AsNoTracking().ToList();
        }
        public List<DestaqueConceitual> Listar(int regiaoID, bool publicados)
        {
            var result = (from dcr in dbSite.DestaqueConceitualRegiao
                          join dc in dbSite.DestaqueConceitual on dcr.DestaqueConceitualID equals dc.ID
                          where dcr.RegiaoID == regiaoID && dc.Publicado == publicados
                          orderby dcr.Ordem
                          select dc);
            return result.AsNoTracking().ToList();
        }
    }
}
