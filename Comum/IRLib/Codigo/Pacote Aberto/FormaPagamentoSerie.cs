/**************************************************
* Arquivo: FormaPagamentoSerie.cs
* Gerado: 02/02/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib
{

    public class FormaPagamentoSerie : FormaPagamentoSerie_B
    {

        public FormaPagamentoSerie() { }

        public FormaPagamentoSerie(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void GerenciarFormasDePagamento(BD bd, int SerieID, List<EstruturaFormaPagamentoSerie> lstFormasDePagamento)
        {
            foreach (EstruturaFormaPagamentoSerie item in lstFormasDePagamento)
            {
                switch (item.Acao)
                {
                    case Enumerators.TipoAcaoCanal.Associar:
                        this.FormaPagamentoID.Valor = item.FormaPagamentoID;
                        this.SerieID.Valor = SerieID;
                        this.Inserir(bd);
                        break;
                    case Enumerators.TipoAcaoCanal.Remover:
                        if (item.ID == 0)
                            continue;

                        this.Excluir(bd, item.ID);
                        break;
                    case Enumerators.TipoAcaoCanal.Manter:
                        break;
                }
            }
        }

        /// <summary>
        /// Exclui FormaPagamentoSerie com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int id)
        {


            this.Control.ID = id;

            string sqlSelect = "SELECT MAX(Versao) FROM cFormaPagamentoSerie WHERE ID=" + this.Control.ID;
            object obj = bd.ConsultaValor(sqlSelect);
            int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
            this.Control.Versao = versao;

            InserirControle("D");
            InserirLog();

            string sqlDelete = "DELETE FROM tFormaPagamentoSerie WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = (x == 1);
            return result;



        }

        /// <summary>
        /// Inserir novo(a) FormaPagamentoSerie
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MAX(ID) AS ID FROM cFormaPagamentoSerie");
            object obj = bd.ConsultaValor(sql);
            int id = (obj != null) ? Convert.ToInt32(obj) : 0;

            this.Control.ID = ++id;
            this.Control.Versao = 0;

            sql = new StringBuilder();
            sql.Append("INSERT INTO tFormaPagamentoSerie(ID, SerieID, FormaPagamentoID) ");
            sql.Append("VALUES (@ID,@001,@002)");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.SerieID.ValorBD);
            sql.Replace("@002", this.FormaPagamentoID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = (x == 1);

            if (result)
                InserirControle("I");

            return result;
        }
    }

    public class FormaPagamentoSerieLista : FormaPagamentoSerieLista_B
    {

        public FormaPagamentoSerieLista() { }

        public FormaPagamentoSerieLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
