using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class Motivo : Motivo_B
    {

        public Motivo()
        {
        }

        public enum TiposMotivos
        {
            Analise = 'N',
            Fraude = 'F',
            AguardandoTroca = 'A',
            Aprovado = 'P',
            Cancelamento = 'C',
            Sangria = 'S',
            CancelamentoParcial = 'X',
            CancelamentoNovo = 'E'
        }

        public List<ClientObjects.EstruturaMotivo> MotivoByTipo(TiposMotivos Tipo)
        {
            List<ClientObjects.EstruturaMotivo> lista = new List<ClientObjects.EstruturaMotivo>();

            try
            {
                string sql = "SELECT ID, Motivo  FROM tMotivo(nolock) WHERE Tipo = '" + (char)Tipo + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new ClientObjects.EstruturaMotivo()
                    {
                        ID = bd.LerInt("ID"),
                        motivo = bd.LerString("Motivo"),
                        tipo = ((char)Tipo).ToString(),
                    });
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool CancelamentoEnviarEmail(int motivoId)
        {
            try
            {
                string sql = "SELECT TOP 1 CASE WHEN EnviaEmailCliente = 'T' THEN 1 ELSE 0 END FROM tMotivo (NOLOCK) WHERE ID = " + motivoId;
                return (Convert.ToBoolean(bd.ConsultaValor(sql)));
            }
            finally
            {
                bd.Fechar();
            }
        }
        public string NomeArquivoHTMLCancelamento(int motivoId)
        {
            try
            {
                return Convert.ToString(bd.ConsultaValor("SELECT ArquivoEmail FROM tMotivo (NOLOCK) WHERE ID = " + motivoId));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaHistoricoDeAcaoDaVenda> CarregaListagemCompletaDeAcoes(string SenhaVendaBilheteria)
        {
            try
            {
                string sql = string.Empty;
                List<EstruturaHistoricoDeAcaoDaVenda> lista = new List<EstruturaHistoricoDeAcaoDaVenda>();

                sql = string.Format(@"select TOP 1 v.ID as VendaBilheteriaID , u.Nome  as Usuario , v.DataVenda as DataMudanca , StatusAnterior as Acao
                                    FROM  tVendaBilheteria v (NOLOCK)
                                    INNER JOIN tHistoricoMudancaAcao h (NOLOCK) on v.ID = h.VendaBilheteriaID
                                    INNER JOIN tCaixa c (NOLOCK) on v.CaixaID = c.ID
                                    INNER JOIN tUsuario u (NOLOCK) on c.UsuarioID = u.ID
                                    WHERE v.Senha = '" + SenhaVendaBilheteria + "' Order BY h.TimeStamp");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaHistoricoDeAcaoDaVenda()
                    {
                        VendaBilheteriaID = bd.LerString("VendaBilheteriaID"),
                        acao = bd.LerString("acao"),
                        dataMudanca = bd.LerDateTime("dataMudanca"),
                        motivo = " - ",
                        observacao = " - ",
                        usuario = bd.LerString("usuario"),
                    });
                }

                sql = string.Empty;
                sql = string.Format(@"SELECT h.timestamp as DataMudanca , h.Obs as Observacao , 
                                    u.Nome as Usuario , m1.Motivo as Motivo, m1.Tipo as Acao
                                    FROM tHistoricoMudancaAcao h (NOLOCK)
                                    INNER JOIN tUsuario u (NOLOCK) on h.usuarioID = u.ID
                                    INNER JOIN tMotivo m1 (NOLOCK) on h.MotivoID = m1.ID
                                    WHERE h.VendaBilheteriaID = " + lista[0].VendaBilheteriaID + " Order by DataMudanca");

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaHistoricoDeAcaoDaVenda()
                    {
                        acao = bd.LerString("acao"),
                        dataMudanca = bd.LerDateTime("dataMudanca"),
                        motivo = bd.LerString("motivo"),
                        observacao = bd.LerString("observacao"),
                        usuario = bd.LerString("usuario"),
                    });
                }

                return lista;
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

        public string BuscarMotivo(int motivoID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tMotivo.Motivo 
                        FROM  tMotivo (NOLOCK)
                        WHERE ID = {0} ", motivoID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao procurar o Motivo");
                return bd.LerString("Motivo");
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}



