using CTLib;
using System;
using System.IO;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class GerenciadorUpload : MarshalByRefObject
    {
        /// <summary>
        /// Método que efetua o Upload para uma pasta no Servidor via Remoting
        /// </summary>
        /// <param name="Arquivo">Array de bytes - Arquivo</param>
        /// <param name="NomeArquivo">Nome que o arquivo será salvo no computador   </param>
        /// <param name="Extensao">extensao do arquivo</param>
        /// <param name="PastaDestino">pasta do servidor que recebera o arquivo</param>
        public void Upload(byte[] Arquivo, string NomeArquivo, string Extensao, string PastaDestino)
        {
           

            if (Extensao == ".exe" || Extensao == ".scr" || Extensao == ".msi")
                throw new Exception("Ooops, Extensão inválida");

            MemoryStream mStream = null;
            FileStream fStream = null;
            try
            {
                mStream = new MemoryStream(Arquivo);

                fStream = new FileStream(PastaDestino + "/" + NomeArquivo + Extensao, FileMode.Create, FileAccess.Write);

                mStream.WriteTo(fStream);

                mStream.Close();
                fStream.Close();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mStream.Close();
                fStream.Close();
            }
        }



    }
}
