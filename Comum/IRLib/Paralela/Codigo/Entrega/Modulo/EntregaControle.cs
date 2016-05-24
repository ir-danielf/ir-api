using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRLib.Paralela
{
    public enum EnumEntregaControleStatus
    {
        Novo,
        Normal,
        Editado,
        Excluido
    }

    public enum EnumSimNao
    {
        Sim,
        Nao
    }

    public class EntregaControle : EntregaControle_B
    {

        public EntregaControle() { }

        public EntregaControle(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void Inserir(EstruturaEntregaControle estruturaEntregaControle)
        {
            this.AtribuirEstrutura(estruturaEntregaControle);
            this.Inserir();
            foreach (int dia in estruturaEntregaControle.ListaDiasDaSemana)
            {
                DiasSemana dS = new DiasSemana();
                dS.DiaDaSemana.Valor = dia;
                dS.ControleEntregaID.Valor = this.Control.ID;
                dS.Inserir();
            }
        }

        public void Atualizar(EstruturaEntregaControle estruturaEntregaControle)
        {
            this.AtribuirEstrutura(estruturaEntregaControle);
            this.Atualizar();

        }

        public void AtribuirEstrutura(EstruturaEntregaControle estruturaEntregaControle)
        {
            this.EntregaAreaID.Valor = estruturaEntregaControle.EntregaAreaID;
            this.DiasTriagem.Valor = estruturaEntregaControle.DiasTriagem;
            this.EntregaID.Valor = estruturaEntregaControle.EntregaID;
            this.Control.ID = estruturaEntregaControle.ID;
            this.PeriodoID.Valor = estruturaEntregaControle.PeriodoID;
            this.ProcedimentoEntrega.Valor = estruturaEntregaControle.ProcedimentoEntrega;
            this.QuantidadeEntregas.Valor = estruturaEntregaControle.QuantidadeEntregas;
            this.Valor.Valor = estruturaEntregaControle.Valor;

        }

        public void Atualizar(List<EstruturaEntregaControle> listaEntregaControle)
        {
            foreach (EstruturaEntregaControle ecAux in listaEntregaControle)
            {
                this.Atualizar(ecAux);
            }
        }

        public void Inserir(List<EstruturaEntregaControle> listaEntregaControle, int EntregaID)
        {
            foreach (EstruturaEntregaControle ecAux in listaEntregaControle)
            {
                ecAux.EntregaID = EntregaID;
                this.Inserir(ecAux);
            }
        }

        public void Excluir(List<EstruturaEntregaControle> listaEntregaControle)
        {
            foreach (EstruturaEntregaControle ecAux in listaEntregaControle)
            {
                if (ecAux.ID > 0)
                {
                    this.Excluir(ecAux.ID);
                }
            }
        }

        /// <summary>
        /// Exclui EntregaControle com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlVersion = "SELECT MAX(Versao) FROM cEntregaControle WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tEntregaControle SET Ativa='F' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

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

        public List<EstruturaEntregaControle> ListarEntregaID(int entregaID)
        {

            return Listar(entregaID, 0, 0);
        }

        public List<EstruturaEntregaControle> Listar(int entregaID, int entregaAreaID, int periodoID)
        {

            try
            {
                EntregaArea oEntregaArea = new EntregaArea();
                EntregaPeriodo oEntregaPeriodo = new EntregaPeriodo();
                DiasSemana oDiasSemana = new DiasSemana();

                List<EstruturaEntregaControle> lista = new List<EstruturaEntregaControle>();
                string filtro = "";
                if (entregaID > 0)
                {
                    filtro += " AND EntregaID= " + entregaID;
                }
                if (entregaAreaID > 0)
                {

                    filtro += " AND EntregaAreaID= " + entregaAreaID;
                }
                if (periodoID > 0)
                {

                    filtro += " AND PeriodoID= " + periodoID;
                }

                string sql = @"SELECT ID, 
                            EntregaID, 
                            EntregaAreaID, 
                            PeriodoID, 
                            QuantidadeEntregas, 
                            Valor, 
                            DiasTriagem, 
                            ProcedimentoEntrega 
                            FROM tEntregaControle (nolock)
                            WHERE Ativa = 'T' " + filtro;


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    int controleID = bd.LerInt("ID");
                    int areaID = bd.LerInt("EntregaAreaID");
                    string nomeArea = oEntregaArea.LerNome(areaID);

                    int periodoIDConsulta = bd.LerInt("periodoID");
                    string nomePeriodo = oEntregaPeriodo.LerNome(periodoIDConsulta);

                    int diasTriagem = bd.LerInt("DiasTriagem");

                    string procedimento = bd.LerString("ProcedimentoEntrega");

                    EstruturaEntregaControleDias dias = new EstruturaEntregaControleDias();
                    dias = oDiasSemana.Listar(controleID);

                    lista.Add(new EstruturaEntregaControle
                    {
                        ID = controleID,
                        EntregaID = bd.LerInt("EntregaID"),
                        EntregaAreaID = areaID,
                        NomeArea = nomeArea,
                        UsarDiasTriagemPadrao = diasTriagem > 0 ? "Não" : "Sim",
                        DiasTriagem = diasTriagem,
                        PeriodoID = periodoIDConsulta,
                        NomePeriodo = nomePeriodo,
                        Manter = "Sim",
                        UsarProcedimentoEntregaPadrao = procedimento.Length > 0 ? "Não" : "Sim",
                        ProcedimentoEntrega = procedimento,
                        QuantidadeEntregas = bd.LerInt("QuantidadeEntregas"),
                        Valor = bd.LerDecimal("Valor"),
                        DiasDaSemana = dias.DiasDaSemana,
                        ListaDiasDaSemana = dias.ListaDiasDaSemana
                    });

                }

                return lista;

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

        public List<EstruturaEntregaControle> CarregarEventoEntrega(int eventoID, int entregaID, int entregaAreaID, int periodoID, bool disponiveis)
        {

            try
            {
                EntregaArea oEntregaArea = new EntregaArea();
                EntregaPeriodo oEntregaPeriodo = new EntregaPeriodo();
                Entrega oEntrega = new Entrega();

                List<EstruturaEntregaControle> lista = new List<EstruturaEntregaControle>();

                string filtroAux = " WHERE tEntrega.Disponivel = 'T' AND tEntregaControle.Ativa = 'T' ";
                if (entregaID > 0)
                {
                    filtroAux += " AND tEntregaControle.EntregaID= " + entregaID;
                }
                if (entregaAreaID > 0)
                {
                    filtroAux += " AND tEntregaControle.EntregaAreaID= " + entregaAreaID;
                }
                if (periodoID > 0)
                {
                    filtroAux += " AND tEntregaControle.PeriodoID= " + periodoID;
                }
                if (disponiveis)
                {
                    filtroAux += " AND EventoID is not NULL ";
                }

                string sql = @"SELECT tEntregaControle.ID, tEntregaControle.EntregaID, tEntregaControle.EntregaAreaID, tEntregaControle.PeriodoID, 
                            tEntregaControle.QuantidadeEntregas, tEntregaControle.Valor, tEntregaControle.ProcedimentoEntrega as ProcedimentoArea, 
                            tEntrega.ProcedimentoEntrega as ProcedimentoTaxa, tEventoEntregaControle.ProcedimentoEntrega as ProcedimentoEvento,
                            tEventoEntregaControle.DiasTriagem, EventoID 
                            FROM tEntregaControle 
                            LEFT JOIN tEntrega ON tEntrega.ID = tEntregaControle.EntregaID 
                            LEFT JOIN tEventoEntregaControle ON tEventoEntregaControle.EntregaControleID = tEntregaControle.ID AND EventoID = "
                    + eventoID
                   + filtroAux
                   + " ORDER BY EntregaID, EntregaAreaID, PeriodoID";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    int areaID = bd.LerInt("EntregaAreaID");
                    string nomeArea = oEntregaArea.LerNome(areaID);

                    int periodoIDConsulta = bd.LerInt("periodoID");
                    string nomePeriodo = oEntregaPeriodo.LerNome(periodoIDConsulta);

                    int entregaIDConsulta = bd.LerInt("entregaID");
                    string nomeEntrega = oEntrega.LerNome(entregaIDConsulta);

                    int diasTriagem = bd.LerInt("DiasTriagem");

                    bool disponivel = (bd.LerInt("EventoID") != 0);

                    string procedimentoEvento = bd.LerString("ProcedimentoEvento");
                    string procedimentoTaxa = bd.LerString("ProcedimentoTaxa");
                    string procedimentoArea = bd.LerString("ProcedimentoArea");

                    string procedimento = string.Empty;

                    if (procedimentoEvento.Length > 0)
                        procedimento = procedimentoEvento;
                    else if (procedimentoArea.Length > 0)
                        procedimento = procedimentoArea;
                    else if (procedimentoTaxa.Length > 0)
                        procedimento = procedimentoTaxa;

                    lista.Add(new EstruturaEntregaControle
                    {
                        ID = bd.LerInt("ID"),
                        EntregaID = entregaIDConsulta,
                        NomeTaxa = nomeEntrega,
                        EntregaAreaID = areaID,
                        NomeArea = nomeArea,
                        UsarDiasTriagemPadrao = diasTriagem > 0 ? "Sim" : "Não",
                        DiasTriagem = diasTriagem,
                        PeriodoID = periodoID,
                        NomePeriodo = nomePeriodo,
                        Manter = disponivel ? "Sim" : "Não",
                        UsarProcedimentoEntregaPadrao = procedimentoEvento.Length > 0 ? "Não" : "Sim",
                        ProcedimentoEntrega = procedimento,
                        QuantidadeEntregas = bd.LerInt("QuantidadeEntregas"),
                        Valor = bd.LerDecimal("Valor"),
                        Disponivel = disponivel
                    });

                }

                return lista;

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

        public List<EstruturaEntrega> Buscar(string nomeEntrega, bool padrao, bool disponivel)
        {

            try
            {


                List<EstruturaEntrega> lista = new List<EstruturaEntrega>();
                string filtro = "";
                if (nomeEntrega.Length > 0)
                {
                    filtro += " WHERE e.nome like '%" + nomeEntrega + "%'";
                }
                if (padrao)
                {
                    if (filtro.Length > 0)
                    {
                        filtro += " AND ";
                    }
                    else
                    {
                        filtro += " WHERE ";
                    }
                    filtro += " e.Padrao= 'T' ";
                }
                if (disponivel)
                {
                    if (filtro.Length > 0)
                    {
                        filtro += " AND ";
                    }
                    else
                    {
                        filtro += " WHERE ";
                    }
                    filtro += " e.Disponivel = 'T' ";
                }

                string sql = string.Format(@"SELECT 
                                e.ID,
                                e.Nome,
                                e.Padrao, 
                                e.Disponivel
                                FROM tEntrega e(nolock) 
                                {0}
                                ORDER BY e.Nome", filtro);
                ;


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaEntrega
                     {
                         Nome = bd.LerString("Nome"),
                         ID = bd.LerInt("ID"),
                         Padrao = bd.LerBoolean("Padrao"),
                         Disponivel = bd.LerBoolean("Disponivel"),

                     });

                }

                return lista;

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

        public bool Existe(int EntregaControleID)
        {
            try
            {
                bool retorno = false;
                string sql = @"SELECT ID FROM tEntregaControle (nolock) WHERE Ativa = 'T' AND ID =" + EntregaControleID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void HabilitarPeriodoAreas(int EventoID, int EntregaID)
        {
            try
            {
                EventoEntregaControle oEEC = new EventoEntregaControle();
                List<int> ListaEntregaControleID = new List<int>();

                string sql = @"SELECT c.ID FROM  tEntregaControle c
                        LEFT JOIN tEventoEntregaControle e ON c.ID = e.EntregaControleID and e.EventoID = " + EventoID + @" 
                        WHERE c.Ativa = 'T' and c.EntregaID = " + EntregaID + " and e.ID is null";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    ListaEntregaControleID.Add(bd.LerInt("ID"));
                }

                bd.Fechar();

                foreach (int EntregaControleID in ListaEntregaControleID)
                {
                    oEEC.Limpar();
                    oEEC.EventoID.Valor = EventoID;
                    oEEC.EntregaControleID.Valor = EntregaControleID;
                    oEEC.ProcedimentoEntrega.Valor = "";
                    oEEC.DiasTriagem.Valor = 0;
                    oEEC.Inserir();
                }
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

        public void DesabilitarPeriodoAreas(int EventoID, int EntregaID)
        {
            try
            {
                EventoEntregaControle oEEC = new EventoEntregaControle();
                List<int> ListaEntregaControleID = new List<int>();
                List<int> ListaEventoEntregaControleID = new List<int>();

                string sql = "SELECT ID FROM tEntregaControle (nolock) WHERE Ativa = 'T' AND tEntregaControle.EntregaID = " + EntregaID;

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    ListaEntregaControleID.Add(bd.LerInt("ID"));
                }

                bd.Fechar();

                foreach (int EntregaControleID in ListaEntregaControleID)
                {
                    string sqlaux = "select ID from tEventoEntregaControle (nolock) where tEventoEntregaControle.EntregaControleID =  " + EntregaControleID + "and tEventoEntregaControle.EventoID = " + EventoID;
                    bd.Consulta(sqlaux);

                    while (bd.Consulta().Read())
                    {
                        ListaEventoEntregaControleID.Add(bd.LerInt("ID"));
                    }

                }

                bd.Fechar();
                foreach (int EventoEntregaControleID in ListaEventoEntregaControleID)
                {
                    oEEC.Excluir(EventoEntregaControleID);
                }

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

        public EntregaControle BuscarPorEnderecoEntrega(string CEP, int entregaID)
        {
            try
            {
                string sql =
                    string.Format(@"            
                        SELECT
	                        TOP 1 ec.ID, ec.Valor
	                        FROM tEntregaControle ec (NOLOCK)
	                        INNER JOIN tEntregaArea ea (NOLOCK) ON ea.ID = ec.EntregaAreaID
	                        INNER JOIN tEntregaAreaCep eac (NOLOCK) ON eac.EntregaAreaID = ea.ID
	                        WHERE ec.EntregaID = {0} AND eac.CepInicial <= '{1}' AND eac.CepFinal >= '{1}'
                    ", entregaID, CEP);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Nenhuma entrega encontrada para o endereço fornecido.");

                this.Control.ID = bd.LerInt("ID");
                this.Valor.Valor = bd.LerDecimal("Valor");

                return this;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Somente retiradabilheteria e retirada pdv!!!!!
        /// </summary>
        /// <param name="entregaID"></param>
        /// <returns></returns>
        public EntregaControle BuscarPorEntregaID(int entregaID)
        {
            try
            {
                string sql = "SELECT TOP 1 ID, Valor FROM tEntregaControle ec (NOLOCK) WHERE ec.EntregaID = " + entregaID;

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existe taxa de entrega do tipo Retirada Bilheteria.");

                this.Limpar();
                this.Control.ID = bd.LerInt("ID");
                this.Valor.Valor = bd.LerDecimal("Valor");

                return this;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class EntregaControleLista : EntregaControleLista_B
    {

        public EntregaControleLista() { }

        public EntregaControleLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}

