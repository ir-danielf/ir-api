using CTLib;
using System;
using System.Data;
using System.Data.SqlClient;

namespace IngressoRapido.Lib
{
    public class Cliente
    {

        #region * Fields
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

        private string sexo;
        public string Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }

        private DateTime dtNasc;
        public DateTime DtNasc
        {
            get { return dtNasc; }
            set { dtNasc = value; }
        }

        private string rg;
        public string Rg
        {
            get { return rg; }
            set { rg = value; }
        }

        private string cpf;
        public string Cpf
        {
            get { return cpf; }
            set { cpf = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string foneRes;
        public string FoneRes
        {
            get { return foneRes; }
            set { foneRes = value; }
        }

        private string foneCel;
        public string FoneCel
        {
            get { return foneCel; }
            set { foneCel = value; }
        }

        private string cartEstudante;
        public string CartEstudante
        {
            get { return cartEstudante; }
            set { cartEstudante = value; }
        }

        private string end;
        public string End
        {
            get { return end; }
            set { end = value; }
        }

        private string endTipo;
        public string EndTipo
        {
            get { return endTipo; }
            set { endTipo = value; }
        }

        private string endComp;
        public string EndComp
        {
            get { return endComp; }
            set { endComp = value; }
        }

        private string endNum;
        public string EndNum
        {
            get { return endNum; }
            set { endNum = value; }
        }

        private string bairro;
        public string Bairro
        {
            get { return bairro; }
            set { bairro = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string cep;
        public string Cep
        {
            get { return cep; }
            set { cep = value; }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        private bool optIn;

        public bool OptIn
        {
            get { return optIn; }
            set { optIn = value; }
        }

        #endregion

        #region *vars

        DAL oDAL = new DAL();
        SiteCrypto oSiteCrypto = new SiteCrypto();

        #endregion

        #region * Methods

        public bool Autenticar(string login, string senha)
        {
            string strSql = "SELECT IDATENDENTE, NOME, EMAIL, LOGIN, SENHA " +
                            "FROM ATENDENTES " +
                            "WHERE (LOGIN = '" + login + "') AND (SENHA = '" + senha + "')";
            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                this.id = Convert.ToInt32(dr["IDATENDENTE"].ToString());

                // Fecha conexão da classe DataAcess
                oDAL.ConnClose();

                //oCliente = GetbyId(oCliente.Id);
                //DAL.ConnClose();
            }
            catch (SqlException ex)
            {
                oDAL.ConnClose();
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
                //exception geral
            }
            finally
            {
                oDAL.ConnClose();
            }
            return true;
        }

        public Cliente GetbyId(int id)
        {
            // Verifica e Retorna os Dados  atuais da promoção
            //string strSql = "SELECT AT.NOME, AT.EMAIL, ATD_COMP.CPF, ATD_COMP.CEP, ATD_COMP.TIPO, ATD_COMP.ENDERECO, ATD_COMP.COMPLEMENTO, ATD_COMP.NUMERO, ATD_COMP.BAIRRO, ATD_COMP.CIDADE, ATD_COMP.ESTADO, ATD_COMP.TELRES, ATD_COMP.TELCEL, ATD_COMP.RG, ATD_COMP.ESTUDANTE, ATD_COMP.SEXO, ATD_COMP.DTNASC FROM ATENDENTES_COMPLEMENTO AS ATD_COMP INNER JOIN ATENDENTES AS AT ON ATD_COMP.IDATENDENTE = AT.IDATENDENTE WHERE (AT.IDATENDENTE = '" + id + "')";
            string strSql = "SELECT AT.IDATENDENTE, AT.NOME, AT.EMAIL, ATD_COMP.CPF, ATD_COMP.CEP, ATD_COMP.TIPO, ATD_COMP.ENDERECO, ATD_COMP.COMPLEMENTO, ATD_COMP.NUMERO, " +
                            "ATD_COMP.BAIRRO, ATD_COMP.CIDADE, ATD_COMP.ESTADO, ATD_COMP.TELRES, ATD_COMP.TELCEL, ATD_COMP.RG, ATD_COMP.ESTUDANTE, " +
                            "ATD_COMP.SEXO, ATD_COMP.DTNASC " +
                            "FROM ATENDENTES_COMPLEMENTO AS ATD_COMP INNER JOIN " +
                            "ATENDENTES AS AT ON ATD_COMP.IDATENDENTE = AT.IDATENDENTE " +
                            "WHERE (AT.IDATENDENTE = " + id + ")";

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                this.id = Convert.ToInt32(dr["IDATENDENTE"].ToString());

                this.bairro = dr["BAIRRO"].ToString();
                this.cartEstudante = dr["ESTUDANTE"].ToString();
                this.cep = dr["CEP"].ToString();
                this.cidade = dr["CIDADE"].ToString();
                this.cpf = dr["CPF"].ToString();
                this.dtNasc = DateTime.Parse(dr["DTNASC"].ToString());
                this.email = oSiteCrypto.Cripto(dr["EMAIL"].ToString(), false);
                this.end = dr["ENDERECO"].ToString();
                this.endComp = dr["COMPLEMENTO"].ToString();
                this.endNum = dr["NUMERO"].ToString();
                this.endTipo = dr["TIPO"].ToString();
                this.estado = dr["ESTADO"].ToString();
                this.foneCel = dr["TELRES"].ToString();
                this.foneRes = dr["TELCEL"].ToString();

                this.nome = oSiteCrypto.Cripto(dr["NOME"].ToString(), false);

                this.rg = dr["RG"].ToString();
                this.sexo = dr["SEXO"].ToString();

                // Fecha conexão da classe DataAcess
                oDAL.ConnClose();

            }
            catch (SqlException ex)
            {
                oDAL.ConnClose();
                throw ex;
            }
            catch (Exception ex)
            {
                //exception geral
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
            return this;
        }

        public int Update()
        {
            #region Parametros

            SqlParameter[] parametros = new SqlParameter[18];

            parametros[0] = new SqlParameter("@ID", SqlDbType.Int);
            parametros[0].Value = this.id;

            parametros[1] = new SqlParameter("@NOME", SqlDbType.NVarChar, 200);
            parametros[1].Value = oSiteCrypto.Cripto(this.nome, true);

            parametros[2] = new SqlParameter("@SEXO", SqlDbType.NVarChar, 200);
            parametros[2].Value = this.sexo;

            parametros[3] = new SqlParameter("@DTNASC", SqlDbType.SmallDateTime);
            parametros[3].Value = this.dtNasc;

            parametros[4] = new SqlParameter("@RG", SqlDbType.NVarChar, 200);
            parametros[4].Value = this.rg;

            parametros[5] = new SqlParameter("@CPF", SqlDbType.NVarChar, 200);
            parametros[5].Value = this.cpf;

            parametros[6] = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 200);
            parametros[6].Value = oSiteCrypto.Cripto(this.email, true);

            parametros[7] = new SqlParameter("@TELRES", SqlDbType.NVarChar, 200);
            parametros[7].Value = this.foneRes;

            parametros[8] = new SqlParameter("@TELCEL", SqlDbType.NVarChar, 200);
            parametros[8].Value = this.foneCel;

            parametros[9] = new SqlParameter("@ESTUDANTE", SqlDbType.NVarChar, 200);
            parametros[9].Value = this.cartEstudante;

            parametros[10] = new SqlParameter("@CEP", SqlDbType.NVarChar, 200);
            parametros[10].Value = this.cep;

            parametros[11] = new SqlParameter("@TIPO", SqlDbType.NVarChar, 200);
            parametros[11].Value = this.endTipo;

            parametros[12] = new SqlParameter("@ENDERECO", SqlDbType.NVarChar, 200);
            parametros[12].Value = this.end;

            parametros[13] = new SqlParameter("@NUMERO", SqlDbType.NVarChar, 200);
            parametros[13].Value = this.endNum;

            parametros[14] = new SqlParameter("@COMPLEMENTO", SqlDbType.NVarChar, 200);
            parametros[14].Value = this.endComp;

            parametros[15] = new SqlParameter("@BAIRRO", SqlDbType.NVarChar, 200);
            parametros[15].Value = this.bairro;

            parametros[16] = new SqlParameter("@CIDADE", SqlDbType.NVarChar, 200);
            parametros[16].Value = this.cidade;

            parametros[17] = new SqlParameter("@ESTADO", SqlDbType.NVarChar, 200);
            parametros[17].Value = this.estado;

            #endregion

            string[] arraystrSql = new string[2];// = "UPDATE ATENDENTES SET NOME = @NOME  WHERE IDATENDENTE = @ID";
            arraystrSql[0] = "UPDATE ATENDENTES SET NOME = @NOME, EMAIL = @EMAIL WHERE IDATENDENTE = @ID";
            arraystrSql[1] = "UPDATE ATENDENTES_COMPLEMENTO SET " +
                        "SEXO = @SEXO, " +
                        "DTNASC = @DTNASC, " +
                        "RG = @RG, " +
                        "CPF = @CPF, " +
                        "TELRES = @TELRES, " +
                        "TELCEL = @TELCEL, " +
                        "ESTUDANTE = @ESTUDANTE, " +
                        "CEP = @CEP, " +
                        "TIPO = @TIPO, " +
                        "ENDERECO = @ENDERECO, " +
                        "NUMERO = @NUMERO, " +
                        "COMPLEMENTO = @COMPLEMENTO, " +
                        "BAIRRO = @BAIRRO, " +
                        "CIDADE = @CIDADE, " +
                        "ESTADO = @ESTADO " +
                // Implementar campos de atualizacao q faltam
                     " WHERE IDATENDENTE = @ID";

            int status = oDAL.Execute(arraystrSql, parametros);
            return status;
        }

        #endregion

    }
}