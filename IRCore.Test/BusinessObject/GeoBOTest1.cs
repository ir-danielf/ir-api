using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.BusinessObject;

namespace IRCore.Test.BusinessObject
{
    [TestClass]
    public class GeoBOTest1
    {

        GeoBO geoBO = new GeoBO();

        [TestMethod]
        public void GeoBO_ListaEstado()
        {
            //teste do método ListarEstado()
            var geo = geoBO.ListarEstado();
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Sigla));
                Assert.AreEqual("RS", geo[22].Sigla);
            }
        }

        [TestMethod]
        public void GeoBO_ListaCidade()
        {
            //teste do método ListarCidade(string uf)
            var geo = geoBO.ListarCidade("RS");
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Nome));
            }
        }

        [TestMethod]
        public void GeoBO_ListaPais()
        {
            //teste do método ListarPais()
            var geo = geoBO.ListarPais();
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Nome));
                Assert.AreEqual("Brasil", geo[0].Nome);
            }
        }
    }
}
