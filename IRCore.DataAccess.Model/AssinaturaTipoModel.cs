using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class AssinaturaTipoModel
    {
        public int ID { get; set; }
        public int LocalID { get; set; }
        public int CanalAcessoID { get; set; }
        public string CanalAcessoNome { get; set; }
        public string Nome { get; set; }
        public string RenovacaoFim { get; set; }
        public string TrocaPrioritariaInicio { get; set; }
        public string TrocaPrioritariaFim { get; set; }
        public string TrocaInicio { get; set; }
        public string TrocaFim { get; set; }
        public string NovaAquisicaoInicio { get; set; }
        public string NovaAquisicaoFim { get; set; }
        public string RenovacaoInicio { get; set; }
        public string Programacao { get; set; }
        public string Precos { get; set; }
        public string Termos { get; set; }
        public string PaginaPrincipal { get; set; }
        public string PermiteAgregados { get; set; }
        public string Layout { get; set; }
        public string PaginaLogin { get; set; }
        public string PaginaRodape { get; set; }
        public string RetiradaBilheteria { get; set; }
        public string ValorEntregaFixo { get; set; }
        public string EntregaID { get; set; }
        public double ValorEntrega { get; set; }
        public string Logo { get; set; }
        public string AtivaBancoIngresso { get; set; }
        public string LocalNome { get; set; }
        public bool isPermiteAgregado { get; set; }
        public bool isAtivaBancoIngresso { get; set; }
        public int Ano { get; set; }
        public int TipoAno { get; set; }
    }

    public class AssinaturaTextoModel
    {
        public int ID { get; set; }
        public int assinaturaAnoID { get; set; }
        public int assinaturaFaseID { get; set; }
        public string paginaPrincipal { get; set; }
        public string paginaLogin { get; set; }
        public string paginaRodape { get; set; }
        public string termos { get; set; }
        public AssinaturaAnoModel AssinaturaAno { get; set; }
        public AssinaturaFaseModel AssinaturaFase { get; set; }
    }
    public class AssinaturaAnoModel
    {
        public int ID { get; set; }
        public int assinaturaID { get; set; }
        public int ano { get; set; }
        public string informacoes { get; set; }
    }

    public class AssinaturaFaseModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
    }

    public class AssinaturaModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string TipoCancelamento { get; set; }
        public int AssinaturaTipoID { get; set; }
        public string Ativo { get; set; }
        public int LocalID { get; set; }
        public int BloqueioID { get; set; }
        public int DesistenciaBloqueioID { get; set; }
        public int ExtintoBloqueioID { get; set; }
        public string Sigla { get; set; }
        public string AlertaAssinante { get; set; }
    }

    public class AssinaturaMigracaoLogModel
    {
        public int ID { get; set; }
        public int AssinaturaTipoIDOrigem { get; set; }
        public int AssinaturaAnoOrigem { get; set; }
        public int AssinaturaTipoIDNovo { get; set; }
        public int AssinaturaAnoNovo { get; set; }
        public int AssinaturaIDOrigem { get; set; }
        public int AssinaturaIDNovo { get; set; }
        public string DataMigracao { get; set; }
        public int UsuarioID { get; set; }
        public string AssinaturaTipoOrigem { get; set; }
        public string AssinaturaTipoNovo { get; set; }
        public string AssinaturaOrigemNome { get; set; }
        public string AssinaturaNovoNome { get; set; }
        public string UsuarioNome { get; set; }
    }
    public class AssinaturaMigracaoLogModelRel
    {
        public string DataMigracao { get; set; }
        public string AssinaturaTipoOrigem { get; set; }
        public string AssinaturaTipoNovo { get; set; }
        public int AssinaturaAnoOrigem { get; set; }
        public int AssinaturaAnoNovo { get; set; }
        public string AssinaturaOrigemNome { get; set; }
        public string AssinaturaNovoNome { get; set; }
        public string UsuarioNome { get; set; }
    }


    public class AssinaturaMapeamentoModel
    {
        public int AnteriorID { get; set; }
        public int NovoID { get; set; }
        public int NovoAnoID { get; set; }
    }

    public class AssinaturaMigracaoModel
    {
        public int AssinaturaTipoIDOrigem { get; set; }
        public int AssinaturaAnoIDOrigem { get; set; }
        public int AssinaturaTipoIDNovo { get; set; }
        public int AssinaturaAnoIDNovo { get; set; }
        public int AssinaturaIDOrigem { get; set; }
        public int AssinaturaIDNovo { get; set; }
    }

    public class AssinaturaClientesNaoMigradosRel
    {
        public string UsuarioNome { get; set; }
        public string AssinaturaTipoOrigem { get; set; }
        public string DataMigracao { get; set; }
        public string AssinaturaTipoNovo { get; set; }
        public int AssinaturaAnoOrigem { get; set; }
        public int AssinaturaAnoNovo { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteEmail { get; set; }
        public string ClienteTelefone { get; set; }
        public string AssinaturaOrigemNome { get; set; }
        public string ClienteLugar { get; set; }

    }

    public class AssinaturaConfigModel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int AssinaturaTipoID { get; set; }
        public string AssinaturaTipoNome { get; set; }
        public int AnoAtivoAssinatura { get; set; }
        public int AnoAtivoBancoIngressos { get; set; }
        public int AnoAtivo { get; set; }
    }

    public class ListaAssTipoPorNome_Result
    {
        public int ID { get; set; }
        public string Nome { get; set; }
    }

    public class ListaAssAnoPorAno_Result
    {
        public int Ano { get; set; }
    }
}