using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Cinema
{
    public class Erro
    {
        public string Erro_Cod { get; set; }
        public string Erro_Desc { get; set; }
    }

    public class Autenticacao
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }

    public class LugarSala
    {
        public string CodLugar { get; set; }
        public string CodSala { get; set; }
        public int Coluna { get; set; }
        public string IDLugar { get; set; }
        public int Linha { get; set; }
        public string Tipo { get; set; }

        public int LugarID { get; set; }
    }

    public class Online
    {
        public string Data_Hora { get; set; }
        public string Praca_Cod { get; set; }
    }

    public class Programacao
    {
        public string CodSala { get; set; }
        public string CodIsPre { get; set; }
        public string CPOC { get; set; }
        public DateTime DataSessao { get; set; }
        public string Feriado { get; set; }
        public int IDFilme { get; set; }
        public string IDFilme_Desc { get; set; }
        public string IDProg { get; set; }
        public List<ProgramacaoIngresso> Ingressos { get; set; }
        public string NLIBFidelidade { get; set; }
        public string NVendLuga { get; set; }
        public string Tempo { get; set; }
        public string Vende_Numerada { get; set; }

        public int ApresentacaoID { get; set; }
    }

    public class LugarProg
    {
        public string CodLugar { get; set; }
        public string Status { get; set; }
    }

    public class ProgramacaoIngresso
    {
        public string CodIngress { get; set; }
        public string ExibFid { get; set; }
        public string NexiBFid { get; set; }
        public string Ordem { get; set; }
        public string TaxaAux { get; set; }
        public string Valor { get; set; }

        public int PrecoID { get; set; }
    }

    public class RetVenda
    {
        public string IDMoviMe { get; set; }
    }

    public class Sala
    {
        public string Capacidade { get; set; }
        public string CodigoInt { get; set; }
        public string CodPraca { get; set; }
        public string CodPracaDesc { get; set; }
        public string CodSala { get; set; }
        public string Coluna { get; set; }
        public string Cor { get; set; }
        public string Descricao { get; set; }
        public string Descrresu { get; set; }
        public string Digital { get; set; }
        public string Linha { get; set; }
        public List<LugarSala> Lugares { get; set; }
        public string NieCode { get; set; }
        public string NVendLuga { get; set; }

        public int SetorID { get; set; }
    }

    public class VagasProg
    {
        public string IDProg { get; set; }
        public int Capacidade { get; set; }
        public int Disponivel { get; set; }
    }

    public class VendaItem
    {
        public string Acrescimo { get; set; }
        public string CodBarra { get; set; }
        public string CodIngress { get; set; }
        public string CodLugar { get; set; }
        public string Desconto { get; set; }
        public string IDProg { get; set; }
        public string IDProgramacao { get; set; }
        public string Infos { get; set; }
        public string Item { get; set; }
        public string NT_CodBar { get; set; }
        public string NT_Descr { get; set; }
        public string NT_Status { get; set; }
        public string NT_STNome { get; set; }
        public string Produto { get; set; }
        public string Promo_Cod { get; set; }
        public int Qtd { get; set; }
        public string TipoItem { get; set; }
        public string ValorRun { get; set; }
        public string IDProg_CodSala { get; set; }
        public int IDProg_IdFilme { get; set; }
        public DateTime IDProg_DataSessao { get; set; }
    }

    public class VendaPaga
    {
        public string Documento { get; set; }
        public string Infos { get; set; }
        public string Net_Validade { get; set; }
        public string Parcela { get; set; }
        public string ParcTOT { get; set; }
        public string TipoDoc { get; set; }
        public string Valor { get; set; }
        public string Vencimento { get; set; }
    }

    public class Filme
    {
        public string Titulo { get; set; }
        public string TituloResu { get; set; }
        public string DataCad { get; set; }
        public string Duracao { get; set; }
        public string Estilo { get; set; }
        public string Estilo_Desc { get; set; }
        public string Idade { get; set; }
        public string IdadeJusti { get; set; }
        public string Sinopse { get; set; }
        public string Dublado { get; set; }
        public string NIERELCODE { get; set; }
        public string IMDB { get; set; }
        public int FilmeID { get; set; }
        public int EventoID { get; set; }
    }


    public class Filtros
    {
        public class GetFilmes
        {
            public DateTime DataIni { get; set; }
            public DateTime DataFim { get; set; }
            public string IdFilme { get; set; }
        }
        public class GetLugarProg
        {
            public string CodLugar { get; set; }
            public string IDCompra { get; set; }
            public string IDProg { get; set; }
            public string Status { get; set; }
        }
        public class GetProg
        {
            public string CodSala { get; set; }
            public DateTime DataIni { get; set; }
            public DateTime DataFim { get; set; }
            public string IDProg { get; set; }
        }
        public class GetSalas
        {
            public string CodPraca { get; set; }
            public string CodSala { get; set; }
        }
        public class GetVagasProg
        {
            public string IDProg { get; set; }
        }
        public class SetVenda
        {
            public string CodForma { get; set; }
            public string Descricao { get; set; }
            public string Infos { get; set; }
            public List<VendaItem> Itens { get; set; }
            public string Momento { get; set; }
            public string Mov_Acrescimo { get; set; }
            public string Mov_Desconto { get; set; }
            public string Mov_Valor { get; set; }
            public string NT_AssCPF { get; set; }
            public string NT_AssDOC2 { get; set; }
            public string NT_AssID { get; set; }
            public string NT_AssNome { get; set; }
            public string NT_Autoriz { get; set; }
            public string NT_CodBar { get; set; }
            public string NT_ID { get; set; }
            public string NT_Status { get; set; }
            public string NT_STNome { get; set; }
            public string NT_Transac { get; set; }
            public VendaPaga[] Pagamentos { get; set; }
            public string Simulador { get; set; }
            public string TipoMovim { get; set; }

            public string IdMovime { get; set; }
        }
        public class SetReservaLugar
        {
            public string Acao { get; set; }
            public string CodLugar { get; set; }
            public string IDProg { get; set; }
            public string IDCompra { get; set; }
        }


        public class SetCancela
        {
            public string IDMovime { get; set; }
            public string NT_CodBar { get; set; }
            public string NT_ID { get; set; }
            public string Mov_Valor { get; set; }
            public string Motivo { get; set; }
        }
    }


    public class Retornos
    {
        public class GetFilmes
        {
            public List<Filme> Filmes { get; set; }
        }

        public class GetLugarProg
        {
            public string CodLugar { get; set; }
            public string IDCompra { get; set; }
            public string IDProg { get; set; }
            public string Status { get; set; }
        }

        public class GetOnline
        {
            public Erro[] Erros { get; set; }
            public Online Online { get; set; }
        }

        public class GetProgramacao
        {
            public Erro[] Erros { get; set; }
            public List<Programacao> Programacao { get; set; }
        }

        public class SetRealizaVenda
        {
            public Erro[] Erros { get; set; }
            public RetVenda RetVenda { get; set; }
        }

        public class SetCancelaVenda
        {
            public Erro[] Erros { get; set; }
        }


        public class SetReservaLugares
        {
            public Erro[] Erros { get; set; }
        }

        public class GetSalas
        {
            public Erro[] Erros { get; set; }
            public List<Sala> Salas { get; set; }
        }

        public class GetVagasProgramacao
        {
            public Erro[] Erros { get; set; }
            public VagasProg[] VagasProgs { get; set; }
        }
    }

    public static class Service
    {
        private static wsCinema.WSCinema wsCinema = new wsCinema.WSCinema();

        private static wsCinema.CINE_AUTENTICA GerarAutenticacao(Autenticacao auth)
        {
            return new wsCinema.CINE_AUTENTICA()
            {
                USUARIO = auth.Usuario,
                SENHA = auth.Senha
            };
        }

        private static string ToDataFormatada(this DateTime data)
        {
            return data.ToString("yyyy-MM-dd");
        }

        public static Online getOnline(Autenticacao autenticacao)
        {
            var retorno = wsCinema.Get_Cinema_OnLine(GerarAutenticacao(autenticacao));

            if (retorno.ERROS != null && retorno.ERROS.Length > 0)
                throw new Exception("Erro ao tentar buscar as praças: " + retorno.ERROS[0]);

            return new Online()
                {
                    Data_Hora = retorno.ONLINE.DATA_HORA,
                    Praca_Cod = retorno.ONLINE.PRACA_COD,
                };
        }

        public static List<Filme> getFilmes(Autenticacao autenticacao, Filtros.GetFilmes getFilmes)
        {
            var retorno = wsCinema.Get_Cinema_Filmes(GerarAutenticacao(autenticacao),
                new wsCinema.CINE_GETFILMES()
            {
                DATAINI = getFilmes.DataIni.ToDataFormatada(),
                DATAFIN = getFilmes.DataFim.ToDataFormatada(),
                IDFILME = getFilmes.IdFilme,
            });

            if (retorno.ERROS != null && retorno.ERROS.Length > 0)
                throw new Exception("Erro ao tentar buscar as praças: " + retorno.ERROS[0].ERRO_DESC);

            if (retorno.FILMES == null || retorno.FILMES.Length == 0)
                throw new Exception("Não existe nenhum filme a ser pesquisado neste intervalo!");

            return (from f in retorno.FILMES
                    select new Filme()
                    {
                        FilmeID = Convert.ToInt32(f.IDFILME),
                        DataCad = f.DATACAD,
                        Dublado = f.DUBLADO,
                        Duracao = f.DURACAO,
                        Estilo = f.ESTILO,
                        Estilo_Desc = f.ESTILO_DESC,
                        Idade = f.IDADE,
                        IdadeJusti = f.IDADEJUSTI,
                        IMDB = f.IMDB,
                        NIERELCODE = f.NIERELCODE,
                        Sinopse = f.SINOPSE,
                        Titulo = f.TITULO,
                        TituloResu = f.TITULORESU
                    }).ToList();
        }

        public static List<Sala> getSalas(Autenticacao autenticacao, Filtros.GetSalas getSalas)
        {
            var retorno = wsCinema.Get_Cinema_Salas(Service.GerarAutenticacao(autenticacao),
                new wsCinema.CINE_GETSALAS()
            {
                CODPRACA = getSalas.CodPraca,
                CODSALA = getSalas.CodSala
            });

            if (retorno.ERROS != null && retorno.ERROS.Length > 0)
                throw new Exception("Erro ao tentar buscar as salas: " + retorno.ERROS[0].ERRO_DESC);

            if (retorno.SALAS == null || retorno.SALAS.Length == 0)
                throw new Exception("Não existe nenhuma sala cadastrada para a praça informada!");

            return (from s in retorno.SALAS
                    select new Sala()
                    {
                        Capacidade = s.CAPACIDADE,
                        CodigoInt = s.CODIGOINT,
                        CodPraca = s.CODPRACA,
                        CodPracaDesc = s.CODPRACA_DESC,
                        CodSala = s.CODSALA,
                        Coluna = s.COLUNA,
                        Cor = s.COR,
                        Descricao = s.DESCRICAO,
                        Descrresu = s.DESCRRESU,
                        Digital = s.DIGITAL,
                        Linha = s.LINHA,
                        Lugares = (from l in s.LUGARES
                                   select new LugarSala()
                                   {
                                       CodLugar = l.CODLUGAR,
                                       CodSala = l.CODSALA,
                                       Coluna = Convert.ToInt32(l.COLUNA),
                                       IDLugar = l.IDLUGAR,
                                       Linha = Convert.ToInt32(l.LINHA),
                                       Tipo = l.TIPO,
                                   }).ToList(),
                        NieCode = s.NIECODE,
                        NVendLuga = s.NVENDLUGA
                    }).ToList();
        }

        public static List<Programacao> getProgramacao(Autenticacao autenticacao, Filtros.GetProg getProg)
        {
            var retorno = wsCinema.Get_Cinema_Programacao(Service.GerarAutenticacao(autenticacao),
                new wsCinema.CINE_GETPROG()
            {
                CODSALA = getProg.CodSala,
                DATAINI = getProg.DataIni.ToDataFormatada(),
                DATAFIN = getProg.DataFim.ToDataFormatada(),
                IDPROG = getProg.IDProg
            });

            if (retorno.ERROS != null && retorno.ERROS.Length > 0)
                throw new Exception("Erro ao tentar buscar a programação: " + retorno.ERROS[0].ERRO_DESC);

            if (retorno.PROGS == null || retorno.PROGS.Length == 0)
                return null;

            return (from p in retorno.PROGS
                    select new Programacao()
                    {
                        CodIsPre = p.CODSISPRE,
                        CodSala = p.CODSALA,
                        CPOC = p.CPOC,
                        DataSessao = Convert.ToDateTime(p.DATASESSAO),
                        Feriado = p.FERIADO,
                        IDFilme = Convert.ToInt32(p.IDFILME),
                        IDFilme_Desc = p.IDFILME_DESC,
                        IDProg = p.IDPROG,
                        Ingressos = (from i in p.INGRESSOS
                                     select new ProgramacaoIngresso()
                                     {
                                         CodIngress = i.CODINGRESS,
                                         ExibFid = i.EXIBFID,
                                         NexiBFid = i.NEXIBFID,
                                         Ordem = i.ORDEM,
                                         TaxaAux = i.TAXAAUX,
                                         Valor = i.VALOR.Replace(".", ","),
                                     }).ToList(),
                        NLIBFidelidade = p.NLIBFIDELIDADE,
                        NVendLuga = p.NVENDLUGA,
                        Tempo = p.TEMPO,
                        Vende_Numerada = p.VENDE_NUMERADA,
                    }).ToList();

        }

        public static VagasProg getVagasProgramacao(Autenticacao autenticacao, Filtros.GetVagasProg getVagasProg)
        {
            var vagasProg = new wsCinema.CINE_GETVAGASPROG[1];

            vagasProg[0] = new wsCinema.CINE_GETVAGASPROG()
            {
                IDPROG = getVagasProg.IDProg,
            };

            var retorno = wsCinema.Get_Cinema_VagasProg(Service.GerarAutenticacao(autenticacao), vagasProg);

            if (retorno.ERROS != null && retorno.ERROS.Length > 0)
                throw new Exception("Não foi possível pesquisar o horário selecionado - Erro: " + retorno.ERROS[0]);

            return (from v in retorno.VAGASPROGS
                    select new VagasProg()
                    {
                        Capacidade = Convert.ToInt32(v.CAPACIDADE),
                        Disponivel = Convert.ToInt32(v.DISPONIVEL),
                        IDProg = v.IDPROG,
                    }).FirstOrDefault();
        }

        public static Retornos.GetLugarProg getLugarProg(Autenticacao autenticacao, Filtros.GetLugarProg getLugarProg)
        {
            var retorno = wsCinema.Get_Cinema_LugarProg(Service.GerarAutenticacao(autenticacao), new wsCinema.CINE_GETLUGARPROG()
            {
                IDPROG = getLugarProg.IDProg,


            });

            return new Retornos.GetLugarProg()
            {

            };
        }

        public static void setRealizaVenda(Autenticacao autenticacao, Filtros.SetVenda venda)
        {
            var itens = new List<wsCinema.CINE_VENDAITEM>();
            itens = (from i in venda.Itens
                     select new wsCinema.CINE_VENDAITEM()
                     {
                         ACRESCIMO = i.Acrescimo,
                         CODBARRA = i.CodBarra,
                         CODINGRESS = i.CodIngress,
                         CODLUGAR = i.CodLugar,
                         DESCONTO = i.Desconto,
                         DESCRICAO = i.Desconto,
                         IDPROG = i.IDProg,
                         IDPROG_CODSALA = i.IDProg_CodSala,
                         //IDPROG_DATASESSAO = i.IDProg_DataSessao,
                         IDPROG_IDFILME = i.IDProg_IdFilme.ToString(),
                         INFOS = i.Infos,
                         ITEM = i.Item,
                         NT_CODBAR = i.NT_CodBar,
                         NT_DESCR = i.NT_Descr,
                         NT_STATUS = i.NT_Status,
                         NT_STNOME = i.NT_STNome,
                         PRODUTO = i.Produto,
                         PROMO_COD = i.Promo_Cod,
                         QTD = i.Qtd.ToString(),
                         TIPOITEM = i.TipoItem,
                         VALORUN = i.ValorRun,
                     }).ToList();

            var retorno = wsCinema.Set_Cinema_RealizaVenda(Service.GerarAutenticacao(autenticacao), new wsCinema.CINE_VENDA()
            {
                CODFORMA = venda.CodForma,
                DESCRICAO = venda.Descricao,
                INFOS = venda.Infos,
                ITENS = itens.ToArray(),
                MOMENTO = venda.Momento,
                IDMOVIME = venda.IdMovime,
                MOV_ACRESCIMO = venda.Mov_Acrescimo,
                MOV_DESCONTO = venda.Mov_Desconto,
                MOV_VALOR = venda.Mov_Valor,
                NT_ASSCPF = venda.NT_AssCPF,
                NT_ASSDOC2 = venda.NT_AssDOC2,
                NT_ASSID = venda.NT_AssID,
                NT_ASSNOME = venda.NT_AssNome,
                NT_AUTORIZ = venda.NT_Autoriz,
                NT_CODBAR = venda.NT_CodBar,
                NT_ID = venda.NT_ID,
                NT_STATUS = venda.NT_Status,
                NT_STNOME = venda.NT_STNome,
                NT_TRANSAC = venda.NT_Transac,
                SIMULAR = venda.Simulador,
                TIPOMOVIM = venda.TipoMovim
            });

            if (retorno.ERROS.Length > 0)
                throw new Exception("Não foi possível efetuar a venda dos ingressos deste filme. - Erro: " + retorno.ERROS[0]);
        }

        public static void setCancelaVenda(Autenticacao autenticacao, Filtros.SetCancela cancela)
        {
            var retorno = wsCinema.Set_Cinema_CancelaVenda(Service.GerarAutenticacao(autenticacao), new wsCinema.CINE_CANCVENDA()
            {
                IDMOVIME = cancela.IDMovime,
                MOTIVO = cancela.Motivo,
                MOV_VALOR = cancela.Mov_Valor,
                NT_CODBAR = cancela.NT_CodBar,
                NT_ID = cancela.NT_ID,
            });


            if (retorno.ERROS.Length > 0)
                throw new Exception("Não foi possível efetuar o cancelamento dos ingressos deste filme. - Erro: " + retorno.ERROS[0]);
        }

        public static void setReservaLugares(Autenticacao autenticacao, Filtros.SetReservaLugar[] setReservaLugar)
        {
            var ingressos = (from res in setReservaLugar
                             select new wsCinema.CINE_SETRESERVALUGAR()
                             {
                                 IDPROG = res.IDProg,
                                 ACAO = res.Acao,
                                 CODLUGAR = res.CodLugar,
                                 IDCOMPRA = res.IDCompra
                             }).ToArray();

            var retorno = wsCinema.Set_Cinema_ReservaLugares(Service.GerarAutenticacao(autenticacao), ingressos);

            if (retorno.ERROS.Length > 0)
                throw new Exception("Não foi possível efetuar a reserva dos lugares informados, Erro: " + retorno.ERROS[0].ERRO_DESC);
        }

        public static List<Programacao> getProgramacoes(List<string> codigos)
        {
            List<Programacao> programacoes = new List<Programacao>();
            foreach (var programacao in codigos.Distinct())
            {
                var lugarProgramacao = IRLib.Cinema.Service.getProgramacao(IRLib.Cinema.SincronizarCinemas.MontarAuth(), new Cinema.Filtros.GetProg()
                {
                    IDProg = programacao,
                });

                programacoes.AddRange(lugarProgramacao);
            }

            return programacoes;
        }
    }
}
