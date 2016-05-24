namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class tUsuario
    {
        public DateTime? ValidoDeAsDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(ValidoDe))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        try
                        {
                            return DateTime.ParseExact(ValidoDe, "yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            return null;
                        }
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
                    ValidoDe = null;
                }
                else
                {
                    ValidoDe = value.Value.ToString("yyyyMMdd");
                }
            }
        }

        public DateTime? ValidoAteAsDateTime
        {
            get 
            {
                if (string.IsNullOrEmpty(ValidoAte))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return DateTime.ParseExact(ValidoAte, "yyyyMMdd", CultureInfo.InvariantCulture); 
                    }
                    catch
                    {
                        return null;
                    }
                    
                }
            }
            set 
            { 
                if(value == null)
                {
                    ValidoAte = null;
                }
                else
                {
                    ValidoAte = value.Value.ToString("yyyyMMdd"); 
                }
            } 
        }


        public bool ValidadeAsBool
        {
            get { return (Validade != "F"); }
            set { Validade = (value) ? "T" : "F"; } 
        }

        public enumUsuarioStatus StatusAsEnum
        {

            get
            {
                if (String.IsNullOrEmpty(Status))
                {
                    return enumUsuarioStatus.bloqueado;
                }
                else
                {
                    return (enumUsuarioStatus)Status[0];
                }
            }
            set { Status = ((char)value).ToString(); }
        }
 

    }
}