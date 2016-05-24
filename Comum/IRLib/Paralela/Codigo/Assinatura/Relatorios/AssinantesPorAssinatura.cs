using CTLib;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class AssinantesPorAssinatura
    {

        BD bd;

        public AssinantesPorAssinatura()
        {
            bd = new BD();
        }

        public List<IRLib.Paralela.Assinaturas.Models.Relatorios.AssinanteAssinatura> BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas, bool Email)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var filtro = "";

                if (Assinaturas > 0)
                    filtro += " AND ass.ID = " + Assinaturas;


                filtro += " AND ( ac.Status <> 'D' AND ac.Acao <> 'D' ) ";

                if (Email)
                    filtro += " AND (c.Email IS NOT NULL AND LEN(c.Email) > 0) ";

                string sql = @"SELECT DISTINCT
                                    ass.Nome as Assinatura, l.Codigo as Lugar, s.Nome as Setor ,
                                    c.Nome as Assinante, c.LoginOSESP, c.Email,c.CPF ,
                                    fp.Nome AS FormaPagamento, pt.Nome AS PrecoTipo
                                FROM tAssinaturaCliente ac(NOLOCK)
                                INNER JOIN tVendaBilheteria vb (NOLOCK) ON ac.VendaBilheteriaID = vb.ID
                                INNER JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID
                                INNER JOIN tFormaPagamento fp (NOLOCK) ON fp.ID = vbfp.FormaPagamentoID
                                INNER JOIN tAssinaturaAno aa(NOLOCK) on aa.ID = ac.AssinaturaAnoID 
                                INNER JOIN tAssinatura ass(NOLOCK) on ass.ID = aa.AssinaturaID
								INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
                                INNER JOIN tCliente c(NOLOCK) on ac.ClienteID = c.ID
                                INNER JOIN tSetor s(NOLOCK) on ac.SetorID = s.ID
                                INNER JOIN tPrecoTipo pt (NOLOCK) ON ac.PrecoTipoID = pt.ID
                                WHERE aa.Ano =  '" + Temporadas + "' AND ass.AssinaturaTipoID = " + AssinaturaTipoID + filtro + " AND fp.ID <> 147  ORDER BY c.Nome, ass.Nome, s.Nome, l.Codigo";

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
                        CPF = bd.LerString("CPF"),
                        Email = bd.LerString("Email"),
                        FormaPagamento = bd.LerString("FormaPagamento"),
                        Preco = bd.LerString("PrecoTipo"),
                    });

                return lstRetorno;
            }
            finally
            {
                bd.Fechar();

            }
        }


    }
}
