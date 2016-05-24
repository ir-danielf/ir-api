using CTLib;
using IRLib.wsPhones;
using IRLib.wsTicket;
using IRLib.Paralela.ClientObjects;
using IRLib.wsPhones;
using IRLib.wsTicket;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.CAO)]
    public class EntregaCelular : MarshalByRefObject, ISponsoredObject
    {
        string Mensagem = ConfigurationManager.AppSettings["EdeployMensagem"].ToString(); //Apresente este codigo na portaria do evento para retirar os seus ingressos
        string UserName = ConfigurationManager.AppSettings["EdeployUsername"].ToString(); //admin
        string PassWord = ConfigurationManager.AppSettings["EdeployPassword"].ToString(); //i4p1d0
        int CompanyID = Convert.ToInt32(ConfigurationManager.AppSettings["EdeployCompanyID"]); //14


        public string SenhaVenda { get; set; }

        TicketSendSystemServiceImplService TicketSend = new TicketSendSystemServiceImplService();
        AdminUser administrador = new AdminUser();
        Phone phoneEntrega = new Phone();
        MessageInfo messageInfo = new MessageInfo();

        private List<EstruturaEntregaCelular> listaEntrega;
        public List<EstruturaEntregaCelular> ListaEntrega
        {
            get { return listaEntrega; }
            set { listaEntrega = value; }
        }
        public string Celular { get; set; }

        /// <summary>
        /// Retorna os fabricantes dos celulares
        /// </summary>
        /// <returns></returns>
        public List<string> getFabricantes()
        {
            try
            {
                PhoneModelServiceImplService phoneModels = new PhoneModelServiceImplService();
                vendorsRequest vendors = new vendorsRequest();
                vendors.companyId = CompanyID;
                vendors.username = UserName;
                vendors.password = PassWord;
                vendors.companyIdSpecified = true;

                List<string> lista = new List<string>();
                lista.Add("Selecione");
                string[] fabricantes = phoneModels.getVendors(vendors);

                lista.AddRange(fabricantes);

                if (lista.Count > 1)
                    return lista;
                else
                    throw new Exception("Nenhum fabricante encontrado");

            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao carregar os fabricantes de celulares. Por favor, tente novamente mais tarde. (" + ex.Message + ")");
            }
        }

        /// <summary>
        /// Retorna os Modelos apartir de um fabricante
        /// </summary>
        /// <param name="fabricante"></param>
        /// <returns></returns>
        public phoneModel[] getModelos(string fabricante)
        {
            try
            {
                PhoneModelServiceImplService phone = new PhoneModelServiceImplService();
                modelsRequest request = new modelsRequest();
                request.companyId = CompanyID;
                request.username = UserName;
                request.password = PassWord;
                request.companyIdSpecified = true;
                request.vendorName = fabricante;
                phoneModel[] phones = phone.getModels(request);

                if (phones.Length > 0)
                    return phones;
                else
                    throw new Exception("Nenhum Modelo Encontrado");

            }
            catch (Exception)
            {
                throw;
            }


        }

        /// <summary>
        /// Adiciona as informações do Administrador
        /// </summary>
        private void gerarAdmin()
        {
            this.administrador.companyId = CompanyID;
            this.administrador.username = UserName;
            this.administrador.password = PassWord;
            this.administrador.idSpecified = true;
        }

        /// <summary>
        /// Adiciona as informações do celular
        /// </summary>
        /// <param name="area"></param>
        /// <param name="country"></param>
        /// <param name="numero"></param>
        private void gerarPhoneInfos(string area, string country, string numero)
        {
            phoneEntrega.areaCode = area; // 11
            phoneEntrega.countryCode = country; // 55
            phoneEntrega.phoneNumber = numero; //00000000
        }

        /// <summary>
        /// Gera as Informações da Mensagem a ser enviada
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="fabricante"></param>
        /// <param name="messageText"></param>
        private void gerarMessageInfos(int modelo, string fabricante)
        {
            try
            {
                string msg = this.SenhaVenda + this.Mensagem;

                messageInfo.modelId = modelo;
                messageInfo.modelIdSpecified = true;
                messageInfo.vendor = fabricante;
                messageInfo.messageTypeSpecified = true;
                messageInfo.code = Guid.NewGuid().ToString().Substring(0, 10);
                messageInfo.text = msg;
                messageInfo.messageType = MessageType.DATAMATRIX;
                messageInfo.messageTypeSpecified = true;
            }
            catch (ApplicationException)
            {
                throw new ApplicationException("Houve um erro de comunicação ao pesquisar os Frabricantes de Celular, Por favor, tente novamente em alguns instantes");
            }
        }

        /// <summary>
        /// Envia os tickets mas antes precisa gerar o Admin, os Phones e as Msgs
        /// </summary>
        /// <returns></returns>
        public bool enviarTickets()
        {
            try
            {
                string ultimoEvento = string.Empty;
                DateTime ultimoHorario = DateTime.Now;

                string[] celInfo = Celular.Split(';');
                this.gerarAdmin();
                this.gerarPhoneInfos(celInfo[0], celInfo[1], celInfo[3]);
                this.gerarMessageInfos(Convert.ToInt32(celInfo[4]), celInfo[2]);

                List<Event> listaEvents = new List<Event>();


                List<EventInfo> listaEventInfos = new List<EventInfo>();

                TicketInfo ticket;
                List<TicketInfo> listaTicketInfo = new List<TicketInfo>();
                Event evento = new Event();
                EventInfo eventInfo;
                for (int i = 0; i < listaEntrega.Count; i++)
                {
                    if (ultimoEvento != listaEntrega[i].Evento || ultimoHorario != listaEntrega[i].Data)
                    {
                        ultimoEvento = listaEntrega[i].Evento;
                        ultimoHorario = listaEntrega[i].Data;
                        if (i != 0)
                        {
                            eventInfo = new EventInfo();
                            eventInfo.@event = evento;
                            eventInfo.ticketInfo = listaTicketInfo.ToArray();
                            listaEventInfos.Add(eventInfo);
                            listaTicketInfo.Clear();
                        }
                        evento = new Event();
                        evento.eventId = listaEntrega[i].EventoID.ToString();
                        evento.name = listaEntrega[i].Evento;
                        listaEvents.Add(evento);
                    }
                    ticket = new TicketInfo();
                    ticket.orderId = listaEntrega[i].Senha.ToString();
                    ticket.ticketId = listaEntrega[i].IngressoID.ToString();
                    ticket.customerName = listaEntrega[i].Nome;
                    ticket.customerDocument = listaEntrega[i].CPF.ToString();
                    ticket.customValues = new string[10];
                    ticket.customValues[0] = listaEntrega[i].Nome;
                    ticket.customValues[1] = listaEntrega[i].Evento;
                    ticket.customValues[2] = listaEntrega[i].Data.ToString();
                    ticket.customValues[3] = listaEntrega[i].Setor;
                    ticket.customValues[4] = listaEntrega[i].PrecoNome;
                    ticket.customValues[5] = listaEntrega[i].Codigo;
                    ticket.customValues[6] = listaEntrega[i].Senha;
                    ticket.customValues[7] = listaEntrega[i].Loja;
                    ticket.customValues[8] = listaEntrega[i].Pagamento;
                    ticket.customValues[9] = listaEntrega[i].Local;

                    listaTicketInfo.Add(ticket);
                }
                eventInfo = new EventInfo();
                eventInfo.@event = evento;
                eventInfo.ticketInfo = listaTicketInfo.ToArray();
                listaEventInfos.Add(eventInfo);

                try
                {
                    TicketSend.sendTicket2(administrador, "Projeto", listaEvents.ToArray(), phoneEntrega, messageInfo, listaEventInfos.ToArray());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int retornarTaxaID()
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["EntregaCelularTaxaID"]);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possivel encontrar a chave contendo a Taxa de Entrega Correspondente a Celular");
            }
        }

    }
}
