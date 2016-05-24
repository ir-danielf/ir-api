using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRLib
{
    [Serializable]
    public class BufferSpecialEventGerenciamento : MarshalByRefObject, ISponsoredObject
    {
        #region Properties // Objects

        public bool AlreadyLoadedGerenciamento { get; set; }
        public bool IsLoadingGerenciamento { get; set; }

        List<Gerenciamento.Data> Apresentacoes = new List<Gerenciamento.Data>();
        List<Gerenciamento.Precos> Precos = new List<Gerenciamento.Precos>();

        public DateTime LastRefreshGerenciamento { get; set; }

        public string MensagemRetorno { get; set; }

        const string URL = "http://www.ingressorapido.com.br/ImagensSistema/Eventos/";
        const string Extensao = ".gif";

        #endregion

        public BufferSpecialEventGerenciamento()
        {
            if (IsLoadingGerenciamento && !AlreadyLoadedGerenciamento)
            {
                MensagemRetorno = "Por favor, aguarde um momento. \n A estrutura de Eventos Gerenciados já está sendo carregada por outra pessoa";
                return;
            }

            if (AlreadyLoadedGerenciamento)
                return;

            this.IsLoadingGerenciamento = true;
            this.Load();
            this.AlreadyLoadedGerenciamento = true;
            this.IsLoadingGerenciamento = false;
        }

        /// <summary>
        /// Carrega o buffer de Eventos Gerenciados
        /// A PROC deve possuir o ID dos eventos do contrario não será carregado no objeto
        /// </summary>
        public void Load()
        {
            BD bd = new BD();
            int ApresentacaoID = 0;

            try
            {
                lock (this)
                {
                    this.Apresentacoes.Clear();


                    bd.Consulta("EXEC sp_GerenciamentoLoadingData");
                    while (bd.Consulta().Read())
                    {

                        ApresentacaoID = bd.LerInt("ApresentacaoID");

                        if (Apresentacoes.Where(c => c.ApresentacaoID == ApresentacaoID).Count() == 0)
                            Apresentacoes.Add(new Gerenciamento.Data
                            {
                                ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID"),
                                ApresentacaoID = ApresentacaoID,
                                DataApresentacao = bd.LerDateTime("ApresentacaoHorario"),
                                EventoID = bd.LerInt("EventoID"),
                                EventoNome = bd.LerString("EventoNome"),
                                SetorID = bd.LerInt("SetorID")
                            });

                        Precos.Add(new Gerenciamento.Precos
                        {
                            ApresentacaoID = ApresentacaoID,
                            Evento = bd.LerString("EventoNome"),
                            EventoID = bd.LerInt("EventoID"),
                            DataApresentacao = bd.LerDateTime("ApresentacaoHorario"),
                            Horario = bd.LerString("HorarioPreco"),
                            Label = bd.LerString("Label"),
                            Nome = bd.LerString("PrecoNome"),
                            SetorID = bd.LerInt("SetorID"),
                            PrecoID = bd.LerInt("PrecoID"),
                            PrecoTipoID = bd.LerInt("PrecoTipoID"),
                            Valor = bd.LerDecimal("Valor"),
                            GerenciamentoIngressoID = bd.LerInt("GerenciamentoIngressosID"),
                            QuantidadeDisponivel = bd.LerInt("QuantidadeDisponivel"),
                            QuantidadeMaxima = bd.LerInt("QuantidadeMaxima")
                        });

                        Apresentacoes.Where(c => c.ApresentacaoID == ApresentacaoID).FirstOrDefault().Precos = Precos.Where(c => c.ApresentacaoID == ApresentacaoID).ToList();


                    }
                }
                this.LastRefreshGerenciamento = DateTime.Now;
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
        /// Retorna os Precos apartir da Data ( ApresentacaoID)
        /// Não deve retornar o IEnumerable diretamente pois o IRBilheteria não está em 3.5!!
        /// </summary>
        /// <param name="tipoID"></param>
        /// <returns></returns>
        public List<Gerenciamento.Precos> getPrecosApresentacaoMaisProxima()
        {
            try
            {
                var items = (from a in Apresentacoes
                             join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                             where a.DataApresentacao > DateTime.Now
                             select new
                             {
                                 PrecoID = p.PrecoID,
                                 Valor = p.Valor,
                                 SetorID = p.SetorID,
                                 PrecoTipoID = p.PrecoTipoID,
                                 Nome = p.Nome,
                                 Label = p.Label,
                                 Horario = p.Horario,
                                 ApresentacaoID = p.ApresentacaoID,
                                 GerenciamentoIngressoID = p.GerenciamentoIngressoID,
                                 QuantidadeDisponivel = p.QuantidadeDisponivel,
                                 QuantidadeMaxima = p.QuantidadeMaxima,
                                 EventoID = p.EventoID,
                                 DataApresentacao = p.DataApresentacao,
                                 Evento = p.Evento

                             }).Distinct().ToList();



                List<Gerenciamento.Precos> retorno = new List<Gerenciamento.Precos>();
                foreach (var item in items)
                {
                    retorno.Add(new Gerenciamento.Precos()
                    {
                        PrecoID = item.PrecoID,
                        SetorID = item.SetorID,
                        Valor = item.Valor,
                        PrecoTipoID = item.PrecoTipoID,
                        Nome = item.Nome,
                        Label = item.Label,
                        Horario = item.Horario,
                        ApresentacaoID = item.ApresentacaoID,
                        GerenciamentoIngressoID = item.GerenciamentoIngressoID,
                        QuantidadeDisponivel = item.QuantidadeDisponivel,
                        QuantidadeMaxima = item.QuantidadeMaxima,
                        EventoID = item.EventoID,
                        DataApresentacao = item.DataApresentacao,
                        Evento = item.Evento

                    });
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Gerenciamento.Precos> getPrecosApresentacaoMaisProxima(int EventoID)
        {
            try
            {
                var items = (from a in Apresentacoes
                             join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                             where a.DataApresentacao > DateTime.Now && a.EventoID == EventoID
                             select new
                             {
                                 PrecoID = p.PrecoID,
                                 Valor = p.Valor,
                                 SetorID = p.SetorID,
                                 PrecoTipoID = p.PrecoTipoID,
                                 Nome = p.Nome,
                                 Label = p.Label,
                                 Horario = p.Horario,
                                 ApresentacaoID = p.ApresentacaoID,
                                 GerenciamentoIngressoID = p.GerenciamentoIngressoID,
                                 QuantidadeDisponivel = p.QuantidadeDisponivel,
                                 QuantidadeMaxima = p.QuantidadeMaxima,
                                 EventoID = p.EventoID,
                                 DataApresentacao = p.DataApresentacao,
                                 Evento = p.Evento

                             }).Distinct().ToList();



                List<Gerenciamento.Precos> retorno = new List<Gerenciamento.Precos>();
                foreach (var item in items)
                {
                    retorno.Add(new Gerenciamento.Precos()
                    {
                        PrecoID = item.PrecoID,
                        SetorID = item.SetorID,
                        Valor = item.Valor,
                        PrecoTipoID = item.PrecoTipoID,
                        Nome = item.Nome,
                        Label = item.Label,
                        Horario = item.Horario,
                        ApresentacaoID = item.ApresentacaoID,
                        GerenciamentoIngressoID = item.GerenciamentoIngressoID,
                        QuantidadeDisponivel = item.QuantidadeDisponivel,
                        QuantidadeMaxima = item.QuantidadeMaxima,
                        EventoID = item.EventoID,
                        DataApresentacao = item.DataApresentacao,
                        Evento = item.Evento

                    });
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna os Precos apartir da Data ( ApresentacaoID)
        /// Não deve retornar o IEnumerable diretamente pois o IRBilheteria não está em 3.5!!
        /// </summary>
        /// <param name="tipoID"></param>
        /// <returns></returns>
        public List<Gerenciamento.Precos> getPrecosDataApresentacao(DateTime dataApresentacao, int eventoID)
        {
            try
            {
                var items = (from a in Apresentacoes
                             join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                             where a.DataApresentacao.Date.Equals(dataApresentacao) && a.EventoID == eventoID
                             select new
                             {
                                 PrecoID = p.PrecoID,
                                 Valor = p.Valor,
                                 SetorID = p.SetorID,
                                 PrecoTipoID = p.PrecoTipoID,
                                 Nome = p.Nome,
                                 Label = p.Label,
                                 Horario = p.Horario,
                                 ApresentacaoID = p.ApresentacaoID,
                                 GerenciamentoIngressoID = p.GerenciamentoIngressoID,
                                 QuantidadeDisponivel = p.QuantidadeDisponivel,
                                 QuantidadeMaxima = p.QuantidadeMaxima,
                                 EventoID = p.EventoID,
                                 DataApresentacao = p.DataApresentacao,
                                 Evento = p.Evento
                             }).Distinct().ToList();

                List<Gerenciamento.Precos> retorno = new List<Gerenciamento.Precos>();
                foreach (var item in items)
                {
                    retorno.Add(new Gerenciamento.Precos()
                    {
                        PrecoID = item.PrecoID,
                        Valor = item.Valor,
                        SetorID = item.SetorID,
                        PrecoTipoID = item.PrecoTipoID,
                        Nome = item.Nome,
                        Label = item.Label,
                        Horario = item.Horario,
                        ApresentacaoID = item.ApresentacaoID,
                        GerenciamentoIngressoID = item.GerenciamentoIngressoID,
                        QuantidadeDisponivel = item.QuantidadeDisponivel,
                        QuantidadeMaxima = item.QuantidadeMaxima,
                        EventoID = item.EventoID,
                        DataApresentacao = item.DataApresentacao,
                        Evento = item.Evento
                    });
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoID">EventoID</param>
        /// <param name="ApresentacaoID">ApresentacaoID</param>
        /// <param name="SetorID">SetorID</param>
        /// <param name="valor">PrecoValor</param>
        /// <param name="data">Data(yyyymmddhhmmss)</param>
        /// <param name="GerenciamentoIngressoID">GerenciamentoIngressoID</param>
        /// <param name="holder">Useless</param>
        /// <returns></returns>
        public Gerenciamento.RetornoReserva Reservar(int EventoID, int ApresentacaoID, int SetorID, int PrecoID, decimal valor, DateTime data, int GerenciamentoIngressoID, bool holder)
        {
            try
            {
                Gerenciamento.RetornoReserva reserva = new Gerenciamento.RetornoReserva();
                var item = (from a in Apresentacoes
                            join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                            where a.ApresentacaoID == ApresentacaoID &&
                            p.PrecoID == PrecoID &&
                            p.GerenciamentoIngressoID == GerenciamentoIngressoID &&
                            p.Valor == valor &&
                            a.EventoID == EventoID
                            select new { a.ApresentacaoID, a.ApresentacaoSetorID, p.PrecoID, p.GerenciamentoIngressoID }).FirstOrDefault();

                if (item != null)
                {
                    reserva.ApresentacaoID = item.ApresentacaoID;
                    reserva.ApresentacaoSetorID = item.ApresentacaoSetorID;
                    reserva.EventoID = EventoID;
                    reserva.PrecoID = item.PrecoID;
                    reserva.SetorID = SetorID;
                    reserva.GerenciamentoIngressoID = item.GerenciamentoIngressoID;
                    reserva.Reservado = true;
                }
                else
                    reserva.Reservado = false;

                return reserva;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo de decrementar,
        /// deve dar Lock na aplicação para através de concorrencia não seja possível reservar o PrecoID ao mesmo tempo com qtd <= 0
        /// TODO: Dar LOCK
        /// </summary>
        /// <param name="precoID"></param>
        /// <returns></returns>
        public bool DecrementarPrecoID(int precoID, int gerenciamentoIngressoID)
        {
            try
            {
                lock (this.Precos)
                {
                    Gerenciamento.Precos preco = this.Precos.Where(c => c.PrecoID == precoID && c.GerenciamentoIngressoID == gerenciamentoIngressoID).FirstOrDefault();

                    if (preco == null || preco.QuantidadeDisponivel <= 0)
                        return false;

                    preco.QuantidadeDisponivel--;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Reservar(int precoID, int gerenciamentoIngressoID)
        {
            return this.DecrementarPrecoID(precoID, gerenciamentoIngressoID);
        }

        public void LiberarReserva(List<int> PrecosID)
        {
            for (int i = 0; i < PrecosID.Count; i++)
                this.LiberarReserva(PrecosID[i]);
        }

        /// <summary>
        /// Metodo que libera a reserva apartir do PrecoID
        /// TODO:DEVE LOCKAR A Aplicação
        /// </summary>
        /// <param name="PrecoID"></param>
        public void LiberarReserva(int PrecoID)
        {
            try
            {
                lock (this.Precos)
                {
                    Gerenciamento.Precos preco = this.Precos.Where(c => c.PrecoID == PrecoID && c.QuantidadeDisponivel + 1 <= c.QuantidadeMaxima).FirstOrDefault();
                    if (preco != null)
                        preco.QuantidadeDisponivel++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime getDataApresentacaoMaisProxima(int eventoID)
        {
            try
            {
                DateTime dt = new DateTime();
                dt = (from a in Apresentacoes
                      where a.DataApresentacao.Date >= DateTime.Now.Date && a.EventoID == eventoID
                      select new
                      {
                          a.DataApresentacao
                      }).First().DataApresentacao.Date;

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TipoGerenciado(int eventoID)
        {
            try
            {

                if ((from a in Apresentacoes
                     where a.EventoID == eventoID
                     select new
                     {
                         a.EventoID
                     }).Count() > 0)
                {
                    return true;
                }

                return false;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool verificaFreePass(int precoID, int apresentacaoID)
        {
            if ((from a in Apresentacoes
                 join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                 where a.ApresentacaoID == apresentacaoID && p.PrecoID == precoID && p.PrecoTipoID == IRLib.GerenciamentoIngressos.PRECOFREEPASS
                 select new
                 {
                     p.PrecoID
                 }).Count() > 0)
            {
                return true;
            }

            return false;
        }

        public bool verificaNormal(int precoID, int apresentacaoID)
        {
            if ((from a in Apresentacoes
                 join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                 where a.ApresentacaoID == apresentacaoID && p.PrecoID == precoID && p.PrecoTipoID == IRLib.GerenciamentoIngressos.PRECONORMAL
                 select new
                 {
                     p.PrecoID
                 }).Count() > 0)
            {
                return true;
            }

            return false;
        }

        public bool verificaMarcada(int precoID, int apresentacaoID)
        {
            if ((from a in Apresentacoes
                 join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                 where a.ApresentacaoID == apresentacaoID && p.PrecoID == precoID && p.PrecoTipoID == IRLib.GerenciamentoIngressos.PRECOHORAMARCADA
                 select new
                 {
                     p.PrecoID
                 }).Count() > 0)
            {
                return true;
            }

            return false;
        }

        public Gerenciamento.Precos getPreco(int precoID, int apresentacaoID)
        {
            Gerenciamento.Precos preco = new Gerenciamento.Precos();

            var item = (from a in Apresentacoes
                        join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                        where a.ApresentacaoID == apresentacaoID && p.PrecoID == precoID
                        select new
                        {
                            PrecoID = p.PrecoID,
                            Valor = p.Valor,
                            SetorID = p.SetorID,
                            PrecoTipoID = p.PrecoTipoID,
                            Nome = p.Nome,
                            Label = p.Label,
                            Horario = p.Horario,
                            ApresentacaoID = p.ApresentacaoID,
                            GerenciamentoIngressoID = p.GerenciamentoIngressoID,
                            QuantidadeDisponivel = p.QuantidadeDisponivel,
                            QuantidadeMaxima = p.QuantidadeMaxima,
                            EventoID = p.EventoID,
                            DataApresentacao = p.DataApresentacao,
                            Evento = p.Evento
                        }).FirstOrDefault();

            if (item != null)
                preco = new Gerenciamento.Precos()
                    {
                        PrecoID = item.PrecoID,
                        Valor = item.Valor,
                        SetorID = item.SetorID,
                        PrecoTipoID = item.PrecoTipoID,
                        Nome = item.Nome,
                        Label = item.Label,
                        Horario = item.Horario,
                        ApresentacaoID = item.ApresentacaoID,
                        GerenciamentoIngressoID = item.GerenciamentoIngressoID,
                        QuantidadeDisponivel = item.QuantidadeDisponivel,
                        QuantidadeMaxima = item.QuantidadeMaxima,
                        EventoID = item.EventoID,
                        DataApresentacao = item.DataApresentacao,
                        Evento = item.Evento
                    };


            return preco;
        }

        public object getHorarios(int precoID, int apresentacaoID)
        {
            var horarios = (from a in Apresentacoes
                            join p in Precos on a.ApresentacaoID equals p.ApresentacaoID
                            where a.ApresentacaoID == apresentacaoID && p.PrecoID == precoID && p.PrecoTipoID == GerenciamentoIngressos.PRECOHORAMARCADA
                            select new
                            {
                                GerenciamentoIngressosID = p.GerenciamentoIngressoID,
                                Horario = p.Horario,
                            }).Distinct().ToList();


            StringBuilder stb = new StringBuilder();
            stb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", 0, "Selecione");
            foreach (var item in horarios)
            {
                stb.AppendFormat("<option value='{0}'>{1}</option>", item.GerenciamentoIngressosID, item.Horario.Length > 0 ? item.Horario.Substring(0, 2) + ":" + item.Horario.Substring(2, 2) : "");
            }


            return stb.ToString();
        }


    }
}
