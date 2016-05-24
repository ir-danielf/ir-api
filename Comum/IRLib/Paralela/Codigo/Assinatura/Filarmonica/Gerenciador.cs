using CTLib;
using IRLib.Paralela.ClientObjects.Assinaturas.Filarmonica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace IRLib.Paralela.Assinaturas
{
    public class Gerenciador
    {
        private BD bd = new BD();
        public bool Logar(string cpf, string nome)
        {
            try
            {
                return
                     Convert.ToInt32(bd.ConsultaValor(string.Format(
                         "SELECT TOP 1 ID FROM tFilarmonica (NOLOCK) WHERE CPF = '{0}' AND Nome LIKE '{1}%'",
                         cpf, nome))) > 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IEnumerable<EstruturaAssinaturaFilarmonica> CarregarAssinaturas(string cpf, string nome)
        {
            try
            {
                cpf = cpf.Replace("'", string.Empty);
                nome = nome.Replace("'", string.Empty);

                if (BuscarSeExisteAcao(cpf, nome))
                    throw new Exception("Atenção: suas ações em relação a assinatura Filarmonica 2011 já foram efetuadas.");

                bd.FecharConsulta();

                string sql =
                    string.Format(@"
                        SELECT 
	                        ID, Nome, Assinatura, Setor, Lugar
	                        FROM tFilarmonica f (NOLOCK)
	                        WHERE f.CPF = '{0}' AND f.Nome LIKE '{1}%'
                    ", cpf, nome);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem assinaturas a serem exibidas.");

                do
                {
                    yield return new EstruturaAssinaturaFilarmonica()
                    {
                        ID = bd.LerInt("ID"),
                        Assinatura = bd.LerString("Assinatura"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Lugar"),
                        Nome = bd.LerString("Nome"),
                    };
                } while (bd.Consulta().Read());
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IEnumerable<EstruturaAssinaturaFilarmonicaPreco> CarregarPrecos(int AssinaturaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT
	                        fp.ID, fp.Preco, fp.Valor
	                    FROM tFilarmonica f (NOLOCK)
	                    INNER JOIN tFilarmonicaPreco fp (NOLOCK) ON fp.Assinatura = f.Assinatura AND fp.Setor = f.Setor
	                    WHERE f.ID = {0}
                        ORDER BY fp.Preco ASC", AssinaturaID);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível localizar nenhum preço para a assinatura escolhida.");

                do
                {
                    yield return new EstruturaAssinaturaFilarmonicaPreco()
                    {
                        AssinaturaID = AssinaturaID,
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Preco"),
                        Valor = bd.LerDecimal("Valor"),
                    };
                } while (bd.Consulta().Read());
            }
            finally
            {
                bd.FecharConsulta();
            }
        }

        public void Pagar(string cpf, string nome, string FormaPagamento, int Parcelas, string Numero, string Data, string Codigo,
            string SemCodigo, float ValorTotal, List<EstruturaEfetuarAcao> acoesEfetuar, int ValorEntrega)
        {
            IRLib.Paralela.Sitef sitef = new IRLib.Paralela.Sitef();
            bool precisaPagar = ValorTotal > 0 && FormaPagamento.Length > 0 && string.Compare(FormaPagamento, "debito", false) != 0 && string.Compare(FormaPagamento, "dinheiro", false) != 0;

            try
            {
                FormaPagamento = FormaPagamento.Replace("'", string.Empty);
                Numero = Numero.Replace("'", string.Empty);
                Data = Data.Replace("'", string.Empty);
                Codigo = Codigo.Replace("'", string.Empty);

                if (this.BuscarSeExisteAcao(cpf, nome))
                    throw new Exception("Atenção: suas ações em relação a assinatura Filarmonica 2011 já foram efetuadas.");

                if (!this.BuscarSeAssinaturaPertenceAoCliente(cpf, nome, acoesEfetuar.Select(c => c.AssinaturaID).Distinct().ToList()))
                    return;

                if (!this.VerificarValores(acoesEfetuar.Where(c => c.Preco > 0).Select(c => c.Preco).ToList(), ValorTotal))
                    throw new Exception("O valor total não corresponde aos preços selecionados, por favor tente novamente.");

                bd.IniciarTransacao();

                var notaFiscal = string.Empty;
                if (precisaPagar)
                {
                    sitef.terminal = acoesEfetuar[0].AssinaturaID.ToString("00000000");

                    sitef.Terminal = IRLib.Paralela.Sitef.enumTerminal.SiteIR;
                    sitef.Empresa = IRLib.Paralela.Sitef.enumEmpresa.IngressoRapido;
                    sitef.ValorCompra = (Convert.ToDecimal(ValorTotal + ValorEntrega)).ToString("#.00");
                    sitef.Parcelas = Parcelas.ToString();
                    sitef.ClienteID = "0";
                    //sitef.Bandeira = (Sitef.enumBandeira)Enum.Parse(typeof(Sitef.enumBandeira), oRetorno.Bandeira);
                    sitef.NumeroCartao = Numero;
                    sitef.DataVencimento = Data.ToString();
                    sitef.CodigoSeguranca = Codigo.ToString();
                    sitef.TipoFinanciamento = IRLib.Paralela.Sitef.enumTipoFinanciamento.Estabelecimento;
                    sitef.IniciaSitef();

                    notaFiscal = sitef.CupomFiscal;
                }

                string inserirHistorico =
                        string.Format(
                        @"
                            INSERT INTO tFilarmonicaHistorico 
                            VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}') SELECT SCOPE_IDENTITY();
                        ", Numero, Data, Codigo, FormaPagamento, Parcelas, notaFiscal);

                int historicoID = Convert.ToInt32(bd.ConsultaValor(inserirHistorico));

                foreach (var item in acoesEfetuar)
                {
                    string inserirAcao =
                        string.Format(
                        @"
                            INSERT INTO tFilarmonicaAcao 
                            VALUES ({0}, {1}, {2}, '{3}')  
                        ", item.AssinaturaID, item.Preco, historicoID, item.Acao);

                    bd.Executar(inserirAcao);
                }

                if (precisaPagar)
                    if (sitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível efetuar a cobrança. Por favor tente novamente.");

                bd.FinalizarTransacao();

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();

                if (precisaPagar)
                    if (sitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) == Sitef.enumRetornoSitef.Invalido)
                        throw new Exception("Não foi possível efetuar a cobrança. Por favor tente novamente.");

                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private bool VerificarValores(List<int> precos, float ValorTotal)
        {
            try
            {
                if (precos.Count == 0)
                    return true;

                bd.BulkInsert(precos.Select(c => c).Distinct().ToList(), "#tmpPrecos", false, true);
                string sql =
                    @"SELECT 
                        fp.ID, fp.Valor
                      FROM tFilarmonicaPreco fp (NOLOCK)
                      INNER JOIN #tmpPrecos tm ON tm.ID = fp.ID";

                List<EstruturaAssinaturaFilarmonicaPreco> lista = new List<EstruturaAssinaturaFilarmonicaPreco>();
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaAssinaturaFilarmonicaPreco()
                    {
                        ID = bd.LerInt("ID"),
                        Valor = bd.LerDecimal("Valor"),
                    });
                }

                decimal valorBD = 0;

                foreach (int preco in precos)
                {
                    var itemPreco = lista.Where(c => c.ID == preco).FirstOrDefault();
                    if (itemPreco == null)
                        return false;

                    valorBD += itemPreco.Valor;
                }

                return (Convert.ToDecimal(ValorTotal) == valorBD);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool BuscarSeExisteAcao(string cpf, string nome)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor
                                             (string.Format(@"SELECT TOP 1 
	                                            fa.ID
	                                        FROM tFilarmonicaAcao fa (NOLOCK)
	                                        INNER JOIN tFilarmonica f (NOLOCK) on f.ID = fa.FilarmonicaID
	                                        WHERE f.CPF = '{0}' AND f.Nome LIKE '{1}%'", cpf, nome))) > 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool BuscarSeAssinaturaPertenceAoCliente(string cpf, string nome, List<int> assinaturasID)
        {
            try
            {
                bd.BulkInsert(assinaturasID, "#tmp", false, true);

                string sql
                    = string.Format(@"SELECT
	                            f.ID AS FilarmonicaID, #tmp.ID AS TempID
	                        FROM tFilarmonica f (NOLOCK)
	                        LEFT JOIN #tmp ON f.ID = #tmp.ID
	                        WHERE f.CPF = '{0}' AND f.Nome LIKE '{1}%' AND (f.ID IS NULL OR #tmp.ID IS NULL)", cpf, nome);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    throw new Exception("Existem assinaturas não associadas na ação efetuada. Tente novamente");
                else
                    return true;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void FilarminicaImportar(int entregaID)
        {
            List<string> listaCPF = this.AlimentarListaCPF();
            List<Assinaturas.Models.AcaoProvisoria> listaProvisoria = new List<Models.AcaoProvisoria>();

            var estruturaIdentificacaoUsuario = new IRLib.Paralela.ClientObjects.EstruturaReservaInternet
                {
                    CaixaID = 488695,
                    CanalID = 2276,
                    UsuarioID = 11003,
                    LojaID = 3394,
                    EmpresaID = 653,
                };

            BD bdAux = new BD();
            #region CPF a CPF
            foreach (string cpf in listaCPF.Distinct())
            {
                try
                {
                    var objeto = new
                    {
                        ID = 0,
                        CPF = string.Empty,
                        Assinatura = string.Empty,
                        Setor = string.Empty,
                        Lugar = string.Empty,
                        Acao = string.Empty,
                        Preco = string.Empty,
                        FormaPagamento = string.Empty,
                        Parcelas = 0,
                        NotaFiscal = string.Empty,
                        Indice = 0,
                    };

                    var lista = VendaBilheteria.ToAnonymousList(objeto);

                    #region Buscar Lugares do CPF
                    string sql =
                        @"
                            SELECT
	                            f.ID, f.CPF, f.Assinatura, f.Setor, 
	                            f.Lugar, fa.Acao, fp.Preco, fh.FormaPagamento, fh.Parcelas, fh.NotaFiscal
	
	                        FROM tFilarmonica f (NOLOCK)
	                        INNER JOIN tFilarmonicaAcao fa (NOLOCK) ON fa.FilarmonicaID = f.ID
	                        INNER JOIN tFilarmonicaHistorico fh (NOLOCK) ON fa.tFilarmonicaHistoricoID = fh.ID
	                        LEFT JOIN tFilarmonicaPreco fp (NOLOCK) ON fa.FilarmonicaPrecoID = fp.ID
	                        WHERE CPF = '" + cpf + "' AND fa.Acao <> 'D'";

                    if (!bd.Consulta(sql).Read())
                        continue;

                    int i = 1;
                    do
                    {

                        lista.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            CPF = cpf,
                            Assinatura = bd.LerString("Assinatura"),
                            Setor = bd.LerString("Setor"),
                            Lugar = bd.LerString("Lugar"),
                            Acao = bd.LerString("Acao"),
                            Preco = bd.LerString("Preco"),
                            FormaPagamento = bd.LerString("FormaPagamento"),
                            Parcelas = bd.LerInt("Parcelas"),
                            NotaFiscal = bd.LerString("NotaFiscal"),
                            Indice = i
                        });
                        i++;
                    } while (bd.Consulta().Read());

                    bd.FecharConsulta();

                    #endregion

                    decimal valorTotal = Convert.ToDecimal(bd.ConsultaValor(
                                        @"
                                            SELECT
	                                            SUM(Valor)
	                                            FROM tFilarmonica f (NOLOCK)
	                                            INNER JOIN tFilarmonicaAcao fa (NOLOCK) ON fa.FilarmonicaID = f.ID
	                                            INNER JOIN tFilarmonicaPreco fp (NOLOCK) ON fa.FilarmonicaPrecoID = fp.ID
	                                            WHERE CPF = '" + cpf + "'"));
                    bd.FecharConsulta();

                    #region Busca ClienteID
                    int clienteID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tCliente WHERE CPF = '" + cpf + "'"));
                    if (clienteID == 0)
                        throw new Exception("Não existe cliente com CPF : " + cpf);

                    bd.FecharConsulta();

                    Cliente oCliente = new Cliente();
                    oCliente.Ler(clienteID);

                    ClienteEndereco oClienteEndereco = new ClienteEndereco();

                    oClienteEndereco.Nome.Valor = oCliente.Nome.Valor;
                    oClienteEndereco.CPF.Valor = oCliente.CPF.Valor;
                    oClienteEndereco.ClienteID.Valor = oCliente.Control.ID;
                    oClienteEndereco.Endereco.Valor = oCliente.EnderecoCliente.Valor;
                    oClienteEndereco.Numero.Valor = oCliente.NumeroCliente.Valor;
                    oClienteEndereco.Complemento.Valor = oCliente.ComplementoCliente.Valor;
                    oClienteEndereco.Bairro.Valor = oCliente.BairroCliente.Valor;
                    oClienteEndereco.CEP.Valor = oCliente.CEPCliente.Valor;
                    oClienteEndereco.Cidade.Valor = oCliente.CidadeCliente.Valor;
                    oClienteEndereco.Estado.Valor = oCliente.EstadoCliente.Valor;
                    oClienteEndereco.EnderecoPrincipal.Valor = true;
                    oClienteEndereco.EnderecoTipoID.Valor = 1;

                    int ClienteEnderecoID = oClienteEndereco.VerificaEnderecoCliente(oCliente.Control.ID, oCliente.CEPCliente.Valor);

                    if (ClienteEnderecoID == 0)
                        oClienteEndereco.Inserir(bd);
                    else
                    {
                        oClienteEndereco.Control.ID = ClienteEnderecoID;
                        oClienteEndereco.Atualizar(bd);
                    }

                    #endregion

                    #region Preenche item a item a assinatura do cliente
                    listaProvisoria.Clear();

                    foreach (var item in lista)
                    {
                        sql =
                           string.Format(
                           @"
                                SELECT 
	                                TOP 1
	                                a.ID AS AssinaturaID, l.ID AS LugarID, an.ID AS AssinaturaAnoID, s.ID AS SetorID
	                                FROM tAssinatura a (NOLOCK)
	                                INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
	                                INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
	                                INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
	                                INNER JOIN tLugar l (NOLOCK) ON l.SetorID = s.ID
	                                WHERE a.Nome = '{0}' AND s.Nome = '{1}' AND l.Codigo = '{2}'
                            ", item.Assinatura, item.Setor, item.Lugar);

                        if (!bd.Consulta(sql).Read())
                            throw new Exception("Não foi possível encontrar o Lugar: Código " + item.Lugar);

                        listaProvisoria.Add(new Models.AcaoProvisoria()
                        {
                            ClienteID = clienteID,
                            AssinaturaID = bd.LerInt("AssinaturaID"),
                            LugarID = bd.LerInt("LugarID"),
                            AssinaturaAnoID = bd.LerInt("AssinaturaAnoID"),
                            SetorID = bd.LerInt("SetorID"),
                            IndiceImportar = item.Indice
                        });
                    }
                    bd.FecharConsulta();
                    #endregion

                    try
                    {
                        bd.IniciarTransacao();

                        //listaProvisoria = new AssinaturaCliente().EfetuarAcoes(bd, clienteID, Usuario.INTERNET_USUARIO_ID, listaProvisoria);
                        foreach (var item in listaProvisoria)
                        {
                            AssinaturaCliente assinaturaCLiente = new AssinaturaCliente();
                            assinaturaCLiente.Reservar(bd, clienteID, item.AssinaturaID, item.SetorID, item.LugarID, DateTime.Now.Year + 1, estruturaIdentificacaoUsuario, 0);
                            item.AssinaturaClienteID = assinaturaCLiente.Control.ID;
                        }

                        List<IRLib.Paralela.ClientObjects.Assinaturas.EstruturaAssinaturaAcao> listaNova = new List<IRLib.Paralela.ClientObjects.Assinaturas.EstruturaAssinaturaAcao>();

                        foreach (var item in lista)
                        {
                            int precoTipoID = 0;

                            if (item.Acao == "R")
                            {
                                precoTipoID = Convert.ToInt32(
                                     bdAux.ConsultaValor(
                                         string.Format(
                                             @"
                                                SELECT 
	                                                DISTINCT pt.ID
	                                                FROM tAssinatura a (NOLOCK)
	                                                INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
	                                                INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
	                                                INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
	                                                INNER JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = ai.PrecoTipoID
	                                                WHERE a.Nome = '{0}' AND s.Nome = '{1}' AND pt.Nome = '{2}'
                                            ", item.Assinatura, item.Setor, item.Preco)));

                                if (precoTipoID == 0)
                                    throw new Exception("O preco tipo " + item.Preco + " não existe");
                            }

                            var acaoProvisoria = listaProvisoria.Where(c => c.IndiceImportar == item.Indice).FirstOrDefault();
                            acaoProvisoria.PrecoTipoID = precoTipoID;
                            acaoProvisoria.Acao = (AssinaturaCliente.EnumAcao)Convert.ToChar(item.Acao);
                            bdAux.FecharConsulta();

                            listaNova.Add(new ClientObjects.Assinaturas.EstruturaAssinaturaAcao()
                            {
                                AssinaturaClienteID = acaoProvisoria.AssinaturaClienteID.ToString(),
                                PrecoTipo = acaoProvisoria.PrecoTipoID.ToString(),
                                Acao = item.Acao
                            });
                        }

                        new AssinaturaAcaoProvisoria().AdicionarAcoes(bd, clienteID, entregaID, listaNova, estruturaIdentificacaoUsuario.UsuarioID);

                        var listaFinal = new IRLib.Paralela.AssinaturaAcaoProvisoria().BuscarLista(clienteID, false);

                        string senha = string.Empty;
                        if (listaFinal.Where(c => c.Acao == AssinaturaCliente.EnumAcao.Renovar).Count() > 0)
                            senha = new AssinaturaCliente().Vender(bd, clienteID, estruturaIdentificacaoUsuario.UsuarioID, lista.FirstOrDefault().FormaPagamento,
                                 lista.FirstOrDefault().Parcelas, string.Empty, string.Empty, string.Empty, estruturaIdentificacaoUsuario,
                                 listaFinal.Where(c => c.Acao == AssinaturaCliente.EnumAcao.Renovar).ToList(), 0, entregaID, 10, false, oCliente.Email.Valor);

                        new AssinaturaCliente().EfetuarAcoes(bd, clienteID, estruturaIdentificacaoUsuario.UsuarioID, listaFinal.Where(c => c.Acao == AssinaturaCliente.EnumAcao.Trocar).ToList());

                        bd.Executar("UPDATE tFilarmonica SET Importado = 'T' WHERE CPF = '" + cpf + "'");
                        if (!string.IsNullOrEmpty(senha))
                        {
                            valorTotal += 10;

                            decimal valorVenda = Convert.ToDecimal(bd.ConsultaValor("SELECT ValorTotal FROM tVendaBilheteria WHERE Senha = '" + senha + "'"));

                            if (valorVenda != valorTotal)
                                throw new Exception("O Valor total da venda está incompativel com o valor da assinatura.");
                        }

                        bd.FinalizarTransacao();
                    }
                    catch (Exception)
                    {
                        bd.DesfazerTransacao();
                        throw;
                    }

                }
                catch (Exception ex)
                {
                    bdAux.Fechar();
                    bd.Executar("UPDATE tFilarmonica SET Importado = 'F', MotivoErro = '" + ex.Message.Replace("'", "") + "' WHERE CPF = '" + cpf + "'");
                }
            }
            #endregion
        }

        private void ImportarSemAcao(string cpf)
        {
            return;
        }

        private List<string> AlimentarListaCPF()
        {
            try
            {
                string sql =
                    @"
                        SELECT
                            CPF
                        FROM tFilarmonica (NOLOCK) WHERE Importado = 'F' AND ID > 8
                    ";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem dados a serem importados.");

                List<string> lista = new List<string>();

                do
                {
                    lista.Add(bd.LerString("CPF"));
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class AcaoEfetuadaException : Exception
    {
        public AcaoEfetuadaException() : base() { }

        public AcaoEfetuadaException(string msg) : base(msg) { }

        public AcaoEfetuadaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
