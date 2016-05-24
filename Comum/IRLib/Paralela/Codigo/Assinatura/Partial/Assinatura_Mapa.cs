using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{
    partial class Assinatura
    {
        public void CancelarAssinaturas(BD bd, List<EstruturaAssinaturaBloqueio> lista)
        {
            this.EfetuarDesistencia(bd, lista);
        }
        public void CancelarAssinaturas(BD bd, List<int> lstAssClienteID)
        {
            try
            {
                List<EstruturaAssinaturaBloqueio> lstAss = this.CarregarAssinatura(lstAssClienteID);

                this.EfetuarDesistencia(bd, lstAss);

                foreach (var item in lstAss.Where(c => c.AssinaturaDesistenciaID > 0))
                {
                    this.TornarCancelado(bd, item.AssinaturaID, item.LugarID, item.AssinaturaDesistenciaID, item.AssinaturaClienteID, 0);
                }
                foreach (var item in lstAss.Where(c => c.AssinaturaDesistenciaID == 0))
                {
                    this.TornarDisponivel(bd, item.LugarID, item.AssinaturaID);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<EstruturaAssinaturaBloqueio> CarregarAssinatura(List<int> lstAssClienteID)
        {
            BD bdaux = new BD();
            try
            {

                string sql = @"SELECT distinct i.Grupo,i.Classificacao, ac.id as AssinaturaClienteID, ac.AssinaturaID, i.LugarID, s.ID as SetorID, i.Codigo AS Lugar,s.Nome AS Setor, ass.Nome as NomeAssinatura, 
                                ISNULL(c.Nome,'') AS Assinante, ISNULL(c.ID,0) AS ClienteID, ISNULL(c.CPF,'') as CPF ,ISNULL(pt.Nome,'') AS Preco, ISNULL(ac.Status,'') as StatusAssinatura , ISNULL(ac.Acao,'') as AcaoAssinatura , 
                                i.Status as StatusIngresso, i.BloqueioID as IngressoBloqueioID,ass.BloqueioID AssinaturaBloqueioID, ass.DesistenciaBloqueioID AssinaturaDesistenciaBloqueioID, ass.ExtintoBloqueioID as AssinaturaExtintoBloqueioID, bl.Nome as BloqueioUtilizado,
                                l.PosicaoY, l.PosicaoX, bl.CorID AS BloqueioCorID
                                FROM tAssinaturaCliente ac
                                INNER JOIN tIngresso i (NOLOCK) ON ac.ID = i.AssinaturaClienteID
                                INNER JOIN tLugar l (NOLOCK) on i.LugarID = l.ID
                                INNER JOIN tAssinatura ass (NOLOCK) on ass.ID = ac.AssinaturaID
                                INNER JOIN tApresentacao ap (NOLOCK) on i.ApresentacaoID = ap.ID
                                INNER JOIN tSetor s (NOLOCK) on i.SetorID = s.ID
                                LEFT JOIN tCliente c (NOLOCK) ON ac.ClienteID = c.ID
                                LEFT JOIN tPrecoTipo pt (NOLOCK) ON ac.PrecoTipoID = pt.ID 
                                LEFT JOIN tBloqueio bl(NOLOCK) ON i.BloqueioID = bl.ID
                                WHERE ac.ID in(" + Utilitario.ArrayToString(lstAssClienteID.ToArray()) + @") 
                                ORDER BY i.Grupo,i.Classificacao";

                bdaux.Consulta(sql);

                var lstRetorno = new List<EstruturaAssinaturaBloqueio>();

                EstruturaAssinaturaBloqueio eABaux = new EstruturaAssinaturaBloqueio();

                while (bdaux.Consulta().Read())
                {
                    if (lstRetorno.Where(c => c.LugarID == bdaux.LerInt("LugarID")).Count() == 0)
                    {
                        string statusAssinatura = bdaux.LerString("StatusAssinatura") != "" ? ((AssinaturaCliente.EnumStatus)Convert.ToChar(bdaux.LerString("StatusAssinatura"))).ToString() : "--";
                        string statusIngresso = bdaux.LerString("StatusIngresso") != "" ? ((Ingresso.StatusIngresso)Convert.ToChar(bdaux.LerString("StatusIngresso"))).ToString() : "--";


                        eABaux = new EstruturaAssinaturaBloqueio()
                        {
                            LugarID = bdaux.LerInt("LugarID"),
                            NomeAssinatura = bdaux.LerString("NomeAssinatura"),
                            Assinante = bdaux.LerString("Assinante"),
                            CPF = bdaux.LerString("CPF"),
                            Setor = bdaux.LerString("Setor"),
                            Lugar = bdaux.LerString("Lugar"),
                            Preco = bdaux.LerString("Preco"),
                            ClienteID = bdaux.LerInt("ClienteID"),
                            AssinaturaClienteID = bdaux.LerInt("AssinaturaClienteID"),
                            AssinaturaBloqueioID = bdaux.LerInt("AssinaturaBloqueioID"),
                            BloqueioUtilizado = bdaux.LerString("BloqueioUtilizado") != "" ? bdaux.LerString("BloqueioUtilizado") : "--",
                            StatusAssinatura = statusAssinatura,
                            StatusIngresso = statusIngresso,
                            SetorID = bdaux.LerInt("SetorID"),
                            Selecionar = false,
                            PosicaoX = bdaux.LerInt("PosicaoX"),
                            PosicaoY = bdaux.LerInt("PosicaoY"),
                            BloqueioID = bdaux.LerInt("IngressoBloqueioID"),
                            BloqueioCorID = bdaux.LerInt("BloqueioCorID"),
                            AssinaturaID = bdaux.LerInt("AssinaturaID"),
                            AssinaturaDesistenciaID = bdaux.LerInt("AssinaturaDesistenciaBloqueioID"),


                        };

                        eABaux.Status = this.VerificaStatusVisual(bdaux.LerString("StatusIngresso"), bdaux.LerString("StatusAssinatura"), bdaux.LerString("AcaoAssinatura"), bdaux.LerInt("IngressoBloqueioID"), bdaux.LerInt("AssinaturaBloqueioID"), bdaux.LerInt("AssinaturaExtintoBloqueioID"), bdaux.LerInt("AssinaturaDesistenciaBloqueioID"));

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

                return lstRetorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bdaux.Fechar();
            }
        }

        public List<EstruturaAssinaturaBloqueio> DadosMapa(int assinaturaID, int anoID, int setorID)
        {
            return Busca(assinaturaID, setorID, null, null, null, anoID, true, true);
        }


        private string MontaFiltroAssinatura(int setorID, string lugar, string assinante, bool comAssinantes, bool semAssinantes)
        {
            string filtro = "";

            if (setorID > 0)
            {
                filtro += " AND s.ID = " + setorID;
            }

            if (lugar != null && lugar.Length > 0)
                filtro += " AND i.Codigo LIKE '%" + lugar.ToSafeString() + "%' ";

            if (assinante != null && assinante.Length > 0)
                filtro += " AND c.Nome LIKE '%" + assinante.ToSafeString() + "%' ";

            if (!comAssinantes || !semAssinantes)
            {
                if (comAssinantes)
                    filtro += " AND ac.ID > 0 ";

                if (semAssinantes)
                    filtro += " AND ac.ID IS NULL ";
            }
            return filtro;
        }

    }
}
