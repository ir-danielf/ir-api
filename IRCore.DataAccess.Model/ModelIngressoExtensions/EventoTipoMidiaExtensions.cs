namespace IRCore.DataAccess.Model
{
    using IRCore.DataAccess.Model.Enumerator;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class EventoTipoMidia
    {

        public enumEventoTipoMidiaTipo TipoAsEnum
        {

            get { return (enumEventoTipoMidiaTipo)Tipo; }
            set { Tipo = ((int)value); }
        }

    }
}