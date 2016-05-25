using CTLib;
using System;
using System.Data;
using System.Text;

namespace IngressoRapido.Lib
{
    public class Preco
    {
        private Evento evento;
        private Apresentacao apresentacao;
        private Setor setor;



        private Local local;

        public Local Local
        {
            get { return this.local; }
            set { this.local = value; }
        }

        public Evento Evento
        {
            get { return this.evento; }
            set { this.evento = value; }
        }

        public Apresentacao Apresentacao
        {
            get { return this.apresentacao; }
            set { this.apresentacao = value; }
        }

        public Setor Setor
        {
            get { return this.setor; }
            set { this.setor = value; }
        }

        public string Pacote { get; set; }

        public Preco()
        {
        }

        public Preco(int id)
        {
            this.id = id;
        }

        private DAL oDAL = new DAL();
        private BD oBD = new BD();

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

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private int apresentacaoID;
        public int ApresentacaoID
        {
            get { return apresentacaoID; }
            set { apresentacaoID = value; }
        }

        private int setorID;
        public int SetorID
        {
            get { return setorID; }
            set { setorID = value; }
        }


        private int quantidadePorCliente;
        public int QuantidadePorCliente
        {
            get { return quantidadePorCliente; }
            set { quantidadePorCliente = value; }

        }

        public bool Principal { get; set; }

        public string ToGrid
        {
            get { return nome + " - " + valor.ToString("c"); }
        }

        public Preco CarregarEstruturaPorPreco(int precoID, int pacoteID)
        {
            string strSql = @"SELECT 
                            Local.IR_LocalID AS LocalID, 
                            Local.Nome AS LocalNome, 
                            Evento.Nome AS EventoNome, 
                            Horario, 
                            Setor.IR_SetorID AS SetorID,
                            Setor.Nome AS SetorNome, 
                            IR_PrecoID, Preco.Nome, Preco.Valor,
                            p.Nome AS PacoteNome,
                            Apresentacao.IR_ApresentacaoID AS ApresentacaoID
                            FROM Preco (NOLOCK) 
                            INNER JOIN Setor (NOLOCK) ON IR_SetorID = SetorID AND Setor.ApresentacaoID = Preco.ApresentacaoID 
                            INNER JOIN Apresentacao (NOLOCK) ON IR_ApresentacaoID = Setor.ApresentacaoID 
                            INNER JOIN Evento (NOLOCK) ON IR_EventoID = EventoID 
                            INNER JOIN Local (NOLOCK) ON IR_LocalID = LocalID 
                            INNER JOIN PacoteItem pi (NOLOCK) ON pi.EventoID = IR_EventoID
                            INNER JOIN Pacote p (NOLOCK) ON p.IR_PacoteID = pi.PacoteID
                            WHERE 
                            IR_PrecoID =" + precoID + " AND IR_PacoteID = " + pacoteID;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (!dr.Read())
                        throw new ApplicationException("Preço Indisponível");

                    this.id = Convert.ToInt32(dr["IR_PrecoID"].ToString());
                    this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                    this.Valor = (decimal)dr["Valor"];

                    this.Evento = new Evento();
                    this.Apresentacao = new Apresentacao(Convert.ToInt32(dr["ApresentacaoID"]));
                    this.Setor = new Setor(Convert.ToInt32(dr["SetorID"]));
                    this.Local = new Local(dr["LocalID"] != DBNull.Value ? (int)dr["LocalID"] : 0);

                    this.Local.Nome = Util.ToTitleCase(dr["LocalNome"] != null ? dr["LocalNome"].ToString() : string.Empty);
                    this.Evento.Nome = Util.ToTitleCase(dr["EventoNome"] != null ? dr["EventoNome"].ToString() : string.Empty);
                    if (dr["Horario"] != null)
                        this.Apresentacao.Horario = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao);
                    this.Setor.Nome = Util.ToTitleCase(dr["SetorNome"] != null ? dr["SetorNome"].ToString() : string.Empty);
                    this.Pacote = Util.ToTitleCase(dr["PacoteNome"].ToString());
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

        public Preco GetByID(int id)
        {
            string strSql = "SELECT IR_PrecoID, Nome, Valor, ApresentacaoID, SetorID FROM Preco (NOLOCK) " +
                            "WHERE (IR_PrecoID = " + id + ")";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {

                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["IR_PrecoID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString());
                        this.ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"].ToString());
                        this.SetorID = Convert.ToInt32(dr["SetorID"].ToString());
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

        public int LerPrecoClienteVivo(int idApresentacao, int idSetor)
        {
            int retorno = 0;

            string strSql = "SELECT IR_PrecoID FROM Preco (NOLOCK) WHERE Nome='ClienteVivo' AND ApresentacaoID=" +
                            idApresentacao + " AND SetorID=" + idSetor;

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {

                    if (dr.Read())
                    {
                        retorno = Convert.ToInt32(dr["IR_PrecoID"].ToString());
                    }
                }

                oDAL.ConnClose();
                return retorno;
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

        public static string GerarOptions(int Quantidade)
        {
            Quantidade = Quantidade == 0 ? 8 : Quantidade;

            StringBuilder stb = new StringBuilder();
            stb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", 0, 0);
            for (int i = 1; i <= Quantidade; i++)
                stb.AppendFormat("<option value='{0}'>{1}</option>", i, i);

            return stb.ToString();
        }

        public bool ValidarPrecoLoteAtivo(int precoID, int quantidade)
        {
            return new IRLib.Preco().ValidarPrecoLoteAtivo(precoID, quantidade);
        }

        public bool ValidarMesmoLote(int precoIDOLD, int precoIDNEW)
        {
            return new IRLib.Preco().ValidarMesmoLote(precoIDOLD, precoIDNEW);
        }

        public Preco GetPrecoPrincipalID(int apresentacaoID, int setorID)
        {
            try
            {
                string strQuery = string.Format("select top 1 ID, Nome, Valor from PrecosDisponiveisPorApresentacaoSetor((select top 1 ID from tApresentacaoSetor (nolock) where ApresentacaoID = {0} AND SetorID = {1})) order by nome asc", apresentacaoID, setorID);

                oBD.Consulta(strQuery);

                Preco retorno = new Preco();
                if (oBD.Consulta().Read())
                {
                    retorno.Id = oBD.LerInt("ID");
                    retorno.Nome = oBD.LerString("Nome");
                    retorno.Valor = oBD.LerDecimal("Valor");
                    retorno.ApresentacaoID = apresentacaoID;
                    retorno.SetorID = setorID;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oBD.Fechar();
            }
        }
    }
}