using System;

namespace IRLib.Assinaturas.Models.Relatorios
{
    public class AcompanhamentoVenda
    {
        public int Cortesia { get; set; }

        public int Serie { get; set; }

        public int Assinatura { get; set; }

        public string Evento { get; set; }

        public DateTime Horario { get; set; }

        public int Bloqueado { get; set; }

        public int Disponivel { get; set; }

        public int Avulso { get; set; }

        public string NomeSerie { get; set; }
    }
}
