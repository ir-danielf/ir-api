/**************************************************
* Arquivo: TaxaEntrega.cs
* Gerado: 19/09/2005
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib
{

    public class TaxaEntrega : TaxaEntrega_B
    {

        public TaxaEntrega() { }

        public TaxaEntrega(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter todas as taxas de entrega
        /// </summary>
        /// <returns></returns>
        public DataTable Todas()
        {

            try
            {

                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                string sql = "SELECT tTaxaEntrega.ID,tTaxaEntrega.Nome,tTaxaEntrega.Valor,tRegiao.Nome AS Regiao " +
                    "FROM tTaxaEntrega (NOLOCK) " +
                    "INNER JOIN tRegiao (NOLOCK) ON tRegiao.ID=tTaxaEntrega.RegiaoID " +
                    "WHERE tTaxaEntrega.Disponivel='T' " +
                    "ORDER BY tRegiao.Nome,tTaxaEntrega.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Regiao") + ", " + bd.LerString("Nome");
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
        /// Obter todas as taxas de entrega por Regional
        /// </summary>
        /// <returns></returns>
        public DataTable Todas(int RegionalID)
        {
            try
            {
                string sql = "SELECT Estados FROM tRegional (NOLOCK) WHERE ID = " + RegionalID;
                
                bd.Consulta(sql);

                bd.Consulta().Read();
                string Estado = bd.LerString("Estados");
                string[] Estados = Estado.Split(';');
                
                DataTable tabela = new DataTable("TaxaEntrega");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));

                for (int cont = 0; cont < Estados.Length; cont++)
                {
                    sql = "SELECT tTaxaEntrega.ID,tTaxaEntrega.Nome,tTaxaEntrega.Valor,tRegiao.Nome AS Regiao " +
                        "FROM tTaxaEntrega (NOLOCK) " +
                        "INNER JOIN tRegiao (NOLOCK) ON tRegiao.ID=tTaxaEntrega.RegiaoID " +
                        "WHERE tTaxaEntrega.Disponivel='T' " +
                        "AND Estado = '" + Estados[cont] +
                        "' ORDER BY tRegiao.Nome,tTaxaEntrega.Nome";

                    bd.Consulta(sql);

                    while (bd.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = bd.LerInt("ID");
                        linha["Nome"] = bd.LerString("Regiao") + ", " + bd.LerString("Nome");
                        linha["Valor"] = bd.LerDecimal("Valor");
                        tabela.Rows.Add(linha);
                    }
                }
                bd.Fechar();

                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ProcedimentoEntregaTaxa(int taxaEntregaID)
        {
            string strProcedimentoEntregaTaxa = "";

            try
            {

                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "  tTaxaEntrega.ProcedimentoEntrega " +
                    "FROM " +
                    "  tTaxaEntrega (NOLOCK)  " +
                    "WHERE " +
                    "  tTaxaEntrega.ID = " + taxaEntregaID))
                {

                    if (oDataReader.Read())
                    {
                        strProcedimentoEntregaTaxa = bd.LerString("ProcedimentoEntrega");
                    }
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strProcedimentoEntregaTaxa;
        }

        public List<EstruturaTaxaEntrega> CarregarListaFiltrada(int RegiaoID, string Disponivel, string Padrao)
        {
            try
            {
                StringBuilder filter = new StringBuilder();

                switch (Disponivel)
                {
                    case "Sim":
                        filter.Append(" Disponivel = 'T' ");
                        break;
                    case "Não":
                        filter.Append(" Disponivel = 'F' ");
                        break;
                    default:
                        //ignora (Ambos)
                        break;
                }

                if (RegiaoID > 0)
                {
                    if (filter.Length > 0)
                        filter.Append("AND");

                    filter.AppendFormat(" RegiaoID = {0} ", RegiaoID);
                }

                switch (Padrao)
                {
                    case "Sim":
                        if (filter.Length > 0)
                            filter.Append("AND");
                        filter.Append(" te.Padrao = 'T' ");
                        break;
                    case "Não":
                        if (filter.Length > 0)
                            filter.Append("AND");
                        filter.Append(" te.Padrao = 'F' ");
                        break;
                }

                string sql = string.Empty;
                List<EstruturaTaxaEntrega> lista = new List<EstruturaTaxaEntrega>();

                sql = string.Format(@"SELECT te.ID, te.Nome, te.Valor, te.RegiaoID, r.Nome AS RegiaoNome, te.Prazo, te.Disponivel, te.Obs, te.Estado, te.ProcedimentoEntrega,
                        te.DiasTriagem, te.Padrao, te.EnviaAlerta, te.PermitirImpressaoInternet
                        FROM tTaxaEntrega te (NOLOCK)
                        INNER JOIN tRegiao r (NOLOCK) ON r.ID = te.RegiaoID 
                        {0}
                        ORDER BY te.Nome ",
                            filter.Length > 0 ? "WHERE " + filter.ToString() : string.Empty);


                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaTaxaEntrega()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                        RegiaoID = bd.LerInt("RegiaoID"),
                        RegiaoNome = bd.LerString("RegiaoNome"),
                        Prazo = bd.LerInt("Prazo"),
                        Disponivel = bd.LerBoolean("Disponivel"),
                        Obs = bd.LerString("Obs"),
                        Estado = bd.LerString("Estado"),
                        ProcedimentoEntrega = bd.LerString("ProcedimentoEntrega"),
                        DiasTriagem = bd.LerInt("DiasTriagem"),
                        Padrao = bd.LerBoolean("Padrao"),
                        EnviaAlerta = bd.LerBoolean("EnviaAlerta"),
                        PermitirImpressaoInternet = bd.LerBoolean("PermitirImpressaoInternet"),
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AtribuirEstrutura(EstruturaTaxaEntrega estruturaTaxaEntrega)
        {
            this.Nome.Valor = estruturaTaxaEntrega.Nome;
            this.Valor.Valor = estruturaTaxaEntrega.Valor;
            this.RegiaoID.Valor = estruturaTaxaEntrega.RegiaoID;
            this.Prazo.Valor = estruturaTaxaEntrega.Prazo;
            this.Disponivel.Valor = estruturaTaxaEntrega.Disponivel;
            this.Obs.Valor = estruturaTaxaEntrega.Obs;
            this.Estado.Valor = estruturaTaxaEntrega.Estado;
            this.ProcedimentoEntrega.Valor = estruturaTaxaEntrega.ProcedimentoEntrega;
            this.DiasTriagem.Valor = estruturaTaxaEntrega.DiasTriagem;
            this.Padrao.Valor = estruturaTaxaEntrega.Padrao;
            this.EnviaAlerta.Valor = estruturaTaxaEntrega.EnviaAlerta;
            this.PermitirImpressaoInternet.Valor = estruturaTaxaEntrega.PermitirImpressaoInternet;
        }

        public void Inserir(EstruturaTaxaEntrega estruturaTaxaEntrega)
        {
            this.AtribuirEstrutura(estruturaTaxaEntrega);
            this.Inserir();
        }

        public void Atualizar(EstruturaTaxaEntrega estruturaTaxaEntrega)
        {
            this.AtribuirEstrutura(estruturaTaxaEntrega);
            this.Control.ID = estruturaTaxaEntrega.ID;
            this.Atualizar();
        }


        public List<int> GetTaxaEntregaPadrao()
        {
            try
            {
                List<int> ids = new List<int>();
                bd.Consulta("SELECT ID FROM tTaxaEntrega (NOLOCK) WHERE Padrao = 'T'");
                while (bd.Consulta().Read())
                    ids.Add(bd.LerInt("ID"));

                return ids;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class TaxaEntregaLista : TaxaEntregaLista_B
    {

        public TaxaEntregaLista() { }

        public TaxaEntregaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioTaxaEntrega");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Regiao", typeof(string));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Valor", typeof(decimal));
                    tabela.Columns.Add("Prazo", typeof(string));
                    tabela.Columns.Add("Disponivel", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        Regiao regiao = new Regiao();
                        regiao.Ler(taxaEntrega.RegiaoID.Valor);
                        linha["Regiao"] = regiao.Nome.Valor;
                        linha["Nome"] = taxaEntrega.Nome.Valor;
                        linha["Valor"] = taxaEntrega.Valor.Valor;
                        linha["Prazo"] = "    " + taxaEntrega.Prazo.Valor;
                        linha["Disponivel"] = (taxaEntrega.Disponivel.Valor) ? "Sim" : "Não";
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

}
