using CTLib;
using IRLib;
using System;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{

    public class MinhasComprasDetalhesLista : List<MinhasComprasDetalhes>
    {
        private BD oBD = new BD();

        private MinhasComprasDetalhes oMinhasComprasDetalhes { get; set; }

        public MinhasComprasDetalhesLista()
        {
            this.Clear();
        }

        public MinhasComprasDetalhesLista GetByVendaBilheteriaId(int vendabilheteriaId, int clientID)
        {
            try
            {
                if (vendabilheteriaId == 0)
                    throw new Exception("Venda Inválida.");

                string strSql = string.Empty;

                string ValidaIngresso = string.Format(@"
                        SELECT DISTINCT vb.Senha  FROM tVendaBilheteria vb (NOLOCK)
                        INNER JOIN tIngresso ON tIngresso.VendaBilheteriaID = vb.ID
		                WHERE vb.ID = {0} AND vb.ClienteID = {1}", vendabilheteriaId, clientID);

                var ValidaSenha = oBD.ConsultaValor(ValidaIngresso) as string;

                if (string.IsNullOrEmpty(ValidaSenha))
                {
                    string ValidaValeIngresso = string.Format(@"
                        SELECT DISTINCT vb.Senha  FROM tVendaBilheteria vb (NOLOCK)
                        INNER JOIN tValeIngresso ON tValeIngresso.VendaBilheteriaID = vb.ID
		                WHERE vb.ID = {0} AND vb.ClienteID = {1}", vendabilheteriaId, clientID);

                    ValidaSenha = oBD.ConsultaValor(ValidaValeIngresso) as string;

                    if (string.IsNullOrEmpty(ValidaSenha))
                        throw new Exception("Ingresso não existe!");

                    strSql = string.Format(@"SELECT V.ID as ValeIngressoID ,VT.Nome, VT.Valor, vbi.TaxaConvenienciaValor AS Conveniencia, VT.ValidadeDiasImpressao, VT.ValidadeData, V.CodigoTroca, 
                                            VB.Status AS VBStatus, V.Status as IStatus, V.VendaBilheteriaID  as IVBID,VB.ID AS VBID , Vil.Acao as ILAcao, vb.PagamentoProcessado,IsNull(e.Tipo, '') AS TaxaEntregaTipo
                                            FROM tValeIngressoLog as Vil
                                            INNER JOIN tValeIngresso as V (NOLOCK) ON Vil.ValeIngressoID = V.ID
                                            INNER JOIN tValeIngressoTipo AS VT ON VT.ID = V.ValeIngressoTipoID
                                            INNER JOIN tVendaBilheteria AS VB ON VB.ID = V.VendaBilheteriaID
                                            INNER JOIN tVendaBilheteriaItem vbi (NOLOCK)ON Vil.VendaBilheteriaItemID = vbi.ID 
                                            LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
                                            LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID          
                                            WHERE Vil.VendaBilheteriaID ={0}", vendabilheteriaId);

                    oBD.Consulta(strSql);

                    while (oBD.Consulta().Read())
                    {
                        bool exibir = false;

                        MinhasComprasDetalhes mcd = new MinhasComprasDetalhes();

                        mcd.ValeIngressoID = oBD.LerInt("ValeIngressoID");
                        mcd.NomeValeIngresso = oBD.LerString("Nome");
                        mcd.Valor = oBD.LerDecimal("Valor");
                        mcd.Conveniencia = oBD.LerDecimal("Conveniencia");

                        DateTime dataValidade = oBD.LerDateTime("ValidadeData");
                        int ValidadeDiasImpressa = oBD.LerInt("ValidadeDiasImpressao");

                        mcd.ValidadeValeIngress = DateTime.Now.AddDays(ValidadeDiasImpressa);

                        mcd.CodigoDeTroca = oBD.LerString("CodigoTroca");
                        mcd.ILAcao = oBD.LerString("ILAcao");
                        mcd.IStatus = oBD.LerString("IStatus");
                        mcd.VBStatus = oBD.LerString("VBStatus");
                        mcd.VBID = oBD.LerInt("VBID");
                        mcd.IVBID = oBD.LerInt("IVBID");
                        mcd.Senha = ValidaSenha;

                        if (mcd.IVBID == mcd.VBID && !(mcd.VBStatus == VendaBilheteria.CANCELADO || mcd.VBStatus == VendaBilheteria.FRAUDE))
                        {
                            if (mcd.VBStatus == VendaBilheteria.AGUARDANDO_APROVACAO)
                            {
                                mcd.Status = "Aguardando Aprovação";
                                exibir = true;
                            }
                            else if (mcd.VBStatus == VendaBilheteria.EMANALISE || !oBD.LerBoolean("PagamentoProcessado"))
                            {
                                mcd.Status = "Em Análise";
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.VENDIDO)
                            {
                                mcd.Status = "Aguardando Impressão";
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.IMPRESSO)
                            {
                                switch (oBD.LerString("TaxaEntregaTipo"))
                                {
                                    case IRLib.Entrega.RETIRADA:
                                    case IRLib.Entrega.RETIRADABILHETERIA:
                                        mcd.Status = "Aguardando Retirada";
                                        break;
                                    case IRLib.Entrega.AGENDADA:
                                    case IRLib.Entrega.NORMAL:
                                        mcd.Status = "Pedido em Trânsito";
                                        break;
                                    default:
                                        mcd.Status = "Ingresso Impresso";
                                        break;
                                }
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.ENTREGUE)
                            {
                                mcd.Status = "Pedido Entregue";
                                exibir = true;
                            }
                        }
                        else
                        {
                            mcd.Status = "Consulte SAC";
                            exibir = true;
                        }
                        if (exibir)
                            this.Add(mcd);
                    }

                }
                else
                {
                    strSql = string.Format(@"
                        SELECT i.ID AS IngressoID,l.Nome AS [Local],ev.Nome AS Evento,a.Horario AS Apresentacao, s.Nome AS Setor,
                            p.Nome AS Preco,p.Valor,
                            CASE WHEN pctItm.Quantidade IS NOT NULL 
                                THEN CAST(vbi.TaxaConvenienciaValor/SUM(pctItm.Quantidade) AS DECIMAL(10,2))
                                ELSE vbi.TaxaConvenienciaValor
                            END AS Conveniencia,
                            i.VendaBilheteriaID as IVBID, vb.ID AS VBID,i.Codigo AS CodIngresso, il.Acao as ILAcao,i.[Status] as IStatus,
                            vb.[Status] AS VBStatus,
                            IsNull(e.Tipo, '') AS TaxaEntregaTipo, vb.PagamentoProcessado
                        FROM tIngressoLog il (NOLOCK) 
                        INNER JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = il.VendaBilheteriaID
                        LEFT OUTER JOIN tPreco p (NOLOCK) ON p.ID = il.PrecoID 
                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = il.IngressoID  
                        INNER JOIN tEvento ev (NOLOCK) ON ev.ID = i.EventoID  
                        INNER JOIN tLocal l (NOLOCK) ON l.ID = i.LocalID  
                        INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID  
                        INNER JOIN tApresentacao a (NOLOCK) ON a.ID = i.ApresentacaoID
                        INNER JOIN tVendaBilheteriaItem vbi (NOLOCK)ON il.VendaBilheteriaItemID = vbi.ID 
                        LEFT JOIN tPacoteItem pctItm (NOLOCK) ON vbi.PacoteID = pctItm.PacoteID 
                        INNER JOIN tCliente ci (NOLOCK) ON ci.ID = il.ClienteID 
                        LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
                        LEFT JOIN tEntrega e (NOLOCK) ON e.ID = tc.EntregaID          
                            WHERE il.VendaBilheteriaID = {0} 
                        GROUP BY pctItm.Quantidade,vbi.TaxaConvenienciaValor, i.ID, l.Nome,ev.Nome,a.Horario, s.Nome ,
                        p.Nome ,p.Valor, i.VendaBilheteriaID,i.Codigo, il.Acao,i.[Status] ,il.IngressoID,il.ID,
                        vb.[Status] ,vb.ClienteID ,vb.ID, e.Tipo, vb.PagamentoProcessado
                            ORDER BY il.IngressoID,il.ID", vendabilheteriaId);

                    oBD.Consulta(strSql);

                    while (oBD.Consulta().Read())
                    {
                        bool exibir = false;

                        MinhasComprasDetalhes mcd = new MinhasComprasDetalhes();
                        mcd.Local = oBD.LerString("Local");
                        mcd.Evento = oBD.LerString("Evento");
                        mcd.Apresentacao = DateTime.ParseExact(oBD.LerString("Apresentacao"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        mcd.Setor = oBD.LerString("Setor");
                        mcd.NomePreco = oBD.LerString("Preco");
                        mcd.Valor = oBD.LerDecimal("Valor");
                        mcd.Conveniencia = oBD.LerDecimal("Conveniencia");
                        mcd.CodIngresso = oBD.LerString("CodIngresso");
                        mcd.ILAcao = oBD.LerString("ILAcao");
                        mcd.IStatus = oBD.LerString("IStatus");
                        mcd.VBStatus = oBD.LerString("VBStatus");
                        mcd.IngressoID = oBD.LerInt("IngressoID");
                        mcd.VBID = oBD.LerInt("VBID");
                        mcd.IVBID = oBD.LerInt("IVBID");
                        mcd.Senha = ValidaSenha;

                        if (mcd.IVBID == mcd.VBID && !(mcd.VBStatus == VendaBilheteria.CANCELADO || mcd.VBStatus == VendaBilheteria.FRAUDE))
                        {
                            if (mcd.VBStatus == VendaBilheteria.AGUARDANDO_APROVACAO)
                            {
                                mcd.Status = "Aguardando Aprovação";
                                exibir = true;
                            }
                            else if (mcd.VBStatus == VendaBilheteria.EMANALISE || !oBD.LerBoolean("PagamentoProcessado"))
                            {
                                mcd.Status = "Em Análise";
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.VENDIDO)
                            {
                                mcd.Status = "Aguardando Impressão";
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.IMPRESSO)
                            {
                                switch (oBD.LerString("TaxaEntregaTipo"))
                                {
                                    case IRLib.Entrega.RETIRADA:
                                    case IRLib.Entrega.RETIRADABILHETERIA:
                                        mcd.Status = "Aguardando Retirada";
                                        break;
                                    case IRLib.Entrega.AGENDADA:
                                    case IRLib.Entrega.NORMAL:
                                        mcd.Status = "Pedido em Trânsito";
                                        break;
                                    default:
                                        mcd.Status = "Ingresso Impresso";
                                        break;
                                }
                                exibir = true;
                            }
                            else if (mcd.IStatus == Ingresso.ENTREGUE)
                            {
                                mcd.Status = "Pedido Entregue";
                                exibir = true;
                            }
                        }
                        else
                        {
                            mcd.Status = "Consulte SAC";
                            exibir = true;
                        }
                        if (exibir)
                            this.Add(mcd);
                    }
                }
                return this;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                oBD.Fechar();
            }
        }
    }
}
