using CTLib;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.EstornoDadosPlanilhas.Operadoras
{
    public class PlanilhasEstornosManuais
    {
        public int ID { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string SenhaVenda { get; set; }
        public string Cartao { get; set; }
        public string Bandeira { get; set; }
        public string NumeroAutorizacao { get; set; }
        public string DataVenda { get; set; }
        public string ValorVenda { get; set; }
        public string ValorEstorno { get; set; }
        public string Parcelas { get; set; }
        public string FormaPagamento { get; set; }
        public string Tipo { get; set; }
        public string TipoFormaPagamento { get; set; }
        public int VendaBilheteriaID { get; set; }

        public static List<PlanilhasEstornosManuais> lstEstornoManuais { get; set; }

        private static IRLib.Emails.MailServiceSoapClient _oService_Emails;
        public static IRLib.Emails.MailServiceSoapClient GetInstance()
        {
            if (_oService_Emails == null)
                _oService_Emails = new IRLib.Emails.MailServiceSoapClient();

            return _oService_Emails;
        }

        public static void GeraPlanilhaEstornosManuais(string strPath, string emailEnvio)
        {
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string data = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string caminho = strPath + "EstornosManuais_" + data + ".xlsx";
            string nomeArquivo = "EstornosManuais_" + data + ".xlsx";

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelamento");

            ws.Cells["A1"].Value = "NomeCliente";
            ws.Cells["B1"].Value = "EmailCliente";
            ws.Cells["C1"].Value = "SenhaVenda";
            ws.Cells["D1"].Value = "Cartao";
            ws.Cells["E1"].Value = "Bandeira";
            ws.Cells["F1"].Value = "NumeroAutorizacao";
            ws.Cells["G1"].Value = "DataVenda";
            ws.Cells["H1"].Value = "ValorVenda";
            ws.Cells["I1"].Value = "ValorEstorno";
            ws.Cells["J1"].Value = "Parcelas";
            ws.Cells["K1"].Value = "FormaPagamento";
            ws.Cells["L1"].Value = "TipoFormaPagamento";
           // ws.Cells["M1"].Value = "VendaBilheteriaID";

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //int linha = 2;

            List<PlanilhasEstornosManuais> lstEstornoManuais = new List<PlanilhasEstornosManuais>();

            lstEstornoManuais = PlanilhasEstornosManuais.getEstornosManuais();

            if (lstEstornoManuais.Count > 0)
            {
                //int linha = 2;

                //foreach (PlanilhasEstornosManuais item in lstEstornoManuais)
                //{
                //    ws.Cells[linha, 1].Value = item.NomeCliente;
                //    ws.Cells[linha, 2].Value = item.EmailCliente;
                //    ws.Cells[linha, 3].Value = item.SenhaVenda;
                //    ws.Cells[linha, 4].Value = item.Cartao;
                //    ws.Cells[linha, 5].Value = item.Bandeira;
                //    ws.Cells[linha, 6].Value = item.NumeroAutorizacao;
                //    ws.Cells[linha, 7].Value = item.DataVenda;
                //    ws.Cells[linha, 8].Value = item.ValorVenda;
                //    ws.Cells[linha, 9].Value = item.ValorEstorno;
                //    ws.Cells[linha, 10].Value = item.Parcelas;
                //    ws.Cells[linha, 11].Value = item.FormaPagamento;
                //    ws.Cells[linha, 12].Value = item.TipoFormaPagamento;
                //    //ws.Cells[linha, 13].Value = item.VendaBilheteriaID;

                //    linha++;
                //}

                //ws.Cells.AutoFitColumns();

                //FileInfo fi = new FileInfo(caminho);
                //package.SaveAs(fi);

                //BinaryReader br = new BinaryReader(fi.OpenRead());

                foreach (PlanilhasEstornosManuais item in lstEstornoManuais)
                {
                    //item.setPlanilhaGerada();
                    item.setRegistrosAguardando();
                }

                //string[] lstEmails = emailEnvio.Split(';');

                //byte[] arrAnexo = br.ReadBytes((int)fi.Length);

                //foreach (string email in lstEmails)
                //{
                //    IRLib.Emails.Retorno retorno = GetInstance().
                //        EnviarEmailCancelamentoEstornoOperadora(email, "Estorno Planilhas", arrAnexo, nomeArquivo);
                //}

                //InsertHistorico("PlanilhasEstornosManuais", caminho, nomeArquivo, emailEnvio);
            }
        }

        static bool InsertHistorico(string bandeira, string caminho, string nomearquivo, string email)
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

        protected bool setPlanilhaGerada()
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

        protected bool setRegistrosAguardando()
        {
            BD bd = new BD();
            StringBuilder str = new StringBuilder();
            str.Append("UPDATE EstornoDadosCartaoCredito ");
            str.Append("SET PlanilhaGerada = 0, Status = 'W' ");
            str.Append("WHERE ID = " + this.ID);

            if (bd.Executar(str) > 0)
                return true;
            else
                return false;
        }
        private static List<PlanilhasEstornosManuais> getEstornosManuais()
        {
            DateTime dt = System.DateTime.Now;
            BD bd = new BD();
            StringBuilder str = new StringBuilder();
            str.Append("SELECT ");
            str.Append("est.ID, c.Nome AS NomeCliente, c.email AS EmailCliente, vb.Senha AS SenhaVenda, ");
            str.Append("est.Cartao, b.Nome AS Bandeira,  ");
            str.Append("vbfp.NumeroAutorizacao, vb.DataVenda,  ");
            str.Append("vb.ValorTotal AS ValorVenda, est.Valor AS ValorEstorno, ");
            str.Append("fp.Parcelas, fp.Nome AS FormaPagamento,  ");
            str.Append("fp.Tipo, fpt.Nome AS TipoFormaPagamento, vb.id AS VendaBilheteriaID ");
            str.Append("FROM EstornoDadosCartaoCredito est  ");
            str.Append("INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = est.VendaBilheteriaIDvenda ");
            str.Append("INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID ");
            str.Append("INNER JOIN tFormaPagamento fp (NOLOCK) ON fp.ID = vbfp.FormaPagamentoID ");
            str.Append("INNER JOIN tFormaPagamentoTipo fpt (NOLOCK) ON fp.FormaPagamentoTipoID = fpt.ID ");
            str.Append("INNER JOIN tBandeira b (NOLOCK) ON fp.BandeiraID = b.ID ");
            str.Append("INNER JOIN tcliente c (NOLOCK) ON vb.ClienteID = c.ID ");
            str.Append("WHERE est.PlanilhaGerada IN(0)  AND est.Status = 'P' ");
            str.Append("AND (vbfp.NSUHost = 0 OR vbfp.NSUSitef = 0) AND b.Nome NOT IN('PayPal', 'Visa', 'Vale Cultura', 'Elo Cultura') ");

            List<PlanilhasEstornosManuais> lstEstornoManuais = new List<PlanilhasEstornosManuais>();

            bd.Consulta(str.ToString());

            while (bd.Consulta().Read())
            {
                PlanilhasEstornosManuais estornosManuais = new PlanilhasEstornosManuais();
                estornosManuais.ID = bd.LerInt("ID");
                estornosManuais.NomeCliente = bd.LerString("NomeCliente");
                estornosManuais.EmailCliente = bd.LerString("EmailCliente");
                estornosManuais.SenhaVenda = bd.LerString("SenhaVenda");
                estornosManuais.Cartao = bd.LerString("Cartao");
                estornosManuais.Bandeira = bd.LerString("Bandeira"); ;
                estornosManuais.NumeroAutorizacao = bd.LerString("NumeroAutorizacao");
                estornosManuais.DataVenda = bd.LerStringFormatoData("DataVenda");
                estornosManuais.ValorVenda = bd.LerString("ValorVenda");
                estornosManuais.ValorEstorno = bd.LerString("ValorEstorno");
                estornosManuais.Parcelas = bd.LerString("Parcelas");
                estornosManuais.FormaPagamento = bd.LerString("FormaPagamento");
                estornosManuais.TipoFormaPagamento = bd.LerString("TipoFormaPagamento");
                //estornosManuais.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");
                lstEstornoManuais.Add(estornosManuais);
            }

            return lstEstornoManuais;
        }       
    }
}
