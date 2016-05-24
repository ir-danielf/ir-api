using CTLib;
using IRLib.Emails;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.EstornoDadosPlanilhas.Operadoras
{
	public class Cielo
	{
		public int ID { get; set; }
		public string Estabelecimento { get; set; }
		public string Lote { get; set; }
		public string DataDepósito { get; set; }
		public string CartãoXID { get; set; }
		public string DataVenda { get; set; }
		public string ValorVenda { get; set; }
		public string ValorCancelar { get; set; }
		public string Autorizacao { get; set; }
		public string Motivo { get; set; }
        public string SenhaVenda { get; set; }

        private static IRLib.Emails.MailServiceSoapClient _oService_Emails;
        public static IRLib.Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new IRLib.Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        public static void GeraPlanilhaCielo(string strPath, string emailEnvio)
		{
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string arquivCopia = strPath + "Cielo_" + data + ".xlsx";
            //string arquivCopia = @"C:\Users\tiago.dias\Downloads\Layouts Planilhas IR\ValeCultura.xlsx";
            string nomeArquivo = "Cielo_" + data + ".xlsx";

            //string arquivCopia = @"C:\Service\Planilhas\Cielo.xlsx";
            //string arquivCopia = @"C:\Users\tiago.dias\Downloads\Layouts Planilhas IR\Cielo.xlsx";
			//string nomeArquivo = "Cielo.xlsx";
			//string emailTiago = "tiago.dias@rcadigital.com.br";
            //string emailEnvio = ConfigurationManager.AppSettings["EmailEnvioPlanilhaEstorno"].ToString();

			ExcelPackage package = new ExcelPackage();
			ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

			//ExcelRange celulas = new ExcelRange();
			ws.Cells["A1"].Value = "ESTABELECIMENTO";
			ws.Cells["B1"].Value = "LOTE";
			ws.Cells["C1"].Value = "DT DEPÓSITO";
			ws.Cells["D1"].Value = "CARTÃO / XID";
			ws.Cells["E1"].Value = "DT VENDA";
			ws.Cells["F1"].Value = "VL VENDA";
			ws.Cells["G1"].Value = "VL CANCELAR";
			ws.Cells["H1"].Value = "AUT.";
			ws.Cells["I1"].Value = "MOTIVO";
            ws.Cells["J1"].Value = "SENHA VENDA";
            ////////////////////////////////////////////////////////////////////////////////////////////////////

			List<Cielo> lstCielo = Cielo.getEstornosCielo();

            if (lstCielo.Count > 0)
            {

                int linha = 2;
                foreach (Cielo item in lstCielo)
                {
                    ws.Cells[linha, 1].Value = item.Estabelecimento;
                    ws.Cells[linha, 2].Value = item.Lote;
                    ws.Cells[linha, 3].Value = item.DataDepósito;
                    ws.Cells[linha, 4].Value = item.CartãoXID;
                    ws.Cells[linha, 5].Value = item.DataVenda;
                    ws.Cells[linha, 6].Value = item.ValorVenda;
                    ws.Cells[linha, 7].Value = item.ValorCancelar;
                    ws.Cells[linha, 8].Value = item.Autorizacao;
                    ws.Cells[linha, 9].Value = item.Motivo;
                    ws.Cells[linha, 10].Value = item.SenhaVenda;
                    linha++;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                ws.Cells.AutoFitColumns();

                FileInfo fi = new FileInfo(arquivCopia);
                package.SaveAs(fi);

                BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (Cielo item in lstCielo)
                {
                    item.setPlanilhaGerada();
                }

                string[] lstEmails = emailEnvio.Split(';');

                byte[] arrAnexo = br.ReadBytes((int)fi.Length);

                foreach (string email in lstEmails)
                {
                    IRLib.Emails.Retorno retorno = GetInstance().
                        EnviarEmailCancelamentoEstornoOperadora(email, "Estorno Planilhas", arrAnexo, nomeArquivo);
                }
               InsertHistorico("Visa", arquivCopia, nomeArquivo, emailEnvio);
            }
		}
		public bool setPlanilhaGerada()
		{
			BD bd = new BD();
			StringBuilder str = new StringBuilder();
			str.Append("UPDATE EstornoDadosCartaoCredito ");
			str.Append("SET PlanilhaGerada = 1, Status = 'O'");
			str.Append("WHERE ID = " + this.ID);

			if (bd.Executar(str) > 0)
				return true;
			else
				return false;
		}

        private static bool InsertHistorico(string bandeira, string caminho, string nomearquivo, string email)
        {
            BD bd = new BD();
            StringBuilder str = new StringBuilder();
            str.Append("INSERT INTO EstornoHistoricoPlanilhas");
            str.Append("(Bandeira, ArquivoEnvio, EmailEnvio, DataEnvio) ");
            str.Append(" VALUES ('" + bandeira + "','" + nomearquivo + "','" + email + "',GETDATE())");

            if (bd.Executar(str) > 0)
                return true;
            else
                return false;
        }

		public static List<Cielo> getEstornosCielo()
		{
			DateTime dt = System.DateTime.Now;
			BD bd = new BD();

			StringBuilder str = new StringBuilder();
			str.Append("SELECT est.ID, est.Cartao, b.Nome, vbfp.NSUHost, vbfp.NSUSitef, vbfp.NumeroAutorizacao, vb.DataVenda, ");
            str.Append("vb.Senha AS SenhaVenda, ");
            str.Append("vb.ValorTotal as ValorVenda, est.Valor as ValorEstorno, fp.Parcelas, fp.Nome ,fp.Tipo, fpt.Nome, vb.id as VendaBilheteriaID ");
            str.Append("FROM EstornoDadosCartaoCredito est (NOLOCK) ");
            str.Append("inner join tVendaBilheteria vb (nolock) on vb.ID = est.VendaBilheteriaIDvenda ");
			str.Append("inner join tVendaBilheteriaFormaPagamento vbfp (nolock) on vbfp.VendaBilheteriaID = vb.ID ");
			str.Append("inner join tFormaPagamento fp (nolock) on fp.ID = vbfp.FormaPagamentoID ");
			str.Append("inner join tFormaPagamentoTipo fpt (nolock) on fp.FormaPagamentoTipoID = fpt.ID ");
			str.Append("inner join tBandeira b (nolock) on fp.BandeiraID = b.ID ");
			str.Append("WHERE est.PlanilhaGerada = 0 ");
            str.Append("AND b.Nome IN ('Visa', 'Elo') AND est.Status = 'P' ");
            //str.Append("AND (vbfp.NSUHost <> 0 OR vbfp.NSUSitef <> 0) ");

			List<Cielo> lstCielo = new List<Cielo>();

			bd.Consulta(str.ToString());
            
            VendaBilheteriaItem vbi = new VendaBilheteriaItem();
			
            while (bd.Consulta().Read())
			{
                IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamento;

                TipoPagamento = vbi.TipoPagamentoBilheteria(bd.LerInt("VendaBilheteriaID"));

				Cielo cielo = new Cielo();
				cielo.ID = bd.LerInt("ID");
                switch (TipoPagamento)
                {
                    case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF:
                        cielo.Estabelecimento = "1037983464";//Valor default para compras via TEF
                        break;
                    case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen:
                        cielo.Estabelecimento = "1037590390";//Valor default para compras via Adyen
                        break;
                    default:
                        cielo.Estabelecimento = "1037983464";//Valor default
                        break;
                }
				//cielo.Estabelecimento = "1037983464";//Valor default por enquanto
				cielo.Lote = string.Empty;
				cielo.DataDepósito = string.Empty;
				cielo.CartãoXID = bd.LerString("Cartao");
				cielo.DataVenda = bd.LerStringFormatoData("DataVenda");
                cielo.ValorVenda = bd.LerString("ValorVenda"); ;
                cielo.ValorCancelar = bd.LerString("ValorEstorno");
				cielo.Autorizacao = bd.LerString("NumeroAutorizacao");
				cielo.Motivo = "";
                cielo.SenhaVenda = bd.LerString("SenhaVenda");
				lstCielo.Add(cielo);
			}

			return lstCielo;
		}

        private static string getNSUConciliacao(int idVenda)
        {
            DateTime dt = System.DateTime.Now;
            BD bd = new BD();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT v.NSUHost FROM ");
            str.Append("Conciliacao..vendas v (NOLOCK) ");
            str.Append("WHERE v.idVenda = {0} ");

            bd.Consulta(string.Format(str.ToString(), idVenda.ToString()));

            return bd.LerString("NSUHost");
        }

        private static string getAutorizacaoConciliacao(int idVenda)
        {
            DateTime dt = System.DateTime.Now;
            BD bd = new BD();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT v.NumeroAutorizacao FROM ");
            str.Append("Conciliacao..vendas v (NOLOCK) ");
            str.Append("WHERE v.idVenda = {0} ");

            bd.Consulta(string.Format(str.ToString(), idVenda.ToString()));

            return bd.LerString("NumeroAutorizacao");
        }
	}
}
