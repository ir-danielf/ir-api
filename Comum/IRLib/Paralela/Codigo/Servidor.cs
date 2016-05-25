using CTLib;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela {

	/// <summary>
	/// Obtem recursos do servidor.
	/// </summary>
    [ObjectType (ObjectType.RemotingType.SingleCall)]
	public class Servidor : MarshalByRefObject,ISponsoredObject  {

		/// <summary>
		/// Retorna o dia e a hora atuais.
		/// </summary>
		public DateTime Agora{
			get{ return System.DateTime.Now; }
		}

		/// <summary>
		/// Retorna o dia atual.
		/// </summary>
		public DateTime Hoje{
			get { return System.DateTime.Today; }
		}
        /// <summary>
        /// Retorna a lista de arquivos necessários para o funcionamento do TEF
        /// </summary>
        /// <returns></returns>
        public List<string> ArquivosTefParaBaixar()
        {
            try
            {
                string arquivoResource = Template.ArquivosBaixarTEF.ToString().Replace(Environment.NewLine, "");
                string[] arrayArquivos = arquivoResource.Split(',');
                
                List<string> retorno = new List<string>();

                foreach (string item in arrayArquivos)
                {
                    retorno.Add(item);
                }
                return retorno;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }


	}

}
