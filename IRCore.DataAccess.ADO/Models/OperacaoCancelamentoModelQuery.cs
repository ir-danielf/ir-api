﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.ADO.Models
{
    public class OperacaoCancelamentoModelQuery
    {
        public int VendaBilheteriaID { get; set; }
        public int CanalID { get; set; }
        public int LojaID { get; set; }
        public int IngressoID { get; set; }
        public char Operacao { get; set; }
    }
}
