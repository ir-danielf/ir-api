using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class EntregaAreaBlackList : EntregaAreaBlackList_B
    {

        public EntregaAreaBlackList() { }

        public EntregaAreaBlackList(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

    public class EntregaAreaBlackListLista : EntregaAreaBlackListLista_B
    {
        public bool VerificaExistente(string CepInicio, string CepFim)
        {
            try
            {
                string sql = @"SELECT TOP 1 ID FROM tEntregaAreaBlackList (NOLOCK) WHERE 
                                (CepInicial <= '" + CepInicio + "' and CepFinal >= '" + CepInicio + @"')
                                OR
                                (CepInicial <= '" + CepFim + "' and CepFinal >= '" + CepFim + "')";

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntregaAreaBlackListLista() { }

        public EntregaAreaBlackListLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }
}
