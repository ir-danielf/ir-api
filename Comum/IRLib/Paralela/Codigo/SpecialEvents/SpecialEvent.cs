using CTLib;
using System;
using System.Configuration;
using System.IO;
namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.SingleTonSpecialEvent)]
    public class SingleTonObjectsParalela : MarshalByRefObject
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
        public static SingleTonObjectsParalela RegistrarSpecialEvent(string ip, int porta)
        {
            bool singletonAtivo = ConfiguracaoParalela.GetBoolean(ConfiguracaoParalela.Keys.SingleTonAtivo);
            string ipRemoting = ip;

            string file = ConfigurationManager.AppSettings["EnderecoArquivoIP"];
            if (File.Exists(file))
                ipRemoting = File.ReadAllText(file);

            var objectName = string.Format("tcp://{0}:{1}/", ip, porta) + typeof(IRLib.Paralela.SingleTonObjectsParalela).Name;

            if (singletonAtivo)
            {
                var objeto = Activator.GetObject(typeof(IRLib.Paralela.SingleTonObjectsParalela), objectName);
                return (IRLib.Paralela.SingleTonObjectsParalela)objeto;
            }
            else
                return new SingleTonObjectsParalela(true);
        }

        #endregion

        public BufferMapaEsquematico bufferMapaEsquematico { get; set; }
        public BufferSpecialEvent bufferSpecialEvent { get; set; }
        public BufferMensagens bufferMensagens { get; set; }
        public BufferSpecialEventGerenciamento bufferGerenciamento { get; set; }



        public SingleTonObjectsParalela(bool local)
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

        public SingleTonObjectsParalela()
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
