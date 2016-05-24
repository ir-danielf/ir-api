using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaContrato
    {
        //TODO: Trocar todas propriedades por propriedades automaticas
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int empresaid;
        public int EmpresaID
        {
            get { return empresaid; }
            set { empresaid = value; }
        }

        private string empresanome;
        public string EmpresaNome
        {
            get { return empresanome; }
            set { empresanome = value; }
        }

        private int criadorid;
        public int CriadorID
        {
            get { return criadorid; }
            set { criadorid = value; }
        }

        private string criadornome;
        public string CriadorNome
        {
            get { return criadornome; }
            set { criadornome = value; }
        }
        
        private string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private DateTime datacriacao;
        public DateTime DataCriacao
        {
            get { return datacriacao; }
            set { datacriacao = value; }
        }

        private string observacoes;
        public string Observacoes
        {
            get { return observacoes; }
            set { observacoes = value; }
        }

        private Contrato.TipoDeRepasse tiporepasse;
        public Contrato.TipoDeRepasse TipoRepasse
        {
            get { return tiporepasse; }
            set { tiporepasse = value; }
        }

        private Contrato.TipoDeComissao tipocomissao;
        public Contrato.TipoDeComissao TipoComissao
        {
            get { return tipocomissao; }
            set { tipocomissao = value; }
        }

        private Contrato.TipoDePapelPagamento tipopapelpagamento;
        public Contrato.TipoDePapelPagamento TipoPapelPagamento
        {
            get { return tipopapelpagamento; }
            set { tipopapelpagamento = value; }
        }

        private bool papelcobrancautilizacao;
        public bool PapelCobrancaUtilizacao
        {
            get { return papelcobrancautilizacao; }
            set { papelcobrancautilizacao = value; }
        }

        private bool papelcomholografia;
        public bool PapelComHolografia
        {
            get { return papelcomholografia; }
            set { papelcomholografia = value; }
        }

        public int MaximoParcelas { get; set; }

        private List<int> contratocontasexcluir;
        public List<int> ContratoContasExcluir
        {
            get { return contratocontasexcluir; }
            set { contratocontasexcluir = value; }
        }

        private List<EstruturaContratoPapel> contratopapeis;
        public List<EstruturaContratoPapel> Papeis
        {
            get { return contratopapeis; }
            set { contratopapeis = value; }
        }

        private List<EstruturaContratoFormaPagamento> formapagamento;
        public List<EstruturaContratoFormaPagamento> FormaPagamento
        {
            get { return formapagamento; }
            set { formapagamento = value; }
        }

    }

    [Serializable]
    public class EstruturaEmpresaConta
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int empresaid;
        public int EmpresaID
        {
            get { return empresaid; }
            set { empresaid = value; }
        }

        private string empresanome;
        public string EmpresaNome
        {
            get { return empresanome; }
            set { empresanome = value; }
        }

        private string beneficiario;
        public string Beneficiario
        {
            get { return beneficiario; }
            set { beneficiario = value; }
        }

        private string banco;
        public string Banco
        {
            get { return banco; }
            set { banco = value; }
        }

        private string agencia;
        public string Agencia
        {
            get { return agencia; }
            set { agencia = value; }
        }

        private string conta;
        public string Conta
        {
            get { return conta; }
            set { conta = value; }
        }

        private string cpfcnpj;
        public string CPFCNPJ
        {
            get { return cpfcnpj; }
            set { cpfcnpj = value; }
        }

        private bool contapadrao;
        public bool ContaPadrao
        {
            get { return contapadrao; }
            set { contapadrao = value; }
        }
    }

    [Serializable]
    public class EstruturaContratoPapel
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int canaltipoid;
        public int CanalTipoID
        {
            get { return canaltipoid; }
            set { canaltipoid = value; }
        }

        private string canaltiponome;
        public string CanalTipoNome
        {
            get { return canaltiponome; }
            set { canaltiponome = value; }
        }
        
        private decimal ingressonormalvalor;
        public decimal IngressoNormalValor
        {
            get { return ingressonormalvalor; }
            set { ingressonormalvalor = value; }
        }

        private decimal preimpressovalor;
        public decimal PreImpressoValor
        {
            get { return preimpressovalor; }
            set { preimpressovalor = value; }
        }

        private decimal cortesiavalor;
        public decimal CortesiaValor
        {
            get { return cortesiavalor; }
            set { cortesiavalor = value; }
        }
    }
}
