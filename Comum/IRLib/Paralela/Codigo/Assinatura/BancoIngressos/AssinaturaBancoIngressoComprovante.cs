/**************************************************
* Arquivo: AssinaturaBancoIngressoComprovante.cs
* Gerado: 01/12/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.Assinaturas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IRLib.Paralela
{

    public class AssinaturaBancoIngressoComprovante : AssinaturaBancoIngressoComprovante_B
    {
        public enum AcaoComprovante
        {
            [Description("Doação")]
            Doar = 'D',
            [Description("Resgate")]
            Resgatar = 'R',
        }
        public AssinaturaBancoIngressoComprovante() { }


        public int EfetuarDoacao(List<int> ingressos, int clienteID, int usuarioID)
        {
            try
            {

                bd.BulkInsert(ingressos, "#tmpIngressos", false, true);
                List<int> bancoIngressoID = new List<int>();

                bd.Consulta(
                        @"
                            SELECT 
                                bi.ID
                            FROM tAssinaturaBancoIngresso bi (NOLOCK)
                            INNER JOIN #tmpIngressos ti ON ti.ID = bi.IngressoID 
                            WHERE bi.ClienteID = " + clienteID);

                if (!bd.Consulta().Read())
                    throw new Exception("Os ingressos selecionados não estão associados a você, caso já tenha efetuado a ação de doação ou devolução, por favor, verifique os comprovantes.");

                do { bancoIngressoID.Add(bd.LerInt("ID")); } while (bd.Consulta().Read());
                bd.FecharConsulta();

                if (bancoIngressoID.Count != ingressos.Count)
                    throw new Exception("Um dos ingressos selecionados não está associado a você, caso já tenha efetuado a ação de doação ou devolução, por favor, verifique os comprovantes.");

                bd.IniciarTransacao();

                this.ClienteID.Valor = clienteID;
                this.UsuarioID.Valor = usuarioID;
                this.Timestamp.Valor = DateTime.Now;
                this.Acao.Valor = ((char)AcaoComprovante.Doar).ToString();
                this.Inserir(bd);

                AssinaturaBancoIngressoHistorico oHistorico = new AssinaturaBancoIngressoHistorico();
                AssinaturaBancoIngressoCredito oCredito = new AssinaturaBancoIngressoCredito();

                foreach (var bancoIngresso in bancoIngressoID)
                {
                    if (bd.Executar(string.Format("UPDATE tAssinaturaBancoIngresso SET ClienteID = 0 WHERE ID = {0} AND ClienteID = {1}", bancoIngresso, clienteID)) != 1)
                        throw new Exception("Não foi possível efetuar a ação de doação de um dos ingressos selecionados, caso já tenha efetuado sua ação de doação ou devolução, por favor, verifique os comprovantes.");

                    oHistorico.Limpar();
                    oCredito.Limpar();

                    oCredito.ClienteID.Valor = clienteID;
                    oCredito.Utilizado.Valor = false;
                    oCredito.Inserir(bd);

                    oHistorico.AssinaturaBancoIngressoID.Valor = bancoIngresso;
                    oHistorico.AssinaturaBancoIngressoCreditoID.Valor = oCredito.Control.ID;
                    oHistorico.AssianturaBancoIngressoComprovanteID.Valor = this.Control.ID;
                    oHistorico.Inserir(bd);

                }

                bd.FinalizarTransacao();

                return this.Control.ID;
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

        public int EfetuarResgate(int clienteID, int usuarioID)
        {
            try
            {
                string sql =
                    @"
                        SELECT
                            DISTINCT bi.ID, bi.ClienteID
                        FROM tAssinaturaBancoIngressoResgate bir (NOLOCK)
                        INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = bir.AssinaturaBancoIngressoID                
                        WHERE bir.ClienteID = " + clienteID;

                List<int> ids = new List<int>();
                List<int> idsCredito = new List<int>();

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem ingressos a serem resgatados, caso você já tenha efetuado a ação de resgate, por favor, verifique os comprovantes.");

                int clienteID_aux = 0;
                do
                {
                    clienteID_aux = bd.LerInt("ClienteID");

                    if (clienteID_aux != 0)
                    {
                        if (clienteID_aux == clienteID)
                            throw new Exception("Você já efetuou a ação de resgate dos seus ingressos, por favor, verifique os comprovantes.");
                        else
                            throw new Exception("Um dos ingressos a serem resgatados já está vinculado a outro cliente, não será possível continuar, por favor, tente resgatar os ingressos novamente.");
                    }
                    ids.Add(bd.LerInt("ID"));
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                sql = "SELECT DISTINCT TOP " + ids.Count + " ID FROM tAssinaturaBancoIngressoCredito WHERE ClienteID = " + clienteID + " AND Utilizado = 'F'";

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Você não possui créditos para finalizar o processo de resgate.");

                do
                {
                    idsCredito.Add(bd.LerInt("ID"));
                } while (bd.Consulta().Read());

                if (idsCredito.Count < ids.Count)
                    throw new Exception("Seu saldo de crédito atual é de: " + idsCredito.Count + ", não será possível efetuar o resgate de: " + ids.Count + " ingresso(s)");

                bd.FecharConsulta();

                bd.IniciarTransacao();

                this.ClienteID.Valor = clienteID;
                this.UsuarioID.Valor = usuarioID;
                this.Timestamp.Valor = DateTime.Now;
                this.Acao.Valor = ((char)AcaoComprovante.Resgatar).ToString();
                this.Inserir(bd);

                AssinaturaBancoIngressoHistorico oHistorico = new AssinaturaBancoIngressoHistorico();

                foreach (var id in ids)
                {
                    if (bd.Executar(string.Format("UPDATE tAssinaturaBancoIngresso SET ClienteID = {0} WHERE ID = {1} AND ClienteID = 0", clienteID, id)) != 1)
                        throw new Exception("Um dos ingressos a serem resgatados já está vinculado a outro cliente, não será possível continuar, por favor, tente resgatar os ingressos novamente.");

                    int creditoID = idsCredito.FirstOrDefault();

                    bd.Executar("UPDATE tAssinaturaBancoIngressoCredito SET Utilizado = 'T' WHERE ID = " + creditoID + " AND ClienteID = " + clienteID + " AND Utilizado = 'F'");

                    oHistorico.Limpar();
                    oHistorico.AssinaturaBancoIngressoID.Valor = id;
                    oHistorico.AssinaturaBancoIngressoCreditoID.Valor = creditoID;
                    oHistorico.AssianturaBancoIngressoComprovanteID.Valor = this.Control.ID;
                    oHistorico.Inserir(bd);

                    idsCredito.Remove(creditoID);
                }

                bd.Executar("DELETE FROM tAssinaturaBancoIngressoResgate WHERE ClienteID = " + clienteID);

                bd.FinalizarTransacao();

                return this.Control.ID;
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

        public List<Comprovante> Comprovantes(int clienteID)
        {
            try
            {
                string sql = string.Format(@"
                        SELECT 
                            bic.ID, bic.Acao, bic.Timestamp, 
                            CASE WHEN bic.UsuarioID = 1657 OR bic.UsuarioID = 0
                                THEN c.Nome 
                                ELSE u.Nome
                            END AS Responsavel,
                            COUNT(bih.ID) AS Quantidade
                        FROM tAssinaturaBancoIngressoComprovante bic (NOLOCK)
                        INNER JOIN tAssinaturaBancoIngressoHistorico bih (NOLOCK) ON bih.AssianturaBancoIngressoComprovanteID = bic.ID
                        INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = bih.AssinaturaBancoIngressoID
                        INNER JOIN tCliente c (NOLOCK) ON c.ID = bic.ClienteID
                        LEFT JOIN tUsuario u (NOLOCK) ON u.ID = bic.UsuarioID
                        WHERE bic.ClienteID = {0}
                        GROUP BY bic.ID, bic.Acao, bic.Timestamp, bic.UsuarioID, c.Nome, u.Nome
                        ORDER BY bic.ID DESC", clienteID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Você não possui nenhum comprovante de doação, resgate ou devolução.");

                List<Comprovante> lista = new List<Comprovante>();

                do
                {
                    lista.Add(new Comprovante()
                    {
                        ID = bd.LerInt("ID"),
                        Acao = Utils.Enums.GetDescription<AcaoComprovante>(Utils.Enums.ParseCharEnum<AcaoComprovante>(bd.LerString("Acao"))),
                        TimeStamp = bd.LerDateTime("Timestamp"),
                        Responsavel = bd.LerString("Responsavel"),
                        Quantidade = bd.LerInt("Quantidade"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<ComprovanteItem> Comprovante(int id, int clienteID)
        {
            try
            {
                string sql = string.Format(
                    @"    
                        SELECT    
                            bi.ID, bi.ApresentacaoID, bic.Acao, bic.Timestamp, ap.Horario, a.Nome AS Assinatura, s.Nome AS Setor, i.Codigo, bi.ClienteID
                        FROM tAssinaturaBancoIngressoComprovante bic (NOLOCK)
                        INNER JOIN tAssinaturaBancoIngressoHistorico bih (NOLOCK) ON bic.ID = bih.AssianturaBancoIngressoComprovanteID
                        INNER JOIN tAssinaturaBancoIngresso bi (NOLOCK) ON bi.ID = bih.AssinaturaBancoIngressoID
                        INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
                        INNER JOIn tApresentacao ap (NOLOCK) ON ap.ID = bi.ApresentacaoID
                        INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                        INNER JOIN tAssinatura a (NOLOCK) ON a.ID = bi.AssinaturaID
                        WHERE bic.ID = {0} AND bic.ClienteID = {1}
                    
                    ", id, clienteID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível localizar o comprovante informado.");

                List<ComprovanteItem> lista = new List<ComprovanteItem>();
                do
                {
                    lista.Add(new ComprovanteItem()
                    {
                        ID = bd.LerInt("ID"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        Acao = (AcaoComprovante)Convert.ToChar(bd.LerString("Acao")),
                        TimeStamp = bd.LerDateTime("Timestamp"),
                        Horario = bd.LerDateTime("Horario"),
                        Assinatura = bd.LerString("Assinatura"),
                        Setor = bd.LerString("Setor"),
                        Codigo = bd.LerString("Codigo"),
                        Invalido = bd.LerInt("ClienteID") != clienteID,
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaBancoIngressoComprovanteLista : AssinaturaBancoIngressoComprovante_B
    {

        public AssinaturaBancoIngressoComprovanteLista() { }

    }

}
