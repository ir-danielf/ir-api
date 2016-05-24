/**************************************************
* Arquivo: IngressoCodigoBarra.cs
* Gerado: 04/02/2009
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;

namespace IRLib
{

    public class IngressoCodigoBarra : IngressoCodigoBarra_B
    {

        public IngressoCodigoBarra() { }

        public IngressoCodigoBarra(int usuarioIDLogado) : base() { }

        public string StringInserir()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tIngressoCodigoBarra (EventoID,CodigoBarra,BlackList,TimeStamp) ");
                sql.Append("VALUES (" + this.EventoID.ValorBD + ",'" + this.CodigoBarra.ValorBD + "','" + this.BlackList.ValorBD + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");

                return sql.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string StringUpdate()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" UPDATE tIngressoCodigoBarra ");
                sql.Append(" Set BlackList = '" + this.BlackList.ValorBD + "', ");
                sql.Append(" Sincronizado = 'F', ");
                sql.Append(" TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ");
                sql.Append(" WHERE CodigoBarra = '" + this.CodigoBarra.Valor + "' AND EventoID = " + this.EventoID.Valor);

                return sql.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class IngressoCodigoBarraLista : IngressoCodigoBarraLista_B
    {

        public IngressoCodigoBarraLista() { }

        public IngressoCodigoBarraLista(int usuarioIDLogado) : base() { }

    }

}
