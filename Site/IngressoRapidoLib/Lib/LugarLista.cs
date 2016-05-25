using CTLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IngressoRapido.Lib
{
    public class LugarLista : List<Lugar>
    {
        public int ApresentacaoID { get; set; }
        public int SetorID { get; set; }
        public IRLib.Setor.enumLugarMarcado LugarMarcado { get; set; }

        private BD bd = new BD();
        private int PacoteID;
        public LugarLista(int apresentacaoID, int setorID, IRLib.Setor.enumLugarMarcado lugarMarcado)
        {
            this.ApresentacaoID = apresentacaoID;
            this.SetorID = setorID;
            this.LugarMarcado = lugarMarcado;
        }

        public LugarLista(int PacoteID, int SetorID)
        {
            // TODO: Complete member initialization
            this.PacoteID = PacoteID;
            this.SetorID = SetorID;
        }
        public LugarLista LoadMapa()
        {
            if (PacoteID > 0)
                this.LoadAssinatura();
            else if (this.LugarMarcado == IRLib.Setor.enumLugarMarcado.Cadeira)
                this.LoadCadeira();
            else if (this.LugarMarcado == IRLib.Setor.enumLugarMarcado.MesaAberta)
                this.LoadMesaAberta();
            else if (this.LugarMarcado == IRLib.Setor.enumLugarMarcado.MesaFechada)
                this.LoadMesaFechada();

            return this;
        }

        private void LoadAssinatura()
        {
            try
            {
                bd.Consulta(string.Format(@"
                    SELECT i.ID AS IngressoID, i.LugarID, i.Status, i.Codigo,
                            l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, IsNull(pl.Descricao, '') AS DescricaoPerspectiva
                    FROM tPacoteItem pi (NOLOCK)
                    INNER JOIN tSetor s (NOLOCK) ON pi.SetorID = s.ID
                    INNER JOIN tIngresso i (NOLOCK) ON i.SetorID = s.ID AND i.ApresentacaoID = pi.ApresentacaoID
                    INNER JOIN tLugar l (NOLOCK) ON l.ID = i.LugarID
                    LEFT JOIN tPerspectivaLugar pl (NOLOCK)ON  pl.ID = l.PerspectivaLugarID
                        WHERE pi.PacoteID = {0} AND i.Codigo <> '' AND s.LugarMarcado = 'C' AND s.AprovadoPublicacao = 'T'
                    ORDER BY l.ID, i.ApresentacaoID
                     ", PacoteID));

                Lugar lugarAux = new Lugar();
                string statusAux = "D";

                while (bd.Consulta().Read())
                {
                    if (this.Where(c => c.ID == bd.LerInt("LugarID")).Count() == 0)
                    {
                        lugarAux = new Lugar();
                        this.Add(lugarAux);
                        statusAux = IRLib.Ingresso.DISPONIVEL;
                        lugarAux.S = IRLib.Ingresso.DISPONIVEL;
                    }


                    lugarAux.ID = bd.LerInt("LugarID");
                    lugarAux.C = bd.LerString("Codigo");
                    lugarAux.X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9);
                    lugarAux.Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9);
                    lugarAux.P = bd.LerInt("PerspectivaLugarID").ToString("000000");
                    lugarAux.Q = 1;

                    //Aqui caso o Status seja diferente de D (Disponivel) atribui como indisponivel
                    if (statusAux != bd.LerString("Status"))
                        //Este status só serve para informar que este ingresso não esta disponível, não existe utilizaçao para I
                        lugarAux.S = "I";

                    lugarAux.D = -1;
                    statusAux = bd.LerString("Status");
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LoadCadeira()
        {
            try
            {
                bd.Consulta(string.Format(@"SELECT i.ID AS IngressoID, i.LugarID, i.Status, i.Codigo,
                                l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, IsNull(pl.Descricao, '') AS DescricaoPerspectiva 
                            FROM tIngresso i (NOLOCK)
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID 	
                            INNER JOIN tLugar l (NOLOCK) ON i.LugarID = l.ID AND l.SetorID = i.SetorID
                            LEFT JOIN tPerspectivaLugar pl (NOLOCK) ON pl.ID = l.PerspectivaLugarID 
                                WHERE i.Codigo <> '' AND ApresentacaoID = {0} AND i.SetorID = {1}  AND s.LugarMarcado = 'C' AND s.AprovadoPublicacao = 'T'
                            ORDER BY i.ID ", this.ApresentacaoID, this.SetorID));

                while (bd.Consulta().Read())
                {
                    this.Add(new Lugar
                    {
                        I = bd.LerInt("IngressoID"),
                        ID = bd.LerInt("LugarID"),
                        C = bd.LerString("Codigo"),
                        X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9),
                        Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9),
                        P = bd.LerInt("PerspectivaLugarID").ToString("000000"),
                        Q = 1,
                        S = bd.LerString("Status"),
                        D = -1,
                    });
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LoadMesaAberta()
        {
            try
            {
                string sql = string.Format(@"SELECT LugarID, SUM(QuantidadeDisponivel) AS QuantidadeDisponivel, Quantidade,
	                                            PosicaoX, PosicaoY, Codigo, PerspectivaLugarID, DescricaoPerspectiva FROM
                                                (SELECT DISTINCT tIngresso.LugarID, tIngresso.ID,
			                                    SUM(CASE tIngresso.Status WHEN 'D' THEN 1 ELSE 0 END) AS QuantidadeDisponivel,
			                                    tLugar.Quantidade, tLugar.PosicaoX, tLugar.PosicaoY, 
                                                CASE tSetor.LugarMarcado
						                            WHEN 'C' THEN tIngresso.Codigo
						                            ELSE substring(tIngresso.Codigo, 1, patindex('%-%',tIngresso.Codigo)-1) END As Codigo,
						                        IsNull(tLugar.PerspectivaLugarID, 0) AS PerspectivaLugarID,
						                        IsNull(tPerspectivaLugar.Descricao, '') AS DescricaoPerspectiva
                                            FROM tIngresso (NOLOCK)
                                            INNER JOIN tLugar (NOLOCK) ON tIngresso.LugarID = tLugar.ID 
                                            INNER JOIN tSetor (NOLOCK) ON tIngresso.SetorID = tSetor.ID AND tLugar.SetorID = tSetor.ID
                                            LEFT JOIN tPerspectivaLugar (NOLOCK) ON tPerspectivaLugar.ID = tLugar.PerspectivaLugarID
                                                WHERE  tIngresso.Codigo <> '' AND tIngresso.ApresentacaoID = {0} AND tSetor.ID = {1} AND tSetor.LugarMarcado = 'A' AND tSetor.AprovadoPublicacao = 'T'
                                            GROUP BY tIngresso.LugarID, tIngresso.ID, tLugar.Quantidade, tLugar.PosicaoX, tLugar.PosicaoY, tSetor.LugarMarcado, tIngresso.Codigo,
                                                tLugar.PerspectivaLugarID, tPerspectivaLugar.Descricao
                                                ) AS a
                                            GROUP BY LugarID, Quantidade, PosicaoX, PosicaoY, Codigo, PerspectivaLugarID, DescricaoPerspectiva", this.ApresentacaoID, this.SetorID);

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    this.Add(new Lugar()
                    {
                        ID = bd.LerInt("LugarID"),
                        C = bd.LerString("Codigo"),
                        X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9),
                        Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9),
                        P = bd.LerInt("PerspectivaLugarID").ToString("000000"),
                        D = bd.LerInt("QuantidadeDisponivel"),
                        Q = bd.LerInt("Quantidade"),
                        S = bd.LerInt("QuantidadeDisponivel") > 0 ? "D" : "V",
                    });
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LoadMesaFechada()
        {
            try
            {
                bd.Consulta(string.Format(
                            @"SELECT COUNT(DISTINCT i.ID) AS Quantidade, i.LugarID, i.Status, substring(i.Codigo,1,patindex('%-%', i.Codigo)-1) AS Codigo,
                                l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, IsNull(pl.Descricao, '') AS DescricaoPerspectiva 
                            FROM tIngresso i (NOLOCK) 
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = i.SetorID
                            INNER JOIN tLugar l (NOLOCK) ON i.LugarID = l.ID AND l.SetorID = i.SetorID 
                            LEFT JOIN tPerspectivaLugar pl (NOLOCK) ON pl.ID = l.PerspectivaLugarID
                                WHERE  i.Codigo <> '' AND ApresentacaoID = {0} AND i.SetorID = {1} AND s.LugarMarcado = 'M' AND s.AprovadoPublicacao = 'T'
                            GROUP BY i.LugarID, i.Status, substring(i.Codigo,1, patindex('%-%',i.Codigo)-1), l.PerspectivaLugarID, l.PosicaoX, l.PosicaoY, pl.Descricao",
                            this.ApresentacaoID, this.SetorID));

                while (bd.Consulta().Read())
                {
                    this.Add(new Lugar
                    {
                        ID = bd.LerInt("LugarID"),
                        C = bd.LerString("Codigo"),
                        X = Convert.ToInt32(bd.LerInt("PosicaoX") * 0.9),
                        Y = Convert.ToInt32(bd.LerInt("PosicaoY") * 0.9),
                        P = bd.LerInt("PerspectivaLugarID").ToString("000000"),
                        Q = bd.LerInt("Quantidade"),
                        S = bd.LerString("Status"),
                        D = -1,
                    });
                }
            }
            finally
            {
                bd.Fechar();
            }
        }

        public string MontarJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
