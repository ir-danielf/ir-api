/**************************************************
* Arquivo: AssinaturaBancoIngressoCredito.cs
* Gerado: 01/12/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Assinaturas.Models;
using System;

namespace IRLib
{

    public class AssinaturaBancoIngressoCredito : AssinaturaBancoIngressoCredito_B
    {

        public AssinaturaBancoIngressoCredito() { }


        public int BuscarCredito(int clienteID)
        {
            try
            {
                int quantidadeCredito = Convert.ToInt32(bd.ConsultaValor("SELECT COUNT(ID) FROM tAssinaturaBancoIngressoCredito WHERE ClienteID = " + clienteID + " AND Utilizado = 'F'"));
                int quantidadeResgate = Convert.ToInt32(bd.ConsultaValor("SELECT COUNT(ID) AS Quantidade FROM tAssinaturaBancoIngressoResgate (NOLOCK) WHERE ClienteID =" + clienteID));

                return quantidadeCredito - quantidadeResgate;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int BuscarCredito(int clienteID, int anoAtivoBancoIngresso, int AssinaturaTipoIDBancoIngresso)
        {
            try
            {
                int quantidadeCredito = Convert.ToInt32(bd.ConsultaValor
                                                    (string.Format(@"SELECT COUNT(tAssinaturaBancoIngressoCredito.ID) FROM tAssinaturaBancoIngressoCredito(NOLOCK)
			                                                         INNER JOIN tAssinaturaAno(NOLOCK) ON tAssinaturaAno.ID = tAssinaturaBancoIngressoCredito.AssinaturaAnoID
			                                                         INNER JOIN tAssinatura(NOLOCK) ON tAssinatura.ID = tAssinaturaAno.AssinaturaID
			                                                         WHERE tAssinatura.AssinaturaTipoID = {2} AND ClienteID = {0} AND tAssinaturaAno.Ano = {1} AND Utilizado = 'F'", clienteID, anoAtivoBancoIngresso, AssinaturaTipoIDBancoIngresso)));

                int quantidadeResgate = Convert.ToInt32(bd.ConsultaValor(
                                                    string.Format(@"SELECT COUNT(tAssinaturaBancoIngressoResgate.ID) FROM tAssinaturaBancoIngressoResgate(NOLOCK)
			                                                        INNER JOIN tAssinaturaBancoIngresso(NOLOCK) ON tAssinaturaBancoIngressoResgate.AssinaturaBancoIngressoID = tAssinaturaBancoIngresso.ID
			                                                        INNER JOIN tAssinatura(NOLOCK) ON tAssinatura.ID = tAssinaturaBancoIngresso.AssinaturaID
			                                                        WHERE tAssinatura.AssinaturaTipoID = {2} AND tAssinaturaBancoIngressoResgate.ClienteID = {0} AND tAssinaturaBancoIngresso.Ano = {1}", clienteID, anoAtivoBancoIngresso, AssinaturaTipoIDBancoIngresso)));

                return quantidadeCredito - quantidadeResgate;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public Credito Creditos(int clienteID,  int AssinaturaTipoIDBancoIngresso)
        {
            try
            {
                string sql = string.Format(@"
                    SELECT
	                        bir.ID AS Credito, bir.Utilizado, COUNT(bih.ID) AS Doado
	                    FROM tAssinaturaBancoIngressoCredito bir (NOLOCK)
                        INNER JOIN tAssinaturaAno ano (NOLOCK) ON ano.ID = bir.AssinaturaAnoID
						INNER JOIN tAssinatura ta (NOLOCK) ON ta.ID = ano.AssinaturaID	                    
                        LEFT JOIN tAssinaturaBancoIngressoHistorico bih (NOLOCK) ON bir.ID = bih.AssinaturaBancoIngressoCreditoID
	                    LEFT JOIN tAssinaturaBancoIngressoComprovante bic (NOLOCK) ON bih.AssianturaBancoIngressoComprovanteID = bic.ID
	                    WHERE bir.ClienteID = {0} AND (bic.Acao = 'D' OR bic.ID IS NULL) AND bir.Utilizado = 'F' and ta.AssinaturaTipoID = {1}
	                    GROUP BY bir.ID, bir.Utilizado
	                    ORDER BY bir.ID DESC
                    ", clienteID, AssinaturaTipoIDBancoIngresso);

                if (!bd.Consulta(sql).Read())
                    return new Credito() { };

                var credito = new Credito() { };

                do
                {
                    if (bd.LerInt("Doado") > 0)
                        credito.Doacoes++;
                    else
                        credito.Concedidos++;

                    if (bd.LerBoolean("Utilizado"))
                        credito.Utilizados++;
                }
                while (bd.Consulta().Read());

                return credito;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ConcederCreditos(int clienteID, int concedidos, int AssinaturaTipoIDBancoIngresso)
        {
            try
            {
                if (clienteID == 0)
                    throw new Exception("Você deve selecionar um cliente para continuar!");

                var concedidosAtual = this.Creditos(clienteID, AssinaturaTipoIDBancoIngresso).Concedidos;

                var conceder = concedidos - concedidosAtual;

                if (conceder > 0)
                    for (int i = 0; i < conceder; i++)
                        bd.Executar("INSERT INTO tAssinaturaBancoIngressoCredito VALUES (" + clienteID + ", 'F', 349)");
                else
                    throw new Exception("Não é possível decrementar a quantidade de créditos concedidos.");
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaBancoIngressoCreditoLista : AssinaturaBancoIngressoCreditoLista_B
    {

        public AssinaturaBancoIngressoCreditoLista() { }

    }

}
