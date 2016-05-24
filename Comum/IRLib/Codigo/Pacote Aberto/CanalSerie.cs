/**************************************************
* Arquivo: CanalSerie.cs
* Gerado: 10/01/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects.Serie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IRLib
{

    public class CanalSerie : CanalSerie_B
    {

        public CanalSerie() { }

        public CanalSerie(int usuarioIDLogado) : base(usuarioIDLogado) { }



        public void GerenciarCanais(BD bd, int SerieID, EstruturaCanaisIR estruturaCanaisIR, List<EstruturaCanalSerie> lstCanaisSerie)
        {
            // Distribuir Canais Selecionados
            foreach (var canalSerie in lstCanaisSerie)
            {
                switch (canalSerie.Acao)
                {
                    case Enumerators.TipoAcaoCanal.Associar:
                        this.CanalID.Valor = canalSerie.CanalID;
                        this.SerieID.Valor = SerieID;

                        this.Inserir(bd);
                        break;
                    case Enumerators.TipoAcaoCanal.Remover:
                        if (canalSerie.CanalSerieID == 0)
                            continue;

                        this.Excluir(canalSerie.CanalSerieID);
                        break;
                    case Enumerators.TipoAcaoCanal.Manter:
                        break;
                    default:
                        break;
                }
            }

            ////Distribuir Canais IR
            //if (estruturaCanaisIR.Acao == Enumerators.TipoAcaoCanalIR.Manter)
            //    return;

            BD bdConsulta = new BD();
            bdConsulta.Consulta(string.Format(@"SELECT DISTINCT	
	                                CASE WHEN cs.ID IS NULL
		                                THEN 'F'
		                                ELSE 'T'
		                            END AS Distribuido,
	                                c.ID AS CanalID,
	                                cs.ID AS CanalSerieID
                                FROM tEmpresa e (NOLOCK)
	                            INNER JOIN tCanal c (NOLOCK) ON e.ID = c.EmpresaID
	                            LEFT JOIN tCanalSerie cs (NOLOCK) ON cs.CanalID = c.ID AND cs.SerieID = {0}
                                WHERE e.EmpresaVende = 'T' AND e.EmpresaPromove = 'F'", SerieID));

            int CanalInternet = Canal.CANAL_INTERNET;
            int CanalCC = Canal.CANAL_CALL_CENTER;

            bool achouInternet = false;
            bool achouCC = false;

            List<EstruturaCanalSerieDistribuicao> CanaisIR = new List<EstruturaCanalSerieDistribuicao>();

            int canalID = 0;
            bool distribuido = false;
            while (bdConsulta.Consulta().Read())
            {
                canalID = bdConsulta.LerInt("CanalID");
                distribuido = bdConsulta.LerBoolean("Distribuido");
                if (canalID == CanalCC && distribuido)
                    achouCC = true;

                if (canalID == CanalInternet && distribuido)
                    achouInternet = true;

                CanaisIR.Add(new EstruturaCanalSerieDistribuicao()
                {
                    ID = bdConsulta.LerInt("CanalSerieID"),
                    CanalID = canalID,
                    Distribuido = bdConsulta.LerBoolean("Distribuido"),
                });
            }
            bdConsulta.Fechar();

            switch (estruturaCanaisIR.Acao)
            {
                case Enumerators.TipoAcaoCanalIR.Distribuir:
                    foreach (EstruturaCanalSerieDistribuicao item in CanaisIR.Where(c => !c.Distribuido && c.CanalID != CanalInternet && c.CanalID != CanalCC))
                    {
                        this.CanalID.Valor = item.CanalID;
                        this.SerieID.Valor = SerieID;
                        this.Inserir(bd);
                    }
                    break;
                case Enumerators.TipoAcaoCanalIR.Remover:
                    foreach (EstruturaCanalSerieDistribuicao item in CanaisIR.Where(c => c.Distribuido))
                        this.Excluir(bd, item.ID);
                    break;
                default:
                    break;
            }


            //Quer deixar disponível mas o canal não está disponivel na lista(Antes era False_
            if (estruturaCanaisIR.DisponivelInternet && !achouInternet)
            {
                this.CanalID.Valor = CanalInternet;
                this.SerieID.Valor = SerieID;
                this.Inserir(bd);
            }
            else if (!estruturaCanaisIR.DisponivelInternet && achouInternet)
                this.Excluir(bd, CanaisIR.Where(c=> c.CanalID == CanalInternet).Select(c=> c.ID).FirstOrDefault());


            //Quer deixar disponível mas o canal não está disponivel na lista(Antes era False_
            if (estruturaCanaisIR.DisponivelCallcenter && !achouCC)
            {
                this.CanalID.Valor = CanalCC;
                this.SerieID.Valor = SerieID;
                this.Inserir(bd);
            }
            else if (!estruturaCanaisIR.DisponivelCallcenter && achouCC)
                this.Excluir(bd, CanaisIR.Where(c => c.CanalID == CanalCC).Select(c => c.ID).FirstOrDefault());


        }

        /// <summary>
        /// Inserir novo(a) canalserie
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MAX(ID) AS ID FROM ccanalserie");
            object obj = bd.ConsultaValor(sql);
            int id = (obj != null) ? Convert.ToInt32(obj) : 0;

            this.Control.ID = ++id;
            this.Control.Versao = 0;

            sql = new StringBuilder();
            sql.Append("INSERT INTO tcanalserie(ID, CanalID, SerieID) ");
            sql.Append("VALUES (@ID,@001,@002)");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.CanalID.ValorBD);
            sql.Replace("@002", this.SerieID.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = (x == 1);

            if (result)
                InserirControle("I");

            return result;



        }

        /// <summary>
        /// Exclui canalserie com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int id)
        {
            this.Control.ID = id;

            string sqlSelect = "SELECT MAX(Versao) FROM ccanalserie WHERE ID=" + this.Control.ID;
            object obj = bd.ConsultaValor(sqlSelect);
            int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
            this.Control.Versao = versao;

            InserirControle("D");
            InserirLog();

            string sqlDelete = "DELETE FROM tcanalserie WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = (x == 1);

            return result;
        }
    }

    public class CanalSerieLista : CanalSerieLista_B
    {

        public CanalSerieLista() { }

        public CanalSerieLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
