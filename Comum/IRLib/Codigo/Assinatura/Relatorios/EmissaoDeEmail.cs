using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using System;
using System.Collections.Generic;

namespace IRLib.Assinaturas.Relatorios
{
    public class EmissaoDeEmail
    {
        public EmissaoDeEmail() { }

        BD bd = new BD();

        public List<EmissaoDeEmails> ListarEmissaoEmails(int Remetente, int Assunto)
        {
            try
            {
                List<EmissaoDeEmails> lista = new List<EmissaoDeEmails>();

                string remetente = string.Empty;
                string assunto = string.Empty;

                if (Remetente > 0)
                    remetente = " AND tAssinaturaEmailRemetente.ID = '" + Remetente + "' ";

                if (Assunto > 0)
                    assunto = " AND tAssinaturaEmailModelo.ID = '" + Assunto + "' ";

                string sql = string.Format(@"SELECT DISTINCT
                CASE WHEN LEN(tAssinaturaEmailEnviar.DataEnvio) > 0
                THEN tAssinaturaEmailEnviar.DataEnvio
                ELSE '--'
                END AS DataEnvio,
                tAssinaturaEmailRemetente.NomeExibicao as Remetente, tAssinaturaEmailModelo.Assunto, COUNT(tAssinaturaEmailEnviar.ID) AS QuantidadeCliente
                FROM tAssinaturaEmailRemetente (NOLOCK)
                INNER JOIN tAssinaturaEmailModelo (NOLOCK) ON tAssinaturaEmailModelo.AssinautraEmailRemetenteID = tAssinaturaEmailRemetente.ID
                INNER JOIN tAssinaturaEmailEnviar (NOLOCK) ON tAssinaturaEmailEnviar.AssinaturaEmailModeloID = tAssinaturaEmailModelo.ID
                WHERE tAssinaturaEmailModelo.Salvo = 'F' {0} {1}
                GROUP BY DataEnvio, NomeExibicao, Assunto", remetente, assunto);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EmissaoDeEmails
                    {
                        DataEnvio = bd.LerString("DataEnvio"),
                        Assunto = bd.LerString("Assunto"),
                        Remetente = bd.LerString("Remetente"),
                        QuantidadeCliente = bd.LerInt("QuantidadeCliente")
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
