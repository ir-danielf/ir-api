using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Assinaturas
{
    public class ImportacaoOSESP
    {
        public void ImportarClientes()
        {
            BD bd = new BD();
            try
            {
                var obj = new
                {
                    ClienteID = 0,
                    Login = string.Empty,
                    Nome = string.Empty,
                    Senha = string.Empty,
                    CPF = string.Empty,
                    Sexo = string.Empty,
                    CEP = string.Empty,
                    Endereco = string.Empty,
                    Numero = string.Empty,
                    Complemento = string.Empty,
                    Estado = string.Empty,
                    Cidade = string.Empty,
                    Bairro = string.Empty,
                    Email = string.Empty,
                    DDDResidencial = string.Empty,
                    TelResidencial = string.Empty,
                    DDDCelular = string.Empty,
                    TelCelular = string.Empty,
                    DDDComercial1 = string.Empty,
                    TelComercial1 = string.Empty,
                    DDDComercial2 = string.Empty,
                    TelComercial2 = string.Empty,
                    DDDFax = string.Empty,
                    TelFax = string.Empty,
                    Profissao = string.Empty,
                    ProfissaoSituacao = string.Empty,
                    DataNascimento = DateTime.MinValue,
                    ValorDesconto = 0m,
                    TextoDesconto = string.Empty,
                };

                var listaAnonima = VendaBilheteria.ToAnonymousList(obj);

                bd.Consulta("SELECT * FROM ClientesOSESP WHERE Importado = 0");

                if (!bd.Consulta().Read())
                    throw new Exception("Todos os clientes foram importados.");

                var listaSitProfissional = new SituacaoProfissional().ListaTudo();

                do
                {
                    string[] residencial = bd.LerString("Tel_Residencial").Replace("-", "").Split(' ');
                    string[] celular = bd.LerString("Tel_Celular").Replace("-", "").Split(' ');
                    string[] comercial1 = bd.LerString("Tel_Comercial1").Replace("-", "").Split(' ');
                    string[] comercial2 = bd.LerString("Tel_Comercial2").Replace("-", "").Split(' ');
                    string[] fax = bd.LerString("Tel_Fax").Replace("-", "").Split(' ');
                    string sexo = bd.LerString("Sexo");
                    sexo = sexo.Length == 0 ? string.Empty : sexo.Substring(0, 1);


                    listaAnonima.Add(new
                    {
                        ClienteID = bd.LerInt("ClienteID"),
                        Login = bd.LerString("Login").Replace("'", ""),
                        Nome = bd.LerString("Nome").Replace("'", ""),
                        Senha = bd.LerString("Senha").Replace("'", ""),
                        CPF = bd.LerString("CPF").Replace("'", ""),
                        Sexo = sexo,
                        CEP = bd.LerString("CEP").Replace("'", ""),
                        Endereco = bd.LerString("Endereco").Replace("'", ""),
                        Numero = bd.LerString("Numero").Replace("'", ""),
                        Complemento = bd.LerString("Complemento").Replace("'", ""),
                        Estado = bd.LerString("Estado").Replace("'", ""),
                        Cidade = bd.LerString("Cidade").Replace("'", ""),
                        Bairro = bd.LerString("Bairro").Replace("'", ""),
                        Email = bd.LerString("Email").Replace("'", ""),
                        DDDResidencial = residencial.Length > 1 ? residencial[0] : string.Empty,
                        TelResidencial = residencial.Length > 1 ? residencial[1] : string.Empty,
                        DDDCelular = celular.Length > 1 ? celular[0] : string.Empty,
                        TelCelular = celular.Length > 1 ? celular[1] : string.Empty,
                        DDDComercial1 = comercial1.Length > 1 ? comercial1[0] : string.Empty,
                        TelComercial1 = comercial1.Length > 1 ? comercial1[1] : string.Empty,
                        DDDComercial2 = comercial2.Length > 1 ? comercial2[0] : string.Empty,
                        TelComercial2 = comercial2.Length > 1 ? comercial2[1] : string.Empty,
                        DDDFax = fax.Length > 1 ? fax[0] : string.Empty,
                        TelFax = fax.Length > 1 ? fax[1] : string.Empty,
                        Profissao = bd.LerString("Profissao").Replace("'", ""),
                        ProfissaoSituacao = bd.LerString("ProfissaoSituacao").Replace("'", ""),
                        DataNascimento = bd.LerDateTime("Data_Nascimento"),
                        ValorDesconto = bd.LerDecimal("ValorDesconto"),
                        TextoDesconto = bd.LerString("TextoDesconto").Replace("'", ""),
                    });

                } while (bd.Consulta().Read());

                IRLib.Cliente oCliente = new Cliente();
                foreach (var cliente in listaAnonima)
                {
                    try
                    {
                        oCliente.Limpar();
                        if (cliente.ClienteID > 0)
                            oCliente.Ler(cliente.ClienteID);

                        oCliente.LoginOsesp.Valor = cliente.Login;
                        oCliente.Nome.Valor = cliente.Nome;
                        oCliente.Senha.Valor = cliente.Senha;
                        oCliente.CPF.Valor = cliente.CPF;
                        oCliente.Sexo.Valor = cliente.Sexo;
                        oCliente.CEPCliente.Valor = cliente.CEP;
                        oCliente.EnderecoCliente.Valor = cliente.Endereco;
                        oCliente.NumeroCliente.Valor = cliente.Numero;
                        oCliente.ComplementoCliente.Valor = cliente.Complemento;
                        oCliente.EstadoCliente.Valor = cliente.Estado;
                        oCliente.CidadeCliente.Valor = cliente.Cidade;
                        oCliente.BairroCliente.Valor = cliente.Bairro;
                        oCliente.Email.Valor = cliente.Email.Split(',').FirstOrDefault().Trim();
                        oCliente.DDDTelefone.Valor = cliente.DDDResidencial;
                        oCliente.Telefone.Valor = cliente.TelResidencial;
                        oCliente.DDDCelular.Valor = cliente.DDDCelular;
                        oCliente.Celular.Valor = cliente.TelCelular;
                        oCliente.DDDTelefoneComercial.Valor = cliente.DDDComercial1;
                        oCliente.TelefoneComercial.Valor = cliente.TelComercial1;
                        oCliente.DDDTelefoneComercial2.Valor = cliente.DDDComercial2;
                        oCliente.TelefoneComercial2.Valor = cliente.TelComercial2;
                        oCliente.Profissao.Valor = cliente.Profissao;
                        oCliente.SituacaoProfissionalID.Valor = listaSitProfissional.Where(c => string.Compare(c.Nome, cliente.ProfissaoSituacao, true) == 0).FirstOrDefault().ID;
                        oCliente.DataNascimento.Valor = cliente.DataNascimento;

                        var retorno = oCliente.Salvar(IRLib.Usuario.INTERNET_USUARIO_ID);

                        switch (retorno.RetornoProcedure)
                        {
                            case Cliente.RetornoProcSalvar.InsertOK:
                            case Cliente.RetornoProcSalvar.UpdateOK:
                                bd.Executar("UPDATE ClientesOSESP SET Importado = 1, ClienteID = " + retorno.ClienteID + " WHERE Login = '" + cliente.Login + "'");
                                break;
                            case Cliente.RetornoProcSalvar.CPF_Email_JaExistem:
                                throw new Exception("CPF/Email/Login já existe e houve tentativa de inclusão");
                            case Cliente.RetornoProcSalvar.CPF_Email_Vazios:
                                throw new Exception("CPF/Email/Login vazios");
                            case Cliente.RetornoProcSalvar.CNPJ_JaExiste:
                                throw new Exception("CNPJ já existem");
                            default:
                                break;
                        }

                        if (cliente.ValorDesconto > 0)
                            bd.Executar(
                                @"INSERT INTO tAssinaturaDesconto (ClienteID, Valor, Utilizado, TextoDesconto) 
								VALUES (" + retorno.ClienteID + ", " + cliente.ValorDesconto.ToString("#.00").Replace(",", ".") + ", 'F', '" + cliente.TextoDesconto + "')");

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            bd.Executar("UPDATE ClientesOSESP SET ErroImportacao = '" + ex.Message + "' WHERE Login = '" + cliente.Login + "'");
                        }
                        catch { }
                    }
                }

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Importar(int id)
        {
            BD bd = new BD();
            try
            {
                var obj = new
                {
                    ID = 0,
                    ClienteID = 0,
                    AssinaturaID = 0,
                    Lugares = new List<int>(),
                    AssinaturaAnoID = 0,
                    SetorID = 0,
                    BloqueioID = 0,
                    ExtintoBloqueioID = 0,
                };

                var listaAnonina = VendaBilheteria.ToAnonymousList(obj);

                string sql =
                    @"
						SELECT DISTINCT 
							    ac.UserId AS ID, ac.ClienteID, a.ID AS AssinaturaID, ac.LugarID, ac.LugarID1,ac.LugarID2,ac.LugarID3,ac.LugarID4,ac.LugarID5, an.ID AS AssinaturaAnoID, ac.SetorID,
								a.BloqueioID, a.ExtintoBloqueioID
							FROM AssinantesCultura ac (NOLOCK)
							INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
							INNER JOIN tAssinaturaAno an (NOLOCK) ON a.ID = an.AssinaturaID
							WHERE ac.SetorID IS NOT NULL AND ac.Importado = 0 AND a.ID = " + id;

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem mais assinaturas a serem importadas.");

                do
                {
                    List<int> lugares = new List<int>();
                    if (bd.LerInt("LugarID") > 0)
                        lugares.Add(bd.LerInt("LugarID"));

                    if (bd.LerInt("LugarID1") > 0)
                        lugares.Add(bd.LerInt("LugarID1"));

                    if (bd.LerInt("LugarID2") > 0)
                        lugares.Add(bd.LerInt("LugarID2"));

                    if (bd.LerInt("LugarID3") > 0)
                        lugares.Add(bd.LerInt("LugarID3"));

                    if (bd.LerInt("LugarID4") > 0)
                        lugares.Add(bd.LerInt("LugarID4"));

                    if (bd.LerInt("LugarID5") > 0)
                        lugares.Add(bd.LerInt("LugarID5"));

                    listaAnonina.Add(new
                    {
                        ID = bd.LerInt("ID"),
                        ClienteID = bd.LerInt("ClienteID"),
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        Lugares = lugares,

                        AssinaturaAnoID = bd.LerInt("AssinaturaAnoID"),
                        SetorID = bd.LerInt("SetorID"),

                        BloqueioID = bd.LerInt("BloqueioID"),
                        ExtintoBloqueioID = bd.LerInt("ExtintoBloqueioID"),
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();
                foreach (var assinatura in listaAnonina)
                {
                    try
                    {
                        foreach (int lugarID in assinatura.Lugares)
                        {
                            List<int> ingressos = new List<int>();
                            AssinaturaCliente oAssinaturaCliente = new AssinaturaCliente();

                            sql = string.Format(@"
                                SELECT 
	                                i.ID, i.Status, i.BloqueioID
	                            FROM tAssinaturaAno an (NOLOCK)
	                            INNER JOIN tAssinaturaItem ai (NOLOCK) ON an.ID = ai.AssinaturaAnoID
	                            INNER JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.SetorID = ai.SetorID
	                            WHERE an.ID = {0} AND i.LugarID = {1} AND PrecoTipoID = 8
                                ", assinatura.AssinaturaAnoID, lugarID);

                            if (!bd.Consulta(sql).Read())
                                throw new Exception("Não foi possível encontrar os ingressos desta assinatura.");

                            int bloqueioID = bd.LerInt("BloqueioID");
                            do
                            {
                                if (bd.LerString("Status") != Ingresso.DISPONIVEL && bd.LerString("Status") != Ingresso.BLOQUEADO)
                                    throw new Exception("Ingresso com status: " + bd.LerString("Status"));

                                ingressos.Add(bd.LerInt("ID"));
                            } while (bd.Consulta().Read());

                            bd.FecharConsulta();

                            oAssinaturaCliente.Limpar();
                            oAssinaturaCliente.ClienteID.Valor = assinatura.ClienteID;
                            oAssinaturaCliente.AssinaturaID.Valor = assinatura.AssinaturaID;
                            oAssinaturaCliente.PrecoTipoID.Valor = 0;
                            oAssinaturaCliente.LugarID.Valor = lugarID;
                            oAssinaturaCliente.AssinaturaAnoID.Valor = assinatura.AssinaturaAnoID;
                            oAssinaturaCliente.VendaBilheteriaID.Valor = 0;
                            oAssinaturaCliente.SetorID.Valor = assinatura.SetorID;
                            oAssinaturaCliente.Acao.Valor = Convert.ToChar((AssinaturaCliente.EnumStatus.Aguardando)).ToString();

                            string status = string.Empty;

                            if (bloqueioID == assinatura.ExtintoBloqueioID)
                            {
                                oAssinaturaCliente.Status.Valor = Convert.ToChar((AssinaturaCliente.EnumStatus.Indisponivel)).ToString();
                                bloqueioID = assinatura.ExtintoBloqueioID;
                            }
                            else
                            {
                                oAssinaturaCliente.Status.Valor = Convert.ToChar((AssinaturaCliente.EnumStatus.Aguardando)).ToString();
                                bloqueioID = assinatura.BloqueioID;
                            }

                            try
                            {

                                bd.IniciarTransacao();

                                oAssinaturaCliente.Inserir(bd);

                                if (ingressos.Count != 10)
                                    throw new Exception("Quantidade de ingressos diferente de 10!!!!");

                                foreach (var ingresso in ingressos)
                                {
                                    if (bd.Executar(
                                            string.Format(
                                            @"UPDATE tIngresso SET Status = 'B', BloqueioID = {0}, ClienteID = {1}, AssinaturaClienteID = {2} WHERE ID = {3} ",
                                            bloqueioID, assinatura.ClienteID, oAssinaturaCliente.Control.ID, ingresso)) != 1)
                                        throw new Exception("Não foi possível atualizar o ingresso: " + ingresso);
                                }


                                bd.Executar("UPDATE AssinantesCultura SET Importado = 1, Erro = '' WHERE UserID = " + assinatura.ID);

                                bd.FinalizarTransacao();
                            }
                            catch (Exception ex)
                            {
                                bd.DesfazerTransacao();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        bd.Executar("UPDATE AssinantesCultura SET Importado = 0, Erro = '" + ex.Message + "' WHERE UserID = " + assinatura.ID);
                    }
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Fix(int assinaturaID)
        {
            BD bd = new BD();
            try
            {
                string buscaInicial = @"
                        SELECT ap.AssinaturaClienteID 
	                    FROM AssinaturasProblemasCaioDesistencias ap (NOLOCK)
	                    LEFT JOIN tAssinaturaErroFix af (NOLOCK) ON af.AssinaturaClienteID = ap.AssinaturaClienteID
                    WHERE (af.ID IS NULL OR af.Fixed = 0) AND Acao <> 'R' AND Status <> 'R' AND AssinaturaID = " + assinaturaID;

                bd.Consulta(buscaInicial);

                if (!bd.Consulta().Read())
                    throw new Exception("Nenhuma inconsistencia");

                List<int> listAssinaturaClienteID = new List<int>();

                do
                {
                    listAssinaturaClienteID.Add(bd.LerInt("AssinaturaClienteID"));
                } while (bd.Consulta().Read());
                bd.FecharConsulta();

                string consultaAssinaturaCliente =
                    @"
                        SELECT 
	                        i.ID, i.Status, ac.ClienteID, a.BloqueioID, i.AssinaturaClienteID
	                        FROM tAssinaturaCliente ac (NOLOCK)
	                        INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
	                        INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
	                        INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
	                        INNER JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.SetorID = ai.SetorID AND i.LugarID = ac.LugarID
	                        WHERE ac.ID = {0} AND ai.PrecoTipoID = 1
                    ";

                var itemAnonimo = new
                {
                    ID = 0,
                    ClienteID = 0,
                    Status = string.Empty,
                    AssinaturaClienteID = 0,
                    BloqueioID = 0
                };

                var listaAnonima = VendaBilheteria.ToAnonymousList(itemAnonimo);

                foreach (int id in listAssinaturaClienteID)
                {
                    try
                    {
                        bd.Consulta(string.Format(consultaAssinaturaCliente, id));

                        if (!bd.Consulta().Read())
                            throw new Exception("Sem ingressos!!!!");

                        listaAnonima.Clear();

                        do
                        {
                            listaAnonima.Add(new
                            {
                                ID = bd.LerInt("ID"),
                                ClienteID = bd.LerInt("ClienteID"),
                                Status = bd.LerString("Status"),
                                AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                                BloqueioID = bd.LerInt("BloqueioID"),
                            });
                        } while (bd.Consulta().Read());


                        bd.FecharConsulta();
                        bd.IniciarTransacao();

                        foreach (var ingresso in listaAnonima)
                        {
                            if (ingresso.Status == "V" || ingresso.Status == "I")
                                throw new Exception("Ingresso com status: " + ingresso.Status);


                            if (ingresso.AssinaturaClienteID == id)
                                continue;

                            //Deve assicuar
                            if (ingresso.AssinaturaClienteID != 0)
                            {
                                if (bd.Executar(string.Format("UPDATE tIngresso SET Status = '{0}', ClienteID = {1}, BloqueioID = {2}, AssinaturaClienteID = {3} WHERE ID = {4} AND Status <> 'V' AND Status <> 'I'",
                                          Ingresso.BLOQUEADO, ingresso.ClienteID, ingresso.BloqueioID, id, ingresso.ID)) != 1)
                                { throw new Exception("Não foi possível atualizar o status do ingresso" + ingresso.ID); }
                            }
                            //Reseta esta porcaria, porque qndo passar a outra assinatura, deve associar este ingresso!
                            else
                                if (bd.Executar(string.Format("UPDATE tIngresso SET Status = 'D', ClienteID = 0, BloqueioID = 0, AssinaturaClienteID = 0 WHERE ID = {0} AND Status <> 'V' AND Status <> 'I'", ingresso.ID)) != 1)
                                { throw new Exception("Não foi possível atualizar o status do ingresso" + ingresso.ID); }


                        }


                        bd.Executar("INSERT INTO tAssinaturaErroFix (AssinaturaClienteID, Fixed, Erro) VALUES (" + id + ", 1, '')");
                        bd.FinalizarTransacao();
                    }
                    catch (Exception ex)
                    {
                        bd.DesfazerTransacao();
                        bd.Fechar();
                        bd.Executar("INSERT INTO tAssinaturaErroFix (AssinaturaClienteID, Fixed, Erro) VALUES (" + id + ", 0, '" + ex.Message + "') ");
                    }
                    finally
                    {
                        bd.FecharConsulta();
                    }
                }

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void FixEleazar(int assinaturaID)
        {
            BD bd = new BD();
            try
            {
                string buscaInicial = @"
                        SELECT ap.AssinaturaClienteID 
	                    FROM AssinaturasProblemasCaioUCO ap (NOLOCK)
	                    LEFT JOIN tAssinaturaErroFix af (NOLOCK) ON af.AssinaturaClienteID = ap.AssinaturaClienteID
                        WHERE (af.ID IS NULL OR af.Fixed = 0) AND AssinaturaID = " + assinaturaID;

                bd.Consulta(buscaInicial);

                if (!bd.Consulta().Read())
                    throw new Exception("Nenhuma inconsistencia");

                List<int> listAssinaturaClienteID = new List<int>();

                do
                {
                    listAssinaturaClienteID.Add(bd.LerInt("AssinaturaClienteID"));
                } while (bd.Consulta().Read());
                bd.FecharConsulta();


                var itemAnonimo = new
                {
                    ID = 0,
                    ClienteID = 0,
                    Status = string.Empty,
                    BloqueioID = 0,
                    EventoID = 0,
                    VendaBilheteriaID = 0,
                    PrecoTipoID = 0,
                    ApresentacaoSetorID = 0,
                    Valor = 0m,
                };

                var listaAnonima = VendaBilheteria.ToAnonymousList(itemAnonimo);

                foreach (int id in listAssinaturaClienteID)
                {
                    try
                    {
                        bd.Consulta(string.Format(@"SELECT 
	                        i.ID, i.Status, ac.ClienteID, a.BloqueioID, i.EventoID, i.VendaBilheteriaID, ac.PrecoTipoID, i.ApresentacaoSetorID, p.Valor
	                        FROM tAssinaturaCliente ac (NOLOCK)
	                        INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
	                        INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
	                        INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
	                        INNER JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.SetorID = ai.SetorID AND i.LugarID = ac.LugarID
	                        LEFT JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
	                        WHERE ac.ID = {0} AND ai.PrecoTipoID = 1
                            ORDER BY i.ID ASC", id));

                        if (!bd.Consulta().Read())
                            throw new Exception("Sem ingressos!!!!");

                        listaAnonima.Clear();

                        do
                        {
                            listaAnonima.Add(new
                            {
                                ID = bd.LerInt("ID"),
                                ClienteID = bd.LerInt("ClienteID"),
                                Status = bd.LerString("Status"),
                                BloqueioID = bd.LerInt("BloqueioID"),
                                EventoID = bd.LerInt("EventoID"),
                                VendaBilheteriaID = bd.LerInt("VendaBilheteriaID"),
                                PrecoTipoID = bd.LerInt("PrecoTipoID"),
                                ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                                Valor = bd.LerDecimal("Valor"),
                            });
                        } while (bd.Consulta().Read());


                        bd.FecharConsulta();
                        bd.IniciarTransacao();

                        int ingressoIDAtualizar = 0;
                        int vendaBilheteriaID = 0;
                        int precoTipoID = 0;
                        string status = string.Empty;
                        foreach (var ingresso in listaAnonima)
                        {
                            //Aqui entra o ingresso ANTIGO
                            if (ingresso.EventoID == 18253)
                            {
                                ingressoIDAtualizar = ingresso.ID;
                                vendaBilheteriaID = ingresso.VendaBilheteriaID;
                                precoTipoID = ingresso.PrecoTipoID;
                                status = ingresso.Status;

                                if (bd.Executar(string.Format("UPDATE tIngresso SET PrecoID = 0, Status = 'D', ClienteID = 0, BloqueioID = 0, AssinaturaClienteID = 0, VendaBilheteriaID = 0 WHERE ID = {0}", ingresso.ID)) != 1)
                                { throw new Exception("Não foi possível atualizar o status do ingresso" + ingresso.ID); }
                            }
                            //Esse aqui é o NOVO EventoID
                            else if (ingresso.EventoID == 18489)
                            {
                                if (ingressoIDAtualizar == 0)
                                    throw new Exception("Não encontrou ingresso correspondente no evento errado.");

                                if (status == Ingresso.VENDIDO)
                                {
                                    int precoID = Convert.ToInt32(bd.ConsultaValor("SELECT ID FROM tPreco WHERE PrecoTipoID = " + precoTipoID + " AND ApresentacaoSetorID = " + ingresso.ApresentacaoSetorID));
                                    if (precoID == 0)
                                        throw new Exception("PrecoID invalido");

                                    if (bd.Executar(string.Format("UPDATE tIngresso SET Status = '{0}', ClienteID = {1}, BloqueioID = {2}, AssinaturaClienteID = {3}, VendaBilheteriaID = {4}, PrecoID = {5} WHERE ID = {6} AND Status <> 'V' AND Status <> 'I'",
                                              Ingresso.VENDIDO, ingresso.ClienteID, ingresso.BloqueioID, id, vendaBilheteriaID, precoID, ingresso.ID)) != 1)
                                    { throw new Exception("Não foi possível atualizar o status do ingresso" + ingresso.ID); }

                                    bd.Executar("UPDATE tIngressoLOG SET IngressoID = " + ingresso.ID + ", PrecoID = " + precoID + " WHERE IngressoID = " + ingressoIDAtualizar);
                                }
                                else
                                {
                                    if (bd.Executar(string.Format("UPDATE tIngresso SET Status = '{0}', ClienteID = {1}, BloqueioID = {2}, AssinaturaClienteID = {3}, VendaBilheteriaID = 0, PrecoID = 0 WHERE ID = {4} AND Status <> 'V' AND Status <> 'I'",
                                             Ingresso.BLOQUEADO, ingresso.ClienteID, ingresso.BloqueioID, id, ingresso.ID)) != 1)
                                    { throw new Exception("Não foi possível atualizar o status do ingresso" + ingresso.ID); }

                                }
                                break;
                            }
                        }

                        bd.Executar("INSERT INTO tAssinaturaErroFix (AssinaturaClienteID, Fixed, Erro) VALUES (" + id + ", 1, '')");
                        bd.FinalizarTransacao();
                    }
                    catch (Exception ex)
                    {
                        bd.DesfazerTransacao();
                        bd.Fechar();
                        bd.Executar("INSERT INTO tAssinaturaErroFix (AssinaturaClienteID, Fixed, Erro) VALUES (" + id + ", 0, '" + ex.Message + "') ");
                    }
                    finally
                    {
                        bd.FecharConsulta();
                    }
                }
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
