/**************************************************
* Arquivo: Assinatura.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IRLib
{
    public partial class Assinatura : Assinatura_B
    {
        public enum enumTipoCancelamento
        {
            Bloquear = 'B',
            Disponibilizar = 'D'
        }

        public enumTipoCancelamento Cancelamento
        {
            get
            {

                return Utils.Enums.ParseItem<enumTipoCancelamento>(this.TipoCancelamento.Valor);
            }
        }

        public Assinatura() { }


        public Assinatura(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaIDNome> CarregarAssinaturas(int AssinaturaTipoID, int ano, bool registroZero)
        {

            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                string sql = @"SELECT DISTINCT ass.ID, ass.Nome
                            FROM tAssinatura ass (NOLOCK)
                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
                            WHERE ass.AssinaturaTipoID = " + AssinaturaTipoID + " AND Ano = " + ano + @"
                            ORDER BY ass.Nome";

                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lista.Add(new EstruturaIDNome
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });

                }

                if (registroZero)
                    lista.Insert(0, new EstruturaIDNome { ID = 0, Nome = "-- Selecione --" });
                return lista;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<EstruturaAssinatura> CarregarAssinaturas(int LocalID, int AssinaturaTipoID, string AssinaturaNome, bool somenteAtivas)
        {
            try
            {

                List<EstruturaAssinatura> lista = new List<EstruturaAssinatura>();

                string filtroAux = "";

                filtroAux += "  WHERE tAssinatura.LocalID = " + LocalID;

                if (AssinaturaTipoID > 0)
                {
                    filtroAux += " AND tAssinatura.AssinaturaTipoID= " + AssinaturaTipoID;
                }
                if (AssinaturaNome.Length > 0)
                {
                    filtroAux += " AND tAssinatura.AssinaturaNome = " + AssinaturaNome;
                }
                if (somenteAtivas)
                {
                    filtroAux += " AND tAssinatura.Ativo = 'T' ";
                }

                string sql = @"SELECT	tAssinatura.ID, tAssinatura.Nome,
                        TipoCancelamento =
                              CASE tAssinatura.TipoCancelamento
                                 WHEN 'B' THEN 'Bloquear'
                                 WHEN 'D' THEN 'Disponibilizar'
                                 ELSE ''
                              END,
                        tAssinaturaTipo.Nome as AssinaturaTipo,tAssinatura.Ativo,
                        tLocal.Nome as Local, bl.Nome as Bloqueio, dbl.Nome as DesistenciaBloqueio
                        FROM	tAssinatura
                        INNER JOIN tLocal ON tAssinatura.LocalID = tLocal.ID
                        LEFT JOIN tAssinaturaTipo ON tAssinatura.AssinaturaTipoID = tAssinaturaTipo.ID
                        LEFT JOIN tBloqueio bl ON tAssinatura.BloqueioID = bl.ID
                        LEFT JOIN tBloqueio dbl ON tAssinatura.DesistenciaBloqueioID = dbl.ID"
                   + filtroAux
                   + " ORDER BY tAssinatura.Nome";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lista.Add(new EstruturaAssinatura
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        TipoCancelamento = bd.LerString("TipoCancelamento"),
                        AssinaturaTipo = bd.LerString("AssinaturaTipo"),
                        Ativo = bd.LerBoolean("Ativo"),
                        Local = bd.LerString("Local"),
                        Bloqueio = bd.LerString("Bloqueio"),
                        DesistenciaBloqueio = bd.LerString("DesistenciaBloqueio"),
                    });

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

        public void GerenciarCanais(BD bd, List<EstruturaAssinaturaCanal> lstCanaisDisponiveis)
        {
            AssinaturaCanal oAC = new AssinaturaCanal();

            oAC.Gerenciar(bd, lstCanaisDisponiveis, this.Control.ID);

        }

        public void GerenciarFormaPagamento(BD bd, List<EstruturaAssinaturaFormaPagamento> lstFormasPagamentoDisponiveis)
        {
            AssinaturaFormaPagamento oAFP = new AssinaturaFormaPagamento();

            oAFP.Gerenciar(bd, lstFormasPagamentoDisponiveis, this.Control.ID);

        }

        public int GerenciarAno(BD bd, EstruturaAssinaturaAno eAno)
        {
            try
            {
                int AssinaturaAnoID = 0;
                AssinaturaAno oAA = new AssinaturaAno();

                if (eAno.ID == 0)
                {
                    oAA.Inserir(bd, eAno, this.Control.ID);
                }
                else
                {
                    oAA.Atualizar(bd, eAno, this.Control.ID);
                }

                AssinaturaAnoID = oAA.Control.ID;

                return AssinaturaAnoID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GerenciarItens(BD bd, List<int> ApresentacoesID, List<int> SetoresID, List<int> PrecosID, int AssinaturaAnoID)
        {
            AssinaturaItem oAI = new AssinaturaItem();

            oAI.ExcluirItensAssinatura(bd, AssinaturaAnoID);

            foreach (int apresentacaoID in ApresentacoesID)
            {
                foreach (int setorID in SetoresID)
                {
                    foreach (int precoID in PrecosID)
                    {
                        oAI.Limpar();
                        oAI.ApresentacaoID.Valor = apresentacaoID;
                        oAI.AssinaturaAnoID.Valor = AssinaturaAnoID;
                        oAI.SetorID.Valor = setorID;
                        oAI.PrecoTipoID.Valor = precoID;
                        oAI.Inserir(bd);

                    }

                }

            }
        }

        public List<EstruturaAssinaturaAno> CarregarLstAno()
        {
            try
            {
                List<EstruturaAssinaturaAno> lstRetorno = new List<EstruturaAssinaturaAno>();


                string sql = @"SELECT tAssinaturaAno.ID, 
                            tAssinaturaAno.Ano,
                            tAssinaturaAno.Informacoes
                            FROM tAssinaturaAno
                            WHERE AssinaturaID = " + this.Control.ID + "order by tAssinaturaAno.Ano desc";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lstRetorno.Add(new EstruturaAssinaturaAno
                    {
                        ID = bd.LerInt("ID"),
                        Ano = bd.LerInt("Ano"),
                        AnoInfo = bd.LerString("Informacoes"),

                    });

                }

                bd.Fechar();
                return lstRetorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaFormaPagamento> AssinaturaFormaPagamento(int AssinaturaID)
        {
            try
            {
                List<EstruturaAssinaturaFormaPagamento> lstRetorno = new List<EstruturaAssinaturaFormaPagamento>();


                string sql = @"SELECT tAssinaturaFormaPagamento.ID, tAssinaturaFormaPagamento.FormaPagamentoID,  tFormaPagamento.Nome ,tFormaPagamento.Tipo , tBandeira.Nome as Bandeira
                                FROM tAssinaturaFormaPagamento(NOLOCK)
                                INNER JOIN tFormaPagamento (NOLOCK) ON tAssinaturaFormaPagamento.FormaPagamentoID  = tFormaPagamento.ID
                                LEFT JOIN tBandeira (NOLOCK) ON tFormaPagamento.BandeiraID  = tBandeira.ID
                                WHERE tAssinaturaFormaPagamento.AssinaturaID = " + this.Control.ID + " ORDER BY tAssinaturaFormaPagamento.FormaPagamentoID DESC";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lstRetorno.Add(new EstruturaAssinaturaFormaPagamento()
                        {
                            Acao = Enumerators.TipoAcaoCanal.Manter,
                            Bandeira = bd.LerString("Bandeira"),
                            Tipo = bd.LerString("Tipo"),
                            FormaPagamentoID = bd.LerInt("FormaPagamentoID"),
                            ID = bd.LerInt("ID"),
                            FormaPagamento = bd.LerString("Nome"),
                        });

                }

                bd.Fechar();
                return lstRetorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public List<EstruturaAssinaturaCanal> AssinaturaCanais(int AssinaturaID)
        {
            try
            {
                List<EstruturaAssinaturaCanal> lstRetorno = new List<EstruturaAssinaturaCanal>();


                string sql = @"SELECT tAssinaturaCanal.ID,tAssinaturaCanal.CanalID,tCanal.Nome as CanalNome, tEmpresa.Nome as EmpresaNome, tRegional.Nome as RegionalNome
                                    FROM tAssinaturaCanal (NOLOCK)
                                    INNER JOIN tCanal(NOLOCK) ON tAssinaturaCanal.CanalID = tCanal.ID
                                    LEFT JOIN tEmpresa(NOLOCK) ON tCanal.EmpresaID = tEmpresa.ID
                                    LEFT JOIN tRegional (NOLOCK) ON tEmpresa.RegionalID = tRegional.ID
                                    WHERE tAssinaturaCanal.AssinaturaID = " + this.Control.ID + " ORDER BY tAssinaturaCanal.CanalID DESC";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lstRetorno.Add(new EstruturaAssinaturaCanal()
                    {
                        Acao = Enumerators.TipoAcaoCanal.Manter,
                        Canal = bd.LerString("CanalNome"),
                        CanalID = bd.LerInt("CanalID"),
                        Regional = bd.LerString("RegionalNome") == null ? string.Empty : bd.LerString("RegionalNome"),
                        Empresa = bd.LerString("EmpresaNome"),
                        ID = bd.LerInt("ID"),
                    });

                }

                bd.Fechar();
                return lstRetorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool PossuiClienteAssinaturaAno(int ano, int assinaturaID)
        {

            try
            {
                string sql = @"SELECT TOP 1 tAssinaturaAno.ID 
                                FROM tAssinaturaAno (NOLOCK)
                                INNER JOIN tAssinaturaCliente (NOLOCK) ON tAssinaturaAno.ID = tAssinaturaCliente.AssinaturaAnoID
                                WHERE tAssinaturaAno.Ano = " + ano + " AND tAssinaturaAno.AssinaturaID = " + assinaturaID +
                                " AND tAssinaturaCliente.Acao IN " +
                                @"('" + IRLib.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.Renovar) + "', '" + IRLib.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.EfetivarTroca) + "', '" + IRLib.Utils.Enums.GetChar(AssinaturaCliente.EnumAcao.Aquisicao) + "')";

                return bd.ConsultaValor(sql) != null;
            }
            finally
            {
                bd.Fechar();
            }


        }

        public bool ExcluirAssinatura(int ID)
        {
            bool verifica = false;
            if (!PossuiDadosVinculados(ID))
            {
                verifica = true;
                ExcluirRegistros(ID);

            }
            return verifica;
        }

        private void ExcluirRegistros(int ID)
        {
            try
            {
                Assinatura oAss = new Assinatura(this.Control.UsuarioID);
                AssinaturaAno oAssAno = new AssinaturaAno(this.Control.UsuarioID);
                AssinaturaFormaPagamento oAssFP = new AssinaturaFormaPagamento(this.Control.UsuarioID);
                AssinaturaCanal oAssC = new AssinaturaCanal(this.Control.UsuarioID);
                AssinaturaItem oAssI = new AssinaturaItem(this.Control.UsuarioID);

                List<int> lstAno = new List<int>();
                bd.Consulta("SELECT ID FROM tAssinaturaAno WHERE AssinaturaID = " + ID);
                while (bd.Consulta().Read())
                    lstAno.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                List<int> lstFP = new List<int>();
                bd.Consulta("SELECT ID FROM tAssinaturaFormaPagamento WHERE AssinaturaID = " + ID);
                while (bd.Consulta().Read())
                    lstFP.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                List<int> lstC = new List<int>();
                bd.Consulta("SELECT ID FROM tAssinaturaCanal WHERE AssinaturaID = " + ID);
                while (bd.Consulta().Read())
                    lstC.Add(bd.LerInt("ID"));

                bd.FecharConsulta();

                List<int> lstI = new List<int>();
                bd.Consulta(@"SELECT tAssinaturaItem.ID 
                                FROM tAssinaturaItem (NOLOCK)
                                INNER JOIN tAssinaturaAno (NOLOCK) ON tAssinaturaAno.ID = tAssinaturaItem.AssinaturaAnoID
                                WHERE tAssinaturaAno.AssinaturaID =  " + ID);
                while (bd.Consulta().Read())
                    lstI.Add(bd.LerInt("ID"));

                bd.FecharConsulta();


                bd.IniciarTransacao();

                foreach (int AnoID in lstAno)
                    oAssAno.Excluir(bd, AnoID);

                foreach (int AssFPID in lstFP)
                    oAssFP.Excluir(bd, AssFPID);

                foreach (int CanalID in lstC)
                    oAssC.Excluir(bd, CanalID);

                foreach (int ItemID in lstI)
                    oAssI.Excluir(bd, ItemID);

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

        public void DesativarAssinatura(int ID)
        {
            this.Ler(ID);
            this.Ativo.Valor = false;
            this.Atualizar();
        }

        private bool PossuiDadosVinculados(int ID)
        {
            try
            {
                bool retorno = false;


                string sql = @"SELECT TOP 1 tAssinaturaCliente.ID 
                                FROM tAssinaturaCliente (NOLOCK) 
                                WHERE tAssinaturaCliente.AssinaturaID = " + ID;


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    retorno = true;
                }

                bd.Fechar();
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<Assinaturas.Models.NovasAquisicoes> BuscarAssinaturasAtivas(int assinaturaTipoID,
            int ano, bool adquirirForaPeriodo, List<int> eleazarAssinaturaID, EstruturaReservaInternet estruturaIdentificacaoUsuario, bool trocando)
        {
            try
            {
                string foraDePeriodo = string.Empty;
                if (eleazarAssinaturaID != null && eleazarAssinaturaID.Count > 0)
                    if (adquirirForaPeriodo)
                        foraDePeriodo = "AND a.ID IN (" + CTLib.Utilitario.VetorInteiroParaString(eleazarAssinaturaID.ToArray()) + ")";
                    else
                        foraDePeriodo = "AND a.ID NOT IN (" + CTLib.Utilitario.VetorInteiroParaString(eleazarAssinaturaID.ToArray()) + ")";


                string sql =
                        string.Format(@" SELECT a.ID, a.Nome 
                        FROM tAssinatura a (NOLOCK) 
                        INNER JOIN tAssinaturaCanal ac (NOLOCK) ON ac.AssinaturaID = a.ID
                        INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                        WHERE a.Ativo = 'T' AND an.Ano = '{0}' AND  AssinaturaTipoID = {1} AND ac.CanalID = {2} {3} ORDER BY a.Nome ",
                        ano, assinaturaTipoID, estruturaIdentificacaoUsuario.CanalID, foraDePeriodo);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem assinaturas ativas para este ano.");

                List<Assinaturas.Models.NovasAquisicoes> lista = new List<Assinaturas.Models.NovasAquisicoes>();
                do
                {
                    lista.Add(new Assinaturas.Models.NovasAquisicoes()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Gerenciar(int anoID, List<EstruturaAssinaturaAno> lstAno, List<EstruturaAssinaturaFormaPagamento> lstFormasPagamentoDisponiveis, List<EstruturaAssinaturaCanal> lstCanaisDisponiveis, List<int> ApresentacoesID, List<int> SetoresID, List<int> PrecosID)
        {
            try
            {
                bd.IniciarTransacao();

                if (this.Control.ID == 0)
                    this.Inserir(bd);
                else
                    this.Atualizar(bd);

                EstruturaAssinaturaAno eAno = lstAno.Find(delegate(EstruturaAssinaturaAno item)
                {
                    return item.Ano == Convert.ToInt32(anoID);
                });

                int AssinaturaAnoID = this.GerenciarAno(bd, eAno);

                this.GerenciarItens(bd, ApresentacoesID, SetoresID, PrecosID, AssinaturaAnoID);

                this.GerenciarFormaPagamento(bd, lstFormasPagamentoDisponiveis);

                this.GerenciarCanais(bd, lstCanaisDisponiveis);

                bd.FinalizarTransacao();
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


        public string GetNomeImg(int AssinaturaID)
        {
            string nomeImagem = "ap" + AssinaturaID.ToString("000000") + ".gif";
            return nomeImagem;
        }

        public List<EstruturaIDNome> BuscarAssinaturas(int assinaturaTipoID, int ano)
        {
            return BuscarAssinaturas(assinaturaTipoID, ano, true);
        }

        public List<EstruturaIDNome> BuscarAssinaturas(int assinaturaTipoID, int ano, bool todas)
        {
            try
            {
                string sql =
                    string.Format(@"
                    SELECT DISTINCT
                        a.ID, a.Nome
                    FROM tAssinatura a (NOLOCK)
                    INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                    WHERE a.AssinaturaTipoID = {0} {1}
                    ORDER BY a.Nome
                    ", assinaturaTipoID, ano > 0 ? " AND an.Ano = '" + ano + "'" : string.Empty);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo e ano selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                if (todas)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todas" });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarCanaisAssinaturas(int assinaturaTipoID, bool todas)
        {
            try
            {
                string sql =
                    string.Format(@"
                    select cn.ID, cn.Nome 
                    from tAssinatura a
                    inner join tLocal l on a.LocalID = l.ID
                    inner join tEmpresa e on l.EmpresaID = e.ID
                    inner join tCanal cn on cn.EmpresaID = e.ID
                    where a.AssinaturaTipoID = {0}
                    group by cn.ID, cn.Nome 
                    order by cn.Nome
                    ", assinaturaTipoID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                if (todas)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todas" });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarLojasAssinaturas(int assinaturaTipoID, bool todas)
        {
            try
            {
                string sql =
                    string.Format(@"
                    select lj.ID,lj.Nome 
                    from tAssinatura a
                    inner join tLocal l on a.LocalID = l.ID
                    inner join tEmpresa e on l.EmpresaID = e.ID
                    inner join tCanal cn on cn.EmpresaID = e.ID
                    inner join tLoja lj on lj.CanalID = cn.ID
                    where a.AssinaturaTipoID = {0}
                    group by lj.ID,lj.Nome 
                    order by lj.Nome
                    ", assinaturaTipoID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                if (todas)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todas" });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarSetoresAssinaturas(int assinaturaTipoID, bool todas)
        {
            try
            {
                string sql =
                    string.Format(@"
                    select s.ID,s.Nome 
                    from tAssinatura a
                    inner join tLocal l on a.LocalID = l.ID
                    inner join tSetor s on l.ID = s.LocalID
                    where a.AssinaturaTipoID = {0}
                    group by s.ID,s.Nome 
                    order by s.Nome
                    ", assinaturaTipoID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                if (todas)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todos" });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public String BuscarFiltrosCaixaResumo(int Temporadas, int Assinaturas, int Canais, int Lojas, int Setores, string DataInicial, string DataFinal)
        {
            try
            {
                string retorno = "Temporada: " + Temporadas;

                string sql = "";

                if (Assinaturas > 0)
                {
                    sql = string.Format(@"
                    select a.Nome 
                    from tAssinatura a
                    where a.ID = {0}   
                    ", Assinaturas);


                    if (bd.Consulta(sql).Read())
                    {
                        retorno += " Assinatura: " + bd.LerString("Nome");
                    }
                }

                if (Canais > 0)
                {
                    sql =
                       string.Format(@"
                    select c.Nome 
                    from tCanal c
                    where c.ID = {0}   
                    ", Canais);

                    if (bd.Consulta(sql).Read())
                    {
                        retorno += " Canal: " + bd.LerString("Nome");
                    }
                }


                if (Lojas > 0)
                {
                    sql =
                    string.Format(@"
                    select l.Nome 
                    from tLoja l
                    where l.ID = {0}   
                    ", Lojas);

                    if (bd.Consulta(sql).Read())
                    {
                        retorno += " Loja: " + bd.LerString("Nome");
                    }
                }

                if (Setores > 0)
                {
                    sql =
                    string.Format(@"
                    select s.Nome 
                    from tSetor s
                    where s.ID = {0}   
                    ", Setores);

                    if (bd.Consulta(sql).Read())
                    {
                        retorno += " Setor: " + bd.LerString("Nome");
                    }
                }

                if (DataInicial.Length > 0)
                {


                    retorno += " Periodo de: " + DataInicial;

                }

                if (DataFinal.Length > 0)
                {

                    retorno += " até: " + DataFinal;

                }




                return retorno;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaValores> BuscaValores(int assinaturaID, int temporada)
        {
            try
            {
                string sql =
                    string.Format(@"select Setor,Qtdade, Preco,Valor FROM vwAssinaturaPagamento as vv
                    INNER JOIN tAssinaturaAno (nolock) on tAssinaturaAno.ID = AssinaturaAnoID
                    WHERE vv.AssinaturaID = {0} AND tAssinaturaAno.Ano = '{1}'
                    ", assinaturaID, temporada);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo e ano selecionado.");

                List<EstruturaAssinaturaValores> lista = new List<EstruturaAssinaturaValores>();
                do
                {
                    lista.Add(new EstruturaAssinaturaValores()
                    {
                        Setor = bd.LerString("Setor"),
                        Quantidade = bd.LerInt("Qtdade"),
                        PrecoTipo = bd.LerString("Preco"),
                        Preco = bd.LerDecimal("Valor")
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaValores> BuscaValoresBuffer(List<int> ApresentacoesID, List<int> SetoresID, List<int> PrecosID)
        {
            try
            {
                DataTable dtt = new DataTable();
                dtt.Columns.Add("ApresentacaoID", typeof(int));
                dtt.Columns.Add("SetorID", typeof(int));
                dtt.Columns.Add("PrecoTipoID", typeof(int));

                DataRow dtr;
                foreach (int apresentacaoID in ApresentacoesID)
                {
                    foreach (int setorID in SetoresID)
                    {
                        foreach (int precoID in PrecosID)
                        {
                            dtr = dtt.NewRow();
                            dtr["ApresentacaoID"] = apresentacaoID;
                            dtr["SetorID"] = setorID;
                            dtr["PrecoTipoID"] = precoID;
                            dtt.Rows.Add(dtr);
                        }
                    }
                }

                bd.Executar(string.Format("CREATE TABLE #tempAssinatura ( ApresentacaoID INT, SetorID INT, PrecoTipoID INT ) "));

                SqlBulkCopy bulkCopy;
                bulkCopy = new SqlBulkCopy((SqlConnection)bd.Cnn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = "#tempAssinatura";
                bulkCopy.WriteToServer(dtt);



                string sql =
                    string.Format(@"SELECT     
                        tSetor.ID AS SetorID, tPrecoTipo.ID AS PrecoTipoID, tPrecoTipo.Nome AS Preco, SUM(tPreco.Valor) As Valor, 
                        COUNT(temp.ApresentacaoID) AS Qtdade, tSetor.Nome AS Setor, tSetor.LocalID AS LocalID
                        FROM #tempAssinatura temp (NOLOCK),tSetor(NOLOCK), tPrecoTipo(NOLOCK), tPreco(NOLOCK), tApresentacaoSetor(NOLOCK)    
                        WHERE     
                        tPrecoTipo.ID = temp.PrecoTipoID AND 
                        temp.SetorID = tSetor.ID AND     
                        tApresentacaoSetor.ApresentacaoID = temp.ApresentacaoID AND 
                        tApresentacaoSetor.SetorID = temp.SetorID AND     
                        tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID AND 
                        tPreco.PrecoTipoID = tPrecoTipo.ID  
                        GROUP BY 
                        tSetor.ID , tPrecoTipo.ID , tPrecoTipo.Nome, tSetor.Nome, tSetor.LocalID
                        ORDER BY tSetor.Nome, tPrecoTipo.Nome
                    ");

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo e ano selecionado.");

                List<EstruturaAssinaturaValores> lista = new List<EstruturaAssinaturaValores>();
                do
                {
                    lista.Add(new EstruturaAssinaturaValores()
                    {
                        Setor = bd.LerString("Setor"),
                        Quantidade = bd.LerInt("Qtdade"),
                        PrecoTipo = bd.LerString("Preco"),
                        Preco = bd.LerDecimal("Valor")
                    });
                } while (bd.Consulta().Read());


                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public int getApresentacao(int Assinaturas, int Temporadas)
        {
            try
            {
                int retorno = 0;


                string sql = @"SELECT
                        DISTINCT ApresentacaoID
                    FROM tAssinatura a (NOLOCK) 
                    INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                    INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
                    WHERE a.ID = " + Assinaturas + " AND an.Ano = '" + Temporadas + "' AND a.Ativo = 'T' ";


                bd.Consulta(sql);


                if (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("ApresentacaoID");
                }

                bd.Fechar();
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void CarregarNome(int id)
        {
            try
            {

                string sql = "SELECT Nome FROM tAssinatura WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.Nome.ValorBD = bd.LerString("Nome");

                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string GetLocal(int id)
        {
            try
            {
                string retorno = "";

                string sql = @"select l.Nome as Local 
                                from tAssinatura a 
                                inner join tLocal l on a.LocalID = l.ID
                                where a.ID = " + id;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = bd.LerString("Local");
                }

                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string BuscarAlertaAssinante(int clienteID)
        {
            try
            {
                string retorno = "";

                string sql = @"select Distinct(a.AlertaAssinante) as Alerta
                                from tAssinaturaCliente ac(NOLOCK) 
                                inner join tAssinatura a (NOLOCK) on a.ID = ac.AssinaturaID
                                where a.AlertaAssinante is not null and a.AlertaAssinante <> '' and ac.ClienteID =  " + clienteID;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno += bd.LerString("Alerta");
                    retorno += " ;";
                }

                if (retorno.Length > 0)
                {
                    retorno = retorno.Remove(retorno.Length - 2);
                }

                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarApresentacoesAssinaturas(int assinaturaTipoID, int ano, bool selecione)
        {
            try
            {
                string sql =
                    string.Format(@"
                    SELECT DISTINCT ap.ID,ap.Horario 
                    FROM tApresentacao ap
                    INNER JOIN tAssinaturaItem ai ON ai.ApresentacaoID = ap.ID
                    INNER JOIN tAssinaturaAno aa ON aa.ID = ai.AssinaturaAnoID
                    INNER JOIN tAssinatura  a ON aa.assinaturaid = a.id
                    WHERE ap.DisponivelAjuste = 'T' AND 
                    a.AssinaturaTipoID = {0} AND aa.ano = {1}
                    ORDER BY ap.Horario", assinaturaTipoID, ano);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo e ano selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerDateTime("Horario").ToString("dd/MM/yyyy HH:mm"),
                    });
                } while (bd.Consulta().Read());

                if (selecione)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaIDNome> BuscarEventosAssinaturas(int assinaturaTipoID, int ano, bool selecione)
        {
            try
            {
                string sql =
                     string.Format(@"
                    SELECT DISTINCT e.ID,e.Nome FROM tEvento e
                    INNER JOIN tApresentacao ap ON ap.EventoID = e.ID   
                    INNER JOIN tAssinaturaItem ai ON ai.ApresentacaoID = ap.ID
                    INNER JOIN tAssinaturaAno aa ON aa.ID = ai.AssinaturaAnoID
                    INNER JOIN tAssinatura  a ON aa.assinaturaid = a.id
                    WHERE ap.DisponivelVenda = 'T' AND  
                    a.AssinaturaTipoID = {0} AND aa.ano = {1}
                    ORDER BY e.Nome", assinaturaTipoID, ano);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem assinaturas ativas para o tipo e ano selecionado.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                if (selecione)
                    lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                return lista;

            }
            finally
            {
                bd.Fechar();
            }
        }

        public int[] LimpaReservasAssinatura()
        {
            try
            {
                int delayReserva = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayReserva"]);
                ArrayList ids = new ArrayList();
                DateTime data = DateTime.Now;

                data = data.AddMinutes(-delayReserva);

                bd.FecharConsulta();

                var sqlConsultaIngresso = @"SELECT ID, ApresentacaoID, Status, CotaItemID FROM tIngresso ( NOLOCK ) 
                                            WHERE @data > TimeStampReserva AND TimeStampReserva IS NOT NULL AND TimeStampReserva <> '' AND Status = @status AND AssinaturaClienteID > 0
                                           ";

                var parametrosConsultaIngresso = new List<SqlParameter>()
                {
                    new SqlParameter("data", data.ToString("yyyyMMddHHmmss")),
                    new SqlParameter("status", (char)Ingresso.StatusIngresso.RESERVADO)
                };

                bd.Consulta(sqlConsultaIngresso, parametrosConsultaIngresso);

                var bdUpdate = new BD();

                while (bd.Consulta().Read())
                {
                    var ingressoId = bd.LerInt("ID");
                    var apresentacaoId = bd.LerInt("ApresentacaoID");
                    var cotaItemId = bd.LerInt("CotaItemID");

                    var sqlUpdateIngresso = @"UPDATE tIngresso 
                                              SET AssinaturaClienteID = 0, TimeStampReserva = '', ClienteID = 0, Status = @statusNovo, CotaItemID = NULL
                                              WHERE Status = @statusAntigo AND ID = @ingressoId
                                             ";

                    var parametrosUpdateIngresso = new List<SqlParameter>()
                    {
                        new SqlParameter("statusNovo", (char) Ingresso.StatusIngresso.DISPONIVEL),
                        new SqlParameter("statusAntigo", (char) Ingresso.StatusIngresso.RESERVADO),
                        new SqlParameter("ingressoId", ingressoId)
                    };

                    var sucesso = bdUpdate.Executar(sqlUpdateIngresso, parametrosUpdateIngresso) > 0;
                    
                    if (!sucesso) continue;

                    if (apresentacaoId != 0 && cotaItemId != 0)
                    {
                        var cotaItemControle = new CotaItemControle();
                        cotaItemControle.DecrementarControladorApresentacao(apresentacaoId, cotaItemId);
                    }

                    ids.Add(bd.LerInt("ID"));
                }

                return (int[])ids.ToArray(typeof(int));
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.DesfazerTransacao();
                bd.Fechar();
            }
        }

    }
    public class AssinaturaLista : AssinaturaLista_B
    {

        public AssinaturaLista() { }

        public AssinaturaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
