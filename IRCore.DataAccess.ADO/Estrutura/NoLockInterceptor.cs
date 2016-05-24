using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Estrutura
{
    public class NoLockInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex =
            new Regex(@"(?<tableAlias>\] AS \[Extent\d+\](?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

        [ThreadStatic]
        public static bool SuppressNoLock;

        public override void ScalarExecuting(DbCommand command,
            DbCommandInterceptionContext<object> interceptionContext)
        {
            if (!SuppressNoLock)
            {
                command.CommandText =
                    _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (!SuppressNoLock)
            {
                command.CommandText =
                    _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }
    }
}
