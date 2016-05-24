/**************************************************
* Arquivo: Filme.cs
* Gerado: 08/03/2012
* Autor: Celeritas Ltda
***************************************************/

using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class Filme : Filme_B
    {

        public Filme() { }

        public Filme(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public List<IRLib.Paralela.ClientObjects.EstruturaIDNome> BuscarListaFilmes()
        {
            try
            {
                bd.Consulta("SELECT ID, Nome FROM tFilme (NOLOCK) ORDER BY Nome");
                if (!bd.Consulta().Read())
                    throw new Exception("Não existem filmes para serem visualizados");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome() { ID = bd.LerInt("ID"), Nome = bd.LerString("Nome") });
                } while (bd.Consulta().Read());

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public override bool Atualizar()
        {
            try
            {

                bd.IniciarTransacao();

                bd.Executar("UPDATE tEvento SET Nome = '" + this.Nome.Valor.Replace("'", "''''") + "' WHERE FilmeID = " + this.FilmeID.Valor);

                bool atualizou = base.Atualizar(bd);
                if (!atualizou)
                    throw new Exception("Não foi possível atualizar o filme informado.");

                bd.FinalizarTransacao();

                return atualizou;
            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

    }

    public class FilmeLista : FilmeLista_B
    {

        public FilmeLista() { }

        public FilmeLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
