/**************************************************
* Arquivo: Cota.cs
* Gerado: 14/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Cota : Cota_B
    {

        public enum TipoDistribuicao
        {
            TodosEventos,
            TodasApresentacoes,
            Apresentacao,
            TodosSetores,
            Setor
        }
        public const string TextoValidacaoPadrao = "Digite o código promocional para o preço selecionado";

        public const string TextoValidacaoPadraoCodigoExterno = "Digite o CPF para validar o Preço";

        public Cota() { }

        public Cota(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public DataTable getDttCotaItemPorCotaID(int cotaID)
        {
            try
            {
                DataTable dtt = this.getEstruturaCotaItem();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT it.ID, it.CotaID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.ParceiroID, ");
                stbSQL.Append("it.ValidaBin, it.TextoValidacao, it.ObrigatoriedadeID, it.Termo, it.CPFResponsavel, it.Nominal, it.QuantidadePorCodigo ");
                stbSQL.Append("FROM tCotaItem it (NOLOCK) ");
                stbSQL.Append("WHERE it.CotaID =" + cotaID);
                stbSQL.Append(" ORDER BY it.PrecoIniciaCom");
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
                    dtr["CPFResponsavel"] = bd.LerBoolean("CPFResponsavel");
                    dtr["Termo"] = bd.LerString("Termo");
                    dtr["Nominal"] = bd.LerBoolean("Nominal");
                    dtr["QuantidadePorCodigo"] = bd.LerInt("QuantidadePorCodigo");
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
            dtt.Columns.Add("CPFResponsavel", typeof(bool));
            dtt.Columns.Add("Termo", typeof(string));
            dtt.Columns.Add("Nominal", typeof(bool));
            dtt.Columns.Add("QuantidadePorCodigo", typeof(int));
            return dtt;
        }

        public List<EstruturaCotaItem> getCotaItemPorCotaID(int cotaID)
        {
            try
            {
                List<EstruturaCotaItem> lista = new List<EstruturaCotaItem>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT it.ID, c.ID AS CotaID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.ParceiroID, it.Nominal, it.QuantidadePorCodigo, ");
                stbSQL.Append("it.ValidaBin, it.TextoValidacao, it.ObrigatoriedadeID, it.CPFResponsavel, it.Termo, it.TermoSite, c.Quantidade AS QuantidadeCota, c.QuantidadePorCliente AS QuantidadePorClienteCota, Count(IsNull(ic.ID, 0)) AS Distribuido ");
                stbSQL.Append("FROM tCota c (NOLOCK) ");
                stbSQL.Append("LEFT JOIN tCotaItem it (NOLOCK) ON c.ID = it.CotaID ");
                stbSQL.Append("LEFT JOIN tCotaItemControle ic(NOLOCK) ON it.ID = ic.CotaItemID ");
                stbSQL.Append("WHERE c.ID =" + cotaID);
                stbSQL.Append(" GROUP BY it.ID, c.ID, it.PrecoIniciaCom, it.Quantidade, it.QuantidadePorCliente, it.ParceiroID, it.ValidaBin, ");
                stbSQL.Append("it.TextoValidacao, it.ObrigatoriedadeID, c.Quantidade, c.QuantidadePorCliente, it.Termo, it.TermoSite, it.CPFResponsavel, it.Nominal, it.QuantidadePorCodigo ");
                stbSQL.Append(" ORDER BY it.PrecoIniciaCom");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaCotaItem()
                    {
                        ID = bd.LerInt("ID"),
                        CotaID = bd.LerInt("CotaID"),
                        precoIniciaCom = bd.LerString("PrecoIniciaCom"),
                        quantidade = bd.LerInt("Quantidade"),
                        quantidadePorCliente = bd.LerInt("QuantidadePorCliente"),
                        parceiroID = bd.LerInt("ParceiroID"),
                        validaBin = bd.LerBoolean("ValidaBin"),
                        textoValidacao = bd.LerString("TextoValidacao"),
                        obrigatoriedadeID = bd.LerInt("ObrigatoriedadeID"),
                        QuantidadeCota = bd.LerInt("QuantidadeCota"),
                        QuantidadePorClienteCota = bd.LerInt("QuantidadePorClienteCota"),
                        distribuido = bd.LerInt("Distribuido") > 0,
                        CPFResponsavel = bd.LerBoolean("CPFResponsavel"),
                        Termo = bd.LerString("Termo"),
                        TermoSite = bd.LerString("TermoSite"),
                        QuantidadePorCodigo = bd.LerInt("QuantidadePorCodigo"),
                        Nominal = bd.LerBoolean("Nominal"),
                    });
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

        public List<EstruturaCotasSelecao> getCotasPorLocalID(int localID)
        {
            try
            {
                List<EstruturaCotasSelecao> listaCotas = new List<EstruturaCotasSelecao>();
                EstruturaCotasSelecao item = new EstruturaCotasSelecao();
                string strSQL = "SELECT ID, Nome,Quantidade,QuantidadePorCliente FROM tCota (NOLOCK) Where LocalID = " + localID +
                    " ORDER BY Nome ";

                item.ID = 0;
                item.Nome = "Selecione...";
                item.QuantidadePorCliente = 0;
                item.Quantidade = 0;
                listaCotas.Add(item);

                bd.Consulta(strSQL);

                while (bd.Consulta().Read())
                {
                    item = new EstruturaCotasSelecao();
                    item.ID = bd.LerInt("ID");
                    item.Nome = bd.LerString("Nome");
                    item.Quantidade = bd.LerInt("Quantidade");
                    item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
                    listaCotas.Add(item);
                }

                return listaCotas;
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

        public int SalvarCota(List<EstruturaCotaItem> listaCotaItem, EstruturaCota cota)
        {
            try
            {
                bd.IniciarTransacao();

                int cotaID = 0;
                this.Nome.Valor = cota.Nome;
                this.LocalID.Valor = cota.LocalID;
                this.Quantidade.Valor = cota.Quantidade;
                this.QuantidadePorCliente.Valor = cota.QuantidadePorCliente;
                if (cota.Novo)
                {
                    this.Inserir(bd);
                    cotaID = this.Control.ID;
                }
                else
                {
                    cotaID = cota.ID;
                    this.Control.ID = cotaID;
                    this.Atualizar(bd);
                }

                CotaItem oCotaItem = new CotaItem(this.Control.UsuarioID);
                oCotaItem.SalvarItem(bd, listaCotaItem, cotaID, !cota.Novo);

                bd.FinalizarTransacao();
                return cotaID;
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

        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cCota (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xCota (ID, Versao, Nome, LocalID, Quantidade, QuantidadePorCliente) ");
                sql.Append("SELECT ID, @V, Nome, LocalID, Quantidade, QuantidadePorCliente FROM tCota WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) Cota
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cCota");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tCota(ID, Nome, LocalID, Quantidade, QuantidadePorCliente) ");
                sql.Append("VALUES (@ID,'@001',@002,@003,@004)");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.LocalID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle(bd, "I");


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Atualiza Cota
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {


                string sqlVersion = "SELECT MAX(Versao) FROM cCota WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCota SET Nome = '@001', LocalID = @002, Quantidade = @003, QuantidadePorCliente = @004 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Nome.ValorBD);
                sql.Replace("@002", this.LocalID.ValorBD);
                sql.Replace("@003", this.Quantidade.ValorBD);
                sql.Replace("@004", this.QuantidadePorCliente.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EstruturaCotasRetornoDistribuicao> getDistribuicaoApresentacao(int localID, int cotaID)
        {
            try
            {
                List<EstruturaCotasRetornoDistribuicao> lista = new List<EstruturaCotasRetornoDistribuicao>();
                EstruturaCotasRetornoDistribuicao item;

                bd.Consulta("EXEC sp_getCotasApresentacoes " + localID + ", " + cotaID);
                while (bd.Consulta().Read())
                {
                    item = new EstruturaCotasRetornoDistribuicao();
                    item.EventoID = bd.LerInt("EventoID");
                    item.Evento = bd.LerString("Evento");
                    item.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    item.Horario = bd.LerStringFormatoDataHora("Horario");
                    item.Quantidade = bd.LerInt("Quantidade");
                    item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
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

        public List<EstruturaCotasRetornoDistribuicao> getDistribuicaoApresentacaoSetor(int localID, int cotaID)
        {
            try
            {
                List<EstruturaCotasRetornoDistribuicao> lista = new List<EstruturaCotasRetornoDistribuicao>();
                EstruturaCotasRetornoDistribuicao item;

                bd.Consulta("EXEC sp_getCotasApresentacaoSetor " + localID + ", " + cotaID);
                while (bd.Consulta().Read())
                {
                    item = new EstruturaCotasRetornoDistribuicao();
                    item.EventoID = bd.LerInt("EventoID");
                    item.Evento = bd.LerString("Evento");
                    item.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    item.Horario = bd.LerStringFormatoDataHora("Horario");
                    item.SetorID = bd.LerInt("SetorID");
                    item.Setor = bd.LerString("Setor");
                    item.ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                    item.Quantidade = bd.LerInt("Quantidade");
                    item.QuantidadePorCliente = bd.LerInt("QuantidadePorCliente");
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

        public DataTable getDataTableDistribuicaoApresentacaoSetor(int localID, int cotaID)
        {
            try
            {
                DataTable dtt = this.estruturaRetornoDistribuicao();

                bd.Consulta("EXEC sp_getCotasApresentacaoSetor " + localID + ", " + cotaID);
                while (bd.Consulta().Read())
                {
                    DataRow item = dtt.NewRow();
                    item["EventoID"] = bd.LerInt("EventoID");
                    item["Evento"] = bd.LerString("Evento");
                    item["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    item["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    item["SetorID"] = bd.LerInt("SetorID");
                    item["Setor"] = bd.LerString("Setor");
                    item["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    item["Quantidade"] = bd.LerInt("Quantidade");
                    item["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                    item["QuantidadeJaVendida"] = bd.LerInt("QuantidadeJaVendida");
                    dtt.Rows.Add(item);
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

        public DataTable getDataTableDistribuicaoApresentacao(int localID, int cotaID)
        {
            try
            {
                DataTable dtt = this.estruturaRetornoDistribuicao();

                bd.Consulta("EXEC sp_getCotasApresentacoes " + localID + ", " + cotaID);
                while (bd.Consulta().Read())
                {

                    DataRow item = dtt.NewRow();
                    item["EventoID"] = bd.LerInt("EventoID");
                    item["Evento"] = bd.LerString("Evento");
                    item["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    item["Horario"] = bd.LerStringFormatoDataHora("Horario");
                    item["Quantidade"] = bd.LerInt("Quantidade");
                    item["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                    item["QuantidadeJaVendida"] = bd.LerInt("QuantidadeJaVendida");
                    //if (bd.LerBoolean("Exibir"))
                    dtt.Rows.Add(item);
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

        public DataTable estruturaRetornoDistribuicao()
        {
            DataTable dtt = new DataTable();

            dtt.Columns.Add("EventoID", typeof(int));
            dtt.Columns[0].DefaultValue = 0;

            dtt.Columns.Add("ApresentacaoID", typeof(int));
            dtt.Columns[1].DefaultValue = 0;

            dtt.Columns.Add("SetorID", typeof(int));
            dtt.Columns[2].DefaultValue = 0;

            dtt.Columns.Add("ApresentacaoSetorID", typeof(int));
            dtt.Columns[3].DefaultValue = 0;

            dtt.Columns.Add("Evento", typeof(string));
            dtt.Columns[4].DefaultValue = string.Empty;

            dtt.Columns.Add("Horario", typeof(string));
            dtt.Columns[5].DefaultValue = string.Empty;

            dtt.Columns.Add("Setor", typeof(string));
            dtt.Columns[6].DefaultValue = string.Empty;

            dtt.Columns.Add("Quantidade", typeof(int));
            dtt.Columns[7].DefaultValue = 0;

            dtt.Columns.Add("QuantidadePorCliente", typeof(int));
            dtt.Columns[8].DefaultValue = 0;

            dtt.Columns.Add("QuantidadeJaVendida", typeof(int));
            dtt.Columns[9].DefaultValue = 0;

            return dtt;
        }

        public int DistribuiCota(TipoDistribuicao tipo, EstruturaCotasDistribuir oDistribuir)
        {
            try
            {
                switch (tipo)
                {
                    case TipoDistribuicao.TodosEventos:
                        {
                            //Apresentacao SET CotaID = this.CotaID WHERE LocalID = this.LocalID
                            Apresentacao apresentacao = new Apresentacao();
                            apresentacao.DistribuirCotasPorLocal(oDistribuir);
                            return 1;
                        }
                    case TipoDistribuicao.TodasApresentacoes:
                        {
                            //Apresentacao SET CotaID = this.CotaID WHERE tEvento.ID = this.cmbEvento.SelectedValue
                            Apresentacao apresentacao = new Apresentacao();
                            apresentacao.DistribuirCotasPorEvento(oDistribuir, true);
                            return 1;
                        }
                    case TipoDistribuicao.Apresentacao:
                    case TipoDistribuicao.TodosSetores:
                        {
                            //Apresentacao SET CotaID = this.CotaID WHERE tApresentacao = this.cmbApresentacao.SelectedValue
                            Apresentacao apresentacao = new Apresentacao();
                            apresentacao.DistribuirCotasPorApresentacao(oDistribuir, true);
                            return 1;
                        }
                    case TipoDistribuicao.Setor:
                        {
                            //ApresentacaoSetor Set CotaID = this.CotaID WHERE ApresentacaoID = this.cmbApresentacao.SelectedValue AND SetorID = this.cmbSetor.SelectedValue
                            ApresentacaoSetor apresentacaoSetor = new ApresentacaoSetor();
                            return apresentacaoSetor.DistribuirCotasPorApresentacaoSetor(oDistribuir);
                        }
                    default:
                        return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public override bool Inserir()
        {
            throw new NotImplementedException();
        }
    }

    public class CotaLista : CotaLista_B
    {

        public CotaLista() { }

        public CotaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
