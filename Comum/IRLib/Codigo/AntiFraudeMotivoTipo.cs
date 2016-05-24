/**************************************************
* Arquivo: AntiFraudeMotivoTipo.cs
* Gerado: 18/08/2011
* Autor: Celeritas Ltda
***************************************************/


namespace IRLib
{

    public class AntiFraudeMotivoTipo : AntiFraudeMotivoTipo_B
    {
        public enum enumAntiFraudeMotivoTipo
        {
            CartaoOutraPessoa = 1,
            CompraRisco = 2
        }

        public AntiFraudeMotivoTipo() { }

        public AntiFraudeMotivoTipo(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }

    public class AntiFraudeMotivoTipoLista : AntiFraudeMotivoTipoLista_B
    {

        public AntiFraudeMotivoTipoLista() { }

        public AntiFraudeMotivoTipoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}
