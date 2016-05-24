namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using IRCore.Util;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class Evento
    {
        public virtual Local Local { get; set; }

        public virtual EventoSubtipo Subtipo { get; set; }
        public virtual Tipo Tipo { get; set; }

        public DateTime? DataAberturaVendaAsDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(DataAberturaVenda))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return DateTime.ParseExact(DataAberturaVenda, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return null;
                    }
                    
                }
            }
            set
            {
                if (value == null)
                {
                    DataAberturaVenda = null;
                }
                else
                {
                    DataAberturaVenda = value.Value.ToString("yyyyMMdd");
                }
            }
        }

        public Apresentacao PrimeiraApresentacao { get; set; }
        public Apresentacao UltimaApresentacao { get; set; }

        public int QtdeDisponivel { get; set; }
        public Preco MaiorPreco { get; set; }
        public Preco MenorPreco { get; set; }
        public int QuantidadeApresentacoes { get; set; }

        public int Ordem { get; set; }

        public decimal MenorPrecoEvento { get; set; }

        public enumEventoSemVendaMotivo PublicarSemVendaMotivoAsEnum
        {
            get 
            {
                try
                {
                    return (enumEventoSemVendaMotivo)PublicarSemVendaMotivo; 
                }
                catch
                {
                    return enumEventoSemVendaMotivo.vendaDisponivel;
                }    
            }
            set { PublicarSemVendaMotivo = ((int)value); }
        }

        public Dictionary<enumEventoTipoMidiaTipo, Dictionary<string, List<string>>> EventoMidias { get; set; }

    }
}
