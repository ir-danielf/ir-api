using CTLib;
using Google.Api.Maps.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using IRCore.Util;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for Local
    /// </summary>
    public class Local
    {
        public string URLImagem = ConfigurationManager.AppSettings["DiretorioImagensLocais"];
        public Local()
        {
        }

        public Local(int id)
        {
            this.id = id;
        }

        DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private string nome;
        public string Nome
        {
            get { return Util.ToTitleCase(this.nome); }
            set { nome = value; }
        }

        private string endereco;
        public string Endereco
        {
            get { return endereco; }
            set { endereco = value; }
        }

        private string cep;
        public string CEP
        {
            get { return cep; }
            set { cep = value; }
        }

        private string dddtelefone;
        public string DDDTelefone
        {
            get { return dddtelefone; }
            set { dddtelefone = value; }
        }

        private string telefone;
        public string Telefone
        {
            get { return telefone; }
            set { telefone = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string uf;
        public string Uf
        {
            get { return uf; }
            set { uf = value; }
        }

        private string obs;
        public string Obs
        {
            get { return obs; }
            set { obs = value; }
        }


        public int QtdEventos { get; set; }

        private string imagem { get; set; }
        public string Imagem
        {
            get { return URLImagem + imagem; }
            set
            {
                if (value != null && value.Length > 0)
                    imagem = value;
                else
                    imagem = "noimage.gif";
            }
        }

        public string Estado { get; set; }

        public string ComoChegar { get; set; }

        public string Pais { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }


        public Local GetByID(int id)
        {
            string strSql = "SELECT IR_LocalID, Nome, Imagem, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, Obs, ComoChegar, Pais FROM Local " +
                            "WHERE (IR_LocalID = " + id + ")";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["IR_LocalID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.Endereco = Util.LimparTitulo(dr["Endereco"].ToString());
                        this.CEP = Util.LimparTitulo(dr["CEP"].ToString());
                        this.DDDTelefone = Util.LimparTitulo(dr["DDDTelefone"].ToString());
                        this.Telefone = Util.LimparTitulo(dr["Telefone"].ToString());
                        this.Cidade = Util.LimparTitulo(dr["Cidade"].ToString());
                        this.Uf = Util.LimparTitulo(dr["Estado"].ToString());
                        this.Obs = dr["Obs"].ToString();
                        this.Pais = dr["Pais"].ToString();
                        this.ComoChegar = dr["ComoChegar"].ToString();
                        this.Imagem = dr["Imagem"].ToString();
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


        public bool UsarBannerPadrao(int clienteID, string sessionID, int localID, int eventoID)
        {

            try
            {
                string strSql = string.Empty;

                if (localID > 0)
                {
                    strSql = "SELECT BannersPadraoSite " +
                            "FROM Local (NOLOCK) " +
                            "WHERE IR_LocalID = " + localID;

                    return oDAL.Scalar(strSql).ToBoolean();

                }
                else if (eventoID > 0)
                {
                    strSql = "SELECT BannersPadraoSite " +
                             "FROM Evento (NOLOCK) " +
                             "WHERE IR_EventoID = " + eventoID;

                    return oDAL.Scalar(strSql).ToBoolean();
                }
                else
                {
                    strSql = string.Format(@"
                                SELECT COUNT(l.ID)
                                FROM Carrinho c (NOLOCK)
                                INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = c.LocalID
                                WHERE ClienteID = {0} AND SessionID = '{1}' AND BannersPadraoSite = 0 ", clienteID, sessionID);

                    return oDAL.Scalar(strSql).ToInt32() == 0;
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }


        private List<Local> BuscarTodos()
        {
            var strSql = @"SELECT Id, CEP, Endereco, Cidade, Estado FROM INGRESSOSNOVO..tLocal WHERE ( Latitude IS NULL OR Longitude IS NULL )";

            var retorno = new List<Local>();

            try
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos.BuscandoLocais##"));

                oDAL = new DAL();

                using (var dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(new Local
                        {
                            id = dr["Id"].ToInt32(),
                            CEP = dr["CEP"].ToString(),
                            Endereco = dr["Endereco"].ToString(),
                            Cidade = dr["Cidade"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }

                LogUtil.Debug(string.Format("##RoboAtzSite.BuscarTodos.BuscandoLocais.SUCCESS##"));

                return retorno;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                LogUtil.Error(string.Format("##RoboAtzSite.BuscarTodos.BuscandoLocais.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        private List<Local> BuscarTodos_SiteIR()
        {
            var strSql = @"SELECT IR_LocalID, CEP, Endereco, Cidade, Estado FROM Local WHERE ( Latitude IS NULL OR Longitude IS NULL )";

            var retorno = new List<Local>();

            try
            {
                LogUtil.Debug(string.Format("##RoboAtzSite.LocalBuscarTodos_SiteIR.BuscandoLocais##"));

                oDAL = new DAL();

                using (var dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(new Local
                        {
                            id = dr["IR_LocalID"].ToInt32(),
                            CEP = dr["CEP"].ToString(),
                            Endereco = dr["Endereco"].ToString(),
                            Cidade = dr["Cidade"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }

                LogUtil.Debug(string.Format("##RoboAtzSite.LocalBuscarTodos_SiteIR.BuscandoLocais.SUCCESS##"));

                return retorno;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                LogUtil.Error(string.Format("##RoboAtzSite.LocalBuscarTodos_SiteIR.BuscandoLocais.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void AtualizarCoordenadas()
        {
            var retorno = this.BuscarTodos();

            try
            {
                foreach (var item in retorno)
                {
                    LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais## NOME: {0}", item.nome));

                    try
                    {
                        var coordenadas = IRLib.CEP.BuscarCoordenadas(item.CEP.ToCEP(), item.Endereco, item.Cidade, item.Estado);
                        LogUtil.Info(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais.SUCCESS## LATITUDE: {0}, LONGITUDE: {1}", coordenadas.Latitude, coordenadas.Longitude));

                        var strSql = string.Format(@"UPDATE INGRESSOSNOVO..tLocal SET Latitude = '{0}', Longitude = '{1}' WHERE Id = {2}", coordenadas.Latitude, coordenadas.Longitude, item.ID);
                        oDAL.Execute(strSql);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais.EXCEPTION## MSG: {0}", ex.Message), ex);
                    }

                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void AtualizarCoordenadas_SiteIR()
        {
            var retorno = this.BuscarTodos_SiteIR();

            try
            {
                foreach (var item in retorno)
                {
                    LogUtil.Debug(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais_SiteIR## NOME: {0}", item.nome));

                    try
                    {
                        var coordenadas = IRLib.CEP.BuscarCoordenadas(item.CEP.ToCEP(), item.Endereco, item.Cidade, item.Estado);
                        LogUtil.Info(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais_SiteIR.SUCCESS## LATITUDE: {0}, LONGITUDE: {1}", coordenadas.Latitude, coordenadas.Longitude));

                        var strSql = string.Format(@"UPDATE Local SET Latitude = '{0}', Longitude = '{1}' WHERE IR_LocalID = {2}", coordenadas.Latitude, coordenadas.Longitude, item.ID);
                        oDAL.Execute(strSql);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais_SiteIR.EXCEPTION## MSG: {0}", ex.Message), ex);
                    }

                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##RoboAtzSite.AtualizarCoordenadasLocais_SiteIR.EXCEPTION## MSG: {0}", ex.Message), ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}