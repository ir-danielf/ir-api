using CTLib;
using System;
using System.Data;

namespace IngressoRapido.Lib
{
    public class CotaItem : ICloneable
    {

        public CotaItem() { }
        public CotaItem(int ingressoID) { this.IngressoID = ingressoID; }

        public int ID { get; set; }
        public bool ValidaBin { get; set; }
        public bool Nominal { get; set; }
        public int Quantidade { get; set; }
        public string Termo { get; set; }
        public bool TemTermo { get; set; }
        public int ParceiroID { get; set; }
        private string textoValidacao { get; set; }
        public string TextoValidacao
        {
            get
            {
                if (this.ID > 0 && ParceiroID > 0 && !ValidaBin && textoValidacao.Length == 0)
                    return IRLib.Cota.TextoValidacaoPadrao;
                else
                    return textoValidacao;
            }
            set { textoValidacao = value; }
        }

        public int DonoID { get; set; }
        public string DonoCPF { get; set; }
        public string CodigoPromocional { get; set; }
        public int IngressoID { get; set; }
        public int ObrigatoriedadeID { get; set; }

        private bool verificado { get; set; }
        public bool Verificado
        {
            get
            {
                if (!this.Nominal && (this.ValidaBin || (!this.ValidaBin && this.ParceiroID == 0)))
                    return true;

                return verificado;
            }
            set
            {
                if (!this.Nominal && (this.ValidaBin || (!this.ValidaBin && this.ParceiroID == 0)))
                    verificado = true;

                verificado = value;
            }
        }

        public bool ExibirDados
        {
            get
            {
                if (!this.Nominal && (this.ValidaBin || (!this.ValidaBin && this.ParceiroID == 0)))
                    return false;

                return true;
            }
        }

        public bool ValidaCodidoPromocional { get; set; }

        public CotaItem GetByID(int ID)
        {
            //Retorna vazio
            if (ID == 0)
                return null;

            DAL oDal = new DAL();
            try
            {
                using (IDataReader dr = oDal.SelectToIDataReader(
                    @"SELECT TOP 1 IR_CotaItemID AS ID, Nominal, ParceiroID, ValidaBin, TextoValidacao, ObrigatoriedadeID,
                        CASE WHEN LEN(Termo) > 0 
                            THEN 1
                            ELSE 0
                        END AS TemTermo
                        FROM CotaItem (NOLOCK)
                        WHERE IR_CotaItemID = " + ID))
                {
                    if (dr.Read())
                    {
                        this.ID = dr["ID"].ToInt32();
                        this.Nominal = dr["Nominal"].ToBoolean();
                        this.ParceiroID = dr["ParceiroID"].ToInt32();
                        this.ValidaBin = dr["ValidaBin"].ToBoolean();
                        this.TextoValidacao = dr["TextoValidacao"].ToString();
                        this.TemTermo = dr["TemTermo"].ToBoolean();
                        this.DonoID = 0;
                        this.DonoCPF = string.Empty;
                        this.CodigoPromocional = string.Empty;
                        this.ObrigatoriedadeID = dr["ObrigatoriedadeID"].ToInt32();
                        this.ValidaCodidoPromocional = (this.ParceiroID > 0 && !this.ValidaBin ? true : false);
                    }
                    else
                        return null;
                }
                return this;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public object Clone()
        {
            if (this == null)
                return null;

            return this.MemberwiseClone();
        }
    }
}
