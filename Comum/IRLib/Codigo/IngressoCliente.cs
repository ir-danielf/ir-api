/**************************************************
* Arquivo: IngressoCliente.cs
* Gerado: 01/02/2010
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;

namespace IRLib
{

    public class IngressoCliente : IngressoCliente_B
    {

        public IngressoCliente() { }

        /// <summary>
        /// Se o ClienteID for ZERO significa necessariamente que não é uma cota Nominal, inserir como NULO
        /// </summary>
        /// <returns></returns>
        public string StringInserir()
        {
            StringBuilder stbSQL = new StringBuilder();
            stbSQL.Append("INSERT INTO tIngressoCliente ");
            stbSQL.Append("( ApresentacaoSetorID, ApresentacaoID, DonoID, CotaItemID, IngressoID, CodigoPromocional, CPF ) ");
            stbSQL.Append("VALUES (");
            stbSQL.Append(this.ApresentacaoSetorID.Valor);
            stbSQL.Append(", ");
            stbSQL.Append(this.ApresentacaoID.Valor);
            stbSQL.Append(", ");

            if (this.DonoID.Valor == 0)
                stbSQL.Append("NULL");
            else
                stbSQL.Append(this.DonoID.Valor);

            stbSQL.Append(", ");
            stbSQL.Append(this.CotaItemID.Valor);
            stbSQL.Append(", ");
            stbSQL.Append(this.IngressoID.Valor);
            stbSQL.Append(", ");
            stbSQL.Append("'" + this.CodigoPromocional.Valor + "' ");
            stbSQL.Append(", ");

            if (this.DonoID.Valor == 0)
                stbSQL.Append("NULL)");
            else
                stbSQL.Append("'" + this.CPF.Valor + "')");

            return stbSQL.ToString();
        }

        public int[] QuantidadeJaComprada()
        {
            try
            {
                int[] qtds = new int[3] { 0, 0, 0 };
                string strSQL = "EXEC sp_getQuantidadePorClienteCota " + this.ApresentacaoID.Valor + ", " + this.ApresentacaoSetorID.Valor + ", " + this.DonoID.Valor + ", " + this.CotaItemID.Valor;
                bd.Consulta(strSQL);
                if (bd.Consulta().Read())
                {
                    qtds[0] = bd.LerInt("Apresentacao");
                    qtds[1] = bd.LerInt("ApresentacaoSetor");
                    qtds[2] = bd.LerInt("Cota");
                }

                return qtds;
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

        public int[] QuantidadeJaCompradaNovo(int CotaItemID, int CotaItemIDAPS, int IngressoID = 0)
        {
            try
            {
                int[] qtds = new int[4] { 0, 0, 0, 0 };
                string strSQL = "EXEC sp_getQuantidadePorClienteCotaCPF " + this.ApresentacaoID.Valor + ", " + this.ApresentacaoSetorID.Valor + ", '" + this.CPF.Valor + "', " + CotaItemID + ", " + CotaItemIDAPS + ", " + IngressoID;
                bd.Consulta(strSQL);
                if (bd.Consulta().Read())
                {
                    qtds[0] = bd.LerInt("Apresentacao");
                    qtds[1] = bd.LerInt("ApresentacaoSetor");
                    qtds[2] = bd.LerInt("Cota");
                    qtds[3] = bd.LerInt("CotaAPS");
                }

                return qtds;
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

    }

    public class IngressoClienteLista : IngressoClienteLista_B
    {

        public IngressoClienteLista() { }

    }

}
