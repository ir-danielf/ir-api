using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Dapper;

namespace IRCore.DataAccess.ADO
{
    public class LojaADO : MasterADO<dbIngresso>
    {
        public LojaADO(MasterADOBase ado = null) : base(ado) { }

        public tLoja Consultar(int id)
        {
            var queryStr = @"Select Top 1
                         l.ID, l.EstoqueID, l.CanalID, l.Nome, l.Endereco, l.Cidade, l.Estado, l.CEP, l.DDDTelefone, l.Telefone, l.Email, l.Obs, l.TEF, l.NroEstabelecimento, l.ComprovanteQuantidade, l.TEFTipo, l.numeroPOS, l.UsuarioPosID, l.TravaImpressao
                    FROM tLoja (nolock) l
                WHERE l.ID = @id";

            var query = conIngresso.Query<tLoja>(queryStr, new { id = id });
            return query.FirstOrDefault();
        }
    }
}