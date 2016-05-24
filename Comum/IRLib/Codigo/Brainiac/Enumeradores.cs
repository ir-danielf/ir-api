using System;

namespace IRLib.Codigo.Brainiac
{
    [Serializable]
    public class Enumeradores
    {
        public enum EnumTipoAcao
        {
            Erro = '#',
            Inserir = '_',
            Remover = '!',
            Alterar = '=',
        }

        public enum EnumTipoRetorno
        {
            AvancarNome,
            ImplicarErro,
            AplicarNome,
            Ok,
            Parcial,
        }
    }
}
