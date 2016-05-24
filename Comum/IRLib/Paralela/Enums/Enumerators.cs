using System;

namespace IRLib.Paralela
{
    [Serializable]
    public class Enumerators
    {
        public enum TipoParceiro
        {
            Bin = 1,
            Codigo = 2,
            CodigoExterno = 3,
            Indefinido = 0,
        }
        public enum TipoChat
        {
            IR = 0,
            NIR = 1,
        }

        public enum TipoAberturaArea
        {
            Gerenciar = 0,
            Associar = 1,
        }

        public enum TipoFiltro
        {
            Frente = 0,
            Tras = 1,
        }

        public enum TipoAberturaSerie
        {
            Novo = 0, //Create Mode
            Antigo = 1, //Edit Mode
            Parcial = 2, //Identifica que somente alguns itens podem ser alterados
        }
        public enum TipoAcaoPreco
        {
            AssociarECriar = 0,
            Associar = 1,
            Remover = 2,
            Manter = 3,
            Alterar = 4,
        }
        public enum TipoAcaoCanal
        {
            Associar = 0,
            Remover = 1,
            Manter = 2,
        }

        public enum TipoAcaoCanalIR
        {
            Manter = 0,
            Distribuir = 1,
            Remover = 2,
        }

        public enum TipoPerfil
        {
            Especial,
            Regional,
            Local,
        }

        public enum TipoArvore
        {
            Regional,
            Empresa,
            Local,
            Evento,
            Apresentacao,
            Setor,
            Preco
        }

        public enum TipoCodigoBarra
        {
            ListaBranca = 'B',
            Estruturado = 'E'
        }

        public enum TipoDataSet
        {
            Locais = 0,
            Eventos = 1,
            Apresentacao = 2,
            Setores = 3,
            Precos = 4,
            Cortesias = 5,
            Boqueios = 6,
            Obrigatoriedade = 7
        }

        public enum TipoDataSetPacote
        {
            pacotesLugarMarcado = 0,
            pacotesitens = 1,
            pacotes = 2
        }

    }
}
