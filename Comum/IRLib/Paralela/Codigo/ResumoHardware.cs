using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class ResumoHardware : ResumoHardware_B
    {

        public ResumoHardware() { }

        public ResumoHardware(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public bool Validar()
        {

            bool valido = Resolucao.Valor != "" && VersaoOS.Valor != "" && LoginUsuario.Valor != "" && IDMaquina.Valor != "" && VersaoFramework.Valor != "";
            bool existe = true;
            if (valido)
            {

                BD bd = new BD();
                string select = "Select top 1 IDMaquina from tresumohardware where IDMaquina = '{0}'";
                var retorno = bd.ConsultaValor(string.Format(select, this.IDMaquina.Valor));
                if (retorno == null)
                {
                    existe = false;
                }
            }

            return valido && !existe;
        }


    }

    public class ResumoHardwareLista : ResumoHardwareLista_B
    {

        public ResumoHardwareLista() { }

        public ResumoHardwareLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}