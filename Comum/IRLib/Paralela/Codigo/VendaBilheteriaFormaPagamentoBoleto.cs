/**************************************************
* Arquivo: VendaBilheteriaFormaPagamentoBoleto.cs
* Gerado: 19/10/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.Assinaturas.Models;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{

    public class VendaBilheteriaFormaPagamentoBoleto : VendaBilheteriaFormaPagamentoBoleto_B
    {
        public VendaBilheteriaFormaPagamentoBoleto() { }

        public enum EnumStatusBoleto
        {
            [Description("Ambos")]
            Ambos = 'A',
            [Description("Pago")]
            Pago = 'P',
            [Description("Aguardando Pagamento")]
            AguardandoPagamento = 'G',
        }

        public List<int> GerarBoletos(BD bd, int parcela, decimal valorTotal, int vendaBilheteriaFormaPagamentoID)
        {
            DateTime vencimentoBoleto = DateTime.Now.AddMonths(-1).AddDays(IRLib.Paralela.Boleto.Instancia.Info.DiasPrimeiroVencimento.Valor);
            decimal valorBoleto = valorTotal / parcela;
            decimal valorAux = 0;
            List<int> lstRetorno = new List<int>();
            this.Limpar();

            for (int i = 1; i <= parcela; i++)
            {
                if (i == parcela)
                    valorBoleto = valorTotal - valorAux;

                valorAux += valorBoleto;
                this.Valor.Valor = valorBoleto;
                this.Parcela.Valor = i;
                this.TimeStamp.Valor = DateTime.Now;
                this.DataVencimento.Valor = vencimentoBoleto.AddMonths(i);
                this.VendaBilheteriaFormaPagamentoID.Valor = vendaBilheteriaFormaPagamentoID;
                this.Inserir(bd);
                lstRetorno.Add(this.Control.ID);
                this.Limpar();
            }

            return lstRetorno;
        }

        public void EnviarBoletos(BD bd, int parcela, decimal valorTotal, int vendaBilheteriaFormaPagamentoID, List<AcaoProvisoria> listaAcoes, string email)
        {
            try
            {
                string linkBoletos = string.Empty;
                List<int> lstBoletosID = this.GerarBoletos(bd, parcela, valorTotal, vendaBilheteriaFormaPagamentoID);

                for (int i = 0; i < lstBoletosID.Count; i++)
                    linkBoletos += "<a href=\"" + Boleto.Instancia.Info.LinkVisualizacao.Valor + lstBoletosID[i] + "\">Boleto " + i + "</a><br/>";

                string assinaturasTabela = @"<table style='width: 540px; border-collapse: collapse; border-color: #cccccc;' border='1'>
                                        <tr>
                                            <td><b>Assinatura</b></td>
                                            <td><b>Setor</b></td>
                                            <td><b>Lugar</b></td>
                                            <td><b>Valor</b></td>
                                        </tr>";
                foreach (AcaoProvisoria item in listaAcoes)
                    assinaturasTabela += @"<tr>
                                            <td>" + item.Assinatura + @"</td>
                                            <td>" + item.Setor + @"</td>
                                            <td>" + item.Lugar + @"</td>
                                            <td>" + item.Valor + @"</td>
                                           </tr>";
                assinaturasTabela += "</table>";

                if (Convert.ToBoolean(IRLib.Paralela.Boleto.Instancia.Info.BoletoGerenciado.Valor))
                    if (!string.IsNullOrEmpty(email))
                        ServicoEmailParalela.EnviarBoletos(email, linkBoletos, assinaturasTabela);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao enviar o email.");
            }
        }

        public List<Assinaturas.Models.Boleto> ListarBoleto(int clienteID, string filtro, int assinaturaTipoID)
        {
            try
            {
                string sql =
                    string.Format(@"
                            SELECT 
                                DISTINCT vb.DataVenda, vbfpb.Parcela, vbfpb.* 
		                    FROM  tAssinaturaCliente ac (nolock)
                            INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
		                    INNER JOIN tVendaBilheteria vb  (NOLOCK) ON ac.VendaBilheteriaID = vb.ID
                            INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
                            INNER JOIN tVendaBilheteriaFormaPagamento vbfp (nolock) on vbfp.VendaBilheteriaID = vb.ID
                            INNER JOIN tVendaBilheteriaFormaPagamentoBoleto vbfpb(nolock)on vbfpb.VendaBilheteriaFormaPagamentoID = vbfp.ID
                            WHERE vb.ClienteID = {0} {1} AND a.AssinaturaTipoID = {2}
                            ORDER BY vb.DataVenda, vbfpb.Parcela
                            ", clienteID, filtro, assinaturaTipoID);

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return null;

                var lista = new List<Assinaturas.Models.Boleto>();

                do
                {
                    lista.Add(new Assinaturas.Models.Boleto()
                    {
                        ID = bd.LerInt("ID"),
                        VendaBilheteriaFormaPagamentoID = bd.LerInt("VendaBilheteriaFormaPagamentoID"),
                        Valor = bd.LerDecimal("Valor"),
                        Parcela = bd.LerInt("Parcela"),
                        TimeStamp = bd.LerDateTime("TimeStamp"),
                        ValorPago = bd.LerInt("ValorPago"),
                        DataPagamento = bd.LerDateTime("DataPagamento"),
                        DataVencimento = bd.LerDateTime("DataVencimento"),
                        Impresso = bd.LerBoolean("Impresso"),
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void MarcarImpresso()
        {
            this.Impresso.Valor = true;
            this.Atualizar();
        }

        public Dictionary<string, string> ListaStatus()
        {
            var retorno = Utils.Enums.EnumToDictionary<FormaPagamento.ESTADO>("S", "Selecione ....");
            return retorno;
        }

        public List<EstruturaBoleto> ListarBoleto(int clienteID, FormaPagamento.ESTADO? estadoBoleto, string senhaVenda, int codigoBoleto, int AssinaturaID, int AssinaturaTipoID, string AssinaturaAno)
        {
            try
            {
                string filtro = " ass.AssinaturaTipoID = " + AssinaturaTipoID + " AND Ano = " + AssinaturaAno;
                filtro += AssinaturaID > 0 ? " AND ass.ID = " + AssinaturaID : "";
                filtro += clienteID > 0 ? " AND  ac.ClienteID = " + clienteID : "";
                filtro += senhaVenda != null && senhaVenda.Length > 0 ? " AND v.Senha = '" + senhaVenda + "' " : "";
                filtro += codigoBoleto > 0 ? " AND b.ID = " + codigoBoleto : "";

                string sql =
                    string.Format(@"SELECT DISTINCT b.ID, 
                                    b.Valor, 
                                    b.Impresso,	
                                    b.ValorPago, 
                                    b.TimeStamp as DataEmissao, 
                                    b.DataPagamento, 
                                    b.DataVencimento, 
                                    b.DataConfirmacao, 
                                    c.CPF , 
                                    c.Nome as Assinante, 
                                    c.ID AS ClienteID,
                                    v.Senha as SenhaVenda, 
                                    ass.Nome as AssinaturaNome 
                                    FROM tVendaBilheteria v(nolock)
                                    INNER JOIN tVendaBilheteriaFormaPagamento p(NOLOCK) ON p.VendaBilheteriaID = v.ID
                                    INNER JOIN tVendaBilheteriaFormaPagamentoBoleto b(NOLOCK)ON b.VendaBilheteriaFormaPagamentoID = p.ID
                                    INNER JOIN tAssinaturaCliente ac(NOLOCK) ON ac.VendaBilheteriaID = v.ID
                                    INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = ac.AssinaturaID
                                    INNER JOIN tAssinaturaAno aa (NOLOCK) ON aa.AssinaturaID = ass.ID
                                    INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
                                    WHERE {0}", filtro);

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return null;

                var lista = new List<EstruturaBoleto>();
                FormaPagamento.ESTADO auxEstado = new FormaPagamento.ESTADO();
                do
                {

                    auxEstado = FormaPagamento.ESTADO.Aberto;

                    if (bd.LerDateTime("DataVencimento").Date < DateTime.Now.Date)
                    {
                        auxEstado = FormaPagamento.ESTADO.Vencido;
                    }

                    if (bd.LerDateTime("DataPagamento") > DateTime.MinValue)
                    {
                        auxEstado = FormaPagamento.ESTADO.Pago;
                    }


                    lista.Add(new EstruturaBoleto()
                    {
                        ID = bd.LerInt("ID"),
                        Valor = bd.LerDecimal("Valor"),
                        Impresso = bd.LerBoolean("Impresso"),
                        ValorPago = bd.LerDecimal("ValorPago"),
                        DataEmissao = bd.LerDateTime("DataEmissao"),
                        DataPagamento = bd.LerDateTime("DataPagamento"),
                        DataVencimento = bd.LerDateTime("DataVencimento"),
                        DataConfirmacao = bd.LerDateTime("DataConfirmacao"),
                        CPF = bd.LerString("CPF"),
                        Assinante = bd.LerString("Assinante"),
                        ClienteID = bd.LerInt("ClienteID"),
                        SenhaVenda = bd.LerString("SenhaVenda"),
                        AssinaturaNome = bd.LerString("AssinaturaNome"),
                        Cancelado = false,
                        EstadoPagamento = auxEstado,

                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();


                string sql2 =
                    string.Format(@"SELECT DISTINCT b.ID, 
                                    b.Valor,				
                                    b.Impresso,			
                                    b.ValorPago,			
                                    b.TimeStamp as DataEmissao,			
                                    b.DataPagamento,		
                                    b.DataVencimento,		
                                    b.DataConfirmacao,		
                                    c.CPF ,				
                                    c.Nome as Assinante,	
                                    c.ID AS ClienteID,		
                                    v.Senha as SenhaVenda,			
                                    ass.Nome as AssinaturaNome
                                    FROM tVendaBilheteria v(nolock)
                                    INNER JOIN tVendaBilheteriaFormaPagamento p(NOLOCK) ON p.VendaBilheteriaID = v.ID
                                    INNER JOIN tVendaBilheteriaFormaPagamentoBoletoHistorico b(NOLOCK)ON b.VendaBilheteriaFormaPagamentoID = p.ID
                                    INNER JOIN tAssinaturaCliente ac(NOLOCK) ON ac.VendaBilheteriaID = v.ID
                                    INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = ac.AssinaturaID
                                    INNER JOIN tAssinaturaAno aa (NOLOCK) ON aa.AssinaturaID = ass.ID
                                    INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
                                    WHERE {0}", filtro);

                bd.Consulta(sql2);

                if (bd.Consulta().Read())
                    do
                    {
                        auxEstado = FormaPagamento.ESTADO.Aberto;

                        if (bd.LerDateTime("DataVencimento").Date < DateTime.Now.Date)
                        {
                            auxEstado = FormaPagamento.ESTADO.Vencido;
                        }

                        if (bd.LerDateTime("DataPagamento") > DateTime.MinValue)
                        {
                            auxEstado = FormaPagamento.ESTADO.Pago;
                        }

                        lista.Add(new EstruturaBoleto()
                        {
                            ID = bd.LerInt("ID"),
                            Valor = bd.LerDecimal("Valor"),
                            Impresso = bd.LerBoolean("Impresso"),
                            ValorPago = bd.LerDecimal("ValorPago"),
                            DataEmissao = bd.LerDateTime("DataEmissao"),
                            DataPagamento = bd.LerDateTime("DataPagamento"),
                            DataVencimento = bd.LerDateTime("DataVencimento"),
                            DataConfirmacao = bd.LerDateTime("DataConfirmacao"),
                            CPF = bd.LerString("CPF"),
                            Assinante = bd.LerString("Assinante"),
                            ClienteID = bd.LerInt("ClienteID"),
                            SenhaVenda = bd.LerString("SenhaVenda"),
                            AssinaturaNome = bd.LerString("AssinaturaNome"),
                            Cancelado = true,
                            EstadoPagamento = auxEstado,
                        });
                    } while (bd.Consulta().Read());

                bd.FecharConsulta();

                if (estadoBoleto != null)
                {
                    return lista.Where(c => c.EstadoPagamento == estadoBoleto).ToList();
                }


                return lista;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void RegistraPagamento(EstruturaBoleto eEstruturaBoleto)
        {
            try
            {
                this.AtribuirEstrutura(eEstruturaBoleto);

                this.DataConfirmacao.Valor = DateTime.Now;
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tVendaBilheteriaFormaPagamentoBoleto SET  ValorPago = '@001', DataPagamento = '@002',  DataConfirmacao = '@003' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.ValorPago.ValorBD);
                sql.Replace("@002", this.DataPagamento.ValorBD);
                sql.Replace("@003", this.DataConfirmacao.ValorBD);

                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                if (!result)
                {

                    sql = new StringBuilder();
                    sql.Append("UPDATE tVendaBilheteriaFormaPagamentoBoletoHistorico SET  ValorPago = '@001', DataPagamento = '@002',  DataConfirmacao = '@003' ");
                    sql.Append("WHERE ID = @ID");
                    sql.Replace("@ID", this.Control.ID.ToString());
                    sql.Replace("@001", this.ValorPago.ValorBD);
                    sql.Replace("@002", this.DataPagamento.ValorBD);
                    sql.Replace("@003", this.DataConfirmacao.ValorBD);

                    x = bd.Executar(sql.ToString());

                    result = Convert.ToBoolean(x);
                    bd.Fechar();
                }


                this.VerificaPagamentoTotal();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                bd.Fechar();
            }
        }

        public void VerificaPagamentoTotal()
        {

            BD bdAux = new BD();
            try
            {
                string sql =
                    string.Format(@"SELECT vbba.* 
                                    FROM tVendaBilheteriaFormaPagamentoBoleto vbb(NOLOCK) 
                                    INNER JOIN tVendaBilheteriaFormaPagamentoBoleto vbba(NOLOCK) ON vbb.VendaBilheteriaFormaPagamentoID = vbba.VendaBilheteriaFormaPagamentoID
                                    WHERE vbb.ID = {0} and vbba.ValorPago <= 0.00 ", this.Control.ID);

                bdAux.Consulta(sql);
                if (bdAux.Consulta().Read())
                    return;

                bdAux.FecharConsulta();

                VendaBilheteria oVb = new VendaBilheteria();
                oVb.AtualizaPagamentoConcluido(this.Control.ID);

            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public void AtribuirEstrutura(EstruturaBoleto eEstruturaBoleto)
        {
            this.Control.ID = eEstruturaBoleto.ID;
            this.Impresso.Valor = eEstruturaBoleto.Impresso;
            this.Valor.Valor = eEstruturaBoleto.Valor;
            this.ValorPago.Valor = eEstruturaBoleto.ValorPago;
            this.DataConfirmacao.Valor = eEstruturaBoleto.DataConfirmacao;
            this.TimeStamp.Valor = eEstruturaBoleto.DataEmissao;
            this.DataPagamento.Valor = eEstruturaBoleto.DataPagamento;
            this.DataVencimento.Valor = eEstruturaBoleto.DataVencimento;



        }

        public void ReenviaBoleto(int clienteID, int BoletoID)
        {

            List<int> lstBoletosID = new List<int>();
            string caminho = Boleto.Instancia.Info.TemplateEmail.Valor;
            string linkBoletos = "";
            string assinaturasTabela = "";

            if (clienteID == 0)
                throw new Exception("Identificação incorreta, não foi possível encontrar o seu cadastro.");

            linkBoletos += "<a href=\"" + Boleto.Instancia.Info.LinkVisualizacao.Valor + BoletoID + "\">Boleto reemitido</a><br/><br/>";

            List<AcaoProvisoria> listaAcoes = new List<AcaoProvisoria>();
            listaAcoes = this.CarregarAssinaturasBoleto(BoletoID);

            assinaturasTabela += "<table width=\"600\" border=\"1\" cellspacing=\"0\" cellpadding=\"5\" bordercolor=\"#CCCCCC\">";
            assinaturasTabela += "<tr><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'><b>Assinatura</b></font></td><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'><b>Setor</b></font></td><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'><b>Lugar</b></font></td></tr>";
            foreach (AcaoProvisoria item in listaAcoes)
            {
                assinaturasTabela += "<tr><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'>" + item.Assinatura + "</font></td><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'>" + item.Setor + "</font></td><td><font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#666666'>" + item.Lugar + "</font></td></tr>";
            }
            assinaturasTabela += "</table>";

            Cliente oCliente = new Cliente();

            oCliente.Ler(clienteID);
            if (oCliente.Email.Valor.Length != 0)
                ServicoEmailParalela.EnviarBoletos(oCliente.Email.Valor, linkBoletos, assinaturasTabela);
        }

        public List<AcaoProvisoria> CarregarAssinaturasBoleto(int BoletoID)
        {
            BD bdAux = new BD();
            try
            {
                string sql =
                    string.Format(@"SELECT ass.Nome as Assinatura,s.Nome as Setor,l.Codigo as Lugar FROM tVendaBilheteriaFormaPagamentoBoleto b (NOLOCK)
                                    INNER JOIN tVendaBilheteriaFormaPagamento fp (NOLOCK) ON b.VendaBilheteriaFormaPagamentoID = fp.ID
                                    INNER JOIN tAssinaturaCliente ac (NOLOCK) ON fp.VendaBilheteriaID = ac.VendaBilheteriaID
                                    INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = ac.AssinaturaID
                                    INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
                                    INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
                                    WHERE b.ID = {0} ", BoletoID);

                bdAux.Consulta(sql);
                if (!bdAux.Consulta().Read())
                    return null;

                var lista = new List<AcaoProvisoria>();

                do
                {
                    lista.Add(new AcaoProvisoria()
                    {
                        Assinatura = bdAux.LerString("Assinatura"),
                        Setor = bdAux.LerString("Setor"),
                        Lugar = bdAux.LerString("Lugar"),

                    });
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();


                return lista;
            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public void RegistraPagamento(List<EstruturaBoleto> lstBoleto)
        {
            foreach (var boleto in lstBoleto)
            {
                this.RegistraPagamento(boleto);
            }
        }

        public void Reemitir()
        {
            try
            {
                bd.IniciarTransacao();
                this.InserirHistorico(bd);
                this.Excluir(bd, this.Control.ID);
                this.DataVencimento.Valor = DateTime.Now.Date.AddDays(IRLib.Paralela.Boleto.Instancia.Info.DiasPrimeiroVencimento.Valor);
                this.Inserir(bd);
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Reemitir(DateTime dataSelecionada, int ClienteID)
        {
            try
            {
                bd.IniciarTransacao();
                this.InserirHistorico(bd);
                this.Excluir(bd, this.Control.ID);
                this.DataVencimento.Valor = dataSelecionada.Date;
                this.Inserir(bd);

                this.ReenviaBoleto(ClienteID, this.Control.ID);

                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public bool InserirHistorico(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tVendaBilheteriaFormaPagamentoBoletoHistorico(ID,VendaBilheteriaFormaPagamentoID, Valor, Parcela, TimeStamp, ValorPago, DataPagamento, DataVencimento, DataConfirmacao, Impresso) ");
            sql.Append("VALUES (@ID,@001,'@002',@003,'@004','@005','@006','@007','@008','@009');");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.VendaBilheteriaFormaPagamentoID.ValorBD);
            sql.Replace("@002", this.Valor.ValorBD);
            sql.Replace("@003", this.Parcela.ValorBD);
            sql.Replace("@004", this.TimeStamp.ValorBD);
            sql.Replace("@005", this.ValorPago.ValorBD);
            sql.Replace("@006", this.DataPagamento.ValorBD);
            sql.Replace("@007", this.DataVencimento.ValorBD);
            sql.Replace("@008", this.DataConfirmacao.ValorBD);
            sql.Replace("@009", this.Impresso.ValorBD);

            int verfica = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return verfica > 0;
        }

        public bool VerificaBoleto(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT tFormaPagamento.Tipo 
                                    FROM tVendaBilheteriaFormaPagamento (NOLOCK)
                                    INNER JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.id
                                    WHERE VendaBilheteriaID = {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return false;

                return bd.LerInt("Tipo") == Convert.ToInt32(FormaPagamento.TIPO.Boleto);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<int> ProcessaCancelamentoBoleto(BD bd, int vendaBilheteriaID, decimal valorCancelado)
        {
            List<int> lstRetorno = new List<int>();
            VendaBilheteria oVendaBilheteria = new VendaBilheteria();
            oVendaBilheteria.LerFormaPagamento(vendaBilheteriaID);

            decimal valoresPagos = oVendaBilheteria.BoletosPagos(vendaBilheteriaID);
            decimal valorReal = oVendaBilheteria.ValorTotal.Valor;
            decimal valorPendente = this.VerficiaValorReal(vendaBilheteriaID);


            this.Cancelar(bd, vendaBilheteriaID, oVendaBilheteria.ClienteID.Valor, valoresPagos);

            if (valorPendente > 0)
                lstRetorno = this.ReCalcular(bd, valorReal, oVendaBilheteria.Parcelas(vendaBilheteriaID), oVendaBilheteria.FormasPagamentoID(vendaBilheteriaID), valorCancelado, oVendaBilheteria.ClienteID.Valor);

            return lstRetorno;

        }

        public decimal VerficiaValorReal(int vendaBilheteriaID)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT SUM(p.Valor) AS ValorReal FROM tIngresso i (NOLOCK)
                                    INNER JOIN tPreco p (NOLOCK) ON i.PrecoID = p.ID
                                    WHERE i.VendaBilheteriaID = {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return 0;

                return bd.LerDecimal("ValorReal");
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<int> ReCalcular(BD bd, decimal valorTotal, int parcela, int vendaBilheteriaFormaPagamentoID, decimal valorCancelamento, int clienteID)
        {
            List<int> lstBoletos = this.GerarBoletos(bd, parcela, (valorTotal - valorCancelamento), vendaBilheteriaFormaPagamentoID);
            return lstBoletos;
        }

        public void Cancelar(BD bd, int vendaBilheteriaID, int clienteID, decimal valoresPagos)
        {
            this.EnviarEmailEstorno(clienteID, valoresPagos);

            List<int> lstBoletos = this.ListaBoletosVenda(vendaBilheteriaID);
            foreach (var boletoID in lstBoletos)
            {
                this.Ler(boletoID);
                this.InserirHistorico(bd);
                this.Excluir(bd, this.Control.ID);
            }
        }

        public void EnviarEmailEstorno(int clienteID, decimal valoresPagos)
        {
            if (valoresPagos > 0)
            {
                Cliente oCliente = new Cliente();
                oCliente.Ler(clienteID);

                ServicoEmailParalela.EnviarEmailEstorno(oCliente.Email.Valor, oCliente.Nome.Valor, oCliente.CPF.Valor, valoresPagos);
            }
        }

        public List<int> ListaBoletosVenda(int vendaBilheteriaID)
        {
            try
            {
                List<int> lstRetorno = new List<int>();
                string sql =
                    string.Format(@"select tVendaBilheteriaFormaPagamentoBoleto.ID AS BoletoID 
                                    FROM  tVendaBilheteriaFormaPagamento (nolock)
                                    INNER JOIN tFormaPagamento (NOLOCK) ON tVendaBilheteriaFormaPagamento.FormaPagamentoID = tFormaPagamento.id
                                    INNER JOIN tVendaBilheteriaFormaPagamentoBoleto (NOLOCK) ON tVendaBilheteriaFormaPagamento.ID = tVendaBilheteriaFormaPagamentoBoleto.VendaBilheteriaFormaPagamentoID
                                    WHERE VendaBilheteriaID =  {0} ", vendaBilheteriaID);
                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Boletos não encontrados");

                do
                {
                    lstRetorno.Add(bd.LerInt("BoletoID"));
                } while (bd.Consulta().Read());

                return lstRetorno;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ReenviaBoleto(List<int> lstBoletos, int clienteID)
        {
            foreach (var boleto in lstBoletos)
            {
                ReenviaBoleto(clienteID, boleto);
            }
        }
    }

    public class VendaBilheteriaFormaPagamentoBoletoLista : VendaBilheteriaFormaPagamentoBoletoLista_B
    {

        public VendaBilheteriaFormaPagamentoBoletoLista() { }


    }

}
