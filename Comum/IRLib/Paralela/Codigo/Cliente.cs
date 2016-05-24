/**************************************************
* Arquivo: Cliente.cs
* Gerado: 01/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Text;
using System.Web.UI.WebControls;
namespace IRLib.Paralela
{

    public partial class Cliente : Cliente_B
    {

        private const int UsuarioSiteID = 1657;

        private const string DDDSP = "11";

        public enum Infos
        {
            ErroIndefinido = -1,	// Erro não definido
            ClienteExistente = 0,	// Cliente existe no banco de dados
            Sucesso = 1,			// Sucesso
            ClienteInexistente = 2,	// Cliente não existe
            ClienteSemSenha = 3,	// Cliente cadastrado, porém não possui senha. Obrigar a atualização.
            ErroDatabase = 5,		// Erro de banco de dados.
            NaoAtivado = 6,			// Cliente ainda não ativou o seu cadastro
            InfoIncorreta = 7,		// Informações incorretas
            ClienteBloqueado = 8    // Cliente Bloqueado
        }

        public enum StatusCliente
        {
            [System.ComponentModel.Description("Bloqueado")]
            Bloqueado = 1,
            [System.ComponentModel.Description("Liberado")]
            Liberado = 2
        }

        public enum StatusClienteChar
        {
            Bloqueado = 'B',
            Liberado = 'L'
        }

        public enum TipoCadastroCliente
        {
            PessoaFisica = 'F',
            PessoaJuridica = 'J'
        }

        public enum RetornoProcSalvar
        {
            [System.ComponentModel.Description("Cliente salvo com sucesso.")]
            InsertOK = 1,
            [System.ComponentModel.Description("Cadastro do cliente atualizado sucesso.")]
            UpdateOK = 2,
            [System.ComponentModel.Description("CPF e/ou E-mail já cadastrado(s).")]
            CPF_Email_JaExistem = 3,
            [System.ComponentModel.Description("CPF ou E-mail em Branco.")]
            CPF_Email_Vazios = 4,
            [System.ComponentModel.Description("CNPJ já cadastrado.")]
            CNPJ_JaExiste = 5
        }

        public enum Duplicidade
        {
            NaoDefinido,
            CPF,
            Email
        }

        public enum enumTipoBusca
        {
            Invalido = 0,
            CPF = 1,
            Email = 2,
            Nome = 3,
            CNPJ = 4
        }

        public enum enumTipoBuscaVendas
        {
            ClienteVendasCom = 1,
            ClienteVendasSem = 2,
            ClienteVendasAmbos = 3
        }

        public Cliente() { }

        public Cliente(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public EstruturaCliente AtribuirDadosEstrutura(IRLib.Paralela.ClientObjects.EstruturaCliente cliente)
        {
            cliente.ID = Convert.ToInt32(cliente.ID);
            cliente.ContatoTipoID = cliente.ContatoTipoID;
            cliente.Nome = Convert.ToString(cliente.Nome) + "";
            cliente.CPF = Convert.ToString(cliente.CPF) + "";
            cliente.RG = Convert.ToString(cliente.RG) + "";
            cliente.Sexo = Convert.ToString(cliente.Sexo) + "";
            cliente.DataNascimentoTS = Convert.ToString(cliente.DataNascimentoTS) + "";
            cliente.TelefoneResidencialDDD = Convert.ToString(cliente.TelefoneResidencialDDD) + "";
            cliente.TelefoneResidencial = Convert.ToString(cliente.TelefoneResidencial) + "";
            cliente.TelefoneCelularDDD = Convert.ToString(cliente.TelefoneCelularDDD) + "";
            cliente.TelefoneCelular = Convert.ToString(cliente.TelefoneCelular) + "";
            cliente.TelefoneComercialDDD = Convert.ToString(cliente.TelefoneComercialDDD) + "";
            cliente.TelefoneComercial = Convert.ToString(cliente.TelefoneComercial) + "";
            cliente.Email = Convert.ToString(cliente.Email) + "";
            cliente.Senha = Convert.ToString(cliente.Senha) + "";
            cliente.SenhaConfirmacao = Convert.ToString(cliente.SenhaConfirmacao) + "";
            cliente.ReceberEmail = Convert.ToString(cliente.ReceberEmail) + "";
            cliente.CEPCliente = Convert.ToString(cliente.CEPCliente) + "";
            cliente.EnderecoCliente = Convert.ToString(cliente.EnderecoCliente) + "";
            cliente.EnderecoNumeroCliente = Convert.ToString(cliente.EnderecoNumeroCliente) + "";
            cliente.EnderecoComplementoCliente = Convert.ToString(cliente.EnderecoComplementoCliente) + "";
            cliente.BairroCliente = Convert.ToString(cliente.BairroCliente) + "";
            cliente.CidadeCliente = Convert.ToString(cliente.CidadeCliente) + "";
            cliente.EstadoCliente = Convert.ToString(cliente.EstadoCliente) + "";
            cliente.CPFResponsavel = cliente.CPF ?? string.Empty;
            cliente.Pais = string.IsNullOrEmpty(cliente.Pais) ? "Brasil" : cliente.Pais;
            return cliente;
        }

        public int ClienteManutencao(IRLib.Paralela.ClientObjects.EstruturaCliente cliente)
        {
            try
            {
                StringBuilder sql = new StringBuilder("INSERT INTO tManutencaoSite VALUES('@001', '@002', '@003', '@004')");
                sql.Replace("@001", cliente.Nome);
                sql.Replace("@002", cliente.Email);
                sql.Replace("@003", cliente.TelefoneCelular);
                sql.Replace("@004", DateTime.Now.ToString());
                int r = bd.Executar(sql);
                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> Atualizar(IRLib.Paralela.ClientObjects.EstruturaCliente cliente, bool tudoObrigatorio)
        {
            if (tudoObrigatorio)
                return Atualizar(cliente);
            else
                return AtualizarSemObrigatorios(cliente);
        }

        /// <summary>
        /// Valida e Atualiza os dados do cliente.
        /// </summary>
        /// <returns>Estrutura de Cliente</returns>	
        public List<string> Atualizar(IRLib.Paralela.ClientObjects.EstruturaCliente cliente)
        {
            int usuarioSite = UsuarioSiteID;
            bool blnAtualizar = true;
            List<string> oMensagens = new List<string>();
            IRLib.Paralela.ClientObjects.EstruturaRetornoProcSalvarCliente retornoProc = new EstruturaRetornoProcSalvarCliente();
            // Conversão de Valores, para não ter nenhum valor Null.
            cliente = AtribuirDadosEstrutura(cliente);

            // Nome
            if (cliente.Nome.Trim() == "")
            {
                oMensagens.Add("O nome é obrigatório.");
                blnAtualizar = false;
            }
            else
            {
                if (cliente.Nome.Trim().Length < 6 || cliente.Nome.Trim().IndexOf(' ') < 0)
                {
                    oMensagens.Add("O nome deve conter pelo menos seis caracteres e um espaço.");
                    blnAtualizar = false;
                }
            }
            if (cliente.TipoCadastro == Convert.ToChar(Cliente.TipoCadastroCliente.PessoaJuridica))
            {
                if (cliente.CNPJ != null)
                {
                    if (cliente.CNPJ.Trim() == "")
                    {
                        oMensagens.Add(cliente.Pais == "Brasil" ? "O CNPJ é obrigatório." : "O Document ID é obrigatório");
                        blnAtualizar = false;
                    }
                    else
                    {
                        if (cliente.Pais == "Brasil" && !Utilitario.IsCNPJ(cliente.CNPJ.Trim()))
                        {
                            oMensagens.Add("O CNPJ deve estar no formato válido.");
                            blnAtualizar = false;
                        }
                    }
                }
            }

            // CPF
            if (cliente.CPF.Trim() == "")
            {
                oMensagens.Add(cliente.Pais == "Brasil" ? "O CPF é obrigatório." : "O Document ID é obrigatório");
                blnAtualizar = false;
            }
            else
            {
                if (cliente.Pais == "Brasil" && !Utilitario.IsCPF(cliente.CPF.Trim()))
                {
                    oMensagens.Add("O CPF deve estar no formato válido.");
                    blnAtualizar = false;
                }
                else if (cliente.Pais != "Brasil" && cliente.CPF.Trim().Length < 4)
                {
                    oMensagens.Add("O Document ID deve conter no mínimo 4 números.");
                    blnAtualizar = false;
                }
            }

            // RG
            if (cliente.RG.Trim() != "")
            {
                if (cliente.RG.Trim().Length < 5 || (!Utilitario.IsLetrasNumeros(cliente.RG.Trim())))
                {
                    oMensagens.Add("O rg deve conter pelo menos cinco caracteres (números ou letras).");
                    blnAtualizar = false;
                }
            }

            if (cliente.ContatoTipoID <= 0)
            {
                oMensagens.Add("Um tipo de contato deve ser selecionado");
                blnAtualizar = false;
            }

            // Data de Nascimento
            if (cliente.DataNascimentoTS.Trim() == "" || cliente.DataNascimento == null)
            {
                oMensagens.Add("A data de nascimento é obrigatória.");
                blnAtualizar = false;
            }
            else
            {
                if (cliente.DataNascimentoTS.Trim() != "" && (!Utilitario.IsDateTime(cliente.DataNascimentoTS.Trim(), "yyyy/MM/dd")))
                {
                    oMensagens.Add("A data de nascimento deve estar no formato válido.");
                    blnAtualizar = false;
                }
                if (Convert.ToDateTime(cliente.DataNascimentoTS) < DateTime.Now.AddYears(-99))
                {
                    oMensagens.Add("Cliente com mais de 99 anos. Impossível continuar.");
                    blnAtualizar = false;
                }

            }

            // Telefones
            if (cliente.TelefoneResidencialDDD.Trim() == "" && cliente.TelefoneCelularDDD.Trim() == "" && cliente.TelefoneComercialDDD.Trim() == "")
            {
                oMensagens.Add("É necessário preencher pelo menos um telefone.");
                blnAtualizar = false;
            }
            else
            {
                bool residencial = cliente.TelefoneResidencialDDD.Trim().Length > 0 || cliente.TelefoneResidencial.Trim().Length > 0;
                bool celular = cliente.TelefoneCelularDDD.Trim().Length > 0 || cliente.TelefoneCelular.Trim().Length > 0;
                bool comercial = cliente.TelefoneComercialDDD.Trim().Length > 0 || cliente.TelefoneComercial.Trim().Length > 0;

                // Telefone Residencial
                if (residencial && (cliente.TelefoneResidencialDDD.Length != 2 || cliente.TelefoneResidencial.Trim().Length != 8))
                {
                    oMensagens.Add("O telefone residencial deve ter 2 dígitos para o DDD e 8 para o telefone.");
                    blnAtualizar = false;
                }
                else
                {
                    if (residencial && (!Utilitario.ehInteiro(cliente.TelefoneResidencialDDD.Trim()) || !Utilitario.ehInteiro(cliente.TelefoneResidencial.Trim())))
                    {
                        oMensagens.Add("O telefone residencial deve ser preenchido apenas com números.");
                        blnAtualizar = false;
                    }
                }

                // Telefone Celular

                if (celular && (cliente.TelefoneCelularDDD.Length != 2 || cliente.TelefoneCelular.Trim().Length < 8))
                {
                    oMensagens.Add("O telefone celular deve ter 2 dígitos para o DDD e 8/9 para o telefone.");
                    blnAtualizar = false;
                }
                else
                {
                    if (celular && (!Utilitario.ehInteiro(cliente.TelefoneCelularDDD.Trim()) || !Utilitario.ehInteiro(cliente.TelefoneCelular.Trim())))
                    {
                        oMensagens.Add("O telefone celular deve ser preenchido apenas com números.");
                        blnAtualizar = false;
                    }
                }


                // Telefone Comercial
                if (comercial && (cliente.TelefoneComercialDDD.Length != 2 || cliente.TelefoneComercial.Trim().Length != 8))
                {
                    oMensagens.Add("O telefone comercial deve ter 2 dígitos para o DDD e 8 para o telefone.");
                    blnAtualizar = false;
                }
                else
                {
                    if (comercial && (!Utilitario.ehInteiro(cliente.TelefoneComercialDDD.Trim()) || !Utilitario.ehInteiro(cliente.TelefoneComercial.Trim())))
                    {
                        oMensagens.Add("O telefone comercial deve ser preenchido apenas com números.");
                        blnAtualizar = false;
                    }
                }
            }

            // E-mail
            if (cliente.Email.Trim() == "")
            {
                oMensagens.Add("O e-mail é obrigatório.");
                blnAtualizar = false;
            }
            else
            {
                if (!Utilitario.IsEmail(cliente.Email.Trim()))
                {
                    oMensagens.Add("O e-mail deve estar no formato válido.");
                    blnAtualizar = false;
                }
            }

            // Senha
            if (!cliente.CanaisEspeciais)
            {
                if (cliente.Senha.Trim() == "" && (cliente.ID == 0 || cliente.PrecisaSenha))
                {
                    oMensagens.Add("A senha é obrigatória.");
                    blnAtualizar = false;
                }
                else
                {
                    if ((cliente.ID == 0 && cliente.Senha.Trim().Length < 6) || (cliente.ID != 0 && cliente.Senha.Trim().Length > 0 && cliente.Senha.Trim().Length < 6))
                    {
                        oMensagens.Add("A senha deve conter pelo menos 6 caracteres.");
                        blnAtualizar = false;
                    }
                    else
                    {
                        if ((cliente.ID == 0 && (!Utilitario.IsLetrasNumeros(cliente.Senha.Trim())) || (cliente.ID != 0 && cliente.Senha.Trim().Length > 0 && (!Utilitario.IsLetrasNumeros(cliente.Senha.Trim())))))
                        {
                            oMensagens.Add("A senha deve conter apenas letras e números.");
                            blnAtualizar = false;
                        }
                    }
                }

                // Senha Confirmação
                if (cliente.Senha.Trim() != cliente.SenhaConfirmacao.Trim())
                {
                    oMensagens.Add("Confirmação de senha é inválida.");
                    blnAtualizar = false;
                }
            }


            // Gravar Dados
            if (blnAtualizar)
            {
                this.Ler(cliente.ID);
                this.Nome.Valor = cliente.Nome;
                this.CPF.Valor = cliente.CPF;
                this.RG.Valor = cliente.RG;
                this.Sexo.Valor = cliente.Sexo;
                this.ContatoTipoID.Valor = cliente.ContatoTipoID;

                if (cliente.DataNascimentoTS.Trim() != "")
                    cliente.DataNascimento = Convert.ToDateTime(cliente.DataNascimentoTS);

                this.DataNascimento.Valor = cliente.DataNascimento;
                this.DDDTelefone.Valor = cliente.TelefoneResidencialDDD;
                this.Telefone.Valor = cliente.TelefoneResidencial;
                this.DDDCelular.Valor = cliente.TelefoneCelularDDD;
                this.Celular.Valor = cliente.TelefoneCelular;
                this.DDDTelefoneComercial.Valor = cliente.TelefoneComercialDDD;
                this.TelefoneComercial.Valor = cliente.TelefoneComercial;
                this.Email.Valor = cliente.Email;

                if (cliente.Senha != "")
                    this.Senha.Valor = cliente.Senha;

                this.RecebeEmail.Valor = (cliente.ReceberEmail == "T");
                this.EnderecoCliente.Valor = cliente.EnderecoCliente;
                this.CEPCliente.Valor = cliente.CEPCliente;
                this.NumeroCliente.Valor = cliente.EnderecoNumeroCliente;
                this.ComplementoCliente.Valor = cliente.EnderecoComplementoCliente;
                this.BairroCliente.Valor = cliente.BairroCliente;
                this.CidadeCliente.Valor = cliente.CidadeCliente;
                this.EstadoCliente.Valor = cliente.EstadoCliente;
                this.Pais.Valor = cliente.Pais;
                this.CPFResponsavel.Valor = cliente.CPFResponsavel;
                this.CNPJ.Valor = cliente.CNPJ;
                this.InscricaoEstadual.Valor = cliente.InscricaoEstadual;
                this.NomeFantasia.Valor = cliente.NomeFantasia;
                this.RazaoSocial.Valor = cliente.RazaoSocial;

                retornoProc = Salvar(usuarioSite);

                if (retornoProc.RetornoProcedure != RetornoProcSalvar.UpdateOK && retornoProc.RetornoProcedure != RetornoProcSalvar.InsertOK)
                {
                    Type enumType = typeof(RetornoProcSalvar);
                    DescriptionAttribute[] da = (DescriptionAttribute[])(enumType.GetField(retornoProc.RetornoProcedure.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
                    oMensagens.Add(da[0].Description);
                }
            }

            return oMensagens;
        }

        /// <summary>
        /// Valida e Atualiza os dados do cliente.
        /// </summary>
        /// <returns>Estrutura de Cliente</returns>	
        public List<string> AtualizarSemObrigatorios(IRLib.Paralela.ClientObjects.EstruturaCliente cliente)
        {
            int usuarioSite = UsuarioSiteID;
            bool blnAtualizar = true;
            List<string> oMensagens = new List<string>();
            IRLib.Paralela.ClientObjects.EstruturaRetornoProcSalvarCliente retornoProc = new EstruturaRetornoProcSalvarCliente();
            // Conversão de Valores, para não ter nenhum valor Null.
            cliente = AtribuirDadosEstrutura(cliente);
            oMensagens.AddRange(cliente.ValidarCampos());

            if (oMensagens.Count > 0)
                return oMensagens;

            // Nome
            if (string.IsNullOrEmpty(cliente.Nome) || cliente.Nome.Trim().Length > 0 && (cliente.Nome.Trim().Length < 6 || cliente.Nome.Trim().IndexOf(' ') < 0))
            {
                oMensagens.Add("O Nome deve conter pelo menos seis caracteres e um espaço.");
                blnAtualizar = false;
            }
            // CPF
            if (cliente.CPF.Trim().Length > 0)
            {
                if (cliente.Pais == "Brasil" && !Utilitario.IsCPF(cliente.CPF.Trim().Replace("-", "").Replace(".", "")))
                {
                    oMensagens.Add("O CPF deve estar no formato válido.");
                    blnAtualizar = false;
                }
                else if (cliente.Pais != "Brasil" && cliente.CPF.Trim().Length < 4)
                {
                    oMensagens.Add("O Document ID(CPF) deve conter no mínimo 4 dígitos.");
                    blnAtualizar = false;
                }

            }

            // RG
            if (cliente.RG.Trim().Length > 0)
            {
                if (cliente.RG.Trim().Length < 5 || (!Utilitario.IsLetrasNumeros(cliente.RG.Trim())))
                {
                    oMensagens.Add("O RG deve conter pelo menos cinco caracteres (números e/ou letras).");
                    blnAtualizar = false;
                }
            }

            // E-mail
            if (cliente.Email.Trim().Length > 0)
            {
                if (!Utilitario.IsEmail(cliente.Email.Trim()))
                {
                    oMensagens.Add("O E-mail deve estar no formato válido.");
                    blnAtualizar = false;
                }
            }

            // Gravar Dados
            if (blnAtualizar)
            {
                this.Ler(cliente.ID);
                this.Nome.Valor = cliente.Nome;
                this.CPF.Valor = cliente.CPF;
                this.RG.Valor = cliente.RG;
                this.Sexo.Valor = cliente.Sexo;
                this.ContatoTipoID.Valor = cliente.ContatoTipoID;

                if (cliente.DataNascimentoTS.Trim() != "")
                    cliente.DataNascimento = new DateTime(Convert.ToInt32(cliente.DataNascimentoTS.Trim().Substring(0, 4)), Convert.ToInt32(cliente.DataNascimentoTS.Trim().Substring(4, 2)), Convert.ToInt32(cliente.DataNascimentoTS.Trim().Substring(6, 2)));

                this.DataNascimento.Valor = cliente.DataNascimento;
                this.DDDTelefone.Valor = cliente.TelefoneResidencialDDD;
                this.Telefone.Valor = cliente.TelefoneResidencial;
                this.DDDCelular.Valor = cliente.TelefoneCelularDDD;
                this.Celular.Valor = cliente.TelefoneCelular;
                this.DDDTelefoneComercial.Valor = cliente.TelefoneComercialDDD;
                this.TelefoneComercial.Valor = cliente.TelefoneComercial;
                this.Email.Valor = cliente.Email;

                if (cliente.Senha != "")
                    this.Senha.Valor = cliente.Senha;

                this.RecebeEmail.Valor = (cliente.ReceberEmail == "T");
                this.EnderecoCliente.Valor = cliente.EnderecoCliente;
                this.CEPCliente.Valor = cliente.CEPCliente;
                this.NumeroCliente.Valor = cliente.EnderecoNumeroCliente;
                this.ComplementoCliente.Valor = cliente.EnderecoComplementoCliente;
                this.BairroCliente.Valor = cliente.BairroCliente;
                this.CidadeCliente.Valor = cliente.CidadeCliente;
                this.EstadoCliente.Valor = cliente.EstadoCliente;
                this.EnderecoEntrega.Valor = cliente.EnderecoEntrega;
                this.CEPEntrega.Valor = cliente.CEPEntrega;
                this.NumeroEntrega.Valor = cliente.EnderecoNumeroEntrega;
                this.ComplementoEntrega.Valor = cliente.EnderecoComplementoEntrega;
                this.BairroEntrega.Valor = cliente.BairroEntrega;
                this.CidadeEntrega.Valor = cliente.CidadeEntrega;
                this.EstadoEntrega.Valor = cliente.EstadoEntrega;
                this.Pais.Valor = cliente.Pais;
                this.CPFResponsavel.Valor = cliente.CPFResponsavel;
                retornoProc = Salvar(usuarioSite);

                if (retornoProc.RetornoProcedure != RetornoProcSalvar.UpdateOK && retornoProc.RetornoProcedure != RetornoProcSalvar.InsertOK)
                {
                    Type enumType = typeof(RetornoProcSalvar);
                    DescriptionAttribute[] da = (DescriptionAttribute[])(enumType.GetField(retornoProc.RetornoProcedure.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
                    oMensagens.Add(da[0].Description);
                }
            }

            return oMensagens;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public List<string> AtualizarEntrega(IRLib.Paralela.ClientObjects.EstruturaCliente cliente)
        {
            int usuarioSite = UsuarioSiteID;
            bool blnAtualizar = true;
            List<string> oMensagens = new List<string>();
            IRLib.Paralela.ClientObjects.EstruturaRetornoProcSalvarCliente retornoProc = new EstruturaRetornoProcSalvarCliente();
            // Conversão de Valores, para não ter nenhum valor Null.
            cliente.ID = Convert.ToInt32(cliente.ID);
            cliente.NomeEntrega = Convert.ToString(cliente.NomeEntrega) + "";
            cliente.CpfEntrega = Convert.ToString(cliente.CpfEntrega) + "";
            cliente.RgEntrega = Convert.ToString(cliente.RgEntrega) + "";
            cliente.CEPEntrega = Convert.ToString(cliente.CEPEntrega) + "";
            cliente.EnderecoEntrega = Convert.ToString(cliente.EnderecoEntrega) + "";
            cliente.EnderecoNumeroEntrega = Convert.ToString(cliente.EnderecoNumeroEntrega) + "";
            cliente.EnderecoComplementoEntrega = Convert.ToString(cliente.EnderecoComplementoEntrega) + "";
            cliente.BairroEntrega = Convert.ToString(cliente.BairroEntrega) + "";
            cliente.CidadeEntrega = Convert.ToString(cliente.CidadeEntrega) + "";
            cliente.EstadoEntrega = Convert.ToString(cliente.EstadoEntrega) + "";

            // Gravar Dados
            if (blnAtualizar)
            {
                this.Ler(cliente.ID);
                this.NomeEntrega.Valor = cliente.NomeEntrega;
                this.CPFEntrega.Valor = cliente.CpfEntrega;
                this.RGEntrega.Valor = cliente.RgEntrega;
                this.CEPEntrega.Valor = cliente.CEPEntrega;
                this.EnderecoEntrega.Valor = cliente.EnderecoEntrega;
                this.NumeroEntrega.Valor = cliente.EnderecoNumeroEntrega;
                this.ComplementoEntrega.Valor = cliente.EnderecoComplementoEntrega;
                this.BairroEntrega.Valor = cliente.BairroEntrega;
                this.CidadeEntrega.Valor = cliente.CidadeEntrega;
                this.EstadoEntrega.Valor = cliente.EstadoEntrega;

                retornoProc = SalvarEntrega(usuarioSite);

                if (retornoProc.RetornoProcedure != RetornoProcSalvar.UpdateOK)
                {
                    Type enumType = typeof(RetornoProcSalvar);
                    DescriptionAttribute[] da = (DescriptionAttribute[])(enumType.GetField(retornoProc.RetornoProcedure.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
                    oMensagens.Add(da[0].Description);
                }
            }
            return oMensagens;
        }

        public int[] BuscarClienteEmailSenhaWeb(string email, string senha)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "(Email = '" + email.Replace("'", "''") + "')";
                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0)//Encontrou o cliente
                {
                    clienteLista.Primeiro();

                    if (clienteLista.Cliente.StatusAtual.Valor.ToUpper() == "B")
                    {
                        // Cliente Bloqueado
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteBloqueado, clienteLista.Cliente.Control.ID);
                    }

                    if (clienteLista.Cliente.Senha.Valor == "" || clienteLista.Cliente.Senha.Valor == null)
                    {
                        // Senha Vazia
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteSemSenha, clienteLista.Cliente.Control.ID);
                    }

                    if (clienteLista.Cliente.Senha.Valor == senha)
                    {
                        // Senha Correta
                        return MontaRetornoWeb((int)Cliente.Infos.Sucesso, clienteLista.Cliente.Control.ID);
                    }
                    else
                    {
                        if (!clienteLista.Cliente.Ativo.Valor)
                            //Usuario não ativou cadastro
                            return MontaRetornoWeb((int)Cliente.Infos.NaoAtivado, clienteLista.Cliente.Control.ID);
                        else
                            //senha inválida
                            return MontaRetornoWeb((int)Cliente.Infos.InfoIncorreta, clienteLista.Cliente.Control.ID);
                    }
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, clienteLista.Cliente.Control.ID);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, clienteLista.Cliente.Control.ID);
            }
        }

        private int[] MontaRetornoWeb(int msgCodigo, int ID)
        {
            return new int[] { msgCodigo, ID };
        }

        public int[] BuscarClientePorEmailWeb(string email)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "Email = '" + email.Replace("'", "''") + "'";

                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0)//Encontrou o cliente
                {
                    clienteLista.Primeiro();

                    if (clienteLista.Cliente.Email.Valor == email)
                        return MontaRetornoWeb((int)Cliente.Infos.Sucesso, clienteLista.Cliente.Control.ID);
                    else
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);

            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, clienteLista.Cliente.Control.ID);
            }
            catch
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, clienteLista.Cliente.Control.ID);
            }
        }

        public int[] BuscarClienteEmailCPFWeb(string email, string CPF)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "(Email = '" + email.Replace("'", "''") + "' AND CPF = '" + CPF.Replace("'", "''") + "') AND (Senha = '' OR Senha IS NULL) ";

                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0)//Encontrou o cliente
                {
                    clienteLista.Primeiro();

                    if (clienteLista.Cliente.Email.Valor == email && clienteLista.Cliente.CPF.Valor == CPF && clienteLista.Cliente.Senha.Valor == "")
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteExistente, clienteLista.Cliente.Control.ID);
                    else
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, clienteLista.Cliente.Control.ID);
            }
            catch
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, clienteLista.Cliente.Control.ID);
            }
        }

        public int[] BuscarClienteCPF(string CPF)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "(CPF ='" + CPF.Replace("'", "") + "' AND LEN(Email) > 0)";
                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0)
                {
                    clienteLista.Primeiro();
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteExistente, clienteLista.Cliente.Control.ID);
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] BuscarClienteCPFMobile(string CPF)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "(CPF ='" + CPF.Replace("'", "") + "' AND LEN(Email) > 0)";
                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0 && !string.IsNullOrEmpty(CPF))
                {
                    clienteLista.Primeiro();
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteExistente, clienteLista.Cliente.Control.ID);
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] BuscarClienteEmailMobile(string Email)
        {
            ClienteLista clienteLista = new ClienteLista();
            try
            {
                clienteLista.FiltroSQL = "(Email ='" + Email.Replace("'", "") + "' AND LEN(Email) > 0)";
                clienteLista.TamanhoMax = 1;
                clienteLista.Carregar();

                if (clienteLista.Tamanho > 0)
                {
                    clienteLista.Primeiro();
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteExistente, clienteLista.Cliente.Control.ID);
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, clienteLista.Cliente.Control.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] BuscaClienteEmailSenhaWebReduzido(string email, string senha)
        {
            try
            {
                string consulta = string.Format(@"SELECT ID, Senha, StatusAtual, Ativo FROM tCliente WHERE CPF = '{0}' OR Email = '{1}'", email, email);

                int ID = 0;
                string STATUS = string.Empty;
                string SENHA = string.Empty;
                bool ATIVO = false;

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                {
                    ID = bd.LerInt("ID");
                    STATUS = bd.LerString("StatusAtual");
                    SENHA = bd.LerString("Senha");
                    ATIVO = bd.LerBoolean("Ativo");
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, 0);

                if (STATUS == "B")
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteBloqueado, ID);
                if (string.IsNullOrEmpty(SENHA))
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteSemSenha, ID);
                if (!ATIVO)
                    return MontaRetornoWeb((int)Cliente.Infos.NaoAtivado, ID);

                if (SENHA == senha)
                    return MontaRetornoWeb((int)Cliente.Infos.Sucesso, ID);
                else
                    return MontaRetornoWeb((int)Cliente.Infos.InfoIncorreta, ID);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, 0);
            }
        }

        public int[] BuscaClienteEmailCPFWebReduzido(string cpf, string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ID FROM tCliente WHERE CPF = '{0}' OR Email = '{1}'", cpf, email);

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                    return MontaRetornoWeb((int)Cliente.Infos.Sucesso, bd.LerInt("ID"));
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, 0);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, 0);
            }
        }

        public EstruturaIDNome BuscaClienteEmailCPFWebReduzidoEstrutura(string cpf, string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ID, Nome FROM tCliente WHERE CPF = '{0}' OR Email = '{1}'", cpf, email);

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                {
                    return new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome")
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] BuscaClienteEmailCPFWebReduzido(string email)
        {
            try
            {
                string consulta = string.Format(@"SELECT ID FROM tCliente WHERE Email = '{0}'", email);

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                    return MontaRetornoWeb((int)Cliente.Infos.Sucesso, bd.LerInt("ID"));
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, 0);
            }
            catch (ClienteException ex)
            {
                return MontaRetornoWeb((int)ex.CodigoErro, 0);
            }
            catch (Exception)
            {
                return MontaRetornoWeb((int)Cliente.Infos.ErroIndefinido, 0);
            }
        }

        /// <summary>
        /// Essa função foi descontinuada. Somente o site utiliza ela. a nova funcação inclui Atualizar e Inserir no metodo Salvar()
        /// Atualiza Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {
            int usuarioSite = UsuarioSiteID;
            EstruturaRetornoProcSalvarCliente ret = Salvar(usuarioSite);

            if (ret.RetornoProcedure == RetornoProcSalvar.UpdateOK)
                return true;
            else
                return false;
        }

        public bool ValidacaoACSP(EstruturaClienteVendas clienteVendas, bool IsEntrega)
        {
            bool retorno = false;

            this.Control.ID = clienteVendas.Cliente.ID;
            this.StatusAtual.Valor = clienteVendas.Cliente.Status;
            this.Nome.Valor = clienteVendas.Cliente.Nome;
            this.RG.Valor = clienteVendas.Cliente.RG;
            this.CPF.Valor = clienteVendas.Cliente.CPF;
            this.CarteiraEstudante.Valor = clienteVendas.Cliente.CarteiraEstudante;
            this.Sexo.Valor = clienteVendas.Cliente.Sexo;
            this.DDDTelefone.Valor = clienteVendas.Cliente.TelefoneResidencialDDD;
            this.Telefone.Valor = clienteVendas.Cliente.TelefoneResidencial;
            this.DDDTelefoneComercial.Valor = clienteVendas.Cliente.TelefoneComercialDDD;
            this.TelefoneComercial.Valor = clienteVendas.Cliente.TelefoneComercial;
            this.DDDCelular.Valor = clienteVendas.Cliente.TelefoneCelularDDD;
            this.Celular.Valor = clienteVendas.Cliente.TelefoneCelular;
            this.DataNascimento.Valor = clienteVendas.Cliente.DataNascimento;
            this.Email.Valor = clienteVendas.Cliente.Email;
            this.RecebeEmail.Valor = (clienteVendas.Cliente.ReceberEmail == "T");
            this.CEPEntrega.Valor = clienteVendas.Cliente.CEPEntrega;
            this.EnderecoEntrega.Valor = clienteVendas.Cliente.EnderecoEntrega;
            this.NumeroEntrega.Valor = clienteVendas.Cliente.EnderecoNumeroEntrega;
            this.ComplementoEntrega.Valor = clienteVendas.Cliente.EnderecoComplementoEntrega;
            this.BairroEntrega.Valor = clienteVendas.Cliente.BairroEntrega;
            this.CidadeEntrega.Valor = clienteVendas.Cliente.CidadeEntrega;
            this.EstadoEntrega.Valor = clienteVendas.Cliente.EstadoEntrega;
            this.CEPCliente.Valor = clienteVendas.Cliente.CEPCliente;
            this.EnderecoCliente.Valor = clienteVendas.Cliente.EnderecoCliente;
            this.NumeroCliente.Valor = clienteVendas.Cliente.EnderecoNumeroCliente;
            this.ComplementoCliente.Valor = clienteVendas.Cliente.EnderecoComplementoCliente;
            this.BairroCliente.Valor = clienteVendas.Cliente.BairroCliente;
            this.CidadeCliente.Valor = clienteVendas.Cliente.CidadeCliente;
            this.EstadoCliente.Valor = clienteVendas.Cliente.EstadoCliente;
            this.Obs.Valor = clienteVendas.Cliente.Observacao;
            this.ClienteIndicacaoID.Valor = clienteVendas.Cliente.ClienteIndicacaoID;
            this.Senha.Valor = clienteVendas.Cliente.Senha;
            this.Ativo.Valor = clienteVendas.Cliente.Ativo;
            this.NomeEntrega.Valor = clienteVendas.Cliente.NomeEntrega;
            this.CPFEntrega.Valor = clienteVendas.Cliente.CpfEntrega;
            this.RGEntrega.Valor = clienteVendas.Cliente.RgEntrega;

            if (!IsEntrega)
            {
                //if (this.CadastroValido())
                //    retorno = true;
                if (this.CadastroValido().TipoRetorno == IRLib.Paralela.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro)
                    return false;

            }
            else
            {
                //if (this.CadastroEntregaValido())
                //    retorno = true;
                if (this.CadastroEntregaValido().TipoRetorno == IRLib.Paralela.Codigo.Brainiac.Enumeradores.EnumTipoRetorno.ImplicarErro)
                    return false;

            }
            return retorno;
        }

        /// <summary>
        /// Método para inserir ou atualizar o cliente.
        /// Retorna um objeto RetornoSalvar do banco com o erro ou sucesso especifico.
        /// kim
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        public EstruturaRetornoProcSalvarCliente Salvar(int usuarioID)
        {
            return Salvar(usuarioID, false);
        }

        /// <summary>
        /// Método para inserir ou atualizar o cliente, ignorando a regra de duplicidade
        /// Retorna um objeto RetornoSalvar do banco com o erro ou sucesso especifico.
        /// kim
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        public EstruturaRetornoProcSalvarCliente SalvarDuplicidade(EstruturaClienteVendas clienteVendas, int usuarioID)
        {
            this.Control.ID = clienteVendas.Cliente.ID;
            this.StatusAtual.Valor = clienteVendas.Cliente.Status;
            this.Nome.Valor = clienteVendas.Cliente.Nome;
            this.RG.Valor = clienteVendas.Cliente.RG;
            this.CPF.Valor = clienteVendas.Cliente.CPF;
            this.CarteiraEstudante.Valor = clienteVendas.Cliente.CarteiraEstudante;
            this.Sexo.Valor = clienteVendas.Cliente.Sexo;
            this.DDDTelefone.Valor = clienteVendas.Cliente.TelefoneResidencialDDD;
            this.Telefone.Valor = clienteVendas.Cliente.TelefoneResidencial;
            this.DDDTelefoneComercial.Valor = clienteVendas.Cliente.TelefoneComercialDDD;
            this.TelefoneComercial.Valor = clienteVendas.Cliente.TelefoneComercial;
            this.DDDCelular.Valor = clienteVendas.Cliente.TelefoneCelularDDD;
            this.Celular.Valor = clienteVendas.Cliente.TelefoneCelular;
            this.DataNascimento.Valor = clienteVendas.Cliente.DataNascimento;
            this.Email.Valor = clienteVendas.Cliente.Email;
            this.RecebeEmail.Valor = (clienteVendas.Cliente.ReceberEmail == "T");
            this.CEPEntrega.Valor = clienteVendas.Cliente.CEPEntrega;
            this.EnderecoEntrega.Valor = clienteVendas.Cliente.EnderecoEntrega;
            this.NumeroEntrega.Valor = clienteVendas.Cliente.EnderecoNumeroEntrega;
            this.ComplementoEntrega.Valor = clienteVendas.Cliente.EnderecoComplementoEntrega;
            this.BairroEntrega.Valor = clienteVendas.Cliente.BairroEntrega;
            this.CidadeEntrega.Valor = clienteVendas.Cliente.CidadeEntrega;
            this.EstadoEntrega.Valor = clienteVendas.Cliente.EstadoEntrega;
            this.CEPCliente.Valor = clienteVendas.Cliente.CEPCliente;
            this.EnderecoCliente.Valor = clienteVendas.Cliente.EnderecoCliente;
            this.NumeroCliente.Valor = clienteVendas.Cliente.EnderecoNumeroCliente;
            this.ComplementoCliente.Valor = clienteVendas.Cliente.EnderecoComplementoCliente;
            this.BairroCliente.Valor = clienteVendas.Cliente.BairroCliente;
            this.CidadeCliente.Valor = clienteVendas.Cliente.CidadeCliente;
            this.EstadoCliente.Valor = clienteVendas.Cliente.EstadoCliente;
            this.Obs.Valor = clienteVendas.Cliente.Observacao;
            this.ClienteIndicacaoID.Valor = clienteVendas.Cliente.ClienteIndicacaoID;
            this.Senha.Valor = clienteVendas.Cliente.Senha;
            this.Ativo.Valor = clienteVendas.Cliente.Ativo;
            this.NomeEntrega.Valor = clienteVendas.Cliente.NomeEntrega;
            this.CPFEntrega.Valor = clienteVendas.Cliente.CpfEntrega;
            this.RGEntrega.Valor = clienteVendas.Cliente.RgEntrega;
            return Salvar(usuarioID, true);
        }

        /// <summary>
        /// Método para inserir ou atualizar o cliente, verificando se considera duplicidade ou não
        /// Retorna um objeto RetornoSalvar do banco com o erro ou sucesso especifico.
        /// kim
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        private EstruturaRetornoProcSalvarCliente Salvar(int usuarioID, bool permitirDuplicidade)
        {
            try
            {
                string sql = @"EXEC dbo.salvar_tCliente2ComContatoTipoID    
                             @ClienteID = " + this.Control.ID.ToString() + ", " +
                            "@UsuarioID = " + usuarioID + ", " +
                            "@TimeStamp = " + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ", " +
                            "@Nome = '" + this.Nome.ValorBD + "', " +
                            "@RG = '" + this.RG.ValorBD + "', " +
                            "@CPF = '" + this.CPF.ValorBD + "', " +
                            "@CarteiraEstudante = '" + this.CarteiraEstudante.ValorBD + "', " +
                            "@Sexo = '" + this.Sexo.ValorBD + "', " +
                            "@DDDTelefone = '" + this.DDDTelefone.ValorBD + "', " +
                            "@Telefone = '" + this.Telefone.ValorBD + "', " +
                            "@DDDTelefoneComercial = '" + this.DDDTelefoneComercial.ValorBD + "', " +
                            "@TelefoneComercial = '" + this.TelefoneComercial.ValorBD + "', " +
                            "@DDDCelular = '" + this.DDDCelular.ValorBD + "', " +
                            "@Celular = '" + this.Celular.ValorBD + "', " +
                            "@DataNascimento = '" + this.DataNascimento.ValorBD + "', " +
                            "@Email = '" + this.Email.ValorBD + "', " +
                            "@RecebeEmail = '" + this.RecebeEmail.ValorBD + "', " +
                            "@CEPEntrega = '" + this.CEPEntrega.ValorBD + "', " +
                            "@EnderecoEntrega = '" + this.EnderecoEntrega.ValorBD + "', " +
                            "@NumeroEntrega = '" + this.NumeroEntrega.ValorBD + "', " +
                            "@CidadeEntrega = '" + this.CidadeEntrega.ValorBD + "', " +
                            "@EstadoEntrega = '" + this.EstadoEntrega.ValorBD + "', " +
                            "@CEPCliente = '" + this.CEPCliente.ValorBD + "', " +
                            "@EnderecoCliente = '" + this.EnderecoCliente.ValorBD + "', " +
                            "@NumeroCliente = '" + this.NumeroCliente.ValorBD + "', " +
                            "@CidadeCliente = '" + this.CidadeCliente.ValorBD + "', " +
                            "@EstadoCliente = '" + this.EstadoCliente.ValorBD + "', " +
                            "@ClienteIndicacaoID = " + this.ClienteIndicacaoID.ValorBD + ", " +
                            "@Obs = '" + this.Obs.ValorBD + "', " +
                            "@ComplementoEntrega = '" + this.ComplementoEntrega.ValorBD + "', " +
                            "@BairroEntrega = '" + this.BairroEntrega.ValorBD + "', " +
                            "@ComplementoCliente = '" + this.ComplementoCliente.ValorBD + "', " +
                            "@BairroCliente = '" + this.BairroCliente.ValorBD + "', " +
                            "@Senha = '" + this.Senha.ValorBD + "', " +
                            "@Ativo = 'T', " +
                            "@NomeEntrega = '" + this.NomeEntrega.ValorBD + "', " +
                            "@CPFEntrega = '" + this.CPFEntrega.ValorBD + "', " +
                            "@RGEntrega = '" + this.RGEntrega.ValorBD + "', " +
                            "@StatusAtual = '" + this.StatusAtual.ValorBD + "'," +
                            "@Pais = '" + this.Pais.ValorBD + "'," +
                            "@CPFResponsavel = '" + this.CPFResponsavel.ValorBD + "'," +
                            "@ContatoTipoID = " + this.ContatoTipoID.ValorBD + "," +
                            "@CNPJ = '" + this.CNPJ.ValorBD + "'," +
                            "@NomeFantasia = '" + this.NomeFantasia.ValorBD + "'," +
                            "@RazaoSocial = '" + this.RazaoSocial.ValorBD + "'," +
                            "@InscricaoEstadual = '" + this.InscricaoEstadual.ValorBD + "'," +
                            "@LoginOSESP = '" + this.LoginOsesp.ValorBD + "'," +
                            "@Profissao = '" + this.Profissao.ValorBD + "'," +
                            "@SituacaoProfissionalID = '" + this.SituacaoProfissionalID.ValorBD + "'," +
                            "@DDDTelefoneComercial2 = '" + this.DDDTelefoneComercial2.ValorBD + "'," +
                            "@TelefoneComercial2 = '" + this.TelefoneComercial2.ValorBD + "'" +
                            ((permitirDuplicidade) ? ", @AtualizacaoDuplicidade = 1" : "");


                bd.Consulta(sql);
                EstruturaRetornoProcSalvarCliente retorno = new EstruturaRetornoProcSalvarCliente();
                if (bd.Consulta().Read())
                {
                    try
                    {
                        retorno.RetornoProcedure = (RetornoProcSalvar)bd.LerInt("Retorno");
                    }
                    catch { throw new Exception("Erro desconhecido ao inserir no banco"); }

                    retorno.ClienteID = bd.LerInt("ClienteID");
                }

                bd.Consulta().Close();

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método para inserir ou atualizar o cliente, verificando se considera duplicidade ou não
        /// Retorna um objeto RetornoSalvar do banco com o erro ou sucesso especifico.
        /// kim
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        private EstruturaRetornoProcSalvarCliente SalvarEntrega(int usuarioID)
        {
            try
            {
                string sql = @"EXEC dbo.salvar_entrega_tCliente 
                                 @ClienteID = " + this.Control.ID.ToString() + ", " +
                                "@UsuarioID = " + usuarioID + ", " +
                                "@TimeStamp = " + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ", " +
                                "@CEPEntrega = '" + this.CEPEntrega.ValorBD + "', " +
                                "@EnderecoEntrega = '" + this.EnderecoEntrega.ValorBD + "', " +
                                "@NumeroEntrega = '" + this.NumeroEntrega.ValorBD + "', " +
                                "@CidadeEntrega = '" + this.CidadeEntrega.ValorBD + "', " +
                                "@EstadoEntrega = '" + this.EstadoEntrega.ValorBD + "', " +
                                "@ComplementoEntrega = '" + this.ComplementoEntrega.ValorBD + "', " +
                                "@BairroEntrega = '" + this.BairroEntrega.ValorBD + "', " +
                                "@NomeEntrega = '" + this.NomeEntrega.ValorBD + "', " +
                                "@CPFEntrega = '" + this.CPFEntrega.ValorBD + "', " +
                                "@RGEntrega = '" + this.RGEntrega.ValorBD + "'";
                bd.Consulta(sql);
                EstruturaRetornoProcSalvarCliente retorno = new EstruturaRetornoProcSalvarCliente();
                if (bd.Consulta().Read())
                {
                    try
                    {
                        retorno.RetornoProcedure = (RetornoProcSalvar)bd.LerInt("Retorno");
                    }
                    catch { throw new Exception("Erro desconhecido ao inserir no banco"); }

                    retorno.ClienteID = bd.LerInt("ClienteID");
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// kim
        /// Essa função foi descontinuada. Somente o site utiliza ela. a nova funcação inclui Atualizar e Inserir no metodo Salvar()
        /// Inserir novo(a) Cliente
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            try
            {

                int usuarioSite = UsuarioSiteID;
                Salvar(usuarioSite);
                return true;//O método salvar joga exception caso não consiga inserir

                //ClienteLista clienteLista = new ClienteLista();
                //if (this.CPF.Valor != "" && this.Email.Valor != "")
                //    clienteLista.FiltroSQL = "(CPF='" + this.CPF + "' OR Email='" + this.Email + "')";
                //else if (this.CPF.Valor != "")
                //    clienteLista.FiltroSQL = "(CPF='" + this.CPF + "')";
                //else if (this.Email.Valor != "")
                //    clienteLista.FiltroSQL = "(Email='" + this.Email + "')";

                //if (clienteLista.FiltroSQL != null)
                //{
                //    clienteLista.TamanhoMax = 1;
                //    clienteLista.Carregar();

                //    if (clienteLista.Tamanho > 0)
                //    {
                //        throw new ClienteException("Cliente já existe.", Infos.ClienteExistente, clienteLista.ClienteID);
                //    }
                //    else
                //    {
                //        return base.Inserir();
                //    }
                //}
                //else
                //    return base.Inserir();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new ClienteException("Erro de acesso ao banco: " + ex.Message, Cliente.Infos.ErroDatabase);
            }
            catch (ClienteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClienteException("Erro não definido: " + ex.Message, Cliente.Infos.ErroIndefinido);
            }

        }

        /// <summary>
        /// Relatório de clientes baseado na apresentação ID
        /// Kim
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public DataTable RelatorioDeClientes(int apresentacaoID, List<string> lstCodigos, string SessionID)
        {
            try
            {
                DataTable retorno = new DataTable();
                retorno.Columns.Add("Setor", typeof(string));
                retorno.Columns.Add("Codigo", typeof(string));
                retorno.Columns.Add("Preco", typeof(string));
                retorno.Columns.Add("Valor", typeof(Decimal));
                retorno.Columns.Add("Senha", typeof(string));
                retorno.Columns.Add("Cortesia", typeof(string));
                retorno.Columns.Add("Canal", typeof(string));
                retorno.Columns.Add("Cliente", typeof(string));
                retorno.Columns.Add("CPF", typeof(string));
                retorno.Columns.Add("Email", typeof(string));
                retorno.Columns.Add("Telefone", typeof(string));
                retorno.Columns.Add("TelefoneComercial", typeof(string));
                retorno.Columns.Add("Celular", typeof(string));
                retorno.Columns.Add("CepEntrega", typeof(string));
                retorno.Columns.Add("EnderecoEntrega", typeof(string));
                retorno.Columns.Add("NumeroEntrega", typeof(string));
                retorno.Columns.Add("ComplementoEntrega", typeof(string));
                retorno.Columns.Add("BairroEntrega", typeof(string));
                retorno.Columns.Add("CidadeEntrega", typeof(string));
                retorno.Columns.Add("EstadoEntrega", typeof(string));
                retorno.Columns.Add("CodigoBarraPresente", typeof(string));

                DataTable dtCodigos = new DataTable("Codigos");
                dtCodigos.Columns.Add(new DataColumn("SessionID", typeof(string)));
                dtCodigos.Columns.Add(new DataColumn("Codigo", typeof(string)));

                DataRow linha;
                DataRow oDataRow;
                // Alimenta o DataTable com os Códigos
                foreach (string Codigo in lstCodigos)
                {
                    oDataRow = dtCodigos.NewRow();
                    oDataRow["Codigo"] = Codigo;
                    oDataRow["SessionID"] = SessionID;

                    dtCodigos.Rows.Add(oDataRow);
                }

                // Alimenta a Tabela Temporária
                System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy((System.Data.SqlClient.SqlConnection)bd.Cnn, System.Data.SqlClient.SqlBulkCopyOptions.TableLock | System.Data.SqlClient.SqlBulkCopyOptions.FireTriggers | System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = "RelatorioPresencaCodigos";
                bulkCopy.WriteToServer(dtCodigos);


                bd.Consulta(@"SELECT s.Nome As Setor, i.Codigo,p.Nome AS Preco,p.Valor,vb.Senha,ct.Nome AS Cortesia,ca.Nome AS Canal,rpc.Codigo AS CodigoBarraPresente,
                                c.Nome AS Cliente, c.CPF,c.Email,'('+ CAST(c.DDDTelefone AS NVARCHAR)+') '+ CAST(c.Telefone AS NVARCHAR) AS Telefone,('('+CAST(c.DDDTelefoneComercial AS NVARCHAR) +') ' + CAST(c.TelefoneComercial AS NVARCHAR)) AS TelefoneComercial,('('+ CAST(c.DDDCelular AS NVARCHAR) +') '+ CAST(c.Celular AS NVARCHAR)) AS Celular,
                                c.CepEntrega,c.EnderecoEntrega,c.NumeroEntrega,c.ComplementoEntrega,c.BairroEntrega,c.CidadeEntrega,c.EstadoEntrega
                                FROM tIngresso i(NOLOCK) 
                                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                                LEFT JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
                                LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = i.VendaBilheteriaID
                                LEFT JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
                                LEFT JOIN tLoja l (NOLOCK) ON l.ID = i.LojaID
                                LEFT JOIN tCanal ca (NOLOCK) ON ca.ID = l.CanalID
                                LEFT JOIN tCortesia ct (NOLOCK) ON ct.ID = i.CortesiaID
                                LEFT JOIN RelatorioPresencaCodigos rpc (NOLOCK) ON rpc.Codigo = i.CodigoBarra AND rpc.SessionID = '" + SessionID + "' " +
                                "WHERE i.ApresentacaoID = " + apresentacaoID +
                                " ORDER BY Setor, i.Codigo");
                while (bd.Consulta().Read())
                {
                    linha = retorno.NewRow();

                    linha["Setor"] = bd.LerString("Setor");
                    linha["Codigo"] = bd.LerString("Codigo");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Senha"] = bd.LerDecimal("Senha");
                    linha["Cortesia"] = bd.LerString("Cortesia");
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Cliente"] = bd.LerString("Cliente");
                    linha["CPF"] = bd.LerString("CPF");
                    linha["Email"] = bd.LerString("Email");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["TelefoneComercial"] = bd.LerString("TelefoneComercial");
                    linha["Celular"] = bd.LerString("Celular");
                    linha["CepEntrega"] = bd.LerString("CepEntrega");
                    linha["EnderecoEntrega"] = bd.LerString("EnderecoEntrega");
                    linha["NumeroEntrega"] = bd.LerString("NumeroEntrega");
                    linha["ComplementoEntrega"] = bd.LerString("ComplementoEntrega");
                    linha["BairroEntrega"] = bd.LerString("BairroEntrega");
                    linha["CidadeEntrega"] = bd.LerString("CidadeEntrega");
                    linha["EstadoEntrega"] = bd.LerString("EstadoEntrega");
                    linha["CodigoBarraPresente"] = bd.LerString("CodigoBarraPresente").Length > 0 ? "Sim" : "Não";
                    retorno.Rows.Add(linha);
                }
                return retorno;
            }
            catch
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Função utilizada para a importação de clientes OSESP a partir de uma string[]. Os dados devem vir de um arquivo CSV.
        /// Todas as verificações relacionadas a estrutuda do arquivo devem ser feitas antes da chamada da função.
        /// Cada string no vetor deve representar um cliente. Os dados são separados por ';' na seguinte ordem: Nome, CPF, Login e e-mail
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public int ImportarClientesOSESP(string[] dados)
        {
            int x = 0;

            try
            {
                int clienteID = 0;//guarda o resultado das pesquisas feitas no banco

                string clienteOsespNome;//nome do cliente OSESP
                string clienteOsespCPF;//CPF do cliente OSESP
                string clienteOsespLogin;//Login do cliente OSESP
                string clienteOsespEmail;//Email do cliente OSESP

                int contador = 0;//contador para saber quantos registros foram atualizados e criados
                int ok = 0; //execução de comando ok se for maior que 0
                string[] registro;
                for (int i = 1; i < dados.Length; i++)//começa com i = 1 pq o primeiro registro é o nome das colunas
                {
                    try
                    {
                        //if (x == 50)
                        //    x = x;
                        x++;
                        registro = dados[i].Split(';');

                        //popula as variaveis para melhor leitura.
                        clienteOsespNome = registro[0];
                        clienteOsespCPF = registro[1];
                        clienteOsespLogin = registro[2];
                        clienteOsespEmail = registro[3];

                        //Primeiro faz a busca do cliente no banco por Cpf, email e nome para evitar duplicidades de registros no BD
                        if (clienteOsespCPF != String.Empty)
                            clienteID = PesquisarClienteID("CPF = '" + clienteOsespCPF + "'");

                        if (clienteID > 0)//achou registro pelo CPF, atualiza o registro
                        {
                            ok = bd.Executar("UPDATE tCliente SET Nome = '" + clienteOsespNome.Replace("'", "''") + "', LoginOSESP = '" + clienteOsespLogin.Replace("'", "''") + "' , Email = '" + clienteOsespEmail + "' WHERE ID = " + clienteID);
                            if (ok > 0)
                                contador++;

                        }
                        else// não achou pelo CPF, deve procuprar pelo email 
                        {
                            if (clienteOsespEmail != String.Empty)
                                clienteID = PesquisarClienteID("Email = '" + clienteOsespEmail + "'");

                            if (clienteID > 0)//achou registro pelo email, atualiza o registro
                            {
                                ok = bd.Executar("UPDATE tCliente SET Nome = '" + clienteOsespNome + "', CPF = '" + clienteOsespCPF + "', LoginOSESP = '" + clienteOsespLogin + "'  WHERE ID = " + clienteID);
                                if (ok > 0)
                                    contador++;
                            }
                            else//não achou pelo email deve procurar pelo nome.
                            {
                                if (clienteOsespNome != String.Empty)
                                    clienteID = PesquisarClienteID("Nome = '" + clienteOsespNome + "'");

                                if (clienteID > 0)//achou pelo nome, atualiza o registro
                                {
                                    ok = bd.Executar("UPDATE tCliente SET CPF = '" + clienteOsespCPF + "', LoginOSESP = '" + clienteOsespLogin + "' , Email = '" + clienteOsespEmail + "' WHERE ID = " + clienteID);
                                    if (ok > 0)
                                        contador++;
                                }
                                else//cliente não encontrado. Deve-se inserir um novo registro.
                                {
                                    string sql;
                                    sql = "SELECT MAX(ID) AS ID FROM tCliente";
                                    object obj = bd.ConsultaValor(sql);
                                    int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                                    id++;

                                    sql = "INSERT INTO tCliente(ID, Nome, CPF, LoginOsesp, Email) " +
                                    "VALUES (" + id + ",'" + clienteOsespNome + "','" + clienteOsespCPF + "','" + clienteOsespLogin + "','" + clienteOsespEmail + "')";

                                    ok = bd.Executar(sql);
                                    if (ok > 0)
                                        contador++;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }
                return contador;

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizaStatusCadastro(string cpf, string status)
        {
            try
            {
                BD bd = new BD();
                bd.Executar("UPDATE tCliente SET StatusAtual = '" + status + "' WHERE CPF = '" + cpf + "'");
            }
            catch
            {
            }
        }

        public void SCPC_AtualizaStatusCPF(string cpf, string status)
        {
            try
            {
                BD bd = new BD();
                bd.Executar("UPDATE tCliente SET CPFConsultado = '" + status + "' WHERE CPF = '" + cpf + "'");
            }
            catch
            {
            }
        }

        public void SCPC_AtualizaNomeRetorno(string cpf, string SCPCNomeRetorno)
        {
            try
            {
                BD bd = new BD();
                bd.Executar("UPDATE tCliente SET SCPCNomeRetorno = '" + SCPCNomeRetorno + "' WHERE CPF = '" + cpf + "'");
            }
            catch
            {
            }
        }

        public string SCPC_CPFConsultadoBDStatus(string cpf)
        {
            BD bd = new BD();
            string retorno = "";
            try
            {
                if (cpf != string.Empty)
                {
                    bd.FecharConsulta();
                    bd.Consulta("SELECT CPFConsultado FROM tCliente (NOLOCK) WHERE CPF='" + cpf + "'");



                    if (bd.Consulta().Read())
                    {
                        retorno = bd.LerString("CPFConsultado");
                    }
                }
                return retorno;
            }
            catch
            {
                throw;
            }
        }

        public string SCPC_ConsultaNomeBD(string cpf)
        {
            BD bd = new BD();
            try
            {
                if (cpf != string.Empty)
                {
                    bd.FecharConsulta();
                    bd.Consulta("SELECT SCPCNomeRetorno FROM tCliente (NOLOCK) WHERE CPF='" + cpf + "'");
                }
                string retorno = "";

                if (bd.Consulta().Read())
                {
                    retorno = bd.LerString("SCPCNomeRetorno");
                }
                return retorno;
            }
            catch
            {
                throw;
            }
        }

        public bool CPFConsultadoStatus(string txtNome, string cpfBD, string dataNascimentoBD)
        {
            bool retorno = false;
            string sitCPF = String.Empty;
            bool SCPC_Consultado = false;

            //VERIFICA O STATUS DA COLUNA CPFConsultado da tCliente
            string CPFConsultadoBD = SCPC_CPFConsultadoBDStatus(cpfBD);

            //CPFConsultadoBD = "" NULL (Cliente não foi consultado no SCPC)
            if (CPFConsultadoBD.Trim() == String.Empty || CPFConsultadoBD.Trim() == "" || CPFConsultadoBD.Trim() == null)
            {
                //Faz Consulta no SCPC
                SCPC_Consultado = SCPC_ConsultaCPF(cpfBD, txtNome, dataNascimentoBD);
            }
            if (CPFConsultadoBD == "1" || CPFConsultadoBD == "0")
            {
                CPFConsultadoBD = SCPC_CPFConsultadoBDStatus(cpfBD);


                //Consulta OK
                if (CPFConsultadoBD == "1" && SCPC_ComparaNomes(cpfBD, txtNome))
                {
                    retorno = true;
                }
                //Consulta Nome NÃO encontrado
                else if (CPFConsultadoBD == "0")
                {
                    retorno = false;
                    if (SCPC_ComparaNomes(cpfBD, txtNome))
                    {
                        //Grava Status "1" na  tCliente CPFConsultado
                        try
                        {
                            SCPC_AtualizaStatusCPF(cpfBD, "1");
                            retorno = true;
                        }
                        catch { }
                        finally { }

                    }
                    else
                    {
                        retorno = false;
                        //Grava Status "0" na  tCliente CPFConsultado
                        try
                        {
                            SCPC_AtualizaStatusCPF(cpfBD, "0");
                            retorno = false;
                        }
                        catch { }
                        finally { }
                    }
                }
            }
            return retorno;
        }

        public bool SCPC_ComparaNomes(string cpf, string nome)
        {
            bool retorno = false;
            IRLib.Paralela.Cliente Cliente = new IRLib.Paralela.Cliente();
            string nomeBD = Cliente.SCPC_ConsultaNomeBD(cpf);

            if (nomeBD.ToUpper().Trim() == nome.ToUpper().Trim())
                retorno = true;
            else
                retorno = false;

            return retorno;
        }

        public bool SCPC_ConsultaCPF(string cpfBD, string nomeBD, string dataNascimentoBD)
        {
            bool retorno = false;

            IRLib.Paralela.Utilitario Utilitario = new Utilitario();

            #region Variaveis
            //Código e senha para acessar o sistema de TESTES
            string codigo_teste = ConfigurationManager.AppSettings["SCPC_codigo_teste"];
            string senha_teste = ConfigurationManager.AppSettings["SCPC_senha_teste"];

            //Código e senha para acessar o sistema de PRODUÇÃO
            string codigo_prod = ConfigurationManager.AppSettings["SCPC_codigo_prod"];
            string senha_prod = ConfigurationManager.AppSettings["SCPC_senha_prod"];

            //URL's para acessar o sistema de TESTES e PRODUÇÃO
            string url_teste = ConfigurationManager.AppSettings["SCPC_url_teste"];
            string url_prod = ConfigurationManager.AppSettings["SCPC_url_prod"];

            string transacao = ConfigurationManager.AppSettings["SCPC_transacao"];
            string versao = ConfigurationManager.AppSettings["SCPC_versao"];
            string indicadorFimTexto = ConfigurationManager.AppSettings["SCPC_indicadorFimTexto"];
            string tipoConsulta = ConfigurationManager.AppSettings["SCPC_tipoConsulta"];

            string _s5 = "     ";
            string _s10 = "          ";
            string urlConsulta = String.Empty;

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(Extensions.ValidateRemoteCertificate);

            /*Campos para a URL de COnsulta:
            * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
             * Coluna 1: Ordem
             * Coluna 2: Campo
             * Coluna 3: Posição Inicial
             * Coluna 4: Posição Final
             * Coluna 5: Tam. Byte
             * Coluna 6: Formato - Tipo
             * Coluna 7: Formato - Casas Decimais
             * Coluna 8: Conteúdo
            * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
            |C1||C2                       | |C3|    |C4|    |C5|    |C6|   |C7| |C8|
            01  TRANSAÇÃO                   001     008     008     X           “CSR50001”
            02  VERSÃO                      009     010     002     X           “01” 
            03  RESERVADO SOLICITANTE       011     020     010     X           USO DO SOLICITANTE                                                 (Informar somente letras                                                maiúsculas, sem acentuação ou                                                números)
            04  RESERVADO ACSP              021     040     020     X           USO DA ACSP
            05  CÓDIGO                      041     048     008     N       0   CÓDIGO DE SERVIÇO
            06  SENHA                       049     056     008     X           SENHA DE ACESSO
            07  CONSULTA                    057     064     008     X           “CONF” – Conferencia de Nomes                                                     “SCAD” – Sintese P/ Documento                                                    “FONE” – Sintese P/ Nome + Data
            08  CPF                         065     075     011     N       0   NÚMERO DO CPF COMPLETO
            09  NOME                        076     125     050     X           NOME (Preencher apenas 40 posições)
            10  DATA                        126     133     008     N       0   FORMATO: “DDMMAAAA”
            11  RESERVADO                   134     148     015     X           BRANCOS
            12  INDICADOR DE FIM DE TEXTO   149     150     002     X           X “0DA0” ou X“0D25”
            * ¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨-¨
            Campos obrigatórios: 
             * Transação, 
             * Versão, 
             * Código de Serviço, 
             * Senha de Acesso,
             * Consulta e Indicador de Fim de Texto, 
             * CPF para a consulta “SCAD”, 
             * Nome e Data de Nascimento 
             */

            /*
             EXEMPLO:
                ....+....1....+....2....+....3....+....4....+....5....+....6....+....7....+....8
                CSR5000101RRRRRRRRRR                    12345678SENHA  CONSULTA12345678901NOME
                ....+....9....+....0....+....1....+....2....+....3....+....4....+....5
                                                             DDMMAAAA
             */

            #endregion

            #region MontaStringURL
            //Tamanho 150 posições;
            //1-8
            urlConsulta = transacao;
            //9-10
            urlConsulta += versao;
            //11-20
            urlConsulta += _s10;
            //21-40
            urlConsulta += _s10 + _s10;
            //41-48  
            //urlConsulta += codigo_teste;
            urlConsulta += codigo_prod;
            //49-56  
            //urlConsulta += senha_teste;
            urlConsulta += senha_prod;
            //57-64  
            urlConsulta += tipoConsulta;
            //65-75  
            urlConsulta += cpfBD.Substring(0, 11);
            //76-125  
            //urlConsulta += nomeBD + _s10 + _s10 + _s5 + _s + _s + _s;
            for (int i = 1; i <= 50; i++)
            {
                i = nomeBD.Length;
                nomeBD += " ";
            }

            urlConsulta += nomeBD.Substring(0, 50);
            //126-133
            urlConsulta += dataNascimentoBD.Substring(0, 8);
            //134-148
            urlConsulta += _s10 + _s5;
            //149-150
            urlConsulta += indicadorFimTexto;

            urlConsulta = urlConsulta.Replace(" ", "%20");

            //string urlFinal = url_teste += urlConsulta;
            string urlFinal = url_prod += urlConsulta;

            int totalPosicoes = urlConsulta.Length;

            #endregion

            string strUrlRetorno = IRLib.Paralela.Utilitario.HTTPGetPage(urlFinal).Trim().Replace("<PRE>", "").Replace("</PRE>", "").Replace("\n", "").Replace("\r", "");

            if (GravaConsulta(strUrlRetorno, cpfBD, nomeBD))
                retorno = true;
            else
                retorno = false;

            return retorno;
            //Response.Redirect(urlFinal);

        }

        public bool GravaConsulta(string strUrlRetorno, string cpfBD, string txtNome)
        {
            bool retorno = false;

            #region Variáveis
            /*
            SCPC | SÍNTESE
             * LAYOUT: RESPOSTA DA CONSULTA
            REGISTRO: RESPOSTA DA CONSULTA TAMANHO 228
            
            |COD.|  |CAMPO|                     |I| |F|     |TAM.| |T| |CONTEÚDO|
            01      TRANSAÇÃO                   001 008     008     X   “CSR51001”
            02      VERSÃO                      009 010     002     X   “01”
            03      RESERVADO SOLICITANTE       011 020     010     X   CORRESPONDENTE AO INFORMADO
            04      RESERVADO ACSP              021 040     020     X   USO DA ACSP
            05      CÓDIGO                      041 048     008     N   0 CÓDIGO DE SERVIÇO
            06      SENHA                       049 056     008     X   SENHA DE ACESSO
            07      CONSULTA                    057 064     008     X   “CONF” - Conferencia de Nomes
                                                                        “SCAD” - Sintese P/ Documneto
                                                                        “FONE” - Sintese P/ Nome + Data
            08      CÓDIGO DE RETORNO           065 065     001     X   “0” - Consulta Concluida
                                                                        “1” - Existem mais registros a serem transmitidos
                                                                        “9” - Consulta não efetuada (ver mensagem de erro no campo NOME)
            09      CÓDIGO DE RESPOSTA          066 067     002     X   VER ANEXO 01
            10      NÚMERO DA CONSULTA          068 073     006     X   NÚMERO DA CONSULTA
                                                                        (Quando código de retorno for diferente de “00” e “01”, será preenchido com zeros)
            11      NOME                        074 143     070     X   NOME
            12      DATA DE NASCIMENTO          144 151     008     N   0 FORMATO: “DDMMAAAA”
            13      NOME DA MÃE                 152 201     050     X   NOME DA MÃE
            14      TÍTULO DE ELEITOR           202 214     013     N   0 NÚMERO DO TÍTULO DE ELEITOR
            15      CONDIÇÃO                    215 215     001     X   “R” – Regular
                                                                        “P” - Pendente de Regularização
                                                                        “C” - Cancelado
            16      RESERVADO                   216 226     011     X   BRANCOS
            17      INDICADOR DE FIM DE TEXTO   227 228     002     X   X“0DA0” ou X“0D25”
            */


            //Para Testes
            //strUrlRetorno = "CSR5100101                              CCCCCCCCSSSSSS  SCAD    000014951LEANDRO BASSO DA SILVA                                                07081982MIRNA BASSO DA SILVA                              0000000000000R           ";

            string nomeRet = strUrlRetorno.Replace(" ", "$");

            nomeRet = nomeRet.Substring(73, 70).Replace("$$", "").Replace("$", " ").ToUpper();

            //Session["SCPC_NomeRet"] = nomeRet;

            IRLib.Paralela.Cliente Cliente = new IRLib.Paralela.Cliente();

            #endregion

            //AQUI VERIFICA SE O NOME É IGUAL AO NOME DO BANCO.
            string nomeBD = txtNome;//Cliente.SCPC_ConsultaNomeBD(cpfBD);
            string statusAtualiza = String.Empty;

            SCPC_AtualizaNomeRetorno(cpfBD, nomeRet.ToUpper());

            if (nomeRet.ToUpper() == nomeBD.ToUpper())
            {
                retorno = true;
                //AQUI ATUALIZA |CPFConsultado = True|
                statusAtualiza = "1";
                SCPC_AtualizaStatusCPF(cpfBD, statusAtualiza);
            }
            else
            {
                retorno = false;
                //AQUI ATUALIZA |CPFConsultado = False|
                statusAtualiza = "0";
                SCPC_AtualizaStatusCPF(cpfBD, statusAtualiza);
                AtualizaStatusCadastro(cpfBD, "B");
                //AQUI MENSAGEM DE ERRO , NÃO PERMITINDO A ENTRADA OU COMPRA NO SITE.
            }
            return retorno;
        }

        public DataTable PesquisarCliente(string filtro)
        {
            try
            {
                IDataReader dr = bd.Consulta("SELECT TOP 31 * FROM tCliente (NOLOCK) WHERE " + filtro);
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                dr.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna o cliente ID de acordo com o filtro desejado na tabela tCliente. Retorna 0 se não encontrado
        /// IMPORTANTE: essa função não fecha a conexão de propósito, afim de reutilizar-la mais tarde.
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public int PesquisarClienteID(string filtro)
        {
            try
            {
                if (filtro != string.Empty)
                {
                    bd.FecharConsulta();
                    bd.Consulta("SELECT ID FROM tCliente (NOLOCK) WHERE " + filtro);
                }
                int retorno = 0;

                if (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("ID");
                }
                return retorno;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Exclui Cliente
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            VendaBilheteriaLista vendaBilheteriaLista = new VendaBilheteriaLista();
            vendaBilheteriaLista.FiltroSQL = "ClienteID=" + this.Control.ID;
            vendaBilheteriaLista.TamanhoMax = 1;
            vendaBilheteriaLista.Carregar();

            if (vendaBilheteriaLista.Tamanho > 0)
            {
                throw new ClienteException("Não pode excluir esse cliente. Ele(a) efetuou compras na Ingresso Rápido.");
            }
            else
            {
                return this.Excluir(this.Control.ID);
            }

        }

        /// <summary>
        /// Obter a qtde de ingressos comprados por esse cliente numa apresentacaoSetor especifico
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressoCompradoPorApresentacaoSetor(int apresentacaosetorid)
        {

            try
            {

                if (this.Control.ID == 0)
                    throw new Exception("Cliente não foi atribuído.");

                string sql = "SELECT Count(*) as Qtde FROM tVendaBilheteria as vb, tVendaBilheteriaItem as vbi, tCliente as c, tIngressoLog as il, tIngresso as i, tApresentacaoSetor as tas, tPreco as p " +
                    "WHERE (il.IngressoID=i.ID AND i.ApresentacaoSetorID=tas.ID AND i.PrecoID=p.ID AND " +
                    "(vb.Status='" + VendaBilheteria.PAGO + "' OR vb.Status='" + VendaBilheteria.ENTREGUE + "') AND vbi.VendaBilheteriaID=vb.ID AND il.VendaBilheteriaItemID=vbi.ID AND vb.ClienteID=c.ID AND tas.ID=" + apresentacaosetorid + " AND c.ID=" + this.Control.ID + ")";

                object ret = bd.ConsultaValor(sql);
                bd.Fechar();
                int qtde = (ret != null) ? (int)ret : 0;
                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter a qtde de ingressos comprados por esse cliente numa apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressoCompradoPorApresentacao(int apresentacaoid)
        {

            try
            {

                if (this.Control.ID == 0)
                    throw new Exception("Cliente não foi atribuído.");

                string sql = "SELECT Count(*) as Qtde FROM tVendaBilheteria as vb, tVendaBilheteriaItem as vbi, tCliente as c, tIngressoLog as il, tIngresso as i, tApresentacao as a, tApresentacaoSetor as tas, tPreco as p " +
                    "WHERE (il.IngressoID=i.ID AND tas.ApresentacaoID=a.ID AND i.ApresentacaoSetorID=tas.ID AND i.PrecoID=p.ID AND " +
                    "(vb.Status='" + VendaBilheteria.PAGO + "' OR vb.Status='" + VendaBilheteria.ENTREGUE + "') AND vbi.VendaBilheteriaID=vb.ID AND il.VendaBilheteriaItemID=vbi.ID AND vb.ClienteID=c.ID AND a.ID=" + apresentacaoid + " AND c.ID=" + this.Control.ID + ")";

                object ret = bd.ConsultaValor(sql);
                bd.Fechar();
                int qtde = (ret != null) ? (int)ret : 0;
                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter a qtde de ingressos comprados por esse cliente num preco especifico
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressoCompradoPorPreco(int precoid)
        {

            try
            {

                if (this.Control.ID == 0)
                    throw new Exception("Cliente não foi atribuído.");

                string sql = "SELECT Count(*) as Qtde FROM tVendaBilheteria as vb, tVendaBilheteriaItem as vbi, tCliente as c, tIngressoLog as il, tIngresso as i, tApresentacaoSetor as tas, tPreco as p " +
                    "WHERE (il.IngressoID=i.ID AND i.ApresentacaoSetorID=tas.ID AND i.PrecoID=p.ID AND " +
                    "(vb.Status='" + VendaBilheteria.PAGO + "' OR vb.Status='" + VendaBilheteria.ENTREGUE + "') AND vbi.VendaBilheteriaID=vb.ID AND il.VendaBilheteriaItemID=vbi.ID AND vb.ClienteID=c.ID AND p.ID=" + precoid + " AND c.ID=" + this.Control.ID + ")";

                object ret = bd.ConsultaValor(sql);
                bd.Fechar();
                int qtde = (ret != null) ? (int)ret : 0;
                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obter a qtde de ingressos comprados por esse cliente num pacote especifico
        /// </summary>
        /// <returns></returns>
        public override int QtdeIngressoCompradoPorPacote(int pacoteid)
        {
            return 0;
        }

        /// <summary>
        /// Obtendo um DataTable dos Homônimos
        /// </summary>
        /// <param name="registroZero">Um string que vai conter a descricao do registro zero</param>
        /// <returns></returns>
        public override DataTable Homonimos(string registroZero)
        {
            DataTable tabela = new DataTable("Homonimos");
            try
            {
                // Criar DataTable com as colunas
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                // Obtendo dados atravehs de SQL
                BD bd = new BD();
                string sql = "SELECT tCliente.ID, tCliente.Nome " +
                    "FROM tCliente " +
                    "WHERE (tCliente.Nome like '%" + this.Nome.Valor + "%')";
                bd.Consulta(sql);
                // Alimentando DataTable
                if (registroZero != "")
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = 0;
                    linha["Nome"] = registroZero;
                    tabela.Rows.Add(linha);
                }
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("Erro ao pesquisar homônimos no cliente!!");
            }
            // retorna DataTable
            return tabela;
        } // fim de Homonimos

        public string NovaSenha(int ID, string senha)
        {
            if (bd.Executar("UPDATE tCliente SET Ativo = 'T', Senha = '" + senha + "' WHERE ID = '" + ID + "'") > 0)
            {
                object x = bd.ConsultaValor("SELECT Email FROM tCliente WHERE ID = " + ID);
                return x.ToString();
            }
            return string.Empty;

        }

        /// <summary>
        /// Captura um array com a descrição do evento e a quantidade
        /// </summary>
        /// <param name="ID">ID do cliente</param>
        /// <returns></returns>
        public string[] EventosComprados(int ID)
        {
            System.Collections.Generic.List<string> ListaEventos = new System.Collections.Generic.List<string>();

            BD bdEventos = new BD();

            try
            {
                if (ID > 0)
                {
                    using (IDataReader oDataReader = bdEventos.Consulta("" +
                        "SELECT TOP 10 " +
                        "	COUNT(tIngresso.ID) AS Qtd, " +
                        "	tIngresso.EventoID, " +
                        "	tEvento.Nome, " +
                        "	tIngresso.VendaBilheteriaID " +
                        "FROM tIngresso (NOLOCK) " +
                        "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                        "WHERE " +
                        "	tIngresso.ClienteID = " + ID + " " +
                        "GROUP BY " +
                        "	tIngresso.EventoID, " +
                        "	tEvento.Nome, " +
                        "	tIngresso.VendaBilheteriaID " +
                        "ORDER BY " +
                        "	tIngresso.VendaBilheteriaID  " +
                        "DESC "))
                    {
                        while (oDataReader.Read())
                            ListaEventos.Add("(" + bdEventos.LerString("Nome") + ") - " + bdEventos.LerInt("Qtd"));
                    }

                    bdEventos.FecharConsulta();

                    string sql = "SELECT TOP 5 COUNT(tValeIngresso.ID) AS Qtd, tValeIngressoTipo.Nome, tValeIngresso.VendaBilheteriaID " +
                        "FROM tValeIngresso (NOLOCK) " +
                        "INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngresso.ValeIngressoTipoID = tValeIngressoTipo.ID " +
                        "WHERE tValeIngresso.ClienteID = " + ID +
                        " GROUP BY tValeIngressoTipo.Nome, tValeIngresso.VendaBilheteriaID ORDER BY tValeIngresso.VendaBilheteriaID DESC ";

                    bdEventos.Consulta(sql);
                    while (bdEventos.Consulta().Read())
                        ListaEventos.Add("(" + bdEventos.LerString("Nome") + ") - " + bdEventos.LerInt("Qtd"));



                    if (ListaEventos.Count == 0)
                    {
                        ListaEventos.Add("Nenhum evento comprado.");
                    }
                }
                else
                {
                    ListaEventos.Add("Erro ao pesquisar cliente.");
                    ListaEventos.Add("Tente selecionar o cliente novamente.");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bdEventos.Fechar();
            }
            return ListaEventos.ToArray();
        }

        /// <summary>
        /// Utilizada para eliminar todos os registros existentes na lista, e transferir todos os dados relacionados para o ClienteID informado.
        /// </summary>
        /// <param name="listaClienteVendas">Lista com todos os clientes e vendas</param>
        /// <param name="clienteID">ID do cliente correto</param>
        public void EliminarDuplicidade(List<EstruturaClienteVendas> listaClienteVendas, int clienteID)
        {
            BD bdTransacao = null;
            EstruturaClienteVendas clienteVendas;
            EstruturaVenda venda;
            int retornoExecutar = 0;

            try
            {
                // Se não existir itens na lista, não precisa executar o processo de abertura de transação
                if (listaClienteVendas.Count != 0)
                {
                    bdTransacao = new BD();
                    bdTransacao.IniciarTransacao();

                    // Loop na lista de clientes
                    for (int contadorClientes = 0; contadorClientes < listaClienteVendas.Count; contadorClientes++)
                    {
                        // Captura o clienteVenda
                        clienteVendas = listaClienteVendas[contadorClientes];

                        // Se for um cliente que deve ser eliminado.
                        if (clienteVendas.Cliente.ID != clienteID)
                        {
                            // Se existirem vendas para o cliente
                            if (clienteVendas.Vendas.Count > 0)
                            {
                                for (int contadorVendas = 0; contadorVendas < clienteVendas.Vendas.Count; contadorVendas++)
                                {
                                    venda = clienteVendas.Vendas[contadorVendas];

                                    // Atualiza tVendaBilheteria, transfere as vendas do cliente antigo, para o cliente novo
                                    retornoExecutar = bdTransacao.Executar("" +
                                        "UPDATE " +
                                        "   tVendaBilheteria " +
                                        "SET " +
                                        "   ClienteID = " + clienteID + " " +
                                        "WHERE " +
                                        "   ID = " + venda.VendaBilheteriaID);
                                    if (retornoExecutar == 0)
                                        throw new ClienteException("Não foi possível alterar o cliente nas vendas informadas.");

                                    // Registra o Log da atualização na tabela de auditoria
                                    retornoExecutar = bdTransacao.Executar("" +
                                        "INSERT INTO " +
                                        "   tAuditoriaVendaBilheteria " +
                                        "( " +
                                        "   VendaBilheteriaID, " +
                                        "   ClienteIDNovo, " +
                                        "   ClienteIDAntigo, " +
                                        "   UsuarioID, " +
                                        "   Data " +
                                        ") VALUES ( " +
                                            venda.VendaBilheteriaID + ", " +
                                            clienteID + ", " +
                                            clienteVendas.Cliente.ID + ", " +
                                            this.Control.UsuarioID + ", " +
                                            "'" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "' " +
                                        ")");
                                    if (retornoExecutar == 0)
                                        throw new ClienteException("Não foi possível incluir a auditoria de atualização de venda.");

                                    // Atualiza tIngresso
                                    bdTransacao.Executar("" +
                                        "UPDATE " +
                                        "   tIngresso " +
                                        "SET " +
                                        "   ClienteID = " + clienteID + " " +
                                        "WHERE " +
                                        "   VendaBilheteriaID = " + venda.VendaBilheteriaID);

                                    // Atualiza tIngressoLog
                                    bdTransacao.Executar("" +
                                        "UPDATE " +
                                        "   tIngressoLog " +
                                        "SET " +
                                        "   ClienteID = " + clienteID + " " +
                                        "WHERE " +
                                        "   VendaBilheteriaID = " + venda.VendaBilheteriaID);

                                }
                            }

                            // Exclui o Cliente
                            retornoExecutar = bdTransacao.Executar("" +
                                "DELETE FROM " +
                                "   tCliente " +
                                "WHERE " +
                                "   ID = " + clienteVendas.Cliente.ID);
                            if (retornoExecutar == 0)
                                throw new ClienteException("Não foi possível excluir o cliente.");

                        }
                    }

                    bdTransacao.FinalizarTransacao();
                }
            }
            catch (Exception)
            {
                bdTransacao.DesfazerTransacao();
                throw;
            }
            finally
            {
                if (bdTransacao != null)
                    bdTransacao.Fechar();
            }
        }

        /// <summary>
        /// Busca duplicidade na tabela de clientes de acordo com os parametros informados
        /// </summary>
        /// <param name="texto">Texto a validar duplicidade</param>
        /// <param name="tipoDuplicidade">Campo que deseja validar</param>
        /// <returns>Lista com os clientes em duplicidade</returns>
        public List<EstruturaClienteVendas> BuscaDuplicidade(string texto, Duplicidade tipoDuplicidade, int tipoBuscaVenda)
        {
            List<EstruturaClienteVendas> listaClienteVendas = new List<EstruturaClienteVendas>();
            EstruturaClienteVendas clientevendas;
            EstruturaCliente cliente;
            EstruturaVenda venda;

            string campo = string.Empty;
            string filtro_1 = string.Empty;
            string filtroTipoBusca = string.Empty;
            string filtroTipoBuscaVenda = string.Empty;

            if (tipoBuscaVenda > 0)
                filtroTipoBusca = " HAVING COUNT(tVendaBilheteria.ID) > 0";

            if (tipoBuscaVenda == 1)
                filtroTipoBuscaVenda = " WHERE QtdeEventos > 0 ";
            else if (tipoBuscaVenda == 2)
                filtroTipoBuscaVenda = " WHERE QtdeEventos = 0 OR QtdeEventos IS NULL ";

            try
            {
                // Se o tipo de duplicidade não for definido corretamente, o sistema deve abortar a busca.
                switch (tipoDuplicidade)
                {
                    case Duplicidade.CPF:
                        campo = "CPF";
                        break;
                    case Duplicidade.Email:
                        campo = "Email";
                        break;
                    default:
                        throw new ClienteException("Não foi identificada o tipo de duplicidade que deseja buscar.");
                }

                if (texto.Length > 0)
                    filtro_1 = " AND tClienteFiltro." + campo + " = '" + texto + "'";

                // Consulta de clientes
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "	tClienteFinal.*, " +
                    "	(" +
                    "		SELECT  " +
                    "			COUNT(tVendaBilheteria.ID) " +
                    "		FROM " +
                    "			tVendaBilheteria (NOLOCK) " +
                    "		WHERE " +
                    "			tVendaBilheteria.ClienteID = tClienteFinal.ID " +
                    "		AND " +
                    "			tVendaBilheteria.Status = 'P' " +
                            filtroTipoBusca +
                    "	) AS QtdeEventos, " +
                    "	(" +
                    "		SELECT TOP 1 " +
                    "			cCliente.[TimeStamp] " +
                    "		FROM  " +
                    "			cCliente (NOLOCK) " +
                    "		WHERE " +
                    "			cCliente.ID = tClienteFinal.ID " +
                    "		ORDER BY " +
                    "			cCliente.[TimeStamp] " +
                    "		DESC " +
                    "	) AS DataUltimaAtualizacao, " +
                    "   (" +
                    "       SELECT " +
                    "	        COUNT(tIngresso.ID) " +
                    "       FROM " +
                    "	        tIngresso (NOLOCK) " +
                    "       WHERE " +
                    "	        tIngresso.Status = 'V' " +
                    "       AND " +
                    "	        tIngresso.ClienteID = tClienteFinal.ID " +
                    "   ) AS TotalIngressosNaoImpressos " +
                    "  INTO #temp  " +
                    "FROM " +
                    "	tCliente AS tClienteFinal " +
                    "WHERE " +
                    "	tClienteFinal." + campo + " IN " +
                    "	(" +
                    "		SELECT " +
                    "			tClienteFiltro." + campo + " " +
                    "		FROM " +
                    "			tCliente AS tClienteFiltro (NOLOCK) " +
                    "		WHERE " +
                    "			tClienteFiltro." + campo + " <> '' " +
                    //"		AND " +
                    //"			tClienteFiltro." + campo + " = '" + texto + "'" +
                    filtro_1 +
                    "		GROUP BY 	" +
                    "			tClienteFiltro." + campo + " " +
                    "		HAVING " +
                    "			COUNT(tClienteFiltro." + campo + ") > 1 " +
                    "	) " +
                    "ORDER BY " +
                    "	tClienteFinal.Nome" +

                    " 	SELECT  * " +
                    "	  FROM #temp " +
                    filtroTipoBuscaVenda +
                    "/*DROP TABLE #temp */"
                    ))
                {
                    while (oDataReader.Read())
                    {
                        // Inicializa as estruturas
                        clientevendas = new EstruturaClienteVendas();
                        clientevendas.Vendas = new List<EstruturaVenda>();
                        cliente = new EstruturaCliente();

                        // Dados gerais
                        clientevendas.DataAtualizacao = bd.LerDateTime("DataUltimaAtualizacao");
                        clientevendas.QuantidadeVendas = bd.LerInt("QtdeEventos");
                        clientevendas.PossuiIngressos = bd.LerInt("TotalIngressosNaoImpressos") > 0;

                        // Dados do Cliente
                        cliente.ID = bd.LerInt("ID");
                        cliente.ClienteIndicacaoID = bd.LerInt("ClienteIndicacaoID");
                        cliente.Status = bd.LerString("StatusAtual");
                        cliente.Nome = bd.LerString("Nome");
                        cliente.RG = bd.LerString("RG");
                        cliente.CPF = bd.LerString("CPF");
                        cliente.CarteiraEstudante = bd.LerString("CarteiraEstudante");
                        cliente.Sexo = bd.LerString("Sexo");
                        cliente.TelefoneResidencialDDD = bd.LerString("DDDTelefone");
                        cliente.TelefoneResidencial = bd.LerString("Telefone");
                        cliente.TelefoneComercialDDD = bd.LerString("DDDTelefoneComercial");
                        cliente.TelefoneComercial = bd.LerString("TelefoneComercial");
                        cliente.TelefoneCelularDDD = bd.LerString("DDDCelular");
                        cliente.TelefoneCelular = bd.LerString("Celular");
                        cliente.DataNascimento = bd.LerDateTime("DataNascimento");
                        cliente.Email = bd.LerString("Email");
                        cliente.ReceberEmail = bd.LerString("RecebeEmail");
                        cliente.CEPEntrega = bd.LerString("CEPEntrega");
                        cliente.EnderecoEntrega = bd.LerString("EnderecoEntrega");
                        cliente.EnderecoNumeroEntrega = bd.LerString("NumeroEntrega");
                        cliente.EnderecoComplementoEntrega = bd.LerString("ComplementoEntrega");
                        cliente.BairroEntrega = bd.LerString("BairroEntrega");
                        cliente.CidadeEntrega = bd.LerString("CidadeEntrega");
                        cliente.EstadoEntrega = bd.LerString("EstadoEntrega");
                        cliente.CEPCliente = bd.LerString("CEPCliente");
                        cliente.EnderecoCliente = bd.LerString("EnderecoCliente");
                        cliente.EnderecoNumeroCliente = bd.LerString("NumeroCliente");
                        cliente.EnderecoComplementoCliente = bd.LerString("ComplementoCliente");
                        cliente.BairroCliente = bd.LerString("BairroCliente");
                        cliente.CidadeCliente = bd.LerString("CidadeCliente");
                        cliente.EstadoCliente = bd.LerString("EstadoCliente");
                        cliente.NomeEntrega = bd.LerString("NomeEntrega");
                        cliente.CpfEntrega = bd.LerString("CPFEntrega");
                        cliente.RgEntrega = bd.LerString("RGEntrega");
                        cliente.Observacao = bd.LerString("Obs");
                        cliente.Senha = bd.LerString("Senha");
                        cliente.Ativo = bd.LerBoolean("Ativo");
                        cliente.StatusConsulta = bd.LerString("StatusConsulta");
                        cliente.StatusConsultaEntrega = bd.LerString("StatusConsultaEntrega");

                        clientevendas.Cliente = cliente;

                        listaClienteVendas.Add(clientevendas);
                    }
                }

                // Atribui a lista de vendas
                for (int contador = 0; contador < listaClienteVendas.Count; contador++)
                {
                    // Seleciona o índice atual dentro da lista de ClienteVendas
                    clientevendas = listaClienteVendas[contador];

                    // Consulta de vendas
                    using (IDataReader oDataReader = bd.Consulta("" +
                        "SELECT " +
                        "   tVendaBilheteria.ID, " +
                        "   tVendaBilheteria.DataVenda, " +
                        "   tVendaBilheteria.Status, " +
                        "   tVendaBilheteria.Senha " +
                        "FROM " +
                        "   tVendaBilheteria (NOLOCK) " +
                        "WHERE " +
                        "   tVendaBilheteria.ClienteID = " + clientevendas.Cliente.ID + " " +
                        "ORDER BY " +
                        "   tVendaBilheteria.ID DESC"))
                    {
                        while (oDataReader.Read())
                        {
                            venda = new EstruturaVenda();

                            venda.VendaBilheteriaID = bd.LerInt("ID");
                            venda.Data = bd.LerDateTime("DataVenda");
                            venda.Senha = bd.LerString("Senha");
                            venda.Status = VendaBilheteria.StatusDescritivo(bd.LerString("Status"));

                            clientevendas.Vendas.Add(venda);
                        }
                    }

                    listaClienteVendas[contador] = clientevendas;
                }
                bd.Fechar();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return listaClienteVendas;
        }

        public int AtualizaStatusLiberado(int idCliente)
        {
            int retorno = 0;
            try
            {
                string sql = "Update tCliente SET StatusAtual = 'L' WHERE ID = " + idCliente;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int AtualizaStatusBloqueado(int idCliente)
        {
            int retorno = 0;
            try
            {
                string sql = "Update tCliente SET StatusAtual = 'B' WHERE ID = " + idCliente;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int AtualizaBomCliente(int idCliente)
        {
            int retorno = 0;
            try
            {
                string sql = "Update tCliente SET NivelCliente = 1 WHERE ID = " + idCliente;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int AtualizaMalCliente(int idCliente)
        {
            int retorno = 0;
            try
            {
                string sql = "Update tCliente SET NivelCliente = 0 WHERE ID = " + idCliente;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int AtualizaStatusBomCliente(int VendaBilheteriaID)
        {
            int retorno = 0;
            try
            {
                string sql = "Select V.ClienteID from tVendaBilheteria as V where ID = " + VendaBilheteriaID;
                int clienteID = Convert.ToInt32(bd.ConsultaValor(sql));

                sql = "Update tCliente SET NivelCliente = 1 WHERE ID = " + clienteID;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente. " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int AtualizaStatusMaliente(int VendaBilheteriaID)
        {
            int retorno = 0;
            try
            {
                string sql = "Select V.ClienteID from tVendaBilheteria as V where ID = " + VendaBilheteriaID;
                int clienteID = Convert.ToInt32(bd.ConsultaValor(sql));

                sql = "Update tCliente SET NivelCliente = 0 WHERE ID = " + clienteID;
                retorno = bd.Executar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o Cliente. " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public int BloquearCliente(int VendaBilheteriaID, char Status)
        {
            int retorno = 0;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE tCliente ");
                sql.Append("    SET StatusAtual = '" + Status + "' ");
                sql.Append("  WHERE id in( ");
                sql.Append("                SELECT ClienteID  ");
                sql.Append("                  FROM tVendaBilheteria  ");
                sql.Append("                 WHERE ID = " + VendaBilheteriaID + " ");
                sql.Append("              )");
                retorno = bd.Executar(sql.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Erro ao atualizar o Cliente");
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;
        }

        public string buscaContatoTipo(int contatoTipo)
        {
            string contato = string.Empty;

            try
            {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("SELECT ");
                strSql.Append("TOP 1 NOME ");
                strSql.Append("FROM tContatoTipo ");
                strSql.Append("WHERE ID = " + contatoTipo);

                contato = bd.ConsultaValor(strSql.ToString()).ToString();

                //if (bd.Consulta(strSql.ToString()).Read())
                //{
                //    contato = bd.LerString("Nome");
                //}

                return contato;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string[] BuscaEmail(int id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                string[] retorno = new string[2];

                strSql.Append("  SELECT ISNULL(Nome, '') AS Nome, ");
                strSql.Append(" ISNULL(Email, '') AS Email ");
                strSql.Append(" FROM tCliente ");
                strSql.Append(" WHERE ID = " + id);

                if (bd.Consulta(strSql.ToString()).Read())
                {
                    retorno[0] = bd.LerString("Email");
                    retorno[1] = bd.LerString("Nome");
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaCliente> BuscaDetalhes(int id)
        {
            try
            {
                List<EstruturaCliente> lista = new List<EstruturaCliente>();
                EstruturaCliente item;
                StringBuilder strSql = new StringBuilder();

                strSql.Append("  SELECT ");
                strSql.Append(" Nome, ");
                strSql.Append(" Email, ");
                strSql.Append(" CPF, ");
                strSql.Append(" RG, ");
                strSql.Append(" DDDTelefone, ");
                strSql.Append(" Telefone, ");
                strSql.Append(" DDDCelular, ");
                strSql.Append(" Celular, ");
                strSql.Append(" CEP, ");
                strSql.Append(" Complemento, ");
                strSql.Append(" Bairro, ");
                strSql.Append(" Pais, ");
                strSql.Append(" Endereco, ");
                strSql.Append(" Numero, ");
                strSql.Append(" Cidade, ");
                strSql.Append(" estado, ");
                strSql.Append(" EnderecoEntrega, ");
                strSql.Append(" NumeroEntrega, ");
                strSql.Append(" CidadeEntrega, ");
                strSql.Append(" EstadoEntrega, ");
                strSql.Append(" ComplementoEntrega, ");
                strSql.Append(" BairroEntrega, ");
                strSql.Append(" NomeEntrega, ");
                strSql.Append(" CPFEntrega, ");
                strSql.Append(" RGEntrega, ");
                strSql.Append(" Obs, ");
                strSql.Append(" ContatoTipoID, ");
                strSql.Append(" StatusAtual, ");
                strSql.Append(" NivelCliente, ");
                strSql.Append(" CNPJ, ");
                strSql.Append(" NomeFantasia ");
                strSql.Append(" FROM tCliente ");
                strSql.Append(" WHERE ID = " + id);

                if (bd.Consulta(strSql.ToString()).Read())
                {
                    item = new EstruturaCliente();

                    item.Nome = bd.LerString("Nome");
                    item.Email = bd.LerString("Email");
                    item.CPF = bd.LerString("CPF");
                    item.RG = bd.LerString("RG");
                    if (bd.LerString("Telefone").Length > 0)
                        item.TelefoneResidencial = "(" + bd.LerString("DDDTelefone") + ") " + bd.LerString("Telefone");
                    if (bd.LerString("Celular").Length > 0)
                        item.TelefoneCelular = "(" + bd.LerString("DDDCelular") + ") " + bd.LerString("Celular");
                    item.CEPCliente = bd.LerString("CEP");
                    item.EnderecoComplementoCliente = bd.LerString("Complemento");
                    item.BairroCliente = bd.LerString("Bairro");
                    item.Pais = bd.LerString("Pais");
                    item.EnderecoCliente = bd.LerString("Endereco");

                    if (bd.LerString("Numero").Length > 0)
                        item.EnderecoCliente += ", " + bd.LerString("Numero");
                    if (bd.LerString("Cidade").Length > 0)
                        item.EnderecoCliente += " - " + bd.LerString("Cidade");
                    if (bd.LerString("Estado").Length > 0)
                        item.EnderecoCliente += " - " + bd.LerString("Estado");

                    if (bd.LerString("EnderecoEntrega").Length > 0)
                        item.EnderecoEntrega = bd.LerString("EnderecoEntrega");
                    if (bd.LerString("NumeroEntrega").Length > 0)
                        item.EnderecoNumeroEntrega = bd.LerString("NumeroEntrega");
                    if (bd.LerString("CidadeEntrega").Length > 0)
                        item.CidadeEntrega = bd.LerString("CidadeEntrega");
                    if (bd.LerString("EstadoEntrega").Length > 0)
                        item.EstadoEntrega = bd.LerString("EstadoEntrega");
                    if (bd.LerString("ComplementoEntrega").Length > 0)
                        item.EnderecoComplementoEntrega = bd.LerString("ComplementoEntrega");
                    if (bd.LerString("BairroEntrega").Length > 0)
                        item.BairroEntrega = bd.LerString("BairroEntrega");
                    if (bd.LerString("NomeEntrega").Length > 0)
                        item.NomeEntrega = bd.LerString("NomeEntrega");
                    if (bd.LerString("CPFEntrega").Length > 0)
                        item.CpfEntrega = bd.LerString("CPFEntrega");
                    if (bd.LerString("RGEntrega").Length > 0)
                        item.RgEntrega = bd.LerString("RGEntrega");
                    if (bd.LerString("Obs").Length > 0)
                        item.Observacao = bd.LerString("Obs");
                    if (bd.LerString("ContatoTipoID").Length > 0)
                        item.ContatoTipoID = bd.LerInt("ContatoTipoID");

                    item.Status = bd.LerString("StatusAtual");
                    item.CNPJ = bd.LerString("CNPJ");
                    item.NomeFantasia = bd.LerString("NomeFantasia");

                    if (item.Status.Equals("L"))
                    {
                        item.StatusLiberado = true;
                        item.StatusBloqueado = false;
                    }
                    else if (item.Status.Equals("B"))
                    {
                        item.StatusLiberado = false;
                        item.StatusBloqueado = true;
                    }

                    item.NivelCliente = bd.LerInt("NivelCliente");

                    if (item.NivelCliente == 0)
                    {
                        item.BomCliente = false;
                        item.MalCliente = true;
                    }
                    else if (item.NivelCliente == 1)
                    {
                        item.BomCliente = true;
                        item.MalCliente = false;
                    }

                    lista.Add(item);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LerClienteSenhaDeCompra(string Senha)
        {
            try
            {

                bd.FecharConsulta();
                bd.Consulta(@"SELECT tCliente.ID 
                    FROM tCliente (NOLOCK)
                    INNER JOIN tVendaBilheteria ON tCliente.ID=tVendaBilheteria.ClienteID
                    WHERE tVendaBilheteria.Senha= '" + Senha + "' ");

                if (bd.Consulta().Read())
                {
                    this.Ler(bd.LerInt("ID"));
                }


            }
            catch
            {
                throw;
            }
        }

        public List<EstruturaCliente> BuscaTodasInformacoes(int ClienteID)
        {
            try
            {
                List<EstruturaCliente> lista = new List<EstruturaCliente>();
                EstruturaCliente item;
                StringBuilder strSql = new StringBuilder();

                strSql.Append("  SELECT * FROM tCliente (NOLOCK)");
                strSql.Append("   WHERE ID = " + ClienteID);

                if (bd.Consulta(strSql.ToString()).Read())
                {
                    item = new EstruturaCliente();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    item.RG = bd.LerString("RG");
                    item.CPF = bd.LerString("CPF");
                    item.CPFResponsavel = bd.LerString("CPFResponsavel");
                    item.Sexo = bd.LerString("Sexo");
                    item.TelefoneResidencialDDD = bd.LerString("DDDTelefone");
                    item.TelefoneResidencial = bd.LerString("Telefone");
                    item.TelefoneComercialDDD = bd.LerString("DDDTelefoneComercial");
                    item.TelefoneComercial = bd.LerString("TelefoneComercial");
                    item.TelefoneCelularDDD = bd.LerString("DDDCelular");
                    item.TelefoneCelular = bd.LerString("Celular");
                    item.DataNascimento = bd.LerDateTime("DataNascimento");
                    item.Email = bd.LerString("Email");
                    item.CEPCliente = bd.LerString("CEP");
                    item.EnderecoCliente = bd.LerString("Endereco");
                    item.EnderecoNumeroCliente = bd.LerString("Numero");
                    item.CidadeCliente = bd.LerString("Cidade");
                    item.EstadoCliente = bd.LerString("Estado");
                    item.Observacao = bd.LerString("Obs");
                    item.EnderecoComplementoCliente = bd.LerString("Complemento");
                    item.BairroCliente = bd.LerString("Bairro");
                    item.EnderecoComplementoEntrega = bd.LerString("ComplementoCobranca");
                    item.Ativo = bd.LerBoolean("Ativo");
                    item.Status = bd.LerString("StatusAtual");
                    item.CEPEntrega = bd.LerString("CEPEntrega");
                    item.EnderecoEntrega = bd.LerString("EnderecoEntrega");
                    item.EnderecoNumeroEntrega = bd.LerString("NumeroEntrega");
                    item.CidadeEntrega = bd.LerString("CidadeEntrega");
                    item.EstadoEntrega = bd.LerString("EstadoEntrega");
                    item.EnderecoComplementoEntrega = bd.LerString("ComplementoEntrega");
                    item.BairroEntrega = bd.LerString("BairroEntrega");
                    item.CEPCliente = bd.LerString("CEPCliente");
                    item.EnderecoCliente = bd.LerString("EnderecoCliente");
                    item.EnderecoNumeroCliente = bd.LerString("NumeroCliente");
                    item.CidadeCliente = bd.LerString("CidadeCliente");
                    item.EstadoCliente = bd.LerString("EstadoCliente");
                    item.EnderecoComplementoCliente = bd.LerString("ComplementoCliente");
                    item.BairroCliente = bd.LerString("BairroCliente");
                    item.NomeEntrega = bd.LerString("NomeEntrega");
                    item.CpfEntrega = bd.LerString("CPFEntrega");
                    item.RgEntrega = bd.LerString("RGEntrega");
                    item.Senha = bd.LerString("Senha");
                    item.ContatoTipoID = bd.LerInt("ContatoTipoID");
                    item.NivelCliente = bd.LerInt("NivelCliente");
                    item.CNPJ = bd.LerString("CNPJ");
                    item.NomeFantasia = bd.LerString("NomeFantasia");
                    item.Pais = bd.LerString("Pais");
                    item.TipoCadastro = Convert.ToChar(bd.LerString("TipoCadstro"));
                    lista.Add(item);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaCliente Estrutura
        {
            get
            {
                return new EstruturaCliente()
                {
                    ID = this.Control.ID,
                    Nome = this.Nome.Valor,
                    RG = this.RG.Valor,
                    Pais = this.Pais.Valor,
                    CPF = this.CPF.Valor,
                    CPFResponsavel = this.CPFResponsavel.Valor,
                    Sexo = this.Sexo.Valor,
                    TelefoneResidencialDDD = this.DDDTelefone.Valor,
                    TelefoneResidencial = this.Telefone.Valor,
                    TelefoneComercialDDD = this.DDDTelefoneComercial.Valor,
                    TelefoneComercial = this.TelefoneComercial.Valor,
                    TelefoneCelularDDD = this.DDDCelular.Valor,
                    TelefoneCelular = this.Celular.Valor,
                    DataNascimento = this.DataNascimento.Valor,

                    Email = this.Email.Valor,

                    CEPCliente = this.CEPCliente.Valor,
                    EnderecoCliente = this.EnderecoCliente.Valor,
                    EnderecoNumeroCliente = this.NumeroCliente.Valor,
                    CidadeCliente = this.CidadeCliente.Valor,
                    EstadoCliente = this.EstadoCliente.Valor,
                    EnderecoComplementoCliente = this.ComplementoCliente.Valor,
                    BairroCliente = this.BairroCliente.Valor,
                };
            }

            // cliente.ID = Convert.ToInt32(cliente.ID);
            //cliente.ContatoTipo = cliente.ContatoTipo;
            //cliente.Nome = Convert.ToString(cliente.Nome) + "";
            //cliente.CPF = Convert.ToString(cliente.CPF) + "";
            //cliente.RG = Convert.ToString(cliente.RG) + "";
            //cliente.Sexo = Convert.ToString(cliente.Sexo) + "";
            //cliente.DataNascimentoTS = Convert.ToString(cliente.DataNascimentoTS) + "";
            //cliente.TelefoneResidencialDDD = Convert.ToString(cliente.TelefoneResidencialDDD) + "";
            //cliente.TelefoneResidencial = Convert.ToString(cliente.TelefoneResidencial) + "";
            //cliente.TelefoneCelularDDD = Convert.ToString(cliente.TelefoneCelularDDD) + "";
            //cliente.TelefoneCelular = Convert.ToString(cliente.TelefoneCelular) + "";
            //cliente.TelefoneComercialDDD = Convert.ToString(cliente.TelefoneComercialDDD) + "";
            //cliente.TelefoneComercial = Convert.ToString(cliente.TelefoneComercial) + "";
            //cliente.Email = Convert.ToString(cliente.Email) + "";
            //cliente.Senha = Convert.ToString(cliente.Senha) + "";
            //cliente.SenhaConfirmacao = Convert.ToString(cliente.SenhaConfirmacao) + "";
            //cliente.ReceberEmail = Convert.ToString(cliente.ReceberEmail) + "";

            //cliente.CEPCliente = Convert.ToString(cliente.CEPCliente) + "";
            //cliente.EnderecoCliente = Convert.ToString(cliente.EnderecoCliente) + "";
            //cliente.EnderecoNumeroCliente = Convert.ToString(cliente.EnderecoNumeroCliente) + "";
            //cliente.EnderecoComplementoCliente = Convert.ToString(cliente.EnderecoComplementoCliente) + "";
            //cliente.BairroCliente = Convert.ToString(cliente.BairroCliente) + "";
            //cliente.CidadeCliente = Convert.ToString(cliente.CidadeCliente) + "";
            //cliente.EstadoCliente = Convert.ToString(cliente.EstadoCliente) + "";

            //cliente.CEPEntrega = Convert.ToString(cliente.CEPEntrega) + "";
            //cliente.EnderecoEntrega = Convert.ToString(cliente.EnderecoEntrega) + "";
            //cliente.EnderecoNumeroEntrega = Convert.ToString(cliente.EnderecoNumeroEntrega) + "";
            //cliente.EnderecoComplementoEntrega = Convert.ToString(cliente.EnderecoComplementoEntrega) + "";
            //cliente.BairroEntrega = Convert.ToString(cliente.BairroEntrega) + "";
            //cliente.CidadeEntrega = Convert.ToString(cliente.CidadeEntrega) + "";
            //cliente.EstadoEntrega = Convert.ToString(cliente.EstadoEntrega) + "";
            //cliente.CPFResponsavel = cliente.CPF ?? string.Empty;

        }

        public string BuscaCep(int ClienteID)
        {
            try
            {
                string Cep = "";
                bd.FecharConsulta();
                bd.Consulta(@"SELECT tCliente.CEPCliente 
                    FROM tCliente (NOLOCK)
                    WHERE tCliente.ID= '" + ClienteID + "' ");

                if (bd.Consulta().Read())
                {
                    Cep = bd.LerString("CEPCliente");
                }

                return Cep;
            }
            catch
            {
                throw;
            }
        }

        public int GetNivelRiscoCliente(int ClienteID)
        {
            try
            {
                int retorno = 0;

                string sql = "SELECT NivelCliente FROM tCliente WHERE ID = " + ClienteID;
                retorno = Convert.ToInt32(bd.ConsultaValor(sql));
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Buscar o Cliente. " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int[] BuscaClientePelaSenha(string SenhaVendaBilheteria, string CPF, int EventoID)
        {
            try
            {
                int ClienteID = 0;
                int VendaBilheteriaID = 0;
                int[] retorno = new int[2];

                string sql = @"SELECT DISTINCT tVendaBilheteria.ID, tVendaBilheteria.ClienteID 
                FROM tVendaBilheteria 
                INNER JOIN tIngresso ON tIngresso.VendaBilheteriaID = tVendaBilheteria.ID
                INNER JOIN tCliente ON tCliente.ID = tVendaBilheteria.ClienteID
                WHERE tVendaBilheteria.Senha = '" + SenhaVendaBilheteria + "' AND tIngresso.EventoID = " + EventoID
                + " AND tCliente.CPF = '" + CPF + "' AND tVendaBilheteria.Status = 'P'";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    VendaBilheteriaID = bd.LerInt("ID");
                    ClienteID = bd.LerInt("ClienteID");
                }

                retorno[0] = VendaBilheteriaID;
                retorno[1] = ClienteID;

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Buscar o Cliente. " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int[] BuscarClienteAssinatura(string Login, string Senha, IRLib.Paralela.Utils.Enums.EnumTipoAssinatura tipoAssinatura)
        {
            try
            {
                Login = Login.Replace("'", "''");
                Senha = Senha.Replace("'", "''");

                string sql = string.Empty;

                if (tipoAssinatura == Utils.Enums.EnumTipoAssinatura.OSESP)
                {
                    string filtro = string.Empty;
                    if (Utilitario.IsEmail(Login))
                        filtro = "Email = '" + Login + "'";
                    else if (Utilitario.IsCPF(Login) || Utilitario.IsCNPJ(Login))
                        filtro = " (CPF = '" + Login + "' OR CNPJ = '" + Login + "')";
                    else
                        filtro = "LoginOSESP = '" + Login + "'";


                    sql = string.Format(@"
                        SELECT TOP 1 
                                ID, Nome, StatusAtual, Senha, Email FROM tCliente (NOLOCK) 
                            WHERE {0}
                        ", filtro);
                }
                else if (tipoAssinatura == Utils.Enums.EnumTipoAssinatura.Filarmonica)
                {
                    sql = string.Format(@"
                        SELECT TOP 1 
                                ID, Nome, StatusAtual, Senha, Email FROM tCliente (NOLOCK) 
                            WHERE (Nome LIKE '{0}%' AND (CPF = '{1}' OR CNPJ = '{1}') )
                        ", Login, Senha);
                }
                else
                    sql = string.Format(@"
                        SELECT TOP 1 
                                ID, Nome, StatusAtual, Senha, Email FROM tCliente (NOLOCK) 
                            WHERE (Nome LIKE '{0}%' AND CPF = '{1}')
                        ", Login, Senha);

                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.Nome.Valor = bd.LerString("Nome");
                    this.StatusAtual.Valor = bd.LerString("StatusAtual");
                    this.Senha.Valor = bd.LerString("Senha");
                    this.Email.Valor = bd.LerString("Email");

                    if (this.StatusAtual.Valor.ToUpper() == "B")
                        // Cliente Bloqueado
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteBloqueado, this.Control.ID);

                    if (this.Senha.Valor == "" || this.Senha.Valor == null)
                        // Senha Vazia
                        return MontaRetornoWeb((int)Cliente.Infos.ClienteSemSenha, this.Control.ID);

                    if (string.Compare(this.Senha.Valor, Senha, true) == 0 || tipoAssinatura == Utils.Enums.EnumTipoAssinatura.Filarmonica)
                        // Senha Correta
                        return MontaRetornoWeb((int)Cliente.Infos.Sucesso, this.Control.ID);
                    else
                    {
                        if (!this.Ativo.Valor)
                            //Usuario não ativou cadastro
                            return MontaRetornoWeb((int)Cliente.Infos.NaoAtivado, this.Control.ID);
                        else
                            //senha inválida
                            return MontaRetornoWeb((int)Cliente.Infos.InfoIncorreta, this.Control.ID);
                    }
                }
                else
                    return MontaRetornoWeb((int)Cliente.Infos.ClienteInexistente, 0);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IRLib.Paralela.Assinaturas.Models.Cliente BuscaClienteAssinatura(int ClienteID)
        {
            try
            {
                IRLib.Paralela.Assinaturas.Models.Cliente ClienteAssinatura = new Assinaturas.Models.Cliente();

                this.Ler(ClienteID);

                ClienteAssinatura.ID = this.Control.ID;
                ClienteAssinatura.Login = this.LoginOsesp.Valor;
                ClienteAssinatura.Senha = this.Senha.Valor;
                ClienteAssinatura.ConfirmacaoSenha = this.Senha.Valor;
                ClienteAssinatura.Nome = this.Nome.Valor;
                ClienteAssinatura.DataNascimento = this.DataNascimento.Valor.ToString("dd/MM/yyyy");

                if (this.Sexo.Valor == "M")
                    ClienteAssinatura.Sexo = "Masculino";
                else
                    ClienteAssinatura.Sexo = "Feminino";

                ClienteAssinatura.CPF = this.CPF.Valor;
                ClienteAssinatura.CNPJ = this.CNPJ.Valor;
                ClienteAssinatura.TipoCadastro = this.CNPJ.Valor.Length > 0 ? "Pessoa Jurídica" : "Pessoa Física";
                ClienteAssinatura.NomeFantasia = this.NomeFantasia.Valor;
                ClienteAssinatura.RazaoSocial = this.RazaoSocial.Valor;

                ClienteAssinatura.Endereco = this.EnderecoCliente.Valor;
                ClienteAssinatura.EnderecoNumero = this.NumeroCliente.Valor;
                ClienteAssinatura.Complemento = this.ComplementoCliente.Valor;
                ClienteAssinatura.Bairro = this.BairroCliente.Valor;

                ClienteAssinatura.EstadoID = new Estado().GetEstadoID(this.EstadoCliente.Valor);
                ClienteAssinatura.CidadeID = new Cidade().GetCidadeID(this.CidadeCliente.Valor, this.EstadoCliente.Valor);

                ClienteAssinatura.Cep = this.CEPCliente.Valor;
                ClienteAssinatura.Email = this.Email.Valor;
                ClienteAssinatura.DDDResidencial = this.DDDTelefone.Valor;
                ClienteAssinatura.TelResidencial = this.Telefone.Valor;
                ClienteAssinatura.DDDTelCelular = this.DDDCelular.Valor;
                ClienteAssinatura.TelCelular = this.Celular.Valor;
                ClienteAssinatura.DDDTelComercial1 = this.DDDTelefoneComercial.Valor;
                ClienteAssinatura.TelComercial1 = this.TelefoneComercial.Valor;
                ClienteAssinatura.DDDTelComercial2 = this.DDDTelefoneComercial2.Valor;
                ClienteAssinatura.TelComercial2 = this.TelefoneComercial2.Valor;
                ClienteAssinatura.Profissao = this.Profissao.Valor;

                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();
                situacaoProfissional.Ler(this.SituacaoProfissionalID.Valor);

                ClienteAssinatura.SituacaoProfissional = situacaoProfissional.Situacao.Valor;

                return ClienteAssinatura;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EstruturaRetornoProcSalvarCliente SalvarClienteAssinatura(IRLib.Paralela.Assinaturas.Models.Cliente estruturaCliente)
        {
            if (estruturaCliente == null)
                throw new ArgumentNullException("estruturaCliente", "Estrutura vazia.");

            try
            {
                bd.IniciarTransacao();

                EstruturaRetornoProcSalvarCliente retornoProc = new EstruturaRetornoProcSalvarCliente();

                if (estruturaCliente.ID > 0)
                    this.Ler(estruturaCliente.ID);

                if (string.IsNullOrEmpty(this.LoginOsesp.Valor))
                {
                    string LoginGerado = string.Empty;
                    var Nomes = estruturaCliente.Nome.Trim().Split(' ').Where(c => c.Length >= 2).ToList();
                    Random rnd = new Random(DateTime.Now.Millisecond);

                    for (int cont = 0; cont < Nomes.Count() || cont < 2; cont++)
                        LoginGerado += Nomes[cont].Substring(0, 2);

                    LoginGerado += rnd.Next(9999).ToString();


                    this.LoginOsesp.Valor = LoginGerado;
                }

                this.Nome.Valor = estruturaCliente.Nome.Trim();
                this.CPF.Valor = estruturaCliente.CPF;
                this.CNPJ.Valor = estruturaCliente.CNPJ;
                this.RazaoSocial.Valor = estruturaCliente.RazaoSocial;
                this.NomeFantasia.Valor = estruturaCliente.NomeFantasia;

                if (!string.IsNullOrEmpty(estruturaCliente.Senha))
                    this.Senha.Valor = estruturaCliente.Senha.Trim(); ;

                this.DataNascimento.Valor = Convert.ToDateTime(estruturaCliente.DataNascimento);

                if (estruturaCliente.Sexo.ToUpper().Equals("MASCULINO"))
                    this.Sexo.Valor = "M";
                else
                    this.Sexo.Valor = "F";

                this.EnderecoCliente.Valor = estruturaCliente.Endereco.Trim();
                this.NumeroCliente.Valor = estruturaCliente.EnderecoNumero;

                if (!string.IsNullOrEmpty(estruturaCliente.Complemento))
                    this.ComplementoCliente.Valor = estruturaCliente.Complemento.Trim();
                else
                    this.ComplementoCliente.Valor = "";

                this.BairroCliente.Valor = estruturaCliente.Bairro.Trim();

                Cidade cidade = new Cidade();
                cidade.Ler(estruturaCliente.CidadeID);
                this.CidadeCliente.Valor = cidade.Nome.Valor;

                Estado estado = new Estado();
                estado.Ler(estruturaCliente.EstadoID);
                this.EstadoCliente.Valor = estado.Sigla.Valor;

                this.CEPCliente.Valor = estruturaCliente.Cep;
                this.Email.Valor = (estruturaCliente.Email ?? string.Empty).Trim();
                this.DDDTelefone.Valor = estruturaCliente.DDDResidencial;
                this.Telefone.Valor = estruturaCliente.TelResidencial;
                this.DDDCelular.Valor = estruturaCliente.DDDTelCelular;
                this.Celular.Valor = estruturaCliente.TelCelular;
                this.DDDTelefoneComercial.Valor = estruturaCliente.DDDTelComercial1;
                this.TelefoneComercial.Valor = estruturaCliente.TelComercial1;
                this.DDDTelefoneComercial2.Valor = estruturaCliente.DDDTelComercial2;
                this.TelefoneComercial2.Valor = estruturaCliente.TelComercial2;

                if (!string.IsNullOrEmpty(estruturaCliente.Profissao))
                    this.Profissao.Valor = estruturaCliente.Profissao.Trim();
                else
                    this.Profissao.Valor = "";

                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();

                int situacao = situacaoProfissional.BuscarIDPeloNome(estruturaCliente.SituacaoProfissional);

                if (situacao > 0)
                    this.SituacaoProfissionalID.Valor = situacao;

                if (this.Control.ID > 0)
                {
                    if (this.Atualizar())
                    {
                        retornoProc.ClienteID = this.Control.ID;
                        retornoProc.RetornoProcedure = RetornoProcSalvar.UpdateOK;
                    }
                    else
                    {
                        retornoProc.ClienteID = 0;
                        retornoProc.RetornoProcedure = RetornoProcSalvar.CPF_Email_JaExistem;
                    }
                }
                else
                {
                    retornoProc = this.Salvar(UsuarioSiteID);
                }

                if (retornoProc.RetornoProcedure == RetornoProcSalvar.InsertOK || retornoProc.RetornoProcedure == RetornoProcSalvar.UpdateOK)
                {
                    this.Control.ID = retornoProc.ClienteID;

                    ClienteEndereco oClienteEndereco = new ClienteEndereco();

                    oClienteEndereco.Nome.Valor = estruturaCliente.Nome;
                    oClienteEndereco.CPF.Valor = estruturaCliente.CPF;
                    oClienteEndereco.ClienteID.Valor = this.Control.ID;
                    oClienteEndereco.Endereco.Valor = estruturaCliente.Endereco;
                    oClienteEndereco.Numero.Valor = estruturaCliente.EnderecoNumero;
                    oClienteEndereco.Complemento.Valor = estruturaCliente.Complemento;
                    oClienteEndereco.Bairro.Valor = estruturaCliente.Bairro;
                    oClienteEndereco.Cidade.Valor = cidade.Nome.Valor;
                    oClienteEndereco.Endereco.Valor = estruturaCliente.Endereco;
                    oClienteEndereco.CEP.Valor = estruturaCliente.Cep;
                    oClienteEndereco.Estado.Valor = estado.Sigla.Valor;
                    oClienteEndereco.EnderecoPrincipal.Valor = true;
                    oClienteEndereco.EnderecoTipoID.Valor = 1;

                    int ClienteEnderecoID = oClienteEndereco.VerificaEnderecoCliente(retornoProc.ClienteID, estruturaCliente.Cep);

                    if (ClienteEnderecoID == 0)
                        oClienteEndereco.Inserir();
                    else
                    {
                        oClienteEndereco.Control.ID = ClienteEnderecoID;
                        oClienteEndereco.Atualizar();
                    }
                }

                bd.FinalizarTransacao();

                return retornoProc;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
        }

        public void EsqueciSenha(string caminho, string identificacao)
        {
            try
            {
                int clienteID = Convert.ToInt32(bd.ConsultaValor(
                    string.Format("SELECT TOP 1 ID FROM tCliente (NOLOCK) WHERE (CPF = '{0}' OR Email = '{0}' OR LoginOSESP = '{0}' )", identificacao)));

                if (clienteID == 0)
                    throw new Exception("Identificação incorreta, não foi possível encontrar o seu cadastro.");

                this.Ler(clienteID);
                if (this.Email.Valor.Length == 0)
                    throw new Exception("Desculpe, seu cadastro não possui email associado.");

                if (this.Senha.Valor.Length == 0)
                {
                    this.Senha.Valor = new Random().Next(10000, 999999).ToString();
                    this.Salvar(IRLib.Paralela.Usuario.INTERNET_USUARIO_ID);
                }

                ServicoEmailParalela.EnviarNovaSenha(this.Nome.Valor, this.Senha.Valor, this.Email.Valor);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LerClienteBoleto(int boletoID)
        {
            try
            {

                bd.FecharConsulta();
                bd.Consulta(@"SELECT tCliente.ID 
                            FROM tCliente (NOLOCK)
                            INNER JOIN tVendaBilheteria ON tCliente.ID=tVendaBilheteria.ClienteID
                            INNER JOIN tVendaBilheteriaFormaPagamento ON tVendaBilheteria.ID=tVendaBilheteriaFormaPagamento.VendaBilheteriaID
                            INNER JOIN tVendaBilheteriaFormaPagamentoBoleto ON tVendaBilheteriaFormaPagamento.ID=tVendaBilheteriaFormaPagamentoBoleto.VendaBilheteriaFormaPagamentoID
                            WHERE tVendaBilheteriaFormaPagamentoBoleto.ID = '" + boletoID + "' ");

                if (bd.Consulta().Read())
                {
                    this.Ler(bd.LerInt("ID"));
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        public bool VerificaAssinante(int clienteID)
        {
            try
            {
                bool retorno = false;
                string sql = @"select top 1 ID
                from tAssinaturaCliente
                where ClienteID = " + clienteID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Buscar o Cliente. " + ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public EstruturaCliente RetornoClienteMobile(int ClienteID)
        {
            try
            {
                EstruturaCliente retorno = new EstruturaCliente();

                this.Ler(ClienteID);

                retorno.Ativo = this.Ativo.Valor;
                retorno.BairroCliente = this.BairroCliente.Valor;
                retorno.CarteiraEstudante = this.CarteiraEstudante.Valor;
                retorno.CEPCliente = this.CEPCliente.Valor;
                retorno.CidadeCliente = this.CidadeCliente.Valor;
                retorno.ContatoTipoID = this.ContatoTipoID.Valor;
                retorno.CNPJ = this.CNPJ.Valor;
                retorno.CPF = this.CPF.Valor;
                retorno.CPFResponsavel = this.CPFResponsavel.Valor;
                retorno.DataNascimento = this.DataNascimento.Valor;
                retorno.Email = this.Email.Valor;
                retorno.EnderecoCliente = this.EnderecoCliente.Valor;
                retorno.EnderecoComplementoCliente = this.ComplementoCliente.Valor;
                retorno.EnderecoNumeroCliente = this.NumeroCliente.Valor;
                retorno.EstadoCliente = this.NumeroCliente.Valor;
                retorno.ID = this.Control.ID;
                retorno.InscricaoEstadual = this.InscricaoEstadual.Valor;
                retorno.Nome = this.Nome.Valor;
                retorno.NomeFantasia = this.NomeFantasia.Valor;
                retorno.Observacao = this.Obs.Valor;
                retorno.Pais = this.Pais.Valor;
                retorno.RazaoSocial = this.RazaoSocial.Valor;
                retorno.ReceberEmail = this.RecebeEmail.Valor.ToString();
                retorno.RG = this.RG.Valor;
                retorno.Sexo = this.Sexo.Valor;
                retorno.StatusAtual = this.StatusAtual.Valor;
                retorno.TelefoneCelular = this.Celular.Valor;
                retorno.TelefoneCelularDDD = this.DDDCelular.Valor;
                retorno.TelefoneComercial = this.TelefoneComercial.Valor;
                retorno.TelefoneComercialDDD = this.DDDTelefoneComercial.Valor;
                retorno.TelefoneResidencial = this.Telefone.Valor;
                retorno.TelefoneResidencialDDD = this.DDDTelefone.Valor;
                retorno.Senha = this.Senha.Valor;

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Buscar o Cliente. " + ex.Message);
            }
        }
    } // fim de classe

    public class ClienteLista : ClienteLista_B
    {
        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tCliente (NOLOCK)";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCliente (NOLOCK)";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tCliente (NOLOCK)";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCliente (NOLOCK)";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ClienteID
        {
            get
            {
                if (this.lista.Count > 0)
                    return (int)this.lista[0];
                else
                    return 0;
            }
        }

        public ClienteLista() { }

        public ClienteLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Método usado para a pesquisa de clientes.
        /// </summary>
        /// <param name="maxRegistros">O número de registros máximo que o método deve trazer. 0 (zero) para ilimitado</param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<EstruturaIDNome> CarregaIDNome(int maxRegistros, string filtro, bool registroZero)
        {
            try
            {

                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();

                EstruturaIDNome idNomeItem;
                string top;

                if (maxRegistros == 0)
                    top = "";
                else
                    top = "TOP " + maxRegistros;

                string sql = @"SELECT " + top + " ID,CASE WHEN CNPJ IS NULL OR CNPJ = '' THEN Nome ELSE NomeFantasia COLLATE Latin1_General_CI_AI END AS Nome FROM tCliente (NOLOCK) WHERE " + filtro;

                bd.Consulta(sql);

                if (registroZero)
                    retorno.Add(new EstruturaIDNome { ID = 0, Nome = "Selecione..." });

                while (bd.Consulta().Read())
                {
                    idNomeItem = new EstruturaIDNome();
                    idNomeItem.ID = bd.LerInt("ID");
                    idNomeItem.Nome = bd.LerString("Nome");
                    retorno.Add(idNomeItem);
                }
                return retorno;

            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        ///  Obtem uma tabela de nomes e ids de Cliente carregados na lista
        /// </summary>
        /// <returns></returns>
        public DataTable TabelaSimples()
        {

            try
            {

                DataTable tabela = new DataTable("Cliente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cliente.Control.ID;
                        linha["Nome"] = cliente.Nome.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable DistinctTabelaDisponivelAjuste()
        {
            try
            {
                DataTable tabela = new DataTable("Evento");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                DataRow linha;
                string sql;

                sql = "SELECT DISTINCT tEvento.ID, tEvento.Nome FROM tEvento(NOLOCK), tApresentacao(NOLOCK) WHERE EventoID = tEvento.ID AND DisponivelAjuste = 'T' ORDER BY Nome";

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable TabelaClientesPorEvento(DataTable TabelaSelecionados, bool email, bool endereco, bool telefone, bool recebeEmail, string localTipo)
        {
            try
            {
                DataTable tabela = new DataTable("Clientes");
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("CarteiraEstudante", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDTelefoneComercial", typeof(string));
                tabela.Columns.Add("TelefoneComercial", typeof(string));
                tabela.Columns.Add("DDDCelular", typeof(string));
                tabela.Columns.Add("Celular", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("CEPCliente", typeof(string));
                tabela.Columns.Add("EnderecoCliente", typeof(string));
                tabela.Columns.Add("NumeroCliente", typeof(string));
                tabela.Columns.Add("CidadeCliente", typeof(string));
                tabela.Columns.Add("EstadoCliente", typeof(string));
                tabela.Columns.Add("ComplementoCliente", typeof(string));
                tabela.Columns.Add("BairroCliente", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("RecebeEmail", typeof(string));
                tabela.Columns.Add("ContatoTipo", typeof(string));


                DataRow linha;
                StringBuilder sql = new StringBuilder();
                string filtroEmail;
                string filtroEndereco;
                string filtroTelefone;
                string filtroRecebeEmail;
                string filtroTipo;

                if (email == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0)
                        filtroEmail = " AND Email != '' ";
                    else
                        filtroEmail = " Email != '' ";
                }
                else
                {
                    filtroEmail = "";
                }

                if (telefone == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email)
                        filtroTelefone = " AND (Telefone != '' OR TelefoneComercial != '' OR Celular != '') ";
                    else
                        filtroTelefone = " (Telefone != '' OR TelefoneComercial != '' OR Celular != '') ";
                }
                else
                {
                    filtroTelefone = "";
                }

                if (endereco == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone)
                        filtroEndereco = " AND EnderecoCliente != '' ";
                    else
                        filtroEndereco = " EnderecoCliente != '' ";
                }
                else
                {
                    filtroEndereco = "";
                }

                if (recebeEmail == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone || endereco)
                        filtroRecebeEmail = " AND RecebeEmail = 'T' ";
                    else
                        filtroRecebeEmail = " RecebeEmail = 'T' ";
                }
                else
                {
                    filtroRecebeEmail = "";
                }

                if (localTipo != "")
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone || endereco || recebeEmail)
                        filtroTipo = " AND EventoTipoID = " + localTipo;
                    else
                        filtroTipo = " EventoTipoID = " + localTipo;
                }
                else
                {
                    filtroTipo = "";
                }

                sql.Append("SELECT " +
                      "DISTINCT tc.Nome,[RG],[CPF],[CarteiraEstudante],[Sexo],[DDDTelefone], " +
                      "[Telefone],[DDDTelefoneComercial],[TelefoneComercial],tc.DDDCelular " +
                      ",tc.Celular,tc.DataNascimento,[Email],[CEPCliente],[EnderecoCliente] " +
                      ",[NumeroCliente],[CidadeCliente],[EstadoCliente],[ComplementoCliente],[BairroCliente], [Pais],[RecebeEmail] , ISNULL(cp.Nome, '') AS ContatoTipo " +
                      "FROM " +
                      "tCliente tc " +
                      "INNER JOIN tVendaBilheteria tvb ON tvb.ClienteID=tc.ID " +
                      "INNER JOIN tIngresso ti ON ti.VendaBilheteriaID=tvb.ID " +
                      "INNER JOIN tEvento te ON te.ID=ti.EventoID " +
                      "LEFT JOIN tContatoTipo cp (NOLOCK) ON cp.ID = tc.ContatoTipoID " +
                      "WHERE ");

                for (int i = 0; i < TabelaSelecionados.Rows.Count; i++)
                {
                    if (i == 0)
                        sql.Append(" ti.EventoID = " + TabelaSelecionados.Rows[i]["ID"].ToString());

                    else
                        sql.Append(" OR ti.EventoID = " + TabelaSelecionados.Rows[i]["ID"].ToString());
                }

                sql.Append(filtroEmail + filtroTelefone + filtroEndereco + filtroRecebeEmail + filtroTipo);

                lista.Clear();

                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();

                    linha["Nome"] = bd.LerString("Nome");
                    linha["RG"] = bd.LerString("RG");
                    linha["CPF"] = bd.LerString("CPF");
                    linha["CarteiraEstudante"] = bd.LerString("CarteiraEstudante");
                    linha["Sexo"] = bd.LerString("Sexo");
                    linha["DDDTelefone"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["DDDTelefoneComercial"] = bd.LerString("DDDTelefoneComercial");
                    linha["TelefoneComercial"] = bd.LerString("TelefoneComercial");
                    linha["DDDCelular"] = bd.LerString("DDDCelular");
                    linha["Celular"] = bd.LerString("Celular");
                    linha["DataNascimento"] = bd.LerStringFormatoData("DataNascimento");
                    linha["Email"] = bd.LerString("Email");
                    linha["CEPCliente"] = bd.LerString("CEPCliente");
                    linha["EnderecoCliente"] = bd.LerString("EnderecoCliente");
                    linha["NumeroCliente"] = bd.LerString("NumeroCliente");
                    linha["CidadeCliente"] = bd.LerString("CidadeCliente");
                    linha["EstadoCliente"] = bd.LerString("EstadoCliente");
                    linha["ComplementoCliente"] = bd.LerString("ComplementoCliente");
                    linha["BairroCliente"] = bd.LerString("BairroCliente");
                    linha["Pais"] = bd.LerString("Pais");
                    linha["RecebeEmail"] = bd.LerString("RecebeEmail") == "T" ? "Sim" : "Não";
                    linha["ContatoTipo"] = bd.LerString("ContatoTipo");

                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public DataTable TabelaClientesPorEvento(DataTable TabelaSelecionados, bool email, bool endereco, bool telefone, bool recebeEmail, string localTipo, int apresentacaoID)
        {
            try
            {
                DataTable tabela = new DataTable("Clientes");
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("RG", typeof(string));
                tabela.Columns.Add("CPF", typeof(string));
                tabela.Columns.Add("CarteiraEstudante", typeof(string));
                tabela.Columns.Add("Sexo", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("DDDTelefoneComercial", typeof(string));
                tabela.Columns.Add("TelefoneComercial", typeof(string));
                tabela.Columns.Add("DDDCelular", typeof(string));
                tabela.Columns.Add("Celular", typeof(string));
                tabela.Columns.Add("DataNascimento", typeof(string));
                tabela.Columns.Add("Email", typeof(string));
                tabela.Columns.Add("CEPCliente", typeof(string));
                tabela.Columns.Add("EnderecoCliente", typeof(string));
                tabela.Columns.Add("NumeroCliente", typeof(string));
                tabela.Columns.Add("CidadeCliente", typeof(string));
                tabela.Columns.Add("EstadoCliente", typeof(string));
                tabela.Columns.Add("ComplementoCliente", typeof(string));
                tabela.Columns.Add("BairroCliente", typeof(string));
                tabela.Columns.Add("Pais", typeof(string));
                tabela.Columns.Add("RecebeEmail", typeof(string));
                tabela.Columns.Add("ContatoTipo", typeof(string));

                DataRow linha;
                StringBuilder sql = new StringBuilder();

                string filtroEmail;
                string filtroEndereco;
                string filtroTelefone;
                string filtroRecebeEmail;
                string filtroTipo;
                string filtroHorario = " AND ta.ID = '" + apresentacaoID + "'";

                if (email == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0)
                        filtroEmail = " AND Email != '' ";
                    else
                        filtroEmail = " Email != '' ";
                }
                else
                {
                    filtroEmail = "";
                }

                if (telefone == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email)
                        filtroTelefone = " AND (Telefone != '' OR TelefoneComercial != '' OR Celular != '') ";
                    else
                        filtroTelefone = " (Telefone != '' OR TelefoneComercial != '' OR Celular != '') ";
                }
                else
                {
                    filtroTelefone = "";
                }

                if (endereco == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone)
                        filtroEndereco = " AND EnderecoCliente != '' ";
                    else
                        filtroEndereco = " EnderecoCliente != '' ";
                }
                else
                {
                    filtroEndereco = "";
                }

                if (recebeEmail == true)
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone || endereco)
                        filtroRecebeEmail = " AND RecebeEmail = 'T' ";
                    else
                        filtroRecebeEmail = " RecebeEmail = 'T' ";
                }
                else
                {
                    filtroRecebeEmail = "";
                }

                if (localTipo != "")
                {
                    if (TabelaSelecionados.Rows.Count > 0 || email || telefone || endereco || recebeEmail)
                        filtroTipo = " AND EventoTipoID = " + localTipo;
                    else
                        filtroTipo = " EventoTipoID = " + localTipo;
                }
                else
                {
                    filtroTipo = "";
                }

                sql.Append("SELECT " +
                      "DISTINCT tc.Nome,[RG],[CPF],[CarteiraEstudante],[Sexo],[DDDTelefone], " +
                      "[Telefone],[DDDTelefoneComercial],[TelefoneComercial],[DDDCelular] " +
                      ",[Celular],[DataNascimento],[Email],[CEPCliente],[EnderecoCliente] " +
                      ",[NumeroCliente],[CidadeCliente],[EstadoCliente],[ComplementoCliente],[BairroCliente], [Pais],[RecebeEmail] , ISNULL(cp.Nome, '') as ContatoTipo  " +
                      "FROM " +
                      "tCliente tc " +
                      "INNER JOIN tVendaBilheteria tvb (NOLOCK) ON tvb.ClienteID=tc.ID " +
                      "INNER JOIN tIngresso ti (NOLOCK) ON ti.VendaBilheteriaID=tvb.ID " +
                      "INNER JOIN tEvento te (NOLOCK) ON te.ID=ti.EventoID " +
                      "INNER JOIN tApresentacao ta (NOLOCK) ON ti.ApresentacaoID=ta.ID " +
                      "LEFT JOIN tContatoTipo cp (NOLOCK) ON cp.ID = tc.ContatoTipoID " +
                      "WHERE ");

                for (int i = 0; i < TabelaSelecionados.Rows.Count; i++)
                {
                    if (i == 0)
                        sql.Append(" ti.EventoID = " + TabelaSelecionados.Rows[i]["ID"].ToString());

                    else
                        sql.Append(" OR ti.EventoID = " + TabelaSelecionados.Rows[i]["ID"].ToString());
                }

                sql.Append(filtroTelefone + filtroEndereco + filtroEmail + filtroRecebeEmail + filtroTipo + filtroHorario);

                lista.Clear();

                bd.Consulta(sql.ToString());

                while (bd.Consulta().Read())
                {
                    linha = tabela.NewRow();
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RG"] = bd.LerString("RG");
                    linha["CPF"] = bd.LerString("CPF");
                    linha["CarteiraEstudante"] = bd.LerString("CarteiraEstudante");
                    linha["Sexo"] = bd.LerString("Sexo");
                    linha["DDDTelefone"] = bd.LerString("DDDTelefone");
                    linha["Telefone"] = bd.LerString("Telefone");
                    linha["DDDTelefoneComercial"] = bd.LerString("DDDTelefoneComercial");
                    linha["TelefoneComercial"] = bd.LerString("TelefoneComercial");
                    linha["DDDCelular"] = bd.LerString("DDDCelular");
                    linha["Celular"] = bd.LerString("Celular");
                    linha["DataNascimento"] = bd.LerString("DataNascimento");
                    linha["Email"] = bd.LerString("Email");
                    linha["CEPCliente"] = bd.LerString("CEPCliente");
                    linha["EnderecoCliente"] = bd.LerString("EnderecoCliente");
                    linha["NumeroCliente"] = bd.LerString("NumeroCliente");
                    linha["CidadeCliente"] = bd.LerString("CidadeCliente");
                    linha["EstadoCliente"] = bd.LerString("EstadoCliente");
                    linha["ComplementoCliente"] = bd.LerString("ComplementoCliente");
                    linha["BairroCliente"] = bd.LerString("BairroCliente");
                    linha["Pais"] = bd.LerString("Pais");
                    linha["RecebeEmail"] = bd.LerString("RecebeEmail") == "T" ? "Sim" : "Não";
                    linha["ContatoTipo"] = bd.LerString("ContatoTipo");

                    tabela.Rows.Add(linha);
                }

                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    #region "ClienteException"

    [Serializable]
    public class ClienteException : ClienteException_B
    {
        private Cliente.Infos codigoErro = Cliente.Infos.ErroIndefinido; // -1 não contém erro.
        private int id;
        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public Cliente.Infos CodigoErro
        {
            get { return this.codigoErro; }
            set { this.codigoErro = value; }
        }

        public ClienteException() : base() { }

        public ClienteException(string msg) : base(msg) { }
        public ClienteException(string msg, Cliente.Infos codigoErro)
            : base(msg)
        {
            this.CodigoErro = codigoErro;
        }
        public ClienteException(string msg, Cliente.Infos codigoErro, int ID)
            : base(msg)
        {
            this.CodigoErro = codigoErro;
            this.ID = ID;
        }

        public ClienteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }


    }


    [Serializable]
    public class ClienteException_B : Exception
    {

        public ClienteException_B() : base() { }

        public ClienteException_B(string msg) : base(msg) { }

        public ClienteException_B(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }


    #endregion

}
