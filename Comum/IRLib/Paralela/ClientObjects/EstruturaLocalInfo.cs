using System;

namespace IRLib.Paralela.ClientObjects
{
	[Serializable]
    public class EstruturaLocalInfo
    {
        public int LocalID;
        public string Endereco;
        public int Numero;
        public string Complemento;
        public string Bairro;
        public string Cep;
        public string Cidade;
        public string Pais;
        public string Estado;

        public Nullable<bool> Estacionamento;
        public Nullable<bool> AcessoNecessidadeEspecial;
        public string AcessoNecessidadeEspecialObs;
        public string EstacionamentoObs;
        public Nullable<bool> ArCondicionado;
        public string ComoChegar;
        public string HorariosBilheteria;
        public bool Carregou;
    }
}
