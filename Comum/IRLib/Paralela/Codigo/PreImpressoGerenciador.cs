using CTLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela
{

    /// <summary>
    /// Gerenciador do PreImpressoGerenciador
    /// </summary>
    public class PreImpressoGerenciador : MarshalByRefObject, ISponsoredObject
    {
        private BD bd;

        public const string EVENTO = "Evento";
        public const string APRESENTACAO = "Horário";
        public const string SETOR = "Setor";
        public const string PRECO = "Preço";
        public const string QTDE = "Qtd";
        public const string VALOR = "Valor";

        private DataTable info;
        private DataTable info2;
        private DataTable resumo;

        //Essas tabelas diferenciam os ingressos por tipo de setor
        private DataTable ingressosMesaFechada = EstrutraIngressosTipos();
        private DataTable ingressosMesaAberta = EstrutraIngressosTipos();
        private DataTable ingressosCadeira = EstrutraIngressosTipos();
        private DataTable ingressosPista = EstrutraIngressosTipos();
        private DataTable ingressosPacote = EstrutraIngressosTipos();

        public PreImpressoGerenciador()
        {
            bd = new BD();
        }

        public static DataTable EstrutraIngressos()
        {

            DataTable ingressos = new DataTable();
            ingressos.Columns.Add("IngressoID", typeof(int));
            ingressos.Columns.Add("CodigoBarra", typeof(string));
            ingressos.Columns.Add("Codigo", typeof(string));
            ingressos.Columns.Add("ApresentacaoSetorID", typeof(int));
            ingressos.Columns.Add("TipoSetor", typeof(string)); //Pista,MesaFechada,MesaAberta,Cadeira
            ingressos.Columns.Add("PrecoID", typeof(int));
            ingressos.Columns.Add("Quantidade", typeof(int));
            ingressos.Columns.Add("PacoteID", typeof(int));
            ingressos.Columns.Add("Classificacao", typeof(string));

            return ingressos;

        }

        public static DataTable EstruturaExibicao()
        {
            DataTable retorno = new DataTable();

            retorno.Columns.Add("Evento", typeof(string));
            retorno.Columns.Add("Apresentacao", typeof(string));
            retorno.Columns.Add("Setor", typeof(string));
            retorno.Columns.Add("Preco", typeof(string));
            retorno.Columns.Add("Valor", typeof(string));
            retorno.Columns.Add("Quantidade", typeof(int));
            retorno.Columns.Add("Canal", typeof(string));
            retorno.Columns.Add("Loja", typeof(string));
            retorno.Columns.Add("PacoteID", typeof(int));

            return retorno;
        }

        public static DataTable EstrutraIngressosTipos()
        {
            DataTable ingressos = new DataTable();

            ingressos.Columns.Add("ID", typeof(int));
            ingressos.Columns.Add("IngressoID", typeof(int));
            ingressos.Columns.Add("PrecoID", typeof(int));
            ingressos.Columns.Add("PacoteGrupo", typeof(string));
            ingressos.Columns.Add("LugarID", typeof(int));
            ingressos.Columns.Add("PacoteID", typeof(int));
            ingressos.Columns.Add("BloqueioID", typeof(int));
            ingressos.Columns.Add("CortesiaID", typeof(int));
            ingressos.Columns.Add("Valor", typeof(decimal));
            ingressos.Columns.Add("Conv", typeof(int));
            ingressos.Columns.Add("ValorConv", typeof(decimal));
            ingressos.Columns.Add("TaxaMaxima", typeof(decimal));
            ingressos.Columns.Add("TaxaMinima", typeof(decimal));
            ingressos.Columns.Add("TaxaComissao", typeof(int));
            ingressos.Columns.Add("ValorComissao", typeof(decimal));
            ingressos.Columns.Add("ComissaoMaxima", typeof(decimal));
            ingressos.Columns.Add("ComissaoMinima", typeof(decimal));
            ingressos.Columns.Add("LugarMarcado", typeof(string));
            ingressos.Columns.Add("CodigoBarra", typeof(string));

            return ingressos;
        }

        public DataTable InfoTransferenciaLoja
        {
            get
            {
                //devolver agrupado

                DataTable info = this.info.Clone();

                DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(this.info, new string[] { EVENTO, APRESENTACAO, SETOR, PRECO });
                foreach (DataRow linha in tabelaTmp.Rows)
                {

                    string Evento = (string)linha[EVENTO];
                    string apresentacao = (string)linha[APRESENTACAO];
                    string setor = (string)linha[SETOR];
                    string Preco = (string)linha[PRECO];

                    object Ovalor = this.info.Compute("SUM(" + VALOR + ")", EVENTO + "='" + Evento + "' AND " + APRESENTACAO + "='" + apresentacao + "' AND " + SETOR + "='" + setor + "' AND " + PRECO + "='" + Preco + "'");
                    object Oqtde = this.info.Compute("SUM(" + QTDE + ")", EVENTO + "='" + Evento + "' AND " + APRESENTACAO + "='" + apresentacao + "' AND " + SETOR + "='" + setor + "' AND " + PRECO + "='" + Preco + "'");

                    decimal valor = (Ovalor != DBNull.Value) ? Convert.ToDecimal(Ovalor) : 0;
                    int qtde = (Oqtde != DBNull.Value) ? Convert.ToInt32(Oqtde) : 0;

                    DataRow linhaInfo = info.NewRow();
                    linhaInfo[EVENTO] = Evento;
                    linhaInfo[APRESENTACAO] = apresentacao;
                    linhaInfo[SETOR] = setor;
                    linhaInfo[PRECO] = Preco;
                    linhaInfo[QTDE] = qtde;
                    linhaInfo[VALOR] = valor;
                    info.Rows.Add(linhaInfo);

                }

                return info;

            }
        }

        public DataTable InfoTransferenciaLoja2
        {
            get
            {
                //devolver agrupado

                DataTable info = this.info2.Clone();

                DataTable tabelaTmp = CTLib.TabelaMemoria.Distinct(this.info2, new string[] { EVENTO, APRESENTACAO, SETOR, PRECO });
                foreach (DataRow linha in tabelaTmp.Rows)
                {

                    string Evento = (string)linha[EVENTO];
                    string apresentacao = (string)linha[APRESENTACAO];
                    string setor = (string)linha[SETOR];
                    string Preco = (string)linha[PRECO];

                    object Ovalor = this.info2.Compute("SUM(" + VALOR + ")", EVENTO + "='" + Evento + "' AND " + APRESENTACAO + "='" + apresentacao + "' AND " + SETOR + "='" + setor + "' AND " + PRECO + "='" + Preco + "'");
                    object Oqtde = this.info2.Compute("SUM(" + QTDE + ")", EVENTO + "='" + Evento + "' AND " + APRESENTACAO + "='" + apresentacao + "' AND " + SETOR + "='" + setor + "' AND " + PRECO + "='" + Preco + "'");

                    decimal valor = (Ovalor != DBNull.Value) ? Convert.ToDecimal(Ovalor) : 0;
                    int qtde = (Oqtde != DBNull.Value) ? Convert.ToInt32(Oqtde) : 0;

                    DataRow linhaInfo = info.NewRow();
                    linhaInfo[EVENTO] = Evento;
                    linhaInfo[APRESENTACAO] = apresentacao;
                    linhaInfo[SETOR] = setor;
                    linhaInfo[PRECO] = Preco;
                    linhaInfo[QTDE] = qtde;
                    linhaInfo[VALOR] = valor;
                    info.Rows.Add(linhaInfo);

                }

                return info;

            }
        }

        /// <summary>
        /// Transferir ingressos com codigo de barra para 2 lojas mas antes verificar se os mesmos pertencem a loja indicada
        /// </summary>
        /// <returns></returns>
        public int[] Transferir(int lojaIDDe, DataTable ingressos, int lojaIDPara, int canalIDPara, int empresaIDPara, int usuarioID, int lojaID2Para, int canalID2Para, int empresaID2Para)
        {

            int[] qtdes = new int[2];

            int qtde1 = 0;
            int qtde2 = 0;

            DataRow[] ingressosLotePreco = ingressos.Select("PrecoID<>0");
            DataRow[] ingressosCodigo = ingressos.Select("PrecoID=0");

            //construindo AS estruturas
            DataTable ingressosTemp = new DataTable("Temporaria");
            ingressosTemp.Columns.Add("ID", typeof(int));
            ingressosTemp.Columns.Add("PrecoID", typeof(int));
            ingressosTemp.Columns.Add("BloqueioID", typeof(int));
            ingressosTemp.Columns.Add("CortesiaID", typeof(int));

            info = new DataTable("Info");
            info.Columns.Add(EVENTO, typeof(string));
            info.Columns.Add(APRESENTACAO, typeof(string));
            info.Columns.Add(SETOR, typeof(string));
            info.Columns.Add(PRECO, typeof(string));
            info.Columns.Add(QTDE, typeof(int));
            info.Columns.Add(VALOR, typeof(decimal));

            info2 = info.Clone();

            if (ingressosCodigo.Length > 0)
            { //ingresso a ingresso

                ArrayList listaErrados = new ArrayList();

                foreach (DataRow ingresso in ingressosCodigo)
                {

                    try
                    {

                        string sql = "SELECT tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                            "WHERE tIngresso.LojaID=" + lojaIDDe + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND ";

                        if (ingresso["CodigoBarra"] == DBNull.Value)
                        {
                            string tipoSetor = (string)ingresso["TipoSetor"];
                            if (tipoSetor == Setor.Pista || tipoSetor == Setor.Cadeira)
                                sql += "tIngresso.Codigo = '" + (string)ingresso["Codigo"] + "' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                            else
                                sql += "tIngresso.Codigo like '" + (string)ingresso["Codigo"] + "%' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                        }
                        else
                        {
                            sql += "(tIngresso.CodigoBarra='" + (string)ingresso["CodigoBarra"] + "' OR tIngresso.CodigoBarraCliente='" + (string)ingresso["CodigoBarra"] + "')";
                        }

                        bd.Consulta(sql);

                        bool ok = false;
                        while (bd.Consulta().Read())
                        {
                            DataRow linhaInfo = info.NewRow();
                            linhaInfo[EVENTO] = bd.LerString("Evento");
                            linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                            linhaInfo[SETOR] = bd.LerString("Setor");
                            linhaInfo[PRECO] = bd.LerString("Preco");
                            linhaInfo[QTDE] = 1;
                            linhaInfo[VALOR] = bd.LerDecimal("Valor");
                            info.Rows.Add(linhaInfo);

                            DataRow linha = ingressosTemp.NewRow();
                            linha["ID"] = bd.LerInt("ID");
                            linha["PrecoID"] = bd.LerInt("PrecoID");
                            linha["BloqueioID"] = bd.LerInt("BloqueioID");
                            linha["CortesiaID"] = bd.LerInt("CortesiaID");
                            ingressosTemp.Rows.Add(linha);
                            ok = true;
                        }
                        bd.Consulta().Close();

                        if (!ok)
                        { //gravar codigos de erro.
                            if (ingresso["CodigoBarra"] == DBNull.Value)
                                listaErrados.Add((string)ingresso["Codigo"]);
                            else
                                listaErrados.Add((string)ingresso["CodigoBarra"]);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                listaErrados.TrimToSize();

                if (listaErrados.Count == ingressosCodigo.Length)
                    throw new PreImpressoGerenciadorException("Todos os códigos informados não pertencem a loja.\nSe forem de ingressos não-marcados, informe o código completo.");

                if (listaErrados.Count > 0)
                {
                    string codigos = Utilitario.ArrayToString((string[])listaErrados.ToArray(typeof(string)));
                    throw new PreImpressoGerenciadorException("Os códigos informados abaixo não pertencem a loja:\n\n" + codigos);
                }

            }
            else
            {

                if ((int)ingressosLotePreco[0]["PrecoID"] == -1)
                { //todo o lote

                    try
                    {

                        string sql = "SELECT tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                            "WHERE LojaID=" + lojaIDDe + " AND Status='" + Ingresso.PREIMPRESSO + "' " +
                            "ORDER BY tEvento.Nome, tApresentacao.Horario, tSetor.Nome, tPreco.Nome, tIngresso.Codigo";

                        bd.Consulta(sql);

                        while (bd.Consulta().Read())
                        {
                            DataRow linhaInfo = info.NewRow();
                            linhaInfo[EVENTO] = bd.LerString("Evento");
                            linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                            linhaInfo[SETOR] = bd.LerString("Setor");
                            linhaInfo[PRECO] = bd.LerString("Preco");
                            linhaInfo[QTDE] = 1;
                            linhaInfo[VALOR] = bd.LerDecimal("Valor");
                            info.Rows.Add(linhaInfo);

                            DataRow linhaIngresso = ingressosTemp.NewRow();
                            linhaIngresso["ID"] = bd.LerInt("ID");
                            linhaIngresso["PrecoID"] = bd.LerInt("PrecoID");
                            linhaIngresso["BloqueioID"] = bd.LerInt("BloqueioID");
                            linhaIngresso["CortesiaID"] = bd.LerInt("CortesiaID");
                            ingressosTemp.Rows.Add(linhaIngresso);
                        }
                        bd.Consulta().Close();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                { //ingresso por lote

                    try
                    {

                        foreach (DataRow lote in ingressosLotePreco)
                        {

                            int PrecoID = (int)lote["PrecoID"];
                            int apresentacaoSetorID = (int)lote["ApresentacaoSetorID"];
                            int qtde = (int)lote["Quantidade"];

                            string sql = "SELECT top " + qtde + " tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                                "FROM tIngresso (NOLOCK) " +
                                "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                                "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                                "WHERE tIngresso.LojaID=" + lojaIDDe + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " " +
                                "ORDER BY tIngresso.Codigo";

                            bd.Consulta(sql);

                            while (bd.Consulta().Read())
                            {
                                DataRow linhaInfo = info.NewRow();
                                linhaInfo[EVENTO] = bd.LerString("Evento");
                                linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                                linhaInfo[SETOR] = bd.LerString("Setor");
                                linhaInfo[PRECO] = bd.LerString("Preco");
                                linhaInfo[QTDE] = 1;
                                linhaInfo[VALOR] = bd.LerDecimal("Valor");
                                info.Rows.Add(linhaInfo);

                                DataRow linha = ingressosTemp.NewRow();
                                linha["ID"] = bd.LerInt("ID");
                                linha["PrecoID"] = bd.LerInt("PrecoID");
                                linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                ingressosTemp.Rows.Add(linha);
                            }
                            bd.Consulta().Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            }

            try
            {

                bd.IniciarTransacao();

                foreach (DataRow ingresso in ingressosTemp.Rows)
                {
                    int ingressoID = (int)ingresso["ID"];
                    int PrecoID = (int)ingresso["PrecoID"];
                    int bloqueioID = (int)ingresso["BloqueioID"];
                    int cortesiaID = (int)ingresso["CortesiaID"];

                    bool ok = transferirPreImpresso(ingressoID, PrecoID, bloqueioID, cortesiaID, lojaIDDe, lojaIDPara, canalIDPara, empresaIDPara, usuarioID);

                    if (ok)
                        qtde1++;
                }

                //agora realizar a trasnferencia do restante para a segunda loja
                ingressosTemp.Clear();

                string sql = "SELECT tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                    "FROM tIngresso (NOLOCK) " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                    "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                    "WHERE LojaID=" + lojaIDDe + " AND Status='" + Ingresso.PREIMPRESSO + "' " +
                    "ORDER BY tEvento.Nome, tApresentacao.Horario, tSetor.Nome, tPreco.Nome, tIngresso.Codigo";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaInfo = info2.NewRow();
                    linhaInfo[EVENTO] = bd.LerString("Evento");
                    linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                    linhaInfo[SETOR] = bd.LerString("Setor");
                    linhaInfo[PRECO] = bd.LerString("Preco");
                    linhaInfo[QTDE] = 1;
                    linhaInfo[VALOR] = bd.LerDecimal("Valor");
                    info2.Rows.Add(linhaInfo);

                    DataRow linha = ingressosTemp.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    ingressosTemp.Rows.Add(linha);
                }
                bd.Consulta().Close();

                foreach (DataRow ingresso in ingressosTemp.Rows)
                {
                    int ingressoID = (int)ingresso["ID"];
                    int PrecoID = (int)ingresso["PrecoID"];
                    int bloqueioID = (int)ingresso["BloqueioID"];
                    int cortesiaID = (int)ingresso["CortesiaID"];

                    bool ok = transferirPreImpresso(ingressoID, PrecoID, bloqueioID, cortesiaID, lojaIDDe, lojaID2Para, canalID2Para, empresaID2Para, usuarioID);

                    if (ok)
                        qtde2++;
                }

                bd.FinalizarTransacao();

            }
            catch (Exception ex)
            {
                qtde2 = 0;
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            qtdes[0] = qtde1;
            qtdes[1] = qtde2;

            return qtdes;

        }

        /// <summary>
        /// Transferir ingressos com codigo de barra mas antes verificar se os mesmos pertencem a loja indicada
        /// </summary>
        /// <returns></returns>
        public int Transferir(int lojaIDDe, DataTable ingressos, int lojaIDPara, int canalIDPara, int empresaIDPara, int usuarioID)
        {

            DataRow[] ingressosLotePreco = ingressos.Select("PrecoID<>0");
            DataRow[] ingressosCodigo = ingressos.Select("PrecoID=0");

            //construindo AS estruturas
            DataTable ingressosTemp = new DataTable("Temporaria");
            ingressosTemp.Columns.Add("ID", typeof(int));
            ingressosTemp.Columns.Add("PrecoID", typeof(int));
            ingressosTemp.Columns.Add("BloqueioID", typeof(int));
            ingressosTemp.Columns.Add("CortesiaID", typeof(int));

            info = new DataTable("Info");
            info.Columns.Add(EVENTO, typeof(string));
            info.Columns.Add(APRESENTACAO, typeof(string));
            info.Columns.Add(SETOR, typeof(string));
            info.Columns.Add(PRECO, typeof(string));
            info.Columns.Add(QTDE, typeof(int));
            info.Columns.Add(VALOR, typeof(decimal));

            if (ingressosCodigo.Length > 0)
            { //ingresso a ingresso

                ArrayList listaErrados = new ArrayList();

                foreach (DataRow ingresso in ingressosCodigo)
                {

                    try
                    {

                        string sql = "SELECT tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                            "WHERE tIngresso.LojaID=" + lojaIDDe + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND ";

                        if (ingresso["CodigoBarra"] == DBNull.Value)
                        {
                            string tipoSetor = (string)ingresso["TipoSetor"];
                            if (tipoSetor == Setor.Pista || tipoSetor == Setor.Cadeira)
                                sql += "tIngresso.Codigo = '" + (string)ingresso["Codigo"] + "' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                            else
                                sql += "tIngresso.Codigo like '" + (string)ingresso["Codigo"] + "%' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                        }
                        else
                        {
                            sql += "(tIngresso.CodigoBarra='" + (string)ingresso["CodigoBarra"] + "' OR tIngresso.CodigoBarraCliente='" + (string)ingresso["CodigoBarra"] + "')";
                        }

                        bd.Consulta(sql);

                        bool ok = false;
                        while (bd.Consulta().Read())
                        {
                            DataRow linhaInfo = info.NewRow();
                            linhaInfo[EVENTO] = bd.LerString("Evento");
                            linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                            linhaInfo[SETOR] = bd.LerString("Setor");
                            linhaInfo[PRECO] = bd.LerString("Preco");
                            linhaInfo[QTDE] = 1;
                            linhaInfo[VALOR] = bd.LerDecimal("Valor");
                            info.Rows.Add(linhaInfo);

                            DataRow linha = ingressosTemp.NewRow();
                            linha["ID"] = bd.LerInt("ID");
                            linha["PrecoID"] = bd.LerInt("PrecoID");
                            linha["BloqueioID"] = bd.LerInt("BloqueioID");
                            linha["CortesiaID"] = bd.LerInt("CortesiaID");
                            ingressosTemp.Rows.Add(linha);
                            ok = true;
                        }
                        bd.Consulta().Close();

                        if (!ok)
                        { //gravar codigos de erro.
                            if (ingresso["CodigoBarra"] == DBNull.Value)
                                listaErrados.Add((string)ingresso["Codigo"]);
                            else
                                listaErrados.Add((string)ingresso["CodigoBarra"]);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                listaErrados.TrimToSize();

                if (listaErrados.Count == ingressosCodigo.Length)
                    throw new PreImpressoGerenciadorException("Todos os códigos informados não pertencem a loja.\nSe forem de ingressos não-marcados, informe o código completo.");

                if (listaErrados.Count > 0)
                {
                    string codigos = Utilitario.ArrayToString((string[])listaErrados.ToArray(typeof(string)));
                    throw new PreImpressoGerenciadorException("Os códigos informados abaixo não pertencem a loja:\n\n" + codigos);
                }

            }
            else
            {

                if ((int)ingressosLotePreco[0]["PrecoID"] == -1)
                { //todo o lote

                    try
                    {

                        string sql = "SELECT tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                            "WHERE LojaID=" + lojaIDDe + " AND Status='" + Ingresso.PREIMPRESSO + "' " +
                            "ORDER BY tEvento.Nome, tApresentacao.Horario, tSetor.Nome, tPreco.Nome, tIngresso.Codigo";

                        bd.Consulta(sql);

                        while (bd.Consulta().Read())
                        {
                            DataRow linhaInfo = info.NewRow();
                            linhaInfo[EVENTO] = bd.LerString("Evento");
                            linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                            linhaInfo[SETOR] = bd.LerString("Setor");
                            linhaInfo[PRECO] = bd.LerString("Preco");
                            linhaInfo[QTDE] = 1;
                            linhaInfo[VALOR] = bd.LerDecimal("Valor");
                            info.Rows.Add(linhaInfo);

                            DataRow linhaIngresso = ingressosTemp.NewRow();
                            linhaIngresso["ID"] = bd.LerInt("ID");
                            linhaIngresso["PrecoID"] = bd.LerInt("PrecoID");
                            linhaIngresso["BloqueioID"] = bd.LerInt("BloqueioID");
                            linhaIngresso["CortesiaID"] = bd.LerInt("CortesiaID");
                            ingressosTemp.Rows.Add(linhaIngresso);
                        }
                        bd.Consulta().Close();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                { //ingresso por lote

                    try
                    {

                        foreach (DataRow lote in ingressosLotePreco)
                        {

                            int PrecoID = (int)lote["PrecoID"];
                            int apresentacaoSetorID = (int)lote["ApresentacaoSetorID"];
                            int qtde = (int)lote["Quantidade"];

                            string sql = "SELECT top " + qtde + " tIngresso.ID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, tPreco.Valor " +
                                "FROM tIngresso (NOLOCK) " +
                                "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                                "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                                "WHERE tIngresso.LojaID=" + lojaIDDe + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " " +
                                "ORDER BY tIngresso.Codigo";

                            bd.Consulta(sql);

                            while (bd.Consulta().Read())
                            {
                                DataRow linhaInfo = info.NewRow();
                                linhaInfo[EVENTO] = bd.LerString("Evento");
                                linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                                linhaInfo[SETOR] = bd.LerString("Setor");
                                linhaInfo[PRECO] = bd.LerString("Preco");
                                linhaInfo[QTDE] = 1;
                                linhaInfo[VALOR] = bd.LerDecimal("Valor");
                                info.Rows.Add(linhaInfo);

                                DataRow linha = ingressosTemp.NewRow();
                                linha["ID"] = bd.LerInt("ID");
                                linha["PrecoID"] = bd.LerInt("PrecoID");
                                linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                ingressosTemp.Rows.Add(linha);
                            }
                            bd.Consulta().Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            }

            int retorno = 0;

            try
            {

                bd.IniciarTransacao();

                foreach (DataRow ingresso in ingressosTemp.Rows)
                {
                    int ingressoID = (int)ingresso["ID"];
                    int PrecoID = (int)ingresso["PrecoID"];
                    int bloqueioID = (int)ingresso["BloqueioID"];
                    int cortesiaID = (int)ingresso["CortesiaID"];

                    bool ok = transferirPreImpresso(ingressoID, PrecoID, bloqueioID, cortesiaID, lojaIDDe, lojaIDPara, canalIDPara, empresaIDPara, usuarioID);

                    if (ok)
                        retorno++;
                }

                bd.FinalizarTransacao();

            }
            catch (Exception ex)
            {
                retorno = 0;
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return retorno;

        }

        /// <summary>
        /// Muda o status do ingresso reservado para pre-impresso. Retorna o sucesso da operacao.
        /// </summary>
        /// <returns></returns>
        private bool transferirPreImpresso(int ingressoID, int PrecoID, int bloqueioID, int cortesiaID, int lojaIDDe, int lojaIDPara, int canalIDPara, int empresaIDPara, int usuarioID)
        {

            string sql = "UPDATE tIngresso SET LojaID=" + lojaIDPara + ",UsuarioID=" + usuarioID + " " +
                "WHERE ID=" + ingressoID + " AND LojaID=" + lojaIDDe + " AND Status='" + Ingresso.PREIMPRESSO + "'";

            object x = bd.Executar(sql);

            bool ok = Convert.ToBoolean(x);

            if (ok)
            {

                IngressoLog ingressoLog = new IngressoLog();
                ingressoLog.IngressoID.Valor = ingressoID;
                ingressoLog.UsuarioID.Valor = usuarioID;
                ingressoLog.BloqueioID.Valor = bloqueioID;
                ingressoLog.CortesiaID.Valor = cortesiaID;
                ingressoLog.PrecoID.Valor = PrecoID;
                ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                ingressoLog.Acao.Valor = IngressoLog.TRANSFERENCIA_PREIMPRESSO;
                ingressoLog.LojaID.Valor = lojaIDPara;
                ingressoLog.CanalID.Valor = canalIDPara;
                ingressoLog.EmpresaID.Valor = empresaIDPara;

                string sqlIngressoLog = ingressoLog.StringInserir();
                bool okV = Convert.ToBoolean(bd.Executar(sqlIngressoLog));

                if (!okV)
                    throw new PreImpressoGerenciadorException("Log de transferencia do pré-impresso " + ingressoID + " não foi inserido.");

            }

            return ok;

        }

        public DataTable EstruturaResumo()
        {
            DataTable tabela = new DataTable("tabela");
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("Apresentacao", typeof(string));
            tabela.Columns.Add("ApresentacaoSetorID", typeof(int));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("PrecoID", typeof(int));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("Valor", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(int));
            tabela.Columns.Add("Canal", typeof(string));
            tabela.Columns.Add("Loja", typeof(string));
            tabela.Columns.Add("PacoteID", typeof(int));
            return tabela;
        }

        public DataTable InfoResumo(int lojaID, DataTable ingressos)
        {
            try
            {
                string idsIn = string.Empty;
                StringBuilder ids = new StringBuilder();

                resumo = this.EstruturaResumo();

                DataRow[] ingressosLotePreco = ingressos.Select("PrecoID<>0 AND PacoteID = 0");
                DataRow[] ingressosCodigo = ingressos.Select("PrecoID=0 AND PacoteID = 0");
                DataRow[] ingressosPacote = ingressos.Select("PacoteID <> 0");
                DataTable table = ingressos.Clone();

                foreach (DataRow ingresso in ingressosCodigo)
                {
                    resumo.Rows.Add(ResumoIngressosPorCodigo(lojaID, ingresso));
                }
                foreach (DataRow ingresso in ingressosLotePreco)
                {
                    resumo.Rows.Add(ResumoIngressosPorLote(lojaID, ingresso));
                }

                foreach (var item in ingressosPacote)
                    table.ImportRow(item);

                DataTable PacotesID = TabelaMemoria.Distinct(table, "PacoteID", "Quantidade");

                foreach (DataRow item in PacotesID.Rows)
                    resumo.Rows.Add(ResumoIngressosPorPacote(lojaID, (int)item["PacoteID"], (int)item["Quantidade"]));

                return resumo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataRow ResumoIngressosPorPacote(int LojaID, int PacoteID, int Quantidade)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT tPa.Nome as Evento , SUM(tp.Valor) as Valor 
                                FROM tPacote tPa
                                INNER JOIN tPacoteItem as tPi on tPa.ID = tPi.PacoteID
                                INNER JOIN tPreco tp on tp.ID = tPi.PrecoID
                                WHERE tPa.ID = " + PacoteID + "GROUP BY tPa.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = " - ";
                    linha["Setor"] = " - ";
                    linha["Preco"] = " - ";
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = Quantidade;
                    linha["PacoteID"] = PacoteID;
                    linha["PrecoID"] = 0;
                    linha["ApresentacaoSetorID"] = 0;
                }

                sql = @"SELECT tLoja.Nome AS Loja, tCanal.Nome AS Canal
                        FROM tLoja 
                        INNER JOIN tCanal ON tCanal.ID = tLoja.CanalID
                        WHERE tLoja.ID = " + LojaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                }

                bd.Fechar();

                return linha;
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

        private DataRow ResumoIngressosPorCodigo(int lojaID, DataRow ingresso)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                if (ingresso["Codigo"] != DBNull.Value)
                {

                    string codigo = (string)ingresso["Codigo"];
                    int lugarID = 0;
                    int PrecoID = 0;
                    int apresentacaoSetorID = (int)ingresso["ApresentacaoSetorID"];
                    string tipoSetor = "";
                    string sqlInfoCodigo = string.Empty;

                    if ((string)ingresso["TipoSetor"] == "P")
                    {
                        sqlInfoCodigo = @"SELECT DISTINCT LugarID, PrecoID, tSetor.LugarMarcado
                            FROM tIngresso (NOLOCK)
                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                            WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ApresentacaoSetorID = '" + apresentacaoSetorID +
                                "' AND tIngresso.Codigo = '" + codigo + "'";

                    }
                    else
                    {
                        sqlInfoCodigo = @"SELECT DISTINCT LugarID, PrecoID, tSetor.LugarMarcado
                            FROM tIngresso (NOLOCK)
                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                            INNER JOIN tLugar ON tLugar.ID = tIngresso.LugarID WHERE Status='" + Ingresso.PREIMPRESSO +
                         "' AND ApresentacaoSetorID = '" + apresentacaoSetorID + "' AND (tIngresso.Codigo = '" + codigo +
                         "' OR tLugar.Codigo = '" + codigo + "')";
                    }

                    bd.Consulta(sqlInfoCodigo);

                    while (bd.Consulta().Read())
                    {
                        lugarID = bd.LerInt("LugarID");
                        PrecoID = bd.LerInt("PrecoID");
                        tipoSetor = bd.LerString("LugarMarcado");
                    }

                    if (tipoSetor == "M")
                        linha = ResumoIngressoPorCodigoMarcado(lojaID, PrecoID, apresentacaoSetorID, lugarID);
                    else
                        linha = ResumoIngressoPorCodigo(lojaID, PrecoID, apresentacaoSetorID, lugarID, tipoSetor);
                }
                else if (ingresso["CodigoBarra"] != DBNull.Value)
                {
                    string codigo = (string)ingresso["CodigoBarra"];
                    int lugarID = 0;
                    int PrecoID = 0;
                    string tipoSetor = "";

                    string sqlInfoCodigo = @"SELECT DISTINCT LugarID, PrecoID, tSetor.LugarMarcado
                                            FROM tIngresso (NOLOCK)
                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                            WHERE
                                            Status='" + Ingresso.PREIMPRESSO +
                                            "' AND CodigoBarra = '" + codigo + "'";

                    bd.Consulta(sqlInfoCodigo);

                    while (bd.Consulta().Read())
                    {
                        lugarID = bd.LerInt("LugarID");
                        PrecoID = bd.LerInt("PrecoID");
                        tipoSetor = bd.LerString("LugarMarcado");
                    }

                    if (tipoSetor == "M")
                        linha = ResumoIngressoPorBarrasMarcado(lojaID, PrecoID, lugarID, codigo);
                    else
                        linha = ResumoIngressoPorBarras(lojaID, PrecoID, lugarID, codigo);
                }

                return linha;
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

        private DataRow ResumoIngressoPorBarrasMarcado(int lojaID, int PrecoID, int lugarID, string codigo)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, (tPreco.Valor * tl.Quantidade ) as Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            INNER JOIN tLugar tl on ts.ID = tl.SetorID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.CodigoBarra = '" + codigo + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = 1;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        private DataRow ResumoIngressoPorBarras(int lojaID, int PrecoID, int lugarID, string codigo)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, tPreco.Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.LugarID = " + lugarID + " AND tIngresso.CodigoBarra = '" + codigo + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = 1;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        private DataRow ResumoIngressoPorCodigoMarcado(int lojaID, int PrecoID, int apresentacaoSetorID, int lugarID)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, (tPreco.Valor * tl.Quantidade ) as Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            INNER JOIN tLugar tl on ts.ID = tl.SetorID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND LugarID = " + lugarID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = 1;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        private DataRow ResumoIngressoPorCodigo(int lojaID, int PrecoID, int apresentacaoSetorID, int lugarID, string tipoSetor)
        {
            try
            {
                DataRow linha = resumo.NewRow();
                string sql = string.Empty;

                if (tipoSetor == "P")
                {
                    sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, tPreco.Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND tIngresso.LugarID = " + lugarID;
                }
                else
                {
                    sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, tPreco.Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            INNER JOIN tLugar tl on ts.ID = tl.SetorID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND tIngresso.LugarID = " + lugarID;
                }

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = 1;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        private DataRow ResumoIngressosPorLote(int lojaID, DataRow ingresso)
        {
            DataRow linha = resumo.NewRow();

            int PrecoID = (int)ingresso["PrecoID"];
            int apresentacaoSetorID = (int)ingresso["ApresentacaoSetorID"];
            int qtde = (int)ingresso["Quantidade"];
            string tipoSetor = (string)ingresso["TipoSetor"];
            int[] lugaresID = new int[qtde];
            int i = 0;

            StringBuilder sblugaresID = new StringBuilder();

            if (tipoSetor == "M")
            {
                string sqlLugaresID = "SELECT DISTINCT TOP " + qtde + " LugarID " +
                                      "FROM tIngresso " +
                                      "WHERE " +
                                      "tIngresso.Status='" + Ingresso.PREIMPRESSO +
                                      "' AND tIngresso.LojaID=" + lojaID +
                                      " AND tIngresso.PrecoID=" + PrecoID +
                                      " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID;

                bd.Consulta(sqlLugaresID);

                while (bd.Consulta().Read())
                {
                    lugaresID[i] = bd.LerInt("LugarID");

                    if (i == 0)
                        sblugaresID.Append(" LugarID = " + lugaresID[i].ToString());
                    else
                        sblugaresID.Append(" OR LugarID = " + lugaresID[i].ToString());

                    i++;
                }

                linha = ResumoIngressoPorLoteMarcado(lojaID, PrecoID, apresentacaoSetorID, qtde, sblugaresID);
            }
            else
            {
                linha = ResumoIngressoPorLoteNaoMarcado(lojaID, PrecoID, apresentacaoSetorID, qtde);
            }

            return linha;
        }

        private DataRow ResumoIngressoPorLoteMarcado(int lojaID, int PrecoID, int apresentacaoSetorID, int qtde, StringBuilder sblugaresID)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT DISTINCT te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, (tPreco.Valor * tl.Quantidade ) as Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID
                            FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            INNER JOIN tLugar tl on ts.ID = tl.SetorID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND(" + sblugaresID + ")";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = qtde;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        private DataRow ResumoIngressoPorLoteNaoMarcado(int lojaID, int PrecoID, int apresentacaoSetorID, int qtde)
        {
            try
            {
                DataRow linha = resumo.NewRow();

                string sql = @"SELECT Top " + qtde + " te.Nome as Evento, ta.Horario as Apresentacao, tse.Nome as Setor, tPreco.Nome as Preco, tPreco.Valor,  tLoja.Nome as Loja,tc.Nome AS Canal ,tPreco.Nome as Preco, tPreco.Valor, tp.ID as PrecoID, tIngresso.ApresentacaoSetorID, tIngresso.PacoteID " +
                            @"FROM tIngresso (NOLOCK) 
                            INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID
                            INNER JOIN tLoja (NOLOCK) ON tIngresso.LojaID = tLoja.ID
                            INNER JOIN tSetor ts ON ts.ID = tIngresso.SetorID
                            INNER JOIN tEvento te on te.ID = tIngresso.EventoID
                            INNER JOIN tApresentacao ta on ta.ID = tIngresso.ApresentacaoID
                            INNER JOIN tSetor tse on tse.ID = tIngresso.SetorID
                            INNER JOIN tPreco tp on tp.ID = tIngresso.PrecoID
                            INNER JOIN tCanal tc on tc.ID = tLoja.CanalID
                            WHERE tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    linha["Evento"] = bd.LerString("Evento");
                    linha["Apresentacao"] = bd.LerStringFormatoDataHora("Apresentacao");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["Preco"] = bd.LerString("Preco");
                    linha["Valor"] = bd.LerDecimal("Valor").ToString("c");
                    linha["Quantidade"] = qtde;
                    linha["Canal"] = bd.LerString("Canal");
                    linha["Loja"] = bd.LerString("Loja");
                    linha["PacoteID"] = bd.LerInt("PacoteID");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                }
                bd.Consulta().Close();

                return linha;
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

        public bool VerificaCodigoExiste(int lojaID, int apresentacaoID, string codigo, int setorID, string setor)
        {
            try
            {
                bool existe = false;
                string sql = string.Empty;

                if (setor == Setor.Pista)
                {
                    sql = @"SELECT tIngresso.ID FROM tIngresso 
                            WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + lojaID + " AND ApresentacaoID =" + apresentacaoID +
                           @" AND tIngresso.Codigo = '" + codigo + "' AND tIngresso.SetorID =" + setorID;
                }
                else
                {
                    sql = @"SELECT tIngresso.ID FROM tIngresso 
                            INNER JOIN tLugar ON tLugar.ID = tIngresso.LugarID
                            WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + lojaID + " AND ApresentacaoID =" + apresentacaoID +
                           @" AND (tIngresso.Codigo = '" + codigo + "' OR tLugar.Codigo = '" + codigo + "') AND tIngresso.SetorID =" + setorID;
                }
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    existe = true;
                    break;
                }
                bd.Consulta().Close();

                return existe;
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

        public List<string> VerificaCodigoExisteBarras(int lojaID, string codigo)
        {
            try
            {
                List<string> retorno = new List<string>();

                string sql = @"SELECT tIngresso.Classificacao, tSetor.LugarMarcado FROM tIngresso 
                                INNER JOIN tSetor ON tIngresso.SetorID = tSetor.ID
                                WHERE Status='" + Ingresso.PREIMPRESSO + "' AND LojaID=" + lojaID +
                                " AND (CodigoBarra = '" + codigo + "' OR CodigoBarraCliente='" + codigo + "')";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(bd.LerString("LugarMarcado"));
                    retorno.Add(Convert.ToString(bd.LerInt("Classificacao")));

                }
                bd.Consulta().Close();

                return retorno;
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
        /// Obter informacoes dos ingressos como valores e conveniencias e verificar se os mesmos pertencem a loja indicada
        /// </summary>
        /// <returns></returns>
        public DataTable InfoVenda(int lojaID, int canalID, DataTable ingressos, bool vendidos)
        {
            try
            {
                //a tabela ingressos possui a estrutura do metodo EstrutraIngressos()

                DataRow[] ingressosLotePreco = ingressos.Select("PrecoID<>0");
                DataRow[] ingressosCodigo = ingressos.Select("PrecoID=0");
                ArrayList listaErrados = new ArrayList();

                info = new DataTable("Info");
                info.Columns.Add("ID", typeof(int));
                info.Columns.Add("IngressoID", typeof(int));
                info.Columns.Add("PrecoID", typeof(int));
                info.Columns.Add("PacoteGrupo", typeof(string));
                info.Columns.Add("LugarID", typeof(int));
                info.Columns.Add("PacoteID", typeof(int));
                info.Columns.Add("BloqueioID", typeof(int));
                info.Columns.Add("CortesiaID", typeof(int));
                info.Columns.Add("Valor", typeof(decimal));
                info.Columns.Add("Conv", typeof(int));
                info.Columns.Add("ValorConv", typeof(decimal));
                info.Columns.Add("TaxaMaxima", typeof(decimal));
                info.Columns.Add("TaxaMinima", typeof(decimal));
                info.Columns.Add("TaxaComissao", typeof(int));
                info.Columns.Add("ValorComissao", typeof(decimal));
                info.Columns.Add("ComissaoMaxima", typeof(decimal));
                info.Columns.Add("ComissaoMinima", typeof(decimal));
                info.Columns.Add("LugarMarcado", typeof(string));
                info.Columns.Add("CodigoBarra", typeof(string));
                info.Columns.Add("TipoCodigoBarra", typeof(string));
                info.Columns.Add("EventoID", typeof(int));
                info.Columns.Add("ApresentacaoSetorID", typeof(int));


                foreach (DataRow ingresso in ingressosCodigo)
                {
                    //ingressos por Codigo
                    if (ingresso["Codigo"] != DBNull.Value)
                    {
                        string codigo = (string)ingresso["Codigo"];
                        int lugarID = 0;
                        int PrecoID = 0;
                        int apresentacaoSetorID = (int)ingresso["ApresentacaoSetorID"];
                        string tipoSetor = (string)ingresso["TipoSetor"];
                        string sqlInfoCodigo = string.Empty;
                        string pacoteGrupo = string.Empty;

                        if (tipoSetor == "P")
                        {
                            sqlInfoCodigo = @"SELECT DISTINCT LugarID, PrecoID, tSetor.LugarMarcado, PacoteGrupo
                                                            FROM tIngresso (NOLOCK)
                                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                                            WHERE
                                                            Status='" + Ingresso.PREIMPRESSO +
                                                    "' AND ApresentacaoSetorID = '" + apresentacaoSetorID +
                                                    "' AND tIngresso.Codigo = '" + codigo + "'";
                        }
                        else
                        {
                            sqlInfoCodigo = @"SELECT DISTINCT LugarID, PrecoID, tSetor.LugarMarcado, PacoteGrupo
                                                            FROM tIngresso (NOLOCK)
                                                            INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID
                                                            INNER JOIN tLugar ON tLugar.ID = tIngresso.LugarID
                                                            WHERE
                                                            Status='" + Ingresso.PREIMPRESSO +
                                                    "' AND ApresentacaoSetorID = '" + apresentacaoSetorID +
                                                    "' AND (tIngresso.Codigo = '" + codigo + "' OR tLugar.Codigo = '" + codigo + "')";
                        }
                        bd.Consulta(sqlInfoCodigo);

                        while (bd.Consulta().Read())
                        {
                            lugarID = bd.LerInt("LugarID");
                            PrecoID = bd.LerInt("PrecoID");
                            tipoSetor = bd.LerString("LugarMarcado");
                            pacoteGrupo = bd.LerString("PacoteGrupo");
                        }
                        //ingresso por codigo tipo mesa fechada
                        if (tipoSetor == "M")
                        {
                            string sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteID,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                            "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao, isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao ,tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                            "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                            "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + "AND LugarID = " + lugarID +
                            "ORDER BY tIngresso.ID";

                            bool ok = false;
                            bd.Consulta(sql);

                            while (bd.Consulta().Read())
                            {
                                int IngressoID = bd.LerInt("ID");
                                DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                if (item.Length == 0)
                                {
                                    DataRow linha = info.NewRow();
                                    linha["ID"] = 0;
                                    linha["IngressoID"] = IngressoID;
                                    linha["PrecoID"] = bd.LerInt("PrecoID");
                                    linha["LugarID"] = bd.LerInt("LugarID");
                                    linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                    linha["PacoteID"] = bd.LerString("PacoteID");
                                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                    linha["Valor"] = bd.LerDecimal("Valor");
                                    linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                    linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                    linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                    linha["EventoID"] = bd.LerInt("EventoID");
                                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                    info.Rows.Add(linha);
                                }

                                ok = true;
                            }
                            bd.Consulta().Close();

                            if (!ok)
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                    listaErrados.Add((string)ingresso["Codigo"]);
                                else
                                    listaErrados.Add((string)ingresso["CodigoBarra"]);
                            }
                        }

                        //ingresso por codigo tipo normal
                        else
                        {
                            string sql = string.Empty;

                            if (pacoteGrupo == "")
                            {
                                if (tipoSetor == "P")
                                    sql = @"SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.PacoteID,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                           "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao, isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao ,tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                           "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                           "FROM tIngresso (NOLOCK) " +
                                           "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                           "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                           "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                           "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                           "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND tIngresso.Codigo = '" + codigo + "'" +
                                           " ORDER BY tIngresso.ID";
                                else
                                    sql = @"SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.PacoteID,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                       "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao, isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao ,tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                       "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                       "FROM tIngresso (NOLOCK) " +
                                       "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                       "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                       "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                       "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                       "INNER JOIN tLugar ON tLugar.ID = tIngresso.LugarID " +
                                       "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND (tIngresso.Codigo = '" + codigo + "' OR tLugar.Codigo = '" + codigo + "') " +
                                       "ORDER BY tIngresso.ID";
                            }
                            else
                                sql = @"SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.PacoteID,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                   "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao, isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao ,tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                   "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                   "FROM tIngresso (NOLOCK) " +
                                   "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                   "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                   "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                   "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                   "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND PacoteGrupo=" + pacoteGrupo +
                                   " ORDER BY tIngresso.ID";

                            bool ok = false;
                            bd.Consulta(sql);

                            while (bd.Consulta().Read())
                            {
                                int IngressoID = bd.LerInt("ID");
                                DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                if (item.Length == 0)
                                {
                                    DataRow linha = info.NewRow();
                                    linha["ID"] = 0;
                                    linha["IngressoID"] = IngressoID;
                                    linha["PrecoID"] = bd.LerInt("PrecoID");
                                    linha["LugarID"] = bd.LerInt("LugarID");
                                    linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                    linha["PacoteID"] = bd.LerString("PacoteID");
                                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                    linha["Valor"] = bd.LerDecimal("Valor");
                                    linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                    linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                    linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                    linha["EventoID"] = bd.LerInt("EventoID");
                                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                    info.Rows.Add(linha);
                                }

                                ok = true;
                            }
                            bd.Consulta().Close();
                            if (!ok)
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                    listaErrados.Add((string)ingresso["Codigo"]);
                                else
                                    listaErrados.Add((string)ingresso["CodigoBarra"]);
                            }
                        }
                    } //fim do if ingressos por codigo
                    else
                    {
                        int qtde = (int)ingresso["Quantidade"];
                        int lugarID = 0;
                        int PrecoID = 0;
                        int apresentacaoSetorID = 0;
                        string tipoSetor = "";
                        string pacoteGrupo = string.Empty;

                        string sqlInfoCodigoBarra = "SELECT DISTINCT LugarID, PrecoID,ApresentacaoSetorID, PacoteGrupo, tSetor.LugarMarcado " +
                                                    "FROM tIngresso (NOLOCK) " +
                                                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                                    "WHERE " +
                                                    "Status='" + Ingresso.PREIMPRESSO +
                                                    "' AND LojaID=" + lojaID + " AND(tIngresso.CodigoBarra='" + (string)ingresso["CodigoBarra"] + "' OR tIngresso.CodigoBarraCliente='" + (string)ingresso["CodigoBarra"] + "')";

                        bd.Consulta(sqlInfoCodigoBarra);
                        while (bd.Consulta().Read())
                        {
                            lugarID = bd.LerInt("LugarID");
                            PrecoID = bd.LerInt("PrecoID");
                            apresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                            tipoSetor = bd.LerString("LugarMarcado");
                            pacoteGrupo = bd.LerString("PacoteGrupo");
                        }

                        if (tipoSetor == "M")
                        {
                            string sqlLugarID = string.Empty;

                            if (pacoteGrupo == "")
                            {
                                sqlLugarID = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tIngresso.PacoteID, tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                                "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                                "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                                "FROM tIngresso (NOLOCK) " +
                                                "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                                "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                                "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                                "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                                "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + "AND tIngresso.LugarID=" + lugarID +
                                                " ORDER BY tIngresso.ID";
                            }
                            else
                            {
                                sqlLugarID = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tIngresso.PacoteID, tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                                "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                                "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                                "FROM tIngresso (NOLOCK) " +
                                                "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                                "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                                "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                                "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                                "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND PacoteGrupo=" + pacoteGrupo +
                                                " ORDER BY tIngresso.ID";
                            }

                            bd.Consulta(sqlLugarID);

                            bool ok = false;
                            while (bd.Consulta().Read())
                            {
                                int IngressoID = bd.LerInt("ID");
                                DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                if (item.Length == 0)
                                {
                                    DataRow linha = info.NewRow();
                                    linha["ID"] = 0;
                                    linha["IngressoID"] = IngressoID;
                                    linha["PrecoID"] = bd.LerInt("PrecoID");
                                    linha["LugarID"] = bd.LerInt("LugarID");
                                    linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                    linha["PacoteID"] = bd.LerString("PacoteID");
                                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                    linha["Valor"] = bd.LerDecimal("Valor");
                                    linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                    linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                    linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                    linha["EventoID"] = bd.LerInt("EventoID");
                                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                    info.Rows.Add(linha);
                                }

                                ok = true;
                            }
                            bd.Consulta().Close();

                            if (!ok)
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                    listaErrados.Add((string)ingresso["Codigo"]);
                                else
                                    listaErrados.Add((string)ingresso["CodigoBarra"]);
                            }
                        }//fim do if TipoSetor = "M"
                        else if (tipoSetor == "P")
                        {
                            string sql = string.Empty;
                            if (pacoteGrupo == "")
                            {
                                sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteID,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                               "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                               "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                               "FROM tIngresso (NOLOCK) " +
                               "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                               "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                               "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                               "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                               "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID +
                               "AND tIngresso.ApresentacaoSetorID = " + apresentacaoSetorID + " AND tIngresso.PrecoID = " + PrecoID + " AND tIngresso.CodigoBarra = '" + ingresso["CodigoBarra"] + "' " +
                               "ORDER BY tIngresso.ID";
                            }
                            else
                            {
                                sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteID,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                               "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                               "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                               "FROM tIngresso (NOLOCK) " +
                               "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                               "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                               "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                               "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                               "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND PacoteGrupo = '" + pacoteGrupo + "' " +
                               "ORDER BY tIngresso.ID";
                            }

                            bool ok = false;

                            bd.Consulta(sql);

                            while (bd.Consulta().Read())
                            {
                                int IngressoID = bd.LerInt("ID");
                                DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                if (item.Length == 0)
                                {
                                    DataRow linha = info.NewRow();
                                    linha["ID"] = 0;
                                    linha["IngressoID"] = IngressoID;
                                    linha["PrecoID"] = bd.LerInt("PrecoID");
                                    linha["LugarID"] = bd.LerInt("LugarID");
                                    linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                    linha["PacoteID"] = bd.LerString("PacoteID");
                                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                    linha["Valor"] = bd.LerDecimal("Valor");
                                    linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                    linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                    linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                    linha["EventoID"] = bd.LerInt("EventoID");
                                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                    info.Rows.Add(linha);
                                }

                                ok = true;
                            }
                            bd.Consulta().Close();

                            if (!ok)
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                    listaErrados.Add((string)ingresso["Codigo"]);
                                else
                                    listaErrados.Add((string)ingresso["CodigoBarra"]);
                            }
                        }
                        else if (tipoSetor != "M" && tipoSetor != "P")
                        {
                            string sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tIngresso.PacoteID, tIngresso.PrecoID,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                           "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                           "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                           "FROM tIngresso (NOLOCK) " +
                                           "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                           "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                           "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                                           "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                           "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND ";

                            if (pacoteGrupo == "")
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                {
                                    if (tipoSetor == Setor.Pista || tipoSetor == Setor.Cadeira)
                                        sql += "tIngresso.Codigo = '" + (string)ingresso["Codigo"] + "' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                                    else
                                        sql += "tIngresso.Codigo like '" + (string)ingresso["Codigo"] + "%' AND tIngresso.ApresentacaoSetorID=" + (int)ingresso["ApresentacaoSetorID"];
                                }
                                else
                                {
                                    sql += "(tIngresso.CodigoBarra='" + (string)ingresso["CodigoBarra"] + "' OR tIngresso.CodigoBarraCliente='" + (string)ingresso["CodigoBarra"] + "')";
                                }
                            }
                            else
                                sql += "pacoteGrupo = " + pacoteGrupo;

                            bd.Consulta(sql);

                            object oID = info.Compute("MAX(ID)", "1=1");
                            int id = (oID != DBNull.Value) ? Convert.ToInt32(oID) + 1 : info.Rows.Count + 1;

                            bool ok = false;

                            while (bd.Consulta().Read())
                            {
                                int IngressoID = bd.LerInt("ID");
                                DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                if (item.Length == 0)
                                {
                                    DataRow linha = info.NewRow();
                                    linha["ID"] = 0;
                                    linha["IngressoID"] = IngressoID;
                                    linha["PrecoID"] = bd.LerInt("PrecoID");
                                    linha["LugarID"] = bd.LerInt("LugarID");
                                    linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                    linha["PacoteID"] = bd.LerString("PacoteID");
                                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                    linha["Valor"] = bd.LerDecimal("Valor");
                                    linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                    linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                    linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                    linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                    linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                    linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                    linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                    linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                    linha["EventoID"] = bd.LerInt("EventoID");
                                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                    info.Rows.Add(linha);
                                }

                                ok = true;
                            }
                            bd.Consulta().Close();

                            if (!ok)
                            {
                                if (ingresso["CodigoBarra"] == DBNull.Value)
                                    listaErrados.Add((string)ingresso["Codigo"]);
                                else
                                    listaErrados.Add((string)ingresso["CodigoBarra"]);
                            }
                        }
                    }
                }//fim do foreach 

                if (ingressosLotePreco.Length > 0)
                {
                    if ((int)ingressosLotePreco[0]["PrecoID"] == -1)
                    {
                        string sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra, tIngresso.PrecoID,tIngresso.PacoteGrupo,tIngresso.LugarID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                            "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                            "tPreco.Valor,tSetor.LugarMarcado, tIngresso.PacoteID, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                            "FROM tIngresso (NOLOCK) " +
                            "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                            "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                            "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                            "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                            "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " " +
                            "ORDER BY tIngresso.ID";

                        bd.Consulta(sql);

                        while (bd.Consulta().Read())
                        {
                            int IngressoID = bd.LerInt("ID");
                            DataRow[] item = info.Select("IngressoID = " + IngressoID);

                            if (item.Length == 0)
                            {
                                DataRow linha = info.NewRow();
                                linha["ID"] = 0;
                                linha["IngressoID"] = IngressoID;
                                linha["PrecoID"] = bd.LerInt("PrecoID");
                                linha["LugarID"] = bd.LerInt("LugarID");
                                linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                linha["PacoteID"] = bd.LerString("PacoteID");
                                linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                linha["Valor"] = bd.LerDecimal("Valor");
                                linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                linha["EventoID"] = bd.LerInt("EventoID");
                                linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                info.Rows.Add(linha);
                            }
                        }
                        bd.Consulta().Close();
                    }
                    else
                    {
                        foreach (DataRow lote in ingressosLotePreco)
                        {

                            int PrecoID = (int)lote["PrecoID"];
                            int apresentacaoSetorID = (int)lote["ApresentacaoSetorID"];
                            int qtde = (int)lote["Quantidade"];
                            int pacoteID = (int)lote["PacoteID"];
                            string tipoSetor = (string)lote["TipoSetor"];
                            int[] lugaresID = new int[qtde];
                            int i = 0;
                            StringBuilder sblugaresID = new StringBuilder();

                            if (tipoSetor == "M")
                            {

                                string sqlLugaresID = "SELECT DISTINCT TOP " + qtde + " LugarID " +
                                                      "FROM tIngresso " +
                                                      "WHERE " +
                                                      "tIngresso.Status='" + Ingresso.PREIMPRESSO +
                                                      "' AND tIngresso.LojaID=" + lojaID +
                                                      " AND tIngresso.PrecoID=" + PrecoID +
                                                      " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID;
                                bd.Consulta(sqlLugaresID);
                                while (bd.Consulta().Read())
                                {

                                    lugaresID[i] = bd.LerInt("LugarID");
                                    if (i == 0)
                                        sblugaresID.Append(" LugarID = " + lugaresID[i].ToString());
                                    else
                                        sblugaresID.Append(" OR LugarID = " + lugaresID[i].ToString());

                                    i++;
                                }

                                string sql = "SELECT tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.PacoteID,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao, isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao ,tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                "FROM tIngresso (NOLOCK) " +
                                "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND(" + sblugaresID + ") AND PacoteID = " + pacoteID +
                                " ORDER BY tIngresso.ID";

                                bd.Consulta(sql);

                                while (bd.Consulta().Read())
                                {
                                    int IngressoID = bd.LerInt("ID");
                                    DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                    if (item.Length == 0)
                                    {
                                        DataRow linha = info.NewRow();
                                        linha["ID"] = 0;
                                        linha["IngressoID"] = bd.LerInt("ID");
                                        linha["PrecoID"] = bd.LerInt("PrecoID");
                                        linha["LugarID"] = bd.LerInt("LugarID");
                                        linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                        linha["PacoteID"] = bd.LerString("PacoteID");
                                        linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                        linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                        linha["Valor"] = bd.LerDecimal("Valor");
                                        linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                        linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                        linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                        linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                        linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                        linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                        linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                        linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                        linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                        linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                        linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                        linha["EventoID"] = bd.LerInt("EventoID");
                                        linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                        info.Rows.Add(linha);
                                    }
                                }
                                bd.Consulta().Close();
                            }
                            else
                            {
                                string sql = "SELECT Top " + qtde + " tIngresso.ID,tIngresso.CodigoBarra,tSetor.LugarMarcado,tIngresso.PacoteGrupo,tIngresso.PacoteID,tIngresso.LugarID, tIngresso.PrecoID, tIngresso.BloqueioID, tIngresso.CortesiaID, tCanalEvento.TaxaConveniencia, " +
                                   "isnull(dbo.Dividir(tCanalEvento.TaxaConveniencia,100) * tPreco.Valor, 0) AS ValorConv, tCanalEvento.TaxaMaxima, tCanalEvento.TaxaMinima, tCanalEvento.TaxaComissao,isnull(dbo.Dividir(tCanalEvento.TaxaComissao,100) * tPreco.Valor, 0) AS ValorComissao , tCanalEvento.ComissaoMaxima , tCanalEvento.ComissaoMinima, " +
                                   "tPreco.Valor, tEvento.TipoCodigoBarra, tIngresso.EventoID, tIngresso.ApresentacaoSetorID " +
                                   "FROM tIngresso (NOLOCK) " +
                                   "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                                   "INNER JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tIngresso.EventoID " +
                                   "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                                   "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID AND tPreco.ApresentacaoSetorID = tIngresso.ApresentacaoSetorID " +
                                   "WHERE tCanalEvento.CanalID=" + canalID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' AND tIngresso.LojaID=" + lojaID + " AND tIngresso.PrecoID=" + PrecoID + " AND tIngresso.ApresentacaoSetorID=" + apresentacaoSetorID + " AND PacoteID = " + pacoteID +
                                   " ORDER BY tIngresso.ID";

                                bd.Consulta(sql);

                                while (bd.Consulta().Read())
                                {
                                    int IngressoID = bd.LerInt("ID");
                                    DataRow[] item = info.Select("IngressoID = " + IngressoID);

                                    if (item.Length == 0)
                                    {
                                        DataRow linha = info.NewRow();
                                        linha["ID"] = 0;
                                        linha["IngressoID"] = bd.LerInt("ID");
                                        linha["PrecoID"] = bd.LerInt("PrecoID");
                                        linha["LugarID"] = bd.LerInt("LugarID");
                                        linha["PacoteGrupo"] = bd.LerString("PacoteGrupo");
                                        linha["PacoteID"] = bd.LerInt("PacoteID");
                                        linha["BloqueioID"] = bd.LerInt("BloqueioID");
                                        linha["CortesiaID"] = bd.LerInt("CortesiaID");
                                        linha["Valor"] = bd.LerDecimal("Valor");
                                        linha["Conv"] = bd.LerInt("TaxaConveniencia");
                                        linha["ValorConv"] = bd.LerDecimal("ValorConv");
                                        linha["TaxaMaxima"] = bd.LerDecimal("TaxaMaxima");
                                        linha["TaxaMinima"] = bd.LerDecimal("TaxaMinima");
                                        linha["TaxaComissao"] = bd.LerInt("TaxaComissao");
                                        linha["ValorComissao"] = bd.LerDecimal("ValorComissao");
                                        linha["ComissaoMaxima"] = bd.LerDecimal("ComissaoMaxima");
                                        linha["ComissaoMinima"] = bd.LerDecimal("ComissaoMinima");
                                        linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                                        linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                                        linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                                        linha["EventoID"] = bd.LerInt("EventoID");
                                        linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                                        info.Rows.Add(linha);
                                    }
                                }
                                bd.Consulta().Close();
                            }
                        }//fim do foreach
                    }//fim do if ((int)ingressosLotePreco[0]["PrecoID"] == -1)
                }
                info = CTLib.TabelaMemoria.Distinct(info);

                listaErrados.TrimToSize();

                if (listaErrados.Count == ingressos.Rows.Count)
                    throw new PreImpressoGerenciadorException("Todos os códigos informados não pertencem a loja.");

                if (listaErrados.Count > 0)
                {
                    string codigos = Utilitario.ArrayToString((string[])listaErrados.ToArray(typeof(string)));
                    throw new PreImpressoGerenciadorException("Os códigos informados abaixo não pertencem a loja:\n\n" + codigos);
                }

                return info;
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
        /// Obter informacoes dos ingressos como valores, Eventos, apresentacoes e setores que pertencem a loja indicada
        /// </summary>
        /// <returns></returns>
        public DataTable InfoLoja(int lojaID)
        {

            try
            {

                DataTable info = new DataTable("Info");
                info.Columns.Add(EVENTO, typeof(string));
                info.Columns.Add(APRESENTACAO, typeof(string));
                info.Columns.Add(SETOR, typeof(string));
                info.Columns.Add(PRECO, typeof(string));
                info.Columns.Add(QTDE, typeof(int));
                info.Columns.Add(VALOR, typeof(decimal));

                string sql = "SELECT tEvento.Nome AS Evento, tApresentacao.Horario AS Apresentacao, tSetor.Nome AS Setor, tPreco.Nome AS Preco, " +
                    "SUM(tPreco.Valor) AS Valor, " +
                    "Count(tIngresso.ID) AS Qtde " +
                    "FROM tIngresso (NOLOCK) " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID  " +
                    "INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                    "INNER JOIN tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                    "WHERE tIngresso.LojaID=" + lojaID + " AND tIngresso.Status='" + Ingresso.PREIMPRESSO + "' " +
                    "GROUP BY tEvento.Nome, tApresentacao.Horario, tSetor.Nome, tPreco.Nome " +
                    "ORDER BY tEvento.Nome, tApresentacao.Horario, tSetor.Nome, tPreco.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaInfo = info.NewRow();
                    linhaInfo[EVENTO] = bd.LerString("Evento");
                    linhaInfo[APRESENTACAO] = bd.LerStringFormatoDataHora("Apresentacao");
                    linhaInfo[SETOR] = bd.LerString("Setor");
                    linhaInfo[PRECO] = bd.LerString("Preco");
                    linhaInfo[QTDE] = bd.LerInt("Qtde");
                    linhaInfo[VALOR] = bd.LerDecimal("Valor");
                    info.Rows.Add(linhaInfo);
                }
                bd.Fechar();

                return info;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Disponibilizar os pre-impressos selecionados. Torna-los disponiveis.
        /// </summary>
        /// <returns></returns>
        public int DisponibilizarSelecionados(int lojaID, int canalID, int empresaID, int usuarioID)
        {
            int n = info.Rows.Count;

            if (n > 0)
            {

                try
                {

                    bd.IniciarTransacao();

                    CodigoBarra oCodigoBarra = new CodigoBarra();
                    IngressoCodigoBarra oIngressoCodigoBarra = new IngressoCodigoBarra();

                    foreach (DataRow ingresso in info.Rows)
                    {

                        int ingressoID = (int)ingresso["IngressoID"];
                        int bloqueioID = (int)ingresso["BloqueioID"];
                        int PrecoID = (int)ingresso["PrecoID"];
                        int cortesiaID = (int)ingresso["CortesiaID"];
                        bool okV = false;
                        string status = (bloqueioID != 0) ? Ingresso.BLOQUEADO : Ingresso.DISPONIVEL;

                        string codigoBarra = string.Empty;

                        if (!string.IsNullOrEmpty(ingresso["TipoCodigoBarra"].ToString()) && Convert.ToChar(ingresso["TipoCodigoBarra"]) == (char)Enumerators.TipoCodigoBarra.ListaBranca)
                        {
                            codigoBarra = oCodigoBarra.NovoCodigoBarraListaBranca(bd, Convert.ToInt32(ingresso["ApresentacaoSetorID"]));

                            oIngressoCodigoBarra.Limpar();
                            oIngressoCodigoBarra.EventoID.Valor = Convert.ToInt32(ingresso["EventoID"]);
                            oIngressoCodigoBarra.CodigoBarra.Valor = codigoBarra;
                            oIngressoCodigoBarra.BlackList.Valor = false;
                            bd.Executar(oIngressoCodigoBarra.StringInserir());
                        }

                        string sql = "UPDATE tIngresso SET UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                            "Status='" + status + "' " +
                            "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + ingressoID;

                        int x = bd.Executar(sql);
                        bool ok = Convert.ToBoolean(x);

                        if (ok)
                        { //inserir na Log
                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.PrecoID.Valor = PrecoID;
                            ingressoLog.BloqueioID.Valor = bloqueioID;
                            ingressoLog.CortesiaID.Valor = cortesiaID;
                            ingressoLog.LojaID.Valor = lojaID;
                            ingressoLog.CanalID.Valor = canalID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.Acao.Valor = IngressoLog.CANCELAR_PREIMPRESSO;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            x = bd.Executar(sqlIngressoLogV);
                            okV = Convert.ToBoolean(x);
                            if (!okV)
                                throw new BilheteriaException("Log de cancelamento do pré-impresso não foi inserido.");
                        }

                        if (okV)
                        {
                            oIngressoCodigoBarra.Limpar();
                            oIngressoCodigoBarra.EventoID.Valor = Convert.ToInt32(ingresso["EventoID"]);
                            oIngressoCodigoBarra.CodigoBarra.Valor = ingresso["CodigoBarra"].ToString();
                            oIngressoCodigoBarra.TimeStamp.Valor = DateTime.Now;
                            oIngressoCodigoBarra.BlackList.Valor = true;

                            bd.Executar(oIngressoCodigoBarra.StringUpdate());

                        }
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

            return n;

        }

        /// <summary>
        /// Disponibilizar os pre-impressos restantes. Torna-los disponiveis.
        /// </summary>
        /// <returns></returns>
        public int Disponibilizar(int lojaID, int canalID, int empresaID, int usuarioID)
        {

            try
            {

                info.Clear();

                string sql = "SELECT tIngresso.ID, PrecoID, BloqueioID, CortesiaID, EventoID, CodigoBarra, tEvento.TipoCodigoBarra, ApresentacaoSetorID " +
                    "FROM tIngresso (NOLOCK) " +
                    "INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID " +
                    "WHERE LojaID=" + lojaID + " AND Status='" + Ingresso.PREIMPRESSO + "'";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = info.NewRow();
                    linha["IngressoID"] = bd.LerInt("ID");
                    linha["PrecoID"] = bd.LerInt("PrecoID");
                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    linha["EventoID"] = bd.LerInt("EventoID");
                    linha["CodigoBarra"] = bd.LerString("CodigoBarra");
                    linha["TipoCodigoBarra"] = bd.LerString("TipoCodigoBarra");
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    info.Rows.Add(linha);
                }
                bd.Consulta().Close();

            }
            catch
            {
                info.Clear();
            }

            int n = info.Rows.Count;

            if (n > 0)
            {

                try
                {

                    bd.IniciarTransacao();

                    IngressoCodigoBarra oIngressoCodigoBarra = new IngressoCodigoBarra();
                    CodigoBarra oCodigoBarra = new CodigoBarra();

                    foreach (DataRow ingresso in info.Rows)
                    {

                        int ingressoID = (int)ingresso["IngressoID"];
                        int bloqueioID = (int)ingresso["BloqueioID"];
                        int PrecoID = (int)ingresso["PrecoID"];
                        int cortesiaID = (int)ingresso["CortesiaID"];
                        string codigoBarra = ingresso["CodigoBarra"].ToString();

                        if (codigoBarra.Length > 0)
                        {
                            oIngressoCodigoBarra.Limpar();
                            oIngressoCodigoBarra.EventoID.Valor = Convert.ToInt32(ingresso["EventoID"]);
                            oIngressoCodigoBarra.CodigoBarra.Valor = codigoBarra;
                            oIngressoCodigoBarra.BlackList.Valor = true;
                            bd.Executar(oIngressoCodigoBarra.StringUpdate());
                        }

                        if (Convert.ToChar(ingresso["TipoCodigoBarra"]) == (char)Enumerators.TipoCodigoBarra.ListaBranca)
                        {
                            codigoBarra = oCodigoBarra.NovoCodigoBarraListaBranca(bd, Convert.ToInt32(ingresso["ApresentacaoSetorID"]));

                            oIngressoCodigoBarra.Limpar();
                            oIngressoCodigoBarra.EventoID.Valor = Convert.ToInt32(ingresso["EventoID"]);
                            oIngressoCodigoBarra.CodigoBarra.Valor = codigoBarra;
                            oIngressoCodigoBarra.BlackList.Valor = false;
                            bd.Executar(oIngressoCodigoBarra.StringInserir());
                        }
                        else
                            codigoBarra = string.Empty;

                        string status = (bloqueioID != 0) ? Ingresso.BLOQUEADO : Ingresso.DISPONIVEL;

                        string sql = "UPDATE tIngresso SET UsuarioID=0, PrecoID=0, CortesiaID=0, LojaID=0, CodigoBarra='" + codigoBarra + "', " +
                            "Status='" + status + "' " +
                            "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + ingressoID;

                        int x = bd.Executar(sql);
                        bool ok = Convert.ToBoolean(x);
                        if (ok)
                        { //inserir na Log
                            IngressoLog ingressoLog = new IngressoLog();
                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                            ingressoLog.IngressoID.Valor = ingressoID;
                            ingressoLog.UsuarioID.Valor = usuarioID;
                            ingressoLog.PrecoID.Valor = PrecoID;
                            ingressoLog.BloqueioID.Valor = bloqueioID;
                            ingressoLog.CortesiaID.Valor = cortesiaID;
                            ingressoLog.LojaID.Valor = lojaID;
                            ingressoLog.CanalID.Valor = canalID;
                            ingressoLog.EmpresaID.Valor = empresaID;
                            ingressoLog.Acao.Valor = IngressoLog.CANCELAR_PREIMPRESSO;
                            string sqlIngressoLogV = ingressoLog.StringInserir();
                            x = bd.Executar(sqlIngressoLogV);
                            bool okV = Convert.ToBoolean(x);
                            if (!okV)
                                throw new BilheteriaException("Log de cancelamento do pré-impresso não foi inserido.");
                        }

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

            return n;

        }

        /// <summary>
        /// Prestar contas. Retorna a senha da venda
        /// </summary>
        /// <returns></returns>
        public string PrestarConta(int lojaID, int caixaID, int usuarioID, int canalID, int empresaID, DataTable pagamentos, decimal valorTotal)
        {
            try
            {
                decimal valorTotalConv = 0;
                decimal valorTotalComissao = 0;
                decimal valorTaxaMaxima = 0;
                decimal valorTaxaMinima = 0;
                decimal valorComissaoMaxima = 0;
                decimal valorComissaoMinima = 0;
                bool ConvenienciaMaxima = false;
                bool ConvenienciaMinima = false;
                bool ComissaoMaxima = false;
                bool ComissaoMinima = false;

                Pacote pacote = new Pacote();
                VendaBilheteria vendaBilheteria = new VendaBilheteria();

                DataSet tabelasIngresso = new DataSet("tabelaIngresso");
                //Separa os itens diferentes em tabelas. Dessa forma é possível prestar contas dos itens separadamente.
                DataRow[] itensMesaFechada = info.Select("LugarMarcado='M' AND PacoteID=0");
                DataRow[] itensMesaAberta = info.Select("LugarMarcado='A' AND PacoteID=0");
                DataRow[] itensCadeira = info.Select("LugarMarcado='C' AND PacoteID=0");
                DataRow[] itensPista = info.Select("LugarMarcado='P' AND PacoteID=0");
                DataRow[] itensPacote = info.Select("PacoteID <> 0 AND PacoteID IS NOT NULL");

                object oValorConv = info.Compute("SUM(ValorConv)", "1=1");
                object oValorTaxaMaxima = info.Compute("SUM(TaxaMaxima)", "1=1");
                object oValorTaxaMinima = info.Compute("SUM(TaxaMinima)", "1=1");
                object oValorComissao = info.Compute("SUM(ValorComissao)", "1=1");
                object oValorComissaoMaxima = info.Compute("SUM(ComissaoMaxima)", "1=1");
                object oValorComissaoMinima = info.Compute("SUM(ComissaoMinima)", "1=1");
                valorTotalConv = (oValorConv != DBNull.Value) ? Convert.ToDecimal(oValorConv) : 0;
                valorTaxaMaxima = (oValorTaxaMaxima != DBNull.Value) ? Convert.ToDecimal(oValorTaxaMaxima) : 0;
                valorTaxaMinima = (oValorTaxaMinima != DBNull.Value) ? Convert.ToDecimal(oValorTaxaMinima) : 0;
                valorTotalComissao = (oValorComissao != DBNull.Value) ? Convert.ToDecimal(oValorComissao) : 0;
                valorComissaoMaxima = (oValorComissaoMaxima != DBNull.Value) ? Convert.ToDecimal(oValorComissaoMaxima) : 0;
                valorComissaoMinima = (oValorComissaoMinima != DBNull.Value) ? Convert.ToDecimal(oValorComissaoMinima) : 0;

                if (valorTotalConv > valorTaxaMaxima)
                {
                    valorTotalConv = valorTaxaMaxima;
                    ConvenienciaMaxima = true;
                }
                else if (valorTotalConv < valorTaxaMinima)
                {
                    valorTotalConv = valorTaxaMinima;
                    ConvenienciaMinima = true;
                }

                if (valorTotalComissao > valorComissaoMaxima)
                {
                    valorTotalComissao = valorComissaoMaxima;
                    ComissaoMaxima = true;
                }
                else if (valorTotalComissao < valorComissaoMinima)
                {
                    valorTotalComissao = valorComissaoMinima;
                    ComissaoMinima = true;
                }

                tabelasIngresso.Tables.Clear();
                //Preenche as tabelas com os dados.
                for (int i = 0; i < itensMesaFechada.Length; i++)
                    ingressosMesaFechada.ImportRow(itensMesaFechada[i]);

                for (int i = 0; i < itensMesaAberta.Length; i++)
                    ingressosMesaAberta.ImportRow(itensMesaAberta[i]);

                for (int i = 0; i < itensPista.Length; i++)
                    ingressosPista.ImportRow(itensPista[i]);

                for (int i = 0; i < itensCadeira.Length; i++)
                    ingressosCadeira.ImportRow(itensCadeira[i]);

                for (int i = 0; i < itensPacote.Length; i++)
                    ingressosPacote.ImportRow(itensPacote[i]);

                //Muda o nome das tabelas para que seja possível identifica-las mais tarde
                tabelasIngresso.Tables.Add(ingressosMesaFechada.Copy());
                tabelasIngresso.Tables[0].TableName = "ingressosMesaFechada";
                tabelasIngresso.Tables.Add(ingressosMesaAberta.Copy());
                tabelasIngresso.Tables[1].TableName = "ingressosMesaAberta";
                tabelasIngresso.Tables.Add(ingressosCadeira.Copy());
                tabelasIngresso.Tables[2].TableName = "ingressosCadeira";
                tabelasIngresso.Tables.Add(ingressosPista.Copy());
                tabelasIngresso.Tables[3].TableName = "ingressosPista";
                tabelasIngresso.Tables.Add(ingressosPacote.Copy());
                tabelasIngresso.Tables[4].TableName = "ingressosPacote";

                bd.IniciarTransacao();

                //fazer uma venda
                vendaBilheteria.CaixaID.Valor = caixaID;
                vendaBilheteria.Status.Valor = VendaBilheteria.PAGO;
                vendaBilheteria.DataVenda.Valor = System.DateTime.Now;
                vendaBilheteria.ValorTotal.Valor = valorTotal;
                vendaBilheteria.TaxaConvenienciaValorTotal.Valor = valorTotalConv;
                vendaBilheteria.ComissaoValorTotal.Valor = valorTotalComissao;
                vendaBilheteria.PagamentoProcessado.Valor = true;

                string sqlVendaBilheteria = vendaBilheteria.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                vendaBilheteria.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (vendaBilheteria.Control.ID == 0)
                    throw new BilheteriaException("Venda não foi gerada.");

                //gravar os itens da venda (ingressos)
                DataTable infoTemp = CTLib.TabelaMemoria.Distinct(info, "IngressoID");
                DataTable infoTempPacote = CTLib.TabelaMemoria.Distinct(ingressosPacote, "PacoteGrupo");
                DataTable infoTempMesaFechada = CTLib.TabelaMemoria.Distinct(ingressosMesaFechada, "LugarID");
                //Passa por todas as tabelas achando a ação adequada para cada uma.
                foreach (DataTable tabela in tabelasIngresso.Tables)
                {
                    //Ingressos de e mesa fechada
                    if (tabela.TableName == "ingressosPacote" || tabela.TableName == "ingressosMesaFechada")
                    {
                        //PACOTES
                        if (tabela.TableName == "ingressosPacote" && tabela.Rows.Count != 0)
                        {
                            IngressoLog ingressoLog = new IngressoLog();
                            VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();

                            //Insere os registros de VendaBilheteriaItem e IngressoLog. Atualiza tambem a tIngresso.
                            foreach (DataRow itemTempPacote in infoTempPacote.Rows)
                            {
                                string idTempPacoteGrupo = (string)itemTempPacote["PacoteGrupo"];

                                DataRow[] itemPacote = ingressosPacote.Select("PacoteGrupo='" + idTempPacoteGrupo + "'");

                                pacote.Ler((int)itemPacote[0]["PacoteID"]);

                                if (pacote.PermitirCancelamentoAvulso.Valor)
                                {
                                    //VendaBilheteriaItem, para pacotes deve inserir todos os itens
                                    foreach (DataRow item in itemPacote)
                                    {
                                        vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                        vendaBilheteriaItem.PacoteID.Valor = (int)item["PacoteID"];
                                        vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                                        vendaBilheteriaItem.TaxaConveniencia.Valor = (int)item["Conv"];

                                        if (ConvenienciaMaxima)
                                            vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(TaxaMaxima)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);
                                        else if (ConvenienciaMinima)
                                            vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(TaxaMinima)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);
                                        else
                                            vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ValorConv)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);

                                        vendaBilheteriaItem.TaxaComissao.Valor = (int)item["TaxaComissao"];

                                        if (ComissaoMaxima)
                                            vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ComissaoMaxima)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);
                                        else if (ComissaoMinima)
                                            vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ComissaoMinima)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);
                                        else
                                            vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ValorComissao)", "PacoteGrupo='" + idTempPacoteGrupo + "'") / itemPacote.Length);

                                        vendaBilheteriaItem.PacoteGrupo.Valor = int.Parse(item["PacoteGrupo"].ToString());

                                        string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                                        //Utiliza o consulta valor para pegar o ID de retorno.
                                        object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                                        vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                                        if (vendaBilheteriaItem.Control.ID == 0)
                                            throw new BilheteriaException("Item de venda não foi gerada.");
                                        //Passa pelos itens do pacote inserindo registros na tIngressoLog e atualizando o status da tIngresso.

                                        string sql = "UPDATE tIngresso SET " +
                                            "VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.IMPRESSO + "' " +
                                            "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + (int)item["IngressoID"];

                                        int x = bd.Executar(sql);
                                        bool ok = Convert.ToBoolean(x);
                                        if (ok)
                                        {
                                            //VENDA
                                            //inserir na Log o registro de VENDA
                                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                            ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                            ingressoLog.UsuarioID.Valor = usuarioID;
                                            ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                            ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                            ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                            ingressoLog.CaixaID.Valor = caixaID;
                                            ingressoLog.LojaID.Valor = lojaID;
                                            ingressoLog.CanalID.Valor = canalID;
                                            ingressoLog.EmpresaID.Valor = empresaID;
                                            ingressoLog.Acao.Valor = IngressoLog.VENDER;
                                            string sqlIngressoLogV = ingressoLog.StringInserir();
                                            x = bd.Executar(sqlIngressoLogV);
                                            bool okV = Convert.ToBoolean(x);
                                            if (!okV)
                                                throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                            //IMPRESSÃO
                                            //Pré-Impresso insere registro "P". Deve inserir um registro de IMPRESSÃO para fim de auditoria.
                                            ingressoLog = new IngressoLog();
                                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                            ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                            ingressoLog.UsuarioID.Valor = usuarioID;
                                            ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                            ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                            ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                            ingressoLog.CaixaID.Valor = caixaID;
                                            ingressoLog.LojaID.Valor = lojaID;
                                            ingressoLog.CanalID.Valor = canalID;
                                            ingressoLog.EmpresaID.Valor = empresaID;
                                            ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                                            ingressoLog.CodigoBarra.Valor = (string)item["CodigoBarra"];
                                            sqlIngressoLogV = ingressoLog.StringInserir();
                                            x = bd.Executar(sqlIngressoLogV);
                                            okV = Convert.ToBoolean(x);
                                            if (!okV)
                                                throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                        }
                                        else
                                        {
                                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                                        }
                                    }
                                }
                                else
                                {
                                    //VendaBilheteriaItem, para pacotes deve inserir somente 1 item
                                    vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                    vendaBilheteriaItem.PacoteID.Valor = (int)itemPacote[0]["PacoteID"];
                                    vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                                    vendaBilheteriaItem.TaxaConveniencia.Valor = (int)itemPacote[0]["Conv"];

                                    if (ConvenienciaMaxima)
                                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(TaxaMaxima)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));
                                    else if (ConvenienciaMinima)
                                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(TaxaMinima)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));
                                    else
                                        vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ValorConv)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));

                                    vendaBilheteriaItem.TaxaComissao.Valor = (int)itemPacote[0]["TaxaComissao"];

                                    if (ComissaoMaxima)
                                        vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ComissaoMaxima)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));
                                    else if (ComissaoMinima)
                                        vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ComissaoMinima)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));
                                    else
                                        vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosPacote.Compute("SUM(ValorComissao)", "PacoteGrupo='" + idTempPacoteGrupo + "'"));

                                    vendaBilheteriaItem.PacoteGrupo.Valor = int.Parse(itemPacote[0]["PacoteGrupo"].ToString());

                                    string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                                    //Utiliza o consulta valor para pegar o ID de retorno.
                                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                                    vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                                    if (vendaBilheteriaItem.Control.ID == 0)
                                        throw new BilheteriaException("Item de venda não foi gerada.");
                                    //Passa pelos itens do pacote inserindo registros na tIngressoLog e atualizando o status da tIngresso.
                                    foreach (DataRow item in itemPacote)
                                    {

                                        string sql = "UPDATE tIngresso SET " +
                                            "VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.IMPRESSO + "' " +
                                            "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + (int)item["IngressoID"];

                                        int x = bd.Executar(sql);
                                        bool ok = Convert.ToBoolean(x);
                                        if (ok)
                                        {
                                            //VENDA
                                            //inserir na Log o registro de VENDA

                                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                            ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                            ingressoLog.UsuarioID.Valor = usuarioID;
                                            ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                            ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                            ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                            ingressoLog.CaixaID.Valor = caixaID;
                                            ingressoLog.LojaID.Valor = lojaID;
                                            ingressoLog.CanalID.Valor = canalID;
                                            ingressoLog.EmpresaID.Valor = empresaID;
                                            ingressoLog.Acao.Valor = IngressoLog.VENDER;
                                            string sqlIngressoLogV = ingressoLog.StringInserir();
                                            x = bd.Executar(sqlIngressoLogV);
                                            bool okV = Convert.ToBoolean(x);
                                            if (!okV)
                                                throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                            //IMPRESSÃO
                                            //Pré-Impresso insere registro "P". Deve inserir um registro de IMPRESSÃO para fim de auditoria.
                                            ingressoLog = new IngressoLog();
                                            ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                            ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                            ingressoLog.UsuarioID.Valor = usuarioID;
                                            ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                            ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                            ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                            ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                            ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                            ingressoLog.CaixaID.Valor = caixaID;
                                            ingressoLog.LojaID.Valor = lojaID;
                                            ingressoLog.CanalID.Valor = canalID;
                                            ingressoLog.EmpresaID.Valor = empresaID;
                                            ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                                            ingressoLog.CodigoBarra.Valor = (string)item["CodigoBarra"];
                                            sqlIngressoLogV = ingressoLog.StringInserir();
                                            x = bd.Executar(sqlIngressoLogV);
                                            okV = Convert.ToBoolean(x);
                                            if (!okV)
                                                throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                        }
                                        else
                                        {
                                            throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                                        }
                                    }
                                }
                            }
                        }
                        //MESA FECHADA
                        if (tabela.TableName == "ingressosMesaFechada" && tabela.Rows.Count != 0)
                        {
                            foreach (DataRow itemTempMesaFechada in infoTempMesaFechada.Rows)
                            {

                                int idTempLugarID = (int)itemTempMesaFechada["LugarID"];

                                DataRow[] itemMesaFechada = ingressosMesaFechada.Select("LugarID=" + idTempLugarID, "IngressoID");
                                //VendaBilheteriaItem, para Mesa Fechada deve inserir apenas 1 registro
                                VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                                vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                vendaBilheteriaItem.PacoteID.Valor = 0;
                                vendaBilheteriaItem.PacoteGrupo.Valor = 0;
                                vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                                vendaBilheteriaItem.TaxaConveniencia.Valor = (int)itemMesaFechada[0]["Conv"];

                                if (ConvenienciaMaxima)
                                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(TaxaMaxima)", "LugarID=" + idTempLugarID));
                                else if (ConvenienciaMinima)
                                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(TaxaMinima)", "LugarID=" + idTempLugarID));
                                else
                                    vendaBilheteriaItem.TaxaConvenienciaValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(ValorConv)", "LugarID=" + idTempLugarID));

                                vendaBilheteriaItem.TaxaComissao.Valor = (int)itemMesaFechada[0]["TaxaComissao"];

                                if (ComissaoMaxima)
                                    vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(ComissaoMaxima)", "LugarID=" + idTempLugarID));
                                else if (ComissaoMinima)
                                    vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(ComissaoMinima)", "LugarID=" + idTempLugarID));
                                else
                                    vendaBilheteriaItem.ComissaoValor.Valor = ((decimal)ingressosMesaFechada.Compute("SUM(ValorComissao)", "LugarID=" + idTempLugarID));

                                string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                                object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                                vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                                if (vendaBilheteriaItem.Control.ID == 0)
                                    throw new BilheteriaException("Item de venda não foi gerada.");
                                //PAssa por cada item da mesa fechada, atualizando a tIngresso e inserindo registros na tIngressoLog
                                foreach (DataRow item in itemMesaFechada)
                                {

                                    string sql = "UPDATE tIngresso SET " +
                                        "VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.IMPRESSO + "' " +
                                        "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + (int)item["IngressoID"];

                                    int x = bd.Executar(sql);

                                    bool ok = Convert.ToBoolean(x);
                                    if (ok)
                                    {
                                        //VENDA
                                        //inserir na Log o registro de VENDA
                                        IngressoLog ingressoLog = new IngressoLog();
                                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                        ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                        ingressoLog.UsuarioID.Valor = usuarioID;
                                        ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                        ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                        ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                        ingressoLog.CaixaID.Valor = caixaID;
                                        ingressoLog.LojaID.Valor = lojaID;
                                        ingressoLog.CanalID.Valor = canalID;
                                        ingressoLog.EmpresaID.Valor = empresaID;
                                        ingressoLog.Acao.Valor = IngressoLog.VENDER;
                                        string sqlIngressoLogV = ingressoLog.StringInserir();
                                        //Insere e valida
                                        x = bd.Executar(sqlIngressoLogV);
                                        bool okV = Convert.ToBoolean(x);
                                        if (!okV)
                                            throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");

                                        //IMPRESSÃO
                                        //Pré-Impresso insere registro "P". Deve inserir um registro de IMPRESSÃO para fim de auditoria.
                                        ingressoLog = new IngressoLog();
                                        ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                        ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                        ingressoLog.UsuarioID.Valor = usuarioID;
                                        ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                        ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                        ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                        ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                        ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                        ingressoLog.CaixaID.Valor = caixaID;
                                        ingressoLog.LojaID.Valor = lojaID;
                                        ingressoLog.CanalID.Valor = canalID;
                                        ingressoLog.EmpresaID.Valor = empresaID;
                                        ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                                        ingressoLog.CodigoBarra.Valor = (string)item["CodigoBarra"];
                                        sqlIngressoLogV = ingressoLog.StringInserir();
                                        //Insere e valida
                                        x = bd.Executar(sqlIngressoLogV);
                                        okV = Convert.ToBoolean(x);
                                        if (!okV)
                                            throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                    }
                                    else
                                    {
                                        throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow itemTemp in tabela.Rows)
                        {
                            int idTemp = (int)itemTemp["IngressoID"];
                            DataRow[] itens = tabela.Select("IngressoID=" + idTemp, "IngressoID");

                            VendaBilheteriaItem vendaBilheteriaItem = new VendaBilheteriaItem();
                            vendaBilheteriaItem.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                            vendaBilheteriaItem.PacoteID.Valor = 0;
                            vendaBilheteriaItem.PacoteGrupo.Valor = 0;
                            vendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;

                            vendaBilheteriaItem.TaxaConveniencia.Valor = (int)itens[0]["Conv"];

                            if (ConvenienciaMaxima)
                                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)itens[0]["TaxaMaxima"];
                            else if (ConvenienciaMinima)
                                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)itens[0]["TaxaMinima"];
                            else
                                vendaBilheteriaItem.TaxaConvenienciaValor.Valor = (decimal)itens[0]["ValorConv"];

                            vendaBilheteriaItem.TaxaComissao.Valor = (int)itens[0]["TaxaComissao"];

                            if (ComissaoMaxima)
                                vendaBilheteriaItem.ComissaoValor.Valor = (decimal)itens[0]["ComissaoMaxima"];
                            else if (ComissaoMinima)
                                vendaBilheteriaItem.ComissaoValor.Valor = (decimal)itens[0]["ComissaoMinima"];
                            else
                                vendaBilheteriaItem.ComissaoValor.Valor = (decimal)itens[0]["ValorComissao"];

                            string sqlVendaBilheteriaItem = vendaBilheteriaItem.StringInserir();
                            object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                            vendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                            if (vendaBilheteriaItem.Control.ID == 0)
                                throw new BilheteriaException("Item de venda não foi gerada.");

                            foreach (DataRow item in itens)
                            {
                                string sql = "UPDATE tIngresso SET " +
                                    "VendaBilheteriaID=" + vendaBilheteria.Control.ID + ", Status='" + Ingresso.IMPRESSO + "' " +
                                    "WHERE Status='" + Ingresso.PREIMPRESSO + "' AND ID=" + (int)item["IngressoID"];

                                int x = bd.Executar(sql);

                                bool ok = Convert.ToBoolean(x);
                                if (ok)
                                { //inserir na Log
                                    IngressoLog ingressoLog = new IngressoLog();
                                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                    ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                    ingressoLog.UsuarioID.Valor = usuarioID;
                                    ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                    ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                    ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                    ingressoLog.CaixaID.Valor = caixaID;
                                    ingressoLog.LojaID.Valor = lojaID;
                                    ingressoLog.CanalID.Valor = canalID;
                                    ingressoLog.EmpresaID.Valor = empresaID;
                                    ingressoLog.Acao.Valor = IngressoLog.VENDER;

                                    string sqlIngressoLogV = ingressoLog.StringInserir();
                                    x = bd.Executar(sqlIngressoLogV);
                                    bool okV = Convert.ToBoolean(x);
                                    if (!okV)
                                        throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");

                                    //Insere tambem o registro de impressão na log para fins de 
                                    ingressoLog = new IngressoLog();
                                    ingressoLog.TimeStamp.Valor = System.DateTime.Now;
                                    ingressoLog.IngressoID.Valor = (int)item["IngressoID"];
                                    ingressoLog.UsuarioID.Valor = usuarioID;
                                    ingressoLog.PrecoID.Valor = (int)item["PrecoID"];
                                    ingressoLog.BloqueioID.Valor = (int)item["BloqueioID"];
                                    ingressoLog.CortesiaID.Valor = (int)item["CortesiaID"];
                                    ingressoLog.VendaBilheteriaItemID.Valor = vendaBilheteriaItem.Control.ID;
                                    ingressoLog.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                                    ingressoLog.CaixaID.Valor = caixaID;
                                    ingressoLog.LojaID.Valor = lojaID;
                                    ingressoLog.CanalID.Valor = canalID;
                                    ingressoLog.EmpresaID.Valor = empresaID;
                                    ingressoLog.Acao.Valor = IngressoLog.IMPRIMIR;
                                    ingressoLog.CodigoBarra.Valor = (string)item["CodigoBarra"];
                                    sqlIngressoLogV = ingressoLog.StringInserir();
                                    x = bd.Executar(sqlIngressoLogV);
                                    okV = Convert.ToBoolean(x);
                                    if (!okV)
                                        throw new BilheteriaException("Log de venda do pré-impresso não foi inserido.");
                                }
                                else
                                {
                                    throw new BilheteriaException("Status do ingresso não pode ser atualizado.");
                                }
                            }
                        }
                    }
                }

                //dividir o valorTotal nas formas de pagamento

                decimal porcentagemTotal = 0;

                for (int i = 0; i < pagamentos.Rows.Count; i++)
                {
                    DataRow pagto = pagamentos.Rows[i];
                    VendaBilheteriaFormaPagamento vendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();
                    vendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = (int)pagto["ID"];
                    vendaBilheteriaFormaPagamento.Valor.Valor = (decimal)pagto["Valor"];
                    //calcular porcentagem
                    decimal porc = (vendaBilheteriaFormaPagamento.Valor.Valor * 100) / valorTotal;
                    decimal porcentagem = Math.Round(porc, 2);

                    //a porcentagem final tem q dar 100%
                    if (i != (pagamentos.Rows.Count - 1))
                    {
                        porcentagemTotal += porcentagem;
                    }
                    else
                    { //eh a última parcela
                        decimal totalTmp = porcentagemTotal + porcentagem;
                        if (totalTmp != 100)
                        {
                            porcentagem = 100 - porcentagemTotal;
                            porcentagemTotal += porcentagem;
                        }
                    }
                    vendaBilheteriaFormaPagamento.Porcentagem.Valor = porcentagem;
                    vendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = vendaBilheteria.Control.ID;
                    string sqlVendaBilheteriaFormaPagamento = vendaBilheteriaFormaPagamento.StringInserir();
                    int x = bd.Executar(sqlVendaBilheteriaFormaPagamento);
                    bool ok = Convert.ToBoolean(x >= 1);
                    if (!ok)
                        throw new BilheteriaException("Forma de pagamento não foi cadastrada.");

                }
                bd.FinalizarTransacao();

                string sqlSenha = "SELECT Senha FROM tVendaBilheteria WHERE ID=" + vendaBilheteria.Control.ID;
                object ret = bd.ConsultaValor(sqlSenha);
                string senha = (ret != null) ? Convert.ToString(ret) : null;

                return senha;
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

        #region Relatótios WEB

        /// <summary>
        ///  Carrega os Canais relacionados a um determinado localID
        /// </summary>
        /// <param name="localID"></param>
        /// <returns></returns>
        public DataTable CarregaCanaisPreImpresso(int LocalID, int EmpresaID, int RegionalID, string registroZero)
        {
            //Estrutura tabela
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                if (EmpresaID <= 0 && RegionalID <= 0 && LocalID <= 0)
                    throw new Exception("É necessário definir ao menos um parâmetro para busca.");

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome " +
                    "FROM " +
                    "   tCanal " +
                    "INNER JOIN " +
                    "   tCanalEvento " +
                    "ON " +
                    "   tCanal.ID = tCanalEvento.CanalID " +
                    "INNER JOIN " +
                    "   tEvento " +
                    "ON " +
                    "   tEvento.ID = tCanalEvento.EventoID " +
                    ((EmpresaID > 0 || RegionalID > 0) ? "" +
                        "INNER JOIN " +
                        "   tLocal " +
                        "ON " +
                        "   tLocal.ID = tEvento.LocalID " +
                        ((RegionalID > 0) ? "" +
                            "INNER JOIN " +
                            "   tEmpresa " +
                            "ON " +
                            "   tEmpresa.ID = tLocal.EmpresaID " +
                            "" : "") +
                            "" : "") +
                    "WHERE " +
                    "  (1 = 1) " +
                    ((LocalID > 0) ? "" +
                        "AND (tEvento.LocalID = " + LocalID + ") " : "") +
                    ((EmpresaID > 0) ? "" +
                    "AND (tLocal.EmpresaID = " + EmpresaID + ") " : "") +
                    ((RegionalID > 0) ? "" +
                    "AND (tEmpresa.RegionalID = " + RegionalID + ") " : "") +
                    "ORDER BY " +
                    "  tCanal.Nome"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linhaTabela = tabela.NewRow();
                        linhaTabela["ID"] = bd.LerInt("ID");
                        linhaTabela["Nome"] = bd.LerString("Nome");

                        tabela.Rows.Add(linhaTabela);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }

        /// <summary>
        ///  Carrega os Canais relacionados a um determinado empresa
        /// </summary>
        /// <param name="localID"></param>
        /// <returns></returns>
        public DataTable CarregaCanaisPreImpresso(int EmpresaID, int RegionalID, string registroZero)
        {
            //Estrutura tabela
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome " +
                    "FROM " +
                    "   tCanal " +
                    "INNER JOIN " +
                    "   tCanalEvento " +
                    "ON " +
                    "   tCanal.ID = tCanalEvento.CanalID " +
                    "INNER JOIN " +
                    "   tEvento " +
                    "ON " +
                    "   tEvento.ID = tCanalEvento.EventoID " +
                    "INNER JOIN " +
                    "  tLocal " +
                    "ON " +
                    "   tLocal.ID = tEvento.LocalID " +
                    ((RegionalID > 0) ? "" +
                        "INNER JOIN tEmpresa ON tEmpresa.ID = tLocal.EmpresaID " : "") +
                    "WHERE " +
                    "  (tLocal.EmpresaID = " + EmpresaID + " OR  tCanal.EmpresaID = " + EmpresaID + ") " +
                    ((RegionalID > 0) ? "" +
                        "AND (tEmpresa.RegionalID = " + RegionalID + ") " : "")))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linhaTabela = tabela.NewRow();
                        linhaTabela["ID"] = bd.LerInt("ID");
                        linhaTabela["Nome"] = bd.LerString("Nome");

                        tabela.Rows.Add(linhaTabela);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }

        /// <summary>
        /// Carrega os Eventos filtrando por Empresa e Canal
        /// </summary>
        /// <param name="localID"></param>
        /// <param name="EmpresaID"></param>
        /// <returns></returns>
        public DataTable CarregaEventosEmpresaECanal(string registroZero, int EmpresaID, int RegionalID, int LocalID, int CanalID)
        {

            //Estrutura tabela
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {

                if (EmpresaID <= 0 && RegionalID <= 0 && LocalID <= 0 && CanalID <= 0)
                    throw new Exception("É necessário definir ao menos um parâmetro para busca.");

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT DISTINCT " +
                    "   tEvento.ID, " +
                    "   tEvento.Nome " +
                    "FROM " +
                    "   tEvento " +
                    "INNER JOIN " +
                    "   tLocal " +
                    "ON " +
                    "   tEvento.LocalID = tLocal.ID " +
                    "INNER JOIN " +
                    "   tCanalEvento " +
                    "ON " +
                    "   tEvento.ID = tCanalEvento.EventoID " +
                    "INNER JOIN " +
                    "   tCanal " +
                    "ON " +
                    "   tCanal.ID = tCanalEvento.CanalID " +
                    ((RegionalID > 0) ? "" +
                        "INNER JOIN " +
                        "  tEmpresa " +
                        "ON " +
                        "  tEmpresa.ID = tLocal.EmpresaID " : "") +
                    "WHERE " +
                    "   (1 = 1) " +
                    ((EmpresaID > 0) ? "AND (tLocal.EmpresaID = " + EmpresaID + " OR tCanal.EmpresaID = " + EmpresaID + ") " : "") +
                    ((RegionalID > 0) ? "AND (tEmpresa.RegionalID = " + RegionalID + ") " : "") +
                    ((LocalID > 0) ? "AND (tEvento.LocalID = " + LocalID + ") " : "") +
                    ((CanalID > 0) ? "AND (tCanalEvento.CanalID = " + CanalID + ") " : "") +
                    "ORDER BY " +
                    "  tEvento.Nome"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linhaTabela = tabela.NewRow();
                        linhaTabela["ID"] = bd.LerInt("ID");
                        linhaTabela["Nome"] = bd.LerString("Nome");

                        tabela.Rows.Add(linhaTabela);
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

            return tabela;

        }

        public DataTable CarregaEventosLocalECanal(string registroZero, int LocalID, int CanalID)
        {

            //Estrutura tabela
            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Nome", typeof(string));

            try
            {
                string sql;


                sql = @"SELECT tEvento.ID,tEvento.Nome
							FROM tEvento 
							INNER JOIN tLocal ON tEvento.LocalID = tLocal.ID 
							INNER JOIN tCanalEvento ON tEvento.ID = tCanalEvento.EventoID
							WHERE tCanalEvento.CanalID =" + CanalID + " AND tLocal.ID =" + LocalID + " ORDER BY tEvento.Nome";


                bd.Consulta(sql);

                if (registroZero != null)
                    tabela.Rows.Add(new Object[] { 0, registroZero });

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();
                    linhaTabela["ID"] = bd.LerInt("ID");
                    linhaTabela["Nome"] = bd.LerString("Nome");

                    tabela.Rows.Add(linhaTabela);
                }

                bd.Consulta().Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return tabela;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable getDataPreImpressoCanal(int EmpresaID, int RegionalID, int CanalID, int LocalID, int LojaID, int EventoID, int ApresentacaoID)
        {
            //estruturta da tabela
            DataTable tabela = new DataTable("PreImpressoCanal");
            tabela.Columns.Add("CanalID", typeof(int));
            tabela.Columns.Add("Canal", typeof(string));
            tabela.Columns.Add("LojaID", typeof(int));
            tabela.Columns.Add("Loja", typeof(string));
            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("Horario", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("PrecoValor", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(decimal));
            tabela.Columns.Add("Valor", typeof(decimal));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tCanal.ID AS CanalID, " +
                    "   tCanal.Nome AS Canal, " +
                    "   tLoja.ID AS LojaID, " +
                    "   tLoja.Nome AS Loja, " +
                    "   tEvento.ID AS EventoID, " +
                    "   tEvento.nome AS Evento, " +
                    "   tApresentacao.ID AS ApresentacaoID, " +
                    "   tApresentacao.Horario, " +
                    "   tSetor.Nome AS Setor, " +
                    "   tPreco.Nome AS Preco, " +
                    "   tPreco.Valor AS PrecoValor, " +
                    "   COUNT(tIngresso.ID) AS Quantidade, " +
                    "   SUM(tPreco.valor) AS Valor " +
                    "FROM " +
                    "  tEvento (NOLOCK) " +
                    "INNER JOIN " +
                    "  tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
                    "INNER JOIN " +
                    "  tIngresso (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN " +
                    "  tLoja (NOLOCK) ON tLoja.ID = tIngresso.LojaID " +
                    "INNER JOIN " +
                    "  tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID " +
                    "INNER JOIN " +
                    "  tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                    "INNER JOIN " +
                    "  tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                    ((RegionalID > 0) ? "" +
                        "INNER JOIN " +
                        "  tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID " +
                        "INNER JOIN " +
                        "  tEmpresa (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID " +
                    "" : "") +
                    "WHERE " +
                    "  (tIngresso.Status = '" + Ingresso.PREIMPRESSO + "') " +
                    "AND " +
                    "   (tApresentacao.DisponivelRelatorio = 'T') " +
                    ((EmpresaID > 0) ? "" +
                    "AND " +
                    "   (tIngresso.EmpresaID = " + EmpresaID + ") " : "") +
                    ((CanalID > 0) ? "" +
                    "AND " +
                    "   (tCanal.ID = " + CanalID + ") " : "") +
                    ((LocalID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.LocalID = " + LocalID + ") " : "") +
                    ((LojaID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.LojaID = " + LojaID + ") " : "") +
                    ((EventoID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.EventoID = " + EventoID + ") " : "") +
                    ((ApresentacaoID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.ApresentacaoID = " + ApresentacaoID + ") " : "") +
                    ((RegionalID > 0) ? "" +
                    "AND " +
                    "  (tEmpresa.RegionalID = " + RegionalID + ") " : "") +
                    "GROUP BY " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome, " +
                    "   tLoja.ID, " +
                    "   tLoja.Nome, " +
                    "   tEvento.ID, " +
                    "   tEvento.Nome, " +
                    "   tApresentacao.ID, " +
                    "   tApresentacao.Horario, " +
                    "   tSetor.Nome, " +
                    "   tPreco.Nome, " +
                    "   tPreco.Valor " +
                    "ORDER BY " +
                    "  tCanal.Nome, " +
                    "  tLoja.Nome, " +
                    "  tApresentacao.Horario, " +
                    "  tSetor.Nome, " +
                     " tPreco.Nome"))
                {
                    while (oDataReader.Read())
                    {
                        DataRow linhaTabela = tabela.NewRow();

                        linhaTabela["CanalID"] = bd.LerInt("CanalID");
                        linhaTabela["Canal"] = bd.LerString("Canal");
                        linhaTabela["LojaID"] = bd.LerInt("LojaID");
                        linhaTabela["Loja"] = bd.LerString("Loja");
                        linhaTabela["EventoID"] = bd.LerInt("EventoID");
                        linhaTabela["Evento"] = bd.LerString("Evento");
                        linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        linhaTabela["Horario"] = bd.LerStringFormatoSemanaDataHora("Horario");
                        linhaTabela["Setor"] = bd.LerString("Setor");
                        linhaTabela["Preco"] = bd.LerString("Preco");
                        linhaTabela["PrecoValor"] = bd.LerString("PrecoValor");
                        linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
                        linhaTabela["Valor"] = bd.LerDecimal("Valor");

                        tabela.Rows.Add(linhaTabela);
                    }
                }
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

        public DataTable CarregaTabelaPreImpressoCanal(int EmpresaID, int RegionalID, int CanalID, int LocalID, int LojaID, int EventoID, int ApresentacaoID)
        {
            //variaveis
            object valorAuxiliar = 0;
            decimal qtdIngresso = 0;
            decimal valorTotalPreco = 0;
            decimal qtdTotalIngresso = 0;
            decimal valorTotal = 0;
            string Canal = string.Empty;
            string Loja = string.Empty;
            string Evento = string.Empty;
            string Apresentacao = string.Empty;
            string Setor = string.Empty;
            string Preco = string.Empty;
            DataRow linhaFinal = null;

            DataTable tabela = getDataPreImpressoCanal(EmpresaID, RegionalID, CanalID, LocalID, LojaID, EventoID, ApresentacaoID);

            //tabela que carrega o Grid
            DataTable tabelaFinal = Utilitario.EstruturaPreImpressoCanal();

            DataTable dtCanal = CTLib.TabelaMemoria.Distinct(tabela, "Canal");
            foreach (DataRow linhaCanal in dtCanal.Rows)
            {
                //insere nome do Canal
                linhaFinal = tabelaFinal.NewRow();

                tabelaFinal.Rows.Add(linhaFinal);
                linhaFinal["Apresentação"] = "<div style='text-align:left;font-weight:bold'>Canal " + linhaCanal["Canal"] + "</div>";
                Canal = linhaCanal["Canal"].ToString();


                DataTable dtLoja = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Loja", "Canal = '" + Canal + "'");
                foreach (DataRow linhaLoja in dtLoja.Rows)
                {
                    //insere nome da Loja
                    linhaFinal = tabelaFinal.NewRow();

                    tabelaFinal.Rows.Add(linhaFinal);
                    linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:40px'>Loja " + linhaLoja["Loja"] + "</div>";
                    Loja = linhaLoja["Loja"].ToString();



                    DataTable dtEvento = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Evento", "Canal = '" + Canal + "' and Loja = '" + Loja + "'");
                    foreach (DataRow linhaEvento in dtEvento.Rows)
                    {
                        //insere nome do Evento
                        linhaFinal = tabelaFinal.NewRow();

                        tabelaFinal.Rows.Add(linhaFinal);
                        linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:75px'>Evento " + linhaEvento["Evento"] + "</div>";
                        Evento = linhaEvento["Evento"].ToString();


                        DataTable dtApresentacao = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Horario", "Canal = '" + Canal + "' and Loja = '" + Loja + "' and Evento = '" + Evento + "'");
                        foreach (DataRow linhaApresentacao in dtApresentacao.Rows)
                        {
                            //insere nome da Apresentacao
                            linhaFinal = tabelaFinal.NewRow();

                            tabelaFinal.Rows.Add(linhaFinal);
                            linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:105px'>" + linhaApresentacao["Horario"] + "</div>";
                            Apresentacao = linhaApresentacao["Horario"].ToString();


                            DataTable dtSetor = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Setor", "Canal = '" + Canal + "' and Loja = '" + Loja + "' and Evento = '" + Evento + "' and Horario = '" + Apresentacao + "'");
                            foreach (DataRow linhaSetor in dtSetor.Rows)
                            {
                                //insere nome do Setor
                                linhaFinal = tabelaFinal.NewRow();

                                tabelaFinal.Rows.Add(linhaFinal);
                                linhaFinal["Setor"] = "<div style='text-align:left';>" + linhaSetor["Setor"].ToString() + "</div>";
                                Setor = linhaSetor["Setor"].ToString();

                                DataTable dtPreco = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Preco", "Canal = '" + Canal + "' and Loja = '" + Loja + "' and Evento = '" + Evento + "' and Horario = '" + Apresentacao + "' and Setor = '" + Setor + "'");
                                foreach (DataRow linhaPreco in dtPreco.Rows)
                                {
                                    //insere nome da Loja
                                    linhaFinal = tabelaFinal.NewRow();

                                    tabelaFinal.Rows.Add(linhaFinal);
                                    linhaFinal["Preço"] = "<div style='text-align:left;'>" + linhaPreco["Preco"].ToString() + "</div>";
                                    Preco = linhaPreco["Preco"].ToString();

                                    //qtde total para o Preco 
                                    valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "' and Horario = '" + Apresentacao + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "'");
                                    qtdIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                                    //valor total para linha de Preco
                                    valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "' and Horario = '" + Apresentacao + "' and  Setor = '" + Setor + "' and Preco = '" + Preco + "'");
                                    valorTotalPreco = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


                                    linhaFinal["Quantidade"] = qtdIngresso;
                                    linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotalPreco) + "</div>";
                                    linhaFinal["Unitário"] = Utilitario.AplicaFormatoMoeda(valorTotalPreco / qtdIngresso);
                                }


                            }
                            //total Apresentacao
                            linhaFinal = tabelaFinal.NewRow();
                            linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:105px'>Total Apresentação " + Apresentacao + "</div>";
                            //qtde total 
                            valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "' and Horario = '" + Apresentacao + "'");
                            qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                            //valor total
                            valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "' and Horario = '" + Apresentacao + "'");
                            valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                            linhaFinal["Quantidade"] = qtdTotalIngresso;
                            linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                            tabelaFinal.Rows.Add(linhaFinal);


                        }
                        //total Evento
                        linhaFinal = tabelaFinal.NewRow();
                        linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:75px'>Total Evento " + Evento + "</div>";
                        //qtde total 
                        valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "'");
                        qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        //valor total
                        valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "' and Loja= '" + Loja + "' and Evento= '" + Evento + "'");
                        valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        linhaFinal["Quantidade"] = qtdTotalIngresso;
                        linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                        tabelaFinal.Rows.Add(linhaFinal);

                    }
                    //total Loja
                    linhaFinal = tabelaFinal.NewRow();
                    linhaFinal["Apresentação"] = "<div style='text-align:left;margin-left:40px'>Total Loja " + Loja + "</div>";
                    //qtde total 
                    valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Canal= '" + Canal + "' and Loja= '" + Loja + "'");
                    qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                    //valor total
                    valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "' and Loja= '" + Loja + "'");
                    valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                    linhaFinal["Quantidade"] = qtdTotalIngresso;
                    linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                    tabelaFinal.Rows.Add(linhaFinal);


                }
                //total canal
                linhaFinal = tabelaFinal.NewRow();
                linhaFinal["Apresentação"] = "<div style='text-align:left;font-weight:bold'>Total Canal " + Canal + "</div>";
                //qtde total 
                valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Canal= '" + Canal + "'");
                qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                //valor total
                valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "'");
                valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                linhaFinal["Quantidade"] = qtdTotalIngresso;
                linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                tabelaFinal.Rows.Add(linhaFinal);


            }
            //linha em branco
            linhaFinal = tabelaFinal.NewRow();
            tabelaFinal.Rows.Add(linhaFinal);

            //Total Geral
            linhaFinal = tabelaFinal.NewRow();
            linhaFinal["Apresentação"] = "<div style='text-align:left;'>Total</div>";
            //qtde total 
            valorAuxiliar = tabela.Compute("SUM(Quantidade)", "1=1");
            qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

            //valor total
            valorAuxiliar = tabela.Compute("SUM(Valor)", "1=1");
            valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

            linhaFinal["Quantidade"] = qtdTotalIngresso;
            linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
            tabelaFinal.Rows.Add(linhaFinal);




            return tabelaFinal;

        }

        public DataTable getDataPreImpressoEvento(int EmpresaID, int RegionalID, int EventoID, int LocalID, int ApresentacaoID, int SetorID, int PrecoID)
        {
            //estruturta da tabela
            DataTable tabela = new DataTable("PreImpressoEvento");

            tabela.Columns.Add("EventoID", typeof(int));
            tabela.Columns.Add("Evento", typeof(string));
            tabela.Columns.Add("ApresentacaoID", typeof(int));
            tabela.Columns.Add("Horario", typeof(string));
            tabela.Columns.Add("Setor", typeof(string));
            tabela.Columns.Add("Preco", typeof(string));
            tabela.Columns.Add("CanalID", typeof(int));
            tabela.Columns.Add("Canal", typeof(string));
            tabela.Columns.Add("LojaID", typeof(int));
            tabela.Columns.Add("Loja", typeof(string));
            tabela.Columns.Add("Quantidade", typeof(decimal));
            tabela.Columns.Add("Valor", typeof(decimal));

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tEvento.ID AS EventoID, " +
                    "   tEvento.Nome AS Evento, " +
                    "   tApresentacao.ID AS ApresentacaoID, " +
                    "   tApresentacao.Horario, " +
                    "   tCanal.ID AS CanalID, " +
                    "   tSetor.Nome AS Setor, " +
                    "   tCanal.Nome AS Canal, " +
                    "   tLoja.ID AS LojaID, " +
                    "   tLoja.Nome AS Loja, " +
                    "   tPreco.Nome AS Preco, " +
                    "   tPreco.Valor AS PrecoValor, " +
                    "   COUNT(tIngresso.ID) AS Quantidade, " +
                    "   SUM(tPreco.valor) AS Valor " +
                    "FROM " +
                    "   tEvento (NOLOCK) " +
                    "INNER JOIN " +
                    "   tApresentacao (NOLOCK) ON tApresentacao.EventoID = tEvento.ID " +
                    "INNER JOIN " +
                    "   tIngresso (NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID " +
                    "INNER JOIN " +
                    "   tLoja (NOLOCK) ON tLoja.ID = tIngresso.LojaID " +
                    "INNER JOIN " +
                    "   tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID " +
                    "INNER JOIN " +
                    "   tSetor (NOLOCK) ON tSetor.ID = tIngresso.SetorID " +
                    "INNER JOIN " +
                    "   tPreco (NOLOCK) ON tPreco.ID = tIngresso.PrecoID " +
                    ((RegionalID > 0) ? "" +
                        "INNER JOIN " +
                        "  tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID " +
                        "INNER JOIN " +
                        "  tEmpresa (NOLOCK) ON tEmpresa.ID = tLocal.EmpresaID " +
                    "" : "") +
                    "WHERE " +
                    "  (tIngresso.Status = '" + Ingresso.PREIMPRESSO + "') " +
                    "AND " +
                    "   (tApresentacao.DisponivelRelatorio = 'T') " +
                    ((EmpresaID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.EmpresaID = " + EmpresaID + ") " : "") +
                    ((EventoID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.EventoID = " + EventoID + ") " : "") +
                    ((LocalID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.LocalID = " + LocalID + ") " : "") +
                    ((ApresentacaoID > 0) ? "" +
                    "AND " +
                    "  (tIngresso.ApresentacaoID = " + ApresentacaoID + ") " : "") +
                    ((SetorID > 0) ? "" +
                    "AND " +
                    "  (tSetor.ID = " + SetorID + ") " : "") +
                    ((PrecoID > 0) ? "" +
                    "AND " +
                    "  (tPreco.ID = " + PrecoID + ") " : "") +
                    ((RegionalID > 0) ? "" +
                    "AND " +
                    "  (tEmpresa.RegionalID = " + RegionalID + ") " : "") +
                    "GROUP BY " +
                    "   tEvento.ID, " +
                    "   tEvento.Nome, " +
                    "   tApresentacao.ID, " +
                    "   tApresentacao.Horario, " +
                    "   tSetor.Nome, " +
                    "   tCanal.ID, " +
                    "   tCanal.Nome, " +
                    "   tLoja.ID, " +
                    "   tLoja.Nome, " +
                    "   tPreco.Nome, " +
                    "   tPreco.Valor " +
                    "ORDER BY " +
                    "  tEvento.Nome, " +
                    "  tApresentacao.Horario, " +
                    "  tSetor.Nome, " +
                    "  tPreco.Nome, " +
                    "  tCanal.Nome, " +
                    "tLoja.Nome"))
                {
                    while (bd.Consulta().Read())
                    {
                        DataRow linhaTabela = tabela.NewRow();


                        linhaTabela["EventoID"] = bd.LerInt("EventoID");
                        linhaTabela["Evento"] = bd.LerString("Evento");
                        linhaTabela["ApresentacaoID"] = bd.LerInt("ApresentacaoID");
                        linhaTabela["Horario"] = bd.LerString("Horario");
                        linhaTabela["Setor"] = bd.LerString("Setor");
                        linhaTabela["Preco"] = bd.LerString("Preco");
                        linhaTabela["CanalID"] = bd.LerInt("CanalID");
                        linhaTabela["Canal"] = bd.LerString("Canal");
                        linhaTabela["LojaID"] = bd.LerInt("LojaID");
                        linhaTabela["Loja"] = bd.LerString("Loja");
                        linhaTabela["Quantidade"] = bd.LerDecimal("Quantidade");
                        linhaTabela["Valor"] = bd.LerDecimal("Valor");

                        tabela.Rows.Add(linhaTabela);
                    }
                }

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

        public DataTable CarregaEventosPreImpresso(int LocalID)
        {
            try
            {
                //Estrutura tabela

                DataTable tabela = new DataTable();

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));

                string sql = @"SELECT DISTINCT tEvento.id,tEvento.Nome 
								FROM tEvento                 
								INNER JOIN tIngresso ON tIngresso.EventoID = tEvento.ID
								INNER JOIN tIngressolog ON tIngresso.ID = tIngressolog.IngressoID        
								WHERE tIngressolog.acao in ('P','T','K') and tIngresso.LocalID = " + LocalID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linhaTabela = tabela.NewRow();
                    linhaTabela["ID"] = bd.LerInt("ID");
                    linhaTabela["Nome"] = bd.LerString("Nome");

                    tabela.Rows.Add(linhaTabela);
                }

                bd.Consulta().Close();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable CarregaTabelaPreImpressoEvento(int EmpresaID, int RegionalID, int EventoID, int LocaID, int ApresentacaoID, int SetorID, int PrecoID)
        {
            //variaveis
            object valorAuxiliar = 0;
            decimal qtdIngresso = 0;
            decimal valor = 0;
            decimal qtdTotalIngresso = 0;
            decimal valorTotal = 0;
            string Canal = string.Empty;
            string Loja = string.Empty;
            string Evento = string.Empty;
            string Apresentacao = string.Empty;
            string Setor = string.Empty;
            string Preco = string.Empty;
            DataRow linhaFinal = null;

            DataTable tabela = getDataPreImpressoEvento(EmpresaID, RegionalID, EventoID, LocaID, ApresentacaoID, SetorID, PrecoID);

            //tabela que carrega o Grid
            DataTable tabelaFinal = Utilitario.EstruturaPreImpressoEvento();

            DataTable dtEvento = CTLib.TabelaMemoria.Distinct(tabela, "Evento");
            foreach (DataRow linhaEvento in dtEvento.Rows)
            {
                //insere nome do Evento
                linhaFinal = tabelaFinal.NewRow();

                tabelaFinal.Rows.Add(linhaFinal);
                linhaFinal["Evento"] = "<div style='text-align:left;font-weight:bold'>Evento " + linhaEvento["Evento"] + "</div>";
                Evento = linhaEvento["Evento"].ToString();


                DataTable dtApresentacao = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Horario", "Evento = '" + Evento + "'");
                foreach (DataRow linhaApresentacao in dtApresentacao.Rows)
                {
                    //insere nome da Apresentação
                    linhaFinal = tabelaFinal.NewRow();
                    tabelaFinal.Rows.Add(linhaFinal);
                    Apresentacao = Utilitario.FormatoSemanaDataHora(linhaApresentacao["Horario"].ToString());
                    linhaFinal["Evento"] = "<div style='text-align:left;margin-left:40px'>" + Apresentacao + "</div>";


                    DataTable dtSetor = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Setor", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "'");
                    foreach (DataRow linhaSetor in dtSetor.Rows)
                    {
                        //insere nome do Setor
                        linhaFinal = tabelaFinal.NewRow();

                        tabelaFinal.Rows.Add(linhaFinal);
                        linhaFinal["Evento"] = "<div style='text-align:left;margin-left:75px'>" + linhaSetor["Setor"] + "</div>";
                        Setor = linhaSetor["Setor"].ToString();


                        DataTable dtPreco = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Preco", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "'");
                        foreach (DataRow linhaPreco in dtPreco.Rows)
                        {
                            //insere nome do Preço
                            linhaFinal = tabelaFinal.NewRow();

                            tabelaFinal.Rows.Add(linhaFinal);
                            linhaFinal["Evento"] = "<div style='text-align:left;margin-left:105px'>" + linhaPreco["Preco"] + "</div>";
                            Preco = linhaPreco["Preco"].ToString();


                            DataTable dtCanal = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Canal", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "'");
                            foreach (DataRow linhaCanal in dtCanal.Rows)
                            {
                                //insere nome do Canal
                                linhaFinal = tabelaFinal.NewRow();

                                tabelaFinal.Rows.Add(linhaFinal);
                                linhaFinal["Canal"] = "<div style='text-align:left';>" + linhaCanal["Canal"].ToString() + "</div>";
                                Canal = linhaCanal["Canal"].ToString();

                                DataTable dtLoja = CTLib.TabelaMemoria.DistinctComFiltro(tabela, "Loja", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "' and Canal = '" + Canal + "'");
                                foreach (DataRow linhaLoja in dtLoja.Rows)
                                {
                                    //insere nome da Loja
                                    linhaFinal = tabelaFinal.NewRow();

                                    tabelaFinal.Rows.Add(linhaFinal);
                                    linhaFinal["Loja"] = "<div style='text-align:left;'>" + linhaLoja["Loja"].ToString() + "</div>";
                                    Loja = linhaLoja["Loja"].ToString();

                                    //qtde total para a loja 
                                    valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "' and Canal = '" + Canal + "' and Loja = '" + Loja + "'");
                                    qtdIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                                    //valor total para linha de loja
                                    valorAuxiliar = tabela.Compute("SUM(Valor)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "' and Canal = '" + Canal + "' and Loja = '" + Loja + "'");
                                    valor = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;


                                    linhaFinal["Quantidade"] = qtdIngresso;
                                    linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valor) + "</div>";
                                }


                            }
                            //total Preço
                            linhaFinal = tabelaFinal.NewRow();
                            linhaFinal["Evento"] = "<div style='text-align:left;margin-left:105px'>Total " + Preco + "</div>";
                            //qtde total 
                            valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "'");
                            qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                            //valor total
                            valorAuxiliar = tabela.Compute("SUM(Valor)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "' and Preco = '" + Preco + "'");
                            valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                            linhaFinal["Quantidade"] = qtdTotalIngresso;
                            linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                            tabelaFinal.Rows.Add(linhaFinal);


                        }
                        //total Setor
                        linhaFinal = tabelaFinal.NewRow();
                        linhaFinal["Evento"] = "<div style='text-align:left;margin-left:75px'>Total " + Setor + "</div>";
                        //qtde total 
                        valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "'");
                        qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        //valor total
                        valorAuxiliar = tabela.Compute("SUM(Valor)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "' and Setor = '" + Setor + "'");
                        valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                        linhaFinal["Quantidade"] = qtdTotalIngresso;
                        linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                        tabelaFinal.Rows.Add(linhaFinal);

                    }
                    //total Apresentação
                    linhaFinal = tabelaFinal.NewRow();
                    linhaFinal["Evento"] = "<div style='text-align:left;margin-left:40px'>Total " + Apresentacao + "</div>";
                    //qtde total 
                    valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Evento = '" + Evento + "' and Horario = '" + linhaApresentacao["Horario"] + "'");
                    qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                    //valor total
                    valorAuxiliar = tabela.Compute("SUM(Valor)", "Canal= '" + Canal + "' and Loja= '" + Loja + "'");
                    valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                    linhaFinal["Quantidade"] = qtdTotalIngresso;
                    linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                    tabelaFinal.Rows.Add(linhaFinal);


                }
                //total Evento
                linhaFinal = tabelaFinal.NewRow();
                linhaFinal["Evento"] = "<div style='text-align:left;font-weight:bold'>Total Evento " + Evento + "</div>";
                //qtde total 
                valorAuxiliar = tabela.Compute("SUM(Quantidade)", "Evento = '" + Evento + "'");
                qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                //valor total
                valorAuxiliar = tabela.Compute("SUM(Valor)", "Evento = '" + Evento + "'");
                valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

                linhaFinal["Quantidade"] = qtdTotalIngresso;
                linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
                tabelaFinal.Rows.Add(linhaFinal);


            }

            //linha em branco
            linhaFinal = tabelaFinal.NewRow();
            tabelaFinal.Rows.Add(linhaFinal);

            //Total Geral
            linhaFinal = tabelaFinal.NewRow();
            linhaFinal["Evento"] = "<div style='text-align:left;'>Total</div>";
            //qtde total 
            valorAuxiliar = tabela.Compute("SUM(Quantidade)", "1=1");
            qtdTotalIngresso = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

            //valor total
            valorAuxiliar = tabela.Compute("SUM(Valor)", "1=1");
            valorTotal = valorAuxiliar == System.DBNull.Value ? 0 : (decimal)valorAuxiliar;

            linhaFinal["Quantidade"] = qtdTotalIngresso;
            linhaFinal["Valor"] = "<div style='text-align:right;'>" + Utilitario.AplicaFormatoMoeda(valorTotal) + "</div>";
            tabelaFinal.Rows.Add(linhaFinal);





            return tabelaFinal;

        }

        #endregion

        public override object InitializeLifetimeService()
        {
            ILease l = (ILease)base.InitializeLifetimeService();
            l.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            l.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            l.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return l;
        }
    }

    [Serializable]
    public class PreImpressoGerenciadorException : Exception
    {
        public PreImpressoGerenciadorException() : base() { }

        public PreImpressoGerenciadorException(string msg) : base(msg) { }

        public PreImpressoGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
