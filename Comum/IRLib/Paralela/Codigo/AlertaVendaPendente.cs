using CTLib;
using System;
using System.Text;


namespace IRLib.Paralela
{
    public class AlertaVendaPendente
    {
        private BD bd { get; set; }
        public int quantidadeDias { get; set; }
        public int quantidadeCompras { get; set; }
        public string DataPesquisada { get; set; }

        public AlertaVendaPendente()
        {
            bd = new BD();
        }

        public void PesquisaComprasPendentes(int DiasAlerta)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT COUNT(ID)");
                stbSQL.Append(" FROM tVendaBilheteria (NOLOCK) ");
                stbSQL.Append(" WHERE (Status = 'A' OR Status = 'N') AND (VendaCancelada Is NULL OR VendaCancelada = 'F') ");
                stbSQL.Append(" AND DataVenda <= '" + DateTime.Now.AddDays(1).AddDays(-DiasAlerta).Date.ToString("yyyyMMdd") + "000000" + "'");

                quantidadeCompras = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Enviar(string EmailsAlertaPeanut)
        {
            try
            {
                if (quantidadeCompras > 0)
                {
                    EnviarEmailParalela enviarEmail = new EnviarEmailParalela();

                    enviarEmail.AlertaDeComprasComPrioridadeDeAtendimento(quantidadeCompras, quantidadeDias, EmailsAlertaPeanut);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
