/**************************************************
* Arquivo: EventoBilheteriaTipos.cs
* Gerado: 06/11/2009
* Autor: Celeritas Ltda
***************************************************/

using System.Data;

namespace IRLib.Paralela
{

    public class EventoBilheteriaTipos : EventoBilheteriaTipos_B
    {

        public EventoBilheteriaTipos() { }

        public EventoBilheteriaTipos(int usuarioIDLogado) : base(usuarioIDLogado) { }

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

    public class EventoBilheteriaTiposLista : EventoBilheteriaTiposLista_B
    {

        public EventoBilheteriaTiposLista() { }

        public EventoBilheteriaTiposLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
