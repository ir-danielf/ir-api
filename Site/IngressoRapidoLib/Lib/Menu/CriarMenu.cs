using CTLib;
using IngressoRapido.Lib.Menu;
using IRLib;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace IngressoRapido.Lib
{
    public class CriarMenu
    {
        private List<Categoria> Categorias { get; set; }
        private StringBuilder Conteudo = new StringBuilder();
        private string LocalSalvar = Temporizador.Instancia.Menu.Local.Valor;
        private const string NomeArquivo = "wucMenu.ascx";
        public void Iniciar()
        {
            this.CarregarObjeto();
            this.Montar();
            this.Salvar();
        }

        public void CarregarObjeto()
        {
            Categorias = new List<Categoria>();
            DAL oDal = new DAL();
            try
            {
                List<Genero> generos = new List<Genero>();

                using (IDataReader dr = oDal.SelectToIDataReader(
                    @"
                        SELECT DISTINCT
                            t.IR_TipoID AS CategoriaID, t.Nome AS Categoria,
                            s.IR_SubTipoID AS GeneroID, s.Descricao AS Genero
                        FROM Evento e (NOLOCK)    
                        LEFT JOIN  Tipos tp (NOLOCK) ON e.IR_EventoID = tp.EventoID 
                        INNER JOIN  EventoSubtipo s (NOLOCK) ON s.IR_SubtipoID = e.SubtipoID OR s.IR_SubtipoID = tp.EventoSubtipoID 
                        INNER  JOIN  Tipo t WITH (NOLOCK) ON s.TipoID = t.IR_TipoID
						WHERE t.ID IS NOT NULL AND s.ID IS NOT NULL
                        ORDER BY t.Nome, s.Descricao
                        "))
                {
                    while (dr.Read())
                    {
                        if (Categorias.Where(c => c.ID == dr["CategoriaID"].ToInt32()).Count() == 0)
                        {
                            generos = new List<Genero>();
                            Categorias.Add(new Categoria()
                            {
                                ID = dr["CategoriaID"].ToInt32(),
                                Nome = dr["Categoria"].ToString(),
                                Generos = generos,
                            });
                        }

                        generos.Add(new Genero()
                        {
                            ID = dr["GeneroID"].ToInt32(),
                            Nome = dr["Genero"].ToString(),
                        });

                    }
                }
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        private void Montar()
        {
            this.Cabecalho();
            this.LinhasComuns();
            this.MontarMenu();
            this.FecharLinhasComuns();
        }

        public void Cabecalho()
        {
            Conteudo.Append("<%@ Control Language=\"C#\" AutoEventWireup=\"true\" CodeBehind=\"wucMenu.ascx.cs\" Inherits=\"Site.WUC.wucMenu\" %>");
        }

        public void LinhasComuns()
        {
            Conteudo.Append("<div style=\"padding-left: 0px; padding-top: 0px; text-align: center; border: 0px solid #000\">");
            Conteudo.Append("<ul id=\"nav\" class=\"dropdown dropdown-horizontal\">");
            Conteudo.Append("<li><a href=\"default.aspx\">Home</a></li>");
        }

        public void MontarMenu()
        {
            foreach (var categoria in this.Categorias)
            {
                this.AbrirCategoria(categoria);
                this.AdicionarGeneros(categoria.Generos);
                this.FecharCategoria();
            }
        }

        private void AbrirCategoria(Categoria categoria)
        {
            Conteudo.AppendFormat("<li><a href=\"eventos.aspx?categoria={0}\">{1}</a>", categoria.ID, categoria.Nome);
            Conteudo.Append("<ul style=\"width: 170px; padding-bottom: 10px\">");
        }

        private void AdicionarGeneros(List<Genero> generos)
        {
            foreach (var genero in generos)
                Conteudo.AppendFormat("<li><a href=\"eventos.aspx?genero={0}\">{1}</a></li>", genero.ID, genero.Nome);
        }

        private void FecharCategoria()
        {
            Conteudo.Append("</ul></li>");
        }

        private void FecharLinhasComuns()
        {
            Conteudo.Append("</ul></div>");
        }

        private void Salvar()
        {
            this.ReadOnly(false);

            using (StreamWriter sw =
                new StreamWriter(this.LocalSalvar + NomeArquivo, false, Encoding.UTF8))
                sw.Write(Conteudo.ToString());

            this.ReadOnly(true);
        }

        private void ReadOnly(bool readOnly)
        {
            FileInfo fi = new FileInfo(this.LocalSalvar + NomeArquivo);
            if (!fi.Exists)
                return;

            fi.IsReadOnly = readOnly;
            fi = null;
        }
    }
}
