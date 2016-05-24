/**************************************************
* Arquivo: MensagemRegional.cs
* Gerado: 11/04/2011
* Autor: Celeritas Ltda
***************************************************/


namespace IRLib.Paralela
{

    public class MensagemRegional : MensagemRegional_B
    {

        public MensagemRegional() { }

        public MensagemRegional(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public void Inserir(int regionalID, int mensagemID)
        {
            this.RegionalID.Valor = regionalID;
            this.MensagemID.Valor = mensagemID;
            this.Inserir();
        }
    }

    public class MensagemRegionalLista : MensagemRegionalLista_B
    {

        public MensagemRegionalLista() { }

        public MensagemRegionalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
