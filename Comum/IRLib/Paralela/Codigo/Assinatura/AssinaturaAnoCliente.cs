/**************************************************
* Arquivo: AssinaturaAnoCliente.cs
* Gerado: 12/10/2011
* Autor: Celeritas Ltda
***************************************************/

using System;

namespace IRLib.Paralela
{

    public class AssinaturaAnoCliente : AssinaturaAnoCliente_B
    {
        public AssinaturaAnoCliente() { }

        public bool JaAceitou(int assinaturaTipoID, int clienteID, int ano)
        {
            try
            {
                string sql =
                    string.Format(@"
                        SELECT 
                            COUNT(anc.ID)
                        FROM tAssinatura a (NOLOCK)
                        INNER JOIN tAssinaturaAno an (NOLOCK) ON a.ID = an.AssinaturaID
                        INNER JOIN tAssinaturaAnoCliente anc (NOLOCK) ON anc.AssinaturaAnoID = an.ID
                        WHERE a.AssinaturaTipoID = {0} AND an.Ano = '{1}' AND anc.ClienteID = {2}
                    ", assinaturaTipoID, ano, clienteID);

                if (Convert.ToInt32(bd.ConsultaValor(sql)) == 0)
                    return false;

                return true;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Aceitar(int assinaturaTipoID, int clienteID, int ano)
        {
            try
            {
                string sql =
                   string.Format(@"
                        SELECT 
                            TOP 1 an.ID
                        FROM tAssinatura a (NOLOCK)
                        INNER JOIN tAssinaturaAno an (NOLOCK) ON a.ID = an.AssinaturaID
                        WHERE a.AssinaturaTipoID = {0} AND an.Ano = '{1}'
                    ", assinaturaTipoID, ano);

                int assinaturaAnoID = Convert.ToInt32(bd.ConsultaValor(sql));

                if (assinaturaAnoID == 0)
                    return;

                bd.FecharConsulta();

                if (this.JaAceitou(assinaturaTipoID, clienteID, ano))
                    return;

                this.AssinaturaAnoID.Valor = assinaturaAnoID;
                this.ClienteID.Valor = clienteID;
                this.Inserir();
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaAnoClienteLista : AssinaturaAnoClienteLista_B
    {
        public AssinaturaAnoClienteLista() { }

    }

}
