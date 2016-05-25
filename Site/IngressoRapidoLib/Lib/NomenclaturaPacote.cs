using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Lib {
    public class NomenclaturaPacote {

        public int ID { get; set; }
        public string Nome { get; set; }
        private DAL oDAL = new DAL();

        public NomenclaturaPacote() {
        }

        public NomenclaturaPacote(int id) {
            GetByID(id);
        }

        public void GetByID(int id) {

            if(id == 0 || id == null) {
                id = 1;
            }

            string strSql = " SELECT [IR_NomenclaturaPacoteID],[Nome] FROM [NomenclaturaPacote] " +
                            " WHERE IR_NomenclaturaPacoteID = " + id;


            try {
                using(IDataReader dr = oDAL.SelectToIDataReader(strSql)) {
                    if(dr.Read()) {
                        this.ID = Convert.ToInt32(dr["IR_NomenclaturaPacoteID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString()).Trim();

                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess

            } catch(Exception ex) {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            } finally {
                oDAL.ConnClose();
            }
        }
    }
}
