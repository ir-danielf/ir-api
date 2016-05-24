/**************************************************
* Arquivo: Despesas.cs
* Gerado: 24/09/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class Despesas : Despesas_B
    {

        public Despesas() { }

        public Despesas(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaDespesas> buscaDespesas(int apresentacaoID, int eventoID, int localID)
        {
            try
            {
                List<EstruturaDespesas> lista = new List<EstruturaDespesas>();

                string filtro = "";

                if (apresentacaoID > 0)
                    filtro = " ApresentacaoID = " + apresentacaoID;
                else if (eventoID > 0)
                    filtro = " EventoID = " + eventoID;
                else
                    filtro = " LocalID = " + localID;

                string sql = string.Format(@"Select * From tDespesas where  {0} order by Nome", filtro);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    lista.Add(new EstruturaDespesas
                    {

                        ID = bd.LerInt("ID"),
                        LocalID = bd.LerInt("LocalID"),
                        EventoID = bd.LerInt("EventoID"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Nome = bd.LerString("Nome"),
                        PorPorcentagem = bd.LerBoolean("PorPorcentagem"),
                        Valor = bd.LerDecimal("Valor"),
                        ValorMinimo = bd.LerDecimal("ValorMinimo"),
                        Obs = bd.LerString("Obs"),
                        ValorLiquido = bd.LerBoolean("ValorLiquido"),
                        TipoFormaPagamento = bd.LerBoolean("TipoFormaPagamento"),
                        TipoPagamentoID = bd.LerInt("TipoPagamentoID")

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

        public void AdicionarDespesasLocal(int eventoID, int localID)
        {
            try
            {
                List<EstruturaDespesas> lista = new List<EstruturaDespesas>();
                lista = buscaDespesas(0, 0, localID);

                foreach (EstruturaDespesas item in lista)
                {
                    item.EventoID = eventoID;
                    item.LocalID = 0;
                    item.ID = 0;

                    this.AtribuirEstrutura(item);
                    this.Inserir();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void AtribuirEstrutura(EstruturaDespesas item)
        {
            this.Control.ID = item.ID;
            this.Valor.Valor = item.Valor;
            this.ValorMinimo.Valor = item.ValorMinimo;
            this.Nome.Valor = item.Nome;
            this.TipoPagamentoID.Valor = item.TipoPagamentoID;
            this.TipoFormaPagamento.Valor = item.TipoFormaPagamento;
            this.ApresentacaoID.Valor = item.ApresentacaoID;
            this.EventoID.Valor = item.EventoID;
            this.LocalID.Valor = item.LocalID;
            this.ValorLiquido.Valor = item.ValorLiquido;
            this.PorPorcentagem.Valor = item.PorPorcentagem;
            this.Obs.Valor = item.Obs;
        }
    }

    public class DespesasLista : DespesasLista_B
    {

        public DespesasLista() { }

        public DespesasLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
