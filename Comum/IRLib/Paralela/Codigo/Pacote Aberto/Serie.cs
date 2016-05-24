/**************************************************
* Arquivo: Serie.cs
* Gerado: 10/01/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ClientObjects.Serie;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IRLib.Paralela
{

    public class Serie : Serie_B
    {
        public enum enumTipoCarregar
        {
            Especial = 0,
            Regional = 1,
            Local = 2,
        }

        public enum enumTipoConsulta
        {
            Todos = 0,
            Regional = 1,
            Local = 2,
        }

        public enum enumTipoExibicaoSerie
        {
            [Description("Tabela")]
            Tabela = 'T',
            [Description("Calendário")]
            Calendario = 'C'
        }

        public Serie() { }

        public Serie(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public Dictionary<string, string> Tipos()
        {
            return Utils.Enums.EnumToDictionary<enumTipoExibicaoSerie>();
        }


        public List<EstruturaSerie> Carregar(enumTipoConsulta tipoConsulta, int ID, string Titulo, string Nome)
        {
            try
            {
                List<EstruturaSerie> lista = new List<EstruturaSerie>();
                string sql = string.Empty;

                switch (tipoConsulta)
                {
                    case enumTipoConsulta.Todos:
                        sql = string.Format(@"SELECT 
                                                        s.ID, s.Titulo, s.Nome, s.QuantidadeMaximaApresentacao, s.QuantidadeMinimaApresentacao,
                                                        s.QuantidadeMinimaGrupo, s.QuantidadeMaximaGrupo, s.QuantidadeMinimaIngressosPorApresentacao,
                                                        s.QuantidadeMaximaIngressosPorApresentacao,
                                                        s.Regras, s.Descricao, s.LocalID, l.EmpresaID, IsNull(l.Nome, '--') AS Local, 
                                                        r.ID AS RegionalID, IsNull(r.Nome, '--') AS Regional, s.Tipo
                                                    FROM tSerie s (NOLOCK)
                                                    LEFT JOIN tLocal l (NOLOCK) ON l.ID = s.LocalID
                                                    LEFT JOIN tEmpresa e (NOLOCK) ON e.ID = l.EmpresaID
                                                    LEFT JOIN tRegional r (NOLOCK) ON r.ID = s.RegionalID OR r.ID = e.RegionalID
                                                    {0} 
                                                    ORDER BY s.Titulo, s.Nome, r.Nome, l.Nome",
                                            Titulo.Length > 0 && Nome.Length > 0 ?
                                                string.Format("WHERE s.Titulo LIKE '%{0}%' AND s.Nome LIKE '%{1}%'", Titulo, Nome) :
                                            Titulo.Length > 0 ?
                                                string.Format("WHERE s.Titulo LIKE '%{0}%'", Titulo) :
                                            Nome.Length > 0 ?
                                                string.Format("WHERE s.Nome LIKE '%{0}%'", Nome) :
                                            string.Empty);
                        break;
                    case enumTipoConsulta.Regional:
                        sql = string.Format(@"SELECT 
                                                        s.ID, s.Titulo, s.Nome, s.QuantidadeMaximaApresentacao, s.QuantidadeMinimaApresentacao,
                                                        s.QuantidadeMinimaGrupo, s.QuantidadeMaximaGrupo, s.QuantidadeMinimaIngressosPorApresentacao,
                                                        s.QuantidadeMaximaIngressosPorApresentacao,
                                                        s.Regras, s.Descricao, s.LocalID, l.EmpresaID, IsNull(l.Nome, '--') AS Local, 
                                                        r.ID AS RegionalID, IsNull(r.Nome, '--') AS Regional, s.Tipo
                                                    FROM tSerie s (NOLOCK)
                                                    LEFT JOIN tLocal l (NOLOCK) ON l.ID = s.LocalID
                                                    LEFT JOIN tEmpresa e (NOLOCK) ON e.ID = l.EmpresaID
                                                    INNER JOIN tRegional r (NOLOCK) ON r.ID = s.RegionalID OR e.RegionalID = e.RegionalID
                                                    WHERE r.ID = {0} {1}
                                                    ORDER BY s.Titulo, s.Nome, r.Nome, l.Nome", ID,
                                            Titulo.Length > 0 && Nome.Length > 0 ?
                                                string.Format("AND s.Titulo LIKE '%{0}%' AND s.Nome LIKE '%{1}%'", Titulo, Nome) :
                                            Titulo.Length > 0 ?
                                                string.Format("AND s.Titulo LIKE '%{0}%'", Titulo) :
                                            Nome.Length > 0 ?
                                                string.Format("AND s.Nome LIKE '%{0}%'", Nome) :
                                                string.Empty);
                        break;
                    case enumTipoConsulta.Local:
                        sql = string.Format(@"SELECT 
                                                        s.ID, s.Titulo, s.Nome, s.QuantidadeMaximaApresentacao, s.QuantidadeMinimaApresentacao,
                                                        s.QuantidadeMinimaGrupo, s.QuantidadeMaximaGrupo, s.QuantidadeMinimaIngressosPorApresentacao,
                                                        s.QuantidadeMaximaIngressosPorApresentacao,
                                                        s.Regras, s.Descricao, s.LocalID, l.EmpresaID, IsNull(l.Nome, '--') AS Local, 
                                                        r.ID AS RegionalID, IsNull(r.Nome, '--') AS Regional, s.Tipo
                                                    FROM tSerie s (NOLOCK)
                                                    INNER JOIN tLocal l (NOLOCK) ON l.ID = s.LocalID
                                                    INNER JOIN tEmpresa e (NOLOCK) ON e.ID = l.EmpresaID
                                                    INNER JOIN tRegional r (NOLOCK) ON r.ID = e.RegionalID
                                                    WHERE s.LocalID = {0} {1}
                                                    ORDER BY s.Titulo, s.Nome, r.Nome, l.Nome", ID,
                                            Titulo.Length > 0 && Nome.Length > 0 ?
                                                string.Format("AND s.Titulo LIKE '%{0}%' AND s.Nome LIKE '%{1}%'", Titulo, Nome) :
                                            Titulo.Length > 0 ?
                                                string.Format("AND s.Titulo LIKE '%{0}%'", Titulo) :
                                            Nome.Length > 0 ?
                                                string.Format("AND s.Nome LIKE '%{0}%'", Nome) :
                                                string.Empty);
                        break;
                }

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaSerie()
                    {
                        ID = bd.LerInt("ID"),
                        Titulo = bd.LerString("Titulo"),
                        Nome = bd.LerString("Nome"),
                        RegionalID = bd.LerInt("RegionalID"),
                        Regional = bd.LerString("Regional"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        LocalID = bd.LerInt("LocalID"),
                        Local = bd.LerString("Local"),
                        QuantidadeMaximaApresentacao = bd.LerInt("QuantidadeMaximaApresentacao"),
                        QuantidadeMinimaApresentacao = bd.LerInt("QuantidadeMinimaApresentacao"),
                        QuantidadeMaximaGrupo = bd.LerInt("QuantidadeMaximaGrupo"),
                        QuantidadeMinimaGrupo = bd.LerInt("QuantidadeMinimaGrupo"),
                        QuantidadeMinimaIngressosPorApresentacao = bd.LerInt("QuantidadeMinimaIngressosPorApresentacao"),
                        QuantidadeMaximaIngressosPorApresentacao = bd.LerInt("QuantidadeMaximaIngressosPorApresentacao"),
                        Descricao = bd.LerString("Descricao"),
                        Regras = bd.LerString("Regras"),
                        Tipo = bd.LerString("Tipo"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Carregar os dados desta Serie apartir do ID,
        /// Serão Carregados: Canais Próprios, Canais IR, Disponibilidade Site/CC
        ///                   Precos Disponíveis, Preços 
        /// </summary>
        /// <param name="ID"></param>
        public EstruturaPreenchimentoSerie CarregarCanaisPrecosFormaPagamento(int SerieID, Enumerators.TipoPerfil tipoPerfil, int ID)
        {
            try
            {
                bool Alterar = true;

                EstruturaPreenchimentoSerie ePreenchimentoSerie = this.PreencherCanaisIR(SerieID);
                ePreenchimentoSerie.lstCanaisDisponiveis = this.PreencherCanais(SerieID);
                ePreenchimentoSerie.lstPrecosDisponiveis = this.PreencherPrecos(SerieID, tipoPerfil, ID, ref Alterar);
                ePreenchimentoSerie.lstFormasPagamentoDisponiveis = this.PreencherFormasPagamento(SerieID);


                ePreenchimentoSerie.HabilitarEdicao = Alterar;

                return ePreenchimentoSerie;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private List<EstruturaFormaPagamentoSerie> PreencherFormasPagamento(int SerieID)
        {
            try
            {
                List<EstruturaFormaPagamentoSerie> lst = new List<EstruturaFormaPagamentoSerie>();
                string sql =
                    @"SELECT 
                        fps.ID, FormaPagamentoID, fp.Nome as FormaPagamento,
                        b.Nome AS Bandeira, t.Nome as Tipo
                     FROM tFormaPagamento fp (NOLOCK)
                     INNER JOIN tFormaPagamentoSerie fps (NOLOCK) ON fps.FormaPagamentoID = fp.ID
                     INNER JOIN tBandeira b (NOLOCK) ON b.ID = fp.BandeiraID
                     INNER JOIN tFormaPagamentoTipo t (NOLOCK) ON t.ID = fp.FormaPagamentoTipoID
                     WHERE fps.SerieID = " + SerieID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lst.Add(new EstruturaFormaPagamentoSerie()
                    {
                        Acao = Enumerators.TipoAcaoCanal.Manter,
                        Bandeira = bd.LerString("Bandeira"),
                        FormaPagamento = bd.LerString("FormaPagamento"),
                        ID = bd.LerInt("ID"),
                        FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                        Tipo = bd.LerString("Tipo"),
                    });
                }
                return lst;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private List<EstruturaSerieItem> PreencherPrecos(int SerieID, Enumerators.TipoPerfil tipoPerfil, int ID, ref bool Alterar)
        {
            try
            {
                List<EstruturaSerieItem> lstPrecos = new List<EstruturaSerieItem>();


                string continuacao = string.Empty;
                string campos = string.Empty;
                if (tipoPerfil == Enumerators.TipoPerfil.Regional)
                {
                    continuacao = @"INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID
                        INNER JOIN tEmpresa em (NOLOCK) ON em.ID = l.EmpresaID";

                    campos = ", em.RegionalID";
                }
                else
                {
                    continuacao = @"INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID";
                    campos = ", l.ID AS LocalID";
                }


                string sql = string.Format(
                            @"SELECT 
                                    si.ID AS SerieItemID,
                                    e.ID as EventoID,
                                    e.Nome AS Evento,
                                    AP.ID AS ApresentacaoID,
                                    ap.Horario,
                                    s.Nome AS Setor,
                                    s.ID AS SetorID,
                                    p.Nome AS Preco,
                                    p.ID AS PrecoID,
                                    p.Valor AS PrecoValor,
                                    si.Promocional,
                                    si.QuantidadePorPromocional
                                    {0}
                            FROM tSerieItem si (NOLOCK)
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = si.PrecoID
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = si.SetorID
                            INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = si.ApresentacaoID
                            INNER JOIN tEvento e (NOLOCK) ON e.ID = ap.EventoID
                            {1}
                            WHERE si.SerieID = {2}
                            ORDER BY e.Nome, ap.Horario, s.Nome, p.Nome", campos,
                                continuacao, SerieID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lstPrecos.Add(new EstruturaSerieItem()
                    {
                        Acao = Enumerators.TipoAcaoPreco.Manter,
                        SerieItemID = bd.LerInt("SerieItemID"),
                        EventoID = bd.LerInt("EventoID"),
                        Evento = bd.LerString("Evento"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Horario = bd.LerDateTime("Horario").ToString("dddd, dd/MM/yyyy HH:mm"),
                        Setor = bd.LerString("Setor"),
                        SetorID = bd.LerInt("SetorID"),
                        Preco = bd.LerString("Preco"),
                        PrecoID = bd.LerInt("PrecoID"),
                        Valor = bd.LerDecimal("PrecoValor").ToString(),
                        Promocional = bd.LerBoolean("Promocional"),
                        QuantidadePorPromocional = bd.LerInt("QuantidadePorPromocional"),
                    });

                    if (tipoPerfil == Enumerators.TipoPerfil.Regional)
                        Alterar &= bd.LerInt("RegionalID") == ID;
                    else if (tipoPerfil == Enumerators.TipoPerfil.Local)
                        Alterar &= bd.LerInt("LocalID") == ID;

                }

                return lstPrecos;
            }
            finally
            {
                //Não fechar a consulta aqui!!!
                bd.FecharConsulta();
            }
        }

        private EstruturaPreenchimentoSerie PreencherCanaisIR(int SerieID)
        {
            try
            {
                EstruturaPreenchimentoSerie ePreenchimento = new EstruturaPreenchimentoSerie();

                string sql = string.Empty;

                //Consultando Canais IR Distribuídos
                sql = string.Format(@"SELECT 
                                        CASE WHEN cs.SerieID IS NOT NULL 
                                            THEN 'T'
                                            ELSE 'F'
                                        END AS Disponivel,
                                        c.ID AS CanalID
                                    FROM tEmpresa e (NOLOCK)
	                                INNER JOIN tCanal c (NOLOCK) ON e.ID = c.EmpresaID
	                                LEFT JOIN tCanalSerie cs (NOLOCK) ON cs.CanalID = c.ID AND cs.SerieID = {0}
                                    WHERE e.EmpresaVende = 'T' AND e.EmpresaPromove = 'F'", SerieID);


                bd.Consulta(sql);

                int canalID = 0;
                int Internet = Canal.CANAL_INTERNET;
                int Callcenter = Canal.CANAL_CALL_CENTER;
                while (bd.Consulta().Read())
                {
                    canalID = bd.LerInt("CanalID");
                    if (bd.LerBoolean("Disponivel"))
                    {
                        if (canalID == Internet)
                            ePreenchimento.eCanaisIR.DisponivelInternet = true;
                        else if (canalID == Callcenter)
                            ePreenchimento.eCanaisIR.DisponivelCallcenter = true;


                        ePreenchimento.eCanaisIR.QuantidadeDisponivelIR++;

                    }
                    ePreenchimento.eCanaisIR.QuantidadeMaximaIR++;
                }

                return ePreenchimento;
            }
            finally
            {
                //Não precisa fechar o BD aqui!
                bd.FecharConsulta();
            }
        }

        private List<EstruturaCanalSerie> PreencherCanais(int SerieID)
        {
            try
            {
                List<EstruturaCanalSerie> lstCanalSerieItem = new List<EstruturaCanalSerie>();

                string sql = string.Format(@"SELECT 
                                c.ID AS CanalID,
                                cs.ID AS CanalSerieID,
                                r.Nome AS Regional,
                                e.Nome as Empresa,
                                c.Nome as Canal
                            FROM tSerie s (NOLOCK)
                            INNER JOIN tCanalSerie cs (NOLOCK) ON cs.SerieID = s.ID
                            INNER JOIN tCanal c (NOLOCK) ON c.ID = cs.CanalID
                            INNER JOIN tEmpresa e (NOLOCK) ON e.ID = c.EmpresaID
                            INNER JOIN tRegional r (NOLOCK) ON r.ID = e.RegionalID
                            WHERE s.ID = {0} AND e.EmpresaPromove = 'T'
                            ORDER BY r.Nome, e.Nome, c.Nome", SerieID);

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lstCanalSerieItem.Add(new EstruturaCanalSerie()
                    {
                        CanalID = bd.LerInt("CanalID"),
                        CanalSerieID = bd.LerInt("CanalSerieID"),
                        Regional = bd.LerString("Regional"),
                        Empresa = bd.LerString("Empresa"),
                        Canal = bd.LerString("Canal"),
                        Acao = Enumerators.TipoAcaoCanal.Manter,
                    });
                }

                return lstCanalSerieItem;
            }
            finally
            {
                //Não precisa fechar o BD aqui!
                bd.FecharConsulta();
            }
        }

        public void GerarGerenciador(EstruturaSerie eSerie, EstruturaPreenchimentoSerie ePreenchimentoSerie)
        {
            try
            {
                bd.IniciarTransacao();

                this.Nome.Valor = eSerie.Nome;
                this.Titulo.Valor = eSerie.Titulo;
                this.Tipo.Valor = eSerie.Tipo;

                this.QuantidadeMaximaGrupo.Valor = eSerie.QuantidadeMaximaGrupo;
                this.QuantidadeMinimaGrupo.Valor = eSerie.QuantidadeMinimaGrupo;

                this.QuantidadeMaximaApresentacao.Valor = eSerie.QuantidadeMaximaApresentacao;
                this.QuantidadeMinimaApresentacao.Valor = eSerie.QuantidadeMinimaApresentacao;

                this.QuantidadeMinimaIngressosPorApresentacao.Valor = eSerie.QuantidadeMinimaIngressosPorApresentacao;
                this.QuantidadeMaximaIngressosPorApresentacao.Valor = eSerie.QuantidadeMaximaIngressosPorApresentacao;

                this.Regras.Valor = eSerie.Regras;
                this.Descricao.Valor = eSerie.Descricao;
                this.LocalID.Valor = eSerie.LocalID;
                this.RegionalID.Valor = eSerie.RegionalID;
                

                //Registro antigo
                if (eSerie.ID > 0)
                {
                    this.Control.ID = eSerie.ID;
                    if (!this.Atualizar(bd))
                        throw new Exception("Não foi possível atualizar o registro da Série.");
                }
                //Novo Registro
                else
                    if (!this.Inserir(bd))
                        throw new Exception("Não foi possível inserir o registro da Série.");


                SerieItem si = new SerieItem(this.Control.UsuarioID);
                si.GerenciarItems(bd, this.Control.ID, ePreenchimentoSerie.lstPrecosDisponiveis);

                CanalSerie cs = new CanalSerie(this.Control.UsuarioID);
                cs.GerenciarCanais(bd, this.Control.ID, ePreenchimentoSerie.eCanaisIR, ePreenchimentoSerie.lstCanaisDisponiveis);

                FormaPagamentoSerie fps = new FormaPagamentoSerie(this.Control.UsuarioID);
                fps.GerenciarFormasDePagamento(bd, this.Control.ID, ePreenchimentoSerie.lstFormasPagamentoDisponiveis);

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

        public void ExcluirRegistros(int ID)
        {
            try
            {
                CanalSerie cs = new CanalSerie(this.Control.UsuarioID);
                SerieItem si = new SerieItem(this.Control.UsuarioID);
                FormaPagamentoSerie fps = new FormaPagamentoSerie(this.Control.UsuarioID);

                List<int> Canais = new List<int>();
                bd.Consulta("SELECT ID FROM tCanalSerie WHERE SerieID = " + ID);
                while (bd.Consulta().Read())
                    Canais.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                List<int> Items = new List<int>();
                bd.Consulta("SELECT ID FROM tSerieItem WHERE SerieID = " + ID);
                while (bd.Consulta().Read())
                    Items.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                List<int> FormasPagamento = new List<int>();
                bd.Consulta("SELECT ID FROM tFormaPagamentoSerie WHERE SerieID = " + ID);
                while (bd.Consulta().Read())
                    FormasPagamento.Add(bd.LerInt("ID"));

                bd.FecharConsulta();


                bd.IniciarTransacao();

                foreach (int canalSerieID in Canais)
                    cs.Excluir(bd, canalSerieID);

                foreach (int serieItemID in Items)
                    si.Excluir(bd, serieItemID);

                foreach (int formaPagamentoSerieID in FormasPagamento)
                    fps.Excluir(bd, formaPagamentoSerieID);

                this.Excluir(bd, ID);

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

    }

    public class SerieLista : SerieLista_B
    {

        public SerieLista() { }

        public SerieLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
