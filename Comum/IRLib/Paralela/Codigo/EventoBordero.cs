/**************************************************
* Arquivo: EventoBordero.cs
* Gerado: 28/09/2012
* Autor: Celeritas Ltda
***************************************************/

using System;

namespace IRLib.Paralela
{

    public class EventoBordero : EventoBordero_B
    {

        public EventoBordero() { }

        public EventoBordero(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void IncluirInformacoes(ClientObjects.EstruturaEventoBordero estrutura)
        {
            AtribuirEstrutura(estrutura);
            this.Inserir();
        }

        public void AtribuirEstrutura(ClientObjects.EstruturaEventoBordero estrutura)
        {
            this.Control.ID = estrutura.ID;
            this.EventoID.Valor = estrutura.EventoID;
            this.GestorRazaoSocial.Valor = estrutura.GestorRazaoSocial;
            this.GestorCpfCnpj.Valor = estrutura.GestorCpfCnpj;
            this.GestorEndereco.Valor = estrutura.GestorEndereco;
            this.ProdutorRazaoSocial.Valor = estrutura.ProdutorRazaoSocial;
            this.ProdutorCpfCnpj.Valor = estrutura.ProdutorCpfCnpj;
            this.ProdutorEndereco.Valor = estrutura.ProdutorEndereco;
        }

        /// <summary>
        /// Preenche todos os atributos de EventoBordero
        /// </summary>
        /// <param name="id">Informe o EventoID</param>
        /// <returns></returns>
        public void LerEvento(int eventoID)
        {

            try
            {

                string sql = "SELECT * FROM tEventoBordero WHERE EventoID = " + eventoID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.EventoID.ValorBD = bd.LerInt("EventoID").ToString();
                    this.GestorRazaoSocial.ValorBD = bd.LerString("GestorRazaoSocial");
                    this.GestorCpfCnpj.ValorBD = bd.LerString("GestorCpfCnpj");
                    this.GestorEndereco.ValorBD = bd.LerString("GestorEndereco");
                    this.ProdutorRazaoSocial.ValorBD = bd.LerString("ProdutorRazaoSocial");
                    this.ProdutorCpfCnpj.ValorBD = bd.LerString("ProdutorCpfCnpj");
                    this.ProdutorEndereco.ValorBD = bd.LerString("ProdutorEndereco");
                }
                else
                {
                    this.Limpar();
                    this.EventoID.Valor = eventoID;
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ClientObjects.EstruturaEventoBordero CarregarEstrutura(int eventoID)
        {
            ClientObjects.EstruturaEventoBordero retorno = new ClientObjects.EstruturaEventoBordero();
            this.LerEvento(eventoID);

            retorno.ID = this.Control.ID;
            retorno.EventoID = this.EventoID.Valor;
            retorno.GestorRazaoSocial = this.GestorRazaoSocial.Valor;
            retorno.GestorCpfCnpj = this.GestorCpfCnpj.Valor;
            retorno.GestorEndereco = this.GestorEndereco.Valor;
            retorno.ProdutorRazaoSocial = this.ProdutorRazaoSocial.Valor;
            retorno.ProdutorCpfCnpj = this.ProdutorCpfCnpj.Valor;
            retorno.ProdutorEndereco = this.ProdutorEndereco.Valor;

            return retorno;

        }
    }

    public class EventoBorderoLista : EventoBorderoLista_B
    {

        public EventoBorderoLista() { }

        public EventoBorderoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
