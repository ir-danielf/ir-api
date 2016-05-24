using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace IRLib.Paralela.Willcall
{
    #region Estruturas dos Objetos do Banco

    [Serializable]
    public class IngressoReimpresso
    {
        public int ID { get; set; }
        public int IngressoID { get; set; }
        public string Usuario { get; set; }
        public string Timestamp { get; set; }
    }

    [Serializable]
    public class IngressoCodigoBarra
    {
        public int ID { get; set; }
        public int EventoID { get; set; }
        public string CodigoBarra { get; set; }
        public string BlackList { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; }
    }

    [Serializable()]
    public class EntregaInfo
    {
        public Compra Compra { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Usuario { get; set; }
        public int UsuarioID { get; set; }
    }

    [Serializable()]
    public class Compra
    {
        public Compra()
        {
            this.Ingressos = new List<Ingresso>();
        }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public decimal TaxaConveniencia { get; set; }
        public decimal Total { get; set; }
        public decimal ValorEntrega { get; set; }
        public DateTime DataVenda { get; set; }
        public string TaxaEntregaNome { get; set; }
        public string Canal { get; set; }
        public string Pagamento { get; set; }
        public string Cartao { get; set; }
        public string EntregaNome { get; set; }
        public string TaxaEntregaTipo { get; set; }
        public decimal TaxaEntregaValor { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Entregue { get; set; }
        public string Usuario { get; set; }

        public int VendaBilheteriaID { get; set; }
        public int LojaID { get; set; }
        public int CanalID { get; set; }
        public int UsuarioID { get; set; }
        public int EmpresaID { get; set; }

        public List<Ingresso> Ingressos { get; set; }
        public int QuantidadeIngressos
        {
            get
            {
                if (this.Ingressos == null)
                    return 0;
                else
                    return this.Ingressos.Count;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", this.Nome, this.CPF, this.Senha);
        }
    }

    [Serializable()]
    public class Ingresso
    {
        public int IngressoID { get; set; }
        public int VendaBilheteriaID { get; set; }
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int PrecoID { get; set; }
        public int BloqueioID { get; set; }
        public int CortesiaID { get; set; }
        public int EventoID { get; set; }
        public int SetorID { get; set; }
        public string CodigoBarraCliente { get; set; }
        public string CodigoBarra { get; set; }
        public string CodigoSequencial { get; set; }
        public int CodigoImpressao { get; set; }
        public string Status { get; set; }
        public DateTime Horario { get; set; }
        public string Evento { get; set; }
        public string Setor { get; set; }
        public string Preco { get; set; }
        public decimal Valor { get; set; }
        public string Senha { get; set; }
        public string Codigo { get; set; }
        public string ImprimirCarimbo { get; set; }
        public string CarimboTexto1 { get; set; }
        public string CarimboTexto2 { get; set; }
        public int CanalID { get; set; }
        public int UsuarioID { get; set; }
        public int CaixaID { get; set; }
        public int LojaID { get; set; }
        public int ClienteID { get; set; }
        public int LocalID { get; set; }
        public string LocalNome { get; set; }
        public string LocalEndereco { get; set; }
        public string LocalEstado { get; set; }
        public string LocalCep { get; set; }
        public string LocalCidade { get; set; }
        public int EmpresaID { get; set; }
        public string Apresentacao
        {
            get
            {

                return Horario.ToLongDateString() + " às " + Horario.ToString("hh:mm");
            }
        }
        public string ApresentacaoImpressao
        {
            get
            {
                return Horario.ToString("dd/MM/yy - HH:mm");
            }
        }
        public string PrecoCompleto
        {
            get
            {
                return this.Preco + "(" + this.Valor.ToString("c") + ")";
            }
        }
    }

    [Serializable]
    public class CodigoBarra
    {
        public int Id { get; set; }
        public int EventoID { get; set; }
        public string EventoCodigo { get; set; }
        public int ApresentacaoID { get; set; }
        public string ApresentacaoCodigo { get; set; }
        public int SetorID { get; set; }
        public string SetorCodigo { get; set; }
        public int PrecoID { get; set; }
        public string PrecoCodigo { get; set; }
        public string Ativo { get; set; }
    }
    #endregion


    [ObjectType(ObjectType.RemotingType.SingleCall)]
    [JirayaObject]
    public class GerenciadorParalela : MarshalByRefObject
    {

        BD bd = new BD();

        public List<Usuario> CarregarUsuarios()
        {
            var sql = @"SELECT 
					DISTINCT  tUsuario.ID, tUsuario.Login, tUsuario.Senha, tUsuario.Status
					FROM tPerfilCanal (NOLOCK)
					INNER JOIN tUsuario(NOLOCK) ON tUsuario.ID = UsuarioID
					WHERE CanalID IN (3035,2985,3640,3642)";
            var usuarios = new List<Usuario>();

            BD bd = new BD();

            while (bd.Consulta(sql).Read())
            {
                var usuario = new Usuario();
                usuario.Control.ID = bd.LerInt("ID");
                usuario.Login.Valor = bd.LerString("Login");
                usuario.Senha.Valor = bd.LerString("Senha");
                usuario.Status.Valor = bd.LerString("Status");
                usuarios.Add(usuario);
            }

            sql = @"SELECT 
					DISTINCT  tUsuario.ID, tUsuario.Login, tUsuario.Senha, tUsuario.Status
					FROM tPerfilLocal (NOLOCK)
					INNER JOIN tUsuario(NOLOCK) ON tUsuario.ID = UsuarioID
					WHERE LocalID IN (2030,2031,2154)";

            while (bd.Consulta(sql).Read())
            {
                var usuario = new Usuario();
                usuario.Control.ID = bd.LerInt("ID");
                usuario.Login.Valor = bd.LerString("Login");
                usuario.Senha.Valor = bd.LerString("Senha");
                usuario.Status.Valor = bd.LerString("Status");
                usuarios.Add(usuario);
            }




            return usuarios;

        }

        public void CarregarIngressos(int apresentacaoID, List<Compra> compras, DateTime? ultimaChamada, int eventoID = 0)
        {
            var sql = @"
									SELECT DISTINCT
										tVendaBilheteria.ID AS VendaBilheteriaID,
										tEvento.Nome AS Evento, 
										tApresentacao.Horario AS Apresentacao,
										tSetor.Nome AS Setor,
										tPreco.Nome AS Preco,
										tPreco.Valor AS PrecoValor,
										tIngresso.ID AS IngressoID,
										tIngresso.PrecoID,
										tIngresso.ApresentacaoID,
										tIngresso.CodigoBarra,
										tIngresso.BloqueioID,
										tIngresso.CortesiaID,
										tIngresso.EventoID,
										tIngresso.CodigoSequencial,
										tIngresso.Status AS StatusIngresso,
										tIngresso.ApresentacaoSetorID,
										tVendaBilheteria.Senha
									FROM  tIngresso (NOLOCK)
									INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
									INNER JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tVendaBilheteria.EntregaControleID
									INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
									INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
									INNER JOIN tSetor(NOLOCK) ON tSetor.ID = tIngresso.SetorID
									INNER JOIN tPreco(NOLOCK) ON tPreco.ID = tIngresso.PrecoID
									WHERE
										tIngresso.Status IN ('V') ";
            if (eventoID > 0)
                sql += " AND tIngresso.EventoID = " + eventoID;
            else
                sql += "AND tIngresso.ApresentacaoID = " + apresentacaoID;

            if (ultimaChamada.HasValue)
                sql += " AND DataVenda >= '" + ultimaChamada.Value.ToString("yyyyMMddHHmmss") + "'";


            while (bd.Consulta(sql).Read())
            {
                var compra = compras.Where(c => c.Senha.Equals(bd.LerString("Senha"))).FirstOrDefault();

                if (compra == null)
                {
                    compras.Remove(compra);
                    continue;
                }

                compra.Ingressos.Add(new Ingresso
                {
                    IngressoID = bd.LerInt("IngressoID"),
                    VendaBilheteriaID = bd.LerInt("VendaBilheteriaID"),
                    ApresentacaoID = bd.LerInt("ApresentacaoID"),
                    ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                    PrecoID = bd.LerInt("PrecoID"),
                    BloqueioID = bd.LerInt("BloqueioID"),
                    CortesiaID = bd.LerInt("CortesiaID"),
                    EventoID = bd.LerInt("EventoID"),

                    CodigoBarra = bd.LerString("CodigoBarra"),
                    CodigoSequencial = bd.LerString("CodigoSequencial"),
                    Status = bd.LerString("StatusIngresso"),
                    Horario = bd.LerDateTime("Apresentacao"),
                    Evento = bd.LerString("Evento"),
                    Setor = bd.LerString("Setor"),
                    Preco = bd.LerString("Preco"),
                    Valor = bd.LerDecimal("PrecoValor"),
                    Senha = bd.LerString("Senha")

                });
            }
        }

        public List<Compra> CarregarCompras(int apresentacaoID, DateTime? ultimaChamada = null, int eventoID = 0)
        {

            var sql = @"SELECT DISTINCT 
	                    tCliente.Nome AS Nome,
	                    tCliente.CPF,
	                    tCliente.Email,
	                    tVendaBilheteria.ID AS VendaBilheteriaID,
	                    tVendaBilheteria.Senha,
	                    tVendaBilheteria.TaxaConvenienciaValorTotal AS TaxaConveniencia,
	                    tVendaBilheteria.ValorTotal,
	                    tVendaBilheteria.TaxaEntregaValor,
	                    tVendaBilheteria.DataVenda,
	                    tTaxaEntrega.Nome AS TaxaEntregaNome,
	                    tLoja.ID AS LojaID,
	                    tCanal.Nome AS Canal,
	                    tCanal.ID AS CanalID,
	                    tFormaPagamento.Nome AS Pagamento,
	                    tCartao.NroCartao AS Cartao,	
	                    tEntrega.Nome AS EntregaNome,
	                    tEntrega.Tipo AS TaxaEntregaTipo,
	                    tUsuario.ID AS UsuarioID,
	                    tUsuario.EmpresaID,
                        tUsuario.Login
                    FROM  tIngresso (NOLOCK)
                    INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                    INNER JOIN tVendaBilheteriaFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.VendaBilheteriaID = tVendaBilheteria.ID
                    INNER JOIN tFormaPagamento (NOLOCK) ON tFormaPagamento.ID = tVendaBilheteriaFormaPagamento.FormaPagamentoID
                    INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                    INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
                    INNER JOIN tCanal (NOLOCK) ON tCanal.ID = tLoja.CanalID
                    INNER JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tVendaBilheteria.EntregaControleID
                    INNER JOIN tEntrega (NOLOCK) ON tEntrega.ID = tEntregaControle.EntregaID
                    LEFT JOIN tTaxaEntrega (NOLOCK) ON tTaxaEntrega.ID = tVendaBilheteria.TaxaEntregaID
                    LEFT JOIN tCartao (NOLOCK) ON tCartao.ID = tVendaBilheteriaFormaPagamento.CartaoID
                    INNER JOIN tCliente (NOLOCK) ON tCliente.ID = tIngresso.ClienteID
                    INNER JOIN tUsuario (NOLOCK) ON tUsuario.ID = tCaixa.UsuarioID
                    WHERE
	                    tIngresso.Status IN ('V') ";

            if (eventoID > 0)
                sql += " AND tIngresso.EventoID = " + eventoID;
            else
                sql += "AND tIngresso.ApresentacaoID = " + apresentacaoID;

            if (ultimaChamada.HasValue)
                sql += " AND DataVenda >= '" + ultimaChamada.Value.ToString("yyyyMMddHHmmss") + "'";

            var compras = new List<Compra>();


            while (bd.Consulta(sql).Read())
            {
                compras.Add(new Compra
                {
                    Nome = bd.LerString("Nome"),
                    CPF = bd.LerString("CPF"),
                    Senha = bd.LerString("Senha"),
                    Canal = bd.LerString("Canal"),
                    Pagamento = bd.LerString("Pagamento"),
                    Cartao = bd.LerString("Cartao"),
                    TaxaConveniencia = bd.LerDecimal("TaxaConveniencia"),
                    EntregaNome = bd.LerString("EntregaNome"),
                    ValorEntrega = bd.LerDecimal("TaxaEntregaValor"),
                    Total = bd.LerDecimal("ValorTotal"),
                    DataVenda = bd.LerDateTime("DataVenda"),
                    Entregue = false,
                    CanalID = bd.LerInt("CanalID"),
                    EmpresaID = bd.LerInt("EmpresaID"),
                    UsuarioID = bd.LerInt("UsuarioID"),
                    TaxaEntregaNome = bd.LerString("TaxaEntregaNome"),
                    TaxaEntregaTipo = bd.LerString("TaxaEntregaTipo"),
                    VendaBilheteriaID = bd.LerInt("VendaBilheteriaID"),
                    LojaID = bd.LerInt("LojaID"),
                    Email = bd.LerString("Email"),
                    TimeStamp = DateTime.Now.AddDays(-5),
                    Usuario = bd.LerString("Login")
                });
            }


            return compras;
        }

        public List<Ingresso> CarregarIngressos(int apresentacaoID, DateTime? ultimaChamada, int eventoID = 0)
        {

            var sql = @"SELECT DISTINCT	
	                    tEvento.Nome AS Evento,	 
	                    tApresentacao.Horario AS Apresentacao,
                        tSetor.ID AS SetorID,
	                    tSetor.Nome AS Setor,
	                    tPreco.Nome AS Preco,
	                    tPreco.Valor AS PrecoValor,
	                    tPreco.ImprimirCarimbo,
	                    tPreco.CarimboTexto1,
	                    tPreco.CarimboTexto2,
	                    tVendaBilheteria.ID AS VendaBilheteriaID,
	                    tApresentacao.ID AS ApresentacaoID,
	                    tIngresso.ApresentacaoSetorID,
	                    tIngresso.ID AS IngressoID,
	                    tPreco.ID AS PrecoID,	
	                    tIngresso.BloqueioID,
	                    tIngresso.CortesiaID,
	                    tEvento.ID AS EventoID,
	                    tIngresso.CodigoBarraCliente,
	                    tIngresso.CodigoBarra,
	                    tIngresso.CodigoSequencial,
	                    tIngresso.Status AS StatusIngresso,	
	                    tIngresso.Codigo,
                        tIngresso.CodigoImpressao,
	                    tIngresso.ClienteID,
	                    tIngresso.EmpresaID,
	                    tVendaBilheteria.Senha,
                        tLocal.ID AS LocalID,
	                    tLocal.Nome AS LocalNome,
	                    tLocal.Endereco AS LocalEndereco,
	                    tLocal.Cidade AS LocalCidade,
	                    tLocal.Estado AS LocalEstado,
	                    tLocal.CEP AS LocalCep,
	                    tLoja.ID AS LojaID,
	                    tLoja.CanalID,
	                    tCaixa.ID AS CaixaID,
	                    tCaixa.UsuarioID
                        FROM  tIngresso (NOLOCK)
                        INNER JOIN tVendaBilheteria (NOLOCK) ON tVendaBilheteria.ID = tIngresso.VendaBilheteriaID
                        INNER JOIN tEntregaControle (NOLOCK) ON tEntregaControle.ID = tVendaBilheteria.EntregaControleID
                        INNER JOIN tEvento (NOLOCK) ON tEvento.ID = tIngresso.EventoID
                        INNER JOIN tApresentacao(NOLOCK) ON tApresentacao.ID = tIngresso.ApresentacaoID
                        INNER JOIN tSetor(NOLOCK) ON tSetor.ID = tIngresso.SetorID
                        INNER JOIN tPreco(NOLOCK) ON tPreco.ID = tIngresso.PrecoID
                        INNER JOIN tLocal (NOLOCK) ON tLocal.ID = tEvento.LocalID
                        INNER JOIN tCaixa (NOLOCK) ON tCaixa.ID = tVendaBilheteria.CaixaID
                        INNER JOIN tLoja (NOLOCK) ON tLoja.ID = tCaixa.LojaID
                        WHERE
	                    tIngresso.Status IN ('V')";

            if (eventoID > 0)
                sql += " AND tIngresso.EventoID = " + eventoID;
            else
                sql += "AND tIngresso.ApresentacaoID = " + apresentacaoID;

            if (ultimaChamada.HasValue)
            {
                sql += " AND DataVenda >= '" + ultimaChamada.Value.ToString("yyyyMMddHHmmss") + "'";
            }

            List<Ingresso> ingressos = new List<Ingresso>();
            while (bd.Consulta(sql).Read())
            {

                ingressos.Add(new Ingresso
                {
                    IngressoID = bd.LerInt("IngressoID"),
                    VendaBilheteriaID = bd.LerInt("VendaBilheteriaID"),
                    ApresentacaoID = bd.LerInt("ApresentacaoID"),
                    ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                    PrecoID = bd.LerInt("PrecoID"),
                    BloqueioID = bd.LerInt("BloqueioID"),
                    CortesiaID = bd.LerInt("CortesiaID"),
                    EventoID = bd.LerInt("EventoID"),
                    SetorID = bd.LerInt("SetorID"),
                    CodigoBarra = bd.LerString("CodigoBarra"),
                    CodigoSequencial = bd.LerString("CodigoSequencial"),
                    CodigoImpressao = bd.LerInt("CodigoImpressao"),
                    Status = bd.LerString("StatusIngresso"),
                    Horario = bd.LerDateTime("Apresentacao"),
                    Evento = bd.LerString("Evento"),
                    Setor = bd.LerString("Setor"),
                    Preco = bd.LerString("Preco"),
                    Valor = bd.LerDecimal("PrecoValor"),
                    Senha = bd.LerString("Senha"),
                    Codigo = bd.LerString("Codigo"),
                    ImprimirCarimbo = bd.LerString("ImprimirCarimbo"),
                    CarimboTexto1 = bd.LerString("CarimboTexto1"),
                    CarimboTexto2 = bd.LerString("CarimboTexto2"),
                    CodigoBarraCliente = bd.LerString("CodigoBarraCliente"),
                    LocalID = bd.LerInt("LocalID"),
                    LocalNome = bd.LerString("LocalNome"),
                    LocalCep = bd.LerString("LocalCep"),
                    LocalCidade = bd.LerString("LocalCidade"),
                    LocalEndereco = bd.LerString("LocalEndereco"),
                    LocalEstado = bd.LerString("LocalEstado"),
                    CaixaID = bd.LerInt("CaixaID"),
                    LojaID = bd.LerInt("LojaID"),
                    CanalID = bd.LerInt("CanalID"),
                    ClienteID = bd.LerInt("ClienteID"),
                    UsuarioID = bd.LerInt("USuarioID"),
                    EmpresaID = bd.LerInt("EmpresaID")
                });
            }
            return ingressos;
        }

        public List<CodigoBarra> CarregarBaseCodigoBarra(int eventoID, int apresentacaoID)
        {
            List<CodigoBarra> listaRetorno = new List<CodigoBarra>();
            StringBuilder query = new StringBuilder(@"SELECT 
                                                    ID, EventoID, EventoCodigo, ApresentacaoID, ApresentacaoCodigo, SetorID, SetorCodigo, PrecoID, PrecoCodigo, Ativo
                                                    FROM tCodigoBarra (NOLOCK) 
                                                    WHERE EventoID = @001 AND ApresentacaoID = @002 AND Ativo = 'T'");
            query.Replace("@001", eventoID.ToString());
            query.Replace("@002", apresentacaoID.ToString());
            while (bd.Consulta(query).Read())
            {
                listaRetorno.Add(new CodigoBarra
                {
                    Id = bd.LerInt("ID"),
                    EventoID = bd.LerInt("EventoID"),
                    EventoCodigo = bd.LerString("EventoCodigo"),
                    ApresentacaoID = bd.LerInt("ApresentacaoID"),
                    ApresentacaoCodigo = bd.LerString("ApresentacaoCodigo"),
                    SetorID = bd.LerInt("SetorID"),
                    SetorCodigo = bd.LerString("SetorCodigo"),
                    PrecoID = bd.LerInt("PrecoID"),
                    PrecoCodigo = bd.LerString("PrecoCodigo"),
                    Ativo = bd.LerString("Ativo")
                });
            }
            return listaRetorno;
        }

        public List<string> GerarCodigoBarra(List<KeyValuePair<int, int>> listaChave, int eventoID)
        {
            List<string> retorno = new List<string>();
            IRLib.Paralela.IngressoCodigoBarra oIngressoCodigoBarra;
            IRLib.Paralela.CodigoBarra oCodigoBarra = new IRLib.Paralela.CodigoBarra();

            foreach (KeyValuePair<int, int> item in listaChave)
            {
                string codigoGerado = oCodigoBarra.NovoCodigoBarraIngresso(item.Key, eventoID, item.Value);

                oIngressoCodigoBarra = new IRLib.Paralela.IngressoCodigoBarra();
                oIngressoCodigoBarra.EventoID.Valor = eventoID;
                oIngressoCodigoBarra.CodigoBarra.Valor = codigoGerado;
                oIngressoCodigoBarra.BlackList.Valor = false;
                oIngressoCodigoBarra.TimeStamp.Valor = DateTime.Now;

                oIngressoCodigoBarra.Inserir();

                retorno.Add(codigoGerado);
            }

            return retorno;
        }

        public List<int> BuscarIngressosImpressos(string timestamp)
        {
            List<int> lstRetorno = new List<int>();
            StringBuilder query = new StringBuilder(@"SELECT tIngresso.ID 
                                                        FROM tIngresso (NOLOCK), tIngressoLog (NOLOCK)
                                                        WHERE tIngressoLog.IngressoID = tIngresso.ID
	                                                            AND tIngressoLog.Acao = 'I' 
	                                                            AND tIngressoLog.TimeStamp >= '@001' 
	                                                            AND tIngresso.EventoID = @002");
            query.Replace("@001", timestamp);
            query.Replace("@002", ConfigurationManager.AppSettings["EventoID"].ToString());
            try
            {
                if (bd.Consulta(query).Read())
                {
                    lstRetorno.Add(bd.LerInt(0));
                }
                return lstRetorno;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AtualizarStatusIngressoImpresso(int id)
        {
            StringBuilder query = new StringBuilder(@"UPDATE tIngresso SET Status = '@001' WHERE ID = @002");
            query.Replace("@001", IRLib.Paralela.Ingresso.IMPRESSO);
            query.Replace("@002", id.ToString());
            try
            {
                bd.Executar(query);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SincronizarReimpressao(int ingressoID, string timeStamp, string acao, int precoID, int BloqueioID, int EmpresaID, int vendaBilheteriaID, string codigoBarra, int codigoImpressao)
        {
            string query = @"INSERT INTO tIngressoLog (IngressoID, TimeStamp, Acao, PrecoID, BloqueioID, EmpresaID, VendaBilheteriaID, CodigoBarra, CodigoImpressao) 
				  VALUES (@001,'@002','@003', @004, @005, @005, @006, @007, '@008','@009')";
            query.Replace("@001", ingressoID.ToString());
            query.Replace("@002", timeStamp);
            query.Replace("@003", acao);
            query.Replace("@004", precoID.ToString());
            query.Replace("@005", BloqueioID.ToString());
            query.Replace("@006", EmpresaID.ToString());
            query.Replace("@007", vendaBilheteriaID.ToString());
            query.Replace("@008", codigoBarra);
            query.Replace("@009", codigoImpressao.ToString());

            try
            {
                bd.Executar(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
