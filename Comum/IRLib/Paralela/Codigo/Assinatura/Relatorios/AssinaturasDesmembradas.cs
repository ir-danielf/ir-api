using CTLib;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class AssinaturasDesmembradas
    {

        BD bd;

        public AssinaturasDesmembradas()
        {
            bd = new BD();
        }

        public List<IRLib.Paralela.Assinaturas.Models.AssinaturasDesmembradas> BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var filtro = "";

                if (Assinaturas > 0)
                {
                    filtro = " AND a.ID = " + Assinaturas;
                }

                string sql = @"SELECT ad.Motivo,l.Codigo as Lugar, s.Nome as Setor,a.Nome as Assinatura,
                        aa.Ano as Temporada,u.Nome as Usuario, c.Nome as ClienteAtual, cd.Nome as ClienteAntigo,
                        ac.Desmembrada,ad.TimeStamp,ad.Motivo 
                        FROM tAssinaturaCliente ac (NOLOCK) 
                        INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
                        INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
                        INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
                        INNER JOIN tAssinaturaAno aa (NOLOCK) ON aa.ID = ac.AssinaturaAnoID
                        INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
                        LEFT JOIN tAssinaturaDesmembramento ad (NOLOCK) ON ac.ID = ad.AssinaturaClienteID
                        LEFT JOIN tUsuario u (NOLOCK) ON u.ID = ad.UsuarioID
                        LEFT JOIN tCliente cd (NOLOCK) ON cd.ID = ad.AntigoClienteID
                        Where ac.Desmembrada = 'T' AND aa.Ano =  '" + Temporadas + "' AND a.AssinaturaTipoID = " + AssinaturaTipoID + filtro + "  Order by c.Nome ";

                bd.Consulta(sql);

                var lstRetorno = new List<Models.AssinaturasDesmembradas>();

                Models.AssinaturasDesmembradas eABaux = new Models.AssinaturasDesmembradas();

                while (bd.Consulta().Read())
                {
                    eABaux = new Models.AssinaturasDesmembradas()
                         {
                             Antigo = bd.LerString("ClienteAntigo"),
                             Atual = bd.LerString("ClienteAtual"),
                             Data = bd.LerDateTime("TimeStamp"),
                             Assento = bd.LerString("Lugar"),
                             Motivo = bd.LerString("Motivo"),
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
