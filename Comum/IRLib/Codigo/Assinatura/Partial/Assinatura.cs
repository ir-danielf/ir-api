using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IRLib
{
    public partial class Assinatura
    {

        public enum EnumStatusVisual
        {
            [Description("Aguardando ação")]
            AguardandoAcao = 'A',
            [Description("Aguardando ação administrativa")]
            AguardandoAcaoAdministrativa = 'M',
            [Description("Aguardando ação (extinto)")]
            AguardandoAcaoExtinto = 'J',
            [Description("Aguardando ação prioritária")]
            AguardandoAcaoPrioritaria = 'P',
            Bloqueado = 'B',
            Comprado = 'C',
            [Description("Disponível")]
            Disponivel = 'D',
            Emitido = 'E',
            Entregue = 'N',
            Extinto = 'X',
            [Description("Indisponível")]
            Indisponivel = 'I',
            [Description("Parcialmente pago")]
            ParcialmentePago = 'G',
            Reservando = 'R',
            Renovado = 'O',
            [Description("Renovado sem Pagamento")]
            RenovadoSemPagamento = 'V',
            [Description("Troca Efetuada")]
            TrocaEfetuada = 'F',
            [Description("Troca sinalizada")]
            TrocaSinalizada = 'T'
        }

        public Dictionary<string, string> GetLstStatus()
        {
            var retorno = Utils.Enums.EnumToDictionary<Assinatura.EnumStatusVisual>("S", " Selecione ....");
            retorno.Remove(((char)Assinatura.EnumStatusVisual.ParcialmentePago).ToString());

            return SortDictionary(retorno);
        }

        public Dictionary<string, string> GetLstTipoCancelamento()
        {
            var retorno = Utils.Enums.EnumToDictionary<Assinatura.enumTipoCancelamento>("S", "Selecione ....");
            return retorno;
        }

        public static Dictionary<string, string> SortDictionary(Dictionary<string, string> data)
        {
            var sortedDict = (from entry in data orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDict;
        }

        public Dictionary<string, string> GetLstStatus(List<Assinatura.EnumStatusVisual> lstNecessito, bool todos)
        {
            var retornoAux = Utils.Enums.EnumToDictionary<Assinatura.EnumStatusVisual>("S", "Selecione....");
            Dictionary<string, string> retorno = new Dictionary<string, string>();

            if (todos)
                retorno.Add("0", "Todos");

            foreach (var status in retornoAux)
                if (lstNecessito.Contains((Assinatura.EnumStatusVisual)Convert.ToChar(status.Key)))
                    retorno.Add(status.Key, status.Value);
            return retorno;
        }

        public static string DesmembrarStatus(EnumStatusVisual? status)
        {
            string retorno = "";
            switch (status)
            {
                case EnumStatusVisual.AguardandoAcaoAdministrativa:
                    retorno = " AND (  ac.Status = 'D' AND ac.Acao= 'D' AND i.Status= 'B' AND i.BloqueioID = ass.DesistenciaBloqueioID )";
                    break;
                case EnumStatusVisual.Comprado:
                    retorno = " AND (  ac.Status = 'R' AND ac.Acao = 'R' AND i.Status = 'V' ) OR (  ac.Status = 'S' AND ac.Acao = 'R' AND i.Status = 'V' ) OR (  ac.Status = 'N' AND ac.Acao = 'N' AND i.Status = 'V' ) OR (  ac.Status = 'T' AND ac.Acao = 'T' AND i.Status = 'V' ) ";
                    break;
                case EnumStatusVisual.Emitido:
                    retorno = " AND (  ac.Status = 'R' AND ac.Acao = 'R' AND i.Status = 'I' ) OR  (  ac.Status = 'N' AND ac.Acao = 'N' AND i.Status = 'V' ) OR (  ac.Status = 'T' AND ac.Acao = 'T' AND i.Status = 'V' ) ";
                    break;
                case EnumStatusVisual.Entregue:
                    retorno = " AND (  ac.Status = 'R' AND ac.Acao = 'R' AND i.Status = 'E' ) OR  (  ac.Status = 'N' AND ac.Acao = 'N' AND i.Status = 'V' ) OR (  ac.Status = 'T' AND ac.Acao = 'T' AND i.Status = 'V' ) ";
                    break;
                case EnumStatusVisual.Extinto:
                    retorno = " AND (  (ac.Status = '' OR ac.Status IS NULL) AND (ac.Acao = '' OR ac.Acao IS NULL) AND i.Status= 'B' AND i.BloqueioID = ass.ExtintoBloqueioID )";
                    break;
                case EnumStatusVisual.AguardandoAcao:
                    retorno = " AND (  ac.Status = 'A' AND ac.Acao= 'A' AND i.Status = 'B' AND i.BloqueioID = ass.BloqueioID )";
                    break;
                case EnumStatusVisual.AguardandoAcaoExtinto:
                    retorno = " AND (  ac.Status = 'I' AND ac.Acao= 'A' AND i.Status = 'B' AND i.BloqueioID = ass.ExtintoBloqueioID )";
                    break;
                case EnumStatusVisual.AguardandoAcaoPrioritaria:
                    retorno = " AND (  ac.Status = 'I' AND ac.Acao='T' AND i.Status = 'B' AND i.BloqueioID = ass.ExtintoBloqueioID )";
                    break;
                case EnumStatusVisual.Bloqueado:
                    retorno = " AND (  (ac.Status = '' OR ac.Status IS NULL) AND (ac.Acao= '' OR ac.Acao IS NULL) AND i.Status= 'B' AND ( i.BloqueioID <> ass.ExtintoBloqueioID AND i.BloqueioID <> ass.DesistenciaBloqueioID ))";
                    break;
                case EnumStatusVisual.Disponivel:
                    retorno = " AND (  (ac.Status = '' OR ac.Status IS NULL) AND (ac.Acao= '' OR ac.Acao IS NULL) AND i.Status= 'D' )";
                    break;
                case EnumStatusVisual.Reservando:

                    retorno = " AND (  (ac.Status = '' OR ac.Status IS NULL) AND (ac.Acao= '' OR ac.Acao IS NULL) AND i.Status= 'R' )";
                    break;
                case EnumStatusVisual.TrocaSinalizada:
                    retorno = " AND (  ac.Status = 'Z' AND ac.Acao= 'T' AND i.Status= 'B' )";
                    break;
                default:
                    break;
            }


            return retorno;
        }

        public List<EstruturaAssinaturaBloqueio> Busca(int AssinaturaID, int SetorID, Assinatura.EnumStatusVisual? status, string Lugar, string Assinante, int AnoID, bool comAssinantes, bool semAssinantes)
        {

            try
            {
                var filtro = this.MontaFiltroAssinatura(SetorID, Lugar, Assinante, comAssinantes, semAssinantes);

                string sql = @"SELECT DISTINCT i.Grupo,i.Classificacao, i.LugarID, i.Codigo AS Lugar,
                                i.Status as StatusIngresso, i.BloqueioID as IngressoBloqueioID,
                                ac.id as AssinaturaClienteID,ISNULL(ac.Status,'') as StatusAssinatura , ISNULL(ac.Acao,'') as AcaoAssinatura , 
                                ISNULL(c.Nome,'') AS Assinante,ISNULL(c.ID,0) AS ClienteID, ISNULL(c.CPF,'') as CPF ,
                                ass.Nome as NomeAssinatura, ass.BloqueioID AssinaturaBloqueioID, ass.DesistenciaBloqueioID AssinaturaDesistenciaBloqueioID, ass.ID AS AssinaturaID,
                                ass.ExtintoBloqueioID as AssinaturaExtintoBloqueioID,
                                s.ID as SetorID, s.Nome AS Setor,
                                ISNULL(pt.Nome,'') AS Preco, 
                                bl.Nome as BloqueioUtilizado, bl.CorID AS BloqueioCorID,
                                l.PosicaoY, l.PosicaoX,  
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
                                WHERE aa.AssinaturaID = " + AssinaturaID + " AND ai.AssinaturaAnoID =  " + AnoID + filtro + @"
                                ORDER BY i.Codigo";

                bd.Consulta(sql);

                var lstRetorno = new List<EstruturaAssinaturaBloqueio>();

                EstruturaAssinaturaBloqueio eABaux = new EstruturaAssinaturaBloqueio();

                while (bd.Consulta().Read())
                {
                    if (lstRetorno.Where(c => c.LugarID == bd.LerInt("LugarID")).Count() == 0)
                    {
                        string statusAssinatura = bd.LerString("StatusAssinatura") != "" ? ((AssinaturaCliente.EnumStatus)Convert.ToChar(bd.LerString("StatusAssinatura"))).ToString() : "--";
                        string statusIngresso = bd.LerString("StatusIngresso") != "" ? ((Ingresso.StatusIngresso)Convert.ToChar(bd.LerString("StatusIngresso"))).ToString() : "--";


                        eABaux = new EstruturaAssinaturaBloqueio()
                        {
                            LugarID = bd.LerInt("LugarID"),
                            NomeAssinatura = bd.LerString("NomeAssinatura"),
                            Assinante = bd.LerString("Assinante"),
                            CPF = bd.LerString("CPF"),
                            Setor = bd.LerString("Setor"),
                            Lugar = bd.LerString("Lugar"),
                            Preco = bd.LerString("Preco"),
                            ClienteID = bd.LerInt("ClienteID"),
                            AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                            AssinaturaBloqueioID = bd.LerInt("AssinaturaBloqueioID"),
                            BloqueioUtilizado = bd.LerString("BloqueioUtilizado") != "" ? bd.LerString("BloqueioUtilizado") : "--",
                            StatusAssinatura = statusAssinatura,
                            StatusIngresso = statusIngresso,
                            SetorID = bd.LerInt("SetorID"),
                            Selecionar = false,
                            PosicaoX = bd.LerInt("PosicaoX"),
                            PosicaoY = bd.LerInt("PosicaoY"),
                            BloqueioID = bd.LerInt("IngressoBloqueioID"),
                            BloqueioCorID = bd.LerInt("BloqueioCorID"),
                            AssinaturaAnoID = bd.LerInt("AssinaturaAnoID"),
                            AssinaturaID = bd.LerInt("AssinaturaID"),
                            AssinaturaDesistenciaID = bd.LerInt("AssinaturaDesistenciaBloqueioID"),
                            AssinaturaExtintoID = bd.LerInt("AssinaturaExtintoBloqueioID"),
                        };

                        eABaux.Status = this.VerificaStatusVisual(bd.LerString("StatusIngresso"), bd.LerString("StatusAssinatura"), bd.LerString("AcaoAssinatura"), bd.LerInt("IngressoBloqueioID"), bd.LerInt("AssinaturaBloqueioID"), bd.LerInt("AssinaturaExtintoBloqueioID"), bd.LerInt("AssinaturaDesistenciaBloqueioID"));


                        lstRetorno.Add(eABaux);
                    }
                    else
                    {
                        eABaux.Status = EnumStatusVisual.Indisponivel;
                        eABaux.StatusIngresso = "--";
                        eABaux.StatusAssinatura = "--";
                        eABaux.BloqueioUtilizado = "--";
                        eABaux.BloqueioID = 0;
                    }
                }

                if (status != null)
                    switch (status)
                    {
                        case EnumStatusVisual.AguardandoAcao:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.AguardandoAcao);
                        case EnumStatusVisual.AguardandoAcaoAdministrativa:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.AguardandoAcaoAdministrativa);
                        case EnumStatusVisual.AguardandoAcaoExtinto:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.AguardandoAcaoExtinto);
                        case EnumStatusVisual.AguardandoAcaoPrioritaria:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.AguardandoAcaoPrioritaria);
                        case EnumStatusVisual.Bloqueado:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Bloqueado);
                        case EnumStatusVisual.Comprado:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Comprado);
                        case EnumStatusVisual.Disponivel:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Disponivel);
                        case EnumStatusVisual.Emitido:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Emitido);
                        case EnumStatusVisual.Entregue:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Entregue);
                        case EnumStatusVisual.Extinto:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Extinto);
                        case EnumStatusVisual.ParcialmentePago:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.ParcialmentePago);
                        case EnumStatusVisual.Reservando:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Reservando);
                        case EnumStatusVisual.TrocaSinalizada:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.TrocaSinalizada);
                        case EnumStatusVisual.Indisponivel:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Indisponivel);
                        case EnumStatusVisual.TrocaEfetuada:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.TrocaEfetuada);
                        case EnumStatusVisual.Renovado:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.Renovado);
                        case EnumStatusVisual.RenovadoSemPagamento:
                            return lstRetorno.FindAll(c => c.Status == EnumStatusVisual.RenovadoSemPagamento);
                        default:
                            break;
                    }
                return lstRetorno;
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

        public EnumStatusVisual VerificaStatusVisual(string statusIngresso, string statusAssinatura, string acaoAssinatura, int ingressoBloqueioID, int assinaturaBloqueioID, int assinaturaExtintoBloqueioID, int assinaturaDesistenciaBloqueioID)
        {
            try
            {
                EnumStatusVisual statusRetorno;

                if ((Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.RESERVADO)
                {
                    statusRetorno = EnumStatusVisual.Reservando;
                }
                else if ((Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.DISPONIVEL)
                {
                    statusRetorno = EnumStatusVisual.Disponivel;
                }
                else if ((statusAssinatura == "" || (AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel) && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (acaoAssinatura == "" || (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Desisistir) && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.Extinto;
                }
                else if (statusAssinatura == "" && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && acaoAssinatura == "" && ingressoBloqueioID != assinaturaExtintoBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.Bloqueado;
                }
                else if (statusAssinatura == "" && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) != Ingresso.StatusIngresso.BLOQUEADO && acaoAssinatura == "")
                {
                    statusRetorno = EnumStatusVisual.Indisponivel;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aguardando && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.AguardandoAcao && ingressoBloqueioID == assinaturaBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.AguardandoAcao;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.AguardandoAcao && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.AguardandoAcaoExtinto;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Indisponivel && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Trocar && ingressoBloqueioID == assinaturaExtintoBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.AguardandoAcaoPrioritaria;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Desistido && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Desisistir && ingressoBloqueioID == assinaturaDesistenciaBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.AguardandoAcaoAdministrativa;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.TrocaSinalizada && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Trocar && ingressoBloqueioID == assinaturaBloqueioID)
                {
                    statusRetorno = EnumStatusVisual.TrocaSinalizada;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar)
                {
                    statusRetorno = EnumStatusVisual.Renovado;

                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.RenovadoSemPagamento && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.BLOQUEADO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar)
                {
                    statusRetorno = EnumStatusVisual.RenovadoSemPagamento;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao)
                {
                    statusRetorno = EnumStatusVisual.Comprado;
                }
                else if ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.VENDIDO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca)
                {
                    statusRetorno = EnumStatusVisual.TrocaEfetuada;
                }
                else if (((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.IMPRESSO && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca))
                {
                    statusRetorno = EnumStatusVisual.Emitido;

                }
                else if (((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Renovado && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Renovar) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Aquisicao && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.Aquisicao) ||
                    ((AssinaturaCliente.EnumStatus)Convert.ToChar(statusAssinatura) == AssinaturaCliente.EnumStatus.Troca && (Ingresso.StatusIngresso)Convert.ToChar(statusIngresso) == Ingresso.StatusIngresso.ENTREGUE && (AssinaturaCliente.EnumAcao)Convert.ToChar(acaoAssinatura) == AssinaturaCliente.EnumAcao.EfetivarTroca))
                {
                    statusRetorno = EnumStatusVisual.Entregue;

                }
                else
                {
                    statusRetorno = EnumStatusVisual.Indisponivel;
                }

                return statusRetorno;
            }
            catch
            {
                //bd.Fechar();
                //throw;
            }

            return EnumStatusVisual.AguardandoAcao;
        }

        public List<EstruturaIDNome> CarregarLstAssinatura(int assinaturaTipoID, bool registroZero)
        {
            try
            {
                string sql = @" SELECT a.ID, a.Nome 
                        FROM tAssinatura a (NOLOCK) 
                        WHERE a.AssinaturaTipoID = " + assinaturaTipoID + " order by a.Nome";

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }

                return lista;

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

        public List<EstruturaIDNome> CarregarLstSetor(int AssinaturaID, int anoID, bool registroZero)
        {
            try
            {
                string sql = @"SELECT DISTINCT ai.SetorID AS ID, s.Nome AS Setor
                                FROM tAssinatura a(NOLOCK)
                                INNER JOIN tAssinaturaAno aa(NOLOCK) ON a.ID = aa.AssinaturaID
                                INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = aa.ID
                                INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
                                WHERE a.ID = " + AssinaturaID + " AND aa.ID = " + anoID + " order by s.Nome ";

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Setor"),
                    });
                }

                return lista;

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

        public List<EstruturaIDNome> CarregarLstSetorAtivo(int AssinaturaID, int anoID, bool registroZero)
        {
            try
            {
                string sql = @"SELECT DISTINCT ai.SetorID AS ID, s.Nome AS Setor
                                FROM tAssinatura a(NOLOCK)
                                INNER JOIN tAssinaturaAno aa(NOLOCK) ON a.ID = aa.AssinaturaID
                                INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = aa.ID
                                INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
                                WHERE a.ID = " + AssinaturaID + " AND aa.ID = " + anoID + " AND s.Ativo = 'T' order by s.Nome ";

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Setor"),
                    });
                }

                return lista;

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

        public List<EstruturaIDNome> CarregarLstTemporada(int AssinaturaID, bool registroZero)
        {
            try
            {
                string sql = @"SELECT an.ID as AnoID, an.Ano 
                                FROM tAssinatura ass (NOLOCK)
                                INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
                                WHERE ass.ID = " + AssinaturaID;

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("AnoID"),
                        Nome = bd.LerString("Ano"),
                    });
                }

                return lista;

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

        public List<EstruturaIDNome> CarregarLstTemporadaAssinaturaTipo(int assinaturaTipoID, bool registroZero)
        {
            try
            {
                string sql = @"SELECT DISTINCT an.Ano as AnoID , an.Ano 
                            FROM tAssinatura ass (NOLOCK)
                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = ass.ID
                            WHERE ass.AssinaturaTipoID = " + assinaturaTipoID;

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaIDNome() { ID = 0, Nome = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("AnoID"),
                        Nome = bd.LerString("Ano"),
                    });
                }

                return lista;

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

        public object CarregarLstStatusAssinatura(int AssinaturaID, bool registroZero)
        {

            try
            {
                string sql = @"SELECT DISTINCT ac.Status
                                FROM tAssinaturaCliente ac (NOLOCK)
                                WHERE ac.AssinaturaID = " + AssinaturaID;

                List<EstruturaAssinaturaStatus> lista = new List<EstruturaAssinaturaStatus>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaAssinaturaStatus() { StatusChar = '0', Status = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    char stchar = Convert.ToChar(bd.LerString("Status"));
                    lista.Add(new EstruturaAssinaturaStatus()
                    {
                        StatusChar = stchar,
                        Status = ((AssinaturaCliente.EnumStatus)stchar).ToString(),
                    });
                }

                return lista;

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

        public object CarregarLstStatusIngresso(int AssinaturaID, bool registroZero)
        {

            try
            {
                string sql = @"SELECT DISTINCT i.Status AS StatusIngresso
                                FROM tIngresso i (NOLOCK)
                                INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.ApresentacaoID = i.ApresentacaoID AND ai.SetorID = i.SetorID
                                INNER JOIN tAssinaturaAno aa (NOLOCK) ON ai.AssinaturaAnoID = aa.ID
                                WHERE aa.AssinaturaID = " + AssinaturaID;

                List<EstruturaAssinaturaStatus> lista = new List<EstruturaAssinaturaStatus>();

                if (registroZero)
                    lista.Add(new IRLib.ClientObjects.EstruturaAssinaturaStatus() { StatusChar = '0', Status = "Selecione..." });

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    char stchar = Convert.ToChar(bd.LerString("StatusIngresso"));
                    lista.Add(new EstruturaAssinaturaStatus()
                    {
                        StatusChar = stchar,
                        Status = ((Ingresso.StatusIngresso)stchar).ToString(),
                    });
                }

                return lista;

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

        public int GetLocalID(int AssinaturaID)
        {
            try
            {
                int retorno = 0;

                string sql = @"select LocalID from tAssinatura where ID = " + AssinaturaID;
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("LocalID");
                }

                return retorno;
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

        //private void EfetuarDesistencia(CTLib.BD bd, EstruturaAssinaturaBloqueio assinatura)
        //{
        //    AssinaturaCliente oAC = new AssinaturaCliente(this.Control.UsuarioID);

        //    var z = new IRLib.Assinaturas.Models.AcaoProvisoria
        //    {
        //        Acao = AssinaturaCliente.EnumAcao.Desisistir,
        //        AssinaturaClienteID = assinatura.AssinaturaClienteID,
        //    };

        //    oAC.EfetuarAcoes(bd, assinatura.ClienteID, this.Control.UsuarioID, z, 0);
        //}

        private void EfetuarDesistencia(CTLib.BD bd, IEnumerable<EstruturaAssinaturaBloqueio> lstAssinatura)
        {
            try
            {
                AssinaturaCliente oAC = new AssinaturaCliente();
                List<Assinaturas.Models.AcaoProvisoria> lista = new List<IRLib.Assinaturas.Models.AcaoProvisoria>();


                foreach (EstruturaAssinaturaBloqueio item in lstAssinatura)
                {

                    lista = new List<IRLib.Assinaturas.Models.AcaoProvisoria>();

                    lista.Add(new IRLib.Assinaturas.Models.AcaoProvisoria
                    {
                        Acao = AssinaturaCliente.EnumAcao.Desisistir,
                        AssinaturaClienteID = item.AssinaturaClienteID,
                    });

                    oAC.EfetuarAcoes(bd, item.ClienteID, this.Control.UsuarioID, lista, 0);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TornarDisponivel(CTLib.BD bd, int LugarID, int assinaturaID)
        {
            BD bdAux = new BD();

            try
            {

                string sqlBusca =
                   string.Format(
                       @"SELECT DISTINCT i.ID as IngressoID, i.Codigo, i.EventoID, i.ApresentacaoID, i.Status, i.BloqueioID,
                        i.ApresentacaoSetorID, i.SetorID, i.LugarID,  DesistenciaBloqueioID    
                        FROM tIngresso i (nolock)
                        INNER JOIN tLugar l (nolock) on i.LugarID = l.ID
                        INNER JOIN tAssinaturaItem ai (nolock) on ai.ApresentacaoID = i.ApresentacaoID AND ai.SetorID = i.SetorID
                        INNER JOIN tAssinaturaAno aa (nolock) on ai.AssinaturaAnoID = aa.ID
                        INNER JOIN tAssinatura ass (nolock) on ass.ID = aa.AssinaturaID
	                    WHERE ass.ID = {0} AND i.LugarID = {1}
                    ", assinaturaID, LugarID);

                bdAux.Consulta(sqlBusca);
                if (!bdAux.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");

                var ingressoAnonimo = new
                {
                    IngressoID = 0,
                    Codigo = string.Empty,
                    EventoID = 0,
                    ApresentacaoID = 0,
                    Status = string.Empty,
                    BloqueioID = 0,
                    ApresentacaoSetorID = 0,
                    SetorID = 0,
                    LugarID = 0,
                    Acao = AssinaturaCliente.EnumAcao.AguardandoAcao,
                    DesistenciaBloqueioID = 0,
                };

                var listaIngressos = VendaBilheteria.ToAnonymousList(ingressoAnonimo);

                do
                {
                    var ingresso = (new
                    {
                        IngressoID = bdAux.LerInt("IngressoID"),
                        Codigo = bdAux.LerString("Codigo"),
                        EventoID = bdAux.LerInt("EventoID"),
                        ApresentacaoID = bdAux.LerInt("ApresentacaoID"),
                        Status = bdAux.LerString("Status"),
                        BloqueioID = bdAux.LerInt("BloqueioID"),
                        ApresentacaoSetorID = bdAux.LerInt("ApresentacaoSetorID"),
                        SetorID = bdAux.LerInt("SetorID"),
                        LugarID = bdAux.LerInt("LugarID"),
                        Acao = AssinaturaCliente.EnumAcao.Desisistir,
                        DesistenciaBloqueioID = bdAux.LerInt("DesistenciaBloqueioID"),
                    });

                    listaIngressos.Add(ingresso);
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();

                foreach (var ingresso in listaIngressos)
                {
                    if (bd.Executar(Ingresso.StringRemoverReservaAssinatura(ingresso.IngressoID)) != 1)
                        throw new Exception("Não foi possível efetuar a ação de desistencia de um dos ingressos.");
                }

            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public void Disponibilizar(List<EstruturaAssinaturaBloqueio> lstAssinatura, int assinaturaID)
        {

            try
            {
                bd.IniciarTransacao();

                this.EfetuarDesistencia(bd, lstAssinatura.Where(c => c.Status == EnumStatusVisual.AguardandoAcao || c.Status == EnumStatusVisual.TrocaSinalizada || c.Status == EnumStatusVisual.RenovadoSemPagamento));

                foreach (var item in lstAssinatura)
                {
                    this.TornarDisponivel(bd, item.LugarID, assinaturaID);
                }

                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void Extinguir(List<EstruturaAssinaturaBloqueio> lstAssinatura)
        {

            var oAssinaturaCliente = new AssinaturaCliente(this.Control.UsuarioID);

            try
            {
                foreach (var assinatura in lstAssinatura)
                {
                    bd.IniciarTransacao();

                    if (assinatura.ClienteID > 0)
                    {
                        oAssinaturaCliente.EfetuarAcoes(assinatura.ClienteID, this.Control.UsuarioID,
                                                    new IRLib.Assinaturas.Models.AcaoProvisoria()
                                                    {
                                                        Acao = AssinaturaCliente.EnumAcao.Extinguir,
                                                        ClienteID = assinatura.ClienteID,
                                                        AssinaturaID = assinatura.AssinaturaID,
                                                        LugarID = assinatura.LugarID,
                                                        AssinaturaAnoID = assinatura.AssinaturaAnoID,
                                                        SetorID = assinatura.SetorID,
                                                        AssinaturaClienteID = assinatura.AssinaturaClienteID
                                                    },
                                                    0);
                    }

                    this.TornarBloqueado(bd, assinatura.AssinaturaID, assinatura.LugarID, assinatura.AssinaturaExtintoID, assinatura.AssinaturaClienteID, assinatura.ClienteID);

                    bd.FinalizarTransacao();
                }
            }
            catch
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }

        }

        public void Associar(int clienteID, List<EstruturaAssinaturaBloqueio> lstAssinatura, int assinaturaID, int anoID)
        {
            try
            {

                bd.IniciarTransacao();

                this.EfetuarDesistencia(bd, lstAssinatura.Where(c => c.Status == EnumStatusVisual.AguardandoAcao || c.Status == EnumStatusVisual.AguardandoAcaoPrioritaria || c.Status == EnumStatusVisual.TrocaSinalizada || c.Status == EnumStatusVisual.RenovadoSemPagamento));

                AssinaturaCliente oAC = new AssinaturaCliente();
                List<Assinaturas.Models.AcaoProvisoria> lista = new List<IRLib.Assinaturas.Models.AcaoProvisoria>();

                foreach (EstruturaAssinaturaBloqueio item in lstAssinatura)
                {

                    lista.Clear();

                    lista.Add(new IRLib.Assinaturas.Models.AcaoProvisoria
                    {
                        Acao = AssinaturaCliente.EnumAcao.AguardandoAcao,
                        ClienteID = clienteID,
                        AssinaturaID = assinaturaID,
                        LugarID = item.LugarID,
                        AssinaturaAnoID = anoID,
                        SetorID = item.SetorID
                    });

                    oAC.EfetuarAcoes(bd, clienteID, this.Control.UsuarioID, lista, 0);

                    this.TornarBloqueado(bd, assinaturaID, item.LugarID, item.AssinaturaBloqueioID, oAC.Control.ID, clienteID);

                }


                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


        private void TornarBloqueado(BD bd, int assinaturaID, int lugarID, int bloqueioID, int assinaturaClienteID, int clienteID)
        {
            BD bdAux = new BD();

            try
            {

                string sqlBusca =
                   string.Format(
                       @"SELECT DISTINCT i.ID as IngressoID, i.Codigo, i.EventoID, i.ApresentacaoID, i.Status, i.BloqueioID,
                        i.ApresentacaoSetorID, i.SetorID, i.LugarID,  DesistenciaBloqueioID    
                        FROM tIngresso i (nolock)
                        INNER JOIN tLugar l (nolock) on i.LugarID = l.ID
                        INNER JOIN tAssinaturaItem ai (nolock) on ai.ApresentacaoID = i.ApresentacaoID AND ai.SetorID = i.SetorID
                        INNER JOIN tAssinaturaAno aa (nolock) on ai.AssinaturaAnoID = aa.ID
                        INNER JOIN tAssinatura ass (nolock) on ass.ID = aa.AssinaturaID
	                    WHERE ass.ID = {0} AND i.LugarID = {1}
                    ", assinaturaID, lugarID);

                bdAux.Consulta(sqlBusca);
                if (!bdAux.Consulta().Read())
                     throw new Exception("Não foi possível encontrar seus ingressos.");

                    var ingressoAnonimo = new
                    {
                        IngressoID = 0,
                        Codigo = string.Empty,
                        EventoID = 0,
                        ApresentacaoID = 0,
                        Status = string.Empty,
                        BloqueioID = 0,
                        ApresentacaoSetorID = 0,
                        SetorID = 0,
                        LugarID = 0,
                        DesistenciaBloqueioID = 0,
                    };

                    var listaIngressos = VendaBilheteria.ToAnonymousList(ingressoAnonimo);

                    do
                    {
                        var ingresso = (new
                        {
                            IngressoID = bdAux.LerInt("IngressoID"),
                            Codigo = bdAux.LerString("Codigo"),
                            EventoID = bdAux.LerInt("EventoID"),
                            ApresentacaoID = bdAux.LerInt("ApresentacaoID"),
                            Status = bdAux.LerString("Status"),
                            BloqueioID = bdAux.LerInt("BloqueioID"),
                            ApresentacaoSetorID = bdAux.LerInt("ApresentacaoSetorID"),
                            SetorID = bdAux.LerInt("SetorID"),
                            LugarID = bdAux.LerInt("LugarID"),
                            DesistenciaBloqueioID = bdAux.LerInt("DesistenciaBloqueioID"),
                        });

                        listaIngressos.Add(ingresso);
                    } while (bdAux.Consulta().Read());

                    bdAux.FecharConsulta();
                    bdAux.Fechar();

                    StringBuilder stbDesistencia = new StringBuilder();

                    foreach (var ingresso in listaIngressos)
                    {

                        if (bd.Executar(Ingresso.StringRemoverReservaAssinatura(ingresso.IngressoID)) != 1)
                            throw new Exception("Não foi possível efetuar a ação de desistencia de um dos ingressos.");

                        stbDesistencia.AppendFormat("UPDATE tIngresso SET Status = '{0}', BloqueioID = {1}, AssinaturaClienteID = {2}, ClienteID = {3} ", Ingresso.BLOQUEADO, bloqueioID, assinaturaClienteID, clienteID);
                        stbDesistencia.AppendFormat("WHERE ID = {0} AND (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "' OR Status='" + Ingresso.RESERVANDO + "')", ingresso.IngressoID);
                        if (bd.Executar(stbDesistencia.ToString()) != 1)
                            throw new Exception("Não foi possível efetuar a ação de desistência de um dos ingressos.");
                        stbDesistencia = new StringBuilder();
                    }
                
            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public bool InsereLog(int AssinaturaTipoIDOrigem, int AssinaturaAnoOrigem, int AssinaturaTipoIDNovo, int AssinaturaAnoNovo, int AssinaturaIDOrigem, int AssinaturaIDNovo, string DataMigracao, int UsuarioID = 1)
        {
            BD bdAux = new BD();
            bool retorno = false;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat(@"insert into tAssinaturaMigracaoLog
                                            (  AssinaturaTipoIDOrigem
                                                , AssinaturaAnoOrigem
                                                , AssinaturaTipoIDNovo
                                                , AssinaturaAnoNovo
                                                , AssinaturaIDOrigem
                                                , AssinaturaIDNovo
                                                , DataMigracao
                                                , UsuarioID)
								            values(
                                                 {0}
                                                ,{1}
                                                ,{2}
                                                ,{3}
                                                ,{4}
                                                ,{5}
                                                ,'{6}'
                                                ,{7}
                                            ))", AssinaturaTipoIDOrigem, AssinaturaAnoOrigem, AssinaturaTipoIDNovo, AssinaturaAnoNovo, AssinaturaIDOrigem, AssinaturaIDNovo, DataMigracao, UsuarioID);

                if (bd.Executar(sb.ToString()) >= 1)
                    retorno = true;
            }
            finally
            {
                bdAux.Fechar();
            }
            return retorno;
        }

        private void TornarCancelado(BD bd, int assinaturaID, int lugarID, int bloqueioID, int assinaturaClienteID, int clienteID)
        {
            BD bdAux = new BD();

            try
            {

                string sqlBusca =
                   string.Format(
                       @"SELECT DISTINCT i.ID as IngressoID, i.Codigo, i.EventoID, i.ApresentacaoID, i.Status, i.BloqueioID,
                        i.ApresentacaoSetorID, i.SetorID, i.LugarID,  DesistenciaBloqueioID    
                        FROM tIngresso i (nolock)
                        INNER JOIN tLugar l (nolock) on i.LugarID = l.ID
                        INNER JOIN tAssinaturaItem ai (nolock) on ai.ApresentacaoID = i.ApresentacaoID AND ai.SetorID = i.SetorID
                        INNER JOIN tAssinaturaAno aa (nolock) on ai.AssinaturaAnoID = aa.ID
                        INNER JOIN tAssinatura ass (nolock) on ass.ID = aa.AssinaturaID
	                    WHERE ass.ID = {0} AND i.LugarID = {1}
                    ", assinaturaID, lugarID);

                bdAux.Consulta(sqlBusca);
                if (!bdAux.Consulta().Read())
                    throw new Exception("Não foi possível encontrar seus ingressos.");

                var ingressoAnonimo = new
                {
                    IngressoID = 0,
                    Codigo = string.Empty,
                    EventoID = 0,
                    ApresentacaoID = 0,
                    Status = string.Empty,
                    BloqueioID = 0,
                    ApresentacaoSetorID = 0,
                    SetorID = 0,
                    LugarID = 0,
                    DesistenciaBloqueioID = 0,
                };

                var listaIngressos = VendaBilheteria.ToAnonymousList(ingressoAnonimo);

                do
                {
                    var ingresso = (new
                    {
                        IngressoID = bdAux.LerInt("IngressoID"),
                        Codigo = bdAux.LerString("Codigo"),
                        EventoID = bdAux.LerInt("EventoID"),
                        ApresentacaoID = bdAux.LerInt("ApresentacaoID"),
                        Status = bdAux.LerString("Status"),
                        BloqueioID = bdAux.LerInt("BloqueioID"),
                        ApresentacaoSetorID = bdAux.LerInt("ApresentacaoSetorID"),
                        SetorID = bdAux.LerInt("SetorID"),
                        LugarID = bdAux.LerInt("LugarID"),
                        DesistenciaBloqueioID = bdAux.LerInt("DesistenciaBloqueioID"),
                    });

                    listaIngressos.Add(ingresso);
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();
                bdAux.Fechar();

                StringBuilder stbDesistencia = new StringBuilder();

                foreach (var ingresso in listaIngressos)
                {

                    if (bd.Executar("UPDATE tIngresso SET BloqueioID = 0, PrecoID=0, UsuarioID=0, Status='D', LojaID = 0, ClienteID = 0, TimeStampReserva='', SessionID='', AssinaturaClienteID = 0 WHERE ID = " + ingresso.IngressoID) != 1)
                        throw new Exception("Não foi possível cancelar um dos ingressos.");

                    stbDesistencia.AppendFormat("UPDATE tIngresso SET Status = '{0}', BloqueioID = {1}, AssinaturaClienteID = {2}, ClienteID = {3} ", Ingresso.BLOQUEADO, bloqueioID, assinaturaClienteID, clienteID);
                    stbDesistencia.AppendFormat("WHERE ID = {0} AND (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "' OR Status='" + Ingresso.RESERVANDO + "')", ingresso.IngressoID);
                    if (bd.Executar(stbDesistencia.ToString()) != 1)
                        throw new Exception("Não foi possível cancelar um dos ingressos.");
                    stbDesistencia = new StringBuilder();
                }

            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public void Bloquear(int BloqueioID, List<EstruturaAssinaturaBloqueio> lstAssinatura, int assinaturaID)
        {
            try
            {

                bd.IniciarTransacao();

                this.EfetuarDesistencia(bd, lstAssinatura.Where(c => c.Status == EnumStatusVisual.AguardandoAcao || c.Status == EnumStatusVisual.AguardandoAcaoPrioritaria || c.Status == EnumStatusVisual.AguardandoAcaoExtinto || c.Status == EnumStatusVisual.TrocaSinalizada || c.Status == EnumStatusVisual.RenovadoSemPagamento));

                foreach (var item in lstAssinatura)
                {
                    this.TornarBloqueado(bd, assinaturaID, item.LugarID, BloqueioID, 0, 0);
                }

                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void ApagarAno(EstruturaAssinaturaAno eAno)
        {
            try
            {
                AssinaturaAno oAA = new AssinaturaAno(this.Control.UsuarioID);
                AssinaturaItem oAI = new AssinaturaItem(this.Control.UsuarioID);
                oAI.ExcluirItensAssinatura(eAno.ID);
                oAA.Excluir(eAno.ID);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
