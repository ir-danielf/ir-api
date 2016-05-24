using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRCore.DataAccess.ADO;

namespace IRCore.Test.DataAccess.ADO
{
    [TestClass]
    public class GeoADOTest1: MasterTest
    {
        private GeoADO geoADO;
        
        public GeoADOTest1() : base(){
            geoADO = new GeoADO(ado);
        }

        [TestMethod]
        public void GeoADO_ListaEstado()
        {
            //teste do método ListarEstado()
            var geo = geoADO.ListarEstado();
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Sigla));
                Assert.AreEqual("RS", geo[22].Sigla);
            }
        }

        [TestMethod]
        public void GeoADO_ListaCidade()
        {
            //teste do método ListarCidade(string uf)
            var geo = geoADO.ListarCidade("RS");
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Nome));
            }
        }

        [TestMethod]
        public void GeoADO_ListaPais()
        {
            //teste do método ListarPais()
            var geo = geoADO.ListarPais();
            Assert.AreNotEqual(0, geo.Count);
            if (geo.Count > 0)
            {
                Assert.AreEqual(false, string.IsNullOrEmpty(geo[0].Nome));
                Assert.AreEqual("Brasil", geo[0].Nome);
            }
        }

        [TestMethod]
        public void GeoADO_BuscarEndereco()
        {
            //teste do método BuscarEndereco(string cep)
            var geo = geoADO.BuscarEndereco("00000000");
            Assert.IsNull(geo);

            geo = geoADO.BuscarEndereco("03274160");
            Assert.AreEqual("Olímpio José Gonçalves", geo.Endereco);
            
        }

        [TestMethod]
        public void GeoBO_ComparaListCidades()
        {
            //teste para comparar antigo e novo listar
            var geo2 = geoADO.ListarCidade("RS");
            var geo = geoADO.ListarCidade("RS");
            Assert.AreEqual(geo2.Count, geo.Count);
        }

        [TestMethod]
        public void GeoBO_ComparaListEstados()
        {
            //teste para comparar antigo e novo listar
            var geo2 = geoADO.ListarEstado();
            var geo = geoADO.ListarEstado();
            Assert.AreEqual(geo2.Count, geo.Count);
        }

        [TestMethod]
        public void GeoBO_ComparaListPaises()
        {
            //teste para comparar antigo e novo listar
            var geo2 = geoADO.ListarPais();
            var geo = geoADO.ListarPais();
            Assert.AreEqual(geo2.Count, geo.Count);
        }

        [TestMethod]
        public void GeoBO_ComparaConsultaCEP()
        {
            //teste para comparar antigo e novo listar
            var geo2 = geoADO.BuscarEndereco("03274160");
            var geo = geoADO.BuscarEndereco("03274160");
            Assert.AreEqual(geo2.ID, geo.ID);
        }
    }
}
