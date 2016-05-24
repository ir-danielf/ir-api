using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public static class LocalExtensions
    {
        public static Local toLocal(this tLocal tLocal)
        {
            return new Local()
                                {
                                BannersPadraoSite = null,
                                CEP = tLocal.CEP,
                                Cidade = tLocal.Cidade,
                                CodigoPraca = tLocal.CodigoPraca,
                                ComoChegar = tLocal.ComoChegar,
                                DDDTelefone = tLocal.DDDTelefone,
                                Distancia = 0,
                                EmpresaID = tLocal.EmpresaID,
                                Endereco = tLocal.Endereco,
                                Estado = tLocal.Estado,
                                Evento = new List<Evento>(),
                                ID = tLocal.ID,
                                Imagem = tLocal.ImagemInternet,
                                IR_LocalID = tLocal.ID,
                                Latitude = tLocal.Latitude,
                                Longitude = tLocal.Longitude,
                                Nome = tLocal.Nome,
                                Obs = tLocal.Obs,
                                Pais = string.Empty,
                                TaxaMaximaEmpresa = 0,
                                Telefone = tLocal.Telefone
                            };
        }
    }
}
