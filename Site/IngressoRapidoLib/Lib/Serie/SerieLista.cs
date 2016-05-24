using CTLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
namespace IngressoRapido.Lib
{
    public class SerieLista : List<Serie>
    {
        private DAL oDal { get; set; }
        public SerieLista() { }
        public int QuantidadePorPagina { get; set; }
        public int QuantidadeDePaginas { get; set; }

        public string CaminhoImagem = ConfigurationManager.AppSettings["PathSerie"];

        public void CarregarLista(string clausula, int QtdPorPagina)
        {
            oDal = new DAL();
            try
            {
                this.QuantidadePorPagina = QtdPorPagina;

                string sql = @"SELECT DISTINCT Serie.IR_SerieID AS ID, Serie.Titulo, Serie.Nome, Serie.Descricao,
                                       QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao, Regras, Tipo
                            FROM Serie (NOLOCK) ";

                if (!string.IsNullOrEmpty(clausula))
                {
                    clausula.Replace("'", "''");
                    sql += "WHERE Nome LIKE '%" + clausula + "%' OR Titulo LIKE '%" + clausula + "%'";
                }
                int c = 0;
                int pagAtual = 1;

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        pagAtual = (c % QtdPorPagina == 0 && c > 0 ? pagAtual + 1 : pagAtual);
                        c++;

                        this.Add(new Serie()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Titulo = dr["Titulo"].ToString(),
                            Nome = dr["Nome"].ToString(),
                            Descricao = dr["Descricao"].ToString(),
                            QuantidadeMaximaApresentacao = Convert.ToInt32(dr["QuantidadeMaximaApresentacao"]),
                            QuantidadeMinimaApresentacao = Convert.ToInt32(dr["QuantidadeMinimaApresentacao"]),
                            Regras = dr["Regras"].ToString(),
                            Pagina = pagAtual,
                            Tipo = dr["Tipo"].ToString()
                        });
                    }
                    this.QuantidadeDePaginas = pagAtual;
                }

            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public string ToJsonPaginado()
        {

            if (this.Count == 0)
                throw new Exception("Não foram encontrados itens para listar.");

            StringBuilder stb = new StringBuilder();
            stb.Append("{");
            stb.AppendFormat("'Ret' : 1, 'QuantidadePaginas' : {0}, ", this.QuantidadeDePaginas);
            stb.Append("'Series' : [");

            foreach (Serie serie in this)
            {
                stb.Append(JsonConvert.SerializeObject(new
                {

                    SerieID = serie.ID,
                    Imagem = CaminhoImagem + Serie.ToImage(serie.ID),
                    Titulo = serie.Titulo,
                    Nome = serie.Nome,
                    Descricao = serie.Descricao,
                    Pagina = serie.Pagina,
                    Tipo = serie.Tipo
                }, Formatting.Indented) + ",");
            }

            stb.Remove(stb.Length - 1, 1);
            stb.Append("]}");

            return
                JsonConvert.SerializeObject(stb.ToString());
        }

        public SerieLista CarregarPorEventoID(int eventoID)
        {
            oDal = new DAL();
            try
            {
                using (IDataReader dr = oDal.SelectToIDataReader(
                    @"SELECT DISTINCT Serie.IR_SerieID AS ID, Serie.Titulo, Serie.Nome
                        FROM Serie (NOLOCK)
                        INNER JOIN SerieItem (NOLOCK) ON SerieItem.SerieID = Serie.IR_SerieID
                    WHERE EventoID = " + eventoID))
                {
                    while (dr.Read())
                        this.Add(new Serie()
                        {
                            ID = dr["ID"].ToInt32(),
                            Nome = dr["Nome"].ToString(),
                            Titulo = dr["Titulo"].ToString(),
                        });
                }
                return this;
            }
            finally
            {
                oDal.ConnClose();
            }
        }
    }

}
