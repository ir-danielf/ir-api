using System;

namespace IngressoRapido.Lib
{
	public class Reserva
	{
		public enum enumTipoReserva
		{
			Avulso,
			Pacote
		}
		private string setor = string.Empty;

		public string Setor
		{
			get
			{
				if (SetorTipo == IRLib.Setor.Pista)
					return this.setor;
				else
					return this.setor + " (lugar marcado)";
			}
			set
			{
				this.setor = value;
			}
		}

		public string Pacote { get; set; }
		public enumTipoReserva TipoReserva { get; set; }
		public string Evento { get; set; }
		public DateTime Apresentacao { get; set; }
		public string Codigo { get; set; }
		public string SetorTipo { get; set; }

		public Reserva Copiar()
		{
			Reserva oReserva = new Reserva();
			oReserva.Evento = this.Evento;
			oReserva.Apresentacao = this.Apresentacao;
			oReserva.SetorTipo = this.SetorTipo;
			oReserva.Setor = this.setor;
			oReserva.Codigo = this.Codigo;
			return oReserva;
		}
	}
}
