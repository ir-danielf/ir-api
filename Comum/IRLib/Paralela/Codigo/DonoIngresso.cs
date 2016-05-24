/**************************************************
* Arquivo: DonoIngresso.cs
* Gerado: 05/07/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;

namespace IRLib.Paralela
{

    public class DonoIngresso : DonoIngresso_B
    {

        public DonoIngresso() { }

        public override bool Inserir()
        {
            try
            {

                if (string.IsNullOrEmpty(this.CPF.Valor))
                    return base.Inserir();

                int donoID = Convert.ToInt32(bd.ConsultaValor("SELECT ID FROM tDonoIngresso (NOLOCK) WHERE CPF = '" + this.CPF.Valor + "'"));

                if (donoID > 0)
                {
                    this.Control.ID = donoID;
                    return this.Atualizar();
                }
                else
                    return base.Inserir();
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaDonoIngressoSite Buscar(int DonoID)
        {
            this.Ler(DonoID);
            return new EstruturaDonoIngressoSite()
            {
                Nome = this.Nome.Valor,
                RG = this.RG.Valor,
                CPF = this.CPF.Valor,
                Email = this.Email.Valor,
                DataNascimento = string.Compare(this.DataNascimento.Valor.ToString("ddMMyyyy"), "01011753", 0) == 0 ? string.Empty : this.DataNascimento.Valor.ToString("dd/MM/yyyy"),
                Telefone = this.Telefone.Valor,
                NomeResponsavel = this.NomeResponsavel.Valor,
                CPFResponsavel = this.CPFResponsavel.Valor
            };
        }
    }

    public class DonoIngressoLista : DonoIngressoLista_B
    {

        public DonoIngressoLista() { }

    }

}
