using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEvento
    {
        public EstruturaEvento()
        {
            this.Apresentacoes = new List<EstruturaApresentacaoSimples>();
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string imagem;
        public string Imagem
        {
            get { return imagem; }
            set { imagem = value; }
        }

        private string local;
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        private int localid;

        public int LocalID
        {
            get { return localid; }
            set { localid = value; }
        }

        private string cidade;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string pais;
        public string Pais
        {
            get { return pais; }
            set { pais = value; }
        }

        private int tipo;
        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private int subTipo;
        public int SubTipo
        {
            get { return subTipo; }
            set { subTipo = value; }
        }

        private List<EstruturaApresentacaoSimples> apresentacoes;
        public List<EstruturaApresentacaoSimples> Apresentacoes
        {
            get { return apresentacoes; }
            set { apresentacoes = value; }
        }

        public int Distancia { get; set; }

        public IRLib.Paralela.Evento.SemVendaMotivo NaoVendaMotivo { get; set; }

        public DateTime? NaoVendaDataInicio { get; set; }
    }

    [Serializable]
    public class EstruturaApresentacaoSimples
    {
        private int ir_apresentacaoID;
        public int IR_ApresentacaoID
        {
            get { return ir_apresentacaoID; }
            set { ir_apresentacaoID = value; }
        }

        private string horarioFormatado;
        public string HorarioFormatado
        {
            get { return horarioFormatado; }
            set { horarioFormatado = value; }
        }

        private int eventoID;
        public int EventoID
        {
            get { return eventoID; }
            set { eventoID = value; }
        }

        private DateTime horario;
        public DateTime Horario
        {
            get { return horario; }
            set { horario = value; }
        }

    }
}
