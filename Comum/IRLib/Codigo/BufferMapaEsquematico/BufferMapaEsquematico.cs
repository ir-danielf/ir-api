using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;

namespace IRLib
{
    [Serializable]
    public class BufferMapaEsquematico : MarshalByRefObject
    {
        /// <summary>
        /// Armazena todas os mapas esquemáticos das apresentacoes disponíveis.
        /// </summary>
        public List<MapaEsquematicoInfo> MapasEsquematicosInfo { get; set; }
        public bool Carregando { get; set; }
        public bool Recarregando { get; set; }
        public int TempoMaximoAtualizacaoQuantidade = Convert.ToInt32(ConfigurationManager.AppSettings["TempoMaximoAtualizacaoQuantidade"]);
        public BufferMapaEsquematico()
        {

        }


        /// <summary>
        /// Carrega/Recarrega o Buffer de mapas esquematicos
        /// </summary>
        public void Carregar()
        {
            BD bd = new BD();

            this.Carregando = true;
            List<MapaEsquematicoInfo> mapasEsquematicosInfoAux = new List<MapaEsquematicoInfo>();
            try
            {
                List<SetorInfo> lstSetores = new List<SetorInfo>();
                bd.Consulta("EXEC sp_LoadMapasEsquematicos " + DateTime.Now.ToString("yyyyMMddHHmmss"));
                List<int> apresentacaoIDaux = new List<int>();
                while (bd.Consulta().Read())
                {
                    if (apresentacaoIDaux.Where(c => c == bd.LerInt("ApresentacaoID")).Count() == 0)
                    {
                        lstSetores = new List<SetorInfo>();
                        mapasEsquematicosInfoAux.Add(new MapaEsquematicoInfo
                                                           {
                                                               ID = bd.LerInt("MapaID"),
                                                               ApresentacaoID = bd.LerInt("ApresentacaoID"),
                                                               Setores = lstSetores,
                                                               UltimaAtualizacao = DateTime.Now,
                                                               UsarMapaEsquematico = bd.LerInt("MapaID") > 0 ? true : false,
                                                           });

                        apresentacaoIDaux.Add(bd.LerInt("ApresentacaoID"));

                    }

                    if (bd.LerInt("MapaEsquematicoSetorID") == 0)
                        mapasEsquematicosInfoAux[mapasEsquematicosInfoAux.Count - 1].UsarMapaEsquematico = false;

                    lstSetores.Add(new SetorInfo
                                       {
                                           ID = bd.LerInt("SetorID"),
                                           Nome = bd.LerString("SetorNome"),
                                           AprovadoPublicacao = bd.LerBoolean("AprovadoPublicacao"),
                                           EnumLugarMarcado = (Setor.enumLugarMarcado)(Convert.ToChar(bd.LerString("LugarMarcado"))),
                                           UltimaAtualizacao = DateTime.Now,
                                           Lotacao = -1,
                                           QuantidadeDisponível = -1,
                                           PrecoPrincipal = bd.LerString("PrecoPrincipal"),
                                           ExibeQuantidade = bd.LerBoolean("ExibeQuantidade"),
                                       });
                }

                bd.Consulta().NextResult();
                while (bd.Consulta().Read())
                {
                    //List<MapaEsquematicoInfo> lst = mapasEsquematicosInfoAux.Where(c => c.ID > 0 && c.ID == bd.LerInt("MapaID")).ToList();
                    foreach (MapaEsquematicoInfo mapaEsquematicoInfo in mapasEsquematicosInfoAux.Where(c => c.ID > 0 && c.ID == bd.LerInt("MapaID")).ToList())
                    {
                        if (mapaEsquematicoInfo.Setores.Where(c => c.ID == bd.LerInt("SetorID")).Count() == 0)
                        {
                            mapaEsquematicoInfo.UsarMapaEsquematico = false;
                            continue;
                        }
                        mapaEsquematicoInfo.Nome = bd.LerString("MapaNome");

                        SetorInfo setor = mapaEsquematicoInfo.Setores.Where(c => c.ID == bd.LerInt("SetorID") && c.Coordenadas.Count == 0).FirstOrDefault();
                        if (setor == null)
                            continue;

                        setor.Coordenadas = BufferMapaEsquematico.GerarPontos(bd.LerString("Coordenadas"));
                    }
                }

                bd.Fechar();
                if (this.MapasEsquematicosInfo != null)
                    lock (this.MapasEsquematicosInfo)
                        this.MapasEsquematicosInfo = mapasEsquematicosInfoAux;
                else
                {
                    this.MapasEsquematicosInfo = new List<MapaEsquematicoInfo>();
                    this.MapasEsquematicosInfo = mapasEsquematicosInfoAux;
                }

                this.CarregarQuantidades();
                this.CarregarQuantidadesBancoIngressos();
            }
            finally
            {
                this.Carregando = false;
                bd.Fechar();
            }
        }

        private void CarregarQuantidadesBancoIngressos()
        {
            BD bd = new BD();
            try
            {
                //bd.BulkInsert(this.MapasEsquematicosInfo.Select(c => c.ApresentacaoID).Distinct().ToList(), "#ApresentacoesMapaEsquematico", false, true);

                //var found = this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == 98446).ToList();

                string sql = @"SELECT
                        bi.ApresentacaoID, i.SetorID, COUNT(DISTINCT bi.ID) AS Quantidade
                    FROM tAssinaturaBancoIngresso bi (NOLOCK)
                    LEFT JOIN tAssinaturaBancoIngressoResgate bir (NOLOCK) ON bir.AssinaturaBancoIngressoID = bi.ID
                    INNER JOIN tIngresso i (NOLOCK) ON i.ID = bi.IngressoID
                    WHERE bi.ClienteID = 0 AND bir.ID IS NULL         
                    GROUP BY bi.ApresentacaoID, SetorID 
                    ORDER BY bi.ApresentacaoID, SetorID";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    try
                    {
                        var apresentacao = this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == bd.LerInt("ApresentacaoID")).FirstOrDefault();
                        if (apresentacao == null)
                            continue;

                        foreach (SetorInfo setor in apresentacao.Setores.Where(c => c.ID == bd.LerInt("SetorID")).ToList())
                            setor.QuantidadeBancoIngresso = bd.LerInt("Quantidade");
                    }
                    catch
                    {
                        continue;
                    }
                }

                //var foundAgain = this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == 98446).ToList();

            }
            finally
            {
                bd.Fechar();
            }
        }

        public void CarregarQuantidades()
        {
            BD bd = new BD();
            try
            {
                using (DataTable dttBulk = new DataTable("Apresentacoes"))
                {
                    dttBulk.Columns.Add("ID", typeof(int));
                    DataRow dtr;
                    foreach (int apresentacaoID in this.MapasEsquematicosInfo.Select(c => c.ApresentacaoID).Distinct())
                    {
                        if (apresentacaoID.Equals(0))
                            continue;
                        dtr = dttBulk.NewRow();
                        dtr["ID"] = apresentacaoID;
                        dttBulk.Rows.Add(dtr);
                    }
                    bd.BulkInsert(dttBulk, "#ApresentacoesMapaEsquematico", false, true);
                }


                bd.Consulta(
                    @"SELECT ApresentacaoID,
                            SetorID,
                            SUM(CASE 
                                WHEN i.Status = 'D'
                                    THEN 1
                                    ELSE 0
		                        END) AS QuantidadeDisponivel,
                            COUNT(i.ID) AS Lotacao 
                    FROM tIngresso i (NOLOCK) 
                    INNER JOIN #ApresentacoesMapaEsquematico ap ON ap.ID = i.ApresentacaoID 
                    GROUP BY ApresentacaoID, SetorID 
                    ORDER BY ApresentacaoID, SetorID");

                while (bd.Consulta().Read())
                {
                    try
                    {
                        foreach (SetorInfo setor in this.MapasEsquematicosInfo.
                            Where(c => c.ApresentacaoID == bd.LerInt("ApresentacaoID"))
                            .FirstOrDefault()
                            .Setores.Where(c => c.ID == bd.LerInt("SetorID")).ToList())
                        {
                            setor.Lotacao = bd.LerInt("Lotacao");
                            setor.QuantidadeDisponível = bd.LerInt("QuantidadeDisponivel");
                            setor.UltimaAtualizacao = DateTime.Now;
                        }
                    }
                    catch (Exception)
                    {
                        //Ignora, Apresentacao está nula
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void AlterarApresentacoes()
        {
            DAL oDal = new DAL();
            try
            {
                DataTable dttBulk = new DataTable();
                DataRow dtr;
                dttBulk.Columns.Add("ID", typeof(int));
                foreach (int apresentacao in this.MapasEsquematicosInfo.Where(c => c.UsarMapaEsquematico).Select(c => c.ApresentacaoID).Distinct().ToList())
                {
                    dtr = dttBulk.NewRow();
                    dtr["ID"] = apresentacao;
                    dttBulk.Rows.Add(dtr);
                }
                oDal.Execute("CREATE Table #tmpAps (ID INT)");
                oDal.BulkInsert(dttBulk, "#tmpAps", false);
                oDal.Execute(
                    @"UPDATE ap SET UsarEsquematico = 1 
                    FROM Apresentacao ap
                    INNER JOIN #tmpAps ON ap.IR_ApresentacaoID = #tmpAps.ID");

                oDal.Execute(
                    @"UPDATE ap SET UsarEsquematico = 0
                    FROM Apresentacao ap
                    LEFT JOIN #tmpAps ON ap.IR_ApresentacaoID = #tmpAps.ID
                    WHERE #tmpAps.ID IS NULL");

            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public static List<Point> GerarPontos(string coords)
        {
            List<Point> ptos = new List<Point>();
            foreach (string coord in coords.Split(';'))
                //foreach (string pt in coord.Split(','))
                ptos.Add(new Point(Convert.ToInt32(coord.Split(',')[0]), Convert.ToInt32(coord.Split(',')[1])));
            return ptos;
        }

        [Obsolete("Criei antes só pra manter a estrutura, acredito q seja necessario atualizar a apresentacao em partes.", true)]
        public void Recarregar(int apresentacaoID)
        {

            while (Carregando || Recarregando)
                continue;

            BD bd = new BD();

            try
            {
                if (this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).Count() > 0)
                    return;

                bd.Consulta(
                        @"SELECT * FROM tApresentacao (NOLOCK");
                bool adicionado = false;

                List<SetorInfo> lstSetores = new List<SetorInfo>();
                while (bd.Consulta().Read())
                {
                    if (adicionado)
                    {
                        lstSetores = new List<SetorInfo>();
                        this.MapasEsquematicosInfo.Add(new MapaEsquematicoInfo
                                                           {
                                                               ID = bd.LerInt("MapaID"),
                                                               Nome = bd.LerString("NOme"),
                                                               ApresentacaoID = bd.LerInt("ApresentacaoID"),
                                                               UltimaAtualizacao = DateTime.Now,
                                                               UsarMapaEsquematico = bd.LerBoolean("UsarMapaEsquematico"),
                                                               Setores = lstSetores,
                                                           });
                        adicionado = true;
                    }

                    lstSetores.Add(new SetorInfo
                                       {
                                           ID = bd.LerInt("SetorID"),
                                           Nome = bd.LerString("SetorNome"),
                                           Coordenadas = new List<Point>(),
                                           Lotacao = bd.LerInt("Lotacao"),
                                           QuantidadeDisponível = bd.LerInt("QuantidadeDisponivel"),
                                           UltimaAtualizacao = DateTime.Now,
                                       });
                }

            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna o Mapa Esquematico atualizado da apresentacao
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public MapaEsquematicoInfo BuscarInformacaoEstrutura(int apresentacaoID)
        {
            MapaEsquematicoInfo ret =
                this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).FirstOrDefault();

            if (ret == null)
                throw new MapaEsquematicoException("Não existe mapa esquemático associado a esta apresentação.");

            return ret;
        }

        public MapaEsquematicoInfo BuscarInformacaoEstruturaNovo(int apresentacaoID, DateTime ultimaAtualizacaoCliente)
        {
            if (this.MapasEsquematicosInfo == null)
                return null;

            MapaEsquematicoInfo ret =
               this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).FirstOrDefault();

            if (ret == null)
                return null;

            if (ultimaAtualizacaoCliente == ret.UltimaAtualizacao)
                return null;

            return ret;
        }


        /// <summary>
        /// Retorna a lista de Setores da apresentacao, as informacoes foram atualizadas
        /// </summary>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public List<SetorInfo> BuscarQuantidades(int apresentacaoID)
        {
            BD bd = new BD();
            try
            {
                MapaEsquematicoInfo ret =
                 this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).FirstOrDefault();

                if (ret == null)
                    throw new MapaEsquematicoException("Não existe mapa esquemático associado a esta apresentação.");

                return ret.Setores.ToList();
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna a necessidade de atualizar a Apresentacao Local
        /// </summary>
        /// <param name="ultimaAtualizacaoCliente"></param>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public bool EstruturaAtualizacao(DateTime ultimaAtualizacaoCliente, int apresentacaoID)
        {
            MapaEsquematicoInfo mapaEsquematicoInfo =
                this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).FirstOrDefault();

            if (mapaEsquematicoInfo == null)
                throw new MapaEsquematicoException("Não existe mapa esquemático associado a esta apresentação.");

            return mapaEsquematicoInfo.UltimaAtualizacao == ultimaAtualizacaoCliente ? true : false;
        }

        /// <summary>
        /// Retorna a necessidade de atualizar as Quantidades dos setores
        /// </summary>
        /// <param name="ultimaAtualizacaoCliente"></param>
        /// <param name="apresentacaoID"></param>
        /// <returns></returns>
        public bool QuantidadesAtualizacao(DateTime ultimaAtualizacaoCliente, int apresentacaoID)
        {
            MapaEsquematicoInfo mapaEsquematicoInfo =
                this.MapasEsquematicosInfo.Where(c => c.ApresentacaoID == apresentacaoID).FirstOrDefault();

            if (mapaEsquematicoInfo == null)
                throw new MapaEsquematicoException("Não existe mapa esquemático associado a esta apresentação.");

            if (mapaEsquematicoInfo.Setores == null || mapaEsquematicoInfo.Setores.Count == 0)
                throw new MapaEsquematicoSetorException("Não foi possível encontrar os setores desta apresentação para exibição do mapa esquemático.");

            return (mapaEsquematicoInfo.Setores[0].UltimaAtualizacao.Subtract(ultimaAtualizacaoCliente).Minutes >= TempoMaximoAtualizacaoQuantidade ? true : false);

        }
    }
}
