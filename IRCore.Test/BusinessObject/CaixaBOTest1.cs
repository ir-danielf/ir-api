using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class CaixaBOTest1 : MasterTest
    {

        private CaixaBO caixaBO;
        
        public CaixaBOTest1() : base(){
            caixaBO = new CaixaBO(ado);
        }
   
        [TestMethod]
        public void ConsultarCaixaInternet()
        {
            tCaixa caixa = caixaBO.Abrir(21940, new tLoja() { ID = 2 }, new tCanal() { ID = 2, Comissao = 0 });
            caixaBO.Fechar(caixa);
        }
    }
}
