namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tApresentacao
    {
        public DateTime HorarioAsDateTime
        {
            get 
            {
                try
                {
                    return DateTime.ParseExact(Horario, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    return DateTime.Now;
                }
                
            }
            set { Horario = value.ToString("yyyyMMddHHmmss"); }
        }
    }
}
