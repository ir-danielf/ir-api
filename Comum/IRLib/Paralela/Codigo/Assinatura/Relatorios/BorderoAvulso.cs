using CTLib;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IRLib.Paralela.Assinaturas.Relatorios
{
    public class BorderoAvulso
    {

        BD bd;

        public BorderoAvulso()
        {
            bd = new BD();
        }

        public IRLib.Paralela.Assinaturas.Relatorios.ModelBorderoAvulso BuscarRelatorio(int Eventos, int Apresentacoes)
        {
            try
            {
                ModelBorderoAvulso retorno = new ModelBorderoAvulso();
                Evento oEvento = new Evento();
                Apresentacao oApresentacao = new Apresentacao();

                oApresentacao.Ler(Apresentacoes);
                retorno.Apresentacao = oApresentacao.Horario.Valor.ToString("dd/MM/yyyy HH:mm");

                string[] infos = oEvento.EventoLocalNome(Eventos);

                retorno.Assinatura = infos[1];
                retorno.Local = infos[0];

                string sql = string.Format(@"                                    
                SELECT s.Nome AS Setor, p.Nome AS Preco, 
                SUM(p.Valor) AS Faturamento, COUNT(DISTINCT i.ID) AS Quantidade
                FROM   tIngresso i (NOLOCK)
                INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                WHERE i.ApresentacaoID = {0} And i.EventoID = {1} AND (i.Status = 'V' OR i.Status= 'I' OR i.Status = 'E') 
                AND i.AssinaturaClienteID = 0
                AND i.CortesiaID = 0
                GROUP BY s.ID, s.Nome, p.Nome
                ORDER BY s.Nome, p.Nome 

                SELECT
                s.Nome AS Setor, s.ID, p.ID, p.Nome AS Preco, 
                SUM(p.Valor) AS Faturamento, COUNT(DISTINCT i.ID) AS Quantidade
                FROM   tIngresso i (NOLOCK)
                INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                WHERE i.ApresentacaoID = {0} And i.EventoID = {1} AND i.Status IN  ('V', 'I', 'E') 
                AND i.CortesiaID <> 0 AND i.AssinaturaClienteID = 0
                GROUP BY s.ID, s.Nome, p.Nome, p.ID
                ORDER BY s.Nome, p.Nome 

                SELECT
                s.Nome AS Setor,
                COUNT(i.ID) AS Lotacao, 
                        SUM(CASE WHEN i.AssinaturaClienteID > 0 AND i.CortesiaID <= 0
                            THEN 
                                CASE WHEN i.Status IN ('V', 'E', 'I')
                                THEN 1	
                                ELSE 0
                                END
                            ELSE
                                0
                            END) 
                        AS Assinante,
                        SUM(CASE WHEN i.AssinaturaClienteID <=0 AND i.CortesiaID <= 0
                            THEN 
                                CASE WHEN i.Status IN ('V', 'E', 'I')
                                THEN 1 
                                ELSE 0
                                END
                            ELSE
                                0
                            END) 
                        AS Avulso,
                        SUM(CASE WHEN  i.CortesiaID > 0
                            THEN 
                                CASE WHEN i.Status IN ('V', 'E', 'I')
                                THEN 1 
                                ELSE 0
                                END
                            ELSE
                                0
                            END) 
                    AS Cortesia,
                        SUM( 
			                CASE WHEN i.Status IN ('B')
			                THEN 1 
			                ELSE 0
			                END
                            ) 
                    AS Bloqueado
                FROM tIngresso i (NOLOCK)
                INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                WHERE i.ApresentacaoID = {0} And i.EventoID = {1}
                GROUP BY s.ID,s.Nome
                ORDER BY s.ID,s.Nome ", Apresentacoes, Eventos);

                bd.Consulta(sql);

                List<int> auxSetorID = new List<int>();

                int quantidadeTotal = 0;
                decimal faturamentoTotal = 0;

                while (bd.Consulta().Read())
                {
                    string setor = bd.LerString("Setor");
                    string preco = bd.LerString("Preco");
                    decimal faturamento = bd.LerDecimal("Faturamento");
                    int quantidade = bd.LerInt("Quantidade");
                    quantidadeTotal += quantidade;
                    faturamentoTotal += faturamento;

                    retorno.ListaResultado.Add(new Resultado()
                    {
                        Setor = setor,
                        Preco = preco,
                        Qtd = quantidade,
                        Faturamento = faturamento,
                    });
                }

                retorno.ListaResultado.Add(new Resultado()
                {
                    Setor = "Total",
                    Preco = "-",
                    Qtd = quantidadeTotal,
                    Faturamento = faturamentoTotal,
                });

                retorno.Despesas.Bruto = faturamentoTotal;

                bd.Consulta().NextResult();

                quantidadeTotal = 0;
                faturamentoTotal = 0;

                while (bd.Consulta().Read())
                {
                    string setor = bd.LerString("Setor");
                    string preco = bd.LerString("Preco");
                    decimal faturamento = bd.LerDecimal("Faturamento");
                    int quantidade = bd.LerInt("Quantidade");
                    quantidadeTotal += quantidade;
                    faturamentoTotal += faturamento;

                    retorno.ListaResultadoCortesia.Add(new Resultado()
                    {
                        Setor = setor,
                        Preco = preco,
                        Qtd = quantidade,
                        Faturamento = faturamento,
                    });
                }

                retorno.ListaResultadoCortesia.Add(new Resultado()
                {
                    Setor = "Total",
                    Preco = "-",
                    Qtd = quantidadeTotal,
                    Faturamento = faturamentoTotal,
                });

                bd.Consulta().NextResult();

                int lotacaoTotal = 0;
                int bloqueadosTotal = 0;
                int cortesiaTotal = 0;
                int assinanteTotal = 0;
                int avulsoTotal = 0;

                while (bd.Consulta().Read())
                {
                    string setor = bd.LerString("Setor");
                    int lotacao = bd.LerInt("Lotacao");
                    int bloqueados = bd.LerInt("Bloqueado");
                    int cortesia = bd.LerInt("Cortesia");
                    int assinante = bd.LerInt("Assinante");
                    int avulso = bd.LerInt("Avulso");

                    lotacaoTotal += lotacao;
                    bloqueadosTotal += bloqueados;
                    cortesiaTotal += cortesia;
                    assinanteTotal += assinante;
                    avulsoTotal += avulso;

                    retorno.ListaResultadoSetor.Add(new ResultadoSetor()
                    {
                        Setor = setor,
                        Lotacao = lotacao,
                        Bloqueados = bloqueados,
                        Cortesia = cortesia,
                        Assinante = assinante,
                        Avulso = avulso,
                    });
                }

                retorno.ListaResultadoSetor.Add(new ResultadoSetor()
                {
                    Setor = "Total",
                    Lotacao = lotacaoTotal,
                    Bloqueados = bloqueadosTotal,
                    Cortesia = cortesiaTotal,
                    Assinante = assinanteTotal,
                    Avulso = avulsoTotal,
                });



                return retorno;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class ModelBorderoAvulso
    {
        public ModelBorderoAvulso() { }
        public List<Resultado> ListaResultado = new List<Resultado>();
        public List<Resultado> ListaResultadoCortesia = new List<Resultado>();
        public List<ResultadoSetor> ListaResultadoSetor = new List<ResultadoSetor>();
        public ResultadoDescontos Despesas = new ResultadoDescontos();
        public string Assinatura;
        public string Apresentacao;
        public string Local;
        public DateTime Emissao
        {
            get
            {
                return DateTime.Now;
            }
        }

        public string PontuarNumero(int numero)
        {
            string retorno = String.Format("{0:0,0}", numero);
            return retorno.Replace(',', '.');
        }
    }
    public class Resultado
    {
        public string Setor;
        public string Preco;
        public decimal PrecoMedio
        {
            get
            {
                return Qtd > 0 ? (Faturamento / Qtd) : 0;
            }
        }
        public int Qtd;
        public decimal Faturamento;
    }
    public class ResultadoSetor
    {
        public string Setor;
        public int Lotacao;
        public int Bloqueados;
        public int Cortesia;
        public int Assinante;
        public int Avulso;
        public int Publico { get { return (Cortesia + Assinante + Avulso); } }
        public int Disponivel { get { return (Lotacao - (Bloqueados + Cortesia + Assinante + Avulso)); } }

        public string Ocupacao
        {
            get
            {
                if (Publico > 0 && Lotacao > 0)
                    return (Publico * 100 / Lotacao).ToString() + " %";
                else
                    return "0 %";
            }
        }
    }

    public class ResultadoDescontos
    {
        public decimal Bruto;

        private decimal ISS = 2;

        public string PorcentagemISS
        {
            get
            {
                return ISS.ToString() + " %";
            }
        }

        public decimal ValorISS
        {
            get
            {
                return (Bruto * (ISS / 100));
            }
        }


        public decimal Liquido
        {
            get
            {
                return Bruto - ValorISS;
            }
        }
    }

}
