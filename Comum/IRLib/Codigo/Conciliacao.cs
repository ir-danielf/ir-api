/**************************************************
* Arquivo: Conciliacao.cs
* Gerado: 20/12/2012
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;

namespace IRLib
{

    public class Conciliacao : Conciliacao_B
    {

        public Conciliacao() { }

        public string Conteudo { get; set; }
        public int CaixaID { get; set; }

        public void Gerar()
        {
            BD bd = new BD();

            try
            {

                int caixaID = this.GetCaixaProcesso();
                string nome = "";
                string conteudo = "";

                string sql = @" EXEC Gerar_Conciliacao " + caixaID;

                this.CaixaID = caixaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    nome = "ir-vendas-" + bd.LerString("DataHoje");
                }

                bd.Consulta().NextResult();

                int qtdLinhas = 0;

                while (bd.Consulta().Read())
                {
                    qtdLinhas++;
                    conteudo += bd.LerString("Linha") + "\r\n";
                }

                if (qtdLinhas > 1)
                {
                    this.Conteudo = conteudo;

                    this.Nome.Valor = nome;

                    this.Inserir(bd);

                    string id = String.Format("{0:00000000000}", this.Control.ID);
                    this.Nome.Valor = this.Nome.Valor + id;

                    this.Atualizar(bd);
                    this.TimeStampCriacao.Valor = DateTime.Now;
                    this.Atualizar(bd);
                    this.AtualizaCaixa(this.CaixaID, this.Control.ID);
                }
                else
                {
                    this.Conteudo = "";
                    this.AtualizaCaixa(this.CaixaID, 0);
                }


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

        public void AtualizaCaixa(int caixaID, int conciliacaoID)
        {
            Caixa caixa = new Caixa();
            caixa.Ler(caixaID);
            caixa.ConciliacaoID.Valor = conciliacaoID;
            caixa.Atualizar();
        }

        private int GetCaixaProcesso()
        {
            try
            {

                string sql = string.Format(@"
                                        declare @caixaID int
                                        select @caixaID = MIN(c.ID) 
                                        from tCaixa c
                                        left join tVendaBilheteria v On c.ID = v.CaixaID
                                        where ConciliacaoID is null AND DataAbertura >= 20130101000000  And v.ID  is not null

                                        update tCaixa set ConciliacaoID = 0 where ID = @caixaID

                                        select @caixaID as ID");

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Erro ao Buscar Caixa");
                return bd.LerInt("ID");
            }
            finally
            {
                bd.Fechar();
            }


        }

        public string GetArquivoUpload()
        {
            string sql =
                     string.Format(@"select MIN(ID) as ID,Nome from tConciliacao where TimeStampEnvio is null ");
            bd.Consulta(sql);
            if (!bd.Consulta().Read())
                throw new Exception("Erro ao Buscar Caixa");

            int id = bd.LerInt("ID");
            string nome = bd.LerString("Nome");

            this.Ler(id);

            return nome;
        }

        public void AtualizarEnvio()
        {
            this.TimeStampEnvio.Valor = DateTime.Now;

            this.Atualizar();
        }

        public void PopularTabelas(string dtFiltro)
        {
            string sql = string.Format("PR_POPULAR_TABELAS_CONCILIACAO '" + dtFiltro + "'");

            bd.Executar(sql);
        }

        public void LimparDadosTabelaInflexion()
        {
            string sql = @"DELETE FROM LOJA_VENDA";
            bd.Executar(sql);

            sql = @"DELETE FROM LOJA_VENDA_PARCELAS";
            bd.Executar(sql);

        }
    }

    public class ConciliacaoLista : ConciliacaoLista_B
    {

        public ConciliacaoLista() { }


    }

}
