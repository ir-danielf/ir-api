using CTLib;
using System;
using System.Data;
using System.Drawing;

namespace IngressoRapido.Lib
{
    public class Setor
    {
        public enum LugarTipo
        {
            Pista = 'P',
            Cadeira = 'C',
            MesaAberta = 'A',
            MesaFechada = 'M'
        }


        public Setor()
        { }
        public Setor(int id)
        { this.id = id; }

        DAL oDAL = new DAL();

        private int id;
        public int Id
        {
            set { id = value; }
            get { return id; }
        }

        private string nome;
        public string Nome
        {
            get { return Util.ToTitleCase(this.nome); }
            set { nome = value; }
        }

        public PrecoLista PrecoLista { get; set; }

        private String lugarMarcado;
        public String LugarMarcado
        {
            get { return lugarMarcado; }
            set { lugarMarcado = value; }
        }

        private int apresentacaoID;
        public int ApresentacaoID
        {
            get { return apresentacaoID; }
            set { apresentacaoID = value; }
        }

        private int qtdeDisponivel;
        public int QtdeDisponivel
        {
            get { return qtdeDisponivel; }
            set { qtdeDisponivel = value; }
        }

        private int quantidadeMapa;
        public int QuantidadeMapa
        {
            get { return quantidadeMapa; }
            set { quantidadeMapa = value; }
        }

        public string IdentificacaoComposta
        {
            get
            {
                return
                    this.id + "-" + this.lugarMarcado + "-" + this.qtdeDisponivel + "-" + quantidadeMapa;
            }
        }

        public string NomeQuantidadeMapa
        {
            get
            {
                if (this.lugarMarcado == "M")
                    return this.nome + " (" + this.quantidadeMapa + " lugares)";
                else
                    return this.nome;
            }
        }


        public bool AprovadoPublicacao { get; set; }

        PrecoLista oPrecos { get; set; }

        public Setor GetByID(int id)
        {
            string strSql = "SELECT IR_SetorID, Nome, LugarMarcado, ApresentacaoID, QtdeDisponivel, QuantidadeMapa " +
                            "FROM Setor (NOLOCK) " +
                            "WHERE (IR_SetorID = " + id + ")";
            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strSql);

                if (dr.Read())
                {
                    this.id = Convert.ToInt32(dr["IR_SetorID"].ToString());
                    this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                    this.LugarMarcado = dr["LugarMarcado"].ToString();
                    this.ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"].ToString());
                    this.QtdeDisponivel = Convert.ToInt32(dr["QtdeDisponivel"].ToString());
                    this.QuantidadeMapa = Convert.ToInt32(dr["QuantidadeMapa"].ToString());
                }

                // Fecha conexão da classe DataAccess
                oDAL.ConnClose();
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public static Image ProcurarImagem(string caminho, int setorID)
        {
            System.Drawing.Image img;
            try
            {
                img = System.Drawing.Image.FromFile(caminho + "s" + setorID.ToString("000000") + ".gif");
                //return img.GetThumbnailImage(Convert.ToInt32(img.Width * 0.9), Convert.ToInt32(img.Height * 0.9), null, IntPtr.Zero);
                return img;
            }
            catch
            {
                return null;
            }
        }

        public static Image ProcurarImagemThumbNail(int setorID, string caminho)
        {
            System.Drawing.Image img;
            try
            {
                img = System.Drawing.Image.FromFile(caminho + "s" + setorID.ToString("000000") + ".gif");
                return img.GetThumbnailImage(Convert.ToInt32(img.Width * 0.9), Convert.ToInt32(img.Height * 0.9), null, IntPtr.Zero);
            }
            catch
            {
                return null;
            }
        }
    }
}