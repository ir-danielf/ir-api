using CTLib;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace IngressoRapido.Lib
{
    public class ValeIngressoTipo
    {
        public string URLImagem = ConfigurationManager.AppSettings["DiretorioImagensValeIngresso"].ToString();
        private const string SUFIXO_THUMB = "thumb";
        DAL oDAL = new DAL();
        public ValeIngressoTipo(int id)
        {
            this.ID = id;
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public string Acumulativo { get; set; }
        public string ValidadeData { get; set; }
        public int ValidadeDiasImpressao { get; set; }
        public decimal ValorPagamento { get; set; }
        public char ValorTipo { get; set; }
        public bool TrocaConveniencia { get; set; }
        public bool TrocaIngresso { get; set; }
        public bool TrocaEntrega { get; set; }


        public string Imagem
        {
            get
            {
                return URLImagem + ID.ToString("ivir000000") + ".jpg";
            }
        }

        public string ReleaseInternet { get; set; }
        public string ProcedimentoTroca { get; set; }
        public string Thumb
        {
            get { return URLImagem + Path.GetFileNameWithoutExtension(this.Imagem) + SUFIXO_THUMB + Path.GetExtension(this.Imagem); }
        }

        public ValeIngressoTipo getByID()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Nome, Valor, Acumulativo, IsNull(ValidadeData, 0) AS ValidadeData, IsNull(ValidadeDiasImpressao,0) AS ValidadeDiasImpressao, ProcedimentoTroca, ReleaseInternet,ValorPagamento,ValorTipo,TrocaConveniencia,TrocaIngresso,TrocaEntrega FROM ValeIngressoTipo (NOLOCK) ");
                stbSQL.Append("WHERE IR_ValeIngressoTipoID = " + this.ID);

                using (IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString()))
                {
                    if (dr.Read())
                    {
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.ReleaseInternet = dr["ReleaseInternet"].ToString();
                        this.ProcedimentoTroca = dr["ProcedimentoTroca"].ToString();
                        this.ValorPagamento = Convert.ToDecimal(dr["ValorPagamento"]);

                        switch (dr["Acumulativo"].ToString())
                        {
                            case "T":
                                this.Acumulativo = "Sim";
                                break;
                            case "F":
                                this.Acumulativo = "Não";
                                break;
                        }

                        this.ValorTipo = Convert.ToChar(dr["ValorTipo"].ToString());
                        switch (this.ValorTipo)
                        {
                            case 'V':
                                this.Valor = "R$" + Convert.ToDecimal(dr["Valor"]);
                                break;
                            case 'P':
                                this.Valor = Convert.ToInt32(dr["Valor"]) + "%";
                                break;
                        }

                        this.TrocaConveniencia = Convert.ToChar(dr["TrocaConveniencia"]) == 'T';
                        this.TrocaEntrega = Convert.ToChar(dr["TrocaEntrega"]) == 'T';
                        this.TrocaIngresso = Convert.ToChar(dr["TrocaIngresso"]) == 'T';

                        this.ValidadeDiasImpressao = Convert.ToInt32(dr["ValidadeDiasImpressao"]);
                        this.ValidadeData = dr["ValidadeData"].ToString();
                    }
                }
                return this;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}
