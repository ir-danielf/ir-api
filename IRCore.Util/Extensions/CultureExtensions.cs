using IRCore.Util;
using IRCore.Util.Enumerator;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Routing;

namespace System.Globalization
{
    public static class CultureExtensions
    {

        public static CultureInfo GetNeutralCulture (this CultureInfo value)
        {
            if(value.IsNeutralCulture)
            {
                return value;
            }
            else
            {
                return value.Parent;
            }
        }

        public static string GetNeutralName (this CultureInfo value)
        {
            return value.GetNeutralCulture().Name;
        }

    }
}