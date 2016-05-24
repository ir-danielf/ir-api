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
    public static class SetorExtensions
    {
        public static Setor toSetor(this tSetor tSetor)
        {
            return new Setor()
            {
                Apresentacao = null,
                ApresentacaoID = 0,
                AprovadoPublicacao = tSetor.AprovadoPublicacao == "T"?true:false,
                CodigoSala = tSetor.CodigoSala,
                ID = tSetor.ID,
                IR_SetorID = tSetor.ID,
                LugarMarcado = tSetor.LugarMarcado,
                Nome = tSetor.Nome,
                NVendeLugar = null,
                Obs = tSetor.ObservacaoImportante,
                Preco = null,
                PrecoParceiroMidia = null,
                PrincipalPrecoID = null,
                QtdeDisponivel = null,
                QuantidadeMapa = null
            };
        }
    }
}
