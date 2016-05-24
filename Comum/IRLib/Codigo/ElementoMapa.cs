/**************************************************
* Arquivo: CotaItemFormaPagamento.cs
* Gerado: 14/01/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IRLib
{

    public class ElementoMapa : ElementoMapa_B
    {

        public ElementoMapa() { }

        public ElementoMapa(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<ElementoMapa> ListarElementos()
        {
            List<ElementoMapa> elementos = new List<ElementoMapa>();
            try
            {
                string sql = "SELECT * FROM ElementoMapa (NOLOCK) where Listar = 1";

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                {
                    ElementoMapa sem = new ElementoMapa();
                    sem.Ler(bd.LerInt("ID"));
                    elementos.Add(sem);
                }

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return elementos;

        }

        public void LerFundo()
        {
            try
            {

                string sql = "SELECT TOP 1 * FROM ElementoMapa WHERE Tipo = 'F'";
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.Nome.ValorBD = bd.LerString("Nome").ToString();
                    this.Valor.ValorBD = bd.LerString("Valor").ToString();
                    this.Listar.ValorBD = bd.LerString("Listar").ToString();
                    this.Tipo.ValorBD = bd.LerString("Tipo").ToString();
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

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

    }

    public class ElementoMapaLista : ElementoMapaLista_B
    {

        public ElementoMapaLista() { }

    }

}
