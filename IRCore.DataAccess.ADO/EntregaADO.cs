using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class EntregaADO : MasterADO<dbIngresso>
    {
        public EntregaADO(MasterADOBase ado = null) : base(ado) { }
        

        public tEntregaAreaBlackList VerificarCEPBlackList(string cep)
        {
            return this.conIngresso.Query<tEntregaAreaBlackList>(
                @"SELECT TOP 1 * FROM tEntregaAreaBlackList (NOLOCK) 
                WHERE @cep BETWEEN CAST(CepInicial AS INT) and CAST(CepFinal AS INT)"
                .Replace("@cep", cep)
            ).FirstOrDefault();
        }
        
        /// <summary>
        /// Retorna uma lista de entregasControles disponiveis para uma lista de eventos
        /// </summary>
        /// <param name="eventosIds"></param>
        /// <returns></returns>
        public List<tEntregaControle>ListarInEventos(List<int> eventosIds)
        {
            var queryString = @"
SELECT
	ec.ID,
    ec.EntregaID,
    ec.EntregaAreaID,
    ec.PeriodoID,
    ec.QuantidadeEntregas,
    ec.Valor,
    ec.DiasTriagem,
    ec.Ativa,
	e.ID,
	e.Nome,
    e.PrazoEntrega,
    e.Disponivel,
    CASE
		WHEN eec.ProcedimentoEntrega <> '' AND eec.ProcedimentoEntrega IS NOT NULL THEN eec.ProcedimentoEntrega
		WHEN ec.ProcedimentoEntrega <> '' AND ec.ProcedimentoEntrega IS NOT NULL THEN ec.ProcedimentoEntrega
		ELSE e.ProcedimentoEntrega
	END AS ProcedimentoEntrega,
    e.EnviaAlerta,
    e.Padrao,
    e.PermitirImpressaoInternet,
    e.Tipo,
    e.Ativo,
    e.DiasTriagem,
    e.De,
    e.Ate
FROM
	tEntrega (NOLOCK) e
	INNER JOIN tEntregaControle (NOLOCK) ec ON ec.EntregaID = e.ID
	INNER JOIN tEventoEntregaControle (NOLOCK) eec ON eec.EntregaControleID = ec.ID
WHERE
	eec.EventoID IN (@EventosIDs) 
	AND ec.Ativa = 'T' 
	AND e.Ativo = 'T'
GROUP BY 
	ec.ID,
    ec.EntregaID,
    ec.EntregaAreaID,
    ec.PeriodoID,
    ec.QuantidadeEntregas,
    ec.Valor,
    ec.DiasTriagem,
    ec.Ativa,
	e.ID,
	e.Nome,
    e.PrazoEntrega,
    e.Disponivel,
	eec.ProcedimentoEntrega,
	ec.ProcedimentoEntrega,
	e.ProcedimentoEntrega,
	e.EnviaAlerta,
    e.Padrao,
    e.PermitirImpressaoInternet,
    e.Tipo,
    e.Ativo,
    e.DiasTriagem,
    e.De,
    e.Ate
HAVING 
	Count(eec.EventoID) = @Count
ORDER BY e.Nome";

            var result = conIngresso.Query<tEntregaControle, tEntrega, tEntregaControle>(queryString, addResultListarInEventos, new 
            {
                EventosIDs = eventosIds,
                Count = eventosIds.Count()
            });

            return result.ToList();
        }

        public tEntregaControle addResultListarInEventos(tEntregaControle entregaControle, tEntrega entrega)
        {
            return new EntregaControleModelQuery()
            {
                entregaControle = entregaControle,
                entrega = entrega
            }.toEntregaControle();
        }
       
        public int CountFeriados(DateTime dataInicial, DateTime dataFinal, List<int> diasSemana)
        {
            string queryString = @"
                            SELECT
	                            Count(ID)
                            FROM 
	                            tFeriado (NOLOCK)
                            WHERE
	                            CalcHorario >= @DataInicial AND CalcHorario <= @DataFinal AND CalcDiaDaSemana IN (@DiasSemana)";
            queryString = queryString.Replace("@DiasSemana", String.Join(",", diasSemana));
            int result = conIngresso.Query<int>(queryString, new
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal
            }).FirstOrDefault();
            return result;
        }
        
        /// <summary>
        /// Lista EntregaControles que possuem area de entrega para os endereços passados
        /// </summary>
        /// <returns></returns>
        public List<tEntregaArea> ConsultarEntregaArea(string cep)
        {
            return this.conIngresso.Query<tEntregaArea>(
                @"SELECT ea.ID, ea.Nome
                   FROM tEntregaArea (NOLOCK) ea
                JOIN tEntregaAreaCep (NOLOCK) eac on ea.ID = eac.EntregaAreaID
                WHERE @cep BETWEEN CAST(eac.CepInicial AS INT) and CAST(eac.CepFinal AS INT)"
                .Replace("@cep", cep)
            ).ToList();
        }

    }
}
