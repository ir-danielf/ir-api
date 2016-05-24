using IRCore.BusinessObject.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRAPI.Models
{
    public class NpsAdicionarAgendamentoModel
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public int Delay { get; set; }

        public string Canal { get; set; }

    }

    public class NpsAtualizarAgendamentoRange
    {

        public int IdAgendamentoInicial { get; set; }

        public int IdAgendamentoFinal { get; set; }

        public enumStatusNPS Status { get; set; }

    }
    
    public class NpsAtualizarAgendamento
    {

        public int IdAgendamento { get; set; }

        public enumStatusNPS Status { get; set; }

    }

    public class NpsModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Delay { get; set; }

        public string Status { get; set; }

        public string Canal { get; set; }

        public Nullable<DateTime> DataInclusao { get; set; }

        public Nullable<DateTime> DataEnvio { get; set; }

    }
}