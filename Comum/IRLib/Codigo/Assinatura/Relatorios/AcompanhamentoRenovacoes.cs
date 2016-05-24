using CTLib;
using IRLib.Assinaturas.Models.Relatorios;
using IRLib.Codigo.Assinatura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;



namespace IRLib.Codigo.Assinatura.Relatorios
{
    public class AcompanhamentoRenovacoes
    {

        public List<AcompanhamentoRenovacao> BuscarRenovacoes(int AssinaturaTipoID, int AssinaturaID, string DataInicial, string DataFinal)
        {
            List<AcompanhamentoRenovacao> _List = new List<AcompanhamentoRenovacao>();
            BD bd = new BD();
            DateTime DataInicio = Convert.ToDateTime(DataInicial);
            DateTime DataFim = Convert.ToDateTime(DataFinal);
            string di = DataInicio.ToString("yyyyMMdd");
            string df = DataFim.ToString("yyyyMMdd");

            string QueryRenovacao;
            try
            {
                if (AssinaturaID == 0)
                {
                    QueryRenovacao = string.Format("Exec sp_AcompanhamentoRenovacaoPorAssinaturaTipo {0},{1},{2}", AssinaturaTipoID, di, df);
                }
                else
                {
                    QueryRenovacao = string.Format("Exec sp_AcompanhamentoRenovacaoPorAssinatura {0},{1},{2}", AssinaturaID, di, df);
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
                    acomp.Status = dr["Status"].ToString();
                    acomp.SenhaVenda = dr["Senha de Venda"].ToString();
                    acomp.Data = dr["Data"].ToString();
                    acomp.Endereco = dr["Endereço"].ToString();
                    acomp.Numero = dr["Número"].ToString();
                    acomp.Complemento = dr["Complemento"].ToString();
                    acomp.Bairro = dr["Bairro"].ToString();
                    acomp.CEP = dr["CEP"].ToString();
                    acomp.Estado = dr["Estado"].ToString();
                    acomp.Cidade = dr["Cidade"].ToString();
                    acomp.NomeUsuario = dr["NomeUsuario"].ToString();

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
