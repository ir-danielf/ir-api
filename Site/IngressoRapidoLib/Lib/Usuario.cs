using System;
using System.Configuration;
using System.Data;

namespace IngressoRapido.Lib
{
    public class Usuario
    {
        public int Login(string login, string senha)
        {
            IRLib.Usuario Usuario = new IRLib.Usuario();
            Usuario.Login.Valor = login;
            Usuario.Senha.Valor = senha;
            Usuario.Validar();
            return Usuario.Control.ID;

        }

        public DataTable CarregarCanais(int UsuarioID)
        {
            IRLib.Usuario Usuario = new IRLib.Usuario();
            Usuario.Control.ID = UsuarioID;

            var canais = ConfigurationManager.AppSettings["CanaisInternet"];
            if (string.IsNullOrEmpty(canais))
                throw new Exception("O Arquivo de configuração não contém a chave CanaisInternet.");

            var canaisSplit = canais.Split(',');

            DataTable CanaisInternet = Usuario.PerfisCanalVenderInternet(canaisSplit);
            CanaisInternet.Merge(Usuario.PerfilSacEspecial());

            return CanaisInternet;
        }
    }
}
