using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization;

namespace IngressoRapido.Lib
{
    public class SerieItem
    {
        public int ID { get; set; }
        public int SerieID { get; set; }

        public int EventoID { get; set; }

        [DataMember]
        public string Evento { get; set; }

        public int ApresentacaoID { get; set; }
        public DateTime Horario { get; set; }

        [DataMember]
        public string HorarioStr
        {
            get
            {
                if (Horario == DateTime.MinValue)
                    return "Todos";

                return this.Horario.ToShortDateString() + " às " + this.Horario.ToShortTimeString();
            }
        }

        public int SetorID { get; set; }
        [DataMember]

        public string Setor { get; set; }
        [DataMember]

        public int PrecoID { get; set; }
        [DataMember]

        public string Preco { get; set; }
        [DataMember]

        public int Quantidade { get; set; }
        [DataMember]
        public bool EscolherLugarMarcado { get; set; }

        public int QuantidadePorCliente { get; set; }
        public bool Disponivel { get; set; }

        public string Valor { get; set; }

        public bool Promocional { get; set; }

        public int QuantidadePorPromocional { get; set; }
        public string Background
        {
            get
            {
                return
                    this.Imagem == null ? "0" : this.SetorID.ToString("000000");

            }
        }
        public int BackgroundHeight
        {
            get
            {
                return this.imagem == null ?
                    0 :
                    Imagem.GetThumbnailImage(Convert.ToInt32(Imagem.Width * 0.9), Convert.ToInt32(Imagem.Height * 0.9), null, IntPtr.Zero).Height;
            }
        }
        public int BackgroundWidth
        {
            get
            {
                return this.imagem == null ?
                    0 :
                    Imagem.GetThumbnailImage(Convert.ToInt32(Imagem.Width * 0.9), Convert.ToInt32(Imagem.Height * 0.9), null, IntPtr.Zero).Width;
            }
        }

        private Image imagem { get; set; }
        private Image Imagem
        {
            get
            {
                //if (imagem == null)
                //    imagem = this.EncontrarBG();

                return imagem;

            }
        }

        public Image EncontrarBG()
        {
            if (!this.EscolherLugarMarcado || !this.Disponivel)
                return null; ;

            return IngressoRapido.Lib.Setor.ProcurarImagem(
                ConfigurationManager.AppSettings["DiretorioImagensBackgroundSetor"], this.SetorID);
        }

        public void GetByPrecoSerie(int SerieID, int PrecoID)
        {
            DAL oDal = new DAL();
            try
            {
                string sql =
                    string.Format(@"SELECT TOP 1 IR_SerieItemID AS ID, Promocional, QuantidadePorPromocional 
                    FROM SerieItem (NOLOCK) 
                    WHERE SerieID = {0} AND PrecoID = {1}", SerieID, PrecoID);

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    if (!dr.Read())
                        return;

                    this.ID = dr["ID"].ToInt32();
                    this.Promocional = dr["Promocional"].ToBoolean();
                    this.QuantidadePorPromocional = dr["QuantidadePorPromocional"].ToInt32();
                }
            }
            finally
            {
                oDal.ConnClose();
            }
        }
    }


    public class SerieItemCSS
    {
        public string Evento { get; set; }
        public int ApresentacaoID { get; set; }
        public string Data { get; set; }
        public List<string> Setores { get; set; }
        public string Programacao { get; set; }
    }
}
