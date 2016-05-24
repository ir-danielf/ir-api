/**************************************************
* Arquivo: MensagemCanalTipo.cs
* Gerado: 11/04/2011
* Autor: Celeritas Ltda
***************************************************/


namespace IRLib.Paralela
{

    public class MensagemCanalTipo : MensagemCanalTipo_B
    {

        public MensagemCanalTipo() { }

        public MensagemCanalTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public void Inserir(int canalTipoID, int mensagemID)
        {
            this.CanalTipoID.Valor = canalTipoID;
            this.MensagemID.Valor = mensagemID;
            this.Inserir();
        }
    }

    public class MensagemCanalTipoLista : MensagemCanalTipoLista_B
    {

        public MensagemCanalTipoLista() { }

        public MensagemCanalTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
