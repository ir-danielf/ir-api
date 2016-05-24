using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IRLib
{
    public partial class BufferInternet
    {
        public enum enumTables
        {
            TaxaEntrega,
            EventoTaxaEntrega,
            FormaPagamento,
            FormaPagamentoEvento,
            Tipo,
            Local,
            EventoSubtipo,
            Tipos,
            Evento,
            Apresentacao,
            Setor,
            Preco,
            PrecoParceiroMidia,
            Pacote,
            NomenclaturaPacote,
            PacoteItem,
            Banner,
            EventoTipoDestaque,
            ValeIngressoTipo,
            PontoVenda,
            PontoVendaHorario,
            PontoVendaXFormaPgto,
            PontoVendaFormaPgto,
            BannersPadraoSite,
            Serie,
            SerieItem,
            FormaPagamentoCotaItem,
            FormaPagamentoSerie,
            CotaItem,
            VoceSabia,
            Faq,
            Filme,
        }

        public class Evento : aSync<Evento>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public int LocalID { get; set; }
            public string Release { get; set; }
            public int TipoID { get; set; }
            public int SubTipoID { get; set; }
            public string Imagem { get; set; }
            public bool Destaque { get; set; }
            public int Prioridade { get; set; }
            public int Parcelas { get; set; }
            public bool EntregaGratuita { get; set; }
            public bool RetiradaBilheteria { get; set; }
            public bool DisponivelAvulso { get; set; }
            public string Publicar { get; set; }
            public int PublicarSemVendaMotivo { get; set; }
            public string DataAberturaVenda { get; set; }
            public string PalavraChave { get; set; }
            public string LocalImagemNome { get; set; }
            public bool EscolherLugarMarcado { get; set; }
            public bool ExibeQuantidade { get; set; }
            public bool BannersPadraoSite { get; set; }
            public int MenorPeriodoEntrega { get; set; }
            public int FilmeID { get; set; }
            public string ImagemDestaque { get; set; }
            public bool PossuiTaxaProcessamento { get; set; }
            public int LimiteMaximoIngressosEvento { get; set; }
            public int LimiteMaximoIngressosEstado { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Evento SET Nome = @Nome, 
                                    LocalID = @LocalID, 
                                    Release = @Release, 
                                    TipoID = @TipoID, 
                                    SubtipoID = @SubtipoID, 
                                    Imagem = @Imagem,
                                    Destaque = @Destaque, 
                                    Prioridade = @Prioridade, 
                                    Parcelas = @Parcelas, 
                                    EntregaGratuita = @EntregaGratuita, 
                                    RetiradaBilheteria = @RetiradaBilheteria,
                                    DisponivelAvulso = @DisponivelAvulso,
                                    Publicar = @Publicar,
                                    PublicarSemVendaMotivo = @PublicarSemVendaMotivo,
                                    DataAberturaVenda = @DataAberturaVenda,
                                    PalavraChave = @PalavraChave,
                                    EscolherLugarMarcado = @EscolherLugarMarcado,
                                    ExibeQuantidade = @ExibeQuantidade,
                                    BannersPadraoSite = @BannersPadraoSite,
                                    MenorPeriodoEntrega = @MenorPeriodoEntrega,
                                    FilmeID = @FilmeID,
                                    ImagemDestaque = @ImagemDestaque,
                                    PossuiTaxaProcessamento = @PossuiTaxaProcessamento, 
                                    LimiteMaximoIngressosEvento = @LimiteMaximoIngressosEvento,
                                    LimiteMaximoIngressosEstado = @LimiteMaximoIngressosEstado
                    
                                    WHERE IR_EventoID = @ID";

                }

            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Evento 
                            (IR_EventoID, Nome, LocalID, Release, TipoID, SubtipoID, Imagem, Destaque,Prioridade, Parcelas,
                            EntregaGratuita, RetiradaBilheteria, DisponivelAvulso, Publicar, PublicarSemVendaMotivo, DataAberturaVenda,
                            PalavraChave, EscolherLugarMarcado, ExibeQuantidade , BannersPadraoSite, MenorPeriodoEntrega, FilmeID, ImagemDestaque, PossuiTaxaProcessamento, LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado )
                            VALUES
                            (@ID, @Nome, @LocalID, @Release, @TipoID, @SubtipoID, @Imagem, @Destaque, @Prioridade, @Parcelas,
                            @EntregaGratuita, @RetiradaBilheteria, @DisponivelAvulso, @Publicar, @PublicarSemVendaMotivo, @DataAberturaVenda,
                            @PalavraChave, @EscolherLugarMarcado, @ExibeQuantidade , @BannersPadraoSite, @MenorPeriodoEntrega, @FilmeID, @ImagemDestaque, @PossuiTaxaProcessamento, @LimiteMaximoIngressosEvento, @LimiteMaximoIngressosEstado)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@LocalID", SqlDbType.Int, this.LocalID);
                this.AssignParameter("@Release", SqlDbType.NText, this.Release.Replace(Environment.NewLine, "<br />"));
                this.AssignParameter("@TipoID", SqlDbType.Int, this.TipoID);
                this.AssignParameter("@SubtipoID", SqlDbType.Int, this.SubTipoID);
                this.AssignParameter("@Imagem", SqlDbType.NVarChar, this.Imagem);
                this.AssignParameter("@Destaque", SqlDbType.Bit, this.Destaque);
                this.AssignParameter("@Prioridade", SqlDbType.Int, this.Prioridade);
                this.AssignParameter("@Parcelas", SqlDbType.Int, this.Parcelas);
                this.AssignParameter("@EntregaGratuita", SqlDbType.Bit, this.EntregaGratuita);
                this.AssignParameter("@RetiradaBilheteria", SqlDbType.Bit, this.RetiradaBilheteria);
                this.AssignParameter("@DisponivelAvulso", SqlDbType.Bit, this.DisponivelAvulso);
                this.AssignParameter("@Publicar", SqlDbType.NChar, this.Publicar);
                this.AssignParameter("@PublicarSemVendaMotivo", SqlDbType.Int, this.PublicarSemVendaMotivo);
                this.AssignParameter("@DataAberturaVenda", SqlDbType.NVarChar, this.DataAberturaVenda);
                this.AssignParameter("@PalavraChave", SqlDbType.NVarChar, this.PalavraChave);
                this.AssignParameter("@EscolherLugarMarcado", SqlDbType.Bit, this.EscolherLugarMarcado);
                this.AssignParameter("@ExibeQuantidade", SqlDbType.Bit, this.ExibeQuantidade);
                this.AssignParameter("@BannersPadraoSite", SqlDbType.Bit, this.BannersPadraoSite);
                this.AssignParameter("@MenorPeriodoEntrega", SqlDbType.Int, this.MenorPeriodoEntrega);
                this.AssignParameter("@FilmeID", SqlDbType.Int, this.FilmeID);
                this.AssignParameter("@ImagemDestaque", SqlDbType.NVarChar, this.ImagemDestaque ?? string.Empty);
                this.AssignParameter("@PossuiTaxaProcessamento", SqlDbType.Bit, this.PossuiTaxaProcessamento);
                this.AssignParameter("@LimiteMaximoIngressosEvento", SqlDbType.Int, this.LimiteMaximoIngressosEvento);
                this.AssignParameter("@LimiteMaximoIngressosEstado", SqlDbType.Int, LimiteMaximoIngressosEstado);
                return this.Parameters;
            }

            public override bool CompareIt(Evento item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    this.LocalID != item.LocalID ||
                    string.Compare(this.Release, item.Release) != 0 ||
                    this.TipoID != item.TipoID ||
                    this.SubTipoID != item.SubTipoID ||
                    string.Compare(this.Imagem, item.Imagem) != 0 ||
                    this.Destaque != item.Destaque ||
                    this.Prioridade != item.Prioridade ||
                    this.Parcelas != item.Parcelas ||
                    this.EntregaGratuita != item.EntregaGratuita ||
                    this.RetiradaBilheteria != item.RetiradaBilheteria ||
                    this.DisponivelAvulso != item.DisponivelAvulso ||
                    this.Publicar != item.Publicar ||
                    this.PublicarSemVendaMotivo != item.PublicarSemVendaMotivo ||
                    string.Compare(this.DataAberturaVenda, item.DataAberturaVenda) != 0 ||
                    this.PalavraChave != item.PalavraChave ||
                    this.EscolherLugarMarcado != item.EscolherLugarMarcado ||
                    this.ExibeQuantidade != item.ExibeQuantidade ||
                    this.BannersPadraoSite != item.BannersPadraoSite ||
                    this.MenorPeriodoEntrega != item.MenorPeriodoEntrega ||
                    string.Compare(this.ImagemDestaque, item.ImagemDestaque) != 0 ||
                    this.FilmeID != item.FilmeID ||
                    this.PossuiTaxaProcessamento != item.PossuiTaxaProcessamento ||
                    this.LimiteMaximoIngressosEvento != item.LimiteMaximoIngressosEvento ||
                    this.LimiteMaximoIngressosEstado != item.LimiteMaximoIngressosEstado
                    )
                    return false;
                else
                    return true;
            }


        }

        public class Filme : aSync<Filme>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public string Sinopse { get; set; }
            public int Duracao { get; set; }
            public int Idade { get; set; }
            public string IdadeJusti { get; set; }
            public bool Dublado { get; set; }
            public string IMDB { get; set; }
            public int FilmeID { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Filme SET Nome = @Nome, Sinopse = @Sinopse, Duracao = @Duracao, Idade = @Idade, IdadeJustificativa = @IdadeJustificativa, Dublado = @Dublado, IMDB = @IMDB,
                            FilmeID = @FilmeID
                            WHERE IR_FilmeID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Filme
                            (IR_FilmeID, FilmeID, Nome, Sinopse, Duracao, Idade, IdadeJustificativa, Dublado, IMDB)
                            VALUES 
                            (@ID, @FilmeID, @Nome, @Sinopse, @Duracao, @Idade, @IdadeJustificativa, @Dublado, @IMDB)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Sinopse", SqlDbType.NVarChar, this.Sinopse);
                this.AssignParameter("@Duracao", SqlDbType.Int, this.Duracao);
                this.AssignParameter("@Idade", SqlDbType.Int, this.Idade);
                this.AssignParameter("@IdadeJustificativa", SqlDbType.NVarChar, this.IdadeJusti);
                this.AssignParameter("@Dublado", SqlDbType.Bit, this.Dublado);
                this.AssignParameter("@IMDB", SqlDbType.NVarChar, this.IMDB);
                this.AssignParameter("@FilmeID", SqlDbType.Int, this.FilmeID);
                return this.Parameters;
            }

            public override bool CompareIt(Filme item)
            {
                return !(string.Compare(Nome, item.Nome) != 0 || string.Compare(Sinopse, item.Sinopse) != 0 || Duracao != item.Duracao || Idade != item.Idade || string.Compare(IdadeJusti, item.IdadeJusti, true) != 0 || Dublado != item.Dublado || IMDB != item.IMDB || FilmeID != item.FilmeID);
            }


        }

        public class FormaPagamentoEvento : aSync<FormaPagamentoEvento>
        {
            public override int ID { get; set; }
            public int EventoID { get; set; }
            public int FormaPagamentoID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO FormaPagamentoEvento
                            (IR_FormaPagamentoEventoID, EventoID, FormaPagamentoID)
                            VALUES
                            (@ID, @EventoID, @FormaPagamentoID)";

                }
            }

            public string ShowProperties
            {
                get { throw new NotImplementedException(); }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@FormaPagamentoID", SqlDbType.Int, this.FormaPagamentoID);
                return this.Parameters;
            }

            //Nao faz comparacao, sempre serão iguais.
            public override bool CompareIt(FormaPagamentoEvento item)
            {
                return true;
            }
        }

        public class Local : aSync<Local>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public string Endereco { get; set; }
            public string CEP { get; set; }
            public string DDDTelefone { get; set; }
            public string Telefone { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Obs { get; set; }
            public string ComoChegar { get; set; }
            public decimal TaxaMaximaEmpresa { get; set; }
            public int EmpresaID { get; set; }
            public bool BannersPadraoSite { get; set; }
            public string Pais { get; set; }
            public string Imagem { get; set; }
            public string CodigoPraca { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Local SET Nome = @Nome, Endereco = @Endereco, CEP = @CEP, DDDTelefone = @DDDTelefone, Imagem = @Imagem,
                            Telefone = @Telefone, Cidade = @Cidade, Estado = @Estado, Obs = @Obs, ComoChegar = @ComoChegar,
                            BannersPadraoSite = @BannersPadraoSite, TaxaMaximaEmpresa = @TaxaMaximaEmpresa , EmpresaID = @EmpresaID, Pais = @Pais, CodigoPraca = @CodigoPraca
                            WHERE IR_LocalID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Local
                            (IR_LocalID, Nome, Endereco, CEP, DDDTelefone, Telefone, Cidade, Estado, Obs, ComoChegar, BannersPadraoSite ,TaxaMaximaEmpresa , EmpresaID, Pais, Imagem, CodigoPraca)
                            VALUES 
                            (@ID, @Nome, @Endereco, @CEP, @DDDTelefone, @Telefone, @Cidade, @Estado, @Obs, @ComoChegar, @BannersPadraoSite, @TaxaMaximaEmpresa , @EmpresaID, @Pais, @Imagem, @CodigoPraca)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Endereco", SqlDbType.NVarChar, this.Endereco);
                this.AssignParameter("@CEP", SqlDbType.NVarChar, this.CEP ?? string.Empty);
                this.AssignParameter("@DDDTelefone", SqlDbType.NVarChar, this.DDDTelefone ?? string.Empty);
                this.AssignParameter("@Telefone", SqlDbType.NVarChar, this.Telefone ?? string.Empty);
                this.AssignParameter("@Cidade", SqlDbType.NVarChar, this.Cidade ?? string.Empty);
                this.AssignParameter("@Estado", SqlDbType.NVarChar, this.Estado ?? string.Empty);
                this.AssignParameter("@Obs", SqlDbType.NText, this.Obs.Replace(Environment.NewLine, "<br />"));
                this.AssignParameter("@ComoChegar", SqlDbType.NVarChar, this.ComoChegar.Replace(Environment.NewLine, "<br />"));
                this.AssignParameter("@BannersPadraoSite", SqlDbType.Bit, this.BannersPadraoSite);
                this.AssignParameter("@TaxaMaximaEmpresa", SqlDbType.Decimal, this.TaxaMaximaEmpresa);
                this.AssignParameter("@EmpresaID", SqlDbType.Int, this.EmpresaID);
                this.AssignParameter("@Pais", SqlDbType.NVarChar, this.Pais);
                this.AssignParameter("@Imagem", SqlDbType.NVarChar, this.Imagem ?? string.Empty);
                this.AssignParameter("@CodigoPraca", SqlDbType.NVarChar, this.CodigoPraca ?? string.Empty);
                return this.Parameters;
            }

            public override bool CompareIt(Local item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    string.Compare(this.Endereco, item.Endereco) != 0 ||
                    string.Compare(this.CEP, item.CEP) != 0 ||
                    this.DDDTelefone != item.DDDTelefone ||
                    this.Telefone != item.Telefone ||
                    string.Compare(this.Cidade, item.Cidade) != 0 ||
                    string.Compare(this.Estado, item.Estado) != 0 ||
                    string.Compare(this.Obs, item.Obs) != 0 ||
                    string.Compare(this.ComoChegar, item.ComoChegar) != 0 ||
                    this.BannersPadraoSite != item.BannersPadraoSite ||
                    decimal.Compare(this.TaxaMaximaEmpresa, item.TaxaMaximaEmpresa) != 0 ||
                    this.EmpresaID != item.EmpresaID ||
                    string.Compare(this.Pais, item.Pais) != 0 ||
                    string.Compare(this.Imagem, item.Imagem) != 0 ||
                    string.Compare(this.CodigoPraca, item.CodigoPraca) != 0)

                    return false;
                else
                    return true;
            }
        }

        public class Apresentacao : aSync<Apresentacao>
        {
            public override int ID { get; set; }
            public string Horario { get; set; }
            public int EventoID { get; set; }
            public string Programacao { get; set; }
            public string CodigoProgramacao { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Apresentacao SET
                            Horario = @Horario, EventoID = @EventoID, Programacao = @Programacao, CodigoProgramacao = @CodigoProgramacao
                            WHERE IR_ApresentacaoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Apresentacao 
                            (IR_ApresentacaoID, Horario, EventoID, Programacao, CodigoProgramacao) 
                            VALUES 
                            (@ID, @Horario, @EventoID, @Programacao, @CodigoProgramacao)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Horario", SqlDbType.NVarChar, this.Horario);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@Programacao", SqlDbType.NVarChar, this.Programacao ?? string.Empty);
                this.AssignParameter("@CodigoProgramacao", SqlDbType.NVarChar, this.CodigoProgramacao ?? string.Empty);
                return this.Parameters;
            }

            public override bool CompareIt(Apresentacao item)
            {
                if (string.Compare(this.Horario, item.Horario) != 0 ||
                    this.EventoID != item.EventoID ||
                    string.Compare(this.Programacao, item.Programacao, true) != 0 ||
                    string.Compare(this.CodigoProgramacao, item.CodigoProgramacao, true) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class Setor : aSync<Setor>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public string LugarMarcado { get; set; }
            public override int ApresentacaoID { get; set; }
            public int QuantidadeMapa { get; set; }
            public int QuantidadeDisponivel { get; set; }
            public bool AprovadoPublicacao { get; set; }
            public int PrincipalPrecoID { get; set; }
            public bool NVendeLugar { get; set; }
            public string CodigoSala { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Setor SET 
                            Nome = @Nome, LugarMarcado = @LugarMarcado, ApresentacaoID = @ApresentacaoID, QuantidadeMapa = @QuantidadeMapa, AprovadoPublicacao = @AprovadoPublicacao, PrincipalPrecoID = @PrincipalPrecoID, CodigoSala = @CodigoSala, NVendeLugar = @NVendeLugar
                            WHERE IR_SetorID = @ID AND ApresentacaoID = @ApresentacaoID ";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Setor 
                            (IR_SetorID, Nome, LugarMarcado, ApresentacaoID, QuantidadeMapa, QtdeDisponivel, AprovadoPublicacao, PrincipalPrecoID, CodigoSala, NVendeLugar)
                            VALUES
                            (@ID, @Nome, @LugarMarcado, @ApresentacaoID, @QuantidadeMapa, @QuantidadeDisponivel, @AprovadoPublicacao, @PrincipalPrecoID, @CodigoSala, @NVendeLugar)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@LugarMarcado", SqlDbType.Char, this.LugarMarcado);
                this.AssignParameter("@ApresentacaoID", SqlDbType.Int, this.ApresentacaoID);
                this.AssignParameter("@QuantidadeMapa", SqlDbType.Int, this.QuantidadeMapa);
                this.AssignParameter("@QuantidadeDisponivel", SqlDbType.Int, 1000);
                this.AssignParameter("@AprovadoPublicacao", SqlDbType.Bit, this.AprovadoPublicacao);
                this.AssignParameter("@PrincipalPrecoID", SqlDbType.Int, this.PrincipalPrecoID);
                this.AssignParameter("@CodigoSala", SqlDbType.NVarChar, this.CodigoSala ?? string.Empty);
                this.AssignParameter("@NVendeLugar", SqlDbType.Bit, this.NVendeLugar);
                return this.Parameters;
            }

            public override bool CompareIt(Setor item)
            {
                //Nao verificar a quantidade disponivel, é um job aparte que faz isso
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    string.Compare(this.LugarMarcado, item.LugarMarcado) != 0 ||
                    this.ApresentacaoID != item.ApresentacaoID ||
                    this.QuantidadeMapa != item.QuantidadeMapa ||
                    this.AprovadoPublicacao != item.AprovadoPublicacao ||
                    this.PrincipalPrecoID != item.PrincipalPrecoID ||
                    string.Compare(this.CodigoSala, item.CodigoSala) != 0 ||
                    this.NVendeLugar != item.NVendeLugar)
                    return false;
                else
                    return true;
            }
        }

        public class Preco : aSync<Preco>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public override int ApresentacaoID { get; set; }
            public int SetorID { get; set; }
            public int QuantidadePorCliente { get; set; }
            public bool Pacote { get; set; }
            public bool Serie { get; set; }
            public string CodigoCinema { get; set; }
            
            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Preco SET
                            Nome = @Nome, Valor = @Valor, ApresentacaoID = @ApresentacaoID, SetorID = @SetorID,
                            QuantidadePorCliente = @QuantidadePorCliente, Pacote = @Pacote, Serie = @Serie, CodigoCinema = @CodigoCinema
                            WHERE
                                IR_PrecoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Preco 
                            (IR_PrecoID, Nome, Valor, ApresentacaoID, SetorID, QuantidadePorCliente, Pacote, Serie, CodigoCinema)
                            VALUES 
                            (@ID, @Nome, @Valor, @ApresentacaoID, @SetorID, @QuantidadePorCliente, @Pacote, @Serie, @CodigoCinema)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Valor", SqlDbType.Decimal, this.Valor);
                this.AssignParameter("@ApresentacaoID", SqlDbType.Int, this.ApresentacaoID);
                this.AssignParameter("@SetorID", SqlDbType.Int, this.SetorID);
                this.AssignParameter("@QuantidadePorCliente", SqlDbType.Int, this.QuantidadePorCliente);
                this.AssignParameter("@Pacote", SqlDbType.Bit, this.Pacote);
                this.AssignParameter("@Serie", SqlDbType.Bit, this.Serie);
                this.AssignParameter("@CodigoCinema", SqlDbType.NVarChar, this.CodigoCinema ?? string.Empty);
                return this.Parameters;
            }

            public override bool CompareIt(Preco item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    this.Valor != item.Valor ||
                    this.ApresentacaoID != item.ApresentacaoID ||
                    this.SetorID != item.SetorID ||
                    this.QuantidadePorCliente != item.QuantidadePorCliente ||
                    this.Pacote != item.Pacote ||
                    this.Serie != item.Serie ||
                    string.Compare(this.CodigoCinema, item.CodigoCinema, true) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class PrecoParceiroMidia : aSync<PrecoParceiroMidia>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public override int ApresentacaoID { get; set; }
            public int SetorID { get; set; }
            public int QuantidadePorCliente { get; set; }
            public bool Pacote { get; set; }
            public bool Serie { get; set; }
            public string CodigoCinema { get; set; }

            public int ParceiroMidiaID { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE PrecoParceiroMidia SET
                            Nome = @Nome, Valor = @Valor, ApresentacaoID = @ApresentacaoID, SetorID = @SetorID, 
                            QuantidadePorCliente = @QuantidadePorCliente, Pacote = @Pacote, Serie = @Serie, CodigoCinema = @CodigoCinema, ParceiroMidiaID = @ParceiroMidiaID
                            WHERE IR_PrecoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PrecoParceiroMidia 
                            (IR_PrecoID, Nome, Valor, ApresentacaoID, SetorID, QuantidadePorCliente, Pacote, Serie, CodigoCinema, ParceiroMidiaID)
                            VALUES 
                            (@ID, @Nome, @Valor, @ApresentacaoID, @SetorID, @QuantidadePorCliente, @Pacote, @Serie, @CodigoCinema, @ParceiroMidiaID)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Valor", SqlDbType.Decimal, this.Valor);
                this.AssignParameter("@ApresentacaoID", SqlDbType.Int, this.ApresentacaoID);
                this.AssignParameter("@SetorID", SqlDbType.Int, this.SetorID);
                this.AssignParameter("@QuantidadePorCliente", SqlDbType.Int, this.QuantidadePorCliente);
                this.AssignParameter("@Pacote", SqlDbType.Bit, this.Pacote);
                this.AssignParameter("@Serie", SqlDbType.Bit, this.Serie);
                this.AssignParameter("@CodigoCinema", SqlDbType.NVarChar, this.CodigoCinema ?? string.Empty);
                this.AssignParameter("@ParceiroMidiaID", SqlDbType.Int, this.ParceiroMidiaID);
                return this.Parameters;
            }

            public override bool CompareIt(PrecoParceiroMidia item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    this.Valor != item.Valor ||
                    this.ApresentacaoID != item.ApresentacaoID ||
                    this.SetorID != item.SetorID ||
                    this.QuantidadePorCliente != item.QuantidadePorCliente ||
                    this.Pacote != item.Pacote ||
                    this.Serie != item.Serie ||
                    this.ParceiroMidiaID != item.ParceiroMidiaID ||
                    string.Compare(this.CodigoCinema, item.CodigoCinema, true) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class Tipo : aSync<Tipo>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Tipo SET Nome = @Nome WHERE IR_TipoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Tipo 
                            (IR_TipoID, Nome)
                            VALUES
                            (@ID, @Nome)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                return this.Parameters;
            }

            public override bool CompareIt(Tipo item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class Subtipo : aSync<Subtipo>
        {
            public override int ID { get; set; }
            public int TipoID { get; set; }
            public string Descricao { get; set; }


            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE EventoSubtipo SET
                            TipoID = @TipoID, Descricao = @Descricao
                            WHERE IR_SubtipoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO EventoSubtipo 
                            (IR_SubtipoID, TipoID, Descricao) 
                            VALUES
                            (@ID, @TipoID, @Descricao)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@TipoID", SqlDbType.Int, this.TipoID);
                this.AssignParameter("@Descricao", SqlDbType.NVarChar, this.Descricao);
                return this.Parameters;
            }

            public override bool CompareIt(Subtipo item)
            {
                if (this.TipoID != item.TipoID ||
                    string.Compare(this.Descricao, item.Descricao) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class TipoSubtipo : aSync<TipoSubtipo>
        {
            public override int ID { get; set; }
            public int EventoID { get; set; }
            public int EventoTipoID { get; set; }
            public int EventoSubTipoID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Tipos 
                            (ID, EventoID, EventoTipoID, EventoSubtipoID)
                            VALUES
                            (@ID, @EventoID, @EventoTipoID, @EventoSubtipoID)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@EventoTipoID", SqlDbType.Int, this.EventoTipoID);
                this.AssignParameter("@EventoSubtipoID", SqlDbType.Int, this.EventoSubTipoID);
                return this.Parameters;
            }

            //Não possui update
            public override bool CompareIt(TipoSubtipo item)
            {
                return true;
            }
        }

        public class TaxaEntrega : aSync<TaxaEntrega>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public int PrazoEntrega { get; set; }
            public string Estado { get; set; }
            public string ProcedimentoEntrega { get; set; }
            public bool PermitirImpressaoInternet { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE TaxaEntrega SET
                                          Nome = @Nome
                                        , Valor = @Valor
                                        , Estado = @Estado
                                        , PrazoEntrega = @PrazoEntrega
                                        , ProcedimentoEntrega = @ProcedimentoEntrega
                                        , PermitirImpressaoInternet = @PermitirImpressaoInternet
                                         WHERE IR_TaxaEntregaID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO TaxaEntrega 
                            (IR_TaxaEntregaID, Nome, Valor, Estado, PrazoEntrega, ProcedimentoEntrega, PermitirImpressaoInternet)
                            VALUES
                            (@ID, @Nome, @Valor, @Estado, @PrazoEntrega, @ProcedimentoEntrega, @PermitirImpressaoInternet)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Valor", SqlDbType.Decimal, this.Valor);
                this.AssignParameter("@PrazoEntrega", SqlDbType.Int, this.PrazoEntrega);
                this.AssignParameter("@Estado", SqlDbType.NVarChar, this.Estado);
                this.AssignParameter("@ProcedimentoEntrega", SqlDbType.NVarChar, this.ProcedimentoEntrega);
                this.AssignParameter("@PermitirImpressaoInternet", SqlDbType.Bit, this.PermitirImpressaoInternet);
                return this.Parameters;
            }

            public override bool CompareIt(TaxaEntrega item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    this.Valor != item.Valor ||
                    this.PrazoEntrega != item.PrazoEntrega ||
                    string.Compare(this.Estado, item.Estado) != 0 ||
                    string.Compare(this.ProcedimentoEntrega, item.ProcedimentoEntrega) != 0 ||
                    this.PermitirImpressaoInternet != item.PermitirImpressaoInternet)
                    return false;
                else
                    return true;

            }
        }

        public class EventoTaxaEntrega : aSync<EventoTaxaEntrega>
        {
            public override int ID { get; set; }
            public int EventoID { get; set; }
            public int TaxaEntregaID { get; set; }
            public string DetalhesEntrega { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE EventoTaxaEntrega SET
                                             TaxaEntregaID = @TaxaEntregaID
                                           , EventoID = @EventoID
                                           , DetalhesEntrega = @DetalhesEntrega
                                            WHERE IR_EventoTaxaEntregaID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO EventoTaxaEntrega 
                            (TaxaEntregaID, IR_EventoTaxaEntregaID, EventoID, DetalhesEntrega)
                            VALUES 
                            (@TaxaEntregaid, @ID, @EventoID, @DetalhesEntrega)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@TaxaEntregaID", SqlDbType.Int, this.TaxaEntregaID);
                this.AssignParameter("@DetalhesEntrega", SqlDbType.NVarChar, this.DetalhesEntrega);
                return this.Parameters;
            }

            public override bool CompareIt(EventoTaxaEntrega item)
            {
                if (string.Compare(this.DetalhesEntrega, item.DetalhesEntrega) != 0)
                    return false;
                else
                    return true;

            }
        }

        public class Pacote : aSync<Pacote>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public int NomenclaturaPacoteID { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return "UPDATE Pacote SET Nome = @Nome, NomenclaturaPacoteID = @NomenclaturaPacoteID WHERE IR_PacoteID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Pacote
                            (IR_PacoteID, Nome,NomenclaturaPacoteID)
                            VALUES
                            (@ID, @Nome,@NomenclaturaPacoteID)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@NomenclaturaPacoteID", SqlDbType.Int, this.NomenclaturaPacoteID);

                return this.Parameters;
            }

            public override bool CompareIt(Pacote item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0
                    || this.NomenclaturaPacoteID != item.NomenclaturaPacoteID)
                    return false;
                else
                    return true;
            }
        }

        public class NomenclaturaPacote : aSync<NomenclaturaPacote>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }


            public override string UpdateSQL
            {
                get
                {
                    return "UPDATE NomenclaturaPacote SET Nome = @Nome WHERE IR_NomenclaturaPacoteID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO NomenclaturaPacote
                            (IR_NomenclaturaPacoteID, Nome)
                            VALUES
                            (@ID, @Nome)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);


                return this.Parameters;
            }

            public override bool CompareIt(NomenclaturaPacote item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class PacoteItem : aSync<PacoteItem>
        {
            public override int ID { get; set; }
            public int PacoteID { get; set; }
            public int EventoID { get; set; }
            public int PrecoID { get; set; }
            public int Quantidade { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE PacoteItem SET 
                            PacoteID = @PacoteID, PrecoID = @PrecoID, EventoID = @EventoID, Quantidade = @Quantidade 
                            WHERE IR_PacoteItemID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PacoteItem 
                            (IR_PacoteItemID, PacoteID, PrecoID, EventoID, Quantidade) 
                            VALUES
                            (@ID, @PacoteID, @PrecoID, @EventoID, @Quantidade)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@PacoteID", SqlDbType.Int, this.PacoteID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@PrecoID", SqlDbType.Int, this.PrecoID);
                this.AssignParameter("@Quantidade", SqlDbType.Int, this.Quantidade);
                return this.Parameters;
            }

            public override bool CompareIt(PacoteItem item)
            {
                if (this.PacoteID != item.PacoteID ||
                    this.EventoID != item.EventoID ||
                    this.PrecoID != item.PrecoID ||
                    this.Quantidade != item.Quantidade)
                    return false;
                else
                    return true;
            }
        }

        public class Serie : aSync<Serie>
        {
            public override int ID { get; set; }
            public string Titulo { get; set; }
            public string Nome { get; set; }

            public int QuantidadeMinimaGrupo { get; set; }
            public int QuantidadeMaximaGrupo { get; set; }

            public int QuantidadeMinimaApresentacao { get; set; }
            public int QuantidadeMaximaApresentacao { get; set; }

            public int QuantidadeMinimaIngressosPorApresentacao { get; set; }
            public int QuantidadeMaximaIngressosPorApresentacao { get; set; }

            public string Regras { get; set; }
            public string Descricao { get; set; }
            public string Tipo { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Serie SET
                            Titulo = @Titulo, Nome = @Nome, 
                            QuantidadeMinimaGrupo = @QuantidadeMinimaGrupo,
                            QuantidadeMaximaGrupo = @QuantidadeMaximaGrupo,
                            QuantidadeMinimaApresentacao = @QuantidadeMinimaApresentacao,
                            QuantidadeMaximaApresentacao = @QuantidadeMaximaApresentacao, 
                            QuantidadeMinimaIngressosPorApresentacao = @QuantidadeMinimaIngressosPorApresentacao,
                            QuantidadeMaximaIngressosPorApresentacao = @QuantidadeMaximaIngressosPorApresentacao,
                            Regras = @Regras, Descricao = @Descricao, Tipo = @Tipo
                            WHERE IR_SerieID = @ID";
                }
            }
            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Serie
                            (IR_SerieID, Titulo, Nome, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo, QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, 
                                QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao, Descricao, Regras, Tipo)
                            VALUES
                            (@ID, @Titulo, @Nome, @QuantidadeMinimaGrupo, @QuantidadeMaximaGrupo, @QuantidadeMinimaApresentacao, @QuantidadeMaximaApresentacao, 
                                @QuantidadeMinimaIngressosPorApresentacao, @QuantidadeMaximaIngressosPorApresentacao, @Descricao, @Regras, @Tipo)";
                }
            }
            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Titulo", SqlDbType.NVarChar, this.Titulo);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);

                this.AssignParameter("@QuantidadeMinimaGrupo", SqlDbType.Int, this.QuantidadeMinimaGrupo);
                this.AssignParameter("@QuantidadeMaximaGrupo", SqlDbType.Int, this.QuantidadeMaximaGrupo);

                this.AssignParameter("@QuantidadeMinimaApresentacao", SqlDbType.Int, this.QuantidadeMinimaApresentacao);
                this.AssignParameter("@QuantidadeMaximaApresentacao", SqlDbType.Int, this.QuantidadeMaximaApresentacao);

                this.AssignParameter("@QuantidadeMinimaIngressosPorApresentacao", SqlDbType.Int, this.QuantidadeMinimaIngressosPorApresentacao);
                this.AssignParameter("@QuantidadeMaximaIngressosPorApresentacao", SqlDbType.Int, this.QuantidadeMaximaIngressosPorApresentacao);

                this.AssignParameter("@Regras", SqlDbType.NVarChar, this.Regras);
                this.AssignParameter("@Descricao", SqlDbType.NVarChar, this.Descricao);
                this.AssignParameter("@Tipo", SqlDbType.NVarChar, this.Tipo);

                return this.Parameters;
            }
            public override bool CompareIt(Serie item)
            {
                if (string.Compare(this.Titulo, item.Titulo) != 0 ||
                    string.Compare(this.Nome, item.Nome) != 0 ||
                    this.QuantidadeMinimaGrupo != item.QuantidadeMinimaGrupo ||
                    this.QuantidadeMaximaGrupo != item.QuantidadeMaximaGrupo ||
                    this.QuantidadeMaximaApresentacao != item.QuantidadeMaximaApresentacao ||
                    this.QuantidadeMinimaApresentacao != item.QuantidadeMinimaApresentacao ||
                    this.QuantidadeMinimaIngressosPorApresentacao != item.QuantidadeMinimaIngressosPorApresentacao ||
                    this.QuantidadeMaximaIngressosPorApresentacao != item.QuantidadeMaximaIngressosPorApresentacao ||
                    string.Compare(this.Regras, item.Regras) != 0 ||
                    string.Compare(this.Descricao, item.Descricao) != 0 ||
                    String.Compare(this.Tipo, item.Tipo) != 0)
                    return false;
                else
                    return true;
            }



        }

        public class SerieItem : aSync<SerieItem>
        {
            public override int ID { get; set; }
            public int SerieID { get; set; }
            public int EventoID { get; set; }
            public int ApresentacaoID { get; set; }
            public int SetorID { get; set; }
            public int PrecoID { get; set; }
            public bool Promocional { get; set; }
            public int QuantidadePorPromocional { get; set; }
            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE SerieItem SET 
                            SerieID = @SerieID, PrecoID = @PrecoID, EventoID = @EventoID, ApresentacaoID = @ApresentacaoID,
                            SetorID = @SetorID, Promocional = @Promocional, QuantidadePorPromocional = @QuantidadePorPromocional
                            WHERE IR_SerieItemID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO SerieItem 
                            (IR_SerieItemID, SerieID, PrecoID, EventoID, ApresentacaoID, SetorID, Promocional, QuantidadePorPromocional) 
                            VALUES
                            (@ID, @SerieID, @PrecoID, @EventoID, @ApresentacaoID, @SetorID, @Promocional, @QuantidadePorPromocional)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@SerieID", SqlDbType.Int, this.SerieID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@ApresentacaoID", SqlDbType.Int, this.ApresentacaoID);
                this.AssignParameter("@SetorID", SqlDbType.Int, this.SetorID);
                this.AssignParameter("@PrecoID", SqlDbType.Int, this.PrecoID);
                this.AssignParameter("@Promocional", SqlDbType.Bit, this.Promocional);
                this.AssignParameter("@QuantidadePorPromocional", SqlDbType.Int, this.QuantidadePorPromocional);
                return this.Parameters;
            }

            public override bool CompareIt(SerieItem item)
            {
                if (this.SerieID != item.SerieID ||
                    this.EventoID != item.EventoID ||
                    this.PrecoID != item.PrecoID ||
                    this.ApresentacaoID != item.ApresentacaoID ||
                    this.SetorID != item.SetorID ||
                    this.Promocional != item.Promocional ||
                    this.QuantidadePorPromocional != item.QuantidadePorPromocional)
                    return false;
                else
                    return true;
            }
        }

        public class Banner : aSync<Banner>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public string Alt { get; set; }
            public string Url { get; set; }
            public int Target { get; set; }
            public int Localizacao { get; set; }
            public int Posicao { get; set; }
            public string Descricao { get; set; }
            public string Img { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Banner SET 
                            Nome = @Nome, Alt = @Alt, Url = @Url, Target = @Target, Localizacao = @Localizacao, Posicao = @Posicao,
                            Descricao = @Descricao, Img = @Img WHERE ID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO BANNER 
                            (ID, Nome, Alt, Url, Target, Localizacao, Posicao, Descricao, Img)
                            VALUES 
                            (@ID, @Nome, @Alt, @Url, @Target, @Localizacao, @Posicao, @Descricao, @Img)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Alt", SqlDbType.NVarChar, this.Alt);
                this.AssignParameter("@Url", SqlDbType.NVarChar, this.Url);
                this.AssignParameter("@Target", SqlDbType.Int, this.Target);
                this.AssignParameter("@Localizacao", SqlDbType.Int, this.Localizacao);
                this.AssignParameter("@Posicao", SqlDbType.Int, this.Posicao);
                this.AssignParameter("@Descricao", SqlDbType.NVarChar, this.Descricao);
                this.AssignParameter("@Img", SqlDbType.NVarChar, this.Img);
                return this.Parameters;
            }

            public override bool CompareIt(Banner item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    string.Compare(this.Alt, item.Alt) != 0 ||
                    string.Compare(this.Url, item.Url) != 0 ||
                    this.Target != item.Target ||
                    this.Localizacao != item.Localizacao ||
                    this.Posicao != item.Posicao ||
                    string.Compare(this.Descricao, item.Descricao) != 0 ||
                    string.Compare(this.Img, item.Img) != 0)
                    return false;
                else
                    return true;


            }
        }

        public class EventoTipoDestaque : aSync<EventoTipoDestaque>
        {
            public override int ID { get; set; }
            public int EventoID { get; set; }
            public int EventoTipoID { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE EventoTipoDestaque SET 
                             EventoID = @EventoID, EventoTipoID = @EventoTipoID,                            
                             WHERE IR_EventoTipoDestaqueID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO EventoTipoDestaque 
                            (IR_EventoTipoDestaqueID, EventoID, EventoTipoID)
                            VALUES
                            (@ID, @EventoID, @EventoTipoID)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@EventoID", SqlDbType.Int, this.EventoID);
                this.AssignParameter("@EventoTipoID", SqlDbType.Int, this.EventoTipoID);

                return this.Parameters;
            }

            public override bool CompareIt(EventoTipoDestaque item)
            {
                if (this.EventoID != item.EventoID || this.EventoTipoID != item.EventoTipoID)
                    return false;
                else
                    return true;
            }
        }

        public class ValeIngressoTipo : aSync<ValeIngressoTipo>
        {
            public override int ID { get; set; }
            public string Nome { get; set; }
            public decimal Valor { get; set; }
            public string ValidadeData { get; set; }
            public int ValidadeDiasImpressao { get; set; }
            public string ProcedimentoTroca { get; set; }
            public char Acumulativo { get; set; }
            public string ReleaseInternet { get; set; }
            public decimal ValorPagamento { get; set; }
            public char ValorTipo { get; set; }
            public char TrocaConveniencia { get; set; }
            public char TrocaIngresso { get; set; }
            public char TrocaEntrega { get; set; }

            public override string UpdateSQL
            {
                get
                {

                    return @"UPDATE ValeIngressoTipo SET 
                            Nome = @Nome, Valor = @Valor, 
                            ValorPagamento = @ValorPagamento,ValorTipo = @ValorTipo,
                            ValidadeData = @ValidadeData, ValidadeDiasImpressao = @ValidadeDiasImpressao, TrocaConveniencia = @TrocaConveniencia, 
                            TrocaIngresso = @TrocaIngresso,  TrocaEntrega = @TrocaEntrega, 
                            ProcedimentoTroca = @ProcedimentoTroca, Acumulativo = @Acumulativo, ReleaseInternet = @ReleaseInternet
                            WHERE IR_ValeIngressoTipoID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO ValeIngressoTipo 
                            (IR_ValeIngressoTipoID, Nome, Valor, ValidadeData, ValidadeDiasImpressao, ProcedimentoTroca, 
                            Acumulativo, ReleaseInternet,ValorPagamento,ValorTipo,TrocaIngresso,TrocaConveniencia,TrocaEntrega)
                            VALUES
                            (@ID, @Nome,@Valor, @ValidadeData, @ValidadeDiasImpressao, @ProcedimentoTroca,
                            @Acumulativo, @ReleaseInternet,@ValorPagamento,@ValorTipo,@TrocaIngresso,@TrocaConveniencia,@TrocaEntrega)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Valor", SqlDbType.Decimal, this.Valor);
                this.AssignParameter("@ValidadeData", SqlDbType.NVarChar, this.ValidadeData);
                this.AssignParameter("@ValidadeDiasImpressao", SqlDbType.Int, this.ValidadeDiasImpressao);
                this.AssignParameter("@ProcedimentoTroca", SqlDbType.NVarChar, this.ProcedimentoTroca);
                this.AssignParameter("@Acumulativo", SqlDbType.Char, this.Acumulativo);
                this.AssignParameter("@ReleaseInternet", SqlDbType.NVarChar, this.ReleaseInternet);
                this.AssignParameter("@ValorPagamento", SqlDbType.Decimal, this.ValorPagamento);
                this.AssignParameter("@ValorTipo", SqlDbType.Char, this.ValorTipo);
                this.AssignParameter("@TrocaIngresso", SqlDbType.Char, this.TrocaIngresso);
                this.AssignParameter("@TrocaConveniencia", SqlDbType.Char, this.TrocaConveniencia);
                this.AssignParameter("@TrocaEntrega", SqlDbType.Char, this.TrocaEntrega);
                return this.Parameters;
            }

            public override bool CompareIt(ValeIngressoTipo item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0 ||
                    this.Valor != item.Valor ||
                    string.Compare(this.ValidadeData, item.ValidadeData) != 0 ||
                    this.ValidadeDiasImpressao != item.ValidadeDiasImpressao ||
                    string.Compare(this.ProcedimentoTroca, item.ProcedimentoTroca) != 0 ||
                    this.Acumulativo != item.Acumulativo ||
                    string.Compare(this.ReleaseInternet, item.ReleaseInternet) != 0 ||
                    this.ValorPagamento != item.ValorPagamento ||
                    this.ValorTipo != item.ValorTipo ||
                    this.TrocaIngresso != item.TrocaIngresso ||
                    this.TrocaConveniencia != item.TrocaConveniencia ||
                    this.TrocaEntrega != item.TrocaEntrega
                   )
                    return false;
                else
                    return true;
            }
        }

        public class PontoVenda : aSync<PontoVenda>
        {
            public override int ID { get; set; }
            public string Local { get; set; }
            public string Nome { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Compl { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Bairro { get; set; }
            public string Obs { get; set; }
            public string Referencia { get; set; }
            public string CEP { get; set; }
            public string PermiteRetirada { get; set; }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE PontoVenda SET
                            Local = @Local,
                            Nome = @Nome,
                            Endereco = @Endereco,
                            Numero = @Numero,
                            Compl = @Compl,
                            Cidade = @Cidade,
                            Estado = @Estado,
                            Bairro = @Bairro,
                            Obs = @Obs,
                            Referencia = @Referencia,
                            CEP = @CEP,
                            PermiteRetirada = @PermiteRetirada
                            WHERE IR_PontoVendaID = @ID";
                }
            }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PontoVenda
                            (IR_PontoVendaID, Local, Nome, Endereco, Numero, Compl, Cidade,
                            Estado, Bairro, Obs, Referencia, CEP, PermiteRetirada) 
                            VALUES
                            (@ID, @Local, @Nome, @Endereco, @Numero, @Compl, @Cidade,
                            @Estado, @Bairro, @Obs, @Referencia, @CEP, @PermiteRetirada)";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Local", SqlDbType.NVarChar, this.Local);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                this.AssignParameter("@Endereco", SqlDbType.NVarChar, this.Endereco);
                this.AssignParameter("@Compl", SqlDbType.NVarChar, this.Compl);
                this.AssignParameter("@Cidade", SqlDbType.NVarChar, this.Cidade);
                this.AssignParameter("@Estado", SqlDbType.NVarChar, this.Estado);
                this.AssignParameter("@Bairro", SqlDbType.NVarChar, this.Bairro);
                this.AssignParameter("@Obs", SqlDbType.NVarChar, this.Obs);
                this.AssignParameter("@Referencia", SqlDbType.NVarChar, this.Referencia);
                this.AssignParameter("@CEP", SqlDbType.NVarChar, this.CEP);
                this.AssignParameter("@Numero", SqlDbType.NVarChar, this.Numero);
                this.AssignParameter("@PermiteRetirada", SqlDbType.NVarChar, this.PermiteRetirada);
                return this.Parameters;
            }

            public override bool CompareIt(PontoVenda item)
            {
                if (string.Compare(this.Local, item.Local) != 0 ||
                    string.Compare(this.Nome, item.Nome) != 0 ||
                    string.Compare(this.Endereco, item.Endereco) != 0 ||
                    string.Compare(this.Compl, item.Compl) != 0 ||
                    string.Compare(this.Cidade, item.Cidade) != 0 ||
                    string.Compare(this.Estado, item.Estado) != 0 ||
                    string.Compare(this.Bairro, item.Bairro) != 0 ||
                    string.Compare(this.Obs, item.Obs) != 0 ||
                    string.Compare(this.Referencia, item.Referencia) != 0 ||
                    string.Compare(this.CEP, item.CEP) != 0 ||
                    string.Compare(this.Numero, item.Numero) != 0 ||
                    string.Compare(this.PermiteRetirada, item.PermiteRetirada) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class PontoVendaXFormaPgto : aSync<PontoVendaXFormaPgto>
        {

            public override int ID { get; set; }
            public int PontoVendaID { get; set; }
            public int PontoVendaFormaPgtoID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PontoVendaXFormaPgto (IR_PontoVendaXFormaPgtoID, PontoVendaFormaPgtoID, PontoVendaID)
                            VALUES 
                            (@ID, @PontoVendaFormaPgtoID, @PontoVendaID)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return string.Empty;
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@PontoVendaFormaPgtoID", SqlDbType.Int, this.PontoVendaFormaPgtoID);
                this.AssignParameter("@PontoVendaID", SqlDbType.Int, this.PontoVendaID);
                return this.Parameters;
            }

            //Nao possui Update!
            public override bool CompareIt(PontoVendaXFormaPgto item)
            {
                return true;
            }
        }

        public class PontoVendaHorario : aSync<PontoVendaHorario>
        {
            public override int ID { get; set; }
            public int PontoVendaID { get; set; }
            public string HorarioInicial { get; set; }
            public string HorarioFinal { get; set; }
            public int DiaSemana { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PontoVendaHorario (IR_PontoVendaHorarioID, PontoVendaID, HorarioInicial, HorarioFinal, DiaSemana)
                            VALUES
                            (@ID, @PontoVendaID, @HorarioInicial, @HorarioFinal, @DiaSemana)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE PontoVendaHorario SET PontoVendaID = @PontoVendaID,
                            HorarioInicial = @HorarioInicial,
                            HorarioFinal = @HorarioFinal,
                            DiaSemana = @DiaSemana
                            WHERE IR_PontoVendaHorarioID = @ID";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@PontoVendaID", SqlDbType.Int, this.PontoVendaID);
                this.AssignParameter("@HorarioInicial", SqlDbType.NVarChar, this.HorarioInicial);
                this.AssignParameter("@HorarioFinal", SqlDbType.NVarChar, this.HorarioFinal);
                this.AssignParameter("@DiaSemana", SqlDbType.Int, this.DiaSemana);
                return this.Parameters;
            }

            public override bool CompareIt(PontoVendaHorario item)
            {
                if (this.PontoVendaID != item.PontoVendaID ||
                    string.Compare(this.HorarioInicial, item.HorarioInicial) != 0 ||
                    string.Compare(this.HorarioFinal, item.HorarioFinal) != 0 ||
                    this.DiaSemana != item.DiaSemana)
                    return false;
                else
                    return true;
            }
        }

        public class PontoVendaFormaPgto : aSync<PontoVendaFormaPgto>
        {

            public override int ID { get; set; }
            public string Nome { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO PontoVendaFormaPgto (IR_PontoVendaFormaPgtoID, Nome) 
                            VALUES 
                                (@ID, @Nome)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE PontoVendaFormaPgto SET Nome = @Nome WHERE IR_PontoVendaFormaPgtoID = @ID ";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Nome", SqlDbType.NVarChar, this.Nome);
                return this.Parameters;
            }

            public override bool CompareIt(PontoVendaFormaPgto item)
            {
                if (string.Compare(this.Nome, item.Nome) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class FormaPagamentoCotaItem : aSync<FormaPagamentoCotaItem>
        {
            public override int ID { get; set; }
            public int FormaPagamentoID { get; set; }
            public int CotaItemID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO FormaPagamentoCotaItem
                            (IR_FormaPagamentoCotaItemID, CotaItemID, FormaPagamentoID)
                            VALUES
                            (@ID, @CotaItemID, @FormaPagamentoID)";

                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@CotaItemID", SqlDbType.Int, this.CotaItemID);
                this.AssignParameter("@FormaPagamentoID", SqlDbType.Int, this.FormaPagamentoID);
                return this.Parameters;
            }

            //Nao faz comparacao, sempre serão iguais.
            public override bool CompareIt(FormaPagamentoCotaItem item)
            {
                return true;
            }
        }

        public class FormaPagamentoSerie : aSync<FormaPagamentoSerie>
        {
            public override int ID { get; set; }
            public int FormaPagamentoID { get; set; }
            public int SerieID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO FormaPagamentoSerie
                            (IR_FormaPagamentoSerieID, SerieID, FormaPagamentoID)
                            VALUES
                            (@ID, @SerieID, @FormaPagamentoID)";

                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@SerieID", SqlDbType.Int, this.SerieID);
                this.AssignParameter("@FormaPagamentoID", SqlDbType.Int, this.FormaPagamentoID);
                return this.Parameters;
            }

            //Nao faz comparacao, sempre serão iguais.
            public override bool CompareIt(FormaPagamentoSerie item)
            {
                return true;
            }
        }

        //public class FormaPagamento : aSync<FormaPagamento>
        //{
        //    public override int ID { get; set; }

        //    public override List<SqlParameter> setParameters()
        //    {
        //        this.Parameters = new List<SqlParameter>();
        //        this.AssignParameter("@ID", SqlDbType.Int, this.ID);
        //        return this.Parameters;
        //    }

        //    public override string InsertSQL
        //    {
        //        get
        //        {
        //            return @"";
        //        }
        //    }

        //    public override string UpdateSQL
        //    {
        //        get
        //        {
        //            return @"";
        //        }
        //    }
        //    public override bool CompareIt(FormaPagamento item)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public class CotaItem : aSync<CotaItem>
        {
            public override int ID { get; set; }
            public bool ValidaBin { get; set; }
            public bool Nominal { get; set; }
            public string Termo { get; set; }
            public int ParceiroID { get; set; }
            public string TextoValidacao { get; set; }
            public int ObrigatoriedadeID { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO CotaItem (IR_CotaItemID, ValidaBin, Nominal, Termo, ParceiroID, TextoValidacao, ObrigatoriedadeID) 
                                VALUES 
                                (@ID, @ValidaBin, @Nominal, @Termo, @ParceiroID, @TextoValidacao, @ObrigatoriedadeID)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE CotaItem 
                                SET ValidaBin = @ValidaBin, Nominal = @Nominal, Termo = @Termo, 
                                    ParceiroID = @ParceiroID, TextoValidacao = @TextoValidacao, ObrigatoriedadeID = @ObrigatoriedadeID
                            WHERE IR_CotaItemID = @ID ";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@ValidaBin", SqlDbType.BigInt, this.ValidaBin);
                this.AssignParameter("@Nominal", SqlDbType.Bit, this.Nominal);
                this.AssignParameter("@Termo", SqlDbType.NVarChar, this.Termo);
                this.AssignParameter("@ParceiroID", SqlDbType.Int, this.ParceiroID);
                this.AssignParameter("@TextoValidacao", SqlDbType.NVarChar, this.TextoValidacao);
                this.AssignParameter("@ObrigatoriedadeID", SqlDbType.Int, this.ObrigatoriedadeID);
                return this.Parameters;
            }

            public override bool CompareIt(CotaItem item)
            {
                return
                    !(this.ValidaBin != item.ValidaBin ||
                    this.Nominal != item.Nominal ||
                    this.Termo.Length != item.Termo.Length ||
                    this.ParceiroID != item.ParceiroID ||
                    string.Compare(this.TextoValidacao, item.TextoValidacao) != 0 ||
                    this.ObrigatoriedadeID != item.ObrigatoriedadeID);
            }
        }

        public class VoceSabia : aSync<VoceSabia>
        {
            public override int ID { get; set; }
            public string Identificacao { get; set; }
            public string Texto { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO VoceSabia (ID , Identificacao, Texto) 
                                VALUES (@ID , @Identificacao, @Texto)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE VoceSabia 
                                SET Identificacao = @Identificacao, Texto = @Texto
                            WHERE ID = @ID ";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Identificacao", SqlDbType.NVarChar, this.Identificacao);
                this.AssignParameter("@Texto", SqlDbType.NVarChar, this.Texto);
                return this.Parameters;
            }

            public override bool CompareIt(VoceSabia item)
            {
                return
                    !(string.Compare(this.Identificacao, item.Identificacao) != 0 ||
                    string.Compare(this.Texto, item.Texto) != 0);
            }
        }

        public class Faq : aSync<Faq>
        {
            public override int ID { get; set; }
            public string Pergunta { get; set; }
            public string Resposta { get; set; }
            public string FaqTipo { get; set; }
            public string Tag { get; set; }
            public string Exibicao { get; set; }

            public override string InsertSQL
            {
                get
                {
                    return @"INSERT INTO Faq (IR_Faq, Pergunta, Resposta, FaqTipo, Tags, Exibicao) 
                                VALUES (@ID , @Pergunta, @Resposta, @FaqTipo, @Tag, @Exibicao)";
                }
            }

            public override string UpdateSQL
            {
                get
                {
                    return @"UPDATE Faq 
                                SET Pergunta = @Pergunta, Resposta = @Resposta, FaqTipo =@FaqTipo, Tags =  @Tag, Exibicao = @Exibicao
                            WHERE IR_Faq = @ID";
                }
            }

            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                this.AssignParameter("@Pergunta", SqlDbType.NVarChar, this.Pergunta);
                this.AssignParameter("@Resposta", SqlDbType.NVarChar, this.Resposta);
                this.AssignParameter("@FaqTipo", SqlDbType.NVarChar, this.FaqTipo);
                this.AssignParameter("@Tag", SqlDbType.NVarChar, this.Tag);
                this.AssignParameter("@Exibicao", SqlDbType.NVarChar, this.Exibicao);
                return this.Parameters;
            }

            public override bool CompareIt(Faq item)
            {
                if (string.Compare(this.Pergunta, item.Pergunta) != 0 ||
                    string.Compare(this.Resposta, item.Resposta) != 0 ||
                    string.Compare(this.FaqTipo, item.FaqTipo) != 0 ||
                    string.Compare(this.Tag, item.Tag) != 0 ||
                    string.Compare(this.Exibicao, item.Exibicao) != 0)
                    return false;
                else
                    return true;
            }
        }

        public class LoteStatus : aSync<LoteStatus>
        {
            public override int ID { get; set; }
            public DateTime DataLimite { get; set; }
            public int Quantidade { get; set; }
            public int Vendidos { get; set; }
            public int LoteSeguinte { get; set; }
            public string LoteSeguinteStatus { get; set; }
            public override string UpdateSQL
            {
                get
                {
                    string sql = "";
                    if (this.Vendidos >= this.Quantidade || (this.DataLimite != DateTime.MinValue && this.DataLimite < DateTime.Now))
                    {
                        sql = @"INSERT INTO xLote SELECT ID, (SELECT MAX(Versao) FROM cLote(NOLOCK) WHERE ID = t.ID) AS Versao, Nome, Status, Quantidade, DataLimite, LoteAnterior, ApresentacaoSetorID FROM tLote(NOLOCK) AS t WHERE ID = @ID;
                                INSERT INTO cLote SELECT ID, (SELECT MAX(Versao)+1 FROM cLote(NOLOCK) WHERE ID = t.ID) AS Versao, 'U' AS Acao, @Date AS TimeStamp, 0 AS UsuarioID FROM tLote(NOLOCK) AS t WHERE ID = @ID;
                                UPDATE tLote SET Status = 'E' WHERE ID = @ID;";
                    }
                    if (this.LoteSeguinte > 0 && this.LoteSeguinteStatus != "A" && this.LoteSeguinteStatus != "E")
                    {
                        sql += @"INSERT INTO xLote SELECT ID, (SELECT MAX(Versao) FROM cLote(NOLOCK) WHERE ID = t.ID) AS Versao, Nome, Status, Quantidade, DataLimite, LoteAnterior, ApresentacaoSetorID FROM tLote(NOLOCK) AS t WHERE ID = @LoteSeguinte;
                                 INSERT INTO cLote SELECT ID, (SELECT MAX(Versao)+1 FROM cLote(NOLOCK) WHERE ID = t.ID) AS Versao, 'U' AS Acao, @Date AS TimeStamp, 0 AS UsuarioID FROM tLote(NOLOCK) AS t WHERE ID = @LoteSeguinte;
                                 UPDATE tLote SET Status = 'A' WHERE ID = @LoteSeguinte";
                    }
                    return sql;
                }
            }
            public override List<SqlParameter> setParameters()
            {
                this.Parameters = new List<SqlParameter>();
                this.AssignParameter("@Date", SqlDbType.NVarChar, DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                if (this.Vendidos >= this.Quantidade || (this.DataLimite != DateTime.MinValue && this.DataLimite < DateTime.Now))
                {
                    this.AssignParameter("@ID", SqlDbType.Int, this.ID);
                }
                if (this.LoteSeguinte > 0 && this.LoteSeguinteStatus != "A")
                {
                    this.AssignParameter("@LoteSeguinte", SqlDbType.Int, this.LoteSeguinte);
                }
                return this.Parameters;
            }
            public override bool CompareIt(LoteStatus item)
            {
                return (this.ID == item.ID && this.DataLimite == item.DataLimite && this.Quantidade == item.Quantidade && this.Vendidos == item.Vendidos && this.LoteSeguinte == item.LoteSeguinte && this.LoteSeguinteStatus == this.LoteSeguinteStatus);
            }
        }

    }
}
