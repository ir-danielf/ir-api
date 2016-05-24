using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Cinema
{
    public class SincronizarCinemas
    {
        private static Autenticacao oautenticacao { get; set; }
        private static Autenticacao oAutenticacao
        {
            get
            {
                if (oautenticacao == null)
                    oautenticacao = MontarAuth();

                return oautenticacao;
            }
        }
        public static Autenticacao MontarAuth()
        {
            return new Autenticacao()
            {
                Usuario = "INTERNET",
                Senha = "INTERNET123",
            };
        }

        /// <summary>
        /// Dá inicio a sincronização completa através da sincronização com o WS da Consciencia
        /// </summary>
        public void SincronizarTudo()
        {
            #region Praça
            var praca = Service.getOnline(oAutenticacao);
            int localID = this.MontarPraca(praca);
            #endregion

            #region Filmes
            var filmes = Service.getFilmes(oAutenticacao, new Filtros.GetFilmes()
            {
                DataIni = DateTime.Now.AddDays(-30),
                DataFim = DateTime.Now.AddDays(14),
            });
            filmes = this.MontarFilmes(filmes, localID);
            #endregion

            #region Salas
            var salas = Service.getSalas(oAutenticacao, new Filtros.GetSalas()
            {
                CodPraca = praca.Praca_Cod,
            });
            salas = this.MontarSalas(salas, localID);
            #endregion

            #region Programações
            foreach (var sala in salas)
            {
                var programacoes = Service.getProgramacao(oAutenticacao, new Filtros.GetProg()
                {
                    CodSala = sala.CodSala,
                    DataIni = DateTime.Now.AddDays(-7),
                    DataFim = DateTime.Now.AddDays(14),
                });

                if (programacoes == null)
                    continue;

                this.MontarProgramacao(sala, filmes, salas, programacoes);
            }
            #endregion
        }

        private int MontarPraca(Online praca)
        {
            BD bd = new BD();
            Local oLocal = new Local();
            try
            {
                int localID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tLocal WHERE CodigoPraca = '" + praca.Praca_Cod + "'"));

                if (localID > 0)
                    return localID;

                oLocal.Nome.Valor = praca.Praca_Cod;
                oLocal.EmpresaID.Valor = ConfiguracaoCinema.Instancia.Empresa.EmpresaID.Valor;
                oLocal.CodigoPraca.Valor = praca.Praca_Cod;
                oLocal.Inserir();

                return oLocal.Control.ID;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private List<Filme> MontarFilmes(List<Filme> filmes, int localID)
        {
            Evento oEvento = new Evento();
            IRLib.Filme oFilme = new IRLib.Filme();
            EventoEntregaControle oEntregaControle = new EventoEntregaControle();
            FormaPagamentoEvento oFormaPagamentoEvento = new FormaPagamentoEvento();
            EventoSubtipo oEventoSubTipo = new EventoSubtipo();

            BD bd = new BD();

            List<int> lstEntregaControle = this.Entregas();
            List<int> listaFormaPagamento = new FormaPagamento().GetFormasPagamentoPadrao();

            foreach (var filme in filmes)
            {
                try
                {
                    int eventoID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tEvento WHERE FilmeID = " + filme.FilmeID + " AND LocalID = " + localID));

                    if (eventoID > 0)
                    {
                        filme.EventoID = eventoID;
                        continue;
                    }

                    bd.IniciarTransacao();
                    oEvento.Limpar();
                    oFilme.Limpar();
                    oEventoSubTipo.Limpar();

                    int filmeID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tFilme WHERE FilmeID = " + filme.FilmeID));

                    //Se ainda não tem o filme, insere
                    if (filmeID == 0)
                    {
                        oFilme.Nome.Valor = filme.Titulo;
                        oFilme.Sinopse.Valor = filme.Sinopse;
                        oFilme.FilmeID.Valor = filme.FilmeID;
                        oFilme.Dublado.Valor = !string.IsNullOrEmpty(filme.Dublado);
                        oFilme.Duracao.Valor = Convert.ToInt32(string.IsNullOrEmpty(filme.Duracao) ? "0" : filme.Duracao);
                        oFilme.Idade.Valor = Convert.ToInt32(string.IsNullOrEmpty(filme.Idade) ? "0" : filme.Idade);
                        oFilme.IdadeJustificativa.Valor = filme.IdadeJusti;
                        oFilme.IMDB.Valor = filme.IMDB;
                        oFilme.Inserir(bd);

                    }

                    //Evento
                    oEvento.Nome.Valor = filme.Titulo;
                    oEvento.LocalID.Valor = localID;
                    oEvento.TipoCodigoBarra.Valor = ((char)Enumerators.TipoCodigoBarra.Estruturado).ToString();
                    oEvento.DuracaoEvento.Valor = filme.Duracao;
                    oEvento.FilmeID.Valor = filme.FilmeID;
                    oEvento.EventoSubTipoID.Valor = oEventoSubTipo.MontarPorCategoriaEstilo(ConfiguracaoCinema.Instancia.Categoria.Valor, filme.Estilo);
                    oEvento.Inserir(bd);

                    //Distribui as taxas de entrega
                    foreach (var entregaControleID in lstEntregaControle)
                    {
                        oEntregaControle.Limpar();
                        oEntregaControle.EventoID.Valor = oEvento.Control.ID;
                        oEntregaControle.EntregaControleID.Valor = entregaControleID;
                        oEntregaControle.Inserir(bd);
                    }

                    //Distribui as formas de pagamento
                    foreach (int FormaPagamentoID in listaFormaPagamento)
                    {
                        oFormaPagamentoEvento.EventoID.Valor = oEvento.Control.ID;
                        oFormaPagamentoEvento.FormaPagamentoID.Valor = FormaPagamentoID;
                        oFormaPagamentoEvento.Inserir(bd);
                    }

                    //Distribuir para o canal Internet
                    oEvento.DistribuirCanais(bd, Canal.CANAL_INTERNET.ToString(), true);

                    filme.EventoID = oEvento.Control.ID;

                    bd.FinalizarTransacao();
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

            return filmes;
        }

        private List<Sala> MontarSalas(List<Sala> salas, int localID)
        {
            Setor oSetor = new Setor();
            Lugar oLugar = new Lugar();
            BD bd = new BD();

            foreach (var sala in salas)
            {
                try
                {
                    bd.IniciarTransacao();

                    int setorID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tSetor s (NOLOCK) WHERE CodigoSala = '" + sala.CodSala + "' AND LocalID = " + localID));
                    if (setorID > 0)
                    {
                        sala.SetorID = setorID;
                        continue;
                    }


                    oSetor.Limpar();
                    oSetor.Nome.Valor = sala.Descrresu;
                    oSetor.NomeInterno.Valor = sala.CodSala;
                    oSetor.CodigoSala.Valor = sala.CodSala;
                    oSetor.LocalID.Valor = localID;
                    oSetor.Colunas.Valor = Convert.ToInt32(sala.Coluna);
                    oSetor.Linhas.Valor = Convert.ToInt32(sala.Linha);
                    oSetor.LugarMarcado.Valor = sala.Lugares.Count == 0 ? Setor.Pista : Setor.Cadeira;
                    oSetor.Capacidade.Valor = Convert.ToInt32(sala.Capacidade);
                    oSetor.Inserir(bd);

                    sala.SetorID = oSetor.Control.ID;

                    //Campos constantes para manter o espaçamento correto entre os assentos (Caso existam)
                    int espacamento = 5;
                    int c = 150;
                    int clas = 1;

                    foreach (var lugar in sala.Lugares)
                    {
                        oLugar.Limpar();
                        oLugar.Codigo.Valor = lugar.CodLugar;
                        oLugar.Classificacao.Valor = clas;
                        oLugar.Grupo.Valor = clas++;
                        oLugar.PosicaoX.Valor = c + (lugar.Coluna * espacamento);
                        oLugar.PosicaoY.Valor = c + (lugar.Linha * espacamento);
                        oLugar.CodigoCinema.Valor = lugar.IDLugar;
                        oLugar.Inserir(bd);

                        lugar.LugarID = oLugar.Control.ID;
                    }

                    bd.FinalizarTransacao();
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

            return salas;
        }

        private List<Programacao> MontarProgramacao(Sala sala, List<Filme> filmes, List<Sala> salas, List<Programacao> programacoes)
        {
            BD bd = new BD();
            Apresentacao oApresentacao = new Apresentacao();
            ApresentacaoSetor oApresentacaoSetor = new ApresentacaoSetor();
            Preco oPreco = new Preco();
            CanalPreco oCanalPreco = new CanalPreco();

            foreach (var programacao in programacoes)
            {
                try
                {
                    var eventoID = filmes.Where(c => c.FilmeID == programacao.IDFilme).Select(c => c.EventoID).FirstOrDefault();
                    var setorID = salas.Where(c => c.CodSala == programacao.CodSala).Select(c => c.SetorID).FirstOrDefault();

                    if (eventoID == 0)
                        continue;

                    int apresentacaoID = Convert.ToInt32(bd.ConsultaValor("SELECT ID FROM tApresentacao (NOLOCK) WHERE CodigoProgramacao = '" + programacao.IDProg + "'"));

                    if (apresentacaoID > 0)
                    {
                        programacao.ApresentacaoID = apresentacaoID;
                        continue;
                    }

                    bd.IniciarTransacao();

                    oApresentacao.Limpar();
                    oApresentacao.EventoID.Valor = eventoID;
                    oApresentacao.Horario.Valor = Convert.ToDateTime(programacao.DataSessao);
                    oApresentacao.DisponivelVenda.Valor = true;
                    oApresentacao.DisponivelAjuste.Valor = true;
                    oApresentacao.DisponivelRelatorio.Valor = true;
                    oApresentacao.CodigoProgramacao.Valor = programacao.IDProg;
                    oApresentacao.Inserir(bd);

                    oApresentacaoSetor.Limpar();
                    oApresentacaoSetor.ApresentacaoID.Valor = oApresentacao.Control.ID;
                    oApresentacaoSetor.SetorID.Valor = setorID;
                    oApresentacaoSetor.NVendeLugar.Valor = Convert.ToBoolean(programacao.NVendLuga);
                    oApresentacaoSetor.Inserir(bd);

                    foreach (var preco in programacao.Ingressos)
                    {
                        int precoID = Convert.ToInt32(bd.ConsultaValor("SELECT ID FROM tPreco (NOLOCK) WHERE CodigoCinema = '" + preco.CodIngress + "' AND ApresentacaoSetorID = " + oApresentacaoSetor.Control.ID));
                        if (precoID > 0)
                            continue;

                        oPreco.Limpar();
                        oPreco.ApresentacaoSetorID.Valor = oApresentacaoSetor.Control.ID;
                        oPreco.Nome.Valor = preco.CodIngress;
                        oPreco.Valor.Valor = Convert.ToDecimal(preco.Valor);
                        oPreco.CodigoCinema.Valor = preco.CodIngress;
                        oPreco.CorID.Valor = 1;
                        oPreco.Inserir(eventoID, setorID, oApresentacao.Control.ID, false, bd);

                        preco.PrecoID = oPreco.Control.ID;

                        //Distribuição de Preços por Canal
                        oCanalPreco.Limpar();
                        oCanalPreco.CanalID.Valor = Canal.CANAL_INTERNET;
                        oCanalPreco.PrecoID.Valor = oPreco.Control.ID;
                        oCanalPreco.Inserir(bd, false);
                    }

                    bd.FinalizarTransacao();
                }
                catch
                {
                    bd.DesfazerTransacao();
                }
                finally
                {
                    bd.Fechar();
                }

            }
            return programacoes;
        }

        private List<int> Entregas()
        {
            List<int> lista = new List<int>();
            BD bd = new BD();
            try
            {
                if (ConfiguracaoCinema.Instancia.Entrega.Bilheteria.Valor > 0 || ConfiguracaoCinema.Instancia.Entrega.Impressao.Valor > 0)
                {
                    bd.Consulta(string.Format(@"
                                SELECT ec.ID FROM tEntrega e (NOLOCK)
	                            INNER JOIN tEntregaControle ec (NOLOCK) ON ec.EntregaID = e.ID
	                            WHERE e.ID IN ({0}, {1})",
                                                             ConfiguracaoCinema.Instancia.Entrega.Bilheteria.Valor, ConfiguracaoCinema.Instancia.Entrega.Impressao.Valor));
                    while (bd.Consulta().Read())
                        lista.Add(bd.LerInt("ID"));
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        #region _-_-_-_-_ Métodos de Atualização (Vem do Webservice) _-_-_-_-_
        public void AtualizarPraca(string codigoPraca)
        {

        }

        public void AtualizarFilme(int filmeID)
        {
            BD bd = new BD();
            try
            {
                IRLib.Filme oFilme = new IRLib.Filme();
                int id = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tFilme WHERE FilmeID = " + filmeID));

                var filme = Service.getFilmes(oAutenticacao, new Filtros.GetFilmes()
                {
                    IdFilme = filmeID.ToString()
                }).FirstOrDefault();

                if (filme == null || id == 0)
                    throw new Exception("O Filme informado não está cadastrado.");

                oFilme.Control.ID = id;
                oFilme.FilmeID.Valor = filmeID;
                oFilme.Nome.Valor = filme.Titulo;
                oFilme.Sinopse.Valor = filme.Sinopse;
                oFilme.Dublado.Valor = !string.IsNullOrEmpty(filme.Dublado);
                oFilme.Duracao.Valor = Convert.ToInt32(string.IsNullOrEmpty(filme.Duracao) ? "0" : filme.Duracao);
                oFilme.Idade.Valor = Convert.ToInt32(string.IsNullOrEmpty(filme.Idade) ? "0" : filme.Idade);
                oFilme.IdadeJustificativa.Valor = filme.IdadeJusti;
                oFilme.IMDB.Valor = filme.IMDB;
                oFilme.Atualizar();
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtualizarSala(string codigoSala)
        {
            var praca = Service.getOnline(oAutenticacao);
            int localID = this.MontarPraca(praca);

            var salas = Service.getSalas(oautenticacao, new Filtros.GetSalas()
            {
                CodSala = codigoSala,
            });

        }

        public void AtualizarProgramacao(string codigoProgramacao)
        {
            BD bd = new BD();
            try
            {
                var programacao = Service.getProgramacao(oAutenticacao, new Filtros.GetProg()
                {
                    IDProg = codigoProgramacao,
                }).FirstOrDefault();

                int apresentacaoID = Convert.ToInt32(bd.ConsultaValor("SELECT ID FROM tApresentacao (NOLOCK) WHERE CodigoProgramacao = '" + codigoProgramacao + "'"));
                if (apresentacaoID == 0)
                    throw new Exception("Não existe programação cadastrada com este código.");

                Apresentacao oApresentacao = new Apresentacao();
                oApresentacao.Ler(apresentacaoID);
                oApresentacao.Horario.Valor = Convert.ToDateTime(programacao.DataSessao);
                oApresentacao.Atualizar();


                int apresentacaoSetorID = Convert.ToInt32(bd.ConsultaValor("SELECT TOP 1 ID FROM tApresentacaoSetor WHERE ApresentacaoID = " + apresentacaoID));
                ApresentacaoSetor oApresentacaoSetor = new ApresentacaoSetor();
                oApresentacaoSetor.Ler(apresentacaoSetorID);
                oApresentacaoSetor.NVendeLugar.Valor = Convert.ToBoolean(programacao.NVendLuga);
                oApresentacaoSetor.Atualizar();



            }
            finally
            {
                bd.Fechar();
            }
        }
        #endregion

    }
}
