/**************************************************
* Arquivo: Pacote.cs
* Gerado: 17/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace IRLib
{

    public class Pacote : Pacote_B
    {
        private int UsuarioIDLogado;

        public Pacote() { }

        public Pacote(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.UsuarioIDLogado = usuarioIDLogado;
        }

        #region Métodos de Manipulação do Pacote

        #region Inserir
        /// <summary>
        /// Inserir novo(a) Pacote
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>
        internal bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cPacote");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tPacote(ID, LocalID, Nome, VendaDistribuida, Quantidade, Obs,PermitirCancelamentoAvulso,NomenclaturaPacoteID) ");
                sql.Append("VALUES (@ID,@001,'@002','@003',@004,'@005','@006',@007)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.Quantidade.ValorBD);
                sql.Replace("@005", this.Obs.ValorBD);
                sql.Replace("@006", this.PermitirCancelamentoAvulso.ValorBD);
                sql.Replace("@007", this.NomenclaturaPacoteID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I", bd);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Atualizar

        /// <summary>
        /// Atualiza Pacote
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        /// <returns></returns>
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cPacote WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tPacote SET LocalID = @001, Nome = '@002', VendaDistribuida = '@003', Quantidade = @004, Obs = '@005', NomenclaturaPacoteID = @006 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);
                sql.Replace("@003", this.VendaDistribuida.ValorBD);
                sql.Replace("@004", this.Quantidade.ValorBD);
                sql.Replace("@005", this.Obs.ValorBD);
                sql.Replace("@006", this.NomenclaturaPacoteID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Controle e Log

        protected internal void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cPacote (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected internal void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xPacote (ID, Versao, LocalID, Nome, VendaDistribuida, Quantidade, Obs) ");
                sql.Append("SELECT ID, @V, LocalID, Nome, VendaDistribuida, Quantidade, Obs FROM tPacote WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #endregion

        /// <summary>
        /// Grava o Pacote
        /// </summary>
        /// <param name="estruturaPacote">Estrutura com todos os dados do pacote</param>
        /// <param name="usuarioIDLogado">Usuário que está manipulando o sistema</param>
        /// <returns></returns>
        public ClientObjects.EstruturaPacote GravarPacote(ClientObjects.EstruturaPacote estruturaPacote)
        {
            BD bdGravarPacote = null;
            PacoteItem pacoteItem;
            PacoteItemLista pacoteItemLista;
            Preco preco;
            CanalPacote canalPacote;
            CanalPacoteLista canalPacoteLista;
            CodigoBarra oCodigoBarra;
            string strLugarMarcado = string.Empty;

            try
            {

                bdGravarPacote = new BD();

                bdGravarPacote.IniciarTransacao();

                oCodigoBarra = new CodigoBarra(this.Control.UsuarioID);

                #region Validação dos Dados

                if (this.Control.UsuarioID == 0)
                    throw new PacoteException("É necessário informar o usuário que está executando essa operação.");

                if (estruturaPacote.Nome.Trim().Length == 0)
                    throw new PacoteException("O nome do pacote não pode estar em branco.");

                if (estruturaPacote.Itens.Count == 0)
                    throw new PacoteException("O pacote não possui itens.");

                #endregion

                #region Manipulação do Pacote
                this.LocalID.Valor = estruturaPacote.LocalID;
                this.Nome.Valor = estruturaPacote.Nome;
                this.Quantidade.Valor = estruturaPacote.Quantidade;
                this.Obs.Valor = estruturaPacote.Observacao;
                this.VendaDistribuida.Valor = estruturaPacote.IRVende;
                this.PermitirCancelamentoAvulso.Valor = estruturaPacote.PermitirCancelamentoAvulso;
                this.NomenclaturaPacoteID.Valor = estruturaPacote.NomenclaturaPacoteID;

                if (estruturaPacote.ID <= 0)
                {
                    if (!this.Inserir(bdGravarPacote))
                        throw new PacoteException("Não foi possível inserir o pacote.");

                    estruturaPacote.ID = this.Control.ID;
                }
                else
                {
                    this.Control.ID = estruturaPacote.ID;

                    if (!this.Atualizar(bdGravarPacote))
                        throw new PacoteException("Não foi possível atualizar o pacote.");
                }

                if (estruturaPacote.ID == 0)
                    throw new PacoteException("O pacote não foi encontrado.");

                #endregion

                #region Manipulação do Item do Pacote
                pacoteItem = new PacoteItem(this.Control.UsuarioID);
                pacoteItemLista = new PacoteItemLista(this.Control.UsuarioID);
                int precoID = 0;

                // Exclui os itens já existentes no pacote                
                pacoteItemLista.FiltroSQL = "PacoteID = " + estruturaPacote.ID;
                pacoteItemLista.Carregar();
                if (pacoteItemLista.Primeiro())
                {
                    do
                    {
                        if (!pacoteItem.Excluir(pacoteItemLista.PacoteItem.Control.ID, ref bdGravarPacote))
                            throw new PacoteItemException("Não possível excluir o item do pacote.");
                    } while (pacoteItemLista.Proximo());
                }

                // Inclui os itens ao pacote
                preco = new Preco(this.Control.UsuarioID);
                ClientObjects.EstruturaPacotePreco oPacotePreco = new IRLib.ClientObjects.EstruturaPacotePreco();
                foreach (ClientObjects.EstruturaPacoteItem oPacoteItem in estruturaPacote.Itens)
                {
                    if (strLugarMarcado != string.Empty && strLugarMarcado != oPacoteItem.SetorLugarMarcado)
                        throw new PrecoException("Não é permitido ter setores Pista e Cadeira no mesmo pacote.");

                    strLugarMarcado = oPacoteItem.SetorLugarMarcado;

                    precoID = oPacoteItem.PrecoID;
                    if (oPacoteItem.PrecoID < 0)
                    {
                        // Captura o preço na lista de preços novos cadastrados
                        foreach (ClientObjects.EstruturaPacotePreco oItem in estruturaPacote.Precos)
                        {
                            if (oItem.ID == oPacoteItem.PrecoID)
                            {
                                oPacotePreco = oItem;
                                break;
                            }
                        }

                        // Insere o novo preço
                        preco.ApresentacaoSetorID.Valor = oPacoteItem.ApresentacaoSetorID;
                        preco.Nome.Valor = oPacotePreco.Nome;
                        preco.Quantidade.Valor = 0;
                        preco.QuantidadePorCliente.Valor = 0;
                        preco.CorID.Valor = oPacotePreco.CorID;
                        preco.Valor.Valor = oPacotePreco.Valor;
                        preco.Impressao.Valor = Preco.IMPRESSAO_AMBOS;
                        //if (!preco.Inserir(bdGravarPacote))
                        //    throw new PrecoException("Não foi possível incluir o novo preço.");
                        //oCodigoBarra.Inserir(oPacoteItem.EventoID, oPacoteItem.ApresentacaoID, oPacoteItem.SetorID, precoID, bdGravarPacote, this.LocalID.Valor);

                        preco.Inserir(oPacoteItem.EventoID, oPacoteItem.SetorID, oPacoteItem.ApresentacaoID, true, bdGravarPacote);
                        precoID = preco.Control.ID;
                    }

                    // Incluir Itens ao Pacote
                    pacoteItem.PacoteID.Valor = estruturaPacote.ID;
                    pacoteItem.PrecoID.Valor = precoID;
                    pacoteItem.Quantidade.Valor = oPacoteItem.Quantidade;
                    pacoteItem.EventoID.Valor = oPacoteItem.EventoID;
                    pacoteItem.ApresentacaoID.Valor = oPacoteItem.ApresentacaoID;
                    pacoteItem.SetorID.Valor = oPacoteItem.SetorID;
                    pacoteItem.CortesiaID.Valor = oPacoteItem.CortesiaID;
                    if (!pacoteItem.Inserir(ref bdGravarPacote))
                        throw new PacoteItemException("Não foi possível incluir o item do pacote.");

                }

                #endregion

                #region Manipulação dos Canais do Pacote
                canalPacote = new CanalPacote(this.Control.UsuarioID);
                canalPacoteLista = new CanalPacoteLista(this.Control.UsuarioID);

                // Exclui todos os canais do pacote
                canalPacoteLista.FiltroSQL = "PacoteID = " + estruturaPacote.ID;
                canalPacoteLista.Carregar();
                if (canalPacoteLista.Primeiro())
                {
                    do
                    {
                        if (!canalPacote.Excluir(canalPacoteLista.CanalPacote.Control.ID, ref bdGravarPacote))
                            throw new CanalPacoteException("Não possível excluir o canal do pacote.");
                    } while (canalPacoteLista.Proximo());
                }

                // Captura os canais que foram selecionados
                List<ClientObjects.EstruturaPacoteCanal> lstPacoteCanalSelecionado = new List<IRLib.ClientObjects.EstruturaPacoteCanal>();
                foreach (ClientObjects.EstruturaPacoteCanal oItem in estruturaPacote.Canais)
                {
                    if (oItem.Selecionado)
                        lstPacoteCanalSelecionado.Add(oItem);
                }

                // Adiciona os canais selecionados
                foreach (ClientObjects.EstruturaPacoteCanal oPacoteCanal in lstPacoteCanalSelecionado)
                {
                    canalPacote.PacoteID.Valor = estruturaPacote.ID;
                    canalPacote.CanalID.Valor = oPacoteCanal.CanalID;
                    canalPacote.Quantidade.Valor = oPacoteCanal.Quantidade;
                    canalPacote.TaxaConveniencia.Valor = oPacoteCanal.TaxaConveniencia;
                    canalPacote.TaxaMinima.Valor = oPacoteCanal.TaxaMinima;
                    canalPacote.TaxaMaxima.Valor = oPacoteCanal.TaxaMaxima;
                    if (!canalPacote.Inserir(ref bdGravarPacote))
                        throw new CanalPacoteException("Não possível inserir o canal do pacote.");
                }

                #endregion

                #region Manipulação dos Canais IR

                if (estruturaPacote.IRVende)
                    if (!DistribuirCanaisIR(bdGravarPacote))
                        throw new PacoteException("Não possível distribuir os canais IR.");

                #endregion

                bdGravarPacote.FinalizarTransacao();

            }
            catch (Exception ex)
            {
                if (bdGravarPacote != null)
                {
                    bdGravarPacote.DesfazerTransacao();
                }
                throw ex;
            }

            finally
            {
                if (bdGravarPacote != null)
                {
                    bdGravarPacote.Fechar();
                    bdGravarPacote = null;
                }

                pacoteItem = null;
                pacoteItemLista = null;
                preco = null;
                canalPacote = null;
                canalPacoteLista = null;
            }

            return estruturaPacote;
        }

        public DataTable PacotesDisponivelAjuste(int localID)
        {
            IDataReader dr = bd.Consulta(@"SELECT DISTINCT p.ID, p.Nome 
                                    FROM tPacote p, tPacoteItem pit, tApresentacao a
                                    WHERE pit.PacoteID = p.ID 
                                    AND pit.ApresentacaoID = a.ID 
                                    AND a.DisponivelAjuste = 'T'
                                    AND p.LocalID = " + localID);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public DataTable PacotesDisponivelVendaAssinatura(int localID)
        {
            IDataReader dr = bd.Consulta(@"SELECT DISTINCT p.ID, p.Nome 
                                    FROM tPacote p, tPacoteItem pit, tApresentacao a, tSetor s
                                    WHERE pit.PacoteID = p.ID 
                                    AND pit.ApresentacaoID = a.ID 
                                    AND pit.SetorID=s.ID
                                    AND a.DisponivelVenda = 'T'
                                    AND s.LugarMarcado <> 'P'
                                    AND p.LocalID = " + localID);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
        public DataTable InfoPacotesAssinatura(int localID)
        {
            IDataReader dr = bd.Consulta(@"SELECT tPacote.ID AS PacoteID,tPacote.Nome, tPacoteItem.SetorID AS SetorID, 
                                            tPacoteItem.ApresentacaoID, tSetor.Nome AS SetorNome,
                                            tPreco.ApresentacaoSetorID AS ApresentacaoSetorID, LugarMarcado 
                                            FROM tLocal(NOLOCK), tPacote(NOLOCK), 
                                            tCanalPacote(NOLOCK), tPacoteItem(NOLOCK), tPreco(NOLOCK), tApresentacao(NOLOCK), 
                                            tApresentacaoSetor (NOLOCK), tSetor (NOLOCK) 
                                            WHERE tSetor.ID = tPacoteItem.SetorID AND 
                                            tLocal.ID=tPacote.LocalID AND 
                                            tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND 
                                            tApresentacaoSetor.ID=tPreco.ApresentacaoSetorID AND 
                                            tPacote.ID=tCanalPacote.PacoteID AND 
                                            tPreco.ID=tPacoteItem.PrecoID AND 
                                            tPacoteItem.PacoteID=tPacote.ID AND 
                                            tLocal.ID= " + localID +
                                            " AND " +
                                            " tSetor.LugarMarcado <> 'P' " +
                                            " GROUP BY tPacote.ID,tPacote.Nome, tPacoteItem.SetorID, tPacoteItem.ApresentacaoID, " +
                                            " tPreco.ApresentacaoSetorID, LugarMarcado,tSetor.Nome");
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }


        #region Importação Assinatura LINQ
        /*
        /// <summary>
        /// Método utilizado para a importação de assinaturas da Osesp (Sala São Paulo)
        /// Faz a reserva e venda das assinaturas do cliente passado.
        /// Retorna a senha de venda ou o erro para registro no XML.
        /// </summary>
        /// <param name="clienteInfos">Informações em XML do cliente e das assinaturas desejadas.</param>
        public string ImportarAssinatura(XElement clienteInfos,int canalID,int lojaID,int empresaID,int usuarioID,int caixaID)
        {
            List<int> ingressoIDsLista = new List<int>();
             Bilheteria bilheteria = new Bilheteria();
            try
            {
                int clienteID = 0;
                string login = "";

                Cliente cliente = new Cliente();
                Ingresso ingresso = new Ingresso();

                DataSet estruturaVenda = IRLib.Bilheteria.EstruturaReservas();
                //esse DataSet serve apenas como auxiliar para o preenchimento da estrutruraVenda
                DataSet estruturaVendaAux = estruturaVenda.Clone();

                //tabelas para a venda das reservas
                DataTable tGridLeve;
                DataTable tReserva = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_RESERVA];
                DataTable tImpressao = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_ESTRUTURA_IMPRESSAO];
                DataTable tEventoTaxaEntrega = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_EVENTO_TAXA_ENTREGA];

               

                //Informações necessarias para a busca de ingressos da assinatura desejada.
                IRLib.ClientObjects.EstruturaAssinaturaInfo assinaturasInfoItem;

                //Preenche as variáveis com os dados do cliente atual para melhor leitura.
                login = clienteInfos.Element("Login").Value.ToString();

                clienteID = cliente.PesquisarClienteID("LoginOsesp = '" + login + "'");

                if (clienteID == 0)
                {
                    return "Erro: Cliente não encontrado";
                }
                else // Encontrou o cliente, buscar a assinatura.
                {
                    //busca os Nomes,Preços e Setores das assinaturas do cliente atual.
                    var assinaturasCliente = from a in clienteInfos.Descendants("Assinatura")
                                      select new
                                      {
                                          AssinaturaNome = a.Element("Nome").Value,
                                          AssinaturaPreco = a.Element("Preco").Value,
                                          AssinaturaSetor = a.Element("Setor").Value,
                                          AssinaturaFileira = a.Element("Fileira").Value,
                                          AssinaturaPoltrona = a.Element("Poltrona").Value
                                      };

                    int seedReserva = 1;
                    DataTable lugar = new DataTable();
                    //Primeiro faz a reserva de todas as assinaturas.
                    foreach (var assinatura in assinaturasCliente)
                    {
                        //Busca as informações da assinatura no banco de dados. Essa funçao não fecha o acesso ao banco
                        assinaturasInfoItem = PesquisarAssinaturaSSP(assinatura.AssinaturaNome, assinatura.AssinaturaSetor, assinatura.AssinaturaPreco);

                        if (assinaturasInfoItem.ApresentacoesID.Count() > 0)
                        {
                            //formata o código do lugar para o formato do BD da IR e popula o objeto tipado AssinaturasInfoItem
                            assinaturasInfoItem.CodigoLugar = FormatarCodigoLugarAssinatura(assinatura.AssinaturaFileira, assinatura.AssinaturaPoltrona, assinatura.AssinaturaSetor);

                            foreach (int apresentacaoID in assinaturasInfoItem.ApresentacoesID)
                            {
                                lugar = ingresso.tabelaLugarAssinatura(apresentacaoID, assinaturasInfoItem.SetorID, assinaturasInfoItem.CodigoLugar);
                                if (lugar.Rows.Count > 0)
                                    ingressoIDsLista.Add((int)lugar.Rows[0]["IngressoID"]);
                                else
                                {
                                    bilheteria.CancelarReservas(usuarioID);//antes de retornar deve-se cancelar todas as reservas
                                    return "Erro: Falha ao Reservar";
                                }
                            }
                            
                            if (((string)lugar.Rows[0]["Cod"]).Length != 0)//Assinatura
                            {
                                //faz a reserva de assinatura
                                estruturaVendaAux = bilheteria.ReservarLugarMarcadoAssinatura(assinaturasInfoItem.PacoteID, lugar, canalID, lojaID, usuarioID, seedReserva);
                                seedReserva++;
                                ingressoIDsLista.Clear();//limpa a lista que é utilizada na reserva de pacote de pista
                            }
                            else//pacote
                            {
                                Pacote oPacote = new Pacote();
                                oPacote.Control.ID = assinaturasInfoItem.PacoteID;
                                //itens do pacote.
                                DataTable itensPacote = oPacote.ItensParaReservaDePacote(bd);
                                
                                estruturaVendaAux = bilheteria.ReservarPista(lojaID, usuarioID, canalID, assinaturasInfoItem.PacoteID, 1, (int)itensPacote.Rows[0]["EventoID"], 0, 0, assinaturasInfoItem.PrecoID, 0, 0, 0, seedReserva, false);
                                seedReserva++;
                                ingressoIDsLista.Clear();//limpa a lista que é utilizada na reserva de pacote de pista
                            }

                            //Importa a estrutura de venda (retorno da reserva) para uma tabela única. 
                            //Isso é necessário para estruturar os paramentros da chamada do método de venda.
                            foreach (DataTable  tabela in estruturaVendaAux.Tables)
                            {
                                foreach (DataRow linha in tabela.Rows)
                                {
                                    DataRow linhaInserir = estruturaVenda.Tables[tabela.TableName].NewRow();

                                    foreach (DataColumn coluna in tabela.Columns)
                                    {
                                        linhaInserir[coluna.ColumnName] = linha[coluna.ColumnName];
                                    }
                                    estruturaVenda.Tables[tabela.TableName].Rows.Add(linhaInserir);
                                }
                            }
                        }
                        else
                        {
                            bilheteria.CancelarReservas(usuarioID);//antes de retornar deve-se cancelar todas as reservas
                            return "Erro: Assinatura não encontrada";
                        }
                    }
                    //Remove algumas colunas para melhor desenpenho na rotina
                    tGridLeve = estruturaVenda.Tables["Grid"].Copy();

                    tGridLeve.Columns.Remove(IRLib.Bilheteria.EVENTO_PACOTE);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.HORARIO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.SETOR_PRODUTO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.CODIGO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.PRECO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.CORTESIA);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.VALOR);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.LUGAR_MARCADO);
                    Dictionary<int, int> dicionario = new Dictionary<int, int>();//precisa desse dicionario para a venda

                    //depois de popular as informações e reservar os ingressos, faz a venda.
                    return bilheteria.Vender(tGridLeve, tReserva, new DataTable("tPagamento"), caixaID, lojaID, canalID, empresaID, clienteID, 0, 0, 0, usuarioID, 0, false, "", 0, dicionario, dicionario);
                }
            }
            catch (Exception)
            {
                bilheteria.CancelarReservas(usuarioID);
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
        */
        #endregion

        /// <summary>
        /// Método utilizado para a importação de assinaturas da Osesp (Sala São Paulo)
        /// Faz a reserva e venda das assinaturas do cliente passado.
        /// Retorna a senha de venda ou o erro para registro no XML.
        /// </summary>
        /// <param name="clienteInfos">Informações em XML do cliente e das assinaturas desejadas.</param>
        public string ImportarAssinatura(XmlNode clienteInfos, int canalID, int lojaID, int empresaID, int usuarioID, int caixaID)
        {
            List<int> ingressoIDsLista = new List<int>();
            Bilheteria bilheteria = new Bilheteria();
            try
            {
                int clienteID = 0;
                string login = "";

                Cliente cliente = new Cliente(usuarioID);
                Ingresso ingresso = new Ingresso();

                DataSet estruturaVenda = IRLib.Bilheteria.EstruturaReservas();
                //esse DataSet serve apenas como auxiliar para o preenchimento da estrutruraVenda
                DataSet estruturaVendaAux = estruturaVenda.Clone();

                //tabelas para a venda das reservas
                DataTable tGridLeve;
                DataTable tReserva = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_RESERVA];
                DataTable tImpressao = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_ESTRUTURA_IMPRESSAO];
                DataTable tEventoTaxaEntrega = estruturaVenda.Tables[IRLib.Bilheteria.TABELA_EVENTO_TAXA_ENTREGA];

                //Informações necessarias para a busca de ingressos da assinatura desejada.
                IRLib.ClientObjects.EstruturaAssinaturaInfo assinaturasInfoItem;

                //Preenche as variáveis com os dados do cliente atual para melhor leitura.
                login = clienteInfos["Login"].InnerText;

                clienteID = cliente.PesquisarClienteID("LoginOsesp = '" + login + "'");

                if (clienteID == 0)
                {
                    return "Erro: Cliente não encontrado";
                }
                else // Encontrou o cliente, buscar a assinatura.
                {
                    List<ClientObjects.EstruturaAssinaturaDetalhe> assinaturasCliente = new List<IRLib.ClientObjects.EstruturaAssinaturaDetalhe>();
                    //busca os Nomes,Preços e Setores das assinaturas do cliente atual.
                    foreach (XmlNode node in clienteInfos["Assinaturas"].GetElementsByTagName("Assinatura"))
                    {
                        ClientObjects.EstruturaAssinaturaDetalhe assinatura = new IRLib.ClientObjects.EstruturaAssinaturaDetalhe();
                        assinatura.AssinaturaNome = node["Nome"].InnerText;
                        assinatura.AssinaturaPreco = node["Preco"].InnerText;
                        assinatura.AssinaturaSetor = node["Setor"].InnerText;
                        assinatura.AssinaturaFileira = node["Fileira"].InnerText;
                        assinatura.AssinaturaPoltrona = node["Poltrona"].InnerText;
                        assinaturasCliente.Add(assinatura);
                    }

                    int seedReserva = 1;
                    DataTable lugar = new DataTable();
                    //Primeiro faz a reserva de todas as assinaturas.
                    foreach (ClientObjects.EstruturaAssinaturaDetalhe assinatura in assinaturasCliente)
                    {
                        //Busca as informações da assinatura no banco de dados. Essa funçao não fecha o acesso ao banco
                        assinaturasInfoItem = PesquisarAssinaturaSSP(assinatura.AssinaturaNome, assinatura.AssinaturaSetor, assinatura.AssinaturaPreco);

                        if (assinaturasInfoItem.ApresentacoesID.Count > 0)
                        {
                            //formata o código do lugar para o formato do BD da IR e popula o objeto tipado AssinaturasInfoItem
                            assinaturasInfoItem.CodigoLugar = FormatarCodigoLugarAssinatura(assinatura.AssinaturaFileira, assinatura.AssinaturaPoltrona, assinatura.AssinaturaSetor);

                            foreach (int apresentacaoID in assinaturasInfoItem.ApresentacoesID)
                            {
                                lugar = ingresso.tabelaLugarAssinatura(apresentacaoID, assinaturasInfoItem.SetorID, assinaturasInfoItem.CodigoLugar);
                                if (lugar.Rows.Count > 0)
                                    ingressoIDsLista.Add((int)lugar.Rows[0]["IngressoID"]);
                                else
                                {
                                    bilheteria.CancelarReservas(usuarioID);//antes de retornar deve-se cancelar todas as reservas
                                    return "Erro: Falha ao Reservar";
                                }
                            }

                            if (((string)lugar.Rows[0]["Cod"]).Length != 0)//Assinatura
                            {
                                //faz a reserva de assinatura
                                estruturaVendaAux = bilheteria.ReservarLugarMarcadoAssinatura(assinaturasInfoItem.PacoteID, lugar, canalID, lojaID, usuarioID, seedReserva, false, false, false);
                                seedReserva++;
                                ingressoIDsLista.Clear();//limpa a lista que é utilizada na reserva de pacote de pista
                            }
                            else//pacote
                            {
                                Pacote oPacote = new Pacote();
                                oPacote.Control.ID = assinaturasInfoItem.PacoteID;
                                //itens do pacote.
                                DataTable itensPacote = oPacote.ItensParaReservaDePacote(bd);
                                EstruturaCotasInfo cotasinfovazio = new EstruturaCotasInfo();

                                estruturaVendaAux = bilheteria.ReservarPista(lojaID, usuarioID, canalID, assinaturasInfoItem.PacoteID, 1, (int)itensPacote.Rows[0]["EventoID"], 0, 0, assinaturasInfoItem.PrecoID, string.Empty, 0, 0, 0, seedReserva, false, false, false, 0, cotasinfovazio, 0, string.Empty, 0, false, 0, 0);
                                seedReserva++;
                                ingressoIDsLista.Clear();//limpa a lista que é utilizada na reserva de pacote de pista
                            }

                            //Importa a estrutura de venda (retorno da reserva) para uma tabela única. 
                            //Isso é necessário para estruturar os paramentros da chamada do método de venda.
                            foreach (DataTable tabela in estruturaVendaAux.Tables)
                            {
                                foreach (DataRow linha in tabela.Rows)
                                {
                                    DataRow linhaInserir = estruturaVenda.Tables[tabela.TableName].NewRow();

                                    foreach (DataColumn coluna in tabela.Columns)
                                    {
                                        linhaInserir[coluna.ColumnName] = linha[coluna.ColumnName];
                                    }
                                    estruturaVenda.Tables[tabela.TableName].Rows.Add(linhaInserir);
                                }
                            }
                        }
                        else
                        {
                            bilheteria.CancelarReservas(usuarioID);//antes de retornar deve-se cancelar todas as reservas
                            return "Erro: Assinatura não encontrada";
                        }
                    }
                    //Remove algumas colunas para melhor desenpenho na rotina
                    tGridLeve = estruturaVenda.Tables["Grid"].Copy();

                    tGridLeve.Columns.Remove(IRLib.Bilheteria.EVENTO_PACOTE);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.HORARIO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.SETOR_PRODUTO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.CODIGO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.PRECO);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.CORTESIA);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.VALOR);
                    tGridLeve.Columns.Remove(IRLib.Bilheteria.LUGAR_MARCADO);
                    Dictionary<int, int> dicionario = new Dictionary<int, int>();//precisa desse dicionario para a venda
                    List<EstruturaDonoIngresso> listaDonoIngresso = new List<EstruturaDonoIngresso>();//Precisa da estrutura mesmo q vazia
                    List<int> VirIdsVazio = new List<int>();
                    string codigoTrocaFixo = string.Empty;

                    //depois de popular as informações e reservar os ingressos, faz a venda.

                    DataTable tPagamento = new DataTable("Pagamento");
                    tPagamento.Columns.Add("ID", typeof(int)); //FormaPagamentoID
                    tPagamento.Columns.Add("FormaPagamento", typeof(string));
                    tPagamento.Columns.Add("Valor", typeof(decimal));
                    tPagamento.Columns.Add("Dias", typeof(int));
                    tPagamento.Columns.Add("TaxaAdm", typeof(decimal));
                    tPagamento.Columns.Add("IR", typeof(string));
                    tPagamento.Columns.Add("VirIngressoID", typeof(int));

                    DataRow fp = tPagamento.NewRow();
                    fp["ID"] = 139;
                    fp["FormaPagamento"] = "ASSINATURAS";
                    fp["Valor"] = Convert.ToInt32(estruturaVenda.Tables["Grid"].Compute("SUM(Valor)", "1=1"));
                    fp["Dias"] = 0;
                    fp["TaxaAdm"] = 0;
                    fp["IR"] = "T";
                    tPagamento.Rows.Add(fp);

                    return bilheteria.Vender(tGridLeve, tReserva, tPagamento, caixaID, lojaID, canalID, empresaID, clienteID, 0, 0, Convert.ToInt32(fp["Valor"]), usuarioID, 0, false, "", 0, dicionario, dicionario, Canal.TipoDeVenda.VendaRemota, false, false, string.Empty, listaDonoIngresso, VirIdsVazio, codigoTrocaFixo, 0, null, 0, null, false, 0).Senha;
                }
            }
            catch (Exception ex)
            {
                bilheteria.CancelarReservas(usuarioID);
                return "Erro: ERRO não identificado " + ex.Message;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Formata o codigo do lugar de acordo com a o numero de caracteres.
        /// Utiliado para a importação de assinaturas da OSESP.
        /// </summary>
        /// <param name="Caracteres"></param>
        /// <returns></returns>
        private string FormatarCodigoLugarAssinatura(int caracteres, string fileira, string poltrona)
        {
            string novaPoltrona = poltrona.Trim();
            string codigoFormatado;

            while ((fileira.Trim() + " " + novaPoltrona).Length < caracteres)
            {
                novaPoltrona = "0" + novaPoltrona;
            }
            codigoFormatado = fileira.Trim() + " " + novaPoltrona;

            return codigoFormatado;

        }
        /// <summary>
        /// Formata o codigo do lugar de acordo com a fileira e a poltrona.
        /// Método utilizado para importação de assinaturas. O formato passado pela OSESP através do XML nem sempre é o mesmo do BD da IR.
        /// </summary>
        /// <returns></returns>
        private string FormatarCodigoLugarAssinatura(string fileira, string poltrona, string setorNome)
        {
            switch (setorNome.ToLower())
            {
                case "platéia central"://deve ter 4 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(4, fileira, poltrona);
                    }
                case "platéia elevada"://deve ter 5 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(5, fileira, poltrona);
                    }
                case "balcão mezanino": //deve ter 4 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(4, fileira, poltrona);
                    }
                case "balcão superior"://deve ter 4 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(4, fileira, poltrona);
                    }
                case "cam. mezanino"://deve ter 6 caracteres
                case "camarote mezanino"://deve ter 6 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(6, "C" + Convert.ToInt32(fileira).ToString("00"), poltrona);
                    }
                case "cam. superior"://deve ter 6 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(6, "C" + Convert.ToInt32(fileira).ToString("00"), poltrona);
                    }
                case "coro": //deve ter 4 caracteres
                    {
                        return FormatarCodigoLugarAssinatura(4, fileira, poltrona);
                    }
                case "caderno de programacao":
                case "setor único":
                    {
                        return "";
                    }
                default:
                    {
                        throw new Exception("Setor não encontrado: " + setorNome);
                    }



            }
        }

        /// <summary>
        /// Busca o SetorID e as ApresentaçãoID da assinatura desejada.
        /// Esse método trabalha com a busca através do nome do Pacote, Setor e Preço para fins de importação de assinaturas da OSESP
        /// IMPORTANTE: essa função não fecha a conexão de propósito, afim de reutilizar-la mais tarde.
        /// </summary>
        /// <param name="assinaturaNome">Nome do Pacote para a busca</param>
        /// <param name="setorNome">Nome do Setor para a busca</param>
        /// <param name="precoNome">Nome do Preço para a busca</param>
        /// <returns></returns>
        private IRLib.ClientObjects.EstruturaAssinaturaInfo PesquisarAssinaturaSSP(string assinaturaNome, string setorNome, string precoNome)
        {
            try
            {
                IRLib.ClientObjects.EstruturaAssinaturaInfo retorno = new IRLib.ClientObjects.EstruturaAssinaturaInfo();

                if (assinaturaNome != string.Empty && setorNome != string.Empty && precoNome != string.Empty)
                {
                    bd.FecharConsulta();

                    bd.Consulta(@"SELECT tSetor.ID AS SetorID,tPreco.ID AS PrecoID, tApresentacao.ID AS ApresentacaoID, tPacote.ID AS PacoteID FROM  tPacote(NOLOCK)
                                    INNER JOIN tPacoteItem ON tPacoteItem.PacoteID = tPacote.ID
                                    INNER JOIN tSetor ON tPacoteItem.SetorID = tSetor.ID
                                    INNER JOIN tPreco ON tPacoteItem.PrecoID = tPreco.ID
                                    INNER JOIN tApresentacao ON tPacoteItem.ApresentacaoID = tApresentacao.ID
                                    WHERE tPacote.LocalID = 123  AND tPacote.ID > 4415
                                    AND tPacote.Nome = '" + assinaturaNome + " " + setorNome + " " + precoNome + "'");
                }

                while (bd.Consulta().Read())
                {
                    retorno.ApresentacoesID.Add((int)bd.LerInt("ApresentacaoID"));
                    retorno.PacoteID = (int)bd.LerInt("PacoteID");
                    retorno.SetorID = (int)bd.LerInt("SetorID");
                    retorno.PrecoID = (int)bd.LerInt("PrecoID");
                }


                return retorno;

            }
            catch
            {
                throw;
            }

        }
        public DataTable ItensDoPacote(int pacoteID)
        {
            string sql = @"SELECT tpi.ID PacoteItemID, e.ID EventoID, e.Nome Evento, 
                        a.ID ApresentacaoID, a.Horario Apresentacao, 
                        s.ID SetorID, s.Nome Setor,
                        p.ID PrecoID, p.Nome Preco,
                        c.ID CortesiaID, c.Nome Cortesia,
                        tpi.Quantidade Qtde
                        FROM tPacoteItem tpi
                        INNER JOIN tEvento e ON e.ID = tpi.EventoID
                        INNER JOIN tApresentacao a ON a.ID = tpi.ApresentacaoID
                        INNER JOIN tSetor s ON s.ID = tpi.SetorID
                        INNER JOIN tPreco p ON p.ID = tpi.PrecoID
                        LEFT JOIN tCortesia c ON c.ID = CortesiaID
                        WHERE PacoteID = " + pacoteID;

            DataTable tabela = new DataTable("ItensPacote");

            tabela.Columns.Add("PacoteItemID", typeof(int));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("CortesiaID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("Cortesia", typeof(string));
            tabela.Columns.Add("Qtde", typeof(int));

            DataRow linhanova;

            while (bd.Consulta(sql).Read())
            {
                linhanova = tabela.NewRow();
                linhanova["PacoteItemID"] = bd.LerInt("PacoteItemID");
                linhanova["EventoID"] = bd.LerInt("EventoID");
                linhanova["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                linhanova["SetorID"] = bd.LerInt("SetorID");
                linhanova["PrecoID"] = bd.LerInt("PrecoID");
                linhanova["CortesiaID"] = bd.LerInt("CortesiaID");
                linhanova["Evento"] = bd.LerString("Evento");
                linhanova["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                linhanova["Setor"] = bd.LerString("Setor");
                linhanova["Preco"] = bd.LerString("Preco");
                linhanova["Cortesia"] = bd.LerString("Cortesia");
                linhanova["Qtde"] = bd.LerInt("Qtde");
                tabela.Rows.Add(linhanova);
            }

            return tabela;
        }


        public DataTable ItensPacoteSelecionado(int pacoteID)
        {
            string sql = @"SELECT te.Nome as Evento , ta.Horario as Apresentacao, ts.LugarMarcado ,tr.ID as PrecoID, tr.Nome as Preco , tr.Valor , tas.ID as ApresentacaoSetorID
                        FROM tPacoteItem as tp
                        INNER JOIN tEvento as te on te.ID = tp.EventoID
                        INNER JOIN tApresentacao as ta on ta.id = tp.ApresentacaoID
                        INNER JOIN tPreco as tr on tr.ID = tp.PrecoID
                        INNER JOIN tSetor as ts on ts.ID = tp.SetorID
                        INNER JOIN tApresentacaoSetor as tas on tas.ApresentacaoID = ta.ID and tas.SetorID = ts.ID
                        WHERE tp.PacoteID = " + pacoteID;

            DataTable tabela = new DataTable("ItensPacote");

            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("LugarMarcado", typeof(string));
            tabela.Columns.Add("PrecoID", typeof(string));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("Valor", typeof(string));
            tabela.Columns.Add("ApresentacaoSetorID", typeof(string));


            DataRow linhanova;

            while (bd.Consulta(sql).Read())
            {
                linhanova = tabela.NewRow();
                linhanova["Evento"] = bd.LerString("Evento");
                linhanova["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                linhanova["LugarMarcado"] = bd.LerString("LugarMarcado");
                linhanova["PrecoID"] = bd.LerString("PrecoID");
                linhanova["Preco"] = bd.LerString("Preco");
                linhanova["Valor"] = bd.LerDecimal("Valor").ToString("c");
                linhanova["ApresentacaoSetorID"] = bd.LerString("ApresentacaoSetorID");

                tabela.Rows.Add(linhanova);
            }
            return tabela;
        }

        public int TaxaConveniencia(int canalID, BD bd)
        {
            return this.TaxaConveniencia(this.Control.ID, canalID, bd);
        }
        public int TaxaConveniencia(int pacoteID, int canalID, BD bd)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("SELECT TaxaConveniencia FROM tCanalPacote(NOLOCK) WHERE CanalID = " + canalID + " AND PacoteID = " + pacoteID));
            }
            catch
            {
                throw new PacoteException("Este pacote não está mais disponível para o canal. Por favor, carregue a tela de venda novamente!");
            }
        }

        public bool CancelamentoAvulso(int PacoteID)
        {
            {

                try
                {

                    bd.IniciarTransacao();

                    StringBuilder sql = new StringBuilder();

                    sql.Append("SELECT PermitirCancelamentoAvulso from tPacote ");
                    sql.Append(" WHERE id = @ID");

                    sql.Replace("@ID", PacoteID.ToString());

                    bd.Consulta(sql.ToString());

                    bool result = false;

                    if (bd.Consulta().Read())
                    {
                        result = bd.LerBoolean("PermitirCancelamentoAvulso");
                    }


                    bd.FinalizarTransacao();

                    this.PermitirCancelamentoAvulso.Valor = result;

                    return result;

                }
                catch (Exception ex)
                {
                    bd.DesfazerTransacao();
                    throw ex;
                }
                finally
                {
                    bd.Fechar();
                }

            }

        }

        public DataTable ItensParaReservaDePacote(int pacoteID, CTLib.BD db)
        {
            this.Control.ID = pacoteID;
            return this.ItensParaReservaDePacote(db);
        }

        public DataTable ItensParaReservaDePacote(CTLib.BD db)
        {

            try
            {
                StringBuilder stbSQL = new StringBuilder();

                DataTable itens = new DataTable("tPacoteItem");
                itens.Columns.Add("PacoteNome", typeof(string));
                itens.Columns.Add("PacoteID", typeof(int));
                itens.Columns.Add("PrecoID", typeof(int));
                itens.Columns.Add("EventoID", typeof(int));
                itens.Columns.Add("CortesiaID", typeof(int));
                itens.Columns.Add("Quantidade", typeof(int));
                itens.Columns.Add("ApresentacaoSetorID", typeof(int));
                itens.Columns.Add("ApresentacaoID", typeof(int));
                itens.Columns.Add("ValorItem", typeof(decimal));
                itens.Columns.Add("SetorNome", typeof(string));
                itens.Columns.Add("PrecoNome", typeof(string));
                itens.Columns.Add("ApresentacaoCotaID", typeof(int));
                itens.Columns.Add("ApresentacaoSetorCotaID", typeof(int));
                itens.Columns.Add("QuantidadeApresentacaoSetor", typeof(int));
                itens.Columns.Add("QuantidadePorClienteApresentacaoSetor", typeof(int));
                itens.Columns.Add("QuantidadeApresentacao", typeof(int));
                itens.Columns.Add("QuantidadePorClienteApresentacao", typeof(int));
                itens.Columns.Add("ObrigatoriedadeID", typeof(int));
                itens.Columns.Add("PermitirCancelamentoAvulso", typeof(bool));
                itens.Columns.Add("TipoCodigoBarra", typeof(string));

                stbSQL.Append("SELECT tPacote.Nome, tPacote.PermitirCancelamentoAvulso, tPacote.ID PacoteID, tPacoteItem.PrecoID, tPreco.Nome AS PrecoNome, tPacoteItem.EventoID, ");
                stbSQL.Append("tPacoteItem.Quantidade, tPreco.ApresentacaoSetorID , ");
                stbSQL.Append("tPacoteItem.CortesiaID, tPacoteItem.ApresentacaoID, tPreco.Valor ValorItem, ");
                stbSQL.Append("st.Nome SetorNome, ");
                stbSQL.Append("IsNull(ap.CotaID, 0) AS ApresentacaoCotaID, IsNull(aps.CotaID, 0) AS ApresentacaoSetorCotaID, ");
                stbSQL.Append("isNull(ap.Quantidade,0) AS QuantidadeApresentacao, ");
                stbSQL.Append("IsNull(ap.QuantidadePorCliente, 0) AS QuantidadePorClienteApresentacao, ");
                stbSQL.Append("IsNull(aps.Quantidade, 0) AS QuantidadeApresentacaoSetor, ");
                stbSQL.Append("IsNull(aps.QuantidadePorCliente, 0) AS QuantidadePorClienteApresentacaoSetor, ");
                stbSQL.Append("IsNull(ev.ObrigatoriedadeID, 0) AS ObrigatoriedadeID, ev.TipoCodigoBarra ");
                stbSQL.Append("FROM tPacoteItem (NOLOCK) ");
                stbSQL.Append("INNER JOIN tPreco (NOLOCK) ON tPacoteItem.PrecoID = tPreco.ID ");
                stbSQL.Append("INNER JOIN tPacote (NOLOCK) ON tPacoteItem.PacoteID = tPacote.ID ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) aps ON aps.ID = tPreco.ApresentacaoSetorID ");
                stbSQL.Append("INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = aps.ApresentacaoID ");
                stbSQL.Append("INNER JOIN tSetor st (NOLOCK) ON st.ID = aps.SetorID ");
                stbSQL.Append("INNER JOIN tEvento ev (NOLOCK) ON tPacoteItem.EventoID = ev.ID ");

                stbSQL.Append("WHERE tPacoteItem.PacoteID=" + this.Control.ID);

                bd.Consulta(stbSQL.ToString());


                while (bd.Consulta().Read())
                {
                    DataRow linha = itens.NewRow();
                    linha["PacoteNome"] = bd.LerString("Nome");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    linha["ValorItem"] = bd.LerDecimal("ValorItem");
                    linha["SetorNome"] = bd.LerString("SetorNome");
                    linha["PrecoNome"] = bd.LerString("PrecoNome");
                    linha["ApresentacaoCotaID"] = bd.LerInt("ApresentacaoCotaID");
                    linha["ApresentacaoSetorCotaID"] = bd.LerInt("ApresentacaoSetorCotaID");
                    linha["QuantidadeApresentacaoSetor"] = bd.LerInt("QuantidadeApresentacaoSetor");
                    linha["QuantidadePorClienteApresentacaoSetor"] = bd.LerInt("QuantidadePorClienteApresentacaoSetor");
                    linha["QuantidadeApresentacao"] = bd.LerInt("QuantidadeApresentacao");
                    linha["QuantidadePorClienteApresentacao"] = bd.LerInt("QuantidadePorClienteApresentacao");
                    linha["ObrigatoriedadeID"] = bd.LerInt("ObrigatoriedadeID");
                    linha["PermitirCancelamentoAvulso"] = bd.LerBoolean("PermitirCancelamentoAvulso");
                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                    itens.Rows.Add(linha);
                }
                bd.Consulta().Close();

                return itens;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ItensParaReserva(CTLib.BD db)
        {

            try
            {
                DataTable itens = new DataTable("tPacoteItem");
                itens.Columns.Add("PrecoID", typeof(int));
                itens.Columns.Add("EventoID", typeof(int));
                itens.Columns.Add("CortesiaID", typeof(int));
                itens.Columns.Add("Quantidade", typeof(int));
                itens.Columns.Add("ApresentacaoSetorID", typeof(int));
                itens.Columns.Add("ValorItem", typeof(decimal));
                itens.Columns.Add("PrecoNome", typeof(string));
                itens.Columns.Add("ApresentacaoID", typeof(int));
                itens.Columns.Add("ApresentacaoCotaID", typeof(int));
                itens.Columns.Add("ApresentacaoSetorCotaID", typeof(int));
                itens.Columns.Add("QuantidadeApresentacaoSetor", typeof(int));
                itens.Columns.Add("QuantidadePorClienteApresentacaoSetor", typeof(int));
                itens.Columns.Add("QuantidadeApresentacao", typeof(int));
                itens.Columns.Add("QuantidadePorClienteApresentacao", typeof(int));
                itens.Columns.Add("TipoCodigoBarra", typeof(string));
                itens.Columns.Add("ValorTaxaProcessamento", typeof(decimal));
                itens.Columns.Add("Estado", typeof(string));


                //Chamada da Proc
                string sqlProc = "EXEC sp_getPacoteInfo " + this.Control.ID;

                bd.Consulta(sqlProc);

                while (bd.Consulta().Read())
                {
                    DataRow linha = itens.NewRow();
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["ValorItem"] = bd.LerDecimal("Valor");
                    linha["PrecoNome"] = bd.LerString("PrecoNome");
                    linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    linha["ApresentacaoCotaID"] = bd.LerInt("ApresentacaoCotaID");
                    linha["ApresentacaoSetorCotaID"] = bd.LerInt("ApresentacaoSetorCotaID");
                    linha["QuantidadeApresentacaoSetor"] = bd.LerInt("QuantidadeApresentacaoSetor");
                    linha["QuantidadePorClienteApresentacaoSetor"] = bd.LerInt("QuantidadePorClienteApresentacaoSetor");
                    linha["QuantidadeApresentacao"] = bd.LerInt("QuantidadeApresentacao");
                    linha["QuantidadePorClienteApresentacao"] = bd.LerInt("QuantidadePorClienteApresentacao");
                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                    linha["ValorTaxaProcessamento"] = bd.LerDecimal("ValorTaxaProcessamento");
                    linha["Estado"] = bd.LerString("Estado");
                    itens.Rows.Add(linha);
                }
                bd.Consulta().Close();

                return itens;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Faz a reserva do pacote. Retorna os ingressosIDs reservados. Se retornar null, nao pôde reservar o pacote.
        /// </summary>
        public int[] Reservar(int lojaID)
        {

            BD bd = new BD();
            List<EstruturaIngressoCotaItem> lstCotaItemID = new List<EstruturaIngressoCotaItem>();
            return Reservar(bd, ItensParaReserva(bd), ref lstCotaItemID, lojaID);

        }

        ///<summary>
        ///Faz a reserva do pacote. Retorna os ingressosIDs reservados. Se retornar null, nao pôde reservar o pacote.
        ///</summary>
        public int[] Reservar(DataTable itens, int lojaID)
        {

            BD bd = new BD();
            List<EstruturaIngressoCotaItem> lstCotaItemID = new List<EstruturaIngressoCotaItem>();
            return Reservar(bd, itens, ref lstCotaItemID, lojaID);

        }

        /// <summary>
        /// Faz a reserva do pacote. Retorna os ingressosIDs reservados. Se retornar null, nao pôde reservar o pacote.
        /// </summary>
        public int[] Reservar(CTLib.BD db, DataTable itens, ref List<EstruturaIngressoCotaItem> lstIngressoCotaItem, int lojaID)
        {
            int apresentacaoIDaux = 0, apresentacaoSetorIDaux = 0;
            bool mudou = false;
            EstruturaCotasInfo cotasInfo = new EstruturaCotasInfo();
            EstruturaPrecoReservaSite ePreco;
            List<EstruturaCotaItemReserva> listaCotaItem = new List<EstruturaCotaItemReserva>();
            EstruturaIngressoCotaItem estruturaItem;
            int usuarioID = UsuarioIDLogado;

            if (usuarioID == 0)
                throw new PacoteException("Usuário nao pode ser nulo.");

            try
            {

                bd.IniciarTransacao();

                ArrayList ingressosIDs = new ArrayList();
                CotaItem oCotaItem = new CotaItem();
                Ingresso ingresso = new Ingresso();

                Random rnd = new Random((int)System.DateTime.Now.Ticks);
                int numRan = rnd.Next(1, 999999);
                int cotaItemID = 0;
                string pacoteGrupo = numRan.ToString("000000");
                string precoNome = string.Empty;

                foreach (DataRow item in itens.Rows)
                {
                    int eventoID = (int)item["EventoID"];
                    int precoID = (int)item["PrecoID"];
                    int cortesiaID = (int)item["CortesiaID"];
                    int qtde = (int)item["Quantidade"];
                    int apresentacaoSetorID = (int)item["ApresentacaoSetorID"];
                    int apresentacaoID = Convert.ToInt32(item["ApresentacaoID"]);

                    Enumerators.TipoCodigoBarra tipoCodigoBarra = (Enumerators.TipoCodigoBarra)Convert.ToChar(item["TipoCodigoBarra"]);
                    precoNome = item["PrecoNome"].ToString();
                    cotasInfo.CotaID_Apresentacao = Convert.ToInt32(item["ApresentacaoCotaID"]);
                    cotasInfo.CotaID_ApresentacaoSetor = Convert.ToInt32(item["ApresentacaoSetorCotaID"]);
                    cotasInfo.QuantidadeApresentacao = Convert.ToInt32(item["QuantidadeApresentacao"]);
                    cotasInfo.QuantidadePorClienteApresentacao = Convert.ToInt32(item["QuantidadePorClienteApresentacao"]);
                    cotasInfo.QuantidadeApresentacaoSetor = Convert.ToInt32(item["QuantidadeApresentacaoSetor"]);
                    cotasInfo.QuantidadePorClienteApresentacaoSetor = Convert.ToInt32(item["QuantidadePorClienteApresentacaoSetor"]);

                    //Busca as Cotas
                    ePreco = new EstruturaPrecoReservaSite();
                    ePreco.PrecoNome = precoNome;
                    ePreco.ID = precoID;
                    ePreco.Quantidade = 1;
                    if (apresentacaoID != apresentacaoIDaux || apresentacaoSetorID != apresentacaoSetorIDaux)
                    {
                        apresentacaoIDaux = apresentacaoID;
                        apresentacaoSetorIDaux = apresentacaoSetorID;
                        mudou = true;
                        listaCotaItem = new List<EstruturaCotaItemReserva>();
                        if (cotasInfo.CotaID_Apresentacao != 0 || cotasInfo.CotaID_ApresentacaoSetor != 0)
                            listaCotaItem = oCotaItem.getListaCotaItemReserva(cotasInfo.CotaID_Apresentacao, cotasInfo.CotaID_ApresentacaoSetor);

                    }
                    else
                        mudou = false;

                    cotasInfo.ApresentacaoID = apresentacaoID;
                    cotasInfo.ApresentacaoSetorID = apresentacaoSetorID;

                    List<int> lstCotasItemIDs = new List<int>();
                    // Dispara Exception e nao deixa reservar\
                    if (listaCotaItem.Count != 0 && mudou)
                    {
                        lstCotasItemIDs = oCotaItem.getQuantidadeQPodeReservarCota(listaCotaItem, ePreco, cotasInfo);
                        if (lstCotasItemIDs.Count > 0 && (lstCotasItemIDs[0].Equals(-1) || lstCotasItemIDs[1].Equals(-1)))
                            throw new BilheteriaException("Um dos preços do pacote selecionado excedeu o limite de Reserva/Venda.", Bilheteria.CodMensagemReserva.PrecoIndisponivel);
                    }
                    else if (listaCotaItem.Count == 0)
                        cotaItemID = 0;
                    string sql = "SELECT Top " + qtde + " ID, CodigoSequencial, CodigoBarra " +
                        "FROM tIngresso (NOLOCK) " +
                        "WHERE ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.DISPONIVEL + "' " +
                        "ORDER BY Codigo";
                    bd.Consulta(sql);
                    int codigoSequencial = 0;
                    int[] ingressoID = new int[qtde];
                    string[] codigoBarra = new string[qtde];
                    int c = 0;
                    while (bd.Consulta().Read())
                    {
                        int id = bd.LerInt("ID");
                        codigoSequencial = bd.LerInt("CodigoSequencial");
                        ingressoID[c] = id;
                        codigoBarra[c] = bd.LerString("CodigoBarra");
                        c++;
                    }
                    bd.Consulta().Close();
                    if (c != qtde)
                    {
                        bd.DesfazerTransacao();
                        return null;
                    }
                    else
                    {
                        ingresso.UsuarioID.Valor = usuarioID;
                        ingresso.PrecoID.Valor = precoID;
                        ingresso.CortesiaID.Valor = cortesiaID;
                        ingresso.PacoteID.Valor = this.Control.ID;
                        ingresso.PacoteGrupo.Valor = pacoteGrupo;
                        ingresso.LojaID.Valor = lojaID;

                        for (int i = 0; i < ingressoID.Length; i++)
                        {
                            int id = ingressoID[i];
                            if (tipoCodigoBarra == Enumerators.TipoCodigoBarra.ListaBranca)
                                ingresso.CodigoBarra.Valor = codigoBarra[i];

                            ingresso.Control.ID = id;
                            string sqlReservar = ingresso.StringReservar();
                            int x = bd.Executar(sqlReservar);
                            bool okR = (x == 1);
                            if (!okR)
                            {
                                bd.DesfazerTransacao();
                                return null;
                            }
                            else
                            {
                                EstruturaCotaItemReserva itemEncontrado = new EstruturaCotaItemReserva();
                                itemEncontrado = listaCotaItem.Find(delegate(EstruturaCotaItemReserva itemBusca)
                               {
                                   return (itemBusca.ID == cotaItemID);
                               });

                                if (itemEncontrado == null)
                                {
                                    itemEncontrado = new EstruturaCotaItemReserva();
                                }

                                estruturaItem = new EstruturaIngressoCotaItem();
                                if (lstCotasItemIDs.Count > 0)
                                    estruturaItem.CotaItemID = lstCotasItemIDs[1] != lstCotasItemIDs[0] && lstCotasItemIDs[1] > 0 ?
                                        lstCotasItemIDs[1] : lstCotasItemIDs[0];

                                estruturaItem.IngressoID = id;
                                estruturaItem.QuantidadePorCliente_CotaItem = itemEncontrado.QuantidadePorCliente;
                                estruturaItem.QuantidadePorCliente_ApresentacaoSetor = cotasInfo.QuantidadePorClienteApresentacaoSetor;
                                estruturaItem.QuantidadePorCliente_Apresentacao = cotasInfo.QuantidadePorClienteApresentacao;

                                estruturaItem.QuantidadeCotaItem = cotasInfo.QuantidadeCota;
                                estruturaItem.Quantidade_Apresentacao = cotasInfo.QuantidadeApresentacao;
                                estruturaItem.Quantidade_ApresentacaoSetor = cotasInfo.QuantidadeApresentacaoSetor;
                                estruturaItem.Nominal = cotasInfo.Nominal;

                                lstIngressoCotaItem.Add(estruturaItem);
                                ingressosIDs.Add(id);
                            }
                        }
                    }

                }//foreach(DataRow item in itens.Rows)

                ingressosIDs.TrimToSize();

                bd.FinalizarTransacao();

                return (int[])ingressosIDs.ToArray(typeof(int));

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /*public int[] ReservarAssinatura(CTLib.BD db, DataTable itens,int LugarID)
        {

            int usuarioID = UsuarioIDLogado;

            if (usuarioID == 0)
                throw new PacoteException("Usuário nao pode ser nulo.");

            try
            {

                bd.IniciarTransacao();

                ArrayList ingressosIDs = new ArrayList();

                Ingresso ingresso = new Ingresso();

                

                foreach (DataRow item in itens.Rows)
                {
                    int eventoID = (int)item["EventoID"];
                    int precoID = (int)item["PrecoID"];
                    int cortesiaID = (int)item["CortesiaID"];
                    int qtde = (int)item["Quantidade"];
                    int apresentacaoSetorID = (int)item["ApresentacaoSetorID"];


                    string sql = "SELECT ID " +
                        "FROM tIngresso (NOLOCK) " +
                        "WHERE ApresentacaoSetorID=" + apresentacaoSetorID + " AND Status='" + Ingresso.DISPONIVEL + "'AND LugarID ="+LugarID+
                        " ORDER BY Codigo";
                    bd.Consulta(sql);

                    int[] ingressoID = new int[qtde];

                    int c = 0;
                    while (bd.Consulta().Read())
                    {
                        int id = bd.LerInt("ID");
                        ingressoID[c++] = id;
                    }
                    bd.Consulta().Close();
                    if (c != qtde)
                    {
                        bd.DesfazerTransacao();
                        return null;
                    }
                    else
                    {
                        ingresso.UsuarioID.Valor = usuarioID;
                        ingresso.PrecoID.Valor = precoID;
                        ingresso.CortesiaID.Valor = cortesiaID;
                        ingresso.PacoteID.Valor = this.Control.ID;
                        ingresso.GerarCodigoBarra(precoID, eventoID); //aki demora
                        ingresso.PacoteGrupo.Valor = pacoteGrupo;
                        foreach (int id in ingressoID)
                        {
                            ingresso.Control.ID = id;
                            string sqlReservar = ingresso.StringReservar();
                            int x = bd.Executar(sqlReservar);
                            bool okR = (x == 1);
                            if (!okR)
                            {
                                bd.DesfazerTransacao();
                                return null;
                            }
                            else
                            {
                                ingressosIDs.Add(id);
                            }
                        }
                    }

                }//foreach(DataRow item in itens.Rows)

                ingressosIDs.TrimToSize();

                bd.FinalizarTransacao();

                return (int[])ingressosIDs.ToArray(typeof(int));

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }*/

        //public Ingresso[] ReservarPistaNovo(BD bd, DataTable itens, int quantidade, int usuarioID, int lojaID)
        //{
        //    try
        //    {
        //        List<Ingresso> ingressos = new List<Ingresso>();

        //        List<Ingresso> ids = new List<Ingresso>();
        //        Ingresso ingresso = new Ingresso(); // Ingresso auxiliar.


        //        Random rnd = new Random((int)System.DateTime.Now.Ticks);
        //        int numRan = rnd.Next(1, 999999);

        //        string pacoteGrupo = numRan.ToString("000000");

        //        foreach (DataRow item in itens.Rows)
        //        {
        //            int eventoID = (int)item["EventoID"];
        //            int precoID = (int)item["PrecoID"];
        //            int cortesiaID = (int)item["CortesiaID"];
        //            int qtde = (int)item["Quantidade"];
        //            int apresentacaoSetorID = (int)item["ApresentacaoSetorID"];

        //            string sql = "SELECT Top " + qtde + " tIngresso.ID, Codigo " +
        //                    "FROM tIngresso (NOLOCK) " +
        //                    "WHERE tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND tIngresso.Status='" + Ingresso.DISPONIVEL + "' " +
        //                    "ORDER BY Codigo";
        //            bd.Consulta(sql);
        //            int codigoSequencial = 0;

        //            ids.Clear();

        //            while (bd.Consulta().Read())
        //            {
        //                ingresso = new Ingresso();
        //                ingresso.Control.ID = bd.LerInt("ID");
        //                ingresso.Codigo.Valor = bd.LerString("Codigo");
        //                ingresso.EventoID.Valor = eventoID;
        //                ingresso.ApresentacaoID.Valor = bd.LerInt("ApresentacaoID");
        //                ingresso.SetorID.Valor = bd.LerInt("SetorID");
        //                ingresso.PrecoID.Valor = precoID;
        //                ingresso.CotaItemID = 0;
        //                ingresso.UsuarioID.Valor = usuarioID;
        //                ingresso.PrecoID.Valor = precoID;
        //                ingresso.CortesiaID.Valor = cortesiaID;
        //                ingresso.PacoteID.Valor = this.Control.ID;
        //                CodigoBarra oCodigoBarra = new CodigoBarra();
        //                ingresso.CodigoBarra.Valor = oCodigoBarra.NovoCodigoBarraIngresso(precoID, eventoID, codigoSequencial);
        //                ingresso.PacoteGrupo.Valor = pacoteGrupo;
        //                ids.Add(ingresso);
        //            }
        //            bd.Consulta().Close();
        //            if (ids.Count != qtde)
        //            {
        //                bd.DesfazerTransacao();
        //                return null;
        //            }
        //            else
        //            {
        //                foreach (Ingresso id in ids)
        //                {
        //                    ingresso = new Ingresso();

        //                    // Define os parâmetros em comum
        //                    ingresso.UsuarioID.Valor = usuarioID;
        //                    ingresso.PrecoID.Valor = precoID;
        //                    ingresso.PacoteID.Valor = this.Control.ID;
        //                    ingresso.CodigoBarra.Valor = id.CodigoBarra.Valor;
        //                    ingresso.PacoteGrupo.Valor = id.PacoteGrupo.Valor;
        //                    ingresso.LojaID.Valor = lojaID;
        //                    ingresso.PacoteGrupo.Valor = pacoteGrupo.ToString();
        //                    ingresso.PacoteID.Valor = this.Control.ID;
        //                    ingresso.Control.ID = id.Control.ID;
        //                    ingresso.Codigo.Valor = id.Codigo.Valor;
        //                    ingresso.EventoID.Valor = id.EventoID.Valor;
        //                    ingresso.ApresentacaoID.Valor = id.ApresentacaoID.Valor;
        //                    ingresso.SetorID.Valor = id.SetorID.Valor;
        //                    ingresso.PrecoID.Valor = id.PrecoID.Valor;
        //                    ingresso.TxConv = id.TxConv;
        //                    ingresso.CotaItemID = id.CotaItemID;
        //                    ingressos.Add(ingresso);
        //                }
        //            }

        //        }//foreach(DataRow item in itens.Rows)

        //        return ingressos.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}


        public List<Ingresso> ReservarAssinaturaInternet(CTLib.BD db, DataTable itens, int usuarioID, string sessionID, int clienteID, int lojaID, int quantidade, decimal pctTaxa, int lugarID)
        {
            try
            {
                Random rnd = new Random();
                BD bdReserva = new BD();
                Cota oCota = new Cota();
                CotaItem oCotaItem = new CotaItem();
                //List<EstruturaCotasReserva> cotasSetor;
                //List<EstruturaCotasReserva> cotasApresentacao;
                EstruturaPrecoReservaSite ePreco;
                EstruturaCotasInfo cotasInfo;
                List<EstruturaCotaItemReserva> listaCotaItem = new List<EstruturaCotaItemReserva>();
                List<Ingresso> ingressos = new List<Ingresso>();


                Ingresso ingresso = new Ingresso(); // Ingresso auxiliar.

                decimal valorTotalPacote = 0;
                valorTotalPacote = Convert.ToDecimal(itens.Compute("SUM(ValorItem)", string.Empty));

                int pacoteGrupo = rnd.Next(0, 999999);
                DateTime timeStamp = DateTime.Now;

                int apresentacaoIDaux = 0;
                int apresentacaoSetorIDaux = 0;
                bool mudou = false;


                // Toda a reserva do pacote deve estar em transação. Tudo ou nada.
                bdReserva.IniciarTransacao();
                pacoteGrupo++;

                int indicePacote = 0;

                // Varre os itens do pacote
                foreach (DataRow item in itens.Rows)
                {
                    cotasInfo = new EstruturaCotasInfo();

                    int eventoID = (int)item["EventoID"];
                    int precoID = (int)item["PrecoID"];
                    int qtde = (int)item["Quantidade"];
                    int apresentacaoSetorID = (int)item["ApresentacaoSetorID"];
                    int apresentacaoID = (int)item["ApresentacaoID"];
                    string precoNome = item["PrecoNome"].ToString();
                    int cortesiaId = Convert.ToInt32(item["CortesiaID"]);
                    decimal valorTaxaProcessamento = pctTaxa == 0 ? 0 : Convert.ToDecimal(item["ValorTaxaProcessamento"]);

                    cotasInfo.CotaID_Apresentacao = Convert.ToInt32(item["ApresentacaoCotaID"]);
                    cotasInfo.CotaID_ApresentacaoSetor = Convert.ToInt32(item["ApresentacaoSetorCotaID"]);
                    cotasInfo.QuantidadeApresentacao = Convert.ToInt32(item["QuantidadeApresentacao"]);
                    cotasInfo.QuantidadePorClienteApresentacao = Convert.ToInt32(item["QuantidadePorClienteApresentacao"]);
                    cotasInfo.QuantidadeApresentacaoSetor = Convert.ToInt32(item["QuantidadeApresentacaoSetor"]);
                    cotasInfo.QuantidadePorClienteApresentacaoSetor = Convert.ToInt32(item["QuantidadePorClienteApresentacaoSetor"]);



                    //Busca as Cotas
                    ePreco = new EstruturaPrecoReservaSite();
                    ePreco.PrecoNome = precoNome;
                    ePreco.ID = precoID;
                    ePreco.Quantidade = qtde;
                    ePreco.QuantidadeMapa = 1;
                    if (apresentacaoID != apresentacaoIDaux || apresentacaoSetorID != apresentacaoSetorIDaux)
                    {
                        apresentacaoIDaux = apresentacaoID;
                        apresentacaoSetorIDaux = apresentacaoSetorID;
                        mudou = true;
                        listaCotaItem = new List<EstruturaCotaItemReserva>();
                        if (cotasInfo.CotaID_Apresentacao != 0 || cotasInfo.CotaID_ApresentacaoSetor != 0)
                            listaCotaItem = oCotaItem.getListaCotaItemReserva(cotasInfo.CotaID_Apresentacao, cotasInfo.CotaID_ApresentacaoSetor);

                    }
                    else
                        mudou = false;

                    cotasInfo.ApresentacaoID = apresentacaoID;
                    cotasInfo.ApresentacaoSetorID = apresentacaoSetorID;
                    List<int> lstCotaItemIDs = new List<int>();

                    //Verifica se é possivel reservar o ingresso apartir das cotas geradas p/ a apresentacao/setor
                    //Dispara Exception e nao deixa reservar\
                    if (listaCotaItem.Count != 0 && mudou)
                    {
                        lstCotaItemIDs = oCotaItem.getQuantidadeQPodeReservarCota(listaCotaItem, ePreco, cotasInfo);
                        if (lstCotaItemIDs.Count > 0 && (lstCotaItemIDs[0].Equals(-1) || lstCotaItemIDs[1].Equals(-1)))
                            throw new BilheteriaException("A quantidade de venda do preço \"" + precoNome + "\" foi excedida.", Bilheteria.CodMensagemReserva.PrecoIndisponivel);
                    }

                    while (lstCotaItemIDs.Count < 2)
                        lstCotaItemIDs.Add(0);

                    if (cortesiaId == 0 && pctTaxa == 0 && Convert.ToDecimal(item["ValorItem"]) == 0)
                    {
                        cortesiaId = new Cortesia().CortesiaPadraoEvento(eventoID);
                        if (cortesiaId <= 0)
                            throw new Exception("Desculpe mas nenhuma cortesia foi cadastrada para este evento, tente novamente mais tarde.");
                    }
                    // Busca a qtd de ingressos do item.
                    string sql = "SELECT Top 1 tIngresso.ID, Codigo, EventoID, ApresentacaoID, SetorID " +
                        "FROM tIngresso (NOLOCK) " +
                        "WHERE tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND LugarID = " + lugarID + " AND tIngresso.Status='" + Ingresso.DISPONIVEL + "' " +
                        "ORDER BY Codigo";

                    bd.Consulta(sql);

                    while (lstCotaItemIDs.Count < 2)
                        lstCotaItemIDs.Add(0);
                    // atribui os ingressos encontrados em um vetor
                    if (bd.Consulta().Read())
                    {
                        ingresso = new Ingresso();
                        ingresso.Control.ID = bd.LerInt("ID");
                        ingresso.Codigo.Valor = bd.LerString("Codigo");
                        ingresso.EventoID.Valor = bd.LerInt("EventoID");
                        ingresso.ApresentacaoID.Valor = bd.LerInt("ApresentacaoID");
                        ingresso.SetorID.Valor = bd.LerInt("SetorID");
                        ingresso.PrecoID.Valor = precoID;
                        ingresso.CotaItemID = lstCotaItemIDs[0];
                        ingresso.CotaItemIDAPS = lstCotaItemIDs[1];
                        ingresso.LugarID.Valor = lugarID;
                        ingresso.LojaID.Valor = lojaID;
                        ingresso.UsuarioID.Valor = usuarioID;
                        ingresso.SessionID.Valor = sessionID;
                        ingresso.ClienteID.Valor = clienteID;
                        ingresso.TimeStampReserva.Valor = timeStamp;
                        ingresso.PacoteGrupo.Valor = pacoteGrupo.ToString();
                        ingresso.PacoteID.Valor = this.Control.ID;
                        ingresso.CortesiaID.Valor = cortesiaId;
                        ingresso.TaxaProcessamentoValor = valorTaxaProcessamento;

                        // Atribuir a taxa de conveniência apenas para o primeiro item da lista de ingressos
                        if (indicePacote == 0 && valorTaxaProcessamento == 0)
                            ingresso.TxConv = pctTaxa;//taxa min e maxima foram calculadas antes.
                        else
                            ingresso.TxConv = 0;

                        indicePacote++;

                        //if (valorTotalPacote == 0 && pctTaxa == 0)
                        //    throw new Exception("Não foi possível os ingressos desta assinatura, um dos ingressos não possui preço associado.");

                        bd.Consulta().Close();

                        string sqlReservar = ingresso.StringReservar();

                        if (bdReserva.Executar(sqlReservar) != 1)
                        {
                            bdReserva.DesfazerTransacao();
                            throw new Exception("Não foi possível efetuar a reserva desta assinatura, por favor tente novamente.");
                        }
                        ingressos.Add(ingresso);

                    }
                    else
                        throw new Exception("Um dos ingressos desta assinatura não está disponível, por favor tente outro assento.");

                    cortesiaId = 0;
                }//foreach(DataRow item in itens.Rows)


                bdReserva.FinalizarTransacao();
                return ingressos;
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Faz a reserva do pacote. Retorna os ingressosIDs reservados. Se retornar null, nao pôde reservar o pacote.
        /// </summary>
        public Ingresso[] ReservarInternet(CTLib.BD db, DataTable itens, int usuarioID, string sessionID, int clienteID, int lojaID, int quantidade, decimal pctTaxa)
        {

            try
            {
                var rnd = new Random();
                var bdReserva = new BD();
                var oCotaItem = new CotaItem();
                var listaCotaItem = new List<EstruturaCotaItemReserva>();
                var ingressos = new List<Ingresso>();

                var ids = new List<Ingresso>();

                var pacoteGrupo = rnd.Next(0, 999999);
                var timeStamp = DateTime.Now;

                var apresentacaoIDaux = 0;
                var apresentacaoSetorIDaux = 0;
                for (var i = 0; i < quantidade; i++)
                {
                    // Toda a reserva do pacote deve estar em transação. Tudo ou nada.
                    bdReserva.IniciarTransacao();
                    pacoteGrupo++;

                    var indicePacote = 0;

                    // Varre os itens do pacote
                    foreach (DataRow item in itens.Rows)
                    {
                        var cotasInfo = new EstruturaCotasInfo();

                        var eventoID = (int)item["EventoID"];
                        var precoID = (int)item["PrecoID"];
                        var qtde = (int)item["Quantidade"];
                        var apresentacaoSetorID = (int)item["ApresentacaoSetorID"];
                        var apresentacaoID = (int)item["ApresentacaoID"];
                        var precoNome = item["PrecoNome"].ToString();
                        var cortesiaID = Convert.ToInt32(item["CortesiaID"]);
                        cotasInfo.CotaID_Apresentacao = Convert.ToInt32(item["ApresentacaoCotaID"]);
                        cotasInfo.CotaID_ApresentacaoSetor = Convert.ToInt32(item["ApresentacaoSetorCotaID"]);
                        cotasInfo.QuantidadeApresentacao = Convert.ToInt32(item["QuantidadeApresentacao"]);
                        cotasInfo.QuantidadePorClienteApresentacao = Convert.ToInt32(item["QuantidadePorClienteApresentacao"]);
                        cotasInfo.QuantidadeApresentacaoSetor = Convert.ToInt32(item["QuantidadeApresentacaoSetor"]);
                        cotasInfo.QuantidadePorClienteApresentacaoSetor = Convert.ToInt32(item["QuantidadePorClienteApresentacaoSetor"]);
                        var valorTaxaProcessamento = pctTaxa == 0 ? 0 : Convert.ToDecimal(item["ValorTaxaProcessamento"]); //Não tem taxa de conveniencia? Ignora a taxa e processamento


                        //Busca as Cotas
                        var ePreco = new EstruturaPrecoReservaSite
                        {
                            PrecoNome = precoNome,
                            ID = precoID,
                            Quantidade = qtde,
                            QuantidadeMapa = 1 * quantidade
                        };


                        bool mudou;

                        if (apresentacaoID != apresentacaoIDaux || apresentacaoSetorID != apresentacaoSetorIDaux)
                        {
                            apresentacaoIDaux = apresentacaoID;
                            apresentacaoSetorIDaux = apresentacaoSetorID;
                            mudou = true;
                            listaCotaItem = new List<EstruturaCotaItemReserva>();
                            if (cotasInfo.CotaID_Apresentacao != 0 || cotasInfo.CotaID_ApresentacaoSetor != 0)
                                listaCotaItem = oCotaItem.getListaCotaItemReserva(cotasInfo.CotaID_Apresentacao, cotasInfo.CotaID_ApresentacaoSetor);

                        }
                        else
                            mudou = false;

                        cotasInfo.ApresentacaoID = apresentacaoID;
                        cotasInfo.ApresentacaoSetorID = apresentacaoSetorID;
                        var lstCotaItemIDs = new List<int>();

                        //Verifica se é possivel reservar o ingresso apartir das cotas geradas p/ a apresentacao/setor
                        //Dispara Exception e nao deixa reservar\
                        if (listaCotaItem.Count != 0 && mudou)
                        {
                            lstCotaItemIDs = oCotaItem.getQuantidadeQPodeReservarCota(listaCotaItem, ePreco, cotasInfo);
                            if (lstCotaItemIDs.Count > 0 && (lstCotaItemIDs[0].Equals(-1) || lstCotaItemIDs[1].Equals(-1)))
                                throw new BilheteriaException(string.Format("A quantidade de venda do preço '{0}' foi excedida.", precoNome), Bilheteria.CodMensagemReserva.PrecoIndisponivel);
                        }

                        while (lstCotaItemIDs.Count < 2)
                            lstCotaItemIDs.Add(0);

                        // Busca a qtd de ingressos do item.
                        var sql = string.Format(@"SELECT TOP {0} tIngresso.ID, Codigo, EventoID, ApresentacaoID, SetorID
                                    FROM tIngresso (NOLOCK)
                                    WHERE tIngresso.ApresentacaoSetorID = {1} AND tIngresso.Status = '{2}'
                                    ORDER BY NEWID()", qtde, apresentacaoSetorID, Ingresso.DISPONIVEL);

                        bd.Consulta(sql);

                        ids.Clear();

                        if (Convert.ToDecimal(item["ValorItem"]) == 0 && pctTaxa == 0 && cortesiaID == 0)
                        {
                            cortesiaID = new Cortesia().CortesiaPadraoEvento(eventoID);
                            if (cortesiaID <= 0)
                                throw new Exception("Desculpe, mas nenhuma cortesia foi cadastrada para este evento. Tente novamente mais tarde.");
                        }

                        while (lstCotaItemIDs.Count < 2)
                            lstCotaItemIDs.Add(0);

                        Ingresso ingresso; // Ingresso auxiliar.

                        // Joga os ingressos encontrados em um vetor
                        while (bd.Consulta().Read())
                        {
                            ingresso = new Ingresso
                            {
                                Control = { ID = bd.LerInt("ID") },
                                Codigo = { Valor = bd.LerString("Codigo") },
                                EventoID = { Valor = bd.LerInt("EventoID") },
                                ApresentacaoID = { Valor = bd.LerInt("ApresentacaoID") },
                                SetorID = { Valor = bd.LerInt("SetorID") },
                                PrecoID = { Valor = precoID },
                                CotaItemID = lstCotaItemIDs[0],
                                CotaItemIDAPS = lstCotaItemIDs[1],
                                CortesiaID = { Valor = cortesiaID },
                                TaxaProcessamentoValor = valorTaxaProcessamento
                            };

                            // Atribuir a taxa de conveniência apenas para o primeiro item da lista de ingressos
                            if (indicePacote == 0 && valorTaxaProcessamento == 0)
                                ingresso.TxConv = pctTaxa;//taxa min e maxima foram calculadas antes.
                            else
                                ingresso.TxConv = 0;

                            indicePacote++;

                            ids.Add(ingresso);
                        }

                        bd.Consulta().Close();

                        // Verifica se a quantidade encontrada é suficiente para o pacote
                        if (ids.Count != qtde)
                        {
                            // Se não for, desfaz a transação.
                            bd.DesfazerTransacao();
                            bdReserva.DesfazerTransacao();
                            return null;
                        }
                        else
                        {
                            foreach (var id in ids)
                            {
                                ingresso = new Ingresso();

                                // Define os parâmetros em comum
                                ingresso.UsuarioID.Valor = usuarioID;
                                ingresso.PrecoID.Valor = precoID;
                                ingresso.PacoteID.Valor = Control.ID;
                                ingresso.PacoteGrupo.Valor = pacoteGrupo.ToString();
                                ingresso.LojaID.Valor = lojaID;
                                ingresso.SessionID.Valor = sessionID;
                                ingresso.ClienteID.Valor = clienteID;
                                ingresso.TimeStampReserva.Valor = timeStamp;
                                ingresso.PacoteGrupo.Valor = pacoteGrupo.ToString();
                                ingresso.PacoteID.Valor = this.Control.ID;
                                ingresso.Control.ID = id.Control.ID;
                                ingresso.Codigo.Valor = id.Codigo.Valor;
                                ingresso.EventoID.Valor = id.EventoID.Valor;
                                ingresso.ApresentacaoID.Valor = id.ApresentacaoID.Valor;
                                ingresso.SetorID.Valor = id.SetorID.Valor;
                                ingresso.PrecoID.Valor = id.PrecoID.Valor;
                                ingresso.TxConv = id.TxConv;
                                ingresso.CotaItemID = id.CotaItemID;
                                ingresso.CotaItemIDAPS = id.CotaItemIDAPS;
                                ingresso.CortesiaID.Valor = id.CortesiaID.Valor;
                                ingresso.TaxaProcessamentoValor = id.TaxaProcessamentoValor;

                                var sqlReservar = ingresso.StringReservar();
                                var x = bdReserva.Executar(sqlReservar);
                                var okR = (x == 1);

                                if (!okR)
                                {
                                    bdReserva.DesfazerTransacao();
                                    return null;
                                    //throw new PacoteException("Itens do pacote esgotados");                                                                        
                                }
                                else
                                {
                                    ingressos.Add(ingresso);
                                }

                            }
                        }


                    }

                    bdReserva.FinalizarTransacao();
                }

                return ingressos.ToArray();

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna o valor total do pacote
        /// </summary>
        /// <returns></returns>
        public override decimal Valor()
        {

            try
            {

                string sql = "SELECT SUM(pe.Valor * pi.Quantidade) AS Valor " +
                    "FROM tPreco as pe, tPacote as pa, tPacoteItem as pi " +
                    "WHERE pe.ID=pi.PrecoID AND pi.PacoteID=pa.ID AND pa.ID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                decimal valor = (obj != null) ? (decimal)obj : 0;

                return valor;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Retorna o valor total do pacote utilizando conexão já criada.
        /// </summary>
        /// <returns></returns>
        public decimal Valor(CTLib.BD database)
        {

            try
            {

                string sql = "SELECT SUM(pe.Valor * pi.Quantidade) AS Valor " +
                    "FROM tPreco as pe, tPacote as pa, tPacoteItem as pi " +
                    "WHERE pe.ID=pi.PrecoID AND pi.PacoteID=pa.ID AND pa.ID=" + this.Control.ID;

                object obj = database.ConsultaValor(sql);
                return (obj != null) ? (decimal)obj : 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta();
            }
        }

        /// <summary>		
        /// Obtem a quantidade de ingressos vendidos com esse preço. Se nao foi vendido nenhum ingresso, retorna zero.
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeVendido()
        {

            try
            {

                string sql = "SELECT Count(ID) AS Qtde " +
                    "FROM tVendaBilheteriaItem " +
                    "WHERE PacoteID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                int qtde = (obj != null) ? (int)obj : 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obtem a quatidade de itens do pacote
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeItens()
        {

            try
            {

                string sql = "SELECT Count(*) AS Qtde " +
                    "FROM tPacoteItem " +
                    "WHERE PacoteID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                int qtde = (obj != null) ? Convert.ToInt32(obj) : 0;

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Gerar o pacote e cadastra-os nos canais que poderam vender.
        /// </summary>
        /// <param name="info">DataSet contendo todas as informações para gerar o pacote.
        public int Novo(DataSet info)
        {

            try
            {

                //passo 1 - inserir pacote
                this.Nome.Valor = (string)info.Tables["Pacote"].Rows[0]["Nome"];
                this.VendaDistribuida.Valor = (bool)info.Tables["Pacote"].Rows[0]["VendaDistribuida"];
                this.LocalID.Valor = (int)info.Tables["Pacote"].Rows[0]["LocalID"];
                this.Quantidade.Valor = (int)info.Tables["Pacote"].Rows[0]["Quantidade"];
                this.Obs.Valor = (string)info.Tables["Pacote"].Rows[0]["Obs"];

                bool pacoteOk = this.Inserir();

                if (!pacoteOk)
                    return 0;

                CodigoBarra oCodigoBarra = new CodigoBarra(this.UsuarioIDLogado);


                for (int i = 0; i < info.Tables["CanalPacote"].Rows.Count; i++)
                {

                    CanalPacote canalPacote = new CanalPacote(UsuarioIDLogado);

                    canalPacote.CanalID.Valor = (int)info.Tables["CanalPacote"].Rows[i]["CanalID"];
                    canalPacote.Quantidade.Valor = (int)info.Tables["CanalPacote"].Rows[i]["Quantidade"];
                    canalPacote.PacoteID.Valor = this.Control.ID;

                    Canal canal = new Canal();
                    canal.Ler(canalPacote.CanalID.Valor);
                    canalPacote.TaxaConveniencia.Valor = canal.TaxaConveniencia.Valor;

                    canalPacote.Inserir();

                }

                //passo 2 - inserir itens

                for (int j = 0; j < info.Tables["PacoteItem"].Rows.Count; j++)
                {

                    DataRow linhapacoteItem = info.Tables["PacoteItem"].Rows[j];

                    bool precoOk = true;

                    int precoID = (int)linhapacoteItem["PrecoID"];
                    if (precoID < 0)
                    { //eh preco novo
                        DataRow[] linhasPreco = info.Tables["Preco"].Select("ID=" + precoID);

                        Preco preco = new Preco(UsuarioIDLogado);

                        preco.ApresentacaoSetorID.Valor = (int)linhasPreco[0]["ApresentacaoSetorID"];
                        preco.Valor.Valor = (decimal)linhasPreco[0]["Valor"];
                        preco.Nome.Valor = (string)linhasPreco[0]["Nome"];
                        preco.CorID.Valor = (int)linhasPreco[0]["CorID"];
                        preco.Quantidade.Valor = (int)linhasPreco[0]["Quantidade"];
                        preco.QuantidadePorCliente.Valor = (int)linhasPreco[0]["QuantidadePorCliente"];

                        precoOk = preco.Inserir();

                        if (precoOk)
                        {
                            bd.FecharConsulta();

                            oCodigoBarra.Inserir((int)linhapacoteItem["EventoID"], (int)linhapacoteItem["ApresentacaoID"], (int)linhapacoteItem["SetorID"], preco.Control.ID, bd);

                            precoID = preco.Control.ID;
                        }
                    }

                    PacoteItem pacoteItem = new PacoteItem(UsuarioIDLogado);

                    pacoteItem.PacoteID.Valor = this.Control.ID;
                    pacoteItem.PrecoID.Valor = precoID;
                    pacoteItem.EventoID.Valor = (int)linhapacoteItem["EventoID"];
                    pacoteItem.ApresentacaoID.Valor = (int)linhapacoteItem["ApresentacaoID"];
                    pacoteItem.SetorID.Valor = (int)linhapacoteItem["SetorID"];
                    pacoteItem.CortesiaID.Valor = (int)linhapacoteItem["CortesiaID"];
                    pacoteItem.Quantidade.Valor = (int)linhapacoteItem["Quantidade"];
                    pacoteItem.Inserir();

                }

                if (this.VendaDistribuida.Valor)
                    DistribuirCanaisIR();

                return this.Control.ID;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Distribui esse pacote nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        /// <param name="bd">Objeto de conexão</param>
        internal bool DistribuirCanaisIR(BD bd)
        {
            bool ok = true;
            int canalID;
            int taxaConveniencia;

            try
            {

                EmpresaLista empresaLista = new EmpresaLista();
                empresaLista.FiltroSQL = "EmpresaVende = 'T'";
                empresaLista.FiltroSQL = "EmpresaPromove = 'F'";
                empresaLista.Carregar();

                if (empresaLista.Primeiro())
                {
                    do
                    {
                        DataTable canais = empresaLista.Empresa.Canais();
                        foreach (DataRow canal in canais.Rows)
                        {
                            canalID = Convert.ToInt32(canal["ID"]);
                            taxaConveniencia = Convert.ToInt32(canal["TaxaConveniencia"]);

                            CanalPacoteLista canalPacoteLista = new CanalPacoteLista();
                            canalPacoteLista.FiltroSQL = "CanalID=" + canalID;
                            canalPacoteLista.FiltroSQL = "PacoteID=" + this.Control.ID;
                            canalPacoteLista.Carregar();

                            if (canalPacoteLista.Tamanho == 0)
                            {

                                CanalPacote canalPacote = new CanalPacote(this.Control.UsuarioID);

                                canalPacote.CanalID.Valor = canalID;
                                canalPacote.PacoteID.Valor = this.Control.ID;
                                canalPacote.TaxaConveniencia.Valor = taxaConveniencia;
                                canalPacote.Quantidade.Valor = 0;

                                ok &= canalPacote.Inserir(ref bd);
                            }

                        }
                    } while (empresaLista.Proximo());
                }

            }
            catch
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Distribui esse pacote nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        public override bool DistribuirCanaisIR()
        {

            return DistribuirCanaisIR(bd);

        }

        /// <summary>
        /// Distribui esse pacote nos canais da Ingresso Rapido (Devolve o sucesso da operação)
        /// </summary>
        public override bool RemoverDistribuicaoCanaisIR()
        {

            bool ok = true;

            try
            {

                EmpresaLista empresaLista = new EmpresaLista();
                empresaLista.FiltroSQL = "EmpresaVende='T'";
                empresaLista.FiltroSQL = "EmpresaPromove='F'";
                empresaLista.Carregar();

                if (empresaLista.Primeiro())
                {
                    do
                    {
                        DataTable canais = empresaLista.Empresa.Canais();
                        foreach (DataRow canal in canais.Rows)
                        {
                            int canalID = (int)canal["ID"];

                            CanalPacoteLista canalPacoteLista = new CanalPacoteLista();
                            canalPacoteLista.FiltroSQL = "CanalID=" + canalID;
                            canalPacoteLista.FiltroSQL = "PacoteID=" + this.Control.ID;
                            canalPacoteLista.ExcluirTudo();

                        }
                    } while (empresaLista.Proximo());
                }

            }
            catch
            {
                ok = false;
            }

            return ok;

        }

        /// <summary>		
        /// Obtem os itens de pacote
        /// </summary>
        /// <returns></returns>
        public override DataTable Itens()
        {

            DataTable tabela = new DataTable("PacoteItem");

            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("PacoteID", typeof(int)).DefaultValue = this.Control.ID;
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("PrecoNome", typeof(string));
            tabela.Columns.Add("CortesiaID", typeof(int));
            tabela.Columns.Add("CortesiaNome", typeof(string));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("EventoNome", typeof(string));
            tabela.Columns.Add("SetorID", typeof(int));
            tabela.Columns.Add("SetorNome", typeof(string));
            tabela.Columns.Add("SetorLugarMarcado", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("ApresentacaoHorario", typeof(string));
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int)); //esse campo eh necessario por causa do AutoSelecionador
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Valor", typeof(decimal));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tPacoteItem.ID, " +
                    "   tPacoteItem.PrecoID, " +
                    "   tPacoteItem.CortesiaID, " +
                    "   tPacoteItem.Quantidade, " +
                    "   tPacoteItem.EventoID, " +
                    "   tPacoteItem.ApresentacaoID, " +
                    "   tPacoteItem.SetorID, " +
                    "   tPreco.Valor, " +
                    "   tPreco.Nome AS PrecoNome, " +
                    "   tPreco.ApresentacaoSetorID, " +
                    "   tEvento.Nome AS EventoNome, " +
                    "   tApresentacao.Horario AS ApresentacaoHorario, " +
                    "   tSetor.Nome AS SetorNome, " +
                    "   tSetor.LugarMarcado AS SetorLugarMarcado, " +
                    "   tCortesia.Nome AS CortesiaNome " +
                    "FROM tPacoteItem (NOLOCK) " +
                    "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tPacoteItem.PrecoID " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tPacoteItem.EventoID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tPacoteItem.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tPacoteItem.SetorID " +
                    "LEFT OUTER JOIN tCortesia (NOLOCK) ON tCortesia.ID = tPacoteItem.CortesiaID " +
                    "WHERE " +
                    "  tPacoteItem.PacoteID=" + this.Control.ID))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["PrecoID"] = bd.LerInt("PrecoID");
                        linha["PrecoNome"] = bd.LerString("PrecoNome");
                        linha["EventoID"] = bd.LerInt("EventoID");
                        linha["EventoNome"] = bd.LerString("EventoNome");
                        linha["SetorID"] = bd.LerInt("SetorID");
                        linha["SetorNome"] = bd.LerString("SetorNome");
                        linha["SetorLugarMarcado"] = bd.LerString("SetorLugarMarcado");
                        linha["CortesiaID"] = bd.LerInt("CortesiaID");
                        linha["CortesiaNome"] = bd.LerString("CortesiaNome");
                        linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        linha["ApresentacaoHorario"] = bd.LerStringFormatoDataHora("ApresentacaoHorario");
                        linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                        linha["Quantidade"] = bd.LerInt("Quantidade");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        tabela.Rows.Add(linha);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        public int carregaQuantidadePorPacote(int PacoteID)
        {
            try
            {
                int quantidade;

                string sql = "SELECT COUNT(I.ID) AS Qtde FROM tIngresso AS I" +
                    " WHERE (I.Status = '" + Ingresso.VENDIDO + "' OR I.Status = '" + Ingresso.PREIMPRESSO + "') AND PacoteID = " + PacoteID;

                quantidade = Convert.ToInt32(bd.ConsultaValor(sql));

                bd.Fechar();

                return quantidade;
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


        /// <summary>		
        /// Obtem os itens de pacote
        /// </summary>
        /// <returns></returns>
        public DataTable Itens(CTLib.BD database)
        {

            try
            {
                //PacoteItemLista oPacoteItemLista = new PacoteItemLista();

                string sql = "SELECT tPacoteItem.PrecoID,tPacoteItem.CortesiaID,tPacoteItem.Quantidade,tPreco.ApresentacaoSetorID " +
                    "FROM tPacoteItem (NOLOCK),tPreco (NOLOCK)" +
                    "WHERE tPreco.ID=tPacoteItem.PrecoID AND tPacoteItem.PacoteID=" + this.Control.ID;

                DataTable tabela = new DataTable("PacoteItem");
                tabela.Columns.Add("PrecoID", typeof(int));
                tabela.Columns.Add("CortesiaID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                DataRow item = null;

                while (database.Consulta(sql).Read())
                {
                    item = tabela.NewRow();

                    item["PrecoID"] = database.LerInt("PrecoID");
                    item["CortesiaID"] = database.LerInt("CortesiaID");
                    item["ApresentacaoSetorID"] = database.LerInt("ApresentacaoSetorID");
                    item["Quantidade"] = database.LerInt("Quantidade");
                    tabela.Rows.Add(item);
                }
                return tabela;

            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta();
            }
        }

        /// <summary>
        /// Exclui Pacote com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                //verificar se há ingressos vendidos com esse preço.
                VendaBilheteriaItemLista lista = new VendaBilheteriaItemLista();
                lista.FiltroSQL = "PacoteID=" + id;
                lista.TamanhoMax = 1;
                lista.Carregar();

                if (lista.Tamanho > 0)
                { //nao pode excluir
                    throw new PacoteException("Pacote não pode ser excluído pois já existem vendas com esse pacote! Desative esse pacote em todos os canais para deixar de vendê-lo.");
                }

                PacoteItemLista pacoteItemLista = new PacoteItemLista();
                pacoteItemLista.FiltroSQL = "PacoteID=" + id;
                bool ok = pacoteItemLista.ExcluirTudo();

                if (ok)
                {

                    CanalPacoteLista canalPacoteLista = new CanalPacoteLista();
                    canalPacoteLista.FiltroSQL = "PacoteID=" + id;
                    ok = canalPacoteLista.ExcluirTudo();

                    if (ok)
                        ok = base.Excluir(id);

                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui Pacote
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            return this.Excluir(this.Control.ID);

        }

        /// <summary>		
        /// Obtem informações (evento,apresentacao,setor...) de todos os ingressos desse pacote vendido passado como parametro
        /// </summary>
        /// <returns></returns>
        public override DataTable InfoVendido(int[] ingressosIDs)
        {
            //TODO: PACOTE - descobrir os ingressosIDs sem precisar passar por parametro. os ingressos tem q estar vendidos antes.

            try
            {
                if (ingressosIDs.Length > 0)
                {

                    Ingresso ingresso = new Ingresso();

                    DataTable tabela = ingresso.InfoVendidos(ingressosIDs);

                    return tabela;

                }
                else
                {
                    throw new Exception("A lista de ingressosIDs esta vazia.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obtem informações (evento,apresentacao,setor...) de todos os ingressos desse pacote reservado passado como parametro
        /// </summary>
        /// <returns></returns>
        public override DataTable InfoReservado(int[] ingressosIDs, int lojaID)
        {
            //TODO: PACOTE - descobrir os ingressosIDs sem precisar passar por parametro. os ingressos tem q estar vendidos antes.

            try
            {
                if (ingressosIDs.Length > 0)
                {

                    Ingresso ingresso = new Ingresso();

                    DataTable tabela = ingresso.InfoReservados(ingressosIDs, bd, lojaID);

                    return tabela;

                }
                else
                {
                    throw new Exception("A lista de ingressosIDs esta vazia.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obtem informações (evento,apresentacao,setor...) de todos os ingressos desse pacote reservado passado como parametro
        /// </summary>
        /// <returns></returns>
        public DataTable InfoReservado(int[] ingressosIDs, CTLib.BD bd, List<EstruturaIngressoCotaItem> lstIngressoCotaItem, int lojaID)
        {
            //TODO: PACOTE - descobrir os ingressosIDs sem precisar passar por parametro. os ingressos tem q estar vendidos antes.

            try
            {
                CotaItem oCotaItem = new CotaItem();
                if (ingressosIDs != null && ingressosIDs.Length > 0)
                {
                    //Ingresso ingresso = new Ingresso();

                    DataTable tabela = new Ingresso().InfoReservados(ingressosIDs, bd, lojaID);

                    tabela.Columns.Add(Bilheteria.COTA_ITEM_ID, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADE_COTA, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADEPORCLIENTE_COTA, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADE_APRESENTACAO, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADEPORCLIENTE_APRESENTACAO, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR, typeof(int)).DefaultValue = 0;
                    tabela.Columns.Add(Bilheteria.VALIDA_BIN, typeof(bool)).DefaultValue = false;
                    tabela.Columns.Add(Bilheteria.NOMINAL, typeof(bool)).DefaultValue = 1;

                    int cotaItemIDaux = 0;
                    bool validaBinaux = false;

                    foreach (DataRow dtr in tabela.Rows)
                    {
                        for (int i = 0; i < lstIngressoCotaItem.Count; i++)
                        {
                            if ((int)dtr["ID"] == lstIngressoCotaItem[i].IngressoID)
                            {
                                dtr[Bilheteria.COTA_ITEM_ID] = lstIngressoCotaItem[i].CotaItemID;
                                dtr[Bilheteria.QUANTIDADE_COTA] = lstIngressoCotaItem[i].QuantidadeCotaItem;
                                dtr[Bilheteria.QUANTIDADEPORCLIENTE_COTA] = lstIngressoCotaItem[i].QuantidadePorCliente_CotaItem;
                                dtr[Bilheteria.QUANTIDADE_APRESENTACAO] = lstIngressoCotaItem[i].Quantidade_Apresentacao;
                                dtr[Bilheteria.QUANTIDADE_APRESENTACAO_SETOR] = lstIngressoCotaItem[i].Quantidade_ApresentacaoSetor;
                                dtr[Bilheteria.QUANTIDADEPORCLIENTE_APRESENTACAO] = lstIngressoCotaItem[i].QuantidadePorCliente_Apresentacao;
                                dtr[Bilheteria.QUANTIDADEPORCLIENTE_APRESENTACAO_SETOR] = lstIngressoCotaItem[i].QuantidadePorCliente_ApresentacaoSetor;
                                dtr[Bilheteria.NOMINAL] = lstIngressoCotaItem[i].Nominal;

                                if (cotaItemIDaux != lstIngressoCotaItem[i].CotaItemID)
                                {
                                    cotaItemIDaux = lstIngressoCotaItem[i].CotaItemID;
                                    validaBinaux = oCotaItem.getValidaBIN(lstIngressoCotaItem[i].CotaItemID);
                                    dtr[Bilheteria.VALIDA_BIN] = validaBinaux;
                                }
                                else
                                    dtr[Bilheteria.VALIDA_BIN] = validaBinaux;
                                break;
                            }
                        }
                    }

                    //tGrid.Columns.Add(COTA_ITEM_ID, typeof(int));
                    //tGrid.Columns.Add(COTA, typeof(string));
                    //tGrid.Columns.Add(CLIENTE, typeof(string));
                    //tReserva.Columns.Add(COTA_ITEM_ID, typeof(int));
                    //tReserva.Columns.Add(QUANTIDADEPORCLIENTE_COTA, typeof(int));
                    //tReserva.Columns.Add(QUANTIDADE_COTA, typeof(int));
                    //tReserva.Columns.Add(OBRIGATORIEDADE_ID, typeof(int));
                    //tReserva.Columns.Add(CLIENTE_ID, typeof(int));
                    //tReserva.Columns.Add(VALIDA_BIN, typeof(bool));
                    //tReserva.Columns.Add(CODIGO_PROMO, typeof(string));
                    //tReserva.Columns.Add(STATUS_CODIGO_PROMO, typeof(string)).DefaultValue = "Nao";
                    return tabela;
                }
                else
                {
                    throw new Exception("A lista de ingressosIDs esta vazia.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obtem os canais que esse pacote possui
        /// </summary>
        /// <returns></returns>
        public override DataTable Canais()
        {
            try
            {
                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ca.ID,ca.Nome " +
                    "FROM tEvento AS e, tCanalPacote AS cp, tCanal AS ca " +
                    "WHERE ca.ID=cp.CanalID AND cp.PacoteID=e.ID AND " +
                    "cp.PacoteID=" + this.Control.ID + " ORDER BY ca.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
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
        }

        /// <summary>		
        /// Obtem os canais que esse pacote possui
        /// </summary>
        /// <returns></returns>
        public DataTable Canais2()
        {
            try
            {
                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT ca.ID,ca.Nome,tEmpresa.Nome AS Empresa " +
                    "FROM tEmpresa, tEvento AS e, tCanalPacote AS cp, tCanal AS ca " +
                    "WHERE ca.EmpresaID=tEmpresa.ID AND ca.ID=cp.CanalID AND cp.PacoteID=e.ID AND " +
                    "cp.PacoteID=" + this.Control.ID + " ORDER BY ca.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome") + "/" + bd.LerString("Empresa");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obtem os canais de uma empresa que esse pacote possui
        /// </summary>
        /// <returns></returns>
        public DataTable Canais(int empresaid)
        {
            try
            {
                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tCanal.ID, tCanal.Nome " +
                    "FROM tCanalPacote, tCanal " +
                    "WHERE tCanal.ID=tCanalPacote.CanalID AND tCanal.EmpresaID=" + empresaid + " AND " +
                    "tCanalPacote.PacoteID=" + this.Control.ID + " " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
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
        }

        /// <summary>		
        /// Obtem os canais ativos de uma empresa que esse pacote possui
        /// </summary>
        /// <returns></returns>
        public DataTable CanaisAtivos(int empresaid)
        {
            try
            {
                DataTable tabela = new DataTable("Canal");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = "SELECT tCanal.ID, tCanal.Nome " +
                    "FROM tCanalPacote INNER JOIN tCanal " +
                    "ON tCanal.ID=tCanalPacote.CanalID WHERE tCanal.Ativo = 'T' AND tCanal.EmpresaID=" + empresaid + " AND " +
                    "tCanalPacote.PacoteID=" + this.Control.ID + " " +
                    "ORDER BY tCanal.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
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
        }

        /// <summary>
        /// Classe utilizada para o agrupamento de informações relacionadas ao pacote para envio à reserva.
        /// </summary>
        public class InfoToReserva
        {
            private Pacote pacote;

            private int qtdVendido = 0;
            private int qtdDisponivelCanal = 0; // Disponível para o canal e não ainda disponível para o canal.
            private int taxaConveniencia = 0; // Disponível para o canal e não ainda disponível para o canal.

            public Pacote Pacote
            { //Informações sobre o pacote.
                get { return this.pacote; }
                set { this.pacote = value; }
            }

            public int QtdVendido
            { //Quantidade vendida ou reservada do preço.
                get { return this.qtdVendido; }
                set { this.qtdVendido = value; }
            }

            public int QtdDisponivelCanal
            { //Quantidade disponibilizada para o Canal.
                get { return this.qtdDisponivelCanal; }
                set { this.qtdDisponivelCanal = value; }
            }

            public int TaxaConveniencia
            { //Taxa de Conveniencia para o Canal.
                get { return this.taxaConveniencia; }
                set { this.taxaConveniencia = value; }
            }
        }

        public InfoToReserva LerToReserva(int pacoteID, int canalID, CTLib.BD database)
        {

            try
            {
                string sql = string.Format(@"  SELECT 
                                tPacote.Nome,
                                tPacote.Quantidade PacoteQuantidade, 
                                tPacote.PermitirCancelamentoAvulso,
                                tCanalPacote.Quantidade AS CanalQuantidade, 
                                tCanalPacote.TaxaConveniencia AS TaxaConveniencia, 
                                SUM(CASE WHEN i.Status <> 'D' THEN 1 ELSE 0 END) AS QtdeVendido
                                FROM 
                                tPacote (NOLOCK)
                                INNER JOIN tCanalPacote (NOLOCK) ON tPacote.ID = tCanalPacote.PacoteID  
                                LEFT JOIN tIngresso i (NOLOCK) ON i.PacoteID = tPacote.ID
                                WHERE 
                        tPacote.ID = {0} AND tCanalPacote.CanalID = {1}
                        GROUP BY tPacote.ID, tPacote.Nome, tPacote.Quantidade, tCanalPacote.Quantidade, tPacote.PermitirCancelamentoAvulso, tCanalPacote.TaxaConveniencia", pacoteID, canalID);

                this.Limpar();

                InfoToReserva info = new InfoToReserva();

                if (!database.Consulta(sql).Read())
                    throw new Exception("O pacote selecionado não foi encontrado.");

                this.Control.ID = pacoteID;
                this.Nome.Valor = database.LerString("Nome");
                this.Quantidade.Valor = database.LerInt("PacoteQuantidade");
                this.PermitirCancelamentoAvulso.Valor = database.LerBoolean("PermitirCancelamentoAvulso");
                info.Pacote = this;
                info.QtdDisponivelCanal = database.LerInt("CanalQuantidade");
                info.QtdVendido = database.LerInt("QtdeVendido");
                info.TaxaConveniencia = database.LerInt("TaxaConveniencia");
                return info;
            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta();
            }
        }

        public int LerFromCliente(int pacoteID, int canalID, int clienteID, string sessionID, CTLib.BD database)
        {

            try
            {
                var sql1 = string.Format(@"SELECT COUNT(*) AS QtdeComprado
				 FROM tIngresso (NOLOCK) i
				 INNER JOIN tCanalPacote (NOLOCK) ON i.PacoteID = tCanalPacote.PacoteID
				 WHERE 
					tCanalPacote.CanalID = {1} AND
					i.PacoteID = {0} AND 
					i.SessionID = '{3}' AND 
					i.ClienteID = {2} AND
					i.Status in ('R','V')", pacoteID, canalID, clienteID, sessionID);

                var sql2 = "SELECT Count(tPacoteItem.ID) as QtdePacote FROM tPacoteItem (NOLOCK) where PacoteID = " + pacoteID;

                Limpar();

                if (!database.Consulta(sql1).Read())
                    throw new Exception("O pacote selecionado não foi encontrado.");

                var qtdeComprado = database.LerInt("QtdeComprado");
                database.FecharConsulta();

                if (!database.Consulta(sql2).Read())
                    throw new Exception("O pacote selecionado não foi encontrado.");

                var qtdePacote = database.LerInt("QtdePacote");

                return qtdeComprado / qtdePacote;
            }
            finally
            {
                database.FecharConsulta();
            }
        }

        public bool VetificaQuantidadePreco(int pacoteID, int canalID, int quantidadeReserva, BD database)
        {
            try
            {
                Preco preco = new Preco();
                List<int> PrecoID = new List<int>();
                bool retorno = true;

                string sql = string.Format(@"SELECT PrecoID FROM tPacoteItem WHERE PacoteID = {0} ", pacoteID);

                while (database.Consulta(sql).Read())
                    PrecoID.Add(database.LerInt("PrecoID"));

                database.FecharConsulta();

                for (int i = 0; i < PrecoID.Count; i++)
                {
                    string sqlConsulta = string.Format(@"SELECT COUNT(tIngresso.ID) AS QuantidadeVendidoPreco
                    FROM tIngresso(NOLOCK)
                    LEFT JOIN tLoja (NOLOCK) ON tLoja.ID = tIngresso.LojaID
                    LEFT JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
                    LEFT JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                    LEFT JOIN tCanalPreco (NOLOCK) ON tCanalPreco.PrecoID = tPreco.ID AND tCanalPreco.CanalID = 1
                    WHERE tIngresso.PrecoID =  {1} AND tIngresso.Status IN ('R','I','V')", canalID, PrecoID[i]);

                    database.Consulta(sqlConsulta);

                    while (database.Consulta().Read())
                    {
                        preco.Ler(PrecoID[i]);

                        int quantidadeVendida = database.LerInt("QuantidadeVendidoPreco");
                        int quantidadePreco = preco.Quantidade.Valor;
                        int quantidade = quantidadePreco - quantidadeVendida;

                        if (quantidade <= 0 && quantidadePreco > 0)
                        {
                            retorno = false;
                        }
                        else if (quantidadeReserva > quantidadePreco && quantidadePreco > 0)
                        {
                            retorno = false;
                        }
                    }

                    if (retorno == false)
                        break;
                }

                return retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                database.FecharConsulta();
            }
        }
    }

    public class PacoteLista : PacoteLista_B
    {
        public PacoteLista() { }

        public PacoteLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}
