namespace IRCore.DataAccess.Model
{
    using System;
    using System.Globalization;

    public partial class Apresentacao
    {
        public virtual tMapaEsquematico MapaEsquematico { get; set; }

        public DateTime HorarioAsDateTime
        {
            get
            {
                try
                {
                    return DateTime.ParseExact(Horario, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    return DateTime.Now;
                }

            }
            set { Horario = value.ToString("yyyyMMddHHmmss"); }
        }

        public int QtdeDisponivel { get; set; }

        public Preco MaiorPreco { get; set; }

        public Preco MenorPreco { get; set; }

        public int CotaID { get; set; }

        public EstatisticaIngressos Estatistica { get; set; }
    }
}
