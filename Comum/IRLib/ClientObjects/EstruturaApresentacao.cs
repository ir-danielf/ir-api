using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaApresentacao
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int eventoid;
        public int EventoID
        {
            get { return eventoid; }
            set { eventoid = value; }
        }

        private DateTime horario;
        public DateTime Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        private bool disponivelajuste;
        public bool DisponivelAjuste
        {
            get { return disponivelajuste; }
            set { disponivelajuste = value; }
        }

        private bool disponivelrelatorio;
        public bool DisponivelRelatorio
        {
            get { return disponivelrelatorio; }
            set { disponivelrelatorio = value; }
        }

        private bool disponivelvenda;
        public bool DisponivelVenda
        {
            get { return disponivelvenda; }
            set { disponivelvenda = value; }
        }

        private int versaoimagemingresso;
        public int VersaoImagemIngresso
        {
            get { return versaoimagemingresso; }
            set { versaoimagemingresso = value; }
        }

        private int versaoimagemvale;
        public int VersaoImagemVale
        {
            get { return versaoimagemvale; }
            set { versaoimagemvale = value; }
        }

        private int versaoimagemvale2;
        public int VersaoImagemVale2
        {
            get { return versaoimagemvale2; }
            set { versaoimagemvale2 = value; }
        }

        private int versaoimagemvale3;
        public int VersaoImagemVale3
        {
            get { return versaoimagemvale3; }
            set { versaoimagemvale3 = value; }
        }

        private string impressao;
        public string Impressao
        {
            get { return impressao; }
            set { impressao = value.Substring(0, 1); }
        }

        private int localimagemmapaid;
        public int LocalImagemMapaID
        {
            get { return localimagemmapaid; }
            set { localimagemmapaid = value; }
        }

        private bool descricaopadrao;
        public bool DescricaoPadrao
        {
            get { return descricaopadrao; }
            set { descricaopadrao = value; }
        }

        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        private string obs;
        public string Obs
        {
            get { return obs; }
            set { obs = value; }
        }

        private int ultimocodigoimpresso;
        public int UltimoCodigoImpresso
        {
            get { return ultimocodigoimpresso; }
            set { ultimocodigoimpresso = value; }
        }

        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
        public int CotaID { get; set; }
        public int MapaEsquematicoID { get; set; }

    }
}