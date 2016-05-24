/**************************************************
* Arquivo: CotaItemControle.cs
* Gerado: 26/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IRLib
{

    public class CotaItemControle : CotaItemControle_B
    {
        public CotaItemControle() { }

        /// <summary>
        /// Inserir novo(a) CotaItemControle
        /// </summary>
        /// <returns></returns>	
        /// 

        public int GetCotaItem(int IngressoID)
        {

            string sql = "EXEC sp_GetCotaItemIngresso " + IngressoID.ToString();

            int CotaItemID = 0;

            bd.Consulta(sql);

            while (bd.Consulta().Read())
            {
                CotaItemID = bd.LerInt("CotaItemID");
            }

            return CotaItemID;

        }
        public int Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaItemControleInMemory(ApresentacaoID, ApresentacaoSetorID, Quantidade, CotaItemID) ");
                sql.Append("VALUES (@001,@002,@003,@004)");

                sql.Replace("@001", this.ApresentacaoID.ValorBD);
                sql.Replace("@002", this.ApresentacaoSetorID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.CotaItemID.ValorBD);

                int x = this.bd.Executar(sql.ToString());

                return x;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InserirControladorPorCotaID(BD bd, int cotaID)
        {
            BD bd2 = new BD();
            try
            {
                List<int> ids = new List<int>();
                string strSQL = "SELECT ID FROM tCotaItem (NOLOCK) WHERE CotaID = " + cotaID;

                bd2.Consulta(strSQL);
                while (bd2.Consulta().Read())
                    ids.Add(bd2.LerInt("ID"));


                int c = 0;

                for (int i = 0; i < ids.Count; i++)
                {
                    this.CotaItemID.Valor = ids[i];
                    this.Inserir(bd);
                    c++;
                }

                return (ids.Count - c);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd2.Fechar();
            }
        }

        public int InserirControladorReplicando(BD bd, List<EstruturaCotaItem> lstReplicar)
        {
            BD bd2 = new BD();
            CotaItem oCotaItem = new CotaItem();
            try
            {
                for (int i = 0; i < lstReplicar.Count; i++)
                {
                    int CotaItemID = lstReplicar[i].ID;
                    int qtdCadastrada = 0;
                    qtdCadastrada = oCotaItem.GetCamposCotaPromoCodigoqtd(CotaItemID);


                    if (qtdCadastrada > 0)
                    {
                        this.CotaItemID.Valor = lstReplicar[i].ID;
                        this.Quantidade.Valor = lstReplicar[i].quantidade;
                        this.Inserir(bd);
                    }

                }
                return lstReplicar.Count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd2.Fechar();
            }
        }

        public int IncrementarControladorApresentacao(int apresentacaoId, int cotaItemId)
        {
            var linhas = 0;
            try
            {
                string sql = @"
                                UPDATE cic
                                SET cic.Quantidade = cic.Quantidade + 1
                                FROM tCotaItemControleInMemory cic
                                  INNER JOIN tCotaItem ci ON ci.ID = cic.CotaItemID
                                WHERE ApresentacaoID = @ApresentacaoId AND CotaItemID = @CotaItemId AND cic.Quantidade < ci.Quantidade
                            ";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter("ApresentacaoId", apresentacaoId),
                    new SqlParameter("CotaItemId", cotaItemId)
                };

                linhas = this.bd.Executar(sql, parametros);

                return linhas;
            }
            catch (Exception)
            {
                return linhas;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int DecrementarControladorApresentacao(int apresentacaoId, int cotaItemId)
        {
            var linhas = 0;
            try
            {
                string sql = @"
                                UPDATE cic
                                SET cic.Quantidade = cic.Quantidade - 1
                                FROM tCotaItemControleInMemory cic
                                  INNER JOIN tCotaItem ci ON ci.ID = cic.CotaItemID
                                WHERE ApresentacaoID = @ApresentacaoId AND CotaItemID = @CotaItemId AND ci.Quantidade > 0 AND cic.Quantidade <= ci.Quantidade
                            ";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter("ApresentacaoId", apresentacaoId),
                    new SqlParameter("CotaItemId", cotaItemId)
                };

                linhas = this.bd.Executar(sql, parametros);

                return linhas;
            }
            catch (Exception)
            {
                return linhas;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaCotaItem> ItensParaReplicar(int cotaID, bool apresentacao)
        {

            BD bd2 = new BD();
            try
            {
                List<EstruturaCotaItem> lstCotaItem = new List<EstruturaCotaItem>();
                List<EstruturaCotaItem> lstCotaItemRetornar = new List<EstruturaCotaItem>();
                EstruturaCotaItem item;
                string strSQL = "Select ID, PrecoIniciaCom FROM tCotaItem (NOLOCK) WHERE CotaID = " + cotaID + " ORDER BY PrecoIniciaCom ";

                bd2.Consulta(strSQL);
                while (bd2.Consulta().Read())
                {
                    item = new EstruturaCotaItem();
                    item.ID = bd2.LerInt("ID");
                    item.precoIniciaCom = bd2.LerString("PrecoIniciaCom");
                    lstCotaItem.Add(item);

                }
                bd2.FecharConsulta();
                foreach (EstruturaCotaItem itemNovo in lstCotaItem)
                {

                    item = new EstruturaCotaItem();
                    if (apresentacao)
                        item.quantidade = this.VerificaQuantidadeCotaPrecoApresentacao(bd2, itemNovo.ID);
                    else
                        item.quantidade = this.getQuantidadeJaVendidaAPS(bd2, itemNovo.precoIniciaCom);
                    item.precoIniciaCom = itemNovo.precoIniciaCom;
                    item.ID = itemNovo.ID;
                    item.alterado = true;
                    lstCotaItemRetornar.Add(item);
                }
                return lstCotaItemRetornar;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd2.Fechar();
            }
        }
        public int getQtdJaVendida(int ApresentacaoID, string Nome, int CotaID)
        {
            try
            {
                string sql = string.Format(@"
                                        SELECT COUNT(ti.ID) AS Quantidade FROM tIngresso ti (NOLOCK)
                                        INNER JOIN tApresentacao ta (NOLOCK) ON ti.ApresentacaoID = ta.ID
                                        INNER JOIN tEvento te (NOLOCK) ON ti.EventoID = te.ID
                                        INNER JOIN tPreco tp (NOLOCK) ON ti.PrecoID = tp.ID
                                        WHERE ta.CotaID = {0} AND tp.Nome LIKE '' + {1} + '%' AND ta.ID = {2}", CotaID, Nome, ApresentacaoID);
                bd.Consulta(sql);

                int quantidade = 0;

                while (bd.Consulta().Read())
                {
                    quantidade = bd.LerInt("Quantidade");
                }

                return quantidade;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }


        }

        public int getQuantidadeJaVendidaAPS(BD bd, string precoNome)
        {
            try
            {
                string strSQL = "EXEC sp_getQuantidadeJaVendidaApresentacaoSetor " + this.ApresentacaoSetorID.Valor + ", '" + precoNome + "%'";
                return (Convert.ToInt32(bd.ConsultaValor(strSQL)));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
        }

        public int getQuantidadeJaVendidaAP(BD bd, string precoNome)
        {
            try
            {
                string strSQL = "EXEC sp_getQuantidadeJaVendidaApresentacao " + this.ApresentacaoID.Valor + ", '" + precoNome + "%'";
                return (Convert.ToInt32(bd.ConsultaValor(strSQL)));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
        }



        public int VerificaQuantidadeCotaPrecoApresentacao(BD bd, int ID)
        {
            try
            {
                string strSQL = "EXEC sp_VerificaCotaQuantidadePrecoApresentacao " + ID.ToString() + "," + this.ApresentacaoID.Valor;
                return (Convert.ToInt32(bd.ConsultaValor(strSQL)));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
        }


        /// <summary>
        /// Gera um controlador de Quantidade apartir de uma Cota Antiga (Novo Item adicionado) -
        /// Precisa replicar o item pra todas as apresentacacoes que fazer parte daquela cota
        /// </summary>
        /// <param name="bd"></param>
        public void GerarControladorDeCotaAntiga(BD bd, int cotaID, string precoIniciaCom)
        {
            BD bd2 = new BD();

            try
            {
                StringBuilder stbSQL = new StringBuilder();
                List<EstruturaDistribuicaoCotaAntiga> lstItems = new List<EstruturaDistribuicaoCotaAntiga>();
                EstruturaDistribuicaoCotaAntiga item;
                stbSQL.Append("SELECT IsNull(AP.ID, 0) ApresentacaoID, ");
                stbSQL.Append("CASE WHEN AP.CotaID > 0 THEN 'T' ELSE 'F' END AS TemAP, ");
                stbSQL.Append("IsNull(APS.ID, 0) ApresentacaoSetorID, ");
                stbSQL.Append("CASE  WHEN APS.CotaID > 0 THEN 'T' ELSE 'F' END AS TemAPS ");
                stbSQL.Append("FROM tCota (NOLOCK) ");
                stbSQL.Append("LEFT JOIN tApresentacao AP (NOLOCK) ON AP.CotaID = tCota.ID ");
                stbSQL.Append("LEFT JOIN tApresentacaoSetor APS (NOLOCK) ON APS.CotaID = tCota.ID ");
                stbSQL.Append("WHERE tCota.ID = " + cotaID);
                bd2.Consulta(stbSQL.ToString());
                while (bd2.Consulta().Read())
                {
                    item = new EstruturaDistribuicaoCotaAntiga();

                    item.ApresentacaoID = bd2.LerInt("ApresentacaoID");
                    item.ApresentacaoSetorID = bd2.LerInt("ApresentacaoSetorID");
                    item.TemAP = bd2.LerBoolean("TemAP");
                    item.TemAPS = bd2.LerBoolean("TemAPS");
                    if (item.ApresentacaoID == 0)
                        item.ApresentacaoID = bd2.LerInt("ApresentacaoID");
                    lstItems.Add(item);
                    ////Precisa duplicar pensando no ponto em q foi gerada a mesma cota pra AP e APS (só retorna 1 linha)
                    //if (item.TemAP && item.TemAPS)
                    //    lstItems.Add(item);
                }
                bd2.FecharConsulta();


                int quantidade = 0;
                stbSQL = new StringBuilder();
                foreach (EstruturaDistribuicaoCotaAntiga itemEncontrado in lstItems)
                {
                    if (itemEncontrado.TemAP)
                    {
                        this.ApresentacaoID.Valor = itemEncontrado.ApresentacaoID;
                        quantidade = this.getQuantidadeJaVendidaAP(bd, precoIniciaCom);
                        stbSQL.Append("INSERT INTO tCotaItemControle ");
                        stbSQL.Append("(ApresentacaoID, ApresentacaoSetorID, Quantidade, CotaItemID) VALUES ");
                        stbSQL.Append("( " + itemEncontrado.ApresentacaoID + ", 0, " + quantidade + ", " + this.CotaItemID.Valor + ") ");
                    }
                    if (itemEncontrado.TemAPS)
                    {
                        this.ApresentacaoID.Valor = itemEncontrado.ApresentacaoID;
                        this.ApresentacaoSetorID.Valor = itemEncontrado.ApresentacaoSetorID;
                        quantidade = this.getQuantidadeJaVendidaAPS(bd, precoIniciaCom);
                        stbSQL.Append("INSERT INTO tCotaItemControle ");
                        stbSQL.Append("(ApresentacaoID, ApresentacaoSetorID, Quantidade, CotaItemID) VALUES ");
                        stbSQL.Append("( " + itemEncontrado.ApresentacaoID + ", " + itemEncontrado.ApresentacaoSetorID + ", " + quantidade + ", " + this.CotaItemID.Valor + ") ");
                    }
                }
                if (stbSQL.Length > 0)
                    bd.Executar(stbSQL.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd2.Fechar();
            }
        }

        public void ExcluirControlador(BD bd)
        {
            try
            {
                string strSQL = "DELETE FROM tCotaItemControleInMemory WHERE ApresentacaoID = " + this.ApresentacaoID.Valor + " AND ApresentacaoSetorID = " + this.ApresentacaoSetorID.Valor;
                this.bd.Executar(strSQL);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExcluirControladorPorCotaItemID(BD bd)
        {
            try
            {
                string strSQL = "DELETE FROM tCotaItemControle WHERE CotaItemID = " + this.CotaItemID.Valor;
                bd.Executar(strSQL);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int GetQtdCotaInMemory(int cotaitemID, int apresentacaoID)
        {
            try
            {
                int quantidade = 0;
                string sql = "EXEC sp_getQuantidadePorCotaInMemory " + cotaitemID.ToString() + "," + apresentacaoID.ToString();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    quantidade = bd.LerInt("Quantidade");
                }

                return quantidade;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }

        }

        public int[] getQuantidade(int cotaItemID, int apresentacaoID, int apresentacaoSetorID)
        {
            try
            {
                int[] qtds = new int[3] { 0, 0, 0 };
                string sqlPROC = "EXEC sp_getQuantidadePorCotaReserva " + apresentacaoID + ", " + apresentacaoSetorID + ", " + cotaItemID;
                bd.Consulta(sqlPROC);

                if (bd.Consulta().Read())
                {
                    qtds[0] = bd.LerInt("Apresentacao");
                    qtds[1] = bd.LerInt("ApresentacaoSetor");
                    qtds[2] = bd.LerInt("Cota");
                }
                return qtds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int[] getQuantidadeNovo(int cotaItemID, int CotaItemIDAPS, int apresentacaoID, int apresentacaoSetorID)
        {
            try
            {

                int[] qtds = new int[4] { 0, 0, 0, 0 };
                string sqlPROC = "EXEC sp_getQuantidadePorCotaReservaNovoInMemory " + apresentacaoID + ", " + apresentacaoSetorID + ", " + cotaItemID + ", " + CotaItemIDAPS;
                bd.Consulta(sqlPROC);

                if (bd.Consulta().Read())
                {
                    qtds[0] = bd.LerInt("Apresentacao");
                    qtds[1] = bd.LerInt("ApresentacaoSetor");
                    qtds[2] = bd.LerInt("Cota");
                    qtds[3] = bd.LerInt("CotaAPS");
                }
                return qtds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int getQuantidadeCota(int cotaItemId, int apresentacaoId)
        {
            var qtd = 0;
            try
            {
                string sql = @"
                                SELECT TOP 1 Quantidade FROM tCotaItem (NOLOCK) WHERE ID = @cotaItemId AND @apresentacaoId = @apresentacaoId;
                            ";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter("CotaItemId", cotaItemId),
                    new SqlParameter("ApresentacaoId", apresentacaoId)
                };

                this.bd.Consulta(sql, parametros);

                if (this.bd.Consulta().Read())
                {
                    qtd = bd.LerInt("Quantidade");
                }

                return qtd;
            }
            catch (Exception)
            {
                return qtd;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Metodo que busca as quantidades ja compradas da apresentacao e caso a cota tenha quantidade maxima por cliente
        /// busca oq já foi compradona tIngressoCliente
        /// </summary>
        /// <param name="quantidadePorCliente"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="apresentacaoSetorID"></param>
        /// <param name="ingressoID"></param>
        /// <param name="cotaItemID"></param>
        /// <param name="clienteID"></param>
        /// <returns></returns>
        public EstruturaCotasInfo getQuantidadeEJaCompradaCliente(int quantidadePorCliente, int apresentacaoID, int apresentacaoSetorID, int ingressoID, int cotaItemID, int donoID)
        {
            try
            {
                IngressoCliente oIngressoCliente = new IngressoCliente();
                if (apresentacaoSetorID == 0)
                {
                    apresentacaoSetorID = Convert.ToInt32(bd.ConsultaValor("SELECT ApresentacaoSetorID FROM tIngresso (NOLOCK) WHERE ID = " + ingressoID + " AND ApresentacaoID = " + apresentacaoID));
                    bd.FecharConsulta();
                }
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();

                int[] qtds = this.getQuantidade(cotaItemID, apresentacaoID, apresentacaoSetorID);
                cotaInfo.QuantidadeApresentacao = qtds[0];
                cotaInfo.QuantidadeApresentacaoSetor = qtds[1];

                if (quantidadePorCliente > 0)
                {
                    //Preenche o OBJ de IngressoCliente e retorna as quantidades ja compradas
                    oIngressoCliente.ApresentacaoID.Valor = apresentacaoID;
                    oIngressoCliente.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                    oIngressoCliente.DonoID.Valor = donoID;
                    oIngressoCliente.CotaItemID.Valor = cotaItemID;

                    qtds = oIngressoCliente.QuantidadeJaComprada();

                    cotaInfo.QuantidadePorClienteApresentacao = qtds[0];
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = qtds[1];
                }
                else
                {
                    cotaInfo.QuantidadePorClienteApresentacao = 0;
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = 0;
                }

                return cotaInfo;
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaCotasInfo getQuantidadesComprasInternet(EstruturaCotaItemReserva cota, int apresentacaoID, int apresentacaoSetorID, int ingressoID, int cotaItemID, int donoID)
        {
            try
            {
                IngressoCliente oIngressoCliente = new IngressoCliente();
                if (apresentacaoID == 0)
                {
                    apresentacaoSetorID = Convert.ToInt32(bd.ConsultaValor("SELECT ApresentacaoSetorID FROM tIngresso (NOLOCK) WHERE ID = " + ingressoID + " AND ApresentacaoID = " + apresentacaoID));
                    bd.FecharConsulta();
                }
                EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();

                int[] qtds = this.getQuantidade(cotaItemID, apresentacaoID, apresentacaoSetorID);
                cotaInfo.QuantidadeApresentacao = qtds[0];
                cotaInfo.QuantidadeApresentacaoSetor = qtds[1];
                cotaInfo.QuantidadePorClienteCota = qtds[2];

                if (cota.QuantidadePorCliente > 0 || cota.QuantidadePorClienteApresentacao > 0 || cota.QuantidadePorClienteApresentacaoSetor > 0)
                {
                    //Preenche o OBJ de IngressoCliente e retorna as quantidades ja compradas
                    oIngressoCliente.ApresentacaoID.Valor = apresentacaoID;
                    oIngressoCliente.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                    oIngressoCliente.DonoID.Valor = donoID;
                    oIngressoCliente.CotaItemID.Valor = cotaItemID;

                    qtds = oIngressoCliente.QuantidadeJaComprada();

                    cotaInfo.QuantidadePorClienteApresentacao = qtds[0];
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = qtds[1];
                    cotaInfo.QuantidadePorClienteCota = qtds[2];
                }
                else
                {
                    cotaInfo.QuantidadePorClienteApresentacao = 0;
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = 0;
                    cotaInfo.QuantidadePorClienteCota = 0;
                }

                return cotaInfo;
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaCotasInfo> ValidacaoQuantidadesCliente(List<EstruturaCotaValidacaoCliente> lstCotas)
        {
            IngressoCliente oIngressoCliente = new IngressoCliente();
            List<EstruturaCotasInfo> lstRetorno = new List<EstruturaCotasInfo>();
            EstruturaCotasInfo cotaInfo;

            int apresentacaoSetorID = 0;

            try
            {
                for (int i = 0; i < lstCotas.Count; i++)
                {
                    if (lstCotas[i].ApresentacaoSetorID == 0)
                    {
                        apresentacaoSetorID = Convert.ToInt32(bd.ConsultaValor("SELECT ApresentacaoSetorID FROM tIngresso (NOLOCK) WHERE ID = " + lstCotas[i].IngressoID + " AND ApresentacaoID = " + lstCotas[i].ApresentacaoID));
                        bd.FecharConsulta();
                    }
                    else
                        apresentacaoSetorID = lstCotas[i].ApresentacaoSetorID;

                    cotaInfo = new EstruturaCotasInfo();

                    int[] qtds = this.getQuantidadeNovo(lstCotas[i].CotaItemID, lstCotas[i].CotaItemIDAPS, lstCotas[i].ApresentacaoID, apresentacaoSetorID);
                    cotaInfo.QuantidadeApresentacao = qtds[0];
                    cotaInfo.QuantidadeApresentacaoSetor = qtds[1];
                    cotaInfo.QuantidadeCota = qtds[2];
                    cotaInfo.QuantidadeCotaAPS = qtds[3];

                    //Eh um Ingresso Nominal e Tem quantidade Maxima?
                    if (lstCotas[i].Nominal && (lstCotas[i].QuantidadePorClienteApresentacao > 0 ||
                        lstCotas[i].QuantidadePorClienteApresentacaoSetor > 0 ||
                        lstCotas[i].QuantidadePorClienteCotaItem > 0 ||
                        lstCotas[i].QuantidadeCotaItemAPS > 0))
                    {
                        //Preenche o OBJ de IngressoCliente e retorna as quantidades ja compradas
                        oIngressoCliente.ApresentacaoID.Valor = lstCotas[i].ApresentacaoID;
                        oIngressoCliente.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                        oIngressoCliente.DonoID.Valor = lstCotas[i].DonoID;
                        oIngressoCliente.CotaItemID.Valor = lstCotas[i].CotaItemID;
                        oIngressoCliente.CPF.Valor = lstCotas[i].CPF;

                        //Busca na Proc e retorna o Array
                        qtds = oIngressoCliente.QuantidadeJaCompradaNovo(lstCotas[i].CotaItemID, lstCotas[i].CotaItemIDAPS, lstCotas[i].IngressoID);

                        cotaInfo.QuantidadePorClienteApresentacao = qtds[0];
                        cotaInfo.QuantidadePorClienteApresentacaoSetor = qtds[1];
                        cotaInfo.QuantidadePorClienteCota = qtds[2];
                        cotaInfo.QuantidadePorClienteCotaAPS = qtds[3];
                    }
                    else
                    {
                        cotaInfo.QuantidadePorClienteApresentacao = 0;
                        cotaInfo.QuantidadePorClienteApresentacaoSetor = 0;
                        cotaInfo.QuantidadePorClienteCota = 0;
                        cotaInfo.QuantidadePorClienteCotaAPS = 0;
                    }

                    //Atribui as quantidades maximas
                    cotaInfo.MaximaApresentacao = lstCotas[i].QuantidadeApresentacao;
                    cotaInfo.MaximaApresentacaoSetor = lstCotas[i].QuantidadeApresentacaoSetor;
                    cotaInfo.MaximaCotaItem = lstCotas[i].QuantidadeCotaItem;
                    cotaInfo.MaximaCotaItemAPS = lstCotas[i].QuantidadeCotaItemAPS;

                    cotaInfo.MaximaPorClienteApresentacao = lstCotas[i].QuantidadePorClienteApresentacao;
                    cotaInfo.MaximaPorClienteApresentacaoSetor = lstCotas[i].QuantidadePorClienteApresentacaoSetor;
                    cotaInfo.MaximaPorClienteCotaItem = lstCotas[i].QuantidadePorClienteCotaItem;
                    cotaInfo.MaximaPorClienteCotaItemAPS = lstCotas[i].QuantidadePorClienteCotaItemAPS;

                    //Propriedades de continuidade
                    cotaInfo.ReservaID = lstCotas[i].ReservaID;
                    cotaInfo.ApresentacaoID = lstCotas[i].ApresentacaoID;
                    cotaInfo.ApresentacaoSetorID = apresentacaoSetorID;
                    cotaInfo.DonoID = lstCotas[i].DonoID;
                    cotaInfo.IngressoID = lstCotas[i].IngressoID;
                    cotaInfo.CotaItemID = lstCotas[i].CotaItemID;
                    cotaInfo.CotaItemID_APS = lstCotas[i].CotaItemIDAPS;
                    cotaInfo.CodigoPromocional = lstCotas[i].CodigoPromocional;
                    cotaInfo.CPF = lstCotas[i].CPF;
                    cotaInfo.Nominal = lstCotas[i].Nominal;
                    lstRetorno.Add(cotaInfo);
                }

                return lstRetorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


        public string StringAtualizarAP()
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("UPDATE tCotaItemControleInMemory SET ");
            stbSQL.Append("Quantidade = Quantidade + 1 ");
            stbSQL.Append("WHERE ApresentacaoID = " + this.ApresentacaoID.Valor + " AND ");
            stbSQL.Append("ApresentacaoSetorID = 0 AND CotaItemID = " + this.CotaItemID.Valor);

            return stbSQL.ToString();
        }

        public string StringAtualizarAPQtd(int quantidade, int apresentacaoID, int cotaItemID)
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("UPDATE tCotaItemControleInMemory SET ");
            stbSQL.Append("Quantidade = SELECT Quantidade FROM tCotaItemControleInMemory WHERE CotaiTemID = " + cotaItemID + " + " + quantidade);
            stbSQL.Append("WHERE ApresentacaoID = " + apresentacaoID + " AND ");
            stbSQL.Append("ApresentacaoSetorID = 0 AND CotaItemID = " + cotaItemID);

            return stbSQL.ToString();
        }
        public string StringAtualizarAPS()
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("UPDATE tCotaItemControleInMemory SET ");
            stbSQL.Append("Quantidade = Quantidade + 1 ");
            stbSQL.Append("WHERE ApresentacaoID = " + this.ApresentacaoID.Valor + " AND ");
            stbSQL.Append("ApresentacaoSetorID = " + this.ApresentacaoSetorID.Valor + " AND CotaItemID = " + this.CotaItemID.Valor);

            return stbSQL.ToString();
        }

        public bool verficiarPrecoJaDistribuido(int apresentacaoID, int setorID, string precoNome)
        {
            try
            {
                string strSQL = "sp_checkPrecoTemCota " + apresentacaoID + ", " + setorID + ",'" + precoNome + "'";
                return Convert.ToInt32(bd.ConsultaValor(strSQL)) > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int getQuantidadePromocionalPorCliente(int apresentacaoID, string codigoPromocional)
        {
            try
            {
                int qtd = 0;
                string sql = @"SELECT COUNT(ID) AS Apresentacao FROM tIngressoCliente (NOLOCK)
		                        WHERE ApresentacaoID = " + apresentacaoID + " and CodigoPromocional = '" + codigoPromocional + "'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    qtd = bd.LerInt("Apresentacao");

                }
                return qtd;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool VerificaQtdControleInMemory(int QtdIngressos, int CotaItemID, int QtdVendida)
        {
            try
            {
                int QuantidadeCadastrada = 0;
                int qtdTotal = 0;
                string sql = string.Format(@"SELECT tcmi.Quantidade AS QuantidadeCadastrada FROM dbo.tCotaItem tcmi WHERE tcmi.ID = {0}", CotaItemID);
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    QuantidadeCadastrada = bd.LerInt("QuantidadeCadastrada");
                }

                qtdTotal = QtdVendida + QtdIngressos;
                if (qtdTotal <= QuantidadeCadastrada)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                bd.Fechar();
            }
        }
        public void RevertControleInMemory(int qtd, int CotaItemID,int ApresentacaoID)
        {
            try
            {
                string sql = string.Format(@"UPDATE tCotaItemControleInMemory SET tCotaItemControleInMemory.Quantidade = tCotaItemControleInMemory.Quantidade - {0} WHERE tCotaItemControleInMemory.CotaItemID = {1} AND tCotaItemControleInMemory.ApresentacaoID = {2}", qtd, CotaItemID,ApresentacaoID);
                bd.Consulta(sql);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }
        public void AdicionarControleInMemory(int qtd, int CotaItemID,int ApresentacaoID)
        {
            try
            {
                string sql = string.Format(@"UPDATE tCotaItemControleInMemory SET tCotaItemControleInMemory.Quantidade = tCotaItemControleInMemory.Quantidade + {0} WHERE tCotaItemControleInMemory.CotaItemID = {1} AND tCotaItemControleInMemory.ApresentacaoID = {2}", qtd, CotaItemID,ApresentacaoID);
                bd.Consulta(sql);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public int getQtdVendidaTotalCota(string apresentacaoID, int cotaItemID)
        {
            try
            {
                int qtd = 0;
                string sql = string.Format(@"EXEC sp_VerificaCotaQuantidadePrecoApresentacao " + cotaItemID.ToString() + " , " + apresentacaoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    qtd = bd.LerInt("Quantidade");
                }
                return qtd;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }
    }

    public class CotaItemControleLista : CotaItemControleLista_B
    {
        public CotaItemControleLista() { }
    }

}
