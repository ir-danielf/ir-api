using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using IRCore.Util;

namespace IRCore.DataAccess.ADO
{
    public class ClienteADO : MasterADO<dbIngresso>
    {
        public int SiteId { get; set; }

        public ClienteADO(MasterADOBase ado = null) : base(ado) { SiteId = 1; }

        public List<ClienteComprasCotaNetModel> ListaComprasCotaNet(string cpf)
        {
            var queryStr = @"SELECT 
                                c.CPF AS CPFResponsavelCompra
                                ,c.Nome AS NomeResponsavelCompra
                                ,l.Nome AS Local
                                ,e.Nome AS Evento
                                ,(SUBSTRING(a.Horario,7,2) + '/' + SUBSTRING(a.Horario,5,2) + '/' + SUBSTRING(a.Horario,1,4)) AS DataApresentacao
                                ,(SUBSTRING(a.Horario,9,2) + ':' + SUBSTRING(a.Horario,11,2) + ':' + SUBSTRING(a.Horario,13,2)) AS HoraApresentacao
                                ,s.Nome AS Setor
                                ,(SUBSTRING(vb.DataVenda,7,2) + '/' + SUBSTRING(vb.DataVenda,5,2) + '/' + SUBSTRING(vb.DataVenda,1,4) + ' ' + SUBSTRING(vb.DataVenda,9,2) + ':' + SUBSTRING(vb.DataVenda,11,2) + ':' + SUBSTRING(vb.DataVenda,13,2)) AS DataCompra 
                                ,p.Valor AS ValorTicket
                                ,COUNT(*) AS QuantidadeTicket
                                ,vb.senha as Senha
                            FROM 
                                tIngresso i(NOLOCK) 
                                LEFT JOIN tVendaBilheteria vb (NOLOCK) on vb.ID = i.VendaBilheteriaID
                                INNER JOIN tIngressoCliente ic (NOLOCK) on ic.IngressoID = i.ID
                                INNER JOIN tCliente c (NOLOCK) on c.ID = i.ClienteID
                                INNER JOIN tCotaItem cti (NOLOCK) on cti.ID = ic.CotaItemID
                                INNER JOIN tLocal l (NOLOCK) on l.ID = i.LocalID	
                                INNER JOIN tEvento e (NOLOCK) on e.ID = i.EventoID
                                INNER JOIN tApresentacao a (NOLOCK) on a.ID = i.ApresentacaoID
                                INNER JOIN tSetor s (NOLOCK) on s.ID = i.SetorID
                                INNER JOIN tPreco p (NOLOCK) on p.ID = i.PrecoID
                            WHERE 
                                cti.ParceiroID = 786
                                AND cti.ID = 4042
                                and c.CPF = @cpf
                                and c.CPF COLLATE Latin1_General_CI_AI = ic.CodigoPromocional COLLATE Latin1_General_CI_AI 
                            GROUP BY 
                                c.CPF
                                ,c.Nome
                                ,l.Nome
                                ,e.Nome
                                ,a.Horario
                                ,s.Nome
                                ,vb.DataVenda
                                ,p.Valor
                                ,ic.CodigoPromocional
                                ,vb.senha
                            ORDER BY 
                                c.CPF";
            var result = conIngresso.Query<ClienteComprasCotaNetModel>(queryStr, new { cpf = cpf }).ToList();
            return result;
        }

        public tCliente Consultar(int id)
        {
            const string sql = @"SELECT c.* FROM tCliente(NOLOCK) AS c WHERE ID = @id";

            var query = conIngresso.Query<tCliente>(sql, new { id });

            return query.FirstOrDefault();
        }

        public tCliente ConsultarOSESP(int id)
        {
            const string queryString = @"SELECT tCliente.ID,tCliente.Nome,tCliente.RG,tCliente.CPF,tCliente.CarteiraEstudante,tCliente.Sexo,tCliente.DDDTelefone,tCliente.Telefone,tCliente.DDDTelefoneComercial,tCliente.TelefoneComercial,tCliente.DDDCelular,tCliente.Celular,tCliente.DataNascimento,tCliente.Email,tCliente.RecebeEmail,tCliente.CEP,tCliente.Endereco,tCliente.Numero,tCliente.Cidade,tCliente.Estado,tCliente.ClienteIndicacaoID,tCliente.Obs,tCliente.Complemento,tCliente.Bairro,tCliente.Senha,tCliente.Ativo,tCliente.StatusAtual,tCliente.LoginOSESP,tCliente.CEPEntrega,tCliente.EnderecoEntrega,tCliente.NumeroEntrega,tCliente.CidadeEntrega,tCliente.EstadoEntrega,tCliente.ComplementoEntrega,tCliente.BairroEntrega,tCliente.CEPCliente,tCliente.EnderecoCliente,tCliente.NumeroCliente,tCliente.CidadeCliente,tCliente.EstadoCliente,tCliente.ComplementoCliente,tCliente.BairroCliente,tCliente.NomeEntrega,tCliente.CPFEntrega,tCliente.RGEntrega,tCliente.CPFConsultado,tCliente.NomeConsultado,tCliente.StatusConsulta,tCliente.CPFConsultadoEntrega,tCliente.NomeConsultadoEntrega,tCliente.StatusConsultaEntrega,tCliente.Pais,tCliente.CPFResponsavel,tCliente.Updated,tCliente.ContatoTipoID,tCliente.NivelCliente,tCliente.CNPJ,tCliente.NomeFantasia,tCliente.RazaoSocial,tCliente.InscricaoEstadual,tCliente.TipoCadastro,tCliente.Profissao,tCliente.SituacaoProfissionalID,tCliente.DDDTelefoneComercial2,tCliente.TelefoneComercial2,tCliente.DataCadastro,SiteID
                                    FROM tCliente (NOLOCK) join API_Osesp_Assinantes (NOLOCK) a on tCliente.ID = a.ClienteID where tCliente.ID = @id";

            var query = conIngresso.Query<tCliente>(queryString, new
            {
                id
            });
            return query.FirstOrDefault();
        }

        public tCliente ConsultarUsername(string username)
        {
            const string queryString = @"SELECT ID,Nome,RG,CPF,CarteiraEstudante,Sexo,DDDTelefone,Telefone,DDDTelefoneComercial,TelefoneComercial,DDDCelular,Celular,DataNascimento,Email,RecebeEmail,CEP,Endereco,Numero,Cidade,Estado,ClienteIndicacaoID,Obs,Complemento,Bairro,Senha,Ativo,StatusAtual,LoginOSESP,CEPEntrega,EnderecoEntrega,NumeroEntrega,CidadeEntrega,EstadoEntrega,ComplementoEntrega,BairroEntrega,CEPCliente,EnderecoCliente,NumeroCliente,CidadeCliente,EstadoCliente,ComplementoCliente,BairroCliente,NomeEntrega,CPFEntrega,RGEntrega,CPFConsultado,NomeConsultado,StatusConsulta,CPFConsultadoEntrega,NomeConsultadoEntrega,StatusConsultaEntrega,Pais,CPFResponsavel,Updated,ContatoTipoID,NivelCliente,CNPJ,NomeFantasia,RazaoSocial,InscricaoEstadual,TipoCadastro,Profissao,SituacaoProfissionalID,DDDTelefoneComercial2,TelefoneComercial2,DataCadastro,SiteID
                                    FROM tCliente (NOLOCK) where (Email = @username OR CPF = @username) AND SiteID = @siteId";

            var query = conIngresso.Query<tCliente>(queryString, new
            {
                username,
                siteId = SiteId
            });

            return query.FirstOrDefault();
        }

        public tCliente ConsultarEmailCPF(string email, string cpf)
        {
            const string queryString = @"SELECT * FROM tCliente (NOLOCK) where email = @email AND cpf = @cpf AND SiteID = @siteId";

            var query = conIngresso.Query<tCliente>(queryString, new
            {
                email = email,
                cpf = cpf.Replace(".", "").Replace("-", ""),
                siteId = SiteId
            });

            return query.FirstOrDefault();
        }

        public tCliente ConsultarEmail(string email)
        {
            const string queryString = @"SELECT ID,Nome,RG,CPF,CarteiraEstudante,Sexo,DDDTelefone,Telefone,DDDTelefoneComercial,TelefoneComercial,DDDCelular,Celular,DataNascimento,Email,RecebeEmail,CEP,Endereco,Numero,Cidade,Estado,ClienteIndicacaoID,Obs,Complemento,Bairro,Senha,Ativo,StatusAtual,LoginOSESP,CEPEntrega,EnderecoEntrega,NumeroEntrega,CidadeEntrega,EstadoEntrega,ComplementoEntrega,BairroEntrega,CEPCliente,EnderecoCliente,NumeroCliente,CidadeCliente,EstadoCliente,ComplementoCliente,BairroCliente,NomeEntrega,CPFEntrega,RGEntrega,CPFConsultado,NomeConsultado,StatusConsulta,CPFConsultadoEntrega,NomeConsultadoEntrega,StatusConsultaEntrega,Pais,CPFResponsavel,Updated,ContatoTipoID,NivelCliente,CNPJ,NomeFantasia,RazaoSocial,InscricaoEstadual,TipoCadastro,Profissao,SituacaoProfissionalID,DDDTelefoneComercial2,TelefoneComercial2,DataCadastro,SiteID
                                    FROM tCliente (NOLOCK) where Email = @email and SiteID = @siteId";

            var query = conIngresso.Query<tCliente>(queryString, new
            {
                email,
                siteId = SiteId
            });

            return query.FirstOrDefault();
        }

        public tCliente ConsultarCPF(string cpf)
        {
            const string queryString = @"SELECT ID,Nome,RG,CPF,CarteiraEstudante,Sexo,DDDTelefone,Telefone,DDDTelefoneComercial,TelefoneComercial,DDDCelular,Celular,DataNascimento,Email,RecebeEmail,CEP,Endereco,Numero,Cidade,Estado,ClienteIndicacaoID,Obs,Complemento,Bairro,Senha,Ativo,StatusAtual,LoginOSESP,CEPEntrega,EnderecoEntrega,NumeroEntrega,CidadeEntrega,EstadoEntrega,ComplementoEntrega,BairroEntrega,CEPCliente,EnderecoCliente,NumeroCliente,CidadeCliente,EstadoCliente,ComplementoCliente,BairroCliente,NomeEntrega,CPFEntrega,RGEntrega,CPFConsultado,NomeConsultado,StatusConsulta,CPFConsultadoEntrega,NomeConsultadoEntrega,StatusConsultaEntrega,Pais,CPFResponsavel,Updated,ContatoTipoID,NivelCliente,CNPJ,NomeFantasia,RazaoSocial,InscricaoEstadual,TipoCadastro,Profissao,SituacaoProfissionalID,DDDTelefoneComercial2,TelefoneComercial2,DataCadastro,SiteID
                                    FROM tCliente (NOLOCK) where CPF = @cpf and SiteID = @siteId";

            var query = conIngresso.Query<tCliente>(queryString, new
            {
                cpf,
                siteId = SiteId
            });

            return query.FirstOrDefault();
        }

        public List<tContatoTipo> ListarContatoTipo()
        {
            var sql = @"SELECT ID,Nome
                           FROM tContatoTipo(NOLOCK)";
            var query = conIngresso.Query<tContatoTipo>(sql);

            return query.ToList();
        }

        public List<tClienteEndereco> ListarEndereco(int clienteId)
        {
            var queryStr = @"
                                SELECT
	                                ID,CEP,Endereco,Numero,Cidade,Estado,Complemento,Bairro,Nome,CPF,RG,ClienteID,EnderecoTipoID,EnderecoPrincipal,StatusConsulta
                                FROM 
	                                tClienteEndereco (NOLOCK)
                                WHERE
	                                ClienteID = @ClienteID";
            var result = conIngresso.Query<tClienteEndereco>(queryStr, new { ClienteID = clienteId }).ToList();
            return result;
        }

        public bool Salvar(tCliente cliente, int usuarioId)
        {
            var retorno = dbIngresso.salvar_tCliente2ComContatoTipoID(
                usuarioId,
                cliente.ID,
                DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                cliente.Nome,
                cliente.RG,
                cliente.CPF,
                cliente.CarteiraEstudante,
                cliente.Sexo,
                cliente.DDDTelefone,
                cliente.Telefone,
                cliente.DDDTelefoneComercial,
                cliente.TelefoneComercial,
                cliente.DDDCelular,
                cliente.Celular,
                cliente.DataNascimento,
                cliente.Email,
                cliente.RecebeEmail,
                cliente.CEPEntrega,
                cliente.EnderecoEntrega,
                cliente.NumeroEntrega,
                cliente.CidadeEntrega,
                cliente.EstadoEntrega,
                cliente.CEPCliente,
                cliente.EnderecoCliente,
                cliente.NumeroCliente,
                cliente.CidadeCliente,
                cliente.EstadoCliente,
                cliente.ClienteIndicacaoID,
                cliente.Obs,
                cliente.ComplementoEntrega,
                cliente.BairroEntrega,
                cliente.ComplementoCliente,
                cliente.BairroCliente,
                cliente.Senha,
                cliente.Ativo,
                cliente.StatusAtual,
                null,
                cliente.NomeEntrega,
                cliente.CEPEntrega,
                cliente.RGEntrega,
                cliente.Pais,
                cliente.CPFResponsavel,
                cliente.ContatoTipoID,
                cliente.CNPJ,
                cliente.NomeFantasia,
                cliente.RazaoSocial,
                cliente.InscricaoEstadual,
                cliente.Profissao,
                cliente.SituacaoProfissionalID,
                cliente.DDDTelefoneComercial2,
                cliente.TelefoneComercial2,
                cliente.LoginOSESP,
                SiteId
                ).FirstOrDefault();

            if (retorno != null)
            {
                if ((retorno.Retorno == 1) || (retorno.Retorno == 2))
                {
                    cliente.ID = retorno.ClienteID;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Método que consulta um endereço de um cliente pelo CEP e número
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="CEP"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public tClienteEndereco Consultar(int clienteID, string cep, string numero)
        {
            var query = @"SELECT ID,
                                 CEP,
                                 Endereco,
                                 Numero,
                                 Cidade,
                                 Estado,
                                 Complemento,
                                 Bairro,
                                 Nome,
                                 CPF,
                                 RG,
                                 ClienteID,
                                 EnderecoTipoID,
                                 EnderecoPrincipal,
                                 StatusConsulta
                          FROM dbo.tClienteEndereco tce (NOLOCK)
	                      WHERE tce.ClienteID = @clienteId AND tce.CEP = @cep AND tce.Numero = @numero";

            var endereco = conIngresso.Query<tClienteEndereco>(query, new { clienteId = clienteID, cep = cep, numero = numero }).FirstOrDefault();
            return endereco;
        }

        public tClienteEndereco ConsultarEndereco(int clienteEnderecoID)
        {
            const string sql = @"SELECT TOP 1 ID, CEP, Endereco, Numero, Cidade, Estado, Complemento, Bairro, Nome, CPF, RG, ClienteID, EnderecoTipoID, EnderecoPrincipal, StatusConsulta FROM tClienteEndereco (NOLOCK) WHERE ID = @id";
            var endereco = conIngresso.Query<tClienteEndereco>(sql, new { id = clienteEnderecoID }).FirstOrDefault();
            return endereco;
        }

        public tClienteEndereco ConsultarEndereco(string cep, string endereco, string numero, string complemento, int clienteID)
        {
            var sqlString = @"
                                SELECT
                                    ID,CEP,Endereco,Numero,Cidade,Estado,Complemento,Bairro,Nome,CPF,RG,ClienteID,EnderecoTipoID,EnderecoPrincipal,StatusConsulta
                                FROM
                                    tClienteEndereco (NOLOCK)
                                WHERE
                                    CEP = @CEP AND Endereco = @Endereco AND Numero = @Numero AND Complemento = @Complemento AND ClienteID = @ClienteID";
            return conIngresso.Query<tClienteEndereco>(sqlString, new { CEP = cep, Endereco = endereco, Numero = numero, Complemento = complemento, ClienteID = clienteID }).FirstOrDefault();
        }

        public List<tCartao> ListarCartoes(List<int> ids)
        {
            return (from item in dbIngresso.tCartao
                    where ids.Any(x => x == item.ID)
                    select item).ToList();
        }

        public bool VerificarOsesp(int clienteID)
        {
            var queryString = @"SELECT 
                                        count([AssinaturaID])
                                   FROM [API_Osesp_Assinantes] (NOLOCK)
                                        where [ClienteID] = @id";

            var query = conIngresso.Query<int>(queryString, new
            {
                id = clienteID
            });

            if (query.FirstOrDefault() > 0)
                return true;
            return false;
        }

        public int InserirEndereco(tClienteEndereco clienteEndereco)
        {
            LogUtil.Debug(string.Format("##Post.ClienteADO.InserirEndereco## CLIENTE {0}, ENDEREÇO {1}", clienteEndereco.ClienteID, clienteEndereco.Endereco));

            var sql =
@"INSERT INTO [tClienteEndereco]
           ([CEP]
           ,[Endereco]
           ,[Numero]
           ,[Cidade]
           ,[Estado]
           ,[Complemento]
           ,[Bairro]
           ,[Nome]
           ,[CPF]
           ,[RG]
           ,[ClienteID]
           ,[EnderecoTipoID]
           ,[EnderecoPrincipal]
           ,[StatusConsulta])
     VALUES
           (@CEP
           ,@Endereco
           ,@Numero
           ,@Cidade
           ,@Estado
           ,@Complemento
           ,@Bairro
           ,@Nome
           ,@CPF
           ,@RG
           ,@ClienteID
           ,@EnderecoTipoID
           ,@EnderecoPrincipal
           ,@StatusConsulta);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var result = conIngresso.Query<int>(sql, new
            {
                CEP = clienteEndereco.CEP,
                Endereco = clienteEndereco.Endereco,
                Numero = clienteEndereco.Numero,
                Cidade = clienteEndereco.Cidade,
                Estado = clienteEndereco.Estado,
                Complemento = clienteEndereco.Complemento,
                Bairro = clienteEndereco.Bairro,
                Nome = clienteEndereco.Nome,
                CPF = clienteEndereco.CPF,
                RG = clienteEndereco.RG,
                ClienteID = clienteEndereco.ClienteID,
                EnderecoTipoID = clienteEndereco.EnderecoTipoID,
                EnderecoPrincipal = clienteEndereco.EnderecoPrincipal,
                StatusConsulta = clienteEndereco.StatusConsulta
            });

            var clienteEnderecoId = result.Single();

            if (clienteEnderecoId <= 0)
            {
                LogUtil.Debug(string.Format("##Post.ClienteADO.InserirEndereco.ERROR## CLIENTE {0}, MSG {1}", clienteEndereco.ClienteID, "Endereço não inserido"));

                clienteEnderecoId = -1;
            }
            else
            {
                LogUtil.Debug(string.Format("##Post.ClienteADO.InserirEndereco.SUCCESS## CLIENTE {0}, ENDEREÇO {1}", clienteEndereco.ClienteID, clienteEnderecoId));
            }

            return clienteEnderecoId;
        }

        public bool AtualizarLoginNaBileto(int _loginId)
        {
            var sql = @"UPDATE Login SET SyncStatus = @syncStatus, UltimoAcesso = @ultimoAcesso WHERE ID = @loginId";

            var response = conSite.Execute(sql, new { syncStatus = SyncStatus.IMPORT_LATER, loginId = _loginId, ultimoAcesso = DateTime.Now.ToString("yyyyMMddHHmmss") });

            return response > 0;
        }

        public bool BloquearCliente(int ClienteID)
        {
            var sql = @"UPDATE tCliente SET StatusAtual = 'B' WHERE ID = @ClienteID";

            var response = conIngresso.Execute(sql, new { ClienteID });

            return response > 0;
        }
    }
}
