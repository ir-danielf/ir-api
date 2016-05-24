using System;


namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class Lugar
    {
		public int Index;
        public int ID;
        public int SetorID;
        public string Codigo;
        public int Quantidade;
        public int QuantidadeBloqueada;
        public int PosicaoX;
        public int PosicaoY;
        public int Simbolo;
        public int BloqueioID;
        public int Classificacao;
        public int Grupo;
        public string Obs;
        public int PerspectivaLugarID;
        public TiposAcao Acao;
        
        public enum TiposAcao
        {
            Inserir,
            Atualizar,
            Excluir
        }         

    }
}
