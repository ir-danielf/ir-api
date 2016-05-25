using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IngressoRapido.Lib
{
    public class FaqlLista : List<FaqTipo>
    {
        private DAL oDAL = new DAL();
        Faq faq = new Faq();

        public FaqlLista MontaListaCompleta()
        {
            
            String strSql = "SELECT ID, Pergunta, FaqTipo FROM Faq ORDER BY FaqTipo";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    List<Faq> perguntas = new List<Faq>();
                    while (dr.Read())
                    {
                        if(this.Where(c=> c.Nome == dr["FaqTipo"].ToString()).Count() == 0)
                        {
                            perguntas = new List<Faq>();
                            this.Add(new FaqTipo()
                            {
                                Nome = dr["FaqTipo"].ToString(),
                                Perguntas = perguntas,
                            });
                        }

                        perguntas.Add(new Faq()
                        {
                            ID = dr["ID"].ToInt32(),
                            Pergunta = dr["Pergunta"].ToString(),
                        });
                    }
                }
                return this;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public FaqlLista MontaListaPesquisada(string busca)
        {
            string strSql = "SELECT ID, Pergunta, Resposta,FaqTipo, Tags, Exibicao FROM Faq";
            string filtro = " WHERE  Tags like '%" + busca + "%' OR Pergunta like '%" + busca + "%' OR Resposta like '%" + busca + "%' OR FaqTipo like '%" + busca + "%'";

            if (filtro.Length > 0)
            {
                strSql = strSql + filtro;
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    List<Faq> perguntas = new List<Faq>();
                    while (dr.Read())
                    {
                        if (this.Where(c => c.Nome == dr["FaqTipo"].ToString()).Count() == 0)
                        {
                            perguntas = new List<Faq>();
                            this.Add(new FaqTipo()
                            {
                                Nome = dr["FaqTipo"].ToString(),
                                Perguntas = perguntas,
                            });
                        }

                        perguntas.Add(new Faq()
                        {
                            ID = dr["ID"].ToInt32(),
                            Pergunta = dr["Pergunta"].ToString(),
                        });
                    }
                }
                return this;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public string Resposta(int resp)
        {

            string strSql = "SELECT Resposta FROM Faq where id = " + resp;
            string resposta = string.Empty;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    
                    while (dr.Read())
                    {
                        resposta = dr["Resposta"].ToString();
                    }
                }
                return resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

    }
}
