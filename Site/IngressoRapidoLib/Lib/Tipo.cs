using CTLib;
using System;
using System.Data;

/// <summary>
/// Summary description for Tipo
/// </summary>

namespace IngressoRapido.Lib
{
    public class Tipo
    {
        public Tipo()
        {
        }

        public Tipo(int id)
        {
            this.id = id;
        }

        DAL oDAL = new DAL();

        private int id;
        public int Id
        {
            get { return id; }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string obs;
        public string Obs
        {
            get { return obs; }
            set { obs = value; }
        }


        public Tipo GetByID(int id)
        {
            string strSql = "SELECT IR_TipoID, Nome, Obs FROM Tipo " +
                            "WHERE (IR_LocalID = " + id + ")";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["IR_TipoID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.Obs = dr["Obs"].ToString();
                    }
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;

            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}