using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Assinaturas.Relatorios
{
    public class ListagemBloqueios
    {

        BD bd;

        public ListagemBloqueios()
        {
            bd = new BD();
        }

        public IRLib.Assinatura.EnumStatusVisual VerificaStatusVisual(string statusIngresso, string statusAssinatura, string acaoAssinatura, int ingressoBloqueioID, int assinaturaBloqueioID, int assinaturaExtintoBloqueioID, int assinaturaDesistenciaBloqueioID)
        {
            try
            {
                IRLib.Assinatura.EnumStatusVisual statusRetorno;

                if ((Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.RESERVADO)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Reservando;
                }
                else if ((Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.DISPONIVEL)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Disponivel;
                }
                else if ((statusAssinatura == "" || (AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel) && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (acaoAssinatura == "" || (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Desisistir) && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Extinto;
                }
                else if (statusAssinatura == "" && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && acaoAssinatura == "" && ingressoBloqueioID != assinaturaExtintoBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Bloqueado;
                }
                else if (statusAssinatura == "" && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) != Ingresso.StatusIngresso.BLOQUEADO && acaoAssinatura == "")
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Indisponivel;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aguardando && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.AguardandoAcao && ingressoBloqueioID == assinaturaBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.AguardandoAcao;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.AguardandoAcao && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.AguardandoAcaoExtinto;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Trocar && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.AguardandoAcaoPrioritaria;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Desistido && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Desisistir && ingressoBloqueioID == assinaturaDesistenciaBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.AguardandoAcaoAdministrativa;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.TrocaSinalizada && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Trocar && ingressoBloqueioID == assinaturaBloqueioID)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.TrocaSinalizada;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Renovado;

                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.RenovadoSemPagamento && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.RenovadoSemPagamento;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Comprado;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca)
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.TrocaEfetuada;
                }
                else if (((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca))
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Emitido;

                }
                else if (((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca))
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Entregue;

                }
                else
                {
                    statusRetorno = IRLib.Assinatura.EnumStatusVisual.Indisponivel;
                }

                return statusRetorno;
            }
            catch
            {
                //bd.Fechar();
                //throw;
            }

            return IRLib.Assinatura.EnumStatusVisual.AguardandoAcao;
        }


        public List<IRLib.Assinaturas.Models.ListagemBloqueios> BuscarRelatorio(int AssinaturaTipoID, int Temporadas, int Assinaturas)
        {
            try
            {

                string sql = @"SELECT DISTINCT i.Grupo, i.LugarID, i.Codigo AS Lugar,
                                i.Status as StatusIngresso, i.BloqueioID as IngressoBloqueioID,
                                ac.id as AssinaturaClienteID,ISNULL(ac.Status,'') as StatusAssinatura , ISNULL(ac.Acao,'') as AcaoAssinatura , 
                                ass.Nome as NomeAssinatura, ass.BloqueioID AssinaturaBloqueioID, ass.DesistenciaBloqueioID AssinaturaDesistenciaBloqueioID, ass.ID AS AssinaturaID,
                                ass.ExtintoBloqueioID as AssinaturaExtintoBloqueioID,
                                s.ID as SetorID, s.Nome AS Setor,
                                bl.Nome as BloqueioUtilizado,
                                aa.ID AS AssinaturaAnoID
                                FROM tAssinatura ass(NOLOCK)
                                INNER JOIN tAssinaturaAno  aa(NOLOCK) on ass.ID = aa.AssinaturaID
                                INNER JOIN tAssinaturaItem ai(NOLOCK) on aa.ID = ai.AssinaturaAnoID
                                INNER JOIN tIngresso i(NOLOCK) on ai.SetorID = i.SetorID and ai.ApresentacaoID = i.ApresentacaoID
                                INNER JOIN tSetor s(NOLOCK) on s.ID = ai.SetorID
                                INNER JOIN tLugar l (NOLOCK) on i.LugarID = l.ID
                                LEFT JOIN tAssinaturaCliente ac(NOLOCK) on ac.AssinaturaAnoID = aa.ID and ai.SetorID = ac.SetorID and ac.LugarID = i.LugarID and ac.AssinaturaID = ass.ID and ac.ID = i.AssinaturaClienteID
                                LEFT JOIN tCliente c(NOLOCK) on c.ID = ac.ClienteID
                                LEFT JOIN tPrecoTipo pt (NOLOCK) ON ac.PrecoTipoID = pt.ID 
                                LEFT JOIN tBloqueio bl (NOLOCK) on bl.ID = i.BloqueioID
                                WHERE aa.AssinaturaID = " + Assinaturas + " AND aa.Ano =  '" + Temporadas + @"' 
                                ORDER BY s.ID, i.Codigo";

                bd.Consulta(sql);

                var lstAux = new List<EstruturaAssinaturaBloqueio>();

                EstruturaAssinaturaBloqueio eABaux = new EstruturaAssinaturaBloqueio();

                while (bd.Consulta().Read())
                {
                    if (lstAux.Where(c => c.LugarID == bd.LerInt("LugarID")).Count() == 0)
                    {
                        string statusAssinatura = bd.LerString("StatusAssinatura") != "" ? ((AssinaturaCliente.EnumStatus)Convert.ToChar(bd.LerString("StatusAssinatura"))).ToString() : "--";
                        string statusIngresso = bd.LerString("StatusIngresso") != "" ? ((Ingresso.StatusIngresso)Convert.ToChar(bd.LerString("StatusIngresso"))).ToString() : "--";


                        eABaux = new EstruturaAssinaturaBloqueio()
                        {
                            LugarID = bd.LerInt("LugarID"),
                            NomeAssinatura = bd.LerString("NomeAssinatura"),
                            Setor = bd.LerString("Setor"),
                            Lugar = bd.LerString("Lugar"),
                            BloqueioUtilizado = bd.LerString("BloqueioUtilizado") != "" ? bd.LerString("BloqueioUtilizado") : "--",
                            StatusAssinatura = statusAssinatura,
                            StatusIngresso = statusIngresso,
                            SetorID = bd.LerInt("SetorID"),
                            AssinaturaAnoID = bd.LerInt("AssinaturaAnoID"),
                            AssinaturaID = bd.LerInt("AssinaturaID"),

                        };

                        eABaux.Status = this.VerificaStatusVisual(bd.LerString("StatusIngresso"), bd.LerString("StatusAssinatura"), bd.LerString("AcaoAssinatura"), bd.LerInt("IngressoBloqueioID"), bd.LerInt("AssinaturaBloqueioID"), bd.LerInt("AssinaturaExtintoBloqueioID"), bd.LerInt("AssinaturaDesistenciaBloqueioID"));


                        lstAux.Add(eABaux);
                    }
                    else
                    {
                        eABaux.Status = IRLib.Assinatura.EnumStatusVisual.Indisponivel;
                        eABaux.StatusIngresso = "--";
                        eABaux.StatusAssinatura = "--";
                        eABaux.BloqueioUtilizado = "--";
                        eABaux.BloqueioID = 0;
                    }
                }

                var lstRetorno = new List<Models.ListagemBloqueios>();

                Models.ListagemBloqueios ModelAux = new Models.ListagemBloqueios();

                foreach (var item in lstAux)
                {
                    ModelAux = new Models.ListagemBloqueios()
                         {
                             Assinatura = item.NomeAssinatura,
                             Setor = item.Setor,
                             Codigo = item.Lugar,
                             Status = item.Status == Assinatura.EnumStatusVisual.Bloqueado? item.BloqueioUtilizado : item.StatusDescricao,
                         };
                    lstRetorno.Add(ModelAux);
                }

                return lstRetorno;

            }
            finally
            {
                bd.Fechar();
            }
        }


    }
}
