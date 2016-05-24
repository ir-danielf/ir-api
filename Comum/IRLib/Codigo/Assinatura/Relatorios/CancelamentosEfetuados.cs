using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;

namespace IRLib.Assinaturas.Relatorios
{
    public class CancelamentosEfetuados
    {
        BD bd = new BD();
        public List<AssinanteAssinatura> RelacaoDesistencias(int assinaturaTipoID, int temporada, int assinaturaID)
        {
            try
            {
                bd.Consulta("EXEC buscarAssinaturasDesistencia " + assinaturaTipoID + ", '" + (temporada > 0 ? temporada.ToString() : string.Empty) + "', " + assinaturaID);

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem registros de desistência para o filtro selecionado.");

                List<AssinanteAssinatura> lista = new List<AssinanteAssinatura>();

                do
                {
                    lista.Add(new AssinanteAssinatura()
                    {
                        Temporada = bd.LerInt("Ano"),
                        Login = bd.LerString("LoginOSESP"),
                        Nome = bd.LerString("Nome"),
                        CPF = bd.LerString("CPF"),
                        DataAcao = bd.LerDateTime("Data").ToShortDateString(),
                        Assinatura = bd.LerString("Assinatura"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Codigo"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
