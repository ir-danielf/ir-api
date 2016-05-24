using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{
    public class IngressoGerenciadorAccertify
    {
        public BD bd { get; set; }

        public IngressoGerenciadorAccertify()
        {
            bd = new BD();
        }

        public List<IngressoImpressao> PesquisaVendaBilheteria(int vendaBilheteriaID)
        {
            try
            {
                string sql = string.Format(@"EXEC Proc_IngressosImpressaoETickect {0}", vendaBilheteriaID);

                List<IngressoImpressao> ingressos = new List<IngressoImpressao>();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    ingressos.Add(new IngressoImpressao()
                    {
                        ID = bd.LerInt("IngressoID"),
                        Senha = bd.LerString("Senha"),
                        DataVenda = (bd.LerDateTime("DataVenda")).ToString("dd/MM/yyyy"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = (bd.LerDateTime("Horario")).ToString("dddd, dd \\de MMMM \\de yyyy \\à\\s HH:mm"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        Valor = bd.LerDecimal("Valor"),
                        Preco = bd.LerString("Preco"),
                        Local = bd.LerString("Local"),
                        Acesso = bd.LerString("Acesso"),
                        EnderecoLocal = bd.LerString("EnderecoLocal"),
                        NumeroLocal = bd.LerString("NumeroLocal"),
                        BairroLocal = bd.LerString("BairroLocal"),
                        CepLocal = bd.LerString("CepLocal"),
                        CidadeLocal = bd.LerString("CidadeLocal"),
                        EstadoLocal = bd.LerString("EstadoLocal"),
                        EnderecoCliente = bd.LerString("EnderecoCliente"),
                        NumeroCliente = bd.LerString("NumeroCliente"),
                        BairroCliente = bd.LerString("BairroCliente"),
                        EstadoCliente = bd.LerString("EstadoCliente"),
                        CidadeCliente = bd.LerString("CidadeCliente"),
                        CepCliente = bd.LerString("CepCliente"),
                        Email = bd.LerString("Email"),
                        DDDTelefone = bd.LerString("DDDTelefone"),
                        Telefone = bd.LerString("Telefone"),
                        DDDCelular = bd.LerString("DDDCelular"),
                        Celular = bd.LerString("Celular"),
                        ClienteNome = bd.LerString("Cliente"),
                        ClienteCPFCNPJ = bd.LerString("ClienteCPF"),
                        FormaPagamento = bd.LerString("FormaPagamento"),
                        ValorConveniencia = bd.LerDecimal("ValorConveniencia"),
                        EventoID = bd.LerInt("EventoID"),
                        EntregaID = bd.LerInt("EntregaID"),
                        Canal = bd.LerString("Canal"),
                        Alvara = bd.LerString("Alvara"),
                        FonteImposto = bd.LerString("FonteImposto"),
                        AVCB = bd.LerString("AVCB"),
                        PorcentagemImposto = bd.LerDecimal("PorcentagemImposto")
                    });
                }

                foreach (var ingresso in ingressos)
                {
                    if (string.IsNullOrEmpty(ingresso.CodigoBarra))
                    {
                        List<EstruturaRetornoRegistroImpressao> estruturaimpressao = this.RegistrarImpressaoInternet(ingresso.ID);

                        foreach (var item in estruturaimpressao)
                            ingresso.CodigoBarra = item.CodigoBarra;
                    }
                }

                return ingressos;
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

        private List<EstruturaRetornoRegistroImpressao> RegistrarImpressaoInternet(int ingressoID)
        {
            try
            {
                //Cria o item de tipo anonimo
                var item = new { ID = 0, };

                //Forma a lista do tipo anonimo
                var items = AnonymousList.ToAnonymousList(item);

                items.Add(new { ID = ingressoID, });

                BilheteriaParalela bil = new BilheteriaParalela();

                return bil.RegistrarImpressao(items.Select(c => c.ID).ToArray(), IRLib.Paralela.Usuario.INTERNET_USUARIO_ID, BilheteriaParalela.INTERNET_EMPRESA_ID, bil.VerificaCaixaInternet(), Canal.CANAL_INTERNET, Loja.INTERNET_LOJA_ID, false, 1, null, IRLib.Paralela.Usuario.INTERNET_USUARIO_ID, false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class IngressoImpressao
    {
        public int ID { get; set; }
        public string Senha { get; set; }
        public string DataVenda { get; set; }
        public string Evento { get; set; }
        public string Apresentacao { get; set; }
        public string Setor { get; set; }
        public string Codigo { get; set; }
        public string CodigoBarra { get; set; }
        public decimal Valor { get; set; }
        public string Preco { get; set; }
        public string Local { get; set; }
        public string Acesso { get; set; }
        public string EnderecoLocal { get; set; }
        public string NumeroLocal { get; set; }
        public string BairroLocal { get; set; }
        public string CepLocal { get; set; }
        public string CidadeLocal { get; set; }
        public string EstadoLocal { get; set; }
        public string EnderecoCliente { get; set; }
        public string NumeroCliente { get; set; }
        public string ComplementoCliente { get; set; }
        public string BairroCliente { get; set; }
        public string EstadoCliente { get; set; }
        public string CidadeCliente { get; set; }
        public string CepCliente { get; set; }
        public string Email { get; set; }
        public string DDDTelefone { get; set; }
        public string Telefone { get; set; }
        public string DDDCelular { get; set; }
        public string Celular { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteCPFCNPJ { get; set; }
        public string FormaPagamento { get; set; }
        public decimal ValorConveniencia { get; set; }

        public string ValorTotal
        {
            get
            {
                return (this.Valor + this.ValorConveniencia).ToString("c");
            }
        }

        public int EventoID { get; set; }
        public int EntregaID { get; set; }

        public string Canal { get; set; }

        public string Alvara { get; set; }

        public string FonteImposto { get; set; }

        public string AVCB { get; set; }

        public decimal PorcentagemImposto { get; set; }

    }

    public static class AnonymousList
    {
        public static List<T> ToAnonymousList<T>(T ItemType)
        {
            return new List<T>();
        }
    }
}