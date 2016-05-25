using CTLib;
using IRLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IngressoRapido.Lib
{
    public class Login
    {
        public int ClienteID { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public char Status { get; set; }
        public string DataCadastro { get; set; }
        public string UltimoAcesso { get; set; }
        public string FacebookUserID { get; set; }
        public string FacebookUserToken { get; set; }
        public string FacebookUserInfos { get; set; }

        DAL oDAL = new DAL();

        private string ChaveCriptografiaLogin = ConfigurationManager.AppSettings["ChaveCriptografiaLogin"];

        public int[] BuscaClienteEmailSenhaWebReduzido(string login, string senha)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Senha, StatusAtual, Ativo, FacebookUserID, FacebookUserToken FROM Login WHERE CPF = '{0}' OR Email = '{1}'", login, login);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Status = Convert.ToChar(dr["StatusAtual"]);
                        this.Senha = dr["Senha"].ToString();
                        this.Ativo = dr["Ativo"].ToString() == "T" ? true : false;
                        this.FacebookUserID = dr["FacebookUserID"].ToString();
                        this.FacebookUserToken = dr["FacebookUserToken"].ToString();
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.Status == Convert.ToChar(IRLib.Cliente.StatusClienteChar.Bloqueado))
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteBloqueado, this.ClienteID);
                if (string.IsNullOrEmpty(this.Senha))
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteSemSenha, this.ClienteID);
                if (!this.Ativo)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.NaoAtivado, this.ClienteID);

                if (this.Senha == IRLib.Criptografia.Crypto.Criptografar(senha, ChaveCriptografiaLogin))
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscaClienteEmailCPFWebReduzido(string email, string cpf)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Email, CPF, FacebookUserID, FacebookUserToken FROM Login WHERE CPF = '{0}' AND Email = '{1}'", cpf, email);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Email = dr["Email"].ToString();
                        this.CPF = dr["CPF"].ToString();
                        this.FacebookUserID = dr["FacebookUserID"].ToString();
                        this.FacebookUserToken = dr["FacebookUserToken"].ToString();
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.Email == email && this.CPF == cpf)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscaClienteEmailWebReduzido(string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Email, FacebookUserID, FacebookUserToken FROM Login WHERE Email = '{0}'", email);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Email = dr["Email"].ToString();
                        this.FacebookUserID = dr["FacebookUserID"].ToString();
                        this.FacebookUserToken = dr["FacebookUserToken"].ToString();
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.Email == email)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscaClienteTokenWebReduzido(string token, string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Email, FacebookUserID FROM Login WHERE FacebookUserToken = '{0}'", token);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Email = dr["Email"].ToString();
                        this.FacebookUserID = dr["FacebookUserID"].ToString();
                        this.FacebookUserToken = token;
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.FacebookUserToken == token && this.Email == email)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscaClienteFacebookIDWebReduzido(string facebookid, string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Email, FacebookUserToken FROM Login WHERE FacebookUserID = '{0}' AND Email = '{1}'", facebookid, email);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Email = dr["Email"].ToString();
                        this.FacebookUserID = facebookid;
                        this.FacebookUserToken = dr["FacebookUserToken"].ToString();
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.FacebookUserID == facebookid && this.Email == email)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscarClienteCPFMobile(string CPF)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID, Email, CPF, FacebookUserID, FacebookUserToken FROM Login WHERE CPF = '{0}'", CPF);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                    {
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"]);
                        this.Email = dr["Email"].ToString();
                        this.CPF = dr["CPF"].ToString();
                        this.FacebookUserID = dr["FacebookUserID"].ToString();
                        this.FacebookUserToken = dr["FacebookUserToken"].ToString();
                    }
                }

                if (this.ClienteID == 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);

                if (this.CPF == CPF)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.InfoIncorreta, this.ClienteID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] CriarLogin()
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                parametros.Add(new SqlParameter("@ClienteID", this.ClienteID));
                parametros.Add(new SqlParameter("@CPF", this.CPF));
                parametros.Add(new SqlParameter("@Senha", IRLib.Criptografia.Crypto.Criptografar(this.Senha, ChaveCriptografiaLogin)));
                parametros.Add(new SqlParameter("@Email", this.Email));
                parametros.Add(new SqlParameter("@Ativo", this.Ativo ? "T" : "F"));
                parametros.Add(new SqlParameter("@StatusAtual", this.Status));
                parametros.Add(new SqlParameter("@DataCadastro", this.DataCadastro));
                parametros.Add(new SqlParameter("@UltimoAcesso", this.UltimoAcesso));

                parametros.Add(new SqlParameter("@FacebookUserID", string.IsNullOrEmpty(this.FacebookUserID) ? string.Empty : this.FacebookUserID));
                parametros.Add(new SqlParameter("@FacebookUserToken", string.IsNullOrEmpty(this.FacebookUserToken) ? string.Empty : this.FacebookUserToken));
                parametros.Add(new SqlParameter("@FacebookUserInfos", string.IsNullOrEmpty(this.FacebookUserInfos) ? string.Empty : this.FacebookUserInfos));


                int ID = Convert.ToInt32(oDAL.Scalar(@"INSERT INTO Login (ClienteID, CPF, Senha, Email, Ativo, StatusAtual, DataCadastro, UltimoAcesso, FacebookUserID, FacebookUserToken, FacebookUserInfos) 
							VALUES (@ClienteID, @CPF, @Senha, @Email, @Ativo, @StatusAtual, @DataCadastro, @UltimoAcesso, @FacebookUserID, @FacebookUserToken, @FacebookUserInfos); 
                            SELECT SCOPE_IDENTITY();", parametros.ToArray()));

                if (ID > 0)
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, this.ClienteID);
                else
                    return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public bool AtualizarLogin()
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();
                StringBuilder set = new StringBuilder();

                set.Append("UPDATE Login SET  ");

                if (!string.IsNullOrEmpty(this.CPF))
                {
                    set.Append("CPF = @CPF, ");
                    parametros.Add(new SqlParameter("@CPF", this.CPF));
                }

                if (!string.IsNullOrEmpty(this.Senha))
                {
                    set.Append("Senha = @Senha, ");
                    parametros.Add(new SqlParameter("@Senha", IRLib.Criptografia.Crypto.Criptografar(this.Senha, ChaveCriptografiaLogin)));
                }

                if (!string.IsNullOrEmpty(this.Email))
                {
                    set.Append("Email = @Email, ");
                    parametros.Add(new SqlParameter("@Email", this.Email));
                }

                if (!string.IsNullOrEmpty(this.FacebookUserID))
                {
                    set.Append("FacebookUserID = @FacebookUserID, ");
                    parametros.Add(new SqlParameter("@FacebookUserID", this.FacebookUserID));
                }

                if (!string.IsNullOrEmpty(this.FacebookUserToken))
                {
                    set.Append("FacebookUserToken = @FacebookUserToken, ");
                    parametros.Add(new SqlParameter("@FacebookUserToken", this.FacebookUserToken));
                }

                if (!string.IsNullOrEmpty(this.FacebookUserInfos))
                {
                    set.Append("FacebookUserInfos = @FacebookUserInfos, ");
                    parametros.Add(new SqlParameter("@FacebookUserInfos", this.FacebookUserInfos));
                }

                set.Append("UltimoAcesso = @UltimoAcesso ");
                parametros.Add(new SqlParameter("@UltimoAcesso", this.UltimoAcesso));

                string where = string.Format("WHERE ClienteID = {0}", this.ClienteID);

                oDAL.Execute(set.ToString() + where, parametros.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscaClienteCNPJreduzido(string CNPJ)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID FROM Login WHERE CNPJ = '{0}'", CNPJ);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                        return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, Convert.ToInt32(dr["ClienteID"]));
                    else
                        return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);
                }
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public int[] BuscarClienteCPFreduzido(string CPF)
        {
            try
            {
                string consulta = string.Format(@"SELECT ClienteID FROM Login WHERE CPF = '{0}'", CPF);

                using (IDataReader dr = oDAL.SelectToIDataReader(consulta))
                {
                    if (dr.Read())
                        return MontaRetornoWeb((int)IRLib.Cliente.Infos.Sucesso, Convert.ToInt32(dr["ClienteID"]));
                    else
                        return MontaRetornoWeb((int)IRLib.Cliente.Infos.ClienteInexistente, 0);
                }
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)IRLib.Cliente.Infos.ErroIndefinido, 0);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public string NovaSenha(int ClienteID, string senha)
        {
            try
            {
                int retorno = oDAL.Execute(@"UPDATE Login SET Ativo = 'T', Senha = '" + IRLib.Criptografia.Crypto.Criptografar(senha, ChaveCriptografiaLogin) + "' WHERE ClienteID = '" + ClienteID + "'");

                using (IDataReader dr = oDAL.SelectToIDataReader(@"SELECT Email FROM Login WHERE ClienteID = " + ClienteID))
                {
                    if (dr.Read())
                        return dr["Email"].ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        private int[] MontaRetornoWeb(int msgCodigo, int ID)
        {
            return new int[] { msgCodigo, ID };
        }
    }
}