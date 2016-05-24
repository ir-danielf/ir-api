using CTLib;
using System;
using System.Linq;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class TotaisPorProcessoAssinatura
    {

        BD bd;

        public TotaisPorProcessoAssinatura()
        {
            bd = new BD();
        }

        public Models.TotaisPorProcessoAssinatura BuscarRelatorio(int assinaturaTipoID, string ano)
        {
            try
            {
                Models.TotaisPorProcessoAssinatura oTotais = new Models.TotaisPorProcessoAssinatura();
                string sqlBusca =
                   string.Format(
                       @"SELECT 
	                        ac.ID as AssinaturaClienteID, 
	                        ac.Status, 
	                        ac.TimeStamp,
	                        ac.UsuarioID
	                        FROM tAssinaturaCliente  ac(NOLOCK)
                        INNER JOIN tAssinatura ass(NOLOCK) ON ass.ID = ac.AssinaturaID
                        INNER JOIN tAssinaturaAno aa (NOLOCK) ON ass.ID = aa.AssinaturaID
                        where ass.AssinaturaTipoID = {0} AND aa.Ano = '{1}'
                    ", assinaturaTipoID, ano);

                bd.Consulta(sqlBusca);
                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");

                var assinaturaAnonimo = new
                {
                    AssinaturaCliente = 0,
                    Status = string.Empty,
                    UsuarioID = 0,
                    TimeStamp = DateTime.MinValue,
                };

                var listaAssinatura = VendaBilheteria.ToAnonymousList(assinaturaAnonimo);

                do
                {
                    var assinaturaAnonimoInsert = (new
                    {
                        AssinaturaCliente = bd.LerInt("AssinaturaClienteID"),
                        Status = bd.LerString("Status"),
                        UsuarioID = bd.LerInt("UsuarioID"),
                        TimeStamp = bd.LerDateTime("TimeStamp"),

                    });

                    listaAssinatura.Add(assinaturaAnonimoInsert);
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                AssinaturaTipo oAssinaturaTipo = new AssinaturaTipo();
                oAssinaturaTipo.Ler(assinaturaTipoID);

                oTotais.InternetRenovacaoNaRenovacao = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp <= oAssinaturaTipo.RenovacaoFim.Valor && c.UsuarioID == Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.InternetRenovacaoNaTroca = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp > oAssinaturaTipo.RenovacaoFim.Valor && c.TimeStamp <= oAssinaturaTipo.NovaAquisicaoInicio.Valor && c.UsuarioID == Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.InternetRenovacaoNaAquisicao = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp > oAssinaturaTipo.NovaAquisicaoInicio.Valor && c.UsuarioID == Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.InternetTroca = listaAssinatura.Where(c => Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Troca) && c.UsuarioID == Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.InternetAquisicao = listaAssinatura.Where(c => Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Aquisicao) && c.UsuarioID == Usuario.INTERNET_USUARIO_ID).Count();

                oTotais.OperadorRenovacaoNaRenovacao = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp <= oAssinaturaTipo.RenovacaoFim.Valor && c.UsuarioID != Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.OperadorRenovacaoNaTroca = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp > oAssinaturaTipo.RenovacaoFim.Valor && c.TimeStamp <= oAssinaturaTipo.NovaAquisicaoInicio.Valor && c.UsuarioID != Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.OperadorRenovacaoNaAquisicao = listaAssinatura.Where(c => (Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Renovado) || Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.RenovadoSemPagamento)) && c.TimeStamp > oAssinaturaTipo.NovaAquisicaoInicio.Valor && c.UsuarioID != Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.OperadorTroca = listaAssinatura.Where(c => Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Troca) && c.UsuarioID != Usuario.INTERNET_USUARIO_ID).Count();
                oTotais.OperadorAquisicao = listaAssinatura.Where(c => Convert.ToChar(c.Status) == Convert.ToChar(AssinaturaCliente.EnumStatus.Aquisicao) && c.UsuarioID != Usuario.INTERNET_USUARIO_ID).Count();
                
                return oTotais;
            }
            finally
            {
                bd.Fechar();
            }

        }
    }
}
