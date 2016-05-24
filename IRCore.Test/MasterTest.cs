using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.Test
{
    public abstract class MasterTest
    {
        protected bool disposed = false;
        protected MasterADOBase ado;

        public MasterTest() : base()
        {
            ado = new MasterADOBase();
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
            ado.Dispose();

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);   
        }

        ~MasterTest()
        {
            Dispose(false);
        }

    }
}
