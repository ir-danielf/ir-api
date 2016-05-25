using CTLib;
using IRLib;
using IRLib.ClientObjects;
using IRLib.Codigo.ModuloLogistica;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IngressoRapido.Lib
{
    public class IngressoGerenciadorInternet
    {
        BD bd = new BD();

        bool ImprimiValeIngresso = false;

        public string PesquisaVendaBilheteria(int vendaBilheteriaID, int clienteID)
        {
            try
            {
                this.RegistrarImpressaoInternet(vendaBilheteriaID, clienteID);

                if (!ImprimiValeIngresso)
                {
                    string sql = string.Format(@"SELECT e.Nome AS Evento, a.Horario, s.Nome AS Setor, i.ID as IngressoID, i.Codigo, i.CodigoBarra, p.Valor, p.Nome AS Preco, l.Nome AS Local, l.Logradouro, l.Numero, l.Estado, l.Cidade, l.Bairro, l.Latitude, l.Longitude, vb.Senha, vb.DataVenda,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.NomeFantasia
	                                        ELSE 
		                                        CASE WHEN LEN(cImp.ID ) > 0
			                                        THEN cImp.Nome
			                                        ELSE 
				                                        CASE WHEN LEN(ce.ID) > 0
					                                        THEN ce.Nome COLLATE Latin1_General_CI_AI 
					                                        ELSE c.Nome COLLATE Latin1_General_CI_AI  
				                                        END
		                                        END
                                        END AS Cliente,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.CNPJ
                                        ELSE 
	                                        CASE WHEN LEN(cImp.ID) > 0
		                                        THEN
			                                        CASE WHEN LEN(ic.CPF) > 0
				                                        THEN ic.CPF COLLATE Latin1_General_CI_AI 
					                                        ELSE cimp.CPF COLLATE Latin1_General_CI_AI 
				                                        END
			                                        ELSE
				                                        CASE WHEN LEN(ce.ID ) > 0
					                                        THEN ce.CPF
						                                        ELSE c.CPF 
					                                        END 
			                                        END
	                                        END AS ClienteCPF, tvbi.TaxaConvenienciaValor, e.ImagemInternet
                                        FROM tVendaBilheteria vb (NOLOCK)
                                        INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                                        INNER JOIN tIngressoLog til (NOLOCK) ON til.IngressoID = i.ID AND til.VendaBilheteriaID = vb.ID AND til.Acao = 'V'
                                        INNER JOIN tVendaBilheteriaItem tvbi (NOLOCK) ON tvbi.VendaBilheteriaID = vb.ID AND tvbi.ID = til.VendaBilheteriaItemID
										INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
										INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
										INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
										INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
										INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
										INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
										LEFT JOIN tIngressoCliente ic (NOLOCK) ON ic.IngressoID = i.ID
										LEFT JOIN tCliente cImp (NOLOCK) ON cImp.ID = ic.ClienteID
										LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
										WHERE vb.ID = {0} AND vb.ClienteID = {1}
										AND vb.Status = '{2}' AND i.Status IN ('{3}', '{4}', '{5}')
								", vendaBilheteriaID, clienteID, VendaBilheteria.PAGO, Ingresso.VENDIDO, Ingresso.IMPRESSO, Ingresso.ENTREGUE);

                    if (!bd.Consulta(sql).Read())
                        throw new Exception("Não foi possível encontrar os ingressos desta compra, por favor tente novamente.");

                    List<IngressoImpressaoInternet> ingressos = new List<IngressoImpressaoInternet>();
                    int indice = 0;
                    do
                    {
                        ingressos.Add(new IngressoImpressaoInternet()
                        {
                            Indice = indice++,
                            Evento = bd.LerString("Evento"),
                            Apresentacao = (bd.LerDateTime("Horario")).ToString("dd/MM/yy à\\s HH:mm"),
                            Setor = bd.LerString("Setor"),
                            Codigo = bd.LerString("Codigo"),
                            Valor = bd.LerDecimal("Valor").ToString("c"),
                            Preco = bd.LerString("Preco"),
                            Local = bd.LerString("Local"),
                            Endereco = bd.LerString("Logradouro"),
                            Numero = bd.LerString("Numero"),
                            Bairro = bd.LerString("Bairro"),
                            Cidade = bd.LerString("Cidade"),
                            Estado = bd.LerString("Estado"),
                            Senha = bd.LerString("Senha"),
                            DataVenda = (bd.LerDateTime("DataVenda")).ToString("dd/MM/yy à\\s HH:mm"),
                            ClienteNome = bd.LerString("Cliente"),
                            ClienteCPFCNPJ = bd.LerString("ClienteCPF"),
                            CodigoBarra = bd.LerString("CodigoBarra"),
                            ValeIngresso = false,
                            Imagem = bd.LerString("ImagemInternet"),
                            Latitude = bd.LerString("Latitude"),
                            Longitude = bd.LerString("Longitude")
                        });
                    } while (bd.Consulta().Read());

                    return JsonConvert.SerializeObject
                    (ingressos, Formatting.Indented);
                }
                else
                {
                    string sql = string.Format(@"SELECT tVendaBilheteria.Senha,tValeIngresso.DataExpiracao, tValeIngressoTipo.Valor,tValeIngressoTipo.ValorTipo,tValeIngresso.CodigoTroca, tValeIngresso.CodigoBarra,
							tValeIngresso.ClienteNome, tValeIngressoTipo.ProcedimentoTroca, tValeIngressoTipo.SaudacaoNominal, tValeIngressoTipo.SaudacaoPadrao, tValeIngressoTipo.ID as Codigo
							FROM tVendaBilheteria (NOLOCK)
							INNER JOIN tValeIngresso (NOLOCK) ON tVendaBilheteria.ID = tValeIngresso.VendaBilheteriaID
							INNER JOIN tValeIngressoTipo (NOLOCK) ON tValeIngresso.ValeIngressoTipoID = tValeIngressoTipo.ID
							WHERE tValeIngresso.ClienteID = {0} AND tValeIngresso.VendaBilheteriaID = {1}
							AND tVendaBilheteria.Status = '{2}' AND tValeIngresso.Status IN ('{3}', '{4}', '{5}')",
                            clienteID, vendaBilheteriaID, VendaBilheteria.PAGO, (char)ValeIngresso.enumStatus.Vendido, (char)ValeIngresso.enumStatus.Impresso, (char)ValeIngresso.enumStatus.Entregue);

                    if (!bd.Consulta(sql).Read())
                        throw new Exception("Não foi possível encontrar os Vale ingressos desta compra, por favor tente novamente.");

                    List<ValeIngressoImpressaoInternet> valeIngressos = new List<ValeIngressoImpressaoInternet>();

                    int indice = 0;
                    do
                    {
                        string nome = bd.LerString("ClienteNome");
                        string saudacao = string.Empty;

                        if (nome.Length > 0)
                        {
                            saudacao = bd.LerString("SaudacaoNominal");
                            saudacao = saudacao.Replace("#cliente#", nome);
                        }
                        else
                            saudacao = bd.LerString("SaudacaoPadrao");

                        int codigo = bd.LerInt("Codigo");

                        string nomeImagem = "ivir" + codigo.ToString("000000");

                        char valorTipo;
                        string valor;

                        valorTipo = Convert.ToChar(bd.LerString("ValorTipo"));

                        if (valorTipo == (char)IRLib.ValeIngressoTipo.EnumValorTipo.Valor)
                            valor = "R$ " + bd.LerDecimal("Valor").ToString("c");
                        else
                            valor = bd.LerInt("Valor").ToString() + " %";

                        valeIngressos.Add(new ValeIngressoImpressaoInternet()
                        {
                            Indice = indice++,
                            Senha = bd.LerString("Senha"),
                            DataValidade = bd.LerStringFormatoData("DataExpiracao"),
                            ValorTipo = valorTipo,
                            Valor = valor,
                            CodigoTroca = bd.LerString("CodigoTroca"),
                            CodigoBarra = bd.LerString("CodigoBarra"),
                            ProcedimentoTroca = bd.LerString("ProcedimentoTroca"),
                            Codigo = nomeImagem,
                            Saudacao = saudacao,
                            ValeIngresso = true,
                        });

                    } while (bd.Consulta().Read());

                    return JsonConvert.SerializeObject
                    (valeIngressos, Formatting.Indented);
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
        }

        public List<IngressoImpressaoInternet> PesquisaVendaBilheteriaMobile(int vendaBilheteriaID, int clienteID)
        {
            try
            {
                string sql = string.Format(@"SELECT e.Nome AS Evento, a.Horario, s.Nome AS Setor, i.ID as IngressoID, i.Codigo, i.CodigoBarra, p.Valor, p.Nome AS Preco, l.Nome AS Local, l.Logradouro, l.Numero, l.Estado, l.Cidade, l.Bairro, l.Latitude, l.Longitude, vb.Senha, vb.DataVenda,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.NomeFantasia
	                                        ELSE 
		                                        CASE WHEN LEN(cImp.ID ) > 0
			                                        THEN cImp.Nome
			                                        ELSE 
				                                        CASE WHEN LEN(ce.ID) > 0
					                                        THEN ce.Nome COLLATE Latin1_General_CI_AI 
					                                        ELSE c.Nome COLLATE Latin1_General_CI_AI  
				                                        END
		                                        END
                                        END AS Cliente,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.CNPJ
                                        ELSE 
	                                        CASE WHEN LEN(cImp.ID) > 0
		                                        THEN
			                                        CASE WHEN LEN(ic.CPF) > 0
				                                        THEN ic.CPF COLLATE Latin1_General_CI_AI 
					                                        ELSE cimp.CPF COLLATE Latin1_General_CI_AI 
				                                        END
			                                        ELSE
				                                        CASE WHEN LEN(ce.ID ) > 0
					                                        THEN ce.CPF
						                                        ELSE c.CPF 
					                                        END 
			                                        END
	                                        END AS ClienteCPF, tvbi.TaxaConvenienciaValor, e.ImagemInternet
                                        FROM tVendaBilheteria vb (NOLOCK)
                                        INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                                        INNER JOIN tIngressoLog til (NOLOCK) ON til.IngressoID = i.ID AND til.VendaBilheteriaID = vb.ID AND til.Acao = 'V'
                                        INNER JOIN tVendaBilheteriaItem tvbi (NOLOCK) ON tvbi.VendaBilheteriaID = vb.ID AND tvbi.ID = til.VendaBilheteriaItemID
										INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
										INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
										INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
										INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
										INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
										INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
										LEFT JOIN tIngressoCliente ic (NOLOCK) ON ic.IngressoID = i.ID
										LEFT JOIN tCliente cImp (NOLOCK) ON cImp.ID = ic.ClienteID
										LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
										WHERE vb.ID = {0} AND vb.ClienteID = {1}
										AND vb.Status = '{2}' AND i.Status IN ('{3}', '{4}', '{5}')
								", vendaBilheteriaID, clienteID, VendaBilheteria.PAGO, Ingresso.VENDIDO, Ingresso.IMPRESSO, Ingresso.ENTREGUE);

                bd.Consulta(sql);

                List<IngressoImpressaoInternet> ingressos = new List<IngressoImpressaoInternet>();

                int indice = 0;

                while (bd.Consulta().Read())
                {
                    ingressos.Add(new IngressoImpressaoInternet()
                    {
                        Indice = indice++,
                        IngressoID = bd.LerInt("IngressoID"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = (bd.LerDateTime("Horario")).ToString("dd/MM/yy à\\s HH:mm"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        Valor = bd.LerDecimal("Valor").ToString("c"),
                        Conveniencia = bd.LerDecimal("TaxaConvenienciaValor").ToString("c"),
                        Preco = bd.LerString("Preco"),
                        Local = bd.LerString("Local"),
                        Endereco = bd.LerString("Logradouro"),
                        Numero = bd.LerString("Numero"),
                        Bairro = bd.LerString("Bairro"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Senha = bd.LerString("Senha"),
                        DataVenda = (bd.LerDateTime("DataVenda")).ToString("dd/MM/yy à\\s HH:mm"),
                        ClienteNome = bd.LerString("Cliente"),
                        ClienteCPFCNPJ = bd.LerString("ClienteCPF"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        ValeIngresso = false,
                        Imagem = bd.LerString("ImagemInternet"),
                        Latitude = bd.LerString("Latitude"),
                        Longitude = bd.LerString("Longitude")
                    });
                }

                foreach (var ingresso in ingressos)
                {
                    if (string.IsNullOrEmpty(ingresso.CodigoBarra))
                    {
                        List<EstruturaRetornoRegistroImpressao> estruturaimpressao = this.RegistrarImpressaoInternet(ingresso.IngressoID);

                        foreach (var item in estruturaimpressao)
                            ingresso.CodigoBarra = item.CodigoBarra;
                    }
                }

                if (ingressos.Count == 0)
                    throw new Exception("Não foi possível encontrar os ingressos desta compra, por favor tente novamente.");

                return ingressos;
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

        public List<IngressoImpressaoInternet> PesquisaVendaBilheteriaMobile(string Senha, int clienteID)
        {
            try
            {
                string sql = string.Format(@"SELECT e.Nome AS Evento, a.Horario, s.Nome AS Setor, i.ID as IngressoID, i.Codigo, i.CodigoBarra, p.Valor, p.Nome AS Preco, l.Nome AS Local, l.Logradouro, l.Numero, l.Estado, l.Cidade, l.Bairro, l.Latitude, l.Longitude, vb.Senha, vb.DataVenda,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.NomeFantasia
	                                        ELSE 
		                                        CASE WHEN LEN(cImp.ID ) > 0
			                                        THEN cImp.Nome
			                                        ELSE 
				                                        CASE WHEN LEN(ce.ID) > 0
					                                        THEN ce.Nome COLLATE Latin1_General_CI_AI 
					                                        ELSE c.Nome COLLATE Latin1_General_CI_AI  
				                                        END
		                                        END
                                        END AS Cliente,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.CNPJ
                                        ELSE 
	                                        CASE WHEN LEN(cImp.ID) > 0
		                                        THEN
			                                        CASE WHEN LEN(ic.CPF) > 0
				                                        THEN ic.CPF COLLATE Latin1_General_CI_AI 
					                                        ELSE cimp.CPF COLLATE Latin1_General_CI_AI 
				                                        END
			                                        ELSE
				                                        CASE WHEN LEN(ce.ID ) > 0
					                                        THEN ce.CPF
						                                        ELSE c.CPF 
					                                        END 
			                                        END
	                                        END AS ClienteCPF, tvbi.TaxaConvenienciaValor, e.ImagemInternet
                                        FROM tVendaBilheteria vb (NOLOCK)
                                        INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                                        INNER JOIN tIngressoLog til (NOLOCK) ON til.IngressoID = i.ID AND til.VendaBilheteriaID = vb.ID AND til.Acao = 'V'
                                        INNER JOIN tVendaBilheteriaItem tvbi (NOLOCK) ON tvbi.VendaBilheteriaID = vb.ID AND tvbi.ID = til.VendaBilheteriaItemID
										INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
										INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
										INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
										INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
										INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
										INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
										LEFT JOIN tIngressoCliente ic (NOLOCK) ON ic.IngressoID = i.ID
										LEFT JOIN tCliente cImp (NOLOCK) ON cImp.ID = ic.ClienteID
										LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
										WHERE vb.Senha = '{0}' AND vb.ClienteID = {1}
										AND vb.Status = '{2}' AND i.Status IN ('{3}', '{4}', '{5}')
								", Senha, clienteID, VendaBilheteria.PAGO, Ingresso.VENDIDO, Ingresso.IMPRESSO, Ingresso.ENTREGUE);

                bd.Consulta(sql);

                List<IngressoImpressaoInternet> ingressos = new List<IngressoImpressaoInternet>();

                int indice = 0;

                while (bd.Consulta().Read())
                {
                    ingressos.Add(new IngressoImpressaoInternet()
                    {
                        Indice = indice++,
                        IngressoID = bd.LerInt("IngressoID"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = (bd.LerDateTime("Horario")).ToString("dd/MM/yy à\\s HH:mm"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        Valor = bd.LerDecimal("Valor").ToString("c"),
                        Conveniencia = bd.LerDecimal("TaxaConvenienciaValor").ToString("c"),
                        Preco = bd.LerString("Preco"),
                        Local = bd.LerString("Local"),
                        Endereco = bd.LerString("Logradouro"),
                        Numero = bd.LerString("Numero"),
                        Bairro = bd.LerString("Bairro"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Senha = Senha,
                        DataVenda = (bd.LerDateTime("DataVenda")).ToString("dd/MM/yy à\\s HH:mm"),
                        ClienteNome = bd.LerString("Cliente"),
                        ClienteCPFCNPJ = bd.LerString("ClienteCPF"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        ValeIngresso = false,
                        Imagem = bd.LerString("ImagemInternet"),
                        Latitude = bd.LerString("Latitude"),
                        Longitude = bd.LerString("Longitude")
                    });
                }

                foreach (var ingresso in ingressos)
                {
                    if (string.IsNullOrEmpty(ingresso.CodigoBarra))
                    {
                        List<EstruturaRetornoRegistroImpressao> estruturaimpressao = this.RegistrarImpressaoInternet(ingresso.IngressoID);

                        foreach (var item in estruturaimpressao)
                            ingresso.CodigoBarra = item.CodigoBarra;
                    }
                }

                if (ingressos.Count == 0)
                    throw new Exception("Não foi possível encontrar os ingressos desta compra, por favor tente novamente.");

                return ingressos;
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

        public List<IngressoImpressaoInternet> PesquisaVendaBilheteriaMobileSemRegistar(int vendaBilheteriaID, int clienteID)
        {
            try
            {
                string sql = string.Format(@"SELECT e.Nome AS Evento, a.Horario, s.Nome AS Setor, i.ID as IngressoID, i.Codigo, i.CodigoBarra, p.Valor, p.Nome AS Preco, l.Nome AS Local, l.Logradouro, l.Numero, l.Estado, l.Cidade, l.Bairro, l.Latitude, l.Longitude, vb.Senha, vb.DataVenda,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.NomeFantasia
	                                        ELSE 
		                                        CASE WHEN LEN(cImp.ID ) > 0
			                                        THEN cImp.Nome
			                                        ELSE 
				                                        CASE WHEN LEN(ce.ID) > 0
					                                        THEN ce.Nome COLLATE Latin1_General_CI_AI 
					                                        ELSE c.Nome COLLATE Latin1_General_CI_AI  
				                                        END
		                                        END
                                        END AS Cliente,
                                        CASE WHEN LEN(c.CNPJ) > 0
	                                        THEN c.CNPJ
                                        ELSE 
	                                        CASE WHEN LEN(cImp.ID) > 0
		                                        THEN
			                                        CASE WHEN LEN(ic.CPF) > 0
				                                        THEN ic.CPF COLLATE Latin1_General_CI_AI 
					                                        ELSE cimp.CPF COLLATE Latin1_General_CI_AI 
				                                        END
			                                        ELSE
				                                        CASE WHEN LEN(ce.ID ) > 0
					                                        THEN ce.CPF
						                                        ELSE c.CPF 
					                                        END 
			                                        END
	                                        END AS ClienteCPF, tvbi.TaxaConvenienciaValor, e.ImagemInternet
                                        FROM tVendaBilheteria vb (NOLOCK)
                                        INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                                        INNER JOIN tIngressoLog til (NOLOCK) ON til.IngressoID = i.ID AND til.VendaBilheteriaID = vb.ID AND til.Acao = 'V'
                                        INNER JOIN tVendaBilheteriaItem tvbi (NOLOCK) ON tvbi.VendaBilheteriaID = vb.ID AND tvbi.ID = til.VendaBilheteriaItemID
										INNER JOIN tPreco p (NOLOCK) ON p.ID = i.PrecoID
										INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
										INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
										INNER JOIN tEvento e (NOLOCK) ON e.ID = i.EventoID
										INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID
										INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
										LEFT JOIN tIngressoCliente ic (NOLOCK) ON ic.IngressoID = i.ID
										LEFT JOIN tCliente cImp (NOLOCK) ON cImp.ID = ic.ClienteID
										LEFT JOIN tClienteEndereco ce (NOLOCK) ON ce.ID = vb.ClienteEnderecoID
										WHERE vb.ID = {0} AND vb.ClienteID = {1}
										AND i.Status IN ('{2}', '{3}', '{4}')
								", vendaBilheteriaID, clienteID, Ingresso.VENDIDO, Ingresso.IMPRESSO, Ingresso.ENTREGUE);

                bd.Consulta(sql);

                List<IngressoImpressaoInternet> ingressos = new List<IngressoImpressaoInternet>();

                int indice = 0;

                while (bd.Consulta().Read())
                {
                    ingressos.Add(new IngressoImpressaoInternet()
                    {
                        Indice = indice++,
                        Evento = bd.LerString("Evento"),
                        Apresentacao = (bd.LerDateTime("Horario")).ToString("dd/MM/yy à\\s HH:mm"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        Valor = bd.LerDecimal("Valor").ToString("c"),
                        Conveniencia = bd.LerDecimal("TaxaConvenienciaValor").ToString("c"),
                        Preco = bd.LerString("Preco"),
                        Local = bd.LerString("Local"),
                        Endereco = bd.LerString("Logradouro"),
                        Numero = bd.LerString("Numero"),
                        Bairro = bd.LerString("Bairro"),
                        Cidade = bd.LerString("Cidade"),
                        Estado = bd.LerString("Estado"),
                        Senha = bd.LerString("Senha"),
                        DataVenda = (bd.LerDateTime("DataVenda")).ToString("dd/MM/yy à\\s HH:mm"),
                        ClienteNome = bd.LerString("Cliente"),
                        ClienteCPFCNPJ = bd.LerString("ClienteCPF"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        ValeIngresso = false,
                        Imagem = bd.LerString("ImagemInternet"),
                        Latitude = bd.LerString("Latitude"),
                        Longitude = bd.LerString("Longitude")
                    });
                }

                return ingressos;
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

        private void RegistrarImpressaoInternet(int vendaBilheteriaID, int clienteID)
        {
            try
            {
                List<EstruturaRegistroImpressaoVir> ValeIngressos = new List<EstruturaRegistroImpressaoVir>();
                EstruturaRegistroImpressaoVir EstruturaVale = new EstruturaRegistroImpressaoVir();
                EstruturaLogistica estruturaLogistica = new EstruturaLogistica();

                //Cria o item de tipo anonimo
                var item = new
                {
                    ID = 0,
                    Status = "V",
                    QuantidadeImpressoes = 0,
                    ValeIngress = false,
                };

                //Forma a lista do tipo anonimo
                var items = AnonymousList.ToAnonymousList(item);

                bd.Consulta(
                   string.Format(@"SELECT i.ID, i.Status, vb.QuantidadeImpressoesInternet 
						FROM tVendaBilheteria vb (NOLOCK)
						INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
						LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
						LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID 
							WHERE vb.ID = {0} 
									AND vb.ClienteID = {1} 
									AND e.PermitirImpressaoInternet = 'T'
									AND vb.Status = '{2}' ", vendaBilheteriaID, clienteID, VendaBilheteria.PAGO));

                if (bd.Consulta().Read())
                {
                    do
                    {
                        items.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            Status = bd.LerString("Status"),
                            QuantidadeImpressoes = bd.LerInt("QuantidadeImpressoesInternet"),
                            ValeIngress = false,
                        });

                    } while (bd.Consulta().Read());
                }
                else
                {
                    bd.Consulta(
                        string.Format(@"SELECT V.ID, V.Status, vb.QuantidadeImpressoesInternet , ISNULL(V.ClienteNome, '--') AS ClientePresenteado,
							V.CodigoTroca, Vi.CodigoTrocaFixo, V.DataExpiracao, Vi.ValidadeDiasImpressao, e.Nome as TaxaEntrega , 
							C.Nome as Cliente, C.Email, vb.Senha , e.Tipo
							FROM tVendaBilheteria vb (NOLOCK)
							INNER JOIN tCliente C (NOLOCK) ON vb.ClienteID = C.ID
							INNER JOIN tValeIngresso V (NOLOCK) ON V.VendaBilheteriaID = vb.ID
							INNER JOIN tValeIngressoTipo Vi (NOLOCK) ON Vi.ID = V.ValeIngressoTipoID
							LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
							LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID  
							WHERE vb.ID = {0} 
									AND vb.ClienteID = {1} 
									AND e.PermitirImpressaoInternet = 'T'
									AND vb.Status = '{2}' ", vendaBilheteriaID, clienteID, VendaBilheteria.PAGO));

                    if (!bd.Consulta().Read())
                        throw new Exception("Não foi possível encontrar os ingressos da compra informada, por favor tente novamente.");

                    do
                    {
                        items.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            Status = bd.LerString("Status"),
                            QuantidadeImpressoes = bd.LerInt("QuantidadeImpressoesInternet"),
                            ValeIngress = true,
                        });

                        EstruturaVale.ClientePresenteado = bd.LerString("ClientePresenteado");
                        EstruturaVale.CodigoTroca = bd.LerString("CodigoTroca");
                        EstruturaVale.CodigoTrocaFixo = (bd.LerString("CodigoTrocaFixo").Length > 0 ? true : false);
                        EstruturaVale.DataExpiracao = bd.LerDateTime("DataExpiracao").ToString("dd/MM/yyyy");
                        EstruturaVale.DiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                        EstruturaVale.StatusAtual = ValeIngresso.enumStatus.Vendido;
                        EstruturaVale.valeIngressoID = bd.LerInt("ID");

                        ValeIngressos.Add(EstruturaVale);

                        estruturaLogistica.TaxaEntrega = bd.LerString("TaxaEntrega");
                        estruturaLogistica.Cliente = bd.LerString("Cliente");
                        estruturaLogistica.Email = bd.LerString("Email");
                        estruturaLogistica.Senha = bd.LerString("Senha");

                        string tipoEntrega = bd.LerString("Tipo");

                        if (tipoEntrega == "E")
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Entrega;
                        else if (tipoEntrega == "R")
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Retirada;
                        else
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Nenhum;

                        estruturaLogistica.VendaBilheteriaID = vendaBilheteriaID;

                        ImprimiValeIngresso = true;

                    } while (bd.Consulta().Read());
                }

                if (items.Select(c => c.QuantidadeImpressoes).FirstOrDefault() > Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMaximaImpressoesInternet"]))
                    throw new Exception("ATENÇÃO, Não será possivél efetuar a impressão, você excedeu o limite de impressões.");

                Bilheteria bil = new Bilheteria();

                if (items.Where(c => c.ValeIngress).Count() == 0)
                {
                    //Verifica se esta é uma Reimpressão
                    if (items.Where(c => c.Status != Ingresso.VENDIDO).Count() > 0)
                        bil.RegistrarReimpressao(items.Select(c => c.ID).ToArray(),
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            Bilheteria.INTERNET_MOTIVO_REIMPRESSAO,
                            items.Select(c => c.QuantidadeImpressoes).FirstOrDefault() + 1,
                            IRLib.Usuario.INTERNET_USUARIO_ID, false);
                    else
                        bil.RegistrarImpressao(
                            items.Select(c => c.ID).ToArray(),
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            false,
                            1, null,
                            IRLib.Usuario.INTERNET_USUARIO_ID, false);
                }
                else
                {
                    //Verifica se esta é uma Reimpressão
                    if (items.Where(c => c.Status != Ingresso.VENDIDO).Count() > 0)
                    {
                        bil.RegistrarImpressaoValeIngresso(
                            ValeIngressos,
                            vendaBilheteriaID,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            true,
                            null,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            estruturaLogistica);
                    }
                    else
                    {
                        bil.RegistrarImpressaoValeIngresso(
                            ValeIngressos,
                            vendaBilheteriaID,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            false,
                            null,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            estruturaLogistica);
                    }
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
        }

        private void RegistrarImpressaoInternet(string Senha, int clienteID)
        {
            try
            {
                List<EstruturaRegistroImpressaoVir> ValeIngressos = new List<EstruturaRegistroImpressaoVir>();
                EstruturaRegistroImpressaoVir EstruturaVale = new EstruturaRegistroImpressaoVir();
                EstruturaLogistica estruturaLogistica = new EstruturaLogistica();

                //Cria o item de tipo anonimo
                var item = new
                {
                    ID = 0,
                    Status = "V",
                    QuantidadeImpressoes = 0,
                    ValeIngress = false,
                };

                //Forma a lista do tipo anonimo
                var items = AnonymousList.ToAnonymousList(item);

                bd.Consulta(
                   string.Format(@"SELECT i.ID, i.Status, vb.QuantidadeImpressoesInternet 
						FROM tVendaBilheteria vb (NOLOCK)
						INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
						LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
						LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID 
							WHERE vb.Senha = '{0}' 
									AND vb.ClienteID = {1} 
									AND e.PermitirImpressaoInternet = 'T'
									AND vb.Status = '{2}' ", Senha, clienteID, VendaBilheteria.PAGO));

                if (bd.Consulta().Read())
                {
                    do
                    {
                        items.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            Status = bd.LerString("Status"),
                            QuantidadeImpressoes = bd.LerInt("QuantidadeImpressoesInternet"),
                            ValeIngress = false,
                        });

                    } while (bd.Consulta().Read());
                }
                else
                {
                    bd.Consulta(
                        string.Format(@"SELECT V.ID, V.Status, vb.QuantidadeImpressoesInternet , ISNULL(V.ClienteNome, '--') AS ClientePresenteado,
							V.CodigoTroca, Vi.CodigoTrocaFixo, V.DataExpiracao, Vi.ValidadeDiasImpressao, e.Nome as TaxaEntrega , 
							C.Nome as Cliente, C.Email, e.Tipo, vb.ID as VendaBilheteriaID
							FROM tVendaBilheteria vb (NOLOCK)
							INNER JOIN tCliente C (NOLOCK) ON vb.ClienteID = C.ID
							INNER JOIN tValeIngresso V (NOLOCK) ON V.VendaBilheteriaID = vb.ID
							INNER JOIN tValeIngressoTipo Vi (NOLOCK) ON Vi.ID = V.ValeIngressoTipoID
							LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
							LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID  
							WHERE vb.Senha = '{0}' 
									AND vb.ClienteID = {1} 
									AND e.PermitirImpressaoInternet = 'T'
									AND vb.Status = '{2}' ", Senha, clienteID, VendaBilheteria.PAGO));

                    if (!bd.Consulta().Read())
                        throw new Exception("Não foi possível encontrar os ingressos da compra informada, por favor tente novamente.");

                    do
                    {
                        items.Add(new
                        {
                            ID = bd.LerInt("ID"),
                            Status = bd.LerString("Status"),
                            QuantidadeImpressoes = bd.LerInt("QuantidadeImpressoesInternet"),
                            ValeIngress = true,
                        });

                        EstruturaVale.ClientePresenteado = bd.LerString("ClientePresenteado");
                        EstruturaVale.CodigoTroca = bd.LerString("CodigoTroca");
                        EstruturaVale.CodigoTrocaFixo = (bd.LerString("CodigoTrocaFixo").Length > 0 ? true : false);
                        EstruturaVale.DataExpiracao = bd.LerDateTime("DataExpiracao").ToString("dd/MM/yyyy");
                        EstruturaVale.DiasImpressao = bd.LerInt("ValidadeDiasImpressao");
                        EstruturaVale.StatusAtual = ValeIngresso.enumStatus.Vendido;
                        EstruturaVale.valeIngressoID = bd.LerInt("ID");

                        ValeIngressos.Add(EstruturaVale);

                        estruturaLogistica.TaxaEntrega = bd.LerString("TaxaEntrega");
                        estruturaLogistica.Cliente = bd.LerString("Cliente");
                        estruturaLogistica.Email = bd.LerString("Email");
                        estruturaLogistica.Senha = Senha;

                        string tipoEntrega = bd.LerString("Tipo");

                        if (tipoEntrega == "E")
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Entrega;
                        else if (tipoEntrega == "R")
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Retirada;
                        else
                            estruturaLogistica.TaxaEntregaTipo = Enumeradores.TaxaEntregaTipo.Nenhum;

                        estruturaLogistica.VendaBilheteriaID = bd.LerInt("VendaBilheteriaID");

                        ImprimiValeIngresso = true;

                    } while (bd.Consulta().Read());
                }

                if (items.Select(c => c.QuantidadeImpressoes).FirstOrDefault() > Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMaximaImpressoesInternet"]))
                    throw new Exception("ATENÇÃO, Não será possivél efetuar a impressão, você excedeu o limite de impressões.");

                Bilheteria bil = new Bilheteria();

                if (items.Where(c => c.ValeIngress).Count() == 0)
                {
                    //Verifica se esta é uma Reimpressão
                    if (items.Where(c => c.Status != Ingresso.VENDIDO).Count() > 0)
                        bil.RegistrarReimpressao(items.Select(c => c.ID).ToArray(),
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            Bilheteria.INTERNET_MOTIVO_REIMPRESSAO,
                            items.Select(c => c.QuantidadeImpressoes).FirstOrDefault() + 1,
                            IRLib.Usuario.INTERNET_USUARIO_ID, false);
                    else
                        bil.RegistrarImpressao(
                            items.Select(c => c.ID).ToArray(),
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            false,
                            1, null,
                            IRLib.Usuario.INTERNET_USUARIO_ID, false);
                }
                else
                {
                    //Verifica se esta é uma Reimpressão
                    if (items.Where(c => c.Status != Ingresso.VENDIDO).Count() > 0)
                    {
                        bil.RegistrarImpressaoValeIngresso(
                            ValeIngressos,
                            estruturaLogistica.VendaBilheteriaID,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            true,
                            null,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            estruturaLogistica);
                    }
                    else
                    {
                        bil.RegistrarImpressaoValeIngresso(
                            ValeIngressos,
                            estruturaLogistica.VendaBilheteriaID,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            Bilheteria.INTERNET_EMPRESA_ID,
                            bil.VerificaCaixaInternet(),
                            Canal.CANAL_INTERNET,
                            Loja.INTERNET_LOJA_ID,
                            false,
                            null,
                            IRLib.Usuario.INTERNET_USUARIO_ID,
                            estruturaLogistica);
                    }
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

                Bilheteria bil = new Bilheteria();

                return bil.RegistrarImpressao(items.Select(c => c.ID).ToArray(), IRLib.Usuario.INTERNET_USUARIO_ID, Bilheteria.INTERNET_EMPRESA_ID, bil.VerificaCaixaInternet(), Canal.CANAL_INTERNET, Loja.INTERNET_LOJA_ID, false, 1, null, IRLib.Usuario.INTERNET_USUARIO_ID, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IngressoImpressao> BuscarCompra(string VendabilheteriaID)
        {
            List<IngressoImpressao> _List = new List<IngressoImpressao>();

            try
            {
                string sql = string.Format(@"SELECT ti.ID, tvb.Senha, dbo.DataHoraFormatada(tvb.DataVenda) AS DataVenda, te.Nome AS Evento, dbo.DataHoraFormatada(ta.Horario) AS Apresentacao, ts.Nome AS Setor, ti.Codigo,
                                             ti.CodigoBarra, tp.Valor, tp.Valor AS Preco, tl.Nome AS Local, ts.Acesso, tl.Endereco, tl.Numero, tl.Bairro, tl.CEP, tl.Cidade, tl.Estado, tc.EnderecoCliente, tc.NumeroCliente,
                                             tc.ComplementoCliente, tc.BairroCliente, tc.EstadoCliente, tc.CidadeCliente, tc.CEPCliente, tc.Email, tc.DDDTelefone, tc.Telefone, tc.DDDCelular, tc.Celular,
                                             tc.Nome AS ClienteNome, tc.CPF, tfp.Nome as FormaPagamento, tvb.TaxaConvenienciaValorTotal AS ConvenienciaTotal, te.ID AS EventoID, ten.ID AS EntregaID
                                             FROM tIngresso ti (NOLOCK)
                                             INNER JOIN tVendaBilheteria tvb (NOLOCK)ON tvb.ID = ti.VendaBilheteriaID
                                             INNER JOIN tCliente tc (NOLOCK)ON tc.ID = tvb.ClienteID
                                             INNER JOIN tEvento te (NOLOCK)ON te.ID = ti.EventoID
                                             INNER JOIN tApresentacao ta (NOLOCK)ON ta.ID = ti.ApresentacaoID
                                             INNER JOIN tSetor ts (NOLOCK)ON ts.ID = ti.SetorID
                                             INNER JOIN tPreco tp (NOLOCK)ON tp.ID = ti.PrecoID
                                             INNER JOIN tLocal tl (NOLOCK)ON tl.ID = te.LocalID
                                             INNER JOIN tVendaBilheteriaFormaPagamento tvbfp (NOLOCK)ON tvbfp.VendaBilheteriaID = tvb.ID
                                             INNER JOIN tFormaPagamento tfp (NOLOCK)ON tfp.ID = tvbfp.FormaPagamentoID
                                             INNER JOIN tEntregaControle tec (NOLOCK)ON tec.ID = tvb.EntregaControleID
                                             INNER JOIN tEntrega ten (NOLOCK)ON ten.ID = tec.EntregaID
                                             WHERE ti.VendaBilheteriaID = {0}", VendabilheteriaID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    _List.Add(new IngressoImpressao()
                    {
                        ID = bd.LerInt("ID"),
                        Senha = bd.LerString("Senha"),
                        DataVenda = bd.LerString("DataVenda"),
                        Evento = bd.LerString("Evento"),
                        Apresentacao = bd.LerString("Apresentacao"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        CodigoBarra = bd.LerString("CodigoBarra"),
                        Valor = bd.LerDecimal("Valor"),
                        Preco = bd.LerString("Preco"),
                        Local = bd.LerString("Local"),
                        Acesso = bd.LerString("Acesso"),
                        EnderecoLocal = bd.LerString("Endereco"),
                        NumeroLocal = bd.LerString("Numero"),
                        BairroLocal = bd.LerString("Bairro"),
                        CepLocal = bd.LerString("Local"),
                        CidadeLocal = bd.LerString("Cidade"),
                        EstadoLocal = bd.LerString("Estado"),
                        EnderecoCliente = bd.LerString("EnderecoCliente"),
                        NumeroCliente = bd.LerString("NumeroCliente"),
                        ComplementoCliente = bd.LerString("ComplementoCliente"),
                        BairroCliente = bd.LerString("BairroCliente"),
                        EstadoCliente = bd.LerString("EstadoCliente"),
                        CidadeCliente = bd.LerString("CidadeCliente"),
                        CepCliente = bd.LerString("CEPCliente"),
                        Email = bd.LerString("Email"),
                        DDDTelefone = bd.LerString("DDDTelefone"),
                        Telefone = bd.LerString("Telefone"),
                        DDDCelular = bd.LerString("DDDCelular"),
                        Celular = bd.LerString("Celular"),
                        ClienteNome = bd.LerString("ClienteNome"),
                        ClienteCPFCNPJ = bd.LerString("CPF"),
                        FormaPagamento = bd.LerString("FormaPagamento"),
                        ValorConveniencia = bd.LerDecimal("ConvenienciaTotal"),
                        EventoID = bd.LerInt("EventoID"),
                        EntregaID = bd.LerInt("EntregaID")
                    });
                }

                return _List;
            }

            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                bd.Fechar();
            }

        }


    }

    public class IngressoImpressaoInternet
    {
        public int Indice { get; set; }
        public int IngressoID { get; set; }
        public string Evento { get; set; }
        public string Apresentacao { get; set; }
        public string Setor { get; set; }
        public string Codigo { get; set; }
        public string Valor { get; set; }
        public string Preco { get; set; }
        public string DataVenda { get; set; }
        public string Local { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Senha { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteCPFCNPJ { get; set; }
        public string CodigoBarra { get; set; }
        public bool ValeIngresso { get; set; }
        public string Conveniencia { get; set; }
        public string Imagem { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class ValeIngressoImpressaoInternet
    {
        public int Indice { get; set; }
        public string Senha { get; set; }
        public string DataValidade { get; set; }
        public string Valor { get; set; }
        public char ValorTipo { get; set; }
        public string CodigoTroca { get; set; }
        public string CodigoBarra { get; set; }
        public string ClienteNome { get; set; }
        public string ProcedimentoTroca { get; set; }
        public string Saudacao { get; set; }
        public string Codigo { get; set; }
        public bool ValeIngresso { get; set; }
        public decimal ValorPagamento { get; set; }
    }
}
