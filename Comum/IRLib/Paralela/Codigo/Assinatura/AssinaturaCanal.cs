/**************************************************
* Arquivo: AssinaturaCanal.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class AssinaturaCanal : AssinaturaCanal_B
    {

        public AssinaturaCanal() { }

        public AssinaturaCanal(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void Gerenciar(BD bd, List<EstruturaAssinaturaCanal> lstCanaisDisponiveis, int AssinaturaID)
        {
            foreach (var canalSerie in lstCanaisDisponiveis)
            {
                switch (canalSerie.Acao)
                {
                    case Enumerators.TipoAcaoCanal.Associar:
                        this.CanalID.Valor = canalSerie.CanalID;
                        this.AssinaturaID.Valor = AssinaturaID;
                        this.Inserir(bd);
                        break;
                    case Enumerators.TipoAcaoCanal.Remover:
                        if (canalSerie.ID == 0)
                            continue;
                        this.Excluir(bd,canalSerie.ID);
                        break;
                    case Enumerators.TipoAcaoCanal.Manter:
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public class AssinaturaCanalLista : AssinaturaCanalLista_B
    {

        public AssinaturaCanalLista() { }

        public AssinaturaCanalLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
