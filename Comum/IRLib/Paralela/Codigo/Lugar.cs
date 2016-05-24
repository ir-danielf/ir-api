/**************************************************
* Arquivo: Lugar.cs
* Gerado: 02/06/2005
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Lugar : Lugar_B
    {

        public Lugar() { }

        public Lugar(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>		
        /// Obter os ingressos desse lugar
        /// </summary>
        /// <returns></returns>
        public override DataTable Ingressos()
        {

            return null;

        }

        /// <summary>
        /// Bloquear esse lugar
        /// </summary>
        /// <returns></returns>
        public override bool Bloquear(int bloqueioid)
        {

            return false;

        }

        /// <summary>
        /// Desbloquear esse lugar
        /// </summary>
        /// <returns></returns>
        public override bool Desbloquear()
        {

            return false;

        }

        public delegate void salvarLugaresDelegate(int porcentagem);

        public event salvarLugaresDelegate atualizarBarraDeProgresso;

        int controlePorcentagem = 0;

        int porcentagemAtual = 10;

        /// <summary>
        /// Função usada para salvar novos lugares na tela MapaDesenhar
        /// </summary>
        /// <returns>Vetor de ClientObjects.Lugar contendo as informações do lugar assim como o ID inserido.</returns>
        public IRLib.Paralela.ClientObjects.Lugar[] SalvarLugaresInserir(IRLib.Paralela.ClientObjects.Lugar[] listaInserir)
        {

            for (int i = 0; i < listaInserir.Length; i++)
            {
                this.Control.ID = listaInserir[i].ID;
                this.SetorID.ValorBD = listaInserir[i].SetorID.ToString();
                this.Codigo.ValorBD = listaInserir[i].Codigo;
                this.Quantidade.ValorBD = listaInserir[i].Quantidade.ToString();
                this.QuantidadeBloqueada.ValorBD = listaInserir[i].QuantidadeBloqueada.ToString();
                this.PosicaoX.ValorBD = listaInserir[i].PosicaoX.ToString();
                this.PosicaoY.ValorBD = listaInserir[i].PosicaoY.ToString();
                this.Simbolo.ValorBD = listaInserir[i].Simbolo.ToString();
                this.BloqueioID.ValorBD = listaInserir[i].BloqueioID.ToString();
                this.Classificacao.ValorBD = listaInserir[i].Classificacao.ToString();
                this.Grupo.ValorBD = listaInserir[i].Grupo.ToString();
                this.Obs.ValorBD = listaInserir[i].Obs;
                this.PerspectivaLugarID.Valor = listaInserir[i].PerspectivaLugarID;

                base.Inserir();
                listaInserir[i].ID = this.Control.ID; //atualiza o respectivo ID.


                #region controle da barra de progresso
                if ((controlePorcentagem > 5 && controlePorcentagem <= 10) && porcentagemAtual <= 10)
                {
                    this.atualizarBarraDeProgresso(10);
                    porcentagemAtual = 20;
                }
                if ((controlePorcentagem > 10 && controlePorcentagem <= 20) && porcentagemAtual <= 20)
                {
                    this.atualizarBarraDeProgresso(20);
                    porcentagemAtual = 30;
                }
                if ((controlePorcentagem > 20 && controlePorcentagem <= 30) && porcentagemAtual <= 30)
                {
                    this.atualizarBarraDeProgresso(30);
                    porcentagemAtual = 40;
                }
                if ((controlePorcentagem > 30 && controlePorcentagem <= 40) && porcentagemAtual <= 40)
                {
                    this.atualizarBarraDeProgresso(40);
                    porcentagemAtual = 50;
                }
                if ((controlePorcentagem > 40 && controlePorcentagem <= 50) && porcentagemAtual <= 50)
                {
                    this.atualizarBarraDeProgresso(50);
                    porcentagemAtual = 60;
                }
                if ((controlePorcentagem > 50 && controlePorcentagem <= 60) && porcentagemAtual <= 60)
                {
                    this.atualizarBarraDeProgresso(60);
                    porcentagemAtual = 70;
                }
                if ((controlePorcentagem > 60 && controlePorcentagem <= 70) && porcentagemAtual <= 70)
                {
                    this.atualizarBarraDeProgresso(70);
                    porcentagemAtual = 80;
                }
                if ((controlePorcentagem > 70 && controlePorcentagem <= 80) && porcentagemAtual <= 80)
                {
                    this.atualizarBarraDeProgresso(80);
                    porcentagemAtual = 90;
                }
                if ((controlePorcentagem > 80 && controlePorcentagem <= 90) && porcentagemAtual <= 90)
                {
                    this.atualizarBarraDeProgresso(90);
                    porcentagemAtual = 100;
                }
                if ((controlePorcentagem > 90 && controlePorcentagem <= 100) && porcentagemAtual <= 100)
                {
                    this.atualizarBarraDeProgresso(100);
                }
                #endregion

            }
            return listaInserir;
        }

        /// <summary>
        /// Método usado para retornar a lista de melhores lugares juntos.
        /// Apesar de estar na classe Lugar o acesso ao banco acessando a tabela ingresso.
        /// Na Classe ingresso existe um método de mesmo nome que retorna os melhores ingresso (tambem buscados através desse método)
        /// </summary>
        /// <param name="quantidade">Quantidade de lugares juntos</param>
        /// <param name="apresentacaoSetorID"></param>
        /// <returns></returns>
        public List<Lugar> MelhorLugarMarcado(int quantidade, int apresentacaoSetorID, Setor.enumLugarMarcado setorTipo)
        {

            try
            {
                List<Lugar> lugares = new List<Lugar>();
                int[] aux;
                int lugarID;
                int classificacao;
                Lugar lugar = new Lugar();

                Apresentacao oAp = new Apresentacao();
                if (!oAp.PossuiClassificacao(apresentacaoSetorID))
                {
                    throw new Exception("Atenção, nenhum ingresso foi reservado. O Setor selecionado não possui agrupamento e classificação e por este motivo não será possível efetuar a reserva dos melhores lugares.");
                }

                ArrayList gruposTemp;
                switch (setorTipo)
                {
                    case Setor.enumLugarMarcado.MesaAberta:
                        #region Melhores Lugares - Mesa Aberta

                        string sqlMesaAberta = "SELECT " +
                            "Grupo,LugarID " +
                            "FROM tIngresso (NOLOCK)  " +
                            "WHERE  " +
                            "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                            "Grupo > 0 AND  " +
                            "Classificacao > 0  " +
                            "AND Status = 'D' " +
                            "ORDER BY Grupo, Classificacao";

                        bd.Consulta(sqlMesaAberta);
                        ArrayList gruposMesaAberta = new ArrayList();
                        while (bd.Consulta().Read())
                            gruposMesaAberta.Add(new int[] { bd.LerInt("Grupo"), bd.LerInt("LugarID") });
                        bd.FecharConsulta();


                        /// Vai do primeiro ao último grupo.

                        bool primeiroIngresso = true;
                        int classificacaoAnteriorMesaAberta = -1;
                        int classificacaoAtualMesaAberta;
                        int grupoMesaAberta;
                        int lugaresQuantidadeTotal = 0; //Contagem de quantos lugares já foram selecionados.

                        for (int i = 0; i < gruposMesaAberta.Count && lugaresQuantidadeTotal < quantidade; i++)
                        {
                            aux = (int[])gruposMesaAberta[i];
                            grupoMesaAberta = aux[0];
                            lugarID = aux[1];

                            // Busca os lugares do grupo em questão.
                            bd.Consulta("SELECT ID, Codigo, Classificacao, LugarID FROM tIngresso (NOLOCK) " +
                                "WHERE " +
                                "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                                "Grupo = " + grupoMesaAberta + " AND LugarID = " + lugarID + " AND " +
                                "Classificacao > 0 " +
                                "AND Status = '" + Ingresso.DISPONIVEL + "' " +
                                "ORDER BY Classificacao");

                            primeiroIngresso = true;
                            //variável para controle. nescessárias para saber se mudou ou não de mesa.
                            int lugarIDAnterior = 0;
                            if (bd.Consulta().Read())
                            {
                                do//(do/while)
                                {

                                    /// Verifica se a classificação do item anterior + 1 é diferente da atual.
                                    /// _____
                                    /// 1
                                    /// 2
                                    /// 3
                                    /// -------
                                    /// 3 + 1 != da classificacao atual?!
                                    ///		Classificação Atual = 4 = OK
                                    ///		Classificação Atual = 5 = ERRADO.
                                    ///
                                    classificacaoAtualMesaAberta = bd.LerInt("Classificacao");
                                    if (primeiroIngresso)
                                    {
                                        if (lugaresQuantidadeTotal > 0 && classificacaoAnteriorMesaAberta != -1 && classificacaoAnteriorMesaAberta + 1 != classificacaoAtualMesaAberta)
                                            lugares.Clear();
                                    }
                                    if (lugarIDAnterior != lugarID)
                                    {
                                        lugares.Clear();
                                        //preenche o objeto de retorno com os lugares.
                                        lugar = new Lugar();
                                        lugar.Control.ID = lugarID;
                                        lugar.Grupo.Valor = grupoMesaAberta;
                                        lugar.Classificacao.Valor = classificacaoAtualMesaAberta;
                                        lugar.Quantidade.Valor = 1;
                                        lugaresQuantidadeTotal = 1;
                                        //lugaresQuantidadeTotal++;
                                        lugares.Add(lugar);
                                    }
                                    else//se ainda não mudou de lugar não deve inserir um novo. Deve-se atualizar a quantidade.
                                    {
                                        lugar.Quantidade.Valor++;//só incrementa a quantidade.
                                        lugaresQuantidadeTotal++;//incrementa a cotagem geral de lugares.
                                    }
                                    lugarIDAnterior = lugarID;
                                    classificacaoAnteriorMesaAberta = classificacaoAtualMesaAberta; //atualiza a classificação anterior
                                    primeiroIngresso = false;
                                }
                                while (bd.Consulta().Read() && lugaresQuantidadeTotal < quantidade);
                            }
                        }
                        bd.FecharConsulta();

                        #endregion
                        break;

                    case Setor.enumLugarMarcado.MesaFechada:
                        #region Melhores Lugares - Mesa Fechada

                        string sqlMesaFechada = "SELECT " +
                            "DISTINCT  " +
                            "Grupo, Classificacao,LugarID " +
                            "FROM tIngresso (NOLOCK)  " +
                            "WHERE " +
                            "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                            "Grupo > 0 AND " +
                            "Classificacao > 0  " +
                            "AND Status = 'D' " +
                            "ORDER BY Grupo, Classificacao";

                        bd.Consulta(sqlMesaFechada);
                        ArrayList grupos = new ArrayList();
                        gruposTemp = new ArrayList();

                        int grupoAtual = 1;
                        int grupoAnterior = 1;

                        while (bd.Consulta().Read())
                        {
                            grupoAtual = bd.LerInt("Grupo");
                            if (gruposTemp.Count > 0 && grupoAnterior != grupoAtual && gruposTemp.Count >= quantidade)
                            {
                                grupos.AddRange(gruposTemp);
                                gruposTemp.Clear();
                            }

                            if (grupoAtual != grupoAnterior)
                                gruposTemp.Clear();

                            gruposTemp.Add(new int[] { grupoAtual, bd.LerInt("Classificacao"), bd.LerInt("LugarID") });
                            grupoAnterior = grupoAtual;
                        }

                        bd.FecharConsulta();

                        if (gruposTemp.Count > 0 && gruposTemp.Count >= quantidade)
                        {
                            grupos.AddRange(gruposTemp);
                            gruposTemp.Clear();
                        }
                        /// Vai do primeiro ao último grupo.

                        int grupoMesaFechada;
                        for (int i = 0; i < grupos.Count && lugares.Count < quantidade; i++)
                        {
                            aux = (int[])grupos[i];
                            grupoMesaFechada = aux[0];
                            classificacao = aux[1];
                            lugarID = aux[2];

                            /// Verifica se a classificação do item anterior + 1 é diferente da atual.
                            /// _____
                            /// 1
                            /// 2
                            /// 3
                            /// -------
                            /// 3 + 1 != da classificacao atual?!
                            ///		Classificação Atual = 4 = OK
                            ///		Classificação Atual = 5 = ERRADO.
                            ///

                            if (lugares.Count > 0 && ((Lugar)lugares[lugares.Count - 1]).Classificacao.Valor + 1 != classificacao)
                                lugares.Clear();

                            lugar = new Lugar();
                            lugar.Control.ID = lugarID;
                            lugar.Grupo.Valor = grupoMesaFechada;
                            lugar.Classificacao.Valor = classificacao;
                            lugares.Add(lugar);
                        }
                        #endregion
                        break;

                    case Setor.enumLugarMarcado.Cadeira:
                        #region Melhores Lugares - Cadeira

                        string sql = "SELECT " +
                            "Grupo, COUNT(ID) Quantidade " +
                            "FROM tIngresso (NOLOCK) " +
                            "WHERE  " +
                            "ApresentacaoSetorID = " + apresentacaoSetorID + " AND  " +
                            "Grupo > 0 AND  " +
                            "Classificacao > 0 " +
                            "AND Status = 'D' " +
                            "GROUP BY Grupo " +
                            "HAVING COUNT(ID) >=  " + quantidade +
                            " ORDER BY Grupo, Quantidade";

                        //SqlDataReader dr = bd.ConsultaDR(sql);

                        ArrayList gruposCadeira = bd.ConsultaDR(sql);

                        //if (!dr.Read())
                        //    throw new Exception("Não existem " + quantidade + " ingressos juntos para esse setor. Tente reservar os ingressos separadamente");

                        //while (dr.Read())
                        //    gruposCadeira.Add(dr["Grupo"]);
                        //bd.FecharConsulta();
                        //bd.Fechar();
                        //dr.Close();

                        ArrayList ingressos = new ArrayList();

                        /// Vai do primeiro ao último grupo.
                        int grupo;
                        int classificacaoAnterior = -1;
                        int classificacaoAtual;
                        for (int i = 0; i < gruposCadeira.Count && ingressos.Count < quantidade; i++)
                        {
                            grupo = (int)gruposCadeira[i];
                            // Busca os lugares do grupo em questão.
                            bd.Consulta("SELECT ID, Codigo, Classificacao, LugarID FROM tIngresso (NOLOCK) " +
                                "WHERE " +
                                "ApresentacaoSetorID = " + apresentacaoSetorID + " AND " +
                                "Grupo = " + grupo + " AND " +
                                "Classificacao > 0 " +
                                "AND Status = '" + Ingresso.DISPONIVEL + "' " +
                                "ORDER BY Classificacao");

                            while (bd.Consulta().Read() && lugares.Count < quantidade)
                            {

                                /// Verifica se a classificação do item anterior + 1 é diferente da atual.
                                /// _____
                                /// 1
                                /// 2
                                /// 3
                                /// -------
                                /// 3 + 1 != da classificacao atual?!
                                ///		Classificação Atual = 4 = OK
                                ///		Classificação Atual = 5 = ERRADO.
                                ///
                                classificacaoAtual = bd.LerInt("Classificacao");
                                if (lugares.Count > 0 && classificacaoAnterior != -1 && classificacaoAnterior + 1 != classificacaoAtual)
                                    lugares.Clear();

                                classificacaoAnterior = classificacaoAtual;
                                //preenche o objeto de retorno com os lugares.
                                lugar = new Lugar();
                                lugar.Control.ID = bd.LerInt("LugarID");
                                lugar.Grupo.Valor = grupo;
                                lugar.Classificacao.Valor = classificacaoAtual;
                                lugares.Add(lugar);
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                return lugares;
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
        /// Função usada para salvar lugares (atualizando) na tela MapaDesenhar
        /// </summary>
        /// <returns></returns>
        public void SalvarLugaresAtualizar(List<IRLib.Paralela.ClientObjects.Lugar> listaAtualizar)
        {
            for (int i = 0; i < listaAtualizar.Count; i++)
            {
                this.Control.ID = listaAtualizar[i].ID;
                this.SetorID.ValorBD = listaAtualizar[i].SetorID.ToString();
                this.Codigo.ValorBD = listaAtualizar[i].Codigo;
                this.Quantidade.ValorBD = listaAtualizar[i].Quantidade.ToString();
                this.QuantidadeBloqueada.ValorBD = listaAtualizar[i].QuantidadeBloqueada.ToString();
                this.PosicaoX.ValorBD = listaAtualizar[i].PosicaoX.ToString();
                this.PosicaoY.ValorBD = listaAtualizar[i].PosicaoY.ToString();
                this.Simbolo.ValorBD = listaAtualizar[i].Simbolo.ToString();
                this.BloqueioID.ValorBD = listaAtualizar[i].BloqueioID.ToString();
                this.Classificacao.ValorBD = listaAtualizar[i].Classificacao.ToString();
                this.Grupo.ValorBD = listaAtualizar[i].Grupo.ToString();
                this.Obs.ValorBD = listaAtualizar[i].Obs;
                this.PerspectivaLugarID.Valor = listaAtualizar[i].PerspectivaLugarID;
                base.Atualizar();


                #region controle da barra de progresso
                if ((controlePorcentagem > 5 && controlePorcentagem <= 10) && porcentagemAtual <= 10)
                {
                    this.atualizarBarraDeProgresso(10);
                    porcentagemAtual = 20;
                }
                if ((controlePorcentagem > 10 && controlePorcentagem <= 20) && porcentagemAtual <= 20)
                {
                    this.atualizarBarraDeProgresso(20);
                    porcentagemAtual = 30;
                }
                if ((controlePorcentagem > 20 && controlePorcentagem <= 30) && porcentagemAtual <= 30)
                {
                    this.atualizarBarraDeProgresso(30);
                    porcentagemAtual = 40;
                }
                if ((controlePorcentagem > 30 && controlePorcentagem <= 40) && porcentagemAtual <= 40)
                {
                    this.atualizarBarraDeProgresso(40);
                    porcentagemAtual = 50;
                }
                if ((controlePorcentagem > 40 && controlePorcentagem <= 50) && porcentagemAtual <= 50)
                {
                    this.atualizarBarraDeProgresso(50);
                    porcentagemAtual = 60;
                }
                if ((controlePorcentagem > 50 && controlePorcentagem <= 60) && porcentagemAtual <= 60)
                {
                    this.atualizarBarraDeProgresso(60);
                    porcentagemAtual = 70;
                }
                if ((controlePorcentagem > 60 && controlePorcentagem <= 70) && porcentagemAtual <= 70)
                {
                    this.atualizarBarraDeProgresso(70);
                    porcentagemAtual = 80;
                }
                if ((controlePorcentagem > 70 && controlePorcentagem <= 80) && porcentagemAtual <= 80)
                {
                    this.atualizarBarraDeProgresso(80);
                    porcentagemAtual = 90;
                }
                if ((controlePorcentagem > 80 && controlePorcentagem <= 90) && porcentagemAtual <= 90)
                {
                    this.atualizarBarraDeProgresso(90);
                    porcentagemAtual = 100;
                }
                if ((controlePorcentagem > 90 && controlePorcentagem <= 100) && porcentagemAtual <= 100)
                {
                    this.atualizarBarraDeProgresso(100);
                }
                #endregion
            }
        }

        public IRLib.Paralela.ClientObjects.Lugar[] SalvarMapa(IRLib.Paralela.ClientObjects.Lugar[] alteracoes, List<string> lstExcluidosID)
        {
            var bdLugares = new BD();

            try
            {
                bdLugares.IniciarTransacao();

                IRLib.Paralela.ClientObjects.Lugar[] lugares = this.SalvarLugares(alteracoes, bdLugares);

                if (lstExcluidosID.Count > 0)
                    this.ExcluirLugares(lstExcluidosID, bdLugares);

                return lugares;

            }
            catch
            {
                bdLugares.DesfazerTransacao();
                throw;
            }
            finally
            {
                bdLugares.FinalizarTransacao();
                bdLugares.Fechar();
            }

        }
        private void ExcluirLugares(List<string> listaExcluir, BD bdLugares)
        {
            try
            {
                var sql = String.Format("DELETE FROM tLugar where ID IN ({0})", String.Join(",", listaExcluir.ToArray()).ToString());

                bdLugares.Executar(sql.ToString());
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        private IRLib.Paralela.ClientObjects.Lugar[] SalvarLugares(IRLib.Paralela.ClientObjects.Lugar[] listaInserir, BD bdLugares)
        {
            StringBuilder sql;
            foreach (var lugar in listaInserir)
            {
                lugar.Codigo = lugar.Codigo.Replace("'", "''");
                switch (lugar.Acao)
                {
                    case IRLib.Paralela.ClientObjects.Lugar.TiposAcao.Inserir:

                        sql = new StringBuilder();
                        sql.Append(" INSERT INTO tLugar(SetorID, Codigo, Quantidade, QuantidadeBloqueada, PosicaoX, PosicaoY, Simbolo, BloqueioID, Classificacao, Grupo, Obs, PerspectivaLugarID) ");
                        sql.Append(" VALUES (@001,'@002',@003,@004,@005,@006,@007,@008,@009,@010,'@011',@012)");

                        sql.Replace("@001", lugar.SetorID.ToString());
                        sql.Replace("@002", lugar.Codigo);
                        sql.Replace("@003", lugar.Quantidade.ToString());
                        sql.Replace("@004", lugar.QuantidadeBloqueada.ToString());
                        sql.Replace("@005", lugar.PosicaoX.ToString());
                        sql.Replace("@006", lugar.PosicaoY.ToString());
                        sql.Replace("@007", lugar.Simbolo.ToString());
                        sql.Replace("@008", lugar.BloqueioID.ToString());
                        sql.Replace("@009", lugar.Classificacao.ToString());
                        sql.Replace("@010", lugar.Grupo.ToString());
                        sql.Replace("@011", lugar.Obs);
                        sql.Replace("@012", lugar.PerspectivaLugarID.ToString());

                        bdLugares.Executar(sql.ToString());

                        break;
                    case IRLib.Paralela.ClientObjects.Lugar.TiposAcao.Atualizar:

                        sql = new StringBuilder();
                        sql.Append(" UPDATE tLugar SET SetorID = @001 , Codigo = '@002' , Quantidade = @003 , QuantidadeBloqueada = @004 , PosicaoX = @005 , PosicaoY = @006 , ");
                        sql.Append(" Simbolo = '@007' , BloqueioID = @008 , Classificacao = @009 , Grupo = @010 , Obs = '@011' , PerspectivaLugarID = @012");
                        sql.Append(" WHERE ID = @ID ");

                        sql.Replace("@ID", lugar.ID.ToString());
                        sql.Replace("@001", lugar.SetorID.ToString());
                        sql.Replace("@002", lugar.Codigo);
                        sql.Replace("@003", lugar.Quantidade.ToString());
                        sql.Replace("@004", lugar.QuantidadeBloqueada.ToString());
                        sql.Replace("@005", lugar.PosicaoX.ToString());
                        sql.Replace("@006", lugar.PosicaoY.ToString());
                        sql.Replace("@007", lugar.Simbolo.ToString());
                        sql.Replace("@008", lugar.BloqueioID.ToString());
                        sql.Replace("@009", lugar.Classificacao.ToString());
                        sql.Replace("@010", lugar.Grupo.ToString());
                        sql.Replace("@011", lugar.Obs);
                        sql.Replace("@012", lugar.PerspectivaLugarID.ToString());

                        bdLugares.Executar(sql.ToString());

                        break;
                }
            }

            return null;
        }
        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
      

    }

    public class LugarLista : LugarLista_B
    {

        public LugarLista() { }

        public LugarLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {
            try
            {
                Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = true;

                if (lista.Count > 0)
                { //verifica se tem itens

                    // Verificar de tem ingresso indisponivel
                    IngressoLista ingressoLista = new IngressoLista();
                    ingressoLista.FiltroSQL = "LugarID in (" + this.ToString() + ")";
                    ingressoLista.FiltroSQL = "Status<>'" + Ingresso.DISPONIVEL + "' AND Status<>'" + Ingresso.BLOQUEADO + "'";
                    ingressoLista.Carregar();
                    if (ingressoLista.Tamanho > 0)
                        throw new LugarException("Não pode excluir esses lugares porque há " + ingressoLista.Tamanho + " ingressos não-disponíveis em todos ou em algum deles.");

                    ingressoLista.FiltroSQL = null;
                    ingressoLista.FiltroSQL = "LugarID in (" + this.ToString() + ")";
                    ingressoLista.Carregar();

                    if (ingressoLista.Tamanho > 0)
                    {
                        IngressoLogLista ingressoLogLista = new IngressoLogLista();
                        ingressoLogLista.FiltroSQL = "IngressoID in (" + ingressoLista + ")";
                        ingressoLogLista.FiltroSQL = "VendaBilheteriaItemID <> 0";
                        ingressoLogLista.Carregar(1);
                        if (ingressoLogLista.Tamanho > 0)
                        {
                            throw new LugarException("Não pode excluir esses lugares porque há ingressos com venda efetuada.");
                        }
                        else
                        {
                            ingressoLogLista.FiltroSQL = null;
                            ingressoLogLista.FiltroSQL = "IngressoID in (" + ingressoLista + ")";
                            ok = ingressoLogLista.ExcluirTudo();
                        }
                    }

                    if (ok)
                    {
                        Ultimo();
                        //fazer varredura de traz pra frente.
                        do
                            ok = base.Excluir();
                        while (ok && Anterior());
                    }

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }

}
