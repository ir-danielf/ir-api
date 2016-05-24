using CTLib;
using IRLib.ClientObjects;
using System;

namespace IRLib.Codigo
{
    class EnviaSMS : MarshalByRefObject
    {
        APIHuman APIHuman = new APIHuman();

        public void EnviaSms(int VendaBilheteriaID)
        {
            try
            {
                if (APIHuman.Ativo)
                {
                    EstruturaEnvioSMS Sms = GetInfoByVendaBilheteriaID(VendaBilheteriaID);

                    this.EnviarSms(Sms);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EnviaSms(int VendaBilheteriaID, string numeroCelular)
        {
            try
            {
                if (APIHuman.Ativo)
                {
                    EstruturaEnvioSMS Sms = GetInfoByVendaBilheteriaID(VendaBilheteriaID);

                    Sms.Numero = numeroCelular;

                    this.EnviarSms(Sms);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EnviarSms(EstruturaEnvioSMS EstruturaEnvio)
        {
            try
            {
                APIHuman.CorpoEmail.Replace("##DataVenda##", EstruturaEnvio.DataVenda);
                APIHuman.CorpoEmail.Replace("##Senha##", EstruturaEnvio.Senha);
                APIHuman.CorpoEmail.Replace("##Valor##", "R$ " + EstruturaEnvio.ValorTotal);
                APIHuman.CorpoEmail.Replace("##Email##", EstruturaEnvio.Email);

                APIHuman.EnviarSms(EstruturaEnvio.Numero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EstruturaEnvioSMS GetInfoByVendaBilheteriaID(int VendaBilheteriaID)
        {
            try
            {
                BD bd = new BD();
                EstruturaEnvioSMS Sms = new EstruturaEnvioSMS();

                string sql = @"SELECT vb.Senha, vb.ValorTotal, vb.DataVenda, tc.Nome , tc.DDDCelular + tc.Celular AS Numero, tc.Email
                               FROM tVendaBilheteria AS vb
                               INNER JOIN tCliente AS tc ON tc.ID = vb.ClienteID 
                               WHERE vb.ID = " + VendaBilheteriaID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    Sms.DataVenda = bd.LerStringFormatoDataHora("DataVenda");
                    Sms.Numero = bd.LerString("Numero");
                    Sms.Senha = bd.LerString("Senha");
                    Sms.ValorTotal = bd.LerDecimal("ValorTotal");
                    Sms.Nome = bd.LerString("Nome");
                    Sms.Email = bd.LerString("Email");
                }

                return Sms;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
