using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public class EventosDestaqueBO : MasterBO<EventosDestaqueADO>
    {

        public EventosDestaqueBO(MasterADOBase ado = null) : base(ado) { }


        /// <summary>
        /// Remover Destaque
        /// </summary>
        /// <param name="usuario"></param>
        public void Remover(DestaqueRegiao destaque)
        {
            ado.Remover(destaque);
        }

        /// <summary>
        /// Salvar Destaque
        /// </summary>
        /// <param name="destaque"></param>
        public void Salvar(DestaqueRegiao destaque)
        {
            ado.Salvar(destaque);
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="busca"></param>
        /// <param name="regiaoId"></param>
        /// <returns></returns>
        public List<Evento> ListarIn(int regiaoId,string tipoDestaque, string busca=null)
        {
            List<Evento> lista = new List<Evento>();
            lista = ado.ListarIn(regiaoId, tipoDestaque, busca);
            LocalBO locBO = new LocalBO(ado);
            foreach (Evento evt in lista)
            {
                evt.Local = locBO.Consultar(evt.LocalID) ?? new Local() { Cidade = "Não Registrado" };
            }
            return lista;
        }

        public IPagedList<Evento> ListarIn(int pageNumber, int pageSize, int regiaoId,string tipoDestaque, string busca = null)
        {
            IPagedList<Evento> lista = ado.ListarIn(pageNumber, pageSize, regiaoId,tipoDestaque, busca);
            LocalBO locBO = new LocalBO(ado);
            foreach (Evento evt in lista)
            {
                evt.Local = locBO.Consultar(evt.LocalID) ?? new Local() { Cidade = "Não Registrado" };
            }
            return lista;
        }


        public List<Evento> ListarOut(string busca, int regiaoId,string tipoDestque)
        {
            List<Evento> lista = new List<Evento>();
            lista = ado.ListarOut(regiaoId,tipoDestque, busca);
            LocalBO locBO = new LocalBO(ado);
            foreach (Evento evt in lista)
            {
                evt.Local = locBO.Consultar(evt.LocalID) ?? new Local(){ Cidade = "Não Registrado" };
            }
            return lista;
        }

        public DestaqueRegiao Consultar(int eventoId, int regiaoId, string tipoDestaque)
        {
            return ado.Consultar(eventoId, regiaoId,tipoDestaque);
        }

        public bool AtualizarOrdem(int ID, int Ordem)
        {
            return ado.AtualizarOrdem(ID, Ordem);
        }

        public int MaiorOrdem(enumDestaqueRegiaoTipo tipo)
        {
            return ado.MaiorOrdem(tipo);
        }

        public int getID(int eventoId, int regiaoId, enumDestaqueRegiaoTipo tipoDestaque)
        {
            return ado.getID(eventoId, regiaoId, tipoDestaque);
        }

        public List<Evento> ListarPorTipo(int regiaoID, int tipoID)
        {
            List<Evento> lista = new List<Evento>();
            lista = ado.ListarPorTipo(regiaoID, tipoID);
            LocalBO locBO = new LocalBO(ado);
            foreach (Evento evt in lista)
            {
                evt.Local = locBO.Consultar(evt.LocalID) ?? new Local() { Cidade = "Não Registrado" };
            }
            return lista;
        }

        public List<Evento> ListarDestaquesCarrosselMenu()
        {
            List<Evento> lista = new List<Evento>();
            lista = ado.ListarDestaquesCarrosselMenu();
            LocalBO locBO = new LocalBO(ado);
            foreach (Evento evt in lista)
            {
                evt.Local = locBO.Consultar(evt.LocalID) ?? new Local() { Cidade = "",Nome="" };
            }
            return lista;
        }
    }
}
