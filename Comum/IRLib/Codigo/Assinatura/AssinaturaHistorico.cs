/**************************************************
* Arquivo: AssinaturaHistorico.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib
{

    public class AssinaturaHistorico : AssinaturaHistorico_B
    {

        public AssinaturaHistorico() { }

        public List<IRLib.Assinaturas.Models.Historico> BuscarHistorico(int clienteID, int assinaturaTipoID, string clienteNome)
        {
            try
            {
                string sql =
                    string.Format(@"
                        SELECT 
	                        DISTINCT 
		                        ac.ID AS AssinaturaClienteID, a.ID AS AssinaturaID, a.Nome AS Assinatura, 
		                        s.ID AS SetorID, s.Nome AS Setor, l.ID AS LugarID, l.Codigo, ah.Acao, ah.Status, ah.TimeStamp,
                                CASE WHEN ah.UsuarioID = {0}
									THEN '{1}'
									ELSE u.Nome
									END AS Usuario
	                        FROM tAssinaturaHistorico ah (NOLOCK)
	                        INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ah.AssinaturaClienteID = ac.ID
	                        INNER JOIN tAssinatura a (NOLOCK) ON ac.AssinaturaID = a.ID
	                        INNER JOIN tSetor s (NOLOCK) ON ac.SetorID = s.ID
	                        INNER JOIN tLugar l (NOLOCK) ON ac.LugarID = l.ID
                            LEFT JOIN tUsuario u (NOLOCK) ON u.ID = ah.UsuarioID    
	                        WHERE 
		                        ah.Acao <> 'A' AND ac.ClienteID = {2} AND a.AssinaturaTipoID = {3}
							ORDER BY ah.Timestamp DESC, a.Nome, s.Nome, l.Codigo
                        ", Usuario.INTERNET_USUARIO_ID, clienteNome, clienteID, assinaturaTipoID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem ações a serem exibidas.");

                List<IRLib.Assinaturas.Models.Historico> lista = new List<Assinaturas.Models.Historico>();
                do
                {
                    lista.Add(new Assinaturas.Models.Historico()
                    {
                        AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        Assinatura = bd.LerString("Assinatura"),
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("Setor"),
                        LugarID = bd.LerInt("LugarID"),
                        Codigo = bd.LerString("Codigo"),
                        Data = bd.LerDateTime("Timestamp"),
                        Usuario = bd.LerString("Usuario"),
                        Acao = (AssinaturaCliente.EnumAcao)Convert.ToChar(bd.LerString("Acao")),
                        Status = (AssinaturaCliente.EnumStatus)Convert.ToChar(bd.LerString("Status")),
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

    public class AssinaturaHistoricoLista : AssinaturaHistoricoLista_B
    {

        public AssinaturaHistoricoLista() { }

    }

}
