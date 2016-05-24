using CTLib;
using IRLib.Codigo.NovoGerador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRLib
{

    [Serializable]
    public class NovoEventoFase1 
    {

        public GerarEventoEtapa1Fase1Model Etapa1 { get; set; }
        public GerarEventoEtapa1Fase2Model Etapa2 { get; set; }

        public GerarEventoEtapa1Fase3Model Etapa3 { get; set; }

        public int EventoIDComplementar { get; set; }
    }

    [Serializable]
    public class NovoEventoFase2
    {
        public NovoEventoFase2(GerarEventoEtapa2Fase1Model infoPrecos)
        {
            if (infoPrecos == null || infoPrecos.Precos == null || infoPrecos.Precos.Count() == 0)
                throw new ArgumentException("Info preços não pode ser nulo ou vazio.");

            this.Info = infoPrecos;
        }

        public GerarEventoEtapa2Fase1Model Info { get; set; }
    }

    [Serializable]
    public class StatusCriacao
    {
        public StatusCriacao()
        {
            this.Status = enumStatus.NaoDefinido;
            this.MsgErro = "";

        }

        public enum enumStatus
        {
            OK,
            Erro,
            NaoDefinido
        }

        public string MsgErro { get; set; }
        public enumStatus Status { get; set; }

        public void Erro(string msg)
        {
            this.Status = enumStatus.Erro;
            this.MsgErro = msg;
        }

        public void OK()
        {
            this.Status = enumStatus.OK;
        }

        public bool Concluido
        {
            get { return this.Status != enumStatus.NaoDefinido; }
        }


        public override string ToString()
        {
            return string.Format("Status={0} e Msg={1}", this.Status.ToString(), this.MsgErro);
        }

    }

    [Serializable]
    public class FluxoCriacaoEvento
    {
        public FluxoCriacaoEvento()
        {
            this.Evento = new StatusCriacao();
            this.Entregas = new StatusCriacao();
            this.Pagamentos = new StatusCriacao();
            this.Apresentacoes = new StatusCriacao();
            this.Setores = new StatusCriacao();
            this.CanaisProprios = new StatusCriacao();
            this.CanaisIR = new StatusCriacao();
            this.Ingressos = new StatusCriacao();
            this.Precos = new StatusCriacao();

        }
        public StatusCriacao Evento { get; set; }
        public StatusCriacao Entregas { get; set; }
        public StatusCriacao Pagamentos { get; set; }
        public StatusCriacao Apresentacoes { get; set; }
        public StatusCriacao Setores { get; set; }
        public StatusCriacao CanaisProprios { get; set; }
        public StatusCriacao CanaisIR { get; set; }
        public StatusCriacao Ingressos { get; set; }
        public StatusCriacao Precos { get; set; }

        public bool Fase1Criada
        {
            get
            {
                return
                    this.Evento.Concluido &&
                    this.Apresentacoes.Concluido &&
                    this.Setores.Concluido &&
                    this.CanaisIR.Concluido &&
                    this.CanaisProprios.Concluido &&
                    this.Entregas.Concluido &&
                    this.Pagamentos.Concluido &&
                    this.Ingressos.Concluido;

            }
        }

        public bool Fase2Criada
        {
            get
            {
                return
                    this.Precos.Concluido;
            }
        }

    }


    public class EventoNovo : MarshalByRefObject, ISponsoredObject
    {
        public EventoNovo(int usuarioID)
        {
            if (!Usuario.UsuarioValido(usuarioID))
                throw new ArgumentException("Usuário inválido.");

            this.UsuarioID = usuarioID;

            this.Resultado = new EventoResultado();
            this.Resultado.FluxoCriacao = new FluxoCriacaoEvento();
            this.Precos = new List<PrecoDistribuicao>();



        }

        private Queue<PrecoDistribuicao> filaPreco = new Queue<PrecoDistribuicao>();
        public List<PrecoDistribuicao> Precos { get; set; }


        private int empresaID = 0;

        public EventoResultado Resultado { get; private set; }

        private NovoEventoFase1 InfoFase1 { get; set; }
        private NovoEventoFase2 InfoFase2 { get; set; }

        public FluxoCriacaoEvento Fluxo
        {
            get
            {
                return this.Resultado.FluxoCriacao;
            }
        }

        public int UsuarioID { get; private set; }



        [Serializable]
        public class EventoResultado : MarshalByRefObject, ISponsoredObject
        {
            public Evento Evento { get; set; }
            public List<int> Apresentacoes { get; set; }

            public FluxoCriacaoEvento FluxoCriacao { get; set; }

            public List<GerarEventoEtapa1Fase3Model.SetoresNovo> Setores { get; set; }


        }

        private EventoResultado CriarEstruturaEvento()
        {
            var evento = new Evento(this.UsuarioID);

            if (InfoFase1.EventoIDComplementar > 0)
                evento.Ler(InfoFase1.EventoIDComplementar);

            evento.Nome.Valor = InfoFase1.Etapa1.Nome;
            evento.LocalID.Valor = InfoFase1.Etapa1.LocalID;
            evento.ImpressaoCodigoBarra.Valor = true;
            evento.DesabilitaAutomatico.Valor = true;

            evento.ExibeQuantidade.Valor = false;
            evento.EntregaGratuita.Valor = false;
            evento.RetiradaBilheteria.Valor = true;
            evento.HabilitarRetiradaTodosPDV.Valor = true;
            evento.TipoImpressao.Valor = Configuracao.GetString(Configuracao.Keys.EventoTipoImpressao, ConfigurationManager.AppSettings["ConfigVersion"]);
            evento.TipoCodigoBarra.Valor = Configuracao.GetString(Configuracao.Keys.EventoTipoCodigoBarra, ConfigurationManager.AppSettings["ConfigVersion"]);

            evento.LimiteMaximoIngressosEvento.Valor = 0;
            evento.VendaSemAlvara.Valor = Configuracao.GetBoolean(Configuracao.Keys.EventoVendaSemAlvara, ConfigurationManager.AppSettings["ConfigVersion"]);

            evento.MapaEsquematicoID.Valor = InfoFase1.Etapa3.MapaEsquematicoId;
            evento.Ativo.Valor = "T";

            List<int> apresentacoes = null;
            List<GerarEventoEtapa1Fase3Model.SetoresNovo> setores = null;

            this.Resultado.Evento = evento;

            using (BD bd = new BD())
            {
                bd.IniciarTransacao();


                if (InfoFase1.EventoIDComplementar > 0)
                {
                    evento.CompletarCadastro.Valor = 0;

                    if (!evento.Atualizar(bd))
                        throw new ApplicationException("Erro ao atualizar o evento!");
                }
                else
                {
                    if (!evento.Inserir(bd))
                        throw new ApplicationException("Erro ao criar o evento!");
                }

                apresentacoes = this.InsereApresentacoes(bd);
                setores = this.InsereSetores(bd, apresentacoes);

                #region Forma de Pagamento Padrão do Evento

                FormaPagamentoEvento forma = new FormaPagamentoEvento(this.UsuarioID);
                FormaPagamento formaPagamentoPadrao = new FormaPagamento(this.UsuarioID);
                List<int> listaFormaPagamento = formaPagamentoPadrao.GetFormasPagamentoPadrao();
                foreach (int FormaPagamentoID in listaFormaPagamento)
                {
                    forma.EventoID.Valor = this.Resultado.Evento.Control.ID;
                    forma.FormaPagamentoID.Valor = FormaPagamentoID;

                    if (!forma.Inserir(bd))
                         throw new FormaPagamentoEventoException("Problemas ao inserir a forma de pagamento padrão para o evento.");
                    
                }

                #endregion


                bd.FinalizarTransacao();

                this.Fluxo.Evento.OK();
                
                this.Resultado.Apresentacoes = apresentacoes;
                this.Resultado.Setores = setores;
                this.empresaID = Local.getEmpresaID(InfoFase1.Etapa1.LocalID, bd);
                evento.LocalID.Valor = InfoFase1.Etapa1.LocalID;
                

             

            }

        

            return this.Resultado;
        }

        private List<GerarEventoEtapa1Fase3Model.SetoresNovo> InsereSetores(BD bd, List<int> apresentacoes)
        {
            var apresentacaoSetor = new ApresentacaoSetor(this.UsuarioID);

            List<GerarEventoEtapa1Fase3Model.SetoresNovo> setores = new List<GerarEventoEtapa1Fase3Model.SetoresNovo>();

            try
            {
                var setoresInseridos = apresentacaoSetor.InsereSetores(apresentacoes.ToArray(), InfoFase1.Etapa3.Setores.Select(c => c.SetorID).ToArray(), bd);

                foreach (ApresentacaoSetor setor in setoresInseridos)
                {
                    setores.Add(new GerarEventoEtapa1Fase3Model.SetoresNovo()
                    {
                        ApresentacaoSetorID = setor.Control.ID,
                        ApresentacaoID = setor.ApresentacaoID.Valor,
                        SetorID = setor.SetorID.Valor,
                        Tipo = setor.TipoSetor,
                        Quantidade = InfoFase1.Etapa3.Setores.Where(c => c.SetorID == setor.SetorID.Valor).FirstOrDefault().Quantidade
                    });

                }

                this.Fluxo.Setores.OK();

            }
            catch (Exception ex)
            {
                this.Fluxo.Setores.Erro(ex.Message);
                throw ex;
            }
            return setores;
        }

        private List<int> InsereApresentacoes(BD bd)
        {
            var apresentacoesInseridas = new List<int>();


            var apresentacao = new Apresentacao(this.UsuarioID);

            try
            {
                foreach (var ap in InfoFase1.Etapa2.Apresentacoes)
                {

                    apresentacao.EventoID.Valor = this.Resultado.Evento.Control.ID;
                    apresentacao.Horario.Valor = ap;
                    apresentacao.DisponivelAjuste.Valor = true;
                    apresentacao.DisponivelRelatorio.Valor = true;
                    apresentacao.DisponivelVenda.Valor = false;
                    apresentacao.Impressao.Valor = "A";

                    if (!apresentacao.Inserir(bd))
                        throw new ApplicationException("Falha ao inserir apresentação: " + ap);

                    apresentacoesInseridas.Add(apresentacao.Control.ID);
                }

                Fluxo.Apresentacoes.OK();

                return apresentacoesInseridas;
            }
            catch (Exception ex)
            {
                Fluxo.Apresentacoes.Erro(ex.Message);
                throw ex;
            }
        }

        private void GerarIngressosSetor(BD bd, List<GerarEventoEtapa1Fase3Model.SetoresNovo> setores)
        {

            foreach (GerarEventoEtapa1Fase3Model.SetoresNovo setor in setores)
            {
                try
                {
                    var ingresso = new Ingresso();

                    if (setor.Tipo == Setor.Tipo.Pista)
                        ingresso.Acrescentar(setor.ApresentacaoSetorID, this.Resultado.Evento.Control.ID, setor.ApresentacaoID, setor.SetorID, this.empresaID, this.Resultado.Evento.LocalID.Valor, 0, setor.Quantidade, 1, bd, true, null);
                    else
                        setor.Quantidade = ingresso.GerarLugares(setor.SetorID, setor.ApresentacaoSetorID, this.Resultado.Evento.Control.ID, setor.ApresentacaoID, this.empresaID, this.Resultado.Evento.LocalID.Valor, null, Enumerators.TipoCodigoBarra.Estruturado, bd);

                    ApresentacaoSetor aps = new ApresentacaoSetor(UsuarioID);
                    aps.Control.ID = setor.ApresentacaoSetorID;
                    aps.AtualizarIngressosGerados(bd);
                }
                catch (IngressoException ex)
                {
                    setor.IngressosGerados = false;
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void GerarIngressosEvento()
        {
            using (BD bd = new BD())
            {
                var marcados = this.Resultado.Setores.Where(c => c.Tipo != Setor.Tipo.Pista).OrderBy(t => t.SetorID).ToList();
                var naoMarcados = this.Resultado.Setores.Where(c => c.Tipo == Setor.Tipo.Pista).OrderBy(t => t.SetorID).ToList();

                string erro = "";
                
                try
                {
                    GerarIngressosSetor(bd, marcados);
                }
                catch(Exception ex)
                {
                    erro = ex.Message;
                }

                try
                {
                    GerarIngressosSetor(bd, naoMarcados);
                }
                catch (Exception ex)
                {
                    erro += ((string.IsNullOrEmpty(erro))?"":"\n") + ex.Message;
                }

                if(string.IsNullOrEmpty(erro))
                {
                    this.Fluxo.Ingressos.OK();
                }
                else
                {
                    this.Fluxo.Ingressos.Erro(erro);
                }

                
                

                
            }
        }

        private void InsereEntregaPadrao()
        {

            using (BD bd = new BD())
            {
                try
                {
                    bd.IniciarTransacao();
                    EventoEntregaControle.InsereEntregaPadrao(bd, this.Resultado.Evento.Control.ID);

                    this.Resultado.Evento.MenorPeriodoEntrega(bd);

                    Fluxo.Entregas.OK();

                    bd.FinalizarTransacao();
                    bd.Fechar();
                }
                catch (Exception ex)
                {
                    bd.DesfazerTransacao();
                    Fluxo.Entregas.Erro(ex.Message);
                }
            }
        }

        private void InserePagamentos()
        {

            using (BD bd = new BD())
            {
                try
                {
                    var pagamento = new FormaPagamentoEvento();
                    pagamento.DistribuiFormasPagamento(bd);

                    Fluxo.Pagamentos.OK();
                    bd.Fechar();
                }
                catch (Exception ex)
                {
                    Fluxo.Pagamentos.Erro(ex.Message);
                }
            }
        }

        private void DistribuiEventoCanaisIR()
        {

            using (BD bd = new BD())
            {
                try
                {
                    if (this.InfoFase1.Etapa1.VendeCanaisIR)
                        this.Resultado.Evento.DistribuirEventoIR(bd);

                    Fluxo.CanaisIR.OK();
                }
                catch (Exception ex)
                {
                    Fluxo.CanaisIR.Erro(ex.Message);
                }
            }
        }
        private void DistribuiEventoCanaisProprios()
        {
            if (this.InfoFase1.Etapa1.CanaisProprios.Count == 0)
            {
                Fluxo.CanaisProprios.OK();
                return;
            }
                

            using (BD bd = new BD())
            {
                try
                {
                    this.Resultado.Evento.DistribuirParaCanais(bd, this.Resultado.Evento.Control.ID, this.InfoFase1.Etapa1.CanaisProprios);
                    Fluxo.CanaisProprios.OK();
                }
                catch (Exception ex)
                {
                    Fluxo.CanaisProprios.Erro(ex.Message);
                }
            }
        }

        private void DistribuicoesSecundarias(bool async)
        {
            if (async)
            {
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Run(() => InsereEntregaPadrao()));
                tasks.Add(Task.Run(() => InserePagamentos()));

                tasks.Add(Task.Run(() =>
                {
                    DistribuiEventoCanaisIR();
                    DistribuiEventoCanaisProprios();
                }));

                //Task.WaitAll(tasks.ToArray());
            }
            else
            {
                InsereEntregaPadrao();
                InserePagamentos();

                DistribuiEventoCanaisIR();
                DistribuiEventoCanaisProprios();
            }
        }


        public EventoResultado CriarEvento(NovoEventoFase1 info, bool async = true)
        {

            this.InfoFase1 = info;

            CriarEstruturaEvento();

            if (async)
                Task.Run(() => GerarIngressosEvento());
            else
                GerarIngressosEvento();


            DistribuicoesSecundarias(async);

            return this.Resultado;
        }

        public EventoResultado CriarPreco(List<ApresentacaoSetorPrecoModel> precos, int EventoID, int LocalID)
        {

            if(precos == null || precos.Count == 0 || EventoID == 0 || LocalID == 0)
            {
                this.Resultado.FluxoCriacao.Precos.Erro("Parâmetros inválidos");
                return this.Resultado;
            }

            Evento evento = new Evento();
            evento.Control.ID = EventoID;
            evento.LocalID.Valor = LocalID;

            this.Resultado.Evento = evento;

            

            Task.Run(() => GerenciaFilaDistribuicaoPreco(precos));
            
            using (BD bd = new BD())
            {
                this.empresaID = Local.getEmpresaID(evento.LocalID.Valor, bd);

                int corID = Configuracao.GetInt(Configuracao.Keys.PrecoCorID, ConfigurationManager.AppSettings["ConfigVersion"]);
                var precoBD = new Preco(this.UsuarioID);

                foreach (var precoCriar in precos)
                {
                    precoBD.ApresentacaoSetorID.Valor = precoCriar.ApresentacaoSetorID;
                    precoBD.Valor.Valor = precoCriar.Valor;
                    precoBD.Nome.Valor = precoCriar.PrecoNome;
                    precoBD.CorID.Valor = corID;
                    precoBD.Impressao.Valor = Utils.Enums.GetChar(precoCriar.Impressao).ToString();
                    precoBD.Quantidade.Valor = 0;
                    precoBD.QuantidadePorCliente.Valor = 0;
                    precoBD.IRVende = false;

                    try
                    {
                        precoBD.Inserir(this.Resultado.Evento.Control.ID, precoCriar.SetorID, precoCriar.ApresentacaoID, true, bd);

                        precoCriar.PrecoID = precoBD.Control.ID;
                        precoCriar.Status.OK();

                        var precoDistribuicao = new PrecoDistribuicao(precoBD.Control.ID, precoCriar.VendeIR, precoCriar.VendeCanaisProprios);
                        this.Precos.Add(precoDistribuicao);
                        this.filaPreco.Enqueue(precoDistribuicao);
                    }
                    catch (Exception ex)
                    {
                        precoCriar.Status.Erro(ex.Message);
                    }
                }
            }
            if (precos.All(t => t.Status.Status == StatusCriacao.enumStatus.Erro))
            {
                this.Resultado.FluxoCriacao.Precos.Erro(precos[0].Status.MsgErro);
            }
            return this.Resultado;
        }

        public EventoResultado CriarApresentacoes(NovoEventoFase1 info, int EventoID)
        {
            bool async = false;


            this.InfoFase1 = info;

            List<int> apresentacoes = null;
            List<GerarEventoEtapa1Fase3Model.SetoresNovo> setores = null;

            Evento evento = new Evento();
            evento.Control.ID = EventoID;
            evento.LocalID.Valor = info.Etapa1.LocalID;

            this.Resultado.Evento = evento;

            using (BD bd = new BD())
            {
                bd.IniciarTransacao();

                apresentacoes = this.InsereApresentacoes(bd);
                setores = this.InsereSetores(bd, apresentacoes);
                bd.FinalizarTransacao();

                this.Fluxo.Evento.OK();


                this.Resultado.Apresentacoes = apresentacoes;
                this.Resultado.Setores = setores;
                this.empresaID = Local.getEmpresaID(InfoFase1.Etapa1.LocalID, bd);
                evento.LocalID.Valor = InfoFase1.Etapa1.LocalID;
            }


            if (async)
                Task.Run(() => GerarIngressosEvento());
            else
                GerarIngressosEvento();

            return this.Resultado;
        }

        private void CriaPrecoSetor(BD bd, PrecoVendaModel precoInfo)
        {
            int corID = Configuracao.GetInt(Configuracao.Keys.PrecoCorID, ConfigurationManager.AppSettings["ConfigVersion"]);
            var precoBD = new Preco(this.UsuarioID);
            var apresentacaoSetorBD = new ApresentacaoSetor();

            foreach (var precoCriar in precoInfo.SetorValores)
            {
                foreach (var apresentacaoSetor in this.Resultado.Setores.Where(c => c.SetorID.Equals(precoCriar.SetorID)).ToList())
                {
                    precoBD.ApresentacaoSetorID.Valor = apresentacaoSetor.ApresentacaoSetorID;
                    precoBD.Valor.Valor = precoCriar.Valor.Value;
                    precoBD.Nome.Valor = precoInfo.Descricao;
                    precoBD.CorID.Valor = corID;
                    precoBD.Impressao.Valor = Utils.Enums.GetChar(precoInfo.Impressao).ToString();
                    precoBD.Quantidade.Valor = 0;
                    precoBD.QuantidadePorCliente.Valor = 0;
                    precoBD.IRVende = false;

                    try
                    {
                        precoBD.Inserir(this.Resultado.Evento.Control.ID, apresentacaoSetor.SetorID, apresentacaoSetor.ApresentacaoID, true, bd);

                        if (precoInfo.Principal)
                        {
                            apresentacaoSetorBD.Control.ID = apresentacaoSetor.ApresentacaoSetorID;
                            apresentacaoSetorBD.AtualizarPrecoPrincipal(bd, precoBD.Control.ID);
                        }
                            

                        precoCriar.PrecoID = precoBD.Control.ID;
                        precoCriar.Status.OK();

                        var precoDistribuicao = new PrecoDistribuicao(precoBD.Control.ID, precoInfo.VendeIR, precoInfo.VendeCanaisProprios);
                        this.Precos.Add(precoDistribuicao);
                        this.filaPreco.Enqueue(precoDistribuicao);

                    }
                    catch (Exception ex)
                    {
                        precoCriar.Status.Erro(ex.Message);
                    }
                }
            }
        }


        

        private void DistribuirPrecoCanalIR(BD bd, PrecoDistribuicao preco)
        {
            try
            {
                if (preco.IR)
                    Preco.DistribuirParaCanaisIR(bd, preco.PrecoID);

                preco.StatusDistribuicaoIR.OK();
            }
            catch (Exception ex)
            {
                preco.StatusDistribuicaoIR.Erro(ex.Message);
            }
        }

        private void DistribuirPrecoCanaisProprios(BD bd, PrecoDistribuicao preco)
        {
            try
            {
                if (preco.CanaisProprios)
                 Preco.DistribuirParaCanaisProprios(bd, preco.PrecoID, empresaID);

                preco.StatusDistribuicaoPropria.OK();
            }
            catch (Exception ex)
            {
                preco.StatusDistribuicaoPropria.Erro(ex.Message);
            }
        }

        private void GerenciaFilaDistribuicaoPreco(List<ApresentacaoSetorPrecoModel> precosVenda = null)
        {
            bool precoNaoCriado = true;

            using (BD bd = new BD())
            {
                List<PrecoDistribuicao> precos = new List<PrecoDistribuicao>();
                
                do
                {
                    if (filaPreco.Count == 0)
                        continue;
                    var preco = filaPreco.Dequeue();
                    DistribuirPrecoCanalIR(bd, preco);
                    DistribuirPrecoCanaisProprios(bd, preco);

                    if (precosVenda == null)
                    {
                        precoNaoCriado = this.InfoFase2.Info.Precos.Where(c => c.Status.Status == StatusCriacao.enumStatus.NaoDefinido).Count() > 0;
                    }
                    else
                    {
                        precoNaoCriado = precosVenda.Where(c => c.Status.Status == StatusCriacao.enumStatus.NaoDefinido).Count() > 0;
                    }
                    

                } while (precoNaoCriado || filaPreco.Count() > 0);
            }

            this.Fluxo.Precos.OK();

        }

        private void GerenciaCriacaoPrecos()
        {
            Task.Run(() => GerenciaFilaDistribuicaoPreco());

            using (BD bd = new BD())
            {
                foreach (var preco in this.InfoFase2.Info.Precos)
                {
                    CriaPrecoSetor(bd, preco);
                    preco.Status.OK();
                }
            }
        }

        public void NovosPrecos(NovoEventoFase2 info)
        {
            this.InfoFase2 = info;

            GerenciaCriacaoPrecos();
        }
    }

}
