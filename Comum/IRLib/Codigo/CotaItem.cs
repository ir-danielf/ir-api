/**************************************************
* Arquivo: CotaItem.cs
* Gerado: 14/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IRLib
{

    public class CotaItem : CotaItem_B
    {
        public enum EnumRetornoTipo
        {
            Ok = 0,
            Cliente = 1,
            ClienteNaoExiste = 2,
            ClienteInvalido = 3,
            Quantidade = 4,
            QuantidadeCliente = 5,
            Codigo = 6,
            Ingresso = 7,
            ClienteIgnorar = 8,
        }

        public CotaItem() { }

        public CotaItem(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void SalvarItem(BD bd, List<EstruturaCotaItem> lista, int cotaID, bool gerarControle)
        {
            try
            {
                CotaItemFormaPagamento oCotaItemFormaPagamento = new CotaItemFormaPagamento();
                CotaItemControle oCotaItemControle = new CotaItemControle();
                Obrigatoriedade oObrigatoriedade = new Obrigatoriedade();

                for (int i = 0; i < lista.Count; i++)
                {
                    this.Limpar();

                    if (lista[i].Novo && !lista[i].Excluir)
                    {
                        oObrigatoriedade.Limpar();
                        int ObrigatoriedadeID = 0;

                        if (lista[i].obrigatoriedade != null)
                        {
                            #region Preenche o Obj obrigatoriedade


                            oObrigatoriedade.Nome.Valor = lista[i].obrigatoriedade.Nome;
                            oObrigatoriedade.RG.Valor = lista[i].obrigatoriedade.RG;
                            oObrigatoriedade.CPF.Valor = lista[i].obrigatoriedade.CPF;
                            oObrigatoriedade.Telefone.Valor = lista[i].obrigatoriedade.Telefone;
                            oObrigatoriedade.DataNascimento.Valor = lista[i].obrigatoriedade.DataNascimento;
                            oObrigatoriedade.Email.Valor = lista[i].obrigatoriedade.Email;
                            oObrigatoriedade.CPFResponsavel.Valor = lista[i].obrigatoriedade.CPFResponsavel;
                            oObrigatoriedade.NomeResponsavel.Valor = lista[i].obrigatoriedade.NomeResponsavel;

                            #endregion
                            oObrigatoriedade.Inserir(bd);
                            ObrigatoriedadeID = oObrigatoriedade.Control.ID;
                        }


                        this.PrecoIniciaCom.Valor = lista[i].precoIniciaCom;
                        this.Quantidade.Valor = lista[i].quantidade;
                        this.QuantidadePorCliente.Valor = lista[i].quantidadePorCliente;
                        this.ParceiroID.Valor = lista[i].parceiroID;
                        this.ValidaBin.Valor = lista[i].validaBin;
                        this.ObrigatoriedadeID.Valor = ObrigatoriedadeID;
                        this.CotaID.Valor = cotaID;
                        this.Tipo.Valor = lista[i].Tipo;
                        this.TextoValidacao.Valor = lista[i].textoValidacao;

                        this.TermoSite.Valor = lista[i].TermoSite;
                        this.Termo.Valor = lista[i].Termo;
                        this.TermoSite.Valor = lista[i].TermoSite;
                        this.CPFResponsavel.Valor = lista[i].CPFResponsavel;
                        this.Nominal.Valor = lista[i].Nominal;
                        this.QuantidadePorCodigo.Valor = lista[i].QuantidadePorCodigo;
                        this.Inserir(bd);

                        for (int w = 0; w < lista[i].FormaPagamentoInserir.Count; w++)
                        {
                            oCotaItemFormaPagamento.Limpar();
                            oCotaItemFormaPagamento.CotaItemID.Valor = this.Control.ID;
                            oCotaItemFormaPagamento.FormaPagamentoID.Valor = lista[i].FormaPagamentoInserir[w].FormaPagamentoID;
                            oCotaItemFormaPagamento.Inserir();
                        }

                        //Gera um novo Controle de Quantidade do CotaItem
                        if (gerarControle)
                        {
                            oCotaItemControle.CotaItemID.Valor = this.Control.ID;
                            oCotaItemControle.GerarControladorDeCotaAntiga(bd, cotaID, lista[i].precoIniciaCom);
                        }

                    }
                    else if (!lista[i].Novo && !lista[i].Excluir)
                    {
                        if (lista[i].obrigatoriedade != null && lista[i].obrigatoriedade.Mudou)
                        {
                            oObrigatoriedade.Limpar();
                            #region Preenche o Obj obrigatoriedade
                            oObrigatoriedade.Control.ID = lista[i].obrigatoriedadeID;
                            oObrigatoriedade.Nome.Valor = lista[i].obrigatoriedade.Nome;
                            oObrigatoriedade.RG.Valor = lista[i].obrigatoriedade.RG;
                            oObrigatoriedade.CPF.Valor = lista[i].obrigatoriedade.CPF;
                            oObrigatoriedade.Telefone.Valor = lista[i].obrigatoriedade.Telefone;

                            oObrigatoriedade.DataNascimento.Valor = lista[i].obrigatoriedade.DataNascimento;
                            oObrigatoriedade.Email.Valor = lista[i].obrigatoriedade.Email;

                            oObrigatoriedade.CPFResponsavel.Valor = lista[i].obrigatoriedade.CPFResponsavel;
                            oObrigatoriedade.NomeResponsavel.Valor = lista[i].obrigatoriedade.NomeResponsavel;

                            #endregion
                            oObrigatoriedade.Atualizar(bd);
                        }

                        this.Control.ID = lista[i].ID;
                        this.PrecoIniciaCom.Valor = lista[i].precoIniciaCom;
                        this.Quantidade.Valor = lista[i].quantidade;
                        this.QuantidadePorCliente.Valor = lista[i].quantidadePorCliente;
                        this.ParceiroID.Valor = lista[i].parceiroID;
                        this.ValidaBin.Valor = lista[i].validaBin;
                        this.ObrigatoriedadeID.Valor = lista[i].obrigatoriedadeID;
                        this.CotaID.Valor = cotaID;
                        this.Tipo.Valor = lista[i].Tipo;
                        this.TextoValidacao.Valor = lista[i].textoValidacao;
                        this.Termo.Valor = lista[i].Termo;
                        this.TermoSite.Valor = lista[i].TermoSite;
                        this.CPFResponsavel.Valor = lista[i].CPFResponsavel;
                        this.Nominal.Valor = lista[i].Nominal;
                        this.QuantidadePorCodigo.Valor = lista[i].QuantidadePorCodigo;
                        this.Atualizar(bd);

                        //Inclui novas formas de pagamento
                        for (int w = 0; w < lista[i].FormaPagamentoInserir.Count; w++)
                        {
                            oCotaItemFormaPagamento.Limpar();
                            oCotaItemFormaPagamento.CotaItemID.Valor = this.Control.ID;
                            oCotaItemFormaPagamento.FormaPagamentoID.Valor = lista[i].FormaPagamentoInserir[w].FormaPagamentoID;
                            oCotaItemFormaPagamento.Inserir();
                        }
                        //Excluir formas de pagamento
                        for (int w = 0; w < lista[i].FormaPagamentoExcluir.Count; w++)
                        {
                            oCotaItemFormaPagamento.Limpar();
                            oCotaItemFormaPagamento.ExcluirPorIDs(bd, lista[i].FormaPagamentoExcluir[w].FormaPagamentoID, lista[i].ID);
                        }
                    }
                    else if (!lista[i].Novo)
                    {
                        oObrigatoriedade.Limpar();
                        oObrigatoriedade.Excluir(bd, lista[i].obrigatoriedadeID);

                        oCotaItemFormaPagamento.Limpar();
                        oCotaItemFormaPagamento.ExcluirPorCotaItemID(bd, lista[i].ID);

                        //Excluir o Controlador de Quantidade já vendidas
                        oCotaItemControle.CotaItemID.Valor = lista[i].ID;
                        oCotaItemControle.ExcluirControladorPorCotaItemID(bd);

                        this.Excluir(bd, lista[i].ID);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCotaID(int apresentacaoID)
        {
            try
            {
                string sql = @"SELECT ta.CotaID FROM dbo.tApresentacao ta WHERE ta.ID = " + apresentacaoID;
                int CotaID = 0;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    CotaID = bd.LerInt("CotaID");
                }

                return CotaID;
            }
            catch (Exception ex)
            {
                string MessageErro = ex.Message;
                return 0;
            }
            finally
            {
                bd.FecharConsulta();

            }

        }

        public string CarregarNomesPrecos(int precoID)
        {
            try
            {
                string NomePreco = string.Empty;
                string sql = "SELECT tp.Nome FROM tPreco tp WHERE tp.ID = " + precoID.ToString();
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    NomePreco = bd.LerString("Nome");
                }

                return NomePreco;
              
            }
            catch (Exception ex)
            {
                string messagemErro = ex.Message;
                return string.Empty;
            }
            finally
            {
                bd.FecharConsulta();
            }
           
        }

        public bool ExcluiCotaItem(int IdObrigatoriedade, int IdCota)
        {
            if (IdObrigatoriedade > 0 && IdCota > 0)
            {
                CotaItemFormaPagamento oCotaItemFormaPagamento = new CotaItemFormaPagamento();
                CotaItemControle oCotaItemControle = new CotaItemControle();
                Obrigatoriedade oObrigatoriedade = new Obrigatoriedade();

                oObrigatoriedade.Limpar();
                oObrigatoriedade.Excluir(bd, IdObrigatoriedade);

                oCotaItemFormaPagamento.Limpar();
                oCotaItemFormaPagamento.ExcluirPorCotaItemID(bd, IdCota);

                //Excluir o Controlador de Quantidade já vendidas
                oCotaItemControle.CotaItemID.Valor = IdCota;
                oCotaItemControle.ExcluirControladorPorCotaItemID(bd);

                return this.Excluir(bd, IdCota);
            }
            else
                return false;
        }

        public List<EstruturaCotaItemReserva> getListaCotaItemReserva(int cotaID, int cotaIDAPS)
        {
            try
            {
                List<EstruturaCotaItemReserva> lista = new List<EstruturaCotaItemReserva>();
                EstruturaCotaItemReserva item;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID, CotaID, PrecoIniciaCom, Quantidade, QuantidadePorCliente, ValidaBin,ParceiroID, Nominal ");
                stbSQL.Append("FROM tCotaItem (NOLOCK) ");
                stbSQL.Append("WHERE CotaID IN (" + cotaID + ", " + cotaIDAPS + ") ");
                stbSQL.Append("ORDER BY PrecoIniciaCom");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    item = new EstruturaCotaItemReserva();
                    item.ID = bd.LerInt("ID");
                    item.PrecoIniciaCom = bd.LerString("PrecoIniciaCom");
                    item.Quantidade = bd.LerInt("Quantidade");
                    item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                    item.ValidaBin = bd.LerBoolean("ValidaBin");
                    item.ParceiroID = bd.LerInt("ParceiroID");
                    item.Nominal = bd.LerBoolean("Nominal");

                    if (bd.LerInt("CotaID") == cotaID)
                        item.isApresentacao = true;

                    lista.Add(item);
                }
                return lista;


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

        public List<int> getQuantidadeQPodeReservarCota(List<EstruturaCotaItemReserva> listaCotaItem, EstruturaPrecoReservaSite preco, EstruturaCotasInfo cotaInfo)
        {
            try
            {
                List<int> retorno = new List<int>();

                #region Encontra Item da Apresentacao
                foreach (EstruturaCotaItemReserva itemEncontrado in listaCotaItem.Where(c => preco.PrecoNome.ToLower().StartsWith(c.PrecoIniciaCom.ToLower()) && c.isApresentacao))
                {
                    retorno.Add(itemEncontrado.ID);
                    cotaInfo.CotaItemID = itemEncontrado.ID;
                    cotaInfo.EncontrouCotaItem = true;
                    cotaInfo.QuantidadeApresentacao = itemEncontrado.QuantidadeApresentacao;
                    cotaInfo.QuantidadePorClienteCota = itemEncontrado.QuantidadePorCliente;
                    cotaInfo.QuantidadeCota = itemEncontrado.Quantidade;
                    cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                    cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                    cotaInfo.Nominal = itemEncontrado.Nominal;
                    break;
                }

                if (retorno.Count.Equals(0))
                    retorno.Add(0);

                #endregion

                #region Encontra Item da Apresentacao Setor
                foreach (EstruturaCotaItemReserva itemEncontrado in listaCotaItem.Where(c => preco.PrecoNome.ToLower().StartsWith(c.PrecoIniciaCom.ToLower()) && !c.isApresentacao))
                {
                    retorno.Add(itemEncontrado.ID);
                    cotaInfo.CotaItemID_APS = itemEncontrado.ID;
                    cotaInfo.EncontrouCotaItemAPS = true;
                    cotaInfo.QuantidadeCotaAPS = itemEncontrado.Quantidade;
                    cotaInfo.QuantidadePorClienteCotaAPS = itemEncontrado.QuantidadePorCliente;
                    cotaInfo.ValidaBinAPS = itemEncontrado.ValidaBin;
                    cotaInfo.ParceiroIDAPS = itemEncontrado.ParceiroID;
                    cotaInfo.Nominal = itemEncontrado.Nominal;
                    break;
                }
                if (retorno.Count.Equals(1))
                    retorno.Add(0);

                #endregion


                if (cotaInfo.CotaItemID > 0 && cotaInfo.CotaItemID_APS > 0 && cotaInfo.CotaItemID_APS != cotaInfo.CotaItemID
                        && (cotaInfo.ValidaBin != cotaInfo.ValidaBinAPS || cotaInfo.ParceiroID != cotaInfo.ParceiroIDAPS))
                    throw new BilheteriaException("Não será possivel reservar o(s) pacote(s), pois ele(s) não está(ão) mais disponível(is).");


                //Validacao de Cotas
                if (cotaInfo.CotaItemID > 0 || cotaInfo.CotaItemID_APS > 0)
                {
                    cotaInfo.QuantidadeJaVendida = new CotaItemControle().getQuantidadeNovo(cotaInfo.CotaItemID, cotaInfo.CotaItemID_APS, cotaInfo.ApresentacaoID, cotaInfo.ApresentacaoSetorID);
                    //cotaInfo.sumQuantidadeVendidaReservar(qtdeJaReservadaCota, qtdeJaReservadaCotaItem);

                    //Fez a soma dos itens agora faz a validacao, se estiver incorreto retorna -1 na lista
                    if (!cotaInfo.ValidaReserva(preco.QuantidadeReservar()))
                        for (int i = 0; i < retorno.Count; i++)
                            retorno[i] = -1;
                }
                retorno.Add(cotaInfo.Nominal ? 1 : 0);
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstruturaCotaItemReserva getCotaItemPorID(int cotaItemID, int apresentacaoID, int setorID)
        {
            try
            {
                EstruturaCotaItemReserva Item = new EstruturaCotaItemReserva();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT tApresentacao.ID AS ApresentacaoID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.ParceiroID, it.Nominal, it.ValidaBin, it.QuantidadePorCodigo, ");
                stbSQL.Append("IsNull(tApresentacao.Quantidade, 0) AS QuantidadeApresentacao, IsNull(tApresentacao.QuantidadePorCliente, 0) AS QuantidadePorClienteApresentacao, tApresentacaoSetor.ID AS ApresentacaoSetorID ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("INNER JOIN tApresentacao (NOLOCK) ON  tApresentacao.CotaID = it.CotaID ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID ");
                stbSQL.Append("WHERE it.ID =" + cotaItemID + " AND tApresentacaoSetor.ApresentacaoID = " + apresentacaoID + " AND tApresentacaoSetor.SetorID = " + setorID);

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Item.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    Item.ID = cotaItemID;
                    Item.PrecoIniciaCom = bd.LerString("PrecoIniciaCom");
                    Item.Quantidade = bd.LerInt("Quantidade");
                    Item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                    Item.ParceiroID = bd.LerInt("ParceiroID");
                    Item.ValidaBin = bd.LerBoolean("ValidaBin");
                    Item.QuantidadeApresentacao = bd.LerInt("QuantidadeApresentacao");
                    Item.QuantidadePorClienteApresentacao = bd.LerInt("QuantidadePorClienteApresentacao");
                    Item.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                    Item.QuantidadePorCodigo = bd.LerInt("QuantidadePorCodigo");
                    Item.Nominal = bd.LerBoolean("Nominal");
                    Item.isApresentacao = true;
                }
                return Item;
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

        public EstruturaCotaItemReserva getCotaItemPorID(int cotaItemID, int apresentacaoID, int setorID, int apresentacaoSetorID)
        {
            try
            {
                EstruturaCotaItemReserva Item = new EstruturaCotaItemReserva();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT aps.SetorID, aps.ApresentacaoID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.Nominal, it.ParceiroID, it.ValidaBin, it.QuantidadePorCodigo,  ");
                stbSQL.Append("IsNull(aps.Quantidade, 0) AS QuantidadeAPS, IsNull(aps.QuantidadePorCliente, 0) AS QuantidadePorClienteAPS ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                if (apresentacaoSetorID > 0)
                {
                    stbSQL.Append("INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.CotaID = it.CotaID ");
                    stbSQL.Append("WHERE it.ID = " + cotaItemID + " AND aps.ID = " + apresentacaoSetorID);
                }
                else
                {
                    stbSQL.Append("INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.CotaID = it.CotaID ");
                    stbSQL.Append("WHERE it.ID =" + cotaItemID + " AND aps.ApresentacaoID =" + apresentacaoID + " AND aps.SetorID = " + setorID);
                }

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Item.ID = cotaItemID;
                    Item.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    Item.SetorID = bd.LerInt("SetorID");
                    Item.PrecoIniciaCom = bd.LerString("PrecoIniciaCom");
                    Item.Quantidade = bd.LerInt("Quantidade");
                    Item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                    Item.ParceiroID = bd.LerInt("ParceiroID");
                    Item.ValidaBin = bd.LerBoolean("ValidaBin");
                    Item.QuantidadeApresentacaoSetor = bd.LerInt("QuantidadeAPS");
                    Item.QuantidadePorClienteApresentacaoSetor = bd.LerInt("QuantidadePorClienteAPS");
                    Item.QuantidadePorCodigo = bd.LerInt("QuantidadePorCodigo");
                    Item.Nominal = bd.LerBoolean("Nominal");
                }
                return Item;
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

        public EstruturaCotaItemReserva getCotaItemPorID(int cotaItemID)
        {
            try
            {
                EstruturaCotaItemReserva Item = new EstruturaCotaItemReserva();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append(
                    @"SELECT it.PrecoIniciaCom,
                            it.Quantidade, it.QuantidadePorCliente, it.ParceiroID,
                            it.ValidaBin, TextoValidacao, CPFResponsavel,
                            CASE WHEN Termo <> ''
                                THEN 'T'
                                ELSE 'F'
                            END AS TemTermo ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("WHERE it.ID =" + cotaItemID);

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Item.ID = cotaItemID;
                    Item.TextoValidacao = bd.LerString("TextoValidacao");
                    Item.PrecoIniciaCom = bd.LerString("PrecoIniciaCom");
                    Item.Quantidade = bd.LerInt("Quantidade");
                    Item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                    Item.ParceiroID = bd.LerInt("ParceiroID");
                    Item.ValidaBin = bd.LerBoolean("ValidaBin");
                    Item.CPFResponsavel = bd.LerBoolean("CPFResponsavel");
                    Item.TemTermo = bd.LerBoolean("TemTermo");
                }
                return Item;
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

        public EstruturaCotasValidacao getCotaItemSimples(int cotaItemID)
        {
            try
            {
                EstruturaCotasValidacao Item = new EstruturaCotasValidacao();
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("SELECT it.PrecoIniciaCom, it.ParceiroID, it.ValidaBin, Quantidade, QuantidadePorCliente ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("WHERE it.ID =" + cotaItemID);

                bd.Consulta(stbSQL.ToString());
                if (bd.Consulta().Read())
                {
                    Item.ID = cotaItemID;
                    Item.PrecoIniciaCom = bd.LerString("PrecoIniciaCom");
                    Item.ParceiroID = bd.LerInt("ParceiroID");
                    Item.ValidaBin = bd.LerBoolean("ValidaBin");
                    Item.Quantidade = bd.LerInt("Quantidade");
                    Item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                }
                return Item;
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

        public void AtualizarInformacoesCota(List<EstruturaCotaItemReserva> listaCotaItem, EstruturaPrecoReservaSite preco, EstruturaCotasInfo cotaInfo)
        {
            cotaInfo.CotaItemID = 0;
            cotaInfo.EncontrouCotaItem = false;
            cotaInfo.QuantidadeApresentacao = 0;
            cotaInfo.QuantidadePorClienteCota = 0;
            cotaInfo.QuantidadeCota = 0;
            cotaInfo.ValidaBin = false;
            cotaInfo.ParceiroID = 0;
            cotaInfo.Nominal = false;

            foreach (EstruturaCotaItemReserva itemEncontrado in listaCotaItem.Where(c => preco.PrecoNome.ToLower().StartsWith(c.PrecoIniciaCom.ToLower()) && c.isApresentacao))
            {
                cotaInfo.CotaItemID = itemEncontrado.ID;
                cotaInfo.EncontrouCotaItem = true;
                cotaInfo.QuantidadeApresentacao = itemEncontrado.QuantidadeApresentacao;
                cotaInfo.QuantidadePorClienteCota = itemEncontrado.QuantidadePorCliente;
                cotaInfo.QuantidadeCota = itemEncontrado.Quantidade;
                cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                cotaInfo.Nominal = itemEncontrado.Nominal;
                break;
            }
        }

        public void ValidarCotaInformacoes(int apresentacaoID, int setorID, EstruturaCotaItemReserva item, EstruturaPrecoReservaSite preco, int BIN, int formaPagamentoID, int clienteID, string sessionID, ref string[] msgCota, bool somenteVIR)
        {
            CotaItemControle oCotaItemControle = new CotaItemControle();
            ValeIngresso oValeIngresso = new ValeIngresso();
            try
            {
                int[] quantidades = new int[2] { 0, 0 };
                int apresentacaoSetorID = new ApresentacaoSetor().ApresentacaoSetorID(apresentacaoID, setorID);
                quantidades = oCotaItemControle.getQuantidade(item.ID, apresentacaoID, apresentacaoSetorID);
                if (((quantidades[0] + preco.Quantidade > item.Quantidade || quantidades[1] + preco.Quantidade > item.Quantidade) && item.Quantidade != 0)
                   || (quantidades[0] + preco.Quantidade > item.QuantidadeApresentacao && item.QuantidadeApresentacao != 0)
                   || (quantidades[1] + preco.Quantidade > item.QuantidadeApresentacaoSetor && item.QuantidadeApresentacaoSetor != 0))
                {
                    msgCota[0] = "4";
                    msgCota[1] = "O Limite de venda do preço especial: " + preco.PrecoNome + " foi excedido";
                }
                if (string.IsNullOrEmpty(msgCota[0]) && item.ValidaBin)
                {
                    if (item.ValidaBin && BIN == 0)
                    {
                        msgCota[0] = "1";

                        if (somenteVIR)
                            msgCota[1] = "Atenção, O Preço: " + preco.PrecoNome + " requer que o ingresso seja pago com um cartão válido para a promoção <br /> Não será possivel comprar somente com Vale Ingressos";

                        else
                            msgCota[1] = "Atenção, O Preço: " + preco.PrecoNome + " requer que o ingresso seja pago com um cartão válido para a promoção. <br /> Compras com Visa Electron, ItauShopLine e Somente Vale Ingressos não serão aceitas";

                    }
                    else if (!this.ValidarBin(BIN, item.ID, item.ParceiroID))
                    {
                        msgCota[0] = "1";
                        msgCota[1] = "Atenção, o BIN do cartão digitado não corresponde a um BIN válido para o preço: " + preco.PrecoNome + ".";
                    }
                }
                else if (string.IsNullOrEmpty(msgCota[0]))
                {
                    if (somenteVIR)
                        formaPagamentoID = oValeIngresso.FormaDePagamentoID;
                    if (!this.ValidarFormaPagamento(formaPagamentoID, item.ID))
                    {
                        msgCota[0] = "2";

                        if (somenteVIR)
                            msgCota[1] = "A Forma de Pagamento Somente Vale Ingresso não é válida para o Preço: " + preco.PrecoNome;
                        else
                            msgCota[1] = "A Forma de Pagamento selecionada é válida para o Preço: " + preco.PrecoNome;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string[] ValidarCotaInformacoesNovo(EstruturaCotasInfo cotasInfo, string precoNome, int BIN, int formaPagamentoID, bool somenteVIR, bool somenteCortesias)
        {
            string[] msgCota = new string[2];
            CotaItemControle oCotaItemControle = new CotaItemControle();
            ValeIngresso oValeIngresso = new ValeIngresso();
            try
            {
                if (string.IsNullOrEmpty(msgCota[0]) && cotasInfo.ValidaBin)
                {
                    if (BIN == 0)
                    {
                        msgCota[0] = "1";

                        if (somenteVIR)
                            msgCota[1] = "Atenção, O preço: " + precoNome + " requer que o ingresso seja pago com um cartão válido para a promoção <br /> Não será possivel comprar somente com Vale Ingressos";

                        else
                            msgCota[1] = "Atenção, O preço: " + precoNome + " requer que o ingresso seja pago com um cartão válido para a promoção. <br /> Compras com Visa Electron, ItauShopLine e Somente Vale Ingressos não serão aceitas";

                    }
                    else if (!this.ValidarBin(BIN, cotasInfo.CotaItemID > 0 ? cotasInfo.CotaItemID : cotasInfo.CotaItemID_APS, cotasInfo.ParceiroID))
                    {
                        msgCota[0] = "1";
                        msgCota[1] = "Atenção, o BIN digitado não corresponde a um BIN válido para o preço: " + precoNome + ".";
                    }
                }

                if (string.IsNullOrEmpty(msgCota[0]))
                {
                    if (somenteVIR)
                        formaPagamentoID = oValeIngresso.FormaDePagamentoID;
                    else if (somenteCortesias)
                        return msgCota;

                    if (!this.ValidarFormaPagamento(formaPagamentoID, cotasInfo.CotaItemID > 0 ? cotasInfo.CotaItemID : cotasInfo.CotaItemID_APS))
                    {
                        msgCota[0] = "2";

                        if (somenteVIR)
                            msgCota[1] = "A Forma de Pagamento Somente Vale Ingresso não é válida para o Preço: " + precoNome;
                        else
                            msgCota[1] = "A Forma de Pagamento selecionada é válida para o Preço: " + precoNome;
                    }
                }

                return msgCota;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int getCotaItemIDporPrecoID(int PrecoID)
        {
            try
            {
                string sql = "EXEC sp_GetCotaItemIDPorPrecoID " + PrecoID.ToString();
                int CotaItemID = 0;
                while (bd.Consulta(sql).Read())
                {
                    CotaItemID = bd.LerInt("ID");
                }

                return CotaItemID;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
            

        }
        public bool ValidarBin(int BIN, int cotaItemID, int parceiroID)
        {
            try
            {
                string sql = "SELECT COUNT(ID) FROM tBin (NOLOCK) WHERE BIN = '" + BIN + "' AND ParceiroID = " + parceiroID;
                return (Convert.ToInt32(bd.ConsultaValor(sql)) > 0);
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

        public bool ValidarFormaPagamento(int formaPagamentoID, int cotaItemID)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(it.ID) FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("INNER JOIN tCotaItemFormaPagamento cifp (NOLOCK) ON it.ID = cifp.CotaItemID ");
                stbSQL.Append("WHERE cifp.FormaPagamentoID = " + formaPagamentoID);

                return (Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString())) > 0);
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

        public string ValidarCodigoPromo(int parceiroID, List<string> lstCodigoPromo, string codigoAtual, int apresentacaoID)
        {
            try
            {
                StringBuilder stbSQL;
                int qtd = 0;
                for (int i = 0; i < lstCodigoPromo.Count; i++)
                {
                    if (string.Compare(codigoAtual, lstCodigoPromo[i]) == 0)
                        qtd++;

                    if (qtd > 1)
                        return ("O Código Promocional " + codigoAtual + " não pode ser utilizado múltiplas vezes para a mesma apresentação.");

                }

                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT COUNT(ID) FROM tIngressoCliente (NOLOCK) WHERE ApresentacaoID = " + apresentacaoID);
                stbSQL.Append(" AND CodigoPromocional = '" + codigoAtual.Replace("'", "") + "'");

                qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                if (qtd > 0)
                    return ("O Código Promocional " + codigoAtual + " já foi utilizado para esta apresentação.");
                else
                    bd.FecharConsulta();


                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tCodigoPromo (NOLOCK) ");
                stbSQL.Append("WHERE ParceiroID = " + parceiroID + " AND ");
                stbSQL.Append("Codigo = '" + codigoAtual.Replace("'", "") + "'");

                qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                if (qtd > 0)
                    return string.Empty;
                else
                    return ("O Código Promocional " + codigoAtual + " não é valido.");
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

        public string ValidarCodigoPromoInternet(int parceiroID, string codigoAtual, int apresentacaoID)
        {
            try
            {
                StringBuilder stbSQL;
                int qtd = 0;

                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT COUNT(ID) FROM tIngressoCliente (NOLOCK) WHERE ApresentacaoID = " + apresentacaoID);
                stbSQL.Append(" AND CodigoPromocional = '" + codigoAtual.Replace("'", "") + "'");

                qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                if (qtd > 0)
                    return ("O Código Promocional " + codigoAtual + " já foi utilizado para esta apresentação.");
                else
                    bd.FecharConsulta();


                stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM tCodigoPromo (NOLOCK) ");
                stbSQL.Append("WHERE ParceiroID = " + parceiroID + " AND ");
                stbSQL.Append("Codigo = '" + codigoAtual.Replace("'", "") + "'");

                qtd = Convert.ToInt32(bd.ConsultaValor(stbSQL.ToString()));
                if (qtd > 0)
                    return string.Empty;
                else
                    return ("O Código Promocional " + codigoAtual + " não é valido.");
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

        public int getQtd(int cotaItemID)
        {
            int Qtd = 0;
            string sql = string.Format(@"SELECT tci.Quantidade FROM dbo.tCotaItem tci WHERE tci.ID = {0}", cotaItemID);
            bd.Consulta(sql);
            if (bd.Consulta().Read())
            {
                Qtd = bd.LerInt("Quantidade");
            }
            return Qtd;
        }
        public int GetCotaItemIDPorCotaID(int cotaID, string precoNome)
        {
            int ID = 0;
            string sql = string.Format(@"SELECT ID FROM dbo.tCotaItem tci WHERE tci.CotaID = {0} AND tci.PrecoIniciaCom LIKE '{1}%'", cotaID, precoNome);
            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                ID = bd.LerInt("ID");
            }

            return ID;
        }

        public int GetPrecoIDPacoteID(int cotaID, string PacoteID)
        {
            int ID = 0;
            string sql = string.Format(@"EXEC sp_GetPrecoPacoteID {0},{1}", cotaID, PacoteID);
            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                ID = bd.LerInt("ID");
            }

            return ID;
        } 
        public DataTable getDttCotaItemPorCotaID(int[] cotaItemArray)
        {
            try
            {
                DataTable dtt = this.getEstruturaCotaItem();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT it.ID, it.CotaID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.ParceiroID, ");
                stbSQL.Append("it.ValidaBin, it.TextoValidacao, it.ObrigatoriedadeID, it.Nominal, it.QuantidadePorCodigo, it.TermoSite ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("WHERE it.ID IN (" + Utilitario.ArrayToString(cotaItemArray));
                stbSQL.Append(") ORDER BY it.PrecoIniciaCom");
                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow dtr = dtt.NewRow();
                    dtr["ID"] = bd.LerInt("ID");
                    dtr["CotaID"] = bd.LerInt("CotaID");
                    dtr["PrecoIniciaCom"] = bd.LerString("PrecoIniciaCom");
                    dtr["Quantidade"] = bd.LerInt("Quantidade");
                    dtr["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                    dtr["ParceiroID"] = bd.LerInt("ParceiroID");
                    dtr["ValidaBin"] = bd.LerBoolean("ValidaBin");
                    dtr["TextoValidacao"] = bd.LerString("TextoValidacao");
                    dtr["ObrigatoriedadeID"] = bd.LerInt("ObrigatoriedadeID");
                    dtr["Nominal"] = bd.LerBoolean("Nominal");
                    dtr["QuantidadePorCodigo"] = bd.LerInt("QuantidadePorCodigo");
                    dtr["TermoSite"] = bd.LerString("TermoSite");
                    dtt.Rows.Add(dtr);
                }
                return dtt;
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

        public DataTable getEstruturaCotaItem()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID", typeof(int));
            dtt.Columns.Add("CotaID", typeof(int));
            dtt.Columns.Add("PrecoIniciaCom", typeof(string));
            dtt.Columns.Add("ParceiroID", typeof(int));
            dtt.Columns.Add("ValidaBin", typeof(bool));
            dtt.Columns.Add("TextoValidacao", typeof(string));
            dtt.Columns.Add("ObrigatoriedadeID", typeof(int));
            dtt.Columns.Add("Quantidade", typeof(int));
            dtt.Columns.Add("QuantidadePorCliente", typeof(int));
            dtt.Columns.Add("Nominal", typeof(bool));
            dtt.Columns.Add("QuantidadePorCodigo", typeof(int));
            dtt.Columns.Add("TermoSite", typeof(string));
            return dtt;
        }

        public Dictionary<string,int> GetCamposCotaPromoCodigo(int CotaItemID)
        {
            try
            {
                Dictionary<string, int> retorno = new Dictionary<string, int>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("EXEC sp_getCamposCotaPromocaoCodigo " + CotaItemID.ToString() );

                while (bd.Consulta(stbSQL).Read())
                {
                    retorno.Add("Quantidade", bd.LerInt("Quantidade"));
                    retorno.Add("QuantidadePorCliente", bd.LerInt("QuantidadePorCliente"));
                    retorno.Add("QuantidadePorCodigo", bd.LerInt("QuantidadePorCodigo"));
 
                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
        }


        public int GetCamposCotaPromoCodigoqtd(int CotaItemID)
        {
            try
            {
                int retorno = 0;
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("EXEC sp_getCamposCotaPromocaoCodigo " + CotaItemID.ToString());

                while (bd.Consulta(stbSQL).Read())
                {
                    retorno = bd.LerInt("Quantidade");
                }
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                bd.FecharConsulta();
            }
        }

        public bool getValidaBIN(int cotaItemID)
        {
            try
            {
                string strSQL = "SELECT CASE ValidaBin WHEN 'T' THEN 1 ELSE 0 END AS ValidaBin FROM tCotaItem (NOLOCK) WHERE ID = " + cotaItemID;
                bool valida = Convert.ToInt32(bd.ConsultaValor(strSQL)) == 1;
                return valida;
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

        public List<EstruturaTermo> CarregarTermos(List<int> ids)
        {
            try
            {
                List<EstruturaTermo> lista = new List<EstruturaTermo>();
                DataTable dttBulk = new DataTable();
                dttBulk.Columns.Add("ID");
                DataRow dtr;
                foreach (int id in ids)
                {
                    dtr = dttBulk.NewRow();
                    dtr["ID"] = id;
                    dttBulk.Rows.Add(dtr);
                }
                bd.BulkInsert(dttBulk, "#tmpCotaItems", false, true);

                bd.Consulta(@"SELECT PrecoIniciaCom, Termo FROM tCotaItem (NOLOCK)
                            INNER JOIN #tmpCotaItems ON tCotaItem.ID = #tmpCotaItems.ID");

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaTermo()
                    {
                        PrecoNome = bd.LerString("PrecoIniciaCom"),
                        Termo = bd.LerString("Termo"),
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
        /// Inserir novo(a) CotaItem
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCotaItem");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCotaItem(ID, CotaID, PrecoIniciaCom, Quantidade, QuantidadePorCliente, ParceiroID, ValidaBin, TextoValidacao, ObrigatoriedadeID, CPFResponsavel, Termo, TermoSite, Nominal, QuantidadePorCodigo, Tipo) ");
                sql.Append("VALUES (@ID,@001,'@002',@003,@004,@005,'@006','@007',@008,'@009','@010','@011','@012',@013, @014)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.CotaID.ValorBD);
                sql.Replace("@002", this.PrecoIniciaCom.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@005", this.ParceiroID.ValorBD);
                sql.Replace("@006", this.ValidaBin.ValorBD);
                sql.Replace("@007", this.TextoValidacao.ValorBD);
                sql.Replace("@008", this.ObrigatoriedadeID.ValorBD);
                sql.Replace("@009", this.CPFResponsavel.ValorBD);
                sql.Replace("@010", this.Termo.ValorBD);
                sql.Replace("@011", this.TermoSite.ValorBD);
                sql.Replace("@012", this.Nominal.ValorBD);
                sql.Replace("@013", this.QuantidadePorCodigo.ValorBD);
                sql.Replace("@014", this.Tipo.ValorBD);

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



        protected void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCotaItem (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        public bool VerificaQuantidadeVendida(string QuantidadeCastrada, string CotaitemID)
        {


            int Quantidade = 0;


           bd.Consulta(@"EXEC sp_VerificaQuantidadeVendidaCotaItem " + CotaitemID);

           while (bd.Consulta().Read())
           {
               Quantidade = bd.LerInt("Quantidade");
           }

            if (Quantidade > Convert.ToInt32(QuantidadeCastrada))
            {
                return false;
            }

            return true;
        }

    }

    public class CotaItemLista : CotaItemLista_B
    {

        public CotaItemLista() { }

        public CotaItemLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
