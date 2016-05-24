namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class tEvento
    {
        public tApresentacao PrimeiraApresentacao 
        { 
            get
            {
                if (this.tApresentacao != null)
                    return tApresentacao.FirstOrDefault();
                return null;
            }
            }
        public tApresentacao UltimaApresentacao { 
            get{
                if (this.tApresentacao != null)
                    return tApresentacao.LastOrDefault();
                return null;
            }}
        public int QuantidadeApresentacoes 
        { 
            get{
                if (this.tApresentacao != null)
                    return tApresentacao.Count;
                else
                    return 0;
            }
        }
    }
}
