/**************************************************
* Arquivo: RegionalArea.cs
* Gerado: 10/05/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{

    public class RegionalArea : RegionalArea_B
    {

        public RegionalArea() { }

        public RegionalArea(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaRegionalArea> Listar(int RegionalID)
        {
            try
            {
                List<EstruturaRegionalArea> lista = new List<EstruturaRegionalArea>();
                bd.Consulta(
                      string.Format(
                        @"SELECT ea.ID AS EntregaAreaID, IsNull(ra.ID, 0) AS RegionalAreaID,  ea.Nome,
	                        CASE WHEN ra.ID IS NULL
		                        THEN 'F'
		                        ELSE 'T'
	                        END AS Distribuido
	                    FROM tEntregaArea ea (NOLOCK)
	                    LEFT JOIN tRegionalArea ra (NOLOCK) ON ra.AreaID = ea.ID AND ra.RegionalID = {0}
                        ORDER BY ea.Nome
                        ", RegionalID));

                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaRegionalArea()
                    {
                        Distribuido = bd.LerBoolean("Distribuido"),
                        EntregaAreaID = bd.LerInt("EntregaAreaID"),
                        RegionalAreaID = bd.LerInt("RegionalAreaID"),
                        EntregaAreaNome = bd.LerString("Nome"),
                    });
                }

                return lista.ToList();
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void Gerenciar(BD bd, int regionalID, List<EstruturaRegionalArea> lista)
        {
            foreach (var item in lista)
            {
                if (item.Distribuido)
                {
                    if (item.RegionalAreaID > 0)
                        continue;

                    this.Preencher(regionalID, item);
                    this.Inserir(bd);
                }
                else
                {
                    if (item.RegionalAreaID == 0)
                        continue;

                    this.Excluir(bd, item.RegionalAreaID);
                }
            }
        }

        public void Preencher(int regionalID, EstruturaRegionalArea item)
        {
            this.AreaID.Valor = item.EntregaAreaID;
            this.RegionalID.Valor = regionalID;
        }

        public void ExcluirPorRegional(BD bd, int id)
        {
            bd.Executar("DELETE FROM tRegionalArea WHERE RegionalID = " + id);
        }
    }

    public class RegionalAreaLista : RegionalAreaLista_B
    {

        public RegionalAreaLista() { }

        public RegionalAreaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
