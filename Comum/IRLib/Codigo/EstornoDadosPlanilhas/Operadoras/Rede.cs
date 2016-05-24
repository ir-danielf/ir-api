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
	public class Rede
	{
		public int ID { get; set; }
		public string CodEstabelecimento { get; set; }
        public string TipoVenda { get; set; }
        public string DataTransacao { get; set; }
		public string NumeroCartao { get; set; }
        public string NumeroComprovanteNSU { get; set; }
		//public string Autorizacao { get; set; }		
		public string ValorTotalVenda { get; set; }
        public string SaldoDisponivel { get; set; }
        public string TipoDeCancelamento { get; set; }
		public string ValorAEstornar { get; set; }
        public string SenhaVenda { get; set; }
        public char Status { get; set; }
        //public string ValorJaEstornado { get; set; }
        //public string ValorAtivo { get; set; }
        //public string ParceladoOUaVista { get; set; }
        //public string NumeroParcelas { get; set; }
        //public string OBSHipercard { get; set; }

        private static IRLib.Emails.MailServiceSoapClient _oService_Emails;
        public static IRLib.Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new IRLib.Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        public static void GeraPlanilhaRede(string strPath, string emailEnvio)
		{
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string arquivCopia = strPath + "Rede_" + data + ".xlsx";
            string nomeArquivo = "Rede_" + data + ".xlsx";

    		ExcelPackage package = new ExcelPackage();
			ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

			ws.Cells["A1"].Value = "Código Estabelecimento";
            ws.Cells["B1"].Value = "Tipo de Venda";
            ws.Cells["C1"].Value = "Nº do Cartão";
            ws.Cells["D1"].Value = "Comprovante NSU";
			ws.Cells["E1"].Value = "Data da  Transação";
			ws.Cells["F1"].Value = "Valor Total da Venda";
            ws.Cells["G1"].Value = "Saldo Disponivel";
            ws.Cells["H1"].Value = "Tipo Cancelamento";
			ws.Cells["I1"].Value = "Valor a Estornar";
            ws.Cells["J1"].Value = "Senha Venda";
            //ws.Cells["H1"].Value = "Valor já Estornado";
            //ws.Cells["I1"].Value = "Valor Ativo";
            //ws.Cells["J1"].Value = "Parcelado / À Vista";
            //ws.Cells["K1"].Value = "Nº de Parcelas";
            //ws.Cells["L1"].Value = "OBS HIPERCARD";
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//int linha = 2;

			List<Rede> lstRede = new List<Rede>();

			lstRede = Rede.getEstornosRede();

            if (lstRede.Count > 0)
            {
                int linha = 2;
                foreach (Rede item in lstRede)
                {
                    if (item.Status == 'R')
                    {
                        ws.Cells[linha, 1].Value = item.CodEstabelecimento;
                        ws.Cells[linha, 2].Value = item.TipoVenda;
                        ws.Cells[linha, 3].Value = item.NumeroCartao;
                        ws.Cells[linha, 4].Value = item.NumeroComprovanteNSU;
                        ws.Cells[linha, 5].Value = item.DataTransacao;
                        ws.Cells[linha, 6].Value = item.ValorTotalVenda;
                        ws.Cells[linha, 7].Value = item.SaldoDisponivel;
                        ws.Cells[linha, 8].Value = item.TipoDeCancelamento;
                        ws.Cells[linha, 9].Value = item.ValorAEstornar;
                        ws.Cells[linha, 10].Value = item.SenhaVenda;

                        linha++;
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                ws.Cells.AutoFitColumns();

                FileInfo fi = new FileInfo(arquivCopia);
                package.SaveAs(fi);

                BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (Rede item in lstRede)
                {
                    if (item.Status == 'R')
                        item.setPlanilhaGerada();

                    else if (item.Status == 'W')
                        item.setItemWait();
                }

                bool registrosNovos = lstRede.ToList().Count(t => t.Status.Equals('R')) > 0;

                if (registrosNovos)
                {
                    string[] lstEmails = emailEnvio.Split(';');

                    byte[] arrAnexo = br.ReadBytes((int)fi.Length);

                    foreach (string email in lstEmails)
                    {
                        IRLib.Emails.Retorno retorno = GetInstance().
                            EnviarEmailCancelamentoEstornoOperadora(email, "Estorno Planilhas", arrAnexo, nomeArquivo);
                    }

                    InsertHistorico("Mastercard", arquivCopia, nomeArquivo, emailEnvio);
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

		public static List<Rede> getEstornosRede()
		{
			DateTime dt = System.DateTime.Now;
			BD bd = new BD();
			StringBuilder str = new StringBuilder();
			str.Append("SELECT est.ID, est.Cartao, b.Nome, vbfp.NSUHost, vbfp.NSUSitef, ");
            str.Append("vbfp.NumeroAutorizacao, vb.DataVenda, vb.ValorTotal as ValorVenda, ");
            str.Append("vb.Senha AS SenhaVenda, c.CanalTipoID, ");
            str.Append("est.Valor as ValorEstorno, fp.Parcelas, fp.Nome ,fp.Tipo, fpt.Nome, ");
            str.Append("case fpt.Parcelas when 1 then 'Á VISTA' else 'PARCELADO' END AS AVistaParcelado, ");
            str.Append("case fpt.Parcelas when 1 then '' else fpt.Parcelas END AS NrParcelas, ");
            str.Append("CASE (vb.ValorTotal - est.Valor) ");
            str.Append("WHEN 0 THEN 'Total' ELSE 'Parcial' END AS TipoDeCancelamento, vb.id as VendaBilheteriaID ");
            str.Append("FROM EstornoDadosCartaoCredito est (NOLOCK) ");
            str.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) on vb.ID = est.VendaBilheteriaIDvenda ");
            str.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) on vbfp.VendaBilheteriaID = vb.ID ");
            str.Append("INNER JOIN tFormaPagamento fp (NOLOCK) on fp.ID = vbfp.FormaPagamentoID ");
			str.Append("INNER JOIN tFormaPagamentoTipo fpt (nolock) on fp.FormaPagamentoTipoID = fpt.ID ");
            str.Append("INNER JOIN tBandeira b (NOLOCK) on fp.BandeiraID = b.ID ");
            str.Append("INNER JOIN tCaixa cx (NOLOCK) on cx.id = vb.caixaid ");
            str.Append("INNER JOIN tloja l (NOLOCK) on l.id = cx.lojaid ");
            str.Append("INNER JOIN tCanal c (NOLOCK) on c.id = l.canalid ");
            str.Append("INNER JOIN tCanalTipo ct (NOLOCK) ON c.CanalTipoID = ct.ID ");
			str.Append("WHERE est.PlanilhaGerada = 0 ");
            str.Append("AND b.Nome IN ('Mastercard','Hipercard')  AND est.Status IN('P', 'W') ");
            //str.Append("AND (vbfp.NSUHost <> 0 OR vbfp.NSUSitef <> 0) ");

			List<Rede> lstRede = new List<Rede>();

			bd.Consulta(str.ToString());

            VendaBilheteriaItem vbi = new VendaBilheteriaItem();

            while (bd.Consulta().Read())
            {
                string NSUHost = "";

                int VendaID = bd.LerInt("VendaBilheteriaID");
                int CanalTipo = bd.LerInt("CanalTipoID");

                IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento TipoPagamento;

                TipoPagamento = vbi.TipoPagamentoBilheteria(VendaID);

                Rede rede = new Rede();
                rede.ID = bd.LerInt("ID");
                switch (TipoPagamento)
                {
                    case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.TEF:
                        rede.CodEstabelecimento = "40559637";//Valor default para compras via TEF
                        break;
                    case IRLib.ClientObjects.EstruturaPagamento.enumTipoPagamento.Adyen:
                        rede.CodEstabelecimento = "40762882";//Valor default para compras via Adyen
                        break;
                    default:
                        rede.CodEstabelecimento = "";//Valor default
                        break;
                }

                rede.TipoVenda = "Crédito";
                rede.DataTransacao = bd.LerStringFormatoData("DataVenda");
                rede.NumeroCartao = bd.LerString("Cartao");
                
                NSUHost = bd.LerString("NSUHost");// == "" ? getNSUConciliacao(VendaID) : bd.LerString("NSUHost");

                NSUHost = NSUHost == "" ? getNSUConciliacao(VendaID) : NSUHost;

                if (NSUHost == "" || NSUHost == "0")
                    rede.Status = 'W';//Wait, aguardar inserir manualmente NSU e Autorização
                else
                {
                    rede.NumeroComprovanteNSU = NSUHost;
                    rede.Status = 'R';//Ready, pronta para processar
                }
                
                //rede.NumeroComprovanteNSU = bd.LerString("NSUHost");
                //rede.NumeroComprovanteNSU = bd.LerString("NSUSitef");
                //rede.Autorizacao = bd.LerString("NumeroAutorizacao");
                rede.ValorTotalVenda = bd.LerString("ValorVenda"); ;
                rede.SaldoDisponivel = "";//Sempre em branco, usado apenas pela operadora
                rede.TipoDeCancelamento = bd.LerString("TipoDeCancelamento");
                rede.ValorAEstornar = bd.LerString("ValorEstorno"); ;
                //rede.ValorJaEstornado = "R$ 0,00";
                //rede.ValorAtivo = "R$ 0,00";
                //rede.ParceladoOUaVista = bd.LerString("AVistaParcelado"); ;
                //rede.NumeroParcelas = bd.LerString("NrParcelas");
                //rede.OBSHipercard = "";
                rede.SenhaVenda = bd.LerString("SenhaVenda");
                lstRede.Add(rede);
            }

			return lstRede;
		}

        private static string getNSUConciliacao(int idVenda)
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