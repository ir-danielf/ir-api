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
	public class ValeCultura
	{
		public int ID { get; set; }
		public string CNPJLoja { get; set; }
		public string NumeroCartao { get; set; }
		public string DataTransacao { get; set; }
		public string ValorTransacao { get; set; }
        public string ValorParcial { get; set; }
		public string CodigoAutorizavaoVenda { get; set; }
		public string RetornoTicket { get; set; }
		public string Motivo { get; set; }
        public string SenhaVenda { get; set; }

        private static IRLib.Emails.MailServiceSoapClient _oService_Emails;
        public static IRLib.Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new IRLib.Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        public static void GeraPlanilhaValeCultura(string strPath, string emailEnvio, string BandeiraCultura)
		{
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string arquivCopia;// = @"C:\Service\Planilhas\" + BandeiraCultura.Trim() + "_" + data + ".xlsx";
            //string arquivCopia = @"C:\Users\tiago.dias\Downloads\Layouts Planilhas IR\ValeCultura.xlsx";
            string nomeArquivo;// = BandeiraCultura.Trim() + "_" + data + ".xlsx";

            if (BandeiraCultura == "Vale Cultura")
            {
                arquivCopia = strPath + "ValeCultura_" + data + ".xlsx";
                nomeArquivo = "ValeCultura_" + data + ".xlsx";
            }
            else
            {
                arquivCopia = strPath + "EloCultura_" + data + ".xlsx";
                nomeArquivo = "EloCultura_" + data + ".xlsx";
            }
            //string emailTiago = "tiago.dias@rcadigital.com.br";
            //string emailEnvio = ConfigurationManager.AppSettings["EmailEnvioPlanilhaEstorno"].ToString();

			ExcelPackage package = new ExcelPackage();
			ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

			//ExcelRange celulas = new ExcelRange();
			ws.Cells["A1"].Value = "CNPJ DA LOJA";
			ws.Cells["B1"].Value = "NÚMERO DO CARTÃO";
			ws.Cells["C1"].Value = "DATA DA TRANSAÇÃO";
			ws.Cells["D1"].Value = "VALOR DA TRANSAÇÃO";
            ws.Cells["D1"].Value = "VALOR DO ESTORNO";
			ws.Cells["E1"].Value = "CÓDIGO DE AUTORIZAÇÃO DA VENDA";
			ws.Cells["F1"].Value = "RETORNO TICKET";
			ws.Cells["G1"].Value = "MOTIVO";
            ws.Cells["H1"].Value = "SENHA VENDA";
			////////////////////////////////////////////////////////////////////////////////////////////////////

            List<ValeCultura> lstValeCultura = ValeCultura.getEstornosValeCultura(BandeiraCultura);

            if (lstValeCultura.Count > 0)
            {
                int linha = 2;
                foreach (ValeCultura item in lstValeCultura)
                {
                    ws.Cells[linha, 1].Value = item.CNPJLoja;
                    ws.Cells[linha, 2].Value = item.NumeroCartao;
                    ws.Cells[linha, 3].Value = item.DataTransacao;
                    ws.Cells[linha, 4].Value = item.ValorTransacao;
                    ws.Cells[linha, 5].Value = item.ValorParcial;
                    ws.Cells[linha, 6].Value = item.CodigoAutorizavaoVenda;
                    ws.Cells[linha, 7].Value = item.RetornoTicket;
                    ws.Cells[linha, 8].Value = item.Motivo;
                    ws.Cells[linha, 9].Value = item.SenhaVenda;
                    linha++;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                ws.Cells.AutoFitColumns();

                //package.SaveAs(new FileInfo(arquivCopia));

                FileInfo fi = new FileInfo(arquivCopia);
                package.SaveAs(fi);

                BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (ValeCultura item in lstValeCultura)
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

                InsertHistorico(BandeiraCultura, arquivCopia, nomeArquivo, emailEnvio);
            }
		}

		public bool setPlanilhaGerada()
		{
			BD bd = new BD();
			StringBuilder str = new StringBuilder();
			str.Append("UPDATE EstornoDadosCartaoCredito ");
			str.Append("SET PlanilhaGerada = 1, Status = 'O' ");
			str.Append("WHERE ID = " + this.ID);
            //str.Append(" AND b.Nome NOT IN ('Mastercard', 'Visa') ");

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
		public static List<ValeCultura> getEstornosValeCultura(string BandeiraCultura)
		{
			DateTime dt = System.DateTime.Now;
			BD bd = new BD();

			StringBuilder str = new StringBuilder();
			str.Append("SELECT est.ID, est.Cartao, b.Nome, vbfp.NSUHost, vbfp.NSUSitef, vbfp.NumeroAutorizacao, vb.DataVenda, ");
			str.Append("est.Valor AS ValorTotal, ");
            str.Append("vb.Senha AS SenhaVenda, ");
            str.Append("CASE (vb.ValorTotal - est.Valor) ");
            str.Append("WHEN 0 THEN 0 ELSE est.Valor END AS ValorParcial, fp.Parcelas, fp.Nome ,fp.Tipo, fpt.Nome ");
            str.Append("FROM EstornoDadosCartaoCredito est (NOLOCK) ");
            str.Append("INNER JOIN tVendaBilheteria vb (nolock) on vb.ID = est.VendaBilheteriaIDvenda ");
			str.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (nolock) on vbfp.VendaBilheteriaID = vb.ID ");
			str.Append("INNER JOIN tFormaPagamento fp (nolock) on fp.ID = vbfp.FormaPagamentoID ");
			str.Append("INNER JOIN tFormaPagamentoTipo fpt (nolock) on fp.FormaPagamentoTipoID = fpt.ID ");
			str.Append("INNER JOIN tBandeira b (nolock) on fp.BandeiraID = b.ID ");
			str.Append("WHERE est.PlanilhaGerada = 0 ");
            str.Append("AND b.Nome IN ('" + BandeiraCultura + "')  AND est.Status = 'P' ");
            //str.Append("AND (vbfp.NSUHost <> 0 OR vbfp.NSUSitef <> 0) ");

			List<ValeCultura> lstValeCultura = new List<ValeCultura>();

			bd.Consulta(str.ToString());

			while (bd.Consulta().Read())
			{
				ValeCultura valeCultura = new ValeCultura();
				valeCultura.ID = bd.LerInt("ID");
				valeCultura.CNPJLoja = "15150423/0001-65";//Valor default por enquanto
				valeCultura.NumeroCartao = bd.LerString("Cartao"); ;
				valeCultura.DataTransacao = bd.LerString("DataVenda"); ;
                valeCultura.ValorTransacao = bd.LerString("ValorTotal");
                valeCultura.ValorParcial = bd.LerString("ValorParcial");
				valeCultura.CodigoAutorizavaoVenda = bd.LerString("NumeroAutorizacao");
				valeCultura.RetornoTicket = "";
				valeCultura.Motivo = "";
                valeCultura.SenhaVenda = bd.LerString("SenhaVenda");
				lstValeCultura.Add(valeCultura);
			}

			return lstValeCultura;
		}
	}
}
