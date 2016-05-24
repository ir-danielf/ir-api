using CTLib;
using IRLib.Paralela.Assinaturas.Models.Relatorios;
using IRLib.Paralela.Codigo.Assinatura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;



namespace IRLib.Paralela.Codigo.Assinatura.Relatorios
{
    public class AcompanhamentoRenovacoes
    {

        public List<AcompanhamentoRenovacao> BuscarRenovacoes(int AssinaturaTipoID, int AssinaturaID, string DataInicial, string DataFinal)
        {
            List<AcompanhamentoRenovacao> _List = new List<AcompanhamentoRenovacao>();
            BD bd = new BD();
            DateTime DataInicio = Convert.ToDateTime(DataInicial);
            DateTime DataFim = Convert.ToDateTime(DataFinal);
            string di = DataInicio.ToString("yyyyMMddHHmmss");
            string df = DataFim.ToString("yyyyMMddHHmmss");

            string QueryRenovacao;
            try
            {
                if (AssinaturaID == 0)
                {
                    QueryRenovacao = string.Format(@"select cli.Nome as 'Cliente',cli.Cpf, Cli.RG, Cli.Email,tass.nome, tset.nome as 'Setor',
                             tlug.Codigo as 'Codigo' , COALESCE(tx.Nome, '') as 'Forma de Entrega', COALESCE(prectip.nome, '') as 'Preço',
                             case acao
                             	when 'D' then 'Desistencia'
                             	when 'R' then 'Renovado'
                             	when 'E' then 'Troca Efetiva'
                             	when 'N' then 'Aquisição'
                             	when 'T' then 'Troca Sinalizada'
                             	end
                             	as 'Ação'
                             ,COALESCE(vb.Senha, '') as 'Senha de Venda', dbo.DataFormatada(timestamp) as 'Data',
                             cli.EnderecoEntrega as 'Endereço', cli.NumeroEntrega as 'Número', cli.ComplementoEntrega as 'Complemento',
                             cli.BairroEntrega as 'Bairro', cli.CEPEntrega as 'CEP', cli.EstadoEntrega as 'Estado', cli.CidadeEntrega as 'Cidade'
                             from tassinaturacliente tac(nolock)
                             left join tcliente cli (nolock) on tac.ClienteID = cli.ID
                             left join tassinatura tass (nolock) on tass.id = tac.assinaturaid
                             left join tsetor tset(nolock) on tset.id = tac.setorid
                             left join tlugar tlug(nolock) on tlug.id = tac.lugarid
                             left join tvendabilheteria vb(nolock) on vb.id = tac.vendabilheteriaid
                             left join tentregacontrole txc(nolock) on txc.ID = vb.entregacontroleid
                             left join tentrega tx(nolock) on tx.id = txc.entregaid
                             left join tprecotipo prectip (nolock) on prectip.id = tac.precotipoid
                             where  tass.AssinaturaTipoID = {0} and acao in ('E','N','R', 'D', 'T') and tac.TimeStamp between '{1}' and '{2}'
                             order by timestamp, tass.ID asc", AssinaturaTipoID, di, df);
                }
                else
                {
                    QueryRenovacao = string.Format(@"select cli.Nome as 'Cliente',cli.Cpf, Cli.RG, Cli.Email,tass.nome, tset.nome as 'Setor',
                             tlug.Codigo as 'Codigo' , COALESCE(tx.Nome, '') as 'Forma de Entrega', COALESCE(prectip.nome, '') as 'Preço',
                             case acao
                             	when 'D' then 'Desistencia'
                             	when 'R' then 'Renovado'
                             	when 'E' then 'Troca Efetiva'
                             	when 'N' then 'Aquisição'
                             	when 'T' then 'Troca Sinalizada'
                             	end
                             	as 'Ação'
                             ,COALESCE(vb.Senha, '') as 'Senha de Venda', dbo.DataFormatada(timestamp) as 'Data',
                             cli.EnderecoEntrega as 'Endereço', cli.NumeroEntrega as 'Número', cli.ComplementoEntrega as 'Complemento',
                             cli.BairroEntrega as 'Bairro', cli.CEPEntrega as 'CEP', cli.EstadoEntrega as 'Estado', cli.CidadeEntrega as 'Cidade'
                             from tassinaturacliente tac(nolock)
                             left join tcliente cli (nolock) on tac.ClienteID = cli.ID
                             left join tassinatura tass (nolock) on tass.id = tac.assinaturaid
                             left join tsetor tset(nolock) on tset.id = tac.setorid
                             left join tlugar tlug(nolock) on tlug.id = tac.lugarid
                             left join tvendabilheteria vb(nolock) on vb.id = tac.vendabilheteriaid
                             left join tentregacontrole txc(nolock) on txc.ID = vb.entregacontroleid
                             left join tentrega tx(nolock) on tx.id = txc.entregaid
                             left join tprecotipo prectip (nolock) on prectip.id = tac.precotipoid
                             where assinaturaID = {0} and acao in ('E','N','R', 'D', 'T') and tac.TimeStamp between '{1}' and '{2}'
                             order by timestamp, tac.assinaturaID asc", AssinaturaID, di, df);
                }

                SqlDataReader dr = (SqlDataReader)bd.Consulta(QueryRenovacao);

                while (dr.Read())
                {
                    AcompanhamentoRenovacao acomp = new AcompanhamentoRenovacao();
                    acomp.Cliente = dr["Cliente"].ToString();
                    acomp.Cpf = dr["Cpf"].ToString();
                    acomp.RG = dr["RG"].ToString();
                    acomp.Email = dr["Email"].ToString();
                    acomp.AssinaturaNome = dr["nome"].ToString();
                    acomp.Setor = dr["Setor"].ToString();
                    acomp.Codigo = dr["Codigo"].ToString();
                    acomp.FormaEntrega = dr["Forma de Entrega"].ToString();
                    acomp.Preco = dr["Preço"].ToString();
                    acomp.Acao = dr["Ação"].ToString();
                    acomp.SenhaVenda = dr["Senha de Venda"].ToString();
                    acomp.Data = dr["Data"].ToString();
                    acomp.Endereco = dr["Endereço"].ToString();
                    acomp.Numero = dr["Número"].ToString();
                    acomp.Complemento = dr["Complemento"].ToString();
                    acomp.Bairro = dr["Bairro"].ToString();
                    acomp.CEP = dr["CEP"].ToString();
                    acomp.Estado = dr["Estado"].ToString();
                    acomp.Cidade = dr["Cidade"].ToString();

                    _List.Add(acomp);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return _List;
        }



    }
}
