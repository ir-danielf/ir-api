/**************************************************
* Arquivo: SerieItem.cs
* Gerado: 10/01/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects.Serie;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib
{

    public class SerieItem : SerieItem_B
    {

        public SerieItem() { }

        public SerieItem(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public void GerenciarItems(BD bd, int SerieID, List<EstruturaSerieItem> lstSerieItem)
        {
            Preco preco = new Preco(this.Control.UsuarioID);
            ApresentacaoSetor aps = new ApresentacaoSetor();

            int apresentacaoSetorID = 0;

            foreach (EstruturaSerieItem item in lstSerieItem)
            {
                this.Control.ID = 0;
                this.EventoID.Valor = item.EventoID;
                this.ApresentacaoID.Valor = item.ApresentacaoID;
                this.SetorID.Valor = item.SetorID;
                this.Promocional.Valor = item.Promocional;
                this.QuantidadePorPromocional.Valor = item.QuantidadePorPromocional;
                this.SerieID.Valor = SerieID;
                this.PrecoID.Valor = 0;

                switch (item.Acao)
                {
                    //Criar o preço antes e depois criar o serieitem
                    case Enumerators.TipoAcaoPreco.AssociarECriar:

                        apresentacaoSetorID = lstSerieItem
                               .Where(c => c.ApresentacaoID == item.ApresentacaoID && c.SetorID == item.SetorID && c.ApresentacaoSetorID != 0)
                               .Select(c => c.ApresentacaoSetorID).FirstOrDefault();

                        if (apresentacaoSetorID == 0)
                            apresentacaoSetorID = aps.ApresentacaoSetorID(item.ApresentacaoID, item.SetorID);


                        preco.ApresentacaoSetorID.Valor = item.ApresentacaoSetorID = apresentacaoSetorID;
                        preco.Nome.Valor = item.Preco;
                        preco.CorID.Valor = item.CorID;
                        preco.Quantidade.Valor = 0;
                        preco.QuantidadePorCliente.Valor = 0;
                        preco.Valor.Valor = Convert.ToDecimal(item.Valor);
                        preco.Impressao.Valor = Preco.IMPRESSAO_AMBOS;


                        preco.Inserir(item.EventoID, item.SetorID, item.ApresentacaoID, true, bd);

                        //preco.Inserir(bd);

                        this.PrecoID.Valor = preco.Control.ID;
                        this.Inserir(bd);
                        break;
                    //Não existe, precisa criar
                    case Enumerators.TipoAcaoPreco.Associar:
                        this.PrecoID.Valor = item.PrecoID;
                        this.Inserir(bd);
                        break;
                    case Enumerators.TipoAcaoPreco.Remover:
                        this.Excluir(item.SerieItemID);
                        break;
                    case Enumerators.TipoAcaoPreco.Alterar:
                        this.Control.ID = item.SerieItemID;
                        this.PrecoID.Valor = item.PrecoID;
                        this.Atualizar(bd);
                        break;
                    case Enumerators.TipoAcaoPreco.Manter:
                    default:
                        break;
                }
            }
        }

    }

    public class SerieItemLista : SerieItemLista_B
    {

        public SerieItemLista() { }

        public SerieItemLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
