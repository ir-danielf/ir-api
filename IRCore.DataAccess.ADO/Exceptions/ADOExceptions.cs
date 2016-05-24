using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using IRCore.DataAccess.Model;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using IRCore.Util;

namespace IRCore.DataAccess.ADO.Exceptions
{

    public class ADOException : Exception
    {

        public ADOException()
        {
        }

        public ADOException(string message)
            : base(message)
        {
        }

        public ADOException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
