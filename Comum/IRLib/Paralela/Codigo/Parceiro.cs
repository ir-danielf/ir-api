/**************************************************
* Arquivo: Parceiro.cs
* Gerado: 06/12/2009
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

    public class Parceiro : Parceiro_B
    {

        public Parceiro() { }

        public Parceiro(int usuarioIDLogado) { }

        public int BuscaTipo(int ParceiroID)
        {
            try
            {
                string sql = "SELECT Tipo FROM tParceiro (NOLOCK) WHERE ID = " + ParceiroID;
                int Tipo = 0;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    Tipo = bd.LerInt("Tipo");

                return Tipo;
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

        public List<EstruturaParceiro> getParceiros(bool indiceZero)
        {
            try
            {

                List<EstruturaParceiro> listaParceiro = new List<EstruturaParceiro>();
                string sql = "SELECT ID, Parceiro, Tipo FROM tParceiro (NOLOCK) ORDER BY Parceiro";
                EstruturaParceiro parceiro;
                bd.Consulta(sql);
                if (indiceZero)
                {
                    parceiro = new EstruturaParceiro();
                    parceiro.ID = 0;
                    parceiro.Parceiro = "Nenhum";
                    parceiro.Tipo = -1;
                    listaParceiro.Add(parceiro);
                }

                while (bd.Consulta().Read())
                {
                    parceiro = new EstruturaParceiro();
                    parceiro.ID = bd.LerInt("ID");
                    parceiro.Parceiro = bd.LerString("Parceiro");
                    parceiro.Tipo = bd.LerInt("Tipo");
                    listaParceiro.Add(parceiro);
                }

                return listaParceiro;
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

        public List<EstruturaParceiroNovo> getParceirosListagem(string filtro)
        {
            try
            {
                List<EstruturaParceiroNovo> listaParceiro = new List<EstruturaParceiroNovo>();

                StringBuilder stb = new StringBuilder();
                stb.Append("CREATE TABLE #tmp (ParceiroID INT,Parceiro NVARCHAR(300),Tipo INT,Quantidade INT, Url NVARCHAR(200))");

                stb.Append("INSERT INTO #tmp ");
                stb.Append("SELECT p.ID, Parceiro, Tipo, COUNT(tBin.ID) AS Quantidade, Url ");
                stb.Append("    FROM tParceiro p (NOLOCK)");
                stb.Append("    LEFT JOIN tBin (NOLOCK) ON  tBin.ParceiroID = p.ID ");
                stb.Append("WHERE Tipo = 1 ");
                if (filtro.Length > 0)
                    stb.Append(" AND Parceiro LIKE '%" + filtro + "%' ");
                stb.Append("    GROUP BY p.ID, p.Parceiro, Tipo, Url ");

                stb.Append("INSERT INTO #tmp ");
                stb.Append("SELECT p.ID, Parceiro, Tipo, COUNT(tCodigoPromo.ID) AS Quantidade, Url");
                stb.Append("    FROM tParceiro p (NOLOCK)");
                stb.Append("    LEFT JOIN tCodigoPromo (NOLOCK) ON  tCodigoPromo.ParceiroID = p.ID ");
                stb.Append("WHERE Tipo = 2 ");
                if (filtro.Length > 0)
                    stb.Append(" AND Parceiro LIKE '%" + filtro + "%' ");
                stb.Append("    GROUP BY p.ID, p.Parceiro, Tipo, Url ");

                stb.Append("INSERT INTO #tmp ");
                stb.Append("SELECT p.ID, Parceiro, Tipo, 0 as Quantidade , Url");
                stb.Append("    FROM tParceiro p (NOLOCK)");
                stb.Append("WHERE Tipo = 3 ");
                if (filtro.Length > 0)
                    stb.Append(" AND Parceiro LIKE '%" + filtro + "%' ");
                stb.Append("    GROUP BY p.ID, p.Parceiro, Tipo, Url ");

                stb.Append("SELECT * FROM #tmp ORDER BY Parceiro ");
                stb.Append("DROP TABLE #tmp");

                bd.Consulta(stb.ToString());

                while (bd.Consulta().Read())
                {
                    listaParceiro.Add(new EstruturaParceiroNovo()
                    {
                        ID = bd.LerInt("ParceiroID"),
                        Parceiro = bd.LerString("Parceiro"),
                        Tipo = (Enumerators.TipoParceiro)Enum.Parse(typeof(Enumerators.TipoParceiro), bd.LerString("Tipo"), true),
                        QuantidadeDistribuida = bd.LerInt("Quantidade"),
                        Url = bd.LerString("Url"),
                    });
                }

                return listaParceiro;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Atualizar(EstruturaParceiroNovo parceiro, Dictionary<int, string> listaCodigos, bool limparCodigos)
        {
            BD bdAux = new BD();
            try
            {

                bd.IniciarTransacao();

                this.Parceiro.Valor = parceiro.Parceiro;
                this.Tipo.Valor = (int)parceiro.Tipo;
                this.Url.Valor = parceiro.Url;

                this.Control.ID = parceiro.ID;

                this.Atualizar(bd);

                Dictionary<int, string> listaCodigosAntigos = new Dictionary<int, string>();
                Dictionary<int, string> listaCodigoInserir = new Dictionary<int, string>();
                //Excluir os códigos se tiver marcado pra excluir e/ou o tipo for diferente do selecionado
                if (limparCodigos || parceiro.Tipo == Enumerators.TipoParceiro.Bin)
                    bd.Executar("DELETE FROM tCodigoPromo WHERE ParceiroID = " + parceiro.ID);
                else
                {
                    bdAux.Consulta("SELECT ID, BIN FROM tBin (NOLOCK) WHERE ParceiroID = " + parceiro.ID);
                    while (bdAux.Consulta().Read())
                        listaCodigosAntigos.Add(bdAux.LerInt("ID"), bdAux.LerString("BIN"));
                }

                if (limparCodigos || parceiro.Tipo == Enumerators.TipoParceiro.Codigo)
                    bd.Executar("DELETE FROM tBin WHERE ParceiroID = " + parceiro.ID);
                else
                {
                    bdAux.Consulta("SELECT ID, Codigo FROM tCodigoPromo (NOLOCK) WHERE ParceiroID = " + parceiro.ID);
                    while (bdAux.Consulta().Read())
                        listaCodigosAntigos.Add(bdAux.LerInt("ID"), bdAux.LerString("Codigo"));
                }
                bdAux.FecharConsulta();

                //Se limpou os códigos, só inclui a lista inteira
                if (limparCodigos && listaCodigos.Count > 0)
                    this.InserirCodigos(bd, parceiro.ID, parceiro.Tipo, listaCodigos);
                //Do contrario, pega os itens que não são duplicados e inclui
                else if (listaCodigos.Count > 0)
                {
                    foreach (var codigo in listaCodigos)
                        if (!listaCodigosAntigos.ContainsValue(codigo.Value))
                            listaCodigoInserir.Add(codigo.Key, codigo.Value);

                    if (listaCodigoInserir.Count > 0)
                        this.InserirCodigos(bd, parceiro.ID, parceiro.Tipo, listaCodigoInserir);
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
                bdAux.Fechar();
                bd.Fechar();
            }
        }

        public void Inserir(EstruturaParceiroNovo parceiro, Dictionary<int, string> listaCodigos)
        {
            try
            {
                bd.IniciarTransacao();

                this.Parceiro.Valor = parceiro.Parceiro;
                this.Tipo.Valor = (int)parceiro.Tipo;
                this.Url.Valor = parceiro.Url;

                this.Inserir(bd);

                if (listaCodigos.Count > 0)
                    this.InserirCodigos(bd, this.Control.ID, parceiro.Tipo, listaCodigos);

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

        public void Excluir(EstruturaParceiroNovo parceiro)
        {
            try
            {
                bd.IniciarTransacao();
                bd.Executar(
                    "DELETE FROM tCodigoPromo WHERE ParceiroID = " + parceiro.ID);
                bd.Executar(
                    "DELETE FROM tBin WHERE ParceiroID = " + parceiro.ID);
                bd.Executar(
                    "UPDATE tCotaItem SET ParceiroID = 0 WHERE ParceiroID = " + parceiro.ID);

                this.Excluir(bd, parceiro.ID);

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

        public void InserirCodigos(BD bd, int ParceiroID, Enumerators.TipoParceiro tipo, Dictionary<int, string> listaInserir)
        {
            DataTable dttBulk = new DataTable();
            DataRow dtr;
            switch (tipo)
            {
                case Enumerators.TipoParceiro.Codigo:
                    dttBulk.Columns.Add("ID", typeof(int)).DefaultValue = 0;
                    dttBulk.Columns.Add("ParceiroID", typeof(int));
                    dttBulk.Columns.Add("Codigo", typeof(string));
                    dttBulk.Columns.Add("TamanhoCodigo", typeof(string));
                    foreach (var str in listaInserir)
                    {
                        dtr = dttBulk.NewRow();
                        dtr["ParceiroID"] = ParceiroID;
                        dtr["Codigo"] = str.Value;
                        dtr["TamanhoCodigo"] = str.Value.Length;
                        dttBulk.Rows.Add(dtr);
                    }

                    bd.BulkInsert(dttBulk, "tCodigoPromo", false, false);
                    break;
                case Enumerators.TipoParceiro.Bin:
                    dttBulk.Columns.Add("ID", typeof(int)).DefaultValue = 0;
                    dttBulk.Columns.Add("BIN", typeof(string));
                    dttBulk.Columns.Add("ParceiroID", typeof(int)).DefaultValue = ParceiroID;
                    foreach (var str in listaInserir)
                    {
                        dtr = dttBulk.NewRow();
                        dtr["Bin"] = str.Value;
                        dtr["ParceiroID"] = ParceiroID;
                        dttBulk.Rows.Add(dtr);
                    }
                    bd.BulkInsert(dttBulk, "tBin", false, false);

                    break;
            }
        }

        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cParceiro (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xParceiro (ID, Versao, Parceiro, Tipo) ");
                sql.Append("SELECT ID, @V, Parceiro, Tipo FROM tParceiro WHERE ID = @I");
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
        /// Inserir novo(a) Parceiro
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cParceiro");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tParceiro(ID, Parceiro, Tipo, Url) ");
                sql.Append("VALUES (@ID,'@001',@002, '@003')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Parceiro.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.Url.ValorBD);

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
        /// Atualiza Parceiro
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cParceiro WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tParceiro SET Parceiro = '@001', Tipo = @002, Url = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.Parceiro.ValorBD);
                sql.Replace("@002", this.Tipo.ValorBD);
                sql.Replace("@003", this.Url.ValorBD);

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
        /// Exclui Parceiro com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cParceiro WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "D");
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tParceiro WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class ParceiroLista : ParceiroLista_B
    {

        public ParceiroLista() { }

        public ParceiroLista(int usuarioIDLogado) { }

    }

}
