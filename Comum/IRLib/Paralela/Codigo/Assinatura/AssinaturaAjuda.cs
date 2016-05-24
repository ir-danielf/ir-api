/**************************************************
* Arquivo: AssinaturaAjuda.cs
* Gerado: 28/09/2011
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;
namespace IRLib.Paralela
{

    public class AssinaturaAjuda : AssinaturaAjuda_B
    {

        public AssinaturaAjuda() { }

        public AssinaturaAjuda(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<EstruturaAssinaturaAjuda> Carregar(int TipoAssinaturaID, string nomePagina)
        {
            try
            {

                List<EstruturaAssinaturaAjuda> lista = new List<EstruturaAssinaturaAjuda>();

                string filtroAux = "";

                filtroAux += "  WHERE tAssinaturaAjuda.AssinaturaTipoID = " + TipoAssinaturaID;

                if (nomePagina.Length > 0)
                {
                    filtroAux += " AND tAssinaturaAjuda.NomePagina like '%" + nomePagina + "%' ";
                }


                string sql = @"SELECT	
                        tAssinaturaAjuda.ID                     
                        ,tAssinaturaAjuda.NomePagina             
                        ,tAssinaturaAjuda.AssinaturaTipoID       
                        ,tAssinaturaTipo.Nome as AssinaturaTipo         
                        ,tAssinaturaAjuda.Conteudo
                        FROM tAssinaturaAjuda (NOLOCK)
                        INNER JOIN tAssinaturaTipo (NOLOCK) ON tAssinaturaAjuda.AssinaturaTipoID = tAssinaturaTipo.ID"
                        + filtroAux
                        + " ORDER BY tAssinaturaAjuda.NomePagina";


                bd.Consulta(sql);


                while (bd.Consulta().Read())
                {

                    lista.Add(new EstruturaAssinaturaAjuda
                    {
                        ID = bd.LerInt("ID"),
                        NomePagina = bd.LerString("NomePagina"),
                        AssinaturaTipoID = bd.LerInt("AssinaturaTipoID"),
                        AssinaturaTipo = bd.LerString("AssinaturaTipo"),
                        Conteudo = bd.LerString("Conteudo"),

                    });

                }

                return lista;

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

        public void Atualizar(EstruturaAssinaturaAjuda eAA)
        {
            this.AtribuirEstrutura(eAA);
            this.Atualizar();
        }

        private void AtribuirEstrutura(EstruturaAssinaturaAjuda eAA)
        {
            this.AssinaturaTipoID.Valor = eAA.AssinaturaTipoID;
            this.NomePagina.Valor = eAA.NomePagina;
            this.Conteudo.Valor = eAA.Conteudo;
            this.Control.ID = eAA.ID;
        }

        public string Buscar(string pagina, int assinaturaTipoID)
        {
            try
            {
                string sql =
                    string.Format(@"
                        SELECT
                            Conteudo
                        FROM tAssinaturaAjuda (NOLOCK)
                        WHERE NomePagina = '{0}' AND AssinaturaTipoID = {1}
                    ", pagina.ToSafeString(), assinaturaTipoID);

                var conteudo = bd.ConsultaValor(sql);

                if (conteudo == null)
                    return string.Empty;

                else return conteudo.ToString();
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaAjudaLista : AssinaturaAjudaLista_B
    {

        public AssinaturaAjudaLista() { }

        public AssinaturaAjudaLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
