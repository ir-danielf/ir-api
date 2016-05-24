using IRCore.DataAccess.ADO.Estrutura;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using Dapper;
using IRLib;
using Apresentacao = IRCore.DataAccess.Model.Apresentacao;
using Evento = IRCore.DataAccess.Model.Evento;
using Local = IRCore.DataAccess.Model.Local;
using Preco = IRCore.DataAccess.Model.Preco;
using Setor = IRCore.DataAccess.Model.Setor;
using IRCore.DataAccess.Model;

namespace IRCore.DataAccess.ADO
{
    public class SetorADO : MasterADO<dbIngresso>
    {

        private List<Setor> result = new List<Setor>();

        private int eventoID { get; set; }
        private int canalID { get; set; }

        private int addResult(Setor setor, Preco preco)
        {
            var resultSetor = result.FirstOrDefault(t => t.IR_SetorID == setor.IR_SetorID);

            if (resultSetor == null)
            {
                resultSetor = setor;
                resultSetor.Preco = new List<Preco> { preco };
                result.Add(resultSetor);
            }
            else
            {
                resultSetor.Preco.Add(preco);
            }
            
            return setor.IR_SetorID;
        }

        public SetorADO(MasterADOBase ado = null) : base(ado) { }

        public List<Setor> ListarIn(List<int> setorIDs, int apresentacaoID, int parceiroMediaID = 0)
        {

            var query = (from item in dbSite.Setor
                                 where item.ApresentacaoID == apresentacaoID && setorIDs.Contains(item.IR_SetorID)
                                select item);

            if (parceiroMediaID > 0)
            {
                query = query.Where(x => x.PrecoParceiroMidia.Select(y => y.ParceiroMidiaID).Contains(parceiroMediaID));
            }

            return query.Distinct().OrderBy(t => t.Nome).AsNoTracking().ToList();
        }

        public Setor Consultar(int setorId, int apresentacaoId)
        {
            var sql = @"SELECT  st.ID,st.IR_SetorID,st.Nome,st.LugarMarcado,st.ApresentacaoID,st.QtdeDisponivel,st.QuantidadeMapa,st.Obs,st.AprovadoPublicacao,st.PrincipalPrecoID,st.CodigoSala,st.NVendeLugar,
		                ap.ID,ap.IR_ApresentacaoID,ap.Horario,ap.EventoID,ap.UsarEsquematico,ap.Programacao,ap.CodigoProgramacao,ap.CalcDiaDaSemana,ap.CalcHorario,
		                ev.ID,ev.IR_EventoID,ev.Nome,ev.LocalID,ev.TipoID,ev.Release,ev.Obs,ev.Imagem,ev.Destaque,ev.Prioridade,ev.EntregaGratuita,ev.RetiradaBilheteria,ev.DisponivelAvulso,ev.Parcelas,ev.PublicarSemVendaMotivo,ev.Publicar,ev.SubtipoID,ev.DataAberturaVenda,ev.LocalImagemMapaID,ev.LocalImagemNome,ev.EscolherLugarMarcado,ev.PalavraChave,ev.ExibeQuantidade,ev.BannersPadraoSite,ev.MenorPeriodoEntrega,ev.FilmeID,ev.PermiteVendaPacote,ev.PossuiTaxaProcessamento,ev.LimiteMaximoIngressosEvento,ev.LimiteMaximoIngressosEstado,ev.ImagemDestaque,
                        lc.ID,lc.IR_LocalID,lc.Nome,lc.Cidade,lc.Estado,lc.Obs,lc.Endereco,lc.CEP,lc.DDDTelefone,lc.Telefone,lc.ComoChegar,lc.TaxaMaximaEmpresa,lc.BannersPadraoSite,lc.EmpresaID,lc.Pais,lc.Imagem,lc.CodigoPraca,lc.Latitude,lc.Longitude,lc.LatitudeAsDecimal,lc.LongitudeAsDecimal
		                FROM Setor st (NOLOCK)
		                INNER JOIN Apresentacao ap (NOLOCK) on st.ApresentacaoID = ap.IR_ApresentacaoID
		                INNER JOIN Evento ev (NOLOCK) on ev.IR_EventoID = ap.EventoID
                        INNER JOIN Local lc(NOLOCK) on ev.LocalID = lc.IR_LocalID
                        WHERE st.IR_SetorID = @setorID AND st.apresentacaoID = @apresentacaoID";

            var result = conSite.Query<Setor, Apresentacao, Evento, Local, Setor>(sql, addConsultar, new
            {
                setorID = setorId,
                apresentacaoID = apresentacaoId
            }).FirstOrDefault();

            return result;
        }

        private Setor addConsultar(Setor setor, Apresentacao apresentacao, Evento evento, Local local)
        {
            evento.Local = local;
            apresentacao.Evento = evento;
            setor.Apresentacao = apresentacao;
            return setor;
        }

        public List<Setor> consultarEvento(int apresentacaoid, string date)
        {
            List<Setor> result = (from item in dbSite.Apresentacao
                                  where item.IR_ApresentacaoID == apresentacaoid
                                  select item)
                          .AsNoTracking()
                          .Where(t => t.Horario == date)
                          .Select(t => t.Setor.ToList())
                          .FirstOrDefault();

            return result.ToList();
        }

        public List<Setor> Listar(int idApresentacao, int idEvento = 0, int idCanal = 2, bool comCotaNominal = true, bool comCotaPromocional = true, bool canalPOS = false)
        {
            var strQuery = "";
            eventoID = idEvento;
            canalID = idCanal;

            result = new List<Setor>();

            var strCon = ConfigurationManager.AppSettings["Conexao"];
            var builder = new SqlConnectionStringBuilder(strCon);
            var database = builder.InitialCatalog;

            if (!comCotaNominal || !comCotaPromocional)
            {
                var parametro = "";

                if (!comCotaNominal)
                {
                    parametro = "cotaNominal = 1";
                }

                if (!comCotaPromocional)
                {
                    parametro += (string.IsNullOrEmpty(parametro) ? "" : " OR ") + "cotaParceiro = 1";
                }

                if (canalPOS)
                {
                    strQuery =
                        string.Format(@"SELECT
                        setor.ID,
                        setor.IR_SetorID,
                        setor.Nome,
                        setor.LugarMarcado,
                        setor.ApresentacaoID,
                        setor.QtdeDisponivel,
                        setor.QuantidadeMapa,
                        setor.Obs,
                        setor.AprovadoPublicacao,
                        setor.PrincipalPrecoID,
                        setor.CodigoSala,
                        setor.NVendeLugar,
                        ISNULL(preco.ID, tp.ID) AS ID,
                        tp.ID as IR_PrecoID,
                        tp.Nome,
                        tp.Valor,
                        aps.ApresentacaoID,
                        aps.SetorID,
                        ISNULL(preco.QuantidadePorCliente, 0) AS QuantidadePorCliente,
                        ISNULL(preco.Pacote, 0) AS Pacote,
                        ISNULL(preco.Serie, 0) AS Serie,
                        ISNULL(preco.CodigoCinema, '') AS CodigoCinema
                    FROM Setor
                        ( NOLOCK )
                        INNER JOIN {0}..tApresentacaoSetor AS aps ( NOLOCK )
                        ON aps.SetorID = setor.IR_SetorID AND aps.ApresentacaoID = setor.ApresentacaoID
                        RIGHT JOIN {0}..tPreco AS tp ( NOLOCK ) ON tp.ApresentacaoSetorID = aps.ID
                        LEFT JOIN Preco AS preco ( NOLOCK ) ON tp.ID = preco.IR_PrecoID
                    WHERE aps.ApresentacaoID = @apresentacaoID AND tp.ID NOT IN (SELECT PrecoID
                                                                                               FROM vwPrecoCotaControle
                                                                                               WHERE
                                                                                                 ({1}))", database, parametro);
                }
                else
                {
                    strQuery = @"SELECT 
                           setor.ID,setor.IR_SetorID,setor.Nome,setor.LugarMarcado,setor.ApresentacaoID,setor.QtdeDisponivel,setor.QuantidadeMapa,setor.Obs,setor.AprovadoPublicacao,setor.PrincipalPrecoID,setor.CodigoSala,setor.NVendeLugar
	                      ,preco.ID,preco.IR_PrecoID,preco.Nome,preco.Valor,preco.ApresentacaoID,preco.SetorID,preco.QuantidadePorCliente,preco.Pacote,preco.Serie,preco.CodigoCinema
                      FROM Setor (NOLOCK)
                      INNER JOIN Preco (NOLOCK) on Preco.SetorID = setor.IR_SetorID AND preco.ApresentacaoID = setor.ApresentacaoID
                      WHERE preco.apresentacaoID = @apresentacaoID AND Preco.Pacote = 0 AND preco.IR_PrecoID not in (Select PrecoID from vwPrecoCotaControle where (" + parametro + "))";
                }
            }
            else
            {
                if (canalPOS)
                {
                    strQuery =
                        string.Format(@"SELECT
                          setor.ID,
                          setor.IR_SetorID,
                          setor.Nome,
                          setor.LugarMarcado,
                          setor.ApresentacaoID,
                          setor.QtdeDisponivel,
                          setor.QuantidadeMapa,
                          setor.Obs,
                          setor.AprovadoPublicacao,
                          setor.PrincipalPrecoID,
                          setor.CodigoSala,
                          setor.NVendeLugar,
                          ISNULL(preco.ID, tp.ID) AS ID,
                          tp.ID as IR_PrecoID,
                          tp.Nome,
                          tp.Valor,
                          aps.ApresentacaoID,
                          aps.SetorID,
                          ISNULL(preco.QuantidadePorCliente, 0) AS QuantidadePorCliente,
                          ISNULL(preco.Pacote, 0) AS Pacote,
                          ISNULL(preco.Serie, 0) AS Serie,
                          ISNULL(preco.CodigoCinema, '') AS CodigoCinema

                        FROM Setor
                          ( NOLOCK )
                          INNER JOIN {0}..tApresentacaoSetor AS aps ( NOLOCK )
                            ON aps.SetorID = setor.IR_SetorID AND aps.ApresentacaoID = setor.ApresentacaoID
                          RIGHT JOIN {0}..tPreco AS tp ( NOLOCK ) ON tp.ApresentacaoSetorID = aps.ID
                          LEFT JOIN Preco AS preco ( NOLOCK ) ON tp.ID = preco.IR_PrecoID
                        WHERE Setor.ApresentacaoID = @apresentacaoID", database);
                }
                else
                {
                    strQuery = string.Format(@"SELECT  
                           setor.ID,setor.IR_SetorID,setor.Nome,setor.LugarMarcado,setor.ApresentacaoID,setor.QtdeDisponivel,setor.QuantidadeMapa,setor.Obs,setor.AprovadoPublicacao,setor.PrincipalPrecoID,setor.CodigoSala,setor.NVendeLugar
	                      ,preco.ID,preco.IR_PrecoID,preco.Nome,preco.Valor,preco.ApresentacaoID,preco.SetorID,preco.QuantidadePorCliente,preco.Pacote,preco.Serie,preco.CodigoCinema
                          FROM Setor (NOLOCK)
                          INNER JOIN Preco (NOLOCK) on Preco.SetorID = setor.IR_SetorID AND preco.ApresentacaoID = setor.ApresentacaoID
                          where Setor.ApresentacaoID = @apresentacaoID AND Preco.Pacote = 0", database);
                }
            }

            var query = conSite.Query<Setor, Preco, int>(strQuery, addResult, new
            {
                apresentacaoID = idApresentacao,
                CotaNominal = comCotaNominal,
                CotaPromocional = comCotaPromocional,

            });

            var canalEvento = new CanalEvento();
            var taxas = canalEvento.BuscaTaxasMinMaxEConveniencia(canalID, eventoID);

            foreach (var setor in result)
            {
                foreach (var preco in setor.Preco)
                {
                    var taxaConveniencia = (taxas[2] / 100m) * preco.Valor;

                    if (taxaConveniencia < taxas[0] && taxas[0] > 0)
                        taxaConveniencia = Decimal.Round(taxas[0], 2);
                    else if (taxaConveniencia > taxas[1] && taxas[1] > 0)
                        taxaConveniencia = Decimal.Round(taxas[1], 2);
                    else
                        taxaConveniencia = Decimal.Round((decimal)taxaConveniencia, 2);

                    preco.TaxaConveniencia = taxaConveniencia;
                }
            }

            return result.ToList();
        }


        public EstatisticaIngressos ConsultarEstatisticaSetor(int eventoId, int apresentacaoId, int setorId)
        {

            var strCon = ConfigurationManager.AppSettings["Conexao"];
            var builder = new SqlConnectionStringBuilder(strCon);
            var database = builder.InitialCatalog;

            var queryStr = string.Format(@"
                SELECT  {3}.dbo.GetQuantidadeIngressos({0},{1},{2},null) as TotalIngressos, 
	                    ({3}.dbo.GetQuantidadeIngressos({0},{1},{2},'V') + IngressosNovo.dbo.GetQuantidadeIngressos({0},{1},null,'I')) as Vendidos, 
		                {3}.dbo.GetQuantidadeIngressos({0},{1},{2},'D') as Disponiveis,
		                {3}.dbo.GetQuantidadeIngressos({0},{1},{2},'R') as Reservados", eventoId, apresentacaoId,setorId, database);

            var query = conIngresso.Query<EstatisticaIngressos>(queryStr);

            return query.FirstOrDefault();
        }

        public List<Setor> ListarOSESP(int idApresentacao, bool comCotaNominal = true, bool comCotaPromocional = true)
        {
            string strQuery = "";
            result = new List<Setor>();
            if (!comCotaNominal || !comCotaPromocional)
            {
                string parametro = "";
                if (!comCotaNominal)
                {
                    parametro = "cotaNominal = 1";
                }
                if (!comCotaPromocional)
                {
                    parametro += (string.IsNullOrEmpty(parametro) ? "" : " OR ") + "cotaParceiro = 1";
                }

                strQuery = @"SELECT 
                           setor.ID,setor.IR_SetorID,setor.Nome,setor.LugarMarcado,setor.ApresentacaoID,setor.QtdeDisponivel,setor.QuantidadeMapa,setor.Obs,setor.AprovadoPublicacao,setor.PrincipalPrecoID,setor.CodigoSala,setor.NVendeLugar
	                      ,preco.ID,preco.IR_PrecoID,preco.Nome,preco.Valor,preco.ApresentacaoID,preco.SetorID,preco.QuantidadePorCliente,preco.Pacote,preco.Serie,preco.CodigoCinema
                          FROM Setor (NOLOCK)
                          INNER JOIN Apresentacao ON Apresentacao.IR_ApresentacaoID = Setor.ApresentacaoID
                          INNER JOIN Preco (NOLOCK) on Preco.SetorID = setor.IR_SetorID AND preco.ApresentacaoID = setor.ApresentacaoID
                          WHERE preco.apresentacaoID = @apresentacaoID AND Preco.Pacote = 0 AND preco.IR_PrecoID not in (Select PrecoID from vwPrecoCotaControle where (" + parametro + ")) AND Apresentacao.EventoID in (select ID from API_Osesp_Eventos)";
            }
            else
            {
                strQuery = @"SELECT  
                           setor.ID,setor.IR_SetorID,setor.Nome,setor.LugarMarcado,setor.ApresentacaoID,setor.QtdeDisponivel,setor.QuantidadeMapa,setor.Obs,setor.AprovadoPublicacao,setor.PrincipalPrecoID,setor.CodigoSala,setor.NVendeLugar
	                      ,preco.ID,preco.IR_PrecoID,preco.Nome,preco.Valor,preco.ApresentacaoID,preco.SetorID,preco.QuantidadePorCliente,preco.Pacote,preco.Serie,preco.CodigoCinema
                          FROM Setor (NOLOCK)
                          INNER JOIN Apresentacao ON Apresentacao.IR_ApresentacaoID = Setor.ApresentacaoID
                          INNER JOIN Preco (NOLOCK) on Preco.SetorID = setor.IR_SetorID AND preco.ApresentacaoID = setor.ApresentacaoID
                          where Setor.ApresentacaoID = @apresentacaoID AND Preco.Pacote = 0 AND Apresentacao.EventoID in (select ID from API_Osesp_Eventos)";
            }

            var query = conSite.Query<Setor, Preco, int>(strQuery, addResult, new
            {
                apresentacaoID = idApresentacao,
                CotaNominal = comCotaNominal,
                CotaPromocional = comCotaPromocional
            });
            return result.ToList();

        }

        public List<Setor> ListarVoucher(int parceiroMidiaID, int apresentacaoID, int nivel)
        {
            string sql = @"Select st.ID,st.IR_SetorID,st.Nome,st.LugarMarcado,st.ApresentacaoID,st.QtdeDisponivel,st.QuantidadeMapa,st.Obs,st.AprovadoPublicacao,st.PrincipalPrecoID,st.CodigoSala,st.NVendeLugar  
                            From vwParceiroMidiaClasseSetor pmcs
                            INNER JOIN Apresentacao ap (NOLOCK) on ap.IR_ApresentacaoID = pmcs.ApresentacaoID
							INNER JOIN Setor st(NOLOCK) on st.Ir_SetorID = pmcs.SetorID and st.ApresentacaoID = pmcs.ApresentacaoID
		                    		                    INNER JOIN PrecoParceiroMidia ppm (NOLOCK) on ppm.ApresentacaoID = ap.IR_ApresentacaoID and pmcs.ParceiroMidiaID = ppm.ParceiroMidiaID and st.IR_SetorID = ppm.setorID
		                    WHERE pmcs.ParceiroMidiaID = @parceiroMidiaID and pmcs.ApresentacaoID = @apresentacaoID and pmcs.nivel <= @nivel";
            var result = conSite.Query<Setor>(sql, new
            {
                parceiroMidiaID = parceiroMidiaID,
                apresentacaoID = apresentacaoID,
                nivel = nivel
            }).ToList();
            return result;
        }
    }
}
