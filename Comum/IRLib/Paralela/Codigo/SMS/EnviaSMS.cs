using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{
    public class EnviaSMS : MarshalByRefObject
    {
        APIHuman APIHuman = new APIHuman();

        public EnviaSMS() { }

        public void EnviaSms(bool sync, int VendaBilheteriaID)
        {
            try
            {
                if (APIHuman.Ativo)
                {
                    EstruturaEnvioSMS Sms = GetInfoByVendaBilheteriaID(VendaBilheteriaID);

                    if (string.IsNullOrEmpty(Sms.Numero))
                        return;

                    this.EnviarSms(sync, Sms);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro: " + e.Message);
            }
        }

        public void EnviaSms(bool sync, int VendaBilheteriaID, string numeroCelular)
        {
            try
            {
                if (APIHuman.Ativo)
                {
                    EstruturaEnvioSMS Sms = GetInfoByVendaBilheteriaID(VendaBilheteriaID);

                    if (numeroCelular != "")
                        Sms.Numero = numeroCelular;

                    this.EnviarSms(sync, Sms);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro: " + e.Message);
            }
        }


        public void EnviarSMS(HammerHead.EstruturaVenda venda)
        {
            if (!APIHuman.Ativo)
                return;

            try
            {
                if (string.IsNullOrEmpty(venda.NumeroCelular))
                    return;

                EstruturaEnvioSMS sms = new EstruturaEnvioSMS()
                {
                    Senha = venda.Senha,
                    DataVenda = venda.DataVenda.ToShortDateString(),
                    Email = venda.Cliente.Email,
                    Nome = venda.Cliente.Nome,
                    Numero = venda.NumeroCelular,
                    ValorTotal = venda.ValorTotal,
                };

                this.EnviarSms(true, sms);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message);
            }

        }

        public void EnviarSms(bool sync, EstruturaEnvioSMS EstruturaEnvio)
        {
            try
            {
                APIHuman.CorpoEmail = APIHuman.CorpoEmail.Replace("##DataVenda##", EstruturaEnvio.DataVenda);
                APIHuman.CorpoEmail = APIHuman.CorpoEmail.Replace("##Senha##", EstruturaEnvio.Senha);
                APIHuman.CorpoEmail = APIHuman.CorpoEmail.Replace("##Valor##", "R$ " + EstruturaEnvio.ValorTotal);
                APIHuman.CorpoEmail = APIHuman.CorpoEmail.Replace("##Email##", EstruturaEnvio.Email);

                if (EstruturaEnvio.Numero != null)
                    if (sync)
                        APIHuman.EnviarSmsSync(EstruturaEnvio.Numero);
                    else
                        APIHuman.EnviarSms(EstruturaEnvio.Numero);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public EstruturaEnvioSMS GetInfoByVendaBilheteriaID(int VendaBilheteriaID)
        {
            BD bd = new BD();
            try
            {

                EstruturaEnvioSMS Sms = new EstruturaEnvioSMS();

                string sql = @"SELECT vb.Senha, vb.ValorTotal, vb.DataVenda, tc.Nome , ISNULL(vb.DDD, 0) as DDD , 
                               ISNULL(vb.NumeroCelular,0) AS Numero, tc.Email
                               FROM tVendaBilheteria AS vb (NOLOCK)
                               INNER JOIN tCliente AS tc (NOLOCK) ON tc.ID = vb.ClienteID 
                               WHERE vb.ID = " + VendaBilheteriaID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DateTime data = bd.LerDateTime("DataVenda");
                    Sms.DataVenda = data.ToString("d/M/yy");
                    string ddd = Convert.ToString(bd.LerInt("DDD"));
                    string celular = Convert.ToString(bd.LerInt("Numero"));
                    if (ddd != "0" && celular != "0")
                        Sms.Numero = ddd + celular;
                    Sms.Senha = bd.LerString("Senha");
                    Sms.ValorTotal = bd.LerDecimal("ValorTotal");
                    Sms.Nome = bd.LerString("Nome");
                    Sms.Email = bd.LerString("Email");
                }

                return Sms;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void EnviarSMS_AlertaCodigoBarra(List<string> Numeros, EstruturaQuantidadeCodigosListaBranca item, bool criados)
        {
            APIHuman.CorpoEmail
                = string.Format
                (@"Bar Codes - {0} - {1} - {2} - Criados: {3}",
                item.Evento, item.Horario.ToString("HH:mm d/M/yy"), item.Setor, criados ? "Sim" : "Não");

            foreach (var numero in Numeros)
                APIHuman.EnviarSms(numero);
        }

    }
}
