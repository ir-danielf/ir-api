using CTLib;
using System;
using System.Configuration;
using System.IO;
namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleTonSpecialEvent)]
    public class SingleTonObjects : MarshalByRefObject
    {
        //Permite que o objeto Single ton não expire
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region Registro estatico
        /// <summary>
        /// Metodo responsavel pelo registro da classe single ton no Cliente
        /// Por ser estatico deve possuir as informacoes de Porta e IP no Cliente
        /// </summary>
        /// <returns></returns>
        public static SingleTonObjects RegistrarSpecialEvent()
        {

            string versao = ConfigurationManager.AppSettings["ConfigVersion"];


            if (!Configuracao.GetBoolean(Configuracao.Keys.SingleTonAtivo, versao))
                return new SingleTonObjects(true);


            string ip = Configuracao.GetString(Configuracao.Keys.IPRemoting, versao);
            if (String.IsNullOrEmpty(ip))
                throw new Exception("Valor da Chave não encontrada " + Configuracao.Keys.IPRemoting);

            int porta = Configuracao.GetInt(Configuracao.Keys.PortaSpecialEvent, versao);
            if (porta == 0)
                throw new Exception("Valor da Chave não encontrada " + Configuracao.Keys.PortaSpecialEvent);


            string file = ConfigurationManager.AppSettings["EnderecoArquivoIP"];
            if (File.Exists(file))
                ip = File.ReadAllText(file);

            var objectName = string.Format("tcp://{0}:{1}/", ip, porta) + typeof(IRLib.SingleTonObjects).Name;

            
            var objeto = Activator.GetObject(typeof(IRLib.SingleTonObjects), objectName);
            return (IRLib.SingleTonObjects)objeto;
                
        }

        #endregion

        public BufferMapaEsquematico bufferMapaEsquematico { get; set; }
        public BufferSpecialEvent bufferSpecialEvent { get; set; }
        public BufferMensagens bufferMensagens { get; set; }
        public BufferSpecialEventGerenciamento bufferGerenciamento { get; set; }



        public SingleTonObjects(bool local)
        {
            if (this.bufferMapaEsquematico == null)
            {
                this.bufferMapaEsquematico = new BufferMapaEsquematico();
                if (local)
                    this.bufferMapaEsquematico.Carregar();
            }
            if (this.bufferSpecialEvent == null)
                this.bufferSpecialEvent = new BufferSpecialEvent();

            if (this.bufferMensagens == null)
            {
                this.bufferMensagens = new BufferMensagens();
                if (local)
                    this.bufferMensagens.Carregar();
            }
            if (this.bufferGerenciamento == null)
            {
                this.bufferGerenciamento = new BufferSpecialEventGerenciamento();
            }
        }

        public SingleTonObjects()
        {
            if (this.bufferMapaEsquematico == null)
                this.bufferMapaEsquematico = new BufferMapaEsquematico();

            if (this.bufferSpecialEvent == null)
                this.bufferSpecialEvent = new BufferSpecialEvent();

            if (this.bufferMensagens == null)
                this.bufferMensagens = new BufferMensagens();

            if (this.bufferGerenciamento == null)
                this.bufferGerenciamento = new BufferSpecialEventGerenciamento();
        }
    }
}
