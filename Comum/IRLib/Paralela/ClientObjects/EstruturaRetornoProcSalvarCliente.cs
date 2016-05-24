using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoProcSalvarCliente
    {
        private int clienteID;

        public int ClienteID
        {
            get { return clienteID; }
            set { clienteID = value; }
        }
        private Cliente.RetornoProcSalvar retornoProcedure;

        public Cliente.RetornoProcSalvar RetornoProcedure
        {
            get { return retornoProcedure; }
            set { retornoProcedure = value; }
        }
    }
}
