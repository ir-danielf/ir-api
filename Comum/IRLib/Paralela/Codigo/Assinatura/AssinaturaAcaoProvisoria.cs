/**************************************************
* Arquivo: AssinaturaAcaoProvisoria.cs
* Gerado: 23/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects.Assinaturas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{

    public class AssinaturaAcaoProvisoria : AssinaturaAcaoProvisoria_B
    {
        public AssinaturaAcaoProvisoria() { }

        public void AdicionarAcoes(BD bd, int ClienteID, int Entrega, List<EstruturaAssinaturaAcao> Items, int usuarioID)
        {

            this.ExcluirAcoes(bd, ClienteID);

            foreach (var item in Items)
            {
                this.Limpar();
                this.AssinaturaClienteID.Valor = Convert.ToInt32(item.AssinaturaClienteID);
                this.ClienteID.Valor = ClienteID;
                this.PrecoTipoID.Valor = Convert.ToInt32(item.PrecoTipo);
                this.EntregaID.Valor = Entrega;
                this.Acao.Valor = Convert.ToString((char)AssinaturaCliente.ToAcao(item.Acao));
                this.Processado.Valor = false;
                this.AgregadoID.Valor = item.AgregadoID;
                this.Inserir(bd);
            }

        }
        public void AdicionarAcoes(int ClienteID, int Entrega, List<EstruturaAssinaturaAcao> Items, int usuarioID)
        {
            try
            {
                this.ExcluirAcoes(ClienteID);

                //AssinaturaHistorico oHistorico = new AssinaturaHistorico();

                foreach (var item in Items)
                {
                    this.Limpar();
                    this.AssinaturaClienteID.Valor = Convert.ToInt32(item.AssinaturaClienteID);
                    this.ClienteID.Valor = ClienteID;
                    this.PrecoTipoID.Valor = Convert.ToInt32(item.PrecoTipo);
                    this.EntregaID.Valor = Entrega;
                    this.AgregadoID.Valor = item.AgregadoID;
                    this.Acao.Valor = Convert.ToString((char)AssinaturaCliente.ToAcao(item.Acao));
                    this.Processado.Valor = false;
                    this.AgregadoID.Valor = item.AgregadoID;
                    this.Inserir();
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AdicionarAcoesTroca(int clienteID, int Entrega, List<EstruturaAssinaturaAcao> Items,
                                        List<Assinaturas.Models.Assinatura> assinaturas, int usuarioID, int vendaBilheteriaID)
        {
            try
            {
                foreach (var acao in Items)
                {
                    var assinatura = assinaturas.Where(c => c.AssinaturaClienteID == Convert.ToInt32(acao.AssinaturaClienteID)).FirstOrDefault();
                    if (assinatura == null)
                        throw new Exception("Não foi possível encontrar uma das assinaturas associadas a você.");

                    switch (assinatura.Acao)
                    {
                        case AssinaturaCliente.EnumAcao.AguardandoAcao:
                            break;
                        case AssinaturaCliente.EnumAcao.Renovar:
                            acao.PrecoTipo = assinatura.PrecoTipoID.ToString();
                            acao.Acao = "renovar";
                            break;
                        case AssinaturaCliente.EnumAcao.Desisistir:
                            acao.Acao = "desistir";
                            break;
                        case AssinaturaCliente.EnumAcao.Trocar:
                            switch (acao.Acao.ToLower())
                            {
                                case "desistir":
                                    acao.PrecoTipo = "0";
                                    break;
                                case "renovar":
                                    break;
                                default:
                                    throw new Exception("Aparentemente uma das ações não foi selecionada, por favor tente novamente.");
                            }
                            break;
                        case AssinaturaCliente.EnumAcao.EfetivarTroca:
                        case AssinaturaCliente.EnumAcao.Aquisicao:
                            acao.Acao = "eftivartroca";
                            break;
                        default:
                            break;
                    }

                    assinaturas.Remove(assinatura);
                }

                //deixa as assinaturas que foram trocadas no "Efetivar" como desistencia
                foreach (var assinatura in assinaturas)
                {
                    Items.Add(new EstruturaAssinaturaAcao()
                    {
                        Acao = "desistir",
                        AssinaturaClienteID = assinatura.AssinaturaClienteID.ToString(),
                        PrecoTipo = "0",
                    });
                }

                this.AdicionarAcoes(clienteID, Entrega, Items, usuarioID);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Paralela.Assinaturas.Models.AcaoProvisoria> BuscarLista(int ClienteID, bool processado)
        {
            try
            {
                string sql =
                    string.Format(@" 
						SELECT 
							ac.ID AS AssinaturaClienteID,
							a.Nome AS Assinatura,
							s.Nome AS Setor,
							l.Codigo AS Codigo,
							ap.Acao, ac.Status,
                            ap.agregadoID as AgregadoID,
							SUM(IsNull(CASE WHEN (ap.Acao = 'R' OR ap.Acao = 'N' OR ap.Acao = 'E') AND pt.ID = ai.PrecoTipoID AND pt.ID = ap.PrecoTipoID
								THEN p.Valor
								ELSE 0
							END, 0)) AS Valor,
							ap.PrecoTipoID, ap.EntregaID,
                            pt.Nome AS PrecoTipo
						FROM tAssinaturaAcaoProvisoria ap (NOLOCK)
						INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = ap.AssinaturaClienteID
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
						INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
						INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = ac.AssinaturaAnoID AND ai.SetorID = ac.SetorID
						LEFT JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = ap.PrecoTipoID
						LEFT JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ai.ApresentacaoID AND aps.SetorID = ai.SetorID
						LEFT JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = aps.ID AND p.PrecoTipoID = ap.PrecoTipoID
							WHERE ap.ClienteID = {0} AND ap.Processado = '{1}'
						GROUP BY ac.ID, a.Nome, s.Nome, l.Codigo, ap.Acao, ac.Status, ap.PrecoTipoID, ap.EntregaID, pt.Nome, ap.agregadoid
						ORDER BY a.Nome, s.Nome, l.Codigo
						", ClienteID, processado ? "T" : "F");

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar ações a serem exibidas.");

                List<IRLib.Paralela.Assinaturas.Models.AcaoProvisoria> lista = new List<IRLib.Paralela.Assinaturas.Models.AcaoProvisoria>();
                do
                {
                    lista.Add(new IRLib.Paralela.Assinaturas.Models.AcaoProvisoria()
                    {
                        AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                        Assinatura = bd.LerString("Assinatura"),
                        Setor = bd.LerString("Setor"),
                        Lugar = bd.LerString("Codigo"),
                        Acao = (IRLib.Paralela.AssinaturaCliente.EnumAcao)Convert.ToChar(bd.LerString("Acao")),
                        Valor = bd.LerDecimal("Valor"),
                        PrecoTipoID = bd.LerInt("PrecoTipoID"),
                        Status = (IRLib.Paralela.AssinaturaCliente.EnumStatus)Convert.ToChar(bd.LerString("Status")),
                        EntregaID = bd.LerInt("EntregaID"),
                        PrecoTipo = bd.LerString("PrecoTipo"),
                        AgregadoID = bd.LerInt("AgregadoID")
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ExcluirAcoes(int clienteID)
        {
            try
            {
                bd.Executar("DELETE FROM tAssinaturaAcaoProvisoria WHERE ClienteID = " + clienteID);
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ExcluirAcoes(BD bd, int clienteID)
        {
            bd.Executar("DELETE FROM tAssinaturaAcaoProvisoria WHERE ClienteID = " + ClienteID);
        }

        public void ExecutarAcoes(BD bd, int clienteID)
        {
            bd.Executar("UPDATE tAssinaturaAcaoProvisoria SET Processado = 'T' WHERE ClienteID = " + clienteID);
        }
    }

    public class AssinaturaAcaoProvisoriaLista : AssinaturaAcaoProvisoriaLista_B
    {
        public AssinaturaAcaoProvisoriaLista() { }
    }

}
