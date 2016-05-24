/**************************************************
* Arquivo: ApresentacaoSetor.cs
* Gerado: 08/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace IRLib
{

    public class ApresentacaoSetor : ApresentacaoSetor_B
    {
        public enum enumAcaoPrecoPrincipal
        {
            Adicionar,
            Remover,
            Manter,
        }


        private int UsuarioIDLogado;

        public ApresentacaoSetor() { }

        public ApresentacaoSetor(int usuarioIDLogado)
            : base(usuarioIDLogado)
        {
            this.UsuarioIDLogado = usuarioIDLogado;
        }

        public Setor.Tipo TipoSetor { get; set; }



        /// <summary>
        /// Preenche todos os atributos de ApresentacaoSetor (Utiliza NOLOCK!)
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerNolock(int id)
        {

            try
            {
                string sql = "SELECT * FROM tApresentacaoSetor (NOLOCK) WHERE ID = " + id;
                using (bd.Consulta(sql))
                {
                    if (bd.Consulta().Read())
                    {
                        this.Control.ID = id;
                        this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                        this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                        this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                        this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                        this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                        this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                        this.IngressosGerados.ValorBD = bd.LerString("IngressosGerados");
                        this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                        this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                        this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                        this.PrincipalPrecoID.ValorBD = bd.LerInt("PrincipalPrecoID").ToString();
                    }
                    else
                        this.Limpar();

                    bd.Fechar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Remove ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public bool Remover()
        {

            IngressoLista ingressoLista = new IngressoLista();
            ingressoLista.FiltroSQL = "ApresentacaoSetorID=" + this.Control.ID;
            ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
            ingressoLista.Carregar(1);
            if (ingressoLista.Tamanho > 0)
                throw new LugarException("Não pode excluir a ApresentaçãoSetor " + this.Control.ID + " porque há ingressos não-disponíveis.");

            ingressoLista.FiltroSQL = null;
            ingressoLista.FiltroSQL = "ApresentacaoSetorID=" + this.Control.ID;
            ingressoLista.Carregar();

            bool ok = true;

            if (ingressoLista.Tamanho > 0)
            {
                IngressoLogLista ingressoLogLista = new IngressoLogLista();
                ingressoLogLista.FiltroSQL = "IngressoID in (" + ingressoLista + ")";
                ingressoLogLista.FiltroSQL = "VendaBilheteriaItemID <> 0";
                ingressoLogLista.Carregar(1);
                if (ingressoLogLista.Tamanho > 0)
                {
                    throw new LugarException("Não pode excluir a ApresentaçãoSetor " + this.Control.ID + " porque mesmo q os ingressos estejam disponíveis, há vendas efetuadas com esses ingressos.");
                }
                else
                {
                    ingressoLogLista.FiltroSQL = null;
                    ingressoLogLista.FiltroSQL = "IngressoID in (" + ingressoLista + ")";
                    ok = ingressoLogLista.ExcluirTudo();
                }

                if (ok)
                {

                    try
                    {

                        string sqlDelete = "DELETE FROM tIngresso WHERE ApresentacaoSetorID=" + this.Control.ID;

                        int x = bd.Executar(sqlDelete);
                        bd.Fechar();

                        ok = Convert.ToBoolean(x);

                    }
                    catch
                    {
                        ok = false;
                    }

                }

            }

            if (ok)
            {

                PrecoLista precoLista = new PrecoLista(UsuarioIDLogado);
                precoLista.FiltroSQL = "ApresentacaoSetorID=" + this.Control.ID;
                precoLista.Carregar();
                if (precoLista.Tamanho > 0)
                {
                    CanalPrecoLista canalPrecoLista = new CanalPrecoLista(UsuarioIDLogado);
                    canalPrecoLista.FiltroSQL = "PrecoID in (" + precoLista + ")";
                    canalPrecoLista.Carregar();
                    ok = canalPrecoLista.ExcluirTudo();
                    if (ok)
                        ok = precoLista.ExcluirTudo();
                }

            }

            if (ok)
                ok = this.Excluir();

            return ok;

        }

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xApresentacaoSetor (ID, Versao, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID) ");
                sql.Append("SELECT ID, @V, SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID FROM tApresentacaoSetor WHERE ID = @I");
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
        /// Retorna o ID da ApresentacaoSetor dada uma apresentacao e um setor
        /// </summary>
        /// <returns></returns>
        public override int ApresentacaoSetorID(int apresentacaoid, int setorid)
        {

            try
            {

                string sql = "SELECT ID FROM tApresentacaoSetor (NOLOCK) " +
                    "WHERE SetorID=" + setorid + " AND ApresentacaoID=" + apresentacaoid;

                object ret = bd.ConsultaValor(sql);

                bd.Fechar();

                int apresentacaoSetorID = (ret != null) ? (int)ret : 0;

                return apresentacaoSetorID;

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

        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cApresentacaoSetor (NOLOCK) WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U", bd);
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tApresentacaoSetor SET SetorID = @001, ApresentacaoID = @002, VersaoImagemIngresso = @003, VersaoImagemVale = @004, VersaoImagemVale2 = @005, VersaoImagemVale3 = @006, IngressosGerados = '@007', CotaID = @008, Quantidade = @009, QuantidadePorCliente = @010, PrincipalPrecoID = @011 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna um 'stringao' com os lugares dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public override string Mapa()
        {
            try
            {
                StringBuilder mapa = new StringBuilder();

                string SQL = string.Format(@"SELECT a.LugarID,
                                                    MAX(a.BloqueioID) AS BloqueioID,
                                                    a.Quantidade,
                                                    a.PosicaoX,
                                                    a.PosicaoY,
                                                    a.Codigo,
                                                    a.PerspectivaLugarID,
                                                    a.DescricaoPerspectiva,
                                                    a.PNETipo,
                                                    a.LugarIDCadeirante
                                                FROM (SELECT DISTINCT
                                                             tIngresso.LugarID,
                                                             tIngresso.BloqueioID,
                                                             tLugar.Quantidade,
                                                             tLugar.PosicaoX,
                                                             tLugar.PosicaoY,
                                                             CASE tSetor.LugarMarcado
                                                             WHEN 'C'
                                                               THEN tIngresso.Codigo
                                                             ELSE substring(tIngresso.Codigo, 1, patindex('%-%', tIngresso.Codigo) - 1) END AS Codigo,
                                                             IsNull(tLugar.PerspectivaLugarID, 0)                                           AS PerspectivaLugarID,
                                                             IsNull(tPerspectivaLugar.Descricao, '')                                        AS DescricaoPerspectiva,
                                                  isnull(tLugar.PNETipo, '') as PNETipo,
                                                  isnull(tLugar.LugarIDCadeirante, 0) as LugarIDCadeirante
                                                      FROM tIngresso
                                                        ( NOLOCK )
                                                        INNER JOIN tLugar
                                                        ( NOLOCK ) ON tIngresso.LugarID = tLugar.ID
                                                        INNER JOIN tSetor
                                                        ( NOLOCK ) ON tIngresso.SetorID = tSetor.ID AND tLugar.SetorID = tSetor.ID
                                                        LEFT JOIN tPerspectivaLugar
                                                        ( NOLOCK ) ON tPerspectivaLugar.ID = tLugar.PerspectivaLugarID
                                                      WHERE tIngresso.ApresentacaoSetorID = {0} AND tSetor.ID = {1}) AS a
                                                GROUP BY a.LugarID, a.Quantidade, a.PosicaoX, a.PosicaoY, a.Codigo, a.PerspectivaLugarID, a.DescricaoPerspectiva, a.PNETipo, a.LugarIDCadeirante
                                                ORDER BY a.LugarID", this.Control.ID, this.SetorID.Valor);

                bd.Consulta(SQL);

                while (bd.Consulta().Read())
                {
                    mapa.Append(bd.LerInt("LugarID") + ":");
                    mapa.Append(bd.LerInt("BloqueioID") + ":");
                    mapa.Append(bd.LerString("Codigo") + ":");
                    mapa.Append(bd.LerInt("Quantidade") + ":");
                    mapa.Append(bd.LerInt("PosicaoX") + ":");
                    mapa.Append(bd.LerInt("PosicaoY") + ":");
                    mapa.Append(bd.LerInt("PerspectivaLugarID") + ":");
                    mapa.Append(bd.LerString("DescricaoPerspectiva") + ":");
                    mapa.Append(bd.LerString("PNETipo") + ":");
                    mapa.Append(bd.LerString("LugarIDCadeirante") + "|");
                    //cada lugar corresponde a uma |
                    //cada : corresponde a um campo
                }

                bd.Fechar();

                return mapa.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um 'stringao' com os lugares dessa ApresentacaoSetor
        /// job 206. kim
        /// </summary>
        /// <returns></returns>
        public DataTable StatusLugaresComCores(bool bloqueios)
        { //Tela de Status

            try
            {

                DataTable buffer = new DataTable("MapaStatus");
                //Monta o Datatable de retorno
                buffer.Columns.Add("LugarID", typeof(int));
                buffer.Columns.Add("BloqueioID", typeof(int));
                buffer.Columns.Add("Bloqueio", typeof(string)).DefaultValue = ""; //para descricao
                buffer.Columns.Add("BloqueioCorID", typeof(int));
                buffer.Columns.Add("Preco", typeof(string)).DefaultValue = ""; //para descricao
                buffer.Columns.Add("PrecoID", typeof(int));
                buffer.Columns.Add("PrecoCorID", typeof(int));
                buffer.Columns.Add("Cortesia", typeof(string)).DefaultValue = ""; //para descricao
                buffer.Columns.Add("CortesiaID", typeof(int));
                buffer.Columns.Add("CortesiaCorID", typeof(int));
                buffer.Columns.Add("Usuario", typeof(string));
                buffer.Columns.Add("Vendido", typeof(int));
                buffer.Columns.Add("Reservado", typeof(int));
                buffer.Columns.Add("Bloqueado", typeof(int));
                buffer.Columns.Add("Preimpresso", typeof(int));
                buffer.Columns.Add("Prereserva", typeof(int));
                buffer.Columns.Add("Quantidade", typeof(int));
                buffer.Columns.Add("Descricao", typeof(string));
                buffer.Columns.Add("Misc", typeof(bool));

                string sql = "SELECT LugarID, BloqueioID, tBloqueio.Nome AS Bloqueio, tBloqueio.CorID AS BloqueioCorID, PrecoID, tPreco.Nome AS Preco, tPreco.CorID AS PrecoCorID, CortesiaID, tCortesia.Nome AS Cortesia, tCortesia.CorID AS CortesiaCorID, tUsuario.Login, " +
                    "CASE WHEN (tIngresso.Status='" + Ingresso.VENDIDO + "' or tIngresso.Status='" + Ingresso.IMPRESSO + "' or tIngresso.Status='" + Ingresso.ENTREGUE + "') THEN Count(*) ELSE 0 END AS Vendido, " +
                    "CASE WHEN (tIngresso.Status='" + Ingresso.RESERVADO + "') THEN Count(*) ELSE 0 END AS Reservado, " +
                    "CASE WHEN (tIngresso.Status='" + Ingresso.PREIMPRESSO + "') THEN  Count(*) ELSE  0 END AS Preimpresso, " +
                    "CASE WHEN (tIngresso.Status='" + Ingresso.BLOQUEADO + "') THEN Count(*) ELSE 0 END AS Bloqueado, " +
                    "CASE WHEN (tIngresso.Status='" + Ingresso.PRE_RESERVA + "') THEN Count(*) ELSE 0 END AS Prereserva, " +
                    "Count(*) AS Quantidade " +
                    "FROM tIngresso " +
                    "LEFT JOIN tBloqueio ON tBloqueio.ID = tIngresso.BloqueioID " +
                    "LEFT JOIN tPreco ON tPreco.ID = tIngresso.PrecoID " +
                    "LEFT JOIN tCortesia ON tCortesia.ID = tIngresso.CortesiaID " +
                    "LEFT JOIN tUsuario ON tUsuario.ID = tIngresso.UsuarioID " +
                    "WHERE tIngresso.ApresentacaoSetorID=" + this.Control.ID + " " +
                    "GROUP BY LugarID, BloqueioID, tBloqueio.Nome, tBloqueio.CorID, tIngresso.Status, PrecoID, tPreco.Nome, tPreco.CorID, CortesiaID, tCortesia.Nome, tCortesia.CorID, tUsuario.Login " +
                    "ORDER BY LugarID, Vendido desc, Preimpresso desc, Reservado desc, Bloqueado desc";

                bd.Consulta(sql);

                DataTable tabelaTemp = buffer.Clone();

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabelaTemp.NewRow();
                    linha["LugarID"] = bd.LerInt("LugarID");
                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                    linha["Bloqueio"] = bd.LerString("Bloqueio");
                    linha["BloqueioCorID"] = bd.LerInt("BloqueioCorID");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["PrecoCorID"] = bd.LerInt("PrecoCorID");
                    linha["Cortesia"] = bd.LerString("Cortesia");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    linha["CortesiaCorID"] = bd.LerInt("CortesiaCorID");
                    linha["Usuario"] = bd.LerString("Login");
                    linha["Vendido"] = bd.LerInt("Vendido");
                    linha["Preimpresso"] = bd.LerInt("Preimpresso");
                    linha["Prereserva"] = bd.LerInt("Prereserva");
                    linha["Reservado"] = bd.LerInt("Reservado");
                    linha["Bloqueado"] = bd.LerInt("Bloqueado");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    tabelaTemp.Rows.Add(linha);
                }
                bd.Fechar();

                //evita duplicidade de registros.
                DataTable tabelaDistinct = TabelaMemoria.Distinct(tabelaTemp, "LugarID");

                foreach (DataRow lugar in tabelaDistinct.Rows)
                {

                    int lugarID = (int)lugar["LugarID"];

                    DataRow itemBuffer = buffer.NewRow();

                    object valorVend = tabelaTemp.Compute("SUM(Vendido)", "LugarID=" + lugarID);
                    itemBuffer["Vendido"] = (valorVend != DBNull.Value) ? Convert.ToDecimal(valorVend) : 0;

                    object valorPre = tabelaTemp.Compute("SUM(Preimpresso)", "LugarID=" + lugarID);
                    itemBuffer["Preimpresso"] = (valorPre != DBNull.Value) ? Convert.ToDecimal(valorPre) : 0;

                    object valorReser = tabelaTemp.Compute("SUM(Reservado)", "LugarID=" + lugarID);
                    itemBuffer["Reservado"] = (valorReser != DBNull.Value) ? Convert.ToDecimal(valorReser) : 0;

                    object valorBloq = tabelaTemp.Compute("SUM(Bloqueado)", "LugarID=" + lugarID);
                    itemBuffer["Bloqueado"] = (valorBloq != DBNull.Value) ? Convert.ToDecimal(valorBloq) : 0;

                    object valorQtde = tabelaTemp.Compute("SUM(Quantidade)", "LugarID=" + lugarID);
                    itemBuffer["Quantidade"] = (valorQtde != DBNull.Value) ? Convert.ToDecimal(valorQtde) : 0;

                    object valorPreres = tabelaTemp.Compute("SUM(Prereserva)", "LugarID=" + lugarID);
                    itemBuffer["Prereserva"] = (valorPreres != DBNull.Value) ? Convert.ToDecimal(valorPreres) : 0;

                    itemBuffer["LugarID"] = lugarID;

                    //agora juntar todos numa string concatenada separados por | e incluir no buffer.
                    DataRow[] linhas = tabelaTemp.Select("LugarID=" + lugarID);

                    itemBuffer["BloqueioID"] = (int)linhas[0]["BloqueioID"];
                    itemBuffer["BloqueioCorID"] = (int)linhas[0]["BloqueioCorID"];
                    itemBuffer["PrecoID"] = (int)linhas[0]["PrecoID"];
                    itemBuffer["PrecoCorID"] = (int)linhas[0]["PrecoCorID"];
                    itemBuffer["CortesiaID"] = (int)linhas[0]["CortesiaID"];
                    itemBuffer["CortesiaCorID"] = (int)linhas[0]["CortesiaCorID"];
                    itemBuffer["Usuario"] = (string)linhas[0]["Usuario"];

                    StringBuilder desc = new StringBuilder();
                    int qtde = (int)itemBuffer["Quantidade"];
                    if (qtde > 1)
                        desc.Append("\nQuantidade: " + qtde);

                    itemBuffer["Misc"] = (linhas.Length > 1);

                    for (int i = 0; i < linhas.Length; i++)
                    {
                        string preco = (string)linhas[i]["Preco"];
                        string bloqueio = (string)linhas[i]["Bloqueio"];
                        string cortesia = (string)linhas[i]["Cortesia"];
                        string usuario = (string)linhas[i]["Usuario"];
                        int pre = (int)linhas[i]["Preimpresso"];
                        int preres = (int)linhas[i]["Prereserva"];
                        int resr = (int)linhas[i]["Reservado"];
                        int bloq = (int)linhas[i]["Bloqueado"];
                        int vend = (int)linhas[i]["Vendido"];

                        if (!bloqueios)
                        {
                            if (pre > 0)
                            {
                                desc.Append("\nPré-impresso: " + pre + " - ");
                                desc.Append("Preço: " + preco + ". ");
                                if (cortesia != "")
                                    desc.Append("Cortesia: " + cortesia + ". ");
                                desc.Append("por: " + usuario);
                            }
                            else if (vend > 0)
                            {
                                desc.Append("\nVendido: " + vend + " - ");
                                desc.Append("Preço: " + preco + ". ");
                                if (cortesia != "")
                                    desc.Append("Cortesia: " + cortesia + ". ");
                                desc.Append("por: " + usuario);
                            }
                            else if (resr > 0)
                            {
                                desc.Append("\nReservado: " + resr + " - ");
                                desc.Append("Preço: " + preco + ". ");
                                if (cortesia != "")
                                    desc.Append("Cortesia: " + cortesia + ". ");
                                desc.Append("por: " + usuario);
                            }
                            else if (preres > 0)
                            {
                                desc.Append("\nPré-Reservado: " + preres + " - ");
                                desc.Append("Preço: " + preco + ". ");
                                if (cortesia != "")
                                    desc.Append("Cortesia: " + cortesia + ". ");
                                desc.Append("por: " + usuario);
                            }

                        }
                        else
                        { //ingresso bloqueado

                            if (bloqueio == "")
                            {

                                if (pre > 0)
                                {
                                    desc.Append("\nPré-impresso: " + pre + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (vend > 0)
                                {
                                    desc.Append("\nVendido: " + vend + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (resr > 0)
                                {
                                    desc.Append("\nReservado: " + resr + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (preres > 0)
                                {
                                    desc.Append("\nPré-Reservado: " + preres + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("por: " + usuario);
                                }


                            }
                            else
                            {

                                if (pre > 0)
                                {
                                    desc.Append("\nPré-impresso Bloqueado: " + pre + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("Bloqueio: " + bloqueio + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (vend > 0)
                                {
                                    desc.Append("\nVendido Bloqueado: " + vend + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("Bloqueio: " + bloqueio + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (resr > 0)
                                {
                                    desc.Append("\nReservado Bloqueado: " + resr + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("Bloqueio: " + bloqueio + ". ");
                                    desc.Append("por: " + usuario);
                                }
                                else if (preres > 0)
                                {
                                    desc.Append("\nPré-Reservado Bloqueado: " + preres + " - ");
                                    desc.Append("Preço: " + preco + ". ");
                                    if (cortesia != "")
                                        desc.Append("Cortesia: " + cortesia + ". ");
                                    desc.Append("por: " + usuario);
                                }


                            }

                        }
                        //continue;

                        if (bloqueios)
                        {
                            if (bloq > 0)
                                desc.Append("\nBloqueado: " + bloq + " - Bloqueio: " + bloqueio + ". ");
                        }
                        //desc.Append("\n");
                    }

                    itemBuffer["Descricao"] = desc.ToString();

                    buffer.Rows.Add(itemBuffer);

                }

                buffer.Columns.Remove("Preco");
                buffer.Columns.Remove("Bloqueio");
                buffer.Columns.Remove("Cortesia");
                buffer.Columns.Remove("Usuario");

                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable PrecosQtdes()
        {

            try
            {

                DataTable tabela = new DataTable("PrecoQtde");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Qtde", typeof(int));

                tabela.Rows.Add(new Object[] { 0, 0 });

                string sql = "SELECT tPreco.ID, Count(tIngresso.ID) AS Qtde " +
                    "FROM tPreco (nolock) " +
                    "LEFT JOIN tIngresso (nolock) ON tPreco.ID = tIngresso.PrecoID " +
                    "WHERE tPreco.ApresentacaoSetorID=" + this.Control.ID + " AND tIngresso.ApresentacaoSetorID=" + this.Control.ID + " " +
                    "GROUP BY tPreco.ID,tPreco.Nome " +
                    "ORDER BY tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Qtde"] = bd.LerInt("Qtde");
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

        public DataTable CortesiasQtdes(int localID)
        {

            try
            {

                DataTable tabela = new DataTable("CortesiaQtde");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Qtde", typeof(int));

                tabela.Rows.Add(new Object[] { 0, 0 });

                string sql = "SELECT tCortesia.ID, Count(tIngresso.ID) AS Qtde " +
                    "FROM tCortesia (nolock) " +
                    "LEFT JOIN tIngresso (nolock) ON tCortesia.ID = tIngresso.CortesiaID AND tIngresso.ApresentacaoSetorID=" + this.Control.ID + " " +
                    "WHERE tCortesia.LocalID=" + localID + " " +
                    "GROUP BY tCortesia.ID,tCortesia.Nome " +
                    "ORDER BY tCortesia.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Qtde"] = bd.LerInt("Qtde");
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

        public DataTable BloqueiosQtdes(int localID)
        {

            try
            {

                DataTable tabela = new DataTable("BloqueioQtde");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Qtde", typeof(int));

                tabela.Rows.Add(new Object[] { 0, 0 });

                string sql = "SELECT tBloqueio.ID, Count(tIngresso.ID) AS Qtde " +
                    "FROM tBloqueio (NOLOCK) " +
                    "LEFT JOIN tIngresso (NOLOCK) ON tBloqueio.ID = tIngresso.BloqueioID AND tIngresso.ApresentacaoSetorID=" + this.Control.ID + " AND Status = '" + Ingresso.BLOQUEADO + "' " +
                    "WHERE tBloqueio.LocalID = " + localID + " " +
                    "GROUP BY tBloqueio.ID,tBloqueio.Nome " +
                    "ORDER BY tBloqueio.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Qtde"] = bd.LerInt("Qtde");
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
        /// Retorna um 'stringao' com os lugares dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public override string StatusLugares()
        { //usa essa funcao para ver o q foi vendido

            try
            {

                string sql =

                    "SELECT a.LugarID, SUM(a.QtdeVendida) AS QtdeVendida, SUM(a.QtdeReservada) AS QtdeReservada, " +
                    "SUM(a.QtdeBloqueada) AS QtdeBloqueada, MAX(a.BloqueioID) AS BloqueioID " +
                    "FROM (SELECT LugarID, BloqueioID, " +
                    "CASE WHEN (Status IN (" + StatusNaoDisponiveisVenda() + ")) THEN Count(*) ELSE 0 END AS QtdeVendida, " +
                    "CASE WHEN (Status='" + Ingresso.RESERVADO + "') THEN Count(*) ELSE 0 END AS QtdeReservada, " +
                    "CASE WHEN (BloqueioID<>0 AND Status = '" + Ingresso.BLOQUEADO + "') THEN Count(*) ELSE 0 END AS QtdeBloqueada " +
                    "FROM tIngresso " +
                    "WHERE ApresentacaoSetorID=" + this.Control.ID + " " +
                    "GROUP BY LugarID, BloqueioID, Status) AS a " +
                    "GROUP BY a.LugarID " +
                    "ORDER BY a.LugarID";

                bd.Consulta(sql);

                StringBuilder mapa = new StringBuilder();

                while (bd.Consulta().Read())
                {

                    int lugarID = bd.LerInt("LugarID");
                    int qtdeVendida = bd.LerInt("QtdeVendida");
                    int qtdeReservada = bd.LerInt("QtdeReservada");
                    int qtdeBloqueada = bd.LerInt("QtdeBloqueada");
                    int bloqueioID = bd.LerInt("BloqueioID");
                    if (lugarID == 145399)
                        lugarID = 145399;
                    mapa.Append(lugarID + ":" + qtdeVendida + ":" + qtdeReservada + ":" + qtdeBloqueada + ":" + bloqueioID + "|");

                }
                bd.Fechar();

                return mapa.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string StatusNaoDisponiveisVenda()
        {
            string retorno = "'" + Ingresso.VENDIDO + "','" + Ingresso.PRE_RESERVA + "','" + Ingresso.IMPRESSO + "','" + Ingresso.ENTREGUE + "','" + Ingresso.PREIMPRESSO + "','" + Ingresso.AGUARDANDO_TROCA + "'";
            return retorno;
        }
        public string StatusLugaresMarcados(string apresentacaoSetorID)
        //recebe uma string com a ApresentacaoSetorID das apresentacoes do pacote com lugar marcado
        //e usa essa funcao para ver o q foi vendido
        {
            try
            {

                string sql =
                            @"
                            SELECT a.LugarID, 
                            SUM(a.QntNaoDisponivel) AS QntNaoDisponivel, 
                            SUM(a.QntDisponivel) AS QntDisponivel, 
                            SUM(a.QntReservados) AS QntReservados
                            FROM 
                            (SELECT LugarID,Status, 
                            CASE WHEN (Status='D') THEN Count(*) ELSE 0 END AS QntDisponivel, 
                            CASE WHEN (Status='R') THEN Count(*) ELSE 0 END AS QntReservados, 
                            CASE WHEN (Status!='D') THEN Count(*) ELSE 0 END AS QntNaoDisponivel
                            FROM tIngresso 
                            WHERE ApresentacaoSetorID IN (" + apresentacaoSetorID + @") 
                            GROUP BY LugarID,Status) AS a 
                            GROUP BY LugarID ORDER BY LugarID ";

                bd.Consulta(sql);

                StringBuilder mapa = new StringBuilder();

                while (bd.Consulta().Read())
                {

                    int lugarID = bd.LerInt("LugarID");
                    int QntDisponivel = bd.LerInt("QntDisponivel");
                    int QntNaoDisponivel = bd.LerInt("QntNaoDisponivel");
                    int QntReservados = bd.LerInt("QntReservados");

                    // Três situações de exibição no mapa: Disponível, não disponível e reservado.
                    Nullable<bool> status = null;
                    /// = null : reservados
                    /// = false : não disponível
                    /// = true : disponível

                    if (QntDisponivel == 0 && QntReservados == QntNaoDisponivel) // Reservado (vermelho)
                        status = null;
                    else if (QntNaoDisponivel > 0) // Não disponível (Cinza)
                        status = false;
                    else
                        status = true;

                    if (QntNaoDisponivel >= 1)
                        QntNaoDisponivel = 1;


                    mapa.Append(lugarID + ":" + (status == false ? 1 : 0) + ":" + (status == null ? 1 : 0) + ":0:0|");


                }
                bd.Fechar();

                return mapa.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obter setores de uma apresentacao e um canal especificos
        /// </summary>
        /// <returns></returns>
        public DataTable Setores(int apresentacaoid, int canalid)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

                string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                string sql = "SELECT DISTINCT s.ID,s.Nome,s.Produto,s.LugarMarcado " +
                    "FROM tSetor AS s, tApresentacaoSetor, tCanalPreco AS cp, tPreco AS p " +
                    "WHERE (cp.DataFim >= '" + hoje + "' or cp.DataFim = '') AND tApresentacaoSetor.SetorID=s.ID AND p.ApresentacaoSetorID=tApresentacaoSetor.ID AND " +
                    "cp.PrecoID=p.ID AND cp.CanalID=" + canalid + " AND " +
                    "tApresentacaoSetor.ApresentacaoID=" + apresentacaoid + " " +
                    "ORDER BY s.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter setores de uma apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public override DataTable Setores(int apresentacaoid)
        {
            DataTable tabela = new DataTable("Setor");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Produto", typeof(bool));
            tabela.Columns.Add("LugarMarcado", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tSetor.ID, " +
                    "   tSetor.Nome, " +
                    "   tSetor.Produto, " +
                    "   tSetor.LugarMarcado " +
                    "FROM tSetor (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                    "LEFT JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "WHERE tApresentacaoSetor.ApresentacaoID = " + apresentacaoid + " " +
                    "ORDER BY tSetor.Nome"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Produto"] = bd.LerBoolean("Produto");
                        linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        /// <summary>		
        /// Obter setores de uma apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public DataTable SetoresAtivos(int apresentacaoid)
        {
            DataTable tabela = new DataTable("Setor");
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Produto", typeof(bool));
            tabela.Columns.Add("LugarMarcado", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tSetor.ID, " +
                    "   tSetor.Nome, " +
                    "   tSetor.Produto, " +
                    "   tSetor.LugarMarcado " +
                    "FROM tSetor (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID " +
                    "LEFT JOIN tPreco (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "WHERE tSetor.Ativo = 'T' AND tApresentacaoSetor.ApresentacaoID = " + apresentacaoid + " " +
                    "ORDER BY tSetor.Nome"))
                {
                    DataRow linha;
                    while (oDataReader.Read())
                    {
                        linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Nome");
                        linha["Produto"] = bd.LerBoolean("Produto");
                        linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;
        }

        /// <summary>		
        /// Obter setores de cadeira e pista de uma apresentacao específica
        /// </summary>
        /// <returns></returns>
        public DataTable SetoresPistaCadeira(int apresentacaoid)
        {
            try
            {
                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

                string sql = "SELECT DISTINCT tSetor.ID,tSetor.Nome,tSetor.Produto,tSetor.LugarMarcado " +
                    "FROM tSetor,tApresentacaoSetor,tPreco " +
                    "WHERE tSetor.ID=tApresentacaoSetor.SetorID AND tPreco.ApresentacaoSetorID=tApresentacaoSetor.ID AND " +
                    "(LugarMarcado = 'P' OR LugarMarcado = 'C') AND tApresentacaoSetor.ApresentacaoID=" + apresentacaoid +
                    " ORDER BY tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter setores de lugares marcados de uma apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public override DataTable SetoresMarcados(int apresentacaoid)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

                string sql = "SELECT s.ID,s.Nome,s.LugarMarcado " +
                    "FROM tSetor AS s,tApresentacaoSetor AS tas " +
                    "WHERE s.Produto='F' AND s.ID=tas.SetorID AND " +
                    "s.LugarMarcado<>'" + Setor.Pista + "' AND " +
                    "tas.ApresentacaoID=" + apresentacaoid + " ORDER BY s.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
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
        /// Obter setores de lugares nao marcados de uma apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public override DataTable SetoresNaoMarcados(int apresentacaoid)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

                string sql = "SELECT tSetor.ID,tSetor.Nome,tSetor.Produto " +
                    "FROM tSetor,tApresentacaoSetor " +
                    "WHERE tSetor.ID=tApresentacaoSetor.SetorID AND " +
                    "tSetor.LugarMarcado='" + Setor.Pista + "' AND " +
                    "tApresentacaoSetor.ApresentacaoID=" + apresentacaoid + " ORDER BY tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
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
        /// Obter todos os setores de uma apresentacao especifica
        /// </summary>
        /// <returns></returns>
        public DataTable SetoresTodos(int apresentacaoid)
        {
            try
            {
                DataTable tabela = new DataTable("Setor");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;

                string sql = "SELECT tSetor.ID,tSetor.Nome,tSetor.Produto " +
                    "FROM tSetor,tApresentacaoSetor " +
                    "WHERE tSetor.ID=tApresentacaoSetor.SetorID AND " +
                    "tApresentacaoSetor.ApresentacaoID=" + apresentacaoid + " ORDER BY tSetor.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Produto"] = bd.LerBoolean("Produto");
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
        /// Obter apresentacoes de um setor especifico
        /// </summary>
        /// <returns></returns>
        public override DataTable Apresentacoes(int setorid)
        {

            try
            {

                DataTable tabela = new DataTable("Apresentacao");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Horario", typeof(string));
                tabela.Columns.Add("SetorID", typeof(int)).DefaultValue = setorid;

                string sql = "SELECT ID,Horario FROM tApresentacao,tApresentacaoSetor " +
                    "WHERE tApresentacao.ID=tApresentacaoSetor.ApresentacaoID AND " +
                    "tApresentacaoSetor.SetorID=" + setorid + " ORDER BY tApresentacao.Horario desc";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Horario"] = bd.LerStringFormatoDataHora("Horario");
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
        /// Obter preços dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public override DataTable Precos()
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                string sql = "SELECT tPreco.ID,tPreco.Nome,tPreco.CorID,tPreco.Valor,tCor.RGB " +
                    "FROM tPreco,tCor " +
                    "WHERE tPreco.ApresentacaoSetorID=" + this.Control.ID + " AND tCor.ID=tPreco.CorID " +
                    "ORDER BY tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RGB"] = bd.LerString("RGB");
                    linha["Valor"] = bd.LerDecimal("Valor");
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
        /// Obter preços dessa ApresentacaoSetor
        /// </summary>
        /// <returns></returns>
        public DataTable Precos(string registroZero)
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int)).DefaultValue = this.Control.ID;
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("RGB", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, 0, registroZero, 0, "255255255", 0 });

                string sql = "SELECT tPreco.ID,tPreco.Nome,tPreco.CorID,tPreco.Valor,tCor.RGB " +
                    "FROM tPreco(NOLOCK),tCor(NOLOCK) " +
                    "WHERE tPreco.ApresentacaoSetorID=" + this.Control.ID + " AND tCor.ID=tPreco.CorID " +
                    "ORDER BY tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["RGB"] = bd.LerString("RGB");
                    linha["Valor"] = bd.LerDecimal("Valor");
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
        /// Obter porcentagem de ingressos (separado por status)
        /// Em função do Setor e Apresentacao
        /// </summary>
        public override DataTable PorcentagemIngressosStatus()
        {

            DataTable tabela = Utilitario.EstruturaPorcentagemIngressosStatus();
            try
            {
                DataTable quantidadeIngressosStatus = QuantidadeIngressosStatus();
                decimal total = TotalIngressos();
                foreach (DataRow linha in quantidadeIngressosStatus.Rows)
                {
                    DataRow linhaPorcentagem = tabela.NewRow();
                    linhaPorcentagem["ApresentacaoSetorID"] = linha["ApresentacaoSetorID"];
                    linhaPorcentagem["ApresentacaoID"] = linha["ApresentacaoID"];
                    linhaPorcentagem["SetorID"] = linha["SetorID"];
                    linhaPorcentagem["Status"] = linha["Status"];
                    linhaPorcentagem["Quantidade"] = linha["Quantidade"];
                    linhaPorcentagem["CortesiaID"] = linha["CortesiaID"];
                    linhaPorcentagem["Porcentagem"] = (decimal)(Convert.ToDecimal(linha["Quantidade"]) / total) * 100;
                    tabela.Rows.Add(linhaPorcentagem);
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
        /// Obter Quantidade de Ingressos (separado por status)
        /// Por Apresentacao
        /// </summary>
        public override DataTable QuantidadeIngressosStatus()
        {

            DataTable tabela = Utilitario.EstruturaQuantidadeIngressosStatus();
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        COUNT(tIngresso.ID) AS Quantidade, tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngresso.Status, tApresentacaoSetor.ID, tIngresso.CortesiaID  " +
                    "FROM          tIngresso INNER JOIN " +
                                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "GROUP BY tApresentacaoSetor.ApresentacaoID, tApresentacaoSetor.SetorID, tIngresso.Status, tApresentacaoSetor.ID, tIngresso.CortesiaID " +
                    "HAVING        (tApresentacaoSetor.ID = " + this.Control.ID + ") " +
                    "ORDER BY tIngresso.Status";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ApresentacaoSetorID"] = bd.LerInt("ID");
                    linha["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                    linha["SetorID"] = bd.LerInt("SetorID");
                    linha["Status"] = bd.LerString("Status");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
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
        public int QuantidadePorStatus(string status, bool comCortesia, int apresentacaoSetorID)
        {
            string condicaoCortesia = "";
            if (comCortesia)
                condicaoCortesia = " AND (tIngresso.CortesiaID >= 0) ";
            else
                condicaoCortesia = " AND (tIngresso.CortesiaID = 0) ";
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        tIngresso.ApresentacaoSetorID, COUNT(tIngresso.ID) AS Quantidade " +
                    "FROM            tIngresso " +
                    "WHERE        (tIngresso.Status IN (" + status + ")) " + condicaoCortesia +
                    "GROUP BY tIngresso.ApresentacaoSetorID " +
                    "HAVING        (tIngresso.ApresentacaoSetorID = " + apresentacaoSetorID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = 0;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }		// Quantidade
        public decimal FaturamentoPorStatus(string status, bool comCortesia, int apresentacaoSetorID)
        {
            string condicaoCortesia = "";
            if (comCortesia)
                condicaoCortesia = " AND (tIngresso.CortesiaID >= 0) ";
            else
                condicaoCortesia = " AND (tIngresso.CortesiaID = 0) ";
            decimal total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        tIngresso.ApresentacaoSetorID, SUM(tPreco.Valor) AS Faturamento " +
                    "FROM            tIngresso " +
                    "INNER JOIN tPreco ON tIngresso.PrecoID = tPreco.ID " +
                    "WHERE        (tIngresso.Status IN (" + status + ")) " + condicaoCortesia +
                    "GROUP BY tIngresso.ApresentacaoSetorID " +
                    "HAVING        (tIngresso.ApresentacaoSetorID = " + apresentacaoSetorID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerDecimal("Faturamento");
                }
                else
                {
                    total = 0;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }		// Quantidade
        public override int TotalIngressos()
        {
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        COUNT(tIngresso.ID) AS Quantidade, tApresentacaoSetor.ApresentacaoID " +
                    "FROM            tIngresso INNER JOIN " +
                                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "WHERE        (tApresentacaoSetor.ID = " + this.Control.ID + ") " +
                    "GROUP BY tApresentacaoSetor.ApresentacaoID ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = -1;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }

        /// <summary>		
        /// Obter preços da Apresentacao e Setor passados como parametro
        /// </summary>
        /// <returns></returns>
        public override DataTable Precos(int apresentacaoid, int setorid, bool listarComLote = true)
        {
            //esse metodo evita executar o Ler() antes.
            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("CorID", typeof(int));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadePorCliente", typeof(int));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("Impressao", typeof(string));
                tabela.Columns.Add("IRVende", typeof(bool));
                tabela.Columns.Add("ImprimirCarimbo", typeof(bool));
                tabela.Columns.Add("CarimboTexto1", typeof(string));
                tabela.Columns.Add("CarimboTexto2", typeof(string));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;
                tabela.Columns.Add("SetorID", typeof(int)).DefaultValue = setorid;
                tabela.Columns.Add("PrincipalPrecoID", typeof(bool)).DefaultValue = false;
                tabela.Columns.Add("PrecoTipoID", typeof(int)).DefaultValue = 0;

                string stbSQL = "SELECT tPreco.*, " +
                                "(CASE WHEN tApresentacaoSetor.PrincipalPrecoID = tPreco.ID " +
                                "THEN 'T' " +
                                "ELSE 'F' " +
                                "END) AS Principal FROM tPreco (NOLOCK) " +
                                "INNER JOIN tApresentacaoSetor (NOLOCK) ON tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                                "WHERE tApresentacaoSetor.ApresentacaoID=" + apresentacaoid + " AND " +
                                "tApresentacaoSetor.SetorID=" + setorid + " AND " +
                                "tApresentacaoSetor.ID=tPreco.ApresentacaoSetorID " +
                                (!listarComLote ? "AND tPreco.LoteID IS NULL " : "") +
                                "ORDER BY tPreco.Nome";
                bd.Consulta(stbSQL);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["CorID"] = bd.LerInt("CorID");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["QuantidadePorCliente"] = bd.LerInt("QuantidadePorCliente");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["Impressao"] = bd.LerString("Impressao");
                    linha["IRVende"] = bd.LerBoolean("IRVende");
                    linha["ImprimirCarimbo"] = bd.LerBoolean("ImprimirCarimbo");
                    linha["CarimboTexto1"] = bd.LerString("CarimboTexto1");
                    linha["CarimboTexto2"] = bd.LerString("CarimboTexto2");
                    linha["SetorID"] = setorid;
                    linha["ApresentacaoID"] = apresentacaoid;
                    linha["PrincipalPrecoID"] = bd.LerBoolean("Principal");
                    linha["PrecoTipoID"] = bd.LerInt("PrecoTipoID");
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
        /// Obter preços (que o Canal pode vender) da Apresentacao e Setor passados como parametro
        /// </summary>
        /// <returns></returns>
        public override DataTable PrecosPorCanal(int canalid, int apresentacaoid, int setorid)
        {

            try
            {

                DataTable tabela = new DataTable("Preco");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int)).DefaultValue = apresentacaoid;
                tabela.Columns.Add("SetorID", typeof(int)).DefaultValue = setorid;

                string hoje = System.DateTime.Today.ToString("yyyyMMdd");

                string sql = "SELECT p.ID, p.Nome, p.Valor, p.ApresentacaoSetorID " +
                    "FROM tPreco AS p, tApresentacaoSetor, tCanalPreco AS cp " +
                    "WHERE (cp.DataInicio <= '" + hoje + "' or cp.DataInicio = '') AND (cp.DataFim >= '" + hoje + "' or cp.DataFim = '') AND cp.PrecoID=p.ID AND cp.CanalID=" + canalid + " AND " +
                    "tApresentacaoSetor.ApresentacaoID=" + apresentacaoid + " AND " +
                    "tApresentacaoSetor.SetorID=" + setorid + " AND " +
                    "tApresentacaoSetor.ID=p.ApresentacaoSetorID ORDER BY p.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
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
        /// Pesquisa ApresentacaoSetor.ID em funcao da Apresentacao e Setor
        /// </summary>
        public void PesquisaApresentacaoSetorID()
        {
            try
            {
                BD bd = new BD();
                string sql = "SELECT ID FROM tApresentacaoSetor WHERE " +
                    "ApresentacaoID=" + this.ApresentacaoID.Valor + " AND " +
                    "SetorID=" + this.SetorID.Valor + " ORDER BY ID";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                }
                else
                {
                    Debug.Fail("ApresentacaoSetor.ID nao encontrado, ao pesquisa em funcao da Apresentacao e Setor!!");
                }
                bd.Fechar();
            }
            catch
            {
                Debug.Fail("ApresentacaoSetor.ID nao encontrado, ao pesquisa em funcao da Apresentacao e Setor!!");
            } // fim de try
        } // fim de PesquisaApresentacaoSetorID

        /// <summary>
        /// Preenche todos os atributos de ApresentacaoSetor
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void Ler(BD bd, int id)
        {

            try
            {

                string sql = "SELECT * FROM tApresentacaoSetor WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.SetorID.ValorBD = bd.LerInt("SetorID").ToString();
                    this.ApresentacaoID.ValorBD = bd.LerInt("ApresentacaoID").ToString();
                    this.VersaoImagemIngresso.ValorBD = bd.LerInt("VersaoImagemIngresso").ToString();
                    this.VersaoImagemVale.ValorBD = bd.LerInt("VersaoImagemVale").ToString();
                    this.VersaoImagemVale2.ValorBD = bd.LerInt("VersaoImagemVale2").ToString();
                    this.VersaoImagemVale3.ValorBD = bd.LerInt("VersaoImagemVale3").ToString();
                    this.IngressosGerados.ValorBD = bd.LerString("IngressosGerados");
                    this.CotaID.ValorBD = bd.LerInt("CotaID").ToString();
                    this.Quantidade.ValorBD = bd.LerInt("Quantidade").ToString();
                    this.QuantidadePorCliente.ValorBD = bd.LerInt("QuantidadePorCliente").ToString();
                    this.PrincipalPrecoID.ValorBD = bd.LerInt("PrincipalPrecoID").ToString();
                }
                else
                    this.Limpar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public int DistribuirCotasPorApresentacaoSetor(EstruturaCotasDistribuir oDistribuir)
        {
            CotaItemControle oControle;
            try
            {

                int qtd = 0;
                int apresentacaoSetorID = this.PesquisaApresentacaoSetorID(oDistribuir.ApresentacaoID, oDistribuir.SetorID);

                this.Ler(apresentacaoSetorID);
                bd.FecharConsulta();

                bd.IniciarTransacao();

                //Verifica se a ApresentacaoSetor ja teve alguma cota distribuida.
                this.CotaID.Valor = oDistribuir.CotaID;
                this.Quantidade.Valor = oDistribuir.Quantidade;
                this.QuantidadePorCliente.Valor = oDistribuir.QuantidadePorCliente;

                this.Atualizar(bd);

                oControle = new CotaItemControle();
                oControle.ApresentacaoID.Valor = oDistribuir.ApresentacaoID;
                oControle.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                oControle.Quantidade.Valor = 0;

                List<EstruturaCotaItem> lstReplicar = new List<EstruturaCotaItem>();
                //Encontra os itens que serão replicados
                lstReplicar = oControle.ItensParaReplicar(oDistribuir.CotaID, false);


                //if (redistribuir)
                //{
                //    if (replicarAntigaVenda)
                //    {

                oControle.ExcluirControlador(bd);
                qtd = oControle.InserirControladorReplicando(bd, lstReplicar);
                //    }
                //    else
                //    {
                //        //Setar pra ZERO a quantidade da Cota e dar UPDATE no CotaItemID se necessario?
                //        oControle.ExcluirControlador(bd);
                //        qtd = oControle.InserirControladorPorCotaID(bd, oDistribuir.CotaID);
                //    }
                //}
                //else
                //{
                //    //Insere o Controlador apartir do CotaID ( Ira buscar todos os CotaItems daquela Cota para adicionar um por um.
                //    qtd = oControle.InserirControladorPorCotaID(bd, oDistribuir.CotaID);
                //}
                bd.FinalizarTransacao();
                return 1;
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

        public int PesquisaApresentacaoSetorID(int apresentacaoID, int setorID)
        {
            try
            {
                string sql = "SELECT top 1 ID FROM tApresentacaoSetor WHERE " +
                    "ApresentacaoID=" + apresentacaoID + " AND " +
                    "SetorID=" + setorID + " ORDER BY ID";
                return Convert.ToInt32(bd.ConsultaValor(sql));
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

        /// <summary>
        /// Pesquisa apresentacoesID apartir de uma lista de apresentacoes e um setorID
        /// </summary>
        /// <param name="apresentacoes"></param>
        /// <param name="setorID"></param>
        /// <returns></returns>
        public List<int> PesquisaApresentacaoSetorID(List<int> apresentacoes, int setorID)
        {
            try
            {
                List<int> lista = new List<int>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ID FROM tApresentacaoSetor (NOLOCK)");
                stbSQL.Append("WHERE SetorID = " + setorID + " AND (");
                //for (int i = 0; i < apresentacoes.Count; i++)
                stbSQL.Append("ApresentacaoID IN (" + Utilitario.ArrayToString(apresentacoes.ToArray()) + ")");

                stbSQL.Remove(stbSQL.Length - 3, 3);
                stbSQL.Append(" )");

                bd.Consulta(stbSQL.ToString());
                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

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

        /// <summary>
        /// Exclui os ingressos desta Apresentacao Setor 
        /// Chamar Excluir Cascata do Ingresso e Preco
        /// </summary>
        /// <returns>Informa se a exclusão ocorreu com sucesso</returns>
        public override bool ExcluirCascata()
        {
            bool excluiuTodosIngressos = true;
            bool excluiuTodosPrecos = true;
            bool excluiuTudo = false;
            try
            {
                #region Excluir Cascata de cada Preco desta Apresentacao Setor
                // Preco precisa ser excluído antes do Ingresso, pois no Ingresso tem ID do Preco
                PrecoLista precoLista = new PrecoLista();
                precoLista.FiltroSQL = "ApresentacaoSetorID= " + this.Control.ID;
                precoLista.Carregar();
                precoLista.Primeiro();
                int contadorPreco = 0;
                if (precoLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodosPrecos = excluiuTodosPrecos && precoLista.Preco.ExcluirCascata();
                        contadorPreco++;
                        precoLista.Proximo();
                    } while (excluiuTodosPrecos && contadorPreco < precoLista.Tamanho);
                }
                #endregion
                #region Excluir Cascata de cada Ingresso desta Apresentacao Setor
                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "ApresentacaoSetorID= " + this.Control.ID;
                ingressoLista.Carregar();
                ingressoLista.Primeiro();
                int contador = 0;
                if (ingressoLista.Tamanho > 0)
                {
                    do
                    {
                        excluiuTodosIngressos = excluiuTodosIngressos && ingressoLista.Ingresso.ExcluirCascata();
                        contador++;
                        ingressoLista.Proximo();
                    } while (excluiuTodosIngressos && contador < ingressoLista.Tamanho);
                }
                #endregion
                // Excluir esta Apresentacao Setor
                if (excluiuTodosIngressos && excluiuTodosPrecos)
                    excluiuTudo = this.Excluir();
                // Retorna sucesso se todas as operações forem sucesso
                return excluiuTudo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Estrutura do DataTable do VendaItem()
        /// </summary>
        public DataTable EstruturaVendaItem()
        {
            DataTable tabela = new DataTable("VendaItem");
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
            tabela.Columns.Add("IngressoID", typeof(int));
            tabela.Columns.Add("VendaBilheteriaItemID", typeof(int));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("PacoteID", typeof(int));
            tabela.Columns.Add("TaxaConveniencia", typeof(int));
            tabela.Columns.Add("TaxaConvenienciaValor", typeof(decimal));
            tabela.Columns.Add("ValorItem", typeof(decimal));
            tabela.Columns.Add("VendaBilheteriaID", typeof(int));
            return tabela;
        }
        /// <summary>
        /// Obtém os ingressos em função do Item de venda por ApresenatcaoSetor
        /// Leva em conta o Pacote por isso não repete os ingressos de um Item de pacote
        /// </summary>
        public override DataTable VendaItem(string apresentacaoSetorIDs)
        {
            try
            {
                // Filtrando por Apresentacao e Setor
                IngressoLista ingressoLista = new IngressoLista();
                ingressoLista.FiltroSQL = "ApresentacaoSetorID in (" + apresentacaoSetorIDs + ")";
                ingressoLista.FiltroSQL = "Status in ('V', 'I', 'E')";
                ingressoLista.Carregar();
                ingressoLista.Primeiro();
                string[] itemVetor = ingressoLista.ItemIDs().Split(',');

                int[] itemIDsVetor = CTLib.Utilitario.VetorStringParaInteiro(itemVetor);
                // Para cada item ID checar se é pacote
                // E insere informações no DataTable
                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                DataTable tabela = EstruturaVendaItem();
                int ingressoID;
                DataTable tabelaEventoApresentacaoSetor;
                foreach (int itemID in itemIDsVetor)
                {
                    vendaBilheteriaItem.Control.ID = itemID;
                    vendaBilheteriaItem.Ler(itemID);
                    ingressoID = vendaBilheteriaItem.PrimeiroIngressoID();
                    Ingresso ingresso = new Ingresso();
                    ingresso.Ler(ingressoID);
                    tabelaEventoApresentacaoSetor = ingresso.EventoApresentacaoSetor();
                    if (tabelaEventoApresentacaoSetor.Rows.Count == 1)
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ApresentacaoSetorID"] = tabelaEventoApresentacaoSetor.Rows[0]["ApresentacaoSetorID"];
                        linha["IngressoID"] = ingressoID;
                        linha["VendaBilheteriaItemID"] = itemID;
                        linha["PacoteID"] = vendaBilheteriaItem.PacoteID.Valor;
                        linha["PrecoID"] = tabelaEventoApresentacaoSetor.Rows[0]["PrecoID"];
                        linha["TaxaConveniencia"] = vendaBilheteriaItem.TaxaConveniencia.Valor;
                        linha["TaxaConvenienciaValor"] = vendaBilheteriaItem.TaxaConvenienciaValor.Valor;
                        Preco preco = new Preco();
                        preco.Ler(Convert.ToInt32(linha["PrecoID"]));
                        linha["Preco"] = preco.Nome.Valor;
                        if (Convert.ToInt32(linha["PacoteID"]) <= 0)
                        { // não é pacote
                            linha["ValorItem"] = preco.Valor.Valor;
                        }
                        else
                        { // pacote
                            Pacote pacote = new Pacote();
                            pacote.Ler(Convert.ToInt32(linha["PacoteID"]));
                            linha["ValorItem"] = pacote.Valor();
                        }
                        linha["VendaBilheteriaID"] = vendaBilheteriaItem.VendaBilheteriaID.Valor;
                        tabela.Rows.Add(linha);
                    }
                }
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoverDistribuicaoCotaPorApresentacaoSetor(int apresentacaoSetorID)
        {
            CotaItemControle oCotaItem = new CotaItemControle();
            try
            {
                bd.IniciarTransacao();

                this.Ler(apresentacaoSetorID);
                this.CotaID.Valor = 0;
                this.Quantidade.Valor = 0;
                this.QuantidadePorCliente.Valor = 0;

                oCotaItem.ApresentacaoSetorID.Valor = apresentacaoSetorID;
                oCotaItem.ApresentacaoID.Valor = this.ApresentacaoID.Valor;
                oCotaItem.ExcluirControlador(bd);

                bool ok = this.Atualizar(bd);

                bd.FinalizarTransacao();

                return ok;
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

        public void RemoverDistribuicaoCotaPorApresentacaoSetor(int[] apresentacaoSetorID)
        {
            try
            {
                CotaItemControle oControle = new CotaItemControle();
                bd.IniciarTransacao();
                for (int i = 0; i < apresentacaoSetorID.Length; i++)
                {
                    this.Ler(apresentacaoSetorID[i]);
                    this.CotaID.Valor = 0;
                    this.Quantidade.Valor = 0;
                    this.QuantidadePorCliente.Valor = 0;

                    oControle.ApresentacaoID.Valor = this.ApresentacaoID.Valor;
                    oControle.ApresentacaoSetorID.Valor = apresentacaoSetorID[i];
                    oControle.ExcluirControlador(bd);

                    this.Atualizar(bd);
                }
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

        /// <summary>
        /// Inserir novo(a) ApresentacaoSetor
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(CTLib.BD database)
        {

            try
            {
                this.Control.Versao = 0;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tApresentacaoSetor(SetorID, ApresentacaoID, VersaoImagemIngresso, VersaoImagemVale, VersaoImagemVale2, VersaoImagemVale3, IngressosGerados, CotaID, Quantidade, QuantidadePorCliente, PrincipalPrecoID) ");
                sql.Append("VALUES (@001,@002,@003,@004,@005,@006,'@007',@008,@009,@010,@011); SELECT SCOPE_IDENTITY();");

                sql.Replace("@001", this.SetorID.ValorBD);
                sql.Replace("@002", this.ApresentacaoID.ValorBD);
                sql.Replace("@003", this.VersaoImagemIngresso.ValorBD);
                sql.Replace("@004", this.VersaoImagemVale.ValorBD);
                sql.Replace("@005", this.VersaoImagemVale2.ValorBD);
                sql.Replace("@006", this.VersaoImagemVale3.ValorBD);
                sql.Replace("@007", this.IngressosGerados.ValorBD);
                sql.Replace("@008", this.CotaID.ValorBD);
                sql.Replace("@009", this.Quantidade.ValorBD);
                sql.Replace("@010", this.QuantidadePorCliente.ValorBD);
                sql.Replace("@011", this.PrincipalPrecoID.ValorBD);

                object x = database.ConsultaValor(sql.ToString());
                this.Control.ID = (x != null) ? Convert.ToInt32(x) : 0;

                bool result = this.Control.ID > 0;

                if (result)
                    InserirControle("I", database);


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ApresentacaoSetor> InsereSetores(int[] apresentacoes, int[] setores, BD bd)
        {
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Apresentacoes", Utilitario.ArrayToString(apresentacoes)));
            parametros.Add(new SqlParameter("@Setores", Utilitario.ArrayToString(setores)));

            var reader = bd.Consulta("EXEC GerarEvento_InserirSetores @Apresentacoes, @Setores", parametros);

            List<ApresentacaoSetor> setoresInseridos = new List<ApresentacaoSetor>();

            while (reader.Read())
            {
                var setor = new ApresentacaoSetor();

                setor.Control.ID = bd.LerInt("ID");
                setor.ApresentacaoID.Valor = bd.LerInt("ApresentacaoID");
                setor.SetorID.Valor = bd.LerInt("SetorID");

                //Utils.Enums.ParseCharEnum<Setor.Tipo>(bd.LerString("LugarMarcado"))
                setor.TipoSetor = Utils.Enums.ParseCharEnum<Setor.Tipo>(bd.LerString("LugarMarcado"));
                setoresInseridos.Add(setor);
            }

            return setoresInseridos;
        }


        protected void InserirControle(string acao, CTLib.BD database)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cApresentacaoSetor (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                database.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal void AtualizarIngressosGerados(BD bd)
        {
            try
            {
                if (this.Control.ID < 1)
                    throw new Exception("ApresentacaoSetor ID inválido.");

                bd.Executar("UPDATE tApresentacaoSetor SET IngressosGerados = 'T' WHERE ID = " + this.Control.ID);
            }
            catch
            {
                throw;
            }
        }

        public void AtualizarPrecoPrincipal(BD bd, int precoID, int apresentacaoSetorID = 0)
        {

            if (this.Control.ID == 0)
                this.Control.ID = apresentacaoSetorID;

            bd.Executar(string.Format("UPDATE tApresentacaoSetor SET PrincipalPrecoID = {0} WHERE ID = {1}", precoID, this.Control.ID));

        }

        public void AtualizarPrecoPrincipal(int precoID, int apresentacaoSetorID = 0)
        {
            try {
                bd.IniciarTransacao();
                if (this.Control.ID == 0)
                    this.Control.ID = apresentacaoSetorID;

                bd.Executar(string.Format("UPDATE tApresentacaoSetor SET PrincipalPrecoID = {0} WHERE ID = {1}", precoID, this.Control.ID));

                bd.FinalizarTransacao();}
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
            }

        }

        public EstruturaInfoEventoMondial ConsultaInfoEvento(int ApresentacaoID)
        {
            try
            {
                EstruturaInfoEventoMondial estruturaInfo = new EstruturaInfoEventoMondial();

                string sql = string.Format(@"SELECT tApresentacao.ID AS ApresentacaoID, Horario, tEvento.Nome AS Evento, tLocal.Nome AS Local, tSetor.ID AS SetorID, tSetor.Nome AS Setor, tPreco.Valor
                                FROM tApresentacaoSetor (NOLOCK)
                                INNER JOIN tApresentacao (NOLOCK) on tApresentacao.ID = tApresentacaoSetor.ApresentacaoID
                                INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID
                                INNER JOIN tLocal (NOLOCK) ON tEvento.LocalID = tLocal.ID
                                INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID
                                INNER JOIN tPreco (NOLOCK) ON tPreco.ApresentacaoSetorID = tApresentacaoSetor.ID
                                WHERE tApresentacaoSetor.ID = {0}", ApresentacaoID);

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    estruturaInfo = new EstruturaInfoEventoMondial()
                    {
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Horario = bd.LerDateTime("Horario"),
                        Evento = bd.LerString("Evento"),
                        Local = bd.LerString("Local"),
                        Setor = bd.LerString("Setor"),
                        SetorID = bd.LerInt("SetorID"),
                        Valor = bd.LerDecimal("Valor"),
                    };
                }

                bd.Fechar();

                return estruturaInfo;
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

        public DataTable ConsultarPrecos(List<int> apresentacoesIDs, List<int> setoresIDs)
        {
            CTLib.BD bd = new BD();
            try
            {
                string query = @"SELECT DISTINCT tPreco.Nome FROM tPreco (NOLOCK)
                                 INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID 
                                 where tApresentacaoSetor.ApresentacaoID  in (" + string.Join(",", apresentacoesIDs) + @") 
                                 AND tApresentacaoSetor.SetorID in (" + string.Join(",", setoresIDs) + ")  order by tPreco.nome";
                
                bd.Consulta(query);

                DataTable dt = new DataTable("Preco");
                //dt.Columns.Add("ID");
                dt.Columns.Add("Naturezas de Preços");

                while (bd.Consulta().Read())
                {
                    //bd.LerInt("ID"),
                    dt.Rows.Add(new object[] { bd.LerString("Nome") });
                }

                return dt;
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

        public DataTable ConsultarPrecos(List<int> apresentacoesIDs, List<int> setoresIDs, List<string> precosNomes)
        {
            CTLib.BD bd = new BD();
            try
            {
                string query = @"SELECT tPreco.ID as 'ID',tPreco.nome as 'Preco',tPreco.valor as 'Valor', tApresentacao.Horario as 'Horario' ,tSetor.nome as 'Setor',
                                 (SELECT Count(tIngresso.ID) FROM tIngresso (NOLOCK) WHERE tIngresso.precoID = tPreco.ID  AND Status IN ('V', 'I')) 
									as 'Ingressos' 
                                 FROM tPreco (NOLOCK)
                                 INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID
                                 INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID
                                 INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID
								 LEFT JOIN tIngresso(NOLOCK) ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID and tIngresso.Status in ('V','I')
                                 WHERE tApresentacaoSetor.ApresentacaoID in (" + string.Join(",", apresentacoesIDs.ToArray()) + @") 
                                 AND tApresentacaoSetor.SetorID in (" + string.Join(",", setoresIDs.ToArray()) + @") 
                                 AND tPreco.Nome in (" + string.Join(",", precosNomes.Select(t => "'" + t + "'").ToArray()) + @")
                                 group by tPreco.ID,tPreco.nome,tPreco.Valor,tApresentacao.Horario,tSetor.nome
								 order by tPreco.nome,tSetor.Nome,tApresentacao.Horario";
                
                bd.Consulta(query);

                DataTable dt = new DataTable("Preco");
                dt.Columns.Add("ID");
                dt.Columns.Add("Nome do Preço");
                dt.Columns.Add("Valor");
                dt.Columns.Add("Apresentação");
                dt.Columns.Add("Setor");
                dt.Columns.Add("Ingressos");


                while (bd.Consulta().Read())
                {
                    dt.Rows.Add(new object[] { bd.LerInt("ID"), bd.LerString("Preco"), bd.LerDecimal("Valor"), bd.LerStringFormatoSemanaDataHora("Horario"), bd.LerString("Setor"), bd.LerString("Ingressos") });
                }

                return dt;
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

    } // fim de ApresentacaoSetor


    public class ApresentacaoSetorLista : ApresentacaoSetorLista_B
    {

        public ApresentacaoSetorLista() { }

        public ApresentacaoSetorLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


    }

}
