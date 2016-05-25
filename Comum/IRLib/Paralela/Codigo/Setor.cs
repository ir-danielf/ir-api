/**************************************************
* Arquivo: Setor.cs
* Gerado: 02/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Setor : Setor_B
    {

        public const string Pista = "P";
        public const string MesaAberta = "A";
        public const string MesaFechada = "M";
        public const string Cadeira = "C";

        public enum enumLugarMarcado
        {
            [System.ComponentModel.Description("Setor tipo Pista")]
            Pista = 'P',
            [System.ComponentModel.Description("Setor tipo Mesa Aberta")]
            MesaAberta = 'A',
            [System.ComponentModel.Description("Setor tipo Mesa Fechada")]
            MesaFechada = 'M',
            [System.ComponentModel.Description("Setor tipo Cadeira")]
            Cadeira = 'C'
        }

        public Setor() { }

        public Setor(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public override bool Inserir()
        {
            if (this.LugarMarcado.Valor != Setor.Pista && this.LugarMarcado.Valor != Setor.MesaAberta && this.LugarMarcado.Valor != Setor.MesaFechada && this.LugarMarcado.Valor != Setor.Cadeira)
                throw new SetorException("O Lugar Marcado não está dentro dos tipos permitidos.");

            return base.Inserir();
        }

        public override bool Atualizar()
        {
            if (this.LugarMarcado.Valor != Setor.Pista && this.LugarMarcado.Valor != Setor.MesaAberta && this.LugarMarcado.Valor != Setor.MesaFechada && this.LugarMarcado.Valor != Setor.Cadeira)
                throw new SetorException("O Lugar Marcado não está dentro dos tipos permitidos.");

            return base.Atualizar();
        }

        public DataTable Tipos()
        {

            try
            {

                DataTable tabela = new DataTable("LugarMarcado");

                tabela.Columns.Add("Tipo", typeof(string));
                tabela.Columns.Add("Nome", typeof(string));

                DataRow linha = tabela.NewRow();
                linha["Tipo"] = Cadeira;
                linha["Nome"] = "Cadeira";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = MesaAberta;
                linha["Nome"] = "Mesa Aberta";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = MesaFechada;
                linha["Nome"] = "Mesa Fechada";
                tabela.Rows.Add(linha);

                linha = tabela.NewRow();
                linha["Tipo"] = Pista;
                linha["Nome"] = "Pista";
                tabela.Rows.Add(linha);

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int GetLocalID(int setorID)
        {
            try
            {
                return Convert.ToInt32(bd.ConsultaValor("SELECT LocalID FROM tSetor WHERE ID = " + setorID));
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Exclui Setor
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = true;

                ApresentacaoSetorLista apresentacaoSetorLista = new ApresentacaoSetorLista();
                apresentacaoSetorLista.FiltroSQL = "SetorID=" + this.Control.ID;
                apresentacaoSetorLista.Carregar();

                if (apresentacaoSetorLista.Tamanho > 0)
                {
                    IngressoLista ingressoLista = new IngressoLista();
                    ingressoLista.FiltroSQL = "ApresentacaoSetorID in (" + apresentacaoSetorLista + ")";
                    ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                    ingressoLista.Carregar(1);
                    if (ingressoLista.Tamanho > 0)
                        throw new LugarException("Não pode excluir o setor porque há ingressos não-disponíveis.");

                    LugarLista lugarLista = new LugarLista();
                    lugarLista.FiltroSQL = "SetorID=" + this.Control.ID;
                    lugarLista.Carregar();

                    ok = lugarLista.ExcluirTudo();

                    if (ok)
                    {
                        ingressoLista.FiltroSQL = null;
                        ingressoLista.FiltroSQL = "ApresentacaoSetorID in (" + apresentacaoSetorLista + ")";
                        ingressoLista.Carregar();

                        if (ingressoLista.Tamanho > 0)
                        {
                            IngressoLogLista ingressoLogLista = new IngressoLogLista();
                            ingressoLogLista.FiltroSQL = "IngressoID in (" + ingressoLista + ")";
                            ingressoLogLista.Carregar();
                            ok = ingressoLogLista.ExcluirTudo();
                            if (ok)
                                ok = ingressoLista.ExcluirTudo();
                        }

                        if (ok)
                        {
                            PrecoLista precoLista = new PrecoLista();
                            precoLista.FiltroSQL = "ApresentacaoSetorID in (" + apresentacaoSetorLista + ")";
                            precoLista.Carregar();
                            if (precoLista.Tamanho > 0)
                            {
                                CanalPrecoLista canalPrecoLista = new CanalPrecoLista();
                                canalPrecoLista.FiltroSQL = "PrecoID in (" + precoLista + ")";
                                canalPrecoLista.Carregar();
                                ok = canalPrecoLista.ExcluirTudo();
                                if (ok)
                                    ok = precoLista.ExcluirTudo();
                            }
                        }
                    }

                    if (ok)
                        ok = apresentacaoSetorLista.ExcluirTudo();

                } // fim if (apresentacaoSetorLista.Tamanho > 0)
                else
                {
                    LugarLista lugarLista = new LugarLista();
                    lugarLista.FiltroSQL = "SetorID=" + this.Control.ID;
                    lugarLista.Carregar();

                    ok = lugarLista.ExcluirTudo();
                }

                if (ok)
                    ok = base.Excluir(this.Control.ID);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>		
        /// Obter porcentagem de ingressos (separado por status)
        /// Em função do Setor e Apresentacao
        /// </summary>
        public override DataTable PorcentagemIngressosStatus(string apresentacoes)
        {

            DataTable tabela = Utilitario.EstruturaPorcentagemIngressosStatus();
            try
            {
                DataTable quantidadeIngressosStatus = QuantidadeIngressosStatus(apresentacoes);
                decimal total = TotalIngressos(apresentacoes);
                foreach (DataRow linha in quantidadeIngressosStatus.Rows)
                {
                    DataRow linhaPorcentagem = tabela.NewRow();
                    linhaPorcentagem["ApresentacaoSetorID"] = 0;
                    linhaPorcentagem["ApresentacaoID"] = 0;
                    linhaPorcentagem["SetorID"] = 0;
                    linhaPorcentagem["Status"] = linha["Status"];
                    linhaPorcentagem["Quantidade"] = linha["Quantidade"];
                    linhaPorcentagem["CortesiaID"] = linha["CortesiaID"];
                    linhaPorcentagem["Porcentagem"] = (decimal)(Convert.ToDecimal(linha["Quantidade"]) / total) * 100;
                    tabela.Rows.Add(linhaPorcentagem);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>		
        /// Obter Quantidade de Ingressos (separado por status)
        /// Por Setor e Apresentacoes 
        /// </summary>
        public override DataTable QuantidadeIngressosStatus(string apresentacoes)
        {

            DataTable tabela = Utilitario.EstruturaQuantidadeIngressosStatus();
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT       COUNT(tIngresso.ID) AS Quantidade, tApresentacaoSetor.SetorID, tIngresso.Status, tIngresso.CortesiaID, tApresentacaoSetor.ApresentacaoID " +
                    "FROM         tIngresso INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "GROUP BY tApresentacaoSetor.SetorID, tIngresso.Status, tIngresso.CortesiaID, tApresentacaoSetor.ApresentacaoID " +
                    "HAVING        (tApresentacaoSetor.SetorID = " + this.Control.ID + ") AND (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) ";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["Status"] = bd.LerString("Status");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["CortesiaID"] = bd.LerInt("CortesiaID");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override int TotalIngressos(string apresentacoes)
        {
            int total = 0;
            try
            {
                // Obtendo Ingressos por Setor e por Apresentacao
                string sql =
                    "SELECT        COUNT(tIngresso.ID) AS Quantidade, tApresentacaoSetor.SetorID " +
                    "FROM            tIngresso INNER JOIN " +
                    "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID " +
                    "WHERE        (tApresentacaoSetor.ApresentacaoID IN (" + apresentacoes + ")) " +
                    "GROUP BY tApresentacaoSetor.SetorID " +
                    "HAVING        (tApresentacaoSetor.SetorID = " + this.Control.ID + ") ";
                bd.Consulta(sql);
                if (bd.Consulta().Read())
                {
                    total = bd.LerInt("Quantidade");
                }
                else
                {
                    total = -1;
                }
                bd.Fechar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            return total;
        }

        /// <summary>
        /// Retorna as apresentacoesSetoresIDs desse setor
        /// </summary>
        /// <returns></returns>
        public override int[] ApresentacoesSetoresIDs()
        {

            try
            {

                ArrayList ids = new ArrayList();

                string sql = "SELECT ID FROM tApresentacaoSetor WHERE SetorID=" + this.Control.ID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    int id = bd.LerInt("ID");
                    ids.Add(id);
                }

                bd.Fechar();

                return (int[])ids.ToArray(typeof(int));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um 'stringao' com os lugares desse Setor
        /// </summary>
        /// <returns></returns>
        public override string Mapa()
        {

            try
            {

                StringBuilder mapa = new StringBuilder();

                //TODO: Mapa

                string sql = "SELECT ID AS LugarID,Codigo,Quantidade,PosicaoX,PosicaoY " +
                    "FROM tLugar " +
                    "WHERE SetorID=" + this.Control.ID + " " +
                    "ORDER BY ID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {

                    int lugarID = bd.LerInt("LugarID");
                    string codigo = bd.LerString("Codigo");
                    int quantidade = bd.LerInt("Quantidade");
                    int posicaoX = bd.LerInt("PosicaoX");
                    int posicaoY = bd.LerInt("PosicaoY");

                    mapa.Append(lugarID + ":" + codigo + ":" + quantidade + ":" + posicaoX + ":" + posicaoY + "|");
                    //cada lugar corresponde a uma |
                    //cada : corresponde a um campo
                }

                bd.Fechar();

                return mapa.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string TipoLugarMarcado(int apresentacaoSetorID)
        {
            string lugarTipo = string.Empty;

            string sql = @"SELECT tSetor.LugarMarcado FROM tSetor(NOLOCK)
							INNER JOIN tApresentacaoSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID
							WHERE tapresentacaoSetor.ID = " + apresentacaoSetorID;

            bd.Consulta(sql);

            if (bd.Consulta().Read())
            {
                lugarTipo = bd.LerString("LugarMarcado");

                switch (lugarTipo)
                {
                    case Setor.Cadeira:
                        return Setor.Cadeira;
                    case Setor.MesaAberta:
                        return Setor.MesaAberta;
                    case Setor.MesaFechada:
                        return Setor.MesaFechada;
                    case Setor.Pista:
                        return Setor.Pista;
                    default:
                        throw new SetorException("Setor não encontrado");

                }
            }
            else
                throw new SetorException("Setor não encontrado");
        }

        /// <summary>
        /// Método responsável por retornar um DataTable com os lugares de um setor ou apenas uma indicação como SETOR PISTA
        /// </summary>
        /// <param name="database">Objeto bd, utilizado para evitar abertura de conexões e trabalhar transaction</param>
        /// <returns>IDataReader Interface</returns>
        public CTLib.BD Lugares(int setorID)
        {
            string sql = @"SELECT " +
                @"tLugar.ID, tLugar.Codigo, tLugar.Quantidade, tLugar.QuantidadeBloqueada, tLugar.BloqueioID, tLugar.Grupo, tLugar.Classificacao, LugarMarcado, tSetor.Nome AS SetorNome " +
                @"FROM tLugar (NOLOCK) " +
                @"RIGHT JOIN tSetor (NOLOCK) ON tSetor.ID = tLugar.SetorID " +
                @"WHERE tSetor.ID = " + setorID + @" ORDER BY Codigo";

            bd.Consulta(sql);
            return bd;
        }

        /// <summary>		
        /// Obtem os lugares desse setor
        /// </summary>
        /// <returns></returns>
        public override DataTable Lugares()
        {

            try
            {

                //TODO: Lugares

                DataTable tabela = new DataTable("Lugar");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Codigo", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("QuantidadeBloqueada", typeof(int));
                tabela.Columns.Add("PosicaoX", typeof(int));
                tabela.Columns.Add("PosicaoY", typeof(int));
                tabela.Columns.Add("BloqueioID", typeof(int));
                tabela.Columns.Add("Classificacao", typeof(int));
                tabela.Columns.Add("Grupo", typeof(int));
                tabela.Columns.Add("PerspectivaLugarID", typeof(int)).DefaultValue = 0;
                tabela.Columns.Add("DescricaoPerspectiva", typeof(string)).DefaultValue = string.Empty;
                tabela.Columns.Add("PodeExcluir", typeof(bool)).DefaultValue = true;

                var sql = String.Format("PR_LISTAR_LUGARES " + this.Control.ID);
                //para ordem de desenho de mapa

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Codigo"] = bd.LerString("Codigo");
                    linha["Quantidade"] = bd.LerInt("Quantidade");
                    linha["QuantidadeBloqueada"] = bd.LerInt("QuantidadeBloqueada");
                    linha["PosicaoX"] = bd.LerInt("PosicaoX");
                    linha["PosicaoY"] = bd.LerInt("PosicaoY");
                    linha["BloqueioID"] = bd.LerInt("BloqueioID");
                    linha["Classificacao"] = bd.LerInt("Classificacao");
                    linha["Grupo"] = bd.LerInt("Grupo");
                    linha["PerspectivaLugarID"] = bd.LerInt("PerspectivaLugarID");
                    linha["DescricaoPerspectiva"] = bd.LerString("DescricaoPerspectiva");
                    linha["PodeExcluir"] = bd.LerBoolean("PodeExcluir");
                    tabela.Rows.Add(linha);
                }

                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obtem a quantidade total de lugares desse setor de lugar marcado
        /// </summary>
        /// <returns></returns>
        public override int Quantidade()
        {

            try
            {

                if (this.LugarMarcado.Valor == "")
                    this.Ler(this.Control.ID);

                if (this.LugarMarcado.Valor != Pista)
                {

                    string sql = "SELECT SUM(Quantidade) AS Qtde " +
                        "FROM tLugar " +
                        "WHERE SetorID=" + this.Control.ID;

                    object obj = bd.ConsultaValor(sql);

                    bd.Fechar();

                    int qtde = (obj != null) ? (int)obj : 0;

                    return qtde;

                }
                else
                {
                    if (this.Nome.Valor == "")
                        this.Ler(this.Control.ID);

                    throw new SetorException(this.Nome.Valor + " não é de lugar-marcado");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>		
        /// Obtem a quantidade total de lugares desse setor
        /// </summary>
        /// <returns></returns>
        public override int Quantidade(int apresentacaoid)
        {

            try
            {

                int qtde;

                if (this.LugarMarcado.Valor == "")
                    this.Ler(this.Control.ID);

                if (this.LugarMarcado.Valor == Pista)
                {

                    string sql = "SELECT Count(i.ID) AS Qtde " +
                        "FROM tIngresso as i,tApresentacaoSetor as aps " +
                        "WHERE aps.ID=i.ApresentacaoSetorID AND " +
                        "aps.ApresentacaoID=" + apresentacaoid + " AND aps.SetorID=" + this.Control.ID;

                    object obj = bd.ConsultaValor(sql);

                    bd.Fechar();

                    if (obj != null)
                        qtde = (int)obj;
                    else
                        throw new SetorException("Setor ou apresentação não existe.");

                    return qtde;

                }
                else
                {
                    return Quantidade();
                    //					if (this.Nome.Valor == "")
                    //						this.Ler(this.Control.ID);
                    //
                    //					throw new SetorException(this.Nome.Valor+" não é de lugar-não-marcado");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obtem a quantidade de lugares bloqueados desse setor dada uma apresentacao
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeBloqueado(int apresentacaoid)
        {

            try
            {

                int qtde;

                string sql = "SELECT Count(i.ID) AS Qtde " +
                    "FROM tIngresso as i,tApresentacaoSetor as aps " +
                    "WHERE i.Status='" + Ingresso.BLOQUEADO + "' AND aps.ID=i.ApresentacaoSetorID AND " +
                    "aps.ApresentacaoID=" + apresentacaoid + " AND aps.SetorID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new SetorException("Setor ou apresentação não existe.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obtem a quantidade de lugares disponiveis desse setor dada uma apresentacao
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeDisponivel(int apresentacaoid)
        {

            try
            {

                int qtde;

                string sql = "SELECT Count(i.ID) AS Qtde " +
                    "FROM tIngresso as i,tApresentacaoSetor as aps " +
                    "WHERE i.Status='" + Ingresso.DISPONIVEL + "' AND aps.ID=i.ApresentacaoSetorID AND " +
                    "aps.ApresentacaoID=" + apresentacaoid + " AND aps.SetorID=" + this.Control.ID;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                if (obj != null)
                    qtde = (int)obj;
                else
                    throw new SetorException("Setor ou apresentação não existe.");

                return qtde;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool PossuiBackground(int setorID)
        {
            this.Control.ID = setorID;
            return this.PossuiBackground();
        }
        public bool PossuiBackground()
        {
            try
            {
                var versaoImagem = Convert.ToInt32(bd.ConsultaValor("SELECT VersaoBackground FROM tSetor WHERE ID = " + this.Control.ID));
                return versaoImagem > 0;

            }
            finally
            {
                bd.Fechar();
            }
        }


        /// <summary>
        /// Atualiza Classificação e Grupo nos ingressos
        /// </summary>
        public bool AtualizarMapa()
        {

            try
            {

                string sql = "UPDATE tIngresso " +
                    "SET tIngresso.Classificacao=tLugar.Classificacao, tIngresso.Grupo=tLugar.Grupo " +
                    "FROM tIngresso " +
                    "INNER JOIN tLugar ON tIngresso.LugarID = tLugar.ID " +
                    "WHERE tLugar.SetorID=" + this.Control.ID;

                int x = bd.Executar(sql);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Obsolete("", true)]
        public void AtualizarMapaPorApresentacao(int[] apresentacoes)
        {

            try
            {
                StringBuilder sbApresentacoes = new StringBuilder();
                StringBuilder sbSetoresID = new StringBuilder();

                for (int i = 0; i < apresentacoes.Length; i++)
                {
                    sbApresentacoes.Append(apresentacoes[i].ToString() + ", ");
                }
                sbApresentacoes.Remove(sbApresentacoes.Length - 2, 2);


                string sql = @"SELECT DISTINCT SetorID, LugarMarcado FROM tIngresso (NOLOCK)
                            INNER JOIN tSetor (NOLOCK) ON tIngresso.SetorID = tSetor.ID
                            WHERE ApresentacaoID IN(" + sbApresentacoes.ToString() + ")";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    if (bd.LerString("LugarMarcado").ToString() != "P")
                    {
                        sbSetoresID.Append(bd.LerInt("SetorID").ToString() + ", ");
                    }
                }
                if (sbSetoresID.Length > 2)
                {
                    sbSetoresID.Remove(sbSetoresID.Length - 2, 2);

                    sql = "UPDATE tIngresso " +
                        "SET tIngresso.Classificacao=tLugar.Classificacao, tIngresso.Grupo=tLugar.Grupo " +
                        "FROM tIngresso " +
                        "INNER JOIN tLugar ON tIngresso.LugarID = tLugar.ID " +
                        "WHERE tLugar.SetorID IN (" + sbSetoresID + ")";

                    bd.Executar(sql);

                    bd.Fechar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Obter os precos dessa apresentacao e setor
        /// </summary>
        /// <returns></returns>
        public DataTable Precos(int apresentacaoID, int setorID, Apresentacao.Disponibilidade disponibilidade)
        {

            try
            {

                DataTable tabela = new DataTable("Setor");

                // Verificando a disponibilidade
                string disponivelVenda = ((disponibilidade & Apresentacao.Disponibilidade.Vender) == Apresentacao.Disponibilidade.Vender) ? " AND a.DisponivelVenda='T' " : "";
                string disponivelAjuste = ((disponibilidade & Apresentacao.Disponibilidade.Ajustar) == Apresentacao.Disponibilidade.Ajustar) ? " AND a.DisponivelAjuste='T' " : "";
                string disponivelRelatorio = ((disponibilidade & Apresentacao.Disponibilidade.GerarRelatorio) == Apresentacao.Disponibilidade.GerarRelatorio) ? " AND a.DisponivelRelatorio='T' " : "";

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Valor", typeof(decimal));
                tabela.Columns.Add("SetorID", typeof(int));
                tabela.Columns.Add("ApresentacaoID", typeof(int));
                tabela.Columns.Add("ApresentacaoSetorID", typeof(int));

                string sql = "SELECT p.ID, p.Nome,p.Valor, tas.ID AS ApresentacaoSetorID FROM tpreco as p, tApresentacaoSetor as tas,tSetor as s, tApresentacao as a " +
                    "WHERE tas.SetorID=s.ID AND tas.ApresentacaoID=" + apresentacaoID + "AND tas.SetorID=" + setorID + " AND p.ApresentacaoSetorID = tas.ID AND a.ID=tas.ApresentacaoID " +
                    disponivelVenda +
                    disponivelAjuste +
                    disponivelRelatorio +
                    "ORDER BY s.Nome";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["Valor"] = bd.LerDecimal("Valor");
                    linha["SetorID"] = setorID;
                    linha["ApresentacaoID"] = apresentacaoID;
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>		
        /// Obtem o primeiro preço cadastrado com ingresso disponivel para esse setor dado uma apresentacao
        /// </summary>
        /// <returns></returns>
        public override decimal PrimeiroPrecoDisponivel(int apresentacaoid)
        {

            try
            {

                decimal preco;

                string sql = "SELECT top 1 Valor " +
                    "FROM tPreco as p, tApresentacaoSetor as aps " +
                    "WHERE aps.ID=p.ApresentacaoSetorID AND " +
                    "(p.Quantidade=0 OR p.Quantidade > (SELECT Count(*) FROM tIngresso WHERE PrecoID=p.ID)) AND " +
                    "aps.SetorID=" + this.Control.ID + " AND aps.ApresentacaoID=" + apresentacaoid;

                object obj = bd.ConsultaValor(sql);

                bd.Fechar();

                preco = (obj != null) ? (decimal)obj : 0;

                return preco;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private decimal CalculaPct(object valor, object valorTotal)
        {
            try
            {
                return Convert.ToDecimal(Convert.ToDecimal((((decimal)valor * 100) / (decimal)valorTotal)).ToString(Utilitario.FormatoPorcentagem1Casa));

            }
            catch
            {
                return 0;
            }
        }

        private string VerificaCompute(object valor)
        {
            try
            {
                return Convert.ToDecimal(valor).ToString(Utilitario.FormatoMoeda); ;
            }
            catch
            {
                return "0";
            }
        }

        public List<EstruturaSetores> getSetoresPorLocalApresentacoes(List<int> apresentacoesID)
        {
            try
            {
                List<EstruturaSetores> lista = new List<EstruturaSetores>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT DISTINCT SetorID, tSetor.Nome FROM tApresentacaoSetor (NOLOCK) ");
                stbSQL.Append("INNER JOIN tSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID ");
                stbSQL.Append("WHERE tApresentacaoSetor.ApresentacaoID IN (" + Utilitario.ArrayToString(apresentacoesID.ToArray()) + " ) ");
                stbSQL.Append("GROUP BY SetorID, tSetor.Nome ");
                stbSQL.Append("HAVING Count(Distinct ApresentacaoID) = " + apresentacoesID.Count);

                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    EstruturaSetores oEstrutura = new EstruturaSetores();
                    oEstrutura.ID = bd.LerInt("SetorID");
                    oEstrutura.Nome = bd.LerString("Nome");
                    lista.Add(oEstrutura);
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

        /// <summary>
        /// Vendas Gerenciais por setor com Quantidade e Valores dos Ingressos dos Vendidos e Cancelados e Total
        /// Com porcentagem
        /// </summary>
        public override DataTable VendasGerenciais(string dataInicial, string dataFinal, bool comCortesia, int apresentacaoID, int eventoID, int localID, int empresaID, bool vendasCanal, string tipoLinha, bool disponivel, bool empresaVendeIngressos, bool empresaPromoveEventos)
        {
            try
            {
                int usuarioID = 0;
                int lojaID = 0;
                int canalID = 0;
                if (vendasCanal)
                { // se for por Canal, os parâmetro têm representações diferentes
                    usuarioID = apresentacaoID;
                    lojaID = eventoID;
                    canalID = localID;
                    apresentacaoID = 0;
                    eventoID = 0;
                    localID = 0;
                }
                // Variáveis usados no final do Grid (totalizando)
                int quantidadeVendidosTotais = 0;
                int quantidadeCanceladosTotais = 0;
                int quantidadeTotalTotais = 0;
                decimal valoresVendidosTotais = 0;
                decimal valoresCanceladosTotais = 0;
                decimal valoresTotalTotais = 0;
                decimal quantidadeVendidosTotaisPorcentagem = 0;
                decimal quantidadeCanceladosTotaisPorcentagem = 0;
                decimal quantidadeTotalTotaisPorcentagem = 0;
                decimal valoresVendidosTotaisPorcentagem = 0;
                decimal valoresCanceladosTotaisPorcentagem = 0;
                decimal valoresTotalTotaisPorcentagem = 0;
                #region Obter os dados na condição especificada
                // Filtrando as condições
                IngressoLog ingressoLog = new IngressoLog(); // obter em função de vendidos e cancelados
                Caixa caixa = new Caixa();
                string ingressoLogIDsTotais = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos + "," + ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsVendidos = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Vendidos, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                string ingressoLogIDsCancelados = caixa.IngressoLogIDsPorPeriodoCaixaSQL(dataInicial, dataFinal, ingressoLog.Cancelados, comCortesia,
                    apresentacaoID, eventoID, localID, empresaID, 0, 0, 0, tipoLinha, disponivel, vendasCanal, empresaVendeIngressos, empresaPromoveEventos);
                // Linhas do Grid
                DataTable tabela = LinhasVendasGerenciais(ingressoLogIDsTotais);
                // Totais antecipado para poder calcular porcentagem no laço
                this.Control.ID = 0; // setor zero pega todos
                int totaisVendidos = QuantidadeIngressosPorSetor(ingressoLogIDsVendidos);
                int totaisCancelados = QuantidadeIngressosPorSetor(ingressoLogIDsCancelados);
                int totaisTotal = totaisVendidos - totaisCancelados;
                decimal valoresVendidos = ValorIngressosPorSetor(ingressoLogIDsVendidos);
                decimal valoresCancelados = ValorIngressosPorSetor(ingressoLogIDsCancelados);
                decimal valoresTotal = valoresVendidos - valoresCancelados;
                #endregion
                // Para cada setor na condição especificada, calcular
                foreach (DataRow linha in tabela.Rows)
                {
                    this.Control.ID = Convert.ToInt32(linha["VariacaoLinhaID"]);
                    #region Quantidade
                    // Vendidos
                    linha["Qtd Vend"] = QuantidadeIngressosPorSetor(ingressoLogIDsVendidos);
                    if (totaisVendidos > 0)
                        linha["% Vend"] = (decimal)Convert.ToInt32(linha["Qtd Vend"]) / (decimal)totaisVendidos * 100;
                    else
                        linha["% Vend"] = 0;
                    // Cancelados
                    linha["Qtd Canc"] = QuantidadeIngressosPorSetor(ingressoLogIDsCancelados);
                    if (totaisCancelados > 0)
                        linha["% Canc"] = (decimal)Convert.ToInt32(linha["Qtd Canc"]) / (decimal)totaisCancelados * 100;
                    else
                        linha["% Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["Qtd Total"] = Convert.ToInt32(linha["Qtd Vend"]) - Convert.ToInt32(linha["Qtd Canc"]);
                    if (totaisTotal > 0)
                        linha["% Total"] = (decimal)Convert.ToInt32(linha["Qtd Total"]) / (decimal)totaisTotal * 100;
                    else
                        linha["% Total"] = 0;
                    // Totalizando
                    quantidadeVendidosTotais += Convert.ToInt32(linha["Qtd Vend"]);
                    quantidadeCanceladosTotais += Convert.ToInt32(linha["Qtd Canc"]);
                    quantidadeTotalTotais += Convert.ToInt32(linha["Qtd Total"]);
                    quantidadeVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% Vend"]);
                    quantidadeCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% Canc"]);
                    quantidadeTotalTotaisPorcentagem += Convert.ToDecimal(linha["% Total"]);
                    // Formato
                    linha["% Total"] = Convert.ToDecimal(linha["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Vend"] = Convert.ToDecimal(linha["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% Canc"] = Convert.ToDecimal(linha["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                    #region Valor
                    // Vendidos
                    linha["R$ Vend"] = ValorIngressosPorSetor(ingressoLogIDsVendidos);
                    if (valoresVendidos > 0)
                        linha["% R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]) / valoresVendidos * 100;
                    else
                        linha["% R$ Vend"] = 0;
                    // Cancelados
                    linha["R$ Canc"] = ValorIngressosPorSetor(ingressoLogIDsCancelados);
                    if (valoresCancelados > 0)
                        linha["% R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]) / valoresCancelados * 100;
                    else
                        linha["% R$ Canc"] = 0;
                    // Total (diferença), não posso usar o método para obter, pois IngressoID do Vendido é igual do cancelado
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Vend"]) - Convert.ToDecimal(linha["R$ Canc"]);
                    if (valoresTotal > 0)
                        linha["% R$ Total"] = Convert.ToDecimal(linha["R$ Total"]) / valoresTotal * 100;
                    else
                        linha["% R$ Total"] = 0;
                    // Totalizando
                    valoresVendidosTotais += Convert.ToDecimal(linha["R$ Vend"]);
                    valoresCanceladosTotais += Convert.ToDecimal(linha["R$ Canc"]);
                    valoresTotalTotais += Convert.ToDecimal(linha["R$ Total"]);
                    valoresVendidosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Vend"]);
                    valoresCanceladosTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Canc"]);
                    valoresTotalTotaisPorcentagem += Convert.ToDecimal(linha["% R$ Total"]);
                    // Formato
                    linha["R$ Total"] = Convert.ToDecimal(linha["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Vend"] = Convert.ToDecimal(linha["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linha["R$ Canc"] = Convert.ToDecimal(linha["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linha["% R$ Total"] = Convert.ToDecimal(linha["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Vend"] = Convert.ToDecimal(linha["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linha["% R$ Canc"] = Convert.ToDecimal(linha["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    #endregion
                }
                if (tabela.Rows.Count > 0)
                {
                    DataRow linhaTotais = tabela.NewRow();
                    // Totais
                    linhaTotais["VariacaoLinha"] = "Totais";
                    linhaTotais["Qtd Total"] = quantidadeTotalTotais;
                    linhaTotais["Qtd Vend"] = quantidadeVendidosTotais;
                    linhaTotais["Qtd Canc"] = quantidadeCanceladosTotais;
                    linhaTotais["% Total"] = quantidadeTotalTotaisPorcentagem;
                    linhaTotais["% Vend"] = quantidadeVendidosTotaisPorcentagem;
                    linhaTotais["% Canc"] = quantidadeCanceladosTotaisPorcentagem;
                    linhaTotais["R$ Total"] = valoresTotalTotais;
                    linhaTotais["R$ Vend"] = valoresVendidosTotais;
                    linhaTotais["R$ Canc"] = valoresCanceladosTotais;
                    linhaTotais["% R$ Total"] = valoresTotalTotaisPorcentagem;
                    linhaTotais["% R$ Vend"] = valoresVendidosTotaisPorcentagem;
                    linhaTotais["% R$ Canc"] = valoresCanceladosTotaisPorcentagem;
                    // Formato
                    linhaTotais["% Total"] = Convert.ToDecimal(linhaTotais["% Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Vend"] = Convert.ToDecimal(linhaTotais["% Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% Canc"] = Convert.ToDecimal(linhaTotais["% Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["R$ Total"] = Convert.ToDecimal(linhaTotais["R$ Total"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Vend"] = Convert.ToDecimal(linhaTotais["R$ Vend"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["R$ Canc"] = Convert.ToDecimal(linhaTotais["R$ Canc"]).ToString(Utilitario.FormatoMoeda);
                    linhaTotais["% R$ Total"] = Convert.ToDecimal(linhaTotais["% R$ Total"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Vend"] = Convert.ToDecimal(linhaTotais["% R$ Vend"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    linhaTotais["% R$ Canc"] = Convert.ToDecimal(linhaTotais["% R$ Canc"]).ToString(Utilitario.FormatoPorcentagem1Casa);
                    tabela.Rows.Add(linhaTotais);
                }
                tabela.Columns["VariacaoLinha"].ColumnName = "Setor";
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Setores por definido pelos IngressoLogIDs
        /// </summary>
        public override DataTable LinhasVendasGerenciais(string ingressoLogIDs)
        {
            try
            {
                DataTable tabela = Utilitario.EstruturaVendasGerenciais();
                if (ingressoLogIDs != "")
                {
                    // Obtendo dados através de SQL
                    BD obterDados = new BD();
                    string sql =
                        "SELECT DISTINCT tSetor.ID, tSetor.Nome AS Setor " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) ";
                    obterDados.Consulta(sql);
                    while (obterDados.Consulta().Read())
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VariacaoLinhaID"] = obterDados.LerInt("ID");
                        linha["VariacaoLinha"] = obterDados.LerString("Setor");
                        tabela.Rows.Add(linha);
                    }
                    obterDados.Fechar();
                }
                return tabela;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override int QuantidadeIngressosPorSetor(string ingressoLogIDs)
        {
            try
            {
                int quantidade = 0;
                if (ingressoLogIDs != "")
                {
                    // Trantando a condição
                    string condicaoSetor = "";
                    if (this.Control.ID > 0)
                        condicaoSetor = "AND (tSetor.ID = " + this.Control.ID + ") ";
                    else
                        condicaoSetor = " "; // todos se for = zero
                    // Obtendo dados
                    string sql;
                    sql =
                        "SELECT   COUNT(tSetor.ID) AS QuantidadeIngressos " +
                        "FROM     tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoSetor;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Quantidade de setor
                        if (bd.Consulta().Read())
                        {
                            quantidade = bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    else
                    {
                        // Quantidade de todos setores
                        while (bd.Consulta().Read())
                        {
                            quantidade += bd.LerInt("QuantidadeIngressos");
                        }
                    }
                    bd.Fechar();
                }
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorSetor
        /// <summary>
        /// Obter quantidade de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public int QuantidadeIngressosBloqueados(int apresentacaoID)
        {
            try
            {
                int quantidade = 0;
                // Trantando a condição
                string condicaoSetor = "";
                if (this.Control.ID > 0)
                    condicaoSetor = "AND (tApresentacaoSetor.SetorID = " + this.Control.ID + ") ";
                else
                    condicaoSetor = " "; // todos se for = zero
                // Obtendo dados
                string sql;
                sql =
                    "SELECT        tApresentacaoSetor.SetorID, COUNT(tIngresso.ID) AS QuantidadeIngressos, tIngresso.Status " +
                    "FROM            tApresentacaoSetor INNER JOIN " +
                    "tIngresso ON tApresentacaoSetor.ID = tIngresso.ApresentacaoSetorID " +
                    "GROUP BY tApresentacaoSetor.SetorID, tIngresso.Status, tApresentacaoSetor.ApresentacaoID " +
                    "HAVING        (tIngresso.Status = '" + Ingresso.BLOQUEADO + "') AND (tApresentacaoSetor.ApresentacaoID = " + apresentacaoID + ") " + condicaoSetor;
                bd.Consulta(sql);
                if (this.Control.ID > 0)
                {
                    // Quantidade de setor
                    if (bd.Consulta().Read())
                    {
                        quantidade = bd.LerInt("QuantidadeIngressos");
                    }
                }
                else
                {
                    // Quantidade de todos setores
                    while (bd.Consulta().Read())
                    {
                        quantidade += bd.LerInt("QuantidadeIngressos");
                        // tem sentido se incluir SetorID
                    }
                }
                bd.Fechar();
                return quantidade;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de QuantidadeIngressosPorSetor
        /// <summary>
        /// Obter valor de ingressos em função do IngressoIDs
        /// </summary>
        /// <returns></returns>
        public override decimal ValorIngressosPorSetor(string ingressoLogIDs)
        {
            try
            {
                int valor = 0;
                if (ingressoLogIDs != "")
                {
                    string condicaoSetor = "";
                    // Obtendo dados
                    if (this.Control.ID > 0)
                        condicaoSetor = "AND (tSetor.ID = " + this.Control.ID + ") ";
                    else
                        condicaoSetor = " "; // todos se for = zero
                    string sql;
                    sql =
                        "SELECT    SUM(tPreco.Valor) AS Valor " +
                        "FROM      tIngressoLog INNER JOIN " +
                        "tIngresso ON tIngressoLog.IngressoID = tIngresso.ID INNER JOIN " +
                        "tApresentacaoSetor ON tIngresso.ApresentacaoSetorID = tApresentacaoSetor.ID INNER JOIN " +
                        "tPreco ON tIngresso.PrecoID = tPreco.ID INNER JOIN " +
                        "tSetor ON tApresentacaoSetor.SetorID = tSetor.ID " +
                        "WHERE (tIngressoLog.ID IN (" + ingressoLogIDs + ")) " + condicaoSetor;
                    bd.Consulta(sql);
                    if (this.Control.ID > 0)
                    {
                        // Valor do setor
                        if (bd.Consulta().Read())
                        {
                            valor = bd.LerInt("Valor");
                        }
                    }
                    else
                    {
                        // Valor de todos setores
                        while (bd.Consulta().Read())
                        {
                            valor += bd.LerInt("Valor");
                        }
                    }
                    bd.Fechar();
                }
                return valor;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        } // fim de ValorIngressosPorSetor

        /// <summary>		
        /// Obtem os Todos setores
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {
            try
            {
                DataTable tabela = new DataTable("Setor");
                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("LocalID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("NomeInterno", typeof(string));
                tabela.Columns.Add("LugarMarcado", typeof(string));
                tabela.Columns.Add("Produto", typeof(bool));
                tabela.Columns.Add("Descricao", typeof(string));
                tabela.Columns.Add("AprovadoPublicacao", typeof(bool)).DefaultValue = false;
                tabela.Columns.Add("VersaoBackground", typeof(int)).DefaultValue = 0;
                // pega todos setores
                string sql =
                    "SELECT * FROM tSetor (NOLOCK) ORDER BY NOME";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    DataRow linha = tabela.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["LocalID"] = bd.LerInt("LocalID");
                    linha["Nome"] = bd.LerString("Nome");
                    linha["NomeInterno"] = bd.LerString("NomeInterno");
                    linha["LugarMarcado"] = bd.LerString("LugarMarcado");
                    linha["Produto"] = bd.LerBoolean("Produto");
                    linha["Descricao"] = bd.LerString("Descricao");
                    linha["AprovadoPublicacao"] = bd.LerBoolean("AprovadoPublicacao");
                    linha["VersaoBackground"] = bd.LerInt("VersaoBackground");
                    tabela.Rows.Add(linha);
                }
                bd.Fechar();
                return tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable gerarEstruturaCascading()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("ID");
            dtt.Columns.Add("Setor");
            dtt.Columns.Add("ApresentacaoID");
            dtt.Columns.Add("ApresentacaoSetorID");
            return dtt;
        }

        public DataTable SetoresPorApresentacaoCascading(int apresentacaoID)
        {
            try
            {
                DataTable dtt = gerarEstruturaCascading();

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT tSetor.ID, tSetor.Nome AS Setor, tApresentacaoSetor.ID AS ApresentacaoSetorID FROM tSetor (NOLOCK) ");
                stbSQL.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID ");
                stbSQL.Append("WHERE ApresentacaoID = " + apresentacaoID);
                stbSQL.Append(" ORDER BY tSetor.Nome");
                bd.Consulta(stbSQL.ToString());

                while (bd.Consulta().Read())
                {
                    DataRow linha = dtt.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Setor"] = bd.LerString("Setor");
                    linha["ApresentacaoID"] = apresentacaoID;
                    linha["ApresentacaoSetorID"] = bd.LerInt("ApresentacaoSetorID");
                    dtt.Rows.Add(linha);
                }

                return dtt;

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


        public List<EstruturaIDNome> BuscarPorLocalID(int localID)
        {
            try
            {
                List<EstruturaIDNome> retorno = new List<EstruturaIDNome>();
                string sql = @"SELECT ID, Nome FROM tSetor WHERE LocalID = " + localID + " ORDER BY Nome";
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                    retorno.Add(new EstruturaIDNome() { ID = bd.LerInt("ID"), Nome = bd.LerString("Nome") });

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

        public List<EstruturaAssinaturaSetor> ListaSetoresAssinatura(List<int> ApresentacoesID)
        {
            try
            {
                List<EstruturaAssinaturaSetor> retorno = new List<EstruturaAssinaturaSetor>();

                string sql = @"SELECT tApresentacaoSetor.SetorID as ID, tSetor.Nome
                ,COUNT(tApresentacaoSetor.ApresentacaoID) as QuantidadeApresentacao
                FROM tApresentacaoSetor (NOLOCK)
                INNER JOIN tSetor ON tApresentacaoSetor.SetorID = tSetor.ID
                WHERE tApresentacaoSetor.ApresentacaoID in (" + Utilitario.ArrayToString(ApresentacoesID.ToArray()) + @") 
                GROUP BY tApresentacaoSetor.SetorID, tSetor.Nome ORDER BY tSetor.Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaAssinaturaSetor
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        QtdApresentacoes = bd.LerInt("QuantidadeApresentacao"),
                        Incluir = false
                    });
                }

                bd.Fechar();


                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<EstruturaAssinaturaSetor> ListaSetoresAssinatura(List<int> ApresentacoesID, int AssinaturaID, int Ano)
        {
            try
            {
                List<EstruturaAssinaturaSetor> retorno = new List<EstruturaAssinaturaSetor>();

                string sql = @"SELECT tApresentacaoSetor.SetorID as ID, tSetor.Nome,
	                            COUNT(DISTINCT tApresentacaoSetor.ApresentacaoID) as QuantidadeApresentacao,
                                CASE WHEN COUNT(tAssinaturaAno.AssinaturaID) > 0
                                    THEN 'T'
                                    ELSE 'F'
                                    END as Incluir
                                FROM tApresentacaoSetor (NOLOCK)
                                INNER JOIN tSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID
                                LEFT JOIN tAssinaturaItem (nolock) ON tAssinaturaItem.SetorID = tSetor.ID
	                            LEFT JOIN tAssinaturaAno (nolock) ON tAssinaturaItem.AssinaturaAnoID = tAssinaturaAno.ID 
                                AND tAssinaturaAno.AssinaturaID = " + AssinaturaID + " AND Ano = " + Ano + @" 
                                WHERE tApresentacaoSetor.ApresentacaoID IN (" + Utilitario.ArrayToString(ApresentacoesID.ToArray()) + @") 
                                GROUP BY tApresentacaoSetor.SetorID, tSetor.Nome ORDER BY tSetor.Nome";
                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    retorno.Add(new EstruturaAssinaturaSetor
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                        QtdApresentacoes = bd.LerInt("QuantidadeApresentacao"),
                        Incluir = bd.LerBoolean("Incluir"),
                    });
                }

                bd.Fechar();


                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class SetorLista : SetorLista_B
    {

        public SetorLista() { }

        public SetorLista(int usuarioIDLogado) : base(usuarioIDLogado) { }


        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioSetor");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("NomeInterno", typeof(string));
                    tabela.Columns.Add("Lugar", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["Nome"] = setor.Nome.Valor;
                        linha["NomeInterno"] = setor.NomeInterno.Valor;
                        linha["Lugar"] = setor.LugarMarcado.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable DistinctSetoresPorLocal(DataTable setor, string filtroLocal, params string[] columns)
        {
            try
            {
                DataTable setoresDistinct = TabelaMemoria.DistinctComFiltro(setor, filtroLocal, columns);
                return setoresDistinct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void FiltrarPorNome(string texto)
        {
            if (lista.Count > listaBasePesquisa.Count || listaBasePesquisa.Count == 0)
                listaBasePesquisa = lista;

            string IDsAtuais = Utilitario.ArrayToString(listaBasePesquisa);
            BD bd = new BD();
            try
            {
                bd.Consulta("SELECT ID FROM tSetor WHERE ID IN (" + IDsAtuais + ") AND Nome like '%" + texto.Replace("'", "''").Trim() + "%' ORDER BY Nome");

                ArrayList listaNova = new ArrayList();
                while (bd.Consulta().Read())
                    listaNova.Add(bd.LerInt("ID"));

                if (listaNova.Count > 0)
                    lista = listaNova;
                else
                    throw new Exception("Nenhum resultado para a pesquisa!");

                lista.TrimToSize();
                this.Primeiro();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

}
