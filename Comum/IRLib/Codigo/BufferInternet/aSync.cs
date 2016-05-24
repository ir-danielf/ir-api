using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IRLib
{
    public abstract class aSync<T>
    {
        public abstract int ID { get; set; }
        public virtual int ApresentacaoID { get; set; }
        public virtual List<SqlParameter> Parameters { get; set; }
        public virtual List<SqlParameter> setParameters()
        {
            return this.Parameters;
        }

        public virtual void AssignParameter(string field, SqlDbType type, object value)
        {
            this.Parameters.Add(new SqlParameter
            {
                Value = value,
                SqlDbType = type,
                ParameterName = field
            });
        }

        public abstract bool CompareIt(T item);

        public virtual string UpdateSQL { get; private set; }
        public virtual string InsertSQL { get; private set; }

    }
}
