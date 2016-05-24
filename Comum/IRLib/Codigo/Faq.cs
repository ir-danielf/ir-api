/**************************************************
* Arquivo: Faq.cs
* Gerado: 25/02/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Data;

namespace IRLib
{

    public class Faq : Faq_B
    {
        public const string DATASET_FAQ = "ListaFaq";
        public const string FAQ_TIPO = "FaqTipo";
        public const string FAQ = "Faq";
        public const string ID = "ID";
        public const string TIPO = "Tipo";
        public const string EDITAVEL = "Editavel";
        public const string FAQ_TIPO_ID = "FaqTipoID";
        public const string PERGUNTA = "Pergunta";
        public const string RESPOSTA = "Resposta";
        public const string TAGS = "Tags";
        public const string CANCELADO = "Cancelado";
        public const string CLASSIFICACAO = "Classificacao";

        public Faq() { }

        public Faq(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public static DataSet EstruturaBusca()
        {

            DataSet ds = new DataSet(DATASET_FAQ);

            DataTable tFaqTipo = new DataTable(FAQ_TIPO);
            DataTable tFaq = new DataTable(FAQ);

            tFaqTipo.Columns.Add(ID, typeof(int)).DefaultValue = 0;
            tFaqTipo.Columns.Add(TIPO, typeof(string)).DefaultValue = "-";
            tFaqTipo.Columns.Add(EDITAVEL, typeof(bool)).DefaultValue = false;

            tFaq.Columns.Add(ID, typeof(int));
            tFaq.Columns.Add(FAQ_TIPO_ID, typeof(int)).DefaultValue = 0;
            tFaq.Columns.Add(PERGUNTA, typeof(string)).DefaultValue = "-";
            tFaq.Columns.Add(EDITAVEL, typeof(bool)).DefaultValue = true;

            ds.Tables.Add(tFaqTipo);

            ds.Tables.Add(tFaq);


            //Grid com a InfoPacote
            DataColumn colFaq = tFaq.Columns[FAQ_TIPO_ID];
            DataColumn colFaqTipo = tFaqTipo.Columns[ID];
            DataRelation dr2 = new DataRelation("FaqTipoXFaq", colFaqTipo, colFaq, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colFaqTipo, colFaq);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tFaq.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            return ds;

        }

        public static DataSet EstruturaBuscaAux()
        {
            DataSet ds = new DataSet(DATASET_FAQ);
            DataTable tFaqTipo = new DataTable(FAQ_TIPO);
            DataTable tFaq = new DataTable(FAQ);

            tFaqTipo.Columns.Add(ID, typeof(int)).DefaultValue = 0;
            tFaqTipo.Columns.Add(TIPO, typeof(string)).DefaultValue = "-";
            tFaqTipo.Columns.Add(EDITAVEL, typeof(bool)).DefaultValue = false;

            tFaq.Columns.Add(ID, typeof(int));
            tFaq.Columns.Add(FAQ_TIPO_ID, typeof(int)).DefaultValue = 0;
            tFaq.Columns.Add(PERGUNTA, typeof(string)).DefaultValue = "-";
            tFaq.Columns.Add(TAGS, typeof(string)).DefaultValue = "-";
            tFaq.Columns.Add(RESPOSTA, typeof(string)).DefaultValue = "-";
            tFaq.Columns.Add(EDITAVEL, typeof(bool)).DefaultValue = true;
            tFaq.Columns.Add(CLASSIFICACAO, typeof(int)).DefaultValue = 0;

            ds.Tables.Add(tFaqTipo);

            ds.Tables.Add(tFaq);

            //Grid com a InfoPacote
            DataColumn colFaq = tFaq.Columns[FAQ_TIPO_ID];
            DataColumn colFaqTipo = tFaqTipo.Columns[ID];
            DataRelation dr2 = new DataRelation("FaqTipoXFaq", colFaqTipo, colFaq, true);

            ForeignKeyConstraint idKeyRestraint2 = new ForeignKeyConstraint(colFaqTipo, colFaq);
            idKeyRestraint2.DeleteRule = Rule.Cascade;
            tFaq.Constraints.Add(idKeyRestraint2);

            ds.EnforceConstraints = true;

            ds.Relations.Add(dr2);

            return ds;
        }

        public DataSet Buscar(int TipoFaqID)
        {
            try
            {
                DataSet retorno = EstruturaBusca();
                string sql = "";
                string TipoFaq = "";

                if (TipoFaqID > 0)
                    TipoFaq = " WHERE ID = " + TipoFaqID;

                sql = "SELECT ID,NOME FROM tFaqTipo ";

                bd.Consulta((TipoFaqID > 0) ? sql += TipoFaq : sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.Tables[FAQ_TIPO].NewRow();
                    linha[ID] = bd.LerInt("ID");
                    linha[TIPO] = bd.LerString("Nome");
                    retorno.Tables[FAQ_TIPO].Rows.Add(linha);
                }

                sql = "SELECT ID,FaqTipoID,Pergunta FROM tFaq ";

                if (TipoFaqID > 0)
                    TipoFaq = " WHERE FaqTipoID = " + TipoFaqID;

                bd.Consulta((TipoFaqID > 0) ? sql += TipoFaq : sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = retorno.Tables[FAQ].NewRow();
                    linha[ID] = bd.LerInt("ID");
                    linha[FAQ_TIPO_ID] = bd.LerInt("FaqTipoID");
                    linha[PERGUNTA] = bd.LerString("Pergunta");
                    retorno.Tables[FAQ].Rows.Add(linha);
                }

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet BuscarAvancada(int TipoFaqID, string busca)
        {
            try
            {
                DataSet retorno = EstruturaBusca();
                DataSet aux = EstruturaBuscaAux();
                string sql = "";
                string TipoFaq = "";

                if (TipoFaqID > 0)
                    TipoFaq = " WHERE ID = " + TipoFaqID;

                sql = "SELECT ID,NOME FROM tFaqTipo ";

                bd.Consulta((TipoFaqID > 0) ? sql += TipoFaq : sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = aux.Tables[FAQ_TIPO].NewRow();
                    linha[ID] = bd.LerInt("ID");
                    linha[TIPO] = bd.LerString("Nome");
                    aux.Tables[FAQ_TIPO].Rows.Add(linha);
                }

                sql = "SELECT ID,FaqTipoID,Pergunta,Tags,Resposta FROM tFaq ";

                string filtro = "";
                if (TipoFaqID > 0)
                    filtro = " WHERE FaqTipoID = " + TipoFaqID + "AND (Exibicao = 'S' OR Exibicao = 'A')";
                else
                    filtro = " WHERE (Exibicao = 'S' OR Exibicao = 'A')";

                if (busca.Length > 0)
                {
                    if (filtro.Length <= 0)
                        filtro = " WHERE  (Tags like '%" + busca + "%' OR Pergunta like '%" + busca + "%' OR Resposta like '%" + busca + "%') AND (Exibicao = 'S' OR Exibicao = 'A')";
                    else
                        filtro += " AND ( Tags like '%" + busca + "%' OR Pergunta like '%" + busca + "%' OR Resposta like '%" + busca + "%') ";
                }
                bd.Consulta((filtro.Length > 0) ? sql += filtro : sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = aux.Tables[FAQ].NewRow();
                    linha[ID] = bd.LerInt("ID");
                    linha[FAQ_TIPO_ID] = bd.LerInt("FaqTipoID");
                    linha[PERGUNTA] = bd.LerString("Pergunta");
                    linha[RESPOSTA] = bd.LerString("Resposta");
                    linha[TAGS] = bd.LerString("Tags");
                    aux.Tables[FAQ].Rows.Add(linha);
                }

                foreach (DataRow linha in aux.Tables[FAQ].Select("Resposta like '%" + busca + "%'"))
                {
                    linha[CLASSIFICACAO] = 3;
                }

                foreach (DataRow linha in aux.Tables[FAQ].Select("Pergunta like '%" + busca + "%'"))
                {
                    linha[CLASSIFICACAO] = 2;
                }

                foreach (DataRow linha in aux.Tables[FAQ].Select("Tags like '%" + busca + "%'"))
                {
                    linha[CLASSIFICACAO] = 1;
                }

                foreach (DataRow linha in aux.Tables[FAQ_TIPO].Rows)
                {
                    DataRow[] drAux = aux.Tables[FAQ].Select(FAQ_TIPO_ID + "=" + linha[ID]);
                    if (drAux.Length > 0)
                    {
                        DataRow linhaRetorno = retorno.Tables[FAQ_TIPO].NewRow();
                        linhaRetorno[ID] = linha[ID];
                        linhaRetorno[TIPO] = linha[TIPO];
                        retorno.Tables[FAQ_TIPO].Rows.Add(linhaRetorno);
                    }
                }

                foreach (DataRow linha in aux.Tables[FAQ].Select("1=1",CLASSIFICACAO))
                {
                    DataRow linhaRetorno = retorno.Tables[FAQ].NewRow();
                    linhaRetorno[ID] = linha[ID];
                    linhaRetorno[FAQ_TIPO_ID] = linha[FAQ_TIPO_ID];
                    linhaRetorno[PERGUNTA] = linha[PERGUNTA];
                    retorno.Tables[FAQ].Rows.Add(linhaRetorno);
                }

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class FaqLista : FaqLista_B
    {
        public FaqLista() { }

        public FaqLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }

}
