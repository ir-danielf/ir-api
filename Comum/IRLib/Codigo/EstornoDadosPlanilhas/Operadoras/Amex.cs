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
	public class Amex
	{
		public int ID {get; set;}
		public string Estabelecimento { get; set; }
		public string NumeroCartao { get; set; }
		public string DataVenda { get; set; }
		public string NSU { get; set; }
		public string Autorizacao { get; set; }
		public string ValorVenda { get; set; }
		public string ValorCancelar { get; set; }
        public string SenhaVenda { get; set; }
        public char Status { get; set; }

        private static IRLib.Emails.MailServiceSoapClient _oService_Emails;
        public static IRLib.Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new IRLib.Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        public static void GeraPlanilhaAmexAVista(string strPath, string emailEnvio)
		{
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string arquivCopia = strPath + "Amex - Á Vista_" + data + ".xlsx";
            string nomeArquivo = "Amex - Á Vista_" + data + ".xlsx";

			ExcelPackage package = new ExcelPackage();
			ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

			ws.Cells["A1"].Value = "ESTABELECIMENTO";
			ws.Cells["B1"].Value = "NÚMERO DO CARTÂO";
			ws.Cells["C1"].Value = "DT VENDA";
			ws.Cells["D1"].Value = "NSU";
			ws.Cells["E1"].Value = "AUTORIZAÇÃO";
			ws.Cells["F1"].Value = "VLR VENDA";
			ws.Cells["G1"].Value = "VLR CANCELAR";
            ws.Cells["H1"].Value = "SENHA VENDA";
            ////////////////////////////////////////////////////////////////////////////////////////////////////
			//int linha = 2;

			List<Amex> lstAmexVista = Amex.getEstornosAmex();

            if (lstAmexVista.Count > 0)
            {
                int linha = 2;
                foreach (Amex item in lstAmexVista)
                {
                    if (item.Status == 'R')
                    {
                        ws.Cells[linha, 1].Value = item.Estabelecimento;
                        ws.Cells[linha, 2].Value = item.NumeroCartao;
                        ws.Cells[linha, 3].Value = item.DataVenda;
                        ws.Cells[linha, 4].Value = item.NSU;
                        ws.Cells[linha, 5].Value = item.Autorizacao;
                        ws.Cells[linha, 6].Value = item.ValorVenda;
                        ws.Cells[linha, 7].Value = item.ValorCancelar;
                        ws.Cells[linha, 8].Value = item.SenhaVenda;
                        linha++;
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                ws.Cells.AutoFitColumns();

                FileInfo fi = new FileInfo(arquivCopia);
                package.SaveAs(fi);

                BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (Amex item in lstAmexVista)
                {
                    if (item.Status == 'R')
                        item.setPlanilhaGerada();

                    else if (item.Status == 'W')
                        item.setItemWait();
                }

                bool registrosNovos = lstAmexVista.ToList().Count(t => t.Status.Equals('R')) > 0;

                if (registrosNovos)
                {
                    string[] lstEmails = emailEnvio.Split(';');

                    byte[] arrAnexo = br.ReadBytes((int)fi.Length);

                    foreach (string email in lstEmails)
                    {
                        IRLib.Emails.Retorno retorno = GetInstance().
                            EnviarEmailCancelamentoEstornoOperadora(email, "Estorno Planilhas", arrAnexo, nomeArquivo);
                    }

                    InsertHistorico("Amex - A Vista", arquivCopia, nomeArquivo, emailEnvio);
                }
            }
		}
		public bool setPlanilhaGerada()
		{
			BD bd = new BD();
			StringBuilder str = new StringBuilder();
			str.Append("UPDATE EstornoDadosCartaoCredito ");
			str.Append("SET PlanilhaGerada = 1, Status = 'O' ");
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
            str.Append(" VALUES ('" + bandeira + "','" + nomearquivo + "','"+email+"',GETDATE())");

			if (bd.Executar(str) > 0)
				return true;
			else
				return false;
		}

        public static List<Amex> getEstornosAmex()
		{
			DateTime dt = System.DateTime.Now;
			BD bd = new BD();

			StringBuilder str = new StringBuilder();
			str.Append("SELECT est.ID, est.Cartao, b.Nome, vbfp.NSUHost, vbfp.NSUSitef, vbfp.NumeroAutorizacao, vb.DataVenda, ");
            str.Append("vb.Senha AS SenhaVenda, SUBSTRING(vbfp.CodigoIR, 2, 10) AS CodigoIR, c.CanalTipoID, ");
            str.Append("vbfp.Valor, fp.Parcelas, fp.Nome ,fp.Tipo, fpt.Nome, c.NroEstabelecimentoAmex, vb.id as VendaBilheteriaID ");
            str.Append("FROM EstornoDadosCartaoCredito est (NOLOCK)");
            str.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) on vb.ID = est.VendaBilheteriaIDvenda ");
            str.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) on vbfp.VendaBilheteriaID = vb.ID ");
            str.Append("INNER JOIN tFormaPagamento fp (NOLOCK) on fp.ID = vbfp.FormaPagamentoID ");
            str.Append("INNER JOIN tFormaPagamentoTipo fpt (NOLOCK) on fp.FormaPagamentoTipoID = fpt.ID ");
			str.Append("INNER JOIN tBandeira b (NOLOCK) on fp.BandeiraID = b.ID ");
            str.Append("INNER JOIN tCancelDevolucaoPendente cdp (NOLOCK) on cdp.VendaBilheteriaIDCancel = est.VendaBilheteriaIDcancel ");
            str.Append("INNER JOIN tCanal c (NOLOCK) on c.ID = cdp.CanalID ");
            str.Append("WHERE est.PlanilhaGerada = 0 AND est.Status IN ('P', 'W') ");
            //str.Append("WHERE est.PlanilhaGerada IN (0,1) AND est.Status IN ('O', 'P', 'W') ");
            str.Append("AND b.Nome IN ('American Express')  AND fp.Parcelas = 1 ");
            //str.Append("AND (vbfp.NSUHost <> 0 OR vbfp.NSUSitef <> 0) ");

			List<Amex> lstAmex = new List<Amex>();

			bd.Consulta(str.ToString());

            VendaBilheteriaItem vbi = new VendaBilheteriaItem();

			while (bd.Consulta().Read())
			{
                string NSU = "";
                int CanalTipo = bd.LerInt("CanalTipoID");
                int VendaID = bd.LerInt("VendaBilheteriaID");

                Amex amex = new Amex();

                if (CanalTipo == 1 || CanalTipo == 2)
                {
                    IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamento;

                    TipoPagamento = vbi.TipoPagamentoBilheteria(bd.LerInt("VendaBilheteriaID"));

                    amex.ID = bd.LerInt("ID");

                    switch (TipoPagamento)
                    {
                        case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF:
                            amex.Estabelecimento = "9061745048";//Valor default para compras via TEF
                            break;
                        case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen:
                            amex.Estabelecimento = "9061745055";//Valor default para compras via Adyen
                            break;
                        default:
                            amex.Estabelecimento = "";//Se não achou, em branco.....
                            break;
                    }
                }
                else
                    amex.Estabelecimento = GetCodigoIR(bd.LerString("CodigoIR")); //bd.LerString("NroEstabelecimentoAmex");//"1037983464";//Valor default por enquanto

				amex.NumeroCartao = bd.LerString("Cartao");
				amex.DataVenda = bd.LerStringFormatoData("DataVenda");
				//amex.NSU = bd.LerString("NSUSitef");

                NSU = bd.LerString("NSUSitef");// == "" ? getNSUConciliacao(VendaID) : bd.LerString("NSUHost");

                NSU = NSU == "" ? getNSUConciliacao(VendaID) : NSU;

                if (NSU == "" || NSU == "0")
                    amex.Status = 'W';//Wait, aguardar inserir manualmente NSU e Autorização
                else
                {
                    amex.NSU = NSU;
                    amex.Status = 'R';//Ready, pronta para processar
                }

                amex.Autorizacao = bd.LerString("NumeroAutorizacao");
				amex.ValorVenda = bd.LerString("Valor");
				amex.ValorCancelar = bd.LerString("Valor");
                amex.SenhaVenda = bd.LerString("SenhaVenda");
				lstAmex.Add(amex);
			}

			return lstAmex;
		}

        protected static string getNSUConciliacao(int idVenda)
        {
            DateTime dt = System.DateTime.Now;
            BD bd = new BD();

            StringBuilder str = new StringBuilder();
            str.Append("SELECT v.NSUHost FROM ");
            str.Append("Conciliacao..vendas v (NOLOCK) ");
            str.Append("WHERE v.idVenda = {0} ");

            if (bd.Consulta(string.Format(str.ToString(), idVenda.ToString())).Read())
                return bd.LerString("NSUHost");
            else
                return "";
        }

        public bool setItemWait()
        {
            BD bd = new BD();
            StringBuilder str = new StringBuilder();
            str.Append("UPDATE EstornoDadosCartaoCredito ");
            str.Append("SET Status = 'W' ");
            str.Append("WHERE ID = " + this.ID);

            if (bd.Executar(str) > 0)
                return true;
            else
                return false;
        }

        protected static string GetCodigoIR(string NroEstabelecimento)
        {
            string Retorno = string.Empty;

            switch (NroEstabelecimento)
            {
                case "1037983464":
                    Retorno = "9061745048";
                    break;
                case "1037983898":
                    Retorno = "9061745071";
                    break;
                case "1037984274":
                    Retorno = "9061745097";
                    break;
                case "1037983731":
                    Retorno = "9061745113";
                    break;
                case "1037983740":
                    Retorno = "9061745139";
                    break;
                case "1037984223":
                    Retorno = "9061745204";
                    break;
                case "1037983545":
                    Retorno = "9061745287";
                    break;
                case "1038707258":
                    Retorno = "9082432410";
                    break;
                case "1038707282":
                    Retorno = "9062169719";
                    break;
                case "1038707231":
                    Retorno = "9062169693";
                    break;
                case "1038707266":
                    Retorno = "9062169701";
                    break;
            }

            return Retorno;
        }
	}

	public class AmexParcelado : Amex
	{
		public string QuantidadeParcelas { get; set; }

        public static void GeraPlanilhaAmexParcelado(string strPath, string emailEnvio)
		{
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string arquivCopia = strPath + "Amex - Parcelado_" + data + ".xlsx";
            string nomeArquivo = "Amex - Parcelado_" + data + ".xlsx";

			ExcelPackage package = new ExcelPackage();
			ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

			ws.Cells["A1"].Value = "ESTABELECIMENTO";
			ws.Cells["B1"].Value = "NÚMERO DO CARTÂO";
			ws.Cells["C1"].Value = "DT VENDA";
			ws.Cells["D1"].Value = "NSU";
			ws.Cells["E1"].Value = "AUTORIZAÇÃO";
			ws.Cells["F1"].Value = "VLR VENDA";
			ws.Cells["G1"].Value = "VLR CANCELAR";
            ws.Cells["H1"].Value = "SENHA VENDA";
            ////////////////////////////////////////////////////////////////////////////////////////////////////
			//int linha = 2;

			List<AmexParcelado> lstAmexParcelado = AmexParcelado.getEstornosAmexParcelado();

            if (lstAmexParcelado.Count > 0)
            {
                int linha = 2;
                foreach (AmexParcelado item in lstAmexParcelado)
                {
                    if (item.Status == 'R')
                    {
                        ws.Cells[linha, 1].Value = item.Estabelecimento;
                        ws.Cells[linha, 2].Value = item.NumeroCartao;
                        ws.Cells[linha, 3].Value = item.DataVenda;
                        ws.Cells[linha, 4].Value = item.NSU;
                        ws.Cells[linha, 5].Value = item.Autorizacao;
                        ws.Cells[linha, 6].Value = item.ValorVenda;
                        ws.Cells[linha, 7].Value = item.ValorCancelar;
                        ws.Cells[linha, 8].Value = item.SenhaVenda;
                        linha++;
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                ws.Cells.AutoFitColumns();

                FileInfo fi = new FileInfo(arquivCopia);
                package.SaveAs(fi);

                BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (AmexParcelado item in lstAmexParcelado)
                {
                    if (item.Status == 'R')
                        item.setPlanilhaGerada();

                    else if (item.Status == 'W')
                        item.setItemWait();
                }

                bool RegGerados = lstAmexParcelado.ToList().Count(t => t.Status.Equals('R')) > 0;

                if (RegGerados)
                {
                    string[] lstEmails = emailEnvio.Split(';');

                    byte[] arrAnexo = br.ReadBytes((int)fi.Length);

                    foreach (string email in lstEmails)
                    {
                        IRLib.Emails.Retorno retorno = GetInstance().
                            EnviarEmailCancelamentoEstornoOperadora(email, "Estorno Planilhas", arrAnexo, nomeArquivo);
                    }

                    InsertHistorico("Amex - Parcelado", arquivCopia, nomeArquivo, emailEnvio);
                }
            }
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

		public static List<AmexParcelado> getEstornosAmexParcelado()
		{
			DateTime dt = System.DateTime.Now;
			BD bd = new BD();

			StringBuilder str = new StringBuilder();
			str.Append("SELECT est.ID, est.Cartao, b.Nome, vbfp.NSUHost, vbfp.NSUSitef, vbfp.NumeroAutorizacao, vb.DataVenda, ");
            str.Append("vb.Senha AS SenhaVenda, SUBSTRING(vbfp.CodigoIR, 2, 10) AS CodigoIR, c.CanalTipoID, ");
            str.Append("vbfp.Valor, fp.Parcelas, fp.Nome ,fp.Tipo, fpt.Nome, c.NroEstabelecimentoAmex, vb.id as VendaBilheteriaID ");
            str.Append("FROM EstornoDadosCartaoCredito est (NOLOCK) ");
            str.Append("inner join tVendaBilheteria vb (nolock) on vb.ID = est.VendaBilheteriaIDvenda ");
			str.Append("inner join tVendaBilheteriaFormaPagamento vbfp (nolock) on vbfp.VendaBilheteriaID = vb.ID ");
			str.Append("inner join tFormaPagamento fp (nolock) on fp.ID = vbfp.FormaPagamentoID ");
			str.Append("inner join tFormaPagamentoTipo fpt (nolock) on fp.FormaPagamentoTipoID = fpt.ID ");
			str.Append("inner join tBandeira b (nolock) on fp.BandeiraID = b.ID ");
            str.Append("inner join tCancelDevolucaoPendente cdp (NOLOCK) on cdp.VendaBilheteriaIDCancel = est.VendaBilheteriaIDcancel ");
            str.Append("inner join tCanal c (nolock) on c.ID = cdp.CanalID ");
            //str.Append("WHERE est.PlanilhaGerada IN (0,1) AND est.Status IN ('O', 'P', 'W') ");
            str.Append("WHERE est.PlanilhaGerada = 0  AND est.Status IN('P', 'W') ");
            str.Append("AND b.Nome IN ('American Express') AND fp.Parcelas > 1 ");
            //str.Append("AND (vbfp.NSUHost <> 0 OR vbfp.NSUSitef <> 0) ");

			List<AmexParcelado> lstAmexParcelado = new List<AmexParcelado>();

			bd.Consulta(str.ToString());

            VendaBilheteriaItem vbi = new VendaBilheteriaItem();

			while (bd.Consulta().Read())
			{
				AmexParcelado amexParc = new AmexParcelado();
				//amexParc.ID = bd.LerInt("ID");
                //amexParc.Estabelecimento = bd.LerString("NroEstabelecimentoAmex"); //"9061745055";//Valor default por enquanto

                string NSU = "";
                int CanalTipo = bd.LerInt("CanalTipoID");
                int VendaID = bd.LerInt("VendaBilheteriaID");

                //Amex amex = new Amex();

                if (CanalTipo == 1 || CanalTipo == 2)
                {
                    IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamento;

                    TipoPagamento = vbi.TipoPagamentoBilheteria(VendaID);

                    amexParc.ID = bd.LerInt("ID");

                    switch (TipoPagamento)
                    {
                        case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF:
                            amexParc.Estabelecimento = "9061745048";//Valor default para compras via TEF
                            break;
                        case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen:
                            amexParc.Estabelecimento = "9061745055";//Valor default para compras via Adyen
                            break;
                        default:
                            amexParc.Estabelecimento = "";//Valor default
                            break;
                    }
                }
                else
                    amexParc.Estabelecimento = GetCodigoIR(bd.LerString("CodigoIR")); //bd.LerString("NroEstabelecimentoAmex");//"1037983464";//Valor default por enquanto

                amexParc.NumeroCartao = bd.LerString("Cartao");
                amexParc.DataVenda = bd.LerStringFormatoData("DataVenda");
                //amex.NSU = bd.LerString("NSUSitef");

                NSU = bd.LerString("NSUSitef");// == "" ? getNSUConciliacao(VendaID) : bd.LerString("NSUHost");

                NSU = NSU == "" ? getNSUConciliacao(VendaID) : NSU;

                if (NSU == "" || NSU == "0")
                    amexParc.Status = 'W';//Wait, aguardar inserir manualmente NSU e Autorização
                else
                {
                    amexParc.NSU = NSU;
                    amexParc.Status = 'R';//Ready, pronta para processar
                }

				//amexParc.NumeroCartao = bd.LerString("Cartao");
				//amexParc.DataVenda = bd.LerStringFormatoData("DataVenda");
				//amexParc.NSU = bd.LerString("NSUSitef");
				amexParc.Autorizacao = bd.LerStringFormatoData("NumeroAutorizacao");
				amexParc.ValorVenda = bd.LerString("Valor"); ;
				amexParc.ValorCancelar = bd.LerString("Valor");
                amexParc.SenhaVenda = bd.LerString("SenhaVenda");

				lstAmexParcelado.Add(amexParc);
			}

			return lstAmexParcelado;
		}
	}
}
