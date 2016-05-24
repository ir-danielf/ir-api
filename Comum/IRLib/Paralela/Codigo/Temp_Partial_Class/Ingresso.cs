using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IRLib.Paralela
{
    public partial class Ingresso : Ingresso_B
    {
        public void ValidarLugarMarcadoInternet(EstruturaPrecoReservaSite preco, Setor.enumLugarMarcado tipoSetor,
            EstruturaCotasInfo cotaInfo, int serieID, EstruturaReservaInternet estruturaReserva)
        {
            int cortesiaID = 0;
            BilheteriaParalela oBilheteria = new BilheteriaParalela();
            CotaItem oCotaItem = new CotaItem();
            List<EstruturaCotaItemReserva> listaCotaItem = new List<EstruturaCotaItemReserva>();
            if (cotaInfo.CotaID_Apresentacao != 0 || cotaInfo.CotaID_ApresentacaoSetor != 0)
                listaCotaItem = oCotaItem.getListaCotaItemReserva(cotaInfo.CotaID_Apresentacao, cotaInfo.CotaID_ApresentacaoSetor);

            cotaInfo.ApresentacaoID = this.ApresentacaoID.Valor;
            cotaInfo.ApresentacaoSetorID = this.ApresentacaoSetorID.Valor;
            List<int> retornoCotaItens = new List<int>(2);

            //Dispara Exception e nao deixa reservar
            if (listaCotaItem.Count != 0)
            {
                retornoCotaItens = oCotaItem.getQuantidadeQPodeReservarCota(listaCotaItem, preco, cotaInfo);
                if (retornoCotaItens.Count > 0 && (retornoCotaItens[0].Equals(-1) || retornoCotaItens[1].Equals(-1)))
                    throw new BilheteriaException("Não será possível reservar o preço \"" + preco.PrecoNome + "\", o limite de venda foi excedido.", BilheteriaParalela.CodMensagemReserva.PrecoIndisponivel);
            }

            decimal precoValor = 0;
            // Verifica se existe quantidade do PrecoID disponivel para venda e retorna via referencia o valor do preço
            if (preco.Quantidade != oBilheteria.GetIngressosQPodeReservar(this.ClienteID.Valor, this.SessionID.Valor, preco.ID, preco.Quantidade, ref precoValor, false, serieID, estruturaReserva))
                throw new BilheteriaException("A Quantidade disponivel para o preço \"" + preco.PrecoNome.ToString() + "\" foi excedida.", BilheteriaParalela.CodMensagemReserva.PrecoIndisponivel);

            this.TxConv = oBilheteria.TaxaConveniencia(this.EventoID.Valor, preco.ID, estruturaReserva.CanalID);

            if (this.TxConv > 0)
            {
                this.TaxaProcessamentoValor = oBilheteria.ValorTaxaProcessamento(this.EventoID.Valor);
                if (this.TaxaProcessamentoValor > 0)
                    this.TxConv = 0;
            }
            //Aqui precisa fazer a validação, é cortesia??
            if (preco.Valor == 0 && TxConv == 0 && TaxaProcessamentoValor == 0)
            {
                cortesiaID = new Cortesia().CortesiaPadraoEvento(this.EventoID.Valor);
                if (cortesiaID <= 0)
                    throw new Exception("Não possível encontrar a cortesia associada a este evento!");

                this.CortesiaID.Valor = cortesiaID;
            }


            this.PrecoID.Valor = preco.ID;
            this.CotaItemID = retornoCotaItens.Count > 0 ? retornoCotaItens[0] : 0;
            this.CotaItemIDAPS = retornoCotaItens.Count > 0 ? retornoCotaItens[1] : 0;
            this.SerieID.Valor = serieID;
        }

        public bool MudarPreco(BD bd, int precoID)
        {
            string sql = string.Format("UPDATE tIngresso SET PrecoID = {0}, CortesiaID = {1} WHERE ID = {2}", precoID, this.CortesiaID.Valor, this.Control.ID);
            return (bd.Executar(sql) > 0);
        }
        /// <summary>
        /// Método para retornar os ingressos (para reserva) dos melhores lugares de mesa fechada.
        /// kim
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <param name="lojaID"></param>
        /// <param name="sessionID"></param>
        /// <param name="clienteID"></param>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoSetorID"></param>
        /// <param name="precos"></param>
        /// <param name="preReserva"></param>
        /// <returns></returns>
        public List<Ingresso> MelhoresIngressos(int usuarioID, int lojaID, string sessionID, int clienteID, int eventoID,
            int apresentacaoID, int apresentacaoSetorID, int setorID, List<EstruturaPrecoReservaSite> precos,
            Setor.enumLugarMarcado tipoSetor, EstruturaCotasInfo cotaInfo, int serieID, EstruturaReservaInternet estruturaReserva)
        {
            BD bd = new BD(ConfigurationManager.AppSettings["ConexaoReadOnly"]);
            BD bdReserva = new BD();
            Ingresso oIngresso = new Ingresso();
            BilheteriaParalela oBilheteria = new BilheteriaParalela();
            Cota oCota = new Cota();
            CotaItem oCotaItem = new CotaItem();
            Lugar oLugar = new Lugar(); //usado para a busca de melhores lugares.
            List<Ingresso> ingressosRetorno;
            decimal precoValor;
            int p;
            decimal valorConv = 0;
            decimal valorProcessamento = 0;
            //decimal valor;
            Cortesia oCortesia = new Cortesia();
            int CortesiaPadraoID = 0;

            DateTime timeStamp = DateTime.Now.AddMinutes(new ConfigGerenciadorParalela().getValorTempoReserva());

            try
            {
                int qtdPrecos = 0;
                //Essa variavel duplica o preço e o valor pela quantidade de para reservar.
                //Dessa forma é possível reservar os lugares com os preços corretos. kim
                List<EstruturaPrecoReservaSite> precoPorLugar = new List<EstruturaPrecoReservaSite>();
                List<EstruturaPrecoReservaSite> listaPreco = new List<EstruturaPrecoReservaSite>();
                EstruturaPrecoReservaSite itemPreco;

                List<EstruturaCotaItemReserva> listaCotaItem = new List<EstruturaCotaItemReserva>();
                if (cotaInfo.CotaID_Apresentacao != 0 || cotaInfo.CotaID_ApresentacaoSetor != 0)
                    listaCotaItem = oCotaItem.getListaCotaItemReserva(cotaInfo.CotaID_Apresentacao, cotaInfo.CotaID_ApresentacaoSetor);

                cotaInfo.ApresentacaoID = apresentacaoID;
                cotaInfo.ApresentacaoSetorID = apresentacaoSetorID;
                List<int> retornoCotaItens = new List<int>(2);

                foreach (EstruturaPrecoReservaSite preco in precos)
                {
                    //Verifica se é possivel reservar o ingresso apartir das cotas geradas p/ a apresentacao/setor

                    //Dispara Exception e nao deixa reservar
                    if (listaCotaItem.Count != 0)
                    {
                        retornoCotaItens = oCotaItem.getQuantidadeQPodeReservarCota(listaCotaItem, preco, cotaInfo);
                        //cotaItemID = oCotaItem.getQuantidadeQPodeReservarCota(-2, -1, listaCotaItem, preco, cotaInfo);

                        if (retornoCotaItens.Count > 0 && (retornoCotaItens[0].Equals(-1) || retornoCotaItens[1].Equals(-1)))
                            throw new BilheteriaException("Infelizmente não será possível comprar o preço \"" + preco.PrecoNome + "\", pois o limite de venda foi excedido. <br /> Escolha um novo preço.", BilheteriaParalela.CodMensagemReserva.PrecoIndisponivel);
                    }

                    precoValor = 0;
                    // Verifica se existe quantidade do PrecoID disponivel para venda e retorna via referencia o valor do preço
                    if (preco.Quantidade != oBilheteria.GetIngressosQPodeReservar(clienteID, sessionID, preco.ID, preco.Quantidade, ref precoValor, false, serieID, estruturaReserva))
                        throw new BilheteriaException("A Quantidade disponível para o preço \"" + preco.PrecoNome.ToString() + "\" foi excedida.", BilheteriaParalela.CodMensagemReserva.PrecoIndisponivel);
                    //incrementa a quantidade 
                    qtdPrecos += preco.Quantidade;
                    //só utiliza os preços duplicados caso seja lugar marcado
                    while (retornoCotaItens.Count < 2)
                        retornoCotaItens.Add(0);

                    if (tipoSetor != IRLib.Paralela.Setor.enumLugarMarcado.Pista)
                    {

                        //duplica os registros para serem usados mais tarde na hora da reserva de lugares marcados
                        for (int x = 0; x < preco.Quantidade; x++)
                        {
                            itemPreco = new EstruturaPrecoReservaSite();
                            itemPreco.ID = preco.ID;
                            itemPreco.GerenciamentoIngressosID = preco.GerenciamentoIngressosID;
                            itemPreco.Valor = precoValor;
                            itemPreco.Quantidade = preco.Quantidade;
                            itemPreco.CotaItemID = retornoCotaItens[0];
                            itemPreco.CotaItemIDAPS = retornoCotaItens[1];
                            itemPreco.CodigoCinema = preco.CodigoCinema;
                            itemPreco.CodigoProgramacao = preco.CodigoProgramacao;
                            precoPorLugar.Add(itemPreco);
                        }
                    }
                    else
                    {
                        //gera novos registros para adicionar a cotaItemID do contrario não ha como verificar a multipla selecao de precos
                        itemPreco = new EstruturaPrecoReservaSite();
                        itemPreco.ID = preco.ID;
                        itemPreco.GerenciamentoIngressosID = preco.GerenciamentoIngressosID;
                        itemPreco.PrecoNome = preco.PrecoNome;
                        itemPreco.Quantidade = preco.Quantidade;
                        itemPreco.Valor = preco.Valor;
                        itemPreco.CotaItemID = retornoCotaItens[0];
                        itemPreco.CotaItemIDAPS = retornoCotaItens[1];
                        itemPreco.CodigoCinema = preco.CodigoCinema;
                        itemPreco.CodigoProgramacao = preco.CodigoProgramacao;
                        listaPreco.Add(itemPreco);
                    }
                }
                //Lugares marcados. Precisa achar os melhores lugares.
                if (tipoSetor != IRLib.Paralela.Setor.enumLugarMarcado.Pista)
                {
                    #region Busca Ingresso de Lugar Marcado
                    // Busca a lista dos Melhores Lugares
                    List<Lugar> lugares = oLugar.MelhorLugarMarcado(qtdPrecos, apresentacaoSetorID, tipoSetor);

                    //Verifica se os ingressos estão juntos
                    if (tipoSetor == IRLib.Paralela.Setor.enumLugarMarcado.MesaFechada)
                    {
                        if (lugares.Count == 0)
                            throw new Exception("Não foi possível efetuar todas as reserva.");

                        if (lugares.Count != qtdPrecos)
                            throw new Exception("Não existem mesas com a capacidade de acomodar todas as pessoas juntas");
                    }
                    int quantidadeMesaAberta = 0;
                    bool mesaAberta = (tipoSetor == IRLib.Paralela.Setor.enumLugarMarcado.MesaAberta);
                    // Se for mesa aberta o total da de ingressos é a soma de todas as quantidades na listagem.
                    if (mesaAberta)
                    {

                        foreach (Lugar itemLugar in lugares)
                        {
                            quantidadeMesaAberta += itemLugar.Quantidade.Valor;
                        }
                        if (quantidadeMesaAberta != qtdPrecos) // Não encontrou a qtd suficiente?!
                            lugares.Clear(); // Limpa os ingressos e passa para a próxima.
                    }
                    else
                    {

                        if (lugares.Count != qtdPrecos) // Não encontrou a qtd suficiente?!
                            lugares.Clear(); // Limpa os ingressos e passa para a próxima.
                    }

                    ingressosRetorno = new List<Ingresso>();
                    p = 0;//variavel de controle para os preços
                    int controlePrecoID = 0; //variavel de controle para saber se mudou o precoID e trazer a nova taxa de entrega
                    //Busca os ingressos para os melhores lugares encontrados
                    foreach (Lugar l in lugares)
                    {

                        try
                        {
                            //Só faz a select com base na quantidade caso seja mesa aberta.
                            string top = mesaAberta ? "TOP " + l.Quantidade.Valor : "";

                            string sql = "SELECT " + top + " ID, Codigo,EmpresaID FROM tIngresso(NOLOCK) " +
                                 "WHERE ApresentacaoSetorID = " + apresentacaoSetorID + " AND Status = '" + Ingresso.DISPONIVEL + "' " +
                                 "AND LugarID = " + l.Control.ID;

                            bd.Consulta(sql);


                            while (bd.Consulta().Read())
                            {
                                if (precoPorLugar[p].ID != controlePrecoID)
                                {
                                    valorConv = oBilheteria.TaxaConveniencia(eventoID, precoPorLugar[p].ID, estruturaReserva.CanalID);

                                    //Se não tem conveniencia, não deve contar que possui taxa de processamento, ignora a busca
                                    if (valorConv > 0)
                                        valorProcessamento = oBilheteria.ValorTaxaProcessamento(eventoID);

                                    controlePrecoID = precoPorLugar[p].ID;
                                }

                                //popula o objeto ingresso
                                oIngresso = new Ingresso();
                                oIngresso.Control.ID = bd.LerInt("ID");
                                oIngresso.PrecoID.Valor = precoPorLugar[p].ID;
                                oIngresso.UsuarioID.Valor = usuarioID;
                                oIngresso.Codigo.Valor = bd.LerString("Codigo");
                                oIngresso.LojaID.Valor = lojaID;
                                oIngresso.ClienteID.Valor = clienteID;
                                oIngresso.SessionID.Valor = sessionID;
                                oIngresso.TimeStampReserva.Valor = timeStamp;
                                oIngresso.LugarID.Valor = l.Control.ID;
                                oIngresso.TxConv = valorProcessamento > 0 ? 0 : valorConv;
                                oIngresso.TaxaProcessamentoValor = valorProcessamento;
                                oIngresso.Grupo.Valor = l.Grupo.Valor;
                                oIngresso.Classificacao.Valor = l.Classificacao.Valor;
                                oIngresso.CotaItemID = precoPorLugar[p].CotaItemID;
                                oIngresso.CotaItemIDAPS = precoPorLugar[p].CotaItemIDAPS;
                                oIngresso.EmpresaID.Valor = bd.LerInt("EmpresaID");
                                oIngresso.SerieID.Valor = serieID;

                                ////se não tiver valor e não tiver conveniencia não deve reservar
                                if (precoPorLugar[p].Valor == 0 && oIngresso.TxConv == 0 && oIngresso.TaxaProcessamentoValor == 0)
                                {
                                    // Atribui a Cortesia Padrão do Evento/Local - INICIO
                                    if (CortesiaPadraoID == 0)
                                        CortesiaPadraoID = oCortesia.CortesiaPadraoEvento(eventoID);

                                    if (CortesiaPadraoID <= 0)
                                        throw new Exception("Falha ao reservar os ingressos. Não existe cortesia associada a este evento.");

                                    oIngresso.CortesiaID.Valor = CortesiaPadraoID;
                                    //    // Atribui a Cortesia Padrão do Evento/Local - FIM

                                    //    oIngresso.Status.Valor = Ingresso.CORTESIA_SEM_CONVENIENCIA;
                                    ingressosRetorno.Add(oIngresso);
                                    //    break;//break para inserir somente um registro. esse registro de ingresso vai ser utilizado 
                                    //    //como base de info para deletar o preço inválido do banco de dados do site.
                                }
                                else
                                    ingressosRetorno.Add(oIngresso);

                                if (tipoSetor != IRLib.Paralela.Setor.enumLugarMarcado.MesaFechada)
                                    p++;
                            }
                            if (tipoSetor == IRLib.Paralela.Setor.enumLugarMarcado.MesaFechada)
                                p++;
                        }
                        catch
                        {
                            throw;
                        }
                    }

                    #endregion
                }

                else //Ingressos de pista
                {
                    #region Busca Ingressos de Pista
                    Ingresso ing;
                    ingressosRetorno = new List<Ingresso>();
                    ArrayList ingressosIDInseridos = new ArrayList();//lista de ingressos ID para não buscar os mesmos ingressos.
                    string evitaDuplicidades = "";//monta a string para a sql
                    foreach (EstruturaPrecoReservaSite preco in listaPreco)
                    {
                        if (ingressosRetorno.Count > 0)
                        {
                            //Monta a query que evita duplicidade de ingressos na hora de reservar.
                            foreach (Ingresso item in ingressosRetorno)
                                ingressosIDInseridos.Add(item.Control.ID);

                            evitaDuplicidades = "AND ID NOT IN ( " + Utilitario.ArrayToString(ingressosIDInseridos) + ") ";
                        }
                        //Busca os ingressos de pista.
                        bd.FecharConsulta();

                        var bdBusca = new BD(ConfigurationManager.AppSettings["ConexaoReadOnly"]);

                        bdBusca.Consulta(string.Format("SELECT TOP {0} ID, Codigo, LugarID FROM tIngresso(NOLOCK) WHERE ApresentacaoSetorID = {1} AND Status = '{2}' {3} ORDER BY newid()", preco.Quantidade, apresentacaoSetorID, Ingresso.DISPONIVEL, evitaDuplicidades));

                        while (bdBusca.Consulta().Read())
                        {
                            valorConv = oBilheteria.TaxaConveniencia(eventoID, preco.ID, estruturaReserva.CanalID);
                            //Se não tem conveniencia, não deve contar que possui taxa de processamento, ignora a busca
                            if (valorConv > 0)
                                valorProcessamento = oBilheteria.ValorTaxaProcessamento(eventoID);
                            else
                                valorProcessamento = 0;

                            //Popula o objeto ingresso e adiciona a lista de retorno
                            ing = new Ingresso();
                            ing.Control.ID = bdBusca.LerInt("ID");
                            ing.PrecoID.Valor = preco.ID;
                            ing.GerenciamentoIngressosID.Valor = preco.GerenciamentoIngressosID;
                            ing.UsuarioID.Valor = estruturaReserva.UsuarioID;
                            ing.Codigo.Valor = bdBusca.LerString("Codigo");
                            ing.LojaID.Valor = estruturaReserva.LojaID;
                            ing.ClienteID.Valor = clienteID;
                            ing.SessionID.Valor = sessionID;
                            ing.TimeStampReserva.Valor = timeStamp;
                            ing.LugarID.Valor = bdBusca.LerInt("LugarID");
                            ing.TxConv = valorProcessamento > 0 ? 0 : valorConv;
                            ing.TaxaProcessamentoValor = valorProcessamento;
                            ing.CotaItemID = preco.CotaItemID;
                            ing.CotaItemIDAPS = preco.CotaItemIDAPS;
                            ing.SerieID.Valor = serieID;
                            ing.CompraGUID.Valor = estruturaReserva.GUID;
                            //se não tiver valor e não tiver conveniencia não deve reservar
                            if (preco.Valor == 0 && ing.TxConv == 0 && ing.TaxaProcessamentoValor == 0)
                            {
                                // Atribui a Cortesia Padrão do Evento/Local - INICIO
                                if (CortesiaPadraoID == 0)
                                    CortesiaPadraoID = oCortesia.CortesiaPadraoEvento(eventoID);

                                if (CortesiaPadraoID <= 0)
                                    throw new Exception("Não foi possível reservar o ingresso. Por favor, tente novamente mais tarde.");

                                ing.CortesiaID.Valor = CortesiaPadraoID;
                                // Atribui a Cortesia Padrão do Evento/Local - FIM

                                //ing.Status.Valor = Ingresso.CORTESIA_SEM_CONVENIENCIA;
                                ingressosRetorno.Add(ing);
                                //break;//break para inserir somente um registro. esse registro de ingresso vai ser utilizado 
                                ////como base de info para deletar o preço inválido do banco de dados do site.
                            }
                            else
                                ingressosRetorno.Add(ing);
                        }//fim while consulta banco

                        bdBusca.Fechar();

                    }//fim foreach precos

                    //Ingressos para cinema, deve utilizar on demand!
                    if (listaPreco.Any(c => !string.IsNullOrEmpty(c.CodigoCinema)))
                        ingressosRetorno = this.NovoNaoMarcadoOnDemand(estruturaReserva, apresentacaoSetorID, eventoID, apresentacaoID, setorID, ingressosRetorno, listaPreco);


                    #endregion
                }

                return ingressosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bdReserva.Fechar();
                bd.Fechar();
            }

        }

        /// <summary>
        /// Vai inserir os ingressos on demand, este método só é utilizado em caso de compras para cinema!!!
        /// Não gerará código sequencial nem código do lugar!
        /// </summary>
        /// <param name="estruturaReserva"></param>
        /// <param name="apresentacaoSetorID"></param>
        /// <param name="eventoID"></param>
        /// <param name="apresentacaoID"></param>
        /// <param name="setorID"></param>
        /// <param name="ingressosRetorno"></param>
        /// <param name="listaPreco"></param>
        /// <returns></returns>
        private List<Ingresso> NovoNaoMarcadoOnDemand(EstruturaReservaInternet estruturaReserva, int apresentacaoSetorID, int eventoID, int apresentacaoID, int setorID, List<Ingresso> ingressosRetorno, List<EstruturaPrecoReservaSite> listaPreco)
        {
            BD bd = new BD();
            try
            {

                List<int> ids = this.NovoNaoMarcadoOnDemand(bd, eventoID, apresentacaoID, apresentacaoSetorID, setorID, listaPreco.Count - ingressosRetorno.Count);
                int atual = ingressosRetorno.Count;

                for (int i = atual; i < listaPreco.Count; i++)
                {
                    var ingresso = new Ingresso();
                    ingresso.Control.ID = ids[0];
                    ingresso.PrecoID.Valor = listaPreco[atual].ID;
                    ingresso.UsuarioID.Valor = estruturaReserva.UsuarioID;
                    ingresso.LojaID.Valor = estruturaReserva.LojaID;
                    ingresso.ClienteID.Valor = estruturaReserva.ClienteID;
                    ingresso.SessionID.Valor = estruturaReserva.SessionID;
                    ingresso.TimeStampReserva.Valor = DateTime.Now;
                    ingresso.TxConv = new BilheteriaParalela().TaxaConveniencia(eventoID, listaPreco[atual].ID, estruturaReserva.CanalID); ;
                    ingresso.CotaItemID = listaPreco[atual].CotaItemID;
                    ingresso.CotaItemIDAPS = listaPreco[atual].CotaItemIDAPS;
                    ingresso.SerieID.Valor = 0;
                    ingresso.CompraGUID.Valor = estruturaReserva.GUID;
                    ingressosRetorno.Add(ingresso);
                }

                return ingressosRetorno;
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

        private List<int> NovoNaoMarcadoOnDemand(BD bd, int eventoID, int apresentacaoID, int apresentacaoSetorID, int setorID, int quantidade)
        {

            bd.Consulta(@"SELECT 
                                em.ID AS EmpresaID, l.ID AS LocalID 
                        FROM tEmpresa em (NOLOCK) 
                        INNER JOIN tLocal l (NOLOCK) ON l.EmpresaID = em.ID 
                        INNER JOIN tEvento e (NOLOCK) ON e.LocalID = l.ID 
                        WHERE 
                            l.CodigoPraca IS NOT NULL AND LEN(l.CodigoPraca) > 0 AND 
                            e.FilmeID IS NOT NULL AND e.FilmeID > 0 AND 
                            e.ID = " + eventoID);
            int empresaID = 0;
            int localID = 0;

            if (!bd.Consulta().Read())
                throw new Exception("Não foi possível identificar as informações deste ingresso para efetuar a inclusão no carrinho.");

            empresaID = bd.LerInt("EmpresaID");
            localID = bd.LerInt("LocalID");

            bd.FecharConsulta();

            Random rnd = new Random();
            List<int> ingressos = new List<int>();
            for (int i = 0; i < quantidade; i++)
            {
                this.NovoNaoMarcado(apresentacaoSetorID, eventoID, apresentacaoID, setorID, empresaID, localID, 0, rnd.Next(0, 99999), bd, 0, string.Empty);
                ingressos.Add(this.Control.ID);
            }


            return ingressos;
        }

        public static string StringRemoverReservaAssinatura(int ingressoID)
        {
            return
                "UPDATE tIngresso SET BloqueioID = 0, PrecoID=0, UsuarioID=0, Status='D', LojaID = 0, ClienteID = 0, TimeStampReserva='', SessionID='', AssinaturaClienteID = 0, SerieID = 0 WHERE ID = " + ingressoID + " AND (Status='" + DISPONIVEL + "' OR Status='" + BLOQUEADO + "' OR Status='" + RESERVADO + "')";
        }

        public bool Bloquear(BD bd, EstruturaAssinaturaIngresso ingresso, int usuarioid, int empresaid)
        {

            try
            {

                this.Control.ID = ingresso.IngressoID;
                this.UsuarioID.Valor = usuarioid;
                this.BloqueioID.Valor = ingresso.BloqueioID;
                this.CortesiaID.Valor = 0;
                this.PrecoID.Valor = 0;

                string sql = "UPDATE tIngresso SET " +
                    "BloqueioID=" + ingresso.BloqueioID + ", " +
                    "Status='" + BLOQUEADO + "' " +
                    "WHERE (Status='" + DISPONIVEL + "' OR Status='" + BLOQUEADO + "') AND ID=" + this.Control.ID;

                int x = bd.Executar(sql);
                bd.Fechar();

                bool ok = Convert.ToBoolean(x);

                if (ok)
                {
                    //inserir na Log
                    this.BloqueioID.Valor = ingresso.BloqueioID;
                    this.Status.Valor = BLOQUEADO;
                    ingressoLog.Ingresso(this);
                    ingressoLog.UsuarioID.Valor = usuarioid;
                    ingressoLog.EmpresaID.Valor = empresaid;
                    ingressoLog.Acao.Valor = IngressoLog.BLOQUEAR;
                    ingressoLog.Inserir(bd);
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
