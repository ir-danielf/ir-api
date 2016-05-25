using CTLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace IngressoRapido.Lib
{
    public class BannerLista : List<Banner>
    {
        DAL oDAL = new DAL();
        Banner oBanner;


        public BannerLista()
        {
            this.Clear();
        }

        private BannerLista CarregarLista(string clausula)
        {
            string strSql = String.Empty;

            if (clausula != "")
            {
                strSql = "SELECT ID, Nome, Url, Alt, Img, Target, Localizacao, Posicao, Descricao " +
                         "FROM Banner (NOLOCK) " +
                         "WHERE " + clausula + " ORDER BY Localizacao, Posicao";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oBanner = new Banner(Convert.ToInt32(dr["ID"].ToString()));
                        oBanner.Nome = dr["Nome"].ToString();
                        oBanner.Alt = dr["Alt"].ToString();
                        oBanner.Img = this.ForcarFileName(dr["Img"].ToString());
                        oBanner.Url = dr["Url"].ToString();
                        oBanner.Target = Convert.ToInt32(dr["Target"].ToString());
                        oBanner.Localizacao = Convert.ToInt32(dr["Localizacao"].ToString());
                        oBanner.Posicao = Convert.ToInt32(dr["Posicao"].ToString());
                        oBanner.Descricao = dr["Descricao"].ToString();

                        this.Add(oBanner);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
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
        /// <summary>
        /// Criado por : Caio Maganha Rosa
        /// Data: 14/09/2009
        /// Utilização: Força o FileName e retorna para utilização de links relativos na integração HSBC
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        //TODO: Retirar daqui, alterar no Banco de Dados para a imagem passar diretamente o nome, sem o path Absoluto
        public string ForcarFileName(string path)
        {
            try
            {
                string FileName = (ConfigurationManager.AppSettings["DiretorioImagensBanners"] + System.IO.Path.GetFileName(path));
                return FileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BannerLista CarregarBanners()
        {
            return CarregarLista("1=1");
        }

        public BannerLista CarregarBannersLaterais()
        {
            return CarregarLista("Localizacao=2");
        }

        public BannerLista CarregarBannerTopo()
        {
            return CarregarLista("Localizacao=1");
        }

        public BannerLista CarregarBannerTopoMobile()
        {
            return CarregarLista("Localizacao=6");
        }

        public BannerLista CarregaBannerTopoPrefeitura()
        {
            return CarregarLista("Localizacao=4");
        }

        public BannerLista CarregarBannerRodape()
        {
            return CarregarLista("Localizacao=3");
        }


        public BannerLista CarregarBannerCadastro()
        {
            return CarregarLista("Localizacao=5");
        }

        public string CarregarTV()
        {
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(
                    @"SELECT DISTINCT Nome, Url, Alt, Img, Target, Localizacao, Posicao, Descricao " +
                         "FROM Banner (NOLOCK) " +
                         "WHERE Localizacao = 2 ORDER BY  Posicao "))
                {
                    while (dr.Read())
                        this.Add(new Banner()
                        {
                            Nome = dr["Nome"].ToString(),
                            Alt = dr["Alt"].ToString(),
                            Img = this.ForcarFileName(dr["Img"].ToString()),
                            Url = dr["Url"].ToString(),
                            Target = Convert.ToInt32(dr["Target"].ToString()),
                            Localizacao = Convert.ToInt32(dr["Localizacao"].ToString()),
                            Posicao = Convert.ToInt32(dr["Posicao"].ToString()),
                            Descricao = dr["Descricao"].ToString(),
                        });
                };
                return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.None);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }


        public BannerLista CarregarTVmobile()
        {
            try
            {
                string sql = @"SELECT DISTINCT Nome, Url, Alt, Img, Target, Localizacao, Posicao, Descricao 
                            FROM Banner (NOLOCK)
                            WHERE Localizacao = 6 ORDER BY  Posicao";

                using (IDataReader dr = oDAL.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        this.Add(new Banner()
                        {
                            Nome = dr["Nome"].ToString(),
                            Alt = dr["Alt"].ToString(),
                            Img = this.ForcarFileName(dr["Img"].ToString()),
                            Url = dr["Url"].ToString(),
                            Target = Convert.ToInt32(dr["Target"].ToString()),
                            Localizacao = Convert.ToInt32(dr["Localizacao"].ToString()),
                            Posicao = Convert.ToInt32(dr["Posicao"].ToString()),
                            Descricao = dr["Descricao"].ToString(),
                        });
                    }
                };
                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

    }
}
