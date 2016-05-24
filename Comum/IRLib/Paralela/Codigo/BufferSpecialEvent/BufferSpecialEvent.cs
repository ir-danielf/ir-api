using CTLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{
    [Serializable]
    public class BufferSpecialEvent : MarshalByRefObject
    {
        #region Properties // Objects

        public bool AlreadyLoadedQRX { get; set; }
        public bool IsLoadingQRX { get; set; }
        List<QRX.Tipo> tipos = new List<QRX.Tipo>();
        List<QRX.DataHora> horarios = new List<QRX.DataHora>();
        List<QRX.Montadora> montadoras = new List<QRX.Montadora>();
        List<QRX.Carro> carros = new List<QRX.Carro>();

        public DateTime LastRefreshQRX { get; set; }

        public string MensagemRetorno { get; set; }

        const string URL = "http://www.ingressorapido.com.br/ImagensSistema/Eventos/";
        const string Extensao = ".gif";

        #endregion

        public BufferSpecialEvent()
        {
            if (IsLoadingQRX && !AlreadyLoadedQRX)
            {
                MensagemRetorno = "Por favor, aguarde um momento. \n A estrutura do Evento QRX já está sendo carregada por outra pessoa";
                return;
            }

            if (AlreadyLoadedQRX)
                return;

            this.IsLoadingQRX = true;
            this.Load();
            this.AlreadyLoadedQRX = true;
            this.IsLoadingQRX = false;
        }

        /// <summary>
        /// Carrega o buffer do QRX
        /// A PROC deve possuir o ID dos eventos do contrario não será carregado no objeto
        /// </summary>
        public void Load()
        {
            BD bd = new BD();
            int EventoID = 0;
            int ApresentacaoID = 0;
            int ApresentacaoSetorID = 0;
            int SetorID = 0;

            try
            {
                lock (this)
                {
                    this.tipos.Clear();
                    this.montadoras.Clear();
                    this.carros.Clear();
                    this.horarios.Clear();

                    bd.Consulta("EXEC sp_QRXLoadingData");
                    while (bd.Consulta().Read())
                    {
                        EventoID = bd.LerInt("EventoID");
                        if (tipos.Where(c => c.ID == EventoID).Count() == 0)
                            tipos.Add(new QRX.Tipo { ID = EventoID, Nome = bd.LerString("Evento") });

                        ApresentacaoID = bd.LerInt("ApresentacaoID");
                        if (horarios.Where(c => c.ID == ApresentacaoID).Count() == 0)
                            horarios.Add(new QRX.DataHora { ID = ApresentacaoID, Horario = bd.LerDateTime("Horario"), EventoID = EventoID });

                        ApresentacaoSetorID = bd.LerInt("ApresentacaoSetorID");
                        SetorID = bd.LerInt("SetorID");
                        montadoras.Add(new QRX.Montadora
                        {
                            ID = ApresentacaoSetorID,
                            SetorID = SetorID,
                            SetorNome = bd.LerString("Setor"),
                            ApresentacaoID = ApresentacaoID,
                            Apresentacao = horarios.Where(c => c.ID == ApresentacaoID).FirstOrDefault(),
                            Imagem = URL + bd.LerString("Setor") + Extensao,
                        });

                        horarios.Where(c => c.ID == ApresentacaoID).FirstOrDefault().Setores = montadoras.Where(c => c.ApresentacaoID == ApresentacaoID).ToList();


                        carros.Add(new QRX.Carro()
                        {
                            ID = bd.LerInt("PrecoID"),
                            ApresentacaoSetorID = ApresentacaoSetorID,
                            Nome = bd.LerString("PrecoNome"),
                            Valor = bd.LerDecimal("Valor"),
                            ApresentacaoSetor = montadoras.Where(c => c.ID == ApresentacaoSetorID).FirstOrDefault(),
                            QuantidadeDisponivel = bd.LerInt("QuantidadeDisponivel"),
                            QuantidadeMaxima = bd.LerInt("QuantidadeMaxima")
                        });

                        montadoras.Where(c => c.ID == ApresentacaoSetorID).FirstOrDefault().Precos = carros.Where(c => c.ApresentacaoSetorID == ApresentacaoSetorID).ToList();

                        tipos.Where(c => c.ID == EventoID).FirstOrDefault().Apresentacoes = horarios;
                    }
                }
                this.LastRefreshQRX = DateTime.Now;
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
        /// Retorna as montadoras apartir do TipoID ( EventoID)
        /// Não deve retornar o IEnumerable diretamente pois o IRBilheteria não está em 3.5!!
        /// </summary>
        /// <param name="tipoID"></param>
        /// <returns></returns>
        public List<QRX.Montadora> getMontadorasPorTipo(int tipoID)
        {
            try
            {
                var items = (from c in montadoras
                             where c.Apresentacao.EventoID.Equals(tipoID)
                             select new { SetorID = c.SetorID, SetorNome = c.SetorNome, c.Imagem }).Distinct().ToList();

                List<QRX.Montadora> retorno = new List<QRX.Montadora>();
                foreach (var item in items)
                {
                    retorno.Add(new QRX.Montadora()
                    {
                        Imagem = item.Imagem,
                        SetorNome = item.SetorNome,
                        SetorID = item.SetorID
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
        /// Metodo que só retorna as informações simples da Montadora selecionada, 
        /// Só é utilizado no Site
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public QRX.Montadora getMontadoraInfo(int ID)
        {
            try
            {
                QRX.Montadora retorno = this.montadoras.Where(c => c.SetorID == ID).FirstOrDefault();
                if (retorno == null)
                    throw new Exception("A Montadora selecionada retornou um erro.");

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna os Carros (Precos) da montadora selecionada
        /// deve passar o TipoID(EventoID) também do contrario a seleção trará carros de outros tipos
        /// AKA OnRoad - Volkswagen e OffRoad - Volkswagen
        /// </summary>
        /// <param name="tipoID">EventoID</param>
        /// <param name="montadoraID">SetorID</param>
        /// <returns></returns>
        public List<QRX.Carro> getCarrosPorMontadora(int tipoID, int montadoraID)
        {
            try
            {
                var items = (from c in carros
                             where
                             c.ApresentacaoSetor.SetorID.Equals(montadoraID) &&
                             c.ApresentacaoSetor.Apresentacao.EventoID.Equals(tipoID)
                             select new { c.Nome, c.Valor }).Distinct().ToList();

                List<QRX.Carro> retorno = new List<QRX.Carro>();
                foreach (var item in items)
                {
                    retorno.Add(new QRX.Carro
                    {
                        Nome = item.Nome,
                        Valor = item.Valor,

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
        /// Retorna as Datas apartir do Tipo, Montadora e Carro
        /// 
        /// </summary>
        /// <param name="tipoID">EventoID</param>
        /// <param name="montadoraID">SetorID</param>
        /// <param name="carroNome">PrecoNome</param>
        /// <param name="valor">PrecoValor</param>
        /// <returns></returns>
        public List<DateTime> getDataPorCarro(int tipoID, int montadoraID, string carroNome, decimal valor)
        {
            try
            {
                return (from c in carros
                        where
                        c.Nome.Equals(carroNome) && c.Valor.Equals(valor) &&
                        c.ApresentacaoSetor.Apresentacao.EventoID.Equals(tipoID) &&
                        c.ApresentacaoSetor.SetorID.Equals(montadoraID)
                        select c.ApresentacaoSetor.Apresentacao.Horario.Date).OrderBy(c => c).Distinct().ToList();
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
        /// <param name="montadoraID">SetorID</param>
        /// <param name="carroNome">PrecoNome</param>
        /// <param name="valor">PrecoValor</param>
        /// <param name="data">Data(yyyymmdd)</param>
        /// <returns></returns>
        public List<string> getHorariosPorCarro(int tipoID, int montadoraID, string carroNome, decimal valor, DateTime data)
        {
            try
            {
                List<string> horarios = new List<string>();
                foreach (var item in (from c in carros
                                      where
                                      c.ApresentacaoSetor.Apresentacao.EventoID.Equals(tipoID) &&
                                      c.ApresentacaoSetor.SetorID.Equals(montadoraID) &&
                                      c.Nome.Equals(carroNome) && c.Valor.Equals(valor) &&
                                      c.ApresentacaoSetor.Apresentacao.Horario.Day.Equals(data.Day) &&
                                      c.ApresentacaoSetor.Apresentacao.Horario.Year.Equals(data.Year) &&
                                      c.ApresentacaoSetor.Apresentacao.Horario.Month.Equals(data.Month) &&
                                      c.QuantidadeDisponivel > 0
                                      select c.ApresentacaoSetor.Apresentacao.Horario.ToString("HH:mm")).OrderBy(c => c).Distinct().ToList())
                    horarios.Add(item);

                return horarios;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Obsolete("Metodo de reserva antigo, possui falha, está aqui de exemplo para futura verificação", true)]
        public List<int> Reservar(int tipoID, int montadoraID, string carroNome, decimal valor, DateTime data)
        {
            try
            {
                List<int> ret = new List<int>();
                var item = (from t in tipos
                            join m in montadoras on t.ID equals m.Apresentacao.EventoID
                            join c in carros on m.ID equals c.ApresentacaoSetorID
                            where t.ID == tipoID &&
                            c.Nome.Equals(carroNome) &&
                            c.Valor == valor &&
                            c.ApresentacaoSetor.Apresentacao.Horario == data &&
                            c.QuantidadeDisponivel > 0
                            select new { c.ID, m.ApresentacaoID }).FirstOrDefault();


                if (item != null)
                {
                    ret.Add(item.ID);
                    ret.Add(item.ApresentacaoID);
                    if (!this.DecrementarPrecoID(item.ID))
                    {
                        ret[0] = 0;
                        ret[1] = 0;
                    }
                }
                else
                {
                    ret.Add(0);
                    ret.Add(0);
                }


                return ret;
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
        /// <param name="montadoraID">SetorID</param>
        /// <param name="carroNome">PrecoNome</param>
        /// <param name="valor">PrecoValor</param>
        /// <param name="data">Data(yyyymmddhhmmss)</param>
        /// <param name="holder">Useless</param>
        /// <returns></returns>
        public QRX.RetornoReserva Reservar(int tipoID, int montadoraID, string carroNome, decimal valor, DateTime data, bool holder)
        {
            try
            {
                QRX.RetornoReserva reserva = new QRX.RetornoReserva();
                var item = (from t in tipos
                            join m in montadoras on t.ID equals m.Apresentacao.EventoID
                            join c in carros on m.ID equals c.ApresentacaoSetorID
                            where t.ID == tipoID &&
                            c.Nome.Equals(carroNome) &&
                            c.Valor == valor &&
                            c.ApresentacaoSetor.Apresentacao.Horario == data &&
                            c.QuantidadeDisponivel > 0
                            select new { c.ID, c.ApresentacaoSetorID, m.ApresentacaoID, }).FirstOrDefault();

                if (item != null)
                {
                    reserva.ApresentacaoID = item.ApresentacaoID;
                    reserva.ApresentacaoSetorID = item.ApresentacaoSetorID;
                    reserva.EventoID = tipoID;
                    reserva.PrecoID = item.ID;
                    reserva.SetorID = montadoraID;
                    reserva.Reservado = true;
                    if (!this.DecrementarPrecoID(item.ID))
                        throw new Exception("Não foi possível reservar o preço selecionado");
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
        public bool DecrementarPrecoID(int precoID)
        {
            try
            {
                lock (this.carros)
                {
                    QRX.Carro carro = this.carros.Where(c => c.ID == precoID).FirstOrDefault();

                    if (carro == null || carro.QuantidadeDisponivel <= 0)
                        return false;

                    carro.QuantidadeDisponivel--;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Reservar(int precoID)
        {
            return this.DecrementarPrecoID(precoID);
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
                lock (this.carros)
                {
                    QRX.Carro carro = this.carros.Where(c => c.ID == PrecoID && c.QuantidadeDisponivel + 1 <= c.QuantidadeMaxima).FirstOrDefault();
                    if (carro != null)
                        carro.QuantidadeDisponivel++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
