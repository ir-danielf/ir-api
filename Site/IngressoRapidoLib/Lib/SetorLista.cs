using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for SetorLista
    /// </summary>
    public class SetorLista : List<Setor>, IOrderedEnumerable<Setor>
    {
        DAL oDAL = new DAL();
        Setor oSetor;

        public SetorLista()
        {
            this.Clear();
        }

        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Apresentações do tipo Apresentação, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        private SetorLista CarregarLista(string clausula)
        {
            string strSql = string.Empty;

            if (clausula != "")
            {
                strSql = "SELECT IR_SetorID, Nome, LugarMarcado, QtdeDisponivel, QuantidadeMapa " +
                            "FROM Setor (NOLOCK) " +
                            "WHERE " + clausula + " ORDER BY Nome";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oSetor = new Setor(Convert.ToInt32(dr["IR_SetorID"].ToString()));
                        oSetor.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        oSetor.LugarMarcado = dr["LugarMarcado"].ToString();
                        oSetor.QtdeDisponivel = Convert.ToInt32(dr["QtdeDisponivel"].ToString());
                        oSetor.QuantidadeMapa = dr["QuantidadeMapa"].ToString() == "" ? 1 : Convert.ToInt32(dr["QuantidadeMapa"].ToString());

                        this.Add(oSetor);
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

        public SetorLista CarregarDadosporApresentacaoID(int id)
        {
            return CarregarLista("(ApresentacaoID = " + id + ")");
        }


        public SetorLista CarregarSetorPrecoMapaEsquematico(int apresentacaoID)
        {
            try
            {

                PrecoLista otPreco = new PrecoLista();
                Preco oPreco;

                StringBuilder stbSql = new StringBuilder();


                stbSql.Append("SELECT Setor.IR_SetorID, Setor.LugarMarcado, Setor.Nome AS SetorNome, IsNull(Setor.AprovadoPublicacao, 0) AS AprovadoPublicacao, ");
                stbSql.Append("Preco.IR_PrecoID, Preco.Nome AS PrecoNome, Preco.Valor AS PrecoValor,QtdeDisponivel, QuantidadeMapa, QuantidadePorCliente, ");
                stbSql.Append("CASE WHEN Setor.PrincipalPrecoID = Preco.IR_PrecoID THEN 1 ELSE 0 END AS Principal ");
                stbSql.Append("FROM SETOR (NOLOCK) INNER JOIN Preco (NOLOCK) ON Preco.ApresentacaoID = Setor.ApresentacaoID AND SetorID = IR_SetorID ");
                stbSql.Append("WHERE Setor.ApresentacaoID =" + apresentacaoID + " AND Pacote = 0 AND Serie = 0 ORDER BY Setor.ID");

                SetorLista setorTemp = new SetorLista();
                using (IDataReader dr = oDAL.SelectToIDataReader(stbSql.ToString()))
                {
                    while (dr.Read())
                    {
                        bool adicionarSetor = setorTemp.Where(c => c.Id == Convert.ToInt32(dr["IR_SetorID"])).Count() == 0;
                        if (adicionarSetor)
                        {
                            otPreco = new PrecoLista();
                            oSetor = new Setor(Convert.ToInt32(dr["IR_SetorID"]));
                            oSetor.Nome = Util.LimparTitulo(dr["SetorNome"].ToString());
                            oSetor.LugarMarcado = dr["LugarMarcado"].ToString();
                            oSetor.ApresentacaoID = apresentacaoID;
                            oSetor.PrecoLista = otPreco;
                            oSetor.QtdeDisponivel = Convert.ToInt32(dr["QtdeDisponivel"]);
                            oSetor.QuantidadeMapa = Convert.ToInt32(dr["QuantidadeMapa"]);
                            oSetor.AprovadoPublicacao = Convert.ToBoolean(dr["AprovadoPublicacao"]);
                            setorTemp.Add(oSetor);
                        }

                        oPreco = new Preco(Convert.ToInt32(dr["IR_PrecoID"]));
                        oPreco.ApresentacaoID = apresentacaoID;
                        oPreco.SetorID = oSetor.Id;
                        oPreco.Nome = Util.LimparTitulo(Convert.ToString(dr["PrecoNome"]));
                        oPreco.Valor = Convert.ToDecimal(dr["PrecoValor"]);
                        oPreco.QuantidadePorCliente = Convert.ToInt32(dr["QuantidadePorCliente"]);
                        oPreco.Principal = Convert.ToBoolean(dr["Principal"]);
                        otPreco.Add(oPreco);
                    }
                }

                this.AddRange(setorTemp.OrderBy(c => c.Nome).ToArray());
                return this;

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
        public SetorLista CarregarSetorPreco(int apresentacaoID)
        {
            try
            {

                PrecoLista otPreco = new PrecoLista();
                Preco oPreco;

                StringBuilder stbSql = new StringBuilder();


                stbSql.Append("SELECT Setor.IR_SetorID, Setor.LugarMarcado, Setor.Nome AS SetorNome, ");
                stbSql.Append("Preco.IR_PrecoID, Preco.Nome AS PrecoNome, Preco.Valor AS PrecoValor,QtdeDisponivel, QuantidadeMapa, QuantidadePorCliente ");
                stbSql.Append("FROM SETOR (NOLOCK) INNER JOIN Preco (NOLOCK) ON Preco.ApresentacaoID = Setor.ApresentacaoID AND SetorID = IR_SetorID ");
                stbSql.Append("WHERE Setor.ApresentacaoID =" + apresentacaoID + "AND Pacote = 0 AND Serie = 0 ORDER BY Setor.ID");

                SetorLista setorTemp = new SetorLista();
                using (IDataReader dr = oDAL.SelectToIDataReader(stbSql.ToString()))
                {
                    while (dr.Read())
                    {
                        bool adicionarSetor = setorTemp.Where(c => c.Id == Convert.ToInt32(dr["IR_SetorID"])).Count() == 0;
                        if (adicionarSetor)
                        {
                            otPreco = new PrecoLista();
                            oSetor = new Setor(Convert.ToInt32(dr["IR_SetorID"]));
                            oSetor.Nome = Util.LimparTitulo(dr["SetorNome"].ToString());
                            oSetor.LugarMarcado = dr["LugarMarcado"].ToString();
                            oSetor.ApresentacaoID = apresentacaoID;
                            oSetor.PrecoLista = otPreco;
                            oSetor.QtdeDisponivel = Convert.ToInt32(dr["QtdeDisponivel"]);
                            oSetor.QuantidadeMapa = Convert.ToInt32(dr["QuantidadeMapa"]);
                            setorTemp.Add(oSetor);
                        }

                        oPreco = new Preco(Convert.ToInt32(dr["IR_PrecoID"]));
                        oPreco.ApresentacaoID = apresentacaoID;
                        oPreco.SetorID = oSetor.Id;
                        oPreco.Nome = Util.LimparTitulo(Convert.ToString(dr["PrecoNome"]));
                        oPreco.Valor = Convert.ToDecimal(dr["PrecoValor"]);
                        oPreco.QuantidadePorCliente = Convert.ToInt32(dr["QuantidadePorCliente"]);
                        otPreco.Add(oPreco);
                    }
                }

                this.AddRange(setorTemp.OrderBy(c => c.Nome).ToArray());
                return this;

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


        #region IOrderedEnumerable<Setor> Members

        public IOrderedEnumerable<Setor> CreateOrderedEnumerable<TKey>(Func<Setor, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return null;
        }

        #endregion
    }

}