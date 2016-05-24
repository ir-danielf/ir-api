namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;

    public partial class NewsAssinante
    {

        public enumNewsAssinantes StatusAsEnum
        {
            get { return (enumNewsAssinantes)Status; }
            set { Status = ((int)value); }
        }
    }
}
