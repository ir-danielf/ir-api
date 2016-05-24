using System;
using System.Reflection;
using IRCore.DataAccess.ADO.Estrutura;
using IRLib.Emails;

namespace IRCore.BusinessObject.Estrutura
{
    public class MasterBO<T> : IDisposable where T : MasterADOBase
    {
        private MasterADOBase ADO;

        public T ado
        {
            get
            {
                return (T)ADO;
            }
            set
            {
                ADO = value;
            }
        }

        private MailServiceSoapClient mail;

        public MailServiceSoapClient Mail
        {
            get
            {
                if (mail == null)
                {
                    mail = new MailServiceSoapClient();
                }
                return mail;
            }
            set
            {
                mail = value;
            }
        }

        public int CanalId { get; private set; }

        public MasterBO(MasterADOBase ADO = null)
        {
            Type type = typeof(T);
            ConstructorInfo ctor = type.GetConstructor(new[] { typeof(MasterADOBase) });
            ado = (T)ctor.Invoke(new object[] { ADO });
        }

        public MasterBO(MasterADOBase ADO, int canalId)
        {
            Type type = typeof(T);
            ConstructorInfo ctor = type.GetConstructor(new[] { typeof(MasterADOBase) });
            ado = (T)ctor.Invoke(new object[] { ADO });
            CanalId = ado.CanalId = canalId;
        }

        protected bool disposed;

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
            ado.Dispose();

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

        ~MasterBO()
        {
            Dispose(false);
        }

    }
}
