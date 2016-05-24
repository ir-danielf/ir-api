using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace IRCore.DataAccess.ADO.Estrutura
{
    public class ContextoConexao : IDisposable
    {
        private static bool AdicionarNolock = true;

        private IRCoreEntitiesIngresso DBIngresso;
        
        public IRCoreEntitiesIngresso dbIngresso { 
            get 
            {
                if (DBIngresso == null)
                {
                    DBIngresso = new IRCoreEntitiesIngresso();
                    DBIngresso.Database.CommandTimeout = 0;
                    DBIngresso.Database.ExecuteSqlCommand("SET DATEFIRST 7");
                    //DBIngresso.Database.Log = s => LogUtil.DebugTitulo("SQL dbIngresso", s);
                    DBIngresso.Configuration.ProxyCreationEnabled = _ProxyHabilitado;
                    DBIngresso.Configuration.LazyLoadingEnabled = _ReferenciaSobDemandaHabilitado;
                    DBIngresso.Configuration.AutoDetectChangesEnabled = false;
                    DBIngresso.Configuration.ValidateOnSaveEnabled = false;
                    if (AdicionarNolock)
                    {
                        DbInterception.Add(new NoLockInterceptor());
                    }
                    
                    
                }
                return DBIngresso;
            } 
            set 
            {
                DBIngresso = value;
            } 
        }

        private IRCoreEntitiesSite DBSite;
        public IRCoreEntitiesSite dbSite
        {
            get
            {
                if (DBSite == null)
                {
                    DBSite = new IRCoreEntitiesSite();
                    DBSite.Database.CommandTimeout = 0;
                    DBSite.Database.ExecuteSqlCommand("SET DATEFIRST 7");
                    //DBSite.Database.Log = s => LogUtil.Debug("SQL dbSite", s);
                    DBSite.Configuration.ProxyCreationEnabled = _ProxyHabilitado;
                    DBSite.Configuration.LazyLoadingEnabled = _ReferenciaSobDemandaHabilitado;
                    DBSite.Configuration.AutoDetectChangesEnabled = false;
                    DBSite.Configuration.ValidateOnSaveEnabled = false;
                    if (AdicionarNolock)
                    {
                        DbInterception.Add(new NoLockInterceptor());
                    }
                }
                return DBSite;
            }
            set
            {
                DBSite = value;
            }
        }


        private DbConnection CONIngresso;

        public DbConnection conIngresso
        {
            get
            {
                if (CONIngresso == null)
                {
                    string conString = ConfiguracaoAppUtil.Get("Conexao");
                    CONIngresso = new SqlConnection(conString);
                    CONIngresso.Execute("SET DATEFIRST 7");
                }
                return CONIngresso;
            }
            set
            {
                CONIngresso = value;
            }
        }

        private DbConnection CONSite;

        public DbConnection conSite
        {
            get
            {
                if (CONSite == null)
                {
                    string conString = ConfigurationManager.ConnectionStrings["strConn"].ConnectionString;
                    CONSite = new SqlConnection(conString);
                    CONSite.Execute("SET DATEFIRST 7");
                }
                return CONSite;
            }
            set
            {
                CONSite = value;
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
                if (DBSite != null)
                {
                    DBSite.Configuration.ProxyCreationEnabled = _ProxyHabilitado;
                }
                if (DBIngresso != null)
                {
                    DBIngresso.Configuration.ProxyCreationEnabled = _ProxyHabilitado;
                }
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
                if (DBSite != null)
                {
                    DBSite.Configuration.LazyLoadingEnabled = _ReferenciaSobDemandaHabilitado;
                }
                if (DBIngresso != null)
                {
                    DBIngresso.Configuration.LazyLoadingEnabled = _ReferenciaSobDemandaHabilitado;
                }
            }
        }

        public ContextoConexao() {}

        protected bool disposed = false;
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
            
            if(DBIngresso != null)
            {
                try
                {
                    DBIngresso.Dispose();
                }
                catch { }
                
            }
            if(DBSite != null)
            {
                try
                {
                    DBSite.Dispose();
                }
                catch { }
                
            }
            if(CONIngresso != null)
            {
                try
                {
                    CONIngresso.Close();
                    CONIngresso.Dispose();
                }
                catch { }
                
            }
            if (CONSite != null)
            {
                try
                {
                    CONSite.Close();
                    CONSite.Dispose();
                }
                catch { }
                
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

        ~ContextoConexao()
        {
            Dispose(false);
        }
    }
}
