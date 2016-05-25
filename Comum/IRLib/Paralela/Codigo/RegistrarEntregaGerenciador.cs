using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class RegistrarEntregaGerenciadorParalela : MarshalByRefObject
    {

        public List<EstruturaRegistroEntrega> PesquisarSenhas(List<string> senhas, string condicao)
        {
            BD bd = new BD();
            try
            {
                DataTable dttBulk = new DataTable("Senhas");
                dttBulk.Columns.Add("Senha", typeof(string));

                DataRow dtr;
                foreach (string senha in senhas)
                {
                    dtr = dttBulk.NewRow();
                    dtr["Senha"] = senha;
                    dttBulk.Rows.Add(dtr);
                }

                string sql = string.Empty;

                bd.BulkInsert(dttBulk, "#tmpVendas", true, "Senha", "varchar(20)", "Latin1_General_CI_AI");

                if (condicao != "")
                {
                    condicao = String.Concat("AND", " ", condicao);

                    sql = string.Format(@"SELECT DISTINCT vb.ID, vb.Senha, IsNull(e.Nome, 'Sem Taxa') AS TaxaEntrega, vb.DataVenda, 
                    CASE WHEN c.CNPJ IS NOT NULL
                    THEN c.NomeFantasia
                    ELSE IsNull(c.Nome, 'Sem Cliente') COLLATE Latin1_General_CI_AI
                    END AS Cliente, vb.Status, c.Email
                    FROM tVendaBilheteria vb (NOLOCK)
                    INNER JOIN #tmpVendas ON vb.Senha = #tmpVendas.Senha
                    INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID                 
                    INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
                    INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID
                    INNER JOIN tLoja l (NOLOCK) on l.ID =  ca.LojaID
                    LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
                    LEFT JOIN tEntrega e (NOLOCK) ON tc.EntregaID = e.ID
                    WHERE vb.Status = '{0}' AND i.Status = '{1}' {2}
                    ", VendaBilheteria.PAGO, Ingresso.IMPRESSO, condicao);
                }
                else
                {
                    sql = string.Format(@"SELECT DISTINCT vb.ID, vb.Senha, IsNull(e.Nome, 'Sem Taxa') AS TaxaEntrega, vb.DataVenda, 
                    CASE WHEN c.CNPJ IS NOT NULL
                    THEN c.NomeFantasia
                    ELSE IsNull(c.Nome, 'Sem Cliente') COLLATE Latin1_General_CI_AI
                    END AS Cliente, vb.Status, c.Email
                    FROM tVendaBilheteria vb (NOLOCK)
                    INNER JOIN #tmpVendas ON vb.Senha = #tmpVendas.Senha
                    INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                    INNER JOIN tCliente c (NOLOCK) ON c.ID = vb.ClienteID
                    INNER JOIN tCaixa ca (NOLOCK) ON ca.ID = vb.CaixaID
                    INNER JOIN tLoja l (NOLOCK) on l.ID =  ca.LojaID
                    LEFT JOIN tEntregaControle tc (NOLOCK) ON tc.ID = vb.EntregaControleID
                    LEFT JOIN tEntrega e (NOLOCK) ON tc.EntregaID = e.ID
                    WHERE vb.Status = '{0}' AND i.Status = '{1}'",
                        VendaBilheteria.PAGO, Ingresso.IMPRESSO);
                }

                List<EstruturaRegistroEntrega> lista = new List<EstruturaRegistroEntrega>();

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaRegistroEntrega()
                    {
                        VendaBilheteriaID = bd.LerInt("ID"),
                        Cliente = bd.LerString("Cliente"),
                        DataVenda = bd.LerDateTime("DataVenda"),
                        Senha = bd.LerString("Senha"),
                        StatusVenda = bd.LerString("Status"),
                        TaxaEntrega = bd.LerString("TaxaEntrega"),
                        Email = bd.LerString("Email")
                    });
                }

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void RegistrarEntrega(List<int> lstVendaBilheteriaID, int UsuarioID, int EmpresaID, int CaixaID, int LojaID, int CanalID)
        {
            //Auxiliar para consulta
            BD bd = new BD();
            BD bdAux = new BD();
            IngressoLog log = null;
            try
            {
                bd.IniciarTransacao();

                DataTable dttBulk = new DataTable("VendaBilheteria");
                dttBulk.Columns.Add("ID", typeof(int));

                DataRow dtr;
                foreach (int id in lstVendaBilheteriaID)
                {
                    dtr = dttBulk.NewRow();
                    dtr["ID"] = id;
                    dttBulk.Rows.Add(dtr);
                }
                bdAux.BulkInsert(dttBulk, "#tmpVendaBilheteriaID", false, true);

                bdAux.Consulta(string.Format(
                    @"SELECT i.ID, i.PrecoID, i.EventoID, i.BloqueioID, i.CortesiaID, 
                    i.ClienteID, i.VendaBilheteriaID, vb.ClienteID
                    FROM tVendaBilheteria vb (NOLOCK)
                    INNER JOIN #tmpVendaBilheteriaID tmp ON tmp.ID = vb.ID
                    INNER JOIN tIngresso i (NOLOCK) ON i.VendaBilheteriaID = vb.ID
                    WHERE i.Status = '{0}' ", Ingresso.IMPRESSO));

                while (bdAux.Consulta().Read())
                {
                    bd.Executar(string.Format(
                        @"UPDATE tIngresso SET Status = '{0}'
                        WHERE tIngresso.ID = {1} ", Ingresso.ENTREGUE, bdAux.LerInt("ID")));

                    log = new IngressoLog();
                    log.IngressoID.Valor = bdAux.LerInt("ID");
                    log.UsuarioID.Valor = UsuarioID;
                    log.TimeStamp.Valor = DateTime.Now;
                    log.Acao.Valor = Ingresso.ENTREGUE;
                    log.PrecoID.Valor = bdAux.LerInt("PrecoID");
                    log.CortesiaID.Valor = bdAux.LerInt("CortesiaID");
                    log.BloqueioID.Valor = bdAux.LerInt("BloqueioID");
                    log.VendaBilheteriaItemID.Valor = 0;
                    log.Obs.Valor = string.Empty;
                    log.EmpresaID.Valor = EmpresaID;
                    log.VendaBilheteriaID.Valor = bdAux.LerInt("VendaBilheteriaID");
                    log.CaixaID.Valor = CaixaID;
                    log.LojaID.Valor = LojaID;
                    log.CanalID.Valor = CanalID;
                    log.ClienteID.Valor = bdAux.LerInt("ClienteID");
                    log.CodigoBarra.Valor = string.Empty;
                    log.CodigoImpressao.Valor = 0;

                    if (!Convert.ToBoolean(bd.Executar(log.StringInserir())))
                        throw new BilheteriaException("Log de impressão do ingresso não foi inserido.");
                }

                bd.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
