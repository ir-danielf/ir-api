using IRLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace IngressoRapido.Lib
{

    public class Entrega
    {
        public DateTime DataAtual()
        {
            return new ConfigGerenciador().Data();
        }

        public List<DataAgenda> ListarDatas(DateTime dataExibicao, List<int> listaApresentacao, int EntregaID, List<int> listaEventoID, string CEP)
        {
            IRLib.Entrega oEntrega = new IRLib.Entrega();

            return oEntrega.ListarDatas(dataExibicao, listaApresentacao, EntregaID, listaEventoID, CEP);
        }

        public List<EstruturaEntrega> GetByEventoApresentacao(int EventoID, int ApresentacaoID)
        {
            IRLib.Entrega oEntrega = new IRLib.Entrega();
            return oEntrega.GetByEventoApresentacao(EventoID, ApresentacaoID);
        }

        public List<EstruturaEntrega> GetByEventoSomentePacotes(int EventoID)
        {
            IRLib.Entrega oEntrega = new IRLib.Entrega();
            return oEntrega.GetByEventoSomentePacotes(EventoID);
        }

        public void Finalizar(int EnderecoID, int PDVSelecionado, int EntregaControleIDSelecionado, int ClienteID, string SessionID, string DataSelecionada, decimal EntregaValor)
        {
            IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

            oCompraTemporaria.ClienteID.Valor = ClienteID;
            oCompraTemporaria.SessionID.Valor = SessionID;

            EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExiste();

            oCompraTemporaria.EnderecoID.Valor = EnderecoID;
            oCompraTemporaria.EntregaControleIDSelecionado.Valor = EntregaControleIDSelecionado;
            oCompraTemporaria.DataSelecionada.Valor = DataSelecionada;
            oCompraTemporaria.EntregaValor.Valor = EntregaValor;
            oCompraTemporaria.PDVSelecionado.Valor = PDVSelecionado;

            if (oEstrutura.Encontrado)
            {
                oCompraTemporaria.Control.ID = oEstrutura.ID;
                oCompraTemporaria.AtualizarEntrega();
            }
            else
                oCompraTemporaria.Inserir();
        }


        public void FinalizarSessionID(int EnderecoID, int PDVSelecionado, int EntregaControleIDSelecionado, string SessionID, string DataSelecionada, decimal EntregaValor)
        {
            IRLib.CompraTemporaria oCompraTemporaria = new IRLib.CompraTemporaria();

            oCompraTemporaria.SessionID.Valor = SessionID;

            EstruturaCompraTemporaria oEstrutura = oCompraTemporaria.ConsultarSeExisteSessionID();

            oCompraTemporaria.EnderecoID.Valor = EnderecoID;
            oCompraTemporaria.EntregaControleIDSelecionado.Valor = EntregaControleIDSelecionado;
            oCompraTemporaria.DataSelecionada.Valor = DataSelecionada;
            oCompraTemporaria.EntregaValor.Valor = EntregaValor;
            oCompraTemporaria.PDVSelecionado.Valor = PDVSelecionado;

            if (oEstrutura.Encontrado)
            {
                oCompraTemporaria.Control.ID = oEstrutura.ID;
                oCompraTemporaria.AtualizarEntrega();
            }
            else
                oCompraTemporaria.Inserir();
        }


        

        public EstruturaEntrega CarregarEstruturaPeloControleID(int EntregaControleID)
        {
            try
            {
                IRLib.Entrega oEntrega = new IRLib.Entrega();
                return oEntrega.CarregarEstruturaPeloControleID(EntregaControleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<DataAgenda> ListarDatasVIR(DateTime dataExibicao, int EntregaID, string CEP, DateTime MenorValidade)
        {
            try
            {
                IRLib.Entrega oEntrega = new IRLib.Entrega();
                return oEntrega.ListarDatasVIR(dataExibicao, EntregaID, CEP, MenorValidade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificaEntregaSelecionada(int EnderecoID, int PDVSelecionado, int EntregaControleIDSelecionado, string DataSelecionada, decimal EntregaValor)
        {
            decimal valor = this.VerificaValor(EntregaControleIDSelecionado);

            if (valor != EntregaValor)
                return false;

            switch (this.TipoEntrega(EntregaControleIDSelecionado))
            {
                case IRLib.Entrega.AGENDADA:
                    if (EnderecoID <= 0 || PDVSelecionado > 0 || EntregaControleIDSelecionado <= 0 || DataSelecionada.Length <= 0)
                        return false;
                    break;
                case IRLib.Entrega.RETIRADABILHETERIA:
                    if (EnderecoID > 0 || PDVSelecionado > 0 || EntregaControleIDSelecionado <= 0 || DataSelecionada.Length > 0)
                        return false;
                    break;
                case IRLib.Entrega.RETIRADA:
                    if (EnderecoID > 0 || PDVSelecionado < 0 || EntregaControleIDSelecionado <= 0 || DataSelecionada.Length > 0)
                        return false;
                    break;
                case IRLib.Entrega.NORMAL:
                    if (EnderecoID <= 0 || PDVSelecionado > 0 || EntregaControleIDSelecionado <= 0 || DataSelecionada.Length > 0)
                        return false;
                    break;
                default:
                    break;
            }

            if (EnderecoID > 0)
                if (!this.VerificaCep(EntregaControleIDSelecionado, EnderecoID))
                    return false;

            return true;
        }

        private bool VerificaCep(int EntregaControleIDSelecionado, int EnderecoID)
        {
            IRLib.Entrega oEntrega = new IRLib.Entrega();
            return oEntrega.VerificaCep(EntregaControleIDSelecionado, EnderecoID);
        }

        private decimal VerificaValor(int EntregaControleIDSelecionado)
        {
            IRLib.Entrega oEntrega = new IRLib.Entrega();
            return oEntrega.VerificaValor(EntregaControleIDSelecionado);
        }

        private string TipoEntrega(int EntregaControleIDSelecionado)
        {
            try
            {
                IRLib.Entrega oEntrega = new IRLib.Entrega();
                return oEntrega.TipoEntrega(EntregaControleIDSelecionado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EstrutraEntregaSimples> BuscarEntregasMobile(List<int> listaApresentacao, List<int> listaEventos)
        {
            try
            {
                List<EstrutraEntregaSimples> retorno = new List<EstrutraEntregaSimples>();

                int BilheteriaID = ConfigurationManager.AppSettings["IDRetiradaBilheteria"].ToInt32();
                int BilheteWebID = ConfigurationManager.AppSettings["IDBilheteWeb"].ToInt32();

                retorno = new IRLib.Entrega().BuscarEntregasMobile(BilheteriaID, BilheteWebID, listaApresentacao, listaEventos);

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
