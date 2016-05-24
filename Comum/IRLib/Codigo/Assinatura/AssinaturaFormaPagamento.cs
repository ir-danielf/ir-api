/**************************************************
* Arquivo: AssinaturaFormaPagamento.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib
{

    public class AssinaturaFormaPagamento : AssinaturaFormaPagamento_B
    {

        public AssinaturaFormaPagamento() { }

        public AssinaturaFormaPagamento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void Gerenciar(BD bd, List<EstruturaAssinaturaFormaPagamento> lstFormasPagamentoDisponiveis, int AssinaturaID)
        {
            foreach (EstruturaAssinaturaFormaPagamento item in lstFormasPagamentoDisponiveis)
            {
                switch (item.Acao)
                {
                    case Enumerators.TipoAcaoCanal.Associar:
                        this.FormaPagamentoID.Valor = item.FormaPagamentoID;
                        this.AssinaturaID.Valor = AssinaturaID;
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

        public List<Assinaturas.Models.FormaPagamento> BuscarFormasPagamento(int clienteID, bool forcarParcela, bool permiteTroca)
        {
            try
            {
                int forcar = Convert.ToInt32(forcarParcela);
                int troca = Convert.ToInt32(permiteTroca);

                bd.Consulta(string.Format(@"exec sp_Assinaturas_GetFormasPagamento {0}, {1}, {2}, {3}, {4}", forcar.ToString(), clienteID, FormaPagamento.DINHEIRO, FormaPagamento.TIPO_CARTAO_DEBITO, troca.ToString()));

                if (!bd.Consulta().Read())
                    throw new Exception("Não existem formas de pagamento disponíveis para as assinaturas selecionadas.");

                List<Assinaturas.Models.FormaPagamento> lista = new List<Assinaturas.Models.FormaPagamento>();

                do
                {
                    lista.Add(new Assinaturas.Models.FormaPagamento()
                    {
                        Nome = bd.LerString("Nome").Replace(".", string.Empty).Replace("!", string.Empty).ToLower(),
                        Parcelas = bd.LerInt("Parcelas"),
                        ForcaParcela = forcar
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaFormaPagamentoLista : AssinaturaFormaPagamentoLista_B
    {

        public AssinaturaFormaPagamentoLista() { }

        public AssinaturaFormaPagamentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
