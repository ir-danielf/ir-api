using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTLib;

namespace IRLib.Assinaturas.Models.Relatorios
{
    public class AcompanhamentoPresencas
    {
        public List<EstruturaDataApresentacao> BuscarDatasEventos(int assinaturaTipo, int ano)
        {
            string query = @"select distinct ap.ID as 'Apresentacao', Horario, ta.nome, ta.id as 'Assinatura' from tapresentacao ap(nolock)
                                inner join tassinaturaitem item (nolock) on ap.id = item.apresentacaoid
                                inner join tassinaturaano ano(nolock) on ano.id = item.assinaturaanoid
                                inner join tassinatura ta (nolock) on ta.ID = ano.assinaturaid
                                where ta.assinaturatipoid = {0} and ano.ano = {1}";


            query = string.Format(query, assinaturaTipo, ano);

            List<EstruturaDataApresentacao> datas = new List<EstruturaDataApresentacao>();
            BD bd = new BD();
            Dictionary<string, int> assinaturas = new Dictionary<string, int>();
            while (bd.Consulta(query).Read())
            {
                EstruturaDataApresentacao est = new EstruturaDataApresentacao();
                string nome = est.AssinaturaNome = bd.LerString("Nome");
                if (!nome.Contains("2"))
                {
                    est.ApresentacaoID = bd.LerInt("Apresentacao");
                    est.Data = bd.LerDateTime("horario");
                    est.AssinaturaNome = nome;
                    est.AssinaturaID = bd.LerInt("Assinatura");
                    datas.Add(est);
                }
            }
            return datas;
        }


        public void CarregarArquivo(List<string> linhas, int apresentacaoID)
        {
            string insertstring = @"insert into tIngressoAcesso (ID, ApresentacaoID,Compareceu) values ({0},{1},'{2}')";
            string selectID = @"(select ID from tingresso where codigobarra = '{0}')";
            BD bd = new BD();
            if (bd.Consulta("select top 1 apresentacaoID from tIngressoAcesso where apresentacaoid = " + apresentacaoID).Read())
            {
                throw new Exception("Essa apresentação já foi processada.");
            }
            bool apresentacaoChecada = false;
            foreach (string linha in linhas)
            {
                string[] valores = linha.Split(';');
                if (valores.Length < 11 || valores[0].ToLower().Contains("senha"))
                    continue;

                if (string.IsNullOrEmpty(valores[9]) || string.IsNullOrEmpty(valores[0]))
                    continue;
                else
                {
                    if (!apresentacaoChecada)
                    {
                        string s = "Select apresentacaoID from tingresso where codigoBarra = '{0}'";
                        s = string.Format(s, valores[9]);
                        System.Diagnostics.Debug.WriteLine(s);
                        int ap = Convert.ToInt32(bd.ConsultaValor(s));
                        if (apresentacaoID != ap)
                            throw new Exception("Arquivo não corresponde a apresentação selecionada.");

                        apresentacaoChecada = true;
                    }
                    try
                    {
                        string cid = string.Format(selectID, valores[9]);
                        string compareceu = valores[10] == "Sim" ? "TRUE" : "FALSE";
                        string st = string.Format(insertstring, cid, apresentacaoID, compareceu);
                        bd.Executar(st);
                    }
                    catch(Exception)
                    {
                    }
                }
            }
        }


        private List<int> BuscarApresentacoesAcessoCadastrado(BD bd, string dataInicial, string dataFinal, int assinaturaTipo, int ano)
        {
            string query = @"select distinct ap.ID as 'Apresentacao' from tapresentacao ap(nolock)
                                inner join tassinaturaitem item (nolock) on ap.id = item.apresentacaoid
                                inner join tassinaturaano ano(nolock) on ano.id = item.assinaturaanoid
                                inner join tassinatura ta (nolock) on ta.ID = ano.assinaturaid
                                where ta.assinaturatipoid = {0} and ano.ano = {1}
								and horario between '{2}' and '{3}'
								and (select top 1 apresentacaoid from tingressoacesso where apresentacaoid = ap.id) is not null";


            DateTime dti = DateTime.Parse(dataInicial);
            DateTime dtf = DateTime.Parse(dataFinal);

            string sdi = dti.ToString("yyyyMMdd") + "060000";
            string sdf = dtf.ToString("yyyyMMdd") + "235959";


            List<int> apresentacoes = new List<int>();
            query = string.Format(query, assinaturaTipo, ano, sdi, sdf);
            while (bd.Consulta(query).Read())
            {
                apresentacoes.Add(bd.LerInt("Apresentacao"));
            }

            return apresentacoes;
        }

        public List<EstruturaPresenca> RelatorioPresencas(string dataInicial, string dataFinal, int AssinaturaTipo, int ano)
        {
            List<EstruturaPresenca> relatorioCompleto = new List<EstruturaPresenca>();

            BD bd = new BD();
            try
            {
                string relatorio = @"rel_PresencasAssinaturas {0} , {1} , '{2}' ";
                List<int> res = BuscarApresentacoesAcessoCadastrado(bd, dataInicial, dataFinal, AssinaturaTipo, ano);
                string apresentacoes = Utilitario.ArrayToString(res.ToArray());

                relatorio = string.Format(relatorio, AssinaturaTipo, ano, apresentacoes);

                while (bd.Consulta(relatorio).Read())
                {
                    EstruturaPresenca ln = new EstruturaPresenca();

                    ln.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    ln.Assinantes = bd.LerInt("Assinantes");
                    ln.Avulsas = bd.LerInt("Avulsas");
                    ln.CapacidadeSala = bd.LerInt("CapacidadeSala");
                    ln.Cortesias = bd.LerInt("Cortesias");
                    ln.DataApresentacao = bd.LerDateTime("Horario");
                    ln.PublicoPrevisto = bd.LerInt("PublicoPrevisto");
                    ln.PresencasAssinantes = bd.LerInt("PresencasAssinantes");
                    ln.PresencasAvulsas = bd.LerInt("PresencasAvulsas");
                    ln.PresencasCortesias = bd.LerInt("PresencasCortesias");
                    relatorioCompleto.Add(ln);
                }
            }
            catch { }
            finally
            {
                bd.Fechar();
            }

            return relatorioCompleto;
        }
    }
    [Serializable]
    public class EstruturaPresenca
    {
        public int ApresentacaoID { get; set; }
        public DateTime DataApresentacao { get; set; }
        public int CapacidadeSala { get; set; }
        public int PublicoPrevisto { get; set; }
        public int Assinantes { get; set; }
        public int Avulsas { get; set; }
        public int Cortesias { get; set; }
        public int PresencasAssinantes { get; set; }
        public int PresencasAvulsas { get; set; }
        public int PresencasCortesias { get; set; }
    }


    [Serializable]
    public class EstruturaDataApresentacao
    {
        public DateTime Data { get; set; }
        public int AssinaturaID { get; set; }
        public int ApresentacaoID { get; set; }
        public string AssinaturaNome { get; set; }
    }
}
