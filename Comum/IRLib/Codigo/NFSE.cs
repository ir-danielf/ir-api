using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Codigo
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class NFSE : MarshalByRefObject
    {
        public List<string> GerarXML(EstruturaNFSE estrutura)
        {
            BD bd = new BD();
            try
            {
                string sql =
                   string.Format(@"SELECT
                       vb.DataVenda, il.ID AS iLogID, p.Valor
                    FROM tVendaBilheteria vb (NOLOCK)
                    INNER JOIN tIngressoLog il (NOLOCK) ON il.VendaBilheteriaID = vb.ID
                    INNER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID
                    INNER JOIN tIngresso i (NOLOCK) ON il.IngressoID = i.ID
                    WHERE il.Acao = '{0}' AND i.EventoID = {1} AND p.Valor > 0
                    ", IngressoLog.VENDER, estrutura.EventoID);

                List<ListaRPS> listagem = new List<ListaRPS>();

                int i = 0;

                int maxRegistros = int.MaxValue;
                //int maxRegistros = Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMaximaRegistros"]); // 200 Registros = 500kb

                Random rnd = new Random();
                ListaRPS listaRPS = null;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    if (i == 0 || i % maxRegistros == 0)
                    {
                        if (i > 0)
                            listagem.Add(listaRPS);

                        listaRPS = new ListaRPS(
                          rnd.Next(int.MaxValue).ToString(), rnd.Next(int.MaxValue), estrutura.CNPJ, estrutura.IE);
                    }


                    listaRPS.Add(new RPS(new EstruturaGeral()
                    {
                        CNPJ = estrutura.CNPJ,
                        IE = estrutura.IE,
                        DataEmissao = bd.LerDateTime("DataVenda"),
                        Id = bd.LerInt("iLogID"),
                        Numero = bd.LerInt("iLogID").ToString(),
                        Razao = estrutura.Razao,
                        Serie = estrutura.Serie,
                        Tipo = estrutura.Tipo,
                        Valor = bd.LerDecimal("Valor"),
                        Cliente = new EstruturaClienteNFSE()
                        {
                            AtribuirCliente = false,
                            //Nome = bd.LerString("Nome"),
                            //Bairro = bd.LerString("BairroCliente"),
                            //CEP = bd.LerString("CEPCliente"),
                            //Complemento = bd.LerString("ComplementoCliente"),
                            //CPF = bd.LerString("CPF"),
                            //Endereco = bd.LerString("EnderecoCliente"),
                            //Numero = bd.LerString("NumeroCliente"),
                            //Uf = bd.LerString("EstadoCliente"),
                            //Email = bd.LerString("Email"),
                        },
                    }));
                    i++;
                }

                if (i >= maxRegistros)
                    listagem.Add(listaRPS);


                List<string> listaInnerXml = new List<string>();
                for (i = 0; i < listagem.Count; i++)
                    listaInnerXml.Add(listagem[i].GerarXML().InnerXml);

                return listaInnerXml;
            }
            finally
            {
                bd.Fechar();
            }
        }

    }
}
