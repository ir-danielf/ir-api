using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public static class ApresentacaoExtensions
    {
        public static Apresentacao toApresentacao(this tApresentacao tApresentacao)
        {
            return new Apresentacao()
                                {
                                CalcDiaDaSemana = tApresentacao.CalcDiaDaSemana,
                                CalcHorario = tApresentacao.CalcHorario,
                                CodigoProgramacao = tApresentacao.CodigoProgramacao,
                                EventoID = tApresentacao.EventoID??0,
                                Horario = tApresentacao.Horario,
                                ID = tApresentacao.ID,
                                IR_ApresentacaoID = tApresentacao.ID,
                                MaiorPreco = null,
                                MenorPreco = null,
                                Preco = null,
                                PrecoParceiroMidia = null,
                                Programacao = tApresentacao.Programacao,
                                QtdeDisponivel = 0,
                                Setor = null,
                                UsarEsquematico = false
                            };
        }
    }
}
