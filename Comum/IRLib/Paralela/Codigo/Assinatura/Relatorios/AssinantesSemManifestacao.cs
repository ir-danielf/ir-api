using CTLib;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class AssinantesSemManifestacao
    {

        BD bd;

        public AssinantesSemManifestacao()
        {
            bd = new BD();
        }

        public List<IRLib.Paralela.Assinaturas.Models.AssinantesSemManifestacao> BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas, AssinaturaCliente.EnumStatus filtroStatus)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var filtro = "";

                if (Assinaturas > 0)
                {
                    filtro = " AND ass.ID = " + Assinaturas;
                }

                switch (filtroStatus)
                {
                    case AssinaturaCliente.EnumStatus.Aguardando:
                        filtro += " AND ac.Status = '" + IRLib.Paralela.Utils.Enums.GetChar(filtroStatus) + "' AND ac.Acao = '" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.AguardandoAcao) + "' ";
                        break;
                    case AssinaturaCliente.EnumStatus.TrocaSinalizada:
                        filtro += " AND ac.Status = '" + IRLib.Paralela.Utils.Enums.GetChar(filtroStatus) + "' AND ac.Acao = '" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.Trocar) + "' ";
                        break;
                    default:
                        filtro += " AND ac.Status IN ('" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumStatus.Aguardando) + "', '" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumStatus.TrocaSinalizada) + "') AND ac.Acao IN ('" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.Trocar) + "', '" + IRLib.Paralela.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.AguardandoAcao) + "') ";
                        break;
                }

                string sql = @"SELECT DISTINCT
                                ass.Nome as Assinatura, l.Codigo as Lugar, s.Nome as Setor ,
                                ISNULL(ac.Status,'') as StatusAssinatura , 
                                c.Nome as Assinante,c.LoginOSESP
                                FROM tAssinaturaCliente ac(NOLOCK)
                                INNER JOIN tAssinatura ass(NOLOCK) on ass.ID = ac.AssinaturaID
                                INNER JOIN tAssinaturaAno aa(NOLOCK) on aa.ID = ac.AssinaturaAnoID
                                INNER JOIN tCliente c(NOLOCK) on ac.ClienteID = c.ID
                                INNER JOIN tSetor s(NOLOCK) on ac.SetorID = s.ID
                                INNER JOIN tLugar l(NOLOCK) on ac.LugarID = l.ID    
                                WHERE aa.Ano =  '" + Temporadas + "' AND ass.AssinaturaTipoID = " + AssinaturaTipoID + filtro + "  Order by c.Nome ";

                bd.Consulta(sql);

                var lstRetorno = new List<Models.AssinantesSemManifestacao>();

                Models.AssinantesSemManifestacao eABaux = new Models.AssinantesSemManifestacao();

                while (bd.Consulta().Read())
                {
                    var status = Utils.Enums.ParseItem<AssinaturaCliente.EnumStatus>(bd.LerString("StatusAssinatura"));

                    eABaux = new Models.AssinantesSemManifestacao()
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Assinatura = bd.LerString("Assinatura"),
                        Cliente = bd.LerString("Assinante"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Lugar"),
                        Status = status == AssinaturaCliente.EnumStatus.Aguardando ? Assinatura.EnumStatusVisual.AguardandoAcao : Assinatura.EnumStatusVisual.TrocaSinalizada
                    };
                    lstRetorno.Add(eABaux);
                }

                return lstRetorno;

            }
            finally
            {
                bd.Fechar();
            }
        }


    }
}
