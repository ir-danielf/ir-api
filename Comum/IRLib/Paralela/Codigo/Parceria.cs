using CTLib;
using System;
namespace IRLib.Paralela
{
    /// <summary>
    /// Summary description for Itau.
    /// </summary>
    public class Parceria : MarshalByRefObject
    {
        public Parceria()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [Serializable]
        public enum Parceiro 
        {               
            Itau = 1,
            HSBC = 2,
            Vivo = 3,
            Bradesco = 4,
			BinNaoEncontrado = 0

        }        

        public Parceiro ValidaBin(string bin)
        {        
            BD bd = new BD();
            
            try
            {
                if (bin == null || bin.Length != 6 || !IRLib.Paralela.Utilitario.ehInteiro(bin))
                    throw new ApplicationException("BIN Inválido");

                object aux = bd.ConsultaValor("SELECT ParceiroID FROM tBin (NOLOCK) WHERE BIN = '" + bin + "'");
                if (aux != null)
                {
                    
                    int parceiroID = int.Parse(aux.ToString());
                    bd.Fechar();
                    if (parceiroID == 1)
                        return Parceiro.BinNaoEncontrado;
                    else
                        return (Parceiro)parceiroID;
                }
                else
                {
                    bd.Fechar();
					return Parceiro.BinNaoEncontrado;
                    //throw new ApplicationException("BIN Inexistente");
                } 
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }


    }
}