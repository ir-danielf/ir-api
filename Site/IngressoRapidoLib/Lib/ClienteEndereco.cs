using IRLib.ClientObjects;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class ClienteEndereco
    {

        IRLib.ClienteEndereco oClienteEndereco;
        public int ID { get { return oClienteEndereco.Control.ID; } }

        public ClienteEndereco()
        {
            oClienteEndereco = new IRLib.ClienteEndereco();
        }

        public List<EstruturaClienteEndereco> ListaEndereco(int ClienteID)
        {
            List<EstruturaClienteEndereco> listaRetorno = oClienteEndereco.ListaEndereco(ClienteID);

            foreach (EstruturaClienteEndereco item in listaRetorno)
            {
                item.PodeAlterar = !oClienteEndereco.PossuiAgendada(item.ID);
                item.PodeExcluir = !oClienteEndereco.PossuiVenda(item.ID);
            }

            return listaRetorno;
        }

        public void Atualizar(EstruturaClienteEndereco estrutura)
        {
            oClienteEndereco.Atualizar(estrutura);
        }

        public void Inserir(EstruturaClienteEndereco estrutura)
        {
            oClienteEndereco.Inserir(estrutura);
        }

        public void Excluir(int ClienteEnderecoID)
        {
            oClienteEndereco.Excluir(ClienteEnderecoID);
        }

        public EstruturaClienteEndereco LerEstrutura(int ClienteEnderecoID)
        {
            return oClienteEndereco.LerEstrutura(ClienteEnderecoID);
        }

        public IRLib.Codigo.Brainiac.Retorno CadastroValido()
        {
            return oClienteEndereco.CadastroValido();
        }

        public bool PossuiVenda(int EnderecoID)
        {
            return oClienteEndereco.PossuiVenda(EnderecoID);
        }
    }
}
