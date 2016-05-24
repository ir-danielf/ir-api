using CTLib;
using IRLib.HammerHead;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IRLib
{
    public class Accertify
    {
        private BD bd = new BD();

        public static IRLib.HammerHead.Enumeradores.RetornoAccertify ParseRetorno(string recommendation)
        {
            try
            {
                var retorno = (IRLib.RetornoAccertify.Recommendation)Enum.Parse(typeof(IRLib.RetornoAccertify.Recommendation), recommendation);

                switch (retorno)
                {
                    case IRLib.RetornoAccertify.Recommendation.ACCEPT:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.Aceitar;
                    case IRLib.RetornoAccertify.Recommendation.REJECT:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.CancelarAltoRisco;
                    case IRLib.RetornoAccertify.Recommendation.REVIEW:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.AguardarReview;
                    case IRLib.RetornoAccertify.Recommendation.FOLLOWUP:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.AcompanhamentoComCliente;
                    case IRLib.RetornoAccertify.Recommendation.CANCEL:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.CancelarSemFraude;
                    default:
                        return IRLib.HammerHead.Enumeradores.RetornoAccertify.AguardarReview;
                }
            }
            catch
            {
                return Enumeradores.RetornoAccertify.Indefinido;
            }
        }

        public void GerarHistoricalData(int quantidade, string de, string ate, string caminho)
        {
            try
            {
                bd.Consulta(string.Format(@"
                       SELECT {0}
	                        vb.Senha AS Senha, vb.ID AS VendaBilheteriaID, vb.Senha, vb.Status, vb.DataVenda, vb.ValorTotal, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.IP, cn.Nome AS Canal,
	                        c.ID AS ClienteID, c.CPF AS CPFCliente, c.DataCadastro, c.Nome AS NomeCliente, c.Email AS EmailCliente, c.DDDTelefone + c.Telefone AS TelefoneCliente, c.EnderecoCliente, c.CidadeCliente, c.EstadoCliente, c.CEPCliente, 'BR' AS PaisCliente,
	                        ce.Nome, ce.CPF, ce.Endereco, ce.Cidade, ce.Estado, ce.CEP, e.Nome AS Entrega, e.PrazoEntrega, e.ID AS EntregaID,
	                        CASE WHEN ea.ID IS NOT NULL
		                        THEN ea.Data + '235959'
		                        ELSE
			                        CONVERT(VARCHAR,DateAdd(Day, e.PrazoEntrega, CONVERT(DATETIME, SUBSTRING(vb.DataVenda, 0 ,9), 112)), 112) + '235959'
	                        END AS DataEntrega
	                        FROM tVendaBilheteria vb (NOLOCK)
	                        INNER JOIN tCaixa cx (NOLOCK) ON cx.ID = vb.CaixaID
	                        INNER JOIN tLoja l (NOLOCK) ON l.ID = cx.LojaID
	                        INNER JOIN tCanal cn (NOLOCK) ON cn.ID = l.CanalID
	                        INNER JOIN tCliente c (NOLOCK) ON vb.ClienteID = c.ID
	                        LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
	                        INNER JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID
	                        INNER JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID 
	                        LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID
	                        WHERE 
								vb.DataVenda >= '{1}' AND vb.DataVenda <= '{2}' AND vb.Status = 'F' AND cn.ID IN(1, 2)
								--vb.ID IN (7796494, 7800386)
                ", quantidade > 0 ? "TOP " + quantidade : string.Empty, de, ate));

                if (!bd.Consulta().Read())
                    throw new Exception("Derp! No results!");

                var transaction = new List<order>();
                var ids = new List<int>();

                do
                {
                    ids.Add(bd.LerInt("VendaBilheteriaID"));

                    transaction.Add(new order()
                    {
                        orderNumber = bd.LerString("Senha"),
                        orderType = "Sale",
                        orderDateTime = bd.LerDateTimeAccertify("DataVenda"),
                        totalAmount = bd.LerDecimal("ValorTotal").ToString("#0.00"),
                        totalSalesTax = bd.LerDecimal("TaxaConvenienciaValorTotal").ToString("#0.00"),
                        totalShippingCharges = bd.LerDecimal("TaxaEntregaValor").ToString("#0.00"),
                        ipAddress = bd.LerString("IP"),
                        salesChannel = bd.LerString("Canal").RemoveAcentos().ToUpper(),
                        paymentInformation = new paymentInformation()
                        {

                        },
                        failedCCInformation = new failedCCInformation() { },
                        memberInformation = new memberInformation()
                        {
                            memberID = bd.LerInt("ClienteID").ToString(),
                            memberCPF = bd.LerString("CPFCliente").ToString(),
                            membershipDate = bd.LerDateTimeAccertify("DataCadastro"),
                            memberFullName = bd.LerString("NomeCliente").RemoveAcentos().ToUpper(),
                            memberFirstName = bd.LerString("NomeCliente").Split(' ').FirstOrDefault().RemoveAcentos().ToUpper(),
                            memberLastName = bd.LerString("NomeCliente").Split(' ').LastOrDefault().RemoveAcentos().ToUpper(),
                            memberPostalCode = bd.LerString("CEPCliente").AsCEP(),
                            memberEmail = bd.LerString("EmailCliente").RemoveAcentos().ToUpper(),
                            memberPhone = bd.LerString("TelefoneCliente").RemoveAcentos().ToUpper(),
                            memberAddress = bd.LerString("EnderecoCliente").RemoveAcentos().ToUpper(),
                            memberCity = bd.LerString("CidadeCliente").RemoveAcentos().ToUpper(),
                            memberRegion = bd.LerString("EstadoCliente").RemoveAcentos().ToUpper(),
                            memberCountry = "BR",
                            registeredMember = "Y",
                            guest = "N",
                        },
                        shippingInformation = new shippingInformation()
                        {
                            shippingFullName = bd.LerString("Nome").ToUpper(),
                            shippingCPF = bd.LerString("CPF"),
                            shippingFirstName = bd.LerString("Nome").Split(' ').FirstOrDefault().RemoveAcentos().ToUpper(),
                            shippingLastName = bd.LerString("Nome").Split(' ').LastOrDefault().RemoveAcentos().ToUpper(),
                            shippingAddress = bd.LerString("Endereco").RemoveAcentos().ToUpper(),
                            shippingCity = bd.LerString("Cidade").RemoveAcentos().ToUpper(),
                            shippingRegion = bd.LerString("Estado").RemoveAcentos().ToUpper(),
                            shippingPostalCode = bd.LerString("CEP").AsCEP(),
                            shippingCountry = bd.LerInt("EntregaID") == 0 ? string.Empty : "BR",
                            shippingMethod = bd.LerString("Entrega").RemoveAcentos(),
                            shippingDeadline = bd.LerInt("EntregaID") == 0 ? string.Empty : bd.LerDateTimeAccertify("DataEntrega")
                        }
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                bd.BulkInsert(ids, "#vb", false, true);

                bd.Consulta(@"
	                        SELECT
		                            #vb.ID, e.ID AS EventoID, e.Nome AS Evento, ap.Horario, l.Nome AS Local, l.Logradouro, l.Cidade, l.Estado, l.CEP, p.ID AS PrecoID, 
                                    p.Valor, COUNT(DISTINCT i.ID) AS Quantidade, et.Nome AS Categoria, es.Descricao AS Genero, vbi.TaxaConvenienciaValor
		                        FROM #vb (NOLOCK)
		                        INNER JOIN tIngressoLog il (NOLOCK) ON il.VendaBilheteriaID = #vb.ID
		                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = il.IngressoID
		                        INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
		                        INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
		                        INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID
		                        INNER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID
		                        LEFT JOIN tEventoSubtipo es (NOLOCK) ON es.ID = e.EventoSubTipoID
                                LEFT JOIN tEventoTipo et (NOLOCK) ON et.ID = es.EventoTipoID
		                        INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = il.VendaBilheteriaItemID
		                        WHERE il.Acao = 'V' AND vbi.PacoteID = 0
		                        GROUP BY #vb.ID, e.ID, e.Nome, ap.Horario, l.Nome, l.Logradouro, l.Cidade, l.Estado, l.CEP, p.ID, p.Valor, et.Nome, es.Descricao, vbi.TaxaConvenienciaValor
                                ORDER BY #vb.ID, p.ID");

                int vendaID = 0;
                var venda = new order();

                while (bd.Consulta().Read())
                {
                    if (vendaID != bd.LerInt("ID"))
                    {
                        vendaID = bd.LerInt("ID");
                        venda = transaction.Where(c => c.orderNumber.EndsWith(vendaID.ToString())).FirstOrDefault();
                        if (venda == null)
                            continue;
                    }
                    venda.eventDetails.Add(new eventinfo()
                    {
                        eventID = bd.LerInt("EventoID").ToString(),
                        eventDateTime = bd.LerDateTimeAccertify("Horario"),
                        eventLocation = bd.LerString("Local").RemoveAcentos().ToUpper(),
                        eventAddress = bd.LerString("Logradouro").RemoveAcentos().ToUpper(),
                        eventPostalCode = bd.LerString("CEP").AsCEP(),
                        eventCity = bd.LerString("Cidade").RemoveAcentos().ToUpper(),
                        eventState = bd.LerString("Estado").RemoveAcentos().ToUpper(),
                        eventCountry = "BR",
                        ticketPrice = bd.LerDecimal("Valor").ToString("#0.00"),
                        ticketQuantity = bd.LerInt("Quantidade").ToString(),
                        eventCategory = bd.LerString("Categoria").RemoveAcentos().ToUpper(),
                        eventGenre = bd.LerString("Genero").RemoveAcentos().ToUpper(),
                        eventTax = bd.LerDecimal("TaxaConvenienciaValor").ToString("#0.00"),
                        eventDescription = bd.LerString("Evento"),
                    });
                }

                XmlSerializer ser = new XmlSerializer(transaction.GetType());
                XmlDocument xml = new XmlDocument();

                using (MemoryStream fs = new MemoryStream())
                {
                    ser.Serialize(fs, transaction);
                    fs.Position = 0;
                    var sr = new StreamReader(fs);
                    string myStr = sr.ReadToEnd().Replace("eventinfo", "event").Replace("ArrayOfOrder", "transactions");

                    xml.LoadXml(myStr);
                }

                xml.Save(caminho);

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void GerarNegativeFiles(string caminho)
        {
            try
            {
                string sql = "SELECT ID, CPF, Email, DDDTelefone + Telefone AS Telefone FROM tCliente WHERE StatusAtual = 'B'";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Derp! There's no data!");

                StringBuilder stbID = new StringBuilder();
                StringBuilder stbCPF = new StringBuilder();
                StringBuilder stbEmail = new StringBuilder();
                StringBuilder stbTelefone = new StringBuilder();

                do
                {
                    stbID.Append(bd.LerInt("ID") + Environment.NewLine);

                    if (!string.IsNullOrEmpty(bd.LerString("CPF")))
                        stbCPF.Append(bd.LerString("CPF") + Environment.NewLine);

                    if (!string.IsNullOrEmpty(bd.LerString("Email")))
                        stbEmail.Append(bd.LerString("Email").RemoveAcentos().ToUpper().Trim() + Environment.NewLine);

                    if (!string.IsNullOrEmpty(bd.LerString("Telefone")))
                        stbTelefone.Append(bd.LerString("Telefone").RemoveAcentos().Trim() + Environment.NewLine);

                } while (bd.Consulta().Read());

                File.WriteAllText(caminho + "\\MemberID.txt", stbID.ToString());
                File.WriteAllText(caminho + "\\MemberCPF.txt", stbCPF.ToString());
                File.WriteAllText(caminho + "\\MemberEmail.txt", stbEmail.ToString());
                File.WriteAllText(caminho + "\\MemberTelefone.txt", stbTelefone.ToString());

            }
            finally
            {
                bd.Fechar();
            }
        }

        public static XmlDocument GerarXMLVenda(HammerHead.EstruturaVenda Venda)
        {
            BD bd = new BD();
            try
            {

                bd.Consulta(string.Format(@"
                       SELECT
	                        vb.Senha AS Senha, vb.ID AS VendaBilheteriaID, vb.Senha, vb.Status, vb.DataVenda, vb.ValorTotal, vb.TaxaConvenienciaValorTotal, vb.TaxaEntregaValor, vb.IP, cn.Nome AS Canal,
	                        c.ID AS ClienteID, c.CPF AS CPFCliente, c.DataCadastro, c.Nome AS NomeCliente, c.Email AS EmailCliente, c.DDDTelefone + c.Telefone AS TelefoneCliente, c.EnderecoCliente, c.CidadeCliente, c.EstadoCliente, c.CEPCliente, 'BR' AS PaisCliente,
	                        ce.Nome, ce.CPF, ce.Endereco, ce.Cidade, ce.Estado, ce.CEP, e.Nome AS Entrega, e.PrazoEntrega, e.ID AS EntregaID,
	                        CASE WHEN ea.ID IS NOT NULL
		                        THEN ea.Data + '235959'
		                        ELSE
			                        CONVERT(VARCHAR,DateAdd(Day, e.PrazoEntrega, CONVERT(DATETIME, SUBSTRING(vb.DataVenda, 0 ,9), 112)), 112) + '235959'
	                        END AS DataEntrega, VendaCancelada
	                        FROM tVendaBilheteria vb (NOLOCK)
	                        INNER JOIN tCaixa cx (NOLOCK) ON cx.ID = vb.CaixaID
	                        INNER JOIN tLoja l (NOLOCK) ON l.ID = cx.LojaID
	                        INNER JOIN tCanal cn (NOLOCK) ON cn.ID = l.CanalID
	                        INNER JOIN tCliente c (NOLOCK) ON vb.ClienteID = c.ID
	                        LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
	                        INNER JOIN tEntregaControle ec (NOLOCK) ON ec.ID = vb.EntregaControleID
	                        INNER JOIN tEntrega e (NOLOCK) ON e.ID = ec.EntregaID 
	                        LEFT JOIN tEntregaAgenda ea (NOLOCK) ON ea.ID = vb.EntregaAgendaID
	                        WHERE 
								vb.ID = {0}
                ", Venda.ID));

                if (!bd.Consulta().Read() || bd.LerBoolean("VendaCancelada"))
                    throw new VendaCanceladaException(Venda.ID);

                var transaction = new List<order>();

                string forcestatus = string.Empty;

                if (!string.IsNullOrEmpty(Venda.AccertifyForceStatus))
                    forcestatus = Venda.AccertifyForceStatus == ((char)ForceStatusType.APPROVE).ToString() ? ForceStatusType.APPROVE.ToString() : ForceStatusType.REVIEW.ToString();

                do
                {
                    transaction.Add(new order()
                    {
                        orderNumber = bd.LerString("Senha"),
                        orderType = "Sale",
                        orderDateTime = bd.LerDateTimeAccertify("DataVenda"),
                        totalAmount = bd.LerDecimal("ValorTotal").ToString("#0.00"),
                        totalSalesTax = bd.LerDecimal("TaxaConvenienciaValorTotal").ToString("#0.00"),
                        totalShippingCharges = bd.LerDecimal("TaxaEntregaValor").ToString("#0.00"),
                        ipAddress = bd.LerString("IP"),
                        salesChannel = bd.LerString("Canal").RemoveAcentos().ToUpper(),
                        ForceStatus = forcestatus,

                        paymentInformation = new paymentInformation()
                        {
                            billingFullName = Venda.FormaPagamento.NomeCartao,
                            billingFirstName = Venda.FormaPagamento.NomeCartao.Split(' ').FirstOrDefault(),
                            billingLastName = Venda.FormaPagamento.NomeCartao.Split(' ').LastOrDefault(),
                            cardNumber = Venda.Cartao.NumeroCartao,
                            cardExpireDate = !string.IsNullOrEmpty(Venda.Cartao.DataVencimento) ? Venda.Cartao.DataVencimento.Substring(2, 2) + "/" + Venda.Cartao.DataVencimento.Substring(0, 2) : string.Empty,
                            cardAuthorizationCode = "approved", //Venda.Cartao.CodigoSeguranca,
                            paymentType = Venda.FormaPagamento.Bandeira,
                            cardType = Venda.FormaPagamento.Bandeira,
                        },
                        failedCCInformation = new failedCCInformation() { },
                        memberInformation = new memberInformation()
                        {
                            memberID = bd.LerInt("ClienteID").ToString(),
                            memberCPF = bd.LerString("CPFCliente").ToString(),
                            membershipDate = bd.LerDateTimeAccertify("DataCadastro"),
                            memberFullName = bd.LerString("NomeCliente").RemoveAcentos().ToUpper(),
                            memberFirstName = bd.LerString("NomeCliente").Split(' ').FirstOrDefault().RemoveAcentos().ToUpper(),
                            memberLastName = bd.LerString("NomeCliente").Split(' ').LastOrDefault().RemoveAcentos().ToUpper(),
                            memberPostalCode = bd.LerString("CEPCliente").AsCEP(),
                            memberEmail = bd.LerString("EmailCliente").RemoveAcentos().ToUpper(),
                            memberPhone = bd.LerString("TelefoneCliente").RemoveAcentos().ToUpper(),
                            memberAddress = bd.LerString("EnderecoCliente").RemoveAcentos().ToUpper(),
                            memberCity = bd.LerString("CidadeCliente").RemoveAcentos().ToUpper(),
                            memberRegion = bd.LerString("EstadoCliente").RemoveAcentos().ToUpper(),
                            memberCountry = "BR",
                            registeredMember = "Y",
                            guest = "N",
                        },
                        shippingInformation = new shippingInformation()
                        {
                            shippingFullName = bd.LerString("Nome").ToUpper(),
                            shippingCPF = bd.LerString("CPF"),
                            shippingFirstName = bd.LerString("Nome").Split(' ').FirstOrDefault().RemoveAcentos().ToUpper(),
                            shippingLastName = bd.LerString("Nome").Split(' ').LastOrDefault().RemoveAcentos().ToUpper(),
                            shippingAddress = bd.LerString("Endereco").RemoveAcentos().ToUpper(),
                            shippingCity = bd.LerString("Cidade").RemoveAcentos().ToUpper(),
                            shippingRegion = bd.LerString("Estado").RemoveAcentos().ToUpper(),
                            shippingPostalCode = bd.LerString("CEP").AsCEP(),
                            shippingCountry = bd.LerString("CEP").Length == 0 ? string.Empty : "BR",
                            shippingMethod = bd.LerString("Entrega").RemoveAcentos(),
                            shippingDeadline = bd.LerString("CEP").Length == 0 ? string.Empty : bd.LerDateTimeAccertify("DataEntrega")
                        }
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                bd.Consulta(string.Format(@"
	                        SELECT
		                            vb.ID, e.ID AS EventoID, e.Nome AS Evento, ap.Horario, l.Nome AS Local, l.Logradouro, l.Cidade, l.Estado, l.CEP, p.ID AS PrecoID, 
                                    p.Valor, COUNT(DISTINCT i.ID) AS Quantidade, et.Nome AS Categoria, es.Descricao AS Genero, vbi.TaxaConvenienciaValor, i.SessionID
		                        FROM tVendaBilheteria vb (NOLOCK)
		                        INNER JOIN tIngressoLog il (NOLOCK) ON il.VendaBilheteriaID = vb.ID
		                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = il.IngressoID
		                        INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
		                        INNER JOIN tApresentacao ap (NOLOCK) ON ap.ID = i.ApresentacaoID
		                        INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID
		                        INNER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID
		                        LEFT JOIN tEventoSubtipo es (NOLOCK) ON es.ID = e.EventoSubTipoID
                                LEFT JOIN tEventoTipo et (NOLOCK) ON et.ID = es.EventoTipoID
		                        INNER JOIN tVendaBilheteriaItem vbi (NOLOCK) ON vbi.ID = il.VendaBilheteriaItemID
		                            WHERE vb.ID = {0} AND i.Status IN('{1}', '{2}', '{3}') AND il.Acao = 'V' 
		                        GROUP BY vb.ID, e.ID, e.Nome, ap.Horario, l.Nome, l.Logradouro, l.Cidade, l.Estado, l.CEP, p.ID, p.Valor, et.Nome, es.Descricao, vbi.TaxaConvenienciaValor, i.SessionID
                                ORDER BY vb.ID, p.ID", Venda.ID, Ingresso.VENDIDO, Ingresso.ENTREGUE, Ingresso.IMPRESSO));

                int vendaID = 0;
                var venda = new order();

                while (bd.Consulta().Read())
                {
                    if (vendaID != bd.LerInt("ID"))
                    {
                        vendaID = bd.LerInt("ID");
                        venda = transaction.Where(c => c.orderNumber.EndsWith(vendaID.ToString())).FirstOrDefault();
                        if (venda == null)
                            continue;
                    }

                    venda.eventDetails.Add(new eventinfo()
                    {
                        eventID = bd.LerInt("EventoID").ToString(),
                        eventDateTime = bd.LerDateTimeAccertify("Horario"),
                        eventLocation = bd.LerString("Local").RemoveAcentos().ToUpper(),
                        eventAddress = bd.LerString("Logradouro").RemoveAcentos().ToUpper(),
                        eventPostalCode = bd.LerString("CEP").AsCEP(),
                        eventCity = bd.LerString("Cidade").RemoveAcentos().ToUpper(),
                        eventState = bd.LerString("Estado").RemoveAcentos().ToUpper(),
                        eventCountry = "BR",
                        ticketPrice = bd.LerDecimal("Valor").ToString("#0.00"),
                        ticketQuantity = bd.LerInt("Quantidade").ToString(),
                        eventCategory = bd.LerString("Categoria").RemoveAcentos().ToUpper(),
                        eventGenre = bd.LerString("Genero").RemoveAcentos().ToUpper(),
                        eventTax = bd.LerDecimal("TaxaConvenienciaValor").ToString("#0.00"),
                        eventDescription = bd.LerString("Evento"),
                    });


                    venda.SessionID = bd.LerString("SessionID");

                }

                XmlSerializer ser = new XmlSerializer(transaction.GetType());
                XmlDocument xml = new XmlDocument();

                using (MemoryStream fs = new MemoryStream())
                {
                    ser.Serialize(fs, transaction);
                    fs.Position = 0;
                    var sr = new StreamReader(fs);
                    string myStr = sr.ReadToEnd().Replace("eventinfo", "event").Replace("ArrayOfOrder", "transactions");

                    xml.LoadXml(myStr);
                }

                return xml;

            }
            finally
            {
                bd.Fechar();
            }
        }

    }

    public class order
    {
        public order() { this.eventDetails = new List<eventinfo>(); }
        public string orderNumber { get; set; }
        public string orderType { get; set; }
        public string orderDateTime { get; set; }
        public string totalAmount { get; set; }
        public string totalShippingCharges { get; set; }
        public string totalSalesTax { get; set; }
        public string salesChannel { get; set; }
        public string ipAddress { get; set; }
        public string ForceStatus { get; set; }
        public string browserCookie { get; set; }
        public paymentInformation paymentInformation { get; set; }
        public failedCCInformation failedCCInformation { get; set; }
        public memberInformation memberInformation { get; set; }
        public List<eventinfo> eventDetails { get; set; }
        public shippingInformation shippingInformation { get; set; }

        public string SessionID { get; set; }
    }

    public class paymentInformation
    {
        public string paymentType { get; set; }
        public string cardNumber { get; set; }
        public string cardType { get; set; }
        public string cardAuthorizedAmount { get; set; }
        public string cardAuthorizationCode { get; set; }
        public string cardExpireDate { get; set; }
        public string billingFullName { get; set; }
        public string billingFirstName { get; set; }
        public string billingLastName { get; set; }
        public string payPalRequestID { get; set; }
        public string payPalEmail { get; set; }
        public string payPalAmount { get; set; }
        public string payPalStatus { get; set; }
        public List<coupon> coupons { get; set; }
    }

    public class coupon
    {
        public string couponCode { get; set; }
    }

    public class failedCCInformation
    {
        public string totalFailedCCAttempts { get; set; }
        public string failedCardNumber { get; set; }
        public string failedCardType { get; set; }
        public string failedCardExpireDate { get; set; }
        public string failedAuthRejectCode { get; set; }
        public string failedbillingFullName { get; set; }
        public string failedbillingFirstName { get; set; }
        public string failedbillingLastName { get; set; }
    }

    public class memberInformation
    {
        public string memberID { get; set; }
        public string memberCPF { get; set; }
        public string membershipDate { get; set; }
        public string memberFullName { get; set; }
        public string memberFirstName { get; set; }
        public string memberLastName { get; set; }
        public string memberEmail { get; set; }
        public string memberPhone { get; set; }
        public string memberAddress { get; set; }
        public string memberCity { get; set; }
        public string memberRegion { get; set; }
        public string memberPostalCode { get; set; }
        public string memberCountry { get; set; }
        public string memberPassword { get; set; }
        public string registeredMember { get; set; }
        public string guest { get; set; }



    }

    public class eventinfo
    {
        public string eventID { get; set; }
        public string eventDateTime { get; set; }
        public string eventDescription { get; set; }
        public string eventLocation { get; set; }
        public string eventAddress { get; set; }
        public string eventCity { get; set; }
        public string eventState { get; set; }
        public string eventPostalCode { get; set; }
        public string eventCountry { get; set; }
        public string ticketPrice { get; set; }
        public string ticketQuantity { get; set; }
        public string eventCategory { get; set; }
        public string eventGenre { get; set; }
        public string eventTax { get; set; }


    }

    public class shippingInformation
    {
        public string shippingFullName { get; set; }
        public string shippingFirstName { get; set; }
        public string shippingLastName { get; set; }
        public string shippingCPF { get; set; }
        public string shippingAddress { get; set; }
        public string shippingCity { get; set; }
        public string shippingRegion { get; set; }
        public string shippingPostalCode { get; set; }
        public string shippingCountry { get; set; }
        public string shippingMethod { get; set; }
        public string shippingDeadline { get; set; }
    }

    public enum ForceStatusType
    {
        APPROVE = 'A',
        REVIEW = 'R'
    }
}
