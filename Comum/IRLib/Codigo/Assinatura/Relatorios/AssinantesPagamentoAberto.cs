using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Assinaturas.Relatorios
{
    public class AssinantesPagamentoAberto
    {

        BD bd;

        public AssinantesPagamentoAberto()
        {
            bd = new BD();
        }

        public List<IRLib.Assinaturas.Models.Relatorios.AssinanteAssinatura> BuscarRelatorio(int AssinaturaTipoID, int Temporadas)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();


                string sql = @"SELECT DISTINCT  aa.Ano as Temporada,
                                ass.Nome as Assinatura,  l.Codigo as Lugar, s.Nome as Setor ,
                                c.Nome as Assinante, c.CPF,c.LoginOSESP,c.Email,c.CPF
                                FROM tAssinaturaCliente ac(NOLOCK)
                                INNER JOIN tAssinatura ass(NOLOCK) on ass.ID = ac.AssinaturaID
                                INNER JOIN tAssinaturaAno aa(NOLOCK) on aa.ID = ac.AssinaturaAnoID
                                INNER JOIN tCliente c(NOLOCK) on ac.ClienteID = c.ID
                                INNER JOIN tSetor s(NOLOCK) on ac.SetorID = s.ID
                                INNER JOIN tLugar l (NOLOCK) on l.ID = ac.LugarID
                                WHERE ac.Status = 'S' AND ac.Acao='R' AND  aa.Ano =  '" + Temporadas + "' AND ass.AssinaturaTipoID = " + AssinaturaTipoID + "  Order by c.Nome ";

                bd.Consulta(sql);

                var lstRetorno = new List<Models.Relatorios.AssinanteAssinatura>();

                Models.Relatorios.AssinanteAssinatura eABaux = new Models.Relatorios.AssinanteAssinatura();

                while (bd.Consulta().Read())
                    lstRetorno.Add(new Models.Relatorios.AssinanteAssinatura()
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Assinatura = bd.LerString("Assinatura"),
                        Nome = bd.LerString("Assinante"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Lugar"),
                        Email = bd.LerString("Email"),
                        CPF = bd.LerString("CPF"),
                    });


                bd.FecharConsulta();

                sql = @"SELECT DISTINCT 
                    aa.Ano as Temporada,
                    ass.Nome as Assinatura,  l.Codigo as Lugar, s.Nome as Setor ,
                    c.Nome as Assinante, c.CPF,c.LoginOSESP,c.Email,c.CPF
                    FROM tAssinaturaCliente ac(NOLOCK)
                    INNER JOIN tAssinatura ass(NOLOCK) on ass.ID = ac.AssinaturaID
                    INNER JOIN tAssinaturaAno aa(NOLOCK) on aa.ID = ac.AssinaturaAnoID
                    INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = ac.VendaBilheteriaID
                    INNER JOIN tVendaBilheteriaFormaPagamento fp (NOLOCK) on vb.ID = fp.VendaBilheteriaID
                    INNER JOIN tVendaBilheteriaFormaPagamentoBoleto fpb (NOLOCK) on fp.ID = fpb.VendaBilheteriaFormaPagamentoID
                    INNER JOIN tCliente c(NOLOCK) on ac.ClienteID = c.ID
                    INNER JOIN tSetor s(NOLOCK) on ac.SetorID = s.ID
                    INNER JOIN tLugar l (NOLOCK) on l.ID = ac.LugarID
                    WHERE ac.Acao IN('R', 'E', 'N') AND aa.Ano =  '" + Temporadas + "' AND ass.AssinaturaTipoID = " + AssinaturaTipoID + "  AND fpb.DataPagamento = ''";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lstRetorno.Add(new Models.Relatorios.AssinanteAssinatura()
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Assinatura = bd.LerString("Assinatura"),
                        Nome = bd.LerString("Assinante"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Lugar"),
                        Email = bd.LerString("Email"),
                        CPF = bd.LerString("CPF"),
                        BoletoAberto = true
                    });

                return lstRetorno.OrderBy(c => c.Nome).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();

            }
        }


    }
}
