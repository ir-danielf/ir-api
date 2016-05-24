using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.ADO.Models;
using IRCore.DataAccess.Model;
using Dapper;
using System.Data.Entity.Core.Objects;

namespace IRCore.DataAccess.ADO
{
    public class CortesiaADO : MasterADO<dbIngresso>
    {
        public CortesiaADO(MasterADOBase ado = null) : base(ado, true, true) { }

        public bool InsereCCortesia(IRCore.DataAccess.Model.CortesiaModel.CortesiaModelInsercao cmi)
        {

            var result = (conIngresso.Execute(@"insert into cCortesia (ID, Versao, Acao, Timestamp, UsuarioID) values(@id, 0, 'I',@timestamp, @usuarioid)",
                new
                {
                    id = cmi.ID,
                    timestamp = FormataData(DateTime.Now.ToShortDateString()),
                    usuarioid = cmi.UsuarioID
                }) > 0);

            return result;

        }

        public bool Insere(IRCore.DataAccess.Model.CortesiaModel.CortesiaModelInsercao cmi)
        {

            var result = (conIngresso.Execute(@"insert into tCortesia(ID, Nome, LocalID, CorID, Obs, Padrao, ParceiroMidiaID)
               values(@ID, @Nome, @LocalID, @CorID, @Obs, @Padrao, @ParceiroMidiaID)",
                new
                {
                    ID = cmi.ID,
                    Nome = cmi.Nome,
                    LocalID = cmi.LocalID,
                    CorID = cmi.CorID,
                    Obs = cmi.Obs,
                    Padrao = cmi.Padrao,
                    ParceiroMidiaID = cmi.ParceiroMidiaID
                }) > 0);

            return result;

        }

        public int GetID()
        {
            var sql = "select MAX(ID)+1 as ID from cCortesia (NOLOCK )";

            var query = conIngresso.Query<int>(sql);

            return query.First();
        }

        public CortesiaModel BuscaCortesiaByLocalParceiroMidia(int LocalID, int ParceiroMidiaID)
        {

            var queryStr = @"select ID, Nome, LocalID, CorID, Obs, Padrao, ParceiroMidiaID from tCortesia (NOLOCK ) where LocalID = @localId AND ParceiroMidiaID = @parceiroMidiaId";

            var query = conIngresso.Query<CortesiaModel>(queryStr, new
            {
                localId = LocalID,
                parceiroMidiaId = ParceiroMidiaID
            });

            return query.FirstOrDefault();
        }

        public CortesiaModel BuscaCortesiaByID(int id)
        {

            var queryStr = @"select ID, Nome, LocalID, CorID, Obs, Padrao, ParceiroMidiaID from tCortesia (NOLOCK ) where ID = @ID";

            var query = conIngresso.Query<CortesiaModel>(queryStr, new
            {
                ID = id
            });

            return query.FirstOrDefault();
        }
        private string FormataData(string data)
        {
            string retorno = data;
            if (!string.IsNullOrEmpty(data))
            {
                string[] dataArray = data.Split(new char[] { '/' });

                retorno = dataArray[2] + dataArray[1] + dataArray[0];
                retorno = retorno.PadRight(14, '0');
            }

            return retorno;
        }
    }
}
