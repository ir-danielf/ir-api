using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using IRCore.Util.Enumerator;
using System.Globalization;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.DataAccess.ADO.Enumerator;
using Dapper;
using System.Data.SqlClient;
using System.Data;


namespace IRCore.DataAccess.ADO
{
    public class IngressoADO : MasterADO<dbIngresso>
    {

        private List<tIngresso> ingressos { get; set; }

        public IngressoADO(MasterADOBase ado = null) : base(ado, false) { }

        /// <summary>
        /// Consulta a quantidade de ingressos por evento e apresentação.
        /// </summary>
        /// <param name="eventoId">Código do evento.</param>
        /// <param name="clienteId">Código do cliente.</param>
        /// <param name="apresentacaoId">Código da aprensetação.</param>
        /// <exception cref="InvalidOperationException">Operação inválida se o número de ingressos não estiver dentro da regra.</exception>
        public void ConsultarTotalIngressoPorClienteApresentacao(int eventoId, int clienteId, int apresentacaoId, string sessionId, int quantidadeTotalDeIngressosPorEvento, int quantidadeTotalDeIngressosPorApresentacao, int quantidadeTotalDeApresentacoesPorEvento, string textoTermoValidacao)
        {
            IEnumerable<tIngresso> ingressos;

            // Constrói a query de consulta baseada no cliente ou session id.
            var sql = "SELECT ID, Status, SetorID, ApresentacaoID, EventoID, ClienteID FROM tIngresso (NOLOCK) WHERE STATUS IN ('R', 'V', 'I') AND EventoId = @eventoId";
            if (clienteId > 0)
            {
                // Cliente autenticado no contexto da aplicação.
                sql += " AND ClienteId = @clienteId";
                ingressos = conIngresso.Query<tIngresso>(sql, new { eventoId = eventoId, clienteId = clienteId });
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                // Cliente anônimo.
                sql += " AND SessionId = @sessionId";
                ingressos = conIngresso.Query<tIngresso>(sql, new { eventoId = eventoId, sessionId = sessionId });
            }
            else
            {
                return;
            }

            // Primeiro, verifica se o cliente já utilizou o limite máximo de ingressos para o evento.
            if (ingressos.Count() > quantidadeTotalDeIngressosPorEvento)
            {
                throw new InvalidOperationException(string.Format("Você só pode reservar/comprar {0} {1} para este evento e já atingiu esse limite.", quantidadeTotalDeIngressosPorEvento, textoTermoValidacao));
            }

            // Verifica por apresentação.
            var ingressosPorApresentacao = ingressos.Where(x => x.ApresentacaoID == apresentacaoId);
            if (ingressosPorApresentacao.Count() > quantidadeTotalDeIngressosPorApresentacao)
            {
                throw new InvalidOperationException(string.Format("Só é possível reservar/comprar até {0} {1} para esta apresentação.", quantidadeTotalDeIngressosPorApresentacao, textoTermoValidacao));
            }

            // Verifica se o usuário já comprou para mais de duas apresentações.
            var totalApresentacoes = ingressos.Select(x => x.ApresentacaoID).Distinct().Count();
            if (totalApresentacoes > quantidadeTotalDeApresentacoesPorEvento)
            {
                throw new InvalidOperationException(string.Format("Você já reservou/comprou {0} apresentações neste evento e não é possível efetuar novas reservas/compras.", quantidadeTotalDeApresentacoesPorEvento));
            }
        }

        public tIngresso Consultar(int id)
        {
            string queryString = @"SELECT ID,ApresentacaoSetorID,PrecoID,LugarID,UsuarioID,CortesiaID,BloqueioID,Codigo,Status,CodigoBarra,SetorID,ApresentacaoID,EventoID,LocalID,EmpresaID,CodigoBarraCliente,LojaID,VendaBilheteriaID,ClienteID,PacoteID,PacoteGrupo,Classificacao,Grupo,SessionID,TimeStampReserva,CodigoSequencial,PrecoExclusivoCodigoID,CodigoImpressao,CanalID,AssinaturaClienteID,SerieID,CompraGUID,GerenciamentoIngressosID,ParceiroMidiaID,CotaItemID
									FROM tIngresso (NOLOCK)
									where Id = @id";

            var query = conIngresso.Query<tIngresso>(queryString, new
            {
                id = id
            });

            return query.FirstOrDefault();
        }

        public List<tIngresso> ListarParceiroStatus(int parceiroId, int setorId, int apresentacaoId, enumIngressoStatus status, int quantidade)
        {
            var sql = "SELECT TOP " + quantidade + @" ID,ApresentacaoSetorID,PrecoID,LugarID,UsuarioID,CortesiaID,BloqueioID,Codigo,Status,CodigoBarra,SetorID,ApresentacaoID,EventoID,LocalID,EmpresaID,CodigoBarraCliente,LojaID,VendaBilheteriaID,ClienteID,PacoteID,PacoteGrupo,Classificacao,Grupo,SessionID,TimeStampReserva,CodigoSequencial,PrecoExclusivoCodigoID,CodigoImpressao,CanalID,AssinaturaClienteID,SerieID,CompraGUID,GerenciamentoIngressosID,ParceiroMidiaID
						FROM tIngresso
						WHERE status = @status AND ParceiroMidiaID = @ParceiroMidiaID
						AND SetorID = @SetorID AND ApresentacaoID = @ApresentacaoID";

            var result = conIngresso.Query<tIngresso>(sql, new
            {
                ParceiroMidiaID = parceiroId,
                status = status.ValueAsString(),
                SetorID = setorId,
                ApresentacaoID = apresentacaoId
            }).ToList();

            return result;
        }

        public List<tIngresso> ListarID(List<int> ingressoID)
        {
            var result = (from item in dbIngresso.tIngresso
                          where ingressoID.Contains(item.ID)
                          select item);
            return result.AsNoTracking().ToList();
        }

        public List<tIngresso> ListarSessaoStatus(string sessionID, enumIngressoStatus status)
        {
            string statusAsStr = status.ValueAsString();
            var result = (from item in dbIngresso.tIngresso
                          where item.SessionID == sessionID
                          && item.Status == statusAsStr
                          select item);
            return result.AsNoTracking().ToList();
        }

        /// <summary>
        /// Lista ingressos pela apresentação, setor e status
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<tIngresso> ListarParceiroMidia(int apresentacaoID, int setorID, string busca, enumIngressoStatus status, int notParceiroMidiaID = 0, int quantidade = 0)
        {
            string statusAsStr = status.ValueAsString();
            var query = (from item in dbIngresso.tIngresso
                         join bloqueio in dbIngresso.tBloqueio on item.BloqueioID equals bloqueio.ID
                         where item.ApresentacaoID == apresentacaoID && item.SetorID == setorID
                         && item.Status == statusAsStr
                         select new IngressoBloqueioModelQuery()
                         {
                             ingresso = item,
                             bloqueio = bloqueio
                         });
            if (!string.IsNullOrWhiteSpace(busca))
            {
                List<string> items = busca.Split(',').ToList();
                query = query.Where(x => items.Count(y => x.ingresso.Codigo.Contains(y)) > 0);
            }
            if (notParceiroMidiaID > 0)
            {
                query = query.Where(x => x.ingresso.ParceiroMidiaID != notParceiroMidiaID);
            }

            if (quantidade > 0)
            {
                query = query.Take(quantidade);
            }

            return query.AsNoTracking().ToList().Select(t => t.toIngresso()).ToList();
        }

        public List<BloqueioResult> ListarIngressoParceiroMediaApresentacao(int parceiroMediaID)
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)conIngresso;
                cmd.CommandTimeout = 600;
                cmd.CommandText = string.Format(

                          @"SELECT te.Nome Evento,
                               dbo.DataHoraFormatada(ta.horario) Horario,
                               ti.Codigo,
                               ti.id IngressoID,
                               tb.nome Bloqueio,
                               ta.id ApresentacaoID
                        FROM tingresso ti
                             INNER JOIN dbo.tBloqueio tb(NOLOCK) ON tb.ID = ti.BloqueioID
                             INNER JOIN dbo.tParceiroMediaBloqueio pb ON pb.BloqueioID = tb.id
                             INNER JOIN dbo.tEvento te(NOLOCK) ON ti.EventoID = te.id
                             INNER JOIN dbo.tApresentacao ta(NOLOCK) ON ti.ApresentacaoID = ta.ID
                             INNER JOIN dbo.tLocal tl ON ti.LocalID = tl.ID
                             
            
                             INNER JOIN SiteIR..evento siteEvento(NOLOCK) ON siteEvento.IR_EventoID = ti.EventoID
                             INNER JOIN SiteIR..apresentacao siteApresentacao(NOLOCK) ON siteApresentacao.IR_ApresentacaoID = ti.ApresentacaoID
            
                        WHERE pb.ParceiroMidiaID = {0}
                              AND ti.ParceiroMidiaID IS NULL
                        ORDER BY	  ti.Codigo,tb.nome;
                        ", parceiroMediaID);

                conIngresso.Open();

                //SqlDataReader dr = cmd.ExecuteReader();

                DataTable dtb = new DataTable();

                dtb.Load(cmd.ExecuteReader());

                DataView dtv = new DataView(dtb);

                DataTable distinctValues = dtv.ToTable(true, "Evento", "Horario", "ApresentacaoID");

                List<BloqueioResult> Bloqueios = new List<BloqueioResult>();

                foreach (DataRow rowDistinct in distinctValues.Rows)
                {
                    Bloqueios.Add(new BloqueioResult()
                    {
                        ApresentacaoID = int.Parse(rowDistinct["ApresentacaoID"].ToString()),
                        DataApresentacao = rowDistinct["Horario"].ToString(),
                        NomeEvento = rowDistinct["Evento"].ToString(),
                        DetalhesBloqueio = (from rowDtb in dtb.AsEnumerable()
                                            where rowDtb["ApresentacaoID"].ToString() == rowDistinct["ApresentacaoID"].ToString()
                                            select new DetalheBloqueio()
                                            {
                                                CodigoIngresso = rowDtb["Codigo"].ToString(),
                                                IngressoID = int.Parse(rowDtb["IngressoID"].ToString()),
                                                NomeBloqueio = rowDtb["Bloqueio"].ToString()
                                            }).ToList()
                    });
                }


                return Bloqueios;


            }
            catch (Exception ex)
            {

                throw;

            }
            finally
            {
                if (conIngresso.State != System.Data.ConnectionState.Open)
                {
                    conIngresso.Close();
                }
            }






            //            var query = conIngresso.Query<BuscarApresentacoesIngressoResult>(
            //            string.Format(

            //              @"SELECT te.Nome Evento,
            //                   dbo.DataHoraFormatada(ta.horario) Apresentacao,
            //                   ti.Codigo,
            //                   ti.id IngressoID,
            //                   tb.nome Bloqueio,
            //                   ta.id ApresentacaoID
            //            FROM tingresso ti
            //                 INNER JOIN dbo.tBloqueio tb(NOLOCK) ON tb.ID = ti.BloqueioID
            //                 INNER JOIN dbo.tParceiroMediaBloqueio pb ON pb.BloqueioID = tb.id
            //                 INNER JOIN dbo.tEvento te(NOLOCK) ON ti.EventoID = te.id
            //                 INNER JOIN dbo.tApresentacao ta(NOLOCK) ON ti.ApresentacaoID = ta.ID
            //                 INNER JOIN dbo.tLocal tl ON ti.LocalID = tl.ID
            //                 INNER JOIN dbo.ParceiroMidiaClasseSetor pmcs(NOLOCK) ON pmcs.ApresentacaoID = ta.ID
            //
            //                 INNER JOIN SiteIR..evento siteEvento(NOLOCK) ON siteEvento.IR_EventoID = ti.EventoID
            //                 INNER JOIN SiteIR..apresentacao siteApresentacao(NOLOCK) ON siteApresentacao.IR_ApresentacaoID = ti.ApresentacaoID
            //
            //            WHERE pb.ParceiroMidiaID = 6
            //                  AND ti.ParceiroMidiaID IS NULL;
            //            ", parceiroMediaID));

            //            var result = query.ToList();



        }

        /// <summary>
        /// Obter o total de ingressos
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <param name="busca"></param>
        /// <param name="status"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public int ObterTotalIngressos(int apresentacaoID, int setorID, string busca, enumIngressoStatus status, int quantidade)
        {
            string statusAsStr = status.ValueAsString();
            var query = (from item in dbIngresso.tIngresso
                         where item.ApresentacaoID == apresentacaoID && item.SetorID == setorID
                         && item.Status == statusAsStr
                         select item);

            if (!string.IsNullOrWhiteSpace(busca))
            {
                List<string> items = busca.Split(',').ToList();
                query = query.Where(x => items.Count(y => x.Codigo.Contains(y)) > 0);
            }

            if (quantidade > 0)
                query = query.Take(quantidade);

            return query.AsNoTracking().Count();
        }



        /// <summary>
        ///  Retorna o número de lugares de uma determinado 
        /// </summary>
        /// <param name="apresentacaoId"></param>
        /// <param name="lugarId"></param>
        /// <returns></returns>
        public int ObterTotalIngressosDeUmaMesaFechada(int apresentacaoId, int lugarId)
        {
            var query = (from item in dbIngresso.tIngresso
                         where item.ApresentacaoID == apresentacaoId
                                && item.LugarID == lugarId
                         select item);

            return query.AsNoTracking().Count();
        }



        /// <summary>
        /// Lista os ingressos que contéms os ids informados
        /// </summary>
        /// <param name="ingressosIds"></param>
        /// <returns></returns>
        public List<tIngresso> Listar(List<int> ingressosIds)
        {
            string sql = @"SELECT ing.*,
	                        sto.*,
	                        evt.*,
	                        emd.*,
	                        etm.*,
	                        loc.*,
	                        apr.*
                           FROM tIngresso(NOLOCK) ing
                           LEFT JOIN tSetor(NOLOCK) sto ON sto.ID = ing.SetorID
                           LEFT JOIN tEvento(NOLOCK) evt ON evt.ID = ing.eventoID
                           LEFT JOIN EventoMidia(NOLOCK) emd ON emd.EventoID = evt.ID
                           LEFT JOIN EventoTipoMidia(NOLOCK) etm ON etm.ID = emd.EventoID
                           LEFT JOIN tLocal(NOLOCK) loc ON loc.ID = evt.LocalID
                           LEFT JOIN tApresentacao(NOLOCK) apr ON apr.EventoID = evt.ID
                           WHERE ing.ID in @ingressosIds";

            ingressos = new List<tIngresso>();

            conIngresso.Query<tIngresso, tSetor, tEvento, EventoMidia, EventoTipoMidia, tLocal, tApresentacao, bool>(sql, addResultListarIngresso, new
            {
                ingressosIds = ingressosIds
            });


            List<tIngresso> result = ingressos;

            ingressos = null;

            return result;
        }

        private bool addResultListarIngresso(tIngresso ingresso, tSetor setor, tEvento evento, EventoMidia eventoMidia, EventoTipoMidia eventoTipoMidia, tLocal local, tApresentacao apresentacao)
        {
            tIngresso aux = ingressos.Where(x => x.ID == ingresso.ID).FirstOrDefault();

            if (aux == null)
            {
                aux = ingresso;
                aux.tSetor = setor;
                aux.tEvento = evento;
                aux.tEvento.EventoMidia = new List<EventoMidia>();
                if (eventoMidia != null)
                {
                    eventoMidia.EventoTipoMidia = eventoTipoMidia;
                    aux.tEvento.EventoMidia.Add(eventoMidia);
                }
                aux.tEvento.tLocal = local;
                aux.tEvento.tApresentacao = new List<tApresentacao>();
                if (apresentacao != null)
                {
                    aux.tEvento.tApresentacao.Add(apresentacao);
                    if (apresentacao.ID == aux.ApresentacaoID)
                        aux.tApresentacao = apresentacao;
                }



                ingressos.Add(aux);
            }
            else
            {

                if (eventoMidia != null && aux.tEvento.EventoMidia.All(x => x.ID != eventoMidia.ID))
                {
                    eventoMidia.EventoTipoMidia = eventoTipoMidia;
                    aux.tEvento.EventoMidia.Add(eventoMidia);
                }
                if (apresentacao != null && aux.tEvento.tApresentacao.All(x => x.ID != apresentacao.ID))
                {
                    aux.tEvento.tApresentacao.Add(apresentacao);
                    if (apresentacao.ID == aux.ApresentacaoID)
                        aux.tApresentacao = apresentacao;
                }
            }
            return true;
        }

        public void DesassociarParceiroMidia(int parceiroMidiaId, string acao, int usuarioID, string ingressosIds)
        {
            conIngresso.Execute(
                string.Format(
                    @"exec  Pr_DesassociarIngressoParceiroMidia {0}, '{1}', {2}, '{3}'
                    ", parceiroMidiaId, acao, usuarioID, ingressosIds));

        }

        public void RelacionarBloqueioApresentacaoParceiroMidia(int parceiroMidiaId, int usuarioID, string ingressosIds)
        {
            conIngresso.Execute(
                string.Format(
                    @"exec  Pr_BloquearIngressoParceiroMidia {0}, {1}, '{2}'
                    ", parceiroMidiaId, usuarioID, ingressosIds));
        }

        public List<tIngresso> ListarBloqueados(int eventoID, int parceiroMidiaID)
        {
            List<tIngresso> result = (from item in dbIngresso.tIngresso.Include(t => t.tEvento).Include(t => t.tApresentacao)
                                      where item.Status == "B"
                                      && item.ParceiroMidiaID == parceiroMidiaID && item.EventoID == eventoID
                                      select item).AsNoTracking().ToList();
            return result;
        }

        public List<tIngresso> Consultar(string sessionID, int clienteID)
        {
            return dbIngresso.tIngresso.Where(s => s.SessionID.Equals(sessionID) && s.ClienteID == clienteID).AsNoTracking().ToList();
        }

        public tIngresso BuscaRetornoOsesp(int IngressoID)
        {
            string queryString = @"select i.* from tIngresso (NOLOCK) i 
								JOIN API_Osesp_Eventos (NOLOCK) e on i.EventoID = e.ID 
								WHERE i.ID = @IngressoID";
            var ingresso = conIngresso.Query<tIngresso>(queryString, new
            {
                IngressoID = IngressoID
            }).FirstOrDefault();

            return ingresso;
        }

        public string BuscaNomeBloqueio(int BloqueioID)
        {
            string queryString = @"SELECT [Nome]
                                      FROM [dbo].[tBloqueio]
                                      where ID = @bloqueioID";
            string nome = conIngresso.Query<string>(queryString, new
            {
                bloqueioID = BloqueioID
            }).FirstOrDefault();

            return nome.ToString();
        }


        public bool DesbloquearIngressoOSESP(int IngressoID, int UsuarioID, int PluID, int PluUtilizadoID, int BloqueioPadrao)
        {
            string queryString = @"select i.* from tIngresso (NOLOCK) i 
								JOIN API_Osesp_Eventos (NOLOCK) e on i.EventoID = e.ID 
								WHERE i.ID = @IngressoID  --AND i.Status = 'B'";

            var ingresso = conIngresso.Query<tIngresso>(queryString, new
            {
                IngressoID = IngressoID
            }).FirstOrDefault();

            if (ingresso != null)
            {
                int BloqueioAtualID = Convert.ToInt32(ingresso.BloqueioID);
                int bloqueio = 0;

                /*DESBLOQUEIO
                Se o ingresso estiver bloqueado (st = ‘B’), se o bloqueio for BLOQUEIO_PADRAO ( BI (Banco de Ingresso)), Desbloquear o ingresso (st = ‘D’)
                                                Se o bloqueio for PLU_UTILIZADO, mudar o bloqueio para PLU
                                                Se não, não faz nada (sucsses = false)
                                                Se não (st != ‘B’), não faz nada (sucsses = false)*/

                //- Estando o ingresso desbloqueado com qualquer status, o mesmo não poderá ser debloqueado novamente.
                if (!ingresso.Status.Equals("B"))
                {
                    return false;
                }
                else
                {
                    //Se o ingresso estiver bloqueado (st = ‘B’), se o bloqueio for BLOQUEIO_PADRAO ( BI (Banco de Ingresso)), Desbloquear o ingresso (st = ‘D’)
                    if (ingresso.Status.Equals("B") && BloqueioAtualID == BloqueioPadrao)
                    {
                        bloqueio = BloqueioPadrao;
                    }
                    else
                    {
                        //Se o bloqueio for PLU_UTILIZADO, mudar o bloqueio para PLU
                        if (ingresso.Status.Equals("B") && BloqueioAtualID == PluUtilizadoID)
                        {
                            bloqueio = PluID;
                            var mudaBloqueio = conIngresso.Execute("update tIngresso set BloqueioID =  @BloqueioID where ID = @IngressoID", new { IngressoID = IngressoID, BloqueioID = bloqueio }) > 0;

                            if (mudaBloqueio)
                            {
                                return conIngresso.Execute(
                                    @"insert into tIngressoLog(
							                    [IngressoID]
							                    ,[UsuarioID]
							                    ,[TimeStamp]
							                    ,[Acao]
							                    ,[PrecoID]
							                    ,[CortesiaID]
							                    ,[BloqueioID]
							                    ,[VendaBilheteriaItemID]
							                    ,[Obs]
							                    ,[EmpresaID]
							                    ,[VendaBilheteriaID]
							                    ,[CaixaID]
							                    ,[LojaID]
							                    ,[CanalID]
							                    ,[ClienteID]
							                    ,[CodigoBarra]
							                    ,[CodigoImpressao]
							                    ,[MotivoId]
							                    ,[SupervisorID]
							                    ,[GerenciamentoIngressosID]
							                    ,[AssinaturaClienteID]
						                    )
						                    values
						                    (
							                    @IngressoID
							                    ,@UsuarioID
							                    ,GETDATE()
							                    ,'D'
							                    ,@PrecoID
							                    ,0
							                    ,0
							                    ,0
							                    ,'BloqueioOSESP'
							                    ,0
							                    ,0
							                    ,0
							                    ,0
							                    ,0
							                    ,null
							                    ,null
							                    ,null
							                    ,null
							                    ,null
							                    ,null
							                    ,null
						                    );",
                                    new
                                    {
                                        IngressoID = IngressoID,
                                        UsuarioID = UsuarioID,
                                        PrecoID = ingresso.PrecoID,
                                        BloqueioID = bloqueio
                                    }
                                ) > 0;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                var atualizou = conIngresso.Execute("update tIngresso set Status = 'D', BloqueioID =  @BloqueioID where ID = @IngressoID", new { IngressoID = IngressoID, BloqueioID = bloqueio }) > 0;

                if (atualizou)
                {
                    return conIngresso.Execute(
                        @"insert into tIngressoLog(
							[IngressoID]
							,[UsuarioID]
							,[TimeStamp]
							,[Acao]
							,[PrecoID]
							,[CortesiaID]
							,[BloqueioID]
							,[VendaBilheteriaItemID]
							,[Obs]
							,[EmpresaID]
							,[VendaBilheteriaID]
							,[CaixaID]
							,[LojaID]
							,[CanalID]
							,[ClienteID]
							,[CodigoBarra]
							,[CodigoImpressao]
							,[MotivoId]
							,[SupervisorID]
							,[GerenciamentoIngressosID]
							,[AssinaturaClienteID]
						)
						values
						(
							@IngressoID
							,@UsuarioID
							,GETDATE()
							,'D'
							,@PrecoID
							,0
							,0
							,0
							,'BloqueioOSESP'
							,0
							,0
							,0
							,0
							,0
							,null
							,null
							,null
							,null
							,null
							,null
							,null
						);",
                        new
                        {
                            IngressoID = IngressoID,
                            UsuarioID = UsuarioID,
                            PrecoID = ingresso.PrecoID,
                            BloqueioID = bloqueio
                        }
                    ) > 0;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool BloquearIngressoOSESP(int IngressoID, int UsuarioID, int PluID, int PluUtilizadoID, int BloqueioPadrao)
        {
            string queryString = @"select i.* from tIngresso (NOLOCK) i 
								JOIN API_Osesp_Eventos (NOLOCK) e on i.EventoID = e.ID 
								WHERE i.ID = @IngressoID --AND i.Status = 'D'";

            var ingresso = conIngresso.Query<tIngresso>(queryString, new
            {
                IngressoID = IngressoID

            }).FirstOrDefault();

            if (ingresso != null)
            {
                int BloqueioAtualID = Convert.ToInt32(ingresso.BloqueioID);
                int bloqueio = 0;

                /*BLOQUEIO
                Se o Ingresso estiver disponível (st = ‘D’ ), bloquear o Ingresso como BLOQUEIO_PADRAO ( BI (Banco de Ingresso))
                Se o Ingresso estiver bloqueado (st = ‘B’), se o bloqueio for PLU (bn = ‘PLU’) mudar o bloqueio para PLU_UTILIZADO
                Se não, não faz nada (sucsses = false)
                 * 
                Se não (st != ‘D’ e st != ‘B’), não faz nada (sucsses = false)*/
                //============================================================//

                //Se não (st != ‘D’ e st != ‘B’), não faz nada (sucsses = false)
                if (!ingresso.Status.Equals("D") && !ingresso.Status.Equals("B"))
                {
                    return false;
                }
                else
                {
                    //Se o Ingresso estiver disponível (st = ‘D’ ), bloquear o Ingresso como BLOQUEIO_PADRAO ( BI (Banco de Ingresso))
                    if (ingresso.Status.Equals("D"))
                    {
                        bloqueio = BloqueioPadrao;
                    }
                    else
                    {
                        //Se o Ingresso estiver bloqueado (st = ‘B’), se o bloqueio for PLU (bn = ‘PLU’) mudar o bloqueio para PLU_UTILIZADO
                        if (ingresso.Status.Equals("B") && BloqueioAtualID == PluID)
                        {
                            bloqueio = PluUtilizadoID;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                var atualizou = conIngresso.Execute("update tIngresso set Status = 'B', BloqueioID = @BloqueioID where ID = @IngressoID", new { IngressoID = IngressoID, BloqueioID = bloqueio }) > 0;

                if (atualizou)
                {
                    return conIngresso.Execute(
                        @"insert into tIngressoLog(
							[IngressoID]
							,[UsuarioID]
							,[TimeStamp]
							,[Acao]
							,[PrecoID]
							,[CortesiaID]
							,[BloqueioID]
							,[VendaBilheteriaItemID]
							,[Obs]
							,[EmpresaID]
							,[VendaBilheteriaID]
							,[CaixaID]
							,[LojaID]
							,[CanalID]
							,[ClienteID]
							,[CodigoBarra]
							,[CodigoImpressao]
							,[MotivoId]
							,[SupervisorID]
							,[GerenciamentoIngressosID]
							,[AssinaturaClienteID]
						)
						values
						(
							@IngressoID
							,@UsuarioID
							,GETDATE()
							,'B'
							,@PrecoID
							,0
							,@BloqueioID
							,0
							,'BloqueioOSESP'
							,0
							,0
							,0
							,0
							,0
							,null
							,null
							,null
							,null
							,null
							,null
							,null
						);",
                        new
                        {
                            IngressoID = IngressoID,
                            UsuarioID = UsuarioID,
                            PrecoID = ingresso.PrecoID,
                            BloqueioID = bloqueio
                        }
                    ) > 0;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool RemoverReserva(List<tIngresso> listaReserva)
        {
            var sql = @"UPDATE tIngresso SET UsuarioID = @usuarioId, SessionID = @sessionId, ClienteID = @clienteId, Status = @status, CotaItemId = @cotaitemId WHERE ID = @ID";

            var linhas = conIngresso.Execute(sql, listaReserva);

            return linhas > 0;
        }

        class MappingCodigoBarras
        {
            public int ID { get; set; }
            public string CodigoBarra { get; set; }
        }

        public Dictionary<int, string> BuscarCodigosBarras(int[] ingressosId)
        {
            var sql = string.Format("SELECT ID,CodigoBarra FROM tIngresso (NOLOCK) WHERE ID IN ({0})",
                                    string.Join(",", ingressosId));

            var retorno = new Dictionary<int, string>();
            var result = this.conIngresso.Query<MappingCodigoBarras>(sql).ToList();

            foreach (var item in result)
            {
                retorno.Add(item.ID, item.CodigoBarra);
            }
            return retorno;
        }
    }
}