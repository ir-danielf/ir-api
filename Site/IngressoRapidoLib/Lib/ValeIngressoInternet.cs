using CTLib;
using Newtonsoft.Json;
using System;

namespace IngressoRapido.Lib {
    public class ValeIngressoInternet {
        private BD bd = new BD();

        private ValeIngressoImpressaoInternet oValeIngressoImpressaoInternet { get; set; }

        public ValeIngressoInternet() {

        }

        public string GetByValeIngressoID(int ValeIngressoID, string NomePresenteado) {
            try {
                ValeIngressoImpressaoInternet retorno = new ValeIngressoImpressaoInternet();

                string sql = string.Format(@"SELECT tValeIngresso.DataExpiracao, tValeIngressoTipo.Valor,tValeIngresso.CodigoTroca, tValeIngresso.CodigoBarra,
                                        tValeIngresso.ClienteNome, tValeIngressoTipo.ProcedimentoTroca, tValeIngressoTipo.ValorTipo, tValeIngressoTipo.ValorPagamento,
                                        tValeIngressoTipo.SaudacaoNominal, tValeIngressoTipo.SaudacaoPadrao, tValeIngressoTipo.ID as Codigo
                                        FROM tValeIngresso (NOLOCK)
                                        INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngresso.ValeIngressoTipoID = tValeIngressoTipo.ID
                                        WHERE tValeIngresso.ID =  {0}", ValeIngressoID);

                bd.Consulta(sql);

                if(bd.Consulta().Read()) {
                    string nome = NomePresenteado;
                    string saudacao = string.Empty;

                    if(nome.Length > 0) {
                        saudacao = bd.LerString("SaudacaoNominal");
                        saudacao = saudacao.Replace("#cliente#", nome);
                    } else
                        saudacao = bd.LerString("SaudacaoPadrao");

                    int codigo = bd.LerInt("Codigo");

                    string nomeImagem = "ivir" + codigo.ToString("000000");

                    retorno.Senha = "0000000000";
                    retorno.DataValidade = DateTime.Now.ToString("dd/MM/yyyy");
                    retorno.ValorPagamento = bd.LerDecimal("ValorPagamento");
                    retorno.CodigoTroca = "Vale Ingresso";
                    retorno.ValorTipo = Convert.ToChar(bd.LerString("valorTipo"));
                    retorno.Valor = retorno.ValorTipo == (char)IRLib.ValeIngressoTipo.EnumValorTipo.Valor ? bd.LerDecimal("Valor").ToString("c") : bd.LerInt("Valor").ToString();
                    retorno.CodigoBarra = "000000000000000000";
                    retorno.ProcedimentoTroca = bd.LerString("ProcedimentoTroca");
                    retorno.Codigo = nomeImagem;
                    retorno.Saudacao = saudacao;
                } else
                    throw new Exception("Não foi possível encontrar os Vale ingressos desta compra, por favor tente novamente.");

                return JsonConvert.SerializeObject(retorno, Formatting.Indented);
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                bd.Fechar();
            }
        }

    }
}
