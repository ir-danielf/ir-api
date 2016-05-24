/**************************************************
* Arquivo: TaxaEntregaEventoPadrao.cs
* Gerado: 11/11/2008
* Autor: Celeritas Ltda
***************************************************/


namespace IRLib.Paralela
{

    public class TaxaEntregaEventoPadrao : TaxaEntregaEventoPadrao_B
    {

        public TaxaEntregaEventoPadrao() { }

        public TaxaEntregaEventoPadrao(int usuarioIDLogado) : base() { }

        //public List<int> GetTaxasPadrao()
        //{
        //    List<int> lstTaxas = new List<int>();

        //    try
        //    {
        //        using (IDataReader oDataReader = bd.Consulta("" + 
        //            "SELECT " + 
        //                "tTaxaEntregaEventoPadrao.TaxaEntregaID " + 
        //            "FROM " + 
        //            "   tTaxaEntregaEventoPadrao (NOLOCK)"))
        //        {
        //            while (oDataReader.Read())
        //            {
        //                lstTaxas.Add(bd.LerInt("TaxaEntregaID"));
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //        finally 
        //    {
        //        bd.Fechar();
        //    }

        //    return lstTaxas;
        //}


    }

    public class TaxaEntregaEventoPadraoLista : TaxaEntregaEventoPadraoLista_B
    {

        public TaxaEntregaEventoPadraoLista() { }

        public TaxaEntregaEventoPadraoLista(int usuarioIDLogado) : base() { }

    }

}
