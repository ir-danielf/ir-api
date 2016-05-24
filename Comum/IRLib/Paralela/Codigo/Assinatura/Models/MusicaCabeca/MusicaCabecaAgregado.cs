using System;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class MusicaCabecaAgregado
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public int MusicaCabecaInscritoID { get; set; }
        public int AgregadoID { get; set; }
        public bool Presente { get; set; }
        public string PresenteValor
        {
            get
            {
                return Presente ? "Sim" : "Não";
            }
        }
        public bool Inscrito { get; set; }
        public string InscritoValor
        {
            get
            {
                return Inscrito ? "Sim" : "Não";
            }
        }
        public DateTime DataInscricao { get; set; }

    }
}
