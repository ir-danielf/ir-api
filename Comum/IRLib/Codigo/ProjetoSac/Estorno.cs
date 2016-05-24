using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IRLib
{
    public class Estorno : Estorno_B
    {
        public Estorno(int usuarioID)
            : base(usuarioID)
        {
            this.Status.Valor = EnumStatusEstorno.Pendente.ToString();
        }

        public Estorno()
        {

        }

        public bool Inserir(EstruturaEstorno estrutura, bool RequerAprovacao)
        {
            this.TipoEstorno.Valor = ((char)estrutura.TipoEstorno).ToString();
            this.ClienteID.Valor = estrutura.ClienteID;
            this.DataSolicitacao.Valor = DateTime.Now;
            this.MotivoID.Valor = estrutura.MotivoID;
            this.SenhaVenda.Valor = estrutura.SenhaVenda;
            this.ValorEstorno.Valor = estrutura.ValorEstorno;
            this.SenhaVenda.Valor = estrutura.SenhaVenda;

            bd.IniciarTransacao();
            this.Inserir(bd);

            try
            {
                if (estrutura.TipoEstorno == EnumTipoEstorno.Deposito)
                {
                    EstornoDeposito deposito = new EstornoDeposito();
                    deposito.BancoID.Valor = estrutura.Deposito.BancoID;
                    deposito.Conta.Valor = estrutura.Deposito.Conta;
                    deposito.Agencia.Valor = estrutura.Deposito.Agencia;
                    deposito.CPF.Valor = estrutura.Deposito.CPF;
                    deposito.DepositoContaCorrente.Valor = estrutura.Deposito.IsContaCorrente;
                    deposito.Nome.Valor = estrutura.Deposito.Correntista;
                    deposito.EstornoID.Valor = this.Control.ID;
                    deposito.Inserir(bd);
                }
                else
                {
                    EstornoCartao cartao = new EstornoCartao();

                    cartao.EstabeleciomentoID.Valor = estrutura.Cartao.Estabelecimento;
                    cartao.EstornoID.Valor = this.Control.ID;
                    cartao.Nome.Valor = estrutura.Cartao.TitularCartao;
                    cartao.NSU.Valor = estrutura.Cartao.NSU;
                    cartao.NumeroCartao.Valor = estrutura.Cartao.NumeroCartao;
                    cartao.RedeID.Valor = estrutura.Cartao.Rede;
                    cartao.Inserir(bd);
                }

                if (RequerAprovacao)
                {
                    EstornoAprovacao aprov = new EstornoAprovacao();
                    aprov.EstornoID.Valor = this.Control.ID;
                    aprov.Entrega.Valor = estrutura.ValorEntrega;
                    aprov.Inserir(bd);

                    foreach (KeyValuePair<int, decimal> ingresso in estrutura.IngressoID)
                    {
                        EstornoAprovacaoItem item = new EstornoAprovacaoItem();
                        item.IngressoID.Valor = ingresso.Key;
                        item.Conveniencia.Valor = ingresso.Value;
                        item.AprovacaoID.Valor = aprov.Control.ID;
                        item.Inserir(bd);
                    }
                }
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                return false;
            }
            return true;
        }

        public List<EstruturaEstorno> BuscarEstornosPendentes()
        {
            List<EstruturaEstorno> lstEstruturas = new List<EstruturaEstorno>();

            using (IDataReader reader = bd.Consulta(@"Select te.ID, te.SenhaVenda, te.SenhaCancelamento, te.ValorEstorno, te.TipoEstorno,
                            te.Status, te.DataSolicitacao, te.DataProcessamento, te.NumeroChamado, cli.Nome as Nome
                            from tEstorno te
                            left join tcliente cli on cli.ID = te.ClienteID
                            where te.status in ('X', 'S')"))
            {
                while (reader.Read())
                {
                    EstruturaEstorno est = new EstruturaEstorno();
                    est.EstornoID = Convert.ToInt32(reader["ID"]);
                    est.ValorEstorno = Convert.ToDecimal(reader["ValorEstorno"]);
                    est.TipoEstorno = (EnumTipoEstorno)Convert.ToChar(reader["TipoEstorno"]);
                    est.NomeCliente = Convert.ToString(reader["Nome"]);
                    est.StatusEstorno = (EnumStatusEstorno)Convert.ToChar(reader["Status"]);
                    est.DataSolicitacao = Utilitario.String2DateTime(Convert.ToString(reader["DataSolicitacao"]));
                    est.SenhaVenda = Convert.ToString(reader["SenhaVenda"]);
                    est.SenhaCancelamento = Convert.ToString(reader["SenhaVenda"]);
                    est.NumeroChamado = Convert.ToString(reader["NumeroChamado"]);
                    lstEstruturas.Add(est);
                }
            }
            return lstEstruturas;
        }

        public EstruturaEstorno BuscarEstornoCompleto(int id)
        {
            EstruturaEstorno est = new EstruturaEstorno();

            string query = @"select est.ID as 'EstornoID', est.SenhaCancelamento, est.ValorEstorno, est.TipoEstorno, est.Status, est.DataSolicitacao, est.ClienteID,
                            est.motivoID, est.numerochamado, apr.id as 'AprovacaoID' ,apr.Entrega as 'ValorEntrega'from testorno est
                            left join tEstornoAprovacao apr on apr.EstornoID = est.id   where est.id = {0}";

            using (IDataReader reader = bd.Consulta(string.Format(query, id)))
            {
                if (reader.Read())
                {
                    est.EstornoID = Convert.ToInt32(reader["ID"]);
                    est.ValorEstorno = Convert.ToDecimal(reader["ValorEstorno"]);
                    est.TipoEstorno = (EnumTipoEstorno)Convert.ToChar(reader["TipoEstorno"]);
                    est.NomeCliente = Convert.ToString(reader["Nome"]);
                    est.StatusEstorno = (EnumStatusEstorno)Convert.ToChar(reader["Status"]);
                    est.DataSolicitacao = Utilitario.String2DateTime(Convert.ToString(reader["DataSolicitacao"]));
                    est.SenhaVenda = Convert.ToString(reader["SenhaVenda"]);
                    est.SenhaCancelamento = Convert.ToString(reader["SenhaVenda"]);
                    est.NumeroChamado = Convert.ToString(reader["NumeroChamado"]);

                    reader.Close();
                }
                else
                    throw new Exception("Desculpe, mas este estorno não está mais disponível.");
            }

            return est;
        }
    }


    public class EstruturaEstorno
    {
        [Browsable(false)]
        public int EstornoID { get; set; }

        [Browsable(false)]
        public EstruturaEstornoCartao Cartao { get; set; }

        [Browsable(false)]
        public EstruturaEstornoDeposito Deposito { get; set; }

        [Browsable(false)]
        public EstruturaEstornoDinheiro Dinheiro { get; set; }

        [DisplayName("Tipo de estorno")]
        public EnumTipoEstorno TipoEstorno { get; set; }

        [DisplayName("Status")]
        public EnumStatusEstorno StatusEstorno { get; set; }

        [DisplayName("Nome do cliente")]
        public string NomeCliente { get; set; }

        [DisplayName("Email do cliente")]
        public string EmailCliente { get; set; }

        [Browsable(false)]
        public int ClienteID { get; set; }

        [DisplayName("Data da solicitação")]
        public DateTime DataSolicitacao { get; set; }

        [Browsable(false)]
        public int UsuarioID { get; set; }

        [Browsable(false)]
        public string SenhaVenda { get; set; }

        public string SenhaCancelamento { get; set; }


        public EstruturaEstorno()
        {
            IngressoID = new Dictionary<int, decimal>();
        }
        //id / taxa de conveniencia
        public Dictionary<int, decimal> IngressoID { get; set; }
        public EstornoEntregaStatus StatusEntrega { get; set; }
        public EstornoConvenienciaStatus StatusConveniencia { get; set; }
        public EstornoIngressoStatus StatusIngresso { get; set; }
        public EstornoPrazoDevolucao StatusPrazoDevolucao { get; set; }
        public decimal ValorEntrega { get; set; }
        public decimal ValorEntregaEstornado { get; set; }
        public decimal ValorConvenienciaTotal { get; set; }
        public decimal ValorConvenienciaEstornada { get; set; }
        public decimal ValorIngressosTotal { get; set; }
        public decimal ValorIngressosEstornado { get; set; }
        public decimal ValorCompraTotal { get; set; }
        public decimal ValorEstorno { get; set; }
        public decimal ValorSeguroPago { get; set; }
        public decimal ValorSeguroEstornado { get; set; }
        public string NumeroChamado { get; set; }

        public int MotivoID { get; set; }

    }

    public enum EnumStatusEstorno
    {
        Pendente = 'X',
        Solicitado = 'S',
        Processado = 'P'
    }

    public enum EnumTipoEstorno
    {
        [Description("SacPrazoEstornoDeposito")]
        Deposito = 'D',

        [Description("SacPrazoEstornoCartao")]
        Cartao = 'C',

        [Description("SacEstornoDinheiro")]
        Dinheiro = 'E',
    
        [Description("SacSemEstorno")]
        SemEstorno = 'N'
    }


    public enum EstornoEntregaStatus
    {
        [Description("SacEntregaInexistente")]
        EntregaInexistente,

        [Description("SacEntregaEfetuada")]
        EntregaJaEfetuada,

        [Description("SacEntregaNaoEfetuada")]
        EntregaNaoEfetuada,

        [Description("SacEntregaCancelada")]
        EntregaCancelada
    }


    public enum EstornoConvenienciaStatus
    {
        [Description("SacConvenienciaNaoCancelavel")]
        NaoCancelavel,

        [Description("SacConvenienciaCancelada")]
        Cancelada
    }

    public enum EstornoIngressoStatus
    {
        [Description("SacIngressoAindaNaoEntregue")]
        IngressoNaoSaiuEntrega,

        [Description("SacIngressoPosseCliente")]
        IngressoEmPosseCliente,

        [Description("SacIngressoAguardandoDevolucao")]
        IngressoAguardandoDevolucao,

        [Description("SacIngressoPosseIR")]
        IngressoEmPosseIR
    }

    public enum EstornoPrazoDevolucao
    {
        [Description("SacPrazoNormal")]
        DevolucaoPrazoNormal,

        [Description("SacPrazoPendencias")]
        DevolucaoPrazoPendencias
    }

    public class EstruturaEstornoDinheiro
    {
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Email de Notificação")]
        public string Email { get; set; }

        public EstruturaEstornoDinheiro()
        {
            Nome = string.Empty;
            Email = string.Empty;
        }
    }
    public class EstruturaEstornoDeposito
    {
        [DisplayName("Corrente")]
        public bool IsContaCorrente { get; set; }

        [DisplayName("Nome do Correntista")]
        public string Correntista { get; set; }
        public string CPF { get; set; }

        [Browsable(false)]
        public int BancoID { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }
        [DisplayName("Valor")]
        public decimal ValorEstorno { get; set; }

        [DisplayName("Email de Notificação")]
        public string Email { get; set; }

        public EstruturaEstornoDeposito()
        {
            IsContaCorrente = true;
            Conta = string.Empty;
            Agencia = string.Empty;
            CPF = string.Empty;
            Correntista = string.Empty;
            Email = string.Empty;
        }
    }

    public class EstruturaEstornoCartao
    {
        [DisplayName("Nome do titular")]
        public string TitularCartao { get; set; }

        public string Bandeira { get; set; }

        [DisplayName("Numero do cartão")]
        public string NumeroCartao { get; set; }

        [Browsable(false)]
        public int Estabelecimento { get; set; }

        [Browsable(false)]
        public string NSU { get; set; }

        [Browsable(false)]
        public int Rede { get; set; }

        [DisplayName("Email de Notificação")]
        public string Email { get; set; }

        public EstruturaEstornoCartao()
        {
            NSU = string.Empty;
            NumeroCartao = string.Empty;
            TitularCartao = string.Empty;
            Bandeira = string.Empty;
            Email = string.Empty;
        }
    }

    public class EstornoLista : EstornoLista_B
    {

    }
}
