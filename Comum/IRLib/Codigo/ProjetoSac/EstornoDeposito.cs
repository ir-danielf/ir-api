using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib
{
    public class EstornoDeposito : EstornoDeposito_B
    {
        public EstornoDeposito()
        {

        }

        public EstornoDeposito(int usuarioID)
            : base(usuarioID)
        {

        }

    }

    public class EstornoDepositoLista : EstornoDeposito_B
    {

    }
}
