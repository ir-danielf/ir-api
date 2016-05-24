using CTLib;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class AssinantesEtiqueta
    {

        BD bd;

        public AssinantesEtiqueta()
        {
            bd = new BD();
        }

        public List<IRLib.Paralela.Assinaturas.Models.Cliente> BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas, bool Email)
        {
            try
            {
                Assinatura oAssinatura = new Assinatura();
                var filtro = "";

                if (Assinaturas > 0)
                    filtro += " AND ass.ID = " + Assinaturas;


                filtro += " AND ( ac.Status <> 'D' AND ac.Acao <> 'D' ) ";

                if (Email)
                    filtro += " AND (AND c.Email IS NOT NULL AND LEN(c.Email) > 0)  ";

                string sql = @"SELECT DISTINCT c.LoginOSESP, c.Senha, c.Nome, c.CPF, c.Sexo,
                                c.EnderecoCliente, c.ComplementoCliente, c.BairroCliente, c.NumeroCliente, c.CEPCliente, c.CidadeCliente, c.EstadoCliente,
                                c.Email,
                                c.DDDCelular +' '+ c.Celular +' '+ c.DDDTelefone +' '+ c.Telefone +' '+ c.DDDTelefoneComercial +' '+ c.TelefoneComercial AS Telefone,
                                c.Profissao, IsNull(sp.Situacao, '') AS SituacaoProfissional, c.DataNascimento
                                FROM tAssinaturaCliente ac(NOLOCK)
                                INNER JOIN tAssinaturaAno aa(NOLOCK) on aa.ID = ac.AssinaturaAnoID
                                INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = aa.AssinaturaID
                                INNER JOIN tCliente c(NOLOCK) on ac.ClienteID = c.ID
                                LEFT JOIN tSituacaoProfissional sp (NOLOCK) ON sp.ID = c.SituacaoProfissionalID
                                WHERE aa.Ano =  '" + Temporadas + "' AND ass.AssinaturaTipoID = " + AssinaturaTipoID + filtro + "  Order by c.Nome ";

                bd.Consulta(sql);

                var lstRetorno = new List<Models.Cliente>();

                while (bd.Consulta().Read())
                    lstRetorno.Add(new Models.Cliente
                    {
                        Login = bd.LerString("LoginOSESP"),
                        Senha = bd.LerString("Senha"),
                        Nome = bd.LerString("Nome"),
                        CPF = bd.LerString("CPF"),
                        Sexo = bd.LerString("Sexo"),
                        Endereco = bd.LerString("EnderecoCliente"),
                        Complemento = bd.LerString("ComplementoCliente"),
                        Bairro = bd.LerString("BairroCliente"),
                        EnderecoNumero = bd.LerString("NumeroCliente"),
                        Cidade = bd.LerString("CidadeCliente"),
                        Estado = bd.LerString("EstadoCliente"),
                        Cep = bd.LerString("CEPCliente"),
                        Email = bd.LerString("Email"),
                        TelResidencial = bd.LerString("Telefone"),
                        Profissao = bd.LerString("Profissao"),
                        SituacaoProfissional = bd.LerString("SituacaoProfissional"),
                        DataNascimento = bd.LerDateTime("DataNascimento") == DateTime.MinValue ? string.Empty : bd.LerDateTime("DataNascimento").ToString("dd/MM/yyyy"),
                    });

                return lstRetorno;
            }
            finally
            {
                bd.Fechar();

            }
        }


    }
}
