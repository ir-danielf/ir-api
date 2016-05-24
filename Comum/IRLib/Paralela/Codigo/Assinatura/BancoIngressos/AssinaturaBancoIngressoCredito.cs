/**************************************************
* Arquivo: AssinaturaBancoIngressoCredito.cs
* Gerado: 01/12/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.Assinaturas.Models;
using System;

namespace IRLib.Paralela
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

        public Credito Creditos(int clienteID)
        {
            try
            {
                string sql = string.Format(@"
                    SELECT
	                        bir.ID AS Credito, bir.Utilizado, COUNT(bih.ID) AS Doado
	                    FROM tAssinaturaBancoIngressoCredito bir (NOLOCK)
	                    LEFT JOIN tAssinaturaBancoIngressoHistorico bih (NOLOCK) ON bir.ID = bih.AssinaturaBancoIngressoCreditoID
	                    LEFT JOIN tAssinaturaBancoIngressoComprovante bic (NOLOCK) ON bih.AssianturaBancoIngressoComprovanteID = bic.ID
	                    WHERE bir.ClienteID = {0} AND (bic.Acao = 'D' OR bic.ID IS NULL)
	                    GROUP BY bir.ID, bir.Utilizado
	                    ORDER BY bir.ID DESC
                    ", clienteID);

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

        public void ConcederCreditos(int clienteID, int concedidos)
        {
            try
            {
                if (clienteID == 0)
                    throw new Exception("Você deve selecionar um cliente para continuar!");

                var concedidosAtual = this.Creditos(clienteID).Concedidos;

                var conceder = concedidos - concedidosAtual;

                if (conceder > 0)
                    for (int i = 0; i < conceder; i++)
                        bd.Executar("INSERT INTO tAssinaturaBancoIngressoCredito VALUES (" + clienteID + ", 'F')");
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
