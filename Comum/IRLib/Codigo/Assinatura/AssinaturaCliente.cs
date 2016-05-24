/**************************************************
* Arquivo: AssinaturaCliente.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Assinaturas.Models;
using IRLib.ClientObjects;
using IRLib.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IRLib
{

    public class AssinaturaCliente : AssinaturaCliente_B
    {
        public enum EnumStatus
        {
            Aguardando = 'A',
            RenovadoSemPagamento = 'S',
            Renovado = 'R',
            TrocaSinalizada = 'Z',
            Troca = 'T',
            TrocaSemPagamento = 'E',
            Desistido = 'D',
            Indisponivel = 'I',
            Aquisicao = 'N',
        }

        public enum EnumAcao
        {
            AguardandoAcao = 'A',
            Renovar = 'R',
            Desisistir = 'D',
            Trocar = 'T',
            EfetivarTroca = 'E',
            Aquisicao = 'N',
            Extinguir = 'X'
        }

        public static string tSQL_Update = "UPDATE tAssinaturaCliente SET Status = '{0}', Acao = '{1}', PrecoTipoID = {2},TimeStamp = '{3}' ,UsuarioID = {4}, VendaBilheteriaID = {6}, AgregadoID = {7}  WHERE ID = {5}";

        public AssinaturaCliente() { }

        public AssinaturaCliente(int usuarioID)
        {
            this.Control.UsuarioID = usuarioID;
        }

        public IEnumerable<IRLib.Assinaturas.Models.Assinatura> AssinaturasPorAcaoDoCliente(
            int assinaturaTipoID, int ano, int clienteID, EnumAcao acao, EnumStatus status, bool incluirIndisponiveis)
        {
            try
            {
                string sql =
                    string.Format(@"SELECT 
						DISTINCT
						a.ID AS AssinaturaID, a.Nome AS Assinatura, a.BloqueioID,
						an.ID AS AssinaturaAnoID,
						s.ID AS SetorID, s.Nome AS Setor,
						l.ID AS LugarID, l.Codigo,
						ac.ID AS AssinaturaClienteID, ac.Status, ac.Acao, ac.PrecoTipoID
						FROM tAssinatura a (NOLOCK)
						INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
						INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
						INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
						INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.AssinaturaAnoID = an.ID AND ac.SetorID = s.ID
						INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
						WHERE 
							a.AssinaturaTipoID = {0} AND a.Ativo = 'T' 
							AND an.Ano = '{1}' AND ac.ClienteID = {2} AND ac.Acao = '{3}' AND (ac.Status = '{4}' {5})
						ORDER BY a.Nome, s.Nome, l.Codigo",
                                                          assinaturaTipoID, ano, clienteID, (char)acao, (char)status,
                                                          incluirIndisponiveis ? "OR ac.Status = '" + (char)EnumStatus.Indisponivel + "' " : string.Empty);

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return null;

                var lista = new List<IRLib.Assinaturas.Models.Assinatura>();

                do
                {
                    lista.Add(new IRLib.Assinaturas.Models.Assinatura()
                    {
                        AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        Nome = bd.LerString("Assinatura"),
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("Setor"),
                        LugarID = bd.LerInt("LugarID"),
                        Lugar = bd.LerString("Codigo"),
                        Status = (EnumStatus)Convert.ToChar(bd.LerString("Status")),
                        Acao = (EnumAcao)Convert.ToChar(bd.LerString("Acao")),
                        PrecoTipoID = bd.LerInt("PrecoTipoID"),
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                foreach (var item in lista)
                    item.Precos = this.BuscarPrecos(item.AssinaturaClienteID, ano, "Selecione...");

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IEnumerable<IRLib.Assinaturas.Models.Assinatura> AssinaturasParaTrocarDoCliente(
            int assinaturaTipoID, int ano, int clienteID, bool trocaPrioritaria)
        {
            try
            {
                string filtro = string.Empty;
                string sql = string.Empty;

                if (trocaPrioritaria)
                {
                    sql = "SELECT COUNT(ac.ID) FROM tAssinaturaCliente ac INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID WHERE a.Ativo = 'T' AND an.Ano = '" + ano + "' AND ac.ClienteID = " + clienteID + " AND ac.Status = '" + (char)EnumStatus.Indisponivel + "'";
                    if (Convert.ToInt32(bd.ConsultaValor(sql)) == 0)
                        return null;
                }


                filtro = string.Format(
                    "(ac.Status = '{0}' OR ac.Status = '{1}' OR (ac.Acao = '{2}' AND ac.Status = '{3}') OR ac.Status = '{4}')",
                     (char)AssinaturaCliente.EnumStatus.RenovadoSemPagamento,
                    (char)AssinaturaCliente.EnumStatus.TrocaSinalizada,
                    (char)AssinaturaCliente.EnumAcao.EfetivarTroca,
                    (char)AssinaturaCliente.EnumStatus.Aguardando,
                    (char)AssinaturaCliente.EnumStatus.Indisponivel);

                sql =
                   string.Format(@"SELECT      
							a.ID AS AssinaturaID, a.Nome AS Assinatura, a.BloqueioID,
							an.ID AS AssinaturaAnoID,
							s.ID AS SetorID, s.Nome AS Setor,
							l.ID AS LugarID, l.Codigo,
							ac.ID AS AssinaturaClienteID, ac.AssinaturaClienteID AS RelacionadoAssinaturaClienteID, ac.Status, 
							ac.Acao, ac.PrecoTipoID
						FROM tAssinaturaCliente ac (NOLOCK)
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
						INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
						INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
						INNER JOIN tLugar l (NOLOCK) ON l.ID = i.LugarID
						WHERE 
							a.AssinaturaTipoID = {0} AND a.Ativo = 'T' 
							AND an.Ano = '{1}' AND ac.ClienteID = {2} 
							AND {3}
						GROUP BY
							a.ID, a.Nome, a.BloqueioID, an.ID, s.ID, s.Nome, l.ID, l.Codigo, ac.ID, ac.AssinaturaClienteID, 
							ac.Status, ac.Acao, ac.PrecoTipoID
						ORDER BY a.Nome, s.Nome, l.Codigo",
                       assinaturaTipoID, ano, clienteID, filtro);

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    return null;

                var lista = new List<IRLib.Assinaturas.Models.Assinatura>();

                do
                {
                    lista.Add(new IRLib.Assinaturas.Models.Assinatura()
                    {
                        AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        Nome = bd.LerString("Assinatura"),
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("Setor"),
                        LugarID = bd.LerInt("LugarID"),
                        Lugar = bd.LerString("Codigo"),
                        Status = (EnumStatus)Convert.ToChar(bd.LerString("Status")),
                        Acao = (EnumAcao)Convert.ToChar(bd.LerString("Acao")),
                        RelacionadoAssinaturaClienteID = bd.LerInt("RelacionadoAssinaturaClienteID"),
                        PrecoTipoID = bd.LerInt("PrecoTipoID"),
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                foreach (var item in lista.Where(c => c.RelacionadoAssinaturaClienteID > 0 || c.Acao == EnumAcao.Trocar || c.Acao == EnumAcao.Renovar))
                    item.Precos = this.BuscarPrecos(item.AssinaturaClienteID, ano, "Selecione...");

                //Precisa verificar se o bloqueioID dos lugares é igual ao definido, do contrario o lugar não está mais disponivel e não pode fazer acao

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public IEnumerable<IRLib.Assinaturas.Models.Aqusicao> AssinaturasDoCliente(int assinaturaTipoID, int ano, int clienteID)
        {
            try
            {
                string sql =
                   string.Format(@"SELECT
							 ac.ID AS AssinaturaClienteID, a.ID AS AssinaturaID, a.Nome AS Assinatura, 
							s.ID AS SetorID, s.Nome AS Setor, l.ID AS LugarID, l.Codigo, pt.Nome AS PrecoTipo, 
								SUM(p.Valor) AS Valor, ac.Acao, ac.Status, ac.agregadoid
							FROM tAssinaturaCliente ac (NOLOCK)
							INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
							INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
							LEFT JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = ac.AssinaturaAnoID AND ai.SetorID = ac.SetorID AND ai.PrecoTipoID = ac.PrecoTipoID
							INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
							LEFT JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.LugarID = ac.LugarID
							LEFT JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = i.ApresentacaoSetorID AND p.PrecoTipoID = ac.PrecoTipoID
							LEFT JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = p.PrecoTipoID
							INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
								WHERE ac.ClienteID = {0} AND a.AssinaturaTipoID = {1} AND an.Ano = '{2}' AND ac.Acao <> '{3}'
							GROUP BY ac.ID, a.ID, a.Nome, s.ID, s.Nome, l.ID, l.Codigo, pt.Nome, ac.Acao, ac.Status, ac.agregadoid
							ORDER BY a.Nome, s.Nome, l.Codigo
						 ", clienteID, assinaturaTipoID, ano, IRLib.Utils.Enums.GetChar(EnumAcao.Desisistir));

                bd.Consulta(sql);

                //Não tem nenhuma assinatura ativa
                if (!bd.Consulta().Read())
                    return null;
                List<IRLib.Assinaturas.Models.Aqusicao> lista = new List<Assinaturas.Models.Aqusicao>();
                do
                {
                    lista.Add(new Assinaturas.Models.Aqusicao()
                    {
                        AssinaturaClienteID = bd.LerInt("AssinaturaClienteID"),
                        AssinaturaID = bd.LerInt("AssinaturaID"),
                        Assinatura = bd.LerString("Assinatura"),
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("Setor"),
                        LugarID = bd.LerInt("LugarID"),
                        Lugar = bd.LerString("Codigo"),
                        PrecoTipo = bd.LerString("PrecoTipo"),
                        Valor = bd.LerDecimal("Valor"),
                        Acao = (EnumAcao)Convert.ToChar(bd.LerString("Acao")),
                        Status = (EnumStatus)Convert.ToChar(bd.LerString("Status")),
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

        public IRLib.Assinaturas.Models.Aqusicao ProgramacaoAssinatura(int assinaturaID)
        {
            Assinatura oAssinatura = new Assinatura();
            string caminhoImg = new ConfigGerenciador().getDownloadPathAssinatura();
            string imgAssinatura = oAssinatura.GetNomeImg(assinaturaID);
            string caminhoTotal = caminhoImg + imgAssinatura;

            IRLib.Assinaturas.Models.Aqusicao retorno = new Assinaturas.Models.Aqusicao()
                {

                    AssinaturaID = assinaturaID,
                    Programacao = caminhoTotal

                };

            return retorno;
        }

       
        public List<IRLib.ClientObjects.Assinaturas.EstruturaAssinaturaPreco> BuscarPrecos(int assinaturaClienteID, int ano, string primeiro)
        {
            var lista = this.BuscarPrecos(assinaturaClienteID, ano);
            lista.Insert(0, new ClientObjects.Assinaturas.EstruturaAssinaturaPreco()
            {
                AssinaturaClienteID = assinaturaClienteID,
                ID = 0,
                Nome = primeiro,
                Valor = 0
            });
            return lista;
        }

        public List<IRLib.ClientObjects.Assinaturas.EstruturaAssinaturaPreco> BuscarPrecos(int assinaturaClienteID, int ano)
        {
            try
            {
                string sql =
                    string.Format(
                        @"SELECT 
							pt.ID, pt.Nome, SUM(p.Valor) AS Valor
						FROM tAssinaturaCliente ac (NOLOCK)
						INNER JOIN tAssinaturaAno an (NOLOCK) ON ac.AssinaturaAnoID = an.ID
						INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID AND ai.SetorID = ac.SetorID
						INNER JOIN tPrecoTipo pt (NOLOCK) ON pt.ID = ai.PrecoTipoID
						INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ai.ApresentacaoID AND aps.SetorID = ai.SetorID
						INNER JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = aps.ID AND ai.PrecoTipoID = p.PrecoTipoID
						WHERE ac.ID = {0} AND an.Ano = '{1}'
						GROUP BY pt.ID, pt.Nome
						ORDER BY pt.Nome
						", assinaturaClienteID, ano);

                bd.Consulta(sql);
                if (!bd.Consulta().Read())
                    throw new Exception("Não existem preços habilitados para esta assinatura.");

                List<IRLib.ClientObjects.Assinaturas.EstruturaAssinaturaPreco> lista = new List<IRLib.ClientObjects.Assinaturas.EstruturaAssinaturaPreco>();

                do
                {
                    lista.Add(new IRLib.ClientObjects.Assinaturas.EstruturaAssinaturaPreco()
                    {
                        AssinaturaClienteID = assinaturaClienteID,
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Valor = bd.LerDecimal("Valor"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public static IEnumerable<EstruturaIDNome> AcoesCliente()
        {
            yield return new EstruturaIDNome() { ID = 1, Nome = "Renovar" };
            yield return new EstruturaIDNome() { ID = 2, Nome = "Trocar" };
            yield return new EstruturaIDNome() { ID = 3, Nome = "Desistir" };
        }

        public static EnumAcao ToAcao(string acao)
        {
            switch (acao.ToLower())
            {
                case "r":
                case "renovar":
                    return EnumAcao.Renovar;
                case "t":
                case "trocar":
                    return EnumAcao.Trocar;
                case "d":
                case "desistir":
                    return EnumAcao.Desisistir;
                case "eftivartroca":
                    return EnumAcao.EfetivarTroca;
                case "aquisicao":
                    return EnumAcao.Aquisicao;

                default:
                    throw new Exception("Ação não encontrada.");
            }
        }

        public void EfetuarAcoes(int clienteID,
                                   int usuarioID,
                                   Assinaturas.Models.AcaoProvisoria item, int vendaBilheteriaID)
        {
            if (item == null)
                throw new ArgumentException("Objeto item não pode ser nulo em EfetuarAcoes");

            var lista = new List<Assinaturas.Models.AcaoProvisoria>();
            lista.Add(item);
            this.EfetuarAcoes(clienteID, usuarioID, lista, vendaBilheteriaID);
        }

        public void EfetuarAcoes(int clienteID,
                                   int usuarioID,
                                   List<Assinaturas.Models.AcaoProvisoria> lista, int vendaBilheteriaID)
        {

            try
            {
                bd.IniciarTransacao();

                this.EfetuarAcoes(bd, clienteID, usuarioID, lista, vendaBilheteriaID);

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

        public void EfetuarAcoes(BD bd, int clienteID,
                                    int usuarioID,
                                    Assinaturas.Models.AcaoProvisoria item, int vendaBilheteriaID)
        {

            if (item == null)
                throw new ArgumentException("Objeto item não pode ser nulo em EfetuarAcoes");

            var lista = new List<Assinaturas.Models.AcaoProvisoria>();
            lista.Add(item);


            this.EfetuarAcoes(bd, clienteID, usuarioID, lista, vendaBilheteriaID);

        }

        public void EfetuarAcoes(BD bd, int clienteID,
                                    int usuarioID,
                                    List<Assinaturas.Models.AcaoProvisoria> lista, int vendaBilheteriaID)
        {


            //Já tem as ações, precisa deixar como processado pra não passar pelo consolidado e pagamento dnovo
            new AssinaturaAcaoProvisoria().ExecutarAcoes(bd, clienteID);

            string update = AssinaturaCliente.tSQL_Update;

            AssinaturaHistorico oAssinaturaHistorico = new AssinaturaHistorico();

            foreach (var item in lista)
            {
                oAssinaturaHistorico.Limpar();

                oAssinaturaHistorico.AssinaturaClienteID.Valor = item.AssinaturaClienteID;
                oAssinaturaHistorico.VendaBilheteriaID.Valor = vendaBilheteriaID;
                oAssinaturaHistorico.Acao.Valor = ((char)item.Acao).ToString();
                oAssinaturaHistorico.TimeStamp.Valor = DateTime.Now;
                oAssinaturaHistorico.UsuarioID.Valor = usuarioID;

                switch (item.Acao)
                {
                    case EnumAcao.Renovar:

                        var status = vendaBilheteriaID == 0 ? (char)AssinaturaCliente.EnumStatus.RenovadoSemPagamento : (char)AssinaturaCliente.EnumStatus.Renovado;

                        bd.Executar(string.Format(update, status, (char)AssinaturaCliente.EnumAcao.Renovar, item.PrecoTipoID, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, vendaBilheteriaID.ToString(), item.AgregadoID.ToString()));//VERIFICAR SE VBID PODE FICAR EM BRANCO

                        oAssinaturaHistorico.Status.Valor = ((char)status).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        if (vendaBilheteriaID > 0)
                            bd.Executar(string.Format("UPDATE tAssinaturaCliente SET VendaBilheteriaID = {0} WHERE ID = {1}", vendaBilheteriaID, item.AssinaturaClienteID));

                        break;
                    case EnumAcao.Desisistir:

                        //var statusDesistencia = item.Status == EnumStatus.Indisponivel ? item.Status : EnumStatus.Desistido;

                        bd.Executar(string.Format(update, (char)EnumStatus.Desistido, (char)AssinaturaCliente.EnumAcao.Desisistir, 0, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, vendaBilheteriaID.ToString(), item.AgregadoID.ToString()));

                        oAssinaturaHistorico.Status.Valor = ((char)EnumStatus.Desistido).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        break;
                    case EnumAcao.Trocar:

                        var statusSinalizar = item.Status == EnumStatus.Indisponivel ? item.Status : EnumStatus.TrocaSinalizada;

                        bd.Executar(string.Format(update, (char)statusSinalizar, (char)AssinaturaCliente.EnumAcao.Trocar, 0, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, vendaBilheteriaID.ToString(), item.AgregadoID.ToString()));
                        oAssinaturaHistorico.Status.Valor = ((char)statusSinalizar).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        break;
                    case EnumAcao.EfetivarTroca:

                        var statusTroca = vendaBilheteriaID == 0 ? (char)AssinaturaCliente.EnumStatus.TrocaSemPagamento : (char)AssinaturaCliente.EnumStatus.Troca;

                        bd.Executar(string.Format(update, (char)statusTroca, (char)AssinaturaCliente.EnumAcao.EfetivarTroca, item.PrecoTipoID, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, vendaBilheteriaID.ToString(), item.AgregadoID.ToString()));

                        oAssinaturaHistorico.Status.Valor = ((char)statusTroca).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        bd.Executar(string.Format("UPDATE tAssinaturaCliente SET VendaBilheteriaID = {0} WHERE ID = {1}", vendaBilheteriaID, item.AssinaturaClienteID));

                        break;
                    case EnumAcao.Aquisicao:

                        if (vendaBilheteriaID == 0)
                            throw new Exception("Houve uma falha no processamento, tentativa de efetuar uma aquisição sem o pagamento.");

                        bd.Executar(string.Format(update, (char)AssinaturaCliente.EnumStatus.Aquisicao, (char)AssinaturaCliente.EnumAcao.Aquisicao, item.PrecoTipoID, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, vendaBilheteriaID.ToString(), item.AgregadoID.ToString()));

                        oAssinaturaHistorico.Status.Valor = ((char)AssinaturaCliente.EnumStatus.Aquisicao).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        bd.Executar(string.Format("UPDATE tAssinaturaCliente SET VendaBilheteriaID = {0} WHERE ID = {1}", vendaBilheteriaID, item.AssinaturaClienteID));

                        break;
                    case EnumAcao.AguardandoAcao:
                        this.ClienteID.Valor = item.ClienteID;
                        this.AssinaturaID.Valor = item.AssinaturaID;
                        this.LugarID.Valor = item.LugarID;
                        this.AssinaturaAnoID.Valor = item.AssinaturaAnoID;

                        this.Status.Valor = ((char)AssinaturaCliente.EnumStatus.Aguardando).ToString();
                        this.Acao.Valor = ((char)AssinaturaCliente.EnumAcao.AguardandoAcao).ToString();
                        this.SetorID.Valor = item.SetorID;
                        this.VendaBilheteriaID.Valor = 0;
                        this.PrecoTipoID.Valor = 0;
                        this.AssinaturaClienteID.Valor = 0;
                        this.TimeStamp.Valor = DateTime.Now;
                        this.UsuarioID.Valor = usuarioID;

                        this.Inserir(bd);

                        oAssinaturaHistorico.AssinaturaClienteID.Valor = this.Control.ID;
                        oAssinaturaHistorico.Status.Valor = ((char)AssinaturaCliente.EnumStatus.Aguardando).ToString();

                        oAssinaturaHistorico.Inserir(bd);

                        break;
                    case EnumAcao.Extinguir:
                        bd.Executar(string.Format("UPDATE tAssinaturaCliente SET Status = '{0}' , TimeStamp = '{1}', UsuarioID = {2} WHERE ID = {3}", Utils.Enums.GetChar(EnumStatus.Indisponivel), DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID));
                        //TODO: Mudar o tIngresso.BloqueioID = tAssinatura.ExtintoBloqueioID
                        break;
                    default:
                        throw new Exception("Ação não definida.");
                }
            }
        }

        public List<Assinaturas.Models.AcaoProvisoria> EfetuarAcoes(BD bd, int clienteID,
                                    int usuarioID,
                                    List<Assinaturas.Models.AcaoProvisoria> lista)
        {


            //Já tem as ações, precisa deixar como processado pra não passar pelo consolidado e pagamento dnovo
            new AssinaturaAcaoProvisoria().ExecutarAcoes(bd, clienteID);

            string update = AssinaturaCliente.tSQL_Update;

            AssinaturaHistorico oAssinaturaHistorico = new AssinaturaHistorico();

            foreach (var item in lista)
            {
                oAssinaturaHistorico.Limpar();

                oAssinaturaHistorico.AssinaturaClienteID.Valor = item.AssinaturaClienteID;
                oAssinaturaHistorico.VendaBilheteriaID.Valor = 0;
                oAssinaturaHistorico.Acao.Valor = ((char)item.Acao).ToString();
                oAssinaturaHistorico.TimeStamp.Valor = DateTime.Now;
                oAssinaturaHistorico.UsuarioID.Valor = usuarioID;

                switch (item.Acao)
                {
                    case EnumAcao.Trocar:

                        var statusSinalizar = item.Status == EnumStatus.Indisponivel ? item.Status : EnumStatus.TrocaSinalizada;

                        bd.Executar(string.Format(update, (char)statusSinalizar, (char)AssinaturaCliente.EnumAcao.Trocar, 0, DateTime.Now.ToString("yyyyMMddHHmmss"), usuarioID, item.AssinaturaClienteID, item.AgregadoID.ToString()));
                        oAssinaturaHistorico.Status.Valor = ((char)statusSinalizar).ToString();
                        oAssinaturaHistorico.Inserir(bd);

                        break;
                    case EnumAcao.AguardandoAcao:
                        this.Limpar();
                        this.ClienteID.Valor = item.ClienteID;
                        this.AssinaturaID.Valor = item.AssinaturaID;
                        this.LugarID.Valor = item.LugarID;
                        this.AssinaturaAnoID.Valor = item.AssinaturaAnoID;

                        this.Status.Valor = ((char)AssinaturaCliente.EnumStatus.Aguardando).ToString();
                        this.Acao.Valor = ((char)AssinaturaCliente.EnumAcao.AguardandoAcao).ToString();
                        this.SetorID.Valor = item.SetorID;
                        this.VendaBilheteriaID.Valor = 0;
                        this.PrecoTipoID.Valor = 0;
                        this.AssinaturaClienteID.Valor = 0;
                        this.TimeStamp.Valor = DateTime.Now;
                        this.UsuarioID.Valor = usuarioID;

                        this.Inserir(bd);

                        item.AssinaturaClienteID = this.Control.ID;

                        oAssinaturaHistorico.AssinaturaClienteID.Valor = this.Control.ID;
                        oAssinaturaHistorico.Status.Valor = ((char)AssinaturaCliente.EnumStatus.Aguardando).ToString();

                        oAssinaturaHistorico.Inserir(bd);

                        break;
                }
            }

            return lista;
        }

        public string Vender(int clienteID, int usuarioID, string formaPagamento, int Parcelas,
                            string numeroCartao, string dataValidade, string codigo,
                            EstruturaReservaInternet estruturaIdentificaoUsuario, List<IRLib.Assinaturas.Models.AcaoProvisoria> listaAcoes,
                            decimal desconto, int entregaID, decimal valorEntrega, bool valorEntregaFixo, int SenhaID, string email)
        {
            try
            {
                bd.IniciarTransacao();

                string senha = this.Vender(bd, clienteID, usuarioID, formaPagamento, Parcelas, numeroCartao, dataValidade, codigo, estruturaIdentificaoUsuario, listaAcoes, desconto, entregaID, valorEntrega, valorEntregaFixo, SenhaID, email);

                bd.FinalizarTransacao();

                return senha;
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

        public string Vender(BD bd, int clienteID, int usuarioID, string formaPagamento, int Parcelas,
                    string numeroCartao, string dataValidade, string codigo,
                    EstruturaReservaInternet estruturaIdentificaoUsuario, List<IRLib.Assinaturas.Models.AcaoProvisoria> listaAcoes,
                    decimal desconto, int entregaID, decimal valorEntrega, bool valorEntregaFixo, int SenhaID, string email)
        {
            IRLib.Sitef oSitef = new IRLib.Sitef();
            bool utilizarTef = false;
            bool tefAtivo = Convert.ToBoolean(ConfigurationManager.AppSettings["TEFAtivo"]);
            bool abriuTransacaoTEF = false;
            BD bdAux = new BD();

            try
            {
                if (estruturaIdentificaoUsuario.CaixaID == 0)
                    throw new Exception("Não será possível continuar, o caixa não está aberto.");

                EstruturaVendaFormaPagamento oFormaPagamento = new IRLib.FormaPagamento().getFormaPagamentoVendaAssinatura(formaPagamento, Parcelas, estruturaIdentificaoUsuario.EmpresaID, clienteID, usuarioID);

                ClienteEndereco oClienteEndereco = new ClienteEndereco();

                Entrega oEntrega = new Entrega();
                oEntrega.Ler(entregaID);

                EntregaControle oEntegaControle = new EntregaControle();
                int clienteEnderecoID = 0;

                if (oEntrega.Tipo.Valor != Entrega.RETIRADABILHETERIA)
                {
                    oClienteEndereco.BuscarPorCliente(clienteID);
                    clienteEnderecoID = oClienteEndereco.Control.ID;

                    oEntegaControle.BuscarPorEnderecoEntrega(oClienteEndereco.CEP.Valor, entregaID);
                }
                else
                    oEntegaControle.BuscarPorEntregaID(entregaID);

                valorEntrega = valorEntregaFixo ? valorEntrega : oEntegaControle.Valor.Valor;

                utilizarTef = numeroCartao.Length > 0 && tefAtivo;

                string sqlBusca =
                    string.Format(
                        @"SELECT 
						   i.ID as IngressoID, Codigo, i.EventoID, i.ApresentacaoID, i.Status, i.BloqueioID,
						   i.ApresentacaoSetorID, i.SetorID, p.ID AS PrecoID, i.LugarID, p.Valor AS PrecoValor, acp.Acao, ac.Status AS StatusAssinaturaCliente, DesistenciaBloqueioID, ExtintoBloqueioID, i.AssinaturaClienteID,
                           acp.AgregadoID as AgregadoID
						FROM tAssinaturaAcaoProvisoria acp (NOLOCK) 
						INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = acp.AssinaturaClienteID
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
						LEFT JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = i.ApresentacaoSetorID AND p.PrecoTipoID = acp.PrecoTipoID
						WHERE acp.ClienteID = {0} AND i.ClienteID = {0} AND acp.Processado = 'F' AND acp.Acao <> 'T'
					", clienteID);

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
                    PrecoID = 0,
                    LugarID = 0,
                    PrecoValor = (decimal)0.00,
                    Acao = EnumAcao.AguardandoAcao,
                    StatusAssinaturaCliente = EnumStatus.Aguardando,
                    DesistenciaBloqueioID = 0,
                    ExtintoBloqueioID = 0,
                    AssinaturaClienteID = 0,
                    AgregadoID = -1
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
                        PrecoID = bdAux.LerInt("PrecoID"),
                        LugarID = bdAux.LerInt("LugarID"),
                        PrecoValor = bdAux.LerDecimal("PrecoValor"),
                        Acao = (EnumAcao)Convert.ToChar(bdAux.LerString("Acao")),
                        StatusAssinaturaCliente = (EnumStatus)Convert.ToChar(bdAux.LerString("StatusAssinaturaCliente")),
                        DesistenciaBloqueioID = bdAux.LerInt("DesistenciaBloqueioID"),
                        ExtintoBloqueioID = bdAux.LerInt("ExtintoBloqueioID"),
                        AssinaturaClienteID = bdAux.LerInt("AssinaturaClienteID"),
                        AgregadoID = bdAux.LerInt("AgregadoID")
                    });

                    listaIngressos.Add(ingresso);
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();
                bdAux.Fechar();

                if (listaAcoes.Count != listaIngressos.Select(c => c.AssinaturaClienteID).Distinct().Count())
                    throw new Exception("Não foi possível encontrar todos os ingressos de sua assinatura, por favor, entre em contato com a ingresso rápido.");

                decimal valorTotal = (listaIngressos.Where(c => c.Acao != EnumAcao.Desisistir).Sum(c => c.PrecoValor) + valorEntrega);

                if (utilizarTef)
                {
                    oSitef.terminal = "I" + (clienteID > 9999999 ? new Random().Next(9999999).ToString("0000000") : clienteID.ToString("0000000"));
                    oSitef.Terminal = IRLib.Sitef.enumTerminal.SiteIR;
                    oSitef.Empresa = IRLib.Sitef.enumEmpresa.IngressoRapido;
                    oSitef.ValorCompra = (valorTotal - desconto).ToString("#.00");
                    oSitef.Parcelas = Parcelas.ToString();
                    oSitef.ClienteID = "0";
                    oSitef.NumeroCartao = numeroCartao;
                    oSitef.DataVencimento = dataValidade.ToString();
                    oSitef.CodigoSeguranca = codigo;
                    oSitef.TipoFinanciamento = IRLib.Sitef.enumTipoFinanciamento.Estabelecimento;
                    oSitef.IniciaSitef();

                    abriuTransacaoTEF = true;
                }

                VendaBilheteria oVendaBilheteira = new VendaBilheteria();
                oVendaBilheteira.ClienteID.Valor = clienteID;
                oVendaBilheteira.CaixaID.Valor = estruturaIdentificaoUsuario.CaixaID;
                oVendaBilheteira.DataVenda.Valor = DateTime.Now;
                oVendaBilheteira.ClienteEnderecoID.Valor = clienteEnderecoID;
                oVendaBilheteira.EntregaControleID.Valor = oEntegaControle.Control.ID;
                oVendaBilheteira.PdvID.Valor = 0;
                oVendaBilheteira.TaxaEntregaValor.Valor = valorEntrega;
                oVendaBilheteira.PagamentoProcessado.Valor = oFormaPagamento.Tipo != (int)IRLib.FormaPagamento.TIPO.Boleto;
                oVendaBilheteira.ValorTotal.Valor = valorTotal;
                oVendaBilheteira.TaxaEntregaValor.Valor = valorEntrega;
                oVendaBilheteira.TaxaConvenienciaValorTotal.Valor = 0;
                oVendaBilheteira.TaxaConvenienciaValorTotal.Valor = 0;
                oVendaBilheteira.Status.Valor = VendaBilheteria.PAGO;
                oVendaBilheteira.IP.Valor = string.Empty;
                oVendaBilheteira.NomeCartao.Valor = string.Empty;
                oVendaBilheteira.QuantidadeImpressoesInternet.Valor = 0;
                oVendaBilheteira.VendaBilhereriaIDTroca.Valor = SenhaID;

                //Se tem tef, precisa colocar a nota fiscal
                oVendaBilheteira.NotaFiscalCliente.Valor = oVendaBilheteira.NotaFiscalEstabelecimento.Valor = oSitef.CupomFiscal;


                string sqlVendaBilheteria = oVendaBilheteira.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                oVendaBilheteira.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (oVendaBilheteira.Control.ID == 0)
                    throw new Exception("Não foi possível gerar o seu registro de venda, por favor tente novamente.");

                VendaBilheteriaItem oVendaBilheteriaItem = new VendaBilheteriaItem();
                IngressoLog oIngressoLog = new IngressoLog();


                //Desistencias
                foreach (var ingressosDesistencia in listaIngressos.Where(c => c.Acao == EnumAcao.Desisistir)
                    .Select(c => new { StatusAssinaturaCliente = c.StatusAssinaturaCliente, IngressoID = c.IngressoID, DesistenciaBloqueioID = c.DesistenciaBloqueioID, ExtintoBloqueioID = c.ExtintoBloqueioID, }).Distinct())
                    if (bd.Executar(this.StringDesistirIngresso(ingressosDesistencia.StatusAssinaturaCliente, ingressosDesistencia.IngressoID, ingressosDesistencia.DesistenciaBloqueioID, ingressosDesistencia.ExtintoBloqueioID, clienteID)) != 1)
                        throw new Exception("Não foi possível gerar o registro de desistência do seu ingresso, por favor tente novamente.");

                //Adiquirencia
                foreach (var ingresso in listaIngressos.Where(c => c.Acao != EnumAcao.Desisistir && c.Acao != EnumAcao.Trocar))
                {
                    oVendaBilheteriaItem.Limpar();
                    oVendaBilheteriaItem.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    oVendaBilheteriaItem.PacoteID.Valor = 0;
                    oVendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    oVendaBilheteriaItem.TaxaConveniencia.Valor = 0;
                    oVendaBilheteriaItem.TaxaConvenienciaValor.Valor = 0;
                    oVendaBilheteriaItem.TaxaComissao.Valor = 0;
                    oVendaBilheteriaItem.ComissaoValor.Valor = 0;

                    string sqlVendaBilheteriaItem = oVendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    oVendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (oVendaBilheteriaItem.Control.ID == 0)
                        throw new Exception("Não foi possível processar a venda. Ocorreu um erro ao incluir os itens.");

                    StringBuilder stb = new StringBuilder();
                    stb.Append("UPDATE tIngresso SET LojaID=" + estruturaIdentificaoUsuario.LojaID + ", PrecoID =" + ingresso.PrecoID + ", UsuarioID = " + estruturaIdentificaoUsuario.UsuarioID + ", ");
                    stb.Append("VendaBilheteriaID=" + oVendaBilheteira.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '', CortesiaID = 0, TimeStampReserva ='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ");
                    stb.Append("WHERE (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "' OR Status='" + Ingresso.RESERVADO + "') AND ClienteID=" + clienteID + " AND ID=" + ingresso.IngressoID);
                    int x = bd.Executar(stb.ToString());
                    if (x == 0)
                        throw new Exception("Não foi possível atualizar os registros do seu ingresso.");

                    oIngressoLog.Limpar();
                    oIngressoLog.TimeStamp.Valor = System.DateTime.Now;
                    oIngressoLog.IngressoID.Valor = ingresso.IngressoID;
                    oIngressoLog.UsuarioID.Valor = estruturaIdentificaoUsuario.UsuarioID; //usuario fixo para Internet
                    oIngressoLog.BloqueioID.Valor = ingresso.BloqueioID;
                    oIngressoLog.CortesiaID.Valor = 0;
                    oIngressoLog.PrecoID.Valor = ingresso.PrecoID;
                    oIngressoLog.VendaBilheteriaItemID.Valor = oVendaBilheteriaItem.Control.ID;
                    oIngressoLog.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    oIngressoLog.CaixaID.Valor = estruturaIdentificaoUsuario.CaixaID; // ABERTURA E FECHAMENTO DE CAIXA DIARIO PARA INTERNET
                    oIngressoLog.LojaID.Valor = estruturaIdentificaoUsuario.LojaID; //loja fixa Internet
                    oIngressoLog.CanalID.Valor = estruturaIdentificaoUsuario.CanalID; //canal fixo Internet
                    oIngressoLog.EmpresaID.Valor = estruturaIdentificaoUsuario.EmpresaID; // FIXO IR
                    oIngressoLog.ClienteID.Valor = clienteID;
                    oIngressoLog.Acao.Valor = IngressoLog.VENDER;
                    oIngressoLog.GerenciamentoIngressosID.Valor = 0;
                    oIngressoLog.AssinaturaClienteID.Valor = ingresso.AssinaturaClienteID;

                    string sqlIngressoLogV = oIngressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new Exception("Log de venda do ingresso não foi inserido.");
                }

                VendaBilheteriaFormaPagamento oVendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                if (valorTotal > 0)
                {
                    oVendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    oVendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    oVendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;//Precisa Achar a FormaPagamentoID
                    oVendaBilheteriaFormaPagamento.Valor.Valor = valorTotal - desconto;
                    oVendaBilheteriaFormaPagamento.Porcentagem.Valor = ((valorTotal - desconto) * 100) / (valorTotal);
                    oVendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;

                    if (utilizarTef)
                    {
                        oVendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oSitef.ID;
                        oVendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oSitef.CodigoRespostaSitefVenda;
                        oVendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oSitef.MensagemFinaliza;
                        oVendaBilheteriaFormaPagamento.HoraTransacao.Valor = oSitef.HoraTransacao;
                        oVendaBilheteriaFormaPagamento.DataTransacao.Valor = oSitef.DataTransacao;
                        oVendaBilheteriaFormaPagamento.CodigoIR.Valor = oSitef.CodigoIR;
                        oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oSitef.NumeroAutorizacao;
                        oVendaBilheteriaFormaPagamento.NSUHost.Valor = oSitef.NSUHost;
                        oVendaBilheteriaFormaPagamento.NSUSitef.Valor = oSitef.NSUSitef;
                        oVendaBilheteriaFormaPagamento.Cupom.Valor = oSitef.CupomFiscal;
                        oVendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oSitef.DadosConfirmacao;
                        oVendaBilheteriaFormaPagamento.Rede.Valor = oSitef.RedeRetorno;
                        oVendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oSitef.CodigoRespostaSitefFinaliza;
                    }

                    int vendaBilheteriaFormaPagamentoID = Convert.ToInt32(bd.ConsultaValor(oVendaBilheteriaFormaPagamento.StringInserir()));

                    VendaBilheteriaFormaPagamentoBoleto oFormaPagamentoBoleto = new VendaBilheteriaFormaPagamentoBoleto();

                    if (oFormaPagamento.Tipo == (int)IRLib.FormaPagamento.TIPO.Boleto)
                        oFormaPagamentoBoleto.EnviarBoletos(bd, Parcelas, valorTotal - desconto, vendaBilheteriaFormaPagamentoID, listaAcoes, email);
                }


                if (desconto > 0 && valorTotal > 0)
                {
                    oVendaBilheteriaFormaPagamento.Limpar();

                    oFormaPagamento = new IRLib.FormaPagamento().getFormaPagamentoVenda(FormaPagamento.DESCONTO_ID, estruturaIdentificaoUsuario.EmpresaID);
                    oVendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    oVendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    oVendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.Valor.Valor = desconto;
                    oVendaBilheteriaFormaPagamento.Porcentagem.Valor = (desconto * 100) / (valorTotal);
                    oVendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    if (bd.Executar(oVendaBilheteriaFormaPagamento.StringInserir()) != 1)
                        throw new Exception("A forma de pagamento a ser aplicada no desconto não está disponível para a assinatura, por favor entre em contato com a Ingresso Rápido.");
                }

                this.EfetuarAcoes(bd, clienteID, estruturaIdentificaoUsuario.UsuarioID, listaAcoes, oVendaBilheteira.Control.ID);

                if (desconto > 0)
                    this.UtilizouDesconto(bd, clienteID);

                if (oSitef != null && utilizarTef)
                    if (oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");

                return bd.ConsultaValor("SELECT TOP 1 Senha FROM tVendaBilheteria (NOLOCK) WHERE ID = " + oVendaBilheteira.Control.ID).ToString();
            }
            catch (Exception ex)
            {
                if (oSitef != null && utilizarTef && abriuTransacaoTEF)
                    if (oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");

                throw ex;
            }
            finally
            {
                oSitef = null;
                bdAux.Fechar();
            }
        }

        public string Vender(BD bd, int clienteID, int usuarioID, string formaPagamento, int Parcelas,
                            string numeroCartao, string dataValidade, string codigo,
                            EstruturaReservaInternet estruturaIdentificaoUsuario, List<IRLib.Assinaturas.Models.AcaoProvisoria> listaAcoes,
                            decimal desconto, int entregaID, decimal valorEntrega, bool valorEntregaFixo, string email)
        {
            IRLib.Sitef oSitef = new IRLib.Sitef();
            bool utilizarTef = false;
            bool tefAtivo = Convert.ToBoolean(ConfigurationManager.AppSettings["TEFAtivo"]);
            bool abriuTransacaoTEF = false;
            BD bdAux = new BD();

            try
            {
                if (estruturaIdentificaoUsuario.CaixaID == 0)
                    throw new Exception("Não será possível continuar, o caixa não está aberto.");

                EstruturaVendaFormaPagamento oFormaPagamento = new IRLib.FormaPagamento().getFormaPagamentoVendaAssinatura(formaPagamento, Parcelas, estruturaIdentificaoUsuario.EmpresaID, clienteID, usuarioID);

                ClienteEndereco oClienteEndereco = new ClienteEndereco();

                Entrega oEntrega = new Entrega();
                oEntrega.Ler(entregaID);

                EntregaControle oEntegaControle = new EntregaControle();
                int clienteEnderecoID = 0;

                if (oEntrega.Tipo.Valor != Entrega.RETIRADABILHETERIA)
                {
                    oClienteEndereco.BuscarPorCliente(clienteID);
                    clienteEnderecoID = oClienteEndereco.Control.ID;

                    oEntegaControle.BuscarPorEnderecoEntrega(oClienteEndereco.CEP.Valor, entregaID);
                }
                else
                    oEntegaControle.BuscarPorEntregaID(entregaID);

                valorEntrega = valorEntregaFixo ? valorEntrega : oEntegaControle.Valor.Valor;

                utilizarTef = numeroCartao.Length > 0 && tefAtivo;

                string sqlBusca =
                    string.Format(
                        @"SELECT 
						   i.ID as IngressoID, Codigo, i.EventoID, i.ApresentacaoID, i.Status, i.BloqueioID,
						   i.ApresentacaoSetorID, i.SetorID, p.ID AS PrecoID, i.LugarID, p.Valor AS PrecoValor, acp.Acao, ac.Status AS StatusAssinaturaCliente, DesistenciaBloqueioID, ExtintoBloqueioID, i.AssinaturaClienteID
						FROM tAssinaturaAcaoProvisoria acp (NOLOCK) 
						INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = acp.AssinaturaClienteID
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
						LEFT JOIN tPreco p (NOLOCK) ON p.ApresentacaoSetorID = i.ApresentacaoSetorID AND p.PrecoTipoID = acp.PrecoTipoID
						WHERE acp.ClienteID = {0} AND i.ClienteID = {0} AND acp.Processado = 'F' AND acp.Acao <> 'T'
					", clienteID);

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
                    PrecoID = 0,
                    LugarID = 0,
                    PrecoValor = (decimal)0.00,
                    Acao = EnumAcao.AguardandoAcao,
                    StatusAssinaturaCliente = EnumStatus.Aguardando,
                    DesistenciaBloqueioID = 0,
                    ExtintoBloqueioID = 0,
                    AssinaturaClienteID = 0,
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
                        PrecoID = bdAux.LerInt("PrecoID"),
                        LugarID = bdAux.LerInt("LugarID"),
                        PrecoValor = bdAux.LerDecimal("PrecoValor"),
                        Acao = (EnumAcao)Convert.ToChar(bdAux.LerString("Acao")),
                        StatusAssinaturaCliente = (EnumStatus)Convert.ToChar(bdAux.LerString("StatusAssinaturaCliente")),
                        DesistenciaBloqueioID = bdAux.LerInt("DesistenciaBloqueioID"),
                        ExtintoBloqueioID = bdAux.LerInt("ExtintoBloqueioID"),
                        AssinaturaClienteID = bdAux.LerInt("AssinaturaClienteID"),
                    });

                    listaIngressos.Add(ingresso);
                } while (bdAux.Consulta().Read());

                bdAux.FecharConsulta();
                bdAux.Fechar();

                if (listaAcoes.Count != listaIngressos.Select(c => c.AssinaturaClienteID).Distinct().Count())
                    throw new Exception("Não foi possível encontrar todos os ingressos de sua assinatura, por favor, entre em contato com a ingresso rápido.");

                decimal valorTotal = (listaIngressos.Where(c => c.Acao != EnumAcao.Desisistir).Sum(c => c.PrecoValor) + valorEntrega);

                string notaFiscal = string.Empty;
                if (utilizarTef)
                {
                    oSitef.terminal = "I" + (clienteID > 9999999 ? new Random().Next(9999999).ToString("0000000") : clienteID.ToString("0000000"));
                    oSitef.Terminal = IRLib.Sitef.enumTerminal.SiteIR;
                    oSitef.Empresa = IRLib.Sitef.enumEmpresa.IngressoRapido;
                    oSitef.ValorCompra = (valorTotal - desconto).ToString("#.00");
                    oSitef.Parcelas = Parcelas.ToString();
                    oSitef.ClienteID = "0";
                    oSitef.NumeroCartao = numeroCartao;
                    oSitef.DataVencimento = dataValidade.ToString();
                    oSitef.CodigoSeguranca = codigo;
                    oSitef.TipoFinanciamento = IRLib.Sitef.enumTipoFinanciamento.Estabelecimento;
                    oSitef.IniciaSitef();

                    abriuTransacaoTEF = true;
                }

                VendaBilheteria oVendaBilheteira = new VendaBilheteria();
                oVendaBilheteira.ClienteID.Valor = clienteID;
                oVendaBilheteira.CaixaID.Valor = estruturaIdentificaoUsuario.CaixaID;
                oVendaBilheteira.DataVenda.Valor = DateTime.Now;
                oVendaBilheteira.ClienteEnderecoID.Valor = clienteEnderecoID;
                oVendaBilheteira.EntregaControleID.Valor = oEntegaControle.Control.ID;
                oVendaBilheteira.PdvID.Valor = 0;
                oVendaBilheteira.TaxaEntregaValor.Valor = valorEntrega;
                oVendaBilheteira.PagamentoProcessado.Valor = oFormaPagamento.Tipo != (int)IRLib.FormaPagamento.TIPO.Boleto;
                oVendaBilheteira.ValorTotal.Valor = valorTotal;
                oVendaBilheteira.TaxaEntregaValor.Valor = valorEntrega;
                oVendaBilheteira.TaxaConvenienciaValorTotal.Valor = 0;
                oVendaBilheteira.TaxaConvenienciaValorTotal.Valor = 0;
                oVendaBilheteira.Status.Valor = VendaBilheteria.PAGO;
                oVendaBilheteira.IP.Valor = string.Empty;
                oVendaBilheteira.NomeCartao.Valor = string.Empty;
                oVendaBilheteira.QuantidadeImpressoesInternet.Valor = 0;

                //Se tem tef, precisa colocar a nota fiscal
                oVendaBilheteira.NotaFiscalCliente.Valor = oVendaBilheteira.NotaFiscalEstabelecimento.Valor = oSitef.CupomFiscal;

                string sqlVendaBilheteria = oVendaBilheteira.StringInserir();
                object vendaID = bd.ConsultaValor(sqlVendaBilheteria);
                oVendaBilheteira.Control.ID = (vendaID != null) ? Convert.ToInt32(vendaID) : 0;

                if (oVendaBilheteira.Control.ID == 0)
                    throw new Exception("Não foi possível gerar o seu registro de venda, por favor tente novamente.");

                VendaBilheteriaItem oVendaBilheteriaItem = new VendaBilheteriaItem();
                IngressoLog oIngressoLog = new IngressoLog();


                //Desistencias
                foreach (var ingressosDesistencia in listaIngressos.Where(c => c.Acao == EnumAcao.Desisistir)
                    .Select(c => new { StatusAssinaturaCliente = c.StatusAssinaturaCliente, IngressoID = c.IngressoID, DesistenciaBloqueioID = c.DesistenciaBloqueioID, ExtintoBloqueioID = c.ExtintoBloqueioID, }).Distinct())
                    if (bd.Executar(this.StringDesistirIngresso(ingressosDesistencia.StatusAssinaturaCliente, ingressosDesistencia.IngressoID, ingressosDesistencia.DesistenciaBloqueioID, ingressosDesistencia.ExtintoBloqueioID, clienteID)) != 1)
                        throw new Exception("Não foi possível gerar o registro de desistência do seu ingresso, por favor tente novamente.");

                //Adiquirencia
                foreach (var ingresso in listaIngressos.Where(c => c.Acao != EnumAcao.Desisistir && c.Acao != EnumAcao.Trocar))
                {
                    oVendaBilheteriaItem.Limpar();
                    oVendaBilheteriaItem.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    oVendaBilheteriaItem.PacoteID.Valor = 0;
                    oVendaBilheteriaItem.Acao.Valor = VendaBilheteriaItem.VENDA;
                    oVendaBilheteriaItem.TaxaConveniencia.Valor = 0;
                    oVendaBilheteriaItem.TaxaConvenienciaValor.Valor = 0;
                    oVendaBilheteriaItem.TaxaComissao.Valor = 0;
                    oVendaBilheteriaItem.ComissaoValor.Valor = 0;

                    string sqlVendaBilheteriaItem = oVendaBilheteriaItem.StringInserir();
                    object id = bd.ConsultaValor(sqlVendaBilheteriaItem);
                    oVendaBilheteriaItem.Control.ID = (id != null) ? Convert.ToInt32(id) : 0;

                    if (oVendaBilheteriaItem.Control.ID == 0)
                        throw new Exception("Não foi possível processar a venda. Ocorreu um erro ao incluir os itens.");

                    StringBuilder stb = new StringBuilder();
                    stb.Append("UPDATE tIngresso SET LojaID=" + estruturaIdentificaoUsuario.LojaID + ", PrecoID =" + ingresso.PrecoID + ", UsuarioID = " + estruturaIdentificaoUsuario.UsuarioID + ", ");
                    stb.Append("VendaBilheteriaID=" + oVendaBilheteira.Control.ID + ", Status='" + Ingresso.VENDIDO + "', PacoteID = 0, PacoteGrupo = '', CortesiaID = 0, TimeStampReserva ='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ");
                    stb.Append("WHERE (Status='" + Ingresso.DISPONIVEL + "' OR Status='" + Ingresso.BLOQUEADO + "' OR Status='" + Ingresso.RESERVADO + "') AND ClienteID=" + clienteID + " AND ID=" + ingresso.IngressoID);
                    int x = bd.Executar(stb.ToString());
                    if (x == 0)
                        throw new Exception("Não foi possível atualizar os registros do seu ingresso.");

                    oIngressoLog.Limpar();
                    oIngressoLog.TimeStamp.Valor = System.DateTime.Now;
                    oIngressoLog.IngressoID.Valor = ingresso.IngressoID;
                    oIngressoLog.UsuarioID.Valor = estruturaIdentificaoUsuario.UsuarioID; //usuario fixo para Internet
                    oIngressoLog.BloqueioID.Valor = ingresso.BloqueioID;
                    oIngressoLog.CortesiaID.Valor = 0;
                    oIngressoLog.PrecoID.Valor = ingresso.PrecoID;
                    oIngressoLog.VendaBilheteriaItemID.Valor = oVendaBilheteriaItem.Control.ID;
                    oIngressoLog.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    oIngressoLog.CaixaID.Valor = estruturaIdentificaoUsuario.CaixaID; // ABERTURA E FECHAMENTO DE CAIXA DIARIO PARA INTERNET
                    oIngressoLog.LojaID.Valor = estruturaIdentificaoUsuario.LojaID; //loja fixa Internet
                    oIngressoLog.CanalID.Valor = estruturaIdentificaoUsuario.CanalID; //canal fixo Internet
                    oIngressoLog.EmpresaID.Valor = estruturaIdentificaoUsuario.EmpresaID; // FIXO IR
                    oIngressoLog.ClienteID.Valor = clienteID;
                    oIngressoLog.Acao.Valor = IngressoLog.VENDER;
                    oIngressoLog.GerenciamentoIngressosID.Valor = 0;
                    oIngressoLog.AssinaturaClienteID.Valor = ingresso.AssinaturaClienteID;

                    string sqlIngressoLogV = oIngressoLog.StringInserir();
                    x = bd.Executar(sqlIngressoLogV);
                    bool okV = (x == 1);
                    if (!okV)
                        throw new Exception("Log de venda do ingresso não foi inserido.");
                }

                VendaBilheteriaFormaPagamento oVendaBilheteriaFormaPagamento = new VendaBilheteriaFormaPagamento();

                if (valorTotal > 0)
                {
                    oVendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    oVendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    oVendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;//Precisa Achar a FormaPagamentoID
                    oVendaBilheteriaFormaPagamento.Valor.Valor = valorTotal - desconto;
                    oVendaBilheteriaFormaPagamento.Porcentagem.Valor = ((valorTotal - desconto) * 100) / (valorTotal);
                    oVendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;

                    if (utilizarTef)
                    {
                        oVendaBilheteriaFormaPagamento.VendaBilheteriaFormaPagamentoTEFID.Valor = oSitef.ID;
                        oVendaBilheteriaFormaPagamento.CodigoRespostaVenda.Valor = oSitef.CodigoRespostaSitefVenda;
                        oVendaBilheteriaFormaPagamento.MensagemRetorno.Valor = oSitef.MensagemFinaliza;
                        oVendaBilheteriaFormaPagamento.HoraTransacao.Valor = oSitef.HoraTransacao;
                        oVendaBilheteriaFormaPagamento.DataTransacao.Valor = oSitef.DataTransacao;
                        oVendaBilheteriaFormaPagamento.CodigoIR.Valor = oSitef.CodigoIR;
                        oVendaBilheteriaFormaPagamento.NumeroAutorizacao.Valor = oSitef.NumeroAutorizacao;
                        oVendaBilheteriaFormaPagamento.NSUHost.Valor = oSitef.NSUHost;
                        oVendaBilheteriaFormaPagamento.NSUSitef.Valor = oSitef.NSUSitef;
                        oVendaBilheteriaFormaPagamento.Cupom.Valor = oSitef.CupomFiscal;
                        oVendaBilheteriaFormaPagamento.DadosConfirmacaoVenda.Valor = oSitef.DadosConfirmacao;
                        oVendaBilheteriaFormaPagamento.Rede.Valor = oSitef.RedeRetorno;
                        oVendaBilheteriaFormaPagamento.CodigoRespostaTransacao.Valor = oSitef.CodigoRespostaSitefFinaliza;
                    }

                    int vendaBilheteriaFormaPagamentoID = Convert.ToInt32(bd.ConsultaValor(oVendaBilheteriaFormaPagamento.StringInserir()));

                    VendaBilheteriaFormaPagamentoBoleto oFormaPagamentoBoleto = new VendaBilheteriaFormaPagamentoBoleto();

                    if (oFormaPagamento.Tipo == (int)IRLib.FormaPagamento.TIPO.Boleto)
                        oFormaPagamentoBoleto.EnviarBoletos(bd, Parcelas, valorTotal - desconto, vendaBilheteriaFormaPagamentoID, listaAcoes, email);
                }

                if (desconto > 0 && valorTotal > 0)
                {
                    oVendaBilheteriaFormaPagamento.Limpar();

                    oFormaPagamento = new IRLib.FormaPagamento().getFormaPagamentoVenda(FormaPagamento.DESCONTO_ID, estruturaIdentificaoUsuario.EmpresaID);
                    oVendaBilheteriaFormaPagamento.Dias.Valor = oFormaPagamento.Dias;
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.IR.Valor = oFormaPagamento.IR;
                    oVendaBilheteriaFormaPagamento.TaxaAdm.Valor = oFormaPagamento.TaxaAdm;
                    oVendaBilheteriaFormaPagamento.DataDeposito.Valor = System.DateTime.Now.AddDays(oFormaPagamento.Dias);
                    oVendaBilheteriaFormaPagamento.FormaPagamentoID.Valor = oFormaPagamento.FormaPagamentoID;
                    oVendaBilheteriaFormaPagamento.Valor.Valor = desconto;
                    oVendaBilheteriaFormaPagamento.Porcentagem.Valor = (desconto * 100) / (valorTotal);
                    oVendaBilheteriaFormaPagamento.VendaBilheteriaID.Valor = oVendaBilheteira.Control.ID;
                    if (bd.Executar(oVendaBilheteriaFormaPagamento.StringInserir()) != 1)
                        throw new Exception("A forma de pagamento a ser aplicada no desconto não está disponível para a assinatura, por favor entre em contato com a Ingresso Rápido.");
                }

                this.EfetuarAcoes(bd, clienteID, estruturaIdentificaoUsuario.UsuarioID, listaAcoes, oVendaBilheteira.Control.ID);

                if (desconto > 0)
                    this.UtilizouDesconto(bd, clienteID);

                if (oSitef != null && utilizarTef)
                    if (oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Confirmar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");

                return bd.ConsultaValor("SELECT TOP 1 Senha FROM tVendaBilheteria (NOLOCK) WHERE ID = " + oVendaBilheteira.Control.ID).ToString();
            }
            catch (Exception ex)
            {
                if (oSitef != null && utilizarTef && abriuTransacaoTEF)
                    if (oSitef.FinalizaTransacao(Sitef.enumTipoConfirmacao.Cancelar) != Sitef.enumRetornoSitef.Ok)
                        throw new Exception("Não foi possível finalizar a transação, por favor tente novamente.");

                throw ex;
            }
            finally
            {
                oSitef = null;
                bdAux.Fechar();
            }
        }

        public void FinalizarComDesistencia(int clienteID, int usuarioID, List<Assinaturas.Models.AcaoProvisoria> lista)
        {
            try
            {


                string sqlBusca =
                   string.Format(
                       @"SELECT 
						   i.ID as IngressoID, ac.Status, DesistenciaBloqueioID, ExtintoBloqueioID                      
						FROM tAssinaturaAcaoProvisoria acp (NOLOCK) 
						INNER JOIN tAssinaturaCliente ac (NOLOCK) ON ac.ID = acp.AssinaturaClienteID
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
						WHERE acp.ClienteID = {0} AND i.ClienteID = {0} AND acp.Acao = '{1}'
					", clienteID, (char)EnumAcao.Desisistir);

                if (!bd.Consulta(sqlBusca).Read())
                    throw new Exception("Não existem ingressos para desistência, por favor tente novamente.");

                var ingressoAnonimo = new
                {
                    ID = 0,
                    Status = EnumStatus.Aguardando,
                    DesistenciaBloqueioID = 0,
                    ExtintoBloqueioID = 0
                };

                var listaIngressos = VendaBilheteria.ToAnonymousList(ingressoAnonimo);

                do
                {
                    listaIngressos.Add(new
                    {
                        ID = bd.LerInt("IngressoID"),
                        Status = (EnumStatus)Convert.ToChar(bd.LerString("Status")),
                        DesistenciaBloqueioID = bd.LerInt("DesistenciaBloqueioID"),
                        ExtintoBloqueioID = bd.LerInt("ExtintoBloqueioID"),
                    });
                } while (bd.Consulta().Read());

                bd.FecharConsulta();
                bd.IniciarTransacao();

                new AssinaturaAcaoProvisoria().ExcluirAcoes(bd, clienteID);
                //Desistencias
                foreach (var ingressosDesistencia in listaIngressos
                    .Select(c => new { StatusAssinaturaCliente = c.Status, IngressoID = c.ID, DesistenciaBloqueioID = c.DesistenciaBloqueioID, ExtintoBloqueioID = c.ExtintoBloqueioID, }).Distinct())
                    if (bd.Executar(this.StringDesistirIngresso(ingressosDesistencia.StatusAssinaturaCliente, ingressosDesistencia.IngressoID, ingressosDesistencia.DesistenciaBloqueioID, ingressosDesistencia.ExtintoBloqueioID, clienteID)) != 1)
                        throw new Exception("Não foi possível gerar o registro de desistência do seu ingresso, por favor tente novamente.");

                this.EfetuarAcoes(bd, clienteID, usuarioID, lista, 0);

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

        public void Reservar(int clienteID, int assinaturaID, int setorID, int lugarID, int ano, EstruturaReservaInternet estruturaIdentificacaoUsuario, int relacionadoAssinaturaClienteID)
        {
            try
            {
                bd.IniciarTransacao();

                this.Reservar(bd, clienteID, assinaturaID, setorID, lugarID, ano, estruturaIdentificacaoUsuario, relacionadoAssinaturaClienteID);

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

        public void Reservar(BD bd, int clienteID, int assinaturaID, int setorID, int lugarID, int ano, EstruturaReservaInternet estruturaIdentificacaoUsuario, int relacionadoAssinaturaClienteID)
        {
            BD bdAux = new BD();
            int assinaturaAnoID = 0;
            try
            {
                assinaturaAnoID = Convert.ToInt32(
                       bd.ConsultaValor(
                           string.Format(@"SELECT 
									an.ID 
								FROM tAssinatura a (NOLOCK) 
								INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID 
								WHERE a.Ativo = 'T' AND a.ID = {0} AND an.Ano = '{1}'", assinaturaID, ano)));
            }
            finally
            {
                bdAux.Fechar();
            }

            if (assinaturaAnoID == 0)
                throw new Exception("Não foi possível identificar o ano da assinatura selecionada, por favor tente novamente");

            this.ClienteID.Valor = clienteID;
            this.AssinaturaID.Valor = assinaturaID;
            this.PrecoTipoID.Valor = 0;
            this.LugarID.Valor = lugarID;
            this.AssinaturaAnoID.Valor = assinaturaAnoID;
            this.Status.Valor = ((char)EnumStatus.Aguardando).ToString();
            this.Acao.Valor = (relacionadoAssinaturaClienteID == 0 ? (char)EnumAcao.Aquisicao : (char)EnumAcao.EfetivarTroca).ToString();
            this.SetorID.Valor = setorID;
            this.AssinaturaClienteID.Valor = relacionadoAssinaturaClienteID;
            this.Inserir(bd);

            if (this.Control.ID == 0)
                throw new Exception("Não foi possível gerar o registro desta reserva. Por favor tente novamente.");

            new Bilheteria().ReservarAssinatura(bd,
                clienteID, assinaturaID, setorID, lugarID, assinaturaAnoID, estruturaIdentificacaoUsuario, this.Control.ID);

        }

        public void ReservarQuantidade(int clienteID, int assinaturaID, int setorID, int quantidade, int ano, EstruturaReservaInternet estruturaIdentificacaoUsuario, int relacionadoAssinaturaClienteID)
        {
            try
            {
                string sqlLugares =
                    string.Format(@"
						SELECT TOP {0}
								i.LugarID
							FROM tAssinatura a (NOLOCK) 
							INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID 
							INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
							INNER JOIN tIngresso i (NOLOCK) ON i.ApresentacaoID = ai.ApresentacaoID AND i.SetorID = ai.SetorID
							WHERE a.ID = {1} AND an.Ano = '{2}' AND ai.SetorID = {3} AND i.Status = '{4}'
							ORDER BY NEWID()	
					", quantidade, assinaturaID, ano, setorID, Ingresso.DISPONIVEL);

                bd.Consulta(sqlLugares);

                if (!bd.Consulta().Read())
                    throw new Exception("Este setor está esgotado, por favor tente outro setor.");

                List<int> lugares = new List<int>();

                do { lugares.Add(bd.LerInt("LugarID")); } while (bd.Consulta().Read());

                bd.FecharConsulta();

                if (lugares.Count != quantidade)
                    throw new Exception("Não foi possível encontrar a quantidade de ingressos necessária para efetuar a reserva.");

                bd.IniciarTransacao();

                foreach (var lugarID in lugares)
                    this.Reservar(bd, clienteID, assinaturaID, setorID, lugarID, ano, estruturaIdentificacaoUsuario, relacionadoAssinaturaClienteID);

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

        public void RemoverReservaAssinatura(int assinaturaClienteID, int clienteID)
        {
            try
            {
                string sql =
                    string.Format(
                    @"
						SELECT
							i.ID, i.Status
							FROM tAssinaturaCliente ac (NOLOCK) 
							INNER JOIN tAssinatura a (NOLOCK) ON ac.AssinaturaID = a.ID
							INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
							WHERE ac.ID = {0} AND ac.ClienteID = {1} AND i.ClienteID = {1}
	
					",
                    assinaturaClienteID, clienteID);
                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new IngressoException("Este lugar não pertence a você, por favor verifique sua reserva.");

                List<int> ingressos = new List<int>();

                do
                {
                    ingressos.Add(bd.LerInt("ID"));
                } while (bd.Consulta().Read());

                bd.FecharConsulta();

                bd.IniciarTransacao();

                foreach (var ingressoID in ingressos)
                    if (bd.Executar(Ingresso.StringRemoverReservaAssinatura(ingressoID)) != 1)
                        throw new Exception("Não foi possível remover sua reserva.");

                bd.Executar("DELETE FROM tAssinaturaCliente WHERE ID = " + assinaturaClienteID);
                bd.Executar("DELETE FROM tAssinaturaHistorico WHERE AssinaturaClienteID = " + assinaturaClienteID);

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

        public IRLib.Assinaturas.Models.Assinatura BuscaAssinatura(int clienteID, int assinaturaClienteID)
        {
            try
            {
                string sql =
                    string.Format(@"
						SELECT 
							a.Nome AS Assinatura, a.ID as AssinaturaID, s.Nome AS Setor, l.Codigo AS Lugar
						FROM tAssinaturaCliente ac (NOLOCK)
						INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
						INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
						INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
						WHERE ac.ID = {0} AND ac.ClienteID = {1}
					", assinaturaClienteID, clienteID);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Tentativa de buscar uma assinatura que não está associada a você. Por favor tente novamente.");

                return new IRLib.Assinaturas.Models.Assinatura()
                {
                    Nome = bd.LerString("Assinatura"),
                    Setor = bd.LerString("Setor"),
                    Lugar = bd.LerString("Lugar"),
                    AssinaturaID = bd.LerInt("AssinaturaID")
                };
            }
            finally
            {
                bd.Fechar();
            }
        }

        private string StringDesistirIngresso(EnumStatus status, int ingressoID, int desistenciaBloqueioID, int extintoBloqueioID, int clienteID)
        {
            StringBuilder stbDesistencia = new StringBuilder();
            if (status == EnumStatus.Indisponivel)
            {
                stbDesistencia.AppendFormat("UPDATE tIngresso SET BloqueioID = {0}, Status='{1}', PrecoID=0, UsuarioID=0,  LojaID = 0, ClienteID = 0, TimeStampReserva='', SessionID='', AssinaturaClienteID = 0 ", extintoBloqueioID, Ingresso.BLOQUEADO);
                stbDesistencia.AppendFormat("WHERE ID = {0} AND Status = '{1}' AND ClienteID = {2}", ingressoID, Ingresso.BLOQUEADO, clienteID);

                return stbDesistencia.ToString();
            }
            else if (desistenciaBloqueioID > 0)
            {
                stbDesistencia.AppendFormat("UPDATE tIngresso SET Status = '{0}', BloqueioID = {1} ", Ingresso.BLOQUEADO, desistenciaBloqueioID);
                stbDesistencia.AppendFormat("WHERE ID = {0} AND Status IN ('{1}', '{2}') AND ClienteID = {3}", ingressoID, Ingresso.BLOQUEADO, Ingresso.RESERVADO, clienteID);

                return stbDesistencia.ToString();
            }
            else
                return Ingresso.StringRemoverReservaAssinatura(ingressoID);
        }

        public decimal ValorDesconto(int clienteID)
        {
            try
            {
                return Convert.ToDecimal(
                    bd.ConsultaValor("SELECT IsNull(Valor, 0) FROM tAssinaturaDesconto ac WHERE Utilizado = 'F' AND ClienteID = " + clienteID));
            }
            finally
            {
                bd.Fechar();
            }
        }

        private void UtilizouDesconto(BD bd, int clienteID)
        {
            if (bd.Executar("UPDATE tAssinaturaDesconto SET Utilizado = 'T' WHERE ClienteID = " + clienteID + " AND Utilizado = 'F'") != 1)
                throw new Exception("Tentativa de utilizar o desconto multiplas vezes.");
        }

        public List<IRLib.Assinaturas.Models.Cliente> BuscarClientesPaginado(
            int assinaturaTipoID, int ano, int assinaturaID, int setorID, string statusLugar, string statusBoleto, string vencMin, string vencMax,
            string login, string assinante, string CPF, int qtdPorPagina, ref List<int> paginas, int pagina)
        {
            try
            {
                List<IRLib.Assinaturas.Models.Cliente> lista = new List<Assinaturas.Models.Cliente>();


                string filtro = this.MontarFiltroClientes(
                    ano, assinaturaID, setorID, statusLugar, statusBoleto, vencMin, vencMax, login, assinante, CPF);

                string sql =
                    string.Format(
                    @"
						WITH tbGeral AS (
							SELECT
								DISTINCT c.ID, c.LoginOSESP, c.Nome, c.CPF
							FROM tAssinaturaCliente ac (NOLOCK)
							INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = ac.AssinaturaID
							INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
							INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID AND ai.SetorID = ac.SetorID
							INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
							INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
							LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = ac.VendaBilheteriaID
							LEFT JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID
							LEFT JOIN tVendaBilheteriaFormaPagamentoBoleto vbfpb (NOLOCK) ON vbfpb.VendaBilheteriaFormaPagamentoID = vbfp.ID
							WHERE ass.AssinaturaTipoID = {0} {1}
							),
	
						tbCount AS (
							SELECT COUNT(ID) AS Registros FROM tbGeral),
	
						tbOrdenada AS (
							SELECT ID, LoginOSESP, Nome, CPF,ROW_NUMBER() OVER (ORDER BY Nome) AS 'RowNumber'
								FROM tbGeral)
		
						SELECT 
							ID, LoginOSESP, Nome, CPF, RowNumber, Registros FROM tbOrdenada, tbCount 
						WHERE RowNumber > {2} AND RowNumber <= {3}
						ORDER BY Nome
					", assinaturaTipoID, filtro, (pagina - 1) * qtdPorPagina, pagina * qtdPorPagina);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível encontrar nenhum cliente com o filtro selecionado.");

                int totalPaginas = bd.LerInt("Registros");
                do
                {
                    lista.Add(new Assinaturas.Models.Cliente()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Login = bd.LerString("LoginOSESP"),
                        CPF = bd.LerString("CPF"),
                    });
                } while (bd.Consulta().Read());

                totalPaginas = Convert.ToInt32(totalPaginas / qtdPorPagina);

                if (totalPaginas < qtdPorPagina || totalPaginas % qtdPorPagina == 1)
                    totalPaginas++;

                int paginaInicial = Math.Max(1, pagina - 10);
                int paginaFinal = Math.Min(pagina + 10, totalPaginas);

                for (int i = paginaInicial; i <= paginaFinal; i++)
                    paginas.Add(i);

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<IRLib.Assinaturas.Models.Cliente> BuscarClientes(
             int assinaturaTipoID, int ano, int assinaturaID, int setorID, string statusLugar, string statusBoleto, string vencMin, string vencMax,
            string login, string assinante, string CPF)
        {
            try
            {
                List<IRLib.Assinaturas.Models.Cliente> lista = new List<Assinaturas.Models.Cliente>();

                string filtro = this.MontarFiltroClientes(
                    ano, assinaturaID, setorID, statusLugar, statusBoleto, vencMin, vencMax, login, assinante, CPF);

                string sql = string.Format(
                    @"
                            SELECT
								    DISTINCT c.ID, c.LoginOSESP, c.Nome, c.CPF
							    FROM tAssinaturaCliente ac (NOLOCK)
							    INNER JOIN tAssinatura ass (NOLOCK) ON ass.ID = ac.AssinaturaID
							    INNER JOIN tAssinaturaAno an (NOLOCK) ON an.ID = ac.AssinaturaAnoID
							    INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID AND ai.SetorID = ac.SetorID
							    INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID
							    INNER JOIN tIngresso i (NOLOCK) ON i.AssinaturaClienteID = ac.ID
							    LEFT JOIN tVendaBilheteria vb (NOLOCK) ON vb.ID = ac.VendaBilheteriaID
							    LEFT JOIN tVendaBilheteriaFormaPagamento vbfp (NOLOCK) ON vbfp.VendaBilheteriaID = vb.ID
							    LEFT JOIN tVendaBilheteriaFormaPagamentoBoleto vbfpb (NOLOCK) ON vbfpb.VendaBilheteriaFormaPagamentoID = vbfp.ID
							    WHERE ass.AssinaturaTipoID = {0} {1}
                    ", assinaturaTipoID, filtro);
                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não foi possível encontrar nenhum cliente a partir do filtro selecionado.");

                do
                {
                    lista.Add(new Assinaturas.Models.Cliente()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        Login = bd.LerString("LoginOSESP"),
                        CPF = bd.LerString("CPF"),
                    });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        private string MontarFiltroClientes(
            int ano, int assinaturaID, int setorID, string statusLugar, string statusBoleto, string vencMin, string vencMax,
            string login, string assinante, string CPF)
        {
            StringBuilder stb = new StringBuilder();

            if (ano > 0)
                stb.AppendFormat(" AND an.Ano = '{0}'", ano);

            if (assinaturaID > 0)
                stb.Append(" AND ac.AssinaturaID = " + assinaturaID);

            if (setorID > 0)
                stb.Append(" AND ai.SetorID = " + setorID);

            if (!string.IsNullOrEmpty(statusLugar) && statusLugar != "S")
                stb.Append(Assinatura.DesmembrarStatus(IRLib.Utils.Enums.ParseItem<IRLib.Assinatura.EnumStatusVisual>(statusLugar)));

            if (!string.IsNullOrEmpty(statusBoleto) && statusBoleto != "S")
            {
                switch ((VendaBilheteriaFormaPagamentoBoleto.EnumStatusBoleto)Convert.ToChar(statusBoleto))
                {
                    case VendaBilheteriaFormaPagamentoBoleto.EnumStatusBoleto.Pago:
                        stb.Append(" AND vbfpb.ValorPago >= vbfp.Valor");
                        break;
                    case VendaBilheteriaFormaPagamentoBoleto.EnumStatusBoleto.AguardandoPagamento:
                        stb.Append(" AND vbfpb.ValorPago = 0 ");
                        break;
                    case VendaBilheteriaFormaPagamentoBoleto.EnumStatusBoleto.Ambos:
                        stb.Append(" AND (vbfpb.ValorPago = 0 OR vbfpb.ValorPago >= vbfp.Valor) ");
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(vencMin))
                stb.AppendFormat(" AND vbfpb.DataVencimento >=  '{0}'", Convert.ToDateTime(vencMin).ToString("yyyyMMdd"));

            if (!string.IsNullOrEmpty(vencMax))
                stb.AppendFormat(" AND vbfpb.DataVencimento <=  '{0}'", Convert.ToDateTime(vencMax).ToString("yyyyMMdd"));

            if (!string.IsNullOrEmpty(login))
                stb.AppendFormat(" AND c.Login LIKE '{0}%", login);

            if (!string.IsNullOrEmpty(assinante))
                stb.AppendFormat(" AND c.Nome LIKE '%{0}%'", assinante);

            if (!string.IsNullOrEmpty(CPF))
                stb.AppendFormat(" AND (c.CPF = '{0}' OR c.CNPJ = '{0}')", CPF);

            return stb.ToString();
        }

        public decimal DescontosUtlizados()
        {
            try
            {
                return Convert.ToDecimal(bd.ConsultaValor("SELECT SUM(Valor) FROM tAssinaturaDesconto WHERE Utilizado = 'T'"));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public Assinatura InfoCancelamento(int assinaturaClienteID)
        {
            bd.Consulta(@"SELECT 
	                        tAssinatura.TipoCancelamento, tAssinatura.DesistenciaBloqueioID
	                        FROM tAssinaturaCliente (NOLOCK)
	                        INNER JOIN tAssinatura (NOLOCK)ON tAssinatura.ID = tAssinaturaCliente.AssinaturaID
	                        WHERE tAssinaturaCliente.ID = " + assinaturaClienteID);

            if (bd.Consulta().Read())
            {
                var ret = new Assinatura();
                ret.DesistenciaBloqueioID.Valor = bd.LerInt("DesistenciaBloqueioID");
                ret.TipoCancelamento.Valor = bd.LerString("TipoCancelamento");

                return ret;
            }

            throw new ApplicationException("Não foi possível encontrar a assinatura!");
        }

        public int BuscaPossivelTroca(int ClienteID, int AssinaturaTipoID)
        {
            try
            {
                string sql = @"SELECT top 1 ac.ID as AssinaturaClienteIDNova  
                            FROM  tAssinaturaCliente ac
                            INNER JOIN tAssinatura a ON ac.AssinaturaID = a.ID
                            WHERE a.AssinaturaTipoID = " + AssinaturaTipoID + " AND ac.ClienteID = " + ClienteID + @" and ac.Status = 'Z' AND ac.Acao = 'T' 
                            AND ac.AssinaturaClienteID = 0 AND 
                            ac.ID NOT IN (select AssinaturaClienteID from tAssinaturaCliente where ClienteID = " + ClienteID + @" and Status = 'A' AND Acao = 'E')";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return bd.LerInt("AssinaturaClienteIDNova");
                else
                    return 0;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int VerificaSenha(string Senha, int ClienteID)
        {
            try
            {

                string sql = @"SELECT top 1 vb.ID   
                            FROM  tVendaBilheteria vb
                            WHERE vb.Senha = '" + Senha + "' AND vb.ClienteID = " + ClienteID;

                return Convert.ToInt32(bd.ConsultaValor(sql));
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<AcaoProvisoria> ComprovanteAssinatura(int AssinaturaClienteID)
        {
            try
            {
                List<AcaoProvisoria> lista = new List<AcaoProvisoria>();

                string sql = string.Format(@"SELECT 
                                tAssinatura.Nome AS Assinatura,
                                tLugar.Codigo AS Lugar,
                                tSetor.Nome AS Setor,
                                dbo.DaraHoraFormatada(tAssinaturaCLiente.TimeStamp) AS Data,
                                tAssinaturaCLiente.Acao ,
                                ISNULL(tVendaBilheteria.ValorTotal, 0.00) AS Valor,
	                            ISNULL(tEntrega.Nome,'') AS Entrega,
                                ISNULL(tVendaBIlheteria.Senha,'') AS Senha
                            FROM tAssinaturaCLiente 
	                            INNER JOIN tAssinatura ON tAssinaturaCLiente.AssinaturaID = tAssinatura.ID 
	                            INNER JOIN tLugar ON tAssinaturaCLiente.LugarID = tLugar.ID 
	                            INNER JOIN tSetor ON tAssinaturaCLiente.SetorID = tSetor.ID 
	                            LEFT JOIN tVendaBilheteria ON tAssinaturaCLiente.VendaBilheteriaID = tVendaBilheteria.ID
	                            LEFT JOIN tAssinaturaAcaoProvisoria ON tAssinaturaCLiente.ID = tAssinaturaAcaoProvisoria.AssinaturaCLienteID
	                            LEFT JOIN tEntrega ON tAssinaturaAcaoProvisoria.EntregaID = tEntrega.ID
                            WHERE tAssinaturaCLiente.ID = {0}", AssinaturaClienteID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    AcaoProvisoria oAcaoProvisoria = new AcaoProvisoria();

                    oAcaoProvisoria.Valor = bd.LerDecimal("Valor");
                    oAcaoProvisoria.Lugar = bd.LerString("Lugar");
                    oAcaoProvisoria.Setor = bd.LerString("Setor");
                    oAcaoProvisoria.Assinatura = bd.LerString("Assinatura");
                    oAcaoProvisoria.Acao = Enums.ParseItem<AssinaturaCliente.EnumAcao>(bd.LerString("Acao"));
                    oAcaoProvisoria.Data = bd.LerString("Data");
                    oAcaoProvisoria.Entrega = bd.LerString("Entrega");
                    oAcaoProvisoria.Senha = bd.LerString("Senha");

                    lista.Add(oAcaoProvisoria);
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaClienteLista : AssinaturaClienteLista_B
    {
        public AssinaturaClienteLista() { }
    }
}
