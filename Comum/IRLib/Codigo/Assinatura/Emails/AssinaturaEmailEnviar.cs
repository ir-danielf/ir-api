/**************************************************
* Arquivo: AssinaturaEmailEnviar.cs
* Gerado: 19/10/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib
{

    public class AssinaturaEmailEnviar : AssinaturaEmailEnviar_B
    {

        public AssinaturaEmailEnviar() { }


        public void InserirRegistros(int assinaturaTipoID, int remetente, string assunto, string corpo, List<int> clientes)
        {
            try
            {
                var oAssinaturaEmailModelo = new AssinaturaEmailModelo();
                oAssinaturaEmailModelo.Nome.Valor = DateTime.Now.ToString();
                oAssinaturaEmailModelo.AssinautraEmailRemetenteID.Valor = remetente;
                oAssinaturaEmailModelo.Assunto.Valor = (assunto ?? string.Empty).Replace("'", "''");
                oAssinaturaEmailModelo.Corpo.Valor = (corpo ?? string.Empty).Replace("'", "''");
                oAssinaturaEmailModelo.Salvo.Valor = false;
                oAssinaturaEmailModelo.AssinaturaTipoID.Valor = assinaturaTipoID;
                oAssinaturaEmailModelo.Inserir();

                bd.BulkInsert(clientes, "#tmpClientesEmail", false, true);

                bd.Executar(
                    string.Format(@"
                      INSERT INTO tAssinaturaEmailEnviar (ClienteID, AssinaturaEmailModeloID, Enviado, DataEnvio, Erro)
                      SELECT ID, {0}, 'F', '', '' FROM #tmpClientesEmail  
                    ", oAssinaturaEmailModelo.Control.ID));
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaEmailEnviarLista : AssinaturaEmailEnviarLista_B
    {

        public AssinaturaEmailEnviarLista() { }
    }

}
