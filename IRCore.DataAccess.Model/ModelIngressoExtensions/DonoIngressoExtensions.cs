namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class tDonoIngresso
    {
        public DateTime? DataNascimentoAsDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(DataNascimento))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return DateTime.ParseExact(DataNascimento, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return null;
                    }

                }
            }
            set
            {
                if (value == null)
                {
                    DataNascimento = null;
                }
                else
                {
                    DataNascimento = value.Value.ToString("yyyyMMdd");
                }
            }
        }
    }
}
