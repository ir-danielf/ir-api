using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace IRCore.DataAccess.ADO.Estrutura
{
    public class MasterADOBase : IDisposable
    {
        protected bool disposed = false;

        private ContextoConexao CON;

        public ContextoConexao con
        {
            get
            {
                if ((CON == null) || (CON.IsDisposed))
                {
                    CON = new ContextoConexao();
                    CON.ProxyHabilitado = _ProxyHabilitado;
                    CON.ReferenciaSobDemandaHabilitado = _ReferenciaSobDemandaHabilitado;
                }
                return CON;
            }
            set
            {
                CON = value;
            }
        }

        private bool _ReferenciaSobDemandaHabilitado = false;
        public bool ReferenciaSobDemandaHabilitado
        {
            get
            {
                return _ReferenciaSobDemandaHabilitado;
            }
            set
            {
                _ReferenciaSobDemandaHabilitado = value;
                if ((CON != null) && (!CON.IsDisposed))
                {
                    CON.ReferenciaSobDemandaHabilitado = value;
                }
            }
        }

        private bool _ProxyHabilitado = false;
        public bool ProxyHabilitado
        {
            get
            {
                return _ProxyHabilitado;
            }
            set
            {
                _ProxyHabilitado = value;
                if ((CON != null) && (!CON.IsDisposed))
                {
                    CON.ProxyHabilitado = value;
                }
            }
        }

        protected IRCoreEntitiesIngresso dbIngresso
        {
            get { return con.dbIngresso; }
            set { con.dbIngresso = value; }
        }

        protected IRCoreEntitiesSite dbSite
        {
            get { return con.dbSite; }
            set { con.dbSite = value; }
        }

        protected DbConnection conIngresso
        {
            get { return con.conIngresso; }
            set { con.conIngresso = value; }
        }

        protected DbConnection conSite
        {
            get { return con.conSite; }
            set { con.conSite = value; }
        }

        bool gerenciaConexao = true;

        public int CanalId { get; set; }

        public MasterADOBase(MasterADOBase ado = null)
        {
            gerenciaConexao = (ado == null);
            if (!gerenciaConexao)
            {
                CON = ado.con;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Libera objetos que a garbage collector gerencia
            }

            // Libera objetos que a garbage collector na gerencia 
            // Open files
            // Open network connections
            // Unmanaged memory
            if ((gerenciaConexao) && (CON != null) && (!CON.IsDisposed))
            {
                CON.Dispose();
            }
            disposed = true;
        }

        public bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MasterADOBase()
        {
            Dispose(false);
        }
    }

    public class PagedModel<T>
    {
        public PagedModel(List<T> result, int size)
        {
            this.List = result;
            this.Count = size;
        }
        public List<T> List { get; set; }
        public int Count { get; set; }
    }
}
