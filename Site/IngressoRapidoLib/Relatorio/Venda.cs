using CTLib;
using IngressoRapido.Lib;
using System;
using System.Data;

namespace IngressoRapido.Relatorio
{
    public class Venda
    {
        public Venda()
        { 
        }

        public Venda(int id)
        {
            this.id = id;
        }

        DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        private string nome;
        public string Nome
        {
            get { return Util.ToTitleCase(this.nome); }
            set { nome = value; }
        }

        private string email;
        public string Email
        {
            get { return this.email; }
            set { email = value; }
        }

        private string telefone;
        public string Telefone
        {
            get { return this.telefone; }
            set { telefone = value; }
        }

        private string telefonecel;
        public string TelefoneCel
        {
            get { return Util.ToTitleCase(this.telefonecel); }
            set { telefonecel = value; }
        }

        private string senha;
        public string Senha
        {
            get { return Util.ToTitleCase(this.senha); }
            set { senha = value; }
        }

        private string tid;
        public string TID
        {
            get { return this.tid; }
            set { tid = value; }
        }

        private string timestamp;
        public string TimeStamp
        {
            get { return this.timestamp; }
            set { timestamp = value; }
        }

        private string numerocartao;
        public string NumeroCartao
        {
            get { return this.numerocartao; }
            set { numerocartao = value; }
        }

        private string msgRetornoAutenticacao;
        public string MsgRetornoAutenticacao
        {
            get { return this.msgRetornoAutenticacao; }
            set { msgRetornoAutenticacao = value; }
        }

        private string msgRetornoCaptura;
        public string MsgRetornoCaptura
        {
            get { return this.msgRetornoCaptura; }
            set { msgRetornoCaptura = value; }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        // Visanet - Código de Autorização da Compra
        private string arp;
        public string ARP
        {
            get { return arp; }
            set { arp = value; }
        }
      
        // Redecard - Código de Autorização da Compra
        private string cv;
        public string CV
        {
            get { return cv; }
            set { cv = value; }
        }

        private string numAutorizacao;
        public string NumAutorizacao
        {
            get { return numAutorizacao; }
            set { numAutorizacao = value; }
        }

        private int parcelas;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        private string token;
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        private string tokenTemporario;
        public string TokenTemporario
        {
            get { return tokenTemporario; }
            set { tokenTemporario = value; }
        }
        
        private string hsbcNome;
        public string HSBCNome
        {
            get { return hsbcNome; }
            set { hsbcNome = value; }
        }

        private string hsncMensagem1;
        public string HSBCMensagem1
        {
            get { return hsncMensagem1; }
            set { hsncMensagem1 = value; }
        }

        private string hsbcMensagem2;
        public string HSBCMensagem2
        {
            get { return hsbcMensagem2; }
            set { hsbcMensagem2 = value; }
        }

        private string numeroPedido;
        public string NumeroPedido
        {
            get { return numeroPedido; }
            set { numeroPedido = value; }
        }

        //SELECT c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular, r.Senha, 
        //r.tid, r.TimeStamp, r.NR_CARTAO, r.MSGRET, r.MSGRET_confirmacao, r.total, r.CODRET 
        //FROM SV05.Ingressos.dbo.tCliente c, Redecard r WHERE c.ID = r.ClienteID
        public Venda GetByID(int id)
        {
            string strSql = "SELECT c.ID, c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular, r.Senha, " +
                            "Fr.tid, r.TimeStamp, r.NR_CARTAO, r.MSGRET, r.MSGRET_confirmacao, r.total, r.CODRET " +
                            "FROM "+VendaLista.serverName+".Ingressos.dbo.tCliente c, Redecard r WHERE c.ID = " + id + " AND c.ID = r.ClienteID";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["ID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.Email = dr["Email"].ToString().Trim();
                        this.Telefone = "(" + dr["DDDTelefone"].ToString().Trim() + ")" + dr["Telefone"].ToString().Trim();
                        this.TelefoneCel = "(" + dr["DDDCelular"].ToString().Trim() + ")" + dr["Celular"].ToString().Trim();
                        this.Senha = dr["Senha"].ToString().Trim();
                        this.TID = dr["tid"].ToString().Trim();
                        this.TimeStamp = dr["TimeStamp"].ToString();
                        this.NumeroCartao = dr["NR_CARTAO"].ToString();
                        this.MsgRetornoAutenticacao = dr["MSGRET"].ToString();
                        this.MsgRetornoCaptura = dr["MSGRET_confirmacao"].ToString();
                        this.Total = Convert.ToDecimal(dr["total"].ToString());
                        this.Status = Util.LimparTitulo(dr["CODRET"].ToString());
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
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
    }
}
