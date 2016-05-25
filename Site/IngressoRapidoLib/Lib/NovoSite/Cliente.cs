using IngressoRapido.Lib;
using IRLib;
using System;
using System.Configuration;

namespace IngressoRapido
{
    public class Cliente
    {
        IRLib.Cliente oCliente = new IRLib.Cliente();
        IngressoRapido.Lib.Login oLogin = new IngressoRapido.Lib.Login();
        private string ChaveCriptografiaLogin = ConfigurationManager.AppSettings["ChaveCriptografiaLogin"];

        public EstruturaCadastroCliente RetornaCadastro(string login, string senha)
        {
            try
            {
                int[] retorno = oLogin.BuscaClienteEmailSenhaWebReduzido(login, senha);

                if (retorno[0] == (int)IRLib.Cliente.Infos.ClienteInexistente)
                    throw new Exception("Cliente Inexistente!");
                else if (retorno[0] == (int)IRLib.Cliente.Infos.InfoIncorreta)
                    throw new Exception("Informações Incorretas!");
                else
                    return oCliente.BuscaCadastro(retorno[1], IRLib.Criptografia.Crypto.Criptografar(senha, ChaveCriptografiaLogin));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public EstruturaCadastroCliente RetornaCadastroCliente(string login, string senha)
        {
            try
            {
                int[] retorno = oCliente.BuscaClienteEmailSenhaWebReduzido(login, senha);

                if (retorno[0] == (int)IRLib.Cliente.Infos.ClienteInexistente)
                    throw new Exception("Cliente Inexistente!");
                else if (retorno[0] == (int)IRLib.Cliente.Infos.InfoIncorreta)
                    throw new Exception("Informações Incorretas!");
                else
                    return oCliente.BuscaCadastro(retorno[1], IRLib.Criptografia.Crypto.Criptografar(senha, ChaveCriptografiaLogin));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
