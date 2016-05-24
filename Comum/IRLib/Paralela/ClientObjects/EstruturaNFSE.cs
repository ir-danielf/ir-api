using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaNFSE
    {
        public int EventoID { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Tipo { get; set; }
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string Razao { get; set; }
        public bool AtribuirCliente { get; set; }
        public bool IdentificarRPS { get; set; }
    }

    public class EstruturaGeral
    {
        public bool IdentificarRPS { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Tipo { get; set; }
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string Razao { get; set; }
        public int Id { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal Valor { get; set; }


        public EstruturaClienteNFSE Cliente { get; set; }
    }

    public class EstruturaClienteNFSE
    {
        public bool AtribuirCliente { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Uf { get; set; }
        public string CEP { get; set; }
        public string Email { get; set; }
    }

    public class ListaRPS : List<RPS>
    {
        public ListaRPS(string loteId, int numeroLote, string cnpj, string inscricaoMunicipal)
        {
            this.LoteId = loteId;
            this.NumeroLote = numeroLote.ToString();
            this.CNPJ = cnpj;
            this.InscricaoMunicipal = inscricaoMunicipal;
        }

        private XmlDocument XML { get; set; }
        public string LoteId { get; set; }
        public string NumeroLote { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoMunicipal { get; set; }

        public XmlDocument GerarXML()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(this.MontarBase());

            XmlNode ListaRPS = xml.GetElementsByTagName("ListaRps").Item(0);

            foreach (RPS rps in this)
            {
                XmlElement RPS = rps.MontarXML(xml);
                if (RPS.Attributes.Count > 0) { }

                ListaRPS.AppendChild(RPS);
            }
            return xml;

        }

        private string MontarBase()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("<EnviarLoteRpsEnvio xmlns=\"http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd\">");
            stb.Append("<LoteRps Id=\"" + this.LoteId + "\">");
            stb.Append("<NumeroLote>" + this.NumeroLote + "</NumeroLote>");
            stb.Append("<Cnpj>" + CNPJ + "</Cnpj>");
            stb.Append("<InscricaoMunicipal>" + InscricaoMunicipal + "</InscricaoMunicipal>");
            stb.Append("<QuantidadeRps>" + this.Count + "</QuantidadeRps>");
            stb.Append("<ListaRps></ListaRps>");
            stb.Append("</LoteRps>");
            stb.Append("</EnviarLoteRpsEnvio>");
            return stb.ToString();
        }
    }

    public class RPS : INFSE
    {
        public RPS(EstruturaGeral estruturaGeral)
        {
            this.InfRps = new InfRps(estruturaGeral);
        }
        public InfRps InfRps { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement RPS = XmlPai.CreateElement("Rps");
            RPS.RemoveAllAttributes();
            RPS.AppendChild(InfRps.MontarXML(XmlPai));
            return RPS;
        }
    }

    public class InfRps : INFSE
    {
        public InfRps(EstruturaGeral estruturaGeral)
        {
            this.Id = estruturaGeral.Id.ToString();
            this.Identificacao = new Identificacao(estruturaGeral);
            this.DataEmissao = estruturaGeral.DataEmissao;
            this.Servico = new Servico(estruturaGeral);
            this.Prestador = new Prestador(estruturaGeral);
            this.Tomardor = new Tomador(estruturaGeral);
            this.Cliente = estruturaGeral.Cliente;
            this.IdentificarRPS = estruturaGeral.IdentificarRPS;
        }
        public string Id { get; set; }
        public bool IdentificarRPS { get; set; }
        public Identificacao Identificacao { get; set; }
        public DateTime DataEmissao { get; set; }
        public string NaturezaOperacao { get { return "1"; } }
        public string OptanteSimplesNacional { get { return "2"; } }
        public string IncentivadorCultural { get { return "2"; } }
        public string Status { get { return "1"; } }
        public Servico Servico { get; set; }
        public Prestador Prestador { get; set; }
        public Tomador Tomardor { get; set; }

        public EstruturaClienteNFSE Cliente { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement InfRps = XmlPai.CreateElement("InfRps");

            XmlAttribute InfRpsAtt = XmlPai.CreateAttribute("Id");
            InfRpsAtt.Value = this.Id;
            InfRps.Attributes.Append(InfRpsAtt);

            //if (this.IdentificarRPS)
            InfRps.AppendChild(this.Identificacao.MontarXML(XmlPai));

            XmlElement dataEmissa = XmlPai.CreateElement("DataEmissao");
            //dataEmissa.InnerText = this.DataEmissao.ToShortDateString() + "T" + this.DataEmissao.ToShortTimeString();
            //dataEmissa.InnerText = this.DataEmissao.ToString("yyyy-MM-ddTHH:mm:ss");
            dataEmissa.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            InfRps.AppendChild(dataEmissa);

            XmlElement natureza = XmlPai.CreateElement("NaturezaOperacao");
            natureza.InnerText = this.NaturezaOperacao;
            InfRps.AppendChild(natureza);

            XmlElement OptanteSimplesNacional = XmlPai.CreateElement("OptanteSimplesNacional");
            OptanteSimplesNacional.InnerText = this.OptanteSimplesNacional;
            InfRps.AppendChild(OptanteSimplesNacional);

            XmlElement IncentivadorCultural = XmlPai.CreateElement("IncentivadorCultural");
            IncentivadorCultural.InnerText = this.IncentivadorCultural;
            InfRps.AppendChild(IncentivadorCultural);

            XmlElement Status = XmlPai.CreateElement("Status");
            Status.InnerText = this.Status;
            InfRps.AppendChild(Status);

            InfRps.AppendChild(this.Servico.MontarXML(XmlPai));
            InfRps.AppendChild(this.Prestador.MontarXML(XmlPai));
            if (Cliente.AtribuirCliente)
                InfRps.AppendChild(this.Tomardor.MontarXML(XmlPai));


            return InfRps;
        }
    }

    public class Identificacao : INFSE
    {
        public Identificacao(EstruturaGeral estruturaGeral)
        {
            this.Numero = estruturaGeral.Numero;
            this.Serie = estruturaGeral.Serie;
            this.Tipo = estruturaGeral.Tipo;
        }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Tipo { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {

            XmlElement identificacao = XmlPai.CreateElement("IdentificacaoRps");


            XmlElement numero = XmlPai.CreateElement("Numero");
            numero.InnerText = this.Numero; // IngressoID
            identificacao.AppendChild(numero);

            XmlElement serie = XmlPai.CreateElement("Serie");
            serie.InnerText = this.Serie;
            identificacao.AppendChild(serie);

            XmlElement tipo = XmlPai.CreateElement("Tipo");
            tipo.InnerText = this.Tipo;
            identificacao.AppendChild(tipo);

            return identificacao;
        }
    }

    public class Servico : INFSE
    {
        public Servico(EstruturaGeral estruturaGeral)
        {
            this.Valores = new Valores(estruturaGeral);
        }
        public Valores Valores { get; set; }
        public string ItemListaServico { get { return "1213"; } }
        public string CodigoTributacaoMunicipio { get { return "121301"; } }
        public string Discriminacao { get { return "Venda ingresso para evento Tênis Espetacular"; } }
        public string CodigoMunicipio { get { return "3304557"; } }


        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Servico = XmlPai.CreateElement("Servico");

            Servico.AppendChild(this.Valores.MontarXML(XmlPai));

            XmlElement ItemListaServico = XmlPai.CreateElement("ItemListaServico");
            ItemListaServico.InnerText = this.ItemListaServico;
            Servico.AppendChild(ItemListaServico);

            XmlElement CodigoTributacaoMunicipio = XmlPai.CreateElement("CodigoTributacaoMunicipio");
            CodigoTributacaoMunicipio.InnerText = this.CodigoTributacaoMunicipio;
            Servico.AppendChild(CodigoTributacaoMunicipio);

            XmlElement Discriminacao = XmlPai.CreateElement("Discriminacao");
            Discriminacao.InnerText = this.Discriminacao; ;
            Servico.AppendChild(Discriminacao);

            XmlElement CodigoMunicipio = XmlPai.CreateElement("CodigoMunicipio");
            CodigoMunicipio.InnerText = this.CodigoMunicipio;
            Servico.AppendChild(CodigoMunicipio);

            return Servico;

        }
    }

    public class Valores : INFSE
    {
        public Valores(EstruturaGeral estruturaGeral)
        {
            this.Valor = estruturaGeral.Valor;
        }
        public decimal Valor { get; set; }
        public string ValorServicos { get { return Valor.ToString("#.00"); } }
        public string ValorDecucoes { get { return "0"; } }
        public string ValorPIS
        {
            get
            {
                //return ((Valor * 1.65m) / 100).ToString("#.00");
                //return "0.0165";
                return "0.00";
            }
        } //Pis: 1,65%
        public string ValorCofins
        {
            get
            {
                //return ((Valor * 7.6m) / 100).ToString("#.00");
                //return "0.076";
                return "0.00";
            }
        } //Cofins: 7,6%
        public string ValorInss { get { return "0"; } }
        public string ValorIr { get { return "0.00"; } }
        public string ValorCsll { get { return "0.00"; } }
        public string IssRetido { get { return "2"; } }
        public string ValorIss { get { return "0.00"; } } //5%
        public string OutrasRetencoes { get { return "0.00"; } }
        public string Aliquota { get { return "0.05"; } }
        public string DescontoIncondicionado { get { return "0"; } }
        public string DescontoCondicionado { get { return "0"; } }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Valores = XmlPai.CreateElement("Valores");

            XmlElement ValorServicos = XmlPai.CreateElement("ValorServicos");
            ValorServicos.InnerText = this.ValorServicos.Replace(",", ".");
            Valores.AppendChild(ValorServicos);

            XmlElement ValorDeducoes = XmlPai.CreateElement("ValorDeducoes");
            ValorDeducoes.InnerText = this.ValorDecucoes;
            Valores.AppendChild(ValorDeducoes);

            XmlElement ValorPis = XmlPai.CreateElement("ValorPis");
            ValorPis.InnerText = this.ValorPIS; //1 - Pis: 1,65%
            Valores.AppendChild(ValorPis);

            XmlElement ValorCofins = XmlPai.CreateElement("ValorCofins");
            ValorCofins.InnerText = this.ValorCofins; // 2 - Cofins: 7,6%
            Valores.AppendChild(ValorCofins);

            XmlElement ValorInss = XmlPai.CreateElement("ValorInss");
            ValorInss.InnerText = this.ValorIss; //Zero
            Valores.AppendChild(ValorInss);

            XmlElement ValorIr = XmlPai.CreateElement("ValorIr");
            ValorIr.InnerText = this.ValorIr; //Zero
            Valores.AppendChild(ValorIr);

            XmlElement ValorCsll = XmlPai.CreateElement("ValorCsll");
            ValorCsll.InnerText = this.ValorCsll; //Zero
            Valores.AppendChild(ValorCsll);

            XmlElement IssRetido = XmlPai.CreateElement("IssRetido");
            IssRetido.InnerText = this.IssRetido;
            Valores.AppendChild(IssRetido);

            XmlElement ValorIss = XmlPai.CreateElement("ValorIss");
            ValorIss.InnerText = this.ValorIss; //5%
            Valores.AppendChild(ValorIss);

            XmlElement OutrasRetencoes = XmlPai.CreateElement("OutrasRetencoes");
            OutrasRetencoes.InnerText = this.OutrasRetencoes; //Faltou
            Valores.AppendChild(OutrasRetencoes);

            XmlElement Aliquota = XmlPai.CreateElement("Aliquota");
            Aliquota.InnerText = this.Aliquota; //Zero
            Valores.AppendChild(Aliquota);

            XmlElement DescontoIncondicionado = XmlPai.CreateElement("DescontoIncondicionado");
            DescontoIncondicionado.InnerText = this.DescontoIncondicionado; //Zero
            Valores.AppendChild(DescontoIncondicionado);

            XmlElement DescontoCondicionado = XmlPai.CreateElement("DescontoCondicionado");
            DescontoCondicionado.InnerText = this.DescontoCondicionado;
            Valores.AppendChild(DescontoCondicionado);

            return Valores;
        }
    }

    public class Prestador : INFSE
    {
        public Prestador(EstruturaGeral estruturaGeral)
        {
            this.CNPJ = estruturaGeral.CNPJ;
            this.IE = estruturaGeral.IE;
        }
        public string CNPJ { get; set; }
        public string IE { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Prestador = XmlPai.CreateElement("Prestador");

            XmlElement Cnpj = XmlPai.CreateElement("Cnpj");
            Cnpj.InnerText = this.CNPJ;
            Prestador.AppendChild(Cnpj);

            XmlElement InscricaoMunicipal = XmlPai.CreateElement("InscricaoMunicipal");
            InscricaoMunicipal.InnerText = this.IE;
            Prestador.AppendChild(InscricaoMunicipal);

            return Prestador;
        }
    }

    public class Tomador : INFSE
    {
        public Tomador(EstruturaGeral estruturaGeral)
        {
            this.IdentificacaoTomador = new IdentificacaoTomador(estruturaGeral);
            this.Endereco = new Endereco(estruturaGeral);
            this.Contato = new Contato(estruturaGeral);
        }
        public IdentificacaoTomador IdentificacaoTomador { get; set; }
        public string RazaoSocial { get; set; }
        public Endereco Endereco { get; set; }
        public Contato Contato { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Tomador = XmlPai.CreateElement("Tomador");
            Tomador.AppendChild(this.IdentificacaoTomador.MontarXML(XmlPai));

            XmlElement RazaoSocial = XmlPai.CreateElement("RazaoSocial");
            RazaoSocial.InnerText = this.RazaoSocial;
            Tomador.AppendChild(RazaoSocial);

            Tomador.AppendChild(this.Endereco.MontarXML(XmlPai));
            Tomador.AppendChild(this.Contato.MontarXML(XmlPai));

            return Tomador;
        }
    }

    public class IdentificacaoTomador : INFSE
    {
        public IdentificacaoTomador(EstruturaGeral estruturaGeral)
        {
            this.CpfCnpj = new CPFCNPJ(estruturaGeral);
        }
        public CPFCNPJ CpfCnpj { get; set; }

        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement IdentificacaoTomador = XmlPai.CreateElement("IdentificacaoTomador");

            IdentificacaoTomador.AppendChild(this.CpfCnpj.MontarXML(XmlPai));

            return IdentificacaoTomador;
        }

    }

    public class CPFCNPJ : INFSE
    {
        public CPFCNPJ(EstruturaGeral estruturaGeral)
        {
            this.CPF = estruturaGeral.Cliente.CPF;
        }
        public string CPF { get; set; }


        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement CpfCnpj = XmlPai.CreateElement("CpfCnpj");

            XmlElement Cpf = XmlPai.CreateElement("Cpf");
            Cpf.InnerText = this.CPF;
            CpfCnpj.AppendChild(Cpf);

            return CpfCnpj;
        }
    }

    public class Endereco : INFSE
    {
        public Endereco(EstruturaGeral estruturaGeral)
        {
            this.strEndereco = estruturaGeral.Cliente.Endereco;
            this.Numero = estruturaGeral.Cliente.Numero;
            this.Complemento = estruturaGeral.Cliente.Complemento;
            this.Bairro = estruturaGeral.Cliente.Bairro;
            this.Uf = estruturaGeral.Cliente.Uf;
            this.CEP = estruturaGeral.Cliente.CEP;
        }

        public string strEndereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Uf { get; set; }
        public string CEP { get; set; }


        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Endereco = XmlPai.CreateElement("Endereco");

            #region Endereco
            XmlElement Endereco1 = XmlPai.CreateElement("Endereco");
            Endereco1.InnerText = this.strEndereco;
            Endereco.AppendChild(Endereco1);

            XmlElement Numero = XmlPai.CreateElement("Numero");
            Numero.InnerText = this.Numero;
            Endereco.AppendChild(Numero);

            XmlElement Complemento = XmlPai.CreateElement("Complemento");
            Complemento.InnerText = this.Complemento;
            Endereco.AppendChild(Complemento);

            XmlElement Bairro = XmlPai.CreateElement("Bairro");
            Bairro.InnerText = this.Bairro;
            Endereco.AppendChild(Bairro);

            XmlElement Uf = XmlPai.CreateElement("Uf");
            Uf.InnerText = this.Uf;
            Endereco.AppendChild(Uf);

            XmlElement Cep = XmlPai.CreateElement("Cep");
            Cep.InnerText = this.CEP;
            Endereco.AppendChild(Cep);
            #endregion

            return Endereco;
        }
    }

    public class Contato : INFSE
    {
        public Contato(EstruturaGeral estruturaGeral)
        {
            this.Email = estruturaGeral.Cliente.Email;
        }
        public string Email { get; set; }


        public XmlElement MontarXML(XmlDocument XmlPai)
        {
            XmlElement Contato = XmlPai.CreateElement("Contato");

            XmlElement Email = XmlPai.CreateElement("Email");
            Email.InnerText = this.Email;
            Contato.AppendChild(Email);
            return Contato;
        }
    }

    public interface INFSE
    {
        XmlElement MontarXML(XmlDocument XmlPai);
    }

}
