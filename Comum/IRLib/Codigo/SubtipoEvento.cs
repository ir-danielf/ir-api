/**************************************************
* Arquivo: SubtipoEvento.cs
* Gerado: 06/10/2009
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib
{

    public class SubtipoEvento : SubtipoEvento_B
    {

        public SubtipoEvento() { }

        public SubtipoEvento(int usuarioIDLogado) : base(usuarioIDLogado) { }

        /// <summary>
        /// Obter todos os tipos
        /// </summary>
        /// <returns></returns>
        public override DataTable Todos()
        {

            return null;

        }

        /// <summary>
        /// Obter eventos desse tipo
        /// </summary>
        /// <returns></returns>
        public override DataTable Eventos()
        {

            return null;

        }


    }

    public class SubtipoEventoLista : SubtipoEventoLista_B
    {

        public SubtipoEventoLista() { }

        public SubtipoEventoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
