using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
	partial class EstornoDadosCartaoCredito
	{
		public string SenhaCancelamento { get; set; }
		public string SenhaCompra { get; set; }
        public string NSU { get; set; }
        public string NumeroAutorizacao { get; set; }
        public int VendaBilFormPagID { get; set; }
	}
}
