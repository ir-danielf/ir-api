using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Mpetodo que retorna a diferença entre duas datas sem considerar finais de semana
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static TimeSpan DiffWorkDays(this DateTime data1, DateTime data2)
        {

            var multiplicador = 1;
            DateTime dataFinal = data2;
            if (data2 < data1)
            {
                dataFinal = data1;
                data1 = data2;
                multiplicador = -1;
            }


            DateTime dataInicial = data1.AddDays(1);

            if (data1.DayOfWeek == DayOfWeek.Friday)
            {
                dataInicial = data1.AddDays(3);
            }
            if (data1.DayOfWeek == DayOfWeek.Saturday)
            {
                dataInicial = data1.AddDays(2);
            }
            int dataFinalDiaSemana = dataFinal.DayOfWeek.ValueAsInt();
            int diaSemanaConsiderado = (dataInicial.DayOfWeek.ValueAsInt() - 1);
            int diferencaConsiderada = Convert.ToInt32((dataFinal - dataInicial).TotalDays) + diaSemanaConsiderado;
            int diasSemanaResto = Convert.ToInt32(diferencaConsiderada % 7);
            if (diasSemanaResto > 5)
                diasSemanaResto = 5;
            int diasUteis = (Convert.ToInt32(diferencaConsiderada / 7) * 5) + diasSemanaResto - diaSemanaConsiderado + (dataFinalDiaSemana > 0 && dataFinalDiaSemana < 6 ? 1 : 0);
            return new TimeSpan(diasUteis * multiplicador, 0, 0, 0);
        }
    }
}
