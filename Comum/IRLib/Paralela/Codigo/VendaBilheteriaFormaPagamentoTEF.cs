/**************************************************
* Arquivo: VendaBilheteriaFormaPagamentoTEF.cs
* Gerado: 17/11/2009
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Text;


namespace IRLib.Paralela
{
    [Obsolete("Não utilizar mais, valores inseridos na tVendaBilheteriaFormaPagamento",false)]
    public class VendaBilheteriaFormaPagamentoTEF : VendaBilheteriaFormaPagamentoTEF_B
    {

        public VendaBilheteriaFormaPagamentoTEF() { }


        //O método inserir da classe VendaBilheteriaFormaPagamentoTEFDB está tentando inserir o ID mas a coluna é identity. 
        //Por isso movemos o método para cá.
        public override bool Inserir()
        {

            try
            {
                
                StringBuilder sql = new StringBuilder();


                sql.Append("INSERT INTO tVendaBilheteriaFormaPagamentoTEF(CodigoRespostaVenda, MensagemRetorno, HoraTransacao, DataTransacao, CodigoIR, NumeroAutorizacao, NSUHost, NSUSitef, Cupom, DadosConfirmacaoVenda, Rede, CodigoRespostaTransacao) ");
                sql.Append("VALUES (@001,'@002','@003','@004','@005','@006',@007,@008,'@009','@010',@011,@012); SELECT SCOPE_IDENTITY()");

                sql.Replace("@001", this.CodigoRespostaVenda.ValorBD);
                sql.Replace("@002", this.MensagemRetorno.ValorBD);
                sql.Replace("@003", this.HoraTransacao.ValorBD);
                sql.Replace("@004", this.DataTransacao.ValorBD);
                sql.Replace("@005", this.CodigoIR.ValorBD);
                sql.Replace("@006", this.NumeroAutorizacao.ValorBD);
                sql.Replace("@007", this.NSUHost.ValorBD);
                sql.Replace("@008", this.NSUSitef.ValorBD);
                sql.Replace("@009", this.Cupom.ValorBD);
                sql.Replace("@010", this.DadosConfirmacaoVenda.ValorBD);
                sql.Replace("@011", this.Rede.ValorBD);
                sql.Replace("@012", this.CodigoRespostaTransacao.ValorBD);

                object x = bd.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bd.Fechar();

                bool result = this.Control.ID > 0;

                return result;

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
    }

    public class VendaBilheteriaFormaPagamentoTEFLista : VendaBilheteriaFormaPagamentoTEFLista_B
    {

        public VendaBilheteriaFormaPagamentoTEFLista() { }

    }

}
